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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        public void btnAnmelden_Click(object sender, EventArgs e)
        {
            string benutzername = txtBenutzername.Text; 
            string passwort = txtPasswort.Text;

            if(benutzername == "admin" && passwort == "1234")
            {
                Startseite startseite = new Startseite();
                startseite.Show();
                this.Hide(); 
            }
            else
            {
                MessageBox.Show("Benutzername oder Passwort falsch"); 
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
