using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;

namespace UI
{
	/// <summary>
	/// Employee View Form 
	/// </summary>
	public class Employees : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox tbFirstName;
		private System.Windows.Forms.Label lblLastName;
		private System.Windows.Forms.TextBox tbLastName;
        private System.Windows.Forms.Button btnDelete;
        private IContainer components;
		private System.Windows.Forms.Button btnSearch;

		// List View indexes
		const int EmployeeIDIndex = 0;
		const int FirstNameIndex = 1;
		const int LastNameIndex = 2;
		const int WorkingUnitIDIndex = 3;
		const int StatusIndex = 4;
		const int HasTag = 5;
		const int Type = 6;
		const int Schema = 7;
		const int ScheduleDate = 8;

		private System.Windows.Forms.ListView lvEmployees;
		private System.Windows.Forms.Label lblFirstName;
		private System.Windows.Forms.Label lblWorkingUnitID;
		private System.Windows.Forms.ComboBox cbWorkingUnitID;
        private System.Windows.Forms.ComboBox cbOrganizationalUnitID;//mm
		private System.Windows.Forms.GroupBox groupBox;

		private CultureInfo culture;
		ResourceManager rm;

		// Current Employee
		private EmployeeTO currentEmployee;
		// Controller instance
		public NotificationController Controller;
		// Observer client instance
		public NotificationObserverClient observerClient;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnClose;

		DebugLog log;

		ApplUserTO logInUser;
		List<ApplRoleTO> currentRoles;
		Hashtable menuItemsPermissions;
		string menuItemID;

		bool readPermission = false;
		bool addPermission = false;
		bool updatePermission = false;
		bool deletePermission = false;

		private List<WorkingUnitTO> wUnits;
        private List<OrganizationalUnitTO> oUnits;
		private string wuString = "";
        private string ouString = ""; //mm
       

		private Hashtable types;

		List<EmployeeTO> currentEmployeesList;
		private int sortOrder;
		private System.Windows.Forms.Button btnPrev;
		private System.Windows.Forms.Button btnNext;
		private int sortField;
		private int startIndex;
		private System.Windows.Forms.TextBox tbEmployeeID;
		private System.Windows.Forms.Label lblEmployeeID;
		private System.Windows.Forms.ComboBox cbHasTag;
		private System.Windows.Forms.Label lblHasTag;
		private System.Windows.Forms.Label lblTotal;
		private System.Windows.Forms.ComboBox cbType;
		private System.Windows.Forms.Label lblType;
		private DataManager dataMgr;
        private Button btnClear;
        private List<string> statuses = new List<string>();

        private bool populateFilter = true;
        private Button btnWUTree;
        private CheckBox chbHierarhicly;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;
        private bool textChanged = true;
        private GroupBox gbFilter;
        private ImageList imageList1;
        private Filter filter;

        List<int> checkedColumns = new List<int>();
        private const int indexChecked = 1;
        private const int indexUnChecked = 0;
        private Button btnReport;
        private Button btnCheckCounters;
        private Label lblEmplChecked;
        private GroupBox gbShiftOnDay;
        private Button btnGenerate;
        private DateTimePicker dtpShiftDay;
        private const int maxRowNum = 12;
        bool tagChanged = false;
        
        private Label lbOrganizationalUnit;
        bool writeDataToTag = false;

        List<OrganizationalUnitTO> ouArray = new List<OrganizationalUnitTO>();

		public Employees()
		{
			InitializeComponent();

			this.CenterToScreen();

			// Init Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);
			
			currentEmployee = new EmployeeTO();
			currentEmployeesList = new List<EmployeeTO>();
			types = new Hashtable();

			logInUser = NotificationController.GetLogInUser();
			
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(Employees).Assembly);  
			
            checkedColumns.Add(EmployeeIDIndex);
            checkedColumns.Add(LastNameIndex);
            checkedColumns.Add(FirstNameIndex);
            checkedColumns.Add(WorkingUnitIDIndex);
            checkedColumns.Add(StatusIndex);
            checkedColumns.Add(HasTag);
            checkedColumns.Add(Schema);
            checkedColumns.Add(Type);
            checkedColumns.Add(ScheduleDate);

            setLanguage();
			// Used in tag's changes tracking (method onLoad, onClose)
			dataMgr = new DataManager();

            btnCheckCounters.Visible = lblEmplChecked.Visible = NotificationController.GetLogInUser().UserID.Equals(Constants.sysUser);                
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Employees));
            this.tbFirstName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.tbLastName = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblWorkingUnitID = new System.Windows.Forms.Label();
            this.cbWorkingUnitID = new System.Windows.Forms.ComboBox();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cbHasTag = new System.Windows.Forms.ComboBox();
            this.lblHasTag = new System.Windows.Forms.Label();
            this.tbEmployeeID = new System.Windows.Forms.TextBox();
            this.lblEmployeeID = new System.Windows.Forms.Label();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnCheckCounters = new System.Windows.Forms.Button();
            this.lblEmplChecked = new System.Windows.Forms.Label();
            this.gbShiftOnDay = new System.Windows.Forms.GroupBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.dtpShiftDay = new System.Windows.Forms.DateTimePicker();
            this.cbOrganizationalUnitID = new System.Windows.Forms.ComboBox();
            this.lbOrganizationalUnit = new System.Windows.Forms.Label();
            this.groupBox.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.gbShiftOnDay.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbFirstName
            // 
            this.tbFirstName.Location = new System.Drawing.Point(113, 91);
            this.tbFirstName.MaxLength = 50;
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.Size = new System.Drawing.Size(206, 20);
            this.tbFirstName.TabIndex = 3;
            this.tbFirstName.TextChanged += new System.EventHandler(this.tbFirstName_TextChanged);
            // 
            // lblLastName
            // 
            this.lblLastName.Location = new System.Drawing.Point(25, 123);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(80, 23);
            this.lblLastName.TabIndex = 4;
            this.lblLastName.Text = "Last Name:";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbLastName
            // 
            this.tbLastName.Location = new System.Drawing.Point(113, 123);
            this.tbLastName.MaxLength = 50;
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.Size = new System.Drawing.Size(206, 20);
            this.tbLastName.TabIndex = 5;
            this.tbLastName.TextChanged += new System.EventHandler(this.tbLastName_TextChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(216, 477);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(676, 120);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 23);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lvEmployees
            // 
            this.lvEmployees.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(16, 174);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.Size = new System.Drawing.Size(910, 258);
            this.lvEmployees.SmallImageList = this.imageList1;
            this.lvEmployees.TabIndex = 4;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.SelectedIndexChanged += new System.EventHandler(this.lvEmployees_SelectedIndexChanged);
            this.lvEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployees_ColumnClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Unchecked.png");
            this.imageList1.Images.SetKeyName(1, "Checked.png");
            // 
            // lblFirstName
            // 
            this.lblFirstName.Location = new System.Drawing.Point(25, 91);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(80, 23);
            this.lblFirstName.TabIndex = 2;
            this.lblFirstName.Text = "First Name:";
            this.lblFirstName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWorkingUnitID
            // 
            this.lblWorkingUnitID.Location = new System.Drawing.Point(309, 24);
            this.lblWorkingUnitID.Name = "lblWorkingUnitID";
            this.lblWorkingUnitID.Size = new System.Drawing.Size(104, 23);
            this.lblWorkingUnitID.TabIndex = 6;
            this.lblWorkingUnitID.Text = "Working Unit ID:";
            this.lblWorkingUnitID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWorkingUnitID
            // 
            this.cbWorkingUnitID.AccessibleDescription = "FILTERABLE";
            this.cbWorkingUnitID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnitID.Location = new System.Drawing.Point(421, 24);
            this.cbWorkingUnitID.Name = "cbWorkingUnitID";
            this.cbWorkingUnitID.Size = new System.Drawing.Size(205, 21);
            this.cbWorkingUnitID.TabIndex = 7;
            this.cbWorkingUnitID.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnitID_SelectedIndexChanged);
            // 
            // groupBox
            // 
            this.groupBox.AccessibleDescription = "";
            this.groupBox.Controls.Add(this.cbOrganizationalUnitID);
            this.groupBox.Controls.Add(this.chbHierarhicly);
            this.groupBox.Controls.Add(this.lbOrganizationalUnit);
            this.groupBox.Controls.Add(this.btnWUTree);
            this.groupBox.Controls.Add(this.btnClear);
            this.groupBox.Controls.Add(this.cbType);
            this.groupBox.Controls.Add(this.lblType);
            this.groupBox.Controls.Add(this.cbHasTag);
            this.groupBox.Controls.Add(this.lblHasTag);
            this.groupBox.Controls.Add(this.tbEmployeeID);
            this.groupBox.Controls.Add(this.lblEmployeeID);
            this.groupBox.Controls.Add(this.btnSearch);
            this.groupBox.Controls.Add(this.tbFirstName);
            this.groupBox.Controls.Add(this.tbLastName);
            this.groupBox.Controls.Add(this.cbWorkingUnitID);
            this.groupBox.Controls.Add(this.lblFirstName);
            this.groupBox.Controls.Add(this.lblLastName);
            this.groupBox.Controls.Add(this.lblWorkingUnitID);
            this.groupBox.Location = new System.Drawing.Point(16, 8);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(772, 160);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Tag = "FILTERABLE";
            this.groupBox.Text = "Employee";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(663, 24);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(101, 24);
            this.chbHierarhicly.TabIndex = 9;
            this.chbHierarhicly.Text = "Hierarchy ";
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(632, 24);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 8;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(538, 120);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 23);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.Location = new System.Drawing.Point(421, 85);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(205, 21);
            this.cbType.TabIndex = 13;
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(309, 84);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(104, 23);
            this.lblType.TabIndex = 12;
            this.lblType.Text = "Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbHasTag
            // 
            this.cbHasTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHasTag.Location = new System.Drawing.Point(421, 56);
            this.cbHasTag.Name = "cbHasTag";
            this.cbHasTag.Size = new System.Drawing.Size(205, 21);
            this.cbHasTag.TabIndex = 11;
            // 
            // lblHasTag
            // 
            this.lblHasTag.Location = new System.Drawing.Point(309, 56);
            this.lblHasTag.Name = "lblHasTag";
            this.lblHasTag.Size = new System.Drawing.Size(104, 23);
            this.lblHasTag.TabIndex = 10;
            this.lblHasTag.Text = "Has Tag:";
            this.lblHasTag.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbEmployeeID
            // 
            this.tbEmployeeID.Location = new System.Drawing.Point(113, 24);
            this.tbEmployeeID.MaxLength = 50;
            this.tbEmployeeID.Name = "tbEmployeeID";
            this.tbEmployeeID.Size = new System.Drawing.Size(206, 20);
            this.tbEmployeeID.TabIndex = 1;
            this.tbEmployeeID.TextChanged += new System.EventHandler(this.tbEmployeeID_TextChanged);
            // 
            // lblEmployeeID
            // 
            this.lblEmployeeID.Location = new System.Drawing.Point(25, 24);
            this.lblEmployeeID.Name = "lblEmployeeID";
            this.lblEmployeeID.Size = new System.Drawing.Size(80, 23);
            this.lblEmployeeID.TabIndex = 0;
            this.lblEmployeeID.Text = "Employee ID:";
            this.lblEmployeeID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(6, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 0;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(32, 56);
            this.btnSaveCriteria.Name = "btnSaveCriteria";
            this.btnSaveCriteria.Size = new System.Drawing.Size(82, 23);
            this.btnSaveCriteria.TabIndex = 1;
            this.btnSaveCriteria.Text = "Save criteria";
            this.btnSaveCriteria.UseVisualStyleBackColor = true;
            this.btnSaveCriteria.Click += new System.EventHandler(this.btnSaveCriteria_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 477);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 23);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Add new";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(120, 477);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 7;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(851, 477);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(854, 145);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 2;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(894, 145);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(774, 435);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(152, 16);
            this.lblTotal.TabIndex = 5;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(794, 8);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 100);
            this.gbFilter.TabIndex = 1;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(306, 477);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(75, 23);
            this.btnReport.TabIndex = 9;
            this.btnReport.Text = "Report";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnCheckCounters
            // 
            this.btnCheckCounters.Location = new System.Drawing.Point(741, 477);
            this.btnCheckCounters.Name = "btnCheckCounters";
            this.btnCheckCounters.Size = new System.Drawing.Size(104, 23);
            this.btnCheckCounters.TabIndex = 11;
            this.btnCheckCounters.Text = "Check counters";
            this.btnCheckCounters.Click += new System.EventHandler(this.btnCheckCounters_Click);
            // 
            // lblEmplChecked
            // 
            this.lblEmplChecked.Location = new System.Drawing.Point(738, 451);
            this.lblEmplChecked.Name = "lblEmplChecked";
            this.lblEmplChecked.Size = new System.Drawing.Size(105, 16);
            this.lblEmplChecked.TabIndex = 27;
            this.lblEmplChecked.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbShiftOnDay
            // 
            this.gbShiftOnDay.Controls.Add(this.btnGenerate);
            this.gbShiftOnDay.Controls.Add(this.dtpShiftDay);
            this.gbShiftOnDay.Location = new System.Drawing.Point(401, 444);
            this.gbShiftOnDay.Name = "gbShiftOnDay";
            this.gbShiftOnDay.Size = new System.Drawing.Size(241, 56);
            this.gbShiftOnDay.TabIndex = 10;
            this.gbShiftOnDay.TabStop = false;
            this.gbShiftOnDay.Text = "Shift on day";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(153, 19);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // dtpShiftDay
            // 
            this.dtpShiftDay.CustomFormat = "dd.MM.yyyy.";
            this.dtpShiftDay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpShiftDay.Location = new System.Drawing.Point(24, 22);
            this.dtpShiftDay.Name = "dtpShiftDay";
            this.dtpShiftDay.Size = new System.Drawing.Size(101, 20);
            this.dtpShiftDay.TabIndex = 0;
            // 
            // cbOrganizationalUnitID
            // 
            this.cbOrganizationalUnitID.AccessibleDescription = "FILTERABLE";
            this.cbOrganizationalUnitID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOrganizationalUnitID.Location = new System.Drawing.Point(113, 56);
            this.cbOrganizationalUnitID.Name = "cbOrganizationalUnitID";
            this.cbOrganizationalUnitID.Size = new System.Drawing.Size(205, 21);
            this.cbOrganizationalUnitID.TabIndex = 17;
            // 
            // lbOrganizationalUnit
            // 
            this.lbOrganizationalUnit.Location = new System.Drawing.Point(1, 56);
            this.lbOrganizationalUnit.Name = "lbOrganizationalUnit";
            this.lbOrganizationalUnit.Size = new System.Drawing.Size(104, 23);
            this.lbOrganizationalUnit.TabIndex = 16;
            this.lbOrganizationalUnit.Text = "Organizational Unit ID:";
            this.lbOrganizationalUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Employees
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(934, 534);
            this.ControlBox = false;
            this.Controls.Add(this.gbShiftOnDay);
            this.Controls.Add(this.lblEmplChecked);
            this.Controls.Add(this.btnCheckCounters);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvEmployees);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Employees";
            this.ShowInTaskbar = false;
            this.Text = "Employees";
            this.Load += new System.EventHandler(this.Employees_Load);
            this.Closed += new System.EventHandler(this.Employees_Closed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Employees_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Employees_KeyDown);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.gbFilter.ResumeLayout(false);
            this.gbShiftOnDay.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Inner Class for sorting Array List of Employees

		/*
		 *  Class used for sorting Array List of Employees
		*/

		private class ArrayListSort:IComparer<EmployeeTO>
		{        
			private int compOrder;        
			private int compField;
			public ArrayListSort(int sortOrder, int sortField)        
			{            
				compOrder = sortOrder;
				compField = sortField;
			}        
			
			public int Compare(EmployeeTO x, EmployeeTO y)
			{
                EmployeeTO empl1 = null;
                EmployeeTO empl2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    empl1 = x;
                    empl2 = y;
                }
                else
                {
                    empl1 = y;
                    empl2 = x;
                }

				switch(compField)            
				{                
					case Employees.EmployeeIDIndex: 
						return empl1.EmployeeID.CompareTo(empl2.EmployeeID);
					case Employees.FirstNameIndex:
						return empl1.FirstName.CompareTo(empl2.FirstName);
					case Employees.LastNameIndex:
						return empl1.LastName.CompareTo(empl2.LastName);
					case Employees.WorkingUnitIDIndex:
						return empl1.WorkingUnitName.CompareTo(empl2.WorkingUnitName);
					case Employees.StatusIndex:
						return empl1.Status.CompareTo(empl2.Status);
					case Employees.HasTag:
						return empl1.HasTag.CompareTo(empl2.HasTag);
					case Employees.Type:
						return empl1.Type.CompareTo(empl2.Type);
					case Employees.Schema:
						return empl1.SchemaName.CompareTo(empl2.SchemaName);
					case Employees.ScheduleDate:
						return empl1.ScheduleDate.CompareTo(empl2.ScheduleDate);
					default:                    
						return empl1.LastName.CompareTo(empl2.LastName);
				}        
			}    
		}

		#endregion

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (lvEmployees.SelectedItems.Count > 0)
			{
				try
				{
					DialogResult result = MessageBox.Show(rm.GetString("messageDeleteEmpl", culture), "", MessageBoxButtons.YesNo);
					if (result == DialogResult.Yes)
					{
						bool isDeleted = true;

                        Dictionary<int, EmployeeAsco4TO> ascoDict = new Dictionary<int, EmployeeAsco4TO>();
                        string emplIDs = "";
                        foreach (ListViewItem item in lvEmployees.SelectedItems)
                        {
                            emplIDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";
                        }

                        if (emplIDs.Length > 0)
                        {
                            emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                            ascoDict = new EmployeeAsco4().SearchDictionary(emplIDs);
                        }

                        bool noRetiredDate = false;
                        foreach (int id in ascoDict.Keys)
                        {
                            if (ascoDict[id].DatetimeValue3.Equals(new DateTime()))
                            {
                                noRetiredDate = true;
                                break;
                            }
                        }

                        if (noRetiredDate)
                        {
                            DialogResult retDateResult = MessageBox.Show(rm.GetString("noRetiredDates", culture), "", MessageBoxButtons.YesNo);

                            if (retDateResult == DialogResult.No)
                                return;
                        }

                        Dictionary<int, WorkTimeSchemaTO> dictTimeSchema = new TimeSchema().getDictionary();
						// Delete Employee means set his status to 'RETIRED'
						foreach(ListViewItem item in lvEmployees.SelectedItems)
						{
                            isDeleted = isDeleted && deleteEmployee((EmployeeTO)item.Tag, ascoDict, dictTimeSchema);							
						}

						if (isDeleted)
						{
							MessageBox.Show(rm.GetString("emplDeleted", culture));
						}
						else
						{
							MessageBox.Show(rm.GetString("emplNotDeleted", culture));
						}

                        //this.Cursor = Cursors.WaitCursor;
                        //tbEmployeeID.Text = "";
                        //tbFirstName.Text = "";
                        //tbLastName.Text = "";
                        //cbWorkingUnitID.SelectedIndex = 0;
                        //cbHasTag.SelectedIndex = 0;
                        //cbType.SelectedIndex = 0;

						if (!wuString.Equals(""))
						{
							//currentEmployeesList = new Employee().SearchTagsWithStatuses(statuses, wuString, (int) Constants.HasTag.all);
							//lblTotal.Text = rm.GetString("lblTotal", culture) + currentEmployeesList.Count.ToString().Trim();
	
							//startIndex = 0;
							//currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
							//populateListView(currentEmployeesList, startIndex);
                            btnSearch.PerformClick();
						}
						else
						{
							MessageBox.Show(rm.GetString("noEmplPrivilege", culture));
						}

						currentEmployee = new EmployeeTO();
					}
				}
				catch(Exception ex)
				{
					log.writeLog(DateTime.Now + " Employees.btnDelete_Click(): " + ex.Message + "\n");
					MessageBox.Show(ex.Message);
				}
				finally
				{
					this.Cursor = Cursors.Arrow;
				}
			}
			else
			{
				MessageBox.Show(rm.GetString("messageEmployeeNotSelected", culture));
			}
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				string wuID = "", ouID="";
				int hasTag;
				
				if ((int) this.cbWorkingUnitID.SelectedValue == -1)
				{
					wuID = wuString;
				}
				else
				{
					wuID = this.cbWorkingUnitID.SelectedValue.ToString().Trim();
				}
                //19.06.2017 Miodrag Mitrovic / dodaje se pretraga i po organizacionim jedinicama)
                if ((int)this.cbOrganizationalUnitID.SelectedValue == -1)
				{
                    ouID = ouString;
				}
				else
				{
                    ouID = this.cbOrganizationalUnitID.SelectedValue.ToString().Trim();
				}
                //MM

                
               
                //10.12.2008. Natasa - if Hierarhicly is checked show employees form child working unit's
                WorkingUnit wu = new WorkingUnit();
                if ((int)this.cbWorkingUnitID.SelectedValue != -1 && chbHierarhicly.Checked)
                {
                    List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                    WorkingUnit workUnit = new WorkingUnit();
                    foreach (WorkingUnitTO workingUnit in wUnits)
                    {
                        if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnitID.SelectedValue)
                        {
                            wuList.Add(workingUnit);
                            workUnit.WUTO = workingUnit;
                        }
                    }
                    if (workUnit.WUTO.ChildWUNumber > 0)
                    wuList = wu.FindAllChildren(wuList);
                    wuID = "";
                    foreach (WorkingUnitTO wunit in wuList)
                    {
                        wuID += wunit.WorkingUnitID.ToString().Trim() + ",";
                    }

                    if (wuID.Length > 0)
                    {
                        wuID = wuID.Substring(0, wuID.Length - 1);
                    }
                }

				if (cbHasTag.Text.Trim().Equals(rm.GetString("yes", culture)))
				{
					hasTag = (int) Constants.HasTag.yes;
				}
				else if (cbHasTag.Text.Trim().Equals(rm.GetString("no", culture)))
				{
					hasTag = (int) Constants.HasTag.no;
				}
				else
				{
					hasTag = (int) Constants.HasTag.all;
				}

				string type = "";
				if (this.cbType.SelectedIndex > 0)
				{
					type = (string) types[cbType.SelectedItem.ToString()];
				}

				if (!wuString.Equals(""))
				{
                    currentEmployee = new EmployeeTO();
                    int emplID = -1;
                    if (int.TryParse(tbEmployeeID.Text.Trim(), out emplID))
                        currentEmployee.EmployeeID = emplID;
                    currentEmployee.FirstName = tbFirstName.Text.Trim();
                    currentEmployee.LastName = tbLastName.Text.Trim();
                    currentEmployee.Type = type;
                    currentEmployee.OrgUnitID = (int)cbOrganizationalUnitID.SelectedValue;
                    Employee employee = new Employee();
                    employee.EmplTO = currentEmployee;
					currentEmployeesList = employee.SearchTagsWithStatuses(statuses, wuID, hasTag);
                                        
					if (currentEmployeesList.Count > 0)
					{                        
						currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
						lblTotal.Text = rm.GetString("lblTotal", culture) + currentEmployeesList.Count.ToString().Trim();
						startIndex = 0;
						populateListView(currentEmployeesList, startIndex);
					}
					else
					{
						MessageBox.Show(rm.GetString("emplNotFound", culture));
					}
				}
				else
				{
					MessageBox.Show(rm.GetString("noEmplPrivilege", culture));
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.btnSearch_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}
	
		/// <summary>
		/// Populate ListView
		/// </summary>
		/// <param name="employeesList">Array of Employee object</param>
		public void populateListView(List<EmployeeTO> employeesList, int startIndex)
		{
			try
			{
				if (employeesList.Count > Constants.recordsPerPage)
				{
					btnPrev.Visible = true;
					btnNext.Visible = true;
				}
				else
				{
					btnPrev.Visible = false;
					btnNext.Visible = false;
				}

				lvEmployees.BeginUpdate();
				lvEmployees.Items.Clear();

				if (employeesList.Count > 0)
				{
					if ((startIndex >= 0) && (startIndex < employeesList.Count))
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
						if (lastIndex >= employeesList.Count)
						{
							btnNext.Enabled = false;
							lastIndex = employeesList.Count;
						}
						else
						{
							btnNext.Enabled = true;
						}

                        //ListViewItem[] lvItems = new ListViewItem[lastIndex - startIndex];

						for (int i = startIndex; i < lastIndex; i++)
						{
							EmployeeTO employee = employeesList[i];
                            
							ListViewItem item = new ListViewItem();
                            //List<OrganizationalUnitTO> le = new List<OrganizationalUnitTO>();
                            //OrganizationalUnit e=new OrganizationalUnit();
							item.Text = employee.EmployeeID.ToString().Trim();
							item.SubItems.Add(employee.FirstName.Trim());
							item.SubItems.Add(employee.LastName.Trim());
							item.SubItems.Add(employee.WorkingUnitName.Trim());
                            //19.06.2017 Miodrag Mitrovic / Trazi ime org jedinice
                            //le=e.Search(employee.OrgUnitID.ToString().Trim());
                            //item.SubItems.Add(le[0].Name);
                           
                            if (cbOrganizationalUnitID.SelectedValue.ToString() =="-1")
                            {
                                int j = 1;
                                for (j = 1; j < ouArray.Count; j++)
                                {
                                    //log.writeLog(ouArray.Count+" - "+ ouArray[j].OrgUnitID + " == " + employee.OrgUnitID);
                                    if (ouArray[j].OrgUnitID == employee.OrgUnitID)
                                    {
                                        item.SubItems.Add(ouArray[j].Name);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                item.SubItems.Add( ouArray[cbOrganizationalUnitID.SelectedIndex].Name );
                            }
                            //mm
                            //item.SubItems.Add(employee.OrgUnitID.ToString().Trim());//mm
							item.SubItems.Add(employee.Status.Trim());
							//item.SubItems.Add(employee.Password.Trim());
							if (employee.HasTag)
							{
								item.SubItems.Add(rm.GetString("yes", culture));
							}
							else
							{
								item.SubItems.Add(rm.GetString("no", culture));
							}
							if (employee.Type.Equals(Constants.emplOrdinary))
							{
								item.SubItems.Add(rm.GetString("emplOrdinary", culture));
							}
							else if (employee.Type.Equals(Constants.emplExtraOrdinary))
							{
								item.SubItems.Add(rm.GetString("emplExtraOrdinary", culture));
							}
							else if (employee.Type.Equals(Constants.emplSpecial))
							{
								item.SubItems.Add(rm.GetString("emplSpecial", culture));
							}
							item.SubItems.Add(employee.SchemaName.Trim());
							item.SubItems.Add(employee.ScheduleDate.ToString("dd/MM/yyy"));
                            item.Tag = employee;
							lvEmployees.Items.Add(item);
                            //lvItems[i - startIndex] = item;
						}
                        //lvEmployees.Items.AddRange(lvItems);
					}
				}

				lvEmployees.EndUpdate();
				lvEmployees.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.populateListView(): " + ex.Message + "\n");
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
				this.Text = rm.GetString("menuEmployees", culture);

				// group box text
				this.groupBox.Text = rm.GetString("gbEmployees", culture);
                this.gbFilter.Text = rm.GetString("gbFilter", culture);
                this.gbShiftOnDay.Text = rm.GetString("gbShiftOnDay", culture);

                //check box text
                this.chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);

				// button's text
				btnAdd.Text = rm.GetString("btnAdd", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnDelete.Text = rm.GetString("btnDelete", culture);
				btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClear.Text = rm.GetString("btnClear", culture);
				btnClose.Text = rm.GetString("btnClose", culture);
                btnReport.Text = rm.GetString("btnReport", culture);
                btnGenerate.Text = rm.GetString("btnGenerate", culture);
                btnSaveCriteria.Text = rm.GetString("SaveFilter", culture);

				// label's text
				lblEmployeeID.Text = rm.GetString("lblEployeeID", culture);
				lblFirstName.Text = rm.GetString("lblFirstName", culture);
				lblLastName.Text = rm.GetString("lblLastName", culture);
				lblWorkingUnitID.Text = rm.GetString("lblWorkingUnit", culture);
                lbOrganizationalUnit.Text = rm.GetString("lblOrganizationalUnit", culture);
				lblHasTag.Text = rm.GetString("lblHasTag", culture);
				lblType.Text = rm.GetString("lblType", culture);
                

				// list view initialization
				lvEmployees.BeginUpdate();
				lvEmployees.Columns.Add(rm.GetString("hdrEmployeeID", culture), (lvEmployees.Width - 9) / 9-3, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployees.Width - 9) / 9 - 3, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployees.Width - 9) / 9 - 3, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvEmployees.Width - 9) / 9 - 3, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrOrganizationalUnit", culture), (lvEmployees.Width - 9) / 9 - 3, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrStatus", culture), (lvEmployees.Width - 9) / 9 - 3, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrHasTag", culture), (lvEmployees.Width - 9) / 9 - 3, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrType", culture), (lvEmployees.Width - 9) / 9 - 3, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrSchema", culture), (lvEmployees.Width - 9) / 9 - 3, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrDate", culture), (lvEmployees.Width - 9) / 9 - 3, HorizontalAlignment.Left);
				lvEmployees.EndUpdate();

                for (int i = 0; i < lvEmployees.Columns.Count; i++)
                {
                    if (checkedColumns.Contains(i))
                        lvEmployees.Columns[i].ImageIndex = indexChecked;
                    else
                        lvEmployees.Columns[i].ImageIndex = indexUnChecked;
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void lvEmployees_SelectedIndexChanged(object sender, System.EventArgs e)
		{

			if(lvEmployees.SelectedItems.Count == 1 && populateFilter)
			{
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    textChanged = false;

                    // populate Employee's search form
                    currentEmployee = (EmployeeTO)lvEmployees.SelectedItems[0].Tag;

                    //tbEmployeeID.Text = currentEmployee.EmployeeID.ToString().Trim();					
                    //tbFirstName.Text = currentEmployee.FirstName.Trim();
                    //tbLastName.Text = currentEmployee.LastName.Trim();
                    //cbWorkingUnitID.SelectedValue = currentEmployee.WorkingUnitID;                    
                    //cbHasTag.SelectedIndex = currentEmployee.HasTag ? ((int)Constants.HasTag.yes + 1) : ((int)Constants.HasTag.no + 1);
                    //cbType.SelectedIndex = cbType.FindStringExact(lvEmployees.SelectedItems[0].SubItems[6].Text.Trim());                    
                    textChanged = true;
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now + " Employees.lvEmployees_SelectedIndexChanged(): " + ex.Message + "\n");
                    MessageBox.Show(ex.Message);
                }
                finally {
                    this.Cursor = Cursors.Arrow;
                }
			}
		}

		private void lvEmployees_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
			{
                

                if (Control.ModifierKeys == Keys.Control)
                {
                    if (e.Column == FirstNameIndex || e.Column == LastNameIndex)
                    {
                        MessageBox.Show(rm.GetString("CanNotUncheck", culture));
                    }
                    else
                    {
                        if (checkedColumns.Contains(e.Column))
                        {
                            checkedColumns.Remove(e.Column);
                            lvEmployees.Columns[e.Column].ImageIndex = indexUnChecked;
                        }
                        else
                        {
                            if (checkedColumns.Count == maxRowNum)
                            {
                                MessageBox.Show(rm.GetString("UnselectAnyColumn", culture));
                            }
                            else
                            {
                                checkedColumns.Add(e.Column);
                                lvEmployees.Columns[e.Column].ImageIndex = indexChecked;
                            }
                        }
                    }
                }
                else
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

                    currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
                    startIndex = 0;
                    populateListView(currentEmployeesList, startIndex);
                }
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.lvEmployees_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show("Exception in lvEmployees_ColumnClick():" + ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void Employees_Load(object sender, System.EventArgs e)
		{
			try
			{                
				this.Cursor = Cursors.WaitCursor;
				InitialiseObserverClient();
                
                statuses.Add(Constants.statusActive);
                statuses.Add(Constants.statusBlocked);
                //07.08.2015 Boris, da svi operateri vide RETIRED zaposlene
                statuses.Add(Constants.statusRetired);

				sortOrder = Constants.sortAsc;
				sortField = Employees.LastNameIndex;
				startIndex = 0;

				menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
				currentRoles = NotificationController.GetCurrentRoles();
				menuItemID = NotificationController.GetCurrentMenuItemID();

				setVisibility();
                btnPrev.Visible = false;
                btnNext.Visible = false;

                //BLOKIRANJE -> status najnoviji ODBLOKIRANO 18.01.2018
                 /*            
               btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                */
				wUnits = new List<WorkingUnitTO>();
                oUnits = new List<OrganizationalUnitTO>();
				if (logInUser != null)
				{
					wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                    oUnits = new OrganizationalUnit().Search(); //MM
                    //new ApplUserXOrgUnit().FindUsersForOU(logInUser.UserID.Trim(), Constants.EmployeesPurpose, status));
                }

				foreach (WorkingUnitTO wUnit in wUnits)
				{
					wuString += wUnit.WorkingUnitID.ToString().Trim() + ","; 
				}
				
				if (wuString.Length > 0)
				{
					wuString = wuString.Substring(0, wuString.Length - 1);
				}

                foreach (OrganizationalUnitTO oUnit in oUnits)
                {
                    ouString += oUnit.OrgUnitID.ToString().Trim() + ",";
                }

                if (ouString.Length > 0)
                {
                    ouString = ouString.Substring(0, ouString.Length - 1);
                }

				populateWorkingUnitCombo();
				populateHasTagCombo();
                

				types.Add(rm.GetString("emplOrdinary", culture), Constants.emplOrdinary);
				types.Add(rm.GetString("emplExtraOrdinary", culture), Constants.emplExtraOrdinary);
				types.Add(rm.GetString("emplSpecial", culture), Constants.emplSpecial);

				populateTypes();
                populateOrganizationalUnitCombo();//MM
                ApplUsersXRole admin = new ApplUsersXRole();
                List<ApplUserTO> adminUserList = admin.FindUsersForRoleID(0);
                foreach (ApplUserTO user in adminUserList)
                {
                    if (user.UserID == logInUser.UserID)
                    {
                        statuses = new List<string>();
                        break;
                    }
                }
				
				if (!wuString.Equals(""))
				{
                    //Console.WriteLine("Before SearchTagsWithStatuses: " + DateTime.Now);
                    //currentEmployeesList = new Employee().SearchTagsWithStatuses(statuses, wuString, (int) Constants.HasTag.all);
                    //Console.WriteLine("After SearchTagsWithStatuses: " + DateTime.Now);
                    //lblTotal.Text = rm.GetString("lblTotal", culture) + currentEmployeesList.Count.ToString().Trim();
                    //Console.WriteLine("Before currentEmployeesList.Sort: " + DateTime.Now);
                    //currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
                    //Console.WriteLine("After currentEmployeesList.Sort: " + DateTime.Now);
                    //Console.WriteLine("Before populateListView: " + DateTime.Now);
                    //populateListView(currentEmployeesList, startIndex);
                    //Console.WriteLine("After populateListView: " + DateTime.Now);
				}
				else
				{
					MessageBox.Show(rm.GetString("noEmplPrivilege", culture));
				}

                Common.Rule rule = new Common.Rule();
                rule.RuleTO.EmployeeTypeID = (int)Constants.EmployeeTypesFIAT.BC;
                rule.RuleTO.WorkingUnitID = Constants.defaultWorkingUnitID;
                rule.RuleTO.RuleType = Constants.RuleWrittingDataToTag;
                List<RuleTO> rules = rule.Search();
                if (rules.Count > 0 && rules[0].RuleValue == Constants.yesInt)
                    writeDataToTag = true;

				// Start Tag's tracking
				dataMgr.StartTagsTracking();

                filter = new Filter();
                filter.SerachButton = this.btnSearch;
                filter.LoadFilters(cbFilter, this,rm.GetString("newFilter",culture));				
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.Employees_Load(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

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

				cbWorkingUnitID.DataSource = wuArray;
				cbWorkingUnitID.DisplayMember = "Name";
				cbWorkingUnitID.ValueMember = "WorkingUnitID";
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.populateWorkigUnitCombo(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        //19.06.2017 Miodrag Mitrovic / f-ja za ucitavanje cb za organizacione jedinice.
        private void populateOrganizationalUnitCombo()
        {
            try
            {
                

                foreach (OrganizationalUnitTO ouTO in oUnits)
                {
                    ouArray.Add(ouTO);
                }

                ouArray.Insert(0, new OrganizationalUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), ""));

                cbOrganizationalUnitID.DataSource = ouArray;
                cbOrganizationalUnitID.DisplayMember = "Name";
                cbOrganizationalUnitID.ValueMember = "OrgUnitID";
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employees.populateOrganizationUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		private void populateHasTagCombo()
		{
			try
			{
				ArrayList array = new ArrayList();
				array.Add(rm.GetString("all", culture));
				array.Add(rm.GetString("yes", culture));
				array.Add(rm.GetString("no", culture));

				cbHasTag.DataSource = array;
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.populateHasTagCombo(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void populateTypes()
		{
			try
			{
				List<string> array = new List<string>();

				array.Add(rm.GetString("all", culture));

				foreach (string type in types.Keys)
				{
					array.Add(type);
				}
				
				cbType.DataSource = array;
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.populateTypes(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        		
		private void Employees_Closed(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = Cursors.WaitCursor;
				Controller.DettachFromNotifier(this.observerClient);
				currentEmployee = new EmployeeTO();
				if (dataMgr.CheckNewTags(null))
				{
					//DialogResult proc = MessageBox.Show(rm.GetString("StartProcLog", culture), "", MessageBoxButtons.YesNo);
					DialogResult proc = MessageBox.Show(rm.GetString("updateTagsNow", culture),"", MessageBoxButtons.YesNo);
					if(proc == DialogResult.Yes)
					{
                        dataMgr.FinalizeTagsTracking(true, true, null, null);
					}
					if(proc == DialogResult.No)
					{
						dataMgr.FinalizeTagsTracking(false, true, null, null);
					}
				}
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.Employees_Closed(): " + ex.Message + "\n");
				MessageBox.Show(rm.GetString("notUpdateTagsNow", culture));
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
				EmployeeAdd addForm = new EmployeeAdd(-1, tbFirstName.Text.Trim(), tbLastName.Text.Trim(),
                    (cbWorkingUnitID.SelectedIndex > 0 ? cbWorkingUnitID.Text : ""),
                    (cbType.SelectedIndex > 0 ? cbType.Text : ""), false);
				addForm.ShowDialog(this);

                this.Cursor = Cursors.WaitCursor;

				//Reload form only if some employee is added
				if (addForm.doReloadOnBack)
				{
                    //tbEmployeeID.Text = "";
                    //tbFirstName.Text = "";
                    //tbLastName.Text = "";
                    //cbWorkingUnitID.SelectedIndex = 0;
                    //cbHasTag.SelectedIndex = 0;
                    //cbType.SelectedIndex = 0;

					if (!wuString.Equals(""))
					{
                        //currentEmployeesList = new Employee().SearchTagsWithStatuses(statuses, wuString, (int) Constants.HasTag.all);
                        //lblTotal.Text = rm.GetString("lblTotal", culture) + currentEmployeesList.Count.ToString().Trim();
                        //startIndex = 0;
                        //currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
                        //populateListView(currentEmployeesList, startIndex);
                        btnSearch.PerformClick();
					}
					else
					{
						MessageBox.Show(rm.GetString("noEmplPrivilege", culture));
					}
				}
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.btnAdd_Click(): " + ex.Message + "\n");
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
				if (lvEmployees.SelectedItems.Count == 1)
				{
					int selectedEmployeeID = currentEmployee.EmployeeID;
					EmployeeAdd addForm = new EmployeeAdd(currentEmployee.EmployeeID, "", "", "", "", currentEmployee.HasTag);
					addForm.ShowDialog(this);

                    this.Cursor = Cursors.WaitCursor;
                    //tbEmployeeID.Text = "";
                    //tbFirstName.Text = "";
                    //tbLastName.Text = "";
                    //cbWorkingUnitID.SelectedIndex = 0;
                    //cbHasTag.SelectedIndex = 0;
                    //cbType.SelectedIndex = 0;

					if (!wuString.Equals(""))
					{
                        //Employee empl = new Employee();
                        //empl.EmplTO.EmployeeID = currentEmployee.EmployeeID;
                        //List<EmployeeTO> updatedEmployees = empl.SearchTagsWithStatuses(statuses, wuString, (int) Constants.HasTag.all);
                        //if ((updatedEmployees.Count == 1) && (updatedEmployees[0].EmployeeID != -1))
                        //{
                        //    EmployeeTO updatedEmployee = updatedEmployees[0];

                        //    lvEmployees.SelectedItems[0].Text = updatedEmployee.EmployeeID.ToString().Trim();
                        //    lvEmployees.SelectedItems[0].SubItems[1].Text = updatedEmployee.FirstName.Trim();
                        //    lvEmployees.SelectedItems[0].SubItems[2].Text = updatedEmployee.LastName.Trim();
                        //    lvEmployees.SelectedItems[0].SubItems[3].Text = updatedEmployee.WorkingUnitName.Trim();
                        //    lvEmployees.SelectedItems[0].SubItems[4].Text = updatedEmployee.Status.Trim();
                        //    //lvEmployees.SelectedItems[0].SubItems[5].Text = updatedEmployee.Password.Trim();
                        //    if (updatedEmployee.HasTag)
                        //    {
                        //        lvEmployees.SelectedItems[0].SubItems[5].Text = rm.GetString("yes", culture);
                        //    }
                        //    else
                        //    {
                        //        lvEmployees.SelectedItems[0].SubItems[5].Text = rm.GetString("no", culture);
                        //    }
                        //    if (updatedEmployee.Type.Equals(Constants.emplOrdinary))
                        //    {
                        //        lvEmployees.SelectedItems[0].SubItems[6].Text = rm.GetString("emplOrdinary", culture);
                        //    }
                        //    else if (updatedEmployee.Type.Equals(Constants.emplExtraOrdinary))
                        //    {
                        //        lvEmployees.SelectedItems[0].SubItems[6].Text = rm.GetString("emplExtraOrdinary", culture);
                        //    }
                        //    else if (updatedEmployee.Type.Equals(Constants.emplSpecial))
                        //    {
                        //        lvEmployees.SelectedItems[0].SubItems[6].Text = rm.GetString("emplSpecial", culture);
                        //    }
                        //    lvEmployees.SelectedItems[0].SubItems[7].Text = updatedEmployee.SchemaName.Trim();
                        //    lvEmployees.SelectedItems[0].SubItems[8].Text = updatedEmployee.ScheduleDate.ToString("dd/MM/yyy");
						//}

						/*currentEmployeesList = currentEmployee.SearchTagsWithStatuses("", "", "", "", statuses, "", "", "", "", (int) Constants.HasTag.all, wuString, "");
						lblTotal.Text = rm.GetString("lblTotal", culture) + currentEmployeesList.Count.ToString().Trim();	
						startIndex = 0;
						currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
						populateListView(currentEmployeesList, startIndex);*/
                        btnSearch.PerformClick();
					}
					else
					{
						MessageBox.Show(rm.GetString("noEmplPrivilege", culture));
					}
				}
				else
				{
					MessageBox.Show(rm.GetString("messageEmployeeNotSelected", culture));
				}
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.btnUpdate_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		public void InitialiseObserverClient()
		{
			Controller = NotificationController.GetInstance();
			observerClient = new NotificationObserverClient(this.ToString());
			Controller.AttachToNotifier(observerClient);
			this.observerClient.Notification += new NotificationEventHandler(this.EmployeeChangedEvent);
		}

		//Bilja, 13.09.2007, not need any more. The same code is in this event, and in
		//Update/Add click event, so, everything was done twice
		//And with this, listview is reload before returning to Update click event, and upd record is lost
		//That is why call to this event in EmployeeAdd.cs, in Save/Upd functions is commented
		public void EmployeeChangedEvent(object sender, NotificationEventArgs args)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (args.isEmployeeChanged)
                {

                    if (!wuString.Equals(""))
                    {
                        currentEmployeesList = new Employee().SearchTagsWithStatuses(statuses, wuString, (int)Constants.HasTag.all);
                        if (currentEmployeesList.Count > 0)
                        {
                            lblTotal.Text = rm.GetString("lblTotal", culture) + currentEmployeesList.Count.ToString().Trim();
                            currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
                            startIndex = 0;
                            populateListView(currentEmployeesList, startIndex);
                        }
                        currentEmployee = new EmployeeTO();
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noEmplPrivilege", culture));
                    }
                }

                tbEmployeeID.Text = "";
                tbFirstName.Text = "";
                tbLastName.Text = "";
                cbWorkingUnitID.SelectedIndex = 0;
                cbOrganizationalUnitID.SelectedIndex = 0;
                cbHasTag.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employees.EmployeeChangedEvent(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally { 
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
                log.writeLog(DateTime.Now + " Employees.btnClose_Click(): " + ex.Message + "\n");
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
				log.writeLog(DateTime.Now + " Employees.setVisibility(): " + ex.Message + "\n");
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
				populateListView(currentEmployeesList, startIndex);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.btnPrev_Click(): " + ex.Message + "\n");
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
				populateListView(currentEmployeesList, startIndex);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " Employees.btnNext_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

        private void tbEmployeeID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (tbEmployeeID.Text.ToString().Trim() != "")
                {
                    for (int i = tbEmployeeID.Text.Length - 1; i >= 0; i--)
                    {
                        try
                        {
                            int digit = Int32.Parse(tbEmployeeID.Text.Substring(i, 1));
                        }
                        catch
                        {
                            MessageBox.Show(rm.GetString("valueNotNum", culture));
                            tbEmployeeID.Text = tbEmployeeID.Text.Remove(i, 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employees.tbEmployeeID_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.tbEmployeeID.Text = "";
                this.tbFirstName.Text = "";
                this.tbLastName.Text = "";
                this.cbOrganizationalUnitID.SelectedIndex = 0; //mm
                this.cbWorkingUnitID.SelectedIndex = 0;
                this.cbHasTag.SelectedIndex = 0;
                this.cbType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void tbFirstName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textChanged && tbFirstName.Focused)
                {
                    // do not invoke list view selected items changed event
                    populateFilter = false;
                    int i = 0;
                    int p = 0;

                    this.Cursor = Cursors.WaitCursor;
                    foreach (EmployeeTO employee in currentEmployeesList)
                    {
                        if (employee.FirstName.ToUpper().StartsWith(tbFirstName.Text.Trim().ToUpper()))
                        {
                            p = i / Constants.recordsPerPage;
                            startIndex = p * Constants.recordsPerPage;
                            populateListView(currentEmployeesList, startIndex);
                            break;
                        }

                        i++;
                    }

                    lvEmployees.SelectedItems.Clear();
                    lvEmployees.Select();
                    lvEmployees.Invalidate();

                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        if (item.SubItems[1].Text.Trim().ToUpper().StartsWith(tbFirstName.Text.Trim().ToUpper()))
                        {
                            item.Selected = true;
                            lvEmployees.Select();
                            lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));
                            currentEmployee = (EmployeeTO)item.Tag;

                            break;
                        }
                    }

                    tbFirstName.Focus();
                    populateFilter = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employees.tbFirstName_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void tbLastName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textChanged && tbLastName.Focused)
                {
                    // do not invoke list view selected items changed event
                    populateFilter = false;
                    int i = 0;
                    int p = 0;

                    this.Cursor = Cursors.WaitCursor;
                    foreach (EmployeeTO employee in currentEmployeesList)
                    {
                        if (employee.LastName.ToUpper().StartsWith(tbLastName.Text.Trim().ToUpper()))
                        {
                            p = i / Constants.recordsPerPage;
                            startIndex = p * Constants.recordsPerPage;
                            populateListView(currentEmployeesList, startIndex);
                            break;
                        }

                        i++;
                    }

                    lvEmployees.SelectedItems.Clear();
                    lvEmployees.Select();
                    lvEmployees.Invalidate();

                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        if (item.SubItems[2].Text.Trim().ToUpper().StartsWith(tbLastName.Text.Trim().ToUpper()))
                        {
                            item.Selected = true;
                            lvEmployees.Select();
                            lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));
                            currentEmployee = (EmployeeTO)item.Tag;

                            break;
                        }
                    }

                    tbLastName.Focus();
                    populateFilter = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employees.tbLastName_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Employees_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " Employees.Employees_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Employees_KeyDown(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " Employees.Employees_KeyUp(): " + ex.Message + "\n");
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
                    cbWorkingUnitID.SelectedIndex = cbWorkingUnitID.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
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

        private void cbWorkingUnitID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                bool check = false;
                foreach (WorkingUnitTO wu in wUnits)
                {
                    if (cbWorkingUnitID.SelectedIndex != 0)
                    {
                        if (wu.WorkingUnitID == (int)cbWorkingUnitID.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                        {
                            check = true;
                        }
                    }
                }

                chbHierarhicly.Checked = check;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employees.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
           
        }
        


        private void btnSaveCriteria_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                UIFeatures.FilterAdd filterAdd = new UIFeatures.FilterAdd(this,filter);
                filterAdd.ShowDialog();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employees.btnSaveCriteria_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Employees.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (lvEmployees.Items.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("visits1");
                    DataTable tableI = new DataTable("images");

                    tableCR.Columns.Add("row1", typeof(System.String));
                    tableCR.Columns.Add("row2", typeof(System.String));
                    tableCR.Columns.Add("row3", typeof(System.String));
                    tableCR.Columns.Add("row4", typeof(System.String));
                    tableCR.Columns.Add("row5", typeof(System.String));
                    tableCR.Columns.Add("row6", typeof(System.String));
                    tableCR.Columns.Add("row7", typeof(System.String));
                    tableCR.Columns.Add("row8", typeof(System.String));
                    tableCR.Columns.Add("row9", typeof(System.String));
                    tableCR.Columns.Add("row10", typeof(System.String));
                    tableCR.Columns.Add("row11", typeof(System.String));
                    tableCR.Columns.Add("row12", typeof(System.String));
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

                    List<string> rowNames = new List<string>();

                    for (int i = 0; i < lvEmployees.Columns.Count; i++)
                    {
                        if (checkedColumns.Contains(i))
                        {
                            rowNames.Add(lvEmployees.Columns[i].Text.ToString());
                        }
                    }
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        DataRow row = tableCR.NewRow();


                        // row["row1"] = item.SubItems[0].Text.ToString().Trim() + " " + item.SubItems[1].Text.ToString().Trim();
                        int j = 1;
                        for (int i = 0; i < lvEmployees.Columns.Count; i++)
                        {
                            if (checkedColumns.Contains(i))
                            {
                                //if (i == SDocumentIndex)
                                //{
                                //    string[] s = item.SubItems[i].Text.ToString().Trim().Split(' ');
                                //    string s1 = "";
                                //    foreach (string s2 in s)
                                //    {
                                //        if (s2.Length > 0)
                                //            s1 = s1 + s2[0];
                                //    }
                                //    row["row" + j] = s1;
                                //}
                                //else
                                //{
                                row["row" + j] = item.SubItems[i].Text.ToString();
                                // }
                                j++;
                            }
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
                    string selEmplID = tbEmployeeID.Text.ToString();
                    string selLastName = tbLastName.Text.ToString();
                    string selFirstName = tbFirstName.Text.ToString();
                    string selCardNum = cbHasTag.Text.ToString();
                    string selType = cbType.Text.ToString();
                    string selWU = cbWorkingUnitID.Text.ToString();


                    if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                    {
                        Reports.Reports_sr.EmplCRView view = new Reports.Reports_sr.EmplCRView(dataSetCR,
                            selEmplID, selFirstName, selLastName, selType, selCardNum, selWU,
                            rowNames);
                        view.ShowDialog(this);
                    }
                    if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                    {
                        Reports.Reports_fi.EmplCRView_fi view = new Reports.Reports_fi.EmplCRView_fi(dataSetCR,
                            selEmplID, selFirstName, selLastName, selType, selCardNum, selWU,
                            rowNames);
                        view.ShowDialog(this);
                    }
                    if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                    {
                        Reports.Reports_en.EmplCRView_en view = new Reports.Reports_en.EmplCRView_en(dataSetCR,
                            selEmplID, selFirstName, selLastName, selType, selCardNum, selWU,
                            rowNames);
                        view.ShowDialog(this);
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
                log.writeLog(DateTime.Now + " Employees.btnReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private bool deleteEmployee(EmployeeTO emplTO, Dictionary<int, EmployeeAsco4TO> ascoDict, Dictionary<int, WorkTimeSchemaTO> dictTimeSchema)
        {
            bool emplDeleted = true;
            try
            {
                Employee empl = new Employee();
                TagTO tagTO = empl.FindActive(emplTO.EmployeeID);

                // get asco record
                EmployeeAsco4 asco = new EmployeeAsco4();
                EmployeeAsco4Hist ascoHist = new EmployeeAsco4Hist();
                
                EmployeeAsco4TO ascoTO = new EmployeeAsco4TO();
                if (ascoDict.ContainsKey(emplTO.EmployeeID))
                {
                    ascoTO = ascoDict[emplTO.EmployeeID];
                    ascoHist.EmplAsco4TO = new EmployeeAsco4TO(ascoTO);
                }                

                bool updateAsco = false;

                if (ascoTO.DatetimeValue3.Equals(new DateTime()))
                {
                    ascoTO.DatetimeValue3 = DateTime.Now.Date;
                    updateAsco = true;
                }

                ApplUserTO applUserTOUpdate = new ApplUserTO();
                List<EmployeeTimeScheduleTO> emplTimeScheduleInsert = new List<EmployeeTimeScheduleTO>();
                List<EmployeeTimeScheduleTO> emplTimeSchedulesDel = new List<EmployeeTimeScheduleTO>();
                EmployeesTimeSchedule emplTimeSchedule = new EmployeesTimeSchedule();
                DateTime retiredDate = ascoTO.DatetimeValue3;
                bool reprocessDays = false;
                List<DateTime> dayToReprocess = new List<DateTime>();

                // retire user if exists
                if (ascoTO.NVarcharValue5 != "")
                {
                    ApplUser applUser = new ApplUser();
                    applUser.UserTO.UserID = ascoTO.NVarcharValue5;
                    applUser.UserTO.Status = Constants.statusActive;
                    List<ApplUserTO> usersTO = applUser.Search();
                    if (usersTO.Count > 0)
                    {
                        applUserTOUpdate = usersTO[0];
                        applUserTOUpdate.Status = Constants.statusRetired;
                    }
                }
                
                //set time schedule to default and delete all io_pairs_processed   
                // if last working day is entering third shift, set leaving third shift for first retired day and leave pairs for that day, and set default schema to second retired day
                emplTimeSchedulesDel = emplTimeSchedule.SearchEmployeesSchedules(emplTO.EmployeeID.ToString(), retiredDate.Date, new DateTime());
                bool firstRetiredDayThirdShiftLeaving = false;
                WorkTimeSchemaTO retiredDaySchema = new WorkTimeSchemaTO();
                // get intervals for last working day
                Dictionary<int, List<EmployeeTimeScheduleTO>> lastWorkingSchList = emplTimeSchedule.SearchEmployeesSchedulesExactDate(emplTO.EmployeeID.ToString().Trim(),
                    retiredDate.Date.AddDays(-1), retiredDate.Date.AddDays(-1), null);

                if (lastWorkingSchList.ContainsKey(emplTO.EmployeeID) && lastWorkingSchList[emplTO.EmployeeID].Count > 0)
                {
                    List<WorkTimeIntervalTO> lastDayIntervals = Common.Misc.getTimeSchemaInterval(retiredDate.AddDays(-1), lastWorkingSchList[emplTO.EmployeeID], dictTimeSchema);

                    // check if last schedule has midnight ending interval                                        
                    foreach (WorkTimeIntervalTO intTO in lastDayIntervals)
                    {
                        if (intTO.EndTime.Hour == 23 && intTO.EndTime.Minute == 59 && intTO.EndTime.TimeOfDay.Subtract(intTO.StartTime.TimeOfDay).TotalMinutes > 0)
                        {
                            firstRetiredDayThirdShiftLeaving = true;
                            if (dictTimeSchema.ContainsKey(intTO.TimeSchemaID))
                                retiredDaySchema = dictTimeSchema[intTO.TimeSchemaID];
                            break;
                        }
                    }
                }

                if (firstRetiredDayThirdShiftLeaving)
                {
                    // get ending third shift day and insert that schedule into first retired day
                    // after that insert default schema
                    int startSchemaDay = -1;
                    foreach (int day in retiredDaySchema.Days.Keys)
                    {
                        if (retiredDaySchema.Days[day].Count == 1)
                        {
                            foreach (WorkTimeIntervalTO intTO in retiredDaySchema.Days[day].Values)
                            {
                                if (intTO.StartTime.Hour == 0 && intTO.StartTime.Minute == 0 && intTO.EndTime.TimeOfDay.Subtract(intTO.StartTime.TimeOfDay).TotalMinutes > 0)
                                {
                                    startSchemaDay = day;
                                    break;
                                }
                            }
                        }

                        if (startSchemaDay != -1)
                            break;
                    }

                    if (retiredDaySchema.TimeSchemaID != -1 && startSchemaDay != -1)
                    {
                        EmployeeTimeScheduleTO etsRetiredInsert = new EmployeeTimeScheduleTO();
                        etsRetiredInsert.EmployeeID = emplTO.EmployeeID;
                        etsRetiredInsert.StartCycleDay = startSchemaDay;
                        etsRetiredInsert.Date = retiredDate.Date;
                        etsRetiredInsert.TimeSchemaID = retiredDaySchema.TimeSchemaID;
                        emplTimeScheduleInsert.Add(etsRetiredInsert);
                    }
                }

                EmployeeTimeScheduleTO etsInsert = new EmployeeTimeScheduleTO();
                etsInsert.EmployeeID = emplTO.EmployeeID;
                etsInsert.StartCycleDay = 0;
                if (firstRetiredDayThirdShiftLeaving)
                    etsInsert.Date = retiredDate.Date.AddDays(1);
                else
                    etsInsert.Date = retiredDate.Date;
                etsInsert.TimeSchemaID = Constants.defaultSchemaID;
                emplTimeScheduleInsert.Add(etsInsert);

                DateTime endDate = new IOPairProcessed().getMaxDateOfPair(emplTO.EmployeeID.ToString(), null);
                if (endDate == new DateTime())
                    endDate = DateTime.Now.Date;

                List<DateTime> datesList = new List<DateTime>();
                DateTime startReprocessDay = firstRetiredDayThirdShiftLeaving ? retiredDate.Date.AddDays(1) : retiredDate.Date;
                for (DateTime dt = startReprocessDay; dt <= endDate; dt = dt.AddDays(1))
                {
                    datesList.Add(dt);
                }

                if (endDate >= retiredDate.Date)
                {
                    reprocessDays = true;
                    dayToReprocess = datesList;
                }
                
                if (empl.BeginTransaction())
                {
                    try
                    {
                        EmployeeHist emplHist = new EmployeeHist();                        
                        emplHist.EmplHistTO = new EmployeeHistTO(emplTO);
                        emplHist.EmplHistTO.ValidTo = DateTime.Now.Date;

                        empl.EmplTO = emplTO;
                        empl.EmplTO.Status = Constants.statusRetired;
                        emplDeleted = empl.Update(false);

                        if (emplDeleted)
                        {
                            emplHist.SetTransaction(empl.GetTransaction());
                            emplDeleted = emplDeleted && (emplHist.Save(false) > 0);
                        }

                        if (emplDeleted)
                        {
                            // change asco if needed
                            if (updateAsco)
                            {
                                ascoHist.SetTransaction(empl.GetTransaction());
                                emplDeleted = emplDeleted && ascoHist.save(false);

                                if (emplDeleted)
                                {
                                    asco.EmplAsco4TO = ascoTO;
                                    asco.SetTransaction(empl.GetTransaction());
                                    emplDeleted = emplDeleted && asco.update(false);
                                }
                            }
                        }

                        if (emplDeleted)
                        {
                            emplTimeSchedule.SetTransaction(empl.GetTransaction());
                            // change schedule
                            if (emplTimeSchedulesDel.Count > 0)
                            {
                                emplDeleted = emplDeleted && emplTimeSchedule.DeleteFromToSchedule(emplTO.EmployeeID, retiredDate.Date, new DateTime(), "", false);
                            }

                            if (emplDeleted)
                            {
                                foreach (EmployeeTimeScheduleTO schIns in emplTimeScheduleInsert)
                                {
                                    emplDeleted = emplDeleted && emplTimeSchedule.Save(schIns.EmployeeID, schIns.Date, schIns.TimeSchemaID, schIns.StartCycleDay, "", false) > 0;

                                    if (!emplDeleted)
                                        break;
                                }
                            }
                        }

                        if (emplDeleted)
                        {
                            // reproces pairs if needed
                            if (reprocessDays)
                            {
                                DateTime startDate = new DateTime();
                                DateTime endReprocessDate = new DateTime();

                                foreach (DateTime date in dayToReprocess)
                                {
                                    if (startDate == new DateTime() || startDate > date)
                                    {
                                        startDate = date;
                                    }
                                    if (endReprocessDate == new DateTime() || endReprocessDate < date)
                                        endReprocessDate = date;
                                }

                                //list of datetime for each employee
                                Dictionary<int, List<DateTime>> emplDateWholeDayList = new Dictionary<int, List<DateTime>>();
                                emplDateWholeDayList.Add(emplTO.EmployeeID, dayToReprocess);

                                if (dayToReprocess.Count > 0)
                                    emplDeleted = emplDeleted && Common.Misc.ReprocessPairsAndRecalculateCounters(emplTO.EmployeeID.ToString(), startDate.Date, endReprocessDate.Date, empl.GetTransaction(), emplDateWholeDayList, null, "");
                            }

                            if (tagTO.TagID > 0 && emplDeleted)
                            {
                                Tags tags = new Tags();
                                tags.setStatus("RETIRED", writeDataToTag);
                                tags.setEmployeeName(emplTO.FirstName + " " + emplTO.LastName);
                                tags.setOwnerID(emplTO.EmployeeID);
                                tags.setTransaction(empl.GetTransaction());
                                tags.ShowDialog(this);

                                emplDeleted = emplDeleted && tagChanged;
                            }
                        }

                        if (emplDeleted)
                        {
                            if (!applUserTOUpdate.UserID.Trim().Equals(""))
                            {
                                ApplUser user = new ApplUser();
                                user.UserTO = applUserTOUpdate;
                                user.SetTransaction(empl.GetTransaction());
                                emplDeleted = emplDeleted && user.Update(false);
                            }
                        }

                        if (emplDeleted)
                        {
                            empl.CommitTransaction();                            
                        }
                        else
                        {
                            if (empl.GetTransaction() != null)
                                empl.RollbackTransaction();                            
                        }                        
                    }
                    catch (Exception ex)
                    {
                        if (empl.GetTransaction() != null)
                            empl.RollbackTransaction();

                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employees.deleteEmployee(): " + ex.Message + "\n");
                emplDeleted = false;
            }

            return emplDeleted;
        }

        public void setTagChanged(bool changed)
        {
            this.tagChanged = changed;
        }
        
        // method for checking employees schedules, changed every checking time according to checking purpose
        private void btnCheckCounters_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DateTime date = dtpShiftDay.Value.Date;
                Dictionary<int, int> dict = Common.Misc.GetEmployeeFundHrs(tbEmployeeID.Text.Trim(), new DateTime(date.Year, date.Month, 1), new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1));

                string msg = "";

                foreach (int id in dict.Keys)
                {
                    msg += id.ToString() + ": " + dict[id].ToString() + Environment.NewLine;
                }

                MessageBox.Show(msg.Trim());

                return;

                //EmployeePYDataBuffer buff = new EmployeePYDataBuffer();
                //List<EmployeePYDataBufferTO> list = buff.getEmployeeBuffers(420);
                //Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> oldCounters = new EmployeeCounterValue().SearchValuesAll();

                //EmployeeCounterValue counter = new EmployeeCounterValue();
                //EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();

                //int succ = 0;
                //int changed = 0;
                //DateTime modTime = DateTime.Now;
                //string modBy = "SYS";
                //List<int> skipEmpl = new List<int>();                
                //skipEmpl.Add(101085);
                //skipEmpl.Add(106835);
                //skipEmpl.Add(104370);
                //skipEmpl.Add(106315);
                //skipEmpl.Add(103513);
                //skipEmpl.Add(106643);
                //skipEmpl.Add(101280);
                //skipEmpl.Add(105218);
                //skipEmpl.Add(104637);
                //skipEmpl.Add(105586);
                //skipEmpl.Add(101186);
                //skipEmpl.Add(105649);
                //skipEmpl.Add(101062);
                //skipEmpl.Add(101231);
                //skipEmpl.Add(106264);
                //skipEmpl.Add(107853);
                //skipEmpl.Add(107860);
                //skipEmpl.Add(107828);
                //skipEmpl.Add(107608);
                //skipEmpl.Add(108065);
                //skipEmpl.Add(100823);
                //skipEmpl.Add(103102);
                //skipEmpl.Add(108025);
                //skipEmpl.Add(101200);
                //skipEmpl.Add(100478);
                //skipEmpl.Add(107214);
                //skipEmpl.Add(100839);
                //foreach (EmployeePYDataBufferTO buffTO in list)
                //{
                //    if (skipEmpl.Contains(buffTO.EmployeeID))
                //        continue;

                //    if (!oldCounters.ContainsKey(buffTO.EmployeeID))
                //        continue;

                //    if (!oldCounters[buffTO.EmployeeID].ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter) 
                //        || !oldCounters[buffTO.EmployeeID].ContainsKey((int)Constants.EmplCounterTypes.NotJustifiedOvertime))
                //        continue;

                //    if (buffTO.EmployeeType != "BC")
                //        continue;

                //    if (buffTO.BankHrsBalans >= 0)
                //        continue;

                //    if (buffTO.NotJustifiedOvertimeBalans <= 0)
                //        continue;

                //    int bhBalans = (int)(buffTO.BankHrsBalans * 60);

                //    int movement = buffTO.NotJustifiedOvertimeBalans;

                //    if (buffTO.NotJustifiedOvertimeBalans >= Math.Abs(bhBalans))
                //        movement = Math.Abs(bhBalans);

                //    if (movement <= 0)
                //        continue;

                //    changed++;
                //    if (counter.BeginTransaction())
                //    {
                //        try
                //        {                            
                //            counterHist.SetTransaction(counter.GetTransaction());

                //            counterHist.ValueTO = new EmployeeCounterValueHistTO(oldCounters[buffTO.EmployeeID][(int)Constants.EmplCounterTypes.BankHoursCounter]);
                //            counterHist.ValueTO.ModifiedBy = modBy;
                //            counterHist.ValueTO.ModifiedTime = modTime;
                //            counterHist.Save(false);

                //            counterHist.ValueTO = new EmployeeCounterValueHistTO(oldCounters[buffTO.EmployeeID][(int)Constants.EmplCounterTypes.NotJustifiedOvertime]);
                //            counterHist.ValueTO.ModifiedBy = modBy;
                //            counterHist.ValueTO.ModifiedTime = modTime;
                //            counterHist.Save(false);

                //            counter.ValueTO = new EmployeeCounterValueTO(oldCounters[buffTO.EmployeeID][(int)Constants.EmplCounterTypes.BankHoursCounter]);
                //            counter.ValueTO.ModifiedBy = modBy;
                //            counter.ValueTO.ModifiedTime = modTime;
                //            counter.ValueTO.Value += movement;
                //            counter.Update(false);

                //            counter.ValueTO = new EmployeeCounterValueTO(oldCounters[buffTO.EmployeeID][(int)Constants.EmplCounterTypes.NotJustifiedOvertime]);
                //            counter.ValueTO.ModifiedBy = modBy;
                //            counter.ValueTO.ModifiedTime = modTime;
                //            counter.ValueTO.Value -= movement;
                //            counter.Update(false);

                //            counter.CommitTransaction();
                //            succ++;
                //        }
                //        catch (Exception ex)
                //        {
                //            if (counter.GetTransaction() != null)
                //                counter.RollbackTransaction();
                //            log.writeLog(DateTime.Now + " EmployeesCounters(): Employee: " + buffTO.EmployeeID + " not changed. Exception: " + ex.Message + "\n");
                //        }
                //    }
                //    else
                //        log.writeLog(DateTime.Now + " EmployeesCounters(): Employee: " + buffTO.EmployeeID + " not changed.\n");
                //}

                //MessageBox.Show("CHANGED: " + changed.ToString() + " SUCC:" + succ.ToString());
                //log.writeLog(DateTime.Now + " CHANGED: " + changed.ToString() + " SUCC:" + succ.ToString());

                //return;
                                                
                DateTime from = new DateTime(2013, 7, 1);
                DateTime to = new DateTime(2014, 1, 1);
                                
                string wunits = "";
                List<WorkingUnitTO> wuList = new WorkingUnit().Search();
                foreach (WorkingUnitTO wuTO in wuList)
                {
                    wunits += wuTO.WorkingUnitID.ToString() + ",";
                }

                if (wunits.Length > 0)
                    wunits = wunits.Substring(0, wunits.Length - 1);
                                
                // get employees from selected working unit that are not currently loaned to other working unit or are currently loand to selected working unit                        
                List<EmployeeTO> employeeList = new Employee().SearchByWULoans(wunits, -1, null, from.Date, to.Date);

                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary("");
                string emplIDs = "";
                foreach (EmployeeTO empl in employeeList)
                {
                    if (empl.Status == Constants.statusActive && ascoDict.ContainsKey(empl.EmployeeID) && ascoDict[empl.EmployeeID].IntegerValue4 >= -5 && ascoDict[empl.EmployeeID].IntegerValue4 <= -2)
                        emplIDs += empl.EmployeeID.ToString().Trim() + ",";
                }

                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                Dictionary<int, PassTypeTO> ptDict = new PassType().SearchDictionary();
                
                Dictionary<int, Dictionary<int, int>> counters = new EmployeeCounterValue().Search(emplIDs);
                
                List<int> bhList = new List<int>();
                bhList.Add(48);
                bhList.Add(1048);
                bhList.Add(2048);
                bhList.Add(3048);

                List<int> bhUsedList = new List<int>();
                bhUsedList.Add(42);
                bhUsedList.Add(1042);
                bhUsedList.Add(2042);
                bhUsedList.Add(3042);

                List<int> swList = new List<int>();
                swList.Add(49);
                swList.Add(1049);
                swList.Add(2049);
                swList.Add(3049);

                List<int> swDoneList = new List<int>();
                swDoneList.Add(50);
                swDoneList.Add(1050);
                swDoneList.Add(2050);
                swDoneList.Add(3050);

                List<int> overtimeList = new List<int>();
                overtimeList.Add(4);
                overtimeList.Add(1004);
                overtimeList.Add(2004);
                overtimeList.Add(3004);

                List<DateTime> dates = new List<DateTime>();

                dates = new List<DateTime>();
                for (DateTime currDate = from.Date; currDate.Date < to.Date; currDate = currDate.AddDays(1).Date)
                {
                    dates.Add(currDate.Date);
                }
                List<IOPairProcessedTO> allPairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, dates, "");
                
                Dictionary<int, int> bhValues = new Dictionary<int, int>();
                Dictionary<int, int> swValues = new Dictionary<int, int>();
                Dictionary<int, int> overtimeValues = new Dictionary<int, int>();
                Dictionary<int, int> plValues = new Dictionary<int, int>();

                DateTime ovBorderDate = dtpShiftDay.Value.Date;
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedulesFrom = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, from.Date, from.Date, null);
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, ovBorderDate, ovBorderDate, null);
                Dictionary<int, WorkTimeSchemaTO> schDict = new TimeSchema().getDictionary();

                dates = new List<DateTime>();
                dates.Add(ovBorderDate.Date);
                List<IOPairProcessedTO> borderDayPairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, dates, "");
                Dictionary<int, List<IOPairProcessedTO>> emplBorderDayPairs = new Dictionary<int, List<IOPairProcessedTO>>();
                foreach (IOPairProcessedTO pair in borderDayPairs)
                {
                    if (!emplBorderDayPairs.ContainsKey(pair.EmployeeID))
                        emplBorderDayPairs.Add(pair.EmployeeID, new List<IOPairProcessedTO>());

                    emplBorderDayPairs[pair.EmployeeID].Add(pair);
                }

                dates = new List<DateTime>();
                dates.Add(from.Date);
                List<IOPairProcessedTO> borderDayPairsFrom = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, dates, "");
                Dictionary<int, List<IOPairProcessedTO>> emplBorderDayPairsFrom = new Dictionary<int, List<IOPairProcessedTO>>();
                foreach (IOPairProcessedTO pair in borderDayPairsFrom)
                {
                    if (!emplBorderDayPairsFrom.ContainsKey(pair.EmployeeID))
                        emplBorderDayPairsFrom.Add(pair.EmployeeID, new List<IOPairProcessedTO>());

                    emplBorderDayPairsFrom[pair.EmployeeID].Add(pair);
                }

                foreach (IOPairProcessedTO pair in allPairs)
                {
                    if (pair.IOPairDate.Date == from.Date)
                    {
                        List<EmployeeTimeScheduleTO> schedulesFrom = new List<EmployeeTimeScheduleTO>();
                        if (emplSchedulesFrom.ContainsKey(pair.EmployeeID))
                            schedulesFrom = emplSchedulesFrom[pair.EmployeeID];
                        List<IOPairProcessedTO> dayPairsFrom = new List<IOPairProcessedTO>();
                        if (emplBorderDayPairsFrom.ContainsKey(pair.EmployeeID))
                            dayPairsFrom = emplBorderDayPairsFrom[pair.EmployeeID];
                        if (Common.Misc.isPreviousDayPair(pair, ptDict, dayPairsFrom, Common.Misc.getTimeSchema(from.Date, schedulesFrom, schDict), Common.Misc.getTimeSchemaInterval(from.Date, schedulesFrom, schDict)))
                            continue;
                    }

                    int pairDuration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;
                    if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                        pairDuration++;

                    if (overtimeList.Contains(pair.PassTypeID))
                    {
                        if (pair.IOPairDate.Date < ovBorderDate)
                            continue;

                        if (pair.IOPairDate.Date == ovBorderDate.Date)
                        {
                            List<EmployeeTimeScheduleTO> schedules = new List<EmployeeTimeScheduleTO>();
                            if (emplSchedules.ContainsKey(pair.EmployeeID))
                                schedules = emplSchedules[pair.EmployeeID];
                            List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                            if (emplBorderDayPairs.ContainsKey(pair.EmployeeID))
                                dayPairs = emplBorderDayPairs[pair.EmployeeID];
                            if (Common.Misc.isPreviousDayPair(pair, ptDict, dayPairs, Common.Misc.getTimeSchema(ovBorderDate.Date, schedules, schDict), Common.Misc.getTimeSchemaInterval(ovBorderDate.Date, schedules, schDict)))
                                continue;
                        }

                        if (!overtimeValues.ContainsKey(pair.EmployeeID))
                            overtimeValues.Add(pair.EmployeeID, 0);

                        overtimeValues[pair.EmployeeID] += pairDuration;
                    }

                    if (bhList.Contains(pair.PassTypeID))
                    {
                        if (!bhValues.ContainsKey(pair.EmployeeID))
                            bhValues.Add(pair.EmployeeID, 0);

                        bhValues[pair.EmployeeID] += pairDuration;
                    }

                    if (bhUsedList.Contains(pair.PassTypeID))
                    {
                        if (!bhValues.ContainsKey(pair.EmployeeID))
                            bhValues.Add(pair.EmployeeID, 0);

                        int bhRounding = 30;

                        if (pairDuration % bhRounding != 0)
                        {
                            pairDuration += bhRounding - pairDuration % bhRounding;
                        }

                        bhValues[pair.EmployeeID] -= pairDuration;
                    }

                    if (swList.Contains(pair.PassTypeID))
                    {
                        if (!swValues.ContainsKey(pair.EmployeeID))
                            swValues.Add(pair.EmployeeID, 0);

                        swValues[pair.EmployeeID] += pairDuration;
                    }

                    if (swDoneList.Contains(pair.PassTypeID))
                    {
                        if (!swValues.ContainsKey(pair.EmployeeID))
                            swValues.Add(pair.EmployeeID, 0);
                        
                        swValues[pair.EmployeeID] -= pairDuration;
                    }

                    if (ptDict.ContainsKey(pair.PassTypeID) && ptDict[pair.PassTypeID].LimitCompositeID != -1 && pair.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                    {
                        if (!plValues.ContainsKey(pair.EmployeeID))
                            plValues.Add(pair.EmployeeID, 0);

                        plValues[pair.EmployeeID]++;
                    }
                }

                string report = "ID|Name|Company|Pairs|Counter|Type" + Environment.NewLine;
                foreach (EmployeeTO empl in employeeList)
                {                    
                    int bhCounter = 0;
                    int bhPairs = 0;
                    int ovCounter = 0;
                    int ovPairs = 0;
                    int swCounter = 0;
                    int swPairs = 0;
                    int plCounter = 0;
                    int plPairs = 0;

                    if (counters.ContainsKey(empl.EmployeeID))
                    {
                        if (counters[empl.EmployeeID].ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                            bhCounter = counters[empl.EmployeeID][(int)Constants.EmplCounterTypes.BankHoursCounter];
                        if (counters[empl.EmployeeID].ContainsKey((int)Constants.EmplCounterTypes.OvertimeCounter))
                            ovCounter = counters[empl.EmployeeID][(int)Constants.EmplCounterTypes.OvertimeCounter];
                        if (counters[empl.EmployeeID].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                            swCounter = counters[empl.EmployeeID][(int)Constants.EmplCounterTypes.StopWorkingCounter];
                        if (counters[empl.EmployeeID].ContainsKey((int)Constants.EmplCounterTypes.PaidLeaveCounter))
                            plCounter = counters[empl.EmployeeID][(int)Constants.EmplCounterTypes.PaidLeaveCounter];
                    }

                    if (overtimeValues.ContainsKey(empl.EmployeeID))
                        ovPairs = overtimeValues[empl.EmployeeID];
                    if (bhValues.ContainsKey(empl.EmployeeID))
                        bhPairs = bhValues[empl.EmployeeID];
                    if (swValues.ContainsKey(empl.EmployeeID))
                        swPairs = swValues[empl.EmployeeID];
                    if (plValues.ContainsKey(empl.EmployeeID))
                        plPairs = plValues[empl.EmployeeID];

                    if (bhCounter != bhPairs)
                    {
                        report += empl.EmployeeID.ToString().Trim() + "|";
                        report += empl.FirstAndLastName.Trim() + "|";
                        if (ascoDict.ContainsKey(empl.EmployeeID))
                            report += ascoDict[empl.EmployeeID].IntegerValue4.ToString().Trim() + "|";
                        else
                            report += "N/A|";
                        report += bhPairs.ToString().Trim() + "|";
                        report += bhCounter.ToString().Trim() + "|";
                        report += "BH" + Environment.NewLine;
                    }

                    if (swCounter != swPairs)
                    {
                        report += empl.EmployeeID.ToString().Trim() + "|";
                        report += empl.FirstAndLastName.Trim() + "|";
                        if (ascoDict.ContainsKey(empl.EmployeeID))
                            report += ascoDict[empl.EmployeeID].IntegerValue4.ToString().Trim() + "|";
                        else
                            report += "N/A|";
                        report += swPairs.ToString().Trim() + "|";
                        report += swCounter.ToString().Trim() + "|";
                        report += "SW" + Environment.NewLine;
                    }

                    if (plCounter != plPairs)
                    {
                        report += empl.EmployeeID.ToString().Trim() + "|";
                        report += empl.FirstAndLastName.Trim() + "|";
                        if (ascoDict.ContainsKey(empl.EmployeeID))
                            report += ascoDict[empl.EmployeeID].IntegerValue4.ToString().Trim() + "|";
                        else
                            report += "N/A|";
                        report += plPairs.ToString().Trim() + "|";
                        report += plCounter.ToString().Trim() + "|";
                        report += "PL" + Environment.NewLine;
                    }

                    if (ovCounter != ovPairs)
                    {
                        report += empl.EmployeeID.ToString().Trim() + "|";
                        report += empl.FirstAndLastName.Trim() + "|";
                        if (ascoDict.ContainsKey(empl.EmployeeID))
                            report += ascoDict[empl.EmployeeID].IntegerValue4.ToString().Trim() + "|";
                        else
                            report += "N/A|";
                        report += ovPairs.ToString().Trim() + "|";
                        report += ovCounter.ToString().Trim() + "|";
                        report += "OV" + Environment.NewLine;
                    }
                }

                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InvalidCounters" + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";

                StreamWriter writer = new StreamWriter(filePath, true, System.Text.Encoding.Unicode);

                writer.WriteLine(report);
                writer.Close();

                return;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employees.btnCheckCounters_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentEmployeesList.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;                

                string emplIDs = "";

                foreach (EmployeeTO empl in currentEmployeesList)
                {                    
                    emplIDs += empl.EmployeeID.ToString().Trim() + ",";
                }

                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, dtpShiftDay.Value.Date, dtpShiftDay.Value.Date, null);
                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary(emplIDs);
                Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema().getDictionary();

                string report = "ID\tStringone\tName\tShift\tCycleDay\tIntervals\tDate" + Environment.NewLine;
                foreach (EmployeeTO empl in currentEmployeesList)
                {
                    report += empl.EmployeeID.ToString().Trim() + "\t";

                    if (ascoDict.ContainsKey(empl.EmployeeID))
                        report += ascoDict[empl.EmployeeID].NVarcharValue2.Trim();
                    else
                        report += "N/A";

                    report += "\t" + empl.FirstAndLastName.Trim() + "\t";

                    // get shift
                    List<EmployeeTimeScheduleTO> schedules = new List<EmployeeTimeScheduleTO>();
                    if (emplSchedules.ContainsKey(empl.EmployeeID))
                        schedules = emplSchedules[empl.EmployeeID];

                    WorkTimeSchemaTO sch = Common.Misc.getTimeSchema(dtpShiftDay.Value.Date, schedules, schemas);

                    if (sch.TimeSchemaID != -1)
                        report += sch.Description.Trim();
                    else
                        report += "N/A";

                    List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(dtpShiftDay.Value.Date, schedules, schemas);

                    int cycleDay = 0;
                    string intervalsString = "";
                    foreach (WorkTimeIntervalTO interval in intervals)
                    {
                        cycleDay = interval.DayNum + 1;

                        intervalsString += interval.StartTime.ToString(Constants.timeFormat) + "-" + interval.EndTime.ToString(Constants.timeFormat);

                        if (!interval.Description.Trim().Equals(""))
                            intervalsString += "(" + interval.Description.Trim() + ")";

                        intervalsString += "; ";
                    }

                    report += "\t" + cycleDay.ToString().Trim() + "\t";
                    report += intervalsString.Trim() + "\t";                    

                    report += dtpShiftDay.Value.Date.ToString("dd.MM.yyyy.") + Environment.NewLine;
                }

                if (report.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }

                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Shift" + dtpShiftDay.Value.Date.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
                
                StreamWriter writer = new StreamWriter(filePath, true, System.Text.Encoding.Unicode);
                
                writer.WriteLine(report);
                writer.Close();

                MessageBox.Show(rm.GetString("shiftReportGenerated", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employees.btnCheckSchedules_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}  
}
