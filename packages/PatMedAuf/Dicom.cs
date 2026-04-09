using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Dicom;
using Dicom.Imaging;
//using FellowOakDicom;
//using FellowOakDicom.Imaging;
using MySql.Data.MySqlClient;

namespace PatMedAuf
{
    public partial class DicomForm : Form
    {
        private readonly string _connStr =
            "server=localhost;database=patmedauf;uid=root;pwd=cF170104!;SslMode=Disabled;";

        // ========= Zoom / Anzeige =========
        private Panel _pnlZoom;
        private Bitmap _originalBitmap;
        private float _zoomFactor = 1.0f;

        // ========= DICOM-Session =========
        private readonly List<string> _currentFilePaths = new List<string>();   // aktuell geladene Dateien (vom Öffnen / DB Laden)
        private readonly List<DicomDataset> _currentDatasets = new List<DicomDataset>();
        private int _viewCount = 1;


        public DicomForm()
        {
            InitializeComponent();


            WrapPictureBoxForZoom();

            pbDicom.BackColor = Color.Black;
            pbDicom.SizeMode = PictureBoxSizeMode.StretchImage; 

            InitTagsGrid();
            InitTrackbarZoom();

            btnOeffnen.Click += btnOeffnen_Click;
            btnSpeichernDB.Click += btnSpeichernAufDB_Click;
            btnDateienLaden.Click += btnDateienLaden_Click;

            btnView1.Click += (s, e) => { _viewCount = 1; RenderCurrent(); };
            btnView2.Click += (s, e) => { _viewCount = 2; RenderCurrent(); };
            btnView4.Click += (s, e) => { _viewCount = 4; RenderCurrent(); };

            btnStartseite.Click += btnStartseite_Click;

            this.Shown += (s, e) =>
            {
                if (_originalBitmap != null) ZoomFit();
            };
        }

        private void WrapPictureBoxForZoom()
        {
            if (_pnlZoom != null) return;

            _pnlZoom = new Panel();
            _pnlZoom.Left = pbDicom.Left;
            _pnlZoom.Top = pbDicom.Top;
            _pnlZoom.Width = pbDicom.Width;
            _pnlZoom.Height = pbDicom.Height;

            _pnlZoom.Anchor = pbDicom.Anchor;

            _pnlZoom.AutoScroll = true;
            _pnlZoom.BackColor = pbDicom.BackColor;

            var parent = pbDicom.Parent;
            parent.Controls.Add(_pnlZoom);
            _pnlZoom.BringToFront();

            pbDicom.Parent = _pnlZoom;
            pbDicom.Left = 0;
            pbDicom.Top = 0;
        }

        private void InitTagsGrid()
        {
            dgvTags.AutoGenerateColumns = false;
            dgvTags.AllowUserToAddRows = false;
            dgvTags.AllowUserToDeleteRows = false;
            dgvTags.ReadOnly = true;
            dgvTags.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTags.MultiSelect = false;

            dgvTags.Columns.Clear();

            dgvTags.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTag",
                HeaderText = "Tag",
                DataPropertyName = "Tag",
                Width = 90
            });

            dgvTags.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = "Name",
                DataPropertyName = "Name",
                Width = 180
            });

            dgvTags.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colValue",
                HeaderText = "Value",
                DataPropertyName = "Value",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        private void InitTrackbarZoom()
        {
            // 10% bis 400%
            trkZoom.Minimum = 10;
            trkZoom.Maximum = 400;
            trkZoom.TickFrequency = 10;
            trkZoom.SmallChange = 5;
            trkZoom.LargeChange = 25;

            trkZoom.Value = 100;
            trkZoom.ValueChanged += trkZoom_ValueChanged;
        }

        private void trkZoom_ValueChanged(object sender, EventArgs e)
        {
            if (_originalBitmap == null) return;
            float factor = trkZoom.Value / 100f;
            ZoomSet(factor);
        }

        private void btnOeffnen_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = "DICOM öffnen";
                    dlg.Filter = "DICOM (*.dcm)|*.dcm|Alle Dateien (*.*)|*.*";
                    dlg.Multiselect = true;

                    if (dlg.ShowDialog() != DialogResult.OK)
                        return;

                    LoadDicomFiles(dlg.FileNames.ToList());
                    _viewCount = 1;
                    RenderCurrent();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Fehler beim Öffnen:\n" + ex.Message,
                    "Fehler",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void LoadDicomFiles(List<string> paths)
        {
            _currentFilePaths.Clear();
            _currentDatasets.Clear();

            foreach (var p in paths)
            {
                if (!File.Exists(p)) continue;

                // DicomFile öffnen
                var dcm = DicomFile.Open(p);
                _currentFilePaths.Add(p);
                _currentDatasets.Add(dcm.Dataset);
            }

            if (_currentDatasets.Count == 0)
                throw new Exception("Keine gültigen DICOM-Dateien gefunden.");
        }

        private void RenderCurrent()
        {
            if (_currentFilePaths.Count == 0)
            {
                ClearView();
                return;
            }

            try
            {
                int n = Math.Min(_viewCount, _currentFilePaths.Count);

                // n Bilder rendern
                var bitmaps = new List<Bitmap>();
                for (int i = 0; i < n; i++)
                {
                    var bmp = RenderDicomToBitmap(_currentFilePaths[i]);
                    bitmaps.Add(bmp);
                }

                // zusammensetzen (1 / 2 / 4)
                Bitmap composed = Compose(bitmaps, n);

                // Original setzen (für Trackbar Zoom)
                SetRenderedBitmap(composed);

                // Tags aus erstem Dataset anzeigen
                BindTags(_currentDatasets[0]);

                // Ressourcen der Einzelbitmaps entsorgen (Compose hat kopiert)
                foreach (var b in bitmaps) b.Dispose();
                composed.Dispose(); // SetRenderedBitmap cloned intern

            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Rendern:\n" + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Bitmap RenderDicomToBitmap(string path)
        {
            // fo-dicom Render
            var dcm = DicomFile.Open(path);
            var img = new DicomImage(dcm.Dataset);

            using (var rendered = img.RenderImage()) // Image
            {
                // AsClonedBitmap ist zuverlässig
                return rendered.AsClonedBitmap();
            }
        }

        private Bitmap Compose(List<Bitmap> bitmaps, int n)
        {
            if (n <= 1)
            {
                // 1:1 Clone
                return new Bitmap(bitmaps[0]);
            }

            if (n == 2)
            {
                // 2 nebeneinander
                int w = bitmaps[0].Width + bitmaps[1].Width;
                int h = Math.Max(bitmaps[0].Height, bitmaps[1].Height);

                var canvas = new Bitmap(w, h, PixelFormat.Format24bppRgb);
                using (var g = Graphics.FromImage(canvas))
                {
                    g.Clear(Color.Black);
                    g.DrawImage(bitmaps[0], new Rectangle(0, 0, bitmaps[0].Width, bitmaps[0].Height));
                    g.DrawImage(bitmaps[1], new Rectangle(bitmaps[0].Width, 0, bitmaps[1].Width, bitmaps[1].Height));
                }
                return canvas;
            }

            // 4 (2x2) – falls weniger als 4 vorhanden, nimmt nur vorhandene
            int count = Math.Min(4, bitmaps.Count);

            int cellW = bitmaps.Take(count).Max(b => b.Width);
            int cellH = bitmaps.Take(count).Max(b => b.Height);

            int wAll = cellW * 2;
            int hAll = cellH * 2;

            var canvas4 = new Bitmap(wAll, hAll, PixelFormat.Format24bppRgb);
            using (var g = Graphics.FromImage(canvas4))
            {
                g.Clear(Color.Black);

                for (int i = 0; i < count; i++)
                {
                    int col = i % 2;
                    int row = i / 2;

                    int x = col * cellW;
                    int y = row * cellH;

                    // zentriert in Zelle
                    int xOff = x + (cellW - bitmaps[i].Width) / 2;
                    int yOff = y + (cellH - bitmaps[i].Height) / 2;

                    g.DrawImage(bitmaps[i], new Rectangle(xOff, yOff, bitmaps[i].Width, bitmaps[i].Height));
                }
            }
            return canvas4;
        }

        private void SetRenderedBitmap(Bitmap bmp)
        {
            // Original ersetzen
            if (_originalBitmap != null)
            {
                _originalBitmap.Dispose();
                _originalBitmap = null;
            }

            _originalBitmap = new Bitmap(bmp); // Clone als Original

            // Fit starten -> Trackbar passt sich an
            ZoomFit();
        }

        private void ClearView()
        {
            dgvTags.DataSource = null;

            if (_originalBitmap != null)
            {
                _originalBitmap.Dispose();
                _originalBitmap = null;
            }

            pbDicom.Image = null;
            pbDicom.Width = Math.Max(1, _pnlZoom.ClientSize.Width);
            pbDicom.Height = Math.Max(1, _pnlZoom.ClientSize.Height);
            pbDicom.Left = 0;
            pbDicom.Top = 0;
        }


        private void ZoomSet(float factor)
        {
            if (_originalBitmap == null) return;

            _zoomFactor = Math.Max(0.1f, Math.Min(8.0f, factor));

            int w = (int)(_originalBitmap.Width * _zoomFactor);
            int h = (int)(_originalBitmap.Height * _zoomFactor);

            pbDicom.Image = _originalBitmap;     
            pbDicom.Width = Math.Max(1, w);
            pbDicom.Height = Math.Max(1, h);
            pbDicom.SizeMode = PictureBoxSizeMode.StretchImage;

            if (pbDicom.Width < _pnlZoom.ClientSize.Width)
                pbDicom.Left = (_pnlZoom.ClientSize.Width - pbDicom.Width) / 2;
            else
                pbDicom.Left = 0;

            if (pbDicom.Height < _pnlZoom.ClientSize.Height)
                pbDicom.Top = (_pnlZoom.ClientSize.Height - pbDicom.Height) / 2;
            else
                pbDicom.Top = 0;
        }

        private void ZoomFit()
        {
            if (_originalBitmap == null) return;

            float fx = (float)_pnlZoom.ClientSize.Width / _originalBitmap.Width;
            float fy = (float)_pnlZoom.ClientSize.Height / _originalBitmap.Height;

            float fit = Math.Min(fx, fy);
            fit = Math.Max(0.1f, Math.Min(4.0f, fit));

            int percent = (int)Math.Round(fit * 100);
            percent = Math.Max(trkZoom.Minimum, Math.Min(trkZoom.Maximum, percent));

            // Trackbar setzen 
            trkZoom.Value = percent;
        }


        private class TagRow
        {
            public string Tag { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private void BindTags(DicomDataset ds)
        {
            try
            {
                var rows = new List<TagRow>();

                foreach (var item in ds)
                {
                    var tag = item.Tag.ToString(); 
                    string name = DicomDictionary.Default[item.Tag]?.Name ?? item.Tag.DictionaryEntry?.Name ?? "";
                    string value = SafeDicomValue(ds, item.Tag);

                    rows.Add(new TagRow
                    {
                        Tag = tag,
                        Name = name,
                        Value = value
                    });
                }

                dgvTags.DataSource = rows;
            }
            catch
            {
                dgvTags.DataSource = null;
            }
        }

        private string SafeDicomValue(DicomDataset ds, DicomTag tag)
        {
            try
            {
                if (!ds.Contains(tag)) return "";

                var s = ds.GetSingleValueOrDefault(tag, string.Empty);
                if (!string.IsNullOrWhiteSpace(s)) return s;

                var arr = ds.GetValues<string>(tag);
                if (arr != null && arr.Length > 0) return string.Join(" | ", arr);

                return "";
            }
            catch
            {
                try
                {
                    var el = ds.GetDicomItem<DicomItem>(tag);
                    return el?.ToString() ?? "";
                }
                catch
                {
                    return "";
                }
            }
        }


        private void btnSpeichernAufDB_Click(object sender, EventArgs e)
        {
            try
            {
                string svnr = (txtSVNr.Text ?? "").Trim();
                if (string.IsNullOrWhiteSpace(svnr))
                {
                    MessageBox.Show("Bitte SVNr eingeben.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_currentFilePaths.Count == 0 || _currentDatasets.Count == 0)
                {
                    MessageBox.Show("Keine DICOM-Dateien geladen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Zielordner
                string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DicomStore", svnr);
                Directory.CreateDirectory(baseDir);

                using (var con = new MySqlConnection(_connStr))
                {
                    con.Open();

                    for (int i = 0; i < _currentFilePaths.Count; i++)
                    {
                        string src = _currentFilePaths[i];
                        var ds = _currentDatasets[i];

                        // Kopie mit eindeutigen Namen
                        string destName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.dcm";
                        string dest = Path.Combine(baseDir, destName);
                        File.Copy(src, dest, true);

                        string sop = ds.GetSingleValueOrDefault(DicomTag.SOPInstanceUID, "");
                        string study = ds.GetSingleValueOrDefault(DicomTag.StudyInstanceUID, "");
                        string series = ds.GetSingleValueOrDefault(DicomTag.SeriesInstanceUID, "");
                        string modality = ds.GetSingleValueOrDefault(DicomTag.Modality, "");
                        string studyDate = ds.GetSingleValueOrDefault(DicomTag.StudyDate, ""); // YYYYMMDD

                        string sql = @"
INSERT INTO patienten_dicom
(SVNr, FilePath, SopInstanceUid, StudyInstanceUid, SeriesInstanceUid, Modality, StudyDate)
VALUES
(@svnr, @path, @sop, @study, @series, @modality, @studyDate);";

                        using (var cmd = new MySqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@svnr", svnr);
                            cmd.Parameters.AddWithValue("@path", dest);
                            cmd.Parameters.AddWithValue("@sop", sop);
                            cmd.Parameters.AddWithValue("@study", study);
                            cmd.Parameters.AddWithValue("@series", series);
                            cmd.Parameters.AddWithValue("@modality", modality);
                            cmd.Parameters.AddWithValue("@studyDate", studyDate);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("DICOM-Datei(en) wurden gespeichert (Pfad + Metadaten).", "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("DB-Fehler:\n" + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler:\n" + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDateienLaden_Click(object sender, EventArgs e)
        {
            try
            {
                string svnr = (txtSVNr.Text ?? "").Trim();
                if (string.IsNullOrWhiteSpace(svnr))
                {
                    MessageBox.Show("Bitte SVNr eingeben.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var paths = new List<string>();

                using (var con = new MySqlConnection(_connStr))
                {
                    con.Open();

                    string sql = @"SELECT FilePath FROM patient_dicom WHERE SVNr=@svnr ORDER BY CreatedAt DESC;";
                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@svnr", svnr);

                        using (var r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                string p = r.GetString(0);
                                if (File.Exists(p)) paths.Add(p);
                            }
                        }
                    }
                }

                if (paths.Count == 0)
                {
                    MessageBox.Show("Keine DICOM-Dateien in der DB gefunden (oder Pfade existieren nicht).",
                        "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearView();
                    return;
                }

                LoadDicomFiles(paths);
                _viewCount = 1;
                RenderCurrent();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("DB-Fehler:\n" + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler:\n" + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnStartseite_Click(object sender, EventArgs e)
        {
            try
            {
                var start = new Startseite();
                start.Show();
                this.Hide();
            }
            catch
            {
                this.Close();
            }
        }


        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                if (_originalBitmap != null)
                {
                    _originalBitmap.Dispose();
                    _originalBitmap = null;
                }
            }
            catch { }

            base.OnFormClosed(e);
        }

        private void DicomForm_Load(object sender, EventArgs e)
        {

        }
    }
}