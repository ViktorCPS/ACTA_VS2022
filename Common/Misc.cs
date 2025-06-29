using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Util;
using System.Data;
using iTextSharp.text.pdf;
using iTextSharp.text;


namespace Common
{
    /// <summary>
    /// This class containes miscellaneous functions.
    /// </summary>
    public class Misc
    {
        public Misc()
        {
        }

        #region Locking functions
        public static bool isLockedDate(DateTime date)
        {
            try
            {
                DateTime lastLock = lastLockDate();
                if (date <= lastLock && lastLock != new DateTime())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DateTime lastLockDate()
        {
            DateTime lastLockDate = new DateTime();
            try
            {
                Lock lastLock = new Lock();
                if (NotificationController.GetLastLocking() == null)
                {
                    NotificationController.SetLastLocking();
                }
                lastLock = NotificationController.GetLastLocking();

                if (lastLock.Type.Equals(Constants.lockTypeLock))
                {
                    lastLockDate = lastLock.LockDate.Date;
                }
                else if (lastLock.Type.Equals(Constants.lockTypeUnlock))
                {
                    lastLockDate = lastLock.LockDate.Date.AddDays(-1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lastLockDate;
        }
        public static string dataLockedMessage(DateTime dateChanging)
        {
            string str = "";
            try
            {
                str = dateChanging.Date.ToString("dd.MM.yyyy") + " <= " + Misc.lastLockDate().Date.ToString("dd.MM.yyy") + " LOCKED";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return str;
        }
        #endregion

        #region Working Time functions

        //get intervals for selected employee, for specific day
        //timeScheduleList contains Time Schemas for selected Employee and selected Time Interval
        //date is specific day
        //is2DaysShift is logical value - is shift for this day actually 2 days shift. It is for specific employee
        //is2DaysShiftPrevious is logical value - is shift for previous schema day (according to specific day) actually 2 days shift. It is for specific schema
        //firstIntervalNextDay - first interval for next schema day (according to specific day)
        //timeSchemas - all time shemas
        public static Dictionary<int, WorkTimeIntervalTO> getDayTimeSchemaIntervals(List<EmployeeTimeScheduleTO> timeScheduleList, DateTime date, ref bool is2DaysShift,
            ref bool is2DaysShiftPrevious, ref WorkTimeIntervalTO firstIntervalNextDay, List<WorkTimeSchemaTO> timeSchemas)
        {
            // find actual time schedule for the day
            WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
            int timeScheduleIndex = -1;
            for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
            {
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                /*
                if (date >= timeScheduleList[scheduleIndex].Date)
                //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
                {
                    timeScheduleIndex = scheduleIndex;
                }
                */
                //21.09.2017 Miodrag / Gledace prvu najnoviju smenu.
                if (date >= timeScheduleList[scheduleIndex].Date)
                {
                    timeScheduleIndex = scheduleIndex;
                }
                else
                {
                    break;
                }
                //MM
            }
            if (timeScheduleIndex == -1) return null;

            EmployeeTimeScheduleTO employeeTimeSchedule = timeScheduleList[timeScheduleIndex];

            // find actual time schema for the day
            WorkTimeSchemaTO actualTimeSchema = null;
            foreach (WorkTimeSchemaTO currentTimeSchema in timeSchemas)
            {
                if (currentTimeSchema.TimeSchemaID == employeeTimeSchedule.TimeSchemaID)
                {
                    actualTimeSchema = currentTimeSchema;
                    break;
                }
            }
            if (actualTimeSchema == null) return null;

            /* 2008-03-14
             * From now one, take the last existing time schedule, don't expect that every month has 
             * time schedule*/
            //int dayNum = (employeeTimeSchedule.StartCycleDay + date.Day - employeeTimeSchedule.Date.Day) % actualTimeSchema.CycleDuration;
            TimeSpan ts = new TimeSpan(date.Date.Ticks - employeeTimeSchedule.Date.Date.Ticks);
            int dayNum = (employeeTimeSchedule.StartCycleDay + (int)ts.TotalDays) % actualTimeSchema.CycleDuration;

            //19.09.2017 Miodrag / Postojala je mogucnost da je dayNum negativan.
            if (dayNum < 0)
            {
                dayNum = actualTimeSchema.CycleDuration + dayNum;
            }
            //mm

            is2DaysShift = false;
            for (int i = 0; i < actualTimeSchema.Days[dayNum].Count; i++)
            {                
                    WorkTimeIntervalTO currentTimeSchemaInterval = actualTimeSchema.Days[dayNum][i]; //key is interval_num which is 0, 1...
                    if ((currentTimeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                        && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                    {
                        is2DaysShift = true;
                        break;
                    }               
            }

            if (is2DaysShift)
            {
                is2DaysShift = false;
                int nextDay = dayNum + 1;
                if (nextDay >= actualTimeSchema.CycleDuration)
                    nextDay = 0;

                for (int i = 0; i < actualTimeSchema.Days[nextDay].Count; i++)
                {
                    WorkTimeIntervalTO currentTimeSchemaInterval = actualTimeSchema.Days[nextDay][i]; //key is interval_num which is 0, 1...
                    if ((currentTimeSchemaInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                        && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                    {
                        is2DaysShift = true;
                        firstIntervalNextDay = currentTimeSchemaInterval;
                        break;
                    }
                }
            }

            is2DaysShiftPrevious = false;
            for (int i = 0; i < actualTimeSchema.Days[dayNum].Count; i++)
            {
                WorkTimeIntervalTO currentTimeSchemaInterval = actualTimeSchema.Days[dayNum][i]; //key is interval_num which is 0, 1...
                if ((currentTimeSchemaInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                    && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                {
                    is2DaysShiftPrevious = true;
                    break;
                }
            }

            if (is2DaysShiftPrevious)
            {
                is2DaysShiftPrevious = false;
                int previousDay = dayNum - 1;
                if (previousDay < 0)
                    previousDay = actualTimeSchema.CycleDuration - 1;
                for (int i = 0; i < actualTimeSchema.Days[previousDay].Count; i++)
                {
                    WorkTimeIntervalTO currentTimeSchemaInterval = actualTimeSchema.Days[previousDay][i]; //key is interval_num which is 0, 1...
                    if ((currentTimeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                        && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                    {
                        is2DaysShiftPrevious = true;
                        break;
                    }
                }
            }

            return actualTimeSchema.Days[dayNum];
        }


        //get intervals for selected employee, for specific day
        //timeScheduleList contains Time Schemas for selected Employee and selected Time Interval
        //date is specific day
        //is2DaysShift is logical value - is shift for this day actually 2 days shift. It is for specific employee
        //is2DaysShiftPrevious is logical value - is shift for previous schema day (according to specific day) actually 2 days shift. It is for specific schema
        //firstIntervalNextDay - first interval for next schema day (according to specific day)
        //timeSchemas - all time shemas
        public static Dictionary<int, WorkTimeIntervalTO> getDayTimeSchemaIntervals(List<EmployeeTimeScheduleTO> timeScheduleList, DateTime date, ref bool is2DaysShift,
            ref bool is2DaysShiftPrevious, ref WorkTimeIntervalTO firstIntervalNextDay, ref WorkTimeSchemaTO actualTimeSchema, Dictionary<int, WorkTimeSchemaTO> timeSchemas)
        {
            // find actual time schedule for the day
            WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
            int timeScheduleIndex = -1;
            for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
            {
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
           //ja     if (date >= timeScheduleList[scheduleIndex].Date)
                //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
           //ja     {
                //21.09.2017 Miodrag / Gledace prvu najnoviju smenu.
                if (date >= timeScheduleList[scheduleIndex].Date)
                {
                    timeScheduleIndex = scheduleIndex;
                }
                else
                {
                    break;
                }
                //MM
          //ja      }
            }
            if (timeScheduleIndex == -1) return null;

            EmployeeTimeScheduleTO employeeTimeSchedule = timeScheduleList[timeScheduleIndex];

            // find actual time schema for the day
            actualTimeSchema = null;
            if (timeSchemas.ContainsKey(employeeTimeSchedule.TimeSchemaID))
                actualTimeSchema = timeSchemas[employeeTimeSchedule.TimeSchemaID];

            if (actualTimeSchema == null) return null;

            /* 2008-03-14
             * From now one, take the last existing time schedule, don't expect that every month has 
             * time schedule*/
            //int dayNum = (employeeTimeSchedule.StartCycleDay + date.Day - employeeTimeSchedule.Date.Day) % actualTimeSchema.CycleDuration;
            TimeSpan ts = new TimeSpan(date.Date.Ticks - employeeTimeSchedule.Date.Date.Ticks);
            int dayNum = (employeeTimeSchedule.StartCycleDay + (int)ts.TotalDays) % actualTimeSchema.CycleDuration;
            
            //19.09.2017 Miodrag / Postojala je mogucnost da je dayNum negativan.
            if (dayNum < 0)
            {
                dayNum = actualTimeSchema.CycleDuration + dayNum;
            }
            //mm
            

            is2DaysShift = false;
            for (int i = 0; i < actualTimeSchema.Days[dayNum].Count; i++)
            {
                WorkTimeIntervalTO currentTimeSchemaInterval = actualTimeSchema.Days[dayNum][i]; //key is interval_num which is 0, 1...
                if ((currentTimeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                    && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                {
                    is2DaysShift = true;
                    break;
                }
            }

            if (is2DaysShift)
            {
                is2DaysShift = false;
                //do not check next day of same schema 
                //int nextDay = dayNum + 1;
                //if (nextDay >= actualTimeSchema.CycleDuration)
                //    nextDay = 0;

                //for (int i = 0; i < actualTimeSchema.Days[nextDay].Count; i++)
                //{
                //    WorkTimeIntervalTO currentTimeSchemaInterval = actualTimeSchema.Days[nextDay][i]; //key is interval_num which is 0, 1...
                //    if ((currentTimeSchemaInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                //        && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                //    {
                //        is2DaysShift = true;
                //        firstIntervalNextDay = currentTimeSchemaInterval;
                //        break;
                //    }
                //}

                //find time schema for next day and check next day 
                // find actual time schedule for the day
                WorkTimeIntervalTO interval1 = new WorkTimeIntervalTO();
                int timeScheduleIndex1 = -1;
                DateTime dateNext = date.AddDays(1);
                for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
                {
                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
                    if (dateNext >= timeScheduleList[scheduleIndex].Date)
                    //&& (dateNext.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
                    {
                        timeScheduleIndex1 = scheduleIndex;
                    }
                }
                if (timeScheduleIndex1 != -1)
                {
                    EmployeeTimeScheduleTO employeeTimeSchedule1 = timeScheduleList[timeScheduleIndex1];

                    // find actual time schema for the day
                    WorkTimeSchemaTO actualTimeSchema1 = null;
                    if (timeSchemas.ContainsKey(employeeTimeSchedule1.TimeSchemaID))
                        actualTimeSchema1 = timeSchemas[employeeTimeSchedule1.TimeSchemaID];

                    if (actualTimeSchema1 != null)
                    {

                        /* 2008-03-14
                         * From now one, take the last existing time schedule, don't expect that every month has 
                         * time schedule*/
                        //int dayNum = (employeeTimeSchedule1.StartCycleDay + dateNext.Day - employeeTimeSchedule1.Date.Day) % actualTimeSchema1.CycleDuration;
                        TimeSpan ts1 = new TimeSpan(dateNext.Date.Ticks - employeeTimeSchedule1.Date.Date.Ticks);
                        int dayNum1 = (employeeTimeSchedule1.StartCycleDay + (int)ts1.TotalDays) % actualTimeSchema1.CycleDuration;

                        for (int i = 0; i < actualTimeSchema1.Days[dayNum1].Count; i++)
                        {
                            WorkTimeIntervalTO currentTimeSchemaInterval = actualTimeSchema1.Days[dayNum1][i]; //key is interval_num which is 0, 1...
                            if ((currentTimeSchemaInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                            {
                                is2DaysShift = true;
                                firstIntervalNextDay = currentTimeSchemaInterval;
                                break;
                            }
                        }
                    }
                }
            }

            is2DaysShiftPrevious = false;
            for (int i = 0; i < actualTimeSchema.Days[dayNum].Count; i++)
            {
                WorkTimeIntervalTO currentTimeSchemaInterval = actualTimeSchema.Days[dayNum][i]; //key is interval_num which is 0, 1...
                if ((currentTimeSchemaInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                    && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                {
                    is2DaysShiftPrevious = true;
                    break;
                }
            }

            if (is2DaysShiftPrevious)
            {
                is2DaysShiftPrevious = false;
                //int previousDay = dayNum - 1;
                //if (previousDay < 0)
                //    previousDay = actualTimeSchema.CycleDuration - 1;
                //for (int i = 0; i < actualTimeSchema.Days[previousDay].Count; i++)
                //{
                //    WorkTimeIntervalTO currentTimeSchemaInterval = actualTimeSchema.Days[previousDay][i]; //key is interval_num which is 0, 1...
                //    if ((currentTimeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                //        && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                //    {
                //        is2DaysShiftPrevious = true;
                //        break;
                //    }
                //}
                //find time schema for next day and check next day 
                // find actual time schedule for the day
                WorkTimeIntervalTO interval1 = new WorkTimeIntervalTO();
                int timeScheduleIndex1 = -1;
                DateTime dateNext = date.AddDays(-1);
                for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
                {
                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
                    if (dateNext >= timeScheduleList[scheduleIndex].Date)
                    //&& (dateNext.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
                    {
                        timeScheduleIndex1 = scheduleIndex;
                    }
                }
                if (timeScheduleIndex1 != -1)
                {
                    EmployeeTimeScheduleTO employeeTimeSchedule1 = timeScheduleList[timeScheduleIndex1];

                    // find actual time schema for the day
                    WorkTimeSchemaTO actualTimeSchema1 = null;
                    if (timeSchemas.ContainsKey(employeeTimeSchedule1.TimeSchemaID))
                        actualTimeSchema1 = timeSchemas[employeeTimeSchedule1.TimeSchemaID];

                    if (actualTimeSchema1 != null)
                    {
                        /* 2008-03-14
                         * From now one, take the last existing time schedule, don't expect that every month has 
                         * time schedule*/
                        //int dayNum = (employeeTimeSchedule1.StartCycleDay + dateNext.Day - employeeTimeSchedule1.Date.Day) % actualTimeSchema1.CycleDuration;
                        TimeSpan ts1 = new TimeSpan(dateNext.Date.Ticks - employeeTimeSchedule1.Date.Date.Ticks);
                        int dayNum1 = (employeeTimeSchedule1.StartCycleDay + (int)ts1.TotalDays) % actualTimeSchema1.CycleDuration;

                        for (int i = 0; i < actualTimeSchema1.Days[dayNum1].Count; i++)
                        {
                            WorkTimeIntervalTO currentTimeSchemaInterval = actualTimeSchema1.Days[dayNum1][i]; //key is interval_num which is 0, 1...
                            if ((currentTimeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                            {
                                is2DaysShiftPrevious = true;
                                break;
                            }
                        }
                    }
                }
            }

            return actualTimeSchema.Days[dayNum];
        }

        //get intervals for selected employee, for specific day
        //timeScheduleList contains Time Schemas for selected Employee and selected Time Interval
        //date is specific day
        //is2DaysShift is logical value - is shift for this day actually 2 days shift. It is for specific employee
        //is2DaysShiftPrevious is logical value - is shift for previous schema day (according to specific day) actually 2 days shift. It is for specific schema
        //firstIntervalNextDay - first interval for next schema day (according to specific day)
        //timeSchemas - all time shemas
        public static Dictionary<int, WorkTimeIntervalTO> getDayTimeSchemaIntervals(List<EmployeeTimeScheduleTO> timeScheduleList, DateTime date, ref bool is2DaysShift,
            ref bool is2DaysShiftPrevious, ref WorkTimeIntervalTO firstIntervalNextDay, Dictionary<int, WorkTimeSchemaTO> timeSchemas)
        {
            // find actual time schedule for the day
            WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
            int timeScheduleIndex = -1;
            for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
            {
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
               /* mm
                if (date >= timeScheduleList[scheduleIndex].Date)
                //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
                {
                    timeScheduleIndex = scheduleIndex;
                }
       mm         */
                //21.09.2017 Miodrag / Gledace prvu najnoviju smenu.
                if (date >= timeScheduleList[scheduleIndex].Date)
                {
                    timeScheduleIndex = scheduleIndex;
                }
                else
                {
                    break;
                }
                //MM

            }
            if (timeScheduleIndex == -1) return null;

            EmployeeTimeScheduleTO employeeTimeSchedule = timeScheduleList[timeScheduleIndex];

            // find actual time schema for the day
            WorkTimeSchemaTO actualTimeSchema = null;
            if (timeSchemas.ContainsKey(employeeTimeSchedule.TimeSchemaID))
                actualTimeSchema = timeSchemas[employeeTimeSchedule.TimeSchemaID];

            if (actualTimeSchema == null) return null;

            /* 2008-03-14
             * From now one, take the last existing time schedule, don't expect that every month has 
             * time schedule*/
            //int dayNum = (employeeTimeSchedule.StartCycleDay + date.Day - employeeTimeSchedule.Date.Day) % actualTimeSchema.CycleDuration;
            TimeSpan ts = new TimeSpan(date.Date.Ticks - employeeTimeSchedule.Date.Date.Ticks);
            int dayNum = (employeeTimeSchedule.StartCycleDay + (int)ts.TotalDays) % actualTimeSchema.CycleDuration;

            //19.09.2017 Miodrag / Postojala je mogucnost da je dayNum negativan.
            if (dayNum < 0)
            {
                dayNum = actualTimeSchema.CycleDuration + dayNum;
            }
            //mm

            is2DaysShift = false;
            for (int i = 0; i < actualTimeSchema.Days[dayNum].Count; i++)
            {
                WorkTimeIntervalTO currentTimeSchemaInterval = actualTimeSchema.Days[dayNum][i]; //key is interval_num which is 0, 1...
                if ((currentTimeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                    && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                {
                    is2DaysShift = true;
                    break;
                }
            }

            if (is2DaysShift)
            {
                is2DaysShift = false;
                int nextDay = dayNum + 1;
                if (nextDay >= actualTimeSchema.CycleDuration)
                    nextDay = 0;

                for (int i = 0; i < actualTimeSchema.Days[nextDay].Count; i++)
                {
                    WorkTimeIntervalTO currentTimeSchemaInterval = actualTimeSchema.Days[nextDay][i]; //key is interval_num which is 0, 1...
                    if ((currentTimeSchemaInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                        && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                    {
                        is2DaysShift = true;
                        firstIntervalNextDay = currentTimeSchemaInterval;
                        break;
                    }
                }
            }

            is2DaysShiftPrevious = false;
            for (int i = 0; i < actualTimeSchema.Days[dayNum].Count; i++)
            {
                WorkTimeIntervalTO currentTimeSchemaInterval = actualTimeSchema.Days[dayNum][i]; //key is interval_num which is 0, 1...
                if ((currentTimeSchemaInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                    && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                {
                    is2DaysShiftPrevious = true;
                    break;
                }
            }

            if (is2DaysShiftPrevious)
            {
                is2DaysShiftPrevious = false;
                int previousDay = dayNum - 1;
                if (previousDay < 0)
                    previousDay = actualTimeSchema.CycleDuration - 1;
                for (int i = 0; i < actualTimeSchema.Days[previousDay].Count; i++)
                {
                    WorkTimeIntervalTO currentTimeSchemaInterval = actualTimeSchema.Days[previousDay][i]; //key is interval_num which is 0, 1...
                    if ((currentTimeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                        && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                    {
                        is2DaysShiftPrevious = true;
                        break;
                    }
                }
            }

            return actualTimeSchema.Days[dayNum];
        }
        //get Time Schema for selected employee, for specific day
        //timeScheduleList contains Time Schemas for selected Employee and selected Time Interval
        //date is specific day
        //timeSchemas - all time shemas
        public static WorkTimeSchemaTO getTimeSchemaForDay(List<EmployeeTimeScheduleTO> timeScheduleList, DateTime date, List<WorkTimeSchemaTO> timeSchemas)
        {
            // find actual time schedule for the day            
            int timeScheduleIndex = -1;
            for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
            {
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                if (date >= timeScheduleList[scheduleIndex].Date)
                //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
                {
                    timeScheduleIndex = scheduleIndex;
                }
            }
            if (timeScheduleIndex == -1) return null;

            EmployeeTimeScheduleTO employeeTimeSchedule = timeScheduleList[timeScheduleIndex];

            // find actual time schema for the day
            WorkTimeSchemaTO actualTimeSchema = null;
            foreach (WorkTimeSchemaTO currentTimeSchema in timeSchemas)
            {
                if (currentTimeSchema.TimeSchemaID == employeeTimeSchedule.TimeSchemaID)
                {
                    actualTimeSchema = currentTimeSchema;
                    break;
                }
            }

            return actualTimeSchema;
        }

        //get Time Schema for selected employee, for specific day
        //timeScheduleList contains Time Schemas for selected Employee and selected Time Interval
        //date is specific day
        //timeSchemas - all time shemas
        public static WorkTimeSchemaTO getTimeSchemaForDayAndDayNum(List<EmployeeTimeScheduleTO> timeScheduleList, DateTime date, List<WorkTimeSchemaTO> timeSchemas, ref int dayNum)
        {
            // find actual time schedule for the day			
            int timeScheduleIndex = -1;
            for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
            {
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                if (date >= timeScheduleList[scheduleIndex].Date)
                //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
                {
                    timeScheduleIndex = scheduleIndex;
                }
            }
            if (timeScheduleIndex == -1) return null;

            EmployeeTimeScheduleTO employeeTimeSchedule = timeScheduleList[timeScheduleIndex];

            // find actual time schema for the day
            WorkTimeSchemaTO actualTimeSchema = null;
            foreach (WorkTimeSchemaTO currentTimeSchema in timeSchemas)
            {
                if (currentTimeSchema.TimeSchemaID == employeeTimeSchedule.TimeSchemaID)
                {
                    actualTimeSchema = currentTimeSchema;
                    break;
                }
            }
            if (actualTimeSchema == null) return null;

            /* 2008-03-14
             * From now one, take the last existing time schedule, don't expect that every month has 
             * time schedule*/
            //dayNum = (employeeTimeSchedule.StartCycleDay + date.Day - employeeTimeSchedule.Date.Day) % actualTimeSchema.CycleDuration;
            TimeSpan ts = new TimeSpan(date.Date.Ticks - employeeTimeSchedule.Date.Date.Ticks);
            dayNum = (employeeTimeSchedule.StartCycleDay + (int)ts.TotalDays) % actualTimeSchema.CycleDuration;

            return actualTimeSchema;
        }

        //get all iopairs for specific employee, for specific day 
        //ioPairList contains all iopairs for selected employee, for selected time interval
        //day is specific day
        //is2DaysShift is logical value - is shift for this day actually 2 days shift. It is for specific employee
        //is2DaysShiftPrevious is logical value - is shift for previous schema day (according to specific day) actually 2 days shift. It is for specific schema
        //firstIntervalNextDay - first interval for next schema day (according to specific day)
        //dayIntervals contains intervals for specific employee, for specific day
        public static List<IOPairTO> getEmployeeDayPairs(List<IOPairTO> ioPairList, DateTime day, bool is2DaysShift,
            bool is2DaysShiftPrevious, WorkTimeIntervalTO firstIntervalNextDay, Dictionary<int, WorkTimeIntervalTO> dayIntervals)
        {
            WorkTimeIntervalTO firstTimeSchemaInterval = null;
            if (is2DaysShiftPrevious)
            {
                for (int i = 0; i < dayIntervals.Count; i++)
                {
                    WorkTimeIntervalTO currentTimeSchemaInterval = dayIntervals[i]; //key is interval_num which is 0, 1...
                    if ((currentTimeSchemaInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                        && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                    {
                        firstTimeSchemaInterval = currentTimeSchemaInterval;
                        firstTimeSchemaInterval.StartTime = firstTimeSchemaInterval.StartTime.Subtract(new TimeSpan(0, 0, firstTimeSchemaInterval.StartTime.Second));
                        firstTimeSchemaInterval.EndTime = firstTimeSchemaInterval.EndTime.Subtract(new TimeSpan(0, 0, firstTimeSchemaInterval.EndTime.Second));
                        break;
                    }
                }
            }

            List<IOPairTO> employeeDayPairs = new List<IOPairTO>();
            foreach (IOPairTO iopair in ioPairList)
            {
                iopair.StartTime = iopair.StartTime.Subtract(new TimeSpan(0, 0, iopair.StartTime.Second));
                iopair.EndTime = iopair.EndTime.Subtract(new TimeSpan(0, 0, iopair.EndTime.Second));

                if (iopair.IOPairDate.Date == day.Date)
                {
                    if (!is2DaysShiftPrevious)
                        employeeDayPairs.Add(iopair);
                    if (is2DaysShiftPrevious && (firstTimeSchemaInterval != null)
                        && (iopair.StartTime.TimeOfDay >= firstTimeSchemaInterval.EndTime.TimeOfDay))
                        employeeDayPairs.Add(iopair);
                }

                //pairs that belong to the tomorrow's part of the 2 days shift 
                if (is2DaysShift && iopair.IOPairDate.Date == day.AddDays(1).Date && (firstIntervalNextDay != null)
                    && iopair.StartTime.TimeOfDay < firstIntervalNextDay.EndTime.TimeOfDay)
                {
                    employeeDayPairs.Add(iopair);
                }
            }

            return employeeDayPairs;
        }


        //get all intervals for specific employee, for specific day 
        //is2DaysShift is logical value - is shift for this day actually 2 days shift. It is for specific employee
        //is2DaysShiftPrevious is logical value - is shift for previous schema day (according to specific day) actually 2 days shift. It is for specific schema
        //firstIntervalNextDay - first interval for next schema day (according to specific day)
        //dayIntervals contains intervals for specific employee, for specific day
        public static List<WorkTimeIntervalTO> getEmployeeDayIntervals(bool is2DaysShift,
            bool is2DaysShiftPrevious, WorkTimeIntervalTO firstIntervalNextDay, Dictionary<int, WorkTimeIntervalTO> dayIntervals)
        {
            int midnightInterval = -1;
            if (is2DaysShiftPrevious)
            {
                for (int i = 0; i < dayIntervals.Count; i++)
                {
                    WorkTimeIntervalTO currentTimeSchemaInterval = dayIntervals[i]; //key is interval_num which is 0, 1...
                    if ((currentTimeSchemaInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                        && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                    {
                        midnightInterval = i;
                        break;
                    }
                }
            }

            List<WorkTimeIntervalTO> employeeDayIntervals = new List<WorkTimeIntervalTO>();
            for (int i = 0; i < dayIntervals.Count; i++)
            {
                if (!is2DaysShiftPrevious ||
                    (is2DaysShiftPrevious && (i != midnightInterval)))
                {
                    WorkTimeIntervalTO tsInterval = dayIntervals[i]; //key is interval_num which is 0, 1...
                    employeeDayIntervals.Add(tsInterval);
                }
            }

            //intervals that belong to the tomorrow's part of the 2 days shift 
            if (is2DaysShift && firstIntervalNextDay != null)
            {
                employeeDayIntervals.Add(firstIntervalNextDay);
            }

            return employeeDayIntervals;
        }

        //trim IO Pairs for specific day. 
        //All pairs that starts between earliest and lates arrival time set to start time of interval
        //All pairs that ends between earliest and lates leaving time set to end time of interval
        public static List<IOPairTO> trimDayPairs(List<IOPairTO> dayIOPairList, List<WorkTimeIntervalTO> dayIntervalsList)
        {
            foreach (IOPairTO iopair in dayIOPairList)
            {
                iopair.StartTime = iopair.StartTime.Subtract(new TimeSpan(0, 0, iopair.StartTime.Second));
                iopair.EndTime = iopair.EndTime.Subtract(new TimeSpan(0, 0, iopair.EndTime.Second));

                bool changeStart = false;
                bool changeEnd = false;
                foreach (WorkTimeIntervalTO timeSchemaInterval in dayIntervalsList)
                {
                    if ((iopair.StartTime.TimeOfDay >= timeSchemaInterval.EarliestArrived.TimeOfDay)
                        && (iopair.StartTime.TimeOfDay <= timeSchemaInterval.LatestArrivaed.TimeOfDay))
                    {
                        iopair.StartTime = iopair.StartTime.Add(iopair.StartTime.TimeOfDay.Negate() + timeSchemaInterval.StartTime.TimeOfDay);

                        if (iopair.EndTime.TimeOfDay < iopair.StartTime.TimeOfDay)
                        {
                            iopair.EndTime = iopair.StartTime;
                        }

                        changeStart = true;
                    }

                    if ((iopair.EndTime.TimeOfDay >= timeSchemaInterval.EarliestLeft.TimeOfDay)
                        && (iopair.EndTime.TimeOfDay <= timeSchemaInterval.LatestLeft.TimeOfDay))
                    {
                        iopair.EndTime = iopair.EndTime.Add(iopair.EndTime.TimeOfDay.Negate() + timeSchemaInterval.EndTime.TimeOfDay);

                        if (iopair.EndTime.TimeOfDay < iopair.StartTime.TimeOfDay)
                        {
                            iopair.StartTime = iopair.EndTime;
                        }

                        changeEnd = true;
                    }

                    if (changeStart && changeEnd)
                        break;
                }
            }

            return dayIOPairList;
        }

        //calculate how many hours employee did work on specific day
        //dayIOPairList contains all iopairs for specific day
        public static TimeSpan getEmployeeJobHourDay(List<IOPairTO> dayIOPairList)
        {
            TimeSpan totalTime = new TimeSpan(0);

            foreach (IOPairTO iopairTO in dayIOPairList)
            {
                //1.12.2010 Natasa count all IO Pairs
                //do not consider manually created pairs
                //if (iopairTO.ManualCreated == 1)
                //    continue;

                iopairTO.StartTime = iopairTO.StartTime.Subtract(new TimeSpan(0, 0, iopairTO.StartTime.Second));
                iopairTO.EndTime = iopairTO.EndTime.Subtract(new TimeSpan(0, 0, iopairTO.EndTime.Second));
                totalTime = totalTime.Add(iopairTO.EndTime.TimeOfDay.Subtract(iopairTO.StartTime.TimeOfDay));
                if (iopairTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                    totalTime = totalTime.Add(new TimeSpan(0, 1, 0));
            }

            return totalTime;
        }

        // same as getEmployeeJobHour but with manually created pairs
        public static TimeSpan getEmployeeJobHourDayTotal(List<IOPairTO> dayIOPairList)
        {
            TimeSpan totalTime = new TimeSpan(0);

            foreach (IOPairTO iopairTO in dayIOPairList)
            {

                iopairTO.StartTime = iopairTO.StartTime.Subtract(new TimeSpan(0, 0, iopairTO.StartTime.Second));
                iopairTO.EndTime = iopairTO.EndTime.Subtract(new TimeSpan(0, 0, iopairTO.EndTime.Second));
                totalTime = totalTime.Add(iopairTO.EndTime.TimeOfDay.Subtract(iopairTO.StartTime.TimeOfDay));
                if (iopairTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                    totalTime = totalTime.Add(new TimeSpan(0, 1, 0));
            }

            return totalTime;
        }

        //calculate how many hours employee suppose to work on specific day
        //intervals contains all intervals for specific day
        public static TimeSpan getEmployeeScheduleHourDay(List<WorkTimeIntervalTO> intervals)
        {
            TimeSpan totalTime = new TimeSpan(0);

            foreach (WorkTimeIntervalTO timeSchemaInterval in intervals)
            {
                timeSchemaInterval.StartTime = timeSchemaInterval.StartTime.Subtract(new TimeSpan(0, 0, timeSchemaInterval.StartTime.Second));
                timeSchemaInterval.EndTime = timeSchemaInterval.EndTime.Subtract(new TimeSpan(0, 0, timeSchemaInterval.EndTime.Second));
                totalTime = totalTime.Add(timeSchemaInterval.EndTime.TimeOfDay.Subtract(timeSchemaInterval.StartTime.TimeOfDay));
                if (timeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                    totalTime = totalTime.Add(new TimeSpan(0, 1, 0));
            }

            return totalTime;
        }

        #endregion

        #region licence functions

        // generate licence request with specified server, port an number of sessions
        public static bool generateLicenceRequest(string serverName, string port)
        {
            try
            {
                bool created = false;

                string sessionsNum = getSessionsNum();
                string moduls = getModuls(null);
                string customer = getCustomer(null);

                // generate licreq.txt in working folder
                string filePath = Application.StartupPath + "\\licreq.txt";
                FileStream stream = new FileStream(filePath, FileMode.Create);
                stream.Close();

                bool succeeded = false;
                DateTime t0 = DateTime.Now;
                while (!succeeded)
                {
                    try
                    {
                        StreamWriter writer = File.AppendText(filePath);

                        // licreq.txt contains encrypted string with server port and name, sessions number, moduls and customer
                        // server port contains 6 characters, server name contains 63 characters, sessions num contains 6 characters
                        // 16 moduls contains 6 characters each and customer contains 6 characters

                        string licreq = port.PadRight(Constants.serverPortLength, ' ')
                            + serverName.PadRight(Constants.serverNameLength, ' ') + sessionsNum.PadRight(Constants.noSessionsLength, ' ')
                            + moduls.PadRight(Constants.modulLength * Constants.modulNum, ' ') + (customer.PadLeft(3, '0')).PadRight(Constants.customerLength, ' ');

                        // encrypt licence request string
                        byte[] buffer = Util.Misc.encrypt(licreq);
                        string licreqCrypted = Convert.ToBase64String(buffer);

                        writer.WriteLine(licreqCrypted);
                        writer.Close();
                        succeeded = true;
                        created = true;
                    }
                    catch
                    {
                        Thread.Sleep(100);
                        if ((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > 3000)
                        {
                            succeeded = true;
                            created = false;
                        }
                    }
                }

                return created;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // get number of current sessions allowed
        public static string getSessionsNum()
        {
            try
            {
                string noSessions = "0";

                LicenceTO licenceTO = new Licence().FindMAX();

                string licence = "";

                try
                {
                    if (!licenceTO.LicenceKey.ToUpper().Equals(Constants.licenceKeyValue))
                    {
                        // decrypt licence request
                        byte[] buffer = Convert.FromBase64String(licenceTO.LicenceKey);
                        licence = Util.Misc.decrypt(buffer);
                    }
                }
                catch
                {
                    noSessions = "0";
                }

                if (licence != null && !licence.Equals("")
                        && !licence.ToUpper().Equals(Constants.licenceKeyValue) && licence.Length == Constants.LicenceLength)
                {
                    noSessions = licence.Substring(0, Constants.noSessionsLength).Trim();
                }

                return noSessions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // get moduls allowed
        public static string getModuls(object conn)
        {
            try
            {
                string moduls = "";

                Licence lic;

                if (conn != null)
                    lic = new Licence(conn);
                else
                    lic = new Licence();

                LicenceTO licenceTO = lic.FindMAX();

                string licence = "";

                try
                {
                    if (!licenceTO.LicenceKey.ToUpper().Equals(Constants.licenceKeyValue))
                    {
                        // decrypt licence request
                        byte[] buffer = Convert.FromBase64String(licenceTO.LicenceKey);
                        licence = Util.Misc.decrypt(buffer);
                    }
                }
                catch
                {
                    moduls = "";
                }

                if (licence != null && !licence.Equals("")
                        && !licence.ToUpper().Equals(Constants.licenceKeyValue) && licence.Length == Constants.LicenceLength)
                {
                    moduls = licence.Substring(Constants.noSessionsLength * 2 + Constants.serverNameLength +
                        Constants.serverPortLength, Constants.modulLength * Constants.modulNum).Trim();
                }

                return moduls;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // get list of moduls allowed
        public static List<int> getLicenceModuls(object conn)
        {
            try
            {
                string moduls = getModuls(conn);
                List<int> licenceModuls = new List<int>();

                for (int i = 0; i < moduls.Length; i += Constants.modulLength)
                {
                    string modul = "";
                    if (i + Constants.modulLength > moduls.Length)
                    {
                        modul = moduls.Substring(i).Trim();
                    }
                    else
                    {
                        modul = moduls.Substring(i, Constants.modulLength).Trim();
                    }
                    int mod = -1;
                    if (int.TryParse(modul, out mod))
                    {
                        licenceModuls.Add(mod);
                    }
                }

                return licenceModuls;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // get customer
        public static string getCustomer(object conn)
        {
            try
            {
                string customer = "0";

                Licence lic;

                if (conn != null)
                    lic = new Licence(conn);
                else
                    lic = new Licence();

                LicenceTO licenceTO = lic.FindMAX();

                string licence = "";

                try
                {
                    if (!licenceTO.LicenceKey.ToUpper().Equals(Constants.licenceKeyValue))
                    {
                        // decrypt licence request
                        byte[] buffer = Convert.FromBase64String(licenceTO.LicenceKey);
                        licence = Util.Misc.decrypt(buffer);
                    }
                }
                catch
                {
                    customer = "0";
                }

                if (licence != null && !licence.Equals("")
                        && !licence.ToUpper().Equals(Constants.licenceKeyValue) && licence.Length == Constants.LicenceLength)
                {
                    customer = licence.Substring(Constants.noSessionsLength * 2 + Constants.serverNameLength +
                        Constants.serverPortLength + Constants.modulLength * Constants.modulNum, Constants.customerLength).Trim();
                }

                return customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // generate licence with server, port, number of sessions, moduls and customer specified
        public static bool generateLicence(string server, string port, string noSessions, string moduls, string customer)
        {
            try
            {
                bool created = false;

                // create licence text
                // licence structure:
                // no_sessions (number of concurrent sessions)			6 characters
                // database_server_port						6 characters
                // database_server_name						63 characters
                // no_sessions_ctrl (same as no_sessions – for later control)	6 characters
                // 16 moduls                                6 characters each
                // customer                                 6 characters
                // check sum                                1 character
                string licenceText = noSessions.ToString().Trim().PadRight(Constants.noSessionsLength, ' ')
                + port.ToString().Trim().PadRight(Constants.serverPortLength, ' ')
                + server.ToString().Trim().PadRight(Constants.serverNameLength, ' ')
                + noSessions.ToString().Trim().PadRight(Constants.noSessionsLength, ' ');

                string[] modulsList = moduls.Split(',');

                if (modulsList.Length > 0)
                {
                    foreach (string modul in modulsList)
                    {
                        licenceText += modul.PadRight(Constants.modulLength, ' ');
                    }
                }

                licenceText = licenceText.PadRight(Constants.noSessionsLength * 2 + Constants.serverNameLength
                    + Constants.serverPortLength + Constants.modulLength * Constants.modulNum, ' ');

                licenceText += (customer.PadLeft(3, '0')).PadRight(Constants.customerLength, ' ');

                byte[] licenceBytes = System.Text.Encoding.ASCII.GetBytes(licenceText);
                byte[] licenceKeyBytes = new byte[licenceBytes.Length + 1];
                byte cs = (byte)0;

                if (licenceBytes.Length > 0)
                {
                    for (int i = 0; i < licenceBytes.Length; i++)
                    {
                        licenceKeyBytes[i] = licenceBytes[i];
                        cs ^= licenceBytes[i];
                    }
                }

                licenceKeyBytes[licenceKeyBytes.Length - 1] = cs;

                licenceText = System.Text.Encoding.ASCII.GetString(licenceKeyBytes);

                // encrypt licence string
                byte[] buffer = Util.Misc.encrypt(licenceText);
                string licenceTextCrypted = Convert.ToBase64String(buffer);

                // create licence.txt in working folder
                string filePath = Application.StartupPath + "\\licence.txt";
                FileStream stream = new FileStream(filePath, FileMode.Create);
                stream.Close();

                bool succeeded = false;
                DateTime t0 = DateTime.Now;
                while (!succeeded)
                {
                    try
                    {
                        StreamWriter writer = File.AppendText(filePath);
                        writer.WriteLine(licenceTextCrypted);
                        writer.Close();

                        succeeded = true;
                        created = true;
                    }
                    catch
                    {
                        Thread.Sleep(100);
                        if ((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > 3000)
                        {
                            succeeded = true;
                            created = false;
                        }
                    }
                }

                return created;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public static bool isADMINRole(string userID)
        {
            try
            {
                // check if log in user has ADMIN role
                ApplUsersXRole uXrole = new ApplUsersXRole();
                uXrole.AuXRoleTO.UserID = userID;
                uXrole.AuXRoleTO.RoleID = Constants.ADMINRoleID;
                List<ApplUsersXRoleTO> roles = uXrole.Search();

                if (roles.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region People counter functions
        //form log rows count total passes on each antenna form people counter graph
        public static List<LogTO> getPassesForLogs(List<LogTO> customerLogs, ref List<LogTO> passesAntenna1, DateTime fromHours, DateTime toHours)
        {
            List<LogTO> passesAntenna0 = new List<LogTO>();
            try
            {
                for (int i = 0; i < customerLogs.Count; i++)
                {
                    LogTO currentLog = customerLogs[i];
                    currentLog.EventTime = currentLog.EventTime.AddMinutes(-1);
                    if ((fromHours.Equals(new DateTime()) && toHours.Equals(new DateTime())) || (currentLog.EventTime.TimeOfDay > fromHours.TimeOfDay && currentLog.EventTime.TimeOfDay < toHours.TimeOfDay))
                    {
                        switch (currentLog.ActionCommited)
                        {
                            case Constants.passAntenna0:
                                passesAntenna0.Add(currentLog);
                                break;
                            case Constants.passAntenna1:
                                passesAntenna1.Add(currentLog);
                                break;
                            default:
                                continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return passesAntenna0;
        }
        //from integer which represents number of passes get number of visitors divide with 2
        // if modulo is more than zero enlarge counter but first check if counter isn't even number and add one visit
        public static int getNumOfVisits(List<LogTO> passesList, ref int counter, string graphDevide)
        {
            int visitsNum = 0;
            try
            {
                foreach (LogTO pass in passesList)
                {
                    visitsNum += pass.Antenna + pass.EventHappened * Constants.NumberOfPasses;
                }
                if (graphDevide != null && graphDevide.Equals(Constants.yes))
                {
                    switch (visitsNum % 2)
                    {
                        case 0:
                            visitsNum = visitsNum / 2;
                            break;

                        case 1:
                            visitsNum = visitsNum / 2;
                            if (counter % 2 == 1)
                            {
                                visitsNum++;
                            }
                            counter++;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return visitsNum;
        }

        #endregion

        #region Vacation evidence functions

        //from vacation plan and list of absences for selected employee and year
        //make new vacation plan with used days and days left
        //count used days only if days are working days
        public static EmployeeVacEvidTO getVacationWithAbsences(EmployeeVacEvidTO vacationEvidence, List<EmployeeAbsenceTO> absencesList, Dictionary<DateTime, HolidayTO> holidays, List<WorkTimeSchemaTO> timeSchemas)
        {
            EmployeeVacEvidTO vacationWithAbsences = vacationEvidence;

            try
            {
                vacationWithAbsences.UsedDays = 0;
                vacationWithAbsences.DaysLeft = vacationWithAbsences.NumOfDays;
                foreach (EmployeeAbsenceTO absence in absencesList)
                {
                    //if absence is related to vacation evidence count used days and days left
                    if (absence.EmployeeID == vacationWithAbsences.EmployeeID && absence.VacationYear == vacationWithAbsences.VacYear)
                    {
                        List<EmployeeTimeScheduleTO> EmplScheduleList = GetEmployeeTimeSchedules(vacationWithAbsences.EmployeeID, absence.DateStart, absence.DateEnd);
                        int usedDays = getNumOfWorkingDays(EmplScheduleList, absence.DateStart, absence.DateEnd, holidays, timeSchemas);

                        vacationWithAbsences.UsedDays += usedDays;
                        vacationWithAbsences.DaysLeft -= usedDays;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return vacationWithAbsences;
        }

        //geting schedule for selected employee in specific interval
        public static List<EmployeeTimeScheduleTO> GetEmployeeTimeSchedules(int employeeID, DateTime from, DateTime to)
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

        //geting number of working days in time interval
        //working day is not holiday day with interval's duration more than zero
        public static int getNumOfWorkingDays(List<EmployeeTimeScheduleTO> EmplScheduleList, DateTime DateStart, DateTime DateEnd, Dictionary<DateTime, HolidayTO> holidays, List<WorkTimeSchemaTO> timeSchemas)
        {
            int days = 100;

            try
            {
                for (DateTime date = DateStart; date <= DateEnd; date = date.AddDays(1))
                {
                    //if day is holiday we don't count it as used vacation
                    if (!holidays.ContainsKey(date))
                    {
                        bool is2DayShift = false;
                        bool is2DaysShiftPrevious = false;
                        WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                        //get intervls for employee and day
                        Dictionary<int, WorkTimeIntervalTO> edi = getDayTimeSchemaIntervals(EmplScheduleList, date, ref is2DayShift, ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);
                        //if employee absences day is working day and it is not holiday count it as used and one day less in LeftDays
                        if (edi != null)
                        {
                            List<WorkTimeIntervalTO> intervals = getEmployeeDayIntervals(is2DayShift, is2DaysShiftPrevious, firstIntervalNextDay, edi);
                            if (isWorkingDay(intervals))
                                days++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return days;
        }

        //geting end date of period with exect number of working date
        //from start date and num of working days
        public static DateTime getDayIntervalStart(List<EmployeeTimeScheduleTO> EmplScheduleList, DateTime DateStart, DateTime DateEnd, int numOfWorkingDays, Dictionary<DateTime, HolidayTO> holidays, List<WorkTimeSchemaTO> timeSchemas)
        {
            DateTime endDate = DateStart.Date;
            int numOfDays = 0;
            try
            {
                while (numOfDays != numOfWorkingDays - 1)
                {
                    numOfDays = 0;
                    for (DateTime date = DateStart; date <= endDate; date = date.AddDays(1))
                    {
                        //if day is holiday we don't count it as used vacation
                        if (!holidays.ContainsKey(date))
                        {
                            bool is2DayShift = false;
                            bool is2DaysShiftPrevious = false;
                            WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                            //get intervls for employee and day
                            Dictionary<int, WorkTimeIntervalTO> edi = getDayTimeSchemaIntervals(EmplScheduleList, date, ref is2DayShift, ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);
                            //if employee absences day is working day and it is not holiday count it as used and one day less in LeftDays
                            if (edi != null)
                            {
                                List<WorkTimeIntervalTO> intervals = getEmployeeDayIntervals(is2DayShift, is2DaysShiftPrevious, firstIntervalNextDay, edi);
                                if (isWorkingDay(intervals))
                                    numOfDays++;
                            }
                        }
                    }
                    endDate = endDate.AddDays(1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return endDate;
        }

        //if interval's duration is zero return false
        public static bool isWorkingDay(List<WorkTimeIntervalTO> intervals)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0);
            for (int i = 0; i < intervals.Count; i++)
            {
                ts = ts.Add(intervals[i].EndTime - intervals[i].StartTime);
            }
            return (ts > new TimeSpan(0, 0, 0));
        }

        #endregion

        #region io pairs processing functions
        public static void roundPairTime(IOPairProcessedTO pair, int roundingMinutes)
        {
            try
            {
                //do not consider secunds
                if (pair.StartTime.Second > 0)
                    pair.StartTime = pair.StartTime.AddSeconds(-pair.StartTime.Second);
                if (pair.EndTime.Second > 0)
                    pair.EndTime = pair.EndTime.AddSeconds(-pair.EndTime.Second);

                pair.StartTime = roundTime(pair.StartTime, roundingMinutes, false); //VRATITI NA false

                pair.EndTime = roundTime(pair.EndTime, roundingMinutes, true); //VRATITI NA true
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime roundTime(DateTime dateTime, int roundingMinutes, bool less)
        {
            DateTime rounded = dateTime;
            try
            {
                dateTime = dateTime.AddSeconds(-dateTime.Second);
                rounded = dateTime;
                if (less)
                {
                    TimeSpan nightPairEnd = new TimeSpan(23, 59, 0);
                    //round end time to 5 minutes before end time
                    if (dateTime.TimeOfDay.TotalMinutes % roundingMinutes > 0 && dateTime.TimeOfDay != nightPairEnd)
                    {
                        rounded = dateTime.AddMinutes(-dateTime.TimeOfDay.TotalMinutes % roundingMinutes);
                    }
                }
                else
                {
                    //round start time to 5 minutes after start time
                    if (dateTime.TimeOfDay.TotalMinutes % roundingMinutes > 0)
                    {
                        rounded = dateTime.AddMinutes(roundingMinutes - dateTime.TimeOfDay.TotalMinutes % roundingMinutes);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rounded;
        }

        public static int roundDuration(int currentDuration, int roundingMinutes, bool less)
        {
            int rounded = currentDuration;
            try
            {

                if (less)
                {
                    if (currentDuration % roundingMinutes >= 0)
                        rounded = (currentDuration / roundingMinutes) * roundingMinutes;
                }
                else
                {
                    if (currentDuration % roundingMinutes > 0)
                        rounded = (currentDuration / roundingMinutes + 1) * roundingMinutes;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rounded;
        }

        public static int getRootWorkingUnit(int wu, Dictionary<int, WorkingUnitTO> WorkingUnitDictionary)
        {
            int workingUnitID = wu;
            try
            {
                while (true)
                {
                    if (!WorkingUnitDictionary.ContainsKey(workingUnitID))
                        break;
                    else
                    {
                        if (WorkingUnitDictionary[workingUnitID].ParentWorkingUID == WorkingUnitDictionary[workingUnitID].WorkingUnitID)
                        {
                            workingUnitID = WorkingUnitDictionary[workingUnitID].WorkingUnitID;
                            break;
                        }
                        else
                        {
                            workingUnitID = WorkingUnitDictionary[workingUnitID].ParentWorkingUID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return workingUnitID;
        }

        public static string getWorkingUnitHierarhicly(int ID, List<int> allowedWU, Object dbConnection)
        {
            string workUnitID = ID.ToString();
            try
            {
                List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                WorkingUnit workUnit;
                if (dbConnection != null)
                    workUnit = new WorkingUnit(dbConnection);
                else
                    workUnit = new WorkingUnit();

                if (workUnit != null)
                {
                    workUnit.WUTO = workUnit.FindWU(ID);
                    wuList.Add(workUnit.WUTO);
                    wuList = workUnit.FindAllChildren(wuList);
                    workUnitID = "";
                    foreach (WorkingUnitTO wunit in wuList)
                    {
                        if (allowedWU.Contains(wunit.WorkingUnitID))
                            workUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                    }

                    if (workUnitID.Length > 0)
                    {
                        workUnitID = workUnitID.Substring(0, workUnitID.Length - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return workUnitID;
        }

        public static string getWorkingUnitHierarhicly(string IDs, List<int> allowedWU, Object dbConnection)
        {
            string workUnitID = IDs;
            try
            {
                List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                WorkingUnit workUnit;
                if (dbConnection != null)
                    workUnit = new WorkingUnit(dbConnection);
                else
                    workUnit = new WorkingUnit();

                if (workUnit != null)
                {
                    wuList = workUnit.Search(IDs);

                    wuList = workUnit.FindAllChildren(wuList);
                    workUnitID = "";
                    foreach (WorkingUnitTO wunit in wuList)
                    {
                        if (allowedWU.Contains(wunit.WorkingUnitID))
                            workUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                    }

                    if (workUnitID.Length > 0)
                    {
                        workUnitID = workUnitID.Substring(0, workUnitID.Length - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return workUnitID;
        }

        public static string getOrgUnitHierarhicly(string IDs, List<int> allowedOU, Object dbConnection)
        {
            string orgUnitID = IDs;
            try
            {
                List<OrganizationalUnitTO> ouList = new List<OrganizationalUnitTO>();
                OrganizationalUnit orgUnit;
                if (dbConnection != null)
                    orgUnit = new OrganizationalUnit(dbConnection);
                else
                    orgUnit = new OrganizationalUnit();

                if (orgUnit != null)
                {
                    ouList = orgUnit.Search(IDs);

                    ouList = orgUnit.FindAllChildren(ouList);
                    orgUnitID = "";
                    foreach (OrganizationalUnitTO ounit in ouList)
                    {
                        if (allowedOU.Contains(ounit.OrgUnitID))
                            orgUnitID += ounit.OrgUnitID.ToString().Trim() + ",";
                    }

                    if (orgUnitID.Length > 0)
                    {
                        orgUnitID = orgUnitID.Substring(0, orgUnitID.Length - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return orgUnitID;
        }

        public static void roundPairEndTime(IOPairProcessedTO pair, int roundingMinutes, bool less)
        {
            try
            {
                TimeSpan ioPairDuration = pair.EndTime.TimeOfDay - pair.StartTime.TimeOfDay;

                if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                    ioPairDuration = ioPairDuration.Add(new TimeSpan(0, 1, 0));

                //if (ioPairDuration.TotalMinutes < roundingMinutes && less)
                //    pair.EndTime = pair.StartTime;

                int left = (int)ioPairDuration.TotalMinutes % roundingMinutes;
                if (left > 0)
                {
                    if (less)
                        pair.EndTime = pair.EndTime.AddMinutes(-left);
                    else
                    {
                        pair.EndTime = pair.EndTime.AddMinutes(roundingMinutes - left);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void roundPairEndTime(IOPairProcessedTO pair, int roundingMinutes, List<IOPairTO> pairsForDayBefore, List<IOPairTO> pairsNextDay)
        {
            try
            {
                TimeSpan ioPairDuration = pair.EndTime.TimeOfDay - pair.StartTime.TimeOfDay;
                bool roundPair = true;
                if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                {
                    ioPairDuration = ioPairDuration.Add(new TimeSpan(0, 1, 0));

                    foreach (IOPairTO pairNextDay in pairsNextDay)
                    {
                        if (pairNextDay.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                        {
                            TimeSpan totalPairDuration = ioPairDuration + (pairNextDay.EndTime.TimeOfDay - pairNextDay.StartTime.TimeOfDay);
                            if ((int)totalPairDuration.TotalMinutes % roundingMinutes < (pairNextDay.EndTime.TimeOfDay - pairNextDay.StartTime.TimeOfDay).TotalMinutes)
                                roundPair = false;
                            break;
                        }
                    }
                }
                else if (pair.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                {
                    foreach (IOPairTO pairPreviousDay in pairsForDayBefore)
                    {
                        if (pairPreviousDay.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                        {
                            ioPairDuration = ioPairDuration + (pairPreviousDay.EndTime.TimeOfDay - pairPreviousDay.StartTime.TimeOfDay) + new TimeSpan(0, 1, 0);
                            if ((int)ioPairDuration.TotalMinutes % roundingMinutes > (pair.EndTime.TimeOfDay - pair.StartTime.TimeOfDay).TotalMinutes)
                            {
                                pair.EndTime = pair.StartTime;
                                roundPair = false;
                            }
                            break;
                        }
                    }
                }

                if (roundPair)
                {
                    if (ioPairDuration.TotalMinutes < roundingMinutes)
                        pair.EndTime = pair.StartTime;
                    int left = (int)ioPairDuration.TotalMinutes % roundingMinutes;
                    if (left > 0)
                    {
                        pair.EndTime = pair.EndTime.AddMinutes(-left);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void roundPairStartTime(IOPairProcessedTO pair, int roundingMinutes, bool less)
        {
            try
            {
                TimeSpan ioPairDuration = pair.EndTime.TimeOfDay - pair.StartTime.TimeOfDay;
                if (ioPairDuration.TotalMinutes < roundingMinutes)
                    pair.StartTime = pair.EndTime;
                int left = (int)ioPairDuration.TotalMinutes % roundingMinutes;
                if (left > 0)
                {
                    if (less)
                        pair.StartTime = pair.StartTime.AddMinutes(left);
                    else
                    {
                        pair.StartTime = pair.StartTime.AddMinutes(-(roundingMinutes - left));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool checkMinPresence(IOPairProcessedTO processedTO, List<IOPairTO> nextDayPairs, List<IOPairTO> prevDayPairs, int presenceMin)
        {
            bool presence = true;
            try
            {
                int duration = (int)(processedTO.EndTime.TimeOfDay.TotalMinutes - processedTO.StartTime.TimeOfDay.TotalMinutes);
                if (duration >= presenceMin)
                {
                    presence = true;
                }
                else if (processedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                {
                    duration += 1;
                    foreach (IOPairTO nextDayPair in nextDayPairs)
                    {
                        if (nextDayPair.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                        {
                            duration += (int)(nextDayPair.EndTime.TimeOfDay.TotalMinutes - nextDayPair.StartTime.TimeOfDay.TotalMinutes);
                            break;
                        }
                    }
                }
                else if (processedTO.EndTime.TimeOfDay == new TimeSpan(0, 0, 0))
                {
                    foreach (IOPairTO prev in prevDayPairs)
                    {
                        if (prev.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                        {
                            duration += (int)(prev.EndTime.TimeOfDay.TotalMinutes - prev.StartTime.TimeOfDay.TotalMinutes);
                            duration += 1;
                            break;
                        }
                    }
                }
                if (duration >= presenceMin)
                {
                    presence = true;
                }
                else
                {
                    presence = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return presence;
        }
        #endregion


        public static void getHolidays(DateTime startTime, DateTime endTime, Dictionary<string, List<DateTime>> personalHolidayDays, List<DateTime> nationalHolidaysDays,
            List<DateTime> nationalHolidaysSundays, Object dbConnection, List<HolidaysExtendedTO> nationaTransferablelHolidays)
        {
            try
            {
                // search from previous day, if start time is transfered holiday
                HolidaysExtended holiday;
                if (dbConnection != null)
                    holiday = new HolidaysExtended(dbConnection);
                else
                    holiday = new HolidaysExtended();

                if (holiday != null)
                {
                    List<HolidaysExtendedTO> Holidays = holiday.Search(startTime.AddDays(-1), endTime);
                    List<DateTime> nationalHolidaysAll = new List<DateTime>();
                    List<DateTime> nationalHolidaysSundaysDays = new List<DateTime>();
                    Dictionary<string, List<DateTime>> personalHolidaySundaysDays = new Dictionary<string, List<DateTime>>();

                    foreach (HolidaysExtendedTO holidayTO in Holidays)
                    {
                        if (holidayTO.SundayTransferable == Constants.yesInt)
                            nationaTransferablelHolidays.Add(holidayTO);

                        for (DateTime dateTime = holidayTO.DateStart; dateTime <= holidayTO.DateEnd; dateTime = dateTime.AddDays(1))
                        {
                            if (!nationalHolidaysDays.Contains(dateTime.Date) && holidayTO.Type == Constants.nationalHoliday)
                            {
                                nationalHolidaysAll.Add(dateTime.Date);

                                if (dateTime.Date >= startTime.Date && dateTime.Date <= endTime.Date)
                                    nationalHolidaysDays.Add(dateTime.Date);

                                if (dateTime.DayOfWeek == DayOfWeek.Sunday && holidayTO.SundayTransferable == Constants.yesInt)
                                    nationalHolidaysSundaysDays.Add(dateTime.Date);
                            }
                            else if (holidayTO.Type == Constants.personalHoliday)
                            {
                                if (!personalHolidayDays.ContainsKey(holidayTO.Category))
                                    personalHolidayDays.Add(holidayTO.Category, new List<DateTime>());

                                if (!personalHolidayDays[holidayTO.Category].Contains(dateTime.Date) && dateTime.Date >= startTime.Date && dateTime.Date <= endTime.Date)
                                    personalHolidayDays[holidayTO.Category].Add(dateTime.Date);

                                if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    if (!personalHolidaySundaysDays.ContainsKey(holidayTO.Category))
                                        personalHolidaySundaysDays.Add(holidayTO.Category, new List<DateTime>());
                                    if (!personalHolidaySundaysDays[holidayTO.Category].Contains(dateTime.Date) && dateTime.Date >= startTime.Date && dateTime.Date <= endTime.Date)
                                        personalHolidaySundaysDays[holidayTO.Category].Add(dateTime.Date);
                                }
                            }
                        }
                    }

                    foreach (DateTime date in nationalHolidaysSundaysDays)
                    {
                        DateTime dateAfterSunday = date.AddDays(1);
                        while (nationalHolidaysAll.Contains(dateAfterSunday) || dateAfterSunday.DayOfWeek == DayOfWeek.Sunday)
                        {
                            dateAfterSunday = dateAfterSunday.AddDays(1);
                        }

                        if (dateAfterSunday.Date >= startTime.Date && dateAfterSunday.Date <= endTime.Date)
                            nationalHolidaysSundays.Add(dateAfterSunday);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool is8HourShift(WorkTimeSchemaTO schema)
        {
            try
            {
                int daysCount = schema.Days.Keys.Count;
                for (int dayIndex = 0; dayIndex < daysCount; dayIndex++)
                {
                    int intervalsCount = schema.Days[dayIndex].Keys.Count;
                    for (int intIndex = 0; intIndex < intervalsCount; intIndex++)
                    {
                        WorkTimeIntervalTO interval = schema.Days[dayIndex][intIndex];

                        // if interval is night shift end, calculate duration with night shift start from previous day
                        if (Common.Misc.isThirdShiftEndInterval(interval))
                        {
                            if (dayIndex - 1 >= 0 && dayIndex - 1 < daysCount)
                            {
                                foreach (WorkTimeIntervalTO prevInterval in schema.Days[dayIndex - 1].Values)
                                {
                                    if (Common.Misc.isThirdShiftBeginningInterval(prevInterval)
                                        && interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).Add(prevInterval.EndTime.TimeOfDay.Subtract(prevInterval.StartTime.TimeOfDay).Add(new TimeSpan(0, 1, 0))).TotalHours == Constants.dayDurationStandardShift)
                                        return true;
                                }
                            }
                        }
                        // if interval is night shift start, calculate duration with night shift end from next day
                        else if (Common.Misc.isThirdShiftBeginningInterval(interval))
                        {
                            if (dayIndex + 1 >= 0 && dayIndex + 1 < daysCount)
                            {
                                foreach (WorkTimeIntervalTO nextInterval in schema.Days[dayIndex + 1].Values)
                                {
                                    if (Common.Misc.isThirdShiftEndInterval(nextInterval)
                                        && interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).Add(new TimeSpan(0, 1, 0)).Add(nextInterval.EndTime.TimeOfDay.Subtract(nextInterval.StartTime.TimeOfDay)).TotalHours == Constants.dayDurationStandardShift)
                                        return true;
                                }
                            }
                        }
                        else
                        {
                            if (interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalHours == Constants.dayDurationStandardShift)
                                return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool is10HourShift(WorkTimeSchemaTO schema)
        {
            try
            {
                int daysCount = schema.Days.Keys.Count;
                for (int dayIndex = 0; dayIndex < daysCount; dayIndex++)
                {
                    int intervalsCount = schema.Days[dayIndex].Keys.Count;
                    for (int intIndex = 0; intIndex < intervalsCount; intIndex++)
                    {
                        WorkTimeIntervalTO interval = schema.Days[dayIndex][intIndex];

                        // if interval is night shift end, calculate duration with night shift start from previous day
                        if (Common.Misc.isThirdShiftEndInterval(interval))
                        {
                            if (dayIndex - 1 >= 0 && dayIndex - 1 < daysCount)
                            {
                                foreach (WorkTimeIntervalTO prevInterval in schema.Days[dayIndex - 1].Values)
                                {
                                    if (Common.Misc.isThirdShiftBeginningInterval(prevInterval)
                                        && interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).Add(prevInterval.EndTime.TimeOfDay.Subtract(prevInterval.StartTime.TimeOfDay).Add(new TimeSpan(0, 1, 0))).TotalHours == Constants.dayDuration10hShift)
                                        return true;
                                }
                            }
                        }
                        // if interval is night shift start, calculate duration with night shift end from next day
                        else if (Common.Misc.isThirdShiftBeginningInterval(interval))
                        {
                            if (dayIndex + 1 >= 0 && dayIndex + 1 < daysCount)
                            {
                                foreach (WorkTimeIntervalTO nextInterval in schema.Days[dayIndex + 1].Values)
                                {
                                    if (Common.Misc.isThirdShiftEndInterval(nextInterval)
                                        && interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).Add(new TimeSpan(0, 1, 0)).Add(nextInterval.EndTime.TimeOfDay.Subtract(nextInterval.StartTime.TimeOfDay)).TotalHours == Constants.dayDuration10hShift)
                                        return true;
                                }
                            }
                        }
                        else
                        {
                            if (interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalHours == Constants.dayDuration10hShift)
                                return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int countWorkingDays(DateTime date, object conn)
        {
            int NumOfWorkingDays = 100;

            try
            {
                int numOfDays = 0;

                DateTime tempDate = new DateTime(date.Year, date.Month, 1);

                // get holidays from date month
                Dictionary<string, List<DateTime>> personalHolidayDays = new Dictionary<string, List<DateTime>>();
                List<DateTime> nationalHolidaysDays = new List<DateTime>();
                List<DateTime> nationalHolidaysSundays = new List<DateTime>();
                List<HolidaysExtendedTO> nationaTransferablelHolidays = new List<HolidaysExtendedTO>();
                getHolidays(tempDate.Date, tempDate.Date.AddMonths(1).AddDays(-1), personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, conn, nationaTransferablelHolidays);

                while (tempDate <= date)
                {
                    if (tempDate.DayOfWeek != DayOfWeek.Sunday && tempDate.DayOfWeek != DayOfWeek.Saturday && !nationalHolidaysDays.Contains(tempDate.Date) && !nationalHolidaysSundays.Contains(tempDate.Date))
                        numOfDays++;
                    tempDate = tempDate.AddDays(1);
                }

                NumOfWorkingDays = numOfDays;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return NumOfWorkingDays;
        }

        public static bool ReprocessPairsAndRecalculateCounters(string employeeIDString, DateTime startTime, DateTime endTime, IDbTransaction trans, Dictionary<int, List<DateTime>> emplDateWholeDayList, object connection, string user)
        {
            bool succ = true;
            try
            {
                EmployeesTimeSchedule emplSch;
                TimeSchema sch;
                Common.Rule rule;
                WorkingUnit wu;
                IOPairProcessed pairProcessed;
                IOPair pair;
                IOPairsProcessedHist pairHist;
                PassType pt;
                Employee empl;
                EmployeeCounterValue counter;

                if (connection != null)
                {
                    emplSch = new EmployeesTimeSchedule(connection);
                    sch = new TimeSchema(connection);
                    rule = new Common.Rule(connection);
                    wu = new WorkingUnit(connection);
                    pairProcessed = new IOPairProcessed(connection);
                    pair = new IOPair(connection);
                    pairHist = new IOPairsProcessedHist(connection);
                    pt = new PassType(connection);
                    empl = new Employee(connection);
                    counter = new EmployeeCounterValue(connection);
                }
                else
                {
                    emplSch = new EmployeesTimeSchedule();
                    sch = new TimeSchema();
                    rule = new Common.Rule();
                    wu = new WorkingUnit();
                    pairProcessed = new IOPairProcessed();
                    pair = new IOPair();
                    pairHist = new IOPairsProcessedHist();
                    pt = new PassType();
                    empl = new Employee();
                    counter = new EmployeeCounterValue();
                }

                //get all time schedules for selected month
                Dictionary<int, List<EmployeeTimeScheduleTO>> EmplTimeSchemas = emplSch.SearchEmployeesSchedulesDS(employeeIDString, startTime.Date, endTime.Date.AddDays(1), trans);

                //get all time Schemas
                Dictionary<int, WorkTimeSchemaTO> timeSchema = sch.getDictionary(trans);

                //list od datetime for each employee for dayBefore
                Dictionary<int, List<DateTime>> emplDateListDayBeforeTemp = new Dictionary<int, List<DateTime>>();

                //list od datetime for each employee for dayAfter
                Dictionary<int, List<DateTime>> emplDateListDayAfterTemp = new Dictionary<int, List<DateTime>>();

                //rules dictionary first key is Working Unit ID, secund key is Employee Type ID, third key is ryle_Type; value is RuleTO
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDictionary = rule.SearchWUEmplTypeDictionary(trans);

                //working units in dictionary for root wu_id 
                Dictionary<int, WorkingUnitTO> WorkingUnitDictionary = wu.getWUDictionary(trans);

                List<IOPairProcessedTO> processedPairsList = pairProcessed.getProcessedPairs(emplDateWholeDayList, trans);

                foreach (IOPairProcessedTO pairProcessedTO in processedPairsList)
                {
                    if (!emplDateWholeDayList.ContainsKey(pairProcessedTO.EmployeeID))
                        emplDateWholeDayList.Add(pairProcessedTO.EmployeeID, new List<DateTime>());

                    if (!emplDateWholeDayList[pairProcessedTO.EmployeeID].Contains(pairProcessedTO.IOPairDate.Date))
                        emplDateWholeDayList[pairProcessedTO.EmployeeID].Add(pairProcessedTO.IOPairDate.Date);

                    if (pairProcessedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                    {
                        if (!emplDateListDayAfterTemp.ContainsKey(pairProcessedTO.EmployeeID))
                            emplDateListDayAfterTemp.Add(pairProcessedTO.EmployeeID, new List<DateTime>());

                        if (!emplDateListDayAfterTemp[pairProcessedTO.EmployeeID].Contains(pairProcessedTO.IOPairDate.Date.AddDays(1)))
                            emplDateListDayAfterTemp[pairProcessedTO.EmployeeID].Add(pairProcessedTO.IOPairDate.Date.AddDays(1));
                    }

                    if (pairProcessedTO.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                    {
                        if (!emplDateListDayBeforeTemp.ContainsKey(pairProcessedTO.EmployeeID))
                            emplDateListDayBeforeTemp.Add(pairProcessedTO.EmployeeID, new List<DateTime>());

                        if (!emplDateListDayBeforeTemp[pairProcessedTO.EmployeeID].Contains(pairProcessedTO.IOPairDate.Date.AddDays(-1)))
                            emplDateListDayBeforeTemp[pairProcessedTO.EmployeeID].Add(pairProcessedTO.IOPairDate.Date.AddDays(-1));
                    }
                }

                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> manualCreated = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                //manualy created io pairs processed
                manualCreated = pairProcessed.getManualCreatedPairs(emplDateWholeDayList, trans);

                //list od datetime for each employee for dayBefore
                Dictionary<int, List<DateTime>> emplDateListDayBefore = new Dictionary<int, List<DateTime>>();

                //list od datetime for each employee for dayAfter
                Dictionary<int, List<DateTime>> emplDateListDayAfter = new Dictionary<int, List<DateTime>>();

                foreach (int emplID in emplDateListDayBeforeTemp.Keys)
                {
                    foreach (DateTime dateTime in emplDateListDayBeforeTemp[emplID])
                    {
                        if (!emplDateWholeDayList.ContainsKey(emplID) || !emplDateWholeDayList[emplID].Contains(dateTime))
                        {
                            if (!emplDateListDayBefore.ContainsKey(emplID))
                                emplDateListDayBefore.Add(emplID, new List<DateTime>());

                            if (!emplDateListDayBefore[emplID].Contains(dateTime))
                                emplDateListDayBefore[emplID].Add(dateTime);
                        }
                    }
                }

                foreach (int emplID in emplDateListDayAfterTemp.Keys)
                {
                    foreach (DateTime dateTime in emplDateListDayAfterTemp[emplID])
                    {
                        if (!emplDateWholeDayList.ContainsKey(emplID) || !emplDateWholeDayList[emplID].Contains(dateTime))
                        {
                            if (!emplDateListDayAfter.ContainsKey(emplID))
                                emplDateListDayAfter.Add(emplID, new List<DateTime>());

                            if (!emplDateListDayAfter[emplID].Contains(dateTime))
                                emplDateListDayAfter[emplID].Add(dateTime);
                        }
                    }
                }

                //if manualy created io_pair_processed is part of night shift add day before or day after
                if (emplDateListDayAfter.Count > 0 || emplDateListDayBefore.Count > 0)
                {
                    manualCreated = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();

                    //manualy created io pairs processed for all days
                    if (emplDateWholeDayList.Keys.Count > 0)
                        manualCreated = pairProcessed.getManualCreatedPairs(emplDateWholeDayList, trans);

                    if (emplDateListDayBefore.Count > 0)
                    {
                        //manualy created io pairs processed for end of the day form 12 to 24
                        pairProcessed.getManualCreatedPairsDayBefore(emplDateListDayBefore, manualCreated, trans);
                    }

                    if (emplDateListDayAfter.Count > 0)
                    {
                        //manualy created io pairs processed for begining of the day 00 to 12
                        pairProcessed.getManualCreatedPairsDayAfter(emplDateListDayAfter, manualCreated, trans);
                    }
                }

                pair.SetTransaction(trans);
                //update all io_pairs to unprocessed
                succ = succ && pair.updateToUnprocessed(emplDateWholeDayList, emplDateListDayAfter, emplDateListDayBefore, false);

                if (succ)
                {
                    DateTime histCreated = DateTime.Now;

                    //save manual created pairs in hist table and update counters                    
                    Dictionary<int, PassTypeTO> passTypes = new Dictionary<int, PassTypeTO>();
                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounterValues = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();

                    //get old counter values
                    if (manualCreated.Keys.Count > 0)
                    {
                        passTypes = pt.SearchDictionary(trans);
                        emplCounterValues = counter.SearchValues(employeeIDString, trans);
                    }

                    foreach (int emplID in manualCreated.Keys)
                    {
                        Dictionary<string, RuleTO> emplRules = new Dictionary<string, RuleTO>();
                        Dictionary<int, EmployeeCounterValueTO> counterValues = new Dictionary<int, EmployeeCounterValueTO>();
                        Dictionary<int, int> counterNewValues = new Dictionary<int, int>();
                        EmployeeTO emplTO = empl.Find(emplID.ToString(), trans);
                        emplTO.WorkingUnitID = Common.Misc.getRootWorkingUnit(emplTO.WorkingUnitID, WorkingUnitDictionary);

                        if (rulesDictionary.ContainsKey(emplTO.WorkingUnitID) && rulesDictionary[emplTO.WorkingUnitID].ContainsKey(emplTO.EmployeeTypeID))
                        {
                            emplRules = rulesDictionary[emplTO.WorkingUnitID][emplTO.EmployeeTypeID];
                        }
                        if (emplCounterValues.ContainsKey(emplID))
                        {
                            counterValues = emplCounterValues[emplID];
                        }
                        bool pairBeforeEndWithNightShift = false;
                        bool updateCounters = true;

                        List<DateTime> weeksChanged = new List<DateTime>();
                        foreach (DateTime date in manualCreated[emplID].Keys)
                        {
                            //for night shift do not update twice counter
                            //if (pairBeforeEndWithNightShift)
                            //{
                            //    updateCounters = false;
                            //    pairBeforeEndWithNightShift = false;
                            //}
                            //else
                            //{
                            //    updateCounters = true;
                            //}

                            foreach (IOPairProcessedTO processed in manualCreated[emplID][date])
                            {
                                int counterType = -1;
                                if (updateCounters)
                                {
                                    //all counters but paid leave take from rules 
                                    foreach (string key in Constants.CounterTypesForRuleTypes.Keys)
                                    {
                                        if (emplRules.ContainsKey(key) && emplRules[key].RuleValue == processed.PassTypeID)
                                        {
                                            counterType = Constants.CounterTypesForRuleTypes[key];
                                            break;
                                        }
                                    }

                                    //paid leave take types from pass_types
                                    if (passTypes[processed.PassTypeID].LimitCompositeID != -1)
                                    {
                                        counterType = (int)Constants.EmplCounterTypes.PaidLeaveCounter;
                                    }
                                }
                                if (counterType != -1)
                                {
                                    bool is2DayShift = false;
                                    bool is2DayShiftPrevious = false;
                                    WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();

                                    WorkTimeSchemaTO workTimeSchema = null;

                                    List<EmployeeTimeScheduleTO> emplTS = new List<EmployeeTimeScheduleTO>();
                                    if (EmplTimeSchemas.ContainsKey(emplID))
                                        emplTS = EmplTimeSchemas[emplID];

                                    //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                                    //are night shift days. If day is night shift day, also take first interval of next day
                                    Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplTS, date, ref is2DayShift,
                                        ref is2DayShiftPrevious, ref firstIntervalNextDay, ref workTimeSchema, timeSchema);

                                    foreach (int count in counterValues.Keys)
                                    {
                                        if (!counterNewValues.ContainsKey(count))
                                        {
                                            counterNewValues.Add(count, counterValues[count].Value);
                                        }
                                        if (count == counterType)
                                        {
                                            //if counter maesure unit is DAY decrease by one
                                            if (counterValues[count].MeasureUnit == Constants.emplCounterMesureDay)
                                            {
                                                if (processed.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                                {
                                                    if (counterNewValues[count] > 0)
                                                    {
                                                        counterNewValues[count]--;
                                                    }
                                                    else
                                                    {
                                                        throw new Exception("Counter value for employee = " + emplID + " and date = " + date + " is less than needed to reprocess data.Counter type = " + counterType);
                                                    }
                                                }
                                            }
                                            //if counter maesure unit is MINUTE
                                            else
                                            {
                                                TimeSpan pairDuration = processed.EndTime.TimeOfDay - processed.StartTime.TimeOfDay;

                                                //add one minut until midnight
                                                if (processed.EndTime.TimeOfDay == new TimeSpan(23, 59, 00))
                                                    pairDuration = pairDuration.Add(new TimeSpan(0, 1, 0));

                                                // if type is stop working done, increase counter - counter is in minutes
                                                if (emplRules.ContainsKey(Constants.RuleCompanyStopWorkingDone) && emplRules[Constants.RuleCompanyStopWorkingDone].RuleValue == processed.PassTypeID)
                                                    counterNewValues[count] += (int)pairDuration.TotalMinutes;
                                                // if type is bank hour used, increase counter - counter is in minutes
                                                else if (emplRules.ContainsKey(Constants.RuleCompanyBankHourUsed) && emplRules[Constants.RuleCompanyBankHourUsed].RuleValue == processed.PassTypeID)
                                                {
                                                    int round = 30;
                                                    if (emplRules.ContainsKey(Constants.RuleBankHoursUsedRounding))
                                                        round = emplRules[Constants.RuleBankHoursUsedRounding].RuleValue;
                                                    counterNewValues[count] += Common.Misc.roundDuration((int)pairDuration.TotalMinutes, round, false);
                                                }
                                                else if (emplRules.ContainsKey(Constants.RuleCompanyInitialOvertimeUsed) && emplRules[Constants.RuleCompanyInitialOvertimeUsed].RuleValue == processed.PassTypeID)
                                                {
                                                    int round = 1;
                                                    if (emplRules.ContainsKey(Constants.RuleInitialOvertimeUsedRounding))
                                                        round = emplRules[Constants.RuleInitialOvertimeUsedRounding].RuleValue;
                                                    counterNewValues[count] += Common.Misc.roundDuration((int)pairDuration.TotalMinutes, round, false);
                                                }
                                                // else decrease counter - counter is in minutes
                                                else if (pairDuration.TotalMinutes > 0)// && counterNewValues[count] >= pairDuration.TotalMinutes)
                                                {
                                                    counterNewValues[count] = counterNewValues[count] - (int)pairDuration.TotalMinutes;

                                                }
                                                //else
                                                //{
                                                //    throw new Exception("Counter value for employee = " + emplID + " and date = " + date + " is less than needed to reprocess data.Counter type = " + counterType);
                                                //}
                                            }

                                            if (processed.StartTime.TimeOfDay != new TimeSpan(0, 0, 0)
                                                && ((emplRules.ContainsKey(Constants.RuleCompanyAnnualLeave) && processed.PassTypeID == emplRules[Constants.RuleCompanyAnnualLeave].RuleValue)
                                                || (emplRules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && processed.PassTypeID == emplRules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue)))
                                            {
                                                //if shift is more than 8 hours long and if employee has whole week annual leave 
                                                //decrease couter once more
                                                if (workTimeSchema != null && Common.Misc.is10HourShift(workTimeSchema))
                                                {
                                                    int dayNum = 0;
                                                    switch (processed.IOPairDate.DayOfWeek)
                                                    {
                                                        case DayOfWeek.Monday:
                                                            dayNum = 0;
                                                            break;
                                                        case DayOfWeek.Tuesday:
                                                            dayNum = 1;
                                                            break;
                                                        case DayOfWeek.Wednesday:
                                                            dayNum = 2;
                                                            break;
                                                        case DayOfWeek.Thursday:
                                                            dayNum = 3;
                                                            break;
                                                        case DayOfWeek.Friday:
                                                            dayNum = 4;
                                                            break;
                                                        case DayOfWeek.Saturday:
                                                            dayNum = 5;
                                                            break;
                                                        case DayOfWeek.Sunday:
                                                            dayNum = 6;
                                                            break;
                                                    }

                                                    DateTime weekBegining = processed.IOPairDate.AddDays(-dayNum).Date; // first day of current week

                                                    if (weeksChanged.Contains(weekBegining.Date))
                                                    {
                                                        break;
                                                    }

                                                    // get week annual leave pairs
                                                    int annualLeaveDays = 0;
                                                    IOPairProcessed annualPair;
                                                    if (connection != null)
                                                        annualPair = new IOPairProcessed(connection);
                                                    else
                                                        annualPair = new IOPairProcessed();

                                                    List<IOPairProcessedTO> annualLeaveWeekPairs = annualPair.SearchWeekPairs(processed.EmployeeID, processed.IOPairDate, false, getAnnualLeaveTypesString(emplRules), trans);

                                                    foreach (IOPairProcessedTO aPair in annualLeaveWeekPairs)
                                                    {
                                                        // second night shift belongs to previous day
                                                        if (aPair.StartTime.Hour == 0 && aPair.StartTime.Minute == 0)
                                                            continue;

                                                        annualLeaveDays++;
                                                    }

                                                    // if fourts day of week is changed, subtract/add two days
                                                    if (annualLeaveDays == (Constants.fullWeek10hShift - 1))
                                                    {
                                                        counterNewValues[count]--;
                                                        weeksChanged.Add(weekBegining);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //update counters and save history                                    
                                    succ = succ && manualPairCounters(processed, counterNewValues, counterValues, histCreated, trans, connection, user);

                                    if (succ)
                                    {
                                        foreach (int counterTypeID in counterNewValues.Keys)
                                        {
                                            foreach (int key in counterValues.Keys)
                                            {
                                                if (counterTypeID == key)
                                                {
                                                    counterValues[key].Value = counterNewValues[key];
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                // if no need to update counters just save manual created pair to hist table
                                else
                                {
                                    IOPairsProcessedHistTO histTO = new IOPairsProcessedHistTO(processed);
                                    pairHist.IOPairProcessedHistTO = histTO;
                                    pairHist.IOPairProcessedHistTO.Alert = Constants.alertStatus.ToString();
                                    if (!user.Trim().Equals(""))
                                        pairHist.IOPairProcessedHistTO.ModifiedBy = user.Trim();
                                    pairHist.IOPairProcessedHistTO.ModifiedTime = histCreated;
                                    pairHist.SetTransaction(trans);
                                    succ = succ && (pairHist.Save(false) > 0);

                                    if (!succ)
                                        break;
                                }
                            }

                            if (!succ)
                                break;
                        }

                        if (!succ)
                            break;
                    }

                    if (succ)
                    {
                        pairProcessed.IOPairProcessedTO = new IOPairProcessedTO();
                        pairProcessed.SetTransaction(trans);
                        //delete all processed iopairs where alert is not manualy changed
                        succ = succ && pairProcessed.DeleteProcessedPairs(emplDateWholeDayList, emplDateListDayAfter, emplDateListDayBefore, false);
                    }
                }
            }
            catch
            {
                //throw ex;
                succ = false;
            }

            return succ;
        }

        private static bool manualPairCounters(IOPairProcessedTO pairToConfirm, Dictionary<int, int> emplCounters, Dictionary<int, EmployeeCounterValueTO> emplOldCounters, DateTime histCreated, IDbTransaction trans, object connection, string user)
        {
            try
            {
                IOPairProcessed pair;
                if (connection != null)
                    pair = new IOPairProcessed(connection);
                else
                    pair = new IOPairProcessed();
                bool saved = true;
                //string error = "";

                pair.SetTransaction(trans);

                //try
                //{
                IOPairsProcessedHist hist;
                EmployeeCounterValue counter;
                EmployeeCounterValueHist counterHist;

                if (connection != null)
                {
                    hist = new IOPairsProcessedHist(connection);
                    counter = new EmployeeCounterValue(connection);
                    counterHist = new EmployeeCounterValueHist(connection);
                }
                else
                {
                    hist = new IOPairsProcessedHist();
                    counter = new EmployeeCounterValue();
                    counterHist = new EmployeeCounterValueHist();
                }

                pair.IOPairProcessedTO = pairToConfirm;
                IOPairsProcessedHistTO histTO = new IOPairsProcessedHistTO(pairToConfirm);
                hist.IOPairProcessedHistTO = histTO;
                hist.IOPairProcessedHistTO.Alert = Constants.alertStatus.ToString();
                if (!user.Trim().Equals(""))
                    hist.IOPairProcessedHistTO.ModifiedBy = user.Trim();
                hist.IOPairProcessedHistTO.ModifiedTime = histCreated;
                hist.SetTransaction(pair.GetTransaction());
                saved = saved && hist.Save(false) > 0;

                if (saved)
                    saved = saved && (pair.Delete(pairToConfirm.RecID.ToString(), false));

                if (saved)
                {
                    // update counters, updated counters insert to hist table
                    counterHist.SetTransaction(pair.GetTransaction());
                    counter.SetTransaction(pair.GetTransaction());
                    // update counters and move old counter values to hist table if updated
                    foreach (int type in emplCounters.Keys)
                    {
                        if (emplOldCounters.ContainsKey(type) && emplOldCounters[type].Value != emplCounters[type])
                        {
                            // move to hist table
                            counterHist.ValueTO = new EmployeeCounterValueHistTO(emplOldCounters[type]);
                            if (!user.Trim().Equals(""))
                                counterHist.ValueTO.ModifiedBy = user.Trim();
                            saved = saved && (counterHist.Save(false) >= 0);

                            if (!saved)
                                break;

                            counter.ValueTO = new EmployeeCounterValueTO();
                            counter.ValueTO.EmplCounterTypeID = type;
                            counter.ValueTO.EmplID = pairToConfirm.EmployeeID;
                            counter.ValueTO.Value = emplCounters[type];
                            if (!user.Trim().Equals(""))
                                counter.ValueTO.ModifiedBy = user;

                            saved = saved && counter.Update(false);

                            if (!saved)
                                break;
                        }
                    }
                }

                //if (!saved)
                //{
                //    if (pair.GetTransaction() != null)
                //        pair.RollbackTransaction();
                //    throw new Exception(".manualPairCounters() : Processing manual pairs counters faild for Employee_ID = " + pairToConfirm.EmployeeID + "; Date = " + pairToConfirm.IOPairDate.ToString("dd.MM.yyyy") + "; Pass_type_ID = " + pairToConfirm.PassTypeID.ToString());
                //}
                //}
                //catch (Exception ex)
                //{
                //    if (pair.GetTransaction() != null)
                //        pair.RollbackTransaction();
                //    throw new Exception(".manualPairCounters() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
                //}

                return saved;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string getAnnualLeaveTypesString(Dictionary<string, RuleTO> emplRules)
        {
            try
            {
                string ptIDs = "";
                if (emplRules.ContainsKey(Constants.RuleCompanyAnnualLeave))
                    ptIDs += emplRules[Constants.RuleCompanyAnnualLeave].RuleValue.ToString().Trim() + ",";
                if (emplRules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave))
                    ptIDs += emplRules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue.ToString().Trim() + ",";

                if (ptIDs.Length > 0)
                    ptIDs = ptIDs.Substring(0, ptIDs.Length - 1);

                return ptIDs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<int> getAnnualLeaveTypes(Dictionary<string, RuleTO> emplRules)
        {
            try
            {
                List<int> ptList = new List<int>();
                if (emplRules.ContainsKey(Constants.RuleCompanyAnnualLeave))
                    ptList.Add(emplRules[Constants.RuleCompanyAnnualLeave].RuleValue);
                if (emplRules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave))
                    ptList.Add(emplRules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue);

                return ptList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // get gap in hours between current interval and next interval
        public static int getNextIntervalGap(int index, List<WorkTimeIntervalTO> dayIntervals, List<WorkTimeIntervalTO> nextIntervals)
        {
            try
            {
                int gap = 0;

                if (index >= 0 && index < dayIntervals.Count)
                {
                    // do not check third sfift beginning, return valid value (minimal interval gap)
                    if (dayIntervals[index].EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                        return Constants.FiatShiftMinimalGap;

                    // try to get next day interval
                    if (index + 1 >= 0 && index + 1 < dayIntervals.Count)
                        gap = (int)dayIntervals[index + 1].StartTime.TimeOfDay.Subtract(dayIntervals[index].EndTime.TimeOfDay).TotalHours;
                    else
                    {
                        int minutes = (int)new TimeSpan(23, 59, 0).Subtract(dayIntervals[index].EndTime.TimeOfDay).TotalMinutes + 1;

                        bool firstFound = false;
                        for (int i = 0; i < nextIntervals.Count; i++)
                        {
                            if (nextIntervals[i].EndTime.TimeOfDay.Subtract(nextIntervals[i].StartTime.TimeOfDay).TotalMinutes > 0)
                            {
                                minutes += (int)nextIntervals[i].StartTime.TimeOfDay.Subtract(new TimeSpan(0, 0, 0)).TotalMinutes;
                                firstFound = true;
                                break;
                            }
                        }

                        if (!firstFound)
                            minutes += 24 * 60; // add whole day                        

                        gap = minutes / 60;
                    }
                }

                return gap;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //static Dictionary<int, WorkTimeSchemaTO> schemas; // when checking employees
        public static DateTime isValidTimeSchedule(Dictionary<int, EmployeeTO> emplDict, Dictionary<int, EmployeeAsco4TO> ascoDict, Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDist,
            string emplIDs, DateTime fromDate, DateTime toDate, IDbTransaction trans, object connection, bool additionalValidation, ref bool validFundHrs, bool checkLaborLaw)
        {
            try
            {
                DateTime invalidDate = new DateTime();

                if (emplIDs.Trim().Equals(""))
                    return invalidDate;

                // when checking employees
                //if (schemas == null)
                //    schemas = getSchemasSortByTime(trans, connection);
                Dictionary<int, WorkTimeSchemaTO> schemas = getSchemasSortByTime(trans, connection);

                EmployeesTimeSchedule emplSch;
                Employee empl;
                EmployeeGroupsTimeSchedule emplGroupSch;

                if (connection != null)
                {
                    emplSch = new EmployeesTimeSchedule(connection);
                    empl = new Employee(connection);
                    emplGroupSch = new EmployeeGroupsTimeSchedule(connection);
                }
                else
                {
                    emplSch = new EmployeesTimeSchedule();
                    empl = new Employee();
                    emplGroupSch = new EmployeeGroupsTimeSchedule();
                }

                // fromDate and toDate should be first and last day from assigning form
                // check for whole first/last weeks and the day before/after those weeks that are assigned previously, checking if combination of new and old schedule is valid
                DateTime from = getWeekBeggining(fromDate.Date).AddDays(-7); // get whole week before becouse of labor law checking                
                DateTime to = getWeekEnding(toDate.Date);

                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = emplSch.SearchEmployeesSchedulesExactDate(emplIDs, from, to, trans);

                // get employee's groups and groups schedules
                Dictionary<int, int> emplGroups = new Dictionary<int, int>();
                Dictionary<int, List<EmployeeGroupsTimeScheduleTO>> groupSchedules = new Dictionary<int, List<EmployeeGroupsTimeScheduleTO>>();

                if (additionalValidation)
                {
                    // get employees groups
                    emplGroups = empl.SearchEmployeesGroups(emplIDs, trans);

                    // get groups schedules
                    string grpIDs = "";
                    foreach (int groupID in emplGroups.Values)
                    {
                        grpIDs += groupID.ToString().Trim() + ",";
                    }

                    if (grpIDs.Length > 0)
                    {
                        grpIDs = grpIDs.Substring(0, grpIDs.Length - 1);

                        List<EmployeeGroupsTimeScheduleTO> schList = emplGroupSch.SearchGroupsSchedules(grpIDs, from.Date, to.AddDays(1).Date, trans);

                        foreach (EmployeeGroupsTimeScheduleTO groupSch in schList)
                        {
                            if (!groupSchedules.ContainsKey(groupSch.EmployeeGroupID))
                                groupSchedules.Add(groupSch.EmployeeGroupID, new List<EmployeeGroupsTimeScheduleTO>());

                            groupSchedules[groupSch.EmployeeGroupID].Add(groupSch);
                        }
                    }
                }

                // validate schedules for every employee
                foreach (int emplID in emplSchedules.Keys)
                {
                    int schIndex = 0;

                    Dictionary<string, RuleTO> emplRules = new Dictionary<string, RuleTO>();
                    if (ascoDict.ContainsKey(emplID) && emplDict.ContainsKey(emplID) && rulesDist.ContainsKey(ascoDict[emplID].IntegerValue4)
                        && rulesDist[ascoDict[emplID].IntegerValue4].ContainsKey(emplDict[emplID].EmployeeTypeID))
                        emplRules = rulesDist[ascoDict[emplID].IntegerValue4][emplDict[emplID].EmployeeTypeID];

                    while (schIndex + 1 < emplSchedules[emplID].Count)
                    {
                        if (emplSchedules[emplID][schIndex].Date <= from.Date && emplSchedules[emplID][schIndex + 1].Date <= from.Date)
                        {
                            schIndex++;
                            continue;
                        }

                        if (emplSchedules[emplID][schIndex].Date >= to.Date && emplSchedules[emplID][schIndex + 1].Date >= to.Date)
                        {
                            schIndex++;
                            continue;
                        }

                        if (schemas.ContainsKey(emplSchedules[emplID][schIndex].TimeSchemaID) && schemas.ContainsKey(emplSchedules[emplID][schIndex + 1].TimeSchemaID))
                        {
                            // get successive time schemas from schedules
                            WorkTimeSchemaTO prevSchema = schemas[emplSchedules[emplID][schIndex].TimeSchemaID];
                            WorkTimeSchemaTO nextSchema = schemas[emplSchedules[emplID][schIndex + 1].TimeSchemaID];

                            if (additionalValidation)
                            {
                                bool is8hPrevShift = is8HourShift(prevSchema);
                                bool is8hNextShift = is8HourShift(nextSchema);

                                // 10 hours shift could be assigned only on Monday
                                if (!is8hPrevShift && emplSchedules[emplID][schIndex].Date.DayOfWeek != DayOfWeek.Monday && emplSchedules[emplID][schIndex].Date >= new DateTime(2012, 9, 3))
                                {
                                    invalidDate = emplSchedules[emplID][schIndex].Date;
                                    break;
                                }

                                if (!is8hNextShift && emplSchedules[emplID][schIndex + 1].Date.DayOfWeek != DayOfWeek.Monday && emplSchedules[emplID][schIndex + 1].Date > new DateTime(2012, 9, 3))
                                {
                                    invalidDate = emplSchedules[emplID][schIndex + 1].Date;
                                    break;
                                }

                                // 10 hours shift and 8 hours shift can not be assigned in same week
                                if (((!is8hPrevShift && is8hNextShift) || (is8hPrevShift && !is8hNextShift)) && emplSchedules[emplID][schIndex + 1].Date.DayOfWeek != DayOfWeek.Monday
                                     && emplSchedules[emplID][schIndex + 1].Date > new DateTime(2012, 9, 3))
                                {
                                    invalidDate = emplSchedules[emplID][schIndex + 1].Date;
                                    break;
                                }
                            }

                            // calculate day of schema on last day of previous schema
                            int prevSchemaDay = (emplSchedules[emplID][schIndex].StartCycleDay
                                + (int)emplSchedules[emplID][schIndex + 1].Date.Date.Subtract(emplSchedules[emplID][schIndex].Date.Date).TotalDays - 1) % prevSchema.CycleDuration;
                          
                            //28112017 NATALIJA MOGUCNOST PRELASKA IZ DNEVNE SMENE U NOCNU PO DANU
                            // get intervals for border days
                            if (prevSchema.Days.ContainsKey(prevSchemaDay) && nextSchema.Days.ContainsKey(emplSchedules[emplID][schIndex + 1].StartCycleDay))
                            {
                                // check beginning of third shift in previous schedule
                                foreach (int intNum in prevSchema.Days[prevSchemaDay].Keys)
                                {
                                    
                                    // if interval is beginning of third shift, check next day                        
                                    if (isThirdShiftBeginningInterval(prevSchema.Days[prevSchemaDay][intNum]))
                                    {
                                        bool found3rdEnd = false;

                                        foreach (int num in nextSchema.Days[emplSchedules[emplID][schIndex + 1].StartCycleDay].Keys)
                                        {
                                            if (isThirdShiftEndInterval(nextSchema.Days[emplSchedules[emplID][schIndex + 1].StartCycleDay][num]))
                                            {
                                                found3rdEnd = true;
                                            }
                                        }
                                        if (emplRules["LABOR LAW"].RuleValue == 0)
                                            found3rdEnd = true;

                                        if (!found3rdEnd)
                                        {
                                            invalidDate = emplSchedules[emplID][schIndex + 1].Date;
                                            break;
                                        }
                                        else if ((prevSchema.Type == Constants.schemaTypeNightFlexi && nextSchema.Type != Constants.schemaTypeNightFlexi)
                                            || prevSchema.Type != Constants.schemaTypeNightFlexi && nextSchema.Type == Constants.schemaTypeNightFlexi)
                                        {
                                            invalidDate = emplSchedules[emplID][schIndex + 1].Date;
                                            break;
                                        }
                                    }
                                    
                                }

                                if (!invalidDate.Equals(new DateTime()))
                                    break;

                                // check end of third sfift in next schedule
                                foreach (int intNum in nextSchema.Days[emplSchedules[emplID][schIndex + 1].StartCycleDay].Keys)
                                {
                                    // if interval is end of third shift, check previous day
                                    if (isThirdShiftEndInterval(nextSchema.Days[emplSchedules[emplID][schIndex + 1].StartCycleDay][intNum]))
                                    {
                                        bool found3rdStart = false;

                                        foreach (int num in prevSchema.Days[prevSchemaDay].Keys)
                                        {
                                            if (isThirdShiftBeginningInterval(prevSchema.Days[prevSchemaDay][num]))
                                            {
                                                found3rdStart = true;
                                            }
                                        }

                                        if (emplRules["LABOR LAW"].RuleValue == 0)
                                            found3rdStart = true;

                                        if (!found3rdStart)
                                        {
                                            invalidDate = emplSchedules[emplID][schIndex + 1].Date;
                                            break;
                                        }
                                        else if ((prevSchema.Type == Constants.schemaTypeNightFlexi && nextSchema.Type != Constants.schemaTypeNightFlexi)
                                            || prevSchema.Type != Constants.schemaTypeNightFlexi && nextSchema.Type == Constants.schemaTypeNightFlexi)
                                        {
                                            invalidDate = emplSchedules[emplID][schIndex + 1].Date;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                                invalidDate = emplSchedules[emplID][schIndex].Date;
                            //NATALIJA
                        }
                        else
                            invalidDate = emplSchedules[emplID][schIndex].Date;

                        if (!invalidDate.Equals(new DateTime()))
                            break;

                        schIndex++;
                    }

                    if (!invalidDate.Equals(new DateTime()))
                        break;

                    //Dictionary<string, RuleTO> emplRules = new Dictionary<string, RuleTO>();
                    //if (ascoDict.ContainsKey(emplID) && emplDict.ContainsKey(emplID) && rulesDist.ContainsKey(ascoDict[emplID].IntegerValue4)
                    //    && rulesDist[ascoDict[emplID].IntegerValue4].ContainsKey(emplDict[emplID].EmployeeTypeID))
                    //    emplRules = rulesDist[ascoDict[emplID].IntegerValue4][emplDict[emplID].EmployeeTypeID];

                    if (checkLaborLaw && emplRules.ContainsKey(Constants.RuleLaborLaw) && emplRules[Constants.RuleLaborLaw].RuleValue == Constants.yesInt)
                    {
                        // validate each day of interval, there must be at least one time free day in each seven days, and 12h between each shift
                        DateTime lastFreeDay = from.Date.AddDays(-1);
                        DateTime currDay = from.Date;
                        Dictionary<DateTime, List<WorkTimeIntervalTO>> emplIntervals = new Dictionary<DateTime, List<WorkTimeIntervalTO>>();
                        while (currDay.Date <= to.Date)
                        {
                            // get intervals for current and next day
                            List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();
                            List<WorkTimeIntervalTO> nextIntervals = new List<WorkTimeIntervalTO>();

                            if (emplIntervals.ContainsKey(currDay.Date))
                                dayIntervals = emplIntervals[currDay.Date];
                            else
                            {
                                dayIntervals = Common.Misc.getTimeSchemaInterval(currDay.Date, emplSchedules[emplID], schemas);
                                emplIntervals.Add(currDay.Date, dayIntervals);
                            }

                            if (currDay.Date < to.Date)
                            {
                                if (emplIntervals.ContainsKey(currDay.Date.AddDays(1)))
                                    nextIntervals = emplIntervals[currDay.Date.AddDays(1)];
                                else
                                {
                                    nextIntervals = Common.Misc.getTimeSchemaInterval(currDay.Date.AddDays(1), emplSchedules[emplID], schemas);
                                    emplIntervals.Add(currDay.Date.AddDays(1), nextIntervals);
                                }
                            }

                            // calculate gaps between intervals, each must be at least 12h, remember last free day and check if there is more then 7 days between last two free days
                            // if number of intervals is 2, check gap between intervals                             
                            int gap = 0;
                            if (dayIntervals.Count == 0)
                            {
                                gap = 24;
                                lastFreeDay = currDay.Date;
                            }
                            else
                            {
                                int intervalDuration = 0;
                                for (int i = 0; i < dayIntervals.Count; i++)
                                {
                                    intervalDuration += (int)dayIntervals[i].EndTime.TimeOfDay.Subtract(dayIntervals[i].StartTime.TimeOfDay).TotalMinutes;

                                    if (dayIntervals[i].EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                        intervalDuration++;

                                    gap = getNextIntervalGap(i, dayIntervals, nextIntervals);

                                    // maybe to put values for free day and minimal interval gaps into rules
                                    if (gap < Constants.FiatShiftMinimalGap)
                                    {
                                        invalidDate = currDay.Date.AddDays(1); // add one day becouse in message is shown previous and invalid day
                                        break;
                                    }
                                }

                                if (intervalDuration == 0)
                                    lastFreeDay = currDay.Date;
                            }

                            if (!invalidDate.Equals(new DateTime()))
                                break;

                            // check when was last free day
                            int firstSchemaFreeDay = Constants.FiatShiftMinimalFreeDayGap;
                            if ((int)currDay.Date.Subtract(lastFreeDay.Date).TotalDays >= Constants.FiatShiftMinimalFreeDayGap)
                            {
                                // 08.07.2013. Sanja - check if schema first free day is greater then 7 and if is so, compare with that value
                                // this change is for industrial schemas (I-I-II-II-III.-.III.-III.-X) - 8th day is free
                                WorkTimeSchemaTO daySch = new WorkTimeSchemaTO();
                                if (dayIntervals.Count > 0 && schemas.ContainsKey(dayIntervals[0].TimeSchemaID))
                                    daySch = schemas[dayIntervals[0].TimeSchemaID];

                                int freeDay = 0;
                                foreach (int day in daySch.Days.Keys)
                                {
                                    if (daySch.Days[day].Count == 0 ||
                                        (daySch.Days[day].Count == 1 && daySch.Days[day][0].StartTime.TimeOfDay == new TimeSpan(0, 0, 0) && daySch.Days[day][0].EndTime.TimeOfDay == new TimeSpan(0, 0, 0)))
                                    {
                                        freeDay = day;
                                        break;
                                    }
                                }

                                if (freeDay >= Constants.FiatShiftMinimalFreeDayGap)
                                    firstSchemaFreeDay = freeDay + 1;
                            }

                            if ((int)currDay.Date.Subtract(lastFreeDay.Date).TotalDays >= firstSchemaFreeDay)
                            {
                                invalidDate = currDay.Date.AddDays(1); // add one day becouse in message is shown previous and invalid day
                                break;
                            }

                            currDay = currDay.AddDays(1).Date;
                            continue;
                        }
                    }

                    if (!invalidDate.Equals(new DateTime()))
                        break;

                    if (additionalValidation)
                    {
                        // validate group and individual fund of hours                        
                        List<EmployeeGroupsTimeScheduleTO> emplGroupSchedules = new List<EmployeeGroupsTimeScheduleTO>();

                        if (emplGroups.ContainsKey(emplID) && groupSchedules.ContainsKey(emplGroups[emplID]))
                            emplGroupSchedules = groupSchedules[emplGroups[emplID]];

                        int grpHrs = 0;
                        int individualHrs = 0;

                        DateTime currDate = from.Date;
                        while (currDate.Date <= to.Date.AddDays(1))
                        {
                            // get intervals for current date
                            List<WorkTimeIntervalTO> emplIntervals = getTimeSchemaInterval(currDate.Date, emplSchedules[emplID], schemas);
                            List<WorkTimeIntervalTO> groupIntervals = getTimeSchemaIntervalGroup(currDate.Date, emplGroupSchedules, schemas);

                            int weekGrpHrs = 0;
                            int weekIndividualHrs = 0;
                            foreach (WorkTimeIntervalTO interval in emplIntervals)
                            {
                                // on Monday, check fund for last week
                                if (currDate.DayOfWeek == DayOfWeek.Monday)
                                {
                                    if (interval.StartTime.Hour == 0 && interval.StartTime.Minute == 0 && !currDate.Date.Equals(to.Date.AddDays(1)))
                                        individualHrs += (int)interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalHours;

                                    if (weekIndividualHrs == 0)
                                    {
                                        weekIndividualHrs = individualHrs;
                                        individualHrs = 0;
                                    }

                                    if (!(interval.StartTime.Hour == 0 && interval.StartTime.Minute == 0))
                                        individualHrs += (int)interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalHours;
                                }
                                else
                                    individualHrs += (int)interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalHours;
                            }

                            foreach (WorkTimeIntervalTO interval in groupIntervals)
                            {
                                // on Monday, check fund for last week
                                if (currDate.DayOfWeek == DayOfWeek.Monday)
                                {
                                    if (interval.StartTime.Hour == 0 && interval.StartTime.Minute == 0 && !currDate.Date.Equals(to.Date.AddDays(1)))
                                        grpHrs += (int)interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalHours;

                                    if (weekGrpHrs == 0)
                                    {
                                        weekGrpHrs = grpHrs;
                                        grpHrs = 0;
                                    }

                                    if (!(interval.StartTime.Hour == 0 && interval.StartTime.Minute == 0))
                                        grpHrs += (int)interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalHours;
                                }
                                else
                                    grpHrs += (int)interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalHours;
                            }

                            if (currDate.DayOfWeek == DayOfWeek.Monday && !currDate.Date.Equals(from.Date) && currDate.Date >= new DateTime(2012, 9, 10))
                            {
                                if (weekGrpHrs != weekIndividualHrs)
                                {
                                    invalidDate = currDate.Date.AddDays(-7);
                                    validFundHrs = false;
                                    break;
                                }
                            }

                            currDate = currDate.AddDays(1);
                        }
                    }

                    if (!invalidDate.Equals(new DateTime()))
                        break;
                }

                return invalidDate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Dictionary<int, WorkTimeSchemaTO> getSchemas(IDbTransaction trans, object connection)
        {
            try
            {
                Dictionary<int, WorkTimeSchemaTO> schDict = new Dictionary<int, WorkTimeSchemaTO>();

                // get time schema dictionary
                TimeSchema sch;

                if (connection != null)
                    sch = new TimeSchema(connection);
                else
                    sch = new TimeSchema();

                List<WorkTimeSchemaTO> schemas = new List<WorkTimeSchemaTO>();

                if (trans != null)
                    schemas = sch.Search(trans);
                else
                    schemas = sch.Search();

                foreach (WorkTimeSchemaTO schema in schemas)
                {
                    if (!schDict.ContainsKey(schema.TimeSchemaID))
                        schDict.Add(schema.TimeSchemaID, schema);
                    else
                        schDict[schema.TimeSchemaID] = schema;
                }

                return schDict;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Dictionary<int, WorkTimeSchemaTO> getSchemasSortByTime(IDbTransaction trans, object connection)
        {
            try
            {
                // get time schema dictionary
                TimeSchema sch;

                if (connection != null)
                    sch = new TimeSchema(connection);
                else
                    sch = new TimeSchema();

                return sch.SearchDictionary(trans);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static WorkingUnitTO getEmplCostCenter(EmployeeTO empl, Dictionary<int, WorkingUnitTO> wuDict, object conn)
        {
            try
            {
                WorkingUnitTO ccWU = new WorkingUnitTO();

                if (wuDict.ContainsKey(empl.WorkingUnitID))
                    ccWU = wuDict[empl.WorkingUnitID];

                string costumer = getCustomer(conn);
                int cost = 0;
                bool costum = int.TryParse(costumer, out cost);

                if (costum)
                {
                    if ((cost == (int)Constants.Customers.FIAT))
                    {
                        if (wuDict.ContainsKey(empl.WorkingUnitID) && wuDict.ContainsKey(wuDict[empl.WorkingUnitID].ParentWorkingUID)
                            && wuDict.ContainsKey(wuDict[wuDict[empl.WorkingUnitID].ParentWorkingUID].ParentWorkingUID))
                            ccWU = wuDict[wuDict[wuDict[empl.WorkingUnitID].ParentWorkingUID].ParentWorkingUID];
                    }
                }

                return ccWU;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<IOPairProcessedTO> getWeekPairs(List<IOPairProcessedTO> periodWholeWeekPairs, DateTime date)
        {
            try
            {
                List<IOPairProcessedTO> weekPairs = new List<IOPairProcessedTO>();

                int dayNum = 0;
                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        dayNum = 0;
                        break;
                    case DayOfWeek.Tuesday:
                        dayNum = 1;
                        break;
                    case DayOfWeek.Wednesday:
                        dayNum = 2;
                        break;
                    case DayOfWeek.Thursday:
                        dayNum = 3;
                        break;
                    case DayOfWeek.Friday:
                        dayNum = 4;
                        break;
                    case DayOfWeek.Saturday:
                        dayNum = 5;
                        break;
                    case DayOfWeek.Sunday:
                        dayNum = 6;
                        break;
                }

                DateTime weekBegining = date.AddDays(-dayNum).Date; // first day of current week
                DateTime weekEnd = date.AddDays(7 - dayNum).Date; // first day of next week

                foreach (IOPairProcessedTO pair in periodWholeWeekPairs)
                {
                    if (pair.IOPairDate.Date >= weekBegining.Date && pair.IOPairDate.Date < weekEnd.Date && !(pair.StartTime.Hour == 0 && pair.StartTime.Minute == 0))
                        weekPairs.Add(pair);
                }

                return weekPairs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool isFirstInWeek(DateTime periodStart, DateTime periodEnd, DateTime pairDate, List<IOPairProcessedTO> weekPairs)
        {
            try
            {
                bool isFirst = true;

                foreach (IOPairProcessedTO pair in weekPairs)
                {
                    if (pair.IOPairDate.Date < periodStart.Date || pair.IOPairDate.Date > periodEnd.Date)
                        continue;

                    if (pair.IOPairDate.Date.Equals(pairDate.Date))
                        isFirst = true;
                    else
                        isFirst = false;

                    break;
                }

                return isFirst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // validate and change counters
        // IF NEW RETURN STRING IS ADDED, ADD IT TO CONSTANTS.MassiveInputFailedMessages()!!!!!
        public static string validatePairsPassType(int emplID, EmployeeAsco4TO emplAsco, DateTime periodStart, DateTime periodEnd, List<IOPairProcessedTO> newPairs, List<IOPairProcessedTO> oldPairs,
            List<IOPairProcessedTO> dayPairs, ref Dictionary<int, EmployeeCounterValueTO> emplCounters, Dictionary<string, RuleTO> rules, Dictionary<int, PassTypeTO> passTypesAllDic,
            Dictionary<int, PassTypeLimitTO> passTypeLimits, Dictionary<int, WorkTimeSchemaTO> schDict, Dictionary<DateTime, WorkTimeSchemaTO> daySchemas, Dictionary<DateTime, List<WorkTimeIntervalTO>> dayIntervals,
            List<IOPairProcessedTO> fromWeekAnnualLeavePairs, List<IOPairProcessedTO> toWeekAnnualLeavePairs, Dictionary<int, int> paidLeaveElementaryPairsDict,
            List<IOPairProcessedTO> newCollectivePairs, List<DateTime> exceptDates, List<IOPairProcessedTO> prevDayPairs, object conn, bool checkLimit,
            bool checkEveryThirdShift, bool checkThirdShift, bool checkCountersAfterChange)
        {
            try
            {
                List<IOPairProcessedTO> wholeWeekPeriodPairs = new List<IOPairProcessedTO>();

                if (fromWeekAnnualLeavePairs != null)
                    wholeWeekPeriodPairs.AddRange(fromWeekAnnualLeavePairs);
                foreach (IOPairProcessedTO oldPair in oldPairs)
                {
                    if ((rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && oldPair.PassTypeID == rules[Constants.RuleCompanyAnnualLeave].RuleValue)
                        || (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && oldPair.PassTypeID == rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue))
                        wholeWeekPeriodPairs.Add(oldPair);
                }
                if (toWeekAnnualLeavePairs != null)
                    wholeWeekPeriodPairs.AddRange(toWeekAnnualLeavePairs);

                bool checkCountersNegativeValue = false;
                for (int i = 0; i < oldPairs.Count; i++)
                {
                    int pairDuration = (int)oldPairs[i].EndTime.Subtract(oldPairs[i].StartTime).TotalMinutes;

                    if (pairDuration > 0 && oldPairs[i].EndTime.Hour == 23 && oldPairs[i].EndTime.Minute == 59)
                        pairDuration++;

                    // if old type is overtime paid, decrease counter - counter value is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyOvertimePaid) && rules[Constants.RuleCompanyOvertimePaid].RuleValue == oldPairs[i].PassTypeID
                        && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.OvertimeCounter))
                    {
                        emplCounters[(int)Constants.EmplCounterTypes.OvertimeCounter].Value -= pairDuration;

                        if (checkLimit && emplCounters[(int)Constants.EmplCounterTypes.OvertimeCounter].Value < 0)
                        {
                            if (!checkCountersAfterChange)
                                return "overtimeNegative";
                            else
                                checkCountersNegativeValue = true;
                        }
                    }

                    int bhCounterDuration = pairDuration;
                    // calculate value for counter considering bank hour rounding rule
                    if (rules.ContainsKey(Constants.RuleBankHoursUsedRounding))
                    {
                        int bhRounding = rules[Constants.RuleBankHoursUsedRounding].RuleValue;

                        if (pairDuration % bhRounding != 0)
                        {
                            bhCounterDuration += bhRounding - pairDuration % bhRounding;
                        }
                    }

                    // if old type is bank hour, decrease counter - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyBankHour) && rules[Constants.RuleCompanyBankHour].RuleValue == oldPairs[i].PassTypeID
                        && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                    {
                        emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value -= pairDuration;

                        if (checkLimit && emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value < 0)
                        {
                            if (!checkCountersAfterChange)
                                return "bankHourNegative";
                            else
                                checkCountersNegativeValue = true;
                        }
                    }

                    // if old type is bank hour used, increase counter - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyBankHourUsed) && rules[Constants.RuleCompanyBankHourUsed].RuleValue == oldPairs[i].PassTypeID
                        && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                        emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value += bhCounterDuration;

                    // if old type is stop working done, increase counter - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyStopWorkingDone) && rules[Constants.RuleCompanyStopWorkingDone].RuleValue == oldPairs[i].PassTypeID
                        && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                        emplCounters[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value += pairDuration;

                    // if old type is stop working, decrease counter - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyStopWorking) && rules[Constants.RuleCompanyStopWorking].RuleValue == oldPairs[i].PassTypeID
                        && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                    {
                        emplCounters[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value -= pairDuration;

                        if (checkLimit && emplCounters[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value < 0)
                        {
                            if (!checkCountersAfterChange)
                                return "stopWorkingNegative";
                            else
                                checkCountersNegativeValue = true;
                        }
                    }

                    int onCounterDuration = pairDuration;
                    // calculate value for counter considering overtime used rounding rule
                    if (rules.ContainsKey(Constants.RuleInitialOvertimeUsedRounding))
                    {
                        int onRounding = rules[Constants.RuleInitialOvertimeUsedRounding].RuleValue;

                        if (pairDuration % onRounding != 0)
                        {
                            onCounterDuration += onRounding - pairDuration % onRounding;
                        }
                    }

                    // if old type is overtime not justified used, increase counter - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyInitialOvertimeUsed) && rules[Constants.RuleCompanyInitialOvertimeUsed].RuleValue == oldPairs[i].PassTypeID
                        && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.NotJustifiedOvertime))
                        emplCounters[(int)Constants.EmplCounterTypes.NotJustifiedOvertime].Value += onCounterDuration;

                    // for night shift, for whole day absences, counters and limits, decrease counters only for first night shift pair
                    if (oldPairs[i].StartTime.Hour == 0 && oldPairs[i].StartTime.Minute == 0 && !checkEveryThirdShift)
                        continue;

                    // Sanja 17.09.2012. - if changing counters for second interval night shifts for whole day absences, do not check third shifts counters again
                    if (((oldPairs[i].StartTime.Hour == 0 && oldPairs[i].StartTime.Minute == 0) || (oldPairs[i].EndTime.Hour == 23 && oldPairs[i].EndTime.Minute == 59)) && !checkThirdShift)
                        continue;

                    // if oldPT is annual leave, decrement annual leave counter
                    if ((rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && rules[Constants.RuleCompanyAnnualLeave].RuleValue == oldPairs[i].PassTypeID)
                        || (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue == oldPairs[i].PassTypeID))
                    {
                        int counterValue = 1;
                        if (daySchemas.ContainsKey(oldPairs[i].IOPairDate.Date) && is10HourShift(daySchemas[oldPairs[i].IOPairDate.Date]))
                        {
                            if (fromWeekAnnualLeavePairs == null && toWeekAnnualLeavePairs == null)
                            {
                                // get week annual leave pairs
                                int annualLeaveDays = 0;
                                IOPairProcessed annualPair;

                                if (conn != null)
                                    annualPair = new IOPairProcessed(conn);
                                else
                                    annualPair = new IOPairProcessed();

                                List<IOPairProcessedTO> annualLeaveWeekPairs = annualPair.SearchWeekPairs(emplID, oldPairs[i].IOPairDate, false, getAnnualLeaveTypesString(rules), null);

                                // if day pairs contains both third shifts for current and previous day, third sfift for current day is already excluded
                                // third sfiht for previous day should be excluded from annualLeaveWeekPairs, and current state from day pairs should be added to those pairs
                                IEnumerator<IOPairProcessedTO> annualPairEnumerator = annualLeaveWeekPairs.GetEnumerator();
                                while (annualPairEnumerator.MoveNext())
                                {
                                    if ((annualPairEnumerator.Current.StartTime.Hour == 0 && annualPairEnumerator.Current.StartTime.Minute == 0)
                                        || (annualPairEnumerator.Current.IOPairDate.Date.Equals(oldPairs[i].IOPairDate.AddDays(-1))
                                        && annualPairEnumerator.Current.EndTime.Hour == 23 && annualPairEnumerator.Current.EndTime.Minute == 59))
                                    {
                                        annualLeaveWeekPairs.Remove(annualPairEnumerator.Current);
                                        annualPairEnumerator = annualLeaveWeekPairs.GetEnumerator();
                                    }
                                }

                                foreach (IOPairProcessedTO iopair in dayPairs)
                                {
                                    if ((rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && iopair.PassTypeID == rules[Constants.RuleCompanyAnnualLeave].RuleValue)
                                        || (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && iopair.PassTypeID == rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue))
                                    {
                                        // if changing third shift from previous day, add already changed pairs from this day
                                        if (oldPairs[i].StartTime.Hour == 0 && oldPairs[i].StartTime.Minute == 0)
                                        {
                                            if (iopair.IOPairDate.Date.Equals(oldPairs[i].IOPairDate.Date) && !iopair.compareAL(oldPairs[i], getAnnualLeaveTypes(rules)))
                                                annualLeaveWeekPairs.Add(iopair);
                                        }

                                        // if changing third shift from current day, add already changed pairs for this or previous day
                                        if (oldPairs[i].EndTime.Hour == 23 && oldPairs[i].EndTime.Minute == 59)
                                        {
                                            if ((iopair.IOPairDate.Date.Equals(oldPairs[i].IOPairDate.Date) && !iopair.compareAL(oldPairs[i], getAnnualLeaveTypes(rules)) && !(iopair.StartTime.Hour == 0 && iopair.StartTime.Minute == 0))
                                                || (iopair.IOPairDate.Date.Equals(oldPairs[i].IOPairDate.AddDays(-1).Date) && iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59))
                                                annualLeaveWeekPairs.Add(iopair);
                                        }
                                    }
                                }

                                annualLeaveDays = annualLeaveWeekPairs.Count;
                                //foreach (IOPairProcessedTO aPair in annualLeaveWeekPairs)
                                //{
                                //    // second night shift belongs to previous day
                                //    if (aPair.StartTime.Hour == 0 && aPair.StartTime.Minute == 0)
                                //        continue;

                                //    annualLeaveDays++;
                                //}

                                // if fourts day of week is changed, subtract/add two days
                                if (annualLeaveDays == (Constants.fullWeek10hShift - 1))
                                    counterValue++;
                            }
                            else
                            {
                                List<IOPairProcessedTO> weekAnnualLeavePairs = getWeekPairs(wholeWeekPeriodPairs, oldPairs[i].IOPairDate.Date);

                                if (isFirstInWeek(periodStart, periodEnd, oldPairs[i].IOPairDate.Date, weekAnnualLeavePairs))
                                {
                                    // if full week of annual leaves is changed, subtract/add two days
                                    if (weekAnnualLeavePairs.Count == Constants.fullWeek10hShift)
                                        counterValue++;
                                }
                            }
                        }

                        if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))
                            emplCounters[(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value -= counterValue;
                    }

                    // if old type is paid leave type (has composite limit), decrease counter - counter is in days
                    if (passTypesAllDic.ContainsKey(oldPairs[i].PassTypeID) && passTypesAllDic[oldPairs[i].PassTypeID].LimitCompositeID != -1
                        && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.PaidLeaveCounter))
                        emplCounters[(int)Constants.EmplCounterTypes.PaidLeaveCounter].Value--;
                }

                wholeWeekPeriodPairs = new List<IOPairProcessedTO>();

                if (fromWeekAnnualLeavePairs != null)
                    wholeWeekPeriodPairs.AddRange(fromWeekAnnualLeavePairs);
                foreach (IOPairProcessedTO newPair in newPairs)
                {
                    if ((rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && newPair.PassTypeID == rules[Constants.RuleCompanyAnnualLeave].RuleValue)
                        || (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && newPair.PassTypeID == rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue))
                        wholeWeekPeriodPairs.Add(newPair);
                }
                if (toWeekAnnualLeavePairs != null)
                    wholeWeekPeriodPairs.AddRange(toWeekAnnualLeavePairs);

                int overtimeValue = 0;
                int bankHourValue = 0;
                int overtimePaidDayDuration = 0;
                int bankHourDayDuration = 0;
                int overtimePaidWeekDuration = 0;
                int bankHourWeekDuration = 0;
                for (int i = 0; i < newPairs.Count; i++)
                {
                    bool isOvertimeCheckGapDurationPair = false;

                    int pairDuration = (int)newPairs[i].EndTime.Subtract(newPairs[i].StartTime).TotalMinutes;

                    if (pairDuration > 0 && newPairs[i].EndTime.Hour == 23 && newPairs[i].EndTime.Minute == 59)
                        pairDuration++;

                    int bhCounterDuration = pairDuration;
                    int bhRounding = 1;
                    // calculate value for counter considering bank hour rounding rule
                    if (rules.ContainsKey(Constants.RuleBankHoursUsedRounding))
                    {
                        bhRounding = rules[Constants.RuleBankHoursUsedRounding].RuleValue;

                        if (pairDuration % bhRounding != 0)
                        {
                            bhCounterDuration += bhRounding - pairDuration % bhRounding;
                        }
                    }

                    // if new type is overtime paid check limits and increase counter if change is valid - counter value is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyOvertimePaid) && rules[Constants.RuleCompanyOvertimePaid].RuleValue == newPairs[i].PassTypeID)
                    {
                        isOvertimeCheckGapDurationPair = true;

                        // calculate overtime day duration
                        overtimeValue = pairDuration;
                    }

                    // if new type is bank hour check limits and increase counter if change is valid - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyBankHour) && rules[Constants.RuleCompanyBankHour].RuleValue == newPairs[i].PassTypeID)
                    {
                        isOvertimeCheckGapDurationPair = true;

                        // calculate bank hour day duration
                        bankHourValue = pairDuration;
                    }

                    // if new type is stop working done decrease counter if there are enough minutes - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyStopWorkingDone) && rules[Constants.RuleCompanyStopWorkingDone].RuleValue == newPairs[i].PassTypeID)
                    {
                        isOvertimeCheckGapDurationPair = true;

                        // decrease counter
                        if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                        {
                            // 19.06.2013. Sanja
                            // if new pair is in previous month, validate against hours earned till month beginning (counter - earned hours from month beginning),
                            int swValueToValidate = emplCounters[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value;

                            DateTime pairDate = newPairs[i].IOPairDate.Date;

                            // for first day of month, check if pair belongs to previous month
                            WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                            if (daySchemas.ContainsKey(newPairs[i].IOPairDate.Date))
                                sch = daySchemas[newPairs[i].IOPairDate.Date];
                            List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                            if (dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date))
                                intervals = dayIntervals[newPairs[i].IOPairDate.Date];
                            if (newPairs[i].IOPairDate.Day == 1)
                            {
                                if (isPreviousDayPair(newPairs[i], passTypesAllDic, dayPairs, sch, intervals))
                                    pairDate = pairDate.AddDays(-1);
                            }

                            DateTime firstMonthDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);

                            if (pairDate.Date < firstMonthDay.Date)
                            {
                                if (rules.ContainsKey(Constants.RuleCompanyStopWorking))
                                {
                                    // get pairs earned after month beginning
                                    if (pairDate.Date < firstMonthDay.Date)
                                        //bankHoursValueToValidate -= pairProcessed.SearchEarnedBankHours(newPairs[i].EmployeeID, rules[Constants.RuleCompanyBankHour].RuleValue, 
                                        //firstMonthDay.Date);
                                        swValueToValidate -= SearchHours(newPairs[i].EmployeeID, rules[Constants.RuleCompanyStopWorking].RuleValue,
                                            rules[Constants.RuleCompanyStopWorkingDone].RuleValue, 1, firstMonthDay.Date, new DateTime(), conn, passTypesAllDic, schDict);
                                }
                            }

                            if (checkLimit && swValueToValidate < pairDuration)
                                return "notEnoughStopWorkingHours";

                            emplCounters[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value -= pairDuration;
                        }
                    }

                    // check gap duration between overtime pairs and regular pairs. There must be at least 12 hours gap between regular hours and first pair, or last pair and regular hours
                    // check is performed if labor law rule is set
                    if (checkLimit && isOvertimeCheckGapDurationPair && rules.ContainsKey(Constants.RuleLaborLaw) && rules[Constants.RuleLaborLaw].RuleValue == Constants.yesInt)
                    {
                        IOPairProcessed pairProcessed;
                        if (conn != null)
                            pairProcessed = new IOPairProcessed(conn);
                        else
                            pairProcessed = new IOPairProcessed();

                        int minimalGap = Constants.FiatShiftMinimalGap * 60; // 12 hours, gap is in minutes

                        // list of noncounting types
                        List<int> nonCountingTypes = new List<int>();
                        nonCountingTypes.Add(Constants.overtimeUnjustified);
                        nonCountingTypes.Add(Constants.ptEmptyInterval);
                        if (rules.ContainsKey(Constants.RuleCompanyInitialNightOvertime))
                            nonCountingTypes.Add(rules[Constants.RuleCompanyInitialNightOvertime].RuleValue);
                        if (rules.ContainsKey(Constants.RuleCompanyOvertimeRejected))
                            nonCountingTypes.Add(rules[Constants.RuleCompanyOvertimeRejected].RuleValue);

                        // get gap in currrent day after pair end
                        DateTime checkStart = newPairs[i].EndTime;
                        int gap = 0;
                        bool regularFound = false;
                        for (int index = 0; index < dayPairs.Count; index++)
                        {
                            // skip pairs from other day
                            if (!dayPairs[index].IOPairDate.Date.Equals(newPairs[i].IOPairDate.Date))
                                continue;

                            // skip pairs before checking pair, and checking pair
                            if (dayPairs[index].StartTime <= newPairs[i].StartTime)
                                continue;

                            // skip pairs of noncounting types
                            if (nonCountingTypes.Contains(dayPairs[index].PassTypeID))
                                continue;

                            gap = (int)dayPairs[index].StartTime.Subtract(checkStart).TotalMinutes;

                            if (passTypesAllDic.ContainsKey(dayPairs[index].PassTypeID) && passTypesAllDic[dayPairs[index].PassTypeID].IsPass != Constants.overtimePassType)
                            {
                                regularFound = true;
                                break;
                            }

                            if (gap >= minimalGap)
                                break;

                            checkStart = dayPairs[index].EndTime;
                        }

                        // if gap is less then minimal
                        if (gap < minimalGap)
                        {
                            // if regular pair is not found in current day and gap is less then minimal try checking in next day
                            bool checkStartGap = false;
                            if (!regularFound)
                            {
                                // get gap until next day midnight
                                if (checkStart.TimeOfDay != new TimeSpan(23, 59, 0))
                                    gap = (int)newPairs[i].IOPairDate.Date.AddDays(1).Subtract(checkStart).TotalMinutes;
                                else
                                    gap = 0;

                                if (gap < minimalGap)
                                {
                                    // get next day pairs
                                    List<DateTime> nextDateList = new List<DateTime>();
                                    nextDateList.Add(newPairs[i].IOPairDate.Date.AddDays(1));
                                    List<IOPairProcessedTO> nextDayPairs = pairProcessed.SearchAllPairsForEmpl(newPairs[i].EmployeeID.ToString().Trim(), nextDateList, "");

                                    checkStart = newPairs[i].IOPairDate.Date.AddDays(1);
                                    for (int index = 0; index < nextDayPairs.Count; index++)
                                    {
                                        // skip pairs of noncounting types
                                        if (nonCountingTypes.Contains(nextDayPairs[index].PassTypeID))
                                            continue;

                                        gap += (int)nextDayPairs[index].StartTime.Subtract(checkStart).TotalMinutes;

                                        if (passTypesAllDic.ContainsKey(nextDayPairs[index].PassTypeID) && passTypesAllDic[nextDayPairs[index].PassTypeID].IsPass != Constants.overtimePassType)
                                        {
                                            regularFound = true;
                                            break;
                                        }

                                        if (gap >= minimalGap)
                                            break;

                                        checkStart = nextDayPairs[index].EndTime;
                                        gap = 0;
                                    }

                                    if (gap < minimalGap)
                                    {
                                        if (!regularFound)
                                        {
                                            // get gap until next day midnight
                                            if (checkStart.TimeOfDay != new TimeSpan(23, 59, 0))
                                                gap = (int)newPairs[i].IOPairDate.Date.AddDays(2).Subtract(checkStart).TotalMinutes;
                                            else
                                                gap = 0;

                                            if (gap < minimalGap)
                                                checkStartGap = true;
                                        }
                                        else
                                            checkStartGap = true;
                                    }
                                }
                            }
                            else
                                checkStartGap = true;

                            if (checkStartGap)
                            {
                                // get gap in currrent day before pair start
                                DateTime checkEnd = newPairs[i].StartTime;
                                gap = 0;
                                regularFound = false;
                                for (int index = dayPairs.Count - 1; index >= 0; index--)
                                {
                                    // skip pairs from other day
                                    if (!dayPairs[index].IOPairDate.Date.Equals(newPairs[i].IOPairDate.Date))
                                        continue;

                                    // skip pairs after checking pair, and checking pair
                                    if (dayPairs[index].StartTime >= newPairs[i].StartTime)
                                        continue;

                                    // skip pairs of noncounting types
                                    if (nonCountingTypes.Contains(dayPairs[index].PassTypeID))
                                        continue;

                                    gap = (int)checkEnd.Subtract(dayPairs[index].EndTime).TotalMinutes;

                                    if (passTypesAllDic.ContainsKey(dayPairs[index].PassTypeID) && passTypesAllDic[dayPairs[index].PassTypeID].IsPass != Constants.overtimePassType)
                                    {
                                        regularFound = true;
                                        break;
                                    }

                                    if (gap >= minimalGap)
                                        break;

                                    checkEnd = dayPairs[index].StartTime;
                                }

                                if (gap < minimalGap)
                                {
                                    // if regular pair is not found in current day and gap is less then minimal try checking in previous day                                    
                                    if (!regularFound)
                                    {
                                        // get gap until current day midnight                                        
                                        gap = (int)checkEnd.Subtract(newPairs[i].IOPairDate.Date).TotalMinutes;

                                        if (gap < minimalGap)
                                        {
                                            if (prevDayPairs == null)
                                            {
                                                // get previous day pairs
                                                List<DateTime> prevDateList = new List<DateTime>();
                                                prevDateList.Add(newPairs[i].IOPairDate.Date.AddDays(-1));
                                                prevDayPairs = pairProcessed.SearchAllPairsForEmpl(newPairs[i].EmployeeID.ToString().Trim(), prevDateList, "");
                                            }

                                            checkEnd = newPairs[i].IOPairDate.Date;
                                            for (int index = prevDayPairs.Count - 1; index >= 0; index--)
                                            {
                                                // skip pairs of noncounting types
                                                if (nonCountingTypes.Contains(prevDayPairs[index].PassTypeID))
                                                    continue;

                                                gap += (int)checkEnd.Subtract(prevDayPairs[index].EndTime).TotalMinutes;

                                                if (passTypesAllDic.ContainsKey(prevDayPairs[index].PassTypeID) && passTypesAllDic[prevDayPairs[index].PassTypeID].IsPass != Constants.overtimePassType)
                                                {
                                                    regularFound = true;
                                                    break;
                                                }

                                                if (gap >= minimalGap)
                                                    break;

                                                checkEnd = prevDayPairs[index].StartTime;
                                                gap = 0;
                                            }

                                            if (gap < minimalGap)
                                            {
                                                if (!regularFound)
                                                {
                                                    // get gap until previous day midnight                                                   
                                                    gap = (int)checkEnd.Subtract(newPairs[i].IOPairDate.Date.AddDays(-1)).TotalMinutes;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (gap < minimalGap)
                            return "notEnoughFreeHoursBetweenWorkingHours";
                    }

                    // if new type is overtime not justified used decrease counter if there are enough minutes - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyInitialOvertimeUsed) && rules[Constants.RuleCompanyInitialOvertimeUsed].RuleValue == newPairs[i].PassTypeID)
                    {
                        // decrease counter
                        if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.NotJustifiedOvertime))
                        {
                            int onCounterDuration = pairDuration;
                            int onRounding = 1;
                            // calculate value for counter considering bank hour rounding rule
                            if (rules.ContainsKey(Constants.RuleInitialOvertimeUsedRounding))
                            {
                                onRounding = rules[Constants.RuleInitialOvertimeUsedRounding].RuleValue;

                                if (pairDuration % onRounding != 0)
                                {
                                    onCounterDuration += onRounding - pairDuration % onRounding;
                                }
                            }

                            if (checkLimit && emplCounters[(int)Constants.EmplCounterTypes.NotJustifiedOvertime].Value < onCounterDuration)
                                return "notEnoughOvertimeNotJustifiedHours";

                            emplCounters[(int)Constants.EmplCounterTypes.NotJustifiedOvertime].Value -= onCounterDuration;
                        }
                    }

                    // if new type is bank hour used decrease counter if there are enough minutes - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyBankHourUsed) && rules[Constants.RuleCompanyBankHourUsed].RuleValue == newPairs[i].PassTypeID)
                    {
                        if (checkLimit && rules.ContainsKey(Constants.RuleVacationBeforeBankHours)
                            && rules[Constants.RuleVacationBeforeBankHours].RuleValue == Constants.yesInt)
                        {
                            // check if pair is from whole day absence, if is, check if there is annual leave to use first                            
                            int bhUsedDayDuration = (int)(newPairs[i].EndTime.TimeOfDay - newPairs[i].StartTime.TimeOfDay).TotalMinutes;
                            if (newPairs[i].EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                bhUsedDayDuration++;

                            // get pair belonging date
                            DateTime pairDate = newPairs[i].IOPairDate.Date;                           
                            WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                            if (daySchemas.ContainsKey(newPairs[i].IOPairDate.Date))
                                sch = daySchemas[newPairs[i].IOPairDate.Date];
                            List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                            if (dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date))
                                intervals = dayIntervals[newPairs[i].IOPairDate.Date];

                            if (isPreviousDayPair(newPairs[i], passTypesAllDic, dayPairs, sch, intervals))
                                pairDate = pairDate.AddDays(-1);
                                                        
                            IOPairProcessed bhUsedPair;
                            if (conn != null)
                                bhUsedPair = new IOPairProcessed(conn);
                            else
                                bhUsedPair = new IOPairProcessed();
                            bhUsedPair.IOPairProcessedTO.EmployeeID = newPairs[i].EmployeeID;
                            Dictionary<DateTime, List<IOPairProcessedTO>> pairsByDay = new Dictionary<DateTime, List<IOPairProcessedTO>>();
                            if (pairDate.Date != newPairs[i].IOPairDate.Date)
                            {                                
                                bhUsedPair.IOPairProcessedTO.IOPairDate = pairDate.Date;
                                if (!pairsByDay.ContainsKey(pairDate.Date))
                                    pairsByDay.Add(pairDate.Date, bhUsedPair.Search());
                            }

                            List<IOPairProcessedTO> pairsForDay = new List<IOPairProcessedTO>();
                            if (dayPairs == null)
                            {
                                bhUsedPair.IOPairProcessedTO.IOPairDate = newPairs[i].IOPairDate.Date;
                                pairsForDay = bhUsedPair.Search();
                            }
                            else
                            {
                                foreach (IOPairProcessedTO dayPair in dayPairs)
                                {
                                    if (dayPair.IOPairDate.Date == newPairs[i].IOPairDate.Date)
                                        pairsForDay.Add(dayPair);
                                }
                            }

                            if (!pairsByDay.ContainsKey(newPairs[i].IOPairDate.Date))
                                pairsByDay.Add(newPairs[i].IOPairDate.Date, pairsForDay);

                            if (pairDate.Date == newPairs[i].IOPairDate.Date)
                            {
                                bhUsedPair.IOPairProcessedTO.IOPairDate = newPairs[i].IOPairDate.Date.AddDays(1);
                                if (!pairsByDay.ContainsKey(newPairs[i].IOPairDate.Date.AddDays(1)))
                                    pairsByDay.Add(newPairs[i].IOPairDate.Date.AddDays(1), bhUsedPair.Search());
                            }

                            // add bank hours used value from changed day                           
                            foreach (DateTime date in pairsByDay.Keys)
                            {
                                foreach (IOPairProcessedTO pair in pairsByDay[date])
                                {
                                    if (pair.PassTypeID != rules[Constants.RuleCompanyBankHourUsed].RuleValue)
                                        continue;

                                    // get pair belonging date
                                    DateTime dayPairDate = pair.IOPairDate.Date;
                                    List<WorkTimeIntervalTO> dayPairIntervals = new List<WorkTimeIntervalTO>();
                                    if (dayIntervals.ContainsKey(pair.IOPairDate.Date))
                                        dayPairIntervals = dayIntervals[pair.IOPairDate.Date];
                                    WorkTimeSchemaTO dayPairSch = new WorkTimeSchemaTO();
                                    if (daySchemas.ContainsKey(pair.IOPairDate.Date))
                                        dayPairSch = daySchemas[pair.IOPairDate.Date];
                                    else if (dayPairIntervals.Count > 0 && schDict.ContainsKey(dayPairIntervals[0].TimeSchemaID))                                    
                                        dayPairSch = schDict[dayPairIntervals[0].TimeSchemaID];                                    

                                    if (isPreviousDayPair(pair, passTypesAllDic, pairsByDay[date], dayPairSch, dayPairIntervals))
                                        dayPairDate = dayPairDate.AddDays(-1);

                                    if (pairDate.Date != dayPairDate.Date)
                                        continue;

                                    int duration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                                    if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                                        duration++;

                                    if (pair.PassTypeID == rules[Constants.RuleCompanyBankHourUsed].RuleValue)
                                        bhUsedDayDuration += duration;
                                }
                            }

                            bool isALCandidate = (bhUsedDayDuration == Constants.dayDurationStandardShift * 60);

                            if (isALCandidate && (emplAsco.DatetimeValue4.Equals(new DateTime()) || emplAsco.DatetimeValue4.Date <= periodStart.Date))
                            {
                                int counterValue = 1;
                                
                                // get already used collective annual leave from last cut off date
                                DateTime startDate = new DateTime();
                                if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeaveReservation))
                                {
                                    startDate = new DateTime(DateTime.Now.Year, rules[Constants.RuleCompanyCollectiveAnnualLeaveReservation].RuleDateTime1.Month, rules[Constants.RuleCompanyCollectiveAnnualLeaveReservation].RuleDateTime1.Day);

                                    if (startDate.Date > DateTime.Now)
                                        startDate = startDate.AddYears(-1).Date;
                                }

                                int ptID = -1;
                                if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave))
                                    ptID = rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue;

                                int numOfDays = SearchCollectiveAnnualLeave(newPairs[i], emplID, ptID, startDate.Date, exceptDates, newCollectivePairs, rules, conn);

                                // get reserved days for collective annual leave
                                int reserved = 0;
                                if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeaveReservation))
                                    reserved = rules[Constants.RuleCompanyCollectiveAnnualLeaveReservation].RuleValue;

                                // employees that are excepted from annual leave reservation
                                if (emplAsco.IntegerValue10 == Constants.yesInt)
                                    reserved = 0;
                                
                                if (numOfDays <= reserved)
                                    reserved -= numOfDays;
                                else
                                    reserved = 0;

                                // check if there are annual leave days reserved
                                int alReserved = 0;
                                if (rules.ContainsKey(Constants.RuleCompanyAnnualLeaveReservation))
                                    alReserved = rules[Constants.RuleCompanyAnnualLeaveReservation].RuleValue;

                                // employees that are excepted from annual leave reservation
                                if (emplAsco.IntegerValue10 == Constants.yesInt)
                                    alReserved = 0;

                                // check if employee can use current year annual leave
                                int alReservedCurrYear = 0;
                                if (checkLimit && !emplAsco.DatetimeValue6.Equals(new DateTime()) && emplAsco.DatetimeValue6.Date > periodStart.Date
                                    && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter))
                                    alReservedCurrYear = emplCounters[(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value;

                                bool updateCounter = emplCounters.ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter) && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter)
                                        && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter)
                                        && (emplCounters[(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value + emplCounters[(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value - Math.Max(reserved + alReserved, alReservedCurrYear)
                                        >= emplCounters[(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value + counterValue);
                                        
                                if (updateCounter)
                                {
                                    return "useAnnualLeaveBeforeBankHours";
                                }
                            }
                        }

                        // decrease counter
                        if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                        {
                            // if new pair is in previous month, validate against hours earned till month beginning (counter - earned hours from month beginning),
                            // else if current month is not first after bank hours cut off date, validate against counter
                            // else validate against hours earned from month beginning
                            int bankHoursValueToValidate = emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value;

                            DateTime bankHourCutOffDate1 = new DateTime();
                            DateTime bankHourCutOffDate2 = new DateTime();
                            if (rules.ContainsKey(Constants.RuleBankHrsCutOffDate1))
                                bankHourCutOffDate1 = rules[Constants.RuleBankHrsCutOffDate1].RuleDateTime1;
                            if (rules.ContainsKey(Constants.RuleBankHrsCutOffDate2))
                                bankHourCutOffDate2 = rules[Constants.RuleBankHrsCutOffDate2].RuleDateTime1;

                            DateTime pairDate = newPairs[i].IOPairDate.Date;

                            // for first day of month, check if pair belongs to previous month
                            WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                            if (daySchemas.ContainsKey(newPairs[i].IOPairDate.Date))
                                sch = daySchemas[newPairs[i].IOPairDate.Date];
                            List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                            if (dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date))
                                intervals = dayIntervals[newPairs[i].IOPairDate.Date];
                            if (newPairs[i].IOPairDate.Day == 1)
                            {
                                if (isPreviousDayPair(newPairs[i], passTypesAllDic, dayPairs, sch, intervals))
                                    pairDate = pairDate.AddDays(-1);
                            }

                            DateTime firstPairsMonthDay = new DateTime(pairDate.Year, pairDate.Month, 1, 0, 0, 0);
                            DateTime firstMonthDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);

                            // calculate if pair month is month after cut off date
                            bool isMonthAfterCutOffDate = false;

                            if (!bankHourCutOffDate1.Equals(new DateTime()) && !bankHourCutOffDate2.Equals(new DateTime())
                                && !bankHourCutOffDate1.Equals(Constants.dateTimeNullValue()) && !bankHourCutOffDate2.Equals(Constants.dateTimeNullValue()))
                            {
                                isMonthAfterCutOffDate = firstPairsMonthDay.Equals(new DateTime(DateTime.Now.Year, bankHourCutOffDate1.Month, 1, 0, 0, 0).AddMonths(1))
                                || firstPairsMonthDay.Equals(new DateTime(DateTime.Now.Year - 1, bankHourCutOffDate1.Month, 1, 0, 0, 0).AddMonths(1))
                                || firstPairsMonthDay.Equals(new DateTime(DateTime.Now.Year, bankHourCutOffDate2.Month, 1, 0, 0, 0).AddMonths(1))
                                || firstPairsMonthDay.Equals(new DateTime(DateTime.Now.Year - 1, bankHourCutOffDate2.Month, 1, 0, 0, 0).AddMonths(1));
                            }

                            if (pairDate.Date < firstMonthDay.Date || isMonthAfterCutOffDate)
                            {
                                if (rules.ContainsKey(Constants.RuleCompanyBankHour))
                                {
                                    // get pairs earned after month beginning
                                    if (pairDate.Date < firstMonthDay.Date)
                                        //bankHoursValueToValidate -= pairProcessed.SearchEarnedBankHours(newPairs[i].EmployeeID, rules[Constants.RuleCompanyBankHour].RuleValue, 
                                        //firstMonthDay.Date);
                                        bankHoursValueToValidate -= SearchHours(newPairs[i].EmployeeID, rules[Constants.RuleCompanyBankHour].RuleValue,
                                            rules[Constants.RuleCompanyBankHourUsed].RuleValue, bhRounding, firstMonthDay.Date, new DateTime(), conn, passTypesAllDic, schDict);
                                    else
                                    {
                                        bankHoursValueToValidate = SearchHours(newPairs[i].EmployeeID, rules[Constants.RuleCompanyBankHour].RuleValue,
                                            rules[Constants.RuleCompanyBankHourUsed].RuleValue, bhRounding, firstPairsMonthDay.Date, pairDate.Date, conn, passTypesAllDic, schDict);

                                        // add bank hours value from changed day
                                        int bhDayValue = 0;
                                        foreach (IOPairProcessedTO pair in dayPairs)
                                        {
                                            if (pair.IOPairDate.Date < firstMonthDay.Date
                                                || (pair.IOPairDate.Date == firstMonthDay.Date && isPreviousDayPair(pair, passTypesAllDic, dayPairs, sch, intervals)))
                                                continue;

                                            int duration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                                            if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                                                duration++;

                                            if (pair.PassTypeID == rules[Constants.RuleCompanyBankHour].RuleValue)
                                                bhDayValue += duration;

                                            if (pair.PassTypeID == rules[Constants.RuleCompanyBankHourUsed].RuleValue)
                                            {
                                                if (duration % bhRounding != 0)
                                                    duration += bhRounding - (duration % bhRounding);

                                                bhDayValue -= duration;
                                            }
                                        }

                                        bankHoursValueToValidate += bhDayValue;
                                    }
                                }
                            }
                            
                          if (checkLimit && (bankHoursValueToValidate <= 0 || bankHoursValueToValidate < bhCounterDuration))
                                return "notEnoughBankHour";

                            emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value -= bhCounterDuration;
                        }
                    }

                    if ((rules.ContainsKey(Constants.RuleCompanyOvertimePaid) && rules[Constants.RuleCompanyOvertimePaid].RuleValue == newPairs[i].PassTypeID)
                        || (rules.ContainsKey(Constants.RuleCompanyBankHour) && rules[Constants.RuleCompanyBankHour].RuleValue == newPairs[i].PassTypeID))
                    {

                        if (dayPairs == null)
                        {
                            // get all pairs for this day
                            IOPairProcessed iopair;

                            if (conn != null)
                                iopair = new IOPairProcessed(conn);
                            else
                                iopair = new IOPairProcessed();
                            iopair.IOPairProcessedTO.EmployeeID = emplID;
                            iopair.IOPairProcessedTO.IOPairDate = newPairs[i].IOPairDate.Date;

                            dayPairs = iopair.Search();
                        }

                        overtimePaidDayDuration += overtimeValue;
                        bankHourDayDuration += bankHourValue;

                        foreach (IOPairProcessedTO pairTO in dayPairs)
                        {
                            if (!pairTO.IOPairDate.Date.Equals(newPairs[i].IOPairDate.Date))
                                continue;

                            int duration = (int)pairTO.EndTime.Subtract(pairTO.StartTime).TotalMinutes;

                            if (pairTO.EndTime.Hour == 23 && pairTO.EndTime.Minute == 59)
                                duration++;

                            bool forDeleting = false;

                            foreach (IOPairProcessedTO oldPair in oldPairs)
                            {
                                if (oldPair.compare(pairTO))
                                {
                                    forDeleting = true;
                                    break;
                                }
                            }

                            if (rules.ContainsKey(Constants.RuleCompanyOvertimePaid) && rules[Constants.RuleCompanyOvertimePaid].RuleValue == pairTO.PassTypeID && !forDeleting)
                                overtimePaidDayDuration += duration;

                            if (rules.ContainsKey(Constants.RuleCompanyBankHour) && rules[Constants.RuleCompanyBankHour].RuleValue == pairTO.PassTypeID && !forDeleting)
                                bankHourDayDuration += duration;
                        }

                        //if (rules.ContainsKey(Constants.RuleCompanyOvertimePaid) && rules[Constants.RuleCompanyOvertimePaid].RuleValue == newPairs[i].PassTypeID)
                        //{
                        //    // get currently overtime week duration, pairs for current day are taken from dayPairs not from db   
                        IOPairProcessed pairProcessed;

                        if (conn != null)
                            pairProcessed = new IOPairProcessed(conn);
                        else
                            pairProcessed = new IOPairProcessed();

                        List<IOPairProcessedTO> overtimeWeekPairs = pairProcessed.SearchWeekPairs(emplID, newPairs[i].IOPairDate.Date, false, rules[Constants.RuleCompanyOvertimePaid].RuleValue.ToString().Trim(), null);

                        overtimePaidWeekDuration = overtimePaidDayDuration;

                        foreach (IOPairProcessedTO pair in overtimeWeekPairs)
                        {
                            overtimePaidWeekDuration += (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;
                        }
                        //}

                        if (rules.ContainsKey(Constants.RuleCompanyBankHour) && rules[Constants.RuleCompanyBankHour].RuleValue == newPairs[i].PassTypeID)
                        {
                            // get currently bank hours week duration, pairs for current day are taken from dayPairs not from db   
                            IOPairProcessed bhPairProcessed;

                            if (conn != null)
                                bhPairProcessed = new IOPairProcessed(conn);
                            else
                                bhPairProcessed = new IOPairProcessed();

                            List<IOPairProcessedTO> bankHoursWeekPairs = bhPairProcessed.SearchWeekPairs(emplID, newPairs[i].IOPairDate.Date, false, rules[Constants.RuleCompanyBankHour].RuleValue.ToString().Trim(), null);

                            bankHourWeekDuration = bankHourDayDuration;

                            foreach (IOPairProcessedTO pair in bankHoursWeekPairs)
                            {
                                bankHourWeekDuration += (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;
                            }
                        }

                        // if new type is overtime paid check limits and increase counter if change is valid - counter value is in minutes
                        if (rules.ContainsKey(Constants.RuleCompanyOvertimePaid) && rules[Constants.RuleCompanyOvertimePaid].RuleValue == newPairs[i].PassTypeID)
                        {
                            //15.09.2017. Miodrag / Postojao je problem, i za dane vikenda je gledao pravilo za prekovremeno tokom radnih dana
                            if (newPairs[i].IOPairDate.DayOfWeek == DayOfWeek.Saturday || newPairs[i].IOPairDate.DayOfWeek == DayOfWeek.Sunday)
                            {
                                // check week limit, rule limit unit is hour, counter unit is minute
                                if (checkLimit && rules.ContainsKey(Constants.RuleOvertimeWeekLimit) && overtimePaidWeekDuration > (rules[Constants.RuleOvertimeWeekLimit].RuleValue * 60))
                                    return "overtimeWeekLimitReached";

                                
                            }
                            else
                            {
                                // check day limit, rule limit unit is hour, counter unit is minute
                                if (checkLimit && rules.ContainsKey(Constants.RuleOvertimeDayLimit) && overtimePaidDayDuration > (rules[Constants.RuleOvertimeDayLimit].RuleValue * 60))
                                    return "overtimeDayLimitReached";

                                if (dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date))
                                {
                                    // check working day limit, rule limit unit is hour, counter unit is minute
                                    if (checkLimit && isWorkingDay(dayIntervals[newPairs[i].IOPairDate.Date]) && rules.ContainsKey(Constants.RuleOvertimeWSLimit) && overtimePaidDayDuration > (rules[Constants.RuleOvertimeWSLimit].RuleValue * 60))
                                        return "overtimeDayLimitReached";
                                }
                            }
                            //MM
                            // check composite week limit with bank hours
                            if (checkLimit && rules.ContainsKey(Constants.RuleBankHrsWeekLimit) && (bankHourWeekDuration + overtimePaidWeekDuration) > (rules[Constants.RuleBankHrsWeekLimit].RuleValue * 60))
                                return "bankHourWeekLimitReached";

                            // increase counter
                            if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.OvertimeCounter))
                                emplCounters[(int)Constants.EmplCounterTypes.OvertimeCounter].Value += overtimeValue;
                        }
                        else
                        {

                        }
                        // if new type is bank hour check limits and increase counter if change is valid - counter is in minutes
                        if (rules.ContainsKey(Constants.RuleCompanyBankHour) && rules[Constants.RuleCompanyBankHour].RuleValue == newPairs[i].PassTypeID)
                        {
                            // Sanja 29.08.2012 - start checking if bank hours is earning before overtime
                            // first check if there is some overtime hours left for use if rule is set to check
                            if (rules.ContainsKey(Constants.RuleOvertimeBeforeBankHours) && rules[Constants.RuleOvertimeBeforeBankHours].RuleValue == Constants.yesInt)
                            {
                                if (checkLimit)
                                {
                                    bool isWSDay = dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date) && isWorkingDay(dayIntervals[newPairs[i].IOPairDate.Date]);
                                    bool dayLimitNotReached = true;
                                    bool wsDayLimitNotReached = true;
                                    bool weekLimitNotReached = true;

                                    if (rules.ContainsKey(Constants.RuleOvertimeDayLimit))
                                        dayLimitNotReached = overtimePaidDayDuration < (rules[Constants.RuleOvertimeDayLimit].RuleValue * 60);

                                    if (isWSDay && rules.ContainsKey(Constants.RuleOvertimeWSLimit))
                                        wsDayLimitNotReached = overtimePaidDayDuration < (rules[Constants.RuleOvertimeWSLimit].RuleValue * 60);

                                    if (rules.ContainsKey(Constants.RuleOvertimeWeekLimit))
                                        weekLimitNotReached = overtimePaidWeekDuration < (rules[Constants.RuleOvertimeWeekLimit].RuleValue * 60);

                                    if (dayLimitNotReached && wsDayLimitNotReached && weekLimitNotReached)
                                        return "overtimeLeftForUse";
                                }
                            }

                            // Sanja 29.08.2012 - end checking if bank hours is earning before overtime
                            if (checkLimit && rules.ContainsKey(Constants.RuleBankHrsDayLimit) && bankHourDayDuration > (rules[Constants.RuleBankHrsDayLimit].RuleValue * 60))
                                return "bankHourDayLimitReached";

                            if (checkLimit && rules.ContainsKey(Constants.RuleBankHrsWeekLimit) && bankHourWeekDuration + overtimePaidWeekDuration > (rules[Constants.RuleBankHrsWeekLimit].RuleValue * 60))
                                return "bankHourWeekLimitReached";

                            // increase counter
                            if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                                emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value += bankHourValue;
                        }
                    }

                    // if new type is stop working increase counter - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyStopWorking) && rules[Constants.RuleCompanyStopWorking].RuleValue == newPairs[i].PassTypeID)
                    {
                        // increase counter
                        if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                            emplCounters[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value += pairDuration;
                    }

                    // for night shift, for whole day absences, counters and limits, do validation only for first pair
                    if (newPairs[i].StartTime.Hour == 0 && newPairs[i].StartTime.Minute == 0 && !checkEveryThirdShift)
                        continue;

                    // Sanja 17.09.2012. - if changing counters for second interval night shifts for whole day absences, do not check third shifts counters again
                    if (((newPairs[i].StartTime.Hour == 0 && newPairs[i].StartTime.Minute == 0) || (newPairs[i].EndTime.Hour == 23 && newPairs[i].EndTime.Minute == 59)) && !checkThirdShift)
                        continue;

                    // if new type is annual leave check if there is enough leave left
                    if ((rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && rules[Constants.RuleCompanyAnnualLeave].RuleValue == newPairs[i].PassTypeID)
                        || (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue == newPairs[i].PassTypeID))
                    {
                        // check if employee has right to use annual leave
                        if (checkLimit && !emplAsco.DatetimeValue4.Equals(new DateTime()) && emplAsco.DatetimeValue4.Date > periodStart.Date)
                            return "noRightToUseAnnualLeave";

                        // check if there are hours on bank hours or not justified overtime counter to use before using vacation
                        if (checkLimit && ((rules.ContainsKey(Constants.RuleBankHoursBeforeVacation) && rules[Constants.RuleBankHoursBeforeVacation].RuleValue == Constants.yesInt
                            && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                            || (rules.ContainsKey(Constants.RuleOvertimeNJHoursBeforeVacation) && rules[Constants.RuleOvertimeNJHoursBeforeVacation].RuleValue == Constants.yesInt
                            && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.NotJustifiedOvertime))))
                        {
                            // get shift duration
                            int shiftDuration = 0;
                            List<WorkTimeIntervalTO> pairDateIntervals = new List<WorkTimeIntervalTO>();
                            WorkTimeSchemaTO pairDateSch = new WorkTimeSchemaTO();
                            if (dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date))
                                pairDateIntervals = dayIntervals[newPairs[i].IOPairDate.Date];
                            if (daySchemas.ContainsKey(newPairs[i].IOPairDate.Date))
                                pairDateSch = daySchemas[newPairs[i].IOPairDate.Date];

                            WorkTimeIntervalTO pairInterval = getPairInterval(newPairs[i], dayPairs, pairDateSch, pairDateIntervals, passTypesAllDic);

                            shiftDuration = (int)pairInterval.EndTime.TimeOfDay.Subtract(pairInterval.StartTime.TimeOfDay).TotalMinutes;

                            // if pair is from third sfiht start
                            if (pairInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                            {
                                shiftDuration++;

                                // get next day third shift ending duration
                                if (dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date.AddDays(1)))
                                {
                                    foreach (WorkTimeIntervalTO nextDayInterval in dayIntervals[newPairs[i].IOPairDate.Date.AddDays(1)])
                                    {
                                        if (nextDayInterval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                            continue;

                                        shiftDuration += (int)nextDayInterval.EndTime.TimeOfDay.Subtract(nextDayInterval.StartTime.TimeOfDay).TotalMinutes;
                                    }
                                }
                            }
                            // if pair is from third sfiht end
                            else if (pairInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                            {
                                // get previous day third shift ending duration
                                if (dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date.AddDays(-1)))
                                {
                                    foreach (WorkTimeIntervalTO prevDayInterval in dayIntervals[newPairs[i].IOPairDate.Date.AddDays(-1)])
                                    {
                                        if (prevDayInterval.EndTime.TimeOfDay != new TimeSpan(23, 59, 0))
                                            continue;

                                        shiftDuration += (int)prevDayInterval.EndTime.TimeOfDay.Subtract(prevDayInterval.StartTime.TimeOfDay).TotalMinutes;

                                        shiftDuration++;
                                    }
                                }
                            }

                            // get bank hours value to validate                            
                            // if new pair is in previous month, validate against hours earned till month beginning (counter - earned hours from month beginning),
                            // else if current month is not first after bank hours cut off date, validate against counter
                            // else validate against hours earned from month beginning
                            int bankHoursValueToValidate = 0;
                            int overtimeHoursValueToValidate = 0;

                            if ((rules.ContainsKey(Constants.RuleOvertimeNJHoursBeforeVacation) && rules[Constants.RuleOvertimeNJHoursBeforeVacation].RuleValue == Constants.yesInt
                                && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.NotJustifiedOvertime)))
                                overtimeHoursValueToValidate = emplCounters[(int)Constants.EmplCounterTypes.NotJustifiedOvertime].Value;

                            if (rules.ContainsKey(Constants.RuleBankHoursBeforeVacation) && rules[Constants.RuleBankHoursBeforeVacation].RuleValue == Constants.yesInt
                                && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                            {
                                bankHoursValueToValidate = emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value;

                                DateTime bankHourCutOffDate1 = new DateTime();
                                DateTime bankHourCutOffDate2 = new DateTime();
                                if (rules.ContainsKey(Constants.RuleBankHrsCutOffDate1))
                                    bankHourCutOffDate1 = rules[Constants.RuleBankHrsCutOffDate1].RuleDateTime1;
                                if (rules.ContainsKey(Constants.RuleBankHrsCutOffDate2))
                                    bankHourCutOffDate2 = rules[Constants.RuleBankHrsCutOffDate2].RuleDateTime1;

                                DateTime pairDate = newPairs[i].IOPairDate.Date;

                                // for first day of month, check if pair belongs to previous month
                                WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                                if (daySchemas.ContainsKey(newPairs[i].IOPairDate.Date))
                                    sch = daySchemas[newPairs[i].IOPairDate.Date];
                                List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                                if (dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date))
                                    intervals = dayIntervals[newPairs[i].IOPairDate.Date];
                                if (newPairs[i].IOPairDate.Day == 1)
                                {
                                    if (isPreviousDayPair(newPairs[i], passTypesAllDic, dayPairs, sch, intervals))
                                        pairDate = pairDate.AddDays(-1);
                                }

                                DateTime firstPairsMonthDay = new DateTime(pairDate.Year, pairDate.Month, 1, 0, 0, 0);
                                DateTime firstMonthDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);

                                // calculate if pair month is month after cut off date
                                bool isMonthAfterCutOffDate = false;

                                if (!bankHourCutOffDate1.Equals(new DateTime()) && !bankHourCutOffDate2.Equals(new DateTime())
                                    && !bankHourCutOffDate1.Equals(Constants.dateTimeNullValue()) && !bankHourCutOffDate2.Equals(Constants.dateTimeNullValue()))
                                {
                                    isMonthAfterCutOffDate = firstPairsMonthDay.Equals(new DateTime(DateTime.Now.Year, bankHourCutOffDate1.Month, 1, 0, 0, 0).AddMonths(1))
                                    || firstPairsMonthDay.Equals(new DateTime(DateTime.Now.Year - 1, bankHourCutOffDate1.Month, 1, 0, 0, 0).AddMonths(1))
                                    || firstPairsMonthDay.Equals(new DateTime(DateTime.Now.Year, bankHourCutOffDate2.Month, 1, 0, 0, 0).AddMonths(1))
                                    || firstPairsMonthDay.Equals(new DateTime(DateTime.Now.Year - 1, bankHourCutOffDate2.Month, 1, 0, 0, 0).AddMonths(1));
                                }

                                if (pairDate.Date < firstMonthDay.Date || isMonthAfterCutOffDate)
                                {
                                    if (rules.ContainsKey(Constants.RuleCompanyBankHour))
                                    {
                                        // get pairs earned after month beginning
                                        if (pairDate.Date < firstMonthDay.Date)
                                            //bankHoursValueToValidate -= pairProcessed.SearchEarnedBankHours(newPairs[i].EmployeeID, rules[Constants.RuleCompanyBankHour].RuleValue, 
                                            //firstMonthDay.Date);
                                            bankHoursValueToValidate -= SearchHours(newPairs[i].EmployeeID, rules[Constants.RuleCompanyBankHour].RuleValue,
                                                rules[Constants.RuleCompanyBankHourUsed].RuleValue, bhRounding, firstMonthDay.Date, new DateTime(), conn, passTypesAllDic, schDict);
                                        else
                                        {
                                            bankHoursValueToValidate = SearchHours(newPairs[i].EmployeeID, rules[Constants.RuleCompanyBankHour].RuleValue,
                                                rules[Constants.RuleCompanyBankHourUsed].RuleValue, bhRounding, firstPairsMonthDay.Date, pairDate.Date, conn, passTypesAllDic, schDict);

                                            // add bank hours value from changed day
                                            int bhDayValue = 0;
                                            foreach (IOPairProcessedTO pair in dayPairs)
                                            {
                                                if (pair.IOPairDate.Date < firstMonthDay.Date
                                                    || (pair.IOPairDate.Date == firstMonthDay.Date && isPreviousDayPair(pair, passTypesAllDic, dayPairs, sch, intervals)))
                                                    continue;

                                                int duration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                                                if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                                                    duration++;

                                                if (pair.PassTypeID == rules[Constants.RuleCompanyBankHour].RuleValue)
                                                    bhDayValue += duration;

                                                if (pair.PassTypeID == rules[Constants.RuleCompanyBankHourUsed].RuleValue)
                                                {
                                                    if (duration % bhRounding != 0)
                                                        duration += bhRounding - (duration % bhRounding);

                                                    bhDayValue -= duration;
                                                }
                                            }

                                            bankHoursValueToValidate += bhDayValue;
                                        }
                                    }
                                }
                            }

                            if (shiftDuration > 0 && overtimeHoursValueToValidate >= shiftDuration)
                                return "useNotjustifedOvertimeHoursBeforeVacation";

                            if (shiftDuration > 0 && bankHoursValueToValidate >= shiftDuration)
                                return "useBankHoursBeforeVacation";

                            if (shiftDuration > 0 && (overtimeHoursValueToValidate + bankHoursValueToValidate) >= shiftDuration)
                                return "useNotjustifedOvertimeBankHoursBeforeVacation";
                        }

                        int counterValue = 1;
                        if (daySchemas.ContainsKey(newPairs[i].IOPairDate.Date) && is10HourShift(daySchemas[newPairs[i].IOPairDate.Date]))
                        {
                            if (fromWeekAnnualLeavePairs == null && toWeekAnnualLeavePairs == null)
                            {
                                // get week annual leave pairs
                                int annualLeaveDays = 0;
                                IOPairProcessed annualPair;

                                if (conn != null)
                                    annualPair = new IOPairProcessed(conn);
                                else
                                    annualPair = new IOPairProcessed();

                                List<IOPairProcessedTO> annualLeaveWeekPairs = annualPair.SearchWeekPairs(emplID, newPairs[i].IOPairDate, false, getAnnualLeaveTypesString(rules), null);

                                // if day pairs contains both third shifts for current and previous day, third sfift for current day is already excluded
                                // third sfiht for previous day should be excluded from annualLeaveWeekPairs, and current state from day pairs should be added to those pairs
                                IEnumerator<IOPairProcessedTO> annualPairEnumerator = annualLeaveWeekPairs.GetEnumerator();
                                while (annualPairEnumerator.MoveNext())
                                {
                                    if ((annualPairEnumerator.Current.StartTime.Hour == 0 && annualPairEnumerator.Current.StartTime.Minute == 0)
                                        || (annualPairEnumerator.Current.IOPairDate.Date.Equals(newPairs[i].IOPairDate.AddDays(-1))
                                        && annualPairEnumerator.Current.EndTime.Hour == 23 && annualPairEnumerator.Current.EndTime.Minute == 59))
                                    {
                                        annualLeaveWeekPairs.Remove(annualPairEnumerator.Current);
                                        annualPairEnumerator = annualLeaveWeekPairs.GetEnumerator();
                                    }
                                }

                                foreach (IOPairProcessedTO iopair in dayPairs)
                                {
                                    if ((rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && iopair.PassTypeID == rules[Constants.RuleCompanyAnnualLeave].RuleValue)
                                        || (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && iopair.PassTypeID == rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue))
                                    {
                                        // if changing third shift from previous day, add already changed pairs from this day
                                        if (newPairs[i].StartTime.Hour == 0 && newPairs[i].StartTime.Minute == 0)
                                        {
                                            if (iopair.IOPairDate.Date.Equals(newPairs[i].IOPairDate.Date) && !iopair.compareAL(newPairs[i], getAnnualLeaveTypes(rules)))
                                                annualLeaveWeekPairs.Add(iopair);
                                        }

                                        // if changing third shift from current day, add already changed pairs for this or previous day
                                        if (newPairs[i].EndTime.Hour == 23 && newPairs[i].EndTime.Minute == 59)
                                        {
                                            if ((iopair.IOPairDate.Date.Equals(newPairs[i].IOPairDate.Date) && !iopair.compareAL(newPairs[i], getAnnualLeaveTypes(rules)) && !(iopair.StartTime.Hour == 0 && iopair.StartTime.Minute == 0))
                                                || (iopair.IOPairDate.Date.Equals(newPairs[i].IOPairDate.AddDays(-1).Date) && iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59))
                                                annualLeaveWeekPairs.Add(iopair);
                                        }
                                    }
                                }

                                annualLeaveDays = annualLeaveWeekPairs.Count;

                                // if fourts day of week is changed, subtract/add two days
                                if (annualLeaveDays == (Constants.fullWeek10hShift - 1))
                                    counterValue++;
                            }
                            else
                            {
                                List<IOPairProcessedTO> weekAnnualLeavePairs = getWeekPairs(wholeWeekPeriodPairs, newPairs[i].IOPairDate.Date);

                                // massive input - all pairs are of same type, weekPairs contains whole week pairs that will be in db, so it does not metter if counter is changed for first or last pair of week
                                if (isFirstInWeek(periodStart, periodEnd, newPairs[i].IOPairDate.Date, weekAnnualLeavePairs))
                                {
                                    // if full week holiday, subtract/add two days
                                    if (weekAnnualLeavePairs.Count == Constants.fullWeek10hShift)
                                        counterValue++;
                                }
                            }
                        }

                        // get already used collective annual leave from last cut off date
                        DateTime startDate = new DateTime();
                        if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeaveReservation))
                        {
                            startDate = new DateTime(DateTime.Now.Year, rules[Constants.RuleCompanyCollectiveAnnualLeaveReservation].RuleDateTime1.Month, rules[Constants.RuleCompanyCollectiveAnnualLeaveReservation].RuleDateTime1.Day);

                            if (startDate.Date > DateTime.Now)
                                startDate = startDate.AddYears(-1).Date;
                        }

                        int ptID = -1;
                        if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave))
                            ptID = rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue;

                        int numOfDays = SearchCollectiveAnnualLeave(newPairs[i], emplID, ptID, startDate.Date, exceptDates, newCollectivePairs, rules, conn);

                        // get reserved days for collective annual leave
                        int reserved = 0;
                        if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeaveReservation))
                            reserved = rules[Constants.RuleCompanyCollectiveAnnualLeaveReservation].RuleValue;

                        // employees that are excepted from annual leave reservation
                        if (emplAsco.IntegerValue10 == Constants.yesInt)
                            reserved = 0;

                        // collective annual leave can not be assigned for more then reserved days
                        if (checkLimit && rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave)
                            && newPairs[i].PassTypeID == rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue && numOfDays >= reserved)
                            return "noMoreReservedCollectiveAnnualLeave";

                        if (numOfDays <= reserved)
                            reserved -= numOfDays;
                        else
                            reserved = 0;

                        // check if there are annual leave days reserved
                        int alReserved = 0;
                        if (rules.ContainsKey(Constants.RuleCompanyAnnualLeaveReservation))
                            alReserved = rules[Constants.RuleCompanyAnnualLeaveReservation].RuleValue;

                        // employees that are excepted from annual leave reservation
                        if (emplAsco.IntegerValue10 == Constants.yesInt)
                            alReserved = 0;

                        // check if employee can use current year annual leave
                        int alReservedCurrYear = 0;
                        if (checkLimit && !emplAsco.DatetimeValue6.Equals(new DateTime()) && emplAsco.DatetimeValue6.Date > periodStart.Date
                            && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter))
                            alReservedCurrYear = emplCounters[(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value;

                        bool updateCounter = false;                        
                        if (rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && newPairs[i].PassTypeID == rules[Constants.RuleCompanyAnnualLeave].RuleValue)
                            updateCounter = !checkLimit || (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter) && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter)
                                && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter)
                                && (emplCounters[(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value + emplCounters[(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value - Math.Max(reserved + alReserved, alReservedCurrYear)
                                >= emplCounters[(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value + counterValue));
                        else if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && newPairs[i].PassTypeID == rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue)
                            updateCounter = !checkLimit || (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter) && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter)
                                && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter)
                                && (emplCounters[(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value + emplCounters[(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value - Math.Max(reserved, alReservedCurrYear)
                                >= emplCounters[(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value + counterValue));

                        if (updateCounter)
                        {
                            if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))
                                emplCounters[(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value += counterValue;
                            else
                                return "noMoreAnnualLeave";
                        }
                        else
                            return "noMoreAnnualLeave";
                    }

                    // if new type is paid leave type (exist not null pass type limit), increase counter if change is valid - counter is in days
                    if (passTypesAllDic.ContainsKey(newPairs[i].PassTypeID) && (passTypesAllDic[newPairs[i].PassTypeID].LimitCompositeID != -1
                        || passTypesAllDic[newPairs[i].PassTypeID].LimitElementaryID != -1 || passTypesAllDic[newPairs[i].PassTypeID].LimitOccasionID != -1))
                    {
                        // if there is elementary limit
                        if (passTypesAllDic[newPairs[i].PassTypeID].LimitElementaryID != -1 && passTypeLimits.ContainsKey(passTypesAllDic[newPairs[i].PassTypeID].LimitElementaryID))
                        {
                            DateTime from = new DateTime(newPairs[i].IOPairDate.Year, passTypeLimits[passTypesAllDic[newPairs[i].PassTypeID].LimitElementaryID].StartDate.Month,
                                passTypeLimits[passTypesAllDic[newPairs[i].PassTypeID].LimitElementaryID].StartDate.Day, 0, 0, 0);
                            DateTime to = from.AddMonths(passTypeLimits[passTypesAllDic[newPairs[i].PassTypeID].LimitElementaryID].Period);

                            IOPairProcessed pairProcessed;
                            if (conn != null)
                                pairProcessed = new IOPairProcessed(conn);
                            else
                                pairProcessed = new IOPairProcessed();
                            int paidLeaveDays = pairProcessed.SearchPaidLeaveDaysOutsidePeriod(from, to, periodStart, periodEnd, newPairs[i].EmployeeID, newPairs[i].PassTypeID,
                                -1, -1, passTypesAllDic[newPairs[i].PassTypeID].LimitElementaryID);

                            if (paidLeaveElementaryPairsDict != null && paidLeaveElementaryPairsDict.ContainsKey(newPairs[i].PassTypeID))
                                paidLeaveDays += paidLeaveElementaryPairsDict[newPairs[i].PassTypeID];

                            if (checkLimit && paidLeaveDays >= passTypeLimits[passTypesAllDic[newPairs[i].PassTypeID].LimitElementaryID].Value)
                                return "ptElementaryLimitReached";

                            if (paidLeaveElementaryPairsDict != null && newPairs[i].StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                            {
                                if (!paidLeaveElementaryPairsDict.ContainsKey(newPairs[i].PassTypeID))
                                    paidLeaveElementaryPairsDict.Add(newPairs[i].PassTypeID, 0);

                                paidLeaveElementaryPairsDict[newPairs[i].PassTypeID]++;
                            }
                        }

                        // if there is composite limit
                        if (passTypesAllDic[newPairs[i].PassTypeID].LimitCompositeID != -1 && passTypeLimits.ContainsKey(passTypesAllDic[newPairs[i].PassTypeID].LimitCompositeID))
                        {
                            if (checkLimit && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.PaidLeaveCounter)
                                && emplCounters[(int)Constants.EmplCounterTypes.PaidLeaveCounter].Value >= passTypeLimits[passTypesAllDic[newPairs[i].PassTypeID].LimitCompositeID].Value)
                                return "ptCompositeLimitReached";

                            // increase counter
                            if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.PaidLeaveCounter))
                                emplCounters[(int)Constants.EmplCounterTypes.PaidLeaveCounter].Value++;
                        }
                    }
                }

                if (checkCountersNegativeValue && checkLimit)
                {
                    if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.OvertimeCounter) && emplCounters[(int)Constants.EmplCounterTypes.OvertimeCounter].Value < 0)
                        return "overtimeNegative";

                    if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter) && emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value < 0)
                        return "bankHourNegative";

                    if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter) && emplCounters[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value < 0)
                        return "stopWorkingNegative";
                }

                // Sanja 29.08.2012 - start checking if bank hours earning limit reached
                if (checkLimit && rules.ContainsKey(Constants.RuleBankHrsLimit) && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter)
                    && emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value > (rules[Constants.RuleBankHrsLimit].RuleValue * 60))
                    return "bankHourLimitReached";
                // Sanja 29.08.2012 - end checking if bank hours earning limit reached

                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //20.10.2017 Miodrag - nova funkcija za supervizore sa parametrom vise
        public static string validatePairsPassType(int emplID, EmployeeAsco4TO emplAsco, DateTime periodStart, DateTime periodEnd, List<IOPairProcessedTO> newPairs, List<IOPairProcessedTO> oldPairs,
            List<IOPairProcessedTO> dayPairs, ref Dictionary<int, EmployeeCounterValueTO> emplCounters, Dictionary<string, RuleTO> rules, Dictionary<int, PassTypeTO> passTypesAllDic,
            Dictionary<int, PassTypeLimitTO> passTypeLimits, Dictionary<int, WorkTimeSchemaTO> schDict, Dictionary<DateTime, WorkTimeSchemaTO> daySchemas, Dictionary<DateTime, List<WorkTimeIntervalTO>> dayIntervals,
            List<IOPairProcessedTO> fromWeekAnnualLeavePairs, List<IOPairProcessedTO> toWeekAnnualLeavePairs, Dictionary<int, int> paidLeaveElementaryPairsDict,
            List<IOPairProcessedTO> newCollectivePairs, List<DateTime> exceptDates, List<IOPairProcessedTO> prevDayPairs, object conn, bool checkLimit,
            bool checkEveryThirdShift, bool checkThirdShift, bool checkCountersAfterChange, bool supervisor)
        {
            try
            {
                List<IOPairProcessedTO> wholeWeekPeriodPairs = new List<IOPairProcessedTO>();

                if (fromWeekAnnualLeavePairs != null)
                    wholeWeekPeriodPairs.AddRange(fromWeekAnnualLeavePairs);
                foreach (IOPairProcessedTO oldPair in oldPairs)
                {
                    if ((rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && oldPair.PassTypeID == rules[Constants.RuleCompanyAnnualLeave].RuleValue)
                        || (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && oldPair.PassTypeID == rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue))
                        wholeWeekPeriodPairs.Add(oldPair);
                }
                if (toWeekAnnualLeavePairs != null)
                    wholeWeekPeriodPairs.AddRange(toWeekAnnualLeavePairs);

                bool checkCountersNegativeValue = false;
                for (int i = 0; i < oldPairs.Count; i++)
                {
                    int pairDuration = (int)oldPairs[i].EndTime.Subtract(oldPairs[i].StartTime).TotalMinutes;

                    if (pairDuration > 0 && oldPairs[i].EndTime.Hour == 23 && oldPairs[i].EndTime.Minute == 59)
                        pairDuration++;

                    // if old type is overtime paid, decrease counter - counter value is in minutes Tamara 25.10.2019.
                    if (((rules.ContainsKey(Constants.RuleCompanyOvertimePaid) && rules[Constants.RuleCompanyOvertimePaid].RuleValue == oldPairs[i].PassTypeID)
                        || (rules.ContainsKey(Constants.RuleCompanyOvertimePaidSatur) && rules[Constants.RuleCompanyOvertimePaidSatur].RuleValue == oldPairs[i].PassTypeID)
                        || (rules.ContainsKey(Constants.RuleCompanyOvertimePaidSun) && rules[Constants.RuleCompanyOvertimePaidSun].RuleValue == oldPairs[i].PassTypeID))
                        && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.OvertimeCounter))
                    {
                        emplCounters[(int)Constants.EmplCounterTypes.OvertimeCounter].Value -= pairDuration;

                        if (checkLimit && emplCounters[(int)Constants.EmplCounterTypes.OvertimeCounter].Value < 0)
                        {
                            if (!checkCountersAfterChange)
                                return "overtimeNegative";
                            else
                                checkCountersNegativeValue = true;
                        }
                    }

                    int bhCounterDuration = pairDuration;
                    // calculate value for counter considering bank hour rounding rule
                    if (rules.ContainsKey(Constants.RuleBankHoursUsedRounding))
                    {
                        int bhRounding = rules[Constants.RuleBankHoursUsedRounding].RuleValue;

                        if (pairDuration % bhRounding != 0)
                        {
                            bhCounterDuration += bhRounding - pairDuration % bhRounding;
                        }
                    }

                    // if old type is bank hour, decrease counter - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyBankHour) && rules[Constants.RuleCompanyBankHour].RuleValue == oldPairs[i].PassTypeID
                        && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                    {
                        emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value -= pairDuration;

                        if (checkLimit && emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value < 0)
                        {
                            if (!checkCountersAfterChange)
                                return "bankHourNegative";
                            else
                                checkCountersNegativeValue = true;
                        }
                    }

                    // if old type is bank hour used, increase counter - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyBankHourUsed) && rules[Constants.RuleCompanyBankHourUsed].RuleValue == oldPairs[i].PassTypeID
                        && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                        emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value += bhCounterDuration;

                    // if old type is stop working done, increase counter - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyStopWorkingDone) && rules[Constants.RuleCompanyStopWorkingDone].RuleValue == oldPairs[i].PassTypeID
                        && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                        emplCounters[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value += pairDuration;

                    // if old type is stop working, decrease counter - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyStopWorking) && rules[Constants.RuleCompanyStopWorking].RuleValue == oldPairs[i].PassTypeID
                        && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                    {
                        emplCounters[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value -= pairDuration;

                        if (checkLimit && emplCounters[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value < 0)
                        {
                            if (!checkCountersAfterChange)
                                return "stopWorkingNegative";
                            else
                                checkCountersNegativeValue = true;
                        }
                    }

                    int onCounterDuration = pairDuration;
                    // calculate value for counter considering overtime used rounding rule
                    if (rules.ContainsKey(Constants.RuleInitialOvertimeUsedRounding))
                    {
                        int onRounding = rules[Constants.RuleInitialOvertimeUsedRounding].RuleValue;

                        if (pairDuration % onRounding != 0)
                        {
                            onCounterDuration += onRounding - pairDuration % onRounding;
                        }
                    }

                    // if old type is overtime not justified used, increase counter - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyInitialOvertimeUsed) && rules[Constants.RuleCompanyInitialOvertimeUsed].RuleValue == oldPairs[i].PassTypeID
                        && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.NotJustifiedOvertime))
                        emplCounters[(int)Constants.EmplCounterTypes.NotJustifiedOvertime].Value += onCounterDuration;

                    // for night shift, for whole day absences, counters and limits, decrease counters only for first night shift pair
                    if (oldPairs[i].StartTime.Hour == 0 && oldPairs[i].StartTime.Minute == 0 && !checkEveryThirdShift)
                        continue;

                    // Sanja 17.09.2012. - if changing counters for second interval night shifts for whole day absences, do not check third shifts counters again
                    if (((oldPairs[i].StartTime.Hour == 0 && oldPairs[i].StartTime.Minute == 0) || (oldPairs[i].EndTime.Hour == 23 && oldPairs[i].EndTime.Minute == 59)) && !checkThirdShift)
                        continue;

                    // if oldPT is annual leave, decrement annual leave counter
                    if ((rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && rules[Constants.RuleCompanyAnnualLeave].RuleValue == oldPairs[i].PassTypeID)
                        || (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue == oldPairs[i].PassTypeID))
                    {
                        int counterValue = 1;
                        if (daySchemas.ContainsKey(oldPairs[i].IOPairDate.Date) && is10HourShift(daySchemas[oldPairs[i].IOPairDate.Date]))
                        {
                            if (fromWeekAnnualLeavePairs == null && toWeekAnnualLeavePairs == null)
                            {
                                // get week annual leave pairs
                                int annualLeaveDays = 0;
                                IOPairProcessed annualPair;

                                if (conn != null)
                                    annualPair = new IOPairProcessed(conn);
                                else
                                    annualPair = new IOPairProcessed();

                                List<IOPairProcessedTO> annualLeaveWeekPairs = annualPair.SearchWeekPairs(emplID, oldPairs[i].IOPairDate, false, getAnnualLeaveTypesString(rules), null);

                                // if day pairs contains both third shifts for current and previous day, third sfift for current day is already excluded
                                // third sfiht for previous day should be excluded from annualLeaveWeekPairs, and current state from day pairs should be added to those pairs
                                IEnumerator<IOPairProcessedTO> annualPairEnumerator = annualLeaveWeekPairs.GetEnumerator();
                                while (annualPairEnumerator.MoveNext())
                                {
                                    if ((annualPairEnumerator.Current.StartTime.Hour == 0 && annualPairEnumerator.Current.StartTime.Minute == 0)
                                        || (annualPairEnumerator.Current.IOPairDate.Date.Equals(oldPairs[i].IOPairDate.AddDays(-1))
                                        && annualPairEnumerator.Current.EndTime.Hour == 23 && annualPairEnumerator.Current.EndTime.Minute == 59))
                                    {
                                        annualLeaveWeekPairs.Remove(annualPairEnumerator.Current);
                                        annualPairEnumerator = annualLeaveWeekPairs.GetEnumerator();
                                    }
                                }

                                foreach (IOPairProcessedTO iopair in dayPairs)
                                {
                                    if ((rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && iopair.PassTypeID == rules[Constants.RuleCompanyAnnualLeave].RuleValue)
                                        || (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && iopair.PassTypeID == rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue))
                                    {
                                        // if changing third shift from previous day, add already changed pairs from this day
                                        if (oldPairs[i].StartTime.Hour == 0 && oldPairs[i].StartTime.Minute == 0)
                                        {
                                            if (iopair.IOPairDate.Date.Equals(oldPairs[i].IOPairDate.Date) && !iopair.compareAL(oldPairs[i], getAnnualLeaveTypes(rules)))
                                                annualLeaveWeekPairs.Add(iopair);
                                        }

                                        // if changing third shift from current day, add already changed pairs for this or previous day
                                        if (oldPairs[i].EndTime.Hour == 23 && oldPairs[i].EndTime.Minute == 59)
                                        {
                                            if ((iopair.IOPairDate.Date.Equals(oldPairs[i].IOPairDate.Date) && !iopair.compareAL(oldPairs[i], getAnnualLeaveTypes(rules)) && !(iopair.StartTime.Hour == 0 && iopair.StartTime.Minute == 0))
                                                || (iopair.IOPairDate.Date.Equals(oldPairs[i].IOPairDate.AddDays(-1).Date) && iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59))
                                                annualLeaveWeekPairs.Add(iopair);
                                        }
                                    }
                                }

                                annualLeaveDays = annualLeaveWeekPairs.Count;
                                //foreach (IOPairProcessedTO aPair in annualLeaveWeekPairs)
                                //{
                                //    // second night shift belongs to previous day
                                //    if (aPair.StartTime.Hour == 0 && aPair.StartTime.Minute == 0)
                                //        continue;

                                //    annualLeaveDays++;
                                //}

                                // if fourts day of week is changed, subtract/add two days
                                if (annualLeaveDays == (Constants.fullWeek10hShift - 1))
                                    counterValue++;
                            }
                            else
                            {
                                List<IOPairProcessedTO> weekAnnualLeavePairs = getWeekPairs(wholeWeekPeriodPairs, oldPairs[i].IOPairDate.Date);

                                if (isFirstInWeek(periodStart, periodEnd, oldPairs[i].IOPairDate.Date, weekAnnualLeavePairs))
                                {
                                    // if full week of annual leaves is changed, subtract/add two days
                                    if (weekAnnualLeavePairs.Count == Constants.fullWeek10hShift)
                                        counterValue++;
                                }
                            }
                        }

                        if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))
                            emplCounters[(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value -= counterValue;
                    }

                    // if old type is paid leave type (has composite limit), decrease counter - counter is in days
                    if (passTypesAllDic.ContainsKey(oldPairs[i].PassTypeID) && passTypesAllDic[oldPairs[i].PassTypeID].LimitCompositeID != -1
                        && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.PaidLeaveCounter))
                        emplCounters[(int)Constants.EmplCounterTypes.PaidLeaveCounter].Value--;
                }

                wholeWeekPeriodPairs = new List<IOPairProcessedTO>();

                if (fromWeekAnnualLeavePairs != null)
                    wholeWeekPeriodPairs.AddRange(fromWeekAnnualLeavePairs);
                foreach (IOPairProcessedTO newPair in newPairs)
                {
                    if ((rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && newPair.PassTypeID == rules[Constants.RuleCompanyAnnualLeave].RuleValue)
                        || (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && newPair.PassTypeID == rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue))
                        wholeWeekPeriodPairs.Add(newPair);
                }
                if (toWeekAnnualLeavePairs != null)
                    wholeWeekPeriodPairs.AddRange(toWeekAnnualLeavePairs);

                int overtimeValue = 0;
                int bankHourValue = 0;
                int overtimePaidDayDuration = 0;
                int bankHourDayDuration = 0;
                int overtimePaidWeekDuration = 0;
                int bankHourWeekDuration = 0;
                for (int i = 0; i < newPairs.Count; i++)
                {
                    bool isOvertimeCheckGapDurationPair = false;

                    int pairDuration = (int)newPairs[i].EndTime.Subtract(newPairs[i].StartTime).TotalMinutes;

                    if (pairDuration > 0 && newPairs[i].EndTime.Hour == 23 && newPairs[i].EndTime.Minute == 59)
                        pairDuration++;

                    int bhCounterDuration = pairDuration;
                    int bhRounding = 1;
                    // calculate value for counter considering bank hour rounding rule
                    if (rules.ContainsKey(Constants.RuleBankHoursUsedRounding))
                    {
                        bhRounding = rules[Constants.RuleBankHoursUsedRounding].RuleValue;

                        if (pairDuration % bhRounding != 0)
                        {
                            bhCounterDuration += bhRounding - pairDuration % bhRounding;
                        }
                    }

                    // if new type is overtime paid check limits and increase counter if change is valid - counter value is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyOvertimePaid) && (rules[Constants.RuleCompanyOvertimePaid].RuleValue == newPairs[i].PassTypeID || rules[Constants.RuleCompanyOvertimePaidSatur].RuleValue == newPairs[i].PassTypeID || rules[Constants.RuleCompanyOvertimePaidSun].RuleValue == newPairs[i].PassTypeID)) //tamara 22.10.2019.
                    {
                        isOvertimeCheckGapDurationPair = true;

                        // calculate overtime day duration
                        overtimeValue = pairDuration;
                    }

                    // if new type is bank hour check limits and increase counter if change is valid - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyBankHour) && rules[Constants.RuleCompanyBankHour].RuleValue == newPairs[i].PassTypeID)
                    {
                        isOvertimeCheckGapDurationPair = true;

                        // calculate bank hour day duration
                        bankHourValue = pairDuration;
                    }

                    // if new type is stop working done decrease counter if there are enough minutes - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyStopWorkingDone) && rules[Constants.RuleCompanyStopWorkingDone].RuleValue == newPairs[i].PassTypeID)
                    {
                        isOvertimeCheckGapDurationPair = true;

                        // decrease counter
                        if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                        {
                            // 19.06.2013. Sanja
                            // if new pair is in previous month, validate against hours earned till month beginning (counter - earned hours from month beginning),
                            int swValueToValidate = emplCounters[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value;

                            DateTime pairDate = newPairs[i].IOPairDate.Date;

                            // for first day of month, check if pair belongs to previous month
                            WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                            if (daySchemas.ContainsKey(newPairs[i].IOPairDate.Date))
                                sch = daySchemas[newPairs[i].IOPairDate.Date];
                            List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                            if (dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date))
                                intervals = dayIntervals[newPairs[i].IOPairDate.Date];
                            if (newPairs[i].IOPairDate.Day == 1)
                            {
                                if (isPreviousDayPair(newPairs[i], passTypesAllDic, dayPairs, sch, intervals))
                                    pairDate = pairDate.AddDays(-1);
                            }

                            DateTime firstMonthDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);

                            if (pairDate.Date < firstMonthDay.Date)
                            {
                                if (rules.ContainsKey(Constants.RuleCompanyStopWorking))
                                {
                                    // get pairs earned after month beginning
                                    if (pairDate.Date < firstMonthDay.Date)
                                        //bankHoursValueToValidate -= pairProcessed.SearchEarnedBankHours(newPairs[i].EmployeeID, rules[Constants.RuleCompanyBankHour].RuleValue, 
                                        //firstMonthDay.Date);
                                        swValueToValidate -= SearchHours(newPairs[i].EmployeeID, rules[Constants.RuleCompanyStopWorking].RuleValue,
                                            rules[Constants.RuleCompanyStopWorkingDone].RuleValue, 1, firstMonthDay.Date, new DateTime(), conn, passTypesAllDic, schDict);
                                }
                            }

                            if (checkLimit && swValueToValidate < pairDuration)
                                return "notEnoughStopWorkingHours";

                            emplCounters[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value -= pairDuration;
                        }
                    }

                    // check gap duration between overtime pairs and regular pairs. There must be at least 12 hours gap between regular hours and first pair, or last pair and regular hours
                    // check is performed if labor law rule is set
                    if (checkLimit && isOvertimeCheckGapDurationPair && rules.ContainsKey(Constants.RuleLaborLaw) && rules[Constants.RuleLaborLaw].RuleValue == Constants.yesInt)
                    {
                        IOPairProcessed pairProcessed;
                        if (conn != null)
                            pairProcessed = new IOPairProcessed(conn);
                        else
                            pairProcessed = new IOPairProcessed();

                        int minimalGap = Constants.FiatShiftMinimalGap * 60; // 12 hours, gap is in minutes

                        // list of noncounting types
                        List<int> nonCountingTypes = new List<int>();
                        nonCountingTypes.Add(Constants.overtimeUnjustified);
                        nonCountingTypes.Add(Constants.ptEmptyInterval);
                        if (rules.ContainsKey(Constants.RuleCompanyInitialNightOvertime))
                            nonCountingTypes.Add(rules[Constants.RuleCompanyInitialNightOvertime].RuleValue);
                        if (rules.ContainsKey(Constants.RuleCompanyOvertimeRejected))
                            nonCountingTypes.Add(rules[Constants.RuleCompanyOvertimeRejected].RuleValue);

                        // get gap in currrent day after pair end
                        DateTime checkStart = newPairs[i].EndTime;
                        int gap = 0;
                        bool regularFound = false;
                        for (int index = 0; index < dayPairs.Count; index++)
                        {
                            // skip pairs from other day
                            if (!dayPairs[index].IOPairDate.Date.Equals(newPairs[i].IOPairDate.Date))
                                continue;

                            // skip pairs before checking pair, and checking pair
                            if (dayPairs[index].StartTime <= newPairs[i].StartTime)
                                continue;

                            // skip pairs of noncounting types
                            if (nonCountingTypes.Contains(dayPairs[index].PassTypeID))
                                continue;

                            gap = (int)dayPairs[index].StartTime.Subtract(checkStart).TotalMinutes;

                            if (passTypesAllDic.ContainsKey(dayPairs[index].PassTypeID) && passTypesAllDic[dayPairs[index].PassTypeID].IsPass != Constants.overtimePassType)
                            {
                                regularFound = true;
                                break;
                            }

                            if (gap >= minimalGap)
                                break;

                            checkStart = dayPairs[index].EndTime;
                        }

                        // if gap is less then minimal
                        if (gap < minimalGap)
                        {
                            // if regular pair is not found in current day and gap is less then minimal try checking in next day
                            bool checkStartGap = false;
                            if (!regularFound)
                            {
                                // get gap until next day midnight
                                if (checkStart.TimeOfDay != new TimeSpan(23, 59, 0))
                                    gap = (int)newPairs[i].IOPairDate.Date.AddDays(1).Subtract(checkStart).TotalMinutes;
                                else
                                    gap = 0;

                                if (gap < minimalGap)
                                {
                                    // get next day pairs
                                    List<DateTime> nextDateList = new List<DateTime>();
                                    nextDateList.Add(newPairs[i].IOPairDate.Date.AddDays(1));
                                    List<IOPairProcessedTO> nextDayPairs = pairProcessed.SearchAllPairsForEmpl(newPairs[i].EmployeeID.ToString().Trim(), nextDateList, "");

                                    checkStart = newPairs[i].IOPairDate.Date.AddDays(1);
                                    for (int index = 0; index < nextDayPairs.Count; index++)
                                    {
                                        // skip pairs of noncounting types
                                        if (nonCountingTypes.Contains(nextDayPairs[index].PassTypeID))
                                            continue;

                                        gap += (int)nextDayPairs[index].StartTime.Subtract(checkStart).TotalMinutes;

                                        if (passTypesAllDic.ContainsKey(nextDayPairs[index].PassTypeID) && passTypesAllDic[nextDayPairs[index].PassTypeID].IsPass != Constants.overtimePassType)
                                        {
                                            regularFound = true;
                                            break;
                                        }

                                        if (gap >= minimalGap)
                                            break;

                                        checkStart = nextDayPairs[index].EndTime;
                                        gap = 0;
                                    }

                                    if (gap < minimalGap)
                                    {
                                        if (!regularFound)
                                        {
                                            // get gap until next day midnight
                                            if (checkStart.TimeOfDay != new TimeSpan(23, 59, 0))
                                                gap = (int)newPairs[i].IOPairDate.Date.AddDays(2).Subtract(checkStart).TotalMinutes;
                                            else
                                                gap = 0;

                                            if (gap < minimalGap)
                                                checkStartGap = true;
                                        }
                                        else
                                            checkStartGap = true;
                                    }
                                }
                            }
                            else
                                checkStartGap = true;

                            if (checkStartGap)
                            {
                                // get gap in currrent day before pair start
                                DateTime checkEnd = newPairs[i].StartTime;
                                gap = 0;
                                regularFound = false;
                                for (int index = dayPairs.Count - 1; index >= 0; index--)
                                {
                                    // skip pairs from other day
                                    if (!dayPairs[index].IOPairDate.Date.Equals(newPairs[i].IOPairDate.Date))
                                        continue;

                                    // skip pairs after checking pair, and checking pair
                                    if (dayPairs[index].StartTime >= newPairs[i].StartTime)
                                        continue;

                                    // skip pairs of noncounting types
                                    if (nonCountingTypes.Contains(dayPairs[index].PassTypeID))
                                        continue;

                                    gap = (int)checkEnd.Subtract(dayPairs[index].EndTime).TotalMinutes;

                                    if (passTypesAllDic.ContainsKey(dayPairs[index].PassTypeID) && passTypesAllDic[dayPairs[index].PassTypeID].IsPass != Constants.overtimePassType)
                                    {
                                        regularFound = true;
                                        break;
                                    }

                                    if (gap >= minimalGap)
                                        break;

                                    checkEnd = dayPairs[index].StartTime;
                                }

                                if (gap < minimalGap)
                                {
                                    // if regular pair is not found in current day and gap is less then minimal try checking in previous day                                    
                                    if (!regularFound)
                                    {
                                        // get gap until current day midnight                                        
                                        gap = (int)checkEnd.Subtract(newPairs[i].IOPairDate.Date).TotalMinutes;

                                        if (gap < minimalGap)
                                        {
                                            if (prevDayPairs == null)
                                            {
                                                // get previous day pairs
                                                List<DateTime> prevDateList = new List<DateTime>();
                                                prevDateList.Add(newPairs[i].IOPairDate.Date.AddDays(-1));
                                                prevDayPairs = pairProcessed.SearchAllPairsForEmpl(newPairs[i].EmployeeID.ToString().Trim(), prevDateList, "");
                                            }

                                            checkEnd = newPairs[i].IOPairDate.Date;
                                            for (int index = prevDayPairs.Count - 1; index >= 0; index--)
                                            {
                                                // skip pairs of noncounting types
                                                if (nonCountingTypes.Contains(prevDayPairs[index].PassTypeID))
                                                    continue;

                                                gap += (int)checkEnd.Subtract(prevDayPairs[index].EndTime).TotalMinutes;

                                                if (passTypesAllDic.ContainsKey(prevDayPairs[index].PassTypeID) && passTypesAllDic[prevDayPairs[index].PassTypeID].IsPass != Constants.overtimePassType)
                                                {
                                                    regularFound = true;
                                                    break;
                                                }

                                                if (gap >= minimalGap)
                                                    break;

                                                checkEnd = prevDayPairs[index].StartTime;
                                                gap = 0;
                                            }

                                            if (gap < minimalGap)
                                            {
                                                if (!regularFound)
                                                {
                                                    // get gap until previous day midnight                                                   
                                                    gap = (int)checkEnd.Subtract(newPairs[i].IOPairDate.Date.AddDays(-1)).TotalMinutes;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (gap < minimalGap)
                            return "notEnoughFreeHoursBetweenWorkingHours";
                    }

                    // if new type is overtime not justified used decrease counter if there are enough minutes - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyInitialOvertimeUsed) && rules[Constants.RuleCompanyInitialOvertimeUsed].RuleValue == newPairs[i].PassTypeID)
                    {
                        // decrease counter
                        if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.NotJustifiedOvertime))
                        {
                            int onCounterDuration = pairDuration;
                            int onRounding = 1;
                            // calculate value for counter considering bank hour rounding rule
                            if (rules.ContainsKey(Constants.RuleInitialOvertimeUsedRounding))
                            {
                                onRounding = rules[Constants.RuleInitialOvertimeUsedRounding].RuleValue;

                                if (pairDuration % onRounding != 0)
                                {
                                    onCounterDuration += onRounding - pairDuration % onRounding;
                                }
                            }

                            if (checkLimit && emplCounters[(int)Constants.EmplCounterTypes.NotJustifiedOvertime].Value < onCounterDuration)
                                return "notEnoughOvertimeNotJustifiedHours";

                            emplCounters[(int)Constants.EmplCounterTypes.NotJustifiedOvertime].Value -= onCounterDuration;
                        }
                    }

                    // if new type is bank hour used decrease counter if there are enough minutes - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyBankHourUsed) && rules[Constants.RuleCompanyBankHourUsed].RuleValue == newPairs[i].PassTypeID)
                    {
                        if (checkLimit && rules.ContainsKey(Constants.RuleVacationBeforeBankHours)
                            && rules[Constants.RuleVacationBeforeBankHours].RuleValue == Constants.yesInt)
                        {
                            // check if pair is from whole day absence, if is, check if there is annual leave to use first                            
                            int bhUsedDayDuration = (int)(newPairs[i].EndTime.TimeOfDay - newPairs[i].StartTime.TimeOfDay).TotalMinutes;
                            if (newPairs[i].EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                bhUsedDayDuration++;

                            // get pair belonging date
                            DateTime pairDate = newPairs[i].IOPairDate.Date;
                            WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                            if (daySchemas.ContainsKey(newPairs[i].IOPairDate.Date))
                                sch = daySchemas[newPairs[i].IOPairDate.Date];
                            List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                            if (dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date))
                                intervals = dayIntervals[newPairs[i].IOPairDate.Date];

                            if (isPreviousDayPair(newPairs[i], passTypesAllDic, dayPairs, sch, intervals))
                                pairDate = pairDate.AddDays(-1);

                            IOPairProcessed bhUsedPair;
                            if (conn != null)
                                bhUsedPair = new IOPairProcessed(conn);
                            else
                                bhUsedPair = new IOPairProcessed();
                            bhUsedPair.IOPairProcessedTO.EmployeeID = newPairs[i].EmployeeID;
                            Dictionary<DateTime, List<IOPairProcessedTO>> pairsByDay = new Dictionary<DateTime, List<IOPairProcessedTO>>();
                            if (pairDate.Date != newPairs[i].IOPairDate.Date)
                            {
                                bhUsedPair.IOPairProcessedTO.IOPairDate = pairDate.Date;
                                if (!pairsByDay.ContainsKey(pairDate.Date))
                                    pairsByDay.Add(pairDate.Date, bhUsedPair.Search());
                            }

                            List<IOPairProcessedTO> pairsForDay = new List<IOPairProcessedTO>();
                            if (dayPairs == null)
                            {
                                bhUsedPair.IOPairProcessedTO.IOPairDate = newPairs[i].IOPairDate.Date;
                                pairsForDay = bhUsedPair.Search();
                            }
                            else
                            {
                                foreach (IOPairProcessedTO dayPair in dayPairs)
                                {
                                    if (dayPair.IOPairDate.Date == newPairs[i].IOPairDate.Date)
                                        pairsForDay.Add(dayPair);
                                }
                            }

                            if (!pairsByDay.ContainsKey(newPairs[i].IOPairDate.Date))
                                pairsByDay.Add(newPairs[i].IOPairDate.Date, pairsForDay);

                            if (pairDate.Date == newPairs[i].IOPairDate.Date)
                            {
                                bhUsedPair.IOPairProcessedTO.IOPairDate = newPairs[i].IOPairDate.Date.AddDays(1);
                                if (!pairsByDay.ContainsKey(newPairs[i].IOPairDate.Date.AddDays(1)))
                                    pairsByDay.Add(newPairs[i].IOPairDate.Date.AddDays(1), bhUsedPair.Search());
                            }

                            // add bank hours used value from changed day                           
                            foreach (DateTime date in pairsByDay.Keys)
                            {
                                foreach (IOPairProcessedTO pair in pairsByDay[date])
                                {
                                    if (pair.PassTypeID != rules[Constants.RuleCompanyBankHourUsed].RuleValue)
                                        continue;

                                    // get pair belonging date
                                    DateTime dayPairDate = pair.IOPairDate.Date;
                                    List<WorkTimeIntervalTO> dayPairIntervals = new List<WorkTimeIntervalTO>();
                                    if (dayIntervals.ContainsKey(pair.IOPairDate.Date))
                                        dayPairIntervals = dayIntervals[pair.IOPairDate.Date];
                                    WorkTimeSchemaTO dayPairSch = new WorkTimeSchemaTO();
                                    if (daySchemas.ContainsKey(pair.IOPairDate.Date))
                                        dayPairSch = daySchemas[pair.IOPairDate.Date];
                                    else if (dayPairIntervals.Count > 0 && schDict.ContainsKey(dayPairIntervals[0].TimeSchemaID))
                                        dayPairSch = schDict[dayPairIntervals[0].TimeSchemaID];

                                    if (isPreviousDayPair(pair, passTypesAllDic, pairsByDay[date], dayPairSch, dayPairIntervals))
                                        dayPairDate = dayPairDate.AddDays(-1);

                                    if (pairDate.Date != dayPairDate.Date)
                                        continue;

                                    int duration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                                    if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                                        duration++;

                                    if (pair.PassTypeID == rules[Constants.RuleCompanyBankHourUsed].RuleValue)
                                        bhUsedDayDuration += duration;
                                }
                            }

                            bool isALCandidate = (bhUsedDayDuration == Constants.dayDurationStandardShift * 60);

                            if (isALCandidate && (emplAsco.DatetimeValue4.Equals(new DateTime()) || emplAsco.DatetimeValue4.Date <= periodStart.Date))
                            {
                                int counterValue = 1;

                                // get already used collective annual leave from last cut off date
                                DateTime startDate = new DateTime();
                                if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeaveReservation))
                                {
                                    startDate = new DateTime(DateTime.Now.Year, rules[Constants.RuleCompanyCollectiveAnnualLeaveReservation].RuleDateTime1.Month, rules[Constants.RuleCompanyCollectiveAnnualLeaveReservation].RuleDateTime1.Day);

                                    if (startDate.Date > DateTime.Now)
                                        startDate = startDate.AddYears(-1).Date;
                                }

                                int ptID = -1;
                                if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave))
                                    ptID = rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue;

                                int numOfDays = SearchCollectiveAnnualLeave(newPairs[i], emplID, ptID, startDate.Date, exceptDates, newCollectivePairs, rules, conn);

                                // get reserved days for collective annual leave
                                int reserved = 0;
                                if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeaveReservation))
                                    reserved = rules[Constants.RuleCompanyCollectiveAnnualLeaveReservation].RuleValue;

                                // employees that are excepted from annual leave reservation
                                if (emplAsco.IntegerValue10 == Constants.yesInt)
                                    reserved = 0;

                                if (numOfDays <= reserved)
                                    reserved -= numOfDays;
                                else
                                    reserved = 0;

                                // check if there are annual leave days reserved
                                int alReserved = 0;
                                if (rules.ContainsKey(Constants.RuleCompanyAnnualLeaveReservation))
                                    alReserved = rules[Constants.RuleCompanyAnnualLeaveReservation].RuleValue;

                                // employees that are excepted from annual leave reservation
                                if (emplAsco.IntegerValue10 == Constants.yesInt)
                                    alReserved = 0;

                                // check if employee can use current year annual leave
                                int alReservedCurrYear = 0;
                                if (checkLimit && !emplAsco.DatetimeValue6.Equals(new DateTime()) && emplAsco.DatetimeValue6.Date > periodStart.Date
                                    && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter))
                                    alReservedCurrYear = emplCounters[(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value;

                                bool updateCounter = emplCounters.ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter) && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter)
                                        && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter)
                                        && (emplCounters[(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value + emplCounters[(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value - Math.Max(reserved + alReserved, alReservedCurrYear)
                                        >= emplCounters[(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value + counterValue);

                                if (updateCounter)
                                {
                                    return "useAnnualLeaveBeforeBankHours";
                                }
                            }
                        }

                        // decrease counter
                        if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                        {
                            // if new pair is in previous month, validate against hours earned till month beginning (counter - earned hours from month beginning),
                            // else if current month is not first after bank hours cut off date, validate against counter
                            // else validate against hours earned from month beginning
                            int bankHoursValueToValidate = emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value;

                            DateTime bankHourCutOffDate1 = new DateTime();
                            DateTime bankHourCutOffDate2 = new DateTime();
                            if (rules.ContainsKey(Constants.RuleBankHrsCutOffDate1))
                                bankHourCutOffDate1 = rules[Constants.RuleBankHrsCutOffDate1].RuleDateTime1;
                            if (rules.ContainsKey(Constants.RuleBankHrsCutOffDate2))
                                bankHourCutOffDate2 = rules[Constants.RuleBankHrsCutOffDate2].RuleDateTime1;

                            DateTime pairDate = newPairs[i].IOPairDate.Date;

                            // for first day of month, check if pair belongs to previous month
                            WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                            if (daySchemas.ContainsKey(newPairs[i].IOPairDate.Date))
                                sch = daySchemas[newPairs[i].IOPairDate.Date];
                            List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                            if (dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date))
                                intervals = dayIntervals[newPairs[i].IOPairDate.Date];
                            if (newPairs[i].IOPairDate.Day == 1)
                            {
                                if (isPreviousDayPair(newPairs[i], passTypesAllDic, dayPairs, sch, intervals))
                                    pairDate = pairDate.AddDays(-1);
                            }

                            DateTime firstPairsMonthDay = new DateTime(pairDate.Year, pairDate.Month, 1, 0, 0, 0);
                            DateTime firstMonthDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);

                            // calculate if pair month is month after cut off date
                            bool isMonthAfterCutOffDate = false;

                            if (!bankHourCutOffDate1.Equals(new DateTime()) && !bankHourCutOffDate2.Equals(new DateTime())
                                && !bankHourCutOffDate1.Equals(Constants.dateTimeNullValue()) && !bankHourCutOffDate2.Equals(Constants.dateTimeNullValue()))
                            {
                                isMonthAfterCutOffDate = firstPairsMonthDay.Equals(new DateTime(DateTime.Now.Year, bankHourCutOffDate1.Month, 1, 0, 0, 0).AddMonths(1))
                                || firstPairsMonthDay.Equals(new DateTime(DateTime.Now.Year - 1, bankHourCutOffDate1.Month, 1, 0, 0, 0).AddMonths(1))
                                || firstPairsMonthDay.Equals(new DateTime(DateTime.Now.Year, bankHourCutOffDate2.Month, 1, 0, 0, 0).AddMonths(1))
                                || firstPairsMonthDay.Equals(new DateTime(DateTime.Now.Year - 1, bankHourCutOffDate2.Month, 1, 0, 0, 0).AddMonths(1));
                            }

                            if (pairDate.Date < firstMonthDay.Date || isMonthAfterCutOffDate)
                            {
                                if (rules.ContainsKey(Constants.RuleCompanyBankHour))
                                {
                                    // get pairs earned after month beginning
                                    if (pairDate.Date < firstMonthDay.Date)
                                        //bankHoursValueToValidate -= pairProcessed.SearchEarnedBankHours(newPairs[i].EmployeeID, rules[Constants.RuleCompanyBankHour].RuleValue, 
                                        //firstMonthDay.Date);
                                        bankHoursValueToValidate -= SearchHours(newPairs[i].EmployeeID, rules[Constants.RuleCompanyBankHour].RuleValue,
                                            rules[Constants.RuleCompanyBankHourUsed].RuleValue, bhRounding, firstMonthDay.Date, new DateTime(), conn, passTypesAllDic, schDict);
                                    else
                                    {
                                        bankHoursValueToValidate = SearchHours(newPairs[i].EmployeeID, rules[Constants.RuleCompanyBankHour].RuleValue,
                                            rules[Constants.RuleCompanyBankHourUsed].RuleValue, bhRounding, firstPairsMonthDay.Date, pairDate.Date, conn, passTypesAllDic, schDict);

                                        // add bank hours value from changed day
                                        int bhDayValue = 0;
                                        foreach (IOPairProcessedTO pair in dayPairs)
                                        {
                                            if (pair.IOPairDate.Date < firstMonthDay.Date
                                                || (pair.IOPairDate.Date == firstMonthDay.Date && isPreviousDayPair(pair, passTypesAllDic, dayPairs, sch, intervals)))
                                                continue;

                                            int duration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                                            if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                                                duration++;

                                            if (pair.PassTypeID == rules[Constants.RuleCompanyBankHour].RuleValue)
                                                bhDayValue += duration;

                                            if (pair.PassTypeID == rules[Constants.RuleCompanyBankHourUsed].RuleValue)
                                            {
                                                if (duration % bhRounding != 0)
                                                    duration += bhRounding - (duration % bhRounding);

                                                bhDayValue -= duration;
                                            }
                                        }

                                        bankHoursValueToValidate += bhDayValue;
                                    }
                                }
                            }
                            //20.10.2017 Miodrag - Dodao i supervisor kao uslov u if-u
                            if (checkLimit && (bankHoursValueToValidate <= 0 || bankHoursValueToValidate < bhCounterDuration) && !supervisor)
                              //mm
                                return "notEnoughBankHour";

                            emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value -= bhCounterDuration;
                        }
                    }

                    if ((rules.ContainsKey(Constants.RuleCompanyOvertimePaid) && (rules[Constants.RuleCompanyOvertimePaid].RuleValue == newPairs[i].PassTypeID || rules[Constants.RuleCompanyOvertimePaidSatur].RuleValue == newPairs[i].PassTypeID) || rules[Constants.RuleCompanyOvertimePaidSun].RuleValue == newPairs[i].PassTypeID)
                        || (rules.ContainsKey(Constants.RuleCompanyBankHour) && rules[Constants.RuleCompanyBankHour].RuleValue == newPairs[i].PassTypeID)) //tamara 22.10.2019.
                    {

                        if (dayPairs == null)
                        {
                            // get all pairs for this day
                            IOPairProcessed iopair;

                            if (conn != null)
                                iopair = new IOPairProcessed(conn);
                            else
                                iopair = new IOPairProcessed();
                            iopair.IOPairProcessedTO.EmployeeID = emplID;
                            iopair.IOPairProcessedTO.IOPairDate = newPairs[i].IOPairDate.Date;

                            dayPairs = iopair.Search();
                        }

                        overtimePaidDayDuration += overtimeValue;
                        bankHourDayDuration += bankHourValue;

                        foreach (IOPairProcessedTO pairTO in dayPairs)
                        {
                            if (!pairTO.IOPairDate.Date.Equals(newPairs[i].IOPairDate.Date))
                                continue;

                            int duration = (int)pairTO.EndTime.Subtract(pairTO.StartTime).TotalMinutes;

                            if (pairTO.EndTime.Hour == 23 && pairTO.EndTime.Minute == 59)
                                duration++;

                            bool forDeleting = false;

                            foreach (IOPairProcessedTO oldPair in oldPairs)
                            {
                                if (oldPair.compare(pairTO))
                                {
                                    forDeleting = true;
                                    break;
                                }
                            }

                            //Tamara 24.10.2019. SKLONI
                            if ((rules.ContainsKey(Constants.RuleCompanyOvertimePaid) && rules[Constants.RuleCompanyOvertimePaid].RuleValue == pairTO.PassTypeID && !forDeleting)
                                || ((rules.ContainsKey(Constants.RuleCompanyOvertimePaidSatur) && rules[Constants.RuleCompanyOvertimePaidSatur].RuleValue == pairTO.PassTypeID && !forDeleting)) ||
                                ((rules.ContainsKey(Constants.RuleCompanyOvertimePaidSun) && rules[Constants.RuleCompanyOvertimePaidSun].RuleValue == pairTO.PassTypeID && !forDeleting)))
                                overtimePaidDayDuration += duration;

                            if (rules.ContainsKey(Constants.RuleCompanyBankHour) && rules[Constants.RuleCompanyBankHour].RuleValue == pairTO.PassTypeID && !forDeleting)
                                bankHourDayDuration += duration;
                        }

                        //if (rules.ContainsKey(Constants.RuleCompanyOvertimePaid) && rules[Constants.RuleCompanyOvertimePaid].RuleValue == newPairs[i].PassTypeID)
                        //{
                        //    // get currently overtime week duration, pairs for current day are taken from dayPairs not from db   
                        IOPairProcessed pairProcessed;

                        if (conn != null)
                            pairProcessed = new IOPairProcessed(conn);
                        else
                            pairProcessed = new IOPairProcessed();

                        //Tamara 25.10.2019. dodala da uzme u obzir i parove sa id-jem za prekovremeni rad subotom i za prekovremeni rad nedeljom
                        List<IOPairProcessedTO> overtimeWeekPairs = pairProcessed.SearchWeekPairs(emplID, newPairs[i].IOPairDate.Date, false, rules[Constants.RuleCompanyOvertimePaid].RuleValue.ToString().Trim() + "," +
                        rules[Constants.RuleCompanyOvertimePaidSatur].RuleValue.ToString().Trim() + "," + rules[Constants.RuleCompanyOvertimePaidSun].RuleValue.ToString().Trim(), null);

                        overtimePaidWeekDuration = overtimePaidDayDuration;

                        foreach (IOPairProcessedTO pair in overtimeWeekPairs)
                        {
                            overtimePaidWeekDuration += (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;
                        }
                        //}

                        if (rules.ContainsKey(Constants.RuleCompanyBankHour) && rules[Constants.RuleCompanyBankHour].RuleValue == newPairs[i].PassTypeID)
                        {
                            // get currently bank hours week duration, pairs for current day are taken from dayPairs not from db   
                            IOPairProcessed bhPairProcessed;

                            if (conn != null)
                                bhPairProcessed = new IOPairProcessed(conn);
                            else
                                bhPairProcessed = new IOPairProcessed();

                            List<IOPairProcessedTO> bankHoursWeekPairs = bhPairProcessed.SearchWeekPairs(emplID, newPairs[i].IOPairDate.Date, false, rules[Constants.RuleCompanyBankHour].RuleValue.ToString().Trim(), null);

                            bankHourWeekDuration = bankHourDayDuration;

                            foreach (IOPairProcessedTO pair in bankHoursWeekPairs)
                            {
                                bankHourWeekDuration += (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;
                            }
                        }

                        // if new type is overtime paid check limits and increase counter if change is valid - counter value is in minutes
                        //Tamara 24.10.2019.
                        if ((rules.ContainsKey(Constants.RuleCompanyOvertimePaid) && rules[Constants.RuleCompanyOvertimePaid].RuleValue == newPairs[i].PassTypeID) || (rules.ContainsKey(Constants.RuleCompanyOvertimePaidSatur) && rules[Constants.RuleCompanyOvertimePaidSatur].RuleValue == newPairs[i].PassTypeID) || (rules.ContainsKey(Constants.RuleCompanyOvertimePaidSun) && rules[Constants.RuleCompanyOvertimePaidSun].RuleValue == newPairs[i].PassTypeID))
                        {
                            //15.09.2017. Miodrag / Postojao je problem, i za dane vikenda je gledao pravilo za prekovremeno tokom radnih dana
                            // check week limit, rule limit unit is hour, counter unit is minute
                            if (checkLimit && rules.ContainsKey(Constants.RuleOvertimeWeekLimit) && overtimePaidWeekDuration > (rules[Constants.RuleOvertimeWeekLimit].RuleValue * 60))
                                return "overtimeWeekLimitReached";
                            if (newPairs[i].IOPairDate.DayOfWeek == DayOfWeek.Saturday || newPairs[i].IOPairDate.DayOfWeek == DayOfWeek.Sunday)
                            {                                
                                //Tamara 24.10.2019.
                                // check day limit, rule limit unit is hour, counter unit is minute
                                if (checkLimit && rules.ContainsKey(Constants.RuleOvertimeDayLimitWeekend) && overtimePaidDayDuration > (rules[Constants.RuleOvertimeDayLimitWeekend].RuleValue * 60))
                                    return "overtimeDayLimitReached";


                            }
                            else
                            {
                                // check day limit, rule limit unit is hour, counter unit is minute
                                if (checkLimit && rules.ContainsKey(Constants.RuleOvertimeDayLimit) && overtimePaidDayDuration > (rules[Constants.RuleOvertimeDayLimit].RuleValue * 60))
                                    return "overtimeDayLimitReached";

                                if (dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date))
                                {
                                    // check working day limit, rule limit unit is hour, counter unit is minute
                                    if (checkLimit && isWorkingDay(dayIntervals[newPairs[i].IOPairDate.Date]) && rules.ContainsKey(Constants.RuleOvertimeWSLimit) && overtimePaidDayDuration > (rules[Constants.RuleOvertimeWSLimit].RuleValue * 60))
                                        return "overtimeDayLimitReached";
                                }
                            }
                            //MM
                            // check composite week limit with bank hours
                            if (checkLimit && rules.ContainsKey(Constants.RuleBankHrsWeekLimit) && (bankHourWeekDuration + overtimePaidWeekDuration) > (rules[Constants.RuleBankHrsWeekLimit].RuleValue * 60))
                                return "bankHourWeekLimitReached";

                            // increase counter
                            if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.OvertimeCounter))
                                emplCounters[(int)Constants.EmplCounterTypes.OvertimeCounter].Value += overtimeValue;
                        }
                        else
                        {

                        }
                        // if new type is bank hour check limits and increase counter if change is valid - counter is in minutes
                        if (rules.ContainsKey(Constants.RuleCompanyBankHour) && rules[Constants.RuleCompanyBankHour].RuleValue == newPairs[i].PassTypeID)
                        {
                            // Sanja 29.08.2012 - start checking if bank hours is earning before overtime
                            // first check if there is some overtime hours left for use if rule is set to check
                            if (rules.ContainsKey(Constants.RuleOvertimeBeforeBankHours) && rules[Constants.RuleOvertimeBeforeBankHours].RuleValue == Constants.yesInt)
                            {
                                if (checkLimit)
                                {
                                    bool isWSDay = dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date) && isWorkingDay(dayIntervals[newPairs[i].IOPairDate.Date]);
                                    bool dayLimitNotReached = true;
                                    bool wsDayLimitNotReached = true;
                                    bool weekLimitNotReached = true;

                                    if (rules.ContainsKey(Constants.RuleOvertimeDayLimit))
                                        dayLimitNotReached = overtimePaidDayDuration < (rules[Constants.RuleOvertimeDayLimit].RuleValue * 60);

                                    if (isWSDay && rules.ContainsKey(Constants.RuleOvertimeWSLimit))
                                        wsDayLimitNotReached = overtimePaidDayDuration < (rules[Constants.RuleOvertimeWSLimit].RuleValue * 60);

                                    if (rules.ContainsKey(Constants.RuleOvertimeWeekLimit))
                                        weekLimitNotReached = overtimePaidWeekDuration < (rules[Constants.RuleOvertimeWeekLimit].RuleValue * 60);

                                    if (dayLimitNotReached && wsDayLimitNotReached && weekLimitNotReached)
                                        return "overtimeLeftForUse";
                                }
                            }
                            if (newPairs[i].IOPairDate.DayOfWeek == DayOfWeek.Saturday || newPairs[i].IOPairDate.DayOfWeek == DayOfWeek.Sunday)
                            {
                                if (checkLimit && rules.ContainsKey(Constants.RuleBankHrsWeekLimit) && bankHourWeekDuration > (rules[Constants.RuleBankHrsWeekLimit].RuleValue * 60))
                                    return "bankHourWeekLimitReached";
                                if (checkLimit && rules.ContainsKey(Constants.RuleBankHoursDayLimitWeekend) && bankHourDayDuration > (rules[Constants.RuleBankHoursDayLimitWeekend].RuleValue * 60))
                                    return "bankHourDayLimitReached";
                            }
                            else
                            {
                                // Sanja 29.08.2012 - end checking if bank hours is earning before overtime
                                if (checkLimit && rules.ContainsKey(Constants.RuleBankHrsDayLimit) && bankHourDayDuration > (rules[Constants.RuleBankHrsDayLimit].RuleValue * 60))
                                    return "bankHourDayLimitReached";

                                if (checkLimit && rules.ContainsKey(Constants.RuleBankHrsWeekLimit) && bankHourWeekDuration  > (rules[Constants.RuleBankHrsWeekLimit].RuleValue * 60))
                                    return "bankHourWeekLimitReached";
                            }
                            // increase counter
                            if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                                emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value += bankHourValue;
                        }
                    }

                    // if new type is stop working increase counter - counter is in minutes
                    if (rules.ContainsKey(Constants.RuleCompanyStopWorking) && rules[Constants.RuleCompanyStopWorking].RuleValue == newPairs[i].PassTypeID)
                    {
                        // increase counter
                        if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                            emplCounters[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value += pairDuration;
                    }

                    // for night shift, for whole day absences, counters and limits, do validation only for first pair
                    if (newPairs[i].StartTime.Hour == 0 && newPairs[i].StartTime.Minute == 0 && !checkEveryThirdShift)
                        continue;

                    // Sanja 17.09.2012. - if changing counters for second interval night shifts for whole day absences, do not check third shifts counters again
                    if (((newPairs[i].StartTime.Hour == 0 && newPairs[i].StartTime.Minute == 0) || (newPairs[i].EndTime.Hour == 23 && newPairs[i].EndTime.Minute == 59)) && !checkThirdShift)
                        continue;

                    // if new type is annual leave check if there is enough leave left
                    if ((rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && rules[Constants.RuleCompanyAnnualLeave].RuleValue == newPairs[i].PassTypeID)
                        || (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue == newPairs[i].PassTypeID))
                    {
                        // check if employee has right to use annual leave
                        if (checkLimit && !emplAsco.DatetimeValue4.Equals(new DateTime()) && emplAsco.DatetimeValue4.Date > periodStart.Date)
                            return "noRightToUseAnnualLeave";

                        // check if there are hours on bank hours or not justified overtime counter to use before using vacation
                        if (checkLimit && ((rules.ContainsKey(Constants.RuleBankHoursBeforeVacation) && rules[Constants.RuleBankHoursBeforeVacation].RuleValue == Constants.yesInt
                            && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                            || (rules.ContainsKey(Constants.RuleOvertimeNJHoursBeforeVacation) && rules[Constants.RuleOvertimeNJHoursBeforeVacation].RuleValue == Constants.yesInt
                            && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.NotJustifiedOvertime))))
                        {
                            // get shift duration
                            int shiftDuration = 0;
                            List<WorkTimeIntervalTO> pairDateIntervals = new List<WorkTimeIntervalTO>();
                            WorkTimeSchemaTO pairDateSch = new WorkTimeSchemaTO();
                            if (dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date))
                                pairDateIntervals = dayIntervals[newPairs[i].IOPairDate.Date];
                            if (daySchemas.ContainsKey(newPairs[i].IOPairDate.Date))
                                pairDateSch = daySchemas[newPairs[i].IOPairDate.Date];

                            WorkTimeIntervalTO pairInterval = getPairInterval(newPairs[i], dayPairs, pairDateSch, pairDateIntervals, passTypesAllDic);

                            shiftDuration = (int)pairInterval.EndTime.TimeOfDay.Subtract(pairInterval.StartTime.TimeOfDay).TotalMinutes;

                            // if pair is from third sfiht start
                            if (pairInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                            {
                                shiftDuration++;

                                // get next day third shift ending duration
                                if (dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date.AddDays(1)))
                                {
                                    foreach (WorkTimeIntervalTO nextDayInterval in dayIntervals[newPairs[i].IOPairDate.Date.AddDays(1)])
                                    {
                                        if (nextDayInterval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                            continue;

                                        shiftDuration += (int)nextDayInterval.EndTime.TimeOfDay.Subtract(nextDayInterval.StartTime.TimeOfDay).TotalMinutes;
                                    }
                                }
                            }
                            // if pair is from third sfiht end
                            else if (pairInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                            {
                                // get previous day third shift ending duration
                                if (dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date.AddDays(-1)))
                                {
                                    foreach (WorkTimeIntervalTO prevDayInterval in dayIntervals[newPairs[i].IOPairDate.Date.AddDays(-1)])
                                    {
                                        if (prevDayInterval.EndTime.TimeOfDay != new TimeSpan(23, 59, 0))
                                            continue;

                                        shiftDuration += (int)prevDayInterval.EndTime.TimeOfDay.Subtract(prevDayInterval.StartTime.TimeOfDay).TotalMinutes;

                                        shiftDuration++;
                                    }
                                }
                            }

                            // get bank hours value to validate                            
                            // if new pair is in previous month, validate against hours earned till month beginning (counter - earned hours from month beginning),
                            // else if current month is not first after bank hours cut off date, validate against counter
                            // else validate against hours earned from month beginning
                            int bankHoursValueToValidate = 0;
                            int overtimeHoursValueToValidate = 0;

                            if ((rules.ContainsKey(Constants.RuleOvertimeNJHoursBeforeVacation) && rules[Constants.RuleOvertimeNJHoursBeforeVacation].RuleValue == Constants.yesInt
                                && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.NotJustifiedOvertime)))
                                overtimeHoursValueToValidate = emplCounters[(int)Constants.EmplCounterTypes.NotJustifiedOvertime].Value;

                            if (rules.ContainsKey(Constants.RuleBankHoursBeforeVacation) && rules[Constants.RuleBankHoursBeforeVacation].RuleValue == Constants.yesInt
                                && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                            {
                                bankHoursValueToValidate = emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value;

                                DateTime bankHourCutOffDate1 = new DateTime();
                                DateTime bankHourCutOffDate2 = new DateTime();
                                if (rules.ContainsKey(Constants.RuleBankHrsCutOffDate1))
                                    bankHourCutOffDate1 = rules[Constants.RuleBankHrsCutOffDate1].RuleDateTime1;
                                if (rules.ContainsKey(Constants.RuleBankHrsCutOffDate2))
                                    bankHourCutOffDate2 = rules[Constants.RuleBankHrsCutOffDate2].RuleDateTime1;

                                DateTime pairDate = newPairs[i].IOPairDate.Date;

                                // for first day of month, check if pair belongs to previous month
                                WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                                if (daySchemas.ContainsKey(newPairs[i].IOPairDate.Date))
                                    sch = daySchemas[newPairs[i].IOPairDate.Date];
                                List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                                if (dayIntervals.ContainsKey(newPairs[i].IOPairDate.Date))
                                    intervals = dayIntervals[newPairs[i].IOPairDate.Date];
                                if (newPairs[i].IOPairDate.Day == 1)
                                {
                                    if (isPreviousDayPair(newPairs[i], passTypesAllDic, dayPairs, sch, intervals))
                                        pairDate = pairDate.AddDays(-1);
                                }

                                DateTime firstPairsMonthDay = new DateTime(pairDate.Year, pairDate.Month, 1, 0, 0, 0);
                                DateTime firstMonthDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);

                                // calculate if pair month is month after cut off date
                                bool isMonthAfterCutOffDate = false;

                                if (!bankHourCutOffDate1.Equals(new DateTime()) && !bankHourCutOffDate2.Equals(new DateTime())
                                    && !bankHourCutOffDate1.Equals(Constants.dateTimeNullValue()) && !bankHourCutOffDate2.Equals(Constants.dateTimeNullValue()))
                                {
                                    isMonthAfterCutOffDate = firstPairsMonthDay.Equals(new DateTime(DateTime.Now.Year, bankHourCutOffDate1.Month, 1, 0, 0, 0).AddMonths(1))
                                    || firstPairsMonthDay.Equals(new DateTime(DateTime.Now.Year - 1, bankHourCutOffDate1.Month, 1, 0, 0, 0).AddMonths(1))
                                    || firstPairsMonthDay.Equals(new DateTime(DateTime.Now.Year, bankHourCutOffDate2.Month, 1, 0, 0, 0).AddMonths(1))
                                    || firstPairsMonthDay.Equals(new DateTime(DateTime.Now.Year - 1, bankHourCutOffDate2.Month, 1, 0, 0, 0).AddMonths(1));
                                }

                                if (pairDate.Date < firstMonthDay.Date || isMonthAfterCutOffDate)
                                {
                                    if (rules.ContainsKey(Constants.RuleCompanyBankHour))
                                    {
                                        // get pairs earned after month beginning
                                        if (pairDate.Date < firstMonthDay.Date)
                                            //bankHoursValueToValidate -= pairProcessed.SearchEarnedBankHours(newPairs[i].EmployeeID, rules[Constants.RuleCompanyBankHour].RuleValue, 
                                            //firstMonthDay.Date);
                                            bankHoursValueToValidate -= SearchHours(newPairs[i].EmployeeID, rules[Constants.RuleCompanyBankHour].RuleValue,
                                                rules[Constants.RuleCompanyBankHourUsed].RuleValue, bhRounding, firstMonthDay.Date, new DateTime(), conn, passTypesAllDic, schDict);
                                        else
                                        {
                                            bankHoursValueToValidate = SearchHours(newPairs[i].EmployeeID, rules[Constants.RuleCompanyBankHour].RuleValue,
                                                rules[Constants.RuleCompanyBankHourUsed].RuleValue, bhRounding, firstPairsMonthDay.Date, pairDate.Date, conn, passTypesAllDic, schDict);

                                            // add bank hours value from changed day
                                            int bhDayValue = 0;
                                            foreach (IOPairProcessedTO pair in dayPairs)
                                            {
                                                if (pair.IOPairDate.Date < firstMonthDay.Date
                                                    || (pair.IOPairDate.Date == firstMonthDay.Date && isPreviousDayPair(pair, passTypesAllDic, dayPairs, sch, intervals)))
                                                    continue;

                                                int duration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                                                if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                                                    duration++;

                                                if (pair.PassTypeID == rules[Constants.RuleCompanyBankHour].RuleValue)
                                                    bhDayValue += duration;

                                                if (pair.PassTypeID == rules[Constants.RuleCompanyBankHourUsed].RuleValue)
                                                {
                                                    if (duration % bhRounding != 0)
                                                        duration += bhRounding - (duration % bhRounding);

                                                    bhDayValue -= duration;
                                                }
                                            }

                                            bankHoursValueToValidate += bhDayValue;
                                        }
                                    }
                                }
                            }

                            if (shiftDuration > 0 && overtimeHoursValueToValidate >= shiftDuration)
                                return "useNotjustifedOvertimeHoursBeforeVacation";

                            if (shiftDuration > 0 && bankHoursValueToValidate >= shiftDuration)
                                return "useBankHoursBeforeVacation";

                            if (shiftDuration > 0 && (overtimeHoursValueToValidate + bankHoursValueToValidate) >= shiftDuration)
                                return "useNotjustifedOvertimeBankHoursBeforeVacation";
                        }

                        int counterValue = 1;
                        if (daySchemas.ContainsKey(newPairs[i].IOPairDate.Date) && is10HourShift(daySchemas[newPairs[i].IOPairDate.Date]))
                        {
                            if (fromWeekAnnualLeavePairs == null && toWeekAnnualLeavePairs == null)
                            {
                                // get week annual leave pairs
                                int annualLeaveDays = 0;
                                IOPairProcessed annualPair;

                                if (conn != null)
                                    annualPair = new IOPairProcessed(conn);
                                else
                                    annualPair = new IOPairProcessed();

                                List<IOPairProcessedTO> annualLeaveWeekPairs = annualPair.SearchWeekPairs(emplID, newPairs[i].IOPairDate, false, getAnnualLeaveTypesString(rules), null);

                                // if day pairs contains both third shifts for current and previous day, third sfift for current day is already excluded
                                // third sfiht for previous day should be excluded from annualLeaveWeekPairs, and current state from day pairs should be added to those pairs
                                IEnumerator<IOPairProcessedTO> annualPairEnumerator = annualLeaveWeekPairs.GetEnumerator();
                                while (annualPairEnumerator.MoveNext())
                                {
                                    if ((annualPairEnumerator.Current.StartTime.Hour == 0 && annualPairEnumerator.Current.StartTime.Minute == 0)
                                        || (annualPairEnumerator.Current.IOPairDate.Date.Equals(newPairs[i].IOPairDate.AddDays(-1))
                                        && annualPairEnumerator.Current.EndTime.Hour == 23 && annualPairEnumerator.Current.EndTime.Minute == 59))
                                    {
                                        annualLeaveWeekPairs.Remove(annualPairEnumerator.Current);
                                        annualPairEnumerator = annualLeaveWeekPairs.GetEnumerator();
                                    }
                                }

                                foreach (IOPairProcessedTO iopair in dayPairs)
                                {
                                    if ((rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && iopair.PassTypeID == rules[Constants.RuleCompanyAnnualLeave].RuleValue)
                                        || (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && iopair.PassTypeID == rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue))
                                    {
                                        // if changing third shift from previous day, add already changed pairs from this day
                                        if (newPairs[i].StartTime.Hour == 0 && newPairs[i].StartTime.Minute == 0)
                                        {
                                            if (iopair.IOPairDate.Date.Equals(newPairs[i].IOPairDate.Date) && !iopair.compareAL(newPairs[i], getAnnualLeaveTypes(rules)))
                                                annualLeaveWeekPairs.Add(iopair);
                                        }

                                        // if changing third shift from current day, add already changed pairs for this or previous day
                                        if (newPairs[i].EndTime.Hour == 23 && newPairs[i].EndTime.Minute == 59)
                                        {
                                            if ((iopair.IOPairDate.Date.Equals(newPairs[i].IOPairDate.Date) && !iopair.compareAL(newPairs[i], getAnnualLeaveTypes(rules)) && !(iopair.StartTime.Hour == 0 && iopair.StartTime.Minute == 0))
                                                || (iopair.IOPairDate.Date.Equals(newPairs[i].IOPairDate.AddDays(-1).Date) && iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59))
                                                annualLeaveWeekPairs.Add(iopair);
                                        }
                                    }
                                }

                                annualLeaveDays = annualLeaveWeekPairs.Count;

                                // if fourts day of week is changed, subtract/add two days
                                if (annualLeaveDays == (Constants.fullWeek10hShift - 1))
                                    counterValue++;
                            }
                            else
                            {
                                List<IOPairProcessedTO> weekAnnualLeavePairs = getWeekPairs(wholeWeekPeriodPairs, newPairs[i].IOPairDate.Date);

                                // massive input - all pairs are of same type, weekPairs contains whole week pairs that will be in db, so it does not metter if counter is changed for first or last pair of week
                                if (isFirstInWeek(periodStart, periodEnd, newPairs[i].IOPairDate.Date, weekAnnualLeavePairs))
                                {
                                    // if full week holiday, subtract/add two days
                                    if (weekAnnualLeavePairs.Count == Constants.fullWeek10hShift)
                                        counterValue++;
                                }
                            }
                        }

                        // get already used collective annual leave from last cut off date
                        DateTime startDate = new DateTime();
                        if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeaveReservation))
                        {
                            startDate = new DateTime(DateTime.Now.Year, rules[Constants.RuleCompanyCollectiveAnnualLeaveReservation].RuleDateTime1.Month, rules[Constants.RuleCompanyCollectiveAnnualLeaveReservation].RuleDateTime1.Day);

                            if (startDate.Date > DateTime.Now)
                                startDate = startDate.AddYears(-1).Date;
                        }

                        int ptID = -1;
                        if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave))
                            ptID = rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue;

                        int numOfDays = SearchCollectiveAnnualLeave(newPairs[i], emplID, ptID, startDate.Date, exceptDates, newCollectivePairs, rules, conn);

                        // get reserved days for collective annual leave
                        int reserved = 0;
                        if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeaveReservation))
                            reserved = rules[Constants.RuleCompanyCollectiveAnnualLeaveReservation].RuleValue;

                        // employees that are excepted from annual leave reservation
                        if (emplAsco.IntegerValue10 == Constants.yesInt)
                            reserved = 0;

                        // collective annual leave can not be assigned for more then reserved days
                        if (checkLimit && rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave)
                            && newPairs[i].PassTypeID == rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue && numOfDays >= reserved)
                            return "noMoreReservedCollectiveAnnualLeave";

                        if (numOfDays <= reserved)
                            reserved -= numOfDays;
                        else
                            reserved = 0;

                        // check if there are annual leave days reserved
                        int alReserved = 0;
                        if (rules.ContainsKey(Constants.RuleCompanyAnnualLeaveReservation))
                            alReserved = rules[Constants.RuleCompanyAnnualLeaveReservation].RuleValue;

                        // employees that are excepted from annual leave reservation
                        if (emplAsco.IntegerValue10 == Constants.yesInt)
                            alReserved = 0;

                        // check if employee can use current year annual leave
                        int alReservedCurrYear = 0;
                        if (checkLimit && !emplAsco.DatetimeValue6.Equals(new DateTime()) && emplAsco.DatetimeValue6.Date > periodStart.Date
                            && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter))
                            alReservedCurrYear = emplCounters[(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value;

                        bool updateCounter = false;
                        if (rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && newPairs[i].PassTypeID == rules[Constants.RuleCompanyAnnualLeave].RuleValue)
                            updateCounter = !checkLimit || (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter) && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter)
                                && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter)
                                && (emplCounters[(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value + emplCounters[(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value - Math.Max(reserved + alReserved, alReservedCurrYear)
                                >= emplCounters[(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value + counterValue));
                        else if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && newPairs[i].PassTypeID == rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue)
                            updateCounter = !checkLimit || (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter) && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter)
                                && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter)
                                && (emplCounters[(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value + emplCounters[(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value - Math.Max(reserved, alReservedCurrYear)
                                >= emplCounters[(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value + counterValue));

                        if (updateCounter)
                        {
                            if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))
                                emplCounters[(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value += counterValue;
                            else
                                return "noMoreAnnualLeave";
                        }
                        else
                            return "noMoreAnnualLeave";
                    }

                    // if new type is paid leave type (exist not null pass type limit), increase counter if change is valid - counter is in days
                    if (passTypesAllDic.ContainsKey(newPairs[i].PassTypeID) && (passTypesAllDic[newPairs[i].PassTypeID].LimitCompositeID != -1
                        || passTypesAllDic[newPairs[i].PassTypeID].LimitElementaryID != -1 || passTypesAllDic[newPairs[i].PassTypeID].LimitOccasionID != -1))
                    {
                        // if there is elementary limit
                        if (passTypesAllDic[newPairs[i].PassTypeID].LimitElementaryID != -1 && passTypeLimits.ContainsKey(passTypesAllDic[newPairs[i].PassTypeID].LimitElementaryID))
                        {
                            DateTime from = new DateTime(newPairs[i].IOPairDate.Year, passTypeLimits[passTypesAllDic[newPairs[i].PassTypeID].LimitElementaryID].StartDate.Month,
                                passTypeLimits[passTypesAllDic[newPairs[i].PassTypeID].LimitElementaryID].StartDate.Day, 0, 0, 0);
                            DateTime to = from.AddMonths(passTypeLimits[passTypesAllDic[newPairs[i].PassTypeID].LimitElementaryID].Period);

                            IOPairProcessed pairProcessed;
                            if (conn != null)
                                pairProcessed = new IOPairProcessed(conn);
                            else
                                pairProcessed = new IOPairProcessed();
                            int paidLeaveDays = pairProcessed.SearchPaidLeaveDaysOutsidePeriod(from, to, periodStart, periodEnd, newPairs[i].EmployeeID, newPairs[i].PassTypeID,
                                -1, -1, passTypesAllDic[newPairs[i].PassTypeID].LimitElementaryID);

                            if (paidLeaveElementaryPairsDict != null && paidLeaveElementaryPairsDict.ContainsKey(newPairs[i].PassTypeID))
                                paidLeaveDays += paidLeaveElementaryPairsDict[newPairs[i].PassTypeID];

                            if (checkLimit && paidLeaveDays >= passTypeLimits[passTypesAllDic[newPairs[i].PassTypeID].LimitElementaryID].Value)
                                return "ptElementaryLimitReached";

                            if (paidLeaveElementaryPairsDict != null && newPairs[i].StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                            {
                                if (!paidLeaveElementaryPairsDict.ContainsKey(newPairs[i].PassTypeID))
                                    paidLeaveElementaryPairsDict.Add(newPairs[i].PassTypeID, 0);

                                paidLeaveElementaryPairsDict[newPairs[i].PassTypeID]++;
                            }
                        }

                        // if there is composite limit
                        if (passTypesAllDic[newPairs[i].PassTypeID].LimitCompositeID != -1 && passTypeLimits.ContainsKey(passTypesAllDic[newPairs[i].PassTypeID].LimitCompositeID))
                        {
                            if (checkLimit && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.PaidLeaveCounter)
                                && emplCounters[(int)Constants.EmplCounterTypes.PaidLeaveCounter].Value >= passTypeLimits[passTypesAllDic[newPairs[i].PassTypeID].LimitCompositeID].Value)
                                return "ptCompositeLimitReached";

                            // increase counter
                            if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.PaidLeaveCounter))
                                emplCounters[(int)Constants.EmplCounterTypes.PaidLeaveCounter].Value++;
                        }
                    }
                }

                if (checkCountersNegativeValue && checkLimit)
                {
                    if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.OvertimeCounter) && emplCounters[(int)Constants.EmplCounterTypes.OvertimeCounter].Value < 0)
                        return "overtimeNegative";

                    if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter) && emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value < 0)
                        return "bankHourNegative";

                    if (emplCounters.ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter) && emplCounters[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value < 0)
                        return "stopWorkingNegative";
                }

                // Sanja 29.08.2012 - start checking if bank hours earning limit reached
                if (checkLimit && rules.ContainsKey(Constants.RuleBankHrsLimit) && emplCounters.ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter)
                    && emplCounters[(int)Constants.EmplCounterTypes.BankHoursCounter].Value > (rules[Constants.RuleBankHrsLimit].RuleValue * 60))
                    return "bankHourLimitReached";
                // Sanja 29.08.2012 - end checking if bank hours earning limit reached

                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int SearchHours(int emplID, int type, int usedType, int usedRounding, DateTime startDate, DateTime exceptDate, object conn,
            Dictionary<int, PassTypeTO> ptDict, Dictionary<int, WorkTimeSchemaTO> schDict)
        {
            try
            {
                IOPairProcessed pairProcessed;
                if (conn != null)
                    pairProcessed = new IOPairProcessed(conn);
                else
                    pairProcessed = new IOPairProcessed();

                EmployeesTimeSchedule schedule;
                if (conn != null)
                    schedule = new EmployeesTimeSchedule(conn);
                else
                    schedule = new EmployeesTimeSchedule();

                // get bank hours after start date
                int typeDuration = pairProcessed.SearchEarnedHours(emplID, type, startDate.AddDays(1).Date, exceptDate.Date) - pairProcessed.SearchUsedHours(emplID, usedType, usedRounding, startDate.AddDays(1).Date, exceptDate.Date);

                // for start date do not calculate pairs that do not belong to start date
                if (startDate.Date != exceptDate.Date)
                {
                    // get start date pairs
                    List<DateTime> dates = new List<DateTime>();
                    dates.Add(startDate.Date);
                    List<IOPairProcessedTO> startDatePairs = pairProcessed.SearchAllPairsForEmpl(emplID.ToString().Trim(), dates, "");

                    // get start date schedule, schema, intervals
                    Dictionary<int, List<EmployeeTimeScheduleTO>> schedules = schedule.SearchEmployeesSchedulesExactDate(emplID.ToString().Trim(), startDate.Date, startDate.Date, null);
                    List<EmployeeTimeScheduleTO> emplSchedule = new List<EmployeeTimeScheduleTO>();
                    if (schedules.ContainsKey(emplID))
                        emplSchedule = schedules[emplID];

                    WorkTimeSchemaTO sch = getTimeSchema(startDate.Date, emplSchedule, schDict);
                    List<WorkTimeIntervalTO> dayIntervals = getTimeSchemaInterval(startDate.Date, emplSchedule, schDict);

                    int earned = 0;
                    int used = 0;

                    foreach (IOPairProcessedTO pair in startDatePairs)
                    {
                        if (isPreviousDayPair(pair, ptDict, startDatePairs, sch, dayIntervals))
                            continue;

                        if (pair.PassTypeID == type)
                        {
                            int duration = (int)pair.EndTime.TimeOfDay.Subtract(pair.StartTime.TimeOfDay).TotalMinutes;

                            if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                                duration++;

                            earned += duration;
                        }

                        if (pair.PassTypeID == usedType)
                        {
                            int pairDuration = (int)pair.EndTime.TimeOfDay.Subtract(pair.StartTime.TimeOfDay).TotalMinutes;

                            if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                                pairDuration++;

                            if (pairDuration % usedRounding != 0)
                                pairDuration += usedRounding - (pairDuration % usedRounding);

                            used += pairDuration;
                        }
                    }

                    typeDuration += earned - used;
                }

                return typeDuration;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int SearchCollectiveAnnualLeave(IOPairProcessedTO newPair, int emplID, int collectiveALType, DateTime startDate, List<DateTime> exceptDates,
            List<IOPairProcessedTO> newPairs, Dictionary<string, RuleTO> rules, object conn)
        {
            try
            {
                int numOfDays = 0;

                if (emplID == -1 || collectiveALType == -1)
                    return numOfDays;

                IOPairProcessed pairProcessed;
                if (conn != null)
                    pairProcessed = new IOPairProcessed(conn);
                else
                    pairProcessed = new IOPairProcessed();

                // get collective annual leave after annual leave cut off date
                numOfDays = pairProcessed.SearchCollectiveAnnualLeaves(emplID, collectiveALType, startDate.Date.AddDays(1), exceptDates);

                // get collective pairs from newPairs
                foreach (IOPairProcessedTO pair in newPairs)
                {
                    if (pair.PassTypeID != collectiveALType || pair.StartTime.TimeOfDay == new TimeSpan(0, 0, 0) || pair.compareAL(newPair, getAnnualLeaveTypes(rules)))
                        continue;

                    // if changing pair from third shift do not check previous day pair from third shift
                    if (newPair.StartTime.TimeOfDay == new TimeSpan(0, 0, 0) && pair.IOPairDate.Date.Equals(newPair.IOPairDate.Date.AddDays(-1)) && pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                        continue;

                    numOfDays++;
                }

                return numOfDays;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool isThirdShiftEndInterval(WorkTimeIntervalTO interval)
        {
            try
            {
                if (interval.StartTime.Hour == 0 && interval.StartTime.Minute == 0)
                {
                    double intervalDuration = 0;
                    if (interval.EndTime.Hour == 23 && interval.EndTime.Minute == 59)
                        intervalDuration = interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).Add(new TimeSpan(0, 1, 0)).TotalHours;
                    else
                        intervalDuration = interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalHours;

                    if (intervalDuration > 0 && intervalDuration < Constants.dayDurationStandardShift)
                        return true;
                    else
                        return false;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool isThirdShiftBeginningInterval(WorkTimeIntervalTO interval)
        {
            try
            {
                if (interval.EndTime.Hour == 23 && interval.EndTime.Minute == 59)
                {
                    double intervalDuration = interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).Add(new TimeSpan(0, 1, 0)).TotalHours;

                    if (intervalDuration > 0 && intervalDuration < Constants.dayDurationStandardShift)
                        return true;
                    else
                        return false;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static WorkTimeSchemaTO getTimeSchema(DateTime date, List<EmployeeTimeScheduleTO> timeScheduleList, Dictionary<int, WorkTimeSchemaTO> schemas)
        {
            try
            {
                WorkTimeSchemaTO sch = new WorkTimeSchemaTO();

                int timeScheduleIndex = -1;

                for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
                {
                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
                    if (date >= timeScheduleList[scheduleIndex].Date)
                    //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
                    {
                        timeScheduleIndex = scheduleIndex;
                    }
                }

                if (timeScheduleIndex >= 0)
                {
                    int schemaID = timeScheduleList[timeScheduleIndex].TimeSchemaID;

                    if (schemas.ContainsKey(schemaID))
                        sch = schemas[schemaID];
                }

                return sch;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<WorkTimeIntervalTO> getTimeSchemaInterval(DateTime date, List<EmployeeTimeScheduleTO> timeScheduleList, Dictionary<int, WorkTimeSchemaTO> schemas)
        {
            try
            {
                List<WorkTimeIntervalTO> intervalList = new List<WorkTimeIntervalTO>();

                int timeScheduleIndex = -1;

                for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
                {
                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
                    if (date >= timeScheduleList[scheduleIndex].Date)
                    //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
                    {
                        timeScheduleIndex = scheduleIndex;
                    }
                }

                if (timeScheduleIndex >= 0)
                {
                    int cycleDuration = 0;
                    int startDay = timeScheduleList[timeScheduleIndex].StartCycleDay;
                    int schemaID = timeScheduleList[timeScheduleIndex].TimeSchemaID;

                    WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                    if (schemas.ContainsKey(schemaID))
                    {
                        schema = schemas[schemaID];
                        cycleDuration = schema.CycleDuration;
                    }

                    TimeSpan days = new TimeSpan(date.Date.Ticks - timeScheduleList[timeScheduleIndex].Date.Date.Ticks);

                    Dictionary<int, WorkTimeIntervalTO> table = schema.Days[(startDay + (int)days.TotalDays) % cycleDuration];
                    for (int i = 0; i < table.Count; i++)
                    {
                        intervalList.Add(table[i]);
                    }
                }

                return intervalList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<WorkTimeIntervalTO> getTimeSchemaIntervalGroup(DateTime date, List<EmployeeGroupsTimeScheduleTO> timeScheduleList, Dictionary<int, WorkTimeSchemaTO> schemas)
        {
            try
            {
                List<WorkTimeIntervalTO> intervalList = new List<WorkTimeIntervalTO>();

                int timeScheduleIndex = -1;

                for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
                {
                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
                    if (date >= timeScheduleList[scheduleIndex].Date)
                    //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
                    {
                        timeScheduleIndex = scheduleIndex;
                    }
                }

                if (timeScheduleIndex >= 0)
                {
                    int cycleDuration = 0;
                    int startDay = timeScheduleList[timeScheduleIndex].StartCycleDay;
                    int schemaID = timeScheduleList[timeScheduleIndex].TimeSchemaID;

                    WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                    if (schemas.ContainsKey(schemaID))
                    {
                        schema = schemas[schemaID];
                        cycleDuration = schema.CycleDuration;
                    }

                    TimeSpan days = new TimeSpan(date.Date.Ticks - timeScheduleList[timeScheduleIndex].Date.Date.Ticks);

                    Dictionary<int, WorkTimeIntervalTO> table = schema.Days[(startDay + (int)days.TotalDays) % cycleDuration];
                    for (int i = 0; i < table.Count; i++)
                    {
                        intervalList.Add(table[i]);
                    }
                }

                return intervalList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime getWeekBeggining(DateTime date)
        {
            try
            {
                int dayNum = 0;
                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        dayNum = 0;
                        break;
                    case DayOfWeek.Tuesday:
                        dayNum = 1;
                        break;
                    case DayOfWeek.Wednesday:
                        dayNum = 2;
                        break;
                    case DayOfWeek.Thursday:
                        dayNum = 3;
                        break;
                    case DayOfWeek.Friday:
                        dayNum = 4;
                        break;
                    case DayOfWeek.Saturday:
                        dayNum = 5;
                        break;
                    case DayOfWeek.Sunday:
                        dayNum = 6;
                        break;
                }

                return date.AddDays(-dayNum).Date; // first day of current week
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime getWeekEnding(DateTime date)
        {
            try
            {
                int dayNum = 0;
                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        dayNum = 0;
                        break;
                    case DayOfWeek.Tuesday:
                        dayNum = 1;
                        break;
                    case DayOfWeek.Wednesday:
                        dayNum = 2;
                        break;
                    case DayOfWeek.Thursday:
                        dayNum = 3;
                        break;
                    case DayOfWeek.Friday:
                        dayNum = 4;
                        break;
                    case DayOfWeek.Saturday:
                        dayNum = 5;
                        break;
                    case DayOfWeek.Sunday:
                        dayNum = 6;
                        break;
                }

                return date.AddDays(6 - dayNum).Date; // last day of current week
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime createDate(string dateStr)
        {
            try
            {
                string day = "";
                string month = "";
                string year = "";

                int dotIndex = dateStr.IndexOf('.');

                if (dotIndex > 0)
                {
                    day = dateStr.Substring(0, dotIndex);
                    dateStr = dateStr.Substring(dotIndex + 1);

                    dotIndex = dateStr.IndexOf('.');

                    if (dotIndex > 0)
                    {
                        month = dateStr.Substring(0, dotIndex);
                        dateStr = dateStr.Substring(dotIndex + 1);

                        dotIndex = dateStr.IndexOf('.');

                        if (dotIndex > 0)
                        {
                            year = dateStr.Substring(0, dotIndex);
                            dateStr = dateStr.Substring(dotIndex + 1);

                            if (!dateStr.Trim().Equals(""))
                                return new DateTime();
                        }
                        else
                            return new DateTime();
                    }
                    else
                        return new DateTime();
                }
                else
                    return new DateTime();

                int dateDay = -1;
                int dateMonth = -1;
                int dateYear = -1;

                if (!int.TryParse(day, out dateDay) || !int.TryParse(month, out dateMonth) || !int.TryParse(year, out dateYear))
                    return new DateTime();

                DateTime date = new DateTime();

                try
                {
                    date = new DateTime(dateYear, dateMonth, dateDay, 0, 0, 0);
                }
                catch
                {
                    date = new DateTime();
                }

                return date;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void writeLoginData(string message)
        {
            try
            {
                DebugLog log = new DebugLog(Constants.logFilePath + "LoginData.txt");
                log.writeLog(message);
            }
            catch { }
        }

        public static void writeIncorrectChangingDate(string message)
        {
            try
            {
                DebugLog log = new DebugLog(Constants.logFilePath + "IncorectChanginDate.txt");
                log.writeLog(message);
            }
            catch { }
        }

        public static bool isWholeIntervalPair(IOPairProcessedTO pair, WorkTimeIntervalTO interval, WorkTimeSchemaTO schema)
        {
            try
            {
                bool wholeIntervalPair = false;

                if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                {
                    if (pair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && pair.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay
                        && pair.EndTime.Subtract(pair.StartTime).TotalMinutes == interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes)
                    {
                        wholeIntervalPair = true;
                    }
                }
                else if (interval.EndTime.TimeOfDay == pair.EndTime.TimeOfDay && interval.StartTime.TimeOfDay == pair.StartTime.TimeOfDay)
                    wholeIntervalPair = true;

                return wholeIntervalPair;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool isPreviousDayPair(IOPairProcessedTO pairTO, Dictionary<int, PassTypeTO> passTypesAllDic, List<IOPairProcessedTO> dayPairs, WorkTimeSchemaTO schema,
            List<WorkTimeIntervalTO> dayIntervals)
        {
            try
            {
                bool previousDayPair = false;

                if (passTypesAllDic.ContainsKey(pairTO.PassTypeID))
                {
                    if (passTypesAllDic[pairTO.PassTypeID].IsPass == Constants.overtimePassType)
                    {
                        int index = indexOf(dayPairs, pairTO);

                        int i = index - 1;

                        bool overtimeAfterShift = true;
                        while (i >= 0 && i < dayPairs.Count - 1)
                        {
                            if (!dayPairs[i + 1].StartTime.Equals(dayPairs[i].EndTime))
                            {
                                overtimeAfterShift = false;
                                break;
                            }
                            else if (passTypesAllDic.ContainsKey(dayPairs[i].PassTypeID) && passTypesAllDic[dayPairs[i].PassTypeID].IsPass != Constants.overtimePassType)
                                break;

                            i--;
                        }

                        if (overtimeAfterShift)
                        {
                            if (i >= 0)
                            {
                                WorkTimeIntervalTO pairInterval = getPairInterval(dayPairs[i], dayPairs, schema, dayIntervals, passTypesAllDic);

                                if (!pairInterval.StartTime.Equals(new DateTime()) || !pairInterval.EndTime.Equals(new DateTime()))
                                {
                                    if (Common.Misc.isThirdShiftEndInterval(pairInterval))// && checkCutOffDate(pairTO.IOPairDate.AddDays(-1).Date))
                                        previousDayPair = true;
                                }
                            }
                            else if (dayPairs.Count > 0 && !dayPairs[0].StartTime.Equals(new DateTime()) && !dayPairs[0].EndTime.Equals(new DateTime())
                                    && dayPairs[0].StartTime.Hour == 0 && dayPairs[0].StartTime.Minute == 0)// && checkCutOffDate(dayPairs[0].IOPairDate.AddDays(-1).Date))
                                previousDayPair = true;
                        }
                    }
                    else
                    {
                        WorkTimeIntervalTO pairInterval = getPairInterval(pairTO, dayPairs, schema, dayIntervals, passTypesAllDic);

                        if (!pairInterval.StartTime.Equals(new DateTime()) || !pairInterval.EndTime.Equals(new DateTime()))
                        {
                            if (Common.Misc.isThirdShiftEndInterval(pairInterval))// && checkCutOffDate(PairTO.IOPairDate.AddDays(-1).Date))
                                previousDayPair = true;
                        }
                    }
                }

                return previousDayPair;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int indexOf(List<IOPairProcessedTO> dayPairs, IOPairProcessedTO pair)
        {
            try
            {
                int index = -1;

                for (int i = 0; i < dayPairs.Count; i++)
                {
                    if (pair.compare(dayPairs[i]))
                    {
                        index = i;
                        break;
                    }
                }

                return index;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static WorkTimeIntervalTO getPairInterval(IOPairProcessedTO pair, List<IOPairProcessedTO> dayPairs, WorkTimeSchemaTO schema, List<WorkTimeIntervalTO> dayIntervals,
            Dictionary<int, PassTypeTO> passTypesAllDic)
        {
            try
            {
                WorkTimeIntervalTO pairInterval = new WorkTimeIntervalTO();
                foreach (WorkTimeIntervalTO interval in dayIntervals)
                {
                    if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                    {
                        if (pair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && pair.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay
                            && passTypesAllDic.ContainsKey(pair.PassTypeID) && passTypesAllDic[pair.PassTypeID].IsPass != Constants.overtimePassType
                            && pair.EndTime.Subtract(pair.StartTime).TotalMinutes <= interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes)
                        {
                            pairInterval = interval.Clone();
                            pairInterval.StartTime = getIntervalStart(interval, dayPairs, schema, pair.IOPairDate.Date, passTypesAllDic);
                            pairInterval.EndTime = getIntervalEnd(interval, dayPairs, schema, pair.IOPairDate.Date, passTypesAllDic);
                            break;
                        }
                    }
                    else if (pair.StartTime.TimeOfDay >= interval.StartTime.TimeOfDay && pair.EndTime.TimeOfDay <= interval.EndTime.TimeOfDay)
                    {
                        pairInterval = interval;
                        break;
                    }
                }

                return pairInterval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static WorkTimeIntervalTO getIntervalBeforePair(IOPairProcessedTO pair, List<IOPairProcessedTO> dayPairs, List<WorkTimeIntervalTO> dayIntervals,
            WorkTimeSchemaTO sch, Dictionary<int, PassTypeTO> ptDict)
        {
            try
            {
                WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
                for (int i = dayIntervals.Count - 1; i >= 0; i--)
                {
                    DateTime end = getIntervalEnd(dayIntervals[i], dayPairs, sch, pair.IOPairDate.Date, ptDict);
                    if (pair.StartTime.TimeOfDay >= end.TimeOfDay)
                    {
                        interval = dayIntervals[i].Clone();
                        interval.StartTime = getIntervalStart(dayIntervals[i], dayPairs, sch, pair.IOPairDate.Date, ptDict);
                        interval.EndTime = end;
                        break;
                    }
                }

                return interval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static WorkTimeIntervalTO getIntervalAfterPair(IOPairProcessedTO pair, List<IOPairProcessedTO> dayPairs, List<WorkTimeIntervalTO> dayIntervals,
            WorkTimeSchemaTO sch, Dictionary<int, PassTypeTO> ptDict)
        {
            try
            {
                WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
                for (int i = 0; i < dayIntervals.Count; i++)
                {
                    DateTime start = getIntervalStart(dayIntervals[i], dayPairs, sch, pair.IOPairDate.Date, ptDict);
                    if (pair.EndTime.TimeOfDay <= start.TimeOfDay)
                    {
                        interval = dayIntervals[i].Clone();
                        interval.StartTime = start;
                        interval.EndTime = getIntervalEnd(dayIntervals[i], dayPairs, sch, pair.IOPairDate.Date, ptDict);
                        break;
                    }
                }

                return interval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime getIntervalStart(WorkTimeIntervalTO interval, List<IOPairProcessedTO> dayPairs, WorkTimeSchemaTO schema, DateTime date, Dictionary<int, PassTypeTO> passTypesAllDic)
        {
            try
            {
                DateTime intervalStart = interval.StartTime;

                if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                {
                    int index = indexOfFirstInInterval(dayPairs, interval, schema, date, passTypesAllDic);

                    if (index >= 0 && index < dayPairs.Count)
                        intervalStart = dayPairs[index].StartTime;
                    else
                        intervalStart = interval.EarliestArrived;
                }

                return intervalStart;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime getIntervalEnd(WorkTimeIntervalTO interval, List<IOPairProcessedTO> dayPairs, WorkTimeSchemaTO schema, DateTime date, Dictionary<int, PassTypeTO> passTypesAllDic)
        {
            try
            {
                DateTime intervalEnd = getIntervalStart(interval, dayPairs, schema, date, passTypesAllDic).AddMinutes((int)(interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes));

                return intervalEnd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int indexOfFirstInInterval(List<IOPairProcessedTO> dayPairs, WorkTimeIntervalTO interval, WorkTimeSchemaTO schema, DateTime date, Dictionary<int, PassTypeTO> passTypesAllDic)
        {
            try
            {
                int index = -1;

                for (int i = 0; i < dayPairs.Count; i++)
                {
                    if (!dayPairs[i].IOPairDate.Date.Equals(date.Date))
                        continue;

                    if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                    {
                        if (dayPairs[i].StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && dayPairs[i].EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay
                            && passTypesAllDic.ContainsKey(dayPairs[i].PassTypeID) && passTypesAllDic[dayPairs[i].PassTypeID].IsPass != Constants.overtimePassType
                            && dayPairs[i].EndTime.Subtract(dayPairs[i].StartTime).TotalMinutes <= interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes)
                        {
                            index = i;
                            break;
                        }
                    }
                    else if (dayPairs[i].StartTime.TimeOfDay >= interval.StartTime.TimeOfDay && dayPairs[i].EndTime.TimeOfDay <= interval.EndTime.TimeOfDay)
                    {
                        index = i;
                        break;
                    }
                }

                return index;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool isExpatOut(Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict, EmployeeTO empl)
        {
            try
            {                
                if (rulesDict.ContainsKey(Constants.defaultWorkingUnitID) && rulesDict[Constants.defaultWorkingUnitID].ContainsKey((int)Constants.EmployeeTypesFIAT.BC)
                    && rulesDict[Constants.defaultWorkingUnitID][(int)Constants.EmployeeTypesFIAT.BC].ContainsKey(Constants.RuleExpatOutType)
                    && rulesDict[Constants.defaultWorkingUnitID][(int)Constants.EmployeeTypesFIAT.BC][Constants.RuleExpatOutType].RuleValue == empl.EmployeeTypeID)
                    return true;
                else
                    return false;
               

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Dictionary<string, EmployeeTO> GetICodeData()
        {
            Dictionary<string, EmployeeTO> ICodeDataDict = new Dictionary<string, EmployeeTO>();

            try
            {
                // create connection from config file
                object conn = DBConnectionManager.Instance.MakeNewDBConnection();
                if (conn == null)
                    return ICodeDataDict;

                // get data
                ICodeDataDict = new EmployeeAsco4(conn).SearchICodeData();

                // close connection
                DBConnectionManager.Instance.CloseDBConnection(conn);
            }
            catch
            {
                ICodeDataDict = new Dictionary<string, EmployeeTO>();
            }

            return ICodeDataDict;
        }

        public static SystemClosingEventTO getClosingEvent(DateTime date, object conn)
        {
            SystemClosingEventTO closingEventTO = new SystemClosingEventTO();
            try
            {
                SystemClosingEvent evt;

                if (conn == null)
                    evt = new SystemClosingEvent();
                else
                    evt = new SystemClosingEvent(conn);

                List<SystemClosingEventTO> closingList = evt.Search(date, date);

                foreach (SystemClosingEventTO evtTO in closingList)
                {
                    if (evtTO.Type.Trim().ToUpper().Equals(Constants.closingEventTypeDemanded))
                    {
                        closingEventTO = evtTO;
                        break;
                    }
                    else if (evtTO.Type.Trim().ToUpper().Equals(Constants.closingEventTypeRegularPeriodical))
                    {
                        if ((evtTO.StartTime.TimeOfDay < evtTO.EndTime.TimeOfDay && date.TimeOfDay >= evtTO.StartTime.TimeOfDay && date.TimeOfDay < evtTO.EndTime.TimeOfDay)
                            || (evtTO.StartTime.TimeOfDay >= evtTO.EndTime.TimeOfDay && (date.TimeOfDay < evtTO.EndTime.TimeOfDay || date.TimeOfDay >= evtTO.StartTime.TimeOfDay)))
                        {
                            closingEventTO = evtTO;
                            break;
                        }
                    }
                }
            }
            catch
            {
                closingEventTO = new SystemClosingEventTO();
            }

            return closingEventTO;
        }

        public static string getString(object list)
        {
            string listString = "";
            try
            {
                if (list != null && list is List<string>)
                {
                    foreach (string element in (List<string>)list)
                    {
                        listString += element.Trim().Trim() + ",";
                    }

                    if (listString.Length > 0)
                        listString = listString.Substring(0, listString.Length - 1);
                }
            }
            catch (Exception ex)
            {
                listString = "";
            }

            return listString;
        }

        public static string getAscoValue(EmployeeAsco4TO ascoTO, string col)
        {
            try
            {
                string value = "";
                switch (col)
                {
                    case "datetime_value_1":
                        if (ascoTO.DatetimeValue1 != new DateTime())
                            value = ascoTO.DatetimeValue1.ToString(Constants.dateFormat);
                        else
                            value = "";
                        break;
                    case "datetime_value_2":
                        if (ascoTO.DatetimeValue2 != new DateTime())
                            value = ascoTO.DatetimeValue2.ToString(Constants.dateFormat);
                        else
                            value = "";
                        break;
                    case "datetime_value_3":
                        if (ascoTO.DatetimeValue3 != new DateTime())
                            value = ascoTO.DatetimeValue3.ToString(Constants.dateFormat);
                        else
                            value = "";
                        break;
                    case "datetime_value_4":
                        if (ascoTO.DatetimeValue4 != new DateTime())
                            value = ascoTO.DatetimeValue4.ToString(Constants.dateFormat);
                        else
                            value = "";
                        break;
                    case "datetime_value_5":
                        if (ascoTO.DatetimeValue5 != new DateTime())
                            value = ascoTO.DatetimeValue5.ToString(Constants.dateFormat);
                        else
                            value = "";
                        break;
                    case "datetime_value_6":
                        if (ascoTO.DatetimeValue6 != new DateTime())
                            value = ascoTO.DatetimeValue6.ToString(Constants.dateFormat);
                        else
                            value = "";
                        break;
                    case "datetime_value_7":
                        if (ascoTO.DatetimeValue7 != new DateTime())
                            value = ascoTO.DatetimeValue7.ToString(Constants.dateFormat);
                        else
                            value = "";
                        break;
                    case "datetime_value_8":
                        if (ascoTO.DatetimeValue8 != new DateTime())
                            value = ascoTO.DatetimeValue8.ToString(Constants.dateFormat);
                        else
                            value = "";
                        break;
                    case "datetime_value_9":
                        if (ascoTO.DatetimeValue9 != new DateTime())
                            value = ascoTO.DatetimeValue9.ToString(Constants.dateFormat);
                        else
                            value = "";
                        break;
                    case "datetime_value_10":
                        if (ascoTO.DatetimeValue10 != new DateTime())
                            value = ascoTO.DatetimeValue10.ToString(Constants.dateFormat);
                        else
                            value = "";
                        break;
                    case "integer_value_1":
                        if (ascoTO.IntegerValue1 != -1)
                            value = ascoTO.IntegerValue1.ToString().Trim();
                        else
                            value = "";
                        break;
                    case "integer_value_2":
                        if (ascoTO.IntegerValue2 != -1)
                            value = ascoTO.IntegerValue2.ToString().Trim();
                        else
                            value = "";
                        break;
                    case "integer_value_3":
                        if (ascoTO.IntegerValue3 != -1)
                            value = ascoTO.IntegerValue3.ToString().Trim();
                        else
                            value = "";
                        break;
                    case "integer_value_4":
                        if (ascoTO.IntegerValue4 != -1)
                            value = ascoTO.IntegerValue4.ToString().Trim();
                        else
                            value = "";
                        break;
                    case "integer_value_5":
                        if (ascoTO.IntegerValue5 != -1)
                            value = ascoTO.IntegerValue5.ToString().Trim();
                        else
                            value = "";
                        break;
                    case "integer_value_6":
                        if (ascoTO.IntegerValue6 != -1)
                            value = ascoTO.IntegerValue6.ToString().Trim();
                        else
                            value = "";
                        break;
                    case "integer_value_7":
                        if (ascoTO.IntegerValue7 != -1)
                            value = ascoTO.IntegerValue7.ToString().Trim();
                        else
                            value = "";
                        break;
                    case "integer_value_8":
                        if (ascoTO.IntegerValue8 != -1)
                            value = ascoTO.IntegerValue8.ToString().Trim();
                        else
                            value = "";
                        break;
                    case "integer_value_9":
                        if (ascoTO.IntegerValue9 != -1)
                            value = ascoTO.IntegerValue9.ToString().Trim();
                        else
                            value = "";
                        break;
                    case "integer_value_10":
                        if (ascoTO.IntegerValue10 != -1)
                            value = ascoTO.IntegerValue10.ToString().Trim();
                        else
                            value = "";
                        break;
                    case "nvarchar_value_1":
                        value = ascoTO.NVarcharValue1.Trim();
                        break;
                    case "nvarchar_value_2":
                        value = ascoTO.NVarcharValue2.Trim();
                        break;
                    case "nvarchar_value_3":
                        value = ascoTO.NVarcharValue3.Trim();
                        break;
                    case "nvarchar_value_4":
                        value = ascoTO.NVarcharValue4.Trim();
                        break;
                    case "nvarchar_value_5":
                        value = ascoTO.NVarcharValue5.Trim();
                        break;
                    case "nvarchar_value_6":
                        value = ascoTO.NVarcharValue6.Trim();
                        break;
                    case "nvarchar_value_7":
                        value = ascoTO.NVarcharValue7.Trim();
                        break;
                    case "nvarchar_value_8":
                        value = ascoTO.NVarcharValue8.Trim();
                        break;
                    case "nvarchar_value_9":
                        value = ascoTO.NVarcharValue9.Trim();
                        break;
                    case "nvarchar_value_10":
                        value = ascoTO.NVarcharValue10.Trim();
                        break;
                }

                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static decimal getMaxDuration(ref string maxCode, ref DateTime maxDate, Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>> emplDayAnalitic, string bhUsedCode)
        {
            try
            {
                decimal maxDuration = 0;
                maxCode = "";
                maxDate = new DateTime();

                foreach (string typeCode in emplDayAnalitic.Keys)
                {
                    if (typeCode == bhUsedCode)
                        continue;

                    foreach (DateTime date in emplDayAnalitic[typeCode].Keys)
                    {
                        decimal duration = emplDayAnalitic[typeCode][date].HrsAmount;
                        if (maxDuration < duration)
                        {
                            maxDuration = duration;
                            maxCode = typeCode;
                            maxDate = date;
                        }
                    }
                }

                return maxDuration;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private static WorkingUnitTO getFiatCostCenter(int wu, Dictionary<int, WorkingUnitTO> WUnits)
        {
            try
            {
                WorkingUnitTO cc = new WorkingUnitTO();

                if (WUnits.ContainsKey(wu) && WUnits.ContainsKey(WUnits[wu].ParentWorkingUID) && WUnits.ContainsKey(WUnits[WUnits[wu].ParentWorkingUID].ParentWorkingUID))
                    cc = WUnits[WUnits[WUnits[wu].ParentWorkingUID].ParentWorkingUID];

                return cc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string getEmplType(int company, int typeID, Dictionary<int, Dictionary<int, string>> emplTypes)
        {
            try
            {
                string type = "";

                if (emplTypes.ContainsKey(company) && emplTypes[company].ContainsKey(typeID))
                    type = emplTypes[company][typeID];

                return type;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool isWorkOnHolidayFiat(Dictionary<string, List<DateTime>> personalHolidayDays, List<DateTime> nationalHolidaysDays, List<DateTime> nationalHolidaysDaysSundays,
            List<HolidaysExtendedTO> nationalTransferableHolidays, List<DateTime> paidHolidaysDays, DateTime date, WorkTimeSchemaTO schema, EmployeeAsco4TO asco, EmployeeTO empl,
            Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict)
        {
            try
            {
                bool isHoliday = false;

                if (Common.Misc.isExpatOut(rulesDict, empl))
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
                                if (paidHolidaysDays.Contains(currDate.Date) && currDate.DayOfWeek == DayOfWeek.Sunday)
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
                throw ex;
            }
        }

        public static bool FIATRegularEmployee(int typeID)
        {
            try
            {
                return typeID == (int)Constants.EmployeeTypesFIAT.BC || typeID == (int)Constants.EmployeeTypesFIAT.WC || typeID == (int)Constants.EmployeeTypesFIAT.Professional
                    || typeID == (int)Constants.EmployeeTypesFIAT.ProfessionalExpert || typeID == (int)Constants.EmployeeTypesFIAT.Manager || typeID == (int)Constants.EmployeeTypesFIAT.ExpatOut;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static uint GenerateFiatPYData(string employeeString, Dictionary<int, EmployeeTO> emplDict, DateTime startDate, DateTime endDate, string type, List<string> analiticStrings,
            List<string> bufferStrings, List<string> bufferExpatStrings, List<string> sumStrings, Dictionary<int, OnlineMealsTypesTO> mealTypesDict, ref bool unRegularDataFound,
            Dictionary<int, List<string>> unregularEmployees, Dictionary<int, Dictionary<DateTime, decimal>> emplBHPayDict, Dictionary<int, Dictionary<DateTime, decimal>> emplSWPayDict, 
            bool vacationPay, bool bhPayFile, bool bhPayRegular, int bhBalanceMonth, bool payBH, bool swPayFile, bool swPay, bool stopWorkingPay, int swBalanceMonth, bool paySW, bool leavingEmpls)
        {
            try
            {
                DateTime defaultSicknessStart = new DateTime(1900, 1, 1);

                Dictionary<int, PassTypeTO> typesDict = new PassType().SearchDictionary();

                // change collective annual leave payment code
                List<int> CALTypes = new Rule().SearchRulesExact(Constants.RuleCompanyCollectiveAnnualLeave);

                // get annual leave payment code
                List<int> ALTypes = new Rule().SearchRulesExact(Constants.RuleCompanyAnnualLeave);

                string ALPaymentCode = "";

                if (ALTypes.Count > 0 && typesDict.ContainsKey(ALTypes[0]))
                    ALPaymentCode = typesDict[ALTypes[0]].PaymentCode;
                
                foreach (int ptID in typesDict.Keys)
                {
                    if (type == Constants.PYTypeEstimated && CALTypes.Contains(ptID))
                        typesDict[ptID].PaymentCode = Constants.FiatCollectiveAnnualLeavePaymentCode;

                    if (Constants.FiatClosureTypes().Contains(ptID))
                        typesDict[ptID].PaymentCode += Constants.FiatClosurePaymentCode;

                    if (Constants.FiatLayOffTypes().Contains(ptID))
                        typesDict[ptID].PaymentCode += Constants.FiatLayOffPaymentCode;

                    if (Constants.FiatStoppageTypes().Contains(ptID))
                        typesDict[ptID].PaymentCode += Constants.FiatStoppagePaymentCode;

                    if (Constants.FiatHolidayTypes().Contains(ptID))
                        typesDict[ptID].PaymentCode += Constants.FiatPublicHollidayPaymentCode;
                }
                

                // get employees dict
                emplDict = new Employee().SearchDictionary(employeeString);

                //get all ioPairsProcessed For Selected Interval
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> processedPairsDict = new IOPairProcessed().getPairsForInterval(startDate.Date, endDate.Date.AddDays(1), employeeString);

                // get all meals for selected period
                Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>> employeeMeals = new OnlineMealsUsed().SearchMealsUsedDict(employeeString, startDate.Date, endDate.Date.AddDays(2));

                // get meal types dictionary
                //Dictionary<int, OnlineMealsTypesTO> mealTypesDict = new Dictionary<int, OnlineMealsTypesTO>();
                List<OnlineMealsTypesTO> typeList = new OnlineMealsTypes().Search();
                foreach (OnlineMealsTypesTO mealType in typeList)
                {
                    if (!mealTypesDict.ContainsKey(mealType.MealTypeID))
                        mealTypesDict.Add(mealType.MealTypeID, mealType);
                }

                //get manual created ioPairsProcessed After Selected Interval
                IOPairProcessed processedPair = new IOPairProcessed();
                processedPair.IOPairProcessedTO.ManualCreated = Constants.yesInt;
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> processedPairsAfterDict = processedPair.getPairsForInterval(endDate.Date.AddDays(1), new DateTime(), employeeString);

                //create employeeDictionary for sum
                Dictionary<int, Dictionary<string, Dictionary<DateTime, EmployeePYDataSumTO>>> employeeSumDict = new Dictionary<int, Dictionary<string, Dictionary<DateTime, EmployeePYDataSumTO>>>();

                //create employeeDictionary for buffers
                Dictionary<int, EmployeePYDataBufferTO> employeeBuffersDict = new Dictionary<int, EmployeePYDataBufferTO>();

                //create list for analitical data
                Dictionary<int, Dictionary<DateTime, Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>>> employeeAnaliticalDict = new Dictionary<int, Dictionary<DateTime, Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>>>();

                // list of payed bank hours
                List<BufferMonthlyBalancePaidTO> emplBHPaidHours = new List<BufferMonthlyBalancePaidTO>();

                // list of payed stop working hours
                List<BufferMonthlyBalancePaidTO> emplSWPaidHours = new List<BufferMonthlyBalancePaidTO>();

                //get all employeesTimeSchema
                Dictionary<int, List<EmployeeTimeScheduleTO>> EmplTimeSchemas = new EmployeesTimeSchedule().SearchEmployeesSchedulesDS(employeeString, startDate.Date, endDate.Date.AddDays(1));

                //get all time Schemas
                Dictionary<int, WorkTimeSchemaTO> dictTimeSchema = new TimeSchema().getDictionary();

                //get all working units 
                Dictionary<int, WorkingUnitTO> wuDict = new WorkingUnit().getWUDictionary();

                // get all employee types
                Dictionary<int, Dictionary<int, string>> employeeTypes = new EmployeeType().SearchDictionary();

                // get asco data
                Dictionary<int, EmployeeAsco4TO> emplAscoData = new EmployeeAsco4().SearchDictionary(employeeString.Trim());

                //get counter values 
                Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounterValues = new EmployeeCounterValue().SearchValues(employeeString);

                //rules dictionary first key is Working Unit ID, secund key is Employee Type ID, third key is ryle_Type; value is RuleTO
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDictionary = new Common.Rule().SearchWUEmplTypeDictionary();

                //dictionary for efective work key is working unit id value is list of pass types for effective work
                Dictionary<int, Dictionary<int, List<int>>> effectiveWorkDict = new Dictionary<int, Dictionary<int, List<int>>>();

                //dictionary for efective work key is working unit id value is list of pass types for effective work for lunch break
                Dictionary<int, Dictionary<int, List<int>>> effectiveLunchBreakWorkDict = new Dictionary<int, Dictionary<int, List<int>>>();

                //dictionary for efective work key is working unit id value is list of pass types for effective work for turnus
                Dictionary<int, Dictionary<int, List<int>>> effectiveRotaryWorkDict = new Dictionary<int, Dictionary<int, List<int>>>();

                //dictionary for types confirmation
                PassTypesConfirmation confirmation = new PassTypesConfirmation();
                Dictionary<int, List<int>> typesConfirmDict = confirmation.SearchDictionary();

                int expatOutType = -1;
                if (rulesDictionary.ContainsKey(Constants.defaultWorkingUnitID) && rulesDictionary[Constants.defaultWorkingUnitID].ContainsKey((int)Constants.EmployeeTypesFIAT.BC)
                    && rulesDictionary[Constants.defaultWorkingUnitID][(int)Constants.EmployeeTypesFIAT.BC].ContainsKey(Constants.RuleExpatOutType))
                    expatOutType = rulesDictionary[Constants.defaultWorkingUnitID][(int)Constants.EmployeeTypesFIAT.BC][Constants.RuleExpatOutType].RuleValue;

                //get types for effective works (use it for meal, transport and night work)
                foreach (int wuID in rulesDictionary.Keys)
                {
                    if (!effectiveWorkDict.ContainsKey(wuID))
                        effectiveWorkDict.Add(wuID, new Dictionary<int, List<int>>());
                    foreach (int emplTpe in rulesDictionary[wuID].Keys)
                    {
                        if (!effectiveWorkDict[wuID].ContainsKey(emplTpe))
                            effectiveWorkDict[wuID].Add(emplTpe, new List<int>());
                        foreach (string ruleName in Constants.effectiveWorkWageTypes())
                        {
                            if (rulesDictionary[wuID][emplTpe].ContainsKey(ruleName))
                            {
                                int value = rulesDictionary[wuID][emplTpe][ruleName].RuleValue;
                                if (!effectiveWorkDict[wuID][emplTpe].Contains(value))
                                {
                                    effectiveWorkDict[wuID][emplTpe].Add(value);
                                }
                            }
                        }

                        // add work on holiday for expats out as effective type
                        if (emplTpe == expatOutType)
                        {
                            if (rulesDictionary[wuID][emplTpe].ContainsKey(Constants.RuleWorkOnHolidayPassType))
                            {
                                int value = rulesDictionary[wuID][emplTpe][Constants.RuleWorkOnHolidayPassType].RuleValue;
                                if (!effectiveWorkDict[wuID][emplTpe].Contains(value))
                                {
                                    effectiveWorkDict[wuID][emplTpe].Add(value);
                                }
                            }
                        }
                    }
                }

                //get types for effective works (use it for turnus )
                foreach (int wuID in rulesDictionary.Keys)
                {
                    if (!effectiveRotaryWorkDict.ContainsKey(wuID))
                        effectiveRotaryWorkDict.Add(wuID, new Dictionary<int, List<int>>());
                    foreach (int emplTpe in rulesDictionary[wuID].Keys)
                    {
                        if (!effectiveRotaryWorkDict[wuID].ContainsKey(emplTpe))
                            effectiveRotaryWorkDict[wuID].Add(emplTpe, new List<int>());
                        foreach (string ruleName in Constants.effectiveWorkRotaryWageTypes())
                        {
                            if (rulesDictionary[wuID][emplTpe].ContainsKey(ruleName))
                            {
                                int value = rulesDictionary[wuID][emplTpe][ruleName].RuleValue;
                                if (!effectiveRotaryWorkDict[wuID][emplTpe].Contains(value))
                                {
                                    effectiveRotaryWorkDict[wuID][emplTpe].Add(value);
                                }
                            }
                        }
                    }
                }

                //get types for effective works (use it for lunch break )
                foreach (int wuID in rulesDictionary.Keys)
                {
                    if (!effectiveLunchBreakWorkDict.ContainsKey(wuID))
                        effectiveLunchBreakWorkDict.Add(wuID, new Dictionary<int, List<int>>());
                    foreach (int emplTpe in rulesDictionary[wuID].Keys)
                    {
                        if (!effectiveLunchBreakWorkDict[wuID].ContainsKey(emplTpe))
                            effectiveLunchBreakWorkDict[wuID].Add(emplTpe, new List<int>());
                        foreach (string ruleName in Constants.effectiveWorkLunchBreakWageTypes())
                        {
                            if (rulesDictionary[wuID][emplTpe].ContainsKey(ruleName))
                            {
                                int value = rulesDictionary[wuID][emplTpe][ruleName].RuleValue;
                                if (!effectiveLunchBreakWorkDict[wuID][emplTpe].Contains(value))
                                {
                                    effectiveLunchBreakWorkDict[wuID][emplTpe].Add(value);
                                }
                            }
                        }

                        // add work on holiday for expats out as effective type
                        if (emplTpe == expatOutType)
                        {
                            if (rulesDictionary[wuID][emplTpe].ContainsKey(Constants.RuleWorkOnHolidayPassType))
                            {
                                int value = rulesDictionary[wuID][emplTpe][Constants.RuleWorkOnHolidayPassType].RuleValue;
                                if (!effectiveLunchBreakWorkDict[wuID][emplTpe].Contains(value))
                                {
                                    effectiveLunchBreakWorkDict[wuID][emplTpe].Add(value);
                                }
                            }
                        }
                    }
                }

                //get last calcID and increase it for one
                uint calcID = new EmployeePYDataSum().getMaxCalcID() + 1;

                //bool unRegularDataFound = false;
                //int unregularEmplID = -1;
                string unregularMessage = "";
                //Dictionary<int, List<string>> unregularEmployees = new Dictionary<int, List<string>>();

                //get national and personal holidays
                List<DateTime> nationalHolidaysDays = new List<DateTime>();
                Dictionary<string, List<DateTime>> personalHolidayDays = new Dictionary<string, List<DateTime>>();
                List<DateTime> nationalHolidaysSundays = new List<DateTime>();
                List<HolidaysExtendedTO> nationalTransferableHolidays = new List<HolidaysExtendedTO>();

                Common.Misc.getHolidays(startDate.Date, endDate.Date, personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, null, nationalTransferableHolidays);

                //nationalHolidaysDays.AddRange(nationalHolidaysSundays);

                EmployeeType emplType = new EmployeeType();
                emplType.EmployeeTypeTO.EmployeeTypeName = Constants.emplTypeManager;
                List<EmployeeTypeTO> emplTypes = emplType.Search();
                int num = 0;

                Dictionary<int, Dictionary<string, RuleTO>> emplRulesEmployees = new Dictionary<int, Dictionary<string, RuleTO>>();

                // get sick leave confirmation types
                Common.Rule rule = new Common.Rule();
                rule.RuleTO.RuleType = Constants.RuleCompanySickLeaveNCF;
                List<RuleTO> rulesList = rule.Search();

                List<int> sickLeaveNCFTypes = new List<int>();
                List<int> sickLeaveConfirmationTypes = new List<int>();
                Dictionary<int, List<int>> confirmationTypes = new PassTypesConfirmation().SearchDictionary();

                foreach (RuleTO ruleTO in rulesList)
                {
                    if (!sickLeaveNCFTypes.Contains(ruleTO.RuleValue))
                    {
                        sickLeaveNCFTypes.Add(ruleTO.RuleValue);

                        if (confirmationTypes.ContainsKey(ruleTO.RuleValue))
                            sickLeaveConfirmationTypes.AddRange(confirmationTypes[ruleTO.RuleValue]);
                    }
                }

                // get stop working done types
                Common.Rule ruleSW = new Common.Rule();
                ruleSW.RuleTO.RuleType = Constants.RuleCompanyStopWorkingDone;
                List<RuleTO> rulesSWList = ruleSW.Search();

                List<int> swDoneTypes = new List<int>();
                foreach (RuleTO ruleTO in rulesSWList)
                {
                    if (!swDoneTypes.Contains(ruleTO.RuleValue))
                    {
                        swDoneTypes.Add(ruleTO.RuleValue);
                    }
                }

                // 24.07.2013. save overtime to justify, night overtime to justify hours and overtime paid and enter those values in estimation analytical records (becouse of Work Analysis reports)
                Dictionary<int, Dictionary<DateTime, Dictionary<string, decimal>>> emplOvertimeEstHours = new Dictionary<int, Dictionary<DateTime, Dictionary<string, decimal>>>();

                // get all bank hours balances before balance month
                DateTime balanceMonthBH = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(-bhBalanceMonth);
                Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>> emplBHBalances = new Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>>();
                if (bhPayRegular)
                    emplBHBalances = new EmployeeCounterMonthlyBalance().SearchEmployeeBalances(employeeString.Trim(), balanceMonthBH, (int)Constants.EmplCounterTypes.BankHoursCounter);

                // get all stop working balances before balance month
                DateTime balanceMonthSW = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(-swBalanceMonth);
                Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>> emplSWBalances = new Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>>();
                if (stopWorkingPay)
                    emplSWBalances = new EmployeeCounterMonthlyBalance().SearchEmployeeBalances(employeeString.Trim(), balanceMonthSW, (int)Constants.EmplCounterTypes.StopWorkingCounter);

                foreach (int employeeID in emplDict.Keys)
                {
                    EmployeeTO empl = emplDict[employeeID];

                    // save employee working_unit_id before changing it to company working unit id
                    int emplWUID = empl.WorkingUnitID;

                    try
                    {
                        num++;

                        if (!EmplTimeSchemas.ContainsKey(empl.EmployeeID) && FIATRegularEmployee(empl.EmployeeTypeID))
                        {
                            //log.writeLog("Misc.GeneratePYData() Time schema not found for employee_id = " + empl.EmployeeID);
                            continue;
                        }

                        //log.writeLog(num.ToString()+"-"+selectedEmployees.Count.ToString());
                        if (empl.EmployeeID == -1)
                            continue;

                        // get employee cost center
                        WorkingUnitTO emplCC = getFiatCostCenter(emplWUID, wuDict);

                        //set working unit
                        Dictionary<string, RuleTO> emplRules = new Dictionary<string, RuleTO>();
                        empl.WorkingUnitID = Common.Misc.getRootWorkingUnit(empl.WorkingUnitID, wuDict);

                        WorkingUnitTO company = new WorkingUnitTO();

                        if (wuDict.ContainsKey(empl.WorkingUnitID))
                            company = wuDict[empl.WorkingUnitID];

                        // get employee type
                        string employeeType = getEmplType(empl.WorkingUnitID, empl.EmployeeTypeID, employeeTypes);

                        //check if employee is manager
                        bool manager = false;
                        foreach (EmployeeTypeTO typeTO in emplTypes)
                        {
                            if (typeTO.WorkingUnitID == empl.WorkingUnitID && empl.EmployeeTypeID == typeTO.EmployeeTypeID)
                            {
                                manager = true;
                                break;
                            }
                        }

                        //get employee rules
                        if (rulesDictionary.ContainsKey(empl.WorkingUnitID) && rulesDictionary[empl.WorkingUnitID].ContainsKey(empl.EmployeeTypeID))
                        {
                            emplRules = rulesDictionary[empl.WorkingUnitID][empl.EmployeeTypeID];
                        }
                        emplRulesEmployees.Add(empl.EmployeeID, emplRules);
                        //get all types considare effective work
                        List<int> emplEffTypes = new List<int>();
                        if (effectiveWorkDict.ContainsKey(empl.WorkingUnitID) && effectiveWorkDict[empl.WorkingUnitID].ContainsKey(empl.EmployeeTypeID))
                        {
                            emplEffTypes = effectiveWorkDict[empl.WorkingUnitID][empl.EmployeeTypeID];
                        }

                        //get all types considare effective work for lunch break
                        List<int> emplEffTypesLunchBreak = new List<int>();
                        if (effectiveLunchBreakWorkDict.ContainsKey(empl.WorkingUnitID) && effectiveLunchBreakWorkDict[empl.WorkingUnitID].ContainsKey(empl.EmployeeTypeID))
                        {
                            emplEffTypesLunchBreak = effectiveLunchBreakWorkDict[empl.WorkingUnitID][empl.EmployeeTypeID];
                        }

                        //get all types considare effective work for rotary shift
                        List<int> emplEffTypesRotaryShift = new List<int>();
                        if (effectiveRotaryWorkDict.ContainsKey(empl.WorkingUnitID) && effectiveRotaryWorkDict[empl.WorkingUnitID].ContainsKey(empl.EmployeeTypeID))
                        {
                            emplEffTypesRotaryShift = effectiveRotaryWorkDict[empl.WorkingUnitID][empl.EmployeeTypeID];
                        }

                        EmployeeAsco4TO asco = new EmployeeAsco4TO();
                        if (emplAscoData.ContainsKey(empl.EmployeeID))
                            asco = emplAscoData[empl.EmployeeID];

                        DateTime date1 = startDate.Date;

                        //add buffer for employee
                        if (!employeeBuffersDict.ContainsKey(empl.EmployeeID))
                        {
                            employeeBuffersDict.Add(empl.EmployeeID, new EmployeePYDataBufferTO());
                            employeeBuffersDict[empl.EmployeeID].PYCalcID = calcID;
                            employeeBuffersDict[empl.EmployeeID].EmployeeType = employeeType.Trim();
                            employeeBuffersDict[empl.EmployeeID].CCName = emplCC.Name.Trim();
                            employeeBuffersDict[empl.EmployeeID].CCDesc = emplCC.Description.Trim();
                            employeeBuffersDict[empl.EmployeeID].LastName = empl.LastName;
                            employeeBuffersDict[empl.EmployeeID].FirstName = empl.FirstName;
                            employeeBuffersDict[empl.EmployeeID].Type = type;
                            employeeBuffersDict[empl.EmployeeID].CompanyCode = company.Code;
                            employeeBuffersDict[empl.EmployeeID].TransportCounter = 0;
                            employeeBuffersDict[empl.EmployeeID].MealCounter = 0;
                            employeeBuffersDict[empl.EmployeeID].VacationLeftCurrYear = 0;
                            employeeBuffersDict[empl.EmployeeID].VacationLeftPrevYear = 0;
                            employeeBuffersDict[empl.EmployeeID].VacationUsedCurrYear = 0;
                            employeeBuffersDict[empl.EmployeeID].ValactionUsedPrevYear = 0;
                            employeeBuffersDict[empl.EmployeeID].BankHrsBalans = 0;
                            employeeBuffersDict[empl.EmployeeID].PaidLeaveBalans = 0;
                            employeeBuffersDict[empl.EmployeeID].PaidLeaveUsed = 0;
                            employeeBuffersDict[empl.EmployeeID].StopWorkingHrsBalans = 0;
                            employeeBuffersDict[empl.EmployeeID].FundDay = 0;
                            employeeBuffersDict[empl.EmployeeID].FundHrs = 0;
                            employeeBuffersDict[empl.EmployeeID].FundDayEst = 0;
                            employeeBuffersDict[empl.EmployeeID].FundHrsEst = 0;
                            employeeBuffersDict[empl.EmployeeID].EmployeeID = empl.EmployeeID;
                            employeeBuffersDict[empl.EmployeeID].DateStart = startDate.Date;
                            employeeBuffersDict[empl.EmployeeID].DateEnd = endDate.Date;
                            employeeBuffersDict[empl.EmployeeID].NotJustifiedOvertime = 0;
                            employeeBuffersDict[empl.EmployeeID].NotJustifiedOvertimeBalans = 0;
                            employeeBuffersDict[empl.EmployeeID].ApprovedMeals = new Dictionary<int, int>();
                            employeeBuffersDict[empl.EmployeeID].NotApprovedMeals = new Dictionary<int, int>();
                        }

                        //if employee has default time schema as last time schema and it is before end of the period do not considare default schema 
                        //and calculate fund from that day until end of the period
                        if (FIATRegularEmployee(empl.EmployeeTypeID) && ((EmplTimeSchemas[empl.EmployeeID][EmplTimeSchemas[empl.EmployeeID].Count - 1].TimeSchemaID == Constants.defaultSchemaID
                            && EmplTimeSchemas[empl.EmployeeID][EmplTimeSchemas[empl.EmployeeID].Count - 1].Date.Date <= endDate.Date && EmplTimeSchemas[empl.EmployeeID][EmplTimeSchemas[empl.EmployeeID].Count - 1].Date.Date > startDate.Date)
                            || (asco.DatetimeValue3 != new DateTime() && asco.DatetimeValue3.Date <= endDate.Date)))
                        {
                            // do not get schedules after retired date
                            List<EmployeeTimeScheduleTO> estimatedList = new List<EmployeeTimeScheduleTO>();
                            foreach (EmployeeTimeScheduleTO TS in EmplTimeSchemas[empl.EmployeeID])
                            {
                                if (EmplTimeSchemas[empl.EmployeeID][EmplTimeSchemas[empl.EmployeeID].Count - 1].Date.Date != TS.Date && !asco.DatetimeValue3.Date.Equals(TS.Date)
                                    && TS.TimeSchemaID != Constants.defaultSchemaID)
                                    estimatedList.Add(TS);
                            }

                            DateTime estDate = EmplTimeSchemas[empl.EmployeeID][EmplTimeSchemas[empl.EmployeeID].Count - 1].Date.Date;
                            if (!asco.DatetimeValue3.Date.Equals(new DateTime()) && asco.DatetimeValue3.Date < estDate.Date)
                                estDate = asco.DatetimeValue3.Date;

                            while (estDate <= endDate.Date)
                            {
                                bool is2DayShift = false;
                                bool is2DayShiftPrevious = false;
                                WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();

                                WorkTimeSchemaTO workTimeSchema = null;

                                //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                                //are night shift days. If day is night shift day, also take first interval of next day
                                Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(estimatedList, estDate, ref is2DayShift,
                                    ref is2DayShiftPrevious, ref firstIntervalNextDay, ref workTimeSchema, dictTimeSchema);

                                List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                                TimeSpan intervalDuration = new TimeSpan();
                                if (edi != null && workTimeSchema != null)
                                {
                                    intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DayShiftPrevious, firstIntervalNextDay, edi);

                                    foreach (WorkTimeIntervalTO interval in intervals)
                                    {
                                        intervalDuration += interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay;
                                        //if shift is until 23:59 add one minute
                                        if (interval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                            intervalDuration = intervalDuration.Add(new TimeSpan(0, 1, 0));
                                    }
                                }

                                //add one day in fund day and hours in fund hrs if employee need to work
                                if (intervalDuration.TotalMinutes > 0)
                                {
                                    employeeBuffersDict[empl.EmployeeID].FundDayEst++;
                                    employeeBuffersDict[empl.EmployeeID].FundHrsEst += (int)intervalDuration.TotalMinutes / 60;
                                }

                                estDate = estDate.AddDays(1);
                            }
                        }

                        if (FIATRegularEmployee(empl.EmployeeTypeID))
                        {
                            //if employee has time schema after begining of the period
                            foreach (EmployeeTimeScheduleTO TS in EmplTimeSchemas[empl.EmployeeID])
                            {
                                if (TS.TimeSchemaID != Constants.defaultSchemaID)
                                {
                                    if (TS.Date.Date > startDate.Date)
                                    {
                                        List<EmployeeTimeScheduleTO> estimatedList = new List<EmployeeTimeScheduleTO>();

                                        TimeSpan ts = new TimeSpan(TS.Date.Date.Ticks - startDate.Date.AddDays(-1).Ticks);
                                        EmployeeTimeScheduleTO newTS = new EmployeeTimeScheduleTO();
                                        newTS.TimeSchemaID = TS.TimeSchemaID;
                                        newTS.Date = startDate.Date.AddDays(-1);
                                        newTS.EmployeeID = TS.EmployeeID;
                                        if (TS.StartCycleDay >= ts.TotalDays)
                                        {
                                            newTS.StartCycleDay = TS.StartCycleDay - (int)ts.TotalDays;
                                        }
                                        else
                                        {
                                            WorkTimeSchemaTO actualTimeSchema = null;

                                            if (dictTimeSchema.ContainsKey(TS.TimeSchemaID))
                                            {
                                                actualTimeSchema = dictTimeSchema[TS.TimeSchemaID];
                                            }
                                            int cycleDuration = actualTimeSchema.CycleDuration;

                                            TimeSpan ts1 = new TimeSpan(TS.Date.Date.AddDays(-TS.StartCycleDay).Ticks - startDate.Date.AddDays(-1).Ticks);
                                            newTS.StartCycleDay = cycleDuration - ((int)ts1.TotalDays % cycleDuration);
                                        }

                                        estimatedList.Add(newTS);
                                        foreach (EmployeeTimeScheduleTO TS1 in EmplTimeSchemas[empl.EmployeeID])
                                        {
                                            if (TS1.TimeSchemaID != Constants.defaultSchemaID || estimatedList.Count > 1)
                                            {
                                                estimatedList.Add(TS1);
                                            }
                                        }

                                        DateTime estDate = startDate.Date;
                                        while (estDate < TS.Date.Date)
                                        {
                                            bool is2DayShift = false;
                                            bool is2DayShiftPrevious = false;
                                            WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();

                                            WorkTimeSchemaTO workTimeSchema = null;

                                            //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                                            //are night shift days. If day is night shift day, also take first interval of next day
                                            Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(estimatedList, estDate, ref is2DayShift,
                                                ref is2DayShiftPrevious, ref firstIntervalNextDay, ref workTimeSchema, dictTimeSchema);

                                            List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                                            TimeSpan intervalDuration = new TimeSpan();
                                            if (edi != null && workTimeSchema != null)
                                            {
                                                intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DayShiftPrevious, firstIntervalNextDay, edi);

                                                foreach (WorkTimeIntervalTO interval in intervals)
                                                {
                                                    intervalDuration += interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay;
                                                    //if shift is until 23:59 add one minute
                                                    if (interval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                        intervalDuration = intervalDuration.Add(new TimeSpan(0, 1, 0));
                                                }
                                            }

                                            //add one day in fund day and hours in fund hrs if employee need to work
                                            if (intervalDuration.TotalMinutes > 0)
                                            {
                                                employeeBuffersDict[empl.EmployeeID].FundDayEst++;
                                                employeeBuffersDict[empl.EmployeeID].FundHrsEst += (int)intervalDuration.TotalMinutes / 60;
                                            }
                                            estDate = estDate.AddDays(1);
                                        }
                                    }
                                    break;
                                }
                            }
                        }

                        int paidLeaveUsed = 0;
                        int vacationUsed = 0;

                        List<DateTime> paidHolidaysDays = new List<DateTime>();
                        int overtimeToKeep = 0;
                        if (FIATRegularEmployee(empl.EmployeeTypeID))
                        {
                            while (date1 <= endDate.Date)
                            {
                                bool is2DayShift = false;
                                bool is2DayShiftPrevious = false;
                                WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();

                                WorkTimeSchemaTO workTimeSchema = null;

                                //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                                //are night shift days. If day is night shift day, also take first interval of next day
                                Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(EmplTimeSchemas[empl.EmployeeID], date1, ref is2DayShift,
                                    ref is2DayShiftPrevious, ref firstIntervalNextDay, ref workTimeSchema, dictTimeSchema);

                                List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                                TimeSpan intervalDuration = new TimeSpan();
                                if (edi != null && workTimeSchema != null)
                                {
                                    intervals = Common.Misc.getEmployeeDayIntervals(is2DayShift, is2DayShiftPrevious, firstIntervalNextDay, edi);

                                    foreach (WorkTimeIntervalTO interval in intervals)
                                    {
                                        intervalDuration += interval.EndTime.TimeOfDay - interval.StartTime.TimeOfDay;
                                        //if shift is until 23:59 add one minute
                                        if (interval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                            intervalDuration = intervalDuration.Add(new TimeSpan(0, 1, 0));
                                    }
                                }

                                //add one day in fund day and hours in fund hrs if employee need to work
                                if (intervalDuration.TotalMinutes > 0)
                                {
                                    employeeBuffersDict[empl.EmployeeID].FundDay++;
                                    employeeBuffersDict[empl.EmployeeID].FundHrs += (int)intervalDuration.TotalMinutes / 60;
                                    employeeBuffersDict[empl.EmployeeID].FundDayEst++;
                                    employeeBuffersDict[empl.EmployeeID].FundHrsEst += (int)intervalDuration.TotalMinutes / 60;
                                }

                                int transport = 0;
                                int transportOutWS = 0;
                                TimeSpan dayPresenceDuration = new TimeSpan();
                                TimeSpan dayPresenceDurationLunchBreak = new TimeSpan();

                                if (processedPairsDict.ContainsKey(empl.EmployeeID) && processedPairsDict[empl.EmployeeID].ContainsKey(date1))
                                {
                                    List<IOPairProcessedTO> processedDayPairs = processedPairsDict[empl.EmployeeID][date1];
                                    List<IOPairProcessedTO> processedDayPairsForShiftDay = new List<IOPairProcessedTO>();

                                    //if day before is 2DayShift and pair start before noon do not calculate pair for this day
                                    foreach (IOPairProcessedTO processedTO in processedDayPairs)
                                    {
                                        if (is2DayShiftPrevious && processedTO.StartTime.TimeOfDay < new TimeSpan(12, 0, 0))
                                            continue;
                                        else
                                            processedDayPairsForShiftDay.Add(processedTO);
                                    }

                                    //if shift is for two days add pairs from next day which start before noon
                                    if (is2DayShift)
                                    {
                                        if (processedPairsDict[empl.EmployeeID].ContainsKey(date1.AddDays(1)))
                                        {
                                            foreach (IOPairProcessedTO processedTO in processedPairsDict[empl.EmployeeID][date1.AddDays(1)])
                                            {
                                                if (processedTO.StartTime.TimeOfDay < new TimeSpan(12, 0, 0))
                                                {
                                                    processedDayPairsForShiftDay.Add(processedTO);
                                                }
                                            }
                                        }
                                    }

                                    int i = 0;
                                    //counters for empl buffers for paid leave and vacation, we counter monthly
                                    bool paidLeaveCount = true;
                                    bool vacationCount = true;
                                    int bankHoursUsedGap = 0;

                                    //go throw all pairs for this day and count pairs duration
                                    foreach (IOPairProcessedTO processedTO in processedDayPairsForShiftDay)
                                    {
                                        DateTime startSicknessDate = Common.Misc.createDate(processedTO.Desc.Trim());

                                        if (!sickLeaveConfirmationTypes.Contains(processedTO.PassTypeID))
                                            startSicknessDate = new DateTime();

                                        if (startSicknessDate.Equals(new DateTime()))
                                            startSicknessDate = defaultSicknessStart;

                                        TimeSpan pairDuration = new TimeSpan();

                                        DateTime currentDate = date1;

                                        //check if pair is unregular pair (unjustified, unverified, uncofirmed)
                                        if (processedTO.ConfirmationFlag == (int)Constants.Confirmation.NotConfirmed ||
                                            processedTO.PassTypeID == Constants.absence
                                            || (emplRules.ContainsKey(Constants.RuleCompanyInitialOvertime) && processedTO.PassTypeID == emplRules[Constants.RuleCompanyInitialOvertime].RuleValue)
                                            || (emplRules.ContainsKey(Constants.RuleCompanyInitialNightOvertime) && processedTO.PassTypeID == emplRules[Constants.RuleCompanyInitialNightOvertime].RuleValue))
                                        {
                                            //unjustified pairs for managers change type to managers filling time
                                            if (type == Constants.PYTypeReal && processedTO.PassTypeID == Constants.absence)
                                            {
                                                if (manager)
                                                    processedTO.PassTypeID = emplRules[Constants.RuleCompanyManagers].RuleValue;
                                            }
                                            else if (type == Constants.PYTypeReal && emplRules.ContainsKey(Constants.RuleCompanyInitialOvertime) && processedTO.PassTypeID == emplRules[Constants.RuleCompanyInitialOvertime].RuleValue)
                                            {
                                                if (emplDict.ContainsKey(processedTO.EmployeeID) && (emplDict[processedTO.EmployeeID].WorkingUnitID == (int)Constants.FiatCompanies.FAS
                                                    || emplDict[processedTO.EmployeeID].WorkingUnitID == (int)Constants.FiatCompanies.MMdoo
                                                    || emplDict[processedTO.EmployeeID].WorkingUnitID == (int)Constants.FiatCompanies.MMauto)
                                                    && emplDict[processedTO.EmployeeID].EmployeeTypeID == (int)Constants.EmployeeTypesFIAT.BC)
                                                {
                                                    overtimeToKeep += (int)processedTO.EndTime.Subtract(processedTO.StartTime).TotalMinutes;

                                                    if (processedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                        overtimeToKeep++;
                                                }

                                                continue;
                                            }
                                            else if (type == Constants.PYTypeReal && emplRules.ContainsKey(Constants.RuleCompanyInitialNightOvertime) && processedTO.PassTypeID == emplRules[Constants.RuleCompanyInitialNightOvertime].RuleValue)
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                unRegularDataFound = true;
                                                //unregularEmplID = empl.EmployeeID;
                                                unregularMessage = "unconfirmedData";
                                                if (!unregularEmployees.ContainsKey(empl.EmployeeID))
                                                    unregularEmployees.Add(empl.EmployeeID, new List<string>());
                                                if (!unregularEmployees[empl.EmployeeID].Contains(unregularMessage))
                                                    unregularEmployees[empl.EmployeeID].Add(unregularMessage);

                                                if (type == Constants.PYTypeReal)
                                                {
                                                    continue;
                                                }
                                                else if (type == Constants.PYTypeEstimated)
                                                {
                                                    //if employee has unjustified pair for estimated report count hours as regular work
                                                    //******************** 02.07.2013. Sanja leave absence
                                                    //if (processedTO.PassTypeID == Constants.absence)
                                                    //    processedTO.PassTypeID = emplRules[Constants.RuleCompanyRegularWork].RuleValue;
                                                    // else
                                                    //******************** 02.07.2013. Sanja leave absence

                                                    //if employee has initial overtime pair for estimated report count hours as over time paid
                                                    if (processedTO.PassTypeID == emplRules[Constants.RuleCompanyInitialOvertime].RuleValue)
                                                    {
                                                        processedTO.OldPassTypeID = processedTO.PassTypeID;
                                                        processedTO.PassTypeID = emplRules[Constants.RuleCompanyOvertimePaid].RuleValue;
                                                    }

                                                    //if employee has initial night overtime pair for estimated report count hours as over time paid
                                                    else if (emplRules.ContainsKey(Constants.RuleCompanyInitialNightOvertime) && processedTO.PassTypeID == emplRules[Constants.RuleCompanyInitialNightOvertime].RuleValue)
                                                    {
                                                        processedTO.OldPassTypeID = processedTO.PassTypeID;
                                                        processedTO.PassTypeID = emplRules[Constants.RuleCompanyOvertimePaid].RuleValue;
                                                    }

                                                    //if employee has sick leave ncf pair for estimated report count hours as sick leave 30 days
                                                    else if (processedTO.PassTypeID == emplRules[Constants.RuleCompanySickLeaveNCF].RuleValue)
                                                        processedTO.PassTypeID = emplRules[Constants.RuleCompanySickLeave30Days].RuleValue;

                                                    //if pass type need to be confirmed change it with confirmation type (take the first one)
                                                    else if (typesConfirmDict.ContainsKey(processedTO.PassTypeID))
                                                    {
                                                        processedTO.PassTypeID = typesConfirmDict[processedTO.PassTypeID][0];
                                                    }
                                                }
                                            }
                                        }

                                        //count bank hours monthly
                                        if (emplRules.ContainsKey(Constants.RuleCompanyBankHour) && emplRules.ContainsKey(Constants.RuleCompanyBankHourMonthly))
                                        {
                                            if (processedTO.PassTypeID == emplRules[Constants.RuleCompanyBankHour].RuleValue)
                                                processedTO.PassTypeID = emplRules[Constants.RuleCompanyBankHourMonthly].RuleValue;
                                        }
                                        //count stop working monthly
                                        if (emplRules.ContainsKey(Constants.RuleCompanyStopWorking) && emplRules.ContainsKey(Constants.RuleCompanyStopWorkingMonthly))
                                        {
                                            if (processedTO.PassTypeID == emplRules[Constants.RuleCompanyStopWorking].RuleValue)
                                                processedTO.PassTypeID = emplRules[Constants.RuleCompanyStopWorkingMonthly].RuleValue;
                                        }

                                        //if (processedTO.PassTypeID == Constants.absence)
                                        //    continue;

                                        //count just one paid leave for one day
                                        if (paidLeaveCount)
                                        {
                                            //paid leave take types from pass_types 
                                            if (typesDict[processedTO.PassTypeID].LimitCompositeID != -1)
                                            {
                                                paidLeaveUsed++;
                                                paidLeaveCount = false;
                                            }
                                        }

                                        //count just one vacation for one day
                                        if (vacationCount)
                                        {
                                            if ((emplRules.ContainsKey(Constants.RuleCompanyAnnualLeave) && emplRules[Constants.RuleCompanyAnnualLeave].RuleValue == processedTO.PassTypeID)
                                                || (emplRules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && emplRules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue == processedTO.PassTypeID))
                                            {
                                                vacationUsed++;
                                                vacationCount = false;
                                                //if shift is more than 8 hours long and if employee has whole week annual leave 
                                                //increase counter once more
                                                if (workTimeSchema != null && Common.Misc.is10HourShift(workTimeSchema) && processedTO.IOPairDate.DayOfWeek == DayOfWeek.Thursday)
                                                {
                                                    // get week annual leave pairs
                                                    int annualLeaveDays = 0;
                                                    IOPairProcessed annualPair = new IOPairProcessed();
                                                    List<IOPairProcessedTO> annualLeaveWeekPairs = annualPair.SearchWeekPairs(processedTO.EmployeeID, processedTO.IOPairDate, false, Common.Misc.getAnnualLeaveTypesString(emplRules), null);

                                                    foreach (IOPairProcessedTO aPair in annualLeaveWeekPairs)
                                                    {
                                                        // second night shift belongs to previous day
                                                        if (aPair.StartTime.Hour == 0 && aPair.StartTime.Minute == 0)
                                                            continue;

                                                        annualLeaveDays++;
                                                    }

                                                    // if fourts day of week is changed, subtract/add two days
                                                    if (annualLeaveDays == (Constants.fullWeek10hShift - 1))
                                                        vacationUsed++;
                                                }
                                            }
                                        }

                                        // add employee analitical to fill data in 
                                        if (!employeeAnaliticalDict.ContainsKey(empl.EmployeeID))
                                        {
                                            employeeAnaliticalDict.Add(empl.EmployeeID, new Dictionary<DateTime, Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>>());
                                        }
                                        if (!employeeAnaliticalDict[empl.EmployeeID].ContainsKey(currentDate))
                                        {
                                            employeeAnaliticalDict[empl.EmployeeID].Add(currentDate, new Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>());
                                        }
                                        if (!employeeAnaliticalDict[empl.EmployeeID][currentDate].ContainsKey(typesDict[processedTO.PassTypeID].PaymentCode))
                                        {
                                            employeeAnaliticalDict[empl.EmployeeID][currentDate].Add(typesDict[processedTO.PassTypeID].PaymentCode, new Dictionary<DateTime, EmployeePYDataAnaliticalTO>());
                                        }
                                        if (!employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[processedTO.PassTypeID].PaymentCode].ContainsKey(startSicknessDate))
                                        {
                                            employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[processedTO.PassTypeID].PaymentCode].Add(startSicknessDate, new EmployeePYDataAnaliticalTO());
                                            employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[processedTO.PassTypeID].PaymentCode][startSicknessDate].PYCalcID = calcID;
                                            employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[processedTO.PassTypeID].PaymentCode][startSicknessDate].EmployeeType = employeeType.Trim();
                                            employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[processedTO.PassTypeID].PaymentCode][startSicknessDate].CCName = emplCC.Name.Trim();
                                            employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[processedTO.PassTypeID].PaymentCode][startSicknessDate].CCDesc = emplCC.Description.Trim();
                                            employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[processedTO.PassTypeID].PaymentCode][startSicknessDate].DateStartSickness = startSicknessDate;
                                            employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[processedTO.PassTypeID].PaymentCode][startSicknessDate].PaymentCode = typesDict[processedTO.PassTypeID].PaymentCode;
                                            employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[processedTO.PassTypeID].PaymentCode][startSicknessDate].EmployeeID = empl.EmployeeID;
                                            employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[processedTO.PassTypeID].PaymentCode][startSicknessDate].CompanyCode = company.Code;
                                            employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[processedTO.PassTypeID].PaymentCode][startSicknessDate].Date = currentDate;
                                            employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[processedTO.PassTypeID].PaymentCode][startSicknessDate].Type = type;
                                            employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[processedTO.PassTypeID].PaymentCode][startSicknessDate].HrsAmount = 0;
                                        }

                                        //if pass type is overtime count day presence for meal duration, 
                                        //count night work if it fit rules for night rules, 
                                        //turnus for people with industrial time schema on not working day -- zakomentarisano 2012.06.12. NATASA                             
                                        if (processedTO.PassTypeID == emplRules[Constants.RuleCompanyOvertimePaid].RuleValue
                                            || processedTO.PassTypeID == emplRules[Constants.RuleCompanyBankHourMonthly].RuleValue || swDoneTypes.Contains(processedTO.PassTypeID))
                                        {
                                            // if start time is midnight, check if there is overtime / bank hour pair in previous day ending minimal minutes before midnight
                                            // if pair exists, do not count transport
                                            bool prevOvertimeFound = false;
                                            if (processedTO.StartTime.Hour == 0 && processedTO.StartTime.Minute == 0 && processedPairsDict[empl.EmployeeID].ContainsKey(date1.AddDays(-1).Date))
                                            {
                                                // get max rounding value
                                                int roundingMinutes = -1;
                                                if (emplRules.ContainsKey(Constants.RuleOvertimeRounding) && emplRules[Constants.RuleOvertimeRounding].RuleValue > roundingMinutes)
                                                    roundingMinutes = emplRules[Constants.RuleOvertimeRounding].RuleValue;
                                                if (emplRules.ContainsKey(Constants.RuleOvertimeRoundingOutWS) && emplRules[Constants.RuleOvertimeRoundingOutWS].RuleValue > roundingMinutes)
                                                    roundingMinutes = emplRules[Constants.RuleOvertimeRoundingOutWS].RuleValue;

                                                foreach (IOPairProcessedTO prevPair in processedPairsDict[empl.EmployeeID][date1.AddDays(-1).Date])
                                                {
                                                    if ((prevPair.PassTypeID == emplRules[Constants.RuleCompanyOvertimePaid].RuleValue || prevPair.PassTypeID == emplRules[Constants.RuleCompanyBankHourMonthly].RuleValue
                                                        || swDoneTypes.Contains(prevPair.PassTypeID))
                                                        && (int)date1.Date.Subtract(prevPair.EndTime).TotalMinutes <= roundingMinutes)
                                                    {
                                                        prevOvertimeFound = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (intervalDuration.TotalMinutes == 0)
                                            {
                                                if (!prevOvertimeFound)
                                                    transportOutWS = 1;

                                                if (!swDoneTypes.Contains(processedTO.PassTypeID))
                                                {
                                                    //count presence duration for meal
                                                    dayPresenceDuration += processedTO.EndTime.TimeOfDay - processedTO.StartTime.TimeOfDay;
                                                    //if shift is until 23:59 add one minute
                                                    if (processedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                        dayPresenceDuration = dayPresenceDuration.Add(new TimeSpan(0, 1, 0));
                                                }
                                            }
                                            else
                                            {
                                                bool connected = false;
                                                foreach (IOPairProcessedTO procTO in processedDayPairsForShiftDay)
                                                {
                                                    if (procTO.StartTime == processedTO.EndTime || processedTO.StartTime == procTO.EndTime)
                                                    {
                                                        connected = true;
                                                        break;
                                                    }
                                                }

                                                if (!connected && !prevOvertimeFound)
                                                    transportOutWS++;
                                            }

                                            //add night work if it corespond to rules for night work
                                            if (emplRules.ContainsKey(Constants.RuleNightWork) && (processedTO.EndTime.TimeOfDay > emplRules[Constants.RuleNightWork].RuleDateTime1.TimeOfDay
                                                || processedTO.StartTime.TimeOfDay < emplRules[Constants.RuleNightWork].RuleDateTime2.TimeOfDay))
                                            {
                                                int nightWork = emplRules[Constants.RuleNightWork].RuleValue;

                                                if (!employeeAnaliticalDict.ContainsKey(empl.EmployeeID))
                                                {
                                                    employeeAnaliticalDict.Add(empl.EmployeeID, new Dictionary<DateTime, Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>>());
                                                }
                                                if (!employeeAnaliticalDict[empl.EmployeeID].ContainsKey(currentDate))
                                                {
                                                    employeeAnaliticalDict[empl.EmployeeID].Add(currentDate, new Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>());
                                                }
                                                if (!employeeAnaliticalDict[empl.EmployeeID][currentDate].ContainsKey(typesDict[nightWork].PaymentCode))
                                                {
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate].Add(typesDict[nightWork].PaymentCode, new Dictionary<DateTime, EmployeePYDataAnaliticalTO>());
                                                }
                                                if (!employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode].ContainsKey(startSicknessDate))
                                                {
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode].Add(startSicknessDate, new EmployeePYDataAnaliticalTO());
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].PYCalcID = calcID;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].CCDesc = emplCC.Description;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].EmployeeType = employeeType.Trim();
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].CCName = emplCC.Name.Trim();
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].DateStartSickness = startSicknessDate;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].PaymentCode = typesDict[nightWork].PaymentCode;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].EmployeeID = empl.EmployeeID;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].CompanyCode = company.Code;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].Date = currentDate;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].Type = type;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].HrsAmount = 0;
                                                }
                                                if (processedTO.EndTime.TimeOfDay > emplRules[Constants.RuleNightWork].RuleDateTime1.TimeOfDay)
                                                {
                                                    if (processedTO.StartTime.TimeOfDay < emplRules[Constants.RuleNightWork].RuleDateTime1.TimeOfDay)
                                                        pairDuration = processedTO.EndTime.TimeOfDay - emplRules[Constants.RuleNightWork].RuleDateTime1.TimeOfDay;
                                                    else
                                                    {
                                                        pairDuration = processedTO.EndTime.TimeOfDay - processedTO.StartTime.TimeOfDay;
                                                    }
                                                }
                                                else
                                                {
                                                    if (processedTO.EndTime.TimeOfDay > emplRules[Constants.RuleNightWork].RuleDateTime2.TimeOfDay)
                                                    {
                                                        pairDuration = emplRules[Constants.RuleNightWork].RuleDateTime2.TimeOfDay - processedTO.StartTime.TimeOfDay;
                                                    }
                                                    else
                                                    {
                                                        pairDuration = processedTO.EndTime.TimeOfDay - processedTO.StartTime.TimeOfDay;
                                                    }
                                                }
                                                //if shift is until 23:59 add one minute
                                                if (processedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                    pairDuration = pairDuration.Add(new TimeSpan(0, 1, 0));

                                                employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].HrsAmount += (decimal)pairDuration.TotalMinutes / 60;
                                            }
                                        }

                                        //if pass_type is efective work check if we should calculate transport, night work 
                                        if (emplEffTypes.Contains(processedTO.PassTypeID) && !swDoneTypes.Contains(processedTO.PassTypeID))
                                        {
                                            dayPresenceDuration += processedTO.EndTime.TimeOfDay - processedTO.StartTime.TimeOfDay;
                                            //if shift is until 23:59 add one minute
                                            if (processedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                dayPresenceDuration = dayPresenceDuration.Add(new TimeSpan(0, 1, 0));

                                            if (transport == 0)
                                                transport = 1;

                                            //add night work if it corespond to rules for night work
                                            if (emplRules.ContainsKey(Constants.RuleNightWork) && (processedTO.EndTime.TimeOfDay > emplRules[Constants.RuleNightWork].RuleDateTime1.TimeOfDay
                                                || processedTO.StartTime.TimeOfDay < emplRules[Constants.RuleNightWork].RuleDateTime2.TimeOfDay))
                                            {
                                                int nightWork = emplRules[Constants.RuleNightWork].RuleValue;

                                                if (!employeeAnaliticalDict.ContainsKey(empl.EmployeeID))
                                                {
                                                    employeeAnaliticalDict.Add(empl.EmployeeID, new Dictionary<DateTime, Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>>());
                                                }
                                                if (!employeeAnaliticalDict[empl.EmployeeID].ContainsKey(currentDate))
                                                {
                                                    employeeAnaliticalDict[empl.EmployeeID].Add(currentDate, new Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>());
                                                }
                                                if (!employeeAnaliticalDict[empl.EmployeeID][currentDate].ContainsKey(typesDict[nightWork].PaymentCode))
                                                {
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate].Add(typesDict[nightWork].PaymentCode, new Dictionary<DateTime, EmployeePYDataAnaliticalTO>());
                                                }
                                                if (!employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode].ContainsKey(startSicknessDate))
                                                {
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode].Add(startSicknessDate, new EmployeePYDataAnaliticalTO());
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].PYCalcID = calcID;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].CCDesc = emplCC.Description;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].EmployeeType = employeeType.Trim();
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].CCName = emplCC.Name.Trim();
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].DateStartSickness = startSicknessDate;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].PaymentCode = typesDict[nightWork].PaymentCode;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].EmployeeID = empl.EmployeeID;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].CompanyCode = company.Code;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].Date = currentDate;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].Type = type;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].HrsAmount = 0;
                                                }
                                                if (processedTO.EndTime.TimeOfDay > emplRules[Constants.RuleNightWork].RuleDateTime1.TimeOfDay)
                                                {
                                                    if (processedTO.StartTime.TimeOfDay < emplRules[Constants.RuleNightWork].RuleDateTime1.TimeOfDay)
                                                        pairDuration = processedTO.EndTime.TimeOfDay - emplRules[Constants.RuleNightWork].RuleDateTime1.TimeOfDay;
                                                    else
                                                    {
                                                        pairDuration = processedTO.EndTime.TimeOfDay - processedTO.StartTime.TimeOfDay;
                                                    }
                                                }
                                                else
                                                {
                                                    if (processedTO.EndTime.TimeOfDay > emplRules[Constants.RuleNightWork].RuleDateTime2.TimeOfDay)
                                                    {
                                                        pairDuration = emplRules[Constants.RuleNightWork].RuleDateTime2.TimeOfDay - processedTO.StartTime.TimeOfDay;
                                                    }
                                                    else
                                                    {
                                                        pairDuration = processedTO.EndTime.TimeOfDay - processedTO.StartTime.TimeOfDay;
                                                    }
                                                }
                                                //if shift is until 23:59 add one minute
                                                if (processedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                    pairDuration = pairDuration.Add(new TimeSpan(0, 1, 0));

                                                employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[nightWork].PaymentCode][startSicknessDate].HrsAmount += (decimal)pairDuration.TotalMinutes / 60;
                                            }
                                        }

                                        // check work on holiday for effective types, overtime paid, bank hours (already changed to monthly type) and official trip
                                        if ((emplEffTypes.Contains(processedTO.PassTypeID) || (emplRules.ContainsKey(Constants.RuleCompanyOfficialTrip)
                                            && processedTO.PassTypeID == emplRules[Constants.RuleCompanyOfficialTrip].RuleValue)
                                            || (emplRules.ContainsKey(Constants.RuleCompanyOvertimePaid)
                                            && processedTO.PassTypeID == emplRules[Constants.RuleCompanyOvertimePaid].RuleValue)
                                            || (emplRules.ContainsKey(Constants.RuleCompanyBankHourMonthly)
                                            && processedTO.PassTypeID == emplRules[Constants.RuleCompanyBankHourMonthly].RuleValue))
                                            && (isWorkOnHolidayFiat(personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, nationalTransferableHolidays, paidHolidaysDays, currentDate,
                                                workTimeSchema, asco, empl, rulesDictionary)))
                                        {
                                            if (!emplRules.ContainsKey(Constants.RuleWorkOnHolidayPassType))
                                            {
                                                //log.writeLog("Misc.GenerateFiatPYData() Can not found RuleWorkOnHolidayPassType for employee_id = " + empl.EmployeeID.ToString());
                                                continue;
                                            }

                                            int workOnHoliday = emplRules[Constants.RuleWorkOnHolidayPassType].RuleValue;

                                            if (!employeeAnaliticalDict.ContainsKey(empl.EmployeeID))
                                            {
                                                employeeAnaliticalDict.Add(empl.EmployeeID, new Dictionary<DateTime, Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>>());
                                            }
                                            if (!employeeAnaliticalDict[empl.EmployeeID].ContainsKey(currentDate))
                                            {
                                                employeeAnaliticalDict[empl.EmployeeID].Add(currentDate, new Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>());
                                            }
                                            if (!employeeAnaliticalDict[empl.EmployeeID][currentDate].ContainsKey(typesDict[workOnHoliday].PaymentCode))
                                            {
                                                employeeAnaliticalDict[empl.EmployeeID][currentDate].Add(typesDict[workOnHoliday].PaymentCode, new Dictionary<DateTime, EmployeePYDataAnaliticalTO>());
                                            }
                                            if (!employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[workOnHoliday].PaymentCode].ContainsKey(startSicknessDate))
                                            {
                                                employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[workOnHoliday].PaymentCode].Add(startSicknessDate, new EmployeePYDataAnaliticalTO());
                                                employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[workOnHoliday].PaymentCode][startSicknessDate].PYCalcID = calcID;
                                                employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[workOnHoliday].PaymentCode][startSicknessDate].PaymentCode = typesDict[workOnHoliday].PaymentCode;
                                                employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[workOnHoliday].PaymentCode][startSicknessDate].EmployeeID = empl.EmployeeID;
                                                employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[workOnHoliday].PaymentCode][startSicknessDate].CCDesc = emplCC.Description;
                                                employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[workOnHoliday].PaymentCode][startSicknessDate].DateStartSickness = startSicknessDate;
                                                employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[workOnHoliday].PaymentCode][startSicknessDate].EmployeeType = employeeType.Trim();
                                                employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[workOnHoliday].PaymentCode][startSicknessDate].CCName = emplCC.Name.Trim();
                                                employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[workOnHoliday].PaymentCode][startSicknessDate].CompanyCode = company.Code;
                                                employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[workOnHoliday].PaymentCode][startSicknessDate].Date = currentDate;
                                                employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[workOnHoliday].PaymentCode][startSicknessDate].Type = type;
                                                employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[workOnHoliday].PaymentCode][startSicknessDate].HrsAmount = 0;
                                            }

                                            pairDuration = processedTO.EndTime.TimeOfDay - processedTO.StartTime.TimeOfDay;
                                            //if shift is until 23:59 add one minute
                                            if (processedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                pairDuration = pairDuration.Add(new TimeSpan(0, 1, 0));

                                            employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[workOnHoliday].PaymentCode][startSicknessDate].HrsAmount += (decimal)pairDuration.TotalMinutes / 60;
                                        }

                                        //count for lunch break
                                        if (emplEffTypesLunchBreak.Contains(processedTO.PassTypeID))
                                        {
                                            //count presence duration for meal
                                            dayPresenceDurationLunchBreak += processedTO.EndTime.TimeOfDay - processedTO.StartTime.TimeOfDay;
                                            //if shift is until 23:59 add one minute
                                            if (processedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                dayPresenceDurationLunchBreak = dayPresenceDurationLunchBreak.Add(new TimeSpan(0, 1, 0));
                                        }

                                        //add turnus hours
                                        if ((emplEffTypesRotaryShift.Contains(processedTO.PassTypeID)))
                                        {
                                            if (workTimeSchema.Turnus == Constants.yesInt)
                                            {
                                                int rotaryShift = emplRules[Constants.RuleComanyRotaryShift].RuleValue;

                                                if (!employeeAnaliticalDict.ContainsKey(empl.EmployeeID))
                                                {
                                                    employeeAnaliticalDict.Add(empl.EmployeeID, new Dictionary<DateTime, Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>>());
                                                }
                                                if (!employeeAnaliticalDict[empl.EmployeeID].ContainsKey(currentDate))
                                                {
                                                    employeeAnaliticalDict[empl.EmployeeID].Add(currentDate, new Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>());
                                                }
                                                if (!employeeAnaliticalDict[empl.EmployeeID][currentDate].ContainsKey(typesDict[rotaryShift].PaymentCode))
                                                {
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate].Add(typesDict[rotaryShift].PaymentCode, new Dictionary<DateTime, EmployeePYDataAnaliticalTO>());
                                                }
                                                if (!employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[rotaryShift].PaymentCode].ContainsKey(startSicknessDate))
                                                {
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[rotaryShift].PaymentCode].Add(startSicknessDate, new EmployeePYDataAnaliticalTO());
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[rotaryShift].PaymentCode][startSicknessDate].PYCalcID = calcID;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[rotaryShift].PaymentCode][startSicknessDate].CCDesc = emplCC.Description;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[rotaryShift].PaymentCode][startSicknessDate].EmployeeType = employeeType.Trim();
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[rotaryShift].PaymentCode][startSicknessDate].CCName = emplCC.Name.Trim();
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[rotaryShift].PaymentCode][startSicknessDate].DateStartSickness = startSicknessDate;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[rotaryShift].PaymentCode][startSicknessDate].PaymentCode = typesDict[rotaryShift].PaymentCode;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[rotaryShift].PaymentCode][startSicknessDate].EmployeeID = empl.EmployeeID;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[rotaryShift].PaymentCode][startSicknessDate].CompanyCode = company.Code;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[rotaryShift].PaymentCode][startSicknessDate].Date = currentDate;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[rotaryShift].PaymentCode][startSicknessDate].Type = type;
                                                    employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[rotaryShift].PaymentCode][startSicknessDate].HrsAmount = 0;
                                                }

                                                pairDuration = processedTO.EndTime.TimeOfDay - processedTO.StartTime.TimeOfDay;
                                                //if shift is until 23:59 add one minute
                                                if (processedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                    pairDuration = pairDuration.Add(new TimeSpan(0, 1, 0));

                                                employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[rotaryShift].PaymentCode][startSicknessDate].HrsAmount += (decimal)pairDuration.TotalMinutes / 60;
                                            }
                                        }

                                        pairDuration = processedTO.EndTime.TimeOfDay - processedTO.StartTime.TimeOfDay;

                                        //if shift is until 23:59 add one minute
                                        if (processedTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                            pairDuration = pairDuration.Add(new TimeSpan(0, 1, 0));
                                        int totalMinutes = (int)pairDuration.TotalMinutes;

                                        //if pass type is bank hours round pairs -- 2012.06.13. Natasa count real pairs
                                        if (processedTO.PassTypeID == emplRules[Constants.RuleCompanyBankHourUsed].RuleValue)
                                        {
                                            int round = 30;
                                            if (emplRules.ContainsKey(Constants.RuleBankHoursUsedRounding))
                                                round = emplRules[Constants.RuleBankHoursUsedRounding].RuleValue;
                                            int newTotalMinutes = Common.Misc.roundDuration((int)pairDuration.TotalMinutes, round, false);
                                            bankHoursUsedGap += newTotalMinutes - totalMinutes;
                                            totalMinutes = newTotalMinutes;
                                        }

                                        employeeAnaliticalDict[empl.EmployeeID][currentDate][typesDict[processedTO.PassTypeID].PaymentCode][startSicknessDate].HrsAmount += (decimal)totalMinutes / 60;
                                        i++;

                                        // 24.07.2013. in estimation, save real hours of overtime paid, overtime to justify and night overtime to justify (for Work Analysis reports)
                                        if (type == Constants.PYTypeEstimated)
                                        {
                                            if (emplRules.ContainsKey(Constants.RuleCompanyOvertimePaid) && processedTO.PassTypeID == emplRules[Constants.RuleCompanyOvertimePaid].RuleValue)
                                            {
                                                if (processedTO.OldPassTypeID == -1)
                                                {
                                                    // add overtime paid hours
                                                    if (typesDict.ContainsKey(processedTO.PassTypeID))
                                                    {
                                                        if (!emplOvertimeEstHours.ContainsKey(processedTO.EmployeeID))
                                                            emplOvertimeEstHours.Add(processedTO.EmployeeID, new Dictionary<DateTime, Dictionary<string, decimal>>());

                                                        if (!emplOvertimeEstHours[processedTO.EmployeeID].ContainsKey(currentDate))
                                                            emplOvertimeEstHours[processedTO.EmployeeID].Add(currentDate, new Dictionary<string, decimal>());

                                                        if (!emplOvertimeEstHours[processedTO.EmployeeID][currentDate].ContainsKey(typesDict[processedTO.PassTypeID].PaymentCode))
                                                            emplOvertimeEstHours[processedTO.EmployeeID][currentDate].Add(typesDict[processedTO.PassTypeID].PaymentCode, 0);

                                                        emplOvertimeEstHours[processedTO.EmployeeID][currentDate][typesDict[processedTO.PassTypeID].PaymentCode] += (decimal)totalMinutes / 60;
                                                    }
                                                }
                                                else
                                                {
                                                    // add overtime to justify hours
                                                    if (typesDict.ContainsKey(processedTO.OldPassTypeID))
                                                    {
                                                        if (!emplOvertimeEstHours.ContainsKey(processedTO.EmployeeID))
                                                            emplOvertimeEstHours.Add(processedTO.EmployeeID, new Dictionary<DateTime, Dictionary<string, decimal>>());

                                                        if (!emplOvertimeEstHours[processedTO.EmployeeID].ContainsKey(currentDate))
                                                            emplOvertimeEstHours[processedTO.EmployeeID].Add(currentDate, new Dictionary<string, decimal>());

                                                        if (!emplOvertimeEstHours[processedTO.EmployeeID][currentDate].ContainsKey(typesDict[processedTO.OldPassTypeID].PaymentCode))
                                                            emplOvertimeEstHours[processedTO.EmployeeID][currentDate].Add(typesDict[processedTO.OldPassTypeID].PaymentCode, 0);

                                                        emplOvertimeEstHours[processedTO.EmployeeID][currentDate][typesDict[processedTO.OldPassTypeID].PaymentCode] += (decimal)totalMinutes / 60;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (unRegularDataFound && type == Constants.PYTypeReal)
                                    {
                                        date1 = date1.AddDays(1);
                                        continue;
                                    }

                                    //if we had bank hours used diferent in buffer and in pairs reduse regular work or if there is not enough regular work reduse some other code
                                    if (bankHoursUsedGap > 0)
                                    {
                                        decimal bankHrsGap = (decimal)bankHoursUsedGap / 60;
                                        if (employeeAnaliticalDict[empl.EmployeeID][date1].ContainsKey(typesDict[emplRules[Constants.RuleCompanyRegularWork].RuleValue].PaymentCode))
                                        {
                                            foreach (DateTime date in employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[emplRules[Constants.RuleCompanyRegularWork].RuleValue].PaymentCode].Keys)
                                            {
                                                if (employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[emplRules[Constants.RuleCompanyRegularWork].RuleValue].PaymentCode][date].HrsAmount > bankHrsGap)
                                                {
                                                    employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[emplRules[Constants.RuleCompanyRegularWork].RuleValue].PaymentCode][date].HrsAmount -= bankHrsGap;
                                                    bankHrsGap = 0;
                                                    break;
                                                }
                                                else
                                                {
                                                    bankHrsGap -= employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[emplRules[Constants.RuleCompanyRegularWork].RuleValue].PaymentCode][date].HrsAmount;
                                                    employeeAnaliticalDict[empl.EmployeeID][date1].Remove(typesDict[emplRules[Constants.RuleCompanyRegularWork].RuleValue].PaymentCode);

                                                    if (bankHrsGap <= 0)
                                                        break;
                                                }
                                            }
                                        }
                                        if (bankHrsGap > 0)
                                        {
                                            decimal maxDuration = 0;
                                            string maxCode = "";
                                            DateTime maxDate = new DateTime();
                                            string bhUsedCode = "";

                                            if (typesDict.ContainsKey(emplRules[Constants.RuleCompanyBankHourUsed].RuleValue))
                                                bhUsedCode = typesDict[emplRules[Constants.RuleCompanyBankHourUsed].RuleValue].PaymentCode;

                                            maxDuration = getMaxDuration(ref maxCode, ref maxDate, employeeAnaliticalDict[empl.EmployeeID][date1], bhUsedCode);
                                            while (bankHrsGap > 0 && maxDuration > 0)
                                            {
                                                if (maxCode != "" && !maxDate.Equals(new DateTime()) && employeeAnaliticalDict[empl.EmployeeID][date1][maxCode][maxDate].HrsAmount > bankHrsGap)
                                                {
                                                    employeeAnaliticalDict[empl.EmployeeID][date1][maxCode][maxDate].HrsAmount -= bankHrsGap;
                                                    bankHrsGap = 0;
                                                    break;
                                                }
                                                else
                                                {
                                                    bankHrsGap -= employeeAnaliticalDict[empl.EmployeeID][date1][maxCode][maxDate].HrsAmount;
                                                    employeeAnaliticalDict[empl.EmployeeID][date1].Remove(maxCode);

                                                    if (bankHrsGap <= 0)
                                                        break;
                                                }

                                                maxDuration = getMaxDuration(ref maxCode, ref maxDate, employeeAnaliticalDict[empl.EmployeeID][date1], bhUsedCode);
                                            }
                                        }

                                        if (bankHrsGap > 0)
                                        {
                                            if (typesDict.ContainsKey(emplRules[Constants.RuleCompanyBankHourUsed].RuleValue) && employeeAnaliticalDict[empl.EmployeeID][date1].ContainsKey(typesDict[emplRules[Constants.RuleCompanyBankHourUsed].RuleValue].PaymentCode))
                                            {
                                                foreach (DateTime date in employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[emplRules[Constants.RuleCompanyBankHourUsed].RuleValue].PaymentCode].Keys)
                                                {
                                                    if (employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[emplRules[Constants.RuleCompanyBankHourUsed].RuleValue].PaymentCode][date].HrsAmount > bankHrsGap)
                                                    {
                                                        employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[emplRules[Constants.RuleCompanyBankHourUsed].RuleValue].PaymentCode][date].HrsAmount -= bankHrsGap;
                                                        bankHrsGap = 0;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        bankHrsGap -= employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[emplRules[Constants.RuleCompanyBankHourUsed].RuleValue].PaymentCode][date].HrsAmount;
                                                        employeeAnaliticalDict[empl.EmployeeID][date1].Remove(typesDict[emplRules[Constants.RuleCompanyBankHourUsed].RuleValue].PaymentCode);

                                                        if (bankHrsGap <= 0)
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (emplRules.ContainsKey(Constants.RuleMealMinPresence))
                                {
                                    //add meal counter
                                    if (dayPresenceDuration.TotalMinutes >= emplRules[Constants.RuleMealMinPresence].RuleValue)
                                    {
                                        employeeBuffersDict[empl.EmployeeID].MealCounter++;
                                    }
                                    // add lunch break
                                    if (dayPresenceDurationLunchBreak.TotalMinutes >= emplRules[Constants.RuleMealMinPresence].RuleValue)
                                    {
                                        DateTime lunchDate = defaultSicknessStart;
                                        int lunchBreak = emplRules[Constants.RuleCompanyLunchBreak].RuleValue;

                                        if (!employeeAnaliticalDict.ContainsKey(empl.EmployeeID))
                                        {
                                            employeeAnaliticalDict.Add(empl.EmployeeID, new Dictionary<DateTime, Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>>());
                                        }
                                        if (!employeeAnaliticalDict[empl.EmployeeID].ContainsKey(date1))
                                        {
                                            employeeAnaliticalDict[empl.EmployeeID].Add(date1, new Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>());
                                        }
                                        if (!employeeAnaliticalDict[empl.EmployeeID][date1].ContainsKey(typesDict[lunchBreak].PaymentCode))
                                        {
                                            employeeAnaliticalDict[empl.EmployeeID][date1].Add(typesDict[lunchBreak].PaymentCode, new Dictionary<DateTime, EmployeePYDataAnaliticalTO>());
                                            employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[lunchBreak].PaymentCode].Add(lunchDate, new EmployeePYDataAnaliticalTO());
                                            employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[lunchBreak].PaymentCode][lunchDate].PYCalcID = calcID;
                                            employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[lunchBreak].PaymentCode][lunchDate].CCDesc = emplCC.Description;
                                            employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[lunchBreak].PaymentCode][lunchDate].EmployeeType = employeeType.Trim();
                                            employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[lunchBreak].PaymentCode][lunchDate].CCName = emplCC.Name.Trim();
                                            employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[lunchBreak].PaymentCode][lunchDate].DateStartSickness = lunchDate;
                                            employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[lunchBreak].PaymentCode][lunchDate].PaymentCode = typesDict[lunchBreak].PaymentCode;
                                            employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[lunchBreak].PaymentCode][lunchDate].EmployeeID = empl.EmployeeID;
                                            employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[lunchBreak].PaymentCode][lunchDate].CompanyCode = company.Code;
                                            employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[lunchBreak].PaymentCode][lunchDate].Date = date1;
                                            employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[lunchBreak].PaymentCode][lunchDate].Type = type;
                                            employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[lunchBreak].PaymentCode][lunchDate].HrsAmount = 0;
                                        }

                                        employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[lunchBreak].PaymentCode][lunchDate].HrsAmount += (decimal)Constants.LunchBreakDuration / 60;

                                        decimal maxDuration = 0;
                                        int maxType = -1;
                                        foreach (int typeID in emplEffTypesLunchBreak)
                                        {
                                            if (employeeAnaliticalDict.ContainsKey(empl.EmployeeID) && employeeAnaliticalDict[empl.EmployeeID].ContainsKey(date1) && employeeAnaliticalDict[empl.EmployeeID][date1].ContainsKey(typesDict[typeID].PaymentCode)
                                                && employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[typeID].PaymentCode].ContainsKey(lunchDate))
                                            {
                                                decimal duration = employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[typeID].PaymentCode][lunchDate].HrsAmount;
                                                if (maxDuration < duration)
                                                {
                                                    maxDuration = duration;
                                                    maxType = typeID;
                                                }
                                            }
                                        }

                                        if (maxDuration > (decimal)Constants.LunchBreakDuration / 60)
                                        {
                                            employeeAnaliticalDict[empl.EmployeeID][date1][typesDict[maxType].PaymentCode][lunchDate].HrsAmount -= (decimal)Constants.LunchBreakDuration / 60;
                                        }
                                        else
                                        {
                                            //log.writeLog(DateTime.Now + this.ToString() + ".GenerateData() : Calculate lunch break error - can not decrease effective work for lunch break! EmployeeID = " + empl.EmployeeID +
                                            //                    "; Date = " + date1.ToString("dd.MM.yyyy"));
                                        }
                                    }
                                }

                                employeeBuffersDict[empl.EmployeeID].TransportCounter += transport;
                                employeeBuffersDict[empl.EmployeeID].TransportCounter += transportOutWS;

                                date1 = date1.AddDays(1);
                            }
                        }
                        //if (unRegularDataFound && type == Constants.PYTypeReal)
                        //    continue;

                        //get counters and recalculate values until date dtTo
                        Dictionary<int, EmployeeCounterValueTO> counterValues = new Dictionary<int, EmployeeCounterValueTO>();
                        if (emplCounterValues.ContainsKey(empl.EmployeeID))
                        {
                            foreach (int typeID in emplCounterValues[empl.EmployeeID].Keys)
                            {
                                counterValues.Add(typeID, new EmployeeCounterValueTO(emplCounterValues[empl.EmployeeID][typeID]));
                            }
                        }

                        #region Recalculate Counters
                        if (FIATRegularEmployee(empl.EmployeeTypeID))
                        {
                            if (processedPairsAfterDict.ContainsKey(empl.EmployeeID))
                            {
                                bool pairBeforeEndWithNightShift = false;
                                bool updateCounters = true;
                                List<DateTime> weeksChanged = new List<DateTime>();
                                foreach (DateTime date in processedPairsAfterDict[empl.EmployeeID].Keys)
                                {
                                    foreach (IOPairProcessedTO processed in processedPairsAfterDict[empl.EmployeeID][date])
                                    {
                                        //for night shift do not update twice counter
                                        if (pairBeforeEndWithNightShift)
                                        {
                                            updateCounters = false;
                                            pairBeforeEndWithNightShift = false;
                                        }
                                        else
                                        {
                                            updateCounters = true;
                                        }

                                        int counterType = -1;
                                        if (updateCounters)
                                        {
                                            //all counters but paid leave take from rules 
                                            foreach (string key in Constants.CounterTypesForRuleTypes.Keys)
                                            {
                                                if (emplRules.ContainsKey(key) && emplRules[key].RuleValue == processed.PassTypeID)
                                                {
                                                    counterType = Constants.CounterTypesForRuleTypes[key];
                                                    break;
                                                }
                                            }

                                            //paid leave take types from pass_types
                                            if (typesDict[processed.PassTypeID].LimitCompositeID != -1)
                                            {
                                                counterType = (int)Constants.EmplCounterTypes.PaidLeaveCounter;
                                            }
                                        }
                                        if (counterType != -1)
                                        {
                                            bool is2DayShift = false;
                                            bool is2DayShiftPrevious = false;
                                            WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();

                                            WorkTimeSchemaTO workTimeSchema = null;

                                            List<EmployeeTimeScheduleTO> emplTS = new List<EmployeeTimeScheduleTO>();
                                            if (EmplTimeSchemas.ContainsKey(empl.EmployeeID))
                                                emplTS = EmplTimeSchemas[empl.EmployeeID];

                                            //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                                            //are night shift days. If day is night shift day, also take first interval of next day
                                            Dictionary<int, WorkTimeIntervalTO> edi = Common.Misc.getDayTimeSchemaIntervals(emplTS, date, ref is2DayShift,
                                                ref is2DayShiftPrevious, ref firstIntervalNextDay, ref workTimeSchema, dictTimeSchema);

                                            //if date is first day after period and if prev day is 2 day shift do not considare io pairs start before noon
                                            // or if previous day is not 2 day shift, do not considere pairs start in midnight
                                            if (((is2DayShiftPrevious && processed.StartTime.TimeOfDay < new TimeSpan(12, 0, 0))
                                                || !is2DayShiftPrevious && processed.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                                && date.Date == endDate.Date.AddDays(1))
                                            {
                                                continue;
                                            }

                                            foreach (int count in counterValues.Keys)
                                            {
                                                if (count == counterType)
                                                {
                                                    //if counter maesure unit is DAY decrease by one
                                                    if (counterValues[count].MeasureUnit == Constants.emplCounterMesureDay)
                                                    {

                                                        if (counterValues[count].Value > 0)
                                                        {
                                                            counterValues[count].Value--;
                                                            if (processed.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                            {
                                                                pairBeforeEndWithNightShift = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //log.writeLog(DateTime.Now + this.ToString() + ".GeneratePYData() : Recalculate counters faild! Counter = 0 for Employee_ID = " +
                                                            //    processed.EmployeeID + "; Date = " + processed.IOPairDate.ToString("dd.MM.yyyy") + "; Pass_type_ID = " + processed.PassTypeID.ToString());
                                                        }
                                                    }
                                                    //if counter maesure unit is MINUTE
                                                    else
                                                    {
                                                        TimeSpan pairDuration = processed.EndTime.TimeOfDay - processed.StartTime.TimeOfDay;
                                                        if (processed.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                            pairDuration = pairDuration.Add(new TimeSpan(0, 1, 0));

                                                        // if type is stop working done, increase counter - counter is in minutes
                                                        if (emplRules.ContainsKey(Constants.RuleCompanyStopWorkingDone) && emplRules[Constants.RuleCompanyStopWorkingDone].RuleValue == processed.PassTypeID)
                                                            counterValues[count].Value += (int)pairDuration.TotalMinutes;
                                                        // if type is bank hour used, increase counter - counter is in minutes
                                                        else if (emplRules.ContainsKey(Constants.RuleCompanyBankHourUsed) && emplRules[Constants.RuleCompanyBankHourUsed].RuleValue == processed.PassTypeID)
                                                        {
                                                            int round = 30;
                                                            if (emplRules.ContainsKey(Constants.RuleBankHoursUsedRounding))
                                                                round = emplRules[Constants.RuleBankHoursUsedRounding].RuleValue;
                                                            counterValues[count].Value += Common.Misc.roundDuration((int)pairDuration.TotalMinutes, round, false);
                                                        }
                                                        // if type is overtime notjustified used, increase counter - counter is in minutes
                                                        else if (emplRules.ContainsKey(Constants.RuleCompanyInitialOvertimeUsed) && emplRules[Constants.RuleCompanyInitialOvertimeUsed].RuleValue == processed.PassTypeID)
                                                        {
                                                            int round = 1;
                                                            if (emplRules.ContainsKey(Constants.RuleInitialOvertimeUsedRounding))
                                                                round = emplRules[Constants.RuleInitialOvertimeUsedRounding].RuleValue;
                                                            counterValues[count].Value += Common.Misc.roundDuration((int)pairDuration.TotalMinutes, round, false);
                                                        }
                                                        // else decrease counter - counter is in minutes
                                                        else
                                                        {
                                                            if (pairDuration.TotalMinutes > 0 && counterValues[count].Value >= pairDuration.TotalMinutes)
                                                            {

                                                            }
                                                            else
                                                            {
                                                                //log.writeLog(DateTime.Now + this.ToString() + ".GeneratePYData() : Recalculate counters go to MINUS! Counter = " + counterValues[count].Value.ToString() +
                                                                //    " for Employee_ID = " + processed.EmployeeID + "; Date = " + processed.IOPairDate.ToString("dd.MM.yyyy") + "; Pass_type_ID = " + processed.PassTypeID.ToString()
                                                                //     + "; Start time = " + processed.StartTime.ToString("HH:mm:ss") + "; End time = " + processed.EndTime.ToString("HH:mm:ss"));
                                                            }

                                                            if (!emplRules.ContainsKey(Constants.RuleCompanyInitialOvertime) || emplRules[Constants.RuleCompanyInitialOvertime].RuleValue != processed.PassTypeID)
                                                                counterValues[count].Value = counterValues[count].Value - (int)pairDuration.TotalMinutes;
                                                        }
                                                    }

                                                    if ((emplRules.ContainsKey(Constants.RuleCompanyAnnualLeave) && processed.PassTypeID == emplRules[Constants.RuleCompanyAnnualLeave].RuleValue)
                                                        || (emplRules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && processed.PassTypeID == emplRules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue))
                                                    {
                                                        //if shift is more than 8 hours long and if employee has whole week annual leave 
                                                        //decrease couter once more
                                                        if (workTimeSchema != null && Common.Misc.is10HourShift(workTimeSchema))
                                                        {
                                                            int dayNum = 0;
                                                            switch (processed.IOPairDate.DayOfWeek)
                                                            {
                                                                case DayOfWeek.Monday:
                                                                    dayNum = 0;
                                                                    break;
                                                                case DayOfWeek.Tuesday:
                                                                    dayNum = 1;
                                                                    break;
                                                                case DayOfWeek.Wednesday:
                                                                    dayNum = 2;
                                                                    break;
                                                                case DayOfWeek.Thursday:
                                                                    dayNum = 3;
                                                                    break;
                                                                case DayOfWeek.Friday:
                                                                    dayNum = 4;
                                                                    break;
                                                                case DayOfWeek.Saturday:
                                                                    dayNum = 5;
                                                                    break;
                                                                case DayOfWeek.Sunday:
                                                                    dayNum = 6;
                                                                    break;
                                                            }

                                                            DateTime weekBegining = processed.IOPairDate.AddDays(-dayNum).Date; // first day of current week

                                                            if (weeksChanged.Contains(weekBegining.Date))
                                                            {
                                                                break;
                                                            }

                                                            // get week annual leave pairs
                                                            int annualLeaveDays = 0;
                                                            IOPairProcessed annualPair = new IOPairProcessed();
                                                            List<IOPairProcessedTO> annualLeaveWeekPairs = annualPair.SearchWeekPairs(processed.EmployeeID, processed.IOPairDate, false, Common.Misc.getAnnualLeaveTypesString(emplRules), null);

                                                            foreach (IOPairProcessedTO aPair in annualLeaveWeekPairs)
                                                            {
                                                                // second night shift belongs to previous day
                                                                if (aPair.StartTime.Hour == 0 && aPair.StartTime.Minute == 0)
                                                                    continue;

                                                                annualLeaveDays++;
                                                            }

                                                            // if fourts day of week is changed, subtract/add two days
                                                            if (annualLeaveDays == (Constants.fullWeek10hShift - 1))
                                                            {
                                                                counterValues[count].Value--;
                                                                weeksChanged.Add(weekBegining);
                                                            }
                                                        }
                                                    }

                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        #endregion

                            employeeBuffersDict[empl.EmployeeID].StopWorkingHrsBalans = (decimal)counterValues[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value / 60;
                            employeeBuffersDict[empl.EmployeeID].BankHrsBalans = (decimal)counterValues[(int)Constants.EmplCounterTypes.BankHoursCounter].Value / 60;
                            employeeBuffersDict[empl.EmployeeID].PaidLeaveBalans = counterValues[(int)Constants.EmplCounterTypes.PaidLeaveCounter].Value;
                            employeeBuffersDict[empl.EmployeeID].PaidLeaveUsed = paidLeaveUsed;
                            employeeBuffersDict[empl.EmployeeID].NotJustifiedOvertimeBalans = counterValues[(int)Constants.EmplCounterTypes.NotJustifiedOvertime].Value + overtimeToKeep;
                            employeeBuffersDict[empl.EmployeeID].NotJustifiedOvertime = overtimeToKeep;
                            int usedBefore = counterValues[(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value - vacationUsed;
                            int leftPrevBefore = 0;
                            int leftCurrBefore = 0;

                            if (counterValues[(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value >= usedBefore)
                            {
                                leftPrevBefore = counterValues[(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value - usedBefore;
                                leftCurrBefore = counterValues[(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value;
                            }
                            else
                            {
                                leftCurrBefore = counterValues[(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value - (usedBefore - counterValues[(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value);
                                leftPrevBefore = 0;
                            }

                            if (leftPrevBefore <= vacationUsed)
                            {
                                employeeBuffersDict[empl.EmployeeID].ValactionUsedPrevYear = leftPrevBefore;
                                employeeBuffersDict[empl.EmployeeID].VacationUsedCurrYear = vacationUsed - leftPrevBefore;
                            }
                            else
                            {
                                employeeBuffersDict[empl.EmployeeID].ValactionUsedPrevYear = vacationUsed;
                                employeeBuffersDict[empl.EmployeeID].VacationUsedCurrYear = 0;
                            }

                            if (counterValues[(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value >= counterValues[(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value)
                            {
                                counterValues[(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value = counterValues[(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value - counterValues[(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value;
                            }
                            else
                            {
                                counterValues[(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value = counterValues[(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value - (counterValues[(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value - counterValues[(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value);
                                counterValues[(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value = 0;
                            }
                            employeeBuffersDict[empl.EmployeeID].VacationLeftCurrYear = counterValues[(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value;
                            employeeBuffersDict[empl.EmployeeID].VacationLeftPrevYear = counterValues[(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value;

                            // check if there are negative counters
                            //if (employeeBuffersDict[empl.EmployeeID].StopWorkingHrsBalans < 0)
                            //{
                            //    if (type == Constants.PYTypeReal)
                            //    {
                            //        unRegularDataFound = true;
                            //        //unregularEmplID = empl.EmployeeID;
                            //        unregularMessage = "negativeStopWorkingBalans";

                            //        if (!unregularEmployees.ContainsKey(empl.EmployeeID))
                            //            unregularEmployees.Add(empl.EmployeeID, new List<string>());
                            //        if (!unregularEmployees[empl.EmployeeID].Contains(unregularMessage))
                            //            unregularEmployees[empl.EmployeeID].Add(unregularMessage);
                            //        continue;
                            //    }
                            //    else
                            //        employeeBuffersDict[empl.EmployeeID].StopWorkingHrsBalans = 0;
                            //}

                            // 31.01.2013. Sanja - allow negative values - need to change insert statement (null value is inserted for -1 balans)
                            //if (employeeBuffersDict[empl.EmployeeID].BankHrsBalans < 0)
                            //{
                            //    if (type == Constants.PYTypeReal)
                            //    {                                
                            //        //unRegularDataFound = true;
                            //        //unregularEmplID = empl.EmployeeID;
                            //        //unregularMessage = rm.GetString("negativeBankHoursBalans", culture);
                            //        //continue;
                            //        //if (!unregularEmployees.ContainsKey(empl.EmployeeID))
                            //        //    unregularEmployees.Add(empl.EmployeeID, rm.GetString("negativeBankHoursBalans", culture));
                            //        //else
                            //        //    unregularEmployees[empl.EmployeeID] += " " + rm.GetString("negativeBankHoursBalans", culture);
                            //    }
                            //    else
                            //        employeeBuffersDict[empl.EmployeeID].BankHrsBalans = 0;
                            //}

                            if (employeeBuffersDict[empl.EmployeeID].PaidLeaveBalans < 0)
                            {
                                if (type == Constants.PYTypeReal)
                                {
                                    unRegularDataFound = true;
                                    //unregularEmplID = empl.EmployeeID;
                                    unregularMessage = "negativePaidLeaveBalans";
                                    if (!unregularEmployees.ContainsKey(empl.EmployeeID))
                                        unregularEmployees.Add(empl.EmployeeID, new List<string>());
                                    if (!unregularEmployees[empl.EmployeeID].Contains(unregularMessage))
                                        unregularEmployees[empl.EmployeeID].Add(unregularMessage);
                                    continue;
                                }
                                else
                                    employeeBuffersDict[empl.EmployeeID].PaidLeaveBalans = 0;
                            }

                            if (employeeBuffersDict[empl.EmployeeID].VacationLeftCurrYear < 0)
                            {
                                if (type == Constants.PYTypeReal)
                                {
                                    unRegularDataFound = true;
                                    //unregularEmplID = empl.EmployeeID;
                                    unregularMessage = "negativeVacationLeftCurrentYearBalans";
                                    if (!unregularEmployees.ContainsKey(empl.EmployeeID))
                                        unregularEmployees.Add(empl.EmployeeID, new List<string>());
                                    if (!unregularEmployees[empl.EmployeeID].Contains(unregularMessage))
                                        unregularEmployees[empl.EmployeeID].Add(unregularMessage);
                                    continue;
                                }
                                else
                                    employeeBuffersDict[empl.EmployeeID].VacationLeftCurrYear = 0;
                            }

                            if (employeeBuffersDict[empl.EmployeeID].VacationLeftPrevYear < 0)
                            {
                                if (type == Constants.PYTypeReal)
                                {
                                    unRegularDataFound = true;
                                    //unregularEmplID = empl.EmployeeID;
                                    unregularMessage = "negativeVacationLeftPreviousYearBalans";
                                    if (!unregularEmployees.ContainsKey(empl.EmployeeID))
                                        unregularEmployees.Add(empl.EmployeeID, new List<string>());
                                    if (!unregularEmployees[empl.EmployeeID].Contains(unregularMessage))
                                        unregularEmployees[empl.EmployeeID].Add(unregularMessage);
                                    continue;
                                }
                                else
                                    employeeBuffersDict[empl.EmployeeID].VacationLeftPrevYear = 0;
                            }

                            if (employeeBuffersDict[empl.EmployeeID].NotJustifiedOvertimeBalans < 0)
                            {
                                if (type == Constants.PYTypeReal)
                                {
                                    unRegularDataFound = true;
                                    //unregularEmplID = empl.EmployeeID;
                                    unregularMessage = "negativeNotJustifiedOvertimeBalans";

                                    if (!unregularEmployees.ContainsKey(empl.EmployeeID))
                                        unregularEmployees.Add(empl.EmployeeID, new List<string>());
                                    if (!unregularEmployees[empl.EmployeeID].Contains(unregularMessage))
                                        unregularEmployees[empl.EmployeeID].Add(unregularMessage);
                                    continue;
                                }
                                else
                                    employeeBuffersDict[empl.EmployeeID].NotJustifiedOvertimeBalans = 0;
                            }
                        }

                        // calculate meals approved and not approved by BG and KG
                        if (employeeMeals.ContainsKey(empl.EmployeeID))
                        {
                            DateTime nightWorkEnd = new DateTime();

                            if (emplRules.ContainsKey(Constants.RuleNightWork))
                                nightWorkEnd = emplRules[Constants.RuleNightWork].RuleDateTime2;

                            Dictionary<int, int> approvedSum = new Dictionary<int, int>();
                            Dictionary<int, int> notApprovedSum = new Dictionary<int, int>();

                            foreach (DateTime date in employeeMeals[empl.EmployeeID].Keys)
                            {
                                foreach (OnlineMealsUsedTO meal in employeeMeals[empl.EmployeeID][date])
                                {
                                    bool nightShiftFound = false;
                                    if ((meal.EventTime.Date.Equals(startDate.Date) || meal.EventTime.Date.Equals(endDate.Date.AddDays(1)))
                                        && meal.EventTime.TimeOfDay <= nightWorkEnd.TimeOfDay)
                                    {
                                        // check if there is night shift
                                        List<EmployeeTimeScheduleTO> schList = new List<EmployeeTimeScheduleTO>();
                                        if (EmplTimeSchemas.ContainsKey(meal.EmployeeID))
                                            schList = EmplTimeSchemas[meal.EmployeeID];
                                        List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(meal.EventTime.Date, schList, dictTimeSchema);

                                        foreach (WorkTimeIntervalTO interval in intervals)
                                        {
                                            if (interval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0) && interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes > 0)
                                            {
                                                nightShiftFound = true;
                                                break;
                                            }
                                        }
                                    }

                                    // first day meals until night work end belongs to previous day if employee has night shift ending so skip them
                                    if (meal.EventTime.Date.Equals(startDate.Date) && meal.EventTime.TimeOfDay <= nightWorkEnd.TimeOfDay && nightShiftFound)
                                        continue;

                                    // day after last day meals until night work end belongs to selected interval if employee has night shift ending, and after night work do not
                                    if (meal.EventTime.Date.Equals(endDate.Date.AddDays(1)) && (meal.EventTime.TimeOfDay > nightWorkEnd.TimeOfDay
                                        || (meal.EventTime.TimeOfDay <= nightWorkEnd.TimeOfDay && !nightShiftFound)))
                                        continue;

                                    // skip deleted meals and buisness trip meals
                                    if (meal.Approved.Trim().ToUpper().Equals(Constants.MealDeleted.Trim().ToUpper())
                                        || meal.Approved.Trim().ToUpper().Equals(Constants.MealBusinessTrip.Trim().ToUpper()))
                                        continue;

                                    if (meal.Approved == Constants.MealApproved)
                                    {
                                        if (!approvedSum.ContainsKey(meal.MealTypeID))
                                            approvedSum.Add(meal.MealTypeID, meal.Qty);
                                        else
                                            approvedSum[meal.MealTypeID] += meal.Qty;
                                    }
                                    else if (meal.Approved == Constants.MealNotApproved && meal.ApprovedBy != Constants.AutoCheckUser)
                                    {
                                        if (!notApprovedSum.ContainsKey(meal.MealTypeID))
                                            notApprovedSum.Add(meal.MealTypeID, meal.Qty);
                                        else
                                            notApprovedSum[meal.MealTypeID] += meal.Qty;
                                    }
                                    else if (meal.AutoCheck == Constants.yesInt)
                                    {
                                        if (!approvedSum.ContainsKey(meal.MealTypeID))
                                            approvedSum.Add(meal.MealTypeID, meal.Qty);
                                        else
                                            approvedSum[meal.MealTypeID] += meal.Qty;
                                    }
                                    else
                                    {
                                        if (type == Constants.PYTypeReal)
                                        {
                                            if (meal.AutoCheck == Constants.noInt || !FIATRegularEmployee(empl.EmployeeTypeID))
                                            {
                                                if (!notApprovedSum.ContainsKey(meal.MealTypeID))
                                                    notApprovedSum.Add(meal.MealTypeID, meal.Qty);
                                                else
                                                    notApprovedSum[meal.MealTypeID] += meal.Qty;
                                            }
                                            else
                                            {
                                                unRegularDataFound = true;
                                                //unregularEmplID = empl.EmployeeID;
                                                unregularMessage = "unregularMeals";
                                                if (!unregularEmployees.ContainsKey(empl.EmployeeID))
                                                    unregularEmployees.Add(empl.EmployeeID, new List<string>());
                                                if (!unregularEmployees[empl.EmployeeID].Contains(unregularMessage))
                                                    unregularEmployees[empl.EmployeeID].Add(unregularMessage);
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            if (!approvedSum.ContainsKey(meal.MealTypeID))
                                                approvedSum.Add(meal.MealTypeID, meal.Qty);
                                            else
                                                approvedSum[meal.MealTypeID] += meal.Qty;
                                        }
                                    }
                                }

                                if (unRegularDataFound && type == Constants.PYTypeReal)
                                    continue;
                            }

                            if (unRegularDataFound && type == Constants.PYTypeReal)
                                continue;

                            foreach (int mType in mealTypesDict.Keys)
                            {
                                if (approvedSum.ContainsKey(mType))
                                {
                                    if (!employeeBuffersDict[empl.EmployeeID].ApprovedMeals.ContainsKey(mType))
                                        employeeBuffersDict[empl.EmployeeID].ApprovedMeals.Add(mType, approvedSum[mType]);
                                    else
                                        employeeBuffersDict[empl.EmployeeID].ApprovedMeals[mType] = approvedSum[mType];
                                }
                                else if (!employeeBuffersDict[empl.EmployeeID].ApprovedMeals.ContainsKey(mType))
                                    employeeBuffersDict[empl.EmployeeID].ApprovedMeals.Add(mType, 0);

                                if (notApprovedSum.ContainsKey(mType))
                                {
                                    if (!employeeBuffersDict[empl.EmployeeID].NotApprovedMeals.ContainsKey(mType))
                                        employeeBuffersDict[empl.EmployeeID].NotApprovedMeals.Add(mType, notApprovedSum[mType]);
                                    else
                                        employeeBuffersDict[empl.EmployeeID].NotApprovedMeals[mType] = notApprovedSum[mType];
                                }
                                else if (!employeeBuffersDict[empl.EmployeeID].NotApprovedMeals.ContainsKey(mType))
                                    employeeBuffersDict[empl.EmployeeID].NotApprovedMeals.Add(mType, 0);
                            }
                        }
                        else
                        {
                            foreach (int mType in mealTypesDict.Keys)
                            {
                                if (!employeeBuffersDict[empl.EmployeeID].ApprovedMeals.ContainsKey(mType))
                                    employeeBuffersDict[empl.EmployeeID].ApprovedMeals.Add(mType, 0);

                                if (!employeeBuffersDict[empl.EmployeeID].NotApprovedMeals.ContainsKey(mType))
                                    employeeBuffersDict[empl.EmployeeID].NotApprovedMeals.Add(mType, 0);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //log.writeLog(this.ToString() + ".GeneratePYData(); Exception: " + ex.Message + "; for employee_id = " + empl.EmployeeID.ToString());
                        throw ex;
                    }

                    // set old working unit value for next calculating
                    empl.WorkingUnitID = emplWUID;
                }

                if (type == Constants.PYTypeReal && unRegularDataFound)
                {
                    return 0;
                }
                else
                {
                    EmployeePYDataAnalitical analitical = new EmployeePYDataAnalitical();
                    bool trans = analitical.BeginTransaction();
                    if (trans)
                    {
                        try
                        {
                            bool succ = true;
                           
                            foreach (int emplID in employeeAnaliticalDict.Keys)
                            {
                                if (!employeeSumDict.ContainsKey(emplID))
                                {
                                    employeeSumDict.Add(emplID, new Dictionary<string, Dictionary<DateTime, EmployeePYDataSumTO>>());
                                }

                                foreach (DateTime date in employeeAnaliticalDict[emplID].Keys)
                                {
                                    foreach (string code in employeeAnaliticalDict[emplID][date].Keys)
                                    {
                                        string paymentCode = code;
                                        if (code == Constants.FiatCollectiveAnnualLeavePaymentCode)
                                            paymentCode = ALPaymentCode;

                                        paymentCode = paymentCode.Replace(Constants.FiatClosurePaymentCode, "").Replace(Constants.FiatLayOffPaymentCode, "").Replace(Constants.FiatStoppagePaymentCode, "").Replace(Constants.FiatPublicHollidayPaymentCode, "");

                                        if (!employeeSumDict[emplID].ContainsKey(paymentCode))
                                        {
                                            employeeSumDict[emplID].Add(paymentCode, new Dictionary<DateTime, EmployeePYDataSumTO>());
                                        }

                                        foreach (DateTime sicknessDate in employeeAnaliticalDict[emplID][date][code].Keys)
                                        {
                                            analitical.EmplAnaliticalTO = employeeAnaliticalDict[emplID][date][code][sicknessDate];

                                            if (!employeeSumDict[emplID][paymentCode].ContainsKey(sicknessDate))
                                            {
                                                employeeSumDict[emplID][paymentCode].Add(sicknessDate, new EmployeePYDataSumTO());
                                                employeeSumDict[emplID][paymentCode][sicknessDate].PYCalcID = analitical.EmplAnaliticalTO.PYCalcID;
                                                employeeSumDict[emplID][paymentCode][sicknessDate].PaymentCode = paymentCode;
                                                employeeSumDict[emplID][paymentCode][sicknessDate].EmployeeID = emplID;
                                                employeeSumDict[emplID][paymentCode][sicknessDate].CCDesc = analitical.EmplAnaliticalTO.CCDesc.Trim();
                                                employeeSumDict[emplID][paymentCode][sicknessDate].DateStartSickness = analitical.EmplAnaliticalTO.DateStartSickness;
                                                employeeSumDict[emplID][paymentCode][sicknessDate].EmployeeType = analitical.EmplAnaliticalTO.EmployeeType.Trim();
                                                employeeSumDict[emplID][paymentCode][sicknessDate].CCName = analitical.EmplAnaliticalTO.CCName.Trim();
                                                employeeSumDict[emplID][paymentCode][sicknessDate].CompanyCode = analitical.EmplAnaliticalTO.CompanyCode;
                                                employeeSumDict[emplID][paymentCode][sicknessDate].DateStart = startDate.Date;
                                                employeeSumDict[emplID][paymentCode][sicknessDate].DateEnd = endDate.Date;
                                                employeeSumDict[emplID][paymentCode][sicknessDate].Type = analitical.EmplAnaliticalTO.Type;
                                                employeeSumDict[emplID][paymentCode][sicknessDate].HrsAmount = 0;
                                            }

                                            employeeSumDict[emplID][paymentCode][sicknessDate].HrsAmount += Math.Round(analitical.EmplAnaliticalTO.HrsAmount, 2);
                                        }
                                    }
                                }
                            }

                            EmployeePYDataBuffer buffer = new EmployeePYDataBuffer();
                            buffer.SetTransaction(analitical.GetTransaction());
                            foreach (int emplID in employeeBuffersDict.Keys)
                            {
                                int typeID = -1;
                                if (emplDict.ContainsKey(emplID))
                                    typeID = emplDict[emplID].EmployeeTypeID;

                                bool leavingEmpl = FIATRegularEmployee(typeID) && emplDict.ContainsKey(emplID) && emplDict[emplID].Status.Trim().ToUpper().Equals(Constants.statusRetired.Trim().ToUpper())
                                    && emplAscoData.ContainsKey(emplID) && !emplAscoData[emplID].DatetimeValue3.Equals(new DateTime()) && emplAscoData[emplID].DatetimeValue3.Date <= endDate.Date.AddDays(1);

                                //if employee has no io_pairs do not insert buffers for him
                                if (employeeSumDict.ContainsKey(emplID) || !FIATRegularEmployee(typeID))
                                {
                                    buffer.EmplBuffTO = employeeBuffersDict[emplID];

                                    string bhCode = "";
                                    string swCode = "";
                                    string alCode = "";
                                    DateTime sicknessDate = defaultSicknessStart;
                                    if (leavingEmpl && buffer.EmplBuffTO.BankHrsBalans != 0)
                                    {
                                        if (buffer.EmplBuffTO.BankHrsBalans > 0 && emplRulesEmployees.ContainsKey(emplID) && emplRulesEmployees[emplID].ContainsKey(Constants.RulePaidUnusedBankedHours)
                                            && typesDict.ContainsKey(emplRulesEmployees[emplID][Constants.RulePaidUnusedBankedHours].RuleValue))
                                        {
                                            bhCode = typesDict[emplRulesEmployees[emplID][Constants.RulePaidUnusedBankedHours].RuleValue].PaymentCode;
                                            if (!employeeSumDict[emplID].ContainsKey(bhCode))
                                            {
                                                employeeSumDict[emplID].Add(bhCode, new Dictionary<DateTime, EmployeePYDataSumTO>());
                                                employeeSumDict[emplID][bhCode].Add(sicknessDate, new EmployeePYDataSumTO());
                                                employeeSumDict[emplID][bhCode][sicknessDate].PYCalcID = buffer.EmplBuffTO.PYCalcID;
                                                employeeSumDict[emplID][bhCode][sicknessDate].EmployeeType = buffer.EmplBuffTO.EmployeeType;
                                                employeeSumDict[emplID][bhCode][sicknessDate].CCName = buffer.EmplBuffTO.CCName;
                                                employeeSumDict[emplID][bhCode][sicknessDate].CCDesc = buffer.EmplBuffTO.CCDesc;
                                                employeeSumDict[emplID][bhCode][sicknessDate].DateStartSickness = sicknessDate;
                                                employeeSumDict[emplID][bhCode][sicknessDate].PaymentCode = bhCode;
                                                employeeSumDict[emplID][bhCode][sicknessDate].EmployeeID = emplID;
                                                employeeSumDict[emplID][bhCode][sicknessDate].CompanyCode = buffer.EmplBuffTO.CompanyCode;
                                                employeeSumDict[emplID][bhCode][sicknessDate].DateStart = startDate.Date;
                                                employeeSumDict[emplID][bhCode][sicknessDate].DateEnd = endDate.Date;
                                                employeeSumDict[emplID][bhCode][sicknessDate].Type = buffer.EmplBuffTO.Type;
                                                employeeSumDict[emplID][bhCode][sicknessDate].HrsAmount = 0;
                                            }

                                            employeeSumDict[emplID][bhCode][sicknessDate].HrsAmount += Math.Round(buffer.EmplBuffTO.BankHrsBalans, 2);

                                            // add to analitical dictionary date for active employees is end of calculation period, date from leaving employees is last working day
                                            if (!employeeAnaliticalDict.ContainsKey(emplID))
                                                employeeAnaliticalDict.Add(emplID, new Dictionary<DateTime, Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>>());

                                            DateTime cumulativeDate = endDate.Date;
                                            if (leavingEmpl && emplAscoData.ContainsKey(emplID) && !emplAscoData[emplID].DatetimeValue3.Equals(new DateTime()))
                                                cumulativeDate = emplAscoData[emplID].DatetimeValue3.AddDays(-1).Date;

                                            if (!employeeAnaliticalDict[emplID].ContainsKey(cumulativeDate))
                                                employeeAnaliticalDict[emplID].Add(cumulativeDate, new Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>());

                                            if (!employeeAnaliticalDict[emplID][cumulativeDate].ContainsKey(bhCode))
                                            {
                                                employeeAnaliticalDict[emplID][cumulativeDate].Add(bhCode, new Dictionary<DateTime, EmployeePYDataAnaliticalTO>());
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhCode].Add(sicknessDate, new EmployeePYDataAnaliticalTO());
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].PYCalcID = buffer.EmplBuffTO.PYCalcID;
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].CCDesc = buffer.EmplBuffTO.CCDesc.Trim();
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].EmployeeType = buffer.EmplBuffTO.EmployeeType.Trim();
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].CCName = buffer.EmplBuffTO.CCName.Trim();
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].DateStartSickness = sicknessDate;
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].PaymentCode = bhCode;
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].EmployeeID = emplID;
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].CompanyCode = buffer.EmplBuffTO.CompanyCode.Trim();
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].Date = cumulativeDate;
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].Type = buffer.EmplBuffTO.Type.Trim();
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].HrsAmount = 0;
                                            }

                                            employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].HrsAmount += Math.Round(buffer.EmplBuffTO.BankHrsBalans, 2);
                                        }
                                        else if (buffer.EmplBuffTO.BankHrsBalans < 0 && emplRulesEmployees.ContainsKey(emplID) && emplRulesEmployees[emplID].ContainsKey(Constants.RuleNegativeBHPayment)
                                            && typesDict.ContainsKey(emplRulesEmployees[emplID][Constants.RuleNegativeBHPayment].RuleValue))
                                        {
                                            string bhNegativeCode = typesDict[emplRulesEmployees[emplID][Constants.RuleNegativeBHPayment].RuleValue].PaymentCode;
                                            if (!employeeSumDict[emplID].ContainsKey(bhNegativeCode))
                                            {
                                                employeeSumDict[emplID].Add(bhNegativeCode, new Dictionary<DateTime, EmployeePYDataSumTO>());
                                                employeeSumDict[emplID][bhNegativeCode].Add(sicknessDate, new EmployeePYDataSumTO());
                                                employeeSumDict[emplID][bhNegativeCode][sicknessDate].PYCalcID = buffer.EmplBuffTO.PYCalcID;
                                                employeeSumDict[emplID][bhNegativeCode][sicknessDate].EmployeeType = buffer.EmplBuffTO.EmployeeType;
                                                employeeSumDict[emplID][bhNegativeCode][sicknessDate].CCName = buffer.EmplBuffTO.CCName;
                                                employeeSumDict[emplID][bhNegativeCode][sicknessDate].CCDesc = buffer.EmplBuffTO.CCDesc;
                                                employeeSumDict[emplID][bhNegativeCode][sicknessDate].DateStartSickness = sicknessDate;
                                                employeeSumDict[emplID][bhNegativeCode][sicknessDate].PaymentCode = bhNegativeCode;
                                                employeeSumDict[emplID][bhNegativeCode][sicknessDate].EmployeeID = emplID;
                                                employeeSumDict[emplID][bhNegativeCode][sicknessDate].CompanyCode = buffer.EmplBuffTO.CompanyCode;
                                                employeeSumDict[emplID][bhNegativeCode][sicknessDate].DateStart = startDate.Date;
                                                employeeSumDict[emplID][bhNegativeCode][sicknessDate].DateEnd = endDate.Date;
                                                employeeSumDict[emplID][bhNegativeCode][sicknessDate].Type = buffer.EmplBuffTO.Type;
                                                employeeSumDict[emplID][bhNegativeCode][sicknessDate].HrsAmount = 0;
                                            }

                                            employeeSumDict[emplID][bhNegativeCode][sicknessDate].HrsAmount += Math.Round(buffer.EmplBuffTO.BankHrsBalans, 2);

                                            // add to analitical dictionary date for active employees is end of calculation period, date from leaving employees is last working day
                                            if (!employeeAnaliticalDict.ContainsKey(emplID))
                                                employeeAnaliticalDict.Add(emplID, new Dictionary<DateTime, Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>>());

                                            DateTime cumulativeDate = endDate.Date;
                                            if (leavingEmpl && emplAscoData.ContainsKey(emplID) && !emplAscoData[emplID].DatetimeValue3.Equals(new DateTime()))
                                                cumulativeDate = emplAscoData[emplID].DatetimeValue3.AddDays(-1).Date;

                                            if (!employeeAnaliticalDict[emplID].ContainsKey(cumulativeDate))
                                                employeeAnaliticalDict[emplID].Add(cumulativeDate, new Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>());

                                            if (!employeeAnaliticalDict[emplID][cumulativeDate].ContainsKey(bhNegativeCode))
                                            {
                                                employeeAnaliticalDict[emplID][cumulativeDate].Add(bhNegativeCode, new Dictionary<DateTime, EmployeePYDataAnaliticalTO>());
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhNegativeCode].Add(sicknessDate, new EmployeePYDataAnaliticalTO());
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhNegativeCode][sicknessDate].PYCalcID = buffer.EmplBuffTO.PYCalcID;
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhNegativeCode][sicknessDate].CCDesc = buffer.EmplBuffTO.CCDesc.Trim();
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhNegativeCode][sicknessDate].EmployeeType = buffer.EmplBuffTO.EmployeeType.Trim();
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhNegativeCode][sicknessDate].CCName = buffer.EmplBuffTO.CCName.Trim();
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhNegativeCode][sicknessDate].DateStartSickness = sicknessDate;
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhNegativeCode][sicknessDate].PaymentCode = bhNegativeCode;
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhNegativeCode][sicknessDate].EmployeeID = emplID;
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhNegativeCode][sicknessDate].CompanyCode = buffer.EmplBuffTO.CompanyCode.Trim();
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhNegativeCode][sicknessDate].Date = cumulativeDate;
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhNegativeCode][sicknessDate].Type = buffer.EmplBuffTO.Type.Trim();
                                                employeeAnaliticalDict[emplID][cumulativeDate][bhNegativeCode][sicknessDate].HrsAmount = 0;
                                            }

                                            employeeAnaliticalDict[emplID][cumulativeDate][bhNegativeCode][sicknessDate].HrsAmount += Math.Round(buffer.EmplBuffTO.BankHrsBalans, 2);
                                        }
                                    }

                                    if (!leavingEmpl)
                                    {
                                        // check if there is something for payment
                                        decimal bhHours = 0;

                                        if (bhPayRegular)
                                        {
                                            if (emplBHBalances.ContainsKey(emplID))
                                            {
                                                foreach (DateTime month in emplBHBalances[emplID].Keys)
                                                {
                                                    if (emplBHBalances[emplID][month].ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                                                    {
                                                        bhHours += ((decimal)emplBHBalances[emplID][month][(int)Constants.EmplCounterTypes.BankHoursCounter].Balance) / 60;

                                                        // create db record
                                                        BufferMonthlyBalancePaidTO paidTO = new BufferMonthlyBalancePaidTO();
                                                        paidTO.PYCalcID = calcID;
                                                        paidTO.EmployeeID = emplID;
                                                        paidTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.BankHoursCounter;
                                                        paidTO.Month = month.Date;
                                                        paidTO.ValuePaid = emplBHBalances[emplID][month][(int)Constants.EmplCounterTypes.BankHoursCounter].Balance;
                                                        emplBHPaidHours.Add(paidTO);
                                                    }
                                                }
                                            }
                                        }
                                        else if (bhPayFile && emplBHPayDict.ContainsKey(emplID))
                                        {
                                            foreach (DateTime month in emplBHPayDict[emplID].Keys)
                                            {
                                                // create db record
                                                BufferMonthlyBalancePaidTO paidTO = new BufferMonthlyBalancePaidTO();
                                                paidTO.PYCalcID = calcID;
                                                paidTO.EmployeeID = emplID;
                                                paidTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.BankHoursCounter;
                                                paidTO.Month = month.Date;
                                                paidTO.ValuePaid = (int)(emplBHPayDict[emplID][month] * 60);
                                                emplBHPaidHours.Add(paidTO);

                                                // add hours for payment
                                                bhHours += emplBHPayDict[emplID][month];
                                            }
                                        }

                                        if (bhHours > 0 && (bhPayFile || (bhPayRegular && payBH)))
                                        {
                                            if (emplRulesEmployees.ContainsKey(emplID) && emplRulesEmployees[emplID].ContainsKey(Constants.RulePaidUnusedBankedHours)
                                                && typesDict.ContainsKey(emplRulesEmployees[emplID][Constants.RulePaidUnusedBankedHours].RuleValue))
                                            {
                                                bhCode = typesDict[emplRulesEmployees[emplID][Constants.RulePaidUnusedBankedHours].RuleValue].PaymentCode;
                                                if (!employeeSumDict[emplID].ContainsKey(bhCode))
                                                {
                                                    employeeSumDict[emplID].Add(bhCode, new Dictionary<DateTime, EmployeePYDataSumTO>());
                                                    employeeSumDict[emplID][bhCode].Add(sicknessDate, new EmployeePYDataSumTO());
                                                    employeeSumDict[emplID][bhCode][sicknessDate].PYCalcID = buffer.EmplBuffTO.PYCalcID;
                                                    employeeSumDict[emplID][bhCode][sicknessDate].EmployeeType = buffer.EmplBuffTO.EmployeeType;
                                                    employeeSumDict[emplID][bhCode][sicknessDate].CCName = buffer.EmplBuffTO.CCName;
                                                    employeeSumDict[emplID][bhCode][sicknessDate].CCDesc = buffer.EmplBuffTO.CCDesc;
                                                    employeeSumDict[emplID][bhCode][sicknessDate].DateStartSickness = sicknessDate;
                                                    employeeSumDict[emplID][bhCode][sicknessDate].PaymentCode = bhCode;
                                                    employeeSumDict[emplID][bhCode][sicknessDate].EmployeeID = emplID;
                                                    employeeSumDict[emplID][bhCode][sicknessDate].CompanyCode = buffer.EmplBuffTO.CompanyCode;
                                                    employeeSumDict[emplID][bhCode][sicknessDate].DateStart = startDate.Date;
                                                    employeeSumDict[emplID][bhCode][sicknessDate].DateEnd = endDate.Date;
                                                    employeeSumDict[emplID][bhCode][sicknessDate].Type = buffer.EmplBuffTO.Type;
                                                    employeeSumDict[emplID][bhCode][sicknessDate].HrsAmount = 0;
                                                }

                                                employeeSumDict[emplID][bhCode][sicknessDate].HrsAmount += Math.Round(bhHours, 2);

                                                // add to analitical dictionary date for active employees is end of calculation period, date from leaving employees is last working day
                                                if (!employeeAnaliticalDict.ContainsKey(emplID))
                                                    employeeAnaliticalDict.Add(emplID, new Dictionary<DateTime, Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>>());

                                                DateTime cumulativeDate = endDate.Date;
                                                if (leavingEmpl && emplAscoData.ContainsKey(emplID) && !emplAscoData[emplID].DatetimeValue3.Equals(new DateTime()))
                                                    cumulativeDate = emplAscoData[emplID].DatetimeValue3.AddDays(-1).Date;

                                                if (!employeeAnaliticalDict[emplID].ContainsKey(cumulativeDate))
                                                    employeeAnaliticalDict[emplID].Add(cumulativeDate, new Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>());

                                                if (!employeeAnaliticalDict[emplID][cumulativeDate].ContainsKey(bhCode))
                                                {
                                                    employeeAnaliticalDict[emplID][cumulativeDate].Add(bhCode, new Dictionary<DateTime, EmployeePYDataAnaliticalTO>());
                                                    employeeAnaliticalDict[emplID][cumulativeDate][bhCode].Add(sicknessDate, new EmployeePYDataAnaliticalTO());
                                                    employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].PYCalcID = buffer.EmplBuffTO.PYCalcID;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].CCDesc = buffer.EmplBuffTO.CCDesc.Trim();
                                                    employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].EmployeeType = buffer.EmplBuffTO.EmployeeType.Trim();
                                                    employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].CCName = buffer.EmplBuffTO.CCName.Trim();
                                                    employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].DateStartSickness = sicknessDate;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].PaymentCode = bhCode;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].EmployeeID = emplID;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].CompanyCode = buffer.EmplBuffTO.CompanyCode.Trim();
                                                    employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].Date = cumulativeDate;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].Type = buffer.EmplBuffTO.Type.Trim();
                                                    employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].HrsAmount = 0;
                                                }

                                                employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].HrsAmount += Math.Round(bhHours, 2);
                                            }
                                        }

                                        // check if there is something for payment
                                        decimal swHours = 0;

                                        if (stopWorkingPay)
                                        {
                                            if (emplSWBalances.ContainsKey(emplID))
                                            {
                                                foreach (DateTime month in emplSWBalances[emplID].Keys)
                                                {
                                                    if (emplSWBalances[emplID][month].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                                                    {
                                                        swHours += ((decimal)emplSWBalances[emplID][month][(int)Constants.EmplCounterTypes.StopWorkingCounter].Balance) / 60;

                                                        // create db record
                                                        BufferMonthlyBalancePaidTO paidTO = new BufferMonthlyBalancePaidTO();
                                                        paidTO.PYCalcID = calcID;
                                                        paidTO.EmployeeID = emplID;
                                                        paidTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.StopWorkingCounter;
                                                        paidTO.Month = month.Date;
                                                        paidTO.ValuePaid = emplSWBalances[emplID][month][(int)Constants.EmplCounterTypes.StopWorkingCounter].Balance;
                                                        emplSWPaidHours.Add(paidTO);
                                                    }
                                                }
                                            }
                                        }
                                        else if (swPayFile && emplSWPayDict.ContainsKey(emplID))
                                        {
                                            foreach (DateTime month in emplSWPayDict[emplID].Keys)
                                            {
                                                // create db record
                                                BufferMonthlyBalancePaidTO paidTO = new BufferMonthlyBalancePaidTO();
                                                paidTO.PYCalcID = calcID;
                                                paidTO.EmployeeID = emplID;
                                                paidTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.StopWorkingCounter;
                                                paidTO.Month = month.Date;
                                                paidTO.ValuePaid = (int)(emplSWPayDict[emplID][month] * 60);
                                                emplSWPaidHours.Add(paidTO);

                                                // add hours for payment
                                                swHours += emplSWPayDict[emplID][month];
                                            }
                                        }
                                            
                                        if (swHours > 0 && (swPayFile || (stopWorkingPay && paySW)))
                                        {
                                            if (emplRulesEmployees.ContainsKey(emplID) && emplRulesEmployees[emplID].ContainsKey(Constants.RuleSWPayment)
                                                && typesDict.ContainsKey(emplRulesEmployees[emplID][Constants.RuleSWPayment].RuleValue))
                                            {
                                                string swPayCode = typesDict[emplRulesEmployees[emplID][Constants.RuleSWPayment].RuleValue].PaymentCode;
                                                if (!employeeSumDict[emplID].ContainsKey(swPayCode))
                                                {
                                                    employeeSumDict[emplID].Add(swPayCode, new Dictionary<DateTime, EmployeePYDataSumTO>());
                                                    employeeSumDict[emplID][swPayCode].Add(sicknessDate, new EmployeePYDataSumTO());
                                                    employeeSumDict[emplID][swPayCode][sicknessDate].PYCalcID = buffer.EmplBuffTO.PYCalcID;
                                                    employeeSumDict[emplID][swPayCode][sicknessDate].EmployeeType = buffer.EmplBuffTO.EmployeeType;
                                                    employeeSumDict[emplID][swPayCode][sicknessDate].CCName = buffer.EmplBuffTO.CCName;
                                                    employeeSumDict[emplID][swPayCode][sicknessDate].CCDesc = buffer.EmplBuffTO.CCDesc;
                                                    employeeSumDict[emplID][swPayCode][sicknessDate].DateStartSickness = sicknessDate;
                                                    employeeSumDict[emplID][swPayCode][sicknessDate].PaymentCode = swPayCode;
                                                    employeeSumDict[emplID][swPayCode][sicknessDate].EmployeeID = emplID;
                                                    employeeSumDict[emplID][swPayCode][sicknessDate].CompanyCode = buffer.EmplBuffTO.CompanyCode;
                                                    employeeSumDict[emplID][swPayCode][sicknessDate].DateStart = startDate.Date;
                                                    employeeSumDict[emplID][swPayCode][sicknessDate].DateEnd = endDate.Date;
                                                    employeeSumDict[emplID][swPayCode][sicknessDate].Type = buffer.EmplBuffTO.Type;
                                                    employeeSumDict[emplID][swPayCode][sicknessDate].HrsAmount = 0;
                                                }

                                                employeeSumDict[emplID][swPayCode][sicknessDate].HrsAmount += Math.Round(swHours, 2);

                                                // add to analitical dictionary date for active employees is end of calculation period, date from leaving employees is last working day
                                                if (!employeeAnaliticalDict.ContainsKey(emplID))
                                                    employeeAnaliticalDict.Add(emplID, new Dictionary<DateTime, Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>>());

                                                DateTime cumulativeDate = endDate.Date;
                                                if (leavingEmpl && emplAscoData.ContainsKey(emplID) && !emplAscoData[emplID].DatetimeValue3.Equals(new DateTime()))
                                                    cumulativeDate = emplAscoData[emplID].DatetimeValue3.AddDays(-1).Date;

                                                if (!employeeAnaliticalDict[emplID].ContainsKey(cumulativeDate))
                                                    employeeAnaliticalDict[emplID].Add(cumulativeDate, new Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>());

                                                if (!employeeAnaliticalDict[emplID][cumulativeDate].ContainsKey(swPayCode))
                                                {
                                                    employeeAnaliticalDict[emplID][cumulativeDate].Add(swPayCode, new Dictionary<DateTime, EmployeePYDataAnaliticalTO>());
                                                    employeeAnaliticalDict[emplID][cumulativeDate][swPayCode].Add(sicknessDate, new EmployeePYDataAnaliticalTO());
                                                    employeeAnaliticalDict[emplID][cumulativeDate][swPayCode][sicknessDate].PYCalcID = buffer.EmplBuffTO.PYCalcID;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][swPayCode][sicknessDate].CCDesc = buffer.EmplBuffTO.CCDesc.Trim();
                                                    employeeAnaliticalDict[emplID][cumulativeDate][swPayCode][sicknessDate].EmployeeType = buffer.EmplBuffTO.EmployeeType.Trim();
                                                    employeeAnaliticalDict[emplID][cumulativeDate][swPayCode][sicknessDate].CCName = buffer.EmplBuffTO.CCName.Trim();
                                                    employeeAnaliticalDict[emplID][cumulativeDate][swPayCode][sicknessDate].DateStartSickness = sicknessDate;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][swPayCode][sicknessDate].PaymentCode = swPayCode;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][swPayCode][sicknessDate].EmployeeID = emplID;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][swPayCode][sicknessDate].CompanyCode = buffer.EmplBuffTO.CompanyCode.Trim();
                                                    employeeAnaliticalDict[emplID][cumulativeDate][swPayCode][sicknessDate].Date = cumulativeDate;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][swPayCode][sicknessDate].Type = buffer.EmplBuffTO.Type.Trim();
                                                    employeeAnaliticalDict[emplID][cumulativeDate][swPayCode][sicknessDate].HrsAmount = 0;
                                                }

                                                employeeAnaliticalDict[emplID][cumulativeDate][swPayCode][sicknessDate].HrsAmount += Math.Round(swHours, 2);
                                            }
                                        }
                                    }

                                    if (vacationPay && buffer.EmplBuffTO.VacationLeftPrevYear > 0)
                                    {
                                        if (emplRulesEmployees.ContainsKey(emplID) && emplRulesEmployees[emplID].ContainsKey(Constants.RuleCompensationForUnusedVacation)
                                            && typesDict.ContainsKey(emplRulesEmployees[emplID][Constants.RuleCompensationForUnusedVacation].RuleValue))
                                        {
                                            alCode = typesDict[emplRulesEmployees[emplID][Constants.RuleCompensationForUnusedVacation].RuleValue].PaymentCode;
                                            if (!employeeSumDict[emplID].ContainsKey(alCode))
                                            {
                                                employeeSumDict[emplID].Add(alCode, new Dictionary<DateTime, EmployeePYDataSumTO>());
                                                employeeSumDict[emplID][alCode].Add(sicknessDate, new EmployeePYDataSumTO());
                                                employeeSumDict[emplID][alCode][sicknessDate].PYCalcID = buffer.EmplBuffTO.PYCalcID;
                                                employeeSumDict[emplID][alCode][sicknessDate].EmployeeType = buffer.EmplBuffTO.EmployeeType;
                                                employeeSumDict[emplID][alCode][sicknessDate].CCName = buffer.EmplBuffTO.CCName;
                                                employeeSumDict[emplID][alCode][sicknessDate].CCDesc = buffer.EmplBuffTO.CCDesc;
                                                employeeSumDict[emplID][alCode][sicknessDate].DateStartSickness = sicknessDate;
                                                employeeSumDict[emplID][alCode][sicknessDate].PaymentCode = alCode;
                                                employeeSumDict[emplID][alCode][sicknessDate].EmployeeID = emplID;
                                                employeeSumDict[emplID][alCode][sicknessDate].CompanyCode = buffer.EmplBuffTO.CompanyCode;
                                                employeeSumDict[emplID][alCode][sicknessDate].DateStart = startDate.Date;
                                                employeeSumDict[emplID][alCode][sicknessDate].DateEnd = endDate.Date;
                                                employeeSumDict[emplID][alCode][sicknessDate].Type = buffer.EmplBuffTO.Type;
                                                employeeSumDict[emplID][alCode][sicknessDate].HrsAmount = 0;
                                            }
                                            employeeSumDict[emplID][alCode][sicknessDate].HrsAmount += buffer.EmplBuffTO.VacationLeftPrevYear * 8;

                                            // add to analitical dictionary date for active employees is end of calculation period, date from leaving employees is last working day
                                            if (!employeeAnaliticalDict.ContainsKey(emplID))
                                                employeeAnaliticalDict.Add(emplID, new Dictionary<DateTime, Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>>());

                                            DateTime cumulativeDate = endDate.Date;
                                            if (leavingEmpl && emplAscoData.ContainsKey(emplID) && !emplAscoData[emplID].DatetimeValue3.Equals(new DateTime()))
                                                cumulativeDate = emplAscoData[emplID].DatetimeValue3.AddDays(-1).Date;

                                            if (!employeeAnaliticalDict[emplID].ContainsKey(cumulativeDate))
                                                employeeAnaliticalDict[emplID].Add(cumulativeDate, new Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>());

                                            if (!employeeAnaliticalDict[emplID][cumulativeDate].ContainsKey(alCode))
                                            {
                                                employeeAnaliticalDict[emplID][cumulativeDate].Add(alCode, new Dictionary<DateTime, EmployeePYDataAnaliticalTO>());
                                                employeeAnaliticalDict[emplID][cumulativeDate][alCode].Add(sicknessDate, new EmployeePYDataAnaliticalTO());
                                                employeeAnaliticalDict[emplID][cumulativeDate][alCode][sicknessDate].PYCalcID = buffer.EmplBuffTO.PYCalcID;
                                                employeeAnaliticalDict[emplID][cumulativeDate][alCode][sicknessDate].CCDesc = buffer.EmplBuffTO.CCDesc.Trim();
                                                employeeAnaliticalDict[emplID][cumulativeDate][alCode][sicknessDate].EmployeeType = buffer.EmplBuffTO.EmployeeType.Trim();
                                                employeeAnaliticalDict[emplID][cumulativeDate][alCode][sicknessDate].CCName = buffer.EmplBuffTO.CCName.Trim();
                                                employeeAnaliticalDict[emplID][cumulativeDate][alCode][sicknessDate].DateStartSickness = sicknessDate;
                                                employeeAnaliticalDict[emplID][cumulativeDate][alCode][sicknessDate].PaymentCode = alCode;
                                                employeeAnaliticalDict[emplID][cumulativeDate][alCode][sicknessDate].EmployeeID = emplID;
                                                employeeAnaliticalDict[emplID][cumulativeDate][alCode][sicknessDate].CompanyCode = buffer.EmplBuffTO.CompanyCode.Trim();
                                                employeeAnaliticalDict[emplID][cumulativeDate][alCode][sicknessDate].Date = cumulativeDate;
                                                employeeAnaliticalDict[emplID][cumulativeDate][alCode][sicknessDate].Type = buffer.EmplBuffTO.Type.Trim();
                                                employeeAnaliticalDict[emplID][cumulativeDate][alCode][sicknessDate].HrsAmount = 0;
                                            }

                                            employeeAnaliticalDict[emplID][cumulativeDate][alCode][sicknessDate].HrsAmount += buffer.EmplBuffTO.VacationLeftPrevYear * 8;
                                        }
                                    }

                                    // 21.02.2014. Sanja - leaving employees should be payed only positive bank hours balans
                                    //if ((swPay || leavingEmpl) && buffer.EmplBuffTO.StopWorkingHrsBalans > 0)
                                    if (swPay && buffer.EmplBuffTO.StopWorkingHrsBalans > 0)
                                    {
                                        if (emplRulesEmployees.ContainsKey(emplID) && typesDict.ContainsKey(Constants.ptFiatSWPay))
                                        {
                                            swCode = typesDict[Constants.ptFiatSWPay].PaymentCode;
                                            if (!employeeSumDict[emplID].ContainsKey(swCode))
                                            {
                                                employeeSumDict[emplID].Add(swCode, new Dictionary<DateTime, EmployeePYDataSumTO>());
                                                employeeSumDict[emplID][swCode].Add(sicknessDate, new EmployeePYDataSumTO());
                                                employeeSumDict[emplID][swCode][sicknessDate].PYCalcID = buffer.EmplBuffTO.PYCalcID;
                                                employeeSumDict[emplID][swCode][sicknessDate].EmployeeType = buffer.EmplBuffTO.EmployeeType;
                                                employeeSumDict[emplID][swCode][sicknessDate].CCName = buffer.EmplBuffTO.CCName;
                                                employeeSumDict[emplID][swCode][sicknessDate].CCDesc = buffer.EmplBuffTO.CCDesc;
                                                employeeSumDict[emplID][swCode][sicknessDate].DateStartSickness = sicknessDate;
                                                employeeSumDict[emplID][swCode][sicknessDate].PaymentCode = swCode;
                                                employeeSumDict[emplID][swCode][sicknessDate].EmployeeID = emplID;
                                                employeeSumDict[emplID][swCode][sicknessDate].CompanyCode = buffer.EmplBuffTO.CompanyCode;
                                                employeeSumDict[emplID][swCode][sicknessDate].DateStart = startDate.Date;
                                                employeeSumDict[emplID][swCode][sicknessDate].DateEnd = endDate.Date;
                                                employeeSumDict[emplID][swCode][sicknessDate].Type = buffer.EmplBuffTO.Type;
                                                employeeSumDict[emplID][swCode][sicknessDate].HrsAmount = 0;
                                            }

                                            employeeSumDict[emplID][swCode][sicknessDate].HrsAmount += Math.Round(buffer.EmplBuffTO.StopWorkingHrsBalans, 2);
                                        }
                                    }

                                    if (leavingEmpl)
                                    {
                                        // calculate bank hours balans due to stop working balans
                                        decimal bhBalans = 0;
                                        decimal swBalans = 0;

                                        if (employeeSumDict[emplID].ContainsKey(bhCode) && employeeSumDict[emplID][bhCode].ContainsKey(sicknessDate))
                                            bhBalans = employeeSumDict[emplID][bhCode][sicknessDate].HrsAmount;

                                        //if (employeeSumDict[emplID].ContainsKey(swCode) && employeeSumDict[emplID][swCode].ContainsKey(sicknessDate))
                                        //    swBalans = employeeSumDict[emplID][swCode][sicknessDate].HrsAmount;

                                        swBalans = Math.Round(buffer.EmplBuffTO.StopWorkingHrsBalans, 2);

                                        // 21.02.2014. Sanja - leaving employees should be payed only positive bank hours balans
                                        //if (swBalans != 0)
                                        //{
                                        //    if (bhBalans < swBalans)
                                        //    {
                                        //        swBalans = bhBalans - swBalans;
                                        //        bhBalans = 0;
                                        //    }
                                        //    else
                                        //    {
                                        //        bhBalans = bhBalans - swBalans;
                                        //        swBalans = 0;
                                        //    }
                                        //}

                                        //if (bhBalans < 0)
                                        //    bhBalans = 0;

                                        // change bank hours and stop working balans in files
                                        //if (employeeSumDict[emplID].ContainsKey(bhCode) && employeeSumDict[emplID][bhCode].ContainsKey(sicknessDate))
                                        //{
                                        //    if (bhBalans != 0)
                                        //        employeeSumDict[emplID][bhCode][sicknessDate].HrsAmount = bhBalans;
                                        //    else
                                        //        employeeSumDict[emplID].Remove(bhCode);
                                        //}

                                        DateTime cumulativeDate = endDate.Date;
                                        if (leavingEmpl && emplAscoData.ContainsKey(emplID) && !emplAscoData[emplID].DatetimeValue3.Equals(new DateTime()))
                                            cumulativeDate = emplAscoData[emplID].DatetimeValue3.AddDays(-1).Date;

                                        //if (employeeAnaliticalDict.ContainsKey(emplID) && employeeAnaliticalDict[emplID].ContainsKey(cumulativeDate)
                                        //    && employeeAnaliticalDict[emplID][cumulativeDate].ContainsKey(bhCode) && employeeAnaliticalDict[emplID][cumulativeDate][bhCode].ContainsKey(sicknessDate))
                                        //{
                                        //    if (bhBalans != 0)
                                        //        employeeAnaliticalDict[emplID][cumulativeDate][bhCode][sicknessDate].HrsAmount = bhBalans;
                                        //    else
                                        //        employeeAnaliticalDict[emplID][cumulativeDate].Remove(bhCode);
                                        //}

                                        if (employeeSumDict[emplID].ContainsKey(swCode) && !swPay)
                                            employeeSumDict[emplID].Remove(swCode);

                                        if (swBalans != 0)
                                        {
                                            // add row for stop working balans - code is stop working payment code
                                            if (emplRulesEmployees.ContainsKey(emplID) && emplRulesEmployees[emplID].ContainsKey(Constants.RuleSWPayment)
                                                && typesDict.ContainsKey(emplRulesEmployees[emplID][Constants.RuleSWPayment].RuleValue))
                                            {
                                                string code = typesDict[emplRulesEmployees[emplID][Constants.RuleSWPayment].RuleValue].PaymentCode;
                                                if (!employeeSumDict[emplID].ContainsKey(code))
                                                {
                                                    employeeSumDict[emplID].Add(code, new Dictionary<DateTime, EmployeePYDataSumTO>());
                                                    employeeSumDict[emplID][code].Add(sicknessDate, new EmployeePYDataSumTO());
                                                    employeeSumDict[emplID][code][sicknessDate].PYCalcID = buffer.EmplBuffTO.PYCalcID;
                                                    employeeSumDict[emplID][code][sicknessDate].EmployeeType = buffer.EmplBuffTO.EmployeeType;
                                                    employeeSumDict[emplID][code][sicknessDate].CCName = buffer.EmplBuffTO.CCName;
                                                    employeeSumDict[emplID][code][sicknessDate].CCDesc = buffer.EmplBuffTO.CCDesc;
                                                    employeeSumDict[emplID][code][sicknessDate].DateStartSickness = sicknessDate;
                                                    employeeSumDict[emplID][code][sicknessDate].PaymentCode = code;
                                                    employeeSumDict[emplID][code][sicknessDate].EmployeeID = emplID;
                                                    employeeSumDict[emplID][code][sicknessDate].CompanyCode = buffer.EmplBuffTO.CompanyCode;
                                                    employeeSumDict[emplID][code][sicknessDate].DateStart = startDate.Date;
                                                    employeeSumDict[emplID][code][sicknessDate].DateEnd = endDate.Date;
                                                    employeeSumDict[emplID][code][sicknessDate].Type = buffer.EmplBuffTO.Type;
                                                    employeeSumDict[emplID][code][sicknessDate].HrsAmount = 0;
                                                }

                                                employeeSumDict[emplID][code][sicknessDate].HrsAmount -= Math.Round(swBalans, 2);

                                                if (!employeeAnaliticalDict.ContainsKey(emplID))
                                                    employeeAnaliticalDict.Add(emplID, new Dictionary<DateTime, Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>>());

                                                if (!employeeAnaliticalDict[emplID].ContainsKey(cumulativeDate))
                                                    employeeAnaliticalDict[emplID].Add(cumulativeDate, new Dictionary<string, Dictionary<DateTime, EmployeePYDataAnaliticalTO>>());

                                                if (!employeeAnaliticalDict[emplID][cumulativeDate].ContainsKey(code))
                                                {
                                                    employeeAnaliticalDict[emplID][cumulativeDate].Add(code, new Dictionary<DateTime, EmployeePYDataAnaliticalTO>());
                                                    employeeAnaliticalDict[emplID][cumulativeDate][code].Add(sicknessDate, new EmployeePYDataAnaliticalTO());
                                                    employeeAnaliticalDict[emplID][cumulativeDate][code][sicknessDate].PYCalcID = buffer.EmplBuffTO.PYCalcID;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][code][sicknessDate].CCDesc = buffer.EmplBuffTO.CCDesc;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][code][sicknessDate].EmployeeType = buffer.EmplBuffTO.EmployeeType.Trim();
                                                    employeeAnaliticalDict[emplID][cumulativeDate][code][sicknessDate].CCName = buffer.EmplBuffTO.CCName.Trim();
                                                    employeeAnaliticalDict[emplID][cumulativeDate][code][sicknessDate].DateStartSickness = sicknessDate;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][code][sicknessDate].PaymentCode = code;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][code][sicknessDate].EmployeeID = emplID;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][code][sicknessDate].CompanyCode = buffer.EmplBuffTO.CompanyCode;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][code][sicknessDate].Date = cumulativeDate;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][code][sicknessDate].Type = buffer.EmplBuffTO.Type;
                                                    employeeAnaliticalDict[emplID][cumulativeDate][code][sicknessDate].HrsAmount = 0;
                                                }

                                                employeeAnaliticalDict[emplID][cumulativeDate][code][sicknessDate].HrsAmount -= swBalans;
                                            }
                                        }

                                        if (!leavingEmpls)
                                        {
                                            buffer.EmplBuffTO.BankHrsBalans = 0;
                                            buffer.EmplBuffTO.StopWorkingHrsBalans = 0;
                                        }
                                    }

                                    // ******************* 02.07.2013. Sanja - cover negative bank hours balans with overtime hours in estimation for bank hours payment month
                                    //if (cbBankHoursPay.Checked && buffer.EmplBuffTO.Type == Constants.PYTypeEstimated && (buffer.EmplBuffTO.BankHrsBalans < 0 || leavingEmpl))
                                    //{
                                    //    string overtimeCode = "";
                                    //    string bhPaidCode = "";
                                    //    string bhMonthlyCode = "";
                                    //    if (emplRulesEmployees.ContainsKey(emplID))
                                    //    {
                                    //        if (emplRulesEmployees[emplID].ContainsKey(Constants.RulePaidUnusedBankedHours)
                                    //            && typesDict.ContainsKey(emplRulesEmployees[emplID][Constants.RulePaidUnusedBankedHours].RuleValue))                                        
                                    //            bhPaidCode = typesDict[emplRulesEmployees[emplID][Constants.RulePaidUnusedBankedHours].RuleValue].PaymentCode;

                                    //        if (emplRulesEmployees[emplID].ContainsKey(Constants.RuleCompanyOvertimePaid)
                                    //            && typesDict.ContainsKey(emplRulesEmployees[emplID][Constants.RuleCompanyOvertimePaid].RuleValue))
                                    //            overtimeCode = typesDict[emplRulesEmployees[emplID][Constants.RuleCompanyOvertimePaid].RuleValue].PaymentCode;

                                    //        if (emplRulesEmployees[emplID].ContainsKey(Constants.RuleCompanyBankHourMonthly)
                                    //            && typesDict.ContainsKey(emplRulesEmployees[emplID][Constants.RuleCompanyBankHourMonthly].RuleValue))
                                    //            bhMonthlyCode = typesDict[emplRulesEmployees[emplID][Constants.RuleCompanyBankHourMonthly].RuleValue].PaymentCode;
                                    //    }

                                    //    if (overtimeCode.Trim() != "" && bhMonthlyCode.Trim() != "" && bhPaidCode.Trim() != "" && employeeSumDict.ContainsKey(emplID) 
                                    //        && employeeSumDict[emplID].ContainsKey(bhPaidCode) && employeeSumDict[emplID][bhPaidCode].ContainsKey(defaultSicknessStart)
                                    //        && employeeSumDict[emplID][bhPaidCode][defaultSicknessStart].HrsAmount < 0)
                                    //    {
                                    //        // get overtime hours                                            
                                    //        if (employeeSumDict[emplID].ContainsKey(overtimeCode) && employeeSumDict[emplID][overtimeCode].ContainsKey(defaultSicknessStart)
                                    //            && employeeSumDict[emplID][overtimeCode][defaultSicknessStart].HrsAmount > 0)
                                    //        {
                                    //            decimal convertingHours = employeeSumDict[emplID][overtimeCode][defaultSicknessStart].HrsAmount;

                                    //            if (Math.Abs(employeeSumDict[emplID][bhPaidCode][defaultSicknessStart].HrsAmount) < convertingHours)
                                    //                convertingHours = Math.Abs(employeeSumDict[emplID][bhPaidCode][defaultSicknessStart].HrsAmount);

                                    //            employeeSumDict[emplID][overtimeCode][defaultSicknessStart].HrsAmount -= convertingHours;

                                    //            if (employeeSumDict[emplID][overtimeCode][defaultSicknessStart].HrsAmount <= 0)
                                    //            {
                                    //                employeeSumDict[emplID][overtimeCode].Remove(defaultSicknessStart);
                                    //                employeeSumDict[emplID].Remove(overtimeCode);
                                    //            }

                                    //            if (!employeeSumDict[emplID].ContainsKey(bhMonthlyCode))
                                    //            {
                                    //                employeeSumDict[emplID].Add(bhMonthlyCode, new Dictionary<DateTime, EmployeePYDataSumTO>());
                                    //                employeeSumDict[emplID][bhMonthlyCode].Add(defaultSicknessStart, new EmployeePYDataSumTO());
                                    //                employeeSumDict[emplID][bhMonthlyCode][defaultSicknessStart].PYCalcID = buffer.EmplBuffTO.PYCalcID;
                                    //                employeeSumDict[emplID][bhMonthlyCode][defaultSicknessStart].EmployeeType = buffer.EmplBuffTO.EmployeeType;
                                    //                employeeSumDict[emplID][bhMonthlyCode][defaultSicknessStart].CCName = buffer.EmplBuffTO.CCName;
                                    //                employeeSumDict[emplID][bhMonthlyCode][defaultSicknessStart].CCDesc = buffer.EmplBuffTO.CCDesc;
                                    //                employeeSumDict[emplID][bhMonthlyCode][defaultSicknessStart].DateStartSickness = defaultSicknessStart;
                                    //                employeeSumDict[emplID][bhMonthlyCode][defaultSicknessStart].PaymentCode = bhMonthlyCode;
                                    //                employeeSumDict[emplID][bhMonthlyCode][defaultSicknessStart].EmployeeID = emplID;
                                    //                employeeSumDict[emplID][bhMonthlyCode][defaultSicknessStart].CompanyCode = buffer.EmplBuffTO.CompanyCode;
                                    //                employeeSumDict[emplID][bhMonthlyCode][defaultSicknessStart].DateStart = dtFrom.Value.Date;
                                    //                employeeSumDict[emplID][bhMonthlyCode][defaultSicknessStart].DateEnd = dtTo.Value.Date;
                                    //                employeeSumDict[emplID][bhMonthlyCode][defaultSicknessStart].Type = buffer.EmplBuffTO.Type;
                                    //                employeeSumDict[emplID][bhMonthlyCode][defaultSicknessStart].HrsAmount = 0;
                                    //            }

                                    //            employeeSumDict[emplID][bhMonthlyCode][defaultSicknessStart].HrsAmount += convertingHours;
                                    //            log.writeLog(emplID.ToString() + "|OV COVER BH NEGATIVE");
                                    //        }
                                    //        else
                                    //            log.writeLog(emplID.ToString() + "|BH NEGATIVE");

                                    //        // remove paid bank hours
                                    //        employeeSumDict[emplID][bhPaidCode].Remove(defaultSicknessStart);
                                    //        employeeSumDict[emplID].Remove(bhPaidCode);
                                    //    }
                                    //}
                                    // ******************* 02.07.2013. Sanja - cover negative bank hours balans with overtime hours in estimation for bank hours payment month

                                    // 20.03.2013. Sanja - if bank hours are paid, show empty buffers in buffer file
                                    //if (bhPay)
                                    //    buffer.EmplBuffTO.BankHrsBalans = 0;

                                    // 30.07.2013. Sanja - if there are bank hours to be paid, decrease buffer in buffers file
                                    if (!leavingEmpl && emplRulesEmployees.ContainsKey(emplID) && emplRulesEmployees[emplID].ContainsKey(Constants.RulePaidUnusedBankedHours)
                                        && typesDict.ContainsKey(emplRulesEmployees[emplID][Constants.RulePaidUnusedBankedHours].RuleValue)
                                        && employeeSumDict.ContainsKey(emplID) && employeeSumDict[emplID].ContainsKey(typesDict[emplRulesEmployees[emplID][Constants.RulePaidUnusedBankedHours].RuleValue].PaymentCode)
                                        && employeeSumDict[emplID][typesDict[emplRulesEmployees[emplID][Constants.RulePaidUnusedBankedHours].RuleValue].PaymentCode].ContainsKey(defaultSicknessStart))
                                        buffer.EmplBuffTO.BankHrsBalans -= employeeSumDict[emplID][typesDict[emplRulesEmployees[emplID][Constants.RulePaidUnusedBankedHours].RuleValue].PaymentCode][defaultSicknessStart].HrsAmount;

                                    // 10.07.2013. Sanja - if annual leave hours are paid, show empty buffers in buffer file
                                    if (vacationPay)
                                        buffer.EmplBuffTO.VacationLeftPrevYear = 0;

                                    // 01.02.203. Sanja - if pay of stop working is checked save just sum records with stop working pay code
                                    if (!swPay)
                                    {
                                        if (FIATRegularEmployee(typeID))
                                            succ = succ && buffer.Save(false) > 0;
                                        else
                                            succ = succ && buffer.SaveExpat(false) > 0;
                                    }
                                    string str = buffer.EmplBuffTO.PYCalcID.ToString() + "\t";
                                    str += buffer.EmplBuffTO.CompanyCode.Replace("\t", "") + "\t";
                                    str += buffer.EmplBuffTO.Type.Replace("\t", "") + "\t";
                                    str += buffer.EmplBuffTO.EmployeeID.ToString() + "\t";
                                    str += buffer.EmplBuffTO.FirstName.Replace("\t", "") + "\t";
                                    str += buffer.EmplBuffTO.LastName.Replace("\t", "") + "\t";
                                    str += buffer.EmplBuffTO.DateStart.ToString("yyyy-MM-dd") + "\t";
                                    str += buffer.EmplBuffTO.DateEnd.ToString("yyyy-MM-dd") + "\t";
                                    str += buffer.EmplBuffTO.FundHrs.ToString() + "\t";
                                    str += buffer.EmplBuffTO.FundDay.ToString() + "\t";
                                    str += buffer.EmplBuffTO.FundHrsEst.ToString() + "\t";
                                    str += buffer.EmplBuffTO.FundDayEst.ToString() + "\t";
                                    str += buffer.EmplBuffTO.MealCounter.ToString() + "\t";
                                    str += buffer.EmplBuffTO.TransportCounter.ToString() + "\t";
                                    str += buffer.EmplBuffTO.VacationLeftCurrYear.ToString() + "\t";
                                    str += buffer.EmplBuffTO.VacationLeftPrevYear.ToString() + "\t";
                                    str += buffer.EmplBuffTO.VacationUsedCurrYear.ToString() + "\t";
                                    str += buffer.EmplBuffTO.ValactionUsedPrevYear.ToString() + "\t";
                                    str += String.Format("{0:0.00}", buffer.EmplBuffTO.BankHrsBalans) + "\t";
                                    str += String.Format("{0:0.00}", buffer.EmplBuffTO.StopWorkingHrsBalans) + "\t";
                                    str += buffer.EmplBuffTO.PaidLeaveBalans.ToString() + "\t";
                                    str += buffer.EmplBuffTO.PaidLeaveUsed.ToString() + "\t";
                                    str += buffer.EmplBuffTO.EmployeeType + "\t";
                                    str += buffer.EmplBuffTO.CCName + "\t";
                                    str += buffer.EmplBuffTO.CCDesc + "\t";

                                    foreach (int mType in mealTypesDict.Keys)
                                    {
                                        if (buffer.EmplBuffTO.ApprovedMeals.ContainsKey(mType))
                                            str += buffer.EmplBuffTO.ApprovedMeals[mType].ToString().Trim() + "\t";
                                        else
                                            str += "0\t";
                                        if (buffer.EmplBuffTO.NotApprovedMeals.ContainsKey(mType))
                                            str += buffer.EmplBuffTO.NotApprovedMeals[mType].ToString().Trim() + "\t";
                                        else
                                            str += "0\t";
                                    }
                                    
                                    str += String.Format("{0:0.00}", (decimal)buffer.EmplBuffTO.NotJustifiedOvertimeBalans / 60) + "\t";
                                    str += String.Format("{0:0.00}", (decimal)buffer.EmplBuffTO.NotJustifiedOvertime / 60) + "\t";

                                    if (FIATRegularEmployee(typeID))
                                        bufferStrings.Add(str);
                                    else
                                        bufferExpatStrings.Add(str);
                                }
                            }

                            // save analitical data
                            foreach (int emplID in employeeAnaliticalDict.Keys)
                            {
                                foreach (DateTime date in employeeAnaliticalDict[emplID].Keys)
                                {
                                    foreach (string code in employeeAnaliticalDict[emplID][date].Keys)
                                    {
                                        foreach (DateTime sicknessDate in employeeAnaliticalDict[emplID][date][code].Keys)
                                        {
                                            EmployeePYDataAnaliticalTO analiticalTO = employeeAnaliticalDict[emplID][date][code][sicknessDate];

                                            // 01.02.203. Sanja - if pay of stop working is checked save just sum records with stop working pay code
                                            if (!swPay)
                                            {
                                                // 24.07.2013. if overtime to paid is saving in estimation, save real overtime hours for Work Analysis reports
                                                if (type == Constants.PYTypeEstimated)
                                                {
                                                    if (emplRulesEmployees.ContainsKey(emplID) && emplRulesEmployees[emplID].ContainsKey(Constants.RuleCompanyOvertimePaid)
                                                        && typesDict.ContainsKey(emplRulesEmployees[emplID][Constants.RuleCompanyOvertimePaid].RuleValue)
                                                        && analiticalTO.PaymentCode == typesDict[emplRulesEmployees[emplID][Constants.RuleCompanyOvertimePaid].RuleValue].PaymentCode)
                                                    {
                                                        string overtimePaidCode = typesDict[emplRulesEmployees[emplID][Constants.RuleCompanyOvertimePaid].RuleValue].PaymentCode;
                                                        string overtimeToJustifyCode = "";
                                                        string nightOvertimeToJustifyCode = "";

                                                        if (emplRulesEmployees.ContainsKey(emplID) && emplRulesEmployees[emplID].ContainsKey(Constants.RuleCompanyInitialOvertime)
                                                            && typesDict.ContainsKey(emplRulesEmployees[emplID][Constants.RuleCompanyInitialOvertime].RuleValue))
                                                            overtimeToJustifyCode = typesDict[emplRulesEmployees[emplID][Constants.RuleCompanyInitialOvertime].RuleValue].PaymentCode;

                                                        if (emplRulesEmployees.ContainsKey(emplID) && emplRulesEmployees[emplID].ContainsKey(Constants.RuleCompanyInitialNightOvertime)
                                                            && typesDict.ContainsKey(emplRulesEmployees[emplID][Constants.RuleCompanyInitialNightOvertime].RuleValue))
                                                            nightOvertimeToJustifyCode = typesDict[emplRulesEmployees[emplID][Constants.RuleCompanyInitialNightOvertime].RuleValue].PaymentCode;

                                                        // save all three values
                                                        if (emplOvertimeEstHours.ContainsKey(emplID) && emplOvertimeEstHours[emplID].ContainsKey(date))
                                                        {
                                                            if (overtimePaidCode != "" && emplOvertimeEstHours[emplID][date].ContainsKey(overtimePaidCode))
                                                            {
                                                                analitical.EmplAnaliticalTO = new EmployeePYDataAnaliticalTO(analiticalTO);
                                                                analitical.EmplAnaliticalTO.HrsAmount = emplOvertimeEstHours[emplID][date][overtimePaidCode];
                                                                succ = succ && analitical.Save(false) > 0;
                                                            }

                                                            if (overtimeToJustifyCode != "" && emplOvertimeEstHours[emplID][date].ContainsKey(overtimeToJustifyCode))
                                                            {
                                                                analitical.EmplAnaliticalTO = new EmployeePYDataAnaliticalTO(analiticalTO);
                                                                analitical.EmplAnaliticalTO.PaymentCode = overtimeToJustifyCode;
                                                                analitical.EmplAnaliticalTO.HrsAmount = emplOvertimeEstHours[emplID][date][overtimeToJustifyCode];
                                                                succ = succ && analitical.Save(false) > 0;
                                                            }

                                                            if (nightOvertimeToJustifyCode != "" && emplOvertimeEstHours[emplID][date].ContainsKey(nightOvertimeToJustifyCode))
                                                            {
                                                                analitical.EmplAnaliticalTO = new EmployeePYDataAnaliticalTO(analiticalTO);
                                                                analitical.EmplAnaliticalTO.PaymentCode = nightOvertimeToJustifyCode;
                                                                analitical.EmplAnaliticalTO.HrsAmount = emplOvertimeEstHours[emplID][date][nightOvertimeToJustifyCode];
                                                                succ = succ && analitical.Save(false) > 0;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        analitical.EmplAnaliticalTO = analiticalTO;
                                                        succ = analitical.Save(false) > 0;
                                                    }
                                                }
                                                else
                                                {
                                                    analitical.EmplAnaliticalTO = analiticalTO;
                                                    succ = analitical.Save(false) > 0;
                                                }
                                            }

                                            string paymentCode = analiticalTO.PaymentCode;
                                            if (code == Constants.FiatCollectiveAnnualLeavePaymentCode)
                                            {
                                                if (employeeAnaliticalDict[emplID][date].ContainsKey(ALPaymentCode))
                                                    continue;
                                                else
                                                    paymentCode = ALPaymentCode;
                                            }

                                            decimal hrsAmount = analiticalTO.HrsAmount;

                                            if (code == ALPaymentCode)
                                            {
                                                if (employeeAnaliticalDict[emplID][date].ContainsKey(Constants.FiatCollectiveAnnualLeavePaymentCode))
                                                {
                                                    foreach (DateTime calSickDate in employeeAnaliticalDict[emplID][date][Constants.FiatCollectiveAnnualLeavePaymentCode].Keys)
                                                    {
                                                        hrsAmount += employeeAnaliticalDict[emplID][date][Constants.FiatCollectiveAnnualLeavePaymentCode][calSickDate].HrsAmount;
                                                    }
                                                }
                                            }

                                            string str = analiticalTO.PYCalcID.ToString() + "\t";
                                            str += analiticalTO.CompanyCode.Replace("\t", "") + "\t";
                                            str += analiticalTO.Type.Replace("\t", "") + "\t";
                                            str += analiticalTO.EmployeeID.ToString() + "\t";
                                            str += analiticalTO.Date.ToString("yyyy-MM-dd") + "\t";
                                            str += paymentCode.Replace(Constants.FiatClosurePaymentCode, "").Replace(Constants.FiatLayOffPaymentCode, "").Replace(Constants.FiatStoppagePaymentCode, "").Replace(Constants.FiatPublicHollidayPaymentCode, "") + "\t";                                            
                                            str += String.Format("{0:0.00}", hrsAmount) + "\t";
                                            str += analiticalTO.EmployeeType + "\t";
                                            str += analiticalTO.CCName + "\t";
                                            str += analiticalTO.CCDesc + "\t";
                                            if (analiticalTO.DateStartSickness.Equals(defaultSicknessStart))
                                                str += "N/A\t";
                                            else
                                                str += analiticalTO.DateStartSickness.ToString("yyyy-MM-dd") + "\t";

                                            analiticStrings.Add(str);
                                        }
                                    }
                                }
                            }

                            EmployeePYDataSum emplSum = new EmployeePYDataSum();
                            emplSum.SetTransaction(analitical.GetTransaction());
                            foreach (int emplID in employeeSumDict.Keys)
                            {
                                foreach (string code in employeeSumDict[emplID].Keys)
                                {
                                    // 01.02.2013. Sanja - if pay of stop working is checked save just sum records with stop working pay code
                                    if (swPay && typesDict.ContainsKey(Constants.ptFiatSWPay) && code != typesDict[Constants.ptFiatSWPay].PaymentCode)
                                        continue;

                                    foreach (DateTime sicknessDate in employeeSumDict[emplID][code].Keys)
                                    {
                                        emplSum.EmplSum = employeeSumDict[emplID][code][sicknessDate];
                                        succ = emplSum.Save(false) > 0;
                                        string str = emplSum.EmplSum.PYCalcID.ToString() + "\t";
                                        str += emplSum.EmplSum.CompanyCode.Replace("\t", "") + "\t";
                                        str += emplSum.EmplSum.Type.Replace("\t", "") + "\t";
                                        str += emplSum.EmplSum.EmployeeID.ToString() + "\t";
                                        str += emplSum.EmplSum.DateStart.ToString("yyyy-MM-dd") + "\t";
                                        str += emplSum.EmplSum.DateEnd.ToString("yyyy-MM-dd") + "\t";
                                        str += emplSum.EmplSum.PaymentCode.Replace(Constants.FiatClosurePaymentCode, "").Replace(Constants.FiatLayOffPaymentCode, "").Replace(Constants.FiatStoppagePaymentCode, "").Replace(Constants.FiatPublicHollidayPaymentCode, "") + "\t";
                                        str += String.Format("{0:0.00}", emplSum.EmplSum.HrsAmount) + "\t";
                                        str += emplSum.EmplSum.EmployeeType + "\t";
                                        str += emplSum.EmplSum.CCName + "\t";
                                        str += emplSum.EmplSum.CCDesc + "\t";
                                        if (emplSum.EmplSum.DateStartSickness.Equals(defaultSicknessStart))
                                            str += "N/A\t";
                                        else
                                            str += emplSum.EmplSum.DateStartSickness.ToString("yyyy-MM-dd") + "\t";
                                        sumStrings.Add(str);
                                    }
                                }
                            }

                            BufferMonthlyBalancePaid emplBuffPaid = new BufferMonthlyBalancePaid();
                            emplBuffPaid.SetTransaction(analitical.GetTransaction());
                            foreach (BufferMonthlyBalancePaidTO buffTO in emplBHPaidHours)
                            {
                                emplBuffPaid.BalanceTO = buffTO;
                                emplBuffPaid.BalanceTO.RecalcFlag = Constants.noInt;
                                succ = emplBuffPaid.Save(false) > 0;
                            }

                            foreach (BufferMonthlyBalancePaidTO buffTO in emplSWPaidHours)
                            {
                                emplBuffPaid.BalanceTO = buffTO;
                                emplBuffPaid.BalanceTO.RecalcFlag = Constants.noInt;
                                succ = emplBuffPaid.Save(false) > 0;
                            }

                            if (succ)
                                analitical.CommitTransaction();
                            else
                            {
                                if (analitical.GetTransaction() != null)
                                    analitical.RollbackTransaction();
                            }
                        }
                        catch
                        {
                            if (analitical.GetTransaction() != null)
                                analitical.RollbackTransaction();

                            //log.writeLog(DateTime.Now + " PYIntegration.btnGenerateData_Click(): " + ex.Message + "\n");
                            //MessageBox.Show(ex.Message);
                            return 0;
                        }
                    }
                    else
                    {
                        //log.writeLog(DateTime.Now + " Misc.GeneratePYData(): Begin transaction faild.\n");
                        //MessageBox.Show("Begin transaction faild.");
                        return 0;
                    }
                }

                return calcID;
            }
            catch (Exception ex)
            {
                //log.writeLog(DateTime.Now + " PYIntegration.GenerateFiatPYData(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public static Dictionary<int, int> GetEmployeeFundHrs(string emplIDs, DateTime from, DateTime to)
        {
            try
            {
                Dictionary<int, int> emplFundHrs = new Dictionary<int, int>();

                if (emplIDs.Trim() == "" || from == new DateTime() || to == new DateTime() || from.Date > to.Date)
                    return emplFundHrs;

                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, from.Date, to.Date.AddDays(1), null);
                Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema().getDictionary();

                string[] idList = emplIDs.Split(',');

                foreach (string id in idList)
                {
                    int emplID = -1;
                    if (!int.TryParse(id, out emplID))
                        emplID = -1;

                    if (emplID == -1)
                        continue;

                    if (!emplFundHrs.ContainsKey(emplID))
                        emplFundHrs.Add(emplID, 0);

                    List<EmployeeTimeScheduleTO> schedules = new List<EmployeeTimeScheduleTO>();
                    if (emplSchedules.ContainsKey(emplID))
                        schedules = emplSchedules[emplID];

                    for (DateTime currDate = from.Date; currDate.Date <= to.Date.AddDays(1); currDate = currDate.AddDays(1))
                    {
                        List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(currDate.Date, schedules, schemas);

                        foreach (WorkTimeIntervalTO interval in intervals)
                        {
                            if (currDate.Date == from.Date && interval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                continue;

                            if (currDate.Date == to.Date.AddDays(1) && interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                continue;

                            int intervalDuration = (int)interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes;

                            if (interval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                intervalDuration++;

                            emplFundHrs[emplID] += intervalDuration / 60;
                        }
                    }
                }

                return emplFundHrs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void GetEmployeeShiftStartEnd(int emplID, DateTime date, WorkTimeIntervalTO intervalTO, List<WorkTimeIntervalTO> prevDayIntervals, WorkTimeIntervalTO nextDayInterval,
            WorkTimeSchemaTO sch, bool nightShift, bool nightShiftPrevious, int rounding, ref DateTime startTime, ref DateTime endTime)
        {
            try
            {
                startTime = intervalTO.StartTime;
                endTime = intervalTO.EndTime;

                TimeSpan shiftDuration = intervalTO.EndTime.TimeOfDay - intervalTO.StartTime.TimeOfDay;
                TimeSpan wholeShiftDuration = intervalTO.EndTime.TimeOfDay - intervalTO.StartTime.TimeOfDay;

                if (sch.Type.Trim() == Constants.schemaTypeFlexi || sch.Type.Trim() == Constants.schemaTypeNightFlexi)
                {
                    // Sanja 02.10.2013. get whole shift (two days intervals) duration for flexy third shifts
                    if (nightShift && intervalTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                    {
                        wholeShiftDuration += new TimeSpan(0, 1, 0);

                        if (nextDayInterval != null && nextDayInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                            wholeShiftDuration += nextDayInterval.EndTime.TimeOfDay - nextDayInterval.StartTime.TimeOfDay;
                    }

                    if (nightShiftPrevious && intervalTO.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                    {
                        foreach (WorkTimeIntervalTO prevInterval in prevDayIntervals)
                        {
                            if (prevInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                            {
                                wholeShiftDuration += (prevInterval.EndTime.TimeOfDay - prevInterval.StartTime.TimeOfDay).Add(new TimeSpan(0, 1, 0));
                            }
                        }
                    }

                    // Sanja 02.10.2013. getting first arrived time for third shifts begginings and regular shifts
                    DateTime firstArrivedTime = new DateTime();
                    List<int> emplList = new List<int>();
                    emplList.Add(emplID);
                    List<IOPairTO> dayPairs = new IOPair().SearchAll(date.Date, date.Date, emplList);
                    if (!nightShiftPrevious || intervalTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                    {
                        //firstArrivedTime = Common.Misc.roundTime(new IOPair().getFirstArrivedTime(emplID, date), rounding, false);                        
                        foreach (IOPairTO dayPair in dayPairs)
                        {
                            // skip pairs that finished before interval start
                            if (roundTime(dayPair.EndTime, rounding, true).TimeOfDay <= intervalTO.EarliestArrived.TimeOfDay)
                                continue;

                            firstArrivedTime = roundTime(dayPair.StartTime, rounding, false);
                            break;
                        }

                        if (firstArrivedTime != new DateTime() || !nightShift)
                        {
                            // if there is first arrived time or shift is 'regular'
                            if (firstArrivedTime.TimeOfDay >= intervalTO.EarliestArrived.TimeOfDay && firstArrivedTime.TimeOfDay <= intervalTO.LatestArrivaed.TimeOfDay)
                                startTime = firstArrivedTime;
                            else if (firstArrivedTime.TimeOfDay < intervalTO.EarliestArrived.TimeOfDay)
                            {
                                startTime = intervalTO.EarliestArrived;
                            }
                            else
                            {
                                startTime = intervalTO.LatestArrivaed;
                            }
                        }
                        else
                        {
                            // if there is no first arrival in processing date and shift is night shift, check if there is arrival in next day
                            List<IOPairTO> nextDayPairs = new IOPair().SearchAll(date.AddDays(1).Date, date.AddDays(1).Date, emplList);
                            foreach (IOPairTO nextDayPair in nextDayPairs)
                            {
                                if (roundTime(nextDayPair.EndTime, rounding, true).TimeOfDay <= nextDayInterval.EarliestArrived.TimeOfDay
                                    || roundTime(nextDayPair.StartTime, rounding, false).TimeOfDay >= nextDayInterval.LatestLeft.TimeOfDay)
                                    continue;

                                firstArrivedTime = roundTime(nextDayPair.StartTime, rounding, false);
                                break;
                            }

                            if (firstArrivedTime == new DateTime())
                            {
                                // if there is no arrival in next day, start is earliest arrive
                                //startTime = intervalTO.EarliestArrived;
                                startTime = intervalTO.StartTime;
                            }
                            else
                            {
                                // if there is arrival in next day, start is latest arrive
                                startTime = intervalTO.LatestArrivaed;
                            }
                        }

                        TimeSpan durationDayEnd = new TimeSpan(23, 59, 0) - startTime.TimeOfDay;

                        if (!nightShift)
                        {
                            if (durationDayEnd > shiftDuration)
                                endTime = startTime.AddMinutes(shiftDuration.TotalMinutes);
                            else
                                endTime = startTime.AddMinutes(durationDayEnd.TotalMinutes);
                        }
                        else
                        {
                            if (durationDayEnd > wholeShiftDuration)
                                endTime = startTime.AddMinutes(wholeShiftDuration.TotalMinutes);
                            else
                                endTime = startTime.AddMinutes(durationDayEnd.TotalMinutes);
                        }
                    }
                    else
                    {
                        // it is midnight interval                        
                        DateTime shiftStart = new DateTime();
                        WorkTimeIntervalTO prevDayInterval = new WorkTimeIntervalTO();
                        // get previous day midnight ending interval
                        foreach (WorkTimeIntervalTO prevInterval in prevDayIntervals)
                        {
                            if (prevInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                            {
                                prevDayInterval = prevInterval;
                                break;
                            }
                        }

                        DateTime firstArrivePrevDay = new DateTime();
                        // check if there is shift beggining in previous day
                        List<IOPairTO> prevDayPairs = new IOPair().SearchAll(date.AddDays(-1).Date, date.AddDays(-1).Date, emplList);
                        foreach (IOPairTO dayPair in prevDayPairs)
                        {
                            // skip pairs that finished before interval start
                            if (roundTime(dayPair.EndTime, rounding, true).TimeOfDay <= prevDayInterval.EarliestArrived.TimeOfDay)
                                continue;

                            firstArrivePrevDay = roundTime(dayPair.StartTime, rounding, false);
                            break;
                        }

                        if (firstArrivePrevDay != new DateTime())
                        {
                            if (firstArrivePrevDay.TimeOfDay >= prevDayInterval.EarliestArrived.TimeOfDay && firstArrivePrevDay.TimeOfDay <= prevDayInterval.LatestArrivaed.TimeOfDay)
                                shiftStart = firstArrivePrevDay;
                            else if (firstArrivePrevDay.TimeOfDay < prevDayInterval.EarliestArrived.TimeOfDay)
                            {
                                shiftStart = prevDayInterval.EarliestArrived;
                            }
                            else
                            {
                                shiftStart = prevDayInterval.LatestArrivaed;
                            }
                        }
                        else
                        {
                            // check if there are pairs in midnight interval
                            foreach (IOPairTO pair in dayPairs)
                            {
                                if (roundTime(pair.EndTime, rounding, true).TimeOfDay <= intervalTO.EarliestArrived.TimeOfDay
                                    || roundTime(pair.StartTime, rounding, false).TimeOfDay >= intervalTO.LatestLeft.TimeOfDay)
                                    continue;

                                shiftStart = roundTime(pair.StartTime, rounding, false);
                                break;
                            }

                            if (shiftStart != new DateTime())
                                shiftStart = prevDayInterval.LatestArrivaed;
                            else
                                shiftStart = prevDayInterval.StartTime;
                        }

                        TimeSpan durationDayEnd = (new TimeSpan(23, 59, 0) - shiftStart.TimeOfDay).Add(new TimeSpan(0, 1, 0));

                        if (shiftStart.TimeOfDay == new TimeSpan(23, 59, 0))
                            endTime = startTime.AddMinutes(wholeShiftDuration.TotalMinutes);
                        else if (durationDayEnd < wholeShiftDuration)
                            endTime = startTime.AddMinutes((wholeShiftDuration - durationDayEnd).TotalMinutes);
                        else
                            endTime = startTime;
                    }
                }
            }
            catch
            { }
        }

        public static string generatePDFDecisions(int company, string filePath, DateTime fromDate, DateTime toDate)
        {
            string generated = "";
            try
            {
                WorkingUnitTO unitCompany = new WorkingUnit().FindWU(company);


                List<DateTime> datesList = new List<DateTime>();

                DateTime dateFrom = fromDate;
                while (fromDate <= toDate)
                {
                    datesList.Add(fromDate.Date);
                    fromDate = fromDate.AddDays(1);
                }

                List<EmployeeTO> emplList = new List<EmployeeTO>();

                string workUnitID = company.ToString();
                List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();

                if (company != -1)
                {
                    List<WorkingUnitTO> plantWU = new WorkingUnit().SearchChildWU(company.ToString());
                    wUnits.AddRange(plantWU);
                    foreach (WorkingUnitTO plant in plantWU)
                    {
                        List<WorkingUnitTO> ccWU = new WorkingUnit().SearchChildWU(plant.WorkingUnitID.ToString());
                        wUnits.AddRange(ccWU);
                        foreach (WorkingUnitTO cc in ccWU)
                        {
                            List<WorkingUnitTO> workGroupWU = new WorkingUnit().SearchChildWU(cc.WorkingUnitID.ToString());
                            wUnits.AddRange(workGroupWU);
                            foreach (WorkingUnitTO workGroup in workGroupWU)
                            {
                                List<WorkingUnitTO> uteWU = new WorkingUnit().SearchChildWU(workGroup.WorkingUnitID.ToString());
                                wUnits.AddRange(uteWU);
                            }
                        }
                    }
                }
                string wUnitIds = "";
                foreach (WorkingUnitTO wu in wUnits)
                {
                    wUnitIds += wu.WorkingUnitID + ",";

                }
                if (wUnitIds.Length > 0)
                    wUnitIds = wUnitIds.Substring(0, wUnitIds.Length - 1);

                Dictionary<int, EmployeeTO> employees = new Dictionary<int, EmployeeTO>();
                List<EmployeeTO> currentEmployeesList = new Employee().SearchByWU(wUnitIds);
                string emplIDs = "";
                foreach (EmployeeTO empl in currentEmployeesList)
                {
                    emplIDs += empl.EmployeeID.ToString().Trim() + ",";
                    if (!employees.ContainsKey(empl.EmployeeID))
                        employees.Add(empl.EmployeeID, empl);
                }

                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                Dictionary<int, EmployeePositionTO> employeePostions = new EmployeePosition().SearchEmployeePositionsDictionary();
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, DateTime.Now.Date, DateTime.Now.Date, null);
                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary(emplIDs);
                Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema().getDictionary();
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rules = new Common.Rule().SearchTypeAllRules(Constants.RuleCompanyBankHour);

                Dictionary<int, Dictionary<string, RuleTO>> companyRules = new Dictionary<int, Dictionary<string, RuleTO>>();
                if (rules.ContainsKey(company))
                    companyRules = rules[company];

                List<IOPairProcessedTO> IOPairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, datesList, "");
                Dictionary<int, List<IOPairProcessedTO>> EmplIOPairs = new Dictionary<int, List<IOPairProcessedTO>>();
                foreach (IOPairProcessedTO pair in IOPairs)
                {

                    EmployeeTO Empl = new EmployeeTO();
                    if (employees.ContainsKey(pair.EmployeeID))
                        Empl = employees[pair.EmployeeID];

                    if (companyRules.ContainsKey(Empl.EmployeeTypeID) && companyRules[Empl.EmployeeTypeID].ContainsKey(Constants.RuleCompanyBankHour)
                        && companyRules[Empl.EmployeeTypeID][Constants.RuleCompanyBankHour].RuleValue == pair.PassTypeID)
                    {
                        if (!EmplIOPairs.ContainsKey(pair.EmployeeID))
                            EmplIOPairs.Add(pair.EmployeeID, new List<IOPairProcessedTO>());
                        EmplIOPairs[pair.EmployeeID].Add(pair);
                    }
                }

                iTextSharp.text.Document pdfDocCreatePDF = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 100, 100, 25, 20);

                //Because of UNICODE char.
                string sylfaenpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\sylfaen.ttf";
                iTextSharp.text.pdf.BaseFont sylfaen = iTextSharp.text.pdf.BaseFont.CreateFont(sylfaenpath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);
                iTextSharp.text.Font head = new iTextSharp.text.Font(sylfaen, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLUE);
                iTextSharp.text.Font normal = new iTextSharp.text.Font(sylfaen, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                iTextSharp.text.Font underline = new iTextSharp.text.Font(sylfaen, 10f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);
                iTextSharp.text.Font bold = new iTextSharp.text.Font(sylfaen, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

                iTextSharp.text.pdf.PdfWriter writerPdf = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDocCreatePDF, new FileStream(filePath, FileMode.Create));
                string responsible = "";

                if (company == -3)
                {
                    responsible = "EMILIO LO BIANCO";
                }
                else if (company == -4)
                {
                    responsible = "VLADIMIR MILOSAVLJEVIĆ";
                }
                else return "Res.person";

                string paragEn1 = " 2. Redistribution of working hours shall be done so that the total working hours of an employee for a period of six months during the calendar year, on average, not more than full time (40 hours per week). Accordingly, employees, based on redistribution in a 6 month period, in some period will work more than regular working time or less than regular working time.";
                string paragSr1 = " 2. Preraspodela radnog vremena vrši se tako da ukupno radno vreme zaposlenog u periodu od šest meseci u toku kalendarske godine u proseku ne bude duže od punog radnog vremena (40 časova nedeljno). Shodno tome, zaposleni će na osnovu izvršene preraspodele u periodu od 6 meseci, u skladu sapotrebama rada, raditi duže od punog radnog vremena, odnosno raditi kraće od punog radnog vremena.";
                string paragSr2 = " 3. U vremenu preraspodele zaposleni će dnevni i nedeljni odmor koristiti u skladu sa Zakonom o radu. Rad kod poslodavca se obavlja radnim danom, a po potrebi i subotom, dok je nedelja neradan dan.	";
                string paragEn2 = " 3. At the time of redistribution employees will use daily and weekly leave in accordance with the Labor Law. Work at the Employer is performing on working days, and if necessary, Saturday, and Sunday is non-working day";

                string paragEn3 = " 4. Redistribution of working hours is not considered as overtime work and employees  on that basis doesn’t have right for increased salary. An employee whose employment is terminated prior to the expiration of the period for which redistribution of working hours is performed, is entitled that overtime is recalculated as full-time working hours  which will be calculated also for retirement or that redistribution hours are calculated and paid as over time.";
                string paragSr3 = " 4. Preraspodela radnog vremena ne smatra se prekovremenim radom i zaposleni nemaju po tom osnovu pravo na uvećanu zaradu. Ukoliko zaposlenom radni odnos prestane pre isteka vremena za koje se vrši preraspodela radnog vremena ima pravo da mu se ostvareni časovi prekovremenog rada preračunaju u puno radno vreme i priznaju u penzijski staž ili da mu se računaju kao časovi rada dužeg od punog radnog vremena		";
                string paragSr4 = "Na osnovu člana 57 Zakona o radu (“Službeni glasnik RS” br. 24/05, 61/05, 54/09 i 32/13) i Pravilnika o radu privrednog društva “" + unitCompany.Description + "” Kragujevac, direktor  gdin. " + responsible + " društva donosi :";
                string paragEn4 = "Based on the article 57 of the Labour Law (“Official gazette RS” no. 24/05, 61/05, 54/09 and 32/13 ) and Rulebook of the company “" + unitCompany.Description + "” Kragujevac, director Mr. " + responsible + " of the company brings:";
                string paragSr5 = "U skladu sa potrebama procesa proizvodnje doneta je odluka kao u dispozitivu Rešenja.";
                string paragEn5 = "In accordance with the needs of the process of product the above mentioned decision has been reached.";
                string paragSr6 = "Pravna pouka: ";
                string paragEn6 = "Legal remedy: ";
                string paragSr7 = "Protiv ovog rešenja zaposleni može pokrenuti spor kod nadležnog suda u roku od 90 dana od dana donošenja rešenja, odnosno od dana saznanja.";
                string paragEn7 = "Against this decision employee may initiated proceedings before competent court within 90 days from the date making decision or from the date of knowledge.";
                string paragSr8 = "Za Poslodavca		                                                    _______________";
                string paragEn8 = "For Employer                                                          _______________";
                string paragSr9 = "Zaposleni     		                                                    _______________";
                string paragEn9 = "Employee                                                           _______________";
                string paragSr11 = "s tim da u vreme trajanja preraspodele ukupno radno vreme zaposlenog u toku preraspodele u proseku neće biti duže od 60 časova u radnoj nedelji.";
                string paragEn11 = "provided that at the time of redistribution of working hours for employees working hours will not exceed 60 hours in a workweek.";
                string timePeriodSr = "";
                string timePeriodEn = "";
                if (DateTime.Now.Date > new DateTime(DateTime.Now.Year, 6, 30).Date)
                {
                    timePeriodSr = "01.07." + DateTime.Now.ToString("yyyy") + " do 31.12." + DateTime.Now.ToString("yyyy");
                    timePeriodEn = "01.07." + DateTime.Now.ToString("yyyy") + " to 31.12." + DateTime.Now.ToString("yyyy");
                }
                else
                {
                    timePeriodSr = "01.01." + DateTime.Now.ToString("yyyy") + " do 30.6." + DateTime.Now.ToString("yyyy");
                    timePeriodEn = "01.01." + DateTime.Now.ToString("yyyy") + " to 30.6." + DateTime.Now.ToString("yyyy");
                }

                pdfDocCreatePDF.Open();
                foreach (int emplID in EmplIOPairs.Keys)
                {
                    if (writerPdf.PageNumber > 1 && writerPdf.PageNumber % 2 != 0)
                    {
                        pdfDocCreatePDF.NewPage();
                        pdfDocCreatePDF.Add(new Chunk());
                    }

                    pdfDocCreatePDF.NewPage();

                    EmployeeTO Empl = new EmployeeTO();
                    if (employees.ContainsKey(emplID))
                        Empl = employees[emplID];

                    List<EmployeeTimeScheduleTO> schedules = new List<EmployeeTimeScheduleTO>();
                    if (emplSchedules.ContainsKey(Empl.EmployeeID))
                        schedules = emplSchedules[Empl.EmployeeID];

                    WorkTimeSchemaTO sch = Common.Misc.getTimeSchema(dateFrom.Date, schedules, schemas);
                    DateTime firstWD = new DateTime(dateFrom.Year, dateFrom.Month, 1);
                    if (sch.TimeSchemaID != -1)
                    {
                        List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(firstWD.Date, schedules, schemas);
                        while (!Common.Misc.isWorkingDay(intervals))
                        {
                            firstWD = firstWD.AddDays(1);
                            intervals = Common.Misc.getTimeSchemaInterval(firstWD.Date, schedules, schemas);
                        }
                    }
                    string postionNameSr = "";
                    string postionNameEn = "";
                    if (ascoDict.ContainsKey(Empl.EmployeeID) && employeePostions.ContainsKey(ascoDict[Empl.EmployeeID].IntegerValue6))
                    {
                        postionNameSr = employeePostions[ascoDict[Empl.EmployeeID].IntegerValue6].PositionTitleSR;
                        postionNameEn = employeePostions[ascoDict[Empl.EmployeeID].IntegerValue6].PositionTitleEN;
                    }

                    string neki10 = "1.	Usled proizvodnih potreba, za zaposlenog " + Empl.FirstAndLastName + " na radnom mestu " + postionNameSr + ", u vremenskom periodu od " + timePeriodSr + " puno radno vreme od 40 časova u radnoj nedelji preraspoređuje se tako da će zaposleni u periodu od [" + dateFrom.Date.ToString("dd.MM.yyyy") + "] do [" + toDate.ToString("dd.MM.yyyy") + "] raditi duže od punog radnog vremena tokom nedelje kako je navedeno dalje, i to: ";
                    string nekiEngl10 = "1.	Due to production requirements employee " + Empl.FirstAndLastName + " on position " + postionNameEn + ", , in a time period from " + timePeriodEn + " full-time working hours of 40 hours in a workweek will be redistributed on the manner that at the period from [" + dateFrom.Date.ToString("dd.MM.yyyy") + "] to [" + toDate.ToString("dd.MM.yyyy") + "] working hours will be more than regular working hours per week as follows: ";

                    PdfPTable table;
                    PdfPCell cell;
                    iTextSharp.text.Paragraph paragraph;
                    iTextSharp.text.Paragraph paragraphEn;

                    table = new PdfPTable(2);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk(unitCompany.Description.ToUpper(), bold));
                    paragraphEn.Add(new Chunk(unitCompany.Description.ToUpper(), bold));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 10;
                    cell.PaddingRight = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 10;
                    cell.PaddingLeft = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk("KRAGUJEVAC", bold));
                    paragraphEn.Add(new Chunk("KRAGUJEVAC", bold));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 10;
                    cell.PaddingRight = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 10;
                    cell.PaddingLeft = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk("KOSOVSKA 4", bold));
                    paragraphEn.Add(new Chunk("KOSOVSKA 4", bold));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 10;
                    cell.PaddingRight = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 10;
                    cell.PaddingLeft = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk("DANA: " + firstWD.ToString("dd.MM.yyyy"), bold));
                    paragraphEn.Add(new Chunk("DANA: " + firstWD.ToString("dd.MM.yyyy"), bold));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 10;
                    cell.PaddingRight = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 10;
                    cell.PaddingLeft = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk("MESTO: KRAGUJEVAC", bold));
                    paragraphEn.Add(new Chunk("PLACE: KRAGUJEVAC", bold));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 10;
                    cell.PaddingRight = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 10;
                    cell.PaddingLeft = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk("BROJ: " + Empl.EmployeeID, bold));
                    paragraphEn.Add(new Chunk("NO: " + Empl.EmployeeID, bold));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 10;
                    cell.PaddingRight = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 10;
                    cell.PaddingLeft = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk(paragSr4, normal));
                    paragraphEn.Add(new Chunk(paragEn4, normal));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 20;
                    cell.PaddingRight = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 20;
                    cell.PaddingLeft = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk("REŠENJE", bold));
                    paragraphEn.Add(new Chunk("DECISION", bold));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingRight = 10;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingLeft = 10;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();

                    paragraph.Add(new Chunk(neki10, normal));
                    paragraphEn.Add(new Chunk(nekiEngl10, normal));

                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingLeft = 15;
                    cell.PaddingRight = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;

                    cell.PaddingLeft = 25;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    foreach (IOPairProcessedTO ioPair in EmplIOPairs[emplID])
                    {
                        string text = ioPair.IOPairDate.ToString("dd.MM.yyyy") + ", ";
                        text += Math.Round((ioPair.EndTime - ioPair.StartTime).TotalHours,2) + "h";

                        paragraph = new Paragraph();
                        paragraphEn = new Paragraph();
                        paragraph.Add(new Chunk(text, normal));
                        paragraphEn.Add(new Chunk(text, normal));
                        cell = new PdfPCell(paragraph);
                        cell.BorderWidth = 0;
                        cell.Padding = 0;
                        cell.PaddingTop = 12;
                        cell.PaddingRight = 10;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);
                        cell = new PdfPCell(paragraphEn);
                        cell.BorderWidth = 0;
                        cell.Padding = 0;
                        cell.PaddingTop = 12;
                        cell.PaddingLeft = 10;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);
                    }

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk(paragSr11, normal));
                    paragraphEn.Add(new Chunk(paragEn11, normal));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingRight = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingLeft = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk(paragSr1, normal));
                    paragraphEn.Add(new Chunk(paragEn1, normal));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingRight = 10;
                    cell.PaddingLeft = 15;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingLeft = 25;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk(paragSr2, normal));
                    paragraphEn.Add(new Chunk(paragEn2, normal));

                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingRight = 10;
                    cell.PaddingLeft = 15;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingLeft = 25;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk(paragSr3, normal));
                    paragraphEn.Add(new Chunk(paragEn3, normal));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingRight = 10;
                    cell.PaddingLeft = 15;

                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingLeft = 25;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk("OBRAZLOŽENJE", bold));
                    paragraphEn.Add(new Chunk("EXPLANATION", bold));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 50;
                    cell.PaddingRight = 10;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 50;
                    cell.PaddingLeft = 10;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);


                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk(paragSr5, normal));
                    paragraphEn.Add(new Chunk(paragEn5, normal));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingRight = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingLeft = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk(paragSr6, bold));
                    paragraphEn.Add(new Chunk(paragEn6, bold));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingRight = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingLeft = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk(paragSr7, normal));
                    paragraphEn.Add(new Chunk(paragEn7, normal));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingRight = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 12;
                    cell.PaddingLeft = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk(paragSr8, normal));
                    paragraphEn.Add(new Chunk(paragEn8, normal));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 40;
                    cell.PaddingRight = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 40;
                    cell.PaddingLeft = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    paragraph = new Paragraph();
                    paragraphEn = new Paragraph();
                    paragraph.Add(new Chunk(paragSr9, normal));
                    paragraphEn.Add(new Chunk(paragEn9, normal));
                    cell = new PdfPCell(paragraph);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 40;
                    cell.PaddingRight = 40;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    table.AddCell(cell);
                    cell = new PdfPCell(paragraphEn);
                    cell.BorderWidth = 0;
                    cell.Padding = 0;
                    cell.PaddingTop = 40;
                    cell.PaddingLeft = 10;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    table.SetWidthPercentage(new float[2] { 300f, 300f }, PageSize.A4);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;

                    pdfDocCreatePDF.Add(table);
                }

                pdfDocCreatePDF.Close();
                generated = ""; 
            }
            catch (Exception ex)
            {
                generated = ex.Message;
            }
            return generated;
        }

        public static DateTime GetMinChangingDate(int company, int emplType, int category, object conn)
        {
            try
            {
                DateTime minChangingDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;

                string cutOffType = Constants.RuleCutOffDate;

                if (category == (int)Constants.Categories.WCManager)
                    cutOffType = Constants.RuleWCDRCutOffDate;
                else if (category == (int)Constants.Categories.HRSSC)
                    cutOffType = Constants.RuleHRSSCCutOffDate;

                int cutOffDate = -1;

                Common.Rule rule;
                if (conn != null)
                    rule = new Common.Rule(conn);
                else
                    rule = new Common.Rule();
                rule.RuleTO.EmployeeTypeID = emplType;
                rule.RuleTO.WorkingUnitID = company;
                rule.RuleTO.RuleType = cutOffType;

                List<RuleTO> ruleList = rule.Search();

                if (ruleList.Count > 0)
                    cutOffDate = ruleList[0].RuleValue;

                int numWorkingDays = Common.Misc.countWorkingDays(DateTime.Now.Date, conn);
                if (numWorkingDays > 30)
                    Common.Misc.writeIncorrectChangingDate(DateTime.Now.ToString(Constants.dateFormat + " " + Constants.timeFormat) + " INCORRECT WORKING DAYS NUMBER: " + numWorkingDays.ToString());
                if (numWorkingDays <= cutOffDate)
                    minChangingDate = minChangingDate.AddMonths(-1).Date;

                return minChangingDate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
