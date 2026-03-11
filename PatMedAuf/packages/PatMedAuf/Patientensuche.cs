using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PatMedAuf
{
    public partial class Patientensuche : Form
    {
        
        private readonly string _connStr =
            "Server=localhost;Database=patmedauf;Uid=root;Pwd=cF170104!;SslMode=none;";

        private DataTable _table = new DataTable();

        public Patientensuche()
        {
            InitializeComponent();

            // Events
            btnSuchen.Click += btnSuchen_Click;
            btnBearbeiten.Click += btnBearbeiten_Click;
            btnTermin.Click += btnTermin_Click;
            btnLoeschen.Click += btnLoeschen_Click;
            btnStartseite.Click += btnStartseite_Click;

            dgvPatienten.SelectionChanged += (s, e) => UpdateButtons();
            dgvPatienten.CellDoubleClick += (s, e) => OpenPatientenaufnahme();

            // Grid Setup
            dgvPatienten.ReadOnly = true;
            dgvPatienten.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPatienten.MultiSelect = false;
            dgvPatienten.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            UpdateButtons();

            // optional: gleich mal laden
            LoadPatients("");
        }

        private void btnStartseite_Click(object sender, EventArgs e)
        {
            var f = new Startseite();
            f.Show();
            this.Hide();
        }

        private void btnSuchen_Click(object sender, EventArgs e)
        {
            LoadPatients(txtSuche.Text.Trim());
        }

        private void LoadPatients(string filter)
        {
            try
            {
                using (var con = new MySqlConnection(_connStr))
                {
                    con.Open();

                    // Suche: SVNr exakt oder Name/Email enthält
                    string sql = @"
SELECT 
  SVNr, Vorname, Nachname, Geburtsdatum, Email, Telefonnummer, Ort,
  Termin_Datum, Termin_Uhrzeit, Dringlichkeit
FROM patienten
WHERE 
  (@q = '' OR
   SVNr = @q OR
   Vorname LIKE CONCAT('%', @q, '%') OR
   Nachname LIKE CONCAT('%', @q, '%') OR
   Email LIKE CONCAT('%', @q, '%') OR
   Ort LIKE CONCAT('%', @q, '%'))
ORDER BY Nachname, Vorname
LIMIT 500;
";

                    using (var da = new MySqlDataAdapter(sql, con))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@q", filter);

                        _table = new DataTable();
                        da.Fill(_table);

                        dgvPatienten.DataSource = _table;
                    }
                }

                UpdateButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden:\n" + ex.Message, "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateButtons()
        {
            bool hasSelection = dgvPatienten.CurrentRow != null && dgvPatienten.CurrentRow.Index >= 0;
            btnBearbeiten.Enabled = hasSelection;
            btnTermin.Enabled = hasSelection;
            btnLoeschen.Enabled = hasSelection;
        }

        private string GetSelectedSvnr()
        {
            if (dgvPatienten.CurrentRow == null) return "";
            var v = dgvPatienten.CurrentRow.Cells["SVNr"].Value;
            return v == null ? "" : v.ToString();
        }

        private void btnBearbeiten_Click(object sender, EventArgs e)
        {
            OpenPatientenaufnahme();
        }

        private void OpenPatientenaufnahme()
        {
            string svnr = GetSelectedSvnr();
            if (string.IsNullOrWhiteSpace(svnr)) return;

            // Patientenaufnahme im Edit-Modus öffnen
            var f = new Patientenaufnahme(svnr);
            f.Show();
            this.Hide();
        }

        private void btnTermin_Click(object sender, EventArgs e)
        {
            string svnr = GetSelectedSvnr();
            if (string.IsNullOrWhiteSpace(svnr)) return;

            // ✅ Termin im Edit-Modus öffnen
            var f = new Termin(svnr);
            f.Show();
            this.Hide();
        }

        private void btnLoeschen_Click(object sender, EventArgs e)
        {
            string svnr = GetSelectedSvnr();
            if (string.IsNullOrWhiteSpace(svnr)) return;

            var confirm = MessageBox.Show(
                $"Patient mit SVNr {svnr} wirklich löschen?\n(Dies kann nicht rückgängig gemacht werden.)",
                "Löschen bestätigen",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                using (var con = new MySqlConnection(_connStr))
                {
                    con.Open();

                    string sql = "DELETE FROM patienten WHERE SVNr = @SVNr;";
                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@SVNr", svnr);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Patient gelöscht.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPatients(txtSuche.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Löschen:\n" + ex.Message, "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit();
        }
    }
}
