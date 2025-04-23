using System;
using System.Drawing;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

using Common;
using Util;
using TransferObjects;

namespace Reports
{
    /// <summary>
    /// Manage Employees presance report for one month.
    /// </summary>
    public class PresenceTracking : System.Windows.Forms.Form
    {
        private System.Windows.Forms.ComboBox cbOrganizationalUnits;
        private System.Windows.Forms.Label lblOrganizationalUnit;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label lblFor;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblReportType;
        private System.Windows.Forms.CheckBox cbPDF;
        private System.Windows.Forms.CheckBox cbCR;
        private CheckBox chbHierarhicly;
        private GroupBox gbWorkingUnit;
        private GroupBox gbFor;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        // Language settings
        private CultureInfo culture;
        private ResourceManager rm;

        // all Holidays, Key is Date, value is Holiday
        Dictionary<DateTime, HolidayTO> holidays = new Dictionary<DateTime, HolidayTO>();
        // Debug
        DebugLog debug;
        ApplUserTO logInUser;

        // Template file name
        //const string templateFileNamePage1 = "ListaPrisutnosti-Template-Page10609.pdf";
        //const string templateFileNamePage2 = "ListaPrisutnosti-Template-Page20609.pdf";
        //string newDocFileName = "";

        // Document
        //PDFDocument doc;

        // Month
        DateTime from = new DateTime();
        DateTime to = new DateTime();
        private CheckBox chbShowRetired;

        // Table definiton 
        //int tableX = 45;
        //int tableY = 279;

        //const int cellStartX = 210;
        //const int cellStartY = 298;

        //const int rowDist = 20;
        //const int cellDist = 10;
        // Working Units that logInUser is granted to
        List<WorkingUnitTO> wUnits;
        List<OrganizationalUnitTO> oUnits;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;

        // all time shemas
        List<WorkTimeSchemaTO> timeSchemas = new List<WorkTimeSchemaTO>();

        // key is legend description, and value is counter how many of them has been found same
        Dictionary<string, int> legendDescriptions = new Dictionary<string, int>();
        private RadioButton rbPresence;
        private GroupBox gbOverview;
        private RadioButton rbNumOfHours;
        private RadioButton rbForecast;

        // Nenad 02. XI 2017. Employee selection by working units, organizational units and divisions
        enum EmployeeSelectionTypes { WORKING_UNIT, ORGANIZATIONAL_UNIT, DIVISION };
        EmployeeSelectionTypes employeeSelectedType = EmployeeSelectionTypes.ORGANIZATIONAL_UNIT;
        List<OrganizationalUnitTO> ouArray = new List<OrganizationalUnitTO>();
        List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();
        private ComboBox cbWorkingUnits;
        private Label lblWorkingUnit;

        Filter filter;

        public PresenceTracking()
        {
            InitializeComponent();
            this.CenterToScreen();

            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();
            wUnits = new List<WorkingUnitTO>();

            // Language
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(LocationsReports).Assembly);
            setLanguage();

            // New Document name
            //newDocFileName = "Mesecni izvestaj o prisustvu zaposlenih-";

            // get all time schemas
            timeSchemas = new TimeSchema().Search();
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
            this.cbOrganizationalUnits = new System.Windows.Forms.ComboBox();
            this.lblOrganizationalUnit = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lblFor = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblReportType = new System.Windows.Forms.Label();
            this.cbPDF = new System.Windows.Forms.CheckBox();
            this.cbCR = new System.Windows.Forms.CheckBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.gbWorkingUnit = new System.Windows.Forms.GroupBox();
            this.cbWorkingUnits = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.gbFor = new System.Windows.Forms.GroupBox();
            this.chbShowRetired = new System.Windows.Forms.CheckBox();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbOverview = new System.Windows.Forms.GroupBox();
            this.rbForecast = new System.Windows.Forms.RadioButton();
            this.rbNumOfHours = new System.Windows.Forms.RadioButton();
            this.rbPresence = new System.Windows.Forms.RadioButton();
            this.gbWorkingUnit.SuspendLayout();
            this.gbFor.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.gbOverview.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbOrganizationalUnits
            // 
            this.cbOrganizationalUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOrganizationalUnits.Location = new System.Drawing.Point(80, 24);
            this.cbOrganizationalUnits.Name = "cbOrganizationalUnits";
            this.cbOrganizationalUnits.Size = new System.Drawing.Size(238, 21);
            this.cbOrganizationalUnits.TabIndex = 3;
            this.cbOrganizationalUnits.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // lblOrganizationalUnit
            // 
            this.lblOrganizationalUnit.Location = new System.Drawing.Point(16, 24);
            this.lblOrganizationalUnit.Name = "lblOrganizationalUnit";
            this.lblOrganizationalUnit.Size = new System.Drawing.Size(48, 23);
            this.lblOrganizationalUnit.TabIndex = 2;
            this.lblOrganizationalUnit.Text = "Sector";
            this.lblOrganizationalUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(16, 422);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(128, 23);
            this.btnGenerate.TabIndex = 11;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lblFor
            // 
            this.lblFor.Location = new System.Drawing.Point(16, 24);
            this.lblFor.Name = "lblFor";
            this.lblFor.Size = new System.Drawing.Size(70, 23);
            this.lblFor.TabIndex = 6;
            this.lblFor.Text = "For:";
            this.lblFor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtTo
            // 
            this.dtTo.Checked = false;
            this.dtTo.CustomFormat = "MM.yyyy";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(103, 24);
            this.dtTo.MinDate = new System.DateTime(2001, 1, 1, 0, 0, 0, 0);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(88, 20);
            this.dtTo.TabIndex = 7;
            this.dtTo.Value = new System.DateTime(2006, 9, 1, 12, 21, 9, 62);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(420, 422);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblReportType
            // 
            this.lblReportType.Location = new System.Drawing.Point(13, 377);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(104, 23);
            this.lblReportType.TabIndex = 8;
            this.lblReportType.Tag = "FILTERABLE";
            this.lblReportType.Text = "Document format:";
            this.lblReportType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPDF
            // 
            this.cbPDF.Enabled = false;
            this.cbPDF.Location = new System.Drawing.Point(157, 377);
            this.cbPDF.Name = "cbPDF";
            this.cbPDF.Size = new System.Drawing.Size(56, 24);
            this.cbPDF.TabIndex = 9;
            this.cbPDF.Tag = "FILTERABLE";
            this.cbPDF.Text = "PDF";
            this.cbPDF.Visible = false;
            // 
            // cbCR
            // 
            this.cbCR.Checked = true;
            this.cbCR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCR.Enabled = false;
            this.cbCR.Location = new System.Drawing.Point(253, 377);
            this.cbCR.Name = "cbCR";
            this.cbCR.Size = new System.Drawing.Size(64, 24);
            this.cbCR.TabIndex = 10;
            this.cbCR.Tag = "FILTERABLE";
            this.cbCR.Text = "CR";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(80, 130);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 4;
            this.chbHierarhicly.Text = "Hierarchy ";
            // 
            // gbWorkingUnit
            // 
            this.gbWorkingUnit.Controls.Add(this.cbWorkingUnits);
            this.gbWorkingUnit.Controls.Add(this.lblWorkingUnit);
            this.gbWorkingUnit.Controls.Add(this.chbHierarhicly);
            this.gbWorkingUnit.Controls.Add(this.cbOrganizationalUnits);
            this.gbWorkingUnit.Controls.Add(this.lblOrganizationalUnit);
            this.gbWorkingUnit.Location = new System.Drawing.Point(16, 24);
            this.gbWorkingUnit.Name = "gbWorkingUnit";
            this.gbWorkingUnit.Size = new System.Drawing.Size(330, 245);
            this.gbWorkingUnit.TabIndex = 1;
            this.gbWorkingUnit.TabStop = false;
            this.gbWorkingUnit.Tag = "FILTERABLE";
            this.gbWorkingUnit.Text = "Working Units";
            // 
            // cbWorkingUnits
            // 
            this.cbWorkingUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnits.Location = new System.Drawing.Point(80, 64);
            this.cbWorkingUnits.Name = "cbWorkingUnits";
            this.cbWorkingUnits.Size = new System.Drawing.Size(238, 21);
            this.cbWorkingUnits.TabIndex = 6;
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(16, 64);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(48, 23);
            this.lblWorkingUnit.TabIndex = 5;
            this.lblWorkingUnit.Text = "Cost center";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbFor
            // 
            this.gbFor.Controls.Add(this.dtTo);
            this.gbFor.Controls.Add(this.lblFor);
            this.gbFor.Location = new System.Drawing.Point(16, 275);
            this.gbFor.Name = "gbFor";
            this.gbFor.Size = new System.Drawing.Size(330, 86);
            this.gbFor.TabIndex = 5;
            this.gbFor.TabStop = false;
            this.gbFor.Tag = "FILTERABLE";
            this.gbFor.Text = "For";
            // 
            // chbShowRetired
            // 
            this.chbShowRetired.AutoSize = true;
            this.chbShowRetired.Location = new System.Drawing.Point(323, 381);
            this.chbShowRetired.Name = "chbShowRetired";
            this.chbShowRetired.Size = new System.Drawing.Size(85, 17);
            this.chbShowRetired.TabIndex = 13;
            this.chbShowRetired.Tag = "FILTERABLE";
            this.chbShowRetired.Text = "Show retired";
            this.chbShowRetired.UseVisualStyleBackColor = true;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(358, 24);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 106);
            this.gbFilter.TabIndex = 32;
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
            // gbOverview
            // 
            this.gbOverview.Controls.Add(this.rbForecast);
            this.gbOverview.Controls.Add(this.rbNumOfHours);
            this.gbOverview.Controls.Add(this.rbPresence);
            this.gbOverview.Location = new System.Drawing.Point(358, 275);
            this.gbOverview.Name = "gbOverview";
            this.gbOverview.Size = new System.Drawing.Size(137, 86);
            this.gbOverview.TabIndex = 33;
            this.gbOverview.TabStop = false;
            this.gbOverview.Text = "Overview";
            // 
            // rbForecast
            // 
            this.rbForecast.AutoSize = true;
            this.rbForecast.Checked = true;
            this.rbForecast.Location = new System.Drawing.Point(6, 63);
            this.rbForecast.Name = "rbForecast";
            this.rbForecast.Size = new System.Drawing.Size(66, 17);
            this.rbForecast.TabIndex = 10;
            this.rbForecast.TabStop = true;
            this.rbForecast.Text = "Forecast";
            this.rbForecast.UseVisualStyleBackColor = true;
            this.rbForecast.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // rbNumOfHours
            // 
            this.rbNumOfHours.AutoSize = true;
            this.rbNumOfHours.Checked = true;
            this.rbNumOfHours.Location = new System.Drawing.Point(6, 41);
            this.rbNumOfHours.Name = "rbNumOfHours";
            this.rbNumOfHours.Size = new System.Drawing.Size(103, 17);
            this.rbNumOfHours.TabIndex = 9;
            this.rbNumOfHours.TabStop = true;
            this.rbNumOfHours.Text = "Number of hours";
            this.rbNumOfHours.UseVisualStyleBackColor = true;
            this.rbNumOfHours.CheckedChanged += new System.EventHandler(this.rbNumOfHours_CheckedChanged);
            // 
            // rbPresence
            // 
            this.rbPresence.AutoSize = true;
            this.rbPresence.Location = new System.Drawing.Point(6, 18);
            this.rbPresence.Name = "rbPresence";
            this.rbPresence.Size = new System.Drawing.Size(85, 17);
            this.rbPresence.TabIndex = 8;
            this.rbPresence.TabStop = true;
            this.rbPresence.Text = "+ (Presence)";
            this.rbPresence.UseVisualStyleBackColor = true;
            this.rbPresence.CheckedChanged += new System.EventHandler(this.rbPresence_CheckedChanged);
            // 
            // PresenceTracking
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(509, 453);
            this.ControlBox = false;
            this.Controls.Add(this.gbOverview);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.chbShowRetired);
            this.Controls.Add(this.gbFor);
            this.Controls.Add(this.gbWorkingUnit);
            this.Controls.Add(this.cbCR);
            this.Controls.Add(this.cbPDF);
            this.Controls.Add(this.lblReportType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PresenceTracking";
            this.ShowInTaskbar = false;
            this.Text = "PresenceTracking";
            this.Load += new System.EventHandler(this.PresenceTracking_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PresenceTracking_KeyUp);
            this.gbWorkingUnit.ResumeLayout(false);
            this.gbFor.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.gbOverview.ResumeLayout(false);
            this.gbOverview.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void populateWorkigUnitCombo()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();
                List<OrganizationalUnitTO> ouArray = new List<OrganizationalUnitTO>();
                if (logInUser != null)
                {

                    OrganizationalUnit ouUser = new OrganizationalUnit();
                    ouUser.OrgUnitTO.Status = "ACTIVE";
                    ouArray = ouUser.Search();
                    oUnits = ouUser.Search();
                    if (cbOrganizationalUnits.SelectedValue == null)
                    {
                        wuArray = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                        wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                    }
                    else if(employeeSelectedType == EmployeeSelectionTypes.WORKING_UNIT)
                    {
                        wuArray = new WorkingUnit().getWorkingUnitsByOU(cbOrganizationalUnits.SelectedValue.ToString());
                        wUnits = new WorkingUnit().getWorkingUnitsByOU(cbOrganizationalUnits.SelectedValue.ToString());
                    }
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), Constants.DefaultStateActive.ToString(), -1));
                ouArray.Insert(0, new OrganizationalUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), Constants.DefaultStateActive.ToString()));

                if (employeeSelectedType == EmployeeSelectionTypes.ORGANIZATIONAL_UNIT)
                {
                    cbOrganizationalUnits.BeginUpdate();
                    cbOrganizationalUnits.DataSource = ouArray;// wuArray;
                    cbOrganizationalUnits.DisplayMember = "Name";
                    cbOrganizationalUnits.ValueMember = "OrgUnitID";
                    cbOrganizationalUnits.EndUpdate();
                }
                else if (employeeSelectedType == EmployeeSelectionTypes.WORKING_UNIT)
                {
                    cbWorkingUnits.BeginUpdate();
                    cbWorkingUnits.DataSource = wuArray;// wuArray;
                    cbWorkingUnits.DisplayMember = "Name";
                    cbWorkingUnits.ValueMember = "WorkingUnitID";
                    cbWorkingUnits.EndUpdate();
                }

            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PresenceTracking.populateWorkigUnitCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void setLanguage()
        {
            this.Text = rm.GetString("presenceTracking", culture);

            gbWorkingUnit.Text = rm.GetString("workingUnits", culture);
            gbFor.Text = rm.GetString("gbFor", culture);
            gbFilter.Text = rm.GetString("gbFilter", culture);
            gbOverview.Text = rm.GetString("gbOverview", culture);

            //radio button's text
            rbNumOfHours.Text = rm.GetString("rbNumOfHours", culture);
            rbPresence.Text = rm.GetString("rbPresence", culture);

            chbHierarhicly.Text = rm.GetString("hierarchically", culture);

            this.btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
            this.btnCancel.Text = rm.GetString("btnCancel", culture);

            this.lblOrganizationalUnit.Text = rm.GetString("Sector", culture);
            this.lblWorkingUnit.Text = rm.GetString("CostCenter", culture);
            this.lblFor.Text = rm.GetString("lblFor", culture);
            lblReportType.Text = rm.GetString("lblDocFormat", culture);
            this.chbShowRetired.Text = rm.GetString("chbShowRetired", culture);
        }

        private void PresenceTracking_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.cbOrganizationalUnits.SelectedIndexChanged -= new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged); // Avoid triggering listener
                populateWorkigUnitCombo();
                this.cbOrganizationalUnits.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
                dtTo.Value = DateTime.Now.Date;
                // get all holidays
                List<HolidayTO> holidayList = new Holiday().Search(new DateTime(), new DateTime());

                foreach (HolidayTO holiday in holidayList)
                {
                    holidays.Add(holiday.HolidayDate, holiday);
                }

                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PresenceTracking.PresenceTracking_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }

        }

        #region comment
        /*
        private void PrepareDocument(string wu, string month)
        {
            try
            {
                this.doc = new PDFDocument();
                this.doc.Font = this.doc.AddFont(Constants.pdfFont);
                this.doc.SetLandscape();
                this.doc.Read(Constants.pdfTemplatePath + "\\" + templateFileNamePage1);
                this.doc.FilePath = Constants.pdfDocPath + "\\" + newDocFileName + wu.Trim() + "-" + month.Trim() + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".pdf";
            }
            catch(Exception ex)
            {
                debug.writeLog(DateTime.Now + " PresenceTracking.PrepareDocument(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        */
        #endregion

        private void btnGenerate_Click(object sender, System.EventArgs e)
        {
            // Nenad 09. X 2017. Commented all code for working units and replaced it with organizational units
            // Nenad 02. XI 2017. Choose between working units, org units or divisions
            try
            {
                //debug.writeLog(DateTime.Now + " PresenceTracking.btnGenerateReport_Click() \n");
                this.Cursor = Cursors.WaitCursor;

                if (this.cbOrganizationalUnits.SelectedIndex.Equals(0))
                {
                    MessageBox.Show(rm.GetString("noWUSelected", culture));
                    return;
                }
                legendDescriptions = new Dictionary<string, int>();
                if (oUnits.Count == 0 && wUnits.Count == 0)
                {
                    MessageBox.Show(rm.GetString("noWUGranted", culture));
                }
                else
                {
                    List<EmployeeTO> emplList = null;
                    List<IOPairTO> data = null;
                    if (employeeSelectedType == EmployeeSelectionTypes.ORGANIZATIONAL_UNIT)
                    {

                        int ouID = -1;
                        string oUnitsString = "";

                        ouID = (int)cbOrganizationalUnits.SelectedValue;

                        if (this.chbHierarhicly.Checked)
                        {
                            //WorkingUnit wuElement = new WorkingUnit();
                            //wuElement.WUTO.WorkingUnitID = wuID;
                            //wuArray = wuElement.Search();
                            //wuArray = wuElement.FindAllChildren(wuArray);

                            OrganizationalUnit ouElement = new OrganizationalUnit();
                            ouElement.OrgUnitTO.OrgUnitID = ouID;
                            ouArray = ouElement.Search();
                            ouArray = ouElement.FindAllChildren(ouArray);

                            ouID = -1;
                            //wuID = -1;
                        }

                        //foreach (WorkingUnitTO wuEl in wuArray)
                        //{
                        //    wUnitsString += wuEl.WorkingUnitID.ToString().Trim() + ",";
                        //}
                        //if (wUnitsString.Length > 0)
                        //{
                        //    wUnitsString = wUnitsString.Substring(0, wUnitsString.Length - 1);
                        //}
                        //else
                        //{
                        //    wUnitsString = wuID.ToString().Trim();
                        //}////////////////// 
                        foreach (OrganizationalUnitTO ouEl in ouArray)
                        {
                            oUnitsString += ouEl.OrgUnitID.ToString().Trim() + ",";
                        }
                        if (oUnitsString.Length > 0)
                        {
                            oUnitsString = oUnitsString.Substring(0, oUnitsString.Length - 1);
                        }
                        else
                        {
                            oUnitsString = ouID.ToString().Trim();
                        }

                        // Get IOPairs
                        //ArrayList data = prepareData();
                        //List<IOPairTO> data = prepareData(wuID, wUnitsString); Brisati
                        data = prepareData(ouID, oUnitsString);
                        // Sanja: changed 04.02.2009. - to show all employees from selected organizational units
                        //ArrayList emplList = getEmployees(wuID, wUnitsString);
                        emplList = new Employee().SearchByOU(oUnitsString);
                    }
                    else if (employeeSelectedType == EmployeeSelectionTypes.WORKING_UNIT)
                    {
                        int wuID = -1;
                        wuID = (int)cbWorkingUnits.SelectedValue;
                        if (wuID == -1)
                        {
                            wuArray = new WorkingUnit().getWorkingUnitsByOU(cbOrganizationalUnits.SelectedValue.ToString());
                        }
                        string wUnitsString = "";

                        if (this.chbHierarhicly.Checked)
                        {
                            WorkingUnit wuElement = new WorkingUnit();
                            wuElement.WUTO.WorkingUnitID = wuID;
                            wuArray = wuElement.Search();
                            wuArray = wuElement.FindAllChildren(wuArray);


                            wuID = -1;
                        }

                        foreach (WorkingUnitTO wuEl in wuArray)
                        {
                            wUnitsString += wuEl.WorkingUnitID.ToString().Trim() + ",";
                        }
                        if (wUnitsString.Length > 0)
                        {
                            wUnitsString = wUnitsString.Substring(0, wUnitsString.Length - 1);
                        }
                        else
                        {
                            wUnitsString = wuID.ToString().Trim();
                        }

                        data = prepareData(wuID, wUnitsString); 
                        // Sanja: changed 04.02.2009. - to show all employees from selected working units
                        emplList = getEmployees(wuID, wUnitsString);
                    }

                    List<EmployeeTO> employeesList = new List<EmployeeTO>();
                    foreach (EmployeeTO employee in emplList)
                    {
                        if (this.chbShowRetired.Checked || !employee.Status.Equals(Constants.statusRetired))
                        {
                            employeesList.Add(employee);
                        }
                    }

                    #region comment
                    /*string wu = rm.GetString("all"); 
					if (cbWorkingUnit.SelectedIndex > 0)
					{
						wu = cbWorkingUnit.Text.Trim();
					}

					string month = dtTo.Value.ToString("MM_yyy");

					
					if (this.cbPDF.Checked)
					{
						PrepareDocument(wu, month);
						setHeaders();

						int count = getEmployeesCount();
						if (count > Constants.maxMonthlyReportRecords)
						{
							this.Cursor = Cursors.Arrow;
							MessageBox.Show(rm.GetString("moreRecordsThanAllowed", culture));
							return;
						}
						else if (count > Constants.warningMonthlyReportRecords)
						{
							this.Cursor = Cursors.Arrow;
							DialogResult result = MessageBox.Show(rm.GetString("recordsForWarning", culture), "", MessageBoxButtons.YesNo);
							if (result.Equals(DialogResult.No))
							{
								return;
							}
						}

						this.Cursor = Cursors.WaitCursor;
						//ArrayList employeesList = getEmployees();
						ArrayList ioPairForDay = new ArrayList();

						DateTime currentDay = from;
						IOPair currentIOPair = new IOPair();

						//PDFDocument cont = new PDFDocument();
						//cont.Read(Constants.pdfTemplatePath + "\\" + templateFileNamePage2);

						int k=0;
						int i = 0;
						int RowPos = 0;

						// Goes through the first 10 of all of employees
						foreach(Employee currentEmployee in employeesList)
						{
							// Write Employee Name
							if ((k < 9) && (this.doc.PageCount == 1))
							{
								//writeName(k, (currentEmployee.LastName.Trim() + " " + currentEmployee.FirstName.Trim()), this.doc);
							}
							else if ((k == 9) && (this.doc.PageCount == 1))
							{
								//this.doc.Append(cont);

								this.doc.PageNumber = this.doc.PageCount;
								RowPos = 0;
								tableY =  420;
						
							}
							else if (((this.doc.PageCount > 1) && ((k-9) % 13) == 0))
							{
								this.doc.Append(cont);
								this.doc.PageNumber = this.doc.PageCount;
								RowPos = 0;
								tableY = 420;
						
							}

							writeName(k, RowPos, (currentEmployee.LastName.Trim() + " " + currentEmployee.FirstName.Trim()), this.doc);

							while(currentDay < to)
							{
								for (int j=0; j < data.Count; j++)
								{
							
									currentIOPair = (IOPair) data[j];

									if ((currentIOPair.EmployeeID.Equals(currentEmployee.EmployeeID)) 
										&& (currentIOPair.IOPairDate.Date.Equals(currentDay.Date)))
									{
										ioPairForDay.Add(currentIOPair);
									}
								}
					
								// Write sign

								writeSign(analyzeData(ioPairForDay), i++, RowPos, this.doc);

								ioPairForDay.Clear();
								currentDay = currentDay.AddDays(1);
							}
							i=0;
							k++;
							RowPos++;
							currentDay = from;
						}

						// Save and Show document
						doc.TextStyle.Bold = false;

						this.doc.FontSize = Constants.pdfFontSize11;
						this.doc.InsertFooter(doc.FontSize);
						this.doc.Save();
						debug.writeLog(DateTime.Now + " PresenceTracking OPEN Document: Started! \n");
						doc.Open();
						debug.writeLog(DateTime.Now + " PresenceTracking OPEN Document: Finished! \n");
						this.doc.Clear();
						this.Close();
					}
					*/
                    #endregion

                    if (this.cbCR.Checked)
                    {
                        try
                        {
                            DataSet crData = PrepareCRData(data, employeesList);
                            DataTable table = crData.Tables["employee_presence"];

                            if (table.Rows.Count == 0)
                            {
                                this.Cursor = Cursors.Arrow;
                                MessageBox.Show(rm.GetString("dataNotFound", culture));
                                return;
                            }

                            if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                            {
                                Reports_sr.EmployeePresenceCRView view =
                                    new Reports_sr.EmployeePresenceCRView(crData, this.dtTo.Value);
                                view.ShowDialog(this);
                            }
                            else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                            {
                                Reports_en.EmployeePresenceCRView_en view =
                                    new Reports_en.EmployeePresenceCRView_en(crData, this.dtTo.Value);
                                view.ShowDialog(this);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PresenceTracking.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        #region comment
        /*
		private void setHeaders()
		{
			try
			{
				this.doc.FontSize = Constants.pdfFontSize12;
				doc.HPos = 0.0;
				doc.VPos = 1.0;
				doc.Color.String = "0 0 0";
				DateTimeFormatInfo dateTimeFormat = new CultureInfo(NotificationController.GetLanguage(), true).DateTimeFormat;
				string[] month = dateTimeFormat.MonthNames;

				// Add Month
				doc.Rect.Position(110, 473);
				doc.Rect.Width = 100;
				doc.Rect.Height = 25;
				//doc.FrameRect();
				doc.AddText(month[dtTo.Value.Month - 1]);

				// Add Year
				doc.Rect.Position(110, 445);
				doc.Rect.Width = 100;
				doc.Rect.Height = 25;
				//doc.FrameRect();
				doc.AddText(DateTime.Now.Year.ToString());

				// Add Working Unit Name
				//doc.Color.String = "255 0 0";
				doc.FontSize = Constants.pdfFontSize14;
				doc.Rect.Position(215, 470);
				doc.Rect.Width = 220;
				doc.Rect.Height = 25;
				//doc.FrameRect();

				if (cbWorkingUnit.SelectedIndex != 0)
				{
					doc.AddText(cbWorkingUnit.Text.Trim());
				}
				else
				{
					doc.AddText("");
				}

				// Add Organization Name
				doc.Rect.Position(290, 445);
				doc.Rect.Width = 150;
				doc.Rect.Height = 25;
				//doc.FrameRect();
				doc.AddText((ConfigurationManager.AppSettings["OrgName"]));
				//doc.Color.String = "0 0 0";
				doc.FontSize = Constants.pdfFontSize12;
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " PresenceTracking.setHeaders(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}
		*/
        #endregion

        public List<IOPairTO> prepareData(int wuID, string wUnitsString)
        {
            List<IOPairTO> ioPairsList = new List<IOPairTO>();
            try
            {
                IOPair ioPair = new IOPair();
                DateTime selectd = dtTo.Value;

                this.from = new DateTime(selectd.Year, selectd.Month, 1, 0, 0, 0);
                this.to = this.from.AddMonths(1);
                //this.to = new DateTime(selectd.Year, selectd.Month + 1, 1, 0, 0 ,0);
                if (employeeSelectedType == EmployeeSelectionTypes.ORGANIZATIONAL_UNIT)
                    ioPairsList = ioPair.SearchForOUPresence(wuID, wUnitsString, from, to);
                else if (employeeSelectedType == EmployeeSelectionTypes.WORKING_UNIT)
                    ioPairsList = ioPair.SearchForPresence(wuID, wUnitsString, from, to);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PresenceTracking.prepareData(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return ioPairsList;
        }

        public List<EmployeeTO> getEmployees(int wuID, string wUnitsString)
        {
            List<EmployeeTO> employeesList = new List<EmployeeTO>();

            try
            {
                IOPair ioPair = new IOPair();
                DateTime selectd = dtTo.Value;
                //int workingUnitID = (int) cbWorkingUnit.SelectedValue;

                this.from = new DateTime(selectd.Year, selectd.Month, 1, 0, 0, 0);
                this.to = this.from.AddMonths(1);

                /*string wuString = "";
                foreach (WorkingUnit wu in wUnits)
                {
                    wuString += wu.WorkingUnitID.ToString().Trim() + ","; 
                }
				
                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }*/
                if (employeeSelectedType == EmployeeSelectionTypes.ORGANIZATIONAL_UNIT)
                    employeesList = ioPair.SearchDistincOUEmployees(from, to, wuID, wUnitsString);
                else if (employeeSelectedType == EmployeeSelectionTypes.WORKING_UNIT)
                    employeesList = ioPair.SearchDistincEmployees(from, to, wuID, wUnitsString);

            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PresenceTracking.getEmployees(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return employeesList;
        }
        bool isHoliday(DateTime day)
        {
            return (holidays.ContainsKey(day));
        }
        #region comment
        /*public int getEmployeesCount()
		{
			int count = 0;

			try
			{
				IOPair ioPair = new IOPair();
				DateTime selectd = dtTo.Value;
				int workingUnitID = (int) cbWorkingUnit.SelectedValue;

				this.from = new DateTime(selectd.Year, selectd.Month, 1, 0, 0 ,0);
				this.to = this.from.AddMonths(1);

				string wuString = "";
				foreach (WorkingUnit wu in wUnits)
				{
					wuString += wu.WorkingUnitID.ToString().Trim() + ","; 
				}
				
				if (wuString.Length > 0)
				{
					wuString = wuString.Substring(0, wuString.Length - 1);
				}

				count = ioPair.SearchDistincEmployeesCount(from, to, workingUnitID, wuString);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " PresenceTracking.getEmployees(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return count;
		}*/

        /*private string analyzeData(ArrayList findedIOPairs)
        {
            string sign = "";

            try
            {
                if (findedIOPairs.Count > 0)
                {
                    switch(((IOPair) findedIOPairs[0]).PassTypeID)
                    {
                        case 0: 
                            return "+";
                        case 1:
                            return "so";
                        case 5:
                            return "go";
                        case 6:
                            return "bo";
                        default:
                            return "-";
                    }
                }
                else
                {
                    return "-";
                }

            }
            catch(Exception ex)
            {
                debug.writeLog(DateTime.Now + " PresenceTracking.analyzeData(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return sign;
        }*/

        /*
        private void writeSign(string sign, int cellNum, int employeeNum, PDFDocument document)
        {
            try
            {
                document.FontSize = Constants.pdfFontSize10;
                // Write Employee Name
				
                document.TextStyle.Bold = true;
                document.HPos = 0.0;
                document.VPos = 0.5;

                document.Rect.Position(tableX + 202 + (cellNum * 18), tableY - (28 * employeeNum));
				
                document.Rect.Width = 17 - (cellNum % 10) + 1;
                document.Rect.Height = 28;
                //doc.FrameRect();
                document.AddText(sign);
                document.TextStyle.Bold = false;

            }
            catch(Exception ex)
            {
                debug.writeLog(DateTime.Now + " PresenceTracking.writeSign(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
		
        */
        /*
        private void writeName(int index, int i, string lastFirstName, PDFDocument document)
        {
            try
            {
                document.FontSize = Constants.pdfFontSize11;
				

                // Write num.
                document.HPos = 0.5;
                document.VPos = 0.5;
                document.Rect.Position(tableX, tableY - (28 * i));
                document.Rect.Width = 45;
                document.Rect.Height = 28;
                document.TextStyle.Bold = false;
                //doc.FrameRect();
                document.AddText((index + 1).ToString()+ ".");

                // Write Employee Name
                doc.FontSize = Constants.pdfFontSize11;
                document.TextStyle.Bold = true;
                doc.HPos = 0.0;
                doc.VPos = 0.5;
                doc.Rect.Position(tableX + 50, tableY - (28 * i));
                doc.Rect.Width = 120;
                doc.Rect.Height = 28;
                //doc.FrameRect();
                doc.AddText(lastFirstName);
                document.TextStyle.Bold = false;
            }
            catch(Exception ex)
            {
                debug.writeLog(DateTime.Now + " PresenceTracking.writeName(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }
        */
        #endregion

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private DataSet PrepareCRData(List<IOPairTO> ioPairs, List<EmployeeTO> employeeList)
        {
            Hashtable pTypes = new Hashtable(); // key is pass_type_id, value is short description from legend
            DataSet dataSet = new DataSet();
            DataTable table = dataSet.Tables.Add("employee_presence");
            Dictionary<int, string> passTypesDesc = new Dictionary<int, string>();

            table.Columns.Add("employee_id", typeof(System.String));
            table.Columns.Add("first_name", typeof(System.String));
            table.Columns.Add("last_name", typeof(System.String));
            table.Columns.Add("working_unit", typeof(System.String));
            table.Columns.Add("date", typeof(DateTime));

            table.Columns.Add("day_1", typeof(System.String));
            table.Columns.Add("day_2", typeof(System.String));
            table.Columns.Add("day_3", typeof(System.String));
            table.Columns.Add("day_4", typeof(System.String));
            table.Columns.Add("day_5", typeof(System.String));
            table.Columns.Add("day_6", typeof(System.String));
            table.Columns.Add("day_7", typeof(System.String));
            table.Columns.Add("day_8", typeof(System.String));
            table.Columns.Add("day_9", typeof(System.String));
            table.Columns.Add("day_10", typeof(System.String));
            table.Columns.Add("day_11", typeof(System.String));
            table.Columns.Add("day_12", typeof(System.String));
            table.Columns.Add("day_13", typeof(System.String));
            table.Columns.Add("day_14", typeof(System.String));
            table.Columns.Add("day_15", typeof(System.String));
            table.Columns.Add("day_16", typeof(System.String));
            table.Columns.Add("day_17", typeof(System.String));
            table.Columns.Add("day_18", typeof(System.String));
            table.Columns.Add("day_19", typeof(System.String));
            table.Columns.Add("day_20", typeof(System.String));
            table.Columns.Add("day_21", typeof(System.String));
            table.Columns.Add("day_22", typeof(System.String));
            table.Columns.Add("day_23", typeof(System.String));
            table.Columns.Add("day_24", typeof(System.String));
            table.Columns.Add("day_25", typeof(System.String));
            table.Columns.Add("day_26", typeof(System.String));
            table.Columns.Add("day_27", typeof(System.String));
            table.Columns.Add("day_28", typeof(System.String));
            table.Columns.Add("day_29", typeof(System.String));
            table.Columns.Add("day_30", typeof(System.String));
            table.Columns.Add("day_31", typeof(System.String));
            table.Columns.Add("total", typeof(System.Int32));
            table.Columns.Add("numOfDays", typeof(System.String));
            table.Columns.Add("totalAbsence", typeof(System.Int32));
            table.Columns.Add("totalNS", typeof(System.Int32));
            table.Columns.Add("imageID", typeof(byte));

            table.Columns.Add("RDS", typeof(System.String)); // Nenad 31. X 2017. 31 collumn for pass types, user request. Explanations are on EmployeePresenceCR_sr.rpt
            table.Columns.Add("RDP", typeof(System.String));
            table.Columns.Add("SPD", typeof(System.String));
            table.Columns.Add("SPN", typeof(System.String));
            table.Columns.Add("NRS", typeof(System.String));
            table.Columns.Add("NRP", typeof(System.String));
            table.Columns.Add("GOS", typeof(System.String));
            table.Columns.Add("GOP", typeof(System.String));
            table.Columns.Add("DPR", typeof(System.String));
            table.Columns.Add("VPR", typeof(System.String));
            table.Columns.Add("PLO", typeof(System.String));
            table.Columns.Add("NOS", typeof(System.String));
            table.Columns.Add("NOP", typeof(System.String));
            table.Columns.Add("BVA", typeof(System.String));
            table.Columns.Add("BPT", typeof(System.String));
            table.Columns.Add("B30V", typeof(System.String));
            table.Columns.Add("B30P", typeof(System.String));
            table.Columns.Add("B30T", typeof(System.String));
            table.Columns.Add("BSP", typeof(System.String));
            table.Columns.Add("BPL", typeof(System.String));
            table.Columns.Add("OPD", typeof(System.String));
            table.Columns.Add("PrS", typeof(System.String));
            table.Columns.Add("PrP", typeof(System.String));
            table.Columns.Add("PrSP", typeof(System.String));
            table.Columns.Add("PrNS", typeof(System.String));
            table.Columns.Add("PrN", typeof(System.String));
            table.Columns.Add("PDP", typeof(System.String));
            table.Columns.Add("PPPP", typeof(System.String));
            table.Columns.Add("NrVr", typeof(System.String));
            table.Columns.Add("PrDp", typeof(System.String));
            table.Columns.Add("NrDp", typeof(System.String));
            table.Columns.Add("ZZS", typeof(System.String));
            table.Columns.Add("IZS", typeof(System.String));
            table.Columns.Add("SUS", typeof(System.String));
            table.Columns.Add("UDR", typeof(System.String));
            DataTable tableI = dataSet.Tables.Add("images");
            tableI.Columns.Add("image", typeof(System.Byte[]));
            tableI.Columns.Add("imageID", typeof(byte));

            DataTable tablePT = dataSet.Tables.Add("pass_types");
            tablePT.Columns.Add("pt_0", typeof(System.String));
            tablePT.Columns.Add("pt_1", typeof(System.String));
            tablePT.Columns.Add("pt_2", typeof(System.String));
            tablePT.Columns.Add("pt_3", typeof(System.String));
            tablePT.Columns.Add("pt_4", typeof(System.String));
            tablePT.Columns.Add("pt_5", typeof(System.String));
            tablePT.Columns.Add("pt_6", typeof(System.String));
            tablePT.Columns.Add("pt_7", typeof(System.String));
            tablePT.Columns.Add("pt_8", typeof(System.String));
            tablePT.Columns.Add("pt_9", typeof(System.String));
            tablePT.Columns.Add("pt_10", typeof(System.String));
            tablePT.Columns.Add("pt_11", typeof(System.String));
            tablePT.Columns.Add("pt_12", typeof(System.String));
            tablePT.Columns.Add("pt_13", typeof(System.String));
            tablePT.Columns.Add("pt_14", typeof(System.String));
            tablePT.Columns.Add("pt_15", typeof(System.String));
            tablePT.Columns.Add("pt_16", typeof(System.String));
            tablePT.Columns.Add("pt_17", typeof(System.String));
            tablePT.Columns.Add("pt_18", typeof(System.String));
            tablePT.Columns.Add("pt_19", typeof(System.String));
            tablePT.Columns.Add("pt_20", typeof(System.String));
            tablePT.Columns.Add("pt_21", typeof(System.String));
            tablePT.Columns.Add("pt_22", typeof(System.String));
            tablePT.Columns.Add("pt_23", typeof(System.String));
            tablePT.Columns.Add("pt_24", typeof(System.String));
            tablePT.Columns.Add("pt_0_desc", typeof(System.String));
            tablePT.Columns.Add("pt_1_desc", typeof(System.String));
            tablePT.Columns.Add("pt_2_desc", typeof(System.String));
            tablePT.Columns.Add("pt_3_desc", typeof(System.String));
            tablePT.Columns.Add("pt_4_desc", typeof(System.String));
            tablePT.Columns.Add("pt_5_desc", typeof(System.String));
            tablePT.Columns.Add("pt_6_desc", typeof(System.String));
            tablePT.Columns.Add("pt_7_desc", typeof(System.String));
            tablePT.Columns.Add("pt_8_desc", typeof(System.String));
            tablePT.Columns.Add("pt_9_desc", typeof(System.String));
            tablePT.Columns.Add("pt_10_desc", typeof(System.String));
            tablePT.Columns.Add("pt_11_desc", typeof(System.String));
            tablePT.Columns.Add("pt_12_desc", typeof(System.String));
            tablePT.Columns.Add("pt_13_desc", typeof(System.String));
            tablePT.Columns.Add("pt_14_desc", typeof(System.String));
            tablePT.Columns.Add("pt_15_desc", typeof(System.String));
            tablePT.Columns.Add("pt_16_desc", typeof(System.String));
            tablePT.Columns.Add("pt_17_desc", typeof(System.String));
            tablePT.Columns.Add("pt_18_desc", typeof(System.String));
            tablePT.Columns.Add("pt_19_desc", typeof(System.String));
            tablePT.Columns.Add("pt_20_desc", typeof(System.String));
            tablePT.Columns.Add("pt_21_desc", typeof(System.String));
            tablePT.Columns.Add("pt_22_desc", typeof(System.String));
            tablePT.Columns.Add("pt_23_desc", typeof(System.String));
            tablePT.Columns.Add("pt_24_desc", typeof(System.String));

            // populate pass_types
            List<PassTypeTO> passTypesAll = new PassType().Search();
            List<PassTypeTO> passTypes = new List<PassTypeTO>();

            foreach (PassTypeTO pt in passTypesAll)
            {
                if (pt.PassTypeID == Constants.regularWork || pt.IsPass != Constants.passOnReader || pt.PassTypeID == Constants.nightShiftWork)
                {
                    passTypes.Add(pt);
                }
            }
            int index = 0;
            int ptLegend = 0;
            DataRow rowPT = tablePT.NewRow();
            bool addRow = false;
            while (ptLegend < Constants.maxPassTypes && index < passTypes.Count)
            {
                if (passTypes[index].PassTypeID >= 0 || passTypes[index].PassTypeID == Constants.extraHours)
                {
                    string legendDesc = "";
                    if (passTypes[index].PassTypeID > 0 || passTypes[index].PassTypeID == Constants.extraHours)
                    {
                        legendDesc = makeLegend(passTypes[index].Description);
                    }
                    else
                    {
                        if (rbPresence.Checked)
                            legendDesc = Constants.passTypeR;
                        else
                            legendDesc = Constants.passTypeR1;
                    }

                    if (!legendDesc.Trim().Equals(""))
                    {
                        rowPT["pt_" + ptLegend.ToString().Trim()] = legendDesc;
                        rowPT["pt_" + ptLegend.ToString().Trim() + "_desc"] = " - " + passTypes[index].Description;
                        addRow = true;

                        pTypes.Add(passTypes[index].PassTypeID, rowPT["pt_" + ptLegend.ToString().Trim()]);
                    }

                    ptLegend++;
                }

                index++;
            }

            if (addRow)
            {
                tablePT.Rows.Add(rowPT);
            }
            // Add all pass types for report
            for (int i = 0; i < passTypes.Count; i++)
            {
                passTypesDesc.Add(passTypes[i].PassTypeID, makeLegend(passTypes[i].Description));

            }
            Dictionary<int, List<IOPairTO>> emplPairs = new Dictionary<int, List<IOPairTO>>(); // Key: Day, Value:IOPair List
            List<IOPairTO> pairDay = new List<IOPairTO>();
            IOPairTO pairTemp = new IOPairTO();
            int forecastField = 0;
            try
            {
                int counter = 0;
                foreach (EmployeeTO employee in employeeList)
                {
                    forecastField = 0;
                    List<EmployeeTimeScheduleTO> emplDaySchedules = Common.Misc.GetEmployeeTimeSchedules(employee.EmployeeID, new DateTime(dtTo.Value.Year, dtTo.Value.Month, 1), new DateTime(dtTo.Value.Year, dtTo.Value.Month, 1).AddMonths(1).AddDays(-1));
                    int lastDay = new DateTime(dtTo.Value.Year, dtTo.Value.Month, 1).AddMonths(1).AddDays(-1).Day;
                    List<IOPairTO> pairs = new List<IOPairTO>();
                    DataRow row = table.NewRow();
                    //cnt++;
                    row["employee_id"] = employee.EmployeeID;
                    row["last_name"] = employee.FirstName[0] + "." + employee.LastName;
                    row["first_name"] = employee.FirstName;
                    row["working_unit"] = employee.WorkingUnitName;
                    row["date"] = dtTo.Value.Date;

                    // Init Values
                    for (int i = 1; i < 32; i++)
                    {
                        row["day_" + i.ToString()] = "";
                    }
                    foreach (IOPairTO pair in ioPairs)
                    {
                        if (pair.EmployeeID.Equals(employee.EmployeeID))
                        {
                            if (emplPairs.ContainsKey(pair.IOPairDate.Day))
                            {
                                List<IOPairTO> existing = emplPairs[pair.IOPairDate.Day];
                                existing.Add(pair);
                            }
                            else
                            {
                                List<IOPairTO> newDay = new List<IOPairTO>();
                                newDay.Add(pair);
                                emplPairs.Add(pair.IOPairDate.Day, newDay);
                            }
                            pairs.Add(pair);
                        }
                    }
                    int totalMinutes = 0;
                    int totalMinutesAbsence = 0;
                    int totalNightShift = 0;
                    int numOfDays = 0;

                    // Nenad 31. X 2017. 31 collumns for pass types, user request. Explanations are on EmployeePresenceCR_sr.rpt
                    int RDS = 0; // 1 
                    int RDP = 0; // 
                    int SPD = 0; // 2
                    int SPN = 0; //
                    int NRS = 0; // 6
                    int NRP = 0; // 
                    int GOS = 0; // 8
                    int GOP = 0; // 
                    int DPR = 0; // 10
                    int VPR = 0; // 12
                    int PLO = 0; // 13, 22, 58
                    int NOS = 0; // 14, 41, 80
                    int NOP = 0; //
                    int BVA = 0; // 25, 39
                    int BPT = 0; // 26
                    int B30V = 0; // 27
                    int B30P = 0; // 40
                    int B30T = 0; // 43
                    int BSP = 0; //
                    int BPL = 0; //
                    int OPD = 0; // 28
                    int PrS = 0; // 4
                    int PrP = 0; //
                    int PrSP = 0; //
                    int PrNS = 0; // 
                    int PrN = 0; // 
                    int PDP = 0; //5
                    int PPPP = 0; // 
                    int NrVr = 0; //
                    int PrDp = 0; //
                    int NrDp = 0; //
					int SUS = 0; // 23, 38
					int ZZS = 0; // 48
					int IZS = 0; // 42, 87
                    int UDR = 0; // 53


                    bool shouldWork = false;
                    for (int date = 1; date <= lastDay; date++)
                    {
                        forecastField = 0;
                        //pairDay = (ArrayList)emplPairs[date];
                        int PassAbsence = 0;
                        shouldWork = false;
                        DateTime day = new DateTime(dtTo.Value.Year, dtTo.Value.Month, date);

                        //28.10.2009 Natasa Calculate hours insted of marking days with regular work
                        bool is2DayShift = false;
                        bool is2DaysShiftPrevious = false;
                        WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                        //get intervls for employee and day
                        Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplDaySchedules, day, ref is2DayShift, ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);
                        Employee empl = new Employee();
                        empl.EmplTO = employee;
                        ArrayList schemas = empl.findTimeSchema(day);
                        if (schemas.Count > 0)
                        {
                            WorkTimeSchemaTO timeSchema = (WorkTimeSchemaTO)schemas[0];

                            //if employee absences day is working day and it is not holiday count it as used and one day less in LeftDays
                            if (edi != null)
                            {
                                TimeSpan nightShiftsRemaining = new TimeSpan();
                                List<WorkTimeIntervalTO> intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DaysShiftPrevious, firstIntervalNextDay, edi);
                                Dictionary<int, WorkTimeIntervalTO> dayIntervals = new Dictionary<int, WorkTimeIntervalTO>();
                                TimeSpan ts = new TimeSpan();
                                for (int i = 0; i < intervals.Count; i++)
                                {
                                    WorkTimeIntervalTO interval = intervals[i];
                                    dayIntervals.Add(i, interval);
                                    shouldWork = true;
                                    ts += interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay;

                                    // Nenad 17. VIII 2017. calculate work hours until end of the month
                                    if (rbForecast.Checked && date >= (int)System.DateTime.Now.Day && date <= System.DateTime.DaysInMonth(System.DateTime.Today.Year, System.DateTime.Today.Month))
                                    {
                                        TimeSpan workingTime = interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay;
                                        if (workingTime.TotalMinutes != 0)
                                        {
                                            //////////////////////////////////////////////////// Nenad, forecast addition to regular report
                                            IOPairProcessed iop = new IOPairProcessed();
                                            iop.IOPairProcessedTO.EmployeeID = empl.EmplTO.EmployeeID;
                                            iop.IOPairProcessedTO.IOPairDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, date);
                                            iop.IOPairProcessedTO.PassTypeID = 1;
                                            if (empl.EmplTO.EmployeeID == 247)
                                                debug.writeLog("");
                                            List<IOPairProcessedTO> iopp = iop.Search();
                                            // If no regular work pairs that day, count as forecast
                                            if (iopp.Count == 0 || (iopp.Count == 1 && is2DaysShiftPrevious))
                                            {
                                                TimeSpan EndHelp = new TimeSpan(07, 30, 0);
                                                TimeSpan startOnly = new TimeSpan(22, 0, 0);
                                                TimeSpan endOnly = new TimeSpan(23, 59, 59);
                                                TimeSpan startOnly1 = new TimeSpan(0, 0, 0);
                                                TimeSpan endOnly1 = new TimeSpan(6, 0, 0);

                                                if (interval.EndTime.TimeOfDay > startOnly)
                                                {

                                                    TimeSpan now = new TimeSpan(22, 0, 0);
                                                    if (interval.StartTime.TimeOfDay >= now)
                                                    {
                                                        nightShiftsRemaining += (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                                                        //numOfDays++;
                                                    }
                                                    else
                                                    {
                                                        int currMonth = DateTime.Today.Month;
                                                        if (date == 31)
                                                        {
                                                            nightShiftsRemaining += (interval.EndTime.TimeOfDay - now + endOnly1);
                                                            //numOfDays++;
                                                        }
                                                        else if (currMonth == 2 && date == 28)
                                                        {
                                                            nightShiftsRemaining += (interval.EndTime.TimeOfDay - now + endOnly1);
                                                            //numOfDays++;
                                                        }
                                                        else if ((currMonth == 4 && date == 30) ||
                                                                 (currMonth == 6 && date == 30) ||
                                                                 (currMonth == 9 && date == 30) ||
                                                                 (currMonth == 11 && date == 30))
                                                        {
                                                            nightShiftsRemaining += (interval.EndTime.TimeOfDay - now + endOnly1);
                                                            //numOfDays++;
                                                        }
                                                        else
                                                        {
                                                            nightShiftsRemaining += (interval.EndTime.TimeOfDay - now);
                                                            //numOfDays++;
                                                        }
                                                    }

                                                }

                                                //else if (interval.EndTime.TimeOfDay <= endOnly1) 
                                                //{

                                                //    tsNightShift += (interval.EndTime.TimeOfDay - (interval.EndTime.TimeOfDay - endOnly1));
                                                //}

                                                if (interval.StartTime.TimeOfDay <= endOnly1)
                                                {


                                                    TimeSpan now = new TimeSpan(0, 0, 0);
                                                    TimeSpan after = new TimeSpan(6, 0, 0);
                                                    if (interval.EndTime.TimeOfDay <= after)
                                                    {
                                                        nightShiftsRemaining += (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                                                    }
                                                    else
                                                    {

                                                        nightShiftsRemaining += (after - interval.StartTime.TimeOfDay);
                                                    }
                                                }
                                                if (nightShiftsRemaining.TotalMinutes != 0)
                                                {
                                                    // Set 7:59 intervals to 8:00
                                                    nightShiftsRemaining = nightShiftsRemaining.Minutes >= 45 ? new TimeSpan(nightShiftsRemaining.Hours + 1, 0, 0) : nightShiftsRemaining;

                                                    string hours = nightShiftsRemaining.Hours >= 10 ? nightShiftsRemaining.Hours.ToString() : "0" + nightShiftsRemaining.Hours.ToString();
                                                    string minutes = nightShiftsRemaining.Minutes >= 10 ? nightShiftsRemaining.Minutes.ToString() : "0" + nightShiftsRemaining.Minutes.ToString();
                                                    row["day_" + date.ToString()] = " " + hours + ":" + minutes; // Add space for lenght = 6. Change later for field last working day

                                                }
                                                else
                                                {
                                                    string hours = workingTime.Hours >= 10 ? workingTime.Hours.ToString() : "0" + workingTime.Hours.ToString();
                                                    string minutes = workingTime.Minutes >= 10 ? workingTime.Minutes.ToString() : "0" + workingTime.Minutes.ToString();
                                                    row["day_" + date.ToString()] = " " + hours + ":" + minutes; // Add space for lenght = 6. Change later for field last working day
                                                    totalMinutes += (int)workingTime.TotalMinutes;
                                                    RDS += (int)workingTime.TotalMinutes; // Calculate regular working hours remaining
                                                    //numOfDays++;
                                                }
                                                forecastField = 1;
                                            }
                                            ////////////////////////////////////////////////////
                                        }
                                    }

                                }
                                List<IOPairTO> dayIOPairList = Common.Misc.getEmployeeDayPairs(pairs, day, is2DayShift, is2DaysShiftPrevious, firstIntervalNextDay, dayIntervals);
                                if (empl.EmplTO.EmployeeID == 504 && date == 13)
                                    debug.writeLog("");
                                bool containRegularWork = false;
                                TimeSpan tsRegularWork = new TimeSpan();
                                TimeSpan tsNightShift = new TimeSpan();
                                bool containWholeDayAbsence = false;
                                TimeSpan tsWholeDayAbsence = new TimeSpan();

                                totalNightShift += (int)nightShiftsRemaining.TotalMinutes; // Nenad 
                                totalMinutes += (int)nightShiftsRemaining.TotalMinutes;

                                NRS += (int)nightShiftsRemaining.TotalMinutes; // Calculate regular night shift hours remaining
                                foreach (IOPairTO interval in dayIOPairList)
                                {
                                    if (interval.PassTypeID == 12)
                                        debug.writeLog("");
                                    if ((this.isPass(interval.PassTypeID) || interval.PassTypeID == Constants.overTimeID || interval.PassTypeID == Constants.earnedHoursID || interval.PassTypeID == Constants.overTimeSundayID) && interval.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter)
                                    {
                                        containRegularWork = true;
                                        tsRegularWork += (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                                        if (interval.PassTypeID == Constants.overTimeID)
                                            numOfDays += (int)(interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay).TotalMinutes;

                                        TimeSpan tsp = interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay;
                                        tsp = set15minBounds(tsp);

                                        // Nenad 1. XI 2017. Calculate working time by pass types
                                        switch (interval.PassTypeID)
                                        {
                                            case 1: RDS += (int)tsp.TotalMinutes;
                                                break;
                                            case Constants.overTimeID: PrS += (int)tsp.TotalMinutes;
                                                break;
                                            case 13: PLO += (int)tsp.TotalMinutes;
                                                break;
                                            case 14: NOS += (int)tsp.TotalMinutes;
                                                break;
                                            case 42: IZS += (int)tsp.TotalMinutes;
                                                break;
                                            case 87: IZS += (int)tsp.TotalMinutes;
                                                break;
                                            case Constants.earnedHoursID: UDR += (int)tsp.TotalMinutes;
                                                break;
                                            case Constants.overTimeSundayID: PrDp += (int)tsp.TotalMinutes;
                                                break;
                                        }

                                    }
                                }

                                foreach (IOPairTO interval in dayIOPairList)
                                {
                                    if (this.isWholeDayAbsence(interval.PassTypeID) && ((TimeSpan)(interval.EndTime.Subtract(interval.StartTime))).TotalSeconds > 0)
                                    {
                                        containWholeDayAbsence = true;
                                        PassAbsence = interval.PassTypeID;
                                        tsWholeDayAbsence += (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);

                                        TimeSpan tsp = (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                                        tsp = set15minBounds(tsp);
                                        // Nenad 1. XI 2017. Calculate whole day absences by pass types
                                        switch (interval.PassTypeID)
                                        {
                                            case 2: SPD += (int)tsp.TotalMinutes;
                                                break;
                                            case 8: GOS += (int)tsp.TotalMinutes;
                                                break;
                                            case 31: PLO += (int)tsp.TotalMinutes;
                                                break;
                                            case 32: PLO += (int)tsp.TotalMinutes;
                                                break;
                                            case 74: PLO += (int)tsp.TotalMinutes;
                                                break;
                                            case 75: PLO += (int)tsp.TotalMinutes;
                                                break;
                                            case 95: PLO += (int)tsp.TotalMinutes;
                                                break;
                                            case 96: PLO += (int)tsp.TotalMinutes;
                                                break;
                                            case 80: NOS += (int)tsp.TotalMinutes;
                                                break;
                                            case 41: NOS += (int)tsp.TotalMinutes;
                                                break;
                                            case 73: BVA += (int)tsp.TotalMinutes;
                                                break;
                                            case 25: BVA += (int)tsp.TotalMinutes;
                                                break;
                                            case 26: BPT += (int)tsp.TotalMinutes;
                                                break;
                                            case 27: B30V += (int)tsp.TotalMinutes;
                                                break;
                                            case 40: B30P += (int)tsp.TotalMinutes;
                                                break;
                                            case 43: B30T += (int)tsp.TotalMinutes;
                                                break;
                                            case 28: OPD += (int)tsp.TotalMinutes;
                                                break;
                                            case 53: UDR += (int)tsp.TotalMinutes;
                                                break;
                                            case 23: SUS += (int)tsp.TotalMinutes;
                                                break;
                                            case 38: SUS += (int)tsp.TotalMinutes;
                                                break;

                                        }
                                        //if (interval.IOPairDate.Day == lastDay && firstIntervalNextDay != null)
                                        //    tsWholeDayAbsence += (firstIntervalNextDay.EndTime.TimeOfDay - firstIntervalNextDay.StartTime.TimeOfDay);
                                    }
                                }

                                foreach (IOPairTO interval in dayIOPairList)
                                {
                                    //if(interval.EmployeeID == 2135)
                                    //{
                                    if (this.isPassNight(interval.PassTypeID) && ((TimeSpan)(interval.EndTime.Subtract(interval.StartTime))).TotalSeconds > 0)
                                    {
                                        TimeSpan EndHelp = new TimeSpan(07, 30, 0);
                                        TimeSpan startOnly = new TimeSpan(22, 0, 0);
                                        TimeSpan endOnly = new TimeSpan(23, 59, 59);
                                        TimeSpan startOnly1 = new TimeSpan(0, 0, 0);
                                        TimeSpan endOnly1 = new TimeSpan(6, 0, 0);

                                        if (interval.EndTime.TimeOfDay >= startOnly)
                                        {

                                            TimeSpan now = new TimeSpan(22, 0, 0);
                                            if (interval.StartTime.TimeOfDay >= now)
                                            {
                                                tsNightShift += (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                                            }

                                            else
                                            {
                                                if (interval.IOPairDate.Day == 31)
                                                {
                                                    tsNightShift += (interval.EndTime.TimeOfDay - now + endOnly1);
                                                }
                                                else if (interval.IOPairDate.Month == 2 && interval.IOPairDate.Day == 28)
                                                {
                                                    tsNightShift += (interval.EndTime.TimeOfDay - now + endOnly1);
                                                }
                                                else if ((interval.IOPairDate.Month == 4 && interval.IOPairDate.Day == 30) ||
                                                         (interval.IOPairDate.Month == 6 && interval.IOPairDate.Day == 30) ||
                                                         (interval.IOPairDate.Month == 9 && interval.IOPairDate.Day == 30) ||
                                                         (interval.IOPairDate.Month == 11 && interval.IOPairDate.Day == 30))
                                                {
                                                    tsNightShift += (interval.EndTime.TimeOfDay - now + endOnly1);
                                                }
                                                else
                                                {
                                                    tsNightShift += (interval.EndTime.TimeOfDay - now);
                                                }
                                            }

                                        }

                                        //else if (interval.EndTime.TimeOfDay <= endOnly1) 
                                        //{

                                        //    tsNightShift += (interval.EndTime.TimeOfDay - (interval.EndTime.TimeOfDay - endOnly1));
                                        //}

                                        if (interval.StartTime.TimeOfDay <= endOnly1)
                                        {


                                            TimeSpan now = new TimeSpan(0, 0, 0);
                                            TimeSpan after = new TimeSpan(6, 0, 0);
                                            if (interval.EndTime.TimeOfDay <= after)
                                            {
                                                tsNightShift += (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                                            }
                                            else
                                            {

                                                tsNightShift += (after - interval.StartTime.TimeOfDay);
                                            }
                                        }

                                        //else if (interval.StartTime.TimeOfDay >= startOnly)
                                        //{


                                        //    TimeSpan after = new TimeSpan(23, 59, 59);

                                        //    tsNightShift += (after - interval.StartTime.TimeOfDay);

                                        //}



                                    }
                                    //   }
                                }

                                if (containRegularWork)
                                {

                                    if (containWholeDayAbsence == false)
                                    {
                                        if (rbNumOfHours.Checked || rbForecast.Checked)
                                        {
                                            // Set 7:59 intervals to 8:00

                                            tsRegularWork = set15minBounds(tsRegularWork);
                                            tsNightShift = set15minBounds(tsNightShift);
                                            // if (tsRegularWork.TotalMinutes > 0) {
                                            string hours = tsRegularWork.Hours.ToString();
                                            if (tsRegularWork.Hours < 10)
                                                hours = "0" + hours;
                                            string minutes = tsRegularWork.Minutes.ToString();
                                            if (tsRegularWork.Minutes < 10)
                                                minutes = "0" + minutes;
                                            row["day_" + date.ToString()] = hours + ":" + minutes;

                                            NRS += (int)tsNightShift.TotalMinutes;
                                            //numOfDays++;
                                            // }
                                        }
                                        else
                                        {
                                            row["day_" + date.ToString()] = "+";
                                            setFlag(Constants.regularWork);
                                        }
                                        totalMinutes += (int)tsRegularWork.TotalMinutes;
                                        totalNightShift += (int)tsNightShift.TotalMinutes;
                                    }
                                    else
                                    {
                                        if (!isHoliday(day))
                                        {
                                            row["day_" + date.ToString()] = pTypes[PassAbsence].ToString() + "*";

                                            tsWholeDayAbsence = tsWholeDayAbsence.Minutes >= 45 ? new TimeSpan(tsWholeDayAbsence.Hours + 1, 0, 0) : tsWholeDayAbsence;
                                            totalMinutesAbsence += (int)tsWholeDayAbsence.TotalMinutes;
                                        }
                                        setFlag(PassAbsence);
                                    }
                                }
                                else if (containWholeDayAbsence == true)
                                {
                                    if (!isHoliday(day))
                                    {
                                        if (date == lastDay && is2DayShift)
                                        {
                                            TimeSpan now = new TimeSpan(0, 0, 0);
                                            TimeSpan after = new TimeSpan(6, 0, 0);
                                            tsWholeDayAbsence += (after - now);

                                        }

                                        //tsWholeDayAbsence = tsWholeDayAbsence.Minutes >= 45 ? new TimeSpan(tsWholeDayAbsence.Hours + 1, 0, 0) : tsWholeDayAbsence;
                                        tsWholeDayAbsence = set15minBounds(tsWholeDayAbsence);
                                        if (passTypesDesc.ContainsKey(PassAbsence))
                                        {
                                            row["day_" + date.ToString()] = passTypesDesc[PassAbsence];

                                        }
                                        totalMinutesAbsence += (int)tsWholeDayAbsence.TotalMinutes;
                                    }
                                }
                                else
                                {

                                    if (dayIOPairList.Count > 0 && ((TimeSpan)(dayIOPairList[0].EndTime.Subtract((dayIOPairList[0].StartTime)))).TotalSeconds > 0)
                                    {
                                        if ((dayIOPairList[0].PassTypeID == Constants.overTimeID
                                            || dayIOPairList[0].PassTypeID == Constants.overTimeSundayID)
                                            && dayIOPairList[0].IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter
                                            && dayIOPairList[0].IOPairID != 0)
                                        {
                                            TimeSpan overTime = dayIOPairList[0].EndTime.TimeOfDay - dayIOPairList[0].StartTime.TimeOfDay;
                                            overTime = set15minBounds(overTime);
                                            string hours = overTime.Hours.ToString();
                                            if (overTime.Hours < 10)
                                                hours = "0" + hours;
                                            string minutes = overTime.Minutes.ToString();
                                            if (overTime.Minutes < 10)
                                                minutes = "0" + minutes;
                                            row["day_" + date.ToString()] = hours + ":" + minutes;
                                            totalMinutes += (int)overTime.TotalMinutes;
                                            numOfDays += (int)overTime.TotalMinutes;
                                        }
                                        else
                                            if (pTypes.ContainsKey(dayIOPairList[0].PassTypeID))
                                            {
                                                row["day_" + date.ToString()] = pTypes[dayIOPairList[0].PassTypeID].ToString();

                                                TimeSpan tsp = (dayIOPairList[0].EndTime.TimeOfDay - dayIOPairList[0].StartTime.TimeOfDay);
                                                tsp = set15minBounds(ts);
                                                // Nenad 1. XI 2017. Calculate national and religion holidays by pass types
                                                switch (dayIOPairList[0].PassTypeID)
                                                {
                                                    case 12: VPR += (int)tsp.TotalMinutes;
                                                        break;
                                                    case 10: DPR += (int)tsp.TotalMinutes;
                                                        break;
                                                    case 5: PDP += (int)tsp.TotalMinutes;
                                                        break;
                                                }
                                            
                                            }
                                            else if (dayIOPairList[0].PassTypeID == Constants.absence && forecastField == 0)
                                            {
                                                row["day_" + date.ToString()] = "00:00";
                                            }
                                        //setFlag(((IOPairTO)pairDay[0]).PassTypeID);
                                    }
                                    else if (!shouldWork && forecastField == 0 && DateTime.Today.Day <= date)
                                    {
                                        row["day_" + date.ToString()] = "00:00";
                                    }
                                }
                            }
                        }
                    }
                    if (totalMinutes > 0 || numOfDays > 0 || totalMinutesAbsence > 0)
                    {
                        row["imageID"] = 1;
                        row["total"] = totalMinutes.ToString();
                        row["numOfDays"] = numOfDays.ToString(); // Num of days is overtime now
                        row["totalAbsence"] = totalMinutesAbsence.ToString();
                        //adding row for night shifts
                        row["totalNS"] = totalNightShift.ToString();
                        row["RDS"] = RDS.ToString(); // 1 
                        row["RDP"] = RDP.ToString(); // 
                        row["SPD"] = SPD.ToString(); // 2
                        row["SPN"] = SPN.ToString(); //
                        row["NRS"] = NRS.ToString(); // 6
                        row["NRP"] = NRP.ToString(); // 
                        row["GOS"] = GOS.ToString(); // 8
                        row["GOP"] = GOP.ToString(); // 
                        row["DPR"] = DPR.ToString(); // 10
                        row["VPR"] = VPR.ToString(); // 12
                        row["PLO"] = PLO.ToString(); // 13, 22, 58
                        row["NOS"] = NOS.ToString(); // 14, 41, 80
                        row["NOP"] = NOP.ToString(); //
                        row["BVA"] = BVA.ToString(); // 25, 39
                        row["BPT"] = BPT.ToString(); // 26
                        row["B30V"] = B30V.ToString(); // 27
                        row["B30P"] = B30P.ToString(); // 40
                        row["B30T"] = B30T.ToString(); // 43
                        row["BSP"] = BSP.ToString(); //
                        row["BPL"] = BPL.ToString(); //
                        row["OPD"] = OPD.ToString(); // 28
                        row["PrS"] = PrS.ToString(); // 4
                        row["PrP"] = PrP.ToString(); //
                        row["PrSP"] = PrSP.ToString(); //
                        row["PrNS"] = PrNS.ToString(); // 
                        row["PrN"] = PrN.ToString(); // 
                        row["PDP"] = PDP.ToString(); //5
                        row["PPPP"] = PPPP.ToString(); // 
                        row["NrVr"] = NrVr.ToString(); //
                        row["PrDp"] = PrDp.ToString(); //
                        row["NrDp"] = NrDp.ToString(); //
                        row["ZZS"] = ZZS.ToString(); // 
                        row["IZS"] = IZS.ToString(); //
                        row["SUS"] = SUS.ToString(); //
                        row["UDR"] = UDR.ToString(); //
                        if (counter == 0)
                        {
                            //add logo image just once
                            DataRow rowI = tableI.NewRow();
                            rowI["image"] = Constants.LogoForReport;
                            rowI["imageID"] = 1;
                            tableI.Rows.Add(rowI);
                            tableI.AcceptChanges();
                        }

                        table.Rows.Add(row);
                        counter++;
                    }
                    emplPairs.Clear();
                    
                }

                table.AcceptChanges();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.PrepareCRData(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            //MessageBox.Show(cnt.ToString());
            return dataSet;
        }

        private TimeSpan set15minBounds(TimeSpan time)
        {

            if (time.Minutes >= 0 && time.Minutes < 7.5)
                time = new TimeSpan(time.Hours, 0, 0);
            else if (time.Minutes >= 7.5 && time.Minutes < 22.5)
                time = new TimeSpan(time.Hours, 15, 0);
            else if (time.Minutes >= 22.5 && time.Minutes < 37.5)
                time = new TimeSpan(time.Hours, 30, 0);
            else if (time.Minutes >= 37.5 && time.Minutes < 52.5)
                time = new TimeSpan(time.Hours, 45, 0);
            else time = new TimeSpan(time.Hours + 1, 0, 0);
            return time;
        }

        private bool isPass(int currentPassTypeID)
        {
            bool isPass = false;

            try
            {
                PassType tempPass = new PassType();
                tempPass.PTypeTO.IsPass = Constants.passOnReader;
                List<PassTypeTO> allPass = tempPass.Search();

                foreach (PassTypeTO pass in allPass)
                {
                    if (pass.PassTypeID == currentPassTypeID)
                    {
                        isPass = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.isPass(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return isPass;
        }
        private bool isPassNight(int currentPassTypeID)
        {
            bool isPass = false;

            try
            {
                PassType tempPass = new PassType();
                tempPass.PTypeTO.IsPass = Constants.passOnReader;
                List<PassTypeTO> allPass = tempPass.Search();

                foreach (PassTypeTO pass in allPass)
                {
                    if (pass.PassTypeID == currentPassTypeID)
                    {
                        isPass = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.isPass(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return isPass;
        }
        private bool isWholeDayAbsence(int currentPassTypeID)
        {
            bool isWholeDayAbsence = false;

            try
            {
                PassType tempPass = new PassType();
                tempPass.PTypeTO.IsPass = Constants.wholeDayAbsence;
                List<PassTypeTO> allPass = tempPass.Search();

                foreach (PassTypeTO pass in allPass)
                {
                    if (pass.PassTypeID == currentPassTypeID)
                    {
                        isWholeDayAbsence = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.isWholeDayAbsence(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return isWholeDayAbsence;
        }

        private string setFlag(int passTypeID)
        {
            string flag = "";

            try
            {
                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                {
                    switch (passTypeID)
                    {
                        case -7:
                            flag = Constants.passTypeRO;
                            break;
                        case -4:
                            flag = Constants.passTypeRP;
                            break;
                        case -3:
                            flag = Constants.passTypeP;
                            break;
                        case 0:
                            flag = Constants.passTypeR;
                            break;
                        case 4:
                            flag = Constants.passTypeSD;
                            break;
                        case 5:
                            flag = Constants.passTypeO;
                            break;
                        case 6:
                            flag = Constants.passTypeB;
                            break;
                        case 7:
                            flag = Constants.passTypeSP;
                            break;
                        case 8:
                            flag = Constants.passTypeNO;
                            break;
                        case 11:
                            flag = Constants.passTypeS;
                            break;
                        case 12:
                            flag = Constants.passTypeB42;
                            break;
                        case 13:
                            flag = Constants.passTypeBP;
                            break;
                        case 14:
                            flag = Constants.passTypeI;
                            break;
                        case 15:
                            flag = Constants.passTypeC;
                            break;
                        case 9:
                            flag = Constants.passTypePO;
                            break;
                        default:
                            flag = Constants.passTypeCO;
                            break;
                    }
                }
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                {
                    switch (passTypeID)
                    {
                        case -7:
                            flag = Constants.passTypeRO;
                            break;
                        case -4:
                            flag = Constants.passTypeRP;
                            break;
                        case -3:
                            flag = Constants.passTypeP;
                            break;
                        case 0:
                            flag = Constants.passTypeR;
                            break;
                        case 4:
                            flag = Constants.passTypeSD;
                            break;
                        case 5:
                            flag = Constants.passTypeO;
                            break;
                        case 6:
                            flag = Constants.passTypeB;
                            break;
                        case 7:
                            flag = Constants.passTypeSP;
                            break;
                        case 8:
                            flag = Constants.passTypeNO;
                            break;
                        case 11:
                            flag = Constants.passTypeS;
                            break;
                        case 12:
                            flag = Constants.passTypeB42;
                            break;
                        case 13:
                            flag = Constants.passTypeBP;
                            break;
                        case 14:
                            flag = Constants.passTypeI;
                            break;
                        case 15:
                            flag = Constants.passTypeC;
                            break;
                        case 9:
                            flag = Constants.passTypePO;
                            break;
                        default:
                            flag = Constants.passTypeCO;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.setFlag(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return flag;
        }

        private void PresenceTracking_KeyUp(object sender, KeyEventArgs e)
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
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.PresenceTracking_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbWorkingUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Nenad 03.XI 2017. Load all working units for employees in selected organizational unit
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool check = false;
                //foreach (WorkingUnitTO wu in wUnits)
                //{
                //    if (cbWorkingUnit.SelectedIndex != 0)
                //    {
                //        if (wu.WorkingUnitID == (int)cbWorkingUnit.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                //        {
                //            check = true;
                //        }
                //    }
                //}

                //chbHierarhicly.Checked = check;
                employeeSelectedType = EmployeeSelectionTypes.WORKING_UNIT;
                populateWorkigUnitCombo();
            }

            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private string makeLegend(string description)
        {
            try
            {
                string desc = "";
                string temp = description;

                int index = -1;
                while (!temp.Trim().Equals("") && desc.Length < Constants.maxLegendDesc)
                {
                    string letter = temp.Trim().Substring(0, 1).ToUpper();
                    if (isLetter(letter))
                    {
                        desc += letter;
                    }

                    index = temp.IndexOf(' ');

                    if (index > 0)
                    {
                        temp = temp.Substring(index).Trim();
                    }
                    else
                    {
                        temp = "";
                    }
                }

                if (!legendDescriptions.ContainsKey(desc))
                {
                    legendDescriptions.Add(desc, 0);
                }
                else
                {
                    legendDescriptions[desc]++;
                    desc += legendDescriptions[desc].ToString().Trim();
                    legendDescriptions.Add(desc, 0);
                }

                return desc;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.makeLegend(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isLetter(string letter)
        {
            try
            {
                return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(letter.Trim().ToUpper()) > 0;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.isLetter(): " + ex.Message + "\n");
                throw ex;
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
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbNumOfHours_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbPresence_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbForecast.Checked)
            {
                //    foreach (OrganizationalUnitTO oo in oUnits)
                //    {
                //        if (oo.OrgUnitID == oo.ParentOrgUnitID)
                //        {
                //            cbOrganizationalUnits.SelectedValue = oo.OrgUnitID;
                //            chbHierarhicly.Checked = true;
                //            cbOrganizationalUnits.Enabled = false;
                //            chbHierarhicly.Enabled = false;
                dtTo.Value = DateTime.Today;
                dtTo.Enabled = false;
                //            chbShowRetired.Checked = true;
                //        }
            }
            else
            {
                //    cbOrganizationalUnits.Enabled = true;
                //    chbHierarhicly.Enabled = true;
                dtTo.Enabled = true;
            }

        }

        private void rbOrganizationalUnits_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void rbWorkingUnits_CheckedChanged(object sender, EventArgs e)
        {
           
        }

    }
}
