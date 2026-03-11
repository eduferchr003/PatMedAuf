using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PatMedAuf
{
    public partial class Patientenaufnahme : Form
    {
        private readonly ErrorProvider _error = new ErrorProvider();

        
        private readonly string _connStr =
            "Server=localhost;Database=patmedauf;Uid=root;Pwd=cF170104!;SslMode=none;";

        // wenn gesetzt => Bearbeiten-Modus, sonst Neu
        private readonly string _svnrEdit = "";

        // ----------------------------
        // Konstruktoren
        // ----------------------------

        public Patientenaufnahme()
        {
            InitializeComponent();

            _error.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            // Ja/Nein Paare verdrahten
            WireYesNo(ckbAllergienJa, ckbAllergienNein);
            WireYesNo(ckbMedikamenteJa, ckbMedikamenteNein);
            WireYesNo(ckbVorerkrankungJa, ckbVorerkrankungNein);
            WireYesNo(ckbRezeptJa, ckbRezeptNein);

            // "Welche"-Felder aktivieren/deaktivieren
            ckbAllergienJa.CheckedChanged += (s, e) => txtAllergien.Enabled = ckbAllergienJa.Checked;
            ckbMedikamenteJa.CheckedChanged += (s, e) => txtMedikamente.Enabled = ckbMedikamenteJa.Checked;
            ckbVorerkrankungJa.CheckedChanged += (s, e) => txtVorerkrankung.Enabled = ckbVorerkrankungJa.Checked;

            txtAllergien.Enabled = ckbAllergienJa.Checked;
            txtMedikamente.Enabled = ckbMedikamenteJa.Checked;
            txtVorerkrankung.Enabled = ckbVorerkrankungJa.Checked;

            // Buttons
            btnWeiter.Click += btnWeiter_Click;
            btnSpeichern.Click += btnSpeichern_Click;
            btnLoeschen.Click += btnLoeschen_Click;
            btnStartseiteP.Click += btnStartseite_Click;
        }

        // Bearbeiten-Konstruktor (öffnet und lädt Daten)
        public Patientenaufnahme(string svnr) : this()
        {
            _svnrEdit = (svnr ?? "").Trim();

            if (!string.IsNullOrWhiteSpace(_svnrEdit))
            {
                LoadPatientFromDb(_svnrEdit);

                // SVNr im Bearbeiten-Modus sperren (empfohlen)
                txtSVNr.ReadOnly = true;
            }
        }

        // ----------------------------
        // Navigation
        // ----------------------------

        private void btnWeiter_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            //  Termin bekommt SVNr
            var f = new Termin(txtSVNr.Text.Trim());
            f.Show();
            this.Hide();
        }

        private void btnStartseite_Click(object sender, EventArgs e)
        {
            var f = new Startseite();
            f.Show();
            this.Hide();
        }

        // ----------------------------
        // Laden aus DB (Bearbeiten)
        // ----------------------------

        private void LoadPatientFromDb(string svnr)
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
                                MessageBox.Show("Patient nicht gefunden.", "Hinweis",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            var map = BuildControlMapByDbName(this);

                            for (int i = 0; i < r.FieldCount; i++)
                            {
                                string col = r.GetName(i);
                                if (!map.ContainsKey(col)) continue;

                                object val = r.IsDBNull(i) ? null : r.GetValue(i);
                                SetControlValue(map[col], val);
                            }
                        }
                    }
                }

                // nach Laden: Welche-Felder korrekt setzen
                txtAllergien.Enabled = ckbAllergienJa.Checked;
                txtMedikamente.Enabled = ckbMedikamenteJa.Checked;
                txtVorerkrankung.Enabled = ckbVorerkrankungJa.Checked;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden:\n" + ex.Message, "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ----------------------------
        // Speichern (INSERT oder UPDATE)
        // ----------------------------

        private void btnSpeichern_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                using (var con = new MySqlConnection(_connStr))
                {
                    con.Open();

                    bool isEdit = !string.IsNullOrWhiteSpace(_svnrEdit);

                    if (!isEdit)
                    {
                        InsertPatient(con);
                        MessageBox.Show("Patient gespeichert (neu).", "OK",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        UpdatePatient(con, _svnrEdit);
                        MessageBox.Show("Patient gespeichert (bearbeitet).", "OK",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MessageBox.Show("Diese SVNr existiert bereits. Bitte eine andere SVNr verwenden.",
                    "Duplikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Speichern:\n" + ex.Message, "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertPatient(MySqlConnection con)
        {

            string sql = @"
INSERT INTO patienten
(
  Anrede, Titel, Vorname, Zweitname, Nachname, Geschlecht, Geburtsdatum, Geburtsland,
  Religion, SVNr, Staat, Rezept,
  Email, Telefonnummer, Strasse, Nr, PLZ, Ort,
  Koerper, Gewicht,
  Allergien, Allergien, Medikamente, Medikamente, Vorerkrankung, Vorerkrankung,
  Diagnosen, ICD10, ICF, Zusatz,

  VornameN, NachnameN, EmailN, TelefonnummerN, StrasseN, NrN, PLZN, OrtN,
  VornameH, NachnameH, EmailH, TelefonnummerH, StrasseH, NrH, PLZH, OrtH
)
VALUES
(
  @Anrede, @Titel, @Vorname, @Zweitname, @Nachname, @Geschlecht, @Geburtsdatum, @Geburtsland,
  @Religion, @SVNr, @Staat, @Rezept,
  @Email, @Telefonnummer, @Strasse, @Nr, @PLZ, @Ort,
  @Koerpergroesse, @Gewicht,
  @Allergien, @Allergien, @Medikamente, @Medikamente, @Vorerkrankung, @Vorerkrankung,
  @Diagnosen, @ICD10, @ICF, @Zusatz,

  @VornameN, @NachnameN, @EmailN, @TelefonnummerN, @StrasseN, @NrN, @PLZN, @OrtN,
  @VornameH, @NachnameH, @EmailH, @TelefonnummerH, @StrasseH, @NrH, @PLZH, @OrtH
);
";

            using (var cmd = new MySqlCommand(sql, con))
            {
                AddCommonParameters(cmd);
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdatePatient(MySqlConnection con, string svnrWhere)
        {
            string sql = @"
UPDATE patienten
SET
  Anrede=@Anrede, Titel=@Titel, Vorname=@Vorname, Zweitname=@Zweitname, Nachname=@Nachname,
  Geschlecht=@Geschlecht, Geburtsdatum=@Geburtsdatum, Geburtsland=@Geburtsland,
  Religion=@Religion, Staat=@Staat, Rezept=@Rezept,
  Email=@Email, Telefonnummer=@Telefonnummer, Strasse=@Strasse, Nr=@Nr, PLZ=@PLZ, Ort=@Ort,
  Koerper=@Koerper, Gewicht=@Gewicht,
  Allergien=@Allergien, Allergien=@Allergien,
  Medikamente=@Medikamente, Medikamente=@Medikamente,
  Vorerkrankung=@Vorerkrankung, Vorerkrankung=@Vorerkrankung,
  Diagnosen=@Diagnosen, ICD10=@ICD10, ICF=@ICF, Zusatz=@Zusatz,

  VornameN=@VornameN, NachnameN=@NachnameN, EmailN=@EmailN, TelefonnummerN=@TelefonnummerN,
  StrasseN=@StrasseN, NrN=@NrN, PLZN=@PLZN, OrtN=@OrtN,

  VornameH=@VornameH, NachnameH=@NachnameH, EmailH=@EmailH, TelefonnummerH=@TelefonnummerH,
  StrasseH=@StrasseH, NrH=@NrH, PLZH=@PLZH, OrtH=@OrtH
WHERE SVNr=@SVNrWhere;
";

            using (var cmd = new MySqlCommand(sql, con))
            {
                AddCommonParameters(cmd);
                cmd.Parameters.AddWithValue("@SVNrWhere", svnrWhere);
                cmd.ExecuteNonQuery();
            }
        }

        // gemeinsame Parameter (aus UI holen)
        private void AddCommonParameters(MySqlCommand cmd)
        {
            // ComboBoxen: cmb...
            cmd.Parameters.AddWithValue("@Anrede", NullIfEmpty(cmbAnrede.Text));
            cmd.Parameters.AddWithValue("@Titel", NullIfEmpty(cmbTitel.Text));
            cmd.Parameters.AddWithValue("@Geschlecht", NullIfEmpty(cmbGeschlecht.Text));
            cmd.Parameters.AddWithValue("@Geburtsland", NullIfEmpty(cmbGeburtsland.Text));
            cmd.Parameters.AddWithValue("@Religion", NullIfEmpty(cmbReligion.Text));
            cmd.Parameters.AddWithValue("@Staat", NullIfEmpty(cmbStaat.Text));
            cmd.Parameters.AddWithValue("@Diagnosen", NullIfEmpty(cmbDiagnosen.Text));
            cmd.Parameters.AddWithValue("@ICD10", NullIfEmpty(cmbICD10.Text));
            cmd.Parameters.AddWithValue("@ICF", NullIfEmpty(cmbICF.Text));

            // TextBoxen: txt...
            cmd.Parameters.AddWithValue("@Vorname", txtVorname.Text.Trim());
            cmd.Parameters.AddWithValue("@Zweitname", NullIfEmpty(txtZweitname.Text));
            cmd.Parameters.AddWithValue("@Nachname", txtNachname.Text.Trim());
            cmd.Parameters.AddWithValue("@Geburtsdatum", dtpGeburtsdatum.Value.Date);

            cmd.Parameters.AddWithValue("@SVNr", txtSVNr.Text.Trim());

            cmd.Parameters.AddWithValue("@Email", NullIfEmpty(txtEmail.Text));
            cmd.Parameters.AddWithValue("@Telefonnummer", NullIfEmpty(txtTelefonnummer.Text));
            cmd.Parameters.AddWithValue("@Strasse", NullIfEmpty(txtStrasse.Text));
            cmd.Parameters.AddWithValue("@Nr", NullIfEmpty(txtNr.Text));
            cmd.Parameters.AddWithValue("@PLZ", NullIfEmpty(txtPLZ.Text));
            cmd.Parameters.AddWithValue("@Ort", NullIfEmpty(txtOrt.Text));

            cmd.Parameters.AddWithValue("@Koerper", NullIfEmpty(txtKoerper.Text));
            cmd.Parameters.AddWithValue("@Gewicht", NullIfEmpty(txtGewicht.Text));

            // CheckBoxen Ja/Nein Paare -> 1/0
            cmd.Parameters.AddWithValue("@Rezept", ckbRezeptJa.Checked ? 1 : 0);

            cmd.Parameters.AddWithValue("@Allergien", ckbAllergienJa.Checked ? 1 : 0);
            cmd.Parameters.AddWithValue("@Allergien", ckbAllergienJa.Checked ? NullIfEmpty(txtAllergien.Text) : DBNull.Value);

            cmd.Parameters.AddWithValue("@Medikamente", ckbMedikamenteJa.Checked ? 1 : 0);
            cmd.Parameters.AddWithValue("@Medikamente", ckbMedikamenteJa.Checked ? NullIfEmpty(txtMedikamente.Text) : DBNull.Value);

            cmd.Parameters.AddWithValue("@Vorerkrankung", ckbVorerkrankungJa.Checked ? 1 : 0);
            cmd.Parameters.AddWithValue("@Vorerkrankung", ckbVorerkrankungJa.Checked ? NullIfEmpty(txtVorerkrankung.Text) : DBNull.Value);

            cmd.Parameters.AddWithValue("@Zusatz", NullIfEmpty(txtZusatz.Text));

            // Notfallkontakt N
            cmd.Parameters.AddWithValue("@VornameN", NullIfEmpty(txtVornameN.Text));
            cmd.Parameters.AddWithValue("@NachnameN", NullIfEmpty(txtNachnameN.Text));
            cmd.Parameters.AddWithValue("@EmailN", NullIfEmpty(txtEmailN.Text));
            cmd.Parameters.AddWithValue("@TelefonnummerN", NullIfEmpty(txtTelefonnummerN.Text));
            cmd.Parameters.AddWithValue("@StrasseN", NullIfEmpty(txtStrasseN.Text));
            cmd.Parameters.AddWithValue("@NrN", NullIfEmpty(txtNrN.Text));
            cmd.Parameters.AddWithValue("@PLZN", NullIfEmpty(txtPLZN.Text));
            cmd.Parameters.AddWithValue("@OrtN", NullIfEmpty(txtOrtN.Text));

            // Hausarzt H
            cmd.Parameters.AddWithValue("@VornameH", NullIfEmpty(txtVornameH.Text));
            cmd.Parameters.AddWithValue("@NachnameH", NullIfEmpty(txtNachnameH.Text));
            cmd.Parameters.AddWithValue("@EmailH", NullIfEmpty(txtEmailH.Text));
            cmd.Parameters.AddWithValue("@TelefonnummerH", NullIfEmpty(txtTelefonnummerH.Text));
            cmd.Parameters.AddWithValue("@StrasseH", NullIfEmpty(txtStrasseH.Text));
            cmd.Parameters.AddWithValue("@NrH", NullIfEmpty(txtNrH.Text));
            cmd.Parameters.AddWithValue("@PLZH", NullIfEmpty(txtPLZH.Text));
            cmd.Parameters.AddWithValue("@OrtH", NullIfEmpty(txtOrtH.Text));
        }

        private object NullIfEmpty(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return DBNull.Value;
            return s.Trim();
        }

        // ----------------------------
        // Löschen Button (Felder leeren)
        // ----------------------------

        private void btnLoeschen_Click(object sender, EventArgs e)
        {
            _error.Clear();
            ClearAllControls(this);

            dtpGeburtsdatum.Value = DateTime.Today;

            // "Welche" Felder deaktivieren
            txtAllergien.Enabled = false;
            txtMedikamente.Enabled = false;
            txtVorerkrankung.Enabled = false;
        }

        private void ClearAllControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox tb) tb.Clear();
                else if (c is ComboBox cb) cb.SelectedIndex = -1;
                else if (c is CheckBox chk) chk.Checked = false;
                else if (c is DateTimePicker dtp) dtp.Value = DateTime.Today;

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

            ok &= RequireCombo(cmbAnrede, "Anrede ist Pflicht.");
            ok &= RequireText(txtVorname, "Vorname ist Pflicht.");
            ok &= RequireText(txtNachname, "Nachname ist Pflicht.");
            ok &= RequireCombo(cmbGeschlecht, "Geschlecht ist Pflicht.");
            ok &= RequireCombo(cmbReligion, "Religion ist Pflicht.");
            ok &= RequireText(txtSVNr, "SVNr ist Pflicht.");
            ok &= RequireCombo(cmbStaat, "Staatsangehörigkeit ist Pflicht.");

            ok &= RequireText(txtEmail, "E-Mail ist Pflicht.");
            ok &= RequireText(txtTelefonnummer, "Telefonnummer ist Pflicht.");

            ok &= LettersOnly(txtVorname, "Vorname: nur Buchstaben/Leerzeichen/Bindestrich.");
            ok &= LettersOnly(txtNachname, "Nachname: nur Buchstaben/Leerzeichen/Bindestrich.");
            ok &= OptionalLettersOnly(txtZweitname, "Zweitname: nur Buchstaben/Leerzeichen/Bindestrich.");

            ok &= ValidateEmail(txtEmail, "Ungültige E-Mail.");
            ok &= ValidatePhone(txtTelefonnummer, "Ungültige Telefonnummer.");
            ok &= ValidateSVNr(txtSVNr, "SVNr: nur Zahlen, genau 10 Stellen.");

            ok &= RequireYesNo(ckbAllergienJa, ckbAllergienNein, "Allergien: Ja oder Nein.");
            ok &= RequireYesNo(ckbMedikamenteJa, ckbMedikamenteNein, "Medikamente: Ja oder Nein.");
            ok &= RequireYesNo(ckbVorerkrankungJa, ckbVorerkrankungNein, "Vorerkrankung: Ja oder Nein.");
            ok &= RequireYesNo(ckbRezeptJa, ckbRezeptNein, "Rezeptgebührenbefreit: Ja oder Nein.");

            if (ckbAllergienJa.Checked) ok &= RequireText(txtAllergien, "Allergien angeben.");
            if (ckbMedikamenteJa.Checked) ok &= RequireText(txtMedikamente, "Medikamente angeben.");
            if (ckbVorerkrankungJa.Checked) ok &= RequireText(txtVorerkrankung, "Vorerkrankung angeben.");

            return ok;
        }

        private bool RequireText(TextBox tb, string msg)
        {
            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                _error.SetError(tb, msg);
                return false;
            }
            return true;
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

        private bool LettersOnly(TextBox tb, string msg)
        {
            if (!Regex.IsMatch(tb.Text.Trim(), @"^[A-Za-zÄÖÜäöüß \-]+$"))
            {
                _error.SetError(tb, msg);
                return false;
            }
            return true;
        }

        private bool OptionalLettersOnly(TextBox tb, string msg)
        {
            if (string.IsNullOrWhiteSpace(tb.Text)) return true;
            return LettersOnly(tb, msg);
        }

        private bool ValidateEmail(TextBox tb, string msg)
        {
            if (!Regex.IsMatch(tb.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                _error.SetError(tb, msg);
                return false;
            }
            return true;
        }

        private bool ValidatePhone(TextBox tb, string msg)
        {
            if (!Regex.IsMatch(tb.Text.Trim(), @"^[0-9+\s\/\-]{6,}$"))
            {
                _error.SetError(tb, msg);
                return false;
            }
            return true;
        }

        private bool ValidateSVNr(TextBox tb, string msg)
        {
            if (!Regex.IsMatch(tb.Text.Trim(), @"^\d{10}$"))
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

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit();
        }

        // ----------------------------
        // Mapping: Control-Name -> DB-Spalte
        // ----------------------------

        // Entfernt Prefixe (txt/cmb/ckb/dtp) und nutzt Rest als DB-Spaltenname
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

            // Prefixe entfernen
            string n = controlName;

            if (n.StartsWith("txt", StringComparison.OrdinalIgnoreCase)) n = n.Substring(3);
            else if (n.StartsWith("cmb", StringComparison.OrdinalIgnoreCase)) n = n.Substring(3);
            else if (n.StartsWith("ckb", StringComparison.OrdinalIgnoreCase)) n = n.Substring(3);
            else if (n.StartsWith("dtp", StringComparison.OrdinalIgnoreCase)) n = n.Substring(3);

            // Buttons/Labels ignorieren
            if (n.StartsWith("btn", StringComparison.OrdinalIgnoreCase)) return "";
            if (n.StartsWith("lbl", StringComparison.OrdinalIgnoreCase)) return "";

            return n;
        }

        private void SetControlValue(Control c, object val)
        {
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
