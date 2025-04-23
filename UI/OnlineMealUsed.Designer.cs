namespace UI
{
    partial class OnlineMealUsed
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OnlineMealUsed));
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnOUTree = new System.Windows.Forms.Button();
            this.cbOU = new System.Windows.Forms.ComboBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.rbOU = new System.Windows.Forms.RadioButton();
            this.rbWU = new System.Windows.Forms.RadioButton();
            this.lblRestaurant = new System.Windows.Forms.Label();
            this.lvRestaurant = new System.Windows.Forms.ListView();
            this.lblLine = new System.Windows.Forms.Label();
            this.lvLine = new System.Windows.Forms.ListView();
            this.lvMealType = new System.Windows.Forms.ListView();
            this.lblMealType = new System.Windows.Forms.Label();
            this.lblQty = new System.Windows.Forms.Label();
            this.numQtyTo = new System.Windows.Forms.NumericUpDown();
            this.numQtyFrom = new System.Windows.Forms.NumericUpDown();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.gbEnterWay = new System.Windows.Forms.GroupBox();
            this.cbReader = new System.Windows.Forms.CheckBox();
            this.cbManual = new System.Windows.Forms.CheckBox();
            this.gbEmployee = new System.Windows.Forms.GroupBox();
            this.cbSelectAllEmpl = new System.Windows.Forms.CheckBox();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.tbEmployee = new System.Windows.Forms.TextBox();
            this.gbValidation = new System.Windows.Forms.GroupBox();
            this.lblReason = new System.Windows.Forms.Label();
            this.lvReason = new System.Windows.Forms.ListView();
            this.cbRed = new System.Windows.Forms.CheckBox();
            this.cbGreen = new System.Windows.Forms.CheckBox();
            this.gbValidationAuto = new System.Windows.Forms.GroupBox();
            this.lblReasonAuto = new System.Windows.Forms.Label();
            this.lvValidationReasonAuto = new System.Windows.Forms.ListView();
            this.cbNOK = new System.Windows.Forms.CheckBox();
            this.cbOK = new System.Windows.Forms.CheckBox();
            this.gbApproval = new System.Windows.Forms.GroupBox();
            this.chbApproval = new System.Windows.Forms.CheckBox();
            this.lblToApproval = new System.Windows.Forms.Label();
            this.lblFromApproval = new System.Windows.Forms.Label();
            this.dtpToApproval = new System.Windows.Forms.DateTimePicker();
            this.dtpFromApproval = new System.Windows.Forms.DateTimePicker();
            this.lvOperater = new System.Windows.Forms.ListView();
            this.lblOperater = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lvStatus = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lvMealsUsed = new System.Windows.Forms.ListView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnApprove = new System.Windows.Forms.Button();
            this.gbExport = new System.Windows.Forms.GroupBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.rbAnalitical = new System.Windows.Forms.RadioButton();
            this.rbSummary = new System.Windows.Forms.RadioButton();
            this.btnHistory = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.gbApprove = new System.Windows.Forms.GroupBox();
            this.btnBusinessTrip = new System.Windows.Forms.Button();
            this.btnNotApprove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.gbAutoCheck = new System.Windows.Forms.GroupBox();
            this.btnValidate = new System.Windows.Forms.Button();
            this.cbCompany = new System.Windows.Forms.ComboBox();
            this.lblCompany = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFromCheck = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.lblToCheck = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.gbMassiveInput = new System.Windows.Forms.GroupBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.gbFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyFrom)).BeginInit();
            this.gbEnterWay.SuspendLayout();
            this.gbEmployee.SuspendLayout();
            this.gbValidation.SuspendLayout();
            this.gbValidationAuto.SuspendLayout();
            this.gbApproval.SuspendLayout();
            this.gbExport.SuspendLayout();
            this.gbApprove.SuspendLayout();
            this.gbAutoCheck.SuspendLayout();
            this.gbMassiveInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnOUTree);
            this.gbFilter.Controls.Add(this.cbOU);
            this.gbFilter.Controls.Add(this.btnWUTree);
            this.gbFilter.Controls.Add(this.cbWU);
            this.gbFilter.Controls.Add(this.rbOU);
            this.gbFilter.Controls.Add(this.rbWU);
            this.gbFilter.Location = new System.Drawing.Point(13, 9);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(417, 87);
            this.gbFilter.TabIndex = 0;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Filter";
            // 
            // btnOUTree
            // 
            this.btnOUTree.Enabled = false;
            this.btnOUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnOUTree.Image")));
            this.btnOUTree.Location = new System.Drawing.Point(370, 50);
            this.btnOUTree.Name = "btnOUTree";
            this.btnOUTree.Size = new System.Drawing.Size(25, 23);
            this.btnOUTree.TabIndex = 5;
            this.btnOUTree.Click += new System.EventHandler(this.btnOUTree_Click);
            // 
            // cbOU
            // 
            this.cbOU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOU.Enabled = false;
            this.cbOU.FormattingEnabled = true;
            this.cbOU.Location = new System.Drawing.Point(172, 50);
            this.cbOU.Name = "cbOU";
            this.cbOU.Size = new System.Drawing.Size(192, 21);
            this.cbOU.TabIndex = 4;
            this.cbOU.SelectedIndexChanged += new System.EventHandler(this.cbOU_SelectedIndexChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(370, 15);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 2;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(172, 17);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(192, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // rbOU
            // 
            this.rbOU.AutoSize = true;
            this.rbOU.Location = new System.Drawing.Point(6, 51);
            this.rbOU.Name = "rbOU";
            this.rbOU.Size = new System.Drawing.Size(117, 17);
            this.rbOU.TabIndex = 3;
            this.rbOU.Text = "Organizational units";
            this.rbOU.UseVisualStyleBackColor = true;
            this.rbOU.Click += new System.EventHandler(this.rbOU_CheckedChanged);
            // 
            // rbWU
            // 
            this.rbWU.AutoSize = true;
            this.rbWU.Checked = true;
            this.rbWU.Location = new System.Drawing.Point(6, 19);
            this.rbWU.Name = "rbWU";
            this.rbWU.Size = new System.Drawing.Size(90, 17);
            this.rbWU.TabIndex = 0;
            this.rbWU.TabStop = true;
            this.rbWU.Text = "Working units";
            this.rbWU.UseVisualStyleBackColor = true;
            this.rbWU.Click += new System.EventHandler(this.rbWU_CheckedChanged);
            // 
            // lblRestaurant
            // 
            this.lblRestaurant.AutoSize = true;
            this.lblRestaurant.Location = new System.Drawing.Point(10, 104);
            this.lblRestaurant.Name = "lblRestaurant";
            this.lblRestaurant.Size = new System.Drawing.Size(59, 13);
            this.lblRestaurant.TabIndex = 1;
            this.lblRestaurant.Text = "Restaurant";
            // 
            // lvRestaurant
            // 
            this.lvRestaurant.GridLines = true;
            this.lvRestaurant.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvRestaurant.HideSelection = false;
            this.lvRestaurant.Location = new System.Drawing.Point(75, 107);
            this.lvRestaurant.Name = "lvRestaurant";
            this.lvRestaurant.Size = new System.Drawing.Size(141, 80);
            this.lvRestaurant.TabIndex = 2;
            this.lvRestaurant.UseCompatibleStateImageBehavior = false;
            this.lvRestaurant.View = System.Windows.Forms.View.Details;
            // 
            // lblLine
            // 
            this.lblLine.AutoSize = true;
            this.lblLine.Location = new System.Drawing.Point(10, 195);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(27, 13);
            this.lblLine.TabIndex = 3;
            this.lblLine.Text = "Line";
            // 
            // lvLine
            // 
            this.lvLine.GridLines = true;
            this.lvLine.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvLine.HideSelection = false;
            this.lvLine.Location = new System.Drawing.Point(75, 193);
            this.lvLine.Name = "lvLine";
            this.lvLine.Size = new System.Drawing.Size(141, 89);
            this.lvLine.TabIndex = 4;
            this.lvLine.UseCompatibleStateImageBehavior = false;
            this.lvLine.View = System.Windows.Forms.View.Details;
            // 
            // lvMealType
            // 
            this.lvMealType.GridLines = true;
            this.lvMealType.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvMealType.HideSelection = false;
            this.lvMealType.Location = new System.Drawing.Point(75, 288);
            this.lvMealType.Name = "lvMealType";
            this.lvMealType.Size = new System.Drawing.Size(141, 59);
            this.lvMealType.TabIndex = 6;
            this.lvMealType.UseCompatibleStateImageBehavior = false;
            this.lvMealType.View = System.Windows.Forms.View.Details;
            // 
            // lblMealType
            // 
            this.lblMealType.AutoSize = true;
            this.lblMealType.Location = new System.Drawing.Point(3, 289);
            this.lblMealType.Name = "lblMealType";
            this.lblMealType.Size = new System.Drawing.Size(53, 13);
            this.lblMealType.TabIndex = 5;
            this.lblMealType.Text = "Meal type";
            // 
            // lblQty
            // 
            this.lblQty.AutoSize = true;
            this.lblQty.Location = new System.Drawing.Point(222, 107);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(46, 13);
            this.lblQty.TabIndex = 7;
            this.lblQty.Text = "Quantity";
            // 
            // numQtyTo
            // 
            this.numQtyTo.Location = new System.Drawing.Point(354, 107);
            this.numQtyTo.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numQtyTo.Name = "numQtyTo";
            this.numQtyTo.Size = new System.Drawing.Size(65, 20);
            this.numQtyTo.TabIndex = 10;
            // 
            // numQtyFrom
            // 
            this.numQtyFrom.Location = new System.Drawing.Point(274, 107);
            this.numQtyFrom.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numQtyFrom.Name = "numQtyFrom";
            this.numQtyFrom.Size = new System.Drawing.Size(65, 20);
            this.numQtyFrom.TabIndex = 8;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(228, 173);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(40, 13);
            this.lblTo.TabIndex = 14;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(228, 147);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 13);
            this.lblFrom.TabIndex = 11;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(274, 171);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(126, 20);
            this.dtpTo.TabIndex = 15;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(274, 145);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(126, 20);
            this.dtpFrom.TabIndex = 12;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // gbEnterWay
            // 
            this.gbEnterWay.Controls.Add(this.cbReader);
            this.gbEnterWay.Controls.Add(this.cbManual);
            this.gbEnterWay.Location = new System.Drawing.Point(238, 213);
            this.gbEnterWay.Name = "gbEnterWay";
            this.gbEnterWay.Size = new System.Drawing.Size(185, 69);
            this.gbEnterWay.TabIndex = 17;
            this.gbEnterWay.TabStop = false;
            this.gbEnterWay.Text = "Enter";
            // 
            // cbReader
            // 
            this.cbReader.AutoSize = true;
            this.cbReader.Location = new System.Drawing.Point(90, 28);
            this.cbReader.Name = "cbReader";
            this.cbReader.Size = new System.Drawing.Size(61, 17);
            this.cbReader.TabIndex = 1;
            this.cbReader.Text = "Reader";
            this.cbReader.UseVisualStyleBackColor = true;
            // 
            // cbManual
            // 
            this.cbManual.AutoSize = true;
            this.cbManual.Location = new System.Drawing.Point(6, 28);
            this.cbManual.Name = "cbManual";
            this.cbManual.Size = new System.Drawing.Size(61, 17);
            this.cbManual.TabIndex = 0;
            this.cbManual.Text = "Manual";
            this.cbManual.UseVisualStyleBackColor = true;
            // 
            // gbEmployee
            // 
            this.gbEmployee.Controls.Add(this.cbSelectAllEmpl);
            this.gbEmployee.Controls.Add(this.lvEmployees);
            this.gbEmployee.Controls.Add(this.tbEmployee);
            this.gbEmployee.Location = new System.Drawing.Point(436, 11);
            this.gbEmployee.Name = "gbEmployee";
            this.gbEmployee.Size = new System.Drawing.Size(200, 327);
            this.gbEmployee.TabIndex = 18;
            this.gbEmployee.TabStop = false;
            this.gbEmployee.Text = "Employee";
            // 
            // cbSelectAllEmpl
            // 
            this.cbSelectAllEmpl.AutoSize = true;
            this.cbSelectAllEmpl.Location = new System.Drawing.Point(8, 300);
            this.cbSelectAllEmpl.Name = "cbSelectAllEmpl";
            this.cbSelectAllEmpl.Size = new System.Drawing.Size(69, 17);
            this.cbSelectAllEmpl.TabIndex = 2;
            this.cbSelectAllEmpl.Text = "Select all";
            this.cbSelectAllEmpl.UseVisualStyleBackColor = true;
            this.cbSelectAllEmpl.CheckedChanged += new System.EventHandler(this.cbSelectAllEmpl_CheckedChanged);
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(8, 47);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.ShowItemToolTips = true;
            this.lvEmployees.Size = new System.Drawing.Size(186, 245);
            this.lvEmployees.TabIndex = 1;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            // 
            // tbEmployee
            // 
            this.tbEmployee.Location = new System.Drawing.Point(8, 21);
            this.tbEmployee.Name = "tbEmployee";
            this.tbEmployee.Size = new System.Drawing.Size(175, 20);
            this.tbEmployee.TabIndex = 0;
            this.tbEmployee.TextChanged += new System.EventHandler(this.tbEmployee_TextChanged);
            // 
            // gbValidation
            // 
            this.gbValidation.Controls.Add(this.lblReason);
            this.gbValidation.Controls.Add(this.lvReason);
            this.gbValidation.Controls.Add(this.cbRed);
            this.gbValidation.Controls.Add(this.cbGreen);
            this.gbValidation.Location = new System.Drawing.Point(642, 11);
            this.gbValidation.Name = "gbValidation";
            this.gbValidation.Size = new System.Drawing.Size(178, 149);
            this.gbValidation.TabIndex = 19;
            this.gbValidation.TabStop = false;
            this.gbValidation.Text = "Validation";
            // 
            // lblReason
            // 
            this.lblReason.AutoSize = true;
            this.lblReason.Location = new System.Drawing.Point(6, 46);
            this.lblReason.Name = "lblReason";
            this.lblReason.Size = new System.Drawing.Size(44, 13);
            this.lblReason.TabIndex = 2;
            this.lblReason.Text = "Reason";
            // 
            // lvReason
            // 
            this.lvReason.Enabled = false;
            this.lvReason.GridLines = true;
            this.lvReason.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvReason.HideSelection = false;
            this.lvReason.Location = new System.Drawing.Point(6, 63);
            this.lvReason.Name = "lvReason";
            this.lvReason.Size = new System.Drawing.Size(161, 80);
            this.lvReason.TabIndex = 3;
            this.lvReason.UseCompatibleStateImageBehavior = false;
            this.lvReason.View = System.Windows.Forms.View.Details;
            // 
            // cbRed
            // 
            this.cbRed.AutoSize = true;
            this.cbRed.Location = new System.Drawing.Point(86, 21);
            this.cbRed.Name = "cbRed";
            this.cbRed.Size = new System.Drawing.Size(46, 17);
            this.cbRed.TabIndex = 1;
            this.cbRed.Text = "Red";
            this.cbRed.UseVisualStyleBackColor = true;
            this.cbRed.CheckedChanged += new System.EventHandler(this.cbRed_CheckedChanged);
            // 
            // cbGreen
            // 
            this.cbGreen.AutoSize = true;
            this.cbGreen.Location = new System.Drawing.Point(6, 21);
            this.cbGreen.Name = "cbGreen";
            this.cbGreen.Size = new System.Drawing.Size(55, 17);
            this.cbGreen.TabIndex = 0;
            this.cbGreen.Text = "Green";
            this.cbGreen.UseVisualStyleBackColor = true;
            // 
            // gbValidationAuto
            // 
            this.gbValidationAuto.Controls.Add(this.lblReasonAuto);
            this.gbValidationAuto.Controls.Add(this.lvValidationReasonAuto);
            this.gbValidationAuto.Controls.Add(this.cbNOK);
            this.gbValidationAuto.Controls.Add(this.cbOK);
            this.gbValidationAuto.Location = new System.Drawing.Point(642, 166);
            this.gbValidationAuto.Name = "gbValidationAuto";
            this.gbValidationAuto.Size = new System.Drawing.Size(178, 157);
            this.gbValidationAuto.TabIndex = 20;
            this.gbValidationAuto.TabStop = false;
            this.gbValidationAuto.Text = "Validation - auto";
            // 
            // lblReasonAuto
            // 
            this.lblReasonAuto.AutoSize = true;
            this.lblReasonAuto.Location = new System.Drawing.Point(6, 55);
            this.lblReasonAuto.Name = "lblReasonAuto";
            this.lblReasonAuto.Size = new System.Drawing.Size(44, 13);
            this.lblReasonAuto.TabIndex = 2;
            this.lblReasonAuto.Text = "Reason";
            // 
            // lvValidationReasonAuto
            // 
            this.lvValidationReasonAuto.Enabled = false;
            this.lvValidationReasonAuto.GridLines = true;
            this.lvValidationReasonAuto.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvValidationReasonAuto.HideSelection = false;
            this.lvValidationReasonAuto.Location = new System.Drawing.Point(6, 71);
            this.lvValidationReasonAuto.Name = "lvValidationReasonAuto";
            this.lvValidationReasonAuto.Size = new System.Drawing.Size(161, 80);
            this.lvValidationReasonAuto.TabIndex = 3;
            this.lvValidationReasonAuto.UseCompatibleStateImageBehavior = false;
            this.lvValidationReasonAuto.View = System.Windows.Forms.View.Details;
            // 
            // cbNOK
            // 
            this.cbNOK.AutoSize = true;
            this.cbNOK.Location = new System.Drawing.Point(86, 26);
            this.cbNOK.Name = "cbNOK";
            this.cbNOK.Size = new System.Drawing.Size(49, 17);
            this.cbNOK.TabIndex = 1;
            this.cbNOK.Text = "NOK";
            this.cbNOK.UseVisualStyleBackColor = true;
            this.cbNOK.CheckedChanged += new System.EventHandler(this.cbNOK_CheckedChanged);
            // 
            // cbOK
            // 
            this.cbOK.AutoSize = true;
            this.cbOK.Location = new System.Drawing.Point(6, 26);
            this.cbOK.Name = "cbOK";
            this.cbOK.Size = new System.Drawing.Size(41, 17);
            this.cbOK.TabIndex = 0;
            this.cbOK.Text = "OK";
            this.cbOK.UseVisualStyleBackColor = true;
            // 
            // gbApproval
            // 
            this.gbApproval.Controls.Add(this.chbApproval);
            this.gbApproval.Controls.Add(this.lblToApproval);
            this.gbApproval.Controls.Add(this.lblFromApproval);
            this.gbApproval.Controls.Add(this.dtpToApproval);
            this.gbApproval.Controls.Add(this.dtpFromApproval);
            this.gbApproval.Controls.Add(this.lvOperater);
            this.gbApproval.Controls.Add(this.lblOperater);
            this.gbApproval.Controls.Add(this.lblStatus);
            this.gbApproval.Controls.Add(this.lvStatus);
            this.gbApproval.Location = new System.Drawing.Point(826, 10);
            this.gbApproval.Name = "gbApproval";
            this.gbApproval.Size = new System.Drawing.Size(198, 328);
            this.gbApproval.TabIndex = 21;
            this.gbApproval.TabStop = false;
            this.gbApproval.Text = "Approval";
            // 
            // chbApproval
            // 
            this.chbApproval.AutoSize = true;
            this.chbApproval.Location = new System.Drawing.Point(6, 19);
            this.chbApproval.Name = "chbApproval";
            this.chbApproval.Size = new System.Drawing.Size(68, 17);
            this.chbApproval.TabIndex = 0;
            this.chbApproval.Text = "Approval";
            this.chbApproval.UseVisualStyleBackColor = true;
            this.chbApproval.CheckedChanged += new System.EventHandler(this.chbApproval_CheckedChanged);
            // 
            // lblToApproval
            // 
            this.lblToApproval.Location = new System.Drawing.Point(31, 302);
            this.lblToApproval.Name = "lblToApproval";
            this.lblToApproval.Size = new System.Drawing.Size(31, 13);
            this.lblToApproval.TabIndex = 7;
            this.lblToApproval.Text = "To:";
            this.lblToApproval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFromApproval
            // 
            this.lblFromApproval.Location = new System.Drawing.Point(29, 276);
            this.lblFromApproval.Name = "lblFromApproval";
            this.lblFromApproval.Size = new System.Drawing.Size(33, 13);
            this.lblFromApproval.TabIndex = 5;
            this.lblFromApproval.Text = "From:";
            this.lblFromApproval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpToApproval
            // 
            this.dtpToApproval.CustomFormat = "dd.MM.yyyy";
            this.dtpToApproval.Enabled = false;
            this.dtpToApproval.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToApproval.Location = new System.Drawing.Point(68, 301);
            this.dtpToApproval.Name = "dtpToApproval";
            this.dtpToApproval.Size = new System.Drawing.Size(124, 20);
            this.dtpToApproval.TabIndex = 8;
            // 
            // dtpFromApproval
            // 
            this.dtpFromApproval.CustomFormat = "dd.MM.yyyy";
            this.dtpFromApproval.Enabled = false;
            this.dtpFromApproval.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromApproval.Location = new System.Drawing.Point(68, 273);
            this.dtpFromApproval.Name = "dtpFromApproval";
            this.dtpFromApproval.Size = new System.Drawing.Size(124, 20);
            this.dtpFromApproval.TabIndex = 6;
            // 
            // lvOperater
            // 
            this.lvOperater.Enabled = false;
            this.lvOperater.GridLines = true;
            this.lvOperater.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvOperater.HideSelection = false;
            this.lvOperater.Location = new System.Drawing.Point(6, 153);
            this.lvOperater.Name = "lvOperater";
            this.lvOperater.Size = new System.Drawing.Size(186, 113);
            this.lvOperater.TabIndex = 4;
            this.lvOperater.UseCompatibleStateImageBehavior = false;
            this.lvOperater.View = System.Windows.Forms.View.Details;
            // 
            // lblOperater
            // 
            this.lblOperater.AutoSize = true;
            this.lblOperater.Location = new System.Drawing.Point(5, 137);
            this.lblOperater.Name = "lblOperater";
            this.lblOperater.Size = new System.Drawing.Size(48, 13);
            this.lblOperater.TabIndex = 3;
            this.lblOperater.Text = "Operater";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(6, 39);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Status";
            // 
            // lvStatus
            // 
            this.lvStatus.Enabled = false;
            this.lvStatus.GridLines = true;
            this.lvStatus.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvStatus.HideSelection = false;
            this.lvStatus.Location = new System.Drawing.Point(6, 55);
            this.lvStatus.Name = "lvStatus";
            this.lvStatus.Size = new System.Drawing.Size(186, 79);
            this.lvStatus.TabIndex = 2;
            this.lvStatus.UseCompatibleStateImageBehavior = false;
            this.lvStatus.View = System.Windows.Forms.View.Details;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 634);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(82, 33);
            this.btnAdd.TabIndex = 28;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvMealsUsed
            // 
            this.lvMealsUsed.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvMealsUsed.FullRowSelect = true;
            this.lvMealsUsed.GridLines = true;
            this.lvMealsUsed.HideSelection = false;
            this.lvMealsUsed.HoverSelection = true;
            this.lvMealsUsed.Location = new System.Drawing.Point(6, 398);
            this.lvMealsUsed.Name = "lvMealsUsed";
            this.lvMealsUsed.Size = new System.Drawing.Size(1018, 192);
            this.lvMealsUsed.TabIndex = 26;
            this.lvMealsUsed.UseCompatibleStateImageBehavior = false;
            this.lvMealsUsed.View = System.Windows.Forms.View.Details;
            this.lvMealsUsed.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvMealsUsed_ColumnClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(21, 52);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 32);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(21, 14);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(100, 32);
            this.btnApprove.TabIndex = 0;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // gbExport
            // 
            this.gbExport.Controls.Add(this.btnExport);
            this.gbExport.Controls.Add(this.rbAnalitical);
            this.gbExport.Controls.Add(this.rbSummary);
            this.gbExport.Location = new System.Drawing.Point(746, 616);
            this.gbExport.Name = "gbExport";
            this.gbExport.Size = new System.Drawing.Size(192, 58);
            this.gbExport.TabIndex = 31;
            this.gbExport.TabStop = false;
            this.gbExport.Text = "Export";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(97, 18);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 33);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // rbAnalitical
            // 
            this.rbAnalitical.AutoSize = true;
            this.rbAnalitical.Location = new System.Drawing.Point(13, 34);
            this.rbAnalitical.Name = "rbAnalitical";
            this.rbAnalitical.Size = new System.Drawing.Size(67, 17);
            this.rbAnalitical.TabIndex = 1;
            this.rbAnalitical.TabStop = true;
            this.rbAnalitical.Text = "Analitical";
            this.rbAnalitical.UseVisualStyleBackColor = true;
            // 
            // rbSummary
            // 
            this.rbSummary.AutoSize = true;
            this.rbSummary.Checked = true;
            this.rbSummary.Location = new System.Drawing.Point(13, 16);
            this.rbSummary.Name = "rbSummary";
            this.rbSummary.Size = new System.Drawing.Size(68, 17);
            this.rbSummary.TabIndex = 0;
            this.rbSummary.TabStop = true;
            this.rbSummary.Text = "Summary";
            this.rbSummary.UseVisualStyleBackColor = true;
            // 
            // btnHistory
            // 
            this.btnHistory.Location = new System.Drawing.Point(949, 634);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(75, 33);
            this.btnHistory.TabIndex = 32;
            this.btnHistory.Text = "History";
            this.btnHistory.UseVisualStyleBackColor = true;
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(945, 344);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 23;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // gbApprove
            // 
            this.gbApprove.Controls.Add(this.btnBusinessTrip);
            this.gbApprove.Controls.Add(this.btnNotApprove);
            this.gbApprove.Controls.Add(this.btnDelete);
            this.gbApprove.Controls.Add(this.btnApprove);
            this.gbApprove.Location = new System.Drawing.Point(113, 593);
            this.gbApprove.Name = "gbApprove";
            this.gbApprove.Size = new System.Drawing.Size(276, 93);
            this.gbApprove.TabIndex = 29;
            this.gbApprove.TabStop = false;
            this.gbApprove.Text = "Approve";
            // 
            // btnBusinessTrip
            // 
            this.btnBusinessTrip.Location = new System.Drawing.Point(151, 52);
            this.btnBusinessTrip.Name = "btnBusinessTrip";
            this.btnBusinessTrip.Size = new System.Drawing.Size(100, 32);
            this.btnBusinessTrip.TabIndex = 3;
            this.btnBusinessTrip.Text = "Business trip";
            this.btnBusinessTrip.UseVisualStyleBackColor = true;
            this.btnBusinessTrip.Click += new System.EventHandler(this.btnBusinessTrip_Click);
            // 
            // btnNotApprove
            // 
            this.btnNotApprove.Location = new System.Drawing.Point(151, 14);
            this.btnNotApprove.Name = "btnNotApprove";
            this.btnNotApprove.Size = new System.Drawing.Size(100, 32);
            this.btnNotApprove.TabIndex = 1;
            this.btnNotApprove.Text = "Not approve";
            this.btnNotApprove.UseVisualStyleBackColor = true;
            this.btnNotApprove.Click += new System.EventHandler(this.btnNotApprove_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(342, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "-";
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(868, 593);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(156, 23);
            this.lblTotal.TabIndex = 27;
            this.lblTotal.Text = "TOTAL:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbAutoCheck
            // 
            this.gbAutoCheck.Controls.Add(this.btnValidate);
            this.gbAutoCheck.Controls.Add(this.cbCompany);
            this.gbAutoCheck.Controls.Add(this.lblCompany);
            this.gbAutoCheck.Controls.Add(this.dtFrom);
            this.gbAutoCheck.Controls.Add(this.lblFromCheck);
            this.gbAutoCheck.Controls.Add(this.dtTo);
            this.gbAutoCheck.Controls.Add(this.lblToCheck);
            this.gbAutoCheck.Location = new System.Drawing.Point(395, 593);
            this.gbAutoCheck.Name = "gbAutoCheck";
            this.gbAutoCheck.Size = new System.Drawing.Size(345, 93);
            this.gbAutoCheck.TabIndex = 30;
            this.gbAutoCheck.TabStop = false;
            this.gbAutoCheck.Text = "Auto check";
            // 
            // btnValidate
            // 
            this.btnValidate.Enabled = false;
            this.btnValidate.Location = new System.Drawing.Point(224, 53);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(75, 22);
            this.btnValidate.TabIndex = 6;
            this.btnValidate.Text = "Validate";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // cbCompany
            // 
            this.cbCompany.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbCompany.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbCompany.FormattingEnabled = true;
            this.cbCompany.Location = new System.Drawing.Point(90, 54);
            this.cbCompany.Name = "cbCompany";
            this.cbCompany.Size = new System.Drawing.Size(117, 21);
            this.cbCompany.TabIndex = 5;
            // 
            // lblCompany
            // 
            this.lblCompany.Location = new System.Drawing.Point(9, 53);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(75, 23);
            this.lblCompany.TabIndex = 4;
            this.lblCompany.Text = "Company:";
            this.lblCompany.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(48, 19);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(128, 20);
            this.dtFrom.TabIndex = 1;
            // 
            // lblFromCheck
            // 
            this.lblFromCheck.Location = new System.Drawing.Point(2, 16);
            this.lblFromCheck.Name = "lblFromCheck";
            this.lblFromCheck.Size = new System.Drawing.Size(40, 23);
            this.lblFromCheck.TabIndex = 0;
            this.lblFromCheck.Text = "From:";
            this.lblFromCheck.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(213, 18);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(124, 20);
            this.dtTo.TabIndex = 3;
            // 
            // lblToCheck
            // 
            this.lblToCheck.Location = new System.Drawing.Point(182, 16);
            this.lblToCheck.Name = "lblToCheck";
            this.lblToCheck.Size = new System.Drawing.Size(25, 23);
            this.lblToCheck.TabIndex = 2;
            this.lblToCheck.Text = "To:";
            this.lblToCheck.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(917, 372);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(44, 23);
            this.btnPrev.TabIndex = 24;
            this.btnPrev.Text = "<";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(976, 372);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(44, 23);
            this.btnNext.TabIndex = 25;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // gbMassiveInput
            // 
            this.gbMassiveInput.Controls.Add(this.lblPath);
            this.gbMassiveInput.Controls.Add(this.btnBrowse);
            this.gbMassiveInput.Controls.Add(this.tbPath);
            this.gbMassiveInput.Location = new System.Drawing.Point(434, 341);
            this.gbMassiveInput.Name = "gbMassiveInput";
            this.gbMassiveInput.Size = new System.Drawing.Size(469, 51);
            this.gbMassiveInput.TabIndex = 22;
            this.gbMassiveInput.TabStop = false;
            this.gbMassiveInput.Text = "Massive input";
            // 
            // lblPath
            // 
            this.lblPath.Location = new System.Drawing.Point(10, 23);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(51, 13);
            this.lblPath.TabIndex = 0;
            this.lblPath.Text = "Path:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(384, 19);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbPath
            // 
            this.tbPath.Enabled = false;
            this.tbPath.Location = new System.Drawing.Point(67, 20);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(308, 20);
            this.tbPath.TabIndex = 1;
            // 
            // OnlineMealUsed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbMassiveInput);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.gbAutoCheck);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbApprove);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnHistory);
            this.Controls.Add(this.gbExport);
            this.Controls.Add(this.lvMealsUsed);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.gbApproval);
            this.Controls.Add(this.gbValidationAuto);
            this.Controls.Add(this.gbValidation);
            this.Controls.Add(this.gbEmployee);
            this.Controls.Add(this.gbEnterWay);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.numQtyTo);
            this.Controls.Add(this.numQtyFrom);
            this.Controls.Add(this.lblQty);
            this.Controls.Add(this.lvMealType);
            this.Controls.Add(this.lblMealType);
            this.Controls.Add(this.lvLine);
            this.Controls.Add(this.lblLine);
            this.Controls.Add(this.lvRestaurant);
            this.Controls.Add(this.lblRestaurant);
            this.Controls.Add(this.gbFilter);
            this.Name = "OnlineMealUsed";
            this.Size = new System.Drawing.Size(1033, 689);
            this.Load += new System.EventHandler(this.OnlineMealUsed_Load);
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyFrom)).EndInit();
            this.gbEnterWay.ResumeLayout(false);
            this.gbEnterWay.PerformLayout();
            this.gbEmployee.ResumeLayout(false);
            this.gbEmployee.PerformLayout();
            this.gbValidation.ResumeLayout(false);
            this.gbValidation.PerformLayout();
            this.gbValidationAuto.ResumeLayout(false);
            this.gbValidationAuto.PerformLayout();
            this.gbApproval.ResumeLayout(false);
            this.gbApproval.PerformLayout();
            this.gbExport.ResumeLayout(false);
            this.gbExport.PerformLayout();
            this.gbApprove.ResumeLayout(false);
            this.gbAutoCheck.ResumeLayout(false);
            this.gbMassiveInput.ResumeLayout(false);
            this.gbMassiveInput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnOUTree;
        private System.Windows.Forms.ComboBox cbOU;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.RadioButton rbOU;
        private System.Windows.Forms.RadioButton rbWU;
        private System.Windows.Forms.Label lblRestaurant;
        private System.Windows.Forms.ListView lvRestaurant;
        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.ListView lvLine;
        private System.Windows.Forms.ListView lvMealType;
        private System.Windows.Forms.Label lblMealType;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.NumericUpDown numQtyTo;
        private System.Windows.Forms.NumericUpDown numQtyFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.GroupBox gbEnterWay;
        private System.Windows.Forms.CheckBox cbManual;
        private System.Windows.Forms.CheckBox cbReader;
        private System.Windows.Forms.GroupBox gbEmployee;
        private System.Windows.Forms.TextBox tbEmployee;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.CheckBox cbSelectAllEmpl;
        private System.Windows.Forms.GroupBox gbValidation;
        private System.Windows.Forms.CheckBox cbRed;
        private System.Windows.Forms.CheckBox cbGreen;
        private System.Windows.Forms.ListView lvReason;
        private System.Windows.Forms.Label lblReason;
        private System.Windows.Forms.GroupBox gbValidationAuto;
        private System.Windows.Forms.Label lblReasonAuto;
        private System.Windows.Forms.ListView lvValidationReasonAuto;
        private System.Windows.Forms.CheckBox cbNOK;
        private System.Windows.Forms.CheckBox cbOK;
        private System.Windows.Forms.GroupBox gbApproval;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListView lvStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblOperater;
        private System.Windows.Forms.ListView lvOperater;
        private System.Windows.Forms.Label lblToApproval;
        private System.Windows.Forms.Label lblFromApproval;
        private System.Windows.Forms.DateTimePicker dtpToApproval;
        private System.Windows.Forms.DateTimePicker dtpFromApproval;
        private System.Windows.Forms.ListView lvMealsUsed;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.GroupBox gbExport;
        private System.Windows.Forms.RadioButton rbAnalitical;
        private System.Windows.Forms.RadioButton rbSummary;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnHistory;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox gbApprove;
        private System.Windows.Forms.Button btnNotApprove;
        private System.Windows.Forms.CheckBox chbApproval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.GroupBox gbAutoCheck;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.Label lblFromCheck;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Label lblToCheck;
        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.ComboBox cbCompany;
        private System.Windows.Forms.Button btnBusinessTrip;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.GroupBox gbMassiveInput;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbPath;


    }
}
