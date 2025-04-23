using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ReaderInterface;
using TransferObjects;
using System.Collections;
using Util;
using Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using DataAccess;
using System.Configuration;

namespace ACTASurveillanceManagement
{
    public class SurveillanceManager
    {
        private Thread worker;
        private bool stopMonitoring;

        private List<ReaderTO> readers = new List<ReaderTO>();
        private List<bool> pingStatuses = new List<bool>();
        private List<long> lastPingTimes = new List<long>();
        private List<IReaderInterface> readerInterfaces = new List<IReaderInterface>();
        private double pingInterval = 20000;    // in ms

        private DebugLog log = null;

        public string workStatus = "";

        public CardOwnerTO currentOwner = null;
        

        private Dictionary<int, Dictionary<int, List<CameraTO>>> readerCameras = new Dictionary<int, Dictionary<int, List<CameraTO>>>();

        private static SurveillanceManager managerInstance;

        uint lastTag = 0;

        //for thread controlling
        private object locker = new object();

        DAOFactory daoFactory = null;
        bool DBconnected = false;
        const string noDBConnectionString = "Can not connect to the database!";

        //last DB check time in ticks
        private long checkDBTime = 0;
        private long checkDBTimeOut = 10000;  // in ms

        string updateEmployeeLocation = "";

        private List<EmployeeTO> employees = new List<EmployeeTO>();
        private List<TagTO> tags = new List<TagTO>();
        private Dictionary<ulong, EmployeeTO> employeeTag = new Dictionary<ulong, EmployeeTO>();

        //ping objects
        System.Net.NetworkInformation.Ping pingSender;
        System.Net.NetworkInformation.PingReply rep;

        public static SurveillanceManager GetInstance()
        {
            try
            {
                if (managerInstance == null)
                {
                    managerInstance = new SurveillanceManager();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return managerInstance;
        }


        public  SurveillanceManager()
        {

            NotificationController.SetApplicationName("ACTASurveillanceService");
            DAOFactory.threadSafe = true;
           log = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");
           try
           {
               updateEmployeeLocation = ConfigurationManager.AppSettings["UpdateEmployeeLocation"];


               daoFactory = DAOFactory.getDAOFactory();
              
               if (daoFactory.TestDataSourceConnection())
               {
                   readDB();
                   DBconnected = true;
                   WriteLog("DB connection established! ");

               }
               else
               {
                   try
                   {
                       daoFactory.CloseConnection();
                   }
                   catch { }
                   daoFactory = null;
               }
               
           }
           catch (Exception ex)
           {
               log.writeLog(ex);
               DBconnected = false;
               setStatus(noDBConnectionString);
           }
          
        }

        private void readDB()
        {
            try
            {
                Reader r = new Reader();
                r.RdrTO.Status = "ENABLED";
                readers = r.Search();
                employees = new Employee().Search();
                tags = new Tag().Search();
                LoadCameras();               
                CreateReadersInterfaces();

                foreach (TagTO tag in this.tags)
                {
                    if ((tag.Status == Constants.statusActive || tag.Status == Constants.statusBlocked))
                    {
                        foreach (EmployeeTO empl in this.employees)
                        {
                            if (tag.OwnerID == empl.EmployeeID)
                            {
                                if (!employeeTag.ContainsKey(tag.TagID))
                                {
                                    employeeTag.Add(tag.TagID, empl);
                                }
                            }
                        }
                    }
                }

                DBconnected = true;
            }
            catch (Exception ex)
            {
                try
                {
                    daoFactory.CloseConnection();
                }
                catch { }
                log.writeLog(ex);
                DBconnected = false;
                daoFactory = null;
                setStatus(noDBConnectionString);
            }
        }

        private void checkDB()
        {
            try
            {
              
                employees = new Employee().Search();
                tags = new Tag().Search();
                

                foreach (TagTO tag in this.tags)
                {
                    if ((tag.Status == Constants.statusActive || tag.Status == Constants.statusBlocked))
                    {
                        foreach (EmployeeTO empl in this.employees)
                        {
                            if (tag.OwnerID == empl.EmployeeID)
                            {
                                if (!employeeTag.ContainsKey(tag.TagID))
                                {
                                    employeeTag.Add(tag.TagID, empl);
                                }
                            }
                        }
                    }
                }


            }
            catch 
            {
                try
                {
                    daoFactory.CloseConnection();
                }
                catch { }
                WriteLog(noDBConnectionString);
                DBconnected = false;
                daoFactory = null;
                setStatus(noDBConnectionString);
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
                            lock (locker)
                            {
                                daoFactory = DAOFactory.getDAOFactory();
                                if (daoFactory.TestDataSourceConnection())
                                {
                                    readDB();
                                    if (DBconnected)
                                        WriteLog(" ACTASurveillanceManagement.tryToConnect() connection to database established.");
                                }
                                else
                                {
                                    try
                                    {
                                        daoFactory.CloseConnection();
                                    }
                                    catch { }
                                    daoFactory = null;
                                }
                            }
                           
                        }
                        catch (Exception ex) { WriteLog(" ACTASurveillanceManagement.tryToConnect(): " + ex.Message); }
                    }
                    else
                    {
                        DBconnected = daoFactory.TestDataSourceConnection();
                        if (DBconnected)
                        {
                            WriteLog(" ACTASurveillanceManagement.tryToConnect() connection to database established.");
                        }
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
                WriteLog(" ACTASurveillanceManagement.tryToConnect(): " + ex.Message);
            }
        }

        private void LoadCameras()
        {
            try
            {
                foreach (ReaderTO rto in readers)
                {
                    if (!readerCameras.ContainsKey(rto.ReaderID))
                    {
                        readerCameras.Add(rto.ReaderID, new Dictionary<int, List<CameraTO>>());
                        ArrayList list =  new Camera().SearchForReaders(rto.ReaderID.ToString(),"%"+ rto.A0Direction+"%");
                        List<CameraTO> ipAdresses0 = new List<CameraTO>();
                        foreach(Camera camera in list)
                        {
                            ipAdresses0.Add(camera.SendTransferObject());
                        }
                        list = new Camera().SearchForReaders(rto.ReaderID.ToString(), rto.A1Direction);
                        List<CameraTO> ipAdresses1 = new List<CameraTO>();
                        foreach (Camera camera in list)
                        {
                            ipAdresses1.Add(camera.SendTransferObject());
                        }
                        readerCameras[rto.ReaderID].Add(0, ipAdresses0);
                        readerCameras[rto.ReaderID].Add(1, ipAdresses1);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(ex);
            }
        }

        public void Start()
        {
            try
            {
                lock (locker)
                {
                    pingSender = new System.Net.NetworkInformation.Ping();
                }
                 OpenReadersConnections();
                 
                stopMonitoring = false;
                WriteLog(" ACTA Surveillance Manager started!");
                setStatus(" ACTA Surveillance Manager started!");
                worker = new Thread(new ThreadStart(Work));
                worker.Name = "ACTASurveillanceManager";
                worker.Start();
            }
            catch (Exception ex)
            {
                log.writeLog(ex);
            }
        }

        public void Stop()
        {
            try
            {
                WriteLog(" ACTA Surveillance Manager is ending!");

                setStatus(" ACTA Surveillance Manager is ending!");
                stopMonitoring = true;
                //pingSender.Dispose();
                if (worker != null)
                {
                    worker.Join(); // Wait for the worker's thread to finish.
                }

                WriteLog(" ACTA Surveillance Manager stopped! ");
                setStatus(" ACTA Surveillance Manager stopped! ");
            }
            catch (Exception ex)
            {
                log.writeLog(ex);
            }
        }

        private bool OpenReadersConnections()
        {
           
            bool anyOpened = false;
            foreach (IReaderInterface readerInterface in this.readerInterfaces)
            {
                try
                {
                    Reader r = new Reader();
                    r.RdrTO = readers[readerInterfaces.IndexOf(readerInterface)];
                    if(lastPingTimes.Count <= readerInterfaces.IndexOf(readerInterface))
                        lastPingTimes.Add(0);
                    if(pingStatuses.Count <= readerInterfaces.IndexOf(readerInterface))
                    {
                        pingStatuses.Add(false);
                        pingStatuses[readerInterfaces.IndexOf(readerInterface)]=PingSuccess(readerInterfaces.IndexOf(readerInterface));
                    }
                    //if (readerInterface.Open(r.RdrTO.ConnectionAddress+":11001")) opened = true;

                    //if (opened)
                    //{
                        anyOpened = anyOpened || pingStatuses[readerInterfaces.IndexOf(readerInterface)];
                    //    readerInterface.Flush();
                    //}
                    //else
                    //{
                    //    log.writeLog(DateTime.Now + " Error opening reader " + readers[readerInterfaces.IndexOf(readerInterface)].ReaderID.ToString() + " connection!");
                    //    setStatus("Error opening reader " + readers[readerInterfaces.IndexOf(readerInterface)].ReaderID.ToString() + " connection!");
                    //}
                }
                catch (Exception ex)
                {
                    log.writeLog(ex);
                    setStatus("Error opening reader " + readers[readerInterfaces.IndexOf(readerInterface)].ReaderID.ToString() + " connection!");
                }
            }
            return anyOpened;
        }

        private bool CreateReadersInterfaces()
        {
            bool allCreated = true;
           
            readerInterfaces = new List<IReaderInterface>();
            foreach (ReaderTO reader in this.readers)
            {
                try
                {
                    ReaderFactory.TechnologyType = reader.TechType;
                    IReaderInterface readerInterface = ReaderFactory.GetReader;
                    readerInterfaces.Add(readerInterface);
                }
                catch (Exception ex)
                {
                    log.writeLog(ex);
                    setStatus("Error creating reader " +
                        reader.ReaderID.ToString() + " interface!");
                    allCreated = false;
                }
            }
            return allCreated;
        }

        public void setStatus( string status)
        {
            lock (locker)
            {
                workStatus = status;
            }
        }

        public void setOwner(CardOwnerTO owner)
        {
            lock (locker)
            {
                currentOwner = owner;
            }
        }

      

        public string getStatus()
        {
            lock (locker)
            {
                return workStatus;
            }
        }

        public CardOwnerTO getOwner()
        {
            lock (locker)
            {
                return currentOwner;
            }
        }

       private bool PingSuccess(int index)
        {
            if ((DateTime.Now.Ticks - lastPingTimes[index]) / TimeSpan.TicksPerMillisecond > pingInterval)
            {
                try
                {
                    lock (locker)
                    {
                        bool opened = false;
                        lastPingTimes[index] = DateTime.Now.Ticks;
                        byte[] pingBytes = { 0x41, 0x42, 0x43, 0x44 };
                        rep = pingSender.Send(readers[index].ConnectionAddress, 2000, pingBytes);

                        if (rep.Status == System.Net.NetworkInformation.IPStatus.Success)
                        {
                            setStatus("Reader ID: " + readers[index].ReaderID.ToString() + "; address = " + readers[index].ConnectionAddress + "; ping success");
                            if (!pingStatuses[index])
                            {
                                WriteLog("Ping success");

                                pingStatuses[index] = true;


                            }
                            bool readerOpened = false;
                            try
                            {
                                readerOpened = readerInterfaces[index].isOpen();
                            }
                            catch { }
                            if (!readerOpened)
                            {
                                if (readerInterfaces[index].Open(readers[index].ConnectionAddress + ":11001")) opened = true;

                                if (opened)
                                {
                                    readerInterfaces[index].Flush();
                                    setStatus("Reader ID: " + readers[index].ReaderID.ToString() + "; address = " + readers[index].ConnectionAddress + "; connected");
                                    WriteLog("Reader ID: " + readers[index].ReaderID.ToString() + "; address = " + readers[index].ConnectionAddress + "; connected");
                                }
                                else
                                {
                                    WriteLog(" Error opening reader " + readers[index].ReaderID.ToString() + " connection!");
                                    setStatus("Error opening reader " + readers[index].ReaderID.ToString() + " connection!");
                                }
                            }
                        }
                        else
                        {
                            setStatus("Reader ID: " + readers[index].ReaderID.ToString() + "; address = " + readers[index].ConnectionAddress + "; ping faild");
                            if (pingStatuses[index])
                            {
                                pingStatuses[index] = false;
                                WriteLog("Ping failed");
                                if (readerInterfaces[index].isOpen())
                                    try
                                    {
                                        readerInterfaces[index].Close();
                                    }
                                    catch (Exception ex) { log.writeLog(ex); }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    pingStatuses[index] = false;

                    WriteLog("TicketControlManager.PingSuccess()" + ex.Message);
                }
            }
            return pingStatuses[index];
        }

        private void Work()
        {
            try
            {
                while (!stopMonitoring)
                {
                    if (DBconnected)
                    {
                        if (IsTimeCheckDB())
                        {
                            checkDB();
                        }
                        // iterate through the readers
                        foreach (IReaderInterface readerInterface in readerInterfaces)
                        {
                            if (PingSuccess(readerInterfaces.IndexOf(readerInterface)))
                            {
                                if (!readerInterface.isOpen()) continue;
                                ReaderTO reader = readers[readerInterfaces.IndexOf(readerInterface)];

                                try
                                {
                                    byte[] logRecord = ReadThrownLogRecord(readerInterface);

                                    bool isValid = true;

                                    uint serNo = logRecord[3] * (uint)Math.Pow(2, 24) + logRecord[2] * (uint)Math.Pow(2, 16) + logRecord[1] * (uint)Math.Pow(2, 8) + logRecord[0];
                                    if (serNo == 0) isValid = false;
                                    if (serNo == lastTag)
                                        isValid = false;
                                    lastTag = serNo;
                                    int antenna = (int)logRecord[4];
                                    if ((antenna != 0) && (antenna != 1)) isValid = false;

                                    bool entrancePermitted = (logRecord[5] == 4);

                                    int seconds = logRecord[6];
                                    int minutes = logRecord[7];
                                    int hours = logRecord[8];
                                    int day = logRecord[9];
                                    int month = logRecord[10] & 0x0F;
                                    int year = logRecord[11];
                                    if ((seconds > 59) || (minutes > 59) || (hours > 23) ||
                                        (day < 1) || (day > 31) || (month < 1) || (month > 12) || (year > 99)) isValid = false;
                                    year += 2000;

                                    DateTime eventTime = new DateTime();
                                    try
                                    {
                                        eventTime = new DateTime(year, month, day, hours, minutes, seconds);
                                    }
                                    catch { isValid = false; }

                                    if (isValid)
                                    {
                                        CardOwnerTO owner = new CardOwnerTO(reader, serNo, antenna, entrancePermitted, eventTime);
                                        if (employeeTag.ContainsKey(serNo))
                                        {
                                            owner.employee = employeeTag[serNo];
                                            setOwner(owner);
                                        }
                                        WriteLog(" Reader id = " + reader.ReaderID.ToString() + "; Antenna = " + antenna.ToString() + "; Tag ID = " + serNo.ToString() + "; Enterance permitted = " + entrancePermitted.ToString());
                                        if (entrancePermitted)
                                        {
                                            if (readerCameras != null && readerCameras.Count > 0 && readerCameras[reader.ReaderID][antenna].Count > 0)
                                            {
                                                foreach (CameraTO camera in readerCameras[reader.ReaderID][antenna])
                                                {

                                                    takeCameraSnapshot(camera, reader.ReaderID.ToString(), serNo, eventTime);
                                                }
                                            }
                                            if (employeeTag.ContainsKey(serNo))
                                            {
                                                int employeeID = owner.employee.EmployeeID;
                                                int readerID = owner.reader.ReaderID;
                                                bool updated = true;

                                                try
                                                {
                                                    updated = (new EmployeeLocation()).Update(employeeID, readerID, antenna, 0, eventTime, 0);
                                                }
                                                catch (Exception ex)
                                                {
                                                    log.writeLog(ex);
                                                    updated = false;
                                                }
                                                if (updated)
                                                {
                                                    setStatus("Card owner " + employeeID.ToString() + " location updated.");
                                                }
                                                else
                                                {
                                                    WriteLog( " CardOwnerLocationUpdater: " + "Enqueue task for card owner " + employeeID.ToString() + ".\n");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        setOwner(null);
                                    }
                                }
                                catch (Exception ex) { log.writeLog(ex); }
                            }
                            
                        }
                        
                    }
                    else
                    {
                        try
                        {
                            daoFactory.CloseConnection();
                        }
                        catch { }
                        daoFactory = null;
                        setStatus("No DB conntection!");
                        if (IsTimeCheckDB())
                        {
                            try
                            {
                                tryToConnect();
                                checkDBTime = DateTime.Now.Ticks;
                            }
                            catch
                            {
                                setStatus(noDBConnectionString);
                            }
                        }
                    }
                    Thread.Sleep(100);
                 
                }
            }
            catch (Exception ex)
            {
                WriteLog( " The thread " + Thread.CurrentThread.Name + " has exited with exception: " +
                    ex.Message + "\n\n" + ex.StackTrace + "\n\n");
            }
        }

        private bool IsTimeCheckDB()
        {
            if ((DateTime.Now.Ticks - checkDBTime) / TimeSpan.TicksPerMillisecond > checkDBTimeOut)
            {
                checkDBTime = DateTime.Now.Ticks;
                return true;
            }
            return false;
        }

        private void takeCameraSnapshot(CameraTO  cameraTO, string reader, uint tagID, DateTime eventTime)
        {
            try
            {

                int total = 0;
                Snapshooting snp = Snapshooting.getSnapshootingClass(cameraTO.Type);
                byte[] buffer = snp.GetSnapshot(cameraTO.ConnAddress, out total,log);
                byte[] newBuffer = new byte[total];
                for (int i = 0; i < total; i++)
                {
                    newBuffer[i] = buffer[i];
                }
               
                CameraSnapshotFileTO file = new CameraSnapshotFileTO();
                file.CameraID = cameraTO.CameraID;
                file.Content = buffer;
                file.TagID = tagID;
                file.EventTime = eventTime;
                file.FileName = reader + "_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss_fff");
               
                if (new CameraSnapshotFile().Save(file, true) <= 0)
                {
                    WriteLog(DateTime.Now + " Camera: " + cameraTO.ConnAddress + " snapshots faild." );
                }

            }
            catch (Exception ex)
            {
                WriteLog(DateTime.Now + " Camera: " + cameraTO.ConnAddress + " snapshots faild." + ex.Message);
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

            byte[] portContent = new byte[1024];

            byte[] buffer = new byte[1];
            byte dataByte = 0;

            uint n = 0;

            int counter = 0;

            try
            {
                n = readerInterface.Ready();
                if (n == 0) return resultBytes;

                for (int i = 0; i < n; i++)
                {
                    readerInterface.Recv(buffer, 1); dataByte = buffer[0];
                    if (dataByte == STX) break;
                }

                bool wasEsc = false;

                if (dataByte == STX)
                {
                    counter = 0;
                    DateTime t0 = DateTime.Now;
                    while ((dataByte != ETX) && (counter < (messageLength - 1)))
                    {

                        if ((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > 1000)
                        {
                            Console.WriteLine("\nTime out!\n");
                            break;
                        }

                        if (readerInterface.Recv(buffer, 1) > 0)
                        {
                            dataByte = buffer[0];
                        }
                        else
                        {
                            continue;
                        }

                        if (dataByte == ESC)
                        {
                            wasEsc = true;
                        }
                        else
                        {
                            if (wasEsc)
                            {
                                portContent[counter] = (byte)(dataByte ^ 0x20);
                            }
                            else
                            {
                                portContent[counter] = dataByte;
                            }
                            wasEsc = false;
                            counter++;
                        }
                    }
                    if ((counter == (messageLength - 1)) && (dataByte == ETX))
                    {
                        Console.WriteLine("\nSuccess\n");
                        for (int i = 0; i < counter; i++)
                        {
                            resultBytes[i] = portContent[i];
                            Console.Write(resultBytes[i].ToString("X2") + " ");
                        }
                        Console.WriteLine("\n");
                    }
                    else
                    {
                        // for testing purpose
                        Console.WriteLine("\nFailure\n");
                        for (int i = 0; i < counter; i++)
                        {
                            Console.Write(portContent[i].ToString("X2") + " ");
                        }
                        Console.WriteLine("\n");

                        readerInterface.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(ex);
                throw ex;
            }

            return resultBytes;
        }

        public void WriteLog(string message)
        {
            log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " " + message.Trim());
        }
    }
}
