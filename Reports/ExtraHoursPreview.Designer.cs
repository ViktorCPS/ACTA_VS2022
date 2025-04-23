namespace Reports
{
    partial class ExtraHoursPreview
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
            this.lblEmpl = new System.Windows.Forms.Label();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.lblToDate = new System.Windows.Forms.Label();
            this.cbEmpl = new System.Windows.Forms.ComboBox();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblWU = new System.Windows.Forms.Label();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.gbCriteria = new System.Windows.Forms.GroupBox();
            this.gbReport = new System.Windows.Forms.GroupBox();
            this.rbUsedHrs = new System.Windows.Forms.RadioButton();
            this.rbEarnedHrs = new System.Windows.Forms.RadioButton();
            this.lblRepType = new System.Windows.Forms.Label();
            this.rbAvaiableHrs = new System.Windows.Forms.RadioButton();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.chbHierarchy = new System.Windows.Forms.CheckBox();
            this.gbCriteria.SuspendLayout();
            this.gbReport.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEmpl
            // 
            this.lblEmpl.Location = new System.Drawing.Point(20, 57);
            this.lblEmpl.Name = "lblEmpl";
            this.lblEmpl.Size = new System.Drawing.Size(81, 17);
            this.lblEmpl.TabIndex = 3;
            this.lblEmpl.Text = "Employee:";
            this.lblEmpl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFromDate
            // 
            this.lblFromDate.Location = new System.Drawing.Point(52, 94);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(44, 17);
            this.lblFromDate.TabIndex = 9;
            this.lblFromDate.Text = "From:";
            this.lblFromDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblToDate
            // 
            this.lblToDate.Location = new System.Drawing.Point(55, 127);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(41, 17);
            this.lblToDate.TabIndex = 11;
            this.lblToDate.Text = "To:";
            this.lblToDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmpl
            // 
            this.cbEmpl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmpl.FormattingEnabled = true;
            this.cbEmpl.Location = new System.Drawing.Point(102, 56);
            this.cbEmpl.Name = "cbEmpl";
            this.cbEmpl.Size = new System.Drawing.Size(158, 21);
            this.cbEmpl.TabIndex = 4;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd.MM.yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(102, 92);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(125, 20);
            this.dtpFromDate.TabIndex = 10;
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd.MM.yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(102, 125);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(125, 20);
            this.dtpToDate.TabIndex = 12;
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(6, 24);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(95, 17);
            this.lblWU.TabIndex = 1;
            this.lblWU.Text = "Working unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(102, 23);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(158, 21);
            this.cbWU.TabIndex = 2;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // gbCriteria
            // 
            this.gbCriteria.Controls.Add(this.chbHierarchy);
            this.gbCriteria.Controls.Add(this.cbWU);
            this.gbCriteria.Controls.Add(this.lblWU);
            this.gbCriteria.Controls.Add(this.dtpToDate);
            this.gbCriteria.Controls.Add(this.dtpFromDate);
            this.gbCriteria.Controls.Add(this.cbEmpl);
            this.gbCriteria.Controls.Add(this.lblToDate);
            this.gbCriteria.Controls.Add(this.lblFromDate);
            this.gbCriteria.Controls.Add(this.lblEmpl);
            this.gbCriteria.Location = new System.Drawing.Point(12, 12);
            this.gbCriteria.Name = "gbCriteria";
            this.gbCriteria.Size = new System.Drawing.Size(414, 181);
            this.gbCriteria.TabIndex = 9;
            this.gbCriteria.TabStop = false;
            this.gbCriteria.Tag = "FILTERABLE";
            this.gbCriteria.Text = "Criteria";
            // 
            // gbReport
            // 
            this.gbReport.Controls.Add(this.cbType);
            this.gbReport.Controls.Add(this.lblType);
            this.gbReport.Controls.Add(this.rbAvaiableHrs);
            this.gbReport.Controls.Add(this.rbUsedHrs);
            this.gbReport.Controls.Add(this.rbEarnedHrs);
            this.gbReport.Controls.Add(this.lblRepType);
            this.gbReport.Location = new System.Drawing.Point(12, 199);
            this.gbReport.Name = "gbReport";
            this.gbReport.Size = new System.Drawing.Size(557, 119);
            this.gbReport.TabIndex = 14;
            this.gbReport.TabStop = false;
            this.gbReport.Tag = "FILTERABLE";
            this.gbReport.Text = "Report";
            // 
            // rbUsedHrs
            // 
            this.rbUsedHrs.AutoSize = true;
            this.rbUsedHrs.Location = new System.Drawing.Point(102, 58);
            this.rbUsedHrs.Name = "rbUsedHrs";
            this.rbUsedHrs.Size = new System.Drawing.Size(79, 17);
            this.rbUsedHrs.TabIndex = 16;
            this.rbUsedHrs.TabStop = true;
            this.rbUsedHrs.Text = "Used hours";
            this.rbUsedHrs.UseVisualStyleBackColor = true;
            this.rbUsedHrs.CheckedChanged += new System.EventHandler(this.rbUsedHrs_CheckedChanged);
            // 
            // rbEarnedHrs
            // 
            this.rbEarnedHrs.AutoSize = true;
            this.rbEarnedHrs.Location = new System.Drawing.Point(103, 23);
            this.rbEarnedHrs.Name = "rbEarnedHrs";
            this.rbEarnedHrs.Size = new System.Drawing.Size(88, 17);
            this.rbEarnedHrs.TabIndex = 15;
            this.rbEarnedHrs.TabStop = true;
            this.rbEarnedHrs.Text = "Earned hours";
            this.rbEarnedHrs.UseVisualStyleBackColor = true;
            // 
            // lblRepType
            // 
            this.lblRepType.Location = new System.Drawing.Point(7, 22);
            this.lblRepType.Name = "lblRepType";
            this.lblRepType.Size = new System.Drawing.Size(84, 17);
            this.lblRepType.TabIndex = 14;
            this.lblRepType.Text = "Report type:";
            this.lblRepType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rbAvaiableHrs
            // 
            this.rbAvaiableHrs.AutoSize = true;
            this.rbAvaiableHrs.Location = new System.Drawing.Point(102, 89);
            this.rbAvaiableHrs.Name = "rbAvaiableHrs";
            this.rbAvaiableHrs.Size = new System.Drawing.Size(95, 17);
            this.rbAvaiableHrs.TabIndex = 17;
            this.rbAvaiableHrs.TabStop = true;
            this.rbAvaiableHrs.Text = "Avaiable hours";
            this.rbAvaiableHrs.UseVisualStyleBackColor = true;
            this.rbAvaiableHrs.CheckedChanged += new System.EventHandler(this.rbAvaiableHrs_CheckedChanged);
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.Enabled = false;
            this.cbType.Location = new System.Drawing.Point(250, 57);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(224, 21);
            this.cbType.TabIndex = 19;
            // 
            // lblType
            // 
            this.lblType.Enabled = false;
            this.lblType.Location = new System.Drawing.Point(187, 57);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(57, 23);
            this.lblType.TabIndex = 18;
            this.lblType.Text = "Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(432, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 106);
            this.gbFilter.TabIndex = 34;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(27, 66);
            this.btnSaveCriteria.Name = "btnSaveCriteria";
            this.btnSaveCriteria.Size = new System.Drawing.Size(82, 23);
            this.btnSaveCriteria.TabIndex = 16;
            this.btnSaveCriteria.Text = "Save criteria";
            this.btnSaveCriteria.UseVisualStyleBackColor = true;
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(5, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(12, 392);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(134, 23);
            this.btnGenerate.TabIndex = 35;
            this.btnGenerate.Text = "Generate report";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(494, 392);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 36;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chbHierarchy
            // 
            this.chbHierarchy.Location = new System.Drawing.Point(266, 21);
            this.chbHierarchy.Name = "chbHierarchy";
            this.chbHierarchy.Size = new System.Drawing.Size(104, 24);
            this.chbHierarchy.TabIndex = 48;
            this.chbHierarchy.Text = "Hierarchy ";
            this.chbHierarchy.CheckedChanged += new System.EventHandler(this.chbHierarchy_CheckedChanged);
            // 
            // ExtraHoursPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 427);
            this.ControlBox = false;
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.gbReport);
            this.Controls.Add(this.gbCriteria);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ExtraHoursPreview";
            this.ShowIcon = false;
            this.Text = "ExtraHoursPreview";
            this.Load += new System.EventHandler(this.ExtraHoursPreview_Load);
            this.gbCriteria.ResumeLayout(false);
            this.gbReport.ResumeLayout(false);
            this.gbReport.PerformLayout();
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblEmpl;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.ComboBox cbEmpl;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.GroupBox gbCriteria;
        private System.Windows.Forms.GroupBox gbReport;
        private System.Windows.Forms.RadioButton rbAvaiableHrs;
        private System.Windows.Forms.RadioButton rbUsedHrs;
        private System.Windows.Forms.RadioButton rbEarnedHrs;
        private System.Windows.Forms.Label lblRepType;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnSaveCriteria;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chbHierarchy;

    }
}