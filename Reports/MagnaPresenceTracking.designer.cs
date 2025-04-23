namespace Reports.Magna
{
    partial class MagnaPresenceTracking
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
            this.gbOverview = new System.Windows.Forms.GroupBox();
            this.rbNumOfHours = new System.Windows.Forms.RadioButton();
            this.rbPresence = new System.Windows.Forms.RadioButton();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.chbShowRetired = new System.Windows.Forms.CheckBox();
            this.gbFor = new System.Windows.Forms.GroupBox();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.lblFor = new System.Windows.Forms.Label();
            this.gbWorkingUnit = new System.Windows.Forms.GroupBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.cbCR = new System.Windows.Forms.CheckBox();
            this.cbPDF = new System.Windows.Forms.CheckBox();
            this.lblReportType = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.gbOverview.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.gbFor.SuspendLayout();
            this.gbWorkingUnit.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbOverview
            // 
            this.gbOverview.Controls.Add(this.rbNumOfHours);
            this.gbOverview.Controls.Add(this.rbPresence);
            this.gbOverview.Location = new System.Drawing.Point(354, 134);
            this.gbOverview.Name = "gbOverview";
            this.gbOverview.Size = new System.Drawing.Size(137, 64);
            this.gbOverview.TabIndex = 43;
            this.gbOverview.TabStop = false;
            this.gbOverview.Text = "Overview";
            // 
            // rbNumOfHours
            // 
            this.rbNumOfHours.AutoSize = true;
            this.rbNumOfHours.Checked = true;
            this.rbNumOfHours.Location = new System.Drawing.Point(6, 41);
            this.rbNumOfHours.Name = "rbNumOfHours";
            this.rbNumOfHours.Size = new System.Drawing.Size(103, 17);
            this.rbNumOfHours.TabIndex = 9;
            this.rbNumOfHours.TabStop = true;
            this.rbNumOfHours.Text = "Number of hours";
            this.rbNumOfHours.UseVisualStyleBackColor = true;
            // 
            // rbPresence
            // 
            this.rbPresence.AutoSize = true;
            this.rbPresence.Location = new System.Drawing.Point(6, 18);
            this.rbPresence.Name = "rbPresence";
            this.rbPresence.Size = new System.Drawing.Size(85, 17);
            this.rbPresence.TabIndex = 8;
            this.rbPresence.TabStop = true;
            this.rbPresence.Text = "+ (Presence)";
            this.rbPresence.UseVisualStyleBackColor = true;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(354, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 106);
            this.gbFilter.TabIndex = 42;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(32, 56);
            this.btnSaveCriteria.Name = "btnSaveCriteria";
            this.btnSaveCriteria.Size = new System.Drawing.Size(82, 23);
            this.btnSaveCriteria.TabIndex = 16;
            this.btnSaveCriteria.Text = "Save criteria";
            this.btnSaveCriteria.UseVisualStyleBackColor = true;
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(6, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            // 
            // chbShowRetired
            // 
            this.chbShowRetired.AutoSize = true;
            this.chbShowRetired.Location = new System.Drawing.Point(322, 226);
            this.chbShowRetired.Name = "chbShowRetired";
            this.chbShowRetired.Size = new System.Drawing.Size(85, 17);
            this.chbShowRetired.TabIndex = 41;
            this.chbShowRetired.Tag = "FILTERABLE";
            this.chbShowRetired.Text = "Show retired";
            this.chbShowRetired.UseVisualStyleBackColor = true;
            // 
            // gbFor
            // 
            this.gbFor.Controls.Add(this.dtTo);
            this.gbFor.Controls.Add(this.lblFor);
            this.gbFor.Location = new System.Drawing.Point(12, 134);
            this.gbFor.Name = "gbFor";
            this.gbFor.Size = new System.Drawing.Size(330, 64);
            this.gbFor.TabIndex = 35;
            this.gbFor.TabStop = false;
            this.gbFor.Tag = "FILTERABLE";
            this.gbFor.Text = "For";
            // 
            // dtTo
            // 
            this.dtTo.Checked = false;
            this.dtTo.CustomFormat = "MM.yyyy";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(103, 24);
            this.dtTo.MinDate = new System.DateTime(2001, 1, 1, 0, 0, 0, 0);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(88, 20);
            this.dtTo.TabIndex = 7;
            this.dtTo.Value = new System.DateTime(2006, 9, 1, 12, 21, 9, 62);
            // 
            // lblFor
            // 
            this.lblFor.Location = new System.Drawing.Point(16, 24);
            this.lblFor.Name = "lblFor";
            this.lblFor.Size = new System.Drawing.Size(70, 23);
            this.lblFor.TabIndex = 6;
            this.lblFor.Text = "For:";
            this.lblFor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbWorkingUnit
            // 
            this.gbWorkingUnit.Controls.Add(this.chbHierarhicly);
            this.gbWorkingUnit.Controls.Add(this.cbWorkingUnit);
            this.gbWorkingUnit.Controls.Add(this.lblWorkingUnit);
            this.gbWorkingUnit.Location = new System.Drawing.Point(12, 12);
            this.gbWorkingUnit.Name = "gbWorkingUnit";
            this.gbWorkingUnit.Size = new System.Drawing.Size(330, 106);
            this.gbWorkingUnit.TabIndex = 34;
            this.gbWorkingUnit.TabStop = false;
            this.gbWorkingUnit.Tag = "FILTERABLE";
            this.gbWorkingUnit.Text = "Working Units";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(80, 62);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 4;
            this.chbHierarhicly.Text = "Hierarchy ";
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(80, 24);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(238, 21);
            this.cbWorkingUnit.TabIndex = 3;
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(16, 24);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(48, 23);
            this.lblWorkingUnit.TabIndex = 2;
            this.lblWorkingUnit.Text = "Name:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbCR
            // 
            this.cbCR.Checked = true;
            this.cbCR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCR.Enabled = false;
            this.cbCR.Location = new System.Drawing.Point(252, 222);
            this.cbCR.Name = "cbCR";
            this.cbCR.Size = new System.Drawing.Size(64, 24);
            this.cbCR.TabIndex = 38;
            this.cbCR.Tag = "FILTERABLE";
            this.cbCR.Text = "CR";
            // 
            // cbPDF
            // 
            this.cbPDF.Enabled = false;
            this.cbPDF.Location = new System.Drawing.Point(156, 222);
            this.cbPDF.Name = "cbPDF";
            this.cbPDF.Size = new System.Drawing.Size(56, 24);
            this.cbPDF.TabIndex = 37;
            this.cbPDF.Tag = "FILTERABLE";
            this.cbPDF.Text = "PDF";
            this.cbPDF.Visible = false;
            // 
            // lblReportType
            // 
            this.lblReportType.Location = new System.Drawing.Point(12, 222);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(104, 23);
            this.lblReportType.TabIndex = 36;
            this.lblReportType.Tag = "FILTERABLE";
            this.lblReportType.Text = "Document format:";
            this.lblReportType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(416, 281);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 40;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(12, 281);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(128, 23);
            this.btnGenerate.TabIndex = 39;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(228, 281);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 44;
            this.button1.Text = "EXCEL";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MagnaPresenceTracking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 334);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gbOverview);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.chbShowRetired);
            this.Controls.Add(this.gbFor);
            this.Controls.Add(this.gbWorkingUnit);
            this.Controls.Add(this.cbCR);
            this.Controls.Add(this.cbPDF);
            this.Controls.Add(this.lblReportType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Name = "MagnaPresenceTracking";
            this.Text = "MagnaPresenceTracking";
            this.Load += new System.EventHandler(this.MagnaPresenceTracking_Load);
            this.gbOverview.ResumeLayout(false);
            this.gbOverview.PerformLayout();
            this.gbFilter.ResumeLayout(false);
            this.gbFor.ResumeLayout(false);
            this.gbWorkingUnit.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbOverview;
        private System.Windows.Forms.RadioButton rbNumOfHours;
        private System.Windows.Forms.RadioButton rbPresence;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnSaveCriteria;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.CheckBox chbShowRetired;
        private System.Windows.Forms.GroupBox gbFor;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Label lblFor;
        private System.Windows.Forms.GroupBox gbWorkingUnit;
        private System.Windows.Forms.CheckBox chbHierarhicly;
        private System.Windows.Forms.ComboBox cbWorkingUnit;
        private System.Windows.Forms.Label lblWorkingUnit;
        private System.Windows.Forms.CheckBox cbCR;
        private System.Windows.Forms.CheckBox cbPDF;
        private System.Windows.Forms.Label lblReportType;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button button1;
    }
}