using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;

using Common;
using TransferObjects;
using Util;

namespace Reports.DSF
{
    public partial class DSFPresenceReport : Form
    {
        CultureInfo culture;
        ResourceManager rm;
        ApplUserTO logInUser;

        DebugLog debug;

        // Working Units that logInUser is granted to
        List<WorkingUnitTO> wUnits;
        string wuString;

        Filter filter;

        private ListViewItemComparer _comp;

        const int EmplIDIndex = 0;
        const int EmplFirstNameIndex = 1;
        const int EmplLastNameIndex = 2;

        public DSFPresenceReport()
        {
            try
            {
                InitializeComponent();

                // Init debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                debug = new DebugLog(logFilePath);

                // Language tool
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(DSFPresenceReport).Assembly);
                setLanguage();

                logInUser = NotificationController.GetLogInUser();

                dtpFromDate.Value = DateTime.Now.Date;
                dtpToDate.Value = DateTime.Now.Date;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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
                get { return _listView; }
            }

            private int _sortColumn = 0;

            public int SortColumn
            {
                get { return _sortColumn; }
                set { _sortColumn = value; }
            }

            public int Compare(object a, object b)
            {
                ListViewItem item1 = (ListViewItem)a;
                ListViewItem item2 = (ListViewItem)b;

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
                    case DSFPresenceReport.EmplIDIndex:
                            return CaseInsensitiveComparer.Default.Compare(Int32.Parse(sub1.Text.Trim()), Int32.Parse(sub2.Text.Trim()));
                    case DSFPresenceReport.EmplFirstNameIndex:
                    case DSFPresenceReport.EmplLastNameIndex:
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);                        
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");
                }
            }
        }

        #endregion

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("PresenceReports", culture);

                gbEmployees.Text = rm.GetString("gbEmployees", culture);
                gbPeriod.Text = rm.GetString("gbPeriod", culture);
                gbReportType.Text = rm.GetString("gbReportType", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);

                chbSelectAll.Text = rm.GetString("chbSelectAll", culture);
                
                lblFrom.Text = rm.GetString("lblFrom", culture);                
                lblTo.Text = rm.GetString("lblTo", culture);
                
                btnGenerateReport.Text = rm.GetString("btnGenerateReport", culture);
                btnSaveCriteria.Text = rm.GetString("btnSave", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                
                rbAnalytical.Text = rm.GetString("analitycal", culture);
                rbSummary.Text = rm.GetString("summary", culture);

                // list view initialization
                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrEmployeeID", culture), lvEmployees.Width / 3 - 8, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), lvEmployees.Width / 3 - 8, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrLastName", culture), lvEmployees.Width / 3 - 8, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " DSFPresenceReport.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateEmployees(string wuString)
        {
            try
            {
                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();

                List<EmployeeTO> employeesList = new List<EmployeeTO>();

                if (!wuString.Trim().Equals(""))
                    employeesList = new Employee().SearchByWU(wuString);

                if (employeesList.Count > 0)
                {
                    for (int i = 0; i < employeesList.Count; i++)
                    {
                        EmployeeTO employee = employeesList[i];
                        ListViewItem item = new ListViewItem();

                        item.Text = employee.EmployeeID.ToString().Trim();
                        item.SubItems.Add(employee.FirstName.Trim());
                        item.SubItems.Add(employee.LastName.Trim());
                        item.Tag = employee;
                        lvEmployees.Items.Add(item);
                    }
                }                

                lvEmployees.EndUpdate();
                lvEmployees.Invalidate();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " DSFPresenceReport.populateEmployees(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DSFPresenceReport_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Initialize comparer object
                _comp = new ListViewItemComparer(lvEmployees);
                lvEmployees.ListViewItemSorter = _comp;
                lvEmployees.Sorting = SortOrder.Ascending;

                wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                wuString = "";
                foreach (WorkingUnitTO wu in wUnits)
                {
                    wuString += wu.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                populateEmployees(wuString);

                // load filter
                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));           
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " DSFPresenceReport.DSFPresenceReport_Load(): " + ex.Message + "\n");
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
                debug.writeLog(DateTime.Now + " DSFPresenceReport.btnSaveCriteria_Click(): " + ex.Message + "\n");
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
                debug.writeLog(DateTime.Now + " DSFPresenceReport.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem item in lvEmployees.Items)
                {
                    item.Selected = chbSelectAll.Checked;
                }

                lvEmployees.Invalidate();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " DSFPresenceReport.chbSelectAll_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvEmployees_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvEmployees.Sorting;
                lvEmployees.Sorting = SortOrder.None;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvEmployees.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvEmployees.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvEmployees.Sorting = SortOrder.Ascending;
                }

                lvEmployees.ListViewItemSorter = _comp;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " DSFPresenceReport.lvEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                                
                string employeesID = "";
                List<int> emplIDs = new List<int>();
                Dictionary<int, EmployeeTO> employeesDic = new Dictionary<int,EmployeeTO>();

                if (lvEmployees.Items.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noEmplReportPrivilege", culture));
                    return;
                }

                if (lvEmployees.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noEmplSelectedForRep", culture));
                    return;
                }

                if (dtpToDate.Value.Date < dtpFromDate.Value.Date)
                {
                    MessageBox.Show(rm.GetString("wrongDatePickUp", culture));
                    return;
                }

                foreach (ListViewItem item in lvEmployees.SelectedItems)
                {                    
                    employeesID += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";

                    if (!emplIDs.Contains(((EmployeeTO)item.Tag).EmployeeID))
                        emplIDs.Add(((EmployeeTO)item.Tag).EmployeeID);

                    if (!employeesDic.ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                        employeesDic.Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);
                }

                if (employeesID.Length > 0)
                    employeesID = employeesID.Substring(0, employeesID.Length - 1);
                
                Dictionary<int, Dictionary<DateTime, List<IOPairTO>>> employeesPairs = new Dictionary<int,Dictionary<DateTime,List<IOPairTO>>>();
                Dictionary<int, Dictionary<DateTime, PassTO>> employeesFirstIn = new Dictionary<int, Dictionary<DateTime, PassTO>>();
                Dictionary<int, Dictionary<DateTime, PassTO>> employeesLastOut = new Dictionary<int, Dictionary<DateTime, PassTO>>();
                Dictionary<int, Dictionary<DateTime, int>> regularWork = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> hole = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> overtime = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> outOfWSWork = new Dictionary<int, Dictionary<DateTime, int>>();

                Dictionary<int, Dictionary<DateTime, List<PassTO>>> employeesPassesPeriod = new Pass().SearchPassesForEmployeesPeriod(employeesID, dtpFromDate.Value.Date, dtpToDate.Value.Date);

                // get first IN and last OUT for each employee for each day of selected employees and period
                PassTO firstIN = new PassTO();
                PassTO lastOUT = new PassTO();
                foreach (int emplID in employeesPassesPeriod.Keys)
                {
                    foreach (DateTime date in employeesPassesPeriod[emplID].Keys)
                    {
                        firstIN = new PassTO();
                        lastOUT = new PassTO();

                        foreach (PassTO pass in employeesPassesPeriod[emplID][date])
                        {
                            if (pass.Direction.Trim().ToUpper().Equals(Constants.DirectionIn.Trim().ToUpper()) && firstIN.PassID == -1)
                                firstIN = pass;

                            if (pass.Direction.Trim().ToUpper().Equals(Constants.DirectionOut.Trim().ToUpper()))
                                lastOUT = pass;
                        }

                        if (firstIN.PassID != -1)
                        {
                            if (!employeesFirstIn.ContainsKey(emplID))
                                employeesFirstIn.Add(emplID, new Dictionary<DateTime, PassTO>());

                            if (!employeesFirstIn[emplID].ContainsKey(date))
                                employeesFirstIn[emplID].Add(date, firstIN);
                            else
                                employeesFirstIn[emplID][date] = firstIN;
                        }

                        if (lastOUT.PassID != -1)
                        {
                            if (!employeesLastOut.ContainsKey(emplID))
                                employeesLastOut.Add(emplID, new Dictionary<DateTime, PassTO>());

                            if (!employeesLastOut[emplID].ContainsKey(date))
                                employeesLastOut[emplID].Add(date, lastOUT);
                            else
                                employeesLastOut[emplID][date] = lastOUT;
                        }
                    }
                }

                List<IOPairTO> pairs = new IOPair().SearchAll(dtpFromDate.Value.Date, dtpToDate.Value.Date, emplIDs);
                                
                // Key is employee ID value is list of valid pairs for that employee by date
                Dictionary<int, Dictionary<DateTime, List<IOPairTO>>> emplPairs = new Dictionary<int,Dictionary<DateTime,List<IOPairTO>>>();                

                foreach (IOPairTO pair in pairs)
                {
                    if (!emplPairs.ContainsKey(pair.EmployeeID))
                        emplPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairTO>>());

                    if (!emplPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        emplPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairTO>());

                    emplPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                }

                // get all employee schedules
                List<EmployeeTimeScheduleTO> timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(employeesID.Trim(), dtpFromDate.Value.Date, dtpToDate.Value.Date);
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new Dictionary<int, List<EmployeeTimeScheduleTO>>();

                foreach (EmployeeTimeScheduleTO schedule in timeScheduleList)
                {
                    if (!emplSchedules.ContainsKey(schedule.EmployeeID))
                        emplSchedules.Add(schedule.EmployeeID, new List<EmployeeTimeScheduleTO>());

                    emplSchedules[schedule.EmployeeID].Add(schedule);
                }
                                
                // get all time schemas
                List<WorkTimeSchemaTO> schemas = new TimeSchema().Search();

                DateTime currentDate = dtpFromDate.Value.Date;
                foreach (int emplID in employeesDic.Keys)
                {
                    currentDate = dtpFromDate.Value.Date;
                    while (currentDate.Date <= dtpToDate.Value.Date)
                    {
                        int workMinutes = 0;
                        int holeMinutes = 0;
                        int overtimeMinutes = 0;
                        int workOutOfWSMinutes = 0;

                        // get intervals from that days
                        List<EmployeeTimeScheduleTO> scheduleList = new List<EmployeeTimeScheduleTO>();
                        if (emplSchedules.ContainsKey(emplID))
                            scheduleList = emplSchedules[emplID];

                        List<WorkTimeIntervalTO> emplIntervals = getTimeSchemaInterval(emplID, currentDate, scheduleList, schemas);

                        // get pairs for that day
                        List<IOPairTO> emplDayPairs = new List<IOPairTO>();

                        if (emplPairs.ContainsKey(emplID) && emplPairs[emplID].ContainsKey(currentDate.Date))
                            emplDayPairs = emplPairs[emplID][currentDate.Date];

                        // calculate work out of ws
                        if ((emplIntervals.Count == 0 || (emplIntervals.Count == 1 && emplIntervals[0].EndTime.TimeOfDay.Subtract(emplIntervals[0].StartTime.TimeOfDay).TotalMinutes == 0))
                            && emplDayPairs.Count > 0)
                        {
                            foreach (IOPairTO pair in emplDayPairs)
                            {
                                if (pair.PassTypeID == Constants.regularWork)
                                {
                                    int pairDuration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                                    if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                                        pairDuration++;

                                    workOutOfWSMinutes += pairDuration;
                                }
                            }
                        }
                        else
                        {
                            // calculate regular work and overtime
                            List<IOPairTO> regularPairs = new List<IOPairTO>();
                            List<IOPairTO> overtimePairs = new List<IOPairTO>();
                            IOPairTO overtimePair = new IOPairTO();
                            IOPairTO regularPair = new IOPairTO();
                            foreach (IOPairTO pair in emplDayPairs)
                            {
                                foreach (WorkTimeIntervalTO interval in emplIntervals)
                                {
                                    if (pair.StartTime.TimeOfDay < interval.StartTime.TimeOfDay)
                                    {
                                        if (pair.EndTime.TimeOfDay <= interval.StartTime.TimeOfDay)
                                            overtimePairs.Add(pair);
                                        else if (pair.EndTime.TimeOfDay <= interval.EndTime.TimeOfDay)
                                        {
                                            overtimePair = new IOPairTO();
                                            overtimePair.IOPairDate = pair.IOPairDate.Date;
                                            overtimePair.StartTime = pair.StartTime;
                                            overtimePair.EndTime = new DateTime(overtimePair.IOPairDate.Year, overtimePair.IOPairDate.Month, overtimePair.IOPairDate.Day, interval.StartTime.Hour, interval.StartTime.Minute, 0);
                                            overtimePairs.Add(overtimePair);

                                            regularPair = new IOPairTO();
                                            regularPair.PassTypeID = pair.PassTypeID;
                                            regularPair.IOPairDate = pair.IOPairDate.Date;
                                            regularPair.EndTime = pair.EndTime;
                                            regularPair.StartTime = new DateTime(regularPair.IOPairDate.Year, regularPair.IOPairDate.Month, regularPair.IOPairDate.Day, interval.StartTime.Hour, interval.StartTime.Minute, 0);
                                            regularPairs.Add(regularPair);
                                        }
                                        else
                                        {
                                            overtimePair = new IOPairTO();
                                            overtimePair.IOPairDate = pair.IOPairDate.Date;
                                            overtimePair.StartTime = pair.StartTime;
                                            overtimePair.EndTime = new DateTime(overtimePair.IOPairDate.Year, overtimePair.IOPairDate.Month, overtimePair.IOPairDate.Day, interval.StartTime.Hour, interval.StartTime.Minute, 0);
                                            overtimePairs.Add(overtimePair);

                                            regularPair = new IOPairTO();
                                            regularPair.PassTypeID = pair.PassTypeID;
                                            regularPair.IOPairDate = pair.IOPairDate.Date;
                                            regularPair.StartTime = new DateTime(regularPair.IOPairDate.Year, regularPair.IOPairDate.Month, regularPair.IOPairDate.Day, interval.StartTime.Hour, interval.StartTime.Minute, 0);
                                            regularPair.EndTime = new DateTime(regularPair.IOPairDate.Year, regularPair.IOPairDate.Month, regularPair.IOPairDate.Day, interval.EndTime.Hour, interval.EndTime.Minute, 0);
                                            regularPairs.Add(regularPair);

                                            overtimePair = new IOPairTO();
                                            overtimePair.IOPairDate = pair.IOPairDate.Date;
                                            overtimePair.EndTime = pair.EndTime;
                                            overtimePair.StartTime = new DateTime(overtimePair.IOPairDate.Year, overtimePair.IOPairDate.Month, overtimePair.IOPairDate.Day, interval.EndTime.Hour, interval.EndTime.Minute, 0);
                                            overtimePairs.Add(overtimePair);
                                        }
                                    }
                                    else if (pair.StartTime.TimeOfDay < interval.EndTime.TimeOfDay)
                                    {
                                        if (pair.EndTime.TimeOfDay <= interval.EndTime.TimeOfDay)
                                            regularPairs.Add(pair);
                                        else
                                        {
                                            regularPair = new IOPairTO();
                                            regularPair.PassTypeID = pair.PassTypeID;
                                            regularPair.IOPairDate = pair.IOPairDate.Date;
                                            regularPair.StartTime = pair.StartTime;
                                            regularPair.EndTime = new DateTime(regularPair.IOPairDate.Year, regularPair.IOPairDate.Month, regularPair.IOPairDate.Day, interval.EndTime.Hour, interval.EndTime.Minute, 0);
                                            regularPairs.Add(regularPair);

                                            overtimePair = new IOPairTO();
                                            overtimePair.IOPairDate = pair.IOPairDate.Date;
                                            overtimePair.EndTime = pair.EndTime;
                                            overtimePair.StartTime = new DateTime(overtimePair.IOPairDate.Year, overtimePair.IOPairDate.Month, overtimePair.IOPairDate.Day, interval.EndTime.Hour, interval.EndTime.Minute, 0);
                                            overtimePairs.Add(overtimePair);
                                        }
                                    }
                                    else
                                        overtimePairs.Add(pair);
                                }
                            }

                            // calculate regular work
                            foreach (IOPairTO regPair in regularPairs)
                            {
                                if (regPair.PassTypeID == Constants.regularWork)
                                    workMinutes += (int)regPair.EndTime.Subtract(regPair.StartTime).TotalMinutes;
                            }

                            // calculate overtime work
                            foreach (IOPairTO oPair in overtimePairs)
                            {
                                overtimeMinutes += (int)oPair.EndTime.Subtract(oPair.StartTime).TotalMinutes;
                            }

                            foreach (WorkTimeIntervalTO interval in emplIntervals)
                            {
                                List<IOPairTO> intervalPairs = new List<IOPairTO>();
                                foreach (IOPairTO regPair in regularPairs)
                                {
                                    if (regPair.StartTime.TimeOfDay >= interval.StartTime.TimeOfDay && regPair.EndTime.TimeOfDay <= interval.EndTime.TimeOfDay)
                                        intervalPairs.Add(regPair);
                                }

                                // calculate holes
                                if (intervalPairs.Count <= 0)
                                    holeMinutes += (int)interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes;
                                else
                                {
                                    // add first hole
                                    holeMinutes += (int)intervalPairs[0].StartTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes;

                                    // add last hole
                                    holeMinutes += (int)interval.EndTime.TimeOfDay.Subtract(intervalPairs[intervalPairs.Count - 1].EndTime.TimeOfDay).TotalMinutes;

                                    // add holes between pairs
                                    int i = 1;
                                    while (i < intervalPairs.Count)
                                    {
                                        if (!intervalPairs[i].StartTime.Equals(intervalPairs[i - 1].EndTime))
                                            holeMinutes += (int)intervalPairs[i].StartTime.Subtract(intervalPairs[i - 1].EndTime).TotalMinutes;

                                        i++;
                                    }
                                }
                            }
                        }

                        if (workMinutes > 0)
                        {
                            if (!regularWork.ContainsKey(emplID))
                                regularWork.Add(emplID, new Dictionary<DateTime, int>());
                            if (!regularWork[emplID].ContainsKey(currentDate.Date))
                                regularWork[emplID].Add(currentDate.Date, workMinutes);
                            else
                                regularWork[emplID][currentDate.Date] = workMinutes;
                        }

                        if (overtimeMinutes > 0)
                        {
                            if (!overtime.ContainsKey(emplID))
                                overtime.Add(emplID, new Dictionary<DateTime, int>());
                            if (!overtime[emplID].ContainsKey(currentDate.Date))
                                overtime[emplID].Add(currentDate.Date, overtimeMinutes);
                            else
                                overtime[emplID][currentDate.Date] = overtimeMinutes;
                        }

                        if (holeMinutes > 0)
                        {
                            if (!hole.ContainsKey(emplID))
                                hole.Add(emplID, new Dictionary<DateTime, int>());
                            if (!hole[emplID].ContainsKey(currentDate.Date))
                                hole[emplID].Add(currentDate.Date, holeMinutes);
                            else
                                hole[emplID][currentDate.Date] = holeMinutes;
                        }

                        if (workOutOfWSMinutes > 0)
                        {
                            if (!outOfWSWork.ContainsKey(emplID))
                                outOfWSWork.Add(emplID, new Dictionary<DateTime,int>());
                            if (!outOfWSWork[emplID].ContainsKey(currentDate.Date))
                                outOfWSWork[emplID].Add(currentDate.Date, workOutOfWSMinutes);
                            else
                                outOfWSWork[emplID][currentDate.Date] = workOutOfWSMinutes;
                        }

                        currentDate = currentDate.AddDays(1);
                    }                    
                }

                // if there are no data for report
                if (employeesFirstIn.Count == 0 && employeesLastOut.Count == 0 && regularWork.Count == 0 && hole.Count == 0 && overtime.Count == 0 && outOfWSWork.Count == 0)
                { 
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }

                // generate report
                if (rbAnalytical.Checked)
                    generateAnalyticalReport(employeesDic, employeesFirstIn, employeesLastOut, regularWork, hole, overtime, outOfWSWork);
                else
                    generateSummaryReport(employeesDic, employeesFirstIn, employeesLastOut, regularWork, hole, overtime, outOfWSWork);

                this.Close();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " DSFPresenceReport.btnGenerateReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void generateAnalyticalReport(Dictionary<int, EmployeeTO> employeesDic, Dictionary<int, Dictionary<DateTime, PassTO>> employeesFirstIn, 
            Dictionary<int, Dictionary<DateTime, PassTO>> employeesLastOut, Dictionary<int, Dictionary<DateTime, int>> regularWork, Dictionary<int, Dictionary<DateTime, int>> hole,
            Dictionary<int, Dictionary<DateTime, int>> overtime, Dictionary<int, Dictionary<DateTime, int>> outOfWSWork)
        {
            try
            {
                DataSet dataSet = new DataSet();
                DataTable table = new DataTable("presence_rep");

                table.Columns.Add("date", typeof(System.String));
                table.Columns.Add("employee_id", typeof(System.String));
                table.Columns.Add("name", typeof(System.String));
                table.Columns.Add("regular_work", typeof(System.String));
                table.Columns.Add("pause", typeof(System.String));
                table.Columns.Add("overtime", typeof(System.String));
                table.Columns.Add("work_out_of_ws", typeof(System.String));
                table.Columns.Add("first_in", typeof(System.String));
                table.Columns.Add("last_out", typeof(System.String));
                table.Columns.Add("imageID", typeof(byte));

                DataTable tableI = new DataTable("images");
                tableI.Columns.Add("image", typeof(System.Byte[]));
                tableI.Columns.Add("imageID", typeof(byte));

                //add logo image just once
                DataRow rowI = tableI.NewRow();
                rowI["image"] = Constants.LogoForReport;
                rowI["imageID"] = 1;
                tableI.Rows.Add(rowI);
                tableI.AcceptChanges();

                DataRow row;

                int totalMinutes = 0;
                int hours = 0;
                int minutes = 0;
                foreach (int emplID in employeesDic.Keys)
                {
                    DateTime currDate = dtpFromDate.Value.Date;
                    while (currDate.Date <= dtpToDate.Value.Date)
                    {
                        row = table.NewRow();

                        row["date"] = currDate.Date.ToString(Constants.dateFormat);
                        row["employee_id"] = emplID.ToString().Trim();
                        row["name"] = employeesDic[emplID].FirstAndLastName;

                        if (employeesFirstIn.ContainsKey(emplID) && employeesFirstIn[emplID].ContainsKey(currDate.Date))                        
                            row["first_in"] = employeesFirstIn[emplID][currDate.Date].EventTime.ToString(Constants.timeFormat);                        
                        else
                            row["first_in"] = "";

                        if (employeesLastOut.ContainsKey(emplID) && employeesLastOut[emplID].ContainsKey(currDate.Date))
                            row["last_out"] = employeesLastOut[emplID][currDate.Date].EventTime.ToString(Constants.timeFormat);
                        else
                            row["last_out"] = "";

                        if (regularWork.ContainsKey(emplID) && regularWork[emplID].ContainsKey(currDate.Date))
                        {
                            totalMinutes = regularWork[emplID][currDate.Date];                            

                            hours = totalMinutes / 60;
                            minutes = totalMinutes % 60;

                            row["regular_work"] = hours.ToString().Trim().PadLeft(2, '0') + "h" + minutes.ToString().Trim().PadLeft(2, '0') + "min";
                        }
                        else
                            row["regular_work"] = "00h00min";

                        if (hole.ContainsKey(emplID) && hole[emplID].ContainsKey(currDate.Date))
                        {
                            totalMinutes = hole[emplID][currDate.Date];

                            hours = totalMinutes / 60;
                            minutes = totalMinutes % 60;

                            row["pause"] = hours.ToString().Trim().PadLeft(2, '0') + "h" + minutes.ToString().Trim().PadLeft(2, '0') + "min";
                        }
                        else
                            row["pause"] = "00h00min";

                        if (overtime.ContainsKey(emplID) && overtime[emplID].ContainsKey(currDate.Date))
                        {
                            totalMinutes = overtime[emplID][currDate.Date];

                            hours = totalMinutes / 60;
                            minutes = totalMinutes % 60;

                            row["overtime"] = hours.ToString().Trim().PadLeft(2, '0') + "h" + minutes.ToString().Trim().PadLeft(2, '0') + "min";
                        }
                        else
                            row["overtime"] = "00h00min";

                        if (outOfWSWork.ContainsKey(emplID) && outOfWSWork[emplID].ContainsKey(currDate.Date))
                        {
                            totalMinutes = outOfWSWork[emplID][currDate.Date];

                            hours = totalMinutes / 60;
                            minutes = totalMinutes % 60;

                            row["work_out_of_ws"] = hours.ToString().Trim().PadLeft(2, '0') + "h" + minutes.ToString().Trim().PadLeft(2, '0') + "min";
                        }
                        else
                            row["work_out_of_ws"] = "00h00min";

                        row["imageID"] = 1;

                        table.Rows.Add(row);

                        currDate = currDate.AddDays(1);
                    }                    
                }

                table.AcceptChanges();
                dataSet.Tables.Add(table);
                dataSet.Tables.Add(tableI);

                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                {
                    DSF_sr.PresenceReportAnalyticalCRView_sr view = new Reports.DSF.DSF_sr.PresenceReportAnalyticalCRView_sr(dataSet, dtpFromDate.Value.Date, dtpToDate.Value.Date);

                    view.ShowDialog(this);
                }
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                {
                    DSF_en.PresenceReportAnalyticalCRView_en view = new Reports.DSF.DSF_en.PresenceReportAnalyticalCRView_en(dataSet, dtpFromDate.Value.Date, dtpToDate.Value.Date);

                    view.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " DSFPresenceReport.generateAnalyticalReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void generateSummaryReport(Dictionary<int, EmployeeTO> employeesDic, Dictionary<int, Dictionary<DateTime, PassTO>> employeesFirstIn,
            Dictionary<int, Dictionary<DateTime, PassTO>> employeesLastOut, Dictionary<int, Dictionary<DateTime, int>> regularWork, Dictionary<int, Dictionary<DateTime, int>> hole,
            Dictionary<int, Dictionary<DateTime, int>> overtime, Dictionary<int, Dictionary<DateTime, int>> outOfWSWork)
        {
            try
            {
                DataSet dataSet = new DataSet();
                DataTable table = new DataTable("presence_rep");

                table.Columns.Add("date", typeof(System.String));
                table.Columns.Add("employee_id", typeof(System.String));
                table.Columns.Add("name", typeof(System.String));                
                table.Columns.Add("regular_work", typeof(System.String));
                table.Columns.Add("pause", typeof(System.String));
                table.Columns.Add("overtime", typeof(System.String));
                table.Columns.Add("work_out_of_ws", typeof(System.String));
                table.Columns.Add("first_in", typeof(System.String));
                table.Columns.Add("last_out", typeof(System.String));
                table.Columns.Add("imageID", typeof(byte));

                DataTable tableI = new DataTable("images");
                tableI.Columns.Add("image", typeof(System.Byte[]));
                tableI.Columns.Add("imageID", typeof(byte));
                                
                //add logo image just once
                DataRow rowI = tableI.NewRow();
                rowI["image"] = Constants.LogoForReport;
                rowI["imageID"] = 1;
                tableI.Rows.Add(rowI);
                tableI.AcceptChanges();                

                DataRow row;

                int totalMinutes = 0;
                int hours = 0;
                int minutes = 0;
                foreach (int emplID in employeesDic.Keys)
                {
                    row = table.NewRow();

                    row["employee_id"] = emplID.ToString().Trim();
                    row["name"] = employeesDic[emplID].FirstAndLastName;

                    if (regularWork.ContainsKey(emplID))
                    {
                        totalMinutes = 0;
                        foreach (DateTime date in regularWork[emplID].Keys)
                        {
                            totalMinutes += regularWork[emplID][date];
                        }

                        hours = totalMinutes / 60;
                        minutes = totalMinutes % 60;

                        row["regular_work"] = hours.ToString().Trim().PadLeft(2, '0') + "h" + minutes.ToString().Trim().PadLeft(2, '0') + "min";
                    }
                    else
                        row["regular_work"] = "00h00min";

                    if (hole.ContainsKey(emplID))
                    {
                        totalMinutes = 0;
                        foreach (DateTime date in hole[emplID].Keys)
                        {
                            totalMinutes += hole[emplID][date];
                        }

                        hours = totalMinutes / 60;
                        minutes = totalMinutes % 60;

                        row["pause"] = hours.ToString().Trim().PadLeft(2, '0') + "h" + minutes.ToString().Trim().PadLeft(2, '0') + "min";
                    }
                    else
                        row["pause"] = "00h00min";

                    if (overtime.ContainsKey(emplID))
                    {
                        totalMinutes = 0;
                        foreach (DateTime date in overtime[emplID].Keys)
                        {
                            totalMinutes += overtime[emplID][date];
                        }

                        hours = totalMinutes / 60;
                        minutes = totalMinutes % 60;

                        row["overtime"] = hours.ToString().Trim().PadLeft(2, '0') + "h" + minutes.ToString().Trim().PadLeft(2, '0') + "min";
                    }
                    else
                        row["overtime"] = "00h00min";

                    if (outOfWSWork.ContainsKey(emplID))
                    {
                        totalMinutes = 0;
                        foreach (DateTime date in outOfWSWork[emplID].Keys)
                        {
                            totalMinutes += outOfWSWork[emplID][date];
                        }

                        hours = totalMinutes / 60;
                        minutes = totalMinutes % 60;

                        row["work_out_of_ws"] = hours.ToString().Trim().PadLeft(2, '0') + "h" + minutes.ToString().Trim().PadLeft(2, '0') + "min";
                    }
                    else
                        row["work_out_of_ws"] = "00h00min";

                    row["imageID"] = 1;

                    table.Rows.Add(row);
                }

                table.AcceptChanges();
                dataSet.Tables.Add(table);
                dataSet.Tables.Add(tableI);

                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                {
                    DSF_sr.PresenceReportCRView_sr view = new Reports.DSF.DSF_sr.PresenceReportCRView_sr(dataSet, dtpFromDate.Value.Date, dtpToDate.Value.Date);

                    view.ShowDialog(this);
                }
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                {
                    DSF_en.PresenceReportCRView_en view = new Reports.DSF.DSF_en.PresenceReportCRView_en(dataSet, dtpFromDate.Value.Date, dtpToDate.Value.Date);

                    view.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " DSFPresenceReport.generateSummaryReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private List<WorkTimeIntervalTO> getTimeSchemaInterval(int employeeID, DateTime date, List<EmployeeTimeScheduleTO> timeScheduleList, List<WorkTimeSchemaTO> timeSchema)
        {
            try
            {
                List<WorkTimeIntervalTO> intervalList = new List<WorkTimeIntervalTO>();

                int timeScheduleIndex = -1;

                for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
                {
                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
                    if (date >= timeScheduleList[scheduleIndex].Date)
                    //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
                    {
                        timeScheduleIndex = scheduleIndex;
                    }
                }

                if (timeScheduleIndex >= 0)
                {
                    int cycleDuration = 0;
                    int startDay = timeScheduleList[timeScheduleIndex].StartCycleDay;
                    int schemaID = timeScheduleList[timeScheduleIndex].TimeSchemaID;
                    List<WorkTimeSchemaTO> timeSchemaEmployee = new List<WorkTimeSchemaTO>();
                    foreach (WorkTimeSchemaTO timeSch in timeSchema)
                    {
                        if (timeSch.TimeSchemaID == schemaID)
                        {
                            timeSchemaEmployee.Add(timeSch);
                        }
                    }
                    WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                    if (timeSchemaEmployee.Count > 0)
                    {
                        schema = timeSchemaEmployee[0];
                        cycleDuration = schema.CycleDuration;
                    }

                    TimeSpan days = new TimeSpan(date.Date.Ticks - timeScheduleList[timeScheduleIndex].Date.Date.Ticks);

                    Dictionary<int, WorkTimeIntervalTO> table = schema.Days[(startDay + (int)days.TotalDays) % cycleDuration];
                    for (int i = 0; i < table.Count; i++)
                    {
                        intervalList.Add(table[i]);
                    }
                }

                return intervalList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
