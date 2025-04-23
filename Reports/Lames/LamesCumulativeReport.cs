using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.Data.OleDb;

using Common;
using TransferObjects;
using Util;

namespace Reports.Lames
{
    public partial class LamesCumulativeReport : Form
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
        
        public LamesCumulativeReport()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(LamesCumulativeReport).Assembly);

                logInUser = NotificationController.GetLogInUser();

                setLanguage();

                rbWU.Checked = true;                

                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    case LamesCumulativeReport.emplIDIndex:
                        {
                            int id1 = -1;
                            int id2 = -1;

                            int.TryParse(sub1.Text, out id1);
                            int.TryParse(sub2.Text, out id2);

                            return CaseInsensitiveComparer.Default.Compare(id1, id2);
                        }                    
                    case LamesCumulativeReport.emplNameIndex:
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
                this.Text = rm.GetString("LamesCumulativeReport", culture);

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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " LamesCumulativeReport.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void LamesCumulativeReport_Load(object sender, EventArgs e)
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
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " LamesCumulativeReport.LamesCumulativeReport_Load(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " LamesCumulativeReport.populateWU(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " LamesCumulativeReport.populateOU(): " + ex.Message + "\n");
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

                    item.Tag = empl;
                    lvEmployees.Items.Add(item);
                }

                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " LamesCumulativeReport.populateEmployees(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " LamesCumulativeReport.rbWU_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " LamesCumulativeReport.rbOU_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " LamesCumulativeReport.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " LamesCumulativeReport.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " LamesCumulativeReport.tbEmployee_TextChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " LamesCumulativeReport.lvEmployees_ColumnClick(): " + ex.Message + "\n");
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
                
                // analitical dictionary
                Dictionary<int, Dictionary<DateTime, Dictionary<int, int>>> emplDayPTDuration = new Dictionary<int, Dictionary<DateTime, Dictionary<int, int>>>();

                // summary dicitionaries
                Dictionary<int, Dictionary<int, int>> emplPTDuration = new Dictionary<int, Dictionary<int, int>>();
                Dictionary<int, Dictionary<DateTime, int>> emplDayDuration = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, int> ptDuration = new Dictionary<int, int>();
                Dictionary<int, int> emplDuration = new Dictionary<int, int>();                
                Dictionary<int, List<DateTime>> emplWorkingDays = new Dictionary<int, List<DateTime>>();
                int totalDuration = 0;
                int totalWorkingDays = 0;

                // create list of pairs foreach employee and each day
                foreach (IOPairProcessedTO pair in allPairs)
                {
                    if (!emplDayPairs.ContainsKey(pair.EmployeeID))
                        emplDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                    if (!emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        emplDayPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());
                    emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                }

                // create list of pairs foreach employee and each pair belonging day                
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
                        if (!emplBelongingDayPairs.ContainsKey(pair.EmployeeID))
                            emplBelongingDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                        if (!emplBelongingDayPairs[pair.EmployeeID].ContainsKey(pairDate.Date))
                            emplBelongingDayPairs[pair.EmployeeID].Add(pairDate.Date, new List<IOPairProcessedTO>());

                        emplBelongingDayPairs[pair.EmployeeID][pairDate.Date].Add(pair);

                        if (!emplDayPTDuration.ContainsKey(pair.EmployeeID))
                            emplDayPTDuration.Add(pair.EmployeeID, new Dictionary<DateTime,Dictionary<int,int>>());
                        if (!emplDayPTDuration[pair.EmployeeID].ContainsKey(pairDate.Date))
                            emplDayPTDuration[pair.EmployeeID].Add(pairDate.Date, new Dictionary<int, int>());
                        if (!emplDayPTDuration[pair.EmployeeID][pairDate.Date].ContainsKey(pair.PassTypeID))
                            emplDayPTDuration[pair.EmployeeID][pairDate.Date].Add(pair.PassTypeID, 0);

                        int pairDuration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                        if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                            pairDuration++;

                        emplDayPTDuration[pair.EmployeeID][pairDate.Date][pair.PassTypeID] += pairDuration;
                        
                        if (!ptDuration.ContainsKey(pair.PassTypeID))
                            ptDuration.Add(pair.PassTypeID, 0);

                        ptDuration[pair.PassTypeID] += pairDuration;

                        if (!emplDuration.ContainsKey(pair.EmployeeID))
                            emplDuration.Add(pair.EmployeeID, 0);

                        emplDuration[pair.EmployeeID] += pairDuration;

                        if (!emplPTDuration.ContainsKey(pair.EmployeeID))
                            emplPTDuration.Add(pair.EmployeeID, new Dictionary<int, int>());
                        if (!emplPTDuration[pair.EmployeeID].ContainsKey(pair.PassTypeID))
                            emplPTDuration[pair.EmployeeID].Add(pair.PassTypeID, 0);

                        emplPTDuration[pair.EmployeeID][pair.PassTypeID] += pairDuration;

                        if (!emplDayDuration.ContainsKey(pair.EmployeeID))
                            emplDayDuration.Add(pair.EmployeeID, new Dictionary<DateTime, int>());
                        if (!emplDayDuration[pair.EmployeeID].ContainsKey(pairDate.Date))
                            emplDayDuration[pair.EmployeeID].Add(pairDate.Date, 0);

                        emplDayDuration[pair.EmployeeID][pairDate.Date] += pairDuration;

                        if (isPresencePair(pair, emplDict))
                        {
                            if (!emplWorkingDays.ContainsKey(pair.EmployeeID))
                                emplWorkingDays.Add(pair.EmployeeID, new List<DateTime>());

                            if (!emplWorkingDays[pair.EmployeeID].Contains(pairDate.Date))
                            {
                                emplWorkingDays[pair.EmployeeID].Add(pairDate.Date);
                                totalWorkingDays++;
                            }
                        }

                        totalDuration += pairDuration;
                    }
                }

                // generate xls report
                generateXLSReport(emplDict, emplDayPTDuration, emplPTDuration, ptDuration, emplDuration, emplDayDuration, emplWorkingDays, totalDuration, totalWorkingDays, from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LamesCumulativeReport.btnGenerateReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void generateXLSReport(Dictionary<int, EmployeeTO> emplDict, Dictionary<int, Dictionary<DateTime, Dictionary<int, int>>> emplDayPTDuration, 
            Dictionary<int, Dictionary<int, int>> emplPTDuration, Dictionary<int, int> ptDuration, Dictionary<int, int> emplDuration, Dictionary<int, Dictionary<DateTime, int>> emplDayDuration, 
            Dictionary<int, List<DateTime>> emplWorkingDays, int totalDuration, int totalWorkingDays, DateTime from, DateTime to)
        {
            try
            {
                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                object misValue = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet wsSummary = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;                
                Microsoft.Office.Interop.Excel.Worksheet wsAnalitycal = (Microsoft.Office.Interop.Excel.Worksheet)xla.Sheets.Add(misValue, misValue, misValue, misValue);

                //wsSummary.Name = rm.GetString("Summary", culture);
                //wsAnalitycal.Name = rm.GetString("analitycal", culture);
                                
                // insert header
                wsSummary.Cells[1, 1] = rm.GetString("period", culture) + ": " + from.ToString(Constants.dateFormat) + "-" + to.ToString(Constants.dateFormat);
                
                wsSummary.Cells[2, 1] = rm.GetString("hdrID", culture);
                wsSummary.Cells[2, 2] = rm.GetString("hdrName", culture);
                wsSummary.Cells[2, 3] = rm.GetString("hdrWU", culture);
                wsSummary.Cells[2, 4] = rm.GetString("hdrOU", culture);

                wsAnalitycal.Cells[1, 1] = rm.GetString("period", culture) + ": " + from.ToString(Constants.dateFormat) + "-" + to.ToString(Constants.dateFormat);

                wsAnalitycal.Cells[2, 1] = rm.GetString("hdrID", culture);
                wsAnalitycal.Cells[2, 2] = rm.GetString("hdrName", culture);
                wsAnalitycal.Cells[2, 3] = rm.GetString("hdrWU", culture);
                wsAnalitycal.Cells[2, 4] = rm.GetString("hdrOU", culture);

                int colNum = 5;
                foreach (int ptID in ptDict.Keys)
                {
                    if (!ptDuration.ContainsKey(ptID))
                        continue;

                    string ptDesc = ptDict[ptID].DescriptionAndID;

                    if (NotificationController.GetLogInUser().LangCode.Trim().ToUpper().Equals(Constants.Lang_en.Trim().ToUpper()))
                        ptDesc = ptDict[ptID].DescriptionAltAndID;

                    wsSummary.Cells[2, colNum] = ptDesc.Trim();

                    wsAnalitycal.Cells[2, colNum] = ptDesc.Trim();

                    colNum++;
                }

                wsSummary.Cells[2, colNum] = rm.GetString("hdrTotal", culture);
                wsAnalitycal.Cells[2, colNum] = rm.GetString("hdrTotal", culture);

                colNum++;

                wsSummary.Cells[2, colNum] = rm.GetString("hdrWorkingDays", culture);
                wsAnalitycal.Cells[2, colNum] = rm.GetString("hdrWorkingDays", culture);

                setRowFontWeight(wsSummary, 1, colNum, true);
                setRowFontWeight(wsSummary, 2, colNum, true);
                setRowFontWeight(wsAnalitycal, 1, colNum, true);
                setRowFontWeight(wsAnalitycal, 2, colNum, true);
                
                int rowS = 3;
                int rowA = 3;
                foreach (int emplID in emplDict.Keys)
                {
                    // insert employee data
                    wsSummary.Cells[rowS, 1] = emplDict[emplID].EmployeeID.ToString().Trim();
                    wsSummary.Cells[rowS, 2] = emplDict[emplID].FirstAndLastName.Trim();
                    wsAnalitycal.Cells[rowA, 1] = emplDict[emplID].EmployeeID.ToString().Trim();
                    wsAnalitycal.Cells[rowA, 2] = emplDict[emplID].FirstAndLastName.Trim();
                    if (wuDict.ContainsKey(emplDict[emplID].WorkingUnitID))
                    {
                        wsSummary.Cells[rowS, 3] = wuDict[emplDict[emplID].WorkingUnitID].Name.Trim();
                        wsAnalitycal.Cells[rowA, 3] = wuDict[emplDict[emplID].WorkingUnitID].Name.Trim();
                    }
                    else
                    {
                        wsSummary.Cells[rowS, 3] = "";
                        wsAnalitycal.Cells[rowA, 3] = "";
                    }
                    if (ouDict.ContainsKey(emplDict[emplID].OrgUnitID))
                    {
                        wsSummary.Cells[rowS, 4] = ouDict[emplDict[emplID].OrgUnitID].Name.Trim();
                        wsAnalitycal.Cells[rowA, 4] = ouDict[emplDict[emplID].OrgUnitID].Name.Trim();
                    }
                    else
                    {
                        wsSummary.Cells[rowS, 4] = "";
                        wsAnalitycal.Cells[rowA, 4] = "";
                    }

                    // insert analytical by days
                    rowA++;
                    DateTime day = from.Date;
                    while (day.Date <= to.Date)
                    {
                        wsAnalitycal.Cells[rowA, 2] = day.ToString(Constants.dateFormat);

                        // insert pass types
                        int ptIndex = 5;
                        foreach (int ptID in ptDict.Keys)
                        {
                            if (!ptDuration.ContainsKey(ptID))
                                continue;

                            if (emplDayPTDuration.ContainsKey(emplID) && emplDayPTDuration[emplID].ContainsKey(day.Date) && emplDayPTDuration[emplID][day.Date].ContainsKey(ptID))
                                wsAnalitycal.Cells[rowA, ptIndex] = ((double)emplDayPTDuration[emplID][day.Date][ptID] / 60).ToString(Constants.doubleFormat);
                            else
                                wsAnalitycal.Cells[rowA, ptIndex] = "";

                            ptIndex++;
                        }

                        if (emplDayDuration.ContainsKey(emplID) && emplDayDuration[emplID].ContainsKey(day.Date))
                            wsAnalitycal.Cells[rowA, ptIndex] = ((double)emplDayDuration[emplID][day.Date] / 60).ToString(Constants.doubleFormat);
                        else
                            wsAnalitycal.Cells[rowA, ptIndex] = ((double)0).ToString(Constants.doubleFormat);

                        setCellFontWeight(wsAnalitycal.Cells[rowA, ptIndex], true);

                        ptIndex++;

                        if (emplWorkingDays.ContainsKey(emplID) && emplWorkingDays[emplID].Contains(day.Date))
                            wsAnalitycal.Cells[rowA, ptIndex] = "1";
                        else
                            wsAnalitycal.Cells[rowA, ptIndex] = "0";

                        setCellFontWeight(wsAnalitycal.Cells[rowA, ptIndex], true);

                        day = day.AddDays(1);
                        rowA++;
                    }

                    wsAnalitycal.Cells[rowA, 4] = rm.GetString("hdrTotal", culture);

                    // insert pass types total
                    int ptColIndex = 5;
                    foreach (int ptID in ptDict.Keys)
                    {
                        if (!ptDuration.ContainsKey(ptID))
                            continue;

                        if (emplPTDuration.ContainsKey(emplID) && emplPTDuration[emplID].ContainsKey(ptID))
                        {
                            wsSummary.Cells[rowS, ptColIndex] = ((double)emplPTDuration[emplID][ptID] / 60).ToString(Constants.doubleFormat);
                            wsAnalitycal.Cells[rowA, ptColIndex] = ((double)emplPTDuration[emplID][ptID] / 60).ToString(Constants.doubleFormat);
                        }
                        else
                        {
                            wsSummary.Cells[rowS, ptColIndex] = "";
                            wsAnalitycal.Cells[rowA, ptColIndex] = ((double)0).ToString(Constants.doubleFormat);
                        }

                        ptColIndex++;
                    }

                    if (emplDuration.ContainsKey(emplID))
                    {
                        wsSummary.Cells[rowS, ptColIndex] = ((double)emplDuration[emplID] / 60).ToString(Constants.doubleFormat);
                        wsAnalitycal.Cells[rowA, ptColIndex] = ((double)emplDuration[emplID] / 60).ToString(Constants.doubleFormat);
                    }
                    else
                    {
                        wsSummary.Cells[rowS, ptColIndex] = ((double)0).ToString(Constants.doubleFormat);
                        wsAnalitycal.Cells[rowA, ptColIndex] = ((double)0).ToString(Constants.doubleFormat);
                    }

                    setCellFontWeight(wsSummary.Cells[rowS, ptColIndex], true);

                    ptColIndex++;

                    if (emplWorkingDays.ContainsKey(emplID))
                    {
                        wsSummary.Cells[rowS, ptColIndex] = emplWorkingDays[emplID].Count.ToString();
                        wsAnalitycal.Cells[rowA, ptColIndex] = emplWorkingDays[emplID].Count.ToString();
                    }
                    else
                    {
                        wsSummary.Cells[rowS, ptColIndex] = "0";
                        wsAnalitycal.Cells[rowA, ptColIndex] = "0";
                    }
                    
                    setCellFontWeight(wsSummary.Cells[rowS, ptColIndex], true);
                    setRowFontWeight(wsAnalitycal, rowA, colNum, true);

                    rowS++;
                    rowA++;
                }

                rowS++;
                rowA++;

                // insert total line
                wsSummary.Cells[rowS, 4] = rm.GetString("hdrTotalAll", culture);
                wsAnalitycal.Cells[rowA, 4] = rm.GetString("hdrTotalAll", culture);

                // insert pass types
                int index = 5;
                foreach (int ptID in ptDict.Keys)
                {
                    if (!ptDuration.ContainsKey(ptID))
                        continue;

                    wsSummary.Cells[rowS, index] = ((double)ptDuration[ptID] / 60).ToString(Constants.doubleFormat);
                    wsAnalitycal.Cells[rowA, index] = ((double)ptDuration[ptID] / 60).ToString(Constants.doubleFormat);
                    
                    index++;
                }

                wsSummary.Cells[rowS, index] = ((double)totalDuration / 60).ToString(Constants.doubleFormat);
                wsAnalitycal.Cells[rowA, index] = ((double)totalDuration / 60).ToString(Constants.doubleFormat);

                index++;

                wsSummary.Cells[rowS, index] = totalWorkingDays.ToString();
                wsAnalitycal.Cells[rowA, index] = totalWorkingDays.ToString();

                setRowFontWeight(wsSummary, rowS, colNum, true);
                setRowFontWeight(wsAnalitycal, rowA, colNum, true);

                wsSummary.Columns.AutoFit();
                wsSummary.Rows.AutoFit();
                wsAnalitycal.Columns.AutoFit();
                wsAnalitycal.Rows.AutoFit();

                string reportName = "CumulativeReport_" + DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss");

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

                releaseObject(wsSummary);
                releaseObject(wsAnalitycal);
                releaseObject(wb);
                releaseObject(xla);

                System.Threading.Thread.CurrentThread.CurrentCulture = Oldci;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LamesCumulativeReport.generateXLSReport(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " LamesCumulativeReport.setRowFontWeight(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " LamesCumulativeReport.setCellFontWeight(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void setCellBorder(object cell)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Range range = ((Microsoft.Office.Interop.Excel.Range)cell);
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " LamesCumulativeReport.setCellBorder(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " LamesCumulativeReport.releaseObject(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        private bool isPresencePair(IOPairProcessedTO pair, Dictionary<int, EmployeeTO> emplDict)
        {
            try
            {
                int regularWorkPT = -1;
                int overtimeRejectedPT = -1;

                int company = -1;

                Dictionary<string, RuleTO> emplRules = new Dictionary<string,RuleTO>();

                if (emplDict.ContainsKey(pair.EmployeeID))
                {
                    company = Common.Misc.getRootWorkingUnit(emplDict[pair.EmployeeID].WorkingUnitID, wuDict);

                    if (rules.ContainsKey(company) && rules[company].ContainsKey(emplDict[pair.EmployeeID].EmployeeTypeID))
                        emplRules = rules[company][emplDict[pair.EmployeeID].EmployeeTypeID];
                }

                if (emplRules.ContainsKey(Constants.RuleCompanyRegularWork))
                    regularWorkPT = emplRules[Constants.RuleCompanyRegularWork].RuleValue;

                if (emplRules.ContainsKey(Constants.RuleCompanyOvertimeRejected))
                    overtimeRejectedPT = emplRules[Constants.RuleCompanyOvertimeRejected].RuleValue;

                if (pair.PassTypeID == regularWorkPT || (ptDict.ContainsKey(pair.PassTypeID) && ptDict[pair.PassTypeID].IsPass == Constants.overtimePassType && pair.PassTypeID != overtimeRejectedPT))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " LamesCumulativeReport.isPresencePair(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
