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

using TransferObjects;
using Common;
using Util;
using Reports;

namespace UI
{
	/// <summary>
	/// Summary description for IOPairs.
	/// </summary>
	public class IOPairs : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbIOPairs;
		private System.Windows.Forms.ComboBox cbShowPairs;
		private System.Windows.Forms.Label lblShowPairs;
		private System.Windows.Forms.Label lblWorkingUnit;
		private System.Windows.Forms.ComboBox cbWorkingUnit;
		private System.Windows.Forms.ComboBox cbLocation;
		private System.Windows.Forms.Label lblLocation;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.ListView lvResults;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnReport;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// Language
		private CultureInfo culture;
		private ResourceManager rm;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.ComboBox cbEmplName;
		private System.Windows.Forms.DateTimePicker dtFrom;
		private System.Windows.Forms.DateTimePicker dtTo;

		// Debug log
		DebugLog log;

		// List View indexes
		const int LocationIndex = 0;
		const int LastNameIndex = 1;
		const int FirstNameIndex = 2;
		const int PassType = 3;
		const int StartTime = 4;
		const int EndTime = 5;

		ApplUserTO logInUser;
		List<ApplRoleTO> currentRoles;
		Hashtable menuItemsPermissions;
		string menuItemID;

		bool readPermission = false;
		bool addPermission = false;
		bool updatePermission = false;
		bool deletePermission = false;

		List<IOPairTO> currentIOPairsList;
		private int sortOrder;
		private int sortField;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnPrev;
		private int startIndex;

        List<IOPairTO> currentNOIOPairsList;
        // all Holidays, Key is Date, value is Holiday
        Dictionary<DateTime, HolidayTO> holidays = new Dictionary<DateTime,HolidayTO>();
        private int sortOrderNoPairs;
        private int sortFieldNoPairs;
        private int startIndexNoPairs;

		private List<WorkingUnitTO> wUnits;
        private System.Windows.Forms.Label lblTotal;
        private Button btnWUTree;
        private Button btnLocationTree;
        private string wuString = "";
        private CheckBox chbHierarhicly;
        private Button btnTemp;

        //for location tree view
        private List<LocationTO> locArray;
        private ComboBox cbPassType;
        private Label lblPassType;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;
        
        //for employeePersonalRecordsMDI
        private List<EmployeeTO> currentEmplArray;
        private GroupBox groupBox1;
        private GroupBox gbIncludeWeekendsAndHolydays;
        private RadioButton rbNo;
        private RadioButton rbYes;
        private CheckBox cbNOPairs;
        private ListView lvNoPairs;
        private Button btnWithoutNext;
        private Button btnWithoutPrev;
        private Label lblTotalNoPairs;

        private Filter filter;
        
		public IOPairs()
		{
			InitializeComponent();
			this.CenterToScreen();

			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			logInUser = NotificationController.GetLogInUser();

			currentIOPairsList = new List<IOPairTO>();
            currentNOIOPairsList = new List<IOPairTO>();

			// Set Language
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(Locations).Assembly);
			setLanguage();

        }
        #region MDI child method's        
        public void MDIchangeSelectedEmployee(int selectedWU, int selectedEmployeeID, DateTime from, DateTime to, bool check)
        {
            try
            {
                dtFrom.Value = from;
                dtTo.Value = to;
                chbHierarhicly.Checked = check;
                foreach (WorkingUnitTO wu in wUnits)
                {
                    if (wu.WorkingUnitID == selectedWU)
                        cbWorkingUnit.SelectedValue = selectedWU;
                }

                foreach (EmployeeTO empl in currentEmplArray)
                {
                    if (empl.EmployeeID == selectedEmployeeID)
                        cbEmplName.SelectedValue = selectedEmployeeID;
                }

                btnSearch_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairs.changeSelectedEmployee(): " + ex.Message + "\n");
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IOPairs));
            this.gbIOPairs = new System.Windows.Forms.GroupBox();
            this.cbPassType = new System.Windows.Forms.ComboBox();
            this.lblPassType = new System.Windows.Forms.Label();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnLocationTree = new System.Windows.Forms.Button();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.lblName = new System.Windows.Forms.Label();
            this.cbEmplName = new System.Windows.Forms.ComboBox();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.lblShowPairs = new System.Windows.Forms.Label();
            this.cbShowPairs = new System.Windows.Forms.ComboBox();
            this.lvResults = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnTemp = new System.Windows.Forms.Button();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbIncludeWeekendsAndHolydays = new System.Windows.Forms.GroupBox();
            this.rbNo = new System.Windows.Forms.RadioButton();
            this.rbYes = new System.Windows.Forms.RadioButton();
            this.cbNOPairs = new System.Windows.Forms.CheckBox();
            this.lvNoPairs = new System.Windows.Forms.ListView();
            this.btnWithoutNext = new System.Windows.Forms.Button();
            this.btnWithoutPrev = new System.Windows.Forms.Button();
            this.lblTotalNoPairs = new System.Windows.Forms.Label();
            this.gbIOPairs.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbIncludeWeekendsAndHolydays.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbIOPairs
            // 
            this.gbIOPairs.Controls.Add(this.cbPassType);
            this.gbIOPairs.Controls.Add(this.lblPassType);
            this.gbIOPairs.Controls.Add(this.chbHierarhicly);
            this.gbIOPairs.Controls.Add(this.btnLocationTree);
            this.gbIOPairs.Controls.Add(this.btnWUTree);
            this.gbIOPairs.Controls.Add(this.btnSearch);
            this.gbIOPairs.Controls.Add(this.lblTo);
            this.gbIOPairs.Controls.Add(this.lblFrom);
            this.gbIOPairs.Controls.Add(this.dtTo);
            this.gbIOPairs.Controls.Add(this.dtFrom);
            this.gbIOPairs.Controls.Add(this.lblLocation);
            this.gbIOPairs.Controls.Add(this.cbLocation);
            this.gbIOPairs.Controls.Add(this.lblName);
            this.gbIOPairs.Controls.Add(this.cbEmplName);
            this.gbIOPairs.Controls.Add(this.cbWorkingUnit);
            this.gbIOPairs.Controls.Add(this.lblWorkingUnit);
            this.gbIOPairs.Controls.Add(this.lblShowPairs);
            this.gbIOPairs.Controls.Add(this.cbShowPairs);
            this.gbIOPairs.Location = new System.Drawing.Point(24, 8);
            this.gbIOPairs.Name = "gbIOPairs";
            this.gbIOPairs.Size = new System.Drawing.Size(504, 256);
            this.gbIOPairs.TabIndex = 0;
            this.gbIOPairs.TabStop = false;
            this.gbIOPairs.Tag = "FILTERABLE";
            this.gbIOPairs.Text = "IO Pairs";
            // 
            // cbPassType
            // 
            this.cbPassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPassType.Location = new System.Drawing.Point(128, 217);
            this.cbPassType.Name = "cbPassType";
            this.cbPassType.Size = new System.Drawing.Size(184, 21);
            this.cbPassType.TabIndex = 14;
            // 
            // lblPassType
            // 
            this.lblPassType.Location = new System.Drawing.Point(34, 215);
            this.lblPassType.Name = "lblPassType";
            this.lblPassType.Size = new System.Drawing.Size(88, 23);
            this.lblPassType.TabIndex = 17;
            this.lblPassType.Text = "Pass Type:";
            this.lblPassType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(411, 54);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(89, 24);
            this.chbHierarhicly.TabIndex = 5;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // btnLocationTree
            // 
            this.btnLocationTree.Image = ((System.Drawing.Image)(resources.GetObject("btnLocationTree.Image")));
            this.btnLocationTree.Location = new System.Drawing.Point(318, 117);
            this.btnLocationTree.Name = "btnLocationTree";
            this.btnLocationTree.Size = new System.Drawing.Size(25, 24);
            this.btnLocationTree.TabIndex = 9;
            this.btnLocationTree.Click += new System.EventHandler(this.btnLocationTree_Click);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(380, 54);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 4;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(413, 217);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 16;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(24, 184);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(96, 23);
            this.lblTo.TabIndex = 14;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(16, 152);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(104, 23);
            this.lblFrom.TabIndex = 12;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "dd.MM.yyyy";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(128, 184);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(184, 20);
            this.dtTo.TabIndex = 12;
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "dd.MM.yyyy";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(128, 152);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(184, 20);
            this.dtFrom.TabIndex = 10;
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(16, 120);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(104, 23);
            this.lblLocation.TabIndex = 9;
            this.lblLocation.Text = "Location:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.Location = new System.Drawing.Point(128, 120);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(184, 21);
            this.cbLocation.TabIndex = 8;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(16, 88);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(104, 23);
            this.lblName.TabIndex = 7;
            this.lblName.Text = "First Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmplName
            // 
            this.cbEmplName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmplName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmplName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmplName.Location = new System.Drawing.Point(128, 88);
            this.cbEmplName.Name = "cbEmplName";
            this.cbEmplName.Size = new System.Drawing.Size(184, 21);
            this.cbEmplName.TabIndex = 6;
            this.cbEmplName.Leave += new System.EventHandler(this.cbEmplName_Leave);
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(128, 56);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(246, 21);
            this.cbWorkingUnit.TabIndex = 3;
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(16, 56);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(104, 23);
            this.lblWorkingUnit.TabIndex = 3;
            this.lblWorkingUnit.Text = "Working Unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShowPairs
            // 
            this.lblShowPairs.Location = new System.Drawing.Point(16, 23);
            this.lblShowPairs.Name = "lblShowPairs";
            this.lblShowPairs.Size = new System.Drawing.Size(104, 23);
            this.lblShowPairs.TabIndex = 1;
            this.lblShowPairs.Text = "Show Pairs:";
            this.lblShowPairs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbShowPairs
            // 
            this.cbShowPairs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbShowPairs.Location = new System.Drawing.Point(128, 24);
            this.cbShowPairs.Name = "cbShowPairs";
            this.cbShowPairs.Size = new System.Drawing.Size(184, 21);
            this.cbShowPairs.TabIndex = 2;
            // 
            // lvResults
            // 
            this.lvResults.FullRowSelect = true;
            this.lvResults.GridLines = true;
            this.lvResults.HideSelection = false;
            this.lvResults.Location = new System.Drawing.Point(24, 272);
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(704, 240);
            this.lvResults.TabIndex = 19;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.View = System.Windows.Forms.View.Details;
            this.lvResults.SelectedIndexChanged += new System.EventHandler(this.lvResults_SelectedIndexChanged);
            this.lvResults.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvResults_ColumnClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(25, 552);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 21;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(125, 552);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 22;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(225, 552);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 23;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(437, 552);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(76, 23);
            this.btnReport.TabIndex = 24;
            this.btnReport.Text = "Report";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(650, 552);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 25;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(696, 240);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 18;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(656, 240);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 17;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(576, 520);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(152, 16);
            this.lblTotal.TabIndex = 20;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnTemp
            // 
            this.btnTemp.Location = new System.Drawing.Point(656, 12);
            this.btnTemp.Name = "btnTemp";
            this.btnTemp.Size = new System.Drawing.Size(75, 23);
            this.btnTemp.TabIndex = 26;
            this.btnTemp.Text = "Temp";
            this.btnTemp.UseVisualStyleBackColor = true;
            this.btnTemp.Visible = false;
            this.btnTemp.Click += new System.EventHandler(this.btnTemp_Click);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(534, 8);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 100);
            this.gbFilter.TabIndex = 27;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gbIncludeWeekendsAndHolydays);
            this.groupBox1.Controls.Add(this.cbNOPairs);
            this.groupBox1.Location = new System.Drawing.Point(534, 125);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(194, 100);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            // 
            // gbIncludeWeekendsAndHolydays
            // 
            this.gbIncludeWeekendsAndHolydays.Controls.Add(this.rbNo);
            this.gbIncludeWeekendsAndHolydays.Controls.Add(this.rbYes);
            this.gbIncludeWeekendsAndHolydays.Location = new System.Drawing.Point(6, 35);
            this.gbIncludeWeekendsAndHolydays.Name = "gbIncludeWeekendsAndHolydays";
            this.gbIncludeWeekendsAndHolydays.Size = new System.Drawing.Size(182, 55);
            this.gbIncludeWeekendsAndHolydays.TabIndex = 7;
            this.gbIncludeWeekendsAndHolydays.TabStop = false;
            this.gbIncludeWeekendsAndHolydays.Text = "Include weekends and holydays";
            // 
            // rbNo
            // 
            this.rbNo.AutoSize = true;
            this.rbNo.Checked = true;
            this.rbNo.Location = new System.Drawing.Point(102, 24);
            this.rbNo.Name = "rbNo";
            this.rbNo.Size = new System.Drawing.Size(39, 17);
            this.rbNo.TabIndex = 1;
            this.rbNo.TabStop = true;
            this.rbNo.Text = "No";
            this.rbNo.UseVisualStyleBackColor = true;
            // 
            // rbYes
            // 
            this.rbYes.AutoSize = true;
            this.rbYes.Location = new System.Drawing.Point(26, 24);
            this.rbYes.Name = "rbYes";
            this.rbYes.Size = new System.Drawing.Size(43, 17);
            this.rbYes.TabIndex = 0;
            this.rbYes.Text = "Yes";
            this.rbYes.UseVisualStyleBackColor = true;
            // 
            // cbNOPairs
            // 
            this.cbNOPairs.Location = new System.Drawing.Point(6, 10);
            this.cbNOPairs.Name = "cbNOPairs";
            this.cbNOPairs.Size = new System.Drawing.Size(89, 24);
            this.cbNOPairs.TabIndex = 6;
            this.cbNOPairs.Text = "No pairs ";
            this.cbNOPairs.CheckedChanged += new System.EventHandler(this.cbNOPairs_CheckedChanged);
            // 
            // lvNoPairs
            // 
            this.lvNoPairs.FullRowSelect = true;
            this.lvNoPairs.GridLines = true;
            this.lvNoPairs.HideSelection = false;
            this.lvNoPairs.Location = new System.Drawing.Point(24, 272);
            this.lvNoPairs.Name = "lvNoPairs";
            this.lvNoPairs.Size = new System.Drawing.Size(704, 240);
            this.lvNoPairs.TabIndex = 29;
            this.lvNoPairs.UseCompatibleStateImageBehavior = false;
            this.lvNoPairs.View = System.Windows.Forms.View.Details;
            this.lvNoPairs.Visible = false;
            this.lvNoPairs.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvNoPairs_ColumnClick);
            // 
            // btnWithoutNext
            // 
            this.btnWithoutNext.Location = new System.Drawing.Point(696, 240);
            this.btnWithoutNext.Name = "btnWithoutNext";
            this.btnWithoutNext.Size = new System.Drawing.Size(32, 23);
            this.btnWithoutNext.TabIndex = 19;
            this.btnWithoutNext.Text = ">";
            this.btnWithoutNext.Visible = false;
            this.btnWithoutNext.Click += new System.EventHandler(this.btnWithoutNext_Click);
            // 
            // btnWithoutPrev
            // 
            this.btnWithoutPrev.Location = new System.Drawing.Point(656, 240);
            this.btnWithoutPrev.Name = "btnWithoutPrev";
            this.btnWithoutPrev.Size = new System.Drawing.Size(32, 23);
            this.btnWithoutPrev.TabIndex = 18;
            this.btnWithoutPrev.Text = "<";
            this.btnWithoutPrev.Visible = false;
            this.btnWithoutPrev.Click += new System.EventHandler(this.btnWithoutPrev_Click);
            // 
            // lblTotalNoPairs
            // 
            this.lblTotalNoPairs.Location = new System.Drawing.Point(576, 520);
            this.lblTotalNoPairs.Name = "lblTotalNoPairs";
            this.lblTotalNoPairs.Size = new System.Drawing.Size(152, 16);
            this.lblTotalNoPairs.TabIndex = 30;
            this.lblTotalNoPairs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTotalNoPairs.Visible = false;
            // 
            // IOPairs
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(742, 582);
            this.ControlBox = false;
            this.Controls.Add(this.lblTotalNoPairs);
            this.Controls.Add(this.btnWithoutNext);
            this.Controls.Add(this.btnWithoutPrev);
            this.Controls.Add(this.lvNoPairs);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.btnTemp);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvResults);
            this.Controls.Add(this.gbIOPairs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "IOPairs";
            this.ShowInTaskbar = false;
            this.Text = "IOPairs";
            this.Load += new System.EventHandler(this.IOPairs_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.IOPairs_KeyUp);
            this.gbIOPairs.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.gbIncludeWeekendsAndHolydays.ResumeLayout(false);
            this.gbIncludeWeekendsAndHolydays.PerformLayout();
            this.ResumeLayout(false);

		}

       
        void cbEmplName_Leave(object sender, EventArgs e)
        {
            if (cbEmplName.SelectedIndex == -1) {
                cbEmplName.SelectedIndex = 0;
            }
        
        }
		#endregion

		#region Inner Class for sorting Array List of Employees

		/*
		 *  Class used for sorting Array List of Employees
		*/

		private class ArrayListSort:IComparer<IOPairTO>
		{        
			private int compOrder;        
			private int compField;
			public ArrayListSort(int sortOrder, int sortField)        
			{            
				compOrder = sortOrder;
				compField = sortField;
			}        
			
			public int Compare(IOPairTO x, IOPairTO y)        
			{
				IOPairTO iop1 = null;
				IOPairTO iop2 = null;

				if (compOrder == Constants.sortAsc)
				{
					iop1 = x;
					iop2 = y;
				}
				else
				{
					iop1 = y;
					iop2 = x;
				}

				switch(compField)            
				{                
					case IOPairs.LocationIndex: 
						return iop1.LocationName.CompareTo(iop2.LocationName); 
					case IOPairs.LastNameIndex:
						return iop1.EmployeeLastName.CompareTo(iop2.EmployeeLastName);                
					case IOPairs.FirstNameIndex:
						return iop1.EmployeeName.CompareTo(iop2.EmployeeName);                
					case IOPairs.PassType:
						return iop1.PassType.CompareTo(iop2.PassType);                
					case IOPairs.StartTime:
						return iop1.StartTime.CompareTo(iop2.StartTime);                
					case IOPairs.EndTime:
						return iop1.EndTime.CompareTo(iop2.EndTime);                
					default:                    
						return iop1.StartTime.CompareTo(iop2.StartTime);            
				}        
			}    
		}

		#endregion

        #region Inner Class for sorting Array List of Employees

        /*
		 *  Class used for sorting Array List of Employees
		*/

        private class ArrayListSortNoPairs : IComparer<IOPairTO>
        {
            private int compOrder;
            private int compField;
            public ArrayListSortNoPairs(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(IOPairTO x, IOPairTO y)
            {
                IOPairTO iop1 = null;
                IOPairTO iop2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    iop1 = x;
                    iop2 = y;
                }
                else
                {
                    iop1 = y;
                    iop2 = x;
                }

                switch (compField)
                {
                    case 0:
                        return iop1.WUName.CompareTo(iop2.WUName);
                    case IOPairs.LastNameIndex:
                        return iop1.EmployeeLastName.CompareTo(iop2.EmployeeLastName);
                    //case IOPairs.FirstNameIndex:
                    //    return iop1.EmployeeName.CompareTo(iop2.EmployeeName);
                    case 2:
                        return iop1.IOPairDate.CompareTo(iop2.IOPairDate);                    
                    default:
                        return iop1.IOPairDate.CompareTo(iop2.IOPairDate);
                }
            }
        }

        #endregion
        bool isHoliday(DateTime day)
        {
            return (holidays.ContainsKey(day));
        }

        bool isWeekend(DateTime day)
        {
            return ((day.DayOfWeek == DayOfWeek.Saturday) || (day.DayOfWeek == DayOfWeek.Sunday));
        }
		private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("IOPairsTitle", culture);
                this.gbIOPairs.Text = rm.GetString("gbIOPairs", culture);
                this.gbFilter.Text = rm.GetString("gbFilter", culture);
                gbIncludeWeekendsAndHolydays.Text = rm.GetString("gbIncludeWeekendsAndHolydays", culture);

                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);
                cbNOPairs.Text = rm.GetString("cbNOPairs", culture);
                rbNo.Text = rm.GetString("no", culture);
                rbYes.Text = rm.GetString("yes", culture);

                this.lblShowPairs.Text = rm.GetString("lblShowPairs", culture);
                this.lblWorkingUnit.Text = rm.GetString("lblWorkingUnit", culture);
                this.lblName.Text = rm.GetString("lblEmployee", culture);
                this.lblLocation.Text = rm.GetString("lblLocation", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                lblPassType.Text = rm.GetString("lblPassType", culture);

                this.btnSearch.Text = rm.GetString("btnSearch", culture);
                this.btnAdd.Text = rm.GetString("btnAdd", culture);
                this.btnUpdate.Text = rm.GetString("btnUpdate", culture);
                this.btnDelete.Text = rm.GetString("btnDelete", culture);
                this.btnReport.Text = rm.GetString("btnReport", culture);
                this.btnClose.Text = rm.GetString("btnClose", culture);

                // List View Header
                lvResults.BeginUpdate();

                lvResults.Columns.Add(rm.GetString("hdrLocation", culture), (lvResults.Width - 6) / 6, HorizontalAlignment.Left);
                lvResults.Columns.Add(rm.GetString("hdrLastName", culture), (lvResults.Width - 6) / 6, HorizontalAlignment.Left);
                lvResults.Columns.Add(rm.GetString("hdrFirstName", culture), (lvResults.Width - 6) / 6, HorizontalAlignment.Left);
                lvResults.Columns.Add(rm.GetString("hdrPassType", culture), (lvResults.Width - 6) / 6, HorizontalAlignment.Left);
                lvResults.Columns.Add(rm.GetString("hdrSartTime", culture), (lvResults.Width - 6) / 6, HorizontalAlignment.Left);
                lvResults.Columns.Add(rm.GetString("hdrEndTime", culture), (lvResults.Width - 6) / 6, HorizontalAlignment.Left);

                lvResults.EndUpdate();
                // List View Header
                lvNoPairs.BeginUpdate();

                lvNoPairs.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvResults.Width - 12) / 3, HorizontalAlignment.Left);
                lvNoPairs.Columns.Add(rm.GetString("hdrEmployee", culture), (lvResults.Width - 12) / 3, HorizontalAlignment.Left);
                // lvNoPairs.Columns.Add(rm.GetString("hdrFirstName", culture), (lvResults.Width - 6) / 4, HorizontalAlignment.Left);
                lvNoPairs.Columns.Add(rm.GetString("hdrDate", culture), (lvResults.Width - 12) / 3, HorizontalAlignment.Left);

                lvNoPairs.EndUpdate();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " IOPairs.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
		}

		private void populatePairStatusCombo()
		{
			try
			{
				ArrayList statusArray = new ArrayList();
				statusArray.Add(rm.GetString("all", culture));
				//statusArray.Add(ConfigurationManager.AppSettings["Complete"]);
                statusArray.Add(Constants.Complete);
				//statusArray.Add(ConfigurationManager.AppSettings["Incomplete"]);
                statusArray.Add(Constants.Incomplete);

				cbShowPairs.DataSource = statusArray;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairs.populatePairStatusCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateWorkigUnitCombo()
		{
			try
			{
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    wuArray.Add(wuTO);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

				cbWorkingUnit.DataSource = wuArray;
				cbWorkingUnit.DisplayMember = "Name";
				cbWorkingUnit.ValueMember = "WorkingUnitID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairs.populateWorkigUnitCombo(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void populateLocationCb()
		{
			try
			{
				Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
				locArray = loc.Search();
				locArray.Insert(0, new LocationTO(-1, rm.GetString("all", culture), rm.GetString("all", culture), -1, 0, ""));

				cbLocation.DataSource = locArray;
				cbLocation.DisplayMember = "Name";
				cbLocation.ValueMember = "LocationID";
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairs.populateLocationCb(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

	    private void  cbWorkingUnit_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                bool check = false;
                if (cbWorkingUnit.SelectedIndex != 0)
                {
                    foreach (WorkingUnitTO wu in wUnits)
                    {
                        if (wu.WorkingUnitID == (int)cbWorkingUnit.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                        {
                            if (!chbHierarhicly.Checked)
                            {
                                chbHierarhicly.Checked = true;
                                check = true;
                            }
                        }
                    }
                }
                if (cbWorkingUnit.SelectedValue is int && !check)
                {
                    populateEmployeeCombo((int)cbWorkingUnit.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairs.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

        /// <summary>
        /// Populate Pass Type Combo Box
        /// </summary>
        private void populatePassTypeCombo()
        {
            try
            {                
                List<int> types = new List<int>();
                types.Add(Constants.passOnReader);
                types.Add(Constants.wholeDayAbsence);
                List<PassTypeTO> ptArray = new PassType().Search(types);
                ptArray.Insert(0, new PassTypeTO(-1, rm.GetString("all", culture), 0, 0, ""));

                cbPassType.DataSource = ptArray;
                cbPassType.DisplayMember = "Description";
                cbPassType.ValueMember = "PassTypeID";
                cbPassType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairs.populatePassTypeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

		private void populateEmployeeCombo(List<EmployeeTO> array)
		{
			try
			{
                EmployeeTO empl = new EmployeeTO();
				empl.LastName = rm.GetString("all", culture);
				array.Insert(0, empl);

				foreach(EmployeeTO employee in array)
				{
					employee.LastName += " " + employee.FirstName;
				}
                currentEmplArray = array;
				cbEmplName.DataSource = array;
				cbEmplName.DisplayMember = "LastName";
				cbEmplName.ValueMember = "EmployeeID";
 			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairs.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
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
                if (wuID ==-1)
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
                            if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnit.SelectedValue)
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

                foreach (EmployeeTO employee in currentEmplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                currentEmplArray.Insert(0, empl);

                cbEmplName.DataSource = currentEmplArray;
                cbEmplName.DisplayMember = "LastName";
                cbEmplName.ValueMember = "EmployeeID";
                cbEmplName.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateEmployeeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReport_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (!cbNOPairs.Checked)
                {
                    if (currentIOPairsList.Count > 0)
                    {
                        // Table Definition for Crystal Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("io_pairs");
                        DataTable tableI = new DataTable("images");

                        tableCR.Columns.Add("location", typeof(System.String));
                        tableCR.Columns.Add("first_name", typeof(System.String));
                        tableCR.Columns.Add("last_name", typeof(System.String));
                        tableCR.Columns.Add("pass_type", typeof(System.String));
                        tableCR.Columns.Add("start_time", typeof(System.String));
                        tableCR.Columns.Add("end_time", typeof(System.String));
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

                        foreach (IOPairTO ioPair in currentIOPairsList)
                        {
                            DataRow row = tableCR.NewRow();

                            row["location"] = ioPair.LocationName;
                            row["last_name"] = ioPair.EmployeeLastName;
                            row["first_name"] = ioPair.EmployeeName;
                            row["pass_type"] = ioPair.PassType;
                            if (ioPair.StartTime == new DateTime(0))
                            {
                                row["start_time"] = "";
                            }
                            else
                            {
                                row["start_time"] = ioPair.StartTime.ToString("dd.MM.yyyy HH:mm:ss");
                            }
                            if (ioPair.EndTime == new DateTime(0))
                            {
                                row["end_time"] = "";
                            }
                            else
                            {
                                row["end_time"] = ioPair.EndTime.ToString("dd.MM.yyyy HH:mm:ss");
                            }
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

                        string selShowPairs = "*";
                        string selWorkingUnit = "*";
                        string selEmplName = "*";
                        string selLocation = "*";


                        if (cbWorkingUnit.SelectedIndex >= 0 && (int)cbWorkingUnit.SelectedValue >= 0)
                            selWorkingUnit = cbWorkingUnit.Text;
                        if (cbEmplName.SelectedIndex >= 0 && (int)cbEmplName.SelectedValue >= 0)
                            selEmplName = cbEmplName.Text;
                        if (cbShowPairs.SelectedIndex >= 0)
                            selShowPairs = cbShowPairs.Text;
                        if (cbLocation.SelectedIndex >= 0 && (int)cbLocation.SelectedValue >= 0)
                            selLocation = cbLocation.Text;


                        if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                        {
                            Reports.Reports_sr.IOPairsCRView view = new Reports.Reports_sr.IOPairsCRView(dataSetCR,
                                 selWorkingUnit, selEmplName, selShowPairs, selLocation, dtFrom.Value, dtTo.Value);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                        {
                            Reports.Reports_en.IOPairsCRView_en view = new Reports.Reports_en.IOPairsCRView_en(dataSetCR,
                                 selWorkingUnit, selEmplName, selShowPairs, selLocation, dtFrom.Value, dtTo.Value);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                        {
                            Reports.Reports_fi.IOPairsCRView_fi view = new Reports.Reports_fi.IOPairsCRView_fi(dataSetCR,
                                 selWorkingUnit, selEmplName, selShowPairs, selLocation, dtFrom.Value, dtTo.Value);
                            view.ShowDialog(this);
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }
                }
                else
                {
                    if (currentNOIOPairsList.Count > 0)
                    {
                        // Table Definition for Crystal Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("no_pairs");
                        DataTable tableI = new DataTable("images");

                        tableCR.Columns.Add("wu", typeof(System.String));
                        tableCR.Columns.Add("first_name", typeof(System.String));
                        tableCR.Columns.Add("last_name", typeof(System.String));
                        tableCR.Columns.Add("date_string", typeof(System.String));
                        tableCR.Columns.Add("id", typeof(System.String));
                        tableCR.Columns.Add("date", typeof(System.DateTime));
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

                        foreach (IOPairTO ioPair in currentNOIOPairsList)
                        {
                            DataRow row = tableCR.NewRow();

                            row["wu"] = ioPair.WUName;
                            row["last_name"] = ioPair.EmployeeLastName;
                            row["id"] = ioPair.EmployeeID;
                            row["first_name"] = ioPair.EmployeeName;
                            row["date_string"] = ioPair.IOPairDate.ToString("dd.MM.yyyy");
                            row["date"] = ioPair.IOPairDate;
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
                        string selEmplName = "*";


                        if (cbWorkingUnit.SelectedIndex >= 0 && (int)cbWorkingUnit.SelectedValue >= 0)
                            selWorkingUnit = cbWorkingUnit.Text;
                        if (cbEmplName.SelectedIndex >= 0 && (int)cbEmplName.SelectedValue >= 0)
                            selEmplName = cbEmplName.Text;
                      

                        if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                        {
                            Reports.Reports_sr.NoIOPairsCRView view = new Reports.Reports_sr.NoIOPairsCRView(dataSetCR,
                                 selWorkingUnit, selEmplName, dtFrom.Value, dtTo.Value);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                        {
                            Reports.Reports_en.NoIOPairsCRView_en view = new Reports.Reports_en.NoIOPairsCRView_en(dataSetCR,
                                 selWorkingUnit, selEmplName, dtFrom.Value, dtTo.Value);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                        {
                            Reports.Reports_fi.NoIOPairsCRView_fi view = new Reports.Reports_fi.NoIOPairsCRView_fi(dataSetCR,
                                 selWorkingUnit, selEmplName, dtFrom.Value, dtTo.Value);
                            view.ShowDialog(this);
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairs.btnReport_Click(): " + ex.Message + "\n");
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
				int count = 0;
				
				DateTime startDate = new DateTime();
				DateTime endDate = new DateTime();

				int emplID = -1;
				if (cbEmplName.SelectedIndex >= 0 && (int) cbEmplName.SelectedValue >= 0)
				{
					emplID = (int) cbEmplName.SelectedValue;
				}

                string selectedWU = wuString;
                if (cbWorkingUnit.SelectedIndex >= 0 && (int)cbWorkingUnit.SelectedValue >= 0)
                {
                    if (chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnit.SelectedValue)
                            {
                                wuList.Add(workingUnit);
                                workUnit.WUTO = workingUnit;
                            }
                        }
                        if (workUnit.WUTO.ChildWUNumber > 0)
                            wuList = workUnit.FindAllChildren(wuList);
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
                    else
                    {
                        selectedWU = cbWorkingUnit.SelectedValue.ToString();
                    }
                }

                if (!wuString.Equals(""))
                {
                    if (cbEmplName.Items.Count == 0)
                    {
                        MessageBox.Show(rm.GetString("noIOPairsFound", culture));
                        clearListView();
                        return;
                    }

                    // ALL
                    if (cbShowPairs.SelectedIndex.Equals(0))
                    {
                        startDate = DateTime.Now;
                        endDate = DateTime.Now;
                    }
                    // COMPLETE
                    else if (cbShowPairs.SelectedIndex.Equals(1))
                    {
                        startDate = new DateTime(1, 1, 1, 0, 0, 0);
                        endDate = new DateTime(1, 1, 1, 0, 0, 0);
                    }
                    // INCOMPLETE
                    else if (cbShowPairs.SelectedIndex.Equals(2))
                    {
                        startDate = new DateTime(1, 1, 1, 0, 0, 1);
                        endDate = new DateTime(1, 1, 1, 0, 0, 1);
                    }
                    int checkedWUID = -1;
                    if (!chbHierarhicly.Checked)
                        checkedWUID = Int32.Parse(cbWorkingUnit.SelectedValue.ToString());

                    IOPair ioPair = new IOPair();
                    ioPair.PairTO.StartTime = startDate;
                    ioPair.PairTO.EndTime = endDate;
                    ioPair.PairTO.EmployeeID = emplID;
                    ioPair.PairTO.LocationID = (int)cbLocation.SelectedValue;
                    ioPair.PairTO.PassTypeID = (int)cbPassType.SelectedValue;

                    if (!cbNOPairs.Checked)
                    {
                        count = ioPair.SearchCount(dtFrom.Value.Date, dtTo.Value.Date, selectedWU, checkedWUID);

                        if (count > Constants.maxRecords)
                        {
                            DialogResult result = MessageBox.Show(rm.GetString("IOPairsGreaterThenAllowed", culture), "", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                currentIOPairsList = ioPair.SearchWithType(dtFrom.Value.Date, dtTo.Value.Date, selectedWU, checkedWUID);
                            }
                            else
                            {
                                currentIOPairsList.Clear();
                                clearListView();
                            }
                        }
                        else
                        {
                            if (count > 0)
                            {
                                currentIOPairsList = ioPair.SearchWithType(dtFrom.Value.Date, dtTo.Value.Date, selectedWU, checkedWUID);
                            }
                            else
                            {
                                currentIOPairsList.Clear();
                                clearListView();
                            }
                        }
                    }
                    else//without IOPairs
                    {
                        //get IOPairs for employees and period
                        ioPair.PairTO.LocationID = -1;
                        ioPair.PairTO.PassTypeID = -1;
                       List<IOPairTO>  pairsList = ioPair.SearchWithType(dtFrom.Value.Date, dtTo.Value.Date, selectedWU, checkedWUID);

                        //key is employeeID, value is hashtable
                       Dictionary<int, Dictionary<DateTime, List<IOPairTO>>> employeesTable = new Dictionary<int,Dictionary<DateTime,List<IOPairTO>>>();

                       foreach (IOPairTO pair in pairsList)
                       {
                           if (!employeesTable.ContainsKey(pair.EmployeeID))
                           {
                               employeesTable.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairTO>>());
                           }
                           if (!employeesTable[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                           {
                               employeesTable[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairTO>());
                           }
                           employeesTable[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                       }

                        //all selected employees
                        List<EmployeeTO> emplList = new List<EmployeeTO>();
                        if (emplID != -1)
                        {
                            foreach (EmployeeTO empl in currentEmplArray)
                            {
                                if (empl.EmployeeID == emplID)
                                    emplList.Add(empl);
                            }
                        }
                        else
                        {
                            emplList = currentEmplArray;
                        }
                        currentNOIOPairsList = new List<IOPairTO>();

                        foreach (EmployeeTO employee in emplList)
                        {
                            if (employee.EmployeeID == -1)
                                continue;
                            if (!employeesTable.ContainsKey(employee.EmployeeID))
                            {
                                for (DateTime date = dtFrom.Value.Date; date <= dtTo.Value.Date; date = date.AddDays(1))
                                {
                                    if (rbYes.Checked || (!isWeekend(date) && !isHoliday(date)))
                                    {
                                        IOPairTO iop = new IOPairTO();
                                        iop.EmployeeID = employee.EmployeeID;
                                        iop.EmployeeLastName = employee.LastName;
                                        iop.EmployeeName = employee.FirstName;
                                        iop.WUName = employee.WorkingUnitName;
                                        iop.IOPairDate = date;
                                        currentNOIOPairsList.Add(iop);
                                    }
                                }
                            }
                            else
                            {
                                for (DateTime date = dtFrom.Value.Date; date <= dtTo.Value.Date; date = date.AddDays(1))
                                {
                                    if (!employeesTable[employee.EmployeeID].ContainsKey(date))
                                    {
                                        if (rbYes.Checked || (!isWeekend(date) && !isHoliday(date)))
                                        {
                                            IOPairTO iop = new IOPairTO();
                                            iop.EmployeeID = employee.EmployeeID;
                                            iop.EmployeeLastName = employee.LastName;
                                            iop.EmployeeName = employee.FirstName;
                                            iop.WUName = employee.WorkingUnitName;
                                            iop.IOPairDate = date;
                                            currentNOIOPairsList.Add(iop);
                                        }
                                    }
                                }
                            }
                        }
                        if (currentNOIOPairsList.Count > 0)
                        {
                            startIndexNoPairs = 0;
                            populateNoPairsListView(currentNOIOPairsList, startIndexNoPairs);
                        }
                        else
                        {
                            lvNoPairs.Items.Clear();
                        }
                        this.lblTotalNoPairs.Text = rm.GetString("lblTotal", culture) + currentNOIOPairsList.Count.ToString().Trim();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noEmplIOPairPrivilege", culture));
                    return;
                }
			
				//ioPairsList.ToString();
				if (currentIOPairsList.Count > 0)
				{
					startIndex = 0;
					currentIOPairsList.Sort(new ArrayListSort(sortOrder, sortField));
					populateListView(currentIOPairsList, startIndex);
					this.lblTotal.Visible = true;
					this.lblTotal.Text = rm.GetString("lblTotal", culture) + currentIOPairsList.Count.ToString().Trim();
				}
				else //else if (count == 0)
				{
					//MessageBox.Show(rm.GetString("noIOPairsFound", culture));
                    clearListView();
                    this.lblTotal.Visible = true;
                    this.lblTotal.Text = rm.GetString("lblTotal", culture) + "0";
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairs.btnSearch_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void populateListView(List<IOPairTO> ioPairList, int startIndex)
		{
			try
			{
				if (ioPairList.Count > Constants.recordsPerPage)
				{
					btnPrev.Visible = true;
					btnNext.Visible = true;
				}
				else
				{
					btnPrev.Visible = false;
					btnNext.Visible = false;
				}

				lvResults.BeginUpdate();
				lvResults.Items.Clear();

				if (ioPairList.Count > 0)
				{
					if ((startIndex >= 0) && (startIndex < ioPairList.Count))
					{
						if (startIndex == 0)
						{
							btnPrev.Enabled = false;
						}
						else
						{
							btnPrev.Enabled = true;
						}

						int lastIndex = startIndex + Constants.recordsPerPage;
						if (lastIndex >= ioPairList.Count)
						{
							btnNext.Enabled = false;
							lastIndex = ioPairList.Count;
						}
						else
						{
							btnNext.Enabled = true;
						}

						for (int i = startIndex; i < lastIndex; i++)
						{
							IOPairTO ioPair = ioPairList[i];
							ListViewItem item = new ListViewItem();

							item.Text = ioPair.LocationName;
							item.SubItems.Add(ioPair.EmployeeLastName);
							item.SubItems.Add(ioPair.EmployeeName);
							item.SubItems.Add(ioPair.PassType);

							if (!ioPair.StartTime.Date.Equals(new DateTime(1,1,1,0,0,0)))
							{
								item.SubItems.Add(ioPair.StartTime.ToString("dd/MM/yyyy   HH:mm"));
							}
							else
							{
								item.SubItems.Add("");
							}
					
							if (!ioPair.EndTime.Date.Equals(new DateTime(1,1,1,0,0,0)))
							{
								item.SubItems.Add(ioPair.EndTime.ToString("dd/MM/yyyy   HH:mm"));
							}
							else
							{
								item.SubItems.Add("");
							}

							item.Tag = ioPair;
							lvResults.Items.Add(item);
						}
					}
				}

				lvResults.EndUpdate();
				lvResults.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairs.populateListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}
        private void populateNoPairsListView(List<IOPairTO> ioPairList, int startIndex)
        {
            try
            {
                if (ioPairList.Count > Constants.recordsPerPage)
                {
                    btnWithoutPrev.Visible = true;
                    btnWithoutNext.Visible = true;
                }
                else
                {
                    btnWithoutPrev.Visible = false;
                    btnWithoutNext.Visible = false;
                }

                lvNoPairs.BeginUpdate();
                lvNoPairs.Items.Clear();

                if (ioPairList.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < ioPairList.Count))
                    {
                        if (startIndex == 0)
                        {
                            btnPrev.Enabled = false;
                        }
                        else
                        {
                            btnPrev.Enabled = true;
                        }

                        int lastIndex = startIndex + Constants.recordsPerPage;
                        if (lastIndex >= ioPairList.Count)
                        {
                            btnWithoutNext.Enabled = false;
                            lastIndex = ioPairList.Count;
                        }
                        else
                        {
                            btnWithoutNext.Enabled = true;
                        }

                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            IOPairTO ioPair = ioPairList[i];
                            ListViewItem item = new ListViewItem();

                            item.Text = ioPair.WUName;
                            item.SubItems.Add(ioPair.EmployeeLastName);
                            //item.SubItems.Add(ioPair.EmployeeName);
                            item.SubItems.Add(ioPair.IOPairDate.ToString("dd.MM.yyyy"));                           

                            item.Tag = ioPair;
                            lvNoPairs.Items.Add(item);
                        }
                    }
                }

                lvNoPairs.EndUpdate();
                lvNoPairs.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairs.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

		private void lvResults_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				/*if (lvResults.SelectedItems.Count > 0)
				{
					IOPair ioPair = (IOPair) lvResults.SelectedItems[0].Tag;

					this.cbWorkingUnit.SelectedIndex = cbWorkingUnit.FindStringExact(ioPair.WUName);
					this.cbEmplName.SelectedValue = ioPair.EmployeeID;
					this.cbLocation.SelectedValue = ioPair.LocationID;
					this.dtFrom.Value = ioPair.IOPairDate.Date;
					this.dtTo.Value = ioPair.IOPairDate.Date;
				}*/
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairs.lvResults_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void lvResults_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				int prevOrder = sortOrder;

				if (e.Column == sortField)
				{
					if (prevOrder == Constants.sortAsc)
					{
						sortOrder = Constants.sortDesc;
					}
					else
					{
						sortOrder = Constants.sortAsc;
					}
				}
				else
				{
					// New Sort Order
					sortOrder = Constants.sortAsc;
				}

				sortField = e.Column;
				currentIOPairsList.Sort(new ArrayListSort(sortOrder, sortField));
				startIndex = 0;
				populateListView(currentIOPairsList, startIndex);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairs.lvResults_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
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
                log.writeLog(DateTime.Now + " IOPairs.btnClose_Click(): " + ex.Message + "\n");
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

				IOPairsAdd addNewIOPair = new IOPairsAdd();
				addNewIOPair.ShowDialog(this);

                //Reload form only if some permission is added
                if (addNewIOPair.doReloadOnBack)
                {
                    btnSearch_Click(sender, e);
                }

                //clearForm();
                //clearListView();

                //if (!wuString.Equals(""))
                //{
                //    /*currentIOPairsList = new IOPair().Search(new DateTime(0), new DateTime(0), -1, -1, -1, 
                //        new DateTime(0), new DateTime(0), wuString);
                //    currentIOPairsList.Sort(new ArrayListSort(sortOrder, sortField));
                //    startIndex = 0;
                //    populateListView(currentIOPairsList, startIndex);*/
                //}
                //else
                //{
                //    MessageBox.Show(rm.GetString("noEmplIOPairPrivilege", culture));
                //}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairs.btnAdd_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				if (lvResults.SelectedItems.Count != 1)
				{
					MessageBox.Show(rm.GetString("noSelIOPairUpd", culture));
				}
				else
				{
                    IOPairTO pair = (IOPairTO)lvResults.SelectedItems[0].Tag;
                    if (pair.PassTypeID < 0)
                    {
                        MessageBox.Show(rm.GetString("typeCanNotChange", culture));
                        return;
                    }
					IOPairsAdd updateIOPair = new IOPairsAdd((IOPairTO) lvResults.SelectedItems[0].Tag);
					updateIOPair.ShowDialog(this);

                    //Reload form only if some permission is updated
                    if (updateIOPair.doReloadOnBack)
                    {
                        IOPairTO selectedIOPair = (IOPairTO)lvResults.SelectedItems[0].Tag;
                        btnSearch_Click(sender, e);
                        //set selected item, if it is still in the list
                        for (int i = 0; i < lvResults.Items.Count; i++)
                        {
                            IOPairTO currentIOPair = (IOPairTO)lvResults.Items[i].Tag;
                            if (currentIOPair.IOPairID == selectedIOPair.IOPairID)
                            {
                                lvResults.Items[i].Selected = true;
                                break;
                            }
                        }
                    }

                    //clearForm();
                    //clearListView();

                    //if (!wuString.Equals(""))
                    //{
                    //    /*currentIOPairsList = new IOPair().Search(new DateTime(0), new DateTime(0), -1, -1, -1, 
                    //        new DateTime(0), new DateTime(0), wuString);
                    //    currentIOPairsList.Sort(new ArrayListSort(sortOrder, sortField));
                    //    startIndex = 0;
                    //    populateListView(currentIOPairsList, startIndex);*/
                    //}
                    //else
                    //{
                    //    MessageBox.Show(rm.GetString("noEmplIOPairPrivilege", culture));
                    //}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairs.btnUpdate_Click(): " + ex.Message + "\n");
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

				if (lvResults.SelectedItems.Count > 0)
				{
					DialogResult result = MessageBox.Show(rm.GetString("deleteIOPairs", culture), "", MessageBoxButtons.YesNo);

					if (result == DialogResult.Yes)
					{
						bool isDeleted = true;
						foreach(ListViewItem item in lvResults.SelectedItems)
						{
							IOPairTO current = (IOPairTO) item.Tag;
							isDeleted = new IOPair().Delete(current.IOPairID, current.IOPairDate) && isDeleted;
						}

						if (isDeleted)
						{
							MessageBox.Show(rm.GetString("IOPairsDeleted", culture));
						}
						else
						{
							MessageBox.Show(rm.GetString("IOPairsNotDeleted", culture));
						}

                        btnSearch_Click(sender, e);

                        //clearForm();
                        //clearListView();

                        //if (!wuString.Equals(""))
                        //{
                        //    /*currentIOPairsList = new IOPair().Search(new DateTime(0), new DateTime(0), -1, -1, -1, 
                        //        new DateTime(0), new DateTime(0), wuString);
                        //    currentIOPairsList.Sort(new ArrayListSort(sortOrder, sortField));
                        //    startIndex = 0;
                        //    populateListView(currentIOPairsList, startIndex);*/
                        //}
                        //else
                        //{
                        //    MessageBox.Show(rm.GetString("noEmplIOPairPrivilege", culture));
                        //}
					}
				}
				else
				{
					MessageBox.Show(rm.GetString("msgPleaseSelect", culture));
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairs.btnDelete_Click(): " + ex.Message + "\n");
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

                //btnMapMaintenance.Visible = false;
                //btnMapObjects.Visible = false;
                //btnMapView.Visible = false;
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.setVisibility(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void clearForm()
		{
			this.cbEmplName.SelectedIndex = 0;
			this.cbLocation.SelectedIndex = 0;
			this.cbShowPairs.SelectedIndex = 0;
			this.cbWorkingUnit.SelectedIndex = 0;
			dtFrom.Value = DateTime.Now;
			dtTo.Value = DateTime.Now;
			this.lblTotal.Visible=false;
		}

		private void clearListView()
		{
			lvResults.BeginUpdate();
			lvResults.Items.Clear();
			lvResults.EndUpdate();

			lvResults.Invalidate();

			btnPrev.Visible = false;
			btnNext.Visible = false;
		}

		private void btnPrev_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				startIndex -= Constants.recordsPerPage;
				if (startIndex < 0)
				{
					startIndex = 0;
				}
				populateListView(currentIOPairsList, startIndex);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairs.btnPrev_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void btnNext_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				startIndex += Constants.recordsPerPage;
				populateListView(currentIOPairsList, startIndex);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " IOPairs.btnNext_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

        
        private void IOPairs_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " IOPairs.IOPairs_KeyUp(): " + ex.Message + "\n");
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
                    cbWorkingUnit.SelectedIndex = cbWorkingUnit.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairs.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                LocationsTreeView locationsTreeView = new LocationsTreeView(locArray);
                locationsTreeView.ShowDialog();
                if (!locationsTreeView.selectedLocation.Equals(""))
                {
                    this.cbLocation.SelectedIndex = cbLocation.FindStringExact(locationsTreeView.selectedLocation);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairs.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                        populateEmployeeCombo((int)cbWorkingUnit.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairs.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnTemp_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                EmplPersonalRecordsMDI epr = new EmplPersonalRecordsMDI();
                epr.ShowDialog();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " IOPairs.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void IOPairs_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.IOPairPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                populateLocationCb();
                populatePairStatusCombo();
                populateWorkigUnitCombo();
                populateEmployeeCombo(-1);
                populatePassTypeCombo();

                dtFrom.Value = DateTime.Now;
                dtTo.Value = DateTime.Now;

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                sortOrder = Constants.sortAsc;
                sortField = IOPairs.StartTime;
                startIndex = 0;

                sortOrderNoPairs = Constants.sortAsc;
                sortFieldNoPairs = IOPairs.StartTime;
                startIndexNoPairs = 0;

                clearListView();
                this.lblTotal.Visible = false;

                // get all holidays
                List<HolidayTO> holidayList = new Holiday().Search(new DateTime(), new DateTime());

                foreach (HolidayTO holiday in holidayList)
                {
                    holidays.Add(holiday.HolidayDate, holiday);
                }

                if (!wuString.Equals(""))
                {
                    /*currentIOPairsList = new IOPair().Search(new DateTime(0), new DateTime(0), -1, -1, -1, 
                        new DateTime(0), new DateTime(0), wuString);
                    currentIOPairsList.Sort(new ArrayListSort(sortOrder, sortField));
                    populateListView(currentIOPairsList, startIndex);*/
                }
                else
                {
                    MessageBox.Show(rm.GetString("noEmplIOPairPrivilege", culture));
                }

                filter = new Filter();
                filter.SerachButton = this.btnSearch;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairs.Load(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " IOPairs.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
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
                log.writeLog(DateTime.Now + " IOPairs.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbNOPairs_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                lvNoPairs.Visible = cbNOPairs.Checked;
                lvResults.Visible = !cbNOPairs.Checked;
                lblTotalNoPairs.Visible = cbNOPairs.Checked;
                if (cbNOPairs.Checked)
                {
                    if (currentNOIOPairsList.Count > Constants.recordsPerPage)
                    {
                        btnWithoutPrev.Visible = true;
                        btnWithoutNext.Visible = true;
                    }
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                    populateNoPairsListView(new List<IOPairTO>(), 0);
                    this.lblTotalNoPairs.Text = rm.GetString("lblTotal", culture) + "0";
                }
                else
                {
                    clearListView();
                    this.lblTotal.Text = rm.GetString("lblTotal", culture) + "0";
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairs.cbNOPairs_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvNoPairs_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int prevOrder = sortOrderNoPairs;

                if (e.Column == sortFieldNoPairs)
                {
                    if (prevOrder == Constants.sortAsc)
                    {
                        sortOrder = Constants.sortDesc;
                    }
                    else
                    {
                        sortOrder = Constants.sortAsc;
                    }
                }
                else
                {
                    // New Sort Order
                    sortOrder = Constants.sortAsc;
                }

                sortFieldNoPairs = e.Column;
                currentNOIOPairsList.Sort(new ArrayListSortNoPairs(sortOrderNoPairs, sortFieldNoPairs));
                startIndexNoPairs = 0;
                populateListView(currentIOPairsList, startIndexNoPairs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairs.lvNoPairs_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnWithoutPrev_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndexNoPairs -= Constants.recordsPerPage;
                if (startIndexNoPairs < 0)
                {
                    startIndexNoPairs = 0;
                }
                populateNoPairsListView(currentIOPairsList, startIndexNoPairs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairs.btnWithoutPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnWithoutNext_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndexNoPairs += Constants.recordsPerPage;
                populateNoPairsListView(currentIOPairsList, startIndexNoPairs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairs.btnWithoutNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

       

       
       
	}
}
