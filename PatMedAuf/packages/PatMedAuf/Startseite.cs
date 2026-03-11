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
        }

        private void Startseite_Load(object sender, EventArgs e)
        {

        }

        private void btnPatientAnlegen_Click(object sender, EventArgs e)
        {
            Patientenaufnahme f = new Patientenaufnahme();
            f.Show();
        }
        private void btnPatientSuchen_Click(object sender, EventArgs e)
        {
            Patientensuche f = new Patientensuche();
            f.Show(); 
        }
        private void btnVSC_Click(object sender, EventArgs e)
        {
            CSV f = new CSV();
            f.Show();
        }
       // private void btnHL7(object sender, EventArgs e)
        //{
          //  HL f = new HL();
            //f.Show();
        //}

        //Abmelden -> zurück zum Login
        private void btnAbmelden_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close(); //Startseite schließen 
        }

        private void btnBeenden_Click(Object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnHL_Click(object sender, EventArgs e)
        {
            HL f = new HL();
            f.Show();
        }
    }
}
