namespace PatMedAuf
{
    partial class Patientensuche
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
            this.txtSuche = new System.Windows.Forms.TextBox();
            this.btnSuchen = new System.Windows.Forms.Button();
            this.dgvPatienten = new System.Windows.Forms.DataGridView();
            this.btnBearbeiten = new System.Windows.Forms.Button();
            this.btnTermin = new System.Windows.Forms.Button();
            this.btnLoeschen = new System.Windows.Forms.Button();
            this.btnStartseite = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatienten)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSuche
            // 
            this.txtSuche.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSuche.Location = new System.Drawing.Point(104, 93);
            this.txtSuche.Name = "txtSuche";
            this.txtSuche.Size = new System.Drawing.Size(449, 38);
            this.txtSuche.TabIndex = 0;
            // 
            // btnSuchen
            // 
            this.btnSuchen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnSuchen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSuchen.Location = new System.Drawing.Point(104, 241);
            this.btnSuchen.Name = "btnSuchen";
            this.btnSuchen.Size = new System.Drawing.Size(175, 79);
            this.btnSuchen.TabIndex = 1;
            this.btnSuchen.Text = "Suchen";
            this.btnSuchen.UseVisualStyleBackColor = false;
            // 
            // dgvPatienten
            // 
            this.dgvPatienten.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPatienten.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPatienten.Location = new System.Drawing.Point(716, 93);
            this.dgvPatienten.MultiSelect = false;
            this.dgvPatienten.Name = "dgvPatienten";
            this.dgvPatienten.ReadOnly = true;
            this.dgvPatienten.RowHeadersWidth = 82;
            this.dgvPatienten.RowTemplate.Height = 33;
            this.dgvPatienten.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPatienten.Size = new System.Drawing.Size(696, 436);
            this.dgvPatienten.TabIndex = 2;
            // 
            // btnBearbeiten
            // 
            this.btnBearbeiten.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnBearbeiten.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBearbeiten.Location = new System.Drawing.Point(490, 344);
            this.btnBearbeiten.Name = "btnBearbeiten";
            this.btnBearbeiten.Size = new System.Drawing.Size(177, 69);
            this.btnBearbeiten.TabIndex = 3;
            this.btnBearbeiten.Text = "Bearbeiten";
            this.btnBearbeiten.UseVisualStyleBackColor = false;
            // 
            // btnTermin
            // 
            this.btnTermin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnTermin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTermin.Location = new System.Drawing.Point(104, 434);
            this.btnTermin.Name = "btnTermin";
            this.btnTermin.Size = new System.Drawing.Size(175, 95);
            this.btnTermin.TabIndex = 4;
            this.btnTermin.Text = "Termin öffnen";
            this.btnTermin.UseVisualStyleBackColor = false;
            // 
            // btnLoeschen
            // 
            this.btnLoeschen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnLoeschen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoeschen.Location = new System.Drawing.Point(294, 344);
            this.btnLoeschen.Name = "btnLoeschen";
            this.btnLoeschen.Size = new System.Drawing.Size(173, 69);
            this.btnLoeschen.TabIndex = 5;
            this.btnLoeschen.Text = "Löschen";
            this.btnLoeschen.UseVisualStyleBackColor = false;
            // 
            // btnStartseite
            // 
            this.btnStartseite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnStartseite.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartseite.Location = new System.Drawing.Point(104, 344);
            this.btnStartseite.Name = "btnStartseite";
            this.btnStartseite.Size = new System.Drawing.Size(175, 69);
            this.btnStartseite.TabIndex = 6;
            this.btnStartseite.Text = "Startseite";
            this.btnStartseite.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(98, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 31);
            this.label1.TabIndex = 7;
            this.label1.Text = "Patient suchen: ";
            // 
            // Patientensuche
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1435, 921);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStartseite);
            this.Controls.Add(this.btnLoeschen);
            this.Controls.Add(this.btnTermin);
            this.Controls.Add(this.btnBearbeiten);
            this.Controls.Add(this.dgvPatienten);
            this.Controls.Add(this.btnSuchen);
            this.Controls.Add(this.txtSuche);
            this.Name = "Patientensuche";
            this.Text = "Patientensuche";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatienten)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSuche;
        private System.Windows.Forms.Button btnSuchen;
        private System.Windows.Forms.DataGridView dgvPatienten;
        private System.Windows.Forms.Button btnBearbeiten;
        private System.Windows.Forms.Button btnTermin;
        private System.Windows.Forms.Button btnLoeschen;
        private System.Windows.Forms.Button btnStartseite;
        private System.Windows.Forms.Label label1;
    }
}