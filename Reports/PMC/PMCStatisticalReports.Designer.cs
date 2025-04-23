namespace Reports.PMC
{
    partial class PMCStatisticalReports
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
            this.gbReportType = new System.Windows.Forms.GroupBox();
            this.rbPositionChange = new System.Windows.Forms.RadioButton();
            this.rbGender = new System.Windows.Forms.RadioButton();
            this.rbAgeAverage = new System.Windows.Forms.RadioButton();
            this.rbLeavingEmployees = new System.Windows.Forms.RadioButton();
            this.rbHiredEmployees = new System.Windows.Forms.RadioButton();
            this.gbDateInterval = new System.Windows.Forms.GroupBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.gbCategory = new System.Windows.Forms.GroupBox();
            this.lvCategory = new System.Windows.Forms.ListView();
            this.gbReportType.SuspendLayout();
            this.gbDateInterval.SuspendLayout();
            this.gbCategory.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(432, 266);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbReportType
            // 
            this.gbReportType.Controls.Add(this.rbPositionChange);
            this.gbReportType.Controls.Add(this.rbGender);
            this.gbReportType.Controls.Add(this.rbAgeAverage);
            this.gbReportType.Controls.Add(this.rbLeavingEmployees);
            this.gbReportType.Controls.Add(this.rbHiredEmployees);
            this.gbReportType.Location = new System.Drawing.Point(12, 12);
            this.gbReportType.Name = "gbReportType";
            this.gbReportType.Size = new System.Drawing.Size(234, 150);
            this.gbReportType.TabIndex = 0;
            this.gbReportType.TabStop = false;
            this.gbReportType.Text = "Report type";
            // 
            // rbPositionChange
            // 
            this.rbPositionChange.AutoSize = true;
            this.rbPositionChange.Location = new System.Drawing.Point(20, 116);
            this.rbPositionChange.Name = "rbPositionChange";
            this.rbPositionChange.Size = new System.Drawing.Size(101, 17);
            this.rbPositionChange.TabIndex = 4;
            this.rbPositionChange.TabStop = true;
            this.rbPositionChange.Text = "Position change";
            this.rbPositionChange.UseVisualStyleBackColor = true;
            // 
            // rbGender
            // 
            this.rbGender.AutoSize = true;
            this.rbGender.Location = new System.Drawing.Point(20, 93);
            this.rbGender.Name = "rbGender";
            this.rbGender.Size = new System.Drawing.Size(60, 17);
            this.rbGender.TabIndex = 3;
            this.rbGender.TabStop = true;
            this.rbGender.Text = "Gender";
            this.rbGender.UseVisualStyleBackColor = true;
            // 
            // rbAgeAverage
            // 
            this.rbAgeAverage.AutoSize = true;
            this.rbAgeAverage.Location = new System.Drawing.Point(20, 70);
            this.rbAgeAverage.Name = "rbAgeAverage";
            this.rbAgeAverage.Size = new System.Drawing.Size(86, 17);
            this.rbAgeAverage.TabIndex = 2;
            this.rbAgeAverage.TabStop = true;
            this.rbAgeAverage.Text = "Age average";
            this.rbAgeAverage.UseVisualStyleBackColor = true;
            // 
            // rbLeavingEmployees
            // 
            this.rbLeavingEmployees.AutoSize = true;
            this.rbLeavingEmployees.Location = new System.Drawing.Point(20, 47);
            this.rbLeavingEmployees.Name = "rbLeavingEmployees";
            this.rbLeavingEmployees.Size = new System.Drawing.Size(116, 17);
            this.rbLeavingEmployees.TabIndex = 1;
            this.rbLeavingEmployees.TabStop = true;
            this.rbLeavingEmployees.Text = "Leaving employees";
            this.rbLeavingEmployees.UseVisualStyleBackColor = true;
            // 
            // rbHiredEmployees
            // 
            this.rbHiredEmployees.AutoSize = true;
            this.rbHiredEmployees.Location = new System.Drawing.Point(20, 24);
            this.rbHiredEmployees.Name = "rbHiredEmployees";
            this.rbHiredEmployees.Size = new System.Drawing.Size(97, 17);
            this.rbHiredEmployees.TabIndex = 0;
            this.rbHiredEmployees.TabStop = true;
            this.rbHiredEmployees.Text = "Hired emplyees";
            this.rbHiredEmployees.UseVisualStyleBackColor = true;
            // 
            // gbDateInterval
            // 
            this.gbDateInterval.Controls.Add(this.dtpFrom);
            this.gbDateInterval.Controls.Add(this.lblFrom);
            this.gbDateInterval.Controls.Add(this.dtpTo);
            this.gbDateInterval.Controls.Add(this.lblTo);
            this.gbDateInterval.Location = new System.Drawing.Point(12, 168);
            this.gbDateInterval.Name = "gbDateInterval";
            this.gbDateInterval.Size = new System.Drawing.Size(234, 80);
            this.gbDateInterval.TabIndex = 2;
            this.gbDateInterval.TabStop = false;
            this.gbDateInterval.Text = "Date interval";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(48, 19);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(110, 20);
            this.dtpFrom.TabIndex = 1;
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
            this.dtpTo.Size = new System.Drawing.Size(110, 20);
            this.dtpTo.TabIndex = 3;
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
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(12, 266);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(114, 23);
            this.btnGenerate.TabIndex = 3;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // gbCategory
            // 
            this.gbCategory.Controls.Add(this.lvCategory);
            this.gbCategory.Location = new System.Drawing.Point(252, 12);
            this.gbCategory.Name = "gbCategory";
            this.gbCategory.Size = new System.Drawing.Size(255, 236);
            this.gbCategory.TabIndex = 1;
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
            this.lvCategory.Size = new System.Drawing.Size(225, 202);
            this.lvCategory.TabIndex = 0;
            this.lvCategory.UseCompatibleStateImageBehavior = false;
            this.lvCategory.View = System.Windows.Forms.View.Details;
            // 
            // PMCStatisticalReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 323);
            this.ControlBox = false;
            this.Controls.Add(this.gbCategory);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.gbDateInterval);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbReportType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PMCStatisticalReports";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Statistical reports";
            this.Load += new System.EventHandler(this.PMCStatisticalReports_Load);
            this.gbReportType.ResumeLayout(false);
            this.gbReportType.PerformLayout();
            this.gbDateInterval.ResumeLayout(false);
            this.gbCategory.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbReportType;
        private System.Windows.Forms.RadioButton rbPositionChange;
        private System.Windows.Forms.RadioButton rbGender;
        private System.Windows.Forms.RadioButton rbAgeAverage;
        private System.Windows.Forms.RadioButton rbLeavingEmployees;
        private System.Windows.Forms.RadioButton rbHiredEmployees;
        private System.Windows.Forms.GroupBox gbDateInterval;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.GroupBox gbCategory;
        private System.Windows.Forms.ListView lvCategory;
    }
}