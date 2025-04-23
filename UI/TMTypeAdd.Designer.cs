namespace UI
{
    partial class TMTypeAdd
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
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbDescAlternative = new System.Windows.Forms.TextBox();
            this.lblDescAlternative = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.lblPaymentCode = new System.Windows.Forms.Label();
            this.cbCompany = new System.Windows.Forms.ComboBox();
            this.lblCompany = new System.Windows.Forms.Label();
            this.gbManualInput = new System.Windows.Forms.GroupBox();
            this.rbManualInputNo = new System.Windows.Forms.RadioButton();
            this.rbManualInputYes = new System.Windows.Forms.RadioButton();
            this.gbMassiveInput = new System.Windows.Forms.GroupBox();
            this.rbMassiveInuptNo = new System.Windows.Forms.RadioButton();
            this.rbMassiveInputYes = new System.Windows.Forms.RadioButton();
            this.gbVerification = new System.Windows.Forms.GroupBox();
            this.rbVerificationNo = new System.Windows.Forms.RadioButton();
            this.rbVerificationYes = new System.Windows.Forms.RadioButton();
            this.rbAbsenceConfirmationNo = new System.Windows.Forms.RadioButton();
            this.rbAbsenceConfirmationYes = new System.Windows.Forms.RadioButton();
            this.gbLimits = new System.Windows.Forms.GroupBox();
            this.cbLimitOccasional = new System.Windows.Forms.ComboBox();
            this.lblLimitOccasional = new System.Windows.Forms.Label();
            this.cbLimitPerType = new System.Windows.Forms.ComboBox();
            this.lblLimitPerType = new System.Windows.Forms.Label();
            this.cbLimitComposite = new System.Windows.Forms.ComboBox();
            this.lblLimitComposite = new System.Windows.Forms.Label();
            this.gbUserCategory = new System.Windows.Forms.GroupBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lvTypeCategories = new System.Windows.Forms.ListView();
            this.lvUserCategory = new System.Windows.Forms.ListView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbPassTypeID = new System.Windows.Forms.TextBox();
            this.lblPassTypeID = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPayCode = new System.Windows.Forms.TextBox();
            this.tbSegmentColor = new System.Windows.Forms.TextBox();
            this.lblSegmentColor = new System.Windows.Forms.Label();
            this.gbAbsenceConfirmation = new System.Windows.Forms.GroupBox();
            this.btnRemoveConfirmType = new System.Windows.Forms.Button();
            this.btnAddConfirmType = new System.Windows.Forms.Button();
            this.lvTypesConfirmation = new System.Windows.Forms.ListView();
            this.lvTypes = new System.Windows.Forms.ListView();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.gbManualInput.SuspendLayout();
            this.gbMassiveInput.SuspendLayout();
            this.gbVerification.SuspendLayout();
            this.gbLimits.SuspendLayout();
            this.gbUserCategory.SuspendLayout();
            this.gbAbsenceConfirmation.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(114, 40);
            this.tbDesc.MaxLength = 50;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(248, 20);
            this.tbDesc.TabIndex = 1;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(45, 38);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(64, 23);
            this.lblDescription.TabIndex = 9;
            this.lblDescription.Text = "Description";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDescAlternative
            // 
            this.tbDescAlternative.Location = new System.Drawing.Point(114, 76);
            this.tbDescAlternative.MaxLength = 50;
            this.tbDescAlternative.Name = "tbDescAlternative";
            this.tbDescAlternative.Size = new System.Drawing.Size(248, 20);
            this.tbDescAlternative.TabIndex = 2;
            // 
            // lblDescAlternative
            // 
            this.lblDescAlternative.Location = new System.Drawing.Point(36, 64);
            this.lblDescAlternative.Name = "lblDescAlternative";
            this.lblDescAlternative.Size = new System.Drawing.Size(72, 43);
            this.lblDescAlternative.TabIndex = 13;
            this.lblDescAlternative.Text = "Desc alternative:";
            this.lblDescAlternative.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbType
            // 
            this.cbType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbType.Location = new System.Drawing.Point(114, 111);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(248, 21);
            this.cbType.TabIndex = 3;
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(20, 109);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(88, 23);
            this.lblType.TabIndex = 15;
            this.lblType.Text = "Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPaymentCode
            // 
            this.lblPaymentCode.Location = new System.Drawing.Point(380, 12);
            this.lblPaymentCode.Name = "lblPaymentCode";
            this.lblPaymentCode.Size = new System.Drawing.Size(88, 23);
            this.lblPaymentCode.TabIndex = 19;
            this.lblPaymentCode.Text = "Payment code:";
            this.lblPaymentCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbCompany
            // 
            this.cbCompany.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbCompany.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbCompany.Location = new System.Drawing.Point(473, 42);
            this.cbCompany.Name = "cbCompany";
            this.cbCompany.Size = new System.Drawing.Size(175, 21);
            this.cbCompany.TabIndex = 5;
            this.cbCompany.SelectedIndexChanged += new System.EventHandler(this.cbCompany_SelectedIndexChanged);
            // 
            // lblCompany
            // 
            this.lblCompany.Location = new System.Drawing.Point(379, 40);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(88, 23);
            this.lblCompany.TabIndex = 21;
            this.lblCompany.Text = "Company:";
            this.lblCompany.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbManualInput
            // 
            this.gbManualInput.Controls.Add(this.rbManualInputNo);
            this.gbManualInput.Controls.Add(this.rbManualInputYes);
            this.gbManualInput.Location = new System.Drawing.Point(23, 164);
            this.gbManualInput.Name = "gbManualInput";
            this.gbManualInput.Size = new System.Drawing.Size(110, 65);
            this.gbManualInput.TabIndex = 6;
            this.gbManualInput.TabStop = false;
            this.gbManualInput.Text = "Manual input";
            // 
            // rbManualInputNo
            // 
            this.rbManualInputNo.AutoSize = true;
            this.rbManualInputNo.Checked = true;
            this.rbManualInputNo.Location = new System.Drawing.Point(7, 43);
            this.rbManualInputNo.Name = "rbManualInputNo";
            this.rbManualInputNo.Size = new System.Drawing.Size(39, 17);
            this.rbManualInputNo.TabIndex = 1;
            this.rbManualInputNo.TabStop = true;
            this.rbManualInputNo.Text = "No";
            this.rbManualInputNo.UseVisualStyleBackColor = true;
            // 
            // rbManualInputYes
            // 
            this.rbManualInputYes.AutoSize = true;
            this.rbManualInputYes.Location = new System.Drawing.Point(7, 20);
            this.rbManualInputYes.Name = "rbManualInputYes";
            this.rbManualInputYes.Size = new System.Drawing.Size(43, 17);
            this.rbManualInputYes.TabIndex = 0;
            this.rbManualInputYes.Text = "Yes";
            this.rbManualInputYes.UseVisualStyleBackColor = true;
            // 
            // gbMassiveInput
            // 
            this.gbMassiveInput.Controls.Add(this.rbMassiveInuptNo);
            this.gbMassiveInput.Controls.Add(this.rbMassiveInputYes);
            this.gbMassiveInput.Location = new System.Drawing.Point(255, 164);
            this.gbMassiveInput.Name = "gbMassiveInput";
            this.gbMassiveInput.Size = new System.Drawing.Size(110, 65);
            this.gbMassiveInput.TabIndex = 8;
            this.gbMassiveInput.TabStop = false;
            this.gbMassiveInput.Text = "Massive input";
            // 
            // rbMassiveInuptNo
            // 
            this.rbMassiveInuptNo.AutoSize = true;
            this.rbMassiveInuptNo.Checked = true;
            this.rbMassiveInuptNo.Location = new System.Drawing.Point(7, 43);
            this.rbMassiveInuptNo.Name = "rbMassiveInuptNo";
            this.rbMassiveInuptNo.Size = new System.Drawing.Size(39, 17);
            this.rbMassiveInuptNo.TabIndex = 1;
            this.rbMassiveInuptNo.TabStop = true;
            this.rbMassiveInuptNo.Text = "No";
            this.rbMassiveInuptNo.UseVisualStyleBackColor = true;
            // 
            // rbMassiveInputYes
            // 
            this.rbMassiveInputYes.AutoSize = true;
            this.rbMassiveInputYes.Location = new System.Drawing.Point(7, 20);
            this.rbMassiveInputYes.Name = "rbMassiveInputYes";
            this.rbMassiveInputYes.Size = new System.Drawing.Size(43, 17);
            this.rbMassiveInputYes.TabIndex = 0;
            this.rbMassiveInputYes.Text = "Yes";
            this.rbMassiveInputYes.UseVisualStyleBackColor = true;
            // 
            // gbVerification
            // 
            this.gbVerification.Controls.Add(this.rbVerificationNo);
            this.gbVerification.Controls.Add(this.rbVerificationYes);
            this.gbVerification.Location = new System.Drawing.Point(139, 164);
            this.gbVerification.Name = "gbVerification";
            this.gbVerification.Size = new System.Drawing.Size(110, 65);
            this.gbVerification.TabIndex = 7;
            this.gbVerification.TabStop = false;
            this.gbVerification.Text = "Verification:";
            // 
            // rbVerificationNo
            // 
            this.rbVerificationNo.AutoSize = true;
            this.rbVerificationNo.Checked = true;
            this.rbVerificationNo.Location = new System.Drawing.Point(7, 43);
            this.rbVerificationNo.Name = "rbVerificationNo";
            this.rbVerificationNo.Size = new System.Drawing.Size(39, 17);
            this.rbVerificationNo.TabIndex = 1;
            this.rbVerificationNo.TabStop = true;
            this.rbVerificationNo.Text = "No";
            this.rbVerificationNo.UseVisualStyleBackColor = true;
            // 
            // rbVerificationYes
            // 
            this.rbVerificationYes.AutoSize = true;
            this.rbVerificationYes.Location = new System.Drawing.Point(7, 20);
            this.rbVerificationYes.Name = "rbVerificationYes";
            this.rbVerificationYes.Size = new System.Drawing.Size(43, 17);
            this.rbVerificationYes.TabIndex = 0;
            this.rbVerificationYes.Text = "Yes";
            this.rbVerificationYes.UseVisualStyleBackColor = true;
            // 
            // rbAbsenceConfirmationNo
            // 
            this.rbAbsenceConfirmationNo.AutoSize = true;
            this.rbAbsenceConfirmationNo.Checked = true;
            this.rbAbsenceConfirmationNo.Location = new System.Drawing.Point(109, 19);
            this.rbAbsenceConfirmationNo.Name = "rbAbsenceConfirmationNo";
            this.rbAbsenceConfirmationNo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rbAbsenceConfirmationNo.Size = new System.Drawing.Size(39, 17);
            this.rbAbsenceConfirmationNo.TabIndex = 1;
            this.rbAbsenceConfirmationNo.TabStop = true;
            this.rbAbsenceConfirmationNo.Text = "No";
            this.rbAbsenceConfirmationNo.UseVisualStyleBackColor = true;
            // 
            // rbAbsenceConfirmationYes
            // 
            this.rbAbsenceConfirmationYes.AutoSize = true;
            this.rbAbsenceConfirmationYes.Location = new System.Drawing.Point(12, 19);
            this.rbAbsenceConfirmationYes.Name = "rbAbsenceConfirmationYes";
            this.rbAbsenceConfirmationYes.Size = new System.Drawing.Size(43, 17);
            this.rbAbsenceConfirmationYes.TabIndex = 0;
            this.rbAbsenceConfirmationYes.Text = "Yes";
            this.rbAbsenceConfirmationYes.UseVisualStyleBackColor = true;
            this.rbAbsenceConfirmationYes.CheckedChanged += new System.EventHandler(this.rbAbsenceConfirmationYes_CheckedChanged);
            // 
            // gbLimits
            // 
            this.gbLimits.Controls.Add(this.cbLimitOccasional);
            this.gbLimits.Controls.Add(this.lblLimitOccasional);
            this.gbLimits.Controls.Add(this.cbLimitPerType);
            this.gbLimits.Controls.Add(this.lblLimitPerType);
            this.gbLimits.Controls.Add(this.cbLimitComposite);
            this.gbLimits.Controls.Add(this.lblLimitComposite);
            this.gbLimits.Location = new System.Drawing.Point(386, 101);
            this.gbLimits.Name = "gbLimits";
            this.gbLimits.Size = new System.Drawing.Size(262, 129);
            this.gbLimits.TabIndex = 11;
            this.gbLimits.TabStop = false;
            this.gbLimits.Text = "Limits";
            // 
            // cbLimitOccasional
            // 
            this.cbLimitOccasional.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLimitOccasional.Location = new System.Drawing.Point(97, 101);
            this.cbLimitOccasional.Name = "cbLimitOccasional";
            this.cbLimitOccasional.Size = new System.Drawing.Size(159, 21);
            this.cbLimitOccasional.TabIndex = 13;
            // 
            // lblLimitOccasional
            // 
            this.lblLimitOccasional.Location = new System.Drawing.Point(3, 99);
            this.lblLimitOccasional.Name = "lblLimitOccasional";
            this.lblLimitOccasional.Size = new System.Drawing.Size(88, 23);
            this.lblLimitOccasional.TabIndex = 12;
            this.lblLimitOccasional.Text = "Limit(occasional):";
            this.lblLimitOccasional.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbLimitPerType
            // 
            this.cbLimitPerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLimitPerType.Location = new System.Drawing.Point(97, 61);
            this.cbLimitPerType.Name = "cbLimitPerType";
            this.cbLimitPerType.Size = new System.Drawing.Size(159, 21);
            this.cbLimitPerType.TabIndex = 11;
            // 
            // lblLimitPerType
            // 
            this.lblLimitPerType.Location = new System.Drawing.Point(3, 59);
            this.lblLimitPerType.Name = "lblLimitPerType";
            this.lblLimitPerType.Size = new System.Drawing.Size(88, 23);
            this.lblLimitPerType.TabIndex = 10;
            this.lblLimitPerType.Text = "Limit(per type):";
            this.lblLimitPerType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbLimitComposite
            // 
            this.cbLimitComposite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLimitComposite.Location = new System.Drawing.Point(97, 19);
            this.cbLimitComposite.Name = "cbLimitComposite";
            this.cbLimitComposite.Size = new System.Drawing.Size(159, 21);
            this.cbLimitComposite.TabIndex = 9;
            // 
            // lblLimitComposite
            // 
            this.lblLimitComposite.Location = new System.Drawing.Point(3, 17);
            this.lblLimitComposite.Name = "lblLimitComposite";
            this.lblLimitComposite.Size = new System.Drawing.Size(88, 23);
            this.lblLimitComposite.TabIndex = 8;
            this.lblLimitComposite.Text = "Limit(composite):";
            this.lblLimitComposite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbUserCategory
            // 
            this.gbUserCategory.Controls.Add(this.btnRemove);
            this.gbUserCategory.Controls.Add(this.btnAdd);
            this.gbUserCategory.Controls.Add(this.lvTypeCategories);
            this.gbUserCategory.Controls.Add(this.lvUserCategory);
            this.gbUserCategory.Location = new System.Drawing.Point(23, 232);
            this.gbUserCategory.Name = "gbUserCategory";
            this.gbUserCategory.Size = new System.Drawing.Size(625, 217);
            this.gbUserCategory.TabIndex = 15;
            this.gbUserCategory.TabStop = false;
            this.gbUserCategory.Text = "User category";
            // 
            // btnRemove
            // 
            this.btnRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.Location = new System.Drawing.Point(293, 111);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(38, 31);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.Text = "<-";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(293, 74);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(38, 31);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "->";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvTypeCategories
            // 
            this.lvTypeCategories.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvTypeCategories.FullRowSelect = true;
            this.lvTypeCategories.GridLines = true;
            this.lvTypeCategories.HideSelection = false;
            this.lvTypeCategories.Location = new System.Drawing.Point(356, 19);
            this.lvTypeCategories.Name = "lvTypeCategories";
            this.lvTypeCategories.Size = new System.Drawing.Size(261, 192);
            this.lvTypeCategories.TabIndex = 41;
            this.lvTypeCategories.UseCompatibleStateImageBehavior = false;
            this.lvTypeCategories.View = System.Windows.Forms.View.Details;
            // 
            // lvUserCategory
            // 
            this.lvUserCategory.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvUserCategory.FullRowSelect = true;
            this.lvUserCategory.GridLines = true;
            this.lvUserCategory.HideSelection = false;
            this.lvUserCategory.Location = new System.Drawing.Point(6, 19);
            this.lvUserCategory.Name = "lvUserCategory";
            this.lvUserCategory.Size = new System.Drawing.Size(261, 192);
            this.lvUserCategory.TabIndex = 40;
            this.lvUserCategory.UseCompatibleStateImageBehavior = false;
            this.lvUserCategory.View = System.Windows.Forms.View.Details;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(573, 697);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(23, 696);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(72, 24);
            this.btnUpdate.TabIndex = 13;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(23, 697);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(364, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 37;
            this.label3.Text = "*";
            // 
            // tbPassTypeID
            // 
            this.tbPassTypeID.Location = new System.Drawing.Point(114, 12);
            this.tbPassTypeID.Name = "tbPassTypeID";
            this.tbPassTypeID.Size = new System.Drawing.Size(248, 20);
            this.tbPassTypeID.TabIndex = 0;
            // 
            // lblPassTypeID
            // 
            this.lblPassTypeID.Location = new System.Drawing.Point(12, 10);
            this.lblPassTypeID.Name = "lblPassTypeID";
            this.lblPassTypeID.Size = new System.Drawing.Size(88, 23);
            this.lblPassTypeID.TabIndex = 35;
            this.lblPassTypeID.Text = "Pass Type ID:";
            this.lblPassTypeID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(364, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 38;
            this.label1.Text = "*";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(364, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 16);
            this.label2.TabIndex = 44;
            this.label2.Text = "*";
            // 
            // tbPayCode
            // 
            this.tbPayCode.Location = new System.Drawing.Point(471, 11);
            this.tbPayCode.MaxLength = 4;
            this.tbPayCode.Name = "tbPayCode";
            this.tbPayCode.Size = new System.Drawing.Size(177, 20);
            this.tbPayCode.TabIndex = 4;
            // 
            // tbSegmentColor
            // 
            this.tbSegmentColor.Location = new System.Drawing.Point(473, 75);
            this.tbSegmentColor.Name = "tbSegmentColor";
            this.tbSegmentColor.Size = new System.Drawing.Size(175, 20);
            this.tbSegmentColor.TabIndex = 9;
            // 
            // lblSegmentColor
            // 
            this.lblSegmentColor.Location = new System.Drawing.Point(379, 74);
            this.lblSegmentColor.Name = "lblSegmentColor";
            this.lblSegmentColor.Size = new System.Drawing.Size(88, 23);
            this.lblSegmentColor.TabIndex = 46;
            this.lblSegmentColor.Text = "Segment color:";
            this.lblSegmentColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbAbsenceConfirmation
            // 
            this.gbAbsenceConfirmation.Controls.Add(this.btnRemoveConfirmType);
            this.gbAbsenceConfirmation.Controls.Add(this.btnAddConfirmType);
            this.gbAbsenceConfirmation.Controls.Add(this.rbAbsenceConfirmationNo);
            this.gbAbsenceConfirmation.Controls.Add(this.rbAbsenceConfirmationYes);
            this.gbAbsenceConfirmation.Controls.Add(this.lvTypesConfirmation);
            this.gbAbsenceConfirmation.Controls.Add(this.lvTypes);
            this.gbAbsenceConfirmation.Location = new System.Drawing.Point(20, 453);
            this.gbAbsenceConfirmation.Name = "gbAbsenceConfirmation";
            this.gbAbsenceConfirmation.Size = new System.Drawing.Size(625, 217);
            this.gbAbsenceConfirmation.TabIndex = 47;
            this.gbAbsenceConfirmation.TabStop = false;
            this.gbAbsenceConfirmation.Text = "Absence confirmation";
            // 
            // btnRemoveConfirmType
            // 
            this.btnRemoveConfirmType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveConfirmType.Location = new System.Drawing.Point(293, 111);
            this.btnRemoveConfirmType.Name = "btnRemoveConfirmType";
            this.btnRemoveConfirmType.Size = new System.Drawing.Size(38, 31);
            this.btnRemoveConfirmType.TabIndex = 1;
            this.btnRemoveConfirmType.Text = "<-";
            this.btnRemoveConfirmType.UseVisualStyleBackColor = true;
            this.btnRemoveConfirmType.Click += new System.EventHandler(this.btnRemoveConfirmType_Click);
            // 
            // btnAddConfirmType
            // 
            this.btnAddConfirmType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddConfirmType.Location = new System.Drawing.Point(293, 74);
            this.btnAddConfirmType.Name = "btnAddConfirmType";
            this.btnAddConfirmType.Size = new System.Drawing.Size(38, 31);
            this.btnAddConfirmType.TabIndex = 0;
            this.btnAddConfirmType.Text = "->";
            this.btnAddConfirmType.UseVisualStyleBackColor = true;
            this.btnAddConfirmType.Click += new System.EventHandler(this.btnAddConfirmType_Click);
            // 
            // lvTypesConfirmation
            // 
            this.lvTypesConfirmation.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvTypesConfirmation.FullRowSelect = true;
            this.lvTypesConfirmation.GridLines = true;
            this.lvTypesConfirmation.HideSelection = false;
            this.lvTypesConfirmation.Location = new System.Drawing.Point(356, 42);
            this.lvTypesConfirmation.Name = "lvTypesConfirmation";
            this.lvTypesConfirmation.Size = new System.Drawing.Size(261, 169);
            this.lvTypesConfirmation.TabIndex = 41;
            this.lvTypesConfirmation.UseCompatibleStateImageBehavior = false;
            this.lvTypesConfirmation.View = System.Windows.Forms.View.Details;
            // 
            // lvTypes
            // 
            this.lvTypes.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvTypes.FullRowSelect = true;
            this.lvTypes.GridLines = true;
            this.lvTypes.HideSelection = false;
            this.lvTypes.Location = new System.Drawing.Point(6, 42);
            this.lvTypes.Name = "lvTypes";
            this.lvTypes.Size = new System.Drawing.Size(261, 169);
            this.lvTypes.TabIndex = 40;
            this.lvTypes.UseCompatibleStateImageBehavior = false;
            this.lvTypes.View = System.Windows.Forms.View.Details;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(364, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 16);
            this.label4.TabIndex = 48;
            this.label4.Text = "*";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(650, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 16);
            this.label5.TabIndex = 49;
            this.label5.Text = "*";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(650, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 16);
            this.label6.TabIndex = 50;
            this.label6.Text = "*";
            // 
            // TMTypeAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 736);
            this.ControlBox = false;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.gbAbsenceConfirmation);
            this.Controls.Add(this.tbSegmentColor);
            this.Controls.Add(this.lblSegmentColor);
            this.Controls.Add(this.tbPayCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbPassTypeID);
            this.Controls.Add(this.lblPassTypeID);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbUserCategory);
            this.Controls.Add(this.gbLimits);
            this.Controls.Add(this.gbManualInput);
            this.Controls.Add(this.gbMassiveInput);
            this.Controls.Add(this.gbVerification);
            this.Controls.Add(this.cbCompany);
            this.Controls.Add(this.lblCompany);
            this.Controls.Add(this.lblPaymentCode);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.lblDescAlternative);
            this.Controls.Add(this.tbDescAlternative);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.lblDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "TMTypeAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TMTypeAdd";
            this.Load += new System.EventHandler(this.TMTypeAdd_Load);
            this.gbManualInput.ResumeLayout(false);
            this.gbManualInput.PerformLayout();
            this.gbMassiveInput.ResumeLayout(false);
            this.gbMassiveInput.PerformLayout();
            this.gbVerification.ResumeLayout(false);
            this.gbVerification.PerformLayout();
            this.gbLimits.ResumeLayout(false);
            this.gbUserCategory.ResumeLayout(false);
            this.gbAbsenceConfirmation.ResumeLayout(false);
            this.gbAbsenceConfirmation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDesc;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbDescAlternative;
        private System.Windows.Forms.Label lblDescAlternative;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblPaymentCode;
        private System.Windows.Forms.ComboBox cbCompany;
        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.GroupBox gbManualInput;
        private System.Windows.Forms.RadioButton rbManualInputNo;
        private System.Windows.Forms.RadioButton rbManualInputYes;
        private System.Windows.Forms.GroupBox gbMassiveInput;
        private System.Windows.Forms.RadioButton rbMassiveInuptNo;
        private System.Windows.Forms.RadioButton rbMassiveInputYes;
        private System.Windows.Forms.GroupBox gbVerification;
        private System.Windows.Forms.RadioButton rbVerificationNo;
        private System.Windows.Forms.RadioButton rbVerificationYes;
        private System.Windows.Forms.RadioButton rbAbsenceConfirmationNo;
        private System.Windows.Forms.RadioButton rbAbsenceConfirmationYes;
        private System.Windows.Forms.GroupBox gbLimits;
        private System.Windows.Forms.ComboBox cbLimitOccasional;
        private System.Windows.Forms.Label lblLimitOccasional;
        private System.Windows.Forms.ComboBox cbLimitPerType;
        private System.Windows.Forms.Label lblLimitPerType;
        private System.Windows.Forms.ComboBox cbLimitComposite;
        private System.Windows.Forms.Label lblLimitComposite;
        private System.Windows.Forms.GroupBox gbUserCategory;
        private System.Windows.Forms.ListView lvTypeCategories;
        private System.Windows.Forms.ListView lvUserCategory;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbPassTypeID;
        private System.Windows.Forms.Label lblPassTypeID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPayCode;
        private System.Windows.Forms.TextBox tbSegmentColor;
        private System.Windows.Forms.Label lblSegmentColor;
        private System.Windows.Forms.GroupBox gbAbsenceConfirmation;
        private System.Windows.Forms.Button btnRemoveConfirmType;
        private System.Windows.Forms.Button btnAddConfirmType;
        private System.Windows.Forms.ListView lvTypesConfirmation;
        private System.Windows.Forms.ListView lvTypes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}