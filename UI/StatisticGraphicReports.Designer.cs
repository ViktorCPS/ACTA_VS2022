namespace UI
{
    partial class StatisticGraphicReports
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticGraphicReports));
            this.btnClose = new System.Windows.Forms.Button();
            this.gbStatisticReportType = new System.Windows.Forms.GroupBox();
            this.rbMultiWorkingUnit = new System.Windows.Forms.RadioButton();
            this.rbSingleWorkingUnit = new System.Windows.Forms.RadioButton();
            this.gbWorkingUnits = new System.Windows.Forms.GroupBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.lvWorkingUnits = new System.Windows.Forms.ListView();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.lblParentWUID = new System.Windows.Forms.Label();
            this.cbParentWorkingUnit = new System.Windows.Forms.ComboBox();
            this.gbGraphic = new System.Windows.Forms.GroupBox();
            this.chbExtraHours = new System.Windows.Forms.CheckBox();
            this.chbAbsenceDuringWorkingTime = new System.Windows.Forms.CheckBox();
            this.chbWholeDayAbsence = new System.Windows.Forms.CheckBox();
            this.chbPhysicalAttendance = new System.Windows.Forms.CheckBox();
            this.gbTimeInterval = new System.Windows.Forms.GroupBox();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.btnShow = new System.Windows.Forms.Button();
            this.gbStatisticReport = new System.Windows.Forms.GroupBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.gbIsWrkHrs = new System.Windows.Forms.GroupBox();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbNo = new System.Windows.Forms.RadioButton();
            this.rbYes = new System.Windows.Forms.RadioButton();
            this.gbLocation = new System.Windows.Forms.GroupBox();
            this.btnLocationTree = new System.Windows.Forms.Button();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.gbStatisticReportType.SuspendLayout();
            this.gbWorkingUnits.SuspendLayout();
            this.gbGraphic.SuspendLayout();
            this.gbTimeInterval.SuspendLayout();
            this.gbIsWrkHrs.SuspendLayout();
            this.gbLocation.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(915, 688);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbStatisticReportType
            // 
            this.gbStatisticReportType.Controls.Add(this.rbMultiWorkingUnit);
            this.gbStatisticReportType.Controls.Add(this.rbSingleWorkingUnit);
            this.gbStatisticReportType.Location = new System.Drawing.Point(12, 12);
            this.gbStatisticReportType.Name = "gbStatisticReportType";
            this.gbStatisticReportType.Size = new System.Drawing.Size(228, 77);
            this.gbStatisticReportType.TabIndex = 1;
            this.gbStatisticReportType.TabStop = false;
            this.gbStatisticReportType.Text = "Statistic report type";
            // 
            // rbMultiWorkingUnit
            // 
            this.rbMultiWorkingUnit.AutoSize = true;
            this.rbMultiWorkingUnit.Location = new System.Drawing.Point(24, 44);
            this.rbMultiWorkingUnit.Name = "rbMultiWorkingUnit";
            this.rbMultiWorkingUnit.Size = new System.Drawing.Size(107, 17);
            this.rbMultiWorkingUnit.TabIndex = 1;
            this.rbMultiWorkingUnit.TabStop = true;
            this.rbMultiWorkingUnit.Text = "Multi working unit";
            this.rbMultiWorkingUnit.UseVisualStyleBackColor = true;
            this.rbMultiWorkingUnit.CheckedChanged += new System.EventHandler(this.rbMultiWorkingUnit_CheckedChanged);
            // 
            // rbSingleWorkingUnit
            // 
            this.rbSingleWorkingUnit.AutoSize = true;
            this.rbSingleWorkingUnit.Checked = true;
            this.rbSingleWorkingUnit.Location = new System.Drawing.Point(24, 20);
            this.rbSingleWorkingUnit.Name = "rbSingleWorkingUnit";
            this.rbSingleWorkingUnit.Size = new System.Drawing.Size(114, 17);
            this.rbSingleWorkingUnit.TabIndex = 0;
            this.rbSingleWorkingUnit.TabStop = true;
            this.rbSingleWorkingUnit.Text = "Single working unit";
            this.rbSingleWorkingUnit.UseVisualStyleBackColor = true;
            this.rbSingleWorkingUnit.CheckedChanged += new System.EventHandler(this.rbSingleWorkingUnit_CheckedChanged);
            // 
            // gbWorkingUnits
            // 
            this.gbWorkingUnits.Controls.Add(this.btnWUTree);
            this.gbWorkingUnits.Controls.Add(this.lvWorkingUnits);
            this.gbWorkingUnits.Controls.Add(this.chbHierarhicly);
            this.gbWorkingUnits.Controls.Add(this.lblParentWUID);
            this.gbWorkingUnits.Controls.Add(this.cbParentWorkingUnit);
            this.gbWorkingUnits.Location = new System.Drawing.Point(12, 95);
            this.gbWorkingUnits.Name = "gbWorkingUnits";
            this.gbWorkingUnits.Size = new System.Drawing.Size(228, 235);
            this.gbWorkingUnits.TabIndex = 2;
            this.gbWorkingUnits.TabStop = false;
            this.gbWorkingUnits.Text = "Working unit";
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(197, 30);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 20;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // lvWorkingUnits
            // 
            this.lvWorkingUnits.FullRowSelect = true;
            this.lvWorkingUnits.GridLines = true;
            this.lvWorkingUnits.HideSelection = false;
            this.lvWorkingUnits.Location = new System.Drawing.Point(6, 59);
            this.lvWorkingUnits.Name = "lvWorkingUnits";
            this.lvWorkingUnits.Size = new System.Drawing.Size(216, 147);
            this.lvWorkingUnits.TabIndex = 4;
            this.lvWorkingUnits.UseCompatibleStateImageBehavior = false;
            this.lvWorkingUnits.View = System.Windows.Forms.View.Details;
            this.lvWorkingUnits.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvWorkingUnits_ColumnClick);
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.AutoSize = true;
            this.chbHierarhicly.Checked = true;
            this.chbHierarhicly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbHierarhicly.Location = new System.Drawing.Point(94, 212);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(71, 17);
            this.chbHierarhicly.TabIndex = 1;
            this.chbHierarhicly.Text = "Hierarchy";
            this.chbHierarhicly.UseVisualStyleBackColor = true;
            // 
            // lblParentWUID
            // 
            this.lblParentWUID.AutoSize = true;
            this.lblParentWUID.Location = new System.Drawing.Point(3, 16);
            this.lblParentWUID.Name = "lblParentWUID";
            this.lblParentWUID.Size = new System.Drawing.Size(101, 13);
            this.lblParentWUID.TabIndex = 2;
            this.lblParentWUID.Text = "Parent working unit:";
            // 
            // cbParentWorkingUnit
            // 
            this.cbParentWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbParentWorkingUnit.FormattingEnabled = true;
            this.cbParentWorkingUnit.Location = new System.Drawing.Point(6, 32);
            this.cbParentWorkingUnit.Name = "cbParentWorkingUnit";
            this.cbParentWorkingUnit.Size = new System.Drawing.Size(185, 21);
            this.cbParentWorkingUnit.TabIndex = 1;
            this.cbParentWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbParentWorkingUnit_SelectedIndexChanged);
            // 
            // gbGraphic
            // 
            this.gbGraphic.Controls.Add(this.chbExtraHours);
            this.gbGraphic.Controls.Add(this.chbAbsenceDuringWorkingTime);
            this.gbGraphic.Controls.Add(this.chbWholeDayAbsence);
            this.gbGraphic.Controls.Add(this.chbPhysicalAttendance);
            this.gbGraphic.Location = new System.Drawing.Point(12, 336);
            this.gbGraphic.Name = "gbGraphic";
            this.gbGraphic.Size = new System.Drawing.Size(228, 115);
            this.gbGraphic.TabIndex = 3;
            this.gbGraphic.TabStop = false;
            this.gbGraphic.Text = "Graphic";
            // 
            // chbExtraHours
            // 
            this.chbExtraHours.AutoSize = true;
            this.chbExtraHours.Location = new System.Drawing.Point(24, 91);
            this.chbExtraHours.Name = "chbExtraHours";
            this.chbExtraHours.Size = new System.Drawing.Size(79, 17);
            this.chbExtraHours.TabIndex = 3;
            this.chbExtraHours.Text = "Extra hours";
            this.chbExtraHours.UseVisualStyleBackColor = true;
            // 
            // chbAbsenceDuringWorkingTime
            // 
            this.chbAbsenceDuringWorkingTime.AutoSize = true;
            this.chbAbsenceDuringWorkingTime.Location = new System.Drawing.Point(24, 67);
            this.chbAbsenceDuringWorkingTime.Name = "chbAbsenceDuringWorkingTime";
            this.chbAbsenceDuringWorkingTime.Size = new System.Drawing.Size(141, 17);
            this.chbAbsenceDuringWorkingTime.TabIndex = 2;
            this.chbAbsenceDuringWorkingTime.Text = "Absence in working time";
            this.chbAbsenceDuringWorkingTime.UseVisualStyleBackColor = true;
            // 
            // chbWholeDayAbsence
            // 
            this.chbWholeDayAbsence.AutoSize = true;
            this.chbWholeDayAbsence.Location = new System.Drawing.Point(24, 43);
            this.chbWholeDayAbsence.Name = "chbWholeDayAbsence";
            this.chbWholeDayAbsence.Size = new System.Drawing.Size(101, 17);
            this.chbWholeDayAbsence.TabIndex = 1;
            this.chbWholeDayAbsence.Text = "All day absence";
            this.chbWholeDayAbsence.UseVisualStyleBackColor = true;
            // 
            // chbPhysicalAttendance
            // 
            this.chbPhysicalAttendance.AutoSize = true;
            this.chbPhysicalAttendance.Location = new System.Drawing.Point(24, 19);
            this.chbPhysicalAttendance.Name = "chbPhysicalAttendance";
            this.chbPhysicalAttendance.Size = new System.Drawing.Size(122, 17);
            this.chbPhysicalAttendance.TabIndex = 0;
            this.chbPhysicalAttendance.Text = "Physical attendance";
            this.chbPhysicalAttendance.UseVisualStyleBackColor = true;
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.dtpTo);
            this.gbTimeInterval.Controls.Add(this.dtpFrom);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Location = new System.Drawing.Point(12, 457);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(228, 96);
            this.gbTimeInterval.TabIndex = 4;
            this.gbTimeInterval.TabStop = false;
            this.gbTimeInterval.Text = "Time interval";
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(21, 62);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(23, 13);
            this.lblTo.TabIndex = 8;
            this.lblTo.Text = "To:";
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(60, 58);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(162, 20);
            this.dtpTo.TabIndex = 7;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(60, 22);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(162, 20);
            this.dtpFrom.TabIndex = 6;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(21, 26);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(33, 13);
            this.lblFrom.TabIndex = 5;
            this.lblFrom.Text = "From:";
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(165, 688);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 5;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // gbStatisticReport
            // 
            this.gbStatisticReport.Location = new System.Drawing.Point(246, 12);
            this.gbStatisticReport.Name = "gbStatisticReport";
            this.gbStatisticReport.Size = new System.Drawing.Size(754, 663);
            this.gbStatisticReport.TabIndex = 6;
            this.gbStatisticReport.TabStop = false;
            this.gbStatisticReport.Text = "Statistic report";
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(12, 688);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(131, 23);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // gbIsWrkHrs
            // 
            this.gbIsWrkHrs.Controls.Add(this.rbAll);
            this.gbIsWrkHrs.Controls.Add(this.rbNo);
            this.gbIsWrkHrs.Controls.Add(this.rbYes);
            this.gbIsWrkHrs.Location = new System.Drawing.Point(13, 603);
            this.gbIsWrkHrs.Name = "gbIsWrkHrs";
            this.gbIsWrkHrs.Size = new System.Drawing.Size(227, 42);
            this.gbIsWrkHrs.TabIndex = 46;
            this.gbIsWrkHrs.TabStop = false;
            this.gbIsWrkHrs.Text = "gbIsWrkHrs";
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Checked = true;
            this.rbAll.Location = new System.Drawing.Point(181, 16);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(36, 17);
            this.rbAll.TabIndex = 45;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "All";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // rbNo
            // 
            this.rbNo.AutoSize = true;
            this.rbNo.Location = new System.Drawing.Point(94, 16);
            this.rbNo.Name = "rbNo";
            this.rbNo.Size = new System.Drawing.Size(39, 17);
            this.rbNo.TabIndex = 1;
            this.rbNo.Text = "No";
            this.rbNo.UseVisualStyleBackColor = true;
            // 
            // rbYes
            // 
            this.rbYes.AutoSize = true;
            this.rbYes.Location = new System.Drawing.Point(8, 16);
            this.rbYes.Name = "rbYes";
            this.rbYes.Size = new System.Drawing.Size(43, 17);
            this.rbYes.TabIndex = 0;
            this.rbYes.TabStop = true;
            this.rbYes.Text = "Yes";
            this.rbYes.UseVisualStyleBackColor = true;
            // 
            // gbLocation
            // 
            this.gbLocation.Controls.Add(this.btnLocationTree);
            this.gbLocation.Controls.Add(this.cbLocation);
            this.gbLocation.Location = new System.Drawing.Point(12, 559);
            this.gbLocation.Name = "gbLocation";
            this.gbLocation.Size = new System.Drawing.Size(228, 41);
            this.gbLocation.TabIndex = 45;
            this.gbLocation.TabStop = false;
            this.gbLocation.Text = "Location";
            // 
            // btnLocationTree
            // 
            this.btnLocationTree.Image = ((System.Drawing.Image)(resources.GetObject("btnLocationTree.Image")));
            this.btnLocationTree.Location = new System.Drawing.Point(196, 12);
            this.btnLocationTree.Name = "btnLocationTree";
            this.btnLocationTree.Size = new System.Drawing.Size(25, 24);
            this.btnLocationTree.TabIndex = 46;
            this.btnLocationTree.Click += new System.EventHandler(this.btnLocationTree_Click);
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.Location = new System.Drawing.Point(9, 14);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(180, 21);
            this.cbLocation.TabIndex = 45;
            // 
            // StatisticGraphicReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 723);
            this.ControlBox = false;
            this.Controls.Add(this.gbIsWrkHrs);
            this.Controls.Add(this.gbLocation);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.gbStatisticReport);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.gbTimeInterval);
            this.Controls.Add(this.gbGraphic);
            this.Controls.Add(this.gbWorkingUnits);
            this.Controls.Add(this.gbStatisticReportType);
            this.Controls.Add(this.btnClose);
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(1010, 757);
            this.MinimumSize = new System.Drawing.Size(1010, 757);
            this.Name = "StatisticGraphicReports";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Statistic reports";
            this.Load += new System.EventHandler(this.StatisticGraphicReports_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.StatisticGraphicReports_KeyUp);
            this.gbStatisticReportType.ResumeLayout(false);
            this.gbStatisticReportType.PerformLayout();
            this.gbWorkingUnits.ResumeLayout(false);
            this.gbWorkingUnits.PerformLayout();
            this.gbGraphic.ResumeLayout(false);
            this.gbGraphic.PerformLayout();
            this.gbTimeInterval.ResumeLayout(false);
            this.gbTimeInterval.PerformLayout();
            this.gbIsWrkHrs.ResumeLayout(false);
            this.gbIsWrkHrs.PerformLayout();
            this.gbLocation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbStatisticReportType;
        private System.Windows.Forms.RadioButton rbMultiWorkingUnit;
        private System.Windows.Forms.RadioButton rbSingleWorkingUnit;
        protected System.Windows.Forms.GroupBox gbWorkingUnits;
        protected System.Windows.Forms.ListView lvWorkingUnits;
        protected System.Windows.Forms.CheckBox chbHierarhicly;
        protected System.Windows.Forms.Label lblParentWUID;
        protected System.Windows.Forms.ComboBox cbParentWorkingUnit;
        private System.Windows.Forms.GroupBox gbGraphic;
        private System.Windows.Forms.CheckBox chbPhysicalAttendance;
        private System.Windows.Forms.CheckBox chbExtraHours;
        private System.Windows.Forms.CheckBox chbAbsenceDuringWorkingTime;
        private System.Windows.Forms.CheckBox chbWholeDayAbsence;
        private System.Windows.Forms.GroupBox gbTimeInterval;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.GroupBox gbStatisticReport;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnWUTree;
        protected System.Windows.Forms.GroupBox gbIsWrkHrs;
        protected System.Windows.Forms.RadioButton rbAll;
        protected System.Windows.Forms.RadioButton rbNo;
        protected System.Windows.Forms.RadioButton rbYes;
        protected System.Windows.Forms.GroupBox gbLocation;
        protected System.Windows.Forms.Button btnLocationTree;
        protected System.Windows.Forms.ComboBox cbLocation;

    }
}