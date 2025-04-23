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
using TransferObjects;
using Util;

namespace UI
{
	/// <summary>
	/// Summary description for ExitPermissions.
	/// </summary>
	public class ExitPermissions : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		ExitPermissionTO currentExitPermission = null;

		ApplUserTO logInUser;
		List<ApplRoleTO> currentRoles;
		Hashtable menuItemsPermissions;
		string menuItemID;

		bool readPermission = false;
		bool addPermission = false;
		bool updatePermission = false;
		bool deletePermission = false;

		private List<WorkingUnitTO> wUnits;
		private string wuString = "";

		DebugLog log;

		// List View indexes
		const int EmployeeIndex = 0;
		const int PassTypeIndex = 1;
		const int DateTimeIndex = 2;
		const int IssuedByIndex = 3;
		const int OffsetIndex = 4;
		const int UsedIndex = 5;
		const int DescriptionIndex = 6;
		const int VerifiedByIndex = 7;

		private ListViewItemComparer _comp;

		private CultureInfo culture;
		private System.Windows.Forms.GroupBox gbExitPerm;
		private System.Windows.Forms.ListView lvExitPerm;
		private System.Windows.Forms.ComboBox cbPassType;
		private System.Windows.Forms.Label lblPassType;
		private System.Windows.Forms.Label lblEmployee;
		private System.Windows.Forms.ComboBox cbEmployee;
		private System.Windows.Forms.ComboBox cbWU;
		private System.Windows.Forms.Label lblWU;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.Label lblIssuedBy;
		private System.Windows.Forms.ComboBox cbUsed;
		private System.Windows.Forms.DateTimePicker dtpTo;
		private System.Windows.Forms.DateTimePicker dtpFrom;
		private System.Windows.Forms.ComboBox cbIssuedBy;
		private System.Windows.Forms.Label lblTotal;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Button btnExitPermVer;
        private Button btnWUTree;
        private Button btnAdvanced;
        private CheckBox chbHierarhicly;
		ResourceManager rm;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;

        //MDI child properties
        List<EmployeeTO> currentEmplArray = new List<EmployeeTO>();
        private Button btnReport;

        private Filter filter;

        Dictionary<string, string> verifiedByUsers = new Dictionary<string, string>();

		public ExitPermissions()
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentExitPermission = new ExitPermissionTO();
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(ExitPermissions).Assembly);
			setLanguage();
        }

        #region MDI child method's       
        public void MDIchangeSelectedEmployee(int selectedWU, int selectedEmployeeID, DateTime from, DateTime to, bool check)
        {
            try
            {
                dtpFrom.Value = from;
                dtpTo.Value = to;
                chbHierarhicly.Checked = check;
                foreach (WorkingUnitTO wu in wUnits)
                {
                    if (wu.WorkingUnitID == selectedWU)
                        cbWU.SelectedValue = selectedWU;
                }

                foreach (EmployeeTO empl in currentEmplArray)
                {
                    if (empl.EmployeeID == selectedEmployeeID)
                        cbEmployee.SelectedValue = selectedEmployeeID;
                }

                btnSearch_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissions.changeSelectedEmployee(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExitPermissions));
            this.gbExitPerm = new System.Windows.Forms.GroupBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbUsed = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cbIssuedBy = new System.Windows.Forms.ComboBox();
            this.lblIssuedBy = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.cbPassType = new System.Windows.Forms.ComboBox();
            this.lblPassType = new System.Windows.Forms.Label();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.lblWU = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lvExitPerm = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnExitPermVer = new System.Windows.Forms.Button();
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.btnReport = new System.Windows.Forms.Button();
            this.gbExitPerm.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbExitPerm
            // 
            this.gbExitPerm.Controls.Add(this.chbHierarhicly);
            this.gbExitPerm.Controls.Add(this.btnWUTree);
            this.gbExitPerm.Controls.Add(this.cbUsed);
            this.gbExitPerm.Controls.Add(this.lblStatus);
            this.gbExitPerm.Controls.Add(this.cbIssuedBy);
            this.gbExitPerm.Controls.Add(this.lblIssuedBy);
            this.gbExitPerm.Controls.Add(this.dtpTo);
            this.gbExitPerm.Controls.Add(this.lblTo);
            this.gbExitPerm.Controls.Add(this.dtpFrom);
            this.gbExitPerm.Controls.Add(this.lblFrom);
            this.gbExitPerm.Controls.Add(this.cbPassType);
            this.gbExitPerm.Controls.Add(this.lblPassType);
            this.gbExitPerm.Controls.Add(this.lblEmployee);
            this.gbExitPerm.Controls.Add(this.cbEmployee);
            this.gbExitPerm.Controls.Add(this.cbWU);
            this.gbExitPerm.Controls.Add(this.lblWU);
            this.gbExitPerm.Controls.Add(this.btnSearch);
            this.gbExitPerm.Location = new System.Drawing.Point(16, 16);
            this.gbExitPerm.Name = "gbExitPerm";
            this.gbExitPerm.Size = new System.Drawing.Size(608, 177);
            this.gbExitPerm.TabIndex = 0;
            this.gbExitPerm.TabStop = false;
            this.gbExitPerm.Tag = "FILTERABLE";
            this.gbExitPerm.Text = "Gates";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(120, 43);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 44;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(302, 14);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 40;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbUsed
            // 
            this.cbUsed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUsed.Location = new System.Drawing.Point(416, 48);
            this.cbUsed.Name = "cbUsed";
            this.cbUsed.Size = new System.Drawing.Size(176, 21);
            this.cbUsed.TabIndex = 11;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(304, 48);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(104, 23);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "Status:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbIssuedBy
            // 
            this.cbIssuedBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIssuedBy.Location = new System.Drawing.Point(416, 80);
            this.cbIssuedBy.Name = "cbIssuedBy";
            this.cbIssuedBy.Size = new System.Drawing.Size(176, 21);
            this.cbIssuedBy.TabIndex = 13;
            // 
            // lblIssuedBy
            // 
            this.lblIssuedBy.Location = new System.Drawing.Point(304, 80);
            this.lblIssuedBy.Name = "lblIssuedBy";
            this.lblIssuedBy.Size = new System.Drawing.Size(104, 23);
            this.lblIssuedBy.TabIndex = 12;
            this.lblIssuedBy.Text = "Issued By:";
            this.lblIssuedBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(120, 144);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(176, 20);
            this.dtpTo.TabIndex = 7;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(8, 141);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(104, 23);
            this.lblTo.TabIndex = 6;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy ";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(120, 112);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(176, 20);
            this.dtpFrom.TabIndex = 5;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(8, 111);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(104, 23);
            this.lblFrom.TabIndex = 4;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPassType
            // 
            this.cbPassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPassType.Location = new System.Drawing.Point(120, 80);
            this.cbPassType.Name = "cbPassType";
            this.cbPassType.Size = new System.Drawing.Size(176, 21);
            this.cbPassType.TabIndex = 3;
            // 
            // lblPassType
            // 
            this.lblPassType.Location = new System.Drawing.Point(8, 80);
            this.lblPassType.Name = "lblPassType";
            this.lblPassType.Size = new System.Drawing.Size(104, 23);
            this.lblPassType.TabIndex = 2;
            this.lblPassType.Text = "Pass Type:";
            this.lblPassType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(336, 16);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(72, 23);
            this.lblEmployee.TabIndex = 8;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmployee
            // 
            this.cbEmployee.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmployee.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmployee.Location = new System.Drawing.Point(416, 16);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(176, 21);
            this.cbEmployee.TabIndex = 9;
            this.cbEmployee.Leave += new EventHandler(cbEmployee_Leave);
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.Location = new System.Drawing.Point(120, 16);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(176, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(8, 16);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(104, 23);
            this.lblWU.TabIndex = 0;
            this.lblWU.Text = "Working Unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(517, 141);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lvExitPerm
            // 
            this.lvExitPerm.FullRowSelect = true;
            this.lvExitPerm.GridLines = true;
            this.lvExitPerm.HideSelection = false;
            this.lvExitPerm.Location = new System.Drawing.Point(16, 211);
            this.lvExitPerm.Name = "lvExitPerm";
            this.lvExitPerm.Size = new System.Drawing.Size(910, 200);
            this.lvExitPerm.TabIndex = 15;
            this.lvExitPerm.UseCompatibleStateImageBehavior = false;
            this.lvExitPerm.View = System.Windows.Forms.View.Details;
            this.lvExitPerm.SelectedIndexChanged += new System.EventHandler(this.lvExitPerm_SelectedIndexChanged);
            this.lvExitPerm.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvExitPerm_ColumnClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 433);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(97, 433);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 17;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(178, 433);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 18;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(854, 433);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(774, 414);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(152, 16);
            this.lblTotal.TabIndex = 16;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnExitPermVer
            // 
            this.btnExitPermVer.Location = new System.Drawing.Point(678, 433);
            this.btnExitPermVer.Name = "btnExitPermVer";
            this.btnExitPermVer.Size = new System.Drawing.Size(170, 23);
            this.btnExitPermVer.TabIndex = 19;
            this.btnExitPermVer.Text = "Exit permissions verification >>";
            this.btnExitPermVer.Click += new System.EventHandler(this.btnExitPermVer_Click);
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.Location = new System.Drawing.Point(419, 433);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(187, 23);
            this.btnAdvanced.TabIndex = 21;
            this.btnAdvanced.Text = "Advanced";
            this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(630, 19);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 100);
            this.gbFilter.TabIndex = 25;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(32, 56);
            this.btnSaveCriteria.Name = "btnSaveCriteria";
            this.btnSaveCriteria.Size = new System.Drawing.Size(82, 23);
            this.btnSaveCriteria.TabIndex = 16;
            this.btnSaveCriteria.Text = "Save criteria";
            this.btnSaveCriteria.UseVisualStyleBackColor = true;
            this.btnSaveCriteria.Click += new System.EventHandler(this.btnSaveCriteria_Click);
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(6, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(337, 433);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(76, 23);
            this.btnReport.TabIndex = 26;
            this.btnReport.Text = "Report";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // ExitPermissions
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(942, 468);
            this.ControlBox = false;
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.btnAdvanced);
            this.Controls.Add(this.btnExitPermVer);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvExitPerm);
            this.Controls.Add(this.gbExitPerm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "ExitPermissions";
            this.ShowInTaskbar = false;
            this.Text = "Gates";
            this.Load += new System.EventHandler(this.ExitPermissions_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ExitPermissions_KeyUp);
            this.gbExitPerm.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        void cbEmployee_Leave(object sender, EventArgs e)
        {
            if (cbEmployee.SelectedIndex == -1) {
                cbEmployee.SelectedIndex = 0;
            }
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
					case ExitPermissions.EmployeeIndex:
					case ExitPermissions.PassTypeIndex:
					case ExitPermissions.IssuedByIndex:
					case ExitPermissions.UsedIndex:
					case ExitPermissions.DescriptionIndex:
					case ExitPermissions.VerifiedByIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
					}
					case ExitPermissions.OffsetIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(Int32.Parse(sub1.Text), 
							Int32.Parse(sub2.Text));
					}
					case ExitPermissions.DateTimeIndex:
					{
						DateTime dt1 = new DateTime(1,1,1,0,0,0);
						DateTime dt2 = new DateTime(1,1,1,0,0,0);

						if (!sub1.Text.Trim().Equals("")) 
						{
                            dt1 = DateTime.ParseExact(sub1.Text.Trim(), "dd.MM.yyyy   HH:mm", null);
						}

						if (!sub2.Text.Trim().Equals(""))
						{
                            dt2 = DateTime.ParseExact(sub2.Text.Trim(), "dd.MM.yyyy   HH:mm", null);
						}
						
						return CaseInsensitiveComparer.Default.Compare(dt1, dt2);
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
				this.Text = rm.GetString("ExitPermissionsForm", culture);

				// group box text
				gbExitPerm.Text = rm.GetString("gbExitPerm", culture);
                this.gbFilter.Text = rm.GetString("gbFilter", culture);
                
                //check box's text
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);

				// button's text
				btnAdd.Text = rm.GetString("btnAdd", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnDelete.Text = rm.GetString("btnDelete", culture);
				btnSearch.Text = rm.GetString("btnSearch", culture);
				btnClose.Text = rm.GetString("btnClose", culture);
				btnExitPermVer.Text = rm.GetString("btnExitPermVer", culture);
                btnAdvanced.Text = rm.GetString("ExitPermAddMultiForm", culture);
                btnReport.Text = rm.GetString("btnReport", culture);

				// label's text
				lblWU.Text = rm.GetString("lblWU", culture);
				lblEmployee.Text = rm.GetString("lblEmployee", culture);
				lblPassType.Text = rm.GetString("lblPassType", culture);
				lblFrom.Text = rm.GetString("lblFrom", culture);
				lblTo.Text = rm.GetString("lblTo", culture);
				//lblUsed.Text = rm.GetString("lblUsed", culture);
				lblStatus.Text = rm.GetString("lblStatus", culture);
				lblIssuedBy.Text = rm.GetString("lblIssuedBy", culture);				

				// list view
				lvExitPerm.BeginUpdate();
				lvExitPerm.Columns.Add(rm.GetString("hdrEmployee", culture), 2 * (lvExitPerm.Width - 18)/ 15,
					HorizontalAlignment.Left);
				lvExitPerm.Columns.Add(rm.GetString("hdrPassType", culture), 2 * (lvExitPerm.Width - 17) / 15,
					HorizontalAlignment.Left);
				lvExitPerm.Columns.Add(rm.GetString("hdrDateTime", culture), 2 * (lvExitPerm.Width - 18) / 15,
					HorizontalAlignment.Left);
				lvExitPerm.Columns.Add(rm.GetString("hdrIssuedBy", culture), 2 * (lvExitPerm.Width - 17) / 15,
					HorizontalAlignment.Left);
				lvExitPerm.Columns.Add(rm.GetString("hdrOffset", culture), (lvExitPerm.Width - 18) / 15,
					HorizontalAlignment.Left);
				//lvExitPerm.Columns.Add(rm.GetString("hdrUsed", culture), (lvExitPerm.Width - 7) / 7,
				//	HorizontalAlignment.Left);
				lvExitPerm.Columns.Add(rm.GetString("hdrStatus", culture), (lvExitPerm.Width - 17) / 15,
					HorizontalAlignment.Left);
				lvExitPerm.Columns.Add(rm.GetString("hdrDescription", culture), 3 * (lvExitPerm.Width - 18) / 15,
					HorizontalAlignment.Left);
				lvExitPerm.Columns.Add(rm.GetString("hdrVerifiedBy", culture), 2 * (lvExitPerm.Width - 17) / 15,
					HorizontalAlignment.Left);

				lvExitPerm.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissions.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate Working Unit Combo Box
		/// </summary>
		private void populateWorkingUnitCombo()
		{
			try
			{
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    wuArray.Add(wuTO);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

				cbWU.DataSource = wuArray;
				cbWU.DisplayMember = "Name";
				cbWU.ValueMember = "WorkingUnitID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissions.populateWorkingUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate Employee Combo Box
		/// </summary>
		private void populateEmployeeCombo(int wuID)
		{
			try
			{
				currentEmplArray = new List<EmployeeTO>();

                string workUnitID = wuID.ToString();
                if (wuID == -1)
                {
                    currentEmplArray = new Employee().SearchByWU(wuString);
                }
                else
                {  
                    if (chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWU.SelectedValue)
                            {
                                wuList.Add(workingUnit);
                                workUnit.WUTO = workingUnit;
                            }
                        }
                        if (workUnit.WUTO.ChildWUNumber > 0)
                            wuList = workUnit.FindAllChildren(wuList);
                        workUnitID = "";
                        foreach (WorkingUnitTO wunit in wuList)
                        {
                            workUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (workUnitID.Length > 0)
                        {
                            workUnitID = workUnitID.Substring(0, workUnitID.Length - 1);
                        }
                    }
                    currentEmplArray = new Employee().SearchByWU(workUnitID);
                }

				foreach(EmployeeTO employee in currentEmplArray)
				{
					employee.LastName += " " + employee.FirstName;
				}

				EmployeeTO empl = new EmployeeTO();
				empl.LastName = rm.GetString("all", culture);
				currentEmplArray.Insert(0, empl);
								
				cbEmployee.DataSource = currentEmplArray;
				cbEmployee.DisplayMember = "LastName";
				cbEmployee.ValueMember = "EmployeeID";
				cbEmployee.Invalidate();
				cbEmployee.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissions.populateEmployeeCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populatePassTypeCombo()
		{
			try
			{
                PassType pType = new PassType();
                pType.PTypeTO.IsPass = Constants.passOnReader;
				List<PassTypeTO> ptArray = pType.Search();
				List<PassTypeTO> passTypes = new List<PassTypeTO>();

				foreach (PassTypeTO pt in ptArray)
				{
					if (pt.PassTypeID != 0)
					{
						passTypes.Add(pt);
					}
				}

				passTypes.Insert(0, new PassTypeTO(-1, rm.GetString("all", culture), 0, 0, ""));

				cbPassType.DataSource = passTypes;
				cbPassType.DisplayMember = "Description";
				cbPassType.ValueMember = "PassTypeID";
				cbPassType.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissions.populatePassTypeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		private void populateUsedCombo()
		{
			try
			{
				ArrayList usedList = new ArrayList();
				/*usedList.Add(rm.GetString("all", culture));
				usedList.Add(rm.GetString("no", culture));
				usedList.Add(rm.GetString("yes", culture));*/

				//Permission verification - new state, unverified added
				usedList.Add(rm.GetString("all", culture));
				usedList.Add(rm.GetString("unverified", culture));
				usedList.Add(rm.GetString("not_used", culture));
				usedList.Add(rm.GetString("used", culture));

				cbUsed.DataSource = usedList;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissions.populateUsedCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		private void populateIssuedCombo()
		{
			try
			{
				List<ApplUserTO> userArray = new ApplUser().Search();

				userArray.Insert(0, new ApplUserTO(rm.GetString("all", culture), "", rm.GetString("all", culture),
					"", -1, "", -1, ""));

				cbIssuedBy.DataSource = userArray;
				cbIssuedBy.DisplayMember = "Name";
				cbIssuedBy.ValueMember = "UserID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissions.populateIssuedCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate List View with ExitPermissions found
		/// </summary>
		/// <param name="ExitPermissionsList"></param>
        public void populateListView(List<ExitPermissionTO> ExitPermissionsList)
		{
			try
			{
				lvExitPerm.BeginUpdate();
				lvExitPerm.Items.Clear();

				CultureInfo ci = CultureInfo.InvariantCulture;

				if (ExitPermissionsList.Count > 0)
				{
					foreach(ExitPermissionTO exitPerm in ExitPermissionsList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = exitPerm.EmployeeName.Trim();
						item.SubItems.Add(exitPerm.PassTypeDesc.Trim());
						if (!exitPerm.StartTime.Date.Equals(new DateTime(1,1,1,0,0,0)))
						{
							item.SubItems.Add(exitPerm.StartTime.ToString("dd.MM.yyyy   HH:mm", ci));
						}
						else
						{								
							item.SubItems.Add("");
						}
						item.SubItems.Add(exitPerm.UserName.ToString().Trim());
						item.SubItems.Add(exitPerm.Offset.ToString().Trim());
						if (exitPerm.Used == (int) Constants.Used.No)
						{
							//item.SubItems.Add(rm.GetString("no", culture));
							item.SubItems.Add(rm.GetString("not_used", culture));
						}
						else if (exitPerm.Used == (int) Constants.Used.Unverified)
						{
							item.SubItems.Add(rm.GetString("unverified", culture));
						}
						else
						{
							//item.SubItems.Add(rm.GetString("yes", culture));
							item.SubItems.Add(rm.GetString("used", culture));
						}
						item.SubItems.Add(exitPerm.Description.ToString().Trim().Replace('\r',' ').Replace('\n',' '));

                        //string verifiedByName = "";
                        //foreach(ApplUserTO applUser in verifiedByUsersList)
                        //{
                        //    if (applUser.UserID == exitPerm.VerifiedBy)
                        //    {
                        //        verifiedByName = applUser.Name;
                        //        break;
                        //    }
                        //}
                        if (verifiedByUsers.ContainsKey(exitPerm.VerifiedBy))
                            item.SubItems.Add(verifiedByUsers[exitPerm.VerifiedBy]);
                        else
                            item.SubItems.Add("");

						item.Tag = exitPerm;

						lvExitPerm.Items.Add(item);
					}
				}

				lvExitPerm.EndUpdate();
				lvExitPerm.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissions.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
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

		private void btnClose_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissions.btnClose_Click(): " + ex.Message + "\n");
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
				ExitPermissionsAdd addForm = new ExitPermissionsAdd();
				addForm.ShowDialog(this);

				//Reload form only if some permission is added
				if (addForm.doReloadOnBack)
					searchAllExitPermissions(sender, e);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissions.btnAdd_Click(): " + ex.Message + "\n");
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
				btnAdd.Enabled = addPermission;
				btnUpdate.Enabled = updatePermission;
				btnDelete.Enabled = deletePermission;
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissions.setVisibility(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void cbWU_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
                bool check = false;
                foreach (WorkingUnitTO wu in wUnits)
                {
                    if (cbWU.SelectedIndex != 0)
                    {
                        if (wu.WorkingUnitID == (int)cbWU.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                        {
                            chbHierarhicly.Checked = true;
                            check = true;
                        }
                    }
                }

                if (cbWU.SelectedValue is int && !check)
                {
                    populateEmployeeCombo((int)cbWU.SelectedValue);
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsAdd.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvExitPerm_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				SortOrder prevOrder = lvExitPerm.Sorting;
				lvExitPerm.Sorting = SortOrder.None;

				if (e.Column == _comp.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvExitPerm.Sorting = SortOrder.Descending;
					}
					else
					{
						lvExitPerm.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp.SortColumn = e.Column;
					lvExitPerm.Sorting = SortOrder.Ascending;
				}
                lvExitPerm.ListViewItemSorter = _comp;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissions.lvExitPerm_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvExitPerm_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				if(lvExitPerm.SelectedItems.Count == 1)
				{
					currentExitPermission = (ExitPermissionTO)lvExitPerm.SelectedItems[0].Tag;

                    //cbEmployee.SelectedValue = currentExitPermission.EmployeeID;
                    //cbPassType.SelectedValue = currentExitPermission.PasTypeID;
                    ////cbUsed.SelectedIndex = currentExitPermission.Used + 1;
                    //cbUsed.SelectedIndex = ((currentExitPermission.Used + 2) == 5) ? 1 : (currentExitPermission.Used + 2);
                    //cbIssuedBy.SelectedValue = currentExitPermission.IssuedBy;
                    //dtpFrom.Value = currentExitPermission.StartTime;
                    //dtpTo.Value = currentExitPermission.StartTime;
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissions.lvExitPerm_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void clearListView()
		{
			lvExitPerm.BeginUpdate();
			lvExitPerm.Items.Clear();
			lvExitPerm.EndUpdate();
			lvExitPerm.Invalidate();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				int employeeID = -1;
				int passTypeID = -1;
				int used = -1;
				string issuedBy = "";

				if (cbEmployee.SelectedIndex >= 0 && (int) cbEmployee.SelectedValue >= 0)
				{
					employeeID = (int)cbEmployee.SelectedValue;
				}
				if (cbPassType.SelectedIndex > 0)
				{
					passTypeID = (int)cbPassType.SelectedValue;
				}
				/*if (cbUsed.SelectedItem.ToString().Equals(rm.GetString("yes", culture)))
				{
					used = ((int) Constants.Used.Yes).ToString();
				}
				else if (cbUsed.SelectedItem.ToString().Equals(rm.GetString("no", culture)))
				{
					used = ((int) Constants.Used.No).ToString();
				}*/

				//Permission verification - new state, unverified added
				if (cbUsed.SelectedItem.ToString().Equals(rm.GetString("used", culture)))
				{
					used = (int) Constants.Used.Yes;
				}
				else if (cbUsed.SelectedItem.ToString().Equals(rm.GetString("not_used", culture)))
				{
					used = (int) Constants.Used.No;
				}
				else if (cbUsed.SelectedItem.ToString().Equals(rm.GetString("unverified", culture)))
				{
					used = (int) Constants.Used.Unverified;
				}
				if (cbIssuedBy.SelectedIndex > 0)
				{
					issuedBy = cbIssuedBy.SelectedValue.ToString();
				}
                string selectedWU = wuString;
                if (cbWU.SelectedIndex >= 0 && (int)cbWU.SelectedValue >= 0)
                {
                    selectedWU = cbWU.SelectedValue.ToString();

                    //10.12.2008. Natasa - if Hierarhicly is checked show employees form child working unit's
                    WorkingUnit wu = new WorkingUnit();
                    if ((int)this.cbWU.SelectedValue != -1 && chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWU.SelectedValue)
                            {
                                wuList.Add(workingUnit);
                                workUnit.WUTO = workingUnit;
                            }
                        }
                        if (workUnit.WUTO.ChildWUNumber > 0)
                            wuList = wu.FindAllChildren(wuList);
                        selectedWU = "";
                        foreach (WorkingUnitTO wunit in wuList)
                        {
                            selectedWU += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (selectedWU.Length > 0)
                        {
                            selectedWU = selectedWU.Substring(0, selectedWU.Length - 1);
                        }
                    }
                }

				if (!wuString.Equals(""))
				{
					if (cbEmployee.Items.Count == 0)
					{
						MessageBox.Show(rm.GetString("noExitPermFound", culture));
						return;
					}

					CultureInfo ci = CultureInfo.InvariantCulture;

					//ArrayList exitPermissionsList = currentExitPermission.Search("", employeeID, passTypeID, 
					//	dtpFrom.Value.Date.ToString("MM/dd/yyy", ci), dtpTo.Value.Date.ToString("MM/dd/yyy", ci), 
					//	"", used, "", issuedBy, "", wuString);
					DateTime dateTO = dtpTo.Value.Date.Add(new TimeSpan(23, 59, 59));
                    ExitPermission perm = new ExitPermission();
                    perm.PermTO.EmployeeID = employeeID;
                    perm.PermTO.PassTypeID = passTypeID;
                    perm.PermTO.Used = used;
                    perm.PermTO.IssuedBy = issuedBy;
                    List<ExitPermissionTO> exitPermissionsList = perm.SearchVerifiedBy(dtpFrom.Value.Date, dateTO, selectedWU);
                   
					if (exitPermissionsList.Count > 0)
					{						
						populateListView(exitPermissionsList);
						this.lblTotal.Visible=true;
						this.lblTotal.Text = rm.GetString("lblTotal", culture) + exitPermissionsList.Count.ToString().Trim();
					}
					else
					{
						//MessageBox.Show(rm.GetString("noExitPermFound", culture));
						clearListView();
						this.lblTotal.Visible = true;
						this.lblTotal.Text = rm.GetString("lblTotal", culture) + "0";
					}
				}
				else
				{
					MessageBox.Show(rm.GetString("noEmplPermPrivilege", culture));
				}
				
				currentExitPermission = new ExitPermissionTO();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Gates.btnSearch_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				if (lvExitPerm.SelectedItems.Count <= 0)
				{
					MessageBox.Show(rm.GetString("noSelExitPermDel", culture));
				}
				else
				{
					DialogResult result = MessageBox.Show(rm.GetString("deleteExitPerm", culture), "", MessageBoxButtons.YesNo);

					if (result == DialogResult.Yes)
					{
						bool isDeleted = true;

						int used = 0;
						foreach(ListViewItem item in lvExitPerm.SelectedItems)
						{
							ExitPermissionTO permTO = (ExitPermissionTO)item.Tag;
							if (permTO.Used == (int) Constants.Used.Yes)
							{
								used++;
							}
							else
							{
								isDeleted = new ExitPermission().Delete(permTO.PermissionID, permTO.StartTime) && isDeleted;
							}
						}

						if (used != 0)
						{
							MessageBox.Show(rm.GetString("usedPermissionsDelete", culture));
						}
						if (isDeleted)
						{
							MessageBox.Show(rm.GetString("ExitPermDel", culture));
						}
						else
						{
							MessageBox.Show(rm.GetString("noExitPermDel", culture));
						}

						searchAllExitPermissions(sender, e);
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissions.btnDelete_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void searchAllExitPermissions(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                cbWU.SelectedIndex = 0;
                cbEmployee.SelectedIndex = 0;
                cbPassType.SelectedIndex = 0;
                cbUsed.SelectedIndex = 0;
                cbIssuedBy.SelectedIndex = 0;
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
                currentExitPermission = new ExitPermissionTO();
                this.lblTotal.Visible = false;

                btnSearch_Click(sender, e);
                /*
                if (!wuString.Equals(""))
                {
                    //ArrayList exitPermissionsList = currentExitPermission.Search("", "", "", "", "", "", "", "", "", "", wuString);
                    ArrayList exitPermissionsList = currentExitPermission.SearchVerifiedBy("", "", "", "", "", "", "", "", "", "", wuString);
                    ArrayList verifiedByUsers = (new ApplUser()).SearchVerifiedByWUnits(wuString);
                    populateListView(exitPermissionsList, verifiedByUsers);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noEmplPermPrivilege", culture));
                }*/

                this.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissions.searchAllExitPermissions(): " + ex.Message + "\n");
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
				if (lvExitPerm.SelectedItems.Count != 1)
				{
					MessageBox.Show(rm.GetString("selExitPermUpd", culture));
				}
				else
				{
					ExitPermissionTO exitPermTO = new ExitPermissionTO();
                    ExitPermission perm = new ExitPermission();
                    perm.PermTO.PermissionID = ((ExitPermissionTO)lvExitPerm.SelectedItems[0].Tag).PermissionID;
                    List<ExitPermissionTO> selectedPerm = perm.SearchVerifiedBy(new DateTime(0), new DateTime(0), "");
					if (selectedPerm.Count == 1)
					{
						exitPermTO = selectedPerm[0];
					}
					//ExitPermissionTO exitPermTO = currentExitPermission.Find((int) lvExitPerm.SelectedItems[0].Tag);

					if (exitPermTO.Used == (int) Constants.Used.Yes)
					{
						//MessageBox.Show(rm.GetString("usedExitPermUpd", culture));
                        currentExitPermission = exitPermTO;
                        ExitPermissionsAdd addForm = new ExitPermissionsAdd(currentExitPermission);
                        addForm.ShowDialog(this);

                        //Reload form only if some permission is updated
                        if (addForm.doReloadOnBack)
                            searchAllExitPermissions(sender, e);
					}
					else
					{
						// Open Update Form
						//currentExitPermission.ReceiveTransferObject(exitPermTO);
						currentExitPermission = exitPermTO;
						ExitPermissionsAdd addForm = new ExitPermissionsAdd(currentExitPermission);
						addForm.ShowDialog(this);
				
						//Reload form only if some permission is updated
						if (addForm.doReloadOnBack)
							searchAllExitPermissions(sender, e);
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissions.btnAdd_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

		private void btnExitPermVer_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				// Open ExitPermissionsVerification Form
				ExitPermissionsVerification exitPermissionsVerificationForm = new ExitPermissionsVerification();
				exitPermissionsVerificationForm.ShowDialog(this);

				searchAllExitPermissions(sender, e);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissions.btnExitPermVer_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void ExitPermissions_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " ExitPermissions.ExitPermissions_KeyUp(): " + ex.Message + "\n");
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
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuString);
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbWU.SelectedIndex = cbWU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissions.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ExitPermissionsAddAdvanced epa = new ExitPermissionsAddAdvanced();
                epa.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissions.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                if (cbWU.SelectedValue is int)
                {
                    if ((int)cbWU.SelectedValue >= 0)
                        populateEmployeeCombo((int)cbWU.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissions.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void ExitPermissions_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                //btnAdvanced.Visible = false;
                _comp = new ListViewItemComparer(lvExitPerm);
                lvExitPerm.ListViewItemSorter = _comp;
                lvExitPerm.Sorting = SortOrder.Ascending;
                this.lblTotal.Visible = false;

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.PermissionPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                List<ApplUserTO> verifiedList = new ApplUser().SearchVerifiedByWUnits(wuString);

                foreach (ApplUserTO userTO in verifiedList)
                {
                    if (!verifiedByUsers.ContainsKey(userTO.UserID))
                        verifiedByUsers.Add(userTO.UserID, userTO.Name);
                }

                populateWorkingUnitCombo();
                populateEmployeeCombo(-1);
                populatePassTypeCombo();
                populateUsedCombo();
                populateIssuedCombo();

                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;

                btnSearch_Click(this, new EventArgs());
                /*if (!wuString.Equals(""))
                {
                    //ArrayList exitPermissionsList = currentExitPermission.Search("", "", "", "", "", "", "", "", "", "", wuString);
                    ArrayList exitPermissionsList = currentExitPermission.SearchVerifiedBy("", "", "", "", "", "", "", "", "", "", wuString);
                    ArrayList verifiedByUsers = (new ApplUser()).SearchVerifiedByWUnits(wuString);
                    populateListView(exitPermissionsList, verifiedByUsers);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noEmplPermPrivilege", culture));
                }*/

                if (logInUser.ExitPermVerification != (int)Constants.PermVerification.Yes)
                    btnExitPermVer.Enabled = false;

                filter = new Filter();
                filter.SerachButton = this.btnSearch;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissions.ExitPermissions_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbFilter.SelectedIndex == 0)
                {
                    this.btnSaveCriteria.Text = rm.GetString("SaveFilter", culture);
                }
                else
                {
                    this.btnSaveCriteria.Text = rm.GetString("UpdateFilter", culture);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissions.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSaveCriteria_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                UIFeatures.FilterAdd filterAdd = new UIFeatures.FilterAdd(this, filter);
                filterAdd.ShowDialog();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissions.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvExitPerm.Items.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("exit_permisions");
                    DataTable tableI = new DataTable("images");

                    tableCR.Columns.Add("employee", typeof(System.String));
                    tableCR.Columns.Add("pass_type", typeof(System.String));
                    tableCR.Columns.Add("date_time", typeof(System.String));
                    tableCR.Columns.Add("isued", typeof(System.String));
                    tableCR.Columns.Add("tolerance", typeof(System.String));
                    tableCR.Columns.Add("state", typeof(System.String));
                    tableCR.Columns.Add("desc", typeof(System.String));
                    tableCR.Columns.Add("verified_by", typeof(System.String));
                    tableCR.Columns.Add("imageID", typeof(byte));

                    tableI.Columns.Add("imageID", typeof(byte));
                    tableI.Columns.Add("image", typeof(System.Byte[]));

                    //add logo image just once
                    DataRow rowI = tableI.NewRow();
                    rowI["image"] = Constants.LogoForReport;
                    rowI["imageID"] = 1;
                    tableI.Rows.Add(rowI);
                    tableI.AcceptChanges();

                    dataSetCR.Tables.Add(tableCR);
                    dataSetCR.Tables.Add(tableI);

                    foreach (ListViewItem item in lvExitPerm.Items)
                    {
                        DataRow row = tableCR.NewRow();

                        row["employee"] = item.Text.ToString().Trim();
                        row["pass_type"] = item.SubItems[1].Text.ToString().Trim();
                        row["date_time"] = item.SubItems[2].Text.ToString().Trim();
                        row["isued"] = item.SubItems[3].Text.ToString().Trim();
                        row["tolerance"] = item.SubItems[4].Text.ToString().Trim();
                        row["state"] = item.SubItems[5].Text.ToString().Trim();
                        row["desc"] = item.SubItems[6].Text.ToString().Trim();
                        row["verified_by"] = item.SubItems[7].Text.ToString().Trim();

                        row["imageID"] = 1;

                        tableCR.Rows.Add(row);
                        tableCR.AcceptChanges();
                    }

                    if (tableCR.Rows.Count == 0)
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }

                    string selWorkingUnit = "*";
                    string selEmployee = "*";
                    string selPassType = "*";
                    string selState = "*";
                    string selIsued = "*";

                    if (cbWU.SelectedIndex >= 0 && (int)cbWU.SelectedValue >= 0)
                        selWorkingUnit = cbWU.Text;
                    if (cbEmployee.SelectedIndex >= 0 && (int)cbEmployee.SelectedValue >= 0)
                        selEmployee = cbEmployee.Text;
                    if (cbPassType.SelectedIndex >= 0 && (int)cbPassType.SelectedValue >= 0)
                        selPassType = cbPassType.Text;
                    if (cbUsed.SelectedIndex >= 0 && !((string)cbUsed.SelectedValue).Equals("*"))
                        selState = cbUsed.Text;
                    if (cbIssuedBy.SelectedIndex >= 0 && !((string)cbIssuedBy.SelectedValue).Equals("*"))
                        selIsued = cbIssuedBy.Text;

                    if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                    {
                        Reports.Reports_sr.ExitPermisionsCRView view = new Reports.Reports_sr.ExitPermisionsCRView(dataSetCR,
                             selWorkingUnit, selEmployee, selPassType, selState, selIsued, dtpFrom.Value, dtpTo.Value);
                        view.ShowDialog(this);
                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                    {
                        Reports.Reports_en.ExitPermisionsCRView_en view = new Reports.Reports_en.ExitPermisionsCRView_en(dataSetCR,
                             selWorkingUnit, selEmployee, selPassType, selState, selIsued, dtpFrom.Value, dtpTo.Value);
                        view.ShowDialog(this);
                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                    {
                        Reports.Reports_fi.ExitPermisionsCRView_fi view = new Reports.Reports_fi.ExitPermisionsCRView_fi(dataSetCR,
                             selWorkingUnit, selEmployee, selPassType, selState, selIsued, dtpFrom.Value, dtpTo.Value);
                        view.ShowDialog(this);
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissions.btnReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
