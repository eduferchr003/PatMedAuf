using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PatMedAuf
{
    public partial class Termin : Form
    {
        private readonly ErrorProvider _error = new ErrorProvider();

        private readonly string _connStr =
            "Server=localhost;Database=patmedauf;Uid=root;Pwd=cF170104!;SslMode=Disabled;";

        private readonly string _svnr;

        // ----------------------------
        // Konstruktoren
        // ----------------------------

        public Termin()
        {
            InitializeComponent();

            _error.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            // Ja/Nein Paar verdrahten (Mitversichert)
            WireYesNo(ckbMitJa, ckbMitNein);

            // Buttons
            btnSpeichern.Click += btnSpeichern_Click;
            btnLoeschen.Click += btnLoeschen_Click;
            btnStartseite.Click += btnStartseite_Click;
        }

        public Termin(string svnr) : this()
        {
            _svnr = (svnr ?? "").Trim();

            if (!string.IsNullOrWhiteSpace(_svnr))
            {
                LoadFromDb(_svnr);
            }
        }

        // ----------------------------
        // Navigation
        // ----------------------------

        private void btnStartseite_Click(object sender, EventArgs e)
        {
            var f = new Startseite();
            f.Show();
            this.Hide();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit();
        }

        // ----------------------------
        // Laden aus DB
        // ----------------------------

        private void LoadFromDb(string svnr)
        {
            try
            {
                using (var con = new MySqlConnection(_connStr))
                {
                    con.Open();

                    using (var cmd = new MySqlCommand("SELECT * FROM patienten WHERE SVNr=@SVNr LIMIT 1;", con))
                    {
                        cmd.Parameters.AddWithValue("@SVNr", svnr);

                        using (var r = cmd.ExecuteReader())
                        {
                            if (!r.Read())
                            {
                                MessageBox.Show("Patient nicht gefunden (SVNr).", "Hinweis",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            var map = BuildControlMapByDbName(this);

                            for (int i = 0; i < r.FieldCount; i++)
                            {
                                string col = r.GetName(i);
                                if (!map.ContainsKey(col)) continue;

                                object val = r.IsDBNull(i) ? null : r.GetValue(i);
                                SetControlValue(map[col], col, val);
                            }
                        }
                    }
                }

                // Termin-Felder explizit korrekt setzen
                // Termin_Datum -> MonthCalendar
                // Termin_Uhrzeit -> DateTimePicker
                
                if (TerminKalender != null)
                {
                    // wenn geladen wurde, zeigt MonthCalendar bereits das Datum
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden:\n" + ex.Message, "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ----------------------------
        // Speichern 
        // ----------------------------

        private void btnSpeichern_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_svnr))
            {
                MessageBox.Show("SVNr fehlt. Bitte über Patientenaufnahme öffnen.", "Hinweis",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInputs()) return;

            try
            {
                using (var con = new MySqlConnection(_connStr))
                {
                    con.Open();

                    string sql = @"
UPDATE patienten
SET
  Versicherungstraeger = @Versicherungstraeger,
  Zusatzversicherung   = @Zusatzversicherung,
  Mitversichert        = @Mitversichert,

  VornameV = @VornameV,
  NachnameV = @NachnameV,
  EmailV = @EmailV,
  TelefonnummerV = @TelefonnummerV,
  StrasseV = @StrasseV,
  NrV = @NrV,
  PLZV = @PLZV,
  OrtV = @OrtV,

  BezeichnungA = @BezeichnungA,
  EmailA = @EmailA,
  TelefonnummerA = @TelefonnummerA,
  StrasseA = @StrasseA,
  NrA = @NrA,
  PLZA = @PLZA,
  OrtA = @OrtA,

  Termin_Datum = @Termin_Datum,
  Termin_Uhrzeit = @Termin_Uhrzeit,
  GrundDesBesuches = @GrundDesBesuches,
  Dringlichkeit = @Dringlichkeit,
  Erinnerung_Email = @Erinnerung_Email,
  Erinnerung_SMS = @Erinnerung_SMS
WHERE SVNr = @SVNr;
";

                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@SVNr", _svnr);

                        cmd.Parameters.AddWithValue("@Versicherungstraeger", NullIfEmpty(cmbVersicherung.Text));
                        cmd.Parameters.AddWithValue("@Zusatzversicherung", NullIfEmpty(cmbZusatz.Text));
                        cmd.Parameters.AddWithValue("@Dringlichkeit", NullIfEmpty(cmbDringlichkeit.Text));

                        cmd.Parameters.AddWithValue("@Mitversichert", ckbMitJa.Checked ? 1 : 0);

                        cmd.Parameters.AddWithValue("@VornameV", NullIfEmpty(txtVornameV.Text));
                        cmd.Parameters.AddWithValue("@NachnameV", NullIfEmpty(txtNachnameV.Text));
                        cmd.Parameters.AddWithValue("@EmailV", NullIfEmpty(txtEmailV.Text));
                        cmd.Parameters.AddWithValue("@TelefonnummerV", NullIfEmpty(txtTelefonnummerV.Text));
                        cmd.Parameters.AddWithValue("@StrasseV", NullIfEmpty(txtStrasseV.Text));
                        cmd.Parameters.AddWithValue("@NrV", NullIfEmpty(txtNrV.Text));
                        cmd.Parameters.AddWithValue("@PLZV", NullIfEmpty(txtPLZV.Text));
                        cmd.Parameters.AddWithValue("@OrtV", NullIfEmpty(txtOrtV.Text));

                        cmd.Parameters.AddWithValue("@BezeichnungA", NullIfEmpty(txtNameA.Text));
                        cmd.Parameters.AddWithValue("@EmailA", NullIfEmpty(txtEmailA.Text));
                        cmd.Parameters.AddWithValue("@TelefonnummerA", NullIfEmpty(txtTelefonnummerA.Text));
                        cmd.Parameters.AddWithValue("@StrasseA", NullIfEmpty(txtStrasseA.Text));
                        cmd.Parameters.AddWithValue("@NrA", NullIfEmpty(txtNrA.Text));
                        cmd.Parameters.AddWithValue("@PLZA", NullIfEmpty(txtPLZA.Text));
                        cmd.Parameters.AddWithValue("@OrtA", NullIfEmpty(txtOrtA.Text));

                        // Termin-Datum aus MonthCalendar
                        DateTime terminDatum = TerminKalender.SelectionStart.Date;
                        cmd.Parameters.AddWithValue("@Termin_Datum", terminDatum);

                        // Uhrzeit aus DateTimePicker
                        TimeSpan t = dtpUhrzeit.Value.TimeOfDay;
                        cmd.Parameters.AddWithValue("@Termin_Uhrzeit", t);

                        cmd.Parameters.AddWithValue("@GrundDesBesuches", NullIfEmpty(txtGrund.Text));

                        cmd.Parameters.AddWithValue("@Erinnerung_Email", ckbEmail.Checked ? 1 : 0);
                        cmd.Parameters.AddWithValue("@Erinnerung_SMS", ckbSMS.Checked ? 1 : 0);

                        int affected = cmd.ExecuteNonQuery();
                        if (affected == 0)
                        {
                            MessageBox.Show("Kein Patient mit dieser SVNr gefunden.", "Hinweis",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                MessageBox.Show("Termin gespeichert.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Speichern:\n" + ex.Message, "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private object NullIfEmpty(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return DBNull.Value;
            return s.Trim();
        }

        // ----------------------------
        // Löschen 
        // ----------------------------

        private void btnLoeschen_Click(object sender, EventArgs e)
        {
            _error.Clear();
            ClearAllControls(this);

            // zurücksetzen
            if (TerminKalender != null) TerminKalender.SetDate(DateTime.Today);
            if (dtpUhrzeit != null) dtpUhrzeit.Value = DateTime.Now;

            ckbMitJa.Checked = false;
            ckbMitNein.Checked = false;
        }

        private void ClearAllControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox tb) tb.Clear();
                else if (c is ComboBox cb) cb.SelectedIndex = -1;
                else if (c is CheckBox chk) chk.Checked = false;
                else if (c is DateTimePicker dtp) dtp.Value = DateTime.Now;

                if (c.HasChildren) ClearAllControls(c);
            }
        }

        // ----------------------------
        // Validierung 
        // ----------------------------

        private bool ValidateInputs()
        {
            _error.Clear();
            bool ok = true;

            // Dringlichkeit Pflicht (weil ComboBox)
            ok &= RequireCombo(cmbDringlichkeit, "Dringlichkeit ist Pflicht.");

            // Mitversichert: Ja/Nein muss gewählt sein
            ok &= RequireYesNo(ckbMitJa, ckbMitNein, "Mitversichert: Ja oder Nein wählen.");

            // optional: Email-Validierung
            ok &= OptionalEmail(txtEmailV, "Ungültige E-Mail (Versicherung).");
            ok &= OptionalEmail(txtEmailA, "Ungültige E-Mail (Arbeitgeber).");

            return ok;
        }

        private bool RequireCombo(ComboBox cb, string msg)
        {
            if (string.IsNullOrWhiteSpace(cb.Text))
            {
                _error.SetError(cb, msg);
                return false;
            }
            return true;
        }

        private bool RequireYesNo(CheckBox yes, CheckBox no, string msg)
        {
            if (!yes.Checked && !no.Checked)
            {
                _error.SetError(yes, msg);
                _error.SetError(no, msg);
                return false;
            }
            return true;
        }

        private bool OptionalEmail(TextBox tb, string msg)
        {
            if (string.IsNullOrWhiteSpace(tb.Text)) return true;
            if (!Regex.IsMatch(tb.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                _error.SetError(tb, msg);
                return false;
            }
            return true;
        }

        private void WireYesNo(CheckBox yes, CheckBox no)
        {
            yes.CheckedChanged += (s, e) => { if (yes.Checked) no.Checked = false; };
            no.CheckedChanged += (s, e) => { if (no.Checked) yes.Checked = false; };
        }

        // ----------------------------
        // Mapping (txt/cmb/ckb/dtp -> DB Spaltenname)
        // ----------------------------

        private Dictionary<string, Control> BuildControlMapByDbName(Control root)
        {
            var dict = new Dictionary<string, Control>(StringComparer.OrdinalIgnoreCase);
            AddRecursive(root, dict);
            return dict;
        }

        private void AddRecursive(Control parent, Dictionary<string, Control> dict)
        {
            foreach (Control c in parent.Controls)
            {
                string dbName = ToDbName(c.Name);
                if (!string.IsNullOrWhiteSpace(dbName) && !dict.ContainsKey(dbName))
                    dict[dbName] = c;

                if (c.HasChildren) AddRecursive(c, dict);
            }
        }

        private string ToDbName(string controlName)
        {
            if (string.IsNullOrWhiteSpace(controlName)) return "";

            string n = controlName;

            if (n.StartsWith("txt", StringComparison.OrdinalIgnoreCase)) n = n.Substring(3);
            else if (n.StartsWith("cmb", StringComparison.OrdinalIgnoreCase)) n = n.Substring(3);
            else if (n.StartsWith("ckb", StringComparison.OrdinalIgnoreCase)) n = n.Substring(3);
            else if (n.StartsWith("dtp", StringComparison.OrdinalIgnoreCase)) n = n.Substring(3);

            // MonthCalendar/Buttons/Labels ignorieren
            if (n.StartsWith("btn", StringComparison.OrdinalIgnoreCase)) return "";
            if (n.StartsWith("lbl", StringComparison.OrdinalIgnoreCase)) return "";
            if (n.StartsWith("mc", StringComparison.OrdinalIgnoreCase)) return ""; // MonthCalendar wird extra gesetzt

            return n;
        }

        private void SetControlValue(Control c, string columnName, object val)
        {
            // Spezial: Termin Datum / Uhrzeit
            if (columnName == "Termin_Datum" && TerminKalender != null)
            {
                DateTime d;
                if (val != null && DateTime.TryParse(val.ToString(), out d))
                    TerminKalender.SetDate(d.Date);
                return;
            }

            if (columnName == "Termin_Uhrzeit" && dtpUhrzeit != null)
            { 
                TimeSpan t;
                if (val is TimeSpan)
                {
                    t = (TimeSpan)val;
                    dtpUhrzeit.Value = DateTime.Today.Add(t);
                    return;
                }
                if (val != null && TimeSpan.TryParse(val.ToString(), out t))
                {
                    dtpUhrzeit.Value = DateTime.Today.Add(t);
                    return;
                }
                return;
            }

            // Standard Controls
            if (c is TextBox tb) tb.Text = val == null ? "" : val.ToString();
            else if (c is ComboBox cb) cb.Text = val == null ? "" : val.ToString();
            else if (c is CheckBox chk)
            {
                if (val == null) chk.Checked = false;
                else
                {
                    int n;
                    if (int.TryParse(val.ToString(), out n)) chk.Checked = n == 1;
                    else chk.Checked = val.ToString().ToLower() == "true";
                }
            }
            else if (c is DateTimePicker dtp)
            {
                DateTime d;
                if (val != null && DateTime.TryParse(val.ToString(), out d)) dtp.Value = d;
                else dtp.Value = DateTime.Today;
            }
        }
    }
}
