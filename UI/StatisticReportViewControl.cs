using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;

namespace UI
{
    public partial class StatisticReportViewControl : UserControl
    {
        protected ResourceManager rm;
        protected CultureInfo culture;
        const int MarginGap = 10;

        private string _lblNameString;
        private int _plannedWorkingTimeMin;
        private int _realizeWorkingTimeMin;
        private List<WorkTimeSchemaTO> _timeSchemaList;
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private TimeSpan dayStart = new TimeSpan(0, 0, 0);
        private TimeSpan dayEnd = new TimeSpan(23, 59, 0);
        private List<HolidayTO> _holidays;
        private Dictionary<int, PassTypeTO> _passTypes;
        private bool _calculatePlanedAndRealizedWorkingTime;
        //planed working time in minutes
        public int PlannedWorkingTimeMin
        {
            get { return _plannedWorkingTimeMin; }
            set { _plannedWorkingTimeMin = value; }
        }
        //realized working time in minutes
        public int RealizeWorkingTimeMin
        {
            get { return _realizeWorkingTimeMin; }
            set { _realizeWorkingTimeMin = value; }
        }              
       //if value is true than calculate planed and realized working time
        public bool CalculatePlanedAndRealizedWorkingTime
        {
            get { return _calculatePlanedAndRealizedWorkingTime; }
            set { _calculatePlanedAndRealizedWorkingTime = value; }
        }
        //hashtable with all passTypes
        public Dictionary<int, PassTypeTO> PassTypes
        {
            get { return _passTypes; }
            set { _passTypes = value; }
        }
        //hashtable with all holydays
        public List<HolidayTO> Holidays
        {
            get { return _holidays; }
            set { _holidays = value; }
        }
        //end date for calculation
        public DateTime DateTo
        {
            get { return _dateTo; }
            set { _dateTo = value; }
        }
        //start date for calculation
        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { _dateFrom = value; }
        }
        //list of time schemas
        public List<WorkTimeSchemaTO> TimeSchemaList
        {
            get { return _timeSchemaList; }
            set { _timeSchemaList = value; }
        }
        //string for label lblName, showen on the graph
        public string GraphNameString
        {
            get { return _lblNameString; }
            set { _lblNameString = value; }
        }

        public StatisticReportViewControl()
        {
            InitializeComponent();
            PlannedWorkingTimeMin = 0;
            RealizeWorkingTimeMin = 0;
            rm = new ResourceManager("UI.Resource", typeof(EmployeePresenceGraphicReports).Assembly);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
        }

        protected void Resize_event(object o, EventArgs args)
        {
            try
            {
                setBounds();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected virtual void setBounds()
        {
            try
            {
                this.lblGraphName.Location = new System.Drawing.Point(5, 5);
                this.pccStatisticsView.SetBounds(5, 5, this.Width - 10, this.Height / 2 - 30);
                this.lvStatistics.SetBounds(5, this.Height / 2 + 53, this.Width - 10, this.Height / 2 - 55);
                this.lblPlaned.Location = new System.Drawing.Point(5, this.Height / 2 - 10);
                this.tbPlanned.Location = new System.Drawing.Point(85, this.Height / 2 - 15);
                this.lblRealized.Location = new System.Drawing.Point(5, this.Height / 2 + 25);
                this.tbRealized.Location = new System.Drawing.Point(85, this.Height / 2 + 23);
                this.lblRotation.Location = new Point((this.Width - numericUpDown1.Width - 55), this.Height / 2 - 10);
                this.numericUpDown1.Location = new Point((this.Width-numericUpDown1.Width-5), this.Height / 2 - 15);
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //calculate holes in working time for employees and days with IOPairs
        public int calculateHolesDuringWorkingTime(Dictionary<int, List<IOPairTO>> emplIOPairs, Dictionary<int, List<EmployeeTimeScheduleTO>> employeeTimeShedule, List<EmployeeTO> employees)
        {
            int holesDuringWorkingTime = 0;
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
                    if (!emplPairs.ContainsKey(employeeID))
                        continue;
                    if (!emplTimeSchedules.ContainsKey(employeeID))
                        continue;

                    for (DateTime day = this.DateFrom; day <= this.DateTo; day = day.AddDays(1))
                    {
                        //calculate holes only for employee's dates
                        bool is2DaysShift = false;
                        bool is2DaysShiftPrevious = false;
                        WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                        //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                        //are night shift days. If day is night shift day, also take first interval of next day
                        Dictionary<int, WorkTimeIntervalTO> dayIntervals = Common.Misc.getDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day, ref is2DaysShift,
                            ref is2DaysShiftPrevious, ref firstIntervalNextDay, this.TimeSchemaList);

                        if (dayIntervals == null)
                            continue;

                        //if previous day is night shift day, take that night shift interval
                        WorkTimeIntervalTO lastIntervalPreviousDay = new WorkTimeIntervalTO();
                        if (is2DaysShiftPrevious)
                            lastIntervalPreviousDay = ioPair.getLastIntervalPrevDay(emplTimeSchedules[employeeID], day, this.TimeSchemaList);

                        //if this day is night shift day, find index of night shift interval
                        int lastIntervalIndex = -1;
                        if (is2DaysShift)
                        {
                            for (int i = 0; i < dayIntervals.Count; i++)
                            {
                                WorkTimeIntervalTO currentTimeSchemaInterval = dayIntervals[i]; //key is interval_num which is 0, 1...
                                if ((currentTimeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                    && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                                {
                                    lastIntervalIndex = i;
                                    break;
                                }
                            }
                        }

                        //if previous day is night shift day, find index of interval in this day that is part of
                        //previous day night shift
                        int partOfNightShiftIndex = -1;
                        if (is2DaysShiftPrevious)
                        {
                            for (int i = 0; i < dayIntervals.Count; i++)
                            {
                                WorkTimeIntervalTO currentTimeSchemaInterval = dayIntervals[i]; //key is interval_num which is 0, 1...
                                if ((currentTimeSchemaInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                    && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                                {
                                    partOfNightShiftIndex = i;
                                    break;
                                }
                            }
                        }

                        //Take only IO Pairs for specific day, considering night shifts
                        List<IOPairTO> dayIOPairList = ioPair.getEmployeeExactDayPairs(emplPairs[employeeID], day, is2DaysShiftPrevious, lastIntervalPreviousDay);

                        if (dayIOPairList.Count <= 0)
                            continue;
                        /*
                        //do not calculate pause if day has at least one open interval
                        bool openPairExist = ioPair.existEmployeeDayOpenPairs(dayIOPairList);
                        if (openPairExist)
                            continue;
                        */
                        for (int i = 0; i < dayIntervals.Count; i++)
                        {
                            WorkTimeIntervalTO tsInterval = dayIntervals[i]; //key is interval_num which is 0, 1...
                            TimeSpan intervalDuration = (new DateTime(tsInterval.EndTime.TimeOfDay.Ticks - tsInterval.StartTime.TimeOfDay.Ticks)).TimeOfDay;

                            //if this is night shift day, skip night shift interval, it will 
                            //be calculated together with next day
                            if (is2DaysShift && (i == lastIntervalIndex))
                                continue;

                            //use pairs from previous day only if previous day is night shift
                            //and current interval is part of night shift
                            bool addPairsFromPrevDay = false;
                            if (is2DaysShiftPrevious && (lastIntervalPreviousDay != null)
                                && (i == partOfNightShiftIndex))
                                addPairsFromPrevDay = true;
                            //get only interval pairs, considering night shift
                            List<IOPairTO> intervalIoPairList = ioPair.getEmployeeIntervalPairs(dayIOPairList, tsInterval, lastIntervalPreviousDay,
                                addPairsFromPrevDay, day);
                            //if (intervalIoPairList.Count < 2)
                            //   continue;

                            List<IOPairTO> intervalIoPairListClone = new List<IOPairTO>();
                            foreach (IOPairTO iop in intervalIoPairList)                            
                            {
                                intervalIoPairListClone.Add(iop);
                            }

                            //make new IOPairsList (trimmedIoPairs) considering overlaping of intervals
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
                                    foreach (IOPairTO iopairTOTrimm in trimmedIoPairs)
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

                            TimeSpan ioPairsDuration = new TimeSpan(0);
                            foreach (IOPairTO ioPairTO in trimmedIoPairs)
                            {
                                ioPairsDuration += (new DateTime(ioPairTO.EndTime.TimeOfDay.Ticks - ioPairTO.StartTime.TimeOfDay.Ticks)).TimeOfDay;
                            }
                            if (intervalDuration.TotalMinutes > ioPairsDuration.TotalMinutes)
                            {
                                holesDuringWorkingTime += (int)(intervalDuration.TotalMinutes - ioPairsDuration.TotalMinutes);
                            }
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

        //for specific date and employee calculate extra hours and planed working time
        //if calculateExtraHours is falsecalculate just planed working time
        public int calculatePlanedWorkingTimeAndExtraHours(DateTime day, List<EmployeeTimeScheduleTO> timeSchedulesForEmpl, List<IOPairTO> ioPairsForEmpl, bool calculateExtraHours)
        {
            int extraHours = 0;
            try
            {
                if (ioPairsForEmpl.Count > 0)
                {
                    bool is2DaysShift = false;
                    bool is2DaysShiftPrevious = false;
                    WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                    //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                    //are night shift days. If day is night shift day, also take first interval of next day
                    Dictionary<int, WorkTimeIntervalTO> dayIntervals = Common.Misc.getDayTimeSchemaIntervals(timeSchedulesForEmpl, day, ref is2DaysShift,
                        ref is2DaysShiftPrevious, ref firstIntervalNextDay, this.TimeSchemaList);

                    if (dayIntervals != null)
                    {
                        //Find schema for using date, to see the type
                        WorkTimeSchemaTO dateUsedSchema = Common.Misc.getTimeSchemaForDay(timeSchedulesForEmpl, day, this.TimeSchemaList);

                        //Take only IO Pairs for specific day, considering night shifts
                        List<IOPairTO> dayIOPairList = Common.Misc.getEmployeeDayPairs(ioPairsForEmpl, day, is2DaysShift, is2DaysShiftPrevious, firstIntervalNextDay, dayIntervals);

                        //Check if day is holiday
                        HolidayTO currentDay = new HolidayTO();
                        foreach (HolidayTO holiday in this.Holidays)
                        {
                            if (holiday.HolidayDate.Date.Equals(day.Date))
                            {
                                currentDay = holiday;
                                break;
                            }
                        }
                        DateTime dt3 = DateTime.Now;
                        //calculate how many hours employee suppose to work on specific day
                        TimeSpan schedule = new TimeSpan(0);
                        //If current day is Holiday, it is not a working day, and how many hours
                        //suppose to work is zero
                        //It is checked only for non industrial schemas
                        if ((dateUsedSchema.Type.Trim() == Constants.schemaTypeIndustrial)
                            || (currentDay.HolidayDate == (new DateTime(0))))
                        {
                            //not a holiday

                            //Take only intervals for specific day, considering night shifts
                            List<WorkTimeIntervalTO> dayIntervalsList = Common.Misc.getEmployeeDayIntervals(is2DaysShift, is2DaysShiftPrevious, firstIntervalNextDay, dayIntervals);

                            // Trim pairs to the working time interval boundaries considering early/latency rules
                            // but only if type is not Flexi
                            if (dateUsedSchema.Type.Trim() != Constants.schemaTypeFlexi)
                                dayIOPairList = Common.Misc.trimDayPairs(dayIOPairList, dayIntervalsList);

                            schedule = Common.Misc.getEmployeeScheduleHourDay(dayIntervalsList);
                        }
                        if (calculateExtraHours)
                        {
                            //calculate how many hours employee did work on specific day
                            TimeSpan job = Common.Misc.getEmployeeJobHourDayTotal(dayIOPairList);

                            TimeSpan overtime = job - schedule;
                            if (overtime.TotalMinutes > 0)
                            {
                                extraHours += (int)overtime.TotalMinutes;
                            }
                        }
                        if (this.CalculatePlanedAndRealizedWorkingTime)
                        {
                            this.PlannedWorkingTimeMin += (int)schedule.TotalMinutes;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return extraHours;
        }

        //returns IOPairs for specific date
        public List<IOPairTO> getIOPairsForEmplDate(List<IOPairTO> ioPairsEmpl, DateTime date)
        {
            List<IOPairTO> ioPairsForEmplDateList = new List<IOPairTO>();
            try
            {
                foreach (IOPairTO ioPair in ioPairsEmpl)
                {
                    if (ioPair.StartTime.ToString("dd/MM/yyyy").Equals(date.ToString("dd/MM/yyyy")))
                    {
                        ioPairsForEmplDateList.Add(ioPair);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ioPairsForEmplDateList;
        }

        //returns list of intervals for specific employee and date
        //from list of time schedules and time schemas
        public List<WorkTimeIntervalTO> getTimeSchemaInterval(int employeeID, DateTime date, List<EmployeeTimeScheduleTO> timeScheduleListForEmployee)
        {
            List<WorkTimeIntervalTO> intervalList = new List<WorkTimeIntervalTO>();
            try
            {
                int timeScheduleIndex = -1;

                for (int scheduleIndex = 0; scheduleIndex < timeScheduleListForEmployee.Count; scheduleIndex++)
                {
                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
                    if (date >= timeScheduleListForEmployee[scheduleIndex].Date)
                    //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleListForEmployee[scheduleIndex])).Date.Month))
                    {
                        timeScheduleIndex = scheduleIndex;
                    }
                }

                if (timeScheduleIndex >= 0)
                {
                    int cycleDuration = 0;
                    int startDay = timeScheduleListForEmployee[timeScheduleIndex].StartCycleDay;
                    int schemaID = timeScheduleListForEmployee[timeScheduleIndex].TimeSchemaID;
                    List<WorkTimeSchemaTO> timeSchemaEmployee = new List<WorkTimeSchemaTO>();
                    foreach (WorkTimeSchemaTO timeSch in this.TimeSchemaList)
                    {
                        if (timeSch.TimeSchemaID == schemaID)
                        {
                            timeSchemaEmployee.Add(timeSch);
                        }
                    }
                    WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                    if (timeSchemaEmployee.Count > 0)
                    {
                        schema = timeSchemaEmployee[0];
                        cycleDuration = schema.CycleDuration;
                    }

                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
                    //TimeSpan days = date - ((EmployeesTimeSchedule) (timeScheduleListForEmployee[timeScheduleIndex])).Date;
                    //interval = (TimeSchemaInterval) ((Hashtable)(schema.Days[(startDay + days.Days) % cycleDuration]))[0];
                    TimeSpan days = new TimeSpan(date.Date.Ticks - timeScheduleListForEmployee[timeScheduleIndex].Date.Date.Ticks);

                    Dictionary<int, WorkTimeIntervalTO> table = schema.Days[(startDay + (int)days.TotalDays) % cycleDuration];
                    for (int i = 0; i < table.Count; i++)
                    {
                        intervalList.Add(table[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return intervalList;
        }

        protected void setStatiticsViewProperties()
        {
            try
            {
                this.pccStatisticsView.BackColor = Color.White;
                this.pccStatisticsView.LeftMargin = MarginGap;
                this.pccStatisticsView.RightMargin = MarginGap;
                this.pccStatisticsView.TopMargin = 18;
                this.pccStatisticsView.BottomMargin = 5;
                this.pccStatisticsView.FitChart = true;
                this.pccStatisticsView.SliceRelativeHeight = (float)0.20;
                this.pccStatisticsView.InitialAngle = 0;
                this.pccStatisticsView.EdgeLineWidth = (float)1.0;
                this.pccStatisticsView.EdgeColorType = System.Drawing.PieChart.EdgeColorType.DarkerThanSurface;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //set text for tbPlaned
        public void ShowPlannedWorkingTime()
        {
            try
            {
                double hours = (double)this.PlannedWorkingTimeMin/60;

                this.tbPlanned.Text = hours.ToString("F1"); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //set text for tbRealized
        public void ShowRealizedWorkingTime()
        {
            try
            {                
                double hours = (double)this.RealizeWorkingTimeMin/60;                                
                this.tbRealized.Text =hours.ToString("F1");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SetLanguage()
        {
            lvStatistics.BeginUpdate();
            lvStatistics.Columns.Add("", (this.lvStatistics.Width / 4) +30, HorizontalAlignment.Left);
            lvStatistics.Columns.Add("",10);
            lvStatistics.Columns.Add(rm.GetString("lvNumOfHours", culture), (this.lvStatistics.Width / 4) - 25, HorizontalAlignment.Right);
            lvStatistics.Columns.Add(rm.GetString("lvPercentOfRelized", culture), (this.lvStatistics.Width / 4) - 18, HorizontalAlignment.Right);
            lvStatistics.Columns.Add(rm.GetString("lvPercentOfPlaned", culture), (this.lvStatistics.Width / 4) - 18, HorizontalAlignment.Right);
            lvStatistics.EndUpdate();
            lvStatistics.Invalidate();
            lblPlaned.Text = rm.GetString("lblPlaned", culture);
            lblRealized.Text = rm.GetString("lblRealized", culture);
            lblRotation.Text = rm.GetString("lblRotation", culture);
            lblGraphName.Text = this.GraphNameString;
        }

        private void StatisticReportViewControl_Load(object sender, EventArgs e)
        {           
            setBounds();
            SetLanguage();
            //seting properties of control for drawing pie
            setStatiticsViewProperties();
            this.tbPlanned.Enabled = false;
            this.tbRealized.Enabled = false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.pccStatisticsView.InitialAngle = (float)this.numericUpDown1.Value;
        }       
    }
}
