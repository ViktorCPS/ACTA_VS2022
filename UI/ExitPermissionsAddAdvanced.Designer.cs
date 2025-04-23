namespace UI
{
    partial class ExitPermissionsAddAdvanced

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExitPermissionsAddAdvanced));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.gbChooseEmpl = new System.Windows.Forms.GroupBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.lblWU = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.gbDays = new System.Windows.Forms.GroupBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblRemoveDay = new System.Windows.Forms.Label();
            this.lvDays = new System.Windows.Forms.ListView();
            this.rbCertainDays = new System.Windows.Forms.RadioButton();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.rbPeriod = new System.Windows.Forms.RadioButton();
            this.gbPassType = new System.Windows.Forms.GroupBox();
            this.cbPassType = new System.Windows.Forms.ComboBox();
            this.gbSortOrder = new System.Windows.Forms.GroupBox();
            this.rbDaySort = new System.Windows.Forms.RadioButton();
            this.rbEmplSort = new System.Windows.Forms.RadioButton();
            this.gbDescription = new System.Windows.Forms.GroupBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.gbChooseEmpl.SuspendLayout();
            this.gbDays.SuspendLayout();
            this.gbPassType.SuspendLayout();
            this.gbSortOrder.SuspendLayout();
            this.gbDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(467, 600);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(12, 600);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 21;
            this.btnPreview.Text = "Preview";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // gbChooseEmpl
            // 
            this.gbChooseEmpl.Controls.Add(this.chbHierarhicly);
            this.gbChooseEmpl.Controls.Add(this.btnWUTree);
            this.gbChooseEmpl.Controls.Add(this.btnNext);
            this.gbChooseEmpl.Controls.Add(this.cbWU);
            this.gbChooseEmpl.Controls.Add(this.lblWU);
            this.gbChooseEmpl.Controls.Add(this.btnPrev);
            this.gbChooseEmpl.Controls.Add(this.lvEmployees);
            this.gbChooseEmpl.Location = new System.Drawing.Point(12, 12);
            this.gbChooseEmpl.Name = "gbChooseEmpl";
            this.gbChooseEmpl.Size = new System.Drawing.Size(530, 295);
            this.gbChooseEmpl.TabIndex = 22;
            this.gbChooseEmpl.TabStop = false;
            this.gbChooseEmpl.Text = "Choose employee";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.AutoSize = true;
            this.chbHierarhicly.Checked = true;
            this.chbHierarhicly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbHierarhicly.Location = new System.Drawing.Point(357, 38);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(71, 17);
            this.chbHierarhicly.TabIndex = 45;
            this.chbHierarhicly.Text = "Hierarchy";
            this.chbHierarhicly.UseVisualStyleBackColor = true;
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(326, 34);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 44;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(479, 52);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 23;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.Location = new System.Drawing.Point(132, 36);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(188, 21);
            this.cbWU.TabIndex = 43;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(26, 34);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(100, 23);
            this.lblWU.TabIndex = 42;
            this.lblWU.Text = "Working Unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(441, 52);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 22;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(18, 81);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.Size = new System.Drawing.Size(493, 208);
            this.lvEmployees.TabIndex = 4;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.SelectedIndexChanged += new System.EventHandler(this.lvEmployees_SelectedIndexChanged);
            this.lvEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployees_ColumnClick);
            // 
            // gbDays
            // 
            this.gbDays.Controls.Add(this.btnAdd);
            this.gbDays.Controls.Add(this.lblRemoveDay);
            this.gbDays.Controls.Add(this.lvDays);
            this.gbDays.Controls.Add(this.rbCertainDays);
            this.gbDays.Controls.Add(this.dtpTo);
            this.gbDays.Controls.Add(this.lblTo);
            this.gbDays.Controls.Add(this.dtpFrom);
            this.gbDays.Controls.Add(this.lblFrom);
            this.gbDays.Controls.Add(this.rbPeriod);
            this.gbDays.Location = new System.Drawing.Point(12, 313);
            this.gbDays.Name = "gbDays";
            this.gbDays.Size = new System.Drawing.Size(297, 275);
            this.gbDays.TabIndex = 23;
            this.gbDays.TabStop = false;
            this.gbDays.Text = "Days";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(153, 210);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 31;
            this.btnAdd.Text = "Add ";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblRemoveDay
            // 
            this.lblRemoveDay.Location = new System.Drawing.Point(26, 245);
            this.lblRemoveDay.Name = "lblRemoveDay";
            this.lblRemoveDay.Size = new System.Drawing.Size(212, 23);
            this.lblRemoveDay.TabIndex = 30;
            this.lblRemoveDay.Text = "*Dbl click to remove day from list";
            this.lblRemoveDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvDays
            // 
            this.lvDays.FullRowSelect = true;
            this.lvDays.GridLines = true;
            this.lvDays.HideSelection = false;
            this.lvDays.Location = new System.Drawing.Point(29, 128);
            this.lvDays.Name = "lvDays";
            this.lvDays.Size = new System.Drawing.Size(118, 105);
            this.lvDays.TabIndex = 24;
            this.lvDays.UseCompatibleStateImageBehavior = false;
            this.lvDays.View = System.Windows.Forms.View.Details;
            this.lvDays.DoubleClick += new System.EventHandler(this.lvDays_DoubleClick);
            // 
            // rbCertainDays
            // 
            this.rbCertainDays.AutoSize = true;
            this.rbCertainDays.Location = new System.Drawing.Point(6, 105);
            this.rbCertainDays.Name = "rbCertainDays";
            this.rbCertainDays.Size = new System.Drawing.Size(83, 17);
            this.rbCertainDays.TabIndex = 29;
            this.rbCertainDays.TabStop = true;
            this.rbCertainDays.Text = "Certain days";
            this.rbCertainDays.UseVisualStyleBackColor = true;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(202, 52);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(80, 20);
            this.dtpTo.TabIndex = 28;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(153, 51);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(43, 23);
            this.lblTo.TabIndex = 27;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(67, 52);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(80, 20);
            this.dtpFrom.TabIndex = 26;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(6, 51);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(55, 23);
            this.lblFrom.TabIndex = 25;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rbPeriod
            // 
            this.rbPeriod.AutoSize = true;
            this.rbPeriod.Checked = true;
            this.rbPeriod.Location = new System.Drawing.Point(6, 19);
            this.rbPeriod.Name = "rbPeriod";
            this.rbPeriod.Size = new System.Drawing.Size(55, 17);
            this.rbPeriod.TabIndex = 24;
            this.rbPeriod.TabStop = true;
            this.rbPeriod.Text = "Period";
            this.rbPeriod.UseVisualStyleBackColor = true;
            this.rbPeriod.CheckedChanged += new System.EventHandler(this.rbPeriod_CheckedChanged);
            // 
            // gbPassType
            // 
            this.gbPassType.Controls.Add(this.cbPassType);
            this.gbPassType.Location = new System.Drawing.Point(315, 315);
            this.gbPassType.Name = "gbPassType";
            this.gbPassType.Size = new System.Drawing.Size(227, 72);
            this.gbPassType.TabIndex = 24;
            this.gbPassType.TabStop = false;
            this.gbPassType.Text = "Pass type";
            // 
            // cbPassType
            // 
            this.cbPassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPassType.Location = new System.Drawing.Point(6, 30);
            this.cbPassType.Name = "cbPassType";
            this.cbPassType.Size = new System.Drawing.Size(215, 21);
            this.cbPassType.TabIndex = 7;
            // 
            // gbSortOrder
            // 
            this.gbSortOrder.Controls.Add(this.rbDaySort);
            this.gbSortOrder.Controls.Add(this.rbEmplSort);
            this.gbSortOrder.Location = new System.Drawing.Point(315, 488);
            this.gbSortOrder.Name = "gbSortOrder";
            this.gbSortOrder.Size = new System.Drawing.Size(227, 100);
            this.gbSortOrder.TabIndex = 25;
            this.gbSortOrder.TabStop = false;
            this.gbSortOrder.Text = "Sort order";
            // 
            // rbDaySort
            // 
            this.rbDaySort.AutoSize = true;
            this.rbDaySort.Location = new System.Drawing.Point(6, 60);
            this.rbDaySort.Name = "rbDaySort";
            this.rbDaySort.Size = new System.Drawing.Size(78, 17);
            this.rbDaySort.TabIndex = 1;
            this.rbDaySort.TabStop = true;
            this.rbDaySort.Text = "Day sorting";
            this.rbDaySort.UseVisualStyleBackColor = true;
            // 
            // rbEmplSort
            // 
            this.rbEmplSort.AutoSize = true;
            this.rbEmplSort.Checked = true;
            this.rbEmplSort.Location = new System.Drawing.Point(6, 27);
            this.rbEmplSort.Name = "rbEmplSort";
            this.rbEmplSort.Size = new System.Drawing.Size(105, 17);
            this.rbEmplSort.TabIndex = 0;
            this.rbEmplSort.TabStop = true;
            this.rbEmplSort.Text = "Employee sorting";
            this.rbEmplSort.UseVisualStyleBackColor = true;
            this.rbEmplSort.CheckedChanged += new System.EventHandler(this.rbEmplSort_CheckedChanged);
            // 
            // gbDescription
            // 
            this.gbDescription.Controls.Add(this.tbDescription);
            this.gbDescription.Location = new System.Drawing.Point(315, 402);
            this.gbDescription.Name = "gbDescription";
            this.gbDescription.Size = new System.Drawing.Size(227, 72);
            this.gbDescription.TabIndex = 26;
            this.gbDescription.TabStop = false;
            this.gbDescription.Text = "Description";
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(6, 26);
            this.tbDescription.MaxLength = 500;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(215, 20);
            this.tbDescription.TabIndex = 12;
            // 
            // ExitPermissionsAddAdvanced
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 645);
            this.ControlBox = false;
            this.Controls.Add(this.gbDescription);
            this.Controls.Add(this.gbSortOrder);
            this.Controls.Add(this.gbPassType);
            this.Controls.Add(this.gbDays);
            this.Controls.Add(this.gbChooseEmpl);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "ExitPermissionsAddAdvanced";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ExitPermissionsAdvanced";
            this.Load += new System.EventHandler(this.ExitPermissionsAddAdvanced_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ExitPermissionsAddAdvanced_KeyUp);
            this.gbChooseEmpl.ResumeLayout(false);
            this.gbChooseEmpl.PerformLayout();
            this.gbDays.ResumeLayout(false);
            this.gbDays.PerformLayout();
            this.gbPassType.ResumeLayout(false);
            this.gbSortOrder.ResumeLayout(false);
            this.gbSortOrder.PerformLayout();
            this.gbDescription.ResumeLayout(false);
            this.gbDescription.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.GroupBox gbChooseEmpl;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.GroupBox gbDays;
        private System.Windows.Forms.RadioButton rbPeriod;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.RadioButton rbCertainDays;
        private System.Windows.Forms.Label lblRemoveDay;
        public System.Windows.Forms.ListView lvDays;
        private System.Windows.Forms.GroupBox gbPassType;
        private System.Windows.Forms.ComboBox cbPassType;
        private System.Windows.Forms.GroupBox gbSortOrder;
        private System.Windows.Forms.RadioButton rbDaySort;
        private System.Windows.Forms.RadioButton rbEmplSort;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.Button btnAdd;
        protected System.Windows.Forms.CheckBox chbHierarhicly;
        private System.Windows.Forms.GroupBox gbDescription;
        private System.Windows.Forms.TextBox tbDescription;
    }
}