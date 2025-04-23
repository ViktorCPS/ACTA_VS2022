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

using TransferObjects;
using Common;
using Util;
using Reports;

namespace UI
{
	/// <summary>
	/// Summary description for Passes.
	/// </summary>
	public class Passes : System.Windows.Forms.Form
	{        
		private System.Windows.Forms.GroupBox gbPasses;
		private System.Windows.Forms.Label lblDirection;
		private System.Windows.Forms.ComboBox cbDirection;
		private System.Windows.Forms.Label lblPassType;
		private System.Windows.Forms.ComboBox cbPassType;
		private System.Windows.Forms.Label lblLocation;
		private System.Windows.Forms.ComboBox cbLocation;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.ListView lvPasses;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.DateTimePicker dtpFrom;
		private System.Windows.Forms.DateTimePicker dtpTo;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.Label lblWU;
		private System.Windows.Forms.ComboBox cbWU;
		private System.Windows.Forms.ComboBox cbEmployee;
		private System.Windows.Forms.Label lblEmployee;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private PassTO currentPass = null;
        private PassHistTO currentPassHist = null;

		// List View indexes
		const int PassIDIndex = 0;
		const int EmployeeIDIndex = 1;
		const int DirectionIndex = 2;
		const int EventTimeIndex = 3;
		const int PassTypeIDIndex = 4;
		const int IsWrkHrsIndex = 5;
		const int LocationIDIndex = 6;
        const int GateIndex = 7;
		const int PairGenUsedIndex = 8;
		const int ManualCreatedIndex = 9;

		private CultureInfo culture;
		private ResourceManager rm;
		DebugLog log;
		ApplUserTO logInUser;
		List<ApplRoleTO> currentRoles;
		Hashtable menuItemsPermissions;
		string menuItemID;
		private List<WorkingUnitTO> wUnits;
		private string wuString = "";

		bool readPermission = false;
		bool addPermission = false;
		bool updatePermission = false;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnPrev;
		bool deletePermission = false;

		List<PassTO> currentPassesList;
		private int sortOrder;
		private int sortField;
		private System.Windows.Forms.Label lblTotal;
        private Button btnPhotos;
        private GroupBox gbAdvancedSearch;
        private GroupBox gbTime;
        private GroupBox groupBox1;
        private DateTimePicker dtFrom;
        private Label lblStart;
        private Label lblEnd;
        private DateTimePicker dtTo;
        private GroupBox gbPassesAdv;
        private GroupBox gbBorderSel;
        private RadioButton rbWithout;
        private RadioButton rbAll;
        private RadioButton rbBorder;
        private CheckBox cbLastOUT;
        private CheckBox cbFirstIN;
        private Button btnReport;
        private Button btnAdvanced;
		private int startIndex;
        private ListView lvWithoutPasses;

        private List<string> statuses = new List<string>();
        private int passesListCount = 0;
        
        List<EmployeeTO> currentEmployeeList;
        private int sortOrderWithout;
        private int sortFieldWithout;
        private int startIndexWithout;
        private int withoutListCount = 0;

        const int EmployeeIDIndex2 = 0;
        const int EmployeeNameIndex2 = 1;
        const int WUNameIndex2 = 2;
        const int DateIndex2 = 3;
        private Button btnWithoutNext;
        private Button btnHistoryOfChange;
        private Label lblIsWrkHrs;
        private Label lblManualCreated;
        private Button btnWithoutPrev;
        private Button btnWUTree;
        private Button btnLocationTree;

        private Dictionary<int, string> gates = new Dictionary<int,string>();
        private CheckBox chbHierarhicly;
        private CheckBox chbIN;
        private CheckBox chbOUT;
        private NumericUpDown nudMoreThan;
        private Label lblMoreThan;
        private Label lblPasses;
        private GroupBox gbPassesNumType;

        //for locations tree view
        List<LocationTO> locArray;
        private GroupBox gbProcessed;
        private RadioButton rbNo;
        private RadioButton rbYes;
        private RadioButton rbObsolete;
        private RadioButton rbAllProcess;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;

        //for employeePersonalRecordsMDI
        List<EmployeeTO> currentEmplArray;
        private ComboBox cbGate;
        private Label lblGate;

        Filter filter;

		public Passes()
		{
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);
                setLanguage();
                currentPass = new PassTO();
                currentPassesList = new List<PassTO>();
                currentEmployeeList = new List<EmployeeTO>();
                logInUser = NotificationController.GetLogInUser();               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                log.writeLog(DateTime.Now + " Passes.changeSelectedEmployee(): " + ex.Message + "\n");
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Passes));
            this.gbPasses = new System.Windows.Forms.GroupBox();
            this.cbGate = new System.Windows.Forms.ComboBox();
            this.lblGate = new System.Windows.Forms.Label();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnLocationTree = new System.Windows.Forms.Button();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cbPassType = new System.Windows.Forms.ComboBox();
            this.lblPassType = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.cbDirection = new System.Windows.Forms.ComboBox();
            this.lblDirection = new System.Windows.Forms.Label();
            this.lblWU = new System.Windows.Forms.Label();
            this.lvPasses = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnPhotos = new System.Windows.Forms.Button();
            this.gbAdvancedSearch = new System.Windows.Forms.GroupBox();
            this.gbProcessed = new System.Windows.Forms.GroupBox();
            this.rbAllProcess = new System.Windows.Forms.RadioButton();
            this.rbObsolete = new System.Windows.Forms.RadioButton();
            this.rbNo = new System.Windows.Forms.RadioButton();
            this.rbYes = new System.Windows.Forms.RadioButton();
            this.gbTime = new System.Windows.Forms.GroupBox();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbBorderSel = new System.Windows.Forms.GroupBox();
            this.cbFirstIN = new System.Windows.Forms.CheckBox();
            this.cbLastOUT = new System.Windows.Forms.CheckBox();
            this.gbPassesNumType = new System.Windows.Forms.GroupBox();
            this.lblPasses = new System.Windows.Forms.Label();
            this.nudMoreThan = new System.Windows.Forms.NumericUpDown();
            this.lblMoreThan = new System.Windows.Forms.Label();
            this.chbIN = new System.Windows.Forms.CheckBox();
            this.chbOUT = new System.Windows.Forms.CheckBox();
            this.gbPassesAdv = new System.Windows.Forms.GroupBox();
            this.rbWithout = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbBorder = new System.Windows.Forms.RadioButton();
            this.btnReport = new System.Windows.Forms.Button();
            this.lvWithoutPasses = new System.Windows.Forms.ListView();
            this.btnWithoutNext = new System.Windows.Forms.Button();
            this.btnWithoutPrev = new System.Windows.Forms.Button();
            this.btnHistoryOfChange = new System.Windows.Forms.Button();
            this.lblIsWrkHrs = new System.Windows.Forms.Label();
            this.lblManualCreated = new System.Windows.Forms.Label();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbPasses.SuspendLayout();
            this.gbAdvancedSearch.SuspendLayout();
            this.gbProcessed.SuspendLayout();
            this.gbTime.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbBorderSel.SuspendLayout();
            this.gbPassesNumType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMoreThan)).BeginInit();
            this.gbPassesAdv.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbPasses
            // 
            this.gbPasses.Controls.Add(this.cbGate);
            this.gbPasses.Controls.Add(this.lblGate);
            this.gbPasses.Controls.Add(this.chbHierarhicly);
            this.gbPasses.Controls.Add(this.btnLocationTree);
            this.gbPasses.Controls.Add(this.btnWUTree);
            this.gbPasses.Controls.Add(this.btnAdvanced);
            this.gbPasses.Controls.Add(this.cbEmployee);
            this.gbPasses.Controls.Add(this.lblEmployee);
            this.gbPasses.Controls.Add(this.cbWU);
            this.gbPasses.Controls.Add(this.dtpTo);
            this.gbPasses.Controls.Add(this.lblTo);
            this.gbPasses.Controls.Add(this.btnSearch);
            this.gbPasses.Controls.Add(this.cbLocation);
            this.gbPasses.Controls.Add(this.lblLocation);
            this.gbPasses.Controls.Add(this.cbPassType);
            this.gbPasses.Controls.Add(this.lblPassType);
            this.gbPasses.Controls.Add(this.dtpFrom);
            this.gbPasses.Controls.Add(this.lblFrom);
            this.gbPasses.Controls.Add(this.cbDirection);
            this.gbPasses.Controls.Add(this.lblDirection);
            this.gbPasses.Controls.Add(this.lblWU);
            this.gbPasses.Location = new System.Drawing.Point(12, 13);
            this.gbPasses.Name = "gbPasses";
            this.gbPasses.Size = new System.Drawing.Size(610, 249);
            this.gbPasses.TabIndex = 1;
            this.gbPasses.TabStop = false;
            this.gbPasses.Tag = "FILTERABLE";
            this.gbPasses.Text = "Passes";
            // 
            // cbGate
            // 
            this.cbGate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGate.Location = new System.Drawing.Point(432, 85);
            this.cbGate.Name = "cbGate";
            this.cbGate.Size = new System.Drawing.Size(160, 21);
            this.cbGate.TabIndex = 25;
            // 
            // lblGate
            // 
            this.lblGate.Location = new System.Drawing.Point(338, 84);
            this.lblGate.Name = "lblGate";
            this.lblGate.Size = new System.Drawing.Size(88, 23);
            this.lblGate.TabIndex = 24;
            this.lblGate.Text = "Gate:";
            this.lblGate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(112, 51);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 23;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // btnLocationTree
            // 
            this.btnLocationTree.Image = ((System.Drawing.Image)(resources.GetObject("btnLocationTree.Image")));
            this.btnLocationTree.Location = new System.Drawing.Point(278, 110);
            this.btnLocationTree.Name = "btnLocationTree";
            this.btnLocationTree.Size = new System.Drawing.Size(25, 24);
            this.btnLocationTree.TabIndex = 22;
            this.btnLocationTree.Click += new System.EventHandler(this.btnLocationTree_Click);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(322, 22);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 20;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.Location = new System.Drawing.Point(482, 162);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(110, 23);
            this.btnAdvanced.TabIndex = 14;
            this.btnAdvanced.Text = "Advanced >>";
            this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
            // 
            // cbEmployee
            // 
            this.cbEmployee.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmployee.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmployee.Location = new System.Drawing.Point(432, 26);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(160, 21);
            this.cbEmployee.TabIndex = 3;
            this.cbEmployee.SelectedIndexChanged += new System.EventHandler(this.cbEmployee_SelectedIndexChanged);
            this.cbEmployee.Leave += new System.EventHandler(this.cbEmployee_Leave);
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(354, 24);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(72, 23);
            this.lblEmployee.TabIndex = 2;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.Location = new System.Drawing.Point(112, 24);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(204, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(112, 165);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(160, 20);
            this.dtpTo.TabIndex = 13;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(34, 162);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(72, 23);
            this.lblTo.TabIndex = 12;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(482, 208);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(110, 23);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.Location = new System.Drawing.Point(112, 112);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(160, 21);
            this.cbLocation.TabIndex = 9;
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(18, 111);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(88, 23);
            this.lblLocation.TabIndex = 8;
            this.lblLocation.Text = "Location:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPassType
            // 
            this.cbPassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPassType.Location = new System.Drawing.Point(112, 85);
            this.cbPassType.Name = "cbPassType";
            this.cbPassType.Size = new System.Drawing.Size(160, 21);
            this.cbPassType.TabIndex = 5;
            // 
            // lblPassType
            // 
            this.lblPassType.Location = new System.Drawing.Point(18, 83);
            this.lblPassType.Name = "lblPassType";
            this.lblPassType.Size = new System.Drawing.Size(88, 23);
            this.lblPassType.TabIndex = 4;
            this.lblPassType.Text = "Pass Type:";
            this.lblPassType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.ImeMode = System.Windows.Forms.ImeMode.On;
            this.dtpFrom.Location = new System.Drawing.Point(112, 139);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(160, 20);
            this.dtpFrom.TabIndex = 11;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(18, 138);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(88, 23);
            this.lblFrom.TabIndex = 10;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbDirection
            // 
            this.cbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDirection.Location = new System.Drawing.Point(432, 54);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new System.Drawing.Size(160, 21);
            this.cbDirection.TabIndex = 7;
            // 
            // lblDirection
            // 
            this.lblDirection.Location = new System.Drawing.Point(338, 52);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(88, 23);
            this.lblDirection.TabIndex = 6;
            this.lblDirection.Text = "Direction:";
            this.lblDirection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(16, 24);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(88, 23);
            this.lblWU.TabIndex = 0;
            this.lblWU.Text = "Working Unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvPasses
            // 
            this.lvPasses.FullRowSelect = true;
            this.lvPasses.GridLines = true;
            this.lvPasses.HideSelection = false;
            this.lvPasses.Location = new System.Drawing.Point(12, 301);
            this.lvPasses.Name = "lvPasses";
            this.lvPasses.Size = new System.Drawing.Size(993, 216);
            this.lvPasses.TabIndex = 3;
            this.lvPasses.UseCompatibleStateImageBehavior = false;
            this.lvPasses.View = System.Windows.Forms.View.Details;
            this.lvPasses.SelectedIndexChanged += new System.EventHandler(this.lvPasses_SelectedIndexChanged);
            this.lvPasses.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvPasses_ColumnClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(14, 552);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(114, 552);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 6;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(214, 552);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(935, 552);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(972, 268);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 12;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(932, 268);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 11;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(858, 520);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(152, 16);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPhotos
            // 
            this.btnPhotos.Location = new System.Drawing.Point(664, 552);
            this.btnPhotos.Name = "btnPhotos";
            this.btnPhotos.Size = new System.Drawing.Size(130, 23);
            this.btnPhotos.TabIndex = 9;
            this.btnPhotos.Text = "Show camera photos";
            this.btnPhotos.Click += new System.EventHandler(this.btnPhotos_Click);
            // 
            // gbAdvancedSearch
            // 
            this.gbAdvancedSearch.Controls.Add(this.gbProcessed);
            this.gbAdvancedSearch.Controls.Add(this.gbTime);
            this.gbAdvancedSearch.Controls.Add(this.groupBox1);
            this.gbAdvancedSearch.Location = new System.Drawing.Point(627, 113);
            this.gbAdvancedSearch.Name = "gbAdvancedSearch";
            this.gbAdvancedSearch.Size = new System.Drawing.Size(378, 149);
            this.gbAdvancedSearch.TabIndex = 2;
            this.gbAdvancedSearch.TabStop = false;
            this.gbAdvancedSearch.Tag = "FILTERABLE";
            this.gbAdvancedSearch.Text = "Advanced passes search";
            // 
            // gbProcessed
            // 
            this.gbProcessed.Controls.Add(this.rbAllProcess);
            this.gbProcessed.Controls.Add(this.rbObsolete);
            this.gbProcessed.Controls.Add(this.rbNo);
            this.gbProcessed.Controls.Add(this.rbYes);
            this.gbProcessed.Location = new System.Drawing.Point(242, 13);
            this.gbProcessed.Name = "gbProcessed";
            this.gbProcessed.Size = new System.Drawing.Size(131, 54);
            this.gbProcessed.TabIndex = 24;
            this.gbProcessed.TabStop = false;
            this.gbProcessed.Text = "Processed";
            // 
            // rbAllProcess
            // 
            this.rbAllProcess.Checked = true;
            this.rbAllProcess.Location = new System.Drawing.Point(6, 14);
            this.rbAllProcess.Name = "rbAllProcess";
            this.rbAllProcess.Size = new System.Drawing.Size(45, 16);
            this.rbAllProcess.TabIndex = 3;
            this.rbAllProcess.TabStop = true;
            this.rbAllProcess.Text = "All";
            // 
            // rbObsolete
            // 
            this.rbObsolete.AutoSize = true;
            this.rbObsolete.Location = new System.Drawing.Point(55, 34);
            this.rbObsolete.Name = "rbObsolete";
            this.rbObsolete.Size = new System.Drawing.Size(67, 17);
            this.rbObsolete.TabIndex = 2;
            this.rbObsolete.TabStop = true;
            this.rbObsolete.Text = "Obsolete";
            this.rbObsolete.UseVisualStyleBackColor = true;
            // 
            // rbNo
            // 
            this.rbNo.AutoSize = true;
            this.rbNo.Location = new System.Drawing.Point(55, 14);
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
            this.rbYes.Location = new System.Drawing.Point(6, 31);
            this.rbYes.Name = "rbYes";
            this.rbYes.Size = new System.Drawing.Size(43, 17);
            this.rbYes.TabIndex = 0;
            this.rbYes.TabStop = true;
            this.rbYes.Text = "Yes";
            this.rbYes.UseVisualStyleBackColor = true;
            // 
            // gbTime
            // 
            this.gbTime.Controls.Add(this.dtTo);
            this.gbTime.Controls.Add(this.lblEnd);
            this.gbTime.Controls.Add(this.lblStart);
            this.gbTime.Controls.Add(this.dtFrom);
            this.gbTime.Location = new System.Drawing.Point(242, 70);
            this.gbTime.Name = "gbTime";
            this.gbTime.Size = new System.Drawing.Size(131, 72);
            this.gbTime.TabIndex = 2;
            this.gbTime.TabStop = false;
            this.gbTime.Text = "Time";
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "HH:mm";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(55, 40);
            this.dtTo.Name = "dtTo";
            this.dtTo.ShowUpDown = true;
            this.dtTo.Size = new System.Drawing.Size(56, 20);
            this.dtTo.TabIndex = 4;
            this.dtTo.Value = new System.DateTime(2008, 3, 5, 23, 59, 0, 0);
            // 
            // lblEnd
            // 
            this.lblEnd.Location = new System.Drawing.Point(6, 40);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(40, 23);
            this.lblEnd.TabIndex = 3;
            this.lblEnd.Text = "End:";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStart
            // 
            this.lblStart.Location = new System.Drawing.Point(6, 17);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(40, 23);
            this.lblStart.TabIndex = 1;
            this.lblStart.Text = "Start:";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "HH:mm";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(55, 17);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.ShowUpDown = true;
            this.dtFrom.Size = new System.Drawing.Size(56, 20);
            this.dtFrom.TabIndex = 2;
            this.dtFrom.Value = new System.DateTime(2008, 3, 5, 0, 0, 0, 0);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gbBorderSel);
            this.groupBox1.Controls.Add(this.gbPassesNumType);
            this.groupBox1.Controls.Add(this.gbPassesAdv);
            this.groupBox1.Location = new System.Drawing.Point(7, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 130);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // gbBorderSel
            // 
            this.gbBorderSel.Controls.Add(this.cbFirstIN);
            this.gbBorderSel.Controls.Add(this.cbLastOUT);
            this.gbBorderSel.Location = new System.Drawing.Point(9, 58);
            this.gbBorderSel.Name = "gbBorderSel";
            this.gbBorderSel.Size = new System.Drawing.Size(212, 66);
            this.gbBorderSel.TabIndex = 2;
            this.gbBorderSel.TabStop = false;
            this.gbBorderSel.Text = "On border search";
            // 
            // cbFirstIN
            // 
            this.cbFirstIN.AutoSize = true;
            this.cbFirstIN.Checked = true;
            this.cbFirstIN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFirstIN.Location = new System.Drawing.Point(10, 20);
            this.cbFirstIN.Name = "cbFirstIN";
            this.cbFirstIN.Size = new System.Drawing.Size(59, 17);
            this.cbFirstIN.TabIndex = 1;
            this.cbFirstIN.Text = "First IN";
            this.cbFirstIN.UseVisualStyleBackColor = true;
            // 
            // cbLastOUT
            // 
            this.cbLastOUT.AutoSize = true;
            this.cbLastOUT.Checked = true;
            this.cbLastOUT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLastOUT.Location = new System.Drawing.Point(10, 43);
            this.cbLastOUT.Name = "cbLastOUT";
            this.cbLastOUT.Size = new System.Drawing.Size(72, 17);
            this.cbLastOUT.TabIndex = 2;
            this.cbLastOUT.Text = "Last OUT";
            this.cbLastOUT.UseVisualStyleBackColor = true;
            // 
            // gbPassesNumType
            // 
            this.gbPassesNumType.Controls.Add(this.lblPasses);
            this.gbPassesNumType.Controls.Add(this.nudMoreThan);
            this.gbPassesNumType.Controls.Add(this.lblMoreThan);
            this.gbPassesNumType.Controls.Add(this.chbIN);
            this.gbPassesNumType.Controls.Add(this.chbOUT);
            this.gbPassesNumType.Location = new System.Drawing.Point(8, 58);
            this.gbPassesNumType.Name = "gbPassesNumType";
            this.gbPassesNumType.Size = new System.Drawing.Size(212, 64);
            this.gbPassesNumType.TabIndex = 18;
            this.gbPassesNumType.TabStop = false;
            this.gbPassesNumType.Text = "Passes number and type";
            // 
            // lblPasses
            // 
            this.lblPasses.Location = new System.Drawing.Point(137, 14);
            this.lblPasses.Name = "lblPasses";
            this.lblPasses.Size = new System.Drawing.Size(61, 23);
            this.lblPasses.TabIndex = 19;
            this.lblPasses.Text = "passes";
            this.lblPasses.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudMoreThan
            // 
            this.nudMoreThan.Location = new System.Drawing.Point(71, 17);
            this.nudMoreThan.Name = "nudMoreThan";
            this.nudMoreThan.Size = new System.Drawing.Size(60, 20);
            this.nudMoreThan.TabIndex = 18;
            // 
            // lblMoreThan
            // 
            this.lblMoreThan.Location = new System.Drawing.Point(7, 14);
            this.lblMoreThan.Name = "lblMoreThan";
            this.lblMoreThan.Size = new System.Drawing.Size(75, 23);
            this.lblMoreThan.TabIndex = 5;
            this.lblMoreThan.Text = "More than:";
            this.lblMoreThan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chbIN
            // 
            this.chbIN.AutoSize = true;
            this.chbIN.Checked = true;
            this.chbIN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbIN.Location = new System.Drawing.Point(10, 43);
            this.chbIN.Name = "chbIN";
            this.chbIN.Size = new System.Drawing.Size(37, 17);
            this.chbIN.TabIndex = 1;
            this.chbIN.Text = "IN";
            this.chbIN.UseVisualStyleBackColor = true;
            // 
            // chbOUT
            // 
            this.chbOUT.AutoSize = true;
            this.chbOUT.Checked = true;
            this.chbOUT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbOUT.Location = new System.Drawing.Point(59, 43);
            this.chbOUT.Name = "chbOUT";
            this.chbOUT.Size = new System.Drawing.Size(49, 17);
            this.chbOUT.TabIndex = 2;
            this.chbOUT.Text = "OUT";
            this.chbOUT.UseVisualStyleBackColor = true;
            // 
            // gbPassesAdv
            // 
            this.gbPassesAdv.Controls.Add(this.rbWithout);
            this.gbPassesAdv.Controls.Add(this.rbAll);
            this.gbPassesAdv.Controls.Add(this.rbBorder);
            this.gbPassesAdv.Location = new System.Drawing.Point(8, 10);
            this.gbPassesAdv.Name = "gbPassesAdv";
            this.gbPassesAdv.Size = new System.Drawing.Size(212, 44);
            this.gbPassesAdv.TabIndex = 1;
            this.gbPassesAdv.TabStop = false;
            this.gbPassesAdv.Text = "Passes";
            // 
            // rbWithout
            // 
            this.rbWithout.Location = new System.Drawing.Point(142, 16);
            this.rbWithout.Name = "rbWithout";
            this.rbWithout.Size = new System.Drawing.Size(65, 24);
            this.rbWithout.TabIndex = 3;
            this.rbWithout.Text = "Without";
            this.rbWithout.CheckedChanged += new System.EventHandler(this.rbWithout_CheckedChanged);
            // 
            // rbAll
            // 
            this.rbAll.Checked = true;
            this.rbAll.Location = new System.Drawing.Point(6, 16);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(45, 24);
            this.rbAll.TabIndex = 1;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "All";
            this.rbAll.CheckedChanged += new System.EventHandler(this.rbAll_CheckedChanged);
            // 
            // rbBorder
            // 
            this.rbBorder.Location = new System.Drawing.Point(59, 16);
            this.rbBorder.Name = "rbBorder";
            this.rbBorder.Size = new System.Drawing.Size(75, 24);
            this.rbBorder.TabIndex = 2;
            this.rbBorder.Text = "First/Last";
            this.rbBorder.CheckedChanged += new System.EventHandler(this.rbBorder_CheckedChanged);
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(564, 552);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(75, 23);
            this.btnReport.TabIndex = 8;
            this.btnReport.Text = "Report";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // lvWithoutPasses
            // 
            this.lvWithoutPasses.FullRowSelect = true;
            this.lvWithoutPasses.GridLines = true;
            this.lvWithoutPasses.HideSelection = false;
            this.lvWithoutPasses.Location = new System.Drawing.Point(12, 301);
            this.lvWithoutPasses.Name = "lvWithoutPasses";
            this.lvWithoutPasses.Size = new System.Drawing.Size(997, 216);
            this.lvWithoutPasses.TabIndex = 3;
            this.lvWithoutPasses.UseCompatibleStateImageBehavior = false;
            this.lvWithoutPasses.View = System.Windows.Forms.View.Details;
            this.lvWithoutPasses.SelectedIndexChanged += new System.EventHandler(this.lvWithoutPasses_SelectedIndexChanged);
            this.lvWithoutPasses.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvWithoutPasses_ColumnClick);
            // 
            // btnWithoutNext
            // 
            this.btnWithoutNext.Location = new System.Drawing.Point(972, 268);
            this.btnWithoutNext.Name = "btnWithoutNext";
            this.btnWithoutNext.Size = new System.Drawing.Size(32, 23);
            this.btnWithoutNext.TabIndex = 14;
            this.btnWithoutNext.Text = ">";
            this.btnWithoutNext.Click += new System.EventHandler(this.btnWithoutNext_Click);
            // 
            // btnWithoutPrev
            // 
            this.btnWithoutPrev.Location = new System.Drawing.Point(932, 268);
            this.btnWithoutPrev.Name = "btnWithoutPrev";
            this.btnWithoutPrev.Size = new System.Drawing.Size(32, 23);
            this.btnWithoutPrev.TabIndex = 13;
            this.btnWithoutPrev.Text = "<";
            this.btnWithoutPrev.Click += new System.EventHandler(this.btnWithoutPrev_Click);
            // 
            // btnHistoryOfChange
            // 
            this.btnHistoryOfChange.Location = new System.Drawing.Point(409, 552);
            this.btnHistoryOfChange.Name = "btnHistoryOfChange";
            this.btnHistoryOfChange.Size = new System.Drawing.Size(130, 23);
            this.btnHistoryOfChange.TabIndex = 15;
            this.btnHistoryOfChange.Text = "History of change";
            this.btnHistoryOfChange.UseVisualStyleBackColor = true;
            this.btnHistoryOfChange.Click += new System.EventHandler(this.btnHistoryOfChange_Click);
            // 
            // lblIsWrkHrs
            // 
            this.lblIsWrkHrs.AutoSize = true;
            this.lblIsWrkHrs.Location = new System.Drawing.Point(13, 523);
            this.lblIsWrkHrs.Name = "lblIsWrkHrs";
            this.lblIsWrkHrs.Size = new System.Drawing.Size(88, 13);
            this.lblIsWrkHrs.TabIndex = 16;
            this.lblIsWrkHrs.Text = "*Is working hours";
            // 
            // lblManualCreated
            // 
            this.lblManualCreated.AutoSize = true;
            this.lblManualCreated.Location = new System.Drawing.Point(213, 523);
            this.lblManualCreated.Name = "lblManualCreated";
            this.lblManualCreated.Size = new System.Drawing.Size(89, 13);
            this.lblManualCreated.TabIndex = 17;
            this.lblManualCreated.Text = "**Manual created";
            this.lblManualCreated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(628, 13);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 91);
            this.gbFilter.TabIndex = 28;
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
            // Passes
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1016, 587);
            this.ControlBox = false;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.lblManualCreated);
            this.Controls.Add(this.lblIsWrkHrs);
            this.Controls.Add(this.btnHistoryOfChange);
            this.Controls.Add(this.btnWithoutNext);
            this.Controls.Add(this.btnWithoutPrev);
            this.Controls.Add(this.lvWithoutPasses);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.gbAdvancedSearch);
            this.Controls.Add(this.btnPhotos);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvPasses);
            this.Controls.Add(this.gbPasses);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Passes";
            this.ShowInTaskbar = false;
            this.Text = "Passes";
            this.Load += new System.EventHandler(this.Passes_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Passes_KeyUp);
            this.gbPasses.ResumeLayout(false);
            this.gbAdvancedSearch.ResumeLayout(false);
            this.gbProcessed.ResumeLayout(false);
            this.gbProcessed.PerformLayout();
            this.gbTime.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.gbBorderSel.ResumeLayout(false);
            this.gbBorderSel.PerformLayout();
            this.gbPassesNumType.ResumeLayout(false);
            this.gbPassesNumType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMoreThan)).EndInit();
            this.gbPassesAdv.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        void cbEmployee_Leave(object sender, EventArgs e)
        {
            if (cbEmployee.SelectedIndex == -1) {
                cbEmployee.SelectedIndex = 0;
            }
        }
		#endregion

		#region Inner Class for sorting Array List of Passes

		/*
		 *  Class used for sorting Array List of Passes
		*/

		private class ArrayListSort:IComparer<PassTO>
		{        
			private int compOrder;        
			private int compField;
			public ArrayListSort(int sortOrder, int sortField)        
			{            
				compOrder = sortOrder;
				compField = sortField;
			}        
			
			public int Compare(PassTO x, PassTO y)        
			{
				PassTO pass1 = null;
				PassTO pass2 = null;

				if (compOrder == Constants.sortAsc)
				{
					pass1 = x;
					pass2 = y;
				}
				else
				{
					pass1 = y;
					pass2 = x;
				}

				switch(compField)            
				{                
					case Passes.PassIDIndex:
                        if (pass1.PassID == pass2.PassID)
                            return pass1.EventTime.CompareTo(pass2.EventTime);
                        else
                            return pass1.PassID.CompareTo(pass2.PassID);
					case Passes.EmployeeIDIndex:
                        if (pass1.EmployeeName.Equals(pass2.EmployeeName))
                            return pass1.EventTime.CompareTo(pass2.EventTime);
                        else
                            return pass1.EmployeeName.CompareTo(pass2.EmployeeName);
					case Passes.DirectionIndex:
                        if (pass1.Direction.Equals(pass2.Direction))
                            return pass1.EventTime.CompareTo(pass2.EventTime);
                        else
                            return pass1.Direction.CompareTo(pass2.Direction);
					case Passes.EventTimeIndex:
						return pass1.EventTime.CompareTo(pass2.EventTime);
					case Passes.PassTypeIDIndex:
                        if (pass1.PassType == pass2.PassType)
                            return pass1.EventTime.CompareTo(pass2.EventTime);
                        else
                            return pass1.PassType.CompareTo(pass2.PassType);
					case Passes.IsWrkHrsIndex:
                        if (pass1.IsWrkHrsCount == pass2.IsWrkHrsCount)
                            return pass1.EventTime.CompareTo(pass2.EventTime);
                        else
                            return pass1.IsWrkHrsCount.CompareTo(pass2.IsWrkHrsCount);
					case Passes.LocationIDIndex:
                        if (pass1.LocationName.Equals(pass2.LocationName))
                            return pass1.EventTime.CompareTo(pass2.EventTime);
                        else
                            return pass1.LocationName.CompareTo(pass2.LocationName);
                    case Passes.GateIndex:
                        if (pass1.GateID == pass2.GateID)
                            return pass1.EventTime.CompareTo(pass2.EventTime);
                        else
                            return pass1.GateID.CompareTo(pass2.GateID);
					case Passes.PairGenUsedIndex:
                        if (pass1.PairGenUsed == pass2.PairGenUsed)
                            return pass1.EventTime.CompareTo(pass2.EventTime);
                        else
                            return pass1.PairGenUsed.CompareTo(pass2.PairGenUsed);
					case Passes.ManualCreatedIndex:
                        if (pass1.ManualyCreated == pass2.ManualyCreated)
                            return pass1.EventTime.CompareTo(pass2.EventTime);
                        else
                            return pass1.ManualyCreated.CompareTo(pass2.ManualyCreated);
					default:                    
						return pass1.EventTime.CompareTo(pass2.EventTime);
				}        
			}    
		}

		#endregion

        #region Inner Class2 for sorting items in View List

        /*
		 *  Class used for sorting items in the List View 
		 */
        private class ArrayListSort2 : IComparer<EmployeeTO>
        {
            private int compOrder;
            private int compField;

            public ArrayListSort2(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(EmployeeTO x, EmployeeTO y)
            {
                EmployeeTO employee1 = null;
                EmployeeTO employee2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    employee1 = x;
                    employee2 = y;
                }
                else
                {
                    employee1 = y;
                    employee2 = x;
                }

                switch (compField)
                {
                    case Passes.EmployeeIDIndex2:
                        {
                            return CaseInsensitiveComparer.Default.Compare(employee1.EmployeeID, employee2.EmployeeID);
                        }
                    case Passes.EmployeeNameIndex2:
                        {
                            return CaseInsensitiveComparer.Default.Compare(employee1.LastName.Trim(), employee2.LastName.Trim());
                        }
                    case Passes.WUNameIndex2:
                        {
                            return CaseInsensitiveComparer.Default.Compare(employee1.WorkingUnitName.Trim(), employee2.WorkingUnitName.Trim());
                        }
                    case Passes.DateIndex2:
                        {
                            return CaseInsensitiveComparer.Default.Compare(employee1.ScheduleDate, employee2.ScheduleDate);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");
                }
            }
        }

        #endregion

		/// <summary>
		/// Set proper language.
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// button's text
				btnAdd.Text = rm.GetString("btnAdd", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnDelete.Text = rm.GetString("btnDelete", culture);
				btnSearch.Text = rm.GetString("btnSearch", culture);
				btnClose.Text = rm.GetString("btnClose", culture);
                btnPhotos.Text = rm.GetString("btnPhotos", culture);
                btnReport.Text = rm.GetString("btnReport", culture);
                btnAdvanced.Text = rm.GetString("btnAdvanced1", culture);
                btnHistoryOfChange.Text = rm.GetString("btnHistoryOfChange", culture);
				
				// Form name
				this.Text = rm.GetString("menuPasses", culture);
				
				// group box text
				this.gbPasses.Text = rm.GetString("gbPasses", culture);
                gbAdvancedSearch.Text = rm.GetString("gbAdvancedSearch", culture);
                gbTime.Text = rm.GetString("gbTime", culture);
                gbPassesAdv.Text = rm.GetString("gbPassesAdv", culture);
                gbBorderSel.Text = rm.GetString("gbBorderSel", culture);
                gbPassesNumType.Text = rm.GetString("gbPassesNumType", culture);
                gbProcessed.Text = rm.GetString("gbProcessed", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);

                // radio buttons text
                rbAll.Text = rm.GetString("rbAll", culture);
                rbBorder.Text = rm.GetString("rbBorder", culture);
                rbWithout.Text = rm.GetString("rbWithout1", culture);
                rbAllProcess.Text = rm.GetString("rbAll", culture);
                rbNo.Text = rm.GetString("no", culture);
                rbYes.Text = rm.GetString("yes", culture);
                rbObsolete.Text = rm.GetString("obsolete", culture);

                // check box text
                cbFirstIN.Text = rm.GetString("cbFirstIN", culture);
                cbLastOUT.Text = rm.GetString("cbLastOUT", culture);
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);
                
				// label's text
				lblWU.Text = rm.GetString("lblWU", culture);
				lblEmployee.Text = rm.GetString("lblEmployee", culture);
				lblDirection.Text = rm.GetString("lblPrimDirect", culture);
				lblPassType.Text = rm.GetString("lblPassType", culture);
				lblLocation.Text = rm.GetString("lblLocation", culture);
				lblFrom.Text = rm.GetString("lblFrom", culture);
				lblTo.Text = rm.GetString("lblTo", culture);
                lblStart.Text = rm.GetString("lblFrom", culture);
                lblEnd.Text = rm.GetString("lblTo", culture);
                lblIsWrkHrs.Text = rm.GetString("lblIsWH", culture);
                lblManualCreated.Text = rm.GetString("lblManualCreated", culture);
                lblMoreThan.Text = rm.GetString("lblMoreThan", culture);
                lblPasses.Text = rm.GetString("lblPasses", culture);
                lblGate.Text = rm.GetString("lblGate", culture);

				// list view initialization
				lvPasses.BeginUpdate();
				lvPasses.Columns.Add(rm.GetString("hdrPassID", culture), (lvPasses.Width - 10) / 10 - 20, HorizontalAlignment.Left);
				lvPasses.Columns.Add(rm.GetString("hdrEmployee", culture), (lvPasses.Width - 10) / 10 + 50, HorizontalAlignment.Left);
				lvPasses.Columns.Add(rm.GetString("hdrDirection", culture), (lvPasses.Width - 10) / 10 - 35, HorizontalAlignment.Left);
				lvPasses.Columns.Add(rm.GetString("hdrEventTime", culture), (lvPasses.Width - 10) / 10 + 25, HorizontalAlignment.Left);
				lvPasses.Columns.Add(rm.GetString("hdrPassType", culture), (lvPasses.Width - 10) / 10 + 15, HorizontalAlignment.Left);
				lvPasses.Columns.Add(rm.GetString("hdrIsWrkHrs", culture), (lvPasses.Width - 10) / 10 - 40, HorizontalAlignment.Left);
				lvPasses.Columns.Add(rm.GetString("hdrLocation", culture), (lvPasses.Width - 10) / 10 + 35, HorizontalAlignment.Left);
				lvPasses.Columns.Add(rm.GetString("hdrGate", culture), (lvPasses.Width - 10) / 10, HorizontalAlignment.Left);
                lvPasses.Columns.Add(rm.GetString("hdrPairGenUsed", culture), (lvPasses.Width - 10) / 10 - 20, HorizontalAlignment.Left);
				lvPasses.Columns.Add(rm.GetString("hdrManualCreated", culture), (lvPasses.Width - 10) / 10 - 30, HorizontalAlignment.Left);
				lvPasses.EndUpdate();

                lvWithoutPasses.BeginUpdate();
                lvWithoutPasses.Columns.Add(rm.GetString("hdrEmployeeID", culture), lvWithoutPasses.Width / 6 - 5, HorizontalAlignment.Left);
                lvWithoutPasses.Columns.Add(rm.GetString("hdrName", culture), lvWithoutPasses.Width / 3, HorizontalAlignment.Left);
                lvWithoutPasses.Columns.Add(rm.GetString("hdrWorkingUnit", culture), lvWithoutPasses.Width / 3, HorizontalAlignment.Left);
                lvWithoutPasses.Columns.Add(rm.GetString("hdrDate", culture), lvWithoutPasses.Width / 6, HorizontalAlignment.Left);
                lvWithoutPasses.EndUpdate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.setLanguage(): " + ex.Message + "\n");
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
				log.writeLog(DateTime.Now + " Passes.populateWorkingUnitCombo(): " + ex.Message + "\n");
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
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.populateEmployeeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate Pass Type Combo Box
		/// </summary>
		private void populatePassTypeCombo()
		{
			try
			{
				PassType pt = new PassType();
                pt.PTypeTO.IsPass = Constants.passOnReader;
				List<PassTypeTO> ptArray = pt.Search();
				ptArray.Insert(0, new PassTypeTO(-1, rm.GetString("all", culture), 0, 0, ""));

				cbPassType.DataSource = ptArray;
				cbPassType.DisplayMember = "Description";
				cbPassType.ValueMember = "PassTypeID";
				cbPassType.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.populatePassTypeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate Direction Combo Box
		/// </summary>
		private void populateDirectionCombo()
		{
			try
			{
				cbDirection.Items.Add(rm.GetString("all", culture));
				cbDirection.Items.Add(Constants.DirectionIn);
				cbDirection.Items.Add(Constants.DirectionOut);
				//cbDirection.Items.Add(Constants.DirectionInOut);

				cbDirection.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.populateDirectionCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate Location Combo Box
		/// </summary>
		private void populateLocationCombo()
		{
			try
			{
				Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
				locArray = loc.Search();
				locArray.Insert(0, new LocationTO(-1, rm.GetString("all", culture), rm.GetString("all", culture), 0, 0, ""));

				cbLocation.DataSource = locArray;
				cbLocation.DisplayMember = "Name";
				cbLocation.ValueMember = "LocationID";
				cbLocation.SelectedIndex = 0;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.populateWorkingUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
			}
		}

        /// <summary>
        /// Populate Gate Combo Box
        /// </summary>
        private void populateGateCombo()
        {
            try
            {
                List<GateTO> gateArray = new Gate().Search();
                gateArray.Insert(0, new GateTO(-1, rm.GetString("all", culture), rm.GetString("all", culture), new DateTime(), -1, -1));

                cbGate.DataSource = gateArray;
                cbGate.DisplayMember = "Name";
                cbGate.ValueMember = "GateID";
                cbGate.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateWorkingUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

		/// <summary>
		/// Populate List View with Passes found
		/// </summary>
		/// <param name="locationsList"></param>
		private void populateListView(List<PassTO> passesList, int startIndex)
		{
			try
			{
				if (passesList.Count > Constants.recordsPerPage)
				{
					btnPrev.Visible = true;
					btnNext.Visible = true;
				}
				else
				{
					btnPrev.Visible = false;
					btnNext.Visible = false;
				}

				lvPasses.BeginUpdate();
				lvPasses.Items.Clear();

				if (passesList.Count > 0)
				{
					if ((startIndex >= 0) && (startIndex < passesList.Count))
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
						if (lastIndex >= passesList.Count)
						{
							btnNext.Enabled = false;
							lastIndex = passesList.Count;
						}
						else
						{
							btnNext.Enabled = true;
						}

						for (int i = startIndex; i < lastIndex; i++)
						{
							PassTO pass = passesList[i];
							ListViewItem item = new ListViewItem();
							item.Text = pass.PassID.ToString().Trim();
							item.SubItems.Add(pass.EmployeeName.Trim());
							item.SubItems.Add(pass.Direction.Trim());
							if (!pass.EventTime.Date.Equals(new DateTime(1,1,1,0,0,0)))
							{
								item.SubItems.Add(pass.EventTime.ToString("dd/MM/yyyy   HH:mm"));
							}
							else
							{								
								item.SubItems.Add("");
							}
							item.SubItems.Add(pass.PassType.Trim());
							if (pass.IsWrkHrsCount == (int) Constants.IsWrkCount.IsCounter)
							{
								item.SubItems.Add(rm.GetString("yes", culture));
							}
							else
							{
								item.SubItems.Add(rm.GetString("no", culture));
							}
							item.SubItems.Add(pass.LocationName.Trim());

                            if (gates.ContainsKey(pass.GateID))
                            {
                                item.SubItems.Add(gates[pass.GateID].ToString().Trim());
                            }
                            else
                            {
                                item.SubItems.Add("N/A");
                            }

							if (pass.PairGenUsed == (int) Constants.PairGenUsed.Used)
							{
								item.SubItems.Add(rm.GetString("yes", culture));
							}
							else if(pass.PairGenUsed == (int)Constants.PairGenUsed.Unused)
							{
								item.SubItems.Add(rm.GetString("no", culture));
							}
                            else if (pass.PairGenUsed == (int)Constants.PairGenUsed.Obsolete)
                            {
                                item.SubItems.Add(rm.GetString("obsolete", culture));
                            }
							if (pass.ManualyCreated == (int) Constants.recordCreated.Manualy)
							{
								item.SubItems.Add(rm.GetString("yes", culture));
							}
							else
							{
								item.SubItems.Add(rm.GetString("no", culture));
							}
                            item.Tag = pass;
						
							lvPasses.Items.Add(item);
						}
					}
				}

				lvPasses.EndUpdate();
				lvPasses.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.populateListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

        private void populateWithoutListView(List<EmployeeTO> employeeList, int startIndex)
        {
            try
            {
                if (employeeList.Count > Constants.recordsPerPage)
                {
                    btnWithoutPrev.Visible = true;
                    btnWithoutNext.Visible = true;
                }
                else
                {
                    btnWithoutPrev.Visible = false;
                    btnWithoutNext.Visible = false;
                }

                lvWithoutPasses.BeginUpdate();
                lvWithoutPasses.Items.Clear();

                if (employeeList.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < employeeList.Count))
                    {
                        if (startIndex == 0)
                        {
                            btnWithoutPrev.Enabled = false;
                        }
                        else
                        {
                            btnWithoutPrev.Enabled = true;
                        }

                        int lastIndex = startIndex + Constants.recordsPerPage;
                        if (lastIndex >= employeeList.Count)
                        {
                            btnWithoutNext.Enabled = false;
                            lastIndex = employeeList.Count;
                        }
                        else
                        {
                            btnWithoutNext.Enabled = true;
                        }

                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            EmployeeTO employee = employeeList[i];
                            ListViewItem item = new ListViewItem();
                            item.Text = employee.EmployeeID.ToString();
                            item.SubItems.Add(employee.LastName);
                            item.SubItems.Add(employee.WorkingUnitName);
                            item.SubItems.Add(employee.ScheduleDate.ToString("dd/MM/yyyy"));

                            lvWithoutPasses.Items.Add(item);
                        }
                    }
                }

                lvWithoutPasses.EndUpdate();
                lvWithoutPasses.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateWithoutListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

		/// <summary>
		/// Clears Form
		/// </summary>
		private void clearForm()
		{
			try
			{
				this.cbWU.SelectedIndex = 0;
				this.cbEmployee.SelectedIndex = 0;
				this.cbPassType.SelectedIndex = 0;
				this.cbDirection.SelectedIndex = 0;
				this.cbLocation.SelectedIndex = 0;
				this.dtpFrom.Value = DateTime.Today;
				this.dtpTo.Value = DateTime.Today;
				this.lblTotal.Visible=false;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.clearForm(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void clearListView()
		{
			lvPasses.BeginUpdate();
			lvPasses.Items.Clear();
			lvPasses.EndUpdate();

			lvPasses.Invalidate();

			btnPrev.Visible = false;
			btnNext.Visible = false;

            passesListCount = 0;
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
                log.writeLog(DateTime.Now + " Passes.btnClose_Click(): " + ex.Message + "\n");
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
                
                if (wuString.Equals(""))
                {
                    MessageBox.Show(rm.GetString("noEmplPassPrivilege", culture));
                    return;
                }

                if (cbEmployee.Items.Count == 0)
                {
                    MessageBox.Show(rm.GetString("noPassesFound", culture));
                    return;
                }

                bool advancedSearch = btnAdvanced.Text.Equals(rm.GetString("btnAdvanced2", culture));
                if (advancedSearch && (dtFrom.Value.TimeOfDay >= dtTo.Value.TimeOfDay))
                {
                    MessageBox.Show(rm.GetString("msgToTimeLessThenFromTime", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                int emplID = -1;
                if (cbEmployee.SelectedIndex >= 0 && (int)cbEmployee.SelectedValue >= 0)
                {
                    emplID = (int)cbEmployee.SelectedValue;
                }

                string direction = "";
                if (cbDirection.SelectedIndex > 0)
                {
                    direction = cbDirection.SelectedItem.ToString();
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
                
                if (advancedSearch && rbBorder.Checked && (!cbFirstIN.Checked) && (!cbLastOUT.Checked))
                {
                    MessageBox.Show(rm.GetString("msgFirstLastNotCheck", culture));
                    return;
                }

                Pass pass1 = new Pass();
                pass1.PssTO.EmployeeID = emplID;
                pass1.PssTO.Direction = direction;
                pass1.PssTO.PassTypeID = (int)cbPassType.SelectedValue;
                pass1.PssTO.LocationID = (int)cbLocation.SelectedValue;
                pass1.PssTO.GateID = (int) cbGate.SelectedValue;
                int count = pass1.SearchIntervalCount(dtpFrom.Value.Date, dtpTo.Value.Date, selectedWU, dtFrom.Value, dtTo.Value);

                if (count > Constants.maxRecords)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("passesGreaterThenAllowed", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        currentPassesList = pass1.SearchInterval(dtpFrom.Value.Date, dtpTo.Value.Date, selectedWU, dtFrom.Value, dtTo.Value);
                    }
                    else
                    {
                        currentPassesList.Clear();
                        clearListView();
                    }
                }
                else
                {
                    if (count > 0)
                    {
                        currentPassesList = pass1.SearchInterval(dtpFrom.Value.Date, dtpTo.Value.Date, selectedWU, dtFrom.Value, dtTo.Value);
                    }
                    else
                    {
                        currentPassesList.Clear();
                        clearListView();
                    }
                }
                //currentPassesList is sorted by employee_id, event_time
                if (advancedSearch && (!rbAllProcess.Checked))
                {
                    List<PassTO> tempList = new List<PassTO>();

                    foreach (PassTO pass in currentPassesList)
                    {                        
                        if (rbYes.Checked && pass.PairGenUsed == (int)Constants.PairGenUsed.Used)
                            tempList.Add(pass);
                        if (rbNo.Checked && pass.PairGenUsed == (int)Constants.PairGenUsed.Unused)
                            tempList.Add(pass);
                        if (rbObsolete.Checked && pass.PairGenUsed == (int)Constants.PairGenUsed.Obsolete)
                            tempList.Add(pass);
                    }

                    if (tempList.Count > 0)
                    {
                        currentPassesList.Clear();
                        currentPassesList = tempList;
                    }
                    else
                    {
                        currentPassesList.Clear();
                        clearListView();
                    }
                } //if (advancedSearch && (!rbAllProcess.Checked))

                //currentPassesList is sorted by employee_id, event_time
                if (advancedSearch && rbBorder.Checked && (currentPassesList.Count > 1))
                {
                    List<PassTO> tempList = new List<PassTO>();

                    PassTO currPass = currentPassesList[0];
                    PassTO currentINPass = new PassTO();
                    PassTO currentOUTPass = new PassTO();

                    if (currPass.Direction.Equals(Constants.DirectionIn))
                        currentINPass = currPass;
                    else if (currPass.Direction.Equals(Constants.DirectionOut))
                        currentOUTPass = currPass;

                    for (int i = 1; i < currentPassesList.Count; i++)
                    {
                        PassTO tempPass = currentPassesList[i];

                        if ((currPass.EmployeeID != tempPass.EmployeeID)
                            || ((currPass.EmployeeID == tempPass.EmployeeID) && (currPass.EventTime.Date != tempPass.EventTime.Date)))
                        {
                            if (cbFirstIN.Checked && (currentINPass.EmployeeID != -1))
                                tempList.Add(currentINPass);
                            if (cbLastOUT.Checked && (currentOUTPass.EmployeeID != -1))
                                tempList.Add(currentOUTPass);

                            currPass = tempPass;
                            if (currPass.Direction.Equals(Constants.DirectionIn))
                                currentINPass = currPass;
                            else
                                currentINPass = new PassTO();
                            if (currPass.Direction.Equals(Constants.DirectionOut))
                                currentOUTPass = currPass;
                            else
                                currentOUTPass = new PassTO();
                        }
                        else
                        {
                            if ((tempPass.Direction.Equals(Constants.DirectionIn))
                                && ((currentINPass.EmployeeID == -1)
                                    || (currentINPass.EventTime > tempPass.EventTime)))
                            {
                                currentINPass = tempPass;
                            }

                            if ((tempPass.Direction.Equals(Constants.DirectionOut))
                                && ((currentOUTPass.EmployeeID == -1)
                                    || (currentOUTPass.EventTime < tempPass.EventTime)))
                            {
                                currentOUTPass = tempPass;
                            }
                        }

                        if (i == (currentPassesList.Count - 1))
                        {
                            if (cbFirstIN.Checked && (currentINPass.EmployeeID != -1))
                                tempList.Add(currentINPass);
                            if (cbLastOUT.Checked && (currentOUTPass.EmployeeID != -1))
                                tempList.Add(currentOUTPass);
                        }
                    }

                    if (tempList.Count > 0)
                    {
                        currentPassesList.Clear();
                        currentPassesList = tempList;
                    }
                    else
                    {
                        currentPassesList.Clear();
                        clearListView();
                    }
                } //if (advancedSearch && rbBorder.Checked && (currentPassesList.Count > 1))
                
                if (advancedSearch && rbAll.Checked)
                {
                    if (currentPassesList.Count > 0)
                    {
                        //key is employeeID value is arrayList Of passes
                        Dictionary<int, List<PassTO>> passesIN = new Dictionary<int,List<PassTO>>();
                        Dictionary<int, List<PassTO>> passesOUT = new Dictionary<int, List<PassTO>>();
                        ArrayList employees = new ArrayList();

                        foreach (PassTO pass in currentPassesList)
                        {
                            if (!passesIN.ContainsKey(pass.EmployeeID))
                            {
                                passesIN.Add(pass.EmployeeID, new List<PassTO>());
                            }
                            if (!passesOUT.ContainsKey(pass.EmployeeID))
                            {
                                passesOUT.Add(pass.EmployeeID, new List<PassTO>());
                            }
                            if (pass.Direction.Equals(Constants.DirectionIn))
                            {
                                passesIN[pass.EmployeeID].Add(pass);
                            }
                            if (pass.Direction.Equals(Constants.DirectionOut))
                            {
                                passesOUT[pass.EmployeeID].Add(pass);
                            }
                            if (!employees.Contains(pass.EmployeeID))
                            {
                                employees.Add(pass.EmployeeID);
                            }
                        }

                        currentPassesList = new List<PassTO>();

                        if (chbIN.Checked)
                        {
                            foreach (int employeeID in employees)
                            {
                                List<PassTO> employeePasses = passesIN[employeeID];
                                if (employeePasses.Count > (int)nudMoreThan.Value)
                                {
                                    foreach (PassTO p in employeePasses)
                                    {
                                        currentPassesList.Add(p);
                                    }
                                }
                            }//foreach (int employeeID in employees)
                        }//if (chbIN.Checked)

                        if (chbOUT.Checked)
                        {
                            foreach (int employeeID in employees)
                            {
                                List<PassTO> employeePasses = passesOUT[employeeID];
                                if (employeePasses.Count > (int)nudMoreThan.Value)
                                {
                                    foreach (PassTO p in employeePasses)
                                    {
                                        currentPassesList.Add(p);
                                    }
                                }
                            }//foreach (int employeeID in employees)
                        }//if (chbOUT.Checked)

                        if (!chbIN.Checked && !chbOUT.Checked)
                        {
                            foreach (int employeeID in employees)
                            {
                                List<PassTO> employeePassesIN = passesIN[employeeID];
                                List<PassTO> employeePassesOUT = passesOUT[employeeID];
                                if ((employeePassesIN.Count + employeePassesOUT.Count) > (int)nudMoreThan.Value)
                                {
                                    foreach (PassTO p in employeePassesIN)
                                    {
                                        currentPassesList.Add(p);
                                    }
                                    foreach (PassTO p in employeePassesOUT)
                                    {
                                        currentPassesList.Add(p);
                                    }
                                }
                            }//foreach (int employeeID in employees)
                        }//if (!chbIN.Checked&&!chbOUT.Checked)

                        passesListCount = currentPassesList.Count;
                        if (currentPassesList.Count > 0)
                        {
                            startIndex = 0;
                            currentPassesList.Sort(new ArrayListSort(sortOrder, sortField));
                            populateListView(currentPassesList, startIndex);
                            this.lblTotal.Visible = true;
                            this.lblTotal.Text = rm.GetString("lblTotal", culture) + currentPassesList.Count.ToString().Trim();
                        }
                        else //else if (count == 0)
                        {
                            //MessageBox.Show(rm.GetString("noPassesFound", culture));
                            clearListView();
                            this.lblTotal.Visible = true;
                            this.lblTotal.Text = rm.GetString("lblTotal", culture) + "0";
                        }

                        currentEmployeeList.Clear();
                        clearWithoutListView();
                    }
                }//if (advancedSearch && rbAll.Checked)

                else
                {
                    //currentPass.Clear();
                    passesListCount = currentPassesList.Count;
                    if (currentPassesList.Count > 0)
                    {
                        startIndex = 0;
                        currentPassesList.Sort(new ArrayListSort(sortOrder, sortField));
                        populateListView(currentPassesList, startIndex);
                        this.lblTotal.Visible = true;
                        this.lblTotal.Text = rm.GetString("lblTotal", culture) + currentPassesList.Count.ToString().Trim();
                    }
                    else //else if (count == 0)
                    {
                        //MessageBox.Show(rm.GetString("noPassesFound", culture));
                        clearListView();
                        this.lblTotal.Visible = true;
                        this.lblTotal.Text = rm.GetString("lblTotal", culture) + "0";
                    }

                    currentEmployeeList.Clear();
                    clearWithoutListView();
                }

                if (advancedSearch && rbWithout.Checked)
                {
                    Hashtable emplAndDays = new Hashtable();
                    foreach (PassTO pass in currentPassesList)
                    {
                        if (!emplAndDays.ContainsKey(pass.EmployeeID))
                        {
                            emplAndDays.Add(pass.EmployeeID, new Hashtable());
                        }

                        Hashtable datesForEmpl = (Hashtable)emplAndDays[pass.EmployeeID];
                        if (!datesForEmpl.ContainsKey(pass.EventTime.Date))
                        {
                            datesForEmpl.Add(pass.EventTime.Date, "");
                        }
                    }

                    currentEmployeeList.Clear();
                    List<EmployeeTO> allEmployeesList = new List<EmployeeTO>();
                    if (emplID != -1)
                    {
                        Employee employee = new Employee();
                        employee.EmplTO.EmployeeID = emplID;
                        allEmployeesList = employee.SearchWithStatuses(statuses, "");
                    }
                    else
                    {
                        allEmployeesList = (new Employee()).SearchByWUWithStatuses(selectedWU, statuses);
                    }
                    for (DateTime day = dtpFrom.Value.Date; day <= dtpTo.Value.Date; day = day.AddDays(1))
                    {
                        foreach (EmployeeTO employee in allEmployeesList)
                        {
                            bool exist = false;
                            if (emplAndDays.ContainsKey(employee.EmployeeID))
                            {
                                Hashtable employeeDates = (Hashtable)emplAndDays[employee.EmployeeID];

                                if (employeeDates.ContainsKey(day.Date))
                                {
                                    exist = true;
                                }
                            }

                            if (!exist)
                            {
                                EmployeeTO currEmpl = new EmployeeTO();
                                currEmpl.EmployeeID = employee.EmployeeID;
                                currEmpl.LastName = employee.LastName + " " + employee.FirstName;
                                currEmpl.WorkingUnitName = employee.WorkingUnitName;
                                currEmpl.WorkingUnitID = employee.WorkingUnitID;
                                currEmpl.ScheduleDate = day;

                                currentEmployeeList.Add(currEmpl);
                            }
                        }
                    }

                    withoutListCount = currentEmployeeList.Count;
                    if (currentEmployeeList.Count > 0)
                    {
                        startIndexWithout = 0;
                        currentEmployeeList.Sort(new ArrayListSort2(sortOrderWithout, sortFieldWithout));
                        populateWithoutListView(currentEmployeeList, startIndexWithout);
                        this.lblTotal.Visible = true;
                        this.lblTotal.Text = rm.GetString("lblTotal", culture) + currentEmployeeList.Count.ToString().Trim();
                    }
                    else // (count == 0)
                    {
                        clearWithoutListView();
                        this.lblTotal.Visible = true;
                        this.lblTotal.Text = rm.GetString("lblTotal", culture) + "0";
                    }

                    currentPassesList.Clear();
                    clearListView();
                } //if (advancedSearch && rbWithout.Checked)

                
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.btnSearch_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		

		private void lvPasses_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
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

				currentPassesList.Sort(new ArrayListSort(sortOrder, sortField));
				startIndex = 0;
				populateListView(currentPassesList, startIndex);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.lvPasses_ColumnClick(): " + ex.Message + "\n");
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

                if (lvPasses.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noSelPassDel", culture));
                }
                else
                {

                    bool isDeleted = true;
                    if (lvPasses.SelectedItems.Count > 1)
                    {
                        DialogResult result = MessageBox.Show(rm.GetString("deletePasses", culture), "", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {
                            foreach (ListViewItem item in lvPasses.SelectedItems)
                            {
                                int saved = 0;
                                foreach (PassTO pass in currentPassesList)
                                {

                                    if (pass.PassID.ToString() == item.Text)
                                    {
                                        currentPassHist = new PassHistTO(-1, pass.PassID, pass.EmployeeID, pass.Direction, pass.EventTime, pass.PassTypeID, pass.IsWrkHrsCount, pass.LocationID,
                                            pass.PairGenUsed, pass.ManualyCreated, Constants.PassDeleted, pass.CreatedBy, pass.CreatedTime, "", new DateTime());
                                        break;
                                    }
                                }

                                PassHist ph = new PassHist();
                                bool trans = ph.BeginTransaction();

                                if (trans)
                                {
                                    try
                                    {
                                        saved = ph.Save(currentPassHist.PassID, currentPassHist.EmployeeID, currentPassHist.Direction, currentPassHist.EventTime, currentPassHist.PassTypeID, currentPassHist.IsWrkHrsCount, currentPassHist.LocationID, currentPassHist.PairGenUsed, currentPassHist.ManualyCreated, currentPassHist.Remarks, currentPassHist.CreatedBy, currentPassHist.CreatedTime, false);

                                        if (saved > 0)
                                        {
                                            PassTO p = (PassTO)item.Tag;
                                            Pass pass = new Pass();
                                            pass.SetTransaction(ph.GetTransaction());
                                            isDeleted = pass.Delete(p.PassID.ToString(), false, p.EventTime) && isDeleted;
                                        }

                                        if (isDeleted)
                                        {
                                            ph.CommitTransaction();
                                        }
                                        else
                                        {
                                            ph.RollbackTransaction();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ph.GetTransaction() != null)
                                            ph.RollbackTransaction();
                                        throw ex;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                                    return;
                                }
                            }

                            if (isDeleted)
                            {
                                MessageBox.Show(rm.GetString("passesDel", culture));
                            }
                            else
                            {
                                MessageBox.Show(rm.GetString("noPassDel", culture));
                            }
                        }
                    }
                    else
                    {
                        ListViewItem item = lvPasses.SelectedItems[0];
                        foreach (PassTO pass in currentPassesList)
                        {

                            if (pass.PassID.ToString() == item.Text)
                            {
                                currentPassHist = new PassHistTO(-1, pass.PassID, pass.EmployeeID, pass.Direction, pass.EventTime, pass.PassTypeID, pass.IsWrkHrsCount, pass.LocationID,
                                    pass.PairGenUsed, pass.ManualyCreated, Constants.PassDeleted, pass.CreatedBy, pass.CreatedTime, "", new DateTime());
                                break;
                            }
                        }
                        PassTO p = (PassTO)item.Tag;
                        PassesAdd pa = new PassesAdd(currentPassHist, p);
                        pa.ShowDialog();
                    }

                    btnSearch_Click(sender, e);

                    //clearForm();
                    //clearListView();

                    //if (!wuString.Equals(""))
                    //{
                    //    /*currentPassesList = currentPass.SearchInterval(-1, "", new DateTime(0), new DateTime(0),
                    //        -1, -1, wuString, dtFrom.Value, dtTo.Value);
                    //    currentPassesList.Sort(new ArrayListSort(sortOrder, sortField));
                    //    startIndex = 0;
                    //    populateListView(currentPassesList, startIndex);
                    //    passesListCount = currentPassesList.Count;*/
                    //}
                    //else
                    //{
                    //    MessageBox.Show(rm.GetString("noEmplPassPrivilege", culture));
                    //}

                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally { 
           
                this.Cursor = Cursors.Arrow;
            
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
                            if (!chbHierarhicly.Checked)
                            {
                                chbHierarhicly.Checked = true;
                                check = true;
                            }
                        }
                    }
                }                
                
				if (cbWU.SelectedValue is int&&!check)
				{
					populateEmployeeCombo((int) cbWU.SelectedValue);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
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

                PassesAdd addPass = new PassesAdd();
                addPass.ShowDialog();

                //Reload form only if some permission is added
                if (addPass.doReloadOnBack)
                {
                    btnSearch_Click(sender, e);
                }

                //clearForm();
                //clearListView();

                //if (!wuString.Equals(""))
                //{
                //    /*currentPassesList = currentPass.SearchInterval(-1, "", new DateTime(0), new DateTime(0), -1, -1, wuString, dtFrom.Value, dtTo.Value);
                //    currentPassesList.Sort(new ArrayListSort(sortOrder, sortField));
                //    startIndex = 0;
                //    populateListView(currentPassesList, startIndex);
                //    passesListCount = currentPassesList.Count;*/
                //}
                //else
                //{
                //    MessageBox.Show(rm.GetString("noEmplPassPrivilege", culture));
                //}
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnAdd_Click(): " + ex.Message + "\n");
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
                if (lvPasses.SelectedItems.Count != 1)
                {
                    MessageBox.Show(rm.GetString("selectOnePass", culture));
                }
                else
                {
                    currentPass = (PassTO)lvPasses.SelectedItems[0].Tag;
                    PassesAdd addPass = new PassesAdd(currentPass, currentPassesList);
                    addPass.ShowDialog();

                    //Reload form only if some permission is updated
                    if (addPass.doReloadOnBack)
                    {
                        string selectedPassID = lvPasses.SelectedItems[0].Text;
                        btnSearch_Click(sender, e);
                        //set selected item, if it is still in the list
                        for (int i = 0; i < lvPasses.Items.Count; i++)
                        {
                            string currentPassID = lvPasses.Items[i].Text;
                            if (currentPassID == selectedPassID)
                            {
                                lvPasses.Items[i].Selected = true;
                                break;
                            }
                        }
                    }

                    //clearForm();
                    //clearListView();

                    //if (!wuString.Equals(""))
                    //{
                    //    /*currentPassesList = currentPass.SearchInterval(-1, "", new DateTime(0), new DateTime(0), -1, -1, wuString, dtFrom.Value, dtTo.Value);
                    //    currentPassesList.Sort(new ArrayListSort(sortOrder, sortField));
                    //    startIndex = 0;
                    //    populateListView(currentPassesList, startIndex);
                    //    passesListCount = currentPassesList.Count;*/
                    //}
                    //else
                    //{
                    //    MessageBox.Show(rm.GetString("noEmplPassPrivilege", culture));
                    //}
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvPasses_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				/*if (lvPasses.SelectedItems.Count > 0)
				{
					PassTO passTO = currentPass.Find(lvPasses.SelectedItems[0].Text.Trim());
					if (passTO.PassID >= 0)
					{
						cbEmployee.SelectedValue = passTO.EmployeeID;
						cbDirection.SelectedIndex = cbDirection.FindStringExact(passTO.Direction);
						dtpFrom.Value = passTO.EventTime;
						dtpTo.Value = passTO.EventTime;
						cbPassType.SelectedValue = passTO.PassTypeID;
						cbLocation.SelectedValue = passTO.LocationID;
					}
				}*/
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.lvPasses_SelectedIndexChanged(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
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
				log.writeLog(DateTime.Now + " Passes.setVisibility(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

        private void setBtnPhotos()
        {
            try
            {
                int permission;
                string btnPhotosMenuItemID = rm.GetString("btnPhotosMenuItemID", culture);
                bool btnReadPermission = false;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permission = (((int[])menuItemsPermissions[btnPhotosMenuItemID])[role.ApplRoleID]);
                    btnReadPermission = btnReadPermission || (((permission / 8) % 2) == 0 ? false : true);
                }

                btnPhotos.Enabled = btnReadPermission;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.setBtnPhotos(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
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
				populateListView(currentPassesList, startIndex);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.btnPrev_Click(): " + ex.Message + "\n");
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
				populateListView(currentPassesList, startIndex);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Passes.btnNext_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

        private void btnPhotos_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvPasses.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noSelPassPhotos", culture));
                }
                else
                {
                    string passID = "";
                    foreach (ListViewItem item in lvPasses.SelectedItems)
                    {
                        passID += item.Text + ",";
                    }
                    if (passID != "")
                        passID = passID.Substring(0, passID.Length - 1);

                    PassesSnapshots passesSnapshots = new PassesSnapshots(passID);
                    passesSnapshots.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnPhotos_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (btnAdvanced.Text.Equals(rm.GetString("btnAdvanced1", culture)))
                {
                    btnAdvanced.Text = rm.GetString("btnAdvanced2", culture);
                    gbAdvancedSearch.Visible = true;
                    gbPassesNumType.Visible = true;
                }
                else
                {
                    resetAdvanced();
                    btnSearch_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnAdvanced_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (rbAll.Checked)
                {
                    //dtFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    //    0, 0, 0);
                    //dtTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    //    23, 59, 0);

                    lvWithoutPasses.Visible = false;
                    lvPasses.Visible = true;

                    if (passesListCount > Constants.recordsPerPage)
                    {
                        btnPrev.Visible = true;
                        btnNext.Visible = true;
                    }
                    btnWithoutPrev.Visible = false;
                    btnWithoutNext.Visible = false;

                    this.lblTotal.Visible = true;
                    this.lblTotal.Text = rm.GetString("lblTotal", culture) + passesListCount.ToString().Trim();

                    gbPassesNumType.Visible = true;
                    gbBorderSel.Visible = false;

                    setVisibility();
                    setBtnPhotos();
                }
                else
                {
                    gbPassesNumType.Visible = false;
                    gbBorderSel.Visible = true;

                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.rbAll_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbBorder_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (rbBorder.Checked)
                {
                    gbBorderSel.Enabled = true;

                    //dtFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    //    0, 0, 0);
                    //dtTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    //    23, 59, 0);

                    lvWithoutPasses.Visible = false;
                    lvPasses.Visible = true;

                    if (passesListCount > Constants.recordsPerPage)
                    {
                        btnPrev.Visible = true;
                        btnNext.Visible = true;
                    }
                    btnWithoutPrev.Visible = false;
                    btnWithoutNext.Visible = false;

                    this.lblTotal.Visible = true;
                    this.lblTotal.Text = rm.GetString("lblTotal", culture) + passesListCount.ToString().Trim();

                    setVisibility();
                    setBtnPhotos();
                }
                else
                {
                    gbBorderSel.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.rbBorder_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbWithout_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (rbWithout.Checked)
                {
                    //dtFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    //    0, 0, 0);
                    //dtTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    //    23, 59, 0);

                    lvWithoutPasses.Visible = true;
                    lvPasses.Visible = false;

                    if (withoutListCount > Constants.recordsPerPage)
                    {
                        btnWithoutPrev.Visible = true;
                        btnWithoutNext.Visible = true;
                    }
                    btnPrev.Visible = false;
                    btnNext.Visible = false;

                    this.lblTotal.Visible = true;
                    this.lblTotal.Text = rm.GetString("lblTotal", culture) + withoutListCount.ToString().Trim();

                    btnAdd.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;
                    btnPhotos.Enabled = false;
                    gbProcessed.Enabled = false;
                    rbAllProcess.Checked = true;
                }
                else
                {
                    gbProcessed.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.rbWithout_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        public void resetAdvanced()
        {
            try
            {
                rbAll.Checked = true;
                cbFirstIN.Checked = true;
                cbLastOUT.Checked = true;
                gbBorderSel.Visible = false;
                gbPassesNumType.Visible = true;
                dtFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    0, 0, 0);
                dtTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    23, 59, 0);

                btnAdvanced.Text = rm.GetString("btnAdvanced1", culture);
                gbAdvancedSearch.Visible = false;

                currentEmployeeList.Clear();
                clearWithoutListView();
                lvWithoutPasses.Visible = false;
                lvPasses.Visible = true;

                setVisibility();
                setBtnPhotos();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.resetAdvanced(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void clearWithoutListView()
        {
            lvWithoutPasses.BeginUpdate();
            lvWithoutPasses.Items.Clear();
            lvWithoutPasses.EndUpdate();

            lvWithoutPasses.Invalidate();

            btnWithoutPrev.Visible = false;
            btnWithoutNext.Visible = false;

            withoutListCount = 0;
        }

        private void lvWithoutPasses_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int prevOrder = sortOrderWithout;

                if (e.Column == sortFieldWithout)
                {
                    if (prevOrder == Constants.sortAsc)
                    {
                        sortOrderWithout = Constants.sortDesc;
                    }
                    else
                    {
                        sortOrderWithout = Constants.sortAsc;
                    }
                }
                else
                {
                    // New Sort Order
                    sortOrderWithout = Constants.sortAsc;
                }

                sortFieldWithout = e.Column;

                currentEmployeeList.Sort(new ArrayListSort2(sortOrderWithout, sortFieldWithout));
                startIndexWithout = 0;
                populateWithoutListView(currentEmployeeList, startIndexWithout);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.lvWithoutPasses_ColumnClick(): " + ex.Message + "\n");
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
                startIndexWithout += Constants.recordsPerPage;
                populateWithoutListView(currentEmployeeList, startIndexWithout);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnWithoutNext_Click(): " + ex.Message + "\n");
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
                startIndexWithout -= Constants.recordsPerPage;
                if (startIndexWithout < 0)
                {
                    startIndexWithout = 0;
                }
                populateWithoutListView(currentEmployeeList, startIndexWithout);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnWithoutPrev_Click(): " + ex.Message + "\n");
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

                bool advancedSearch = btnAdvanced.Text.Equals(rm.GetString("btnAdvanced2", culture));

                if (advancedSearch && rbWithout.Checked)
                {
                    if (currentEmployeeList.Count > 0)
                    {
                        // Table Definition for Crystal Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("employee_without_passes");
                        DataTable tableI = new DataTable("images");

                        tableCR.Columns.Add("date", typeof(System.DateTime));
                        tableCR.Columns.Add("first_name", typeof(System.String));
                        tableCR.Columns.Add("last_name", typeof(System.String));
                        tableCR.Columns.Add("working_unit", typeof(System.String));
                        tableCR.Columns.Add("employee_id", typeof(int));
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

                        foreach (EmployeeTO employee in currentEmployeeList)
                        {
                            DataRow row = tableCR.NewRow();

                            row["employee_id"] = employee.EmployeeID;
                            row["last_name"] = employee.LastName;
                            row["first_name"] = "";
                            row["working_unit"] = employee.WorkingUnitName;
                            row["date"] = employee.ScheduleDate;
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
                        string selDirection = "*";
                        string selLocation = "*";
                        string fromTime = "";
                        string toTime = "";

                        if (cbWU.SelectedIndex >= 0 && (int)cbWU.SelectedValue >= 0)
                            selWorkingUnit = cbWU.Text;
                        if (cbEmployee.SelectedIndex >= 0 && (int)cbEmployee.SelectedValue >= 0)
                            selEmployee = cbEmployee.Text;
                        if (cbPassType.SelectedIndex >= 0 && (int)cbPassType.SelectedValue >= 0)
                            selPassType = cbPassType.Text;
                        if (cbDirection.SelectedIndex > 0)
                            selDirection = cbDirection.Text;
                        if (cbLocation.SelectedIndex >= 0 && (int)cbLocation.SelectedValue >= 0)
                            selLocation = cbLocation.Text;
                        
                        fromTime = dtFrom.Value.ToString("HH:mm");
                        toTime = dtTo.Value.ToString("HH:mm");

                        if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                        {
                            Reports.Reports_sr.EmployeeWithoutPassesCRView view = new Reports.Reports_sr.EmployeeWithoutPassesCRView(dataSetCR,
                                dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selPassType, selDirection, selLocation,
                                fromTime, toTime);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                        {
                            Reports.Reports_en.EmployeeWithoutPassesCRView_en view = new Reports.Reports_en.EmployeeWithoutPassesCRView_en(dataSetCR,
                                dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selPassType, selDirection, selLocation,
                                fromTime, toTime);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                        {
                            Reports.Reports_fi.EmployeeWithoutPassesCRView_fi view = new Reports.Reports_fi.EmployeeWithoutPassesCRView_fi(dataSetCR,
                                dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selPassType, selDirection, selLocation,
                                fromTime, toTime);
                            view.ShowDialog(this);
                        }
                    } //if (currentEmployeeList.Count > 0)
                    else
                    {
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }
                } //if (advancedSearch && rbWithout.Checked)
                else
                {
                    if (currentPassesList.Count > 0)
                    {
                        // Table Definition for Crystal Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("employee_passes");
                        DataTable tableI = new DataTable("images");

                        tableCR.Columns.Add("event_time", typeof(System.DateTime));
                        tableCR.Columns.Add("first_name", typeof(System.String));
                        tableCR.Columns.Add("last_name", typeof(System.String));
                        tableCR.Columns.Add("pass_id", typeof(int));
                        tableCR.Columns.Add("imageID", typeof(byte));
                        tableCR.Columns.Add("direction", typeof(System.String));
                        tableCR.Columns.Add("pass_type", typeof(System.String));
                        tableCR.Columns.Add("is_wrk_hrs", typeof(System.String));
                        tableCR.Columns.Add("location", typeof(System.String));
                        tableCR.Columns.Add("pair_gen_used", typeof(System.String));
                        tableCR.Columns.Add("manual_created", typeof(System.String));
                        tableCR.Columns.Add("gate_id", typeof(System.String));

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

                        foreach (PassTO pass in currentPassesList)
                        {
                            DataRow row = tableCR.NewRow();

                            row["pass_id"] = pass.PassID;
                            row["last_name"] = pass.EmployeeName;
                            row["first_name"] = "";
                            row["direction"] = pass.Direction;
                            row["event_time"] = pass.EventTime;
                            row["pass_type"] = pass.PassType;
                            if (pass.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter)
                            {
                                row["is_wrk_hrs"] = rm.GetString("yes", culture);
                            }
                            else
                            {
                                row["is_wrk_hrs"] = rm.GetString("no", culture);
                            }
                            row["location"] = pass.LocationName;
                            if (pass.PairGenUsed == (int)Constants.PairGenUsed.Used)
                            {
                                row["pair_gen_used"] = rm.GetString("yes", culture);
                            }
                            else
                            {
                                row["pair_gen_used"] = rm.GetString("no", culture);
                            }
                            if (pass.ManualyCreated == (int)Constants.recordCreated.Manualy)
                            {
                                row["manual_created"] = rm.GetString("yes", culture);
                            }
                            else
                            {
                                row["manual_created"] = rm.GetString("no", culture);
                            }
                            if (gates.ContainsKey(pass.GateID))
                            {
                                row["gate_id"] = gates[pass.GateID].ToString().Trim();
                            }
                            else
                            {
                                row["gate_id"] = "N/A";
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

                        string selWorkingUnit = "*";
                        string selEmployee = "*";
                        string selPassType = "*";
                        string selDirection = "*";
                        string selLocation = "*";
                        string fromTime = "";
                        string toTime = "";
                        string firstIn = "";
                        string lastOut = "";
                        string advanced = "";

                        if (cbWU.SelectedIndex >= 0 && (int)cbWU.SelectedValue >= 0)
                            selWorkingUnit = cbWU.Text;
                        if (cbEmployee.SelectedIndex >= 0 && (int)cbEmployee.SelectedValue >= 0)
                            selEmployee = cbEmployee.Text;
                        if (cbPassType.SelectedIndex >= 0 && (int)cbPassType.SelectedValue >= 0)
                            selPassType = cbPassType.Text;
                        if (cbDirection.SelectedIndex > 0)
                            selDirection = cbDirection.Text;
                        if (cbLocation.SelectedIndex >= 0 && (int)cbLocation.SelectedValue >= 0)
                            selLocation = cbLocation.Text;

                        if (advancedSearch)
                        {
                            fromTime = dtFrom.Value.ToString("HH:mm");
                            toTime = dtTo.Value.ToString("HH:mm");
                            advanced = "advanced";
                        }

                        if (advancedSearch && rbBorder.Checked)
                        {
                            if (cbFirstIN.Checked)
                                firstIn = cbFirstIN.Text;

                            if (cbLastOUT.Checked)
                                lastOut = cbLastOUT.Text;
                        }

                        if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                        {
                            Reports.Reports_sr.EmployeePassesCRView view = new Reports.Reports_sr.EmployeePassesCRView(dataSetCR,
                                dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selPassType, selDirection, selLocation,
                                fromTime, toTime, firstIn, lastOut, advanced);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                        {
                            Reports.Reports_en.EmployeePassesCRView_en view = new Reports.Reports_en.EmployeePassesCRView_en(dataSetCR,
                                dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selPassType, selDirection, selLocation,
                                fromTime, toTime, firstIn, lastOut, advanced);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                        {
                            Reports.Reports_fi.EmployeePassesCRView_fi view = new Reports.Reports_fi.EmployeePassesCRView_fi(dataSetCR,
                                dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selPassType, selDirection, selLocation,
                                fromTime, toTime, firstIn, lastOut, advanced);
                            view.ShowDialog(this);
                        }
                    } //if (currentPassesList.Count > 0)
                    else
                    {
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }
                } //not if (advancedSearch && rbWithout.Checked)
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnHistoryOfChange_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvPasses.SelectedItems.Count != 1)
                {
                    PassesHist history = new PassesHist();
                    history.ShowDialog();
                }
                else
                {
                    currentPass = (PassTO)lvPasses.SelectedItems[0].Tag;
                    PassesHist history = new PassesHist(currentPass);
                    history.ShowDialog();

                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnHistoryOfChange_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Passes_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " Passes.Passes_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateGatesHash()
        {
            try
            {
                List<GateTO> gatesList = new Gate().Search();

                foreach (GateTO gate in gatesList)
                {
                    gates.Add(gate.GateID, gate.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateGatesHash(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                    cbWU.SelectedIndex = cbWU .FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Passes.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Passes.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Passes_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                sortOrder = Constants.sortAsc;
                sortField = Passes.EventTimeIndex;
                startIndex = 0;
                this.lblTotal.Visible = false;
                                
                statuses.Add(Constants.statusActive);
                statuses.Add(Constants.statusBlocked);

                wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.PassPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                populateWorkingUnitCombo();
                populateEmployeeCombo(-1);
                populateDirectionCombo();
                populatePassTypeCombo();
                populateLocationCombo();
                populateGatesHash();
                populateGateCombo();

                clearListView();
                clearWithoutListView();

                if (!wuString.Equals(""))
                {
                    /*currentPassesList = currentPass.SearchInterval(-1, "", new DateTime(0), new DateTime(0), -1, -1, wuString, dtFrom.Value, dtTo.Value);
                    currentPassesList.Sort(new ArrayListSort(sortOrder, sortField));
                    populateListView(currentPassesList, startIndex);
                    passesListCount = currentPassesList.Count;*/
                }
                else
                {
                    MessageBox.Show(rm.GetString("noEmplPassPrivilege", culture));
                }

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();
                setBtnPhotos();

                resetAdvanced();
                sortOrderWithout = Constants.sortAsc;
                sortFieldWithout = Passes.EmployeeNameIndex2;
                startIndexWithout = 0;

                filter = new Filter();
                filter.SerachButton = this.btnSearch;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.Passes_Load(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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
                log.writeLog(DateTime.Now + " Passes.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Passes.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvWithoutPasses_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       
	}
}
