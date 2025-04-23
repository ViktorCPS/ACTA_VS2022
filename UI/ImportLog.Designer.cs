namespace UI
{
    partial class ImportLog
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
            this.btnClose = new System.Windows.Forms.Button();
            this.tbSrcFilePath = new System.Windows.Forms.TextBox();
            this.lblSrcFile = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblReader = new System.Windows.Forms.Label();
            this.cbReader = new System.Windows.Forms.ComboBox();
            this.btnToXml = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(408, 238);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tbSrcFilePath
            // 
            this.tbSrcFilePath.Enabled = false;
            this.tbSrcFilePath.Location = new System.Drawing.Point(124, 21);
            this.tbSrcFilePath.Name = "tbSrcFilePath";
            this.tbSrcFilePath.Size = new System.Drawing.Size(273, 20);
            this.tbSrcFilePath.TabIndex = 1;
            // 
            // lblSrcFile
            // 
            this.lblSrcFile.Location = new System.Drawing.Point(12, 24);
            this.lblSrcFile.Name = "lblSrcFile";
            this.lblSrcFile.Size = new System.Drawing.Size(106, 13);
            this.lblSrcFile.TabIndex = 0;
            this.lblSrcFile.Text = "Source file path:";
            this.lblSrcFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(408, 19);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblReader
            // 
            this.lblReader.Location = new System.Drawing.Point(15, 69);
            this.lblReader.Name = "lblReader";
            this.lblReader.Size = new System.Drawing.Size(103, 13);
            this.lblReader.TabIndex = 3;
            this.lblReader.Text = "Reader:";
            this.lblReader.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbReader
            // 
            this.cbReader.FormattingEnabled = true;
            this.cbReader.Location = new System.Drawing.Point(124, 61);
            this.cbReader.Name = "cbReader";
            this.cbReader.Size = new System.Drawing.Size(176, 21);
            this.cbReader.TabIndex = 4;
            // 
            // btnToXml
            // 
            this.btnToXml.Location = new System.Drawing.Point(15, 238);
            this.btnToXml.Name = "btnToXml";
            this.btnToXml.Size = new System.Drawing.Size(110, 23);
            this.btnToXml.TabIndex = 5;
            this.btnToXml.Text = "To XML";
            this.btnToXml.UseVisualStyleBackColor = true;
            this.btnToXml.Click += new System.EventHandler(this.btnToXml_Click);
            // 
            // ImportLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 273);
            this.ControlBox = false;
            this.Controls.Add(this.btnToXml);
            this.Controls.Add(this.cbReader);
            this.Controls.Add(this.lblReader);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.lblSrcFile);
            this.Controls.Add(this.tbSrcFilePath);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ImportLog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ImportLog";
            this.Load += new System.EventHandler(this.ImportLog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox tbSrcFilePath;
        private System.Windows.Forms.Label lblSrcFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblReader;
        private System.Windows.Forms.ComboBox cbReader;
        private System.Windows.Forms.Button btnToXml;
    }
}