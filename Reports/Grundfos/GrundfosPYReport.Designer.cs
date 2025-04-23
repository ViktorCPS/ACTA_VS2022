namespace Reports.Grundfos
{
    partial class GrundfosPYReport
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
            this.lblEmployee = new System.Windows.Forms.Label();
            this.gbUnitFilter = new System.Windows.Forms.GroupBox();
            this.cbOU = new System.Windows.Forms.ComboBox();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.rbOU = new System.Windows.Forms.RadioButton();
            this.rbWU = new System.Windows.Forms.RadioButton();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.tbEmployee = new System.Windows.Forms.TextBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.gbDateInterval = new System.Windows.Forms.GroupBox();
            this.lblTo = new System.Windows.Forms.Label();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvDates = new System.Windows.Forms.ListView();
            this.btnView = new System.Windows.Forms.Button();
            this.gbUnitFilter.SuspendLayout();
            this.gbDateInterval.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(357, 35);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(78, 23);
            this.lblEmployee.TabIndex = 1;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbUnitFilter
            // 
            this.gbUnitFilter.Controls.Add(this.cbOU);
            this.gbUnitFilter.Controls.Add(this.cbWU);
            this.gbUnitFilter.Controls.Add(this.rbOU);
            this.gbUnitFilter.Controls.Add(this.rbWU);
            this.gbUnitFilter.Location = new System.Drawing.Point(17, 14);
            this.gbUnitFilter.Name = "gbUnitFilter";
            this.gbUnitFilter.Size = new System.Drawing.Size(334, 82);
            this.gbUnitFilter.TabIndex = 0;
            this.gbUnitFilter.TabStop = false;
            this.gbUnitFilter.Text = "Unit filter";
            // 
            // cbOU
            // 
            this.cbOU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbOU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbOU.FormattingEnabled = true;
            this.cbOU.Location = new System.Drawing.Point(69, 45);
            this.cbOU.Name = "cbOU";
            this.cbOU.Size = new System.Drawing.Size(221, 21);
            this.cbOU.TabIndex = 3;
            this.cbOU.SelectedIndexChanged += new System.EventHandler(this.cbOU_SelectedIndexChanged);
            // 
            // cbWU
            // 
            this.cbWU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbWU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(69, 18);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(221, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // rbOU
            // 
            this.rbOU.AutoSize = true;
            this.rbOU.Location = new System.Drawing.Point(20, 46);
            this.rbOU.Name = "rbOU";
            this.rbOU.Size = new System.Drawing.Size(41, 17);
            this.rbOU.TabIndex = 2;
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
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(48, 19);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(110, 20);
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
            // tbEmployee
            // 
            this.tbEmployee.Location = new System.Drawing.Point(441, 37);
            this.tbEmployee.Name = "tbEmployee";
            this.tbEmployee.Size = new System.Drawing.Size(249, 20);
            this.tbEmployee.TabIndex = 2;
            this.tbEmployee.TextChanged += new System.EventHandler(this.tbEmployee_TextChanged);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(48, 45);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(110, 20);
            this.dtpTo.TabIndex = 3;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(638, 387);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(214, 117);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(114, 23);
            this.btnGenerate.TabIndex = 5;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // gbDateInterval
            // 
            this.gbDateInterval.Controls.Add(this.dtpFrom);
            this.gbDateInterval.Controls.Add(this.lblFrom);
            this.gbDateInterval.Controls.Add(this.dtpTo);
            this.gbDateInterval.Controls.Add(this.lblTo);
            this.gbDateInterval.Location = new System.Drawing.Point(17, 101);
            this.gbDateInterval.Name = "gbDateInterval";
            this.gbDateInterval.Size = new System.Drawing.Size(173, 80);
            this.gbDateInterval.TabIndex = 4;
            this.gbDateInterval.TabStop = false;
            this.gbDateInterval.Text = "Date interval";
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
            this.lvEmployees.Location = new System.Drawing.Point(360, 61);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.ShowItemToolTips = true;
            this.lvEmployees.Size = new System.Drawing.Size(330, 292);
            this.lvEmployees.TabIndex = 3;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployees_ColumnClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvDates);
            this.groupBox1.Controls.Add(this.btnView);
            this.groupBox1.Controls.Add(this.gbUnitFilter);
            this.groupBox1.Controls.Add(this.lblEmployee);
            this.groupBox1.Controls.Add(this.btnGenerate);
            this.groupBox1.Controls.Add(this.tbEmployee);
            this.groupBox1.Controls.Add(this.gbDateInterval);
            this.groupBox1.Controls.Add(this.lvEmployees);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(701, 369);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // lvDates
            // 
            this.lvDates.FullRowSelect = true;
            this.lvDates.GridLines = true;
            this.lvDates.HideSelection = false;
            this.lvDates.Location = new System.Drawing.Point(17, 193);
            this.lvDates.MultiSelect = false;
            this.lvDates.Name = "lvDates";
            this.lvDates.ShowItemToolTips = true;
            this.lvDates.Size = new System.Drawing.Size(330, 160);
            this.lvDates.TabIndex = 7;
            this.lvDates.UseCompatibleStateImageBehavior = false;
            this.lvDates.View = System.Windows.Forms.View.Details;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(214, 147);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(114, 23);
            this.btnView.TabIndex = 6;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // GrundfosPYReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 427);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "GrundfosPYReport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.Load += new System.EventHandler(this.GrundfosPYReport_Load);
            this.gbUnitFilter.ResumeLayout(false);
            this.gbUnitFilter.PerformLayout();
            this.gbDateInterval.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.GroupBox gbUnitFilter;
        private System.Windows.Forms.ComboBox cbOU;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.RadioButton rbOU;
        private System.Windows.Forms.RadioButton rbWU;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.TextBox tbEmployee;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.GroupBox gbDateInterval;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lvDates;
        private System.Windows.Forms.Button btnView;
    }
}