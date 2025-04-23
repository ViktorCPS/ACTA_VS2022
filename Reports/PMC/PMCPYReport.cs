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
    public partial class PMCPYReport : Form
    {
        private const int emplIDIndex = 0;
        private const int emplNameIndex = 1;

        private const string delimiter = ",";
        private const string unconfirmedDataDelimiter = "\t";        

        private enum ReportTypeCategories
        {
            REG_WORK = 0,
            OVERTIME_PAID = 1,
            ANNUAL_LEAVE = 2,
            HOLIDAY = 3,
            WORK_HOLIDAY = 4,
            PAID_LEAVE = 5,
            SICK_LEAVE_100_PERCENT = 6,
            SICK_LEAVE_65_PERCENT = 7,
            SICK_LEAVE_OVER_30_DAYS = 8,
            SICK_LEAVE_PREGNANCY = 9,
            UNPAID_LEAVE = 10,
            NIGHT_WORK = 11,
            STOP_WORKING = 12,
            BANK_HOURS = 13,
            OVERTIME_PREV_MONTH_PAID = 14,
            OVERTIME_NOT_PAID = 15,
            NOT_PAID = 16,
            UNJUSTIFIED = 17,
            PAID_LEAVE_60 = 18
        }

        private enum ReportTypeCodes
        {
            RW = 0,
            UNPAID = 1,
            B65 = 2,
            B100 = 3,
            BP = 4,
            GO = 5,
            DP = 6,
            PO = 7,
            NO = 8,
            TB = 9,
            PO60 = 10
        }

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
        Dictionary<int, string> emplTypes = new Dictionary<int, string>();
        List<int> paidLeaves = new List<int>();
        List<int> PMCCategories = new List<int>();
        List<int> AgencyCategories = new List<int>();
        List<int> InternationalStaffCategories = new List<int>();

        public PMCPYReport()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(PMCPYReport).Assembly);

                logInUser = NotificationController.GetLogInUser();

                setLanguage();

                rbWU.Checked = true;
                chbHierarhiclyWU.Checked = true;
                chbHierachyOU.Checked = true;
                
                dtpMonth.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
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
                    case PMCPYReport.emplIDIndex:
                        {
                            int id1 = -1;
                            int id2 = -1;

                            int.TryParse(sub1.Text, out id1);
                            int.TryParse(sub2.Text, out id2);

                            return CaseInsensitiveComparer.Default.Compare(id1, id2);
                        }
                    case PMCPYReport.emplNameIndex:
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
                this.Text = rm.GetString("PMCPYReport", culture);

                // tab's text
                tabReport.Text = rm.GetString("tabReport", culture);
                tabCounters.Text = rm.GetString("tabCounters", culture);

                //label's text
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblMonth.Text = rm.GetString("lblMonth", culture);
                lblCalcID.Text = rm.GetString("lblCalcID", culture);

                //button's text
                btnClose.Text = rm.GetString("btnClose", culture);
                btnGenerate.Text = rm.GetString("btnGeneratePY", culture);
                btnGenerateAgency.Text = rm.GetString("btnGenerateAgency", culture);
                btnRecalculate.Text = rm.GetString("btnRecalculate", culture);

                //group box text                
                gbUnitFilter.Text = rm.GetString("gbUnitFilter", culture);
                gbCategory.Text = rm.GetString("gbCategory", culture);
                
                // check box text
                chbHierarhiclyWU.Text = rm.GetString("chbHierarhicly", culture);
                chbHierachyOU.Text = rm.GetString("chbHierarhicly", culture);
                
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PMCPYReport_Load(object sender, EventArgs e)
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

                PassTypesConfirmation conf = new PassTypesConfirmation();
                List<PassTypesConfirmationTO> confList = conf.Search();

                List<int> sickLeaveNotConfirmed = new Common.Rule().SearchRulesExact(Constants.RuleCompanySickLeaveNCF);

                foreach (PassTypesConfirmationTO pt in confList)
                {
                    if (!Constants.PMCUnpaidLeavesTypes().Contains(pt.ConfirmationPassTypeID)
                        && !Constants.PMCPaidLeave60Types().Contains(pt.ConfirmationPassTypeID)
                        && !sickLeaveNotConfirmed.Contains(pt.PassTypeID) && pt.ConfirmationPassTypeID != Constants.unpaidLeave)
                        paidLeaves.Add(pt.ConfirmationPassTypeID);
                }

                PMCCategories.Add((int)Constants.EmployeeTypesPMC.MOD_PMC);
                PMCCategories.Add((int)Constants.EmployeeTypesPMC.MOI_PMC);
                PMCCategories.Add((int)Constants.EmployeeTypesPMC.PROFESSIONALS_PMC);
                PMCCategories.Add((int)Constants.EmployeeTypesPMC.VILLANOVA_LC);

                AgencyCategories.Add((int)Constants.EmployeeTypesPMC.MOD_AGENCY);
                AgencyCategories.Add((int)Constants.EmployeeTypesPMC.MOI_AGENCY);
                AgencyCategories.Add((int)Constants.EmployeeTypesPMC.PROFESSIONALS_AGENCY);
                AgencyCategories.Add((int)Constants.EmployeeTypesPMC.VILLANOVA_INTERIM);
                
                InternationalStaffCategories.Add((int)Constants.EmployeeTypesPMC.INTERNATIONAL_STAFF);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.PMCPYReport_Load(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.populateWU(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.populateOU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateEmployees()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<EmployeeTO> employeeList = new List<EmployeeTO>();
                                
                DateTime from = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
                DateTime to = from.AddMonths(1).AddDays(-1).Date;

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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.populateEmployees(): " + ex.Message + "\n");
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
                chbHierarhiclyWU.Enabled = rbWU.Checked;
                chbHierachyOU.Enabled = !rbWU.Checked;

                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.rbWU_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.rbOU_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " PMCPYReport.tbEmployee_TextChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " PMCPYReport.lvEmployees_ColumnClick(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " PMCPYReport.populateCategories(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isWorkOnHoliday(Dictionary<string, List<DateTime>> personalHolidayDays, List<DateTime> nationalHolidaysDays, List<DateTime> nationalHolidaysDaysSundays,
            List<HolidaysExtendedTO> nationalTransferableHolidays, List<DateTime> paidHolidaysDays, DateTime date, WorkTimeSchemaTO schema, EmployeeAsco4TO asco, EmployeeTO empl)
        {
            try
            {
                bool isHoliday = false;

                if (InternationalStaffCategories.Contains(empl.EmployeeTypeID))
                    return isHoliday;

                // personal holidays are not work on holiday for everyone
                if ((personalHolidayDays.ContainsKey(asco.NVarcharValue1) && personalHolidayDays[asco.NVarcharValue1].Contains(date.Date))
                    || (asco.DatetimeValue1.Day == date.Day && asco.DatetimeValue1.Month == date.Month))
                    isHoliday = false;
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
                log.writeLog(DateTime.Now + " PMCPYReport.isWorkOnHoliday(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " PMCPYReport.isTurnus(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private int nightWorkDuration(Dictionary<string, RuleTO> emplRules, DateTime date, DateTime start, DateTime end, EmployeeTO empl)
        {
            try
            {
                int nightWork = 0;

                if (InternationalStaffCategories.Contains(empl.EmployeeTypeID))
                    return nightWork;

                DateTime nightStart = new DateTime();
                DateTime nightEnd = new DateTime();

                if (emplRules.ContainsKey(Constants.RuleNightWork))
                {
                    nightStart = emplRules[Constants.RuleNightWork].RuleDateTime1;
                    nightEnd = emplRules[Constants.RuleNightWork].RuleDateTime2;
                }

                if (start.TimeOfDay < nightEnd.TimeOfDay)
                {
                    DateTime endNightWork = new DateTime();

                    if (end.TimeOfDay <= nightEnd.TimeOfDay)
                        endNightWork = end;
                    else
                        endNightWork = new DateTime(date.Year, date.Month, date.Day, nightEnd.Hour,
                            nightEnd.Minute, nightEnd.Second);

                    nightWork += (int)endNightWork.TimeOfDay.Subtract(start.TimeOfDay).TotalMinutes;

                    if (endNightWork.Hour == 23 && endNightWork.Minute == 59)
                        nightWork++;
                }

                if (end.TimeOfDay > nightStart.TimeOfDay)
                {
                    DateTime startNightWork = new DateTime();

                    if (start.TimeOfDay >= nightStart.TimeOfDay)
                        startNightWork = start;
                    else
                        startNightWork = new DateTime(date.Year, date.Month, date.Day, nightStart.Hour,
                            nightStart.Minute, nightStart.Second);

                    nightWork += (int)end.TimeOfDay.Subtract(startNightWork.TimeOfDay).TotalMinutes;

                    if (end.Hour == 23 && end.Minute == 59)
                        nightWork++;
                }

                return nightWork;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PMCPYReport.nightWorkDuration(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isPresence(int ptID, Dictionary<string, RuleTO> emplRules)
        {
            try
            {
                bool presencePair = false;

                List<string> presenceTypes = Constants.PMCEffectiveWorkWageTypes();

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
                log.writeLog(DateTime.Now + " PMCPYReport.isPresence(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isMealPresence(int ptID, Dictionary<string, RuleTO> emplRules)
        {
            try
            {
                bool presencePair = false;

                List<string> presenceTypes = Constants.PMCMealEffectiveWorkWageTypes();

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
                log.writeLog(DateTime.Now + " PMCPYReport.isMealPresence(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void dtpMonth_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.dtpMonth_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.chbHierarhiclyWU_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.chbHierachyOU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private int getReportCategory(int ptID, Dictionary<string, RuleTO> emplRules, EmployeeTO empl)
        {
            try
            {
                int category = -1;

                if ((emplRules.ContainsKey(Constants.RuleCompanyRegularWork) && ptID == emplRules[Constants.RuleCompanyRegularWork].RuleValue)
                    || (emplRules.ContainsKey(Constants.RuleCompanyStopWorking) && ptID == emplRules[Constants.RuleCompanyStopWorking].RuleValue)
                    || (emplRules.ContainsKey(Constants.RuleCompanyOfficialTrip) && ptID == emplRules[Constants.RuleCompanyOfficialTrip].RuleValue)
                    || (emplRules.ContainsKey(Constants.RuleCompanyOfficialOut) && ptID == emplRules[Constants.RuleCompanyOfficialOut].RuleValue)
                    || (emplRules.ContainsKey(Constants.RuleCompanyTrening) && ptID == emplRules[Constants.RuleCompanyTrening].RuleValue))
                    category = (int)ReportTypeCategories.REG_WORK;
                else if (AgencyCategories.Contains(empl.EmployeeTypeID))
                    category = (int)ReportTypeCategories.UNPAID_LEAVE;
                else if (emplRules.ContainsKey(Constants.RuleCompanyAnnualLeave) && ptID == emplRules[Constants.RuleCompanyAnnualLeave].RuleValue)
                    category = (int)ReportTypeCategories.ANNUAL_LEAVE;
                else if (emplRules.ContainsKey(Constants.RuleHolidayPassType) && ptID == emplRules[Constants.RuleHolidayPassType].RuleValue)
                    category = (int)ReportTypeCategories.HOLIDAY;
                else if ((emplRules.ContainsKey(Constants.RulePersonalHolidayPassType) && ptID == emplRules[Constants.RulePersonalHolidayPassType].RuleValue)
                    || paidLeaves.Contains(ptID))
                    category = (int)ReportTypeCategories.PAID_LEAVE;
                else if (Constants.PMCSickLeaves100Types().Contains(ptID))
                    category = (int)ReportTypeCategories.SICK_LEAVE_100_PERCENT;
                else if (Constants.PMCSickLeaves65Types().Contains(ptID))
                    category = (int)ReportTypeCategories.SICK_LEAVE_65_PERCENT;
                else if (Constants.PMCSickLeaves30DaysTypes().Contains(ptID))
                    category = (int)ReportTypeCategories.SICK_LEAVE_OVER_30_DAYS;
                else if (Constants.PMCSickLeavesPregnancyTypes().Contains(ptID))
                    category = (int)ReportTypeCategories.SICK_LEAVE_PREGNANCY;
                else if (Constants.PMCPaidLeave60Types().Contains(ptID))
                    category = (int)ReportTypeCategories.PAID_LEAVE_60;
                else if (ptID == Constants.absence || Constants.PMCUnpaidLeavesTypes().Contains(ptID)
                    || (emplRules.ContainsKey(Constants.RuleCompanyDelay) && ptID == emplRules[Constants.RuleCompanyDelay].RuleValue))
                    category = (int)ReportTypeCategories.UNPAID_LEAVE;
                
                return category;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.getReportCategory(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private string getReportCode(int category, int ptID)
        {
            try
            {
                string code = "";

                if (category == (int)ReportTypeCategories.REG_WORK || category == (int)ReportTypeCategories.UNPAID_LEAVE || category == (int)ReportTypeCategories.WORK_HOLIDAY)
                    code = ReportTypeCodes.RW.ToString();
                else if (category == (int)ReportTypeCategories.SICK_LEAVE_65_PERCENT)
                    code = ReportTypeCodes.B65.ToString();
                else if (category == (int)ReportTypeCategories.SICK_LEAVE_100_PERCENT)
                {
                    if (ptID == Constants.PMCWorkInjurySickLeave || ptID == Constants.PMCWorkInjurySickLeaveCont)
                        code = ReportTypeCodes.BP.ToString();
                    else
                        code = ReportTypeCodes.B100.ToString();
                }
                else if (category == (int)ReportTypeCategories.SICK_LEAVE_PREGNANCY)
                    code = ReportTypeCodes.TB.ToString();
                else if (category == (int)ReportTypeCategories.ANNUAL_LEAVE)
                    code = ReportTypeCodes.GO.ToString();
                else if (category == (int)ReportTypeCategories.HOLIDAY)
                    code = ReportTypeCodes.DP.ToString();
                else if (category == (int)ReportTypeCategories.PAID_LEAVE)
                    code = ReportTypeCodes.PO.ToString();
                else if (category == (int)ReportTypeCategories.PAID_LEAVE_60)
                    code = ReportTypeCodes.PO60.ToString();
                else if (ptID == Constants.unpaidLeave)
                    code = ReportTypeCodes.NO.ToString();

                return code;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.getReportCode(): " + ex.Message + "\n");
                throw ex;
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

                this.Cursor = Cursors.WaitCursor;

                DateTime from = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
                DateTime to = from.AddMonths(1).AddDays(-1).Date;

                // get employees
                string emplIDs = "";
                Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
                Dictionary<int, Dictionary<int, EmployeeTO>> wuEmplDictTemp = new Dictionary<int, Dictionary<int, EmployeeTO>>();
                Dictionary<int, Dictionary<int, EmployeeTO>> wuEmplDict = new Dictionary<int, Dictionary<int, EmployeeTO>>();

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

                if (lvEmployees.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvEmployees.SelectedItems)
                    {
                        if (!selectedTypes.Contains(((EmployeeTO)item.Tag).EmployeeTypeID))
                            continue;

                        emplIDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";

                        if (!emplDict.ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                            emplDict.Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);

                        if (!AgencyCategories.Contains(((EmployeeTO)item.Tag).EmployeeTypeID))
                        {
                            if (!wuEmplDictTemp.ContainsKey(((EmployeeTO)item.Tag).WorkingUnitID))
                                wuEmplDictTemp.Add(((EmployeeTO)item.Tag).WorkingUnitID, new Dictionary<int, EmployeeTO>());

                            if (!wuEmplDictTemp[((EmployeeTO)item.Tag).WorkingUnitID].ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                                wuEmplDictTemp[((EmployeeTO)item.Tag).WorkingUnitID].Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);
                        }
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        if (!selectedTypes.Contains(((EmployeeTO)item.Tag).EmployeeTypeID))
                            continue;

                        emplIDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";

                        if (!emplDict.ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                            emplDict.Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);

                        if (!AgencyCategories.Contains(((EmployeeTO)item.Tag).EmployeeTypeID))
                        {
                            if (!wuEmplDictTemp.ContainsKey(((EmployeeTO)item.Tag).WorkingUnitID))
                                wuEmplDictTemp.Add(((EmployeeTO)item.Tag).WorkingUnitID, new Dictionary<int, EmployeeTO>());

                            if (!wuEmplDictTemp[((EmployeeTO)item.Tag).WorkingUnitID].ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                                wuEmplDictTemp[((EmployeeTO)item.Tag).WorkingUnitID].Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);
                        }
                    }
                }

                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                else
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }

                // sort data for report by working unit name
                foreach (int id in wuDict.Keys)
                {
                    if (wuEmplDictTemp.ContainsKey(id) && !wuEmplDict.ContainsKey(id))
                        wuEmplDict.Add(id, wuEmplDictTemp[id]);
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

                // get pairs for one more day becouse of third shifts
                List<DateTime> dateList = new List<DateTime>();
                DateTime currentDate = from.Date;
                while (currentDate.Date <= to.AddDays(1).Date)
                {
                    dateList.Add(currentDate.Date);
                    currentDate = currentDate.AddDays(1);
                }
                List<IOPairProcessedTO> allPairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, dateList, "");

                // get all pairs after period for finding SW balance
                List<int> swList = new Common.Rule().SearchRulesExact(Constants.RuleCompanyStopWorking);
                string swIDs = "";
                foreach (int swID in swList)
                {
                    swIDs += swID.ToString().Trim() + ",";
                }
                if (swIDs.Length > 0)
                    swIDs = swIDs.Substring(0, swIDs.Length - 1);
                List<IOPairProcessedTO> afterSWPairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, to.AddDays(2).Date, new DateTime(), swIDs);

                Dictionary<int, int> emplSWAfter = new Dictionary<int, int>();
                foreach (IOPairProcessedTO pair in afterSWPairs)
                {
                    int duration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;
                    if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                        duration++;

                    if (!emplSWAfter.ContainsKey(pair.EmployeeID))
                        emplSWAfter.Add(pair.EmployeeID, 0);

                    emplSWAfter[pair.EmployeeID] += duration;
                }

                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                
                // analitical dictionary
                Dictionary<int, Dictionary<int, int>> emplReportCategoryDuration = new Dictionary<int, Dictionary<int, int>>();

                Dictionary<int, Dictionary<string, RuleTO>> emplRules = new Dictionary<int, Dictionary<string, RuleTO>>();
                Dictionary<int, List<DateTime>> emplHolidayPaidDays = new Dictionary<int, List<DateTime>>();

                // create list of pairs foreach employee and each day
                foreach (IOPairProcessedTO pair in allPairs)
                {
                    if (!emplDayPairs.ContainsKey(pair.EmployeeID))
                        emplDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                    if (!emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        emplDayPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());
                    emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);

                    if (!emplRules.ContainsKey(pair.EmployeeID))
                    {
                        if (emplDict.ContainsKey(pair.EmployeeID))
                        {
                            int company = Common.Misc.getRootWorkingUnit(emplDict[pair.EmployeeID].WorkingUnitID, wuDict);

                            if (rules.ContainsKey(company) && rules[company].ContainsKey(emplDict[pair.EmployeeID].EmployeeTypeID))
                                emplRules.Add(pair.EmployeeID, rules[company][emplDict[pair.EmployeeID].EmployeeTypeID]);
                        }
                    }
                }
                                
                Dictionary<int, int> emplMeals = new Dictionary<int, int>();
                Dictionary<int, Dictionary<DateTime, int>> emplPresenceByDay = new Dictionary<int, Dictionary<DateTime, int>>();

                // overtime dictionaries
                Dictionary<int, Dictionary<DateTime, int>> emplDayOvertime = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> emplWeekOvertime = new Dictionary<int, Dictionary<DateTime, int>>();

                // night work dictionary
                Dictionary<int, Dictionary<DateTime, int>> emplNightWorkDict = new Dictionary<int, Dictionary<DateTime, int>>();
                
                // get counters
                Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounters = new EmployeeCounterValue().SearchValues(emplIDs);

                // move hours from bank hours counter to report if there are hours for moving
                foreach (int id in emplCounters.Keys)
                {
                    if (emplCounters[id].ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter) && emplCounters[id][(int)Constants.EmplCounterTypes.BankHoursCounter].Value > 0)
                    {
                        if (!emplReportCategoryDuration.ContainsKey(id))
                            emplReportCategoryDuration.Add(id, new Dictionary<int, int>());

                        if (!emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID))
                            emplReportCategoryDuration[id].Add((int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID, 0);
                        
                        emplReportCategoryDuration[id][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID] += emplCounters[id][(int)Constants.EmplCounterTypes.BankHoursCounter].Value;                        
                    }
                }

                Dictionary<int, List<DateTime>> unconfirmed = new Dictionary<int, List<DateTime>>();
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> overtimePairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                
                // data for monthly report
                Dictionary<int, Dictionary<DateTime, Dictionary<string, int>>> emplHrs = new Dictionary<int, Dictionary<DateTime, Dictionary<string, int>>>();                
                Dictionary<int, Dictionary<DateTime, int>> emplOvertimeHrs = new Dictionary<int, Dictionary<DateTime, int>>();                
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
                        // if pair is not confirmed, do not generate report
                        if (pair.ConfirmationFlag == Constants.yesInt)
                        {
                            if (!unconfirmed.ContainsKey(pair.EmployeeID))
                                unconfirmed.Add(pair.EmployeeID, new List<DateTime>());

                            if (!unconfirmed[pair.EmployeeID].Contains(pairDate.Date))
                                unconfirmed[pair.EmployeeID].Add(pairDate.Date);
                        }

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
                        if (emplDaySchemas.ContainsKey(pair.EmployeeID) && emplDaySchemas[pair.EmployeeID].ContainsKey(pairDate.Date))
                            schema = emplDaySchemas[pair.EmployeeID][pairDate.Date];

                        List<DateTime> paidDays = new List<DateTime>();
                        if (emplHolidayPaidDays.ContainsKey(pair.EmployeeID))
                            paidDays = emplHolidayPaidDays[pair.EmployeeID];
                        
                        // calculate code durations and add it to dictionary
                        int pairDuration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                        if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                            pairDuration++;
                        
                        if (ptDict.ContainsKey(pair.PassTypeID) && ptDict[pair.PassTypeID].IsPass == Constants.overtimePassType)
                        {
                            // add to overtime dictionary to process later
                            if (pair.PassTypeID == Constants.overtimeUnjustified)
                            {
                                if (!overtimePairs.ContainsKey(pair.EmployeeID))
                                    overtimePairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                                if (!overtimePairs[pair.EmployeeID].ContainsKey(pairDate.Date))
                                    overtimePairs[pair.EmployeeID].Add(pairDate.Date, new List<IOPairProcessedTO>());

                                overtimePairs[pair.EmployeeID][pairDate.Date].Add(pair);
                            }
                        }
                        else
                        {
                            int category = -1;
                            if (isPresence(pair.PassTypeID, rulesForEmpl) && isWorkOnHoliday(personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, nationalTransferableHolidays, paidDays, pairDate.Date, schema, asco, empl))
                                category = (int)ReportTypeCategories.WORK_HOLIDAY;
                            else
                            {
                                // personal holiday which is not whole day absence is unpaid leave
                                bool unpaidLeave = false;
                                if (rulesForEmpl.ContainsKey(Constants.RulePersonalHolidayPassType) && rulesForEmpl[Constants.RulePersonalHolidayPassType].RuleValue == pair.PassTypeID)
                                {
                                    WorkTimeIntervalTO pairInterval = Common.Misc.getPairInterval(pair, dayPairs, sch, dayIntervals, ptDict);

                                    int pairIntervalDuration = (int)pairInterval.EndTime.TimeOfDay.Subtract(pairInterval.StartTime.TimeOfDay).TotalMinutes;
                                    if (pairInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                        pairIntervalDuration++;

                                    if (pairDuration != pairIntervalDuration)
                                        unpaidLeave = true;
                                    else if (pairDuration != Constants.dayDurationStandardShift * 60)
                                    {
                                        if (pairInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                        {
                                            List<IOPairProcessedTO> prevPairs = new List<IOPairProcessedTO>();
                                            if (emplDayPairs.ContainsKey(pair.EmployeeID) && emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date.AddDays(-1)))
                                                prevPairs = emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date.AddDays(-1)];

                                            bool foundHolidayPair = false;
                                            // find personal holiday from previous pairs
                                            foreach (IOPairProcessedTO prevPair in prevPairs)
                                            {
                                                if (rulesForEmpl[Constants.RulePersonalHolidayPassType].RuleValue == prevPair.PassTypeID)
                                                {
                                                    foundHolidayPair = true;

                                                    // check if it ends in midnight and duration is 8 - pairDuration
                                                    if (prevPair.EndTime.TimeOfDay != new TimeSpan(23, 59, 0) || ((int)prevPair.EndTime.Subtract(prevPair.StartTime).TotalMinutes + 1 + pairDuration) != 480)
                                                    {                                                        
                                                        unpaidLeave = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (!foundHolidayPair)
                                                unpaidLeave = true;
                                        }
                                        else if (pairInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                        {
                                            List<IOPairProcessedTO> nextPairs = new List<IOPairProcessedTO>();
                                            if (emplDayPairs.ContainsKey(pair.EmployeeID) && emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date.AddDays(1)))
                                                nextPairs = emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date.AddDays(1)];

                                            bool foundHolidayPair = false;
                                            // find personal holiday from next pairs
                                            foreach (IOPairProcessedTO nextPair in nextPairs)
                                            {
                                                if (rulesForEmpl[Constants.RulePersonalHolidayPassType].RuleValue == nextPair.PassTypeID)
                                                {
                                                    foundHolidayPair = true;

                                                    // check if it starts in midnight and duration is 8 - pairDuration
                                                    if (nextPair.StartTime.TimeOfDay != new TimeSpan(0, 0, 0) || ((int)nextPair.EndTime.Subtract(nextPair.StartTime).TotalMinutes + pairDuration) != 480)
                                                    {
                                                        unpaidLeave = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (!foundHolidayPair)
                                                unpaidLeave = true;
                                        }                                        
                                    }
                                }

                                if (unpaidLeave)
                                    category = (int)ReportTypeCategories.UNPAID_LEAVE;
                                else
                                    category = getReportCategory(pair.PassTypeID, rulesForEmpl, empl);
                            }

                            // round pair duration to full 30min for work on holiday and sickness leave
                            int rounding = 1;
                            if (rulesForEmpl.ContainsKey(Constants.RuleOvertimeRounding))
                                rounding = rulesForEmpl[Constants.RuleOvertimeRounding].RuleValue;

                            int regWorkAddition = 0;
                            int holidayAddition = 0;
                            if (pairDuration % rounding != 0)
                            {
                                if (category == (int)ReportTypeCategories.WORK_HOLIDAY)
                                {
                                    holidayAddition = pairDuration % rounding;
                                    pairDuration -= holidayAddition;
                                }

                                if (category == (int)ReportTypeCategories.SICK_LEAVE_100_PERCENT || category == (int)ReportTypeCategories.SICK_LEAVE_65_PERCENT
                                    || category == (int)ReportTypeCategories.SICK_LEAVE_OVER_30_DAYS || category == (int)ReportTypeCategories.PAID_LEAVE || category == -1)
                                {
                                    regWorkAddition = rounding - pairDuration % rounding;
                                    pairDuration += regWorkAddition;
                                }
                            }

                            if (category != -1)
                            {
                                if (!emplReportCategoryDuration.ContainsKey(pair.EmployeeID))
                                    emplReportCategoryDuration.Add(pair.EmployeeID, new Dictionary<int, int>());

                                if (!emplReportCategoryDuration[pair.EmployeeID].ContainsKey(category))
                                    emplReportCategoryDuration[pair.EmployeeID].Add(category, 0);

                                emplReportCategoryDuration[pair.EmployeeID][category] += pairDuration;
                            }
                            else if (pair.PassTypeID == Constants.unpaidLeave)
                            {
                                if (!emplReportCategoryDuration.ContainsKey(pair.EmployeeID))
                                    emplReportCategoryDuration.Add(pair.EmployeeID, new Dictionary<int, int>());

                                if (!emplReportCategoryDuration[pair.EmployeeID].ContainsKey((int)ReportTypeCategories.NOT_PAID))
                                    emplReportCategoryDuration[pair.EmployeeID].Add((int)ReportTypeCategories.NOT_PAID, 0);

                                emplReportCategoryDuration[pair.EmployeeID][(int)ReportTypeCategories.NOT_PAID] += pairDuration;
                            }
                            else if (pair.PassTypeID == Constants.unjustifiedLeave)
                            {
                                if (!emplReportCategoryDuration.ContainsKey(pair.EmployeeID))
                                    emplReportCategoryDuration.Add(pair.EmployeeID, new Dictionary<int, int>());

                                if (!emplReportCategoryDuration[pair.EmployeeID].ContainsKey((int)ReportTypeCategories.UNJUSTIFIED))
                                    emplReportCategoryDuration[pair.EmployeeID].Add((int)ReportTypeCategories.UNJUSTIFIED, 0);

                                emplReportCategoryDuration[pair.EmployeeID][(int)ReportTypeCategories.UNJUSTIFIED] += pairDuration;
                            }
                            
                            // get category and par type code
                            string code = getReportCode(category, pair.PassTypeID);

                            if (code == "")
                                code = ReportTypeCodes.UNPAID.ToString();
                            
                            if (!emplHrs.ContainsKey(pair.EmployeeID))
                                emplHrs.Add(pair.EmployeeID, new Dictionary<DateTime, Dictionary<string, int>>());

                            if (!emplHrs[pair.EmployeeID].ContainsKey(pairDate.Date))
                                emplHrs[pair.EmployeeID].Add(pairDate.Date, new Dictionary<string, int>());

                            if (!emplHrs[pair.EmployeeID][pairDate.Date].ContainsKey(code))
                                emplHrs[pair.EmployeeID][pairDate.Date].Add(code, 0);

                            emplHrs[pair.EmployeeID][pairDate.Date][code] += pairDuration;
                            
                            if (holidayAddition > 0)
                            {
                                if (!emplReportCategoryDuration.ContainsKey(pair.EmployeeID))
                                    emplReportCategoryDuration.Add(pair.EmployeeID, new Dictionary<int, int>());

                                if (!emplReportCategoryDuration[pair.EmployeeID].ContainsKey((int)ReportTypeCategories.HOLIDAY))
                                    emplReportCategoryDuration[pair.EmployeeID].Add((int)ReportTypeCategories.HOLIDAY, 0);

                                emplReportCategoryDuration[pair.EmployeeID][(int)ReportTypeCategories.HOLIDAY] += holidayAddition;
                            }

                            if (regWorkAddition > 0)
                            {
                                if (!emplReportCategoryDuration.ContainsKey(pair.EmployeeID))
                                    emplReportCategoryDuration.Add(pair.EmployeeID, new Dictionary<int, int>());

                                if (!emplReportCategoryDuration[pair.EmployeeID].ContainsKey((int)ReportTypeCategories.REG_WORK))
                                    emplReportCategoryDuration[pair.EmployeeID].Add((int)ReportTypeCategories.REG_WORK, 0);

                                emplReportCategoryDuration[pair.EmployeeID][(int)ReportTypeCategories.REG_WORK] -= regWorkAddition;

                                if (!emplHrs.ContainsKey(pair.EmployeeID))
                                    emplHrs.Add(pair.EmployeeID, new Dictionary<DateTime, Dictionary<string, int>>());

                                if (!emplHrs[pair.EmployeeID].ContainsKey(pairDate.Date))
                                    emplHrs[pair.EmployeeID].Add(pairDate.Date, new Dictionary<string, int>());

                                if (!emplHrs[pair.EmployeeID][pairDate.Date].ContainsKey(ReportTypeCodes.RW.ToString()))
                                    emplHrs[pair.EmployeeID][pairDate.Date].Add(ReportTypeCodes.RW.ToString(), 0);

                                emplHrs[pair.EmployeeID][pairDate.Date][ReportTypeCodes.RW.ToString()] -= regWorkAddition;
                            }

                            if (category == -1)
                                continue;

                            if (isMealPresence(pair.PassTypeID, rulesForEmpl))
                            {
                                // count presence for meals
                                if (!emplPresenceByDay.ContainsKey(pair.EmployeeID))
                                    emplPresenceByDay.Add(pair.EmployeeID, new Dictionary<DateTime, int>());
                                if (!emplPresenceByDay[pair.EmployeeID].ContainsKey(pairDate.Date))
                                    emplPresenceByDay[pair.EmployeeID].Add(pairDate.Date, 0);
                                emplPresenceByDay[pair.EmployeeID][pairDate.Date] += pairDuration;
                            }

                            if (isPresence(pair.PassTypeID, rulesForEmpl))
                            {
                                // count night work
                                int nightWork = nightWorkDuration(rulesForEmpl, pair.IOPairDate, pair.StartTime, pair.EndTime, empl);

                                if (nightWork > 0)
                                {
                                    if (!emplNightWorkDict.ContainsKey(pair.EmployeeID))
                                        emplNightWorkDict.Add(pair.EmployeeID, new Dictionary<DateTime, int>());

                                    if (!emplNightWorkDict[pair.EmployeeID].ContainsKey(pairDate.Date))
                                        emplNightWorkDict[pair.EmployeeID].Add(pairDate.Date, 0);

                                    emplNightWorkDict[pair.EmployeeID][pairDate.Date] += nightWork;
                                }
                            }
                        }
                    }
                    else if (pairDate.Date > to.Date && swList.Contains(pair.PassTypeID))
                    {
                        // add to stop working after selected period
                        int duration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;
                        if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                            duration++;

                        if (!emplSWAfter.ContainsKey(pair.EmployeeID))
                            emplSWAfter.Add(pair.EmployeeID, 0);

                        emplSWAfter[pair.EmployeeID] += duration;
                    }
                }

                foreach (int id in emplCounters.Keys)
                {
                    if (!emplCounters[id].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                        continue;

                    // decrease stop working with values after period
                    if (emplSWAfter.ContainsKey(id))
                        emplCounters[id][(int)Constants.EmplCounterTypes.StopWorkingCounter].Value -= emplSWAfter[id];
                    
                    // decrease stop working for unpaid hours from previous month
                    if (emplCounters[id][(int)Constants.EmplCounterTypes.StopWorkingCounter].Value > 0 && emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID))
                    {
                        // get overtime rounding
                        int rounding = 1;
                        if (emplRules.ContainsKey(id) && emplRules[id].ContainsKey(Constants.RuleOvertimeRounding))
                            rounding = emplRules[id][Constants.RuleOvertimeRounding].RuleValue;

                        // get stop working rounded value, overtime hours cover full 30min
                        int swCoverValue = emplCounters[id][(int)Constants.EmplCounterTypes.StopWorkingCounter].Value;

                        if (swCoverValue % rounding != 0)
                            swCoverValue += rounding - swCoverValue % rounding;

                        if (!emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.STOP_WORKING))
                            emplReportCategoryDuration[id].Add((int)ReportTypeCategories.STOP_WORKING, 0);

                        if (emplReportCategoryDuration[id][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID] <= swCoverValue)
                        {
                            int swCoverMinutes = Math.Min(emplReportCategoryDuration[id][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID], emplCounters[id][(int)Constants.EmplCounterTypes.StopWorkingCounter].Value);

                            emplReportCategoryDuration[id][(int)ReportTypeCategories.STOP_WORKING] -= swCoverMinutes;

                            emplCounters[id][(int)Constants.EmplCounterTypes.StopWorkingCounter].Value -= swCoverMinutes;

                            emplReportCategoryDuration[id][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID] = 0;
                        }
                        else
                        {
                            emplReportCategoryDuration[id][(int)ReportTypeCategories.STOP_WORKING] -= emplCounters[id][(int)Constants.EmplCounterTypes.StopWorkingCounter].Value;

                            emplReportCategoryDuration[id][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID] -= swCoverValue;

                            emplCounters[id][(int)Constants.EmplCounterTypes.StopWorkingCounter].Value = 0;
                        }
                    }
                }

                // put unpaid hours to regular work
                foreach (int emplID in emplReportCategoryDuration.Keys)
                {
                    if (emplReportCategoryDuration[emplID].ContainsKey((int)ReportTypeCategories.UNPAID_LEAVE) && emplReportCategoryDuration[emplID][(int)ReportTypeCategories.UNPAID_LEAVE] > 0)
                    {
                        if (!emplReportCategoryDuration[emplID].ContainsKey((int)ReportTypeCategories.REG_WORK))
                            emplReportCategoryDuration[emplID].Add((int)ReportTypeCategories.REG_WORK, 0);

                        emplReportCategoryDuration[emplID][(int)ReportTypeCategories.REG_WORK] += emplReportCategoryDuration[emplID][(int)ReportTypeCategories.UNPAID_LEAVE];

                        // cover unpaid hours
                        if (emplReportCategoryDuration[emplID].ContainsKey((int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID) && emplReportCategoryDuration[emplID][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID] > 0)
                        {
                            // get overtime rounding
                            int rounding = 1;
                            if (emplRules.ContainsKey(emplID) && emplRules[emplID].ContainsKey(Constants.RuleOvertimeRounding))
                                rounding = emplRules[emplID][Constants.RuleOvertimeRounding].RuleValue;

                            // get unpaid hours rounded value, overtime hours cover full 30min
                            int unpaidCoverValue = emplReportCategoryDuration[emplID][(int)ReportTypeCategories.UNPAID_LEAVE];

                            if (unpaidCoverValue % rounding != 0)
                                unpaidCoverValue += rounding - unpaidCoverValue % rounding;

                            if (emplReportCategoryDuration[emplID][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID] <= unpaidCoverValue)
                            {
                                int unpaidCoverMinutes = Math.Min(emplReportCategoryDuration[emplID][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID], emplReportCategoryDuration[emplID][(int)ReportTypeCategories.UNPAID_LEAVE]);

                                emplReportCategoryDuration[emplID][(int)ReportTypeCategories.UNPAID_LEAVE] -= unpaidCoverMinutes;

                                emplReportCategoryDuration[emplID][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID] = 0;
                            }
                            else
                            {
                                emplReportCategoryDuration[emplID][(int)ReportTypeCategories.UNPAID_LEAVE] = 0;

                                emplReportCategoryDuration[emplID][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID] -= unpaidCoverValue;
                            }
                        }
                    }

                    if (emplReportCategoryDuration[emplID].ContainsKey((int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID) && emplReportCategoryDuration[emplID][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID] > 0)
                    {
                        int overtimeToPaid = emplReportCategoryDuration[emplID][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID];

                        int monthLimit = 45000;
                        if (emplRules.ContainsKey(emplID) && emplRules[emplID].ContainsKey(Constants.RuleOvertimeMonthLimit))
                            monthLimit = emplRules[emplID][Constants.RuleOvertimeMonthLimit].RuleValue * 60;

                        if (monthLimit < overtimeToPaid)
                            overtimeToPaid = monthLimit;

                        if (ascoDict.ContainsKey(emplID) && ascoDict[emplID].IntegerValue7 == Constants.yesInt)
                            overtimeToPaid = 0;

                        if (overtimeToPaid < emplReportCategoryDuration[emplID][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID])
                        {
                            if (!emplReportCategoryDuration[emplID].ContainsKey((int)ReportTypeCategories.BANK_HOURS))
                                emplReportCategoryDuration[emplID].Add((int)ReportTypeCategories.BANK_HOURS, 0);

                            emplReportCategoryDuration[emplID][(int)ReportTypeCategories.BANK_HOURS] += emplReportCategoryDuration[emplID][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID] - overtimeToPaid;
                        }

                        if (overtimeToPaid > 0)
                        {
                            if (!emplReportCategoryDuration[emplID].ContainsKey((int)ReportTypeCategories.OVERTIME_PAID))
                                emplReportCategoryDuration[emplID].Add((int)ReportTypeCategories.OVERTIME_PAID, 0);

                            emplReportCategoryDuration[emplID][(int)ReportTypeCategories.OVERTIME_PAID] += overtimeToPaid;
                        }

                        emplReportCategoryDuration[emplID][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID] = overtimeToPaid;
                    }
                }

                foreach (int id in overtimePairs.Keys)
                {
                    foreach (DateTime date in overtimePairs[id].Keys)
                    {
                        foreach (IOPairProcessedTO pair in overtimePairs[id][date])
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

                            // calculate code duration
                            int pairDuration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                            if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                pairDuration++;

                            // calculate work on holiday and night work before processing overtime pair
                            if (asco.IntegerValue7 != Constants.yesInt || asco.IntegerValue8 == Constants.yesInt)
                            {
                                if (isWorkOnHoliday(personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, nationalTransferableHolidays, paidDays, date.Date, schema, asco, empl))
                                {
                                    if (pairDuration > 0)
                                    {
                                        if (!emplReportCategoryDuration.ContainsKey(pair.EmployeeID))
                                            emplReportCategoryDuration.Add(pair.EmployeeID, new Dictionary<int, int>());

                                        if (!emplReportCategoryDuration[pair.EmployeeID].ContainsKey((int)ReportTypeCategories.WORK_HOLIDAY))
                                            emplReportCategoryDuration[pair.EmployeeID].Add((int)ReportTypeCategories.WORK_HOLIDAY, 0);

                                        emplReportCategoryDuration[pair.EmployeeID][(int)ReportTypeCategories.WORK_HOLIDAY] += pairDuration;
                                    }
                                }

                                int nightWork = nightWorkDuration(rulesForEmpl, pair.IOPairDate, pair.StartTime, pair.EndTime, empl);

                                if (nightWork > 0)
                                {
                                    if (!emplNightWorkDict.ContainsKey(empl.EmployeeID))
                                        emplNightWorkDict.Add(empl.EmployeeID, new Dictionary<DateTime, int>());

                                    if (!emplNightWorkDict[empl.EmployeeID].ContainsKey(date.Date))
                                        emplNightWorkDict[empl.EmployeeID].Add(date.Date, 0);

                                    emplNightWorkDict[empl.EmployeeID][date.Date] += nightWork;
                                }
                            }

                            processOvertime(pairDuration, empl, asco, date, rulesForEmpl, emplCounters, emplDayOvertime, emplWeekOvertime, emplReportCategoryDuration, emplPresenceByDay);
                        }
                    }
                }

                foreach (int id in emplCounters.Keys)
                {
                    if (!emplCounters[id].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                        continue;
                    
                    // increase stop working for unpaid hours
                    if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.UNPAID_LEAVE)
                        && emplReportCategoryDuration[id][(int)ReportTypeCategories.UNPAID_LEAVE] > 0)
                    {
                        if (!emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.STOP_WORKING))
                            emplReportCategoryDuration[id].Add((int)ReportTypeCategories.STOP_WORKING, 0);

                        emplReportCategoryDuration[id][(int)ReportTypeCategories.STOP_WORKING] += emplReportCategoryDuration[id][(int)ReportTypeCategories.UNPAID_LEAVE];
                    }
                }

                // put night work to corresponding report category due to overtime rounding rules (full 30min of each day are paid as night work)
                foreach (int id in emplNightWorkDict.Keys)
                {
                    int rounding = 1;
                    if (emplRules.ContainsKey(id) && emplRules[id].ContainsKey(Constants.RuleOvertimeRounding))
                        rounding = emplRules[id][Constants.RuleOvertimeRounding].RuleValue;

                    foreach (DateTime date in emplNightWorkDict[id].Keys)
                    {
                        int paidNightWork = emplNightWorkDict[id][date] - (emplNightWorkDict[id][date] % rounding);

                        if (paidNightWork > 0)
                        {
                            if (!emplReportCategoryDuration.ContainsKey(id))
                                emplReportCategoryDuration.Add(id, new Dictionary<int, int>());

                            if (!emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.NIGHT_WORK))
                                emplReportCategoryDuration[id].Add((int)ReportTypeCategories.NIGHT_WORK, 0);

                            emplReportCategoryDuration[id][(int)ReportTypeCategories.NIGHT_WORK] += paidNightWork;
                        }
                    }
                }

                if (unconfirmed.Count > 0)
                {
                    string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "//UnregularData_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                    
                    FileStream stream = new FileStream(filePath, FileMode.Append);
                    stream.Close();

                    // insert header                    
                    string header = rm.GetString("hdrEmployeeID", culture) + unconfirmedDataDelimiter + rm.GetString("hdrName", culture) + unconfirmedDataDelimiter + rm.GetString("hdrDay", culture);

                    StreamWriter writer = File.AppendText(filePath);
                    writer.WriteLine(header);

                    foreach (int id in unconfirmed.Keys)
                    {
                        foreach (DateTime date in unconfirmed[id])
                        {
                            string line = id.ToString().Trim();
                            if (emplDict.ContainsKey(id))
                                line += unconfirmedDataDelimiter + emplDict[id].FirstAndLastName.Trim();
                            else
                                line += unconfirmedDataDelimiter + " ";

                            line += unconfirmedDataDelimiter + date.ToString(Constants.dateFormat);

                            writer.WriteLine(line);
                        }
                    }

                    writer.Close();

                    MessageBox.Show(rm.GetString("unconfirmedDataFound", culture));
                    return;
                }

                int mealMinimalPresenceMinutes = 0;
                foreach (int id in emplPresenceByDay.Keys)
                {
                    foreach (DateTime day in emplPresenceByDay[id].Keys)
                    {
                        if (emplRules.ContainsKey(id) && emplRules[id].ContainsKey(Constants.RuleMealMinPresence))
                            mealMinimalPresenceMinutes = emplRules[id][Constants.RuleMealMinPresence].RuleValue;
                        else
                            mealMinimalPresenceMinutes = 0;

                        if (emplPresenceByDay[id][day] >= mealMinimalPresenceMinutes)
                        {
                            if (!emplMeals.ContainsKey(id))
                                emplMeals.Add(id, 0);

                            emplMeals[id]++;
                        }
                    }
                }

                // recalculate overtime pairs
                foreach (int wu in wuEmplDict.Keys)
                {
                    foreach (int emplID in wuEmplDict[wu].Keys)
                    {
                        Dictionary<DateTime, int> dayOvertimeHours = new Dictionary<DateTime, int>();
                        Dictionary<DateTime, int> weekOvertimeHours = new Dictionary<DateTime, int>();

                        int overtimeToRecalculate = 0;
                        if (emplReportCategoryDuration.ContainsKey(emplID) && emplReportCategoryDuration[emplID].ContainsKey((int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID))
                            overtimeToRecalculate += emplReportCategoryDuration[emplID][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID];

                        if (overtimeToRecalculate == 0 && !emplDayOvertime.ContainsKey(emplID))
                            continue;

                        // limits in minutes
                        int dayLimit = 1440;
                        int weekLimit = 10080;
                        int monthLimit = 45000;
                        
                        // get day, week and month limit
                        if (emplRules.ContainsKey(emplID) && emplRules[emplID].ContainsKey(Constants.RuleOvertimeDayLimit))
                            dayLimit = emplRules[emplID][Constants.RuleOvertimeDayLimit].RuleValue * 60;

                        if (emplRules.ContainsKey(emplID) && emplRules[emplID].ContainsKey(Constants.RuleOvertimeWeekLimit))
                            weekLimit = emplRules[emplID][Constants.RuleOvertimeWeekLimit].RuleValue * 60;

                        if (emplRules.ContainsKey(emplID) && emplRules[emplID].ContainsKey(Constants.RuleOvertimeMonthLimit))
                            monthLimit = emplRules[emplID][Constants.RuleOvertimeMonthLimit].RuleValue * 60;

                        int overtimeTotal = 0;

                        // go through all day pars and 
                        // get allowed day, week, month duration
                        if (emplDayOvertime.ContainsKey(emplID))
                        {
                            foreach (DateTime date in emplDayOvertime[emplID].Keys)
                            {
                                int dayAllowed = dayLimit;
                                int weekAllowed = weekLimit;
                                int monthAllowed = monthLimit;

                                DateTime pairWeek = Common.Misc.getWeekBeggining(date.Date).Date;

                                if (dayOvertimeHours.ContainsKey(date.Date))
                                    dayAllowed = dayLimit - dayOvertimeHours[date.Date];

                                if (dayAllowed < 0)
                                    dayAllowed = 0;

                                if (weekOvertimeHours.ContainsKey(pairWeek.Date))
                                    weekAllowed = weekLimit - weekOvertimeHours[pairWeek.Date];

                                if (weekAllowed < 0)
                                    weekAllowed = 0;
                                
                                monthAllowed = monthLimit - overtimeTotal;

                                if (monthAllowed < 0)
                                    monthAllowed = 0;

                                // get maximal duration that can be calculated as overtime
                                int maxOvertimeDuration = emplDayOvertime[emplID][date];

                                if (dayAllowed < maxOvertimeDuration)
                                    maxOvertimeDuration = dayAllowed;

                                if (weekAllowed < maxOvertimeDuration)
                                    maxOvertimeDuration = weekAllowed;

                                if (monthAllowed < maxOvertimeDuration)
                                    maxOvertimeDuration = monthAllowed;

                                if (maxOvertimeDuration > 0)
                                {
                                    if (!dayOvertimeHours.ContainsKey(date))
                                        dayOvertimeHours.Add(date, 0);

                                    dayOvertimeHours[date] += maxOvertimeDuration;

                                    if (!weekOvertimeHours.ContainsKey(pairWeek.Date))
                                        weekOvertimeHours.Add(pairWeek.Date, 0);

                                    weekOvertimeHours[pairWeek.Date] += maxOvertimeDuration;

                                    overtimeTotal += maxOvertimeDuration;
                                }

                                if (emplDayOvertime[emplID][date] > maxOvertimeDuration)
                                    overtimeToRecalculate += (emplDayOvertime[emplID][date] - maxOvertimeDuration);
                            }
                        }

                        // put overtime to recalculate hours to specific days
                        DateTime currDay = from.Date;
                        while (currDay.Date <= to.Date && overtimeToRecalculate > 0)
                        {
                            // do not recalculate overtime in holiday days
                            if (!dayOvertimeHours.ContainsKey(currDay.Date) && (nationalHolidaysDays.Contains(currDay.Date) || nationalHolidaysSundays.Contains(currDay.Date)))
                            {
                                currDay = currDay.Date.AddDays(1);
                                continue;
                            }

                            // day is working day
                            if (emplHrs.ContainsKey(emplID) && emplHrs[emplID].ContainsKey(currDay.Date) && emplHrs[emplID][currDay.Date].ContainsKey(ReportTypeCodes.RW.ToString()))
                            {
                                int dayAllowed = dayLimit;
                                int weekAllowed = weekLimit;
                                int monthAllowed = monthLimit;

                                DateTime pairWeek = Common.Misc.getWeekBeggining(currDay.Date).Date;

                                if (dayOvertimeHours.ContainsKey(currDay.Date))
                                    dayAllowed = dayLimit - dayOvertimeHours[currDay.Date];

                                if (dayAllowed < 0)
                                    dayAllowed = 0;

                                if (weekOvertimeHours.ContainsKey(pairWeek.Date))
                                    weekAllowed = weekLimit - weekOvertimeHours[pairWeek.Date];

                                if (weekAllowed < 0)
                                    weekAllowed = 0;

                                monthAllowed = monthLimit - overtimeTotal;

                                if (monthAllowed < 0)
                                    monthAllowed = 0;

                                // get maximal duration that can be calculated as overtime
                                int maxOvertimeDuration = overtimeToRecalculate;

                                if (dayAllowed < maxOvertimeDuration)
                                    maxOvertimeDuration = dayAllowed;

                                if (weekAllowed < maxOvertimeDuration)
                                    maxOvertimeDuration = weekAllowed;

                                if (monthAllowed < maxOvertimeDuration)
                                    maxOvertimeDuration = monthAllowed;

                                if (maxOvertimeDuration > 0)
                                {
                                    if (!dayOvertimeHours.ContainsKey(currDay))
                                        dayOvertimeHours.Add(currDay, 0);

                                    dayOvertimeHours[currDay] += maxOvertimeDuration;

                                    if (!weekOvertimeHours.ContainsKey(pairWeek.Date))
                                        weekOvertimeHours.Add(pairWeek.Date, 0);

                                    weekOvertimeHours[pairWeek.Date] += maxOvertimeDuration;

                                    overtimeTotal += maxOvertimeDuration;
                                }
                                                               
                                overtimeToRecalculate -= maxOvertimeDuration;
                            }

                            currDay = currDay.AddDays(1);
                        }

                        if (!emplOvertimeHrs.ContainsKey(emplID))
                            emplOvertimeHrs.Add(emplID, new Dictionary<DateTime, int>());

                        emplOvertimeHrs[emplID] = dayOvertimeHours;
                    }
                }

                // generate xls report
                generateReport(emplDict, ascoDict, emplRules, emplReportCategoryDuration, emplMeals);

                DialogResult result = MessageBox.Show(rm.GetString("generatingDailyReport", culture), "", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    // generate daily reports
                    generateAnaliticalReport(wuEmplDict, ascoDict, from, emplHrs, emplNightWorkDict, emplOvertimeHrs, emplRules);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PMCPYReport.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void processOvertime(int pairDuration, EmployeeTO empl, EmployeeAsco4TO asco, DateTime pairDate, Dictionary<string, RuleTO> rulesForEmpl, 
            Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounters, Dictionary<int, Dictionary<DateTime, int>> emplDayOvertime,
            Dictionary<int, Dictionary<DateTime, int>> emplWeekOvertime, Dictionary<int, Dictionary<int, int>> emplReportCategoryDuration, Dictionary<int, Dictionary<DateTime, int>> emplPresenceByDay)
        {
            try
            {
                int swCategory = (int)ReportTypeCategories.STOP_WORKING;
                int overtimeCategory = (int)ReportTypeCategories.OVERTIME_PAID;

                // get overtime rounding
                int rounding = 1;
                if (rulesForEmpl.ContainsKey(Constants.RuleOvertimeRounding))
                    rounding = rulesForEmpl[Constants.RuleOvertimeRounding].RuleValue;

                // put hours on stop working first                
                if (emplCounters.ContainsKey(empl.EmployeeID) && emplCounters[empl.EmployeeID].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter)
                    && emplCounters[empl.EmployeeID][(int)Constants.EmplCounterTypes.StopWorkingCounter].Value > 0)
                {
                    // get stop working rounded value, overtime hours cover full 30min
                    int swCoverValue = emplCounters[empl.EmployeeID][(int)Constants.EmplCounterTypes.StopWorkingCounter].Value;

                    if (swCoverValue % rounding != 0)
                        swCoverValue += rounding - swCoverValue % rounding;

                    if (!emplReportCategoryDuration.ContainsKey(empl.EmployeeID))
                        emplReportCategoryDuration.Add(empl.EmployeeID, new Dictionary<int, int>());

                    if (!emplReportCategoryDuration[empl.EmployeeID].ContainsKey(swCategory))
                        emplReportCategoryDuration[empl.EmployeeID].Add(swCategory, 0);

                    if (swCoverValue >= pairDuration)
                    {
                        int swCoverMinutes = Math.Min(pairDuration, emplCounters[empl.EmployeeID][(int)Constants.EmplCounterTypes.StopWorkingCounter].Value);

                        emplReportCategoryDuration[empl.EmployeeID][swCategory] -= swCoverMinutes;

                        emplCounters[empl.EmployeeID][(int)Constants.EmplCounterTypes.StopWorkingCounter].Value -= swCoverMinutes;

                        return;
                    }
                    else // put as much hours as possible to stop working and continue processing the rest of overtime pair
                    {
                        emplReportCategoryDuration[empl.EmployeeID][swCategory] -= emplCounters[empl.EmployeeID][(int)Constants.EmplCounterTypes.StopWorkingCounter].Value;

                        pairDuration -= swCoverValue;                        

                        emplCounters[empl.EmployeeID][(int)Constants.EmplCounterTypes.StopWorkingCounter].Value = 0;
                    }
                }

                // calculate presence for meal
                if (asco.IntegerValue7 != Constants.yesInt)
                {
                    // calculate meal presence
                    if (!emplPresenceByDay.ContainsKey(empl.EmployeeID))
                        emplPresenceByDay.Add(empl.EmployeeID, new Dictionary<DateTime, int>());
                    if (!emplPresenceByDay[empl.EmployeeID].ContainsKey(pairDate.Date))
                        emplPresenceByDay[empl.EmployeeID].Add(pairDate.Date, 0);
                    emplPresenceByDay[empl.EmployeeID][pairDate.Date] += pairDuration;
                }

                // cover unpaid hours
                if (emplReportCategoryDuration.ContainsKey(empl.EmployeeID) && emplReportCategoryDuration[empl.EmployeeID].ContainsKey((int)ReportTypeCategories.UNPAID_LEAVE)
                    && emplReportCategoryDuration[empl.EmployeeID][(int)ReportTypeCategories.UNPAID_LEAVE] > 0)
                {
                    // get unpaid rounded value, overtime hours cover full 30min
                    int unpaidCoverValue = emplReportCategoryDuration[empl.EmployeeID][(int)ReportTypeCategories.UNPAID_LEAVE];

                    if (unpaidCoverValue % rounding != 0)
                        unpaidCoverValue += rounding - unpaidCoverValue % rounding;
                    
                    if (unpaidCoverValue >= pairDuration)
                    {
                        int unpaidCoverMinutes = Math.Min(pairDuration, emplReportCategoryDuration[empl.EmployeeID][(int)ReportTypeCategories.UNPAID_LEAVE]);
                        
                        emplReportCategoryDuration[empl.EmployeeID][(int)ReportTypeCategories.UNPAID_LEAVE] -= unpaidCoverMinutes;

                        return;
                    }
                    else // cover as much unpaid hours as possible and continue processing the rest of overtime pair
                    {
                        pairDuration -= unpaidCoverValue;

                        emplReportCategoryDuration[empl.EmployeeID][(int)ReportTypeCategories.UNPAID_LEAVE] = 0;
                    }
                }

                // limits in minutes
                int dayLimit = 1440;
                int weekLimit = 10080;
                int monthLimit = 45000;

                // check overtime hours
                // check day, week and month limit
                //if (rulesForEmpl.ContainsKey(Constants.RuleOvertimeDayLimit))
                //    dayLimit = rulesForEmpl[Constants.RuleOvertimeDayLimit].RuleValue * 60;

                //if (rulesForEmpl.ContainsKey(Constants.RuleOvertimeWeekLimit))
                //    weekLimit = rulesForEmpl[Constants.RuleOvertimeWeekLimit].RuleValue * 60;

                if (rulesForEmpl.ContainsKey(Constants.RuleOvertimeMonthLimit))
                    monthLimit = rulesForEmpl[Constants.RuleOvertimeMonthLimit].RuleValue * 60;

                // get allowed day, week, month duration
                int dayAllowed = dayLimit;
                int weekAllowed = weekLimit;
                int monthAllowed = monthLimit;

                DateTime pairWeek = Common.Misc.getWeekBeggining(pairDate.Date).Date;

                if (emplDayOvertime.ContainsKey(empl.EmployeeID) && emplDayOvertime[empl.EmployeeID].ContainsKey(pairDate.Date))
                    dayAllowed = dayLimit - emplDayOvertime[empl.EmployeeID][pairDate.Date];

                if (emplWeekOvertime.ContainsKey(empl.EmployeeID) && emplWeekOvertime[empl.EmployeeID].ContainsKey(pairWeek.Date))
                    weekAllowed = weekLimit - emplWeekOvertime[empl.EmployeeID][pairWeek.Date];

                if (emplReportCategoryDuration.ContainsKey(empl.EmployeeID) && emplReportCategoryDuration[empl.EmployeeID].ContainsKey(overtimeCategory))
                    monthAllowed -= emplReportCategoryDuration[empl.EmployeeID][overtimeCategory];

                if (monthAllowed < 0)
                    monthAllowed = 0;

                // get maximal duration that can be calculated as overtime
                int maxOvertimeDuration = pairDuration;

                if (dayAllowed < maxOvertimeDuration)
                    maxOvertimeDuration = dayAllowed;

                if (weekAllowed < maxOvertimeDuration)
                    maxOvertimeDuration = weekAllowed;

                if (monthAllowed < maxOvertimeDuration)
                    maxOvertimeDuration = monthAllowed;

                if (asco.IntegerValue7 == Constants.yesInt)
                    maxOvertimeDuration = 0;

                if (maxOvertimeDuration > 0)
                {
                    if (!emplReportCategoryDuration.ContainsKey(empl.EmployeeID))
                        emplReportCategoryDuration.Add(empl.EmployeeID, new Dictionary<int, int>());

                    if (!emplReportCategoryDuration[empl.EmployeeID].ContainsKey(overtimeCategory))
                        emplReportCategoryDuration[empl.EmployeeID].Add(overtimeCategory, 0);

                    emplReportCategoryDuration[empl.EmployeeID][overtimeCategory] += maxOvertimeDuration;

                    if (!emplDayOvertime.ContainsKey(empl.EmployeeID))
                        emplDayOvertime.Add(empl.EmployeeID, new Dictionary<DateTime, int>());

                    if (!emplDayOvertime[empl.EmployeeID].ContainsKey(pairDate.Date))
                        emplDayOvertime[empl.EmployeeID].Add(pairDate.Date, 0);

                    emplDayOvertime[empl.EmployeeID][pairDate.Date] += maxOvertimeDuration;

                    if (!emplWeekOvertime.ContainsKey(empl.EmployeeID))
                        emplWeekOvertime.Add(empl.EmployeeID, new Dictionary<DateTime, int>());

                    if (!emplWeekOvertime[empl.EmployeeID].ContainsKey(pairWeek.Date))
                        emplWeekOvertime[empl.EmployeeID].Add(pairWeek.Date, 0);

                    emplWeekOvertime[empl.EmployeeID][pairWeek.Date] += maxOvertimeDuration;
                                        
                    pairDuration -= maxOvertimeDuration;
                    
                    if (pairDuration <= 0)
                        return;
                }

                if (pairDuration > 0)
                {
                    if (!emplReportCategoryDuration.ContainsKey(empl.EmployeeID))
                        emplReportCategoryDuration.Add(empl.EmployeeID, new Dictionary<int, int>());

                    if (!emplReportCategoryDuration[empl.EmployeeID].ContainsKey((int)ReportTypeCategories.BANK_HOURS))
                        emplReportCategoryDuration[empl.EmployeeID].Add((int)ReportTypeCategories.BANK_HOURS, 0);

                    emplReportCategoryDuration[empl.EmployeeID][(int)ReportTypeCategories.BANK_HOURS] += pairDuration;

                    if (!emplReportCategoryDuration[empl.EmployeeID].ContainsKey((int)ReportTypeCategories.OVERTIME_NOT_PAID))
                        emplReportCategoryDuration[empl.EmployeeID].Add((int)ReportTypeCategories.OVERTIME_NOT_PAID, 0);

                    emplReportCategoryDuration[empl.EmployeeID][(int)ReportTypeCategories.OVERTIME_NOT_PAID] += pairDuration;
                }

                return;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PMCPYReport.processOvertime(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void generateReport(Dictionary<int, EmployeeTO> emplDict, Dictionary<int, EmployeeAsco4TO> ascoDict, Dictionary<int, Dictionary<string, RuleTO>> emplRules,
            Dictionary<int, Dictionary<int, int>> emplReportCategoryDuration, Dictionary<int, int> emplMeals)
        {
            try
            {
                if (emplDict.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }
                
                // create header
                string header = rm.GetString("hdrEmplID", culture) + delimiter + rm.GetString("hdrName", culture) + delimiter + rm.GetString("hdrJMBG", culture)
                    + delimiter + rm.GetString("hdrEmplCategory", culture) + delimiter + rm.GetString("hdrRegularWork", culture) + delimiter + rm.GetString("hdrOvertime", culture)
                    + delimiter + rm.GetString("hdrAnnualLeave", culture) + delimiter + rm.GetString("hdrHoliday", culture) + delimiter + rm.GetString("hdrWorkOnHoliday", culture)
                    + delimiter + rm.GetString("hdrPaidLeave", culture) + delimiter + rm.GetString("hdrPaidLeave60", culture) 
                    + delimiter + rm.GetString("hdrSickLeave100", culture) + delimiter + rm.GetString("hdrSickLeave65", culture)
                    + delimiter + rm.GetString("hdrSickLeaveOver30Days", culture) + delimiter + rm.GetString("hdrSickLeavePregnancy", culture)
                    // + delimiter + rm.GetString("hdrUnpaidLeave", culture) 
                    + delimiter + rm.GetString("hdrNightWork", culture)
                    + delimiter + rm.GetString("hdrMeals", culture) + delimiter + rm.GetString("hdrRegres", culture) + delimiter + rm.GetString("hdrFundHrs", culture)
                    + delimiter + rm.GetString("hdrStimulation", culture) + delimiter + rm.GetString("hdrNotPaid", culture) + delimiter + rm.GetString("hdrUnjustified", culture);                    
                
                List<string> lines = new List<string>();
                                
                List<EmployeePYDataBufferTO> buffers = new List<EmployeePYDataBufferTO>();

                // get max py calc id
                uint calcID = new EmployeePYDataBuffer().getMaxCalcID() + 1;

                // create file lines
                foreach (int id in emplDict.Keys)
                {
                    double regres = 0;
                    double fundHrs = 0;
                    double hrs = 0;

                    string line = id.ToString().Trim() + delimiter + emplDict[id].FirstAndLastName.Trim().Replace(delimiter, " ");

                    if (ascoDict.ContainsKey(id))
                        line += delimiter + ascoDict[id].NVarcharValue4.Trim().Replace(delimiter, " ");
                    else
                        line += delimiter + "";

                    if (emplTypes.ContainsKey(emplDict[id].EmployeeTypeID))
                        line += delimiter + emplTypes[emplDict[id].EmployeeTypeID].Trim().Replace(delimiter, " ");
                    else
                        line += delimiter + "";

                    if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.REG_WORK))
                    {
                        hrs = ((double)(emplReportCategoryDuration[id][(int)ReportTypeCategories.REG_WORK])) / 60;
                        regres += hrs;
                        fundHrs += hrs;
                        line += delimiter + hrs.ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    }
                    else
                        line += delimiter + "0";

                    if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.OVERTIME_PAID))
                    {
                        hrs = ((double)(emplReportCategoryDuration[id][(int)ReportTypeCategories.OVERTIME_PAID])) / 60;
                        fundHrs += hrs;
                        line += delimiter + hrs.ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    }
                    else
                        line += delimiter + "0";

                    if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.ANNUAL_LEAVE))
                    {
                        hrs = ((double)(emplReportCategoryDuration[id][(int)ReportTypeCategories.ANNUAL_LEAVE])) / 60;
                        regres += hrs;
                        fundHrs += hrs;
                        line += delimiter + hrs.ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    }
                    else
                        line += delimiter + "0";

                    if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.HOLIDAY))
                    {
                        hrs = ((double)(emplReportCategoryDuration[id][(int)ReportTypeCategories.HOLIDAY])) / 60;
                        regres += hrs;
                        fundHrs += hrs;
                        line += delimiter + hrs.ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    }
                    else
                        line += delimiter + "0";

                    if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.WORK_HOLIDAY))
                    {
                        hrs = ((double)(emplReportCategoryDuration[id][(int)ReportTypeCategories.WORK_HOLIDAY])) / 60;
                        regres += hrs;
                        fundHrs += hrs;
                        line += delimiter + hrs.ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    }
                    else
                        line += delimiter + "0";

                    if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.PAID_LEAVE))
                    {
                        hrs = ((double)(emplReportCategoryDuration[id][(int)ReportTypeCategories.PAID_LEAVE])) / 60;
                        regres += hrs;
                        fundHrs += hrs;
                        line += delimiter + hrs.ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    }
                    else
                        line += delimiter + "0";

                    if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.PAID_LEAVE_60))
                    {
                        hrs = ((double)(emplReportCategoryDuration[id][(int)ReportTypeCategories.PAID_LEAVE_60])) / 60;
                        regres += hrs;
                        fundHrs += hrs;
                        line += delimiter + hrs.ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    }
                    else
                        line += delimiter + "0";

                    if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.SICK_LEAVE_100_PERCENT))
                    {
                        hrs = ((double)(emplReportCategoryDuration[id][(int)ReportTypeCategories.SICK_LEAVE_100_PERCENT])) / 60;
                        regres += hrs;
                        fundHrs += hrs;
                        line += delimiter + hrs.ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    }
                    else
                        line += delimiter + "0";

                    if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.SICK_LEAVE_65_PERCENT))
                    {
                        hrs = ((double)(emplReportCategoryDuration[id][(int)ReportTypeCategories.SICK_LEAVE_65_PERCENT])) / 60;
                        regres += hrs;
                        fundHrs += hrs;
                        line += delimiter + hrs.ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    }
                    else
                        line += delimiter + "0";

                    if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.SICK_LEAVE_OVER_30_DAYS))
                    {
                        hrs = ((double)(emplReportCategoryDuration[id][(int)ReportTypeCategories.SICK_LEAVE_OVER_30_DAYS])) / 60;
                        regres += hrs;
                        fundHrs += hrs;
                        line += delimiter + hrs.ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    }
                    else
                        line += delimiter + "0";

                    if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.SICK_LEAVE_PREGNANCY))
                    {
                        hrs = ((double)(emplReportCategoryDuration[id][(int)ReportTypeCategories.SICK_LEAVE_PREGNANCY])) / 60;
                        regres += hrs;
                        fundHrs += hrs;
                        line += delimiter + hrs.ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    }
                    else
                        line += delimiter + "0";

                    //if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.UNPAID_LEAVE))
                    //    line += delimiter + (((double)(emplReportCategoryDuration[id][(int)ReportTypeCategories.UNPAID_LEAVE])) / 60).ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    //else
                    //    line += delimiter + "0";

                    if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.NIGHT_WORK))
                        line += delimiter + (((double)(emplReportCategoryDuration[id][(int)ReportTypeCategories.NIGHT_WORK])) / 60).ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    else
                        line += delimiter + "0";

                    if (emplMeals.ContainsKey(id) && !AgencyCategories.Contains(emplDict[id].EmployeeTypeID))
                        line += delimiter + emplMeals[id].ToString().Trim();
                    else
                        line += delimiter + "0";

                    if (AgencyCategories.Contains(emplDict[id].EmployeeTypeID))
                        regres = 0;

                    line += delimiter + regres.ToString(Constants.doubleFormat).Replace(delimiter, ".").Trim();

                    line += delimiter + fundHrs.ToString(Constants.doubleFormat).Replace(delimiter, ".").Trim();

                    if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.OVERTIME_NOT_PAID))
                        line += delimiter + (((double)(emplReportCategoryDuration[id][(int)ReportTypeCategories.OVERTIME_NOT_PAID])) / 60).ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    else
                        line += delimiter + "0";

                    if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.NOT_PAID))
                        line += delimiter + (((double)(emplReportCategoryDuration[id][(int)ReportTypeCategories.NOT_PAID])) / 60).ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    else
                        line += delimiter + "0";

                    if (emplReportCategoryDuration.ContainsKey(id) && emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.UNJUSTIFIED))
                        line += delimiter + (((double)(emplReportCategoryDuration[id][(int)ReportTypeCategories.UNJUSTIFIED])) / 60).ToString(Constants.doubleFormat).Replace(delimiter, ".");
                    else
                        line += delimiter + "0";

                    lines.Add(line);

                    // insert buffer record for each employee of report                    
                    Dictionary<int, int> buffMeals = new Dictionary<int, int>();
                    // add 3 keys to meals dictionary
                    buffMeals.Add(1, 0);
                    buffMeals.Add(2, 0);
                    buffMeals.Add(3, 0);
                    EmployeePYDataBufferTO buffTO = new EmployeePYDataBufferTO();
                    buffTO.PYCalcID = calcID;
                    buffTO.EmployeeID = id;
                    buffTO.ApprovedMeals = buffMeals;
                    buffTO.BankHrsBalans = 0;
                    buffTO.CCDesc = "N/A";
                    buffTO.CCName = "N/A";
                    buffTO.CompanyCode = "N/A";
                    buffTO.DateStart = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
                    buffTO.DateEnd = buffTO.DateStart.AddMonths(1).AddDays(-1);
                    if (emplTypes.ContainsKey(emplDict[id].EmployeeTypeID))
                        buffTO.EmployeeType = emplTypes[emplDict[id].EmployeeTypeID].Trim();
                    else
                        buffTO.EmployeeType = "N/A";
                    buffTO.FirstName = emplDict[id].FirstName;
                    buffTO.LastName = emplDict[id].LastName;
                    buffTO.MealCounter = 0;
                    buffTO.NotApprovedMeals = buffMeals;
                    buffTO.NotJustifiedOvertimeBalans = 0;
                    buffTO.PaidLeaveBalans = 0;
                    buffTO.PaidLeaveUsed = 0;
                    buffTO.StopWorkingHrsBalans = 0;
                    buffTO.TransportCounter = 0;
                    buffTO.Type = Constants.PYTypeReal.Trim();
                    buffTO.VacationLeftCurrYear = 0;
                    buffTO.VacationLeftPrevYear = 0;
                    buffTO.VacationUsedCurrYear = 0;
                    buffTO.ValactionUsedPrevYear = 0;
                    
                    buffTO.BankHrsBalans = 0;
                    buffTO.StopWorkingHrsBalans = 0;
                    buffTO.NotJustifiedOvertime = 0;

                    if (emplReportCategoryDuration.ContainsKey(id))
                    {
                        if (emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.BANK_HOURS) && emplReportCategoryDuration[id][(int)ReportTypeCategories.BANK_HOURS] > 0)
                            buffTO.BankHrsBalans = (decimal)emplReportCategoryDuration[id][(int)ReportTypeCategories.BANK_HOURS] / 60;
                        if (emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.STOP_WORKING) && emplReportCategoryDuration[id][(int)ReportTypeCategories.STOP_WORKING] != 0)
                            buffTO.StopWorkingHrsBalans = (decimal)emplReportCategoryDuration[id][(int)ReportTypeCategories.STOP_WORKING] / 60;
                        if (emplReportCategoryDuration[id].ContainsKey((int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID) && emplReportCategoryDuration[id][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID] > 0)
                            buffTO.NotJustifiedOvertime = emplReportCategoryDuration[id][(int)ReportTypeCategories.OVERTIME_PREV_MONTH_PAID];
                    }

                    buffers.Add(buffTO);
                }
                
                string reportName = "PYReport_" + calcID.ToString().Trim() + "_" + DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss");

                // save buffers to DB
                bool buffSaved = true;
                if (buffers.Count > 0)
                {
                    EmployeePYDataBuffer buff = new EmployeePYDataBuffer();
                    if (buff.BeginTransaction())
                    {
                        try
                        {
                            foreach (EmployeePYDataBufferTO buffTO in buffers)
                            {
                                buff.EmplBuffTO = buffTO;
                                if (buff.Save(false) < 1)
                                {
                                    buffSaved = false;
                                    break;
                                }
                            }

                            if (buffSaved)
                                buff.CommitTransaction();
                            else if (buff.GetTransaction() != null)
                                buff.RollbackTransaction();
                        }
                        catch (Exception ex)
                        {
                            if (buff.GetTransaction() != null)
                                buff.RollbackTransaction();

                            log.writeLog(DateTime.Now + " PMCPYReport.generateReport(): Saving buffers failed with exception: " + ex.Message + "\n");
                            buffSaved = false;
                        }
                    }
                }

                if (!buffSaved)
                {
                    MessageBox.Show(rm.GetString("generateReportFailed", culture));
                    return;
                }

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
                log.writeLog(DateTime.Now + " PMCPYReport.generateReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnRecalculate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (nudCalcID.Value <= 0)
                {
                    MessageBox.Show(rm.GetString("enterCalcID", culture));
                }
                else
                {
                    List<EmployeePYDataBufferTO> list = new EmployeePYDataBuffer().getEmployeeBuffers(Convert.ToUInt32(nudCalcID.Value));

                    string employeeIDString = "";

                    foreach (EmployeePYDataBufferTO buff in list)
                    {
                        employeeIDString += buff.EmployeeID.ToString() + ",";
                    }
                    if (employeeIDString.Length > 0)
                        employeeIDString = employeeIDString.Substring(0, employeeIDString.Length - 1);

                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounterValues = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
                    emplCounterValues = new EmployeeCounterValue().SearchValues(employeeIDString);
                    EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();
                    EmployeeCounterValue counter = new EmployeeCounterValue();
                    bool trans = counterHist.BeginTransaction();
                    string modUser = Constants.payRollUser;
                    DateTime modTime = DateTime.Now;
                    if (trans)
                    {
                        try
                        {
                            counter.SetTransaction(counterHist.GetTransaction());
                            bool saved = true;
                            // move to hist table
                            foreach (EmployeePYDataBufferTO buff in list)
                            {
                                // overtime hours which are paid from previous month
                                if (emplCounterValues.ContainsKey(buff.EmployeeID) && emplCounterValues[buff.EmployeeID].ContainsKey((int)Constants.EmplCounterTypes.NotJustifiedOvertime))                                    
                                {
                                    counterHist.ValueTO = new EmployeeCounterValueHistTO(emplCounterValues[buff.EmployeeID][(int)Constants.EmplCounterTypes.NotJustifiedOvertime]);
                                    counterHist.ValueTO.ModifiedBy = modUser;
                                    counterHist.ValueTO.ModifiedTime = modTime;
                                    saved = saved && (counterHist.Save(false) >= 0);

                                    if (!saved)
                                        break;

                                    counter.ValueTO = new EmployeeCounterValueTO();
                                    counter.ValueTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.NotJustifiedOvertime;
                                    counter.ValueTO.EmplID = buff.EmployeeID;
                                    counter.ValueTO.Value = buff.NotJustifiedOvertime;
                                    counter.ValueTO.ModifiedBy = modUser;
                                    counter.ValueTO.ModifiedTime = modTime;

                                    saved = saved && counter.Update(false);
                                    
                                    if (!saved)
                                        break;
                                }

                                // overtime hours which are transfered to next month
                                if (emplCounterValues.ContainsKey(buff.EmployeeID) && emplCounterValues[buff.EmployeeID].ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))                                    
                                {
                                    counterHist.ValueTO = new EmployeeCounterValueHistTO(emplCounterValues[buff.EmployeeID][(int)Constants.EmplCounterTypes.BankHoursCounter]);
                                    counterHist.ValueTO.ModifiedBy = modUser;
                                    counterHist.ValueTO.ModifiedTime = modTime;
                                    saved = saved && (counterHist.Save(false) >= 0);

                                    if (!saved)
                                        break;

                                    counter.ValueTO = new EmployeeCounterValueTO();
                                    counter.ValueTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.BankHoursCounter;
                                    counter.ValueTO.EmplID = buff.EmployeeID;
                                    counter.ValueTO.Value = (int)(buff.BankHrsBalans * 60);
                                    counter.ValueTO.ModifiedBy = modUser;
                                    counter.ValueTO.ModifiedTime = modTime;

                                    saved = saved && counter.Update(false);

                                    if (!saved)
                                        break;
                                }

                                // stop working hours which are transfered to next month
                                if (emplCounterValues.ContainsKey(buff.EmployeeID) && emplCounterValues[buff.EmployeeID].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))                                    
                                {
                                    counterHist.ValueTO = new EmployeeCounterValueHistTO(emplCounterValues[buff.EmployeeID][(int)Constants.EmplCounterTypes.StopWorkingCounter]);
                                    counterHist.ValueTO.ModifiedBy = modUser;
                                    counterHist.ValueTO.ModifiedTime = modTime;
                                    saved = saved && (counterHist.Save(false) >= 0);

                                    if (!saved)
                                        break;

                                    counter.ValueTO = new EmployeeCounterValueTO();
                                    counter.ValueTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.StopWorkingCounter;
                                    counter.ValueTO.EmplID = buff.EmployeeID;
                                    counter.ValueTO.Value = emplCounterValues[buff.EmployeeID][(int)Constants.EmplCounterTypes.StopWorkingCounter].Value + (int)(buff.StopWorkingHrsBalans * 60);
                                    counter.ValueTO.ModifiedBy = modUser;
                                    counter.ValueTO.ModifiedTime = modTime;

                                    saved = saved && counter.Update(false);
                                    
                                    if (!saved)
                                        break;
                                }
                            }
                            if (!saved)
                            {
                                if (counterHist.GetTransaction() != null)
                                    counterHist.RollbackTransaction();
                                MessageBox.Show(rm.GetString("CountersUpdateFaild", culture));
                            }
                            else
                            {
                                counterHist.CommitTransaction();
                                MessageBox.Show(rm.GetString("CountersUpdateSucc", culture));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (counterHist.GetTransaction() != null)
                                counterHist.RollbackTransaction();
                            throw new Exception(".recalculate counters() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("CountersUpdateFaild", culture));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PMCPYReport.btnRecalculate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnGenerateAgency_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvEmployees.Items.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                DateTime from = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
                DateTime to = from.AddMonths(1).AddDays(-1).Date;

                string emplIDs = "";
                Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
                Dictionary<int, Dictionary<int, EmployeeTO>> wuEmplDictTemp = new Dictionary<int, Dictionary<int, EmployeeTO>>();
                Dictionary<int, Dictionary<int, EmployeeTO>> wuEmplDict = new Dictionary<int, Dictionary<int, EmployeeTO>>();

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

                if (lvEmployees.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvEmployees.SelectedItems)
                    {
                        if (!selectedTypes.Contains(((EmployeeTO)item.Tag).EmployeeTypeID) || !AgencyCategories.Contains(((EmployeeTO)item.Tag).EmployeeTypeID))                        
                            continue;

                        emplIDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";

                        if (!emplDict.ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                            emplDict.Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);

                        if (!wuEmplDictTemp.ContainsKey(((EmployeeTO)item.Tag).WorkingUnitID))
                            wuEmplDictTemp.Add(((EmployeeTO)item.Tag).WorkingUnitID, new Dictionary<int, EmployeeTO>());

                        if (!wuEmplDictTemp[((EmployeeTO)item.Tag).WorkingUnitID].ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                            wuEmplDictTemp[((EmployeeTO)item.Tag).WorkingUnitID].Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        if (!selectedTypes.Contains(((EmployeeTO)item.Tag).EmployeeTypeID) || !AgencyCategories.Contains(((EmployeeTO)item.Tag).EmployeeTypeID))
                            continue;

                        emplIDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";

                        if (!emplDict.ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                            emplDict.Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);

                        if (!wuEmplDictTemp.ContainsKey(((EmployeeTO)item.Tag).WorkingUnitID))
                            wuEmplDictTemp.Add(((EmployeeTO)item.Tag).WorkingUnitID, new Dictionary<int, EmployeeTO>());

                        if (!wuEmplDictTemp[((EmployeeTO)item.Tag).WorkingUnitID].ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                            wuEmplDictTemp[((EmployeeTO)item.Tag).WorkingUnitID].Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);
                    }
                }

                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                else
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }

                // get asco data
                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary(emplIDs);

                // sort data for report by working unit name
                foreach (int id in wuDict.Keys)
                {
                    if (wuEmplDictTemp.ContainsKey(id) && !wuEmplDict.ContainsKey(id))
                        wuEmplDict.Add(id, wuEmplDictTemp[id]);
                }

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

                // analitical dictionary
                Dictionary<int, Dictionary<DateTime, int>> emplWorkingHoursDict = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> emplNightHoursDict = new Dictionary<int, Dictionary<DateTime, int>>();

                Dictionary<int, Dictionary<string, RuleTO>> emplRules = new Dictionary<int, Dictionary<string, RuleTO>>();

                // create list of pairs foreach employee and each day
                foreach (IOPairProcessedTO pair in allPairs)
                {
                    if (!emplDayPairs.ContainsKey(pair.EmployeeID))
                        emplDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                    if (!emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        emplDayPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());
                    emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);

                    if (!emplRules.ContainsKey(pair.EmployeeID))
                    {
                        if (emplDict.ContainsKey(pair.EmployeeID))
                        {
                            int company = Common.Misc.getRootWorkingUnit(emplDict[pair.EmployeeID].WorkingUnitID, wuDict);

                            if (rules.ContainsKey(company) && rules[company].ContainsKey(emplDict[pair.EmployeeID].EmployeeTypeID))
                                emplRules.Add(pair.EmployeeID, rules[company][emplDict[pair.EmployeeID].EmployeeTypeID]);
                        }
                    }
                }

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
                        if (emplDaySchemas.ContainsKey(pair.EmployeeID) && emplDaySchemas[pair.EmployeeID].ContainsKey(pairDate.Date))
                            schema = emplDaySchemas[pair.EmployeeID][pairDate.Date];

                        // calculate code durations and add it to dictionary
                        int pairDuration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                        if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                            pairDuration++;

                        if (isAgencyPresence(pair.PassTypeID, rulesForEmpl) || pair.PassTypeID == Constants.overtimeUnjustified || Constants.PMCSickLeaves100Types().Contains(pair.PassTypeID))
                        {
                            if (!emplWorkingHoursDict.ContainsKey(pair.EmployeeID))
                                emplWorkingHoursDict.Add(pair.EmployeeID, new Dictionary<DateTime, int>());

                            if (!emplWorkingHoursDict[pair.EmployeeID].ContainsKey(pairDate.Date))
                                emplWorkingHoursDict[pair.EmployeeID].Add(pairDate.Date, 0);

                            emplWorkingHoursDict[pair.EmployeeID][pairDate.Date] += pairDuration;
                        }

                        if (isPresence(pair.PassTypeID, rulesForEmpl) || pair.PassTypeID == Constants.overtimeUnjustified)
                        {
                            int nightWork = nightWorkDuration(rulesForEmpl, pair.IOPairDate, pair.StartTime, pair.EndTime, empl);

                            if (nightWork > 0)
                            {
                                if (!emplNightHoursDict.ContainsKey(pair.EmployeeID))
                                    emplNightHoursDict.Add(pair.EmployeeID, new Dictionary<DateTime, int>());

                                if (!emplNightHoursDict[pair.EmployeeID].ContainsKey(pairDate.Date))
                                    emplNightHoursDict[pair.EmployeeID].Add(pairDate.Date, 0);

                                emplNightHoursDict[pair.EmployeeID][pairDate.Date] += nightWork;
                            }
                        }
                    }
                }

                // generate xls report
                generateAgencyReport(wuEmplDict, ascoDict, from.Date, emplWorkingHoursDict, emplNightHoursDict, emplRules);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PMCPYReport.btnGenerateAgency_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void generateAgencyReport(Dictionary<int, Dictionary<int, EmployeeTO>> wuEmplDict, Dictionary<int, EmployeeAsco4TO> ascoDict, DateTime month,
            Dictionary<int, Dictionary<DateTime, int>> emplWorkingHoursDict, Dictionary<int, Dictionary<DateTime, int>> emplNightWorkDict, Dictionary<int, Dictionary<string, RuleTO>> emplRules)
        {
            try
            {
                if (wuEmplDict.Count <= 0)
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

                int rowNum = 2;
                // insert report name
                ws.Cells[rowNum, 1] = rm.GetString("PMCAgencyMonthlyReport", culture).ToUpper() + " " + getMonthYear(month) + " " + rm.GetString("year", culture).ToUpper();
                setCellFontWeight(ws.Cells[rowNum, 1], true);

                rowNum += 2;
                
                int colNum = 1;
                foreach (int wuID in wuEmplDict.Keys)
                {
                    // insert wu header
                    ws.Cells[rowNum, 1] = rm.GetString("wu", culture).ToUpper();
                    setCellTextStyle(ws.Cells[rowNum, 1]);
                    ws.Cells[rowNum + 1, 1] = wuID.ToString().Trim();
                    ws.Cells[rowNum, 2] = rm.GetString("wuName", culture).ToUpper();
                    if (wuDict.ContainsKey(wuID))
                        ws.Cells[rowNum + 1, 2] = wuDict[wuID].Name.Trim();
                    else
                        ws.Cells[rowNum + 1, 2] = "N/A";

                    setCellBorder(ws.Cells[rowNum, 1], true, true, true, true);
                    setCellBorder(ws.Cells[rowNum, 2], true, true, true, true);
                    setCellBorder(ws.Cells[rowNum + 1, 1], true, true, true, true);
                    setCellBorder(ws.Cells[rowNum + 1, 2], true, true, true, true);

                    // insert empl header
                    ws.Cells[rowNum + 3, 1] = rm.GetString("emplName", culture).ToUpper();
                    ws.Cells[rowNum + 3, 2] = rm.GetString("JMBG", culture).ToUpper();

                    DateTime firstMonthDay = new DateTime(month.Year, month.Month, 1).Date;
                    DateTime lastMonthDay = firstMonthDay.AddMonths(1).AddDays(-1).Date;

                    colNum = 3;
                    Dictionary<DateTime, int> colDict = new Dictionary<DateTime, int>();
                    for (DateTime currDate = firstMonthDay.Date; currDate.Date <= lastMonthDay.Date; currDate = currDate.AddDays(1).Date)
                    {
                        colNum++;

                        if (!colDict.ContainsKey(currDate.Date))
                            colDict.Add(currDate.Date, colNum);

                        ws.Cells[rowNum + 3, colNum] = currDate.Day.ToString().Trim();
                    }

                    setRowFontWeight(ws, rowNum, colNum, true);
                    setRowFontWeight(ws, rowNum + 3, colNum, true);

                    rowNum += 4;
                    foreach (int emplID in wuEmplDict[wuID].Keys)
                    {
                        int rounding = 1;
                        if (emplRules.ContainsKey(emplID) && emplRules[emplID].ContainsKey(Constants.RuleOvertimeRounding))
                            rounding = emplRules[emplID][Constants.RuleOvertimeRounding].RuleValue;

                        ws.Cells[rowNum, 1] = wuEmplDict[wuID][emplID].LastName.Trim() + " " + wuEmplDict[wuID][emplID].FirstName.Trim();
                        setCellTextStyle(ws.Cells[rowNum, 2]);
                        if (ascoDict.ContainsKey(emplID) && ascoDict[emplID].NVarcharValue4.Trim() != "")
                            ws.Cells[rowNum, 2] = ascoDict[emplID].NVarcharValue4.Trim();
                        else
                            ws.Cells[rowNum, 2] = "N/A";                       

                        ws.Cells[rowNum, 3] = rm.GetString("workingHours", culture);
                        ws.Cells[rowNum + 1, 3] = rm.GetString("nightHours", culture);

                        setCellFontWeight(ws.Cells[rowNum, 3], true);
                        setCellFontWeight(ws.Cells[rowNum + 1, 3], true);

                        for (DateTime currDate = firstMonthDay.Date; currDate.Date <= lastMonthDay.Date; currDate = currDate.AddDays(1).Date)
                        {
                            if (colDict.ContainsKey(currDate.Date))
                            {
                                int workingMinutes = 0;
                                int nightWorkMinutes = 0;
                                
                                if (emplWorkingHoursDict.ContainsKey(emplID) && emplWorkingHoursDict[emplID].ContainsKey(currDate.Date))                                
                                    workingMinutes = emplWorkingHoursDict[emplID][currDate.Date];
                                
                                if (emplNightWorkDict.ContainsKey(emplID) && emplNightWorkDict[emplID].ContainsKey(currDate.Date))
                                    nightWorkMinutes = emplNightWorkDict[emplID][currDate.Date];

                                if (workingMinutes % rounding != 0)
                                    workingMinutes -= workingMinutes % rounding;

                                if (nightWorkMinutes % rounding != 0)
                                    nightWorkMinutes -= nightWorkMinutes % rounding;
                                
                                ws.Cells[rowNum, colDict[currDate.Date]] = (workingMinutes / 60.0).ToString(Constants.doubleFormat);
                                ws.Cells[rowNum + 1, colDict[currDate.Date]] = (nightWorkMinutes / 60.0).ToString(Constants.doubleFormat);                                
                            }
                        }

                        rowNum += 3;
                    }

                    rowNum++;
                }

                // merge and center first row
                Microsoft.Office.Interop.Excel.Range range = xla.get_Range(ws.Cells[2, 1], ws.Cells[2, colNum]);
                range.Merge(Type.Missing);
                range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                // insert signiture lines
                ws.Cells[rowNum, 1] = rm.GetString("responsiblePerson", culture).ToUpper() + ": ";
                ws.Cells[rowNum + 2, 1] = rm.GetString("responsiblePersonSigniture", culture).ToUpper() + ": ";
                setRowFontWeight(ws, rowNum, colNum, true);
                setRowFontWeight(ws, rowNum + 2, colNum, true);
                               
                setCellBorder(ws.Cells[rowNum, 2], false, true, false, false);
                setCellBorder(ws.Cells[rowNum + 2, 2], false, true, false, false);
                setCellBorder(ws.Cells[rowNum, 3], false, true, false, false);
                setCellBorder(ws.Cells[rowNum + 2, 3], false, true, false, false);
                
                ws.Columns.AutoFit();
                ws.Rows.AutoFit();

                string reportName = "AgencyPYReport_" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss");

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
                log.writeLog(DateTime.Now + " PMCPYReport.generateAgencyReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void generateAnaliticalReport(Dictionary<int, Dictionary<int, EmployeeTO>> wuEmplDict, Dictionary<int, EmployeeAsco4TO> ascoDict, DateTime month,
            Dictionary<int, Dictionary<DateTime, Dictionary<string, int>>> emplHoursDict, Dictionary<int, Dictionary<DateTime, int>> emplNightWorkDict,
            Dictionary<int, Dictionary<DateTime, int>> emplOvertimeHoursDict, Dictionary<int, Dictionary<string, RuleTO>> emplRules)
        {
            try
            {
                if (wuEmplDict.Count <= 0)
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

                int rowNum = 2;
                // insert report name
                ws.Cells[rowNum, 1] = rm.GetString("PMCMonthlyReport", culture).ToUpper() + " " + getMonthYear(month) + " " + rm.GetString("year", culture).ToUpper();
                setCellFontWeight(ws.Cells[rowNum, 1], true);

                rowNum += 2;

                int colNum = 1;
                foreach (int wuID in wuEmplDict.Keys)
                {
                    // insert wu header
                    ws.Cells[rowNum, 1] = rm.GetString("wu", culture).ToUpper();
                    setCellTextStyle(ws.Cells[rowNum, 1]);
                    ws.Cells[rowNum + 1, 1] = wuID.ToString().Trim();
                    ws.Cells[rowNum, 2] = rm.GetString("wuName", culture).ToUpper();
                    if (wuDict.ContainsKey(wuID))
                        ws.Cells[rowNum + 1, 2] = wuDict[wuID].Name.Trim();
                    else
                        ws.Cells[rowNum + 1, 2] = "N/A";

                    setCellBorder(ws.Cells[rowNum, 1], true, true, true, true);
                    setCellBorder(ws.Cells[rowNum, 2], true, true, true, true);
                    setCellBorder(ws.Cells[rowNum + 1, 1], true, true, true, true);
                    setCellBorder(ws.Cells[rowNum + 2, 2], true, true, true, true);

                    // insert empl header
                    ws.Cells[rowNum + 3, 1] = rm.GetString("emplName", culture).ToUpper();
                    ws.Cells[rowNum + 3, 2] = rm.GetString("JMBG", culture).ToUpper();

                    DateTime firstMonthDay = new DateTime(month.Year, month.Month, 1).Date;
                    DateTime lastMonthDay = firstMonthDay.AddMonths(1).AddDays(-1).Date;

                    colNum = 3;
                    Dictionary<DateTime, int> colDict = new Dictionary<DateTime, int>();
                    for (DateTime currDate = firstMonthDay.Date; currDate.Date <= lastMonthDay.Date; currDate = currDate.AddDays(1).Date)
                    {
                        colNum++;

                        if (!colDict.ContainsKey(currDate.Date))
                            colDict.Add(currDate.Date, colNum);

                        ws.Cells[rowNum + 3, colNum] = currDate.Day.ToString().Trim();

                        if (currDate.DayOfWeek == DayOfWeek.Saturday || currDate.DayOfWeek == DayOfWeek.Sunday)
                            setCellTextColor(ws.Cells[rowNum + 3, colNum], Color.Red);
                    }

                    setRowFontWeight(ws, rowNum, colNum, true);
                    setRowFontWeight(ws, rowNum + 3, colNum, true);

                    rowNum += 4;
                    foreach (int emplID in wuEmplDict[wuID].Keys)
                    {
                        int rounding = 1;
                        if (emplRules.ContainsKey(emplID) && emplRules[emplID].ContainsKey(Constants.RuleOvertimeRounding))
                            rounding = emplRules[emplID][Constants.RuleOvertimeRounding].RuleValue;

                        ws.Cells[rowNum, 1] = wuEmplDict[wuID][emplID].LastName.Trim() + " " + wuEmplDict[wuID][emplID].FirstName.Trim();
                        setCellTextStyle(ws.Cells[rowNum, 2]);
                        if (ascoDict.ContainsKey(emplID) && ascoDict[emplID].NVarcharValue4.Trim() != "")
                            ws.Cells[rowNum, 2] = ascoDict[emplID].NVarcharValue4.Trim();
                        else
                            ws.Cells[rowNum, 2] = "N/A";

                        ws.Cells[rowNum, 3] = rm.GetString("regWorkHours", culture);
                        ws.Cells[rowNum + 1, 3] = rm.GetString("nightHours", culture);
                        ws.Cells[rowNum + 2, 3] = rm.GetString("overtimeHours", culture);
                        ws.Cells[rowNum + 3, 3] = rm.GetString("unjustifiedHours", culture);

                        setCellFontWeight(ws.Cells[rowNum, 3], true);
                        setCellFontWeight(ws.Cells[rowNum + 1, 3], true);
                        setCellFontWeight(ws.Cells[rowNum + 2, 3], true);
                        setCellFontWeight(ws.Cells[rowNum + 3, 3], true);

                        for (DateTime currDate = firstMonthDay.Date; currDate.Date <= lastMonthDay.Date; currDate = currDate.AddDays(1).Date)
                        {
                            if (colDict.ContainsKey(currDate.Date))
                            {
                                int workingMinutes = 0;
                                int nightWorkMinutes = 0;
                                int overtimeMinutes = 0;
                                int unpaidMinutes = 0;

                                string codeDesc = "";
                                if (emplHoursDict.ContainsKey(emplID) && emplHoursDict[emplID].ContainsKey(currDate.Date))
                                {
                                    foreach (string code in emplHoursDict[emplID][currDate.Date].Keys)
                                    {
                                        if (code == ReportTypeCodes.RW.ToString())
                                            workingMinutes = emplHoursDict[emplID][currDate.Date][code];
                                        else if (code == ReportTypeCodes.UNPAID.ToString())
                                            unpaidMinutes = emplHoursDict[emplID][currDate.Date][code];
                                        else codeDesc += code.Trim() + "+";
                                    }

                                    if (codeDesc.Length > 0)
                                        codeDesc = codeDesc.Substring(0, codeDesc.Length - 1);
                                }

                                if (emplNightWorkDict.ContainsKey(emplID) && emplNightWorkDict[emplID].ContainsKey(currDate.Date))
                                    nightWorkMinutes = emplNightWorkDict[emplID][currDate.Date];

                                if (emplOvertimeHoursDict.ContainsKey(emplID) && emplOvertimeHoursDict[emplID].ContainsKey(currDate.Date))
                                    overtimeMinutes = emplOvertimeHoursDict[emplID][currDate.Date];
                                
                                if (nightWorkMinutes % rounding != 0)
                                    nightWorkMinutes -= nightWorkMinutes % rounding;

                                string workingValue = "";

                                if (workingMinutes != 0)
                                {
                                    workingValue += (workingMinutes / 60.0).ToString(Constants.doubleFormat);
                                    if (codeDesc.Trim() != "")
                                        codeDesc = "+" + codeDesc.Trim();
                                }

                                if (codeDesc.Trim() != "")
                                    workingValue += codeDesc.Trim();

                                if (workingValue.Trim() == "")
                                    workingValue = "0";

                                ws.Cells[rowNum, colDict[currDate.Date]] = workingValue.Trim();
                                ws.Cells[rowNum + 1, colDict[currDate.Date]] = (nightWorkMinutes / 60.0).ToString(Constants.doubleFormat);
                                ws.Cells[rowNum + 2, colDict[currDate.Date]] = (overtimeMinutes / 60.0).ToString(Constants.doubleFormat);
                                ws.Cells[rowNum + 3, colDict[currDate.Date]] = (unpaidMinutes / 60.0).ToString(Constants.doubleFormat);

                                if (currDate.DayOfWeek == DayOfWeek.Saturday || currDate.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    setCellTextColor(ws.Cells[rowNum, colDict[currDate.Date]], Color.Red);
                                    setCellTextColor(ws.Cells[rowNum + 1, colDict[currDate.Date]], Color.Red);
                                    setCellTextColor(ws.Cells[rowNum + 2, colDict[currDate.Date]], Color.Red);
                                    setCellTextColor(ws.Cells[rowNum + 3, colDict[currDate.Date]], Color.Red);
                                }
                            }
                        }

                        rowNum += 5;
                    }

                    rowNum++;
                }

                // merge and center first row
                Microsoft.Office.Interop.Excel.Range range = xla.get_Range(ws.Cells[2, 1], ws.Cells[2, colNum]);
                range.Merge(Type.Missing);
                range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                // insert signiture lines
                ws.Cells[rowNum, 1] = rm.GetString("responsiblePerson", culture).ToUpper() + ": ";
                ws.Cells[rowNum + 2, 1] = rm.GetString("responsiblePersonSigniture", culture).ToUpper() + ": ";
                setRowFontWeight(ws, rowNum, colNum, true);
                setRowFontWeight(ws, rowNum + 2, colNum, true);

                setCellBorder(ws.Cells[rowNum, 2], false, true, false, false);
                setCellBorder(ws.Cells[rowNum + 2, 2], false, true, false, false);
                setCellBorder(ws.Cells[rowNum, 3], false, true, false, false);
                setCellBorder(ws.Cells[rowNum + 2, 3], false, true, false, false);

                ws.Columns.AutoFit();
                ws.Rows.AutoFit();

                string reportName = "DailyReport_" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss");

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
                log.writeLog(DateTime.Now + " PMCPYReport.generateAnaliticalReport(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.setRowFontWeight(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.setCellFontWeight(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void setCellBorder(object cell, bool top, bool bottom, bool left, bool right)
        {
            try
            {
                Microsoft.Office.Interop.Excel.XlLineStyle bordreLine = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                Microsoft.Office.Interop.Excel.XlLineStyle bordreNone = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;

                Microsoft.Office.Interop.Excel.Range range = ((Microsoft.Office.Interop.Excel.Range)cell);
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = left ? bordreLine : bordreNone;
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = right ? bordreLine : bordreNone;
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = top ? bordreLine : bordreNone;
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = bottom ? bordreLine : bordreNone;
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.setCellBorder(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void setCellTextStyle(object cell)
        {
            try
            {
                ((Microsoft.Office.Interop.Excel.Range)cell).NumberFormat = "@";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.setCellTextStyle(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void setCellTextColor(object cell, Color color)
        {
            try
            {
                ((Microsoft.Office.Interop.Excel.Range)cell).Font.Color = System.Drawing.ColorTranslator.ToOle(color);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.setCellTextStyle(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCPYReport.releaseObject(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        private string getMonthYear(DateTime month)
        {
            try
            {
                string monthString = "";

                switch (month.Month)
                {
                    case 1:
                        monthString = "I";
                        break;
                    case 2:
                        monthString = "II";
                        break;
                    case 3:
                        monthString = "III";
                        break;
                    case 4:
                        monthString = "IV";
                        break;
                    case 5:
                        monthString = "V";
                        break;
                    case 6:
                        monthString = "VI";
                        break;
                    case 7:
                        monthString = "VII";
                        break;
                    case 8:
                        monthString = "VIII";
                        break;
                    case 9:
                        monthString = "IX";
                        break;
                    case 10:
                        monthString = "X";
                        break;
                    case 11:
                        monthString = "XI";
                        break;
                    case 12:
                        monthString = "XII";
                        break;
                }

                monthString += " " + month.Year.ToString().Trim() + ".";

                return monthString;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PMCPYReport.getMonthYear(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isAgencyPresence(int ptID, Dictionary<string, RuleTO> emplRules)
        {
            try
            {
                bool presencePair = false;

                List<string> presenceTypes = Constants.PMCAgencyEffectiveWorkWageTypes();

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
                log.writeLog(DateTime.Now + " PMCPYReport.isAgencyPresence(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
