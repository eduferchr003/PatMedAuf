using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace PatMedAuf
{
    public partial class Patientensuche : Form
    {
        // ✅ ConnectionString passt du an
        private readonly string _connStr = "SERVER=localhost;DATABASE=patmedauf;UID=root;PWD=cF170104!;";

        private DataTable _dtPatienten = new DataTable();

        public Patientensuche()
        {
            InitializeComponent();

            this.Load += Patientensuche_Load;

            if (txtSuche != null)
                txtSuche.TextChanged += TxtSuche_TextChanged;

//            if (btnAufnahme != null)
//                btnAufnahme.Click += BtnAufnahme_Click;

            if (btnTermin != null)
                btnTermin.Click += BtnTermin_Click;

            if (btnEntlassung != null)
                btnEntlassung.Click += BtnEntlassung_Click;

            if (btnStartseite != null)
                btnStartseite.Click += BtnStartseite_Click;

            if (dgvPatienten != null)
                dgvPatienten.CellDoubleClick += DgvPatienten_CellDoubleClick;
        }

        // =========================
        // LOAD
        // =========================
        private void Patientensuche_Load(object sender, EventArgs e)
        {
            SetupGrid();
            LoadPatienten();
        }

        // =========================
        // GRID SETUP 
        // =========================
        private void SetupGrid()
        {
            if (dgvPatienten == null) return;

            dgvPatienten.AutoGenerateColumns = false;
            dgvPatienten.AllowUserToAddRows = false;
            dgvPatienten.AllowUserToDeleteRows = false;
            dgvPatienten.ReadOnly = true;
            dgvPatienten.MultiSelect = false;
            dgvPatienten.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPatienten.RowHeadersVisible = false;

            dgvPatienten.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPatienten.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvPatienten.DefaultCellStyle.Font = new Font("Segoe UI", 11F);
            dgvPatienten.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            dgvPatienten.Columns.Clear();

            dgvPatienten.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSVNr",
                HeaderText = "SVNr",
                DataPropertyName = "SVNr",
                FillWeight = 25,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvPatienten.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNachname",
                HeaderText = "Nachname",
                DataPropertyName = "Nachname",
                FillWeight = 35
            });

            dgvPatienten.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colVorname",
                HeaderText = "Vorname",
                DataPropertyName = "Vorname",
                FillWeight = 35
            });

            dgvPatienten.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colGeburtsdatum",
                HeaderText = "Geburtsdatum",
                DataPropertyName = "Geburtsdatum",
                FillWeight = 25
            });
        }

        // =========================
        // PATIENTEN LISTE LADEN
        // =========================
        private void LoadPatienten(string filter = "")
        {
            try
            {
                using (var con = new MySqlConnection(_connStr))
                using (var cmd = con.CreateCommand())
                {
                    con.Open();

                    cmd.CommandText = @"
SELECT
    SVNr,
    Nachname,
    Vorname,
    DATE_FORMAT(Geburtsdatum, '%d.%m.%Y') AS Geburtsdatum
FROM patienten
WHERE (@f = '' OR SVNr LIKE CONCAT('%',@f,'%') OR Nachname LIKE CONCAT('%',@f,'%') OR Vorname LIKE CONCAT('%',@f,'%'))
ORDER BY Nachname, Vorname;";

                    cmd.Parameters.AddWithValue("@f", filter ?? "");

                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        _dtPatienten.Clear();
                        da.Fill(_dtPatienten);

                        dgvPatienten.DataSource = _dtPatienten;
                    }
                }

                dgvPatienten.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Patienten:\n" + ex.Message, "DB-Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSuche_TextChanged(object sender, EventArgs e)
        {
            LoadPatienten(txtSuche.Text.Trim());
        }

        // =========================
        // SELECTED SVNR
        // =========================
        private string GetSelectedSVNr()
        {
            if (dgvPatienten == null) return null;
            if (dgvPatienten.SelectedRows.Count == 0) return null;

            return dgvPatienten.SelectedRows[0].Cells["colSVNr"].Value?.ToString();
        }

        // =========================
        // DB: Patient komplett laden
        // =========================
        private DataRow LoadPatientDetails(string svnr)
        {
            if (string.IsNullOrWhiteSpace(svnr)) return null;

            var dt = new DataTable();

            using (var con = new MySqlConnection(_connStr))
            using (var cmd = con.CreateCommand())
            {
                con.Open();

                cmd.CommandText = "SELECT * FROM patienten WHERE SVNr = @svnr LIMIT 1;";
                cmd.Parameters.AddWithValue("@svnr", svnr);

                using (var da = new MySqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            if (dt.Rows.Count == 0) return null;
            return dt.Rows[0];
        }

        // =========================
        // BUTTON: Patientenaufnahme öffnen + füllen
        // =========================
        private void BtnAufnahme_Click(object sender, EventArgs e)
        {
            string svnr = GetSelectedSVNr();
            if (string.IsNullOrWhiteSpace(svnr))
            {
                MessageBox.Show("Bitte zuerst einen Patienten auswählen.", "Hinweis",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRow row = LoadPatientDetails(svnr);
            if (row == null)
            {
                MessageBox.Show("Patient wurde nicht gefunden.", "Hinweis",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var f = new Patientenaufnahme(); 
            FillPatientenaufnahmeForm(f, row);
            f.Show();
            this.Hide();
        }

        // =========================
        // BUTTON: Termin öffnen + füllen
        // =========================
        private void BtnTermin_Click(object sender, EventArgs e)
        {
            string svnr = GetSelectedSVNr();
            if (string.IsNullOrWhiteSpace(svnr))
            {
                MessageBox.Show("Bitte zuerst einen Patienten auswählen.", "Hinweis",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRow row = LoadPatientDetails(svnr);
            if (row == null)
            {
                MessageBox.Show("Patient wurde nicht gefunden.", "Hinweis",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var t = new Termin(); 
            FillTerminForm(t, row);
            t.Show();
            this.Hide();
        }

        // Doppelklick = Aufnahme öffnen
        private void DgvPatienten_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                BtnAufnahme_Click(sender, EventArgs.Empty);
        }

        // =========================
        // BUTTON: Entlassung => HL7 v3 erzeugen + speichern
        // =========================
        private void BtnEntlassung_Click(object sender, EventArgs e)
        {
            string svnr = GetSelectedSVNr();
            if (string.IsNullOrWhiteSpace(svnr))
            {
                MessageBox.Show("Bitte zuerst einen Patienten auswählen.", "Hinweis",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRow row = LoadPatientDetails(svnr);
            if (row == null)
            {
                MessageBox.Show("Patient wurde nicht gefunden.", "Hinweis",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string xml = GenerateHL7v3DischargeXml(row);

                using (var sfd = new SaveFileDialog())
                {
                    sfd.Title = "HL7 v3 Entlassungsnachricht speichern";
                    sfd.Filter = "HL7 v3 XML (*.xml)|*.xml|Alle Dateien (*.*)|*.*";
                    sfd.FileName = $"HL7v3_Entlassung_{svnr}_{DateTime.Now:yyyyMMdd_HHmmss}.xml";

                    if (sfd.ShowDialog() != DialogResult.OK)
                        return;

                    File.WriteAllText(sfd.FileName, xml, Encoding.UTF8);
                }

                MessageBox.Show("HL7 v3 Datei wurde erstellt und gespeichert.", "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Erzeugen der HL7 Nachricht:\n" + ex.Message, "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================
        // BUTTON: Startseite
        // =========================
        private void BtnStartseite_Click(object sender, EventArgs e)
        {
            var s = new Startseite();
            s.Show();
            this.Hide();
        }

        // ============================================================
        // FORM FÜLLEN: Patientenaufnahme
        // ============================================================
        private void FillPatientenaufnahmeForm(Form f, DataRow row)
        {
            SetControlValue(f, "txtSVNr", row["SVNr"]);
            SetControlValue(f, "txtVorname", row["Vorname"]);
            SetControlValue(f, "txtZweitname", row.Table.Columns.Contains("Zweitname") ? row["Zweitname"] : null);
            SetControlValue(f, "txtNachname", row["Nachname"]);

            SetControlValue(f, "cmbAnrede", row.Table.Columns.Contains("Anrede") ? row["Anrede"] : null);
            SetControlValue(f, "cmbTitel", row.Table.Columns.Contains("Titel") ? row["Titel"] : null);
            SetControlValue(f, "cmbGeschlecht", row.Table.Columns.Contains("Geschlecht") ? row["Geschlecht"] : null);
            SetControlValue(f, "dtpGeburtsdatum", row.Table.Columns.Contains("Geburtsdatum") ? row["Geburtsdatum"] : null);

            SetControlValue(f, "cmbGeburtsland", row.Table.Columns.Contains("Geburtsland") ? row["Geburtsland"] : null);
            SetControlValue(f, "cmbReligion", row.Table.Columns.Contains("Religion") ? row["Religion"] : null);
            SetControlValue(f, "cmbStaat", row.Table.Columns.Contains("Staat") ? row["Staat"] : null);

            SetControlValue(f, "txtEmail", row.Table.Columns.Contains("Email") ? row["Email"] : null);
            SetControlValue(f, "txtTelefonnummer", row.Table.Columns.Contains("Telefonnummer") ? row["Telefonnummer"] : null);
            SetControlValue(f, "txtStrasse", row.Table.Columns.Contains("Strasse") ? row["Strasse"] : null);
            SetControlValue(f, "txtNr", row.Table.Columns.Contains("Nr") ? row["Nr"] : null);
            SetControlValue(f, "txtPLZ", row.Table.Columns.Contains("PLZ") ? row["PLZ"] : null);
            SetControlValue(f, "txtOrt", row.Table.Columns.Contains("Ort") ? row["Ort"] : null);

            SetControlValue(f, "txtKoerper", row.Table.Columns.Contains("Koerper") ? row["Koerper"] : null);
            SetControlValue(f, "txtKoerpergroesse", row.Table.Columns.Contains("Koerpergroesse") ? row["Koerpergroesse"] : null);
            SetControlValue(f, "txtGewicht", row.Table.Columns.Contains("Gewicht") ? row["Gewicht"] : null);

            SetControlValue(f, "chkAllergien", row.Table.Columns.Contains("Allergien") ? row["Allergien"] : null);
            SetControlValue(f, "txtAllergienWelche", row.Table.Columns.Contains("AllergienWelche") ? row["AllergienWelche"] : null);

            SetControlValue(f, "chkMedikamente", row.Table.Columns.Contains("Medikamente") ? row["Medikamente"] : null);
            SetControlValue(f, "txtMedikamenteWelche", row.Table.Columns.Contains("MedikamenteWelche") ? row["MedikamenteWelche"] : null);

            SetControlValue(f, "chkVorerkrankung", row.Table.Columns.Contains("Vorerkrankung") ? row["Vorerkrankung"] : null);
            SetControlValue(f, "txtVorerkrankungWelche", row.Table.Columns.Contains("VorerkrankungWelche") ? row["VorerkrankungWelche"] : null);

            SetControlValue(f, "txtDiagnosen", row.Table.Columns.Contains("Diagnosen") ? row["Diagnosen"] : null);
            SetControlValue(f, "cmbICD10", row.Table.Columns.Contains("ICD10") ? row["ICD10"] : null);
            SetControlValue(f, "cmbICF", row.Table.Columns.Contains("ICF") ? row["ICF"] : null);

            SetControlValue(f, "txtZusatz", row.Table.Columns.Contains("Zusatz") ? row["Zusatz"] : null);
        }

        // ============================================================
        // FORM FÜLLEN: Termin
        // ============================================================
        private void FillTerminForm(Form f, DataRow row)
        {
            SetControlValue(f, "txtSVNr", row["SVNr"]);

            // Versicherungsbereich 
            SetControlValue(f, "cmbVersicherung", row.Table.Columns.Contains("Versicherung") ? row["Versicherung"] : null);
            SetControlValue(f, "cmbZusatzversicherung", row.Table.Columns.Contains("Zusatzversicherung") ? row["Zusatzversicherung"] : null);
            SetControlValue(f, "ckbMitJa", row.Table.Columns.Contains("Mitversichert") ? row["Mitversichert"] : null);

            // Termin
            SetControlValue(f, "dtpTerminDatum", row.Table.Columns.Contains("Termin_Datum") ? row["Termin_Datum"] : null);
            SetControlValue(f, "dtpUhrzeit", row.Table.Columns.Contains("Termin_Uhrzeit") ? row["Termin_Uhrzeit"] : null);
            SetControlValue(f, "txtGrund", row.Table.Columns.Contains("GrundDesBesuches") ? row["GrundDesBesuches"] : null);
            SetControlValue(f, "cmbDringlichkeit", row.Table.Columns.Contains("Dringlichkeit") ? row["Dringlichkeit"] : null);

            SetControlValue(f, "ckbEmail", row.Table.Columns.Contains("Erinnerung_Email") ? row["Erinnerung_Email"] : null);
            SetControlValue(f, "ckbSMS", row.Table.Columns.Contains("Erinnerung_SMS") ? row["Erinnerung_SMS"] : null);
        }

        // ============================================================
        // HL7 v3 (XML) GENERATOR - Entlassung
        // ============================================================
        private string GenerateHL7v3DischargeXml(DataRow row)
        {
            // Minimal HL7 v3 / CDA-ähnliches XML (für Abgabe/Projekt ok)
            // enthält Patient + Termin + Diagnosen
            string svnr = SafeStr(row, "SVNr");
            string vorname = SafeStr(row, "Vorname");
            string nachname = SafeStr(row, "Nachname");
            string gebDatum = SafeDate(row, "Geburtsdatum", "yyyy-MM-dd");

            string geschlecht = SafeStr(row, "Geschlecht");
            string icd10 = SafeStr(row, "ICD10");
            string icf = SafeStr(row, "ICF");
            string diagnosen = SafeStr(row, "Diagnosen");
            string grund = SafeStr(row, "GrundDesBesuches");
            string dring = SafeStr(row, "Dringlichkeit");

            string terminDatum = SafeDate(row, "Termin_Datum", "yyyy-MM-dd");
            string terminUhr = SafeTime(row, "Termin_Uhrzeit", "HH:mm:ss");

            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false
            };

            var sb = new StringBuilder();
            using (var xw = XmlWriter.Create(sb, settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("ClinicalDocument", "urn:hl7-org:v3");

                xw.WriteElementString("id", Guid.NewGuid().ToString());
                xw.WriteElementString("typeId", "DISCHARGE");

                xw.WriteStartElement("effectiveTime");
                xw.WriteAttributeString("value", DateTime.Now.ToString("yyyyMMddHHmmss"));
                xw.WriteEndElement();

                xw.WriteStartElement("recordTarget");
                xw.WriteStartElement("patientRole");

                xw.WriteElementString("id", svnr);

                xw.WriteStartElement("patient");
                xw.WriteStartElement("name");
                xw.WriteElementString("given", vorname);
                xw.WriteElementString("family", nachname);
                xw.WriteEndElement(); // name

                xw.WriteStartElement("administrativeGenderCode");
                xw.WriteAttributeString("code", geschlecht);
                xw.WriteEndElement();

                xw.WriteStartElement("birthTime");
                xw.WriteAttributeString("value", gebDatum.Replace("-", ""));
                xw.WriteEndElement();

                xw.WriteEndElement(); // patient
                xw.WriteEndElement(); // patientRole
                xw.WriteEndElement(); // recordTarget

                // Encounter / Termin
                xw.WriteStartElement("component");
                xw.WriteStartElement("structuredBody");
                xw.WriteStartElement("component");
                xw.WriteStartElement("section");

                xw.WriteElementString("title", "Entlassung");

                xw.WriteStartElement("text");
                xw.WriteElementString("paragraph", $"Patient {vorname} {nachname} (SVNr: {svnr}) wurde entlassen.");
                xw.WriteElementString("paragraph", $"Letzter Termin: {terminDatum} {terminUhr}");
                xw.WriteElementString("paragraph", $"Grund: {grund}");
                xw.WriteElementString("paragraph", $"Dringlichkeit: {dring}");
                xw.WriteEndElement(); // text

                // Diagnosen
                xw.WriteStartElement("entry");
                xw.WriteStartElement("observation");
                xw.WriteAttributeString("classCode", "OBS");
                xw.WriteAttributeString("moodCode", "EVN");

                xw.WriteElementString("code", "DIAGNOSIS");
                xw.WriteElementString("value", diagnosen);

                xw.WriteStartElement("reference");
                xw.WriteElementString("ICD10", icd10);
                xw.WriteElementString("ICF", icf);
                xw.WriteEndElement();

                xw.WriteEndElement(); // observation
                xw.WriteEndElement(); // entry

                xw.WriteEndElement(); // section
                xw.WriteEndElement(); // component
                xw.WriteEndElement(); // structuredBody
                xw.WriteEndElement(); // component

                xw.WriteEndElement(); // ClinicalDocument
                xw.WriteEndDocument();
            }

            return sb.ToString();
        }

        // ============================================================
        // HELPER: Controls per Name setzen 
        // ============================================================
        private void SetControlValue(Form f, string controlName, object value)
        {
            if (f == null || string.IsNullOrWhiteSpace(controlName)) return;

            Control c = FindControlRecursive(f, controlName);
            if (c == null) return;

            if (c is TextBox tb)
            {
                tb.Text = value == null || value == DBNull.Value ? "" : value.ToString();
                return;
            }

            if (c is ComboBox cb)
            {
                string v = value == null || value == DBNull.Value ? "" : value.ToString();
                if (!string.IsNullOrWhiteSpace(v))
                {
                    cb.SelectedItem = v;
                    if (cb.SelectedIndex < 0)
                        cb.Text = v; // falls nicht in Liste
                }
                else
                {
                    cb.SelectedIndex = -1;
                    cb.Text = "";
                }
                return;
            }

            if (c is CheckBox chk)
            {
                // akzeptiert 0/1, true/false
                bool b = false;
                if (value != null && value != DBNull.Value)
                {
                    if (value is bool bb) b = bb;
                    else
                    {
                        string s = value.ToString().Trim().ToLower();
                        b = (s == "1" || s == "true" || s == "ja" || s == "yes");
                    }
                }
                chk.Checked = b;
                return;
            }

            if (c is DateTimePicker dtp)
            {
                if (value == null || value == DBNull.Value)
                {
                    return;
                }

                if (DateTime.TryParse(value.ToString(), out DateTime d))
                    dtp.Value = d;

                return;
            }
        }

        private Control FindControlRecursive(Control parent, string name)
        {
            if (parent == null) return null;

            foreach (Control c in parent.Controls)
            {
                if (c.Name == name)
                    return c;

                Control child = FindControlRecursive(c, name);
                if (child != null)
                    return child;
            }
            return null;
        }

        // ============================================================
        // SAFE READS
        // ============================================================
        private string SafeStr(DataRow row, string col)
        {
            if (row == null) return "";
            if (!row.Table.Columns.Contains(col)) return "";
            var v = row[col];
            if (v == null || v == DBNull.Value) return "";
            return SecurityElement.Escape(v.ToString()) ?? "";
        }

        private string SafeDate(DataRow row, string col, string format)
        {
            if (row == null) return "";
            if (!row.Table.Columns.Contains(col)) return "";
            var v = row[col];
            if (v == null || v == DBNull.Value) return "";

            if (DateTime.TryParse(v.ToString(), out DateTime d))
                return d.ToString(format);

            return "";
        }

        private string SafeTime(DataRow row, string col, string format)
        {
            if (row == null) return "";
            if (!row.Table.Columns.Contains(col)) return "";
            var v = row[col];
            if (v == null || v == DBNull.Value) return "";

            // TIME kann als TimeSpan kommen
            if (v is TimeSpan ts)
                return DateTime.Today.Add(ts).ToString(format);

            if (DateTime.TryParse(v.ToString(), out DateTime d))
                return d.ToString(format);

            return "";
        }
    }
}