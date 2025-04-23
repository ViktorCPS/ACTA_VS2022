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
	/// Summary description for ApplUserXWU.
	/// </summary>
	public class ApplUserXWU : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TabPage tabPageUsers;
		private System.Windows.Forms.TabPage tabPageUnits;
		private System.Windows.Forms.TabPage tabPageUserXWU;
		private System.Windows.Forms.Label lblUser;
		private System.Windows.Forms.ComboBox cbUser;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.ListView lvWU;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.ComboBox cbWUName;
        private System.Windows.Forms.Label lblWUName;
		private System.Windows.Forms.ListView lvSelectedUsers;
        private System.Windows.Forms.Button btnAdd;
		/// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblWUForUsers;
		private System.Windows.Forms.Label lblUserForWU;
		private System.Windows.Forms.Label lblUsersForWU;
		private System.Windows.Forms.Label lblSelUsers;
		private System.Windows.Forms.ListView lvUser;
		private System.Windows.Forms.ListView lvUsers;
        private System.Windows.Forms.Label lblPurpose;
        private System.Windows.Forms.ComboBox cbPurpose;
        private System.Windows.Forms.Label lblPurpose1;
        private System.Windows.Forms.ComboBox cbPurpose1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private ListView lvPurposes;
        private ListView lvWorkingUnits;
        private Label lblWorkingUnits;
        private Label lblPurposes;
        private Button btnWUTree;
        private Button btnRemoveSelected;
        private GroupBox gbOUnits;
        private ComboBox cbOUUser;
        private Label lblOUList;
        private ComboBox cbOUPurpose;
        private ListView lvOU;
        private Label lblOUUserID;
        private Label lblOUPurpose;
        private Label lblOUName;
        private TextBox tbOUName;
        private GroupBox gbWUnits;
        private GroupBox gbOrganizationalUnits;
        private Label lblOU;
        private Button btnOUTree;
        private ComboBox cbOUName;
        private ComboBox cbOUPurpose1;
        private Label lblOUDesc;
        private Label lblOUPurpose1;
        private TextBox tbOUDesc;
        private Label lblUserForOU;
        private ListView lvOUUser;
        private GroupBox gbWorkingUnits;
        private TabPage tabPageUserXOU;
        private Button btnOURemoveSelected;
        private ListView lvOUPurposes;
        private ListView lvOUnits;
        private Label lblOUnitsList;
        private Label lblOUPurposes;
        private Label label5;
        private Label label6;
        private Label lblOUSelUsers;
        private Label lblUsersNotXOU;
        private Button btnOUCancel;
        private Button btnOUSave;
        private Button btnOUAdd;
        private ListView lvOUSelectedUsers;
        private ListView lvOUUsers;

		ApplUserTO logInUser;

		private CultureInfo culture;
		ResourceManager rm;
		DebugLog log;

		// List View indexes
		const int UserIDIndex = 0;
		const int NameIndex = 1;
		const int DescriptionIndex = 2;
        const int WUIndex = 3;
        const int PurposeIndex = 4;

		private ListViewItemComparer _comp1;
		private ListViewItemComparer _comp2;
		private ListViewItemComparer _comp3;
		private ListViewItemComparer _comp4;
        private ListViewItemComparer _comp5;
        private ListViewItemComparer _comp6;
        private ListViewItemComparer _comp7;
        private ListViewItemComparer _comp8;
        private ListViewItemComparer _comp9;
        private ListViewItemComparer _comp10;
        private ListViewItemComparer _comp11;
        private ListViewItemComparer _comp12;

        private List<ListViewItem> originalSelectedUserListWU = new List<ListViewItem>();
        private List<ListViewItem> removedUsersWU = new List<ListViewItem>();
        private List<ListViewItem> addedUsersWU = new List<ListViewItem>();

        private List<ListViewItem> originalSelectedUserListOU = new List<ListViewItem>();
        private List<ListViewItem> removedUsersOU = new List<ListViewItem>();
        private List<ListViewItem> addedUsersOU = new List<ListViewItem>();

        private bool showWUMessage = true;
        private bool showOUMessage = true;

        List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();
        private CheckBox chbRetiredUsersOU;
        private CheckBox chbRetiredUsersWU;
        private CheckBox chbRetiredUnitsOU;
        private CheckBox chbRetiredUnitsWU;        
        List<OrganizationalUnitTO> ouArray = new List<OrganizationalUnitTO>();

        public struct Item
        {
            public string _userID;
            public WorkingUnit _wu;
            public OrganizationalUnit _ou;
            public string _purpose;

            public string UserID
            {
                get { return _userID; }
                set { _userID = value; }
            }

            public WorkingUnit WU
            {
                get { return _wu; }
                set { _wu = value; }
            }

            public OrganizationalUnit OU
            {
                get { return _ou; }
                set { _ou = value; }
            }

            public string Purpose
            {
                get { return _purpose; }
                set { _purpose = value; }
            }

            public bool equals(ListViewItem item)
            {
                if (this.WU != null && this.WU.WUTO.WorkingUnitID != -1)
                    return this.UserID.Equals(item.Text) && this.WU.WUTO.Name.Equals(item.SubItems[3].Text)
                        && this.Purpose.Equals(item.SubItems[4].Text);
                else if (this.OU != null && this.OU.OrgUnitTO.OrgUnitID != -1)
                    return this.UserID.Equals(item.Text) && this.OU.OrgUnitTO.Name.Equals(item.SubItems[3].Text)
                        && this.Purpose.Equals(item.SubItems[4].Text);
                else 
                    return false;
            }
        }

		public ApplUserXWU()
		{
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                                
                logInUser = NotificationController.GetLogInUser();

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(ApplUserXWU).Assembly);
                setLanguage();

                populateUserCombo();
                populateOUUserCombo();
                populateWorkingUnitCombo(cbWUName);
                populateOUCombo(cbOUName);
                populateWorkingUnitLV();
                populateOULV();
                populatePurposeCombo(cbPurpose);
                populatePurposeCombo(cbOUPurpose);
                populatePurposeCombo(cbPurpose1);
                populatePurposeCombo(cbOUPurpose1);
                populatePurposeLV(lvPurposes);
                populatePurposeLV(lvOUPurposes);

                clearListView(lvSelectedUsers);
                clearListView(lvOUSelectedUsers);

                List<string> statuses = new List<string>();
                statuses.Add(Constants.statusActive);
                statuses.Add(Constants.statusDisabled);
                List<ApplUserTO> userList = new ApplUser().SearchWithStatus(statuses);
                populateUserListView(userList, this.lvUsers);
                populateUserListView(userList, this.lvOUUsers);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplUserXWU));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageUsers = new System.Windows.Forms.TabPage();
            this.gbOUnits = new System.Windows.Forms.GroupBox();
            this.chbRetiredUsersOU = new System.Windows.Forms.CheckBox();
            this.cbOUUser = new System.Windows.Forms.ComboBox();
            this.lblOUList = new System.Windows.Forms.Label();
            this.cbOUPurpose = new System.Windows.Forms.ComboBox();
            this.lvOU = new System.Windows.Forms.ListView();
            this.lblOUUserID = new System.Windows.Forms.Label();
            this.lblOUPurpose = new System.Windows.Forms.Label();
            this.lblOUName = new System.Windows.Forms.Label();
            this.tbOUName = new System.Windows.Forms.TextBox();
            this.gbWUnits = new System.Windows.Forms.GroupBox();
            this.chbRetiredUsersWU = new System.Windows.Forms.CheckBox();
            this.cbUser = new System.Windows.Forms.ComboBox();
            this.lblWUForUsers = new System.Windows.Forms.Label();
            this.cbPurpose = new System.Windows.Forms.ComboBox();
            this.lvWU = new System.Windows.Forms.ListView();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblPurpose = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tabPageUnits = new System.Windows.Forms.TabPage();
            this.gbOrganizationalUnits = new System.Windows.Forms.GroupBox();
            this.lblOU = new System.Windows.Forms.Label();
            this.btnOUTree = new System.Windows.Forms.Button();
            this.cbOUName = new System.Windows.Forms.ComboBox();
            this.cbOUPurpose1 = new System.Windows.Forms.ComboBox();
            this.lblOUDesc = new System.Windows.Forms.Label();
            this.lblOUPurpose1 = new System.Windows.Forms.Label();
            this.tbOUDesc = new System.Windows.Forms.TextBox();
            this.lblUserForOU = new System.Windows.Forms.Label();
            this.lvOUUser = new System.Windows.Forms.ListView();
            this.gbWorkingUnits = new System.Windows.Forms.GroupBox();
            this.lblWUName = new System.Windows.Forms.Label();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbWUName = new System.Windows.Forms.ComboBox();
            this.cbPurpose1 = new System.Windows.Forms.ComboBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblPurpose1 = new System.Windows.Forms.Label();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.lblUserForWU = new System.Windows.Forms.Label();
            this.lvUser = new System.Windows.Forms.ListView();
            this.tabPageUserXWU = new System.Windows.Forms.TabPage();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.lvPurposes = new System.Windows.Forms.ListView();
            this.lvWorkingUnits = new System.Windows.Forms.ListView();
            this.lblWorkingUnits = new System.Windows.Forms.Label();
            this.lblPurposes = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSelUsers = new System.Windows.Forms.Label();
            this.lblUsersForWU = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lvSelectedUsers = new System.Windows.Forms.ListView();
            this.lvUsers = new System.Windows.Forms.ListView();
            this.tabPageUserXOU = new System.Windows.Forms.TabPage();
            this.btnOURemoveSelected = new System.Windows.Forms.Button();
            this.lvOUPurposes = new System.Windows.Forms.ListView();
            this.lvOUnits = new System.Windows.Forms.ListView();
            this.lblOUnitsList = new System.Windows.Forms.Label();
            this.lblOUPurposes = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblOUSelUsers = new System.Windows.Forms.Label();
            this.lblUsersNotXOU = new System.Windows.Forms.Label();
            this.btnOUCancel = new System.Windows.Forms.Button();
            this.btnOUSave = new System.Windows.Forms.Button();
            this.btnOUAdd = new System.Windows.Forms.Button();
            this.lvOUSelectedUsers = new System.Windows.Forms.ListView();
            this.lvOUUsers = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.chbRetiredUnitsWU = new System.Windows.Forms.CheckBox();
            this.chbRetiredUnitsOU = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPageUsers.SuspendLayout();
            this.gbOUnits.SuspendLayout();
            this.gbWUnits.SuspendLayout();
            this.tabPageUnits.SuspendLayout();
            this.gbOrganizationalUnits.SuspendLayout();
            this.gbWorkingUnits.SuspendLayout();
            this.tabPageUserXWU.SuspendLayout();
            this.tabPageUserXOU.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageUsers);
            this.tabControl1.Controls.Add(this.tabPageUnits);
            this.tabControl1.Controls.Add(this.tabPageUserXWU);
            this.tabControl1.Controls.Add(this.tabPageUserXOU);
            this.tabControl1.Location = new System.Drawing.Point(8, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(872, 517);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageUsers
            // 
            this.tabPageUsers.Controls.Add(this.gbOUnits);
            this.tabPageUsers.Controls.Add(this.gbWUnits);
            this.tabPageUsers.Location = new System.Drawing.Point(4, 22);
            this.tabPageUsers.Name = "tabPageUsers";
            this.tabPageUsers.Size = new System.Drawing.Size(864, 491);
            this.tabPageUsers.TabIndex = 0;
            this.tabPageUsers.Text = "Users";
            this.tabPageUsers.UseVisualStyleBackColor = true;
            // 
            // gbOUnits
            // 
            this.gbOUnits.Controls.Add(this.chbRetiredUsersOU);
            this.gbOUnits.Controls.Add(this.cbOUUser);
            this.gbOUnits.Controls.Add(this.lblOUList);
            this.gbOUnits.Controls.Add(this.cbOUPurpose);
            this.gbOUnits.Controls.Add(this.lvOU);
            this.gbOUnits.Controls.Add(this.lblOUUserID);
            this.gbOUnits.Controls.Add(this.lblOUPurpose);
            this.gbOUnits.Controls.Add(this.lblOUName);
            this.gbOUnits.Controls.Add(this.tbOUName);
            this.gbOUnits.Location = new System.Drawing.Point(433, 19);
            this.gbOUnits.Name = "gbOUnits";
            this.gbOUnits.Size = new System.Drawing.Size(407, 453);
            this.gbOUnits.TabIndex = 1;
            this.gbOUnits.TabStop = false;
            this.gbOUnits.Text = "Organizational units";
            // 
            // chbRetiredUsersOU
            // 
            this.chbRetiredUsersOU.AutoSize = true;
            this.chbRetiredUsersOU.Location = new System.Drawing.Point(159, 54);
            this.chbRetiredUsersOU.Name = "chbRetiredUsersOU";
            this.chbRetiredUsersOU.Size = new System.Drawing.Size(113, 17);
            this.chbRetiredUsersOU.TabIndex = 2;
            this.chbRetiredUsersOU.Text = "Show retired users";
            this.chbRetiredUsersOU.UseVisualStyleBackColor = true;
            this.chbRetiredUsersOU.CheckedChanged += new System.EventHandler(this.chbRetiredUsersOU_CheckedChanged);
            // 
            // cbOUUser
            // 
            this.cbOUUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOUUser.Location = new System.Drawing.Point(159, 27);
            this.cbOUUser.Name = "cbOUUser";
            this.cbOUUser.Size = new System.Drawing.Size(224, 21);
            this.cbOUUser.TabIndex = 1;
            this.cbOUUser.SelectedIndexChanged += new System.EventHandler(this.cbOUUser_SelectedIndexChanged);
            // 
            // lblOUList
            // 
            this.lblOUList.Location = new System.Drawing.Point(20, 134);
            this.lblOUList.Name = "lblOUList";
            this.lblOUList.Size = new System.Drawing.Size(344, 48);
            this.lblOUList.TabIndex = 7;
            this.lblOUList.Text = "List of all Organizational Units for selected User:";
            this.lblOUList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbOUPurpose
            // 
            this.cbOUPurpose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOUPurpose.Location = new System.Drawing.Point(159, 106);
            this.cbOUPurpose.Name = "cbOUPurpose";
            this.cbOUPurpose.Size = new System.Drawing.Size(224, 21);
            this.cbOUPurpose.TabIndex = 6;
            this.cbOUPurpose.SelectedIndexChanged += new System.EventHandler(this.cbOUPurpose_SelectedIndexChanged);
            // 
            // lvOU
            // 
            this.lvOU.FullRowSelect = true;
            this.lvOU.GridLines = true;
            this.lvOU.HideSelection = false;
            this.lvOU.Location = new System.Drawing.Point(23, 185);
            this.lvOU.Name = "lvOU";
            this.lvOU.Size = new System.Drawing.Size(360, 257);
            this.lvOU.TabIndex = 8;
            this.lvOU.UseCompatibleStateImageBehavior = false;
            this.lvOU.View = System.Windows.Forms.View.Details;
            this.lvOU.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvOU_ColumnClick);
            // 
            // lblOUUserID
            // 
            this.lblOUUserID.Location = new System.Drawing.Point(63, 27);
            this.lblOUUserID.Name = "lblOUUserID";
            this.lblOUUserID.Size = new System.Drawing.Size(80, 23);
            this.lblOUUserID.TabIndex = 0;
            this.lblOUUserID.Text = "User ID:";
            this.lblOUUserID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOUPurpose
            // 
            this.lblOUPurpose.Location = new System.Drawing.Point(63, 104);
            this.lblOUPurpose.Name = "lblOUPurpose";
            this.lblOUPurpose.Size = new System.Drawing.Size(80, 23);
            this.lblOUPurpose.TabIndex = 5;
            this.lblOUPurpose.Text = "Purpose:";
            this.lblOUPurpose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOUName
            // 
            this.lblOUName.Location = new System.Drawing.Point(63, 76);
            this.lblOUName.Name = "lblOUName";
            this.lblOUName.Size = new System.Drawing.Size(80, 23);
            this.lblOUName.TabIndex = 3;
            this.lblOUName.Text = "Name";
            this.lblOUName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbOUName
            // 
            this.tbOUName.Enabled = false;
            this.tbOUName.Location = new System.Drawing.Point(159, 78);
            this.tbOUName.Name = "tbOUName";
            this.tbOUName.Size = new System.Drawing.Size(224, 20);
            this.tbOUName.TabIndex = 4;
            // 
            // gbWUnits
            // 
            this.gbWUnits.Controls.Add(this.chbRetiredUsersWU);
            this.gbWUnits.Controls.Add(this.cbUser);
            this.gbWUnits.Controls.Add(this.lblWUForUsers);
            this.gbWUnits.Controls.Add(this.cbPurpose);
            this.gbWUnits.Controls.Add(this.lvWU);
            this.gbWUnits.Controls.Add(this.lblUser);
            this.gbWUnits.Controls.Add(this.lblPurpose);
            this.gbWUnits.Controls.Add(this.lblName);
            this.gbWUnits.Controls.Add(this.tbName);
            this.gbWUnits.Location = new System.Drawing.Point(16, 19);
            this.gbWUnits.Name = "gbWUnits";
            this.gbWUnits.Size = new System.Drawing.Size(407, 453);
            this.gbWUnits.TabIndex = 0;
            this.gbWUnits.TabStop = false;
            this.gbWUnits.Text = "Working units";
            // 
            // chbRetiredUsersWU
            // 
            this.chbRetiredUsersWU.AutoSize = true;
            this.chbRetiredUsersWU.Location = new System.Drawing.Point(159, 52);
            this.chbRetiredUsersWU.Name = "chbRetiredUsersWU";
            this.chbRetiredUsersWU.Size = new System.Drawing.Size(113, 17);
            this.chbRetiredUsersWU.TabIndex = 2;
            this.chbRetiredUsersWU.Text = "Show retired users";
            this.chbRetiredUsersWU.UseVisualStyleBackColor = true;
            this.chbRetiredUsersWU.CheckedChanged += new System.EventHandler(this.chbRetiredUsersWU_CheckedChanged);
            // 
            // cbUser
            // 
            this.cbUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUser.Location = new System.Drawing.Point(159, 25);
            this.cbUser.Name = "cbUser";
            this.cbUser.Size = new System.Drawing.Size(224, 21);
            this.cbUser.TabIndex = 1;
            this.cbUser.SelectedIndexChanged += new System.EventHandler(this.cbUser_SelectedIndexChanged);
            // 
            // lblWUForUsers
            // 
            this.lblWUForUsers.Location = new System.Drawing.Point(20, 134);
            this.lblWUForUsers.Name = "lblWUForUsers";
            this.lblWUForUsers.Size = new System.Drawing.Size(344, 48);
            this.lblWUForUsers.TabIndex = 7;
            this.lblWUForUsers.Text = "List of all Working Units for selected User:";
            this.lblWUForUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbPurpose
            // 
            this.cbPurpose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPurpose.Location = new System.Drawing.Point(159, 104);
            this.cbPurpose.Name = "cbPurpose";
            this.cbPurpose.Size = new System.Drawing.Size(224, 21);
            this.cbPurpose.TabIndex = 6;
            this.cbPurpose.SelectedIndexChanged += new System.EventHandler(this.cbPurpose_SelectedIndexChanged);
            // 
            // lvWU
            // 
            this.lvWU.FullRowSelect = true;
            this.lvWU.GridLines = true;
            this.lvWU.HideSelection = false;
            this.lvWU.Location = new System.Drawing.Point(23, 185);
            this.lvWU.Name = "lvWU";
            this.lvWU.Size = new System.Drawing.Size(360, 257);
            this.lvWU.TabIndex = 8;
            this.lvWU.UseCompatibleStateImageBehavior = false;
            this.lvWU.View = System.Windows.Forms.View.Details;
            this.lvWU.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvWU_ColumnClick);
            // 
            // lblUser
            // 
            this.lblUser.Location = new System.Drawing.Point(63, 25);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(80, 23);
            this.lblUser.TabIndex = 0;
            this.lblUser.Text = "User ID:";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPurpose
            // 
            this.lblPurpose.Location = new System.Drawing.Point(63, 102);
            this.lblPurpose.Name = "lblPurpose";
            this.lblPurpose.Size = new System.Drawing.Size(80, 23);
            this.lblPurpose.TabIndex = 5;
            this.lblPurpose.Text = "Purpose:";
            this.lblPurpose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(63, 76);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(80, 23);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbName
            // 
            this.tbName.Enabled = false;
            this.tbName.Location = new System.Drawing.Point(159, 78);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(224, 20);
            this.tbName.TabIndex = 4;
            // 
            // tabPageUnits
            // 
            this.tabPageUnits.Controls.Add(this.gbOrganizationalUnits);
            this.tabPageUnits.Controls.Add(this.gbWorkingUnits);
            this.tabPageUnits.Location = new System.Drawing.Point(4, 22);
            this.tabPageUnits.Name = "tabPageUnits";
            this.tabPageUnits.Size = new System.Drawing.Size(864, 491);
            this.tabPageUnits.TabIndex = 1;
            this.tabPageUnits.Text = "Units";
            this.tabPageUnits.UseVisualStyleBackColor = true;
            // 
            // gbOrganizationalUnits
            // 
            this.gbOrganizationalUnits.Controls.Add(this.chbRetiredUnitsOU);
            this.gbOrganizationalUnits.Controls.Add(this.lblOU);
            this.gbOrganizationalUnits.Controls.Add(this.btnOUTree);
            this.gbOrganizationalUnits.Controls.Add(this.cbOUName);
            this.gbOrganizationalUnits.Controls.Add(this.cbOUPurpose1);
            this.gbOrganizationalUnits.Controls.Add(this.lblOUDesc);
            this.gbOrganizationalUnits.Controls.Add(this.lblOUPurpose1);
            this.gbOrganizationalUnits.Controls.Add(this.tbOUDesc);
            this.gbOrganizationalUnits.Controls.Add(this.lblUserForOU);
            this.gbOrganizationalUnits.Controls.Add(this.lvOUUser);
            this.gbOrganizationalUnits.Location = new System.Drawing.Point(438, 19);
            this.gbOrganizationalUnits.Name = "gbOrganizationalUnits";
            this.gbOrganizationalUnits.Size = new System.Drawing.Size(416, 453);
            this.gbOrganizationalUnits.TabIndex = 1;
            this.gbOrganizationalUnits.TabStop = false;
            this.gbOrganizationalUnits.Text = "Organizational units";
            // 
            // lblOU
            // 
            this.lblOU.Location = new System.Drawing.Point(22, 23);
            this.lblOU.Name = "lblOU";
            this.lblOU.Size = new System.Drawing.Size(72, 23);
            this.lblOU.TabIndex = 0;
            this.lblOU.Text = "Name:";
            this.lblOU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOUTree
            // 
            this.btnOUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnOUTree.Image")));
            this.btnOUTree.Location = new System.Drawing.Point(348, 25);
            this.btnOUTree.Name = "btnOUTree";
            this.btnOUTree.Size = new System.Drawing.Size(25, 23);
            this.btnOUTree.TabIndex = 2;
            this.btnOUTree.Click += new System.EventHandler(this.btnOUTree_Click);
            // 
            // cbOUName
            // 
            this.cbOUName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOUName.Location = new System.Drawing.Point(118, 25);
            this.cbOUName.Name = "cbOUName";
            this.cbOUName.Size = new System.Drawing.Size(224, 21);
            this.cbOUName.TabIndex = 1;
            this.cbOUName.SelectedIndexChanged += new System.EventHandler(this.cbOUName_SelectedIndexChanged);
            // 
            // cbOUPurpose1
            // 
            this.cbOUPurpose1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOUPurpose1.Location = new System.Drawing.Point(118, 78);
            this.cbOUPurpose1.Name = "cbOUPurpose1";
            this.cbOUPurpose1.Size = new System.Drawing.Size(224, 21);
            this.cbOUPurpose1.TabIndex = 6;
            this.cbOUPurpose1.SelectedIndexChanged += new System.EventHandler(this.cbOUPurpose1_SelectedIndexChanged);
            // 
            // lblOUDesc
            // 
            this.lblOUDesc.Location = new System.Drawing.Point(22, 50);
            this.lblOUDesc.Name = "lblOUDesc";
            this.lblOUDesc.Size = new System.Drawing.Size(72, 23);
            this.lblOUDesc.TabIndex = 3;
            this.lblOUDesc.Text = "Description:";
            this.lblOUDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOUPurpose1
            // 
            this.lblOUPurpose1.Location = new System.Drawing.Point(22, 76);
            this.lblOUPurpose1.Name = "lblOUPurpose1";
            this.lblOUPurpose1.Size = new System.Drawing.Size(72, 23);
            this.lblOUPurpose1.TabIndex = 5;
            this.lblOUPurpose1.Text = "Purpose";
            this.lblOUPurpose1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbOUDesc
            // 
            this.tbOUDesc.Enabled = false;
            this.tbOUDesc.Location = new System.Drawing.Point(118, 50);
            this.tbOUDesc.Name = "tbOUDesc";
            this.tbOUDesc.Size = new System.Drawing.Size(224, 20);
            this.tbOUDesc.TabIndex = 4;
            // 
            // lblUserForOU
            // 
            this.lblUserForOU.Location = new System.Drawing.Point(19, 105);
            this.lblUserForOU.Name = "lblUserForOU";
            this.lblUserForOU.Size = new System.Drawing.Size(344, 48);
            this.lblUserForOU.TabIndex = 7;
            this.lblUserForOU.Text = "List of all Users granted selected Organizational Unit:";
            this.lblUserForOU.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvOUUser
            // 
            this.lvOUUser.FullRowSelect = true;
            this.lvOUUser.GridLines = true;
            this.lvOUUser.HideSelection = false;
            this.lvOUUser.Location = new System.Drawing.Point(22, 156);
            this.lvOUUser.Name = "lvOUUser";
            this.lvOUUser.Size = new System.Drawing.Size(344, 258);
            this.lvOUUser.TabIndex = 8;
            this.lvOUUser.UseCompatibleStateImageBehavior = false;
            this.lvOUUser.View = System.Windows.Forms.View.Details;
            this.lvOUUser.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvOUUser_ColumnClick);
            // 
            // gbWorkingUnits
            // 
            this.gbWorkingUnits.Controls.Add(this.chbRetiredUnitsWU);
            this.gbWorkingUnits.Controls.Add(this.lblWUName);
            this.gbWorkingUnits.Controls.Add(this.btnWUTree);
            this.gbWorkingUnits.Controls.Add(this.cbWUName);
            this.gbWorkingUnits.Controls.Add(this.cbPurpose1);
            this.gbWorkingUnits.Controls.Add(this.lblDesc);
            this.gbWorkingUnits.Controls.Add(this.lblPurpose1);
            this.gbWorkingUnits.Controls.Add(this.tbDesc);
            this.gbWorkingUnits.Controls.Add(this.lblUserForWU);
            this.gbWorkingUnits.Controls.Add(this.lvUser);
            this.gbWorkingUnits.Location = new System.Drawing.Point(16, 19);
            this.gbWorkingUnits.Name = "gbWorkingUnits";
            this.gbWorkingUnits.Size = new System.Drawing.Size(416, 453);
            this.gbWorkingUnits.TabIndex = 0;
            this.gbWorkingUnits.TabStop = false;
            this.gbWorkingUnits.Text = "Working units";
            // 
            // lblWUName
            // 
            this.lblWUName.Location = new System.Drawing.Point(22, 25);
            this.lblWUName.Name = "lblWUName";
            this.lblWUName.Size = new System.Drawing.Size(72, 23);
            this.lblWUName.TabIndex = 0;
            this.lblWUName.Text = "Name:";
            this.lblWUName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(348, 23);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 2;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbWUName
            // 
            this.cbWUName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWUName.Location = new System.Drawing.Point(118, 25);
            this.cbWUName.Name = "cbWUName";
            this.cbWUName.Size = new System.Drawing.Size(224, 21);
            this.cbWUName.TabIndex = 1;
            this.cbWUName.SelectedIndexChanged += new System.EventHandler(this.cbWUName_SelectedIndexChanged);
            // 
            // cbPurpose1
            // 
            this.cbPurpose1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPurpose1.Location = new System.Drawing.Point(118, 78);
            this.cbPurpose1.Name = "cbPurpose1";
            this.cbPurpose1.Size = new System.Drawing.Size(224, 21);
            this.cbPurpose1.TabIndex = 6;
            this.cbPurpose1.SelectedIndexChanged += new System.EventHandler(this.cbPurpose1_SelectedIndexChanged);
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(22, 50);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(72, 23);
            this.lblDesc.TabIndex = 3;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPurpose1
            // 
            this.lblPurpose1.Location = new System.Drawing.Point(22, 76);
            this.lblPurpose1.Name = "lblPurpose1";
            this.lblPurpose1.Size = new System.Drawing.Size(72, 23);
            this.lblPurpose1.TabIndex = 5;
            this.lblPurpose1.Text = "Purpose";
            this.lblPurpose1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDesc
            // 
            this.tbDesc.Enabled = false;
            this.tbDesc.Location = new System.Drawing.Point(118, 52);
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(224, 20);
            this.tbDesc.TabIndex = 4;
            // 
            // lblUserForWU
            // 
            this.lblUserForWU.Location = new System.Drawing.Point(22, 105);
            this.lblUserForWU.Name = "lblUserForWU";
            this.lblUserForWU.Size = new System.Drawing.Size(344, 48);
            this.lblUserForWU.TabIndex = 7;
            this.lblUserForWU.Text = "List of all Users granted selected Working Unit:";
            this.lblUserForWU.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvUser
            // 
            this.lvUser.FullRowSelect = true;
            this.lvUser.GridLines = true;
            this.lvUser.HideSelection = false;
            this.lvUser.Location = new System.Drawing.Point(22, 156);
            this.lvUser.Name = "lvUser";
            this.lvUser.Size = new System.Drawing.Size(344, 258);
            this.lvUser.TabIndex = 8;
            this.lvUser.UseCompatibleStateImageBehavior = false;
            this.lvUser.View = System.Windows.Forms.View.Details;
            this.lvUser.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvUser_ColumnClick);
            // 
            // tabPageUserXWU
            // 
            this.tabPageUserXWU.Controls.Add(this.btnRemoveSelected);
            this.tabPageUserXWU.Controls.Add(this.lvPurposes);
            this.tabPageUserXWU.Controls.Add(this.lvWorkingUnits);
            this.tabPageUserXWU.Controls.Add(this.lblWorkingUnits);
            this.tabPageUserXWU.Controls.Add(this.lblPurposes);
            this.tabPageUserXWU.Controls.Add(this.label2);
            this.tabPageUserXWU.Controls.Add(this.label1);
            this.tabPageUserXWU.Controls.Add(this.lblSelUsers);
            this.tabPageUserXWU.Controls.Add(this.lblUsersForWU);
            this.tabPageUserXWU.Controls.Add(this.btnCancel);
            this.tabPageUserXWU.Controls.Add(this.btnSave);
            this.tabPageUserXWU.Controls.Add(this.btnAdd);
            this.tabPageUserXWU.Controls.Add(this.lvSelectedUsers);
            this.tabPageUserXWU.Controls.Add(this.lvUsers);
            this.tabPageUserXWU.Location = new System.Drawing.Point(4, 22);
            this.tabPageUserXWU.Name = "tabPageUserXWU";
            this.tabPageUserXWU.Size = new System.Drawing.Size(864, 491);
            this.tabPageUserXWU.TabIndex = 2;
            this.tabPageUserXWU.Text = "User <-> Working Unit";
            this.tabPageUserXWU.UseVisualStyleBackColor = true;
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Location = new System.Drawing.Point(344, 454);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(134, 23);
            this.btnRemoveSelected.TabIndex = 12;
            this.btnRemoveSelected.Text = "Remove selected";
            this.btnRemoveSelected.UseVisualStyleBackColor = true;
            this.btnRemoveSelected.Click += new System.EventHandler(this.btnRemoveSelected_Click);
            // 
            // lvPurposes
            // 
            this.lvPurposes.FullRowSelect = true;
            this.lvPurposes.GridLines = true;
            this.lvPurposes.HideSelection = false;
            this.lvPurposes.Location = new System.Drawing.Point(344, 38);
            this.lvPurposes.Name = "lvPurposes";
            this.lvPurposes.Size = new System.Drawing.Size(495, 152);
            this.lvPurposes.TabIndex = 5;
            this.lvPurposes.UseCompatibleStateImageBehavior = false;
            this.lvPurposes.View = System.Windows.Forms.View.Details;
            this.lvPurposes.SelectedIndexChanged += new System.EventHandler(this.lvPurposes_SelectedIndexChanged);
            this.lvPurposes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvPurposes_ColumnClick);
            // 
            // lvWorkingUnits
            // 
            this.lvWorkingUnits.FullRowSelect = true;
            this.lvWorkingUnits.GridLines = true;
            this.lvWorkingUnits.HideSelection = false;
            this.lvWorkingUnits.Location = new System.Drawing.Point(16, 38);
            this.lvWorkingUnits.Name = "lvWorkingUnits";
            this.lvWorkingUnits.Size = new System.Drawing.Size(248, 152);
            this.lvWorkingUnits.TabIndex = 2;
            this.lvWorkingUnits.UseCompatibleStateImageBehavior = false;
            this.lvWorkingUnits.View = System.Windows.Forms.View.Details;
            this.lvWorkingUnits.SelectedIndexChanged += new System.EventHandler(this.lvWorkingUnits_SelectedIndexChanged);
            this.lvWorkingUnits.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvWorkingUnits_ColumnClick);
            // 
            // lblWorkingUnits
            // 
            this.lblWorkingUnits.Location = new System.Drawing.Point(16, 6);
            this.lblWorkingUnits.Name = "lblWorkingUnits";
            this.lblWorkingUnits.Size = new System.Drawing.Size(248, 29);
            this.lblWorkingUnits.TabIndex = 1;
            this.lblWorkingUnits.Text = "List of all working units:";
            this.lblWorkingUnits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPurposes
            // 
            this.lblPurposes.Location = new System.Drawing.Point(341, 6);
            this.lblPurposes.Name = "lblPurposes";
            this.lblPurposes.Size = new System.Drawing.Size(367, 29);
            this.lblPurposes.TabIndex = 4;
            this.lblPurposes.Text = "List of all purposes:";
            this.lblPurposes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(270, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "*";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(845, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "*";
            // 
            // lblSelUsers
            // 
            this.lblSelUsers.Location = new System.Drawing.Point(341, 193);
            this.lblSelUsers.Name = "lblSelUsers";
            this.lblSelUsers.Size = new System.Drawing.Size(498, 48);
            this.lblSelUsers.TabIndex = 28;
            this.lblSelUsers.Text = "List of all Users who are granted selected Working Unit:";
            this.lblSelUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUsersForWU
            // 
            this.lblUsersForWU.Location = new System.Drawing.Point(16, 193);
            this.lblUsersForWU.Name = "lblUsersForWU";
            this.lblUsersForWU.Size = new System.Drawing.Size(248, 48);
            this.lblUsersForWU.TabIndex = 7;
            this.lblUsersForWU.Text = "List of all Users who are not granted selected Working Unit:";
            this.lblUsersForWU.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(764, 454);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 454);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(281, 329);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(48, 23);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = ">";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvSelectedUsers
            // 
            this.lvSelectedUsers.FullRowSelect = true;
            this.lvSelectedUsers.GridLines = true;
            this.lvSelectedUsers.HideSelection = false;
            this.lvSelectedUsers.Location = new System.Drawing.Point(344, 244);
            this.lvSelectedUsers.Name = "lvSelectedUsers";
            this.lvSelectedUsers.Size = new System.Drawing.Size(495, 192);
            this.lvSelectedUsers.TabIndex = 10;
            this.lvSelectedUsers.UseCompatibleStateImageBehavior = false;
            this.lvSelectedUsers.View = System.Windows.Forms.View.Details;
            this.lvSelectedUsers.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvSelectedUsers_ColumnClick);
            // 
            // lvUsers
            // 
            this.lvUsers.FullRowSelect = true;
            this.lvUsers.GridLines = true;
            this.lvUsers.HideSelection = false;
            this.lvUsers.Location = new System.Drawing.Point(16, 244);
            this.lvUsers.Name = "lvUsers";
            this.lvUsers.Size = new System.Drawing.Size(248, 192);
            this.lvUsers.TabIndex = 8;
            this.lvUsers.UseCompatibleStateImageBehavior = false;
            this.lvUsers.View = System.Windows.Forms.View.Details;
            this.lvUsers.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvUsers_ColumnClick);
            // 
            // tabPageUserXOU
            // 
            this.tabPageUserXOU.Controls.Add(this.btnOURemoveSelected);
            this.tabPageUserXOU.Controls.Add(this.lvOUPurposes);
            this.tabPageUserXOU.Controls.Add(this.lvOUnits);
            this.tabPageUserXOU.Controls.Add(this.lblOUnitsList);
            this.tabPageUserXOU.Controls.Add(this.lblOUPurposes);
            this.tabPageUserXOU.Controls.Add(this.label5);
            this.tabPageUserXOU.Controls.Add(this.label6);
            this.tabPageUserXOU.Controls.Add(this.lblOUSelUsers);
            this.tabPageUserXOU.Controls.Add(this.lblUsersNotXOU);
            this.tabPageUserXOU.Controls.Add(this.btnOUCancel);
            this.tabPageUserXOU.Controls.Add(this.btnOUSave);
            this.tabPageUserXOU.Controls.Add(this.btnOUAdd);
            this.tabPageUserXOU.Controls.Add(this.lvOUSelectedUsers);
            this.tabPageUserXOU.Controls.Add(this.lvOUUsers);
            this.tabPageUserXOU.Location = new System.Drawing.Point(4, 22);
            this.tabPageUserXOU.Name = "tabPageUserXOU";
            this.tabPageUserXOU.Size = new System.Drawing.Size(864, 491);
            this.tabPageUserXOU.TabIndex = 3;
            this.tabPageUserXOU.Text = "User <-> Organizational Unit";
            this.tabPageUserXOU.UseVisualStyleBackColor = true;
            // 
            // btnOURemoveSelected
            // 
            this.btnOURemoveSelected.Location = new System.Drawing.Point(344, 454);
            this.btnOURemoveSelected.Name = "btnOURemoveSelected";
            this.btnOURemoveSelected.Size = new System.Drawing.Size(134, 23);
            this.btnOURemoveSelected.TabIndex = 10;
            this.btnOURemoveSelected.Text = "Remove selected";
            this.btnOURemoveSelected.UseVisualStyleBackColor = true;
            this.btnOURemoveSelected.Click += new System.EventHandler(this.btnOURemoveSelected_Click);
            // 
            // lvOUPurposes
            // 
            this.lvOUPurposes.FullRowSelect = true;
            this.lvOUPurposes.GridLines = true;
            this.lvOUPurposes.HideSelection = false;
            this.lvOUPurposes.Location = new System.Drawing.Point(344, 38);
            this.lvOUPurposes.Name = "lvOUPurposes";
            this.lvOUPurposes.Size = new System.Drawing.Size(495, 152);
            this.lvOUPurposes.TabIndex = 3;
            this.lvOUPurposes.UseCompatibleStateImageBehavior = false;
            this.lvOUPurposes.View = System.Windows.Forms.View.Details;
            this.lvOUPurposes.SelectedIndexChanged += new System.EventHandler(this.lvOUPurposes_SelectedIndexChanged);
            this.lvOUPurposes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvOUPurposes_ColumnClick);
            // 
            // lvOUnits
            // 
            this.lvOUnits.FullRowSelect = true;
            this.lvOUnits.GridLines = true;
            this.lvOUnits.HideSelection = false;
            this.lvOUnits.Location = new System.Drawing.Point(16, 38);
            this.lvOUnits.Name = "lvOUnits";
            this.lvOUnits.Size = new System.Drawing.Size(248, 152);
            this.lvOUnits.TabIndex = 1;
            this.lvOUnits.UseCompatibleStateImageBehavior = false;
            this.lvOUnits.View = System.Windows.Forms.View.Details;
            this.lvOUnits.SelectedIndexChanged += new System.EventHandler(this.lvOUnits_SelectedIndexChanged);
            this.lvOUnits.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvOUnits_ColumnClick);
            // 
            // lblOUnitsList
            // 
            this.lblOUnitsList.Location = new System.Drawing.Point(16, 6);
            this.lblOUnitsList.Name = "lblOUnitsList";
            this.lblOUnitsList.Size = new System.Drawing.Size(248, 29);
            this.lblOUnitsList.TabIndex = 0;
            this.lblOUnitsList.Text = "List of all organizational units:";
            this.lblOUnitsList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOUPurposes
            // 
            this.lblOUPurposes.Location = new System.Drawing.Point(341, 6);
            this.lblOUPurposes.Name = "lblOUPurposes";
            this.lblOUPurposes.Size = new System.Drawing.Size(367, 29);
            this.lblOUPurposes.TabIndex = 2;
            this.lblOUPurposes.Text = "List of all purposes:";
            this.lblOUPurposes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(270, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 16);
            this.label5.TabIndex = 31;
            this.label5.Text = "*";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(839, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 16);
            this.label6.TabIndex = 34;
            this.label6.Text = "*";
            // 
            // lblOUSelUsers
            // 
            this.lblOUSelUsers.Location = new System.Drawing.Point(341, 193);
            this.lblOUSelUsers.Name = "lblOUSelUsers";
            this.lblOUSelUsers.Size = new System.Drawing.Size(498, 48);
            this.lblOUSelUsers.TabIndex = 7;
            this.lblOUSelUsers.Text = "List of all Users who are granted selected Organizational Unit:";
            this.lblOUSelUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUsersNotXOU
            // 
            this.lblUsersNotXOU.Location = new System.Drawing.Point(16, 193);
            this.lblUsersNotXOU.Name = "lblUsersNotXOU";
            this.lblUsersNotXOU.Size = new System.Drawing.Size(248, 48);
            this.lblUsersNotXOU.TabIndex = 4;
            this.lblUsersNotXOU.Text = "List of all Users who are not granted selected Organizational Unit:";
            this.lblUsersNotXOU.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOUCancel
            // 
            this.btnOUCancel.Location = new System.Drawing.Point(764, 454);
            this.btnOUCancel.Name = "btnOUCancel";
            this.btnOUCancel.Size = new System.Drawing.Size(75, 23);
            this.btnOUCancel.TabIndex = 11;
            this.btnOUCancel.Text = "Cancel";
            this.btnOUCancel.Click += new System.EventHandler(this.btnOUCancel_Click);
            // 
            // btnOUSave
            // 
            this.btnOUSave.Location = new System.Drawing.Point(16, 454);
            this.btnOUSave.Name = "btnOUSave";
            this.btnOUSave.Size = new System.Drawing.Size(75, 23);
            this.btnOUSave.TabIndex = 9;
            this.btnOUSave.Text = "Save";
            this.btnOUSave.Click += new System.EventHandler(this.btnOUSave_Click);
            // 
            // btnOUAdd
            // 
            this.btnOUAdd.Location = new System.Drawing.Point(281, 329);
            this.btnOUAdd.Name = "btnOUAdd";
            this.btnOUAdd.Size = new System.Drawing.Size(48, 23);
            this.btnOUAdd.TabIndex = 6;
            this.btnOUAdd.Text = ">";
            this.btnOUAdd.Click += new System.EventHandler(this.btnOUAdd_Click);
            // 
            // lvOUSelectedUsers
            // 
            this.lvOUSelectedUsers.FullRowSelect = true;
            this.lvOUSelectedUsers.GridLines = true;
            this.lvOUSelectedUsers.HideSelection = false;
            this.lvOUSelectedUsers.Location = new System.Drawing.Point(344, 244);
            this.lvOUSelectedUsers.Name = "lvOUSelectedUsers";
            this.lvOUSelectedUsers.Size = new System.Drawing.Size(495, 192);
            this.lvOUSelectedUsers.TabIndex = 8;
            this.lvOUSelectedUsers.UseCompatibleStateImageBehavior = false;
            this.lvOUSelectedUsers.View = System.Windows.Forms.View.Details;
            this.lvOUSelectedUsers.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvOUSelectedUsers_ColumnClick);
            // 
            // lvOUUsers
            // 
            this.lvOUUsers.FullRowSelect = true;
            this.lvOUUsers.GridLines = true;
            this.lvOUUsers.HideSelection = false;
            this.lvOUUsers.Location = new System.Drawing.Point(16, 244);
            this.lvOUUsers.Name = "lvOUUsers";
            this.lvOUUsers.Size = new System.Drawing.Size(248, 192);
            this.lvOUUsers.TabIndex = 5;
            this.lvOUUsers.UseCompatibleStateImageBehavior = false;
            this.lvOUUsers.View = System.Windows.Forms.View.Details;
            this.lvOUUsers.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvOUUsers_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(805, 538);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chbRetiredUnitsWU
            // 
            this.chbRetiredUnitsWU.AutoSize = true;
            this.chbRetiredUnitsWU.Location = new System.Drawing.Point(25, 420);
            this.chbRetiredUnitsWU.Name = "chbRetiredUnitsWU";
            this.chbRetiredUnitsWU.Size = new System.Drawing.Size(113, 17);
            this.chbRetiredUnitsWU.TabIndex = 9;
            this.chbRetiredUnitsWU.Text = "Show retired users";
            this.chbRetiredUnitsWU.UseVisualStyleBackColor = true;
            this.chbRetiredUnitsWU.CheckedChanged += new System.EventHandler(this.chbRetiredUnitsWU_CheckedChanged);
            // 
            // chbRetiredUnitsOU
            // 
            this.chbRetiredUnitsOU.AutoSize = true;
            this.chbRetiredUnitsOU.Location = new System.Drawing.Point(22, 420);
            this.chbRetiredUnitsOU.Name = "chbRetiredUnitsOU";
            this.chbRetiredUnitsOU.Size = new System.Drawing.Size(113, 17);
            this.chbRetiredUnitsOU.TabIndex = 9;
            this.chbRetiredUnitsOU.Text = "Show retired users";
            this.chbRetiredUnitsOU.UseVisualStyleBackColor = true;
            this.chbRetiredUnitsOU.CheckedChanged += new System.EventHandler(this.chbRetiredUnitsOU_CheckedChanged);
            // 
            // ApplUserXWU
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 600);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "ApplUserXWU";
            this.ShowInTaskbar = false;
            this.Text = "User <-> Units";
            this.Load += new System.EventHandler(this.ApplUserXWU_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ApplUserXWU_KeyUp);
            this.tabControl1.ResumeLayout(false);
            this.tabPageUsers.ResumeLayout(false);
            this.gbOUnits.ResumeLayout(false);
            this.gbOUnits.PerformLayout();
            this.gbWUnits.ResumeLayout(false);
            this.gbWUnits.PerformLayout();
            this.tabPageUnits.ResumeLayout(false);
            this.gbOrganizationalUnits.ResumeLayout(false);
            this.gbOrganizationalUnits.PerformLayout();
            this.gbWorkingUnits.ResumeLayout(false);
            this.gbWorkingUnits.PerformLayout();
            this.tabPageUserXWU.ResumeLayout(false);
            this.tabPageUserXOU.ResumeLayout(false);
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
					case ApplUserXWU.UserIDIndex:
					case ApplUserXWU.NameIndex:
					case ApplUserXWU.DescriptionIndex:
                    case ApplUserXWU.WUIndex:
                    case ApplUserXWU.PurposeIndex:
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
				this.Text = rm.GetString("formUserXWU", culture);

				// TabPage text
				tabPageUsers.Text = rm.GetString("tabPageUsers", culture);
				tabPageUnits.Text = rm.GetString("tabPageWU", culture);
				tabPageUserXWU.Text = rm.GetString("tabPageUserXWU", culture);
                tabPageUserXOU.Text = rm.GetString("tabPageUserXOU", culture);
				
				// button's text
				btnClose.Text = rm.GetString("btnClose", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
				btnSave.Text = rm.GetString("btnSave", culture);
                btnRemoveSelected.Text = rm.GetString("btnRemoveSel", culture);
                btnOUCancel.Text = rm.GetString("btnCancel", culture);
                btnOUSave.Text = rm.GetString("btnSave", culture);
                btnOURemoveSelected.Text = rm.GetString("btnRemoveSel", culture);

				// label's text
				lblUser.Text = rm.GetString("lblUserID", culture);
                lblOUUserID.Text = rm.GetString("lblUserID", culture);
				lblName.Text = rm.GetString("lblName", culture);
                lblOUName.Text = rm.GetString("lblName", culture);
				lblWUName.Text = rm.GetString("lblName", culture);
                lblOU.Text = rm.GetString("lblName", culture);
				lblDesc.Text = rm.GetString("lblDescription", culture);
                lblOUDesc.Text = rm.GetString("lblDescription", culture);
                lblWorkingUnits.Text = rm.GetString("lblWorkingUnits", culture);
                lblOUnitsList.Text = rm.GetString("lblOUnits", culture);
				lblWUForUsers.Text = rm.GetString("lblWUForUsers", culture);
                lblOUList.Text = rm.GetString("lblOUForUsers", culture);
				lblUserForWU.Text = rm.GetString("lblUserForWU", culture);
                lblUserForOU.Text = rm.GetString("lblUserForOU", culture);
				lblUsersForWU.Text = rm.GetString("lblUsersForWU", culture);
                lblUsersForWU.Text = rm.GetString("lblUsersForOU", culture);
				lblSelUsers.Text = rm.GetString("lblSelUsers", culture);
                lblOUSelUsers.Text = rm.GetString("lblOUSelUsers", culture);
				lblPurpose.Text = rm.GetString("lblPurpose", culture);
                lblOUPurpose.Text = rm.GetString("lblPurpose", culture);
				lblPurpose1.Text = rm.GetString("lblPurpose", culture);
                lblOUPurpose1.Text = rm.GetString("lblPurpose", culture);
				lblPurposes.Text = rm.GetString("lblPurposes", culture);
                lblOUPurposes.Text = rm.GetString("lblPurposes", culture);

                // group boxes
                gbWUnits.Text = rm.GetString("groupBoxWU", culture);
                gbOUnits.Text = rm.GetString("gbOU", culture);
                gbWorkingUnits.Text = rm.GetString("groupBoxWU", culture);
                gbOrganizationalUnits.Text = rm.GetString("gbOU", culture);

                // check box text
                chbRetiredUsersWU.Text = rm.GetString("showRetired", culture);
                chbRetiredUsersOU.Text = rm.GetString("showRetired", culture);
                chbRetiredUnitsWU.Text = rm.GetString("showRetired", culture);
                chbRetiredUnitsOU.Text = rm.GetString("showRetired", culture);
				
				// list view
				lvWU.BeginUpdate();
				lvWU.Columns.Add(rm.GetString("lblName", culture), lvWU.Width / 2 - 10, HorizontalAlignment.Left);
				lvWU.Columns.Add(rm.GetString("lblDescription", culture), lvWU.Width / 2 - 10, HorizontalAlignment.Left);
				lvWU.EndUpdate();

                lvOU.BeginUpdate();
                lvOU.Columns.Add(rm.GetString("lblName", culture), lvOU.Width / 2 - 10, HorizontalAlignment.Left);
                lvOU.Columns.Add(rm.GetString("lblDescription", culture), lvOU.Width / 2 - 10, HorizontalAlignment.Left);
                lvOU.EndUpdate();

				lvUser.BeginUpdate();
				lvUser.Columns.Add(rm.GetString("lblUserID", culture), lvUser.Width / 3 - 7, HorizontalAlignment.Left);
				lvUser.Columns.Add(rm.GetString("lblName", culture), lvUser.Width / 3 - 7, HorizontalAlignment.Left);
				lvUser.Columns.Add(rm.GetString("lblDescription", culture), lvUser.Width / 3 - 7, HorizontalAlignment.Left);
				lvUser.EndUpdate();

                lvOUUser.BeginUpdate();
                lvOUUser.Columns.Add(rm.GetString("lblUserID", culture), lvOUUser.Width / 3 - 7, HorizontalAlignment.Left);
                lvOUUser.Columns.Add(rm.GetString("lblName", culture), lvOUUser.Width / 3 - 7, HorizontalAlignment.Left);
                lvOUUser.Columns.Add(rm.GetString("lblDescription", culture), lvOUUser.Width / 3 - 7, HorizontalAlignment.Left);
                lvOUUser.EndUpdate();

				lvUsers.BeginUpdate();
				lvUsers.Columns.Add(rm.GetString("lblUserID", culture), lvUsers.Width / 3 - 7, HorizontalAlignment.Left);
				lvUsers.Columns.Add(rm.GetString("lblName", culture), lvUsers.Width / 3 - 7, HorizontalAlignment.Left);
				lvUsers.Columns.Add(rm.GetString("lblDescription", culture), lvUsers.Width / 3 - 7, HorizontalAlignment.Left);
				lvUsers.EndUpdate();

				lvSelectedUsers.BeginUpdate();
				lvSelectedUsers.Columns.Add(rm.GetString("lblUserID", culture), lvSelectedUsers.Width / 5 - 4, HorizontalAlignment.Left);
				lvSelectedUsers.Columns.Add(rm.GetString("lblName", culture), lvSelectedUsers.Width / 5 - 4, HorizontalAlignment.Left);
				lvSelectedUsers.Columns.Add(rm.GetString("lblDescription", culture), lvSelectedUsers.Width / 5 - 4, HorizontalAlignment.Left);
                lvSelectedUsers.Columns.Add(rm.GetString("hdrWorkingUnit", culture), lvSelectedUsers.Width / 5 - 4, HorizontalAlignment.Left);
                lvSelectedUsers.Columns.Add(rm.GetString("hdrPurpose", culture), lvSelectedUsers.Width / 5 - 4, HorizontalAlignment.Left);
				lvSelectedUsers.EndUpdate();

                lvWorkingUnits.BeginUpdate();
                lvWorkingUnits.Columns.Add(rm.GetString("lblName", culture), lvWorkingUnits.Width / 2 - 10, HorizontalAlignment.Left);
                lvWorkingUnits.Columns.Add(rm.GetString("lblDescription", culture), lvWorkingUnits.Width / 2 - 10, HorizontalAlignment.Left);
                lvWorkingUnits.EndUpdate();

                lvPurposes.BeginUpdate();
                lvPurposes.Columns.Add(rm.GetString("lblName", culture), lvPurposes.Width / 2 - 10, HorizontalAlignment.Left);
                lvPurposes.Columns.Add(rm.GetString("lblDescription", culture), lvPurposes.Width / 2 - 10, HorizontalAlignment.Left);
                lvPurposes.EndUpdate();

                lvOUUsers.BeginUpdate();
                lvOUUsers.Columns.Add(rm.GetString("lblUserID", culture), lvOUUsers.Width / 3 - 7, HorizontalAlignment.Left);
                lvOUUsers.Columns.Add(rm.GetString("lblName", culture), lvOUUsers.Width / 3 - 7, HorizontalAlignment.Left);
                lvOUUsers.Columns.Add(rm.GetString("lblDescription", culture), lvOUUsers.Width / 3 - 7, HorizontalAlignment.Left);
                lvOUUsers.EndUpdate();

                lvOUSelectedUsers.BeginUpdate();
                lvOUSelectedUsers.Columns.Add(rm.GetString("lblUserID", culture), lvOUSelectedUsers.Width / 5 - 4, HorizontalAlignment.Left);
                lvOUSelectedUsers.Columns.Add(rm.GetString("lblName", culture), lvOUSelectedUsers.Width / 5 - 4, HorizontalAlignment.Left);
                lvOUSelectedUsers.Columns.Add(rm.GetString("lblDescription", culture), lvOUSelectedUsers.Width / 5 - 4, HorizontalAlignment.Left);
                lvOUSelectedUsers.Columns.Add(rm.GetString("hdrOrgUnit", culture), lvOUSelectedUsers.Width / 5 - 4, HorizontalAlignment.Left);
                lvOUSelectedUsers.Columns.Add(rm.GetString("hdrPurpose", culture), lvOUSelectedUsers.Width / 5 - 4, HorizontalAlignment.Left);
                lvOUSelectedUsers.EndUpdate();

                lvOUnits.BeginUpdate();
                lvOUnits.Columns.Add(rm.GetString("lblName", culture), lvOUnits.Width / 2 - 10, HorizontalAlignment.Left);
                lvOUnits.Columns.Add(rm.GetString("lblDescription", culture), lvOUnits.Width / 2 - 10, HorizontalAlignment.Left);
                lvOUnits.EndUpdate();

                lvOUPurposes.BeginUpdate();
                lvOUPurposes.Columns.Add(rm.GetString("lblName", culture), lvOUPurposes.Width / 2 - 10, HorizontalAlignment.Left);
                lvOUPurposes.Columns.Add(rm.GetString("lblDescription", culture), lvOUPurposes.Width / 2 - 10, HorizontalAlignment.Left);
                lvOUPurposes.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUserXWU.setLanguage(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void populateUserCombo()
		{
			try
			{
                List<string> statuses = new List<string>();

                if (!chbRetiredUsersWU.Checked)
                {
                    statuses.Add(Constants.statusActive);
                    statuses.Add(Constants.statusDisabled);
                }                    

                List<ApplUserTO> userArray = new ApplUser().SearchWithStatus(statuses);
				userArray.Insert(0, new ApplUserTO(rm.GetString("all", culture), "", "", "", -1, "", -1, ""));

				this.cbUser.DataSource = userArray;
				this.cbUser.DisplayMember = "UserID";
				this.cbUser.ValueMember = "UserID";                
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUserXWU.populateUserCombo(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        private void populateOUUserCombo()
        {
            try
            {
                List<string> statuses = new List<string>();

                if (!chbRetiredUsersOU.Checked)
                {
                    statuses.Add(Constants.statusActive);
                    statuses.Add(Constants.statusDisabled);
                }  
                
                List<ApplUserTO> userArray = new ApplUser().SearchWithStatus(statuses);
                userArray.Insert(0, new ApplUserTO(rm.GetString("all", culture), "", "", "", -1, "", -1, ""));

                this.cbOUUser.DataSource = userArray;
                this.cbOUUser.DisplayMember = "UserID";
                this.cbOUUser.ValueMember = "UserID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.populateOUUserCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
       
		private void populateWorkingUnitCombo(ComboBox cbName)
		{
			try
			{
				WorkingUnit wu = new WorkingUnit();
                wu.WUTO.Status = Constants.DefaultStateActive;
				wuArray = wu.Search();
				wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

				cbName.DataSource = wuArray;
				cbName.DisplayMember = "Name";
				cbName.ValueMember = "WorkingUnitID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUserXWU.populateWorkingUnitCombo(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        private void populateOUCombo(ComboBox cbName)
        {
            try
            {
                OrganizationalUnit ou = new OrganizationalUnit();
                ou.OrgUnitTO.Status = Constants.DefaultStateActive;
                ouArray = ou.Search();
                ouArray.Insert(0, new OrganizationalUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), ""));

                cbName.DataSource = ouArray;
                cbName.DisplayMember = "Name";
                cbName.ValueMember = "OrgUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.populateOUCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateWorkingUnitLV()
        {
            try
            {
                WorkingUnit wUnit = new WorkingUnit();
                wUnit.WUTO.Status = Constants.DefaultStateActive;
                List<WorkingUnitTO> wuArray = wUnit.Search();

                lvWorkingUnits.BeginUpdate();
                lvWorkingUnits.Items.Clear();
                foreach (WorkingUnitTO wu in wuArray)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = wu.Name.Trim();
                    item.SubItems.Add(wu.Description.ToString().Trim());
                    item.Tag = wu;

                    lvWorkingUnits.Items.Add(item);
                }
                lvWorkingUnits.EndUpdate();
                lvWorkingUnits.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.populateWorkingUnitLV(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateOULV()
        {
            try
            {
                OrganizationalUnit oUnit = new OrganizationalUnit();
                oUnit.OrgUnitTO.Status = Constants.DefaultStateActive;
                List<OrganizationalUnitTO> ouArray = oUnit.Search();

                lvOUnits.BeginUpdate();
                lvOUnits.Items.Clear();
                foreach (OrganizationalUnitTO ou in ouArray)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = ou.Name.Trim();
                    item.SubItems.Add(ou.Desc.ToString().Trim());
                    item.Tag = ou;

                    lvOUnits.Items.Add(item);
                }
                lvOUnits.EndUpdate();
                lvOUnits.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.populateOULV(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public void populatePurposeCombo(ComboBox cbName)
		{
            try
            {
                ArrayList comboValues = new ArrayList();
                comboValues.Add(Constants.ReportPurpose);
                comboValues.Add(Constants.PassPurpose);
                comboValues.Add(Constants.IOPairPurpose);
                comboValues.Add(Constants.LocationPurpose);
                comboValues.Add(Constants.PermissionPurpose);
                comboValues.Add(Constants.AbsencesPurpose);
                comboValues.Add(Constants.EmployeesPurpose);
                comboValues.Add(Constants.ExtraHoursPurpose);
                comboValues.Add(Constants.PermVerificationPurpose);
                comboValues.Add(Constants.RestaurantPurpose);
                comboValues.Add(Constants.VacationPurpose);

                cbName.DataSource = comboValues;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.populatePurposeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
		}

        public void populatePurposeLV(ListView lv)
        {
            try
            {
                Hashtable purposes = new Hashtable();
                purposes.Add(Constants.ReportPurpose, rm.GetString("purposeReport", culture));
                purposes.Add(Constants.PassPurpose, rm.GetString("purposePass", culture));
                purposes.Add(Constants.IOPairPurpose, rm.GetString("purposeIOPair", culture));
                purposes.Add(Constants.LocationPurpose, rm.GetString("purposeLoaction", culture));
                purposes.Add(Constants.PermissionPurpose, rm.GetString("purposePermission", culture));
                purposes.Add(Constants.AbsencesPurpose, rm.GetString("purposeAbsences", culture));
                purposes.Add(Constants.EmployeesPurpose, rm.GetString("purposeEmployees", culture));
                purposes.Add(Constants.ExtraHoursPurpose, rm.GetString("purposeExtraHours", culture));
                purposes.Add(Constants.PermVerificationPurpose, rm.GetString("purposePermVerification", culture));
                purposes.Add(Constants.RestaurantPurpose, rm.GetString("purposeRestaurant", culture));
                purposes.Add(Constants.VacationPurpose,rm.GetString("purposeVacation",culture));

                lv.BeginUpdate();
                lv.Items.Clear();
                foreach (string purpose in purposes.Keys)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = purpose.Trim();
                    item.SubItems.Add(purposes[purpose].ToString().Trim());

                    lv.Items.Add(item);
                }
                lv.EndUpdate();
                lv.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.populatePurposeLV(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		private void clearListView(ListView lvName)
		{
			lvName.BeginUpdate();
			lvName.Items.Clear();
			lvName.EndUpdate();
			lvName.Invalidate();

            if (lvName.Equals(lvSelectedUsers))
            {
                originalSelectedUserListWU.Clear();
                addedUsersWU.Clear();
                removedUsersWU.Clear();
            }

            if (lvName.Equals(lvOUSelectedUsers))
            {
                originalSelectedUserListOU.Clear();
                addedUsersOU.Clear();
                removedUsersOU.Clear();
            }
		}

		private void populateWUListView(List<WorkingUnitTO> wuList)
		{
			try
			{
				lvWU.BeginUpdate();
				lvWU.Items.Clear();
				
				if (wuList.Count > 0)
				{
					foreach(WorkingUnitTO wunit in wuList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = wunit.Name.Trim();
						item.SubItems.Add(wunit.Description.ToString().Trim());
                        item.Tag = wunit;					
						lvWU.Items.Add(item);
					}
				}

				lvWU.EndUpdate();
				lvWU.Invalidate();
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUserXWU.populateWUListView(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        private void populateOUListView(List<OrganizationalUnitTO> ouList)
        {
            try
            {
                lvOU.BeginUpdate();
                lvOU.Items.Clear();

                if (ouList.Count > 0)
                {
                    foreach (OrganizationalUnitTO ounit in ouList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = ounit.Name.Trim();
                        item.SubItems.Add(ounit.Desc.ToString().Trim());
                        item.Tag = ounit;
                        lvOU.Items.Add(item);
                    }
                }

                lvOU.EndUpdate();
                lvOU.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.populateOUListView(): " + ex.Message + "\n");
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
				log.writeLog(DateTime.Now + " ApplUserXWU.populateUserListView(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        private void populateSelUserListView(List<ApplUserTO> userList, WorkingUnitTO wu, string purpose)
        {
            try
            {
                lvSelectedUsers.BeginUpdate();

                if (userList.Count > 0)
                {
                    foreach (ApplUserTO user in userList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = user.UserID.Trim();
                        item.SubItems.Add(user.Name.Trim());
                        item.SubItems.Add(user.Description.Trim());
                        item.SubItems.Add(wu.Name);
                        item.SubItems.Add(purpose);
                        Item itemTag = new Item();
                        itemTag.UserID = user.UserID;
                        itemTag.WU = new WorkingUnit();
                        itemTag.WU.WUTO = wu;
                        itemTag.Purpose = purpose;
                        item.Tag = itemTag;

                        lvSelectedUsers.Items.Add(item);
                    }
                }

                lvSelectedUsers.EndUpdate();
                lvSelectedUsers.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.populateSelUserListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateOUSelUserListView(List<ApplUserTO> userList, OrganizationalUnitTO ou, string purpose)
        {
            try
            {
                lvOUSelectedUsers.BeginUpdate();

                if (userList.Count > 0)
                {
                    foreach (ApplUserTO user in userList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = user.UserID.Trim();
                        item.SubItems.Add(user.Name.Trim());
                        item.SubItems.Add(user.Description.Trim());
                        item.SubItems.Add(ou.Name);
                        item.SubItems.Add(purpose);
                        Item itemTag = new Item();
                        itemTag.UserID = user.UserID;
                        itemTag.OU = new OrganizationalUnit();
                        itemTag.OU.OrgUnitTO = ou;
                        itemTag.Purpose = purpose;
                        item.Tag = itemTag;

                        lvOUSelectedUsers.Items.Add(item);
                    }
                }

                lvOUSelectedUsers.EndUpdate();
                lvOUSelectedUsers.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.populateOUSelUserListView(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " ApplUserXWU.btnClose_Click(): " + ex.Message + "\n");
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

                if (removedUsersWU.Count > 0 || addedUsersWU.Count > 0)
                {
                    MessageBox.Show(rm.GetString("userXWUNotSaved", culture));
                }

                showWUMessage = false;
                this.lvWorkingUnits.SelectedItems.Clear();
                this.lvPurposes.SelectedItems.Clear();
                this.lvUsers.SelectedItems.Clear();
                clearListView(lvSelectedUsers);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void btnOUCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (removedUsersOU.Count > 0 || addedUsersOU.Count > 0)
                {
                    MessageBox.Show(rm.GetString("userXWUNotSaved", culture));
                }

                showOUMessage = false;
                this.lvOUnits.SelectedItems.Clear();
                this.lvOUPurposes.SelectedItems.Clear();
                this.lvOUUsers.SelectedItems.Clear();
                clearListView(lvOUSelectedUsers);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.btnOUCancel_Click(): " + ex.Message + "\n");
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

						populateWUListView(new ApplUsersXWU().FindWUForUser(applUserTO.UserID.Trim(), cbPurpose.SelectedItem.ToString().Trim()));
					}
				}
				else
				{
					this.tbName.Text = "";
					clearListView(lvWU);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUserXWU.cbUser_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void cbOUUser_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (this.cbOUUser.SelectedIndex > 0)
                {
                    ApplUserTO applUserTO = new ApplUser().Find(cbOUUser.SelectedValue.ToString().Trim());
                    if (!applUserTO.UserID.Trim().Equals(""))
                    {
                        this.tbOUName.Text = applUserTO.Name.Trim();

                        Dictionary<int, OrganizationalUnitTO> userXouDict = new ApplUserXOrgUnit().FindOUForUserDictionary(applUserTO.UserID, cbOUPurpose.SelectedItem.ToString().Trim());
                        List<OrganizationalUnitTO> userOUnits = new List<OrganizationalUnitTO>();
                        foreach (int id in userXouDict.Keys)
                        {
                            userOUnits.Add(userXouDict[id]);
                        }
                        populateOUListView(userOUnits);
                    }
                }
                else
                {
                    this.tbOUName.Text = "";
                    clearListView(lvOU);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.cbOUUser_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

		private void cbWUName_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;

				if (this.cbWUName.SelectedIndex > 0)
				{
					WorkingUnit wu = new WorkingUnit();
					wu.Find((int) cbWUName.SelectedValue);
					if (wu.WUTO.WorkingUnitID != -1)
					{
						this.tbDesc.Text = wu.WUTO.Description.Trim();

                        List<string> statuses = new List<string>();
                        if (!chbRetiredUnitsWU.Checked)
                        {
                            statuses.Add(Constants.statusActive);
                            statuses.Add(Constants.statusDisabled);
                        }

						populateUserListView(new ApplUsersXWU().FindUsersForWU(wu.WUTO.WorkingUnitID, cbPurpose1.SelectedItem.ToString().Trim(), statuses), this.lvUser);
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
				log.writeLog(DateTime.Now + " ApplUserXWU.cbWUName_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void cbOUName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (this.cbOUName.SelectedIndex > 0)
                {
                    OrganizationalUnit ou = new OrganizationalUnit();
                    ou.OrgUnitTO = ou.Find((int)cbOUName.SelectedValue);
                    if (ou.OrgUnitTO.OrgUnitID != -1)
                    {
                        this.tbOUDesc.Text = ou.OrgUnitTO.Desc.Trim();

                        List<string> statuses = new List<string>();
                        if (!chbRetiredUnitsOU.Checked)
                        {
                            statuses.Add(Constants.statusActive);
                            statuses.Add(Constants.statusDisabled);
                        }

                        populateUserListView(new ApplUserXOrgUnit().FindUsersForOU(ou.OrgUnitTO.OrgUnitID, cbOUPurpose1.SelectedItem.ToString().Trim(), statuses), this.lvOUUser);
                    }
                }
                else
                {
                    this.tbOUDesc.Text = "";
                    clearListView(lvOUUser);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.cbWUName_SelectedIndexChanged(): " + ex.Message + "\n");
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

                if (lvWorkingUnits.SelectedItems.Count > 0 && lvPurposes.SelectedItems.Count > 0 && lvUsers.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem itemWU in lvWorkingUnits.SelectedItems)
                    {
                        WorkingUnitTO wu = (WorkingUnitTO)itemWU.Tag;
                        foreach (ListViewItem itemP in lvPurposes.SelectedItems)
                        {
                            string purpose = itemP.Text.Trim();
                            foreach (ListViewItem item in lvUsers.SelectedItems)
                            {
                                ListViewItem itemSel = new ListViewItem();
                                itemSel.Text = item.Text.Trim();
                                itemSel.SubItems.Add(item.SubItems[1].Text.Trim());
                                itemSel.SubItems.Add(item.SubItems[2].Text.Trim());
                                itemSel.SubItems.Add(wu.Name);
                                itemSel.SubItems.Add(purpose);
                                Item itemTag = new Item();
                                itemTag.UserID = item.Text;
                                itemTag.WU = new WorkingUnit();
                                itemTag.WU.WUTO = wu;
                                itemTag.Purpose = purpose;
                                itemSel.Tag = itemTag;

                                if (contain(originalSelectedUserListWU, itemSel) < 0 && contain(addedUsersWU, itemSel) < 0)
                                {
                                    lvSelectedUsers.Items.Add(itemSel);

                                    if (contain(originalSelectedUserListWU, itemSel) < 0)
                                        addedUsersWU.Add(itemSel);
                                    int index = contain(removedUsersWU, itemSel);
                                    if (index >= 0)
                                        removedUsersWU.RemoveAt(index);
                                }
                            }
                        }
                    }
                }
                else if (lvWorkingUnits.SelectedItems.Count == 0)
                {
                    MessageBox.Show(rm.GetString("SelectWU", culture));
                }
                else if (lvPurposes.SelectedItems.Count == 0)
                {
                    MessageBox.Show(rm.GetString("SelectPurpose", culture));
                }
                else if (lvUsers.SelectedItems.Count == 0)
                {
                    MessageBox.Show(rm.GetString("SelectUser", culture));
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUserXWU.btnAdd_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void btnOUAdd_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvOUnits.SelectedItems.Count > 0 && lvOUPurposes.SelectedItems.Count > 0 && lvOUUsers.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem itemOU in lvOUnits.SelectedItems)
                    {
                        OrganizationalUnitTO ou = (OrganizationalUnitTO)itemOU.Tag;
                        foreach (ListViewItem itemP in lvOUPurposes.SelectedItems)
                        {
                            string purpose = itemP.Text.Trim();
                            foreach (ListViewItem item in lvOUUsers.SelectedItems)
                            {
                                ListViewItem itemSel = new ListViewItem();
                                itemSel.Text = item.Text.Trim();
                                itemSel.SubItems.Add(item.SubItems[1].Text.Trim());
                                itemSel.SubItems.Add(item.SubItems[2].Text.Trim());
                                itemSel.SubItems.Add(ou.Name);
                                itemSel.SubItems.Add(purpose);
                                Item itemTag = new Item();
                                itemTag.UserID = item.Text;
                                itemTag.OU = new OrganizationalUnit();
                                itemTag.OU.OrgUnitTO = ou;
                                itemTag.Purpose = purpose;
                                itemSel.Tag = itemTag;

                                if (contain(originalSelectedUserListOU, itemSel) < 0 && contain(addedUsersOU, itemSel) < 0)
                                {
                                    lvOUSelectedUsers.Items.Add(itemSel);

                                    if (contain(originalSelectedUserListOU, itemSel) < 0)
                                        addedUsersOU.Add(itemSel);
                                    int index = contain(removedUsersOU, itemSel);
                                    if (index >= 0)
                                        removedUsersOU.RemoveAt(index);
                                }
                            }
                        }
                    }
                }
                else if (lvOUnits.SelectedItems.Count == 0)
                {
                    MessageBox.Show(rm.GetString("SelectOU", culture));
                }
                else if (lvOUPurposes.SelectedItems.Count == 0)
                {
                    MessageBox.Show(rm.GetString("SelectPurpose", culture));
                }
                else if (lvOUUsers.SelectedItems.Count == 0)
                {
                    MessageBox.Show(rm.GetString("SelectUser", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.btnOUAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

		private void ApplUserXWU_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer objects
                _comp1 = new ListViewItemComparer(lvWU);
                lvWU.ListViewItemSorter = _comp1;
                lvWU.Sorting = SortOrder.Ascending;

                _comp7 = new ListViewItemComparer(lvOU);
                lvOU.ListViewItemSorter = _comp7;
                lvOU.Sorting = SortOrder.Ascending;

                _comp2 = new ListViewItemComparer(lvUser);
                lvUser.ListViewItemSorter = _comp2;
                lvUser.Sorting = SortOrder.Ascending;

                _comp8 = new ListViewItemComparer(lvOUUser);
                lvOUUser.ListViewItemSorter = _comp8;
                lvOUUser.Sorting = SortOrder.Ascending;

                _comp3 = new ListViewItemComparer(lvUsers);
                lvUsers.ListViewItemSorter = _comp3;
                lvUsers.Sorting = SortOrder.Ascending;

                _comp4 = new ListViewItemComparer(lvSelectedUsers);
                lvSelectedUsers.ListViewItemSorter = _comp4;
                lvSelectedUsers.Sorting = SortOrder.Ascending;

                _comp5 = new ListViewItemComparer(lvWorkingUnits);
                lvWorkingUnits.ListViewItemSorter = _comp5;
                lvWorkingUnits.Sorting = SortOrder.Ascending;

                _comp6 = new ListViewItemComparer(lvPurposes);
                lvPurposes.ListViewItemSorter = _comp6;
                lvPurposes.Sorting = SortOrder.Ascending;

                _comp9 = new ListViewItemComparer(lvOUUsers);
                lvOUUsers.ListViewItemSorter = _comp9;
                lvOUUsers.Sorting = SortOrder.Ascending;

                _comp10 = new ListViewItemComparer(lvOUSelectedUsers);
                lvOUSelectedUsers.ListViewItemSorter = _comp10;
                lvOUSelectedUsers.Sorting = SortOrder.Ascending;

                _comp11 = new ListViewItemComparer(lvOUnits);
                lvOUnits.ListViewItemSorter = _comp11;
                lvOUnits.Sorting = SortOrder.Ascending;

                _comp12 = new ListViewItemComparer(lvOUPurposes);
                lvOUPurposes.ListViewItemSorter = _comp12;
                lvOUPurposes.Sorting = SortOrder.Ascending;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.ApplUserXWU_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvWU_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				SortOrder prevOrder = lvWU.Sorting;

				if (e.Column == _comp1.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvWU.Sorting = SortOrder.Descending;
					}
					else
					{
						lvWU.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp1.SortColumn = e.Column;
					lvWU.Sorting = SortOrder.Ascending;
				}

                lvWU.Sort();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUserXWU.lvWU_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void lvOU_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvOU.Sorting;

                if (e.Column == _comp7.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvOU.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvOU.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp7.SortColumn = e.Column;
                    lvOU.Sorting = SortOrder.Ascending;
                }

                lvOU.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.lvOU_ColumnClick(): " + ex.Message + "\n");
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

                lvUser.Sort();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUserXWU.lvUser_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void lvOUUser_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvOUUser.Sorting;

                if (e.Column == _comp8.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvOUUser.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvOUUser.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp8.SortColumn = e.Column;
                    lvOUUser.Sorting = SortOrder.Ascending;
                }

                lvOUUser.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.lvOUUser_ColumnClick(): " + ex.Message + "\n");
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

                lvUsers.Sort();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUserXWU.lvUsers_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void lvOUUsers_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvOUUsers.Sorting;

                if (e.Column == _comp9.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvOUUsers.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvOUUsers.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp9.SortColumn = e.Column;
                    lvOUUsers.Sorting = SortOrder.Ascending;
                }

                lvOUUsers.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.lvOUUsers_ColumnClick(): " + ex.Message + "\n");
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

                lvSelectedUsers.Sort();
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " ApplUserXWU.lvSelectedUsers_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void lvOUSelectedUsers_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvOUSelectedUsers.Sorting;

                if (e.Column == _comp10.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvOUSelectedUsers.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvOUSelectedUsers.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp10.SortColumn = e.Column;
                    lvOUSelectedUsers.Sorting = SortOrder.Ascending;
                }

                lvOUSelectedUsers.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.lvOUSelectedUsers_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            this.Cursor = Cursors.WaitCursor;
            ApplUsersXWU userXWU = null;

			try
			{

                if (removedUsersWU.Count > 0 || addedUsersWU.Count > 0)
                {
                    bool isSaved = true;
                    List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
                    WorkingUnit wu = new WorkingUnit();

                    userXWU = new ApplUsersXWU();
                    userXWU.BeginTransaction();

                    if (removedUsersWU.Count > 0)
                    {
                        // delete
                        foreach (ListViewItem remItem in removedUsersWU)
                        {
                            Item tag = (Item)remItem.Tag;
                            wUnits = new List<WorkingUnitTO>();
                            wu = new WorkingUnit();
                            wu.SetTransaction(userXWU.GetTransaction());
                            wUnits.Add(tag.WU.WUTO);
                            wUnits = wu.FindAllChildren(wUnits);

                            foreach (WorkingUnitTO wUnit in wUnits)
                            {
                                isSaved = userXWU.Delete(tag.UserID, wUnit.WorkingUnitID, tag.Purpose, false) && isSaved;

                                if (!isSaved)
                                    break;
                            }

                            if (!isSaved)
                                break;
                        }
                    }

                    if (isSaved && addedUsersWU.Count > 0)
                    {
                        // insert
                        foreach (ListViewItem addItem in addedUsersWU)
                        {
                            Item tag = (Item)addItem.Tag;
                            wUnits = new List<WorkingUnitTO>();
                            wu = new WorkingUnit();
                            wu.SetTransaction(userXWU.GetTransaction());
                            wUnits.Add(tag.WU.WUTO);
                            wUnits = wu.FindAllChildren(wUnits);

                            foreach (WorkingUnitTO wUnit in wUnits)
                            {
                                isSaved = userXWU.Delete(tag.UserID, wUnit.WorkingUnitID, tag.Purpose, false) && isSaved;

                                isSaved = (userXWU.Save(tag.UserID, wUnit.WorkingUnitID, tag.Purpose, false) > 0 ? true : false) && isSaved;

                                if (!isSaved)
                                    break;
                            }

                            if (!isSaved)
                                break;
                        }
                    }

                    if (isSaved)
                    {
                        userXWU.CommitTransaction();
                        MessageBox.Show(rm.GetString("applUserXWUSaved", culture));
                        
                        clearListView(lvSelectedUsers);
                        lvWorkingUnits.SelectedItems.Clear();
                        lvPurposes.SelectedItems.Clear();
                        lvUsers.SelectedItems.Clear();
                    }
                    else
                    {
                        userXWU.RollbackTransaction();
                        MessageBox.Show(rm.GetString("applUserXWUNotSaved", culture));
                    }
                }
			}
			catch(Exception ex)
			{
                if (userXWU != null && userXWU.GetTransaction() != null)
                {
                    userXWU.RollbackTransaction();
                }

				log.writeLog(DateTime.Now + " ApplUserXWU.btnSave_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void btnOUSave_Click(object sender, System.EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ApplUserXOrgUnit userXOU = null;

            try
            {
                if (removedUsersOU.Count > 0 || addedUsersOU.Count > 0)
                {
                    bool isSaved = true;
                    List<OrganizationalUnitTO> oUnits = new List<OrganizationalUnitTO>();
                    OrganizationalUnit ou = new OrganizationalUnit();

                    userXOU = new ApplUserXOrgUnit();
                    userXOU.BeginTransaction();

                    if (removedUsersOU.Count > 0)
                    {
                        // delete
                        foreach (ListViewItem remItem in removedUsersOU)
                        {
                            Item tag = (Item)remItem.Tag;
                            oUnits = new List<OrganizationalUnitTO>();
                            ou = new OrganizationalUnit();
                            ou.SetTransaction(userXOU.GetTransaction());
                            oUnits.Add(tag.OU.OrgUnitTO);
                            oUnits = ou.FindAllChildren(oUnits);

                            foreach (OrganizationalUnitTO oUnit in oUnits)
                            {
                                isSaved = userXOU.Delete(tag.UserID, oUnit.OrgUnitID, tag.Purpose, false) && isSaved;

                                if (!isSaved)
                                    break;
                            }

                            if (!isSaved)
                                break;
                        }
                    }

                    if (isSaved && addedUsersOU.Count > 0)
                    {
                        // insert
                        foreach (ListViewItem addItem in addedUsersOU)
                        {
                            Item tag = (Item)addItem.Tag;
                            oUnits = new List<OrganizationalUnitTO>();
                            ou = new OrganizationalUnit();
                            ou.SetTransaction(userXOU.GetTransaction());
                            oUnits.Add(tag.OU.OrgUnitTO);
                            oUnits = ou.FindAllChildren(oUnits);

                            foreach (OrganizationalUnitTO oUnit in oUnits)
                            {
                                isSaved = userXOU.Delete(tag.UserID, oUnit.OrgUnitID, tag.Purpose, false) && isSaved;

                                isSaved = (userXOU.Save(tag.UserID, oUnit.OrgUnitID, tag.Purpose, false) > 0 ? true : false) && isSaved;

                                if (!isSaved)
                                    break;
                            }

                            if (!isSaved)
                                break;
                        }
                    }

                    if (isSaved)
                    {
                        userXOU.CommitTransaction();
                        MessageBox.Show(rm.GetString("applUserXWUSaved", culture));

                        clearListView(lvOUSelectedUsers);
                        lvOUnits.SelectedItems.Clear();
                        lvOUPurposes.SelectedItems.Clear();
                        lvOUUsers.SelectedItems.Clear();
                    }
                    else
                    {
                        userXOU.RollbackTransaction();
                        MessageBox.Show(rm.GetString("applUserXWUNotSaved", culture));
                    }
                }
            }
            catch (Exception ex)
            {
                if (userXOU != null && userXOU.GetTransaction() != null)
                {
                    userXOU.RollbackTransaction();
                }

                log.writeLog(DateTime.Now + " ApplUserXWU.btnOUSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

		private void cbPurpose_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                cbUser_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " ApplUserXWU.cbPurpose_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void cbOUPurpose_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                cbOUUser_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.cbOUPurpose_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

		private void cbPurpose1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                cbWUName_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.cbPurpose1_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void cbOUPurpose1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                cbOUName_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.cbOUPurpose1_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void ApplUserXWU_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " ApplUserXWU.ApplUserXWU_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvWorkingUnits_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvWorkingUnits.Sorting;

                if (e.Column == _comp5.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvWorkingUnits.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvWorkingUnits.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp5.SortColumn = e.Column;
                    lvWorkingUnits.Sorting = SortOrder.Ascending;
                }

                lvWorkingUnits.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.lvWorkingUnits_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvOUnits_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvOUnits.Sorting;

                if (e.Column == _comp11.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvOUnits.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvOUnits.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp11.SortColumn = e.Column;
                    lvOUnits.Sorting = SortOrder.Ascending;
                }

                lvOUnits.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.lvOUnits_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvPurposes_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvPurposes.Sorting;

                if (e.Column == _comp6.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvPurposes.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvPurposes.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp6.SortColumn = e.Column;
                    lvPurposes.Sorting = SortOrder.Ascending;
                }

                lvPurposes.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.lvPurposes_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvOUPurposes_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvOUPurposes.Sorting;

                if (e.Column == _comp12.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvOUPurposes.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvOUPurposes.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp12.SortColumn = e.Column;
                    lvOUPurposes.Sorting = SortOrder.Ascending;
                }

                lvOUPurposes.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.lvOUPurposes_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvWorkingUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (showWUMessage && (removedUsersWU.Count > 0 || addedUsersWU.Count > 0))
                {
                    DialogResult result = MessageBox.Show(rm.GetString("UserXWUSaveChanges", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        btnSave.PerformClick();
                    }
                }

                clearListView(lvSelectedUsers);
                showWUMessage = true;

                if (this.lvWorkingUnits.SelectedItems.Count > 0 && this.lvPurposes.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvWorkingUnits.SelectedItems)
                    {
                        WorkingUnitTO wu = (WorkingUnitTO)item.Tag;

                        foreach (ListViewItem itemP in lvPurposes.SelectedItems)
                        {
                            string purpose = itemP.Text.Trim();
                            if (wu.WorkingUnitID != -1 && !purpose.Equals(""))
                            {
                                List<string> statuses = new List<string>();
                                statuses.Add(Constants.statusActive);
                                statuses.Add(Constants.statusDisabled);
                                populateSelUserListView(new ApplUsersXWU().FindUsersForWU(wu.WorkingUnitID, purpose, statuses), wu, purpose);
                                foreach (ListViewItem itemU in lvSelectedUsers.Items)
                                {
                                    originalSelectedUserListWU.Add(itemU);
                                }
                            }
                        }
                    }
                }
                
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " ApplUserXWU.lvWorkingUnits_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvOUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (showOUMessage && (removedUsersOU.Count > 0 || addedUsersOU.Count > 0))
                {
                    DialogResult result = MessageBox.Show(rm.GetString("UserXOUSaveChanges", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        btnSave.PerformClick();
                    }
                }

                clearListView(lvOUSelectedUsers);
                showOUMessage = true;

                if (this.lvOUnits.SelectedItems.Count > 0 && this.lvOUPurposes.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvOUnits.SelectedItems)
                    {
                        OrganizationalUnitTO ou = (OrganizationalUnitTO)item.Tag;

                        foreach (ListViewItem itemP in lvOUPurposes.SelectedItems)
                        {
                            string purpose = itemP.Text.Trim();
                            if (ou.OrgUnitID != -1 && !purpose.Equals(""))
                            {
                                List<string> statuses = new List<string>();
                                statuses.Add(Constants.statusActive);
                                statuses.Add(Constants.statusDisabled);
                                populateOUSelUserListView(new ApplUserXOrgUnit().FindUsersForOU(ou.OrgUnitID, purpose, statuses), ou, purpose);
                                foreach (ListViewItem itemU in lvOUSelectedUsers.Items)
                                {
                                    originalSelectedUserListOU.Add(itemU);
                                }
                            }
                        }
                    }
                }

                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " ApplUserXWU.lvOUnits_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvPurposes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                lvWorkingUnits_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.lvPurposes_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvOUPurposes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                lvOUnits_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.lvPurposes_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                lvSelectedUsers.BeginUpdate();

                foreach (ListViewItem item in lvSelectedUsers.SelectedItems)
                {
                    if (!item.Text.Equals("SYS"))
                    {
                        Item tag = (Item)item.Tag;
                        if (!parentWUHasPermission(tag.UserID, tag.WU.WUTO.WorkingUnitID, tag.Purpose))
                        {
                            removedUsersWU.Add(item);

                            int index = contain(addedUsersWU, item);
                            if (index >= 0)
                            {
                                addedUsersWU.RemoveAt(index);
                            }
                            lvSelectedUsers.Items.Remove(item);

                            index = contain(originalSelectedUserListWU, item);
                            if (index >= 0)
                            {
                                originalSelectedUserListWU.RemoveAt(index);
                            }
                        }
                        else
                        {
                            MessageBox.Show(item.Text.Trim() + ": " + rm.GetString("ParentWUPermission", culture));	
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("SYSAllPermission", culture));
                    }
                }
                
                lvSelectedUsers.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.btnRemoveSelected_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnOURemoveSelected_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                lvOUSelectedUsers.BeginUpdate();

                foreach (ListViewItem item in lvOUSelectedUsers.SelectedItems)
                {
                    if (!item.Text.Equals("SYS"))
                    {
                        Item tag = (Item)item.Tag;
                        if (!parentOUHasPermission(tag.UserID, tag.OU.OrgUnitTO.OrgUnitID, tag.Purpose))
                        {
                            removedUsersOU.Add(item);

                            int index = contain(addedUsersOU, item);
                            if (index >= 0)
                            {
                                addedUsersOU.RemoveAt(index);
                            }
                            lvOUSelectedUsers.Items.Remove(item);

                            index = contain(originalSelectedUserListOU, item);
                            if (index >= 0)
                            {
                                originalSelectedUserListOU.RemoveAt(index);
                            }
                        }
                        else
                        {
                            MessageBox.Show(item.Text.Trim() + ": " + rm.GetString("ParentOUPermission", culture));
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("SYSAllPermission", culture));
                    }
                }

                lvOUSelectedUsers.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.btnOURemoveSelected_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private int contain(List<ListViewItem> list, ListViewItem item)
        {
            try
            {
                int index = -1;
                bool found = false;

                foreach (ListViewItem listItem in list)
                {
                    index++;

                    if (((Item)listItem.Tag).equals(item))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    index = -1;
                
                return index;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.contain(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool parentWUHasPermission(string userID, int wuID, string purpose)
        {
            try
            {
                ApplUsersXWU auXwu = new ApplUsersXWU();
                auXwu.AuXWUnitTO.Purpose = purpose;
                auXwu.AuXWUnitTO.UserID = userID;
                auXwu.AuXWUnitTO.WorkingUnitID = wuID;
                List<ApplUsersXWUTO> parentWU = auXwu.FindGrantedParentWUForWU();

                return parentWU.Count > 0;
						
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.parentWUHasPermission(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool parentOUHasPermission(string userID, int ouID, string purpose)
        {
            try
            {
                ApplUserXOrgUnit auXou = new ApplUserXOrgUnit();
                auXou.AuXOUnitTO.Purpose = purpose;
                auXou.AuXOUnitTO.UserID = userID;
                auXou.AuXOUnitTO.OrgUnitID = ouID;
                List<ApplUserXOrgUnitTO> parentOU = auXou.FindGrantedParentOUForOU();

                return parentOU.Count > 0;

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.parentOUHasPermission(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnWUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string wuString = "";
                foreach (WorkingUnitTO wUnit in wuArray)
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
                    this.cbWUName.SelectedIndex = cbWUName.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnOUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string ouString = "";
                foreach (OrganizationalUnitTO oUnit in ouArray)
                {
                    ouString += oUnit.OrgUnitID.ToString().Trim() + ",";
                }
                if (ouString.Length > 0)
                {
                    ouString = ouString.Substring(0, ouString.Length - 1);
                }

                System.Data.DataSet dsOrgUnits = new OrganizationalUnit().getOrganizationUnits(ouString);
                WorkingUnitsTreeView orgUnitsTreeView = new WorkingUnitsTreeView(dsOrgUnits);
                orgUnitsTreeView.ShowDialog();
                if (!orgUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbOUName.SelectedIndex = cbOUName.FindStringExact(orgUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.btnOUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbRetiredUsersWU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                populateUserCombo();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.chbRetiredUsersWU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbRetiredUsersOU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                populateOUUserCombo();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.chbRetiredUsersOU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbRetiredUnitsWU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                cbWUName_SelectedIndexChanged(this, new EventArgs());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.chbRetiredUnitsWU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbRetiredUnitsOU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                cbOUName_SelectedIndexChanged(this, new EventArgs());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXWU.chbRetiredUnitsOU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }        
	}
}
