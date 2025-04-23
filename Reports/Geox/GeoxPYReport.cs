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

namespace Reports.Geox
{
    public partial class GeoxPYReport : Form
    {
        private const int emplIDIndex = 0;
        private const int emplNameIndex = 1;

        private const int stimulationValue = 5000;
                
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
        Dictionary<int, WorkTimeSchemaTO> schemas = new Dictionary<int, WorkTimeSchemaTO>();
        Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rules = new Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>>();

        List<int> unpaidLeaves = new List<int>();
        List<int> paidLeaves = new List<int>();
        List<int> sickLeaves30Days = new List<int>();
        List<int> sickLeavesRefundation = new List<int>();
        
        public GeoxPYReport()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(GeoxPYReport).Assembly);

                logInUser = NotificationController.GetLogInUser();

                setLanguage();

                rbWU.Checked = true;
                                
                dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).Date;
                dtpTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1).Date;
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
                    case GeoxPYReport.emplIDIndex:
                        {
                            int id1 = -1;
                            int id2 = -1;

                            int.TryParse(sub1.Text, out id1);
                            int.TryParse(sub2.Text, out id2);

                            return CaseInsensitiveComparer.Default.Compare(id1, id2);
                        }
                    case GeoxPYReport.emplNameIndex:
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
                this.Text = rm.GetString("GeoxPYReport", culture);

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
                                
                // list view                
                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrID", culture), 75, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrName", culture), 230, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GeoxPYReport_Load(object sender, EventArgs e)
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

                unpaidLeaves = Constants.GeoxUnpaidLeaves();                
                sickLeaves30Days = Constants.GeoxSickLeaves30Days();
                sickLeavesRefundation = Constants.GeoxSickLeavesRefundation();

                // get all paid leave
                Dictionary<int, List<int>> confirmationPTDict = new PassTypesConfirmation().SearchDictionary();
                List<int> sickLeaveNCF = new Common.Rule().SearchRulesExact(Constants.RuleCompanySickLeaveNCF);
                
                foreach (int ptID in confirmationPTDict.Keys)
                {
                    if (!confirmationPTDict[ptID].Contains(Constants.unpaidLeave) && !sickLeaveNCF.Contains(ptID))
                        paidLeaves.AddRange(confirmationPTDict[ptID]);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.GeoxPYReport_Load(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.populateWU(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.populateOU(): " + ex.Message + "\n");
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
                                
                DateTime from = dtpFrom.Value.Date;
                DateTime to = dtpTo.Value.Date;

                if (from > to)
                    MessageBox.Show(rm.GetString("invalidDateInterval", culture));
                else if (ID != -1)
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.populateEmployees(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.rbWU_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.rbOU_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " GeoxPYReport.tbEmployee_TextChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " GeoxPYReport.lvEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.dtpFrom_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.dtpTo_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                foreach (int emplID in emplSchedules.Keys)
                {
                    DateTime currDate = from.Date;

                    while (currDate <= to.Date.AddDays(1))
                    {
                        if (!emplDayIntervals.ContainsKey(emplID))
                            emplDayIntervals.Add(emplID, new Dictionary<DateTime, List<WorkTimeIntervalTO>>());

                        if (!emplDayIntervals[emplID].ContainsKey(currDate.Date))                        
                            emplDayIntervals[emplID].Add(currDate.Date, Common.Misc.getTimeSchemaInterval(currDate.Date, emplSchedules[emplID], schemas));

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
                Dictionary<int, List<DateTime>> emplHolidayPaidDays = new Dictionary<int, List<DateTime>>();

                // create rules for each employee
                foreach (int emplID in emplDict.Keys)
                {
                    if (!emplRules.ContainsKey(emplID))
                    {
                        int company = Common.Misc.getRootWorkingUnit(emplDict[emplID].WorkingUnitID, wuDict);

                        if (rules.ContainsKey(company) && rules[company].ContainsKey(emplDict[emplID].EmployeeTypeID))                        
                            emplRules.Add(emplID, rules[company][emplDict[emplID].EmployeeTypeID]);
                    }
                }

                Dictionary<int, int> emplSundayWork = new Dictionary<int, int>();
                Dictionary<int, Dictionary<int, int>> emplPTDurationSummary = new Dictionary<int, Dictionary<int, int>>();
                Dictionary<int, Dictionary<int, List<DateTime>>> emplPTDates = new Dictionary<int, Dictionary<int, List<DateTime>>>();                
                List<int> noStimulation = new List<int>();

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

                        if (!emplPTDates.ContainsKey(pair.EmployeeID))
                            emplPTDates.Add(pair.EmployeeID, new Dictionary<int, List<DateTime>>());
                        if (!emplPTDates[pair.EmployeeID].ContainsKey(pair.PassTypeID))
                            emplPTDates[pair.EmployeeID].Add(pair.PassTypeID, new List<DateTime>());
                        if (!emplPTDates[pair.EmployeeID][pair.PassTypeID].Contains(pairDate.Date))
                            emplPTDates[pair.EmployeeID][pair.PassTypeID].Add(pairDate.Date);
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
                        // check if employee work more than 8 hours
                        int decreaseMin = 0;
                        int shiftDuration = 0;
                        if (emplDayIntervals.ContainsKey(employeeID) && emplDayIntervals[employeeID].ContainsKey(date.Date))
                        {
                            foreach (WorkTimeIntervalTO interval in emplDayIntervals[employeeID][date.Date])
                            {
                                if (interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                    shiftDuration += (int)interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes;

                                if (interval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                    shiftDuration++;

                                if (emplDayIntervals[employeeID].ContainsKey(date.Date.AddDays(1)))
                                {
                                    foreach (WorkTimeIntervalTO nextInterval in emplDayIntervals[employeeID][date.Date.AddDays(1)])
                                    {
                                        if (nextInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                            shiftDuration += (int)nextInterval.EndTime.TimeOfDay.Subtract(nextInterval.StartTime.TimeOfDay).TotalMinutes;
                                    }
                                }
                            }
                        }

                        decreaseMin = shiftDuration - Constants.dayDurationStandardShift * 60;

                        for (int i = 0; i < emplBelongingDayPairs[employeeID][date].Count; i++)
                        {
                            IOPairProcessedTO pair = emplBelongingDayPairs[employeeID][date][i];

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
                            
                            if (rulesForEmpl.ContainsKey(Constants.RuleWorkOnHolidayPassType))
                                workOnHolidayPT = rulesForEmpl[Constants.RuleWorkOnHolidayPassType].RuleValue;

                            if (rulesForEmpl.ContainsKey(Constants.RuleNightWork))
                                nightWorkPT = rulesForEmpl[Constants.RuleNightWork].RuleValue;

                            if (rulesForEmpl.ContainsKey(Constants.RuleComanyRotaryShift))
                                turnusPT = rulesForEmpl[Constants.RuleComanyRotaryShift].RuleValue;
                            
                            // calculate code durations and add it to dictionary
                            int pairDuration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                            if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                pairDuration++;

                            // check if pair duration should be decreased
                            if (decreaseMin > 0 && pairDuration >= decreaseMin && ptDict.ContainsKey(pair.PassTypeID) && ptDict[pair.PassTypeID].IsPass != Constants.overtimePassType)
                            {
                                // if pair is regular work longer then decreaseMin, decrease duration
                                if (rulesForEmpl.ContainsKey(Constants.RuleCompanyRegularWork) && pair.PassTypeID == rulesForEmpl[Constants.RuleCompanyRegularWork].RuleValue)
                                {
                                    pairDuration -= decreaseMin;
                                    decreaseMin = 0;
                                }
                                else
                                {
                                    bool regFound = false;
                                    for (int j = i + 1; j < emplBelongingDayPairs[employeeID][date].Count; j++)
                                    {
                                        if (rulesForEmpl.ContainsKey(Constants.RuleCompanyRegularWork) && emplBelongingDayPairs[employeeID][date][j].PassTypeID == rulesForEmpl[Constants.RuleCompanyRegularWork].RuleValue)
                                        {
                                            int regDuration = (int)emplBelongingDayPairs[employeeID][date][j].EndTime.Subtract(emplBelongingDayPairs[employeeID][date][j].StartTime).TotalMinutes;
                                            if (emplBelongingDayPairs[employeeID][date][j].EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                regDuration++;

                                            regFound = regDuration >= decreaseMin;
                                        }
                                    }

                                    if (!regFound)
                                    {
                                        pairDuration -= decreaseMin;
                                        decreaseMin = 0;
                                    }
                                }
                            }

                            if (date.DayOfWeek == DayOfWeek.Sunday && rulesForEmpl.ContainsKey(Constants.RuleCompanyOvertimePaid) && pair.PassTypeID == rulesForEmpl[Constants.RuleCompanyOvertimePaid].RuleValue)
                            {
                                if (!emplSundayWork.ContainsKey(pair.EmployeeID))
                                    emplSundayWork.Add(pair.EmployeeID, pairDuration);
                                else
                                    emplSundayWork[pair.EmployeeID] += pairDuration;
                            }
                            else
                            {
                                if (!emplPTDurationSummary.ContainsKey(pair.EmployeeID))
                                    emplPTDurationSummary.Add(pair.EmployeeID, new Dictionary<int, int>());

                                if (!emplPTDurationSummary[pair.EmployeeID].ContainsKey(pair.PassTypeID))
                                    emplPTDurationSummary[pair.EmployeeID].Add(pair.PassTypeID, 0);

                                emplPTDurationSummary[pair.EmployeeID][pair.PassTypeID] += pairDuration;
                            }
                            
                            // calculate addition for night work, turnus and holidays
                            if (pair.PassTypeID != Constants.absence && isPresence(pair.PassTypeID, rulesForEmpl))
                            {
                                // check work on holiday
                                if (workOnHolidayPT != -1
                                    && isWorkOnHoliday(personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, nationalTransferableHolidays, paidDays, date.Date, schema, asco, empl))
                                {
                                    if (!emplPTDurationSummary[pair.EmployeeID].ContainsKey(workOnHolidayPT))
                                        emplPTDurationSummary[pair.EmployeeID].Add(workOnHolidayPT, 0);

                                    emplPTDurationSummary[pair.EmployeeID][workOnHolidayPT] += pairDuration;

                                    if (!emplHolidayPaidDays.ContainsKey(pair.EmployeeID))
                                        emplHolidayPaidDays.Add(pair.EmployeeID, paidDays);
                                    else
                                        emplHolidayPaidDays[pair.EmployeeID] = paidDays;
                                }

                                if (turnusPT != -1 && isTurnus(schema) && ptDict.ContainsKey(pair.PassTypeID) && ptDict[pair.PassTypeID].IsPass != Constants.overtimePassType)
                                {
                                    if (!emplPTDurationSummary[pair.EmployeeID].ContainsKey(turnusPT))
                                        emplPTDurationSummary[pair.EmployeeID].Add(turnusPT, 0);

                                    emplPTDurationSummary[pair.EmployeeID][turnusPT] += pairDuration;
                                }

                                int nightWork = nightWorkDuration(rulesForEmpl, pair, schema);

                                if (nightWorkPT != -1 && nightWork > 0)
                                {
                                    if (!emplPTDurationSummary[pair.EmployeeID].ContainsKey(nightWorkPT))
                                        emplPTDurationSummary[pair.EmployeeID].Add(nightWorkPT, 0);

                                    emplPTDurationSummary[pair.EmployeeID][nightWorkPT] += nightWork;
                                }
                            }

                            // check stimulation if pair is from the shift
                            if (!noStimulation.Contains(pair.EmployeeID) && ptDict.ContainsKey(pair.PassTypeID) && ptDict[pair.PassTypeID].IsPass != Constants.overtimePassType)
                            {
                                if (!isPairForStimulation(pair, rulesForEmpl, emplBelongingDayPairs[employeeID][date]))
                                    noStimulation.Add(pair.EmployeeID);
                            }
                        }
                    }
                }

                // generate xls report
                generateReport(emplDict, ascoDict, emplRules, emplPTDurationSummary, emplPTDates, noStimulation, emplSundayWork);
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GeoxPYReport.btnGenerateReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private bool isWorkOnHoliday(Dictionary<string, List<DateTime>> personalHolidayDays, List<DateTime> nationalHolidaysDays, List<DateTime> nationalHolidaysDaysSundays,
            List<HolidaysExtendedTO> nationalTransferableHolidays, List<DateTime> paidHolidaysDays, DateTime date, WorkTimeSchemaTO schema, EmployeeAsco4TO asco, EmployeeTO empl)
        {
            try
            {
                bool isHoliday = false;
                
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
                log.writeLog(DateTime.Now + " GeoxPYReport.isWorkOnHoliday(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " GeoxPYReport.isTurnus(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private int nightWorkDuration(Dictionary<string, RuleTO> emplRules, IOPairProcessedTO pair, WorkTimeSchemaTO sch)
        {
            try
            {
                int nightWork = 0;
                
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
                log.writeLog(DateTime.Now + " GeoxPYReport.nightWorkDuration(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isPresence(int ptID, Dictionary<string, RuleTO> emplRules)
        {
            try
            {
                bool presencePair = false;

                List<string> presenceTypes = Constants.GeoxEffectiveWorkWageTypes();

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
                log.writeLog(DateTime.Now + " GeoxPYReport.isPresence(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void generateReport(Dictionary<int, EmployeeTO> emplDict, Dictionary<int, EmployeeAsco4TO> ascoDict, Dictionary<int, Dictionary<string, RuleTO>> emplRules,
            Dictionary<int, Dictionary<int, int>> emplPTDurationSummary, Dictionary<int, Dictionary<int, List<DateTime>>> emplPTDates, List<int> noStimulationEmployees, Dictionary<int, int> emplSundayWork)
        {
            try
            {
                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                object misValue = System.Reflection.Missing.Value;

                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;

                ws.Cells[1, 1] = "Adecco";
                
                // insert header
                int colNum = 71;
                ws.Cells[3, 1] = "RB";
                ws.Cells[3, 2] = "Klijent";
                ws.Cells[3, 3] = "Prezime i ime";
                ws.Cells[3, 4] = "Šifra radnika";
                ws.Cells[3, 5] = "Vrsta osnovice";
                ws.Cells[3, 6] = "Osnovica";
                ws.Cells[3, 7] = "Broj sati redovnog rada";
                ws.Cells[3, 8] = "Broj sati praznika";
                ws.Cells[3, 9] = "Broj sati neplaćenog odsustva";
                ws.Cells[3, 10] = "Od datuma (neplaćeno odsustvo)";
                ws.Cells[3, 11] = "Do datuma (neplaćeno odsustvo)";
                ws.Cells[3, 12] = "Sati odmora";
                ws.Cells[3, 13] = "Vrednost sata za odmor";
                ws.Cells[3, 14] = "Sati bolovanja do 30 dana";
                ws.Cells[3, 15] = "Od datuma (bolovanje do 30 dana)";
                ws.Cells[3, 16] = "Do datuma (bolovanje do 30 dana)";        
                ws.Cells[3, 17] = "Vrsta bolovanja do 30 dana";
                ws.Cells[3, 18] = "Sati za refundaciju";
                ws.Cells[3, 19] = "Od datuma (refundirano bolovanje)";
                ws.Cells[3, 20] = "Do datuma (refundirano bolovanje)";
                ws.Cells[3, 21] = "Vrsta bolovanja - refundacija";
                ws.Cells[3, 22] = "Refundacija 35% Beograd";
                ws.Cells[3, 23] = "Vrednost sata bolovanja";
                ws.Cells[3, 24] = "Sati noćnog rada";
                ws.Cells[3, 25] = "Koeficijent za noćni rad";
                ws.Cells[3, 26] = "Sati prekovremenog rada";
                ws.Cells[3, 27] = "Koeficijent za prekovremeni rad";
                ws.Cells[3, 28] = "Sati rada praznikom";
                ws.Cells[3, 29] = "Koeficijent za rad praznikom";
                ws.Cells[3, 30] = "Vrsta stimulacije";
                ws.Cells[3, 31] = "Vrednost stimulacije";
                ws.Cells[3, 32] = "Vrsta osnovice stimulacija";
                ws.Cells[3, 33] = "Regres";
                ws.Cells[3, 34] = "Regres u ili na platu";
                ws.Cells[3, 35] = "Vrsta osnovice regres";
                ws.Cells[3, 36] = "Topli obrok";
                ws.Cells[3, 37] = "Topli obrok u ili na platu";
                ws.Cells[3, 38] = "Vrsta osnovice topli obrok";
                ws.Cells[3, 39] = "Procenat za minuli rad";
                ws.Cells[3, 40] = "Minuli rad u ili na platu";
                ws.Cells[3, 41] = "Prevoz";
                ws.Cells[3, 42] = "Vrsta osnovice prevoz";
                ws.Cells[3, 43] = "Rad nedeljom";
                ws.Cells[3, 44] = "Koeficijent za rad nedeljom";
                ws.Cells[3, 45] = "Vrsta osnovice za dnevni bonus";
                ws.Cells[3, 46] = "Dnevni bonus-iznos";
                ws.Cells[3, 47] = "Vrsta osnovice za bonus miks";
                ws.Cells[3, 48] = "Bonus miks-iznos";
                ws.Cells[3, 49] = "Sati smenskog rada";
                ws.Cells[3, 50] = "Koeficijent smenskog rada";
                ws.Cells[3, 51] = "Sati plaćenog odsustva";
                ws.Cells[3, 52] = "Od datuma (plaćeno odsustvo)";
                ws.Cells[3, 53] = "Do datuma (plaćeno odsustvo)";
                ws.Cells[3, 54] = "Korekcija minulog rada";
                ws.Cells[3, 55] = "Vrsta osnovice za benefit";
                ws.Cells[3, 56] = "Benefit radnika";
                ws.Cells[3, 57] = "Sati prekovremenog rada sa koeficijentom 1";
                ws.Cells[3, 58] = "Dodatak-NETO EUR";
                ws.Cells[3, 59] = "Sati prekovremenog noćnog rada";
                ws.Cells[3, 60] = "Koeficijent za prekovremeni noćni rad";
                ws.Cells[3, 61] = "Topli obrok-fiksna vrednost";
                ws.Cells[3, 62] = "Stimulacija u %";
                ws.Cells[3, 63] = "Prekid rada bez krivice zaposlenog";
                ws.Cells[3, 64] = "Od datuma (prekid rada bez krivice zaposlenog)";
                ws.Cells[3, 65] = "Do datuma (prekid rada bez krivice zaposlenog)";
                ws.Cells[3, 66] = "Unapred placeni rad";
                ws.Cells[3, 67] = "Minuli rad 1";
                ws.Cells[3, 68] = "Sluzbeni put";
                ws.Cells[3, 69] = "Neopravdano odsustvo";
                ws.Cells[3, 70] = "Od datuma (neopravdano odsustvo)";
                ws.Cells[3, 71] = "Do datuma (neopravdano odsustvo)";
	
                setRowFontWeight(ws, 1, colNum, true);
                setRowFontWeight(ws, 3, colNum, true);
                                
                int rowNum = 4;
                int rb = 0;
                
                foreach (int id in emplDict.Keys)
                {
                    rb++;

                    // get rules
                    Dictionary<string, RuleTO> rulesEmpl = new Dictionary<string, RuleTO>();
                    if (emplRules.ContainsKey(id))
                        rulesEmpl = emplRules[id];

                    // employee data
                    ws.Cells[rowNum, 1] = rb.ToString().Trim();                    
                    //if (ascoDict.ContainsKey(id) && wuDict.ContainsKey(ascoDict[id].IntegerValue4))
                    //    ws.Cells[rowNum, 2] = wuDict[ascoDict[id].IntegerValue4].Name.Trim();
                    //else
                    //    ws.Cells[rowNum, 2] = "N/A";                    
                    ws.Cells[rowNum, 3] = emplDict[id].FirstAndLastName.Trim();
                    ws.Cells[rowNum, 4] = id.ToString().Trim();

                    // regular work                    
                    int regWork = 0;
                    if (rulesEmpl.ContainsKey(Constants.RuleCompanyRegularWork) && emplPTDurationSummary.ContainsKey(id)
                        && emplPTDurationSummary[id].ContainsKey(rulesEmpl[Constants.RuleCompanyRegularWork].RuleValue))
                        regWork += emplPTDurationSummary[id][rulesEmpl[Constants.RuleCompanyRegularWork].RuleValue];
                    if (rulesEmpl.ContainsKey(Constants.RuleCompanyBankHourUsed) && emplPTDurationSummary.ContainsKey(id)
                        && emplPTDurationSummary[id].ContainsKey(rulesEmpl[Constants.RuleCompanyBankHourUsed].RuleValue))
                        regWork += emplPTDurationSummary[id][rulesEmpl[Constants.RuleCompanyBankHourUsed].RuleValue];
                    if (rulesEmpl.ContainsKey(Constants.RuleCompanyJustifiedAbsence) && emplPTDurationSummary.ContainsKey(id)
                        && emplPTDurationSummary[id].ContainsKey(rulesEmpl[Constants.RuleCompanyJustifiedAbsence].RuleValue))
                        regWork += emplPTDurationSummary[id][rulesEmpl[Constants.RuleCompanyJustifiedAbsence].RuleValue];
                    if (rulesEmpl.ContainsKey(Constants.RuleCompanyOfficialTrip) && emplPTDurationSummary.ContainsKey(id)
                        && emplPTDurationSummary[id].ContainsKey(rulesEmpl[Constants.RuleCompanyOfficialTrip].RuleValue))
                        regWork += emplPTDurationSummary[id][rulesEmpl[Constants.RuleCompanyOfficialTrip].RuleValue];
                    if (rulesEmpl.ContainsKey(Constants.RuleCompanyOfficialOut) && emplPTDurationSummary.ContainsKey(id)
                        && emplPTDurationSummary[id].ContainsKey(rulesEmpl[Constants.RuleCompanyOfficialOut].RuleValue))
                        regWork += emplPTDurationSummary[id][rulesEmpl[Constants.RuleCompanyOfficialOut].RuleValue];
                    if (rulesEmpl.ContainsKey(Constants.RuleCompanyStopWorking) && emplPTDurationSummary.ContainsKey(id)
                        && emplPTDurationSummary[id].ContainsKey(rulesEmpl[Constants.RuleCompanyStopWorking].RuleValue))
                        regWork += emplPTDurationSummary[id][rulesEmpl[Constants.RuleCompanyStopWorking].RuleValue];
                    ws.Cells[rowNum, 7] = ((decimal)regWork / 60).ToString(Constants.doubleFormat);
                    
                    // holiday
                    int holidayMin = 0;
                    if (rulesEmpl.ContainsKey(Constants.RuleHolidayPassType) && emplPTDurationSummary.ContainsKey(id)
                        && emplPTDurationSummary[id].ContainsKey(rulesEmpl[Constants.RuleHolidayPassType].RuleValue))
                        holidayMin += emplPTDurationSummary[id][rulesEmpl[Constants.RuleHolidayPassType].RuleValue];
                    if (rulesEmpl.ContainsKey(Constants.RulePersonalHolidayPassType) && emplPTDurationSummary.ContainsKey(id)
                        && emplPTDurationSummary[id].ContainsKey(rulesEmpl[Constants.RulePersonalHolidayPassType].RuleValue))
                        holidayMin += emplPTDurationSummary[id][rulesEmpl[Constants.RulePersonalHolidayPassType].RuleValue];
                    ws.Cells[rowNum, 8] = ((decimal)holidayMin / 60).ToString(Constants.doubleFormat);

                    // unpaid leave
                    int unpaidMin = 0;
                    List<int> unpaidTypesList = new List<int>();
                    foreach (int unpaidPT in unpaidLeaves)
                    {
                        if (emplPTDurationSummary.ContainsKey(id) && emplPTDurationSummary[id].ContainsKey(unpaidPT))
                        {
                            unpaidMin += emplPTDurationSummary[id][unpaidPT];
                            if (!unpaidTypesList.Contains(unpaidPT))
                                unpaidTypesList.Add(unpaidPT);
                        }
                    }
                    ws.Cells[rowNum, 9] = ((decimal)unpaidMin / 60).ToString(Constants.doubleFormat);

                    // annual leave
                    int alMinutes = 0;
                    if (rulesEmpl.ContainsKey(Constants.RuleCompanyAnnualLeave) && emplPTDurationSummary.ContainsKey(id)
                        && emplPTDurationSummary[id].ContainsKey(rulesEmpl[Constants.RuleCompanyAnnualLeave].RuleValue))
                        alMinutes += emplPTDurationSummary[id][rulesEmpl[Constants.RuleCompanyAnnualLeave].RuleValue];

                    if (rulesEmpl.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && emplPTDurationSummary.ContainsKey(id)
                        && emplPTDurationSummary[id].ContainsKey(rulesEmpl[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue))
                        alMinutes += emplPTDurationSummary[id][rulesEmpl[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue];

                    ws.Cells[rowNum, 12] = ((decimal)alMinutes / 60).ToString(Constants.doubleFormat);
                    
                    // night work
                    if (rulesEmpl.ContainsKey(Constants.RuleNightWork) && emplPTDurationSummary.ContainsKey(id)
                        && emplPTDurationSummary[id].ContainsKey(rulesEmpl[Constants.RuleNightWork].RuleValue))
                        ws.Cells[rowNum, 24] = ((decimal)emplPTDurationSummary[id][rulesEmpl[Constants.RuleNightWork].RuleValue] / 60).ToString(Constants.doubleFormat);
                    else
                        ws.Cells[rowNum, 24] = "0.00";

                    // overtime
                    if (rulesEmpl.ContainsKey(Constants.RuleCompanyOvertimePaid) && emplPTDurationSummary.ContainsKey(id)
                        && emplPTDurationSummary[id].ContainsKey(rulesEmpl[Constants.RuleCompanyOvertimePaid].RuleValue))
                        ws.Cells[rowNum, 26] = ((decimal)emplPTDurationSummary[id][rulesEmpl[Constants.RuleCompanyOvertimePaid].RuleValue] / 60).ToString(Constants.doubleFormat);
                    else
                        ws.Cells[rowNum, 26] = "0.00";
                
                    // work on holiday                    
                    if (rulesEmpl.ContainsKey(Constants.RuleWorkOnHolidayPassType) && emplPTDurationSummary.ContainsKey(id)
                        && emplPTDurationSummary[id].ContainsKey(rulesEmpl[Constants.RuleWorkOnHolidayPassType].RuleValue))
                        ws.Cells[rowNum, 28] = ((decimal)emplPTDurationSummary[id][rulesEmpl[Constants.RuleWorkOnHolidayPassType].RuleValue] / 60).ToString(Constants.doubleFormat);
                    else
                        ws.Cells[rowNum, 28] = "0.00";

                    // work on sunday
                    if (emplSundayWork.ContainsKey(id))
                        ws.Cells[rowNum, 43] = ((decimal)emplSundayWork[id] / 60).ToString(Constants.doubleFormat);
                    else
                        ws.Cells[rowNum, 43] = "0.00";

                    // paid leave
                    int paidMin = 0;
                    List<int> paidTypesList = new List<int>();
                    foreach (int paidPT in paidLeaves)
                    {
                        if (emplPTDurationSummary.ContainsKey(id) && emplPTDurationSummary[id].ContainsKey(paidPT))
                        {
                            paidMin += emplPTDurationSummary[id][paidPT];

                            if (!paidTypesList.Contains(paidPT))
                                paidTypesList.Add(paidPT);
                        }
                    }
                    ws.Cells[rowNum, 51] = ((decimal)paidMin / 60).ToString(Constants.doubleFormat);

                    // unjustified hours
                    int unjustifiedMinutes = 0;
                    if (emplPTDurationSummary.ContainsKey(id) && emplPTDurationSummary[id].ContainsKey(Constants.absence))
                        unjustifiedMinutes += emplPTDurationSummary[id][Constants.absence];

                    if (rulesEmpl.ContainsKey(Constants.RuleCompanyDelay) && emplPTDurationSummary.ContainsKey(id)
                        && emplPTDurationSummary[id].ContainsKey(rulesEmpl[Constants.RuleCompanyDelay].RuleValue))
                        unjustifiedMinutes += emplPTDurationSummary[id][rulesEmpl[Constants.RuleCompanyDelay].RuleValue];

                    ws.Cells[rowNum, 69] = ((decimal)unjustifiedMinutes / 60).ToString(Constants.doubleFormat);

                    if (!noStimulationEmployees.Contains(id))
                        ws.Cells[rowNum, 31] = stimulationValue.ToString(Constants.doubleFormat);
                    else
                        ws.Cells[rowNum, 31] = "0.00";

                    Dictionary<int, Dictionary<DateTime, DateTime>> unpaidDict = new Dictionary<int, Dictionary<DateTime, DateTime>>();
                    int rowIndex = rowNum;
                    foreach (int unpaidID in unpaidTypesList)
                    {
                        if (emplPTDates.ContainsKey(id) && emplPTDates[id].ContainsKey(unpaidID))
                        {
                            DateTime prevDate = new DateTime();
                            DateTime startDate = new DateTime();
                            DateTime endDate = new DateTime();
                            for (int i = 0; i < emplPTDates[id][unpaidID].Count; i++)
                            {
                                if (prevDate != new DateTime() && !isPreviousDay(prevDate.Date, emplPTDates[id][unpaidID][i].Date))
                                {
                                    if (!unpaidDict.ContainsKey(rowIndex))
                                    {
                                        unpaidDict.Add(rowIndex, new Dictionary<DateTime, DateTime>());
                                        unpaidDict[rowIndex].Add(startDate, endDate);
                                        rowIndex++;
                                    }
                                }

                                if (prevDate == new DateTime() || !isPreviousDay(prevDate.Date, emplPTDates[id][unpaidID][i].Date))
                                    startDate = emplPTDates[id][unpaidID][i].Date;

                                endDate = emplPTDates[id][unpaidID][i].Date;

                                prevDate = emplPTDates[id][unpaidID][i].Date;

                                if (i == emplPTDates[id][unpaidID].Count - 1)
                                {
                                    if (!unpaidDict.ContainsKey(rowIndex))
                                    {
                                        unpaidDict.Add(rowIndex, new Dictionary<DateTime, DateTime>());
                                        unpaidDict[rowIndex].Add(startDate, endDate);
                                        rowIndex++;
                                    }
                                }
                            }
                        }
                    }

                    Dictionary<int, Dictionary<DateTime, DateTime>> paidDict = new Dictionary<int, Dictionary<DateTime, DateTime>>();
                    rowIndex = rowNum;
                    foreach (int paidID in paidTypesList)
                    {
                        if (emplPTDates.ContainsKey(id) && emplPTDates[id].ContainsKey(paidID))
                        {
                            DateTime prevDate = new DateTime();
                            DateTime startDate = new DateTime();
                            DateTime endDate = new DateTime();
                            for (int i = 0; i < emplPTDates[id][paidID].Count; i++)
                            {
                                if (prevDate != new DateTime() && !isPreviousDay(prevDate.Date, emplPTDates[id][paidID][i].Date))
                                {
                                    if (!paidDict.ContainsKey(rowIndex))
                                    {
                                        paidDict.Add(rowIndex, new Dictionary<DateTime, DateTime>());
                                        paidDict[rowIndex].Add(startDate, endDate);
                                        rowIndex++;
                                    }
                                }

                                if (prevDate == new DateTime() || !isPreviousDay(prevDate.Date, emplPTDates[id][paidID][i].Date))
                                    startDate = emplPTDates[id][paidID][i].Date;

                                endDate = emplPTDates[id][paidID][i].Date;

                                prevDate = emplPTDates[id][paidID][i].Date;

                                if (i == emplPTDates[id][paidID].Count - 1)
                                {
                                    if (!paidDict.ContainsKey(rowIndex))
                                    {
                                        paidDict.Add(rowIndex, new Dictionary<DateTime, DateTime>());
                                        paidDict[rowIndex].Add(startDate, endDate);
                                        rowIndex++;
                                    }
                                }
                            }
                        }
                    }

                    Dictionary<int, Dictionary<int, int>> sl30Dict = new Dictionary<int, Dictionary<int, int>>();
                    Dictionary<int, Dictionary<DateTime, DateTime>> sl30StartEndDict = new Dictionary<int, Dictionary<DateTime, DateTime>>();
                    rowIndex = rowNum;
                    bool increaseRow = false;
                    int oldIndex = -1;
                    foreach (int sl30PT in sickLeaves30Days)
                    {
                        if (increaseRow && oldIndex == rowIndex)
                            rowIndex++;

                        increaseRow = false;

                        if (emplPTDurationSummary.ContainsKey(id) && emplPTDurationSummary[id].ContainsKey(sl30PT))
                        {
                            if (!sl30Dict.ContainsKey(rowIndex))
                            {
                                sl30Dict.Add(rowIndex, new Dictionary<int, int>());
                                sl30Dict[rowIndex].Add(sl30PT, emplPTDurationSummary[id][sl30PT]);
                                oldIndex = rowIndex;
                                increaseRow = true;
                            }
                        }

                        if (emplPTDates.ContainsKey(id) && emplPTDates[id].ContainsKey(sl30PT))
                        {
                            DateTime prevDate = new DateTime();
                            DateTime startDate = new DateTime();
                            DateTime endDate = new DateTime();
                            for (int i = 0; i < emplPTDates[id][sl30PT].Count; i++)
                            {
                                if (prevDate != new DateTime() && !isPreviousDay(prevDate.Date, emplPTDates[id][sl30PT][i].Date))
                                {
                                    if (!sl30StartEndDict.ContainsKey(rowIndex))
                                    {
                                        sl30StartEndDict.Add(rowIndex, new Dictionary<DateTime, DateTime>());
                                        sl30StartEndDict[rowIndex].Add(startDate, endDate);
                                        rowIndex++;
                                    }
                                }

                                if (prevDate == new DateTime() || !isPreviousDay(prevDate.Date, emplPTDates[id][sl30PT][i].Date))
                                    startDate = emplPTDates[id][sl30PT][i].Date;

                                endDate = emplPTDates[id][sl30PT][i].Date;

                                prevDate = emplPTDates[id][sl30PT][i].Date;

                                if (i == emplPTDates[id][sl30PT].Count - 1)
                                {
                                    if (!sl30StartEndDict.ContainsKey(rowIndex))
                                    {
                                        sl30StartEndDict.Add(rowIndex, new Dictionary<DateTime, DateTime>());
                                        sl30StartEndDict[rowIndex].Add(startDate, endDate);
                                        rowIndex++;
                                    }
                                }
                            }
                        }                        
                    }

                    Dictionary<int, Dictionary<int, int>> slRefDict = new Dictionary<int, Dictionary<int, int>>();
                    Dictionary<int, Dictionary<DateTime, DateTime>> slRefStartEndDict = new Dictionary<int, Dictionary<DateTime, DateTime>>();
                    rowIndex = rowNum;
                    increaseRow = false;
                    oldIndex = -1;
                    foreach (int slRefPT in sickLeavesRefundation)
                    {
                        if (increaseRow && oldIndex == rowIndex)
                            rowIndex++;

                        increaseRow = false;

                        if (emplPTDurationSummary.ContainsKey(id) && emplPTDurationSummary[id].ContainsKey(slRefPT))
                        {
                            if (!slRefDict.ContainsKey(rowIndex))
                            {
                                slRefDict.Add(rowIndex, new Dictionary<int, int>());
                                slRefDict[rowIndex].Add(slRefPT, emplPTDurationSummary[id][slRefPT]);
                                oldIndex = rowIndex;
                                increaseRow = true;
                            }
                        }

                        if (emplPTDates.ContainsKey(id) && emplPTDates[id].ContainsKey(slRefPT))
                        {
                            DateTime prevDate = new DateTime();
                            DateTime startDate = new DateTime();
                            DateTime endDate = new DateTime();
                            for (int i = 0; i < emplPTDates[id][slRefPT].Count; i++)
                            {
                                if (prevDate != new DateTime() && !isPreviousDay(prevDate.Date, emplPTDates[id][slRefPT][i].Date))
                                {
                                    if (!slRefStartEndDict.ContainsKey(rowIndex))
                                    {
                                        slRefStartEndDict.Add(rowIndex, new Dictionary<DateTime, DateTime>());
                                        slRefStartEndDict[rowIndex].Add(startDate, endDate);
                                        rowIndex++;
                                    }
                                }

                                if (prevDate == new DateTime() || !isPreviousDay(prevDate.Date, emplPTDates[id][slRefPT][i].Date))
                                    startDate = emplPTDates[id][slRefPT][i].Date;

                                endDate = emplPTDates[id][slRefPT][i].Date;

                                prevDate = emplPTDates[id][slRefPT][i].Date;

                                if (i == emplPTDates[id][slRefPT].Count - 1)
                                {
                                    if (!slRefStartEndDict.ContainsKey(rowIndex))
                                    {
                                        slRefStartEndDict.Add(rowIndex, new Dictionary<DateTime, DateTime>());
                                        slRefStartEndDict[rowIndex].Add(startDate, endDate);
                                        rowIndex++;
                                    }
                                }
                            }
                        }
                    }

                    int slRowNum = rowNum;
                    if (sl30Dict.ContainsKey(slRowNum) || slRefDict.ContainsKey(slRowNum) || paidDict.ContainsKey(slRowNum) || unpaidDict.ContainsKey(slRowNum)
                        || sl30StartEndDict.ContainsKey(slRowNum) || slRefStartEndDict.ContainsKey(slRowNum))
                    {
                        while (sl30Dict.ContainsKey(slRowNum) || slRefDict.ContainsKey(slRowNum) || paidDict.ContainsKey(slRowNum) || unpaidDict.ContainsKey(slRowNum)
                        || sl30StartEndDict.ContainsKey(slRowNum) || slRefStartEndDict.ContainsKey(slRowNum))
                        {
                            if (sl30Dict.ContainsKey(slRowNum))
                            {
                                foreach (int ptID in sl30Dict[slRowNum].Keys)
                                {
                                    ws.Cells[slRowNum, 14] = ((decimal)sl30Dict[slRowNum][ptID] / 60).ToString(Constants.doubleFormat);
                                    if (ptDict.ContainsKey(ptID))
                                        ws.Cells[slRowNum, 17] = ptDict[ptID].Description.Trim();
                                }
                            }

                            if (slRefDict.ContainsKey(slRowNum))
                            {
                                foreach (int ptID in slRefDict[slRowNum].Keys)
                                {
                                    ws.Cells[slRowNum, 18] = ((decimal)slRefDict[slRowNum][ptID] / 60).ToString(Constants.doubleFormat);
                                    if (ptDict.ContainsKey(ptID))
                                        ws.Cells[slRowNum, 21] = ptDict[ptID].Description.Trim();
                                }
                            }

                            if (paidDict.ContainsKey(slRowNum))
                            {
                                foreach (DateTime start in paidDict[slRowNum].Keys)
                                {
                                    ws.Cells[slRowNum, 52] = start.Date.ToString(Constants.dateFormat);                                    
                                    ws.Cells[slRowNum, 53] = paidDict[slRowNum][start].Date.ToString(Constants.dateFormat);
                                }
                            }

                            if (unpaidDict.ContainsKey(slRowNum))
                            {
                                foreach (DateTime start in unpaidDict[slRowNum].Keys)
                                {
                                    ws.Cells[slRowNum, 10] = start.Date.ToString(Constants.dateFormat);
                                    ws.Cells[slRowNum, 11] = unpaidDict[slRowNum][start].Date.ToString(Constants.dateFormat);
                                }
                            }

                            if (sl30StartEndDict.ContainsKey(slRowNum))
                            {
                                foreach (DateTime start in sl30StartEndDict[slRowNum].Keys)
                                {
                                    ws.Cells[slRowNum, 15] = start.Date.ToString(Constants.dateFormat);
                                    ws.Cells[slRowNum, 16] = sl30StartEndDict[slRowNum][start].Date.ToString(Constants.dateFormat);
                                }
                            }

                            if (slRefStartEndDict.ContainsKey(slRowNum))
                            {
                                foreach (DateTime start in slRefStartEndDict[slRowNum].Keys)
                                {
                                    ws.Cells[slRowNum, 19] = start.Date.ToString(Constants.dateFormat);
                                    ws.Cells[slRowNum, 20] = slRefStartEndDict[slRowNum][start].Date.ToString(Constants.dateFormat);
                                }
                            }

                            slRowNum++;
                        }

                        rowNum = slRowNum;
                    }
                    else
                        rowNum++;
                }

                ws.Columns.AutoFit();
                ws.Rows.AutoFit();

                string reportName = "PYReport_" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss");

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = reportName;
                sfd.InitialDirectory = Constants.csvDocPath;
                sfd.Filter = "XLS (*.xls)|*.xls";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                string filePath = sfd.FileName;

                wb.SaveAs(filePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
                                    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                                    Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, misValue, misValue, misValue, misValue);

                wb.Close(true, null, null);
                xla.Workbooks.Close();
                xla.Quit();

                releaseObject(ws);
                releaseObject(wb);
                releaseObject(xla);

                System.Threading.Thread.CurrentThread.CurrentCulture = Oldci;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GeoxPYReport.generateReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isPairForStimulation(IOPairProcessedTO pair, Dictionary<string, RuleTO> emplRules, List<IOPairProcessedTO> dayPairs)
        {
            try
            {
                if ((emplRules.ContainsKey(Constants.RuleCompanyRegularWork) && pair.PassTypeID == emplRules[Constants.RuleCompanyRegularWork].RuleValue)
                    || (emplRules.ContainsKey(Constants.RuleCompanyOfficialOut) && pair.PassTypeID == emplRules[Constants.RuleCompanyOfficialOut].RuleValue)
                    || (emplRules.ContainsKey(Constants.RuleCompanyOfficialTrip) && pair.PassTypeID == emplRules[Constants.RuleCompanyOfficialTrip].RuleValue)
                    || (emplRules.ContainsKey(Constants.RuleCompanyJustifiedAbsence) && pair.PassTypeID == emplRules[Constants.RuleCompanyJustifiedAbsence].RuleValue)
                    || (emplRules.ContainsKey(Constants.RuleHolidayPassType) && pair.PassTypeID == emplRules[Constants.RuleHolidayPassType].RuleValue)
                    || (emplRules.ContainsKey(Constants.RulePersonalHolidayPassType) && pair.PassTypeID == emplRules[Constants.RulePersonalHolidayPassType].RuleValue)
                    || paidLeaves.Contains(pair.PassTypeID))
                    return true;
                else if (emplRules.ContainsKey(Constants.RuleCompanyBankHourUsed) && pair.PassTypeID == emplRules[Constants.RuleCompanyBankHourUsed].RuleValue)
                {
                    // check if pair is whole day absence
                    int bhUsedMinutes = 0;
                    foreach (IOPairProcessedTO pairTO in dayPairs)
                    {
                        if (pairTO.PassTypeID == emplRules[Constants.RuleCompanyBankHourUsed].RuleValue)
                        {
                            bhUsedMinutes += (int)pairTO.EndTime.Subtract(pairTO.StartTime).TotalMinutes;
                            if (pairTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                bhUsedMinutes++;
                        }
                    }

                    if (bhUsedMinutes < Constants.dayDurationStandardShift * 60)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.isPairFormStimulation(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void setRowFontWeight(Microsoft.Office.Interop.Excel.Worksheet ws, int row, int colNum, bool isBold)
        {
            try
            {
                for (int i = 1; i <= colNum; i++)
                {
                    ((Microsoft.Office.Interop.Excel.Range)ws.Cells[row, i]).Font.Bold = isBold;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.setRowFontWeight(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void setCellFontWeight(object cell, bool isBold)
        {
            try
            {
                ((Microsoft.Office.Interop.Excel.Range)cell).Font.Bold = isBold;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.setCellFontWeight(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.releaseObject(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        private bool isPreviousDay(DateTime prevDate, DateTime currDate)
        {
            try
            {
                if (prevDate.Date == currDate.Date.AddDays(-1))
                    return true;
                else if (prevDate.DayOfWeek == DayOfWeek.Friday)
                {
                    if (prevDate.Date == currDate.Date.AddDays(-3))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.isPreviousDay(): " + ex.Message + "\n");
                throw ex;
            }            
        }
    }
}
