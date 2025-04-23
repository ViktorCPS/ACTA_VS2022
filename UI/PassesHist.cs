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
    public class PassesHist : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.Label lblDirection;
        private System.Windows.Forms.ComboBox cbDirection;
        private System.Windows.Forms.Label lblPassType;
        private System.Windows.Forms.ComboBox cbPassType;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.ComboBox cbLocation;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ListView lvPassesHist;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.ListView lvPass;
        private ComboBox cbOperator;
        private Label lblOperator;
        private GroupBox gbPassesHist;
        private GroupBox gbPass;
        private Label lblIsWrkHrs;
        private GroupBox gbModifiedTime;
        private DateTimePicker dtpModifiedFrom;
        private Label lblModifiedFrom;
        private DateTimePicker dtpModifiedTo;
        private Label lblModifiedTo;
        private GroupBox gbEventTime;

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
        const int ModifiedByIndex = 7;
        const int ModifiedTimeIndex = 8;
        const int RemarksIndex = 9;

        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        ApplUserTO logInUser;
        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;
        string menuItemID;
        private List<WorkingUnitTO> wUnits;
        private string wuString = "";
        private string OperatorString = "";
        private int passesHistListCount = 0;

        bool readPermission = false;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;

        List<PassHistTO> currentPassHistList;
        List<PassTO> currentPassList;
        private int sortOrder;
        private int sortField;
        private System.Windows.Forms.Label lblTotal;
        private Button btnReport;
        private int startIndex;

        private string[] statuses = { Constants.statusActive, Constants.statusBlocked };
        
        List<EmployeeTO> currentEmployeeList;

        const int EmployeeIDIndex2 = 0;
        const int EmployeeNameIndex2 = 1;
        const int WUNameIndex2 = 2;
        private CheckBox cbEventTime;
        private CheckBox cbModifiedTime;
        private Button btnWUTree;
        private Button btnLocationTree;

        const int DateIndex2 = 3;
        private CheckBox chbHierarhicly;
        List<LocationTO> locArray;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;
        List<WorkingUnitTO> wuArray;

        Filter filter;

        public PassesHist()
        {
            InitializeComponent();
            this.lblTotal.Visible = false;
            this.btnPrev.Visible = false;
            this.btnNext.Visible = false;
            this.CenterToScreen();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);
            setLanguage();
            currentPass = new PassTO();
            currentPassHist = new PassHistTO();
            currentPassHistList = new List<PassHistTO>();
            currentPassList = new List<PassTO>();
            currentEmployeeList = new List<EmployeeTO>();
            logInUser = NotificationController.GetLogInUser();
        }

        public PassesHist(PassTO pass)
        {
            InitializeComponent();
            this.CenterToScreen();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);
            setLanguage();

            this.btnPrev.Visible = false;
            this.btnNext.Visible = false;

            currentPass = pass;
            currentPassList = new List<PassTO>();
            currentPassHist = new PassHistTO(-1, pass.PassID, pass.EmployeeID, pass.Direction, pass.EventTime, pass.PassTypeID, pass.IsWrkHrsCount, pass.LocationID, pass.PairGenUsed,
                pass.ManualyCreated, "", pass.CreatedBy, pass.CreatedTime, "", new DateTime());
            currentPassHistList = new PassHist().Find(pass.PassID);
            passesHistListCount = currentPassHistList.Count;
            clearPassView();
            if (currentPassHistList.Count > 0)
            {
                startIndex = 0;
                populateListView(currentPassHistList, startIndex);
                this.lblTotal.Visible = true;
                this.lblTotal.Text = rm.GetString("lblTotal", culture) + currentPassHistList.Count.ToString().Trim();
                setEventTime();
            }
            else
            {
                this.lblTotal.Visible = true;
                this.lblTotal.Text = rm.GetString("lblTotal", culture) + "0";
            }
            populatePassView(currentPass);
            currentEmployeeList = new List<EmployeeTO>();
            currentPassList.Add(pass);
            logInUser = NotificationController.GetLogInUser();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PassesHist));
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnLocationTree = new System.Windows.Forms.Button();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbEventTime = new System.Windows.Forms.CheckBox();
            this.cbModifiedTime = new System.Windows.Forms.CheckBox();
            this.gbModifiedTime = new System.Windows.Forms.GroupBox();
            this.dtpModifiedFrom = new System.Windows.Forms.DateTimePicker();
            this.lblModifiedFrom = new System.Windows.Forms.Label();
            this.dtpModifiedTo = new System.Windows.Forms.DateTimePicker();
            this.lblModifiedTo = new System.Windows.Forms.Label();
            this.gbEventTime = new System.Windows.Forms.GroupBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.cbOperator = new System.Windows.Forms.ComboBox();
            this.lblOperator = new System.Windows.Forms.Label();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cbPassType = new System.Windows.Forms.ComboBox();
            this.lblPassType = new System.Windows.Forms.Label();
            this.cbDirection = new System.Windows.Forms.ComboBox();
            this.lblDirection = new System.Windows.Forms.Label();
            this.lblWU = new System.Windows.Forms.Label();
            this.lvPassesHist = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnReport = new System.Windows.Forms.Button();
            this.lvPass = new System.Windows.Forms.ListView();
            this.gbPassesHist = new System.Windows.Forms.GroupBox();
            this.gbPass = new System.Windows.Forms.GroupBox();
            this.lblIsWrkHrs = new System.Windows.Forms.Label();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbSearch.SuspendLayout();
            this.gbModifiedTime.SuspendLayout();
            this.gbEventTime.SuspendLayout();
            this.gbPassesHist.SuspendLayout();
            this.gbPass.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.chbHierarhicly);
            this.gbSearch.Controls.Add(this.btnLocationTree);
            this.gbSearch.Controls.Add(this.btnWUTree);
            this.gbSearch.Controls.Add(this.cbEventTime);
            this.gbSearch.Controls.Add(this.cbModifiedTime);
            this.gbSearch.Controls.Add(this.gbModifiedTime);
            this.gbSearch.Controls.Add(this.gbEventTime);
            this.gbSearch.Controls.Add(this.cbOperator);
            this.gbSearch.Controls.Add(this.lblOperator);
            this.gbSearch.Controls.Add(this.cbEmployee);
            this.gbSearch.Controls.Add(this.lblEmployee);
            this.gbSearch.Controls.Add(this.cbWU);
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Controls.Add(this.cbLocation);
            this.gbSearch.Controls.Add(this.lblLocation);
            this.gbSearch.Controls.Add(this.cbPassType);
            this.gbSearch.Controls.Add(this.lblPassType);
            this.gbSearch.Controls.Add(this.cbDirection);
            this.gbSearch.Controls.Add(this.lblDirection);
            this.gbSearch.Controls.Add(this.lblWU);
            this.gbSearch.Location = new System.Drawing.Point(13, 12);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(745, 254);
            this.gbSearch.TabIndex = 1;
            this.gbSearch.TabStop = false;
            this.gbSearch.Tag = "FILTERABLE";
            this.gbSearch.Text = "Search";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(112, 48);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 42;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // btnLocationTree
            // 
            this.btnLocationTree.Image = ((System.Drawing.Image)(resources.GetObject("btnLocationTree.Image")));
            this.btnLocationTree.Location = new System.Drawing.Point(276, 112);
            this.btnLocationTree.Name = "btnLocationTree";
            this.btnLocationTree.Size = new System.Drawing.Size(25, 23);
            this.btnLocationTree.TabIndex = 41;
            this.btnLocationTree.Click += new System.EventHandler(this.btnLocationTree_Click);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(326, 22);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 40;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbEventTime
            // 
            this.cbEventTime.AutoSize = true;
            this.cbEventTime.Checked = true;
            this.cbEventTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEventTime.Location = new System.Drawing.Point(19, 154);
            this.cbEventTime.Name = "cbEventTime";
            this.cbEventTime.Size = new System.Drawing.Size(15, 14);
            this.cbEventTime.TabIndex = 21;
            this.cbEventTime.UseVisualStyleBackColor = true;
            this.cbEventTime.CheckedChanged += new System.EventHandler(this.cbEventTime_Changed);
            // 
            // cbModifiedTime
            // 
            this.cbModifiedTime.AutoSize = true;
            this.cbModifiedTime.Location = new System.Drawing.Point(339, 154);
            this.cbModifiedTime.Name = "cbModifiedTime";
            this.cbModifiedTime.Size = new System.Drawing.Size(15, 14);
            this.cbModifiedTime.TabIndex = 20;
            this.cbModifiedTime.UseVisualStyleBackColor = true;
            this.cbModifiedTime.CheckedChanged += new System.EventHandler(this.cbModifiedTime_Changed);
            // 
            // gbModifiedTime
            // 
            this.gbModifiedTime.Controls.Add(this.dtpModifiedFrom);
            this.gbModifiedTime.Controls.Add(this.lblModifiedFrom);
            this.gbModifiedTime.Controls.Add(this.dtpModifiedTo);
            this.gbModifiedTime.Controls.Add(this.lblModifiedTo);
            this.gbModifiedTime.Enabled = false;
            this.gbModifiedTime.Location = new System.Drawing.Point(360, 154);
            this.gbModifiedTime.Name = "gbModifiedTime";
            this.gbModifiedTime.Size = new System.Drawing.Size(242, 94);
            this.gbModifiedTime.TabIndex = 19;
            this.gbModifiedTime.TabStop = false;
            this.gbModifiedTime.Text = "Modified time";
            // 
            // dtpModifiedFrom
            // 
            this.dtpModifiedFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpModifiedFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpModifiedFrom.Location = new System.Drawing.Point(69, 16);
            this.dtpModifiedFrom.Name = "dtpModifiedFrom";
            this.dtpModifiedFrom.Size = new System.Drawing.Size(160, 20);
            this.dtpModifiedFrom.TabIndex = 11;
            // 
            // lblModifiedFrom
            // 
            this.lblModifiedFrom.Location = new System.Drawing.Point(14, 16);
            this.lblModifiedFrom.Name = "lblModifiedFrom";
            this.lblModifiedFrom.Size = new System.Drawing.Size(49, 23);
            this.lblModifiedFrom.TabIndex = 10;
            this.lblModifiedFrom.Text = "From:";
            this.lblModifiedFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpModifiedTo
            // 
            this.dtpModifiedTo.CustomFormat = "dd.MM.yyyy";
            this.dtpModifiedTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpModifiedTo.Location = new System.Drawing.Point(69, 56);
            this.dtpModifiedTo.Name = "dtpModifiedTo";
            this.dtpModifiedTo.Size = new System.Drawing.Size(160, 20);
            this.dtpModifiedTo.TabIndex = 13;
            // 
            // lblModifiedTo
            // 
            this.lblModifiedTo.Location = new System.Drawing.Point(17, 56);
            this.lblModifiedTo.Name = "lblModifiedTo";
            this.lblModifiedTo.Size = new System.Drawing.Size(46, 23);
            this.lblModifiedTo.TabIndex = 12;
            this.lblModifiedTo.Text = "To:";
            this.lblModifiedTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbEventTime
            // 
            this.gbEventTime.Controls.Add(this.dtpFrom);
            this.gbEventTime.Controls.Add(this.lblFrom);
            this.gbEventTime.Controls.Add(this.dtpTo);
            this.gbEventTime.Controls.Add(this.lblTo);
            this.gbEventTime.Location = new System.Drawing.Point(47, 154);
            this.gbEventTime.Name = "gbEventTime";
            this.gbEventTime.Size = new System.Drawing.Size(239, 94);
            this.gbEventTime.TabIndex = 18;
            this.gbEventTime.TabStop = false;
            this.gbEventTime.Text = "Event time";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(65, 19);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(160, 20);
            this.dtpFrom.TabIndex = 11;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(10, 16);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(49, 23);
            this.lblFrom.TabIndex = 10;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(65, 57);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(160, 20);
            this.dtpTo.TabIndex = 13;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(13, 56);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(46, 23);
            this.lblTo.TabIndex = 12;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbOperator
            // 
            this.cbOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOperator.FormattingEnabled = true;
            this.cbOperator.Location = new System.Drawing.Point(432, 20);
            this.cbOperator.Name = "cbOperator";
            this.cbOperator.Size = new System.Drawing.Size(160, 21);
            this.cbOperator.TabIndex = 17;
            // 
            // lblOperator
            // 
            this.lblOperator.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblOperator.Location = new System.Drawing.Point(357, 27);
            this.lblOperator.Name = "lblOperator";
            this.lblOperator.Size = new System.Drawing.Size(69, 13);
            this.lblOperator.TabIndex = 16;
            this.lblOperator.Text = "Operator:";
            this.lblOperator.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmployee
            // 
            this.cbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmployee.Location = new System.Drawing.Point(112, 79);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(160, 21);
            this.cbEmployee.TabIndex = 3;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(34, 77);
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
            this.cbWU.Size = new System.Drawing.Size(208, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(617, 225);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(110, 23);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.Location = new System.Drawing.Point(112, 116);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(160, 21);
            this.cbLocation.TabIndex = 9;
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(18, 114);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(88, 23);
            this.lblLocation.TabIndex = 8;
            this.lblLocation.Text = "Location:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPassType
            // 
            this.cbPassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPassType.Location = new System.Drawing.Point(432, 51);
            this.cbPassType.Name = "cbPassType";
            this.cbPassType.Size = new System.Drawing.Size(160, 21);
            this.cbPassType.TabIndex = 5;
            // 
            // lblPassType
            // 
            this.lblPassType.Location = new System.Drawing.Point(338, 49);
            this.lblPassType.Name = "lblPassType";
            this.lblPassType.Size = new System.Drawing.Size(88, 23);
            this.lblPassType.TabIndex = 4;
            this.lblPassType.Text = "Pass Type:";
            this.lblPassType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbDirection
            // 
            this.cbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDirection.Location = new System.Drawing.Point(432, 88);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new System.Drawing.Size(160, 21);
            this.cbDirection.TabIndex = 7;
            // 
            // lblDirection
            // 
            this.lblDirection.Location = new System.Drawing.Point(338, 88);
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
            // lvPassesHist
            // 
            this.lvPassesHist.FullRowSelect = true;
            this.lvPassesHist.GridLines = true;
            this.lvPassesHist.HideSelection = false;
            this.lvPassesHist.Location = new System.Drawing.Point(8, 16);
            this.lvPassesHist.MultiSelect = false;
            this.lvPassesHist.Name = "lvPassesHist";
            this.lvPassesHist.Size = new System.Drawing.Size(877, 214);
            this.lvPassesHist.TabIndex = 3;
            this.lvPassesHist.UseCompatibleStateImageBehavior = false;
            this.lvPassesHist.View = System.Windows.Forms.View.Details;
            this.lvPassesHist.SelectedIndexChanged += new System.EventHandler(this.lvPassesHist_SelectedIndexChanged);
            this.lvPassesHist.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvPasses_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(831, 612);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(865, 243);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 12;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(828, 243);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 11;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(753, 511);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(152, 16);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(428, 612);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(75, 23);
            this.btnReport.TabIndex = 8;
            this.btnReport.Text = "Report";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // lvPass
            // 
            this.lvPass.GridLines = true;
            this.lvPass.Location = new System.Drawing.Point(8, 16);
            this.lvPass.Name = "lvPass";
            this.lvPass.Size = new System.Drawing.Size(877, 52);
            this.lvPass.TabIndex = 13;
            this.lvPass.UseCompatibleStateImageBehavior = false;
            this.lvPass.View = System.Windows.Forms.View.Details;
            // 
            // gbPassesHist
            // 
            this.gbPassesHist.Controls.Add(this.lvPassesHist);
            this.gbPassesHist.Location = new System.Drawing.Point(12, 272);
            this.gbPassesHist.Name = "gbPassesHist";
            this.gbPassesHist.Size = new System.Drawing.Size(893, 238);
            this.gbPassesHist.TabIndex = 14;
            this.gbPassesHist.TabStop = false;
            this.gbPassesHist.Text = "History of change";
            // 
            // gbPass
            // 
            this.gbPass.Controls.Add(this.lvPass);
            this.gbPass.Location = new System.Drawing.Point(12, 530);
            this.gbPass.Name = "gbPass";
            this.gbPass.Size = new System.Drawing.Size(893, 76);
            this.gbPass.TabIndex = 15;
            this.gbPass.TabStop = false;
            this.gbPass.Text = "Pass";
            // 
            // lblIsWrkHrs
            // 
            this.lblIsWrkHrs.AutoSize = true;
            this.lblIsWrkHrs.Location = new System.Drawing.Point(10, 612);
            this.lblIsWrkHrs.Name = "lblIsWrkHrs";
            this.lblIsWrkHrs.Size = new System.Drawing.Size(91, 13);
            this.lblIsWrkHrs.TabIndex = 16;
            this.lblIsWrkHrs.Text = "* Is working hours";
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(764, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(142, 100);
            this.gbFilter.TabIndex = 28;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(34, 56);
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
            this.cbFilter.Location = new System.Drawing.Point(8, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // PassesHist
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(919, 647);
            this.ControlBox = false;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.lblIsWrkHrs);
            this.Controls.Add(this.gbPass);
            this.Controls.Add(this.gbPassesHist);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "PassesHist";
            this.ShowInTaskbar = false;
            this.Text = "History of change";
            this.Load += new System.EventHandler(this.Passes_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PassesHist_KeyUp);
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.gbModifiedTime.ResumeLayout(false);
            this.gbEventTime.ResumeLayout(false);
            this.gbPassesHist.ResumeLayout(false);
            this.gbPass.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Inner Class for sorting Array List of Passes

        /*
		 *  Class used for sorting Array List of Passes
		*/

        private class ArrayListSort : IComparer<PassHistTO>
        {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(PassHistTO x, PassHistTO y)
            {
                PassHistTO passHist1 = null;
                PassHistTO passHist2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    passHist1 = x;
                    passHist2 = y;
                }
                else
                {
                    passHist1 = y;
                    passHist2 = x;
                }

                switch (compField)
                {
                    case PassesHist.PassIDIndex:
                        return passHist1.PassID.CompareTo(passHist2.PassID);
                    case PassesHist.EmployeeIDIndex:
                        return passHist1.EmployeeName.CompareTo(passHist2.EmployeeName);
                    case PassesHist.DirectionIndex:
                        return passHist1.Direction.CompareTo(passHist2.Direction);
                    case PassesHist.EventTimeIndex:
                        return passHist1.EventTime.CompareTo(passHist2.EventTime);
                    case PassesHist.PassTypeIDIndex:
                        return passHist1.PassType.CompareTo(passHist2.PassType);
                    case PassesHist.IsWrkHrsIndex:
                        return passHist1.IsWrkHrsCount.CompareTo(passHist2.IsWrkHrsCount);
                    case PassesHist.LocationIDIndex:
                        return passHist1.LocationName.CompareTo(passHist2.LocationName);
                    case PassesHist.ModifiedByIndex:
                        return passHist1.ModifiedBy.CompareTo(passHist2.ModifiedBy);
                    case PassesHist.ModifiedTimeIndex:
                        return passHist1.ModifiedTime.CompareTo(passHist2.ModifiedTime);
                    case PassesHist.RemarksIndex:
                        return passHist1.Remarks.CompareTo(passHist2.Remarks);
                    default:
                        return passHist1.EventTime.CompareTo(passHist2.EventTime);
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
                    case PassesHist.EmployeeIDIndex2:
                        {
                            return CaseInsensitiveComparer.Default.Compare(employee1.EmployeeID, employee2.EmployeeID);
                        }
                    case PassesHist.EmployeeNameIndex2:
                        {
                            return CaseInsensitiveComparer.Default.Compare(employee1.LastName.Trim(), employee2.LastName.Trim());
                        }
                    case PassesHist.WUNameIndex2:
                        {
                            return CaseInsensitiveComparer.Default.Compare(employee1.WorkingUnitName.Trim(), employee2.WorkingUnitName.Trim());
                        }
                    case PassesHist.DateIndex2:
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
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                btnReport.Text = rm.GetString("btnReport", culture);

                // Form name
                this.Text = rm.GetString("PHform", culture);

                // group box text
                gbSearch.Text = rm.GetString("gbSearch", culture);
                gbPassesHist.Text = rm.GetString("gbPassesHist", culture);
                gbPass.Text = rm.GetString("gbCurrentPass", culture);
                gbEventTime.Text = rm.GetString("gbEventTime", culture);
                gbModifiedTime.Text = rm.GetString("gbModifiedTime", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);

                //check box's text
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);

                // label's text
                lblWU.Text = rm.GetString("lblWU", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblDirection.Text = rm.GetString("lblPrimDirect", culture);
                lblPassType.Text = rm.GetString("lblPassType", culture);
                lblLocation.Text = rm.GetString("lblLocation", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblModifiedFrom.Text = rm.GetString("lblFrom", culture);
                lblModifiedTo.Text = rm.GetString("lblTo", culture);
                lblOperator.Text = rm.GetString("lblOperator", culture);
                lblIsWrkHrs.Text = rm.GetString("lblIsWH", culture);

                // list passesHist view initialization
                lvPassesHist.BeginUpdate();
                lvPassesHist.Columns.Add(rm.GetString("hdrPassID", culture), (lvPassesHist.Width - 10) / 10 - 17, HorizontalAlignment.Left);
                lvPassesHist.Columns.Add(rm.GetString("hdrEmployee", culture), (lvPassesHist.Width - 10) / 10 + 15, HorizontalAlignment.Left);
                lvPassesHist.Columns.Add(rm.GetString("hdrDirection", culture), (lvPassesHist.Width - 10) / 10 - 45, HorizontalAlignment.Left);
                lvPassesHist.Columns.Add(rm.GetString("hdrEventTime", culture), (lvPassesHist.Width - 10) / 10 + 20, HorizontalAlignment.Left);
                lvPassesHist.Columns.Add(rm.GetString("hdrPassTypeID", culture), (lvPassesHist.Width - 10) / 10, HorizontalAlignment.Left);
                lvPassesHist.Columns.Add(rm.GetString("hdrIsWrkHrs", culture), (lvPassesHist.Width - 10) / 10 - 35, HorizontalAlignment.Left);
                lvPassesHist.Columns.Add(rm.GetString("hdrLocation", culture), (lvPassesHist.Width - 10) / 10 + 20, HorizontalAlignment.Left);
                lvPassesHist.Columns.Add(rm.GetString("hdrModifiedBy", culture), (lvPassesHist.Width - 10) / 10 - 20, HorizontalAlignment.Left);
                lvPassesHist.Columns.Add(rm.GetString("hdrModifiedTime", culture), (lvPassesHist.Width - 10) / 10 + 20, HorizontalAlignment.Left);
                lvPassesHist.Columns.Add(rm.GetString("hdrRemarks", culture), (lvPassesHist.Width - 10) / 10 + 37, HorizontalAlignment.Left);
                lvPassesHist.EndUpdate();

                // list view pass initialization
                lvPass.BeginUpdate();
                lvPass.Columns.Add(rm.GetString("hdrPassID", culture), (lvPass.Width - 9) / 10 - 17, HorizontalAlignment.Left);
                lvPass.Columns.Add(rm.GetString("hdrEmployee", culture), (lvPass.Width - 9) / 10 + 15, HorizontalAlignment.Left);
                lvPass.Columns.Add(rm.GetString("hdrDirection", culture), (lvPass.Width - 9) / 10 - 45, HorizontalAlignment.Left);
                lvPass.Columns.Add(rm.GetString("hdrEventTime", culture), (lvPass.Width - 9) / 10 + 20, HorizontalAlignment.Left);
                lvPass.Columns.Add(rm.GetString("hdrPassType", culture), (lvPass.Width - 9) / 10, HorizontalAlignment.Left);
                lvPass.Columns.Add(rm.GetString("hdrIsWrkHrs", culture), (lvPass.Width - 9) / 10 - 35, HorizontalAlignment.Left);
                lvPass.Columns.Add(rm.GetString("hdrLocation", culture), (lvPass.Width - 9) / 10 + 20, HorizontalAlignment.Left);
                lvPass.Columns.Add(rm.GetString("hdrPairGenUsed", culture), (lvPass.Width - 9) / 10 + 62, HorizontalAlignment.Left);
                lvPass.Columns.Add(rm.GetString("hdrManualCreated", culture), (lvPass.Width - 9) / 10 + 62, HorizontalAlignment.Left);
                lvPass.EndUpdate();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesHist.setLanguage(): " + ex.Message + "\n");
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
                wuArray = new List<WorkingUnitTO>();
                
                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    wuArray.Add(wuTO);
                }
                
                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWU.DataSource = wuArray;
                cbWU.DisplayMember = "Name";
                cbWU.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
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
                List<EmployeeTO> emplArray = new List<EmployeeTO>();
                string workUnitID = wuID.ToString();
                if (wuID == -1)
                {
                    emplArray = new Employee().SearchByWU(wuString);
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
                    emplArray = new Employee().SearchByWU(workUnitID);
                }
                
                foreach (EmployeeTO employee in emplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                emplArray.Insert(0, empl);

                cbEmployee.DataSource = emplArray;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
                cbEmployee.Invalidate();
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populatePassTypeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Set Event time value Combo Box
        /// </summary>
        private void setEventTime()
        {
            DateTime maxEventTime = new DateTime();
            DateTime minEventTime = new DateTime();

            foreach (PassHistTO passHist in currentPassHistList)
            {
                if (maxEventTime == new DateTime() && minEventTime == new DateTime())
                {
                    maxEventTime = passHist.EventTime;
                    minEventTime = passHist.EventTime;
                }
                else
                {
                    if (passHist.EventTime > maxEventTime)
                    {
                        maxEventTime = passHist.EventTime;
                    }
                    if (passHist.EventTime < minEventTime)
                    {
                        minEventTime = passHist.EventTime;
                    }
                }
            }
            dtpFrom.Value = maxEventTime;
            dtpTo.Value = minEventTime;
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateWorkingUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateOperatorCombo()
        {
            try
            {                
                List<ApplUserTO> auArray = new ApplUser().Search();
                auArray.Insert(0, new ApplUserTO(rm.GetString("all", culture), "", "", "", 0, "", 0, ""));

                cbOperator.DataSource = auArray;
                cbOperator.DisplayMember = "UserID";
                cbOperator.ValueMember = "UserID";
                cbOperator.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateOperatorCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate List View with Passes found
        /// </summary>
        /// <param name="locationsList"></param>
        private void populateListView(List<PassHistTO> passesHistList, int startIndex)
        {
            try
            {
                if (passesHistList.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvPassesHist.BeginUpdate();
                lvPassesHist.Items.Clear();

                if (passesHistList.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < passesHistList.Count))
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
                        if (lastIndex >= passesHistList.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = passesHistList.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            PassHistTO passHist = passesHistList[i];
                            ListViewItem item = new ListViewItem();
                            item.Text = passHist.PassID.ToString().Trim();
                            item.SubItems.Add(passHist.EmployeeName.Trim());
                            item.SubItems.Add(passHist.Direction.Trim());
                            if (!passHist.EventTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                            {
                                item.SubItems.Add(passHist.EventTime.ToString("dd/MM/yyyy   HH:mm"));
                            }
                            else
                            {
                                item.SubItems.Add("");
                            }
                            item.SubItems.Add(passHist.PassType.Trim());
                            if (passHist.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter)
                            {
                                item.SubItems.Add(rm.GetString("yes", culture));
                            }
                            else
                            {
                                item.SubItems.Add(rm.GetString("no", culture));
                            }
                            item.SubItems.Add(passHist.LocationName.Trim());
                            item.SubItems.Add(passHist.ModifiedBy.Trim());
                            if (!passHist.ModifiedTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                            {
                                item.SubItems.Add(passHist.ModifiedTime.ToString("dd/MM/yyyy   HH:mm"));
                            }
                            else
                            {
                                item.SubItems.Add("");
                            }
                            item.SubItems.Add(passHist.Remarks.Trim());

                            lvPassesHist.Items.Add(item);
                        }
                        lvPassesHist.EndUpdate();
                        lvPassesHist.Invalidate();
                    }
                }


            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populatePassView(PassTO pass)
        {
            try
            {
                lvPass.BeginUpdate();
                lvPass.Items.Clear();

                ListViewItem item = new ListViewItem();
                item.Text = pass.PassID.ToString().Trim();
                item.SubItems.Add(pass.EmployeeName.Trim());
                item.SubItems.Add(pass.Direction.Trim());
                if (!pass.EventTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                {
                    item.SubItems.Add(pass.EventTime.ToString("dd/MM/yyyy   HH:mm"));
                }
                else
                {
                    item.SubItems.Add("");
                }
                item.SubItems.Add(pass.PassType.Trim());
                if (pass.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter)
                {
                    item.SubItems.Add(rm.GetString("yes", culture));
                }
                else
                {
                    item.SubItems.Add(rm.GetString("no", culture));
                }
                item.SubItems.Add(pass.LocationName.Trim());
                if (pass.PairGenUsed == (int)Constants.PairGenUsed.Used)
                {
                    item.SubItems.Add(rm.GetString("yes", culture));
                }
                else
                {
                    item.SubItems.Add(rm.GetString("no", culture));
                }
                if (pass.ManualyCreated == (int)Constants.recordCreated.Manualy)
                {
                    item.SubItems.Add(rm.GetString("yes", culture));
                }
                else
                {
                    item.SubItems.Add(rm.GetString("no", culture));
                }

                lvPass.Items.Add(item);

                lvPass.EndUpdate();
                lvPass.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void Passes_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                sortOrder = Constants.sortAsc;
                sortField = PassesHist.EventTimeIndex;
                startIndex = 0;

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
                populateOperatorCombo();


                if (!wuString.Equals(""))
                {
                    /*currentPassesHistList = currentPass.SearchInterval(-1, "", new DateTime(0), new DateTime(0), -1, -1, wuString, dtFrom.Value, dtTo.Value);
                    currentPassesHistList.Sort(new ArrayListSort(sortOrder, sortField));
                    populateListView(currentPassesHistList, startIndex);
                    passesHistListCount = currentPassesHistList.Count;*/
                }
                else
                {
                    MessageBox.Show(rm.GetString("noEmplPassPrivilege", culture));
                }

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                filter = new Filter();
                filter.SerachButton = this.btnSearch;
                filter.TabID = this.Text;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.Passes_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
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
                this.cbOperator.SelectedIndex = 0;
                this.dtpFrom.Value = DateTime.Today;
                this.dtpTo.Value = DateTime.Today;
                this.dtpModifiedFrom.Value = DateTime.Today;
                this.dtpModifiedTo.Value = DateTime.Today;
                this.lblTotal.Visible = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesHist.clearForm(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void clearListView()
        {
            lvPassesHist.BeginUpdate();
            lvPassesHist.Items.Clear();
            lvPassesHist.EndUpdate();

            lvPassesHist.Invalidate();

            btnPrev.Visible = false;
            btnNext.Visible = false;

            passesHistListCount = 0;
        }

        private void clearPassView()
        {
            lvPass.BeginUpdate();
            lvPass.Items.Clear();
            lvPass.EndUpdate();

            lvPass.Invalidate();

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
                log.writeLog(DateTime.Now + " PassesHist.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            clearPassView();
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

                string selectedOperator = OperatorString;
                if (cbOperator.SelectedIndex > 0)
                {
                    selectedOperator = cbOperator.SelectedValue.ToString().Trim();
                }

                PassHist ph = new PassHist();
                ph.PHistTO.EmployeeID = emplID;
                ph.PHistTO.Direction = direction;
                ph.PHistTO.PassTypeID = (int)cbPassType.SelectedValue;
                ph.PHistTO.LocationID = (int)cbLocation.SelectedValue;
                ph.PHistTO.ModifiedBy = selectedOperator;

                int count = ph.SearchIntervalCount(dtpFrom.Value.Date, dtpTo.Value.Date, cbEventTime.Checked, selectedWU, dtpModifiedFrom.Value.Date, dtpModifiedTo.Value.Date, cbModifiedTime.Checked);

                if (count > Constants.maxRecords)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("passesHistGreaterThenAllowed", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        currentPassHistList = ph.SearchInterval(dtpFrom.Value.Date, dtpTo.Value.Date, cbEventTime.Checked, selectedWU, dtpModifiedFrom.Value.Date, dtpModifiedTo.Value.Date, cbModifiedTime.Checked);
                    }
                    else
                    {
                        currentPassHistList.Clear();
                        clearListView();
                    }
                }
                else
                {
                    if (count > 0)
                    {
                        currentPassHistList = ph.SearchInterval(dtpFrom.Value.Date, dtpTo.Value.Date, cbEventTime.Checked, selectedWU, dtpModifiedFrom.Value.Date, dtpModifiedTo.Value.Date, cbModifiedTime.Checked);

                        Pass pass = new Pass();
                        pass.PssTO.EmployeeID = emplID;
                        pass.PssTO.Direction = direction;
                        pass.PssTO.PassTypeID = (int)cbPassType.SelectedValue;
                        pass.PssTO.LocationID = (int)cbLocation.SelectedValue;
                        currentPassList = pass.FindCurrentPasses(dtpFrom.Value.Date, dtpTo.Value.Date, selectedWU, selectedOperator);

                        passesHistListCount = currentPassHistList.Count;
                        startIndex = 0;
                        currentPassHistList.Sort(new ArrayListSort(sortOrder, sortField));
                        populateListView(currentPassHistList, startIndex);
                        this.lblTotal.Visible = true;
                        this.lblTotal.Text = rm.GetString("lblTotal", culture) + currentPassHistList.Count.ToString().Trim();
                    }
                    else
                    {
                        currentPassHistList.Clear();
                        clearListView();
                        this.lblTotal.Visible = true;
                        this.lblTotal.Text = rm.GetString("lblTotal", culture) + "0";
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " .btnSearch_Click(): " + ex.Message + "\n");
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

                currentPassHistList.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                populateListView(currentPassHistList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.lvPasses_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }



        private void cbWU_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                bool check = false;
                foreach (WorkingUnitTO wu in wuArray)
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

                if (cbWU.SelectedValue is int && !check)
                {
                    populateEmployeeCombo((int)cbWU.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }


        private void lvPassesHist_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            bool found = false;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvPassesHist.SelectedItems.Count > 0)
                {
                    foreach (PassTO pass in currentPassList)
                    {
                        if (lvPassesHist.SelectedItems[0].Text.Trim().Equals(pass.PassID.ToString().Trim()))
                        {
                            populatePassView(pass);
                            found = true;
                            break;
                        }
                    }

                    if (found == false)
                    {
                        clearPassView();
                    }
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.lvPassesHIst_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
                    permission = (((int[])menuItemsPermissions[menuItemID])[role.ApplRoleID]);
                    readPermission = readPermission || (((permission / 8) % 2) == 0 ? false : true);

                }

                btnSearch.Enabled = readPermission;

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.setVisibility(): " + ex.Message + "\n");
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
                populateListView(currentPassHistList, startIndex);
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
                populateListView(currentPassHistList, startIndex);
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

        private void cbModifiedTime_Changed(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbModifiedTime.Checked || cbEventTime.Checked)
                {
                    if (cbModifiedTime.Checked)
                    {
                        gbModifiedTime.Enabled = true;
                    }
                    else
                    {
                        gbModifiedTime.Enabled = false;
                    }
                }
                else
                {
                    cbModifiedTime.Checked = true;
                    gbModifiedTime.Enabled = true;
                    MessageBox.Show(rm.GetString("noDateTimePickerSelected", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesHist.cbModifiedTime_Changed(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbEventTime_Changed(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbEventTime.Checked || cbModifiedTime.Checked)
                {
                    if (cbEventTime.Checked)
                    {
                        gbEventTime.Enabled = true;
                    }
                    else
                    {
                        gbEventTime.Enabled = false;
                    }
                }
                else
                {
                    cbEventTime.Checked = true;
                    gbEventTime.Enabled = true;
                    MessageBox.Show(rm.GetString("noDateTimePickerSelected", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesHist.cbEventTime_Changed(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;


                if (currentPassHistList.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("employee_passes_hist");
                    DataTable tableP = new DataTable("employee_passes");
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
                    tableCR.Columns.Add("modified_by", typeof(System.String));
                    tableCR.Columns.Add("modified_time", typeof(System.DateTime));
                    tableCR.Columns.Add("remarks", typeof(System.String));

                    tableP.Columns.Add("event_time", typeof(System.String));
                    tableP.Columns.Add("first_name", typeof(System.String));
                    tableP.Columns.Add("last_name", typeof(System.String));
                    tableP.Columns.Add("pass_id", typeof(int));
                    tableP.Columns.Add("imageID", typeof(byte));
                    tableP.Columns.Add("direction", typeof(System.String));
                    tableP.Columns.Add("pass_type", typeof(System.String));
                    tableP.Columns.Add("is_wrk_hrs", typeof(System.String));
                    tableP.Columns.Add("location", typeof(System.String));
                    tableP.Columns.Add("pair_gen_used", typeof(System.String));
                    tableP.Columns.Add("manual_created", typeof(System.String));

                    tableI.Columns.Add("imageID", typeof(byte));
                    tableI.Columns.Add("image", typeof(System.Byte[]));

                    //add logo image just once
                    DataRow rowI = tableI.NewRow();
                    rowI["image"] = Constants.LogoForReport;
                    rowI["imageID"] = 1;
                    tableI.Rows.Add(rowI);
                    tableI.AcceptChanges();

                    dataSetCR.Tables.Add(tableCR);
                    dataSetCR.Tables.Add(tableP);
                    dataSetCR.Tables.Add(tableI);

                    foreach (PassTO pass in currentPassList)
                    {
                        DataRow row = tableP.NewRow();

                        row["pass_id"] = pass.PassID;
                        row["last_name"] = pass.EmployeeName;
                        row["first_name"] = "";
                        row["direction"] = pass.Direction;
                        row["event_time"] = pass.EventTime.ToString("dd.MM.yyyy HH:mm:ss");
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

                        tableP.Rows.Add(row);
                        tableP.AcceptChanges();
                    }
                    
                    foreach (PassHistTO passHist in currentPassHistList)
                    {
                        if (passHist.Remarks.StartsWith(Constants.PassDeleted))//if pass deleted set values
                        {
                            DataRow rowP = tableP.NewRow();

                            rowP["pass_id"] = passHist.PassID;
                            rowP["last_name"] = Constants.PassDeleted;
                            rowP["first_name"] = "";
                            rowP["direction"] = "";
                            rowP["event_time"] = "";
                            rowP["pass_type"] = "";
                            rowP["is_wrk_hrs"] = "";
                            rowP["location"] = "";
                            rowP["pair_gen_used"] = "";
                            rowP["manual_created"] = "";
                            tableP.Rows.Add(rowP);
                            tableP.AcceptChanges();
                        }
                        DataRow row = tableCR.NewRow();

                        row["pass_id"] = passHist.PassID;
                        row["last_name"] = passHist.EmployeeName;
                        row["first_name"] = "";
                        row["direction"] = passHist.Direction;
                        row["event_time"] = passHist.EventTime;
                        row["pass_type"] = passHist.PassType;
                        if (passHist.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter)
                        {
                            row["is_wrk_hrs"] = rm.GetString("yes", culture);
                        }
                        else
                        {
                            row["is_wrk_hrs"] = rm.GetString("no", culture);
                        }
                        row["location"] = passHist.LocationName;
                        row["modified_by"] = passHist.ModifiedBy;
                        row["modified_time"] = passHist.ModifiedTime;
                        row["remarks"] = passHist.Remarks;

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
                    string selOperator = "*";
                    string from = "";
                    string to = "";
                    string fromModified = "";
                    string toModified = "";


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
                    if (cbOperator.SelectedIndex >= 0)
                        selOperator = cbOperator.Text;
                    if (cbEventTime.Checked)
                    {
                        from = dtpFrom.Value.ToString("dd.MM.yyyy");
                        to = dtpTo.Value.ToString("dd.MM.yyyy");
                    }
                    if (cbModifiedTime.Checked)
                    {
                        fromModified = dtpModifiedFrom.Value.ToString("dd.MM.yyyy");
                        toModified = dtpModifiedTo.Value.ToString("dd.MM.yyyy");
                    }

                    if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                    {
                        Reports.Reports_sr.EmployeePassesHistCRView view = new Reports.Reports_sr.EmployeePassesHistCRView(dataSetCR,
                            from, to, selWorkingUnit, selEmployee, selPassType, selDirection, selLocation,
                            selOperator, fromModified, toModified);
                        view.ShowDialog(this);

                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                    {
                        Reports.Reports_en.EmployeePassesHistCRView_en view = new Reports.Reports_en.EmployeePassesHistCRView_en(dataSetCR,
                            from, to, selWorkingUnit, selEmployee, selPassType, selDirection, selLocation,
                            selOperator, fromModified, toModified);
                        view.ShowDialog(this);
                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                    {
                        Reports.Reports_fi.EmployeePassesHistCRView_fi view = new Reports.Reports_fi.EmployeePassesHistCRView_fi(dataSetCR,
                            from, to, selWorkingUnit, selEmployee, selPassType, selDirection, selLocation,
                            selOperator, fromModified, toModified);
                        view.ShowDialog(this);
                    }
                } //if (currentPassesList.Count > 0)
                else
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesHist.btnReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void PassesHist_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " PassesHist.PassesHist_KeyUp(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " PassesHist.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " PassesHist.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " PassesHist.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " PassesHist.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
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
                UIFeatures.FilterAdd filterAdd = new UIFeatures.FilterAdd(this, filter, new TabPage(this.Text));
                filterAdd.ShowDialog();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesHist.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }



    }
}



