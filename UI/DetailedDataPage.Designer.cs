namespace UI
{
    partial class DetailedDataPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetailedDataPage));
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnOUTree = new System.Windows.Forms.Button();
            this.cbOU = new System.Windows.Forms.ComboBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.rbOU = new System.Windows.Forms.RadioButton();
            this.rbWU = new System.Windows.Forms.RadioButton();
            this.gbEmployee = new System.Windows.Forms.GroupBox();
            this.cbSelectAllEmpl = new System.Windows.Forms.CheckBox();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.tbEmployee = new System.Windows.Forms.TextBox();
            this.gbWageType = new System.Windows.Forms.GroupBox();
            this.lvWageTypes = new System.Windows.Forms.ListView();
            this.tbTimeTo = new System.Windows.Forms.TextBox();
            this.tbTimeFrom = new System.Windows.Forms.TextBox();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.gbFilter.SuspendLayout();
            this.gbEmployee.SuspendLayout();
            this.gbWageType.SuspendLayout();
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
            this.gbFilter.Location = new System.Drawing.Point(12, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(472, 87);
            this.gbFilter.TabIndex = 2;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Filter";
            // 
            // btnOUTree
            // 
            this.btnOUTree.Enabled = false;
            this.btnOUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnOUTree.Image")));
            this.btnOUTree.Location = new System.Drawing.Point(427, 51);
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
            this.cbOU.Location = new System.Drawing.Point(183, 51);
            this.cbOU.Name = "cbOU";
            this.cbOU.Size = new System.Drawing.Size(238, 21);
            this.cbOU.TabIndex = 4;
            this.cbOU.SelectedIndexChanged += new System.EventHandler(this.cbOU_SelectedIndexChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(427, 16);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 2;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(183, 18);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(238, 21);
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
            // gbEmployee
            // 
            this.gbEmployee.Controls.Add(this.cbSelectAllEmpl);
            this.gbEmployee.Controls.Add(this.lvEmployees);
            this.gbEmployee.Controls.Add(this.tbEmployee);
            this.gbEmployee.Location = new System.Drawing.Point(12, 105);
            this.gbEmployee.Name = "gbEmployee";
            this.gbEmployee.Size = new System.Drawing.Size(200, 327);
            this.gbEmployee.TabIndex = 20;
            this.gbEmployee.TabStop = false;
            this.gbEmployee.Text = "Employee";
            // 
            // cbSelectAllEmpl
            // 
            this.cbSelectAllEmpl.AutoSize = true;
            this.cbSelectAllEmpl.Location = new System.Drawing.Point(111, 302);
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
            this.lvEmployees.Size = new System.Drawing.Size(175, 245);
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
            // 
            // gbWageType
            // 
            this.gbWageType.Controls.Add(this.lvWageTypes);
            this.gbWageType.Location = new System.Drawing.Point(229, 114);
            this.gbWageType.Name = "gbWageType";
            this.gbWageType.Size = new System.Drawing.Size(255, 178);
            this.gbWageType.TabIndex = 21;
            this.gbWageType.TabStop = false;
            this.gbWageType.Text = "Wage type";
            // 
            // lvWageTypes
            // 
            this.lvWageTypes.FullRowSelect = true;
            this.lvWageTypes.GridLines = true;
            this.lvWageTypes.HideSelection = false;
            this.lvWageTypes.Location = new System.Drawing.Point(12, 18);
            this.lvWageTypes.Name = "lvWageTypes";
            this.lvWageTypes.ShowItemToolTips = true;
            this.lvWageTypes.Size = new System.Drawing.Size(237, 153);
            this.lvWageTypes.TabIndex = 3;
            this.lvWageTypes.UseCompatibleStateImageBehavior = false;
            this.lvWageTypes.View = System.Windows.Forms.View.Details;
            // 
            // tbTimeTo
            // 
            this.tbTimeTo.Location = new System.Drawing.Point(380, 329);
            this.tbTimeTo.Name = "tbTimeTo";
            this.tbTimeTo.Size = new System.Drawing.Size(44, 20);
            this.tbTimeTo.TabIndex = 27;
            // 
            // tbTimeFrom
            // 
            this.tbTimeFrom.Location = new System.Drawing.Point(380, 303);
            this.tbTimeFrom.Name = "tbTimeFrom";
            this.tbTimeFrom.Size = new System.Drawing.Size(44, 20);
            this.tbTimeFrom.TabIndex = 24;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(226, 333);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(40, 13);
            this.lblTo.TabIndex = 25;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(226, 309);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 13);
            this.lblFrom.TabIndex = 22;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(273, 329);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(100, 20);
            this.dtpTo.TabIndex = 26;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(273, 303);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(100, 20);
            this.dtpFrom.TabIndex = 23;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(354, 510);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 33);
            this.btnClose.TabIndex = 29;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(12, 510);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 33);
            this.btnSearch.TabIndex = 30;
            this.btnSearch.Text = "Export";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(56, 451);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(156, 23);
            this.lblTotal.TabIndex = 31;
            this.lblTotal.Text = "TOTAL:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTotal.Visible = false;
            // 
            // DetailedDataPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 555);
            this.ControlBox = false;
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tbTimeTo);
            this.Controls.Add(this.tbTimeFrom);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.gbWageType);
            this.Controls.Add(this.gbEmployee);
            this.Controls.Add(this.gbFilter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DetailedDataPage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DetailedDataPage";
            this.Load += new System.EventHandler(this.DetailedDataPage_Load);
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.gbEmployee.ResumeLayout(false);
            this.gbEmployee.PerformLayout();
            this.gbWageType.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox gbEmployee;
        private System.Windows.Forms.CheckBox cbSelectAllEmpl;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.TextBox tbEmployee;
        private System.Windows.Forms.GroupBox gbWageType;
        private System.Windows.Forms.ListView lvWageTypes;
        private System.Windows.Forms.TextBox tbTimeTo;
        private System.Windows.Forms.TextBox tbTimeFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblTotal;
    }
}