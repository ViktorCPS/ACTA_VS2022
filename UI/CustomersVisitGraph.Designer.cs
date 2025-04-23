namespace UI
{
    partial class CustomersVisitGraph
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomersVisitGraph));
            this.btnShow = new System.Windows.Forms.Button();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.lblGraphType = new System.Windows.Forms.Label();
            this.cbGraphType = new System.Windows.Forms.ComboBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.gbTimeInterval = new System.Windows.Forms.GroupBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.gbMonthTimeInterval = new System.Windows.Forms.GroupBox();
            this.dtpMonthTo = new System.Windows.Forms.DateTimePicker();
            this.lblMonthFrom = new System.Windows.Forms.Label();
            this.lblMonthTo = new System.Windows.Forms.Label();
            this.dtpMonthFrom = new System.Windows.Forms.DateTimePicker();
            this.gbDayTimeInterval = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblHrs1 = new System.Windows.Forms.Label();
            this.lblHrs = new System.Windows.Forms.Label();
            this.periodNum = new System.Windows.Forms.NumericUpDown();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.lblTimeFrom = new System.Windows.Forms.Label();
            this.dtpTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTimeTo = new System.Windows.Forms.DateTimePicker();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTimeTo = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.graphPanel = new System.Windows.Forms.Panel();
            this.lblGraphName = new System.Windows.Forms.Label();
            this.btnLocationTree = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numInHour = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpHoursFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpHoursTo = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.pbClientLogo = new System.Windows.Forms.PictureBox();
            this.btnSwitch = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveImageAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbTimeInterval.SuspendLayout();
            this.gbMonthTimeInterval.SuspendLayout();
            this.gbDayTimeInterval.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.periodNum)).BeginInit();
            this.graphPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClientLogo)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(523, 680);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 22;
            this.btnShow.Text = "Show";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(12, 602);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(71, 23);
            this.lblLocation.TabIndex = 23;
            this.lblLocation.Text = "Location:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbLocation
            // 
            this.cbLocation.Location = new System.Drawing.Point(89, 604);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(140, 21);
            this.cbLocation.TabIndex = 24;
            // 
            // lblGraphType
            // 
            this.lblGraphType.Location = new System.Drawing.Point(6, 680);
            this.lblGraphType.Name = "lblGraphType";
            this.lblGraphType.Size = new System.Drawing.Size(77, 23);
            this.lblGraphType.TabIndex = 25;
            this.lblGraphType.Text = "Graph type:";
            this.lblGraphType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbGraphType
            // 
            this.cbGraphType.Location = new System.Drawing.Point(89, 682);
            this.cbGraphType.Name = "cbGraphType";
            this.cbGraphType.Size = new System.Drawing.Size(140, 21);
            this.cbGraphType.TabIndex = 26;
            this.cbGraphType.SelectedIndexChanged += new System.EventHandler(this.cbGraphType_SelectedIndexChanged);
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(6, 22);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(64, 23);
            this.lblFrom.TabIndex = 27;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(10, 48);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(61, 23);
            this.lblTo.TabIndex = 28;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(77, 22);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(126, 20);
            this.dtpFrom.TabIndex = 29;
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.dtpTo);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.dtpFrom);
            this.gbTimeInterval.Location = new System.Drawing.Point(275, 599);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(242, 104);
            this.gbTimeInterval.TabIndex = 30;
            this.gbTimeInterval.TabStop = false;
            this.gbTimeInterval.Text = "Time interval";
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(77, 50);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(126, 20);
            this.dtpTo.TabIndex = 30;
            // 
            // gbMonthTimeInterval
            // 
            this.gbMonthTimeInterval.Controls.Add(this.dtpMonthTo);
            this.gbMonthTimeInterval.Controls.Add(this.lblMonthFrom);
            this.gbMonthTimeInterval.Controls.Add(this.lblMonthTo);
            this.gbMonthTimeInterval.Controls.Add(this.dtpMonthFrom);
            this.gbMonthTimeInterval.Location = new System.Drawing.Point(275, 599);
            this.gbMonthTimeInterval.Name = "gbMonthTimeInterval";
            this.gbMonthTimeInterval.Size = new System.Drawing.Size(242, 104);
            this.gbMonthTimeInterval.TabIndex = 31;
            this.gbMonthTimeInterval.TabStop = false;
            this.gbMonthTimeInterval.Text = "Time interval";
            // 
            // dtpMonthTo
            // 
            this.dtpMonthTo.CustomFormat = "MMM,yyyy";
            this.dtpMonthTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMonthTo.Location = new System.Drawing.Point(77, 50);
            this.dtpMonthTo.Name = "dtpMonthTo";
            this.dtpMonthTo.Size = new System.Drawing.Size(126, 20);
            this.dtpMonthTo.TabIndex = 30;
            // 
            // lblMonthFrom
            // 
            this.lblMonthFrom.Location = new System.Drawing.Point(7, 22);
            this.lblMonthFrom.Name = "lblMonthFrom";
            this.lblMonthFrom.Size = new System.Drawing.Size(64, 23);
            this.lblMonthFrom.TabIndex = 27;
            this.lblMonthFrom.Text = "From:";
            this.lblMonthFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMonthTo
            // 
            this.lblMonthTo.Location = new System.Drawing.Point(10, 49);
            this.lblMonthTo.Name = "lblMonthTo";
            this.lblMonthTo.Size = new System.Drawing.Size(61, 23);
            this.lblMonthTo.TabIndex = 28;
            this.lblMonthTo.Text = "To:";
            this.lblMonthTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpMonthFrom
            // 
            this.dtpMonthFrom.CustomFormat = "MMM,yyyy";
            this.dtpMonthFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMonthFrom.Location = new System.Drawing.Point(77, 23);
            this.dtpMonthFrom.Name = "dtpMonthFrom";
            this.dtpMonthFrom.Size = new System.Drawing.Size(126, 20);
            this.dtpMonthFrom.TabIndex = 29;
            // 
            // gbDayTimeInterval
            // 
            this.gbDayTimeInterval.Controls.Add(this.label2);
            this.gbDayTimeInterval.Controls.Add(this.lblHrs1);
            this.gbDayTimeInterval.Controls.Add(this.lblHrs);
            this.gbDayTimeInterval.Controls.Add(this.periodNum);
            this.gbDayTimeInterval.Controls.Add(this.lblPeriod);
            this.gbDayTimeInterval.Controls.Add(this.lblTimeFrom);
            this.gbDayTimeInterval.Controls.Add(this.dtpTimeFrom);
            this.gbDayTimeInterval.Controls.Add(this.dtpTimeTo);
            this.gbDayTimeInterval.Controls.Add(this.lblDate);
            this.gbDayTimeInterval.Controls.Add(this.lblTimeTo);
            this.gbDayTimeInterval.Controls.Add(this.dtpDate);
            this.gbDayTimeInterval.Location = new System.Drawing.Point(275, 599);
            this.gbDayTimeInterval.Name = "gbDayTimeInterval";
            this.gbDayTimeInterval.Size = new System.Drawing.Size(242, 104);
            this.gbDayTimeInterval.TabIndex = 32;
            this.gbDayTimeInterval.TabStop = false;
            this.gbDayTimeInterval.Text = "Time interval";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(124, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 23);
            this.label2.TabIndex = 37;
            this.label2.Text = "-";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHrs1
            // 
            this.lblHrs1.Location = new System.Drawing.Point(202, 45);
            this.lblHrs1.Name = "lblHrs1";
            this.lblHrs1.Size = new System.Drawing.Size(28, 23);
            this.lblHrs1.TabIndex = 36;
            this.lblHrs1.Text = "h";
            this.lblHrs1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHrs
            // 
            this.lblHrs.Location = new System.Drawing.Point(118, 71);
            this.lblHrs.Name = "lblHrs";
            this.lblHrs.Size = new System.Drawing.Size(41, 23);
            this.lblHrs.TabIndex = 35;
            this.lblHrs.Text = "h";
            this.lblHrs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // periodNum
            // 
            this.periodNum.Location = new System.Drawing.Point(77, 71);
            this.periodNum.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.periodNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.periodNum.Name = "periodNum";
            this.periodNum.Size = new System.Drawing.Size(41, 20);
            this.periodNum.TabIndex = 34;
            this.periodNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblPeriod
            // 
            this.lblPeriod.Location = new System.Drawing.Point(7, 68);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(64, 23);
            this.lblPeriod.TabIndex = 33;
            this.lblPeriod.Text = "Period:";
            this.lblPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTimeFrom
            // 
            this.lblTimeFrom.Location = new System.Drawing.Point(7, 44);
            this.lblTimeFrom.Name = "lblTimeFrom";
            this.lblTimeFrom.Size = new System.Drawing.Size(64, 23);
            this.lblTimeFrom.TabIndex = 32;
            this.lblTimeFrom.Text = "From:";
            this.lblTimeFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTimeFrom
            // 
            this.dtpTimeFrom.CustomFormat = "HH";
            this.dtpTimeFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeFrom.Location = new System.Drawing.Point(77, 45);
            this.dtpTimeFrom.Name = "dtpTimeFrom";
            this.dtpTimeFrom.ShowUpDown = true;
            this.dtpTimeFrom.Size = new System.Drawing.Size(41, 20);
            this.dtpTimeFrom.TabIndex = 31;
            // 
            // dtpTimeTo
            // 
            this.dtpTimeTo.CustomFormat = "HH";
            this.dtpTimeTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeTo.Location = new System.Drawing.Point(161, 45);
            this.dtpTimeTo.Name = "dtpTimeTo";
            this.dtpTimeTo.ShowUpDown = true;
            this.dtpTimeTo.Size = new System.Drawing.Size(42, 20);
            this.dtpTimeTo.TabIndex = 30;
            // 
            // lblDate
            // 
            this.lblDate.Location = new System.Drawing.Point(10, 18);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(61, 23);
            this.lblDate.TabIndex = 27;
            this.lblDate.Text = "Date:";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTimeTo
            // 
            this.lblTimeTo.Location = new System.Drawing.Point(88, 44);
            this.lblTimeTo.Name = "lblTimeTo";
            this.lblTimeTo.Size = new System.Drawing.Size(67, 23);
            this.lblTimeTo.TabIndex = 28;
            this.lblTimeTo.Text = "To:";
            this.lblTimeTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "dd.MM.yyyy";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(77, 19);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(126, 20);
            this.dtpDate.TabIndex = 29;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            // 
            // graphPanel
            // 
            this.graphPanel.AutoScroll = true;
            this.graphPanel.Controls.Add(this.lblGraphName);
            this.graphPanel.Location = new System.Drawing.Point(12, 12);
            this.graphPanel.Name = "graphPanel";
            this.graphPanel.Size = new System.Drawing.Size(963, 567);
            this.graphPanel.TabIndex = 31;
            // 
            // lblGraphName
            // 
            this.lblGraphName.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblGraphName.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGraphName.Location = new System.Drawing.Point(0, 9);
            this.lblGraphName.Name = "lblGraphName";
            this.lblGraphName.Size = new System.Drawing.Size(960, 43);
            this.lblGraphName.TabIndex = 0;
            this.lblGraphName.Text = "text";
            this.lblGraphName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnLocationTree
            // 
            this.btnLocationTree.Image = ((System.Drawing.Image)(resources.GetObject("btnLocationTree.Image")));
            this.btnLocationTree.Location = new System.Drawing.Point(235, 603);
            this.btnLocationTree.Name = "btnLocationTree";
            this.btnLocationTree.Size = new System.Drawing.Size(25, 24);
            this.btnLocationTree.TabIndex = 23;
            this.btnLocationTree.Click += new System.EventHandler(this.btnLocationTree_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(903, 592);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 33;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.dtpDateTo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numInHour);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.dtpHoursFrom);
            this.groupBox1.Controls.Add(this.dtpHoursTo);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.dtpDateFrom);
            this.groupBox1.Location = new System.Drawing.Point(603, 585);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(242, 133);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Time interval";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(10, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 23);
            this.label9.TabIndex = 38;
            this.label9.Text = "Date to:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.CustomFormat = "dd.MM.yyyy";
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTo.Location = new System.Drawing.Point(77, 43);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(126, 20);
            this.dtpDateTo.TabIndex = 39;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(123, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 23);
            this.label1.TabIndex = 37;
            this.label1.Text = "-";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(208, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 23);
            this.label3.TabIndex = 36;
            this.label3.Text = "Hrs";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numInHour
            // 
            this.numInHour.Location = new System.Drawing.Point(123, 95);
            this.numInHour.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numInHour.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInHour.Name = "numInHour";
            this.numInHour.Size = new System.Drawing.Size(58, 20);
            this.numInHour.TabIndex = 34;
            this.numInHour.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 23);
            this.label5.TabIndex = 33;
            this.label5.Text = "Passes in hour:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 23);
            this.label6.TabIndex = 32;
            this.label6.Text = "From:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpHoursFrom
            // 
            this.dtpHoursFrom.CustomFormat = "HH";
            this.dtpHoursFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHoursFrom.Location = new System.Drawing.Point(76, 69);
            this.dtpHoursFrom.Name = "dtpHoursFrom";
            this.dtpHoursFrom.ShowUpDown = true;
            this.dtpHoursFrom.Size = new System.Drawing.Size(41, 20);
            this.dtpHoursFrom.TabIndex = 31;
            // 
            // dtpHoursTo
            // 
            this.dtpHoursTo.CustomFormat = "HH";
            this.dtpHoursTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHoursTo.Location = new System.Drawing.Point(161, 68);
            this.dtpHoursTo.Name = "dtpHoursTo";
            this.dtpHoursTo.ShowUpDown = true;
            this.dtpHoursTo.Size = new System.Drawing.Size(42, 20);
            this.dtpHoursTo.TabIndex = 30;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(10, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 23);
            this.label7.TabIndex = 27;
            this.label7.Text = "Date from:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(88, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 23);
            this.label8.TabIndex = 28;
            this.label8.Text = "To:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpDateFrom
            // 
            this.dtpDateFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateFrom.Location = new System.Drawing.Point(77, 18);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(126, 20);
            this.dtpDateFrom.TabIndex = 29;
            // 
            // pbClientLogo
            // 
            this.pbClientLogo.Location = new System.Drawing.Point(872, 673);
            this.pbClientLogo.Name = "pbClientLogo";
            this.pbClientLogo.Size = new System.Drawing.Size(106, 30);
            this.pbClientLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbClientLogo.TabIndex = 35;
            this.pbClientLogo.TabStop = false;
            this.pbClientLogo.DoubleClick += new System.EventHandler(this.pbClientLogo_DoubleClick);
            // 
            // btnSwitch
            // 
            this.btnSwitch.Location = new System.Drawing.Point(522, 602);
            this.btnSwitch.Name = "btnSwitch";
            this.btnSwitch.Size = new System.Drawing.Size(75, 23);
            this.btnSwitch.TabIndex = 37;
            this.btnSwitch.Text = "Switch";
            this.btnSwitch.Click += new System.EventHandler(this.btnSwitch_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.saveImageAsToolStripMenuItem,
            this.printSetupToolStripMenuItem,
            this.printToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(167, 92);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // saveImageAsToolStripMenuItem
            // 
            this.saveImageAsToolStripMenuItem.Name = "saveImageAsToolStripMenuItem";
            this.saveImageAsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.saveImageAsToolStripMenuItem.Text = "Save image as...";
            this.saveImageAsToolStripMenuItem.Click += new System.EventHandler(this.saveImageAsToolStripMenuItem_Click);
            // 
            // printSetupToolStripMenuItem
            // 
            this.printSetupToolStripMenuItem.Name = "printSetupToolStripMenuItem";
            this.printSetupToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.printSetupToolStripMenuItem.Text = "Page setup...";
            this.printSetupToolStripMenuItem.Click += new System.EventHandler(this.printSetupToolStripMenuItem_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.printToolStripMenuItem.Text = "Print...";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // CustomersVisitGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 715);
            this.Controls.Add(this.btnSwitch);
            this.Controls.Add(this.gbTimeInterval);
            this.Controls.Add(this.pbClientLogo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbMonthTimeInterval);
            this.Controls.Add(this.gbDayTimeInterval);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnLocationTree);
            this.Controls.Add(this.graphPanel);
            this.Controls.Add(this.cbGraphType);
            this.Controls.Add(this.lblGraphType);
            this.Controls.Add(this.cbLocation);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.btnShow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomersVisitGraph";
            this.ShowInTaskbar = false;
            this.Text = "Costumers attendance graph";
            this.Load += new System.EventHandler(this.CustomersAttendanceGraph_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CustomersVisitGraph_FormClosed);
            this.gbTimeInterval.ResumeLayout(false);
            this.gbMonthTimeInterval.ResumeLayout(false);
            this.gbDayTimeInterval.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.periodNum)).EndInit();
            this.graphPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numInHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClientLogo)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.ComboBox cbLocation;
        private System.Windows.Forms.Label lblGraphType;
        private System.Windows.Forms.ComboBox cbGraphType;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.GroupBox gbTimeInterval;
        private System.Windows.Forms.DateTimePicker dtpMonthTo;
        private System.Windows.Forms.Panel graphPanel;
        private System.Windows.Forms.GroupBox gbDayTimeInterval;
        private System.Windows.Forms.DateTimePicker dtpTimeTo;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTimeTo;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblTimeFrom;
        private System.Windows.Forms.DateTimePicker dtpTimeFrom;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.NumericUpDown periodNum;
        private System.Windows.Forms.Label lblHrs;
        private System.Windows.Forms.Button btnLocationTree;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblHrs1;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtpDateTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numInHour;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpHoursFrom;
        private System.Windows.Forms.DateTimePicker dtpHoursTo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtpDateFrom;
        private System.Windows.Forms.GroupBox gbMonthTimeInterval;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblMonthFrom;
        private System.Windows.Forms.Label lblMonthTo;
        private System.Windows.Forms.DateTimePicker dtpMonthFrom;
        private System.Windows.Forms.PictureBox pbClientLogo;
        private System.Windows.Forms.Button btnSwitch;
        private System.Windows.Forms.Label lblGraphName;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveImageAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printSetupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
    }
}