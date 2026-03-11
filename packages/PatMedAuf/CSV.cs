using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CsvHelper;
using CsvHelper.Configuration;
using MySql.Data.MySqlClient;

namespace PatMedAuf
{
    public partial class CSV : Form
    {
        private readonly string _connStr =
            "Server=localhost;Database=patmedauf;Uid=root;Pwd=DEIN_PASSWORT;SslMode=Disabled;";

        // ALLE Spalten aus Patientenaufnahme + Termin
        private readonly List<string> DbColumns = new List<string>
        {
            "SVNr",
            "Anrede",
            "Titel",
            "Vorname",
            "Nachname",
            "Geschlecht",
            "Geburtsdatum",
            "Geburtsland",
            "Religion",
            "Staatsangehoerigkeit",

            "Strasse",
            "Hausnummer",
            "PLZ",
            "Ort",
            "Telefon",
            "Email",

            "VornameN",
            "NachnameN",
            "TelefonN",

            "VornameH",
            "NachnameH",
            "TelefonH",

            "Versicherungstraeger",
            "Zusatzversicherung",

            "Termin_Datum",
            "Termin_Uhrzeit",
            "Diagnose",
            "ICD10",
            "ICF",
            "Dringlichkeit",

            "Erinnerung_Email",
            "Erinnerung_SMS"
        };

        public CSV()
        {
            InitializeComponent();

            btnExport.Click += btnExport_Click;
            btnImport.Click += btnImport_Click;
            btnStartseite.Click += btnStartseite_Click;
        }

        // =========================
        // EXPORT
        // =========================
        private void btnExport_Click(object sender, EventArgs e)
        {
            saveFileDialogCSV.Filter = "CSV Dateien (*.csv)|*.csv";
            saveFileDialogCSV.Title = "CSV exportieren";

            if (saveFileDialogCSV.ShowDialog() == DialogResult.OK)
            {
                ExportAllColumnsToCsv(saveFileDialogCSV.FileName);
                MessageBox.Show("CSV erfolgreich exportiert.");
            }
        }

        private void ExportAllColumnsToCsv(string filePath)
        {
            using (var con = new MySqlConnection(_connStr))
            {
                con.Open();

                string sql =
                    $"SELECT {string.Join(",", DbColumns)} FROM patienten ORDER BY Nachname, Vorname";

                using (var cmd = new MySqlCommand(sql, con))
                using (var reader = cmd.ExecuteReader())
                using (var writer = new StreamWriter(filePath, false, new UTF8Encoding(true)))
                {
                    var config = new CsvConfiguration(CultureInfo.GetCultureInfo("de-AT"))
                    {
                        Delimiter = ";",
                        HasHeaderRecord = true
                    };

                    using (var csv = new CsvWriter(writer, config))
                    {
                        // Header
                        foreach (var col in DbColumns)
                        {
                            csv.WriteField(col);
                        }
                        csv.NextRecord();

                        // Daten
                        while (reader.Read())
                        {
                            foreach (var col in DbColumns)
                            {
                                object v = reader[col];
                                csv.WriteField(v == DBNull.Value ? "" : v.ToString());
                            }
                            csv.NextRecord();
                        }
                    }
                }
            }
        }

        // =========================
        // IMPORT
        // =========================
        private void btnImport_Click(object sender, EventArgs e)
        {
            openFileDialogCSV.Filter = "CSV Dateien (*.csv)|*.csv";
            openFileDialogCSV.Title = "CSV importieren";

            if (openFileDialogCSV.ShowDialog() == DialogResult.OK)
            {
                ImportCsvToDatabase(openFileDialogCSV.FileName);
                MessageBox.Show("CSV erfolgreich importiert.");
            }
        }

        private void ImportCsvToDatabase(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.GetCultureInfo("de-AT"))
            {
                Delimiter = ";",
                HasHeaderRecord = true
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            using (var con = new MySqlConnection(_connStr))
            {
                con.Open();

                var records = csv.GetRecords<Dictionary<string, string>>();

                foreach (var row in records)
                {
                    string columns = string.Join(",", DbColumns);
                    string parameters = string.Join(",", DbColumns.ConvertAll(c => "@" + c));

                    string sql =
                        $"INSERT INTO patienten ({columns}) VALUES ({parameters})";

                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        foreach (var col in DbColumns)
                        {
                            if (row.ContainsKey(col) && !string.IsNullOrWhiteSpace(row[col]))
                                cmd.Parameters.AddWithValue("@" + col, row[col]);
                            else
                                cmd.Parameters.AddWithValue("@" + col, DBNull.Value);
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // =========================
        // STARTSEITE
        // =========================
        private void btnStartseite_Click(object sender, EventArgs e)
        {
            Startseite f = new Startseite();
            f.Show();
            this.Close();
        }
    }
}