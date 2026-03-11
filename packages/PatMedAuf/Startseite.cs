using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatMedAuf
{
    public partial class Startseite : Form
    {
        public Startseite()
        {
            InitializeComponent();
            this.FormClosed += (s, e) => Application.Exit();
        }

        private void Startseite_Load(object sender, EventArgs e)
        {

        }


        private void btnHL_Click(object sender, EventArgs e)
        {
            HL f = new HL();
            f.Show();
            this.Hide();
        }

        private void btnAufnahme_Click(object sender, EventArgs e)
        {
            Patientenaufnahme f = new Patientenaufnahme();
            f.Show();
            this.Hide();
        }

        private void btnCSV_Click(object sender, EventArgs e)
        {
            CSV f = new CSV();
            f.Show();
            this.Hide();
        }

        private void btnDicom_Click_1(object sender, EventArgs e)
        {
            DicomForm dicom = new DicomForm(); 
            dicom.Show();
            this.Hide();
        }

        private void btnSuchen_Click(object sender, EventArgs e)
        {
            Patientensuche f = new Patientensuche();
            f.Show();
            this.Hide();
        }

        private void btnBeenden_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAbmelden_Click_1(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide(); 
        }
    }
}
