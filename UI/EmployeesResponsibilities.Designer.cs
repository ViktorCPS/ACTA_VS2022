namespace UI
{
    partial class EmployeesResponsibilities
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPreview = new System.Windows.Forms.TabPage();
            this.gbUnitPreview = new System.Windows.Forms.GroupBox();
            this.lblEmplForUnit = new System.Windows.Forms.Label();
            this.lvResEmployees = new System.Windows.Forms.ListView();
            this.gbType = new System.Windows.Forms.GroupBox();
            this.rbOU = new System.Windows.Forms.RadioButton();
            this.rbWU = new System.Windows.Forms.RadioButton();
            this.cbUnit = new System.Windows.Forms.ComboBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.gbEmplPreview = new System.Windows.Forms.GroupBox();
            this.lblOUForEmpl = new System.Windows.Forms.Label();
            this.lvOU = new System.Windows.Forms.ListView();
            this.cbEmpl = new System.Windows.Forms.ComboBox();
            this.lblWUForEmpl = new System.Windows.Forms.Label();
            this.lvWU = new System.Windows.Forms.ListView();
            this.lblEmpl = new System.Windows.Forms.Label();
            this.lblEmplID = new System.Windows.Forms.Label();
            this.tbEmplID = new System.Windows.Forms.TextBox();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lvResponsibilities = new System.Windows.Forms.ListView();
            this.lblEmployees = new System.Windows.Forms.Label();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.lblUnits = new System.Windows.Forms.Label();
            this.lvUnits = new System.Windows.Forms.ListView();
            this.gbUnitType = new System.Windows.Forms.GroupBox();
            this.rbOUType = new System.Windows.Forms.RadioButton();
            this.rbWUType = new System.Windows.Forms.RadioButton();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPreview.SuspendLayout();
            this.gbUnitPreview.SuspendLayout();
            this.gbType.SuspendLayout();
            this.gbEmplPreview.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.gbUnitType.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPreview);
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(959, 525);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPreview
            // 
            this.tabPreview.Controls.Add(this.gbUnitPreview);
            this.tabPreview.Controls.Add(this.gbEmplPreview);
            this.tabPreview.Location = new System.Drawing.Point(4, 22);
            this.tabPreview.Name = "tabPreview";
            this.tabPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPreview.Size = new System.Drawing.Size(951, 499);
            this.tabPreview.TabIndex = 0;
            this.tabPreview.Text = "Preview";
            this.tabPreview.UseVisualStyleBackColor = true;
            // 
            // gbUnitPreview
            // 
            this.gbUnitPreview.Controls.Add(this.lblEmplForUnit);
            this.gbUnitPreview.Controls.Add(this.lvResEmployees);
            this.gbUnitPreview.Controls.Add(this.gbType);
            this.gbUnitPreview.Controls.Add(this.cbUnit);
            this.gbUnitPreview.Controls.Add(this.lblUnit);
            this.gbUnitPreview.Location = new System.Drawing.Point(563, 16);
            this.gbUnitPreview.Name = "gbUnitPreview";
            this.gbUnitPreview.Size = new System.Drawing.Size(365, 465);
            this.gbUnitPreview.TabIndex = 1;
            this.gbUnitPreview.TabStop = false;
            this.gbUnitPreview.Text = "Unit preview";
            // 
            // lblEmplForUnit
            // 
            this.lblEmplForUnit.AutoSize = true;
            this.lblEmplForUnit.Location = new System.Drawing.Point(16, 129);
            this.lblEmplForUnit.Name = "lblEmplForUnit";
            this.lblEmplForUnit.Size = new System.Drawing.Size(139, 13);
            this.lblEmplForUnit.TabIndex = 3;
            this.lblEmplForUnit.Text = "Employees for selected unit:";
            this.lblEmplForUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvResEmployees
            // 
            this.lvResEmployees.FullRowSelect = true;
            this.lvResEmployees.GridLines = true;
            this.lvResEmployees.HideSelection = false;
            this.lvResEmployees.Location = new System.Drawing.Point(19, 145);
            this.lvResEmployees.Name = "lvResEmployees";
            this.lvResEmployees.ShowItemToolTips = true;
            this.lvResEmployees.Size = new System.Drawing.Size(329, 299);
            this.lvResEmployees.TabIndex = 4;
            this.lvResEmployees.UseCompatibleStateImageBehavior = false;
            this.lvResEmployees.View = System.Windows.Forms.View.Details;
            this.lvResEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvResEmployees_ColumnClick);
            // 
            // gbType
            // 
            this.gbType.Controls.Add(this.rbOU);
            this.gbType.Controls.Add(this.rbWU);
            this.gbType.Location = new System.Drawing.Point(19, 19);
            this.gbType.Name = "gbType";
            this.gbType.Size = new System.Drawing.Size(171, 62);
            this.gbType.TabIndex = 0;
            this.gbType.TabStop = false;
            this.gbType.Text = "Type";
            // 
            // rbOU
            // 
            this.rbOU.AutoSize = true;
            this.rbOU.Location = new System.Drawing.Point(18, 39);
            this.rbOU.Name = "rbOU";
            this.rbOU.Size = new System.Drawing.Size(117, 17);
            this.rbOU.TabIndex = 1;
            this.rbOU.TabStop = true;
            this.rbOU.Text = "Organizational units";
            this.rbOU.UseVisualStyleBackColor = true;
            this.rbOU.CheckedChanged += new System.EventHandler(this.rbOU_CheckedChanged);
            // 
            // rbWU
            // 
            this.rbWU.AutoSize = true;
            this.rbWU.Location = new System.Drawing.Point(18, 16);
            this.rbWU.Name = "rbWU";
            this.rbWU.Size = new System.Drawing.Size(90, 17);
            this.rbWU.TabIndex = 0;
            this.rbWU.TabStop = true;
            this.rbWU.Text = "Working units";
            this.rbWU.UseVisualStyleBackColor = true;
            this.rbWU.CheckedChanged += new System.EventHandler(this.rbWU_CheckedChanged);
            // 
            // cbUnit
            // 
            this.cbUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbUnit.Location = new System.Drawing.Point(65, 87);
            this.cbUnit.Name = "cbUnit";
            this.cbUnit.Size = new System.Drawing.Size(187, 21);
            this.cbUnit.TabIndex = 2;
            this.cbUnit.SelectedIndexChanged += new System.EventHandler(this.cbUnit_SelectedIndexChanged);
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(16, 90);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(29, 13);
            this.lblUnit.TabIndex = 1;
            this.lblUnit.Text = "Unit:";
            this.lblUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbEmplPreview
            // 
            this.gbEmplPreview.Controls.Add(this.lblOUForEmpl);
            this.gbEmplPreview.Controls.Add(this.lvOU);
            this.gbEmplPreview.Controls.Add(this.cbEmpl);
            this.gbEmplPreview.Controls.Add(this.lblWUForEmpl);
            this.gbEmplPreview.Controls.Add(this.lvWU);
            this.gbEmplPreview.Controls.Add(this.lblEmpl);
            this.gbEmplPreview.Controls.Add(this.lblEmplID);
            this.gbEmplPreview.Controls.Add(this.tbEmplID);
            this.gbEmplPreview.Location = new System.Drawing.Point(18, 16);
            this.gbEmplPreview.Name = "gbEmplPreview";
            this.gbEmplPreview.Size = new System.Drawing.Size(539, 465);
            this.gbEmplPreview.TabIndex = 0;
            this.gbEmplPreview.TabStop = false;
            this.gbEmplPreview.Text = "Employee preview";
            // 
            // lblOUForEmpl
            // 
            this.lblOUForEmpl.Location = new System.Drawing.Point(275, 101);
            this.lblOUForEmpl.Name = "lblOUForEmpl";
            this.lblOUForEmpl.Size = new System.Drawing.Size(258, 41);
            this.lblOUForEmpl.TabIndex = 6;
            this.lblOUForEmpl.Text = "Organizational units for selected employee:";
            this.lblOUForEmpl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvOU
            // 
            this.lvOU.FullRowSelect = true;
            this.lvOU.GridLines = true;
            this.lvOU.HideSelection = false;
            this.lvOU.Location = new System.Drawing.Point(278, 145);
            this.lvOU.Name = "lvOU";
            this.lvOU.ShowItemToolTips = true;
            this.lvOU.Size = new System.Drawing.Size(255, 299);
            this.lvOU.TabIndex = 7;
            this.lvOU.UseCompatibleStateImageBehavior = false;
            this.lvOU.View = System.Windows.Forms.View.Details;
            this.lvOU.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvOU_ColumnClick);
            // 
            // cbEmpl
            // 
            this.cbEmpl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmpl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmpl.Location = new System.Drawing.Point(148, 36);
            this.cbEmpl.Name = "cbEmpl";
            this.cbEmpl.Size = new System.Drawing.Size(224, 21);
            this.cbEmpl.TabIndex = 1;
            this.cbEmpl.SelectedIndexChanged += new System.EventHandler(this.cbEmpl_SelectedIndexChanged);
            // 
            // lblWUForEmpl
            // 
            this.lblWUForEmpl.Location = new System.Drawing.Point(4, 101);
            this.lblWUForEmpl.Name = "lblWUForEmpl";
            this.lblWUForEmpl.Size = new System.Drawing.Size(258, 41);
            this.lblWUForEmpl.TabIndex = 4;
            this.lblWUForEmpl.Text = "Working units for selected employee:";
            this.lblWUForEmpl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvWU
            // 
            this.lvWU.FullRowSelect = true;
            this.lvWU.GridLines = true;
            this.lvWU.HideSelection = false;
            this.lvWU.Location = new System.Drawing.Point(7, 145);
            this.lvWU.Name = "lvWU";
            this.lvWU.ShowItemToolTips = true;
            this.lvWU.Size = new System.Drawing.Size(255, 299);
            this.lvWU.TabIndex = 5;
            this.lvWU.UseCompatibleStateImageBehavior = false;
            this.lvWU.View = System.Windows.Forms.View.Details;
            this.lvWU.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvWU_ColumnClick);
            // 
            // lblEmpl
            // 
            this.lblEmpl.Location = new System.Drawing.Point(52, 36);
            this.lblEmpl.Name = "lblEmpl";
            this.lblEmpl.Size = new System.Drawing.Size(80, 23);
            this.lblEmpl.TabIndex = 0;
            this.lblEmpl.Text = "Employee:";
            this.lblEmpl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmplID
            // 
            this.lblEmplID.Location = new System.Drawing.Point(52, 68);
            this.lblEmplID.Name = "lblEmplID";
            this.lblEmplID.Size = new System.Drawing.Size(80, 23);
            this.lblEmplID.TabIndex = 2;
            this.lblEmplID.Text = "Employee ID:";
            this.lblEmplID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbEmplID
            // 
            this.tbEmplID.Enabled = false;
            this.tbEmplID.Location = new System.Drawing.Point(148, 68);
            this.tbEmplID.Name = "tbEmplID";
            this.tbEmplID.Size = new System.Drawing.Size(224, 20);
            this.tbEmplID.TabIndex = 3;
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.btnPrev);
            this.tabSettings.Controls.Add(this.btnNext);
            this.tabSettings.Controls.Add(this.btnSave);
            this.tabSettings.Controls.Add(this.btnRemove);
            this.tabSettings.Controls.Add(this.btnAdd);
            this.tabSettings.Controls.Add(this.lvResponsibilities);
            this.tabSettings.Controls.Add(this.lblEmployees);
            this.tabSettings.Controls.Add(this.lvEmployees);
            this.tabSettings.Controls.Add(this.lblUnits);
            this.tabSettings.Controls.Add(this.lvUnits);
            this.tabSettings.Controls.Add(this.gbUnitType);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(951, 499);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(221, 267);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(51, 23);
            this.btnPrev.TabIndex = 4;
            this.btnPrev.Text = "<";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(278, 267);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(51, 23);
            this.btnNext.TabIndex = 5;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(855, 465);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(486, 465);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(124, 23);
            this.btnRemove.TabIndex = 9;
            this.btnRemove.Text = "Remove selected";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(388, 262);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(51, 23);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvResponsibilities
            // 
            this.lvResponsibilities.FullRowSelect = true;
            this.lvResponsibilities.GridLines = true;
            this.lvResponsibilities.HideSelection = false;
            this.lvResponsibilities.Location = new System.Drawing.Point(486, 21);
            this.lvResponsibilities.Name = "lvResponsibilities";
            this.lvResponsibilities.ShowItemToolTips = true;
            this.lvResponsibilities.Size = new System.Drawing.Size(444, 438);
            this.lvResponsibilities.TabIndex = 8;
            this.lvResponsibilities.UseCompatibleStateImageBehavior = false;
            this.lvResponsibilities.View = System.Windows.Forms.View.Details;
            this.lvResponsibilities.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvResponsibilities_ColumnClick);
            // 
            // lblEmployees
            // 
            this.lblEmployees.AutoSize = true;
            this.lblEmployees.Location = new System.Drawing.Point(6, 272);
            this.lblEmployees.Name = "lblEmployees";
            this.lblEmployees.Size = new System.Drawing.Size(55, 13);
            this.lblEmployees.TabIndex = 3;
            this.lblEmployees.Text = "Emloyees:";
            this.lblEmployees.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(6, 296);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.ShowItemToolTips = true;
            this.lvEmployees.Size = new System.Drawing.Size(323, 192);
            this.lvEmployees.TabIndex = 6;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            // 
            // lblUnits
            // 
            this.lblUnits.AutoSize = true;
            this.lblUnits.Location = new System.Drawing.Point(6, 70);
            this.lblUnits.Name = "lblUnits";
            this.lblUnits.Size = new System.Drawing.Size(34, 13);
            this.lblUnits.TabIndex = 1;
            this.lblUnits.Text = "Units:";
            this.lblUnits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvUnits
            // 
            this.lvUnits.FullRowSelect = true;
            this.lvUnits.GridLines = true;
            this.lvUnits.HideSelection = false;
            this.lvUnits.Location = new System.Drawing.Point(8, 86);
            this.lvUnits.Name = "lvUnits";
            this.lvUnits.ShowItemToolTips = true;
            this.lvUnits.Size = new System.Drawing.Size(321, 175);
            this.lvUnits.TabIndex = 2;
            this.lvUnits.UseCompatibleStateImageBehavior = false;
            this.lvUnits.View = System.Windows.Forms.View.Details;
            this.lvUnits.SelectedIndexChanged += new System.EventHandler(this.lvUnits_SelectedIndexChanged);
            this.lvUnits.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvUnits_ColumnClick);
            // 
            // gbUnitType
            // 
            this.gbUnitType.Controls.Add(this.rbOUType);
            this.gbUnitType.Controls.Add(this.rbWUType);
            this.gbUnitType.Location = new System.Drawing.Point(6, 6);
            this.gbUnitType.Name = "gbUnitType";
            this.gbUnitType.Size = new System.Drawing.Size(171, 60);
            this.gbUnitType.TabIndex = 0;
            this.gbUnitType.TabStop = false;
            this.gbUnitType.Text = "Type";
            // 
            // rbOUType
            // 
            this.rbOUType.AutoSize = true;
            this.rbOUType.Location = new System.Drawing.Point(18, 38);
            this.rbOUType.Name = "rbOUType";
            this.rbOUType.Size = new System.Drawing.Size(117, 17);
            this.rbOUType.TabIndex = 1;
            this.rbOUType.TabStop = true;
            this.rbOUType.Text = "Organizational units";
            this.rbOUType.UseVisualStyleBackColor = true;
            this.rbOUType.CheckedChanged += new System.EventHandler(this.rbOUType_CheckedChanged);
            // 
            // rbWUType
            // 
            this.rbWUType.AutoSize = true;
            this.rbWUType.Location = new System.Drawing.Point(18, 15);
            this.rbWUType.Name = "rbWUType";
            this.rbWUType.Size = new System.Drawing.Size(90, 17);
            this.rbWUType.TabIndex = 0;
            this.rbWUType.TabStop = true;
            this.rbWUType.Text = "Working units";
            this.rbWUType.UseVisualStyleBackColor = true;
            this.rbWUType.CheckedChanged += new System.EventHandler(this.rbWUType_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(892, 543);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // EmployeesResponsibilities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 585);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "EmployeesResponsibilities";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Employees Responsibilities";
            this.Load += new System.EventHandler(this.EmployeesResponsibilities_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPreview.ResumeLayout(false);
            this.gbUnitPreview.ResumeLayout(false);
            this.gbUnitPreview.PerformLayout();
            this.gbType.ResumeLayout(false);
            this.gbType.PerformLayout();
            this.gbEmplPreview.ResumeLayout(false);
            this.gbEmplPreview.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.gbUnitType.ResumeLayout(false);
            this.gbUnitType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPreview;
        private System.Windows.Forms.TabPage tabSettings;        
        private System.Windows.Forms.ListView lvResponsibilities;
        private System.Windows.Forms.Label lblEmployees;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.Label lblUnits;
        private System.Windows.Forms.ListView lvUnits;
        private System.Windows.Forms.GroupBox gbUnitType;
        private System.Windows.Forms.RadioButton rbOUType;
        private System.Windows.Forms.RadioButton rbWUType;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox gbUnitPreview;
        private System.Windows.Forms.Label lblEmplForUnit;
        private System.Windows.Forms.ListView lvResEmployees;
        private System.Windows.Forms.GroupBox gbType;
        private System.Windows.Forms.RadioButton rbOU;
        private System.Windows.Forms.RadioButton rbWU;
        private System.Windows.Forms.ComboBox cbUnit;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.GroupBox gbEmplPreview;
        private System.Windows.Forms.Label lblOUForEmpl;
        private System.Windows.Forms.ListView lvOU;
        private System.Windows.Forms.ComboBox cbEmpl;
        private System.Windows.Forms.Label lblWUForEmpl;
        private System.Windows.Forms.ListView lvWU;
        private System.Windows.Forms.Label lblEmpl;
        private System.Windows.Forms.Label lblEmplID;
        private System.Windows.Forms.TextBox tbEmplID;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;

    }
}