namespace PatMedAuf
{
    partial class HL
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
            this.txtSVNr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGenerieren = new System.Windows.Forms.Button();
            this.btnImportieren = new System.Windows.Forms.Button();
            this.btnStartseite = new System.Windows.Forms.Button();
            this.saveFileDialogHL7 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogHL7 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // txtSVNr
            // 
            this.txtSVNr.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSVNr.Location = new System.Drawing.Point(36, 78);
            this.txtSVNr.Name = "txtSVNr";
            this.txtSVNr.Size = new System.Drawing.Size(244, 38);
            this.txtSVNr.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "SVNr:";
            // 
            // btnGenerieren
            // 
            this.btnGenerieren.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnGenerieren.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerieren.Location = new System.Drawing.Point(36, 157);
            this.btnGenerieren.Name = "btnGenerieren";
            this.btnGenerieren.Size = new System.Drawing.Size(178, 91);
            this.btnGenerieren.TabIndex = 2;
            this.btnGenerieren.Text = "Generieren";
            this.btnGenerieren.UseVisualStyleBackColor = false;
            // 
            // btnImportieren
            // 
            this.btnImportieren.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnImportieren.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportieren.Location = new System.Drawing.Point(220, 157);
            this.btnImportieren.Name = "btnImportieren";
            this.btnImportieren.Size = new System.Drawing.Size(191, 91);
            this.btnImportieren.TabIndex = 3;
            this.btnImportieren.Text = "Importieren";
            this.btnImportieren.UseVisualStyleBackColor = false;
            // 
            // btnStartseite
            // 
            this.btnStartseite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnStartseite.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartseite.Location = new System.Drawing.Point(417, 157);
            this.btnStartseite.Name = "btnStartseite";
            this.btnStartseite.Size = new System.Drawing.Size(175, 91);
            this.btnStartseite.TabIndex = 4;
            this.btnStartseite.Text = "Startseite";
            this.btnStartseite.UseVisualStyleBackColor = false;
            // 
            // saveFileDialogHL7
            // 
            this.saveFileDialogHL7.DefaultExt = "xml";
            this.saveFileDialogHL7.Filter = "(*.xml)|*.xml";
            // 
            // openFileDialogHL7
            // 
            this.openFileDialogHL7.FileName = "openFileDialog1";
            this.openFileDialogHL7.Filter = "(*.xml)|*.xml";
            // 
            // HL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1325, 1157);
            this.Controls.Add(this.btnStartseite);
            this.Controls.Add(this.btnImportieren);
            this.Controls.Add(this.btnGenerieren);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSVNr);
            this.Name = "HL";
            this.Text = "HL7";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSVNr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGenerieren;
        private System.Windows.Forms.Button btnImportieren;
        private System.Windows.Forms.Button btnStartseite;
        private System.Windows.Forms.SaveFileDialog saveFileDialogHL7;
        private System.Windows.Forms.OpenFileDialog openFileDialogHL7;
    }
}