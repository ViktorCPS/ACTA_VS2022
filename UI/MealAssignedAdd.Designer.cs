namespace UI
{
    partial class MealAssignedAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MealAssignedAdd));
            this.gbEmployees = new System.Windows.Forms.GroupBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.gbValidity = new System.Windows.Forms.GroupBox();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.chbUnlimited = new System.Windows.Forms.CheckBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.gbAssignmentType = new System.Windows.Forms.GroupBox();
            this.chbUnlimitedDailyQTY = new System.Windows.Forms.CheckBox();
            this.tbAmount = new System.Windows.Forms.TextBox();
            this.lblAmount = new System.Windows.Forms.Label();
            this.chbUnlimitedMoney = new System.Windows.Forms.CheckBox();
            this.chbUnlimitedMeals = new System.Windows.Forms.CheckBox();
            this.lblDailyLimit = new System.Windows.Forms.Label();
            this.nudQuantityDaily = new System.Windows.Forms.NumericUpDown();
            this.lblNumber = new System.Windows.Forms.Label();
            this.nudQuantity = new System.Windows.Forms.NumericUpDown();
            this.rbMoney = new System.Windows.Forms.RadioButton();
            this.rbMeals = new System.Windows.Forms.RadioButton();
            this.btnAssign = new System.Windows.Forms.Button();
            this.lvAssignMeals = new System.Windows.Forms.ListView();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbMealType = new System.Windows.Forms.GroupBox();
            this.cbMealType = new System.Windows.Forms.ComboBox();
            this.lblMealType = new System.Windows.Forms.Label();
            this.gbEmployees.SuspendLayout();
            this.gbValidity.SuspendLayout();
            this.gbAssignmentType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantityDaily)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).BeginInit();
            this.gbMealType.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbEmployees
            // 
            this.gbEmployees.Controls.Add(this.btnWUTree);
            this.gbEmployees.Controls.Add(this.chbHierarhicly);
            this.gbEmployees.Controls.Add(this.lblWorkingUnit);
            this.gbEmployees.Controls.Add(this.cbWorkingUnit);
            this.gbEmployees.Controls.Add(this.lvEmployees);
            this.gbEmployees.Location = new System.Drawing.Point(12, 12);
            this.gbEmployees.Name = "gbEmployees";
            this.gbEmployees.Size = new System.Drawing.Size(310, 254);
            this.gbEmployees.TabIndex = 0;
            this.gbEmployees.TabStop = false;
            this.gbEmployees.Text = "Employees";
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(279, 17);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 20;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.AutoSize = true;
            this.chbHierarhicly.Checked = true;
            this.chbHierarhicly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbHierarhicly.Location = new System.Drawing.Point(110, 46);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(71, 17);
            this.chbHierarhicly.TabIndex = 3;
            this.chbHierarhicly.Text = "Hierarchy";
            this.chbHierarhicly.UseVisualStyleBackColor = true;
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(6, 22);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(98, 13);
            this.lblWorkingUnit.TabIndex = 1;
            this.lblWorkingUnit.Text = "Working unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.FormattingEnabled = true;
            this.cbWorkingUnit.Location = new System.Drawing.Point(110, 19);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(163, 21);
            this.cbWorkingUnit.TabIndex = 2;
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(6, 69);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.Size = new System.Drawing.Size(297, 179);
            this.lvEmployees.TabIndex = 4;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            // 
            // gbValidity
            // 
            this.gbValidity.Controls.Add(this.lblTo);
            this.gbValidity.Controls.Add(this.dtpTo);
            this.gbValidity.Controls.Add(this.lblFrom);
            this.gbValidity.Controls.Add(this.chbUnlimited);
            this.gbValidity.Controls.Add(this.dtpFrom);
            this.gbValidity.Location = new System.Drawing.Point(12, 272);
            this.gbValidity.Name = "gbValidity";
            this.gbValidity.Size = new System.Drawing.Size(310, 95);
            this.gbValidity.TabIndex = 5;
            this.gbValidity.TabStop = false;
            this.gbValidity.Text = "Validity";
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(81, 49);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(23, 13);
            this.lblTo.TabIndex = 8;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(110, 45);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(193, 20);
            this.dtpTo.TabIndex = 9;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(71, 23);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(33, 13);
            this.lblFrom.TabIndex = 6;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbUnlimited
            // 
            this.chbUnlimited.AutoSize = true;
            this.chbUnlimited.Location = new System.Drawing.Point(110, 71);
            this.chbUnlimited.Name = "chbUnlimited";
            this.chbUnlimited.Size = new System.Drawing.Size(69, 17);
            this.chbUnlimited.TabIndex = 10;
            this.chbUnlimited.Text = "Unlimited";
            this.chbUnlimited.UseVisualStyleBackColor = true;
            this.chbUnlimited.CheckedChanged += new System.EventHandler(this.chbUnlimited_CheckedChanged);
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(110, 19);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(193, 20);
            this.dtpFrom.TabIndex = 7;
            // 
            // gbAssignmentType
            // 
            this.gbAssignmentType.Controls.Add(this.chbUnlimitedDailyQTY);
            this.gbAssignmentType.Controls.Add(this.tbAmount);
            this.gbAssignmentType.Controls.Add(this.lblAmount);
            this.gbAssignmentType.Controls.Add(this.chbUnlimitedMoney);
            this.gbAssignmentType.Controls.Add(this.chbUnlimitedMeals);
            this.gbAssignmentType.Controls.Add(this.lblDailyLimit);
            this.gbAssignmentType.Controls.Add(this.nudQuantityDaily);
            this.gbAssignmentType.Controls.Add(this.lblNumber);
            this.gbAssignmentType.Controls.Add(this.nudQuantity);
            this.gbAssignmentType.Controls.Add(this.rbMoney);
            this.gbAssignmentType.Controls.Add(this.rbMeals);
            this.gbAssignmentType.Location = new System.Drawing.Point(12, 373);
            this.gbAssignmentType.Name = "gbAssignmentType";
            this.gbAssignmentType.Size = new System.Drawing.Size(310, 198);
            this.gbAssignmentType.TabIndex = 11;
            this.gbAssignmentType.TabStop = false;
            this.gbAssignmentType.Text = "Assignment type";
            // 
            // chbUnlimitedDailyQTY
            // 
            this.chbUnlimitedDailyQTY.AutoSize = true;
            this.chbUnlimitedDailyQTY.Location = new System.Drawing.Point(202, 40);
            this.chbUnlimitedDailyQTY.Name = "chbUnlimitedDailyQTY";
            this.chbUnlimitedDailyQTY.Size = new System.Drawing.Size(69, 17);
            this.chbUnlimitedDailyQTY.TabIndex = 14;
            this.chbUnlimitedDailyQTY.Text = "Unlimited";
            this.chbUnlimitedDailyQTY.UseVisualStyleBackColor = true;
            this.chbUnlimitedDailyQTY.CheckedChanged += new System.EventHandler(this.chbUnlimitedDailyQTY_CheckedChanged);
            // 
            // tbAmount
            // 
            this.tbAmount.Location = new System.Drawing.Point(110, 172);
            this.tbAmount.Name = "tbAmount";
            this.tbAmount.Size = new System.Drawing.Size(100, 20);
            this.tbAmount.TabIndex = 22;
            // 
            // lblAmount
            // 
            this.lblAmount.Location = new System.Drawing.Point(32, 175);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(72, 13);
            this.lblAmount.TabIndex = 21;
            this.lblAmount.Text = "Amount:";
            this.lblAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbUnlimitedMoney
            // 
            this.chbUnlimitedMoney.AutoSize = true;
            this.chbUnlimitedMoney.Location = new System.Drawing.Point(110, 137);
            this.chbUnlimitedMoney.Name = "chbUnlimitedMoney";
            this.chbUnlimitedMoney.Size = new System.Drawing.Size(69, 17);
            this.chbUnlimitedMoney.TabIndex = 20;
            this.chbUnlimitedMoney.Text = "Unlimited";
            this.chbUnlimitedMoney.UseVisualStyleBackColor = true;
            this.chbUnlimitedMoney.CheckedChanged += new System.EventHandler(this.chbUnlimitedMoney_CheckedChanged);
            // 
            // chbUnlimitedMeals
            // 
            this.chbUnlimitedMeals.AutoSize = true;
            this.chbUnlimitedMeals.Location = new System.Drawing.Point(61, 40);
            this.chbUnlimitedMeals.Name = "chbUnlimitedMeals";
            this.chbUnlimitedMeals.Size = new System.Drawing.Size(69, 17);
            this.chbUnlimitedMeals.TabIndex = 13;
            this.chbUnlimitedMeals.Text = "Unlimited";
            this.chbUnlimitedMeals.UseVisualStyleBackColor = true;
            this.chbUnlimitedMeals.CheckedChanged += new System.EventHandler(this.chbUnlimitedMeals_CheckedChanged);
            // 
            // lblDailyLimit
            // 
            this.lblDailyLimit.Location = new System.Drawing.Point(176, 77);
            this.lblDailyLimit.Name = "lblDailyLimit";
            this.lblDailyLimit.Size = new System.Drawing.Size(76, 13);
            this.lblDailyLimit.TabIndex = 17;
            this.lblDailyLimit.Text = "Daily limit:";
            this.lblDailyLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudQuantityDaily
            // 
            this.nudQuantityDaily.Location = new System.Drawing.Point(259, 75);
            this.nudQuantityDaily.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudQuantityDaily.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudQuantityDaily.Name = "nudQuantityDaily";
            this.nudQuantityDaily.Size = new System.Drawing.Size(45, 20);
            this.nudQuantityDaily.TabIndex = 18;
            this.nudQuantityDaily.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudQuantityDaily.ValueChanged += new System.EventHandler(this.nudQuantityDaily_ValueChanged);
            // 
            // lblNumber
            // 
            this.lblNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNumber.Location = new System.Drawing.Point(29, 77);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(76, 13);
            this.lblNumber.TabIndex = 15;
            this.lblNumber.Text = "Number:";
            this.lblNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudQuantity
            // 
            this.nudQuantity.Location = new System.Drawing.Point(110, 75);
            this.nudQuantity.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudQuantity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudQuantity.Name = "nudQuantity";
            this.nudQuantity.Size = new System.Drawing.Size(45, 20);
            this.nudQuantity.TabIndex = 16;
            this.nudQuantity.Value = new decimal(new int[] {
            22,
            0,
            0,
            0});
            this.nudQuantity.ValueChanged += new System.EventHandler(this.nudQuantity_ValueChanged);
            // 
            // rbMoney
            // 
            this.rbMoney.AutoSize = true;
            this.rbMoney.Location = new System.Drawing.Point(6, 117);
            this.rbMoney.Name = "rbMoney";
            this.rbMoney.Size = new System.Drawing.Size(57, 17);
            this.rbMoney.TabIndex = 19;
            this.rbMoney.TabStop = true;
            this.rbMoney.Text = "Money";
            this.rbMoney.UseVisualStyleBackColor = true;
            this.rbMoney.CheckedChanged += new System.EventHandler(this.rbMoney_CheckedChanged);
            // 
            // rbMeals
            // 
            this.rbMeals.AutoSize = true;
            this.rbMeals.Location = new System.Drawing.Point(6, 19);
            this.rbMeals.Name = "rbMeals";
            this.rbMeals.Size = new System.Drawing.Size(53, 17);
            this.rbMeals.TabIndex = 12;
            this.rbMeals.Text = "Meals";
            this.rbMeals.UseVisualStyleBackColor = true;
            this.rbMeals.CheckedChanged += new System.EventHandler(this.rbMeals_CheckedChanged);
            // 
            // btnAssign
            // 
            this.btnAssign.Location = new System.Drawing.Point(341, 243);
            this.btnAssign.Name = "btnAssign";
            this.btnAssign.Size = new System.Drawing.Size(75, 23);
            this.btnAssign.TabIndex = 26;
            this.btnAssign.Text = ">>";
            this.btnAssign.UseVisualStyleBackColor = true;
            this.btnAssign.Click += new System.EventHandler(this.btnAssign_Click);
            // 
            // lvAssignMeals
            // 
            this.lvAssignMeals.FullRowSelect = true;
            this.lvAssignMeals.GridLines = true;
            this.lvAssignMeals.HideSelection = false;
            this.lvAssignMeals.Location = new System.Drawing.Point(434, 12);
            this.lvAssignMeals.Name = "lvAssignMeals";
            this.lvAssignMeals.Size = new System.Drawing.Size(570, 559);
            this.lvAssignMeals.TabIndex = 27;
            this.lvAssignMeals.UseCompatibleStateImageBehavior = false;
            this.lvAssignMeals.View = System.Windows.Forms.View.Details;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(434, 608);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 28;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(838, 608);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 29;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(929, 608);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 30;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbMealType
            // 
            this.gbMealType.Controls.Add(this.cbMealType);
            this.gbMealType.Controls.Add(this.lblMealType);
            this.gbMealType.Location = new System.Drawing.Point(12, 577);
            this.gbMealType.Name = "gbMealType";
            this.gbMealType.Size = new System.Drawing.Size(310, 56);
            this.gbMealType.TabIndex = 23;
            this.gbMealType.TabStop = false;
            this.gbMealType.Text = "Meal type";
            // 
            // cbMealType
            // 
            this.cbMealType.FormattingEnabled = true;
            this.cbMealType.Location = new System.Drawing.Point(110, 19);
            this.cbMealType.Name = "cbMealType";
            this.cbMealType.Size = new System.Drawing.Size(193, 21);
            this.cbMealType.TabIndex = 25;
            // 
            // lblMealType
            // 
            this.lblMealType.Location = new System.Drawing.Point(9, 22);
            this.lblMealType.Name = "lblMealType";
            this.lblMealType.Size = new System.Drawing.Size(95, 13);
            this.lblMealType.TabIndex = 24;
            this.lblMealType.Text = "Meal type:";
            this.lblMealType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MealAssignedAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 643);
            this.KeyPreview = true;
            this.ControlBox = false;
            this.Controls.Add(this.gbMealType);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lvAssignMeals);
            this.Controls.Add(this.btnAssign);
            this.Controls.Add(this.gbAssignmentType);
            this.Controls.Add(this.gbValidity);
            this.Controls.Add(this.gbEmployees);
            this.Name = "MealAssignedAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Assigning meals";
            this.Load += new System.EventHandler(this.MealAssignedAdd_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MealAssignedAdd_KeyUp);
            this.gbEmployees.ResumeLayout(false);
            this.gbEmployees.PerformLayout();
            this.gbValidity.ResumeLayout(false);
            this.gbValidity.PerformLayout();
            this.gbAssignmentType.ResumeLayout(false);
            this.gbAssignmentType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantityDaily)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).EndInit();
            this.gbMealType.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.GroupBox gbEmployees;
        protected System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.ComboBox cbWorkingUnit;
        private System.Windows.Forms.Label lblWorkingUnit;
        protected System.Windows.Forms.CheckBox chbHierarhicly;
        protected System.Windows.Forms.GroupBox gbValidity;
        private System.Windows.Forms.Label lblFrom;
        protected System.Windows.Forms.CheckBox chbUnlimited;
        protected System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        protected System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.GroupBox gbAssignmentType;
        private System.Windows.Forms.Label lblNumber;
        private System.Windows.Forms.NumericUpDown nudQuantity;
        private System.Windows.Forms.RadioButton rbMoney;
        private System.Windows.Forms.RadioButton rbMeals;
        protected System.Windows.Forms.CheckBox chbUnlimitedMeals;
        private System.Windows.Forms.Label lblDailyLimit;
        private System.Windows.Forms.NumericUpDown nudQuantityDaily;
        private System.Windows.Forms.TextBox tbAmount;
        private System.Windows.Forms.Label lblAmount;
        protected System.Windows.Forms.CheckBox chbUnlimitedMoney;
        private System.Windows.Forms.Button btnAssign;
        protected System.Windows.Forms.ListView lvAssignMeals;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbMealType;
        private System.Windows.Forms.ComboBox cbMealType;
        private System.Windows.Forms.Label lblMealType;
        protected System.Windows.Forms.CheckBox chbUnlimitedDailyQTY;
        private System.Windows.Forms.Button btnWUTree;
    }
}