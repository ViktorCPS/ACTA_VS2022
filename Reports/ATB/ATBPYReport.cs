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

namespace Reports.ATB
{
    public partial class ATBPYReport : Form
    {
        private const string delimiter = ";";

        private const int emplIDIndex = 0;
        private const int emplNameIndex = 1;

        private int emplSortField;

        private ListViewItemComparer _comp;

        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        //wUnits are acctualy functional structure
        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private string wuString = "";
        private List<int> wuList = new List<int>();

        //oUnits are organizational units
        private Dictionary<int, OrganizationalUnitTO> oUnits = new Dictionary<int, OrganizationalUnitTO>();
        private string ouString = "";
        private List<int> ouList = new List<int>();

        Dictionary<int, WorkingUnitTO> wuDict = new Dictionary<int, WorkingUnitTO>();
        Dictionary<int, OrganizationalUnitTO> ouDict = new Dictionary<int, OrganizationalUnitTO>();
        Dictionary<int, PassTypeTO> ptDict = new Dictionary<int, PassTypeTO>();
        Dictionary<int, Dictionary<int, string>> emplTypesDict = new Dictionary<int, Dictionary<int, string>>();
        Dictionary<int, WorkTimeSchemaTO> schemas = new Dictionary<int, WorkTimeSchemaTO>();
        Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rules = new Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>>();


        private const int unpaidLeavePT = 41;
        private const int sickWithoutPaymentPT = 91;
        private const int tissueAndOrgansDonationPT = 94;
        private const int paidLeaveDueToALackOfWorkPT = 22;
        private const int paidLeaveDueToALackOfWork65PercPT = 58;
        private const int bloodDonation = 11;
        private const int specialCareOfChildPT = 62;
        private const int careOfChildUnder3YearsPT = 63;

        //FOR REPORT
        private const int sickLeavesUntil30DaysPT = 101;
        private const int sickLeavesOver30DaysPT = 102;
        private const int maternityLeavesPT = 103;
        private const int workInjuryPT = 104;
        private const int paidLeavePT = 105;

        //OMLADINCI
        private const int omladinci = 998;

        List<int> unpaidLeaves = new List<int>(); //neplaćena odsustva
        List<int> paidLeaves = new List<int>();//plaćena odsustva
        List<int> paidLeaves1 = new List<int>();//plaćena odsustva 60%
        List<int> sickLeavesWithoutPayment = new List<int>();  // bolovanje bez nadoknade
        List<int> sickLeavesUntil30Days = new List<int>(); //bolovanje 30 dana
        List<int> sickLeavesOver30Days = new List<int>(); // bolovanje preko 30 dana
        List<int> industrialInjury = new List<int>(); //povreda na radu
        List<int> pregnancyLeaves = new List<int>(); // porodiljsko

        public ATBPYReport()
        {
            try
            {
                InitializeComponent();

                // Debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                // Set Language
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(ATBPYReport).Assembly);
                rbWU.Checked = true;

                // set date interval
                DateTime lastMonth = DateTime.Now.AddMonths(-1);

                dtpFrom.Value = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);

                setLanguage();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyatPYReport.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void setLanguage()
        {
            //  this.Text = rm.GetString("HyattPYReport", culture);

            //label's text
            this.lblEmployee.Text = rm.GetString("lblEmployee", culture);
            this.lblFrom.Text = rm.GetString("lblFrom", culture);
            this.lblTo.Text = rm.GetString("lblTo", culture);

            //button's text
            this.btnClose.Text = rm.GetString("btnClose", culture);
            this.btnGenerate.Text = rm.GetString("btnGenerate", culture);

            //group box text
            this.gbDateInterval.Text = rm.GetString("gbDateInterval", culture);

            /// list view                
            lvEmployees.BeginUpdate();
            lvEmployees.Columns.Add(rm.GetString("hdrID", culture), 75, HorizontalAlignment.Left);
            lvEmployees.Columns.Add(rm.GetString("hdrName", culture), 240, HorizontalAlignment.Left);
            lvEmployees.EndUpdate();

            //lvDates.BeginUpdate();
            //lvDates.Columns.Add(rm.GetString("hdrCalcID", culture), 100, HorizontalAlignment.Left);
            //lvDates.Columns.Add(rm.GetString("hdrCreatedTime", culture), 200, HorizontalAlignment.Left);
            //lvDates.EndUpdate();
        }

        #region Inner Class for sorting List of Employees
        /*
		 *  Class used for sorting items in the List View 
		*/
        private class ListViewItemComparer : System.Collections.IComparer
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
                    case ATBPYReport.emplIDIndex:
                        {
                            int id1 = -1;
                            int id2 = -1;

                            int.TryParse(sub1.Text, out id1);
                            int.TryParse(sub2.Text, out id2);

                            return CaseInsensitiveComparer.Default.Compare(id1, id2);
                        }
                    case ATBPYReport.emplNameIndex:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                    default:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                }
            }
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tbEmployees_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string emplName = tbEmployee.Text.Trim().ToUpper();
                int emplID = -1;
                if (!int.TryParse(emplName, out emplID))
                    emplID = -1;

                lvEmployees.SelectedItems.Clear();

                if (emplName.Trim().Equals(""))
                {
                    tbEmployee.Focus();
                    return;
                }

                foreach (ListViewItem item in lvEmployees.Items)
                {
                    if ((emplID != -1 && ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim().ToUpper().StartsWith(emplID.ToString().Trim().ToUpper()))
                        || (emplID == -1 && ((EmployeeTO)item.Tag).FirstAndLastName.Trim().ToUpper().StartsWith(emplName.Trim().ToUpper())))
                    {
                        item.Selected = true;
                        lvEmployees.Select();
                        lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));
                        lvEmployees.Invalidate();
                        tbEmployee.Focus();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyatPYReport.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void HyattPYReport_Load(object sender, EventArgs e)
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

                sickLeavesUntil30Days = Constants.HyattSickLeavesUntil30DaysTypes();
                sickLeavesOver30Days = Constants.HyattSickLeavesOver30DaysTypes();
                sickLeavesWithoutPayment = Constants.HyattSickLeaveWithoutPaymentTypes();
                industrialInjury = Constants.HyattIndustrialInjuryTypes();
                pregnancyLeaves = Constants.HyattPregnancyLeaveTypes();

                unpaidLeaves.Add(Constants.unpaidLeave);

                // get all paid leave
                Dictionary<int, List<int>> confirmationPTDict = new PassTypesConfirmation().SearchDictionary();
                List<int> sickLeaveNCF = new Common.Rule().SearchRulesExact(Constants.RuleCompanySickLeaveNCF);

                foreach (int ptID in confirmationPTDict.Keys)
                {
                    if (!confirmationPTDict[ptID].Contains(Constants.unpaidLeave) && !sickLeaveNCF.Contains(ptID))
                        paidLeaves.AddRange(confirmationPTDict[ptID]);
                }

                paidLeaves.Add(paidLeaveDueToALackOfWorkPT);
                paidLeaves1.Add(paidLeaveDueToALackOfWork65PercPT);

                //populateDateListView();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyatPYReport.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
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

                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary("");

                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();

                foreach (EmployeeTO empl in employeeList)
                {
                    if (ascoDict.ContainsKey(empl.EmployeeID))
                    {
                        EmployeeAsco4TO eAsco = ascoDict[empl.EmployeeID];
                        if (!eAsco.NVarcharValue4.Equals(""))
                        {
                            ListViewItem item = new ListViewItem();
                            item.Text = empl.EmployeeID.ToString().Trim();
                            item.SubItems.Add(empl.FirstAndLastName);
                            item.ToolTipText = empl.FirstAndLastName;

                            //if (empl.EmployeeTypeID == (int)Constants.EmployeeTypesWN.MANAGEMENT)
                            //    empl.EmployeeTypeID = (int)Constants.EmployeeTypesWN.ADMINISTRATION;

                            item.Tag = empl;
                            lvEmployees.Items.Add(item);
                        }
                    }
                }

                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyatPYReport.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        //private void populateDateListView()
        //{
        //    try
        //    {
        //        List<EmployeePYDataSumTO> list = new EmployeePYDataSum().getSumDates(dtpFrom.Value.Date, dtpTo.Value.Date);

        //        lvDates.BeginUpdate();
        //        lvDates.Items.Clear();

        //        foreach (EmployeePYDataSumTO sumTO in list)
        //        {
        //            ListViewItem item = new ListViewItem();
        //            item.Text = sumTO.PYCalcID.ToString().Trim();
        //            item.SubItems.Add(sumTO.CreatedTime.ToString(Constants.dateFormat + " " + Constants.timeFormat));

        //            item.Tag = sumTO.PYCalcID;
        //            lvDates.Items.Add(item);
        //        }

        //        lvDates.EndUpdate();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyatPYReport.populateDateListView(): " + ex.Message + "\n");
        //        MessageBox.Show(ex.Message);
        //    }
        //}

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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyatPYReport.populateWU(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattPYReport.populateOU(): " + ex.Message + "\n");
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

                if (dtpFrom.Value.Date > dtpTo.Value.Date)
                {
                    MessageBox.Show(rm.GetString("invalidDateInterval", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                DateTime from = dtpFrom.Value.Date;
                DateTime to = dtpTo.Value.Date;


                #region  get selected employees, and create dictionary (EmployeeID, EmployeeTO)
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
                #endregion

                #region get pairs for one more day because of third shifts
                List<DateTime> dateList = new List<DateTime>();
                DateTime currentDate = from.Date;
                while (currentDate.Date <= to.AddDays(1).Date)
                {
                    dateList.Add(currentDate.Date);
                    currentDate = currentDate.AddDays(1);
                }
                List<IOPairProcessedTO> allPairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, dateList, ""); //take a list of all io pairs that are processed
                #endregion

                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplBelongingDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                Dictionary<int, List<DateTime>> emplUnjustifiedDays = new Dictionary<int, List<DateTime>>();
                Dictionary<int, Dictionary<DateTime, int>> employeePresence = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, List<DateTime>> employeeDayMeals = new Dictionary<int, List<DateTime>>();
                Dictionary<int, int> employeeMeals = new Dictionary<int, int>();

                #region  create list of pairs foreach employee and each day
                string unregularData = "";
                List<int> unregularIDs = new List<int>();
                foreach (IOPairProcessedTO pair in allPairs)
                {
                    if (!emplDayPairs.ContainsKey(pair.EmployeeID)) //set emplID
                        emplDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                    if (!emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date)) //set date for employe
                        emplDayPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());
                    emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair); //add pair
                }
                #endregion

                // get asco data
                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary(emplIDs);

                #region get national and personal holidays

                List<DateTime> nationalHolidaysDays = new List<DateTime>();
                Dictionary<string, List<DateTime>> personalHolidayDays = new Dictionary<string, List<DateTime>>();
                List<DateTime> nationalHolidaysSundays = new List<DateTime>();
                List<HolidaysExtendedTO> nationalTransferableHolidays = new List<HolidaysExtendedTO>();

                Common.Misc.getHolidays(from.Date, to.Date, personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, null, nationalTransferableHolidays);
                #endregion

                #region get schemas and intervals, and than get schedules for selected employees and date interval
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
                #endregion

                Dictionary<int, Dictionary<string, RuleTO>> emplRules = new Dictionary<int, Dictionary<string, RuleTO>>();
                Dictionary<int, List<DateTime>> emplHolidayPaidDays = new Dictionary<int, List<DateTime>>();

                List<int> sickLeaveNCF = new List<int>();
                List<int> paidPT = new List<int>();
                List<int> paidLeavePT = new List<int>();
                List<int> paidLeavePT1 = new List<int>();

                #region create rules for each employee
                foreach (int emplID in emplDict.Keys)
                {
                    if (!emplRules.ContainsKey(emplID))
                    {
                        int company = Common.Misc.getRootWorkingUnit(emplDict[emplID].WorkingUnitID, wuDict); //get company

                        if (rules.ContainsKey(company) && rules[company].ContainsKey(emplDict[emplID].EmployeeTypeID))
                            emplRules.Add(emplID, rules[company][emplDict[emplID].EmployeeTypeID]);

                        if (rules[company][emplDict[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanySickLeaveNCF)
                                && !sickLeaveNCF.Contains(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanySickLeaveNCF].RuleValue))
                            sickLeaveNCF.Add(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanySickLeaveNCF].RuleValue);

                        //this one is used later
                        if (rules[company][emplDict[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanyPeriodicalMedicalCheckUp)
                            && !paidLeavePT.Contains(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanyPeriodicalMedicalCheckUp].RuleValue))
                            paidLeavePT.Add(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanyPeriodicalMedicalCheckUp].RuleValue);

                        #region add to the dictionary paidLeaves created in Load method
                        if (rules[company][emplDict[emplID].EmployeeTypeID].ContainsKey(Constants.RuleCompanyPeriodicalMedicalCheckUp)
                            && !paidLeavePT.Contains(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanyPeriodicalMedicalCheckUp].RuleValue))
                            paidLeaves.Add(rules[company][emplDict[emplID].EmployeeTypeID][Constants.RuleCompanyPeriodicalMedicalCheckUp].RuleValue);
                        #endregion

                        #region paid pass types
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
                        #endregion
                    }
                }
                #endregion

                #region get sick leave pass types and paid leave pass types(they have sick leave pass types codes)
                Dictionary<int, List<int>> confirmationPTDict = new PassTypesConfirmation().SearchDictionary();
                List<int> sickLeavePT = new List<int>();
                foreach (int ptID in sickLeaveNCF)
                {
                    if (confirmationPTDict.ContainsKey(ptID))
                        sickLeavePT.AddRange(confirmationPTDict[ptID]);
                }
                foreach (int ptID in confirmationPTDict.Keys)
                {
                    //   if (!confirmationPTDict[ptID].Contains(Constants.unpaidLeave) && !sickLeaveNCF.Contains(ptID))
                    if (ptID != unpaidLeavePT)
                        paidLeavePT.AddRange(confirmationPTDict[ptID]);
                    paidLeavePT1.AddRange(confirmationPTDict[ptID]);
                }

                paidLeavePT.Add(paidLeaveDueToALackOfWorkPT);
                paidLeavePT1.Add(paidLeaveDueToALackOfWork65PercPT);


                #endregion

                //// analytical dictionaries
                Dictionary<int, Dictionary<DateTime, int>> emplWorkOnSundays = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> emplWorkOnSuterdays = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> emplWorkOnSundaysBH = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> emplWorkOnSuterdaysBH = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> emplSickLeaves = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> emplPaidLeaves = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, int>> emplUnpaid = new Dictionary<int, Dictionary<DateTime, int>>();
                Dictionary<int, Dictionary<DateTime, Dictionary<int, int>>> emplPTDuration = new Dictionary<int, Dictionary<DateTime, Dictionary<int, int>>>();

                // summary dictionaries
                Dictionary<int, int> emplWorkOnSundaysSummary = new Dictionary<int, int>();
                Dictionary<int, int> emplWorkOnSuterdaysSummary = new Dictionary<int, int>();
                Dictionary<int, int> emplWorkOnSundaysBHSummary = new Dictionary<int, int>();
                Dictionary<int, int> emplWorkOnSuterdaysBHSummary = new Dictionary<int, int>();
                Dictionary<int, int> emplSickLeavesSummary = new Dictionary<int, int>();
                Dictionary<int, int> emplPaidLeavesSummary = new Dictionary<int, int>();
                Dictionary<int, int> emplPaidLeavesSummary60 = new Dictionary<int, int>();
                Dictionary<int, int> emplUnpaidSummary = new Dictionary<int, int>();
                Dictionary<int, Dictionary<int, int>> emplPTDurationSummary = new Dictionary<int, Dictionary<int, int>>();

                #region create emplBelongingDayPairs(EmployeeID,Dictionary<DateTime, List<IOProcessed>>) and emplUnjustifiedDays(EmployeeID,List<DateTime>)
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
                #endregion

                if (unregularIDs.Count > 0)
                {
                    MessageBox.Show(rm.GetString("notConfirmedDataFound", culture) + " " + unregularData.Trim().Substring(0, unregularData.Trim().Length - 1) + ".");
                    return;
                }

                #region create list of pairs foreach employee and each pair belonging day
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

                            #region for analytical report
                            if (!emplPTDuration.ContainsKey(pair.EmployeeID))
                                emplPTDuration.Add(pair.EmployeeID, new Dictionary<DateTime, Dictionary<int, int>>());
                            if (!emplPTDuration[pair.EmployeeID].ContainsKey(date.Date))
                                emplPTDuration[pair.EmployeeID].Add(date.Date, new Dictionary<int, int>());
                            #endregion

                            if (!emplPTDurationSummary.ContainsKey(pair.EmployeeID))
                                emplPTDurationSummary.Add(pair.EmployeeID, new Dictionary<int, int>());

                            if (isSundayWork(pair, date, rulesForEmpl, emplDict, schema))
                            {
                                #region for analytical report
                                if (!emplWorkOnSundays.ContainsKey(pair.EmployeeID))
                                    emplWorkOnSundays.Add(pair.EmployeeID, new Dictionary<DateTime, int>());

                                if (!emplWorkOnSundays[pair.EmployeeID].ContainsKey(date.Date))
                                    emplWorkOnSundays[pair.EmployeeID].Add(date.Date, 0);

                                emplWorkOnSundays[pair.EmployeeID][date.Date] += pairDuration;
                                #endregion

                                if (!emplWorkOnSundaysSummary.ContainsKey(pair.EmployeeID))
                                    emplWorkOnSundaysSummary.Add(pair.EmployeeID, 0);

                                emplWorkOnSundaysSummary[pair.EmployeeID] += pairDuration;
                            }
                            else if (isSaturdayWork(pair, date, rulesForEmpl, emplDict, schema))
                            {
                                #region for analytical report
                                if (!emplWorkOnSuterdays.ContainsKey(pair.EmployeeID))
                                    emplWorkOnSuterdays.Add(pair.EmployeeID, new Dictionary<DateTime, int>());

                                if (!emplWorkOnSuterdays[pair.EmployeeID].ContainsKey(date.Date))
                                    emplWorkOnSuterdays[pair.EmployeeID].Add(date.Date, 0);

                                emplWorkOnSuterdays[pair.EmployeeID][date.Date] += pairDuration;
                                #endregion

                                if (!emplWorkOnSuterdaysSummary.ContainsKey(pair.EmployeeID))
                                    emplWorkOnSuterdaysSummary.Add(pair.EmployeeID, 0);

                                emplWorkOnSuterdaysSummary[pair.EmployeeID] += pairDuration;
                            }
                            else if (isSundayBHWork(pair, date, rulesForEmpl, emplDict, schema))
                            {
                                #region for analytical report
                                if (!emplWorkOnSundaysBH.ContainsKey(pair.EmployeeID))
                                    emplWorkOnSundaysBH.Add(pair.EmployeeID, new Dictionary<DateTime, int>());

                                if (!emplWorkOnSundaysBH[pair.EmployeeID].ContainsKey(date.Date))
                                    emplWorkOnSundaysBH[pair.EmployeeID].Add(date.Date, 0);

                                emplWorkOnSundaysBH[pair.EmployeeID][date.Date] += pairDuration;
                                #endregion

                                if (!emplWorkOnSundaysBHSummary.ContainsKey(pair.EmployeeID))
                                    emplWorkOnSundaysBHSummary.Add(pair.EmployeeID, 0);

                                emplWorkOnSundaysBHSummary[pair.EmployeeID] += pairDuration;
                            }
                            else if (isSaturdayBHWork(pair, date, rulesForEmpl, emplDict, schema))
                            {
                                #region for analytical report
                                if (!emplWorkOnSuterdaysBH.ContainsKey(pair.EmployeeID))
                                    emplWorkOnSuterdaysBH.Add(pair.EmployeeID, new Dictionary<DateTime, int>());

                                if (!emplWorkOnSuterdaysBH[pair.EmployeeID].ContainsKey(date.Date))
                                    emplWorkOnSuterdaysBH[pair.EmployeeID].Add(date.Date, 0);

                                emplWorkOnSuterdaysBH[pair.EmployeeID][date.Date] += pairDuration;
                                #endregion

                                if (!emplWorkOnSuterdaysBHSummary.ContainsKey(pair.EmployeeID))
                                    emplWorkOnSuterdaysBHSummary.Add(pair.EmployeeID, 0);

                                emplWorkOnSuterdaysBHSummary[pair.EmployeeID] += pairDuration;
                            }
                            else
                            {
                                #region for analytical report
                                if (!emplPTDuration[pair.EmployeeID][date.Date].ContainsKey(pair.PassTypeID))
                                    emplPTDuration[pair.EmployeeID][date.Date].Add(pair.PassTypeID, 0);

                                emplPTDuration[pair.EmployeeID][date.Date][pair.PassTypeID] += pairDuration;
                                #endregion

                                if (!emplPTDurationSummary[pair.EmployeeID].ContainsKey(pair.PassTypeID))
                                    emplPTDurationSummary[pair.EmployeeID].Add(pair.PassTypeID, 0);

                                emplPTDurationSummary[pair.EmployeeID][pair.PassTypeID] += pairDuration;

                                if (oldPairPT == deleyPT)
                                {
                                    #region for analytical report
                                    if (!emplPTDuration[pair.EmployeeID][date.Date].ContainsKey(deleyPT))
                                        emplPTDuration[pair.EmployeeID][date.Date].Add(deleyPT, 0);

                                    emplPTDuration[pair.EmployeeID][date.Date][deleyPT] += pairDuration;
                                    #endregion

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
                                    #region for analytical report
                                    if (!emplPTDuration[pair.EmployeeID][date.Date].ContainsKey(workOnHolidayPT))
                                        emplPTDuration[pair.EmployeeID][date.Date].Add(workOnHolidayPT, 0);

                                    emplPTDuration[pair.EmployeeID][date.Date][workOnHolidayPT] += pairDuration;
                                    #endregion

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
                                    #region for analytical report
                                    if (!emplPTDuration[pair.EmployeeID][date.Date].ContainsKey(turnusPT))
                                        emplPTDuration[pair.EmployeeID][date.Date].Add(turnusPT, 0);

                                    emplPTDuration[pair.EmployeeID][date.Date][turnusPT] += pairDuration;
                                    #endregion

                                    if (!emplPTDurationSummary[pair.EmployeeID].ContainsKey(turnusPT))
                                        emplPTDurationSummary[pair.EmployeeID].Add(turnusPT, 0);

                                    emplPTDurationSummary[pair.EmployeeID][turnusPT] += pairDuration;
                                }

                                int nightWork = nightWorkDuration(rulesForEmpl, pair, schema);

                                if (nightWorkPT != -1 && nightWork > 0)
                                {
                                    #region for analytical report
                                    if (!emplPTDuration[pair.EmployeeID][date.Date].ContainsKey(nightWorkPT))
                                        emplPTDuration[pair.EmployeeID][date.Date].Add(nightWorkPT, 0);

                                    emplPTDuration[pair.EmployeeID][date.Date][nightWorkPT] += nightWork;
                                    #endregion

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
                #endregion

                #region create summary data, and sick leaves, paid leaves and unpaid leaves
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
                            else if (paidLeavePT1.Contains(ptID))
                            {
                                if (!emplPaidLeavesSummary60.ContainsKey(emplID))
                                    emplPaidLeavesSummary60.Add(emplID, 0);
                                emplPaidLeavesSummary60[emplID] += duration;

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
                #endregion

                #region populate meals dictionaries
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
                #endregion


                generateEmployeesReport(emplDict, ascoDict, emplRules, emplPTDurationSummary, emplWorkOnSundaysSummary, emplWorkOnSuterdaysSummary, emplWorkOnSundaysBHSummary, emplWorkOnSuterdaysBHSummary,
                emplSickLeavesSummary, emplPaidLeavesSummary, emplPaidLeavesSummary60, emplUnpaidSummary, emplWorkingNormSummary, sickLeaveNCF, employeeMeals);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattPYReport.rbWU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }

        }

        private void generateEmployeesReport(Dictionary<int, EmployeeTO> emplDict, Dictionary<int, EmployeeAsco4TO> ascoDict, Dictionary<int, Dictionary<string, RuleTO>> emplRules,
            Dictionary<int, Dictionary<int, int>> emplPTDurationSummary, Dictionary<int, int> emplWorkOnSundaysSummary, Dictionary<int, int> emplWorkOnSaturdaysSummary,
            Dictionary<int, int> emplWorkOnSundaysBHSummary, Dictionary<int, int> emplWorkOnSaturdaysBHSummary,
            Dictionary<int, int> emplSickLeavesSummary, Dictionary<int, int> emplPaidLeavesSummary, Dictionary<int, int> emplPaidLeavesSummary60, Dictionary<int, int> emplUnpaidSummary,
            Dictionary<int, int> emplWorkingNormSummary, List<int> sickLeavePT, Dictionary<int, int> emplMeals)
        {

            try
            {
                Dictionary<int, PassTypeTO> passTypeDic = new PassType().SearchDictionary();

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
                string header = rm.GetString("JMBG", culture) + delimiter
                    + rm.GetString("regWorkHours", culture) + delimiter
                    + "Prekovremeni do 2h" + delimiter + "Prekovremeni preko 2h" + delimiter
                    + rm.GetString("hdrOfficialTrip", culture) + delimiter
                    + "Rad u smeni II" + delimiter
                    + rm.GetString("workOnSunday", culture) + delimiter
                    + rm.GetString("hdrNightWork", culture) + delimiter
                    + rm.GetString("hdrWorkOnHoliday", culture) + delimiter
                    + rm.GetString("ptWorkOnHoliday", culture) + delimiter
                    + rm.GetString("hdrAnnualLeave", culture) + delimiter
                    + rm.GetString("hdrSickLeaveUnder30Days", culture) + delimiter
                    + rm.GetString("hdrSickLeave100", culture) + delimiter
                    + "Bolovanje fond" + delimiter + "Porođajno do 3 deteta" + delimiter
                    + "Porođajno preko 3 deteta" + delimiter
                    + rm.GetString("hdrUnpaidLeave", culture) + delimiter
                    + rm.GetString("hdrPaidLeave", culture) + delimiter
                    + "Plaćeno odsustvo 60%";

                List<string> lines = new List<string>();

                // create file lines
                foreach (int id in emplDict.Keys)
                {
                    EmployeeAsco4TO dataAboutEmpl = ascoDict[id];

                    string line1 = id.ToString();

                    Dictionary<int, int> emplPTDuration = new Dictionary<int, int>();
                    if (emplPTDurationSummary.ContainsKey(id))
                        emplPTDuration = emplPTDurationSummary[id];

                    //foreach (int pt in sickLeavePT) //BOLOVANJE
                    //{
                    //    line = jmbg + delimiter + pt + delimiter + getMinutes(emplPTDuration, pt);
                    //    lines.Add(line);
                    //}

                    #region REDOVAN RAD --> 1
                    foreach (int pt in regularWork)
                    {
                        int minutes = getMinutes(emplPTDuration, pt);
                        if (minutes > 0)
                        {

                            string paymentCodeForPT = "";
                            if (passTypeDic.ContainsKey(pt))
                            {
                                PassTypeTO type = passTypeDic[pt];
                                paymentCodeForPT = type.PaymentCode;
                            }

                            if (minutes < 600)
                            {

                                line1 += "00" + getHours(minutes, true);
                            }
                            else if (minutes < 6000)
                            {
                                line1 += "0" + getHours(minutes, true);
                            }
                            else
                            {
                                line1 += getHours(minutes, true);
                            }

                        }
                        else
                        {
                            line1 += "0000";
                        }

                    }

                    #endregion

                    #region PREKOVREMENO --> 4
                    foreach (int pt in overtimePaid)
                    {
                        int minutes = getMinutes(emplPTDuration, pt);
                        if (minutes > 0)
                        {
                            string paymentCodeForPT = "";
                            if (passTypeDic.ContainsKey(pt))
                            {
                                PassTypeTO type = passTypeDic[pt];
                                paymentCodeForPT = type.PaymentCode;
                            }
                            if (minutes < 600)
                            {

                                line1 += "00" + getHours(minutes, true);
                            }
                            else if (minutes < 6000)
                            {
                                line1 += "0" + getHours(minutes, true);
                            }
                            else
                            {
                                line1 += getHours(minutes, true);
                            }
                        }

                        else
                        {
                            line1 += "0000";
                        }

                    }
                    #endregion

                    #region PREKOVREMENO DOPUNA --> 4
                    foreach (int pt in overtimePaid)
                    {

                        line1 += "0000";
                    }
                    #endregion


                    #region SLUZBENI PUT --> 2
                    foreach (int pt in officialTrip)
                    {
                        int minutes = getMinutes(emplPTDuration, pt);
                        if (minutes > 0)
                        {
                            string paymentCodeForPT = "";
                            if (passTypeDic.ContainsKey(pt))
                            {
                                PassTypeTO type = passTypeDic[pt];
                                paymentCodeForPT = type.PaymentCode;

                            }
                            if (minutes < 600)
                            {

                                line1 += "00" + getHours(minutes, true);
                            }
                            else if (minutes < 6000)
                            {
                                line1 += "0" + getHours(minutes, true);
                            }
                            else
                            {
                                line1 += getHours(minutes, true);
                            }
                        }
                        else
                        {
                            line1 += "0000";
                        }

                    }
                    #endregion

                    #region RAD U SMENI II


                    line1 += "0000";

                    #endregion

                    #region RAD NEDELJOM
                    int workOnSundayMinutes = getMinutes(emplWorkOnSundaysSummary, id);
                    if (workOnSundayMinutes > 0)
                    {
                        if (workOnSundayMinutes < 600)
                        {

                            line1 += "0" + getHours(workOnSundayMinutes, true);
                        }
                        else
                        {
                            line1 += getHours(workOnSundayMinutes, true);
                        }
                    }
                    else
                    {
                        line1 += "000";
                    }
                    #endregion

                    #region NOCNI RAD --> 6
                    foreach (int pt in nightWork)
                    {
                        int minutes = getMinutes(emplPTDuration, pt);

                        if (minutes > 0)
                        {
                            string paymentCodeForPT = "";
                            if (passTypeDic.ContainsKey(pt))
                            {
                                PassTypeTO type = passTypeDic[pt];
                                paymentCodeForPT = type.PaymentCode;
                            }
                            if (minutes < 600)
                            {

                                line1 += "00" + getHours(minutes, true);
                            }
                            else if (minutes < 6000)
                            {
                                line1 += "0" + getHours(minutes, true);
                            }
                            else
                            {
                                line1 += getHours(minutes, true);
                            }

                            // lines.Add(line);
                        }
                        else
                        {

                            line1 += "0000";

                        }


                    }
                    #endregion

                    #region RAD NA PRAZNIK -->5
                    foreach (int pt in workOnHolidays)
                    {
                        int minutes = getMinutes(emplPTDuration, pt);
                        if (minutes > 0)
                        {
                            string paymentCodeForPT = "";
                            if (passTypeDic.ContainsKey(pt))
                            {
                                PassTypeTO type = passTypeDic[pt];
                                paymentCodeForPT = type.PaymentCode;
                            }
                            if (minutes < 600)
                            {

                                line1 += "0" + getHours(minutes, true).Remove(2);
                            }
                            else
                            {
                                line1 += getHours(minutes, true).Remove(2);
                            }

                            // lines.Add(line);
                        }
                        else
                        {

                            line1 += "00";

                        }



                    }
                    #endregion

                    #region LICNI PRAZNICI --> 12
                    foreach (int pt in personalHolidays)
                    {
                        int minutes = getMinutes(emplPTDuration, pt);
                        if (minutes > 0)
                        {
                            string paymentCodeForPT = "";
                            if (passTypeDic.ContainsKey(pt))
                            {
                                PassTypeTO type = passTypeDic[pt];
                                paymentCodeForPT = type.PaymentCode;
                            }
                            if (minutes < 600)
                            {

                                line1 += "0" + getHours(minutes, true);
                            }
                            else
                            {
                                line1 += getHours(minutes, true);
                            }


                            // lines.Add(line);
                        }
                        else
                        {

                            line1 += "000";

                        }


                    }
                    #endregion


                    #region GODISNJI ODMOR --> 8
                    foreach (int pt in annualLeaves)
                    {
                        int minutes = getMinutes(emplPTDuration, pt);
                        if (minutes > 0)
                        {
                            string paymentCodeForPT = "";
                            if (passTypeDic.ContainsKey(pt))
                            {
                                PassTypeTO type = passTypeDic[pt];
                                paymentCodeForPT = type.PaymentCode;
                            }


                            if (minutes < 600)
                            {

                                line1 += "00" + getHours(minutes, true).Remove(1);
                            }
                            else if (minutes < 6000)
                            {
                                line1 += "0" + getHours(minutes, true).Remove(2);
                            }
                            else
                            {
                                line1 += getHours(minutes, true).Remove(3);
                            }


                        }
                        else
                        {
                            line1 += "000";
                        }
                    }
                    #endregion

                    #region BOLOVANJE DO 30 DANA --> 101
                    int sickLeavesUntil30DaysMinutes = 0;
                    foreach (int pt in sickLeavesUntil30Days)
                    {
                        //int pomocna = sickLeavesUntil30DaysMinutes;
                        //int pomocna1 = 0;
                        sickLeavesUntil30DaysMinutes += getMinutes(emplPTDuration, pt);
                        //  pomocna1 = pomocna + sickLeavesUntil30DaysMinutes;
                        if (sickLeavesUntil30DaysMinutes > 0)
                        {
                            string paymentCodeForPT = "";
                            if (passTypeDic.ContainsKey(sickLeavesUntil30DaysPT))
                            {
                                PassTypeTO type = passTypeDic[sickLeavesUntil30DaysPT];
                                paymentCodeForPT = type.PaymentCode;
                            }
                            //if (pt == 25)
                            //{

                            //    pomocna += sickLeavesUntil30DaysMinutes;
                            //}
                            //else
                            //{

                            if (sickLeavesUntil30DaysMinutes < 600)
                            {

                                line1 += "00" + getHours(sickLeavesUntil30DaysMinutes, true);
                            }
                            else if (sickLeavesUntil30DaysMinutes < 6000)
                            {

                                line1 += "0" + getHours(sickLeavesUntil30DaysMinutes, true);
                            }
                            else
                            {

                                line1 += getHours(sickLeavesUntil30DaysMinutes, true);
                            }


                            //}
                        }
                        else
                        {
                            line1 += "0000";
                        }
                    }
                    #endregion

                    #region POVREDA NA RADU --> 104
                    int workInjuryMinutes = 0;
                    foreach (int pt in industrialInjury)
                    {
                        workInjuryMinutes += getMinutes(emplPTDuration, pt);

                        if (workInjuryMinutes > 0)
                        {


                            string paymentCodeForPT = "";
                            if (passTypeDic.ContainsKey(workInjuryPT))
                            {
                                PassTypeTO type = passTypeDic[workInjuryPT];
                                paymentCodeForPT = type.PaymentCode;
                            }

                            if (workInjuryMinutes < 600)
                            {

                                line1 += "00" + getHours(workInjuryMinutes, true);
                            }
                            else if (workInjuryMinutes < 6000)
                            {

                                line1 += "0" + getHours(workInjuryMinutes, true);
                            }
                            else
                            {

                                line1 += getHours(workInjuryMinutes, true);
                            }

                        }
                        else
                        {
                            line1 += "0000";
                        }

                    }
                    #endregion

                    #region BOLOVANJE PREKO 30 DANA --> 102
                    int sickLeavesOver30DaysMinutes = 0;
                    foreach (int pt in sickLeavesOver30Days)
                    {
                        sickLeavesOver30DaysMinutes += getMinutes(emplPTDuration, pt);

                        if (sickLeavesOver30DaysMinutes > 0)
                        {
                            string paymentCodeForPT = "";
                            if (passTypeDic.ContainsKey(sickLeavesOver30DaysPT))
                            {
                                PassTypeTO type = passTypeDic[sickLeavesOver30DaysPT];
                                paymentCodeForPT = type.PaymentCode;
                            }
                            if (sickLeavesOver30DaysMinutes < 600)
                            {

                                line1 += "00" + getHours(sickLeavesOver30DaysMinutes, true);
                            }
                            else if (sickLeavesOver30DaysMinutes < 6000)
                            {

                                line1 += "0" + getHours(sickLeavesOver30DaysMinutes, true);
                            }
                            else
                            {

                                line1 += getHours(sickLeavesOver30DaysMinutes, true);
                            }



                        }
                        else
                        {
                            line1 += "0000";
                        }

                    }
                    #endregion

                    //#region BOLOVANJE SUMA 
                    //int suma = workInjuryMinutes + sickLeavesUntil30DaysMinutes;
                    //if (suma > 0)
                    //{
                    //    if (suma < 600)
                    //    {

                    //        line1 += "00" + getHours(suma, true);
                    //    }
                    //    else if (suma < 6000)
                    //    {

                    //        line1 += "0" + getHours(suma, true);
                    //    }
                    //    else
                    //    {

                    //        line1 += getHours(workInjuryMinutes, true);
                    //    }

                    //}
                    //else
                    //{
                    //    line1 += "0000";
                    //}


                    //#endregion


                    #region PORODJAJNO DO 3 DETETA


                    line1 += "0000";

                    #endregion

                    #region PORODJAJNO PREKO 3 DETETA


                    line1 += "0000";

                    #endregion

                    #region NEPLACENA ODSUSTVA --> 41
                    int unpaidLeavesMinutes = getMinutes(emplUnpaidSummary, id);
                    if (unpaidLeavesMinutes > 0)
                    {
                        int pt = unpaidLeavePT;
                        string paymentCodeForPT = "";
                        if (passTypeDic.ContainsKey(pt))
                        {
                            PassTypeTO type = passTypeDic[pt];
                            paymentCodeForPT = type.PaymentCode;
                        }
                        if (unpaidLeavesMinutes < 600)
                        {

                            line1 += "00" + getHours(unpaidLeavesMinutes, true);
                        }
                        else if (unpaidLeavesMinutes < 6000)
                        {

                            line1 += "0" + getHours(unpaidLeavesMinutes, true);
                        }
                        else
                        {

                            line1 += getHours(unpaidLeavesMinutes, true);
                        }

                    }
                    else
                    {
                        line1 += "0000";
                    }

                    #endregion

                    #region PLACENA ODSUSTVA --> 22

                    int paidLeavesMinutes = getMinutes(emplPaidLeavesSummary, id);
                    paidLeavesMinutes += getMinutes(emplPTDuration, bloodDonation);
                    paidLeavesMinutes += getMinutes(emplPTDuration, specialCareOfChildPT);
                    paidLeavesMinutes += getMinutes(emplPTDuration, careOfChildUnder3YearsPT);

                    if (paidLeavesMinutes > 0)
                    {
                        int pt = paidLeavePT;
                        string paymentCodeForPT = "";
                        if (passTypeDic.ContainsKey(pt))
                        {
                            PassTypeTO type = passTypeDic[pt];
                            paymentCodeForPT = type.PaymentCode;
                        }
                        if (paidLeavesMinutes < 600)
                        {

                            line1 += "00" + getHours(paidLeavesMinutes, true);
                        }
                        else if (paidLeavesMinutes < 6000)
                        {

                            line1 += "0" + getHours(paidLeavesMinutes, true);
                        }
                        else
                        {

                            line1 += getHours(paidLeavesMinutes, true);
                        }

                    }
                    else
                    {
                        line1 += "0000";
                    }

                    #endregion

                    #region PLACENA ODSUSTVA 60% --> 58

                    int paidLeavesMinutes60 = getMinutes(emplPaidLeavesSummary60, id);

                    if (paidLeavesMinutes60 > 0)
                    {
                        int pt = paidLeavePT;
                        string paymentCodeForPT = "";
                        if (passTypeDic.ContainsKey(pt))
                        {
                            PassTypeTO type = passTypeDic[pt];
                            paymentCodeForPT = type.PaymentCode;
                        }
                        if (paidLeavesMinutes60 < 600)
                        {

                            line1 += "00" + getHours(paidLeavesMinutes60, true);
                        }
                        else if (paidLeavesMinutes60 < 6000)
                        {

                            line1 += "0" + getHours(paidLeavesMinutes60, true);
                        }
                        else
                        {

                            line1 += getHours(paidLeavesMinutes60, true);
                        }

                    }
                    else
                    {
                        line1 += "0000";
                    }



                    #endregion



                    //#region RAD SUBOTOM
                    ////int workOnSuterdayMinutes = getMinutes(emplWorkOnSaturdaysSummary, id);
                    ////if(workOnSuterdayMinutes > 0)
                    ////{
                    ////    line = jmbg + delimiter + "rad subotom" + delimiter + getHours(workOnSuterdayMinutes, true);
                    ////    lines.Add(line);
                    ////}
                    //#endregion


                    //#region SMENSKI RAD --> 7
                    //foreach (int pt in turnus)
                    //{
                    //    int minutes = getMinutes(emplPTDuration, pt);
                    //    if (minutes > 0)
                    //    {
                    //        string paymentCodeForPT = "";
                    //        if (passTypeDic.ContainsKey(pt))
                    //        {
                    //            PassTypeTO type = passTypeDic[pt];
                    //            paymentCodeForPT = type.PaymentCode;
                    //        }
                    //        line1 += getHours(minutes, true);

                    //    }
                    //}

                    //#endregion

                    //    #region PREKOVREMENO PREKO 2h --> 4
                    //    foreach (int pt in overtimePaid)
                    //    {

                    //            line1 += "00000";


                    //    }
                    //    #endregion

                    //    #region RAD NEDELJOM
                    //    int workOnSundayMinutes = getMinutes(emplWorkOnSundaysSummary, id);
                    //    if (workOnSundayMinutes > 0)
                    //    {
                    //        if (workOnSundayMinutes < 600)
                    //        {

                    //            line1 += "00" + getHours(workOnSundayMinutes, true);
                    //        }
                    //        else
                    //        {
                    //            line1 += "0" + getHours(workOnSundayMinutes, true);
                    //        }
                    //    }

                    //    #endregion
                    //    #region RAD NA PRAZNIK -->5
                    //    foreach (int pt in workOnHolidays)
                    //    {
                    //        int minutes = getMinutes(emplPTDuration, pt);
                    //        if (minutes > 0)
                    //        {
                    //            string paymentCodeForPT = "";
                    //            if (passTypeDic.ContainsKey(pt))
                    //            {
                    //                PassTypeTO type = passTypeDic[pt];
                    //                paymentCodeForPT = type.PaymentCode;
                    //            }
                    //            if (minutes < 600)
                    //            {

                    //                line1 += "00" + getHours(minutes, true);
                    //            }
                    //            else 
                    //            {
                    //                line1 += "0" + getHours(minutes, true);
                    //            }


                    //        }
                    //        else
                    //        {
                    //            line1 += "0000";
                    //        }

                    //    }
                    //    #endregion




                    //#region NEOPRAVDANO PREKOVREMENO --> 60
                    //foreach (int pt in overtimeRejected)
                    //{
                    //    int minutes = getMinutes(emplPTDuration, pt);
                    //    if (minutes > 0)
                    //    {
                    //        string paymentCodeForPT = "";
                    //        if (passTypeDic.ContainsKey(pt))
                    //        {
                    //            PassTypeTO type = passTypeDic[pt];
                    //            paymentCodeForPT = type.PaymentCode;
                    //        }
                    //        line1 += getHours(minutes, true);

                    //    }
                    //}
                    //#endregion

                    //#region KASNJENJA --> 59
                    //foreach (int pt in delays)
                    //{
                    //    int minutes = getMinutes(emplPTDuration, pt);
                    //    if (minutes > 0)
                    //    {
                    //        string paymentCodeForPT = "";
                    //        if (passTypeDic.ContainsKey(pt))
                    //        {
                    //            PassTypeTO type = passTypeDic[pt];
                    //            paymentCodeForPT = type.PaymentCode;
                    //        }
                    //        line1 += getHours(minutes, true);

                    //    }
                    //}
                    //#endregion

                    //#region GODISNJI ODMOR --> 8
                    //foreach (int pt in annualLeaves)
                    //{
                    //    int minutes = getMinutes(emplPTDuration, pt);
                    //    if (minutes > 0)
                    //    {
                    //        string paymentCodeForPT = "";
                    //        if (passTypeDic.ContainsKey(pt))
                    //        {
                    //            PassTypeTO type = passTypeDic[pt];
                    //            paymentCodeForPT = type.PaymentCode;
                    //        }
                    //        line1 += getHours(minutes, true);

                    //    }
                    //}
                    //#endregion

                    //#region SLUZBENI PUT --> 2
                    //foreach (int pt in officialTrip)
                    //{
                    //    int minutes = getMinutes(emplPTDuration, pt);
                    //    if (minutes > 0)
                    //    {
                    //        string paymentCodeForPT = "";
                    //        if (passTypeDic.ContainsKey(pt))
                    //        {
                    //            PassTypeTO type = passTypeDic[pt];
                    //            paymentCodeForPT = type.PaymentCode;
                    //        }
                    //        line1 += getHours(minutes, true);

                    //    }

                    //}
                    //#endregion

                    //#region LICNI PRAZNICI --> 12
                    //foreach (int pt in personalHolidays)
                    //{
                    //    int minutes = getMinutes(emplPTDuration, pt);
                    //    if (minutes > 0)
                    //    {
                    //        string paymentCodeForPT = "";
                    //        if (passTypeDic.ContainsKey(pt))
                    //        {
                    //            PassTypeTO type = passTypeDic[pt];
                    //            paymentCodeForPT = type.PaymentCode;
                    //        }
                    //        line1 += getHours(minutes, true);

                    //    }
                    //}
                    //#endregion



                    //#region ODMOR
                    //foreach (int pt in holidays)
                    //{
                    //    int minutes = getMinutes(emplPTDuration, pt);
                    //    if (minutes > 0)
                    //    {
                    //        string paymentCodeForPT = "";
                    //        if (passTypeDic.ContainsKey(pt))
                    //        {
                    //            PassTypeTO type = passTypeDic[pt];
                    //            paymentCodeForPT = type.PaymentCode;
                    //        }
                    //        line1 += getHours(minutes, true);

                    //    }
                    //}
                    //#endregion

                    //#region BOLOVANJE DO 30 DANA --> 101
                    //int sickLeavesUntil30DaysMinutes = 0;
                    //foreach (int pt in sickLeavesUntil30Days)
                    //{
                    //    sickLeavesUntil30DaysMinutes += getMinutes(emplPTDuration, pt);
                    //}
                    //if (sickLeavesUntil30DaysMinutes > 0)
                    //{
                    //    string paymentCodeForPT = "";
                    //    if (passTypeDic.ContainsKey(sickLeavesUntil30DaysPT))
                    //    {
                    //        PassTypeTO type = passTypeDic[sickLeavesUntil30DaysPT];
                    //        paymentCodeForPT = type.PaymentCode;
                    //    }
                    //    line1 += getHours(sickLeavesUntil30DaysMinutes, true);

                    //}
                    //#endregion



                    //#region PORODILJSKO ODSUSTVO --> 103
                    //int pregnancyLeavesMinutes = 0;
                    //foreach (int pt in pregnancyLeaves)
                    //{
                    //    pregnancyLeavesMinutes += getMinutes(emplPTDuration, pt);
                    //}
                    //if (pregnancyLeavesMinutes > 0)
                    //{

                    //    string paymentCodeForPT = "";
                    //    if (passTypeDic.ContainsKey(maternityLeavesPT))
                    //    {
                    //        PassTypeTO type = passTypeDic[maternityLeavesPT];
                    //        paymentCodeForPT = type.PaymentCode;
                    //    }
                    //    line1 += getHours(pregnancyLeavesMinutes, true);

                    //}
                    //#endregion


                    //#region BOLOVANJE BEZ NADOKNADE -->91
                    //int sickLeavesWithoutPaymentMinutes = 0;
                    //foreach (int pt in sickLeavesWithoutPayment)
                    //{
                    //    sickLeavesWithoutPaymentMinutes += getMinutes(emplPTDuration, pt);
                    //}
                    //if (sickLeavesWithoutPaymentMinutes > 0)
                    //{
                    //    string paymentCodeForPT = "";
                    //    if (passTypeDic.ContainsKey(sickWithoutPaymentPT))
                    //    {
                    //        PassTypeTO type = passTypeDic[sickWithoutPaymentPT];
                    //        paymentCodeForPT = type.PaymentCode;
                    //    }
                    //    line1 += getHours(sickLeavesWithoutPaymentMinutes, true);

                    //}
                    //#endregion

                    //#region PLACENA ODSUSTVA --> 105
                    //int paidLeavesMinutes = getMinutes(emplPaidLeavesSummary, id);
                    //paidLeavesMinutes += getMinutes(emplPTDuration, specialCareOfChildPT);
                    //paidLeavesMinutes += getMinutes(emplPTDuration, careOfChildUnder3YearsPT);
                    //if (paidLeavesMinutes > 0)
                    //{
                    //    int pt = paidLeavePT;
                    //    string paymentCodeForPT = "";
                    //    if (passTypeDic.ContainsKey(pt))
                    //    {
                    //        PassTypeTO type = passTypeDic[pt];
                    //        paymentCodeForPT = type.PaymentCode;
                    //    }
                    //    line1 += getHours(paidLeavesMinutes, true);

                    //}
                    //#endregion



                    //#region ZBIRNI ZARADJENI --> 48
                    //foreach (int pt in bankHours)
                    //{
                    //    int minutes = getMinutes(emplPTDuration, pt);
                    //    if (minutes > 0)
                    //    {
                    //        string paymentCodeForPT = "";
                    //        if (passTypeDic.ContainsKey(pt))
                    //        {
                    //            PassTypeTO type = passTypeDic[pt];
                    //            paymentCodeForPT = type.PaymentCode;
                    //        }
                    //        line1 += getHours(minutes, true);

                    //    }
                    //}
                    //#endregion

                    #region obrok 1000
                    //int mealNum = 0;
                    //if (emplMeals.ContainsKey(id))
                    //    mealNum = emplMeals[id];

                    //line = jmbg + delimiter + "obrok" + delimiter + mealNum.ToString().Trim();
                    //lines.Add(line);
                    #endregion

                    lines.Add(line1);
                }

                string reportName = "ATBEmployees_PaymentReport_" + DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss");

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = reportName;
                sfd.InitialDirectory = Constants.txtDocPath;
                sfd.Filter = "TXT (*.txt)|*.txt";

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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattPYReport.rbWU_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattPYReport.rbOU_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattPYReport.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattPYReport.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();

                //populateDateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattPYReport.dtpFrom_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();

                //populateDateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattPYReport.dtpTo_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                        hours = Math.Round(((decimal)emplPTDuration[id] / 60), 2).ToString();
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
                        hours = Math.Round(((decimal)emplPTDuration[id] / 60), 2).ToString();
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
                            hours = Math.Round(((decimal)emplPTDuration[date.Date][id] / 60), 2).ToString();
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
                            hours = Math.Round(((decimal)emplPTDuration[id][date.Date] / 60), 2).ToString();
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
                if (!doRounding) hours = ((int)minutes / 60).ToString(Constants.doubleFormat);
                else if (minutes % 60 != 0 && minutes % 30 == 0)
                    hours = (minutes / 60).ToString();
                else

                    hours = Math.Round(((decimal)minutes / 60), 0).ToString();


                int test = 0;
                if (Int32.TryParse(hours, out test))
                {
                    hours += "0";
                }
                else
                {
                    hours = hours.Replace(',', '.');
                }

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

        private void lblEmployee_Click(object sender, EventArgs e)
        {

        }

        private void gbDateInterval_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }



    }
}
