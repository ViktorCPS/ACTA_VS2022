namespace Reports.PMC
{
    partial class PMCCumulativeReports
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
            this.gbReportType = new System.Windows.Forms.GroupBox();
            this.rbOvertime = new System.Windows.Forms.RadioButton();
            this.rbAbsenceByType = new System.Windows.Forms.RadioButton();
            this.rbAbsence = new System.Windows.Forms.RadioButton();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.gbUnitFilter = new System.Windows.Forms.GroupBox();
            this.chbHierarhiclyWU = new System.Windows.Forms.CheckBox();
            this.cbOU = new System.Windows.Forms.ComboBox();
            this.chbHierachyOU = new System.Windows.Forms.CheckBox();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.rbOU = new System.Windows.Forms.RadioButton();
            this.rbWU = new System.Windows.Forms.RadioButton();
            this.tbEmployee = new System.Windows.Forms.TextBox();
            this.gbDateInterval = new System.Windows.Forms.GroupBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbCategory = new System.Windows.Forms.GroupBox();
            this.lvCategory = new System.Windows.Forms.ListView();
            this.rbDelay = new System.Windows.Forms.RadioButton();
            this.gbReportType.SuspendLayout();
            this.gbUnitFilter.SuspendLayout();
            this.gbDateInterval.SuspendLayout();
            this.gbCategory.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbReportType
            // 
            this.gbReportType.Controls.Add(this.rbDelay);
            this.gbReportType.Controls.Add(this.rbOvertime);
            this.gbReportType.Controls.Add(this.rbAbsenceByType);
            this.gbReportType.Controls.Add(this.rbAbsence);
            this.gbReportType.Location = new System.Drawing.Point(12, 238);
            this.gbReportType.Name = "gbReportType";
            this.gbReportType.Size = new System.Drawing.Size(158, 127);
            this.gbReportType.TabIndex = 5;
            this.gbReportType.TabStop = false;
            this.gbReportType.Text = "Report type";
            // 
            // rbOvertime
            // 
            this.rbOvertime.AutoSize = true;
            this.rbOvertime.Location = new System.Drawing.Point(16, 70);
            this.rbOvertime.Name = "rbOvertime";
            this.rbOvertime.Size = new System.Drawing.Size(96, 17);
            this.rbOvertime.TabIndex = 2;
            this.rbOvertime.TabStop = true;
            this.rbOvertime.Text = "Overtime hours";
            this.rbOvertime.UseVisualStyleBackColor = true;
            // 
            // rbAbsenceByType
            // 
            this.rbAbsenceByType.AutoSize = true;
            this.rbAbsenceByType.Location = new System.Drawing.Point(16, 47);
            this.rbAbsenceByType.Name = "rbAbsenceByType";
            this.rbAbsenceByType.Size = new System.Drawing.Size(104, 17);
            this.rbAbsenceByType.TabIndex = 1;
            this.rbAbsenceByType.TabStop = true;
            this.rbAbsenceByType.Text = "Absence by type";
            this.rbAbsenceByType.UseVisualStyleBackColor = true;
            // 
            // rbAbsence
            // 
            this.rbAbsence.AutoSize = true;
            this.rbAbsence.Location = new System.Drawing.Point(16, 24);
            this.rbAbsence.Name = "rbAbsence";
            this.rbAbsence.Size = new System.Drawing.Size(67, 17);
            this.rbAbsence.TabIndex = 0;
            this.rbAbsence.TabStop = true;
            this.rbAbsence.Text = "Absence";
            this.rbAbsence.UseVisualStyleBackColor = true;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(442, 15);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(78, 23);
            this.lblEmployee.TabIndex = 1;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbUnitFilter
            // 
            this.gbUnitFilter.Controls.Add(this.chbHierarhiclyWU);
            this.gbUnitFilter.Controls.Add(this.cbOU);
            this.gbUnitFilter.Controls.Add(this.chbHierachyOU);
            this.gbUnitFilter.Controls.Add(this.cbWU);
            this.gbUnitFilter.Controls.Add(this.rbOU);
            this.gbUnitFilter.Controls.Add(this.rbWU);
            this.gbUnitFilter.Location = new System.Drawing.Point(12, 12);
            this.gbUnitFilter.Name = "gbUnitFilter";
            this.gbUnitFilter.Size = new System.Drawing.Size(415, 134);
            this.gbUnitFilter.TabIndex = 0;
            this.gbUnitFilter.TabStop = false;
            this.gbUnitFilter.Text = "Unit filter";
            // 
            // chbHierarhiclyWU
            // 
            this.chbHierarhiclyWU.Location = new System.Drawing.Point(66, 46);
            this.chbHierarhiclyWU.Name = "chbHierarhiclyWU";
            this.chbHierarhiclyWU.Size = new System.Drawing.Size(83, 24);
            this.chbHierarhiclyWU.TabIndex = 2;
            this.chbHierarhiclyWU.Text = "Hierarchy ";
            this.chbHierarhiclyWU.CheckedChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // cbOU
            // 
            this.cbOU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbOU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbOU.FormattingEnabled = true;
            this.cbOU.Location = new System.Drawing.Point(66, 76);
            this.cbOU.Name = "cbOU";
            this.cbOU.Size = new System.Drawing.Size(239, 21);
            this.cbOU.TabIndex = 4;
            this.cbOU.SelectedIndexChanged += new System.EventHandler(this.cbOU_SelectedIndexChanged);
            // 
            // chbHierachyOU
            // 
            this.chbHierachyOU.Location = new System.Drawing.Point(66, 103);
            this.chbHierachyOU.Name = "chbHierachyOU";
            this.chbHierachyOU.Size = new System.Drawing.Size(83, 24);
            this.chbHierachyOU.TabIndex = 5;
            this.chbHierachyOU.Text = "Hierarchy ";
            this.chbHierachyOU.CheckedChanged += new System.EventHandler(this.chbHierachyOU_CheckedChanged);
            // 
            // cbWU
            // 
            this.cbWU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbWU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(66, 19);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(239, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // rbOU
            // 
            this.rbOU.AutoSize = true;
            this.rbOU.Location = new System.Drawing.Point(17, 77);
            this.rbOU.Name = "rbOU";
            this.rbOU.Size = new System.Drawing.Size(41, 17);
            this.rbOU.TabIndex = 3;
            this.rbOU.TabStop = true;
            this.rbOU.Text = "OU";
            this.rbOU.UseVisualStyleBackColor = true;
            this.rbOU.CheckedChanged += new System.EventHandler(this.rbOU_CheckedChanged);
            // 
            // rbWU
            // 
            this.rbWU.AutoSize = true;
            this.rbWU.Location = new System.Drawing.Point(17, 20);
            this.rbWU.Name = "rbWU";
            this.rbWU.Size = new System.Drawing.Size(38, 17);
            this.rbWU.TabIndex = 0;
            this.rbWU.TabStop = true;
            this.rbWU.Text = "FS";
            this.rbWU.UseVisualStyleBackColor = true;
            this.rbWU.CheckedChanged += new System.EventHandler(this.rbWU_CheckedChanged);
            // 
            // tbEmployee
            // 
            this.tbEmployee.Location = new System.Drawing.Point(526, 17);
            this.tbEmployee.Name = "tbEmployee";
            this.tbEmployee.Size = new System.Drawing.Size(214, 20);
            this.tbEmployee.TabIndex = 2;
            this.tbEmployee.TextChanged += new System.EventHandler(this.tbEmployee_TextChanged);
            // 
            // gbDateInterval
            // 
            this.gbDateInterval.Controls.Add(this.dtpFrom);
            this.gbDateInterval.Controls.Add(this.lblFrom);
            this.gbDateInterval.Controls.Add(this.dtpTo);
            this.gbDateInterval.Controls.Add(this.lblTo);
            this.gbDateInterval.Location = new System.Drawing.Point(12, 152);
            this.gbDateInterval.Name = "gbDateInterval";
            this.gbDateInterval.Size = new System.Drawing.Size(158, 80);
            this.gbDateInterval.TabIndex = 4;
            this.gbDateInterval.TabStop = false;
            this.gbDateInterval.Text = "Date interval";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(48, 19);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(101, 20);
            this.dtpFrom.TabIndex = 1;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(6, 16);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 23);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(48, 45);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(101, 20);
            this.dtpTo.TabIndex = 3;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(21, 42);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(25, 23);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(445, 43);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.ShowItemToolTips = true;
            this.lvEmployees.Size = new System.Drawing.Size(295, 322);
            this.lvEmployees.TabIndex = 3;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployees_ColumnClick);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(12, 385);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(114, 23);
            this.btnGenerate.TabIndex = 7;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(665, 385);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbCategory
            // 
            this.gbCategory.Controls.Add(this.lvCategory);
            this.gbCategory.Location = new System.Drawing.Point(176, 152);
            this.gbCategory.Name = "gbCategory";
            this.gbCategory.Size = new System.Drawing.Size(251, 213);
            this.gbCategory.TabIndex = 6;
            this.gbCategory.TabStop = false;
            this.gbCategory.Text = "Category";
            // 
            // lvCategory
            // 
            this.lvCategory.FullRowSelect = true;
            this.lvCategory.GridLines = true;
            this.lvCategory.HideSelection = false;
            this.lvCategory.Location = new System.Drawing.Point(15, 19);
            this.lvCategory.Name = "lvCategory";
            this.lvCategory.ShowItemToolTips = true;
            this.lvCategory.Size = new System.Drawing.Size(219, 177);
            this.lvCategory.TabIndex = 0;
            this.lvCategory.UseCompatibleStateImageBehavior = false;
            this.lvCategory.View = System.Windows.Forms.View.Details;
            // 
            // rbDelay
            // 
            this.rbDelay.AutoSize = true;
            this.rbDelay.Location = new System.Drawing.Point(16, 93);
            this.rbDelay.Name = "rbDelay";
            this.rbDelay.Size = new System.Drawing.Size(52, 17);
            this.rbDelay.TabIndex = 3;
            this.rbDelay.TabStop = true;
            this.rbDelay.Text = "Delay";
            this.rbDelay.UseVisualStyleBackColor = true;
            // 
            // PMCCumulativeReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 431);
            this.ControlBox = false;
            this.Controls.Add(this.gbCategory);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblEmployee);
            this.Controls.Add(this.gbReportType);
            this.Controls.Add(this.gbUnitFilter);
            this.Controls.Add(this.tbEmployee);
            this.Controls.Add(this.lvEmployees);
            this.Controls.Add(this.gbDateInterval);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PMCCumulativeReports";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Absence reports";
            this.Load += new System.EventHandler(this.PMCCumulativeReports_Load);
            this.gbReportType.ResumeLayout(false);
            this.gbReportType.PerformLayout();
            this.gbUnitFilter.ResumeLayout(false);
            this.gbUnitFilter.PerformLayout();
            this.gbDateInterval.ResumeLayout(false);
            this.gbCategory.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbReportType;
        private System.Windows.Forms.RadioButton rbAbsenceByType;
        private System.Windows.Forms.RadioButton rbAbsence;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.GroupBox gbUnitFilter;
        private System.Windows.Forms.CheckBox chbHierarhiclyWU;
        private System.Windows.Forms.ComboBox cbOU;
        private System.Windows.Forms.CheckBox chbHierachyOU;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.RadioButton rbOU;
        private System.Windows.Forms.RadioButton rbWU;
        private System.Windows.Forms.TextBox tbEmployee;
        private System.Windows.Forms.GroupBox gbDateInterval;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbCategory;
        private System.Windows.Forms.ListView lvCategory;
        private System.Windows.Forms.RadioButton rbOvertime;
        private System.Windows.Forms.RadioButton rbDelay;
    }
}