using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;

using System.Data;
using System.Data.SqlClient;
using Common;
using Util;
using TransferObjects;
using ReaderInterface;

using System.Resources;
using System.Globalization;

namespace UI
{
    public partial class ExitPermControl : UserControl
    {
        //control layout param's
        const int rowHeight = 28;
        const int headerHeight = 51;
        public const int firstHeaderY = 15;
        public int currentLoc;
        const int headerX = 10;
        const int rowX = 185;

        public const string isStart = "IS_START";
        public const string isBeforeShift = "IS_BEFORE_SHIFT";
        public const string isShiftStart = "IS_SHIFT_START";
        public const string isShiftEnd = "IS_SHIFT_END";
        public const string isAfterShift = "IS_AFTER_SHIFT";

        bool isEmployeeControl;
        int listNum = 0;
        
        //selected pass type
        int passType;
        //list of all Pass types
        List<PassTypeTO> passTypeList;

        //param's form employee sorting
        //employee we whant to give permission's
        EmployeeTO employee;        
        //key is day, value is hashtable of intervals
        Dictionary<DateTime, Dictionary<int, WorkTimeIntervalTO>> emplDayIntervals;
        //key is day, value is array of pairs
        Dictionary<DateTime, List<IOPairTO>> emplDayIOPairs;
        //key is day, value is time schema
        Dictionary<DateTime, WorkTimeSchemaTO> emplDayTimeSchema;
        //selected days
        List<DateTime> daysList;

        //param's for days sorting        
        DateTime date;
        //key is employeeID, value is hashtable of intervals
        Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> emplIntervals;
        //key is employeeID, value is array of pairs
        Dictionary<int, List<IOPairTO>> emplIOPairs;
        //key is employeeID, value is time schema
        Dictionary<int, WorkTimeSchemaTO> emplTimeSchema;
        //selected days
        List<EmployeeTO> emplList;

        DebugLog log;
        ResourceManager rm;
        CultureInfo culture;

        public ArrayList permissionsList;
        string description;

        //currnet list of IOPairs for emloyee and date
        List<IOPairTO> currentIOpairs;

        //constructor if sorting is employee sorting
        public ExitPermControl(EmployeeTO empl, int type, Dictionary<DateTime, Dictionary<int, WorkTimeIntervalTO>> daysIntervals, Dictionary<DateTime, List<IOPairTO>> daysIOPairs, List<DateTime> days, List<PassTypeTO> passTypes, Dictionary<DateTime, WorkTimeSchemaTO> dayTimeShema, string desc)
        {
            InitializeComponent();
            permissionsList = new ArrayList();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(ExitPermissionsAddAdvanced).Assembly);
            
            this.gbExitPerm.Text = empl.FirstName + " " + empl.LastName;
            employee = empl;
            passType = type;
            emplDayIntervals = daysIntervals;
            emplDayIOPairs = daysIOPairs;
            emplDayTimeSchema = dayTimeShema;
            passTypeList = passTypes;
            daysList = days;

            this.emplIntervals = new Dictionary<int,Dictionary<int,WorkTimeIntervalTO>>();
            this.emplIOPairs = new Dictionary<int,List<IOPairTO>>();
            this.emplTimeSchema = new Dictionary<int,WorkTimeSchemaTO>();
            this.emplList = new List<EmployeeTO>();
            this.date =new DateTime();

            this.description = desc;
            currentLoc = firstHeaderY;
            isEmployeeControl = true;
            SetControl();
        }

        //construnctor for days sorting
        public ExitPermControl(DateTime day, int type, Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> emplIntervals, Dictionary<int, List<IOPairTO>> emplIOPairs, List<EmployeeTO> employees, List<PassTypeTO> passTypes, Dictionary<int, WorkTimeSchemaTO> timeShemas, string desc)
        {
            InitializeComponent();
            permissionsList = new ArrayList();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(ExitPermControl).Assembly);

            this.gbExitPerm.Text = rm.GetString("lblDay", culture) + " " + day.ToString("dd.MM.yyyy");
            employee = new EmployeeTO();
            passType = type;
            emplDayIntervals = new Dictionary<DateTime,Dictionary<int,WorkTimeIntervalTO>>();
            emplDayIOPairs = new Dictionary<DateTime,List<IOPairTO>>();
            emplDayTimeSchema = new Dictionary<DateTime,WorkTimeSchemaTO>();
            passTypeList = passTypes;
            daysList = new List<DateTime>();

            this.emplIntervals = emplIntervals;
            this.emplIOPairs = emplIOPairs;
            this.emplList = employees;
            this.emplTimeSchema = timeShemas;
            this.date = day;

            this.description = desc;
            currentLoc = firstHeaderY;
            isEmployeeControl = false;
            SetControl();
        }

        private void ExitPermControl_SizeChanged(object sender, EventArgs e)
        {
            gbExitPerm.Size = new Size(this.Width - 6, this.Height - 6);
        }

        private void  SetControl()
        {
            try
            {
                if (isEmployeeControl)
                {
                    foreach (DateTime date in daysList)
                    {
                        List<IOPairTO> holesList = new List<IOPairTO>();
                        List<IOPairTO> pairsList = new List<IOPairTO>();
                        IOPairTO hole = new IOPairTO();
                        bool manualCreatedExist = false;
                        bool openPairs = false;
                        List<IOPairTO> iopairsTO = new List<IOPairTO>();
                        if (emplDayIOPairs.ContainsKey(date))
                            iopairsTO = emplDayIOPairs[date];
                        if (iopairsTO != null)
                        {
                            Dictionary<int, WorkTimeIntervalTO> intervals = emplDayIntervals[date];
                            List<WorkTimeIntervalTO> intervalList = new List<WorkTimeIntervalTO>();
                            foreach (WorkTimeIntervalTO interval in intervals.Values)
                            {
                                intervalList.Add(interval);
                            }

                            currentIOpairs = new List<IOPairTO>();
                            foreach (IOPairTO ioPairTO in iopairsTO)
                            {
                                IOPairTO pair = new IOPairTO();
                                pair = ioPairTO;
                                if (pair.StartTime.Equals(new DateTime()) || pair.EndTime.Equals(new DateTime()))
                                {
                                    openPairs = true;
                                }
                                else if (pair.ManualCreated==(int)Constants.recordCreated.Manualy)
                                {
                                    manualCreatedExist = true;
                                }                                
                                else
                                {
                                    currentIOpairs.Add(pair);
                                }
                            }
                            WorkTimeSchemaTO timeSchema = emplDayTimeSchema[date];
                            //  trim first and last pair to the working time interval boundaries
                            // considering latency rules
                            TrimDayPairs(currentIOpairs, intervalList, date, timeSchema);
                            if (manualCreatedExist && currentIOpairs.Count <= 0)
                            {
                                foreach (IOPairTO ioPairTO in iopairsTO)
                                {
                                    //add header for each employee
                                    ExitPermHeaderControl header = new ExitPermHeaderControl(date, intervalList);
                                    header.manualyCreatedExist = rm.GetString("eraseManualyIOpairs", culture);
                                    header.Location = new Point(headerX, currentLoc);
                                    this.gbExitPerm.Controls.Add(header);
                                    currentLoc += headerHeight;
                                    this.Height += headerHeight;

                                    addRow(passTypeList, ioPairTO.PassTypeID, ioPairTO.StartTime, ioPairTO.EndTime, true, employee, date, ioPairTO.PassType, "", timeSchema,intervals);
                                }
                            }
                            else
                            {
                                List<IOPairTO> holesBetweenPairs = getHolesBetweenIOPairs(currentIOpairs);
                                if (intervalList.Count > 0)
                                {
                                    holesList = getHolesWithIntervals(intervalList, holesBetweenPairs);

                                    //last and first pair gap 
                                    IOPairTO firstPair = new IOPairTO();
                                    if (currentIOpairs.Count > 0)
                                    {
                                        firstPair = currentIOpairs[0];
                                    }
                                    IOPairTO lastPair = new IOPairTO();
                                    if (currentIOpairs.Count > 0)
                                    {
                                        lastPair = currentIOpairs[currentIOpairs.Count - 1];
                                    }
                                    foreach (WorkTimeIntervalTO interval in intervalList)
                                    {
                                        if (firstPair.IOPairID != -1 && firstPair.StartTime.TimeOfDay > interval.StartTime.TimeOfDay && firstPair.StartTime.TimeOfDay < interval.EndTime.TimeOfDay)
                                        {
                                            IOPairTO newHole = new IOPairTO();
                                            newHole.PassType = isStart;
                                            newHole.IOPairDate = firstPair.IOPairDate;
                                            newHole.StartTime = interval.StartTime;
                                            newHole.EndTime = firstPair.StartTime;
                                            List<IOPairTO> holes = new List<IOPairTO>();
                                            holes.Add(newHole);
                                            foreach (IOPairTO h in holesList)
                                            {
                                                holes.Add(h);
                                            }
                                            holesList = holes;
                                        }
                                        if (lastPair.IOPairID != -1 && (lastPair.EndTime.TimeOfDay > interval.StartTime.TimeOfDay && lastPair.EndTime.TimeOfDay < interval.EndTime.TimeOfDay))
                                        {
                                            IOPairTO newHole = new IOPairTO();
                                            newHole.IOPairDate = lastPair.IOPairDate;
                                            newHole.StartTime = lastPair.EndTime;
                                            newHole.EndTime = interval.EndTime;
                                            holesList.Add(newHole);
                                        }
                                    }
                                }
                                else
                                {
                                    holesList = holesBetweenPairs;
                                }

                                if (holesList.Count > 0)
                                {
                                    //add header for each employee
                                    ExitPermHeaderControl header = new ExitPermHeaderControl(date, intervalList);
                                    if (manualCreatedExist)
                                    {
                                        header.manualyCreatedExist = rm.GetString("manualyCreatedPairs", culture);
                                    }                                    
                                    if (openPairs)
                                    {
                                        header.manualyCreatedExist = rm.GetString("openIOPairsExist", culture);
                                    }
                                    header.Location = new Point(headerX, currentLoc);
                                    this.gbExitPerm.Controls.Add(header);
                                    currentLoc += headerHeight;
                                    this.Height += headerHeight;

                                    int prevIndex = -2;
                                    foreach (IOPairTO iopair in holesList)
                                    {
                                        prevIndex++;
                                        IOPairTO prevHole = new IOPairTO();
                                        if (prevIndex >= 0)
                                        {
                                            prevHole = holesList[prevIndex];
                                        }
                                        foreach (IOPairTO pair in currentIOpairs)
                                        {
                                            //if pair start's before hole start add pair first
                                            if ((iopair.StartTime.TimeOfDay >= pair.StartTime.TimeOfDay) && (prevIndex < 0 || prevHole.EndTime.TimeOfDay <= pair.StartTime.TimeOfDay) && iopair.IOPairDate.Date.Equals(pair.IOPairDate.Date))
                                            {
                                                addRow(passTypeList, pair.PassTypeID, pair.StartTime, pair.EndTime, true, employee, date, pair.PassType, "",timeSchema, intervals);
                                            }
                                        }
                                        if (openPairs)
                                        {
                                            addRow(passTypeList, passType, iopair.StartTime, iopair.EndTime, true, employee, date, "", iopair.PassType,timeSchema,intervals);
                                        }
                                        else
                                        {
                                            addRow(passTypeList, passType, iopair.StartTime, iopair.EndTime, false, employee, date, "", iopair.PassType,timeSchema,intervals);
                                        }
                                        foreach (IOPairTO pair in currentIOpairs)
                                        {
                                            if ((prevIndex == holesList.Count - 2) && ((pair.StartTime.TimeOfDay >= iopair.EndTime.TimeOfDay && pair.StartTime.TimeOfDay >= iopair.StartTime.TimeOfDay && iopair.IOPairDate.Date.Equals(pair.IOPairDate.Date)) || iopair.IOPairDate.AddDays(1).Date.Equals(pair.IOPairDate.Date)))
                                            {
                                                addRow(passTypeList, pair.PassTypeID, pair.StartTime, pair.EndTime, true, employee, date, pair.PassType, "",timeSchema,intervals);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                else
                {
                    foreach (EmployeeTO empl in emplList)
                    {
                        List<IOPairTO> holesList = new List<IOPairTO>();
                        List<IOPairTO> pairsList = new List<IOPairTO>();
                        bool manualCreatedExist = false;
                        bool openPairs = false;
                        IOPair hole = new IOPair();
                        List<IOPairTO> iopairsTO = new List<IOPairTO>();
                        if (emplIOPairs.ContainsKey(empl.EmployeeID))
                            iopairsTO = emplIOPairs[empl.EmployeeID];

                        if (iopairsTO.Count > 0)
                        {
                            Dictionary<int, WorkTimeIntervalTO> intervals = emplIntervals[empl.EmployeeID];
                            List<WorkTimeIntervalTO> intervalList = new List<WorkTimeIntervalTO>();
                            foreach (WorkTimeIntervalTO interval in intervals.Values)
                            {
                                intervalList.Add(interval);
                            }

                            currentIOpairs = new List<IOPairTO>();
                            foreach (IOPairTO ioPairTO in iopairsTO)
                            {
                                IOPairTO pair = new IOPairTO();
                                pair = ioPairTO;
                                if (pair.StartTime.Equals(new DateTime()) || pair.EndTime.Equals(new DateTime()))
                                {
                                    openPairs = true;
                                }
                                else if (pair.ManualCreated == (int)Constants.recordCreated.Manualy)
                                {
                                    manualCreatedExist = true;
                                }  
                                else
                                {
                                    currentIOpairs.Add(pair);
                                }
                            }
                            WorkTimeSchemaTO timeSchema = emplTimeSchema[empl.EmployeeID];
                            // trim first and last pair to the working time interval boundaries
                            // considering latency rules
                            TrimDayPairs(currentIOpairs, intervalList, date,timeSchema);
                            if (manualCreatedExist && currentIOpairs.Count <= 0)
                            {
                                foreach (IOPairTO ioPairTO in iopairsTO)
                                {
                                    //add header for each employee
                                    ExitPermHeaderControl header = new ExitPermHeaderControl(empl, intervalList);
                                    header.manualyCreatedExist = rm.GetString("eraseManualyIOpairs", culture);
                                    header.Location = new Point(headerX, currentLoc);
                                    this.gbExitPerm.Controls.Add(header);
                                    currentLoc += headerHeight;
                                    this.Height += headerHeight;

                                    addRow(passTypeList,ioPairTO.PassTypeID, ioPairTO.StartTime, ioPairTO.EndTime, true, employee, date, ioPairTO.PassType, "",timeSchema,intervals);
                                }
                            }
                            else
                            {
                                List<IOPairTO> holesBetweenPairs = getHolesBetweenIOPairs(currentIOpairs);
                                if (intervals.Count > 0)
                                {
                                    holesList = getHolesWithIntervals(intervalList, holesBetweenPairs);

                                    //last and first pair gap 
                                    IOPairTO firstPair = new IOPairTO();
                                    if (currentIOpairs.Count > 0)
                                    {
                                        firstPair = currentIOpairs[0];
                                    }
                                    IOPairTO lastPair = new IOPairTO();
                                    if (currentIOpairs.Count > 0)
                                    {
                                        lastPair = currentIOpairs[currentIOpairs.Count - 1];
                                    }
                                    foreach (WorkTimeIntervalTO interval in intervalList)
                                    {
                                        if (firstPair.IOPairID != -1 && firstPair.StartTime.TimeOfDay > interval.StartTime.TimeOfDay && firstPair.StartTime.TimeOfDay < interval.EndTime.TimeOfDay)
                                        {
                                            IOPairTO newHole = new IOPairTO();
                                            newHole.PassType = isStart;
                                            newHole.IOPairDate = firstPair.IOPairDate;
                                            newHole.StartTime = interval.StartTime;
                                            newHole.EndTime = firstPair.StartTime;
                                            List<IOPairTO> holes = new List<IOPairTO>();
                                            holes.Add(newHole);
                                            foreach (IOPairTO h in holesList)
                                            {
                                                holes.Add(h);
                                            }
                                            holesList = holes;
                                        }
                                        if (lastPair.IOPairID != -1 && (lastPair.EndTime.TimeOfDay > interval.StartTime.TimeOfDay && lastPair.EndTime.TimeOfDay < interval.EndTime.TimeOfDay))
                                        {
                                            IOPairTO newHole = new IOPairTO();
                                            newHole.IOPairDate = lastPair.IOPairDate;
                                            newHole.StartTime = lastPair.EndTime;
                                            newHole.EndTime = interval.EndTime;
                                            holesList.Add(newHole);
                                        }
                                    }
                                }
                                else
                                {
                                    holesList = holesBetweenPairs;
                                }

                                if (holesList.Count > 0)
                                {
                                    //add header for each day
                                    ExitPermHeaderControl header = new ExitPermHeaderControl(empl, intervalList);
                                    if (manualCreatedExist)
                                    {
                                        header.manualyCreatedExist = rm.GetString("manualyCreatedPairs", culture);
                                    }
                                    if (openPairs)
                                    {
                                        header.manualyCreatedExist = rm.GetString("openIOPairsExist", culture);
                                    }
                                    header.Location = new Point(headerX, currentLoc);
                                    this.gbExitPerm.Controls.Add(header);
                                    currentLoc += headerHeight;
                                    this.Height += headerHeight;

                                    int prevIndex = -2;
                                    foreach (IOPairTO iopair in holesList)
                                    {
                                        prevIndex++;
                                        IOPairTO prevHole = new IOPairTO();
                                        if (prevIndex >= 0)
                                        {
                                            prevHole = holesList[prevIndex];
                                        }
                                        foreach (IOPairTO pair in currentIOpairs)
                                        {
                                            if ((iopair.StartTime.TimeOfDay >= pair.StartTime.TimeOfDay) && (prevIndex < 0 || prevHole.EndTime.TimeOfDay <= pair.StartTime.TimeOfDay) && iopair.IOPairDate.Date.Equals(pair.IOPairDate.Date))
                                            {
                                                addRow(passTypeList, pair.PassTypeID, pair.StartTime, pair.EndTime, true, empl, date, pair.PassType, "", timeSchema, intervals);
                                            }
                                        }
                                        if (openPairs)
                                        {
                                            addRow(passTypeList, passType, iopair.StartTime, iopair.EndTime, true, employee, date, "", iopair.PassType, timeSchema, intervals);
                                        }
                                        else
                                        {
                                            addRow(passTypeList, passType, iopair.StartTime, iopair.EndTime, false, empl, date, "", iopair.PassType, timeSchema, intervals);
                                        }
                                        foreach (IOPairTO pair in currentIOpairs)
                                        {
                                            if ((prevIndex == holesList.Count - 2) && ((pair.StartTime.TimeOfDay >= iopair.EndTime.TimeOfDay && pair.StartTime.TimeOfDay >= iopair.StartTime.TimeOfDay && iopair.IOPairDate.Date.Equals(pair.IOPairDate.Date)) || iopair.IOPairDate.AddDays(1).Date.Equals(pair.IOPairDate.Date)))
                                            {
                                                addRow(passTypeList, pair.PassTypeID, pair.StartTime, pair.EndTime, true, empl, date, pair.PassType, "", timeSchema, intervals);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermControl.ExitPermControl_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void TrimDayPairs(List<IOPairTO> ioPairs, List<WorkTimeIntervalTO> dayIntervals, DateTime day, WorkTimeSchemaTO timeSchema)
        {
            try
            {
                if (ioPairs.Count > 0 && dayIntervals.Count > 0)
                {
                    // find first and last pair
                    IOPairTO firstPair = ioPairs[0];
                    IOPairTO lastPair = firstPair;
                    foreach (IOPairTO iopair in ioPairs)
                    {
                        if (iopair.StartTime < firstPair.StartTime) firstPair = iopair;
                        if (iopair.EndTime > lastPair.EndTime) lastPair = iopair;
                    }
                    if (timeSchema.Type.Equals(Constants.schemaTypeFlexi))
                    {
                        WorkTimeIntervalTO interval = dayIntervals[0];
                        if (firstPair.StartTime.TimeOfDay < interval.LatestArrivaed.TimeOfDay && firstPair.StartTime.TimeOfDay > interval.EarliestArrived.TimeOfDay)
                        {
                            TimeSpan duration = new TimeSpan(interval.EndTime.TimeOfDay.Ticks - interval.StartTime.TimeOfDay.Ticks);
                            interval.StartTime = firstPair.StartTime;
                            interval.EndTime = firstPair.StartTime.Add(duration);
                        }
                    }
                    else
                    {
                        // round up first pair start time to full hour, considering start tolerance
                        if (firstPair.StartTime.TimeOfDay <= dayIntervals[0].LatestArrivaed.TimeOfDay && firstPair.StartTime.TimeOfDay >= dayIntervals[0].EarliestArrived.TimeOfDay)
                        {
                            firstPair.StartTime = firstPair.StartTime.Add(firstPair.StartTime.TimeOfDay.Negate() + dayIntervals[0].StartTime.TimeOfDay);
                        }

                        // round down last pair end time to full hour, considering end tolerance
                        if (lastPair.EndTime.TimeOfDay >= dayIntervals[0].EarliestLeft.TimeOfDay && lastPair.EndTime.TimeOfDay <= dayIntervals[0].LatestLeft.TimeOfDay)
                        {
                            lastPair.EndTime = lastPair.EndTime.Add(lastPair.EndTime.TimeOfDay.Negate() + dayIntervals[0].EndTime.TimeOfDay);
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermControl.TrimDayPairs(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        //from list of holes between pairs and list of intervals get new list of holes
        //if interval start or end in hole make two holes 
        private List<IOPairTO> getHolesWithIntervals(List<WorkTimeIntervalTO> intervalList, List<IOPairTO> holesBetweenPairs)
        {
            List<IOPairTO> holesWithIntervals = new List<IOPairTO>();
            IOPairTO hole = new IOPairTO();
            try
            {
                foreach (WorkTimeIntervalTO interval in intervalList)
                {
                    for (int i = 0; i < holesBetweenPairs.Count; i++)
                    {

                        IOPairTO pairHole = holesBetweenPairs[i];
                        if ((pairHole.StartTime.TimeOfDay < interval.StartTime.TimeOfDay) && (pairHole.EndTime.TimeOfDay > interval.StartTime.TimeOfDay))
                        {
                            hole = new IOPairTO();
                            hole.PassType = isBeforeShift;
                            hole.IOPairDate = pairHole.IOPairDate;
                            hole.StartTime = pairHole.StartTime;
                            hole.EndTime = interval.StartTime;
                            holesBetweenPairs[i] = hole;

                            IOPairTO newHole = new IOPairTO();
                            newHole.PassType = isShiftStart;
                            newHole.IOPairDate = pairHole.IOPairDate;
                            newHole.StartTime = interval.StartTime;
                            newHole.EndTime = pairHole.EndTime;
                            
                            for (int j = i + 1; j < holesBetweenPairs.Count; j++)
                            {
                                IOPairTO nextHole = holesBetweenPairs[j];
                                holesBetweenPairs[j] = newHole;
                                newHole = nextHole;                                
                            }

                            holesBetweenPairs.Add(newHole);
                        }
                        else if ((pairHole.StartTime.TimeOfDay < interval.EndTime.TimeOfDay) && (pairHole.EndTime.TimeOfDay > interval.EndTime.TimeOfDay))
                        {
                            hole = new IOPairTO();
                            hole.PassType = isShiftEnd;
                            hole.IOPairDate = pairHole.IOPairDate;
                            hole.StartTime = pairHole.StartTime;
                            hole.EndTime = interval.EndTime;
                            holesBetweenPairs[i] = hole;

                            IOPairTO newHole = new IOPairTO();
                            newHole.PassType = isAfterShift; 
                            newHole.IOPairDate = pairHole.IOPairDate;
                            newHole.StartTime = interval.EndTime;
                            newHole.EndTime = pairHole.EndTime;
                            for (int j = i + 1; j < holesBetweenPairs.Count; j++)
                            {
                                IOPairTO nextHole = holesBetweenPairs[j];
                                holesBetweenPairs[j] = newHole;
                                newHole = nextHole;
                            }

                            holesBetweenPairs.Add(newHole);
                        }                       
                    }
                }

                holesWithIntervals = holesBetweenPairs;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermControl.getHolesWithIntervals(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return holesWithIntervals;
        }

        private void addRow(List<PassTypeTO> passTypes, int passType, DateTime from, DateTime to, bool disable, EmployeeTO empl, DateTime date, string ptString, string permType, WorkTimeSchemaTO timeSchema, Dictionary<int, WorkTimeIntervalTO> intervals)
        {
            try
            {
                //if time schema is flexi find earliestArrive and LatestArrive
                // start time basic on ioPairs 
                DateTime earliestArrivalTime = new DateTime();
                DateTime latestArrivalTime = new DateTime();
                DateTime start = new DateTime(1, 1, 1, 23, 59, 59);
                if (timeSchema.Type.Equals(Constants.schemaTypeFlexi) && (permType.Equals(isShiftStart)||permType.Equals(isStart)))
                {
                    foreach (int intNum in intervals.Keys)
                    {
                        WorkTimeIntervalTO interval = intervals[intNum];

                        // if flexi time schema set start time for the permission to the Latest Arrived time of the schema <--> Darko 4.12.2007.
                        // if flexi let costumer choose start time for permission <--> Natasa 23.01.2009.

                        foreach (IOPairTO iopair in currentIOpairs)
                        {
                            if (!interval.StartTime.TimeOfDay.Equals(new DateTime(0).TimeOfDay) && !interval.EndTime.TimeOfDay.Equals(new DateTime(0).TimeOfDay)
                                && iopair.EndTime.TimeOfDay > interval.EarliestLeft.TimeOfDay && iopair.EndTime.TimeOfDay < interval.LatestLeft.TimeOfDay)
                            {
                                start = new DateTime(iopair.EndTime.Ticks - (interval.EndTime.Ticks - interval.StartTime.Ticks));
                            }
                            if (iopair.EndTime.TimeOfDay > interval.LatestLeft.TimeOfDay && iopair.StartTime.TimeOfDay < interval.LatestLeft.TimeOfDay)
                            {
                                if (!intervals[intNum].LatestArrivaed.TimeOfDay.Equals(new DateTime(0).TimeOfDay)
                            && intervals[intNum].LatestArrivaed.TimeOfDay < start.TimeOfDay)
                                {
                                    start = intervals[intNum].LatestArrivaed;
                                }
                            }
                        }
                        if (!intervals[intNum].StartTime.TimeOfDay.Equals(new DateTime(0).TimeOfDay))
                        {
                            earliestArrivalTime = new DateTime(date.Year,date.Month,date.Day,intervals[intNum].EarliestArrived.Hour,intervals[intNum].EarliestArrived.Minute,0);
                            latestArrivalTime = new DateTime(date.Year, date.Month, date.Day, intervals[intNum].LatestArrivaed.Hour, intervals[intNum].LatestArrivaed.Minute, 0);
                        }
                        if (start.Equals(new DateTime(1, 1, 1, 23, 59, 59)))
                        {
                            if (!intervals[intNum].StartTime.TimeOfDay.Equals(new DateTime(0).TimeOfDay)
                            && intervals[intNum].StartTime.TimeOfDay < start.TimeOfDay)
                            {
                                start = intervals[intNum].StartTime;
                            }
                        }
                    }
                    if (!start.Equals(new DateTime(1, 1, 1, 23, 59, 59)))
                    {
                        start = new DateTime(date.Year, date.Month, date.Day, start.Hour, start.Minute, 0);
                    }
                    else start = from;
                       
                }
                ExitPermRowControl row = new ExitPermRowControl(passTypes, passType, from, to, disable,empl,date,ptString,permType,this,listNum, description,timeSchema, earliestArrivalTime,latestArrivalTime);
                if (timeSchema.Type.Equals(Constants.schemaTypeFlexi) && (permType.Equals(isShiftStart) || permType.Equals(isStart)))
                { 
                     row.Start = start;
                }
                row.Location = new Point(rowX, currentLoc);
                this.gbExitPerm.Controls.Add(row);
                currentLoc += rowHeight;
                this.Height += rowHeight;
                //if (!disable)
                //{
                //    row.insert = false;
                //}
                permissionsList.Add(row);
                listNum++;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermControl.addRow(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        bool isNightShiftDay(List<WorkTimeIntervalTO> dayIntervals)
        {
            // see if the day is a night shift day (contains intervals starting with 00:00 and/or finishing with 23:59)
            foreach (WorkTimeIntervalTO dayInterval in dayIntervals)
            {                
                if (((dayInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0)) ||
                    (dayInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))) &&
                    (dayInterval.EndTime > dayInterval.StartTime))
                {
                    return true;
                }
            }
            return false;
        }

        //returns holes between ioPairs, list of IOPairs has at least two pairs
        //pairs are sorted
        //hole is IOPair object with pairID = -1, and start and end time
        List<IOPairTO> getHolesBetweenIOPairs(List<IOPairTO> iopairs)
        {
            List<IOPairTO> holesList = new List<IOPairTO>();
            IOPairTO hole = new IOPairTO();
            try
            { 
                 int nextIndex = 0;
                 foreach (IOPairTO pair in iopairs)
                 {
                     IOPairTO nextPair = new IOPairTO();
                     nextIndex++;
                     if (iopairs.Count > nextIndex)
                     {
                         nextPair = iopairs[nextIndex];

                         //if bouth pairs have same date 
                         //and if current pair ends before next pair begins add hole to the list
                         if (pair.IOPairDate.Date.Equals( nextPair.IOPairDate.Date)
                             && (pair.EndTime.TimeOfDay < nextPair.StartTime.TimeOfDay))
                         {
                             hole = new IOPairTO();
                             hole.IOPairDate = pair.IOPairDate;
                             hole.StartTime = pair.EndTime;
                             hole.EndTime = nextPair.StartTime;
                             holesList.Add(hole);
                         }
                         //if this two pairs are for night shift add two holes from first pair end to midnight
                         //and from midnight to second pair start
                         if (!pair.IOPairDate.Date.Equals(nextPair.IOPairDate.Date))
                         {
                             hole = new IOPairTO();
                             hole.IOPairDate = pair.IOPairDate;
                             hole.StartTime = pair.EndTime;
                             hole.EndTime = nextPair.StartTime;
                             holesList.Add(hole);                            
                         }
                     }
                 }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermControl.getHolesBetweenIOPairs(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return holesList;
        }

        public void checkingControl(int numInList)
        {
            if (permissionsList.Count > numInList)
            {
                ExitPermRowControl row = (ExitPermRowControl)permissionsList[numInList];
                if (row.PermissionType.Equals(isShiftStart) && !row.insert && numInList > 0)
                {
                    ExitPermRowControl rowPrev = (ExitPermRowControl)permissionsList[numInList - 1];
                    rowPrev.changeCheck(false);
                }
                else if (row.PermissionType.Equals(isBeforeShift) && row.insert && permissionsList.Count > numInList)
                {
                    ExitPermRowControl rowNext = (ExitPermRowControl)permissionsList[numInList + 1];
                    rowNext.changeCheck(true);
                }
                else if (row.PermissionType.Equals(isAfterShift) && row.insert && numInList > 0)
                {
                    ExitPermRowControl rowPrev = (ExitPermRowControl)permissionsList[numInList - 1];
                    rowPrev.changeCheck(true);
                }
                else if (row.PermissionType.Equals(isShiftEnd) && !row.insert && permissionsList.Count > numInList)
                {
                    ExitPermRowControl rowNext = (ExitPermRowControl)permissionsList[numInList + 1];
                    rowNext.changeCheck(false);
                }
            }
        }
    }
}
