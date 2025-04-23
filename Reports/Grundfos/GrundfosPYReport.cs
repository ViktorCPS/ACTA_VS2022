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

namespace Reports.Grundfos
{
    public partial class GrundfosPYReport : Form
    {
        private const int emplIDIndex = 0;
        private const int emplNameIndex = 1;
                
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

        public GrundfosPYReport()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(GrundfosPYReport).Assembly);

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
                    case GrundfosPYReport.emplIDIndex:
                        {
                            int id1 = -1;
                            int id2 = -1;

                            int.TryParse(sub1.Text, out id1);
                            int.TryParse(sub2.Text, out id2);

                            return CaseInsensitiveComparer.Default.Compare(id1, id2);
                        }
                    case GrundfosPYReport.emplNameIndex:
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
                this.Text = rm.GetString("GrundfosPYReport", culture);

                //label's text
                this.lblEmployee.Text = rm.GetString("lblEmployee", culture);
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);

                //button's text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnGenerate.Text = rm.GetString("btnGenerate", culture);
                btnView.Text = rm.GetString("btnView", culture);

                //group box text
                this.gbDateInterval.Text = rm.GetString("gbDateInterval", culture);
                this.gbUnitFilter.Text = rm.GetString("gbUnitFilter", culture);
                                
                // list view                
                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrID", culture), 75, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrName", culture), 230, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();

                lvDates.BeginUpdate();
                lvDates.Columns.Add(rm.GetString("hdrCalcID", culture), 100, HorizontalAlignment.Left);
                lvDates.Columns.Add(rm.GetString("hdrCreatedTime", culture), 200, HorizontalAlignment.Left);
                lvDates.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GrundfosPYReport.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GrundfosPYReport_Load(object sender, EventArgs e)
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
                
                //unpaidLeaves = Constants.GeoxUnpaidLeaves();                
                //sickLeaves30Days = Constants.GeoxSickLeaves30Days();
                //sickLeavesRefundation = Constants.GeoxSickLeavesRefundation();

                // get all paid leave
                Dictionary<int, List<int>> confirmationPTDict = new PassTypesConfirmation().SearchDictionary();
                List<int> sickLeaveNCF = new Common.Rule().SearchRulesExact(Constants.RuleCompanySickLeaveNCF);
                
                foreach (int ptID in confirmationPTDict.Keys)
                {
                    if (!confirmationPTDict[ptID].Contains(Constants.unpaidLeave) && !sickLeaveNCF.Contains(ptID))
                        paidLeaves.AddRange(confirmationPTDict[ptID]);
                }

                populateDateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GrundfosPYReport.GrundfosPYReport_Load(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GrundfosPYReport.populateWU(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GrundfosPYReport.populateOU(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GrundfosPYReport.populateEmployees(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GrundfosPYReport.rbWU_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GrundfosPYReport.rbOU_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GrundfosPYReport.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GrundfosPYReport.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " GrundfosPYReport.tbEmployee_TextChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " GrundfosPYReport.lvEmployees_ColumnClick(): " + ex.Message + "\n");
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

                populateDateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GrundfosPYReport.dtpFrom_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();

                populateDateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GrundfosPYReport.dtpTo_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvDates.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("selectReport", culture));
                    return;
                }

                if (lvDates.SelectedItems[0].Tag != null && lvDates.SelectedItems[0].Tag is uint)
                {
                    GrundfosPYReportView view = new GrundfosPYReportView((uint)lvDates.SelectedItems[0].Tag);
                    view.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GrundfosPYReport.btnView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateDateListView()
        {
            try
            {
                List<EmployeePYDataSumTO> list = new EmployeePYDataSum().getSumDates(dtpFrom.Value.Date, dtpTo.Value.Date);

                lvDates.BeginUpdate();
                lvDates.Items.Clear();

                foreach (EmployeePYDataSumTO sumTO in list)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = sumTO.PYCalcID.ToString().Trim();
                    item.SubItems.Add(sumTO.CreatedTime.ToString(Constants.dateFormat + " " + Constants.timeFormat));

                    item.Tag = sumTO.PYCalcID;
                    lvDates.Items.Add(item);
                }

                lvDates.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GrundfosPYReport.populateDateListView(): " + ex.Message + "\n");
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
                    }
                }

                if (unregularIDs.Count > 0)
                {
                    MessageBox.Show(rm.GetString("notConfirmedDataFound", culture) + " " + unregularData.Trim().Substring(0, unregularData.Trim().Length - 1) + ".");
                    return;
                }

                // create list of pairs foreach employee and each pair belonging day
                foreach (int employeeID in emplDict.Keys)
                {
                    if (!emplBelongingDayPairs.ContainsKey(employeeID))
                        continue;

                    foreach (DateTime date in emplBelongingDayPairs[employeeID].Keys)
                    {
                        for (int i = 0; i < emplBelongingDayPairs[employeeID][date].Count; i++)
                        {
                            IOPairProcessedTO pair = emplBelongingDayPairs[employeeID][date][i];

                            EmployeeTO empl = emplDict[employeeID];

                            EmployeeAsco4TO asco = new EmployeeAsco4TO();
                            if (ascoDict.ContainsKey(employeeID))
                                asco = ascoDict[employeeID];

                            Dictionary<string, RuleTO> rulesForEmpl = new Dictionary<string, RuleTO>();
                            if (emplRules.ContainsKey(employeeID))
                                rulesForEmpl = emplRules[employeeID];

                            WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                            if (emplDaySchemas.ContainsKey(employeeID) && emplDaySchemas[employeeID].ContainsKey(date.Date))
                                schema = emplDaySchemas[employeeID][date.Date];

                            List<DateTime> paidDays = new List<DateTime>();
                            if (emplHolidayPaidDays.ContainsKey(employeeID))
                                paidDays = emplHolidayPaidDays[employeeID];

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

                            if (!emplPTDurationSummary.ContainsKey(employeeID))
                                emplPTDurationSummary.Add(employeeID, new Dictionary<int, int>());

                            if (!emplPTDurationSummary[employeeID].ContainsKey(pair.PassTypeID))
                                emplPTDurationSummary[employeeID].Add(pair.PassTypeID, 0);

                            emplPTDurationSummary[employeeID][pair.PassTypeID] += pairDuration;

                            // calculate addition for night work, turnus and holidays
                            if (pair.PassTypeID != Constants.absence && isPresence(pair.PassTypeID, rulesForEmpl))
                            {
                                // check work on holiday
                                if (workOnHolidayPT != -1
                                    && isWorkOnHoliday(personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, nationalTransferableHolidays, paidDays, date.Date, schema, asco, empl))
                                {
                                    if (!emplPTDurationSummary[employeeID].ContainsKey(workOnHolidayPT))
                                        emplPTDurationSummary[employeeID].Add(workOnHolidayPT, 0);

                                    emplPTDurationSummary[employeeID][workOnHolidayPT] += pairDuration;

                                    if (!emplHolidayPaidDays.ContainsKey(employeeID))
                                        emplHolidayPaidDays.Add(employeeID, paidDays);
                                    else
                                        emplHolidayPaidDays[employeeID] = paidDays;
                                }

                                if (turnusPT != -1 && isTurnus(schema) && ptDict.ContainsKey(pair.PassTypeID) && ptDict[pair.PassTypeID].IsPass != Constants.overtimePassType)
                                {
                                    if (!emplPTDurationSummary[employeeID].ContainsKey(turnusPT))
                                        emplPTDurationSummary[employeeID].Add(turnusPT, 0);

                                    emplPTDurationSummary[employeeID][turnusPT] += pairDuration;
                                }

                                int nightWork = nightWorkDuration(rulesForEmpl, pair, schema);

                                if (nightWorkPT != -1 && nightWork > 0)
                                {
                                    if (!emplPTDurationSummary[employeeID].ContainsKey(nightWorkPT))
                                        emplPTDurationSummary[employeeID].Add(nightWorkPT, 0);

                                    emplPTDurationSummary[employeeID][nightWorkPT] += nightWork;
                                }
                            }
                        }
                    }
                }

                // generate xls report
                saveReportDetails(emplPTDurationSummary, dtpFrom.Value.Date, dtpTo.Value.Date);

                populateDateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GrundfosPYReport.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void saveReportDetails(Dictionary<int, Dictionary<int, int>> emplPTDurationSummary, DateTime startDate, DateTime endDate)
        {
            try
            {
                // get cuurently max calc id
                EmployeePYDataSum sum = new EmployeePYDataSum();
                uint calcID = sum.getMaxCalcID();
                calcID++;

                List<EmployeePYDataSumTO> sumList = new List<EmployeePYDataSumTO>();
                foreach (int id in emplPTDurationSummary.Keys)
                {
                    foreach (int ptID in emplPTDurationSummary[id].Keys)
                    {
                        EmployeePYDataSumTO sumTO = new EmployeePYDataSumTO();
                        sumTO.Type = Constants.PYTypeReal;                        
                        sumTO.PYCalcID = calcID;
                        sumTO.DateStart = startDate.Date;
                        sumTO.DateEnd = endDate.Date;
                        sumTO.DateStartSickness = Constants.dateTimeNullValue();
                        sumTO.EmployeeID = id;
                        sumTO.PaymentCode = ptID.ToString().Trim();
                        sumTO.HrsAmount = Math.Round((decimal)emplPTDurationSummary[id][ptID] / 60, 2);

                        sumList.Add(sumTO);
                    }
                }

                if (sum.BeginTransaction())
                {
                    try
                    {
                        bool saved = true;

                        foreach (EmployeePYDataSumTO sumTO in sumList)
                        {
                            sum.EmplSum = sumTO;
                            saved = saved && (sum.Save(false) > 0);

                            if (!saved)
                                break;
                        }

                        if (saved)
                        {
                            sum.CommitTransaction();
                            MessageBox.Show(rm.GetString("reportSaved", culture));
                        }
                        else
                        {
                            if (sum.GetTransaction() != null)
                                sum.RollbackTransaction();

                            MessageBox.Show(rm.GetString("reportNotSaved", culture));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (sum.GetTransaction() != null)
                            sum.RollbackTransaction();

                        throw ex;
                    }
                }
                else
                    MessageBox.Show(rm.GetString("reportNotSaved", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GrundfosPYReport.saveReportDetails(): " + ex.Message + "\n");
                throw ex;
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
                log.writeLog(DateTime.Now + " GrundfosPYReport.isWorkOnHoliday(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " GrundfosPYReport.isTurnus(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " GrundfosPYReport.nightWorkDuration(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isPresence(int ptID, Dictionary<string, RuleTO> emplRules)
        {
            try
            {
                bool presencePair = false;

                List<string> presenceTypes = Constants.GrundfosEffectiveWorkWageTypes();

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
                log.writeLog(DateTime.Now + " GrundfosPYReport.isPresence(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
