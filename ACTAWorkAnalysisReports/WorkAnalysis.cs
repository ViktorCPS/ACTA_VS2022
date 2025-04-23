using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using Common;
using DataAccess;
using System.Threading;
using TransferObjects;
using System.IO;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security.Principal;
using System.Security;
using System.Configuration;
using System.Globalization;

namespace ACTAWorkAnalysisReports
{
    public class WorkAnalysis
    {

        private class AnomalyCategory
        {
            private string name = "";
            private int id = -1;

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public int ID
            {
                get { return id; }
                set { id = value; }
            }

            public AnomalyCategory(string name, int id)
            {
                this.Name = name;
                this.ID = id;
            }
        }

        DebugLog debug;
        private static WorkAnalysis managerInstance;
        DAOFactory daoFactory = null;
        public NotificationController Controller;
        private bool isProcessing = false;
        private Thread processManagerThread;
        string userNameSharedArea = "";
        string passwordSharedArea = "";
        string day = "";
        string timeMontly = "";                
        string timeDaily = "";        
        string batDirectory = "";
        Dictionary<string, string> dictionaryMonths = new Dictionary<string, string>();

        //pass allow reports generating start time in ticks
        private DateTime nextPassAllowTime = new DateTime();
        private int passAllowTimeout = 1440;
        private int dayAdvance = 0;

        // ISSE day and start time
        private DateTime nextISSETime = new DateTime();
        private string dayISSE = "";

        public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; }
        }

        DateTime lastExecuteBatch = new DateTime();
        
        protected WorkAnalysis()
        {

            NotificationController.SetApplicationName("ACTAWorkAnalysisService");
            debug = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");
            try
            {
                daoFactory = DAOFactory.getDAOFactory();
                InitializeObserverClient();
                ReadAppSettings();
                dictionaryMonths.Add("January", "Januar");
                dictionaryMonths.Add("February", "Februar");
                dictionaryMonths.Add("March", "Mart");
                dictionaryMonths.Add("April", "April");
                dictionaryMonths.Add("May", "Maj");
                dictionaryMonths.Add("June", "Jun");
                dictionaryMonths.Add("July", "Jul");
                dictionaryMonths.Add("August", "Avgust");
                dictionaryMonths.Add("September", "Septembar");
                dictionaryMonths.Add("October", "Oktobar");
                dictionaryMonths.Add("November", "Novembar");
                dictionaryMonths.Add("December", "Decembar");
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WorkAnalysis(): " + ex.Message + "\n");
            }
        }

        protected WorkAnalysis(bool isSchedule)
        {

            NotificationController.SetApplicationName("ACTAWorkAnalysisReports");
            debug = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");
            try
            {
                daoFactory = DAOFactory.getDAOFactory();
                InitializeObserverClient();
                dictionaryMonths.Add("January", "Januar");
                dictionaryMonths.Add("February", "Februar");
                dictionaryMonths.Add("March", "Mart");
                dictionaryMonths.Add("April", "April");
                dictionaryMonths.Add("May", "Maj");
                dictionaryMonths.Add("June", "Jun");
                dictionaryMonths.Add("July", "Jul");
                dictionaryMonths.Add("August", "Avgust");
                dictionaryMonths.Add("September", "Septembar");
                dictionaryMonths.Add("October", "Oktobar");
                dictionaryMonths.Add("November", "Novembar");
                dictionaryMonths.Add("December", "Decembar");
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WorkAnalysis(): " + ex.Message + "\n");
            }
        }

        private void InitializeObserverClient()
        {
            Controller = NotificationController.GetInstance();
        }

        public static WorkAnalysis GetInstance()
        {
            try
            {
                if (managerInstance == null)
                {
                    managerInstance = new WorkAnalysis();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return managerInstance;
        }

        public static WorkAnalysis GetInstance(bool isSchedule)
        {
            try
            {
                if (managerInstance == null)
                {
                    managerInstance = new WorkAnalysis(isSchedule);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return managerInstance;
        }

        private void executeBatch()
        {
            try
            {
                debug.writeLog("---START EXECUTEBATCH() " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                //execute the batch file

                debug.writeLog(batDirectory.Trim());

                debug.writeLog(File.Exists(batDirectory.Trim()).ToString().Trim());

                System.Diagnostics.ProcessStartInfo procinfo = new ProcessStartInfo(batDirectory);
                procinfo.UseShellExecute = false;
                procinfo.RedirectStandardError = true;
                procinfo.RedirectStandardInput = true;
                procinfo.RedirectStandardOutput = true;
                string argument1 = DateTime.Now.AddMonths(-1).ToString("yyyy_MM");
                string argument2 = DateTime.Now.ToString("yyyy_MM");
                procinfo.Arguments = string.Format("{0} {1}", argument1, argument2);

                System.Diagnostics.Process process = System.Diagnostics.Process.Start(procinfo);
                process.Start();
                debug.writeLog("---END EXECUTEBATCH() " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "executeBatch() " + ex.Message);
            }
        }

        private int returnMonthDays(int month, bool prev)
        {
            int i = 0;
            if (prev)
            {

                DateTime date = new DateTime();

                if (month - 1 == 0)
                    date = new DateTime(DateTime.Now.Year - 1, 12, 1, 0, 0, 0);
                else
                    date = new DateTime(DateTime.Now.Year, month - 1, 1, 0, 0, 0);
                DateTime now = new DateTime(DateTime.Now.Year, month, 1, 0, 0, 0);
                while (date < now)
                {
                    i++;
                    date = date.AddDays(1);
                }
            }
            else
            {
                DateTime date = new DateTime();
                if (month + 1 == 13)
                    date = new DateTime(DateTime.Now.Year + 1, 1, 1, 0, 0, 0);
                else
                    date = new DateTime(DateTime.Now.Year, month, 1, 0, 0, 0);
                DateTime now = new DateTime(DateTime.Now.Year, month + 1, 1, 0, 0, 0);
                while (date < now)
                {
                    i++;
                    date = date.AddDays(1);
                }
            }
            return i;
        }

        private void returnMonthDays()
        {
            int i = 0;
            DateTime date = new DateTime();
            if (DateTime.Now.Month - 1 == 0)
                date = new DateTime(DateTime.Now.Year - 1, 12, 1, 0, 0, 0);
            else
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1, 0, 0, 0);
            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            while (date < now)
            {
                i++;
                date = date.AddDays(1);
            }
            //daysInMonth = i;
        }

        private List<DateTime> returnPreviousWeek()
        {
            int numOfDaysPrevious = returnMonthDays(DateTime.Now.Month, true);
            List<DateTime> datesList = new List<DateTime>();
            DateTime day = new DateTime();
            DateTime dayOne = new DateTime();
            int num = 0;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                if (DateTime.Now.Day - 7 <= 0)
                {
                    num = 7 - DateTime.Now.Day;
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 7, 0, 0, 0);


                dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                while (day < dayOne)
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
            {
                if (DateTime.Now.Day - 8 <= 0)
                {
                    num = 8 - DateTime.Now.Day;
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 8, 0, 0, 0);

                if (DateTime.Now.Day - 1 <= 0)
                {
                    num = 1 - DateTime.Now.Day;
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1, 0, 0, 0);
                while (day < dayOne)
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
            {
                if (DateTime.Now.Day - 9 <= 0)
                {
                    num = 9 - DateTime.Now.Day;
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 9, 0, 0, 0);

                if (DateTime.Now.Day - 2 <= 0)
                {
                    num = 2 - DateTime.Now.Day;
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 2, 0, 0, 0);
                while (day < dayOne)
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
            {
                if (DateTime.Now.Day - 10 <= 0)
                {
                    num = 10 - DateTime.Now.Day;
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 10, 0, 0, 0);

                if (DateTime.Now.Day - 3 <= 0)
                {
                    num = 3 - DateTime.Now.Day;
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 3, 0, 0, 0);
                while (day < dayOne)
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                if (DateTime.Now.Day - 11 <= 0)
                {
                    num = 11 - DateTime.Now.Day;
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 11, 0, 0, 0);

                if (DateTime.Now.Day - 4 <= 0)
                {
                    num = 4 - DateTime.Now.Day;
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 4, 0, 0, 0);
                while (day < dayOne)
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                if (DateTime.Now.Day - 12 <= 0)
                {
                    num = 12 - DateTime.Now.Day;
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 12, 0, 0, 0);
                if (DateTime.Now.Day - 5 <= 0)
                {
                    num = 5 - DateTime.Now.Day;
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 5, 0, 0, 0);
                while (day < dayOne)
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                if (DateTime.Now.Day - 13 <= 0)
                {
                    num = 13 - DateTime.Now.Day;
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 13, 0, 0, 0);
                if (DateTime.Now.Day - 6 <= 0)
                {
                    num = 6 - DateTime.Now.Day;
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, numOfDaysPrevious - num, 0, 0, 0);
                }
                else
                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 6, 0, 0, 0);
                while (day < dayOne)
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }
            }
            return datesList;
        }

        private bool ifMonthly()
        {
            int i = 1;
            DateTime first = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            while (i < 7)
            {
                if (first.DayOfWeek != DayOfWeek.Saturday && first.DayOfWeek != DayOfWeek.Sunday)
                    i++;
                first = first.AddDays(1);
            }
            DateTime seventhDay = first;
            DateTime secondDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 2, 0, 0, 0);
            if (secondDay.DayOfWeek == DayOfWeek.Monday)
                secondDay = secondDay.AddDays(1);
            if (secondDay.DayOfWeek == DayOfWeek.Saturday)
                secondDay = secondDay.AddDays(2);
            if (secondDay.DayOfWeek == DayOfWeek.Sunday)
                secondDay = secondDay.AddDays(2);

            //if ((DateTime.Now.Day == secondDay.Day || DateTime.Now.Day == seventhDay.Day) && timeMontly == DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm"))
            if (DateTime.Now.Day == secondDay.Day || DateTime.Now.Day == seventhDay.Day)
                return true;
            else
                return false;
        }

        private bool ifWeekly()
        {
            bool isWeek = false;
            //if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday && timeMontly == DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm"))
            if (DateTime.Now.DayOfWeek.ToString() == day)
                isWeek = true;

            return isWeek;
        }

        private bool ifISSE()
        {
            if (DateTime.Now > nextISSETime)
                return true;
            else
                return false;
        }

        private void ReadAppSettings()
        {
            if (ConfigurationManager.AppSettings["userNameSharedArea"] != null)
            {
                userNameSharedArea = (string)ConfigurationManager.AppSettings["userNameSharedArea"];
            }
            if (ConfigurationManager.AppSettings["passwordSharedArea"] != null)
            {
                passwordSharedArea = (string)ConfigurationManager.AppSettings["passwordSharedArea"];
            }
            if (ConfigurationManager.AppSettings["day"] != null)
            {
                day = (string)ConfigurationManager.AppSettings["day"];
            }
            if (ConfigurationManager.AppSettings["timeMontly"] != null)
            {
                timeMontly = (string)ConfigurationManager.AppSettings["timeMontly"];
            }
            if (ConfigurationManager.AppSettings["dayISSE"] != null)
            {
                dayISSE = (string)ConfigurationManager.AppSettings["dayISSE"];
            }            
            if (ConfigurationManager.AppSettings["timeDaily"] != null)
            {
                timeDaily = (string)ConfigurationManager.AppSettings["timeDaily"];
            }
            if (ConfigurationManager.AppSettings["batDirectory"] != null)
            {
                batDirectory = (string)ConfigurationManager.AppSettings["batDirectory"];
            }

            nextPassAllowTime = new DateTime();
            if (ConfigurationManager.AppSettings["passAllowStartTime"] == null || ConfigurationManager.AppSettings["passAllowStartTime"].Equals("")
                || (!DateTime.TryParseExact(ConfigurationManager.AppSettings["passAllowStartTime"].ToString().Trim(), Constants.timeFormat, null, DateTimeStyles.None, out nextPassAllowTime)))
                nextPassAllowTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 30, 0);

            passAllowTimeout = -1;
            if (ConfigurationManager.AppSettings["passAllowTimeout"] == null || ConfigurationManager.AppSettings["passAllowTimeout"].Equals("")
                || (!int.TryParse(ConfigurationManager.AppSettings["passAllowTimeout"].ToString().Trim(), out passAllowTimeout)))
                passAllowTimeout = 1440;

            dayAdvance = 0;
            if (ConfigurationManager.AppSettings["dayAdvance"] == null || ConfigurationManager.AppSettings["dayAdvance"].Equals("")
                || (!int.TryParse(ConfigurationManager.AppSettings["dayAdvance"].ToString().Trim(), out dayAdvance)))
                dayAdvance = 0;

            nextISSETime = new DateTime();
            if (ConfigurationManager.AppSettings["timeISSE"] == null || ConfigurationManager.AppSettings["timeISSE"].Equals("")
                || (!DateTime.TryParseExact(ConfigurationManager.AppSettings["timeISSE"].ToString().Trim(), Constants.timeFormat, null, DateTimeStyles.None, out nextISSETime)))
                nextISSETime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 30, 0);
        }

        public string generateSchemaReport(Object dbConnection, Microsoft.Office.Interop.Excel.ApplicationClass excel, Microsoft.Office.Interop.Excel.Workbook myWorkbook,
        string filePath, List<WorkingUnitTO> listWU, DateTime day, int company, string month, bool isAltLang, bool isSchedule)
        {
            debug.writeLog("+ Started presence and absence report! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            try
            {
                PassType passType;
                WorkingUnit workingUnit;

                Employee Employee;
                Common.Rule rule;
                EmployeesTimeSchedule EmplTimeSchedule;
                TimeSchema TimeSchema;


                if (dbConnection == null)
                {
                    passType = new PassType();
                    rule = new Common.Rule();
                    EmplTimeSchedule = new EmployeesTimeSchedule();
                    workingUnit = new WorkingUnit();
                    TimeSchema = new TimeSchema();
                    Employee = new Employee();
                }
                else
                {
                    passType = new PassType(dbConnection);
                    workingUnit = new WorkingUnit(dbConnection);
                    rule = new Common.Rule(dbConnection);

                    TimeSchema = new TimeSchema(dbConnection);

                    Employee = new Employee(dbConnection);
                    EmplTimeSchedule = new EmployeesTimeSchedule(dbConnection);
                }


                int rowStart = 1;
                List<DateTime> datesList = new List<DateTime>();
                datesList.Add(day);

                string mypath = filePath;

                object misValue = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Excel.Worksheet sheet = (Microsoft.Office.Interop.Excel.Worksheet)myWorkbook.Sheets[1];

                DataTable dtable = new DataTable();

                dtable.Columns.Add("1", typeof(string));
                dtable.Columns.Add("ID", typeof(string));
                dtable.Columns.Add("Interval", typeof(string));


                dtable.Clear();

                List<EmployeeTO> listEMpl = new List<EmployeeTO>();
                listWU = new WorkingUnit().FindAllChildren(listWU);
                string wuiids = "";
                foreach (WorkingUnitTO wu in listWU)
                {
                    wuiids += wu.WorkingUnitID + ",";
                }
                if (wuiids.Length > 0)
                    wuiids = wuiids.Remove(wuiids.LastIndexOf(','));

                //TODO change datetime to real fromdate and todate
                listEMpl = new Employee().SearchByWULoans(wuiids, -1, null, new DateTime(), new DateTime());
                foreach (EmployeeTO empl in listEMpl)
                {

                    if (empl.Status == "ACTIVE")
                    {
                        Dictionary<int, TimeSpan> typesCounter = new Dictionary<int, TimeSpan>();

                        List<EmployeeTimeScheduleTO> timeScheduleList = EmplTimeSchedule.SearchEmployeesSchedules(empl.EmployeeID.ToString(), datesList[0].Date, datesList[datesList.Count - 1].Date);
                        string schemaID = "";
                        foreach (EmployeeTimeScheduleTO employeeTimeSchedule in timeScheduleList)
                        {
                            schemaID += employeeTimeSchedule.TimeSchemaID.ToString() + ", ";
                        }
                        if (!schemaID.Equals(""))
                        {
                            schemaID = schemaID.Substring(0, schemaID.Length - 2);
                        }

                        List<WorkTimeSchemaTO> wSchema = TimeSchema.Search(schemaID);

                        List<WorkTimeIntervalTO> timeSchemaIntervalList = getTimeSchemaInterval(empl.EmployeeID, datesList[0], timeScheduleList, wSchema);
                        DataRow row = dtable.NewRow();
                        row[1] = empl.EmployeeID;
                        string intervals = "";

                        foreach (WorkTimeIntervalTO wtime in timeSchemaIntervalList)
                        {
                            intervals += wtime.StartTime.ToString("HH:mm") + " - " + wtime.EndTime.ToString("HH:mm") + ";";
                        }
                        if (intervals.Length > 0)
                            intervals = intervals.Remove(intervals.LastIndexOf(";"));

                        row[2] = intervals;
                        dtable.Rows.Add(row);
                        dtable.AcceptChanges();
                    }
                }

                int rowi = fillExcelSchema(dtable, excel, 1);

                rowStart = rowi;
                if (rowStart % 52 != 0)
                {
                    int indf = 52 - (rowStart % 52);
                    rowStart += indf + 1;
                }


                //if(rowStart >1){
                excel.Rows.AutoFit();

                string Path = Directory.GetParent(mypath).FullName;
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                if (File.Exists(mypath))
                    File.Delete(mypath);

                myWorkbook.Saved = true;
                myWorkbook.SaveCopyAs(mypath);


                Marshal.ReleaseComObject(sheet);
                sheet = null;

                debug.writeLog("+ Finished presence and absence report! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                return mypath;
            }
            catch (Exception ex)
            {

                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "generateExcelPA() " + ex.Message);
                return "";
            }
        }
        private int fillExcelSchema(DataTable dt, Microsoft.Office.Interop.Excel.ApplicationClass excel, int rowStart)
        {

            rowStart += 2;
            for (int i = 1; i < dt.Columns.Count + 1; i++)
            {
                if (i == 1)
                    excel.Cells[rowStart, i] = "       ";
                else
                {
                    excel.Cells[rowStart, i] = dt.Columns[i - 1].ColumnName;

                }
            }

            int rowi = rowStart + 1;
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (rowi == rowStart + 1)
                    {
                        rowi++;
                    }
                    else
                    {

                        if (!row.IsNull(i))
                        {
                            excel.Cells[rowi, i + 1] = row[i].ToString();

                        }
                        else
                        {
                            excel.Cells[rowi, i + 1] = "0.00";
                        }

                    }
                }
                rowi++;
            }

            return rowi;
        }


        public bool StartReporting()
        {
            bool started = false;

            try
            {
                if (!this.isProcessing)
                {
                    // set next sync time
                    while (nextPassAllowTime < DateTime.Now)
                    {
                        nextPassAllowTime = nextPassAllowTime.AddMinutes(passAllowTimeout);
                    }

                    // set next ISSE time
                    if (Constants.WeekDays().Contains(dayISSE))
                    {
                        while (nextISSETime.DayOfWeek.ToString() != dayISSE)
                        {
                            nextISSETime = nextISSETime.AddDays(1);
                        }
                    }                    

                    this.isProcessing = true;
                    processManagerThread = new Thread(new ThreadStart(Processing));
                    processManagerThread.Start();
                    Controller.DataProcessingStateChanged("Thread: processing started");
                    System.Console.WriteLine("++++ Processing Log files Started at:  " + DateTime.Now.ToString(" dd.MM.yyyy hh:mm:ss") + "\n");
                    debug.writeLog("++++ Processing work analysis Started at:  " + DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss") + "\n");
                    started = true;
                }
                else
                {
                    System.Console.WriteLine("*** Processing work analysis Started already !!! \n");
                    debug.writeLog("*** Processing work analysis Started already !!! \n");
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".StartReporting() : " + ex.Message + "\n");
            }

            return started;
        }

        public bool StopReporting()
        {
            bool stopped = false;

            try
            {
                this.isProcessing = false;
                if (processManagerThread.Join(5000))
                {
                    Controller.DataProcessingStateChanged("Thread: processing finished");
                    System.Console.WriteLine("Processing thread finished \n");
                }
                else
                {
                    Controller.DataProcessingStateChanged("Thread: processing aborted");
                    System.Console.WriteLine("Processing thread aborted \n");
                    processManagerThread.Abort();
                }
                System.Console.WriteLine("---- Processing work analysis Stopped at:  " + DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss") + "\n");
                debug.writeLog("---- Processing work analysis Stopped at:  " + DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss") + "\n");
                stopped = true;

            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".StopReporting() : " + ex.Message + "\n");
                stopped = false;
            }

            return stopped;
        }

        private void Processing()
        {
            const int MAX_NUMBER_OF_RETRIES = 1440;
            int numberOfRetries = 1;
            while ((numberOfRetries <= MAX_NUMBER_OF_RETRIES) && isProcessing)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " A new notification process is started: " +
                               numberOfRetries.ToString());
                try
                {
                    while (isProcessing)
                    {
                        //if ((DateTime.Now.Ticks - lastExecuteBatch.Ticks) / TimeSpan.TicksPerMillisecond > 3 * 60 * 60 * 1000)
                        //{
                        //    debug.writeLog("+++Executing batch file" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                        //    executeBatch();
                        //    lastExecuteBatch = DateTime.Now;
                        //}
                        
                        if (isPassAllowTime())
                        {
                            List<int> companyList = new List<int>();
                            companyList.Add((int)Constants.FiatCompanies.FS);
                            companyList.Add((int)Constants.FiatCompanies.MMauto);
                            companyList.Add((int)Constants.FiatCompanies.MMdoo);
                            companyList.Add((int)Constants.FiatCompanies.FAS);
                            new BadgeSystemInterface().GenerateReportService(null, Constants.FiatSharedAreaPassAllowance, companyList, DateTime.Now.Date.AddDays(dayAdvance));
                            //new BadgeSystemInterface().GenerateReportService(null, Environment.GetFolderPath(Environment.SpecialFolder.Desktop), companyList, DateTime.Now.Date.AddDays(dayAdvance));
                            nextPassAllowTime = nextPassAllowTime.AddMinutes(passAllowTimeout);
                        }

                        if (ifISSE()) 
                        {
                            // generate report
                            string filePath = Constants.FiatSharedAreaFASISSE.ToString() + DateTime.Now.Date.ToString("yyyy_MM_dd") + ".txt";
                            new ReportSchedule().GenerateReport((int)Constants.FiatCompanies.FAS, filePath);

                            // move to shared area
                            executeBatch();

                            nextISSETime = nextISSETime.AddDays(7);
                        }

                        if (timeMontly.Equals(DateTime.Now.ToString("HH:mm")) || timeDaily.Equals(DateTime.Now.ToString("HH:mm")))
                        {
                        //if (true)
                        //{

                            Controller.NotificationStateChanged("Thread: work analysis process started");
                            debug.writeBenchmarkLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WORK ANALYSIS PROCESS STARTED!");

                            Dictionary<int, List<WorkingUnitTO>> companies = new Dictionary<int, List<WorkingUnitTO>>();
                            Dictionary<int, string> companyNames = new Dictionary<int, string>();

                            Dictionary<string, string> companiesFilePath400 = Constants.FiatSharedAreaReport400;
                            Dictionary<string, string> companiesFilePath500 = Constants.FiatSharedAreaReport500;
                            Dictionary<string, string> companiesFilePathWageType = Constants.FiatSharedAreaReportWageType;
                            Dictionary<string, string> companiesFilePathPA = Constants.FiatSharedAreaReportPA;
                            Dictionary<string, string> companiesFilePathDecisions = Constants.FiatSharedAreaReportDecisions;
                            Dictionary<string, string> companiesFilePathShifts = Constants.FiatSharedAreaReportShifts;
                            Dictionary<string, string> companiesFilePathAnomalies = Constants.FiatSharedAreaReportAnomalies;

                            string wunit = "";
                            foreach (int comp in Enum.GetValues(typeof(Constants.FiatCompanies)))
                            {
                                List<WorkingUnitTO> plants = new WorkingUnit().SearchChildWU(comp.ToString());
                                companies.Add(comp, plants);
                                string name = Enum.GetName(typeof(Constants.FiatCompanies), comp);
                                companyNames.Add(comp, name);


                                List<WorkingUnitTO> AllWUList = new List<WorkingUnitTO>();
                                List<WorkingUnitTO> List = new List<WorkingUnitTO>();

                                WorkingUnitTO defaultWU = new WorkingUnit().FindWU(comp);

                                List<WorkingUnitTO> listChildren = new WorkingUnit().SearchChildWU(comp.ToString());
                                AllWUList.Add(defaultWU);
                                AllWUList.AddRange(listChildren);

                                bool isParent = false;
                                if (listChildren.Count > 0)
                                    isParent = true;

                                while (isParent)
                                {
                                    foreach (WorkingUnitTO Child in listChildren)
                                    {
                                        List<WorkingUnitTO> wuChildList = new WorkingUnit().SearchChildWU(Child.WorkingUnitID.ToString());
                                        List.AddRange(wuChildList);
                                    }

                                    if (List.Count > 0)
                                        isParent = true;
                                    else
                                        isParent = false;

                                    listChildren.Clear();
                                    listChildren.AddRange(List);
                                    AllWUList.AddRange(List);
                                    List.Clear();

                                }

                                foreach (WorkingUnitTO item in AllWUList)
                                {
                                    wunit += item.WorkingUnitID + ", ";
                                }
                                if (wunit.Length > 0)
                                    wunit = wunit.Remove(wunit.LastIndexOf(','));

                            }

                            try
                            {


                                List<DateTime> datesList = new List<DateTime>();
                                DateTime day = DateTime.Now;
                                day = day.AddMonths(-1);

                                DateTime dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                while (day < dayOne)
                                {
                                    datesList.Add(day);
                                    day = day.AddDays(1);
                                }

                                debug.writeLog("");
                                if (timeDaily.Equals(DateTime.Now.ToString("HH:mm")))
                                {
                                //if (false)
                                //{
                                    debug.writeLog("");
                                    debug.writeLog("---START DAILY REPORTS " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                                    bool isAltLang = true;

                                    debug.writeLog("+ Started daily P&A ! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                                    foreach (KeyValuePair<int, List<WorkingUnitTO>> pair in companies)
                                    {
                                        int company = pair.Key;
                                        List<WorkingUnitTO> listPlants = pair.Value;
                                        string filePath = companiesFilePathPA[companyNames[pair.Key]] +
                                            DateTime.Now.ToString("yyyy_MM") + "\\" + "P&A_" +
                                            companyNames[pair.Key] + "_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                                        generatePAReportNew(null, filePath, listPlants, DateTime.Now.AddDays(-1).Date, company, isAltLang);
                                    }
                                    datesList = new List<DateTime>();
                                    DateTime from = DateTime.Now;


                                    datesList.Add(from);

                                    debug.writeLog("+ Finished daily P&A ! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                    debug.writeLog("");
                                    debug.writeLog("+ Started daily wage types ! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                                    foreach (KeyValuePair<int, List<WorkingUnitTO>> pair in companies)
                                    {

                                        List<WorkingUnitTO> listPlants = pair.Value;
                                        string filePath = companiesFilePathWageType[companyNames[pair.Key]] +
                                            DateTime.Now.ToString("yyyy_MM") + "\\" + "Wage_Type_" +
                                            companyNames[pair.Key] + "_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                                        generateWageTypesReportNew(null, filePath, listPlants, pair.Key, isAltLang, null);


                                    }
                                    debug.writeLog("+ Finished daily WAGE TYPES ! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                    debug.writeLog("");

                                    debug.writeLog("---END DAILY REPORTS " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                    executeBatch();
                                }
                                else
                                {

                                    day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                                    day = day.AddMonths(-1);
                                    dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    dayOne = dayOne.AddDays(-1);

                                    Common.Rule ruleHrssc = new Common.Rule();
                                    ruleHrssc.RuleTO.RuleType = Constants.RuleHRSSCCutOffDate;
                                    List<RuleTO> rulesHrssc = ruleHrssc.Search();
                                    int wd = 6;
                                    if (rulesHrssc.Count > 0)
                                    {
                                        wd = rulesHrssc[0].RuleValue;
                                    }


                                    DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                                    while (Common.Misc.countWorkingDays(firstDay, null) < wd)
                                    {
                                        firstDay = firstDay.AddDays(1);


                                    }
                                    if (DateTime.Now.Date == firstDay.Date)
                                    {
                                        debug.writeLog("+ Started decisions reports generating " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                        foreach (KeyValuePair<int, List<WorkingUnitTO>> pair in companies)
                                        {
                                            if (pair.Key == -3 && pair.Key == -4)
                                            {
                                                debug.writeLog("+ Started decisions generating for company: " + pair.Key + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                                                string file = companiesFilePathDecisions[companyNames[pair.Key]] + DateTime.Now.AddMonths(-1).ToString("yyyy_MM") + "\\Decisions_" + DateTime.Now.ToString("yyyy_MM_dd") + ".pdf";
                                                Common.Misc.generatePDFDecisions(pair.Key, file, day, dayOne);

                                                debug.writeLog("+ Finished decisions generating for company: " + pair.Key + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                            }
                                        }
                                        debug.writeLog("+ Finished decisions reports generating " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                    }
                                    //if (true)
                                    //{
                                    //    //add schedule
                                    //    debug.writeLog("+ Started shifts reports generating " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                    //    foreach (KeyValuePair<int, List<WorkingUnitTO>> pair in companies)
                                    //    {
                                    //        debug.writeLog("+ Started shifts report generating for company: " + pair.Key + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                                    //        string file = companiesFilePathShifts[companyNames[pair.Key]] + DateTime.Now.AddMonths(-1).ToString("yyyy_MM") + "\\Decisions_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                                    //        new ReportSchedule().GenerateReport(pair.Key, file);

                                    //        debug.writeLog("+ Finished shifts report generating for company: " + pair.Key + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                                    //    }
                                    //    debug.writeLog("+ Finished shifts reports generating " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                                    //}


                                    if (ifMonthly())
                                    {
                                    //if (true)
                                    //{

                                        debug.writeLog("");
                                        debug.writeLog("---START MONTHLY REPORTS " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                                        datesList = new List<DateTime>();
                                        day = DateTime.Now;
                                        day = day.AddMonths(-1);
                                        day = new DateTime(day.Year, day.Month, 1);
                                        dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                        while (day < dayOne)
                                        {
                                            datesList.Add(day);
                                            day = day.AddDays(1);
                                        }
                                        //list employees for generating py report
                                        List<EmployeeTO> empolyeeList = new Employee().SearchByWULoans(wunit, -1, null, datesList[0], datesList[datesList.Count - 1]);
                                        string employeeString = "";
                                        foreach (EmployeeTO empl in empolyeeList)
                                        {
                                            employeeString += empl.EmployeeID + ", ";
                                        }
                                        if (employeeString.Length > 0)
                                            employeeString = employeeString.Remove(employeeString.LastIndexOf(','));

                                        List<string> analiticStrings = new List<string>();
                                        List<string> sumStrings = new List<string>();
                                        List<string> bufferStrings = new List<string>();
                                        List<string> bufferExpatStrings = new List<string>();
                                        Dictionary<int, OnlineMealsTypesTO> mealTypesDict = new Dictionary<int, OnlineMealsTypesTO>();
                                        bool unRegularDataFound = false;
                                        Dictionary<int, List<string>> unregularEmployees = new Dictionary<int, List<string>>();
                                        Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
                                        //Dictionary<int, PassTypeTO> typesDict = new PassType().SearchDictionary();
                                        uint calcID = Common.Misc.GenerateFiatPYData(employeeString, emplDict, datesList[0].Date, datesList[datesList.Count - 1].Date, Constants.PYTypeEstimated, 
                                            analiticStrings, bufferStrings, bufferExpatStrings, sumStrings, mealTypesDict, ref unRegularDataFound, unregularEmployees,
                                            new Dictionary<int, Dictionary<DateTime, decimal>>(), new Dictionary<int, Dictionary<DateTime, decimal>>(), false, false, false, 0, false, false, false, false, 0, false, false);

                                        debug.writeLog("+ Started 400 reports! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                        foreach (KeyValuePair<int, List<WorkingUnitTO>> pair in companies)
                                        {
                                            string filePath = companiesFilePath400[companyNames[pair.Key]] +
                                                DateTime.Now.AddMonths(-1).Date.ToString("yyyy_MM") + "\\" + "400_" +
                                                companyNames[pair.Key] + "_Month_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                                            new Report400().GenerateReport(null, filePath, pair.Value, datesList, pair.Key, calcID);

                                        }

                                        debug.writeLog("+ Finished 400 reports! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                        debug.writeLog("");
                                        debug.writeLog("+ Started 500 montly ! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                                        foreach (KeyValuePair<int, List<WorkingUnitTO>> pair in companies)
                                        {
                                            string filePath = companiesFilePath500[companyNames[pair.Key]] +
                                                  DateTime.Now.AddMonths(-1).Date.ToString("yyyy_MM") + "\\" + "500_" +
                                                  companyNames[pair.Key] + "_Month_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                                            new Report500().GenerateReport(null, datesList, filePath, pair.Value, pair.Key, calcID);
                                        }

                                        debug.writeLog("+ Finished 500 montly ! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                        debug.writeLog("");
                                        debug.writeLog("---END MONTHLY REPORTS " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                        executeBatch();
                                    }

                                    if (ifWeekly())
                                    {
                                        debug.writeLog("---START WEEKLY REPORTS " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                        int numOfDaysPrevious = returnMonthDays(DateTime.Now.Month, true);
                                        datesList = new List<DateTime>();
                                        day = new DateTime();
                                        dayOne = new DateTime();
                                        int num = 0;

                                        if (DateTime.Now.Day - 8 <= 0)
                                        {
                                            num = 8 - DateTime.Now.Day;
                                            day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                            day = day.AddMonths(-1);
                                            day = new DateTime(day.Year, day.Month, numOfDaysPrevious, 0, 0, 0);
                                            day = day.AddDays(-num);
                                        }
                                        else
                                        {
                                            day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                                            day = day.AddDays(-8);
                                        }

                                        if (DateTime.Now.Day - 1 <= 0)
                                        {
                                            num = 1 - DateTime.Now.Day;
                                            dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                            dayOne = dayOne.AddMonths(-1);
                                            day = new DateTime(day.Year, day.Month, numOfDaysPrevious, 0, 0, 0);
                                            dayOne = dayOne.AddDays(-num);
                                        }
                                        else
                                        {
                                            dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                                            dayOne = dayOne.AddDays(-1);
                                        }
                                        while (day < dayOne)
                                        {
                                            datesList.Add(day);
                                            day = day.AddDays(1);
                                        }
                                        //list employees for generating py report
                                        List<EmployeeTO> empolyeeList = new Employee().SearchByWULoans(wunit, -1, null, datesList[0], datesList[datesList.Count - 1]);
                                        string employeeString = "";
                                        foreach (EmployeeTO empl in empolyeeList)
                                        {
                                            employeeString += empl.EmployeeID + ", ";
                                        }
                                        if (employeeString.Length > 0)
                                            employeeString = employeeString.Remove(employeeString.LastIndexOf(','));

                                        List<string> analiticStrings = new List<string>();
                                        List<string> sumStrings = new List<string>();
                                        List<string> bufferStrings = new List<string>();
                                        List<string> bufferExpatStrings = new List<string>();
                                        Dictionary<int, OnlineMealsTypesTO> mealTypesDict = new Dictionary<int, OnlineMealsTypesTO>();
                                        bool unRegularDataFound = false;
                                        Dictionary<int, List<string>> unregularEmployees = new Dictionary<int, List<string>>();
                                        Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
                                        //Dictionary<int, PassTypeTO> typesDict = new PassType().SearchDictionary();
                                        uint calcID = Common.Misc.GenerateFiatPYData(employeeString, emplDict, datesList[0].Date, datesList[datesList.Count - 1].Date, Constants.PYTypeEstimated, 
                                            analiticStrings, bufferStrings, bufferExpatStrings, sumStrings, mealTypesDict, ref unRegularDataFound, unregularEmployees, 
                                            new Dictionary<int, Dictionary<DateTime, decimal>>(), new Dictionary<int, Dictionary<DateTime, decimal>>(), false, false, false, 0, false, false, false, false, 0, false, false);

                                        debug.writeLog("");
                                        debug.writeLog("+ Start 500 weekly " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                        foreach (KeyValuePair<int, List<WorkingUnitTO>> pair in companies)
                                        {
                                            string filePath = companiesFilePath500[companyNames[pair.Key]] + DateTime.Now.Date.ToString("yyyy_MM") + "\\" + "500_" +
                                                companyNames[pair.Key] + "_Week_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                                            new Report500().GenerateReport(null, datesList, filePath, pair.Value, pair.Key, calcID);
                                        }

                                        debug.writeLog("+ Finished 500 weekly! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                        debug.writeLog("");
                                        debug.writeLog("+ Started week agregate ! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                        datesList = new List<DateTime>();
                                        day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                        dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                                        while (day < dayOne)
                                        {
                                            datesList.Add(day);
                                            day = day.AddDays(1);
                                        }
                                        analiticStrings = new List<string>();
                                        sumStrings = new List<string>();
                                        bufferStrings = new List<string>();
                                        bufferExpatStrings = new List<string>();
                                        mealTypesDict = new Dictionary<int, OnlineMealsTypesTO>();
                                        unRegularDataFound = false;
                                        unregularEmployees = new Dictionary<int, List<string>>();
                                        emplDict = new Dictionary<int, EmployeeTO>();
                                        //typesDict = new PassType().SearchDictionary();
                                        calcID = Common.Misc.GenerateFiatPYData(employeeString, emplDict, datesList[0].Date, datesList[datesList.Count - 1].Date, Constants.PYTypeEstimated, 
                                            analiticStrings, bufferStrings, bufferExpatStrings, sumStrings, mealTypesDict, ref unRegularDataFound, unregularEmployees, 
                                            new Dictionary<int, Dictionary<DateTime, decimal>>(), new Dictionary<int, Dictionary<DateTime, decimal>>(), false, false, false, 0, false, false, false, false, 0, false, false);
                                        foreach (KeyValuePair<int, List<WorkingUnitTO>> pair in companies)
                                        {
                                            string filePath = companiesFilePath500[companyNames[pair.Key]] +
                                                  DateTime.Now.ToString("yyyy_MM") + "\\" + "500_" +
                                                  companyNames[pair.Key] + "_Week_Agregate_" +
                                                  DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                                            new Report500().GenerateReport(null, datesList, filePath, pair.Value, pair.Key, calcID);
                                        }

                                        debug.writeLog("+ Finished week agregate ! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                        debug.writeLog("");
                                        debug.writeLog("---END WEEKLY REPORTS " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                        executeBatch();
                                    }

                                    debug.writeLog("***CHECK ANOMALIES REPORT SCHEDULE " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                    //ANOMALIES REPORTS EVERY DAY FROM 1. IN MONTH TILL HRSSC+2 WD
                                    foreach (KeyValuePair<int, List<WorkingUnitTO>> pair in companies)
                                    {
                                        DateTime month = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                                        int hrsscCutoffDate = 7;
                                        Common.Rule rule1 = new Common.Rule();
                                        rule1.RuleTO.WorkingUnitID = pair.Key;

                                        rule1.RuleTO.RuleType = Constants.RuleHRSSCCutOffDate;

                                        List<RuleTO> rules = rule1.Search();
                                        hrsscCutoffDate = rules[0].RuleValue;
                                        if (rules.Count > 0)
                                        {
                                            hrsscCutoffDate = rules[0].RuleValue + 1;
                                        }

                                        while (Common.Misc.countWorkingDays(month, null) < hrsscCutoffDate)
                                        {
                                            month = month.AddDays(1);
                                        }

                                        DateTime monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                                        if (DateTime.Now.Date >= monthStart.Date && DateTime.Now.Date <= month.Date)
                                        {
                                            //if (true)
                                            //{
                                            day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                            day = day.AddMonths(-1);
                                            dayOne = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                            datesList = new List<DateTime>();
                                            while (day < dayOne)
                                            {
                                                datesList.Add(day);
                                                day = day.AddDays(1);
                                            }

                                            string filePath = companiesFilePathAnomalies[companyNames[pair.Key]] +
                                                DateTime.Now.AddMonths(-1).Date.ToString("yyyy_MM") + "\\" + "TM_Anomalies_" +
                                                companyNames[pair.Key] + "_" +
                                                DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx";
                                            generateAnomaliesReportNew(datesList, filePath, companies[pair.Key], pair.Key);

                                        }

                                    }
                                    debug.writeLog("***END CHECK ANOMALIES REPORT SCHEDULE " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                    executeBatch();
                                }


                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                GC.Collect();
                            }
                            catch (ThreadAbortException ex)
                            {

                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                GC.Collect();

                                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WORK ANALYSIS PROCESS abort exception in: " +
                            this.ToString() + ".Processing() : " + ex.Message + "\n");

                            }
                            //executeBat();
                        }
                        GC.Collect();
                        Common.DataManager.CloseDataBaseConnection();
                        Thread.Sleep(30 * 1000);
                    }

                }
                catch (ThreadAbortException taex)
                {
                    debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WORK ANALYSIS PROCESS abort exception in: " +
                        this.ToString() + ".Processing() : " + taex.Message + "\n");
                }
                catch (Exception ex)
                {
                    debug.writeLog(DateTime.Now + " Exception in: " +
                        this.ToString() + ".Processing() : " + ex.Message + "\n");
                }
                finally
                {
                    if (isProcessing)
                    {
                        // exception occurred
                        Controller.NotificationStateChanged("Thread: processing restarted, retry no. " + numberOfRetries.ToString());
                        Controller.NotificationStateChanged("");
                        numberOfRetries++;
                        Thread.Sleep(60000);
                        Common.DataManager.CloseDataBaseConnection();
                    }
                }
            }
            if (numberOfRetries > MAX_NUMBER_OF_RETRIES)
            {
                isProcessing = false;
                Controller.NotificationStateChanged("Thread: after " + MAX_NUMBER_OF_RETRIES.ToString() + " retries processing stopped");
            }
        }

        public List<WorkTimeIntervalTO> getTimeSchemaInterval(int employeeID, DateTime date, List<EmployeeTimeScheduleTO> timeScheduleListForEmployee, List<WorkTimeSchemaTO> timeSchema)
        {
            List<WorkTimeIntervalTO> intervalList = new List<WorkTimeIntervalTO>();
            try
            {
                int timeScheduleIndex = -1;

                for (int scheduleIndex = 0; scheduleIndex < timeScheduleListForEmployee.Count; scheduleIndex++)
                {

                    if (date >= timeScheduleListForEmployee[scheduleIndex].Date)
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
                    foreach (WorkTimeSchemaTO timeSch in timeSchema)
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


        public string generate500ReportNew(Object dbConnection, List<DateTime> datesList, string fileName, List<WorkingUnitTO> listWU, int company)
        {

            try
            {
                return new Report500().GenerateReportService(dbConnection, datesList, fileName, listWU, company);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "generateExcel500() " + ex.Message);
                return "";
            }
        }


        public string generate400ReportNew(Object dbConnection, string filePath, List<WorkingUnitTO> listWU, List<DateTime> datesList, int company)
        {

            try
            {
                return new Report400().GenerateReportService(dbConnection, filePath, listWU, datesList, company);

            }
            catch (Exception ex)
            {

                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "generateExcel400() " + ex.Message);
                return "";
            }
        }



        public string generateWageTypesReportNew(Object dbConnection, string filePath, List<WorkingUnitTO> listPlants, int company, bool isAltLang, List<DateTime> datesList)
        {

            try
            {
                return new ReportWageTypes().GenerateReport(dbConnection, filePath, listPlants, company, isAltLang, datesList);

            }
            catch (Exception ex)
            {

                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "generateExcelWageTypes() " + ex.Message);
                return "";
            }
        }


        public string generatePAReportNew(Object dbConnection, string filePath, List<WorkingUnitTO> listPlants, DateTime day, int company, bool isAltLang)
        {

            try
            {
                return new ReportPresenceAndAbsence().GenerateReport(dbConnection, filePath, listPlants, day, company, isAltLang);

            }
            catch (Exception ex)
            {

                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "generateExcelPA() " + ex.Message);
                return "";
            }
        }




        private void generateAnomaliesReportNew(List<DateTime> datesList, string filePath, List<WorkingUnitTO> listWU, int company)
        {

            try
            {
                new ReportAnomalies().GenerateReport(datesList, filePath, listWU, company);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "generateExcelAnomalies() " + ex.Message);


            }
        }

        private void saveOnShared(string userName, string password, string mypath, Process p)
        {
            try
            {
                debug.writeLog("Start saveOnShared");

                p.CloseMainWindow();
                string Path = Directory.GetParent(mypath).FullName;
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                if (File.Exists(mypath))
                    File.Delete(mypath);
                //debug.writeLog("process");


                debug.writeLog("Saved on location: " + mypath);

                p.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void saveOnShared(string userName, string password, string mypath, Microsoft.Office.Interop.Excel.Workbook myWorkbook)
        {
            try
            {
                debug.writeLog("Start saveOnShared");
                string Pathh = Directory.GetParent(mypath).FullName;
                Process p = System.Diagnostics.Process.Start(@"C:\WINDOWS\system32\net.exe", @"use " + Pathh + " " + password + @" /user:" + userName);
                p.CloseMainWindow();
                string Path = Directory.GetParent(mypath).FullName;
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                if (File.Exists(mypath))
                    File.Delete(mypath);
                //debug.writeLog("process");

                myWorkbook.Saved = true;
                myWorkbook.SaveCopyAs(mypath);
                debug.writeLog("Saved on location: " + mypath);

                p.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void deleteDirectory(string filePath)
        {
            string Path = Directory.GetParent(filePath).FullName;
            if (Directory.Exists(Path))
            {

                Directory.Delete(Path, true);
            }
        }

        private bool isPersonalHoliday(Object dbConnection, IOPairProcessedTO pair, WorkTimeIntervalTO pairInterval)
        {
            try
            {
                string holidayType = "";
                DateTime pairDate = pair.IOPairDate.Date;

                // if pair is from second interval of night shift, it belongs to previous day
                //WorkTimeIntervalTO pairInterval = getPairInterval(pair, dayPairs);
                if (pairInterval.TimeSchemaID != -1 && pairInterval.StartTime.Hour == 0 && pairInterval.StartTime.Minute == 0)
                    pairDate = pairDate.AddDays(-1);

                EmployeeAsco4 emplAsco;
                if (dbConnection == null)
                {
                    emplAsco = new EmployeeAsco4();
                }
                else
                {
                    emplAsco = new EmployeeAsco4(dbConnection);
                }
                // check if date is personal holiday, no transfering holidays for personal holidays
                // get employee personal holiday category

                emplAsco.EmplAsco4TO.EmployeeID = pair.EmployeeID;
                List<EmployeeAsco4TO> ascoList = emplAsco.Search();

                if (ascoList.Count == 1)
                {
                    holidayType = ascoList[0].NVarcharValue1.Trim();

                    if (!holidayType.Trim().Equals(""))
                    {
                        // if category is IV, find holiday date and check if pair date is holiday
                        if (holidayType.Trim().Equals(Constants.holidayTypeIV.Trim()))
                        {
                            DateTime holDate = ascoList[0].DatetimeValue1.Date;

                            if (holDate.Month == pairDate.Month && holDate.Day == pairDate.Day)
                                return true;
                        }
                        else
                        {
                            // if category is I, II or III, check if pair date is personal holiday
                            HolidaysExtended holExtended;
                            if (dbConnection == null)
                                holExtended = new HolidaysExtended();
                            else
                                holExtended = new HolidaysExtended(dbConnection);
                            holExtended.HolTO.Type = Constants.personalHoliday.Trim();
                            holExtended.HolTO.Category = holidayType.Trim();

                            if (holExtended.Search(pairDate, pairDate).Count > 0)
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

        private bool isNationalHoliday(Object dbConnection, IOPairProcessedTO pair, string EmplTimeSchema, WorkTimeIntervalTO pairInterval)
        {
            try
            {
                DateTime pairDate = pair.IOPairDate.Date;

                // if pair is from second interval of night shift, it belongs to previous day
                //WorkTimeIntervalTO pairInterval = getPairInterval(pair, dayPairs);
                if (pairInterval.TimeSchemaID != -1 && pairInterval.StartTime.Hour == 0 && pairInterval.StartTime.Minute == 0)
                    pairDate = pairDate.AddDays(-1);

                // check if date is national holiday, national holidays are transferd from Sunday to first working day
                List<DateTime> nationalHolidaysDays = new List<DateTime>();
                List<DateTime> nationalHolidaysSundays = new List<DateTime>();
                List<DateTime> nationalHolidaysSundaysDays = new List<DateTime>();
                Dictionary<string, List<DateTime>> personalHolidayDays = new Dictionary<string, List<DateTime>>();

                getHolidays(dbConnection, pairDate, pairDate, personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays);

                if (EmplTimeSchema.ToUpper().Equals(Constants.schemaTypeIndustrial.Trim().ToUpper()))
                {
                    if (nationalHolidaysDays.Contains(pairDate.Date))
                        return true;
                }
                else if (nationalHolidaysDays.Contains(pairDate.Date) || nationalHolidaysSundays.Contains(pairDate.Date))
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void getHolidays(Object dbConnection, DateTime startTime, DateTime endTime, Dictionary<string, List<DateTime>> personalHolidayDays, List<DateTime> nationalHolidaysDays,
           List<DateTime> nationalHolidaysSundays)
        {
            try
            {
                // search from previous day, if start time is transfered holiday
                HolidaysExtended holiday;
                if (dbConnection == null)
                    holiday = new HolidaysExtended();
                else
                    holiday = new HolidaysExtended(dbConnection);

                if (holiday != null)
                {
                    List<HolidaysExtendedTO> Holidays = holiday.Search(startTime.AddDays(-1), endTime);
                    List<DateTime> nationalHolidaysAll = new List<DateTime>();
                    List<DateTime> nationalHolidaysSundaysDays = new List<DateTime>();
                    Dictionary<string, List<DateTime>> personalHolidaySundaysDays = new Dictionary<string, List<DateTime>>();

                    foreach (HolidaysExtendedTO holidayTO in Holidays)
                    {
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

        private void fillChart(Microsoft.Office.Interop.Excel.Worksheet sheet, bool isDirect, int start, int rowStart, int third, int second, int numChart, string title)
        {
            Microsoft.Office.Interop.Excel.Range chartRange;
            Microsoft.Office.Interop.Excel.Series oSeries;

            Microsoft.Office.Interop.Excel.ChartObjects xlCharts = (Microsoft.Office.Interop.Excel.ChartObjects)sheet.ChartObjects(Type.Missing);

            Microsoft.Office.Interop.Excel.ChartObject myChart = (Microsoft.Office.Interop.Excel.ChartObject)xlCharts.Add(10, 80, 300, 250);

            Microsoft.Office.Interop.Excel.Chart chartPage = myChart.Chart;

            string ba = "";
            string bac = "";
            if (isDirect)
            {
                ba = "C" + (start + 7) + ":C" + (start + third + 6);
                bac = "B" + (start + 7) + ":B" + (start + third + 6);
            }
            else
            {
                ba = "E" + (start + 7) + ":E" + (start + third + 6);
                bac = "B" + (start + 7) + ":B" + (start + third + 6);
            }
            chartRange = sheet.get_Range(ba, Missing.Value);


            chartPage.SetSourceData(chartRange, Missing.Value);

            chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xl3DPie;
            chartPage.HasTitle = true;
            if (isDirect)
                chartPage.ChartTitle.Text = title + " BC";
            else
                chartPage.ChartTitle.Text = title + " WC";

            chartPage.ChartTitle.Left = 0;

            chartPage.Legend.Font.Size = 6;

            oSeries = (Microsoft.Office.Interop.Excel.Series)chartPage.SeriesCollection(1);
            oSeries.XValues = sheet.get_Range(bac, Missing.Value);
            //chartPage.Location(Microsoft.Office.Interop.Excel.XlChartLocation.xlLocationAutomatic, sheet.Name);
            ////((Microsoft.Office.Interop.Excel.Range)sheet.Columns["B", Type.Missing]).ColumnWidth = 36.43;

            string k7 = "A" + (second) + ":H" + (second + 19);
            sheet.Shapes.Item(numChart).Top = (float)(double)sheet.get_Range(k7, Missing.Value).Top;
            sheet.Shapes.Item(numChart).Left = (float)(double)sheet.get_Range(k7, Missing.Value).Left;
            sheet.Shapes.Item(numChart).Width = (float)(double)sheet.get_Range(k7, Missing.Value).Width;
            sheet.Shapes.Item(numChart).Height = (float)(double)sheet.get_Range(k7, Missing.Value).Height;
            chartPage.PlotArea.Width = (float)(double)sheet.get_Range(k7, Missing.Value).Width - 200;
            chartPage.PlotArea.Left = 0;
        }

        private bool isPassAllowTime()
        {
            if (DateTime.Now > nextPassAllowTime)
                return true;
            else
                return false;
        }
    }
}
