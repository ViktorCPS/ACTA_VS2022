using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.IO;

using Util;
using Common;
using TransferObjects;

namespace Reports.WakerNeuson
{
    public partial class WNMonthlyReport : Form
    {
        private const int emplIDIndex = 0;
        private const int emplNameIndex = 1;

        private const string delimiter = ",";

        private const int unpaidLeavePT = 41;
        
        private int emplSortField;

        private ListViewItemComparer _comp;

        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private string wuString = "";
        private List<int> wuList = new List<int>();

        private Dictionary<int, OrganizationalUnitTO> oUnits = new Dictionary<int, OrganizationalUnitTO>();
        private string ouString = "";
        private List<int> ouList = new List<int>();

        Dictionary<int, WorkingUnitTO> wuDict = new Dictionary<int, WorkingUnitTO>();
        Dictionary<int, OrganizationalUnitTO> ouDict = new Dictionary<int, OrganizationalUnitTO>();
        Dictionary<int, PassTypeTO> ptDict = new Dictionary<int, PassTypeTO>();
        Dictionary<int, Dictionary<int, string>> emplTypesDict = new Dictionary<int, Dictionary<int, string>>();
        Dictionary<int, WorkTimeSchemaTO> schemas = new Dictionary<int, WorkTimeSchemaTO>();
        Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rules = new Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>>();
        
        public WNMonthlyReport()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(WNMonthlyReport).Assembly);

                logInUser = NotificationController.GetLogInUser();

                setLanguage();

                rbWU.Checked = true;
                rbSummary.Checked = true;

                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Inner Class for sorting List of Employees
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
                    case WNMonthlyReport.emplIDIndex:
                        {
                            int id1 = -1;
                            int id2 = -1;

                            int.TryParse(sub1.Text, out id1);
                            int.TryParse(sub2.Text, out id2);

                            return CaseInsensitiveComparer.Default.Compare(id1, id2);
                        }
                    case WNMonthlyReport.emplNameIndex:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                    default:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                }
            }
        }

        #endregion

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("WNMonthlyReport", culture);

                //label's text
                this.lblEmployee.Text = rm.GetString("lblEmployee", culture);
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);

                //button's text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnGenerate.Text = rm.GetString("btnGenerate", culture);

                //group box text
                this.gbDateInterval.Text = rm.GetString("gbDateInterval", culture);
                this.gbUnitFilter.Text = rm.GetString("gbUnitFilter", culture);
                this.gbReportType.Text = rm.GetString("gbReportType", culture);

                // radio button text
                this.rbAnalytical.Text = rm.GetString("rbAnalytical", culture);
                this.rbSummary.Text = rm.GetString("rbSummary", culture);

                // check box text
                this.chbEmplTypeSum.Text = rm.GetString("chbEmplTypeSum", culture);

                // list view                
                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrID", culture), 75, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrName", culture), 230, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WNMonthlyReport.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WNMonthlyReport_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                emplSortField = emplNameIndex;

                // Initialize comparer objects
                _comp = new ListViewItemComparer(lvEmployees);
                _comp.SortColumn = emplSortField;
                lvEmployees.ListViewItemSorter = _comp;
                lvEmployees.Sorting = SortOrder.Ascending;

                wuDict = new WorkingUnit().getWUDictionary();
                ouDict = new OrganizationalUnit().SearchDictionary();
                ptDict = new PassType().SearchDictionaryCodeSorted();
                emplTypesDict = new EmployeeType().SearchDictionary();
                schemas = new TimeSchema().getDictionary();
                rules = new Common.Rule().SearchWUEmplTypeDictionary();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                    oUnits = new ApplUserXOrgUnit().FindOUForUserDictionary(logInUser.UserID.Trim(), Constants.ReportPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                    wuList.Add(wUnit.WorkingUnitID);
                }

                if (wuString.Length > 0)
                    wuString = wuString.Substring(0, wuString.Length - 1);

                foreach (int id in oUnits.Keys)
                {
                    ouString += id.ToString().Trim() + ",";
                    ouList.Add(id);
                }

                if (ouString.Length > 0)
                    ouString = ouString.Substring(0, ouString.Length - 1);

                populateWU();
                populateOU();

                rbWU.Checked = true;
                rbWU_CheckedChanged(this, new EventArgs());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WNMonthlyReport.WNMonthlyReport_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateWU()
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
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WNMonthlyReport.populateWU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateOU()
        {
            try
            {
                List<OrganizationalUnitTO> ouArray = new List<OrganizationalUnitTO>();

                foreach (int id in oUnits.Keys)
                {
                    ouArray.Add(oUnits[id]);
                }

                ouArray.Insert(0, new OrganizationalUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), ""));

                cbOU.DataSource = ouArray;
                cbOU.DisplayMember = "Name";
                cbOU.ValueMember = "OrgUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WNMonthlyReport.populateOU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateEmployees()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<EmployeeTO> employeeList = new List<EmployeeTO>();

                bool isWU = rbWU.Checked;

                int ID = -1;
                if (isWU && cbWU.SelectedIndex > 0)
                    ID = (int)cbWU.SelectedValue;
                else if (!isWU && cbOU.SelectedIndex > 0)
                    ID = (int)cbOU.SelectedValue;

                //int onlyEmplTypeID = -1;
                //int exceptEmplTypeID = -1;

                DateTime currMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime from = currMonth.AddMonths(-1);
                DateTime to = currMonth.AddMonths(2).AddDays(-1);

                if (ID != -1)
                {
                    if (isWU)
                    {
                        string wunits = Common.Misc.getWorkingUnitHierarhicly(ID, wuList, null);

                        // get employees from selected working unit that are not currently loaned to other working unit or are currently loand to selected working unit                        
                        employeeList = new Employee().SearchByWULoans(wunits, -1, null, from.Date, to.Date);
                    }
                    else
                    {
                        string ounits = Common.Misc.getOrgUnitHierarhicly(ID.ToString(), ouList, null);

                        employeeList = new Employee().SearchByOU(ounits, -1, null, from.Date, to.Date);
                    }
                }

                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();

                foreach (EmployeeTO empl in employeeList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = empl.EmployeeID.ToString().Trim();
                    item.SubItems.Add(empl.FirstAndLastName);
                    item.ToolTipText = empl.FirstAndLastName;

                    if (empl.EmployeeTypeID == (int)Constants.EmployeeTypesWN.MANAGEMENT)
                        empl.EmployeeTypeID = (int)Constants.EmployeeTypesWN.ADMINISTRATION;

                    item.Tag = empl;
                    lvEmployees.Items.Add(item);
                }

                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WNMonthlyReport.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbWU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cbWU.Enabled = rbWU.Checked;

                cbOU.Enabled = !rbWU.Checked;

                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WNMonthlyReport.rbWU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbOU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cbWU.Enabled = !rbOU.Checked;

                cbOU.Enabled = rbOU.Checked;

                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WNMonthlyReport.rbOU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WNMonthlyReport.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbOU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WNMonthlyReport.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void tbEmployee_TextChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string text = tbEmployee.Text.Trim();

                int id = -1;

                lvEmployees.SelectedItems.Clear();

                if (!text.Trim().Equals(""))
                {
                    if (int.TryParse(text, out id))
                    {
                        foreach (ListViewItem item in lvEmployees.Items)
                        {
                            if (((EmployeeTO)item.Tag).EmployeeID.ToString().Trim().StartsWith(id.ToString().Trim()))
                            {
                                item.Selected = true;
                                lvEmployees.Select();
                                lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));

                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (ListViewItem item in lvEmployees.Items)
                        {
                            if (((EmployeeTO)item.Tag).FirstAndLastName.ToString().Trim().ToUpper().StartsWith(text.ToString().Trim().ToUpper()))
                            {
                                item.Selected = true;
                                lvEmployees.Select();
                                lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));

                                break;
                            }
                        }
                    }
                }

                tbEmployee.Focus();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.tbEmployee_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvEmployees_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvEmployees.Sorting;

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

                lvEmployees.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.lvEmployees_ColumnClick(): " + ex.Message + "\n");
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
                if (lvEmployees.Items.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }

                if (dtpFrom.Value.Date > dtpTo.Value.Date)
                {
                    MessageBox.Show(rm.GetString("invalidDateInterval", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                DateTime from = dtpFrom.Value.Date;
                DateTime to = dtpTo.Value.Date;

                string emplIDs = "";
                Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();

                if (lvEmployees.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvEmployees.SelectedItems)
                    {
                        emplIDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";

                        if (!emplDict.ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                            emplDict.Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        emplIDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";

                        if (!emplDict.ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                            emplDict.Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);
                    }
                }

                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                // get pairs for one more day becouse of third shifts
                List<DateTime> dateList = new List<DateTime>();
                DateTime currentDate = from.Date;
                while (currentDate.Date <= to.AddDays(1).Date)
                {
                    dateList.Add(currentDate.Date);
                    currentDate = currentDate.AddDays(1);
                }
                List<IOPairProcessedTO> allPairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, dateList, "");
                
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplBelongingDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                Dictionary<int, List<DateTime>> emplUnjustifiedDays = new Dictionary<int, List<DateTime>>();
                Dictionary<int, Dictionary<DateTime, int>> employeePresence = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, List<DateTime>> employeeDayMeals = new Dictionary<int, List<DateTime>>();
                Dictionary<int, int> employeeMeals = new Dictionary<int, int>();

                // create list of pairs foreach employee and each day
                string unregularData = "";
                List<int> unregularIDs = new List<int>();
                foreach (IOPairProcessedTO pair in allPairs)
                {
                    if (!emplDayPairs.ContainsKey(pair.EmployeeID))
                        emplDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                    if (!emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        emplDayPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());
                    emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                }

                // get asco data
                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary(emplIDs);

                //get national and personal holidays
                List<DateTime> nationalHolidaysDays = new List<DateTime>();
                Dictionary<string, List<DateTime>> personalHolidayDays = new Dictionary<string, List<DateTime>>();
                List<DateTime> nationalHolidaysSundays = new List<DateTime>();
                List<HolidaysExtendedTO> nationalTransferableHolidays = new List<HolidaysExtendedTO>();

                Common.Misc.getHolidays(from.Date, to.Date, personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, null, nationalTransferableHolidays);
                
                // get schemas and intervals
                Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>> emplDaySchemas = new Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>>();
                Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>> emplDayIntervals = new Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>>();

                // get schedules for selected employees and date interval
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, from.Date, to.Date.AddDays(1), null);
                Dictionary<int, Dictionary<DateTime, int>> emplWorkingNorm = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, int> emplWorkingNormSummary = new Dictionary<int, int>();
                foreach (int emplID in emplSchedules.Keys)
                {
                    if (!emplWorkingNorm.ContainsKey(emplID))
                        emplWorkingNorm.Add(emplID, new Dictionary<DateTime, int>());

                    if (!emplWorkingNormSummary.ContainsKey(emplID))
                        emplWorkingNormSummary.Add(emplID, 0);

                    DateTime currDate = from.Date;

                    while (currDate <= to.Date.AddDays(1))
                    {
                        if (!emplWorkingNorm[emplID].ContainsKey(currDate.Date))
                            emplWorkingNorm[emplID].Add(currDate.Date, 0);

                        if (!emplDayIntervals.ContainsKey(emplID))
                            emplDayIntervals.Add(emplID, new Dictionary<DateTime, List<WorkTimeIntervalTO>>());

                        if (!emplDayIntervals[emplID].ContainsKey(currDate.Date))
                        {
                            emplDayIntervals[emplID].Add(currDate.Date, Common.Misc.getTimeSchemaInterval(currDate.Date, emplSchedules[emplID], schemas));

                            foreach (WorkTimeIntervalTO interval in emplDayIntervals[emplID][currDate.Date])
                            {
                                int intervalDuration = (int)interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes;

                                if (interval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                    intervalDuration++;

                                if (interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                {
                                    emplWorkingNorm[emplID][currDate.Date] += intervalDuration;

                                    if (currDate.Date <= to.Date)
                                        emplWorkingNormSummary[emplID] += intervalDuration;
                                }
                                else if (emplWorkingNorm[emplID].ContainsKey(currDate.AddDays(-1).Date))
                                {
                                    emplWorkingNorm[emplID][currDate.AddDays(-1).Date] += intervalDuration;

                                    if (currDate.AddDays(-1).Date >= from.Date)
                                        emplWorkingNormSummary[emplID] += intervalDuration;
                                }
                            }
                        }

                        if (!emplDaySchemas.ContainsKey(emplID))
                            emplDaySchemas.Add(emplID, new Dictionary<DateTime, WorkTimeSchemaTO>());

                        WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                        if (emplDayIntervals[emplID][currDate.Date].Count > 0 && schemas.ContainsKey(emplDayIntervals[emplID][currDate.Date][0].TimeSchemaID))
                            sch = schemas[emplDayIntervals[emplID][currDate.Date][0].TimeSchemaID];

                        if (!emplDaySchemas[emplID].ContainsKey(currDate.Date))
                            emplDaySchemas[emplID].Add(currDate.Date, sch);
                        
                        currDate = currDate.AddDays(1).Date;
                    }
                }

                Dictionary<int, Dictionary<string, RuleTO>> emplRules = new Dictionary<int, Dictionary<string, RuleTO>>();
                Dictionary<int, List<DateTime>> emplHolidayPaidDays = new Dictionary<int,List<DateTime>>();
                                
                List<int> sickLeaveNCF = new List<int>();
                List<int> paidPT = new List<int>();
                List<int> paidLeavePT = new List<int>();
                // create rules for each employee
                foreach (int emplID in emplDict.Keys)
                {
                    if (!emplRules.ContainsKey(emplID))
                    {
                        int company = Common.Misc.getRootWorkingUnit(emplDict[emplID].WorkingUnitID, wuDict);

                        if (rules.ContainsKey(company) && rules[company].ContainsKey(emplDict[emplID].EmployeeTypeID))
                        {
                            emplRules.Add(emplID, rules[company][emplDict[emplID].EmployeeTypeID]);

                            if (rules[company][emplDict[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanySickLeaveNCF)
                                && !sickLeaveNCF.Contains(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanySickLeaveNCF].RuleValue))
                                sickLeaveNCF.Add(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanySickLeaveNCF].RuleValue);

                            if (rules[company][emplDict[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanyPeriodicalMedicalCheckUp)
                                && !paidLeavePT.Contains(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanyPeriodicalMedicalCheckUp].RuleValue))
                                paidLeavePT.Add(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanyPeriodicalMedicalCheckUp].RuleValue);

                            if (rules[company][emplDict[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanyAnnualLeave)
                                && !paidPT.Contains(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanyAnnualLeave].RuleValue))
                                paidPT.Add(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanyAnnualLeave].RuleValue);

                            if (rules[company][emplDict[emplID].EmployeeTypeID].ContainsKey(Constants.RuleHolidayPassType)
                                && !paidPT.Contains(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleHolidayPassType].RuleValue))
                                paidPT.Add(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleHolidayPassType].RuleValue);

                            if (rules[company][emplDict[emplID].EmployeeTypeID].ContainsKey(Constants.RulePersonalHolidayPassType)
                                && !paidPT.Contains(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RulePersonalHolidayPassType].RuleValue))
                                paidPT.Add(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RulePersonalHolidayPassType].RuleValue);

                            if (rules[company][emplDict[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanyOfficialTrip)
                                && !paidPT.Contains(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanyOfficialTrip].RuleValue))
                                paidPT.Add(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanyOfficialTrip].RuleValue);

                            if (rules[company][emplDict[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanyRegularWork)
                                && !paidPT.Contains(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanyRegularWork].RuleValue))
                                paidPT.Add(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanyRegularWork].RuleValue);

                            if (rules[company][emplDict[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanyOvertimePaid)
                                && !paidPT.Contains(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanyOvertimePaid].RuleValue))
                                paidPT.Add(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanyOvertimePaid].RuleValue);
                        }
                    }
                }

                // get all sick leave
                Dictionary<int, List<int>> confirmationPTDict = new PassTypesConfirmation().SearchDictionary();
                List<int> sickLeavePT = new List<int>();
                foreach (int ptID in sickLeaveNCF)
                {
                    if (confirmationPTDict.ContainsKey(ptID))
                        sickLeavePT.AddRange(confirmationPTDict[ptID]);
                }
                
                foreach (int ptID in confirmationPTDict.Keys)
                {
                    if (ptID != unpaidLeavePT)
                        paidLeavePT.AddRange(confirmationPTDict[ptID]);
                }

                // analytical dictionaries
                Dictionary<int, Dictionary<DateTime, int>> emplWorkOnSundays = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> emplWorkOnSuterdays = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> emplWorkOnSundaysBH = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> emplWorkOnSuterdaysBH = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> emplSickLeaves = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> emplPaidLeaves = new Dictionary<int, Dictionary<DateTime, int>>();
                //Dictionary<int, Dictionary<DateTime, int>> emplDayTotal = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> emplUnpaid = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, Dictionary<int, int>>> emplPTDuration = new Dictionary<int, Dictionary<DateTime, Dictionary<int, int>>>();

                // summary dictionaries
                Dictionary<int, int> emplWorkOnSundaysSummary = new Dictionary<int, int>();
                Dictionary<int, int> emplWorkOnSuterdaysSummary = new Dictionary<int, int>();
                Dictionary<int, int> emplWorkOnSundaysBHSummary = new Dictionary<int, int>();
                Dictionary<int, int> emplWorkOnSuterdaysBHSummary = new Dictionary<int, int>();
                Dictionary<int, int> emplSickLeavesSummary = new Dictionary<int, int>();
                Dictionary<int, int> emplPaidLeavesSummary = new Dictionary<int, int>();
                //Dictionary<int, int> emplTotal = new Dictionary<int, int>();
                Dictionary<int, int> emplUnpaidSummary = new Dictionary<int, int>();
                Dictionary<int, Dictionary<int, int>> emplPTDurationSummary = new Dictionary<int, Dictionary<int, int>>();
                                
                foreach (IOPairProcessedTO pair in allPairs)
                {
                    DateTime pairDate = pair.IOPairDate.Date;

                    List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                    if (emplDayPairs.ContainsKey(pair.EmployeeID) && emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        dayPairs = emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date];

                    WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                    if (emplDaySchemas.ContainsKey(pair.EmployeeID) && emplDaySchemas[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        sch = emplDaySchemas[pair.EmployeeID][pair.IOPairDate.Date];

                    List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();
                    if (emplDayIntervals.ContainsKey(pair.EmployeeID) && emplDayIntervals[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        dayIntervals = emplDayIntervals[pair.EmployeeID][pair.IOPairDate.Date];

                    bool previousDayPair = Common.Misc.isPreviousDayPair(pair, ptDict, dayPairs, sch, dayIntervals);

                    if (previousDayPair)
                        pairDate = pairDate.AddDays(-1).Date;

                    if (pairDate.Date >= from.Date && pairDate.Date <= to.Date)
                    {
                        // do not create report if there is unconfirmed data
                        if (pair.ConfirmationFlag == (int)Constants.Confirmation.NotConfirmed && !unregularIDs.Contains(pair.EmployeeID))
                        {
                            unregularIDs.Add(pair.EmployeeID);
                            unregularData += pair.EmployeeID.ToString().Trim() + ",";
                            continue;
                        }

                        if (!emplBelongingDayPairs.ContainsKey(pair.EmployeeID))
                            emplBelongingDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                        if (!emplBelongingDayPairs[pair.EmployeeID].ContainsKey(pairDate.Date))
                            emplBelongingDayPairs[pair.EmployeeID].Add(pairDate.Date, new List<IOPairProcessedTO>());

                        emplBelongingDayPairs[pair.EmployeeID][pairDate.Date].Add(pair);

                        if (pair.PassTypeID == Constants.absence)
                        {
                            if (!emplUnjustifiedDays.ContainsKey(pair.EmployeeID))
                                emplUnjustifiedDays.Add(pair.EmployeeID, new List<DateTime>());

                            if (!emplUnjustifiedDays[pair.EmployeeID].Contains(pairDate.Date))
                                emplUnjustifiedDays[pair.EmployeeID].Add(pairDate.Date);
                        }
                    }
                }

                if (unregularIDs.Count > 0)
                {
                    MessageBox.Show(rm.GetString("notConfirmedDataFound", culture) + " " + unregularData.Trim().Substring(0, unregularData.Trim().Length - 1) + ".");
                    return;
                }

                // create list of pairs foreach employee and each pair belonging day                
                foreach (int employeeID in emplBelongingDayPairs.Keys)
                {
                    foreach (DateTime date in emplBelongingDayPairs[employeeID].Keys)
                    {
                        bool unjustifiedDay = emplUnjustifiedDays.ContainsKey(employeeID) && emplUnjustifiedDays[employeeID].Contains(date.Date);
                        foreach (IOPairProcessedTO pair in emplBelongingDayPairs[employeeID][date])
                        {
                            EmployeeTO empl = new EmployeeTO();
                            if (emplDict.ContainsKey(pair.EmployeeID))
                                empl = emplDict[pair.EmployeeID];

                            EmployeeAsco4TO asco = new EmployeeAsco4TO();
                            if (ascoDict.ContainsKey(pair.EmployeeID))
                                asco = ascoDict[pair.EmployeeID];

                            Dictionary<string, RuleTO> rulesForEmpl = new Dictionary<string, RuleTO>();
                            if (emplRules.ContainsKey(pair.EmployeeID))
                                rulesForEmpl = emplRules[pair.EmployeeID];

                            WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                            if (emplDaySchemas.ContainsKey(pair.EmployeeID) && emplDaySchemas[pair.EmployeeID].ContainsKey(date.Date))
                                schema = emplDaySchemas[pair.EmployeeID][date.Date];

                            List<DateTime> paidDays = new List<DateTime>();
                            if (emplHolidayPaidDays.ContainsKey(pair.EmployeeID))
                                paidDays = emplHolidayPaidDays[pair.EmployeeID];

                            int workOnHolidayPT = -1;
                            int nightWorkPT = -1;
                            int turnusPT = -1;
                            int regWorkPT = -1;
                            int deleyPT = -1;

                            if (rulesForEmpl.ContainsKey(Constants.RuleWorkOnHolidayPassType))
                                workOnHolidayPT = rulesForEmpl[Constants.RuleWorkOnHolidayPassType].RuleValue;

                            if (rulesForEmpl.ContainsKey(Constants.RuleNightWork))
                                nightWorkPT = rulesForEmpl[Constants.RuleNightWork].RuleValue;

                            if (rulesForEmpl.ContainsKey(Constants.RuleComanyRotaryShift))
                                turnusPT = rulesForEmpl[Constants.RuleComanyRotaryShift].RuleValue;

                            if (rulesForEmpl.ContainsKey(Constants.RuleCompanyRegularWork))
                                regWorkPT = rulesForEmpl[Constants.RuleCompanyRegularWork].RuleValue;

                            if (rulesForEmpl.ContainsKey(Constants.RuleCompanyDelay))
                                deleyPT = rulesForEmpl[Constants.RuleCompanyDelay].RuleValue;

                            int oldPairPT = pair.PassTypeID;
                            if (unjustifiedDay && ptDict.ContainsKey(pair.PassTypeID) && ptDict[pair.PassTypeID].IsPass != Constants.overtimePassType
                                && ptDict[pair.PassTypeID].IsPass != Constants.otherPaymentCode)
                                pair.PassTypeID = Constants.absence;
                                                        
                            if (!unjustifiedDay && ptDict.ContainsKey(pair.PassTypeID) && ptDict[pair.PassTypeID].IsPass == Constants.passOnReader && !paidLeavePT.Contains(pair.PassTypeID))
                                pair.PassTypeID = regWorkPT;
                            
                            // calculate code durations and add it to dictionary
                            int pairDuration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                            if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                pairDuration++;

                            if (!emplPTDuration.ContainsKey(pair.EmployeeID))
                                emplPTDuration.Add(pair.EmployeeID, new Dictionary<DateTime, Dictionary<int, int>>());
                            if (!emplPTDuration[pair.EmployeeID].ContainsKey(date.Date))
                                emplPTDuration[pair.EmployeeID].Add(date.Date, new Dictionary<int, int>());

                            if (!emplPTDurationSummary.ContainsKey(pair.EmployeeID))
                                emplPTDurationSummary.Add(pair.EmployeeID, new Dictionary<int, int>());

                            if (isSundayWork(pair, date, rulesForEmpl, emplDict, schema))
                            {
                                if (!emplWorkOnSundays.ContainsKey(pair.EmployeeID))
                                    emplWorkOnSundays.Add(pair.EmployeeID, new Dictionary<DateTime, int>());

                                if (!emplWorkOnSundays[pair.EmployeeID].ContainsKey(date.Date))
                                    emplWorkOnSundays[pair.EmployeeID].Add(date.Date, 0);

                                emplWorkOnSundays[pair.EmployeeID][date.Date] += pairDuration;

                                if (!emplWorkOnSundaysSummary.ContainsKey(pair.EmployeeID))
                                    emplWorkOnSundaysSummary.Add(pair.EmployeeID, 0);

                                emplWorkOnSundaysSummary[pair.EmployeeID] += pairDuration;
                            }
                            else if (isSaturdayWork(pair, date, rulesForEmpl, emplDict, schema))
                            {
                                if (!emplWorkOnSuterdays.ContainsKey(pair.EmployeeID))
                                    emplWorkOnSuterdays.Add(pair.EmployeeID, new Dictionary<DateTime, int>());

                                if (!emplWorkOnSuterdays[pair.EmployeeID].ContainsKey(date.Date))
                                    emplWorkOnSuterdays[pair.EmployeeID].Add(date.Date, 0);

                                emplWorkOnSuterdays[pair.EmployeeID][date.Date] += pairDuration;

                                if (!emplWorkOnSuterdaysSummary.ContainsKey(pair.EmployeeID))
                                    emplWorkOnSuterdaysSummary.Add(pair.EmployeeID, 0);

                                emplWorkOnSuterdaysSummary[pair.EmployeeID] += pairDuration;
                            }
                            else if (isSundayBHWork(pair, date, rulesForEmpl, emplDict, schema))
                            {
                                if (!emplWorkOnSundaysBH.ContainsKey(pair.EmployeeID))
                                    emplWorkOnSundaysBH.Add(pair.EmployeeID, new Dictionary<DateTime, int>());

                                if (!emplWorkOnSundaysBH[pair.EmployeeID].ContainsKey(date.Date))
                                    emplWorkOnSundaysBH[pair.EmployeeID].Add(date.Date, 0);

                                emplWorkOnSundaysBH[pair.EmployeeID][date.Date] += pairDuration;

                                if (!emplWorkOnSundaysBHSummary.ContainsKey(pair.EmployeeID))
                                    emplWorkOnSundaysBHSummary.Add(pair.EmployeeID, 0);

                                emplWorkOnSundaysBHSummary[pair.EmployeeID] += pairDuration;
                            }
                            else if (isSaturdayBHWork(pair, date, rulesForEmpl, emplDict, schema))
                            {
                                if (!emplWorkOnSuterdaysBH.ContainsKey(pair.EmployeeID))
                                    emplWorkOnSuterdaysBH.Add(pair.EmployeeID, new Dictionary<DateTime, int>());

                                if (!emplWorkOnSuterdaysBH[pair.EmployeeID].ContainsKey(date.Date))
                                    emplWorkOnSuterdaysBH[pair.EmployeeID].Add(date.Date, 0);

                                emplWorkOnSuterdaysBH[pair.EmployeeID][date.Date] += pairDuration;

                                if (!emplWorkOnSuterdaysBHSummary.ContainsKey(pair.EmployeeID))
                                    emplWorkOnSuterdaysBHSummary.Add(pair.EmployeeID, 0);

                                emplWorkOnSuterdaysBHSummary[pair.EmployeeID] += pairDuration;
                            }
                            else
                            {                                
                                if (!emplPTDuration[pair.EmployeeID][date.Date].ContainsKey(pair.PassTypeID))
                                    emplPTDuration[pair.EmployeeID][date.Date].Add(pair.PassTypeID, 0);

                                emplPTDuration[pair.EmployeeID][date.Date][pair.PassTypeID] += pairDuration;

                                if (!emplPTDurationSummary[pair.EmployeeID].ContainsKey(pair.PassTypeID))
                                    emplPTDurationSummary[pair.EmployeeID].Add(pair.PassTypeID, 0);

                                emplPTDurationSummary[pair.EmployeeID][pair.PassTypeID] += pairDuration;

                                if (oldPairPT == deleyPT)
                                {
                                    if (!emplPTDuration[pair.EmployeeID][date.Date].ContainsKey(deleyPT))
                                        emplPTDuration[pair.EmployeeID][date.Date].Add(deleyPT, 0);

                                    emplPTDuration[pair.EmployeeID][date.Date][deleyPT] += pairDuration;

                                    if (!emplPTDurationSummary[pair.EmployeeID].ContainsKey(deleyPT))
                                        emplPTDurationSummary[pair.EmployeeID].Add(deleyPT, 0);

                                    emplPTDurationSummary[pair.EmployeeID][deleyPT] += pairDuration;
                                }
                            }

                            // calculate addition for night work, turnus and holidays
                            if (pair.PassTypeID != Constants.absence && isPresence(oldPairPT, rulesForEmpl))
                            {                                
                                // check work on holiday
                                if (workOnHolidayPT != -1
                                    && isWorkOnHoliday(personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, nationalTransferableHolidays, paidDays, date.Date, schema, asco, empl))
                                {                                    
                                    if (!emplPTDuration[pair.EmployeeID][date.Date].ContainsKey(workOnHolidayPT))
                                        emplPTDuration[pair.EmployeeID][date.Date].Add(workOnHolidayPT, 0);

                                    emplPTDuration[pair.EmployeeID][date.Date][workOnHolidayPT] += pairDuration;

                                    if (!emplPTDurationSummary[pair.EmployeeID].ContainsKey(workOnHolidayPT))
                                        emplPTDurationSummary[pair.EmployeeID].Add(workOnHolidayPT, 0);

                                    emplPTDurationSummary[pair.EmployeeID][workOnHolidayPT] += pairDuration;

                                    if (!emplHolidayPaidDays.ContainsKey(pair.EmployeeID))
                                        emplHolidayPaidDays.Add(pair.EmployeeID, paidDays);
                                    else
                                        emplHolidayPaidDays[pair.EmployeeID] = paidDays;

                                    // if type is not overtime, subtract pair work duration from dictionary
                                    //if (ptDict.ContainsKey(pair.PassTypeID) && ptDict[pair.PassTypeID].IsPass != Constants.overtimePassType)
                                    //{
                                    //    if (emplPTDuration[pair.EmployeeID][date.Date].ContainsKey(pair.PassTypeID))
                                    //        emplPTDuration[pair.EmployeeID][date.Date][pair.PassTypeID] -= pairDuration;

                                    //    if (emplPTDurationSummary[pair.EmployeeID].ContainsKey(pair.PassTypeID))
                                    //        emplPTDurationSummary[pair.EmployeeID][pair.PassTypeID] -= pairDuration;
                                    //}
                                }

                                if (turnusPT != -1 && isTurnus(schema) && ptDict.ContainsKey(pair.PassTypeID) && ptDict[pair.PassTypeID].IsPass != Constants.overtimePassType)
                                {
                                    if (!emplPTDuration[pair.EmployeeID][date.Date].ContainsKey(turnusPT))
                                        emplPTDuration[pair.EmployeeID][date.Date].Add(turnusPT, 0);

                                    emplPTDuration[pair.EmployeeID][date.Date][turnusPT] += pairDuration;

                                    if (!emplPTDurationSummary[pair.EmployeeID].ContainsKey(turnusPT))
                                        emplPTDurationSummary[pair.EmployeeID].Add(turnusPT, 0);

                                    emplPTDurationSummary[pair.EmployeeID][turnusPT] += pairDuration;
                                }

                                int nightWork = nightWorkDuration(rulesForEmpl, pair, schema);

                                if (nightWorkPT != -1 && nightWork > 0)
                                {
                                    if (!emplPTDuration[pair.EmployeeID][date.Date].ContainsKey(nightWorkPT))
                                        emplPTDuration[pair.EmployeeID][date.Date].Add(nightWorkPT, 0);

                                    emplPTDuration[pair.EmployeeID][date.Date][nightWorkPT] += nightWork;

                                    if (!emplPTDurationSummary[pair.EmployeeID].ContainsKey(nightWorkPT))
                                        emplPTDurationSummary[pair.EmployeeID].Add(nightWorkPT, 0);

                                    emplPTDurationSummary[pair.EmployeeID][nightWorkPT] += nightWork;

                                    // if type is not overtime, subtract night work duration from dictionary
                                    //if (ptDict.ContainsKey(pair.PassTypeID) && ptDict[pair.PassTypeID].IsPass != Constants.overtimePassType)
                                    //{
                                    //    if (emplPTDuration[pair.EmployeeID][date.Date].ContainsKey(pair.PassTypeID))
                                    //        emplPTDuration[pair.EmployeeID][date.Date][pair.PassTypeID] -= nightWork;

                                    //    if (emplPTDurationSummary[pair.EmployeeID].ContainsKey(pair.PassTypeID))
                                    //        emplPTDurationSummary[pair.EmployeeID][pair.PassTypeID] -= nightWork;
                                    //}
                                }
                            }

                            // calculate presenece for meals (if day is Saturday or Sunday, do not calculate meals if employee type is not DIRECT or INDIRECT)
                            if (isMealPresence(oldPairPT, rulesForEmpl, empl.EmployeeTypeID, date.Date))
                            {
                                if (!employeePresence.ContainsKey(employeeID))
                                    employeePresence.Add(employeeID, new Dictionary<DateTime, int>());

                                if (!employeePresence[employeeID].ContainsKey(date.Date))
                                    employeePresence[employeeID].Add(date.Date, 0);

                                employeePresence[employeeID][date.Date] += pairDuration;
                            }
                        }
                    }
                }

                // create summary data
                foreach (int emplID in emplPTDuration.Keys)
                {
                    // get delay type
                    int delayPT = -1;

                    if (emplRules.ContainsKey(emplID) && emplRules[emplID].ContainsKey(Constants.RuleCompanyDelay))
                        delayPT = emplRules[emplID][Constants.RuleCompanyDelay].RuleValue;                    

                    foreach (DateTime date in emplPTDuration[emplID].Keys)
                    {
                        if (!emplUnpaid.ContainsKey(emplID))
                            emplUnpaid.Add(emplID, new Dictionary<DateTime, int>());
                        if (!emplUnpaid[emplID].ContainsKey(date))
                            emplUnpaid[emplID].Add(date, 0);

                        if (!emplUnpaidSummary.ContainsKey(emplID))
                            emplUnpaidSummary.Add(emplID, 0);
                        
                        foreach (int ptID in emplPTDuration[emplID][date].Keys)
                        {                        
                            int duration = emplPTDuration[emplID][date][ptID];
                            
                            // check if it is sick leave
                            if (sickLeavePT.Contains(ptID))
                            {
                                if (!emplSickLeaves.ContainsKey(emplID))
                                    emplSickLeaves.Add(emplID, new Dictionary<DateTime, int>());
                                if (!emplSickLeaves[emplID].ContainsKey(date))
                                    emplSickLeaves[emplID].Add(date, 0);
                                emplSickLeaves[emplID][date] += duration;

                                if (!emplSickLeavesSummary.ContainsKey(emplID))
                                    emplSickLeavesSummary.Add(emplID, 0);
                                emplSickLeavesSummary[emplID] += duration;                                
                            }
                            // check if it is paid leave
                            else if (paidLeavePT.Contains(ptID))
                            {
                                if (!emplPaidLeaves.ContainsKey(emplID))
                                    emplPaidLeaves.Add(emplID, new Dictionary<DateTime, int>());
                                if (!emplPaidLeaves[emplID].ContainsKey(date))
                                    emplPaidLeaves[emplID].Add(date, 0);
                                emplPaidLeaves[emplID][date] += duration;

                                if (!emplPaidLeavesSummary.ContainsKey(emplID))
                                    emplPaidLeavesSummary.Add(emplID, 0);
                                emplPaidLeavesSummary[emplID] += duration;                                
                            }
                            else if (ptID == Constants.absence || (ptID != delayPT && !paidPT.Contains(ptID) && ptDict.ContainsKey(ptID) && (ptDict[ptID].IsPass == Constants.passOnReader
                                || ptDict[ptID].IsPass == Constants.wholeDayAbsence)))
                            {
                                emplUnpaid[emplID][date] += duration;
                                emplUnpaidSummary[emplID] += duration;
                            }
                        }
                    }
                }

                // populate meals dictionaries
                foreach (int emplID in employeePresence.Keys)
                {
                    foreach (DateTime date in employeePresence[emplID].Keys)
                    {
                        if (employeePresence[emplID][date.Date] >= Constants.WNMinMealPresence)
                        {
                            if (!employeeDayMeals.ContainsKey(emplID))
                                employeeDayMeals.Add(emplID, new List<DateTime>());

                            employeeDayMeals[emplID].Add(date.Date);

                            if (!employeeMeals.ContainsKey(emplID))
                                employeeMeals.Add(emplID, 0);

                            employeeMeals[emplID]++;
                        }
                    }
                }

                // generate xls report
                if (rbSummary.Checked)
                    generateReportSummary(emplDict, ascoDict, emplRules, emplPTDurationSummary, emplWorkOnSundaysSummary, emplWorkOnSuterdaysSummary, emplWorkOnSundaysBHSummary, emplWorkOnSuterdaysBHSummary, 
                        emplSickLeavesSummary, emplPaidLeavesSummary, emplUnpaidSummary, emplWorkingNormSummary, sickLeavePT, employeeMeals);
                else
                    generateReportAnalytical(emplDict, ascoDict, emplRules, emplPTDuration, emplWorkOnSundays, emplWorkOnSuterdays, emplWorkOnSundaysBH, emplWorkOnSuterdaysBH, emplSickLeaves, emplPaidLeaves, emplUnpaid,
                        emplWorkingNorm, emplPTDurationSummary, emplWorkOnSundaysSummary, emplWorkOnSuterdaysSummary, emplWorkOnSundaysBHSummary, emplWorkOnSuterdaysBHSummary, emplSickLeavesSummary, emplPaidLeavesSummary, 
                        emplUnpaidSummary, emplWorkingNormSummary, sickLeavePT, employeeDayMeals, employeeMeals);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.btnGenerateReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void generateReportSummary(Dictionary<int, EmployeeTO> emplDict, Dictionary<int, EmployeeAsco4TO> ascoDict, Dictionary<int, Dictionary<string, RuleTO>> emplRules, 
            Dictionary<int, Dictionary<int, int>> emplPTDurationSummary, Dictionary<int, int> emplWorkOnSundaysSummary, Dictionary<int, int> emplWorkOnSaturdaysSummary,
            Dictionary<int, int> emplWorkOnSundaysBHSummary, Dictionary<int, int> emplWorkOnSaturdaysBHSummary,
            Dictionary<int, int> emplSickLeavesSummary, Dictionary<int, int> emplPaidLeavesSummary, Dictionary<int, int> emplUnpaidSummary, 
            Dictionary<int, int> emplWorkingNormSummary, List<int> sickLeavePT, Dictionary<int, int> emplMeals)
        {
            try
            {
                Common.Rule rule = new Common.Rule();
                List<int> annualLeaves = new List<int>();
                List<int> personalHolidays = new List<int>();
                List<int> workOnHolidays = new List<int>();
                List<int> holidays = new List<int>();
                List<int> officialTrip = new List<int>();
                List<int> regularWork = new List<int>();
                List<int> nightWork = new List<int>();
                List<int> turnus = new List<int>();
                List<int> overtimePaid = new List<int>();
                List<int> overtimeRejected = new List<int>();
                List<int> delays = new List<int>();
                List<int> bankHours = new List<int>();

                // sum by employee types
                Dictionary<string, Dictionary<int, int>> sumByEmplTypes = new Dictionary<string, Dictionary<int, int>>();
                Dictionary<string, int> sumWorkOnSundays = new Dictionary<string, int>();
                Dictionary<string, int> sumWorkOnSaturdays = new Dictionary<string, int>();
                Dictionary<string, int> sumWorkOnSundaysBH = new Dictionary<string, int>();
                Dictionary<string, int> sumWorkOnSaturdaysBH = new Dictionary<string, int>();
                Dictionary<string, int> sumSickLeaves = new Dictionary<string, int>();
                Dictionary<string, int> sumPaidLeaves = new Dictionary<string, int>();
                Dictionary<string, int> sumUnpaid = new Dictionary<string, int>();
                Dictionary<string, int> sumWorkingNorm = new Dictionary<string, int>();
                Dictionary<string, int> sumTotal = new Dictionary<string, int>();
                Dictionary<string, int> sumMeals = new Dictionary<string, int>();

                // total sum
                Dictionary<int, int> totalByEmplTypes = new Dictionary<int, int>();
                int totalWorkOnSundays = 0;
                int totalWorkOnSaturdays = 0;
                int totalWorkOnSundaysBH = 0;
                int totalWorkOnSaturdaysBH = 0;
                int totalSickLeaves = 0;
                int totalPaidLeaves = 0;
                int totalUnpaid = 0;
                int totalWorkingNorm = 0;
                int totalTotal = 0;
                int totalMeals = 0;

                foreach (int emplID in emplRules.Keys)
                {
                    if (emplRules[emplID].ContainsKey(Constants.RuleCompanyAnnualLeave) && !annualLeaves.Contains(emplRules[emplID][Constants.RuleCompanyAnnualLeave].RuleValue))
                        annualLeaves.Add(emplRules[emplID][Constants.RuleCompanyAnnualLeave].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RulePersonalHolidayPassType) && !personalHolidays.Contains(emplRules[emplID][Constants.RulePersonalHolidayPassType].RuleValue))
                        personalHolidays.Add(emplRules[emplID][Constants.RulePersonalHolidayPassType].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleWorkOnHolidayPassType) && !workOnHolidays.Contains(emplRules[emplID][Constants.RuleWorkOnHolidayPassType].RuleValue))
                        workOnHolidays.Add(emplRules[emplID][Constants.RuleWorkOnHolidayPassType].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleHolidayPassType) && !holidays.Contains(emplRules[emplID][Constants.RuleHolidayPassType].RuleValue))
                        holidays.Add(emplRules[emplID][Constants.RuleHolidayPassType].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleCompanyOfficialTrip) && !officialTrip.Contains(emplRules[emplID][Constants.RuleCompanyOfficialTrip].RuleValue))
                        officialTrip.Add(emplRules[emplID][Constants.RuleCompanyOfficialTrip].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleCompanyRegularWork) && !regularWork.Contains(emplRules[emplID][Constants.RuleCompanyRegularWork].RuleValue))
                        regularWork.Add(emplRules[emplID][Constants.RuleCompanyRegularWork].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleNightWork) && !nightWork.Contains(emplRules[emplID][Constants.RuleNightWork].RuleValue))
                        nightWork.Add(emplRules[emplID][Constants.RuleNightWork].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleComanyRotaryShift) && !turnus.Contains(emplRules[emplID][Constants.RuleComanyRotaryShift].RuleValue))
                        turnus.Add(emplRules[emplID][Constants.RuleComanyRotaryShift].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleCompanyOvertimePaid) && !overtimePaid.Contains(emplRules[emplID][Constants.RuleCompanyOvertimePaid].RuleValue))
                        overtimePaid.Add(emplRules[emplID][Constants.RuleCompanyOvertimePaid].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleCompanyOvertimeRejected) && !overtimeRejected.Contains(emplRules[emplID][Constants.RuleCompanyOvertimeRejected].RuleValue))
                        overtimeRejected.Add(emplRules[emplID][Constants.RuleCompanyOvertimeRejected].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleCompanyDelay) && !delays.Contains(emplRules[emplID][Constants.RuleCompanyDelay].RuleValue))
                        delays.Add(emplRules[emplID][Constants.RuleCompanyDelay].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleCompanyBankHour) && !bankHours.Contains(emplRules[emplID][Constants.RuleCompanyBankHour].RuleValue))
                        bankHours.Add(emplRules[emplID][Constants.RuleCompanyBankHour].RuleValue);                    
                }

                // create header
                string header = rm.GetString("hdrEmplID", culture) + delimiter + rm.GetString("hdrLastName", culture) + delimiter + rm.GetString("hdrFirstName", culture)
                    + delimiter + rm.GetString("hdrType", culture) + delimiter + rm.GetString("hdrWorkingUnit", culture);

                string typeHeader = generateTypeHeader(annualLeaves, personalHolidays, workOnHolidays, holidays, officialTrip, regularWork, nightWork, turnus, overtimePaid, 
                    overtimeRejected, delays, bankHours, sickLeavePT);

                header += typeHeader + delimiter + rm.GetString("hdrAmount", culture) + delimiter + rm.GetString("hdrSaturdayBH", culture) + delimiter + rm.GetString("hdrSundayBH", culture)
                    + delimiter + rm.GetString("hdrMeals", culture);// +delimiter + rm.GetString("hdrProductionLine", culture);
                
                List<string> lines = new List<string>();

                Dictionary<string, int> emplByTypes = new Dictionary<string, int>();
                // create file lines
                foreach (int id in emplDict.Keys)
                {
                    string line = id.ToString().Trim().PadLeft(4, '0') + delimiter + emplDict[id].LastName.Trim().Replace(delimiter, " ") + delimiter + emplDict[id].FirstName.Trim().Replace(delimiter, " ");

                    string type = "";

                    int company = Common.Misc.getRootWorkingUnit(emplDict[id].WorkingUnitID, wuDict);

                    if (emplTypesDict.ContainsKey(company) && emplTypesDict[company].ContainsKey(emplDict[id].EmployeeTypeID))
                        type = emplTypesDict[company][emplDict[id].EmployeeTypeID].Trim().Replace(delimiter, " ");

                    if (!emplByTypes.ContainsKey(type))
                        emplByTypes.Add(type, 0);

                    emplByTypes[type]++;

                    line += delimiter + type;
                    string wuName = "";

                    if (wuDict.ContainsKey(emplDict[id].WorkingUnitID))
                        wuName = wuDict[emplDict[id].WorkingUnitID].Name.Replace(delimiter, " ");

                    line += delimiter + wuName.Trim();

                    if (chbEmplTypeSum.Checked)
                    {
                        if (!sumByEmplTypes.ContainsKey(type))
                            sumByEmplTypes.Add(type, new Dictionary<int, int>());
                        if (!sumPaidLeaves.ContainsKey(type))
                            sumPaidLeaves.Add(type, 0);
                        if (!sumSickLeaves.ContainsKey(type))
                            sumSickLeaves.Add(type, 0);
                        if (!sumUnpaid.ContainsKey(type))
                            sumUnpaid.Add(type, 0);
                        if (!sumWorkingNorm.ContainsKey(type))
                            sumWorkingNorm.Add(type, 0);
                        if (!sumWorkOnSaturdays.ContainsKey(type))
                            sumWorkOnSaturdays.Add(type, 0);
                        if (!sumWorkOnSundays.ContainsKey(type))
                            sumWorkOnSundays.Add(type, 0);
                        if (!sumWorkOnSaturdaysBH.ContainsKey(type))
                            sumWorkOnSaturdaysBH.Add(type, 0);
                        if (!sumWorkOnSundaysBH.ContainsKey(type))
                            sumWorkOnSundaysBH.Add(type, 0);                        
                        if (!sumTotal.ContainsKey(type))
                            sumTotal.Add(type, 0);
                        if (!sumMeals.ContainsKey(type))
                            sumMeals.Add(type, 0);
                    }

                    Dictionary<int, int> emplPTDuration = new Dictionary<int, int>();
                    if (emplPTDurationSummary.ContainsKey(id))
                        emplPTDuration = emplPTDurationSummary[id];

                    int total = 0;
                    foreach (int pt in sickLeavePT)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    line += delimiter + getHours(emplSickLeavesSummary, id, true);
                    total += getMinutes(emplSickLeavesSummary, id);

                    if (chbEmplTypeSum.Checked)
                    {
                        sumSickLeaves[type] += getMinutes(emplSickLeavesSummary, id);
                        totalSickLeaves += getMinutes(emplSickLeavesSummary, id);
                    }

                    foreach (int pt in annualLeaves)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);
                        total += getMinutes(emplPTDuration, pt);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    foreach (int pt in personalHolidays)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);
                        total += getMinutes(emplPTDuration, pt);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    foreach (int pt in workOnHolidays)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    foreach (int pt in holidays)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);
                        total += getMinutes(emplPTDuration, pt);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    foreach (int pt in officialTrip)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);
                        total += getMinutes(emplPTDuration, pt);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }
                                        
                    line += delimiter + getHours(emplPaidLeavesSummary, id, true);
                    total += getMinutes(emplPaidLeavesSummary, id);

                    if (chbEmplTypeSum.Checked)
                    {
                        sumPaidLeaves[type] += getMinutes(emplPaidLeavesSummary, id);
                        totalPaidLeaves += getMinutes(emplPaidLeavesSummary, id);
                    }

                    foreach (int pt in regularWork)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);
                        total += getMinutes(emplPTDuration, pt);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    foreach (int pt in nightWork)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }
                                        
                    line += delimiter + getHours(emplWorkOnSaturdaysSummary, id, true);
                    line += delimiter + getHours(emplWorkOnSundaysSummary, id, true);

                    if (chbEmplTypeSum.Checked)
                    {
                        sumWorkOnSaturdays[type] += getMinutes(emplWorkOnSaturdaysSummary, id);
                        sumWorkOnSundays[type] += getMinutes(emplWorkOnSundaysSummary, id);
                        totalWorkOnSaturdays += getMinutes(emplWorkOnSaturdaysSummary, id);
                        totalWorkOnSundays += getMinutes(emplWorkOnSundaysSummary, id);
                    }

                    //line += delimiter + ((decimal)total / 60).ToString(Constants.doubleFormat).Trim();
                    line += delimiter + getHours(total, true).Trim();

                    if (chbEmplTypeSum.Checked)
                    {
                        sumTotal[type] += total;
                        totalTotal += total;
                    }
                    
                    foreach (int pt in turnus)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    line += delimiter + getHours(emplUnpaidSummary, id, true);

                    if (chbEmplTypeSum.Checked)
                    {
                        sumUnpaid[type] += getMinutes(emplUnpaidSummary, id);
                        totalUnpaid += getMinutes(emplUnpaidSummary, id);
                    }

                    foreach (int pt in overtimePaid)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }
                                        
                    //line += delimiter + ((decimal)(total + getMinutes(emplUnpaidSummary, id)) / 60).ToString(Constants.doubleFormat).Trim();
                    line += delimiter + getHours(total + getMinutes(emplUnpaidSummary, id), true).Trim();

                    line += delimiter + getHours(emplWorkingNormSummary, id, true);

                    if (chbEmplTypeSum.Checked)
                    {
                        sumWorkingNorm[type] += getMinutes(emplWorkingNormSummary, id);
                        totalWorkingNorm += getMinutes(emplWorkingNormSummary, id);
                    }

                    foreach (int pt in overtimeRejected)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    foreach (int pt in delays)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    foreach (int pt in bankHours)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    string amount = "";

                    if (ascoDict.ContainsKey(id))
                        amount = ascoDict[id].NVarcharValue6.Trim();

                    if (amount.Trim().Equals(""))
                        amount = "0.00";

                    line += delimiter + amount.Replace(delimiter, ".");

                    line += delimiter + getHours(emplWorkOnSaturdaysBHSummary, id, true);
                    line += delimiter + getHours(emplWorkOnSundaysBHSummary, id, true);

                    if (chbEmplTypeSum.Checked)
                    {
                        sumWorkOnSaturdaysBH[type] += getMinutes(emplWorkOnSaturdaysBHSummary, id);
                        sumWorkOnSundaysBH[type] += getMinutes(emplWorkOnSundaysBHSummary, id);
                        totalWorkOnSaturdaysBH += getMinutes(emplWorkOnSaturdaysBHSummary, id);
                        totalWorkOnSundaysBH += getMinutes(emplWorkOnSundaysBHSummary, id);
                    }

                    int mealNum = 0;
                    if (emplMeals.ContainsKey(id))
                        mealNum = emplMeals[id];

                    line += delimiter + mealNum.ToString().Trim();

                    if (chbEmplTypeSum.Checked)
                    {
                        sumMeals[type] += mealNum;
                        totalMeals += mealNum;
                    }

                    //string prodLine = "";
                    //if (ascoDict.ContainsKey(id))
                    //    prodLine = ascoDict[id].NVarcharValue7.Trim();

                    //line += delimiter + prodLine.Trim();

                    lines.Add(line);
                }

                if (chbEmplTypeSum.Checked)
                {
                    lines.Add("");

                    lines.Add("" + delimiter + "" + delimiter + "" + delimiter + "" + delimiter + "" + typeHeader + delimiter + "" + delimiter + rm.GetString("hdrSaturdayBH", culture) 
                        + delimiter + rm.GetString("hdrSundayBH", culture) + delimiter + rm.GetString("hdrMeals", culture));

                    lines.AddRange(generateSumByEmplTypes(true, sumByEmplTypes, sumPaidLeaves, sumSickLeaves, sumTotal, sumUnpaid, sumWorkingNorm, sumWorkOnSaturdays, sumWorkOnSundays, sumWorkOnSaturdaysBH, sumWorkOnSundaysBH, 
                        sumMeals, totalByEmplTypes, totalPaidLeaves, totalSickLeaves, totalTotal, totalUnpaid, totalWorkingNorm, totalWorkOnSaturdays, totalWorkOnSundays, totalWorkOnSaturdaysBH, totalWorkOnSundaysBH, totalMeals, 
                        annualLeaves, personalHolidays, workOnHolidays, holidays, officialTrip, regularWork, nightWork, turnus, overtimePaid, overtimeRejected, delays, bankHours, sickLeavePT, emplByTypes));
                }
                
                string reportName = "MonthlyReportSummary_" + DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss");

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = reportName;
                sfd.InitialDirectory = Constants.csvDocPath;
                sfd.Filter = "CSV (*.csv)|*.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;

                    string filePath = sfd.FileName;
                    sfd.Dispose();

                    FileStream stream = new FileStream(filePath, FileMode.Append);
                    stream.Close();
                                        
                    StreamWriter writer = new StreamWriter(filePath, true, Encoding.Unicode);
                    // insert header
                    writer.WriteLine(header);

                    foreach (string line in lines)
                    {
                        writer.WriteLine(line);
                    }

                    writer.Close();
                }                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.generateReportSummary(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void generateReportAnalytical(Dictionary<int, EmployeeTO> emplDict, Dictionary<int, EmployeeAsco4TO> ascoDict, Dictionary<int, Dictionary<string, RuleTO>> emplRules,
            Dictionary<int, Dictionary<DateTime, Dictionary<int, int>>> emplAllPTDuration, Dictionary<int, Dictionary<DateTime, int>> emplWorkOnSundays,
            Dictionary<int, Dictionary<DateTime, int>> emplWorkOnSaturdays, Dictionary<int, Dictionary<DateTime, int>> emplWorkOnSundaysBH, Dictionary<int, Dictionary<DateTime, int>> emplWorkOnSaturdaysBH, 
            Dictionary<int, Dictionary<DateTime, int>> emplSickLeaves, Dictionary<int, Dictionary<DateTime, int>> emplPaidLeaves, Dictionary<int, Dictionary<DateTime, int>> emplUnpaid, 
            Dictionary<int, Dictionary<DateTime, int>> emplWorkingNorm, Dictionary<int, Dictionary<int, int>> emplPTDurationSummary, Dictionary<int, int> emplWorkOnSundaysSummary, Dictionary<int, int> emplWorkOnSaturdaysSummary,
            Dictionary<int, int> emplWorkOnSundaysBHSummary, Dictionary<int, int> emplWorkOnSaturdaysBHSummary,
            Dictionary<int, int> emplSickLeavesSummary, Dictionary<int, int> emplPaidLeavesSummary, Dictionary<int, int> emplUnpaidSummary,
            Dictionary<int, int> emplWorkingNormSummary, List<int> sickLeavePT, Dictionary<int, List<DateTime>> emplDayMeals, Dictionary<int, int> emplMeals)
        {
            try
            {
                Common.Rule rule = new Common.Rule();
                List<int> annualLeaves = new List<int>();
                List<int> personalHolidays = new List<int>();
                List<int> workOnHolidays = new List<int>();
                List<int> holidays = new List<int>();
                List<int> officialTrip = new List<int>();
                List<int> regularWork = new List<int>();
                List<int> nightWork = new List<int>();
                List<int> turnus = new List<int>();
                List<int> overtimePaid = new List<int>();
                List<int> overtimeRejected = new List<int>();
                List<int> delays = new List<int>();
                List<int> bankHours = new List<int>();

                // sum by employee types
                Dictionary<string, Dictionary<int, int>> sumByEmplTypes = new Dictionary<string, Dictionary<int, int>>();
                Dictionary<string, int> sumWorkOnSundays = new Dictionary<string, int>();
                Dictionary<string, int> sumWorkOnSaturdays = new Dictionary<string, int>();
                Dictionary<string, int> sumWorkOnSundaysBH = new Dictionary<string, int>();
                Dictionary<string, int> sumWorkOnSaturdaysBH = new Dictionary<string, int>();
                Dictionary<string, int> sumSickLeaves = new Dictionary<string, int>();
                Dictionary<string, int> sumPaidLeaves = new Dictionary<string, int>();
                Dictionary<string, int> sumUnpaid = new Dictionary<string, int>();
                Dictionary<string, int> sumWorkingNorm = new Dictionary<string, int>();
                Dictionary<string, int> sumTotal = new Dictionary<string, int>();
                Dictionary<string, int> sumMeals = new Dictionary<string, int>();

                // total sum
                Dictionary<int, int> totalByEmplTypes = new Dictionary<int, int>();
                int totalWorkOnSundays = 0;
                int totalWorkOnSaturdays = 0;
                int totalWorkOnSundaysBH = 0;
                int totalWorkOnSaturdaysBH = 0;
                int totalSickLeaves = 0;
                int totalPaidLeaves = 0;
                int totalUnpaid = 0;
                int totalWorkingNorm = 0;
                int totalTotal = 0;
                int totalMeals = 0;

                foreach (int emplID in emplRules.Keys)
                {
                    if (emplRules[emplID].ContainsKey(Constants.RuleCompanyAnnualLeave) && !annualLeaves.Contains(emplRules[emplID][Constants.RuleCompanyAnnualLeave].RuleValue))
                        annualLeaves.Add(emplRules[emplID][Constants.RuleCompanyAnnualLeave].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RulePersonalHolidayPassType) && !personalHolidays.Contains(emplRules[emplID][Constants.RulePersonalHolidayPassType].RuleValue))
                        personalHolidays.Add(emplRules[emplID][Constants.RulePersonalHolidayPassType].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleWorkOnHolidayPassType) && !workOnHolidays.Contains(emplRules[emplID][Constants.RuleWorkOnHolidayPassType].RuleValue))
                        workOnHolidays.Add(emplRules[emplID][Constants.RuleWorkOnHolidayPassType].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleHolidayPassType) && !holidays.Contains(emplRules[emplID][Constants.RuleHolidayPassType].RuleValue))
                        holidays.Add(emplRules[emplID][Constants.RuleHolidayPassType].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleCompanyOfficialTrip) && !officialTrip.Contains(emplRules[emplID][Constants.RuleCompanyOfficialTrip].RuleValue))
                        officialTrip.Add(emplRules[emplID][Constants.RuleCompanyOfficialTrip].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleCompanyRegularWork) && !regularWork.Contains(emplRules[emplID][Constants.RuleCompanyRegularWork].RuleValue))
                        regularWork.Add(emplRules[emplID][Constants.RuleCompanyRegularWork].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleNightWork) && !nightWork.Contains(emplRules[emplID][Constants.RuleNightWork].RuleValue))
                        nightWork.Add(emplRules[emplID][Constants.RuleNightWork].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleComanyRotaryShift) && !turnus.Contains(emplRules[emplID][Constants.RuleComanyRotaryShift].RuleValue))
                        turnus.Add(emplRules[emplID][Constants.RuleComanyRotaryShift].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleCompanyOvertimePaid) && !overtimePaid.Contains(emplRules[emplID][Constants.RuleCompanyOvertimePaid].RuleValue))
                        overtimePaid.Add(emplRules[emplID][Constants.RuleCompanyOvertimePaid].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleCompanyOvertimeRejected) && !overtimeRejected.Contains(emplRules[emplID][Constants.RuleCompanyOvertimeRejected].RuleValue))
                        overtimeRejected.Add(emplRules[emplID][Constants.RuleCompanyOvertimeRejected].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleCompanyDelay) && !delays.Contains(emplRules[emplID][Constants.RuleCompanyDelay].RuleValue))
                        delays.Add(emplRules[emplID][Constants.RuleCompanyDelay].RuleValue);
                    if (emplRules[emplID].ContainsKey(Constants.RuleCompanyBankHour) && !bankHours.Contains(emplRules[emplID][Constants.RuleCompanyBankHour].RuleValue))
                        bankHours.Add(emplRules[emplID][Constants.RuleCompanyBankHour].RuleValue);
                }

                // create header
                string header = rm.GetString("hdrEmplID", culture) + delimiter + rm.GetString("hdrLastName", culture) + delimiter + rm.GetString("hdrFirstName", culture)
                    + delimiter + rm.GetString("hdrType", culture) + delimiter + rm.GetString("hdrWorkingUnit", culture) + delimiter + rm.GetString("hdrDate", culture);
                
                string typeHeader = generateTypeHeader(annualLeaves, personalHolidays, workOnHolidays, holidays, officialTrip, regularWork, nightWork, turnus, overtimePaid,
                    overtimeRejected, delays, bankHours, sickLeavePT);

                header += typeHeader + delimiter + rm.GetString("hdrAmount", culture) + delimiter + rm.GetString("hdrSaturdayBH", culture) + delimiter + rm.GetString("hdrSundayBH", culture)
                    + delimiter + rm.GetString("hdrMeals", culture);// +delimiter + rm.GetString("hdrProductionLine", culture);
                
                List<string> lines = new List<string>();
                Dictionary<string, int> emplByTypes = new Dictionary<string, int>();
                // create file lines
                string emptyLinePart = "" + delimiter + "" + delimiter + "" + delimiter + "" + delimiter + "";
                foreach (int id in emplDict.Keys)
                {
                    // add employee header line
                    string line = id.ToString().Trim().PadLeft(4, '0') + delimiter + emplDict[id].LastName.Trim().Replace(delimiter, " ") + delimiter + emplDict[id].FirstName.Trim().Replace(delimiter, " ");
                                        
                    string type = "";

                    int company = Common.Misc.getRootWorkingUnit(emplDict[id].WorkingUnitID, wuDict);

                    if (emplTypesDict.ContainsKey(company) && emplTypesDict[company].ContainsKey(emplDict[id].EmployeeTypeID))
                        type = emplTypesDict[company][emplDict[id].EmployeeTypeID].Trim().Replace(delimiter, " ");

                    if (!emplByTypes.ContainsKey(type))
                        emplByTypes.Add(type, 0);

                    emplByTypes[type]++;

                    line += delimiter + type;
                    string wuName = "";

                    if (wuDict.ContainsKey(emplDict[id].WorkingUnitID))
                        wuName = wuDict[emplDict[id].WorkingUnitID].Name.Replace(delimiter, " ");

                    line += delimiter + wuName.Trim();

                    lines.Add(line);

                    // add line for each date of interval
                    DateTime currDate = dtpFrom.Value.Date;                    
                    while (currDate.Date <= dtpTo.Value.Date)
                    {
                        int total = 0;                        
                        line = emptyLinePart + delimiter + currDate.Date.ToString(Constants.dateFormat);

                        Dictionary<DateTime, Dictionary<int, int>> emplPTDayDuration = new Dictionary<DateTime, Dictionary<int, int>>();
                        if (emplAllPTDuration.ContainsKey(id))
                            emplPTDayDuration = emplAllPTDuration[id];

                        foreach (int pt in sickLeavePT)
                        {
                            line += delimiter + getHours(emplPTDayDuration, pt, currDate, true);
                        }

                        line += delimiter + getHours(emplSickLeaves, id, currDate, true);
                        total += getMinutes(emplSickLeaves, id, currDate);

                        foreach (int pt in annualLeaves)
                        {
                            line += delimiter + getHours(emplPTDayDuration, pt, currDate, true);
                            total += getMinutes(emplPTDayDuration, pt, currDate);
                        }

                        foreach (int pt in personalHolidays)
                        {
                            line += delimiter + getHours(emplPTDayDuration, pt, currDate, true);
                            total += getMinutes(emplPTDayDuration, pt, currDate);
                        }

                        foreach (int pt in workOnHolidays)
                        {
                            line += delimiter + getHours(emplPTDayDuration, pt, currDate, true);
                        }

                        foreach (int pt in holidays)
                        {
                            line += delimiter + getHours(emplPTDayDuration, pt, currDate, true);
                            total += getMinutes(emplPTDayDuration, pt, currDate);
                        }

                        foreach (int pt in officialTrip)
                        {
                            line += delimiter + getHours(emplPTDayDuration, pt, currDate, true);
                            total += getMinutes(emplPTDayDuration, pt, currDate);
                        }

                        line += delimiter + getHours(emplPaidLeaves, id, currDate, true);
                        total += getMinutes(emplPaidLeaves, id, currDate);

                        foreach (int pt in regularWork)
                        {
                            line += delimiter + getHours(emplPTDayDuration, pt, currDate, true);
                            total += getMinutes(emplPTDayDuration, pt, currDate);
                        }

                        foreach (int pt in nightWork)
                        {
                            line += delimiter + getHours(emplPTDayDuration, pt, currDate, true);
                        }

                        line += delimiter + getHours(emplWorkOnSaturdays, id, currDate, true);
                        line += delimiter + getHours(emplWorkOnSundays, id, currDate, true);
                                                
                        //line += delimiter + ((decimal)total / 60).ToString(Constants.doubleFormat).Trim();
                        line += delimiter + getHours(total, true).Trim();

                        foreach (int pt in turnus)
                        {
                            line += delimiter + getHours(emplPTDayDuration, pt, currDate, true);
                        }

                        line += delimiter + getHours(emplUnpaid, id, currDate, true);

                        foreach (int pt in overtimePaid)
                        {
                            line += delimiter + getHours(emplPTDayDuration, pt, currDate, true);
                        }

                        //line += delimiter + ((decimal)(total + getMinutes(emplUnpaid, id, currDate)) / 60).ToString(Constants.doubleFormat).Trim();
                        line += delimiter + getHours(total + getMinutes(emplUnpaid, id, currDate), true).Trim();

                        line += delimiter + getHours(emplWorkingNorm, id, currDate, true);

                        foreach (int pt in overtimeRejected)
                        {
                            line += delimiter + getHours(emplPTDayDuration, pt, currDate, true);
                        }

                        foreach (int pt in delays)
                        {
                            line += delimiter + getHours(emplPTDayDuration, pt, currDate, true);
                        }

                        foreach (int pt in bankHours)
                        {
                            line += delimiter + getHours(emplPTDayDuration, pt, currDate, true);
                        }

                        line += delimiter + "";

                        line += delimiter + getHours(emplWorkOnSaturdaysBH, id, currDate, true);
                        line += delimiter + getHours(emplWorkOnSundaysBH, id, currDate, true);

                        if (emplDayMeals.ContainsKey(id) && emplDayMeals[id].Contains(currDate.Date))
                            line += delimiter + "1";
                        else
                            line += delimiter + "0";

                        lines.Add(line);

                        currDate = currDate.Date.AddDays(1);
                    }

                    // add total line
                    line = emptyLinePart + delimiter + rm.GetString("hdrTotal", culture);

                    if (chbEmplTypeSum.Checked)
                    {
                        if (!sumByEmplTypes.ContainsKey(type))
                            sumByEmplTypes.Add(type, new Dictionary<int, int>());
                        if (!sumPaidLeaves.ContainsKey(type))
                            sumPaidLeaves.Add(type, 0);
                        if (!sumSickLeaves.ContainsKey(type))
                            sumSickLeaves.Add(type, 0);
                        if (!sumUnpaid.ContainsKey(type))
                            sumUnpaid.Add(type, 0);
                        if (!sumWorkingNorm.ContainsKey(type))
                            sumWorkingNorm.Add(type, 0);
                        if (!sumWorkOnSaturdays.ContainsKey(type))
                            sumWorkOnSaturdays.Add(type, 0);
                        if (!sumWorkOnSundays.ContainsKey(type))
                            sumWorkOnSundays.Add(type, 0);
                        if (!sumWorkOnSaturdaysBH.ContainsKey(type))
                            sumWorkOnSaturdaysBH.Add(type, 0);
                        if (!sumWorkOnSundaysBH.ContainsKey(type))
                            sumWorkOnSundaysBH.Add(type, 0);
                        if (!sumTotal.ContainsKey(type))
                            sumTotal.Add(type, 0);
                        if (!sumMeals.ContainsKey(type))
                            sumMeals.Add(type, 0);
                    }

                    Dictionary<int, int> emplPTDuration = new Dictionary<int, int>();
                    if (emplPTDurationSummary.ContainsKey(id))
                        emplPTDuration = emplPTDurationSummary[id];

                    int emplTotal = 0;
                    foreach (int pt in sickLeavePT)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    line += delimiter + getHours(emplSickLeavesSummary, id, true);
                    emplTotal += getMinutes(emplSickLeavesSummary, id);

                    if (chbEmplTypeSum.Checked)
                    {
                        sumSickLeaves[type] += getMinutes(emplSickLeavesSummary, id);
                        totalSickLeaves += getMinutes(emplSickLeavesSummary, id);
                    }

                    foreach (int pt in annualLeaves)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);
                        emplTotal += getMinutes(emplPTDuration, pt);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    foreach (int pt in personalHolidays)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);
                        emplTotal += getMinutes(emplPTDuration, pt);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    foreach (int pt in workOnHolidays)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    foreach (int pt in holidays)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);
                        emplTotal += getMinutes(emplPTDuration, pt);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    foreach (int pt in officialTrip)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);
                        emplTotal += getMinutes(emplPTDuration, pt);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    line += delimiter + getHours(emplPaidLeavesSummary, id, true);
                    emplTotal += getMinutes(emplPaidLeavesSummary, id);

                    if (chbEmplTypeSum.Checked)
                    {
                        sumPaidLeaves[type] += getMinutes(emplPaidLeavesSummary, id);
                        totalPaidLeaves += getMinutes(emplPaidLeavesSummary, id);
                    }

                    foreach (int pt in regularWork)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);
                        emplTotal += getMinutes(emplPTDuration, pt);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    foreach (int pt in nightWork)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    line += delimiter + getHours(emplWorkOnSaturdaysSummary, id, true);
                    line += delimiter + getHours(emplWorkOnSundaysSummary, id, true);
                    
                    if (chbEmplTypeSum.Checked)
                    {
                        sumWorkOnSaturdays[type] += getMinutes(emplWorkOnSaturdaysSummary, id);
                        sumWorkOnSundays[type] += getMinutes(emplWorkOnSundaysSummary, id);
                        totalWorkOnSaturdays += getMinutes(emplWorkOnSaturdaysSummary, id);
                        totalWorkOnSundays += getMinutes(emplWorkOnSundaysSummary, id);
                    }

                    //line += delimiter + ((decimal)emplTotal / 60).ToString(Constants.doubleFormat).Trim();
                    line += delimiter + getHours(emplTotal, true).Trim();

                    if (chbEmplTypeSum.Checked)
                    {
                        sumTotal[type] += emplTotal;
                        totalTotal += emplTotal;
                    }                    

                    foreach (int pt in turnus)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    line += delimiter + getHours(emplUnpaidSummary, id, true);

                    if (chbEmplTypeSum.Checked)
                    {
                        sumUnpaid[type] += getMinutes(emplUnpaidSummary, id);
                        totalUnpaid += getMinutes(emplUnpaidSummary, id);
                    }

                    foreach (int pt in overtimePaid)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    //line += delimiter + ((decimal)(emplTotal + getMinutes(emplUnpaidSummary, id)) / 60).ToString(Constants.doubleFormat).Trim();
                    line += delimiter + getHours(emplTotal + getMinutes(emplUnpaidSummary, id), true).Trim();

                    line += delimiter + getHours(emplWorkingNormSummary, id, true);

                    if (chbEmplTypeSum.Checked)
                    {
                        sumWorkingNorm[type] += getMinutes(emplWorkingNormSummary, id);
                        totalWorkingNorm += getMinutes(emplWorkingNormSummary, id);
                    }

                    foreach (int pt in overtimeRejected)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    foreach (int pt in delays)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    foreach (int pt in bankHours)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (chbEmplTypeSum.Checked)
                        {
                            if (!sumByEmplTypes[type].ContainsKey(pt))
                                sumByEmplTypes[type].Add(pt, 0);

                            sumByEmplTypes[type][pt] += getMinutes(emplPTDuration, pt);

                            if (!totalByEmplTypes.ContainsKey(pt))
                                totalByEmplTypes.Add(pt, 0);

                            totalByEmplTypes[pt] += getMinutes(emplPTDuration, pt);
                        }
                    }

                    string amount = "";

                    if (ascoDict.ContainsKey(id))
                        amount = ascoDict[id].NVarcharValue6.Trim();

                    if (amount.Trim().Equals(""))
                        amount = "0.00";

                    line += delimiter + amount.Replace(delimiter, ".");

                    line += delimiter + getHours(emplWorkOnSaturdaysBHSummary, id, true);
                    line += delimiter + getHours(emplWorkOnSundaysBHSummary, id, true);

                    if (chbEmplTypeSum.Checked)
                    {
                        sumWorkOnSaturdaysBH[type] += getMinutes(emplWorkOnSaturdaysBHSummary, id);
                        sumWorkOnSundaysBH[type] += getMinutes(emplWorkOnSundaysBHSummary, id);
                        totalWorkOnSaturdaysBH += getMinutes(emplWorkOnSaturdaysBHSummary, id);
                        totalWorkOnSundaysBH += getMinutes(emplWorkOnSundaysBHSummary, id);
                    }

                    int mealNum = 0;
                    if (emplMeals.ContainsKey(id))
                        mealNum = emplMeals[id];
                                      
                    line += delimiter + mealNum.ToString().Trim();

                    if (chbEmplTypeSum.Checked)
                    {
                        sumMeals[type] += mealNum;
                        totalMeals += mealNum;
                    }

                    //string prodLine = "";
                    //if (ascoDict.ContainsKey(id))
                    //    prodLine = ascoDict[id].NVarcharValue7.Trim();
                    //line += delimiter + prodLine.Trim();

                    lines.Add(line);
                }

                if (chbEmplTypeSum.Checked)
                {
                    lines.Add("");
                    
                    lines.Add("" + delimiter + "" + delimiter + "" + delimiter + "" + delimiter + "" + delimiter + "" + typeHeader + delimiter + "" + delimiter + rm.GetString("hdrSaturdayBH", culture)
                        + delimiter + rm.GetString("hdrSundayBH", culture) + delimiter + rm.GetString("hdrMeals", culture));

                    lines.AddRange(generateSumByEmplTypes(false, sumByEmplTypes, sumPaidLeaves, sumSickLeaves, sumTotal, sumUnpaid, sumWorkingNorm, sumWorkOnSaturdays, sumWorkOnSundays, sumWorkOnSaturdaysBH, sumWorkOnSundaysBH,
                        sumMeals, totalByEmplTypes, totalPaidLeaves, totalSickLeaves, totalTotal, totalUnpaid, totalWorkingNorm, totalWorkOnSaturdays, totalWorkOnSundays, totalWorkOnSaturdaysBH, totalWorkOnSundaysBH, totalMeals, 
                        annualLeaves, personalHolidays, workOnHolidays, holidays, officialTrip, regularWork, nightWork, turnus, overtimePaid, overtimeRejected, delays, bankHours, sickLeavePT, emplByTypes));
                }

                string reportName = "MonthlyReportAnalytical_" + DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss");

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = reportName;
                sfd.InitialDirectory = Constants.csvDocPath;
                sfd.Filter = "CSV (*.csv)|*.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;

                    string filePath = sfd.FileName;
                    sfd.Dispose();

                    FileStream stream = new FileStream(filePath, FileMode.Append);
                    stream.Close();

                    StreamWriter writer = new StreamWriter(filePath, true, Encoding.Unicode);
                    // insert header
                    writer.WriteLine(header);

                    foreach (string line in lines)
                    {
                        writer.WriteLine(line);
                    }

                    writer.Close();
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.generateReportAnalytical(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private string generateTypeHeader(List<int> annualLeaves, List<int> personalHolidays, List<int> workOnHolidays, List<int> holidays, List<int> officialTrip, 
            List<int> regularWork, List<int> nightWork, List<int> turnus, List<int> overtimePaid, List<int> overtimeRejected, List<int> delays, List<int> bankHours, List<int> sickLeavePT)
        {
            try
            {
                string header = "";

                foreach (int pt in sickLeavePT)
                {
                    header += delimiter + getPTDesc(pt);
                }
                header += delimiter + rm.GetString("hdrSickLeave", culture);
                foreach (int pt in annualLeaves)
                {
                    header += delimiter + getPTDesc(pt);
                }
                foreach (int pt in personalHolidays)
                {
                    header += delimiter + getPTDesc(pt);
                }
                foreach (int pt in workOnHolidays)
                {
                    header += delimiter + getPTDesc(pt);
                }
                foreach (int pt in holidays)
                {
                    header += delimiter + getPTDesc(pt);
                }
                foreach (int pt in officialTrip)
                {
                    header += delimiter + getPTDesc(pt);
                }
                header += delimiter + rm.GetString("hdrPaid", culture);
                foreach (int pt in regularWork)
                {
                    header += delimiter + getPTDesc(pt);
                }
                foreach (int pt in nightWork)
                {
                    header += delimiter + getPTDesc(pt);
                }
                header += delimiter + rm.GetString("hdrSaturdayWork", culture) + delimiter + rm.GetString("hdrSundayWork", culture) + delimiter + rm.GetString("hdrTotal", culture);
                foreach (int pt in turnus)
                {
                    header += delimiter + getPTDesc(pt);
                }
                header += delimiter + rm.GetString("hdrUnpaid", culture);
                foreach (int pt in overtimePaid)
                {
                    header += delimiter + getPTDesc(pt);
                }
                header += delimiter + rm.GetString("hdrTotalPlusUnpad", culture) + delimiter + rm.GetString("hdrWorkingNorm", culture);
                foreach (int pt in overtimeRejected)
                {
                    header += delimiter + getPTDesc(pt);
                }
                foreach (int pt in delays)
                {
                    header += delimiter + getPTDesc(pt);
                }
                foreach (int pt in bankHours)
                {
                    header += delimiter + getPTDesc(pt);
                }

                return header;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.generateTypeHeader(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private List<string> generateSumByEmplTypes(bool isSummary, Dictionary<string, Dictionary<int, int>> sumByEmplTypes, Dictionary<string, int> sumPaidLeaves,
            Dictionary<string, int> sumSickLeaves, Dictionary<string, int> sumTotal, Dictionary<string, int> sumUnpaid, Dictionary<string, int> sumWorkingNorm,
            Dictionary<string, int> sumWorkOnSaturdays, Dictionary<string, int> sumWorkOnSundays, Dictionary<string, int> sumWorkOnSaturdaysBH, Dictionary<string, int> sumWorkOnSundaysBH, Dictionary<string, int> sumMeals,
            Dictionary<int, int> totalByEmplTypes, int totalPaidLeaves, int totalSickLeaves, int totalTotal, int totalUnpaid, int totalWorkingNorm, int totalWorkOnSaturdays, int totalWorkOnSundays, int totalWorkOnSaturdaysBH, 
            int totalWorkOnSundaysBH, int totalMeals, List<int> annualLeaves, List<int> personalHolidays, List<int> workOnHolidays, List<int> holidays, List<int> officialTrip, List<int> regularWork, List<int> nightWork, 
            List<int> turnus, List<int> overtimePaid, List<int> overtimeRejected, List<int> delays, List<int> bankHours, List<int> sickLeavePT, Dictionary<string, int> emplByTypes)
        {
            try
            {
                List<string> lines = new List<string>();                
                Dictionary<string, string> typeLines = new Dictionary<string, string>();
                string emptyLinePart = "" + delimiter + "" + delimiter + "";

                if (!isSummary)
                    emptyLinePart += delimiter + "";

                string totalLine = "";
                string rateLine = "";
                
                List<string> allTypes = new List<string>();
                foreach (int company in emplTypesDict.Keys)
                {
                    foreach (int typeID in emplTypesDict[company].Keys)
                    {
                        if (typeID != (int)Constants.EmployeeTypesWN.MANAGEMENT && !allTypes.Contains(emplTypesDict[company][typeID]))
                            allTypes.Add(emplTypesDict[company][typeID]);
                    }
                }

                // calculate total line just once
                bool addTotal = true;
                int totalEmployees = 0;
                foreach (string type in allTypes)
                {
                    int numEmployees = 0;
                    if (emplByTypes.ContainsKey(type))
                        numEmployees = emplByTypes[type];

                    totalEmployees += numEmployees;

                    string line = emptyLinePart + delimiter + type + delimiter + numEmployees.ToString().Trim();

                    Dictionary<int, int> emplPTDuration = new Dictionary<int, int>();
                    if (sumByEmplTypes.ContainsKey(type))
                        emplPTDuration = sumByEmplTypes[type];
                                        
                    foreach (int pt in sickLeavePT)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (addTotal)
                        {
                            totalLine += delimiter + getHours(totalByEmplTypes, pt, true);
                            rateLine += delimiter + getRate(getMinutes(totalByEmplTypes, pt), totalWorkingNorm);
                        }
                    }

                    line += delimiter + getHours(sumSickLeaves, type, true);

                    if (addTotal)
                    {
                        //totalLine += delimiter + ((decimal)totalSickLeaves / 60).ToString(Constants.doubleFormat).Trim();
                        totalLine += delimiter + getHours(totalSickLeaves, true).Trim();
                        rateLine += delimiter + getRate(totalSickLeaves, totalWorkingNorm);
                    }

                    foreach (int pt in annualLeaves)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (addTotal)
                        {
                            totalLine += delimiter + getHours(totalByEmplTypes, pt, true);
                            rateLine += delimiter + getRate(getMinutes(totalByEmplTypes, pt), totalWorkingNorm);
                        }
                    }

                    foreach (int pt in personalHolidays)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (addTotal)
                        {
                            totalLine += delimiter + getHours(totalByEmplTypes, pt, true);
                            rateLine += delimiter + getRate(getMinutes(totalByEmplTypes, pt), totalWorkingNorm);
                        }
                    }

                    foreach (int pt in workOnHolidays)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (addTotal)
                        {
                            totalLine += delimiter + getHours(totalByEmplTypes, pt, true);
                            rateLine += delimiter + getRate(getMinutes(totalByEmplTypes, pt), totalWorkingNorm);
                        }
                    }

                    foreach (int pt in holidays)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (addTotal)
                        {
                            totalLine += delimiter + getHours(totalByEmplTypes, pt, true);
                            rateLine += delimiter + getRate(getMinutes(totalByEmplTypes, pt), totalWorkingNorm);
                        }
                    }

                    foreach (int pt in officialTrip)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (addTotal)
                        {
                            totalLine += delimiter + getHours(totalByEmplTypes, pt, true);
                            rateLine += delimiter + getRate(getMinutes(totalByEmplTypes, pt), totalWorkingNorm);
                        }
                    }

                    line += delimiter + getHours(sumPaidLeaves, type, true);

                    if (addTotal)
                    {
                        //totalLine += delimiter + ((decimal)totalPaidLeaves / 60).ToString(Constants.doubleFormat).Trim();
                        totalLine += delimiter + getHours(totalPaidLeaves, true).Trim();
                        rateLine += delimiter + getRate(totalPaidLeaves, totalWorkingNorm);
                    }

                    foreach (int pt in regularWork)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (addTotal)
                        {
                            totalLine += delimiter + getHours(totalByEmplTypes, pt, true);
                            rateLine += delimiter + getRate(getMinutes(totalByEmplTypes, pt), totalWorkingNorm);                            
                        }
                    }

                    foreach (int pt in nightWork)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (addTotal)
                        {
                            totalLine += delimiter + getHours(totalByEmplTypes, pt, true);
                            rateLine += delimiter + getRate(getMinutes(totalByEmplTypes, pt), totalWorkingNorm);
                        }
                    }

                    line += delimiter + getHours(sumWorkOnSaturdays, type, true);
                    line += delimiter + getHours(sumWorkOnSundays, type, true);
                    line += delimiter + getHours(sumTotal, type, true);

                    if (addTotal)
                    {
                        //totalLine += delimiter + ((decimal)totalWorkOnSaturdays / 60).ToString(Constants.doubleFormat).Trim();
                        //totalLine += delimiter + ((decimal)totalWorkOnSundays / 60).ToString(Constants.doubleFormat).Trim();
                        //totalLine += delimiter + ((decimal)totalTotal / 60).ToString(Constants.doubleFormat).Trim();

                        totalLine += delimiter + getHours(totalWorkOnSaturdays, true).Trim();
                        totalLine += delimiter + getHours(totalWorkOnSundays, true).Trim();
                        totalLine += delimiter + getHours(totalTotal, true).Trim();

                        rateLine += delimiter + getRate(totalWorkOnSaturdays, totalWorkingNorm);
                        rateLine += delimiter + getRate(totalWorkOnSundays, totalWorkingNorm);
                        rateLine += delimiter + getRate(totalTotal, totalWorkingNorm);
                    }

                    foreach (int pt in turnus)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (addTotal)
                        {
                            totalLine += delimiter + getHours(totalByEmplTypes, pt, true);
                            rateLine += delimiter + getRate(getMinutes(totalByEmplTypes, pt), totalWorkingNorm);
                        }
                    }

                    line += delimiter + getHours(sumUnpaid, type, true);

                    if (addTotal)
                    {
                        //totalLine += delimiter + ((decimal)totalUnpaid / 60).ToString(Constants.doubleFormat).Trim();
                        totalLine += delimiter + getHours(totalUnpaid, true).Trim();
                        rateLine += delimiter + getRate(totalUnpaid, totalWorkingNorm);
                    }

                    foreach (int pt in overtimePaid)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (addTotal)
                        {
                            totalLine += delimiter + getHours(totalByEmplTypes, pt, true);
                            rateLine += delimiter + getRate(getMinutes(totalByEmplTypes, pt), totalWorkingNorm);
                        }
                    }

                    //line += delimiter + ((decimal)(getMinutes(sumTotal, type) + getMinutes(sumUnpaid, type)) / 60).ToString(Constants.doubleFormat).Trim();
                    line += delimiter + getHours(getMinutes(sumTotal, type) + getMinutes(sumUnpaid, type), true).Trim();

                    if (addTotal)
                    {
                        //totalLine += delimiter + ((decimal)(totalTotal + totalUnpaid) / 60).ToString(Constants.doubleFormat).Trim();
                        totalLine += delimiter + getHours(totalTotal + totalUnpaid, true).Trim();
                        rateLine += delimiter + getRate(totalTotal + totalUnpaid, totalWorkingNorm);
                    }

                    line += delimiter + getHours(sumWorkingNorm, type, true);

                    if (addTotal)
                    {
                        //totalLine += delimiter + ((decimal)totalWorkingNorm / 60).ToString(Constants.doubleFormat).Trim();
                        totalLine += delimiter + getHours(totalWorkingNorm, true).Trim();
                        rateLine += delimiter + getRate(totalWorkingNorm, totalWorkingNorm);
                    }

                    foreach (int pt in overtimeRejected)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (addTotal)
                        {
                            totalLine += delimiter + getHours(totalByEmplTypes, pt, true);
                            rateLine += delimiter + getRate(getMinutes(totalByEmplTypes, pt), totalWorkingNorm);
                        }
                    }

                    foreach (int pt in delays)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (addTotal)
                        {
                            totalLine += delimiter + getHours(totalByEmplTypes, pt, true);
                            rateLine += delimiter + getRate(getMinutes(totalByEmplTypes, pt), totalWorkingNorm);
                        }
                    }

                    foreach (int pt in bankHours)
                    {
                        line += delimiter + getHours(emplPTDuration, pt, true);

                        if (addTotal)
                        {
                            totalLine += delimiter + getHours(totalByEmplTypes, pt, true);
                            rateLine += delimiter + getRate(getMinutes(totalByEmplTypes, pt), totalWorkingNorm);
                        }
                    }

                    line += delimiter + "";

                    line += delimiter + getHours(sumWorkOnSaturdaysBH, type, true);
                    line += delimiter + getHours(sumWorkOnSundaysBH, type, true);

                    if (sumMeals.ContainsKey(type))
                        line += delimiter + sumMeals[type].ToString().Trim();
                    else
                        line += delimiter + "0";

                    if (addTotal)
                    {
                        totalLine += delimiter + "" + delimiter + getHours(totalWorkOnSaturdaysBH, true).Trim();
                        totalLine += delimiter + getHours(totalWorkOnSundaysBH, true).Trim();
                        totalLine += delimiter + totalMeals.ToString().Trim();
                     
                        rateLine += delimiter + "" + delimiter + getRate(totalWorkOnSaturdaysBH, totalWorkingNorm);
                        rateLine += delimiter + getRate(totalWorkOnSundaysBH, totalWorkingNorm);
                        rateLine += delimiter + "0";
                    }

                    typeLines.Add(type, line);
                    
                    if (addTotal)
                        addTotal = false;
                }

                // add rate line
                lines.Add(emptyLinePart + delimiter + rm.GetString("rate", culture) + delimiter + "" + rateLine);

                // total line already start with delimiter
                lines.Add(emptyLinePart + delimiter + rm.GetString("allTypes", culture) + delimiter + totalEmployees.ToString().Trim() + totalLine);

                foreach (string type in typeLines.Keys)
                {
                    lines.Add(typeLines[type]);
                }                

                return lines;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.generateSumByEmplTypes(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private string getPTDesc(int id)
        {
            try
            {
                string desc = "";
                if (ptDict.ContainsKey(id))
                {
                    if (NotificationController.GetLanguage().Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                        desc = ptDict[id].DescriptionAndID.Trim().Replace(delimiter, " ");
                    else
                        desc = ptDict[id].DescriptionAltAndID.Trim().Replace(delimiter, " ");
                }

                return desc;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.getPTDesc(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private string getHours(Dictionary<int, int> emplPTDuration, int id, bool doRounding)
        {
            try
            {
                string hours = "";
                if (emplPTDuration.ContainsKey(id) && emplPTDuration[id] != 0)
                {
                    if (!doRounding)
                        hours = ((decimal)emplPTDuration[id] / 60).ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    else if (emplPTDuration[id] % 60 != 0 && emplPTDuration[id] % 30 == 0)
                        hours = (emplPTDuration[id] / 60).ToString();
                    else
                        hours = Math.Round(((decimal)emplPTDuration[id] / 60), 0).ToString();
                }
                else
                    hours = "0";

                return hours;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.getHours(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private string getHours(Dictionary<string, int> emplPTDuration, string id, bool doRounding)
        {
            try
            {
                string hours = "";
                if (emplPTDuration.ContainsKey(id) && emplPTDuration[id] != 0)
                {
                    if (!doRounding)
                        hours = ((decimal)emplPTDuration[id] / 60).ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    else if (emplPTDuration[id] % 60 != 0 && emplPTDuration[id] % 30 == 0)
                        hours = (emplPTDuration[id] / 60).ToString();
                    else
                        hours = Math.Round(((decimal)emplPTDuration[id] / 60), 0).ToString();
                }
                else
                    hours = "0";

                return hours;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.getHours(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private string getHours(object dict, int id, DateTime date, bool doRounding)
        {
            try
            {
                string hours = "";

                if (dict is Dictionary<DateTime, Dictionary<int, int>>)
                {
                    Dictionary<DateTime, Dictionary<int, int>> emplPTDuration = (Dictionary<DateTime, Dictionary<int, int>>)dict;
                    if (emplPTDuration.ContainsKey(date.Date) && emplPTDuration[date.Date].ContainsKey(id) && emplPTDuration[date.Date][id] != 0)
                    {
                        if (!doRounding)
                            hours = ((decimal)emplPTDuration[date.Date][id] / 60).ToString(Constants.doubleFormat).Replace(delimiter, ".");
                        else if (emplPTDuration[date.Date][id] % 60 != 0 && emplPTDuration[date.Date][id] % 30 == 0)
                            hours = (emplPTDuration[date.Date][id] / 60).ToString();
                        else
                            hours = Math.Round(((decimal)emplPTDuration[date.Date][id] / 60), 0).ToString();
                    }
                    else
                        hours = "0";
                }
                else if (dict is Dictionary<int, Dictionary<DateTime, int>>)
                {
                    Dictionary<int, Dictionary<DateTime, int>> emplPTDuration = (Dictionary<int, Dictionary<DateTime, int>>)dict;
                    if (emplPTDuration.ContainsKey(id) && emplPTDuration[id].ContainsKey(date.Date) && emplPTDuration[id][date.Date] != 0)
                    {
                        if (!doRounding)
                            hours = ((decimal)emplPTDuration[id][date.Date] / 60).ToString(Constants.doubleFormat).Replace(delimiter, ".");
                        else if (emplPTDuration[id][date.Date] % 60 != 0 && emplPTDuration[id][date.Date] % 30 == 0)
                            hours = (emplPTDuration[id][date.Date] / 60).ToString();
                        else
                            hours = Math.Round(((decimal)emplPTDuration[id][date.Date] / 60), 0).ToString();
                    }
                    else
                        hours = "0";
                }

                return hours;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.getHours(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private string getHours(int minutes, bool doRounding)
        {
            try
            {
                string hours = "";

                if (!doRounding)
                    hours = ((decimal)minutes / 60).ToString(Constants.doubleFormat).Replace(delimiter, ".");
                else if (minutes % 60 != 0 && minutes % 30 == 0)
                    hours = (minutes / 60).ToString();
                else
                    hours = Math.Round(((decimal)minutes / 60), 0).ToString();
                
                return hours;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.getHours(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private string getRate(int part, int norm)
        {
            try
            {
                return (((decimal)part / norm) * 100).ToString(Constants.doubleFormat);                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.getRate(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private int getMinutes(Dictionary<int, int> emplPTDuration, int id)
        {
            try
            {
                int min = 0;
                if (emplPTDuration.ContainsKey(id) && emplPTDuration[id] != 0)
                    min = emplPTDuration[id];
                
                return min;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.getMinutes(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private int getMinutes(Dictionary<string, int> emplPTDuration, string id)
        {
            try
            {
                int min = 0;
                if (emplPTDuration.ContainsKey(id) && emplPTDuration[id] != 0)
                    min = emplPTDuration[id];

                return min;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.getMinutes(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private int getMinutes(object dict, int id, DateTime date)
        {
            try
            {
                int min = 0;

                if (dict is Dictionary<DateTime, Dictionary<int, int>>)
                {
                    Dictionary<DateTime, Dictionary<int, int>> emplPTDuration = (Dictionary<DateTime, Dictionary<int, int>>)dict;
                    if (emplPTDuration.ContainsKey(date.Date) && emplPTDuration[date.Date].ContainsKey(id) && emplPTDuration[date.Date][id] != 0)
                        min = emplPTDuration[date.Date][id];                    
                }
                else if (dict is Dictionary<int, Dictionary<DateTime, int>>)
                {
                    Dictionary<int, Dictionary<DateTime, int>> emplPTDuration = (Dictionary<int, Dictionary<DateTime, int>>)dict;
                    if (emplPTDuration.ContainsKey(id) && emplPTDuration[id].ContainsKey(date.Date) && emplPTDuration[id][date.Date] != 0)
                        min = emplPTDuration[id][date.Date];
                }

                return min;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.getMinutes(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isWorkOnHoliday(Dictionary<string, List<DateTime>> personalHolidayDays, List<DateTime> nationalHolidaysDays, List<DateTime> nationalHolidaysDaysSundays,
            List<HolidaysExtendedTO> nationalTransferableHolidays, List<DateTime> paidHolidaysDays, DateTime date, WorkTimeSchemaTO schema, EmployeeAsco4TO asco, EmployeeTO empl)
        {
            try
            {
                bool isHoliday = false;

                // turnus shift does not have work on holiday
                if (isTurnus(schema))
                    return isHoliday;
                                
                // personal holidays are work on holiday for everyone
                if ((personalHolidayDays.ContainsKey(asco.NVarcharValue1) && personalHolidayDays[asco.NVarcharValue1].Contains(date.Date))
                    || (asco.DatetimeValue1.Day == date.Day && asco.DatetimeValue1.Month == date.Month))
                    isHoliday = true;
                else if (schema.Type.Trim().ToUpper() == Constants.schemaTypeIndustrial.Trim().ToUpper())
                {
                    // industrial schema has work on holiday just for national holidays, no transfer of holidays
                    if (nationalHolidaysDays.Contains(date.Date))
                        isHoliday = true;
                }
                else
                {
                    // count work on holiday just once
                    if (nationalHolidaysDays.Contains(date.Date))
                        isHoliday = true;
                    else if (nationalHolidaysDaysSundays.Contains(date.Date))
                    {
                        // check if work is already counted on belonging national holiday
                        DateTime maxNationalHolidayEnd = new DateTime();
                        DateTime maxNationalHolidayStart = new DateTime();

                        foreach (HolidaysExtendedTO hol in nationalTransferableHolidays)
                        {
                            if (hol.DateEnd.Date > maxNationalHolidayEnd.Date && hol.DateEnd.Date < date.Date)
                            {
                                maxNationalHolidayStart = hol.DateStart.Date;
                                maxNationalHolidayEnd = hol.DateEnd.Date;
                            }
                        }

                        bool holidayCalculated = false;
                        if (!maxNationalHolidayStart.Equals(new DateTime()) && !maxNationalHolidayEnd.Equals(new DateTime()))
                        {
                            // check if work on holiday is already calculated for this holiday
                            DateTime currDate = maxNationalHolidayStart.Date;
                            while (currDate.Date <= maxNationalHolidayEnd.Date)
                            {
                                if (paidHolidaysDays.Contains(currDate.Date))
                                {
                                    holidayCalculated = true;
                                    break;
                                }

                                currDate = currDate.AddDays(1);
                            }
                        }

                        isHoliday = !holidayCalculated;
                    }
                }

                if (isHoliday)
                    paidHolidaysDays.Add(date.Date);

                return isHoliday;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.isWorkOnHoliday(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isTurnus(WorkTimeSchemaTO sch)
        {
            try
            {
                return sch.Turnus == Constants.yesInt;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.isTurnus(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private int nightWorkDuration(Dictionary<string, RuleTO> emplRules, IOPairProcessedTO pair, WorkTimeSchemaTO sch)
        {
            try
            {                
                int nightWork = 0;

                if (isTurnus(sch))
                    return nightWork;

                DateTime nightStart = new DateTime();
                DateTime nightEnd = new DateTime();

                if (emplRules.ContainsKey(Constants.RuleNightWork))
                {
                    nightStart = emplRules[Constants.RuleNightWork].RuleDateTime1;
                    nightEnd = emplRules[Constants.RuleNightWork].RuleDateTime2;
                }

                if (pair.StartTime.TimeOfDay < nightEnd.TimeOfDay)
                {
                    DateTime endNightWork = new DateTime();

                    if (pair.EndTime.TimeOfDay <= nightEnd.TimeOfDay)
                        endNightWork = pair.EndTime;
                    else
                        endNightWork = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day, nightEnd.Hour,
                            nightEnd.Minute, nightEnd.Second);

                    nightWork += (int)endNightWork.TimeOfDay.Subtract(pair.StartTime.TimeOfDay).TotalMinutes;

                    if (endNightWork.Hour == 23 && endNightWork.Minute == 59)
                        nightWork++;
                }
                if (pair.EndTime.TimeOfDay > nightStart.TimeOfDay)
                {
                    DateTime startNightWork = new DateTime();

                    if (pair.StartTime.TimeOfDay >= nightStart.TimeOfDay)
                        startNightWork = pair.StartTime;
                    else
                        startNightWork = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day, nightStart.Hour,
                            nightStart.Minute, nightStart.Second);

                    nightWork += (int)pair.EndTime.TimeOfDay.Subtract(startNightWork.TimeOfDay).TotalMinutes;

                    if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                        nightWork++;
                }

                return nightWork;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.nightWorkDuration(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isPresence(int ptID, Dictionary<string, RuleTO> emplRules)
        {
            try
            {
                bool presencePair = false;

                List<string> presenceTypes = Constants.WNEffectiveWorkWageTypes();

                foreach (string ruleType in presenceTypes)
                {
                    if (emplRules.ContainsKey(ruleType) && ptID == emplRules[ruleType].RuleValue)
                    {
                        presencePair = true;
                        break;
                    }
                }

                return presencePair;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.isPresence(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isMealPresence(int ptID, Dictionary<string, RuleTO> emplRules, int emplType, DateTime date)
        {
            try
            {
                bool presencePair = false;

                if ((date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) && !Constants.WNWeekendWorkEmplTypes().Contains(emplType))
                    return presencePair;

                List<string> presenceTypes = Constants.WNEffectiveMealWageTypes();

                foreach (string ruleType in presenceTypes)
                {
                    if (emplRules.ContainsKey(ruleType) && ptID == emplRules[ruleType].RuleValue)
                    {
                        presencePair = true;
                        break;
                    }
                }

                return presencePair;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.isMealPresence(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isSundayWork(IOPairProcessedTO pair, DateTime pairDate, Dictionary<string, RuleTO> emplRules, Dictionary<int, EmployeeTO> emplDict, WorkTimeSchemaTO sch)
        {
            try
            {
                bool sundayWork = false;
                int emplType = -1;

                if (emplDict.ContainsKey(pair.EmployeeID))
                    emplType = emplDict[pair.EmployeeID].EmployeeTypeID;

                if (pairDate.DayOfWeek != DayOfWeek.Sunday || !Constants.WNWeekendWorkEmplTypes().Contains(emplType) || isTurnus(sch))
                    return sundayWork;

                List<string> sundayWorkTypes = Constants.WNSundayWorkWageTypes();

                foreach (string ruleType in sundayWorkTypes)
                {
                    if (emplRules.ContainsKey(ruleType) && pair.PassTypeID == emplRules[ruleType].RuleValue)
                    {
                        sundayWork = true;
                        break;
                    }
                }

                return sundayWork;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.isSundayWork(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isSaturdayWork(IOPairProcessedTO pair, DateTime pairDate, Dictionary<string, RuleTO> emplRules, Dictionary<int, EmployeeTO> emplDict, WorkTimeSchemaTO sch)
        {
            try
            {
                bool saturdayWork = false;
                int emplType = -1;

                if (emplDict.ContainsKey(pair.EmployeeID))
                    emplType = emplDict[pair.EmployeeID].EmployeeTypeID;

                if (pairDate.DayOfWeek != DayOfWeek.Saturday || !Constants.WNWeekendWorkEmplTypes().Contains(emplType) || isTurnus(sch))
                    return saturdayWork;

                List<string> saturdayWorkTypes = Constants.WNSaturdayWorkWageTypes();

                foreach (string ruleType in saturdayWorkTypes)
                {
                    if (emplRules.ContainsKey(ruleType) && pair.PassTypeID == emplRules[ruleType].RuleValue)
                    {
                        saturdayWork = true;
                        break;
                    }
                }

                return saturdayWork;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.isSaturdayWork(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isSundayBHWork(IOPairProcessedTO pair, DateTime pairDate, Dictionary<string, RuleTO> emplRules, Dictionary<int, EmployeeTO> emplDict, WorkTimeSchemaTO sch)
        {
            try
            {
                bool sundayWork = false;
                int emplType = -1;

                if (emplDict.ContainsKey(pair.EmployeeID))
                    emplType = emplDict[pair.EmployeeID].EmployeeTypeID;

                if (pairDate.DayOfWeek != DayOfWeek.Sunday || !Constants.WNBHWeekendWorkEmplTypes().Contains(emplType) || isTurnus(sch))
                    return sundayWork;

                List<string> sundayWorkTypes = Constants.WNSundayBHWorkWageTypes();

                foreach (string ruleType in sundayWorkTypes)
                {
                    if (emplRules.ContainsKey(ruleType) && pair.PassTypeID == emplRules[ruleType].RuleValue)
                    {
                        sundayWork = true;
                        break;
                    }
                }

                return sundayWork;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.isSundayBHWork(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isSaturdayBHWork(IOPairProcessedTO pair, DateTime pairDate, Dictionary<string, RuleTO> emplRules, Dictionary<int, EmployeeTO> emplDict, WorkTimeSchemaTO sch)
        {
            try
            {
                bool saturdayWork = false;
                int emplType = -1;

                if (emplDict.ContainsKey(pair.EmployeeID))
                    emplType = emplDict[pair.EmployeeID].EmployeeTypeID;

                if (pairDate.DayOfWeek != DayOfWeek.Saturday || !Constants.WNBHWeekendWorkEmplTypes().Contains(emplType) || isTurnus(sch))
                    return saturdayWork;

                List<string> saturdayWorkTypes = Constants.WNSaturdayBHWorkWageTypes();

                foreach (string ruleType in saturdayWorkTypes)
                {
                    if (emplRules.ContainsKey(ruleType) && pair.PassTypeID == emplRules[ruleType].RuleValue)
                    {
                        saturdayWork = true;
                        break;
                    }
                }

                return saturdayWork;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WNMonthlyReport.isSaturdayBHWork(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
