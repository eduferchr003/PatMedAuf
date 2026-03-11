namespace PatMedAuf
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAnmelden = new System.Windows.Forms.Button();
            this.txtBenutzername = new System.Windows.Forms.TextBox();
            this.txtPasswort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(174, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(281, 42);
            this.label2.TabIndex = 0;
            this.label2.Text = "Benutzername: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(216, 402);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 42);
            this.label3.TabIndex = 1;
            this.label3.Text = "Passwort: ";
            // 
            // btnAnmelden
            // 
            this.btnAnmelden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnAnmelden.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAnmelden.Location = new System.Drawing.Point(193, 682);
            this.btnAnmelden.Name = "btnAnmelden";
            this.btnAnmelden.Size = new System.Drawing.Size(240, 80);
            this.btnAnmelden.TabIndex = 2;
            this.btnAnmelden.Text = "Anmelden ";
            this.btnAnmelden.UseVisualStyleBackColor = false;
            // 
            // txtBenutzername
            // 
            this.txtBenutzername.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBenutzername.Location = new System.Drawing.Point(134, 236);
            this.txtBenutzername.Name = "txtBenutzername";
            this.txtBenutzername.Size = new System.Drawing.Size(366, 49);
            this.txtBenutzername.TabIndex = 3;
            // 
            // txtPasswort
            // 
            this.txtPasswort.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPasswort.Location = new System.Drawing.Point(134, 484);
            this.txtPasswort.Name = "txtPasswort";
            this.txtPasswort.Size = new System.Drawing.Size(366, 49);
            this.txtPasswort.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(171, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(262, 55);
            this.label4.TabIndex = 5;
            this.label4.Text = "PatMedAuf";
            // 
            // Login
            // 
            this.ClientSize = new System.Drawing.Size(621, 836);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPasswort);
            this.Controls.Add(this.txtBenutzername);
            this.Controls.Add(this.btnAnmelden);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAnmelden;
        private System.Windows.Forms.TextBox txtBenutzername;
        private System.Windows.Forms.TextBox txtPasswort;
        private System.Windows.Forms.Label label4;
    }
}