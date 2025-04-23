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

namespace Reports.Ministarstvo
{
    public partial class MinistarstvoEmployeePresence : Form
    {
        CultureInfo culture;
        ResourceManager rm;
        ApplUserTO logInUser;
        DebugLog debug;

        Filter filter;

        int counter = 0;

        // all Holidays, Key is Date, value is Holiday
        Dictionary<DateTime, HolidayTO> holidays = new Dictionary<DateTime,HolidayTO>();

        // Working Units that logInUser is granted to
        List<WorkingUnitTO> wUnits;

        const int pauseAutomated = -102;
        private const int regularWorkOut = -200;

        // all time shemas
        List<WorkTimeSchemaTO> timeSchemas = new List<WorkTimeSchemaTO>();
        
		List<EmployeeTO> selectedEmployees = new List<EmployeeTO>();

        public MinistarstvoEmployeePresence()
        {
            InitializeComponent();
            // get all time schemas
            timeSchemas = new TimeSchema().Search();

            // get all holidays
            List<HolidayTO> holidayList = new Holiday().Search(new DateTime(), new DateTime());

            foreach (HolidayTO holiday in holidayList)
            {
                holidays.Add(holiday.HolidayDate, holiday);
            }
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("employeePresenceReport", culture);
                gbWorkingUnit.Text = rm.GetString("workingUnits", culture);
                lblWorkingUnitName.Text = rm.GetString("lblName", culture);
                chbHierarhicly.Text = rm.GetString("hierarchically", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                gbTimeInterval.Text = rm.GetString("timeInterval", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);
                chbShowRetired.Text = rm.GetString("chbShowRetired", culture);

                lblReportType.Text = rm.GetString("reportType", culture);
                rbAnalytical.Text = rm.GetString("analitycal", culture);
                rbSummary.Text = rm.GetString("summary", culture);
				
                gbFilter.Text = rm.GetString("gbFilter", culture);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " MinistarstvoEmployeePresece.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void MinistarstvoEmployeePresence_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Init debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                debug = new DebugLog(logFilePath);


                // Language tool
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(EmployeeAnalyticalWU).Assembly);
                setLanguage();

                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));

                logInUser = NotificationController.GetLogInUser();
                populateWorkigUnitCombo();

                DateTime date = DateTime.Now.Date;
                this.CenterToScreen();

                dtpFromDate.Value = new DateTime(date.Year, date.Month, 1);
                dtpToDate.Value = date;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " MinistarstvoEmployeePresece.MinistarstvoEmployeePresence_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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
                debug.writeLog(DateTime.Now + " MinistarstvoEmployeePresece.populateWorkigUnitCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                
                counter = 0;

                Hashtable passTypeSummary = new Hashtable();

                if (wUnits.Count == 0)
                {
                    MessageBox.Show(rm.GetString("noWUGranted", culture));
                }
                else
                {
                    int selectedWorkingUnit = (int)cbWorkingUnit.SelectedValue;
                    if (selectedWorkingUnit == -1)
                    {
                        wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                    }
                    if (this.chbHierarhicly.Checked)
                    {
                        WorkingUnit wu = new WorkingUnit();
                        if (selectedWorkingUnit != -1)
                        {
                            wu.WUTO.WorkingUnitID = selectedWorkingUnit;
                            wUnits = wu.Search();
                            wUnits = wu.FindAllChildren(wUnits);
                            selectedWorkingUnit = -1;
                        }
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
                    int count = ioPair.SearchForWUCount(selectedWorkingUnit, wuString, dtpFromDate.Value, dtpToDate.Value);
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

                    List<IOPairTO> ioPairList = ioPair.SearchForWU(selectedWorkingUnit, wuString,
                        dtpFromDate.Value, dtpToDate.Value.AddDays(1));

                    // Get Employees for selected Working Unit	
                    Employee empl = new Employee();
                    if (this.chbHierarhicly.Checked || (selectedWorkingUnit == -1))
                    {
                        selectedEmployees = empl.SearchByWU(wuString);
                    }
                    else
                    {
                        selectedEmployees = empl.SearchByWU(cbWorkingUnit.SelectedValue.ToString());
                    }
                    if (!chbShowRetired.Checked)
                    {
                        List<EmployeeTO> emplList = selectedEmployees;
                        selectedEmployees = new List<EmployeeTO>();
                        foreach (EmployeeTO emp in emplList)
                        {
                            if (!emp.Status.Equals(Constants.statusRetired))
                            {
                                selectedEmployees.Add(emp);
                            }
                        }
                    }

                    //key is employee id and value is list of time Schedules for selected period 
                    Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedule = new Dictionary<int,List<EmployeeTimeScheduleTO>>();

                    foreach (EmployeeTO currentEmployee in selectedEmployees)
                    {
                        List<EmployeeTimeScheduleTO> emplTS = new EmployeesTimeSchedule().SearchEmployeesSchedules(currentEmployee.EmployeeID.ToString(), dtpFromDate.Value, dtpToDate.Value);

                        if (!emplTimeSchedule.ContainsKey(currentEmployee.EmployeeID))
                        {
                            emplTimeSchedule.Add(currentEmployee.EmployeeID, new List<EmployeeTimeScheduleTO>());
                        }

                        emplTimeSchedule[currentEmployee.EmployeeID] = emplTS;
                    }


                    Dictionary<int, List<IOPairTO>> classifiedPairs = new Dictionary<int, List<IOPairTO>>();
                    List<IOPairTO> clasifiedPairsList = new List<IOPairTO>();

                    Dictionary<DateTime, Dictionary<int, IOPairTO>> firstPairEmplDay = new Dictionary<DateTime, Dictionary<int, IOPairTO>>();
                    Dictionary<DateTime, Dictionary<int, IOPairTO>> lastPairEmplDay = new Dictionary<DateTime, Dictionary<int, IOPairTO>>();

                    //key is date, value is list of employee_ids with open pairs
                    Dictionary<DateTime, List<int>> openPairs = new Dictionary<DateTime, List<int>>();

                    for (int i = 0; i < ioPairList.Count; i++)
                    {
                        Dictionary<int, IOPairTO> firstPair = new Dictionary<int, IOPairTO>();
                        Dictionary<int, IOPairTO> lastPair = new Dictionary<int, IOPairTO>();

                        //Do not calculate notWorking pairs
                        if ((ioPairList[i].IsWrkHrsCount == (int)Constants.IsWrkCount.IsNotCounter) ||
                              (ioPairList[i].StartTime.TimeOfDay.Equals(new TimeSpan(0, 0, 0)))
                              && (ioPairList[i].EndTime.TimeOfDay.Equals(new TimeSpan(0, 0, 0))))
                        {
                            continue;
                        }

                        if (ioPairList[i].StartTime.TimeOfDay.Equals(new TimeSpan(0, 0, 0))
                              || ioPairList[i].EndTime.TimeOfDay.Equals(new TimeSpan(0, 0, 0)))
                        {
                            if (!openPairs.ContainsKey(ioPairList[i].IOPairDate.Date))
                            {
                                openPairs.Add(ioPairList[i].IOPairDate.Date, new List<int>());
                            }
                            if (!(openPairs[ioPairList[i].IOPairDate.Date]).Contains(ioPairList[i].EmployeeID))
                            {
                                openPairs[ioPairList[i].IOPairDate.Date].Add(ioPairList[i].EmployeeID);
                            }
                        }

                        if (ioPairList[i].PassTypeID == pauseAutomated)
                        {
                            ioPairList[i].PassTypeID = Constants.regularWork;
                        }

                        if (ioPairList[i].PassTypeID == Constants.regularWork)
                        {
                            if (!firstPairEmplDay.ContainsKey(ioPairList[i].IOPairDate.Date))
                            {
                                firstPairEmplDay.Add(ioPairList[i].IOPairDate.Date, new Dictionary<int, IOPairTO>());
                            }
                            firstPair = firstPairEmplDay[ioPairList[i].IOPairDate.Date];
                            if (firstPair.ContainsKey(ioPairList[i].EmployeeID))
                            {
                                IOPairTO first = firstPair[ioPairList[i].EmployeeID];
                                if (first.StartTime > ioPairList[i].StartTime) firstPair[ioPairList[i].EmployeeID] = ioPairList[i];
                            }
                            else
                            {
                                firstPair.Add(ioPairList[i].EmployeeID, ioPairList[i]);
                            }
                            if (!lastPairEmplDay.ContainsKey(ioPairList[i].IOPairDate.Date))
                            {
                                lastPairEmplDay.Add(ioPairList[i].IOPairDate.Date, new Dictionary<int, IOPairTO>());
                            }
                            lastPair = lastPairEmplDay[ioPairList[i].IOPairDate.Date];
                            if (lastPair.ContainsKey(ioPairList[i].EmployeeID))
                            {
                                IOPairTO last = (IOPairTO)lastPair[ioPairList[i].EmployeeID];
                                if (last.EndTime < ioPairList[i].EndTime) lastPair[ioPairList[i].EmployeeID] = ioPairList[i];
                            }
                            else
                            {
                                lastPair.Add(ioPairList[i].EmployeeID, ioPairList[i]);
                            }
                        }
                        int currentEmployeeID = ioPairList[i].EmployeeID;
                        if (!classifiedPairs.ContainsKey(currentEmployeeID))
                        {
                            classifiedPairs.Add(currentEmployeeID, new List<IOPairTO>());
                        }

                        classifiedPairs[currentEmployeeID].Add(ioPairList[i]);
                    }

                    TimeSpan totalTime = new TimeSpan(0);
                    int numOfRecords = 0;

                    DataSet dataSet = new DataSet();
                    DataTable table = new DataTable("EmplPresence");

                    table.Columns.Add("date", typeof(System.String));
                    table.Columns.Add("last_name", typeof(System.String));
                    table.Columns.Add("first_name", typeof(System.String));
                    table.Columns.Add("absence", typeof(int));
                    table.Columns.Add("total_time", typeof(int));
                    table.Columns.Add("total", typeof(int));
                    table.Columns.Add("over_time", typeof(int));
                    table.Columns.Add("need_validation", typeof(System.String));
                    table.Columns.Add("working_unit", typeof(System.String));
                    table.Columns.Add("earlyest_arrived", typeof(System.String));
                    table.Columns.Add("latest_left", typeof(System.String));
                    table.Columns.Add("date_sort", typeof(System.DateTime));
                    table.Columns.Add("employee_id", typeof(int));
                    table.Columns.Add("imageID", typeof(byte));

                    DataTable tableI = new DataTable("images");
                    tableI.Columns.Add("image", typeof(System.Byte[]));
                    tableI.Columns.Add("imageID", typeof(byte));

                    // int counter = 0;

                    // Totals by PassType
                    foreach (EmployeeTO currentEmployee in selectedEmployees)
                    {
                        List<EmployeeTimeScheduleTO> emplDaySchedules = new List<EmployeeTimeScheduleTO>();
                        if (emplTimeSchedule.ContainsKey(currentEmployee.EmployeeID))
                            emplDaySchedules = emplTimeSchedule[currentEmployee.EmployeeID];

                        Hashtable daylyPassTypeSummary = new Hashtable();
                        for (DateTime day = dtpFromDate.Value; day <= dtpToDate.Value; day = day.AddDays(1))
                        {
                            DataRow row = table.NewRow();
                            
                            row["employee_id"] = currentEmployee.EmployeeID;
                            row["working_unit"] = currentEmployee.WorkingUnitName;
                            row["first_name"] = currentEmployee.FirstName;
                            row["last_name"] = currentEmployee.LastName;
                            row["date"] = day.ToString("dd.MM.yyyy");
                            row["need_validation"] = " ";
                            row["earlyest_arrived"] = "-";
                            row["latest_left"] = "-";
                            row["absence"] = 0;
                            row["total"] = 0;
                            row["over_time"] = 0;
                            row["total_time"] = 0;
                            row["imageID"] = 1;
                            if (counter == 0)
                            {
                                //add logo image just once
                                DataRow rowI = tableI.NewRow();
                                rowI["image"] = Constants.LogoForReport;
                                rowI["imageID"] = 1;
                                tableI.Rows.Add(rowI);
                                tableI.AcceptChanges();
                            }
                            counter++;

                            IOPairTO first = new IOPairTO();
                            if (firstPairEmplDay.ContainsKey(day.Date))
                            {
                                Dictionary<int, IOPairTO> firstIN = firstPairEmplDay[day.Date];
                                if (firstIN.ContainsKey(currentEmployee.EmployeeID))
                                {
                                    first = firstIN[currentEmployee.EmployeeID];
                                    row["earlyest_arrived"] = firstIN[currentEmployee.EmployeeID].StartTime.ToString("HH:mm");

                                }
                            }

                            row["date_sort"] = new DateTime(day.Year, day.Month, day.Day, first.StartTime.Hour, first.StartTime.Minute, 0);
                            if (lastPairEmplDay.ContainsKey(day.Date))
                            {
                                Dictionary<int, IOPairTO> lastIN = lastPairEmplDay[day.Date];
                                if (lastIN.ContainsKey(currentEmployee.EmployeeID))
                                {
                                    row["latest_left"] = lastIN[currentEmployee.EmployeeID].EndTime.ToString("HH:mm");
                                }
                            }
                            Hashtable passTypeTotalTime = new Hashtable();
                            if (classifiedPairs.ContainsKey(currentEmployee.EmployeeID))
                            {
                                List<EmployeeTO> employees = new List<EmployeeTO>();

                                employees.Add(currentEmployee);

                                TimeSpan total = this.calculateHolesDuringWorkingTime(classifiedPairs, emplTimeSchedule, employees, day, day, first);
                                total = new TimeSpan(total.Hours, total.Minutes, 0);

                                if (!daylyPassTypeSummary.ContainsKey(day))
                                {
                                    daylyPassTypeSummary.Add(day, new Hashtable());
                                }
                                if (total.TotalMinutes >= 1)
                                {
                                    passTypeTotalTime = (Hashtable)daylyPassTypeSummary[day];
                                    if (passTypeTotalTime.ContainsKey(Constants.absence))
                                    {
                                        passTypeTotalTime[Constants.absence] = ((TimeSpan)passTypeTotalTime[Constants.absence]).Add(total);
                                    }
                                    else
                                    {
                                        passTypeTotalTime.Add(Constants.absence, total);
                                        row["absence"] = total.TotalMinutes;
                                        numOfRecords++;
                                    }
                                }

                                if (openPairs.ContainsKey(day.Date))
                                {
                                    List<int> emplIDs = openPairs[day.Date];
                                    if (emplIDs.Contains(currentEmployee.EmployeeID))
                                    {
                                        row["need_validation"] = "X";
                                    }
                                }

                                passTypeTotalTime = (Hashtable)daylyPassTypeSummary[day];
                                //if (passTypeTotalTime.ContainsKey(Constants.regularWork))
                                //{
                                TimeSpan regularWorkTrim = new TimeSpan();
                                TimeSpan regularWorkTotal = new TimeSpan();
                                TimeSpan regularWorkTolerance = new TimeSpan();

                                if (classifiedPairs.ContainsKey(currentEmployee.EmployeeID))
                                {
                                    TimeSpan regWorkForDay = new TimeSpan();
                                    TimeSpan regWorkInWT = new TimeSpan();
                                    TimeSpan regWorkWithTolerance = new TimeSpan();
                                    bool is2DayShift = false;
                                    bool is2DaysShiftPrevious = false;
                                    WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                                    //get intervls for employee and day
                                    Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplDaySchedules, day, ref is2DayShift, ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);
                                    Employee employee = new Employee();
                                    employee.EmplTO = currentEmployee;
                                    ArrayList schemas = employee.findTimeSchema(day); // first member is time schema, second member is day number
                                    WorkTimeSchemaTO timeSchema = new WorkTimeSchemaTO();
                                    if (schemas.Count > 0)
                                        timeSchema = (WorkTimeSchemaTO)schemas[0];
                                    //if employee absences day is working day and it is not holiday count it as used and one day less in LeftDays
                                    List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                                    if (edi != null)
                                    {
                                        intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DaysShiftPrevious, firstIntervalNextDay, edi);
                                    }//if (edi != null)
                                    else
                                    {
                                        WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
                                        intervals.Add(interval);

                                    }
                                    Dictionary<int, WorkTimeIntervalTO> dayIntervals = new Dictionary<int,WorkTimeIntervalTO>();
                                    for (int i = 0; i < intervals.Count; i++)
                                    {
                                        WorkTimeIntervalTO interval = intervals[i];
                                        dayIntervals.Add(i, interval);
                                    }

                                    List<IOPairTO> dayIOPairList = Common.Misc.getEmployeeDayPairs(classifiedPairs[currentEmployee.EmployeeID], day, is2DayShift, is2DaysShiftPrevious, firstIntervalNextDay, dayIntervals);
                                    dayIOPairList = trimPairsStartTimeIntervals(dayIOPairList, intervals, is2DayShift, day, timeSchema, first, false);
                                    foreach (IOPairTO trimPair in dayIOPairList)
                                    {
                                        if (trimPair.PassTypeID == Constants.regularWork)
                                        {
                                            regWorkForDay += trimPair.EndTime.Subtract(trimPair.StartTime);
                                        }
                                    }

                                    List<IOPairTO> trimedPairs = trimPairsWithIntervals(dayIOPairList, intervals, is2DayShift, day, timeSchema, first, false);

                                    foreach (IOPairTO trimPair in trimedPairs)
                                    {
                                        if (trimPair.PassTypeID == Constants.regularWork)
                                        {
                                            regWorkInWT += trimPair.EndTime.Subtract(trimPair.StartTime);
                                        }
                                    }

                                    List<IOPairTO> regWRKPairs = new List<IOPairTO>();

                                    regWRKPairs = trimPairsWithIntervals(dayIOPairList, intervals, is2DayShift, day, timeSchema, first, true);

                                    foreach (IOPairTO trimPair in regWRKPairs)
                                    {
                                        if (trimPair.PassTypeID == Constants.regularWork)
                                        {
                                            regWorkWithTolerance += trimPair.EndTime.Subtract(trimPair.StartTime);
                                        }
                                    }

                                    regularWorkTrim += regWorkInWT;
                                    regularWorkTotal += regWorkForDay;
                                    regularWorkTolerance += regWorkWithTolerance;
                                }//if (classifiedPairs.ContainsKey(currentEmployee.EmployeeID))

                                TimeSpan extraHours = regularWorkTotal - regularWorkTrim;
                                passTypeTotalTime[Constants.regularWork] = regularWorkTrim;
                                passTypeTotalTime.Add(regularWorkOut, extraHours);
                                if (!isHoliday(day))
                                {
                                    row["total_time"] = regularWorkTolerance.TotalMinutes;
                                    row["over_time"] = extraHours.TotalMinutes;
                                    row["total"] = regularWorkTolerance.TotalMinutes + extraHours.TotalMinutes;
                                }
                                else
                                {
                                    row["total_time"] = 0;
                                    row["over_time"] = regularWorkTotal.TotalMinutes;
                                    row["total"] = regularWorkTotal.TotalMinutes;
                                }
                            }
                            else
                            {
                                if (!isHoliday(day))
                                {
                                    TimeSpan timeInterval = new TimeSpan();
                                    bool is2DayShift = false;
                                    bool is2DaysShiftPrevious = false;
                                    WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                                    //get intervls for employee and day
                                    Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplDaySchedules, day, ref is2DayShift, ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);
                                    Employee employee = new Employee();
                                    employee.EmplTO = currentEmployee;
                                    ArrayList schemas = employee.findTimeSchema(day);
                                    WorkTimeSchemaTO timeSchema = new WorkTimeSchemaTO();
                                    if (schemas.Count > 0)
                                        timeSchema = (WorkTimeSchemaTO)schemas[0];
                                    //if employee absences day is working day and it is not holiday count it as used and one day less in LeftDays
                                    List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                                    if (edi != null)
                                    {
                                        intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DaysShiftPrevious, firstIntervalNextDay, edi);
                                    }//if (edi != null)
                                    else
                                    {
                                        WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
                                        intervals.Add(interval);
                                    }
                                    Dictionary<int, WorkTimeIntervalTO> dayIntervals = new Dictionary<int,WorkTimeIntervalTO>();
                                    for (int i = 0; i < intervals.Count; i++)
                                    {
                                        WorkTimeIntervalTO interval = intervals[i];
                                        timeInterval += interval.EndTime.Subtract(interval.StartTime);
                                    }
                                    row["absence"] = timeInterval.TotalMinutes;
                                }
                                else
                                {
                                    row["absence"] = 0;
                                }
                                row["need_validation"] = "";
                                row["total_time"] = 0;
                                row["over_time"] = 0;
                                row["total"] = 0;
                            }
                            //}
                            table.Rows.Add(row);

                        }
                    }
                    table.AcceptChanges();
                    dataSet.Tables.Add(table);
                    dataSet.Tables.Add(tableI);

                    //if (daylyPassTypeSummary.Count > 0)
                    //{
                    //    EmployeesTotalTime.Add(currentEmployee.EmployeeID, daylyPassTypeSummary);
                    //}
                    //if (EmployeesTotalTime.Count == 0)
                    //{
                    //    MessageBox.Show(rm.GetString("dataNotFound", culture));
                    //    return;
                    //}

                    //if (numOfRecords > Constants.maxWUReportRecords)
                    //{
                    //    this.Cursor = Cursors.Arrow;
                    //    MessageBox.Show(rm.GetString("moreRecordsThanAllowed", culture));
                    //    return;
                    //}
                    //else if (numOfRecords > Constants.warningWUReportRecords)
                    //{
                    //    this.Cursor = Cursors.Arrow;
                    //    DialogResult result = MessageBox.Show(rm.GetString("recordsForWarning", culture), "", MessageBoxButtons.YesNo);
                    //    if (result.Equals(DialogResult.No))
                    //    {
                    //        return;
                    //    }
                    //}
                    if (rbAnalytical.Checked)
                    {
                        Reports.Ministarstvo.Ministarstvo_sr.MinistarstvoEmployeePresenceCRView report = new Reports.Ministarstvo.Ministarstvo_sr.MinistarstvoEmployeePresenceCRView(dataSet, dtpFromDate.Value.Date, dtpToDate.Value.Date, cbWorkingUnit.Text.ToString());
                        report.ShowDialog();
                    }
                    else
                    {
                        Reports.Ministarstvo.Ministarstvo_sr.MinistarstvoEmployeePresenceSumCRView report = new Reports.Ministarstvo.Ministarstvo_sr.MinistarstvoEmployeePresenceSumCRView(dataSet, dtpFromDate.Value.Date, dtpToDate.Value.Date, cbWorkingUnit.Text.ToString());
                        report.ShowDialog();
                    }
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
            //calculate holes in working time for employees and days with IOPairs
        //get values in minutes
        public TimeSpan calculateHolesDuringWorkingTime(Dictionary<int, List<IOPairTO>> emplIOPairs, Dictionary<int, List<EmployeeTimeScheduleTO>> employeeTimeShedule, List<EmployeeTO> employees, DateTime fromDay, DateTime toDay, IOPairTO iopair)
        {
           TimeSpan holesDuringWorkingTime = new TimeSpan(0);
           try
           {
               IOPair ioPair = new IOPair();
               // all employee time schedules for selected Time Interval, key is employee ID
               Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedules = new Dictionary<int,List<EmployeeTimeScheduleTO>>();

               List<int> employeesID = new List<int>();
               foreach (EmployeeTO employee in employees)
               {
                   employeesID.Add(employee.EmployeeID);
               }
               // Key is Employee ID, value is ArrayList of valid IO Pairs for that Employee
               Dictionary<int, List<IOPairTO>> emplPairs = new Dictionary<int,List<IOPairTO>>();
               emplPairs = emplIOPairs;
               emplTimeSchedules = employeeTimeShedule;

               //Start calculation
               //for each employee, day, interval in that day
               foreach (int employeeID in employeesID)
               {
                   if (!emplTimeSchedules.ContainsKey(employeeID))
                       continue;
                   
                   TimeSpan intervalDuration = new TimeSpan();
                   for (DateTime day = fromDay; day <= toDay; day = day.AddDays(1))
                   {
                       Employee empl = new Employee();
                       empl.EmplTO.EmployeeID = employeeID;
                       ArrayList schemas = empl.findTimeSchema(day);

                       if (schemas != null && schemas.Count > 0)
                       {
                           WorkTimeSchemaTO schema = (WorkTimeSchemaTO)schemas[0];

                           //for flexi and normal working time don't count holidays as absence
                           if (isHoliday(day) && (schema.Type.Equals(Constants.schemaTypeFlexi) || schema.Type.Equals(Constants.schemaTypeNormal)))
                               continue;
                           //calculate holes only for employee's dates
                           bool is2DayShift = false;
                           bool is2DayShiftPrevious = false;
                           WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                           //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                           //are night shift days. If day is night shift day, also take first interval of next day
                           Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day, ref is2DayShift,
                               ref is2DayShiftPrevious, ref firstIntervalNextDay, this.timeSchemas);

                           TimeSpan ioPairsDuration = new TimeSpan(0);
                           List<IOPairTO> insertedPairs = new List<IOPairTO>();
                           WorkTimeSchemaTO timeSchema = new WorkTimeSchemaTO();
                           if (schemas.Count > 0)
                               timeSchema = (WorkTimeSchemaTO)schemas[0];
                           //if employee absences day is working day and it is not holiday count it as used and one day less in LeftDays
                           if (edi != null)
                           {
                               List<WorkTimeIntervalTO> intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DayShiftPrevious, firstIntervalNextDay, edi);
                               Dictionary<int, WorkTimeIntervalTO> dayIntervals = new Dictionary<int,WorkTimeIntervalTO>();
                               for (int i = 0; i < intervals.Count; i++)
                               {
                                   WorkTimeIntervalTO interval = intervals[i];
                                   dayIntervals.Add(i, interval);
                               }
                               List<IOPairTO> newList = new List<IOPairTO>();
                               if (emplIOPairs.ContainsKey(empl.EmplTO.EmployeeID))
                                   foreach (IOPairTO iop in emplIOPairs[empl.EmplTO.EmployeeID])
                                   {
                                       newList.Add(iop);
                                   }
                               List<IOPairTO> dayIOPairList = Common.Misc.getEmployeeDayPairs(newList, day, is2DayShift, is2DayShiftPrevious, firstIntervalNextDay, dayIntervals);

                               List<IOPairTO> trimedPairs = trimPairsWithIntervals(dayIOPairList, intervals, is2DayShift, day, timeSchema, iopair, true);

                               for (int i = 0; i < trimedPairs.Count;i++)
                               {
                                   IOPairTO pair = trimedPairs[i];
                                   
                                   for (int j = 0; j < insertedPairs.Count;j++)                                      
                                   {
                                       IOPairTO insPair = insertedPairs[j];
                                       if (((pair.EndTime.TimeOfDay >= insPair.StartTime.TimeOfDay && pair.EndTime.TimeOfDay <= insPair.EndTime.TimeOfDay)
                                           || (pair.StartTime.TimeOfDay <= insPair.EndTime.TimeOfDay && pair.StartTime.TimeOfDay >= insPair.StartTime.TimeOfDay)
                                           || (pair.StartTime.TimeOfDay <= insPair.StartTime.TimeOfDay && pair.EndTime.TimeOfDay >= insPair.EndTime.TimeOfDay))
                                           && pair.IOPairDate == insPair.IOPairDate)
                                       {
                                           if (pair.StartTime.TimeOfDay >= insPair.StartTime.TimeOfDay && pair.EndTime.TimeOfDay <= insPair.EndTime.TimeOfDay)
                                           {
                                               pair.EndTime = pair.StartTime;
                                           }
                                           else if (pair.EndTime.TimeOfDay <= insPair.EndTime.TimeOfDay && pair.EndTime.TimeOfDay >= insPair.StartTime.TimeOfDay)
                                           {
                                               pair.EndTime = insPair.StartTime;
                                           }
                                           else if (pair.StartTime.TimeOfDay >= insPair.StartTime.TimeOfDay && pair.StartTime.TimeOfDay <= insPair.EndTime.TimeOfDay)
                                           {
                                               pair.StartTime = insPair.EndTime;
                                           }
                                           else
                                           {
                                               IOPairTO newPair = new IOPairTO(pair);                                               
                                               newPair.EndTime = insPair.StartTime;
                                               pair.StartTime = insPair.EndTime;
                                               insertedPairs.Add(newPair);
                                               ioPairsDuration += (new DateTime(newPair.EndTime.TimeOfDay.Ticks - newPair.StartTime.TimeOfDay.Ticks)).TimeOfDay;
                                           }
                                       }
                                   }
                                   insertedPairs.Add(pair);
                                   ioPairsDuration += pair.EndTime.TimeOfDay - pair.StartTime.TimeOfDay;
                               }

                               foreach (WorkTimeIntervalTO interval in intervals)
                               {
                                   intervalDuration += (new DateTime(interval.EndTime.TimeOfDay.Ticks - interval.StartTime.TimeOfDay.Ticks)).TimeOfDay;
                               }
                           }

                           holesDuringWorkingTime += intervalDuration - ioPairsDuration;
                       }
                   }
               }
           }
           catch (Exception ex)
           {
               throw new Exception(ex.Message);
           }
            return holesDuringWorkingTime;
        }

        bool isHoliday(DateTime day)
        {
            return (holidays.ContainsKey(day));
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
                if (date >= timeScheduleList[scheduleIndex].Date)
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

        //trim pairs with intervals
        private List<IOPairTO> trimPairsWithIntervals(List<IOPairTO> dayPairs, List<WorkTimeIntervalTO> intervalList, bool is2DayShift, DateTime day, WorkTimeSchemaTO schema, IOPairTO firstPair, bool calcTolerance)
        {
            List<IOPairTO> trimedPairs = new List<IOPairTO>();
            List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
            foreach (WorkTimeIntervalTO interval in intervalList)
            {
                intervals.Add(interval.Clone());
            }
            try
            {
                if ((schema.Type.Equals(Constants.schemaTypeFlexi) && dayPairs.Count > 0 && intervalList.Count > 0)
                    || calcTolerance)
                {
                    WorkTimeIntervalTO interval = intervals[0];

                    if (firstPair.StartTime.TimeOfDay < interval.EarliestArrived.TimeOfDay)
                    {
                        interval.EarliestArrived = interval.EarliestArrived;
                        interval.StartTime = interval.EarliestArrived;
                        interval.LatestArrivaed = interval.EarliestArrived;
                        interval.EndTime = interval.EarliestLeft;
                        interval.LatestLeft = interval.EndTime;
                        interval.EarliestLeft = interval.EndTime;
                    }
                    if (firstPair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && firstPair.StartTime.TimeOfDay <= interval.LatestArrivaed.TimeOfDay)
                    {
                        interval.EndTime = new DateTime(firstPair.StartTime.Ticks + (interval.EndTime.Ticks - interval.StartTime.Ticks));
                        interval.LatestLeft = interval.EndTime;
                        interval.EarliestLeft = interval.EndTime;
                        interval.EarliestArrived = firstPair.StartTime;
                        interval.StartTime = firstPair.StartTime;
                        interval.LatestArrivaed = firstPair.StartTime;

                    }
                    if (firstPair.StartTime.TimeOfDay > interval.LatestArrivaed.TimeOfDay)
                    {
                        interval.EarliestArrived = interval.LatestArrivaed;
                        interval.StartTime = interval.LatestArrivaed;
                        interval.LatestArrivaed = interval.LatestArrivaed;
                        interval.EndTime = interval.LatestLeft;
                        interval.LatestLeft = interval.EndTime;
                        interval.EarliestLeft = interval.EndTime;
                    }
                }

                foreach (IOPairTO pair in dayPairs)
                {
                    foreach (WorkTimeIntervalTO interval in intervals)
                    {
                        if (interval.StartTime.TimeOfDay < interval.EarliestArrived.TimeOfDay)
                            interval.EarliestArrived = interval.StartTime;
                        if (interval.LatestLeft.TimeOfDay < interval.EndTime.TimeOfDay)
                            interval.LatestLeft = interval.EndTime;
                        //if whole pair is inside a interval add pair to trimed pairs list
                        if ((pair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay || pair.StartTime.TimeOfDay >= interval.StartTime.TimeOfDay) &&
                           (pair.EndTime.TimeOfDay <= interval.EndTime.TimeOfDay || pair.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay))
                        {
                            trimedPairs.Add(pair);
                        }
                        else if (pair.EndTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && pair.StartTime.TimeOfDay <= interval.EarliestArrived.TimeOfDay)
                        {
                            pair.StartTime = new DateTime(pair.StartTime.Year, pair.StartTime.Month, pair.StartTime.Day, interval.EarliestArrived.Hour, interval.EarliestArrived.Minute, 0);
                            if (pair.StartTime.TimeOfDay < interval.LatestLeft.TimeOfDay &&
                                  pair.EndTime.TimeOfDay > interval.LatestLeft.TimeOfDay)
                            {
                                pair.EndTime = new DateTime(pair.EndTime.Year, pair.EndTime.Month, pair.EndTime.Day, interval.LatestLeft.Hour, interval.LatestLeft.Minute, 0);
                            }
                            trimedPairs.Add(pair);
                        }
                        else if (pair.StartTime.TimeOfDay < interval.LatestLeft.TimeOfDay &&
                       pair.EndTime.TimeOfDay > interval.LatestLeft.TimeOfDay)
                        {
                            pair.EndTime = new DateTime(pair.EndTime.Year, pair.EndTime.Month, pair.EndTime.Day, interval.LatestLeft.Hour, interval.LatestLeft.Minute, 0);
                            trimedPairs.Add(pair);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " EmployeePresenceType.trimPairsWithIntervals(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return trimedPairs;
        }

        //trim pairs with intervals beginning
        private List<IOPairTO> trimPairsStartTimeIntervals(List<IOPairTO> dayPairs, List<WorkTimeIntervalTO> intervalList, bool is2DayShift, DateTime day, WorkTimeSchemaTO schema, IOPairTO firstPair, bool calcTolerance)
        {
            List<IOPairTO> trimedPairs = new List<IOPairTO>();
            List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
            foreach (WorkTimeIntervalTO interval in intervalList)
            {
                intervals.Add(interval.Clone());
            }
            try
            {
                if ((schema.Type.Equals(Constants.schemaTypeFlexi) && dayPairs.Count > 0 && intervalList.Count > 0)
                    || calcTolerance)
                {
                    WorkTimeIntervalTO interval = intervals[0];

                    if (firstPair.StartTime.TimeOfDay < interval.EarliestArrived.TimeOfDay)
                    {
                        interval.EarliestArrived = interval.EarliestArrived;
                        interval.StartTime = interval.EarliestArrived;
                        interval.LatestArrivaed = interval.EarliestArrived;
                        interval.EndTime = interval.EarliestLeft;
                        interval.LatestLeft = interval.EndTime;
                        interval.EarliestLeft = interval.EndTime;
                    }
                    if (firstPair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && firstPair.StartTime.TimeOfDay <= interval.LatestArrivaed.TimeOfDay)
                    {
                        interval.EndTime = new DateTime(firstPair.StartTime.Ticks + (interval.EndTime.Ticks - interval.StartTime.Ticks));
                        interval.LatestLeft = interval.EndTime;
                        interval.EarliestLeft = interval.EndTime;
                        interval.EarliestArrived = firstPair.StartTime;
                        interval.StartTime = firstPair.StartTime;
                        interval.LatestArrivaed = firstPair.StartTime;

                    }
                    if (firstPair.StartTime.TimeOfDay > interval.LatestArrivaed.TimeOfDay)
                    {
                        interval.EarliestArrived = interval.LatestArrivaed;
                        interval.StartTime = interval.LatestArrivaed;
                        interval.LatestArrivaed = interval.LatestArrivaed;
                        interval.EndTime = interval.LatestLeft;
                        interval.LatestLeft = interval.EndTime;
                        interval.EarliestLeft = interval.EndTime;
                    }
                }
                foreach (IOPairTO pair in dayPairs)
                {
                    foreach (WorkTimeIntervalTO interval in intervals)
                    {
                        if (interval.StartTime.TimeOfDay < interval.EarliestArrived.TimeOfDay)
                            interval.EarliestArrived = interval.StartTime;
                        if (interval.LatestLeft.TimeOfDay < interval.EndTime.TimeOfDay)
                            interval.LatestLeft = interval.EndTime;
                        //if whole pair is inside a interval add pair to trimed pairs list
                        if ((pair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay || pair.StartTime.TimeOfDay >= interval.StartTime.TimeOfDay) &&
                           (pair.EndTime.TimeOfDay <= interval.EndTime.TimeOfDay || pair.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay))
                        {
                            trimedPairs.Add(pair);
                        }
                        else if (pair.EndTime.TimeOfDay > interval.EarliestArrived.TimeOfDay && pair.StartTime.TimeOfDay < interval.EarliestArrived.TimeOfDay)
                        {
                            pair.StartTime = new DateTime(pair.StartTime.Year, pair.StartTime.Month, pair.StartTime.Day, interval.EarliestArrived.Hour, interval.EarliestArrived.Minute, 0);
                            //if (pair.StartTime.TimeOfDay < interval.LatestLeft.TimeOfDay &&
                            //      pair.EndTime.TimeOfDay > interval.LatestLeft.TimeOfDay)
                            //{
                            //    pair.EndTime = new DateTime(pair.EndTime.Year, pair.EndTime.Month, pair.EndTime.Day, interval.LatestLeft.Hour, interval.LatestLeft.Minute, 0);
                            //}
                            trimedPairs.Add(pair);
                        }
                        else if ((pair.StartTime.TimeOfDay < interval.LatestLeft.TimeOfDay &&
                       pair.EndTime.TimeOfDay > interval.LatestLeft.TimeOfDay) || pair.StartTime.TimeOfDay > interval.EarliestArrived.TimeOfDay)
                        {
                            //     pair.EndTime = new DateTime(pair.EndTime.Year, pair.EndTime.Month, pair.EndTime.Day, interval.LatestLeft.Hour, interval.LatestLeft.Minute, 0);
                            trimedPairs.Add(pair);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " EmployeePresenceType.trimPairsWithIntervals(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return trimedPairs;
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
    }
}