using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using Common;
using TransferObjects;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using ReaderInterface;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Configuration;

namespace ACTAReaderMonitorManagement
{
    public enum State { WAITING_FOR_TERMINAL_REQUEST, WAITING_FOR_APPLICATION_RESPONSE, WAITING_FOR_TERMINAL_RESPONSE, WAITING_FOR_TERMINAL_RESPONSE_BUTTON };

    public class ACTAReaderMonitorManager
    {
        //thread for reading cards on validation terminal
        private Thread worker;

        string readerPort = "11001";

        Object dbConnection = null;
        Object dbConnectionData = null;
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

        public bool logDebugMessages = true;


        //number of tickets pass on reader 
        private int counter = 0;


        //check reader connection period
        private long readerCheckTimeOut = 10000;  // in ms
        private long lastConnectionCheckingTime = 0;


        bool readerConnected = true;
        //check upload access profiles period
        private long uploadProfilesOnceDayTimeOut = 86400000;  // in ms 
        private long lastUploadProfilesOnceDayTime = 0;

        private string statusOK = "1";
        private string statusNOTOK = "0";

        private int antALL = -1;
        private int antIn = 0;
        private int antOut = 1;
        uint tag_id = 0;


        private string inOut = "INOUT";

        private long lastUsedDataRead = 1;
        private long lastUsedDataReadTime = 0;

        Dictionary<uint, EmployeeTO> dictionaryEmployees = new Dictionary<uint, EmployeeTO>();
        Dictionary<int, Dictionary<int, GateTO>> dictionaryGates = new Dictionary<int, Dictionary<int, GateTO>>();

        Dictionary<string, List<CameraTO>> dictionaryCameras = new Dictionary<string, List<CameraTO>>();
        //tags active
        Dictionary<ulong, TagTO> tagsActive = new Dictionary<ulong, TagTO>();

        private ArrayList readerInterfaces = new ArrayList();
        ReaderTO reader = new ReaderTO();
        IReaderInterface readerInterface;

        public ACTAReaderMonitorManager(ReaderTO vt)
        {
            try
            {
                managerThreadStatus = statusNOTOK;
                NotificationController.SetApplicationName("ACTAReaderMonitorService");
                log = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log_" + vt.ConnectionAddress + ".txt");
                logForAll = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log" + ".txt");

                this.reader = vt;
                readerPort = ConfigurationManager.AppSettings["readerPort"];
                if (readerPort == null || readerPort == "")
                {
                    readerPort = "11001";
                }

                ReaderFactory.TechnologyType = reader.TechType;
                readerInterface = ReaderFactory.GetReader;
            }
            catch (Exception ex)
            {
                WriteLogDebugMessage("error in ACTAReaderMonitorManager " + ex.Message, Constants.basicDebugLevel, -1);
                throw ex;
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

            //if (OpenReadersConnections())
            //{

                WriteLog(" TickeControler started!", Constants.basicDebugLevel, -1);
                setStatus(antALL, " Reader not connected!");
                worker = new Thread(new ThreadStart(Work));
                readerConnected = false;
                worker.Name = "TicketControlManager";
                worker.Start();
            //}
            //lastConnectionCheckingTime = DateTime.Now.Ticks;
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
        private bool GetUsedDataTime()
        {
            if (lastUsedDataReadTime == 0 || ((DateTime.Now.Ticks - lastUsedDataReadTime) / TimeSpan.TicksPerMillisecond > lastUsedDataRead * 60 * 1000))
            {

                lastUsedDataReadTime = DateTime.Now.Ticks;
                //getUsedData();
                return true;
            }
            else
            {
                return false;
            }
        }
    
        //private unsafe bool ReaderConnectedSucc(IReaderInterface readerInterface, ReaderTO reader)
        //{
        //    if (dictionaryLastConnectionCheckingTime.ContainsKey(readerInterface) && dictionaryVTIsPinging.ContainsKey(readerInterface))
        //    {
        //        if ((DateTime.Now.Ticks - dictionaryLastConnectionCheckingTime[readerInterface]) / TimeSpan.TicksPerMillisecond > readerCheckTimeOut)
        //        {
        //            VTIsPinging = dictionaryVTIsPinging[readerInterface];
        //            int numOfTyries = 0;
        //            bool getRegime = false;
        //            while (numOfTyries < 3 && !getRegime)
        //            {

        //                dictionaryLastConnectionCheckingTime[readerInterface] = DateTime.Now.Ticks;
        //                RFIDDevice rfid = new RFIDDevice();
        //                try
        //                {
        //                    int status = rfid.Open(reader.ConnectionAddress); // if (status == 0) greska;
        //                    if (status == 0)
        //                    {
        //                        if (VTIsPinging)
        //                        {
        //                            WriteLogDebugMessage("Reader connection lost! " + reader.ConnectionAddress, Constants.basicDebugLevel, -1);
        //                            setStatus(antALL, "Reader connection lost!");
        //                            VTIsPinging = false;
        //                            readerConnected = false;

        //                        }
        //                    }
        //                    else
        //                    {
        //                        byte regime;

        //                        status = rfid.GetRegime(&regime);   // if (status == 0) greska; 
        //                        rfid.Close();
        //                        if (status == 0)
        //                        {
        //                            //SetManagerStatus(statusNOTOK);
        //                            if (VTIsPinging)
        //                            {
        //                                WriteLogDebugMessage("Reader status reading failed!" + reader.ConnectionAddress, Constants.basicDebugLevel, -1);
        //                                setStatus(antALL, "Reader status reading failed!");
        //                                VTIsPinging = false;
        //                                readerConnected = false;

        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (!VTIsPinging)
        //                            {

        //                                rfid.Reset();
        //                                WriteLogDebugMessage("Reset reader " + reader.ConnectionAddress, Constants.basicDebugLevel, -1);
        //                                Thread.Sleep(5000);
        //                                rfid.Close();
        //                                status = rfid.Open(reader.ConnectionAddress);
        //                                //rfid.GetRegime(&regime);
        //                                WriteLogDebugMessage("Status reader " + status + " " + reader.ConnectionAddress, Constants.basicDebugLevel, -1);

        //                                if (status != 0)
        //                                {
        //                                    WriteLogDebugMessage("Reader connection established! " + reader.ConnectionAddress, Constants.basicDebugLevel, -1);
        //                                    setStatus(antALL, "Reader connection established!");
        //                                    VTIsPinging = true;
        //                                    readerInterface.Close();
        //                                    if (readerInterface.Open(reader.ConnectionAddress + "11001")) readerConnected = true;

        //                                }
        //                            }
        //                        }
        //                    }
        //                    rfid.Close();
        //                }
        //                catch (Exception ex)
        //                {
        //                    if (VTIsPinging)
        //                    {
        //                        WriteLogDebugMessage("Reader connection checking failed with exception: " + ex.Message, Constants.basicDebugLevel, -1);
        //                        setStatus(antALL, "Reader connection checking failed!");
        //                        VTIsPinging = false;
        //                        readerConnected = false;

        //                    }
        //                }
        //                finally
        //                {
        //                    getRegime = VTIsPinging;
        //                    dictionaryVTIsPinging[readerInterface] = VTIsPinging;
        //                    if (!VTIsPinging)
        //                        SetManagerStatus(statusNOTOK);
        //                    numOfTyries++;
        //                    WriteLogDebugMessage("getRegime = " + getRegime.ToString() + "Num of tries = " + numOfTyries.ToString(), 3, -1);
        //                }
        //            }
        //        }
        //    }
        //    return VTIsPinging;
        //}

        private void Work()
        {
            try
            {
                while (true)
                {
                    if (!stopMonitoring)
                    {
                        if (GetUsedDataTime())
                            getUsedData();

                        if (readerConnected)
                        {
                            try
                            {
                                //CHECK HERE I AM MESSAGE
                                if ((DateTime.Now.Ticks - lastConnectionCheckingTime) / TimeSpan.TicksPerMillisecond > 6 * 60 * 1000)
                                {
                                    WriteLog("+++Reader: " + reader.ConnectionAddress + " not connected, no message", Constants.basicDebugLevel, -1);
                                    setStatus(antALL, " Reader not connected! No message");
                                    readerConnected = false;
                                    lastConnectionCheckingTime = DateTime.Now.Ticks;
                                    continue;
                                }

                                byte[] logRecord = ReadThrownLogRecord(readerInterface);

                                bool isValid = true;

                                //TAG ID
                                uint serNo = logRecord[3] * (uint)Math.Pow(2, 24) + logRecord[2] * (uint)Math.Pow(2, 16) + logRecord[1] * (uint)Math.Pow(2, 8) + logRecord[0];
                                if (serNo == 0) { isValid = false; }

                                //ANTENNA (4)
                                int antenna = (int)logRecord[4];
                                if ((antenna != 0) && (antenna != 1)) isValid = false;

                                bool entrancePermitted = (logRecord[5] == 4);

                                //EVENT TIME
                                int seconds = logRecord[6];
                                int minutes = logRecord[7];
                                int hours = logRecord[8];
                                int day = logRecord[9];
                                int month = logRecord[10] & 0x0F;
                                int year = logRecord[11];
                                if ((seconds > 59) || (minutes > 59) || (hours > 23) ||
                                    (day < 1) || (day > 31) || (month < 1) || (month > 12) || (year > 99)) isValid = false;
                                year += 2000;
                                DateTime eventTime = new DateTime(year, month, day, hours, minutes, seconds);
                                

                                lastConnectionCheckingTime = DateTime.Now.Ticks;

                                //TAKE AND SAVE A SNAPSHOT IF MESSAGE IS VALID
                                if (isValid)
                                {
                                    WriteLogDebugMessage("Monitor >  card serial number: " + serNo.ToString() + ", reader: " + reader.ConnectionAddress, Constants.basicDebugLevel, antenna);
                                    try
                                    {
                                        string direction = "";
                                        if (antenna == antOut)
                                            direction = reader.A1Direction;
                                        else
                                            direction = reader.A0Direction;

                                        setStatus(antenna, "Tag ID: " + serNo);

                                        //FOR EACH CAMERA OF THAT READER
                                        if (dictionaryCameras != null && dictionaryCameras.Count > 0)
                                        {
                                            //FOR DIRECTION IN OR OUT
                                            if (dictionaryCameras.ContainsKey(direction))
                                            {
                                                foreach (CameraTO camera in dictionaryCameras[direction])
                                                {
                                                    takeCameraSnapshot(camera.ConnAddress, reader.Description, camera, eventTime);
                                                }
                                            }
                                            //AND FOR DIRECTION INOUT
                                            if (dictionaryCameras.ContainsKey(inOut))
                                            {
                                                foreach (CameraTO camera in dictionaryCameras[inOut])
                                                {
                                                    takeCameraSnapshot(camera.ConnAddress, reader.Description, camera, eventTime);
                                                }

                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        WriteLogDebugMessage("Error while taking snapshot: " + ex.Message + ", reader: " + reader.ConnectionAddress, Constants.basicDebugLevel, -1);
                                    }

                                    //UPDATE EMPLOYEE LOCATION 
                                    if (dictionaryEmployees.ContainsKey(serNo))
                                    {
                                        writeToDatabase(dictionaryEmployees[serNo].EmployeeID, reader, antenna, eventTime);
                                    }

                                }
                                else
                                {
                                    WriteLogDebugMessage("Invalid message, reader: " + reader.ConnectionAddress, Constants.basicDebugLevel, -1);
                                }


                            }
                            catch (Exception ex)
                            {
                                checkDBConnection();
                            }
                            Thread.Sleep(100);

                        }
                        else
                        {
                            try
                            {
                                readerInterface.Close();
                            }
                            catch { WriteLogDebugMessage("Reader connection closing error. ", Constants.basicDebugLevel, -1); }

                            try
                            {
                                if (readerInterface.Open(reader.ConnectionAddress + ":" + readerPort))
                                {
                                    if (readerInterface.isOpen())
                                    {
                                        WriteLogDebugMessage("Reader: " + reader.ConnectionAddress + " connected", Constants.basicDebugLevel, -1);
                                        readerConnected = true;
                                        lastConnectionCheckingTime = DateTime.Now.Ticks;
                                        setStatus(antALL, " Reader connected!"); 
                                    }
                                }
                             }
                            catch (Exception ex)
                            {
                                readerConnected = false;
                                setStatus(antALL, " Reader not connected!");

                                WriteLogDebugMessage("Reader connecting error: " + ex.Message,Constants.basicDebugLevel,-1);
                            }
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        return;
                    }
                }

            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " The thread " + Thread.CurrentThread.Name + " has exited with exception: " +
                    ex.Message + "\n\n" + ex.StackTrace + "\n\n");
            }
        }

        public void writeToDatabase(int employeeID, ReaderTO reader, int antennaNum, DateTime eventTime)
        {
            EmployeeLocation emplLocation = new EmployeeLocation();
            try
            {
                bool updated = false;

                updated = emplLocation.Update(employeeID, reader.ReaderID, antennaNum, 0, eventTime, 0);

            }
            catch (Exception ex)
            {
                if (ex.Message.Trim().Equals("2627"))
                {
                    WriteLogDebugMessage(ex.Message + 2627, Constants.basicDebugLevel, antennaNum);
                }
                else
                {
                    WriteLogDebugMessage(ex.Message, Constants.basicDebugLevel, antennaNum);
                }
                Thread.Sleep(1000);
                throw ex;
            }
        }
        private byte[] ReadThrownLogRecord(IReaderInterface readerInterface)
        {
            int messageLength = 0;
            if (readerInterface.GetTechnologyType().ToUpper() == "MIFARE")
            {
                messageLength = 16;
            }
            else if (readerInterface.GetTechnologyType().ToUpper() == "HITAG1")
            {
                messageLength = 19;     // ? - check, depends on firmware
            }
            else if (readerInterface.GetTechnologyType().ToUpper() == "HITAG2")
            {
                messageLength = 19;
            }
            else if (readerInterface.GetTechnologyType().ToUpper() == "ICODE")
            {
                messageLength = 20;     // ? - check, depends on firmware
            }

            const byte STX = 0x7E;
            const byte ETX = 0x7F;
            const byte ESC = 0x7D;

            byte[] resultBytes = new byte[messageLength];
            resultBytes[0] = resultBytes[1] = resultBytes[2] = resultBytes[3] = 0;

            byte[] portContent = new byte[30];

            byte[] buffer = new byte[30];
            byte dataByte = 0;

            uint n = 0;

            int counter = 0;

            try
            {
                bool wasEsc = false;


                n = readerInterface.Ready();
                if (n == 0) return resultBytes;

                if (readerInterface.Recv(buffer, 30) > 0)
                {
                    portContent = buffer;
                }


                if (portContent[0] == STX)
                {
                    for (int i = 1; i < portContent.Length; i++)
                    {
                        if (portContent[i] == ETX)
                        {
                            break;
                        }
                        if (portContent[i] == ESC)
                        {
                            wasEsc = true;
                        }
                        else
                        {
                            if (wasEsc)
                            {
                                resultBytes[counter] = (byte)(portContent[i] ^ 0x20);
                            }
                            else
                            {
                                resultBytes[counter] = portContent[i];
                            }
                            wasEsc = false;
                            counter++;
                        }
                    }
                }
                if (counter == messageLength - 2)
                {
                    Console.WriteLine("\nSuccess\n");
                    for (int i = 0; i < counter; i++)
                    {

                        Console.Write(resultBytes[i].ToString("X2") + " ");
                    }
                    Console.WriteLine("\n");
                }
                else
                {
                    Console.WriteLine("\nFailure\n");
                    resultBytes = new byte[messageLength];
                    resultBytes[0] = resultBytes[1] = resultBytes[2] = resultBytes[3] = 0;

                }

            }
            catch (Exception ex)
            {
                WriteLogDebugMessage("Read error:" + ex.Message, Constants.basicDebugLevel, -1);
                //log.writeLog(ex);
                throw ex;
            }

            return resultBytes;
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


        public void getUsedData()
        {
            try
            {
                WriteLogDebugMessage("getUsedData, start", Constants.basicDebugLevel, -1);
                dbConnectionData = DBConnectionManager.Instance.MakeNewDBConnection();
                Tag tag = new Tag(dbConnection);
                tag.TgTO.Status = Constants.statusActive;

                tagsActive = tag.SearchActive();
                string tags = "";
                foreach (KeyValuePair<ulong, TagTO> pair in tagsActive)
                {
                    TagTO tagTO = pair.Value;
                    tags += tagTO.TagID + ", ";

                }
                if (tags.Length > 0)
                {
                    tags = tags.Substring(0, tags.Length - 2);
                }
                dictionaryEmployees = new Employee(dbConnection).SearchByTagsDictionary(tags);
                dictionaryCameras = new Dictionary<string, List<CameraTO>>();

                CamerasXReaders cam = new CamerasXReaders(dbConnection);
                cam.ReaderID = reader.ReaderID;
                ArrayList listCamXReaders = cam.Search(-1, reader.ReaderID, "");

                ArrayList listCameras = new Camera(dbConnection).Search(-1, "", "", "");
                Dictionary<int, CameraTO> dictionaryCam = new Dictionary<int, CameraTO>();
                foreach (Camera camTO in listCameras)
                {

                    CameraTO cameraTO = camTO.SendTransferObject();
                    if (!dictionaryCam.ContainsKey(cameraTO.CameraID))
                        dictionaryCam.Add(cameraTO.CameraID, cameraTO);
                }
                foreach (CamerasXReaders camXReader in listCamXReaders)
                {
                    CamerasXReadersTO camXReaderTO = camXReader.SendTransferObject();

                    if (dictionaryCam.ContainsKey(camXReaderTO.CameraID))
                    {
                        if (!dictionaryCameras.ContainsKey(camXReaderTO.DirectionCovered))
                        {
                            dictionaryCameras.Add(camXReaderTO.DirectionCovered, new List<CameraTO>());
                            dictionaryCameras[camXReaderTO.DirectionCovered].Add(dictionaryCam[camXReaderTO.CameraID]);

                        }
                        else
                        {
                            dictionaryCameras[camXReaderTO.DirectionCovered].Add(dictionaryCam[camXReaderTO.CameraID]);

                        }
                    }

                }

                DBConnectionManager.Instance.CloseDBConnection(dbConnectionData);
                WriteLogDebugMessage("getUsedData, end", Constants.basicDebugLevel, -1);
            }
            catch (Exception ex)
            {

                WriteLog("Exception in getusesData: " + ex.Message, Constants.basicDebugLevel, -1);

            }

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
                //WriteLogDebugMessage("HTTPWEBREQ", Constants.basicDebugLevel, -1);
                req.Credentials = new NetworkCredential(Constants.CammeraUser, Constants.CammeraPass);
                //req.Timeout = 1000;
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

        private void takeCameraSnapshot(string IP, string reader, CameraTO camTO, DateTime eventTime)
        {
            try
            {

                int total = 0;
                byte[] buffer = GetSnapshot(IP, out total);


                // get bitmap
                Bitmap bmp = (Bitmap)Bitmap.FromStream(new MemoryStream(buffer, 0, total));
                string camDesc = "";

                if (camTO.CameraID < 100)
                {
                    if (camTO.CameraID < 10)
                        camDesc = "C_00" + camTO.CameraID + "_";
                    else
                        camDesc = "C_0" + camTO.CameraID + "_";
                }
                else
                {
                    camDesc = "C_" + camTO.CameraID.ToString() + "_";
                }
                //string location = Constants.SnapshotsFilePath + camDesc + DateTime.Now.ToString("yy-MM-dd_HH-mm-ss") + "-01.jpg";
                string location = Constants.SnapshotsFilePath + camDesc + eventTime.ToString("yy-MM-dd_HH-mm-ss") + "-01.jpg";
                bmp.Save(location, ImageFormat.Jpeg);
                WriteLogDebugMessage("save snapshop", Constants.basicDebugLevel, -1);
            }
            catch (Exception ex)
            {
                WriteLog("Camera: " + IP + "; snapshots failed." + ex.Message, Constants.basicDebugLevel, -1);
                throw ex;
            }
        }



        //public void SetTicketID(string text)
        //{
        //    lock (locker)
        //    {
        //        try
        //        {
        //            currentAntenna.tagID = uint.Parse(text);
        //            currentAntenna.employeeID = -1;
        //            currentAntenna.mealTypeID = -1;
        //            currentAntenna.transID = -1;
        //            tag_id = currentAntenna.tagID;


        //        }
        //        catch { }
        //    }
        //}

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

        //public string GetValidationTerminalID()
        //{
        //    lock (locker)
        //    {
        //        return validationTerminal.ConnectionAddress.ToString();
        //    }
        //}

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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal IP: " + reader.ConnectionAddress + "; antenna: " + antenna + "; " + message.Trim(), level);
                logForAll.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal ID: " + reader.ConnectionAddress + "; antenna: " + antenna + "; " + message.Trim(), level);
            }
            else
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal IP: " + reader.ConnectionAddress + "; " + message.Trim(), level);
                logForAll.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal ID: " + reader.ConnectionAddress + "; " + message.Trim(), level);
            }
        }
        public void WriteLogDebugMessage(string message, int level, int antenna)
        {
            if (antenna != -1)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal IP: " + reader.ConnectionAddress + "; antenna: " + antenna + "; " + message.Trim(), level);
                logForAll.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal ID: " + reader.ConnectionAddress + "; antenna: " + antenna + "; " + message.Trim(), level);
            }
            else
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal IP: " + reader.ConnectionAddress + "; " + message.Trim(), level);
                logForAll.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal ID: " + reader.ConnectionAddress + "; " + message.Trim(), level);
            }
        }
        public void WriteLogDebugNewLine()
        {
            log.writeLog("\n", Constants.detailedDebugLevel);
            logForAll.writeLog("\n", Constants.detailedDebugLevel);
        }
        //public static ACTAReaderMonitorManager GetInstance()
        //{
        //    try
        //    {
        //        if (managerInstance == null)
        //        {
        //            managerInstance = new ACTAReaderMonitorManager(gateTo);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return managerInstance;
        //}
        public string GetValidationTerminalID()
        {
            lock (locker)
            {
                return reader.ConnectionAddress;
            }
        }


    }
}

