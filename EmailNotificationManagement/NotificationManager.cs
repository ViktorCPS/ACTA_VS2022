using System;
using System.Collections.Generic;
using System.Text;
using Util;
using System.Threading;
using DataAccess;
using Common;
using System.Collections;
using TransferObjects;
using System.IO;
using System.Net;
using System.Configuration;
using System.Globalization;
using System.Net.Mime;
using System.Resources;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Drawing;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;



namespace EmailNotificationManagement {

    public class NotificationManager {
        // Debug
        DebugLog debug;
        public bool doProcess = false;
        object locker = new object();
        private Thread processManagerThread;
        private static NotificationManager managerInstance;
        private DataManager dataMgr; // za azuriranje kartica na citacima
        private bool isProcessing = false;
        string mailTo = "", rootDirectory = "", ReportsDirectory = "";
        string emailAddress = "";
        string Host = "";
        string userName = "";
        string password = "";
        int processingInterval = 30;
        int port = 25;
        //string day = "";
        DateTime fromDate = new DateTime();
        DateTime toDate = new DateTime();
        string status = "";
        Dictionary<int, List<string>> emplAlowedTypes = new Dictionary<int, List<string>>();

        ResourceManager rm;
        CultureInfo culture;
        /*
        public string Day
        {
            get { return day; }
            set { day = value; }
        }
         * */
        string time = "", time1 = "", time2 = "", time3 = "", time4 = "", time5 = "", syncNavTime = "";
        //string timePayslip = "";
        //string timeMedical = "04:30";
        /*
        public string Time
        {
            get { return time; }
            set { time = value; }
        }
         * */
        Dictionary<int, string> months = new Dictionary<int, string>();

        private static long DBWaitTimeout = 28800;    // DB connection time out in sec.
        private static TimeSpan DBWaitTimeoutSpan = new TimeSpan(TimeSpan.TicksPerSecond * DBWaitTimeout * 90 / 100);
        Dictionary<int, WorkingUnitTO> wUnits = new Dictionary<int, WorkingUnitTO>();
        DAOFactory daoFactory = null;

        System.Net.Mail.SmtpClient smtp;

        // Controller instance
        public NotificationController Controller;

        public bool IsProcessing {
            get { return isProcessing; }
        }

        DateTime lastTime = new DateTime();
        /*
        DateTime newLastTime = new DateTime();

        
        static int numOfMailsWC = 0;
        static int numOfMailsPayslip = 0;
        static int numOfMailsWCDR = 0;
         * */
        static int numOfMailsHRM = 0; //12.06.2017 Miodrag Mitrovic


        protected NotificationManager() {

            NotificationController.SetApplicationName("ACTAEmailNotificationService");
            debug = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");
            try {
                months.Add(1, "Januar");
                months.Add(2, "Februar");
                months.Add(3, "Mart");
                months.Add(4, "April");
                months.Add(5, "Maj");
                months.Add(6, "Jun");
                months.Add(7, "Jul");
                months.Add(8, "Avgust");
                months.Add(9, "Septembar");
                months.Add(10, "Oktobar");
                months.Add(11, "Novembar");
                months.Add(12, "Decembar");
                daoFactory = DAOFactory.getDAOFactory();
                InitializeObserverClient();

                ReadAppSettings();
                smtp = new System.Net.Mail.SmtpClient(Host, port);


                if (!userName.Equals("") || !password.Equals("")) {
                    smtp.Credentials = new NetworkCredential(userName, password);
                }
                dataMgr = new DataManager();
                // Start Tag's tracking
                dataMgr.StartTagsTracking();
            }
            catch (Exception ex) {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " NotificationManager(): " + ex.Message + "\n");
            }
        }

        private void InitializeObserverClient() {
            Controller = NotificationController.GetInstance();
        }

        private void ReadAppSettings() {
            if (ConfigurationManager.AppSettings["emailAddress"] != null) {
                emailAddress = (string)ConfigurationManager.AppSettings["emailAddress"];
            }
            if (ConfigurationManager.AppSettings["mailTo"] != null) {
                mailTo = (string)ConfigurationManager.AppSettings["mailTo"];
            }
            if (ConfigurationManager.AppSettings["rootDirectory"] != null) {
                rootDirectory = (string)ConfigurationManager.AppSettings["rootDirectory"];
            }
            if (ConfigurationManager.AppSettings["ReportsDirectory"] != null) {
                ReportsDirectory = (string)ConfigurationManager.AppSettings["ReportsDirectory"];
            }
            //ReportsDirectory

            /*
            if (ConfigurationManager.AppSettings["allowedEmplTypesForSending"] != null)
            {
                string allowed = (string)ConfigurationManager.AppSettings["allowedEmplTypesForSending"];
                string[] types = allowed.Split(';');
                foreach (string type in types)
                {
                    string[] compTypes = type.Split(',');
                    if (compTypes.Length > 0)
                    {
                        if (!emplAlowedTypes.ContainsKey(int.Parse(compTypes[0])))
                            emplAlowedTypes.Add(int.Parse(compTypes[0]), new List<string>());

                        if (compTypes.Length > 1)
                        {
                            emplAlowedTypes[int.Parse(compTypes[0])].Add(compTypes[1]);
                        }

                    }
                }
            }
             * */
            if (ConfigurationManager.AppSettings["host"] != null) {
                Host = (string)ConfigurationManager.AppSettings["host"];
            }

            if (ConfigurationManager.AppSettings["port"] != null) {
                port = int.Parse((string)ConfigurationManager.AppSettings["port"]);
            }

            if (ConfigurationManager.AppSettings["userName"] != null) {
                userName = (string)ConfigurationManager.AppSettings["userName"];
            }

            if (ConfigurationManager.AppSettings["password"] != null) {
                password = (string)ConfigurationManager.AppSettings["password"];
            }

            if (ConfigurationManager.AppSettings["processingSleapInterval"] != null) {
                processingInterval = int.Parse((string)ConfigurationManager.AppSettings["processingSleapInterval"]);
            }
            /*
            if (ConfigurationManager.AppSettings["day"] != null)
            {
                day = (string)ConfigurationManager.AppSettings["day"];
            }
            */
            if (ConfigurationManager.AppSettings["syncNavTime"] != null) {
                syncNavTime = (string)ConfigurationManager.AppSettings["syncNavTime"];
            }
            else {
                syncNavTime = "03:00";
            }
            if (ConfigurationManager.AppSettings["time1"] != null) {
                time1 = (string)ConfigurationManager.AppSettings["time1"];
            }
            if (ConfigurationManager.AppSettings["time2"] != null) {
                time2 = (string)ConfigurationManager.AppSettings["time2"];
            }
            if (ConfigurationManager.AppSettings["time3"] != null) {
                time3 = (string)ConfigurationManager.AppSettings["time3"];
            }
            if (ConfigurationManager.AppSettings["time4"] != null) {
                time4 = (string)ConfigurationManager.AppSettings["time4"];
            }
            if (ConfigurationManager.AppSettings["time5"] != null) {
                time5 = (string)ConfigurationManager.AppSettings["time5"];
            }
            if (ConfigurationManager.AppSettings["time"] != null) {
                time = (string)ConfigurationManager.AppSettings["time"];
            }
        }

        public bool StartNotification() {
            bool started = false;

            try {
                if (!this.isProcessing) {

                    this.isProcessing = true;
                    processManagerThread = new Thread(new ThreadStart(Sending));
                    processManagerThread.Start();
                    Controller.DataProcessingStateChanged("Thread: processing started");
                    System.Console.WriteLine("++++ Processing Log files Started at:  " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\n");
                    debug.writeLog("++++ Processing email notifications Started at:  " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\n");
                    started = true;

                }
                else {
                    System.Console.WriteLine("*** Processing email notifications Started already !!! \n");
                    debug.writeLog("*** Processing email notifications Started already !!! \n");
                }
            }
            catch (Exception ex) {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + " Exception in: " +
                    this.ToString() + ".StartNotification() : " + ex.Message + "\n");
            }

            return started;
        }

        public bool StopNotification() {
            bool stopped = false;

            try {
                this.isProcessing = false;
                if (processManagerThread.Join(5000)) {
                    Controller.DataProcessingStateChanged("Thread: processing finished");
                    System.Console.WriteLine("Processing thread finished \n");
                }
                else {
                    Controller.DataProcessingStateChanged("Thread: processing aborted");
                    System.Console.WriteLine("Processing thread aborted \n");
                    processManagerThread.Abort();
                }
                System.Console.WriteLine("---- Processing email notifications Stopped at:  " + DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + "\n");
                debug.writeLog("---- Processing email notifications Stopped at:  " + DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + "\n");
                stopped = true;

            }
            catch (Exception ex) {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + " Exception in: " +
                    this.ToString() + ".StopNotification() : " + ex.Message + "\n");
                stopped = false;
            }

            return stopped;
        }

        public static NotificationManager GetInstance() {
            try {
                if (managerInstance == null) {
                    managerInstance = new NotificationManager();
                }
            }
            catch (Exception ex) {
                throw ex;
            }

            return managerInstance;
        }

        public string getStatusMessage() {

            return status;
        }

        /// <summary>
        /// if time elapsed process it.
        /// </summary>
        private void Sending() {
            const int MAX_NUMBER_OF_RETRIES = 1440;
            int numberOfRetries = 1;
            while (isProcessing) {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " A new notification process is started: " +
                               numberOfRetries.ToString());
                try {
                    bool udjiSlobodno = true;
                    while (isProcessing) {
                        string curTime = DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm");
                        //16.10.2017 Miodrag / SYNC SA NAV

                        if (udjiSlobodno) //curTime == syncNavTime)
                        {
                            udjiSlobodno = false;
                            try {
                                SyncWithNav procedura = new SyncWithNav();
                                Controller.NotificationStateChanged("Thread: Synchronization data started");
                                debug.writeBenchmarkLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SYNCHRONIZATION PROCESS STARTED!");
                                List<int> a = new List<int>();
                                a = procedura.SinhronizujPodatke();
                                debug.writeBenchmarkLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SYNCHRONIZATION PROCESS FINISHED!");
                                //List<int> lista = new List<int>() {0,2,5 };
                                PopuniLogZaAzuriraneTabele(a);
                            }
                            catch (Exception ex) {
                                debug.writeLog(ex.Message);
                            }
                            try {
                                if (dataMgr.CheckNewTags(null)) {
                                    dataMgr.FinalizeTagsTracking(true, true, null, null);
                                }
                            }
                            catch (Exception ex) {
                                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GREŠKA PRILIKOM SPUŠTANJA KARTICA NA ČITAČE " + ex.Message + "\n");
                            }
                            //Thread.Sleep(5 * 60 * 1000); //uspava petlju  
                        }

                        //mm sync sa Nav
                        //12.06.1017 Miodrag Mitrovic / Prepravka za Hutchinson
                        if (time1 == curTime || time2 == curTime || time3 == curTime || time4 == curTime)
                        {

                            numOfMailsHRM = 0;

                            Controller.NotificationStateChanged("Thread: notification process started");
                            debug.writeBenchmarkLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " NOTIFICATION PROCESS STARTED!");
                            debug.writeLog("+ GetOutstandingData - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                            List<IOPairTO> listIOPairs = GetOutstandingData();

                            debug.writeLog("- GetOutstandingData - FINISHED! Number of new ioPairs = " + listIOPairs.Count + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                            Dictionary<int, List<IOPairTO>> dictionaryIOForEmpl = new Dictionary<int, List<IOPairTO>>();
                            Dictionary<int, string> dictPastType = new Dictionary<int, string>();
                            Dictionary<int, string> dictPastTypeAlt = new Dictionary<int, string>();

                            debug.writeLog("+ getNeededData - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                            //fill dictionaries for sending mails
                            getNeededData(listIOPairs, dictionaryIOForEmpl);


                            debug.writeLog("- getNeededData - FINISHED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                            if (listIOPairs.Count <= 0)
                            {
                                lastTime = DateTime.Now;
                                Controller.NotificationStateChanged("New iopairs not found.");
                                debug.writeLog("+  New iopairs not found" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                            }
                            else
                            {

                                //send to HR Manager
                                debug.writeLog("+ sendToHRManager - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                Controller.DataProcessingStateChanged("Sending emails to HR Manager");
                                int numMailsSentToHRM = SendToHRManager(dictionaryIOForEmpl, dictPastType, dictPastTypeAlt);
                                Controller.DataProcessingStateChanged("");
                                debug.writeLog("- sendToHRManager - FINISHED! Number of sent mails = " + numMailsSentToHRM + " " + DateTime.Now.ToString("dd.MM.yyy HH:mm:ss"));
                                //(min,   s,   ms)                         
                                Thread.Sleep(60 * 60 * 1000); //uspava petlju na sat vremena nakon slanja mejla.
                                //Thread.Sleep(2 * 60 * 1000);
                            }
                        }

                        //18.06.2019 BOJAN / Za slanje izvestaja o broju rucno zatvorenih parova zaposlenih
                        else
                        {
                            if (time5 == curTime)
                            {
                                //() {//curTime) {

                                numOfMailsHRM = 0;

                                Controller.NotificationStateChanged("Thread: notification process started");
                                debug.writeBenchmarkLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " NOTIFICATION PROCESS STARTED!");
                                debug.writeLog("+ GetEmployees - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                                Common.Employee employee = new Common.Employee();
                                employee.EmplTO = new EmployeeTO();
                                employee.EmplTO.Status = Constants.statusActive;

                                List<EmployeeTO> employees = employee.Search();

                                string employeeIDs = "";

                                foreach (EmployeeTO emplTO in employees)
                                {
                                    if (!emplTO.EmployeeID.ToString().Equals("-1"))
                                    {
                                        employeeIDs += emplTO.EmployeeID.ToString().Trim() + ",";
                                    }
                                }

                                if (employeeIDs.Length > 0)
                                {
                                    employeeIDs = employeeIDs.Substring(0, employeeIDs.Length - 1);
                                }

                                debug.writeLog("- GetEmployees - FINISHED! Number of employees = " + employees.Count + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                                List<IOPairProcessedTO> listIOPairsWithManualCreatedByEmployee = new List<IOPairProcessedTO>();

                                debug.writeLog("+ getNeededData - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                                //fill list for sending mails
                                //getNeededData(listIOPairs, dictionaryIOForEmpl);

                                listIOPairsWithManualCreatedByEmployee = new IOPairProcessed().GetIOPairsWithManualCreatedByEmployee(employeeIDs, new DateTime(), new DateTime());
                                debug.writeLog("- getNeededData - FINISHED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                                if (listIOPairsWithManualCreatedByEmployee.Count <= 0)
                                {
                                    lastTime = DateTime.Now;
                                    Controller.NotificationStateChanged("New iopairs not found.");
                                    debug.writeLog("+  New iopairs not found" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                }
                                else
                                {

                                    //send to HR Manager
                                    debug.writeLog("+ sendToHRManager - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                    Controller.DataProcessingStateChanged("Sending emails to HR Manager");
                                    int numMailsSentToHRM = SendToHRManager(listIOPairsWithManualCreatedByEmployee);
                                    Controller.DataProcessingStateChanged("");
                                    debug.writeLog("- sendToHRManager - FINISHED! Number of sent mails = " + numMailsSentToHRM + " " + DateTime.Now.ToString("dd.MM.yyy HH:mm:ss"));
                                    //(min,   s,   ms)                         
                                    Thread.Sleep(60 * 60 * 1000); //uspava petlju na sat vremena nakon slanja mejla.
                                }
                            }
                        }

                        GC.Collect();
                        Common.DataManager.CloseDataBaseConnection();
                        //Thread.Sleep(processingInterval * 1000);
                    }
                }
                catch (ThreadAbortException taex) {
                    debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " NOTIFICATION PROCESS abort exception in: " +
                        this.ToString() + ".Processing() : " + taex.Message + "\n");
                }
                catch (Exception ex) {
                    debug.writeLog(DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + " Exception in: " +
                        this.ToString() + ".Processing() : " + ex.Message + "\n");
                }
                finally {
                    if (isProcessing) {
                        // exception occurred
                        Controller.NotificationStateChanged("Thread: processing restarted, retry no. " + numberOfRetries.ToString());
                        Controller.NotificationStateChanged("");
                        numberOfRetries++;
                        Thread.Sleep(60000);
                        Common.DataManager.CloseDataBaseConnection();
                    }
                }
            }
            if (numberOfRetries > MAX_NUMBER_OF_RETRIES) {
                isProcessing = false;
                Controller.NotificationStateChanged("Thread: after " + MAX_NUMBER_OF_RETRIES.ToString() + " retries processing stopped");
            }
        }
        //  19.06.2019. BOJAN
        private int SendToHRManager(List<IOPairProcessedTO> listIOPairsWithManualCreatedByEmployee) {
            //MMstart

            Excel.Application oXL = new Excel.Application();
            oXL.DisplayAlerts = false;

            //Excel.Workbook oWB = oXL.Workbooks.Add(Type.Missing);
            //Excel._Worksheet oSheet = (Excel.Worksheet) oWB.ActiveSheet;


            //MMend

            int numOfMails = 0;
            try {

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                string message = "";
                //string mail = "";
                /*
                EmployeeAsco4 emplAsco = new EmployeeAsco4();
                emplAsco.EmplAsco4TO.EmployeeID = 1000;
                List<EmployeeAsco4TO> emplAscoList = emplAsco.Search();
                EmployeeAsco4TO HRM = emplAscoList[0];

                string Ounits = HRM.Value.ToString();
                EmployeeAsco4 emplAsco4 = new EmployeeAsco4();
                emplAsco4.EmplAsco4TO.EmployeeID = HRM.Key.EmployeeID;
                List<EmployeeAsco4TO> emplAsco4List = emplAsco4.Search();
                */
                /*
                bool isAltLang = false;
                
                Employee HRM = new Employee();

                List<EmployeeTO> HRList = HRM.Search("1000"); //id HRM
                EmployeeTO HRMTO = HRList[0];
                
                EmployeeAsco4 emplAsco4 = new EmployeeAsco4();
                emplAsco4.EmplAsco4TO.EmployeeID = HRMTO.EmployeeID; 
                List<EmployeeAsco4TO> emplAsco4List = emplAsco4.Search();
    
                mail = emplAsco4List[0].NVarcharValue3;
                ApplUserTO user = new ApplUser().Find(emplAsco4List[0].NVarcharValue5);
                if (getLanguage(user).ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                {//srpski
                    isAltLang = true;
                }
                */
                rm = new ResourceManager("EmailNotificationManagement.Resource", typeof(NotificationManager).Assembly);
                culture = CultureInfo.CreateSpecificCulture(Constants.Lang_sr.Trim().ToUpper());

                if (mailTo != "") {
                    mailMessage.To.Add(mailTo);

                    mailMessage.Subject = rm.GetString("title2", culture);

                    mailMessage.From = new System.Net.Mail.MailAddress(emailAddress);

                    message = "";
                    message += "<html><body>" + rm.GetString("lblMessage3", culture) + "<br />" + rm.GetString("lblMessage4", culture) + "<br /><br />";
                    //message += rm.GetString("Employee", culture) + " <b>" + HRMTO.FirstAndLastName + "</b>, <br />";
                    message += "Spisak ljudi sa brojem ručno prepravljenih parova:<br /><br />";
                    /*
                    //message += rm.GetString("dataIntervention", culture) + "<br /><br />";
                    message += "<table border=\"1\"><tr><th>" + rm.GetString("EmployeeID", culture) + "</th><th>" + rm.GetString("Employee", culture) + "</th><th>" + rm.GetString("date", culture) + "</th><th>" + rm.GetString("start", culture) + "</th><th>" + rm.GetString("end", culture) + "</th><th>" + rm.GetString("passtype", culture) + "</th></tr>";
                    */
                    //List<EmployeeTO> listEmpl = new Employee().SearchByWULoans("2", -1, null, System.DateTime.Now.AddDays(-1), System.DateTime.Now);
                    /*
                    EmployeeResponsibility erespons = new EmployeeResponsibility();
                    erespons.ResponsibilityTO.EmployeeID = 1000;
                    List<EmployeeResponsibilityTO> list = erespons.Search();
                    
                    foreach (EmployeeResponsibilityTO emResTo in list)
                    {
                        if (emResTo.Type == "WU")
                            listEmpl.AddRange(new Employee().SearchByWULoans(emResTo.UnitID.ToString(), -1, null, fromDate, toDate));
                    }
                     * */
                    //foreach employee find iopairs and add it to message
                    /*
                    foreach (EmployeeTO Employee in listEmpl)
                    {
                     * */

                    List<Excel._Worksheet> oSheets = new List<Microsoft.Office.Interop.Excel._Worksheet>();
                    //int k = 0;

                    bool notfirst = false;
                    debug.writeLog("");


                    Excel.Workbook oWB = oXL.Workbooks.Add(Type.Missing);

                    //oSheets[k] = (Microsoft.Office.Interop.Excel._Worksheet)oWB.Sheets[1];

                    Excel._Worksheet oSheet = (Excel.Worksheet)oWB.ActiveSheet;
                    int i = 2;



                    oSheet.get_Range("A1", "G1").Font.Bold = true; //  06.08.2019. BOJAN PROMENJENO SA F1 NA G1

                    oSheet.get_Range("A1", "G" + i).ColumnWidth = 20; //  06.08.2019. BOJAN PROMENJENO SA F NA G
                    oSheet.get_Range("B1", "B" + i).ColumnWidth = 35;
                    oSheet.get_Range("E1", "E" + i).ColumnWidth = 50;
                    oSheet.get_Range("F1", "F" + i).ColumnWidth = 50;
                    oSheet.get_Range("G1", "G" + i).ColumnWidth = 50; //  06.08.2019. BOJAN
                    //oSheet.get_Range("A1", "D" + i). = 20;
                    Excel.Range range = oSheet.get_Range("A1", "G" + listIOPairsWithManualCreatedByEmployee.Count + 1);
                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    oSheet.Cells[1, 1] = rm.GetString("EmployeeID", culture);
                    oSheet.Cells[1, 2] = rm.GetString("Employee", culture);
                    oSheet.Cells[1, 3] = rm.GetString("Division", culture);
                    oSheet.Cells[1, 4] = rm.GetString("Counter", culture);
                    oSheet.Cells[1, 5] = rm.GetString("PassTypeDesc", culture);
                    //oSheet.Cells[1, 6] = rm.GetString("PassTypePaymentCode", culture);
                    oSheet.Cells[1, 6] = rm.GetString("Supervisor", culture);
                    oSheet.Cells[1, 7] = rm.GetString("date", culture); //  06.08.2019. BOJAN

                    int idOrgUnit = -1;
                    int idRadnika = -1;

                    foreach (IOPairProcessedTO singleIOPair in listIOPairsWithManualCreatedByEmployee) {
                       //Employee Zaposleni = new Employee();

                        //List<EmployeeTO> ZapList = Zaposleni.Search(singleIOPair.EmployeeID.ToString().Trim());
                        //EmployeeTO ZaposleniTO = ZapList[0];
                        //int prethodni = ZaposleniTO.EmployeeID;
                        if (notfirst)//prvi put ostaje na prvom sheet-u.
                            {
                            //if (idRadnika == ZaposleniTO.EmployeeID) {
                            //    oSheet.get_Range("A" + (i - 1), "C" + i).Interior.Color = Excel.XlRgbColor.rgbRed;// System.Drawing.Color.Red;
                            //}
                            //if (idOrgUnit != ZaposleniTO.OrgUnitID) {
                                //brojac sita povecavamo
                                //k++; 

                    //            oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oWB.Worksheets.Add
                    //(System.Reflection.Missing.Value,
                    //oWB.Worksheets[oWB.Worksheets.Count],
                    //System.Reflection.Missing.Value,
                    //System.Reflection.Missing.Value);

                                //idOrgUnit = ZaposleniTO.OrgUnitID; //niz je sortiran po org.jed.

                                //i = 2;
                                //oSheet.Cells[1, 1] = rm.GetString("EmployeeID", culture);
                                //oSheet.Cells[1, 2] = rm.GetString("Employee", culture);
                                //oSheet.Cells[1, 3] = rm.GetString("Location", culture);
                                //oSheet.Cells[1, 4] = rm.GetString("Counter", culture);
                                //OrganizationalUnit ou = new OrganizationalUnit();
                                //List<OrganizationalUnitTO> ListOU = ou.Search(idOrgUnit.ToString().Trim());


                                //oSheet.get_Range("A1", "D1").Font.Bold = true;

                                //oSheet.get_Range("A1", "D" + i).ColumnWidth = 20;
                                //oSheet.get_Range("B1", "B" + i).ColumnWidth = 35;
                                //oSheet.Name = ListOU[0].Name;

                            //}
                        }
                        else {

                            //OrganizationalUnit ou = new OrganizationalUnit();
                            //List<OrganizationalUnitTO> ListOU = ou.Search(ZaposleniTO.OrgUnitID.ToString().Trim());
                            oSheet.get_Range("A1", "G1").Font.Bold = true; //  06.08.2019. BOJAN PROMENJENO SA F1 NA G1

                            oSheet.get_Range("A1", "D" + i).ColumnWidth = 20;
                            oSheet.get_Range("B1", "B" + i).ColumnWidth = 35;
                            //idOrgUnit = ZaposleniTO.OrgUnitID;
                            //oSheet.Name = ListOU[0].Name;

                            notfirst = true;
                        }
                        //mm popunjavanje ovde.

                        //idRadnika = ZaposleniTO.EmployeeID;

                        oSheet.Cells[i, 1] = singleIOPair.EmployeeID;
                        oSheet.Cells[i, 2] = singleIOPair.FirstName +" "+singleIOPair.LastName;
                        oSheet.Cells[i, 3] = singleIOPair.Division;
                        oSheet.Cells[i, 4] = singleIOPair.ManualCreated;
                        oSheet.Cells[i, 5] = singleIOPair.PTDesc + " " + singleIOPair.PassTypePaymentCode;
                        //oSheet.Cells[i, 6] = singleIOPair.PassTypePaymentCode;
                        oSheet.Cells[i, 6] = singleIOPair.OrgName;    //  06.08.2019. BOJAN promenjeno sa oSheet.Cells[i++, 6] na oSheet.Cells[i, 6] = singleIOPair.OrgName;
                        oSheet.Cells[i++, 7] = singleIOPair.IOPairDate.ToString("dd.MM.yyyy");    //  06.08.2019. BOJAN
                        //mm

                        //message += "<tr><td>" + singleIOPair.EmployeeID + "</td><td>" + ZaposleniTO.FirstAndLastName + "</td><td>" + singleIOPair.IOPairDate.Date.ToString("dd.MM.yyyy") + "</td><td>" + singleIOPair.StartTime.ToShortTimeString() + "</td><td>" + singleIOPair.EndTime.ToShortTimeString() + "</td><td>" + pto.Description + "</td></tr>";

                        //if (isAltLang)
                        //message += desc + "</td></tr>";
                        /*
                        else
                            message += desc + "</td></tr>";
                        */

                    }




                    //foreach (KeyValuePair<int, List<IOPairTO>> pairForEmpl in dictionaryIOForEmpl) {
                    //    //if (pairForEmpl.Key != HRMTO.EmployeeID)
                    //    //{
                    //    List<IOPairTO> listIOPairsForEmpl = pairForEmpl.Value;

                    //    foreach (IOPairTO singleIOPair in listIOPairsForEmpl) {
                    //        Employee Zaposleni = new Employee();

                    //        List<EmployeeTO> ZapList = Zaposleni.Search(singleIOPair.EmployeeID.ToString().Trim());
                    //        EmployeeTO ZaposleniTO = ZapList[0];
                    //        int prethodni = ZaposleniTO.EmployeeID;
                    //        if (notfirst)//prvi put ostaje na prvom sheet-u.
                    //        {
                    //            if (idRadnika == ZaposleniTO.EmployeeID) {
                    //                oSheet.get_Range("A" + (i - 1), "C" + i).Interior.Color = Excel.XlRgbColor.rgbRed;// System.Drawing.Color.Red;
                    //            }
                    //            if (idOrgUnit != ZaposleniTO.OrgUnitID) {
                    //                //brojac sita povecavamo
                    //                //k++; 

                    //                oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oWB.Worksheets.Add
                    //    (System.Reflection.Missing.Value,
                    //    oWB.Worksheets[oWB.Worksheets.Count],
                    //    System.Reflection.Missing.Value,
                    //    System.Reflection.Missing.Value);

                    //                idOrgUnit = ZaposleniTO.OrgUnitID; //niz je sortiran po org.jed.

                    //                i = 2;
                    //                oSheet.Cells[1, 1] = rm.GetString("EmployeeID", culture);
                    //                oSheet.Cells[1, 2] = rm.GetString("Employee", culture);
                    //                oSheet.Cells[1, 3] = rm.GetString("start", culture);
                    //                OrganizationalUnit ou = new OrganizationalUnit();
                    //                List<OrganizationalUnitTO> ListOU = ou.Search(idOrgUnit.ToString().Trim());


                    //                oSheet.get_Range("A1", "D1").Font.Bold = true;

                    //                oSheet.get_Range("A1", "D" + i).ColumnWidth = 20;
                    //                oSheet.get_Range("B1", "B" + i).ColumnWidth = 35;
                    //                oSheet.Name = ListOU[0].Name;

                    //            }
                    //        }
                    //        else {

                    //            OrganizationalUnit ou = new OrganizationalUnit();
                    //            List<OrganizationalUnitTO> ListOU = ou.Search(ZaposleniTO.OrgUnitID.ToString().Trim());
                    //            oSheet.get_Range("A1", "D1").Font.Bold = true;

                    //            oSheet.get_Range("A1", "D" + i).ColumnWidth = 20;
                    //            oSheet.get_Range("B1", "B" + i).ColumnWidth = 35;
                    //            idOrgUnit = ZaposleniTO.OrgUnitID;
                    //            oSheet.Name = ListOU[0].Name;

                    //            notfirst = true;
                    //        }
                    //        //mm popunjavanje ovde.

                    //        idRadnika = ZaposleniTO.EmployeeID;

                    //        oSheet.Cells[i, 1] = singleIOPair.EmployeeID;
                    //        oSheet.Cells[i, 2] = ZaposleniTO.FirstAndLastName;
                    //        oSheet.Cells[i++, 3] = singleIOPair.StartTime.ToString("dd.MM.yyy HH:mm:ss");

                    //        //mm

                    //        //message += "<tr><td>" + singleIOPair.EmployeeID + "</td><td>" + ZaposleniTO.FirstAndLastName + "</td><td>" + singleIOPair.IOPairDate.Date.ToString("dd.MM.yyyy") + "</td><td>" + singleIOPair.StartTime.ToShortTimeString() + "</td><td>" + singleIOPair.EndTime.ToShortTimeString() + "</td><td>" + pto.Description + "</td></tr>";

                    //        //if (isAltLang)
                    //        //message += desc + "</td></tr>";
                    //        /*
                    //        else
                    //            message += desc + "</td></tr>";
                    //        */
                    //    }
                    //}





                    //}
                    //mm }
                    //mail bottom
                    /*
                    if (!message.EndsWith(rm.GetString("passtype", culture) + "</th></tr>"))
                    {
                        */
                    message += "</table><br />";
                    message += "<table><tr><td> </td></tr><tr><td> </td></tr><tr><td><font size=2 face=\"Helvetica\" >" + rm.GetString("pleaseDoNot", culture) + "</font></td></tr>";
                    message += "<tr><td><font size=1 face=\"Helvetica\"color=black> This e-mail is automatically generated by ActA System (powered by SDDITG,  www.sdditg.com). </font></td></tr>";
                    message += "<tr><td><font size=1 face=\"Helvetica\"color=black>Generated: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " </font></td></tr>";
                    bool sddLogoFound = File.Exists(Constants.SDDITGLogoPath);
                    if (sddLogoFound) {
                        message += "<tr><td><img src='cid:logo_bottom' align=\"left\"></td></tr></table></body></html>";
                        System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(message, new System.Net.Mime.ContentType("text/html"));
                        System.Net.Mail.LinkedResource logo = new System.Net.Mail.LinkedResource(Constants.SDDITGLogoPath); // + sHeaderImg);     
                        logo.ContentId = "logo_bottom";
                        htmlView.LinkedResources.Add(logo);
                        mailMessage.AlternateViews.Add(htmlView);
                    }
                    mailMessage.IsBodyHtml = true;

                    mailMessage.Body = message;

                    if (i != 2) {
                        String NameFile = "Ručno prepravljeni parovi na dan " + DateTime.Now.ToString("yyyy-MM-dd-HH") + ".xlsx";
                        String ReportFile = ReportsDirectory + NameFile;

                        /*
                        oWB.SaveAs(ReportFile, Excel.XlFileFormat.xlOpenXMLWorkbook, Missing.Value,
    Missing.Value, false, false, Excel.XlSaveAsAccessMode.xlNoChange,
    Excel.XlSaveConflictResolution.xlUserResolution, true,
    Missing.Value, Missing.Value, Missing.Value);
                         * */

                        oWB.SaveAs(ReportFile, Excel.XlFileFormat.xlWorkbookDefault,
                                                Type.Missing, Type.Missing,
                                                false, false, Excel.XlSaveAsAccessMode.xlNoChange,
                                                Type.Missing, Type.Missing, Type.Missing,
                                                Type.Missing, Type.Missing);

                        oXL.Quit();

                        Marshal.ReleaseComObject(oSheet);
                        Marshal.ReleaseComObject(oWB);
                        Marshal.ReleaseComObject(oXL);

                        oSheets = null;
                        oWB = null;
                        oXL = null;
                        GC.GetTotalMemory(false);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                        GC.GetTotalMemory(true);

                        Thread.Sleep(5000);

                        //var attachment = new Attachment(File.Open(ReportFile, FileMode.Open), "Prisutni radnici.xlsx");
                        Attachment attachment = new Attachment(ReportFile);
                        //Attachment attachment = new Attachment(File.Open(rootDirectory, FileMode.Open), "Prisutni.xlsx"); 
                        attachment.ContentType = new ContentType("application/vnd.ms-excel");
                        mailMessage.Attachments.Add(attachment);

                        try {
                            smtp.Send(mailMessage);
                            numOfMails++;
                            debug.writeLog(DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + " Mail sent to addresses: " + mailTo);
                        }
                        catch (Exception ex) {
                            debug.writeLog(DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + " Exception in: " +
                                this.ToString() + ".SendingMailToHRManager() : " + ex.Message + "\n");
                            return -1;

                        }
                        finally {
                            attachment.Dispose();
                        }
                    }
                }
            }

            catch (Exception ex) {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + " Exception in: " +
                    this.ToString() + ".SendToHRManager() : " + ex.Message + "\n");
            }
            return numOfMails;
        }

        public static string getLanguage(object user) {
            try {
                string lang = Constants.Lang_sr;

                if (user != null && user is ApplUserTO)
                    lang = ((ApplUserTO)user).LangCode;

                return lang;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        private int SendToHRManager(Dictionary<int, List<IOPairTO>> dictionaryIOForEmpl, Dictionary<int, string> dictPastType, Dictionary<int, string> dictPastTypeAlt) {
            //MMstart

            Excel.Application oXL = new Excel.Application();
            oXL.DisplayAlerts = false;

            //Excel.Workbook oWB = oXL.Workbooks.Add(Type.Missing);
            //Excel._Worksheet oSheet = (Excel.Worksheet) oWB.ActiveSheet;


            //MMend

            int numOfMails = 0;
            try {

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                string message = "";
                //string mail = "";
                /*
                EmployeeAsco4 emplAsco = new EmployeeAsco4();
                emplAsco.EmplAsco4TO.EmployeeID = 1000;
                List<EmployeeAsco4TO> emplAscoList = emplAsco.Search();
                EmployeeAsco4TO HRM = emplAscoList[0];

                string Ounits = HRM.Value.ToString();
                EmployeeAsco4 emplAsco4 = new EmployeeAsco4();
                emplAsco4.EmplAsco4TO.EmployeeID = HRM.Key.EmployeeID;
                List<EmployeeAsco4TO> emplAsco4List = emplAsco4.Search();
                */
                /*
                bool isAltLang = false;
                
                Employee HRM = new Employee();

                List<EmployeeTO> HRList = HRM.Search("1000"); //id HRM
                EmployeeTO HRMTO = HRList[0];
                
                EmployeeAsco4 emplAsco4 = new EmployeeAsco4();
                emplAsco4.EmplAsco4TO.EmployeeID = HRMTO.EmployeeID; 
                List<EmployeeAsco4TO> emplAsco4List = emplAsco4.Search();
    
                mail = emplAsco4List[0].NVarcharValue3;
                ApplUserTO user = new ApplUser().Find(emplAsco4List[0].NVarcharValue5);
                if (getLanguage(user).ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                {//srpski
                    isAltLang = true;
                }
                */
                rm = new ResourceManager("EmailNotificationManagement.Resource", typeof(NotificationManager).Assembly);
                culture = CultureInfo.CreateSpecificCulture(Constants.Lang_sr.Trim().ToUpper());

                if (mailTo != "") {
                    mailMessage.To.Add(mailTo);

                    mailMessage.Subject = rm.GetString("title", culture);

                    mailMessage.From = new System.Net.Mail.MailAddress(emailAddress);

                    message = "";
                    message += "<html><body>" + rm.GetString("lblMessage1", culture) + "<br />" + rm.GetString("lblMessage2", culture) + "<br /><br />";
                    //message += rm.GetString("Employee", culture) + " <b>" + HRMTO.FirstAndLastName + "</b>, <br />";
                    message += "Spisak ljudi koji se nalaze u objektu (sa nezatvorenim parom prolaska):<br /><br />";
                    /*
                    //message += rm.GetString("dataIntervention", culture) + "<br /><br />";
                    message += "<table border=\"1\"><tr><th>" + rm.GetString("EmployeeID", culture) + "</th><th>" + rm.GetString("Employee", culture) + "</th><th>" + rm.GetString("date", culture) + "</th><th>" + rm.GetString("start", culture) + "</th><th>" + rm.GetString("end", culture) + "</th><th>" + rm.GetString("passtype", culture) + "</th></tr>";
                    */
                    //List<EmployeeTO> listEmpl = new Employee().SearchByWULoans("2", -1, null, System.DateTime.Now.AddDays(-1), System.DateTime.Now);
                    /*
                    EmployeeResponsibility erespons = new EmployeeResponsibility();
                    erespons.ResponsibilityTO.EmployeeID = 1000;
                    List<EmployeeResponsibilityTO> list = erespons.Search();
                    
                    foreach (EmployeeResponsibilityTO emResTo in list)
                    {
                        if (emResTo.Type == "WU")
                            listEmpl.AddRange(new Employee().SearchByWULoans(emResTo.UnitID.ToString(), -1, null, fromDate, toDate));
                    }
                     * */
                    //foreach employee find iopairs and add it to message
                    /*
                    foreach (EmployeeTO Employee in listEmpl)
                    {
                     * */

                    List<Excel._Worksheet> oSheets = new List<Microsoft.Office.Interop.Excel._Worksheet>();
                    //int k = 0;

                    bool notfirst = false;
                    debug.writeLog("");


                    Excel.Workbook oWB = oXL.Workbooks.Add(Type.Missing);

                    //oSheets[k] = (Microsoft.Office.Interop.Excel._Worksheet)oWB.Sheets[1];

                    Excel._Worksheet oSheet = (Excel.Worksheet)oWB.ActiveSheet;
                    int i = 2;
                    oSheet.Cells[1, 1] = rm.GetString("EmployeeID", culture);
                    oSheet.Cells[1, 2] = rm.GetString("Employee", culture);
                    oSheet.Cells[1, 3] = rm.GetString("start", culture);
                    //oSheet.Cells[1, 4] = rm.GetString("start", culture);

                    int idOrgUnit = -1;
                    int idRadnika = -1;
                    foreach (KeyValuePair<int, List<IOPairTO>> pairForEmpl in dictionaryIOForEmpl) {
                        //if (pairForEmpl.Key != HRMTO.EmployeeID)
                        //{
                        List<IOPairTO> listIOPairsForEmpl = pairForEmpl.Value;

                        foreach (IOPairTO singleIOPair in listIOPairsForEmpl) {
                            Employee Zaposleni = new Employee();

                            List<EmployeeTO> ZapList = Zaposleni.Search(singleIOPair.EmployeeID.ToString().Trim());
                            EmployeeTO ZaposleniTO = ZapList[0];
                            int prethodni = ZaposleniTO.EmployeeID;
                            if (notfirst)//prvi put ostaje na prvom sheet-u.
                            {
                                if (idRadnika == ZaposleniTO.EmployeeID) {
                                    oSheet.get_Range("A" + (i - 1), "C" + i).Interior.Color = Excel.XlRgbColor.rgbRed;// System.Drawing.Color.Red;
                                }
                                if (idOrgUnit != ZaposleniTO.OrgUnitID) {
                                    //brojac sita povecavamo
                                    //k++; 

                                    oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oWB.Worksheets.Add
                        (System.Reflection.Missing.Value,
                        oWB.Worksheets[oWB.Worksheets.Count],
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value);

                                    idOrgUnit = ZaposleniTO.OrgUnitID; //niz je sortiran po org.jed.

                                    i = 2;
                                    oSheet.Cells[1, 1] = rm.GetString("EmployeeID", culture);
                                    oSheet.Cells[1, 2] = rm.GetString("Employee", culture);
                                    oSheet.Cells[1, 3] = rm.GetString("start", culture);
                                    OrganizationalUnit ou = new OrganizationalUnit();
                                    List<OrganizationalUnitTO> ListOU = ou.Search(idOrgUnit.ToString().Trim());


                                    oSheet.get_Range("A1", "D1").Font.Bold = true;

                                    oSheet.get_Range("A1", "D" + i).ColumnWidth = 20;
                                    oSheet.get_Range("B1", "B" + i).ColumnWidth = 35;
                                    oSheet.Name = ListOU[0].Name;

                                }
                            }
                            else {

                                OrganizationalUnit ou = new OrganizationalUnit();
                                List<OrganizationalUnitTO> ListOU = ou.Search(ZaposleniTO.OrgUnitID.ToString().Trim());
                                oSheet.get_Range("A1", "D1").Font.Bold = true;

                                oSheet.get_Range("A1", "D" + i).ColumnWidth = 20;
                                oSheet.get_Range("B1", "B" + i).ColumnWidth = 35;
                                idOrgUnit = ZaposleniTO.OrgUnitID;
                                oSheet.Name = ListOU[0].Name;

                                notfirst = true;
                            }
                            //mm popunjavanje ovde.

                            idRadnika = ZaposleniTO.EmployeeID;

                            oSheet.Cells[i, 1] = singleIOPair.EmployeeID;
                            oSheet.Cells[i, 2] = ZaposleniTO.FirstAndLastName;
                            oSheet.Cells[i++, 3] = singleIOPair.StartTime.ToString("dd.MM.yyy HH:mm:ss");

                            //mm

                            //message += "<tr><td>" + singleIOPair.EmployeeID + "</td><td>" + ZaposleniTO.FirstAndLastName + "</td><td>" + singleIOPair.IOPairDate.Date.ToString("dd.MM.yyyy") + "</td><td>" + singleIOPair.StartTime.ToShortTimeString() + "</td><td>" + singleIOPair.EndTime.ToShortTimeString() + "</td><td>" + pto.Description + "</td></tr>";

                            //if (isAltLang)
                            //message += desc + "</td></tr>";
                            /*
                            else
                                message += desc + "</td></tr>";
                            */

                        }


                    }
                    //}
                    //mm }
                    //mail bottom
                    /*
                    if (!message.EndsWith(rm.GetString("passtype", culture) + "</th></tr>"))
                    {
                        */
                    message += "</table><br />";
                    message += "<table><tr><td> </td></tr><tr><td> </td></tr><tr><td><font size=2 face=\"Helvetica\" >" + rm.GetString("pleaseDoNot", culture) + "</font></td></tr>";
                    message += "<tr><td><font size=1 face=\"Helvetica\"color=black> This e-mail is automatically generated by ActA System (powered by SDDITG,  www.sdditg.com). </font></td></tr>";
                    message += "<tr><td><font size=1 face=\"Helvetica\"color=black>Generated: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " </font></td></tr>";
                    bool sddLogoFound = File.Exists(Constants.SDDITGLogoPath);
                    if (sddLogoFound) {
                        message += "<tr><td><img src='cid:logo_bottom' align=\"left\"></td></tr></table></body></html>";
                        System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(message, new System.Net.Mime.ContentType("text/html"));
                        System.Net.Mail.LinkedResource logo = new System.Net.Mail.LinkedResource(Constants.SDDITGLogoPath); // + sHeaderImg);     
                        logo.ContentId = "logo_bottom";
                        htmlView.LinkedResources.Add(logo);
                        mailMessage.AlternateViews.Add(htmlView);
                    }
                    mailMessage.IsBodyHtml = true;

                    mailMessage.Body = message;

                    if (i != 2) {
                        String NameFile = "Prisutni" + DateTime.Now.ToString("yyyy-MM-dd-HH") + ".xlsx";
                        String ReportFile = ReportsDirectory + NameFile;

                        /*
                        oWB.SaveAs(ReportFile, Excel.XlFileFormat.xlOpenXMLWorkbook, Missing.Value,
    Missing.Value, false, false, Excel.XlSaveAsAccessMode.xlNoChange,
    Excel.XlSaveConflictResolution.xlUserResolution, true,
    Missing.Value, Missing.Value, Missing.Value);
                         * */

                        oWB.SaveAs(ReportFile, Excel.XlFileFormat.xlWorkbookDefault,
                                                Type.Missing, Type.Missing,
                                                false, false, Excel.XlSaveAsAccessMode.xlNoChange,
                                                Type.Missing, Type.Missing, Type.Missing,
                                                Type.Missing, Type.Missing);

                        oXL.Quit();

                        Marshal.ReleaseComObject(oSheet);
                        Marshal.ReleaseComObject(oWB);
                        Marshal.ReleaseComObject(oXL);

                        oSheets = null;
                        oWB = null;
                        oXL = null;
                        GC.GetTotalMemory(false);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                        GC.GetTotalMemory(true);

                        Thread.Sleep(5000);

                        //var attachment = new Attachment(File.Open(ReportFile, FileMode.Open), "Prisutni radnici.xlsx");
                        Attachment attachment = new Attachment(ReportFile);
                        //Attachment attachment = new Attachment(File.Open(rootDirectory, FileMode.Open), "Prisutni.xlsx"); 
                        attachment.ContentType = new ContentType("application/vnd.ms-excel");
                        mailMessage.Attachments.Add(attachment);

                        try {
                            smtp.Send(mailMessage);
                            numOfMails++;
                            debug.writeLog(DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + " Mail sent to addresses: " + mailTo);
                        }
                        catch (Exception ex) {
                            debug.writeLog(DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + " Exception in: " +
                                this.ToString() + ".SendingMailToHRManager() : " + ex.Message + "\n");
                            return -1;

                        }
                        finally {
                            attachment.Dispose();
                        }
                    }
                }
            }

            catch (Exception ex) {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + " Exception in: " +
                    this.ToString() + ".SendToHRManager() : " + ex.Message + "\n");
            }
            return numOfMails;
        }

        private void getNeededData(List<IOPairTO> list, Dictionary<int, List<IOPairTO>> dictionaryIOForEmpl) {
            try {
                foreach (IOPairTO iopair in list) {
                    List<IOPairTO> newList = new List<IOPairTO>();
                    if (dictionaryIOForEmpl.ContainsKey(iopair.EmployeeID))
                        dictionaryIOForEmpl[iopair.EmployeeID].Add(iopair);
                    else {
                        newList.Add(iopair);
                        dictionaryIOForEmpl.Add(iopair.EmployeeID, newList);
                    }
                }

            }
            catch (Exception ex) {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + " Exception in: " +
                    this.ToString() + ".getNeededData() : " + ex.Message + "\n");
            }

        }

        private List<IOPairTO> GetOutstandingData() {
            List<IOPairTO> pairs = new List<IOPairTO>();
            if (System.DateTime.Now.DayOfWeek != DayOfWeek.Sunday) {
                try {

                    List<DateTime> datesList = new List<DateTime>();
                    int month = DateTime.Now.Month;
                    int hrsscCutoffDate = -1;
                    Common.Rule rule = new Common.Rule();

                    rule.RuleTO.RuleType = Constants.RuleHRSSCCutOffDate;
                    List<RuleTO> rules = rule.Search();
                    hrsscCutoffDate = rules[0].RuleValue;

                    if (System.DateTime.Now.Hour > 7 && System.DateTime.Now.DayOfWeek != DayOfWeek.Saturday) {

                        if (System.DateTime.Now.Hour > 12) {
                            fromDate = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, System.DateTime.Now.Hour - 9, System.DateTime.Now.Minute, 0);
                        }
                        else {
                            fromDate = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 20, 0, 0);

                            fromDate = fromDate.AddDays(-1);

                        }
                    }
                    else if (System.DateTime.Now.DayOfWeek != DayOfWeek.Monday) {
                        fromDate = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 20, 0, 0);
                        fromDate = fromDate.AddDays(-1);
                    }




                    toDate = System.DateTime.Now;

                    /*
                    List<IOPairTO> PairsForAllEmp = new List<IOPairTO>();
                    pairs = new IOPair().SearchEmplTimeInterval("", fromDate, toDate, null);
                    */

                    List<IOPairTO> PairsForAllEmp = new List<IOPairTO>();
                    PairsForAllEmp = new IOPair().SearchEmplTimeInterval2("", fromDate, toDate, null);
                    foreach (IOPairTO e in PairsForAllEmp) {
                        pairs.Add(e);
                    }
                }
                catch (Exception ex) {
                    debug.writeLog(DateTime.Now.ToString("dd.MM.yyy HH:mm:ss") + " Exception in: " +
                       this.ToString() + ".GetOutstandingData() : " + ex.Message + "\n");

                }
            }
            return pairs;

        }

        private void PopuniLogZaAzuriraneTabele(List<int> a) {
            string s = "Upisano je:";
            int br = 0;
            foreach (int i in a) {
                if (i == 111111) {
                    s += "\nSync FAILED\n";
                    br = 0;
                }
                if (br == 0) {
                    s = "\nInserting of working units\n";
                }
                else if (br == 2) {
                    debug.writeLog(s);
                    s = "\nInserting of organizational units\n";
                }
                else if (br == 5) {
                    debug.writeLog(s);
                    s = "\nInserting of employees\n";
                }
                else if (br == 12) {
                    debug.writeLog(s);
                    s = "\nUpdating of working units\n";
                }
                else if (br == 15) {
                    debug.writeLog(s);
                    s = "\nUpdating of organizational units\n";
                }
                else if (br == 19) {
                    debug.writeLog(s);
                    s = "\nUpdating of employees\n";
                }
                else if (br == 23) {
                    debug.writeLog(s);
                    s = "\nDeleting of working units\n";
                }
                else if (br == 25) {
                    debug.writeLog(s);
                    s = "\nDeleting of organizational units\n";
                }
                else if (br == 28) {
                    debug.writeLog(s);
                    s = "\nDeleting of employees\n";
                }
                s += "\t" + i;
                br++;
            }
            debug.writeLog(s);
        }

    }
}
