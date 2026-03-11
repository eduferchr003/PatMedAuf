namespace PatMedAuf
{
    partial class CSV
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
            this.btnExportCSV = new System.Windows.Forms.Button();
            this.btnImportCSV = new System.Windows.Forms.Button();
            this.btnStartseite = new System.Windows.Forms.Button();
            this.saveFileDialogCSV = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogCSV = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // btnExportCSV
            // 
            this.btnExportCSV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnExportCSV.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportCSV.Location = new System.Drawing.Point(94, 56);
            this.btnExportCSV.Name = "btnExportCSV";
            this.btnExportCSV.Size = new System.Drawing.Size(151, 75);
            this.btnExportCSV.TabIndex = 0;
            this.btnExportCSV.Text = "Export";
            this.btnExportCSV.UseVisualStyleBackColor = false;
            // 
            // btnImportCSV
            // 
            this.btnImportCSV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnImportCSV.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportCSV.Location = new System.Drawing.Point(289, 56);
            this.btnImportCSV.Name = "btnImportCSV";
            this.btnImportCSV.Size = new System.Drawing.Size(152, 75);
            this.btnImportCSV.TabIndex = 1;
            this.btnImportCSV.Text = "Import";
            this.btnImportCSV.UseVisualStyleBackColor = false;
            // 
            // btnStartseite
            // 
            this.btnStartseite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnStartseite.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartseite.Location = new System.Drawing.Point(482, 56);
            this.btnStartseite.Name = "btnStartseite";
            this.btnStartseite.Size = new System.Drawing.Size(168, 75);
            this.btnStartseite.TabIndex = 2;
            this.btnStartseite.Text = "Startseite";
            this.btnStartseite.UseVisualStyleBackColor = false;
            // 
            // saveFileDialogCSV
            // 
            this.saveFileDialogCSV.DefaultExt = "csv";
            this.saveFileDialogCSV.Filter = "(*.csv)|*.csv";
            // 
            // openFileDialogCSV
            // 
            this.openFileDialogCSV.FileName = "openFileDialogCSV";
            this.openFileDialogCSV.Filter = "(*.csv)|.*csv";
            // 
            // CSV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 177);
            this.Controls.Add(this.btnStartseite);
            this.Controls.Add(this.btnImportCSV);
            this.Controls.Add(this.btnExportCSV);
            this.Name = "CSV";
            this.Text = "CSV";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExportCSV;
        private System.Windows.Forms.Button btnImportCSV;
        private System.Windows.Forms.Button btnStartseite;
        private System.Windows.Forms.SaveFileDialog saveFileDialogCSV;
        private System.Windows.Forms.OpenFileDialog openFileDialogCSV;
    }
}