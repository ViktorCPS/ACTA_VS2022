using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Data;

using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for Locations.
	/// </summary>
	public class Locations : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.TextBox tbDescription;
		private System.Windows.Forms.Label lblParentLocationID;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ListView lvLocations;
        private IContainer components;
		
		LocationTO currentLocation = null;

		ApplUserTO logInUser;
		List<ApplRoleTO> currentRoles;
		Hashtable menuItemsPermissions;
		string menuItemID;

        ToolTip toolTip1 = new ToolTip();
        TreeNode currentNode = new TreeNode();
        LocationTO currentLocationTree;

		bool readPermission = false;
		bool addPermission = false;
		bool updatePermission = false;
		bool deletePermission = false;

		DebugLog log;

		// List View indexes
		//const int LocationIDIndex = 0;
		const int NameIndex = 0;
		const int DescriptionIndex = 1;
		const int ParentLocationIDIndex = 2;

		private ListViewItemComparer _comp;
		private System.Windows.Forms.ComboBox cbPrentLocation;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.GroupBox gbLocations;

		private CultureInfo culture;
		ResourceManager rm;

		string messageLocUpd1 = "";
		string messageLocUpd2 = "";
		string messageLocDel1 = "";
		string messageLocDel2 = "";
		string messageLocDel3 = "";
		string messageLocDel4 = "";
        private TabControl tabControl1;
        private TabPage tpListView;
        private TabPage tpTreeView;
        private DataTreeView dataTreeView1;
        public System.Data.DataView locationsDataView;
        public System.Windows.Forms.BindingSource bindingSource1;
        public System.Windows.Forms.ImageList imageList1;
        private System.Data.DataSet dsLocations;
        private Button btnCloseTree;
        private Button btnDeleteTree;
        private Button btnUpdateTree;
        private Button btnAddTree;
        private Label lblRefreshToolTip;
        private Button btnLocationTree;
		string messageLocSearch1 = "";
        private Button btnMaps;
        private Button btnMaps1;

        List<LocationTO> actualLocation;

		public Locations()
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

            currentLocationTree = new LocationTO();
			currentLocation = new LocationTO();
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(Locations).Assembly);
			setLanguage();
			this.cbPrentLocation.DropDownStyle = ComboBoxStyle.DropDownList;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Locations));
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lblName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblParentLocationID = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lvLocations = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.cbPrentLocation = new System.Windows.Forms.ComboBox();
            this.gbLocations = new System.Windows.Forms.GroupBox();
            this.btnLocationTree = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpTreeView = new System.Windows.Forms.TabPage();
            this.btnMaps = new System.Windows.Forms.Button();
            this.lblRefreshToolTip = new System.Windows.Forms.Label();
            this.dataTreeView1 = new UI.DataTreeView();
            this.btnCloseTree = new System.Windows.Forms.Button();
            this.btnDeleteTree = new System.Windows.Forms.Button();
            this.btnUpdateTree = new System.Windows.Forms.Button();
            this.btnAddTree = new System.Windows.Forms.Button();
            this.tpListView = new System.Windows.Forms.TabPage();
            this.btnMaps1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.gbLocations.SuspendLayout();
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
            this.imageList1.Images.SetKeyName(1, "home5.ico");
            this.imageList1.Images.SetKeyName(2, "home6.ico");
            this.imageList1.Images.SetKeyName(3, "location.ico");
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(48, 24);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(72, 23);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(136, 24);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(328, 20);
            this.tbName.TabIndex = 2;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(40, 56);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(80, 23);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(136, 56);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(328, 20);
            this.tbDescription.TabIndex = 4;
            // 
            // lblParentLocationID
            // 
            this.lblParentLocationID.Location = new System.Drawing.Point(16, 88);
            this.lblParentLocationID.Name = "lblParentLocationID";
            this.lblParentLocationID.Size = new System.Drawing.Size(104, 23);
            this.lblParentLocationID.TabIndex = 5;
            this.lblParentLocationID.Text = "Parent Location:";
            this.lblParentLocationID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(6, 467);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(87, 467);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 10;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(168, 467);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(411, 130);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lvLocations
            // 
            this.lvLocations.FullRowSelect = true;
            this.lvLocations.GridLines = true;
            this.lvLocations.HideSelection = false;
            this.lvLocations.Location = new System.Drawing.Point(6, 180);
            this.lvLocations.Name = "lvLocations";
            this.lvLocations.Size = new System.Drawing.Size(615, 258);
            this.lvLocations.TabIndex = 8;
            this.lvLocations.UseCompatibleStateImageBehavior = false;
            this.lvLocations.View = System.Windows.Forms.View.Details;
            this.lvLocations.SelectedIndexChanged += new System.EventHandler(this.lvLocations_SelectedIndexChanged);
            this.lvLocations.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvLocations_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(533, 467);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cbPrentLocation
            // 
            this.cbPrentLocation.Location = new System.Drawing.Point(136, 88);
            this.cbPrentLocation.Name = "cbPrentLocation";
            this.cbPrentLocation.Size = new System.Drawing.Size(328, 21);
            this.cbPrentLocation.TabIndex = 6;
            // 
            // gbLocations
            // 
            this.gbLocations.Controls.Add(this.btnLocationTree);
            this.gbLocations.Controls.Add(this.lblName);
            this.gbLocations.Controls.Add(this.tbName);
            this.gbLocations.Controls.Add(this.lblDescription);
            this.gbLocations.Controls.Add(this.tbDescription);
            this.gbLocations.Controls.Add(this.lblParentLocationID);
            this.gbLocations.Controls.Add(this.cbPrentLocation);
            this.gbLocations.Controls.Add(this.btnSearch);
            this.gbLocations.Location = new System.Drawing.Point(4, 6);
            this.gbLocations.Name = "gbLocations";
            this.gbLocations.Size = new System.Drawing.Size(501, 168);
            this.gbLocations.TabIndex = 0;
            this.gbLocations.TabStop = false;
            this.gbLocations.Text = "Locations";
            // 
            // btnLocationTree
            // 
            this.btnLocationTree.Image = ((System.Drawing.Image)(resources.GetObject("btnLocationTree.Image")));
            this.btnLocationTree.Location = new System.Drawing.Point(470, 87);
            this.btnLocationTree.Name = "btnLocationTree";
            this.btnLocationTree.Size = new System.Drawing.Size(25, 24);
            this.btnLocationTree.TabIndex = 21;
            this.btnLocationTree.Click += new System.EventHandler(this.btnLocationTree_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpTreeView);
            this.tabControl1.Controls.Add(this.tpListView);
            this.tabControl1.Location = new System.Drawing.Point(16, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(6, 6);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(635, 528);
            this.tabControl1.TabIndex = 13;
            // 
            // tpTreeView
            // 
            this.tpTreeView.Controls.Add(this.btnMaps);
            this.tpTreeView.Controls.Add(this.lblRefreshToolTip);
            this.tpTreeView.Controls.Add(this.dataTreeView1);
            this.tpTreeView.Controls.Add(this.btnCloseTree);
            this.tpTreeView.Controls.Add(this.btnDeleteTree);
            this.tpTreeView.Controls.Add(this.btnUpdateTree);
            this.tpTreeView.Controls.Add(this.btnAddTree);
            this.tpTreeView.Location = new System.Drawing.Point(4, 28);
            this.tpTreeView.Name = "tpTreeView";
            this.tpTreeView.Padding = new System.Windows.Forms.Padding(3);
            this.tpTreeView.Size = new System.Drawing.Size(627, 496);
            this.tpTreeView.TabIndex = 1;
            this.tpTreeView.Text = "Tree";
            this.tpTreeView.UseVisualStyleBackColor = true;
            // 
            // btnMaps
            // 
            this.btnMaps.Image = ((System.Drawing.Image)(resources.GetObject("btnMaps.Image")));
            this.btnMaps.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMaps.Location = new System.Drawing.Point(349, 467);
            this.btnMaps.Name = "btnMaps";
            this.btnMaps.Size = new System.Drawing.Size(77, 23);
            this.btnMaps.TabIndex = 27;
            this.btnMaps.Text = "Maps";
            this.btnMaps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMaps.UseVisualStyleBackColor = true;
            this.btnMaps.Click += new System.EventHandler(this.btnMaps_Click);
            // 
            // lblRefreshToolTip
            // 
            this.lblRefreshToolTip.Location = new System.Drawing.Point(6, 438);
            this.lblRefreshToolTip.Name = "lblRefreshToolTip";
            this.lblRefreshToolTip.Size = new System.Drawing.Size(626, 23);
            this.lblRefreshToolTip.TabIndex = 26;
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
            this.dataTreeView1.Size = new System.Drawing.Size(615, 423);
            this.dataTreeView1.TabIndex = 1;
            this.dataTreeView1.ValueColumn = null;
            this.dataTreeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.locationsTreeView_AfterSelect);
            this.dataTreeView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataTreeView1_MouseMove);
            // 
            // btnCloseTree
            // 
            this.btnCloseTree.Location = new System.Drawing.Point(533, 467);
            this.btnCloseTree.Name = "btnCloseTree";
            this.btnCloseTree.Size = new System.Drawing.Size(88, 23);
            this.btnCloseTree.TabIndex = 16;
            this.btnCloseTree.Text = "Close";
            this.btnCloseTree.UseVisualStyleBackColor = true;
            this.btnCloseTree.Click += new System.EventHandler(this.btnCloseTree_Click);
            // 
            // btnDeleteTree
            // 
            this.btnDeleteTree.Location = new System.Drawing.Point(168, 467);
            this.btnDeleteTree.Name = "btnDeleteTree";
            this.btnDeleteTree.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteTree.TabIndex = 15;
            this.btnDeleteTree.Text = "Delete";
            this.btnDeleteTree.UseVisualStyleBackColor = true;
            this.btnDeleteTree.Click += new System.EventHandler(this.btnDeleteTree_Click);
            // 
            // btnUpdateTree
            // 
            this.btnUpdateTree.Location = new System.Drawing.Point(87, 467);
            this.btnUpdateTree.Name = "btnUpdateTree";
            this.btnUpdateTree.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateTree.TabIndex = 14;
            this.btnUpdateTree.Text = "Update";
            this.btnUpdateTree.UseVisualStyleBackColor = true;
            this.btnUpdateTree.Click += new System.EventHandler(this.btnUpdateTree_Click);
            // 
            // btnAddTree
            // 
            this.btnAddTree.Location = new System.Drawing.Point(6, 467);
            this.btnAddTree.Name = "btnAddTree";
            this.btnAddTree.Size = new System.Drawing.Size(75, 23);
            this.btnAddTree.TabIndex = 13;
            this.btnAddTree.Text = "Add";
            this.btnAddTree.UseVisualStyleBackColor = true;
            this.btnAddTree.Click += new System.EventHandler(this.btnAddTree_Click);
            // 
            // tpListView
            // 
            this.tpListView.Controls.Add(this.btnMaps1);
            this.tpListView.Controls.Add(this.gbLocations);
            this.tpListView.Controls.Add(this.btnClose);
            this.tpListView.Controls.Add(this.lvLocations);
            this.tpListView.Controls.Add(this.btnDelete);
            this.tpListView.Controls.Add(this.btnAdd);
            this.tpListView.Controls.Add(this.btnUpdate);
            this.tpListView.Location = new System.Drawing.Point(4, 28);
            this.tpListView.Name = "tpListView";
            this.tpListView.Padding = new System.Windows.Forms.Padding(3);
            this.tpListView.Size = new System.Drawing.Size(627, 496);
            this.tpListView.TabIndex = 0;
            this.tpListView.Text = "List ";
            this.tpListView.UseVisualStyleBackColor = true;
            // 
            // btnMaps1
            // 
            this.btnMaps1.Image = ((System.Drawing.Image)(resources.GetObject("btnMaps1.Image")));
            this.btnMaps1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMaps1.Location = new System.Drawing.Point(349, 467);
            this.btnMaps1.Name = "btnMaps1";
            this.btnMaps1.Size = new System.Drawing.Size(78, 23);
            this.btnMaps1.TabIndex = 28;
            this.btnMaps1.Text = "Maps";
            this.btnMaps1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMaps1.UseVisualStyleBackColor = true;
            this.btnMaps1.Click += new System.EventHandler(this.btnMaps1_Click);
            // 
            // Locations
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(663, 552);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.MinimumSize = new System.Drawing.Size(528, 504);
            this.Name = "Locations";
            this.ShowInTaskbar = false;
            this.Text = "Locations";
            this.Load += new System.EventHandler(this.Locations_Load);
            this.Closed += new System.EventHandler(this.Locations_Closed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Locations_KeyUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Locations_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.gbLocations.ResumeLayout(false);
            this.gbLocations.PerformLayout();
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
					case Locations.NameIndex:
					case Locations.DescriptionIndex:
					case Locations.ParentLocationIDIndex:
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
				this.Text = rm.GetString("locationForm", culture);

				// group box text
				gbLocations.Text = rm.GetString("gbLocations", culture);

                // tab page's text
                tpListView.Text = rm.GetString("tpListView", culture);
                tpTreeView.Text = rm.GetString("tpTreeView", culture);
				
				// button's text
				btnAdd.Text = rm.GetString("btnAdd", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnDelete.Text = rm.GetString("btnDelete", culture);
				btnSearch.Text = rm.GetString("btnSearch", culture);
				btnClose.Text = rm.GetString("btnClose", culture);
                btnAddTree.Text = rm.GetString("btnAdd", culture);
                btnUpdateTree.Text = rm.GetString("btnUpdate", culture);
                btnDeleteTree.Text = rm.GetString("btnDelete", culture);
                btnCloseTree.Text = rm.GetString("btnClose", culture);
                btnMaps.Text = rm.GetString("btnMaps", culture);
                btnMaps1.Text = rm.GetString("btnMaps", culture);

				// label's text
				lblName.Text = rm.GetString("lblName", culture);
				lblDescription.Text = rm.GetString("lblDescription", culture);
				lblParentLocationID.Text = rm.GetString("lblParentLocationID", culture);

				// message's text
				messageLocUpd1 = rm.GetString("messageLocUpd1", culture);
				messageLocUpd2 = rm.GetString("messageLocUpd2", culture);
				messageLocDel1 = rm.GetString("messageLocDel1", culture);
				messageLocDel2 = rm.GetString("messageLocDel2", culture);
				messageLocDel3 = rm.GetString("messageLocDel3", culture);
				messageLocDel4 = rm.GetString("messageLocDel4", culture);
				messageLocSearch1 = rm.GetString("messageLocSearch1", culture);
				
				lvLocations.BeginUpdate();
				lvLocations.Columns.Add(rm.GetString("lblName", culture), (lvLocations.Width - 3)/ 3, HorizontalAlignment.Left);
				lvLocations.Columns.Add(rm.GetString("lblDescription", culture), (lvLocations.Width - 3) / 3, HorizontalAlignment.Left);
				lvLocations.Columns.Add(rm.GetString("lblParentLocationID", culture), (lvLocations.Width - 3) / 3, HorizontalAlignment.Left);
				lvLocations.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Locations.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}

		}

		/// <summary>
		/// Populate List View with Locations found
		/// </summary>
		/// <param name="locationsList"></param>
		public void populateListView(List<LocationTO> locationsList)
		{
			LocationTO tempLoc = new LocationTO();
			try
			{
				lvLocations.BeginUpdate();
				lvLocations.Items.Clear();

				if (locationsList.Count > 0)
				{
					foreach(LocationTO location in locationsList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = location.Name.ToString().Trim();
						item.SubItems.Add(location.Description.ToString().Trim());

                        Location loc = new Location();
                        loc.LocTO = location;
						tempLoc = loc.GetParentLocation();
						item.SubItems.Add(tempLoc.Name);

						item.Tag = location;

						lvLocations.Items.Add(item);
					}
				}

				lvLocations.EndUpdate();
				lvLocations.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Locations.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);

			}
		}

		/// <summary>
		/// On Load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Locations_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Initialize comparer object
                _comp = new ListViewItemComparer(lvLocations);
                lvLocations.ListViewItemSorter = _comp;
                lvLocations.Sorting = SortOrder.Ascending;
                populateParentLocationCb();

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
                List<LocationTO> locationList = loc.Search();
                populateListView(locationList);
                dsLocations = new Location().getLocations("");
                populateDataTreeView();
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " Locations.Locations_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void populateDataTreeView()
        {
            try
            {
                if (dsLocations != null)
                {
                    this.locationsDataView = dsLocations.Tables["Locations"].DefaultView;
                    BindingSource bs = new BindingSource();
                    bs.DataSource = this.locationsDataView;
                    this.dataTreeView1.ShowNodeToolTips = true;
                    this.dataTreeView1.IDColumn = "";
                    this.dataTreeView1.ParentIDColumn = "";
                    this.dataTreeView1.ToolTipTextColumn = "";
                    this.dataTreeView1.NameColumn = "";
                    this.dataTreeView1.DataSource = bs;
                    this.dataTreeView1.IDColumn = "location_id";
                    this.dataTreeView1.ParentIDColumn = "parent_location_id";
                    this.dataTreeView1.ToolTipTextColumn = "description";
                    this.dataTreeView1.NameColumn = "name";

                    this.dataTreeView1.Refresh();
                    dataTreeView1.ExpandAll();
                    dataTreeView1.SelectedNode = dataTreeView1.Nodes[0];
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Locations.populateDataTreeView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

		private void lvLocations_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvLocations.Sorting;
                lvLocations.Sorting = SortOrder.None;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvLocations.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvLocations.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvLocations.Sorting = SortOrder.Ascending;
                }
                lvLocations.ListViewItemSorter = _comp;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Locations.lvLocations_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvLocations_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvLocations.SelectedItems.Count == 1)
                {
                    currentLocation = (LocationTO)lvLocations.SelectedItems[0].Tag;

                    tbName.Text = currentLocation.Name.ToString();
                    tbDescription.Text = currentLocation.Description.ToString();
                    cbPrentLocation.SelectedValue = (currentLocation.ParentLocationID < 0) ? 0 : currentLocation.ParentLocationID;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Locations.lvLocations_SelectedIndexChanged(): " + ex.Message + "\n");
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

				if (this.lvLocations.SelectedItems.Count == 0)
				{
					MessageBox.Show(messageLocUpd1);
				}
				else if (this.lvLocations.SelectedItems.Count > 1)
				{
					MessageBox.Show(messageLocUpd2);
				}
				else 
				{
					currentLocation = (LocationTO)lvLocations.SelectedItems[0].Tag;

					// Open Update Form
					LocationsAdd addForm = new LocationsAdd(currentLocation);
					addForm.ShowDialog(this);

                    Location loc = new Location();
                    loc.LocTO.Status = Constants.DefaultStateActive.Trim();
					List<LocationTO> locationList = loc.Search();
					populateListView(locationList);
					tbName.Text = "";
					tbDescription.Text = "";
					currentLocation = new LocationTO();
					populateParentLocationCb();
					cbPrentLocation.SelectedValue = -1;

                    dsLocations = new Location().getLocations("");
                    populateDataTreeView();
					this.Invalidate();
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Locations.btnUpdate_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		/// <summary>
		/// Delete selected Locations. Deleting location is setting its status to 'RETIRED'.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			try
			{ 
                this.Cursor = Cursors.WaitCursor;
            
				int selected = lvLocations.SelectedItems.Count;
				if(lvLocations.SelectedItems.Count > 0)
				{
					// Check if this Location represent parent location to some other location
					// if it is the case, ask user to remove those location first
					List<LocationTO> childLocArray = new List<LocationTO>(); 
					List<ReaderTO> readerArray = new List<ReaderTO>();
					Reader tmpReader = new Reader();

					DialogResult result = MessageBox.Show(messageLocDel1, "", MessageBoxButtons.YesNo);
					if (result == DialogResult.Yes)
					{
						foreach(ListViewItem item in lvLocations.SelectedItems)
						{
							if (((LocationTO)item.Tag).LocationID == 0)
							{
								MessageBox.Show(rm.GetString("defaultLocDel", culture));
								selected--;
							}
							else
							{
								// Find if exists Readers on this location, primary or secondary 
								readerArray = tmpReader.getReadersOnLocation(((LocationTO)item.Tag).LocationID);
						
								if (readerArray.Count > 0)
								{
									MessageBox.Show(item.Text + ": " + messageLocDel2);
									selected--;
								}
								else
								{
									// Check if some Visits belong to this location
									ArrayList visitList = new Visit().Search(-1, -1, "", "", "", "",
										new DateTime(0), new DateTime(0), -1, -1,
										"", ((LocationTO)item.Tag).LocationID, "");
									// If some Visits belong to this working unit, delete them first
									if (visitList.Count > 0)
									{
										MessageBox.Show(item.Text + ": " + rm.GetString("locHasVisits", culture));
										selected--;
									}
									else
									{
										// Find if exists Passes on this location
                                        Pass pass = new Pass();
                                        pass.PssTO.LocationID = ((LocationTO)item.Tag).LocationID;
										List<PassTO> passArray = pass.Search();
						
										if (passArray.Count > 0)
										{
											MessageBox.Show(item.Text + ": " + rm.GetString("locHasPasses", culture));
											selected--;
										}
										else
										{
											// Find if exists IO Pairs on this location
                                            IOPair iop = new IOPair();
                                            iop.PairTO.LocationID = ((LocationTO)item.Tag).LocationID;
                                            List<IOPairTO> ioPairArray = iop.Search();
						
											if (ioPairArray.Count > 0)
											{
												MessageBox.Show(item.Text + ": " + rm.GetString("locHasIOPairs", culture));
												selected--;
											}
											else
											{
												// Find all child Locations to this location
                                                Location loc = new Location();
                                                loc.LocTO.ParentLocationID = ((LocationTO)item.Tag).LocationID;
                                                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
												childLocArray = loc.Search();
						
												bool isDeleted = true;

												// If it is not a parent location, remove it
												if (childLocArray.Count == 0)
												{
													isDeleted = new Location().Delete(((LocationTO)item.Tag).LocationID) && isDeleted;
												}
												else if (childLocArray.Count == 1)
												{
													if (childLocArray[0].LocationID == childLocArray[0].ParentLocationID)
													{
														isDeleted = new Location().Delete(((LocationTO)item.Tag).LocationID) && isDeleted;											
													}
													else
													{
														MessageBox.Show(item.Text + ": " + messageLocDel3);
														selected--;
													}
												}
												else
												{
													MessageBox.Show(item.Text + ": " + messageLocDel3);
													selected--;
												}

												if((selected > 0) && isDeleted)
												{
													MessageBox.Show(rm.GetString("locDeleted", culture));
												}
												else if (!isDeleted)
												{
													MessageBox.Show(rm.GetString("locNotDeleted", culture));
												}
											}
										}
									}
								}
							}
						}

                        Location location = new Location();
                        location.LocTO.Status = Constants.DefaultStateActive.Trim();
                        List<LocationTO> locationList = location.Search();
						populateListView(locationList);
						tbName.Text = "";
						tbDescription.Text = "";
						currentLocation = new LocationTO();
						populateParentLocationCb();
						cbPrentLocation.SelectedValue = -1;

                        dsLocations = new Location().getLocations("");
                        populateDataTreeView();
						this.Invalidate();
					}
				}
				else
				{
					MessageBox.Show(messageLocDel4);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Locations.btnDelete_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				int parentLoc = -1;
				if (!cbPrentLocation.SelectedValue.Equals(-1))
				{
					parentLoc = (int)cbPrentLocation.SelectedValue;
				}

                Location loc = new Location();
                loc.LocTO.Name = tbName.Text.Trim();
                loc.LocTO.Description = tbDescription.Text.Trim();
                loc.LocTO.ParentLocationID = parentLoc;
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
				List<LocationTO> locationList = loc.Search();

				if (locationList.Count > 0)
				{
					populateListView(locationList);
				}
				else
				{
					MessageBox.Show(messageLocSearch1);
				}

				currentLocation = new LocationTO();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Locations.btnSearch_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		/// <summary>
		/// Populate combo box with possible parent locations.
		/// </summary>
        private void populateParentLocationCb()
        {
            try
            {
                Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
                List<LocationTO> parentLocArray = loc.Search();
                actualLocation = new List<LocationTO>();

                actualLocation.Insert(0, new LocationTO(-1, rm.GetString("all", culture), "", -1, -1, Constants.DefaultStateActive.ToString()));

                foreach (LocationTO locMember in parentLocArray)
                {
                    if (currentLocation.LocationID != 0)
                    {
                        if (locMember.ParentLocationID != currentLocation.LocationID)
                        {
                            actualLocation.Add(locMember);
                        }
                    }
                    else
                    {
                        actualLocation.Add(locMember);
                    }
                }

                cbPrentLocation.DataSource = actualLocation;
                cbPrentLocation.DisplayMember = "Name";
                cbPrentLocation.ValueMember = "LocationID";
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Locations.populateParentLocationCb(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

		/// <summary>
		/// Prepare search strings.
		/// </summary>
		/// <param name="forParsing"></param>
		/// <returns></returns>
        //private string parse(string forParsing)
        //{
        //    string parsedString = forParsing.Trim();
        //    if (parsedString.StartsWith("*"))
        //    {
        //        parsedString = parsedString.Substring(1);
        //        parsedString = "%" + parsedString;
        //    }

        //    if (parsedString.EndsWith("*"))
        //    {
        //        parsedString = parsedString.Substring(0, parsedString.Length - 1);
        //        parsedString = parsedString + "%";
        //    }

        //    return parsedString;
        //}

		private void Locations_Closed(object sender, System.EventArgs e)
		{
			currentLocation = new LocationTO();
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
                log.writeLog(DateTime.Now + " Locations.btnClose_Click(): " + ex.Message + "\n");
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
				LocationsAdd addForm = new LocationsAdd();
				addForm.ShowDialog(this);

                Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
				List<LocationTO> locationList = loc.Search();
				populateListView(locationList);
				tbName.Text = "";
				tbDescription.Text = "";
				currentLocation = new LocationTO();
				populateParentLocationCb();
				cbPrentLocation.SelectedValue = -1;

                dsLocations = new Location().getLocations("");
                populateDataTreeView();

				this.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Locations.btnAdd_Click(): " + ex.Message + "\n");
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
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.setVisibility(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

        private void Locations_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " Locations.Locations_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void dataTreeView1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

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
                            this.toolTip1.Show(theNode.ToolTipText, this, e.X + 50, e.Y + 100);
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
                log.writeLog(DateTime.Now + " Locations.dataTreeView1_MouseMove(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void lblRefreshToolTip_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this, "");

        }

        private void Locations_MouseMove(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this, "");
        }

        private void locationsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if ((dataTreeView1.SelectedNodes.Count >= 0) && (ModifierKeys != Keys.Shift) && (ModifierKeys != Keys.Control))
                {
                    TreeNode node = e.Node;

                    currentLocation = new LocationTO();
                    //  Location parentLocation = new Location();
                    if (node != null)
                    {
                        currentLocationTree = new Location().Find((int)node.Tag);
                        //  parentLocation = currentLocation.GetParentLocation();
                    }
                }
                else
                {
                    currentLocation = new LocationTO();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Locations.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAddTree_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Open Add Form
                LocationsAdd addForm = new LocationsAdd();
                addForm.ShowDialog(this);

                dsLocations = new Location().getLocations("");
                populateDataTreeView();

                Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
                List<LocationTO> locationList = loc.Search();
                populateListView(locationList);
                tbName.Text = "";
                tbDescription.Text = "";
                currentLocation = new LocationTO();
                populateParentLocationCb();
                cbPrentLocation.SelectedValue = -1;
                this.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Locations.btnAdd_Click(): " + ex.Message + "\n");
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
                    MessageBox.Show(messageLocUpd1);
                }
                else if (this.dataTreeView1.SelectedNodes.Count > 1)
                {
                    MessageBox.Show(messageLocUpd2);
                }
                else
                {
                    LocationsAdd locationAdd = new LocationsAdd(currentLocationTree);
                    locationAdd.ShowDialog(this);

                    dsLocations = new Location().getLocations("");
                    populateDataTreeView();

                    Location loc = new Location();
                    loc.LocTO.Status = Constants.DefaultStateActive.Trim();
                    List<LocationTO> locationList = loc.Search();
                    populateListView(locationList);
                    tbName.Text = "";
                    tbDescription.Text = "";
                    currentLocation = new LocationTO();
                    populateParentLocationCb();
                    cbPrentLocation.SelectedValue = -1;
                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Locations.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDeleteTree_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int selected = dataTreeView1.SelectedNodes.Count;
                if (dataTreeView1.SelectedNodes.Count > 0)
                {
                    // Check if this Location represent parent location to some other location
                    // if it is the case, ask user to remove those location first
                    List<LocationTO> childLocArray;
                    List<ReaderTO> readerArray = new List<ReaderTO>();
                    Reader tmpReader = new Reader();

                    DialogResult result = MessageBox.Show(messageLocDel1, "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        foreach (TreeNode node in dataTreeView1.SelectedNodes)
                        {
                            if (((int)node.Tag) == 0)
                            {
                                MessageBox.Show(rm.GetString("defaultLocDel",culture));
                                selected--;
                            }
                            else
                            {
                                // Find if exists Readers on this location, primary or secondary 
                                readerArray = tmpReader.getReadersOnLocation(Int32.Parse(node.Tag.ToString().Trim()));

                                if (readerArray.Count > 0)
                                {
                                    MessageBox.Show(node.Text + ": " + messageLocDel2);
                                    selected--;
                                }
                                else
                                {
                                    // Check if some Visits belong to this location
                                    ArrayList visitList = new Visit().Search(-1, -1, "", "", "", "",
                                        new DateTime(0), new DateTime(0), -1, -1,
                                        "", (int)node.Tag, "");
                                    // If some Visits belong to this working unit, delete them first
                                    if (visitList.Count > 0)
                                    {
                                        MessageBox.Show(node.Text + ": " + rm.GetString("locHasVisits",culture));
                                        selected--;
                                    }
                                    else
                                    {
                                        // Find if exists Passes on this location
                                        Pass pass = new Pass();
                                        pass.PssTO.LocationID = (int)node.Tag;
                                        List<PassTO> passArray = pass.Search();

                                        if (passArray.Count > 0)
                                        {
                                            MessageBox.Show(node.Text + ": " + rm.GetString("locHasPasses",culture));
                                            selected--;
                                        }
                                        else
                                        {
                                            // Find if exists IO Pairs on this location
                                            IOPair iop = new IOPair();
                                            iop.PairTO.LocationID = (int)node.Tag;
                                            List<IOPairTO> ioPairArray = iop.Search();

                                            if (ioPairArray.Count > 0)
                                            {
                                                MessageBox.Show(node.Text + ": " + rm.GetString("locHasIOPairs",culture));
                                                selected--;
                                            }
                                            else
                                            {
                                                // Find all child Locations to this location
                                                Location loc = new Location();
                                                loc.LocTO.ParentLocationID = (int)node.Tag;
                                                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
                                                childLocArray = loc.Search();

                                                bool isDeleted = true;

                                                // If it is not a parent location, remove it
                                                if (childLocArray.Count == 0)
                                                {
                                                    isDeleted = new Location().Delete((int)(node.Tag)) && isDeleted;
                                                }
                                                else if (childLocArray.Count == 1)
                                                {
                                                    if (childLocArray[0].LocationID == childLocArray[0].ParentLocationID)
                                                    {
                                                        isDeleted = new Location().Delete((int)(node.Tag)) && isDeleted;

                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show(node.Text + ": " + messageLocDel3);
                                                        selected--;
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show(node.Text + ": " + messageLocDel3);
                                                    selected--;
                                                }

                                                if ((selected > 0) && isDeleted)
                                                {
                                                    MessageBox.Show(rm.GetString("locDeleted",culture));
                                                }
                                                else if (!isDeleted)
                                                {
                                                    MessageBox.Show(rm.GetString("locNotDeleted",culture));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        dsLocations = new Location().getLocations("");
                        populateDataTreeView();

                        Location location = new Location();
                        location.LocTO.Status = Constants.DefaultStateActive.Trim();
                        List<LocationTO> locationList = location.Search();
                        populateListView(locationList);
                        tbName.Text = "";
                        tbDescription.Text = "";
                        currentLocation = new LocationTO();
                        populateParentLocationCb();
                        cbPrentLocation.SelectedValue = -1;
                        this.Invalidate();
                    }
                }
                else
                {
                    MessageBox.Show(messageLocDel4);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Locations.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnCloseTree_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Locations.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnLocationTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                LocationsTreeView locationsTreeView = new LocationsTreeView(actualLocation);
                locationsTreeView.ShowDialog();
                if (!locationsTreeView.selectedLocation.Equals(""))
                {
                    this.cbPrentLocation.SelectedIndex = cbPrentLocation.FindStringExact(locationsTreeView.selectedLocation);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Locations.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnMaps_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                MapObjects mapObjects = new MapObjects(Constants.locationObjectType);
                mapObjects.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Locations.btnMaps_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnMaps1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                MapObjects mapObjects = new MapObjects(Constants.locationObjectType);
                mapObjects.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Locations.btnMaps_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }        
	}
}
