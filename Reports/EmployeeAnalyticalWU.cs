using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Globalization;

using Common;
using TransferObjects;
using Util;

namespace Reports
{
    /// <summary>
    /// Summary description for EmployeeAnalyticalWU.
    /// </summary>
    public class EmployeeAnalyticalWU : System.Windows.Forms.Form
    {
        private System.Windows.Forms.CheckBox cbCR;
        private System.Windows.Forms.CheckBox cbTXT;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.CheckBox checkbCSV;
        private System.Windows.Forms.CheckBox checkbPDF;
        private System.Windows.Forms.Label lblDocFormat;
        private System.Windows.Forms.GroupBox gbTimeInterval;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.GroupBox gbWorkingUnit;
        private System.Windows.Forms.CheckBox chbHierarhicly;
        private System.Windows.Forms.ComboBox cbWorkingUnit;
        private System.Windows.Forms.Label lblWorkingUnitName;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private DateTime date;

        // Working Units that logInUser is granted to
        List<WorkingUnitTO> wUnits;

        CultureInfo culture;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CheckBox chbShowRetired;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;

        DebugLog debug;

        Filter filter;

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("workingUnitReports", culture);

                gbWorkingUnit.Text = rm.GetString("workingUnits", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
                lblWorkingUnitName.Text = rm.GetString("lblName", culture);
                chbHierarhicly.Text = rm.GetString("hierarchically", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                gbTimeInterval.Text = rm.GetString("timeInterval", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblDocFormat.Text = rm.GetString("lblDocFormat", culture);
                btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);
                this.Text = rm.GetString("EmplAnalyticalWU", culture);
                this.chbShowRetired.Text = rm.GetString("chbShowRetired", culture);

                gbFilter.Text = rm.GetString("gbFilter", culture);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " EmployeeAnalyticalWU.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public EmployeeAnalyticalWU()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Init debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(EmployeeAnalyticalWU).Assembly);
            setLanguage();

            logInUser = NotificationController.GetLogInUser();
            populateWorkigUnitCombo();

            DateTime date = DateTime.Now.Date;
            this.CenterToScreen();

            dtpFromDate.Value = new DateTime(date.Year, date.Month, 1);
            dtpToDate.Value = date;

            this.cbTXT.Enabled=false;
            this.cbCR.Enabled=false;
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
            this.cbCR = new System.Windows.Forms.CheckBox();
            this.cbTXT = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.checkbCSV = new System.Windows.Forms.CheckBox();
            this.checkbPDF = new System.Windows.Forms.CheckBox();
            this.lblDocFormat = new System.Windows.Forms.Label();
            this.gbTimeInterval = new System.Windows.Forms.GroupBox();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.gbWorkingUnit = new System.Windows.Forms.GroupBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnitName = new System.Windows.Forms.Label();
            this.chbShowRetired = new System.Windows.Forms.CheckBox();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbTimeInterval.SuspendLayout();
            this.gbWorkingUnit.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbCR
            // 
            this.cbCR.Checked = true;
            this.cbCR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCR.Location = new System.Drawing.Point(346, 224);
            this.cbCR.Name = "cbCR";
            this.cbCR.Size = new System.Drawing.Size(56, 24);
            this.cbCR.TabIndex = 13;
            this.cbCR.Tag = "FILTERABLE";
            this.cbCR.Text = "CR";
            // 
            // cbTXT
            // 
            this.cbTXT.Location = new System.Drawing.Point(282, 224);
            this.cbTXT.Name = "cbTXT";
            this.cbTXT.Size = new System.Drawing.Size(48, 24);
            this.cbTXT.TabIndex = 12;
            this.cbTXT.Tag = "FILTERABLE";
            this.cbTXT.Text = "TXT";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(518, 330);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(18, 330);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(136, 23);
            this.btnGenerate.TabIndex = 14;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // checkbCSV
            // 
            this.checkbCSV.Enabled = false;
            this.checkbCSV.Location = new System.Drawing.Point(210, 224);
            this.checkbCSV.Name = "checkbCSV";
            this.checkbCSV.Size = new System.Drawing.Size(48, 24);
            this.checkbCSV.TabIndex = 11;
            this.checkbCSV.Tag = "FILTERABLE";
            this.checkbCSV.Text = "CSV";
            // 
            // checkbPDF
            // 
            this.checkbPDF.Enabled = false;
            this.checkbPDF.Location = new System.Drawing.Point(146, 224);
            this.checkbPDF.Name = "checkbPDF";
            this.checkbPDF.Size = new System.Drawing.Size(48, 24);
            this.checkbPDF.TabIndex = 10;
            this.checkbPDF.Tag = "FILTERABLE";
            this.checkbPDF.Text = "PDF";
            this.checkbPDF.Visible = false;
            // 
            // lblDocFormat
            // 
            this.lblDocFormat.Location = new System.Drawing.Point(18, 224);
            this.lblDocFormat.Name = "lblDocFormat";
            this.lblDocFormat.Size = new System.Drawing.Size(104, 23);
            this.lblDocFormat.TabIndex = 9;
            this.lblDocFormat.Tag = "FILTERABLE";
            this.lblDocFormat.Text = "Document Format";
            this.lblDocFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.dtpToDate);
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.dtpFromDate);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Location = new System.Drawing.Point(18, 136);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(456, 64);
            this.gbTimeInterval.TabIndex = 4;
            this.gbTimeInterval.TabStop = false;
            this.gbTimeInterval.Tag = "FILTERABLE";
            this.gbTimeInterval.Text = "Date Interval";
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd.MM.yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(320, 24);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(120, 20);
            this.dtpToDate.TabIndex = 8;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(280, 24);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(24, 23);
            this.lblTo.TabIndex = 7;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd.MM.yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(80, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(120, 20);
            this.dtpFromDate.TabIndex = 6;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(16, 24);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 23);
            this.lblFrom.TabIndex = 5;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbWorkingUnit
            // 
            this.gbWorkingUnit.Controls.Add(this.chbHierarhicly);
            this.gbWorkingUnit.Controls.Add(this.cbWorkingUnit);
            this.gbWorkingUnit.Controls.Add(this.lblWorkingUnitName);
            this.gbWorkingUnit.Location = new System.Drawing.Point(18, 24);
            this.gbWorkingUnit.Name = "gbWorkingUnit";
            this.gbWorkingUnit.Size = new System.Drawing.Size(456, 91);
            this.gbWorkingUnit.TabIndex = 0;
            this.gbWorkingUnit.TabStop = false;
            this.gbWorkingUnit.Tag = "FILTERABLE";
            this.gbWorkingUnit.Text = "Working Units";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(80, 56);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 3;
            this.chbHierarhicly.Text = "Hierarchy ";
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(80, 24);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(248, 21);
            this.cbWorkingUnit.TabIndex = 2;
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // lblWorkingUnitName
            // 
            this.lblWorkingUnitName.Location = new System.Drawing.Point(16, 24);
            this.lblWorkingUnitName.Name = "lblWorkingUnitName";
            this.lblWorkingUnitName.Size = new System.Drawing.Size(48, 23);
            this.lblWorkingUnitName.TabIndex = 1;
            this.lblWorkingUnitName.Text = "Name:";
            this.lblWorkingUnitName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowRetired
            // 
            this.chbShowRetired.AutoSize = true;
            this.chbShowRetired.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chbShowRetired.Location = new System.Drawing.Point(71, 288);
            this.chbShowRetired.Name = "chbShowRetired";
            this.chbShowRetired.Size = new System.Drawing.Size(85, 17);
            this.chbShowRetired.TabIndex = 16;
            this.chbShowRetired.Tag = "FILTERABLE";
            this.chbShowRetired.Text = "Show retired";
            this.chbShowRetired.UseVisualStyleBackColor = true;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(485, 24);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 91);
            this.gbFilter.TabIndex = 29;
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
            // EmployeeAnalyticalWU
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(634, 377);
            this.ControlBox = false;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.chbShowRetired);
            this.Controls.Add(this.cbCR);
            this.Controls.Add(this.cbTXT);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.checkbCSV);
            this.Controls.Add(this.checkbPDF);
            this.Controls.Add(this.lblDocFormat);
            this.Controls.Add(this.gbTimeInterval);
            this.Controls.Add(this.gbWorkingUnit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmployeeAnalyticalWU";
            this.ShowInTaskbar = false;
            this.Text = "EmployeeAnalyticalWU";
            this.Load += new System.EventHandler(this.EmployeeAnalyticalWU_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmployeeAnalyticalWU_KeyUp);
            this.gbTimeInterval.ResumeLayout(false);
            this.gbWorkingUnit.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWorkingUnit.DataSource = wuArray;
                cbWorkingUnit.DisplayMember = "Name";
                cbWorkingUnit.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " EmployeeAnalyticalWU.populateWorkigUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public WorkTimeIntervalTO getTimeSchemaInterval(int employeeID, DateTime date, List<EmployeeTimeScheduleTO> timeScheduleList)
        {
            WorkTimeIntervalTO interval = new WorkTimeIntervalTO();

            int timeScheduleIndex = -1;

            for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
            {
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                if ((date >= timeScheduleList[scheduleIndex].Date) && employeeID == timeScheduleList[scheduleIndex].EmployeeID)
                    //&& (date.Month == ((EmployeesTimeSchedule)(timeScheduleList[scheduleIndex])).Date.Month))
                {
                    timeScheduleIndex = scheduleIndex;
                }
            }

            if (timeScheduleIndex >= 0)
            {
                int cycleDuration = 0;
                int startDay = timeScheduleList[timeScheduleIndex].StartCycleDay;
                int schemaID = timeScheduleList[timeScheduleIndex].TimeSchemaID;
                TimeSchema sch = new TimeSchema();
                sch.TimeSchemaTO.TimeSchemaID = schemaID;
                List<WorkTimeSchemaTO> timeSchema = sch.Search();
                WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                if (timeSchema.Count > 0)
                {
                    schema = timeSchema[0];
                    cycleDuration = schema.CycleDuration;
                }

                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                //TimeSpan days = date - ((EmployeesTimeSchedule)(timeScheduleList[timeScheduleIndex])).Date;
                //interval = (TimeSchemaInterval)((Hashtable)(schema.Days[(startDay + days.Days) % cycleDuration]))[0];
                TimeSpan days = new TimeSpan(date.Date.Ticks - timeScheduleList[timeScheduleIndex].Date.Date.Ticks);
                interval = schema.Days[(startDay + (int)days.TotalDays) % cycleDuration][0];
            }

            return interval;
        }

        private void btnGenerate_Click(object sender, System.EventArgs e)
        {
            try
            {
                debug.writeLog(DateTime.Now + " EmployeeAnalyticalWU.btnGenerateReport_Click() \n");
                this.Cursor = Cursors.WaitCursor;
                Hashtable passTypeSummary = new Hashtable();
                
                if (wUnits.Count == 0)
                {
                    MessageBox.Show(rm.GetString("noWUGranted", culture));
                }
                else
                {
                    int selectedWorkingUnit = (int)cbWorkingUnit.SelectedValue;

                    if (this.chbHierarhicly.Checked)
                    {
                        WorkingUnit wu = new WorkingUnit();
                        if (selectedWorkingUnit != -1)
                        {
                            wu.WUTO.WorkingUnitID = selectedWorkingUnit;
                            wUnits = wu.Search();
                        }
                        else
                        {
                            if (selectedWorkingUnit == -1)
                            {
                                for (int i = wUnits.Count - 1; i >= 0; i--)
                                {
                                    if (wUnits[i].WorkingUnitID == wUnits[i].ParentWorkingUID)
                                    {
                                        wUnits.RemoveAt(i);
                                    }
                                }
                            }
                        }
                        wUnits = wu.FindAllChildren(wUnits);
                        selectedWorkingUnit = -1;
                    }

                    string wuString = "";
                    foreach (WorkingUnitTO wu in wUnits)
                    {
                        wuString += wu.WorkingUnitID.ToString().Trim() + ",";

                    }

                    if (wuString.Length > 0)
                    {
                        wuString = wuString.Substring(0, wuString.Length - 1);
                    }

                    IOPair ioPair = new IOPair();

                    // list of pairs for report
                    List<IOPairTO> ioPairList = new List<IOPairTO>();

                    // list of Time Schemas for selected Employee and selected Time Interval
                    List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();

                    // list of Time Schedules for one month
                    //ArrayList timeSchedule = new ArrayList();

                    List<EmployeeTO> EmployeeListWU = new List<EmployeeTO>();
                    if (this.chbHierarhicly.Checked)
                    {
                        EmployeeListWU = new Employee().SearchByWU(wuString);

                    }
                    else
                    {
                        EmployeeListWU = new Employee().SearchByWU(selectedWorkingUnit.ToString());
                    }
                    List<EmployeeTO> EmplList = EmployeeListWU;
                    EmployeeListWU = new List<EmployeeTO>();
                    List<int> employeeListID = new List<int>();
                    foreach (EmployeeTO employee in EmplList)
                    {
                        if (this.chbShowRetired.Checked || !employee.Status.Equals(Constants.statusRetired))
                        {
                            EmployeeListWU.Add(employee);
                            employeeListID.Add(employee.EmployeeID);
                        }
                    }

                    //int count = ioPair.SearchForWUCount(selectedWorkingUnit, wuString, dtpFromDate.Value, dtpToDate.Value);
                    int count = ioPair.SearchEmplDateCount(dtpFromDate.Value, dtpToDate.Value, employeeListID);
                    if (count > Constants.maxWUReportRecords)
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("moreRecordsThanAllowed", culture));
                        return;
                    }
                    else if (count > Constants.warningWUReportRecords)
                    {
                        this.Cursor = Cursors.Arrow;
                        DialogResult result = MessageBox.Show(rm.GetString("recordsForWarning", culture), "", MessageBoxButtons.YesNo);
                        if (result.Equals(DialogResult.No))
                        {
                            return;
                        }
                    }

                    this.Cursor = Cursors.WaitCursor;

                    // get all valid IO Pairs for selected working unit and time interval
                    //ioPairList = ioPair.SearchForWU(selectedWorkingUnit,wuString,dtpFromDate.Value,dtpToDate.Value);
                    ioPairList = ioPair.SearchAll(dtpFromDate.Value, dtpToDate.Value, employeeListID);

                    // get Time Schemas for selected Employee and selected Time Interval
                    date = dtpFromDate.Value.Date;

                    /*while ((date <= dtpToDate.Value) || (date.Month == dtpToDate.Value.Month))
                    {
                        foreach(Employee empl in EmployeeListWU)
                        {
                            timeSchedule = new EmployeesTimeSchedule().SearchMonthSchedule(empl.EmployeeID, date);
						
                            foreach (EmployeesTimeSchedule ets in timeSchedule)
                            {
                                timeScheduleList.Add(ets);
                            }
                        }
                        date = date.AddMonths(1);
                    }*/
                    string employeeIDString = "";
                    foreach (EmployeeTO empl in EmployeeListWU)
                    {
                        employeeIDString = employeeIDString + empl.EmployeeID.ToString() + ",";
                    }
                    if (employeeIDString.Length > 0)
                    {
                        employeeIDString = employeeIDString.Substring(0, employeeIDString.Length - 1);
                    }
                    timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(employeeIDString, dtpFromDate.Value.Date, dtpToDate.Value.Date);

                    // Kay is Date, Value is Time schema for that Date
                    Dictionary<DateTime, Dictionary<int, WorkTimeSchemaTO>> schemaForDay = new Dictionary<DateTime,Dictionary<int,WorkTimeSchemaTO>>();
                    
                    // Set Time Schema for every selected day
                    date = dtpFromDate.Value.Date;
                    while (date <= dtpToDate.Value.Date)
                    {
                        int timeScheduleIndex = -1;

                        for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
                        {
                            /* 2008-03-14
                             * From now one, take the last existing time schedule, don't expect that every month has 
                             * time schedule*/
                            if (date >= timeScheduleList[scheduleIndex].Date)
                                //&& (date.Month == ((EmployeesTimeSchedule)(timeScheduleList[scheduleIndex])).Date.Month))
                            {
                                timeScheduleIndex = scheduleIndex;

                                if (timeScheduleIndex >= 0)
                                {
                                    TimeSchema sch = new TimeSchema();
                                    sch.TimeSchemaTO.TimeSchemaID = timeScheduleList[timeScheduleIndex].TimeSchemaID;
                                    List<WorkTimeSchemaTO> schemas = sch.Search();
                                    if (!schemaForDay.ContainsKey(date))
                                    {
                                        schemaForDay.Add(date, new Dictionary<int,WorkTimeSchemaTO>());
                                    }

                                    if (!schemaForDay[date].ContainsKey(timeScheduleList[scheduleIndex].EmployeeID))
                                    {
                                        schemaForDay[date].Add(timeScheduleList[scheduleIndex].EmployeeID, schemas[0]);
                                    }
                                    else
                                    {
                                        schemaForDay[date][timeScheduleList[scheduleIndex].EmployeeID] = schemas[0];
                                    }
                                }
                            }
                        }

                        date = date.AddDays(1);
                    }

                    // Key is Pass Type Id, Value is Pass Type Description
                    Dictionary<int, string> passTypes = new Dictionary<int,string>();

                    List<PassTypeTO> passTypesAll = new PassType().Search();
                    foreach (PassTypeTO pt in passTypesAll)
                    {
                        passTypes.Add(pt.PassTypeID, pt.Description);
                    }

                    // Key is PassTypeID, Value is total time
                    Hashtable passTypesTotalTime = new Hashtable();

                    // Key is Date, Value is hours spent on job for that date
                    Hashtable job = new Hashtable();

                    // Key is Date, Value is late for that date
                    Hashtable late = new Hashtable();

                    // Key is Date, Value is earlier going away for that date
                    Hashtable early = new Hashtable();

                    // Key is Date, Value is overtime work for that date
                    Hashtable overtime = new Hashtable();

                    Dictionary<DateTime, IOPairTO> earlyestArrivedPair = new Dictionary<DateTime,IOPairTO>();

                    Dictionary<DateTime, IOPairTO> latestLeftPairs = new Dictionary<DateTime,IOPairTO>();


                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("employee_analytical");

                    tableCR.Columns.Add("date", typeof(System.DateTime));
                    tableCR.Columns.Add("time_schema", typeof(System.String));
                    tableCR.Columns.Add("last_name", typeof(System.String));
                    tableCR.Columns.Add("first_name", typeof(System.String));
                    tableCR.Columns.Add("late", typeof(int));
                    tableCR.Columns.Add("early", typeof(int));
                    tableCR.Columns.Add("total_time", typeof(int));
                    tableCR.Columns.Add("over_time", typeof(int));
                    tableCR.Columns.Add("need_validation", typeof(System.String));
                    tableCR.Columns.Add("working_unit", typeof(System.String));
                    tableCR.Columns.Add("employee_id", typeof(int));
                    tableCR.Columns.Add("imageID", typeof(byte));

                    DataTable tableI = new DataTable("images");
                    tableI.Columns.Add("image", typeof(System.Byte[]));
                    tableI.Columns.Add("imageID", typeof(byte));


                    // ******** REPORT ********
                    // all records for report
                    date = dtpFromDate.Value.Date;
                    ArrayList rowList = new ArrayList();
                    WorkTimeSchemaTO tsc = new WorkTimeSchemaTO();
                    // TOTALS
                    TimeSpan totalTime = new TimeSpan(0);
                    TimeSpan totalLate = new TimeSpan(0);
                    TimeSpan totalEarly = new TimeSpan(0);
                    TimeSpan totalWorkTime = new TimeSpan(0);
                    TimeSpan totalOverTime = new TimeSpan(0);

                    int counter = 0;
                    while (date <= dtpToDate.Value.Date)
                    {
                        foreach (EmployeeTO empl in EmployeeListWU)
                        {
                            bool hasPair = false;

                            job.Clear();
                            late.Clear();
                            early.Clear();
                            overtime.Clear();
                            earlyestArrivedPair.Clear();
                            latestLeftPairs.Clear();
                            passTypesTotalTime.Clear();
                            //schemaForDay.Clear();

                            foreach (IOPairTO iopairTO in ioPairList)
                            {
                                if ((iopairTO.IOPairDate == date) && (iopairTO.EmployeeID == empl.EmployeeID))
                                {
                                    hasPair = true;

                                    // Job
                                    totalTime = iopairTO.EndTime.Subtract(iopairTO.StartTime);

                                    if (job.ContainsKey(iopairTO.IOPairDate.Date))
                                    {
                                        job[iopairTO.IOPairDate.Date] = ((TimeSpan)job[iopairTO.IOPairDate.Date]).Add(totalTime);
                                    }
                                    else
                                    {
                                        job.Add(iopairTO.IOPairDate.Date, totalTime);
                                    }

                                    if (passTypesTotalTime.ContainsKey(iopairTO.PassTypeID))
                                    {
                                        passTypesTotalTime[iopairTO.PassTypeID] = ((TimeSpan)passTypesTotalTime[iopairTO.PassTypeID]).Add(totalTime);
                                    }
                                    else
                                    {
                                        passTypesTotalTime.Add(iopairTO.PassTypeID, totalTime);
                                    }

                                    // Late
                                    // Find IOPair with earliest arrival for particular date
                                    if (earlyestArrivedPair.ContainsKey(date))
                                    {
                                        if (earlyestArrivedPair[date].StartTime.TimeOfDay > iopairTO.StartTime.TimeOfDay)
                                        {
                                            earlyestArrivedPair[date] = iopairTO;
                                        }
                                    }
                                    else
                                    {
                                        earlyestArrivedPair.Add(date, iopairTO);
                                    }

                                    // Earlier going away for that date
                                    if (latestLeftPairs.ContainsKey(date))
                                    {
                                        if (latestLeftPairs[date].EndTime.TimeOfDay < iopairTO.EndTime.TimeOfDay)
                                        {
                                            latestLeftPairs[date] = iopairTO;
                                        }
                                    }
                                    else
                                    {
                                        latestLeftPairs.Add(date, iopairTO);
                                    }
                                }
                            }//foreach IO Pair

                            // Calculate Late
                            if (earlyestArrivedPair.ContainsKey(date))
                            {
                                if (!late.ContainsKey(date) && schemaForDay.ContainsKey(date) && schemaForDay[date].ContainsKey(empl.EmployeeID))
                                {
                                    WorkTimeIntervalTO currentInterval = getTimeSchemaInterval(empl.EmployeeID, date, timeScheduleList);
                                    if (((TimeSpan)currentInterval.EndTime.Subtract(currentInterval.StartTime)).TotalMinutes != 0)
                                    {
                                        IOPairTO earlyestReal = earlyestArrivedPair[date];

                                        if (earlyestReal.StartTime.TimeOfDay > currentInterval.LatestArrivaed.TimeOfDay)
                                        {
                                            late.Add(date, earlyestReal.StartTime.TimeOfDay - currentInterval.LatestArrivaed.TimeOfDay);
                                        }
                                    }
                                }
                            }

                            // Calculate earlier going away from job 
                            if (latestLeftPairs.ContainsKey(date))
                            {
                                if (!early.ContainsKey(date) && schemaForDay.ContainsKey(date) && schemaForDay[date].ContainsKey(empl.EmployeeID))
                                {
                                    WorkTimeIntervalTO currentInterval = getTimeSchemaInterval(empl.EmployeeID, date, timeScheduleList);
                                    IOPairTO latestReal = latestLeftPairs[date];

                                    WorkTimeSchemaTO currentSchema = schemaForDay[date][empl.EmployeeID];//ja
                                    if (currentSchema.Type.Trim() == Constants.schemaTypeFlexi) //ja
                                    {
                                        //flexy working time
                                        TimeSpan expectedDuarationL = currentInterval.EndTime.TimeOfDay - currentInterval.StartTime.TimeOfDay;
                                        DateTime expectedEndTime = new DateTime(earlyestArrivedPair[date].StartTime.Ticks);
                                        if (latestReal.EndTime.TimeOfDay < (earlyestArrivedPair[date].StartTime.TimeOfDay + expectedDuarationL))
                                        {
                                            early.Add(date, (earlyestArrivedPair[date].StartTime.TimeOfDay + expectedDuarationL) - latestReal.EndTime.TimeOfDay);
                                        }
                                    }
                                    else
                                    {
                                        //not flexy working time
                                        if (latestReal.EndTime.TimeOfDay < currentInterval.EarliestLeft.TimeOfDay)
                                        {
                                            early.Add(date, currentInterval.EarliestLeft.TimeOfDay - latestReal.EndTime.TimeOfDay);
                                        }
                                    }

                                }
                            }

                            // Overtime work for that date
                            TimeSpan expectedDuaration = new TimeSpan(0);
                            TimeSpan realTime = new TimeSpan(0);
                            TimeSpan dif = new TimeSpan(0);
                            if (job.ContainsKey(date))
                            {
                                WorkTimeIntervalTO currentInterval = getTimeSchemaInterval(empl.EmployeeID, date, timeScheduleList);

                                expectedDuaration = currentInterval.EndTime.TimeOfDay - currentInterval.StartTime.TimeOfDay;
                                realTime = (TimeSpan)job[date];
                                dif = realTime - expectedDuaration;

                                if (dif.TotalMinutes > 0)
                                {
                                    if (overtime.ContainsKey(date))
                                    {
                                        overtime[date] = dif;
                                    }
                                    else
                                    {
                                        overtime.Add(date, dif);
                                    }
                                }
                            }

                            // get all dates in selected interval which has open pairs
                            //ArrayList datesList = new IOPair().SearchDatesWithOpenPairs(dtpFromDate.Value, dtpToDate.Value,
                            List<DateTime> datesList = new IOPair().SearchDatesWithOpenPairsWrkHrs(dtpFromDate.Value, dtpToDate.Value,
                                empl.EmployeeID);

                            if ((schemaForDay.ContainsKey(date)&& schemaForDay[date].ContainsKey(empl.EmployeeID)) || late.ContainsKey(date) || early.ContainsKey(date) ||
                                job.ContainsKey(date) || overtime.ContainsKey(date))
                            {
                                if ((hasPair == true) || datesList.Contains(date))
                                {
                                    DataRow rowCR = tableCR.NewRow();
                                    // One record in table

                                    ArrayList row = new ArrayList();

                                    rowCR["last_name"] = empl.LastName;
                                    rowCR["first_name"] = empl.FirstName;
                                    rowCR["working_unit"] = empl.WorkingUnitName;
                                    rowCR["employee_id"] = empl.EmployeeID;
                                    // Date
                                    row.Add(date.ToString("dd.MM.yyy"));
                                    rowCR["date"] = date;

                                    // Work Time Shema Description
                                    if (schemaForDay.ContainsKey(date) && schemaForDay[date].ContainsKey(empl.EmployeeID))
                                    {
                                        tsc = schemaForDay[date][empl.EmployeeID];
                                    }
                                    else
                                    {
                                        tsc = new WorkTimeSchemaTO();
                                        tsc.Description = rm.GetString("noTimeSchema", culture);
                                    }

                                    row.Add(tsc.Description.ToString());
                                    rowCR["time_schema"] = tsc.Description.ToString();

                                    // Late
                                    if (late.ContainsKey(date))
                                    {
                                        totalTime = ((TimeSpan)late[date]);
                                        row.Add(totalTime.Hours.ToString() + "h " + totalTime.Minutes + "min");
                                        totalLate = totalLate.Add(totalTime);
                                        rowCR["late"] = totalTime.TotalMinutes;
                                    }
                                    else
                                    {
                                        row.Add("0h 0min");
                                        rowCR["late"] = 0;
                                    }

                                    // Early
                                    if (early.ContainsKey(date))
                                    {
                                        totalTime = ((TimeSpan)early[date]);
                                        row.Add(totalTime.Hours.ToString() + "h " + totalTime.Minutes + "min");
                                        totalEarly = totalEarly.Add(totalTime);
                                        rowCR["early"] = totalTime.TotalMinutes;
                                    }
                                    else
                                    {
                                        row.Add("0h 0min");
                                        rowCR["early"] = 0;
                                    }

                                    // Total working hours for a day
                                    if (job.ContainsKey(date))
                                    {
                                        totalTime = ((TimeSpan)job[date]);
                                        row.Add(totalTime.Hours.ToString() + "h " + totalTime.Minutes + "min");
                                        totalWorkTime = totalWorkTime.Add(totalTime);
                                        rowCR["total_time"] = totalTime.TotalMinutes;
                                    }
                                    else
                                    {
                                        row.Add("0h 0min");
                                        rowCR["total_time"] = 0;
                                    }

                                    // Overtime work for that date
                                    if (overtime.ContainsKey(date))
                                    {
                                        totalTime = ((TimeSpan)overtime[date]);
                                        row.Add(totalTime.Hours.ToString() + "h " + totalTime.Minutes + "min");
                                        totalOverTime = totalOverTime.Add(totalTime);
                                        rowCR["over_time"] = totalTime.TotalMinutes;
                                    }
                                    else
                                    {
                                        row.Add("0h 0min");
                                        rowCR["over_time"] = 0;
                                    }

                                    // Note	
                                    if (datesList.Contains(date))
                                    {
                                        row.Add("X");
                                        rowCR["need_validation"] = "X";
                                    }
                                    else
                                    {
                                        row.Add("");
                                        rowCR["need_validation"] = "";
                                    }

                                    rowCR["imageID"] = 1;
                                    if (counter == 0)
                                    {
                                        //add logo image just once
                                        DataRow rowI = tableI.NewRow();
                                        rowI["image"] = Constants.LogoForReport;
                                        rowI["imageID"] = 1;
                                        tableI.Rows.Add(rowI);
                                        tableI.AcceptChanges();
                                    }

                                    rowList.Add(row);
                                    tableCR.Rows.Add(rowCR);
                                    counter++;
                                }
                            }
                        }//foreach(Employee empl in EmployeeListWU)
                        date = date.AddDays(1);
                    }

                    dataSetCR.Tables.Add(tableCR);
                    dataSetCR.Tables.Add(tableI);

                    // Claculate Totals
                    ArrayList totalsRowList = new ArrayList();

                    // One record in Total's table
                    ArrayList rowTotal = new ArrayList();

                    rowTotal.Add("Ukupno:");
                    rowTotal.Add("");

                    rowTotal.Add((totalLate.Hours + (totalLate.Days * 24)).ToString() + "h " + totalLate.Minutes + "min");

                    rowTotal.Add((totalEarly.Hours + (totalEarly.Days * 24)).ToString() + "h " + totalEarly.Minutes + "min");
                    rowTotal.Add((totalWorkTime.Hours + (totalWorkTime.Days * 24)).ToString() + "h " + totalWorkTime.Minutes + "min");
                    rowTotal.Add((totalOverTime.Hours + (totalOverTime.Days * 24)).ToString() + "h " + totalOverTime.Minutes + "min");
                    rowTotal.Add("");

                    totalsRowList.Add(rowTotal);

                    // Calculate Totals by Pass Type
                    ArrayList ptTotalsRowList = new ArrayList();

                    foreach (int ptID in passTypes.Keys)
                    {
                        if (passTypesTotalTime.ContainsKey(ptID))
                        {
                            ptTotalsRowList.Add(passTypes[ptID].ToString().Trim() + ": "
                                + (((TimeSpan)passTypesTotalTime[ptID]).Hours + (((TimeSpan)passTypesTotalTime[ptID]).Days * 24)).ToString() + "h "
                                + ((TimeSpan)passTypesTotalTime[ptID]).Minutes + "min");
                        }
                    }
                    if (this.cbCR.Checked)
                    {
                        if (ioPairList.Count == 0)
                        {
                            this.Cursor = Cursors.Arrow;
                            MessageBox.Show(rm.GetString("dataNotFound", culture));
                            return;
                        }
                        else
                        {
                            this.generateAnalyticalWUCRReport(dataSetCR);
                        }

                    }

                    //this.Close();
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " EmployeeAnalyticalWU.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void generateAnalyticalWUCRReport(DataSet dataCR)
        {
            try
            {
                DataTable table = dataCR.Tables["employee_analytical"];
                
                if (table.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("dataNotFound", culture));
                    return;
                }
                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                {
                    Reports_sr.EmployeeAnalyticalWUView view = new Reports_sr.EmployeeAnalyticalWUView(
                        dataCR,cbWorkingUnit.Text.ToString(), dtpFromDate.Value, dtpToDate.Value);
                    view.ShowDialog(this);
                }
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                {
                    Reports_en.EmployeeAnalyticalWUView_en view = new Reports_en.EmployeeAnalyticalWUView_en(
                        dataCR, cbWorkingUnit.Text.ToString(), dtpFromDate.Value, dtpToDate.Value);
                    view.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " EmployeeAnalyticWU.generateAnalyticalWUCRReport(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void EmployeeAnalyticalWU_KeyUp(object sender, KeyEventArgs e)
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
                MessageBox.Show(ex.Message);
            }
        }

        private void EmployeeAnalyticalWU_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                filter = new Filter();
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

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
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
            finally { this.Cursor = Cursors.Arrow; }
        }
    }
}

