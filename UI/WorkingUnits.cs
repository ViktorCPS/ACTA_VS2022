using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

using Common;
using TransferObjects;
using Util;

namespace UI
{
	/// <summary>
	/// Maintenance screen for WorkingUnits
	/// </summary>
	public class WorkingUnits : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox tbDescription;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.ListView lvWorkingUnits;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnSearch;
		private CultureInfo culture;

		// List View indexes
		//const int WorkingUnitID = 0;
		const int WUName = 0;
		const int Description = 1;
		const int ParentWUID = 2;		

		private ListViewItemComparer _comp;
		private System.Windows.Forms.Label lblParentWUID;
		private System.Windows.Forms.ComboBox cbParentUnitID;

		// Current WOking Unit
        protected WorkingUnitTO currentWorkingUnit;
        private IContainer components;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.GroupBox gbWorkingUnits;

		DebugLog log;
		ResourceManager rm;
		ApplUserTO logInUser;
		List<ApplRoleTO> currentRoles;
		Hashtable menuItemsPermissions;
		string menuItemID;

		bool readPermission = false;
		bool addPermission = false;
		bool updatePermission = false;
		bool deletePermission = false;

		string messageWUSearch1 = "";
		string messageWUUpd1 = "";
		string messageWUUpd2 = "";
		string messageWUDel1 = "";
		string messageWUDel2 = "";
		string messageWUDel3 = "";
        private Button btnEmplXWU;
        private TabControl tabControl1;
        private TabPage tpListView;
        private TabPage tpTreeView;
        private DataTreeView dataTreeView1;
        public System.Windows.Forms.ImageList imageList1;
        private System.Data.DataSet dsWorkingUnits;
        public System.Data.DataView wuDataView;
        WorkingUnitTO currentWUTree;
        ToolTip toolTip1;
        private Button btnEmplXWUTree;
        private Button btnCloseTree;
        private Button btnDeleteTree;
        private Button btnUpdateTree;
        private Button btnAddTree;
        private Label lblRefreshToolTip;
        private Button btnWUTree;
		string messageWUDel4 = "";

        List<WorkingUnitTO> wuArray;

		public WorkingUnits()
		{
			InitializeComponent();
			
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);
			
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(WorkingUnits).Assembly);
			currentWorkingUnit = new WorkingUnitTO();
            currentWUTree = new WorkingUnitTO();
			logInUser = NotificationController.GetLogInUser();
            toolTip1 = new ToolTip();
			setLanguage();
			this.CenterToScreen();
			this.cbParentUnitID.DropDownStyle = ComboBoxStyle.DropDownList;
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkingUnits));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lvWorkingUnits = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.cbParentUnitID = new System.Windows.Forms.ComboBox();
            this.lblParentWUID = new System.Windows.Forms.Label();
            this.gbWorkingUnits = new System.Windows.Forms.GroupBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.btnEmplXWU = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpTreeView = new System.Windows.Forms.TabPage();
            this.lblRefreshToolTip = new System.Windows.Forms.Label();
            this.dataTreeView1 = new UI.DataTreeView();
            this.btnEmplXWUTree = new System.Windows.Forms.Button();
            this.btnCloseTree = new System.Windows.Forms.Button();
            this.btnDeleteTree = new System.Windows.Forms.Button();
            this.btnUpdateTree = new System.Windows.Forms.Button();
            this.btnAddTree = new System.Windows.Forms.Button();
            this.tpListView = new System.Windows.Forms.TabPage();
            this.gbWorkingUnits.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpTreeView.SuspendLayout();
            this.tpListView.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Network.ico");
            this.imageList1.Images.SetKeyName(1, "Msn-Buddy-mobile.ico");
            this.imageList1.Images.SetKeyName(2, "Msn-Buddy.ico");
            this.imageList1.Images.SetKeyName(3, "Msn-Messenger.ico");
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(24, 56);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(136, 23);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(24, 24);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(136, 23);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(168, 56);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(272, 20);
            this.tbDescription.TabIndex = 4;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(168, 24);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(272, 20);
            this.tbName.TabIndex = 2;
            // 
            // lvWorkingUnits
            // 
            this.lvWorkingUnits.FullRowSelect = true;
            this.lvWorkingUnits.GridLines = true;
            this.lvWorkingUnits.Location = new System.Drawing.Point(6, 180);
            this.lvWorkingUnits.Name = "lvWorkingUnits";
            this.lvWorkingUnits.Size = new System.Drawing.Size(627, 216);
            this.lvWorkingUnits.TabIndex = 8;
            this.lvWorkingUnits.UseCompatibleStateImageBehavior = false;
            this.lvWorkingUnits.View = System.Windows.Forms.View.Details;
            this.lvWorkingUnits.SelectedIndexChanged += new System.EventHandler(this.lvWorkingUnits_SelectedIndexChanged);
            this.lvWorkingUnits.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvWorkingUnits_ColumnClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(6, 419);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(168, 419);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(87, 419);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 10;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(365, 128);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(537, 419);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 23);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cbParentUnitID
            // 
            this.cbParentUnitID.Location = new System.Drawing.Point(168, 88);
            this.cbParentUnitID.Name = "cbParentUnitID";
            this.cbParentUnitID.Size = new System.Drawing.Size(272, 21);
            this.cbParentUnitID.TabIndex = 6;
            // 
            // lblParentWUID
            // 
            this.lblParentWUID.Location = new System.Drawing.Point(16, 88);
            this.lblParentWUID.Name = "lblParentWUID";
            this.lblParentWUID.Size = new System.Drawing.Size(144, 23);
            this.lblParentWUID.TabIndex = 5;
            this.lblParentWUID.Text = "Parent Working Unit ID:";
            this.lblParentWUID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbWorkingUnits
            // 
            this.gbWorkingUnits.Controls.Add(this.btnWUTree);
            this.gbWorkingUnits.Controls.Add(this.tbName);
            this.gbWorkingUnits.Controls.Add(this.lblName);
            this.gbWorkingUnits.Controls.Add(this.lblDescription);
            this.gbWorkingUnits.Controls.Add(this.tbDescription);
            this.gbWorkingUnits.Controls.Add(this.lblParentWUID);
            this.gbWorkingUnits.Controls.Add(this.cbParentUnitID);
            this.gbWorkingUnits.Controls.Add(this.btnSearch);
            this.gbWorkingUnits.Location = new System.Drawing.Point(6, 6);
            this.gbWorkingUnits.Name = "gbWorkingUnits";
            this.gbWorkingUnits.Size = new System.Drawing.Size(482, 168);
            this.gbWorkingUnits.TabIndex = 0;
            this.gbWorkingUnits.TabStop = false;
            this.gbWorkingUnits.Text = "Working Units";
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(446, 86);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 20;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // btnEmplXWU
            // 
            this.btnEmplXWU.Location = new System.Drawing.Point(304, 419);
            this.btnEmplXWU.Name = "btnEmplXWU";
            this.btnEmplXWU.Size = new System.Drawing.Size(192, 23);
            this.btnEmplXWU.TabIndex = 12;
            this.btnEmplXWU.Text = "Employees <-> Working Units";
            this.btnEmplXWU.UseVisualStyleBackColor = true;
            this.btnEmplXWU.Click += new System.EventHandler(this.btnEmplXWU_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpTreeView);
            this.tabControl1.Controls.Add(this.tpListView);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(647, 474);
            this.tabControl1.TabIndex = 14;
            // 
            // tpTreeView
            // 
            this.tpTreeView.Controls.Add(this.lblRefreshToolTip);
            this.tpTreeView.Controls.Add(this.dataTreeView1);
            this.tpTreeView.Controls.Add(this.btnEmplXWUTree);
            this.tpTreeView.Controls.Add(this.btnCloseTree);
            this.tpTreeView.Controls.Add(this.btnDeleteTree);
            this.tpTreeView.Controls.Add(this.btnUpdateTree);
            this.tpTreeView.Controls.Add(this.btnAddTree);
            this.tpTreeView.Location = new System.Drawing.Point(4, 22);
            this.tpTreeView.Name = "tpTreeView";
            this.tpTreeView.Padding = new System.Windows.Forms.Padding(3);
            this.tpTreeView.Size = new System.Drawing.Size(639, 448);
            this.tpTreeView.TabIndex = 1;
            this.tpTreeView.Text = "Tree";
            this.tpTreeView.UseVisualStyleBackColor = true;
            // 
            // lblRefreshToolTip
            // 
            this.lblRefreshToolTip.Location = new System.Drawing.Point(7, 392);
            this.lblRefreshToolTip.Name = "lblRefreshToolTip";
            this.lblRefreshToolTip.Size = new System.Drawing.Size(626, 23);
            this.lblRefreshToolTip.TabIndex = 25;
            this.lblRefreshToolTip.MouseHover += new System.EventHandler(this.lblRefreshToolTip_MouseHover);
            // 
            // dataTreeView1
            // 
            this.dataTreeView1.DataMember = null;
            this.dataTreeView1.FullRowSelect = true;
            this.dataTreeView1.HideSelection = false;
            this.dataTreeView1.HotTracking = true;
            this.dataTreeView1.ImageIndex = 3;
            this.dataTreeView1.ImageList = this.imageList1;
            this.dataTreeView1.Location = new System.Drawing.Point(6, 12);
            this.dataTreeView1.Name = "dataTreeView1";
            this.dataTreeView1.SelectedImageIndex = 3;
            this.dataTreeView1.SelectedNodes = ((System.Collections.ArrayList)(resources.GetObject("dataTreeView1.SelectedNodes")));
            this.dataTreeView1.ShowNodeToolTips = true;
            this.dataTreeView1.Size = new System.Drawing.Size(627, 377);
            this.dataTreeView1.TabIndex = 1;
            this.dataTreeView1.ValueColumn = null;
            this.dataTreeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.workingUnitsTreeView_AfterSelect);
            this.dataTreeView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataTreeView1_MouseMove);
            // 
            // btnEmplXWUTree
            // 
            this.btnEmplXWUTree.Location = new System.Drawing.Point(304, 419);
            this.btnEmplXWUTree.Name = "btnEmplXWUTree";
            this.btnEmplXWUTree.Size = new System.Drawing.Size(192, 23);
            this.btnEmplXWUTree.TabIndex = 24;
            this.btnEmplXWUTree.Text = "Employees <-> Working Units";
            this.btnEmplXWUTree.UseVisualStyleBackColor = true;
            this.btnEmplXWUTree.Click += new System.EventHandler(this.btnEmplXWUTree_Click);
            // 
            // btnCloseTree
            // 
            this.btnCloseTree.Location = new System.Drawing.Point(537, 419);
            this.btnCloseTree.Name = "btnCloseTree";
            this.btnCloseTree.Size = new System.Drawing.Size(96, 23);
            this.btnCloseTree.TabIndex = 23;
            this.btnCloseTree.Text = "Close";
            this.btnCloseTree.UseVisualStyleBackColor = true;
            this.btnCloseTree.Click += new System.EventHandler(this.btnCloseTree_Click);
            // 
            // btnDeleteTree
            // 
            this.btnDeleteTree.Location = new System.Drawing.Point(168, 419);
            this.btnDeleteTree.Name = "btnDeleteTree";
            this.btnDeleteTree.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteTree.TabIndex = 22;
            this.btnDeleteTree.Text = "Delete";
            this.btnDeleteTree.UseVisualStyleBackColor = true;
            this.btnDeleteTree.Click += new System.EventHandler(this.btnDeleteTree_Click);
            // 
            // btnUpdateTree
            // 
            this.btnUpdateTree.Location = new System.Drawing.Point(87, 419);
            this.btnUpdateTree.Name = "btnUpdateTree";
            this.btnUpdateTree.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateTree.TabIndex = 21;
            this.btnUpdateTree.Text = "Update";
            this.btnUpdateTree.UseVisualStyleBackColor = true;
            this.btnUpdateTree.Click += new System.EventHandler(this.btnUpdateTree_Click);
            // 
            // btnAddTree
            // 
            this.btnAddTree.Location = new System.Drawing.Point(6, 419);
            this.btnAddTree.Name = "btnAddTree";
            this.btnAddTree.Size = new System.Drawing.Size(75, 23);
            this.btnAddTree.TabIndex = 20;
            this.btnAddTree.Text = "Add";
            this.btnAddTree.UseVisualStyleBackColor = true;
            this.btnAddTree.Click += new System.EventHandler(this.btnAddTree_Click);
            // 
            // tpListView
            // 
            this.tpListView.Controls.Add(this.gbWorkingUnits);
            this.tpListView.Controls.Add(this.btnClose);
            this.tpListView.Controls.Add(this.btnEmplXWU);
            this.tpListView.Controls.Add(this.lvWorkingUnits);
            this.tpListView.Controls.Add(this.btnAdd);
            this.tpListView.Controls.Add(this.btnDelete);
            this.tpListView.Controls.Add(this.btnUpdate);
            this.tpListView.Location = new System.Drawing.Point(4, 22);
            this.tpListView.Name = "tpListView";
            this.tpListView.Padding = new System.Windows.Forms.Padding(3);
            this.tpListView.Size = new System.Drawing.Size(639, 448);
            this.tpListView.TabIndex = 0;
            this.tpListView.Text = "List";
            this.tpListView.UseVisualStyleBackColor = true;
            // 
            // WorkingUnits
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(671, 502);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.MinimumSize = new System.Drawing.Size(631, 504);
            this.Name = "WorkingUnits";
            this.ShowInTaskbar = false;
            this.Text = "WorklingUnits";
            this.Load += new System.EventHandler(this.WorkingUnits_Load);
            this.Closed += new System.EventHandler(this.WorkingUnits_Closed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WorkingUnits_KeyUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WorkingUnits_MouseMove);
            this.gbWorkingUnits.ResumeLayout(false);
            this.gbWorkingUnits.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tpTreeView.ResumeLayout(false);
            this.tpListView.ResumeLayout(false);
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
					case WorkingUnits.Description:
					case WorkingUnits.ParentWUID:
					case WorkingUnits.WUName:
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
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			WorkingUnits workingUnits = new WorkingUnits();
			Application.Run(workingUnits);
		}

		/// <summary>
		/// Delete selected Working Units. Deleting working unit is setting its status to 'RETIRED'.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			// All employees that belongs to this working unit 
			// must be deleted first
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int selected = lvWorkingUnits.SelectedItems.Count;

                if (lvWorkingUnits.SelectedItems.Count > 0)
                {
                    DialogResult result1 = MessageBox.Show(messageWUDel1, "", MessageBoxButtons.YesNo);
                    if (result1 == DialogResult.Yes)
                    {
                        foreach (ListViewItem item in lvWorkingUnits.SelectedItems)
                        {
                            if (((WorkingUnitTO)item.Tag).WorkingUnitID == 0)
                            {
                                MessageBox.Show(rm.GetString("defaultWUDel", culture));
                                selected--;
                            }
                            else
                            {
                                if (((WorkingUnitTO)item.Tag).WorkingUnitID == Constants.basicVisitorCode)
                                {
                                    MessageBox.Show(rm.GetString("visitsWUDel", culture));
                                    selected--;
                                }
                                else
                                {
                                    // Check if some Employees belong to this working unit
                                    Employee empl = new Employee();
                                    empl.EmplTO.WorkingUnitID = ((WorkingUnitTO)item.Tag).WorkingUnitID;
                                    List<EmployeeTO> employeeList = empl.Search();
                                    // If some Employees belong to this working unit, delete them first
                                    if (employeeList.Count > 0)
                                    {
                                        MessageBox.Show(item.Text + ": " + messageWUDel2);
                                        selected--;
                                    }
                                    else
                                    {
                                        // Check if some Visits belong to this working unit
                                        ArrayList visitList = new Visit().Search(-1, -1, "", "", "", "",
                                            new DateTime(0), new DateTime(0), -1, ((WorkingUnitTO)item.Tag).WorkingUnitID,
                                            "", -1, "");
                                        // If some Visits belong to this working unit, delete them first
                                        if (visitList.Count > 0)
                                        {
                                            MessageBox.Show(item.Text + ": " + rm.GetString("wuHasVisits", culture));
                                            selected--;
                                        }
                                        else
                                        {
                                            /*
                                            // Check if some Users are granted to this working unit
                                            ArrayList userList = new ApplUsersXWU().Search("", item.Tag.ToString().Trim(), "");
                                            // If some Users are granted to this working unit, delete them first
                                            if (userList.Count > 0)
                                            {
                                                MessageBox.Show(item.Text + ": " + rm.GetString("wuHasUsers", culture));
                                                selected--;
                                            }
							
                                            else
                                            {*/
                                            // Check if this Working Unit is a parent working unit to some other
                                            //ArrayList childWU = currentWorkingUnit.Search("", item.Text.Trim(), "", "", "");
                                            WorkingUnit workUnit = new WorkingUnit();
                                            workUnit.WUTO.ParentWorkingUID = ((WorkingUnitTO)item.Tag).WorkingUnitID;
                                            workUnit.WUTO.Status = Constants.DefaultStateActive;
                                            List<WorkingUnitTO> childWU = workUnit.Search();

                                            if (childWU.Count > 1)
                                            {
                                                MessageBox.Show(item.Text + ": " + messageWUDel3);
                                                selected--;
                                            }
                                            else
                                            {
                                                bool isDeleted = true;
                                                // This working unit is a parent to itself
                                                if ((childWU.Count == 1) && (employeeList.Count == 0))
                                                {
                                                    WorkingUnit wu = new WorkingUnit();
                                                    wu.WUTO = childWU[0];
                                                    if (wu.WUTO.WorkingUnitID.Equals(wu.WUTO.ParentWorkingUID))
                                                    {
                                                        bool trans = wu.BeginTransaction();
                                                        if (trans)
                                                        {
                                                            ApplUsersXWU applUsersXWU = new ApplUsersXWU();
                                                            applUsersXWU.SetTransaction(wu.GetTransaction());
                                                            isDeleted = applUsersXWU.Delete(wu.WUTO.WorkingUnitID, false) && isDeleted;
                                                            if (isDeleted)
                                                            {
                                                                //isDeleted = wu.Delete(wu.WorkingUnitID) && isDeleted;
                                                                isDeleted = wu.Delete(wu.WUTO.WorkingUnitID, false) && isDeleted;
                                                            }

                                                            if (isDeleted)
                                                            {
                                                                wu.CommitTransaction();

                                                                //if ((selected > 0) && isDeleted)
                                                                MessageBox.Show(rm.GetString("wuDeleted", culture));
                                                            }
                                                            else
                                                            {
                                                                wu.RollbackTransaction();

                                                                //else if (!isDeleted)
                                                                MessageBox.Show(rm.GetString("wuNotDeleted", culture));
                                                            }
                                                            currentWorkingUnit = new WorkingUnitTO();
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                                                            return;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show(item.Text + ": " + messageWUDel3);
                                                        selected--;
                                                    }
                                                }

                                                // There is no child working units to this one
                                                if ((childWU.Count == 0) && (employeeList.Count == 0))
                                                {
                                                    WorkingUnit wu = new WorkingUnit();
                                                    bool trans = wu.BeginTransaction();
                                                    if (trans)
                                                    {
                                                        try
                                                        {
                                                            ApplUsersXWU applUsersXWU = new ApplUsersXWU();
                                                            applUsersXWU.SetTransaction(wu.GetTransaction());
                                                            isDeleted = applUsersXWU.Delete(((WorkingUnitTO)item.Tag).WorkingUnitID, false) && isDeleted;
                                                            if (isDeleted)
                                                            {
                                                                //isDeleted = currentWorkingUnit.Delete((int) item.Tag) && isDeleted;
                                                                isDeleted = wu.Delete(((WorkingUnitTO)item.Tag).WorkingUnitID, false) && isDeleted;
                                                            }

                                                            if (isDeleted)
                                                            {
                                                                wu.CommitTransaction();

                                                                //if ((selected > 0) && isDeleted)
                                                                MessageBox.Show(rm.GetString("wuDeleted", culture));
                                                            }
                                                            else
                                                            {
                                                                wu.RollbackTransaction();

                                                                //else if (!isDeleted)
                                                                MessageBox.Show(rm.GetString("wuNotDeleted", culture));
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            if (wu.GetTransaction() != null)
                                                                wu.RollbackTransaction();
                                                            throw ex;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                                                        return;
                                                    }
                                                }
                                            }
                                            //}
                                        }
                                    }
                                }
                            }
                        }

                        WorkingUnit workingUnit = new WorkingUnit();
                        workingUnit.WUTO.Status = Constants.DefaultStateActive;
                        List<WorkingUnitTO> workingUnitsList = workingUnit.Search();

                        populateListView(workingUnitsList);
                        //cbParentUnitID.SelectedValue = currentWorkingUnit.ParentWorkingUID;
                        tbName.Text = "";
                        tbDescription.Text = "";
                        currentWorkingUnit = new WorkingUnitTO();
                        populateWorkigUnitCombo();
                        cbParentUnitID.SelectedValue = -1;

                        dsWorkingUnits = new WorkingUnit().getWorkingUnits("");
                        populateDataTreeView();

                        this.Invalidate();
                    }
                }
                else
                {
                    MessageBox.Show(messageWUDel4);
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnits.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (this.lvWorkingUnits.SelectedItems.Count == 0)
                {
                    MessageBox.Show(messageWUUpd1);
                }
                else if (this.lvWorkingUnits.SelectedItems.Count > 1)
                {
                    MessageBox.Show(messageWUUpd2);
                }
                else
                {
                    currentWorkingUnit = (WorkingUnitTO)lvWorkingUnits.SelectedItems[0].Tag;

                    // Open Update Form
                    WorkingUnitsAdd addForm = new WorkingUnitsAdd(currentWorkingUnit);
                    addForm.ShowDialog(this);

                    WorkingUnit wUnit = new WorkingUnit();
                    wUnit.WUTO.Status = Constants.DefaultStateActive;
                    List<WorkingUnitTO> wudaoList = wUnit.Search();
                    populateListView(wudaoList);
                    tbName.Text = "";
                    tbDescription.Text = "";
                    currentWorkingUnit = new WorkingUnitTO();
                    populateWorkigUnitCombo();
                    cbParentUnitID.SelectedValue = -1;

                    Common.WorkingUnit wu = new WorkingUnit();
                    dsWorkingUnits = wu.getWorkingUnits("");
                    populateDataTreeView();

                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnits.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Value -1 is assigned to "ALL" in cbParentUnitID
                //if (Int32.Parse(cbParentUnitID.SelectedValue.ToString().Trim()) != -1)
                //{
                //    partentWUID = cbParentUnitID.SelectedValue.ToString().Trim();
                //}

                WorkingUnit wUnit = new WorkingUnit();
                wUnit.WUTO.ParentWorkingUID = (int)cbParentUnitID.SelectedValue;
                wUnit.WUTO.Description = tbDescription.Text.Trim();
                wUnit.WUTO.Name = tbName.Text.Trim();
                wUnit.WUTO.Status = Constants.DefaultStateActive;
                List<WorkingUnitTO> workingUnitsList = wUnit.Search();

                if (workingUnitsList.Count > 0)
                {
                    populateListView(workingUnitsList);
                }
                else
                {
                    MessageBox.Show(messageWUSearch1);
                }

                currentWorkingUnit = new WorkingUnitTO();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnits.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }		
		}

		/// <summary>
		/// Populate List View with Working Units found.
		/// </summary>
		/// <param name="workingUnitsList"></param>
		public void populateListView(List<WorkingUnitTO> workingUnitsList)
		{
			try
			{
				lvWorkingUnits.BeginUpdate();
				lvWorkingUnits.Items.Clear();
				
				WorkingUnit tempWu = new WorkingUnit();
				if (workingUnitsList.Count > 0)
				{
					foreach(WorkingUnitTO wunit in workingUnitsList)
					{
						ListViewItem item = new ListViewItem();
												
						//if ((currentWorkingUnit.Find(Int32.Parse(item.Text.Trim()))) && (currentWorkingUnit.WorkingUnitID != 0))
                        //if ((tempWu.Find(wunit.WorkingUnitID)))// && (tempWu.WorkingUnitID != 0))
                        //{
							item.Text = wunit.Name.Trim();
							item.SubItems.Add(wunit.Description.ToString().Trim());
                            tempWu.WUTO.ParentWorkingUID = wunit.ParentWorkingUID;
							item.SubItems.Add(tempWu.getParentWorkingUnit().Name.Trim());
							item.Tag = wunit;
					
							lvWorkingUnits.Items.Add(item);
						//}
					}
				}

				lvWorkingUnits.EndUpdate();
				lvWorkingUnits.Invalidate();
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " WorkingUnits.setLanguage(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Set proper language.
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// Form name
				this.Text = rm.GetString("WUForm", culture);
				
				// group box text
				gbWorkingUnits.Text = rm.GetString("gbWorkingUnits", culture);

                // tab page's text
                tpListView.Text = rm.GetString("tpListView", culture);
                tpTreeView.Text = rm.GetString("tpTreeView", culture);

				// button's text
				btnAdd.Text = rm.GetString("btnAdd", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnDelete.Text = rm.GetString("btnDelete", culture);
				btnSearch.Text = rm.GetString("btnSearch", culture);
                btnEmplXWU.Text = rm.GetString("btnEmplXWU", culture);
				btnClose.Text = rm.GetString("btnClose", culture);
                btnAddTree.Text = rm.GetString("btnAdd", culture);
                btnUpdateTree.Text = rm.GetString("btnUpdate", culture);
                btnDeleteTree.Text = rm.GetString("btnDelete", culture);
                btnEmplXWUTree.Text = rm.GetString("btnEmplXWU", culture);
                btnCloseTree.Text = rm.GetString("btnClose", culture);

				// label's text
				lblParentWUID.Text = rm.GetString("lblParentWUID", culture);
				lblDescription.Text = rm.GetString("lblDescription", culture);
				lblName.Text = rm.GetString("lblName", culture);

				// message's text
				messageWUDel1 = rm.GetString("messageWUDel1", culture);
				messageWUDel2 = rm.GetString("messageWUDel2", culture);
				messageWUDel3 = rm.GetString("messageWUDel3", culture);
				messageWUDel4 = rm.GetString("messageWUDel4", culture);
				messageWUSearch1 = rm.GetString("messageWUSearch1", culture);
				messageWUUpd1 = rm.GetString("messageWUUpd1", culture);
				messageWUUpd2 = rm.GetString("messageWUUpd2", culture);

				lvWorkingUnits.BeginUpdate();
				lvWorkingUnits.Columns.Add(rm.GetString("lblName", culture), (lvWorkingUnits.Width - 3) / 3, HorizontalAlignment.Left);
				lvWorkingUnits.Columns.Add(rm.GetString("lblDescription", culture), (lvWorkingUnits.Width - 3) / 3, HorizontalAlignment.Left);
				lvWorkingUnits.Columns.Add(rm.GetString("lblParentWUID", culture), (lvWorkingUnits.Width - 3) / 3, HorizontalAlignment.Left);
				lvWorkingUnits.EndUpdate();
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " WorkingUnit.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void lvWorkingUnits_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                currentWorkingUnit = new WorkingUnitTO();
                currentWorkingUnit = new WorkingUnitTO();
                // populate Employee's search form
                if (lvWorkingUnits.SelectedItems.Count == 1)
                {
                    currentWorkingUnit = (WorkingUnitTO)lvWorkingUnits.SelectedItems[0].Tag;

                    tbName.Text = currentWorkingUnit.Name;
                    tbDescription.Text = currentWorkingUnit.Description;
                    cbParentUnitID.SelectedValue = currentWorkingUnit.ParentWorkingUID;
                }
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " WorkingUnit.lvWorkingUnits_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
		}

		private void WorkingUnits_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                _comp = new ListViewItemComparer(lvWorkingUnits);
                lvWorkingUnits.ListViewItemSorter = _comp;
                lvWorkingUnits.Sorting = SortOrder.Ascending;

                populateWorkigUnitCombo();

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                WorkingUnit wu = new WorkingUnit();
                wu.WUTO.Status = Constants.DefaultStateActive;
                List<WorkingUnitTO> workingUnitsList = wu.Search();
                populateListView(workingUnitsList);
                dsWorkingUnits = new WorkingUnit().getWorkingUnits("");
                populateDataTreeView();

                //BLOKIRANJE -> status najnoviji ODBLOKIRANO 18.01.2018
                /*
                btnAddTree.Enabled = false;
                btnDeleteTree.Enabled = false;
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                 * */
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.WorkingUnits_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }	
		}

		private void lvWorkingUnits_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvWorkingUnits.Sorting;
                lvWorkingUnits.Sorting = SortOrder.None;

                if (e.Column == _comp.SortColumn)
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
                    _comp.SortColumn = e.Column;
                    lvWorkingUnits.Sorting = SortOrder.Ascending;
                }
                lvWorkingUnits.ListViewItemSorter = _comp;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.lvWorkingUnits_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		/// <summary>
		/// Populate combo box with values form data table. 
		/// Exclude those which are child of this working unit.
		/// </summary>
		private void populateWorkigUnitCombo()
		{
            WorkingUnit wu = new WorkingUnit();
            wu.WUTO.Status = Constants.DefaultStateActive;
			wuArray = wu.Search();
			List<WorkingUnitTO> actualWu = new List<WorkingUnitTO>();
			// Add All as a first member of combo
			actualWu.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), Constants.DefaultStateActive.ToString(), 0));

			foreach(WorkingUnitTO wuMember in wuArray)
			{
				// TODO: Is this part necessary? 0 or DEFAULT Working Unit shod be shown by its name!!!!!
				if (wuMember.WorkingUnitID == 0)
				{
					//wuMember.Name = "DEFAULT";
					actualWu.Insert(1, wuMember);
				}
				else
				{
					if (!currentWorkingUnit.WorkingUnitID.Equals(0))
					{
						if ((wuMember.ParentWorkingUID != currentWorkingUnit.WorkingUnitID) || (wuMember.WorkingUnitID == 0))
						{
							actualWu.Add(wuMember);
						}
						else
						{
							if (wuMember.WorkingUnitID == currentWorkingUnit.WorkingUnitID)
							{
								actualWu.Add(wuMember);
							}
						}
					}
					else
					{
						actualWu.Add(wuMember);
					}
				}
			}

			cbParentUnitID.DataSource = actualWu;
			cbParentUnitID.DisplayMember = "Name";
			cbParentUnitID.ValueMember = "WorkingUnitID";
		}

		/// <summary>
		/// Prepare search strings.
		/// </summary>
		/// <param name="forParsing"></param>
		/// <returns></returns>
		private string parse(string forParsing)
		{
			string parsedString = forParsing.Trim();
			if (parsedString.StartsWith("*"))
			{
				parsedString = parsedString.Substring(1);
				parsedString = "%" + parsedString;
			}

			if (parsedString.EndsWith("*"))
			{
				parsedString = parsedString.Substring(0, parsedString.Length - 1);
				parsedString = parsedString + "%";
			}

			return parsedString;
		}

		private void WorkingUnits_Closed(object sender, System.EventArgs e)
		{
			currentWorkingUnit = new WorkingUnitTO();
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
                log.writeLog(DateTime.Now + " WorkingUnits.btnClose_Click(): " + ex.Message + "\n");
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
				// Open Add Form
				WorkingUnitsAdd addForm = new WorkingUnitsAdd();
				addForm.ShowDialog(this);

                WorkingUnit wu = new WorkingUnit();
                wu.WUTO.Status = Constants.DefaultStateActive;
				List<WorkingUnitTO> wudaoList = wu.Search();
				populateListView(wudaoList);
				
                tbName.Text = "";
				tbDescription.Text = "";
				currentWorkingUnit =  new WorkingUnitTO();
				populateWorkigUnitCombo();
				cbParentUnitID.SelectedValue = -1;

                wu = new WorkingUnit();
                dsWorkingUnits = wu.getWorkingUnits("");
                populateDataTreeView();

				this.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WorkingUnits.btnAdd_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void setVisibility()
		{
			try
			{
				int permission;

				foreach (ApplRoleTO role in currentRoles)
				{
					permission = (((int[]) menuItemsPermissions[menuItemID])[role.ApplRoleID]);
					readPermission = readPermission || (((permission / 8) % 2) == 0 ? false : true);
					addPermission = addPermission || (((permission / 4) % 2) == 0 ? false : true);
					updatePermission = updatePermission || (((permission / 2) % 2) == 0 ? false : true);
					deletePermission = deletePermission || ((permission % 2) == 0 ? false : true);
				}

				btnSearch.Enabled = readPermission;
				btnAdd.Enabled = btnAddTree.Enabled = addPermission;
				btnUpdate.Enabled = btnUpdateTree.Enabled = updatePermission;
				btnDelete.Enabled = btnDeleteTree.Enabled = deletePermission;
                btnEmplXWU.Enabled = btnEmplXWUTree.Enabled = readPermission && addPermission && updatePermission && deletePermission;
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.setVisibility(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

        private void btnEmplXWU_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                EmployeesXWorkingUnits emplXWU = new EmployeesXWorkingUnits();
                emplXWU.ShowDialog(this);
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " WorkingUnits.btnEmplXWU_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void WorkingUnits_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " WorkingUnits.WorkingUnits_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void workingUnitsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if ((dataTreeView1.SelectedNodes.Count >= 0) && (ModifierKeys != Keys.Shift) && (ModifierKeys != Keys.Control))
                {
                    TreeNode node = e.Node;

                    currentWUTree = new WorkingUnitTO();
                    // WorkingUnit parentWU = new WorkingUnit();
                    if (node != null)
                    {
                        currentWUTree = new WorkingUnit().FindWU((int)node.Tag);
                        //parentWU = currentWorkingUnit.GetParentLocation();
                    }
                }
                else
                {
                    currentWUTree = new WorkingUnitTO();
                }
            }

            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnits.locationsTreeView_AfterSelect(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void dataTreeView1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                // Get the node at the current mouse pointer location.
                TreeNode theNode = this.dataTreeView1.GetNodeAt(e.X, e.Y);
                if ((theNode != null))
                {
                    // Verify that the tag property is not "null".
                    if (theNode.ToolTipText != "")
                    {
                        // Change the ToolTip only if the pointer moved to a new node.
                        if (theNode.ToolTipText != this.toolTip1.GetToolTip(this.dataTreeView1))
                        {
                            this.toolTip1.Show(theNode.ToolTipText, this, e.X +50, e.Y +100);
                        }
                    }
                    else
                    {
                        this.toolTip1.SetToolTip(this, "");
                    }
                }
                else     // Pointer is not over a node so clear the ToolTip.
                {
                    this.toolTip1.SetToolTip(this, "");
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnits.dataTreeView1_MouseMove(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }
        private void WorkingUnits_MouseMove(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this, "");
        }
        private void lblRefreshToolTip_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this, "");

        }
        private void populateDataTreeView()
        {
            try
            {
                if (dsWorkingUnits != null)
                {
                    this.wuDataView = dsWorkingUnits.Tables["Working units"].DefaultView;
                    BindingSource bs = new BindingSource();
                    bs.DataSource = this.wuDataView;

                    this.dataTreeView1.IDColumn = "";
                    this.dataTreeView1.ParentIDColumn = "";
                    this.dataTreeView1.ToolTipTextColumn = "";
                    this.dataTreeView1.NameColumn = "";
                    this.dataTreeView1.DataSource = bs;
                    this.dataTreeView1.IDColumn = "working_unit_id";
                    this.dataTreeView1.ParentIDColumn = "parent_working_unit_id";
                    this.dataTreeView1.ToolTipTextColumn = "description";
                    this.dataTreeView1.NameColumn = "name";
                    this.dataTreeView1.Refresh();
                    dataTreeView1.ExpandAll();
                    dataTreeView1.SelectedNode = dataTreeView1.Nodes[0];
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnits.populateDataTreeView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddTree_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
            
                WorkingUnitsAdd workingUnitsAdd = new WorkingUnitsAdd();
                workingUnitsAdd.ShowDialog();

                WorkingUnit wu = new WorkingUnit();
                dsWorkingUnits = wu.getWorkingUnits("");
                populateDataTreeView();

                wu.WUTO.Status = Constants.DefaultStateActive;
                List<WorkingUnitTO> wudaoList = wu.Search();
                populateListView(wudaoList);
                tbName.Text = "";
                tbDescription.Text = "";
                currentWorkingUnit = new WorkingUnitTO();
                populateWorkigUnitCombo();
                cbParentUnitID.SelectedValue = -1;
                this.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnits.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUpdateTree_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.dataTreeView1.SelectedNodes.Count == 0)
                {
                    MessageBox.Show(messageWUUpd1);
                }
                else if (this.dataTreeView1.SelectedNodes.Count > 1)
                {
                    MessageBox.Show(messageWUUpd2);
                }
                else
                {
                    WorkingUnitsAdd workingUnitsAdd = new WorkingUnitsAdd(currentWUTree);
                    workingUnitsAdd.ShowDialog();

                    WorkingUnit wu = new WorkingUnit();
                    dsWorkingUnits = wu.getWorkingUnits("");
                    populateDataTreeView();

                    wu.WUTO.Status = Constants.DefaultStateActive;
                    List<WorkingUnitTO> wudaoList = wu.Search();
                    populateListView(wudaoList);
                    tbName.Text = "";
                    tbDescription.Text = "";
                    currentWorkingUnit = new WorkingUnitTO();
                    populateWorkigUnitCombo();
                    cbParentUnitID.SelectedValue = -1;
                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnits.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDeleteTree_Click(object sender, EventArgs e)
        {
            // All employees that belongs to this working unit 
            // must be deleted first
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int selected = dataTreeView1.SelectedNodes.Count;

                if (dataTreeView1.SelectedNodes.Count > 0)
                {
                    DialogResult result1 = MessageBox.Show(messageWUDel1, "", MessageBoxButtons.YesNo);
                    if (result1 == DialogResult.Yes)
                    {
                        foreach (TreeNode node in dataTreeView1.SelectedNodes)
                        {
                            if (((int)node.Tag) == 0)
                            {
                                MessageBox.Show(rm.GetString("defaultWUDel",culture));
                                selected--;
                            }
                            else
                            {
                                if (((int)node.Tag) == Constants.basicVisitorCode)
                                {
                                    MessageBox.Show(rm.GetString("visitsWUDel", culture));
                                    selected--;
                                }
                                else
                                {
                                    // Check if some Employees belong to this working unit
                                    Employee empl = new Employee();
                                    empl.EmplTO.WorkingUnitID = (int)node.Tag;
                                    List<EmployeeTO> employeeList = empl.Search();
                                    // If some Employees belong to this working unit, delete them first
                                    if (employeeList.Count > 0)
                                    {
                                        MessageBox.Show(node.Text + ": " + messageWUDel2);
                                        selected--;
                                    }
                                    else
                                    {
                                        // Check if some Visits belong to this working unit
                                        ArrayList visitList = new Visit().Search(-1, -1, "", "", "", "",
                                            new DateTime(0), new DateTime(0), -1, (int)node.Tag,
                                            "", -1, "");
                                        // If some Visits belong to this working unit, delete them first
                                        if (visitList.Count > 0)
                                        {
                                            MessageBox.Show(node.Text + ": " + rm.GetString("wuHasVisits", culture));
                                            selected--;
                                        }
                                        else
                                        {
                                            /*
                                            // Check if some Users are granted to this working unit
                                            ArrayList userList = new ApplUsersXWU().Search("", item.Tag.ToString().Trim(), "");
                                            // If some Users are granted to this working unit, delete them first
                                            if (userList.Count > 0)
                                            {
                                                MessageBox.Show(item.Text + ": " + rm.GetString("wuHasUsers", culture));
                                                selected--;
                                            }
							
                                            else
                                            {*/
                                            // Check if this Working Unit is a parent working unit to some other
                                            //ArrayList childWU = currentWorkingUnit.Search("", item.Text.Trim(), "", "", "");

                                            WorkingUnit wUnit = new WorkingUnit();
                                            wUnit.WUTO.ParentWorkingUID = (int)node.Tag;
                                            wUnit.WUTO.Status = Constants.DefaultStateActive;

                                            List<WorkingUnitTO> childWU = wUnit.Search();

                                            if (childWU.Count > 1)
                                            {
                                                MessageBox.Show(node.Text + ": " + messageWUDel3);
                                                selected--;
                                            }
                                            else
                                            {
                                                bool isDeleted = true;
                                                // This working unit is a parent to itself
                                                if ((childWU.Count == 1) && (employeeList.Count == 0))
                                                {
                                                    WorkingUnit wu = new WorkingUnit();
                                                    wu.WUTO = childWU[0];
                                                    if (wu.WUTO.WorkingUnitID == wu.WUTO.ParentWorkingUID)
                                                    {
                                                        bool trans = wu.BeginTransaction();
                                                        if (trans)
                                                        {
                                                            ApplUsersXWU applUsersXWU = new ApplUsersXWU();
                                                            applUsersXWU.SetTransaction(wu.GetTransaction());
                                                            isDeleted = applUsersXWU.Delete(wu.WUTO.WorkingUnitID, false) && isDeleted;
                                                            if (isDeleted)
                                                            {
                                                                //isDeleted = wu.Delete(wu.WorkingUnitID) && isDeleted;
                                                                isDeleted = wu.Delete(wu.WUTO.WorkingUnitID, false) && isDeleted;
                                                            }

                                                            if (isDeleted)
                                                            {
                                                                wu.CommitTransaction();

                                                                //if ((selected > 0) && isDeleted)
                                                                MessageBox.Show(rm.GetString("wuDeleted", culture));
                                                            }
                                                            else
                                                            {
                                                                wu.RollbackTransaction();

                                                                //else if (!isDeleted)
                                                                MessageBox.Show(rm.GetString("wuNotDeleted", culture));
                                                            }
                                                            currentWUTree = new WorkingUnitTO();
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                                                            return;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show(node.Text + ": " + messageWUDel3);
                                                        selected--;
                                                    }
                                                }

                                                // There is no child working units to this one
                                                if ((childWU.Count == 0) && (employeeList.Count == 0))
                                                {
                                                    WorkingUnit wu = new WorkingUnit();
                                                    bool trans = wu.BeginTransaction();
                                                    if (trans)
                                                    {
                                                        try
                                                        {
                                                            ApplUsersXWU applUsersXWU = new ApplUsersXWU();
                                                            applUsersXWU.SetTransaction(wu.GetTransaction());
                                                            isDeleted = applUsersXWU.Delete((int)node.Tag, false) && isDeleted;
                                                            if (isDeleted)
                                                            {
                                                                //isDeleted = currentWorkingUnit.Delete((int) item.Tag) && isDeleted;
                                                                isDeleted = wu.Delete((int)node.Tag, false) && isDeleted;
                                                            }

                                                            if (isDeleted)
                                                            {
                                                                wu.CommitTransaction();

                                                                //if ((selected > 0) && isDeleted)
                                                                MessageBox.Show(rm.GetString("wuDeleted", culture));
                                                            }
                                                            else
                                                            {
                                                                wu.RollbackTransaction();

                                                                //else if (!isDeleted)
                                                                MessageBox.Show(rm.GetString("wuNotDeleted", culture));
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            if (wu.GetTransaction() != null)
                                                                wu.RollbackTransaction();
                                                            throw ex;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show(rm.GetString("cannotStartTransaction",culture));
                                                        return;
                                                    }
                                                }
                                            }
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                        dsWorkingUnits = new WorkingUnit().getWorkingUnits("");
                        populateDataTreeView();

                        WorkingUnit workUnit = new WorkingUnit();
                        workUnit.WUTO.Status = Constants.DefaultStateActive;

                        List<WorkingUnitTO> wudaoList = workUnit.Search();
                        populateListView(wudaoList);
                        tbName.Text = "";
                        tbDescription.Text = "";
                        currentWorkingUnit = new WorkingUnitTO();
                        populateWorkigUnitCombo();
                        cbParentUnitID.SelectedValue = -1;
                        this.Invalidate();
                    }
                }
                else
                {
                    MessageBox.Show(messageWUDel4);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnits.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnEmplXWUTree_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                EmployeesXWorkingUnits emplXWU = new EmployeesXWorkingUnits();
                emplXWU.ShowDialog(this);
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " WorkingUnits.btnEmplXWU_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
          
        }

        private void btnCloseTree_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnits.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                    this.cbParentUnitID.SelectedIndex = cbParentUnitID.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employees.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
