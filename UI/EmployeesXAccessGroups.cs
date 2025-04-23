using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Resources;
using System.Globalization;

using Common;
using TransferObjects;
using Util;

namespace UI
{
	/// <summary>
	/// Summary description for EmployeesXAccessGroups.
	/// </summary>
	public class EmployeesXAccessGroups : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPageEmployees;
		private System.Windows.Forms.TabPage tabPageAccessGroups;
		private System.Windows.Forms.TabPage tabPageEmployeesXAccessGroups;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label lblAccessGroup;
		private System.Windows.Forms.ComboBox cbAccessGroup;
		private System.Windows.Forms.Label lblAccessGroupDesc;
		private System.Windows.Forms.TextBox tbAccessGroupDesc;
		private System.Windows.Forms.Label lblEmployeesForAccessGroup;
		private System.Windows.Forms.ListView lvEmployees;
		private System.Windows.Forms.Label lblSelEmployees;
		private System.Windows.Forms.ListView lvSelectedEmployees;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnRemoveAll;
		private System.Windows.Forms.Button btnAddAll;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Label lblAccessGroupName;
		private System.Windows.Forms.ComboBox cbAccessGroupName;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.Label lblEmployeeForAccessGroup;
		private System.Windows.Forms.ListView lvEmployee;
		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.Label lblSelAccessGroupDesc;
		private System.Windows.Forms.TextBox tbSelAccessGroupDesc;
		private System.Windows.Forms.TextBox tbSelAccessGroupName;
		private System.Windows.Forms.Label lblSelAccessGroupName;
		private System.Windows.Forms.Label lblWorkingUnit;
		private System.Windows.Forms.ComboBox cbWorkingUnit;
		private System.Windows.Forms.Label lblWorkingUnitDesc;
		private System.Windows.Forms.TextBox tbWorkingUnitDesc;
		private System.Windows.Forms.Label lblEmployee;
		private System.Windows.Forms.ComboBox cbEmployee;
		private System.Windows.Forms.Button btnAccessGroupMaintenance;	
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		ApplUserTO logInUser;
		ResourceManager rm;				
		private CultureInfo culture;
		DebugLog log;		

		private ListViewItemComparer _comp1;
		private ListViewItemComparer _comp2;
		private ListViewItemComparer _comp3;		

		// List View indexes
		const int EmployeeIDIndex = 0;
		const int FirstNameIndex = 1;
		const int LastNameIndex = 2;
		const int WorkingUnitNameIndex = 3;

		// NOTE: removedEmployees is not in use. If it is needed in the future,
		//relations between addedEmployees and removedEmployees need to be added. 
		//See ApplUsersXRoles.cs for example
		private ArrayList removedEmployees;
		private ArrayList addedEmployees;

		private string prevSelValuecbAccessGroup = "";
        private Button btnWUTree;
			
		private bool saveForPrevIndex = false;
        private CheckBox chbHierarhicly;

        List<WorkingUnitTO> workingUnitArray;

		public EmployeesXAccessGroups()
		{
			InitializeComponent();
			setProperties();
		}

		public EmployeesXAccessGroups(bool hideButton)
		{
			InitializeComponent();
			setProperties();
			if (hideButton)
				btnAccessGroupMaintenance.Visible = false;
		}

		public void setProperties()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			removedEmployees = new ArrayList();
			addedEmployees   = new ArrayList();

			rm = new ResourceManager("UI.Resource",typeof(EmployeeAccessGroups).Assembly);
			setLanguage();

			/* Maybe this populate functions should be in Load */
			populateWorkingUnitCombo();
			populateEmployeeCombo();			
			populateAccessGroupCombo(cbAccessGroup);
			populateAccessGroupCombo(cbAccessGroupName);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeesXAccessGroups));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageEmployees = new System.Windows.Forms.TabPage();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.tbSelAccessGroupDesc = new System.Windows.Forms.TextBox();
            this.lblSelAccessGroupDesc = new System.Windows.Forms.Label();
            this.lblSelAccessGroupName = new System.Windows.Forms.Label();
            this.tbSelAccessGroupName = new System.Windows.Forms.TextBox();
            this.tbWorkingUnitDesc = new System.Windows.Forms.TextBox();
            this.lblWorkingUnitDesc = new System.Windows.Forms.Label();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.tabPageAccessGroups = new System.Windows.Forms.TabPage();
            this.btnAccessGroupMaintenance = new System.Windows.Forms.Button();
            this.lvEmployee = new System.Windows.Forms.ListView();
            this.lblEmployeeForAccessGroup = new System.Windows.Forms.Label();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.cbAccessGroupName = new System.Windows.Forms.ComboBox();
            this.lblAccessGroupName = new System.Windows.Forms.Label();
            this.tabPageEmployeesXAccessGroups = new System.Windows.Forms.TabPage();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.btnAddAll = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lvSelectedEmployees = new System.Windows.Forms.ListView();
            this.lblSelEmployees = new System.Windows.Forms.Label();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.lblEmployeesForAccessGroup = new System.Windows.Forms.Label();
            this.tbAccessGroupDesc = new System.Windows.Forms.TextBox();
            this.lblAccessGroupDesc = new System.Windows.Forms.Label();
            this.cbAccessGroup = new System.Windows.Forms.ComboBox();
            this.lblAccessGroup = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPageEmployees.SuspendLayout();
            this.groupBox.SuspendLayout();
            this.tabPageAccessGroups.SuspendLayout();
            this.tabPageEmployeesXAccessGroups.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageEmployees);
            this.tabControl1.Controls.Add(this.tabPageAccessGroups);
            this.tabControl1.Controls.Add(this.tabPageEmployeesXAccessGroups);
            this.tabControl1.Location = new System.Drawing.Point(8, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(616, 408);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageEmployees
            // 
            this.tabPageEmployees.Controls.Add(this.chbHierarhicly);
            this.tabPageEmployees.Controls.Add(this.btnWUTree);
            this.tabPageEmployees.Controls.Add(this.cbEmployee);
            this.tabPageEmployees.Controls.Add(this.lblEmployee);
            this.tabPageEmployees.Controls.Add(this.groupBox);
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
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(358, 24);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 41;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
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
            this.lblEmployee.Location = new System.Drawing.Point(16, 88);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(104, 23);
            this.lblEmployee.TabIndex = 4;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.tbSelAccessGroupDesc);
            this.groupBox.Controls.Add(this.lblSelAccessGroupDesc);
            this.groupBox.Controls.Add(this.lblSelAccessGroupName);
            this.groupBox.Controls.Add(this.tbSelAccessGroupName);
            this.groupBox.Location = new System.Drawing.Point(16, 160);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(576, 120);
            this.groupBox.TabIndex = 6;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Access group that selected employee belongs to";
            // 
            // tbSelAccessGroupDesc
            // 
            this.tbSelAccessGroupDesc.Enabled = false;
            this.tbSelAccessGroupDesc.Location = new System.Drawing.Point(112, 72);
            this.tbSelAccessGroupDesc.Name = "tbSelAccessGroupDesc";
            this.tbSelAccessGroupDesc.Size = new System.Drawing.Size(432, 20);
            this.tbSelAccessGroupDesc.TabIndex = 10;
            // 
            // lblSelAccessGroupDesc
            // 
            this.lblSelAccessGroupDesc.Location = new System.Drawing.Point(16, 72);
            this.lblSelAccessGroupDesc.Name = "lblSelAccessGroupDesc";
            this.lblSelAccessGroupDesc.Size = new System.Drawing.Size(80, 23);
            this.lblSelAccessGroupDesc.TabIndex = 9;
            this.lblSelAccessGroupDesc.Text = "Description:";
            this.lblSelAccessGroupDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSelAccessGroupName
            // 
            this.lblSelAccessGroupName.Location = new System.Drawing.Point(16, 40);
            this.lblSelAccessGroupName.Name = "lblSelAccessGroupName";
            this.lblSelAccessGroupName.Size = new System.Drawing.Size(80, 23);
            this.lblSelAccessGroupName.TabIndex = 7;
            this.lblSelAccessGroupName.Text = "Name:";
            this.lblSelAccessGroupName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbSelAccessGroupName
            // 
            this.tbSelAccessGroupName.Enabled = false;
            this.tbSelAccessGroupName.Location = new System.Drawing.Point(112, 40);
            this.tbSelAccessGroupName.Name = "tbSelAccessGroupName";
            this.tbSelAccessGroupName.Size = new System.Drawing.Size(432, 20);
            this.tbSelAccessGroupName.TabIndex = 8;
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
            this.lblWorkingUnitDesc.Location = new System.Drawing.Point(16, 56);
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
            this.lblWorkingUnit.Location = new System.Drawing.Point(16, 24);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(104, 23);
            this.lblWorkingUnit.TabIndex = 0;
            this.lblWorkingUnit.Text = "Working unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPageAccessGroups
            // 
            this.tabPageAccessGroups.Controls.Add(this.btnAccessGroupMaintenance);
            this.tabPageAccessGroups.Controls.Add(this.lvEmployee);
            this.tabPageAccessGroups.Controls.Add(this.lblEmployeeForAccessGroup);
            this.tabPageAccessGroups.Controls.Add(this.tbDesc);
            this.tabPageAccessGroups.Controls.Add(this.lblDesc);
            this.tabPageAccessGroups.Controls.Add(this.cbAccessGroupName);
            this.tabPageAccessGroups.Controls.Add(this.lblAccessGroupName);
            this.tabPageAccessGroups.Location = new System.Drawing.Point(4, 22);
            this.tabPageAccessGroups.Name = "tabPageAccessGroups";
            this.tabPageAccessGroups.Size = new System.Drawing.Size(608, 382);
            this.tabPageAccessGroups.TabIndex = 1;
            this.tabPageAccessGroups.Text = "Access groups";
            // 
            // btnAccessGroupMaintenance
            // 
            this.btnAccessGroupMaintenance.Location = new System.Drawing.Point(400, 328);
            this.btnAccessGroupMaintenance.Name = "btnAccessGroupMaintenance";
            this.btnAccessGroupMaintenance.Size = new System.Drawing.Size(168, 23);
            this.btnAccessGroupMaintenance.TabIndex = 17;
            this.btnAccessGroupMaintenance.Text = "Access group maintenance";
            this.btnAccessGroupMaintenance.Click += new System.EventHandler(this.btnAccessGroupMaintenance_Click);
            // 
            // lvEmployee
            // 
            this.lvEmployee.FullRowSelect = true;
            this.lvEmployee.GridLines = true;
            this.lvEmployee.HideSelection = false;
            this.lvEmployee.Location = new System.Drawing.Point(16, 152);
            this.lvEmployee.Name = "lvEmployee";
            this.lvEmployee.Size = new System.Drawing.Size(344, 200);
            this.lvEmployee.TabIndex = 16;
            this.lvEmployee.UseCompatibleStateImageBehavior = false;
            this.lvEmployee.View = System.Windows.Forms.View.Details;
            this.lvEmployee.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployee_ColumnClick);
            // 
            // lblEmployeeForAccessGroup
            // 
            this.lblEmployeeForAccessGroup.Location = new System.Drawing.Point(16, 96);
            this.lblEmployeeForAccessGroup.Name = "lblEmployeeForAccessGroup";
            this.lblEmployeeForAccessGroup.Size = new System.Drawing.Size(344, 48);
            this.lblEmployeeForAccessGroup.TabIndex = 15;
            this.lblEmployeeForAccessGroup.Text = "List of all employees who belong to selected access group:";
            this.lblEmployeeForAccessGroup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.lblDesc.Location = new System.Drawing.Point(16, 56);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(72, 23);
            this.lblDesc.TabIndex = 13;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbAccessGroupName
            // 
            this.cbAccessGroupName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAccessGroupName.Location = new System.Drawing.Point(112, 24);
            this.cbAccessGroupName.Name = "cbAccessGroupName";
            this.cbAccessGroupName.Size = new System.Drawing.Size(224, 21);
            this.cbAccessGroupName.TabIndex = 12;
            this.cbAccessGroupName.SelectedIndexChanged += new System.EventHandler(this.cbAccessGroupName_SelectedIndexChanged);
            // 
            // lblAccessGroupName
            // 
            this.lblAccessGroupName.Location = new System.Drawing.Point(16, 24);
            this.lblAccessGroupName.Name = "lblAccessGroupName";
            this.lblAccessGroupName.Size = new System.Drawing.Size(72, 23);
            this.lblAccessGroupName.TabIndex = 11;
            this.lblAccessGroupName.Text = "Name:";
            this.lblAccessGroupName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPageEmployeesXAccessGroups
            // 
            this.tabPageEmployeesXAccessGroups.Controls.Add(this.btnRemove);
            this.tabPageEmployeesXAccessGroups.Controls.Add(this.btnRemoveAll);
            this.tabPageEmployeesXAccessGroups.Controls.Add(this.btnAddAll);
            this.tabPageEmployeesXAccessGroups.Controls.Add(this.btnAdd);
            this.tabPageEmployeesXAccessGroups.Controls.Add(this.btnCancel);
            this.tabPageEmployeesXAccessGroups.Controls.Add(this.btnSave);
            this.tabPageEmployeesXAccessGroups.Controls.Add(this.lvSelectedEmployees);
            this.tabPageEmployeesXAccessGroups.Controls.Add(this.lblSelEmployees);
            this.tabPageEmployeesXAccessGroups.Controls.Add(this.lvEmployees);
            this.tabPageEmployeesXAccessGroups.Controls.Add(this.lblEmployeesForAccessGroup);
            this.tabPageEmployeesXAccessGroups.Controls.Add(this.tbAccessGroupDesc);
            this.tabPageEmployeesXAccessGroups.Controls.Add(this.lblAccessGroupDesc);
            this.tabPageEmployeesXAccessGroups.Controls.Add(this.cbAccessGroup);
            this.tabPageEmployeesXAccessGroups.Controls.Add(this.lblAccessGroup);
            this.tabPageEmployeesXAccessGroups.Location = new System.Drawing.Point(4, 22);
            this.tabPageEmployeesXAccessGroups.Name = "tabPageEmployeesXAccessGroups";
            this.tabPageEmployeesXAccessGroups.Size = new System.Drawing.Size(608, 382);
            this.tabPageEmployeesXAccessGroups.TabIndex = 2;
            this.tabPageEmployeesXAccessGroups.Text = "Employee <-> Access groups";
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
            this.lblSelEmployees.Text = "List of all employees who belong to selected access group:";
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
            // lblEmployeesForAccessGroup
            // 
            this.lblEmployeesForAccessGroup.Location = new System.Drawing.Point(16, 80);
            this.lblEmployeesForAccessGroup.Name = "lblEmployeesForAccessGroup";
            this.lblEmployeesForAccessGroup.Size = new System.Drawing.Size(248, 48);
            this.lblEmployeesForAccessGroup.TabIndex = 22;
            this.lblEmployeesForAccessGroup.Text = "List of all employees who do not belong to selected access group:";
            this.lblEmployeesForAccessGroup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbAccessGroupDesc
            // 
            this.tbAccessGroupDesc.Enabled = false;
            this.tbAccessGroupDesc.Location = new System.Drawing.Point(184, 48);
            this.tbAccessGroupDesc.Name = "tbAccessGroupDesc";
            this.tbAccessGroupDesc.Size = new System.Drawing.Size(240, 20);
            this.tbAccessGroupDesc.TabIndex = 21;
            // 
            // lblAccessGroupDesc
            // 
            this.lblAccessGroupDesc.Location = new System.Drawing.Point(48, 51);
            this.lblAccessGroupDesc.Name = "lblAccessGroupDesc";
            this.lblAccessGroupDesc.Size = new System.Drawing.Size(120, 23);
            this.lblAccessGroupDesc.TabIndex = 20;
            this.lblAccessGroupDesc.Text = "Description:";
            this.lblAccessGroupDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbAccessGroup
            // 
            this.cbAccessGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAccessGroup.Location = new System.Drawing.Point(184, 16);
            this.cbAccessGroup.Name = "cbAccessGroup";
            this.cbAccessGroup.Size = new System.Drawing.Size(240, 21);
            this.cbAccessGroup.TabIndex = 19;
            this.cbAccessGroup.SelectedIndexChanged += new System.EventHandler(this.cbAccessGroup_SelectedIndexChanged);
            // 
            // lblAccessGroup
            // 
            this.lblAccessGroup.Location = new System.Drawing.Point(48, 19);
            this.lblAccessGroup.Name = "lblAccessGroup";
            this.lblAccessGroup.Size = new System.Drawing.Size(120, 23);
            this.lblAccessGroup.TabIndex = 18;
            this.lblAccessGroup.Text = "Access group:";
            this.lblAccessGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(389, 24);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 42;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // EmployeesXAccessGroups
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(632, 454);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 488);
            this.MinimumSize = new System.Drawing.Size(640, 488);
            this.Name = "EmployeesXAccessGroups";
            this.ShowInTaskbar = false;
            this.Text = "Employees <-> Access groups";
            this.Load += new System.EventHandler(this.EmployeesXAccessGroups_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmployeesXAccessGroups_KeyUp);
            this.tabControl1.ResumeLayout(false);
            this.tabPageEmployees.ResumeLayout(false);
            this.tabPageEmployees.PerformLayout();
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.tabPageAccessGroups.ResumeLayout(false);
            this.tabPageAccessGroups.PerformLayout();
            this.tabPageEmployeesXAccessGroups.ResumeLayout(false);
            this.tabPageEmployeesXAccessGroups.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region Inner Class for sorting items in View List

		/*
		 *  Class used for sorting items in the List View 
		 */
		private class ListViewItemComparer : IComparer
		{
			private ListView _listView;

			public ListViewItemComparer(ListView lv)
			{
				_listView = lv;
			}
			public ListView ListView
			{
				get{return _listView;}
			}

			private int _sortColumn = 0;

			public int SortColumn
			{
				get { return _sortColumn; }
				set { _sortColumn = value; }
			}

			public int Compare(object a, object b)
			{
				ListViewItem item1 = (ListViewItem) a;
				ListViewItem item2 = (ListViewItem) b;

				if (ListView.Sorting == SortOrder.Descending)
				{
					ListViewItem temp = item1;
					item1 = item2;
					item2 = temp;
				}
				// Handle non Detail Cases
				return CompareItems(item1, item2);
			}

			public int CompareItems(ListViewItem item1, ListViewItem item2)
			{
				// Subitem instances
				ListViewItem.ListViewSubItem sub1 = item1.SubItems[SortColumn];
				ListViewItem.ListViewSubItem sub2 = item2.SubItems[SortColumn];

				// Return value based on sort column	
				switch (SortColumn)
				{
					case EmployeesXAccessGroups.EmployeeIDIndex:
						int firstID = -1;
						int secondID = -1;

						if (!sub1.Text.Trim().Equals("")) 
						{
							firstID = Int32.Parse(sub1.Text.Trim());
						}

						if (!sub2.Text.Trim().Equals(""))
						{
							secondID = Int32.Parse(sub2.Text.Trim());
						}
						
						return CaseInsensitiveComparer.Default.Compare(firstID, secondID);
					case EmployeesXAccessGroups.FirstNameIndex:
					case EmployeesXAccessGroups.LastNameIndex:
					case EmployeesXAccessGroups.WorkingUnitNameIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
					}
					default:
						throw new IndexOutOfRangeException("Unrecognized column name extension");
				}
			}
		}

		#endregion

		/// <summary>
		/// Set proper language and initialize List View
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// Form name
				this.Text = rm.GetString("employeesXAccessGroupsForm", culture);

				// TabPage text
				tabPageEmployees.Text              = rm.GetString("tabPageEmployees", culture);
				tabPageAccessGroups.Text           = rm.GetString("tabPageAccessGroups", culture);
				tabPageEmployeesXAccessGroups.Text = rm.GetString("tabPageEmployeesXAccessGroups", culture);

				// button's text
				btnClose.Text                  = rm.GetString("btnClose", culture);
				btnAccessGroupMaintenance.Text = rm.GetString("btnAccessGroupMaintenance", culture);
				btnCancel.Text                 = rm.GetString("btnCancel", culture);
				btnSave.Text                   = rm.GetString("btnSave", culture);

				// group box text
				groupBox.Text = rm.GetString("gbEmployeeAccessGroup", culture);

                //check box text
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);

				// label's text
				lblWorkingUnit.Text             = rm.GetString("lblWorkingUnit", culture);
				lblWorkingUnitDesc.Text         = rm.GetString("lblDescription", culture);
				lblEmployee.Text                = rm.GetString("lblEmployee", culture);
				lblSelAccessGroupName.Text      = rm.GetString("lblName", culture);
				lblSelAccessGroupDesc.Text      = rm.GetString("lblDescription", culture);
				lblAccessGroupName.Text         = rm.GetString("lblName", culture);
				lblDesc.Text                    = rm.GetString("lblDescription", culture);
				lblEmployeeForAccessGroup.Text  = rm.GetString("lblEmployeeForAccessGroup", culture);
				lblAccessGroup.Text             = rm.GetString("lblAccessGroup", culture);
				lblAccessGroupDesc.Text         = rm.GetString("lblDescription", culture);
				lblEmployeesForAccessGroup.Text = rm.GetString("lblEmployeesForAccessGroup", culture);
				lblSelEmployees.Text            = rm.GetString("lblEmployeeForAccessGroup", culture);				

				// list view				
				lvEmployee.BeginUpdate();
				lvEmployee.Columns.Add(rm.GetString("hdrEmployeeID", culture), (lvEmployee.Width - 4) / 4, HorizontalAlignment.Left);
				lvEmployee.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployee.Width - 4) / 4, HorizontalAlignment.Left);
				lvEmployee.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployee.Width - 4) / 4, HorizontalAlignment.Left);
				lvEmployee.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvEmployee.Width - 4) / 4, HorizontalAlignment.Left);
				lvEmployee.EndUpdate();

				lvEmployees.BeginUpdate();
				lvEmployees.Columns.Add(rm.GetString("hdrEmployeeID", culture), (lvEmployees.Width - 4) / 4, HorizontalAlignment.Left);
				lvEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployees.Width - 4) / 4, HorizontalAlignment.Left);
				lvEmployees.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployees.Width - 4) / 4, HorizontalAlignment.Left);
				lvEmployees.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvEmployees.Width - 4) / 4, HorizontalAlignment.Left);
				lvEmployees.EndUpdate();

				lvSelectedEmployees.BeginUpdate();
				lvSelectedEmployees.Columns.Add(rm.GetString("hdrEmployeeID", culture), (lvSelectedEmployees.Width - 4) / 4, HorizontalAlignment.Left);
				lvSelectedEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), (lvSelectedEmployees.Width - 4) / 4, HorizontalAlignment.Left);
				lvSelectedEmployees.Columns.Add(rm.GetString("hdrLastName", culture), (lvSelectedEmployees.Width - 4) / 4, HorizontalAlignment.Left);
				lvSelectedEmployees.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvSelectedEmployees.Width - 4) / 4, HorizontalAlignment.Left);
				lvSelectedEmployees.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateEmployeeCombo()
		{
			try
			{
				string workingUnitID = "";

				if (cbWorkingUnit.SelectedIndex > 0)
				{
					workingUnitID = cbWorkingUnit.SelectedValue.ToString().Trim();
                    if (chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in workingUnitArray)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnit.SelectedValue)
                            {
                                wuList.Add(workingUnit);
                                workUnit.WUTO = workingUnit;
                            }
                        }
                        if (workUnit.WUTO.ChildWUNumber > 0)
                            wuList = workUnit.FindAllChildren(wuList);
                        workingUnitID = "";
                        foreach (WorkingUnitTO wunit in wuList)
                        {
                            workingUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (workingUnitID.Length > 0)
                        {
                            workingUnitID = workingUnitID.Substring(0, workingUnitID.Length - 1);
                        }
                    }
				}

				List<EmployeeTO> employeeArray = new Employee().SearchByWUGetAccessGroup(workingUnitID);

				foreach(EmployeeTO employee in employeeArray)
				{
					employee.LastName += " " + employee.FirstName;
				}

				employeeArray.Insert(0, new EmployeeTO(-1, "", rm.GetString("all", culture), -1, "", "", -1, "", -1, ""));

				cbEmployee.DataSource    = employeeArray;
				cbEmployee.DisplayMember = "LastName";
				cbEmployee.ValueMember   = "AccessGroupID";

				//cbEmployee.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.populateEmployeeCombo(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void populateWorkingUnitCombo()
		{
			try
			{
				WorkingUnit workingUnit = new WorkingUnit();
                workingUnit.WUTO.Status = Constants.DefaultStateActive;
                workingUnitArray = workingUnit.Search();
				workingUnitArray.Insert(0, new WorkingUnitTO(-1, -1, "", rm.GetString("all", culture), "", -1));
				
				cbWorkingUnit.DataSource    = workingUnitArray;
				cbWorkingUnit.DisplayMember = "Name";
				cbWorkingUnit.ValueMember   = "WorkingUnitID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.populateWorkingUnitCombo(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void populateAccessGroupCombo(ComboBox cbName)
		{
			try
			{
				EmployeeGroupAccessControl accessGroup = new EmployeeGroupAccessControl();
				ArrayList accessGroupArray = accessGroup.Search("");
				accessGroupArray.Insert(0, new EmployeeGroupAccessControl(-1, rm.GetString("all", culture), rm.GetString("all", culture)));

				cbName.DataSource    = accessGroupArray;
				cbName.DisplayMember = "Name";
				cbName.ValueMember   = "AccessGroupId";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.populateAccessGroupCombo(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void clearListView(ListView lvName)
		{
			lvName.BeginUpdate();
			lvName.Items.Clear();
			lvName.EndUpdate();
			lvName.Invalidate();
		}

		private void populateEmployeeListView(List<EmployeeTO> employeeList, ListView lvName)
		{
			try
			{
				lvName.BeginUpdate();
				lvName.Items.Clear();
				
				if (employeeList.Count > 0)
				{
					foreach(EmployeeTO employee in employeeList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = employee.EmployeeID.ToString().Trim();
						item.SubItems.Add(employee.FirstName.Trim());
						item.SubItems.Add(employee.LastName.Trim());
						item.SubItems.Add(employee.WorkingUnitName.Trim());
											
						lvName.Items.Add(item);									
					}
				}

				lvName.EndUpdate();
				lvName.Invalidate();
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.populateEmployeeListView(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void EmployeesXAccessGroups_Load(object sender, System.EventArgs e)
		{			
			// Initialize comparer objects
			_comp1 = new ListViewItemComparer(lvEmployee);
			lvEmployee.ListViewItemSorter = _comp1;
			lvEmployee.Sorting = SortOrder.Ascending;

			_comp2 = new ListViewItemComparer(lvEmployees);
			lvEmployees.ListViewItemSorter = _comp2;
			lvEmployees.Sorting = SortOrder.Ascending;

			_comp3 = new ListViewItemComparer(lvSelectedEmployees);
			lvSelectedEmployees.ListViewItemSorter = _comp3;
			lvSelectedEmployees.Sorting = SortOrder.Ascending;

			/* Maybe populate functions should be here instead in Constructor */
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXAccessGroups.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				if (cbAccessGroup.SelectedIndex > 0)
				{
					removedEmployees.Clear();
					addedEmployees.Clear();
					cbAccessGroup.SelectedIndex = 0;
			
					MessageBox.Show(rm.GetString("EmployeeXAccessGroupCancelChanges", culture));
				}
				else
				{
					removedEmployees.Clear();
					addedEmployees.Clear();

					clearListView(lvSelectedEmployees); 
					clearListView(lvEmployees);
				}
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbWorkingUnit_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
                bool check = false;
                if (!chbHierarhicly.Checked)
                {
                    foreach (WorkingUnitTO wu in workingUnitArray)
                    {
                        if (cbWorkingUnit.SelectedIndex != 0)
                        {
                            if (wu.WorkingUnitID == (int)cbWorkingUnit.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                            {
                                chbHierarhicly.Checked = true;
                                check = true;
                            }
                        }
                    }
                }
                if (!check)
                {

                    if (cbWorkingUnit.SelectedIndex > 0)
                    {
                        WorkingUnit workingUnit = new WorkingUnit();
                        workingUnit.Find((int)cbWorkingUnit.SelectedValue);

                        if (workingUnit.WUTO.WorkingUnitID != -1)
                        {
                            tbWorkingUnitDesc.Text = workingUnit.WUTO.Description.Trim();
                            populateEmployeeCombo();
                        }
                    }
                    else
                    {
                        tbWorkingUnitDesc.Text = "";
                        populateEmployeeCombo();
                    }
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbEmployee_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				if (cbEmployee.SelectedIndex > 0)
				{
					EmployeeGroupAccessControlTO employeeGroupAccessControlTO = new EmployeeGroupAccessControl().Find(cbEmployee.SelectedValue.ToString().Trim());
					if (employeeGroupAccessControlTO.AccessGroupId != -1)
					{
						tbSelAccessGroupName.Text = employeeGroupAccessControlTO.Name.Trim();
						tbSelAccessGroupDesc.Text = employeeGroupAccessControlTO.Description.Trim();
					}
				}
				else
				{
					tbSelAccessGroupName.Text = "";
					tbSelAccessGroupDesc.Text = "";
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.cbEmployee_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbAccessGroupName_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				if (cbAccessGroupName.SelectedIndex > 0)
				{
					EmployeeGroupAccessControlTO employeeGroupAccessControlTO = new EmployeeGroupAccessControl().Find(cbAccessGroupName.SelectedValue.ToString().Trim());
					if (employeeGroupAccessControlTO.AccessGroupId != -1)
					{
						tbDesc.Text = employeeGroupAccessControlTO.Description.Trim();
						populateEmployeeListView(new Employee().SearchByAccessGroup(employeeGroupAccessControlTO.AccessGroupId.ToString()), lvEmployee);
					}
				}
				else
				{
					tbDesc.Text = "";
					clearListView(lvEmployee);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.cbAccessGroupName_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbAccessGroup_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				if (removedEmployees.Count > 0 || addedEmployees.Count > 0)
				{
					DialogResult result = MessageBox.Show(rm.GetString("EmployeeXAccessGroupSaveChanges", culture), "", MessageBoxButtons.YesNo);
					if (result == DialogResult.Yes)
					{
						saveForPrevIndex = true;
						btnSave.PerformClick();
					}
				}

				if (cbAccessGroup.SelectedIndex > 0)
				{
					EmployeeGroupAccessControlTO employeeGroupAccessControlTO = new EmployeeGroupAccessControl().Find(cbAccessGroup.SelectedValue.ToString().Trim());
					if (employeeGroupAccessControlTO.AccessGroupId != -1)
					{
						tbAccessGroupDesc.Text = employeeGroupAccessControlTO.Description.Trim();
						populateEmployeeListView(new Employee().SearchByAccessGroup(employeeGroupAccessControlTO.AccessGroupId.ToString()), lvSelectedEmployees);
						populateEmployeeListView(new Employee().SearchNotInAccessGroup(employeeGroupAccessControlTO.AccessGroupId.ToString()), lvEmployees);
					}
				}
				else
				{
					tbAccessGroupDesc.Text = "";
					clearListView(lvSelectedEmployees);
					clearListView(lvEmployees);
				}

				removedEmployees.Clear();
				addedEmployees.Clear();				
				if (cbAccessGroup.SelectedIndex != 0)
					prevSelValuecbAccessGroup = cbAccessGroup.SelectedValue.ToString();
				else
					prevSelValuecbAccessGroup = "ALL";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.cbAccessGroup_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				foreach(ListViewItem item in lvEmployees.SelectedItems)
				{
					lvEmployees.Items.Remove(item);
					lvSelectedEmployees.Items.Add(item);
					addedEmployees.Add(item);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.btnAdd_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnAddAll_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				foreach(ListViewItem item in lvEmployees.Items)
				{
					lvEmployees.Items.Remove(item);
					lvSelectedEmployees.Items.Add(item);
					addedEmployees.Add(item);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.btnAddAll_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnRemoveAll_Click(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				foreach(ListViewItem item in lvSelectedEmployees.Items)
				{
					lvSelectedEmployees.Items.Remove(item);
					lvEmployees.Items.Add(item);
					removedEmployees.Add(item);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.btnRemoveAll_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				foreach(ListViewItem item in lvSelectedEmployees.SelectedItems)
				{
					lvSelectedEmployees.Items.Remove(item);
					lvEmployees.Items.Add(item);
					removedEmployees.Add(item);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.btnRemove_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvEmployee_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
                
                SortOrder prevOrder = lvEmployee.Sorting;
				//lvEmployee.Sorting = SortOrder.None;

				if (e.Column == _comp1.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvEmployee.Sorting = SortOrder.Descending;
					}
					else
					{
						lvEmployee.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp1.SortColumn = e.Column;
					lvEmployee.Sorting = SortOrder.Ascending;
				}

                lvEmployee.Sort();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.lvEmployee_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvEmployees_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;

				SortOrder prevOrder = lvEmployees.Sorting;
				//lvEmployees.Sorting = SortOrder.None;

				if (e.Column == _comp2.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvEmployees.Sorting = SortOrder.Descending;
					}
					else
					{
						lvEmployees.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp2.SortColumn = e.Column;
					lvEmployees.Sorting = SortOrder.Ascending;
				}

                lvEmployee.Sort();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.lvEmployees_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvSelectedEmployees_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				SortOrder prevOrder = lvSelectedEmployees.Sorting;
				//lvSelectedEmployees.Sorting = SortOrder.None;

				if (e.Column == _comp3.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvSelectedEmployees.Sorting = SortOrder.Descending;
					}
					else
					{
						lvSelectedEmployees.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp3.SortColumn = e.Column;
					lvSelectedEmployees.Sorting = SortOrder.Ascending;
				}

                lvSelectedEmployees.Sort();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.lvSelectedEmployees_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;				
				if ((saveForPrevIndex && !prevSelValuecbAccessGroup.ToUpper().Equals("ALL")) || (cbAccessGroup.SelectedIndex > 0))
				{					
					bool isUpdated = true;
					Employee employee = new Employee();

					isUpdated = employee.BeginTransaction();
					
					if (isUpdated)
					{
						/*foreach(ListViewItem item in removedEmployees)
						{
							//change it to 0
						}*/
						string acessGroupID;
						if (saveForPrevIndex)							
							acessGroupID = prevSelValuecbAccessGroup;
						else
							acessGroupID = cbAccessGroup.SelectedValue.ToString();

						foreach(ListViewItem item in addedEmployees)
						{														
							isUpdated = employee.UpdateAccessGroup(item.Text.Trim(), acessGroupID, false) && isUpdated;
						}
					}
					else
					{
						MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
						return;
					}

					if(isUpdated)
					{
						employee.CommitTransaction();

						//to refresh cbEmployee combo (to assign new Access group ID to each employee)
						//and to refresh tbSelAccessGroupName and tbSelAccessGroupDesc
						int employeeSelIndex = cbEmployee.SelectedIndex;
						cbWorkingUnit_SelectedIndexChanged(sender, e);
						cbEmployee.SelectedIndex = employeeSelIndex;
						//to refresh lvEmployee list
						cbAccessGroupName_SelectedIndexChanged(sender, e);

						MessageBox.Show(rm.GetString("EmployeeXAccessGroupSaved", culture));
						removedEmployees.Clear();
						addedEmployees.Clear();
						//this.Close();
					}
					else
					{
						MessageBox.Show(rm.GetString("EmployeeXAccessGroupNotSaved", culture));
						employee.RollbackTransaction();
					}
				}
				else
				{
					MessageBox.Show(rm.GetString("SelectAccessGroup", culture));
				}

				saveForPrevIndex = false;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.btnSave_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnAccessGroupMaintenance_Click(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				EmployeeAccessGroups accessGroups = new EmployeeAccessGroups(true);
				accessGroups.ShowDialog(this);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeesXAccessGroups.btnAccessGroupMaintenance_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void EmployeesXAccessGroups_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXAccessGroups.EmployeesXAccessGroups_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnWUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string wuString = "";

                foreach (WorkingUnitTO wUnit in workingUnitArray)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuString);
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    this.cbWorkingUnit.SelectedIndex = cbWorkingUnit.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXAccessGroups.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (cbWorkingUnit.SelectedValue is int)
                {
                    if ((int)cbWorkingUnit.SelectedValue >= 0)
                        populateEmployeeCombo();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXAccessGroups.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
