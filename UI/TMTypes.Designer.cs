namespace UI
{
    partial class TMTypes
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
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lvPassTypes = new System.Windows.Forms.ListView();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.gbManualInput = new System.Windows.Forms.GroupBox();
            this.rbManualInputNo = new System.Windows.Forms.CheckBox();
            this.rbManualInputYes = new System.Windows.Forms.CheckBox();
            this.gbUserCategory = new System.Windows.Forms.GroupBox();
            this.lvUserCategory = new System.Windows.Forms.ListView();
            this.gbLimits = new System.Windows.Forms.GroupBox();
            this.cbLimitOccasional = new System.Windows.Forms.ComboBox();
            this.lblLimitOccasional = new System.Windows.Forms.Label();
            this.cbLimitPerType = new System.Windows.Forms.ComboBox();
            this.lblLimitPerType = new System.Windows.Forms.Label();
            this.cbLimitComposite = new System.Windows.Forms.ComboBox();
            this.lblLimitComposite = new System.Windows.Forms.Label();
            this.gbMassiveInput = new System.Windows.Forms.GroupBox();
            this.rbMassiveInputNo = new System.Windows.Forms.CheckBox();
            this.rbMassiveInputYes = new System.Windows.Forms.CheckBox();
            this.gbVerification = new System.Windows.Forms.GroupBox();
            this.rbVerificationNo = new System.Windows.Forms.CheckBox();
            this.rbVerificationYes = new System.Windows.Forms.CheckBox();
            this.cbCompany = new System.Windows.Forms.ComboBox();
            this.lblCompany = new System.Windows.Forms.Label();
            this.cbPaymentCode = new System.Windows.Forms.ComboBox();
            this.lblPaymentCode = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.lblDescAlternative = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbDescAltenative = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cbDescription = new System.Windows.Forms.ComboBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.gbResults = new System.Windows.Forms.GroupBox();
            this.lvConfirmationTypes = new System.Windows.Forms.ListView();
            this.lvCategoryDetails = new System.Windows.Forms.ListView();
            this.gbSearch.SuspendLayout();
            this.gbManualInput.SuspendLayout();
            this.gbUserCategory.SuspendLayout();
            this.gbLimits.SuspendLayout();
            this.gbMassiveInput.SuspendLayout();
            this.gbVerification.SuspendLayout();
            this.gbResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(214, 651);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(114, 651);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(14, 651);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(923, 651);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lvPassTypes
            // 
            this.lvPassTypes.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvPassTypes.FullRowSelect = true;
            this.lvPassTypes.GridLines = true;
            this.lvPassTypes.HideSelection = false;
            this.lvPassTypes.Location = new System.Drawing.Point(6, 19);
            this.lvPassTypes.MultiSelect = false;
            this.lvPassTypes.Name = "lvPassTypes";
            this.lvPassTypes.ShowItemToolTips = true;
            this.lvPassTypes.Size = new System.Drawing.Size(999, 235);
            this.lvPassTypes.TabIndex = 39;
            this.lvPassTypes.UseCompatibleStateImageBehavior = false;
            this.lvPassTypes.View = System.Windows.Forms.View.Details;
            this.lvPassTypes.SelectedIndexChanged += new System.EventHandler(this.lvPassTypes_SelectedIndexChanged);
            this.lvPassTypes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvPassTypes_ColumnClick);
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.btnClear);
            this.gbSearch.Controls.Add(this.gbManualInput);
            this.gbSearch.Controls.Add(this.gbUserCategory);
            this.gbSearch.Controls.Add(this.gbLimits);
            this.gbSearch.Controls.Add(this.gbMassiveInput);
            this.gbSearch.Controls.Add(this.gbVerification);
            this.gbSearch.Controls.Add(this.cbCompany);
            this.gbSearch.Controls.Add(this.lblCompany);
            this.gbSearch.Controls.Add(this.cbPaymentCode);
            this.gbSearch.Controls.Add(this.lblPaymentCode);
            this.gbSearch.Controls.Add(this.cbType);
            this.gbSearch.Controls.Add(this.lblDescAlternative);
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Controls.Add(this.cbDescAltenative);
            this.gbSearch.Controls.Add(this.lblType);
            this.gbSearch.Controls.Add(this.cbDescription);
            this.gbSearch.Controls.Add(this.lblDescription);
            this.gbSearch.Location = new System.Drawing.Point(1, 12);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(930, 279);
            this.gbSearch.TabIndex = 0;
            this.gbSearch.TabStop = false;
            this.gbSearch.Tag = "FILTERABLE";
            this.gbSearch.Text = "Search";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(651, 245);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(110, 23);
            this.btnClear.TabIndex = 20;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // gbManualInput
            // 
            this.gbManualInput.Controls.Add(this.rbManualInputNo);
            this.gbManualInput.Controls.Add(this.rbManualInputYes);
            this.gbManualInput.Location = new System.Drawing.Point(18, 208);
            this.gbManualInput.Name = "gbManualInput";
            this.gbManualInput.Size = new System.Drawing.Size(110, 65);
            this.gbManualInput.TabIndex = 5;
            this.gbManualInput.TabStop = false;
            this.gbManualInput.Text = "Manual input";
            // 
            // rbManualInputNo
            // 
            this.rbManualInputNo.AutoSize = true;
            this.rbManualInputNo.Checked = true;
            this.rbManualInputNo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rbManualInputNo.Location = new System.Drawing.Point(6, 42);
            this.rbManualInputNo.Name = "rbManualInputNo";
            this.rbManualInputNo.Size = new System.Drawing.Size(40, 17);
            this.rbManualInputNo.TabIndex = 3;
            this.rbManualInputNo.Text = "No";
            this.rbManualInputNo.UseVisualStyleBackColor = true;
            this.rbManualInputNo.CheckedChanged += new System.EventHandler(this.rbManualInputNo_CheckedChanged);
            // 
            // rbManualInputYes
            // 
            this.rbManualInputYes.AutoSize = true;
            this.rbManualInputYes.Checked = true;
            this.rbManualInputYes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rbManualInputYes.Location = new System.Drawing.Point(6, 21);
            this.rbManualInputYes.Name = "rbManualInputYes";
            this.rbManualInputYes.Size = new System.Drawing.Size(44, 17);
            this.rbManualInputYes.TabIndex = 2;
            this.rbManualInputYes.Text = "Yes";
            this.rbManualInputYes.UseVisualStyleBackColor = true;
            this.rbManualInputYes.CheckedChanged += new System.EventHandler(this.rbManualInputYes_CheckedChanged);
            // 
            // gbUserCategory
            // 
            this.gbUserCategory.Controls.Add(this.lvUserCategory);
            this.gbUserCategory.Location = new System.Drawing.Point(651, 22);
            this.gbUserCategory.Name = "gbUserCategory";
            this.gbUserCategory.Size = new System.Drawing.Size(273, 217);
            this.gbUserCategory.TabIndex = 10;
            this.gbUserCategory.TabStop = false;
            this.gbUserCategory.Text = "User category";
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
            // gbLimits
            // 
            this.gbLimits.Controls.Add(this.cbLimitOccasional);
            this.gbLimits.Controls.Add(this.lblLimitOccasional);
            this.gbLimits.Controls.Add(this.cbLimitPerType);
            this.gbLimits.Controls.Add(this.lblLimitPerType);
            this.gbLimits.Controls.Add(this.cbLimitComposite);
            this.gbLimits.Controls.Add(this.lblLimitComposite);
            this.gbLimits.Location = new System.Drawing.Point(377, 19);
            this.gbLimits.Name = "gbLimits";
            this.gbLimits.Size = new System.Drawing.Size(262, 129);
            this.gbLimits.TabIndex = 9;
            this.gbLimits.TabStop = false;
            this.gbLimits.Text = "Limits";
            // 
            // cbLimitOccasional
            // 
            this.cbLimitOccasional.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLimitOccasional.Location = new System.Drawing.Point(97, 101);
            this.cbLimitOccasional.Name = "cbLimitOccasional";
            this.cbLimitOccasional.Size = new System.Drawing.Size(159, 21);
            this.cbLimitOccasional.TabIndex = 2;
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
            this.cbLimitPerType.TabIndex = 1;
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
            this.cbLimitComposite.TabIndex = 0;
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
            // gbMassiveInput
            // 
            this.gbMassiveInput.Controls.Add(this.rbMassiveInputNo);
            this.gbMassiveInput.Controls.Add(this.rbMassiveInputYes);
            this.gbMassiveInput.Location = new System.Drawing.Point(250, 208);
            this.gbMassiveInput.Name = "gbMassiveInput";
            this.gbMassiveInput.Size = new System.Drawing.Size(110, 65);
            this.gbMassiveInput.TabIndex = 7;
            this.gbMassiveInput.TabStop = false;
            this.gbMassiveInput.Text = "Massive input";
            // 
            // rbMassiveInputNo
            // 
            this.rbMassiveInputNo.AutoSize = true;
            this.rbMassiveInputNo.Checked = true;
            this.rbMassiveInputNo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rbMassiveInputNo.Location = new System.Drawing.Point(7, 42);
            this.rbMassiveInputNo.Name = "rbMassiveInputNo";
            this.rbMassiveInputNo.Size = new System.Drawing.Size(40, 17);
            this.rbMassiveInputNo.TabIndex = 3;
            this.rbMassiveInputNo.Text = "No";
            this.rbMassiveInputNo.UseVisualStyleBackColor = true;
            this.rbMassiveInputNo.CheckedChanged += new System.EventHandler(this.rbMassiveInputNo_CheckedChanged);
            // 
            // rbMassiveInputYes
            // 
            this.rbMassiveInputYes.AutoSize = true;
            this.rbMassiveInputYes.Checked = true;
            this.rbMassiveInputYes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rbMassiveInputYes.Location = new System.Drawing.Point(7, 20);
            this.rbMassiveInputYes.Name = "rbMassiveInputYes";
            this.rbMassiveInputYes.Size = new System.Drawing.Size(44, 17);
            this.rbMassiveInputYes.TabIndex = 2;
            this.rbMassiveInputYes.Text = "Yes";
            this.rbMassiveInputYes.UseVisualStyleBackColor = true;
            this.rbMassiveInputYes.CheckedChanged += new System.EventHandler(this.rbMassiveInputYes_CheckedChanged);
            // 
            // gbVerification
            // 
            this.gbVerification.Controls.Add(this.rbVerificationNo);
            this.gbVerification.Controls.Add(this.rbVerificationYes);
            this.gbVerification.Location = new System.Drawing.Point(134, 208);
            this.gbVerification.Name = "gbVerification";
            this.gbVerification.Size = new System.Drawing.Size(110, 65);
            this.gbVerification.TabIndex = 6;
            this.gbVerification.TabStop = false;
            this.gbVerification.Text = "Verification:";
            // 
            // rbVerificationNo
            // 
            this.rbVerificationNo.AutoSize = true;
            this.rbVerificationNo.Checked = true;
            this.rbVerificationNo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rbVerificationNo.Location = new System.Drawing.Point(7, 43);
            this.rbVerificationNo.Name = "rbVerificationNo";
            this.rbVerificationNo.Size = new System.Drawing.Size(40, 17);
            this.rbVerificationNo.TabIndex = 3;
            this.rbVerificationNo.Text = "No";
            this.rbVerificationNo.UseVisualStyleBackColor = true;
            this.rbVerificationNo.CheckedChanged += new System.EventHandler(this.rbVerificationNo_CheckedChanged);
            // 
            // rbVerificationYes
            // 
            this.rbVerificationYes.AutoSize = true;
            this.rbVerificationYes.Checked = true;
            this.rbVerificationYes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rbVerificationYes.Location = new System.Drawing.Point(7, 20);
            this.rbVerificationYes.Name = "rbVerificationYes";
            this.rbVerificationYes.Size = new System.Drawing.Size(44, 17);
            this.rbVerificationYes.TabIndex = 2;
            this.rbVerificationYes.Text = "Yes";
            this.rbVerificationYes.UseVisualStyleBackColor = true;
            this.rbVerificationYes.CheckedChanged += new System.EventHandler(this.rbVerificationYes_CheckedChanged);
            // 
            // cbCompany
            // 
            this.cbCompany.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbCompany.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbCompany.Location = new System.Drawing.Point(113, 179);
            this.cbCompany.Name = "cbCompany";
            this.cbCompany.Size = new System.Drawing.Size(248, 21);
            this.cbCompany.TabIndex = 4;
            // 
            // lblCompany
            // 
            this.lblCompany.Location = new System.Drawing.Point(19, 177);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(88, 23);
            this.lblCompany.TabIndex = 19;
            this.lblCompany.Text = "Company:";
            this.lblCompany.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPaymentCode
            // 
            this.cbPaymentCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbPaymentCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbPaymentCode.Location = new System.Drawing.Point(113, 142);
            this.cbPaymentCode.Name = "cbPaymentCode";
            this.cbPaymentCode.Size = new System.Drawing.Size(101, 21);
            this.cbPaymentCode.TabIndex = 3;
            // 
            // lblPaymentCode
            // 
            this.lblPaymentCode.Location = new System.Drawing.Point(19, 140);
            this.lblPaymentCode.Name = "lblPaymentCode";
            this.lblPaymentCode.Size = new System.Drawing.Size(88, 23);
            this.lblPaymentCode.TabIndex = 17;
            this.lblPaymentCode.Text = "Payment code:";
            this.lblPaymentCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbType
            // 
            this.cbType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbType.Location = new System.Drawing.Point(112, 104);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(248, 21);
            this.cbType.TabIndex = 2;
            // 
            // lblDescAlternative
            // 
            this.lblDescAlternative.Location = new System.Drawing.Point(34, 51);
            this.lblDescAlternative.Name = "lblDescAlternative";
            this.lblDescAlternative.Size = new System.Drawing.Size(72, 43);
            this.lblDescAlternative.TabIndex = 2;
            this.lblDescAlternative.Text = "Desc alternative:";
            this.lblDescAlternative.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(814, 245);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(110, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbDescAltenative
            // 
            this.cbDescAltenative.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDescAltenative.Location = new System.Drawing.Point(112, 63);
            this.cbDescAltenative.Name = "cbDescAltenative";
            this.cbDescAltenative.Size = new System.Drawing.Size(248, 21);
            this.cbDescAltenative.TabIndex = 1;
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(18, 102);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(88, 23);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbDescription
            // 
            this.cbDescription.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDescription.Location = new System.Drawing.Point(112, 22);
            this.cbDescription.Name = "cbDescription";
            this.cbDescription.Size = new System.Drawing.Size(248, 21);
            this.cbDescription.TabIndex = 0;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(18, 20);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(88, 23);
            this.lblDescription.TabIndex = 6;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbResults
            // 
            this.gbResults.Controls.Add(this.lvConfirmationTypes);
            this.gbResults.Controls.Add(this.lvPassTypes);
            this.gbResults.Location = new System.Drawing.Point(1, 298);
            this.gbResults.Name = "gbResults";
            this.gbResults.Size = new System.Drawing.Size(1011, 347);
            this.gbResults.TabIndex = 1;
            this.gbResults.TabStop = false;
            this.gbResults.Text = "Results";
            // 
            // lvConfirmationTypes
            // 
            this.lvConfirmationTypes.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvConfirmationTypes.FullRowSelect = true;
            this.lvConfirmationTypes.GridLines = true;
            this.lvConfirmationTypes.HideSelection = false;
            this.lvConfirmationTypes.Location = new System.Drawing.Point(607, 260);
            this.lvConfirmationTypes.Name = "lvConfirmationTypes";
            this.lvConfirmationTypes.Size = new System.Drawing.Size(398, 81);
            this.lvConfirmationTypes.TabIndex = 41;
            this.lvConfirmationTypes.UseCompatibleStateImageBehavior = false;
            this.lvConfirmationTypes.View = System.Windows.Forms.View.Details;
            // 
            // lvCategoryDetails
            // 
            this.lvCategoryDetails.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvCategoryDetails.FullRowSelect = true;
            this.lvCategoryDetails.GridLines = true;
            this.lvCategoryDetails.HideSelection = false;
            this.lvCategoryDetails.Location = new System.Drawing.Point(7, 558);
            this.lvCategoryDetails.Name = "lvCategoryDetails";
            this.lvCategoryDetails.Size = new System.Drawing.Size(595, 81);
            this.lvCategoryDetails.TabIndex = 40;
            this.lvCategoryDetails.UseCompatibleStateImageBehavior = false;
            this.lvCategoryDetails.View = System.Windows.Forms.View.Details;
            // 
            // TMTypes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 670);
            this.ControlBox = false;
            this.Controls.Add(this.lvCategoryDetails);
            this.Controls.Add(this.gbResults);
            this.Controls.Add(this.gbSearch);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "TMTypes";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TMTypes";
            this.Load += new System.EventHandler(this.TMTypes_Load);
            this.gbSearch.ResumeLayout(false);
            this.gbManualInput.ResumeLayout(false);
            this.gbManualInput.PerformLayout();
            this.gbUserCategory.ResumeLayout(false);
            this.gbLimits.ResumeLayout(false);
            this.gbMassiveInput.ResumeLayout(false);
            this.gbMassiveInput.PerformLayout();
            this.gbVerification.ResumeLayout(false);
            this.gbVerification.PerformLayout();
            this.gbResults.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListView lvPassTypes;
        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label lblDescAlternative;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cbDescAltenative;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.ComboBox cbCompany;
        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.ComboBox cbPaymentCode;
        private System.Windows.Forms.Label lblPaymentCode;
        private System.Windows.Forms.GroupBox gbMassiveInput;
        private System.Windows.Forms.GroupBox gbVerification;
        private System.Windows.Forms.GroupBox gbLimits;
        private System.Windows.Forms.ComboBox cbLimitComposite;
        private System.Windows.Forms.Label lblLimitComposite;
        private System.Windows.Forms.ComboBox cbLimitOccasional;
        private System.Windows.Forms.Label lblLimitOccasional;
        private System.Windows.Forms.ComboBox cbLimitPerType;
        private System.Windows.Forms.Label lblLimitPerType;
        private System.Windows.Forms.GroupBox gbUserCategory;
        private System.Windows.Forms.ListView lvUserCategory;
        private System.Windows.Forms.GroupBox gbResults;
        private System.Windows.Forms.GroupBox gbManualInput;
        private System.Windows.Forms.ListView lvCategoryDetails;
        private System.Windows.Forms.CheckBox rbManualInputNo;
        private System.Windows.Forms.CheckBox rbManualInputYes;
        private System.Windows.Forms.CheckBox rbVerificationYes;
        private System.Windows.Forms.CheckBox rbMassiveInputNo;
        private System.Windows.Forms.CheckBox rbMassiveInputYes;
        private System.Windows.Forms.CheckBox rbVerificationNo;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ListView lvConfirmationTypes;
    }
}