using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Common;
using Util;
using DataAccess;
using ReaderRemoteManagement;
using TransferObjects;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ReaderRemoteControlService
{
    [ServiceContract(Namespace = "http://ReaderRemoteControlService")]
    public interface IReaderRemoteControlService
    {
        [OperationContract]
        void StartTicketProcessing();
        [OperationContract]
        void StopTicketProcessing();
        [OperationContract]
        string GetValidationTerminalIP();
        [OperationContract]
        string GetManagerStatus();
        [OperationContract]
        string GetCardNumbers();
        [OperationContract]
        string GetCounters();
        [OperationContract]
        string GetManagerThreadStatus();

    }


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                   IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(
     RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReaderRemoteControlService : IReaderRemoteControlService
    {
        private static ReaderRemoteControlService instance = null;
        private Dictionary<int, ReaderRemoteControlManager> ticketControlManagerList;

        // Debug
        private DebugLog log;

        private string serviceStatus = "";

        private object locker = new object();
        private static object instanceLocker = new object();

        public System.Timers.Timer timerDB;
        public System.Timers.Timer timerDropArms;
        bool DBconnected = false;

        DAOFactory daoFactory = null;
        string noDBConnectionString = "Cannot connect to the database!";


        public ReaderRemoteControlService()
        {
            // Init Debug
            NotificationController.SetApplicationName("ReaderRemoteControlService");
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            ticketControlManagerList = new Dictionary<int, ReaderRemoteControlManager>();

            // init ticket controller
            try
            {
                try
                {
                    daoFactory = DAOFactory.getDAOFactory();
                    DBconnected = true;

                    InitializeTicketControlManager();

                    log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ReaderRemoteControlService.ReaderRemoteControlService() connection to database established");
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ReaderRemoteControlService.ReaderRemoteControlService() " + ex.Message);
                    DBconnected = false;
                    SetServiceStatus(noDBConnectionString);
                }
                if (!DBconnected)
                {
                    // timer for checking database connection
                    timerDB = new System.Timers.Timer(Constants.dbRefreshTime);
                    timerDB.Elapsed += new System.Timers.ElapsedEventHandler(timerDB_Elapsed);
                    timerDB.Start();
                    log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ReaderRemoteControlService.ReaderRemoteControlService() timerDB started");
                }

            }
            catch (Exception ex)
            {
                log.writeLog(ex);
            }
        }

        private void tryToConnect()
        {
            try
            {
                try
                {
                    if (daoFactory == null)
                    {
                        try
                        {
                            daoFactory = DAOFactory.getDAOFactory();
                            DBconnected = true;

                            InitializeTicketControlManager();
                            StartTicketProcessing();
                            timerDB.Enabled = false;
                            log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ReaderRemoteControlService.tryToConnect() connection to database established.");
                        }
                        catch (Exception ex)
                        {
                            log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ReaderRemoteControlService.tryToConnect() exception: " + ex.Message);
                        }
                    }
                    else
                    {
                        DBconnected = daoFactory.TestDataSourceConnection();
                    }
                }
                catch
                {
                    DBconnected = false;
                }

            }
            catch (Exception ex)
            {
                DBconnected = false;
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ReaderRemoteControlService.tryToConnect(): " + ex.Message + "\n");
            }
        }

        private void timerDB_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                try
                {
                    if (!DBconnected)
                    {
                        SetServiceStatus(noDBConnectionString);
                        tryToConnect();
                    }
                    else if (serviceStatus.Equals(noDBConnectionString))
                    {
                        SetServiceStatus("");
                    }
                }
                catch
                {
                    DBconnected = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ReaderRemoteControlService.timerDB_Elapsed(): " + ex.Message + "\n");
            }
        }


        private bool TestConnection()
        {
            bool connExists = false;

            try
            {
                string connectionString = "";

                try
                {
                    connectionString = ConfigurationManager.AppSettings["connectionString"];
                    connExists = connectionString != null;
                }
                catch
                {
                    connExists = false;
                }

                if (!connExists)
                    return connExists;

                try
                {
                    byte[] buffer = Convert.FromBase64String(connectionString);
                    connectionString = Util.Misc.decrypt(buffer);
                }
                catch
                {
                    connExists = false;
                }

                if (!connExists)
                    return connExists;

                string dataProvider = "";
                int startIndex = -1;
                int endIndex = -1;

                startIndex = connectionString.ToLower().IndexOf("data provider");

                if (startIndex >= 0)
                {
                    endIndex = connectionString.IndexOf(";", startIndex);

                    if (endIndex >= startIndex)
                    {
                        // take data provider value
                        // data provider part of the connection string is like "data provider=mysql;" and we need "mysql"
                        // or string is like "data provider=sqlserver;" and we need "sqlserver"
                        startIndex = connectionString.IndexOf("=", startIndex);
                        if (startIndex >= 0)
                            dataProvider = connectionString.Substring(startIndex + 1, endIndex - startIndex - 1);
                    }
                }

                startIndex = -1;

                startIndex = connectionString.ToLower().IndexOf("server=");

                if (startIndex >= 0)
                {
                    connectionString = connectionString.Substring(startIndex);
                }

                switch (dataProvider.ToLower())
                {
                    case "mysql":
                        {
                            //MySqlConnection connection;

                            //connection = new MySqlConnection(connectionString);
                            //connection.Open();
                            //if (connection.State.Equals(ConnectionState.Open))
                            //{
                            //    connExists = true;
                            //}
                            //else
                            //{
                            //    connExists = false;
                            //}
                            break;
                        }
                    case "sqlserver":
                        {
                            SqlConnection connection;

                            connection = new SqlConnection(connectionString);
                            connection.Open();
                            if (connection.State.Equals(ConnectionState.Open))
                            {
                                connExists = true;
                            }
                            else
                            {
                                connExists = false;
                            }
                            break;
                        }
                    case "":
                        {
                            connExists = false;
                            break;
                        }
                    default:
                        {
                            connExists = false;
                            break;
                        }
                }
            }
            catch
            {
                connExists = false;
            }

            return connExists;
        }

        public static ReaderRemoteControlService Instance
        {
            get
            {
                lock (instanceLocker)
                {
                    if (instance == null)
                    {
                        instance = new ReaderRemoteControlService();
                    }
                    return instance;
                }
            }
        }

        private bool InitializeTicketControlManager()
        {
            bool success = false;
            try
            {
                string readerIds = "";
                string pointsIDs = "";
                if (ConfigurationManager.AppSettings["Readers"] != null)
                {
                    try
                    {
                        readerIds = (string)ConfigurationManager.AppSettings["Readers"];
                    }
                    catch { }
                }



                if (ConfigurationManager.AppSettings["Points"] != null)
                {
                    try
                    {
                        pointsIDs = (string)ConfigurationManager.AppSettings["Points"];
                    }
                    catch { }
                }


                string[] pointID = pointsIDs.Split(';');
                string p = "";
                Dictionary<string, string> dictionaryPoints = new Dictionary<string, string>();

                foreach (string po in pointID)
                {
                    p += po.Remove(po.IndexOf(':')) + ",";

                    dictionaryPoints.Add(po.Remove(po.IndexOf(':')), po.Substring(po.IndexOf(':') + 1));

                }
                if (p.Length > 0)
                    p = p.Remove(p.LastIndexOf(','));

                List<OnlineMealsPointsTO> points = new OnlineMealsPoints().searchForIDs(p);

                List<ValidationTerminalTO> vtList = new List<ValidationTerminalTO>();

                Dictionary<string, ValidationTerminalTO> vtDict = new Dictionary<string, ValidationTerminalTO>();


                foreach (OnlineMealsPointsTO point in points)
                {
                    if (!vtDict.ContainsKey(point.ReaderIPAddress.Trim()))
                    {
                        ValidationTerminalTO terminal = new ValidationTerminalTO();
                        terminal.IpAddress = point.ReaderIPAddress;
                        terminal.Name = point.RestaurantName;
                        terminal.Status = "ENABLED";
                        terminal.Description = point.RestaurantName;
                        terminal.ValidationTerminalID = point.RestaurantID;
                        if (!terminal.Locations.ContainsKey(point.Reader_ant))
                            terminal.Locations.Add(point.Reader_ant, dictionaryPoints[point.PointID.ToString()]);

                        vtDict.Add(terminal.IpAddress, terminal);

                    }
                    else
                    {
                        vtDict[point.ReaderIPAddress].Locations.Add(point.Reader_ant, dictionaryPoints[point.PointID.ToString()]);
                    }

                }
                ticketControlManagerList = new Dictionary<int, ReaderRemoteControlManager>();
                int ticketTransactionTimeOut = 10000;
                int PassTimeOut = 6000;
                int blockPassInterval = 500;
                int pingReaderTime = 600000;
                int logDebugMessages = 1;
                int ScreenTimeOut = 0;
                int downloadInterval = 0;
                string downloadStartTime = "00:00";
                string numberOfMealsTime = "00:00";
                if (ConfigurationManager.AppSettings["TicketTransactionTimeOut"] != null)
                {
                    try
                    {
                        ticketTransactionTimeOut = int.Parse((string)ConfigurationManager.AppSettings["TicketTransactionTimeOut"]);
                    }
                    catch { }
                }

                if (ConfigurationManager.AppSettings["PassTimeOut"] != null)
                {
                    try
                    {
                        PassTimeOut = int.Parse((string)ConfigurationManager.AppSettings["PassTimeOut"]);
                    }
                    catch { }
                }
                if (ConfigurationManager.AppSettings["ScreenTimeOut"] != null)
                {
                    try
                    {
                        ScreenTimeOut = int.Parse((string)ConfigurationManager.AppSettings["ScreenTimeOut"]);
                    }
                    catch { }
                }
                if (ConfigurationManager.AppSettings["BlockPassInterval"] != null)
                {
                    try
                    {
                        blockPassInterval = int.Parse((string)ConfigurationManager.AppSettings["BlockPassInterval"]);
                    }
                    catch { }
                }
                if (ConfigurationManager.AppSettings["readerPingPeriod"] != null)
                {
                    try
                    {
                        pingReaderTime = int.Parse((string)ConfigurationManager.AppSettings["readerPingPeriod"]) * 60 * 1000;
                    }
                    catch { }
                }
                if (ConfigurationManager.AppSettings["DebugLevel"] != null)
                {
                    try
                    {
                        logDebugMessages = int.Parse((string)ConfigurationManager.AppSettings["DebugLevel"]);
                    }
                    catch { }
                }

                if (ConfigurationManager.AppSettings["DownloadInterval"] != null)
                {
                    try
                    {
                        downloadInterval = int.Parse((string)ConfigurationManager.AppSettings["DownloadInterval"]);
                    }
                    catch { }
                }
                if (ConfigurationManager.AppSettings["DownloadStartTime"] != null)
                {
                    try
                    {
                        downloadStartTime = (string)ConfigurationManager.AppSettings["DownloadStartTime"];
                    }
                    catch { }
                }
                if (ConfigurationManager.AppSettings["NumberOfMealsTime"] != null)
                {
                    try
                    {
                        numberOfMealsTime = (string)ConfigurationManager.AppSettings["NumberOfMealsTime"];
                    }
                    catch { }
                }
                foreach (KeyValuePair<string, ValidationTerminalTO> pair in vtDict)
                {
                    //if (validationTerminal.Status == "ENABLED")
                    //{
                    ValidationTerminalTO validationTerminal = pair.Value;
                    //if (validationTerminal.Status == Constants.terminalEnabled )
                    //{
                    validationTerminal.BlockPassInterval = blockPassInterval;
                    validationTerminal.LogDebugMessages = logDebugMessages;
                    validationTerminal.Name = validationTerminal.IpAddress;
                    validationTerminal.PassTimeOut = PassTimeOut;
                    validationTerminal.PingPeriod = pingReaderTime;
                    validationTerminal.TicketTransactionTimeOut = ticketTransactionTimeOut;
                    validationTerminal.ScreenTimeOut = ScreenTimeOut;
                    validationTerminal.DownloadInterval = downloadInterval;
                    validationTerminal.DownloadStartTime = downloadStartTime;
                    ReaderRemoteControlManager control = new ReaderRemoteControlManager(validationTerminal);
                    ticketControlManagerList.Add(validationTerminal.ValidationTerminalID, control);
                    //}
                }
                //if (ticketControlManagerList.Count  == vtList.Count)
                //{
                success = true;
                //}
            }
            catch
            {
                if (ticketControlManagerList != null)
                {
                    foreach (ReaderRemoteControlManager manager in ticketControlManagerList.Values)
                    {
                        manager.Stop();
                    }
                }
            }
            return success;
        }

        private List<ValidationTerminalTO> getTerminalsFromString(string terminals)
        {
            List<ValidationTerminalTO> vTerminals = new List<ValidationTerminalTO>();
            try
            {
                string[] splitTerminals = terminals.Split('|');
                foreach (string str in splitTerminals)
                {
                    string[] vtTOStrings = str.Split(',');
                    if (vtTOStrings.Length == 2)
                    {
                        ValidationTerminalTO vtTO = new ValidationTerminalTO();
                        int vtID = -1;
                        bool succ = int.TryParse(vtTOStrings[0], out vtID);
                        if (succ && vtID == 1)
                        {
                            vtTO.IpAddress = vtTOStrings[1];
                            vtTO.ValidationTerminalID = vtID;
                            vTerminals.Add(vtTO);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ReaderRemoteControlService.getTerminalsFromString(), Exception: " + ex.Message);
            }
            return vTerminals;
        }

        public void StartTicketProcessing()
        {
            try
            {
                if (ticketControlManagerList != null && ticketControlManagerList.Values != null)
                {
                    foreach (ReaderRemoteControlManager manager in ticketControlManagerList.Values)
                    {
                        manager.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ReaderRemoteControlService.StartTicketProcessing(), Exception: " + ex.Message);
            }
        }



        public void StopTicketProcessing()
        {
            try
            {
                if (ticketControlManagerList != null && ticketControlManagerList.Values != null)
                {
                    foreach (ReaderRemoteControlManager manager in ticketControlManagerList.Values)
                    {
                        manager.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ReaderRemoteControlService.StopTicketProcessing(), Exception: " + ex.Message);
            }
        }

        public string GetManagerStatus()
        {
            string managersStatuses = "";
            try
            {
                if (ticketControlManagerList != null && ticketControlManagerList.Values != null)
                {
                    foreach (ReaderRemoteControlManager manager in ticketControlManagerList.Values)
                    {
                        managersStatuses += manager.GetValidationTerminalID() + " " + manager.GetManagerStatus() + ";";
                    }
                    if (managersStatuses.Length > 0)
                    {
                        managersStatuses = managersStatuses.Substring(0, managersStatuses.Length - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ReaderRemoteControlService.GetManagerStatus(), Exception: " + ex.Message);
            }
            return managersStatuses;
        }

        public string GetManagerThreadStatus()
        {
            string managersThreadStatuses = "";
            try
            {
                if (ticketControlManagerList != null && ticketControlManagerList.Values != null)
                {
                    foreach (ReaderRemoteControlManager manager in ticketControlManagerList.Values)
                    {
                        managersThreadStatuses += manager.GetValidationTerminalID() + " " + manager.GetManagerThreadStatus() + ";";
                    }
                    if (managersThreadStatuses.Length > 0)
                    {
                        managersThreadStatuses = managersThreadStatuses.Substring(0, managersThreadStatuses.Length - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ReaderRemoteControlService.GetManagerThreadStatus(), Exception: " + ex.Message);
            }
            return managersThreadStatuses;
        }

        public string GetCounters()
        {
            string managersCardsNumbers = "";
            try
            {
                if (ticketControlManagerList != null && ticketControlManagerList.Values != null)
                {
                    foreach (ReaderRemoteControlManager manager in ticketControlManagerList.Values)
                    {
                        managersCardsNumbers += manager.GetValidationTerminalID() + " " + manager.GetCounter() + ";";
                    }
                    if (managersCardsNumbers.Length > 0)
                    {
                        managersCardsNumbers = managersCardsNumbers.Substring(0, managersCardsNumbers.Length - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ReaderRemoteControlService.GetCounters(), Exception: " + ex.Message);
            }
            return managersCardsNumbers;
        }

        public string GetCardNumbers()
        {
            string managersCardsNumbers = "";
            try
            {
                if (ticketControlManagerList != null && ticketControlManagerList.Values != null)
                {
                    foreach (ReaderRemoteControlManager manager in ticketControlManagerList.Values)
                    {
                        managersCardsNumbers += manager.GetValidationTerminalID() + " " + manager.GetTicketID() + ";";
                    }
                    if (managersCardsNumbers.Length > 0)
                    {
                        managersCardsNumbers = managersCardsNumbers.Substring(0, managersCardsNumbers.Length - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ReaderRemoteControlService.GetCardNumbers(), Exception: " + ex.Message);
            }
            return managersCardsNumbers;
        }

        public string GetValidationTerminalIP()
        {
            string terminalsIP = "";
            return terminalsIP;
        }

        private void SetServiceStatus(string text)
        {
            lock (locker)
            {
                serviceStatus = text;
            }
        }
        public string GetServiceStatus()
        {
            lock (locker)
            {
                return serviceStatus;
            }
        }
    }
}
