using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using ACTAReaderMonitorManagement;
using Common;
using Util;
using DataAccess;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using TransferObjects;

namespace ACTAReaderMonitorService
{

    [ServiceContract(Namespace = "http://ACTAReaderMonitorService")]
    public interface IACTAReaderMonitorService
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
        string GetManagerThreadStatus();

    }


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                   IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(
     RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ACTAReaderMonitorService : IACTAReaderMonitorService
    {
        private static ACTAReaderMonitorService instance = null;
        private Dictionary<int, ACTAReaderMonitorManager> ticketControlManagerList;

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


        public ACTAReaderMonitorService()
        {
            // Init Debug
            NotificationController.SetApplicationName("ACTAReaderMonitorService");
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            ticketControlManagerList = new Dictionary<int, ACTAReaderMonitorManager>();

            // init ticket controller
            try
            {
                try
                {
                    daoFactory = DAOFactory.getDAOFactory();
                    DBconnected = true;

                    InitializeTicketControlManager();

                    log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ACTAReaderMonitorService.ACTAReaderMonitorService() connection to database established");
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ACTAReaderMonitorService.ACTAReaderMonitorService() " + ex.Message);
                    DBconnected = false;
                    SetServiceStatus(noDBConnectionString);
                }
                if (!DBconnected)
                {
                    // timer for checking database connection
                    timerDB = new System.Timers.Timer(Constants.dbRefreshTime);
                    timerDB.Elapsed += new System.Timers.ElapsedEventHandler(timerDB_Elapsed);
                    timerDB.Start();
                    log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ACTAReaderMonitorService.ACTAReaderMonitorService() timerDB started");
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
                    if (!DBconnected)
                    {
                        try
                        {
                            daoFactory = DAOFactory.getDAOFactory();
                            DBconnected = true;

                            try
                            {
                                InitializeTicketControlManager();
                            }
                            catch (Exception ex)
                            {
                                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ACTAReaderMonitorService.tryToConnect() error." + ex.Message);
                                DBconnected = false;
                                throw;
                            }
                            StartTicketProcessing();
                            timerDB.Enabled = false;
                            log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ACTAReaderMonitorService.tryToConnect() connection to database established.");
                        }
                        catch (Exception ex)
                        {
                            log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ACTAReaderMonitorService.tryToConnect() exception: " + ex.Message);
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ACTAReaderMonitorService.tryToConnect(): " + ex.Message + "\n");
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
                    else timerDB.Enabled = false;
                }
                catch
                {
                    DBconnected = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ACTAReaderMonitorService.timerDB_Elapsed(): " + ex.Message + "\n");
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

        public static ACTAReaderMonitorService Instance
        {
            get
            {
                lock (instanceLocker)
                {
                    if (instance == null)
                    {
                        instance = new ACTAReaderMonitorService();
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
                string reader_num = "";
                if (ConfigurationManager.AppSettings["gates"] != null)
                {
                    reader_num = ConfigurationManager.AppSettings["gates"];
                }
                string[] gates = reader_num.Split(',');
                List<GateTO> list = new List<GateTO>();
                foreach (string gate in gates)
                {
                    GateTO reader = new Gate().Find(int.Parse(gate));
                    list.Add(reader);
                }


                //list = new Gate().
                for (int i = 0; i < list.Count; i++)
                {
                    GateTO readerInList = list[i];
                    Reader reader = new Reader();
                    List<ReaderTO> readers = reader.getReaders(-1, readerInList.GateID);
                    foreach (ReaderTO readerTO in readers)
                    {
                        ACTAReaderMonitorManager control;

                        control = new ACTAReaderMonitorManager(readerTO);
                        ticketControlManagerList.Add(readerTO.ReaderID, control);
                    }
                }

                success = true;

            }
            catch (Exception ex)
            {
                //log.writeLog(ex.Message);
                if (ticketControlManagerList != null)
                {
                    foreach (ACTAReaderMonitorManager manager in ticketControlManagerList.Values)
                    {
                        manager.Stop();
                    }
                }
                throw ex;
            }
            return success;
        }
        public void StartTicketProcessing()
        {
            try
            {
                if (ticketControlManagerList != null && ticketControlManagerList.Values != null)
                {
                    foreach (ACTAReaderMonitorManager manager in ticketControlManagerList.Values)
                    {
                        manager.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ACTAReaderMonitorService.StartTicketProcessing(), Exception: " + ex.Message);
            }
        }



        public void StopTicketProcessing()
        {
            try
            {
                if (ticketControlManagerList != null && ticketControlManagerList.Values != null)
                {
                    foreach (ACTAReaderMonitorManager manager in ticketControlManagerList.Values)
                    {
                        manager.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ACTAReaderMonitorService.StopTicketProcessing(), Exception: " + ex.Message);
            }
        }

        public string GetManagerStatus()
        {
            string managersStatuses = "";
            try
            {
                if (ticketControlManagerList != null && ticketControlManagerList.Values != null)
                {
                    foreach (ACTAReaderMonitorManager manager in ticketControlManagerList.Values)
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ACTAReaderMonitorService.GetManagerStatus(), Exception: " + ex.Message);
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
                    foreach (ACTAReaderMonitorManager manager in ticketControlManagerList.Values)
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ACTAReaderMonitorService.GetManagerThreadStatus(), Exception: " + ex.Message);
            }
            return managersThreadStatuses;
        }


        //public string GetCardNumbers()
        //{
        //    string managersCardsNumbers = "";
        //    try
        //    {
        //        if (ticketControlManagerList != null && ticketControlManagerList.Values != null)
        //        {
        //            foreach (ACTAReaderMonitorManager manager in ticketControlManagerList.Values)
        //            {
        //                managersCardsNumbers += manager.GetValidationTerminalID() + " " + manager.GetTicketID() + ";";
        //            }
        //            if (managersCardsNumbers.Length > 0)
        //            {
        //                managersCardsNumbers = managersCardsNumbers.Substring(0, managersCardsNumbers.Length - 1);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ReaderRemoteControlService.GetCardNumbers(), Exception: " + ex.Message);
        //    }
        //    return managersCardsNumbers;
        //}

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