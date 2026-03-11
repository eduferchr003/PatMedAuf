using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CsvHelper;
using CsvHelper.Configuration;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;

namespace PatMedAuf
{
    public partial class CSV : Form
    {
        
        private readonly string _connStr =
            "Server=localhost;Database=patmedauf;Uid=root;Pwd=cF170104!;SslMode=none;";

        // Alle Spalten 
        private static readonly string[] DbColumns =
        {
            // Stammdaten
            "Anrede","Titel","Vorname","Zweitname","Nachname","Geschlecht","Geburtsdatum","Geburtsland",
            "Religion","SVNr","Staat","Rezept",

            // Kontaktdaten
            "Email","Telefonnummer","Strasse","Nr","PLZ","Ort",

            // Medizinisch
            "Koerper","Gewicht","Allergien","Allergien","Medikamente","Medikamente",
            "Vorerkrankung","Vorerkrankung","Diagnosen","ICD10","ICF","Zusatz",

            // Notfallkontakt N
            "VornameN","NachnameN","EmailN","TelefonnummerN","StrasseN","NrN","PLZN","OrtN",

            // Hausarzt H
            "VornameH","NachnameH","EmailH","TelefonnummerH","StrasseH","NrH","PLZH","OrtH",

            // Versicherung/mitversichert V
            "Versicherung","Zusatz","Mit",
            "VornameV","NachnameV","EmailV","TelefonnummerV","StrasseV","NrV","PLZV","OrtV",

            // Arbeitgeber A
            "NameA","EmailA","TelefonnummerA","StrasseA","NrA","PLZA","OrtA",

            // Termin
            "Termin_Datum","Termin_Uhrzeit","Grund","Dringlichkeit","Email","SMS"
        };

        public CSV()
        {
            InitializeComponent();

            // Buttons verdrahten
            btnExportCSV.Click += btnExportCsv_Click;
            btnImportCSV.Click += btnImportCsv_Click;
            btnStartseite.Click += btnStartseite_Click;

            // Optional: Dialoge vorkonfigurieren (im Designer geht's auch)
            // Export:
            saveFileDialogCSV.Filter = "CSV-Datei (*.csv)|*.csv";
            saveFileDialogCSV.DefaultExt = "csv";
            saveFileDialogCSV.AddExtension = true;

            // Import:
            openFileDialogCSV.Filter = "CSV-Datei (*.csv)|*.csv";
            openFileDialogCSV.Multiselect = false;
        }

        // -----------------------------
        // Startseite
        // -----------------------------
        private void btnStartseite_Click(object sender, EventArgs e)
        {
            var f = new Startseite();
            f.Show();
            this.Hide();
        }

        // -----------------------------
        // EXPORT (DB -> CSV)
        // -----------------------------
        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            saveFileDialogCSV.FileName = $"patienten_export_{DateTime.Now:yyyyMMdd_HHmm}.csv";

            if (saveFileDialogCSV.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                ExportAllColumnsToCsv(saveFileDialogCSV.FileName);
                MessageBox.Show("CSV Export erfolgreich.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Export:\n" + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportAllColumnsToCsv(string filePath)
        {
            using (var con = new MySqlConnection(_connStr))
            {
                con.Open();

                string sql = $"SELECT {string.Join(",", DbColumns)} FROM patienten ORDER BY Nachname, Vorname;";

                using (var cmd = new MySqlCommand(sql, con))
                using (var reader = cmd.ExecuteReader())
                using (var writer = new StreamWriter(filePath, false, new UTF8Encoding(true))) // BOM für Excel
                using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.GetCultureInfo("de-AT"))
                {
                    Delimiter = ";",
                    HasHeaderRecord = true
                }))
                {
                    // Header
                    foreach (var col in DbColumns)
                        csv.WriteField(col);
                    csv.NextRecord();

                    while (reader.Read())
                    {
                        foreach (var col in DbColumns)
                        {
                            object v = reader[col];
                            csv.WriteField(v == DBNull.Value ? "" : v);
                        }
                        csv.NextRecord();
                    }
                }
            }
        }

        // -----------------------------
        // IMPORT (CSV -> DB)
        // -----------------------------
        private void btnImportCsv_Click(object sender, EventArgs e)
        {
            if (openFileDialogCSV.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                var result = ImportCsvToDatabase(openFileDialogCSV.FileName);

                MessageBox.Show(
                    $"Import fertig.\nErfolgreich: {result.SuccessCount}\nFehler: {result.Errors.Count}\n\n" +
                    (result.Errors.Count > 0 ? "Erste Fehler:\n" + string.Join("\n", result.Errors.Take(10)) : ""),
                    "Import",
                    MessageBoxButtons.OK,
                    result.Errors.Count == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Import:\n" + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class ImportResult
        {
            public int SuccessCount { get; set; }
            public List<string> Errors { get; } = new List<string>();
        }

        private ImportResult ImportCsvToDatabase(string csvPath)
        {
            var res = new ImportResult();

            string insertCols = string.Join(",", DbColumns);
            string insertVals = string.Join(",", DbColumns.Select(c => "@" + c));
            string updateSet = string.Join(",",
                DbColumns.Where(c => c != "SVNr").Select(c => $"{c}=VALUES({c})"));

            string sql = $@"
INSERT INTO patienten ({insertCols})
VALUES ({insertVals})
ON DUPLICATE KEY UPDATE {updateSet};";

            using (var con = new MySqlConnection(_connStr))
            {
                con.Open();

                using (var tx = con.BeginTransaction())
                using (var sr = new StreamReader(csvPath))
                using (var csv = new CsvReader(sr, new CsvConfiguration(CultureInfo.GetCultureInfo("de-AT"))
                {
                    Delimiter = ";",
                    HasHeaderRecord = true,
                    BadDataFound = null,
                    MissingFieldFound = null
                }))
                {
                    csv.Read();
                    csv.ReadHeader();

                    var headers = new HashSet<string>(csv.HeaderRecord ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);

                    // Mindestspalten
                    if (!headers.Contains("SVNr") || !headers.Contains("Vorname") || !headers.Contains("Nachname"))
                        throw new Exception("CSV muss mindestens die Spalten SVNr, Vorname, Nachname enthalten.");

                    int row = 1;

                    while (csv.Read())
                    {
                        row++;

                        string svnr = (csv.GetField("SVNr") ?? "").Trim();
                        string vorname = (csv.GetField("Vorname") ?? "").Trim();
                        string nachname = (csv.GetField("Nachname") ?? "").Trim();

                        var err = ValidateCore(svnr, vorname, nachname);
                        if (err != null)
                        {
                            res.Errors.Add($"Zeile {row}: {err}");
                            continue;
                        }

                        using (var cmd = new MySqlCommand(sql, con, tx))
                        {
                            foreach (var col in DbColumns)
                            {
                                string raw = headers.Contains(col) ? (csv.GetField(col) ?? "").Trim() : "";
                                object value = ConvertToDbValue(col, raw);
                                cmd.Parameters.AddWithValue("@" + col, value);
                            }

                            cmd.ExecuteNonQuery();
                        }

                        res.SuccessCount++;
                    }

                    tx.Commit();
                }
            }

            return res;
        }

        private string ValidateCore(string svnr, string vorname, string nachname)
        {
            if (!Regex.IsMatch(svnr ?? "", @"^\d{10}$"))
                return "SVNr muss genau 10 Ziffern haben.";
            if (string.IsNullOrWhiteSpace(vorname))
                return "Vorname ist Pflicht.";
            if (string.IsNullOrWhiteSpace(nachname))
                return "Nachname ist Pflicht.";
            return null;
        }

        private object ConvertToDbValue(string column, string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return DBNull.Value;

            // Boolean-Felder (TINYINT / BOOL)
            if (column == "Rezept"
             || column == "Allergien"
             || column == "Medikamente"
             || column == "Vorerkrankung"
             || column == "Mit"
             || column == "Email"
             || column == "SMS")
            {
                return ParseBool01(raw);
            }

            // DATE Felder
            if (column == "Geburtsdatum" || column == "Termin_Datum")
            {
                var d = ParseDate(raw);
                return d.HasValue ? (object)d.Value.Date : DBNull.Value;
            }

            // TIME Feld
            if (column == "Termin_Uhrzeit")
            {
                var t = ParseTime(raw);
                return t.HasValue ? (object)t.Value : DBNull.Value;
            }

            // Standard: Text
            return raw;
        }


        private int ParseBool01(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return 0;

            s = s.Trim().ToLowerInvariant();

            if (s == "1"
             || s == "true"
             || s == "ja"
             || s == "j"
             || s == "yes"
             || s == "y")
                return 1;

            if (s == "0"
             || s == "false"
             || s == "nein"
             || s == "n"
             || s == "no")
                return 0;

            // Fallback: unbekannte Werte -> 0
            return 0;
        }


        private DateTime? ParseDate(string s)
        {
            var formats = new[] { "yyyy-MM-dd", "dd.MM.yyyy", "d.M.yyyy" };
            if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
                return d;
            if (DateTime.TryParse(s, out d))
                return d;
            return null;
        }

        private TimeSpan? ParseTime(string s)
        {
            if (TimeSpan.TryParse(s, out var t))
                return t;
            return null;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit();
        }
    }
}
