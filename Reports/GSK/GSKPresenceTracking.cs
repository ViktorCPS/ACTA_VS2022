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

namespace Reports.GSK
{
	/// <summary>
	/// Manage Employees presance report for one month.
	/// </summary>
    public class GSKPresenceTracking : System.Windows.Forms.Form
    {
        private System.Windows.Forms.ComboBox cbWorkingUnit;
        private System.Windows.Forms.Label lblWorkingUnit;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Button btnCancel;
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
        Dictionary<DateTime, HolidayTO> holidays = new Dictionary<DateTime,HolidayTO>();
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
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;

        // all time shemas
        List<WorkTimeSchemaTO> timeSchemas = new List<WorkTimeSchemaTO>();

        // key is legend description, and value is counter how many of them has been found same
        Dictionary<string, int> legendDescriptions = new Dictionary<string,int>();
        private RadioButton rbPresence;
        private GroupBox gbOverview;
        private RadioButton rbNumOfHours;
        private Label lblMinPresenceWorkingDay;
        private NumericUpDown numMealPresenceWorkingDay;
        private Label lblHrsWD;
        private DateTimePicker dtpTo;
        private Label lblTo;
        private Label lblHrsNWD;
        private NumericUpDown numMealPresenceNotWorkingDay;
        private Label lblMinPrasenceNonWorkingDay;
        private GroupBox gbReportType;
        private GroupBox gbMinimalMealPresence;

        Filter filter;

        public GSKPresenceTracking()
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
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbPDF = new System.Windows.Forms.CheckBox();
            this.cbCR = new System.Windows.Forms.CheckBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.gbWorkingUnit = new System.Windows.Forms.GroupBox();
            this.chbShowRetired = new System.Windows.Forms.CheckBox();
            this.gbFor = new System.Windows.Forms.GroupBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbOverview = new System.Windows.Forms.GroupBox();
            this.rbNumOfHours = new System.Windows.Forms.RadioButton();
            this.rbPresence = new System.Windows.Forms.RadioButton();
            this.lblMinPresenceWorkingDay = new System.Windows.Forms.Label();
            this.numMealPresenceWorkingDay = new System.Windows.Forms.NumericUpDown();
            this.lblHrsWD = new System.Windows.Forms.Label();
            this.lblHrsNWD = new System.Windows.Forms.Label();
            this.numMealPresenceNotWorkingDay = new System.Windows.Forms.NumericUpDown();
            this.lblMinPrasenceNonWorkingDay = new System.Windows.Forms.Label();
            this.gbReportType = new System.Windows.Forms.GroupBox();
            this.gbMinimalMealPresence = new System.Windows.Forms.GroupBox();
            this.gbWorkingUnit.SuspendLayout();
            this.gbFor.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.gbOverview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMealPresenceWorkingDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMealPresenceNotWorkingDay)).BeginInit();
            this.gbReportType.SuspendLayout();
            this.gbMinimalMealPresence.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(80, 24);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(238, 21);
            this.cbWorkingUnit.TabIndex = 1;
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(16, 24);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(48, 23);
            this.lblWorkingUnit.TabIndex = 0;
            this.lblWorkingUnit.Text = "Name:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(16, 295);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(128, 23);
            this.btnGenerate.TabIndex = 6;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(6, 25);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(58, 23);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Checked = false;
            this.dtpFrom.CustomFormat = "dd.MM.yyyy.";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(70, 28);
            this.dtpFrom.MinDate = new System.DateTime(2001, 1, 1, 0, 0, 0, 0);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(88, 20);
            this.dtpFrom.TabIndex = 1;
            this.dtpFrom.Value = new System.DateTime(2006, 9, 1, 12, 21, 9, 62);            
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(420, 295);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbPDF
            // 
            this.cbPDF.Enabled = false;
            this.cbPDF.Location = new System.Drawing.Point(9, 29);
            this.cbPDF.Name = "cbPDF";
            this.cbPDF.Size = new System.Drawing.Size(56, 24);
            this.cbPDF.TabIndex = 0;
            this.cbPDF.Tag = "FILTERABLE";
            this.cbPDF.Text = "PDF";
            this.cbPDF.Visible = false;
            // 
            // cbCR
            // 
            this.cbCR.Checked = true;
            this.cbCR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCR.Enabled = false;
            this.cbCR.Location = new System.Drawing.Point(80, 29);
            this.cbCR.Name = "cbCR";
            this.cbCR.Size = new System.Drawing.Size(48, 24);
            this.cbCR.TabIndex = 1;
            this.cbCR.Tag = "FILTERABLE";
            this.cbCR.Text = "CR";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(80, 51);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 2;
            this.chbHierarhicly.Text = "Hierarchy ";
            // 
            // gbWorkingUnit
            // 
            this.gbWorkingUnit.Controls.Add(this.chbHierarhicly);
            this.gbWorkingUnit.Controls.Add(this.cbWorkingUnit);
            this.gbWorkingUnit.Controls.Add(this.lblWorkingUnit);
            this.gbWorkingUnit.Controls.Add(this.chbShowRetired);
            this.gbWorkingUnit.Location = new System.Drawing.Point(16, 9);
            this.gbWorkingUnit.Name = "gbWorkingUnit";
            this.gbWorkingUnit.Size = new System.Drawing.Size(330, 106);
            this.gbWorkingUnit.TabIndex = 0;
            this.gbWorkingUnit.TabStop = false;
            this.gbWorkingUnit.Tag = "FILTERABLE";
            this.gbWorkingUnit.Text = "Working Units";
            // 
            // chbShowRetired
            // 
            this.chbShowRetired.AutoSize = true;
            this.chbShowRetired.Location = new System.Drawing.Point(80, 81);
            this.chbShowRetired.Name = "chbShowRetired";
            this.chbShowRetired.Size = new System.Drawing.Size(85, 17);
            this.chbShowRetired.TabIndex = 3;
            this.chbShowRetired.Tag = "FILTERABLE";
            this.chbShowRetired.Text = "Show retired";
            this.chbShowRetired.UseVisualStyleBackColor = true;
            // 
            // gbFor
            // 
            this.gbFor.Controls.Add(this.dtpTo);
            this.gbFor.Controls.Add(this.lblTo);
            this.gbFor.Controls.Add(this.dtpFrom);
            this.gbFor.Controls.Add(this.lblFrom);
            this.gbFor.Location = new System.Drawing.Point(16, 121);
            this.gbFor.Name = "gbFor";
            this.gbFor.Size = new System.Drawing.Size(330, 64);
            this.gbFor.TabIndex = 2;
            this.gbFor.TabStop = false;
            this.gbFor.Tag = "FILTERABLE";
            this.gbFor.Text = "For";
            // 
            // dtpTo
            // 
            this.dtpTo.Checked = false;
            this.dtpTo.CustomFormat = "dd.MM.yyyy.";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(217, 28);
            this.dtpTo.MinDate = new System.DateTime(2001, 1, 1, 0, 0, 0, 0);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(88, 20);
            this.dtpTo.TabIndex = 3;
            this.dtpTo.Value = new System.DateTime(2006, 9, 1, 12, 21, 9, 62);
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(164, 25);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(47, 23);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(358, 9);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 106);
            this.gbFilter.TabIndex = 1;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
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
            // gbOverview
            // 
            this.gbOverview.Controls.Add(this.rbNumOfHours);
            this.gbOverview.Controls.Add(this.rbPresence);
            this.gbOverview.Location = new System.Drawing.Point(358, 121);
            this.gbOverview.Name = "gbOverview";
            this.gbOverview.Size = new System.Drawing.Size(137, 64);
            this.gbOverview.TabIndex = 3;
            this.gbOverview.TabStop = false;
            this.gbOverview.Text = "Overview";
            // 
            // rbNumOfHours
            // 
            this.rbNumOfHours.AutoSize = true;
            this.rbNumOfHours.Checked = true;
            this.rbNumOfHours.Location = new System.Drawing.Point(6, 41);
            this.rbNumOfHours.Name = "rbNumOfHours";
            this.rbNumOfHours.Size = new System.Drawing.Size(103, 17);
            this.rbNumOfHours.TabIndex = 1;
            this.rbNumOfHours.TabStop = true;
            this.rbNumOfHours.Text = "Number of hours";
            this.rbNumOfHours.UseVisualStyleBackColor = true;
            // 
            // rbPresence
            // 
            this.rbPresence.AutoSize = true;
            this.rbPresence.Location = new System.Drawing.Point(6, 18);
            this.rbPresence.Name = "rbPresence";
            this.rbPresence.Size = new System.Drawing.Size(85, 17);
            this.rbPresence.TabIndex = 0;
            this.rbPresence.TabStop = true;
            this.rbPresence.Text = "+ (Presence)";
            this.rbPresence.UseVisualStyleBackColor = true;
            // 
            // lblMinPresenceWorkingDay
            // 
            this.lblMinPresenceWorkingDay.Location = new System.Drawing.Point(16, 19);
            this.lblMinPresenceWorkingDay.Name = "lblMinPresenceWorkingDay";
            this.lblMinPresenceWorkingDay.Size = new System.Drawing.Size(116, 23);
            this.lblMinPresenceWorkingDay.TabIndex = 0;
            this.lblMinPresenceWorkingDay.Tag = "FILTERABLE";
            this.lblMinPresenceWorkingDay.Text = "Working day:";
            this.lblMinPresenceWorkingDay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numMealPresenceWorkingDay
            // 
            this.numMealPresenceWorkingDay.Location = new System.Drawing.Point(138, 22);
            this.numMealPresenceWorkingDay.Name = "numMealPresenceWorkingDay";
            this.numMealPresenceWorkingDay.Size = new System.Drawing.Size(50, 20);
            this.numMealPresenceWorkingDay.TabIndex = 1;
            this.numMealPresenceWorkingDay.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // lblHrsWD
            // 
            this.lblHrsWD.Location = new System.Drawing.Point(194, 19);
            this.lblHrsWD.Name = "lblHrsWD";
            this.lblHrsWD.Size = new System.Drawing.Size(30, 23);
            this.lblHrsWD.TabIndex = 2;
            this.lblHrsWD.Text = "hrs";
            this.lblHrsWD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHrsNWD
            // 
            this.lblHrsNWD.Location = new System.Drawing.Point(194, 44);
            this.lblHrsNWD.Name = "lblHrsNWD";
            this.lblHrsNWD.Size = new System.Drawing.Size(30, 23);
            this.lblHrsNWD.TabIndex = 5;
            this.lblHrsNWD.Text = "hrs";
            this.lblHrsNWD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numMealPresenceNotWorkingDay
            // 
            this.numMealPresenceNotWorkingDay.Location = new System.Drawing.Point(138, 47);
            this.numMealPresenceNotWorkingDay.Name = "numMealPresenceNotWorkingDay";
            this.numMealPresenceNotWorkingDay.Size = new System.Drawing.Size(50, 20);
            this.numMealPresenceNotWorkingDay.TabIndex = 4;
            this.numMealPresenceNotWorkingDay.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // lblMinPrasenceNonWorkingDay
            // 
            this.lblMinPrasenceNonWorkingDay.Location = new System.Drawing.Point(6, 44);
            this.lblMinPrasenceNonWorkingDay.Name = "lblMinPrasenceNonWorkingDay";
            this.lblMinPrasenceNonWorkingDay.Size = new System.Drawing.Size(126, 23);
            this.lblMinPrasenceNonWorkingDay.TabIndex = 3;
            this.lblMinPrasenceNonWorkingDay.Tag = "FILTERABLE";
            this.lblMinPrasenceNonWorkingDay.Text = "Nonworking day:";
            this.lblMinPrasenceNonWorkingDay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbReportType
            // 
            this.gbReportType.Controls.Add(this.cbPDF);
            this.gbReportType.Controls.Add(this.cbCR);
            this.gbReportType.Location = new System.Drawing.Point(358, 191);
            this.gbReportType.Name = "gbReportType";
            this.gbReportType.Size = new System.Drawing.Size(137, 78);
            this.gbReportType.TabIndex = 5;
            this.gbReportType.TabStop = false;
            this.gbReportType.Text = "Report type";
            // 
            // gbMinimalMealPresence
            // 
            this.gbMinimalMealPresence.Controls.Add(this.numMealPresenceWorkingDay);
            this.gbMinimalMealPresence.Controls.Add(this.lblMinPresenceWorkingDay);
            this.gbMinimalMealPresence.Controls.Add(this.lblHrsNWD);
            this.gbMinimalMealPresence.Controls.Add(this.lblHrsWD);
            this.gbMinimalMealPresence.Controls.Add(this.numMealPresenceNotWorkingDay);
            this.gbMinimalMealPresence.Controls.Add(this.lblMinPrasenceNonWorkingDay);
            this.gbMinimalMealPresence.Location = new System.Drawing.Point(16, 191);
            this.gbMinimalMealPresence.Name = "gbMinimalMealPresence";
            this.gbMinimalMealPresence.Size = new System.Drawing.Size(330, 78);
            this.gbMinimalMealPresence.TabIndex = 4;
            this.gbMinimalMealPresence.TabStop = false;
            this.gbMinimalMealPresence.Text = "Minimal meal presence";
            // 
            // GSKPresenceTracking
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(509, 344);
            this.ControlBox = false;
            this.Controls.Add(this.gbMinimalMealPresence);
            this.Controls.Add(this.gbReportType);
            this.Controls.Add(this.gbOverview);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.gbFor);
            this.Controls.Add(this.gbWorkingUnit);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GSKPresenceTracking";
            this.ShowInTaskbar = false;
            this.Text = "PresenceTracking";
            this.Load += new System.EventHandler(this.PresenceTracking_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PresenceTracking_KeyUp);
            this.gbWorkingUnit.ResumeLayout(false);
            this.gbWorkingUnit.PerformLayout();
            this.gbFor.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.gbOverview.ResumeLayout(false);
            this.gbOverview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMealPresenceWorkingDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMealPresenceNotWorkingDay)).EndInit();
            this.gbReportType.ResumeLayout(false);
            this.gbMinimalMealPresence.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void populateWorkigUnitCombo()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wuArray = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), Constants.DefaultStateActive.ToString(), -1));

                cbWorkingUnit.DataSource = wuArray;
                cbWorkingUnit.DisplayMember = "Name";
                cbWorkingUnit.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PresenceTracking.populateWorkigUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setLanguage()
        {
            this.Text = rm.GetString("presenceTracking", culture);

            // group box text
            gbWorkingUnit.Text = rm.GetString("workingUnits", culture);
            gbFor.Text = rm.GetString("gbPeriod", culture);
            gbFilter.Text = rm.GetString("gbFilter", culture);
            gbOverview.Text = rm.GetString("gbOverview", culture);
            gbReportType.Text = rm.GetString("lblDocFormat", culture);
            gbMinimalMealPresence.Text = rm.GetString("gbMinimalMealPresence", culture);

            //radio button's text
            rbNumOfHours.Text = rm.GetString("rbNumOfHours", culture);
            rbPresence.Text = rm.GetString("rbPresence", culture);

            // check box text
            chbHierarhicly.Text = rm.GetString("hierarchically", culture);
            this.chbShowRetired.Text = rm.GetString("chbShowRetired", culture);

            // button text
            this.btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
            this.btnCancel.Text = rm.GetString("btnCancel", culture);

            // label's text
            this.lblWorkingUnit.Text = rm.GetString("lblName", culture);
            this.lblFrom.Text = rm.GetString("lblFrom", culture);
            this.lblTo.Text = rm.GetString("lblTo", culture);
            this.lblHrsWD.Text = rm.GetString("lblHrs", culture);
            this.lblHrsNWD.Text = rm.GetString("lblHrs", culture);
            this.lblMinPresenceWorkingDay.Text = rm.GetString("lblWorkingDay", culture);
            this.lblMinPrasenceNonWorkingDay.Text = rm.GetString("lbNonWorkingDay", culture);
        }

        private void PresenceTracking_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                populateWorkigUnitCombo();
                dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 19).AddMonths(-1).Date;
                dtpTo.Value = dtpFrom.Value.Date.AddMonths(1).AddDays(-1);
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
            finally {
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
            try
            {
                //debug.writeLog(DateTime.Now + " PresenceTracking.btnGenerateReport_Click() \n");
                this.Cursor = Cursors.WaitCursor;
                legendDescriptions = new Dictionary<string, int>();
                if (wUnits.Count == 0)
                {
                    MessageBox.Show(rm.GetString("noWUGranted", culture));
                }
                else
                {
                    if (this.cbWorkingUnit.SelectedIndex.Equals(0))
                    {
                        MessageBox.Show(rm.GetString("noWUSelected", culture));
                        return;
                    }

                    // check date interval
                    if (dtpFrom.Value > dtpTo.Value)
                    {
                        MessageBox.Show(rm.GetString("invalidDateInterval", culture));
                        return;
                    }

                    if ((int)dtpTo.Value.Date.Subtract(dtpFrom.Value.Date).TotalDays > 31)
                    {
                        MessageBox.Show(rm.GetString("moreThen31DaysPeriod", culture));
                        return;
                    }

                    int wuID = -1;
                    string wUnitsString = "";
                    List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                    wuID = (int)cbWorkingUnit.SelectedValue;

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

                    // Get IOPairs
                    //ArrayList data = prepareData();
                    List<IOPairTO> data = prepareData(wuID, wUnitsString);

                    // Sanja: changed 04.02.2009. - to show all employees from selected working units
                    //ArrayList emplList = getEmployees(wuID, wUnitsString);
                    List<EmployeeTO> emplList = new Employee().SearchByWU(wUnitsString);
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
                                GSK_sr.EmployeePresenceCRView view =
                                    new GSK_sr.EmployeePresenceCRView(crData, dtpFrom.Value.Date, dtpTo.Value.Date);
                                view.ShowDialog(this);
                            }
                            else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                            {
                                Reports_en.EmployeePresenceCRView_en view =
                                    new Reports_en.EmployeePresenceCRView_en(crData, this.dtpFrom.Value);
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
                DateTime selectd = dtpFrom.Value;

                this.from = new DateTime(selectd.Year, selectd.Month, 1, 0, 0, 0);
                this.to = this.from.AddMonths(1);
                //this.to = new DateTime(selectd.Year, selectd.Month + 1, 1, 0, 0 ,0);
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
                DateTime selectd = dtpFrom.Value;
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

            table.Columns.Add("employee_id", typeof(System.String));
            table.Columns.Add("first_name", typeof(System.String));
            table.Columns.Add("last_name", typeof(System.String));
            table.Columns.Add("working_unit", typeof(System.String));
            table.Columns.Add("date", typeof(DateTime));
            table.Columns.Add("date_to", typeof(DateTime));

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
            table.Columns.Add("imageID", typeof(byte));

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
                if (pt.PassTypeID == Constants.regularWork || pt.IsPass != Constants.passOnReader)
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
                if (passTypes[index].PassTypeID >= 0)
                {
                    string legendDesc = "";
                    if (passTypes[index].PassTypeID > 0)
                    {
                        legendDesc = makeLegend(passTypes[index].Description);
                    }
                    else
                    {
                        if(rbPresence.Checked)
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

            Dictionary<int, List<IOPairTO>> emplPairs = new Dictionary<int,List<IOPairTO>>(); // Key: Day, Value:IOPair List
            List<IOPairTO> pairDay = new List<IOPairTO>();
            IOPairTO pairTemp = new IOPairTO();

            try
            {
                int counter = 0;
                foreach (EmployeeTO employee in employeeList)
                {

                    List<EmployeeTimeScheduleTO> emplDaySchedules = Common.Misc.GetEmployeeTimeSchedules(employee.EmployeeID, dtpFrom.Value.Date, dtpTo.Value.Date);
                    //int lastDay = new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, 1).AddMonths(1).AddDays(-1).Day;
                    List<IOPairTO> pairs = new List<IOPairTO>();  
                    DataRow row = table.NewRow();

                    row["employee_id"] = employee.EmployeeID;
                    row["last_name"] = employee.LastName +" "+ employee.FirstName;
                    row["first_name"] = employee.FirstName;
                    row["working_unit"] = employee.WorkingUnitName;
                    row["date"] = dtpFrom.Value.Date;
                    row["date_to"] = dtpTo.Value.Date;

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
                    int totalDays = 0;
                    int totalMinutesAbsence = 0;
                    int numOfDays = 0;
                    bool shouldWork = false;
                    int date = 0;
                    for (DateTime day = dtpFrom.Value.Date; day <= dtpTo.Value.Date; day = day.AddDays(1).Date)
                    {
                        date++;
                        //pairDay = (ArrayList)emplPairs[date];
                        int PassAbsence = 0;
                        shouldWork = false;
                        //DateTime day = new DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, date);

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
                                List<WorkTimeIntervalTO> intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DaysShiftPrevious, firstIntervalNextDay, edi);
                                Dictionary<int, WorkTimeIntervalTO> dayIntervals = new Dictionary<int,WorkTimeIntervalTO>();
                                TimeSpan ts = new TimeSpan();
                                for (int i = 0; i < intervals.Count; i++)
                                {
                                    WorkTimeIntervalTO interval = intervals[i];
                                    dayIntervals.Add(i, interval);
                                    shouldWork = true;
                                    ts += interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay;
                                }
                                if (ts.TotalMinutes > 0)
                                {
                                    if (day.Date <= DateTime.Now.Date && !isHoliday(day))
                                        numOfDays++;
                                }
                                List<IOPairTO> dayIOPairList = Common.Misc.getEmployeeDayPairs(pairs, day, is2DayShift, is2DaysShiftPrevious, firstIntervalNextDay, dayIntervals);
                                
                                bool containRegularWork = false;
                                TimeSpan tsRegularWork = new TimeSpan();
                                bool containWholeDayAbsence = false;
                                TimeSpan tsWholeDayAbsence = new TimeSpan();

                                foreach (IOPairTO interval in dayIOPairList)
                                {
                                    if (this.isPass(interval.PassTypeID)&&interval.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter
                                        && (interval.PassTypeID == Constants.regularWork|| interval.PassTypeID == Constants.automaticPause))
                                    {
                                        containRegularWork = true;
                                        tsRegularWork += (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);                                       
                                    }
                                }

                                foreach (IOPairTO interval in dayIOPairList)
                                {
                                    if (this.isWholeDayAbsence(interval.PassTypeID) && ((TimeSpan)(interval.EndTime.Subtract(interval.StartTime))).TotalSeconds > 0)
                                    {
                                        containWholeDayAbsence = true;

                                        PassAbsence = interval.PassTypeID;
                                        tsWholeDayAbsence += (interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay);
                                    }
                                }
                            
                                if (containRegularWork)
                                {
                                    if (containWholeDayAbsence == false)
                                    {
                                        if (rbNumOfHours.Checked)
                                        {
                                            string hours = tsRegularWork.Hours.ToString();
                                            if (tsRegularWork.Hours < 10)
                                                hours = "0" + hours;
                                            string minutes = tsRegularWork.Minutes.ToString();
                                            if (tsRegularWork.Minutes < 10)
                                                minutes = "0" + minutes;
                                            row["day_" + date.ToString()] = hours + ":" + minutes;                                            
                                        }
                                        else
                                        {
                                            row["day_" + date.ToString()] = "+";
                                            setFlag(Constants.regularWork);
                                        }

                                        int minMealPresence = (int)numMealPresenceWorkingDay.Value * 60;

                                        if (ts.TotalMinutes <= 0)
                                            minMealPresence = (int)numMealPresenceNotWorkingDay.Value * 60;

                                        if (tsRegularWork.TotalMinutes >= minMealPresence)
                                            totalDays++;

                                        totalMinutes += (int)tsRegularWork.TotalMinutes;
                                    }
                                    else
                                    {
                                        if (!isHoliday(day))
                                        {
                                            row["day_" + date.ToString()] = pTypes[PassAbsence].ToString() + "*";

                                            totalMinutesAbsence += (int)tsWholeDayAbsence.TotalMinutes;
                                        }
                                        //setFlag(PassAbsence);
                                    }
                                }
                                else if (containWholeDayAbsence == true)
                                {
                                    if (!isHoliday(day))
                                    {
                                        row["day_" + date.ToString()] = pTypes[PassAbsence].ToString();
                                        totalMinutesAbsence += (int)tsWholeDayAbsence.TotalMinutes;
                                    }
                                }
                                else
                                {
                                    if (dayIOPairList.Count > 0 && ((TimeSpan)(dayIOPairList[0].EndTime.Subtract((dayIOPairList[0].StartTime)))).TotalSeconds > 0)
                                    {
                                        if (this.isPass(dayIOPairList[0].PassTypeID) && dayIOPairList[0].IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter)
                                            row["day_" + date.ToString()] = "00:00";
                                        else
                                            if(pTypes.ContainsKey(dayIOPairList[0].PassTypeID))
                                                row["day_" + date.ToString()] = pTypes[dayIOPairList[0].PassTypeID].ToString();
                                        //setFlag(((IOPair)pairDay[0]).PassTypeID);
                                    }
                                    else if(!shouldWork)
                                    {
                                        row["day_" + date.ToString()] = "00:00";
                                    }
                                }
                            }
                            
                        }
                    }

                    row["imageID"] = 1;
                    row["total"] = totalDays.ToString();
                    row["numOfDays"] = numOfDays.ToString();
                    int totalAbs = numOfDays - totalDays;
                    row["totalAbsence"] = totalAbs.ToString();
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

                    emplPairs.Clear();
                }

                table.AcceptChanges();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.PrepareCRData(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return dataSet;
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
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool check = false;
                foreach (WorkingUnitTO wu in wUnits)
                {
                    if (cbWorkingUnit.SelectedIndex != 0)
                    {
                        if (wu.WorkingUnitID == (int)cbWorkingUnit.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                        {
                            check = true;
                        }
                    }
                }
                chbHierarhicly.Checked = check;
            }

            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + this.Name + " PresenceTracking.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }
        
    }
}
