namespace UI
{
    partial class AbsenceMassiveInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AbsenceMassiveInput));
            this.gbEmplFilter = new System.Windows.Forms.GroupBox();
            this.rbWSDay = new System.Windows.Forms.RadioButton();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.rbFile = new System.Windows.Forms.RadioButton();
            this.gbWS = new System.Windows.Forms.GroupBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dtpDay = new System.Windows.Forms.DateTimePicker();
            this.lblDay = new System.Windows.Forms.Label();
            this.lblCycleDay = new System.Windows.Forms.Label();
            this.lblWS = new System.Windows.Forms.Label();
            this.cbCycleDay = new System.Windows.Forms.ComboBox();
            this.cbWS = new System.Windows.Forms.ComboBox();
            this.cbWSGroup = new System.Windows.Forms.ComboBox();
            this.rbWSGroup = new System.Windows.Forms.RadioButton();
            this.chbHierarhiclyWU = new System.Windows.Forms.CheckBox();
            this.btnOUTree = new System.Windows.Forms.Button();
            this.cbOU = new System.Windows.Forms.ComboBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.chbHierachyOU = new System.Windows.Forms.CheckBox();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.rbOU = new System.Windows.Forms.RadioButton();
            this.rbWU = new System.Windows.Forms.RadioButton();
            this.gbEmployees = new System.Windows.Forms.GroupBox();
            this.lblEmplInfo = new System.Windows.Forms.Label();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.tbEmployee = new System.Windows.Forms.TextBox();
            this.gbAction = new System.Windows.Forms.GroupBox();
            this.chbIncludeAdditional = new System.Windows.Forms.CheckBox();
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.chbPublicHoliday = new System.Windows.Forms.CheckBox();
            this.chbStoppage = new System.Windows.Forms.CheckBox();
            this.chbLayOff = new System.Windows.Forms.CheckBox();
            this.chbClosure = new System.Windows.Forms.CheckBox();
            this.numHours = new System.Windows.Forms.NumericUpDown();
            this.chbHours = new System.Windows.Forms.CheckBox();
            this.lblPassTypeNew = new System.Windows.Forms.Label();
            this.cbPassTypeNew = new System.Windows.Forms.ComboBox();
            this.rbCheckLimit = new System.Windows.Forms.RadioButton();
            this.rbDoNotCheckLimit = new System.Windows.Forms.RadioButton();
            this.lblPassType = new System.Windows.Forms.Label();
            this.cbPassType = new System.Windows.Forms.ComboBox();
            this.cbAction = new System.Windows.Forms.ComboBox();
            this.lblAction = new System.Windows.Forms.Label();
            this.gbPeriod = new System.Windows.Forms.GroupBox();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.btnGeneratePreview = new System.Windows.Forms.Button();
            this.btnExportPreview = new System.Windows.Forms.Button();
            this.gbPreview = new System.Windows.Forms.GroupBox();
            this.lblPreviewInfo = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lvPreview = new System.Windows.Forms.ListView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tbLogPath = new System.Windows.Forms.TextBox();
            this.btnLogBrowse = new System.Windows.Forms.Button();
            this.lblLogPath = new System.Windows.Forms.Label();
            this.gbEmplFilter.SuspendLayout();
            this.gbWS.SuspendLayout();
            this.gbEmployees.SuspendLayout();
            this.gbAction.SuspendLayout();
            this.gbSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHours)).BeginInit();
            this.gbPeriod.SuspendLayout();
            this.gbPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbEmplFilter
            // 
            this.gbEmplFilter.Controls.Add(this.rbWSDay);
            this.gbEmplFilter.Controls.Add(this.btnBrowse);
            this.gbEmplFilter.Controls.Add(this.tbPath);
            this.gbEmplFilter.Controls.Add(this.rbFile);
            this.gbEmplFilter.Controls.Add(this.gbWS);
            this.gbEmplFilter.Controls.Add(this.cbWSGroup);
            this.gbEmplFilter.Controls.Add(this.rbWSGroup);
            this.gbEmplFilter.Controls.Add(this.chbHierarhiclyWU);
            this.gbEmplFilter.Controls.Add(this.btnOUTree);
            this.gbEmplFilter.Controls.Add(this.cbOU);
            this.gbEmplFilter.Controls.Add(this.btnWUTree);
            this.gbEmplFilter.Controls.Add(this.chbHierachyOU);
            this.gbEmplFilter.Controls.Add(this.cbWU);
            this.gbEmplFilter.Controls.Add(this.rbOU);
            this.gbEmplFilter.Controls.Add(this.rbWU);
            this.gbEmplFilter.Location = new System.Drawing.Point(9, 3);
            this.gbEmplFilter.Name = "gbEmplFilter";
            this.gbEmplFilter.Size = new System.Drawing.Size(670, 188);
            this.gbEmplFilter.TabIndex = 0;
            this.gbEmplFilter.TabStop = false;
            this.gbEmplFilter.Text = "Employee filter";
            // 
            // rbWSDay
            // 
            this.rbWSDay.AutoSize = true;
            this.rbWSDay.Location = new System.Drawing.Point(322, 46);
            this.rbWSDay.Name = "rbWSDay";
            this.rbWSDay.Size = new System.Drawing.Size(49, 17);
            this.rbWSDay.TabIndex = 10;
            this.rbWSDay.TabStop = true;
            this.rbWSDay.Text = "Shift:";
            this.rbWSDay.UseVisualStyleBackColor = true;
            this.rbWSDay.CheckedChanged += new System.EventHandler(this.rbWSDay_CheckedChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(571, 153);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 14;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbPath
            // 
            this.tbPath.Enabled = false;
            this.tbPath.Location = new System.Drawing.Point(116, 155);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(449, 20);
            this.tbPath.TabIndex = 13;
            // 
            // rbFile
            // 
            this.rbFile.AutoSize = true;
            this.rbFile.Location = new System.Drawing.Point(12, 156);
            this.rbFile.Name = "rbFile";
            this.rbFile.Size = new System.Drawing.Size(98, 17);
            this.rbFile.TabIndex = 12;
            this.rbFile.TabStop = true;
            this.rbFile.Text = "Upload from file";
            this.rbFile.UseVisualStyleBackColor = true;
            this.rbFile.CheckedChanged += new System.EventHandler(this.rbFile_CheckedChanged);
            // 
            // gbWS
            // 
            this.gbWS.Controls.Add(this.btnAdd);
            this.gbWS.Controls.Add(this.dtpDay);
            this.gbWS.Controls.Add(this.lblDay);
            this.gbWS.Controls.Add(this.lblCycleDay);
            this.gbWS.Controls.Add(this.lblWS);
            this.gbWS.Controls.Add(this.cbCycleDay);
            this.gbWS.Controls.Add(this.cbWS);
            this.gbWS.Location = new System.Drawing.Point(391, 43);
            this.gbWS.Name = "gbWS";
            this.gbWS.Size = new System.Drawing.Size(265, 104);
            this.gbWS.TabIndex = 11;
            this.gbWS.TabStop = false;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(211, 67);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(44, 23);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dtpDay
            // 
            this.dtpDay.CustomFormat = "dd.MM.yyyy";
            this.dtpDay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDay.Location = new System.Drawing.Point(84, 69);
            this.dtpDay.Name = "dtpDay";
            this.dtpDay.Size = new System.Drawing.Size(100, 20);
            this.dtpDay.TabIndex = 5;
            // 
            // lblDay
            // 
            this.lblDay.Location = new System.Drawing.Point(23, 72);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(60, 13);
            this.lblDay.TabIndex = 4;
            this.lblDay.Text = "Day:";
            this.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCycleDay
            // 
            this.lblCycleDay.Location = new System.Drawing.Point(17, 45);
            this.lblCycleDay.Name = "lblCycleDay";
            this.lblCycleDay.Size = new System.Drawing.Size(66, 13);
            this.lblCycleDay.TabIndex = 2;
            this.lblCycleDay.Text = "Cycle day:";
            this.lblCycleDay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWS
            // 
            this.lblWS.Location = new System.Drawing.Point(6, 18);
            this.lblWS.Name = "lblWS";
            this.lblWS.Size = new System.Drawing.Size(77, 13);
            this.lblWS.TabIndex = 0;
            this.lblWS.Text = "Time schema:";
            this.lblWS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbCycleDay
            // 
            this.cbCycleDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCycleDay.FormattingEnabled = true;
            this.cbCycleDay.Location = new System.Drawing.Point(84, 42);
            this.cbCycleDay.Name = "cbCycleDay";
            this.cbCycleDay.Size = new System.Drawing.Size(171, 21);
            this.cbCycleDay.TabIndex = 3;
            // 
            // cbWS
            // 
            this.cbWS.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbWS.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbWS.FormattingEnabled = true;
            this.cbWS.Location = new System.Drawing.Point(84, 15);
            this.cbWS.Name = "cbWS";
            this.cbWS.Size = new System.Drawing.Size(171, 21);
            this.cbWS.TabIndex = 1;
            this.cbWS.SelectedIndexChanged += new System.EventHandler(this.cbWS_SelectedIndexChanged);
            // 
            // cbWSGroup
            // 
            this.cbWSGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbWSGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbWSGroup.FormattingEnabled = true;
            this.cbWSGroup.Location = new System.Drawing.Point(431, 16);
            this.cbWSGroup.Name = "cbWSGroup";
            this.cbWSGroup.Size = new System.Drawing.Size(219, 21);
            this.cbWSGroup.TabIndex = 9;
            this.cbWSGroup.SelectedIndexChanged += new System.EventHandler(this.cbWSGroup_SelectedIndexChanged);
            // 
            // rbWSGroup
            // 
            this.rbWSGroup.AutoSize = true;
            this.rbWSGroup.Location = new System.Drawing.Point(322, 17);
            this.rbWSGroup.Name = "rbWSGroup";
            this.rbWSGroup.Size = new System.Drawing.Size(103, 17);
            this.rbWSGroup.TabIndex = 8;
            this.rbWSGroup.TabStop = true;
            this.rbWSGroup.Text = "Group schedule:";
            this.rbWSGroup.UseVisualStyleBackColor = true;
            this.rbWSGroup.CheckedChanged += new System.EventHandler(this.rbWSGroup_CheckedChanged);
            // 
            // chbHierarhiclyWU
            // 
            this.chbHierarhiclyWU.Location = new System.Drawing.Point(59, 43);
            this.chbHierarhiclyWU.Name = "chbHierarhiclyWU";
            this.chbHierarhiclyWU.Size = new System.Drawing.Size(83, 24);
            this.chbHierarhiclyWU.TabIndex = 3;
            this.chbHierarhiclyWU.Text = "Hierarchy ";
            this.chbHierarhiclyWU.CheckedChanged += new System.EventHandler(this.chbHierarhiclyWU_CheckedChanged);
            // 
            // btnOUTree
            // 
            this.btnOUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnOUTree.Image")));
            this.btnOUTree.Location = new System.Drawing.Point(268, 71);
            this.btnOUTree.Name = "btnOUTree";
            this.btnOUTree.Size = new System.Drawing.Size(25, 23);
            this.btnOUTree.TabIndex = 6;
            this.btnOUTree.Click += new System.EventHandler(this.btnOUTree_Click);
            // 
            // cbOU
            // 
            this.cbOU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbOU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbOU.FormattingEnabled = true;
            this.cbOU.Location = new System.Drawing.Point(59, 73);
            this.cbOU.Name = "cbOU";
            this.cbOU.Size = new System.Drawing.Size(203, 21);
            this.cbOU.TabIndex = 5;
            this.cbOU.SelectedIndexChanged += new System.EventHandler(this.cbOU_SelectedIndexChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(268, 14);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 2;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // chbHierachyOU
            // 
            this.chbHierachyOU.Location = new System.Drawing.Point(59, 100);
            this.chbHierachyOU.Name = "chbHierachyOU";
            this.chbHierachyOU.Size = new System.Drawing.Size(83, 24);
            this.chbHierachyOU.TabIndex = 7;
            this.chbHierachyOU.Text = "Hierarchy ";
            this.chbHierachyOU.CheckedChanged += new System.EventHandler(this.chbHierachyOU_CheckedChanged);
            // 
            // cbWU
            // 
            this.cbWU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbWU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(59, 16);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(203, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // rbOU
            // 
            this.rbOU.AutoSize = true;
            this.rbOU.Location = new System.Drawing.Point(10, 74);
            this.rbOU.Name = "rbOU";
            this.rbOU.Size = new System.Drawing.Size(41, 17);
            this.rbOU.TabIndex = 4;
            this.rbOU.TabStop = true;
            this.rbOU.Text = "OU";
            this.rbOU.UseVisualStyleBackColor = true;
            this.rbOU.CheckedChanged += new System.EventHandler(this.rbOU_CheckedChanged);
            // 
            // rbWU
            // 
            this.rbWU.AutoSize = true;
            this.rbWU.Location = new System.Drawing.Point(10, 17);
            this.rbWU.Name = "rbWU";
            this.rbWU.Size = new System.Drawing.Size(38, 17);
            this.rbWU.TabIndex = 0;
            this.rbWU.TabStop = true;
            this.rbWU.Text = "FS";
            this.rbWU.UseVisualStyleBackColor = true;
            this.rbWU.CheckedChanged += new System.EventHandler(this.rbWU_CheckedChanged);
            // 
            // gbEmployees
            // 
            this.gbEmployees.Controls.Add(this.lblEmplInfo);
            this.gbEmployees.Controls.Add(this.lvEmployees);
            this.gbEmployees.Controls.Add(this.tbEmployee);
            this.gbEmployees.Location = new System.Drawing.Point(685, 3);
            this.gbEmployees.Name = "gbEmployees";
            this.gbEmployees.Size = new System.Drawing.Size(297, 331);
            this.gbEmployees.TabIndex = 1;
            this.gbEmployees.TabStop = false;
            this.gbEmployees.Text = "Employees";
            // 
            // lblEmplInfo
            // 
            this.lblEmplInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmplInfo.Location = new System.Drawing.Point(6, 292);
            this.lblEmplInfo.Name = "lblEmplInfo";
            this.lblEmplInfo.Size = new System.Drawing.Size(279, 28);
            this.lblEmplInfo.TabIndex = 2;
            this.lblEmplInfo.Text = "* If there are no selected, action is performed under all employees from list";
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(9, 42);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.ShowItemToolTips = true;
            this.lvEmployees.Size = new System.Drawing.Size(279, 247);
            this.lvEmployees.TabIndex = 1;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployees_ColumnClick);
            // 
            // tbEmployee
            // 
            this.tbEmployee.Location = new System.Drawing.Point(9, 16);
            this.tbEmployee.Name = "tbEmployee";
            this.tbEmployee.Size = new System.Drawing.Size(200, 20);
            this.tbEmployee.TabIndex = 0;
            this.tbEmployee.TextChanged += new System.EventHandler(this.tbEmployee_TextChanged);
            // 
            // gbAction
            // 
            this.gbAction.Controls.Add(this.chbIncludeAdditional);
            this.gbAction.Controls.Add(this.gbSettings);
            this.gbAction.Controls.Add(this.cbAction);
            this.gbAction.Controls.Add(this.lblAction);
            this.gbAction.Location = new System.Drawing.Point(9, 197);
            this.gbAction.Name = "gbAction";
            this.gbAction.Size = new System.Drawing.Size(498, 165);
            this.gbAction.TabIndex = 2;
            this.gbAction.TabStop = false;
            this.gbAction.Text = "Action";
            // 
            // chbIncludeAdditional
            // 
            this.chbIncludeAdditional.AutoSize = true;
            this.chbIncludeAdditional.Location = new System.Drawing.Point(364, 15);
            this.chbIncludeAdditional.Name = "chbIncludeAdditional";
            this.chbIncludeAdditional.Size = new System.Drawing.Size(100, 17);
            this.chbIncludeAdditional.TabIndex = 2;
            this.chbIncludeAdditional.Text = "Additional types";
            this.chbIncludeAdditional.UseVisualStyleBackColor = true;
            this.chbIncludeAdditional.CheckedChanged += new System.EventHandler(this.chbIncludeAdditional_CheckedChanged);
            // 
            // gbSettings
            // 
            this.gbSettings.Controls.Add(this.chbPublicHoliday);
            this.gbSettings.Controls.Add(this.chbStoppage);
            this.gbSettings.Controls.Add(this.chbLayOff);
            this.gbSettings.Controls.Add(this.chbClosure);
            this.gbSettings.Controls.Add(this.numHours);
            this.gbSettings.Controls.Add(this.chbHours);
            this.gbSettings.Controls.Add(this.lblPassTypeNew);
            this.gbSettings.Controls.Add(this.cbPassTypeNew);
            this.gbSettings.Controls.Add(this.rbCheckLimit);
            this.gbSettings.Controls.Add(this.rbDoNotCheckLimit);
            this.gbSettings.Controls.Add(this.lblPassType);
            this.gbSettings.Controls.Add(this.cbPassType);
            this.gbSettings.Location = new System.Drawing.Point(12, 40);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(480, 116);
            this.gbSettings.TabIndex = 3;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Settings";
            // 
            // chbPublicHoliday
            // 
            this.chbPublicHoliday.AutoSize = true;
            this.chbPublicHoliday.Location = new System.Drawing.Point(384, 87);
            this.chbPublicHoliday.Name = "chbPublicHoliday";
            this.chbPublicHoliday.Size = new System.Drawing.Size(90, 17);
            this.chbPublicHoliday.TabIndex = 11;
            this.chbPublicHoliday.Text = "public holiday";
            this.chbPublicHoliday.UseVisualStyleBackColor = true;
            this.chbPublicHoliday.CheckedChanged += new System.EventHandler(this.chbPublicHoliday_CheckedChanged);
            // 
            // chbStoppage
            // 
            this.chbStoppage.AutoSize = true;
            this.chbStoppage.Location = new System.Drawing.Point(384, 64);
            this.chbStoppage.Name = "chbStoppage";
            this.chbStoppage.Size = new System.Drawing.Size(70, 17);
            this.chbStoppage.TabIndex = 10;
            this.chbStoppage.Text = "stoppage";
            this.chbStoppage.UseVisualStyleBackColor = true;
            this.chbStoppage.CheckedChanged += new System.EventHandler(this.chbStoppage_CheckedChanged);
            // 
            // chbLayOff
            // 
            this.chbLayOff.AutoSize = true;
            this.chbLayOff.Location = new System.Drawing.Point(384, 39);
            this.chbLayOff.Name = "chbLayOff";
            this.chbLayOff.Size = new System.Drawing.Size(51, 17);
            this.chbLayOff.TabIndex = 9;
            this.chbLayOff.Text = "layoff";
            this.chbLayOff.UseVisualStyleBackColor = true;
            this.chbLayOff.CheckedChanged += new System.EventHandler(this.chbLayOff_CheckedChanged);
            // 
            // chbClosure
            // 
            this.chbClosure.AutoSize = true;
            this.chbClosure.Location = new System.Drawing.Point(384, 15);
            this.chbClosure.Name = "chbClosure";
            this.chbClosure.Size = new System.Drawing.Size(60, 17);
            this.chbClosure.TabIndex = 8;
            this.chbClosure.Text = "closure";
            this.chbClosure.UseVisualStyleBackColor = true;
            this.chbClosure.CheckedChanged += new System.EventHandler(this.chbClosure_CheckedChanged);
            // 
            // numHours
            // 
            this.numHours.DecimalPlaces = 2;
            this.numHours.Location = new System.Drawing.Point(325, 43);
            this.numHours.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numHours.Name = "numHours";
            this.numHours.Size = new System.Drawing.Size(53, 20);
            this.numHours.TabIndex = 5;
            // 
            // chbHours
            // 
            this.chbHours.AutoSize = true;
            this.chbHours.Location = new System.Drawing.Point(325, 17);
            this.chbHours.Name = "chbHours";
            this.chbHours.Size = new System.Drawing.Size(52, 17);
            this.chbHours.TabIndex = 4;
            this.chbHours.Text = "hours";
            this.chbHours.UseVisualStyleBackColor = true;
            this.chbHours.CheckedChanged += new System.EventHandler(this.chbHours_CheckedChanged);
            // 
            // lblPassTypeNew
            // 
            this.lblPassTypeNew.Location = new System.Drawing.Point(6, 45);
            this.lblPassTypeNew.Name = "lblPassTypeNew";
            this.lblPassTypeNew.Size = new System.Drawing.Size(105, 13);
            this.lblPassTypeNew.TabIndex = 2;
            this.lblPassTypeNew.Text = "New pass type:";
            this.lblPassTypeNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPassTypeNew
            // 
            this.cbPassTypeNew.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbPassTypeNew.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbPassTypeNew.FormattingEnabled = true;
            this.cbPassTypeNew.Location = new System.Drawing.Point(117, 42);
            this.cbPassTypeNew.Name = "cbPassTypeNew";
            this.cbPassTypeNew.Size = new System.Drawing.Size(202, 21);
            this.cbPassTypeNew.TabIndex = 3;
            this.cbPassTypeNew.SelectedIndexChanged += new System.EventHandler(this.cbPassTypeNew_SelectedIndexChanged);
            // 
            // rbCheckLimit
            // 
            this.rbCheckLimit.AutoSize = true;
            this.rbCheckLimit.Location = new System.Drawing.Point(22, 69);
            this.rbCheckLimit.Name = "rbCheckLimit";
            this.rbCheckLimit.Size = new System.Drawing.Size(76, 17);
            this.rbCheckLimit.TabIndex = 6;
            this.rbCheckLimit.TabStop = true;
            this.rbCheckLimit.Text = "Check limit";
            this.rbCheckLimit.UseVisualStyleBackColor = true;
            // 
            // rbDoNotCheckLimit
            // 
            this.rbDoNotCheckLimit.AutoSize = true;
            this.rbDoNotCheckLimit.Location = new System.Drawing.Point(22, 92);
            this.rbDoNotCheckLimit.Name = "rbDoNotCheckLimit";
            this.rbDoNotCheckLimit.Size = new System.Drawing.Size(110, 17);
            this.rbDoNotCheckLimit.TabIndex = 7;
            this.rbDoNotCheckLimit.TabStop = true;
            this.rbDoNotCheckLimit.Text = "Do not check limit";
            this.rbDoNotCheckLimit.UseVisualStyleBackColor = true;
            // 
            // lblPassType
            // 
            this.lblPassType.Location = new System.Drawing.Point(6, 18);
            this.lblPassType.Name = "lblPassType";
            this.lblPassType.Size = new System.Drawing.Size(105, 13);
            this.lblPassType.TabIndex = 0;
            this.lblPassType.Text = "Pass type:";
            this.lblPassType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPassType
            // 
            this.cbPassType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbPassType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbPassType.FormattingEnabled = true;
            this.cbPassType.Location = new System.Drawing.Point(117, 15);
            this.cbPassType.Name = "cbPassType";
            this.cbPassType.Size = new System.Drawing.Size(202, 21);
            this.cbPassType.TabIndex = 1;
            this.cbPassType.SelectedIndexChanged += new System.EventHandler(this.cbPassType_SelectedIndexChanged);
            // 
            // cbAction
            // 
            this.cbAction.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbAction.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbAction.FormattingEnabled = true;
            this.cbAction.Location = new System.Drawing.Point(80, 13);
            this.cbAction.Name = "cbAction";
            this.cbAction.Size = new System.Drawing.Size(251, 21);
            this.cbAction.TabIndex = 1;
            this.cbAction.SelectedIndexChanged += new System.EventHandler(this.cbAction_SelectedIndexChanged);
            // 
            // lblAction
            // 
            this.lblAction.Location = new System.Drawing.Point(7, 16);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(68, 13);
            this.lblAction.TabIndex = 0;
            this.lblAction.Text = "Action:";
            this.lblAction.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbPeriod
            // 
            this.gbPeriod.Controls.Add(this.lblTo);
            this.gbPeriod.Controls.Add(this.lblFrom);
            this.gbPeriod.Controls.Add(this.dtpTo);
            this.gbPeriod.Controls.Add(this.dtpFrom);
            this.gbPeriod.Location = new System.Drawing.Point(513, 197);
            this.gbPeriod.Name = "gbPeriod";
            this.gbPeriod.Size = new System.Drawing.Size(166, 81);
            this.gbPeriod.TabIndex = 3;
            this.gbPeriod.TabStop = false;
            this.gbPeriod.Text = "Period";
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(10, 49);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(40, 13);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(11, 23);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 13);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(57, 46);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(97, 20);
            this.dtpTo.TabIndex = 3;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(57, 20);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(97, 20);
            this.dtpFrom.TabIndex = 1;
            // 
            // btnGeneratePreview
            // 
            this.btnGeneratePreview.Location = new System.Drawing.Point(525, 282);
            this.btnGeneratePreview.Name = "btnGeneratePreview";
            this.btnGeneratePreview.Size = new System.Drawing.Size(147, 23);
            this.btnGeneratePreview.TabIndex = 4;
            this.btnGeneratePreview.Text = "Generate preview";
            this.btnGeneratePreview.UseVisualStyleBackColor = true;
            this.btnGeneratePreview.Click += new System.EventHandler(this.btnGeneratePreview_Click);
            // 
            // btnExportPreview
            // 
            this.btnExportPreview.Location = new System.Drawing.Point(525, 313);
            this.btnExportPreview.Name = "btnExportPreview";
            this.btnExportPreview.Size = new System.Drawing.Size(147, 23);
            this.btnExportPreview.TabIndex = 5;
            this.btnExportPreview.Text = "Export preview";
            this.btnExportPreview.UseVisualStyleBackColor = true;
            this.btnExportPreview.Click += new System.EventHandler(this.btnExportPreview_Click);
            // 
            // gbPreview
            // 
            this.gbPreview.Controls.Add(this.lblPreviewInfo);
            this.gbPreview.Controls.Add(this.btnNext);
            this.gbPreview.Controls.Add(this.btnPrev);
            this.gbPreview.Controls.Add(this.lvPreview);
            this.gbPreview.Location = new System.Drawing.Point(12, 368);
            this.gbPreview.Name = "gbPreview";
            this.gbPreview.Size = new System.Drawing.Size(970, 276);
            this.gbPreview.TabIndex = 9;
            this.gbPreview.TabStop = false;
            this.gbPreview.Text = "Preview";
            // 
            // lblPreviewInfo
            // 
            this.lblPreviewInfo.AutoSize = true;
            this.lblPreviewInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreviewInfo.Location = new System.Drawing.Point(13, 19);
            this.lblPreviewInfo.Name = "lblPreviewInfo";
            this.lblPreviewInfo.Size = new System.Drawing.Size(227, 13);
            this.lblPreviewInfo.TabIndex = 0;
            this.lblPreviewInfo.Text = "* Only days that will be changed are in preview";
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(912, 11);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(44, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(862, 11);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(44, 23);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "<";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lvPreview
            // 
            this.lvPreview.FullRowSelect = true;
            this.lvPreview.GridLines = true;
            this.lvPreview.HideSelection = false;
            this.lvPreview.Location = new System.Drawing.Point(10, 38);
            this.lvPreview.Name = "lvPreview";
            this.lvPreview.ShowItemToolTips = true;
            this.lvPreview.Size = new System.Drawing.Size(946, 232);
            this.lvPreview.TabIndex = 3;
            this.lvPreview.UseCompatibleStateImageBehavior = false;
            this.lvPreview.View = System.Windows.Forms.View.Details;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 650);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(907, 650);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tbLogPath
            // 
            this.tbLogPath.Enabled = false;
            this.tbLogPath.Location = new System.Drawing.Point(593, 342);
            this.tbLogPath.Name = "tbLogPath";
            this.tbLogPath.Size = new System.Drawing.Size(308, 20);
            this.tbLogPath.TabIndex = 7;
            // 
            // btnLogBrowse
            // 
            this.btnLogBrowse.Location = new System.Drawing.Point(907, 340);
            this.btnLogBrowse.Name = "btnLogBrowse";
            this.btnLogBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnLogBrowse.TabIndex = 8;
            this.btnLogBrowse.Text = "Browse";
            this.btnLogBrowse.UseVisualStyleBackColor = true;
            this.btnLogBrowse.Click += new System.EventHandler(this.btnLogBrowse_Click);
            // 
            // lblLogPath
            // 
            this.lblLogPath.Location = new System.Drawing.Point(513, 345);
            this.lblLogPath.Name = "lblLogPath";
            this.lblLogPath.Size = new System.Drawing.Size(74, 13);
            this.lblLogPath.TabIndex = 6;
            this.lblLogPath.Text = "Log path:";
            this.lblLogPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AbsenceMassiveInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 694);
            this.ControlBox = false;
            this.Controls.Add(this.lblLogPath);
            this.Controls.Add(this.btnLogBrowse);
            this.Controls.Add(this.tbLogPath);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbPreview);
            this.Controls.Add(this.btnExportPreview);
            this.Controls.Add(this.btnGeneratePreview);
            this.Controls.Add(this.gbPeriod);
            this.Controls.Add(this.gbAction);
            this.Controls.Add(this.gbEmployees);
            this.Controls.Add(this.gbEmplFilter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AbsenceMassiveInput";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Absence Massive Input";
            this.Load += new System.EventHandler(this.AbsenceMassiveInput_Load);
            this.gbEmplFilter.ResumeLayout(false);
            this.gbEmplFilter.PerformLayout();
            this.gbWS.ResumeLayout(false);
            this.gbEmployees.ResumeLayout(false);
            this.gbEmployees.PerformLayout();
            this.gbAction.ResumeLayout(false);
            this.gbAction.PerformLayout();
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHours)).EndInit();
            this.gbPeriod.ResumeLayout(false);
            this.gbPreview.ResumeLayout(false);
            this.gbPreview.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbEmplFilter;
        private System.Windows.Forms.GroupBox gbEmployees;
        private System.Windows.Forms.GroupBox gbAction;
        private System.Windows.Forms.GroupBox gbPeriod;
        private System.Windows.Forms.Button btnGeneratePreview;
        private System.Windows.Forms.Button btnExportPreview;
        private System.Windows.Forms.GroupBox gbPreview;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chbHierarhiclyWU;
        private System.Windows.Forms.Button btnOUTree;
        private System.Windows.Forms.ComboBox cbOU;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.CheckBox chbHierachyOU;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.RadioButton rbOU;
        private System.Windows.Forms.RadioButton rbWU;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.ListView lvPreview;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.RadioButton rbFile;
        private System.Windows.Forms.GroupBox gbWS;
        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.Label lblCycleDay;
        private System.Windows.Forms.Label lblWS;
        private System.Windows.Forms.ComboBox cbCycleDay;
        private System.Windows.Forms.ComboBox cbWS;
        private System.Windows.Forms.ComboBox cbWSGroup;
        private System.Windows.Forms.RadioButton rbWSGroup;
        private System.Windows.Forms.RadioButton rbWSDay;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.TextBox tbEmployee;
        private System.Windows.Forms.DateTimePicker dtpDay;
        private System.Windows.Forms.ComboBox cbAction;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.Label lblPassType;
        private System.Windows.Forms.ComboBox cbPassType;
        private System.Windows.Forms.RadioButton rbCheckLimit;
        private System.Windows.Forms.RadioButton rbDoNotCheckLimit;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblPreviewInfo;
        private System.Windows.Forms.Label lblEmplInfo;
        private System.Windows.Forms.Label lblPassTypeNew;
        private System.Windows.Forms.ComboBox cbPassTypeNew;
        private System.Windows.Forms.NumericUpDown numHours;
        private System.Windows.Forms.CheckBox chbHours;
        private System.Windows.Forms.TextBox tbLogPath;
        private System.Windows.Forms.Button btnLogBrowse;
        private System.Windows.Forms.Label lblLogPath;
        private System.Windows.Forms.CheckBox chbIncludeAdditional;
        private System.Windows.Forms.CheckBox chbPublicHoliday;
        private System.Windows.Forms.CheckBox chbStoppage;
        private System.Windows.Forms.CheckBox chbLayOff;
        private System.Windows.Forms.CheckBox chbClosure;
    }
}