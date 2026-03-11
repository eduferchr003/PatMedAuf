namespace PatMedAuf
{
    partial class DicomForm
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
            this.btnOeffnen = new System.Windows.Forms.Button();
            this.btnSpeichernDB = new System.Windows.Forms.Button();
            this.btnDateienLaden = new System.Windows.Forms.Button();
            this.btnView1 = new System.Windows.Forms.Button();
            this.pbDicom = new System.Windows.Forms.PictureBox();
            this.dgvTags = new System.Windows.Forms.DataGridView();
            this.trkZoom = new System.Windows.Forms.TrackBar();
            this.ofdDicom = new System.Windows.Forms.OpenFileDialog();
            this.fbdSeries = new System.Windows.Forms.FolderBrowserDialog();
            this.btnStartseite = new System.Windows.Forms.Button();
            this.sfdDicomSave = new System.Windows.Forms.SaveFileDialog();
            this.txtSVNr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnView2 = new System.Windows.Forms.Button();
            this.btnView4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbDicom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOeffnen
            // 
            this.btnOeffnen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnOeffnen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOeffnen.Location = new System.Drawing.Point(44, 12);
            this.btnOeffnen.Name = "btnOeffnen";
            this.btnOeffnen.Size = new System.Drawing.Size(154, 85);
            this.btnOeffnen.TabIndex = 0;
            this.btnOeffnen.Text = "Öffnen";
            this.btnOeffnen.UseVisualStyleBackColor = false;
            // 
            // btnSpeichernDB
            // 
            this.btnSpeichernDB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnSpeichernDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSpeichernDB.Location = new System.Drawing.Point(231, 12);
            this.btnSpeichernDB.Name = "btnSpeichernDB";
            this.btnSpeichernDB.Size = new System.Drawing.Size(152, 85);
            this.btnSpeichernDB.TabIndex = 1;
            this.btnSpeichernDB.Text = "Speichern auf DB";
            this.btnSpeichernDB.UseVisualStyleBackColor = false;
            // 
            // btnDateienLaden
            // 
            this.btnDateienLaden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnDateienLaden.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDateienLaden.Location = new System.Drawing.Point(417, 12);
            this.btnDateienLaden.Name = "btnDateienLaden";
            this.btnDateienLaden.Size = new System.Drawing.Size(157, 85);
            this.btnDateienLaden.TabIndex = 2;
            this.btnDateienLaden.Text = "Datein laden";
            this.btnDateienLaden.UseVisualStyleBackColor = false;
            // 
            // btnView1
            // 
            this.btnView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnView1.Location = new System.Drawing.Point(1361, 114);
            this.btnView1.Name = "btnView1";
            this.btnView1.Size = new System.Drawing.Size(158, 51);
            this.btnView1.TabIndex = 3;
            this.btnView1.Text = "1";
            this.btnView1.UseVisualStyleBackColor = false;
            // 
            // pbDicom
            // 
            this.pbDicom.Location = new System.Drawing.Point(44, 114);
            this.pbDicom.Name = "pbDicom";
            this.pbDicom.Size = new System.Drawing.Size(1254, 826);
            this.pbDicom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDicom.TabIndex = 4;
            this.pbDicom.TabStop = false;
            // 
            // dgvTags
            // 
            this.dgvTags.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTags.Location = new System.Drawing.Point(1327, 296);
            this.dgvTags.Name = "dgvTags";
            this.dgvTags.RowHeadersWidth = 82;
            this.dgvTags.RowTemplate.Height = 33;
            this.dgvTags.Size = new System.Drawing.Size(530, 638);
            this.dgvTags.TabIndex = 5;
            // 
            // trkZoom
            // 
            this.trkZoom.Location = new System.Drawing.Point(1327, 185);
            this.trkZoom.Name = "trkZoom";
            this.trkZoom.Size = new System.Drawing.Size(530, 90);
            this.trkZoom.TabIndex = 6;
            // 
            // ofdDicom
            // 
            this.ofdDicom.FileName = "openFileDialog1";
            // 
            // btnStartseite
            // 
            this.btnStartseite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnStartseite.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartseite.Location = new System.Drawing.Point(44, 962);
            this.btnStartseite.Name = "btnStartseite";
            this.btnStartseite.Size = new System.Drawing.Size(154, 65);
            this.btnStartseite.TabIndex = 7;
            this.btnStartseite.Text = "Startseite";
            this.btnStartseite.UseVisualStyleBackColor = false;
            // 
            // txtSVNr
            // 
            this.txtSVNr.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSVNr.Location = new System.Drawing.Point(1361, 59);
            this.txtSVNr.Name = "txtSVNr";
            this.txtSVNr.Size = new System.Drawing.Size(486, 38);
            this.txtSVNr.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1355, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 31);
            this.label1.TabIndex = 9;
            this.label1.Text = "SVNr:";
            // 
            // btnView2
            // 
            this.btnView2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnView2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnView2.Location = new System.Drawing.Point(1525, 114);
            this.btnView2.Name = "btnView2";
            this.btnView2.Size = new System.Drawing.Size(158, 51);
            this.btnView2.TabIndex = 10;
            this.btnView2.Text = "2";
            this.btnView2.UseVisualStyleBackColor = false;
            // 
            // btnView4
            // 
            this.btnView4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnView4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnView4.Location = new System.Drawing.Point(1689, 114);
            this.btnView4.Name = "btnView4";
            this.btnView4.Size = new System.Drawing.Size(158, 51);
            this.btnView4.TabIndex = 11;
            this.btnView4.Text = "4";
            this.btnView4.UseVisualStyleBackColor = false;
            // 
            // DicomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1911, 1048);
            this.Controls.Add(this.btnView4);
            this.Controls.Add(this.btnView2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSVNr);
            this.Controls.Add(this.btnStartseite);
            this.Controls.Add(this.trkZoom);
            this.Controls.Add(this.dgvTags);
            this.Controls.Add(this.pbDicom);
            this.Controls.Add(this.btnView1);
            this.Controls.Add(this.btnDateienLaden);
            this.Controls.Add(this.btnSpeichernDB);
            this.Controls.Add(this.btnOeffnen);
            this.Name = "DicomForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dicom";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.DicomForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbDicom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTags)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkZoom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOeffnen;
        private System.Windows.Forms.Button btnSpeichernDB;
        private System.Windows.Forms.Button btnDateienLaden;
        private System.Windows.Forms.Button btnView1;
        private System.Windows.Forms.PictureBox pbDicom;
        private System.Windows.Forms.DataGridView dgvTags;
        private System.Windows.Forms.TrackBar trkZoom;
        private System.Windows.Forms.OpenFileDialog ofdDicom;
        private System.Windows.Forms.FolderBrowserDialog fbdSeries;
        private System.Windows.Forms.Button btnStartseite;
        private System.Windows.Forms.SaveFileDialog sfdDicomSave;
        private System.Windows.Forms.TextBox txtSVNr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnView2;
        private System.Windows.Forms.Button btnView4;
    }
}