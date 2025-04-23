using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Resources;
using System.Globalization;

using Common;
using Util;
using TransferObjects;
using ReaderInterface;

namespace UI
{
    public partial class ExitPermissionsPreview : Form
    {
        //there is two way's of sorting: by employees or by days
        private int Sorting;
        const int employeesSorting = 0;
        const int daysSorting = 1;

        bool periodPreview;

        List<EmployeeTO> employees;
        DateTime fromDate;
        DateTime toDate;
        int passType;
        DebugLog log;

        // list of pairs for preview
        List<IOPairTO> ioPairList = new List<IOPairTO>();

        // list of Time Schema for selected Employee and selected Time Interval
        List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();

        // list of Time Schedule for one month
        List<EmployeeTimeScheduleTO> timeSchedule = new List<EmployeeTimeScheduleTO>();
        
        List<PassTypeTO> passTypes = new List<PassTypeTO>();

        // all Holidays, Key is Date, value is Holiday
        Dictionary<DateTime, HolidayTO> holidays = new Dictionary<DateTime,HolidayTO>();

        // all employee time schedules
        Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedules = new Dictionary<int,List<EmployeeTimeScheduleTO>>();

        // all time shemas
        List<WorkTimeSchemaTO> timeSchemas = new List<WorkTimeSchemaTO>();

        public const int startPosition = 3;
        const int yPosition = 3;
        public int currentPosition;
        const int offset = 0;

        List<DateTime> days;

        ResourceManager rm;
        private CultureInfo culture;
        ApplUserTO logInUser;

        ArrayList exitPermControls;
        string Description;

        public ExitPermissionsPreview(List<EmployeeTO> employeesList, DateTime from, DateTime to, int passType, int sorting, List<PassTypeTO> ptArray, string description)
        {
            InitializeComponent();
            currentPosition = startPosition;
            Sorting = sorting;
            this.employees = employeesList;
            this.fromDate = from;
            this.toDate = to;
            this.passType = passType;
            this.Description = description;
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(ExitPermissionsAddAdvanced).Assembly);
            setLanguage();
            this.CenterToScreen();

            passTypes = ptArray;
                     
            // get all time schemas
            timeSchemas = new TimeSchema().Search();

            periodPreview = true;

            days = new List<DateTime>();
            for (DateTime day = from; day <= to; day = day.AddDays(1))
            {
                days.Add(day);
            }
            exitPermControls = new ArrayList();
            logInUser = NotificationController.GetLogInUser();
            setControls();
        }

        public ExitPermissionsPreview(List<EmployeeTO> employeesList, List<DateTime> dates, int passType, int sorting, List<PassTypeTO> ptArray, string description)
        {
            InitializeComponent();
            currentPosition = startPosition;
            Sorting = sorting;
            this.employees = employeesList;
            this.fromDate = new DateTime();
            this.toDate = new DateTime();
            this.passType = passType;
            this.Description = description;
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(ExitPermissionsAddAdvanced).Assembly);
            setLanguage();
            this.CenterToScreen();

            passTypes = ptArray;
           
            // get all time schemas
            timeSchemas = new TimeSchema().Search();

            periodPreview = false;
            days = dates;
            exitPermControls = new ArrayList();
            logInUser = NotificationController.GetLogInUser();
            setControls();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsPreview.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setControls()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<int> employeesID = new List<int>();
                IOPair ioPair = new IOPair();

                foreach (EmployeeTO empl in employees)
                {
                    // Employee IDs
                    employeesID.Add(empl.EmployeeID);
                }
                int ioPairsNum = 0;
                if (periodPreview)
                {
                    ioPairsNum = ioPair.SearchForEmplPermCount(fromDate, toDate.AddDays(1), employeesID);
                }
                else
                {
                    ioPairsNum = ioPair.SearchForDaysArrayCount(days, employeesID);
                }

                if (ioPairsNum > 0)
                {
                    if (periodPreview)
                    {
                        // get all valid IO Pairs for selected employees and time interval
                        // get iopairs for one day more, because if employee start night shift in last day,
                        // io pairs are sorted by wu_name, empl_last_name, empl_first_name, io_pair_date ascending
                        ioPairList = ioPair.SearchForEmplPerm(fromDate, toDate.AddDays(1), employeesID);
                    }
                    else
                    {
                        ioPairList = ioPair.SearchForDaysArray(days, employeesID);
                    }
                    // Key is Employee ID, value is ArrayList of valid IO Pairs for that Employee
                    Dictionary<int, List<IOPairTO>> emplPairs = new Dictionary<int,List<IOPairTO>>();

                    foreach (int emplID in employeesID)
                    {
                        emplPairs.Add(emplID, new List<IOPairTO>());
                    }

                    // io pairs for particular employee will be sorted by io_pair_date ascending
                    for (int i = 0; i < ioPairList.Count; i++)
                    {
                        if (!emplPairs.ContainsKey(ioPairList[i].EmployeeID))
                            emplPairs.Add(ioPairList[i].EmployeeID, new List<IOPairTO>());
                        
                        emplPairs[ioPairList[i].EmployeeID].Add(ioPairList[i]);
                    }
                    if (periodPreview)
                    {
                        // get all time schedules for all employees for the given period of time
                        foreach (int emplID in employeesID)
                        {
                            emplTimeSchedules.Add(emplID, GetEmployeeTimeSchedules(emplID, fromDate, toDate.AddDays(1)));
                        }
                    }
                    else
                    {
                        // get all time schedules for all employees for the given period of time
                        foreach (int emplID in employeesID)
                        {
                            emplTimeSchedules.Add(emplID, GetEmployeeTimeSchedules(emplID, days));
                        }
                    }

                    if (Sorting == employeesSorting)
                    {
                        int employeeIDindex = -1;
                        foreach (int employeeID in employeesID)
                        {
                            employeeIDindex++;

                            //key is day, value is hashtable with intervals 
                            Dictionary<DateTime, Dictionary<int, WorkTimeIntervalTO>> daysIntervals = new Dictionary<DateTime,Dictionary<int,WorkTimeIntervalTO>>();
                            //key is day, value is arrayList with ioPairs 
                            Dictionary<DateTime, List<IOPairTO>> daysIOPairs = new Dictionary<DateTime,List<IOPairTO>>();
                            //list of days
                            List<DateTime> daysList = new List<DateTime>();
                            //time schemas
                            Dictionary<DateTime, WorkTimeSchemaTO> daysSchemas = new Dictionary<DateTime,WorkTimeSchemaTO>();

                            foreach (DateTime day in days)
                            {
                                daysList.Add(day);
                                bool isRegularSchema = true;

                                WorkTimeSchemaTO timeSchema = getActualTimeSchema(emplTimeSchedules[employeeID], day);
                                Dictionary<int, WorkTimeIntervalTO> edi = GetDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day, ref isRegularSchema);
                                if (edi == null) continue;
                                Dictionary<int, WorkTimeIntervalTO> employeeDayIntervals = new Dictionary<int,WorkTimeIntervalTO>();
                                IDictionaryEnumerator ediEnum = edi.GetEnumerator();
                                while (ediEnum.MoveNext())
                                {
                                    employeeDayIntervals.Add((int)ediEnum.Key, ((WorkTimeIntervalTO)ediEnum.Value).Clone());
                                }

                                List<IOPairTO> iopList = new List<IOPairTO>();
                                if (emplPairs.ContainsKey(employeeID))
                                    iopList = emplPairs[employeeID];
                                List<IOPairTO> edp = GetEmployeeDayPairs(iopList, isRegularSchema, day);
                                List<IOPairTO> employeeDayPairs = new List<IOPairTO>();
                                foreach (IOPairTO ioPairTO in edp)
                                {
                                    employeeDayPairs.Add(new IOPairTO(ioPairTO));
                                }
                                daysIntervals.Add(day, employeeDayIntervals);
                                daysIOPairs.Add(day, employeeDayPairs);
                                daysSchemas.Add(day, timeSchema);
                            }
                            ExitPermControl exitPermControl = new ExitPermControl(employees[employeeIDindex], passType, daysIntervals, daysIOPairs, daysList, passTypes, daysSchemas, Description);
                            if (exitPermControl.currentLoc != ExitPermControl.firstHeaderY)
                            {
                                this.exitPermPanel.Controls.Add(exitPermControl);
                                exitPermControl.Location = new Point(yPosition, currentPosition);
                                currentPosition += exitPermControl.Height;
                                currentPosition += offset;
                                exitPermControls.Add(exitPermControl);
                            }
                        }
                    }
                    if (Sorting == daysSorting)
                    {
                        foreach (DateTime day in days)
                        {

                            //key is employeeID, value is hashtable with intervals 
                            Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> emplIntervals = new Dictionary<int,Dictionary<int,WorkTimeIntervalTO>>();
                            //key is employeeID, value is arrayList with ioPairs 
                            Dictionary<int, List<IOPairTO>> emplIOPairs = new Dictionary<int,List<IOPairTO>>();
                            //key is employeeID value is time schema
                            Dictionary<int, WorkTimeSchemaTO> emplTimeSchema = new Dictionary<int,WorkTimeSchemaTO>();

                            int employeeIDindex = -1;
                            foreach (int employeeID in employeesID)
                            {
                                employeeIDindex++;

                                bool isRegularSchema = true;

                                WorkTimeSchemaTO timeSchema = getActualTimeSchema(emplTimeSchedules[employeeID], day);

                                Dictionary<int, WorkTimeIntervalTO> edi = GetDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day, ref isRegularSchema);
                                if (edi == null) continue;
                                Dictionary<int, WorkTimeIntervalTO> employeeDayIntervals = new Dictionary<int,WorkTimeIntervalTO>();
                                IDictionaryEnumerator ediEnum = edi.GetEnumerator();
                                while (ediEnum.MoveNext())
                                {
                                    employeeDayIntervals.Add((int)ediEnum.Key, ((WorkTimeIntervalTO)ediEnum.Value).Clone());
                                }

                                List<IOPairTO> iopList = new List<IOPairTO>();
                                if (emplPairs.ContainsKey(employeeID))
                                    iopList = emplPairs[employeeID];
                                List<IOPairTO> edp = GetEmployeeDayPairs(iopList, isRegularSchema, day);
                                List<IOPairTO> employeeDayPairs = new List<IOPairTO>();
                                foreach (IOPairTO ioPairTO in edp)
                                {
                                    employeeDayPairs.Add(new IOPairTO(ioPairTO));
                                }
                                emplIntervals.Add(employeeID, employeeDayIntervals);
                                emplIOPairs.Add(employeeID, employeeDayPairs);
                                emplTimeSchema.Add(employeeID, timeSchema);
                            }
                            ExitPermControl exitPermControl = new ExitPermControl(day, passType, emplIntervals, emplIOPairs, employees, passTypes, emplTimeSchema, Description);
                            if (exitPermControl.currentLoc != ExitPermControl.firstHeaderY)
                            {
                                this.exitPermPanel.Controls.Add(exitPermControl);
                                exitPermControl.Location = new Point(yPosition, currentPosition);
                                currentPosition += exitPermControl.Height;
                                currentPosition += offset;
                                exitPermControls.Add(exitPermControl);
                            }
                        }
                    }
                }//if ioPairsNumber = 0
                this.exitPermPanel.Focus();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsPreview.ExitPermissionsPreview_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private WorkTimeSchemaTO getActualTimeSchema(List<EmployeeTimeScheduleTO> employeeTimeScheduleList, DateTime day)
        {
            WorkTimeSchemaTO actualTimeSchema = null;
            try
            {
                // find actual time schedule for the day
                int timeScheduleIndex = -1;
                for (int scheduleIndex = 0; scheduleIndex < employeeTimeScheduleList.Count; scheduleIndex++)
                {
                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
                    if (day >= employeeTimeScheduleList[scheduleIndex].Date)
                    //&& (day.Month == ((EmployeesTimeSchedule) (employeeTimeScheduleList[scheduleIndex])).Date.Month))
                    {
                        timeScheduleIndex = scheduleIndex;
                    }
                }
                if (timeScheduleIndex == -1) return null;

                EmployeeTimeScheduleTO employeeTimeSchedule = employeeTimeScheduleList[timeScheduleIndex];

                // find actual time schema for the day
                
                foreach (WorkTimeSchemaTO timeSchema in timeSchemas)
                {
                    if (timeSchema.TimeSchemaID == employeeTimeSchedule.TimeSchemaID)
                    {
                        actualTimeSchema = timeSchema;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsPreview.ExitPermissionsPreview_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return actualTimeSchema;
        }
       
        /// <summary>
        /// gets list of employee's time schedules for the given period of time
        /// </summary>
        private List<EmployeeTimeScheduleTO> GetEmployeeTimeSchedules(int employeeID, DateTime from, DateTime to)
        {
            List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();

            DateTime date = from.Date;
            while ((date <= to.Date) || (date.Month == to.Month))
            {
                List<EmployeeTimeScheduleTO> timeSchedule = new EmployeesTimeSchedule().SearchMonthSchedule(employeeID, date);

                foreach (EmployeeTimeScheduleTO ets in timeSchedule)
                {
                    timeScheduleList.Add(ets);
                }

                date = date.AddMonths(1);
            }

            return timeScheduleList;
        }

        private List<EmployeeTimeScheduleTO> GetEmployeeTimeSchedules(int employeeID, List<DateTime> days)
        {
            List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();
            
            foreach (DateTime date in days)
            {
                List<EmployeeTimeScheduleTO> timeSchedule = new EmployeesTimeSchedule().SearchEmployeesSchedules(employeeID.ToString(), date, date.AddDays(1));

                foreach (EmployeeTimeScheduleTO ets in timeSchedule)
                {
                    timeScheduleList.Add(ets);
                }                
            }
            return timeScheduleList;
        }

        /// <summary>
        /// gets employee's working intervals for the given day
        /// </summary>
        private Dictionary<int, WorkTimeIntervalTO> GetDayTimeSchemaIntervals(List<EmployeeTimeScheduleTO> employeeTimeScheduleList, DateTime day, ref bool isRegularSchema)
        {
            // find actual time schedule for the day
            int timeScheduleIndex = -1;
            for (int scheduleIndex = 0; scheduleIndex < employeeTimeScheduleList.Count; scheduleIndex++)
            {
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                if (day >= employeeTimeScheduleList[scheduleIndex].Date)
                //&& (day.Month == ((EmployeesTimeSchedule) (employeeTimeScheduleList[scheduleIndex])).Date.Month))
                {
                    timeScheduleIndex = scheduleIndex;
                }
            }
            if (timeScheduleIndex == -1) return null;

            EmployeeTimeScheduleTO employeeTimeSchedule = employeeTimeScheduleList[timeScheduleIndex];

            // find actual time schema for the day
            WorkTimeSchemaTO actualTimeSchema = null;
            foreach (WorkTimeSchemaTO timeSchema in timeSchemas)
            {
                if (timeSchema.TimeSchemaID == employeeTimeSchedule.TimeSchemaID)
                {
                    actualTimeSchema = timeSchema;
                    break;
                }
            }
            if (actualTimeSchema == null) return null;

            /* 2008-03-14
             * From now one, take the last existing time schedule, don't expect that every month has 
             * time schedule*/
            //int dayNum = (employeeTimeSchedule.StartCycleDay + day.Day - employeeTimeSchedule.Date.Day) % actualTimeSchema.CycleDuration;
            TimeSpan ts = new TimeSpan(day.Date.Ticks - employeeTimeSchedule.Date.Date.Ticks);
            int dayNum = (employeeTimeSchedule.StartCycleDay + (int)ts.TotalDays) % actualTimeSchema.CycleDuration;

            Dictionary<int, WorkTimeIntervalTO> intervals = actualTimeSchema.Days[dayNum];

            isRegularSchema = isRegularTimeSchema(intervals);

            return intervals;
        }
        /// <summary>
        /// Gets all the employee's io pairs for the given working day
        /// </summary>
        private List<IOPairTO> GetEmployeeDayPairs(List<IOPairTO> emplPairs, bool isRegularSchema, DateTime day)
        {
            List<IOPairTO> employeeDayPairs = new List<IOPairTO>();
            foreach (IOPairTO iopair in emplPairs)
            {
                if ((isRegularSchema && iopair.IOPairDate == day) || (iopair.IOPairDate == day &&
                    iopair.StartTime.TimeOfDay > new TimeSpan(7, 0, 0)))
                {
                    employeeDayPairs.Add(iopair);
                }

                // pairs that belong to the tomorrow's part of the night shift (00:00-07:00)
                if (!isRegularSchema && (iopair.IOPairDate == day.AddDays(1) &&
                    iopair.StartTime.TimeOfDay < new TimeSpan(7, 0, 0)))
                {
                    employeeDayPairs.Add(iopair);
                }
            }

            return employeeDayPairs;
        }

        bool isRegularTimeSchema(Dictionary<int, WorkTimeIntervalTO> dayIntervals)
        {
            // see if the day is a night shift day (contains intervals starting with 00:00 and/or finishing with 23:59)
            foreach (WorkTimeIntervalTO dayInterval in dayIntervals.Values)
            {
                if (((dayInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0)) ||
                    (dayInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))) &&
                    (dayInterval.EndTime > dayInterval.StartTime))
                {
                    return false;
                }
            }
            return true;
        }

        private void setLanguage()
        {
            this.Text = rm.GetString("exitPermPreview", culture);

            //button's text
            this.btnCancel.Text = rm.GetString("btnCancel", culture);
            this.btnSave.Text = rm.GetString("btnSave", culture);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool saved = true;

                foreach (ExitPermControl exitControl in exitPermControls)
                {
                    int i = -1;
                    foreach (ExitPermRowControl row in exitControl.permissionsList)
                    {
                        ExitPermRowControl exitPerm = new ExitPermRowControl();
                        if (i > -1 && i < exitControl.permissionsList.Count - 1)
                        { 
                           exitPerm =(ExitPermRowControl)exitControl.permissionsList[i] ;
                        }
                        //if row is checked insret permission
                        if (row.insert)
                        {
                            ExitPermissionTO exitPermission = row.currentPerm;
                            exitPermission.IssuedBy = logInUser.UserID;
                            exitPermission.Used = (int)Constants.Used.Yes;
                            exitPermission.VerifiedBy = logInUser.UserID;

                           //if permission is for start of the shift
                            if (row.PermissionType.Equals(ExitPermControl.isStart) || row.PermissionType.Equals(ExitPermControl.isShiftStart))
                            {
                                // find first pass for that day and show it if it is IN and is after beginning of working day
                                DateTime timeFrom = new DateTime(1900, 1, 1, 0, 0, 0);
                                DateTime timeTo = new DateTime(1900, 1, 1, 23, 59, 0);
                                DateTime laestArrivedTime = row.latestArrivalTime;
                                DateTime date = new DateTime(exitPermission.StartTime.Year, exitPermission.StartTime.Month, exitPermission.StartTime.Day);
                                Pass pass = new Pass();
                                pass.PssTO.EmployeeID = exitPermission.EmployeeID;
                                List<PassTO> passes = pass.SearchInterval(date, date, "", timeFrom, timeTo);

                                pass = new Pass();
                                pass.PssTO.EventTime = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                                bool passOnStartNotExist = true;
                                foreach (PassTO p in passes)
                                {
                                    if ((p.IsWrkHrsCount == 1) && p.Direction.Equals(Constants.DirectionIn) && p.EventTime.TimeOfDay <= laestArrivedTime.TimeOfDay)
                                    {
                                        passOnStartNotExist = false;
                                        break;
                                    }
                                }
                                if(passOnStartNotExist)
                                {
                                    PassTO passIN = new PassTO(-1, exitPermission.EmployeeID, Constants.DirectionIn,
                                              exitPermission.StartTime, (int)Constants.PassType.Work,
                                            (int)Constants.PairGenUsed.Unused, Constants.defaultLocID,
                                            (int)Constants.recordCreated.Automaticaly,
                                            (int)Constants.IsWrkCount.IsCounter);

                                    DateTime eventTime = exitPermission.StartTime.AddSeconds(1);
                                    PassTO passOUT = new PassTO(-1, exitPermission.EmployeeID,
                                        Constants.DirectionOut, eventTime, exitPermission.PassTypeID,
                                        (int)Constants.PairGenUsed.Unused, Constants.defaultLocID,
                                        (int)Constants.recordCreated.Automaticaly,
                                        (int)Constants.IsWrkCount.IsCounter);

                                    List<PassTO> passTOList = new List<PassTO>();
                                    passTOList.Add(passIN);
                                    passTOList.Add(passOUT);
                                    int inserted = new Pass().SavePassesPermission(passTOList, exitPermission, logInUser.UserID.Trim());
                                    saved = saved && (inserted > 0);
                                }
                                else
                                {
                                    saved = false;
                                }
                            }
                            else
                            {
                                // select all passes for selected Employee and Date
                                List<PassTO> passes = new Pass().SearchPassesForExitPerm(exitPermission.EmployeeID,  exitPermission.StartTime);

                                if (passes.Count > 0)
                                {
                                    PassTO passTO = passes[0];
                                    // save Exit Permission and update selected Pass with selected Pass Type
                                    if (passTO.PassID >= 0)
                                    {
                                        passTO.PassTypeID = exitPermission.PassTypeID;
                                        passTO.PairGenUsed = (int)Constants.PairGenUsed.Unused;

                                        bool savedRetroactive = new ExitPermission().SaveRetroactive(exitPermission.EmployeeID, exitPermission.PassTypeID, exitPermission.StartTime,
                                            exitPermission.Offset, exitPermission.Used, exitPermission.Description,
                                            passTO, exitPermission.VerifiedBy);

                                        saved = saved && savedRetroactive;
                                    }
                                }
                                else if (exitPerm!= new ExitPermRowControl()&& exitPerm.passTypeId < -100)
                                {
                                    PassTO passOUT = new PassTO(-1, exitPermission.EmployeeID,
                                    Constants.DirectionOut, exitPermission.StartTime, exitPermission.PassTypeID,
                                    (int)Constants.PairGenUsed.Unused, Constants.defaultLocID,
                                    (int)Constants.recordCreated.Automaticaly,
                                    (int)Constants.IsWrkCount.IsCounter);
                                    List<PassTO> passTOList = new List<PassTO>();
                                    passTOList.Add(passOUT);
                                    int inserted = new Pass().SavePassesPermission(passTOList, exitPermission, logInUser.UserID.Trim());
                                }
                                else
                                {
                                    saved = false;
                                }
                            }
                        }

                        i++;                        
                    }
                }
                if (saved)
                {
                    MessageBox.Show(rm.GetString("exitPermSaved", culture));
                    this.Close();
                }
                else
                {
                    MessageBox.Show(rm.GetString("someExitPermNotSaved", culture));
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsPreview.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void ExitPermissionsPreview_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsPreview.ExitPermissionsPreview_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        
        private void ExitPermissionsPreview_MouseMove(object sender, MouseEventArgs e)
        {
            this.exitPermPanel.Focus();
        }

        private void ExitPermissionsPreview_Load(object sender, EventArgs e)
        {
            this.exitPermPanel.Select();
        }
    }
}