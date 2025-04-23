namespace UI
{
    partial class PresenceGraphForDayControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PresenceGraphForDayControl));
            this.gbWorkingUnits = new System.Windows.Forms.GroupBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.lvWorkingUnits = new System.Windows.Forms.ListView();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.lblParentWUID = new System.Windows.Forms.Label();
            this.cbParentWorkingUnit = new System.Windows.Forms.ComboBox();
            this.gbEmployees = new System.Windows.Forms.GroupBox();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.chbSelectAll = new System.Windows.Forms.CheckBox();
            this.gbDay = new System.Windows.Forms.GroupBox();
            this.chbShowNextDay = new System.Windows.Forms.CheckBox();
            this.dtpDay = new System.Windows.Forms.DateTimePicker();
            this.gbGraphicReport = new System.Windows.Forms.GroupBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.gbLegend = new System.Windows.Forms.GroupBox();
            this.lblLate = new System.Windows.Forms.Label();
            this.panelRegularWork = new System.Windows.Forms.Panel();
            this.panelLate = new System.Windows.Forms.Panel();
            this.lblRegularWork = new System.Windows.Forms.Label();
            this.lblTheRestOfAllDayAbsence = new System.Windows.Forms.Label();
            this.panelOfficialyOutgiong = new System.Windows.Forms.Panel();
            this.panelTheRestOfAllDayAbsence = new System.Windows.Forms.Panel();
            this.lblOfficiallyOutgoing = new System.Windows.Forms.Label();
            this.lblVacation = new System.Windows.Forms.Label();
            this.panelPause = new System.Windows.Forms.Panel();
            this.panelVacation = new System.Windows.Forms.Panel();
            this.lblPause = new System.Windows.Forms.Label();
            this.lblSickLeave = new System.Windows.Forms.Label();
            this.panelOutgoingInPrivate = new System.Windows.Forms.Panel();
            this.panelSickLeave = new System.Windows.Forms.Panel();
            this.lblOutgoingInPrivate = new System.Windows.Forms.Label();
            this.lblTheRestOfReaderPasses = new System.Windows.Forms.Label();
            this.panelTheRestOfReaderPasses = new System.Windows.Forms.Panel();
            this.btnShow = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.gbDaysNavigation = new System.Windows.Forms.GroupBox();
            this.btnPrevDay = new System.Windows.Forms.Button();
            this.btnNextDay = new System.Windows.Forms.Button();
            this.gbPageNavigation = new System.Windows.Forms.GroupBox();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbLocation = new System.Windows.Forms.GroupBox();
            this.btnLocationTree = new System.Windows.Forms.Button();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.gbIsWrkHrs = new System.Windows.Forms.GroupBox();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbNo = new System.Windows.Forms.RadioButton();
            this.rbYes = new System.Windows.Forms.RadioButton();
            this.gbWorkingUnits.SuspendLayout();
            this.gbEmployees.SuspendLayout();
            this.gbDay.SuspendLayout();
            this.gbGraphicReport.SuspendLayout();
            this.gbLegend.SuspendLayout();
            this.gbDaysNavigation.SuspendLayout();
            this.gbPageNavigation.SuspendLayout();
            this.gbLocation.SuspendLayout();
            this.gbIsWrkHrs.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbWorkingUnits
            // 
            this.gbWorkingUnits.Controls.Add(this.btnWUTree);
            this.gbWorkingUnits.Controls.Add(this.lvWorkingUnits);
            this.gbWorkingUnits.Controls.Add(this.chbHierarhicly);
            this.gbWorkingUnits.Controls.Add(this.lblParentWUID);
            this.gbWorkingUnits.Controls.Add(this.cbParentWorkingUnit);
            this.gbWorkingUnits.Location = new System.Drawing.Point(3, 3);
            this.gbWorkingUnits.Name = "gbWorkingUnits";
            this.gbWorkingUnits.Size = new System.Drawing.Size(228, 235);
            this.gbWorkingUnits.TabIndex = 1;
            this.gbWorkingUnits.TabStop = false;
            this.gbWorkingUnits.Text = "Working unit";
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(198, 30);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 1;
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
            this.lvWorkingUnits.TabIndex = 2;
            this.lvWorkingUnits.UseCompatibleStateImageBehavior = false;
            this.lvWorkingUnits.View = System.Windows.Forms.View.Details;
            this.lvWorkingUnits.SelectedIndexChanged += new System.EventHandler(this.lvWorkingUnits_SelectedIndexChanged);
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
            this.chbHierarhicly.TabIndex = 3;
            this.chbHierarhicly.Text = "Hierarchy";
            this.chbHierarhicly.UseVisualStyleBackColor = true;
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
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
            this.cbParentWorkingUnit.Size = new System.Drawing.Size(186, 21);
            this.cbParentWorkingUnit.TabIndex = 0;
            this.cbParentWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbParentWorkingUnit_SelectedIndexChanged);
            // 
            // gbEmployees
            // 
            this.gbEmployees.Controls.Add(this.lvEmployees);
            this.gbEmployees.Controls.Add(this.chbSelectAll);
            this.gbEmployees.Location = new System.Drawing.Point(4, 244);
            this.gbEmployees.Name = "gbEmployees";
            this.gbEmployees.Size = new System.Drawing.Size(227, 197);
            this.gbEmployees.TabIndex = 2;
            this.gbEmployees.TabStop = false;
            this.gbEmployees.Text = "Employees";
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(6, 19);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.Size = new System.Drawing.Size(216, 148);
            this.lvEmployees.TabIndex = 0;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployees_ColumnClick);
            this.lvEmployees.Click += new System.EventHandler(this.lvEmployees_Click);
            // 
            // chbSelectAll
            // 
            this.chbSelectAll.AutoSize = true;
            this.chbSelectAll.Checked = true;
            this.chbSelectAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbSelectAll.Location = new System.Drawing.Point(93, 173);
            this.chbSelectAll.Name = "chbSelectAll";
            this.chbSelectAll.Size = new System.Drawing.Size(69, 17);
            this.chbSelectAll.TabIndex = 1;
            this.chbSelectAll.Text = "Select all";
            this.chbSelectAll.UseVisualStyleBackColor = true;
            this.chbSelectAll.CheckedChanged += new System.EventHandler(this.chbSelectAll_CheckedChanged);
            // 
            // gbDay
            // 
            this.gbDay.Controls.Add(this.chbShowNextDay);
            this.gbDay.Controls.Add(this.dtpDay);
            this.gbDay.Location = new System.Drawing.Point(3, 447);
            this.gbDay.Name = "gbDay";
            this.gbDay.Size = new System.Drawing.Size(227, 69);
            this.gbDay.TabIndex = 3;
            this.gbDay.TabStop = false;
            this.gbDay.Text = "Day";
            // 
            // chbShowNextDay
            // 
            this.chbShowNextDay.AutoSize = true;
            this.chbShowNextDay.Location = new System.Drawing.Point(93, 45);
            this.chbShowNextDay.Name = "chbShowNextDay";
            this.chbShowNextDay.Size = new System.Drawing.Size(96, 17);
            this.chbShowNextDay.TabIndex = 1;
            this.chbShowNextDay.Text = "Show next day";
            this.chbShowNextDay.UseVisualStyleBackColor = true;
            // 
            // dtpDay
            // 
            this.dtpDay.CustomFormat = "dd.MM.yyyy";
            this.dtpDay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDay.Location = new System.Drawing.Point(6, 19);
            this.dtpDay.Name = "dtpDay";
            this.dtpDay.Size = new System.Drawing.Size(216, 20);
            this.dtpDay.TabIndex = 0;
            // 
            // gbGraphicReport
            // 
            this.gbGraphicReport.Controls.Add(this.lblTotal);
            this.gbGraphicReport.Controls.Add(this.lblEmployee);
            this.gbGraphicReport.Location = new System.Drawing.Point(240, 3);
            this.gbGraphicReport.Name = "gbGraphicReport";
            this.gbGraphicReport.Size = new System.Drawing.Size(734, 513);
            this.gbGraphicReport.TabIndex = 9;
            this.gbGraphicReport.TabStop = false;
            this.gbGraphicReport.Text = "Graphic report";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(93, 16);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(34, 13);
            this.lblTotal.TabIndex = 1;
            this.lblTotal.Text = "Total:";
            // 
            // lblEmployee
            // 
            this.lblEmployee.AutoSize = true;
            this.lblEmployee.Location = new System.Drawing.Point(6, 16);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(50, 13);
            this.lblEmployee.TabIndex = 0;
            this.lblEmployee.Text = "Employe:";
            // 
            // gbLegend
            // 
            this.gbLegend.Controls.Add(this.lblLate);
            this.gbLegend.Controls.Add(this.panelRegularWork);
            this.gbLegend.Controls.Add(this.panelLate);
            this.gbLegend.Controls.Add(this.lblRegularWork);
            this.gbLegend.Controls.Add(this.lblTheRestOfAllDayAbsence);
            this.gbLegend.Controls.Add(this.panelOfficialyOutgiong);
            this.gbLegend.Controls.Add(this.panelTheRestOfAllDayAbsence);
            this.gbLegend.Controls.Add(this.lblOfficiallyOutgoing);
            this.gbLegend.Controls.Add(this.lblVacation);
            this.gbLegend.Controls.Add(this.panelPause);
            this.gbLegend.Controls.Add(this.panelVacation);
            this.gbLegend.Controls.Add(this.lblPause);
            this.gbLegend.Controls.Add(this.lblSickLeave);
            this.gbLegend.Controls.Add(this.panelOutgoingInPrivate);
            this.gbLegend.Controls.Add(this.panelSickLeave);
            this.gbLegend.Controls.Add(this.lblOutgoingInPrivate);
            this.gbLegend.Controls.Add(this.lblTheRestOfReaderPasses);
            this.gbLegend.Controls.Add(this.panelTheRestOfReaderPasses);
            this.gbLegend.Location = new System.Drawing.Point(240, 522);
            this.gbLegend.Name = "gbLegend";
            this.gbLegend.Size = new System.Drawing.Size(734, 87);
            this.gbLegend.TabIndex = 36;
            this.gbLegend.TabStop = false;
            this.gbLegend.Text = "Legend";
            // 
            // lblLate
            // 
            this.lblLate.AutoSize = true;
            this.lblLate.Location = new System.Drawing.Point(577, 63);
            this.lblLate.Name = "lblLate";
            this.lblLate.Size = new System.Drawing.Size(28, 13);
            this.lblLate.TabIndex = 32;
            this.lblLate.Text = "Late";
            // 
            // panelRegularWork
            // 
            this.panelRegularWork.BackColor = System.Drawing.Color.ForestGreen;
            this.panelRegularWork.Location = new System.Drawing.Point(14, 19);
            this.panelRegularWork.Name = "panelRegularWork";
            this.panelRegularWork.Size = new System.Drawing.Size(22, 15);
            this.panelRegularWork.TabIndex = 15;
            // 
            // panelLate
            // 
            this.panelLate.BackColor = System.Drawing.Color.Red;
            this.panelLate.Location = new System.Drawing.Point(549, 61);
            this.panelLate.Name = "panelLate";
            this.panelLate.Size = new System.Drawing.Size(22, 15);
            this.panelLate.TabIndex = 31;
            // 
            // lblRegularWork
            // 
            this.lblRegularWork.AutoSize = true;
            this.lblRegularWork.Location = new System.Drawing.Point(42, 21);
            this.lblRegularWork.Name = "lblRegularWork";
            this.lblRegularWork.Size = new System.Drawing.Size(70, 13);
            this.lblRegularWork.TabIndex = 16;
            this.lblRegularWork.Text = "Regular work";
            // 
            // lblTheRestOfAllDayAbsence
            // 
            this.lblTheRestOfAllDayAbsence.AutoSize = true;
            this.lblTheRestOfAllDayAbsence.Location = new System.Drawing.Point(577, 42);
            this.lblTheRestOfAllDayAbsence.Name = "lblTheRestOfAllDayAbsence";
            this.lblTheRestOfAllDayAbsence.Size = new System.Drawing.Size(135, 13);
            this.lblTheRestOfAllDayAbsence.TabIndex = 30;
            this.lblTheRestOfAllDayAbsence.Text = "The rest of all day absence";
            // 
            // panelOfficialyOutgiong
            // 
            this.panelOfficialyOutgiong.BackColor = System.Drawing.Color.LawnGreen;
            this.panelOfficialyOutgiong.Location = new System.Drawing.Point(14, 40);
            this.panelOfficialyOutgiong.Name = "panelOfficialyOutgiong";
            this.panelOfficialyOutgiong.Size = new System.Drawing.Size(22, 15);
            this.panelOfficialyOutgiong.TabIndex = 17;
            // 
            // panelTheRestOfAllDayAbsence
            // 
            this.panelTheRestOfAllDayAbsence.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panelTheRestOfAllDayAbsence.Location = new System.Drawing.Point(549, 42);
            this.panelTheRestOfAllDayAbsence.Name = "panelTheRestOfAllDayAbsence";
            this.panelTheRestOfAllDayAbsence.Size = new System.Drawing.Size(22, 15);
            this.panelTheRestOfAllDayAbsence.TabIndex = 29;
            // 
            // lblOfficiallyOutgoing
            // 
            this.lblOfficiallyOutgoing.AutoSize = true;
            this.lblOfficiallyOutgoing.Location = new System.Drawing.Point(42, 41);
            this.lblOfficiallyOutgoing.Name = "lblOfficiallyOutgoing";
            this.lblOfficiallyOutgoing.Size = new System.Drawing.Size(90, 13);
            this.lblOfficiallyOutgoing.TabIndex = 18;
            this.lblOfficiallyOutgoing.Text = "Officially outgoing";
            // 
            // lblVacation
            // 
            this.lblVacation.AutoSize = true;
            this.lblVacation.Location = new System.Drawing.Point(577, 21);
            this.lblVacation.Name = "lblVacation";
            this.lblVacation.Size = new System.Drawing.Size(49, 13);
            this.lblVacation.TabIndex = 28;
            this.lblVacation.Text = "Vacation";
            // 
            // panelPause
            // 
            this.panelPause.BackColor = System.Drawing.Color.Yellow;
            this.panelPause.Location = new System.Drawing.Point(14, 61);
            this.panelPause.Name = "panelPause";
            this.panelPause.Size = new System.Drawing.Size(22, 15);
            this.panelPause.TabIndex = 19;
            // 
            // panelVacation
            // 
            this.panelVacation.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.panelVacation.Location = new System.Drawing.Point(549, 19);
            this.panelVacation.Name = "panelVacation";
            this.panelVacation.Size = new System.Drawing.Size(22, 15);
            this.panelVacation.TabIndex = 27;
            // 
            // lblPause
            // 
            this.lblPause.AutoSize = true;
            this.lblPause.Location = new System.Drawing.Point(42, 63);
            this.lblPause.Name = "lblPause";
            this.lblPause.Size = new System.Drawing.Size(37, 13);
            this.lblPause.TabIndex = 20;
            this.lblPause.Text = "Pause";
            // 
            // lblSickLeave
            // 
            this.lblSickLeave.AutoSize = true;
            this.lblSickLeave.Location = new System.Drawing.Point(314, 63);
            this.lblSickLeave.Name = "lblSickLeave";
            this.lblSickLeave.Size = new System.Drawing.Size(57, 13);
            this.lblSickLeave.TabIndex = 26;
            this.lblSickLeave.Text = "Sick-leave";
            // 
            // panelOutgoingInPrivate
            // 
            this.panelOutgoingInPrivate.BackColor = System.Drawing.Color.Wheat;
            this.panelOutgoingInPrivate.Location = new System.Drawing.Point(286, 19);
            this.panelOutgoingInPrivate.Name = "panelOutgoingInPrivate";
            this.panelOutgoingInPrivate.Size = new System.Drawing.Size(22, 15);
            this.panelOutgoingInPrivate.TabIndex = 21;
            // 
            // panelSickLeave
            // 
            this.panelSickLeave.BackColor = System.Drawing.Color.Aqua;
            this.panelSickLeave.Location = new System.Drawing.Point(286, 61);
            this.panelSickLeave.Name = "panelSickLeave";
            this.panelSickLeave.Size = new System.Drawing.Size(22, 15);
            this.panelSickLeave.TabIndex = 25;
            // 
            // lblOutgoingInPrivate
            // 
            this.lblOutgoingInPrivate.AutoSize = true;
            this.lblOutgoingInPrivate.Location = new System.Drawing.Point(314, 21);
            this.lblOutgoingInPrivate.Name = "lblOutgoingInPrivate";
            this.lblOutgoingInPrivate.Size = new System.Drawing.Size(96, 13);
            this.lblOutgoingInPrivate.TabIndex = 22;
            this.lblOutgoingInPrivate.Text = "Outgoing in private";
            // 
            // lblTheRestOfReaderPasses
            // 
            this.lblTheRestOfReaderPasses.AutoSize = true;
            this.lblTheRestOfReaderPasses.Location = new System.Drawing.Point(314, 41);
            this.lblTheRestOfReaderPasses.Name = "lblTheRestOfReaderPasses";
            this.lblTheRestOfReaderPasses.Size = new System.Drawing.Size(127, 13);
            this.lblTheRestOfReaderPasses.TabIndex = 24;
            this.lblTheRestOfReaderPasses.Text = "The rest of reader passes";
            // 
            // panelTheRestOfReaderPasses
            // 
            this.panelTheRestOfReaderPasses.BackColor = System.Drawing.Color.Orange;
            this.panelTheRestOfReaderPasses.Location = new System.Drawing.Point(286, 39);
            this.panelTheRestOfReaderPasses.Name = "panelTheRestOfReaderPasses";
            this.panelTheRestOfReaderPasses.Size = new System.Drawing.Size(22, 15);
            this.panelTheRestOfReaderPasses.TabIndex = 23;
            // 
            // btnShow
            // 
            this.btnShow.BackColor = System.Drawing.SystemColors.Control;
            this.btnShow.Location = new System.Drawing.Point(155, 651);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 1;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = false;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.SystemColors.Control;
            this.btnPrint.Location = new System.Drawing.Point(4, 651);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(131, 23);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // gbDaysNavigation
            // 
            this.gbDaysNavigation.Controls.Add(this.btnPrevDay);
            this.gbDaysNavigation.Controls.Add(this.btnNextDay);
            this.gbDaysNavigation.Location = new System.Drawing.Point(240, 628);
            this.gbDaysNavigation.Name = "gbDaysNavigation";
            this.gbDaysNavigation.Size = new System.Drawing.Size(223, 52);
            this.gbDaysNavigation.TabIndex = 40;
            this.gbDaysNavigation.TabStop = false;
            this.gbDaysNavigation.Text = "Days navigation";
            // 
            // btnPrevDay
            // 
            this.btnPrevDay.BackColor = System.Drawing.SystemColors.Control;
            this.btnPrevDay.Location = new System.Drawing.Point(6, 23);
            this.btnPrevDay.Name = "btnPrevDay";
            this.btnPrevDay.Size = new System.Drawing.Size(105, 23);
            this.btnPrevDay.TabIndex = 0;
            this.btnPrevDay.Text = "<-";
            this.btnPrevDay.UseVisualStyleBackColor = false;
            this.btnPrevDay.Click += new System.EventHandler(this.btnPrevDay_Click);
            // 
            // btnNextDay
            // 
            this.btnNextDay.BackColor = System.Drawing.SystemColors.Control;
            this.btnNextDay.Location = new System.Drawing.Point(116, 23);
            this.btnNextDay.Name = "btnNextDay";
            this.btnNextDay.Size = new System.Drawing.Size(101, 23);
            this.btnNextDay.TabIndex = 1;
            this.btnNextDay.Text = "->";
            this.btnNextDay.UseVisualStyleBackColor = false;
            this.btnNextDay.Click += new System.EventHandler(this.btnNextDay_Click);
            // 
            // gbPageNavigation
            // 
            this.gbPageNavigation.Controls.Add(this.btnPrev);
            this.gbPageNavigation.Controls.Add(this.btnNext);
            this.gbPageNavigation.Location = new System.Drawing.Point(469, 628);
            this.gbPageNavigation.Name = "gbPageNavigation";
            this.gbPageNavigation.Size = new System.Drawing.Size(223, 52);
            this.gbPageNavigation.TabIndex = 41;
            this.gbPageNavigation.TabStop = false;
            this.gbPageNavigation.Text = "Page navigation";
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.SystemColors.Control;
            this.btnPrev.Location = new System.Drawing.Point(6, 23);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(105, 23);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.Text = "<-";
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.SystemColors.Control;
            this.btnNext.Location = new System.Drawing.Point(116, 23);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(101, 23);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "->";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(899, 651);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbLocation
            // 
            this.gbLocation.Controls.Add(this.btnLocationTree);
            this.gbLocation.Controls.Add(this.cbLocation);
            this.gbLocation.Location = new System.Drawing.Point(3, 523);
            this.gbLocation.Name = "gbLocation";
            this.gbLocation.Size = new System.Drawing.Size(227, 41);
            this.gbLocation.TabIndex = 43;
            this.gbLocation.TabStop = false;
            this.gbLocation.Text = "Location";
            // 
            // btnLocationTree
            // 
            this.btnLocationTree.Image = ((System.Drawing.Image)(resources.GetObject("btnLocationTree.Image")));
            this.btnLocationTree.Location = new System.Drawing.Point(196, 12);
            this.btnLocationTree.Name = "btnLocationTree";
            this.btnLocationTree.Size = new System.Drawing.Size(25, 24);
            this.btnLocationTree.TabIndex = 1;
            this.btnLocationTree.Click += new System.EventHandler(this.btnLocationTree_Click);
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.Location = new System.Drawing.Point(9, 14);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(180, 21);
            this.cbLocation.TabIndex = 0;
            // 
            // gbIsWrkHrs
            // 
            this.gbIsWrkHrs.Controls.Add(this.rbAll);
            this.gbIsWrkHrs.Controls.Add(this.rbNo);
            this.gbIsWrkHrs.Controls.Add(this.rbYes);
            this.gbIsWrkHrs.Location = new System.Drawing.Point(4, 567);
            this.gbIsWrkHrs.Name = "gbIsWrkHrs";
            this.gbIsWrkHrs.Size = new System.Drawing.Size(227, 42);
            this.gbIsWrkHrs.TabIndex = 44;
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
            this.rbAll.TabIndex = 2;
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
            // PresenceGraphForDayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbIsWrkHrs);
            this.Controls.Add(this.gbLocation);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbPageNavigation);
            this.Controls.Add(this.gbDaysNavigation);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.gbLegend);
            this.Controls.Add(this.gbGraphicReport);
            this.Controls.Add(this.gbDay);
            this.Controls.Add(this.gbEmployees);
            this.Controls.Add(this.gbWorkingUnits);
            this.Name = "PresenceGraphForDayControl";
            this.Size = new System.Drawing.Size(977, 682);
            this.Load += new System.EventHandler(this.PresenceGraphForDayControl_Load);
            this.gbWorkingUnits.ResumeLayout(false);
            this.gbWorkingUnits.PerformLayout();
            this.gbEmployees.ResumeLayout(false);
            this.gbEmployees.PerformLayout();
            this.gbDay.ResumeLayout(false);
            this.gbDay.PerformLayout();
            this.gbGraphicReport.ResumeLayout(false);
            this.gbGraphicReport.PerformLayout();
            this.gbLegend.ResumeLayout(false);
            this.gbLegend.PerformLayout();
            this.gbDaysNavigation.ResumeLayout(false);
            this.gbPageNavigation.ResumeLayout(false);
            this.gbLocation.ResumeLayout(false);
            this.gbIsWrkHrs.ResumeLayout(false);
            this.gbIsWrkHrs.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.GroupBox gbWorkingUnits;
        protected System.Windows.Forms.ListView lvWorkingUnits;
        protected System.Windows.Forms.CheckBox chbHierarhicly;
        protected System.Windows.Forms.Label lblParentWUID;
        protected System.Windows.Forms.ComboBox cbParentWorkingUnit;
        protected System.Windows.Forms.GroupBox gbEmployees;
        protected System.Windows.Forms.ListView lvEmployees;
        protected System.Windows.Forms.CheckBox chbSelectAll;
        protected System.Windows.Forms.GroupBox gbDay;
        protected System.Windows.Forms.CheckBox chbShowNextDay;
        protected System.Windows.Forms.DateTimePicker dtpDay;
        protected System.Windows.Forms.GroupBox gbGraphicReport;
        protected System.Windows.Forms.Label lblTotal;
        protected System.Windows.Forms.Label lblEmployee;
        protected System.Windows.Forms.GroupBox gbLegend;
        protected System.Windows.Forms.Label lblLate;
        protected System.Windows.Forms.Panel panelRegularWork;
        protected System.Windows.Forms.Panel panelLate;
        protected System.Windows.Forms.Label lblRegularWork;
        protected System.Windows.Forms.Label lblTheRestOfAllDayAbsence;
        protected System.Windows.Forms.Panel panelOfficialyOutgiong;
        protected System.Windows.Forms.Panel panelTheRestOfAllDayAbsence;
        protected System.Windows.Forms.Label lblOfficiallyOutgoing;
        protected System.Windows.Forms.Label lblVacation;
        protected System.Windows.Forms.Panel panelPause;
        protected System.Windows.Forms.Panel panelVacation;
        protected System.Windows.Forms.Label lblPause;
        protected System.Windows.Forms.Label lblSickLeave;
        protected System.Windows.Forms.Panel panelOutgoingInPrivate;
        protected System.Windows.Forms.Panel panelSickLeave;
        protected System.Windows.Forms.Label lblOutgoingInPrivate;
        protected System.Windows.Forms.Label lblTheRestOfReaderPasses;
        protected System.Windows.Forms.Panel panelTheRestOfReaderPasses;
        protected System.Windows.Forms.Button btnShow;
        protected System.Windows.Forms.Button btnPrint;
        protected System.Windows.Forms.GroupBox gbDaysNavigation;
        protected System.Windows.Forms.Button btnPrevDay;
        protected System.Windows.Forms.Button btnNextDay;
        protected System.Windows.Forms.GroupBox gbPageNavigation;
        protected System.Windows.Forms.Button btnPrev;
        protected System.Windows.Forms.Button btnNext;
        protected System.Windows.Forms.Button btnClose;
        protected System.Windows.Forms.Button btnWUTree;
        protected System.Windows.Forms.GroupBox gbLocation;
        protected System.Windows.Forms.Button btnLocationTree;
        protected System.Windows.Forms.ComboBox cbLocation;
        protected System.Windows.Forms.GroupBox gbIsWrkHrs;
        protected System.Windows.Forms.RadioButton rbNo;
        protected System.Windows.Forms.RadioButton rbYes;
        protected System.Windows.Forms.RadioButton rbAll;
    }
}
