namespace UI
{
    partial class PYIntegration
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
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.gbType = new System.Windows.Forms.GroupBox();
            this.rbReal = new System.Windows.Forms.RadioButton();
            this.rbEstimated = new System.Windows.Forms.RadioButton();
            this.gbEmployees = new System.Windows.Forms.GroupBox();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.tbEmployee = new System.Windows.Forms.TextBox();
            this.rbSelected = new System.Windows.Forms.RadioButton();
            this.rbAllEmployees = new System.Windows.Forms.RadioButton();
            this.lblPath = new System.Windows.Forms.Label();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnGenerateData = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tc = new System.Windows.Forms.TabControl();
            this.tpCreateReports = new System.Windows.Forms.TabPage();
            this.btnSWBrowse = new System.Windows.Forms.Button();
            this.lblSWPath = new System.Windows.Forms.Label();
            this.tbSWPath = new System.Windows.Forms.TextBox();
            this.cbSWPay = new System.Windows.Forms.CheckBox();
            this.chbSWPayment = new System.Windows.Forms.CheckBox();
            this.chbBHPayment = new System.Windows.Forms.CheckBox();
            this.chbSWPayRegular = new System.Windows.Forms.CheckBox();
            this.numSWBalanceMonth = new System.Windows.Forms.NumericUpDown();
            this.lblSWBalanceMonth = new System.Windows.Forms.Label();
            this.numBHBalanceMonth = new System.Windows.Forms.NumericUpDown();
            this.lblBHBalanceMonth = new System.Windows.Forms.Label();
            this.chbBHPayRegular = new System.Windows.Forms.CheckBox();
            this.btnBHBrowse = new System.Windows.Forms.Button();
            this.lblBHPath = new System.Windows.Forms.Label();
            this.tbBHPath = new System.Windows.Forms.TextBox();
            this.chbWorkAnalysisReport = new System.Windows.Forms.CheckBox();
            this.chbStopWorkingPay = new System.Windows.Forms.CheckBox();
            this.chbVacationPay = new System.Windows.Forms.CheckBox();
            this.gbDate = new System.Windows.Forms.GroupBox();
            this.gbCompany = new System.Windows.Forms.GroupBox();
            this.chbShowLeavingEmployees = new System.Windows.Forms.CheckBox();
            this.dtpBorderDay = new System.Windows.Forms.DateTimePicker();
            this.lblBorderDate = new System.Windows.Forms.Label();
            this.lvCompany = new System.Windows.Forms.ListView();
            this.cbBankHoursPay = new System.Windows.Forms.CheckBox();
            this.tpRestarCounters = new System.Windows.Forms.TabPage();
            this.gbSWPay = new System.Windows.Forms.GroupBox();
            this.btnRecalculateSWPay = new System.Windows.Forms.Button();
            this.numSWPayCalcID = new System.Windows.Forms.NumericUpDown();
            this.lblSWPayCalcID = new System.Windows.Forms.Label();
            this.gbStopWorkingCutOffMonth = new System.Windows.Forms.GroupBox();
            this.btnRecalculateSW = new System.Windows.Forms.Button();
            this.numSWCalcID = new System.Windows.Forms.NumericUpDown();
            this.lblSWCalcID = new System.Windows.Forms.Label();
            this.gbLeavingEmployees = new System.Windows.Forms.GroupBox();
            this.btnLeavingEmployeesRecalculate = new System.Windows.Forms.Button();
            this.numLeavingEmployeesCalcID = new System.Windows.Forms.NumericUpDown();
            this.lblLeavingEmployeesCalcID = new System.Windows.Forms.Label();
            this.gbNotJustifiedOverTimeCounter = new System.Windows.Forms.GroupBox();
            this.btnRecalculateNJOvertime = new System.Windows.Forms.Button();
            this.numNJOvertime = new System.Windows.Forms.NumericUpDown();
            this.lblCalcIDNJOvertime = new System.Windows.Forms.Label();
            this.gbPaidLeaves = new System.Windows.Forms.GroupBox();
            this.btnRecalculatePaidLeaves = new System.Windows.Forms.Button();
            this.gbVacationCutOffMonth = new System.Windows.Forms.GroupBox();
            this.btnRecalculateVac = new System.Windows.Forms.Button();
            this.numVacCutOffDate = new System.Windows.Forms.NumericUpDown();
            this.lblVacCalcID = new System.Windows.Forms.Label();
            this.gbBHCuttOffMonths = new System.Windows.Forms.GroupBox();
            this.btnRecalculateBH = new System.Windows.Forms.Button();
            this.nudBHCalcID = new System.Windows.Forms.NumericUpDown();
            this.lblCalcIDBH = new System.Windows.Forms.Label();
            this.gbRestartOvertimeCounter = new System.Windows.Forms.GroupBox();
            this.btnRecalculate = new System.Windows.Forms.Button();
            this.nudCalcID = new System.Windows.Forms.NumericUpDown();
            this.lblCalcID = new System.Windows.Forms.Label();
            this.tabAdditionalReports = new System.Windows.Forms.TabPage();
            this.gbAbsence = new System.Windows.Forms.GroupBox();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.btnGenerateAbsent = new System.Windows.Forms.Button();
            this.lblCalcIDAbsent = new System.Windows.Forms.Label();
            this.lblWUAbsent = new System.Windows.Forms.Label();
            this.numAbsentCalcID = new System.Windows.Forms.NumericUpDown();
            this.lvPTAbsent = new System.Windows.Forms.ListView();
            this.lblPTAbsent = new System.Windows.Forms.Label();
            this.gbType.SuspendLayout();
            this.gbEmployees.SuspendLayout();
            this.tc.SuspendLayout();
            this.tpCreateReports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSWBalanceMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBHBalanceMonth)).BeginInit();
            this.gbDate.SuspendLayout();
            this.gbCompany.SuspendLayout();
            this.tpRestarCounters.SuspendLayout();
            this.gbSWPay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSWPayCalcID)).BeginInit();
            this.gbStopWorkingCutOffMonth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSWCalcID)).BeginInit();
            this.gbLeavingEmployees.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLeavingEmployeesCalcID)).BeginInit();
            this.gbNotJustifiedOverTimeCounter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNJOvertime)).BeginInit();
            this.gbPaidLeaves.SuspendLayout();
            this.gbVacationCutOffMonth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVacCutOffDate)).BeginInit();
            this.gbBHCuttOffMonths.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBHCalcID)).BeginInit();
            this.gbRestartOvertimeCounter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCalcID)).BeginInit();
            this.tabAdditionalReports.SuspendLayout();
            this.gbAbsence.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAbsentCalcID)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(15, 44);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(41, 23);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(17, 18);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 23);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "dd.MM.yyyy";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(63, 47);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(101, 20);
            this.dtTo.TabIndex = 3;
            this.dtTo.ValueChanged += new System.EventHandler(this.dtTo_ValueChanged);
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "dd.MM.yyyy";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(63, 21);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(101, 20);
            this.dtFrom.TabIndex = 1;
            this.dtFrom.ValueChanged += new System.EventHandler(this.dtFrom_ValueChanged);
            // 
            // gbType
            // 
            this.gbType.Controls.Add(this.rbReal);
            this.gbType.Controls.Add(this.rbEstimated);
            this.gbType.Location = new System.Drawing.Point(6, 280);
            this.gbType.Name = "gbType";
            this.gbType.Size = new System.Drawing.Size(216, 67);
            this.gbType.TabIndex = 3;
            this.gbType.TabStop = false;
            this.gbType.Text = "Type";
            // 
            // rbReal
            // 
            this.rbReal.AutoSize = true;
            this.rbReal.Checked = true;
            this.rbReal.Location = new System.Drawing.Point(23, 42);
            this.rbReal.Name = "rbReal";
            this.rbReal.Size = new System.Drawing.Size(47, 17);
            this.rbReal.TabIndex = 1;
            this.rbReal.TabStop = true;
            this.rbReal.Text = "Real";
            this.rbReal.UseVisualStyleBackColor = true;
            // 
            // rbEstimated
            // 
            this.rbEstimated.AutoSize = true;
            this.rbEstimated.Location = new System.Drawing.Point(23, 19);
            this.rbEstimated.Name = "rbEstimated";
            this.rbEstimated.Size = new System.Drawing.Size(71, 17);
            this.rbEstimated.TabIndex = 0;
            this.rbEstimated.TabStop = true;
            this.rbEstimated.Text = "Estimated";
            this.rbEstimated.UseVisualStyleBackColor = true;
            // 
            // gbEmployees
            // 
            this.gbEmployees.Controls.Add(this.lvEmployees);
            this.gbEmployees.Controls.Add(this.lblEmployee);
            this.gbEmployees.Controls.Add(this.tbEmployee);
            this.gbEmployees.Controls.Add(this.rbSelected);
            this.gbEmployees.Controls.Add(this.rbAllEmployees);
            this.gbEmployees.Location = new System.Drawing.Point(228, 6);
            this.gbEmployees.Name = "gbEmployees";
            this.gbEmployees.Size = new System.Drawing.Size(314, 341);
            this.gbEmployees.TabIndex = 1;
            this.gbEmployees.TabStop = false;
            this.gbEmployees.Text = "Employees";
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(9, 48);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.ShowItemToolTips = true;
            this.lvEmployees.Size = new System.Drawing.Size(299, 262);
            this.lvEmployees.TabIndex = 2;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(6, 20);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(71, 23);
            this.lblEmployee.TabIndex = 0;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbEmployee
            // 
            this.tbEmployee.Enabled = false;
            this.tbEmployee.Location = new System.Drawing.Point(83, 22);
            this.tbEmployee.Name = "tbEmployee";
            this.tbEmployee.Size = new System.Drawing.Size(204, 20);
            this.tbEmployee.TabIndex = 1;
            this.tbEmployee.TextChanged += new System.EventHandler(this.tbEmployees_TextChanged);
            // 
            // rbSelected
            // 
            this.rbSelected.AutoSize = true;
            this.rbSelected.Location = new System.Drawing.Point(51, 317);
            this.rbSelected.Name = "rbSelected";
            this.rbSelected.Size = new System.Drawing.Size(67, 17);
            this.rbSelected.TabIndex = 4;
            this.rbSelected.TabStop = true;
            this.rbSelected.Text = "Selected";
            this.rbSelected.UseVisualStyleBackColor = true;
            this.rbSelected.CheckedChanged += new System.EventHandler(this.rbSelected_CheckedChanged);
            // 
            // rbAllEmployees
            // 
            this.rbAllEmployees.AutoSize = true;
            this.rbAllEmployees.Checked = true;
            this.rbAllEmployees.Location = new System.Drawing.Point(9, 317);
            this.rbAllEmployees.Name = "rbAllEmployees";
            this.rbAllEmployees.Size = new System.Drawing.Size(36, 17);
            this.rbAllEmployees.TabIndex = 3;
            this.rbAllEmployees.TabStop = true;
            this.rbAllEmployees.Text = "All";
            this.rbAllEmployees.UseVisualStyleBackColor = true;
            // 
            // lblPath
            // 
            this.lblPath.Location = new System.Drawing.Point(7, 350);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(71, 23);
            this.lblPath.TabIndex = 4;
            this.lblPath.Text = "Path:";
            this.lblPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbPath
            // 
            this.tbPath.Location = new System.Drawing.Point(84, 353);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(377, 20);
            this.tbPath.TabIndex = 5;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(467, 350);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 6;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnGenerateData
            // 
            this.btnGenerateData.Location = new System.Drawing.Point(10, 475);
            this.btnGenerateData.Name = "btnGenerateData";
            this.btnGenerateData.Size = new System.Drawing.Size(113, 23);
            this.btnGenerateData.TabIndex = 24;
            this.btnGenerateData.Text = "Generate data";
            this.btnGenerateData.UseVisualStyleBackColor = true;
            this.btnGenerateData.Click += new System.EventHandler(this.btnGenerateData_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(488, 555);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(93, 23);
            this.btnClose.TabIndex = 25;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tc
            // 
            this.tc.Controls.Add(this.tpCreateReports);
            this.tc.Controls.Add(this.tpRestarCounters);
            this.tc.Controls.Add(this.tabAdditionalReports);
            this.tc.Location = new System.Drawing.Point(17, 7);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(566, 542);
            this.tc.TabIndex = 26;
            // 
            // tpCreateReports
            // 
            this.tpCreateReports.Controls.Add(this.btnSWBrowse);
            this.tpCreateReports.Controls.Add(this.lblSWPath);
            this.tpCreateReports.Controls.Add(this.tbSWPath);
            this.tpCreateReports.Controls.Add(this.cbSWPay);
            this.tpCreateReports.Controls.Add(this.chbSWPayment);
            this.tpCreateReports.Controls.Add(this.chbBHPayment);
            this.tpCreateReports.Controls.Add(this.chbSWPayRegular);
            this.tpCreateReports.Controls.Add(this.numSWBalanceMonth);
            this.tpCreateReports.Controls.Add(this.lblSWBalanceMonth);
            this.tpCreateReports.Controls.Add(this.numBHBalanceMonth);
            this.tpCreateReports.Controls.Add(this.lblBHBalanceMonth);
            this.tpCreateReports.Controls.Add(this.chbBHPayRegular);
            this.tpCreateReports.Controls.Add(this.btnBHBrowse);
            this.tpCreateReports.Controls.Add(this.lblBHPath);
            this.tpCreateReports.Controls.Add(this.tbBHPath);
            this.tpCreateReports.Controls.Add(this.chbWorkAnalysisReport);
            this.tpCreateReports.Controls.Add(this.chbStopWorkingPay);
            this.tpCreateReports.Controls.Add(this.chbVacationPay);
            this.tpCreateReports.Controls.Add(this.gbDate);
            this.tpCreateReports.Controls.Add(this.gbCompany);
            this.tpCreateReports.Controls.Add(this.cbBankHoursPay);
            this.tpCreateReports.Controls.Add(this.btnGenerateData);
            this.tpCreateReports.Controls.Add(this.btnBrowse);
            this.tpCreateReports.Controls.Add(this.lblPath);
            this.tpCreateReports.Controls.Add(this.tbPath);
            this.tpCreateReports.Controls.Add(this.gbEmployees);
            this.tpCreateReports.Controls.Add(this.gbType);
            this.tpCreateReports.Location = new System.Drawing.Point(4, 22);
            this.tpCreateReports.Name = "tpCreateReports";
            this.tpCreateReports.Padding = new System.Windows.Forms.Padding(3);
            this.tpCreateReports.Size = new System.Drawing.Size(558, 516);
            this.tpCreateReports.TabIndex = 0;
            this.tpCreateReports.Text = "Create reports";
            this.tpCreateReports.UseVisualStyleBackColor = true;
            // 
            // btnSWBrowse
            // 
            this.btnSWBrowse.Location = new System.Drawing.Point(467, 402);
            this.btnSWBrowse.Name = "btnSWBrowse";
            this.btnSWBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnSWBrowse.TabIndex = 14;
            this.btnSWBrowse.Text = "Browse";
            this.btnSWBrowse.UseVisualStyleBackColor = true;
            this.btnSWBrowse.Click += new System.EventHandler(this.btnSWBrowse_Click);
            // 
            // lblSWPath
            // 
            this.lblSWPath.Location = new System.Drawing.Point(132, 407);
            this.lblSWPath.Name = "lblSWPath";
            this.lblSWPath.Size = new System.Drawing.Size(79, 18);
            this.lblSWPath.TabIndex = 12;
            this.lblSWPath.Text = "Path:";
            this.lblSWPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbSWPath
            // 
            this.tbSWPath.Location = new System.Drawing.Point(217, 405);
            this.tbSWPath.Name = "tbSWPath";
            this.tbSWPath.Size = new System.Drawing.Size(244, 20);
            this.tbSWPath.TabIndex = 13;
            // 
            // cbSWPay
            // 
            this.cbSWPay.AutoSize = true;
            this.cbSWPay.Location = new System.Drawing.Point(10, 407);
            this.cbSWPay.Name = "cbSWPay";
            this.cbSWPay.Size = new System.Drawing.Size(108, 17);
            this.cbSWPay.TabIndex = 11;
            this.cbSWPay.Text = "Stop working pay";
            this.cbSWPay.UseVisualStyleBackColor = true;
            this.cbSWPay.CheckedChanged += new System.EventHandler(this.cbSWPay_CheckedChanged);
            // 
            // chbSWPayment
            // 
            this.chbSWPayment.AutoSize = true;
            this.chbSWPayment.Location = new System.Drawing.Point(333, 431);
            this.chbSWPayment.Name = "chbSWPayment";
            this.chbSWPayment.Size = new System.Drawing.Size(67, 17);
            this.chbSWPayment.TabIndex = 18;
            this.chbSWPayment.Text = "Payment";
            this.chbSWPayment.UseVisualStyleBackColor = true;
            // 
            // chbBHPayment
            // 
            this.chbBHPayment.AutoSize = true;
            this.chbBHPayment.Location = new System.Drawing.Point(114, 431);
            this.chbBHPayment.Name = "chbBHPayment";
            this.chbBHPayment.Size = new System.Drawing.Size(67, 17);
            this.chbBHPayment.TabIndex = 16;
            this.chbBHPayment.Text = "Payment";
            this.chbBHPayment.UseVisualStyleBackColor = true;
            // 
            // chbSWPayRegular
            // 
            this.chbSWPayRegular.AutoSize = true;
            this.chbSWPayRegular.Location = new System.Drawing.Point(219, 431);
            this.chbSWPayRegular.Name = "chbSWPayRegular";
            this.chbSWPayRegular.Size = new System.Drawing.Size(99, 17);
            this.chbSWPayRegular.TabIndex = 17;
            this.chbSWPayRegular.Text = "SW pay regular";
            this.chbSWPayRegular.UseVisualStyleBackColor = true;
            this.chbSWPayRegular.CheckedChanged += new System.EventHandler(this.chbSWPayRegular_CheckedChanged);
            // 
            // numSWBalanceMonth
            // 
            this.numSWBalanceMonth.Location = new System.Drawing.Point(219, 449);
            this.numSWBalanceMonth.Name = "numSWBalanceMonth";
            this.numSWBalanceMonth.Size = new System.Drawing.Size(36, 20);
            this.numSWBalanceMonth.TabIndex = 22;
            // 
            // lblSWBalanceMonth
            // 
            this.lblSWBalanceMonth.AutoSize = true;
            this.lblSWBalanceMonth.Location = new System.Drawing.Point(256, 451);
            this.lblSWBalanceMonth.Name = "lblSWBalanceMonth";
            this.lblSWBalanceMonth.Size = new System.Drawing.Size(77, 13);
            this.lblSWBalanceMonth.TabIndex = 23;
            this.lblSWBalanceMonth.Text = "balance month";
            this.lblSWBalanceMonth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numBHBalanceMonth
            // 
            this.numBHBalanceMonth.Location = new System.Drawing.Point(10, 449);
            this.numBHBalanceMonth.Name = "numBHBalanceMonth";
            this.numBHBalanceMonth.Size = new System.Drawing.Size(36, 20);
            this.numBHBalanceMonth.TabIndex = 20;
            // 
            // lblBHBalanceMonth
            // 
            this.lblBHBalanceMonth.AutoSize = true;
            this.lblBHBalanceMonth.Location = new System.Drawing.Point(49, 451);
            this.lblBHBalanceMonth.Name = "lblBHBalanceMonth";
            this.lblBHBalanceMonth.Size = new System.Drawing.Size(77, 13);
            this.lblBHBalanceMonth.TabIndex = 21;
            this.lblBHBalanceMonth.Text = "balance month";
            this.lblBHBalanceMonth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbBHPayRegular
            // 
            this.chbBHPayRegular.AutoSize = true;
            this.chbBHPayRegular.Location = new System.Drawing.Point(10, 431);
            this.chbBHPayRegular.Name = "chbBHPayRegular";
            this.chbBHPayRegular.Size = new System.Drawing.Size(96, 17);
            this.chbBHPayRegular.TabIndex = 15;
            this.chbBHPayRegular.Text = "BH pay regular";
            this.chbBHPayRegular.UseVisualStyleBackColor = true;
            this.chbBHPayRegular.CheckedChanged += new System.EventHandler(this.chbBHPayRegular_CheckedChanged);
            // 
            // btnBHBrowse
            // 
            this.btnBHBrowse.Location = new System.Drawing.Point(467, 376);
            this.btnBHBrowse.Name = "btnBHBrowse";
            this.btnBHBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBHBrowse.TabIndex = 10;
            this.btnBHBrowse.Text = "Browse";
            this.btnBHBrowse.UseVisualStyleBackColor = true;
            this.btnBHBrowse.Click += new System.EventHandler(this.btnBHBrowse_Click);
            // 
            // lblBHPath
            // 
            this.lblBHPath.Location = new System.Drawing.Point(132, 381);
            this.lblBHPath.Name = "lblBHPath";
            this.lblBHPath.Size = new System.Drawing.Size(79, 18);
            this.lblBHPath.TabIndex = 8;
            this.lblBHPath.Text = "Path:";
            this.lblBHPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbBHPath
            // 
            this.tbBHPath.Location = new System.Drawing.Point(217, 379);
            this.tbBHPath.Name = "tbBHPath";
            this.tbBHPath.Size = new System.Drawing.Size(244, 20);
            this.tbBHPath.TabIndex = 9;
            // 
            // chbWorkAnalysisReport
            // 
            this.chbWorkAnalysisReport.AutoSize = true;
            this.chbWorkAnalysisReport.Location = new System.Drawing.Point(219, 479);
            this.chbWorkAnalysisReport.Name = "chbWorkAnalysisReport";
            this.chbWorkAnalysisReport.Size = new System.Drawing.Size(171, 17);
            this.chbWorkAnalysisReport.TabIndex = 25;
            this.chbWorkAnalysisReport.Text = "Generate work analysis reports";
            this.chbWorkAnalysisReport.UseVisualStyleBackColor = true;
            // 
            // chbStopWorkingPay
            // 
            this.chbStopWorkingPay.AutoSize = true;
            this.chbStopWorkingPay.Location = new System.Drawing.Point(406, 479);
            this.chbStopWorkingPay.Name = "chbStopWorkingPay";
            this.chbStopWorkingPay.Size = new System.Drawing.Size(108, 17);
            this.chbStopWorkingPay.TabIndex = 26;
            this.chbStopWorkingPay.Text = "Stop working pay";
            this.chbStopWorkingPay.UseVisualStyleBackColor = true;
            this.chbStopWorkingPay.Visible = false;
            // 
            // chbVacationPay
            // 
            this.chbVacationPay.AutoSize = true;
            this.chbVacationPay.Location = new System.Drawing.Point(406, 431);
            this.chbVacationPay.Name = "chbVacationPay";
            this.chbVacationPay.Size = new System.Drawing.Size(88, 17);
            this.chbVacationPay.TabIndex = 19;
            this.chbVacationPay.Text = "Vacation pay";
            this.chbVacationPay.UseVisualStyleBackColor = true;
            // 
            // gbDate
            // 
            this.gbDate.Controls.Add(this.dtTo);
            this.gbDate.Controls.Add(this.lblTo);
            this.gbDate.Controls.Add(this.lblFrom);
            this.gbDate.Controls.Add(this.dtFrom);
            this.gbDate.Location = new System.Drawing.Point(6, 196);
            this.gbDate.Name = "gbDate";
            this.gbDate.Size = new System.Drawing.Size(216, 78);
            this.gbDate.TabIndex = 2;
            this.gbDate.TabStop = false;
            this.gbDate.Text = "Date interval";
            // 
            // gbCompany
            // 
            this.gbCompany.Controls.Add(this.chbShowLeavingEmployees);
            this.gbCompany.Controls.Add(this.dtpBorderDay);
            this.gbCompany.Controls.Add(this.lblBorderDate);
            this.gbCompany.Controls.Add(this.lvCompany);
            this.gbCompany.Location = new System.Drawing.Point(6, 6);
            this.gbCompany.Name = "gbCompany";
            this.gbCompany.Size = new System.Drawing.Size(216, 184);
            this.gbCompany.TabIndex = 0;
            this.gbCompany.TabStop = false;
            this.gbCompany.Text = "Company";
            // 
            // chbShowLeavingEmployees
            // 
            this.chbShowLeavingEmployees.AutoSize = true;
            this.chbShowLeavingEmployees.Location = new System.Drawing.Point(18, 158);
            this.chbShowLeavingEmployees.Name = "chbShowLeavingEmployees";
            this.chbShowLeavingEmployees.Size = new System.Drawing.Size(137, 17);
            this.chbShowLeavingEmployees.TabIndex = 3;
            this.chbShowLeavingEmployees.Text = "Show leaving emloyees";
            this.chbShowLeavingEmployees.UseVisualStyleBackColor = true;
            this.chbShowLeavingEmployees.CheckedChanged += new System.EventHandler(this.chbShowLeavingEmployees_CheckedChanged);
            // 
            // dtpBorderDay
            // 
            this.dtpBorderDay.CustomFormat = "dd.MM.yyyy";
            this.dtpBorderDay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBorderDay.Location = new System.Drawing.Point(18, 130);
            this.dtpBorderDay.Name = "dtpBorderDay";
            this.dtpBorderDay.Size = new System.Drawing.Size(101, 20);
            this.dtpBorderDay.TabIndex = 2;
            this.dtpBorderDay.ValueChanged += new System.EventHandler(this.dtpBorderDay_ValueChanged);
            // 
            // lblBorderDate
            // 
            this.lblBorderDate.AutoSize = true;
            this.lblBorderDate.Location = new System.Drawing.Point(15, 112);
            this.lblBorderDate.Name = "lblBorderDate";
            this.lblBorderDate.Size = new System.Drawing.Size(65, 13);
            this.lblBorderDate.TabIndex = 1;
            this.lblBorderDate.Text = "Border date:";
            // 
            // lvCompany
            // 
            this.lvCompany.FullRowSelect = true;
            this.lvCompany.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvCompany.HideSelection = false;
            this.lvCompany.Location = new System.Drawing.Point(19, 19);
            this.lvCompany.Name = "lvCompany";
            this.lvCompany.Size = new System.Drawing.Size(180, 89);
            this.lvCompany.TabIndex = 0;
            this.lvCompany.UseCompatibleStateImageBehavior = false;
            this.lvCompany.View = System.Windows.Forms.View.Details;
            this.lvCompany.SelectedIndexChanged += new System.EventHandler(this.lvCompany_SelectedIndexChanged);
            // 
            // cbBankHoursPay
            // 
            this.cbBankHoursPay.AutoSize = true;
            this.cbBankHoursPay.Location = new System.Drawing.Point(10, 381);
            this.cbBankHoursPay.Name = "cbBankHoursPay";
            this.cbBankHoursPay.Size = new System.Drawing.Size(100, 17);
            this.cbBankHoursPay.TabIndex = 7;
            this.cbBankHoursPay.Text = "Bank hours pay";
            this.cbBankHoursPay.UseVisualStyleBackColor = true;
            this.cbBankHoursPay.CheckedChanged += new System.EventHandler(this.cbBankHoursPay_CheckedChanged);
            // 
            // tpRestarCounters
            // 
            this.tpRestarCounters.Controls.Add(this.gbSWPay);
            this.tpRestarCounters.Controls.Add(this.gbStopWorkingCutOffMonth);
            this.tpRestarCounters.Controls.Add(this.gbLeavingEmployees);
            this.tpRestarCounters.Controls.Add(this.gbNotJustifiedOverTimeCounter);
            this.tpRestarCounters.Controls.Add(this.gbPaidLeaves);
            this.tpRestarCounters.Controls.Add(this.gbVacationCutOffMonth);
            this.tpRestarCounters.Controls.Add(this.gbBHCuttOffMonths);
            this.tpRestarCounters.Controls.Add(this.gbRestartOvertimeCounter);
            this.tpRestarCounters.Location = new System.Drawing.Point(4, 22);
            this.tpRestarCounters.Name = "tpRestarCounters";
            this.tpRestarCounters.Padding = new System.Windows.Forms.Padding(3);
            this.tpRestarCounters.Size = new System.Drawing.Size(558, 516);
            this.tpRestarCounters.TabIndex = 1;
            this.tpRestarCounters.Text = "Restart counters";
            this.tpRestarCounters.UseVisualStyleBackColor = true;
            // 
            // gbSWPay
            // 
            this.gbSWPay.Controls.Add(this.btnRecalculateSWPay);
            this.gbSWPay.Controls.Add(this.numSWPayCalcID);
            this.gbSWPay.Controls.Add(this.lblSWPayCalcID);
            this.gbSWPay.Location = new System.Drawing.Point(13, 190);
            this.gbSWPay.Name = "gbSWPay";
            this.gbSWPay.Size = new System.Drawing.Size(450, 50);
            this.gbSWPay.TabIndex = 3;
            this.gbSWPay.TabStop = false;
            this.gbSWPay.Text = "Stop working cut off month";
            // 
            // btnRecalculateSWPay
            // 
            this.btnRecalculateSWPay.Location = new System.Drawing.Point(311, 17);
            this.btnRecalculateSWPay.Name = "btnRecalculateSWPay";
            this.btnRecalculateSWPay.Size = new System.Drawing.Size(93, 23);
            this.btnRecalculateSWPay.TabIndex = 2;
            this.btnRecalculateSWPay.Text = "Recalculate";
            this.btnRecalculateSWPay.UseVisualStyleBackColor = true;
            this.btnRecalculateSWPay.Click += new System.EventHandler(this.btnRecalculateSWPay_Click);
            // 
            // numSWPayCalcID
            // 
            this.numSWPayCalcID.Location = new System.Drawing.Point(119, 19);
            this.numSWPayCalcID.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numSWPayCalcID.Name = "numSWPayCalcID";
            this.numSWPayCalcID.Size = new System.Drawing.Size(103, 20);
            this.numSWPayCalcID.TabIndex = 1;
            // 
            // lblSWPayCalcID
            // 
            this.lblSWPayCalcID.Location = new System.Drawing.Point(16, 17);
            this.lblSWPayCalcID.Name = "lblSWPayCalcID";
            this.lblSWPayCalcID.Size = new System.Drawing.Size(96, 23);
            this.lblSWPayCalcID.TabIndex = 0;
            this.lblSWPayCalcID.Text = "Calc ID:";
            this.lblSWPayCalcID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbStopWorkingCutOffMonth
            // 
            this.gbStopWorkingCutOffMonth.Controls.Add(this.btnRecalculateSW);
            this.gbStopWorkingCutOffMonth.Controls.Add(this.numSWCalcID);
            this.gbStopWorkingCutOffMonth.Controls.Add(this.lblSWCalcID);
            this.gbStopWorkingCutOffMonth.Location = new System.Drawing.Point(13, 430);
            this.gbStopWorkingCutOffMonth.Name = "gbStopWorkingCutOffMonth";
            this.gbStopWorkingCutOffMonth.Size = new System.Drawing.Size(450, 50);
            this.gbStopWorkingCutOffMonth.TabIndex = 7;
            this.gbStopWorkingCutOffMonth.TabStop = false;
            this.gbStopWorkingCutOffMonth.Text = "Stop working cut off month";
            // 
            // btnRecalculateSW
            // 
            this.btnRecalculateSW.Location = new System.Drawing.Point(311, 17);
            this.btnRecalculateSW.Name = "btnRecalculateSW";
            this.btnRecalculateSW.Size = new System.Drawing.Size(93, 23);
            this.btnRecalculateSW.TabIndex = 2;
            this.btnRecalculateSW.Text = "Recalculate";
            this.btnRecalculateSW.UseVisualStyleBackColor = true;
            this.btnRecalculateSW.Click += new System.EventHandler(this.btnRecalculateSW_Click);
            // 
            // numSWCalcID
            // 
            this.numSWCalcID.Location = new System.Drawing.Point(119, 19);
            this.numSWCalcID.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numSWCalcID.Name = "numSWCalcID";
            this.numSWCalcID.Size = new System.Drawing.Size(103, 20);
            this.numSWCalcID.TabIndex = 1;
            // 
            // lblSWCalcID
            // 
            this.lblSWCalcID.Location = new System.Drawing.Point(16, 17);
            this.lblSWCalcID.Name = "lblSWCalcID";
            this.lblSWCalcID.Size = new System.Drawing.Size(96, 23);
            this.lblSWCalcID.TabIndex = 0;
            this.lblSWCalcID.Text = "Calc ID:";
            this.lblSWCalcID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbLeavingEmployees
            // 
            this.gbLeavingEmployees.Controls.Add(this.btnLeavingEmployeesRecalculate);
            this.gbLeavingEmployees.Controls.Add(this.numLeavingEmployeesCalcID);
            this.gbLeavingEmployees.Controls.Add(this.lblLeavingEmployeesCalcID);
            this.gbLeavingEmployees.Location = new System.Drawing.Point(13, 250);
            this.gbLeavingEmployees.Name = "gbLeavingEmployees";
            this.gbLeavingEmployees.Size = new System.Drawing.Size(450, 50);
            this.gbLeavingEmployees.TabIndex = 4;
            this.gbLeavingEmployees.TabStop = false;
            this.gbLeavingEmployees.Text = "Leaving employees counters";
            // 
            // btnLeavingEmployeesRecalculate
            // 
            this.btnLeavingEmployeesRecalculate.Location = new System.Drawing.Point(311, 17);
            this.btnLeavingEmployeesRecalculate.Name = "btnLeavingEmployeesRecalculate";
            this.btnLeavingEmployeesRecalculate.Size = new System.Drawing.Size(93, 23);
            this.btnLeavingEmployeesRecalculate.TabIndex = 2;
            this.btnLeavingEmployeesRecalculate.Text = "Recalculate";
            this.btnLeavingEmployeesRecalculate.UseVisualStyleBackColor = true;
            this.btnLeavingEmployeesRecalculate.Click += new System.EventHandler(this.btnLeavingEmployeesRecalculate_Click);
            // 
            // numLeavingEmployeesCalcID
            // 
            this.numLeavingEmployeesCalcID.Location = new System.Drawing.Point(119, 19);
            this.numLeavingEmployeesCalcID.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numLeavingEmployeesCalcID.Name = "numLeavingEmployeesCalcID";
            this.numLeavingEmployeesCalcID.Size = new System.Drawing.Size(103, 20);
            this.numLeavingEmployeesCalcID.TabIndex = 1;
            // 
            // lblLeavingEmployeesCalcID
            // 
            this.lblLeavingEmployeesCalcID.Location = new System.Drawing.Point(16, 17);
            this.lblLeavingEmployeesCalcID.Name = "lblLeavingEmployeesCalcID";
            this.lblLeavingEmployeesCalcID.Size = new System.Drawing.Size(96, 23);
            this.lblLeavingEmployeesCalcID.TabIndex = 0;
            this.lblLeavingEmployeesCalcID.Text = "Calc ID:";
            this.lblLeavingEmployeesCalcID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbNotJustifiedOverTimeCounter
            // 
            this.gbNotJustifiedOverTimeCounter.Controls.Add(this.btnRecalculateNJOvertime);
            this.gbNotJustifiedOverTimeCounter.Controls.Add(this.numNJOvertime);
            this.gbNotJustifiedOverTimeCounter.Controls.Add(this.lblCalcIDNJOvertime);
            this.gbNotJustifiedOverTimeCounter.Location = new System.Drawing.Point(13, 70);
            this.gbNotJustifiedOverTimeCounter.Name = "gbNotJustifiedOverTimeCounter";
            this.gbNotJustifiedOverTimeCounter.Size = new System.Drawing.Size(450, 50);
            this.gbNotJustifiedOverTimeCounter.TabIndex = 1;
            this.gbNotJustifiedOverTimeCounter.TabStop = false;
            this.gbNotJustifiedOverTimeCounter.Text = "Calculate not justified overtime counter";
            // 
            // btnRecalculateNJOvertime
            // 
            this.btnRecalculateNJOvertime.Location = new System.Drawing.Point(311, 17);
            this.btnRecalculateNJOvertime.Name = "btnRecalculateNJOvertime";
            this.btnRecalculateNJOvertime.Size = new System.Drawing.Size(93, 23);
            this.btnRecalculateNJOvertime.TabIndex = 2;
            this.btnRecalculateNJOvertime.Text = "Recalculate";
            this.btnRecalculateNJOvertime.UseVisualStyleBackColor = true;
            this.btnRecalculateNJOvertime.Click += new System.EventHandler(this.btnRecalculateNJOvertime_Click);
            // 
            // numNJOvertime
            // 
            this.numNJOvertime.Location = new System.Drawing.Point(119, 19);
            this.numNJOvertime.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numNJOvertime.Name = "numNJOvertime";
            this.numNJOvertime.Size = new System.Drawing.Size(103, 20);
            this.numNJOvertime.TabIndex = 1;
            // 
            // lblCalcIDNJOvertime
            // 
            this.lblCalcIDNJOvertime.Location = new System.Drawing.Point(16, 17);
            this.lblCalcIDNJOvertime.Name = "lblCalcIDNJOvertime";
            this.lblCalcIDNJOvertime.Size = new System.Drawing.Size(96, 23);
            this.lblCalcIDNJOvertime.TabIndex = 0;
            this.lblCalcIDNJOvertime.Text = "Calc ID:";
            this.lblCalcIDNJOvertime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbPaidLeaves
            // 
            this.gbPaidLeaves.Controls.Add(this.btnRecalculatePaidLeaves);
            this.gbPaidLeaves.Location = new System.Drawing.Point(13, 370);
            this.gbPaidLeaves.Name = "gbPaidLeaves";
            this.gbPaidLeaves.Size = new System.Drawing.Size(450, 50);
            this.gbPaidLeaves.TabIndex = 6;
            this.gbPaidLeaves.TabStop = false;
            this.gbPaidLeaves.Text = "Paid leaves";
            // 
            // btnRecalculatePaidLeaves
            // 
            this.btnRecalculatePaidLeaves.Location = new System.Drawing.Point(311, 17);
            this.btnRecalculatePaidLeaves.Name = "btnRecalculatePaidLeaves";
            this.btnRecalculatePaidLeaves.Size = new System.Drawing.Size(93, 23);
            this.btnRecalculatePaidLeaves.TabIndex = 0;
            this.btnRecalculatePaidLeaves.Text = "Recalculate";
            this.btnRecalculatePaidLeaves.UseVisualStyleBackColor = true;
            this.btnRecalculatePaidLeaves.Click += new System.EventHandler(this.btnRecalculatePaidLeaves_Click);
            // 
            // gbVacationCutOffMonth
            // 
            this.gbVacationCutOffMonth.Controls.Add(this.btnRecalculateVac);
            this.gbVacationCutOffMonth.Controls.Add(this.numVacCutOffDate);
            this.gbVacationCutOffMonth.Controls.Add(this.lblVacCalcID);
            this.gbVacationCutOffMonth.Location = new System.Drawing.Point(13, 310);
            this.gbVacationCutOffMonth.Name = "gbVacationCutOffMonth";
            this.gbVacationCutOffMonth.Size = new System.Drawing.Size(450, 50);
            this.gbVacationCutOffMonth.TabIndex = 5;
            this.gbVacationCutOffMonth.TabStop = false;
            this.gbVacationCutOffMonth.Text = "Vacation cut off month";
            // 
            // btnRecalculateVac
            // 
            this.btnRecalculateVac.Location = new System.Drawing.Point(311, 18);
            this.btnRecalculateVac.Name = "btnRecalculateVac";
            this.btnRecalculateVac.Size = new System.Drawing.Size(93, 23);
            this.btnRecalculateVac.TabIndex = 2;
            this.btnRecalculateVac.Text = "Recalculate";
            this.btnRecalculateVac.UseVisualStyleBackColor = true;
            this.btnRecalculateVac.Click += new System.EventHandler(this.btnRecalculateVac_Click);
            // 
            // numVacCutOffDate
            // 
            this.numVacCutOffDate.Location = new System.Drawing.Point(119, 20);
            this.numVacCutOffDate.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numVacCutOffDate.Name = "numVacCutOffDate";
            this.numVacCutOffDate.Size = new System.Drawing.Size(103, 20);
            this.numVacCutOffDate.TabIndex = 1;
            // 
            // lblVacCalcID
            // 
            this.lblVacCalcID.Location = new System.Drawing.Point(16, 18);
            this.lblVacCalcID.Name = "lblVacCalcID";
            this.lblVacCalcID.Size = new System.Drawing.Size(96, 23);
            this.lblVacCalcID.TabIndex = 0;
            this.lblVacCalcID.Text = "Calc ID:";
            this.lblVacCalcID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbBHCuttOffMonths
            // 
            this.gbBHCuttOffMonths.Controls.Add(this.btnRecalculateBH);
            this.gbBHCuttOffMonths.Controls.Add(this.nudBHCalcID);
            this.gbBHCuttOffMonths.Controls.Add(this.lblCalcIDBH);
            this.gbBHCuttOffMonths.Location = new System.Drawing.Point(13, 130);
            this.gbBHCuttOffMonths.Name = "gbBHCuttOffMonths";
            this.gbBHCuttOffMonths.Size = new System.Drawing.Size(450, 50);
            this.gbBHCuttOffMonths.TabIndex = 2;
            this.gbBHCuttOffMonths.TabStop = false;
            this.gbBHCuttOffMonths.Text = "Bank hour cut off month";
            // 
            // btnRecalculateBH
            // 
            this.btnRecalculateBH.Location = new System.Drawing.Point(311, 17);
            this.btnRecalculateBH.Name = "btnRecalculateBH";
            this.btnRecalculateBH.Size = new System.Drawing.Size(93, 23);
            this.btnRecalculateBH.TabIndex = 2;
            this.btnRecalculateBH.Text = "Recalculate";
            this.btnRecalculateBH.UseVisualStyleBackColor = true;
            this.btnRecalculateBH.Click += new System.EventHandler(this.btnRecalculateBH_Click);
            // 
            // nudBHCalcID
            // 
            this.nudBHCalcID.Location = new System.Drawing.Point(119, 19);
            this.nudBHCalcID.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudBHCalcID.Name = "nudBHCalcID";
            this.nudBHCalcID.Size = new System.Drawing.Size(103, 20);
            this.nudBHCalcID.TabIndex = 1;
            // 
            // lblCalcIDBH
            // 
            this.lblCalcIDBH.Location = new System.Drawing.Point(16, 17);
            this.lblCalcIDBH.Name = "lblCalcIDBH";
            this.lblCalcIDBH.Size = new System.Drawing.Size(96, 23);
            this.lblCalcIDBH.TabIndex = 0;
            this.lblCalcIDBH.Text = "Calc ID:";
            this.lblCalcIDBH.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbRestartOvertimeCounter
            // 
            this.gbRestartOvertimeCounter.Controls.Add(this.btnRecalculate);
            this.gbRestartOvertimeCounter.Controls.Add(this.nudCalcID);
            this.gbRestartOvertimeCounter.Controls.Add(this.lblCalcID);
            this.gbRestartOvertimeCounter.Location = new System.Drawing.Point(13, 10);
            this.gbRestartOvertimeCounter.Name = "gbRestartOvertimeCounter";
            this.gbRestartOvertimeCounter.Size = new System.Drawing.Size(450, 50);
            this.gbRestartOvertimeCounter.TabIndex = 0;
            this.gbRestartOvertimeCounter.TabStop = false;
            this.gbRestartOvertimeCounter.Text = "Restart overtime counter";
            // 
            // btnRecalculate
            // 
            this.btnRecalculate.Location = new System.Drawing.Point(311, 18);
            this.btnRecalculate.Name = "btnRecalculate";
            this.btnRecalculate.Size = new System.Drawing.Size(93, 23);
            this.btnRecalculate.TabIndex = 2;
            this.btnRecalculate.Text = "Recalculate";
            this.btnRecalculate.UseVisualStyleBackColor = true;
            this.btnRecalculate.Click += new System.EventHandler(this.btnRecalculate_Click);
            // 
            // nudCalcID
            // 
            this.nudCalcID.Location = new System.Drawing.Point(119, 20);
            this.nudCalcID.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudCalcID.Name = "nudCalcID";
            this.nudCalcID.Size = new System.Drawing.Size(103, 20);
            this.nudCalcID.TabIndex = 1;
            // 
            // lblCalcID
            // 
            this.lblCalcID.Location = new System.Drawing.Point(16, 18);
            this.lblCalcID.Name = "lblCalcID";
            this.lblCalcID.Size = new System.Drawing.Size(96, 23);
            this.lblCalcID.TabIndex = 0;
            this.lblCalcID.Text = "Calc ID:";
            this.lblCalcID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabAdditionalReports
            // 
            this.tabAdditionalReports.Controls.Add(this.gbAbsence);
            this.tabAdditionalReports.Location = new System.Drawing.Point(4, 22);
            this.tabAdditionalReports.Name = "tabAdditionalReports";
            this.tabAdditionalReports.Size = new System.Drawing.Size(558, 516);
            this.tabAdditionalReports.TabIndex = 2;
            this.tabAdditionalReports.Text = "Additional reports";
            this.tabAdditionalReports.UseVisualStyleBackColor = true;
            // 
            // gbAbsence
            // 
            this.gbAbsence.Controls.Add(this.cbWU);
            this.gbAbsence.Controls.Add(this.btnGenerateAbsent);
            this.gbAbsence.Controls.Add(this.lblCalcIDAbsent);
            this.gbAbsence.Controls.Add(this.lblWUAbsent);
            this.gbAbsence.Controls.Add(this.numAbsentCalcID);
            this.gbAbsence.Controls.Add(this.lvPTAbsent);
            this.gbAbsence.Controls.Add(this.lblPTAbsent);
            this.gbAbsence.Location = new System.Drawing.Point(11, 9);
            this.gbAbsence.Name = "gbAbsence";
            this.gbAbsence.Size = new System.Drawing.Size(537, 214);
            this.gbAbsence.TabIndex = 7;
            this.gbAbsence.TabStop = false;
            this.gbAbsence.Text = "Absence";
            // 
            // cbWU
            // 
            this.cbWU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbWU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(12, 40);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(221, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // btnGenerateAbsent
            // 
            this.btnGenerateAbsent.Location = new System.Drawing.Point(276, 170);
            this.btnGenerateAbsent.Name = "btnGenerateAbsent";
            this.btnGenerateAbsent.Size = new System.Drawing.Size(158, 23);
            this.btnGenerateAbsent.TabIndex = 6;
            this.btnGenerateAbsent.Text = "Generate";
            this.btnGenerateAbsent.UseVisualStyleBackColor = true;
            this.btnGenerateAbsent.Click += new System.EventHandler(this.btnGenerateAbsent_Click);
            // 
            // lblCalcIDAbsent
            // 
            this.lblCalcIDAbsent.AutoSize = true;
            this.lblCalcIDAbsent.Location = new System.Drawing.Point(9, 110);
            this.lblCalcIDAbsent.Name = "lblCalcIDAbsent";
            this.lblCalcIDAbsent.Size = new System.Drawing.Size(45, 13);
            this.lblCalcIDAbsent.TabIndex = 4;
            this.lblCalcIDAbsent.Text = "Calc ID:";
            // 
            // lblWUAbsent
            // 
            this.lblWUAbsent.AutoSize = true;
            this.lblWUAbsent.Location = new System.Drawing.Point(9, 24);
            this.lblWUAbsent.Name = "lblWUAbsent";
            this.lblWUAbsent.Size = new System.Drawing.Size(70, 13);
            this.lblWUAbsent.TabIndex = 0;
            this.lblWUAbsent.Text = "Working unit:";
            // 
            // numAbsentCalcID
            // 
            this.numAbsentCalcID.Location = new System.Drawing.Point(12, 126);
            this.numAbsentCalcID.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numAbsentCalcID.Name = "numAbsentCalcID";
            this.numAbsentCalcID.Size = new System.Drawing.Size(105, 20);
            this.numAbsentCalcID.TabIndex = 5;
            this.numAbsentCalcID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lvPTAbsent
            // 
            this.lvPTAbsent.FullRowSelect = true;
            this.lvPTAbsent.GridLines = true;
            this.lvPTAbsent.HideSelection = false;
            this.lvPTAbsent.Location = new System.Drawing.Point(276, 40);
            this.lvPTAbsent.Name = "lvPTAbsent";
            this.lvPTAbsent.Size = new System.Drawing.Size(252, 106);
            this.lvPTAbsent.TabIndex = 3;
            this.lvPTAbsent.UseCompatibleStateImageBehavior = false;
            this.lvPTAbsent.View = System.Windows.Forms.View.Details;
            // 
            // lblPTAbsent
            // 
            this.lblPTAbsent.AutoSize = true;
            this.lblPTAbsent.Location = new System.Drawing.Point(273, 24);
            this.lblPTAbsent.Name = "lblPTAbsent";
            this.lblPTAbsent.Size = new System.Drawing.Size(61, 13);
            this.lblPTAbsent.TabIndex = 2;
            this.lblPTAbsent.Text = "Pass types:";
            // 
            // PYIntegration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 586);
            this.ControlBox = false;
            this.Controls.Add(this.tc);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Name = "PYIntegration";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PYIntegration";
            this.Load += new System.EventHandler(this.PYIntegration_Load);
            this.gbType.ResumeLayout(false);
            this.gbType.PerformLayout();
            this.gbEmployees.ResumeLayout(false);
            this.gbEmployees.PerformLayout();
            this.tc.ResumeLayout(false);
            this.tpCreateReports.ResumeLayout(false);
            this.tpCreateReports.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSWBalanceMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBHBalanceMonth)).EndInit();
            this.gbDate.ResumeLayout(false);
            this.gbCompany.ResumeLayout(false);
            this.gbCompany.PerformLayout();
            this.tpRestarCounters.ResumeLayout(false);
            this.gbSWPay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numSWPayCalcID)).EndInit();
            this.gbStopWorkingCutOffMonth.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numSWCalcID)).EndInit();
            this.gbLeavingEmployees.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numLeavingEmployeesCalcID)).EndInit();
            this.gbNotJustifiedOverTimeCounter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numNJOvertime)).EndInit();
            this.gbPaidLeaves.ResumeLayout(false);
            this.gbVacationCutOffMonth.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numVacCutOffDate)).EndInit();
            this.gbBHCuttOffMonths.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudBHCalcID)).EndInit();
            this.gbRestartOvertimeCounter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudCalcID)).EndInit();
            this.tabAdditionalReports.ResumeLayout(false);
            this.gbAbsence.ResumeLayout(false);
            this.gbAbsence.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAbsentCalcID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.GroupBox gbType;
        private System.Windows.Forms.RadioButton rbReal;
        private System.Windows.Forms.RadioButton rbEstimated;
        private System.Windows.Forms.GroupBox gbEmployees;
        private System.Windows.Forms.RadioButton rbSelected;
        private System.Windows.Forms.RadioButton rbAllEmployees;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.TextBox tbEmployee;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnGenerateData;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tc;
        private System.Windows.Forms.TabPage tpCreateReports;
        private System.Windows.Forms.TabPage tpRestarCounters;
        private System.Windows.Forms.GroupBox gbRestartOvertimeCounter;
        private System.Windows.Forms.Button btnRecalculate;
        private System.Windows.Forms.NumericUpDown nudCalcID;
        private System.Windows.Forms.Label lblCalcID;
        private System.Windows.Forms.CheckBox cbBankHoursPay;
        private System.Windows.Forms.GroupBox gbBHCuttOffMonths;
        private System.Windows.Forms.Button btnRecalculateBH;
        private System.Windows.Forms.NumericUpDown nudBHCalcID;
        private System.Windows.Forms.Label lblCalcIDBH;
        private System.Windows.Forms.GroupBox gbCompany;
        private System.Windows.Forms.ListView lvCompany;
        private System.Windows.Forms.GroupBox gbDate;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.CheckBox chbVacationPay;
        private System.Windows.Forms.GroupBox gbVacationCutOffMonth;
        private System.Windows.Forms.Button btnRecalculateVac;
        private System.Windows.Forms.NumericUpDown numVacCutOffDate;
        private System.Windows.Forms.Label lblVacCalcID;
        private System.Windows.Forms.GroupBox gbPaidLeaves;
        private System.Windows.Forms.Button btnRecalculatePaidLeaves;
        private System.Windows.Forms.CheckBox chbStopWorkingPay;
        private System.Windows.Forms.GroupBox gbStopWorkingCutOffMonth;
        private System.Windows.Forms.Button btnRecalculateSW;
        private System.Windows.Forms.NumericUpDown numSWCalcID;
        private System.Windows.Forms.Label lblSWCalcID;
        private System.Windows.Forms.GroupBox gbNotJustifiedOverTimeCounter;
        private System.Windows.Forms.Button btnRecalculateNJOvertime;
        private System.Windows.Forms.NumericUpDown numNJOvertime;
        private System.Windows.Forms.Label lblCalcIDNJOvertime;
        private System.Windows.Forms.CheckBox chbShowLeavingEmployees;
        private System.Windows.Forms.DateTimePicker dtpBorderDay;
        private System.Windows.Forms.Label lblBorderDate;
        private System.Windows.Forms.GroupBox gbLeavingEmployees;
        private System.Windows.Forms.Button btnLeavingEmployeesRecalculate;
        private System.Windows.Forms.NumericUpDown numLeavingEmployeesCalcID;
        private System.Windows.Forms.Label lblLeavingEmployeesCalcID;
        private System.Windows.Forms.CheckBox chbWorkAnalysisReport;
        private System.Windows.Forms.Button btnBHBrowse;
        private System.Windows.Forms.Label lblBHPath;
        private System.Windows.Forms.TextBox tbBHPath;
        private System.Windows.Forms.CheckBox chbBHPayRegular;
        private System.Windows.Forms.TabPage tabAdditionalReports;
        private System.Windows.Forms.Label lblCalcIDAbsent;
        private System.Windows.Forms.ListView lvPTAbsent;
        private System.Windows.Forms.NumericUpDown numAbsentCalcID;
        private System.Windows.Forms.Button btnGenerateAbsent;
        private System.Windows.Forms.Label lblPTAbsent;
        private System.Windows.Forms.Label lblWUAbsent;
        private System.Windows.Forms.GroupBox gbAbsence;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.NumericUpDown numBHBalanceMonth;
        private System.Windows.Forms.Label lblBHBalanceMonth;
        private System.Windows.Forms.NumericUpDown numSWBalanceMonth;
        private System.Windows.Forms.Label lblSWBalanceMonth;
        private System.Windows.Forms.CheckBox chbSWPayRegular;
        private System.Windows.Forms.CheckBox chbBHPayment;
        private System.Windows.Forms.CheckBox chbSWPayment;
        private System.Windows.Forms.GroupBox gbSWPay;
        private System.Windows.Forms.Button btnRecalculateSWPay;
        private System.Windows.Forms.NumericUpDown numSWPayCalcID;
        private System.Windows.Forms.Label lblSWPayCalcID;
        private System.Windows.Forms.Button btnSWBrowse;
        private System.Windows.Forms.Label lblSWPath;
        private System.Windows.Forms.TextBox tbSWPath;
        private System.Windows.Forms.CheckBox cbSWPay;
    }
}