namespace PatMedAuf
{
    partial class Startseite
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBeenden = new System.Windows.Forms.Button();
            this.btnAbmelden = new System.Windows.Forms.Button();
            this.btnAufnahme = new System.Windows.Forms.Button();
            this.btnSuchen = new System.Windows.Forms.Button();
            this.btnCSV = new System.Windows.Forms.Button();
            this.btnHL = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.groupBox1.Controls.Add(this.btnBeenden);
            this.groupBox1.Controls.Add(this.btnAbmelden);
            this.groupBox1.Location = new System.Drawing.Point(3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(269, 951);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Start";
            // 
            // btnBeenden
            // 
            this.btnBeenden.Location = new System.Drawing.Point(35, 820);
            this.btnBeenden.Name = "btnBeenden";
            this.btnBeenden.Size = new System.Drawing.Size(178, 61);
            this.btnBeenden.TabIndex = 1;
            this.btnBeenden.Text = "Beenden";
            this.btnBeenden.UseVisualStyleBackColor = true;
            // 
            // btnAbmelden
            // 
            this.btnAbmelden.Location = new System.Drawing.Point(35, 887);
            this.btnAbmelden.Name = "btnAbmelden";
            this.btnAbmelden.Size = new System.Drawing.Size(178, 56);
            this.btnAbmelden.TabIndex = 0;
            this.btnAbmelden.Text = "Abmelden";
            this.btnAbmelden.UseVisualStyleBackColor = true;
            // 
            // btnAufnahme
            // 
            this.btnAufnahme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnAufnahme.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAufnahme.Location = new System.Drawing.Point(458, 203);
            this.btnAufnahme.Name = "btnAufnahme";
            this.btnAufnahme.Size = new System.Drawing.Size(206, 137);
            this.btnAufnahme.TabIndex = 1;
            this.btnAufnahme.Text = "+ Patient anlegen";
            this.btnAufnahme.UseVisualStyleBackColor = false;
            // 
            // btnSuchen
            // 
            this.btnSuchen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnSuchen.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSuchen.Location = new System.Drawing.Point(458, 419);
            this.btnSuchen.Name = "btnSuchen";
            this.btnSuchen.Size = new System.Drawing.Size(206, 156);
            this.btnSuchen.TabIndex = 2;
            this.btnSuchen.Text = "Patient suchen";
            this.btnSuchen.UseVisualStyleBackColor = false;
            // 
            // btnCSV
            // 
            this.btnCSV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnCSV.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCSV.Location = new System.Drawing.Point(747, 203);
            this.btnCSV.Name = "btnCSV";
            this.btnCSV.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnCSV.Size = new System.Drawing.Size(205, 137);
            this.btnCSV.TabIndex = 3;
            this.btnCSV.Text = "CSV";
            this.btnCSV.UseVisualStyleBackColor = false;
            // 
            // btnHL
            // 
            this.btnHL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnHL.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHL.Location = new System.Drawing.Point(747, 419);
            this.btnHL.Name = "btnHL";
            this.btnHL.Size = new System.Drawing.Size(205, 156);
            this.btnHL.TabIndex = 4;
            this.btnHL.Text = "HL7";
            this.btnHL.UseVisualStyleBackColor = false;
            this.btnHL.Click += new System.EventHandler(this.btnHL_Click);
            // 
            // Startseite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1533, 959);
            this.Controls.Add(this.btnHL);
            this.Controls.Add(this.btnCSV);
            this.Controls.Add(this.btnSuchen);
            this.Controls.Add(this.btnAufnahme);
            this.Controls.Add(this.groupBox1);
            this.Name = "Startseite";
            this.Text = "Startseite";
            this.Load += new System.EventHandler(this.Startseite_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAbmelden;
        private System.Windows.Forms.Button btnBeenden;
        private System.Windows.Forms.Button btnAufnahme;
        private System.Windows.Forms.Button btnSuchen;
        private System.Windows.Forms.Button btnCSV;
        private System.Windows.Forms.Button btnHL;
        // private System.Windows.Forms.Button btnHL7;
    }
}