namespace UI
{
    partial class EmployeeLocationsSummary
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbLocations = new System.Windows.Forms.TabPage();
            this.lvLocByWU = new System.Windows.Forms.ListView();
            this.lvLocations = new System.Windows.Forms.ListView();
            this.tbWU = new System.Windows.Forms.TabPage();
            this.lvWUByLoc = new System.Windows.Forms.ListView();
            this.lvWU = new System.Windows.Forms.ListView();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lblPrint = new System.Windows.Forms.Label();
            this.btnReport = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tbLocations.SuspendLayout();
            this.tbWU.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(907, 590);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbLocations);
            this.tabControl1.Controls.Add(this.tbWU);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(970, 572);
            this.tabControl1.TabIndex = 1;
            // 
            // tbLocations
            // 
            this.tbLocations.Controls.Add(this.lvLocByWU);
            this.tbLocations.Controls.Add(this.lvLocations);
            this.tbLocations.Location = new System.Drawing.Point(4, 22);
            this.tbLocations.Name = "tbLocations";
            this.tbLocations.Padding = new System.Windows.Forms.Padding(3);
            this.tbLocations.Size = new System.Drawing.Size(962, 546);
            this.tbLocations.TabIndex = 0;
            this.tbLocations.Text = "Locations";
            this.tbLocations.UseVisualStyleBackColor = true;
            // 
            // lvLocByWU
            // 
            this.lvLocByWU.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lvLocByWU.FullRowSelect = true;
            this.lvLocByWU.GridLines = true;
            this.lvLocByWU.HideSelection = false;
            this.lvLocByWU.Location = new System.Drawing.Point(516, 5);
            this.lvLocByWU.Name = "lvLocByWU";
            this.lvLocByWU.Size = new System.Drawing.Size(440, 535);
            this.lvLocByWU.TabIndex = 1;
            this.lvLocByWU.UseCompatibleStateImageBehavior = false;
            this.lvLocByWU.View = System.Windows.Forms.View.Details;
            // 
            // lvLocations
            // 
            this.lvLocations.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lvLocations.FullRowSelect = true;
            this.lvLocations.GridLines = true;
            this.lvLocations.HideSelection = false;
            this.lvLocations.Location = new System.Drawing.Point(6, 5);
            this.lvLocations.MultiSelect = false;
            this.lvLocations.Name = "lvLocations";
            this.lvLocations.Size = new System.Drawing.Size(440, 535);
            this.lvLocations.TabIndex = 0;
            this.lvLocations.UseCompatibleStateImageBehavior = false;
            this.lvLocations.View = System.Windows.Forms.View.Details;
            this.lvLocations.SelectedIndexChanged += new System.EventHandler(this.lvLocations_SelectedIndexChanged);
            // 
            // tbWU
            // 
            this.tbWU.Controls.Add(this.lvWUByLoc);
            this.tbWU.Controls.Add(this.lvWU);
            this.tbWU.Location = new System.Drawing.Point(4, 22);
            this.tbWU.Name = "tbWU";
            this.tbWU.Padding = new System.Windows.Forms.Padding(3);
            this.tbWU.Size = new System.Drawing.Size(962, 546);
            this.tbWU.TabIndex = 1;
            this.tbWU.Text = "Working units";
            this.tbWU.UseVisualStyleBackColor = true;
            // 
            // lvWUByLoc
            // 
            this.lvWUByLoc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lvWUByLoc.FullRowSelect = true;
            this.lvWUByLoc.GridLines = true;
            this.lvWUByLoc.HideSelection = false;
            this.lvWUByLoc.Location = new System.Drawing.Point(516, 6);
            this.lvWUByLoc.Name = "lvWUByLoc";
            this.lvWUByLoc.Size = new System.Drawing.Size(440, 535);
            this.lvWUByLoc.TabIndex = 3;
            this.lvWUByLoc.UseCompatibleStateImageBehavior = false;
            this.lvWUByLoc.View = System.Windows.Forms.View.Details;
            // 
            // lvWU
            // 
            this.lvWU.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lvWU.FullRowSelect = true;
            this.lvWU.GridLines = true;
            this.lvWU.HideSelection = false;
            this.lvWU.Location = new System.Drawing.Point(6, 6);
            this.lvWU.MultiSelect = false;
            this.lvWU.Name = "lvWU";
            this.lvWU.Size = new System.Drawing.Size(440, 535);
            this.lvWU.TabIndex = 2;
            this.lvWU.UseCompatibleStateImageBehavior = false;
            this.lvWU.View = System.Windows.Forms.View.Details;
            this.lvWU.SelectedIndexChanged += new System.EventHandler(this.lvWU_SelectedIndexChanged);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(12, 590);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lblPrint
            // 
            this.lblPrint.AutoSize = true;
            this.lblPrint.Location = new System.Drawing.Point(367, 595);
            this.lblPrint.Name = "lblPrint";
            this.lblPrint.Size = new System.Drawing.Size(95, 13);
            this.lblPrint.TabIndex = 5;
            this.lblPrint.Text = "login user datetime";
            this.lblPrint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(106, 590);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(75, 23);
            this.btnReport.TabIndex = 7;
            this.btnReport.Text = "Report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // EmployeeLocationsSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 618);
            this.ControlBox = false;
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.lblPrint);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "EmployeeLocationsSummary";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Employee locations summary preview";
            this.Load += new System.EventHandler(this.EmployeeLocationsSummary_Load);
            this.tabControl1.ResumeLayout(false);
            this.tbLocations.ResumeLayout(false);
            this.tbWU.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbLocations;
        private System.Windows.Forms.TabPage tbWU;
        private System.Windows.Forms.ListView lvLocByWU;
        private System.Windows.Forms.ListView lvLocations;
        private System.Windows.Forms.ListView lvWUByLoc;
        private System.Windows.Forms.ListView lvWU;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblPrint;
        private System.Windows.Forms.Button btnReport;
    }
}