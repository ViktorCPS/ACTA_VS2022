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
	/// Summary description for ApplUsersXRoles.
	/// </summary>
	public class ApplUsersXRoles : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TabPage tabPageUsers;
		private System.Windows.Forms.Label lblUser;
		private System.Windows.Forms.ComboBox cbUser;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.ListView lvSelectedUsers;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnAddAll;
		private System.Windows.Forms.Button btnRemoveAll;
		private System.Windows.Forms.Button btnRemove;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblSelUsers;
		private System.Windows.Forms.ListView lvUser;
		private System.Windows.Forms.ListView lvUsers;

		private ApplUsersXRole currentApplUsersXRole = null;
		ApplUserTO logInUser;

		private CultureInfo culture;
		ResourceManager rm;
		DebugLog log;

		// List View indexes
		const int UserIDIndex = 0;
		const int NameIndex = 1;
		const int DescriptionIndex = 2;

		private ListViewItemComparer _comp1;
		private ListViewItemComparer _comp2;
		private ListViewItemComparer _comp3;
		private ListViewItemComparer _comp4;

		private System.Windows.Forms.Label lblRolesForUsers;
		private System.Windows.Forms.ListView lvRoles;
		private System.Windows.Forms.Label lblUserForRole;
		private System.Windows.Forms.Label lblRoleName;
		private System.Windows.Forms.Label lblUsersForRole;
		private System.Windows.Forms.TextBox tbRoleDesc;
		private System.Windows.Forms.Label lblRoleDesc;
		private System.Windows.Forms.ComboBox cbRole;
		private System.Windows.Forms.Label lblRole;

		private ArrayList removedUsers;
		private System.Windows.Forms.ComboBox cbRoleName;
		private System.Windows.Forms.TabPage tabPageRoles;
		private System.Windows.Forms.TabPage tabPageUserXRoles;
		private ArrayList addedUsers;

		private string prevSelValuecbRole = "";		
		private bool saveForPrevIndex = false;

		ArrayList originalSelectedUserList;

		public ApplUsersXRoles()
		{
			InitializeComponent();
			this.CenterToScreen();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentApplUsersXRole = new ApplUsersXRole();
			logInUser = NotificationController.GetLogInUser();
			removedUsers = new ArrayList();
			addedUsers = new ArrayList();
			originalSelectedUserList = new ArrayList();

			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(ApplUsersXRoles).Assembly);
			setLanguage();

			populateUserCombo();
			populateRoleCombo(cbRoleName);
			populateRoleCombo(cbRole);
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageUsers = new System.Windows.Forms.TabPage();
            this.lblRolesForUsers = new System.Windows.Forms.Label();
            this.lvRoles = new System.Windows.Forms.ListView();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.cbUser = new System.Windows.Forms.ComboBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.tabPageRoles = new System.Windows.Forms.TabPage();
            this.lblUserForRole = new System.Windows.Forms.Label();
            this.lvUser = new System.Windows.Forms.ListView();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.cbRoleName = new System.Windows.Forms.ComboBox();
            this.lblRoleName = new System.Windows.Forms.Label();
            this.tabPageUserXRoles = new System.Windows.Forms.TabPage();
            this.lblSelUsers = new System.Windows.Forms.Label();
            this.lblUsersForRole = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.btnAddAll = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lvSelectedUsers = new System.Windows.Forms.ListView();
            this.lvUsers = new System.Windows.Forms.ListView();
            this.tbRoleDesc = new System.Windows.Forms.TextBox();
            this.lblRoleDesc = new System.Windows.Forms.Label();
            this.cbRole = new System.Windows.Forms.ComboBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPageUsers.SuspendLayout();
            this.tabPageRoles.SuspendLayout();
            this.tabPageUserXRoles.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageUsers);
            this.tabControl1.Controls.Add(this.tabPageRoles);
            this.tabControl1.Controls.Add(this.tabPageUserXRoles);
            this.tabControl1.Location = new System.Drawing.Point(8, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(616, 408);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageUsers
            // 
            this.tabPageUsers.Controls.Add(this.lblRolesForUsers);
            this.tabPageUsers.Controls.Add(this.lvRoles);
            this.tabPageUsers.Controls.Add(this.tbName);
            this.tabPageUsers.Controls.Add(this.lblName);
            this.tabPageUsers.Controls.Add(this.cbUser);
            this.tabPageUsers.Controls.Add(this.lblUser);
            this.tabPageUsers.Location = new System.Drawing.Point(4, 22);
            this.tabPageUsers.Name = "tabPageUsers";
            this.tabPageUsers.Size = new System.Drawing.Size(608, 382);
            this.tabPageUsers.TabIndex = 0;
            this.tabPageUsers.Text = "Users";
            // 
            // lblRolesForUsers
            // 
            this.lblRolesForUsers.Location = new System.Drawing.Point(16, 96);
            this.lblRolesForUsers.Name = "lblRolesForUsers";
            this.lblRolesForUsers.Size = new System.Drawing.Size(344, 48);
            this.lblRolesForUsers.TabIndex = 4;
            this.lblRolesForUsers.Text = "List of all Roles for selected User:";
            this.lblRolesForUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvRoles
            // 
            this.lvRoles.FullRowSelect = true;
            this.lvRoles.GridLines = true;
            this.lvRoles.HideSelection = false;
            this.lvRoles.Location = new System.Drawing.Point(16, 152);
            this.lvRoles.Name = "lvRoles";
            this.lvRoles.Size = new System.Drawing.Size(344, 200);
            this.lvRoles.TabIndex = 5;
            this.lvRoles.UseCompatibleStateImageBehavior = false;
            this.lvRoles.View = System.Windows.Forms.View.Details;
            this.lvRoles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvRole_ColumnClick);
            // 
            // tbName
            // 
            this.tbName.Enabled = false;
            this.tbName.Location = new System.Drawing.Point(112, 56);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(224, 20);
            this.tbName.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(16, 56);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(80, 23);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbUser
            // 
            this.cbUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUser.Location = new System.Drawing.Point(112, 24);
            this.cbUser.Name = "cbUser";
            this.cbUser.Size = new System.Drawing.Size(224, 21);
            this.cbUser.TabIndex = 1;
            this.cbUser.SelectedIndexChanged += new System.EventHandler(this.cbUser_SelectedIndexChanged);
            // 
            // lblUser
            // 
            this.lblUser.Location = new System.Drawing.Point(16, 24);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(80, 23);
            this.lblUser.TabIndex = 0;
            this.lblUser.Text = "User ID:";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPageRoles
            // 
            this.tabPageRoles.Controls.Add(this.lblUserForRole);
            this.tabPageRoles.Controls.Add(this.lvUser);
            this.tabPageRoles.Controls.Add(this.tbDesc);
            this.tabPageRoles.Controls.Add(this.lblDesc);
            this.tabPageRoles.Controls.Add(this.cbRoleName);
            this.tabPageRoles.Controls.Add(this.lblRoleName);
            this.tabPageRoles.Location = new System.Drawing.Point(4, 22);
            this.tabPageRoles.Name = "tabPageRoles";
            this.tabPageRoles.Size = new System.Drawing.Size(608, 382);
            this.tabPageRoles.TabIndex = 1;
            this.tabPageRoles.Text = "Roles";
            // 
            // lblUserForRole
            // 
            this.lblUserForRole.Location = new System.Drawing.Point(16, 96);
            this.lblUserForRole.Name = "lblUserForRole";
            this.lblUserForRole.Size = new System.Drawing.Size(344, 48);
            this.lblUserForRole.TabIndex = 9;
            this.lblUserForRole.Text = "List of all Users granted selected Role:";
            this.lblUserForRole.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvUser
            // 
            this.lvUser.FullRowSelect = true;
            this.lvUser.GridLines = true;
            this.lvUser.HideSelection = false;
            this.lvUser.Location = new System.Drawing.Point(16, 152);
            this.lvUser.Name = "lvUser";
            this.lvUser.Size = new System.Drawing.Size(344, 200);
            this.lvUser.TabIndex = 10;
            this.lvUser.UseCompatibleStateImageBehavior = false;
            this.lvUser.View = System.Windows.Forms.View.Details;
            this.lvUser.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvUser_ColumnClick);
            // 
            // tbDesc
            // 
            this.tbDesc.Enabled = false;
            this.tbDesc.Location = new System.Drawing.Point(112, 56);
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(224, 20);
            this.tbDesc.TabIndex = 8;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(16, 56);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(72, 23);
            this.lblDesc.TabIndex = 7;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbRoleName
            // 
            this.cbRoleName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRoleName.Location = new System.Drawing.Point(112, 24);
            this.cbRoleName.Name = "cbRoleName";
            this.cbRoleName.Size = new System.Drawing.Size(224, 21);
            this.cbRoleName.TabIndex = 6;
            this.cbRoleName.SelectedIndexChanged += new System.EventHandler(this.cbRoleName_SelectedIndexChanged);
            // 
            // lblRoleName
            // 
            this.lblRoleName.Location = new System.Drawing.Point(16, 24);
            this.lblRoleName.Name = "lblRoleName";
            this.lblRoleName.Size = new System.Drawing.Size(72, 23);
            this.lblRoleName.TabIndex = 5;
            this.lblRoleName.Text = "Name:";
            this.lblRoleName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPageUserXRoles
            // 
            this.tabPageUserXRoles.Controls.Add(this.lblSelUsers);
            this.tabPageUserXRoles.Controls.Add(this.lblUsersForRole);
            this.tabPageUserXRoles.Controls.Add(this.btnCancel);
            this.tabPageUserXRoles.Controls.Add(this.btnSave);
            this.tabPageUserXRoles.Controls.Add(this.btnRemove);
            this.tabPageUserXRoles.Controls.Add(this.btnRemoveAll);
            this.tabPageUserXRoles.Controls.Add(this.btnAddAll);
            this.tabPageUserXRoles.Controls.Add(this.btnAdd);
            this.tabPageUserXRoles.Controls.Add(this.lvSelectedUsers);
            this.tabPageUserXRoles.Controls.Add(this.lvUsers);
            this.tabPageUserXRoles.Controls.Add(this.tbRoleDesc);
            this.tabPageUserXRoles.Controls.Add(this.lblRoleDesc);
            this.tabPageUserXRoles.Controls.Add(this.cbRole);
            this.tabPageUserXRoles.Controls.Add(this.lblRole);
            this.tabPageUserXRoles.Location = new System.Drawing.Point(4, 22);
            this.tabPageUserXRoles.Name = "tabPageUserXRoles";
            this.tabPageUserXRoles.Size = new System.Drawing.Size(608, 382);
            this.tabPageUserXRoles.TabIndex = 2;
            this.tabPageUserXRoles.Text = "User <-> Roles";
            // 
            // lblSelUsers
            // 
            this.lblSelUsers.Location = new System.Drawing.Point(344, 80);
            this.lblSelUsers.Name = "lblSelUsers";
            this.lblSelUsers.Size = new System.Drawing.Size(248, 48);
            this.lblSelUsers.TabIndex = 15;
            this.lblSelUsers.Text = "List of all Users who are granted selected Role:";
            this.lblSelUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUsersForRole
            // 
            this.lblUsersForRole.Location = new System.Drawing.Point(16, 80);
            this.lblUsersForRole.Name = "lblUsersForRole";
            this.lblUsersForRole.Size = new System.Drawing.Size(248, 48);
            this.lblUsersForRole.TabIndex = 9;
            this.lblUsersForRole.Text = "List of all Users who are not granted selected Role:";
            this.lblUsersForRole.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(520, 344);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 344);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(280, 272);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(48, 23);
            this.btnRemove.TabIndex = 14;
            this.btnRemove.Text = "<";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Location = new System.Drawing.Point(280, 240);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(48, 23);
            this.btnRemoveAll.TabIndex = 13;
            this.btnRemoveAll.Text = "<<";
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // btnAddAll
            // 
            this.btnAddAll.Location = new System.Drawing.Point(280, 208);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(48, 23);
            this.btnAddAll.TabIndex = 12;
            this.btnAddAll.Text = ">>";
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(280, 176);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(48, 23);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = ">";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvSelectedUsers
            // 
            this.lvSelectedUsers.FullRowSelect = true;
            this.lvSelectedUsers.GridLines = true;
            this.lvSelectedUsers.HideSelection = false;
            this.lvSelectedUsers.Location = new System.Drawing.Point(344, 136);
            this.lvSelectedUsers.Name = "lvSelectedUsers";
            this.lvSelectedUsers.Size = new System.Drawing.Size(248, 200);
            this.lvSelectedUsers.TabIndex = 16;
            this.lvSelectedUsers.UseCompatibleStateImageBehavior = false;
            this.lvSelectedUsers.View = System.Windows.Forms.View.Details;
            this.lvSelectedUsers.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvSelectedUsers_ColumnClick);
            // 
            // lvUsers
            // 
            this.lvUsers.FullRowSelect = true;
            this.lvUsers.GridLines = true;
            this.lvUsers.HideSelection = false;
            this.lvUsers.Location = new System.Drawing.Point(16, 136);
            this.lvUsers.Name = "lvUsers";
            this.lvUsers.Size = new System.Drawing.Size(248, 200);
            this.lvUsers.TabIndex = 10;
            this.lvUsers.UseCompatibleStateImageBehavior = false;
            this.lvUsers.View = System.Windows.Forms.View.Details;
            this.lvUsers.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvUsers_ColumnClick);
            // 
            // tbRoleDesc
            // 
            this.tbRoleDesc.Enabled = false;
            this.tbRoleDesc.Location = new System.Drawing.Point(184, 48);
            this.tbRoleDesc.Name = "tbRoleDesc";
            this.tbRoleDesc.Size = new System.Drawing.Size(240, 20);
            this.tbRoleDesc.TabIndex = 8;
            // 
            // lblRoleDesc
            // 
            this.lblRoleDesc.Location = new System.Drawing.Point(48, 51);
            this.lblRoleDesc.Name = "lblRoleDesc";
            this.lblRoleDesc.Size = new System.Drawing.Size(120, 23);
            this.lblRoleDesc.TabIndex = 7;
            this.lblRoleDesc.Text = "Description";
            this.lblRoleDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbRole
            // 
            this.cbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRole.Location = new System.Drawing.Point(184, 16);
            this.cbRole.Name = "cbRole";
            this.cbRole.Size = new System.Drawing.Size(240, 21);
            this.cbRole.TabIndex = 6;
            this.cbRole.SelectedIndexChanged += new System.EventHandler(this.cbRole_SelectedIndexChanged);
            // 
            // lblRole
            // 
            this.lblRole.Location = new System.Drawing.Point(48, 19);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(120, 23);
            this.lblRole.TabIndex = 5;
            this.lblRole.Text = "Role:";
            this.lblRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // ApplUsersXRoles
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(632, 454);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 488);
            this.MinimumSize = new System.Drawing.Size(640, 488);
            this.Name = "ApplUsersXRoles";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "User <-> Role";
            this.Load += new System.EventHandler(this.ApplUsersXRoles_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ApplUsersXRoles_KeyUp);
            this.tabControl1.ResumeLayout(false);
            this.tabPageUsers.ResumeLayout(false);
            this.tabPageUsers.PerformLayout();
            this.tabPageRoles.ResumeLayout(false);
            this.tabPageRoles.PerformLayout();
            this.tabPageUserXRoles.ResumeLayout(false);
            this.tabPageUserXRoles.PerformLayout();
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
					case ApplUsersXRoles.UserIDIndex:
					case ApplUsersXRoles.NameIndex:
					case ApplUsersXRoles.DescriptionIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
					}
					
					default:
					{
						return 0;
					}

				}
			}

		}

		#endregion

		/// <summary>
		/// Set proper language.
		/// </summary>
		public void setLanguage()
		{
			try
			{
				// Form name
				this.Text = rm.GetString("formUserXRole", culture);

				// TabPage text
				tabPageUsers.Text = rm.GetString("tabPageUsers", culture);
				tabPageRoles.Text = rm.GetString("tabPageRoles", culture);
				tabPageUserXRoles.Text = rm.GetString("tabPageUserXRoles", culture);
				
				// button's text
				btnClose.Text = rm.GetString("btnClose", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
				btnSave.Text = rm.GetString("btnSave", culture);

				// label's text
				lblUser.Text = rm.GetString("lblUserID", culture);
				lblName.Text = rm.GetString("lblName", culture);
				lblRoleName.Text = rm.GetString("lblName", culture);
				lblDesc.Text = rm.GetString("lblDescription", culture);
				lblRole.Text = rm.GetString("lblRole", culture);
				lblRoleDesc.Text = rm.GetString("lblDescription", culture);
				lblRolesForUsers.Text = rm.GetString("lblRolesForUsers", culture);
				lblUserForRole.Text = rm.GetString("lblUserForRole", culture);
				lblUsersForRole.Text = rm.GetString("lblUsersForRole", culture);
				lblSelUsers.Text = rm.GetString("lblSelUsersForRole", culture);
				
				// list view
				lvRoles.BeginUpdate();
				lvRoles.Columns.Add(rm.GetString("lblName", culture), (lvRoles.Width - 4) / 2, HorizontalAlignment.Left);
				lvRoles.Columns.Add(rm.GetString("lblDescription", culture), (lvRoles.Width - 4) / 2, HorizontalAlignment.Left);
				lvRoles.EndUpdate();

				lvUser.BeginUpdate();
				lvUser.Columns.Add(rm.GetString("lblUserID", culture), (lvUser.Width - 6) / 3, HorizontalAlignment.Left);
				lvUser.Columns.Add(rm.GetString("lblName", culture), (lvUser.Width - 6) / 3, HorizontalAlignment.Left);
				lvUser.Columns.Add(rm.GetString("lblDescription", culture), (lvUser.Width - 6) / 3, HorizontalAlignment.Left);
				lvUser.EndUpdate();

				lvUsers.BeginUpdate();
				lvUsers.Columns.Add(rm.GetString("lblUserID", culture), (lvUsers.Width - 6) / 3, HorizontalAlignment.Left);
				lvUsers.Columns.Add(rm.GetString("lblName", culture), (lvUsers.Width - 6) / 3, HorizontalAlignment.Left);
				lvUsers.Columns.Add(rm.GetString("lblDescription", culture), (lvUsers.Width - 6) / 3, HorizontalAlignment.Left);
				lvUsers.EndUpdate();

				lvSelectedUsers.BeginUpdate();
				lvSelectedUsers.Columns.Add(rm.GetString("lblUserID", culture), (lvSelectedUsers.Width - 6) / 3, HorizontalAlignment.Left);
				lvSelectedUsers.Columns.Add(rm.GetString("lblName", culture), (lvSelectedUsers.Width - 6) / 3, HorizontalAlignment.Left);
				lvSelectedUsers.Columns.Add(rm.GetString("lblDescription", culture), (lvSelectedUsers.Width - 6) / 3, HorizontalAlignment.Left);
				lvSelectedUsers.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRoles.setLanguage(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void populateUserCombo()
		{
			try
			{
                List<string> statuses = new List<string>();
                statuses.Add(Constants.statusActive);
                statuses.Add(Constants.statusDisabled);
                
                List<ApplUserTO> userArray = new ApplUser().SearchWithStatus(statuses);
				userArray.Insert(0, new ApplUserTO(rm.GetString("all", culture), "", "", "", -1, "", -1, ""));

				this.cbUser.DataSource = userArray;
				this.cbUser.DisplayMember = "UserID";
				this.cbUser.ValueMember = "UserID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRoles.populateUserCombo(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		
		private void populateRoleCombo(ComboBox cbName)
		{
			try
			{				
				List<ApplRoleTO> roleArray = new ApplRole().SearchUserCreatedRoles();
				roleArray.Insert(0, new ApplRoleTO(-1, rm.GetString("all", culture), rm.GetString("all", culture)));

				cbName.DataSource = roleArray;
				cbName.DisplayMember = "Name";
				cbName.ValueMember = "ApplRoleID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRoles.populateRoleCombo(): " + ex.Message + "\n");
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

		private void populateRoleListView(List<ApplRoleTO> roleList)
		{
			try
			{
				lvRoles.BeginUpdate();
				lvRoles.Items.Clear();
				
				if (roleList.Count > 0)
				{
					foreach(ApplRoleTO role in roleList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = role.Name.Trim();
						item.SubItems.Add(role.Description.ToString().Trim());
											
						lvRoles.Items.Add(item);
					}
				}

				lvRoles.EndUpdate();
				lvRoles.Invalidate();
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRoles.populateRoleListView(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void populateUserListView(List<ApplUserTO> userList, ListView lvName)
		{
			try
			{
				lvName.BeginUpdate();
				lvName.Items.Clear();
				
				if (userList.Count > 0)
				{
					foreach(ApplUserTO user in userList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = user.UserID.Trim();
						item.SubItems.Add(user.Name.Trim());
						item.SubItems.Add(user.Description.Trim());
											
						lvName.Items.Add(item);
					}
				}

				lvName.EndUpdate();
				lvName.Invalidate();
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRoles.populateUserListView(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
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
                log.writeLog(DateTime.Now + " ApplUsersXRoles.btnClose_Click(): " + ex.Message + "\n");
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

                if (this.cbRole.SelectedIndex > 0)
                {
                    removedUsers.Clear();
                    addedUsers.Clear();
                    this.cbRole.SelectedIndex = 0;

                    MessageBox.Show(rm.GetString("userXRolesNotSaved", culture));
                }
                else
                {
                    removedUsers.Clear();
                    addedUsers.Clear();

                    clearListView(lvSelectedUsers);

                    List<ApplUserTO> userList = new ApplUser().Search();
                    populateUserListView(userList, this.lvUsers);
                }
            }catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersXRoles.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbUser_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;

				if (this.cbUser.SelectedIndex > 0)
				{
					ApplUserTO applUserTO = new ApplUser().Find(cbUser.SelectedValue.ToString().Trim());
					if (!applUserTO.UserID.Trim().Equals(""))
					{
						this.tbName.Text = applUserTO.Name.Trim();

						populateRoleListView(new ApplUsersXRole().FindRolesForUser(applUserTO.UserID.Trim()));
					}
				}
				else
				{
					this.tbName.Text = "";
					clearListView(lvRoles);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRoles.cbUser_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbRoleName_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
                
				if (this.cbRoleName.SelectedIndex > 0)
                {
                    this.Cursor = Cursors.WaitCursor;				
					ApplRoleTO role = new ApplRole().Find((int) cbRoleName.SelectedValue);
					if (role.ApplRoleID != -1)
					{
						this.tbDesc.Text = role.Description.Trim();

						populateUserListView(new ApplUsersXRole().FindUsersForRoleID(role.ApplRoleID), this.lvUser);
					}
				}
				else
				{
					this.tbDesc.Text = "";
					clearListView(lvUser);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRoles.cbRoleName_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void cbRole_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				if (removedUsers.Count > 0 || addedUsers.Count > 0)
				{
					DialogResult result = MessageBox.Show(rm.GetString("UserXRoleSaveChanges", culture), "", MessageBoxButtons.YesNo);
					if (result == DialogResult.Yes)
					{
						saveForPrevIndex = true;
						btnSave.PerformClick();
					}
				}

				if (this.cbRole.SelectedIndex > 0)
				{					
					ApplRoleTO role  = new ApplRole().Find((int) cbRole.SelectedValue);
					if (role.ApplRoleID != -1)
					{
						this.tbRoleDesc.Text = role.Description.Trim();
						
						populateUserListView(new ApplUsersXRole().FindUsersForRoleID(role.ApplRoleID), this.lvSelectedUsers);
						foreach(ListViewItem item in lvSelectedUsers.Items)
						{
							originalSelectedUserList.Add(item);
						}

                        List<string> statuses = new List<string>();
                        statuses.Add(Constants.statusActive);
                        statuses.Add(Constants.statusDisabled);
                        List<ApplUserTO> userList = new ApplUser().SearchWithStatus(statuses);
						populateUserListView(userList, this.lvUsers);

						foreach(ListViewItem item2 in lvSelectedUsers.Items)
						{
							foreach(ListViewItem item1 in lvUsers.Items)
							{
								if (item2.Text.Equals(item1.Text))
								{
									lvUsers.Items.Remove(item1);
								}
							}
						}
					}
				}
				else
				{
					this.tbRoleDesc.Text = "";
					clearListView(lvSelectedUsers);
                    List<string> statuses = new List<string>();
                    statuses.Add(Constants.statusActive);
                    statuses.Add(Constants.statusDisabled);
                    List<ApplUserTO> userList = new ApplUser().SearchWithStatus(statuses);
					populateUserListView(userList, this.lvUsers);
				}

				removedUsers.Clear();
				addedUsers.Clear();

				if (cbRole.SelectedIndex != 0)
					prevSelValuecbRole = cbRole.SelectedValue.ToString();
				else
					prevSelValuecbRole = "ALL";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRoles.cbRole_SelectedIndexChanged(): " + ex.Message + "\n");
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

				if(cbRole.SelectedIndex > 0)
				{
					foreach(ListViewItem item in lvUsers.SelectedItems)
					{
						lvUsers.Items.Remove(item);
						lvSelectedUsers.Items.Add(item);
						if (!originalSelectedUserList.Contains(item))
							addedUsers.Add(item);
						if (removedUsers.Contains(item))
							removedUsers.Remove(item);
					}
				}
				else
				{
					MessageBox.Show(rm.GetString("SelectRole", culture));
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRoles.btnAdd_Click(): " + ex.Message + "\n");
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
				if(cbRole.SelectedIndex > 0)
				{
					foreach(ListViewItem item in lvUsers.Items)
					{
						lvUsers.Items.Remove(item);
						lvSelectedUsers.Items.Add(item);
						if (!originalSelectedUserList.Contains(item))
							addedUsers.Add(item);
						if (removedUsers.Contains(item))
							removedUsers.Remove(item);
					}
				}
				else
				{
					MessageBox.Show(rm.GetString("SelectRole", culture));
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRoles.btnAdd_Click(): " + ex.Message + "\n");
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
				foreach(ListViewItem item in lvSelectedUsers.Items)
				{
					if ((((int) cbRole.SelectedValue) == 0) && item.Text.Trim().Equals("SYS"))
					{
						MessageBox.Show(rm.GetString("SYSAllPermission", culture));
					}
					else
					{
						lvSelectedUsers.Items.Remove(item);
						lvUsers.Items.Add(item);
						if (originalSelectedUserList.Contains(item))
							removedUsers.Add(item);
						if (addedUsers.Contains(item))
							addedUsers.Remove(item);
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRoles.btnRemove_Click(): " + ex.Message + "\n");
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
				foreach(ListViewItem item in lvSelectedUsers.SelectedItems)
				{
					if ((((int) cbRole.SelectedValue) == 0) && item.Text.Trim().Equals("SYS"))
					{
						MessageBox.Show(rm.GetString("SYSAllPermission", culture));
					}
					else
					{
						lvSelectedUsers.Items.Remove(item);
						lvUsers.Items.Add(item);
						if (originalSelectedUserList.Contains(item))
							removedUsers.Add(item);
						if (addedUsers.Contains(item))
							addedUsers.Remove(item);
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRoles.btnRemove_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvRole_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				SortOrder prevOrder = lvRoles.Sorting;
				lvRoles.Sorting = SortOrder.None;

				if (e.Column == _comp1.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvRoles.Sorting = SortOrder.Descending;
					}
					else
					{
						lvRoles.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp1.SortColumn = e.Column;
					lvRoles.Sorting = SortOrder.Ascending;
				}
                lvRoles.ListViewItemSorter = _comp1;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRoles.lvRole_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvUser_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				SortOrder prevOrder = lvUser.Sorting;
				lvUser.Sorting = SortOrder.None;

				if (e.Column == _comp2.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvUser.Sorting = SortOrder.Descending;
					}
					else
					{
						lvUser.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp2.SortColumn = e.Column;
					lvUser.Sorting = SortOrder.Ascending;
				}
                lvUser.ListViewItemSorter = _comp2;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRoles.lvUser_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvUsers_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				SortOrder prevOrder = lvUsers.Sorting;
				lvUsers.Sorting = SortOrder.None;

				if (e.Column == _comp3.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvUsers.Sorting = SortOrder.Descending;
					}
					else
					{
						lvUsers.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp3.SortColumn = e.Column;
					lvUsers.Sorting = SortOrder.Ascending;
				}
                lvUsers.ListViewItemSorter = _comp3;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRoles.lvUsers_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvSelectedUsers_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				SortOrder prevOrder = lvSelectedUsers.Sorting;
				lvSelectedUsers.Sorting = SortOrder.None;

				if (e.Column == _comp4.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvSelectedUsers.Sorting = SortOrder.Descending;
					}
					else
					{
						lvSelectedUsers.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp4.SortColumn = e.Column;
					lvSelectedUsers.Sorting = SortOrder.Ascending;
				}
                lvSelectedUsers.ListViewItemSorter = _comp4;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRoles.lvUsers_ColumnClick(): " + ex.Message + "\n");
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

				if ((saveForPrevIndex && !prevSelValuecbRole.ToUpper().Equals("ALL")) || (cbRole.SelectedIndex > 0))
				//if(cbRole.SelectedIndex > 0)
				{
					bool isUpdated = true;

					isUpdated = currentApplUsersXRole.BeginTransaction();
					
					if (isUpdated)
					{
						string roleID;
						if (saveForPrevIndex)							
							roleID = prevSelValuecbRole;
						else
							roleID = cbRole.SelectedValue.ToString();

						foreach(ListViewItem item in removedUsers)
						{
							//isUpdated = currentApplUsersXRole.Delete(item.Text.Trim(),(int) cbRole.SelectedValue, false) && isUpdated;
							isUpdated = currentApplUsersXRole.Delete(item.Text.Trim(), int.Parse(roleID), false) && isUpdated;
						}

						foreach(ListViewItem item in lvSelectedUsers.Items)
						{
							//isUpdated = currentApplUsersXRole.Delete(item.Text.Trim(), (int) cbRole.SelectedValue, false) && isUpdated;
							isUpdated = currentApplUsersXRole.Delete(item.Text.Trim(), int.Parse(roleID), false) && isUpdated;
						}

						foreach(ListViewItem item in lvSelectedUsers.Items)
						{
							//isUpdated = (currentApplUsersXRole.Save(item.Text.Trim(), (int) cbRole.SelectedValue, false) == 1 ? true : false) && isUpdated;
							isUpdated = (currentApplUsersXRole.Save(item.Text.Trim(), int.Parse(roleID), false) == 1 ? true : false) && isUpdated;
						}
					}

					if(isUpdated)
					{
						currentApplUsersXRole.CommitTransaction();

						//to refresh lvRoles list
						cbUser_SelectedIndexChanged(sender, e);
						//to refresh lvUser list
						cbRoleName_SelectedIndexChanged(sender, e);

						MessageBox.Show(rm.GetString("applUserXRoleSaved", culture));
						removedUsers.Clear();
						addedUsers.Clear();
						//this.Close();
					}
				}
				else
				{
					MessageBox.Show(rm.GetString("SelectRole", culture));
				}

				saveForPrevIndex = false;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUserXWU.btnSave_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void ApplUsersXRoles_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer objects
                _comp1 = new ListViewItemComparer(lvRoles);
                lvRoles.ListViewItemSorter = _comp1;
                lvRoles.Sorting = SortOrder.Ascending;

                _comp2 = new ListViewItemComparer(lvUser);
                lvUser.ListViewItemSorter = _comp2;
                lvUser.Sorting = SortOrder.Ascending;

                _comp3 = new ListViewItemComparer(lvUsers);
                lvUsers.ListViewItemSorter = _comp3;
                lvUsers.Sorting = SortOrder.Ascending;

                _comp4 = new ListViewItemComparer(lvSelectedUsers);
                lvSelectedUsers.ListViewItemSorter = _comp4;
                lvSelectedUsers.Sorting = SortOrder.Ascending;
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " ApplUserXWU.ApplUsersXRoles_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void ApplUsersXRoles_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " ApplUsersXRoles.ApplUsersXRoles_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
