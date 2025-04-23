using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Resources;
using System.Globalization;
using System.Collections;
using Common;
using Util;
using TransferObjects;

namespace Reports.PIO
{
    public partial class PIOWorkingSaturdays : Form
    {
        ApplUserTO logInUser;
        ResourceManager rm;
        private CultureInfo culture;
        DebugLog log;

        public PIOWorkingSaturdays()
        {
            try
            {
                InitializeComponent();

                // Init debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();
                this.CenterToScreen();

                // Language tool
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(PIOWorkingSaturdays).Assembly);
                setLanguage();

                dtpDate.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PIOWorkingSaturdays.PIOWorkingSaturdays(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Set proper language
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("pioWorkingSaturdays", culture);

                // button's text
                btnCalculate.Text = rm.GetString("btnCalculate", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);

                // label's text
                lblDate.Text = rm.GetString("date", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PIOWorkingSaturdays.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                IOPair ioPair = new IOPair();
                List<DateTime> datesList = new List<DateTime>();
                datesList.Add(dtpDate.Value);

                //get all ioPairs (including open) for all employees and selected date
                List<IOPairTO> ioPairsList = ioPair.SearchAllEmplDatePairs("", datesList);

                //calculate pause for this ioPairs
                calculatePauseForNotWorkingDay(ioPairsList, dtpDate.Value);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PIOWorkingSaturdays.btnCalculate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }

            this.Close();
        }

        //ioPairList must be Sort by employee_id, io_pair_date, start_time
        //and contains all io_pairs (including open pairs)
        //Pause is calculate only for not working days
        private void calculatePauseForNotWorkingDay(List<IOPairTO> ioPairList, DateTime date)
        {
            if (ioPairList.Count > 0)
            {
                //list of time schedules for all employees, for selected interval
                List<EmployeeTimeScheduleTO> timeSchedules = new List<EmployeeTimeScheduleTO>();

                // all employee time schedules for selected Time Interval, key is employee ID
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedules = new Dictionary<int,List<EmployeeTimeScheduleTO>>();

                // all time shemas
                List<WorkTimeSchemaTO> timeSchemas = new TimeSchema().Search();

                //all pauses
                List<TimeSchemaPauseTO> pauses = new TimeSchemaPause().Search("");

                DateTime fromDay = date;
                DateTime toDay = date;

                ArrayList employeesID = new ArrayList();
                string employeeIDString = "";

                // Key is Employee ID, value is ArrayList of valid IO Pairs for that Employee
                Dictionary<int, List<IOPairTO>> emplPairs = new Dictionary<int,List<IOPairTO>>();

                IOPair ioPair = new IOPair();

                // io pairs for particular employee are sorted by io_pair_date ascending
                for (int i = 0; i < ioPairList.Count; i++)
                {
                    int currentEmployeeID = ioPairList[i].EmployeeID;
                    if (!emplPairs.ContainsKey(currentEmployeeID))
                    {
                        employeesID.Add(currentEmployeeID);
                        employeeIDString += currentEmployeeID.ToString().Trim() + ",";

                        emplPairs.Add(currentEmployeeID, new List<IOPairTO>());
                        emplTimeSchedules.Add(currentEmployeeID, new List<EmployeeTimeScheduleTO>());
                    }

                    emplPairs[currentEmployeeID].Add(ioPairList[i]);
                }
                if (employeeIDString.Length > 0)
                {
                    employeeIDString = employeeIDString.Substring(0, employeeIDString.Length - 1);
                }

                //get time schemas for selected Employees, for selected Time Interval
                timeSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedules(employeeIDString, fromDay, toDay);
                for (int i = 0; i < timeSchedules.Count; i++)
                {
                    emplTimeSchedules[timeSchedules[i].EmployeeID].Add(timeSchedules[i]);
                }

                int emplNotWorkingDayCount = 0;
                int emplOpenPairsCount = 0;
                int emplNotHavePauseCount = 0;
                int emplPauseDurIsZero = 0;
                int emplPausEntryNotSucceed = 0;
                int emplPausEntrySucceed = 0;
                int emplNoTimeSchema = 0;
                int emplWorkingDayCount = 0;
                int emplNoPauseCandidatesCount = 0;
                int emplCantDeletePauseCount = 0;
                //Start calculation for each employee
                foreach (int employeeID in employeesID)
                {
                    if (emplPairs[employeeID].Count <= 0)
                        continue;

                    //get time schema for specific day, for Employee and take dayNum in that schema for specific day.
                    int dayNum = -1;
                    WorkTimeSchemaTO actualTimeSchema = Common.Misc.getTimeSchemaForDayAndDayNum(emplTimeSchedules[employeeID],
                        date, timeSchemas, ref dayNum);

                    if (actualTimeSchema == null)
                    {
                        emplNoTimeSchema++;
                        continue;
                    }
                    if (dayNum < 0)
                    {
                        emplNoTimeSchema++;
                        continue;
                    }

                    Dictionary<int, WorkTimeIntervalTO> dayIntervals = actualTimeSchema.Days[dayNum];

                    if (dayIntervals == null)
                    {
                        emplNoTimeSchema++;
                        continue;
                    }

                    //PIO has only one interval
                    WorkTimeIntervalTO tsInterval = dayIntervals[0]; //key is interval_num which is 0, 1...

                    //Pause is calculate only for not working days
                    if ((tsInterval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                        || (tsInterval.EndTime.TimeOfDay != new TimeSpan(0, 0, 0)))
                    {
                        emplWorkingDayCount++;
                        continue;
                    }
                    emplNotWorkingDayCount++;

                    //do not calculate pause if day has at least one open interval
                    bool openPairExist = ioPair.existEmployeeDayOpenPairs(emplPairs[employeeID]);
                    if (openPairExist)
                    {
                        emplOpenPairsCount++;
                        continue;
                    }

                    WorkTimeIntervalTO tsIntervalForPauseID = null;
                    for (int counter = 0; counter < actualTimeSchema.Days.Count; counter++)
                    {
                        Dictionary<int, WorkTimeIntervalTO> dayIntervalsTemp = actualTimeSchema.Days[counter];
                        if (dayIntervalsTemp == null)
                            continue;

                        if (((dayIntervalsTemp[0].StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                            || (dayIntervalsTemp[0].EndTime.TimeOfDay != new TimeSpan(0, 0, 0)))
                            && (dayIntervalsTemp[0].PauseID != -1))
                        {
                            tsIntervalForPauseID = dayIntervalsTemp[0]; //key is interval_num which is 0, 1...
                            break;
                        }
                    }
                    if (tsIntervalForPauseID == null)
                    {
                        emplNotHavePauseCount++;
                        continue;
                    }

                    //get pause informations
                    int duration = -1;
                    int tolerancy = -1;
                    int shortBreak = -1;
                    DateTime pauseEarliestTime = new DateTime(0);
                    DateTime pauseLatestTime = new DateTime(0);
                    int pauseID = tsIntervalForPauseID.PauseID;

                    foreach (TimeSchemaPauseTO currentPause in pauses)
                    {
                        if (pauseID == currentPause.PauseID)
                        {
                            duration = currentPause.PauseDuration;
                            tolerancy = currentPause.PauseOffset;
                            shortBreak = currentPause.ShortBreakDuration;

                            WorkTimeIntervalTO newInterval = new WorkTimeIntervalTO();

                            //calculate pause latest use time
                            newInterval.EndTime = new DateTime(date.Year, date.Month, date.Day);
                            newInterval.EndTime = newInterval.EndTime.Add(tsIntervalForPauseID.EndTime.TimeOfDay);
                            pauseLatestTime = newInterval.EndTime.Subtract(new TimeSpan(0, currentPause.LatestUseTime, 0));

                            //calculate pause earliest use time
                            newInterval.StartTime = new DateTime(date.Year, date.Month, date.Day);
                            newInterval.StartTime = newInterval.StartTime.Add(tsIntervalForPauseID.StartTime.TimeOfDay);
                            pauseEarliestTime = newInterval.StartTime.AddMinutes(currentPause.EarliestUseTime);
                            break;
                        }
                    }

                    bool succeed = true;
                    List<IOPairTO> intervalIoPairList = deleteExistingPauses(emplPairs[employeeID], ref succeed);
                    if (!succeed)
                    {
                        emplCantDeletePauseCount++;
                        continue;
                    }
                    List<IOPairTO> intervalIoPairListClone = new List<IOPairTO>();

                    foreach (IOPairTO iop in intervalIoPairList)
                    {
                        intervalIoPairListClone.Add(iop);
                    }

                    //make new ioPairList (trimmedIoPairs) considering overlaping of intervals
                    List<IOPairTO> trimmedIoPairs = new List<IOPairTO>();
                    ArrayList equalPairsID = new ArrayList();
                    foreach (IOPairTO iopairTO in intervalIoPairList)
                    {
                        if (!equalPairsID.Contains(iopairTO.IOPairID))
                        {
                            IOPairTO newioPair = new IOPairTO(iopairTO);
                            newioPair.IOPairID = 0;
                            newioPair.IsWrkHrsCount = 0;
                            newioPair.LocationID = 0;
                            newioPair.LocationName = "";
                            newioPair.ManualCreated = 0;
                            newioPair.PassType = "";
                            newioPair.PassTypeID = 0;
                            newioPair.WUName = "";

                            bool insideOtherPair = false;
                            foreach (IOPairTO iopairTOClone in intervalIoPairListClone)
                            {
                                if (iopairTOClone.IOPairID != iopairTO.IOPairID)
                                {
                                    if ((iopairTOClone.StartTime.TimeOfDay == iopairTO.StartTime.TimeOfDay)
                                        && (iopairTOClone.EndTime.TimeOfDay == iopairTO.EndTime.TimeOfDay))
                                    {
                                        //register that for iopairTO pair exist iopairTOClone pair with the same
                                        //start and end time, so, do not do calculation again for iopairTOClone
                                        equalPairsID.Add(iopairTOClone.IOPairID);
                                    }

                                    if ((iopairTOClone.StartTime.TimeOfDay <= iopairTO.StartTime.TimeOfDay)
                                        && (iopairTOClone.EndTime.TimeOfDay >= iopairTO.EndTime.TimeOfDay)
                                        && !((iopairTOClone.StartTime.TimeOfDay == iopairTO.StartTime.TimeOfDay)
                                        && (iopairTOClone.EndTime.TimeOfDay == iopairTO.EndTime.TimeOfDay)))
                                    {
                                        //iopairTO pair is inside samo other iopairTOClone pair
                                        insideOtherPair = true;
                                        break;
                                    }

                                    if ((iopairTOClone.StartTime.TimeOfDay < iopairTO.StartTime.TimeOfDay)
                                        && (iopairTOClone.EndTime.TimeOfDay >= iopairTO.StartTime.TimeOfDay)
                                        && (iopairTOClone.StartTime.TimeOfDay < newioPair.StartTime.TimeOfDay))
                                    {
                                        //iopairTO and iopairTOClone pairs are overlaping, adjust start time
                                        newioPair.StartTime = iopairTOClone.StartTime;
                                    }

                                    if ((iopairTOClone.StartTime.TimeOfDay <= iopairTO.EndTime.TimeOfDay)
                                        && (iopairTOClone.EndTime.TimeOfDay > iopairTO.EndTime.TimeOfDay)
                                        && (iopairTOClone.EndTime.TimeOfDay > newioPair.EndTime.TimeOfDay))
                                    {
                                        //iopairTO and iopairTOClone pairs are overlaping, adjust end time
                                        newioPair.EndTime = iopairTOClone.EndTime;
                                    }
                                } //if (iopairTOClone.IOPairID != iopairTO.IOPairID)
                            } //foreach (TransferObjects.IOPairTO iopairTOClone in intervalIoPairListClone)

                            if (insideOtherPair)
                                continue;

                            if (trimmedIoPairs.Contains(newioPair))
                                continue;

                            //check if this new pair already exist in trimmedIoPairs, or maybe it
                            //overlaps with some pair there
                            //Do necesery changes acording to that
                            bool existInTrimStart = false;
                            bool existInTrimEnd = false;
                            foreach (TransferObjects.IOPairTO iopairTOTrimm in trimmedIoPairs)
                            {
                                if ((iopairTOTrimm.StartTime.TimeOfDay > newioPair.StartTime.TimeOfDay)
                                    && (iopairTOTrimm.StartTime.TimeOfDay <= newioPair.EndTime.TimeOfDay))
                                {
                                    iopairTOTrimm.StartTime = newioPair.StartTime;
                                    existInTrimStart = true;
                                }

                                if ((iopairTOTrimm.EndTime.TimeOfDay >= newioPair.StartTime.TimeOfDay)
                                    && (iopairTOTrimm.EndTime.TimeOfDay < newioPair.EndTime.TimeOfDay))
                                {
                                    iopairTOTrimm.EndTime = newioPair.EndTime;
                                    existInTrimEnd = true;
                                }

                                if ((iopairTOTrimm.StartTime.TimeOfDay <= newioPair.StartTime.TimeOfDay)
                                    && (iopairTOTrimm.EndTime.TimeOfDay >= newioPair.EndTime.TimeOfDay))
                                {
                                    existInTrimStart = true;
                                    existInTrimEnd = true;
                                }
                            }

                            if (existInTrimStart || existInTrimEnd)
                                continue;

                            trimmedIoPairs.Add(newioPair);
                        } //if (!equalPairsID.Contains(iopairTO.IOPairID))
                    } //foreach (TransferObjects.IOPairTO iopairTO in intervalIoPairList)

                    if (trimmedIoPairs.Count < 2)
                    {
                        emplNoPauseCandidatesCount++;
                        continue;
                    }

                    //since iopairs are sorted by start_time, trimmedIOPAirs are also sorted, so, holes are between them
                    //holes will be new IOPairs

                    //create short breaks first
                    if (shortBreak > 0)
                        trimmedIoPairs = ioPair.insertShortBreaks(employeeID, date, trimmedIoPairs, shortBreak, false, null);

                    if (trimmedIoPairs.Count < 2)
                    {
                        emplNoPauseCandidatesCount++;
                        continue;
                    }

                    if (duration <= 0)
                    {
                        emplPauseDurIsZero++;
                        continue;
                    }

                    //create candidates for holes
                    List<IOPairTO> holesList = new List<IOPairTO>();
                    for (int j = 1; j < trimmedIoPairs.Count; j++)
                    {
                        IOPairTO newioPair = new IOPairTO();
                        newioPair.StartTime = trimmedIoPairs[j - 1].EndTime;
                        newioPair.EndTime = trimmedIoPairs[j].StartTime;

                        if (newioPair.StartTime.Date < pauseEarliestTime.Date)
                            continue;
                        if (newioPair.EndTime.Date > pauseLatestTime.Date)
                            continue;

                        //adjust start time according to pause earliest use time
                        if ((newioPair.StartTime.TimeOfDay < pauseEarliestTime.TimeOfDay)
                            && (newioPair.EndTime.TimeOfDay > pauseEarliestTime.TimeOfDay)
                            && (newioPair.StartTime.Date == pauseEarliestTime.Date))
                            newioPair.StartTime = pauseEarliestTime;

                        //adjust end time according to pause latest use time
                        if ((newioPair.EndTime.TimeOfDay > pauseLatestTime.TimeOfDay)
                            && (newioPair.StartTime.TimeOfDay < pauseLatestTime.TimeOfDay)
                            && (newioPair.EndTime.Date == pauseLatestTime.Date))
                            newioPair.EndTime = pauseLatestTime;

                        if ((newioPair.StartTime.TimeOfDay < pauseEarliestTime.TimeOfDay)
                            && (newioPair.StartTime.Date == pauseEarliestTime.Date))
                            continue;

                        if ((newioPair.EndTime.TimeOfDay > pauseLatestTime.TimeOfDay)
                            && (newioPair.EndTime.Date == pauseLatestTime.Date))
                            continue;

                        holesList.Add(newioPair);
                    }

                    if (holesList.Count <= 0)
                    {
                        emplNoPauseCandidatesCount++;
                        continue;
                    }

                    //find appropriate hole for pause
                    DateTime startPause = new DateTime(0);
                    DateTime endPause = new DateTime(0);
                    double curentDuration = 0;
                    foreach (IOPairTO iopairHole in holesList)
                    {
                        TimeSpan holeDuration = (new DateTime(iopairHole.EndTime.Ticks - iopairHole.StartTime.Ticks)).TimeOfDay;

                        if ((holeDuration.TotalMinutes >= (double)duration)
                            && (holeDuration.TotalMinutes <= (double)(duration + tolerancy)))
                        {
                            if (holeDuration.TotalMinutes > curentDuration)
                            {
                                startPause = iopairHole.StartTime;
                                endPause = iopairHole.EndTime;
                                curentDuration = holeDuration.TotalMinutes;
                            }
                        }
                    }

                    if (curentDuration == 0)
                    {
                        foreach (IOPairTO iopairHole in holesList)
                        {
                            TimeSpan holeDuration = (new DateTime(iopairHole.EndTime.Ticks - iopairHole.StartTime.Ticks)).TimeOfDay;

                            if (holeDuration.TotalMinutes > (double)(duration + tolerancy))
                            {
                                if ((curentDuration == 0) || (holeDuration.TotalMinutes < curentDuration))
                                {
                                    startPause = iopairHole.StartTime;
                                    endPause = iopairHole.StartTime.AddMinutes((double)duration);
                                    curentDuration = holeDuration.TotalMinutes;
                                }
                            }
                        }
                    }

                    if (curentDuration == 0)
                    {
                        foreach (IOPairTO iopairHole in holesList)
                        {
                            TimeSpan holeDuration = (new DateTime(iopairHole.EndTime.Ticks - iopairHole.StartTime.Ticks)).TimeOfDay;

                            if (holeDuration.TotalMinutes > curentDuration)
                            {
                                startPause = iopairHole.StartTime;
                                endPause = iopairHole.EndTime;
                                curentDuration = holeDuration.TotalMinutes;
                            }
                        }
                    }

                    int insertedCount = -1;
                    bool inserted = false;
                    insertedCount = (new IOPair()).Save(-1, date.Date, employeeID, Constants.defaultLocID, Constants.automaticPausePassType, (int)Constants.IsWrkCount.IsCounter,
                            startPause, endPause,-1, (int)Constants.recordCreated.Automaticaly, true);
                    inserted = ((insertedCount > 0) ? true : false);

                    if (!inserted)
                    {
                        emplPausEntryNotSucceed++;
                        MessageBox.Show("Can't insert pause for employee: " + employeeID.ToString() + "\n");
                    }
                    else
                        emplPausEntrySucceed++;

                } //foreach(int employeeID in employeesID)

                if (emplNotWorkingDayCount > 0)
                {
                    string emplNotWorkingDayCountMess = rm.GetString("emplNotWorkingDayCountMess", culture) + " "
                        + emplNotWorkingDayCount.ToString() + "\n";
                    string emplPausEntrySucceedMess = (emplPausEntrySucceed == 0) ? "" :
                        (rm.GetString("emplPausEntrySucceedMess", culture) + " " + emplPausEntrySucceed.ToString() + "\n");
                    string emplPausEntryNotSucceedMess = (emplPausEntryNotSucceed == 0) ? "" :
                        (rm.GetString("emplPausEntryNotSucceedMess", culture) + " " + emplPausEntryNotSucceed.ToString() + "\n");
                    string emplNoTimeSchemaMess = (emplNoTimeSchema == 0) ? "" :
                       (rm.GetString("emplNoTimeSchemaMess", culture) + " " + emplNoTimeSchema.ToString() + "\n");
                    string emplWorkingDayCountMess = (emplWorkingDayCount == 0) ? "" :
                        (rm.GetString("emplWorkingDayCountMess", culture) + " " + emplWorkingDayCount.ToString() + "\n");
                    string emplOpenPairsCountMess = (emplOpenPairsCount == 0) ? "" :
                        (rm.GetString("emplOpenPairsCountMess", culture) + " " + emplOpenPairsCount.ToString() + "\n");
                    string emplNotHavePauseCountMess = (emplNotHavePauseCount == 0) ? "" :
                        (rm.GetString("emplNotHavePauseCountMess", culture) + " " + emplNotHavePauseCount.ToString() + "\n");
                    string emplPauseDurIsZeroMess = (emplPauseDurIsZero == 0) ? "" :
                        (rm.GetString("emplPauseDurIsZeroMess", culture) + " " + emplPauseDurIsZero.ToString() + "\n");
                    string emplNoPauseCandidatesCountMess = (emplNoPauseCandidatesCount == 0) ? "" :
                        (rm.GetString("emplNoPauseCandidatesCountMess", culture) + " " + emplNoPauseCandidatesCount.ToString() + "\n");
                    string emplCantDeletePauseCountMess = (emplCantDeletePauseCount == 0) ? "" :
                        (rm.GetString("emplCantDeletePauseCountMess", culture) + " " + emplCantDeletePauseCount.ToString() + "\n");
                    
                    MessageBox.Show(emplNotWorkingDayCountMess + emplPausEntrySucceedMess + 
                        emplPausEntryNotSucceedMess + emplNoTimeSchemaMess +
                        emplWorkingDayCountMess + emplOpenPairsCountMess +
                        emplNotHavePauseCountMess + emplPauseDurIsZeroMess +
                        emplNoPauseCandidatesCountMess + emplCantDeletePauseCountMess);
                }
                else
                    MessageBox.Show(rm.GetString("noCandidatesForPause", culture));

            } //if (ioPairList.Count > 0)
            else
                MessageBox.Show(rm.GetString("noIoPairsForDay", culture));
        }

        private List<IOPairTO> deleteExistingPauses(List<IOPairTO> emplIOPairList, ref bool succeed)
        {
            List<IOPairTO> cleanEmplIOPairs = new List<IOPairTO>();
            IOPair ioPair = new IOPair();
            succeed = true;
            for (int i = 0; i < emplIOPairList.Count; i++)
            {
                int passTypeID = emplIOPairList[i].PassTypeID;
                if ((passTypeID == Constants.automaticPausePassType)
                    || (passTypeID == Constants.automaticShortBreakPassType))
                {
                    succeed = ioPair.Delete(emplIOPairList[i].IOPairID, emplIOPairList[i].IOPairDate) && succeed;
                }
                else
                    cleanEmplIOPairs.Add(emplIOPairList[i]);
            }

            return cleanEmplIOPairs;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}