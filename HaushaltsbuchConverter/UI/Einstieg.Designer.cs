namespace HaushaltsbuchConverter.UI
{
    partial class Einstieg
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
            this.btnInitialize = new System.Windows.Forms.Button();
            this.btnFillDBFromCSV = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnInitialize
            // 
            this.btnInitialize.Location = new System.Drawing.Point(12, 12);
            this.btnInitialize.Name = "btnInitialize";
            this.btnInitialize.Size = new System.Drawing.Size(94, 23);
            this.btnInitialize.TabIndex = 0;
            this.btnInitialize.Text = "DB erstellen";
            this.btnInitialize.UseVisualStyleBackColor = true;
            this.btnInitialize.Click += new System.EventHandler(this.btnInitialize_Click);
            // 
            // btnFillDBFromCSV
            // 
            this.btnFillDBFromCSV.Location = new System.Drawing.Point(12, 41);
            this.btnFillDBFromCSV.Name = "btnFillDBFromCSV";
            this.btnFillDBFromCSV.Size = new System.Drawing.Size(94, 23);
            this.btnFillDBFromCSV.TabIndex = 1;
            this.btnFillDBFromCSV.Text = "DB füllen";
            this.btnFillDBFromCSV.UseVisualStyleBackColor = true;
            this.btnFillDBFromCSV.Click += new System.EventHandler(this.btnFillDBFromCSV_Click);
            // 
            // Einstieg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnFillDBFromCSV);
            this.Controls.Add(this.btnInitialize);
            this.Name = "Einstieg";
            this.Text = "Einstieg";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnInitialize;
        private System.Windows.Forms.Button btnFillDBFromCSV;
    }
}