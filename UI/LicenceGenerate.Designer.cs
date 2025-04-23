namespace UI
{
    partial class LicenceGenerate
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
            this.lblPortValue = new System.Windows.Forms.Label();
            this.lblServer = new System.Windows.Forms.Label();
            this.lblServerValue = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblNoSessionsCurr = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.gbSessions = new System.Windows.Forms.GroupBox();
            this.numSessions = new System.Windows.Forms.NumericUpDown();
            this.lblNoSessionsCurVal = new System.Windows.Forms.Label();
            this.lblSessionsNum = new System.Windows.Forms.Label();
            this.gbFunctionality = new System.Windows.Forms.GroupBox();
            this.chbMedicalCheck = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbRestaurant1 = new System.Windows.Forms.RadioButton();
            this.rbRestaurant2 = new System.Windows.Forms.RadioButton();
            this.cbEnterDataByEmpl = new System.Windows.Forms.CheckBox();
            this.cbVacation = new System.Windows.Forms.CheckBox();
            this.cbAdvance = new System.Windows.Forms.CheckBox();
            this.cbStandard = new System.Windows.Forms.CheckBox();
            this.cbBasic = new System.Windows.Forms.CheckBox();
            this.cbPeopleCounter = new System.Windows.Forms.CheckBox();
            this.rbTerminal = new System.Windows.Forms.RadioButton();
            this.rbTag = new System.Windows.Forms.RadioButton();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.cbRoutes = new System.Windows.Forms.CheckBox();
            this.cbRestaurant = new System.Windows.Forms.CheckBox();
            this.cbGraficalRep = new System.Windows.Forms.CheckBox();
            this.cbVisitors = new System.Windows.Forms.CheckBox();
            this.cbMonitoring = new System.Windows.Forms.CheckBox();
            this.cbSnapshots = new System.Windows.Forms.CheckBox();
            this.cbAccessControl = new System.Windows.Forms.CheckBox();
            this.cbExtraHours = new System.Windows.Forms.CheckBox();
            this.cbExitPermits = new System.Windows.Forms.CheckBox();
            this.cbAbsences = new System.Windows.Forms.CheckBox();
            this.cbTimeTable2 = new System.Windows.Forms.CheckBox();
            this.cbTimeTable1 = new System.Windows.Forms.CheckBox();
            this.gbCustomizedReports = new System.Windows.Forms.GroupBox();
            this.cbCustomer = new System.Windows.Forms.ComboBox();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.cbCustomizedReports = new System.Windows.Forms.CheckBox();
            this.cbProcessAllTags = new System.Windows.Forms.CheckBox();
            this.gbDataProcessing = new System.Windows.Forms.GroupBox();
            this.cbRecordsToProcess = new System.Windows.Forms.CheckBox();
            this.gbSiemensCompatibility = new System.Windows.Forms.GroupBox();
            this.cbSiemensCompatibility = new System.Windows.Forms.CheckBox();
            this.gbSessions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSessions)).BeginInit();
            this.gbFunctionality.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbCustomizedReports.SuspendLayout();
            this.gbDataProcessing.SuspendLayout();
            this.gbSiemensCompatibility.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPortValue
            // 
            this.lblPortValue.AutoSize = true;
            this.lblPortValue.Location = new System.Drawing.Point(177, 55);
            this.lblPortValue.Name = "lblPortValue";
            this.lblPortValue.Size = new System.Drawing.Size(25, 13);
            this.lblPortValue.TabIndex = 3;
            this.lblPortValue.Text = "port";
            this.lblPortValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(15, 29);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(41, 13);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "Server:";
            this.lblServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblServerValue
            // 
            this.lblServerValue.AutoSize = true;
            this.lblServerValue.Location = new System.Drawing.Point(177, 29);
            this.lblServerValue.Name = "lblServerValue";
            this.lblServerValue.Size = new System.Drawing.Size(65, 13);
            this.lblServerValue.TabIndex = 1;
            this.lblServerValue.Text = "server name";
            this.lblServerValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(15, 55);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port:";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNoSessionsCurr
            // 
            this.lblNoSessionsCurr.AutoSize = true;
            this.lblNoSessionsCurr.Location = new System.Drawing.Point(15, 78);
            this.lblNoSessionsCurr.Name = "lblNoSessionsCurr";
            this.lblNoSessionsCurr.Size = new System.Drawing.Size(110, 13);
            this.lblNoSessionsCurr.TabIndex = 4;
            this.lblNoSessionsCurr.Text = "Current sessions num:";
            this.lblNoSessionsCurr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(11, 706);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(125, 23);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "Generate licence";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(353, 706);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(246, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "*";
            // 
            // gbSessions
            // 
            this.gbSessions.Controls.Add(this.numSessions);
            this.gbSessions.Controls.Add(this.lblNoSessionsCurVal);
            this.gbSessions.Controls.Add(this.lblSessionsNum);
            this.gbSessions.Controls.Add(this.lblServer);
            this.gbSessions.Controls.Add(this.label2);
            this.gbSessions.Controls.Add(this.lblServerValue);
            this.gbSessions.Controls.Add(this.lblPort);
            this.gbSessions.Controls.Add(this.lblPortValue);
            this.gbSessions.Controls.Add(this.lblNoSessionsCurr);
            this.gbSessions.Location = new System.Drawing.Point(11, 4);
            this.gbSessions.Name = "gbSessions";
            this.gbSessions.Size = new System.Drawing.Size(413, 133);
            this.gbSessions.TabIndex = 0;
            this.gbSessions.TabStop = false;
            this.gbSessions.Text = "Number of sessions";
            // 
            // numSessions
            // 
            this.numSessions.Location = new System.Drawing.Point(173, 101);
            this.numSessions.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numSessions.Name = "numSessions";
            this.numSessions.Size = new System.Drawing.Size(64, 20);
            this.numSessions.TabIndex = 7;
            // 
            // lblNoSessionsCurVal
            // 
            this.lblNoSessionsCurVal.AutoSize = true;
            this.lblNoSessionsCurVal.Location = new System.Drawing.Point(177, 78);
            this.lblNoSessionsCurVal.Name = "lblNoSessionsCurVal";
            this.lblNoSessionsCurVal.Size = new System.Drawing.Size(13, 13);
            this.lblNoSessionsCurVal.TabIndex = 5;
            this.lblNoSessionsCurVal.Text = "0";
            // 
            // lblSessionsNum
            // 
            this.lblSessionsNum.AutoSize = true;
            this.lblSessionsNum.Location = new System.Drawing.Point(15, 103);
            this.lblSessionsNum.Name = "lblSessionsNum";
            this.lblSessionsNum.Size = new System.Drawing.Size(75, 13);
            this.lblSessionsNum.TabIndex = 6;
            this.lblSessionsNum.Text = "Sessions num:";
            // 
            // gbFunctionality
            // 
            this.gbFunctionality.Controls.Add(this.chbMedicalCheck);
            this.gbFunctionality.Controls.Add(this.panel1);
            this.gbFunctionality.Controls.Add(this.cbEnterDataByEmpl);
            this.gbFunctionality.Controls.Add(this.cbVacation);
            this.gbFunctionality.Controls.Add(this.cbAdvance);
            this.gbFunctionality.Controls.Add(this.cbStandard);
            this.gbFunctionality.Controls.Add(this.cbBasic);
            this.gbFunctionality.Controls.Add(this.cbPeopleCounter);
            this.gbFunctionality.Controls.Add(this.rbTerminal);
            this.gbFunctionality.Controls.Add(this.rbTag);
            this.gbFunctionality.Controls.Add(this.btnSelectAll);
            this.gbFunctionality.Controls.Add(this.btnClear);
            this.gbFunctionality.Controls.Add(this.cbRoutes);
            this.gbFunctionality.Controls.Add(this.cbRestaurant);
            this.gbFunctionality.Controls.Add(this.cbGraficalRep);
            this.gbFunctionality.Controls.Add(this.cbVisitors);
            this.gbFunctionality.Controls.Add(this.cbMonitoring);
            this.gbFunctionality.Controls.Add(this.cbSnapshots);
            this.gbFunctionality.Controls.Add(this.cbAccessControl);
            this.gbFunctionality.Controls.Add(this.cbExtraHours);
            this.gbFunctionality.Controls.Add(this.cbExitPermits);
            this.gbFunctionality.Controls.Add(this.cbAbsences);
            this.gbFunctionality.Controls.Add(this.cbTimeTable2);
            this.gbFunctionality.Controls.Add(this.cbTimeTable1);
            this.gbFunctionality.Location = new System.Drawing.Point(11, 139);
            this.gbFunctionality.Name = "gbFunctionality";
            this.gbFunctionality.Size = new System.Drawing.Size(413, 397);
            this.gbFunctionality.TabIndex = 1;
            this.gbFunctionality.TabStop = false;
            this.gbFunctionality.Text = "Functionality";
            // 
            // chbMedicalCheck
            // 
            this.chbMedicalCheck.AutoSize = true;
            this.chbMedicalCheck.Location = new System.Drawing.Point(18, 367);
            this.chbMedicalCheck.Name = "chbMedicalCheck";
            this.chbMedicalCheck.Size = new System.Drawing.Size(96, 17);
            this.chbMedicalCheck.TabIndex = 22;
            this.chbMedicalCheck.Text = "Medical check";
            this.chbMedicalCheck.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbRestaurant1);
            this.panel1.Controls.Add(this.rbRestaurant2);
            this.panel1.Location = new System.Drawing.Point(122, 248);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(159, 23);
            this.panel1.TabIndex = 24;
            // 
            // rbRestaurant1
            // 
            this.rbRestaurant1.AutoSize = true;
            this.rbRestaurant1.Location = new System.Drawing.Point(19, 4);
            this.rbRestaurant1.Name = "rbRestaurant1";
            this.rbRestaurant1.Size = new System.Drawing.Size(31, 17);
            this.rbRestaurant1.TabIndex = 11;
            this.rbRestaurant1.TabStop = true;
            this.rbRestaurant1.Text = "1";
            this.rbRestaurant1.UseVisualStyleBackColor = true;
            // 
            // rbRestaurant2
            // 
            this.rbRestaurant2.AutoSize = true;
            this.rbRestaurant2.Location = new System.Drawing.Point(98, 4);
            this.rbRestaurant2.Name = "rbRestaurant2";
            this.rbRestaurant2.Size = new System.Drawing.Size(31, 17);
            this.rbRestaurant2.TabIndex = 12;
            this.rbRestaurant2.TabStop = true;
            this.rbRestaurant2.Text = "2";
            this.rbRestaurant2.UseVisualStyleBackColor = true;
            // 
            // cbEnterDataByEmpl
            // 
            this.cbEnterDataByEmpl.AutoSize = true;
            this.cbEnterDataByEmpl.Location = new System.Drawing.Point(18, 345);
            this.cbEnterDataByEmpl.Name = "cbEnterDataByEmpl";
            this.cbEnterDataByEmpl.Size = new System.Drawing.Size(137, 17);
            this.cbEnterDataByEmpl.TabIndex = 21;
            this.cbEnterDataByEmpl.Text = "Enter data by employee";
            this.cbEnterDataByEmpl.UseVisualStyleBackColor = true;
            // 
            // cbVacation
            // 
            this.cbVacation.AutoSize = true;
            this.cbVacation.Location = new System.Drawing.Point(18, 322);
            this.cbVacation.Name = "cbVacation";
            this.cbVacation.Size = new System.Drawing.Size(68, 17);
            this.cbVacation.TabIndex = 20;
            this.cbVacation.Text = "Vacation";
            this.cbVacation.UseVisualStyleBackColor = true;
            // 
            // cbAdvance
            // 
            this.cbAdvance.AutoSize = true;
            this.cbAdvance.Location = new System.Drawing.Point(311, 299);
            this.cbAdvance.Name = "cbAdvance";
            this.cbAdvance.Size = new System.Drawing.Size(68, 17);
            this.cbAdvance.TabIndex = 19;
            this.cbAdvance.Text = "advance";
            this.cbAdvance.UseVisualStyleBackColor = true;
            this.cbAdvance.CheckedChanged += new System.EventHandler(this.cbAdvance_CheckedChanged);
            // 
            // cbStandard
            // 
            this.cbStandard.AutoSize = true;
            this.cbStandard.Location = new System.Drawing.Point(220, 299);
            this.cbStandard.Name = "cbStandard";
            this.cbStandard.Size = new System.Drawing.Size(67, 17);
            this.cbStandard.TabIndex = 18;
            this.cbStandard.Text = "standard";
            this.cbStandard.UseVisualStyleBackColor = true;
            this.cbStandard.CheckedChanged += new System.EventHandler(this.cbStandard_CheckedChanged);
            // 
            // cbBasic
            // 
            this.cbBasic.AutoSize = true;
            this.cbBasic.Location = new System.Drawing.Point(141, 299);
            this.cbBasic.Name = "cbBasic";
            this.cbBasic.Size = new System.Drawing.Size(51, 17);
            this.cbBasic.TabIndex = 17;
            this.cbBasic.Text = "basic";
            this.cbBasic.UseVisualStyleBackColor = true;
            this.cbBasic.CheckedChanged += new System.EventHandler(this.cbBasic_CheckedChanged);
            // 
            // cbPeopleCounter
            // 
            this.cbPeopleCounter.AutoSize = true;
            this.cbPeopleCounter.Location = new System.Drawing.Point(18, 299);
            this.cbPeopleCounter.Name = "cbPeopleCounter";
            this.cbPeopleCounter.Size = new System.Drawing.Size(98, 17);
            this.cbPeopleCounter.TabIndex = 16;
            this.cbPeopleCounter.Text = "People counter";
            this.cbPeopleCounter.UseVisualStyleBackColor = true;
            this.cbPeopleCounter.CheckedChanged += new System.EventHandler(this.cbPeopleCounter_CheckedChanged);
            // 
            // rbTerminal
            // 
            this.rbTerminal.AutoSize = true;
            this.rbTerminal.Location = new System.Drawing.Point(220, 276);
            this.rbTerminal.Name = "rbTerminal";
            this.rbTerminal.Size = new System.Drawing.Size(61, 17);
            this.rbTerminal.TabIndex = 15;
            this.rbTerminal.TabStop = true;
            this.rbTerminal.Text = "terminal";
            this.rbTerminal.UseVisualStyleBackColor = true;
            this.rbTerminal.CheckedChanged += new System.EventHandler(this.rbTerminal_CheckedChanged);
            // 
            // rbTag
            // 
            this.rbTag.AutoSize = true;
            this.rbTag.Location = new System.Drawing.Point(141, 276);
            this.rbTag.Name = "rbTag";
            this.rbTag.Size = new System.Drawing.Size(40, 17);
            this.rbTag.TabIndex = 14;
            this.rbTag.TabStop = true;
            this.rbTag.Text = "tag";
            this.rbTag.UseVisualStyleBackColor = true;
            this.rbTag.CheckedChanged += new System.EventHandler(this.rbTag_CheckedChanged);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(250, 367);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 23;
            this.btnSelectAll.Text = "Select all";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(332, 367);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 24;
            this.btnClear.Text = "Clear all";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cbRoutes
            // 
            this.cbRoutes.AutoSize = true;
            this.cbRoutes.Location = new System.Drawing.Point(18, 276);
            this.cbRoutes.Name = "cbRoutes";
            this.cbRoutes.Size = new System.Drawing.Size(60, 17);
            this.cbRoutes.TabIndex = 13;
            this.cbRoutes.Text = "Routes";
            this.cbRoutes.UseVisualStyleBackColor = true;
            this.cbRoutes.CheckedChanged += new System.EventHandler(this.cbRoutes_CheckedChanged);
            // 
            // cbRestaurant
            // 
            this.cbRestaurant.AutoSize = true;
            this.cbRestaurant.Location = new System.Drawing.Point(18, 253);
            this.cbRestaurant.Name = "cbRestaurant";
            this.cbRestaurant.Size = new System.Drawing.Size(78, 17);
            this.cbRestaurant.TabIndex = 10;
            this.cbRestaurant.Text = "Restaurant";
            this.cbRestaurant.UseVisualStyleBackColor = true;
            this.cbRestaurant.CheckedChanged += new System.EventHandler(this.cbRestaurant_CheckedChanged);
            // 
            // cbGraficalRep
            // 
            this.cbGraficalRep.AutoSize = true;
            this.cbGraficalRep.Location = new System.Drawing.Point(18, 230);
            this.cbGraficalRep.Name = "cbGraficalRep";
            this.cbGraficalRep.Size = new System.Drawing.Size(97, 17);
            this.cbGraficalRep.TabIndex = 9;
            this.cbGraficalRep.Text = "Grafical reports";
            this.cbGraficalRep.UseVisualStyleBackColor = true;
            // 
            // cbVisitors
            // 
            this.cbVisitors.AutoSize = true;
            this.cbVisitors.Location = new System.Drawing.Point(18, 207);
            this.cbVisitors.Name = "cbVisitors";
            this.cbVisitors.Size = new System.Drawing.Size(59, 17);
            this.cbVisitors.TabIndex = 8;
            this.cbVisitors.Text = "Visitors";
            this.cbVisitors.UseVisualStyleBackColor = true;
            // 
            // cbMonitoring
            // 
            this.cbMonitoring.AutoSize = true;
            this.cbMonitoring.Location = new System.Drawing.Point(18, 184);
            this.cbMonitoring.Name = "cbMonitoring";
            this.cbMonitoring.Size = new System.Drawing.Size(75, 17);
            this.cbMonitoring.TabIndex = 7;
            this.cbMonitoring.Text = "Monitoring";
            this.cbMonitoring.UseVisualStyleBackColor = true;
            // 
            // cbSnapshots
            // 
            this.cbSnapshots.AutoSize = true;
            this.cbSnapshots.Location = new System.Drawing.Point(18, 162);
            this.cbSnapshots.Name = "cbSnapshots";
            this.cbSnapshots.Size = new System.Drawing.Size(76, 17);
            this.cbSnapshots.TabIndex = 6;
            this.cbSnapshots.Text = "Snapshots";
            this.cbSnapshots.UseVisualStyleBackColor = true;
            // 
            // cbAccessControl
            // 
            this.cbAccessControl.AutoSize = true;
            this.cbAccessControl.Location = new System.Drawing.Point(18, 139);
            this.cbAccessControl.Name = "cbAccessControl";
            this.cbAccessControl.Size = new System.Drawing.Size(97, 17);
            this.cbAccessControl.TabIndex = 5;
            this.cbAccessControl.Text = "Access Control";
            this.cbAccessControl.UseVisualStyleBackColor = true;
            // 
            // cbExtraHours
            // 
            this.cbExtraHours.AutoSize = true;
            this.cbExtraHours.Location = new System.Drawing.Point(18, 116);
            this.cbExtraHours.Name = "cbExtraHours";
            this.cbExtraHours.Size = new System.Drawing.Size(81, 17);
            this.cbExtraHours.TabIndex = 4;
            this.cbExtraHours.Text = "Extra Hours";
            this.cbExtraHours.UseVisualStyleBackColor = true;
            // 
            // cbExitPermits
            // 
            this.cbExitPermits.AutoSize = true;
            this.cbExitPermits.Location = new System.Drawing.Point(18, 93);
            this.cbExitPermits.Name = "cbExitPermits";
            this.cbExitPermits.Size = new System.Drawing.Size(80, 17);
            this.cbExitPermits.TabIndex = 3;
            this.cbExitPermits.Text = "Exit Permits";
            this.cbExitPermits.UseVisualStyleBackColor = true;
            // 
            // cbAbsences
            // 
            this.cbAbsences.AutoSize = true;
            this.cbAbsences.Location = new System.Drawing.Point(18, 70);
            this.cbAbsences.Name = "cbAbsences";
            this.cbAbsences.Size = new System.Drawing.Size(73, 17);
            this.cbAbsences.TabIndex = 2;
            this.cbAbsences.Text = "Absences";
            this.cbAbsences.UseVisualStyleBackColor = true;
            // 
            // cbTimeTable2
            // 
            this.cbTimeTable2.AutoSize = true;
            this.cbTimeTable2.Location = new System.Drawing.Point(18, 47);
            this.cbTimeTable2.Name = "cbTimeTable2";
            this.cbTimeTable2.Size = new System.Drawing.Size(138, 17);
            this.cbTimeTable2.TabIndex = 1;
            this.cbTimeTable2.Text = "Time Table 2 (industrial)";
            this.cbTimeTable2.UseVisualStyleBackColor = true;
            // 
            // cbTimeTable1
            // 
            this.cbTimeTable1.AutoSize = true;
            this.cbTimeTable1.Location = new System.Drawing.Point(18, 24);
            this.cbTimeTable1.Name = "cbTimeTable1";
            this.cbTimeTable1.Size = new System.Drawing.Size(129, 17);
            this.cbTimeTable1.TabIndex = 0;
            this.cbTimeTable1.Text = "Time Table 1 (regular)";
            this.cbTimeTable1.UseVisualStyleBackColor = true;
            // 
            // gbCustomizedReports
            // 
            this.gbCustomizedReports.Controls.Add(this.cbCustomer);
            this.gbCustomizedReports.Controls.Add(this.lblCustomer);
            this.gbCustomizedReports.Controls.Add(this.cbCustomizedReports);
            this.gbCustomizedReports.Location = new System.Drawing.Point(11, 542);
            this.gbCustomizedReports.Name = "gbCustomizedReports";
            this.gbCustomizedReports.Size = new System.Drawing.Size(413, 50);
            this.gbCustomizedReports.TabIndex = 2;
            this.gbCustomizedReports.TabStop = false;
            this.gbCustomizedReports.Text = "Customizes reports";
            // 
            // cbCustomer
            // 
            this.cbCustomer.FormattingEnabled = true;
            this.cbCustomer.Location = new System.Drawing.Point(250, 18);
            this.cbCustomer.Name = "cbCustomer";
            this.cbCustomer.Size = new System.Drawing.Size(157, 21);
            this.cbCustomer.TabIndex = 2;
            // 
            // lblCustomer
            // 
            this.lblCustomer.Location = new System.Drawing.Point(170, 17);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(74, 23);
            this.lblCustomer.TabIndex = 1;
            this.lblCustomer.Text = "Customer:";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbCustomizedReports
            // 
            this.cbCustomizedReports.AutoSize = true;
            this.cbCustomizedReports.Location = new System.Drawing.Point(18, 21);
            this.cbCustomizedReports.Name = "cbCustomizedReports";
            this.cbCustomizedReports.Size = new System.Drawing.Size(115, 17);
            this.cbCustomizedReports.TabIndex = 0;
            this.cbCustomizedReports.Text = "Customized reports";
            this.cbCustomizedReports.UseVisualStyleBackColor = true;
            this.cbCustomizedReports.CheckedChanged += new System.EventHandler(this.cbCustomizedReports_CheckedChanged);
            // 
            // cbProcessAllTags
            // 
            this.cbProcessAllTags.AutoSize = true;
            this.cbProcessAllTags.Location = new System.Drawing.Point(18, 20);
            this.cbProcessAllTags.Name = "cbProcessAllTags";
            this.cbProcessAllTags.Size = new System.Drawing.Size(107, 17);
            this.cbProcessAllTags.TabIndex = 0;
            this.cbProcessAllTags.Text = "Include zero tags";
            this.cbProcessAllTags.UseVisualStyleBackColor = true;
            // 
            // gbDataProcessing
            // 
            this.gbDataProcessing.Controls.Add(this.cbRecordsToProcess);
            this.gbDataProcessing.Controls.Add(this.cbProcessAllTags);
            this.gbDataProcessing.Location = new System.Drawing.Point(11, 598);
            this.gbDataProcessing.Name = "gbDataProcessing";
            this.gbDataProcessing.Size = new System.Drawing.Size(413, 50);
            this.gbDataProcessing.TabIndex = 3;
            this.gbDataProcessing.TabStop = false;
            this.gbDataProcessing.Text = "Data processing";
            // 
            // cbRecordsToProcess
            // 
            this.cbRecordsToProcess.AutoSize = true;
            this.cbRecordsToProcess.Location = new System.Drawing.Point(220, 19);
            this.cbRecordsToProcess.Name = "cbRecordsToProcess";
            this.cbRecordsToProcess.Size = new System.Drawing.Size(115, 17);
            this.cbRecordsToProcess.TabIndex = 1;
            this.cbRecordsToProcess.Text = "Processing in parts";
            this.cbRecordsToProcess.UseVisualStyleBackColor = true;
            // 
            // gbSiemensCompatibility
            // 
            this.gbSiemensCompatibility.Controls.Add(this.cbSiemensCompatibility);
            this.gbSiemensCompatibility.Location = new System.Drawing.Point(11, 650);
            this.gbSiemensCompatibility.Name = "gbSiemensCompatibility";
            this.gbSiemensCompatibility.Size = new System.Drawing.Size(413, 50);
            this.gbSiemensCompatibility.TabIndex = 6;
            this.gbSiemensCompatibility.TabStop = false;
            this.gbSiemensCompatibility.Text = "Siemens compatibility";
            // 
            // cbSiemensCompatibility
            // 
            this.cbSiemensCompatibility.AutoSize = true;
            this.cbSiemensCompatibility.Location = new System.Drawing.Point(18, 20);
            this.cbSiemensCompatibility.Name = "cbSiemensCompatibility";
            this.cbSiemensCompatibility.Size = new System.Drawing.Size(120, 17);
            this.cbSiemensCompatibility.TabIndex = 0;
            this.cbSiemensCompatibility.Text = "Siemens compatible";
            this.cbSiemensCompatibility.UseVisualStyleBackColor = true;
            this.cbSiemensCompatibility.CheckedChanged += new System.EventHandler(this.chbSiemensCompatibility_CheckedChanged);
            // 
            // LicenceGenerate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 741);
            this.ControlBox = false;
            this.Controls.Add(this.gbSiemensCompatibility);
            this.Controls.Add(this.gbDataProcessing);
            this.Controls.Add(this.gbCustomizedReports);
            this.Controls.Add(this.gbFunctionality);
            this.Controls.Add(this.gbSessions);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "LicenceGenerate";
            this.ShowInTaskbar = false;
            this.Text = "3850289001390";
            this.Load += new System.EventHandler(this.LicenceGenerate_Load);
            this.gbSessions.ResumeLayout(false);
            this.gbSessions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSessions)).EndInit();
            this.gbFunctionality.ResumeLayout(false);
            this.gbFunctionality.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbCustomizedReports.ResumeLayout(false);
            this.gbCustomizedReports.PerformLayout();
            this.gbDataProcessing.ResumeLayout(false);
            this.gbDataProcessing.PerformLayout();
            this.gbSiemensCompatibility.ResumeLayout(false);
            this.gbSiemensCompatibility.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblServerValue;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblNoSessionsCurr;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPortValue;
        private System.Windows.Forms.GroupBox gbSessions;
        private System.Windows.Forms.GroupBox gbFunctionality;
        private System.Windows.Forms.CheckBox cbVisitors;
        private System.Windows.Forms.CheckBox cbMonitoring;
        private System.Windows.Forms.CheckBox cbSnapshots;
        private System.Windows.Forms.CheckBox cbAccessControl;
        private System.Windows.Forms.CheckBox cbExtraHours;
        private System.Windows.Forms.CheckBox cbExitPermits;
        private System.Windows.Forms.CheckBox cbAbsences;
        private System.Windows.Forms.CheckBox cbTimeTable2;
        private System.Windows.Forms.CheckBox cbTimeTable1;
        private System.Windows.Forms.Label lblNoSessionsCurVal;
        private System.Windows.Forms.Label lblSessionsNum;
        private System.Windows.Forms.GroupBox gbCustomizedReports;
        private System.Windows.Forms.CheckBox cbCustomizedReports;
        private System.Windows.Forms.ComboBox cbCustomer;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.CheckBox cbGraficalRep;
        private System.Windows.Forms.CheckBox cbRestaurant;
        private System.Windows.Forms.CheckBox cbRoutes;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.NumericUpDown numSessions;
        private System.Windows.Forms.RadioButton rbTerminal;
        private System.Windows.Forms.RadioButton rbTag;
        private System.Windows.Forms.CheckBox cbPeopleCounter;
        private System.Windows.Forms.CheckBox cbAdvance;
        private System.Windows.Forms.CheckBox cbStandard;
        private System.Windows.Forms.CheckBox cbBasic;
        private System.Windows.Forms.CheckBox cbVacation;
        private System.Windows.Forms.CheckBox cbProcessAllTags;
        private System.Windows.Forms.GroupBox gbDataProcessing;
        private System.Windows.Forms.GroupBox gbSiemensCompatibility;
        private System.Windows.Forms.CheckBox cbSiemensCompatibility;
        private System.Windows.Forms.CheckBox cbRecordsToProcess;
        private System.Windows.Forms.CheckBox cbEnterDataByEmpl;
        private System.Windows.Forms.RadioButton rbRestaurant2;
        private System.Windows.Forms.RadioButton rbRestaurant1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chbMedicalCheck;
    }
}