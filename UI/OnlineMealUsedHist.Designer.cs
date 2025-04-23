namespace UI
{
    partial class OnlineMealUsedHist
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OnlineMealUsedHist));
            this.gbUnitFilter = new System.Windows.Forms.GroupBox();
            this.btnOUTree = new System.Windows.Forms.Button();
            this.cbOU = new System.Windows.Forms.ComboBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.rbOU = new System.Windows.Forms.RadioButton();
            this.rbWU = new System.Windows.Forms.RadioButton();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.cbRestaurant = new System.Windows.Forms.ComboBox();
            this.lblRestaurant = new System.Windows.Forms.Label();
            this.cbLine = new System.Windows.Forms.ComboBox();
            this.lblLine = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbMealType = new System.Windows.Forms.ComboBox();
            this.lblMealType = new System.Windows.Forms.Label();
            this.gbHist = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numQtyTo = new System.Windows.Forms.NumericUpDown();
            this.numQtyFrom = new System.Windows.Forms.NumericUpDown();
            this.lblQty = new System.Windows.Forms.Label();
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
            this.gbValidationAuto = new System.Windows.Forms.GroupBox();
            this.lblReasonAuto = new System.Windows.Forms.Label();
            this.lvValidationReasonAuto = new System.Windows.Forms.ListView();
            this.cbNOK = new System.Windows.Forms.CheckBox();
            this.cbOK = new System.Windows.Forms.CheckBox();
            this.gbValidation = new System.Windows.Forms.GroupBox();
            this.lblReason = new System.Windows.Forms.Label();
            this.lvReason = new System.Windows.Forms.ListView();
            this.cbRed = new System.Windows.Forms.CheckBox();
            this.cbGreen = new System.Windows.Forms.CheckBox();
            this.gbEnterWay = new System.Windows.Forms.GroupBox();
            this.cbReader = new System.Windows.Forms.CheckBox();
            this.cbManual = new System.Windows.Forms.CheckBox();
            this.cbEventTime = new System.Windows.Forms.CheckBox();
            this.cbModifiedTime = new System.Windows.Forms.CheckBox();
            this.gbModifiedTime = new System.Windows.Forms.GroupBox();
            this.dtpModifiedFrom = new System.Windows.Forms.DateTimePicker();
            this.lblModifiedFrom = new System.Windows.Forms.Label();
            this.dtpModifiedTo = new System.Windows.Forms.DateTimePicker();
            this.lblModifiedTo = new System.Windows.Forms.Label();
            this.gbEventTime = new System.Windows.Forms.GroupBox();
            this.tbTimeTo = new System.Windows.Forms.TextBox();
            this.tbTimeFrom = new System.Windows.Forms.TextBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lvMealsUsed = new System.Windows.Forms.ListView();
            this.gbHistoryOfChange = new System.Windows.Forms.GroupBox();
            this.gbCurrentMeal = new System.Windows.Forms.GroupBox();
            this.lvCurrentMeal = new System.Windows.Forms.ListView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.gbUnitFilter.SuspendLayout();
            this.gbHist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyFrom)).BeginInit();
            this.gbApproval.SuspendLayout();
            this.gbValidationAuto.SuspendLayout();
            this.gbValidation.SuspendLayout();
            this.gbEnterWay.SuspendLayout();
            this.gbModifiedTime.SuspendLayout();
            this.gbEventTime.SuspendLayout();
            this.gbHistoryOfChange.SuspendLayout();
            this.gbCurrentMeal.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbUnitFilter
            // 
            this.gbUnitFilter.Controls.Add(this.btnOUTree);
            this.gbUnitFilter.Controls.Add(this.cbOU);
            this.gbUnitFilter.Controls.Add(this.btnWUTree);
            this.gbUnitFilter.Controls.Add(this.cbWU);
            this.gbUnitFilter.Controls.Add(this.rbOU);
            this.gbUnitFilter.Controls.Add(this.rbWU);
            this.gbUnitFilter.Location = new System.Drawing.Point(12, 19);
            this.gbUnitFilter.Name = "gbUnitFilter";
            this.gbUnitFilter.Size = new System.Drawing.Size(534, 87);
            this.gbUnitFilter.TabIndex = 2;
            this.gbUnitFilter.TabStop = false;
            // 
            // btnOUTree
            // 
            this.btnOUTree.Enabled = false;
            this.btnOUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnOUTree.Image")));
            this.btnOUTree.Location = new System.Drawing.Point(437, 48);
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
            this.cbOU.Location = new System.Drawing.Point(198, 51);
            this.cbOU.Name = "cbOU";
            this.cbOU.Size = new System.Drawing.Size(217, 21);
            this.cbOU.TabIndex = 4;
            this.cbOU.SelectedIndexChanged += new System.EventHandler(this.cbOU_SelectedIndexChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(437, 19);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 2;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(198, 18);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(217, 21);
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
            this.rbOU.CheckedChanged += new System.EventHandler(this.rbOU_CheckedChanged);
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
            this.rbWU.CheckedChanged += new System.EventHandler(this.rbWU_CheckedChanged);
            // 
            // lblEmployee
            // 
            this.lblEmployee.AutoSize = true;
            this.lblEmployee.Location = new System.Drawing.Point(15, 131);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(56, 13);
            this.lblEmployee.TabIndex = 3;
            this.lblEmployee.Text = "Employee:";
            // 
            // cbEmployee
            // 
            this.cbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmployee.FormattingEnabled = true;
            this.cbEmployee.Location = new System.Drawing.Point(88, 131);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(180, 21);
            this.cbEmployee.TabIndex = 4;
            // 
            // cbRestaurant
            // 
            this.cbRestaurant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRestaurant.FormattingEnabled = true;
            this.cbRestaurant.Location = new System.Drawing.Point(361, 131);
            this.cbRestaurant.Name = "cbRestaurant";
            this.cbRestaurant.Size = new System.Drawing.Size(185, 21);
            this.cbRestaurant.TabIndex = 6;
            this.cbRestaurant.SelectedIndexChanged += new System.EventHandler(this.cbRestaurant_SelectedIndexChanged);
            // 
            // lblRestaurant
            // 
            this.lblRestaurant.AutoSize = true;
            this.lblRestaurant.Location = new System.Drawing.Point(290, 131);
            this.lblRestaurant.Name = "lblRestaurant";
            this.lblRestaurant.Size = new System.Drawing.Size(65, 13);
            this.lblRestaurant.TabIndex = 5;
            this.lblRestaurant.Text = "Restaurant: ";
            // 
            // cbLine
            // 
            this.cbLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLine.FormattingEnabled = true;
            this.cbLine.Location = new System.Drawing.Point(361, 163);
            this.cbLine.Name = "cbLine";
            this.cbLine.Size = new System.Drawing.Size(185, 21);
            this.cbLine.TabIndex = 8;
            // 
            // lblLine
            // 
            this.lblLine.AutoSize = true;
            this.lblLine.Location = new System.Drawing.Point(290, 171);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(33, 13);
            this.lblLine.TabIndex = 7;
            this.lblLine.Text = "Line: ";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(921, 755);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cbMealType
            // 
            this.cbMealType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMealType.FormattingEnabled = true;
            this.cbMealType.Location = new System.Drawing.Point(88, 163);
            this.cbMealType.Name = "cbMealType";
            this.cbMealType.Size = new System.Drawing.Size(180, 21);
            this.cbMealType.TabIndex = 11;
            // 
            // lblMealType
            // 
            this.lblMealType.AutoSize = true;
            this.lblMealType.Location = new System.Drawing.Point(15, 163);
            this.lblMealType.Name = "lblMealType";
            this.lblMealType.Size = new System.Drawing.Size(56, 13);
            this.lblMealType.TabIndex = 10;
            this.lblMealType.Text = "Meal type:";
            // 
            // gbHist
            // 
            this.gbHist.Controls.Add(this.label1);
            this.gbHist.Controls.Add(this.numQtyTo);
            this.gbHist.Controls.Add(this.numQtyFrom);
            this.gbHist.Controls.Add(this.lblQty);
            this.gbHist.Controls.Add(this.gbApproval);
            this.gbHist.Controls.Add(this.gbValidationAuto);
            this.gbHist.Controls.Add(this.gbValidation);
            this.gbHist.Controls.Add(this.gbEnterWay);
            this.gbHist.Controls.Add(this.cbEventTime);
            this.gbHist.Controls.Add(this.cbModifiedTime);
            this.gbHist.Controls.Add(this.gbModifiedTime);
            this.gbHist.Controls.Add(this.gbEventTime);
            this.gbHist.Controls.Add(this.btnSearch);
            this.gbHist.Controls.Add(this.gbUnitFilter);
            this.gbHist.Controls.Add(this.cbMealType);
            this.gbHist.Controls.Add(this.cbLine);
            this.gbHist.Controls.Add(this.cbEmployee);
            this.gbHist.Controls.Add(this.lblLine);
            this.gbHist.Controls.Add(this.lblMealType);
            this.gbHist.Controls.Add(this.cbRestaurant);
            this.gbHist.Controls.Add(this.lblEmployee);
            this.gbHist.Controls.Add(this.lblRestaurant);
            this.gbHist.Location = new System.Drawing.Point(8, 7);
            this.gbHist.Name = "gbHist";
            this.gbHist.Size = new System.Drawing.Size(985, 408);
            this.gbHist.TabIndex = 12;
            this.gbHist.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(158, 222);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 36;
            this.label1.Text = "-";
            // 
            // numQtyTo
            // 
            this.numQtyTo.Location = new System.Drawing.Point(170, 219);
            this.numQtyTo.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numQtyTo.Name = "numQtyTo";
            this.numQtyTo.Size = new System.Drawing.Size(65, 20);
            this.numQtyTo.TabIndex = 37;
            // 
            // numQtyFrom
            // 
            this.numQtyFrom.Location = new System.Drawing.Point(90, 219);
            this.numQtyFrom.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numQtyFrom.Name = "numQtyFrom";
            this.numQtyFrom.Size = new System.Drawing.Size(65, 20);
            this.numQtyFrom.TabIndex = 35;
            // 
            // lblQty
            // 
            this.lblQty.AutoSize = true;
            this.lblQty.Location = new System.Drawing.Point(17, 219);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(46, 13);
            this.lblQty.TabIndex = 34;
            this.lblQty.Text = "Quantity";
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
            this.gbApproval.Location = new System.Drawing.Point(745, 22);
            this.gbApproval.Name = "gbApproval";
            this.gbApproval.Size = new System.Drawing.Size(223, 349);
            this.gbApproval.TabIndex = 29;
            this.gbApproval.TabStop = false;
            this.gbApproval.Text = "Approval";
            // 
            // chbApproval
            // 
            this.chbApproval.AutoSize = true;
            this.chbApproval.Location = new System.Drawing.Point(9, 23);
            this.chbApproval.Name = "chbApproval";
            this.chbApproval.Size = new System.Drawing.Size(68, 17);
            this.chbApproval.TabIndex = 0;
            this.chbApproval.Text = "Approval";
            this.chbApproval.UseVisualStyleBackColor = true;
            this.chbApproval.CheckedChanged += new System.EventHandler(this.chbApproval_CheckedChanged);
            // 
            // lblToApproval
            // 
            this.lblToApproval.Location = new System.Drawing.Point(5, 321);
            this.lblToApproval.Name = "lblToApproval";
            this.lblToApproval.Size = new System.Drawing.Size(31, 13);
            this.lblToApproval.TabIndex = 7;
            this.lblToApproval.Text = "To:";
            this.lblToApproval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFromApproval
            // 
            this.lblFromApproval.Location = new System.Drawing.Point(6, 297);
            this.lblFromApproval.Name = "lblFromApproval";
            this.lblFromApproval.Size = new System.Drawing.Size(33, 13);
            this.lblFromApproval.TabIndex = 5;
            this.lblFromApproval.Text = "From:";
            this.lblFromApproval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpToApproval
            // 
            this.dtpToApproval.CustomFormat = "dd.MM.yyyy";
            this.dtpToApproval.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToApproval.Location = new System.Drawing.Point(90, 321);
            this.dtpToApproval.Name = "dtpToApproval";
            this.dtpToApproval.Size = new System.Drawing.Size(124, 20);
            this.dtpToApproval.TabIndex = 8;
            // 
            // dtpFromApproval
            // 
            this.dtpFromApproval.CustomFormat = "dd.MM.yyyy";
            this.dtpFromApproval.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromApproval.Location = new System.Drawing.Point(90, 295);
            this.dtpFromApproval.Name = "dtpFromApproval";
            this.dtpFromApproval.Size = new System.Drawing.Size(124, 20);
            this.dtpFromApproval.TabIndex = 6;
            // 
            // lvOperater
            // 
            this.lvOperater.GridLines = true;
            this.lvOperater.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvOperater.HideSelection = false;
            this.lvOperater.Location = new System.Drawing.Point(6, 147);
            this.lvOperater.Name = "lvOperater";
            this.lvOperater.Size = new System.Drawing.Size(208, 127);
            this.lvOperater.TabIndex = 4;
            this.lvOperater.UseCompatibleStateImageBehavior = false;
            this.lvOperater.View = System.Windows.Forms.View.Details;
            // 
            // lblOperater
            // 
            this.lblOperater.AutoSize = true;
            this.lblOperater.Location = new System.Drawing.Point(5, 131);
            this.lblOperater.Name = "lblOperater";
            this.lblOperater.Size = new System.Drawing.Size(48, 13);
            this.lblOperater.TabIndex = 3;
            this.lblOperater.Text = "Operater";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(6, 44);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Status";
            // 
            // lvStatus
            // 
            this.lvStatus.GridLines = true;
            this.lvStatus.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvStatus.HideSelection = false;
            this.lvStatus.Location = new System.Drawing.Point(6, 60);
            this.lvStatus.Name = "lvStatus";
            this.lvStatus.Size = new System.Drawing.Size(208, 68);
            this.lvStatus.TabIndex = 2;
            this.lvStatus.UseCompatibleStateImageBehavior = false;
            this.lvStatus.View = System.Windows.Forms.View.Details;
            // 
            // gbValidationAuto
            // 
            this.gbValidationAuto.Controls.Add(this.lblReasonAuto);
            this.gbValidationAuto.Controls.Add(this.lvValidationReasonAuto);
            this.gbValidationAuto.Controls.Add(this.cbNOK);
            this.gbValidationAuto.Controls.Add(this.cbOK);
            this.gbValidationAuto.Location = new System.Drawing.Point(561, 198);
            this.gbValidationAuto.Name = "gbValidationAuto";
            this.gbValidationAuto.Size = new System.Drawing.Size(178, 173);
            this.gbValidationAuto.TabIndex = 28;
            this.gbValidationAuto.TabStop = false;
            this.gbValidationAuto.Text = "Validation - auto";
            // 
            // lblReasonAuto
            // 
            this.lblReasonAuto.AutoSize = true;
            this.lblReasonAuto.Location = new System.Drawing.Point(3, 59);
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
            this.lvValidationReasonAuto.Location = new System.Drawing.Point(6, 75);
            this.lvValidationReasonAuto.Name = "lvValidationReasonAuto";
            this.lvValidationReasonAuto.Size = new System.Drawing.Size(161, 80);
            this.lvValidationReasonAuto.TabIndex = 3;
            this.lvValidationReasonAuto.UseCompatibleStateImageBehavior = false;
            this.lvValidationReasonAuto.View = System.Windows.Forms.View.Details;
            // 
            // cbNOK
            // 
            this.cbNOK.AutoSize = true;
            this.cbNOK.Location = new System.Drawing.Point(86, 21);
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
            this.cbOK.Location = new System.Drawing.Point(6, 21);
            this.cbOK.Name = "cbOK";
            this.cbOK.Size = new System.Drawing.Size(41, 17);
            this.cbOK.TabIndex = 0;
            this.cbOK.Text = "OK";
            this.cbOK.UseVisualStyleBackColor = true;
            // 
            // gbValidation
            // 
            this.gbValidation.Controls.Add(this.lblReason);
            this.gbValidation.Controls.Add(this.lvReason);
            this.gbValidation.Controls.Add(this.cbRed);
            this.gbValidation.Controls.Add(this.cbGreen);
            this.gbValidation.Location = new System.Drawing.Point(561, 19);
            this.gbValidation.Name = "gbValidation";
            this.gbValidation.Size = new System.Drawing.Size(178, 173);
            this.gbValidation.TabIndex = 27;
            this.gbValidation.TabStop = false;
            this.gbValidation.Text = "Validation";
            // 
            // lblReason
            // 
            this.lblReason.AutoSize = true;
            this.lblReason.Location = new System.Drawing.Point(6, 63);
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
            this.lvReason.Location = new System.Drawing.Point(9, 79);
            this.lvReason.Name = "lvReason";
            this.lvReason.Size = new System.Drawing.Size(161, 84);
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
            // gbEnterWay
            // 
            this.gbEnterWay.Controls.Add(this.cbReader);
            this.gbEnterWay.Controls.Add(this.cbManual);
            this.gbEnterWay.Location = new System.Drawing.Point(307, 206);
            this.gbEnterWay.Name = "gbEnterWay";
            this.gbEnterWay.Size = new System.Drawing.Size(139, 52);
            this.gbEnterWay.TabIndex = 26;
            this.gbEnterWay.TabStop = false;
            this.gbEnterWay.Text = "Enter";
            // 
            // cbReader
            // 
            this.cbReader.AutoSize = true;
            this.cbReader.Location = new System.Drawing.Point(72, 21);
            this.cbReader.Name = "cbReader";
            this.cbReader.Size = new System.Drawing.Size(61, 17);
            this.cbReader.TabIndex = 1;
            this.cbReader.Text = "Reader";
            this.cbReader.UseVisualStyleBackColor = true;
            // 
            // cbManual
            // 
            this.cbManual.AutoSize = true;
            this.cbManual.Location = new System.Drawing.Point(6, 21);
            this.cbManual.Name = "cbManual";
            this.cbManual.Size = new System.Drawing.Size(61, 17);
            this.cbManual.TabIndex = 0;
            this.cbManual.Text = "Manual";
            this.cbManual.UseVisualStyleBackColor = true;
            // 
            // cbEventTime
            // 
            this.cbEventTime.AutoSize = true;
            this.cbEventTime.Checked = true;
            this.cbEventTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEventTime.Location = new System.Drawing.Point(20, 277);
            this.cbEventTime.Name = "cbEventTime";
            this.cbEventTime.Size = new System.Drawing.Size(15, 14);
            this.cbEventTime.TabIndex = 25;
            this.cbEventTime.UseVisualStyleBackColor = true;
            this.cbEventTime.CheckedChanged += new System.EventHandler(this.cbEventTime_Changed);
            // 
            // cbModifiedTime
            // 
            this.cbModifiedTime.AutoSize = true;
            this.cbModifiedTime.Location = new System.Drawing.Point(302, 277);
            this.cbModifiedTime.Name = "cbModifiedTime";
            this.cbModifiedTime.Size = new System.Drawing.Size(15, 14);
            this.cbModifiedTime.TabIndex = 24;
            this.cbModifiedTime.UseVisualStyleBackColor = true;
            this.cbModifiedTime.CheckedChanged += new System.EventHandler(this.cbModifiedTime_Changed);
            // 
            // gbModifiedTime
            // 
            this.gbModifiedTime.Controls.Add(this.dtpModifiedFrom);
            this.gbModifiedTime.Controls.Add(this.lblModifiedFrom);
            this.gbModifiedTime.Controls.Add(this.dtpModifiedTo);
            this.gbModifiedTime.Controls.Add(this.lblModifiedTo);
            this.gbModifiedTime.Enabled = false;
            this.gbModifiedTime.Location = new System.Drawing.Point(320, 277);
            this.gbModifiedTime.Name = "gbModifiedTime";
            this.gbModifiedTime.Size = new System.Drawing.Size(226, 94);
            this.gbModifiedTime.TabIndex = 23;
            this.gbModifiedTime.TabStop = false;
            this.gbModifiedTime.Text = "Modified time";
            // 
            // dtpModifiedFrom
            // 
            this.dtpModifiedFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpModifiedFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpModifiedFrom.Location = new System.Drawing.Point(69, 16);
            this.dtpModifiedFrom.Name = "dtpModifiedFrom";
            this.dtpModifiedFrom.Size = new System.Drawing.Size(129, 20);
            this.dtpModifiedFrom.TabIndex = 11;
            // 
            // lblModifiedFrom
            // 
            this.lblModifiedFrom.Location = new System.Drawing.Point(14, 16);
            this.lblModifiedFrom.Name = "lblModifiedFrom";
            this.lblModifiedFrom.Size = new System.Drawing.Size(49, 23);
            this.lblModifiedFrom.TabIndex = 10;
            this.lblModifiedFrom.Text = "From:";
            this.lblModifiedFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpModifiedTo
            // 
            this.dtpModifiedTo.CustomFormat = "dd.MM.yyyy";
            this.dtpModifiedTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpModifiedTo.Location = new System.Drawing.Point(69, 56);
            this.dtpModifiedTo.Name = "dtpModifiedTo";
            this.dtpModifiedTo.Size = new System.Drawing.Size(129, 20);
            this.dtpModifiedTo.TabIndex = 13;
            // 
            // lblModifiedTo
            // 
            this.lblModifiedTo.Location = new System.Drawing.Point(17, 56);
            this.lblModifiedTo.Name = "lblModifiedTo";
            this.lblModifiedTo.Size = new System.Drawing.Size(46, 23);
            this.lblModifiedTo.TabIndex = 12;
            this.lblModifiedTo.Text = "To:";
            this.lblModifiedTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbEventTime
            // 
            this.gbEventTime.Controls.Add(this.tbTimeTo);
            this.gbEventTime.Controls.Add(this.tbTimeFrom);
            this.gbEventTime.Controls.Add(this.dtpFrom);
            this.gbEventTime.Controls.Add(this.lblFrom);
            this.gbEventTime.Controls.Add(this.dtpTo);
            this.gbEventTime.Controls.Add(this.lblTo);
            this.gbEventTime.Location = new System.Drawing.Point(41, 277);
            this.gbEventTime.Name = "gbEventTime";
            this.gbEventTime.Size = new System.Drawing.Size(227, 94);
            this.gbEventTime.TabIndex = 22;
            this.gbEventTime.TabStop = false;
            this.gbEventTime.Text = "Event time";
            // 
            // tbTimeTo
            // 
            this.tbTimeTo.Location = new System.Drawing.Point(169, 56);
            this.tbTimeTo.Name = "tbTimeTo";
            this.tbTimeTo.Size = new System.Drawing.Size(44, 20);
            this.tbTimeTo.TabIndex = 19;
            // 
            // tbTimeFrom
            // 
            this.tbTimeFrom.Location = new System.Drawing.Point(169, 20);
            this.tbTimeFrom.Name = "tbTimeFrom";
            this.tbTimeFrom.Size = new System.Drawing.Size(44, 20);
            this.tbTimeFrom.TabIndex = 18;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(61, 20);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(102, 20);
            this.dtpFrom.TabIndex = 11;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(6, 17);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(49, 23);
            this.lblFrom.TabIndex = 10;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(61, 56);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(102, 20);
            this.dtpTo.TabIndex = 13;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(6, 56);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(46, 23);
            this.lblTo.TabIndex = 12;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(893, 377);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lvMealsUsed
            // 
            this.lvMealsUsed.FullRowSelect = true;
            this.lvMealsUsed.GridLines = true;
            this.lvMealsUsed.HideSelection = false;
            this.lvMealsUsed.Location = new System.Drawing.Point(7, 19);
            this.lvMealsUsed.MultiSelect = false;
            this.lvMealsUsed.Name = "lvMealsUsed";
            this.lvMealsUsed.Size = new System.Drawing.Size(961, 171);
            this.lvMealsUsed.TabIndex = 25;
            this.lvMealsUsed.UseCompatibleStateImageBehavior = false;
            this.lvMealsUsed.View = System.Windows.Forms.View.Details;
            this.lvMealsUsed.SelectedIndexChanged += new System.EventHandler(this.lvMealsUsed_SelectedIndexChanged);
            this.lvMealsUsed.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvMealsUsed_ColumnClick);
            // 
            // gbHistoryOfChange
            // 
            this.gbHistoryOfChange.Controls.Add(this.lvMealsUsed);
            this.gbHistoryOfChange.Location = new System.Drawing.Point(8, 422);
            this.gbHistoryOfChange.Name = "gbHistoryOfChange";
            this.gbHistoryOfChange.Size = new System.Drawing.Size(985, 205);
            this.gbHistoryOfChange.TabIndex = 26;
            this.gbHistoryOfChange.TabStop = false;
            this.gbHistoryOfChange.Text = "History of change";
            // 
            // gbCurrentMeal
            // 
            this.gbCurrentMeal.Controls.Add(this.lvCurrentMeal);
            this.gbCurrentMeal.Location = new System.Drawing.Point(7, 655);
            this.gbCurrentMeal.Name = "gbCurrentMeal";
            this.gbCurrentMeal.Size = new System.Drawing.Size(986, 94);
            this.gbCurrentMeal.TabIndex = 27;
            this.gbCurrentMeal.TabStop = false;
            this.gbCurrentMeal.Text = "Current meal";
            // 
            // lvCurrentMeal
            // 
            this.lvCurrentMeal.FullRowSelect = true;
            this.lvCurrentMeal.GridLines = true;
            this.lvCurrentMeal.HideSelection = false;
            this.lvCurrentMeal.Location = new System.Drawing.Point(10, 19);
            this.lvCurrentMeal.Name = "lvCurrentMeal";
            this.lvCurrentMeal.Size = new System.Drawing.Size(959, 69);
            this.lvCurrentMeal.TabIndex = 26;
            this.lvCurrentMeal.UseCompatibleStateImageBehavior = false;
            this.lvCurrentMeal.View = System.Windows.Forms.View.Details;
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(840, 629);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(156, 23);
            this.lblTotal.TabIndex = 30;
            this.lblTotal.Text = "TOTAL:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OnlineMealUsedHist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 782);
            this.ControlBox = false;
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.gbCurrentMeal);
            this.Controls.Add(this.gbHistoryOfChange);
            this.Controls.Add(this.gbHist);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "OnlineMealUsedHist";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OnlineMealUsedHist";
            this.Load += new System.EventHandler(this.OnlineMealUsedHist_Load);
            this.gbUnitFilter.ResumeLayout(false);
            this.gbUnitFilter.PerformLayout();
            this.gbHist.ResumeLayout(false);
            this.gbHist.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyFrom)).EndInit();
            this.gbApproval.ResumeLayout(false);
            this.gbApproval.PerformLayout();
            this.gbValidationAuto.ResumeLayout(false);
            this.gbValidationAuto.PerformLayout();
            this.gbValidation.ResumeLayout(false);
            this.gbValidation.PerformLayout();
            this.gbEnterWay.ResumeLayout(false);
            this.gbEnterWay.PerformLayout();
            this.gbModifiedTime.ResumeLayout(false);
            this.gbEventTime.ResumeLayout(false);
            this.gbEventTime.PerformLayout();
            this.gbHistoryOfChange.ResumeLayout(false);
            this.gbCurrentMeal.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbUnitFilter;
        private System.Windows.Forms.Button btnOUTree;
        private System.Windows.Forms.ComboBox cbOU;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.RadioButton rbOU;
        private System.Windows.Forms.RadioButton rbWU;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.ComboBox cbRestaurant;
        private System.Windows.Forms.Label lblRestaurant;
        private System.Windows.Forms.ComboBox cbLine;
        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbMealType;
        private System.Windows.Forms.Label lblMealType;
        private System.Windows.Forms.GroupBox gbHist;
        private System.Windows.Forms.ListView lvMealsUsed;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.CheckBox cbEventTime;
        private System.Windows.Forms.CheckBox cbModifiedTime;
        private System.Windows.Forms.GroupBox gbModifiedTime;
        private System.Windows.Forms.DateTimePicker dtpModifiedFrom;
        private System.Windows.Forms.Label lblModifiedFrom;
        private System.Windows.Forms.DateTimePicker dtpModifiedTo;
        private System.Windows.Forms.Label lblModifiedTo;
        private System.Windows.Forms.GroupBox gbEventTime;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.GroupBox gbHistoryOfChange;
        private System.Windows.Forms.GroupBox gbCurrentMeal;
        private System.Windows.Forms.ListView lvCurrentMeal;
        private System.Windows.Forms.GroupBox gbEnterWay;
        private System.Windows.Forms.CheckBox cbReader;
        private System.Windows.Forms.CheckBox cbManual;
        private System.Windows.Forms.GroupBox gbValidationAuto;
        private System.Windows.Forms.Label lblReasonAuto;
        private System.Windows.Forms.ListView lvValidationReasonAuto;
        private System.Windows.Forms.CheckBox cbNOK;
        private System.Windows.Forms.CheckBox cbOK;
        private System.Windows.Forms.GroupBox gbValidation;
        private System.Windows.Forms.Label lblReason;
        private System.Windows.Forms.ListView lvReason;
        private System.Windows.Forms.CheckBox cbRed;
        private System.Windows.Forms.CheckBox cbGreen;
        private System.Windows.Forms.GroupBox gbApproval;
        private System.Windows.Forms.CheckBox chbApproval;
        private System.Windows.Forms.Label lblToApproval;
        private System.Windows.Forms.Label lblFromApproval;
        private System.Windows.Forms.DateTimePicker dtpToApproval;
        private System.Windows.Forms.DateTimePicker dtpFromApproval;
        private System.Windows.Forms.ListView lvOperater;
        private System.Windows.Forms.Label lblOperater;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ListView lvStatus;
        private System.Windows.Forms.TextBox tbTimeTo;
        private System.Windows.Forms.TextBox tbTimeFrom;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numQtyTo;
        private System.Windows.Forms.NumericUpDown numQtyFrom;
        private System.Windows.Forms.Label lblQty;
    }
}