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

namespace Reports.PMC
{
    public partial class PMCCumulativeReports : Form
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
        Dictionary<int, string> emplTypes = new Dictionary<int, string>();

        public PMCCumulativeReports()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(PMCCumulativeReports).Assembly);

                logInUser = NotificationController.GetLogInUser();

                setLanguage();

                rbWU.Checked = true;
                chbHierarhiclyWU.Checked = true;
                chbHierachyOU.Checked = true;

                rbAbsence.Checked = true;
                
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
                    case PMCCumulativeReports.emplIDIndex:
                        {
                            int id1 = -1;
                            int id2 = -1;

                            int.TryParse(sub1.Text, out id1);
                            int.TryParse(sub2.Text, out id2);

                            return CaseInsensitiveComparer.Default.Compare(id1, id2);
                        }
                    case PMCCumulativeReports.emplNameIndex:
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
                this.Text = rm.GetString("PMCCumulativeReports", culture);

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
                this.gbCategory.Text = rm.GetString("gbCategory", culture);

                // radio button's text
                this.rbAbsence.Text = rm.GetString("rbAbsence", culture);
                this.rbAbsenceByType.Text = rm.GetString("rbAbsenceByType", culture);
                this.rbOvertime.Text = rm.GetString("rbOvertime", culture);
                this.rbDelay.Text = rm.GetString("rbDelay", culture);

                // check box text
                this.chbHierarhiclyWU.Text = rm.GetString("chbHierarhicly", culture);
                this.chbHierachyOU.Text = rm.GetString("chbHierarhicly", culture);

                // list view                
                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrID", culture), 75, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrName", culture), lvEmployees.Width - 100, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();

                lvCategory.BeginUpdate();
                lvCategory.Columns.Add(rm.GetString("hdrCategory", culture), lvCategory.Width - 22, HorizontalAlignment.Left);
                lvCategory.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void PMCCumulativeReports_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Initialize comparer objects
                _comp = new ListViewItemComparer(lvEmployees);
                _comp.SortColumn = emplNameIndex;
                lvEmployees.ListViewItemSorter = _comp;
                lvEmployees.Sorting = SortOrder.Ascending;
                                
                ouDict = new OrganizationalUnit().SearchDictionary();
                wuDict = new WorkingUnit().getWUDictionary();
                schemas = new TimeSchema().getDictionary();
                ptDict = new PassType().SearchDictionary();

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

                Dictionary<int, Dictionary<int, string>> emplTypesAll = new EmployeeType().SearchDictionary();

                foreach (int company in emplTypesAll.Keys)
                {
                    foreach (int type in emplTypesAll[company].Keys)
                    {
                        if (!emplTypes.ContainsKey(type))
                            emplTypes.Add(type, emplTypesAll[company][type].Trim().ToUpper());
                    }
                }

                populateWU();
                populateOU();
                populateCategories();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.PMCCumulativeReports_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateCategories()
        {
            try
            {
                lvCategory.BeginUpdate();
                lvCategory.Items.Clear();

                foreach (int type in emplTypes.Keys)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = emplTypes[type];
                    item.Tag = type;
                    item.ToolTipText = type.ToString().Trim();
                    lvCategory.Items.Add(item);
                }

                lvCategory.EndUpdate();
                lvCategory.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PMCCumulativeReports.populateCategories(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void rbWU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cbWU.Enabled = rbWU.Checked;
                cbOU.Enabled = !rbWU.Checked;
                chbHierarhiclyWU.Enabled = rbWU.Checked;
                chbHierachyOU.Enabled = !rbWU.Checked;

                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.rbWU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbOU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cbWU.Enabled = !rbOU.Checked;
                cbOU.Enabled = rbOU.Checked;
                chbHierarhiclyWU.Enabled = !rbOU.Checked;
                chbHierachyOU.Enabled = rbOU.Checked;

                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.rbOU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.populateWU(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.populateOU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateEmployees()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<EmployeeTO> employeeList = new List<EmployeeTO>();

                DateTime from = dtpFrom.Value.Date;
                DateTime to = dtpTo.Value.Date;

                if (from.Date > to.Date)
                {
                    MessageBox.Show(rm.GetString("invalidDateInterval", culture));
                    return;
                }

                bool isWU = rbWU.Checked;

                int ID = -1;
                if (isWU && cbWU.SelectedIndex > 0)
                    ID = (int)cbWU.SelectedValue;
                else if (!isWU && cbOU.SelectedIndex > 0)
                    ID = (int)cbOU.SelectedValue;

                if (ID != -1)
                {
                    if (isWU)
                    {
                        string wunits = "";
                        if (chbHierarhiclyWU.Checked)
                            wunits = Common.Misc.getWorkingUnitHierarhicly(ID, wuList, null);
                        else
                            wunits = ID.ToString().Trim();

                        // get employees from selected working unit that are not currently loaned to other working unit or are currently loand to selected working unit                        
                        employeeList = new Employee().SearchByWULoans(wunits, -1, null, from.Date, to.Date);
                    }
                    else
                    {
                        string ounits = "";
                        if (chbHierachyOU.Checked)
                            ounits = Common.Misc.getOrgUnitHierarhicly(ID.ToString(), ouList, null);
                        else
                            ounits = ID.ToString().Trim();

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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarhiclyWU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.chbHierarhiclyWU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbHierachyOU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.chbHierachyOU_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
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

                tbEmployee.Focus();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PMCCumulativeReports.tbEmployee_TextChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " PMCCumulativeReports.lvEmployees_ColumnClick(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.dtpFrom_ValueChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.dtpTo_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (dtpFrom.Value.Date > dtpTo.Value.Date)
                {
                    MessageBox.Show(rm.GetString("invalidDateInterval", culture));
                    return;
                }

                // get selected types (categories)
                List<int> selectedTypes = new List<int>();
                if (lvCategory.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvCategory.SelectedItems)
                    {
                        selectedTypes.Add((int)item.Tag);
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvCategory.Items)
                    {
                        selectedTypes.Add((int)item.Tag);
                    }
                }

                string emplIDs = "";

                if (lvEmployees.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvEmployees.SelectedItems)
                    {
                        if (!selectedTypes.Contains(((EmployeeTO)item.Tag).EmployeeTypeID))
                            continue;

                        emplIDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        if (!selectedTypes.Contains(((EmployeeTO)item.Tag).EmployeeTypeID))
                            continue;

                        emplIDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";
                    }
                }

                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                // get all pass types
                string ptIDs = "";
                List<int> ptList = new List<int>();
                List<int> delayList = new Common.Rule().SearchRulesExact(Constants.RuleCompanyDelay);
                foreach (int id in ptDict.Keys)
                {
                    if (((rbAbsence.Checked || rbAbsenceByType.Checked) && ptDict[id].IsPass == Constants.wholeDayAbsence)
                        || (rbOvertime.Checked && ptDict[id].IsPass == Constants.overtimePassType)
                        || (rbDelay.Checked && (delayList.Contains(id) || id == Constants.absence)))
                    {
                        ptList.Add(id);
                        ptIDs += id.ToString().Trim() + ",";
                    }
                }

                if (ptIDs.Length > 0)
                    ptIDs = ptIDs.Substring(0, ptIDs.Length - 1);

                if (emplIDs.Length <= 0 || ptIDs.Length <= 0)
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }

                Dictionary<int, EmployeeTO> emplDict = new Employee().SearchDictionary(emplIDs);
                
                List<DateTime> dates = new List<DateTime>();
                for (DateTime currDate = dtpFrom.Value.Date.AddDays(1); currDate.Date <= dtpTo.Value.Date; currDate = currDate.Date.AddDays(1))
                {
                    dates.Add(currDate.Date);
                }

                // get pairs of specific types
                List<IOPairProcessedTO> emplPairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, dates, ptIDs);

                // get border days pairs
                dates = new List<DateTime>();
                dates.Add(dtpFrom.Value.Date);
                dates.Add(dtpTo.Value.Date.AddDays(1));                
                List<IOPairProcessedTO> borderDayPairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, dates, "");
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplBorderDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                foreach (IOPairProcessedTO pair in borderDayPairs)
                {
                    if (!emplBorderDayPairs.ContainsKey(pair.EmployeeID))
                        emplBorderDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime,List<IOPairProcessedTO>>());

                    if (!emplBorderDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        emplBorderDayPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                    emplBorderDayPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                }

                // get schedules and time schemas
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1), null);
                
                // get pairs from border days that belongs to selected period
                foreach (int emplID in emplBorderDayPairs.Keys)
                {
                    foreach (DateTime date in emplBorderDayPairs[emplID].Keys)
                    {
                        foreach (IOPairProcessedTO pair in emplBorderDayPairs[emplID][date])
                        {
                            if (!ptList.Contains(pair.PassTypeID))
                                continue;

                            List<EmployeeTimeScheduleTO> schedules = new List<EmployeeTimeScheduleTO>();
                            if (emplSchedules.ContainsKey(emplID))
                                schedules = emplSchedules[emplID];
                            List<IOPairProcessedTO> dayPairs = emplBorderDayPairs[emplID][date];
                            bool prevDayPair = Common.Misc.isPreviousDayPair(pair, ptDict, dayPairs, Common.Misc.getTimeSchema(date.Date, schedules, schemas), Common.Misc.getTimeSchemaInterval(date.Date, schedules, schemas));

                            if ((date.Date == dtpFrom.Value.Date && prevDayPair) || (date.Date == dtpTo.Value.Date.AddDays(1) && !prevDayPair))
                                continue;

                            emplPairs.Add(pair);
                        }
                    }
                }

                // absence reports structures
                Dictionary<int, Dictionary<int, int>> emplCategoryPositionsAbsence = new Dictionary<int, Dictionary<int, int>>();
                Dictionary<int, Dictionary<int, int>> emplCategoryLinesAbsence = new Dictionary<int, Dictionary<int, int>>();
                Dictionary<int, Dictionary<int, int>> emplCategoryAbsencePassType = new Dictionary<int, Dictionary<int, int>>();
                Dictionary<int, Dictionary<int, int>> emplPositionAbsencePassType = new Dictionary<int, Dictionary<int, int>>();
                Dictionary<int, Dictionary<int, int>> emplLinesAbsencePassType = new Dictionary<int, Dictionary<int, int>>();
                Dictionary<int, int> emplCategoryAbsence = new Dictionary<int, int>();
                Dictionary<int, int> emplPositionsAbsence = new Dictionary<int, int>();
                Dictionary<int, int> emplLinesAbsence = new Dictionary<int, int>();
                Dictionary<int, int> passTypesAbsence = new Dictionary<int, int>();
                int absenceTotal = 0;

                // overtime report structures                
                Dictionary<int, Dictionary<int, int>> emplCategoryOvertimePassType = new Dictionary<int, Dictionary<int, int>>();
                Dictionary<int, int> emplCategoryOvertime = new Dictionary<int, int>();
                Dictionary<int, int> passTypesOvertime = new Dictionary<int, int>();
                int overtimeTotal = 0;

                // delay report structures
                Dictionary<int, Dictionary<DateTime, int>> emplDelays = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> emplEarlyLeaves = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, List<IOPairTO>>> emplRealPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairTO>>>();
                
                List<int> emplList = new List<int>();
                foreach (IOPairProcessedTO pair in emplPairs)
                {
                    if (!emplList.Contains(pair.EmployeeID))
                        emplList.Add(pair.EmployeeID);

                    EmployeeTO empl = new EmployeeTO();
                    if (emplDict.ContainsKey(pair.EmployeeID))
                        empl = emplDict[pair.EmployeeID];

                    if (rbAbsence.Checked || rbAbsenceByType.Checked)
                    {
                        if (pair.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                            continue;

                        if (!emplCategoryPositionsAbsence.ContainsKey(empl.EmployeeTypeID))
                            emplCategoryPositionsAbsence.Add(empl.EmployeeTypeID, new Dictionary<int, int>());

                        if (!emplCategoryPositionsAbsence[empl.EmployeeTypeID].ContainsKey(empl.WorkingUnitID))
                            emplCategoryPositionsAbsence[empl.EmployeeTypeID].Add(empl.WorkingUnitID, 0);

                        emplCategoryPositionsAbsence[empl.EmployeeTypeID][empl.WorkingUnitID]++;

                        if (!emplCategoryLinesAbsence.ContainsKey(empl.EmployeeTypeID))
                            emplCategoryLinesAbsence.Add(empl.EmployeeTypeID, new Dictionary<int, int>());

                        if (!emplCategoryLinesAbsence[empl.EmployeeTypeID].ContainsKey(empl.OrgUnitID))
                            emplCategoryLinesAbsence[empl.EmployeeTypeID].Add(empl.OrgUnitID, 0);

                        emplCategoryLinesAbsence[empl.EmployeeTypeID][empl.OrgUnitID]++;

                        if (!emplCategoryAbsencePassType.ContainsKey(empl.EmployeeTypeID))
                            emplCategoryAbsencePassType.Add(empl.EmployeeTypeID, new Dictionary<int, int>());

                        if (!emplCategoryAbsencePassType[empl.EmployeeTypeID].ContainsKey(pair.PassTypeID))
                            emplCategoryAbsencePassType[empl.EmployeeTypeID].Add(pair.PassTypeID, 0);

                        emplCategoryAbsencePassType[empl.EmployeeTypeID][pair.PassTypeID]++;

                        if (!emplPositionAbsencePassType.ContainsKey(empl.WorkingUnitID))
                            emplPositionAbsencePassType.Add(empl.WorkingUnitID, new Dictionary<int, int>());

                        if (!emplPositionAbsencePassType[empl.WorkingUnitID].ContainsKey(pair.PassTypeID))
                            emplPositionAbsencePassType[empl.WorkingUnitID].Add(pair.PassTypeID, 0);

                        emplPositionAbsencePassType[empl.WorkingUnitID][pair.PassTypeID]++;

                        if (!emplLinesAbsencePassType.ContainsKey(empl.OrgUnitID))
                            emplLinesAbsencePassType.Add(empl.OrgUnitID, new Dictionary<int, int>());

                        if (!emplLinesAbsencePassType[empl.OrgUnitID].ContainsKey(pair.PassTypeID))
                            emplLinesAbsencePassType[empl.OrgUnitID].Add(pair.PassTypeID, 0);

                        emplLinesAbsencePassType[empl.OrgUnitID][pair.PassTypeID]++;

                        if (!emplCategoryAbsence.ContainsKey(empl.EmployeeTypeID))
                            emplCategoryAbsence.Add(empl.EmployeeTypeID, 0);

                        emplCategoryAbsence[empl.EmployeeTypeID]++;

                        if (!emplPositionsAbsence.ContainsKey(empl.WorkingUnitID))
                            emplPositionsAbsence.Add(empl.WorkingUnitID, 0);

                        emplPositionsAbsence[empl.WorkingUnitID]++;

                        if (!passTypesAbsence.ContainsKey(pair.PassTypeID))
                            passTypesAbsence.Add(pair.PassTypeID, 0);

                        passTypesAbsence[pair.PassTypeID]++;

                        if (!emplLinesAbsence.ContainsKey(empl.OrgUnitID))
                            emplLinesAbsence.Add(empl.OrgUnitID, 0);

                        emplLinesAbsence[empl.OrgUnitID]++;

                        absenceTotal++;
                    }

                    if (rbOvertime.Checked)
                    {
                        int pairDuration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;
                        if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                            pairDuration++;                            
                        
                        if (!emplCategoryOvertimePassType.ContainsKey(empl.EmployeeTypeID))
                            emplCategoryOvertimePassType.Add(empl.EmployeeTypeID, new Dictionary<int, int>());

                        if (!emplCategoryOvertimePassType[empl.EmployeeTypeID].ContainsKey(pair.PassTypeID))
                            emplCategoryOvertimePassType[empl.EmployeeTypeID].Add(pair.PassTypeID, 0);

                        emplCategoryOvertimePassType[empl.EmployeeTypeID][pair.PassTypeID] += pairDuration;
                        
                        if (!emplCategoryOvertime.ContainsKey(empl.EmployeeTypeID))
                            emplCategoryOvertime.Add(empl.EmployeeTypeID, 0);

                        emplCategoryOvertime[empl.EmployeeTypeID] += pairDuration;
                        
                        if (!passTypesOvertime.ContainsKey(pair.PassTypeID))
                            passTypesOvertime.Add(pair.PassTypeID, 0);

                        passTypesOvertime[pair.PassTypeID] += pairDuration;                        

                        overtimeTotal += pairDuration;
                    }
                }

                if (rbAbsence.Checked)
                    generateAbsenceReport(emplPositionsAbsence, emplCategoryAbsence, emplLinesAbsence, emplCategoryPositionsAbsence, emplCategoryLinesAbsence, absenceTotal);

                if (rbAbsenceByType.Checked)
                    generateAbsenceByTypeReport(emplPositionsAbsence, emplCategoryAbsence, emplLinesAbsence, passTypesAbsence, emplCategoryAbsencePassType, 
                        emplPositionAbsencePassType, emplLinesAbsencePassType, absenceTotal);

                if (rbOvertime.Checked)
                    generateOvertimeReport(emplCategoryOvertime, passTypesOvertime, emplCategoryOvertimePassType, overtimeTotal);

                if (rbDelay.Checked)
                {
                    // get real pairs
                    List<IOPairTO> pairs = new IOPair().SearchAll(dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1), emplList);

                    foreach (IOPairTO pair in pairs)
                    {
                        pair.StartTime = pair.StartTime.AddSeconds(-pair.StartTime.Second);
                        pair.EndTime = pair.EndTime.AddSeconds(-pair.EndTime.Second);

                        if (!emplRealPairs.ContainsKey(pair.EmployeeID))
                            emplRealPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairTO>>());

                        if (!emplRealPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                            emplRealPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairTO>());

                        emplRealPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                    }

                    Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>> emplDayIntervals = new Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>>();

                    foreach (IOPairProcessedTO pairTO in emplPairs)
                    {
                        // skip extraordinary employees
                        if (emplDict.ContainsKey(pairTO.EmployeeID) && emplDict[pairTO.EmployeeID].Type.Trim().ToUpper() == Constants.emplExtraOrdinary.Trim().ToUpper())
                            continue;

                        // get intervals for that day
                        List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();

                        if (!emplDayIntervals.ContainsKey(pairTO.EmployeeID))
                        emplDayIntervals.Add(pairTO.EmployeeID, new Dictionary<DateTime, List<WorkTimeIntervalTO>>());

                        if (!emplDayIntervals[pairTO.EmployeeID].ContainsKey(pairTO.IOPairDate.Date))
                        {
                            List<EmployeeTimeScheduleTO> schedules = new List<EmployeeTimeScheduleTO>();
                            if (emplSchedules.ContainsKey(pairTO.EmployeeID))
                                schedules = emplSchedules[pairTO.EmployeeID];

                            emplDayIntervals[pairTO.EmployeeID].Add(pairTO.IOPairDate.Date, Common.Misc.getTimeSchemaInterval(pairTO.IOPairDate.Date, schedules, schemas));
                        }

                        dayIntervals = emplDayIntervals[pairTO.EmployeeID][pairTO.IOPairDate.Date];
                        
                        // get pair interval
                        WorkTimeIntervalTO pairInterval = new WorkTimeIntervalTO();
                        bool intervalFound = false;
                        foreach (WorkTimeIntervalTO interval in dayIntervals)
                        {
                            if (interval.StartTime.TimeOfDay <= pairTO.StartTime.TimeOfDay && interval.EndTime.TimeOfDay >= pairTO.EndTime.TimeOfDay)
                            {
                                pairInterval = interval;
                                intervalFound = true;
                                break;
                            }
                        }

                        if (!intervalFound || pairInterval.StartTime.TimeOfDay == pairInterval.EndTime.TimeOfDay)
                            continue;

                        bool calculateDalay = false;
                        bool calculateLeave = false;
                        // if pairs is dalay, check real pairs for delay
                        if (delayList.Contains(pairTO.PassTypeID))
                            calculateDalay = true;

                        if (pairTO.PassTypeID == Constants.absence)
                        {
                            // if pair is absence at the begining of the shift, check real pair for delay
                            if (pairInterval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0) && pairTO.StartTime.TimeOfDay == pairInterval.StartTime.TimeOfDay)
                                calculateDalay = true;

                            // if pair is absence at the end of the shift, check real pair for earlier left
                            if (pairInterval.EndTime.TimeOfDay != new TimeSpan(23, 59, 0) && pairTO.EndTime.TimeOfDay == pairInterval.EndTime.TimeOfDay)
                                calculateLeave = true;
                        }

                        if (calculateDalay)
                        {
                            // get first pair from interval
                            IOPairTO delayPair = new IOPairTO();

                            if (emplRealPairs.ContainsKey(pairTO.EmployeeID) && emplRealPairs[pairTO.EmployeeID].ContainsKey(pairTO.IOPairDate.Date))
                            {
                                foreach (IOPairTO pTO in emplRealPairs[pairTO.EmployeeID][pairTO.IOPairDate.Date])
                                {
                                    if (pTO.StartTime.TimeOfDay > pairInterval.StartTime.TimeOfDay && pTO.StartTime.TimeOfDay < pairInterval.EndTime.TimeOfDay)
                                    {
                                        delayPair = pTO;
                                        break;
                                    }
                                }
                            }

                            // if interval is third sfift begining and pair is not found, find first pair from third shift end
                            if (delayPair.IOPairID == -1 && pairInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                            {
                                List<EmployeeTimeScheduleTO> schedules = new List<EmployeeTimeScheduleTO>();
                                if (emplSchedules.ContainsKey(pairTO.EmployeeID))
                                    schedules = emplSchedules[pairTO.EmployeeID];

                                List<WorkTimeIntervalTO> nextIntervals = Common.Misc.getTimeSchemaInterval(pairTO.IOPairDate.Date.AddDays(1), schedules, schemas);

                                foreach (WorkTimeIntervalTO interval in nextIntervals)
                                {
                                    if (interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                        continue;

                                    if (emplRealPairs.ContainsKey(pairTO.EmployeeID) && emplRealPairs[pairTO.EmployeeID].ContainsKey(pairTO.IOPairDate.Date.AddDays(1)))
                                    {
                                        foreach (IOPairTO pTO in emplRealPairs[pairTO.EmployeeID][pairTO.IOPairDate.Date.AddDays(1)])
                                        {
                                            if (pTO.StartTime.TimeOfDay >= interval.StartTime.TimeOfDay && pTO.StartTime.TimeOfDay < interval.EndTime.TimeOfDay)
                                            {
                                                delayPair = pTO;
                                                break;
                                            }
                                        }
                                    }
                                }

                            }

                            if (delayPair.IOPairID != -1)
                            {
                                if (!emplDelays.ContainsKey(pairTO.EmployeeID))
                                    emplDelays.Add(pairTO.EmployeeID, new Dictionary<DateTime, int>());

                                if (!emplDelays[pairTO.EmployeeID].ContainsKey(pairTO.IOPairDate.Date))
                                    emplDelays[pairTO.EmployeeID].Add(pairTO.IOPairDate.Date, 0);

                                DateTime intervalStart = pairInterval.StartTime;
                                if (delayPair.IOPairDate.Date != pairTO.IOPairDate.Date)
                                {
                                    intervalStart = delayPair.IOPairDate.Date;
                                    emplDelays[pairTO.EmployeeID][pairTO.IOPairDate.Date] += (int)pairInterval.EndTime.TimeOfDay.Subtract(pairInterval.StartTime.TimeOfDay).TotalMinutes + 1;
                                }

                                emplDelays[pairTO.EmployeeID][pairTO.IOPairDate.Date] += (int)delayPair.StartTime.TimeOfDay.Subtract(intervalStart.TimeOfDay).TotalMinutes;
                            }
                        }

                        if (calculateLeave)
                        {
                            // get last pair from interval                            
                            IOPairTO leavePair = new IOPairTO();

                            if (emplRealPairs.ContainsKey(pairTO.EmployeeID) && emplRealPairs[pairTO.EmployeeID].ContainsKey(pairTO.IOPairDate.Date))
                            {
                                for (int i = emplRealPairs[pairTO.EmployeeID][pairTO.IOPairDate.Date].Count - 1; i >= 0; i--)
                                {
                                    if (emplRealPairs[pairTO.EmployeeID][pairTO.IOPairDate.Date][i].EndTime.TimeOfDay < pairInterval.EndTime.TimeOfDay
                                        && emplRealPairs[pairTO.EmployeeID][pairTO.IOPairDate.Date][i].EndTime.TimeOfDay > pairInterval.StartTime.TimeOfDay)
                                    {
                                        leavePair = emplRealPairs[pairTO.EmployeeID][pairTO.IOPairDate.Date][i];
                                        break;
                                    }
                                }
                            }

                            // if interval is third sfift end and pair is not found, find last pair from third shift begining
                            if (leavePair.IOPairID == -1 && pairInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                            {
                                List<EmployeeTimeScheduleTO> schedules = new List<EmployeeTimeScheduleTO>();
                                if (emplSchedules.ContainsKey(pairTO.EmployeeID))
                                    schedules = emplSchedules[pairTO.EmployeeID];

                                List<WorkTimeIntervalTO> prevIntervals = Common.Misc.getTimeSchemaInterval(pairTO.IOPairDate.Date.AddDays(-1), schedules, schemas);

                                foreach (WorkTimeIntervalTO interval in prevIntervals)
                                {
                                    if (interval.EndTime.TimeOfDay != new TimeSpan(23, 59, 0))
                                        continue;

                                    if (emplRealPairs.ContainsKey(pairTO.EmployeeID) && emplRealPairs[pairTO.EmployeeID].ContainsKey(pairTO.IOPairDate.Date.AddDays(-1)))
                                    {
                                        for (int i = emplRealPairs[pairTO.EmployeeID][pairTO.IOPairDate.Date.AddDays(-1)].Count - 1; i >= 0; i--)
                                        {
                                            if (emplRealPairs[pairTO.EmployeeID][pairTO.IOPairDate.Date.AddDays(-1)][i].EndTime.TimeOfDay <= interval.EndTime.TimeOfDay
                                                && emplRealPairs[pairTO.EmployeeID][pairTO.IOPairDate.Date.AddDays(-1)][i].EndTime.TimeOfDay > interval.StartTime.TimeOfDay)
                                            {
                                                leavePair = emplRealPairs[pairTO.EmployeeID][pairTO.IOPairDate.Date.AddDays(-1)][i];
                                                break;
                                            }
                                        }
                                    }
                                }

                            }

                            if (leavePair.IOPairID != -1)
                            {
                                if (!emplEarlyLeaves.ContainsKey(pairTO.EmployeeID))
                                    emplEarlyLeaves.Add(pairTO.EmployeeID, new Dictionary<DateTime, int>());

                                DateTime pairDate = pairTO.IOPairDate.Date;
                                if (pairInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                    pairDate = pairDate.AddDays(-1).Date;

                                if (!emplEarlyLeaves[pairTO.EmployeeID].ContainsKey(pairDate.Date))
                                    emplEarlyLeaves[pairTO.EmployeeID].Add(pairDate.Date, 0);

                                DateTime intervalEnd = pairInterval.EndTime;
                                if (leavePair.IOPairDate.Date != pairTO.IOPairDate.Date)
                                {
                                    intervalEnd = pairTO.IOPairDate.Date.AddMinutes(-1);
                                    emplEarlyLeaves[pairTO.EmployeeID][pairDate.Date] += (int)pairInterval.EndTime.TimeOfDay.Subtract(pairInterval.StartTime.TimeOfDay).TotalMinutes;
                                }

                                emplEarlyLeaves[pairTO.EmployeeID][pairDate.Date] += (int)intervalEnd.TimeOfDay.Subtract(leavePair.EndTime.TimeOfDay).TotalMinutes;

                                if (intervalEnd.TimeOfDay == new TimeSpan(23, 59, 0))
                                    emplEarlyLeaves[pairTO.EmployeeID][pairDate.Date]++;
                            }
                        }                        
                    }

                    generateDelayReport(emplDict, emplDelays, emplEarlyLeaves);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void generateDelayReport(Dictionary<int, EmployeeTO> emplDict, Dictionary<int, Dictionary<DateTime, int>> emplDelays, Dictionary<int, Dictionary<DateTime, int>> emplLeaves)
        {
            try
            {
                if (emplDelays.Count <= 0 && emplLeaves.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }

                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                object misValue = System.Reflection.Missing.Value;

                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;

                // insert header
                int colNum = 7;
                ws.Cells[1, 1] = rm.GetString("hdrEmplID", culture);
                ws.Cells[1, 2] = rm.GetString("hdrEmployeeName", culture);
                ws.Cells[1, 3] = rm.GetString("hdrWU", culture);
                ws.Cells[1, 4] = rm.GetString("hdrDay", culture);
                ws.Cells[1, 5] = rm.GetString("hdrDelay", culture);
                ws.Cells[1, 6] = rm.GetString("hdrDay", culture);
                ws.Cells[1, 7] = rm.GetString("hdrEarlierLeave", culture);
                
                setRowFontWeight(ws, 1, colNum, true);

                int rowNum = 2;
                foreach (int id in emplDict.Keys)
                {
                    if (!emplDelays.ContainsKey(id) && !emplLeaves.ContainsKey(id))
                        continue;
                    
                    ws.Cells[rowNum, 1] = id.ToString().Trim();
                    ws.Cells[rowNum, 2] = emplDict[id].FirstAndLastName.Trim();

                    if (wuDict.ContainsKey(emplDict[id].WorkingUnitID))
                        ws.Cells[rowNum, 3] = wuDict[emplDict[id].WorkingUnitID].Name.Trim();
                    else
                        ws.Cells[rowNum, 3] = "N/A";

                    setRowFontWeight(ws, rowNum, colNum, true);

                    rowNum++;

                    int totalDelay = 0;
                    int totalDelayDays = 0;
                    int totalLeave = 0;
                    int totalLeaveDays = 0;
                    for (DateTime date = dtpFrom.Value.Date; date.Date <= dtpTo.Value.Date; date = date.Date.AddDays(1))
                    {
                        int delay = 0;
                        int leave = 0;
                        if (emplDelays.ContainsKey(id) && emplDelays[id].ContainsKey(date.Date))
                        {
                            totalDelayDays++;
                            delay = emplDelays[id][date.Date];
                            totalDelay += delay;
                        }

                        if (emplLeaves.ContainsKey(id) && emplLeaves[id].ContainsKey(date.Date))
                        {
                            totalLeaveDays++;
                            leave = emplLeaves[id][date.Date];
                            totalLeave += leave;
                        }

                        if (delay == 0 && leave == 0)
                            continue;

                        ws.Cells[rowNum, 4] = date.ToString(Constants.dateFormat);
                        ws.Cells[rowNum, 5] = getTimeString(delay);
                        ws.Cells[rowNum, 6] = date.ToString(Constants.dateFormat);
                        ws.Cells[rowNum, 7] = getTimeString(leave);

                        rowNum++;
                    }

                    ws.Cells[rowNum, 3] = rm.GetString("hdrTotal", culture);                    
                    ws.Cells[rowNum, 4] = totalDelayDays;                    
                    ws.Cells[rowNum, 5] = getTimeString(totalDelay);
                    ws.Cells[rowNum, 6] = totalLeaveDays;
                    ws.Cells[rowNum, 7] = getTimeString(totalLeave);
                    setRowFontWeight(ws, rowNum, colNum, true);

                    rowNum++;
                }                

                ws.Columns.AutoFit();
                ws.Rows.AutoFit();

                string reportName = "DelayReport_" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss");

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
                log.writeLog(DateTime.Now + " PMCCumulativeReports.generateDelayReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void generateOvertimeReport(Dictionary<int, int> emplCategoryOvertime, Dictionary<int, int> passTypesOvertime, Dictionary<int, Dictionary<int, int>> emplCategoryOvertimePassType, int overtimeTotal)
        {
            try
            {
                if (overtimeTotal <= 0)
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }

                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                object misValue = System.Reflection.Missing.Value;

                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;

                // insert header
                int colNum = 1;
                Dictionary<int, int> positionIndexes = new Dictionary<int, int>();
                foreach (int ptID in passTypesOvertime.Keys)
                {
                    colNum++;

                    if (!positionIndexes.ContainsKey(colNum))
                        positionIndexes.Add(colNum, ptID);

                    if (ptDict.ContainsKey(ptID))
                        ws.Cells[1, colNum] = ptDict[ptID].DescriptionAndID.ToString().Trim();
                    else
                        ws.Cells[1, colNum] = "";
                }

                colNum++;
                ws.Cells[1, colNum] = rm.GetString("hdrTotal", culture);

                setRowFontWeight(ws, 1, colNum, true);

                int rowNum = 2;
                foreach (int category in emplCategoryOvertimePassType.Keys)
                {
                    if (emplTypes.ContainsKey(category))
                        ws.Cells[rowNum, 1] = emplTypes[category].Trim();
                    else
                        ws.Cells[rowNum, 1] = "";

                    setCellFontWeight(ws.Cells[rowNum, 1], true);

                    for (int col = 2; col < colNum; col++)
                    {
                        if (positionIndexes.ContainsKey(col) && emplCategoryOvertimePassType[category].ContainsKey(positionIndexes[col]))
                            ws.Cells[rowNum, col] = ((decimal)emplCategoryOvertimePassType[category][positionIndexes[col]] / 60).ToString(Constants.doubleFormat).Trim();
                        else
                            ws.Cells[rowNum, col] = "0.00";
                    }

                    // add total by categories
                    if (emplCategoryOvertime.ContainsKey(category))
                        ws.Cells[rowNum, colNum] = ((decimal)emplCategoryOvertime[category] / 60).ToString(Constants.doubleFormat).Trim();
                    else
                        ws.Cells[rowNum, colNum] = "0.00";

                    rowNum++;
                }

                // add sum by pass types
                ws.Cells[rowNum, 1] = rm.GetString("hdrTotal", culture);
                setCellFontWeight(ws.Cells[rowNum, 1], true);

                for (int col = 2; col < colNum; col++)
                {
                    if (positionIndexes.ContainsKey(col) && passTypesOvertime.ContainsKey(positionIndexes[col]))
                        ws.Cells[rowNum, col] = ((decimal)passTypesOvertime[positionIndexes[col]] / 60).ToString(Constants.doubleFormat).Trim();
                    else
                        ws.Cells[rowNum, col] = "0.00";
                }

                // add total                
                ws.Cells[rowNum, colNum] = ((decimal)overtimeTotal / 60).ToString(Constants.doubleFormat).Trim();

                ws.Columns.AutoFit();
                ws.Rows.AutoFit();

                string reportName = "OvertimeReport_" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss");

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
                log.writeLog(DateTime.Now + " PMCCumulativeReports.generateOvertimeReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void generateAbsenceReport(Dictionary<int, int> emplPositions, Dictionary<int, int> emplCategory, Dictionary<int, int> emplLines,
            Dictionary<int, Dictionary<int, int>> emplCategoryPositions, Dictionary<int, Dictionary<int, int>> emplCategoryLines, int absenceTotal)
        {
            try
            {
                if (absenceTotal <= 0)
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }                

                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                object misValue = System.Reflection.Missing.Value;

                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet wsLine = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;
                Microsoft.Office.Interop.Excel.Worksheet wsCategory = (Microsoft.Office.Interop.Excel.Worksheet)xla.Sheets.Add(misValue, misValue, misValue, misValue);

                wsCategory.Name = rm.GetString("sheetCategory", culture);
                wsLine.Name = rm.GetString("sheetLine", culture);

                // insert header
                int colNumCategory = 1;
                Dictionary<int, int> positionIndexes = new Dictionary<int, int>();
                foreach (int wuID in emplPositions.Keys)
                {
                    colNumCategory++;

                    if (!positionIndexes.ContainsKey(colNumCategory))
                        positionIndexes.Add(colNumCategory, wuID);

                    if (wuDict.ContainsKey(wuID))
                        wsCategory.Cells[1, colNumCategory] = wuDict[wuID].Name.ToString().Trim();
                    else
                        wsCategory.Cells[1, colNumCategory] = "";
                }

                colNumCategory++;
                wsCategory.Cells[1, colNumCategory] = rm.GetString("hdrTotal", culture);

                setRowFontWeight(wsCategory, 1, colNumCategory, true);

                int colNumLine = 1;
                Dictionary<int, int> lineIndexes = new Dictionary<int, int>();
                foreach (int line in emplLines.Keys)
                {
                    colNumLine++;

                    if (!lineIndexes.ContainsKey(colNumLine))
                        lineIndexes.Add(colNumLine, line);

                    if (ouDict.ContainsKey(line))
                        wsLine.Cells[1, colNumLine] = ouDict[line].Name.ToString().Trim();
                    else
                        wsLine.Cells[1, colNumLine] = "";
                }

                colNumLine++;
                wsLine.Cells[1, colNumLine] = rm.GetString("hdrTotal", culture);

                setRowFontWeight(wsLine, 1, colNumLine, true);

                int rowNumCategory = 2;
                foreach (int category in emplCategoryPositions.Keys)
                {
                    if (emplTypes.ContainsKey(category))
                        wsCategory.Cells[rowNumCategory, 1] = emplTypes[category].Trim();
                    else
                        wsCategory.Cells[rowNumCategory, 1] = "";
                    
                    setCellFontWeight(wsCategory.Cells[rowNumCategory, 1], true);

                    for (int col = 2; col < colNumCategory; col++)
                    {
                        if (positionIndexes.ContainsKey(col) && emplCategoryPositions[category].ContainsKey(positionIndexes[col]))
                            wsCategory.Cells[rowNumCategory, col] = emplCategoryPositions[category][positionIndexes[col]].ToString().Trim();
                        else
                            wsCategory.Cells[rowNumCategory, col] = "0";
                    }

                    // add total by categories
                    if (emplCategory.ContainsKey(category))
                        wsCategory.Cells[rowNumCategory, colNumCategory] = emplCategory[category].ToString().Trim();
                    else
                        wsCategory.Cells[rowNumCategory, colNumCategory] = "0";
                        
                    rowNumCategory++;
                }

                int rowNumLine = 2;
                foreach (int category in emplCategoryLines.Keys)
                {
                    if (emplTypes.ContainsKey(category))
                        wsLine.Cells[rowNumLine, 1] = emplTypes[category].Trim();
                    else
                        wsLine.Cells[rowNumLine, 1] = "";

                    setCellFontWeight(wsLine.Cells[rowNumLine, 1], true);

                    for (int col = 2; col < colNumLine; col++)
                    {
                        if (lineIndexes.ContainsKey(col) && emplCategoryLines[category].ContainsKey(lineIndexes[col]))
                            wsLine.Cells[rowNumLine, col] = emplCategoryLines[category][lineIndexes[col]].ToString().Trim();
                        else
                            wsLine.Cells[rowNumLine, col] = "0";
                    }

                    // add total by lines
                    if (emplCategory.ContainsKey(category))
                        wsLine.Cells[rowNumLine, colNumLine] = emplCategory[category].ToString().Trim();
                    else
                        wsLine.Cells[rowNumLine, colNumLine] = "0";

                    rowNumLine++;
                }

                // add sum by positions
                wsCategory.Cells[rowNumCategory, 1] = rm.GetString("hdrTotal", culture);
                setCellFontWeight(wsCategory.Cells[rowNumCategory, 1], true);

                for (int col = 2; col < colNumCategory; col++)
                {
                    if (positionIndexes.ContainsKey(col) && emplPositions.ContainsKey(positionIndexes[col]))
                        wsCategory.Cells[rowNumCategory, col] = emplPositions[positionIndexes[col]].ToString().Trim();
                    else
                        wsCategory.Cells[rowNumCategory, col] = "0";
                }

                // add sum by lines
                wsLine.Cells[rowNumLine, 1] = rm.GetString("hdrTotal", culture);
                setCellFontWeight(wsLine.Cells[rowNumLine, 1], true);

                for (int col = 2; col < colNumLine; col++)
                {
                    if (lineIndexes.ContainsKey(col) && emplLines.ContainsKey(lineIndexes[col]))
                        wsLine.Cells[rowNumLine, col] = emplLines[lineIndexes[col]].ToString().Trim();
                    else
                        wsLine.Cells[rowNumLine, col] = "0";
                }

                // add total                
                wsCategory.Cells[rowNumCategory, colNumCategory] = absenceTotal.ToString().Trim();
                wsLine.Cells[rowNumLine, colNumLine] = absenceTotal.ToString().Trim();

                wsCategory.Columns.AutoFit();
                wsCategory.Rows.AutoFit();

                wsLine.Columns.AutoFit();
                wsLine.Rows.AutoFit();

                string reportName = "AbsenceReport_" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss");

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

                releaseObject(wsCategory);
                releaseObject(wsLine);
                releaseObject(wb);
                releaseObject(xla);

                System.Threading.Thread.CurrentThread.CurrentCulture = Oldci;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PMCCumulativeReports.generateAbsenceReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void generateAbsenceByTypeReport(Dictionary<int, int> emplPositionsAbsence, Dictionary<int, int> emplCategoryAbsence, Dictionary<int, int> emplLinesAbsence, 
            Dictionary<int, int> passTypesAbsence, Dictionary<int, Dictionary<int, int>> emplCategoryAbsencePassType, Dictionary<int, Dictionary<int, int>> emplPositionAbsencePassType, 
            Dictionary<int, Dictionary<int, int>> emplLinesAbsencePassType, int absenceTotal)
        {
            try
            {
                if (absenceTotal <= 0)
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }

                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                object misValue = System.Reflection.Missing.Value;

                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet wsCategory = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;
                Microsoft.Office.Interop.Excel.Worksheet wsLine = (Microsoft.Office.Interop.Excel.Worksheet)xla.Sheets.Add(misValue, misValue, misValue, misValue);
                Microsoft.Office.Interop.Excel.Worksheet wsPosition = (Microsoft.Office.Interop.Excel.Worksheet)xla.Sheets.Add(misValue, misValue, misValue, misValue);


                wsCategory.Name = rm.GetString("sheetCategory", culture);
                wsLine.Name = rm.GetString("sheetLine", culture);
                wsPosition.Name = rm.GetString("sheetPosition", culture);
                
                // insert header
                int colNum = 1;
                Dictionary<int, int> positionIndexes = new Dictionary<int, int>();
                foreach (int ptID in passTypesAbsence.Keys)
                {
                    colNum++;

                    if (!positionIndexes.ContainsKey(colNum))
                        positionIndexes.Add(colNum, ptID);

                    if (ptDict.ContainsKey(ptID))
                    {
                        wsCategory.Cells[1, colNum] = ptDict[ptID].DescriptionAndID.ToString().Trim();
                        wsLine.Cells[1, colNum] = ptDict[ptID].DescriptionAndID.ToString().Trim();
                        wsPosition.Cells[1, colNum] = ptDict[ptID].DescriptionAndID.ToString().Trim();
                    }
                    else
                    {
                        wsCategory.Cells[1, colNum] = "";
                        wsLine.Cells[1, colNum] = "";
                        wsPosition.Cells[1, colNum] = "";
                    }
                }

                colNum++;
                wsCategory.Cells[1, colNum] = rm.GetString("hdrTotal", culture);
                wsLine.Cells[1, colNum] = rm.GetString("hdrTotal", culture);
                wsPosition.Cells[1, colNum] = rm.GetString("hdrTotal", culture);

                setRowFontWeight(wsCategory, 1, colNum, true);
                setRowFontWeight(wsLine, 1, colNum, true);
                setRowFontWeight(wsPosition, 1, colNum, true);

                // create categories sheet
                int rowNumCategory = 2;
                foreach (int category in emplCategoryAbsencePassType.Keys)
                {
                    if (emplTypes.ContainsKey(category))
                        wsCategory.Cells[rowNumCategory, 1] = emplTypes[category].Trim();
                    else
                        wsCategory.Cells[rowNumCategory, 1] = "";

                    setCellFontWeight(wsCategory.Cells[rowNumCategory, 1], true);

                    for (int col = 2; col < colNum; col++)
                    {
                        if (positionIndexes.ContainsKey(col) && emplCategoryAbsencePassType[category].ContainsKey(positionIndexes[col]))
                            wsCategory.Cells[rowNumCategory, col] = emplCategoryAbsencePassType[category][positionIndexes[col]].ToString().Trim();
                        else
                            wsCategory.Cells[rowNumCategory, col] = "0";
                    }

                    // add total by categories
                    if (emplCategoryAbsence.ContainsKey(category))
                        wsCategory.Cells[rowNumCategory, colNum] = emplCategoryAbsence[category].ToString().Trim();
                    else
                        wsCategory.Cells[rowNumCategory, colNum] = "0";

                    rowNumCategory++;
                }

                // create lines sheet
                int rowNumLine = 2;
                foreach (int line in emplLinesAbsencePassType.Keys)
                {
                    if (ouDict.ContainsKey(line))
                        wsLine.Cells[rowNumLine, 1] = ouDict[line].Name.Trim();
                    else
                        wsLine.Cells[rowNumLine, 1] = "";

                    setCellFontWeight(wsLine.Cells[rowNumLine, 1], true);

                    for (int col = 2; col < colNum; col++)
                    {
                        if (positionIndexes.ContainsKey(col) && emplLinesAbsencePassType[line].ContainsKey(positionIndexes[col]))
                            wsLine.Cells[rowNumLine, col] = emplLinesAbsencePassType[line][positionIndexes[col]].ToString().Trim();
                        else
                            wsLine.Cells[rowNumLine, col] = "0";
                    }

                    // add total by lines
                    if (emplLinesAbsence.ContainsKey(line))
                        wsLine.Cells[rowNumLine, colNum] = emplLinesAbsence[line].ToString().Trim();
                    else
                        wsLine.Cells[rowNumLine, colNum] = "0";

                    rowNumLine++;
                }

                // create positions sheet
                int rowNumPosition = 2;
                foreach (int wuID in emplPositionAbsencePassType.Keys)
                {
                    if (wuDict.ContainsKey(wuID))
                        wsPosition.Cells[rowNumPosition, 1] = wuDict[wuID].Name.Trim();
                    else
                        wsPosition.Cells[rowNumPosition, 1] = "";

                    setCellFontWeight(wsPosition.Cells[rowNumPosition, 1], true);

                    for (int col = 2; col < colNum; col++)
                    {
                        if (positionIndexes.ContainsKey(col) && emplPositionAbsencePassType[wuID].ContainsKey(positionIndexes[col]))
                            wsPosition.Cells[rowNumPosition, col] = emplPositionAbsencePassType[wuID][positionIndexes[col]].ToString().Trim();
                        else
                            wsPosition.Cells[rowNumPosition, col] = "0";
                    }

                    // add total by positions
                    if (emplPositionsAbsence.ContainsKey(wuID))
                        wsPosition.Cells[rowNumPosition, colNum] = emplPositionsAbsence[wuID].ToString().Trim();
                    else
                        wsPosition.Cells[rowNumPosition, colNum] = "0";

                    rowNumPosition++;
                }

                // add sum by pass types
                wsCategory.Cells[rowNumCategory, 1] = rm.GetString("hdrTotal", culture);
                wsLine.Cells[rowNumLine, 1] = rm.GetString("hdrTotal", culture);
                wsPosition.Cells[rowNumPosition, 1] = rm.GetString("hdrTotal", culture);
                
                setCellFontWeight(wsCategory.Cells[rowNumCategory, 1], true);
                setCellFontWeight(wsLine.Cells[rowNumLine, 1], true);
                setCellFontWeight(wsPosition.Cells[rowNumPosition, 1], true);

                for (int col = 2; col < colNum; col++)
                {
                    if (positionIndexes.ContainsKey(col) && passTypesAbsence.ContainsKey(positionIndexes[col]))
                    {
                        wsCategory.Cells[rowNumCategory, col] = passTypesAbsence[positionIndexes[col]].ToString().Trim();
                        wsLine.Cells[rowNumLine, col] = passTypesAbsence[positionIndexes[col]].ToString().Trim();
                        wsPosition.Cells[rowNumPosition, col] = passTypesAbsence[positionIndexes[col]].ToString().Trim();
                    }
                    else
                    {
                        wsCategory.Cells[rowNumCategory, col] = "0";
                        wsLine.Cells[rowNumLine, col] = "0";
                        wsPosition.Cells[rowNumPosition, col] = "0";
                    }
                }

                // add total                
                wsCategory.Cells[rowNumCategory, colNum] = absenceTotal.ToString().Trim();
                wsLine.Cells[rowNumLine, colNum] = absenceTotal.ToString().Trim();
                wsPosition.Cells[rowNumPosition, colNum] = absenceTotal.ToString().Trim();

                wsCategory.Columns.AutoFit();
                wsCategory.Rows.AutoFit();
                wsLine.Columns.AutoFit();
                wsLine.Rows.AutoFit();
                wsPosition.Columns.AutoFit();
                wsPosition.Rows.AutoFit();

                string reportName = "AbsenceByTypeReport_" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss");

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

                releaseObject(wsCategory);
                releaseObject(wsLine);
                releaseObject(wsPosition);
                releaseObject(wb);
                releaseObject(xla);

                System.Threading.Thread.CurrentThread.CurrentCulture = Oldci;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PMCCumulativeReports.generateAbsenceByTypeReport(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.setRowFontWeight(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.setCellFontWeight(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.releaseObject(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        private string getTimeString(int minutes)
        {
            try
            {
                return (minutes / 60).ToString().Trim() + ":" + (minutes % 60).ToString().Trim();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCCumulativeReports.getTimeString(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}