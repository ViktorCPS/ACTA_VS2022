namespace UI
{
    partial class EmployeesXWorkingUnits
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeesXWorkingUnits));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageEmployees = new System.Windows.Forms.TabPage();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.gbWU = new System.Windows.Forms.GroupBox();
            this.tbSelWUDesc = new System.Windows.Forms.TextBox();
            this.lblSelWUDesc = new System.Windows.Forms.Label();
            this.lblSelWUName = new System.Windows.Forms.Label();
            this.tbSelWUName = new System.Windows.Forms.TextBox();
            this.tbWorkingUnitDesc = new System.Windows.Forms.TextBox();
            this.lblWorkingUnitDesc = new System.Windows.Forms.Label();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.tabPageWorkingUnits = new System.Windows.Forms.TabPage();
            this.lvEmployee = new System.Windows.Forms.ListView();
            this.lblEmployeeForWU = new System.Windows.Forms.Label();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.cbWUName = new System.Windows.Forms.ComboBox();
            this.lblWUName = new System.Windows.Forms.Label();
            this.tabPageEmployeesXWorkingUnitss = new System.Windows.Forms.TabPage();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.btnAddAll = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lvSelectedEmployees = new System.Windows.Forms.ListView();
            this.lblSelEmployees = new System.Windows.Forms.Label();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.lblEmployeesForWU = new System.Windows.Forms.Label();
            this.tbWUDesc = new System.Windows.Forms.TextBox();
            this.lblWUDesc = new System.Windows.Forms.Label();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.lblWU = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.btnWUTree1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPageEmployees.SuspendLayout();
            this.gbWU.SuspendLayout();
            this.tabPageWorkingUnits.SuspendLayout();
            this.tabPageEmployeesXWorkingUnitss.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageEmployees);
            this.tabControl1.Controls.Add(this.tabPageWorkingUnits);
            this.tabControl1.Controls.Add(this.tabPageEmployeesXWorkingUnitss);
            this.tabControl1.Location = new System.Drawing.Point(8, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(616, 408);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageEmployees
            // 
            this.tabPageEmployees.Controls.Add(this.btnWUTree);
            this.tabPageEmployees.Controls.Add(this.cbEmployee);
            this.tabPageEmployees.Controls.Add(this.lblEmployee);
            this.tabPageEmployees.Controls.Add(this.gbWU);
            this.tabPageEmployees.Controls.Add(this.tbWorkingUnitDesc);
            this.tabPageEmployees.Controls.Add(this.lblWorkingUnitDesc);
            this.tabPageEmployees.Controls.Add(this.cbWorkingUnit);
            this.tabPageEmployees.Controls.Add(this.lblWorkingUnit);
            this.tabPageEmployees.Location = new System.Drawing.Point(4, 22);
            this.tabPageEmployees.Name = "tabPageEmployees";
            this.tabPageEmployees.Size = new System.Drawing.Size(608, 382);
            this.tabPageEmployees.TabIndex = 0;
            this.tabPageEmployees.Text = "Employees";
            // 
            // cbEmployee
            // 
            this.cbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmployee.Location = new System.Drawing.Point(128, 88);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(224, 21);
            this.cbEmployee.TabIndex = 5;
            this.cbEmployee.SelectedIndexChanged += new System.EventHandler(this.cbEmployee_SelectedIndexChanged);
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(18, 86);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(104, 23);
            this.lblEmployee.TabIndex = 4;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbWU
            // 
            this.gbWU.Controls.Add(this.tbSelWUDesc);
            this.gbWU.Controls.Add(this.lblSelWUDesc);
            this.gbWU.Controls.Add(this.lblSelWUName);
            this.gbWU.Controls.Add(this.tbSelWUName);
            this.gbWU.Location = new System.Drawing.Point(16, 160);
            this.gbWU.Name = "gbWU";
            this.gbWU.Size = new System.Drawing.Size(576, 120);
            this.gbWU.TabIndex = 6;
            this.gbWU.TabStop = false;
            this.gbWU.Text = "Working unit that selected employee belongs to";
            // 
            // tbSelWUDesc
            // 
            this.tbSelWUDesc.Enabled = false;
            this.tbSelWUDesc.Location = new System.Drawing.Point(112, 72);
            this.tbSelWUDesc.Name = "tbSelWUDesc";
            this.tbSelWUDesc.Size = new System.Drawing.Size(432, 20);
            this.tbSelWUDesc.TabIndex = 10;
            // 
            // lblSelWUDesc
            // 
            this.lblSelWUDesc.Location = new System.Drawing.Point(26, 69);
            this.lblSelWUDesc.Name = "lblSelWUDesc";
            this.lblSelWUDesc.Size = new System.Drawing.Size(80, 23);
            this.lblSelWUDesc.TabIndex = 9;
            this.lblSelWUDesc.Text = "Description:";
            this.lblSelWUDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSelWUName
            // 
            this.lblSelWUName.Location = new System.Drawing.Point(26, 37);
            this.lblSelWUName.Name = "lblSelWUName";
            this.lblSelWUName.Size = new System.Drawing.Size(80, 23);
            this.lblSelWUName.TabIndex = 7;
            this.lblSelWUName.Text = "Name:";
            this.lblSelWUName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbSelWUName
            // 
            this.tbSelWUName.Enabled = false;
            this.tbSelWUName.Location = new System.Drawing.Point(112, 40);
            this.tbSelWUName.Name = "tbSelWUName";
            this.tbSelWUName.Size = new System.Drawing.Size(432, 20);
            this.tbSelWUName.TabIndex = 8;
            // 
            // tbWorkingUnitDesc
            // 
            this.tbWorkingUnitDesc.Enabled = false;
            this.tbWorkingUnitDesc.Location = new System.Drawing.Point(128, 56);
            this.tbWorkingUnitDesc.Name = "tbWorkingUnitDesc";
            this.tbWorkingUnitDesc.Size = new System.Drawing.Size(224, 20);
            this.tbWorkingUnitDesc.TabIndex = 3;
            // 
            // lblWorkingUnitDesc
            // 
            this.lblWorkingUnitDesc.Location = new System.Drawing.Point(18, 53);
            this.lblWorkingUnitDesc.Name = "lblWorkingUnitDesc";
            this.lblWorkingUnitDesc.Size = new System.Drawing.Size(104, 23);
            this.lblWorkingUnitDesc.TabIndex = 2;
            this.lblWorkingUnitDesc.Text = "Description:";
            this.lblWorkingUnitDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(128, 24);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(224, 21);
            this.cbWorkingUnit.TabIndex = 1;
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(18, 22);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(104, 23);
            this.lblWorkingUnit.TabIndex = 0;
            this.lblWorkingUnit.Text = "Working unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPageWorkingUnits
            // 
            this.tabPageWorkingUnits.Controls.Add(this.lvEmployee);
            this.tabPageWorkingUnits.Controls.Add(this.lblEmployeeForWU);
            this.tabPageWorkingUnits.Controls.Add(this.tbDesc);
            this.tabPageWorkingUnits.Controls.Add(this.lblDesc);
            this.tabPageWorkingUnits.Controls.Add(this.cbWUName);
            this.tabPageWorkingUnits.Controls.Add(this.lblWUName);
            this.tabPageWorkingUnits.Location = new System.Drawing.Point(4, 22);
            this.tabPageWorkingUnits.Name = "tabPageWorkingUnits";
            this.tabPageWorkingUnits.Size = new System.Drawing.Size(608, 382);
            this.tabPageWorkingUnits.TabIndex = 1;
            this.tabPageWorkingUnits.Text = "Working units";
            // 
            // lvEmployee
            // 
            this.lvEmployee.FullRowSelect = true;
            this.lvEmployee.GridLines = true;
            this.lvEmployee.HideSelection = false;
            this.lvEmployee.Location = new System.Drawing.Point(16, 152);
            this.lvEmployee.Name = "lvEmployee";
            this.lvEmployee.Size = new System.Drawing.Size(563, 200);
            this.lvEmployee.TabIndex = 16;
            this.lvEmployee.UseCompatibleStateImageBehavior = false;
            this.lvEmployee.View = System.Windows.Forms.View.Details;
            this.lvEmployee.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployee_ColumnClick);
            // 
            // lblEmployeeForWU
            // 
            this.lblEmployeeForWU.Location = new System.Drawing.Point(16, 96);
            this.lblEmployeeForWU.Name = "lblEmployeeForWU";
            this.lblEmployeeForWU.Size = new System.Drawing.Size(344, 48);
            this.lblEmployeeForWU.TabIndex = 15;
            this.lblEmployeeForWU.Text = "List of all employees who belong to selected working unit:";
            this.lblEmployeeForWU.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbDesc
            // 
            this.tbDesc.Enabled = false;
            this.tbDesc.Location = new System.Drawing.Point(112, 56);
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(224, 20);
            this.tbDesc.TabIndex = 14;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(34, 53);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(72, 23);
            this.lblDesc.TabIndex = 13;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWUName
            // 
            this.cbWUName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWUName.Location = new System.Drawing.Point(112, 24);
            this.cbWUName.Name = "cbWUName";
            this.cbWUName.Size = new System.Drawing.Size(224, 21);
            this.cbWUName.TabIndex = 12;
            this.cbWUName.SelectedIndexChanged += new System.EventHandler(this.cbWUName_SelectedIndexChanged);
            // 
            // lblWUName
            // 
            this.lblWUName.Location = new System.Drawing.Point(34, 22);
            this.lblWUName.Name = "lblWUName";
            this.lblWUName.Size = new System.Drawing.Size(72, 23);
            this.lblWUName.TabIndex = 11;
            this.lblWUName.Text = "Name:";
            this.lblWUName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPageEmployeesXWorkingUnitss
            // 
            this.tabPageEmployeesXWorkingUnitss.Controls.Add(this.btnWUTree1);
            this.tabPageEmployeesXWorkingUnitss.Controls.Add(this.btnRemove);
            this.tabPageEmployeesXWorkingUnitss.Controls.Add(this.btnRemoveAll);
            this.tabPageEmployeesXWorkingUnitss.Controls.Add(this.btnAddAll);
            this.tabPageEmployeesXWorkingUnitss.Controls.Add(this.btnAdd);
            this.tabPageEmployeesXWorkingUnitss.Controls.Add(this.btnCancel);
            this.tabPageEmployeesXWorkingUnitss.Controls.Add(this.btnSave);
            this.tabPageEmployeesXWorkingUnitss.Controls.Add(this.lvSelectedEmployees);
            this.tabPageEmployeesXWorkingUnitss.Controls.Add(this.lblSelEmployees);
            this.tabPageEmployeesXWorkingUnitss.Controls.Add(this.lvEmployees);
            this.tabPageEmployeesXWorkingUnitss.Controls.Add(this.lblEmployeesForWU);
            this.tabPageEmployeesXWorkingUnitss.Controls.Add(this.tbWUDesc);
            this.tabPageEmployeesXWorkingUnitss.Controls.Add(this.lblWUDesc);
            this.tabPageEmployeesXWorkingUnitss.Controls.Add(this.cbWU);
            this.tabPageEmployeesXWorkingUnitss.Controls.Add(this.lblWU);
            this.tabPageEmployeesXWorkingUnitss.Location = new System.Drawing.Point(4, 22);
            this.tabPageEmployeesXWorkingUnitss.Name = "tabPageEmployeesXWorkingUnitss";
            this.tabPageEmployeesXWorkingUnitss.Size = new System.Drawing.Size(608, 382);
            this.tabPageEmployeesXWorkingUnitss.TabIndex = 2;
            this.tabPageEmployeesXWorkingUnitss.Text = "Employee <-> Working units";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(280, 272);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(48, 23);
            this.btnRemove.TabIndex = 27;
            this.btnRemove.Text = "<";
            this.btnRemove.Visible = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Location = new System.Drawing.Point(280, 240);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(48, 23);
            this.btnRemoveAll.TabIndex = 26;
            this.btnRemoveAll.Text = "<<";
            this.btnRemoveAll.Visible = false;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // btnAddAll
            // 
            this.btnAddAll.Location = new System.Drawing.Point(280, 208);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(48, 23);
            this.btnAddAll.TabIndex = 25;
            this.btnAddAll.Text = ">>";
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(280, 176);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(48, 23);
            this.btnAdd.TabIndex = 24;
            this.btnAdd.Text = ">";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(520, 344);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 344);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 30;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lvSelectedEmployees
            // 
            this.lvSelectedEmployees.FullRowSelect = true;
            this.lvSelectedEmployees.GridLines = true;
            this.lvSelectedEmployees.HideSelection = false;
            this.lvSelectedEmployees.Location = new System.Drawing.Point(344, 136);
            this.lvSelectedEmployees.Name = "lvSelectedEmployees";
            this.lvSelectedEmployees.Size = new System.Drawing.Size(248, 200);
            this.lvSelectedEmployees.TabIndex = 29;
            this.lvSelectedEmployees.UseCompatibleStateImageBehavior = false;
            this.lvSelectedEmployees.View = System.Windows.Forms.View.Details;
            this.lvSelectedEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvSelectedEmployees_ColumnClick);
            // 
            // lblSelEmployees
            // 
            this.lblSelEmployees.Location = new System.Drawing.Point(344, 80);
            this.lblSelEmployees.Name = "lblSelEmployees";
            this.lblSelEmployees.Size = new System.Drawing.Size(248, 48);
            this.lblSelEmployees.TabIndex = 28;
            this.lblSelEmployees.Text = "List of all employees who belong to selected working unit:";
            this.lblSelEmployees.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(16, 136);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.Size = new System.Drawing.Size(248, 200);
            this.lvEmployees.TabIndex = 23;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployees_ColumnClick);
            // 
            // lblEmployeesForWU
            // 
            this.lblEmployeesForWU.Location = new System.Drawing.Point(16, 80);
            this.lblEmployeesForWU.Name = "lblEmployeesForWU";
            this.lblEmployeesForWU.Size = new System.Drawing.Size(248, 48);
            this.lblEmployeesForWU.TabIndex = 22;
            this.lblEmployeesForWU.Text = "List of all employees who do not belong to selected working unit:";
            this.lblEmployeesForWU.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbWUDesc
            // 
            this.tbWUDesc.Enabled = false;
            this.tbWUDesc.Location = new System.Drawing.Point(184, 48);
            this.tbWUDesc.Name = "tbWUDesc";
            this.tbWUDesc.Size = new System.Drawing.Size(240, 20);
            this.tbWUDesc.TabIndex = 21;
            // 
            // lblWUDesc
            // 
            this.lblWUDesc.Location = new System.Drawing.Point(58, 45);
            this.lblWUDesc.Name = "lblWUDesc";
            this.lblWUDesc.Size = new System.Drawing.Size(120, 23);
            this.lblWUDesc.TabIndex = 20;
            this.lblWUDesc.Text = "Description:";
            this.lblWUDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.Location = new System.Drawing.Point(184, 16);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(240, 21);
            this.cbWU.TabIndex = 19;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(58, 14);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(120, 23);
            this.lblWU.TabIndex = 18;
            this.lblWU.Text = "Working unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(552, 424);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(358, 22);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 42;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // btnWUTree1
            // 
            this.btnWUTree1.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree1.Image")));
            this.btnWUTree1.Location = new System.Drawing.Point(430, 14);
            this.btnWUTree1.Name = "btnWUTree1";
            this.btnWUTree1.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree1.TabIndex = 43;
            this.btnWUTree1.Click += new System.EventHandler(this.btnWUTree1_Click);
            // 
            // EmployeesXWorkingUnits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 454);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 488);
            this.MinimumSize = new System.Drawing.Size(640, 488);
            this.Name = "EmployeesXWorkingUnits";
            this.ShowInTaskbar = false;
            this.Text = "Employees <-> Working units";
            this.Load += new System.EventHandler(this.EmployeesXWU_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmployeesXWorkingUnits_KeyUp);
            this.tabControl1.ResumeLayout(false);
            this.tabPageEmployees.ResumeLayout(false);
            this.tabPageEmployees.PerformLayout();
            this.gbWU.ResumeLayout(false);
            this.gbWU.PerformLayout();
            this.tabPageWorkingUnits.ResumeLayout(false);
            this.tabPageWorkingUnits.PerformLayout();
            this.tabPageEmployeesXWorkingUnitss.ResumeLayout(false);
            this.tabPageEmployeesXWorkingUnitss.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageEmployees;
        private System.Windows.Forms.TabPage tabPageWorkingUnits;
        private System.Windows.Forms.TabPage tabPageEmployeesXWorkingUnitss;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.Label lblWUDesc;
        private System.Windows.Forms.TextBox tbWUDesc;
        private System.Windows.Forms.Label lblEmployeesForWU;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.Label lblSelEmployees;
        private System.Windows.Forms.ListView lvSelectedEmployees;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnRemoveAll;
        private System.Windows.Forms.Button btnAddAll;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblWUName;
        private System.Windows.Forms.ComboBox cbWUName;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox tbDesc;
        private System.Windows.Forms.Label lblEmployeeForWU;
        private System.Windows.Forms.ListView lvEmployee;
        private System.Windows.Forms.GroupBox gbWU;
        private System.Windows.Forms.Label lblSelWUDesc;
        private System.Windows.Forms.TextBox tbSelWUDesc;
        private System.Windows.Forms.TextBox tbSelWUName;
        private System.Windows.Forms.Label lblSelWUName;
        private System.Windows.Forms.Label lblWorkingUnit;
        private System.Windows.Forms.ComboBox cbWorkingUnit;
        private System.Windows.Forms.Label lblWorkingUnitDesc;
        private System.Windows.Forms.TextBox tbWorkingUnitDesc;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.Button btnWUTree1;
    }
}