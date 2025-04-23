namespace UI
{
    partial class EmployeesXOrganizationalUnits
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeesXOrganizationalUnits));
            this.btnRemove = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageEmployees = new System.Windows.Forms.TabPage();
            this.btnOUTree = new System.Windows.Forms.Button();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.gbOU = new System.Windows.Forms.GroupBox();
            this.tbSelOUDesc = new System.Windows.Forms.TextBox();
            this.lblSelOUDesc = new System.Windows.Forms.Label();
            this.lblSelOUName = new System.Windows.Forms.Label();
            this.tbSelOUName = new System.Windows.Forms.TextBox();
            this.tbOrgUnitDesc = new System.Windows.Forms.TextBox();
            this.lblOrgUnitDesc = new System.Windows.Forms.Label();
            this.cbOrgUnit = new System.Windows.Forms.ComboBox();
            this.lblOrgUnit = new System.Windows.Forms.Label();
            this.tabPageOrgUnits = new System.Windows.Forms.TabPage();
            this.lvEmployee = new System.Windows.Forms.ListView();
            this.lblEmployeeForOU = new System.Windows.Forms.Label();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.cbOUName = new System.Windows.Forms.ComboBox();
            this.lblOUName = new System.Windows.Forms.Label();
            this.tabPageEmployeesXOrgUnits = new System.Windows.Forms.TabPage();
            this.btnOUTree1 = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.btnAddAll = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lvSelectedEmployees = new System.Windows.Forms.ListView();
            this.lblSelEmployees = new System.Windows.Forms.Label();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.lblEmployeesForOU = new System.Windows.Forms.Label();
            this.tbOUDesc = new System.Windows.Forms.TextBox();
            this.lblOUDesc = new System.Windows.Forms.Label();
            this.cbOU = new System.Windows.Forms.ComboBox();
            this.lblOU = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPageEmployees.SuspendLayout();
            this.gbOU.SuspendLayout();
            this.tabPageOrgUnits.SuspendLayout();
            this.tabPageEmployeesXOrgUnits.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(280, 272);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(48, 23);
            this.btnRemove.TabIndex = 10;
            this.btnRemove.Text = "<";
            this.btnRemove.Visible = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageEmployees);
            this.tabControl1.Controls.Add(this.tabPageOrgUnits);
            this.tabControl1.Controls.Add(this.tabPageEmployeesXOrgUnits);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(616, 408);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageEmployees
            // 
            this.tabPageEmployees.Controls.Add(this.btnOUTree);
            this.tabPageEmployees.Controls.Add(this.cbEmployee);
            this.tabPageEmployees.Controls.Add(this.lblEmployee);
            this.tabPageEmployees.Controls.Add(this.gbOU);
            this.tabPageEmployees.Controls.Add(this.tbOrgUnitDesc);
            this.tabPageEmployees.Controls.Add(this.lblOrgUnitDesc);
            this.tabPageEmployees.Controls.Add(this.cbOrgUnit);
            this.tabPageEmployees.Controls.Add(this.lblOrgUnit);
            this.tabPageEmployees.Location = new System.Drawing.Point(4, 22);
            this.tabPageEmployees.Name = "tabPageEmployees";
            this.tabPageEmployees.Size = new System.Drawing.Size(608, 382);
            this.tabPageEmployees.TabIndex = 0;
            this.tabPageEmployees.Text = "Employees";
            // 
            // btnOUTree
            // 
            this.btnOUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnOUTree.Image")));
            this.btnOUTree.Location = new System.Drawing.Point(405, 22);
            this.btnOUTree.Name = "btnOUTree";
            this.btnOUTree.Size = new System.Drawing.Size(25, 23);
            this.btnOUTree.TabIndex = 2;
            this.btnOUTree.Click += new System.EventHandler(this.btnOUTree_Click);
            // 
            // cbEmployee
            // 
            this.cbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmployee.Location = new System.Drawing.Point(175, 88);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(224, 21);
            this.cbEmployee.TabIndex = 6;
            this.cbEmployee.SelectedIndexChanged += new System.EventHandler(this.cbEmployee_SelectedIndexChanged);
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(65, 87);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(104, 23);
            this.lblEmployee.TabIndex = 5;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbOU
            // 
            this.gbOU.Controls.Add(this.tbSelOUDesc);
            this.gbOU.Controls.Add(this.lblSelOUDesc);
            this.gbOU.Controls.Add(this.lblSelOUName);
            this.gbOU.Controls.Add(this.tbSelOUName);
            this.gbOU.Location = new System.Drawing.Point(16, 160);
            this.gbOU.Name = "gbOU";
            this.gbOU.Size = new System.Drawing.Size(576, 120);
            this.gbOU.TabIndex = 7;
            this.gbOU.TabStop = false;
            this.gbOU.Text = "Organizational unit that selected employee belongs to";
            // 
            // tbSelOUDesc
            // 
            this.tbSelOUDesc.Enabled = false;
            this.tbSelOUDesc.Location = new System.Drawing.Point(112, 72);
            this.tbSelOUDesc.Name = "tbSelOUDesc";
            this.tbSelOUDesc.Size = new System.Drawing.Size(432, 20);
            this.tbSelOUDesc.TabIndex = 3;
            // 
            // lblSelOUDesc
            // 
            this.lblSelOUDesc.Location = new System.Drawing.Point(26, 69);
            this.lblSelOUDesc.Name = "lblSelOUDesc";
            this.lblSelOUDesc.Size = new System.Drawing.Size(80, 23);
            this.lblSelOUDesc.TabIndex = 2;
            this.lblSelOUDesc.Text = "Description:";
            this.lblSelOUDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSelOUName
            // 
            this.lblSelOUName.Location = new System.Drawing.Point(26, 37);
            this.lblSelOUName.Name = "lblSelOUName";
            this.lblSelOUName.Size = new System.Drawing.Size(80, 23);
            this.lblSelOUName.TabIndex = 0;
            this.lblSelOUName.Text = "Name:";
            this.lblSelOUName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbSelOUName
            // 
            this.tbSelOUName.Enabled = false;
            this.tbSelOUName.Location = new System.Drawing.Point(112, 40);
            this.tbSelOUName.Name = "tbSelOUName";
            this.tbSelOUName.Size = new System.Drawing.Size(432, 20);
            this.tbSelOUName.TabIndex = 1;
            // 
            // tbOrgUnitDesc
            // 
            this.tbOrgUnitDesc.Enabled = false;
            this.tbOrgUnitDesc.Location = new System.Drawing.Point(175, 56);
            this.tbOrgUnitDesc.Name = "tbOrgUnitDesc";
            this.tbOrgUnitDesc.Size = new System.Drawing.Size(224, 20);
            this.tbOrgUnitDesc.TabIndex = 4;
            // 
            // lblOrgUnitDesc
            // 
            this.lblOrgUnitDesc.Location = new System.Drawing.Point(65, 54);
            this.lblOrgUnitDesc.Name = "lblOrgUnitDesc";
            this.lblOrgUnitDesc.Size = new System.Drawing.Size(104, 23);
            this.lblOrgUnitDesc.TabIndex = 3;
            this.lblOrgUnitDesc.Text = "Description:";
            this.lblOrgUnitDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbOrgUnit
            // 
            this.cbOrgUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOrgUnit.Location = new System.Drawing.Point(175, 24);
            this.cbOrgUnit.Name = "cbOrgUnit";
            this.cbOrgUnit.Size = new System.Drawing.Size(224, 21);
            this.cbOrgUnit.TabIndex = 1;
            this.cbOrgUnit.SelectedIndexChanged += new System.EventHandler(this.cbOrgUnit_SelectedIndexChanged);
            // 
            // lblOrgUnit
            // 
            this.lblOrgUnit.Location = new System.Drawing.Point(16, 23);
            this.lblOrgUnit.Name = "lblOrgUnit";
            this.lblOrgUnit.Size = new System.Drawing.Size(153, 23);
            this.lblOrgUnit.TabIndex = 0;
            this.lblOrgUnit.Text = "Organizational unit:";
            this.lblOrgUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPageOrgUnits
            // 
            this.tabPageOrgUnits.Controls.Add(this.lvEmployee);
            this.tabPageOrgUnits.Controls.Add(this.lblEmployeeForOU);
            this.tabPageOrgUnits.Controls.Add(this.tbDesc);
            this.tabPageOrgUnits.Controls.Add(this.lblDesc);
            this.tabPageOrgUnits.Controls.Add(this.cbOUName);
            this.tabPageOrgUnits.Controls.Add(this.lblOUName);
            this.tabPageOrgUnits.Location = new System.Drawing.Point(4, 22);
            this.tabPageOrgUnits.Name = "tabPageOrgUnits";
            this.tabPageOrgUnits.Size = new System.Drawing.Size(608, 382);
            this.tabPageOrgUnits.TabIndex = 1;
            this.tabPageOrgUnits.Text = "Organizational units";
            // 
            // lvEmployee
            // 
            this.lvEmployee.FullRowSelect = true;
            this.lvEmployee.GridLines = true;
            this.lvEmployee.HideSelection = false;
            this.lvEmployee.Location = new System.Drawing.Point(16, 152);
            this.lvEmployee.Name = "lvEmployee";
            this.lvEmployee.Size = new System.Drawing.Size(563, 200);
            this.lvEmployee.TabIndex = 5;
            this.lvEmployee.UseCompatibleStateImageBehavior = false;
            this.lvEmployee.View = System.Windows.Forms.View.Details;
            this.lvEmployee.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployee_ColumnClick);
            // 
            // lblEmployeeForOU
            // 
            this.lblEmployeeForOU.Location = new System.Drawing.Point(16, 96);
            this.lblEmployeeForOU.Name = "lblEmployeeForOU";
            this.lblEmployeeForOU.Size = new System.Drawing.Size(344, 48);
            this.lblEmployeeForOU.TabIndex = 4;
            this.lblEmployeeForOU.Text = "List of all employees who belong to selected organizational unit:";
            this.lblEmployeeForOU.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbDesc
            // 
            this.tbDesc.Enabled = false;
            this.tbDesc.Location = new System.Drawing.Point(112, 56);
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(224, 20);
            this.tbDesc.TabIndex = 3;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(34, 53);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(72, 23);
            this.lblDesc.TabIndex = 2;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbOUName
            // 
            this.cbOUName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOUName.Location = new System.Drawing.Point(112, 24);
            this.cbOUName.Name = "cbOUName";
            this.cbOUName.Size = new System.Drawing.Size(224, 21);
            this.cbOUName.TabIndex = 1;
            this.cbOUName.SelectedIndexChanged += new System.EventHandler(this.cbOUName_SelectedIndexChanged);
            // 
            // lblOUName
            // 
            this.lblOUName.Location = new System.Drawing.Point(34, 22);
            this.lblOUName.Name = "lblOUName";
            this.lblOUName.Size = new System.Drawing.Size(72, 23);
            this.lblOUName.TabIndex = 0;
            this.lblOUName.Text = "Name:";
            this.lblOUName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPageEmployeesXOrgUnits
            // 
            this.tabPageEmployeesXOrgUnits.Controls.Add(this.btnOUTree1);
            this.tabPageEmployeesXOrgUnits.Controls.Add(this.btnRemove);
            this.tabPageEmployeesXOrgUnits.Controls.Add(this.btnRemoveAll);
            this.tabPageEmployeesXOrgUnits.Controls.Add(this.btnAddAll);
            this.tabPageEmployeesXOrgUnits.Controls.Add(this.btnAdd);
            this.tabPageEmployeesXOrgUnits.Controls.Add(this.btnCancel);
            this.tabPageEmployeesXOrgUnits.Controls.Add(this.btnSave);
            this.tabPageEmployeesXOrgUnits.Controls.Add(this.lvSelectedEmployees);
            this.tabPageEmployeesXOrgUnits.Controls.Add(this.lblSelEmployees);
            this.tabPageEmployeesXOrgUnits.Controls.Add(this.lvEmployees);
            this.tabPageEmployeesXOrgUnits.Controls.Add(this.lblEmployeesForOU);
            this.tabPageEmployeesXOrgUnits.Controls.Add(this.tbOUDesc);
            this.tabPageEmployeesXOrgUnits.Controls.Add(this.lblOUDesc);
            this.tabPageEmployeesXOrgUnits.Controls.Add(this.cbOU);
            this.tabPageEmployeesXOrgUnits.Controls.Add(this.lblOU);
            this.tabPageEmployeesXOrgUnits.Location = new System.Drawing.Point(4, 22);
            this.tabPageEmployeesXOrgUnits.Name = "tabPageEmployeesXOrgUnits";
            this.tabPageEmployeesXOrgUnits.Size = new System.Drawing.Size(608, 382);
            this.tabPageEmployeesXOrgUnits.TabIndex = 2;
            this.tabPageEmployeesXOrgUnits.Text = "Employee <-> Organizational units";
            // 
            // btnOUTree1
            // 
            this.btnOUTree1.Image = ((System.Drawing.Image)(resources.GetObject("btnOUTree1.Image")));
            this.btnOUTree1.Location = new System.Drawing.Point(430, 14);
            this.btnOUTree1.Name = "btnOUTree1";
            this.btnOUTree1.Size = new System.Drawing.Size(25, 23);
            this.btnOUTree1.TabIndex = 2;
            this.btnOUTree1.Click += new System.EventHandler(this.btnOUTree1_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Location = new System.Drawing.Point(280, 240);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(48, 23);
            this.btnRemoveAll.TabIndex = 9;
            this.btnRemoveAll.Text = "<<";
            this.btnRemoveAll.Visible = false;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // btnAddAll
            // 
            this.btnAddAll.Location = new System.Drawing.Point(280, 208);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(48, 23);
            this.btnAddAll.TabIndex = 8;
            this.btnAddAll.Text = ">>";
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(280, 176);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(48, 23);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = ">";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(520, 344);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 344);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 13;
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
            this.lvSelectedEmployees.TabIndex = 12;
            this.lvSelectedEmployees.UseCompatibleStateImageBehavior = false;
            this.lvSelectedEmployees.View = System.Windows.Forms.View.Details;
            this.lvSelectedEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvSelectedEmployees_ColumnClick);
            // 
            // lblSelEmployees
            // 
            this.lblSelEmployees.Location = new System.Drawing.Point(344, 80);
            this.lblSelEmployees.Name = "lblSelEmployees";
            this.lblSelEmployees.Size = new System.Drawing.Size(248, 48);
            this.lblSelEmployees.TabIndex = 11;
            this.lblSelEmployees.Text = "List of all employees who belong to selected organizational unit:";
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
            this.lvEmployees.TabIndex = 6;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployees_ColumnClick);
            // 
            // lblEmployeesForOU
            // 
            this.lblEmployeesForOU.Location = new System.Drawing.Point(16, 80);
            this.lblEmployeesForOU.Name = "lblEmployeesForOU";
            this.lblEmployeesForOU.Size = new System.Drawing.Size(248, 48);
            this.lblEmployeesForOU.TabIndex = 5;
            this.lblEmployeesForOU.Text = "List of all employees who do not belong to selected organizational unit:";
            this.lblEmployeesForOU.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbOUDesc
            // 
            this.tbOUDesc.Enabled = false;
            this.tbOUDesc.Location = new System.Drawing.Point(184, 48);
            this.tbOUDesc.Name = "tbOUDesc";
            this.tbOUDesc.Size = new System.Drawing.Size(240, 20);
            this.tbOUDesc.TabIndex = 4;
            // 
            // lblOUDesc
            // 
            this.lblOUDesc.Location = new System.Drawing.Point(58, 45);
            this.lblOUDesc.Name = "lblOUDesc";
            this.lblOUDesc.Size = new System.Drawing.Size(120, 23);
            this.lblOUDesc.TabIndex = 3;
            this.lblOUDesc.Text = "Description:";
            this.lblOUDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbOU
            // 
            this.cbOU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOU.Location = new System.Drawing.Point(184, 16);
            this.cbOU.Name = "cbOU";
            this.cbOU.Size = new System.Drawing.Size(240, 21);
            this.cbOU.TabIndex = 1;
            this.cbOU.SelectedIndexChanged += new System.EventHandler(this.cbOU_SelectedIndexChanged);
            // 
            // lblOU
            // 
            this.lblOU.Location = new System.Drawing.Point(19, 14);
            this.lblOU.Name = "lblOU";
            this.lblOU.Size = new System.Drawing.Size(159, 23);
            this.lblOU.TabIndex = 0;
            this.lblOU.Text = "Organizational unit:";
            this.lblOU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(556, 428);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // EmployeesXOrganizationalUnits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 462);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "EmployeesXOrganizationalUnits";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Employees X Organizational Units";
            this.Load += new System.EventHandler(this.EmployeesXOrganizationalUnits_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageEmployees.ResumeLayout(false);
            this.tabPageEmployees.PerformLayout();
            this.gbOU.ResumeLayout(false);
            this.gbOU.PerformLayout();
            this.tabPageOrgUnits.ResumeLayout(false);
            this.tabPageOrgUnits.PerformLayout();
            this.tabPageEmployeesXOrgUnits.ResumeLayout(false);
            this.tabPageEmployeesXOrgUnits.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageEmployees;
        private System.Windows.Forms.Button btnOUTree;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.GroupBox gbOU;
        private System.Windows.Forms.TextBox tbSelOUDesc;
        private System.Windows.Forms.Label lblSelOUDesc;
        private System.Windows.Forms.Label lblSelOUName;
        private System.Windows.Forms.TextBox tbSelOUName;
        private System.Windows.Forms.TextBox tbOrgUnitDesc;
        private System.Windows.Forms.Label lblOrgUnitDesc;
        private System.Windows.Forms.ComboBox cbOrgUnit;
        private System.Windows.Forms.Label lblOrgUnit;
        private System.Windows.Forms.TabPage tabPageOrgUnits;
        private System.Windows.Forms.ListView lvEmployee;
        private System.Windows.Forms.Label lblEmployeeForOU;
        private System.Windows.Forms.TextBox tbDesc;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.ComboBox cbOUName;
        private System.Windows.Forms.Label lblOUName;
        private System.Windows.Forms.TabPage tabPageEmployeesXOrgUnits;
        private System.Windows.Forms.Button btnOUTree1;
        private System.Windows.Forms.Button btnRemoveAll;
        private System.Windows.Forms.Button btnAddAll;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ListView lvSelectedEmployees;
        private System.Windows.Forms.Label lblSelEmployees;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.Label lblEmployeesForOU;
        private System.Windows.Forms.TextBox tbOUDesc;
        private System.Windows.Forms.Label lblOUDesc;
        private System.Windows.Forms.ComboBox cbOU;
        private System.Windows.Forms.Label lblOU;
        private System.Windows.Forms.Button btnClose;
    }
}