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
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnStartseite = new System.Windows.Forms.Button();
            this.saveFileDialogCSV = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogCSV = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(94, 56);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(151, 75);
            this.btnExport.TabIndex = 0;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = false;
            // 
            // btnImport
            // 
            this.btnImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Location = new System.Drawing.Point(289, 56);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(152, 75);
            this.btnImport.TabIndex = 1;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = false;
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
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnExport);
            this.Name = "CSV";
            this.Text = "CSV";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnStartseite;
        private System.Windows.Forms.SaveFileDialog saveFileDialogCSV;
        private System.Windows.Forms.OpenFileDialog openFileDialogCSV;
    }
}