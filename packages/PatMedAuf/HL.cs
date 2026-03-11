using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace PatMedAuf
{
    public partial class HL : Form
    {

        private readonly string _connStr =
            "Server=localhost;Database=patmedauf;Uid=root;Pwd=cF170104!;SslMode=Disabled;";

        // HL7 v3 Namespace
        private static readonly XNamespace NS = "urn:hl7-org:v3";

        public HL()
        {
            InitializeComponent();

            btnGenerieren.Click += btnGenerieren_Click;
            btnImportieren.Click += btnImportieren_Click;
            btnStartseite.Click += btnStartseite_Click;

            // Dialoge kommen aus Designer:
            saveFileDialogHL7.Filter = "HL7 v3 XML (*.xml)|*.xml|Alle Dateien (*.*)|*.*";
            saveFileDialogHL7.DefaultExt = "xml";
            saveFileDialogHL7.AddExtension = true;

            openFileDialogHL7.Filter = "HL7 v3 XML (*.xml)|*.xml|Alle Dateien (*.*)|*.*";
            openFileDialogHL7.Multiselect = false;
        }

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

        // ------------------------------------------------------------
        // 1) HL7 v3 erzeugen + speichern (downloadbar)
        // ------------------------------------------------------------
        private void btnGenerieren_Click(object sender, EventArgs e)
        {
            string svnr = (txtSVNr.Text ?? "").Trim();

            if (!Regex.IsMatch(svnr, @"^\d{10}$"))
            {
                MessageBox.Show("Bitte gültige SVNr eingeben (10 Ziffern).", "Hinweis",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataRow patient = LoadPatientRow(svnr);
                if (patient == null)
                {
                    MessageBox.Show("Patient nicht gefunden.", "Hinweis",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                
                saveFileDialogHL7.FileName = "HL7v3_" + svnr + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xml";
                if (saveFileDialogHL7.ShowDialog() != DialogResult.OK) return;

                XDocument doc = BuildHl7V3Document(patient);

                // Speichern UTF-8
                using (var fs = new FileStream(saveFileDialogHL7.FileName, FileMode.Create, FileAccess.Write))
                using (var sw = new StreamWriter(fs, new UTF8Encoding(true)))
                {
                    doc.Save(sw);
                }


                MessageBox.Show("HL7 v3 XML erfolgreich erstellt.", "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Erstellen:\n" + ex.Message, "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataRow LoadPatientRow(string svnr)
        {
            using (var con = new MySqlConnection(_connStr))
            {
                con.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM patienten WHERE SVNr=@SVNr LIMIT 1;", con))
                {
                    cmd.Parameters.AddWithValue("@SVNr", svnr);

                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count == 0) return null;
                        return dt.Rows[0];
                    }
                }
            }
        }

        private XDocument BuildHl7V3Document(DataRow p)
        {
            // sichere Getter
            string svnr = GetStr(p, "SVNr");
            string vorname = GetStr(p, "Vorname");
            string nachname = GetStr(p, "Nachname");
            string geschlecht = GetStr(p, "Geschlecht");       
            DateTime? geb = GetDate(p, "Geburtsdatum");

            string email = GetStr(p, "Email");
            string tel = GetStr(p, "Telefonnummer");
            string strasse = GetStr(p, "Strasse");
            string nr = GetStr(p, "Nr");
            string plz = GetStr(p, "PLZ");
            string ort = GetStr(p, "Ort");

            DateTime? terminDatum = GetDate(p, "Termin_Datum");
            string terminUhrzeit = GetTimeStr(p, "Termin_Uhrzeit");
            string grund = GetStr(p, "Grund");
            string dringlichkeit = GetStr(p, "Dringlichkeit");

            // HL7 v3  
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(NS + "ClinicalDocument",
                    new XElement(NS + "typeId",
                        new XAttribute("root", "2.16.840.1.113883.1.3"),
                        new XAttribute("extension", "POCD_HD000040")),
                    new XElement(NS + "id",
                        new XAttribute("root", "1.2.40.0.10.1.4.1"), 
                        new XAttribute("extension", "HL7DOC-" + svnr + "-" + DateTime.Now.ToString("yyyyMMddHHmmss"))),
                    new XElement(NS + "code",
                        new XAttribute("code", "34133-9"),
                        new XAttribute("codeSystem", "2.16.840.1.113883.6.1"),
                        new XAttribute("displayName", "Summarization of Episode Note")),
                    new XElement(NS + "title", "Patientenaufnahme + Termin"),
                    new XElement(NS + "effectiveTime",
                        new XAttribute("value", DateTime.Now.ToString("yyyyMMddHHmmss"))),

                    // recordTarget / Patient
                    new XElement(NS + "recordTarget",
                        new XElement(NS + "patientRole",
                            new XElement(NS + "id",
                                new XAttribute("root", "1.2.40.0.10.1.4.2"), 
                                new XAttribute("extension", svnr)),

                            // Adresse
                            new XElement(NS + "addr",
                                new XElement(NS + "streetAddressLine", CombineStreet(strasse, nr)),
                                new XElement(NS + "postalCode", plz),
                                new XElement(NS + "city", ort),
                                new XElement(NS + "country", "AT")),

                            // Telekom
                            string.IsNullOrWhiteSpace(tel) ? null :
                                new XElement(NS + "telecom", new XAttribute("value", "tel:" + tel)),
                            string.IsNullOrWhiteSpace(email) ? null :
                                new XElement(NS + "telecom", new XAttribute("value", "mailto:" + email)),

                            new XElement(NS + "patient",
                                new XElement(NS + "name",
                                    new XElement(NS + "given", vorname),
                                    new XElement(NS + "family", nachname)),
                                geb.HasValue
                                    ? new XElement(NS + "birthTime", new XAttribute("value", geb.Value.ToString("yyyyMMdd")))
                                    : null,
                                !string.IsNullOrWhiteSpace(geschlecht)
                                    ? new XElement(NS + "administrativeGenderCode",
                                        new XAttribute("code", MapGenderToHl7(geschlecht)))
                                    : null
                            )
                        )
                    ),

                    // Body 
                    new XElement(NS + "component",
                        new XElement(NS + "structuredBody",
                            new XElement(NS + "component",
                                new XElement(NS + "section",
                                    new XElement(NS + "code",
                                        new XAttribute("code", "46240-8"),
                                        new XAttribute("codeSystem", "2.16.840.1.113883.6.1"),
                                        new XAttribute("displayName", "History of Present Illness")),
                                    new XElement(NS + "title", "Termin"),
                                    new XElement(NS + "text",
                                        BuildTerminText(terminDatum, terminUhrzeit, grund, dringlichkeit)
                                    )
                                )
                            )
                        )
                    )
                )
            );

            // Entferne null Elemente (von conditional creation)
            RemoveNulls(doc.Root);
            return doc;
        }

        private string BuildTerminText(DateTime? datum, string uhrzeit, string grund, string dring)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Termin-Datum: " + (datum.HasValue ? datum.Value.ToString("yyyy-MM-dd") : ""));
            sb.AppendLine("Termin-Uhrzeit: " + (uhrzeit ?? ""));
            sb.AppendLine("Grund: " + (grund ?? ""));
            sb.AppendLine("Dringlichkeit: " + (dring ?? ""));
            return sb.ToString();
        }

        private void RemoveNulls(XElement element)
        {
            if (element == null) return;

            foreach (var e in element.Elements())
                RemoveNulls(e);

            element.Elements().Where(x => x == null).Remove();
        }

        private string CombineStreet(string strasse, string nr)
        {
            strasse = strasse ?? "";
            nr = nr ?? "";
            if (string.IsNullOrWhiteSpace(nr)) return strasse.Trim();
            if (string.IsNullOrWhiteSpace(strasse)) return nr.Trim();
            return (strasse.Trim() + " " + nr.Trim());
        }

        private string MapGenderToHl7(string g)
        {
            // sehr vereinfachtes Mapping
            g = (g ?? "").Trim().ToLowerInvariant();
            if (g.Contains("m")) return "M";
            if (g.Contains("w") || g.Contains("f")) return "F";
            return "U"; // unknown
        }

        private string GetStr(DataRow r, string col)
        {
            if (!r.Table.Columns.Contains(col)) return "";
            var v = r[col];
            return v == DBNull.Value ? "" : Convert.ToString(v);
        }

        private DateTime? GetDate(DataRow r, string col)
        {
            if (!r.Table.Columns.Contains(col)) return null;
            var v = r[col];
            if (v == DBNull.Value) return null;

            DateTime d;
            if (DateTime.TryParse(Convert.ToString(v), out d)) return d;
            return null;
        }

        private string GetTimeStr(DataRow r, string col)
        {
            if (!r.Table.Columns.Contains(col)) return "";
            var v = r[col];
            if (v == DBNull.Value) return "";

            // MySQL TIME kann TimeSpan sein
            if (v is TimeSpan)
                return ((TimeSpan)v).ToString();

            return Convert.ToString(v);
        }

        // ------------------------------------------------------------
        // 2) HL7 v3 importieren + parsen + in DB schreiben
        // ------------------------------------------------------------
        private void btnImportieren_Click(object sender, EventArgs e)
        {
            if (openFileDialogHL7.ShowDialog() != DialogResult.OK) return;

            try
            {
                var doc = XDocument.Load(openFileDialogHL7.FileName);
                var parsed = ParseHl7V3(doc);

                // Minimalvalidierung
                if (!Regex.IsMatch(parsed.SVNr ?? "", @"^\d{10}$"))
                    throw new Exception("SVNr fehlt oder ungültig (muss 10 Ziffern sein).");

                if (string.IsNullOrWhiteSpace(parsed.Vorname) || string.IsNullOrWhiteSpace(parsed.Nachname))
                    throw new Exception("Vorname oder Nachname fehlen in der HL7 Nachricht.");

                UpsertPatientFromHl7(parsed);

                MessageBox.Show("HL7 v3 importiert und in DB übernommen.", "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Import:\n" + ex.ToString(), "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Datencontainer für Parser
        private class Hl7Patient
        {
            public string SVNr;
            public string Vorname;
            public string Nachname;
            public DateTime? Geburtsdatum;
            public string Geschlecht;

            public string Email;
            public string Telefon;
            public string Strasse;
            public string PLZ;
            public string Ort;

            public DateTime? TerminDatum;
            public TimeSpan? TerminUhrzeit;
            public string Grund;
            public string Dringlichkeit;
        }

        private Hl7Patient ParseHl7V3(XDocument doc)
        {
            // Namespace beachten
            var root = doc.Root;
            if (root == null) throw new Exception("Leeres XML.");

            // patientRole
            var patientRole = root
                .Element(NS + "recordTarget")?
                .Element(NS + "patientRole");

            if (patientRole == null) throw new Exception("patientRole nicht gefunden.");

            // SVNr aus id/@extension
            string svnr = (string)patientRole.Element(NS + "id")?.Attribute("extension") ?? "";

            // Name
            var name = patientRole.Element(NS + "patient")?.Element(NS + "name");
            string given = name != null ? (string)name.Element(NS + "given") : "";
            string family = name != null ? (string)name.Element(NS + "family") : "";

            // birthTime
            string birthVal = (string)patientRole.Element(NS + "patient")?.Element(NS + "birthTime")?.Attribute("value") ?? "";
            DateTime? geb = ParseHl7Date(birthVal);

            // genderCode
            string genderCode = (string)patientRole.Element(NS + "patient")?.Element(NS + "administrativeGenderCode")?.Attribute("code") ?? "";

            // addr
            var addr = patientRole.Element(NS + "addr");
            string streetLine = addr != null ? (string)addr.Element(NS + "streetAddressLine") : "";
            string postal = addr != null ? (string)addr.Element(NS + "postalCode") : "";
            string city = addr != null ? (string)addr.Element(NS + "city") : "";

            // telecom 
            string tel = "";
            string email = "";
            foreach (var t in patientRole.Elements(NS + "telecom"))
            {
                var val = (string)t.Attribute("value") ?? "";
                if (val.StartsWith("tel:", StringComparison.OrdinalIgnoreCase))
                    tel = val.Substring(4);
                if (val.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
                    email = val.Substring(7);
            }

            // Termin Text 
            string terminText = root
                .Element(NS + "component")?
                .Element(NS + "structuredBody")?
                .Element(NS + "component")?
                .Element(NS + "section")?
                .Element(NS + "text")?.Value ?? "";

            DateTime? tDatum = ExtractTerminDatum(terminText);
            TimeSpan? tUhr = ExtractTerminTime(terminText);
            string grund = ExtractLineValue(terminText, "Grund:");
            string dring = ExtractLineValue(terminText, "Dringlichkeit:");

            return new Hl7Patient
            {
                SVNr = svnr.Trim(),
                Vorname = (given ?? "").Trim(),
                Nachname = (family ?? "").Trim(),
                Geburtsdatum = geb,
                Geschlecht = genderCode,

                Email = (email ?? "").Trim(),
                Telefon = (tel ?? "").Trim(),
                Strasse = (streetLine ?? "").Trim(),
                PLZ = (postal ?? "").Trim(),
                Ort = (city ?? "").Trim(),

                TerminDatum = tDatum,
                TerminUhrzeit = tUhr,
                Grund = (grund ?? "").Trim(),
                Dringlichkeit = (dring ?? "").Trim()
            };
        }

        private DateTime? ParseHl7Date(string yyyymmdd)
        {
            if (string.IsNullOrWhiteSpace(yyyymmdd)) return null;

            DateTime d;
            if (DateTime.TryParseExact(yyyymmdd.Trim(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                return d;
            return null;
        }

        private string ExtractLineValue(string text, string prefix)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";
            var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (var l in lines)
            {
                if (l.TrimStart().StartsWith(prefix))
                    return l.Substring(l.IndexOf(prefix) + prefix.Length).Trim();
            }
            return "";
        }

        private DateTime? ExtractTerminDatum(string text)
        {
            string val = ExtractLineValue(text, "Termin-Datum:");
            DateTime d;
            if (DateTime.TryParse(val, out d)) return d.Date;
            return null;
        }

        private TimeSpan? ExtractTerminTime(string text)
        {
            string val = ExtractLineValue(text, "Termin-Uhrzeit:");
            TimeSpan t;
            if (TimeSpan.TryParse(val, out t)) return t;
            return null;
        }

        private void UpsertPatientFromHl7(Hl7Patient p)
        {
            using (var con = new MySqlConnection(_connStr))
            {
                con.Open();

                // SVNr ist Unique Key -> Upsert
                string sql = @"
INSERT INTO patienten
(SVNr, Vorname, Nachname, Geburtsdatum, Geschlecht, Email, Telefonnummer, Strasse, PLZ, Ort,
 Termin_Datum, Termin_Uhrzeit, GrundDesBesuches, Dringlichkeit)
VALUES
(@SVNr, @Vorname, @Nachname, @Geburtsdatum, @Geschlecht, @Email, @Telefonnummer, @Strasse, @PLZ, @Ort,
 @Termin_Datum, @Termin_Uhrzeit, @GrundDesBesuches, @Dringlichkeit)
ON DUPLICATE KEY UPDATE
  Vorname=VALUES(Vorname),
  Nachname=VALUES(Nachname),
  Geburtsdatum=VALUES(Geburtsdatum),
  Geschlecht=VALUES(Geschlecht),
  Email=VALUES(Email),
  Telefonnummer=VALUES(Telefonnummer),
  Strasse=VALUES(Strasse),
  PLZ=VALUES(PLZ),
  Ort=VALUES(Ort),
  Termin_Datum=VALUES(Termin_Datum),
  Termin_Uhrzeit=VALUES(Termin_Uhrzeit),
  GrundDesBesuches=VALUES(GrundDesBesuches),
  Dringlichkeit=VALUES(Dringlichkeit);
";

                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@SVNr", p.SVNr);
                    cmd.Parameters.AddWithValue("@Vorname", p.Vorname);
                    cmd.Parameters.AddWithValue("@Nachname", p.Nachname);

                    cmd.Parameters.AddWithValue("@Geburtsdatum", p.Geburtsdatum.HasValue ? (object)p.Geburtsdatum.Value.Date : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Geschlecht", string.IsNullOrWhiteSpace(p.Geschlecht) ? (object)DBNull.Value : p.Geschlecht);

                    cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(p.Email) ? (object)DBNull.Value : p.Email);
                    cmd.Parameters.AddWithValue("@Telefonnummer", string.IsNullOrWhiteSpace(p.Telefon) ? (object)DBNull.Value : p.Telefon);
                    cmd.Parameters.AddWithValue("@Strasse", string.IsNullOrWhiteSpace(p.Strasse) ? (object)DBNull.Value : p.Strasse);
                    cmd.Parameters.AddWithValue("@PLZ", string.IsNullOrWhiteSpace(p.PLZ) ? (object)DBNull.Value : p.PLZ);
                    cmd.Parameters.AddWithValue("@Ort", string.IsNullOrWhiteSpace(p.Ort) ? (object)DBNull.Value : p.Ort);

                    cmd.Parameters.AddWithValue("@Termin_Datum", p.TerminDatum.HasValue ? (object)p.TerminDatum.Value.Date : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Termin_Uhrzeit", p.TerminUhrzeit.HasValue ? (object)p.TerminUhrzeit.Value : DBNull.Value);

                    cmd.Parameters.AddWithValue("@GrundDesBesuches", string.IsNullOrWhiteSpace(p.Grund) ? (object)DBNull.Value : p.Grund);
                    cmd.Parameters.AddWithValue("@Dringlichkeit", string.IsNullOrWhiteSpace(p.Dringlichkeit) ? (object)DBNull.Value : p.Dringlichkeit);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
