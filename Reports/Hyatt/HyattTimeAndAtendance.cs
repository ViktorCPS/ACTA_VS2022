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

namespace Reports.Hyatt
{
    public partial class HyattTimeAndAtendance : Form
    {
        Dictionary<string, DateTime> dateOfTheDay;
        DateTime mondayDate = new DateTime();
        DateTime from = new DateTime();
        DateTime to  = new DateTime();
        private const string delimiter = ";";

        private const int emplIDIndex = 0;
        private const int emplNameIndex = 1;

        private int emplSortField;

        private ListViewItemComparer _comp;
        DebugLog log;
        ApplUserTO logInUser;
        private CultureInfo culture;
        private ResourceManager rm;
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
        List<int> paidLeaves = new List<int>(); //plaćena odsustva
        List<int> sickLeavesWithoutPayment = new List<int>();  // bolovanje bez nadoknade
        List<int> sickLeavesUntil30Days = new List<int>(); //bolovanje 30 dana
        List<int> sickLeavesOver30Days = new List<int>(); // bolovanje preko 30 dana
        List<int> industrialInjury = new List<int>(); //povreda na radu
        List<int> pregnancyLeaves = new List<int>(); // porodiljsko

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
                    case HyattTimeAndAtendance.emplIDIndex:
                        {
                            int id1 = -1;
                            int id2 = -1;

                            int.TryParse(sub1.Text, out id1);
                            int.TryParse(sub2.Text, out id2);

                            return CaseInsensitiveComparer.Default.Compare(id1, id2);
                        }
                    case HyattTimeAndAtendance.emplNameIndex:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                    default:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                }
            }
        }

        #endregion
        
        public HyattTimeAndAtendance()
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
                rm = new ResourceManager("Reports.ReportResource", typeof(HyattPYReport).Assembly);
                rbWU.Checked = true;

                setLanguage();
                dateOfTheDay = new Dictionary<string, DateTime>();
                setDates();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyatPYReport.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
        }
       
        private void HyattTimeAndAtendance_Load(object sender, EventArgs e)
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
                paidLeaves.Add(paidLeaveDueToALackOfWork65PercPT);

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
        
        public void setLanguage()
        {
             this.Text = rm.GetString("HyattTimeAndAttendance", culture);

            //label's text
            this.lblEmployee.Text = rm.GetString("lblEmployee", culture);

            //button's text
            this.btnClose.Text = rm.GetString("btnClose", culture);
            this.btnGenerate.Text = rm.GetString("btnGenerate", culture);

            /// list view                
            lvEmployees.BeginUpdate();
            lvEmployees.Columns.Add(rm.GetString("hdrID", culture), 80, HorizontalAlignment.Left);
            lvEmployees.Columns.Add(rm.GetString("hdrGUID", culture), 80, HorizontalAlignment.Left);
            lvEmployees.Columns.Add(rm.GetString("hdrName", culture), 180, HorizontalAlignment.Left);
            lvEmployees.EndUpdate();
        }

        private void setDates()
        {
            try
            {
                DateTime now = DateTime.Now;
                
                switch (now.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        mondayDate = now.AddDays(7);
                        break;
                    case DayOfWeek.Tuesday:
                        mondayDate = now.AddDays(6);
                        break;
                    case DayOfWeek.Wednesday:
                        mondayDate = now.AddDays(5);
                        break;
                    case DayOfWeek.Thursday:
                        mondayDate = now.AddDays(4);
                        break;
                    case DayOfWeek.Friday:
                        mondayDate = now.AddDays(3);
                        break;
                    case DayOfWeek.Saturday:
                        mondayDate = now.AddDays(2);
                        break;
                    case DayOfWeek.Sunday:
                        mondayDate = now.AddDays(1);
                        break;
                }

                DateTime date = new DateTime(mondayDate.Year, mondayDate.Month, mondayDate.Day, 0, 0, 0);
                dateOfTheDay.Add(DayOfWeek.Monday.ToString(), date);
                dateOfTheDay.Add(DayOfWeek.Tuesday.ToString(), date.AddDays(1));
                dateOfTheDay.Add(DayOfWeek.Wednesday.ToString(), date.AddDays(2));
                dateOfTheDay.Add(DayOfWeek.Thursday.ToString(), date.AddDays(3));
                dateOfTheDay.Add(DayOfWeek.Friday.ToString(), date.AddDays(4));
                dateOfTheDay.Add(DayOfWeek.Saturday.ToString(), date.AddDays(5));
                dateOfTheDay.Add(DayOfWeek.Sunday.ToString(), date.AddDays(6));

                from = date;
                to = date.AddDays(6);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.setDates(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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

                if (ID != -1)
                {
                    if (isWU)
                    {
                        string wunits = Common.Misc.getWorkingUnitHierarhicly(ID, wuList, null);

                        // get employees from selected working unit that are not currently loaned to other working unit or are currently loand to selected working unit                        
                        employeeList = new Employee().SearchByWU(wunits);
                    }
                    else
                    {
                        string ounits = Common.Misc.getOrgUnitHierarhicly(ID.ToString(), ouList, null);

                        employeeList = new Employee().SearchByOU(ounits);
                    }
                }

                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary("");

                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();

                foreach (EmployeeTO empl in employeeList)
                {

                    ListViewItem item = new ListViewItem();
                    item.Text = empl.EmployeeID.ToString().Trim();
                    if (ascoDict.ContainsKey(empl.EmployeeID) && ascoDict[empl.EmployeeID].IntegerValue1 != -1)
                    {
                        EmployeeAsco4TO eAsco = ascoDict[empl.EmployeeID];
                        item.SubItems.Add(eAsco.IntegerValue1.ToString().Trim());
                    }
                    else
                    {
                        item.SubItems.Add(" ");
                    }
                    item.SubItems.Add(empl.FirstAndLastName);
                    item.ToolTipText = empl.FirstAndLastName;

                    item.Tag = empl;
                    lvEmployees.Items.Add(item);
                }

                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattTimeAndAtendance.populateEmployees(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattTimeAndAtendance.rbWU_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattTimeAndAtendance.populateWU(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattTimeAndAtendance.populateOU(): " + ex.Message + "\n");
                throw ex;
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattTimeAndAtendance.rbOU_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattTimeAndAtendance.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattTimeAndAtendance.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void tbEmployee_TextChanged(object sender, EventArgs e)
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyatYTimeAndAtendance.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
                while (currentDate.Date <= to.Date.AddDays(1).Date)
                {
                    dateList.Add(currentDate.Date);
                    currentDate = currentDate.AddDays(1);
                }
                List<IOPairProcessedTO> allPairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, dateList, ""); 
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
                //get vacation data from counter value
                Dictionary<int, Dictionary<int, int>> counterValuesDic = new EmployeeCounterValue().Search(emplIDs);

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
                }

                paidLeavePT.Add(paidLeaveDueToALackOfWorkPT);
                paidLeavePT.Add(paidLeaveDueToALackOfWork65PercPT);


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

                generateReport(emplDict, ascoDict, emplRules, counterValuesDic, emplDayIntervals, emplDaySchemas, emplPTDuration, emplWorkOnSundays, emplWorkOnSuterdays, emplWorkOnSundaysBH, emplWorkOnSuterdaysBH, emplSickLeaves, emplPaidLeaves, emplUnpaid,
                        emplWorkingNorm, emplPTDurationSummary, emplWorkOnSundaysSummary, emplWorkOnSuterdaysSummary, emplWorkOnSundaysBHSummary, emplWorkOnSuterdaysBHSummary, emplSickLeavesSummary, emplPaidLeavesSummary, 
                        emplUnpaidSummary, emplWorkingNormSummary, sickLeavePT);
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

        private void generateReport(Dictionary<int, EmployeeTO> emplDict, Dictionary<int, EmployeeAsco4TO> ascoDict, Dictionary<int, Dictionary<string, RuleTO>> emplRules, Dictionary<int, Dictionary<int, int>> counterValuesDict,
                                    Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>> emplDayIntervalsDict , Dictionary<int, Dictionary<DateTime,WorkTimeSchemaTO>> emplDaySchemaDict,
                                    Dictionary<int, Dictionary<DateTime, Dictionary<int, int>>> emplAllPTDuration, Dictionary<int, Dictionary<DateTime, int>> emplWorkOnSundays,
                                    Dictionary<int, Dictionary<DateTime, int>> emplWorkOnSaturdays, Dictionary<int, Dictionary<DateTime, int>> emplWorkOnSundaysBH, Dictionary<int, Dictionary<DateTime, int>> emplWorkOnSaturdaysBH,
                                    Dictionary<int, Dictionary<DateTime, int>> emplSickLeaves, Dictionary<int, Dictionary<DateTime, int>> emplPaidLeaves, Dictionary<int, Dictionary<DateTime, int>> emplUnpaid,
                                    Dictionary<int, Dictionary<DateTime, int>> emplWorkingNorm, Dictionary<int, Dictionary<int, int>> emplPTDurationSummary, Dictionary<int, int> emplWorkOnSundaysSummary, Dictionary<int, int> emplWorkOnSaturdaysSummary,
                                    Dictionary<int, int> emplWorkOnSundaysBHSummary, Dictionary<int, int> emplWorkOnSaturdaysBHSummary,
                                    Dictionary<int, int> emplSickLeavesSummary, Dictionary<int, int> emplPaidLeavesSummary, Dictionary<int, int> emplUnpaidSummary,
                                    Dictionary<int, int> emplWorkingNormSummary, List<int> sickLeavePT)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;

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
                string header = "Time and Attendance Record";
                string parametersLine = "Department:" + delimiter + cbWU.Text + delimiter + delimiter + delimiter +
                                        "Week: " + delimiter + mondayDate.ToShortDateString() + " - " + mondayDate.AddDays(6).ToShortDateString();

                string days = delimiter + delimiter + delimiter + delimiter + delimiter +
                                DayOfWeek.Monday + delimiter + DayOfWeek.Tuesday + delimiter + DayOfWeek.Wednesday +
                                delimiter + DayOfWeek.Thursday + delimiter + DayOfWeek.Friday + delimiter + DayOfWeek.Saturday + delimiter + DayOfWeek.Sunday;
               
                string tableHeader = "Full Time Employee" + delimiter + "ID No" + delimiter + "OW" + delimiter + "OLD VAC" + delimiter + "VAC" + delimiter +
                                dateOfTheDay[DayOfWeek.Monday.ToString()].ToShortDateString() + delimiter + dateOfTheDay[DayOfWeek.Tuesday.ToString()].ToShortDateString() + delimiter +
                                dateOfTheDay[DayOfWeek.Wednesday.ToString()].ToShortDateString() + delimiter + dateOfTheDay[DayOfWeek.Thursday.ToString()].ToShortDateString() + delimiter +
                                dateOfTheDay[DayOfWeek.Friday.ToString()].ToShortDateString() + delimiter + dateOfTheDay[DayOfWeek.Saturday.ToString()].ToShortDateString() + delimiter +
                                dateOfTheDay[DayOfWeek.Sunday.ToString()].ToShortDateString();

                string tableHeaderDesc = "Name" + delimiter + delimiter + delimiter + delimiter + delimiter + "Shift Time" + delimiter + "Shift Time" + delimiter +
                                        "Shift Time" + delimiter + "Shift Time" + delimiter + "Shift Time" + delimiter + "Shift Time" + delimiter +
                                        "Shift Time" + delimiter + "Signature";

                
                List<string> lines = new List<string>();

                // create file lines
                foreach (int id in emplDict.Keys)
                {
                    string line = "";
                    EmployeeAsco4TO dataAboutEmpl = ascoDict[id];
                    Dictionary<int, int> emplCounterValue = new Dictionary<int, int>();
                    //STANKO - ovde npr dodaj dictionary Dictionary<int,int>

                    if(counterValuesDict.ContainsKey(id))
                    {
                        emplCounterValue = counterValuesDict[id];
                    }

                    //First and last name of an employee
                    line = emplDict[id].FirstAndLastName;

                    //id of an employee
                    line += delimiter + id.ToString();

                    //STANKO umesto "ow days" stavi broj dana (1 dan znaci da je radio vise od 8 h prekovremeno)
                    line += delimiter + "ow days";
                  
                    //vacation days left from previous year
                    //and
                    //vacation days from this year
                    int noOfPrevDays = (emplCounterValue[2] - emplCounterValue[3]);
                    int noOfCurentDays = emplCounterValue[1];
                    if(noOfPrevDays > 0)
                    { 
                        line += delimiter + noOfPrevDays;
                        line += delimiter + noOfCurentDays;
                    }
                    else
                    {
                        line += delimiter + "0";
                        noOfCurentDays += noOfPrevDays;
                        line += delimiter + noOfCurentDays;
                    }

                    Dictionary<DateTime, List<WorkTimeIntervalTO>> emplDayIntervals = new Dictionary<DateTime,List<WorkTimeIntervalTO>>();
                    if(emplDayIntervalsDict.ContainsKey(id))
                    {
                        emplDayIntervals = emplDayIntervalsDict[id];
                    }

                    //STANKO za svaki dan moras proveriti u prosledjenim dictionary-jima da li je SickLeave, Maternity Leave -> npr Dictionary<int, Dictionary<DateTime, int>> emplSickLeaves koji ti je prosledjen kroz zaglavlje metode, ovde prvo izvuci za tog zaposlenog Dictionary<DateTime, int>
                    foreach(string day in dateOfTheDay.Keys)
                    {
                        DateTime date = dateOfTheDay[day];
                        List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                        if (emplDayIntervals.ContainsKey(date))
                        {
                            intervals = emplDayIntervals[date];
                            string intervalString = "";
                            string start = "";
                            string end = "";
                           
                            foreach(WorkTimeIntervalTO interv in intervals)
                            {
                                if (interv.StartTime.TimeOfDay == new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).TimeOfDay &&
                                    interv.EndTime.TimeOfDay == new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).TimeOfDay)
                                {
                                    intervalString = "DO";
                                }
                                else
                                {
                                    //nocna smena
                                    if (interv.EndTime.TimeOfDay == new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 0).TimeOfDay)
                                    {
                                        start = interv.StartTime.ToShortTimeString();
                                        end = interv.StartTime.AddHours(8).ToShortTimeString();
                                    }
                                    else if(interv.StartTime.TimeOfDay != new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).TimeOfDay)
                                    {
                                        start = interv.StartTime.ToShortTimeString();
                                        end = interv.EndTime.ToShortTimeString();
                                    }
                                }
                            }
                            if(start.Equals("") && end.Equals(""))
                            {
                                intervalString = "DO";
                            }
                            else
                            {
                            intervalString = start + " - " + end;
                            }
                            line += delimiter + intervalString;
                        }
                        else
                        {
                            line += delimiter + " ";
                        }

                    }

                    lines.Add(line);
                }

                string managerForPayroll = delimiter + "Manager: " + delimiter + "______________________________" + delimiter + delimiter + delimiter + "For Payroll: " + delimiter + "______________________________";
                string dates = " " + delimiter + "Date" + delimiter + delimiter + delimiter + " " + delimiter + "Date";

                string legendW = "W - Working Regular";
                string legendDO = "DO - Day Off";
                string legendPH = "PH - Public hoiday";
                string legendOW = "OW - Owed Day";
                string legendSL = "SL - SickLeave";
                string legendLSL = "LSL - Long Sick Leave";
                string legendML = "ML - MaternityLeave";
                string legendV = "V - Vacation";
                string legendS = "S - Slava day";

                string reportName = "Hyatt_TimeAndAttendanceRecord_" + DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss");

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
                    //insert parameters
                    writer.WriteLine(parametersLine);
                    //insert emptyLine
                    writer.WriteLine("");
                    //insert days in weeks
                    writer.WriteLine(days);
                    //insert headerOfTable
                    writer.WriteLine(tableHeader);
                    //insert second header
                    writer.WriteLine(tableHeaderDesc);
                    foreach (string line in lines)
                    {
                        writer.WriteLine(line);
                    }

                    writer.WriteLine(" ");

                    writer.WriteLine(managerForPayroll);
                    writer.WriteLine(dates);

                    writer.WriteLine(" ");

                    writer.WriteLine(legendW);
                    writer.WriteLine(legendDO);
                    writer.WriteLine(legendPH);
                    writer.WriteLine(legendOW);
                    writer.WriteLine(legendSL);
                    writer.WriteLine(legendLSL);
                    writer.WriteLine(legendML);
                    writer.WriteLine(legendV);
                    writer.WriteLine(legendS);
                    writer.Close();
                }


            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " HyattTimeAndAttendance.generateReport(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
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




}

}
