namespace UITool
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCSVPath = new System.Windows.Forms.Button();
            this.btnReadHead = new System.Windows.Forms.Button();
            this.btnReadTable = new System.Windows.Forms.Button();
            this.btnReadBody = new System.Windows.Forms.Button();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.tbCSVPath = new System.Windows.Forms.TextBox();
            this.tbTableLine = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCSVPath
            // 
            this.btnCSVPath.Location = new System.Drawing.Point(12, 12);
            this.btnCSVPath.Name = "btnCSVPath";
            this.btnCSVPath.Size = new System.Drawing.Size(115, 23);
            this.btnCSVPath.TabIndex = 0;
            this.btnCSVPath.Text = "Get CSV-Path";
            this.btnCSVPath.UseVisualStyleBackColor = true;
            this.btnCSVPath.Click += new System.EventHandler(this.btnCSVPath_Click);
            // 
            // btnReadHead
            // 
            this.btnReadHead.Location = new System.Drawing.Point(12, 112);
            this.btnReadHead.Name = "btnReadHead";
            this.btnReadHead.Size = new System.Drawing.Size(115, 23);
            this.btnReadHead.TabIndex = 1;
            this.btnReadHead.Text = "ReadHead";
            this.btnReadHead.UseVisualStyleBackColor = true;
            this.btnReadHead.Click += new System.EventHandler(this.btnReadHead_Click);
            // 
            // btnReadTable
            // 
            this.btnReadTable.Location = new System.Drawing.Point(12, 141);
            this.btnReadTable.Name = "btnReadTable";
            this.btnReadTable.Size = new System.Drawing.Size(115, 23);
            this.btnReadTable.TabIndex = 2;
            this.btnReadTable.Text = "ReadTable";
            this.btnReadTable.UseVisualStyleBackColor = true;
            this.btnReadTable.Click += new System.EventHandler(this.btnReadTable_Click);
            // 
            // btnReadBody
            // 
            this.btnReadBody.Location = new System.Drawing.Point(12, 170);
            this.btnReadBody.Name = "btnReadBody";
            this.btnReadBody.Size = new System.Drawing.Size(115, 23);
            this.btnReadBody.TabIndex = 3;
            this.btnReadBody.Text = "ReadBody";
            this.btnReadBody.UseVisualStyleBackColor = true;
            this.btnReadBody.Click += new System.EventHandler(this.btnReadBody_Click);
            // 
            // rtbOutput
            // 
            this.rtbOutput.Location = new System.Drawing.Point(133, 41);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.Size = new System.Drawing.Size(655, 397);
            this.rtbOutput.TabIndex = 4;
            this.rtbOutput.Text = "";
            // 
            // tbCSVPath
            // 
            this.tbCSVPath.Location = new System.Drawing.Point(133, 12);
            this.tbCSVPath.Name = "tbCSVPath";
            this.tbCSVPath.Size = new System.Drawing.Size(655, 22);
            this.tbCSVPath.TabIndex = 5;
            // 
            // tbTableLine
            // 
            this.tbTableLine.Location = new System.Drawing.Point(73, 41);
            this.tbTableLine.Name = "tbTableLine";
            this.tbTableLine.Size = new System.Drawing.Size(54, 22);
            this.tbTableLine.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tbTableLine);
            this.Controls.Add(this.tbCSVPath);
            this.Controls.Add(this.rtbOutput);
            this.Controls.Add(this.btnReadBody);
            this.Controls.Add(this.btnReadTable);
            this.Controls.Add(this.btnReadHead);
            this.Controls.Add(this.btnCSVPath);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCSVPath;
        private System.Windows.Forms.Button btnReadHead;
        private System.Windows.Forms.Button btnReadTable;
        private System.Windows.Forms.Button btnReadBody;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.TextBox tbCSVPath;
        private System.Windows.Forms.TextBox tbTableLine;
    }
}

