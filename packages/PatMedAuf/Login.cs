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

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void btnAnmelden_Click_1(object sender, EventArgs e)
        {
            string benutzername = txtBenutzername.Text.Trim();
            string passwort = txtPasswort.Text.Trim();

            if (benutzername == "admin" && passwort == "1234")
            {
                var s = new Startseite();
                s.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Benutzername oder Passwort falsch");
            }
        }
    }
}
