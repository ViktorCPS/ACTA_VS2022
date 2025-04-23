namespace SiemensTransfer
{
    partial class SiemensDataTransfer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SiemensDataTransfer));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ascoDBSetupToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.brezaDBSetupToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mappingToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ascoDBSetupToolStripMenuItem1,
            this.brezaDBSetupToolStripMenuItem1,
            this.mappingToolStripMenuItem1,
            this.exitToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(577, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ascoDBSetupToolStripMenuItem1
            // 
            this.ascoDBSetupToolStripMenuItem1.Name = "ascoDBSetupToolStripMenuItem1";
            this.ascoDBSetupToolStripMenuItem1.Size = new System.Drawing.Size(96, 20);
            this.ascoDBSetupToolStripMenuItem1.Text = "Asco DB Setup";
            this.ascoDBSetupToolStripMenuItem1.Click += new System.EventHandler(this.ascoDBSetupToolStripMenuItem1_Click);
            // 
            // brezaDBSetupToolStripMenuItem1
            // 
            this.brezaDBSetupToolStripMenuItem1.Name = "brezaDBSetupToolStripMenuItem1";
            this.brezaDBSetupToolStripMenuItem1.Size = new System.Drawing.Size(98, 20);
            this.brezaDBSetupToolStripMenuItem1.Text = "Breza DB Setup";
            this.brezaDBSetupToolStripMenuItem1.Click += new System.EventHandler(this.brezaDBSetupToolStripMenuItem1_Click);
            // 
            // mappingToolStripMenuItem1
            // 
            this.mappingToolStripMenuItem1.Name = "mappingToolStripMenuItem1";
            this.mappingToolStripMenuItem1.Size = new System.Drawing.Size(67, 20);
            this.mappingToolStripMenuItem1.Text = "Mapping";
            this.mappingToolStripMenuItem1.Click += new System.EventHandler(this.mappingToolStripMenuItem1_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // SiemensDataTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 406);
            this.ControlBox = false;
            this.Controls.Add(this.menuStrip1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SiemensDataTransfer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Siemens data transfer";
            this.Load += new System.EventHandler(this.SiemensDataTransfer_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ascoDBSetupToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem brezaDBSetupToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mappingToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
    }
}

