namespace UI
{
    partial class OnlineMealsUsedAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OnlineMealsUsedAdd));
            this.gbUnitFilter = new System.Windows.Forms.GroupBox();
            this.btnOUTree = new System.Windows.Forms.Button();
            this.cbOU = new System.Windows.Forms.ComboBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.rbOU = new System.Windows.Forms.RadioButton();
            this.rbWU = new System.Windows.Forms.RadioButton();
            this.chbHierachyOU = new System.Windows.Forms.CheckBox();
            this.chbHierarhiclyWU = new System.Windows.Forms.CheckBox();
            this.tbEmployee = new System.Windows.Forms.TextBox();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.gbDates = new System.Windows.Forms.GroupBox();
            this.lvDates = new System.Windows.Forms.ListView();
            this.gbMealType = new System.Windows.Forms.GroupBox();
            this.cbMealTypes = new System.Windows.Forms.ComboBox();
            this.gbQty = new System.Windows.Forms.GroupBox();
            this.numQty = new System.Windows.Forms.NumericUpDown();
            this.gbMassiveInput = new System.Windows.Forms.GroupBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.lvMealsUsed = new System.Windows.Forms.ListView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.gbMealPoint = new System.Windows.Forms.GroupBox();
            this.lblPoint = new System.Windows.Forms.Label();
            this.lblRestaurant = new System.Windows.Forms.Label();
            this.cbPoint = new System.Windows.Forms.ComboBox();
            this.cbRestaurant = new System.Windows.Forms.ComboBox();
            this.gbTime = new System.Windows.Forms.GroupBox();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.gbUnitFilter.SuspendLayout();
            this.gbDates.SuspendLayout();
            this.gbMealType.SuspendLayout();
            this.gbQty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).BeginInit();
            this.gbMassiveInput.SuspendLayout();
            this.gbMealPoint.SuspendLayout();
            this.gbTime.SuspendLayout();
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
            this.gbUnitFilter.Location = new System.Drawing.Point(7, 3);
            this.gbUnitFilter.Name = "gbUnitFilter";
            this.gbUnitFilter.Size = new System.Drawing.Size(334, 82);
            this.gbUnitFilter.TabIndex = 0;
            this.gbUnitFilter.TabStop = false;
            this.gbUnitFilter.Text = "Unit filter";
            // 
            // btnOUTree
            // 
            this.btnOUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnOUTree.Image")));
            this.btnOUTree.Location = new System.Drawing.Point(296, 43);
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
            this.cbOU.Location = new System.Drawing.Point(69, 45);
            this.cbOU.Name = "cbOU";
            this.cbOU.Size = new System.Drawing.Size(221, 21);
            this.cbOU.TabIndex = 5;
            this.cbOU.SelectedIndexChanged += new System.EventHandler(this.cbOU_SelectedIndexChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(296, 16);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 2;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbWU
            // 
            this.cbWU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbWU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(69, 18);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(221, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // rbOU
            // 
            this.rbOU.AutoSize = true;
            this.rbOU.Location = new System.Drawing.Point(20, 46);
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
            this.rbWU.Location = new System.Drawing.Point(20, 19);
            this.rbWU.Name = "rbWU";
            this.rbWU.Size = new System.Drawing.Size(38, 17);
            this.rbWU.TabIndex = 0;
            this.rbWU.TabStop = true;
            this.rbWU.Text = "FS";
            this.rbWU.UseVisualStyleBackColor = true;
            this.rbWU.CheckedChanged += new System.EventHandler(this.rbWU_CheckedChanged);
            // 
            // chbHierachyOU
            // 
            this.chbHierachyOU.Location = new System.Drawing.Point(303, 89);
            this.chbHierachyOU.Name = "chbHierachyOU";
            this.chbHierachyOU.Size = new System.Drawing.Size(25, 24);
            this.chbHierachyOU.TabIndex = 7;
            this.chbHierachyOU.Text = "Hierarchy ";
            this.chbHierachyOU.CheckedChanged += new System.EventHandler(this.chbHierachyOU_CheckedChanged);
            // 
            // chbHierarhiclyWU
            // 
            this.chbHierarhiclyWU.Location = new System.Drawing.Point(263, 89);
            this.chbHierarhiclyWU.Name = "chbHierarhiclyWU";
            this.chbHierarhiclyWU.Size = new System.Drawing.Size(25, 24);
            this.chbHierarhiclyWU.TabIndex = 3;
            this.chbHierarhiclyWU.Text = "Hierarchy ";
            this.chbHierarhiclyWU.CheckedChanged += new System.EventHandler(this.chbHierarhiclyWU_CheckedChanged);
            // 
            // tbEmployee
            // 
            this.tbEmployee.Location = new System.Drawing.Point(7, 91);
            this.tbEmployee.Name = "tbEmployee";
            this.tbEmployee.Size = new System.Drawing.Size(250, 20);
            this.tbEmployee.TabIndex = 1;
            this.tbEmployee.TextChanged += new System.EventHandler(this.tbEmployee_TextChanged);
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(7, 117);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.ShowItemToolTips = true;
            this.lvEmployees.Size = new System.Drawing.Size(342, 275);
            this.lvEmployees.TabIndex = 12;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployees_ColumnClick);
            // 
            // gbDates
            // 
            this.gbDates.Controls.Add(this.lvDates);
            this.gbDates.Location = new System.Drawing.Point(7, 398);
            this.gbDates.Name = "gbDates";
            this.gbDates.Size = new System.Drawing.Size(123, 195);
            this.gbDates.TabIndex = 3;
            this.gbDates.TabStop = false;
            this.gbDates.Text = "Dates";
            // 
            // lvDates
            // 
            this.lvDates.FullRowSelect = true;
            this.lvDates.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvDates.HideSelection = false;
            this.lvDates.Location = new System.Drawing.Point(6, 19);
            this.lvDates.Name = "lvDates";
            this.lvDates.Size = new System.Drawing.Size(108, 164);
            this.lvDates.TabIndex = 16;
            this.lvDates.UseCompatibleStateImageBehavior = false;
            this.lvDates.View = System.Windows.Forms.View.Details;
            // 
            // gbMealType
            // 
            this.gbMealType.Controls.Add(this.cbMealTypes);
            this.gbMealType.Location = new System.Drawing.Point(136, 398);
            this.gbMealType.Name = "gbMealType";
            this.gbMealType.Size = new System.Drawing.Size(205, 52);
            this.gbMealType.TabIndex = 4;
            this.gbMealType.TabStop = false;
            this.gbMealType.Text = "Meal type";
            // 
            // cbMealTypes
            // 
            this.cbMealTypes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbMealTypes.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbMealTypes.FormattingEnabled = true;
            this.cbMealTypes.Location = new System.Drawing.Point(6, 19);
            this.cbMealTypes.Name = "cbMealTypes";
            this.cbMealTypes.Size = new System.Drawing.Size(193, 21);
            this.cbMealTypes.TabIndex = 0;
            // 
            // gbQty
            // 
            this.gbQty.Controls.Add(this.numQty);
            this.gbQty.Location = new System.Drawing.Point(225, 542);
            this.gbQty.Name = "gbQty";
            this.gbQty.Size = new System.Drawing.Size(69, 51);
            this.gbQty.TabIndex = 7;
            this.gbQty.TabStop = false;
            this.gbQty.Text = "Quantity";
            // 
            // numQty
            // 
            this.numQty.Location = new System.Drawing.Point(6, 19);
            this.numQty.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numQty.Name = "numQty";
            this.numQty.Size = new System.Drawing.Size(57, 20);
            this.numQty.TabIndex = 0;
            this.numQty.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // gbMassiveInput
            // 
            this.gbMassiveInput.Controls.Add(this.lblPath);
            this.gbMassiveInput.Controls.Add(this.btnBrowse);
            this.gbMassiveInput.Controls.Add(this.tbPath);
            this.gbMassiveInput.Location = new System.Drawing.Point(7, 599);
            this.gbMassiveInput.Name = "gbMassiveInput";
            this.gbMassiveInput.Size = new System.Drawing.Size(334, 73);
            this.gbMassiveInput.TabIndex = 8;
            this.gbMassiveInput.TabStop = false;
            this.gbMassiveInput.Text = "Massive input";
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(6, 23);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(32, 13);
            this.lblPath.TabIndex = 0;
            this.lblPath.Text = "Path:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(246, 42);
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
            this.tbPath.Location = new System.Drawing.Point(6, 44);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(234, 20);
            this.tbPath.TabIndex = 1;
            // 
            // lvMealsUsed
            // 
            this.lvMealsUsed.FullRowSelect = true;
            this.lvMealsUsed.GridLines = true;
            this.lvMealsUsed.HideSelection = false;
            this.lvMealsUsed.Location = new System.Drawing.Point(365, 10);
            this.lvMealsUsed.Name = "lvMealsUsed";
            this.lvMealsUsed.ShowItemToolTips = true;
            this.lvMealsUsed.Size = new System.Drawing.Size(622, 625);
            this.lvMealsUsed.TabIndex = 10;
            this.lvMealsUsed.UseCompatibleStateImageBehavior = false;
            this.lvMealsUsed.View = System.Windows.Forms.View.Details;
            this.lvMealsUsed.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvMealsUsed_ColumnClick);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(826, 641);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(907, 641);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(365, 641);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(188, 23);
            this.btnRemove.TabIndex = 11;
            this.btnRemove.Text = "Remove selected";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(303, 570);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(56, 23);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gbMealPoint
            // 
            this.gbMealPoint.Controls.Add(this.lblPoint);
            this.gbMealPoint.Controls.Add(this.lblRestaurant);
            this.gbMealPoint.Controls.Add(this.cbPoint);
            this.gbMealPoint.Controls.Add(this.cbRestaurant);
            this.gbMealPoint.Location = new System.Drawing.Point(136, 456);
            this.gbMealPoint.Name = "gbMealPoint";
            this.gbMealPoint.Size = new System.Drawing.Size(205, 80);
            this.gbMealPoint.TabIndex = 5;
            this.gbMealPoint.TabStop = false;
            this.gbMealPoint.Text = "Meal point";
            // 
            // lblPoint
            // 
            this.lblPoint.Location = new System.Drawing.Point(10, 50);
            this.lblPoint.Name = "lblPoint";
            this.lblPoint.Size = new System.Drawing.Size(62, 13);
            this.lblPoint.TabIndex = 2;
            this.lblPoint.Text = "Point:";
            this.lblPoint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRestaurant
            // 
            this.lblRestaurant.Location = new System.Drawing.Point(10, 22);
            this.lblRestaurant.Name = "lblRestaurant";
            this.lblRestaurant.Size = new System.Drawing.Size(62, 13);
            this.lblRestaurant.TabIndex = 0;
            this.lblRestaurant.Text = "Restaurant:";
            this.lblRestaurant.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPoint
            // 
            this.cbPoint.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbPoint.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbPoint.FormattingEnabled = true;
            this.cbPoint.Location = new System.Drawing.Point(75, 47);
            this.cbPoint.Name = "cbPoint";
            this.cbPoint.Size = new System.Drawing.Size(124, 21);
            this.cbPoint.TabIndex = 3;
            // 
            // cbRestaurant
            // 
            this.cbRestaurant.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbRestaurant.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbRestaurant.FormattingEnabled = true;
            this.cbRestaurant.Location = new System.Drawing.Point(75, 19);
            this.cbRestaurant.Name = "cbRestaurant";
            this.cbRestaurant.Size = new System.Drawing.Size(124, 21);
            this.cbRestaurant.TabIndex = 1;
            this.cbRestaurant.SelectedIndexChanged += new System.EventHandler(this.cbRestaurant_SelectedIndexChanged);
            // 
            // gbTime
            // 
            this.gbTime.Controls.Add(this.dtpTime);
            this.gbTime.Location = new System.Drawing.Point(136, 542);
            this.gbTime.Name = "gbTime";
            this.gbTime.Size = new System.Drawing.Size(83, 51);
            this.gbTime.TabIndex = 6;
            this.gbTime.TabStop = false;
            this.gbTime.Text = "Time";
            // 
            // dtpTime
            // 
            this.dtpTime.CustomFormat = "HH:mm";
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTime.Location = new System.Drawing.Point(6, 19);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.ShowUpDown = true;
            this.dtpTime.Size = new System.Drawing.Size(66, 20);
            this.dtpTime.TabIndex = 0;
            // 
            // OnlineMealsUsedAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 684);
            this.ControlBox = false;
            this.Controls.Add(this.chbHierarhiclyWU);
            this.Controls.Add(this.chbHierachyOU);
            this.Controls.Add(this.gbTime);
            this.Controls.Add(this.gbMealPoint);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lvMealsUsed);
            this.Controls.Add(this.gbMassiveInput);
            this.Controls.Add(this.gbQty);
            this.Controls.Add(this.gbMealType);
            this.Controls.Add(this.gbDates);
            this.Controls.Add(this.lvEmployees);
            this.Controls.Add(this.tbEmployee);
            this.Controls.Add(this.gbUnitFilter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "OnlineMealsUsedAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OnlineMealsUsedAdd";
            this.Load += new System.EventHandler(this.OnlineMealsUsedAdd_Load);
            this.gbUnitFilter.ResumeLayout(false);
            this.gbUnitFilter.PerformLayout();
            this.gbDates.ResumeLayout(false);
            this.gbMealType.ResumeLayout(false);
            this.gbQty.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).EndInit();
            this.gbMassiveInput.ResumeLayout(false);
            this.gbMassiveInput.PerformLayout();
            this.gbMealPoint.ResumeLayout(false);
            this.gbTime.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbUnitFilter;
        private System.Windows.Forms.RadioButton rbOU;
        private System.Windows.Forms.RadioButton rbWU;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.Button btnOUTree;
        private System.Windows.Forms.ComboBox cbOU;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.CheckBox chbHierachyOU;
        private System.Windows.Forms.CheckBox chbHierarhiclyWU;
        private System.Windows.Forms.TextBox tbEmployee;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.GroupBox gbDates;
        private System.Windows.Forms.ListView lvDates;
        private System.Windows.Forms.GroupBox gbMealType;
        private System.Windows.Forms.GroupBox gbQty;
        private System.Windows.Forms.ComboBox cbMealTypes;
        private System.Windows.Forms.NumericUpDown numQty;
        private System.Windows.Forms.GroupBox gbMassiveInput;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.ListView lvMealsUsed;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox gbMealPoint;
        private System.Windows.Forms.ComboBox cbRestaurant;
        private System.Windows.Forms.ComboBox cbPoint;
        private System.Windows.Forms.Label lblRestaurant;
        private System.Windows.Forms.Label lblPoint;
        private System.Windows.Forms.GroupBox gbTime;
        private System.Windows.Forms.DateTimePicker dtpTime;
    }
}