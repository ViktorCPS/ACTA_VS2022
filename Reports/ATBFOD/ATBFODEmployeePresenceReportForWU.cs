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
using System.IO;

using Common;
using TransferObjects;
using Util;

namespace Reports.ATBFOD
{
    public partial class ATBFODEmployeePresenceReportForWU : Form
    {
        CultureInfo culture;
        ResourceManager rm;
        ApplUserTO logInUser;

        DebugLog debug;
        List<WorkingUnitTO> wUnits;
        DateTime day;

        Filter filter;

        private static List<WorkTimeIntervalTO> nightShiftIntervals
        {
            get
            {
                List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                WorkTimeIntervalTO tsi = new WorkTimeIntervalTO();
                tsi.StartTime = new DateTime(1, 1, 1, 23, 0, 0);
                tsi.EndTime = new DateTime(1, 1, 1, 23, 59, 0);
                intervals.Add(tsi);
                tsi = new WorkTimeIntervalTO();
                tsi.StartTime = new DateTime(1, 1, 1, 0, 0, 0);
                tsi.EndTime = new DateTime(1, 1, 1, 7, 0, 0);
                intervals.Add(tsi);
                return intervals;
            }
        }
        private static List<WorkTimeIntervalTO> secondShiftIntervals
        {
            get
            {
                List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                WorkTimeIntervalTO tsi = new WorkTimeIntervalTO();
                tsi.StartTime = new DateTime(1, 1, 1, 15, 0, 0);
                tsi.EndTime = new DateTime(1, 1, 1, 23, 0, 0);
                intervals.Add(tsi);
                return intervals;
            }
        }                

        public ATBFODEmployeePresenceReportForWU()
        {
            InitializeComponent();

            // Init debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(WorkingUnitsReports).Assembly);
            setLanguage();
            logInUser = NotificationController.GetLogInUser();
            populateWorkigUnitCombo();

            day = DateTime.Now.Date;
            dtpDate.Value = day;
            this.CenterToScreen();
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
                debug.writeLog(DateTime.Now + " ATBFODEmployeePresenceReportForWU.populateWorkigUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("ATBFODworkingTimeReport", culture);

                gbWorkingUnit.Text = rm.GetString("workingUnits", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
                lblWorkingUnitName.Text = rm.GetString("lblName", culture);
                chbHierarhicly.Text = rm.GetString("hierarchically", culture);
                btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);
                gbDate.Text = rm.GetString("gbDate", culture);
                lblDate.Text = rm.GetString("lblDate", culture);            
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PaymentReports.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                debug.writeLog(DateTime.Now + " ATBFODEmployeePresenceReportForWU.btnGenerateReport_Click() \n");
                this.Cursor = Cursors.WaitCursor;

                //get working units's
                if (wUnits.Count == 0)
                {
                    MessageBox.Show(rm.GetString("noWUGranted", culture));
                }
                else
                {
                    int selectedWorkingUnit = (int)cbWorkingUnit.SelectedValue;

                    if (this.chbHierarhicly.Checked)
                    {
                        WorkingUnit wu = new WorkingUnit();
                        if (selectedWorkingUnit != -1)
                        {
                            wu.WUTO.WorkingUnitID = selectedWorkingUnit;
                            wUnits = wu.Search();
                        }
                        else
                        {
                            if (selectedWorkingUnit == -1)
                            {
                                for (int i = wUnits.Count - 1; i >= 0; i--)
                                {
                                    if (wUnits[i].WorkingUnitID == wUnits[i].ParentWorkingUID)
                                    {
                                        wUnits.RemoveAt(i);
                                    }
                                }
                            }
                        }
                        wUnits = wu.FindAllChildren(wUnits);
                        selectedWorkingUnit = -1;
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

                    // list of pairs for report
                    List<IOPairTO> ioPairList = new List<IOPairTO>();

                    //find employees for selected working unit
                    List<EmployeeTO> EmployeeListWU = new List<EmployeeTO>();
                    if (this.chbHierarhicly.Checked||selectedWorkingUnit==-1)
                    {
                        EmployeeListWU = new Employee().SearchByWU(wuString);

                    }
                    else
                    {
                        EmployeeListWU = new Employee().SearchByWU(selectedWorkingUnit.ToString());
                    }

                    //get all employee's for selected wu
                    List<EmployeeTO> EmplList = EmployeeListWU;
                    EmployeeListWU = new List<EmployeeTO>();
                    List<int> employeeListID = new List<int>();

                    foreach (EmployeeTO employee in EmplList)
                    {
                        EmployeeListWU.Add(employee);
                        employeeListID.Add(employee.EmployeeID);
                    }

                    //get IOPairs for selected WU and date, and next day 
                    int count = ioPair.SearchEmplDateCount(dtpDate.Value, dtpDate.Value.AddDays(1), employeeListID);
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

                    // get all valid IO Pairs for selected working unit and date
                    ioPairList = ioPair.SearchAll(dtpDate.Value, dtpDate.Value.AddDays(1), employeeListID);

                    // get Time Schemas for selected Employee and selected date
                    day = dtpDate.Value.Date;
                    string employeeIDString = "";
                    foreach (EmployeeTO empl in EmployeeListWU)
                    {
                        employeeIDString = employeeIDString + empl.EmployeeID.ToString() + ",";
                    }
                    if (employeeIDString.Length > 0)
                    {
                        employeeIDString = employeeIDString.Substring(0, employeeIDString.Length - 1);
                    }
                    
                    // all time shemas
                    List<WorkTimeSchemaTO> timeSchemas = new TimeSchema().Search();

                    // get all holidays
                    List<HolidayTO> holidayList = new Holiday().Search(new DateTime(), new DateTime());
                    
                    // Key is Pass Type Id, Value is Pass Type IsPass
                    Dictionary<int, int> passTypes = new Dictionary<int,int>();
                    List<PassTypeTO> passTypesAll = new PassType().Search();
                    foreach (PassTypeTO pt in passTypesAll)
                    {
                        passTypes.Add(pt.PassTypeID, pt.IsPass);
                    }

                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("ATBFOD_employee_analytical");

                    tableCR.Columns.Add("working_unit_id", typeof(int));
                    tableCR.Columns.Add("employee_id", typeof(int));
                    tableCR.Columns.Add("last_name", typeof(System.String));
                    tableCR.Columns.Add("first_name", typeof(System.String));
                    tableCR.Columns.Add("earlyest_arrival", typeof(System.String));
                    tableCR.Columns.Add("latest_left", typeof(System.String));
                    tableCR.Columns.Add("sick_leave", typeof(System.String));
                    tableCR.Columns.Add("vacation", typeof(System.String));
                    tableCR.Columns.Add("late", typeof(System.String));
                    tableCR.Columns.Add("night_work", typeof(System.String));
                    tableCR.Columns.Add("payed_absence", typeof(System.String));
                    tableCR.Columns.Add("holiday", typeof(System.String));
                    tableCR.Columns.Add("over_time", typeof(System.String));
                    tableCR.Columns.Add("early", typeof(System.String));
                    tableCR.Columns.Add("secund_shift", typeof(System.String));
                    tableCR.Columns.Add("extra_hours", typeof(System.String));
                    tableCR.Columns.Add("plus_extra_hours", typeof(System.String));
                    tableCR.Columns.Add("minus_extra_hours", typeof(System.String));
                    tableCR.Columns.Add("regular_work", typeof(System.String));
                    tableCR.Columns.Add("business_trip", typeof(System.String));
                    tableCR.Columns.Add("imageID", typeof(byte));

                    DataTable tableI = new DataTable("images");
                    tableI.Columns.Add("image", typeof(System.Byte[]));
                    tableI.Columns.Add("imageID", typeof(byte));

                    int counter = 0;
                    foreach (EmployeeTO empl in EmployeeListWU)
                    {
                        bool hasPair = false;

                        //values for CR
                        TimeSpan sickLeave = new TimeSpan();
                        TimeSpan vacation = new TimeSpan();
                        TimeSpan payedAbsence = new TimeSpan();
                        TimeSpan businessTrip = new TimeSpan();
                        TimeSpan late = new TimeSpan();
                        TimeSpan early = new TimeSpan();
                        TimeSpan holidayTS = new TimeSpan();
                        TimeSpan extraHours = new TimeSpan();                       
                        TimeSpan earlyestArrive = new TimeSpan();
                        DateTime earlyestArriveDate = new DateTime();
                        TimeSpan latestLeft = new TimeSpan();
                        DateTime latestLeftDate = new DateTime();

                        //list of io pairs for current employee
                        List<IOPairTO> employeeIOPairs = new List<IOPairTO>();
                        foreach (IOPairTO iopairTO in ioPairList)
                        {
                            if (iopairTO.EmployeeID == empl.EmployeeID)
                            {
                                employeeIOPairs.Add(iopairTO);
                            }
                        }

                        //get time schemas for current Employee, for selected date
                        List<EmployeeTimeScheduleTO> timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(empl.EmployeeID.ToString(), dtpDate.Value, dtpDate.Value);

                        bool is2DaysShift = false;
                        bool is2DaysShiftPrevious = false;
                        WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                       
                        //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                        //are night shift days. If day is night shift day, also take first interval of next day
                        Dictionary<int, WorkTimeIntervalTO> dayIntervals = Common.Misc.getDayTimeSchemaIntervals(timeScheduleList, day, ref is2DaysShift,
                            ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);

                        if (dayIntervals != null)
                        {
                            List<WorkTimeIntervalTO> employeeIntervals = Common.Misc.getEmployeeDayIntervals(is2DaysShift, is2DaysShiftPrevious, firstIntervalNextDay, dayIntervals);

                            //Take only IO Pairs for specific day, considering night shifts
                            List<IOPairTO> dayIOPairList = Common.Misc.getEmployeeDayPairs(employeeIOPairs, day, is2DaysShift, is2DaysShiftPrevious, firstIntervalNextDay, dayIntervals);

                            foreach (IOPairTO iopairTO in dayIOPairList)
                            {
                                hasPair = true;

                                // sickLeave
                                if (Constants.sickLeaveTable.Contains(iopairTO.PassTypeID))
                                    sickLeave = new TimeSpan(sickLeave.Ticks + (iopairTO.EndTime.Ticks - iopairTO.StartTime.Ticks));

                                // vacation
                                if (Constants.vacationTable.Contains(iopairTO.PassTypeID))
                                    vacation = new TimeSpan(vacation.Ticks + (iopairTO.EndTime.Ticks - iopairTO.StartTime.Ticks));

                                // payedAbsence
                                if (Constants.payedAbsenceTable.Contains(iopairTO.PassTypeID))
                                    payedAbsence = new TimeSpan(payedAbsence.Ticks + (iopairTO.EndTime.Ticks - iopairTO.StartTime.Ticks));

                                // businessTrip
                                if (Constants.businessTripTable.Contains(iopairTO.PassTypeID))
                                    businessTrip = new TimeSpan(businessTrip.Ticks + (iopairTO.EndTime.Ticks - iopairTO.StartTime.Ticks));

                                // extraHours
                                if (Constants.extraHoursTable.Contains(iopairTO.PassTypeID))
                                    extraHours = new TimeSpan(extraHours.Ticks + (iopairTO.EndTime.Ticks - iopairTO.StartTime.Ticks));

                                //if pass is on reader count earlyest arrival and earlier going away
                                if (passTypes[iopairTO.PassTypeID] == Constants.passOnReader)
                                {
                                    // Find IOPair with earliest arrival 
                                    if ((earlyestArriveDate.Equals(new DateTime())) || (earlyestArriveDate > iopairTO.StartTime))
                                    {
                                        earlyestArrive = iopairTO.StartTime.TimeOfDay;
                                        earlyestArriveDate = iopairTO.StartTime;
                                    }
                                    // Earlier going away 
                                    if ((latestLeftDate.Equals(new DateTime())) || (latestLeftDate < iopairTO.EndTime))
                                    {
                                        latestLeft = iopairTO.EndTime.TimeOfDay;
                                        latestLeftDate = iopairTO.EndTime;
                                    }
                                }
                            }

                            //get all ioPairs on reader to count working time
                            List<IOPairTO> jobIOPairs = this.getJobIOPairs(dayIOPairList, passTypes);
                            //calculate how many hours employee did work on specific day
                            TimeSpan jobBeforeTrim = Common.Misc.getEmployeeJobHourDay(jobIOPairs);
                            TimeSpan job = new TimeSpan();
                            if (employeeIntervals != null)
                            {
                                //Do not take time before and after shift
                                List<IOPairTO> trimIOPairs = this.trimPairs(jobIOPairs, employeeIntervals);

                                //calculate how many hours employee did work in his shift   
                                job = Common.Misc.getEmployeeJobHourDay(trimIOPairs);

                                //If day is holiday add schedule hours to holiday time span                       
                                foreach (HolidayTO holiday in holidayList)
                                {
                                    if (holiday.HolidayDate.Date.Equals(day.Date))
                                    {
                                        holidayTS = Common.Misc.getEmployeeScheduleHourDay(employeeIntervals);
                                        hasPair = true;
                                    }
                                }
                            }

                            //calculate work before and after shift
                            TimeSpan overtime = jobBeforeTrim - job;

                            //trim IOPairs for nigh work calculation
                            List<IOPairTO> nightIOPairs = this.trimPairs(jobIOPairs, nightShiftIntervals);
                            TimeSpan nightWork = Common.Misc.getEmployeeJobHourDay(nightIOPairs);

                            //trim IOPairs for second shift
                            List<IOPairTO> secondShiftIOPairs = this.trimPairs(jobIOPairs, secondShiftIntervals);
                            TimeSpan secondShiftWork = Common.Misc.getEmployeeJobHourDay(secondShiftIOPairs);

                            // if employee has at least one ioPair on selected day                                                                 
                            if (hasPair == true)
                            {
                                WorkTimeIntervalTO firstTimeSchemaInterval = new WorkTimeIntervalTO();
                                WorkTimeIntervalTO lastTimeSchemaInterval = new WorkTimeIntervalTO();
                                if (employeeIntervals != null)
                                {
                                    firstTimeSchemaInterval = employeeIntervals[0];

                                    if (earlyestArrive != new TimeSpan() && earlyestArrive > firstTimeSchemaInterval.StartTime.TimeOfDay)
                                    {
                                        late = new TimeSpan(earlyestArrive.Ticks - firstTimeSchemaInterval.StartTime.TimeOfDay.Ticks);
                                    }
                                    //last interval for calculate early
                                    lastTimeSchemaInterval = employeeIntervals[employeeIntervals.Count - 1];
                                    if (latestLeft != new TimeSpan() && latestLeft < lastTimeSchemaInterval.EndTime.TimeOfDay)
                                    {
                                        early = new TimeSpan(lastTimeSchemaInterval.EndTime.TimeOfDay.Ticks - latestLeft.Ticks);
                                    }
                                }
                                DataRow rowCR = tableCR.NewRow();
                                
                                // One record in table
                                ArrayList row = new ArrayList();
                                rowCR["working_unit_id"] = empl.WorkingUnitID;
                                rowCR["last_name"] = empl.LastName;
                                rowCR["first_name"] = empl.FirstName + " " + empl.LastName;
                                rowCR["employee_id"] = empl.EmployeeID;

                                if (earlyestArrive != new TimeSpan())
                                {
                                    rowCR["earlyest_arrival"] = earlyestArriveDate.ToString("HH:mm");
                                }
                                else
                                {
                                    rowCR["earlyest_arrival"] = "";
                                }

                                if (latestLeft != new TimeSpan())
                                {
                                    rowCR["latest_left"] = latestLeftDate.ToString("HH:mm"); 
                                }
                                else
                                {
                                    rowCR["latest_left"] = "";
                                }

                                if (sickLeave != new TimeSpan())
                                {
                                    DateTime sickLeaveDT = new DateTime(sickLeave.Ticks);
                                    rowCR["sick_leave"] = sickLeaveDT.ToString("HH:mm");
                                }
                                else
                                {
                                    rowCR["sick_leave"] = "";
                                }

                                if (vacation != new TimeSpan())
                                {
                                    DateTime vactionDT = new DateTime(vacation.Ticks);
                                    rowCR["vacation"] = vactionDT.ToString("HH:mm");
                                }
                                else
                                {
                                    rowCR["vacation"] = "";
                                }

                                if (late != new TimeSpan())
                                {
                                    DateTime lateDT = new DateTime(late.Ticks);
                                    rowCR["late"] = lateDT.ToString("HH:mm");
                                }
                                else
                                {
                                    rowCR["late"] = "";
                                }

                                if (nightWork != new TimeSpan())
                                {
                                    DateTime nightWorkDT = new DateTime(nightWork.Ticks);
                                    rowCR["night_work"] = nightWorkDT.ToString("HH:mm");
                                }
                                else
                                {
                                    rowCR["night_work"] = "";
                                }                               

                                if (payedAbsence != new TimeSpan())
                                {
                                    DateTime payedAbsenceDT = new DateTime(payedAbsence.Ticks);
                                    rowCR["payed_absence"] = payedAbsenceDT.ToString("HH:mm");
                                }
                                else
                                {
                                    rowCR["payed_absence"] = "";
                                }

                                if (holidayTS != new TimeSpan())
                                {
                                    DateTime holidayDT = new DateTime(holidayTS.Ticks);
                                    rowCR["holiday"] = holidayDT.ToString("HH:mm");
                                }

                                if (overtime != new TimeSpan())
                                {
                                    DateTime overTimeDT = new DateTime(overtime.Ticks);
                                    rowCR["over_time"] = overTimeDT.ToString("HH:mm");
                                }
                                else
                                {
                                    rowCR["over_time"] = "";
                                }

                                if (early != new TimeSpan())
                                {
                                    DateTime earlyDT = new DateTime(early.Ticks);
                                    rowCR["early"] = earlyDT.ToString("HH:mm");
                                }
                                else
                                {
                                    rowCR["early"] = "";
                                }
                                if (secondShiftWork != new TimeSpan())
                                {
                                    DateTime secondShiftWorkDT = new DateTime(secondShiftWork.Ticks);
                                    rowCR["secund_shift"] = secondShiftWorkDT.ToString("HH:mm");
                                }
                                else
                                {
                                    rowCR["secund_shift"] = "";
                                }
                               
                                if (extraHours != new TimeSpan())
                                {
                                    DateTime extraHoursDT = new DateTime(extraHours.Ticks);
                                    rowCR["extra_hours"] = extraHoursDT.ToString("HH:mm");
                                }
                                else
                                {
                                    rowCR["extra_hours"] = "";
                                }
                                //need to be implement
                                rowCR["plus_extra_hours"] = "";
                                //need to be implement
                                rowCR["minus_extra_hours"] = "";

                                if (job != new TimeSpan())
                                {
                                    DateTime jobDT = new DateTime(job.Ticks);
                                    rowCR["regular_work"] = jobDT.ToString("HH:mm");
                                }
                                else
                                {
                                    rowCR["regular_work"] = "";
                                }

                                if (businessTrip != new TimeSpan())
                                {
                                    DateTime businessTripDT = new DateTime(businessTrip.Ticks);
                                    rowCR["business_trip"] = businessTripDT.ToString("HH:mm");
                                }
                                else
                                {
                                    rowCR["business_trip"] = "";
                                }


                                rowCR["imageID"] = 1;
                                if (counter == 0)
                                {
                                    //add logo image just once
                                    DataRow rowI = tableI.NewRow();
                                    rowI["image"] = Constants.LogoForReport;
                                    rowI["imageID"] = 1;
                                    tableI.Rows.Add(rowI);
                                    tableI.AcceptChanges();
                                }

                                tableCR.Rows.Add(rowCR);
                                counter++;
                            }
                        } 
                    }//foreach(Employee empl in EmployeeListWU)                   


                    dataSetCR.Tables.Add(tableCR);
                    dataSetCR.Tables.Add(tableI);
                    
                    if (counter == 0)
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("dataNotFound", culture));
                        return;
                    }
                    else
                    {
                        this.generateAnalyticalWUCRReport(dataSetCR);
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " ATBFODEmployeePresenceReportForWU.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private List<IOPairTO> getJobIOPairs(List<IOPairTO> dayIOPairList, Dictionary<int, int> passTypes)
        {
            List<IOPairTO> jobIOPairs = new List<IOPairTO>();

            try
            {
                foreach (IOPairTO iopairTO in dayIOPairList)
                {
                    if (passTypes[iopairTO.PassTypeID] == Constants.passOnReader && iopairTO.PassTypeID!=Constants.privateOut)
                    {
                        jobIOPairs.Add(iopairTO);
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " ATBFODEmployeePresenceReportForWU.getJobIOPairs(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return jobIOPairs;
        }

        private void generateAnalyticalWUCRReport(DataSet dataCR)
        {
            try
            {
                DataTable table = dataCR.Tables["ATBFOD_employee_analytical"];
                
                if (table.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("dataNotFound", culture));
                    return;
                }
                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                {
                    ATBFOD_sr.ATBFODEmployeeAnalyticalCRView view = new ATBFOD_sr.ATBFODEmployeeAnalyticalCRView(
                        dataCR, dtpDate.Value, cbWorkingUnit.SelectedText.Trim());
                    view.ShowDialog(this);
                }
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                {
                    ATBFOD_en.ATBFODEmployeeAnalyticalCRView_en view = new ATBFOD_en.ATBFODEmployeeAnalyticalCRView_en(
                      dataCR, dtpDate.Value, cbWorkingUnit.SelectedText.ToString());
                    view.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " ATBFODEmployeePresenceReportForWU.generateAnalyticalWUCRReport(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        //trim IO pairs and cut pairs so it fit to schema intervals
        private List<IOPairTO> trimPairs(List<IOPairTO> dayIOPairList, List<WorkTimeIntervalTO> dayIntervalsList)
        {
            List<IOPairTO> ioPairs = new List<IOPairTO>();
            try
            {
                foreach (IOPairTO iopair in dayIOPairList)
                {
                    IOPairTO ioPairTO = new IOPairTO(iopair);

                    foreach (WorkTimeIntervalTO timeSchemaInterval in dayIntervalsList)
                    {

                        if (ioPairTO.StartTime.TimeOfDay < timeSchemaInterval.StartTime.TimeOfDay
                            && ioPairTO.EndTime.TimeOfDay <= timeSchemaInterval.EndTime.TimeOfDay
                            && ioPairTO.EndTime.TimeOfDay >= timeSchemaInterval.StartTime.TimeOfDay)
                        {
                            ioPairTO.StartTime = ioPairTO.StartTime.Add(ioPairTO.StartTime.TimeOfDay.Negate() + timeSchemaInterval.StartTime.TimeOfDay);
                            ioPairs.Add(ioPairTO);
                        }

                        else if (ioPairTO.EndTime.TimeOfDay > timeSchemaInterval.EndTime.TimeOfDay
                            && ioPairTO.StartTime.TimeOfDay <= timeSchemaInterval.EndTime.TimeOfDay
                            && ioPairTO.StartTime.TimeOfDay >= timeSchemaInterval.StartTime.TimeOfDay)
                        {
                            ioPairTO.EndTime = ioPairTO.EndTime.Add(ioPairTO.EndTime.TimeOfDay.Negate() + timeSchemaInterval.EndTime.TimeOfDay);
                            ioPairs.Add(ioPairTO);
                        }
                        else if (ioPairTO.StartTime.TimeOfDay >= timeSchemaInterval.StartTime.TimeOfDay && ioPairTO.EndTime.TimeOfDay <= timeSchemaInterval.EndTime.TimeOfDay)
                        {
                            ioPairs.Add(ioPairTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " ATBFODEmployeePresenceReportForWU.trimPairs(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return ioPairs;
        }

        private void ATBFODEmployeePresenceReportForWU_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                filter = new Filter();
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
    }
}