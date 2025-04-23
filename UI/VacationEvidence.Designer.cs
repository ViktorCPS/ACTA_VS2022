namespace UI
{
    partial class VacationEvidence
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VacationEvidence));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.chbIncludePreviously = new System.Windows.Forms.CheckBox();
            this.lblYear = new System.Windows.Forms.Label();
            this.dtpYearFrom = new System.Windows.Forms.DateTimePicker();
            this.gbDaysLeft = new System.Windows.Forms.GroupBox();
            this.nudFromLeft = new System.Windows.Forms.NumericUpDown();
            this.nudToLeft = new System.Windows.Forms.NumericUpDown();
            this.lblToLeft = new System.Windows.Forms.Label();
            this.lblFromLeft = new System.Windows.Forms.Label();
            this.gbUsedDays = new System.Windows.Forms.GroupBox();
            this.nudToUsed = new System.Windows.Forms.NumericUpDown();
            this.nudFromUsed = new System.Windows.Forms.NumericUpDown();
            this.lblToUsed = new System.Windows.Forms.Label();
            this.lblFromUsed = new System.Windows.Forms.Label();
            this.gbApprovedDays = new System.Windows.Forms.GroupBox();
            this.nudToApproved = new System.Windows.Forms.NumericUpDown();
            this.nudFromApproved = new System.Windows.Forms.NumericUpDown();
            this.lblToApproved = new System.Windows.Forms.Label();
            this.lblFromApproved = new System.Windows.Forms.Label();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblWU = new System.Windows.Forms.Label();
            this.lvVacations = new System.Windows.Forms.ListView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblForCurrentYear = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblEnter = new System.Windows.Forms.Label();
            this.tbEnter = new System.Windows.Forms.TextBox();
            this.tbLastModification = new System.Windows.Forms.TextBox();
            this.lblLastModification = new System.Windows.Forms.Label();
            this.btnUsing = new System.Windows.Forms.Button();
            this.btnDetailes = new System.Windows.Forms.Button();
            this.gbReport = new System.Windows.Forms.GroupBox();
            this.chbIncludeDetails = new System.Windows.Forms.CheckBox();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbSearch.SuspendLayout();
            this.gbDaysLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFromLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudToLeft)).BeginInit();
            this.gbUsedDays.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudToUsed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFromUsed)).BeginInit();
            this.gbApprovedDays.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudToApproved)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFromApproved)).BeginInit();
            this.gbReport.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(916, 688);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 44;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(291, 688);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 41;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(155, 688);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 40;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(19, 688);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 39;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(272, 14);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 38;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.chbIncludePreviously);
            this.gbSearch.Controls.Add(this.lblYear);
            this.gbSearch.Controls.Add(this.dtpYearFrom);
            this.gbSearch.Controls.Add(this.gbDaysLeft);
            this.gbSearch.Controls.Add(this.gbUsedDays);
            this.gbSearch.Controls.Add(this.gbApprovedDays);
            this.gbSearch.Controls.Add(this.chbHierarhicly);
            this.gbSearch.Controls.Add(this.btnWUTree);
            this.gbSearch.Controls.Add(this.cbEmployee);
            this.gbSearch.Controls.Add(this.lblEmployee);
            this.gbSearch.Controls.Add(this.cbWU);
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Controls.Add(this.lblWU);
            this.gbSearch.Location = new System.Drawing.Point(19, 12);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(711, 225);
            this.gbSearch.TabIndex = 0;
            this.gbSearch.TabStop = false;
            this.gbSearch.Tag = "FILTERABLE";
            this.gbSearch.Text = "Passes";
            // 
            // chbIncludePreviously
            // 
            this.chbIncludePreviously.Location = new System.Drawing.Point(174, 104);
            this.chbIncludePreviously.Name = "chbIncludePreviously";
            this.chbIncludePreviously.Size = new System.Drawing.Size(142, 24);
            this.chbIncludePreviously.TabIndex = 8;
            this.chbIncludePreviously.Text = "Include previously ";
            this.chbIncludePreviously.EnabledChanged += new System.EventHandler(this.chbIncludePreviously_EnabledChanged);
            // 
            // lblYear
            // 
            this.lblYear.Location = new System.Drawing.Point(38, 104);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(66, 23);
            this.lblYear.TabIndex = 6;
            this.lblYear.Text = "Year:";
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpYearFrom
            // 
            this.dtpYearFrom.CustomFormat = "yyyy";
            this.dtpYearFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpYearFrom.ImeMode = System.Windows.Forms.ImeMode.On;
            this.dtpYearFrom.Location = new System.Drawing.Point(112, 105);
            this.dtpYearFrom.Name = "dtpYearFrom";
            this.dtpYearFrom.ShowUpDown = true;
            this.dtpYearFrom.Size = new System.Drawing.Size(56, 20);
            this.dtpYearFrom.TabIndex = 7;
            this.dtpYearFrom.ValueChanged += new System.EventHandler(this.dtpYearFrom_ValueChanged);
            // 
            // gbDaysLeft
            // 
            this.gbDaysLeft.Controls.Add(this.nudFromLeft);
            this.gbDaysLeft.Controls.Add(this.nudToLeft);
            this.gbDaysLeft.Controls.Add(this.lblToLeft);
            this.gbDaysLeft.Controls.Add(this.lblFromLeft);
            this.gbDaysLeft.Location = new System.Drawing.Point(463, 133);
            this.gbDaysLeft.Name = "gbDaysLeft";
            this.gbDaysLeft.Size = new System.Drawing.Size(230, 51);
            this.gbDaysLeft.TabIndex = 19;
            this.gbDaysLeft.TabStop = false;
            this.gbDaysLeft.Text = "Days left";
            // 
            // nudFromLeft
            // 
            this.nudFromLeft.Location = new System.Drawing.Point(79, 19);
            this.nudFromLeft.Name = "nudFromLeft";
            this.nudFromLeft.Size = new System.Drawing.Size(45, 20);
            this.nudFromLeft.TabIndex = 21;
            // 
            // nudToLeft
            // 
            this.nudToLeft.Location = new System.Drawing.Point(179, 19);
            this.nudToLeft.Name = "nudToLeft";
            this.nudToLeft.Size = new System.Drawing.Size(45, 20);
            this.nudToLeft.TabIndex = 23;
            this.nudToLeft.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // lblToLeft
            // 
            this.lblToLeft.Location = new System.Drawing.Point(130, 16);
            this.lblToLeft.Name = "lblToLeft";
            this.lblToLeft.Size = new System.Drawing.Size(42, 23);
            this.lblToLeft.TabIndex = 22;
            this.lblToLeft.Text = "To:";
            this.lblToLeft.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFromLeft
            // 
            this.lblFromLeft.Location = new System.Drawing.Point(6, 16);
            this.lblFromLeft.Name = "lblFromLeft";
            this.lblFromLeft.Size = new System.Drawing.Size(66, 23);
            this.lblFromLeft.TabIndex = 20;
            this.lblFromLeft.Text = "From:";
            this.lblFromLeft.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbUsedDays
            // 
            this.gbUsedDays.Controls.Add(this.nudToUsed);
            this.gbUsedDays.Controls.Add(this.nudFromUsed);
            this.gbUsedDays.Controls.Add(this.lblToUsed);
            this.gbUsedDays.Controls.Add(this.lblFromUsed);
            this.gbUsedDays.Location = new System.Drawing.Point(463, 76);
            this.gbUsedDays.Name = "gbUsedDays";
            this.gbUsedDays.Size = new System.Drawing.Size(230, 51);
            this.gbUsedDays.TabIndex = 14;
            this.gbUsedDays.TabStop = false;
            this.gbUsedDays.Text = "Used days";
            // 
            // nudToUsed
            // 
            this.nudToUsed.Location = new System.Drawing.Point(179, 19);
            this.nudToUsed.Name = "nudToUsed";
            this.nudToUsed.Size = new System.Drawing.Size(45, 20);
            this.nudToUsed.TabIndex = 18;
            this.nudToUsed.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // nudFromUsed
            // 
            this.nudFromUsed.Location = new System.Drawing.Point(79, 19);
            this.nudFromUsed.Name = "nudFromUsed";
            this.nudFromUsed.Size = new System.Drawing.Size(45, 20);
            this.nudFromUsed.TabIndex = 16;
            // 
            // lblToUsed
            // 
            this.lblToUsed.Location = new System.Drawing.Point(130, 16);
            this.lblToUsed.Name = "lblToUsed";
            this.lblToUsed.Size = new System.Drawing.Size(42, 23);
            this.lblToUsed.TabIndex = 17;
            this.lblToUsed.Text = "To:";
            this.lblToUsed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFromUsed
            // 
            this.lblFromUsed.Location = new System.Drawing.Point(6, 16);
            this.lblFromUsed.Name = "lblFromUsed";
            this.lblFromUsed.Size = new System.Drawing.Size(66, 23);
            this.lblFromUsed.TabIndex = 15;
            this.lblFromUsed.Text = "From:";
            this.lblFromUsed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbApprovedDays
            // 
            this.gbApprovedDays.Controls.Add(this.nudToApproved);
            this.gbApprovedDays.Controls.Add(this.nudFromApproved);
            this.gbApprovedDays.Controls.Add(this.lblToApproved);
            this.gbApprovedDays.Controls.Add(this.lblFromApproved);
            this.gbApprovedDays.Location = new System.Drawing.Point(463, 19);
            this.gbApprovedDays.Name = "gbApprovedDays";
            this.gbApprovedDays.Size = new System.Drawing.Size(230, 51);
            this.gbApprovedDays.TabIndex = 9;
            this.gbApprovedDays.TabStop = false;
            this.gbApprovedDays.Text = "Approve days";
            // 
            // nudToApproved
            // 
            this.nudToApproved.Location = new System.Drawing.Point(179, 20);
            this.nudToApproved.Name = "nudToApproved";
            this.nudToApproved.Size = new System.Drawing.Size(45, 20);
            this.nudToApproved.TabIndex = 13;
            this.nudToApproved.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // nudFromApproved
            // 
            this.nudFromApproved.Location = new System.Drawing.Point(79, 18);
            this.nudFromApproved.Name = "nudFromApproved";
            this.nudFromApproved.Size = new System.Drawing.Size(45, 20);
            this.nudFromApproved.TabIndex = 11;
            // 
            // lblToApproved
            // 
            this.lblToApproved.Location = new System.Drawing.Point(130, 16);
            this.lblToApproved.Name = "lblToApproved";
            this.lblToApproved.Size = new System.Drawing.Size(42, 23);
            this.lblToApproved.TabIndex = 12;
            this.lblToApproved.Text = "To:";
            this.lblToApproved.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFromApproved
            // 
            this.lblFromApproved.Location = new System.Drawing.Point(6, 16);
            this.lblFromApproved.Name = "lblFromApproved";
            this.lblFromApproved.Size = new System.Drawing.Size(66, 23);
            this.lblFromApproved.TabIndex = 10;
            this.lblFromApproved.Text = "From:";
            this.lblFromApproved.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(353, 22);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 3;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(322, 22);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 2;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbEmployee
            // 
            this.cbEmployee.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmployee.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmployee.Location = new System.Drawing.Point(112, 54);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(204, 21);
            this.cbEmployee.TabIndex = 5;
            this.cbEmployee.Leave += new System.EventHandler(cbEmployee_Leave);
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(32, 52);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(72, 23);
            this.lblEmployee.TabIndex = 4;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.Location = new System.Drawing.Point(112, 24);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(204, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(583, 190);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(110, 23);
            this.btnSearch.TabIndex = 24;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(16, 24);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(88, 23);
            this.lblWU.TabIndex = 0;
            this.lblWU.Text = "Working Unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvVacations
            // 
            this.lvVacations.FullRowSelect = true;
            this.lvVacations.GridLines = true;
            this.lvVacations.HideSelection = false;
            this.lvVacations.Location = new System.Drawing.Point(19, 254);
            this.lvVacations.Name = "lvVacations";
            this.lvVacations.Size = new System.Drawing.Size(972, 216);
            this.lvVacations.TabIndex = 27;
            this.lvVacations.UseCompatibleStateImageBehavior = false;
            this.lvVacations.View = System.Windows.Forms.View.Details;
            this.lvVacations.SelectedIndexChanged += new System.EventHandler(this.lvVacations_SelectedIndexChanged);
            this.lvVacations.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvVacations_ColumnClick);
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(839, 476);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(152, 16);
            this.lblTotal.TabIndex = 29;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(959, 225);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 26;
            this.btnNext.Text = ">";
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(919, 225);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 25;
            this.btnPrev.Text = "<";
            // 
            // lblForCurrentYear
            // 
            this.lblForCurrentYear.Location = new System.Drawing.Point(16, 473);
            this.lblForCurrentYear.Name = "lblForCurrentYear";
            this.lblForCurrentYear.Size = new System.Drawing.Size(137, 23);
            this.lblForCurrentYear.TabIndex = 28;
            this.lblForCurrentYear.Text = "*For current year only";
            this.lblForCurrentYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(19, 529);
            this.richTextBox1.MaxLength = 500;
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(972, 39);
            this.richTextBox1.TabIndex = 31;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(16, 503);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(87, 23);
            this.lblDescription.TabIndex = 30;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEnter
            // 
            this.lblEnter.Location = new System.Drawing.Point(16, 582);
            this.lblEnter.Name = "lblEnter";
            this.lblEnter.Size = new System.Drawing.Size(51, 23);
            this.lblEnter.TabIndex = 32;
            this.lblEnter.Text = "Enter:";
            this.lblEnter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbEnter
            // 
            this.tbEnter.Enabled = false;
            this.tbEnter.Location = new System.Drawing.Point(73, 584);
            this.tbEnter.Name = "tbEnter";
            this.tbEnter.Size = new System.Drawing.Size(300, 20);
            this.tbEnter.TabIndex = 33;
            // 
            // tbLastModification
            // 
            this.tbLastModification.Enabled = false;
            this.tbLastModification.Location = new System.Drawing.Point(691, 584);
            this.tbLastModification.Name = "tbLastModification";
            this.tbLastModification.Size = new System.Drawing.Size(300, 20);
            this.tbLastModification.TabIndex = 35;
            // 
            // lblLastModification
            // 
            this.lblLastModification.Location = new System.Drawing.Point(579, 582);
            this.lblLastModification.Name = "lblLastModification";
            this.lblLastModification.Size = new System.Drawing.Size(106, 23);
            this.lblLastModification.TabIndex = 34;
            this.lblLastModification.Text = "Last modification:";
            this.lblLastModification.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnUsing
            // 
            this.btnUsing.Location = new System.Drawing.Point(427, 688);
            this.btnUsing.Name = "btnUsing";
            this.btnUsing.Size = new System.Drawing.Size(75, 23);
            this.btnUsing.TabIndex = 42;
            this.btnUsing.Text = "Using...";
            this.btnUsing.Click += new System.EventHandler(this.btnUsing_Click);
            // 
            // btnDetailes
            // 
            this.btnDetailes.Location = new System.Drawing.Point(561, 688);
            this.btnDetailes.Name = "btnDetailes";
            this.btnDetailes.Size = new System.Drawing.Size(75, 23);
            this.btnDetailes.TabIndex = 43;
            this.btnDetailes.Text = "Detailes";
            this.btnDetailes.Click += new System.EventHandler(this.btnDetailes_Click);
            // 
            // gbReport
            // 
            this.gbReport.Controls.Add(this.chbIncludeDetails);
            this.gbReport.Controls.Add(this.btnGenerate);
            this.gbReport.Location = new System.Drawing.Point(19, 623);
            this.gbReport.Name = "gbReport";
            this.gbReport.Size = new System.Drawing.Size(354, 44);
            this.gbReport.TabIndex = 36;
            this.gbReport.TabStop = false;
            this.gbReport.Text = "Report";
            // 
            // chbIncludeDetails
            // 
            this.chbIncludeDetails.AutoSize = true;
            this.chbIncludeDetails.Location = new System.Drawing.Point(10, 18);
            this.chbIncludeDetails.Name = "chbIncludeDetails";
            this.chbIncludeDetails.Size = new System.Drawing.Size(94, 17);
            this.chbIncludeDetails.TabIndex = 37;
            this.chbIncludeDetails.Text = "Include details";
            this.chbIncludeDetails.UseVisualStyleBackColor = true;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(736, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 100);
            this.gbFilter.TabIndex = 45;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(32, 56);
            this.btnSaveCriteria.Name = "btnSaveCriteria";
            this.btnSaveCriteria.Size = new System.Drawing.Size(82, 23);
            this.btnSaveCriteria.TabIndex = 16;
            this.btnSaveCriteria.Text = "Save criteria";
            this.btnSaveCriteria.UseVisualStyleBackColor = true;
            this.btnSaveCriteria.Click += new System.EventHandler(this.btnSaveCriteria_Click);
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(6, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // VacationEvidence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 735);
            this.ControlBox = false;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.gbReport);
            this.Controls.Add(this.btnDetailes);
            this.Controls.Add(this.btnUsing);
            this.Controls.Add(this.tbLastModification);
            this.Controls.Add(this.lblLastModification);
            this.Controls.Add(this.tbEnter);
            this.Controls.Add(this.lblEnter);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.lblForCurrentYear);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lvVacations);
            this.Controls.Add(this.gbSearch);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "VacationEvidence";
            this.ShowInTaskbar = false;
            this.Text = "VacationEvidence";
            this.Load += new System.EventHandler(this.VacationEvidence_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.VacationEvidence_KeyUp);
            this.gbSearch.ResumeLayout(false);
            this.gbDaysLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudFromLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudToLeft)).EndInit();
            this.gbUsedDays.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudToUsed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFromUsed)).EndInit();
            this.gbApprovedDays.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudToApproved)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFromApproved)).EndInit();
            this.gbReport.ResumeLayout(false);
            this.gbReport.PerformLayout();
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void cbEmployee_Leave(object sender, System.EventArgs e)
        {
            if (cbEmployee.SelectedIndex == -1) {
                cbEmployee.SelectedIndex = 0;
            }
        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.CheckBox chbHierarhicly;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DateTimePicker dtpYearFrom;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.GroupBox gbApprovedDays;
        private System.Windows.Forms.Label lblToApproved;
        private System.Windows.Forms.Label lblFromApproved;
        private System.Windows.Forms.GroupBox gbUsedDays;
        private System.Windows.Forms.Label lblToUsed;
        private System.Windows.Forms.Label lblFromUsed;
        private System.Windows.Forms.GroupBox gbDaysLeft;
        private System.Windows.Forms.NumericUpDown nudFromLeft;
        private System.Windows.Forms.NumericUpDown nudToLeft;
        private System.Windows.Forms.Label lblToLeft;
        private System.Windows.Forms.Label lblFromLeft;
        private System.Windows.Forms.NumericUpDown nudToUsed;
        private System.Windows.Forms.NumericUpDown nudFromUsed;
        private System.Windows.Forms.NumericUpDown nudToApproved;
        private System.Windows.Forms.NumericUpDown nudFromApproved;
        private System.Windows.Forms.ListView lvVacations;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label lblForCurrentYear;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblEnter;
        private System.Windows.Forms.TextBox tbEnter;
        private System.Windows.Forms.TextBox tbLastModification;
        private System.Windows.Forms.Label lblLastModification;
        private System.Windows.Forms.Button btnUsing;
        private System.Windows.Forms.Button btnDetailes;
        private System.Windows.Forms.CheckBox chbIncludePreviously;
        private System.Windows.Forms.GroupBox gbReport;
        private System.Windows.Forms.CheckBox chbIncludeDetails;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnSaveCriteria;
        private System.Windows.Forms.ComboBox cbFilter;
    }
}