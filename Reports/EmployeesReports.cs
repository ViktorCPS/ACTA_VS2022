using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;

using Common;
using TransferObjects;
using Util;

namespace Reports {
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class EmployeesReports : System.Windows.Forms.Form {
        private System.Windows.Forms.TextBox tbLastName;
        private System.Windows.Forms.CheckBox chBoxEmployeeAll;
        private System.Windows.Forms.ComboBox cbLocation;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.CheckBox chbCSV;
        private System.Windows.Forms.CheckBox chbPdf;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnCancel;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        // List View indexes
        const int FirstNameIndex = 0;
        const int LastNameIndex = 1;
        const int WorkingUnitIDIndex = 2;

        List<EmployeeTO> currentEmployeesList;
        private int sortOrder;
        private int sortField;
        private int startIndex;

        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;

        CultureInfo culture;
        ResourceManager rm;
        ApplUserTO logInUser;

        private System.Windows.Forms.GroupBox gbEmpolyee;
        private System.Windows.Forms.GroupBox gbLocation;
        private System.Windows.Forms.CheckBox cbIncludeSubLoc;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.GroupBox gbTimeInterval;
        private System.Windows.Forms.Label lblDocFormat;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblLastName;

        DebugLog log;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.CheckBox cbTXT;
        private System.Windows.Forms.CheckBox cbCR;
        private CheckBox chbShowRetired;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;

        // Working Units that logInUser is granted to
        List<WorkingUnitTO> wUnits;
        private CheckBox chbOpenPairs;

        Filter filter;


        public EmployeesReports() {
            InitializeComponent();

            this.CenterToScreen();

            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();
            currentEmployeesList = new List<EmployeeTO>();

            wUnits = new List<WorkingUnitTO>();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(EmployeesReports).Assembly);

            DateTime date = DateTime.Now;
            dtpFromDate.Value = new DateTime(date.Year, date.Month, 1);
            dtpToDate.Value = date;

            setLanguage();
        }
        #region MDI child method's
        public void MDIchangeSelectedEmployee(int employeeID, DateTime from, DateTime to) {
            dtpFromDate.Value = from;
            dtpToDate.Value = to;
            if (employeeID > -1) {
                foreach (ListViewItem item in lvEmployees.Items) {
                    if ((int)item.Tag == employeeID) {
                        item.Selected = true;
                        lvEmployees.Select();
                        lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));

                        break;
                    }
                }
            }
        }
        #endregion
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null) {
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
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeesReports));
            this.gbEmpolyee = new System.Windows.Forms.GroupBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.chBoxEmployeeAll = new System.Windows.Forms.CheckBox();
            this.tbLastName = new System.Windows.Forms.TextBox();
            this.gbLocation = new System.Windows.Forms.GroupBox();
            this.cbIncludeSubLoc = new System.Windows.Forms.CheckBox();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.gbTimeInterval = new System.Windows.Forms.GroupBox();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.chbCSV = new System.Windows.Forms.CheckBox();
            this.chbPdf = new System.Windows.Forms.CheckBox();
            this.lblDocFormat = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.cbTXT = new System.Windows.Forms.CheckBox();
            this.cbCR = new System.Windows.Forms.CheckBox();
            this.chbShowRetired = new System.Windows.Forms.CheckBox();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.chbOpenPairs = new System.Windows.Forms.CheckBox();
            this.gbEmpolyee.SuspendLayout();
            this.gbLocation.SuspendLayout();
            this.gbTimeInterval.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbEmpolyee
            // 
            this.gbEmpolyee.Controls.Add(this.lblLastName);
            this.gbEmpolyee.Controls.Add(this.chBoxEmployeeAll);
            this.gbEmpolyee.Controls.Add(this.tbLastName);
            this.gbEmpolyee.Location = new System.Drawing.Point(9, 12);
            this.gbEmpolyee.Name = "gbEmpolyee";
            this.gbEmpolyee.Size = new System.Drawing.Size(274, 93);
            this.gbEmpolyee.TabIndex = 0;
            this.gbEmpolyee.TabStop = false;
            this.gbEmpolyee.Tag = "FILTERABLE";
            this.gbEmpolyee.Text = "Employee";
            // 
            // lblLastName
            // 
            this.lblLastName.Location = new System.Drawing.Point(24, 23);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(72, 23);
            this.lblLastName.TabIndex = 1;
            this.lblLastName.Text = "Last Name:";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chBoxEmployeeAll
            // 
            this.chBoxEmployeeAll.Enabled = false;
            this.chBoxEmployeeAll.Location = new System.Drawing.Point(120, 56);
            this.chBoxEmployeeAll.Name = "chBoxEmployeeAll";
            this.chBoxEmployeeAll.Size = new System.Drawing.Size(56, 24);
            this.chBoxEmployeeAll.TabIndex = 3;
            this.chBoxEmployeeAll.Text = "All";
            this.chBoxEmployeeAll.Visible = false;
            this.chBoxEmployeeAll.CheckedChanged += new System.EventHandler(this.chBoxEmployeeAll_CheckedChanged);
            // 
            // tbLastName
            // 
            this.tbLastName.Location = new System.Drawing.Point(120, 24);
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.Size = new System.Drawing.Size(120, 20);
            this.tbLastName.TabIndex = 2;
            this.tbLastName.TextChanged += new System.EventHandler(this.tbLastName_TextChanged);
            // 
            // gbLocation
            // 
            this.gbLocation.Controls.Add(this.cbIncludeSubLoc);
            this.gbLocation.Controls.Add(this.cbLocation);
            this.gbLocation.Controls.Add(this.lblLocation);
            this.gbLocation.Location = new System.Drawing.Point(9, 380);
            this.gbLocation.Name = "gbLocation";
            this.gbLocation.Size = new System.Drawing.Size(491, 64);
            this.gbLocation.TabIndex = 5;
            this.gbLocation.TabStop = false;
            this.gbLocation.Tag = "FILTERABLE";
            this.gbLocation.Text = "Location";
            // 
            // cbIncludeSubLoc
            // 
            this.cbIncludeSubLoc.Location = new System.Drawing.Point(256, 24);
            this.cbIncludeSubLoc.Name = "cbIncludeSubLoc";
            this.cbIncludeSubLoc.Size = new System.Drawing.Size(192, 24);
            this.cbIncludeSubLoc.TabIndex = 8;
            this.cbIncludeSubLoc.Text = "Include Sublocations";
            // 
            // cbLocation
            // 
            this.cbLocation.Location = new System.Drawing.Point(120, 24);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(121, 21);
            this.cbLocation.TabIndex = 7;
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(16, 24);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(100, 23);
            this.lblLocation.TabIndex = 6;
            this.lblLocation.Text = "Location:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.dtpToDate);
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Controls.Add(this.dtpFromDate);
            this.gbTimeInterval.Location = new System.Drawing.Point(11, 452);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(489, 100);
            this.gbTimeInterval.TabIndex = 9;
            this.gbTimeInterval.TabStop = false;
            this.gbTimeInterval.Tag = "FILTERABLE";
            this.gbTimeInterval.Text = "Time Interval";
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd.MM.yyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(96, 56);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(200, 20);
            this.dtpToDate.TabIndex = 13;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(24, 56);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(80, 23);
            this.lblTo.TabIndex = 12;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(32, 24);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(56, 23);
            this.lblFrom.TabIndex = 10;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd.MM.yyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(96, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(200, 20);
            this.dtpFromDate.TabIndex = 11;
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(9, 145);
            this.lvEmployees.MultiSelect = false;
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.Size = new System.Drawing.Size(492, 216);
            this.lvEmployees.TabIndex = 4;
            this.lvEmployees.Tag = "FILTERABLE";
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployees_ColumnClick);
            // 
            // chbCSV
            // 
            this.chbCSV.Enabled = false;
            this.chbCSV.Location = new System.Drawing.Point(208, 564);
            this.chbCSV.Name = "chbCSV";
            this.chbCSV.Size = new System.Drawing.Size(64, 24);
            this.chbCSV.TabIndex = 16;
            this.chbCSV.Text = "CSV";
            // 
            // chbPdf
            // 
            this.chbPdf.Enabled = false;
            this.chbPdf.Location = new System.Drawing.Point(136, 564);
            this.chbPdf.Name = "chbPdf";
            this.chbPdf.Size = new System.Drawing.Size(64, 24);
            this.chbPdf.TabIndex = 15;
            this.chbPdf.Text = "PDF";
            this.chbPdf.Visible = false;
            // 
            // lblDocFormat
            // 
            this.lblDocFormat.Location = new System.Drawing.Point(16, 564);
            this.lblDocFormat.Name = "lblDocFormat";
            this.lblDocFormat.Size = new System.Drawing.Size(100, 23);
            this.lblDocFormat.TabIndex = 14;
            this.lblDocFormat.Tag = "FILTERABLE";
            this.lblDocFormat.Text = "Document format";
            this.lblDocFormat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(425, 598);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(9, 598);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(128, 23);
            this.btnGenerate.TabIndex = 19;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(468, 116);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 22;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(430, 116);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 21;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // cbTXT
            // 
            this.cbTXT.Location = new System.Drawing.Point(288, 564);
            this.cbTXT.Name = "cbTXT";
            this.cbTXT.Size = new System.Drawing.Size(64, 24);
            this.cbTXT.TabIndex = 17;
            this.cbTXT.Text = "TXT";
            this.cbTXT.CheckedChanged += new System.EventHandler(this.cbTXT_CheckedChanged);
            // 
            // cbCR
            // 
            this.cbCR.Checked = true;
            this.cbCR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCR.Location = new System.Drawing.Point(360, 564);
            this.cbCR.Name = "cbCR";
            this.cbCR.Size = new System.Drawing.Size(104, 24);
            this.cbCR.TabIndex = 18;
            this.cbCR.Text = "CR";
            this.cbCR.CheckedChanged += new System.EventHandler(this.cbCR_CheckedChanged);
            // 
            // chbShowRetired
            // 
            this.chbShowRetired.AutoSize = true;
            this.chbShowRetired.Location = new System.Drawing.Point(9, 116);
            this.chbShowRetired.Name = "chbShowRetired";
            this.chbShowRetired.Size = new System.Drawing.Size(85, 17);
            this.chbShowRetired.TabIndex = 23;
            this.chbShowRetired.Tag = "FILTERABLE";
            this.chbShowRetired.Text = "Show retired";
            this.chbShowRetired.UseVisualStyleBackColor = true;
            this.chbShowRetired.CheckedChanged += new System.EventHandler(this.chbShowRetired_CheckedChanged);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(289, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 93);
            this.gbFilter.TabIndex = 30;
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
            // chbOpenPairs
            // 
            this.chbOpenPairs.AutoSize = true;
            this.chbOpenPairs.Location = new System.Drawing.Point(143, 604);
            this.chbOpenPairs.Name = "chbOpenPairs";
            this.chbOpenPairs.Size = new System.Drawing.Size(92, 17);
            this.chbOpenPairs.TabIndex = 31;
            this.chbOpenPairs.Text = "Open IO Pairs";
            this.chbOpenPairs.UseVisualStyleBackColor = true;
            // 
            // EmployeesReports
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(512, 637);
            this.ControlBox = false;
            this.Controls.Add(this.chbOpenPairs);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.chbShowRetired);
            this.Controls.Add(this.cbCR);
            this.Controls.Add(this.cbTXT);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblDocFormat);
            this.Controls.Add(this.chbPdf);
            this.Controls.Add(this.chbCSV);
            this.Controls.Add(this.lvEmployees);
            this.Controls.Add(this.gbTimeInterval);
            this.Controls.Add(this.gbLocation);
            this.Controls.Add(this.gbEmpolyee);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmployeesReports";
            this.ShowInTaskbar = false;
            this.Text = "Employees Reports";
            this.Load += new System.EventHandler(this.EmployeeReports_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmployeesReports_KeyUp);
            this.gbEmpolyee.ResumeLayout(false);
            this.gbEmpolyee.PerformLayout();
            this.gbLocation.ResumeLayout(false);
            this.gbTimeInterval.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Inner Class for sorting Array List of Employees

        /*
		 *  Class used for sorting Array List of Employees
		*/

        private class ArrayListSort : IComparer<EmployeeTO> {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField) {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(EmployeeTO x, EmployeeTO y) {
                EmployeeTO empl1 = null;
                EmployeeTO empl2 = null;

                if (compOrder == Constants.sortAsc) {
                    empl1 = x;
                    empl2 = y;
                }
                else {
                    empl1 = y;
                    empl2 = x;
                }

                switch (compField) {
                    case EmployeesReports.FirstNameIndex:
                        return empl1.FirstName.CompareTo(empl2.FirstName);
                    case EmployeesReports.LastNameIndex:
                        return empl1.LastName.CompareTo(empl2.LastName);
                    case EmployeesReports.WorkingUnitIDIndex:
                        return empl1.WorkingUnitName.CompareTo(empl2.WorkingUnitName);
                    default:
                        return empl1.LastName.CompareTo(empl2.LastName);
                }
            }
        }

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.Run(new EmployeesReports());
        }

        private void btnCancel_Click(object sender, System.EventArgs e) {
            this.Close();
        }

        private void EmployeeReports_Load(object sender, System.EventArgs e) {
            try {
                this.Cursor = Cursors.WaitCursor;

                lvEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployees.Width - 3) / 3, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployees.Width - 3) / 3, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvEmployees.Width - 3) / 3, HorizontalAlignment.Left);

                populateLocationCb();

                // Initialize comparer object
                sortOrder = Constants.sortAsc;
                sortField = EmployeesReports.LastNameIndex;
                startIndex = 0;

                if (logInUser != null) {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                }

                string wuString = "";
                foreach (WorkingUnitTO wUnit in wUnits) {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0) {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                currentEmployeesList = new Employee().SearchByWU(wuString);
                currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));

                populateEmployeeListView(currentEmployeesList, startIndex);

                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " EmployeesReports.Load(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateLocationCb() {
            try {
                Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
                List<LocationTO> locations = loc.Search();
                locations.Insert(0, new LocationTO(-1, rm.GetString("all", culture), "", -1, -1, Constants.DefaultStateActive.ToString()));

                cbLocation.DataSource = locations;
                cbLocation.DisplayMember = "Name";
                cbLocation.ValueMember = "LocationID";
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " EmployeesReports.populateLocationCb(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateEmployeeListView(List<EmployeeTO> employeeList, int startIndex) {
            try {
                if (employeeList.Count > Constants.recordsPerPage) {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();

                if (employeeList.Count > 0) {
                    if ((startIndex >= 0) && (startIndex < employeeList.Count)) {
                        if (startIndex == 0) {
                            btnPrev.Enabled = false;
                        }
                        else {
                            btnPrev.Enabled = true;
                        }

                        int lastIndex = startIndex + Constants.recordsPerPage;
                        if (lastIndex >= employeeList.Count) {
                            btnNext.Enabled = false;
                            lastIndex = employeeList.Count;
                        }
                        else {
                            btnNext.Enabled = true;
                        }

                        for (int i = startIndex; i < lastIndex; i++) {
                            EmployeeTO employee = employeeList[i];
                            ListViewItem item = new ListViewItem();
                            if (chbShowRetired.Checked || !employee.Status.Equals(Constants.statusRetired)) {
                                item.Text = employee.FirstName.Trim();
                                item.SubItems.Add(employee.LastName.Trim());

                                // Get Working Unit name for the particular user
                                //WorkingUnit wu = new WorkingUnit();
                                //if (wu.Find(employee.WorkingUnitID))
                                //{
                                //    item.SubItems.Add(wu.Name.Trim());
                                //}

                                item.SubItems.Add(employee.WorkingUnitName.Trim());
                                item.Tag = employee.EmployeeID;

                                lvEmployees.Items.Add(item);
                            }
                        }
                    }
                }

                lvEmployees.EndUpdate();
                lvEmployees.Invalidate();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " EmployeesReports.populateEmployeeListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void chBoxEmployeeAll_CheckedChanged(object sender, System.EventArgs e) {
            try {
                this.Cursor = Cursors.WaitCursor;
                if (chBoxEmployeeAll.Checked) {
                    this.tbLastName.Enabled = false;
                    this.lvEmployees.Enabled = false;
                }
                else {
                    this.tbLastName.Enabled = true;
                    this.lvEmployees.Enabled = true;
                }
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " EmployeesReports.chBoxEmployeeAll_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }

        }

        private void lvEmployees_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e) {
            try {
                this.Cursor = Cursors.WaitCursor;
                int prevOrder = sortOrder;

                if (e.Column == sortField) {
                    if (prevOrder == Constants.sortAsc) {
                        sortOrder = Constants.sortDesc;
                    }
                    else {
                        sortOrder = Constants.sortAsc;
                    }
                }
                else {
                    // New Sort Order
                    sortOrder = Constants.sortAsc;
                }

                sortField = e.Column;

                currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                populateEmployeeListView(currentEmployeesList, startIndex);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " EmployeeReports.lvEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void tbLastName_TextChanged(object sender, System.EventArgs e) {
            try {
                int i = 0;
                int p = 0;

                this.Cursor = Cursors.WaitCursor;
                foreach (EmployeeTO employee in currentEmployeesList) {
                    if (employee.LastName.ToUpper().StartsWith(tbLastName.Text.Trim().ToUpper())) {
                        p = i / Constants.recordsPerPage;
                        startIndex = p * Constants.recordsPerPage;
                        populateEmployeeListView(currentEmployeesList, startIndex);
                        break;
                    }
                    i++;

                }


                lvEmployees.SelectedItems.Clear();
                lvEmployees.Select();
                lvEmployees.Invalidate();


                foreach (ListViewItem item in lvEmployees.Items) {
                    if (item.SubItems[1].Text.Trim().ToUpper().StartsWith(tbLastName.Text.Trim().ToUpper())) {
                        item.Selected = true;
                        lvEmployees.Select();
                        lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));

                        break;
                    }

                }


                tbLastName.Focus();

            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " EmployeeReports.tbLastName_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnGenerate_Click(object sender, System.EventArgs e) {
            try {
                log.writeLog(DateTime.Now + " EmployeeReports.btnGenerateReport_Click() \n");
                this.Cursor = Cursors.WaitCursor;
                if (wUnits.Count == 0) {
                    MessageBox.Show(rm.GetString("noWUGranted", culture));
                }
                else {

                    List<int> selectedEmployees = new List<int>();

                    List<IOPairTO> ioPairList = new List<IOPairTO>();
                    //  24.05.2019. BOJAN
                    List<IOPairProcessedTO> ioPairProcessedList = new List<IOPairProcessedTO>();

                    if (chBoxEmployeeAll.Checked == true) {
                        foreach (ListViewItem item in lvEmployees.Items) {
                            selectedEmployees.Add((int)item.Tag);
                        }
                    }
                    else {
                        //  24.05.2019. BOJAN dodat ovaj uslov za slucaj broja rucno prepravljenih parova
                        if (chbOpenPairs.Checked == false) {
                            if (lvEmployees.SelectedItems.Count == 0) {
                                MessageBox.Show(rm.GetString("noEmplSelectedForRep", culture));
                                return;
                            }
                        }
                        else {
                            if (lvEmployees.SelectedItems.Count == 0) {
                                foreach (ListViewItem item in lvEmployees.Items) {
                                    selectedEmployees.Add((int)item.Tag);
                                }
                            }
                        }

                        foreach (ListViewItem item in lvEmployees.SelectedItems) {
                            selectedEmployees.Add((int)item.Tag);
                        }
                    }

                    int selectedLocation = (int)cbLocation.SelectedValue;
                    Location loc = new Location();
                    loc.LocTO.LocationID = selectedLocation;
                    List<LocationTO> locations = loc.Search();

                    if (this.cbIncludeSubLoc.Checked) {
                        if (selectedLocation == -1) {
                            for (int i = locations.Count - 1; i >= 0; i--) {
                                if (locations[i].LocationID != locations[i].ParentLocationID) {
                                    locations.RemoveAt(i);
                                }
                            }
                        }
                        locations = loc.FindAllChildren(locations);
                        selectedLocation = -1;
                    }

                    string locString = "";
                    foreach (LocationTO loc1 in locations) {
                        locString += loc1.LocationID.ToString().Trim() + ",";
                    }

                    if (locString.Length > 0) {
                        locString = locString.Substring(0, locString.Length - 1);
                    }

                    if (chbOpenPairs.Checked == false) {
                        int count = new IOPair().SearchForEmployeesLocCount(dtpFromDate.Value, dtpToDate.Value, selectedEmployees, locString);
                        if (count > Constants.maxEmplReportRecords) {
                            this.Cursor = Cursors.Arrow;
                            MessageBox.Show(rm.GetString("moreRecordsThanAllowed", culture));
                            return;
                        }
                        else if (count > Constants.warningEmplReportRecords) {
                            this.Cursor = Cursors.Arrow;
                            DialogResult result = MessageBox.Show(rm.GetString("recordsForWarning", culture), "", MessageBoxButtons.YesNo);
                            if (result.Equals(DialogResult.No)) {
                                return;
                            }
                        }
                    }

                    this.Cursor = Cursors.WaitCursor;
                    //  24.05.2019. BOJAN dodat uslov
                    if (chbOpenPairs.Checked == false) {
                        ioPairList = new IOPair().SearchForEmployeesLocation(dtpFromDate.Value, dtpToDate.Value, selectedEmployees, locString);
                    }
                    else {
                        //  24.05.2019. BOJAN dodat ceo ELSE
                        string selectedEmployeesString = "";

                        foreach (int emplID in selectedEmployees) {
                            if (emplID != selectedEmployees[selectedEmployees.Count - 1]) {
                                selectedEmployeesString += emplID.ToString() + ", ";
                            }
                            else {
                                selectedEmployeesString += emplID.ToString();
                            }
                        }


                        ioPairProcessedList = new IOPairProcessed().GetIOPairsWithManualCreatedByEmployee(selectedEmployeesString, dtpFromDate.Value, dtpToDate.Value);
                    }

                    // report
                    /*
                    if (this.chbPdf.Checked)
                    {
                        generatePDFReport(ioPairList);
                    }
                    */
                    //  24.05.2019. BOJAN dodat uslov
                    if (chbOpenPairs.Checked == false) {
                        if (this.chbCSV.Checked) {
                            log.writeLog(DateTime.Now + " EmployeesReports csv: STARTED!\n");
                            generateCSVReport(ioPairList);
                            log.writeLog(DateTime.Now + " EmployeesReports csv: FNISHED!\n");
                        }

                        if (this.cbTXT.Checked) {
                            generateTXTReport(ioPairList);
                        }

                        if (this.cbCR.Checked) {
                            generateCRReport(ioPairList);
                        }
                    }
                    else {
                        generateCRReport(ioPairProcessedList);
                    }

                    //this.Close(); 
                }
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " EmployeeReports.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }



        /// <summary>
        /// PDF report setup
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="emplFirstName"></param>
        /// <param name="emplLastName"></param>
        /// <param name="employeeID"></param>
        /*
        private void pdfSetup(PDFDocument doc, string emplFirstName, string emplLastName, string employeeID)
        {
            try
            {
                // Set Document Header
                doc.Title = "Izveštaj o prisustvu zaposlenog po lokacijama za vremenski period";
                doc.LeftBoxText = "Zaposleni: " + emplFirstName + " " + emplLastName + "\n\nOd: " + dtpFromDate.Value.ToString("dd.MM.yyyy") + "\nDo: " +  dtpToDate.Value.ToString("dd.MM.yyyy") + "\n\n\n";
                doc.RightBoxText = DateTime.Now.ToString("dd.MM.yyyy") + "\n\n\n"; 
                doc.FilePath = Constants.pdfDocPath + "\\Izvestaj o zaposlenom " + employeeID.Trim() + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".pdf"; 
                doc.Font = doc.AddFont(Constants.pdfFont);
                doc.FontSize = Constants.pdfFontSize;
            }
            catch(Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeReports.pdfSetup(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        */

        /// <summary>
        /// Generate PDF report
        /// </summary>
        /// <param name="ioPairList"></param>
        /*
        private void generatePDFReport(ArrayList ioPairList)
        {
            try
            {
                PDFDocument doc = new PDFDocument();
                string emplFirstName = "";
                string emplLastName = "";
                string employeeID = "";

                if (lvEmployees.SelectedItems.Count > 0)
                {
                    emplFirstName = lvEmployees.SelectedItems[0].Text;
                    emplLastName = lvEmployees.SelectedItems[0].SubItems[1].Text;
                    employeeID = lvEmployees.SelectedItems[0].Tag.ToString().Trim();
                }

                this.pdfSetup(doc, emplFirstName, emplLastName, employeeID);
                this.InsertTitle(14, doc);
			
                string[] colNames = {"rbr", "Datum", "Lokacija", "Ulazak", "Izlazak", "Trajanje"};
                int[] colWidths = {3, 3, 3, 4, 4, 3};
                double[] colPositions = {0.0, 0.0, 0.0, 0.0, 0.0, 0.0};

                ArrayList tableData = PopulateData(ioPairList);
				
                doc.InsertTable(colNames, colWidths, colPositions, tableData);
                doc.InsertFooter(doc.FontSize);
                doc.Save();
                log.writeLog(DateTime.Now + " EmployeeReports OPEN Document: Started! \n");
                doc.Open();
                log.writeLog(DateTime.Now + " EmployeeReports OPEN Document: Finished! \n");
            }
            catch(Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeReports.generatePDFReport(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        */

        private void generateCSVReport(List<IOPairTO> ioPairList) {
            try {
                ArrayList tableData = new ArrayList();

                int index = 0;

                TimeSpan timeSpan = new TimeSpan();

                foreach (IOPairTO ioPairTO in ioPairList) {
                    index++;
                    timeSpan = ioPairTO.EndTime.Subtract(ioPairTO.StartTime);
                    string[] row = {index.ToString(), ioPairTO.IOPairDate.ToString("dd.MM.yyyy"), ioPairTO.LocationName.ToString(), 
									   ioPairTO.StartTime.ToString("HH:mm:ss"), ioPairTO.EndTime.ToString("HH:mm:ss"), 
									   timeSpan.Hours.ToString() + "h " + timeSpan.Minutes.ToString() + "min"};
                    tableData.Add(row);
                }

                DataTable table = new DataTable();
                table.Columns.Add("rbr", typeof(System.String));
                table.Columns.Add("datum", typeof(System.String));
                table.Columns.Add("lokacija", typeof(System.String));
                table.Columns.Add("ulazak", typeof(System.String));
                table.Columns.Add("izlazak", typeof(System.String));
                table.Columns.Add("trajanje", typeof(System.String));

                for (int i = 0; i < tableData.Count; i++) {
                    string[] al = (string[])tableData[i];
                    DataRow dr = table.NewRow();
                    for (int j = 0; j < al.Length; j++) {
                        dr[j] = al[j];
                    }
                    table.Rows.Add(dr);
                }
                table.AcceptChanges();


                // Specify the column list to export
                int[] iColumns = { 0, 1, 2, 3, 4, 5 };
                string[] cHeaders = { "rbr", "Datum", "Lokacija", "Ulazak", "Izlazak", "Trajanje" };

                // Export the details of specified columns to Excel
                if (table.Rows.Count == 0) {
                    //MessageBox.Show("There are no data to export to CSV!");
                    MessageBox.Show(rm.GetString("noCSVExport", culture));
                }
                else {
                    //19.10.2006, Bilja, da prosledim i imena kolona, bilo bez toga
                    Export objExport = new Export("Win", cHeaders);

                    //RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
                    string employeeID = "";

                    if (lvEmployees.SelectedItems.Count > 0) {
                        employeeID = lvEmployees.SelectedItems[0].Tag.ToString().Trim();
                    }
                    string fileName = Constants.csvDocPath + "\\Izvestaj o zaposlenom " + employeeID.Trim() + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".csv";
                    objExport.ExportDetails(table, Export.ExportFormat.CSV, fileName);
                }
            }
            catch (System.Threading.ThreadAbortException) {
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " EmployeeReports.generateCSVReport(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void generateTXTReport(List<IOPairTO> ioPairList) {
            try {
                ArrayList tableData = new ArrayList();

                int index = 0;

                TimeSpan timeSpan = new TimeSpan();

                foreach (IOPairTO ioPairTO in ioPairList) {
                    index++;
                    timeSpan = ioPairTO.EndTime.Subtract(ioPairTO.StartTime);
                    string[] row = {index.ToString(), ioPairTO.IOPairDate.ToString("dd.MM.yyyy"), ioPairTO.LocationName.ToString(), 
									   ioPairTO.StartTime.ToString("HH:mm:ss"), ioPairTO.EndTime.ToString("HH:mm:ss"), 
									   timeSpan.Hours.ToString() + "h " + timeSpan.Minutes.ToString() + "min"};
                    tableData.Add(row);
                }

                DataTable table = new DataTable();
                table.Columns.Add("rbr", typeof(System.String));
                table.Columns.Add("datum", typeof(System.String));
                table.Columns.Add("lokacija", typeof(System.String));
                table.Columns.Add("ulazak", typeof(System.String));
                table.Columns.Add("izlazak", typeof(System.String));
                table.Columns.Add("trajanje", typeof(System.String));

                for (int i = 0; i < tableData.Count; i++) {
                    string[] al = (string[])tableData[i];
                    DataRow dr = table.NewRow();
                    for (int j = 0; j < al.Length; j++) {
                        dr[j] = al[j];
                    }
                    table.Rows.Add(dr);
                }
                table.AcceptChanges();


                // Specify the column list to export
                int[] iColumns = { 0, 1, 2, 3, 4, 5 };
                string[] cHeaders = { "rbr", "Datum", "Lokacija", "Ulazak", "Izlazak", "Trajanje" };

                // Export the details of specified columns to Excel
                if (table.Rows.Count == 0) {
                    //MessageBox.Show("There are no data to export to CSV!");
                    MessageBox.Show(rm.GetString("noCSVExport", culture));
                }
                else {
                    //19.10.2006, Bilja, da prosledim i imena kolona, bilo bez toga
                    Export objExport = new Export("Win", cHeaders);

                    //RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
                    string employeeID = "";

                    if (lvEmployees.SelectedItems.Count > 0) {
                        employeeID = lvEmployees.SelectedItems[0].Tag.ToString().Trim();
                    }
                    string fileName = Constants.txtDocPath + "\\Izvestaj o zaposlenom " + employeeID.Trim() + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".txt";
                    objExport.ExportDetails(table, Export.ExportFormat.Excel, fileName);
                    System.Diagnostics.Process.Start(fileName);
                }
            }
            catch (System.Threading.ThreadAbortException) {
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " EmployeeReports.generateTXTReport(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private ArrayList PopulateData(List<IOPairTO> ioPairListData) {
            ArrayList result = new ArrayList();
            int i = 0;
            try {
                foreach (IOPairTO ioPairTO in ioPairListData) {
                    i++;
                    TimeSpan totalTime = ioPairTO.EndTime - ioPairTO.StartTime;
                    string[] row = {i.ToString(), ioPairTO.IOPairDate.ToString("dd.MM.yyyy"), ioPairTO.LocationName.ToString(), 
									   ioPairTO.StartTime.ToString("HH:mm:ss"), ioPairTO.EndTime.ToString("HH:mm:ss"), 
									   totalTime.Hours.ToString() + "h " + totalTime.Minutes.ToString() + "min"};
                    result.Add(row);
                }

            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " EmployeeReports.populateDate(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return result;
        }

        private void setLanguage() {
            gbEmpolyee.Text = rm.GetString("employee", culture);
            gbLocation.Text = rm.GetString("location", culture);
            gbFilter.Text = rm.GetString("gbFilter", culture);
            chBoxEmployeeAll.Text = rm.GetString("allEmpl", culture);
            lblLastName.Text = rm.GetString("lblLastName", culture);
            lblLocation.Text = rm.GetString("location", culture);
            cbIncludeSubLoc.Text = rm.GetString("hierarchically", culture);
            lblFrom.Text = rm.GetString("lblFrom", culture);
            gbTimeInterval.Text = rm.GetString("timeInterval", culture);
            lblTo.Text = rm.GetString("lblTo", culture);
            lblDocFormat.Text = rm.GetString("lblDocFormat", culture);
            btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
            btnCancel.Text = rm.GetString("btnCancel", culture);
            chbShowRetired.Text = rm.GetString("chbShowRetired", culture);
            this.Text = rm.GetString("employeeReports", culture);
            chbOpenPairs.Text = rm.GetString("chbOpenPairs", culture);
        }

        // Header
        /// <summary>
        /// Insert pdf document title
        /// </summary>
        /// <param name="titleFontSize"></param>
        /// <param name="doc"></param>
        /*
        public void InsertTitle(int titleFontSize, PDFDocument doc)
        {
            try
            {
                doc.TextStyle.Bold = true;
                doc.HeaderHeight = Constants.HeaderHeight;
                doc.Rect.String = "main";
                double totalWidth = doc.StandardWidth - (doc.LeftMargine + doc.RightMargine);
                doc.Rect.Height = doc.HeaderHeight;
				
                doc.Color.String = "0 0 0";

                doc.PageNumber = 1;
                doc.Rect.Position(doc.LeftMargine, doc.StandardHeight - doc.TopMargine - doc.HeaderHeight);
                doc.Rect.Width = totalWidth / 4;
                doc.FontSize = Constants.pdfFontSize;
                doc.HPos = 0.0;
                doc.VPos = 1.0;
                doc.AddText(doc.LeftBoxText);

                doc.Rect.Position(doc.LeftMargine + totalWidth / 4, doc.StandardHeight - doc.TopMargine - doc.HeaderHeight);
                doc.Rect.Width = 2 * (totalWidth / 4);
                doc.FontSize = titleFontSize;
                doc.HPos = 0.5;
                doc.VPos = 0.0;
                doc.AddText(doc.Title);
				
                doc.Rect.Position(doc.LeftMargine + 3 * (totalWidth / 4), doc.StandardHeight - doc.TopMargine - doc.HeaderHeight);
                doc.FontSize = Constants.pdfFontSize;
                doc.Rect.Width = totalWidth / 4;
                doc.HPos = 1.0;
                doc.VPos = 1.0;
                doc.AddText(doc.RightBoxText);
                doc.TextStyle.Bold = false;
            }
            catch(Exception ex)
            {
                log.writeLog(DateTime.Now + " PDFDocument.InsertHeader(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        */

        private void btnPrev_Click(object sender, System.EventArgs e) {
            try {
                this.Cursor = Cursors.WaitCursor;
                startIndex -= Constants.recordsPerPage;
                if (startIndex < 0) {
                    startIndex = 0;
                }
                populateEmployeeListView(currentEmployeesList, startIndex);

                this.tbLastName.TextChanged -= new System.EventHandler(this.tbLastName_TextChanged);
                this.tbLastName.Clear();
                this.tbLastName.TextChanged += new System.EventHandler(this.tbLastName_TextChanged);
                this.tbLastName.Focus();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " EmployeesReports.btnPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnNext_Click(object sender, System.EventArgs e) {
            try {
                this.Cursor = Cursors.WaitCursor;
                startIndex += Constants.recordsPerPage;
                populateEmployeeListView(currentEmployeesList, startIndex);

                this.tbLastName.TextChanged -= new System.EventHandler(this.tbLastName_TextChanged);
                this.tbLastName.Clear();
                this.tbLastName.TextChanged += new System.EventHandler(this.tbLastName_TextChanged);
                this.tbLastName.Focus();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " EmployeesReports.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void generateCRReport(List<IOPairTO> ioPairList) {
            try {
                if (ioPairList.Count == 0) {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("dataNotFound", culture));
                    return;
                }
                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr)) {
                    Reports_sr.EmployeesCRView view = new Reports_sr.EmployeesCRView(
                        this.PopulateCRData(ioPairList),
                        dtpFromDate.Value, dtpToDate.Value);
                    view.ShowDialog(this);
                }
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_en)) {
                    Reports_en.EmployeesCRView_en view = new Reports_en.EmployeesCRView_en(
                        this.PopulateCRData(ioPairList),
                        dtpFromDate.Value, dtpToDate.Value);
                    view.ShowDialog(this);
                }
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " EmployeesReports.generateCRReport(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        //  24.05.2019. BOJAN
        private void generateCRReport(List<IOPairProcessedTO> ioPairList) {
            try {
                if (ioPairList.Count == 0) {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("dataNotFound", culture));
                    return;
                }
                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr)) {
                    Reports_sr.EmployeesCRView view = new Reports_sr.EmployeesCRView(
                        this.PopulateCRData(ioPairList),
                        dtpFromDate.Value, dtpToDate.Value, true);
                    view.ShowDialog(this);
                }
                //else if (NotificationController.GetLanguage().Equals(Constants.Lang_en)) {
                //    Reports_en.EmployeesCRView_en view = new Reports_en.EmployeesCRView_en(
                //        this.PopulateCRData(ioPairList),
                //        dtpFromDate.Value, dtpToDate.Value);
                //    view.ShowDialog(this);
                //}
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " EmployeesReports.generateCRReport(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate LocReportDS DataSet to create LocationCR
        /// CrystalReports report
        /// </summary>
        /// <param name="dataList">Data list</param>
        /// <returns>Instance of LocReportDS</returns>
        private DataSet PopulateCRData(List<IOPairTO> dataList) {
            DataSet dataSet = new DataSet();
            DataTable table = new DataTable("loc_rep");
            table.Columns.Add("date", typeof(System.String));
            table.Columns.Add("last_name", typeof(System.String));
            table.Columns.Add("first_name", typeof(System.String));
            table.Columns.Add("location", typeof(System.String));
            table.Columns.Add("start", typeof(System.String));
            table.Columns.Add("end", typeof(System.String));
            table.Columns.Add("type", typeof(System.String));
            table.Columns.Add("total_time", typeof(System.String));
            table.Columns.Add("working_unit", typeof(System.String));
            table.Columns.Add("employee_id", typeof(int));
            table.Columns.Add("imageID", typeof(byte));

            DataTable tableI = new DataTable("images");
            tableI.Columns.Add("image", typeof(System.Byte[]));
            tableI.Columns.Add("imageID", typeof(byte));

            try {
                TimeSpan timeSpan = new TimeSpan();

                string minutes;

                int i = 0;
                foreach (IOPairTO pairTO in dataList) {
                    DataRow row = table.NewRow();

                    row["date"] = pairTO.IOPairDate.ToString("dd.MM.yyyy");
                    row["first_name"] = lvEmployees.SelectedItems[0].Text;
                    row["last_name"] = lvEmployees.SelectedItems[0].SubItems[1].Text;
                    row["working_unit"] = lvEmployees.SelectedItems[0].SubItems[2].Text;
                    row["location"] = pairTO.LocationName;
                    row["start"] = pairTO.StartTime.ToString("HH:mm:ss");
                    row["end"] = pairTO.EndTime.ToString("HH:mm:ss");
                    row["type"] = pairTO.PassType;
                    timeSpan = pairTO.EndTime.Subtract(pairTO.StartTime);

                    if (timeSpan.Minutes < 10) {
                        minutes = "0" + timeSpan.Minutes.ToString();
                    }
                    else {
                        minutes = timeSpan.Minutes.ToString();

                    }
                    row["total_time"] = timeSpan.Hours.ToString() + "h " + minutes + "min ";
                    row["employee_id"] = pairTO.EmployeeID;

                    row["imageID"] = 1;
                    if (i == 0) {
                        //add logo image just once
                        DataRow rowI = tableI.NewRow();
                        rowI["image"] = Constants.LogoForReport;
                        rowI["imageID"] = 1;
                        tableI.Rows.Add(rowI);
                        tableI.AcceptChanges();
                    }

                    table.Rows.Add(row);
                    i++;
                }

                table.AcceptChanges();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " LocationsReports.generateCSVReport(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            dataSet.Tables.Add(table);
            dataSet.Tables.Add(tableI);
            return dataSet;
        }
        //  24.05.2019. BOJAN
        private DataSet PopulateCRData(List<IOPairProcessedTO> dataList) {
            DataSet dataSet = new DataSet();
            DataTable table = new DataTable("loc_rep");
            table.Columns.Add("date", typeof(System.String));
            table.Columns.Add("last_name", typeof(System.String));
            table.Columns.Add("first_name", typeof(System.String));
            table.Columns.Add("full_name", typeof(System.String));
            table.Columns.Add("location", typeof(System.String));
            //table.Columns.Add("start", typeof(System.String));
            //table.Columns.Add("end", typeof(System.String));
            //table.Columns.Add("type", typeof(System.String));
            //table.Columns.Add("total_time", typeof(System.String));
            table.Columns.Add("working_unit", typeof(System.String));
            table.Columns.Add("employee_id", typeof(int));
            table.Columns.Add("manual_created", typeof(int));
            table.Columns.Add("imageID", typeof(byte));

            DataTable tableI = new DataTable("images");
            tableI.Columns.Add("image", typeof(System.Byte[]));
            tableI.Columns.Add("imageID", typeof(byte));

            try {
                //TimeSpan timeSpan = new TimeSpan();

                //string minutes;

                int i = 0;
                foreach (IOPairProcessedTO pairTO in dataList) {
                    DataRow row = table.NewRow();

                    row["date"] = pairTO.IOPairDate.ToString("dd.MM.yyyy");
                    //row["first_name"] = lvEmployees.SelectedItems[0].Text;

                    row["employee_id"] = pairTO.EmployeeID;
                    row["first_name"] = pairTO.FirstName;
                    row["last_name"] = pairTO.LastName;

                    if (dataList.Count == 1) {
                        row["full_name"] = pairTO.LastName+" "+pairTO.FirstName;
                    } else 
                    {
                        row["full_name"] = "*";
                    }

                    //row["last_name"] = lvEmployees.SelectedItems[0].SubItems[1].Text;
                    if (dataList.Count == 1) {
                        row["working_unit"] = lvEmployees.SelectedItems[0].SubItems[2].Text;
                    }
                    else {
                        row["working_unit"] = "*";
                    }
                    row["location"] = pairTO.LocationName;
                    row["manual_created"] = Convert.ToInt32(pairTO.ManualCreated);
                    //row["start"] = pairTO.StartTime.ToString("HH:mm:ss");
                    //row["end"] = pairTO.EndTime.ToString("HH:mm:ss");
                    //row["type"] = pairTO.PassType;
                    //timeSpan = pairTO.EndTime.Subtract(pairTO.StartTime);

                    //if (timeSpan.Minutes < 10) {
                    //    minutes = "0" + timeSpan.Minutes.ToString();
                    //}
                    //else {
                    //    minutes = timeSpan.Minutes.ToString();

                    //}
                    //row["total_time"] = timeSpan.Hours.ToString() + "h " + minutes + "min ";


                    row["imageID"] = 1;
                    if (i == 0) {
                        //add logo image just once
                        DataRow rowI = tableI.NewRow();
                        rowI["image"] = Constants.LogoForReport;
                        rowI["imageID"] = 1;
                        tableI.Rows.Add(rowI);
                        tableI.AcceptChanges();
                    }

                    table.Rows.Add(row);
                    i++;
                }

                table.AcceptChanges();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " LocationsReports.generateCSVReport(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            dataSet.Tables.Add(table);
            dataSet.Tables.Add(tableI);
            return dataSet;
        }

        private void cbCR_CheckedChanged(object sender, EventArgs e) {
            if (cbCR.Checked) {
                cbTXT.Checked = false;
            }
            else {
                cbTXT.Checked = true;
            }
        }

        private void cbTXT_CheckedChanged(object sender, EventArgs e) {
            if (cbTXT.Checked) {
                cbCR.Checked = false;
            }
            else {
                cbCR.Checked = true;
            }
        }

        private void chbShowRetired_CheckedChanged(object sender, EventArgs e) {
            this.Cursor = Cursors.WaitCursor;
            try {
                populateEmployeeListView(currentEmployeesList, startIndex);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " EmployeesReports.chbShowRetired_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void EmployeesReports_KeyUp(object sender, KeyEventArgs e) {
            this.Cursor = Cursors.WaitCursor;
            try {
                if (e.KeyCode.Equals(Keys.F1)) {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " EmployeesReports.EmployeesReports_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                this.Cursor = Cursors.WaitCursor;
                if (cbFilter.SelectedIndex == 0) {
                    this.btnSaveCriteria.Text = rm.GetString("SaveFilter", culture);
                }
                else {
                    this.btnSaveCriteria.Text = rm.GetString("UpdateFilter", culture);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSaveCriteria_Click(object sender, EventArgs e) {
            try {
                this.Cursor = Cursors.WaitCursor;
                UIFeatures.FilterAdd filterAdd = new UIFeatures.FilterAdd(this, filter);
                filterAdd.ShowDialog();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
