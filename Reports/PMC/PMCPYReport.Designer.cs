namespace Reports.PMC
{
    partial class PMCPYReport
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabReport = new System.Windows.Forms.TabPage();
            this.btnGenerateAgency = new System.Windows.Forms.Button();
            this.dtpMonth = new System.Windows.Forms.DateTimePicker();
            this.lblMonth = new System.Windows.Forms.Label();
            this.gbCategory = new System.Windows.Forms.GroupBox();
            this.lvCategory = new System.Windows.Forms.ListView();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.gbUnitFilter = new System.Windows.Forms.GroupBox();
            this.chbHierarhiclyWU = new System.Windows.Forms.CheckBox();
            this.chbHierachyOU = new System.Windows.Forms.CheckBox();
            this.cbOU = new System.Windows.Forms.ComboBox();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.rbOU = new System.Windows.Forms.RadioButton();
            this.rbWU = new System.Windows.Forms.RadioButton();
            this.tbEmployee = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.tabCounters = new System.Windows.Forms.TabPage();
            this.lblCalcID = new System.Windows.Forms.Label();
            this.btnRecalculate = new System.Windows.Forms.Button();
            this.nudCalcID = new System.Windows.Forms.NumericUpDown();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabReport.SuspendLayout();
            this.gbCategory.SuspendLayout();
            this.gbUnitFilter.SuspendLayout();
            this.tabCounters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCalcID)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabReport);
            this.tabControl1.Controls.Add(this.tabCounters);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(770, 397);
            this.tabControl1.TabIndex = 0;
            // 
            // tabReport
            // 
            this.tabReport.Controls.Add(this.btnGenerateAgency);
            this.tabReport.Controls.Add(this.dtpMonth);
            this.tabReport.Controls.Add(this.lblMonth);
            this.tabReport.Controls.Add(this.gbCategory);
            this.tabReport.Controls.Add(this.lblEmployee);
            this.tabReport.Controls.Add(this.gbUnitFilter);
            this.tabReport.Controls.Add(this.tbEmployee);
            this.tabReport.Controls.Add(this.btnGenerate);
            this.tabReport.Controls.Add(this.lvEmployees);
            this.tabReport.Location = new System.Drawing.Point(4, 22);
            this.tabReport.Name = "tabReport";
            this.tabReport.Padding = new System.Windows.Forms.Padding(3);
            this.tabReport.Size = new System.Drawing.Size(762, 371);
            this.tabReport.TabIndex = 0;
            this.tabReport.Text = "Report";
            this.tabReport.UseVisualStyleBackColor = true;
            // 
            // btnGenerateAgency
            // 
            this.btnGenerateAgency.Location = new System.Drawing.Point(249, 335);
            this.btnGenerateAgency.Name = "btnGenerateAgency";
            this.btnGenerateAgency.Size = new System.Drawing.Size(184, 23);
            this.btnGenerateAgency.TabIndex = 9;
            this.btnGenerateAgency.Text = "Generate agency report";
            this.btnGenerateAgency.UseVisualStyleBackColor = true;
            this.btnGenerateAgency.Click += new System.EventHandler(this.btnGenerateAgency_Click);
            // 
            // dtpMonth
            // 
            this.dtpMonth.CustomFormat = "MM.yyyy";
            this.dtpMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMonth.Location = new System.Drawing.Point(73, 162);
            this.dtpMonth.Name = "dtpMonth";
            this.dtpMonth.Size = new System.Drawing.Size(85, 20);
            this.dtpMonth.TabIndex = 5;
            this.dtpMonth.ValueChanged += new System.EventHandler(this.dtpMonth_ValueChanged);
            // 
            // lblMonth
            // 
            this.lblMonth.Location = new System.Drawing.Point(13, 159);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(54, 23);
            this.lblMonth.TabIndex = 4;
            this.lblMonth.Text = "Month:";
            this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbCategory
            // 
            this.gbCategory.Controls.Add(this.lvCategory);
            this.gbCategory.Location = new System.Drawing.Point(182, 143);
            this.gbCategory.Name = "gbCategory";
            this.gbCategory.Size = new System.Drawing.Size(251, 166);
            this.gbCategory.TabIndex = 7;
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
            this.lvCategory.Size = new System.Drawing.Size(219, 131);
            this.lvCategory.TabIndex = 0;
            this.lvCategory.UseCompatibleStateImageBehavior = false;
            this.lvCategory.View = System.Windows.Forms.View.Details;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(439, 7);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(78, 23);
            this.lblEmployee.TabIndex = 1;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbUnitFilter
            // 
            this.gbUnitFilter.Controls.Add(this.chbHierarhiclyWU);
            this.gbUnitFilter.Controls.Add(this.chbHierachyOU);
            this.gbUnitFilter.Controls.Add(this.cbOU);
            this.gbUnitFilter.Controls.Add(this.cbWU);
            this.gbUnitFilter.Controls.Add(this.rbOU);
            this.gbUnitFilter.Controls.Add(this.rbWU);
            this.gbUnitFilter.Location = new System.Drawing.Point(6, 6);
            this.gbUnitFilter.Name = "gbUnitFilter";
            this.gbUnitFilter.Size = new System.Drawing.Size(427, 133);
            this.gbUnitFilter.TabIndex = 0;
            this.gbUnitFilter.TabStop = false;
            this.gbUnitFilter.Text = "Unit filter";
            // 
            // chbHierarhiclyWU
            // 
            this.chbHierarhiclyWU.Location = new System.Drawing.Point(69, 45);
            this.chbHierarhiclyWU.Name = "chbHierarhiclyWU";
            this.chbHierarhiclyWU.Size = new System.Drawing.Size(83, 24);
            this.chbHierarhiclyWU.TabIndex = 2;
            this.chbHierarhiclyWU.Text = "Hierarchy ";
            this.chbHierarhiclyWU.CheckedChanged += new System.EventHandler(this.chbHierarhiclyWU_CheckedChanged);
            // 
            // chbHierachyOU
            // 
            this.chbHierachyOU.Location = new System.Drawing.Point(69, 102);
            this.chbHierachyOU.Name = "chbHierachyOU";
            this.chbHierachyOU.Size = new System.Drawing.Size(83, 24);
            this.chbHierachyOU.TabIndex = 5;
            this.chbHierachyOU.Text = "Hierarchy ";
            this.chbHierachyOU.CheckedChanged += new System.EventHandler(this.chbHierachyOU_CheckedChanged);
            // 
            // cbOU
            // 
            this.cbOU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbOU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbOU.FormattingEnabled = true;
            this.cbOU.Location = new System.Drawing.Point(69, 76);
            this.cbOU.Name = "cbOU";
            this.cbOU.Size = new System.Drawing.Size(260, 21);
            this.cbOU.TabIndex = 4;
            this.cbOU.SelectedIndexChanged += new System.EventHandler(this.cbOU_SelectedIndexChanged);
            // 
            // cbWU
            // 
            this.cbWU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbWU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(69, 18);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(260, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // rbOU
            // 
            this.rbOU.AutoSize = true;
            this.rbOU.Location = new System.Drawing.Point(20, 77);
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
            this.rbWU.Location = new System.Drawing.Point(20, 19);
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
            this.tbEmployee.Location = new System.Drawing.Point(523, 9);
            this.tbEmployee.Name = "tbEmployee";
            this.tbEmployee.Size = new System.Drawing.Size(223, 20);
            this.tbEmployee.TabIndex = 2;
            this.tbEmployee.TextChanged += new System.EventHandler(this.tbEmployee_TextChanged);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(6, 335);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(142, 23);
            this.btnGenerate.TabIndex = 8;
            this.btnGenerate.Text = "Generate PY report";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(442, 34);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.ShowItemToolTips = true;
            this.lvEmployees.Size = new System.Drawing.Size(304, 275);
            this.lvEmployees.TabIndex = 3;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            // 
            // tabCounters
            // 
            this.tabCounters.Controls.Add(this.lblCalcID);
            this.tabCounters.Controls.Add(this.btnRecalculate);
            this.tabCounters.Controls.Add(this.nudCalcID);
            this.tabCounters.Location = new System.Drawing.Point(4, 22);
            this.tabCounters.Name = "tabCounters";
            this.tabCounters.Padding = new System.Windows.Forms.Padding(3);
            this.tabCounters.Size = new System.Drawing.Size(762, 371);
            this.tabCounters.TabIndex = 1;
            this.tabCounters.Text = "Counters";
            this.tabCounters.UseVisualStyleBackColor = true;
            // 
            // lblCalcID
            // 
            this.lblCalcID.Location = new System.Drawing.Point(32, 24);
            this.lblCalcID.Name = "lblCalcID";
            this.lblCalcID.Size = new System.Drawing.Size(96, 23);
            this.lblCalcID.TabIndex = 0;
            this.lblCalcID.Text = "Calc ID:";
            this.lblCalcID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRecalculate
            // 
            this.btnRecalculate.Location = new System.Drawing.Point(50, 72);
            this.btnRecalculate.Name = "btnRecalculate";
            this.btnRecalculate.Size = new System.Drawing.Size(162, 23);
            this.btnRecalculate.TabIndex = 2;
            this.btnRecalculate.Text = "Recalculate";
            this.btnRecalculate.UseVisualStyleBackColor = true;
            this.btnRecalculate.Click += new System.EventHandler(this.btnRecalculate_Click);
            // 
            // nudCalcID
            // 
            this.nudCalcID.Location = new System.Drawing.Point(134, 27);
            this.nudCalcID.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudCalcID.Name = "nudCalcID";
            this.nudCalcID.Size = new System.Drawing.Size(78, 20);
            this.nudCalcID.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(707, 415);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // PMCPYReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 455);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PMCPYReport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PMCPYReport";
            this.Load += new System.EventHandler(this.PMCPYReport_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabReport.ResumeLayout(false);
            this.tabReport.PerformLayout();
            this.gbCategory.ResumeLayout(false);
            this.gbUnitFilter.ResumeLayout(false);
            this.gbUnitFilter.PerformLayout();
            this.tabCounters.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudCalcID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabReport;
        private System.Windows.Forms.DateTimePicker dtpMonth;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.GroupBox gbCategory;
        private System.Windows.Forms.ListView lvCategory;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.GroupBox gbUnitFilter;
        private System.Windows.Forms.CheckBox chbHierarhiclyWU;
        private System.Windows.Forms.CheckBox chbHierachyOU;
        private System.Windows.Forms.ComboBox cbOU;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.RadioButton rbOU;
        private System.Windows.Forms.RadioButton rbWU;
        private System.Windows.Forms.TextBox tbEmployee;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.TabPage tabCounters;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRecalculate;
        private System.Windows.Forms.NumericUpDown nudCalcID;
        private System.Windows.Forms.Label lblCalcID;
        private System.Windows.Forms.Button btnGenerateAgency;

    }
}