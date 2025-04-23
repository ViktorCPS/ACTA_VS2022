using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Util;
using Common;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using TransferObjects;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using ReaderInterface;
using System.Collections;
using ACTAReaderRemoteControlProcessing;
using System.Net.Mail;
using System.Net.Mime;
using ReaderRemoteDataAccess;
using System.Configuration;
using RFID;


namespace ReaderRemoteManagement
{

    public class ReaderRemoteControlManager
    {
        //thread for reading cards on validation terminal
        private Thread worker;

        Object dbConnection = null;

        public string workStatus0 = "";
        public string workStatus1 = "";

        //stop thread
        private bool stopMonitoring = false;

        //debuging object
        private DebugLog log = null;
        //debuging object
        private DebugLog logForAll = null;

        //for thread controlling
        private object locker = new object();

        private string managerStatus = "";

        private string managerThreadStatus;

        private uint previous_tag_id = 0;
        int previous_ant_num = 0;
        private long debouncingLastTicket = 0;

        private long debouncingTicketTimeOut = 10000;

        private double pingInterval = 2000;    // in ms
        bool VTIsPinging = true;

        Dictionary<int, Dictionary<int, string>> messageScreen = new Dictionary<int, Dictionary<int, string>>();

        public AntennaCommunication currentAntenna;
        public AntennaCommunication Antenna0;
        public AntennaCommunication Antenna1;
        public List<AntennaCommunication> antennaList = new List<AntennaCommunication>();

        //validation terminal controlling
        public ValidationTerminalTO validationTerminal = new ValidationTerminalTO();

        public Dictionary<int, OnlineMealsPointsTO> distionaryPoints = new Dictionary<int, OnlineMealsPointsTO>();

        //if true write all comunication messages with reader
        public bool logDebugMessages = true;

        //reader communication objects
        private NetworkStream netStream = null;
        private TcpClient tcpClient = null;
        private int portNum = 10002;

        //connected to validation terminal
        private bool readerConnected = false;

        //communication messages
        private CommunicationMessage communicationMessage;

        //number of tickets pass on reader 
        private int counter = 0;
        private DateTime lastPassTime = new DateTime();

        //how long to wait response from validation terminal
        private long ticketTransactionTimeOut = 10000;  // in ms

        //check set time 
        private long readerSetTimeTimeOut = 15000;  // in ms
        private long lastSettingTime = 0;

        //check reader connection period
        private long readerCheckTimeOut = 60000;  // in ms
        private long lastConnectionCheckingTime = 0;


        //check upload access profiles period
        private long uploadProfilesCheckTimeOut = 900000;  // in ms 
        private long lastUploadProfilesCheckingTime = 0;
        private long uploadProfilesOnceDayTimeOut = 86400000;  // in ms 
        private long lastUploadProfilesOnceDayTime = 0;

        //pass blocking param's
        private double blockPassInterval = 1000;    // in ms       

        private string statusOK = "1";
        private string statusNOTOK = "0";

        private int countStandAloneStatus = 0;

        int response = 0;
        byte buttons = 0;
        string screenMessage = "";

        private int antIN = 0;
        private int antOUT = 1;
        private int antALL = -1;
        uint tag_id = 0;


        //DOWNLOAD TIME FOR READING DATA FROM DB
        string downloadStartTime = "04:00";
        long downloadInterval = 1440;
        private long firstDownloadCheckingTime = 0;
        DateTime downloadTime = new DateTime();
        DateTime lastDownloadTime = new DateTime();

        ReaderRemoteControlProcessor tagProcessor;

        //READ NUMBER SOLD MEALS STATUS ONCE A DAY FROM DB
        string statusStartTime = "00:00";
        long statusInterval = 1440;
        private long firstStatusCheckingTime = 0;
        DateTime statusTime = new DateTime();
        DateTime lastStatusTime = new DateTime();
        Dictionary<int, int> greenValidationsDict = new Dictionary<int, int>();
        Dictionary<int, int> redValidationsDict = new Dictionary<int, int>();


        //erase log data
        int octet1, octet2, octet3, octet4;
        bool status1, status2, status3, status4;
        int IPaddress;

        DateTime lastStatusReader = new DateTime();

        //Object readerRemoteConnection = null;
        //ReaderRemoteDAO readerRemoteDAO = null;
        //bool connStringExists = false;

        public ReaderRemoteControlManager(ValidationTerminalTO vt)
        {
            try
            {
                managerThreadStatus = statusNOTOK;
                log = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log" + vt.ValidationTerminalID + "_" + vt.Name + ".txt");
                logForAll = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log" + ".txt");
                validationTerminal = vt;

                communicationMessage = new CommunicationMessage(this);
                if (validationTerminal.BlockPassInterval > 0)
                    blockPassInterval = validationTerminal.BlockPassInterval;
                if (validationTerminal.TicketTransactionTimeOut > 0)
                    ticketTransactionTimeOut = validationTerminal.TicketTransactionTimeOut;
                if (validationTerminal.PassTimeOut > 0)
                    pingInterval = validationTerminal.PingPeriod;

                if (validationTerminal.DownloadInterval > 0)
                    downloadInterval = validationTerminal.DownloadInterval;
                if (!validationTerminal.DownloadStartTime.Equals(""))
                    downloadStartTime = validationTerminal.DownloadStartTime;
                if (!validationTerminal.NumberOfMealsTime.Equals(""))
                    statusStartTime = validationTerminal.NumberOfMealsTime;

                Antenna0 = new AntennaCommunication();
                Antenna0.AntennaNum = antIN;
                Antenna1 = new AntennaCommunication();
                Antenna1.AntennaNum = antOUT;
                antennaList.Add(Antenna0);
                antennaList.Add(Antenna1);

                tagProcessor = new ReaderRemoteControlProcessor(vt.ValidationTerminalID, vt.Locations);

                //message for screen
                Dictionary<int, string> mess = new Dictionary<int, string>();
                Dictionary<int, string> mess1 = new Dictionary<int, string>();

                for (int index = 1; index <= 8; index++)
                {
                    mess.Add(index, "0000000000000;... ;00:00:00");
                    mess1.Add(index, "0000000000000;... ;00:00:00");
                }

                messageScreen.Add(0, mess);
                messageScreen.Add(1, mess1);

                //TIME FOR DOWNLOAD DATA FROM DB
                downloadTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(this.downloadStartTime.Remove(this.downloadStartTime.IndexOf(':'))), int.Parse(this.downloadStartTime.Substring(this.downloadStartTime.IndexOf(':') + 1)), 0);

                //NUMBER OF SOLD MEALS
                statusTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(this.statusStartTime.Remove(this.statusStartTime.IndexOf(':'))), int.Parse(this.statusStartTime.Substring(this.statusStartTime.IndexOf(':') + 1)), 0);
                greenValidationsDict.Add(0, 0);
                greenValidationsDict.Add(1, 0);
                redValidationsDict.Add(0, 0);
                redValidationsDict.Add(1, 0);

                checkDBConnection();
                //dbConnection = DBConnectionManager.Instance.MakeNewDBConnection();

                OnlineMealsPoints onlinePoints = new OnlineMealsPoints(dbConnection);
                distionaryPoints = new Dictionary<int, OnlineMealsPointsTO>();
                List<OnlineMealsPointsTO> listPoints = onlinePoints.Search(validationTerminal.IpAddress, 0);
                if (listPoints.Count == 1)
                    distionaryPoints.Add(0, listPoints[0]);
                listPoints = onlinePoints.Search(validationTerminal.IpAddress, 1);

                if (listPoints.Count == 1)
                    distionaryPoints.Add(1, listPoints[0]);

                //GET IPaddress > int 
                string[] octets = validationTerminal.IpAddress.Split('.');
                status1 = Int32.TryParse(octets[0], out octet1);
                status2 = Int32.TryParse(octets[1], out octet2);
                status3 = Int32.TryParse(octets[2], out octet3);
                status4 = Int32.TryParse(octets[3], out octet4);

                IPaddress = (octet1 << 24) + (octet2 << 16) + (octet3 << 8) + octet4;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public DateTime GetNextReading(DateTime readTime, long readInterval)
        {
            DateTime readingTime = new DateTime();
            DateTime currentTime = DateTime.Now;
            currentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

            DateTime downloadStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, readTime.Hour, readTime.Minute, 0);

            int i = 0;

            try
            {
                readingTime = downloadStartTime;

                if (readingTime.CompareTo(currentTime) >= 0)
                {
                    while (currentTime.CompareTo(readingTime) < 0)
                    {
                        readingTime = readingTime.AddMinutes(-readInterval);
                        i++;
                    }
                    readingTime = readingTime.AddMinutes(readInterval);
                }
                else
                {
                    while (readingTime < currentTime)
                    {
                        readingTime = readingTime.AddMinutes(readInterval);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogDebugMessage(DateTime.Now + " Exception in: " + this.ToString() + ex.Message, 0, -1);
            }

            return readingTime;
        }

        private bool UpdateReaderTags(ArrayList cardList)
        {
            bool succeeded = false;
            ReaderFactory.TechnologyType = "MIFARE";
            IReaderInterface rfid = ReaderFactory.GetReader;

            try
            {
                WriteLog(" +++ CardsUpload : STARTED! \n +++ ", Constants.extendedDebugLevel, -1);

                ReaderFactory.TechnologyType = "MIFARE";
                rfid = ReaderFactory.GetReader;

                WriteLog(" + Terminal ID: " + validationTerminal.IpAddress + " Num. cards: " + cardList.Count + "\n", Constants.extendedDebugLevel, -1);

                rfid.SetCards(GetReaderAddress(), cardList);
                if (rfid.GetError() == null || rfid.GetError().Equals(""))
                {
                    succeeded = true;
                    WriteLog(" +++ CardsUpload : FINISHED! \n +++ ", Constants.extendedDebugLevel, -1);
                }
                else
                {
                    succeeded = false;
                    WriteLog(DateTime.Now + " +++ CardsUpload : FAILED! " + rfid.GetError() +
                            "\n +++ ", Constants.extendedDebugLevel, -1);
                }
            }
            catch (Exception ex)
            {
                WriteLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".CardsUpload() : " + ex.StackTrace + " RFID Error: " + rfid.GetError() + "\n", Constants.basicDebugLevel, -1);
            }

            return succeeded;
        }


        public void ConnectToReader()
        {
            try
            {
                lock (locker)
                {
                    readerConnected = false;
                    tcpClient = new TcpClient();
                    tcpClient.Connect(validationTerminal.IpAddress, portNum);
                    netStream = tcpClient.GetStream();
                    readerConnected = true;
                  
                    //sending mails only for real connection lost
                    if (readerConnLost)
                    {
                        sendMailsConnEstablished();
                        readerConnLost = false;
                    }

                    communicationMessage = new CommunicationMessage(this);
                    if (validationTerminal.PassTimeOut > 0)
                        communicationMessage.PassTimeOut = validationTerminal.PassTimeOut;

                    if (validationTerminal.ScreenTimeOut > 0)
                        communicationMessage.ScreenTimeOut = validationTerminal.ScreenTimeOut;
                    Thread.Sleep(2000); // wait for data in the netstream to become available, if any
                    if (netStream.DataAvailable)
                    {
                        communicationMessage.FlushTerminal(netStream);
                    }

                    WriteLogDebugMessage(" Reader connected.", Constants.basicDebugLevel, -1);
                    setStatus(antALL, " Reader connected.");
                    lastStatusReader = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                if (tcpClient != null && tcpClient.Connected)
                {
                    tcpClient.Close();
                    tcpClient = new TcpClient();
                }
                WriteLog("Reset start", Constants.basicDebugLevel, -1);
                //after restart(lost connection) new tcp conn can not be established, reader didn't finish his tcp conn. Must restart
                RFIDDevice rfid = new RFIDDevice();
                rfid.Open(validationTerminal.IpAddress);
                rfid.Reset();
                rfid.Close();
                WriteLog("Reset end", Constants.basicDebugLevel, -1);
                log.writeLog(ex);
                logForAll.writeLog(ex);
                readerConnected = false;
            }
        }

        public void setStatus(int ant, string status)
        {
            if (ant == 0)
            {
                workStatus0 = status;
            }
            else if (ant == 1)
            {
                workStatus1 = status;
            }
            else
            {
                workStatus0 = status;
                workStatus1 = status;
            }
        }

        public void Start()
        {
            //lock (locker)
            //{
            //    pingSender = new System.Net.NetworkInformation.Ping();
            //}

            // tagProcessor.getValidationDataNew();
            WriteLog(" TickeControler started!", Constants.basicDebugLevel, -1);
            setStatus(antALL, " TickeControler started!");
            worker = new Thread(new ThreadStart(Work));
            worker.Name = "TicketControlManager";
            worker.Start();
        }

        public void Stop()
        {
            try
            {
                WriteLog(" TickeControler is ending!", Constants.basicDebugLevel, -1);

                setStatus(antALL, " TickeControler is ending!");
                stopMonitoring = true;
                //pingSender.Dispose();
                if (worker != null)
                {
                    worker.Join(); // Wait for the worker's thread to finish.
                }

                WriteLog(" TickeControler stopped! ", Constants.basicDebugLevel, -1);
                setStatus(antALL, " TickeControler stopped! ");
            }
            catch (Exception ex)
            {
                log.writeLog(ex);
                logForAll.writeLog(ex);
            }
        }

        private bool uploadCheckTime()
        {
            if ((DateTime.Now.Ticks - lastUploadProfilesCheckingTime) / TimeSpan.TicksPerMillisecond > uploadProfilesCheckTimeOut)
            {
                lastUploadProfilesCheckingTime = DateTime.Now.Ticks;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkDownloadTime()
        {
            DateTime next = GetNextReading(this.downloadTime, this.downloadInterval);

            if (firstDownloadCheckingTime == 0 || ((next.Hour == DateTime.Now.Hour && next.Minute == DateTime.Now.Minute) && ((lastDownloadTime == new DateTime() || (!lastDownloadTime.ToString("dd.MM.yyyy HH:mm").Trim().Equals(DateTime.Now.ToString("dd.MM.yyyy HH:mm")))))))
            {
                if (firstDownloadCheckingTime == 0)
                {
                    firstDownloadCheckingTime = DateTime.Now.Ticks;
                }
                else
                {
                    lastDownloadTime = DateTime.Now;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private void UploadDownload()
        {

            try
            {
                //ONCE A DAY, FOR NOW AT 4.00
                if (checkDownloadTime())
                {
                    WriteLog("Data reading start", Constants.extendedDebugLevel, -1);

                    bool succ = tagProcessor.getValidationData();
                    if (succ)
                    {
                        WriteLog("Data reading end sucessfully", Constants.extendedDebugLevel, -1);
                    }
                    else
                    {
                        WriteLog("Data reading end FAILED", Constants.extendedDebugLevel, -1);
                    }

                    UpdateReaderTags(tagProcessor.CardsList);

                    ReaderFactory.TechnologyType = Constants.technologyType;
                    IReaderInterface readerInterface = ReaderFactory.GetReader;

                    //checks if time is greater than 10sec and sets current time
                    readerInterface.CheckTime(validationTerminal.IpAddress);
                    WriteLog("Time set.", Constants.extendedDebugLevel, -1);

                    int percent = readerInterface.GetLogOccupiedPercentage(validationTerminal.IpAddress);

                    try
                    {
                        WriteLog("Download meals started. ", Constants.extendedDebugLevel, -1);
                        DownloadLog log = new DownloadLog(validationTerminal, validationTerminal.Locations);
                        log.GetLog(true, "");
                        WriteLog("Download meals ended. ", Constants.extendedDebugLevel, -1);

                        // Sanja 23.04.2014. after changing status to remote get new meals used number for displaying on TFT
                        firstStatusCheckingTime = 0;
                    }
                    catch (Exception ex)
                    {
                        WriteLog("Download meals error: " + ex.Message, Constants.extendedDebugLevel, -1);
                    }

                    WriteLog("Log percentage: " + percent, Constants.extendedDebugLevel, -1);
                    if (percent >= 80)
                    {
                        try
                        {
                            WriteLog("Erase log start ", Constants.extendedDebugLevel, -1);
                            readerInterface.EraseLog(IPaddress);
                            WriteLog("Erase log end ", Constants.extendedDebugLevel, -1);
                        }
                        catch (Exception ex)
                        {

                            WriteLog("Erase log error: " + ex.Message, Constants.extendedDebugLevel, -1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                WriteLog("Exception in Check() " + ex.Message, Constants.basicDebugLevel, -1);
            }
        }

        private void checkTimeForCountingStatuses()
        {

            DateTime next = GetNextReading(this.statusTime, this.statusInterval);

            if (firstStatusCheckingTime == 0 || ((next.Hour == DateTime.Now.Hour && next.Minute == DateTime.Now.Minute) && ((lastStatusTime == new DateTime() || (!lastStatusTime.ToString("dd.MM.yyyy HH:mm").Trim().Equals(DateTime.Now.ToString("dd.MM.yyyy HH:mm")))))))
            {

                if (firstStatusCheckingTime == 0)
                {
                    WriteLog("NUMBER OF MEALS READ FROM DB ", Constants.basicDebugLevel, -1);
                    firstStatusCheckingTime = DateTime.Now.Ticks;
                    OnlineMealsUsedDaily dailyMeal = new OnlineMealsUsedDaily(dbConnection);
                    if (distionaryPoints.ContainsKey(0))
                    {
                        greenValidationsDict[0] = dailyMeal.CountByStatus((int)Constants.MealOnlineValidation.Valid, DateTime.Now.Date, distionaryPoints[0].PointID);
                        redValidationsDict[0] = dailyMeal.CountByStatus((int)Constants.MealOnlineValidation.NotValid, DateTime.Now.Date, distionaryPoints[0].PointID);
                    }
                    if (distionaryPoints.ContainsKey(1))
                    {
                        greenValidationsDict[1] = dailyMeal.CountByStatus((int)Constants.MealOnlineValidation.Valid, DateTime.Now.Date, distionaryPoints[1].PointID);
                        redValidationsDict[1] = dailyMeal.CountByStatus((int)Constants.MealOnlineValidation.NotValid, DateTime.Now.Date, distionaryPoints[1].PointID);
                    }
                }
                else
                {
                    WriteLog("NUMBER OF MEALS RESTART ", Constants.basicDebugLevel, -1);
                    lastStatusTime = DateTime.Now;
                    greenValidationsDict[0] = 0;
                    greenValidationsDict[1] = 0;
                    redValidationsDict[0] = 0;
                    redValidationsDict[1] = 0;
                }
            }
        }

        private bool uploadOnceDayTime()
        {
            if ((DateTime.Now.Ticks - lastUploadProfilesOnceDayTime) / TimeSpan.TicksPerMillisecond > uploadProfilesOnceDayTimeOut)
            {

                lastUploadProfilesOnceDayTime = DateTime.Now.Date.AddMinutes(30).Ticks;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Work()
        {
            try
            {
                foreach (AntennaCommunication ant in antennaList)
                {
                    ant.state = State.WAITING_FOR_TERMINAL_REQUEST;
                }
                while (true)
                {
                    try
                    {
                        SetManagerThreadStatus(statusOK);
                        if (!stopMonitoring)
                        {
                            if (ReaderConnectedSucc())
                            {
                                if (readerConnected)
                                {
                                    // Sanja 23.04.2014. - first upload meals then get number of meals for TFT
                                    //download data from db for validation, erase log, updateReaderTags
                                    UploadDownload();

                                    //Number of meals on TFT
                                    checkTimeForCountingStatuses();

                                    //download data from db for validation, erase log, updateReaderTags
                                    //UploadDownload();

                                    if (lastPassTime.Date != DateTime.Now.Date)
                                    {
                                        //counter = 0;
                                        lastPassTime = DateTime.Now.Date;
                                    }
                                    byte[] myReadBuffer = new byte[2 * communicationMessage.MESSAGE_LENGTH];

                                    //CHECK HERE I AM MESSAGE
                                    if ((DateTime.Now.Ticks - lastStatusReader.Ticks) / TimeSpan.TicksPerMillisecond > 6 * 60 * 1000)
                                    {
                                        WriteLog("+++Reader not connected, no message", Constants.basicDebugLevel, -1);
                                        readerConnected = false;
                                        readerConnLost = true;
                                        sendMails();
                                    }

                                    if (communicationMessage.ReadTerminal(netStream, myReadBuffer))
                                    {
                                        countStandAloneStatus = 0;
                                        int antNum = communicationMessage.getAntenna(myReadBuffer);
                                        foreach (AntennaCommunication ant in antennaList)
                                        {
                                            if (ant.AntennaNum == antNum)
                                            {
                                                currentAntenna = ant;
                                            }
                                        }
                                        //here i am message
                                        lastStatusReader = DateTime.Now;
                                        communicationMessage.StartCommunicationMessage(myReadBuffer);

                                        if (communicationMessage.IsMessageCorrect())
                                        {
                                            if (communicationMessage.MessageType == communicationMessage.TERMINAL_REQUEST)
                                            {
                                                SetTicketID(communicationMessage.GetDocumentNumber());
                                                setStatus(currentAntenna.AntennaNum, "TERINAL REQUEST; tag_id = " + tag_id);
                                                ProcessRequest();
                                            }
                                            else if (communicationMessage.MessageType == communicationMessage.TERMINAL_RESPONSE_BUTTON)
                                            {
                                                setStatus(currentAntenna.AntennaNum, "TERMINAL_RESPONSE_BUTTON; tag_id = " + tag_id);
                                                ProcessRequestButton(communicationMessage.GetButtonNumber());
                                            }

                                            else if (communicationMessage.MessageType == communicationMessage.TERMINAL_RESPONSE)
                                            {
                                                setStatus(currentAntenna.AntennaNum, "TERMINAL_RESPONSE; tag_id = " + tag_id);
                                                ProcessResponse();
                                            }
                                        }
                                    }

                                    foreach (AntennaCommunication ant in antennaList)
                                    {
                                        // check for ticket transaction time out
                                        if (ant.ticketTransactionState == ReaderRemoteManagement.AntennaCommunication.TicketTransactionState.STARTED)
                                        {
                                            if (IsTicketTransactionTimeOut(ant))
                                            {
                                                ant.FinishTicketTransaction();
                                            }
                                        }
                                    }

                                    screenMessage = "";

                                }
                                else
                                {
                                    ConnectToReader();
                                }
                            }

                            Thread.Sleep(100);
                        }
                        else
                        {
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        checkDBConnection();

                        WriteLog(" WORK() exception: " + ex.Message, Constants.basicDebugLevel, currentAntenna.AntennaNum);
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog(" WORK() exception: " + ex.Message, Constants.basicDebugLevel, currentAntenna.AntennaNum);
                SetManagerThreadStatus(statusNOTOK);
            }

        }

        //private void closeConnections()
        //{
        //    try
        //    {
        //        DBConnectionManager.Instance.CloseDBConnection(ACTAConnection);

        //        WriteLog(" ReaderRemoteManager.closeConnections() Connection closed successfully!", Constants.basicDebugLevel);

        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog(" ReaderRemoteManager.closeConnections() exception: " + ex.Message, Constants.basicDebugLevel);
        //    }
        //}

        //private bool connectToDataBases()
        //{
        //    bool succ = false;
        //    try
        //    {
        //        ACTAConnection = DBConnectionManager.Instance.MakeNewDBConnection();
        //        if (ACTAConnection == null)
        //        {
        //            WriteLog(" SyncManager.connectToDataBases() Make new ACTA DB connection faild!", Constants.basicDebugLevel);
        //            return false;
        //        }

        //        succ = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog(" SyncManager.connectToDataBases() exception: " + ex.Message, Constants.basicDebugLevel);
        //    }
        //    return succ;
        //}

        private bool uploadTime()
        {
            if ((DateTime.Now.Ticks - lastUploadProfilesCheckingTime) / TimeSpan.TicksPerMillisecond > uploadProfilesCheckTimeOut)
            {

                lastUploadProfilesCheckingTime = DateTime.Now.Date.Ticks;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ProcessRequestButton(int button)
        {
            bool valid = CheckRequestButton(button);
            if (button == 0)
            {
                if (logDebugMessages)
                {
                    WriteLogDebugMessage("---Transaction end button wasn't pressed---", Constants.detailedDebugLevel, -1);
                }
                currentAntenna.FinishTicketTransaction();
                currentAntenna.state = State.WAITING_FOR_TERMINAL_REQUEST;
                return;
            }
            if (!valid)
            {
                // Ticket number is not correct, send denied message
                byte[] passIsDeniedMessage = communicationMessage.GetPassIsDeniedMessageBytes(screenMessage);
                netStream.Flush();
                netStream.Write(passIsDeniedMessage, 0, communicationMessage.MESSAGE_LENGTH_RESPONSE);

                Console.WriteLine("APPLICATION RESPONSE: Pass is denied!");
                Console.WriteLine("\n");
                for (int i = 0; i < passIsDeniedMessage.Length; i++)
                {
                    Console.Write(passIsDeniedMessage[i].ToString("X2") + " ");
                }
                Console.WriteLine("\n");

                if (logDebugMessages)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" APPLICATION RESPONSE: Pass is denied! ");
                    //sb.Append("\n");
                    for (int i = 0; i < passIsDeniedMessage.Length; i++)
                    {
                        sb.Append(passIsDeniedMessage[i].ToString("X2") + " ");
                    }
                    //sb.Append("\n\n");

                    WriteLogDebugMessage(sb.ToString(), Constants.extendedDebugLevel, -1);
                    WriteLogDebugMessage("---Transaction end pass denied---", Constants.detailedDebugLevel, -1);
                }

                currentAntenna.FinishTicketTransaction();
                currentAntenna.state = State.WAITING_FOR_TERMINAL_REQUEST;
            }
            else
            {
                // Ticket number is correct, send allow message
                byte[] passIsAllowedMessage = communicationMessage.GetPassIsAllowedMessageBytes(screenMessage);
                netStream.Flush();
                netStream.Write(passIsAllowedMessage, 0, communicationMessage.MESSAGE_LENGTH_RESPONSE);

                Console.WriteLine("APPLICATION RESPONSE: Pass is allowed!");
                Console.WriteLine("\n");
                for (int i = 0; i < passIsAllowedMessage.Length; i++)
                {
                    Console.Write(passIsAllowedMessage[i].ToString("X2") + " ");
                }
                Console.WriteLine("\n");

                if (this.logDebugMessages)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" APPLICATION RESPONSE: Pass is allowed!");
                    // sb.Append("\n");
                    for (int i = 0; i < passIsAllowedMessage.Length; i++)
                    {
                        sb.Append(passIsAllowedMessage[i].ToString("X2") + " ");
                    }
                    // sb.Append("\n\n");

                    this.WriteLogDebugMessage(sb.ToString(), Constants.extendedDebugLevel, -1);
                }

                currentAntenna.state = State.WAITING_FOR_TERMINAL_RESPONSE;
            }
        }

        private void ProcessResponse()
        {
            Console.WriteLine(communicationMessage.ToString());

            if (logDebugMessages)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(communicationMessage.ToString());
                WriteLogDebugMessage(sb.ToString(), Constants.detailedDebugLevel, currentAntenna.AntennaNum);
            }


            //07.06.2010. Natasa - if no barrier visitor isVisitorPassed is allways true!
            bool isVisitorPassed = communicationMessage.IsVisitorPassed();


            int passDetected = 0;
            if (isVisitorPassed)
            {
                passDetected = 1;
                counter++;
                lastPassTime = DateTime.Now;

                currentAntenna.lastTicketTime = DateTime.Now.Ticks;

                // log RFID ticket pass data
                if (logDebugMessages)
                {

                    WriteLog("---Trasaction end visitor passed--- ", Constants.detailedDebugLevel, currentAntenna.AntennaNum);
                }
            }
            else
            {

                WriteLog("---Trasaction end visitor NOT passed--- ", Constants.detailedDebugLevel, currentAntenna.AntennaNum);
            }
            try
            {
                //WriteLogDebugMessage(" ++CheckMethod() (antenna-" + currentAntenna.AntennaNum + ") called with params: message_type = " + Constants.MESSAGE_PASS_STATUS + "; validation_terminal_id = " + validationTerminal.ValidationTerminalID + "; antenna = " + currentAntenna.AntennaNum
                //+ "; tag_id = " + currentAntenna.tagID.ToString() + "; pass_detected = " + passDetected.ToString() + "; button_number = 0" + "++", Constants.detailedDebugLevel);
                //MealUsed mealUsed = new MealUsed(ACTAConnection);
                //mealUsed.Save(currentAntenna.transID.ToString(), currentAntenna.employeeID, DateTime.Now, validationTerminal.ValidationTerminalID, currentAntenna.mealTypeID, -1, -1);
                //int returnValue = tag.check_pass(Constants.MESSAGE_PASS_STATUS, validationTerminal.ValidationTerminalID, passDetected, currentAntenna.AntennaNum, currentAntenna.tagID, 0, ref response, ref buttons, ref screenMessage);
                //WriteLogDebugMessage(" --CheckMethod() (antenna-" + currentAntenna.AntennaNum + "); return value = 1" + "; return params: response = " + response.ToString() + "; buttons = " + buttons.ToString() + "; screenMessage = " + screenMessage + "--", Constants.detailedDebugLevel);

            }
            catch (Exception ex)
            {
                WriteLog("Tag.CheckMethod() throw exception: " + ex.Message, Constants.basicDebugLevel, currentAntenna.AntennaNum);
            }

            currentAntenna.FinishTicketTransaction();
        }

        private bool IsTicketTransactionTimeOut(AntennaCommunication ant)
        {
            if ((DateTime.Now.Ticks - ant.ticketTransactionStartTime) / TimeSpan.TicksPerMillisecond > ticketTransactionTimeOut)
            {
                Console.WriteLine("\nTicket transaction time out!\n");
                if (this.logDebugMessages)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" Ticket transaction time out!\n");
                    WriteLogDebugMessage(sb.ToString(), Constants.detailedDebugLevel, currentAntenna.AntennaNum);
                    currentAntenna.FinishTicketTransaction();
                    currentAntenna.state = State.WAITING_FOR_TERMINAL_REQUEST;
                }

                return true;
            }

            return false;
        }

        public int GetReaderAddress()
        {
            int address = -1;

            try
            {
                // Convert string to int
                try
                {
                    IPAddress ip = IPAddress.Parse(validationTerminal.IpAddress.Trim());
                    address = ip.GetAddressBytes()[3] + (ip.GetAddressBytes()[2] << 8) +
                        (ip.GetAddressBytes()[1] << 16) + (ip.GetAddressBytes()[0] << 24);
                }
                catch (Exception exIP)
                {
                    throw exIP;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Exception in: " + this.ToString() +
                    ".GetReaderAddres() : " + ex.Message);
            }

            return address;
        }

        private unsafe void SetTime()
        {
            if ((DateTime.Now.Ticks - lastSettingTime) / TimeSpan.TicksPerMillisecond > readerSetTimeTimeOut)
            {
                try
                {
                    lastSettingTime = DateTime.Now.Ticks;
                    RFIDDevice rfid = new RFIDDevice();
                    int status = rfid.Open(validationTerminal.IpAddress); // if (status == 0) greska;
                    if (status == 0)
                    {
                        WriteLogDebugMessage("SetTime - RFID.open() failed", Constants.basicDebugLevel, -1);
                    }
                    else
                    {
                        byte year, month, day, dayOfWeek, hour, minute, second, dayLight;
                        year = (byte)(DateTime.Now.Year - 2000); month = (byte)DateTime.Now.Month; day = (byte)DateTime.Now.Day; dayOfWeek = (byte)DateTime.Now.DayOfWeek;
                        hour = (byte)DateTime.Now.Hour; minute = (byte)DateTime.Now.Minute; second = (byte)DateTime.Now.Second; dayLight = 0;
                        status = rfid.SetRTC(year, month, day, dayOfWeek, hour, minute, second, dayLight);
                        rfid.Close();
                        if (status == 0) WriteLogDebugMessage("RFID interface error: SetTime", Constants.basicDebugLevel, -1);
                        else
                        {
                            WriteLogDebugMessage("Time set to current", 3, -1);
                        }
                    }
                }
                catch (Exception e) { WriteLog("Set Timer error: " + e.Message, Constants.basicDebugLevel, currentAntenna.AntennaNum); }
            }
        }

        private void checkDBConnection()
        {

            if (dbConnection != null)
            {
                //check connection state
                if (!DBConnectionManager.Instance.TestDBConnection(dbConnection))
                {
                    try
                    {
                        DBConnectionManager.Instance.CloseDBConnection(dbConnection);
                    }
                    catch (Exception ex)
                    {
                        WriteLog("Exception closing DB connection: " + ex.Message, Constants.basicDebugLevel, -1);
                    }

                    try
                    {
                        dbConnection = DBConnectionManager.Instance.MakeNewDBConnection();
                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {

                        WriteLog("Exception DB Closed: " + ex.Message, Constants.basicDebugLevel, -1);
                    }
                }
                //dummy select
                else if (!DBConnectionManager.Instance.TestDBConnectionDummySelect(dbConnection))
                {
                    try
                    {
                        DBConnectionManager.Instance.CloseDBConnection(dbConnection);
                    }
                    catch (Exception)
                    {
                        WriteLog("Error closing DB connection", Constants.basicDebugLevel, -1);
                    }

                    try
                    {
                        dbConnection = DBConnectionManager.Instance.MakeNewDBConnection();
                        Thread.Sleep(2000);
                    }
                    catch (Exception)
                    {
                        WriteLog("DB Closed", Constants.basicDebugLevel, -1);
                    }
                }
            }
            else
            {
                try
                {
                    dbConnection = DBConnectionManager.Instance.MakeNewDBConnection();
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    WriteLog("Exception cannot opet DB connection: " + ex.Message, Constants.basicDebugLevel, -1);
                }
            }
        }

        protected void sendMailsReset()
        {
            try
            {
                string host = Constants.Host;
                string emailAddress = Constants.EMailAddress;
                string userName = Constants.UserName;
                string password = Constants.Password;
                int port = 25;

                string ipAddress = validationTerminal.IpAddress;

                //emailAddress = "tmsystemnotification@fiatservices.com";
                //host = "smtprelay.fgremc.it";
                userName = @"gescoeurope\A004885";
                //password = "Serbia12";

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(host, port);

                string mailFrom = emailAddress;
                smtp.Credentials = new NetworkCredential(userName, password);

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                string message = "";
                message += "<html><body>" + "Poštovani, <br /><br />";
                message += "Došlo je do prekida komunikacije, nakon resetovanja <br /><br /> čitača sa IP adresom - <b> " + ipAddress + "</b>, vreme prekida - <b>" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "</b>. <br /><br /></body></html>";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = message;

                mailMessage.To.Add(Constants.MailsToSent);
                mailMessage.Subject = "Prekid komunikacije nakon resetovanja";
                mailMessage.From = new System.Net.Mail.MailAddress(mailFrom);

                try
                {
                    smtp.Send(mailMessage);
                    WriteLogDebugMessage("Mails sendMailsReset", Constants.basicDebugLevel, -1);
                }
                catch (Exception tex)
                {
                    WriteLogDebugMessage("Exception in sendMails: " + tex.Message, Constants.basicDebugLevel, -1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        protected void sendMailsConnEstablished()
        {
            try
            {
                string host = Constants.Host;
                string emailAddress = Constants.EMailAddress;
                string userName = Constants.UserName;
                string password = Constants.Password;
                int port = 25;

                string ipAddress = validationTerminal.IpAddress;

                //emailAddress = "tmsystemnotification@fiatservices.com";
                //host = "smtprelay.fgremc.it";
                userName = @"gescoeurope\A004885";
                //password = "Serbia12";

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(host, port);

                string mailFrom = emailAddress;
                smtp.Credentials = new NetworkCredential(userName, password);

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                string message = "";
                message += "<html><body>" + "Poštovani, <br /><br />";
                message += "Komunikacija je ponovo uspostavljena: <br /><br /> Čitač sa IP adresom - <b> " + ipAddress + "</b>, vreme uspostavljanja - <b>" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "</b>. <br /><br /></body></html>";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = message;

                mailMessage.To.Add(Constants.MailsToSent);
                mailMessage.Subject = "Komunikacija uspostavljena";
                mailMessage.From = new System.Net.Mail.MailAddress(mailFrom);

                try
                {
                    smtp.Send(mailMessage);
                    WriteLogDebugMessage("Mails sent", Constants.basicDebugLevel, -1);
                }
                catch (Exception tex)
                {
                    WriteLogDebugMessage("Exception in  sending sendMailsConnEstablished: " + tex.Message, Constants.basicDebugLevel, -1);
                }
            }
            catch (Exception ex)
            {
                WriteLogDebugMessage("Exception in sendMailsConnEstablished: " + ex.Message, Constants.basicDebugLevel, -1);
            }
        }
        
        protected void sendMails()
        {
            try
            {
                string host = Constants.Host;
                string emailAddress = Constants.EMailAddress;
                string userName = Constants.UserName;
                string password = Constants.Password;
                int port = 25;

                string ipAddress = validationTerminal.IpAddress;

                //emailAddress = "tmsystemnotification@fiatservices.com";
                //host = "smtprelay.fgremc.it";
                userName = @"gescoeurope\A004885";
                //password = "Serbia12";

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(host, port);

                string mailFrom = emailAddress;
                smtp.Credentials = new NetworkCredential(userName, password);

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                string message = "";
                message += "<html><body>" + "Poštovani, <br /><br />";
                message += "Došlo je do prekida komunikacije: <br /><br /> Čitač sa IP adresom - <b> " + ipAddress + "</b>, vreme prekida - <b>" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "</b>. <br /><br /></body></html>";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = message;

                mailMessage.To.Add(Constants.MailsToSent);
                mailMessage.Subject = "Prekid komunikacije";
                mailMessage.From = new System.Net.Mail.MailAddress(mailFrom);

                try
                {
                    smtp.Send(mailMessage);
                    WriteLogDebugMessage("Mails sent", Constants.basicDebugLevel, -1);
                }
                catch (Exception tex)
                {
                    WriteLogDebugMessage("Exception in sendMails: " + tex.Message, Constants.basicDebugLevel, -1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        bool readerConnLost = false;
        private unsafe bool ReaderConnectedSucc()
        {

            if ((DateTime.Now.Ticks - lastConnectionCheckingTime) / TimeSpan.TicksPerMillisecond > readerCheckTimeOut)
            {
                //TEST DB CONNECTION!
                checkDBConnection();

                int numOfTyries = 0;
                bool getRegime = false;
                while (numOfTyries < 3 && !getRegime)
                {
                    lastConnectionCheckingTime = DateTime.Now.Ticks;
                    RFIDDevice rfid = new RFIDDevice();
                    try
                    {
                        int status = rfid.Open(validationTerminal.IpAddress); // if (status == 0) greska;
                        if (status == 0)
                        {
                            //SetManagerStatus(statusNOTOK);
                            if (VTIsPinging)
                            {
                                WriteLogDebugMessage("Reader connection lost!", Constants.basicDebugLevel, -1);
                                setStatus(antALL, "Reader connection lost!");
                                VTIsPinging = false;
                                readerConnected = false;
                                if (tcpClient != null && tcpClient.Connected)
                                {
                                    tcpClient.Close();
                                    tcpClient = new TcpClient();
                                }

                                //sending mails only for real connection lost
                                readerConnLost = true;
                                sendMails();
                            }
                        }
                        else
                        {
                            // ocitaj tekuci rezim
                            byte regime;

                            status = rfid.GetRegime(&regime);   // if (status == 0) greska; 
                            rfid.Close();
                            if (status == 0)
                            {
                                //SetManagerStatus(statusNOTOK);
                                if (VTIsPinging)
                                {
                                    WriteLogDebugMessage("Reader status reading failed!", Constants.basicDebugLevel, -1);
                                    setStatus(antALL, "Reader status reading failed!");
                                    VTIsPinging = false;
                                    readerConnected = false;
                                    if (tcpClient != null && tcpClient.Connected)
                                    {
                                        tcpClient.Close();
                                        tcpClient = new TcpClient();
                                    }
                                    //sendMails();
                                }
                            }
                            else
                            {
                                // ako je status STANDALONE (0) postavi na REMOTE (1);
                                if (regime == 0)
                                {
                                    ReaderFactory.TechnologyType = Constants.technologyType;
                                    IReaderInterface readerInterface = ReaderFactory.GetReader;
                                    status = rfid.Open(validationTerminal.IpAddress);
                                    status = rfid.SetRegime(1); // if (status == 0) greska;
                                    rfid.Close();

                                    if (status == 0)
                                    {
                                        //SetManagerStatus(statusNOTOK);
                                        WriteLogDebugMessage("Reader status setting failed!", Constants.basicDebugLevel, -1);
                                        VTIsPinging = false;
                                        readerConnected = false;
                                        if (tcpClient != null && tcpClient.Connected)
                                        {
                                            tcpClient.Close();
                                            tcpClient = new TcpClient();
                                        }
                                        //sendMails();
                                    }
                                    else
                                    {
                                        //REGIME REMOTE SUCCESSFUL SET, RESET READER
                                        WriteLog("Reader RESET", Constants.extendedDebugLevel, -1);

                                        RFIDDevice rfidReset = new RFIDDevice();
                                        int statusReset = rfidReset.Open(validationTerminal.IpAddress);
                                        statusReset = rfidReset.Reset();
                                        rfidReset.Close();
                                        Thread.Sleep(2000);

                                        //checks if time is greater than 10sec and sets current time
                                        readerInterface.CheckTime(validationTerminal.IpAddress);
                                        WriteLog("Time after reset set.", Constants.extendedDebugLevel, -1);

                                        WriteLogDebugMessage("Reader status changed from STANDALONE to REMOTE successfully!", Constants.basicDebugLevel, -1);
                                        VTIsPinging = true;

                                        //sending mails when reset reader after changing status to remote
                                        readerConnLost = true;
                                        sendMailsReset();
                                    }

                                    countStandAloneStatus++;
                                    if (countStandAloneStatus >= 3)
                                    {
                                        VTIsPinging = false;
                                        readerConnected = false;
                                        if (tcpClient != null && tcpClient.Connected)
                                        {
                                            tcpClient.Close();
                                            tcpClient = new TcpClient();
                                        }
                                        //sendMails();
                                    }
                                }
                                else
                                {
                                    SetManagerStatus(statusOK);
                                    if (!VTIsPinging)
                                    {
                                        WriteLogDebugMessage("Reader connection test success!", Constants.basicDebugLevel, -1);
                                        setStatus(antALL, "Reader connection test success!");
                                        VTIsPinging = true;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (VTIsPinging)
                        {
                            WriteLogDebugMessage("Reader connection checking failed with exception: " + ex.Message, Constants.basicDebugLevel, -1);
                            setStatus(antALL, "Reader connection checking failed!");
                            VTIsPinging = false;
                            readerConnected = false;
                            if (tcpClient != null && tcpClient.Connected)
                            {
                                tcpClient.Close();
                                tcpClient = new TcpClient();
                            }
                            //sendMails();
                        }
                    }
                    finally
                    {
                        getRegime = VTIsPinging;
                        if (!VTIsPinging)
                            SetManagerStatus(statusNOTOK);
                        numOfTyries++;
                        WriteLogDebugMessage("getRegime = " + getRegime.ToString() + "Num of tries = " + numOfTyries.ToString(), 3, -1);
                    }
                }
            }

            return VTIsPinging;
        }

        //private bool logsDownload(ArrayList logLineList)
        //{
        //    bool succBool = false;
        //    try
        //    {
        //        // Kontrola da li su meseci u log zapisima u rastucem redosledu - anomalija primecena u Eunet-u, Hitag
        //        int prevMonth = -1;
        //        foreach (LogLine logLine in logLineList)
        //        {
        //            int month = Int32.Parse((logLine.DateTime).Substring(5, 2));
        //            if ((prevMonth > month) && (month != 1))
        //            {
        //                month = prevMonth;
        //                logLine.DateTime = logLine.DateTime.Substring(0, 5) + month.ToString("D2") + logLine.DateTime.Substring(7);
        //            }
        //            prevMonth = month;
        //        }
        //        int succ = 0;
        //        int total = 0;
        //        foreach (LogLine logLine in logLineList)
        //        {
        //            if (logLine.IsValid && logLine.TagID.ToString().Trim() != "0")
        //            {
        //                total++;
        //               succ += tag.InsertLog(long.Parse(logLine.TagID.ToString()), Convert.ToDateTime(logLine.DateTime.ToString()), validationTerminal.ValidationTerminalID, Convert.ToInt32(logLine.Antenna.ToString()));                      
        //            }
        //        }
        //        WriteLog("Log Interted OK Num = " + succ+" / Log Download Num Total = "+total, Constants.extendedDebugLevel);
        //        succBool = succ == total;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.writeLog(DateTime.Now + " Exception in: " + this.ToString() +
        //            ".logsDownload() : " + ex.Message);
        //    }
        //    return succBool;
        //}

        //private void AddToTAPDbytes(TimeAccessProfileDtl tapd, byte[] tapdB, string A0Direction, string A1Direction)
        //{
        //    if (tapd.Direction == A0Direction) AddToTAPDbytesForAntenna(tapd, tapdB, 0);
        //    if (tapd.Direction == A1Direction) AddToTAPDbytesForAntenna(tapd, tapdB, 1);
        //}

        //private void AddToTAPDbytesForAntenna(TimeAccessProfileDtl tapd, byte[] tapdB, int ant)
        //{
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 0] = (byte)tapd.Hrs0;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 1] = (byte)tapd.Hrs1;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 2] = (byte)tapd.Hrs2;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 3] = (byte)tapd.Hrs3;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 4] = (byte)tapd.Hrs4;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 5] = (byte)tapd.Hrs5;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 6] = (byte)tapd.Hrs6;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 7] = (byte)tapd.Hrs7;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 8] = (byte)tapd.Hrs8;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 9] = (byte)tapd.Hrs9;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 10] = (byte)tapd.Hrs10;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 11] = (byte)tapd.Hrs11;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 12] = (byte)tapd.Hrs12;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 13] = (byte)tapd.Hrs13;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 14] = (byte)tapd.Hrs14;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 15] = (byte)tapd.Hrs15;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 16] = (byte)tapd.Hrs16;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 17] = (byte)tapd.Hrs17;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 18] = (byte)tapd.Hrs18;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 19] = (byte)tapd.Hrs19;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 20] = (byte)tapd.Hrs20;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 21] = (byte)tapd.Hrs21;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 22] = (byte)tapd.Hrs22;
        //    tapdB[ant * (16 * 7 * 24) + tapd.TimeAccessProfileId * (7 * 24) + (tapd.DayOfWeek - 1) * 24 + 23] = (byte)tapd.Hrs23;
        //}

        //private bool UpdateReaderTimeAccessProfiles( ArrayList timeAccessProfiles)
        //{
        //    bool succeeded = false;
        //    IReaderInterface rfid = ReaderFactory.GetReader;

        //    byte[] TAPDbytes = new byte[2 * 16 * 7 * 24];
        //    for (int i = 0; i < TAPDbytes.Length; i++) TAPDbytes[i] = 0;

        //    try
        //    {
        //        WriteLog(DateTime.Now + " +++ TimeAccessProfilesUpload : STARTED! \n +++ ", Constants.extendedDebugLevel);

        //        ReaderFactory.TechnologyType = Constants.technologyType;
        //        rfid = ReaderFactory.GetReader;
        //        ArrayList timeAccessProfilesList = new ArrayList();

        //        foreach (TimeAccessProfileDtl timeAccessProfileDtl in timeAccessProfiles)
        //        {
        //            AddToTAPDbytes(timeAccessProfileDtl, TAPDbytes, "IN", "OUT");
        //        }
        //        WriteLog(DateTime.Now + " + Terminal ID: " + validationTerminal.ValidationTerminalID + " Num. time access profiles: " + timeAccessProfilesList.Count + "\n", Constants.extendedDebugLevel);

        //        rfid.SetTimeAccessProfiles(GetReaderAddress(), TAPDbytes);
        //        if (rfid.GetError().Equals(""))
        //        {
        //            succeeded = true;
        //            WriteLog(DateTime.Now + " +++ TimeAccessProfilesUpload : FINISHED! \n +++ ", Constants.extendedDebugLevel);
        //        }
        //        else
        //        {
        //            succeeded = false;
        //            WriteLog(DateTime.Now + " +++ TimeAccessProfilesUpload : FAILED! " + rfid.GetError() +
        //                "\n +++ ", Constants.extendedDebugLevel);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog(DateTime.Now + " Exception in: " +
        //            this.ToString() + ".UpdateReaderTimeAccessProfiles() : " + ex.StackTrace + "RFID Error: " + rfid.GetError() + "\n", Constants.basicDebugLevel);
        //    }

        //    return succeeded;
        //}

        private bool IsBlockPassTimeOut()
        {
            if ((DateTime.Now.Ticks - currentAntenna.lastTicketTime) / TimeSpan.TicksPerMillisecond < blockPassInterval)
            {
                Console.WriteLine("\nBlock pass time out!\n");
                if (this.logDebugMessages)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" Block pass time out!");
                    WriteLogDebugMessage(sb.ToString(), Constants.detailedDebugLevel, currentAntenna.AntennaNum);
                }

                return true;
            }

            return false;
        }

        private bool IsBlockPassTimeOutSameTicket()
        {
            if (previous_tag_id == tag_id && previous_ant_num == currentAntenna.AntennaNum && (DateTime.Now.Ticks - debouncingLastTicket) / TimeSpan.TicksPerMillisecond < debouncingTicketTimeOut)
                return true;
            else
                return false;
        }

        //returns snapshot from camera as byte array
        public byte[] GetSnapshot(string IP, out int total)
        {
            byte[] buffer = null;
            total = 0;
            try
            {
                string sourceURL = "http://" + IP + "/jpg/1/image.jpg";
                int read = 0;

                // create HTTP request
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sourceURL);
                req.Credentials = new NetworkCredential(Constants.CammeraUser, Constants.CammeraPass);
                req.Timeout = 1000;
                // get response
                try
                {
                    WebResponse resp = req.GetResponse();
                    buffer = new byte[100000];

                    // get response stream
                    Stream stream = resp.GetResponseStream();
                    // read data from stream
                    while ((read = stream.Read(buffer, total, 1000)) != 0)
                    {
                        total += read;
                    }

                    resp.Close();
                }
                catch (WebException ex)
                {
                    WriteLog(" Camera: " + IP + "; snapshots failed." + ex.Message, Constants.basicDebugLevel, -1);
                }
            }
            catch (Exception ex)
            {
                WriteLog(" Camera: " + IP + " snapshots failed." + ex.Message, Constants.basicDebugLevel, -1);
            }

            return buffer;
        }

        private void takeCameraSnapshot(string IP, string reader)
        {
            try
            {

                int total = 0;
                byte[] buffer = GetSnapshot(IP, out total);

                // get bitmap
                Bitmap bmp = (Bitmap)Bitmap.FromStream(
                              new MemoryStream(buffer, 0, total));
                bmp.Save(Constants.SnapshotsFilePath + "C_001_" + DateTime.Now.ToString("yy-MM-dd_HH-mm-ss") + "-01.jpg", ImageFormat.Jpeg);

            }
            catch (Exception ex)
            {
                WriteLog("Camera: " + IP + "; snapshots failed." + ex.Message, Constants.basicDebugLevel, -1);
            }
        }


        /**PRIMARY USED FOR FIAT PROCESS REQUEST, WITH MORE VALIDATION RULES**/
        private void ProcessRequestFiat()
        {
            screenMessage = "";
            string screenMess = "";
            string jmbg = "";

            currentAntenna.StartTicketTransaction();

            int returnValue = tagProcessor.check_passFiat(Constants.MESSAGE_TAG_DETECTED, validationTerminal.ValidationTerminalID, 0, currentAntenna.AntennaNum, currentAntenna.tagID, 0, ref response, ref buttons, ref screenMessage, new DateTime());


            //employee_id i JMBG zaposlenog pokupi
            if (tagProcessor.TagsEmployees.ContainsKey(currentAntenna.tagID))
            {
                currentAntenna.employeeID = tagProcessor.TagsEmployees[currentAntenna.tagID].EmployeeID;
                EmployeeAsco4TO emplAscp = new EmployeeAsco4TO();
                emplAscp.EmployeeID = currentAntenna.employeeID;
                EmployeeAsco4 empl = new EmployeeAsco4(dbConnection);
                empl.EmplAsco4TO = emplAscp;
                List<EmployeeAsco4TO> listAsco = empl.Search();

                if (listAsco[0].NVarcharValue4.Length > 0)
                    jmbg = listAsco[0].NVarcharValue4.Remove(7);
            }

            //TODO
            //pokupi point za taj citac, promeniti jer ce se iz configa citati point a ne reader
            OnlineMealsPoints onlinePoints = new OnlineMealsPoints(dbConnection);
            List<OnlineMealsPointsTO> listPoints = onlinePoints.Search(validationTerminal.IpAddress, currentAntenna.AntennaNum);

            //dictionary koji se salje monitoru
            string one = messageScreen[currentAntenna.AntennaNum][1];
            string two = messageScreen[currentAntenna.AntennaNum][2];
            string three = messageScreen[currentAntenna.AntennaNum][3];
            messageScreen[currentAntenna.AntennaNum][2] = one;
            messageScreen[currentAntenna.AntennaNum][3] = two;

            if (!IsBlockPassTimeOut())
            {
                if (returnValue == (int)Constants.MealDenied.Approved)
                {
                    //da li je koriscen obrok
                    OnlineMealsUsed mealUsed = new OnlineMealsUsed(dbConnection);
                    List<OnlineMealsUsedTO> list = mealUsed.Search(currentAntenna.employeeID, DateTime.Now.Date, DateTime.Now.Date);
                    if (list.Count == 0)
                    {
                        //TODO dodaj poruke u Constants
                        screenMess = jmbg + " Odobren";
                        messageScreen[currentAntenna.AntennaNum][1] = screenMess;

                        foreach (KeyValuePair<int, string> pair in messageScreen[currentAntenna.AntennaNum])
                        {
                            screenMessage += pair.Value.PadRight(20);


                        }
                        byte[] passIsAllowedMessage = communicationMessage.GetPassIsAllowedMessageBytes(screenMessage);
                        netStream.Flush();
                        netStream.Write(passIsAllowedMessage, 0, communicationMessage.MESSAGE_LENGTH_RESPONSE);

                        Console.WriteLine("APPLICATION RESPONSE: Pass is allowed!");
                        Console.WriteLine("\n");
                        for (int i = 0; i < passIsAllowedMessage.Length; i++)
                        {
                            Console.Write(passIsAllowedMessage[i].ToString("X2") + " ");
                        }
                        Console.WriteLine("\n");

                        if (this.logDebugMessages)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append(" APPLICATION RESPONSE: Pass is allowed!");
                            // sb.Append("\n");
                            for (int i = 0; i < passIsAllowedMessage.Length; i++)
                            {
                                sb.Append(passIsAllowedMessage[i].ToString("X2") + " ");
                            }
                            // sb.Append("\n\n");

                            this.WriteLogDebugMessage(sb.ToString(), Constants.extendedDebugLevel, currentAntenna.AntennaNum);
                        }

                        // sacuvaj obrok
                        if (listPoints.Count == 1)
                        {
                            OnlineMealsUsed onlineMeal = new OnlineMealsUsed(dbConnection);

                            OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                            onlineMealUsedTo.EmployeeID = currentAntenna.employeeID;
                            onlineMealUsedTo.PointID = listPoints[0].PointID;
                            onlineMealUsedTo.MealTypeID = listPoints[0].MealTypeID;
                            onlineMealUsedTo.Qty = 1;
                            onlineMealUsedTo.OnlineValidation = 1;
                            onlineMealUsedTo.ManualCreated = 0;
                            onlineMealUsedTo.EventTime = DateTime.Now;
                            onlineMealUsedTo.CreatedBy = "RC SERVICE";
                            onlineMealUsedTo.CreatedTime = DateTime.Now;

                            onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                            onlineMeal.Save(true);
                        }
                        currentAntenna.state = State.WAITING_FOR_TERMINAL_RESPONSE;
                    }
                    else
                    {
                        byte[] passIsDeniedMessage;
                        screenMess = jmbg + " Iskorišćen";
                        messageScreen[currentAntenna.AntennaNum][1] = screenMess;

                        foreach (KeyValuePair<int, string> pair in messageScreen[currentAntenna.AntennaNum])
                        {
                            screenMessage += pair.Value.PadRight(20);
                        }
                        passIsDeniedMessage = communicationMessage.GetPassIsDeniedMessageBytes(screenMessage);
                        netStream.Flush();
                        netStream.Write(passIsDeniedMessage, 0, passIsDeniedMessage.Length);
                        StringBuilder sb = new StringBuilder();
                        sb.Append(" APPLICATION RESPONSE: Pass is denied!");
                        // sb.Append("\n");
                        for (int i = 0; i < passIsDeniedMessage.Length; i++)
                        {
                            sb.Append(passIsDeniedMessage[i].ToString("X2") + " ");
                        }
                        // sb.Append("\n\n");

                        //sacuvaj poruku da je obrok koriscen
                        if (listPoints.Count == 1)
                        {
                            OnlineMealsUsed onlineMeal = new OnlineMealsUsed(dbConnection);

                            OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                            onlineMealUsedTo.EmployeeID = currentAntenna.employeeID;
                            onlineMealUsedTo.PointID = listPoints[0].PointID;
                            onlineMealUsedTo.MealTypeID = listPoints[0].MealTypeID;
                            onlineMealUsedTo.Qty = 1;
                            onlineMealUsedTo.ReasonRefused = screenMess;
                            onlineMealUsedTo.OnlineValidation = 0;
                            onlineMealUsedTo.EventTime = DateTime.Now;
                            onlineMealUsedTo.ManualCreated = 0;
                            onlineMealUsedTo.CreatedBy = "RC SERVICE";
                            onlineMealUsedTo.CreatedTime = DateTime.Now;

                            onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                            onlineMeal.Save(true);
                        }
                        currentAntenna.FinishTicketTransaction();
                        currentAntenna.state = State.WAITING_FOR_TERMINAL_REQUEST;
                        this.WriteLogDebugMessage(sb.ToString(), Constants.extendedDebugLevel, currentAntenna.AntennaNum);
                    }
                }
                else
                {
                    byte[] passIsDeniedMessage;


                    if (returnValue == (int)Constants.MealDenied.CompanyRestaurant)
                    {
                        screenMess = jmbg + " Restoran";

                        if (listPoints.Count == 1)
                        {
                            OnlineMealsUsed onlineMeal = new OnlineMealsUsed(dbConnection);
                            OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                            onlineMealUsedTo.EmployeeID = currentAntenna.employeeID;
                            onlineMealUsedTo.PointID = listPoints[0].PointID;
                            onlineMealUsedTo.MealTypeID = listPoints[0].MealTypeID;
                            onlineMealUsedTo.Qty = 1;
                            onlineMealUsedTo.ReasonRefused = screenMess;
                            onlineMealUsedTo.OnlineValidation = 0;
                            onlineMealUsedTo.EventTime = DateTime.Now;
                            onlineMealUsedTo.ManualCreated = 0;
                            onlineMealUsedTo.CreatedBy = "RC SERVICE";
                            onlineMealUsedTo.CreatedTime = DateTime.Now;

                            onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                            onlineMeal.Save(true);
                        }

                    }
                    else if (returnValue == (int)Constants.MealDenied.EmployeeNotActive)
                    {

                        screenMess = jmbg + " Neaktivan";
                    }
                    else if (returnValue == (int)Constants.MealDenied.TagNotActive)
                    {
                        screenMess = jmbg + " Kartica";

                    }
                    else if (returnValue == (int)Constants.MealDenied.NotWorkingSchedule)
                    {
                        screenMess = jmbg + " Nema smene";


                        if (listPoints.Count == 1)
                        {
                            OnlineMealsUsed onlineMeal = new OnlineMealsUsed(dbConnection);

                            OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                            onlineMealUsedTo.EmployeeID = currentAntenna.employeeID;
                            onlineMealUsedTo.PointID = listPoints[0].PointID;
                            onlineMealUsedTo.MealTypeID = listPoints[0].MealTypeID;
                            onlineMealUsedTo.Qty = 1;
                            onlineMealUsedTo.ReasonRefused = screenMess;
                            onlineMealUsedTo.OnlineValidation = 0;
                            onlineMealUsedTo.ManualCreated = 0;
                            onlineMealUsedTo.EventTime = DateTime.Now;
                            onlineMealUsedTo.CreatedBy = "RC SERVICE";
                            onlineMealUsedTo.CreatedTime = DateTime.Now;

                            onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                            onlineMeal.Save(true);
                        }
                    }
                    else if (returnValue == (int)Constants.MealDenied.WholeDayAbsence)
                    {
                        screenMess = jmbg + " Odsustvo";

                        if (listPoints.Count == 1)
                        {
                            OnlineMealsUsed onlineMeal = new OnlineMealsUsed(dbConnection);

                            OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                            onlineMealUsedTo.EmployeeID = currentAntenna.employeeID;
                            onlineMealUsedTo.PointID = listPoints[0].PointID;
                            onlineMealUsedTo.MealTypeID = listPoints[0].MealTypeID;
                            onlineMealUsedTo.Qty = 1;
                            onlineMealUsedTo.ReasonRefused = screenMess;
                            onlineMealUsedTo.OnlineValidation = 0;
                            onlineMealUsedTo.ManualCreated = 0;
                            onlineMealUsedTo.EventTime = DateTime.Now;
                            onlineMealUsedTo.CreatedBy = "RC SERVICE";
                            onlineMealUsedTo.CreatedTime = DateTime.Now;

                            onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                            onlineMeal.Save(true);
                        }
                    }

                    messageScreen[currentAntenna.AntennaNum][1] = screenMess;

                    foreach (KeyValuePair<int, string> pair in messageScreen[currentAntenna.AntennaNum])
                    {
                        screenMessage += pair.Value.PadRight(20);
                    }

                    passIsDeniedMessage = communicationMessage.GetPassIsDeniedMessageBytes(screenMessage);
                    netStream.Flush();
                    netStream.Write(passIsDeniedMessage, 0, passIsDeniedMessage.Length);

                    StringBuilder sb = new StringBuilder();
                    sb.Append(" APPLICATION RESPONSE: Pass is denied!");
                    // sb.Append("\n");
                    for (int i = 0; i < passIsDeniedMessage.Length; i++)
                    {
                        sb.Append(passIsDeniedMessage[i].ToString("X2") + " ");
                    }
                    // sb.Append("\n\n");

                    this.WriteLogDebugMessage(sb.ToString(), Constants.extendedDebugLevel, currentAntenna.AntennaNum);


                    currentAntenna.FinishTicketTransaction();
                    currentAntenna.state = State.WAITING_FOR_TERMINAL_REQUEST;
                    this.WriteLogDebugMessage(sb.ToString(), Constants.extendedDebugLevel, currentAntenna.AntennaNum);
                }

            }
        }

        private void populateDictionaryMonitor(int antenna)
        {
            for (int i = 8; i >= 1; i--)
            {
                if (messageScreen[antenna].ContainsKey(i))
                {
                    if (i != 1)
                        messageScreen[antenna][i] = messageScreen[antenna][i - 1];
                }
            }
        }

        private void ProcessRequest()
        {
            try
            {
                currentAntenna.StartTicketTransaction();

                bool button = false;
                int result = 0;
                screenMessage = "";
                string screenMess = "";
                string jmbg = "";
                string[] listForMonitor = new string[9];

                populateDictionaryMonitor(currentAntenna.AntennaNum);

                if (!IsBlockPassTimeOut() && CheckRequest(ref button, ref result))
                {
                    if (button)
                    {
                        communicationMessage.displayNum = (byte)(buttons + 2);
                        byte[] waitForButtonMessage = communicationMessage.GetWaitForButtonMessageMessageBytes();
                        netStream.Write(waitForButtonMessage, 0, communicationMessage.MESSAGE_LENGTH);

                        Console.WriteLine("APPLICATION RESPONSE: Show buttons!");
                        Console.WriteLine("\n");
                        string s = "";
                        for (int i = 0; i < waitForButtonMessage.Length; i++)
                        {
                            Console.Write(waitForButtonMessage[i].ToString("X2") + " ");
                            s += waitForButtonMessage[i].ToString("X2") + " ";
                        }
                        Console.WriteLine("\n");
                        if (this.logDebugMessages)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append(" APPLICATION RESPONSE: ShowButtons!");

                            for (int i = 0; i < waitForButtonMessage.Length; i++)
                            {
                                sb.Append(waitForButtonMessage[i].ToString("X2") + " ");
                            }
                            this.WriteLogDebugMessage(sb.ToString(), Constants.extendedDebugLevel, currentAntenna.AntennaNum);
                        }

                        currentAntenna.state = State.WAITING_FOR_TERMINAL_RESPONSE_BUTTON;
                    }
                    else
                    {

                        if (tagProcessor.TagsEmployees.ContainsKey(currentAntenna.tagID))
                        {
                            currentAntenna.employeeID = tagProcessor.TagsEmployees[currentAntenna.tagID].EmployeeID;

                            if (tagProcessor.EmployeesAsco4.ContainsKey(currentAntenna.employeeID))
                                jmbg = tagProcessor.EmployeesAsco4[currentAntenna.employeeID].NVarcharValue4;

                        }
                        int list = 0;


                        OnlineMealsUsedDaily mealUsed = new OnlineMealsUsedDaily(dbConnection);
                        list = mealUsed.Count(currentAntenna.employeeID, DateTime.Now.Date, DateTime.Now.Date);

                        if (list == 0)
                        {

                            screenMess = Constants.RestaurantApproved;
                            //ADD GREEN TO DICTIONARY FOR STATUS LINE ON MONITOR
                            greenValidationsDict[currentAntenna.AntennaNum]++;
                            if (jmbg.Length <= 13)
                                messageScreen[currentAntenna.AntennaNum][1] = jmbg + ";" + screenMess + ";" + DateTime.Now.ToString("HH:mm:ss");
                            else
                                messageScreen[currentAntenna.AntennaNum][1] = jmbg.Remove(13) + ";" + screenMess + ";" + DateTime.Now.ToString("HH:mm:ss");

                            int index = 0;
                            foreach (KeyValuePair<int, string> pair in messageScreen[currentAntenna.AntennaNum])
                            {
                                listForMonitor[index] = pair.Value;
                                index++;
                            }

                            listForMonitor[8] = greenValidationsDict[currentAntenna.AntennaNum] + ";" + redValidationsDict[currentAntenna.AntennaNum];
                            // sacuvaj obrok

                            OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                            onlineMealUsedTo.EmployeeID = currentAntenna.employeeID;
                            if (distionaryPoints.ContainsKey(currentAntenna.AntennaNum))
                            {
                                onlineMealUsedTo.PointID = distionaryPoints[currentAntenna.AntennaNum].PointID;
                                onlineMealUsedTo.MealTypeID = distionaryPoints[currentAntenna.AntennaNum].MealTypeID;
                                onlineMealUsedTo.Qty = 1;
                                onlineMealUsedTo.OnlineValidation = 1;
                                onlineMealUsedTo.ManualCreated = 0;
                                onlineMealUsedTo.EventTime = DateTime.Now;
                                onlineMealUsedTo.CreatedBy = "RC SERVICE";
                                onlineMealUsedTo.CreatedTime = DateTime.Now;


                                OnlineMealsUsedDaily onlineMeal = new OnlineMealsUsedDaily(dbConnection);
                                onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                                onlineMeal.Save(true, 0);

                            }

                            //byte[] passIsAllowedMessage = communicationMessage.GetPassIsAllowedMessageBytes(screenMess);
                            byte[] passIsAllowedMessage = communicationMessage.CanteenMonitorFIAT(listForMonitor, currentAntenna.AntennaNum);
                            netStream.Flush();
                            netStream.Write(passIsAllowedMessage, 0, passIsAllowedMessage.Length);

                            Console.WriteLine("APPLICATION RESPONSE: Pass is allowed!");
                            Console.WriteLine("\n");
                            for (int i = 0; i < passIsAllowedMessage.Length; i++)
                            {
                                Console.Write(passIsAllowedMessage[i].ToString("X2") + " ");
                            }
                            Console.WriteLine("\n");

                            if (this.logDebugMessages)
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append(" APPLICATION RESPONSE: Pass is allowed!");
                                this.WriteLogDebugMessage(sb.ToString(), Constants.extendedDebugLevel, currentAntenna.AntennaNum);
                            }
                            //currentAntenna.state = State.WAITING_FOR_TERMINAL_RESPONSE;
                            //DON'T NEED TO WAIT FOR TERMINAL RESPONSE, VISITORS DON'T PASS, JUST CHECK
                            currentAntenna.FinishTicketTransaction();
                            currentAntenna.state = State.WAITING_FOR_TERMINAL_REQUEST;
                        }
                        else
                        {
                            byte[] passIsDeniedMessage;

                            screenMess = Constants.RestaurantUsed;

                            //ADD RED TO DICTIONARY FOR STATUS LINE ON MONITOR
                            redValidationsDict[currentAntenna.AntennaNum]++;
                            if (jmbg.Length <= 13)
                                messageScreen[currentAntenna.AntennaNum][1] = jmbg + ";" + screenMess + ";" + DateTime.Now.ToString("HH:mm:ss");
                            else
                                messageScreen[currentAntenna.AntennaNum][1] = jmbg.Remove(13) + ";" + screenMess + ";" + DateTime.Now.ToString("HH:mm:ss");

                            int index = 0;
                            foreach (KeyValuePair<int, string> pair in messageScreen[currentAntenna.AntennaNum])
                            {
                                listForMonitor[index] = pair.Value;
                                index++;
                            }
                            listForMonitor[8] = greenValidationsDict[currentAntenna.AntennaNum] + ";" + redValidationsDict[currentAntenna.AntennaNum];
                            //sacuvaj poruku da je obrok koriscen
                            OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                            onlineMealUsedTo.EmployeeID = currentAntenna.employeeID;
                            if (distionaryPoints.ContainsKey(currentAntenna.AntennaNum))
                            {
                                onlineMealUsedTo.PointID = distionaryPoints[currentAntenna.AntennaNum].PointID;
                                onlineMealUsedTo.MealTypeID = distionaryPoints[currentAntenna.AntennaNum].MealTypeID;
                                onlineMealUsedTo.Qty = 1;
                                onlineMealUsedTo.ReasonRefused = Constants.RestaurantUsed;
                                onlineMealUsedTo.OnlineValidation = 0;
                                onlineMealUsedTo.EventTime = DateTime.Now;
                                onlineMealUsedTo.ManualCreated = 0;
                                onlineMealUsedTo.CreatedBy = "RC SERVICE";
                                onlineMealUsedTo.CreatedTime = DateTime.Now;


                                OnlineMealsUsedDaily onlineMeal = new OnlineMealsUsedDaily(dbConnection);
                                onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                                onlineMeal.Save(true, 0);

                            }

                            passIsDeniedMessage = communicationMessage.CanteenMonitorFIAT(listForMonitor, currentAntenna.AntennaNum);
                            netStream.Flush();
                            netStream.Write(passIsDeniedMessage, 0, passIsDeniedMessage.Length);

                            StringBuilder sb = new StringBuilder();
                            sb.Append(" APPLICATION RESPONSE: Pass is denied!" + screenMess + " ");

                            this.WriteLogDebugMessage(sb.ToString(), Constants.extendedDebugLevel, currentAntenna.AntennaNum);
                            currentAntenna.FinishTicketTransaction();
                            currentAntenna.state = State.WAITING_FOR_TERMINAL_REQUEST;

                        }
                    }

                }
                else
                {
                    byte[] passIsDeniedMessage;


                    if (tagProcessor.TagsEmployees.ContainsKey(currentAntenna.tagID))
                    {
                        currentAntenna.employeeID = tagProcessor.TagsEmployees[currentAntenna.tagID].EmployeeID;

                        if (tagProcessor.EmployeesAsco4.ContainsKey(currentAntenna.employeeID))
                            jmbg = tagProcessor.EmployeesAsco4[currentAntenna.employeeID].NVarcharValue4;

                    }

                    if (result == (int)Constants.MealDenied.CompanyRestaurant)
                    {
                        screenMess = Constants.RestaurantCompany;
                        //ADD RED TO DICTIONARY FOR STATUS LINE ON MONITOR
                        redValidationsDict[currentAntenna.AntennaNum]++;

                        OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                        onlineMealUsedTo.EmployeeID = currentAntenna.employeeID;
                        if (distionaryPoints.ContainsKey(currentAntenna.AntennaNum))
                        {
                            onlineMealUsedTo.PointID = distionaryPoints[currentAntenna.AntennaNum].PointID;
                            onlineMealUsedTo.MealTypeID = distionaryPoints[currentAntenna.AntennaNum].MealTypeID;
                            onlineMealUsedTo.Qty = 1;
                            onlineMealUsedTo.ReasonRefused = Constants.RestaurantCompany;
                            onlineMealUsedTo.OnlineValidation = 0;
                            onlineMealUsedTo.EventTime = DateTime.Now;
                            onlineMealUsedTo.ManualCreated = 0;
                            onlineMealUsedTo.CreatedBy = "RC SERVICE";
                            onlineMealUsedTo.CreatedTime = DateTime.Now;


                            OnlineMealsUsedDaily onlineMeal = new OnlineMealsUsedDaily(dbConnection);
                            onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                            onlineMeal.Save(true, 0);


                        }
                    }
                    else if (result == (int)Constants.MealDenied.EmployeeNotActive)
                    {

                        screenMess = Constants.RestaurantEmployeeNotActive;

                        //ADD RED TO DICTIONARY FOR STATUS LINE ON MONITOR
                        redValidationsDict[currentAntenna.AntennaNum]++;
                        OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                        onlineMealUsedTo.EmployeeID = currentAntenna.employeeID;
                        if (distionaryPoints.ContainsKey(currentAntenna.AntennaNum))
                        {
                            onlineMealUsedTo.PointID = distionaryPoints[currentAntenna.AntennaNum].PointID;
                            onlineMealUsedTo.MealTypeID = distionaryPoints[currentAntenna.AntennaNum].MealTypeID;
                            onlineMealUsedTo.Qty = 1;
                            onlineMealUsedTo.ReasonRefused = Constants.RestaurantEmployeeNotActive;
                            onlineMealUsedTo.OnlineValidation = 0;
                            onlineMealUsedTo.EventTime = DateTime.Now;
                            onlineMealUsedTo.ManualCreated = 0;
                            onlineMealUsedTo.CreatedBy = "RC SERVICE";
                            onlineMealUsedTo.CreatedTime = DateTime.Now;


                            OnlineMealsUsedDaily onlineMeal = new OnlineMealsUsedDaily(dbConnection);
                            onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                            onlineMeal.Save(true, 0);

                        }
                    }
                    else if (result == (int)Constants.MealDenied.TagUnknown)
                    {
                        screenMess = Constants.RestaurantTagUnknown;
                        jmbg = "0000000000000";
                    }
                    else if (result == (int)Constants.MealDenied.TagNotActive)
                    {
                        //ADD RED TO DICTIONARY FOR STATUS LINE ON MONITOR
                        redValidationsDict[currentAntenna.AntennaNum]++;
                        EmployeeTO empl = new Employee(dbConnection).SearchByTag(tag_id.ToString());
                        if (empl != new EmployeeTO())
                        {

                            EmployeeAsco4TO emplAscp = new EmployeeAsco4TO();
                            emplAscp.EmployeeID = empl.EmployeeID;
                            EmployeeAsco4 emplAsco = new EmployeeAsco4(dbConnection);
                            emplAsco.EmplAsco4TO = emplAscp;
                            List<EmployeeAsco4TO> listAsco = emplAsco.Search();

                            if (listAsco[0].NVarcharValue4.Length > 0)
                                jmbg = listAsco[0].NVarcharValue4;

                            screenMess = Constants.RestaurantTagNotActive;


                            OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                            onlineMealUsedTo.EmployeeID = empl.EmployeeID;
                            if (distionaryPoints.ContainsKey(currentAntenna.AntennaNum))
                            {
                                onlineMealUsedTo.PointID = distionaryPoints[currentAntenna.AntennaNum].PointID;
                                onlineMealUsedTo.MealTypeID = distionaryPoints[currentAntenna.AntennaNum].MealTypeID;
                                onlineMealUsedTo.Qty = 1;
                                onlineMealUsedTo.ReasonRefused = Constants.RestaurantTagNotActive;
                                onlineMealUsedTo.OnlineValidation = 0;
                                onlineMealUsedTo.EventTime = DateTime.Now;
                                onlineMealUsedTo.ManualCreated = 0;
                                onlineMealUsedTo.CreatedBy = "RC SERVICE";
                                onlineMealUsedTo.CreatedTime = DateTime.Now;


                                OnlineMealsUsedDaily onlineMeal = new OnlineMealsUsedDaily(dbConnection);
                                onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                                onlineMeal.Save(true, 0);


                            }
                        }
                    }
                    else if (result == (int)Constants.MealDenied.Location)
                    {
                        //ADD RED TO DICTIONARY FOR STATUS LINE ON MONITOR
                        redValidationsDict[currentAntenna.AntennaNum]++;

                        screenMess = Constants.RestaurantLocation;


                        OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                        onlineMealUsedTo.EmployeeID = currentAntenna.employeeID;
                        if (distionaryPoints.ContainsKey(currentAntenna.AntennaNum))
                        {
                            onlineMealUsedTo.PointID = distionaryPoints[currentAntenna.AntennaNum].PointID;

                            onlineMealUsedTo.MealTypeID = distionaryPoints[currentAntenna.AntennaNum].MealTypeID;
                            onlineMealUsedTo.Qty = 1;
                            onlineMealUsedTo.ReasonRefused = Constants.RestaurantLocation;
                            onlineMealUsedTo.OnlineValidation = 0;
                            onlineMealUsedTo.EventTime = DateTime.Now;
                            onlineMealUsedTo.ManualCreated = 0;
                            onlineMealUsedTo.CreatedBy = "RC SERVICE";
                            onlineMealUsedTo.CreatedTime = DateTime.Now;


                            OnlineMealsUsedDaily onlineMeal = new OnlineMealsUsedDaily(dbConnection);
                            onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                            onlineMeal.Save(true, 0);

                        }
                    }

                    if (jmbg.Length <= 13)
                        messageScreen[currentAntenna.AntennaNum][1] = jmbg + ";" + screenMess + ";" + DateTime.Now.ToString("HH:mm:ss");
                    else
                        messageScreen[currentAntenna.AntennaNum][1] = jmbg.Remove(13) + ";" + screenMess + ";" + DateTime.Now.ToString("HH:mm:ss");

                    int index = 0;
                    foreach (KeyValuePair<int, string> pair in messageScreen[currentAntenna.AntennaNum])
                    {
                        listForMonitor[index] = pair.Value;
                        index++;
                    }
                    listForMonitor[8] = greenValidationsDict[currentAntenna.AntennaNum] + ";" + redValidationsDict[currentAntenna.AntennaNum];


                    passIsDeniedMessage = communicationMessage.CanteenMonitorFIAT(listForMonitor, currentAntenna.AntennaNum);
                    netStream.Flush();
                    netStream.Write(passIsDeniedMessage, 0, passIsDeniedMessage.Length);

                    StringBuilder sb = new StringBuilder();
                    sb.Append(" APPLICATION RESPONSE: Pass is denied!" + screenMess + " ");


                    currentAntenna.FinishTicketTransaction();
                    currentAntenna.state = State.WAITING_FOR_TERMINAL_REQUEST;
                    this.WriteLogDebugMessage(sb.ToString(), Constants.extendedDebugLevel, currentAntenna.AntennaNum);
                }
            }
            catch (Exception ex)
            {
                this.WriteLogDebugMessage(ex.Message, Constants.basicDebugLevel, currentAntenna.AntennaNum);
                throw ex;
            }
        }

        private bool CheckRequest(ref bool button, ref int returnValue)
        {
            bool valid = false;

            response = 0;
            buttons = 0;
            screenMessage = "";
            bool ticketInProcess = false;
            foreach (AntennaCommunication ant in antennaList)
            {
                if (ant.AntennaNum != currentAntenna.AntennaNum && ant.tagID == currentAntenna.tagID)
                {
                    ticketInProcess = true;
                }
            }
            if (!ticketInProcess)
            {
                try
                {

                    //WriteLogDebugMessage(" ++CheckMethod() (antenna-" + currentAntenna.AntennaNum + ") called with params: message_type = " + Constants.MESSAGE_TAG_DETECTED + "; validation_terminal_id = " + validationTerminal.ValidationTerminalID + "; antenna = " + currentAntenna.AntennaNum
                    //   + "; tag_id = " + currentAntenna.tagID.ToString() + "; pass_detected = 0; button_number = 0" + "++", Constants.detailedDebugLevel);
                    response = 0;
                    //int returnValue = tag.check_pass(Constants.MESSAGE_TAG_DETECTED, validationTerminal.ValidationTerminalID, currentAntenna.AntennaNum, 0, currentAntenna.tagID, 0, ref response, ref buttons, ref screenMessage);
                    //WriteLogDebugMessage(" --CheckMethod() (antenna-" + currentAntenna.AntennaNum + "); return value = 1"+"; return params: response = " + response.ToString() + "; buttons = " + buttons.ToString() + "; screenMessage = " + screenMessage + "--", Constants.detailedDebugLevel);
                    valid = tagProcessor.CheckRequest(currentAntenna.tagID, currentAntenna.AntennaNum, ref returnValue);
                }
                catch (Exception ex)
                {
                    WriteLog("Tag.CheckMethod() throw exception: " + ex.Message, Constants.basicDebugLevel, currentAntenna.AntennaNum);
                }
                //valid = response > 0;
                button = response == 2;

            }

            return valid;
        }

        private bool CheckRequestButton(int btn)
        {
            bool valid = true;
            response = 0;
            buttons = 0;
            screenMessage = "";
            try
            {
                //WriteLogDebugMessage(" ++CheckMethod() (antenna-"+currentAntenna.AntennaNum+") called with params: message_type = " + Constants.MESSAGE_BUTTON_PRESSED + "; validation_terminal_id = " + validationTerminal.ValidationTerminalID + "; antenna = " + currentAntenna.AntennaNum
                //        + "; tag_id = " + currentAntenna.tagID.ToString() + "; pass_detected = 0; button_number = " + btn + "++", Constants.detailedDebugLevel);
                //int returnValue = tag.check_pass(Constants.MESSAGE_BUTTON_PRESSED, validationTerminal.ValidationTerminalID, currentAntenna.AntennaNum, 0, currentAntenna.tagID, btn, ref response, ref buttons, ref screenMessage);
                //WriteLogDebugMessage(" --CheckMethod() (antenna-" + currentAntenna.AntennaNum + "); return value = " + returnValue.ToString() + "; return params: response = " + response.ToString() + "; buttons = " + buttons.ToString() + "; screenMessage = " + screenMessage + "--", Constants.detailedDebugLevel);

            }
            catch (Exception ex)
            {
                WriteLog("Tag.CheckMethod() throw exception: " + ex.Message, Constants.basicDebugLevel, -1);
            }
            valid = (response > 0);
            return valid;
        }


        public void SetTicketID(string text)
        {
            lock (locker)
            {
                try
                {
                    currentAntenna.tagID = uint.Parse(text);
                    currentAntenna.employeeID = -1;
                    currentAntenna.mealTypeID = -1;
                    currentAntenna.transID = -1;
                    tag_id = currentAntenna.tagID;


                }
                catch { }
            }
        }

        public uint GetTicketID()
        {
            lock (locker)
            {
                return tag_id;
            }
        }

        public string GetCounter()
        {
            lock (locker)
            {
                return counter.ToString();
            }
        }

        public string GetValidationTerminalID()
        {
            lock (locker)
            {
                return validationTerminal.ValidationTerminalID.ToString();
            }
        }

        public string GetManagerStatus()
        {
            lock (locker)
            {
                return managerStatus;
            }
        }

        public string GetManagerThreadStatus()
        {
            lock (locker)
            {
                return managerThreadStatus;
            }
        }

        private void SetManagerStatus(string text)
        {
            lock (locker)
            {
                managerStatus = text;
            }
        }

        private void SetManagerThreadStatus(string text)
        {
            lock (locker)
            {
                managerThreadStatus = text;
            }
        }


        public void WriteLog(string message, int level, int antenna)
        {
            if (antenna != -1)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal IP: " + validationTerminal.IpAddress + "; antenna: " + antenna + "; " + message.Trim(), level);
                logForAll.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal ID: " + validationTerminal.IpAddress + "; antenna: " + antenna + "; " + message.Trim(), level);
            }
            else
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal IP: " + validationTerminal.IpAddress + "; " + message.Trim(), level);
                logForAll.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal ID: " + validationTerminal.IpAddress + "; " + message.Trim(), level);
            }
        }
        public void WriteLogDebugMessage(string message, int level, int antenna)
        {
            if (antenna != -1)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal IP: " + validationTerminal.IpAddress + "; antenna: " + antenna + "; " + message.Trim(), level);
                logForAll.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal ID: " + validationTerminal.IpAddress + "; antenna: " + antenna + "; " + message.Trim(), level);
            }
            else
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal IP: " + validationTerminal.IpAddress + "; " + message.Trim(), level);
                logForAll.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal ID: " + validationTerminal.IpAddress + "; " + message.Trim(), level);
            }
        }
        public void WriteLogDebugNewLine()
        {
            log.writeLog("\n", Constants.detailedDebugLevel);
            logForAll.writeLog("\n", Constants.detailedDebugLevel);
        }

        //public void DropArm()
        //{
        //    try
        //    {
        //        if (netStream != null)
        //        {
        //            // Drop arm
        //            byte[] dropArmMessage = communicationMessage.GetDropArmMessageBytes();
        //            netStream.Write(dropArmMessage, 0, communicationMessage.MESSAGE_LENGTH);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.writeLog(DateTime.Now.ToString() + "TicketControlManager.DropArm() " + ex.Message);
        //        //throw ex;
        //    }
        //}

    }
}
