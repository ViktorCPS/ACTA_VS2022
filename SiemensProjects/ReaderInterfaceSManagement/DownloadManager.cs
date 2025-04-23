using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections;
using System.Configuration;

//comment siemens
//using ADVANTAGELib;
//using ADVCOMAPILib; 

using Util;
using Common;
using SiemensDataAccess;
using TransferObjects;

namespace ReaderInterfaceSManagement
{
    public class DownloadManager : IDisposable
    {
        private static DownloadManager managerInstance;

        private Thread downloadManagerThread;
        private Thread synchronizationManagerThread;

        private BrezaDAO brezaDAO = null;
        private BrezaDAO brezaReadingDAO = null;
        private SiemensDAO siemensDAO = null;
        private SiemensDAO siemensSyncDAO = null;
        private SiemensReportDAO siemensReportDAO = null;
        private SiemensReportDAO siemensReportSyncDAO = null;
        private object brezaConn = null;
        private object brezaReadingConn = null;
        private object siemensConn = null;
        private object siemensSyncConn = null;
        private object siemensReportConn = null;
        private object siemensReportSyncConn = null;
        
        //comment siemens
        //private IAdvSession Session = null;
        
        // Debug
        DebugLog debug;
        private volatile bool isReading = false;
        private volatile bool isSynchronizing = false;

        //sync period
        private int period = 5 * 60000;// in ms
        private long lastSyncTime = 0;

        //transaction start time in ticks
        private DateTime nextSyncTime = Constants.SyncStartTime;
        private DateTime nextMonthlyCheckTime = Constants.MonthlyCheckStartTime;
        
        private long syncTimeout = Constants.SyncTimeout;  // in min        
        private long monthlyCheckTimeout = Constants.MonthlyCheckTimeout;  // in min

        // SiPass connection parameters
        private string SiPassServer = Constants.SiPassServer;
        private string SiPassUsername = Constants.SiPassUsername;
        private string SiPassPassword = Constants.SiPassPassword;

        Dictionary<int, SiemensEmployeeTO> employeeDict = new Dictionary<int, SiemensEmployeeTO>(); // key is SiPass employee ID, value is employee
        Dictionary<int, SiemensTruckLogTO> trucksDict = new Dictionary<int, SiemensTruckLogTO>(); // key is SiPass visitor (truck or truck driver) employee ID, value is truck or truck driver record for IT_kamioni record creation
        List<string> notRegStatesList = new List<string>();
        List<string> regStatesList = new List<string>();

        protected DownloadManager()
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";            
            debug = new DebugLog(logFilePath);

            if (ConfigurationManager.AppSettings["Period"] != null)
                period = int.Parse(ConfigurationManager.AppSettings["Period"]) * 60000;
          
            downloadManagerThread = new Thread(new ThreadStart(Reading));
            isReading = false;
            isSynchronizing = false;
        }

        public static DownloadManager GetInstance()
        {
            try
            {
                if (managerInstance == null)
                {
                    managerInstance = new DownloadManager();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return managerInstance;
        }

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            this.downloadManagerThread = null;
            this.synchronizationManagerThread = null;
            GC.Collect();
        }

        private void Reading()
        {
            try
            {
                while (isReading)
                {
                    DateTime passSyncTime = DateTime.Now;
                    bool isMidnight = (passSyncTime.Hour == 23 && passSyncTime.Minute == 59);

                    bool checkMonthly = isMonthlyCheckTime();

                    if (((passSyncTime.Ticks - lastSyncTime) / TimeSpan.TicksPerMillisecond > period) || isMidnight || checkMonthly)
                    {
                        lastSyncTime = passSyncTime.Ticks;
                        if (checkMonthly)
                            debug.writeLog(DateTime.Now + "Reading(): Monthly check started!");
                        bool downloadSucceeded = this.downloadNewLogs(checkMonthly);
                        if (!downloadSucceeded)
                        {
                            debug.writeLog(DateTime.Now + "Reading(): Download new passes failed");
                        }
                        else if (checkMonthly)
                        {
                            debug.writeLog(DateTime.Now + "Reading(): Monthly check finished!");
                            nextMonthlyCheckTime = nextMonthlyCheckTime.AddMinutes(monthlyCheckTimeout);
                        }
                    }

                    Thread.Sleep(1500);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(ex);
                AbortReading();
            }
        }

        private void Synchronization()
        {
            try
            {
                while (isSynchronizing)
                {
                    if (isSyncTime())
                    {
                        debug.writeLog(DateTime.Now + " SYNCHRONIZATION STARTED.");

                        Synchronize();

                        debug.writeLog(DateTime.Now + " SYNCHRONIZATION COMPLETED.");

                        nextSyncTime = nextSyncTime.AddMinutes(syncTimeout);
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(ex);
                AbortSynchronization();                
            }
        }

        /// <summary>
        /// Getting new logs from Siemens database and write it to Log.xml
        /// </summary>
        private bool downloadNewLogs(bool checkMonthly)
        {
            bool Succeeded = true;
            try
            {
                if (!File.Exists(Constants.SiPassMappingPath))
                {
                    debug.writeLog(DateTime.Now + " ReadingNewLog(): mapping path not found. \n");
                    return false;
                }

                bool brezaConnected = ConnectReadingBreza();
                bool simConnected = ConnectSiemens();
                bool reportConnected = ConnectSiemensReport();
                if (brezaConnected && simConnected && reportConnected)
                {
                    try
                    {
                        //get point names which are for work time count
                        ArrayList pointList = siemensDAO.deserializeMapping(Constants.SiPassMappingPath);

                        // get truck points
                        ArrayList truckPointList = new ArrayList();
                        if (File.Exists(Constants.SiPassTruckMappingPath))
                            truckPointList = siemensDAO.deserializeMapping(Constants.SiPassTruckMappingPath);

                        debug.writeLog(DateTime.Now + " ReadingNewLog(): mapping.xml reading finished. \n");
                        if (pointList.Count > 0 || truckPointList.Count > 0)
                        {
                            //get last proccess log ID
                            int diffLogID = -1;

                            try
                            {
                                diffLogID = siemensDAO.getDiffLogID(Constants.SiPassDiffLogPath);
                                debug.writeLog(DateTime.Now + " ReadingNewLog(): diffLogpointer.xml reading succed. \n");
                            }
                            catch (Exception ex)
                            {
                                debug.writeLog(DateTime.Now + " ReadingNewLog(): diffLogpointer.xml reading FAILED. Exception:" + ex.Message + " \n");
                                Succeeded = false;
                            }

                            if (!Succeeded)
                            {
                                DisconnectReadingConnections();
                                return false;
                            }

                            // make truck points dictionary
                            Dictionary<int, PointTO> truckPointsDict = new Dictionary<int, PointTO>();
                            foreach (PointTO point in truckPointList)
                            {
                                if (!truckPointsDict.ContainsKey(point.PointID))
                                    truckPointsDict.Add(point.PointID, point);
                            }

                            //make string from points of interest
                            string pointIDAllLogs = "";
                            Dictionary<int, PointTO> pointsDict = new Dictionary<int, PointTO>();                            
                            foreach (PointTO point in pointList)
                            {
                                if (point.ReadPasses == Constants.SiemensPointPassReading || truckPointsDict.ContainsKey(point.PointID))
                                {
                                    if (!pointsDict.ContainsKey(point.PointID))
                                        pointsDict.Add(point.PointID, point);
                                    
                                    if (pointIDAllLogs.Equals(""))
                                        pointIDAllLogs += "'" + point.PointID.ToString().Trim() + "'";
                                    else
                                        pointIDAllLogs += ", '" + point.PointID.ToString().Trim() + "'";
                                }
                            }

                            if (pointIDAllLogs.Equals(""))
                            {
                                DisconnectReadingConnections();
                                return false;
                            }

                            // get maximal date of passes transfered
                            DateTime maxDatePassTransfered = brezaReadingDAO.getMaxDatePassTransfered();
                            DateTime maxDateTruckTransfered = brezaReadingDAO.getMaxDateTruckPassTransfered();
                            DateTime maxDateTransfered = new DateTime();
                            if (maxDatePassTransfered.Date > maxDateTruckTransfered.Date)
                                maxDateTransfered = maxDatePassTransfered.Date;
                            else
                                maxDateTransfered = maxDateTruckTransfered.Date;

                            if (checkMonthly && maxDateTransfered.Date > DateTime.Now.Date.AddMonths(-1))
                                maxDateTransfered = DateTime.Now.Date.AddMonths(-1);

                            List<SiemensLogTO> newLogsAll = new List<SiemensLogTO>();
                            if (maxDateTransfered != new DateTime() && maxDateTransfered.Date != DateTime.Now.Date)
                            {
                                DateTime currDate = maxDateTransfered.Date;
                                while (currDate.Date < DateTime.Now.Date)
                                {
                                    // read from file
                                    newLogsAll.AddRange(ReadLogsFromFile(currDate.Date, pointsDict, truckPointsDict));
                                    currDate = currDate.Date.AddDays(1);
                                }

                                if (!checkMonthly)
                                {
                                    diffLogID = -1;

                                    try
                                    {
                                        siemensDAO.setDiffLogID(diffLogID, Constants.SiPassDiffLogPath);
                                        debug.writeLog(DateTime.Now + " ReadingNewLog(): diffLogpointer.xml (new day beggining) writing succeded. \n");
                                    }
                                    catch (Exception ex)
                                    {
                                        debug.writeLog(DateTime.Now + " ReadingNewLog(): diffLogpointer.xml (new day beggining) writing FAILED. Exception:" + ex.Message + " \n");
                                        Succeeded = false;
                                    }

                                    if (!Succeeded)
                                    {
                                        DisconnectReadingConnections();
                                        return false;
                                    }
                                }
                            }

                            int lastLogID = diffLogID;

                            List<SiemensLogTO> auditTrailViewLogs = new List<SiemensLogTO>();

                            if (!checkMonthly)
                                auditTrailViewLogs = siemensDAO.getNewLogs(pointIDAllLogs, diffLogID, pointsDict, truckPointsDict);

                            // check audit trail view records, if there are records from previous days, check if they already are took from files
                            foreach (SiemensLogTO logTO in auditTrailViewLogs)
                            {
                                bool found = false;

                                if (logTO.RegTime.Date != DateTime.Now.Date)
                                {
                                    foreach (SiemensLogTO newPass in newLogsAll)
                                    {
                                        if (logTO.Direction.Trim() == newPass.Direction.Trim() && logTO.LastName.Trim() == newPass.LastName.Trim() && logTO.Name.Trim() == newPass.Name.Trim()
                                            && logTO.RegLoc == newPass.RegLoc && logTO.RegTime == newPass.RegTime && logTO.TypeID.Trim() == newPass.TypeID.Trim()
                                            && (logTO.Id.Trim() == newPass.Id.Trim() || (logTO.TypeID == Constants.SiemensVisitorType && logTO.Id.Trim() == "")))
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                }

                                if (!found)
                                    newLogsAll.Add(logTO);
                            }

                            if (newLogsAll.Count == 0)
                            {
                                debug.writeLog(DateTime.Now + " ReadingNewLog(): No new logs from last reading \n");
                                DisconnectReadingConnections();
                                return true;
                            }

                            List<SiemensLogTO> newLogs = new List<SiemensLogTO>();
                            List<SiemensTruckLogTO> newTruckLogs = new List<SiemensTruckLogTO>();
                            List<SiemensVechicleLogTO> newVechicleLogs = new List<SiemensVechicleLogTO>();

                            DateTime fromDate = new DateTime();
                            DateTime toDate = new DateTime();
                            
                            bool checkTrucks = false;
                            foreach (SiemensLogTO pTO in newLogsAll)
                            {
                                if (pTO.RegTime.Date != DateTime.Now.Date)
                                {
                                    if (fromDate == new DateTime() || fromDate.Date > pTO.RegTime.Date)
                                        fromDate = pTO.RegTime.Date;

                                    if (toDate == new DateTime() || toDate.Date < pTO.RegTime.Date)
                                        toDate = pTO.RegTime.Date;
                                }

                                if (pTO.TypeID == Constants.SiemensVisitorType)
                                {
                                    if (employeeDict.ContainsKey(pTO.EmplID))
                                        pTO.Id = employeeDict[pTO.EmplID].JMBG.Trim();
                                    else
                                        pTO.Id = "";
                                }

                                if (pTO.TruckCandidate)
                                    checkTrucks = true;
                            }

                            // do the check for previous days records which records are alredy transfered
                            // get all passes transfered previous days
                            List<SiemensLogTO> passesTransfered = new List<SiemensLogTO>();
                            List<SiemensTruckLogTO> trucksTransfered = new List<SiemensTruckLogTO>();
                            List<SiemensVechicleLogTO> vechiclesTransfered = new List<SiemensVechicleLogTO>();

                            if (fromDate != new DateTime() || toDate != new DateTime())
                            {
                                passesTransfered = brezaReadingDAO.getPasses("", "", "", "", fromDate.Date, toDate.Date);

                                if (checkTrucks)
                                    trucksTransfered = brezaReadingDAO.getTruckPasses("", "", "", "", "", "", "", "", fromDate.Date, toDate.Date);

                                vechiclesTransfered = siemensReportDAO.getVechiclePasses(fromDate.Date, toDate.Date);
                            }

                            int i = 0;
                            SiemensLogTO passTO = null;
                            SiemensTruckLogTO truckPassTO = null;
                            SiemensVechicleLogTO vechiclePassTO = null;
                            while (i < newLogsAll.Count)
                            {
                                passTO = null;
                                truckPassTO = null;
                                vechiclePassTO = null;
                                
                                // if record is on vechicle gate, check if this and next records are vechicle registrations records
                                if (Constants.VechiclePoints.Contains(newLogsAll[i].PointID))
                                {
                                    if (i + 1 < newLogsAll.Count && Constants.VechiclePoints.Contains(newLogsAll[i + 1].PointID)                                            
                                        && employeeDict.ContainsKey(newLogsAll[i].EmplID) && employeeDict.ContainsKey(newLogsAll[i + 1].EmplID)
                                        && ((isVechicleType(employeeDict[newLogsAll[i].EmplID].CardType) && isDriver(employeeDict[newLogsAll[i + 1].EmplID]))
                                        || (isVechicleType(employeeDict[newLogsAll[i + 1].EmplID].CardType) && isDriver(employeeDict[newLogsAll[i].EmplID])))
                                        && newLogsAll[i].Direction == newLogsAll[i + 1].Direction && newLogsAll[i].PointID == newLogsAll[i + 1].PointID && newLogsAll[i].RegTime == newLogsAll[i + 1].RegTime
                                        && newLogsAll[i].LogID == (newLogsAll[i + 1].LogID - 1))
                                    {
                                        vechiclePassTO = new SiemensVechicleLogTO();
                                        // create vechicle record
                                        if (isVechicleType(employeeDict[newLogsAll[i].EmplID].CardType))
                                        {
                                            vechiclePassTO.DriverID = newLogsAll[i + 1].EmplID;
                                            vechiclePassTO.VechicleID = newLogsAll[i].EmplID;
                                            vechiclePassTO.PointID = newLogsAll[i].PointID;
                                            vechiclePassTO.EventTime = newLogsAll[i].RegTime;
                                            vechiclePassTO.Direction = newLogsAll[i].Direction;
                                        }
                                        else
                                        {
                                            vechiclePassTO.DriverID = newLogsAll[i].EmplID;
                                            vechiclePassTO.VechicleID = newLogsAll[i + 1].EmplID;
                                            vechiclePassTO.PointID = newLogsAll[i].PointID;
                                            vechiclePassTO.EventTime = newLogsAll[i].RegTime;
                                            vechiclePassTO.Direction = newLogsAll[i].Direction;
                                        }
                                    }

                                    if (vechiclePassTO != null)
                                    {
                                        bool logTransfered = false;

                                        if (vechiclePassTO.EventTime.Date != DateTime.Now.Date)
                                        {
                                            foreach (SiemensVechicleLogTO trPass in vechiclesTransfered)
                                            {
                                                if (vechiclePassTO.DriverID == trPass.DriverID && vechiclePassTO.VechicleID == trPass.VechicleID && vechiclePassTO.PointID == trPass.PointID
                                                    && vechiclePassTO.EventTime == trPass.EventTime && vechiclePassTO.Direction.Trim() == trPass.Direction.Trim())
                                                {
                                                    logTransfered = true;
                                                    break;
                                                }
                                            }
                                        }

                                        if (!logTransfered)
                                            newVechicleLogs.Add(vechiclePassTO);
                                    }
                                }

                                if (newLogsAll[i].TruckCandidate)
                                {
                                    // check if next two records are truck records
                                    if (i + 1 < newLogsAll.Count && newLogsAll[i + 1].TruckCandidate
                                        && trucksDict.ContainsKey(newLogsAll[i].EmplID) && trucksDict.ContainsKey(newLogsAll[i + 1].EmplID)
                                        && ((trucksDict[newLogsAll[i].EmplID].Type == Constants.SiemensVisitorTypeTruck && trucksDict[newLogsAll[i + 1].EmplID].Type == Constants.SiemensVisitorTypeTruckDriver)
                                        || (trucksDict[newLogsAll[i].EmplID].Type == Constants.SiemensVisitorTypeTruckDriver && trucksDict[newLogsAll[i + 1].EmplID].Type == Constants.SiemensVisitorTypeTruck))
                                        && newLogsAll[i].Direction == newLogsAll[i + 1].Direction && newLogsAll[i].RegLoc == newLogsAll[i + 1].RegLoc && newLogsAll[i].RegTime == newLogsAll[i + 1].RegTime
                                        && newLogsAll[i].LogID == (newLogsAll[i + 1].LogID - 1))
                                    {
                                        truckPassTO = new SiemensTruckLogTO();
                                        // create truck record
                                        if (trucksDict[newLogsAll[i].EmplID].Type == Constants.SiemensVisitorTypeTruck)
                                        {
                                            truckPassTO.BuyerName = trucksDict[newLogsAll[i].EmplID].BuyerName;
                                            truckPassTO.DriverFirstName = trucksDict[newLogsAll[i + 1].EmplID].DriverFirstName;
                                            truckPassTO.DriverLastName = trucksDict[newLogsAll[i + 1].EmplID].DriverLastName;
                                            truckPassTO.ForwarderName = trucksDict[newLogsAll[i].EmplID].ForwarderName;
                                            truckPassTO.OrderForm = trucksDict[newLogsAll[i].EmplID].OrderForm;
                                            truckPassTO.TruckRegistration = trucksDict[newLogsAll[i].EmplID].TruckRegistration;
                                            truckPassTO.TruckID = newLogsAll[i].EmplID;
                                            truckPassTO.DriverID = newLogsAll[i + 1].EmplID;
                                            if (truckPointsDict.ContainsKey(newLogsAll[i].RegLoc))
                                                truckPassTO.RegistrationLocation = truckPointsDict[newLogsAll[i].RegLoc].PointName.ToString();
                                            truckPassTO.RegistrationTime = newLogsAll[i].RegTime;
                                            truckPassTO.RegistrationType = newLogsAll[i].Direction;
                                        }
                                        else
                                        {
                                            truckPassTO.BuyerName = trucksDict[newLogsAll[i + 1].EmplID].BuyerName;
                                            truckPassTO.DriverFirstName = trucksDict[newLogsAll[i].EmplID].DriverFirstName;
                                            truckPassTO.DriverLastName = trucksDict[newLogsAll[i].EmplID].DriverLastName;
                                            truckPassTO.ForwarderName = trucksDict[newLogsAll[i + 1].EmplID].ForwarderName;
                                            truckPassTO.OrderForm = trucksDict[newLogsAll[i + 1].EmplID].OrderForm;
                                            truckPassTO.TruckRegistration = trucksDict[newLogsAll[i + 1].EmplID].TruckRegistration;
                                            truckPassTO.TruckID = newLogsAll[i + 1].EmplID;
                                            truckPassTO.DriverID = newLogsAll[i].EmplID;
                                            if (truckPointsDict.ContainsKey(newLogsAll[i].RegLoc))
                                                truckPassTO.RegistrationLocation = truckPointsDict[newLogsAll[i].RegLoc].PointName.ToString();
                                            truckPassTO.RegistrationTime = newLogsAll[i].RegTime;
                                            truckPassTO.RegistrationType = newLogsAll[i].Direction;
                                        }

                                        truckPassTO.LogID = newLogsAll[i + 1].LogID;
                                        truckPassTO.FromFile = newLogsAll[i + 1].FromFile;
                                    }
                                    else
                                    {
                                        i++;
                                        continue;
                                    }
                                }
                                else
                                    passTO = newLogsAll[i];

                                bool transfered = false;
                                if (passTO != null)
                                {
                                    if (passTO.RegTime.Date != DateTime.Now.Date)
                                    {
                                        foreach (SiemensLogTO trPass in passesTransfered)
                                        {
                                            if (passTO.Direction.Trim() == trPass.Direction.Trim() && passTO.LastName.Trim() == trPass.LastName.Trim() && passTO.Name.Trim() == trPass.Name.Trim()
                                                && passTO.RegLoc == trPass.RegLoc && passTO.RegTime == trPass.RegTime && passTO.TypeID.Trim() == trPass.TypeID.Trim() 
                                                && (passTO.Id.Trim() == trPass.Id.Trim() || (passTO.TypeID == Constants.SiemensVisitorType && passTO.Id.Trim() == "")))
                                            {
                                                transfered = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (!transfered)
                                        newLogs.Add(passTO);
                                }
                                else if (truckPassTO != null)
                                {
                                    if (truckPassTO.RegistrationTime.Date != DateTime.Now.Date)
                                    {
                                        foreach (SiemensTruckLogTO trPass in trucksTransfered)
                                        {
                                            if (truckPassTO.BuyerName.Trim() == trPass.BuyerName.Trim() && truckPassTO.DriverFirstName.Trim() == trPass.DriverFirstName.Trim()
                                                && truckPassTO.DriverLastName.Trim() == trPass.DriverLastName.Trim() && truckPassTO.ForwarderName.Trim() == trPass.ForwarderName.Trim()
                                                && truckPassTO.OrderForm.Trim() == trPass.OrderForm.Trim() && truckPassTO.RegistrationLocation.Trim() == trPass.RegistrationLocation.Trim()
                                                && truckPassTO.RegistrationTime == trPass.RegistrationTime && truckPassTO.RegistrationType.Trim() == trPass.RegistrationType.Trim()
                                                && truckPassTO.TruckRegistration.Trim() == trPass.TruckRegistration.Trim())
                                            {
                                                transfered = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (!transfered)
                                        newTruckLogs.Add(truckPassTO);
                                }

                                if (truckPassTO != null)
                                    i += 2;
                                else
                                    i++;
                            }

                            if (newLogs.Count == 0 && newTruckLogs.Count == 0 && newVechicleLogs.Count == 0)
                            {
                                debug.writeLog(DateTime.Now + " ReadingNewLog(): No new logs from last reading \n");
                                DisconnectReadingConnections();
                                return true;
                            }

                            bool saved = true;
                            bool brezaTrans = brezaReadingDAO.beginTransaction();
                            bool reportTrans = siemensReportDAO.beginTransaction();
                            if (brezaTrans && reportTrans)
                            {
                                try
                                {
                                    foreach (SiemensLogTO l in newLogs)
                                    {
                                        if (!saved)
                                            break;

                                        if (!l.FromFile && l.LogID > lastLogID)
                                            lastLogID = l.LogID;
                                        l.LogID = Constants.SiemensLogID;
                                        uint PK = brezaReadingDAO.insert(l, false);
                                        saved = saved && PK > 0 && siemensReportDAO.insertRegAddInfo(PK, l.EmplID, false) > 0;
                                    }

                                    foreach (SiemensTruckLogTO l in newTruckLogs)
                                    {
                                        if (!saved)
                                            break;

                                        if (!l.FromFile && l.LogID > lastLogID)
                                            lastLogID = l.LogID;
                                        l.LogID = Constants.SiemensLogID;
                                        int id = brezaReadingDAO.insertTruckLog(l, false);
                                        saved = saved && id > 0 && siemensReportDAO.insertKamioniAddInfo(id, l.TruckID, l.DriverID, false) > 0;
                                    }

                                    foreach (SiemensVechicleLogTO l in newVechicleLogs)
                                    {
                                        if (!saved)
                                            break;

                                        saved = saved && siemensReportDAO.insertVechicleLog(l, false) > 0;
                                    }

                                    if (saved)
                                    {
                                        brezaReadingDAO.commitTransaction();
                                        siemensReportDAO.commitTransaction();
                                    }
                                    else
                                    {
                                        brezaReadingDAO.rollbackTransaction();
                                        siemensReportDAO.rollbackTransaction();
                                        DisconnectReadingConnections();
                                        return false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    brezaReadingDAO.rollbackTransaction();
                                    siemensReportDAO.rollbackTransaction();
                                    DisconnectReadingConnections();
                                    debug.writeLog(DateTime.Now + " ReadingNewLog(): Exception in registrations saving: " + ex.Message.Trim() + " \n");
                                    return false;
                                }
                            }
                            else
                            {
                                if (brezaTrans)
                                    brezaReadingDAO.commitTransaction();

                                if (reportTrans)
                                    siemensReportDAO.commitTransaction();
                            }
                            
                            try
                            {
                                saved = siemensDAO.setDiffLogID(lastLogID, Constants.SiPassDiffLogPath);
                                debug.writeLog(DateTime.Now + " ReadingNewLog(): diffLogpointer.xml writing succeded. \n");
                            }
                            catch (Exception ex)
                            {
                                debug.writeLog(DateTime.Now + " ReadingNewLog(): diffLogpointer.xml writing FAILED. Exception:" + ex.Message + " \n");
                                saved = false;
                            }
                            if (!saved)
                            {
                                DisconnectReadingConnections();
                                return false;
                            }
                        }
                        else
                        {
                            debug.writeLog(DateTime.Now + " ReadingNewLog(): No point is mark as time attendance counter. \n");
                            DisconnectReadingConnections();
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        DisconnectReadingConnections();
                        throw ex;
                    }

                    DisconnectReadingConnections();
                }
                else
                {
                    debug.writeLog(DateTime.Now + " ReadingNewLog(): No connection. Breza: " + brezaConnected.ToString() + " Simmens: " + simConnected.ToString() + " Report: " + reportConnected.ToString() + " \n");
                    if (brezaConnected)
                        DisconnectReadingBreza();
                    if (simConnected)
                        DisconnectSiemens();
                    if (reportConnected)
                        DisconnectSiemensReport();

                    return false;
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " ReadingNewLog(): " + ex.Message + "\n");
                Succeeded = false;
            }

            return Succeeded;
        }

        private List<SiemensLogTO> ReadLogsFromFile(DateTime date, Dictionary<int, PointTO> pointsDict, Dictionary<int, PointTO> truckPointDict)
        {
            try
            {
                List<SiemensLogTO> logList = new List<SiemensLogTO>();
                string filePath = Constants.SiemensAuditTrailPath + date.Date.ToString("yyyyMMdd") + "000.TAB";
                if (System.IO.File.Exists(filePath))
                {
                    string fileNewPath = "";
                    try
                    {
                        System.IO.FileStream strTest = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        strTest.Dispose();
                        strTest.Close();
                    }
                    catch
                    {
                        fileNewPath = Constants.SiemensAuditTrailPath + date.Date.ToString("yyyyMMdd") + "000Copy.TAB";
                        File.Copy(filePath, fileNewPath, true);
                        filePath = fileNewPath;
                    }

                    //debug.writeLog(" filePath: " + filePath.Trim() + "\n");

                    System.IO.FileStream str = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    System.IO.StreamReader reader = new System.IO.StreamReader(str, Encoding.Default);
                    
                    // read file lines                        
                    string line = "";

                    // skip header
                    reader.ReadLine();

                    // get first record line
                    line = reader.ReadLine();
                    while (line != null)
                    {
                        // file header:
                        //0     1       2       3           4       5           6               7               8               9               10          11      12          13          14          15          16
                        //at_id remote  unit_no point_no    type    point_name  date_occurred   time_occurred   date_recorded   time_recorded   category    message state_id    last_name   first_name  workgroup   card_no
                        //17    18      19      20      21      22      23      24              25      26      27      28                      29                      30      31          32              33          34
                        //udf1  udf2    udf3    udf4    udf5    udf6    udf7    last_updated    pt_id   buss    at_type date_occurred_server    time_occurred_server    fln_no  device_no   card_facility   card_tech   new_area
                        //35        36      37      38              39              40                  41          42      43
                        //old_area  db_type db_id   bHasAlarmColors lDisplayColor   lBackgroundColor    utc_offset  opg_id  emp_id
                        string[] log = line.Split('\t');

                        if (log.Length > 43)
                        {
                            int pointID = -1;
                            if (!int.TryParse(log[25].Trim(), out pointID))
                                pointID = -1;

                            int emplID = -1;
                            if (!int.TryParse(log[43].ToString().Trim(), out emplID))
                                emplID = -1;

                            // skip not registration records
                            if (!notRegStatesList.Contains(log[12].Trim()) && regStatesList.Contains(log[12].Trim()) && pointID != -1 && pointsDict.ContainsKey(pointID)
                                && log[4].Trim() == Constants.SiemensType.ToString().Trim() && emplID != -1 && employeeDict.ContainsKey(emplID))
                            {
                                SiemensLogTO logTO = new SiemensLogTO();

                                // create log from line
                                int id = -1;
                                if (int.TryParse(log[0].ToString().Trim(), out id))
                                    logTO.LogID = id;
                                if (employeeDict[emplID].Visitor)
                                {
                                    logTO.TypeID = Constants.SiemensVisitorType;

                                    // ID should be JMBG
                                    logTO.Id = employeeDict[emplID].JMBG.Trim();
                                }
                                else
                                {
                                    logTO.TypeID = Constants.SiemensEmployeeType;

                                    // ID should be employee number
                                    logTO.Id = employeeDict[emplID].ID.Trim();
                                }

                                logTO.EmplID = emplID;                                
                                if (log[6].Trim() != "" && log[7].Trim() != "")
                                {
                                    string datePart = log[6].Trim();
                                    string timePart = log[7].Trim();
                                    int year = int.Parse(datePart.Substring(0, 4));
                                    int month = int.Parse(datePart.Substring(4, 2));
                                    int day = int.Parse(datePart.Substring(6));
                                    int hour = int.Parse(timePart.Substring(0, 2));
                                    int minute = int.Parse(timePart.Substring(2, 2));
                                    int sec = int.Parse(timePart.Substring(4));
                                    logTO.RegTime = new DateTime(year, month, day, hour, minute, sec);
                                }
                                //if (truckPointDict.ContainsKey(pointNames[log[5].Trim()].PointID))
                                //{
                                //    logTO.RegLoc = pointNames[log[5].Trim()].PointID;
                                //    logTO.TruckCandidate = true;
                                //}
                                //else
                                //{
                                //    logTO.RegLoc = pointNames[log[5].Trim()].Gate;
                                //    logTO.TruckCandidate = false;
                                //}
                                //if (pointNames[log[5].Trim()].Direction == Constants.SiemensDirectionIn)
                                //    logTO.Direction = Constants.SiemensRegDirectionIn;
                                //else
                                //    logTO.Direction = Constants.SiemensRegDirectionOut;
                                if (truckPointDict.ContainsKey(pointID))
                                {
                                    logTO.RegLoc = pointID;
                                    logTO.TruckCandidate = true;
                                }
                                else
                                {
                                    logTO.RegLoc = pointsDict[pointID].Gate;
                                    logTO.TruckCandidate = false;
                                }
                                logTO.PointID = pointID;
                                if (pointsDict[pointID].Direction == Constants.SiemensDirectionIn)
                                    logTO.Direction = Constants.SiemensRegDirectionIn;
                                else
                                    logTO.Direction = Constants.SiemensRegDirectionOut;
                                logTO.Name = log[14].Trim();
                                logTO.LastName = log[13].Trim();
                                logTO.ReadStatus = Constants.SiemensDefaultReadStatus;
                                logTO.Message = log[11].Trim();
                                logTO.FromFile = true;

                                logList.Add(logTO);
                            }
                        }

                        line = reader.ReadLine();
                    }

                    reader.Close();
                    str.Dispose();
                    str.Close();

                    if (fileNewPath.Trim() != "" && File.Exists(fileNewPath))
                        File.Delete(fileNewPath);
                }

                return logList;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " ReadLogsFromFile(): " + ex.Message + "\n");
                throw ex;
            }
        }

        /// <summary>
        /// Synchronize Breza and SiPass
        /// </summary>
        private void Synchronize()
        {
            try
            {
                bool brezaConnected = ConnectBreza();
                bool siPassConnected = ConnectSiPass();
                bool simConnected = ConnectSiemensSync();
                if (brezaConnected && siPassConnected && simConnected)
                {
                    try
                    {
                        /*comment siemens*/
                        //debug.writeLog(DateTime.Now + " Synchronization departments started.");
                        //SynchronizeDepartments();
                        //debug.writeLog(DateTime.Now + " Synchronization departments completed.");

                        //debug.writeLog(DateTime.Now + " Synchronization employees started.");
                        //SynchronizeEmployees();
                        //debug.writeLog(DateTime.Now + " Synchronization employees completed.");

                        //debug.writeLog(DateTime.Now + " Refreshing employee data started.");
                        //GetEmployeeData();
                        //debug.writeLog(DateTime.Now + " Refreshing employee data completed.");

                        //debug.writeLog(DateTime.Now + " Synchronization employees images started.");
                        //SynchronizeEmployeesImages();
                        //debug.writeLog(DateTime.Now + " Synchronization employees images completed.");

                        //debug.writeLog(DateTime.Now + " Synchronization employees cards started.");
                        //SynchronizeEmployeesCards();
                        //debug.writeLog(DateTime.Now + " Synchronization employees cards completed.");
                        /*comment siemens*/
                    }
                    catch (Exception ex)
                    {
                        DisconnectBreza();
                        DisconnectSiPass();
                        DisconnectSiemensSync();
                        throw ex;
                    }

                    DisconnectBreza();
                    DisconnectSiPass();
                    DisconnectSiemensSync();
                }
                else
                {
                    debug.writeLog(DateTime.Now + " Connection failed. \n");
                    if (brezaConnected)
                        DisconnectBreza();
                    if (siPassConnected)
                        DisconnectSiPass();
                    if (simConnected)
                        DisconnectSiemensSync();
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + "SYNCHRONIZATION ERROR: Synchronize(): " + ex.Message + "\n");                
            }
        }

        private bool ConnectSiPass()
        {
            bool logged = false;

            try
            {
                /*comment siemens*/
                //Session = new AdvSession();
                //Session.ServerName = SiPassServer;
                //Session.Connect();
                //Session.Logon(SiPassUsername, SiPassPassword, 0, 0);
                //logged = true;
                /*comment siemens*/
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " ConnectSiPass(): " + ex.Message + "\n");
                logged = false;
            }

            return logged;
        }

        private bool ConnectBreza()
        {
            bool logged = false;
            try
            {
                if (brezaDAO == null)
                    brezaDAO = BrezaDAO.getDAO();

                if (brezaDAO == null)
                    return false;

                brezaConn = brezaDAO.MakeNewDBConnection();

                if (brezaConn == null)
                    return false;

                brezaDAO.SetDBConnection(brezaConn);

                logged = true;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " ConnectBreza(): " + ex.Message + "\n");
                logged = false;
            }

            return logged;
        }

        private bool ConnectReadingBreza()
        {
            bool logged = false;
            try
            {
                if (brezaReadingDAO == null)
                    brezaReadingDAO = BrezaDAO.getDAO();

                if (brezaReadingDAO == null)
                    return false;

                brezaReadingConn = brezaReadingDAO.MakeNewDBConnection();

                if (brezaReadingConn == null)
                    return false;

                brezaReadingDAO.SetDBConnection(brezaReadingConn);

                logged = true;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " ConnectReadingBreza(): " + ex.Message + "\n");
                logged = false;
            }

            return logged;
        }

        private bool ConnectSiemens()
        {
            bool logged = false;
            try
            {
                if (siemensDAO == null)
                    siemensDAO = SiemensDAO.getDAO();

                if (siemensDAO == null)
                    return false;

                siemensConn = siemensDAO.MakeNewDBConnection();

                if (siemensConn == null)
                    return false;

                siemensDAO.SetDBConnection(siemensConn);

                logged = true;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " ConnectSiemens(): " + ex.Message + "\n");
                logged = false;
            }

            return logged;
        }

        private bool ConnectSiemensSync()
        {
            bool logged = false;
            try
            {
                if (siemensSyncDAO == null)
                    siemensSyncDAO = SiemensDAO.getDAO();

                if (siemensSyncDAO == null)
                    return false;

                siemensSyncConn = siemensSyncDAO.MakeNewDBConnection();

                if (siemensSyncConn == null)
                    return false;

                siemensSyncDAO.SetDBConnection(siemensSyncConn);

                logged = true;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " ConnectSiemensSync(): " + ex.Message + "\n");
                logged = false;
            }

            return logged;
        }

        private bool ConnectSiemensReport()
        {
            bool logged = false;
            try
            {
                if (siemensReportDAO == null)
                    siemensReportDAO = SiemensReportDAO.getDAO();

                if (siemensReportDAO == null)
                    return false;

                siemensReportConn = siemensReportDAO.MakeNewDBConnection();

                if (siemensReportConn == null)
                    return false;

                siemensReportDAO.SetDBConnection(siemensReportConn);

                logged = true;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " ConnectSiemensReport(): " + ex.Message + "\n");
                logged = false;
            }

            return logged;
        }

        private bool ConnectSiemensReportSync()
        {
            bool logged = false;
            try
            {
                if (siemensReportSyncDAO == null)
                    siemensReportSyncDAO = SiemensReportDAO.getDAO();

                if (siemensReportSyncDAO == null)
                    return false;

                siemensReportSyncConn = siemensReportSyncDAO.MakeNewDBConnection();

                if (siemensReportSyncConn == null)
                    return false;

                siemensReportSyncDAO.SetDBConnection(siemensReportSyncConn);

                logged = true;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " ConnectSiemensReportSync(): " + ex.Message + "\n");
                logged = false;
            }

            return logged;
        }

        private void DisconnectSiPass()
        {
            try
            {
                /*comment siemens*/
                //Session.Logoff();
                //Session.Disconnect();
                /*comment siemens*/
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " DisconnectSiPass(): " + ex.Message + "\n");
            }
        }

        private void DisconnectBreza()
        {
            try
            {
                if (brezaDAO != null)
                    brezaDAO.CloseConnection(brezaConn);                    
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " DisconnectBreza(): " + ex.Message + "\n");
            }
        }

        private void DisconnectReadingBreza()
        {
            try
            {
                if (brezaReadingDAO != null)
                    brezaReadingDAO.CloseConnection(brezaReadingConn);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " DisconnectReadingBreza(): " + ex.Message + "\n");
            }
        }

        private void DisconnectSiemens()
        {
            try
            {
                if (siemensDAO != null)
                    siemensDAO.CloseConnection(siemensConn);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " DisconnectSiemens(): " + ex.Message + "\n");
            }
        }

        private void DisconnectSiemensSync()
        {
            try
            {
                if (siemensSyncDAO != null)
                    siemensSyncDAO.CloseConnection(siemensSyncConn);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " DisconnectSiemensSync(): " + ex.Message + "\n");
            }
        }

        private void DisconnectSiemensReport()
        {
            try
            {
                if (siemensReportDAO != null)
                    siemensReportDAO.CloseConnection(siemensReportConn);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " DisconnectSiemensReport(): " + ex.Message + "\n");
            }
        }

        private void DisconnectSiemensReportSync()
        {
            try
            {
                if (siemensReportSyncDAO != null)
                    siemensReportSyncDAO.CloseConnection(siemensReportSyncConn);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " DisconnectSiemensReport(): " + ex.Message + "\n");
            }
        }

        private void DisconnectReadingConnections()
        {
            try
            {
                DisconnectReadingBreza();
                DisconnectSiemens();
                DisconnectSiemensReport();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " DisconnectReadingConnections(): " + ex.Message + "\n");
            }
        }

        /*comment siemens*/
        //private void SynchronizeEmployees()
        //{
        //    try
        //    {
        //        // get all employees for synchronization
        //        List<SiemensEmployeeTO> syncEmployees = brezaDAO.getEmployeesUnprocessed();

        //        // get all departments from xml
        //        ArrayList departmentsList = brezaDAO.deserializeDepartments(Constants.SiPassDepartmentMappingPath);

        //        Dictionary<int, int> brezaDepartmentsDict = new Dictionary<int, int>();
        //        foreach (object member in departmentsList)
        //        {
        //            if (member is SiemensDepartmentTO)
        //            {
        //                if (!brezaDepartmentsDict.ContainsKey(((SiemensDepartmentTO)member).Odid))
        //                    brezaDepartmentsDict.Add(((SiemensDepartmentTO)member).Odid, ((SiemensDepartmentTO)member).SiPassID);
        //            }
        //        }

        //        // get all groups
        //        ADVCOMAPILib.IAdvWorkgroups groups = Session.Server.WorkgroupManager.GetWorkgroups(null);

        //        foreach (SiemensEmployeeTO syncEmpl in syncEmployees)
        //        {
        //            try
        //            {
        //                // check group validity                    
        //                ADVCOMAPILib.IAdvWorkgroup emplGroup = null;
        //                string groupName = "";
        //                if (syncEmpl.DepartmentID != -1)
        //                {
        //                    // get SiPass groupID
        //                    int emplGroupID = syncEmpl.DepartmentID;
        //                    int groupLevel = brezaDAO.getLevelDepartment(syncEmpl.DepartmentID);
        //                    groupName = brezaDAO.getNameDepartment(syncEmpl.DepartmentID);

        //                    if (groupLevel >= Constants.SiemensDepartmentLevel)
        //                        emplGroupID = brezaDAO.getParentDepartment(syncEmpl.DepartmentID);

        //                    if (emplGroupID == -1)
        //                    {
        //                        debug.writeLog(DateTime.Now + " SynchronizeEmployees(): Not found parent group for ID: " + syncEmpl.DepartmentID.ToString().Trim() + " for employee ID: "
        //                            + syncEmpl.ID.Trim() + ", PK: " + syncEmpl.PK.ToString().Trim() + "\n");
        //                        continue;
        //                    }

        //                    if (brezaDepartmentsDict.ContainsKey(emplGroupID))
        //                    {
        //                        foreach (ADVCOMAPILib.IAdvWorkgroup group in groups)
        //                        {
        //                            if (group.Id == brezaDepartmentsDict[emplGroupID])
        //                            {
        //                                emplGroup = group;
        //                                break;
        //                            }
        //                        }
        //                    }

        //                    if (emplGroup == null)
        //                    {
        //                        debug.writeLog(DateTime.Now + " SynchronizeEmployees(): Not found group ID: " + syncEmpl.DepartmentID.ToString().Trim() + " for employee ID: "
        //                            + syncEmpl.ID.Trim() + ", PK: " + syncEmpl.PK.ToString().Trim() + "\n");
        //                        continue;
        //                    }
        //                }

        //                ADVCOMAPILib.IAdvFilter filter = Session.Server.Common.CreateFilter();
        //                ADVCOMAPILib.AdvFilterEntry filterEntry = new ADVCOMAPILib.AdvFilterEntry();
        //                filterEntry.propertyName = "EmployeeNumber";
        //                filterEntry.value = syncEmpl.ID.Trim();
        //                filter.Add(filterEntry);

        //                // check if employee already exists
        //                ADVCOMAPILib.IAdvEmployees emplList = Session.Server.EmployeeManager.GetEmployeesWithFilter(filter);

        //                if ((syncEmpl.TypeOfCh == Constants.siemensDeleteFlag || syncEmpl.TypeOfCh == Constants.siemensUpdateFlag)
        //                    && emplList.Count <= 0)
        //                {
        //                    debug.writeLog(DateTime.Now + " SynchronizeEmployees(): There is no existing employee to delete/change for ID: " + syncEmpl.ID.Trim() + ", PK: " + syncEmpl.PK.ToString().Trim() + "\n");
        //                    continue;
        //                }

        //                if (syncEmpl.TypeOfCh == Constants.siemensUpdateFlag || (syncEmpl.TypeOfCh == Constants.siemensAddFlag && emplList.Count > 0))
        //                {
        //                    // update existing employee
        //                    foreach (ADVCOMAPILib.IAdvEmployee empl in emplList)
        //                    {
        //                        if (syncEmpl.FirstName.Trim() != "")
        //                            empl["FirstName"] = syncEmpl.FirstName.Trim();
        //                        if (syncEmpl.LastName.Trim() != "")
        //                            empl["LastName"] = syncEmpl.LastName.Trim();
        //                        if (emplGroup != null)
        //                        {
        //                            empl["Workgroup"] = emplGroup.Name;

        //                            // set group name in custom filed
        //                            if (groupName.Trim() != "")
        //                                empl["Odeljenje"] = groupName.Trim();

        //                            // set employee group name lines in custom fileds
        //                            string line1 = "";
        //                            string line2 = "";
        //                            string line3 = "";

        //                            getWorkgroupLines(emplGroup.Name.Trim(), ref line1, ref line2, ref line3);

        //                            empl["WorkgroupTxtLn1"] = line1.Trim();
        //                            empl["WorkgroupTxtLn2"] = line2.Trim();
        //                            empl["WorkgroupTxtLn3"] = line3.Trim();
        //                        }
        //                        if (syncEmpl.JMBG.Trim() != "")
        //                            empl["jmbg"] = syncEmpl.JMBG.Trim();

        //                        if (syncEmpl.Status.Trim().ToUpper() != Constants.SiemensStatusActive)
        //                        {
        //                            // void cards
        //                            foreach (ADVCOMAPILib.IAdvCredential credential in empl.Credentials)
        //                            {
        //                                credential.Active = false;
        //                            }

        //                            empl["Void"] = true;
        //                        }

        //                        empl.SaveChanges();

        //                        brezaDAO.updateEmployeeToProcessed(syncEmpl.PK, true);

        //                        break;
        //                    }
        //                }
        //                else if (syncEmpl.TypeOfCh == Constants.siemensAddFlag)
        //                {
        //                    if (emplGroup == null)
        //                    {
        //                        debug.writeLog(DateTime.Now + " SynchronizeEmployees(): There is no group for new employee ID: " + syncEmpl.ID.Trim() + ", PK: " + syncEmpl.PK.ToString().Trim() + "\n");
        //                        continue;
        //                    }

        //                    ADVCOMAPILib.IAdvEmployee empl = Session.Server.EmployeeManager.CreateNewEmployee();
        //                    empl["EmployeeNumber"] = syncEmpl.ID.Trim();
        //                    if (syncEmpl.FirstName.Trim() != "")
        //                        empl["FirstName"] = syncEmpl.FirstName.Trim();
        //                    if (syncEmpl.LastName.Trim() != "")
        //                        empl["LastName"] = syncEmpl.LastName.Trim();
        //                    empl["Workgroup"] = emplGroup.Name;
        //                    // set group name in custom filed
        //                    if (groupName.Trim() != "")
        //                        empl["Odeljenje"] = groupName.Trim();
        //                    if (syncEmpl.JMBG.Trim() != "")
        //                        empl["jmbg"] = syncEmpl.JMBG.Trim();

        //                    // set employee group name lines in custom fileds
        //                    string line1 = "";
        //                    string line2 = "";
        //                    string line3 = "";

        //                    getWorkgroupLines(emplGroup.Name.Trim(), ref line1, ref line2, ref line3);

        //                    empl["WorkgroupTxtLn1"] = line1.Trim();
        //                    empl["WorkgroupTxtLn2"] = line2.Trim();
        //                    empl["WorkgroupTxtLn3"] = line3.Trim();

        //                    empl["StartDateTime"] = DateTime.Now.Date;

        //                    if (syncEmpl.Status.Trim().ToUpper() != Constants.SiemensStatusActive)
        //                    {
        //                        // void cards
        //                        foreach (ADVCOMAPILib.IAdvCredential credential in empl.Credentials)
        //                        {
        //                            credential.Active = false;
        //                        }

        //                        empl["Void"] = true;
        //                    }

        //                    Session.Server.EmployeeManager.GetEmployees().Add(empl);
                            
        //                    brezaDAO.updateEmployeeToProcessed(syncEmpl.PK, true);
        //                }
        //                else if (syncEmpl.TypeOfCh == Constants.siemensDeleteFlag)
        //                {
        //                    // delete existing employee
        //                    foreach (ADVCOMAPILib.IAdvEmployee empl in emplList)
        //                    {
        //                        foreach (ADVCOMAPILib.IAdvCredential credential in empl.Credentials)
        //                        {
        //                            credential.Active = false;                                    
        //                        }

        //                        empl["Void"] = true;

        //                        empl.SaveChanges();

        //                        brezaDAO.updateEmployeeToProcessed(syncEmpl.PK, true);

        //                        break;
        //                    }
        //                }
        //                else
        //                {
        //                    debug.writeLog(DateTime.Now + " SynchronizeEmployees(): Unknown type of change flag: " + syncEmpl.TypeOfCh.Trim() + " for employee ID: " + syncEmpl.ID.Trim() + ", PK: " + syncEmpl.PK.ToString().Trim() + "\n");
        //                    continue;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                debug.writeLog(DateTime.Now + " SynchronizeEmployees(): PK: " + syncEmpl.PK.ToString().Trim() + " - " + ex.Message + "\n");                                                
        //                continue;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        debug.writeLog(DateTime.Now + " SynchronizeEmployees(): " + ex.Message + "\n");
        //        //throw ex;
        //    }
        //}

        //private void SynchronizeDepartments()
        //{            
        //    try
        //    {
        //        // get all departments from xml
        //        ArrayList departmentsList = brezaDAO.deserializeDepartments(Constants.SiPassDepartmentMappingPath);

        //        Dictionary<int, int> brezaDepartmentsDict = new Dictionary<int, int>();
        //        foreach (object member in departmentsList)
        //        {
        //            if (member is SiemensDepartmentTO)
        //            {
        //                if (!brezaDepartmentsDict.ContainsKey(((SiemensDepartmentTO)member).Odid))
        //                    brezaDepartmentsDict.Add(((SiemensDepartmentTO)member).Odid, ((SiemensDepartmentTO)member).SiPassID);
        //            }
        //        }

        //        // get all departments for synchronization
        //        List<SiemensDepartmentTO> syncDepartments = brezaDAO.getDepartmentsUnprocessed();

        //        foreach (SiemensDepartmentTO syncDept in syncDepartments)
        //        {
        //            try
        //            {
        //                ADVCOMAPILib.IAdvWorkgroup department = null;

        //                ADVCOMAPILib.IAdvWorkgroups groups = Session.Server.WorkgroupManager.GetWorkgroups(null);

        //                if (syncDept.Odid != -1)
        //                {
        //                    int siPassID = -1;
        //                    if (brezaDepartmentsDict.ContainsKey(syncDept.Odid))
        //                        siPassID = brezaDepartmentsDict[syncDept.Odid];

        //                    bool sameName = false;
        //                    foreach (ADVCOMAPILib.IAdvWorkgroup group in groups)
        //                    {
        //                        if (siPassID != -1 && group.Id == siPassID)
        //                            department = group;
        //                        else if (group.Name.Trim().ToUpper() == syncDept.Name.Trim().ToUpper())
        //                        {
        //                            // group with same name found
        //                            sameName = true;
        //                            break;
        //                        }
        //                    }

        //                    if (sameName)
        //                    {
        //                        debug.writeLog(DateTime.Now + " SynchronizeDepartments(): Same group name exists for PK: " + syncDept.PK.ToString().Trim() + "\n");
        //                        continue;
        //                    }
        //                }
        //                else
        //                {
        //                    debug.writeLog(DateTime.Now + " SynchronizeDepartments(): Not valid department Odid for PK: " + syncDept.PK.ToString().Trim() + "\n");
        //                    continue;
        //                }

        //                if ((syncDept.TypeOfCh == Constants.siemensDeleteFlag || syncDept.TypeOfCh == Constants.siemensUpdateFlag) && department == null)
        //                {
        //                    debug.writeLog(DateTime.Now + " SynchronizeDepartments(): There is no existing department to delete/change for Odid: " + syncDept.Odid.ToString().Trim() + ", PK: " + syncDept.PK.ToString().Trim() + "\n");
        //                    continue;
        //                }

        //                if (syncDept.TypeOfCh == Constants.siemensUpdateFlag || (syncDept.TypeOfCh == Constants.siemensAddFlag && department != null))
        //                {
        //                    // update existing department                        
        //                    if (syncDept.Name.Trim() != "")
        //                        department.Name = syncDept.Name.Trim();

        //                    department.Save();

        //                    brezaDAO.updateDepartmentToProcessed(syncDept.PK, true);
        //                }
        //                else if (syncDept.TypeOfCh == Constants.siemensAddFlag)
        //                {
        //                    department = Session.Server.WorkgroupManager.CreateNewWorkgroup();

        //                    if (syncDept.Name.Trim() != "")
        //                        department.Name = syncDept.Name.Trim();

        //                    // ***** are there more initial settings?????
        //                    department.Partition = true;
        //                    department.GroupType = ADVCOMAPILib.AdvWorkgroupType.AdvWorkgroupType_Department;

        //                    department.Save();

        //                    // add new department to dictionary
        //                    if (!brezaDepartmentsDict.ContainsKey(syncDept.Odid))
        //                        brezaDepartmentsDict.Add(syncDept.Odid, department.Id);

        //                    brezaDAO.updateDepartmentToProcessed(syncDept.PK, true);

        //                    // add new group to file
        //                    ArrayList deptList = new ArrayList();
        //                    foreach (int Odid in brezaDepartmentsDict.Keys)
        //                    {
        //                        SiemensDepartmentTO deptTO = new SiemensDepartmentTO();
        //                        deptTO.Odid = Odid;
        //                        deptTO.SiPassID = brezaDepartmentsDict[Odid];

        //                        deptList.Add(deptTO);
        //                    }

        //                    if (File.Exists(Constants.SiPassDepartmentMappingPath))
        //                        File.Delete(Constants.SiPassDepartmentMappingPath);

        //                    brezaDAO.serializeDepartments(deptList, Constants.SiPassDepartmentMappingPath);
        //                }
        //                else if (syncDept.TypeOfCh == Constants.siemensDeleteFlag)
        //                {
        //                    // delete existing department
        //                    Session.Server.WorkgroupManager.DeleteWorkgroup(department);
        //                    brezaDAO.updateDepartmentToProcessed(syncDept.PK, true);

        //                    // delete group from file
        //                    ArrayList deptList = new ArrayList();
        //                    foreach (int Odid in brezaDepartmentsDict.Keys)
        //                    {
        //                        if (Odid != syncDept.Odid)
        //                        {
        //                            SiemensDepartmentTO deptTO = new SiemensDepartmentTO();
        //                            deptTO.Odid = Odid;
        //                            deptTO.SiPassID = brezaDepartmentsDict[Odid];

        //                            deptList.Add(deptTO);
        //                        }
        //                    }

        //                    if (File.Exists(Constants.SiPassDepartmentMappingPath))
        //                        File.Delete(Constants.SiPassDepartmentMappingPath);

        //                    brezaDAO.serializeDepartments(deptList, Constants.SiPassDepartmentMappingPath);
        //                }
        //                else
        //                {
        //                    debug.writeLog(DateTime.Now + " SynchronizeDepartments(): Unknown type of change flag: " + syncDept.TypeOfCh.Trim() + " for department Odid: "
        //                        + syncDept.Odid.ToString().Trim() + ", PK: " + syncDept.PK.ToString().Trim() + "\n");
        //                    continue;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                debug.writeLog(DateTime.Now + " SynchronizeDepartments(): PK: " + syncDept.PK.ToString().Trim() + " - " + ex.Message + "\n");
        //                continue;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        debug.writeLog(DateTime.Now + " SynchronizeDepartments(): " + ex.Message + "\n");
        //        //throw ex;
        //    }
        //}

        //private void SynchronizeEmployeesImages()
        //{
        //    try
        //    {
        //        // get all employees
        //        ADVCOMAPILib.IAdvEmployees emplList = Session.Server.EmployeeManager.GetEmployees();
        //        List<string> brezaEmplImages = brezaDAO.getEmplImages();
 
        //        foreach (ADVCOMAPILib.IAdvEmployee empl in emplList)
        //        {
        //            // check if employee's image is changed since last sinchronization
        //            //if (empl["ImageUpdateDate"] != null && empl["ImageUpdateDate"] is DateTime && (DateTime)empl["ImageUpdateDate"] > lastSynchronizationTime)
        //            if (!brezaEmplImages.Contains(empl["EmployeeNumber"].ToString().Trim()))
        //            {
        //                Array img = empl.GetCardholderImage(ADVCOMAPILib.AdvImageType.AdvCardholderImage);

        //                if (img != null && img is byte[])
        //                {
        //                    SiemensEmployeeImageTO imgTO = new SiemensEmployeeImageTO();
        //                    imgTO.ID = empl["EmployeeNumber"].ToString().Trim();
        //                    imgTO.Image = (byte[])img;
        //                    imgTO.ReadStatus = 0;

        //                    brezaDAO.insertEmployeeImage(imgTO, true);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        debug.writeLog(DateTime.Now + " SynchronizeEmployeesImages(): " + ex.Message + "\n");
        //        //throw ex;
        //    }
        //}

        //private void SynchronizeEmployeesCards()
        //{
        //    try
        //    {
        //        // get all employees
        //        ADVCOMAPILib.IAdvEmployees emplList = Session.Server.EmployeeManager.GetEmployees();

        //        // get all cards (last synchronized card for each employee)
        //        Dictionary<string, string> empCardsDict = brezaDAO.getEmpls();

        //        foreach (ADVCOMAPILib.IAdvEmployee empl in emplList)
        //        {
        //            // add employee card if it is not in the table
        //            if (empCardsDict.ContainsKey(empl["EmployeeNumber"].ToString().Trim())
        //                && empCardsDict[empl["EmployeeNumber"].ToString().Trim()].Contains(empl["CardNumber"].ToString().Trim()))
        //                continue;
                    
        //            SiemensEmpTO empTO = new SiemensEmpTO();
        //            empTO.ID = empl["EmployeeNumber"].ToString().Trim();
        //            empTO.IDC = empl["CardNumber"].ToString().Trim();
        //            empTO.RS = 0;

        //            brezaDAO.insertEmp(empTO, true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        debug.writeLog(DateTime.Now + " SynchronizeEmployeesCards(): " + ex.Message + "\n");
        //        //throw ex;
        //    }
        //}

        //private void GetEmployeeData()
        //{
        //    try
        //    {
        //        // get all visitors
        //        ADVCOMAPILib.IAdvFilter filter = Session.Server.Common.CreateFilter();
        //        ADVCOMAPILib.AdvFilterEntry filterEntry = new ADVCOMAPILib.AdvFilterEntry();
        //        filterEntry.propertyName = "Visitor";
        //        filterEntry.value = true;
        //        filter.Add(filterEntry);

        //        ADVCOMAPILib.IAdvEmployees emplList = Session.Server.EmployeeManager.GetEmployeesWithFilter(filter);

        //        Dictionary<int, SiemensEmployeeTO> employeeDictNew = new Dictionary<int, SiemensEmployeeTO>();
        //        Dictionary<int, SiemensTruckLogTO> trucksDictNew = new Dictionary<int, SiemensTruckLogTO>();
                
        //        Dictionary<int, SiemensEmployeeTO> emplDict = siemensSyncDAO.getAllEmployees();
        //        Dictionary<int, bool> cardStatuses = siemensSyncDAO.getCardStatuses();

        //        List<SiemensEmployeeTO> empDatalList = new List<SiemensEmployeeTO>();
        //        List<int> emplIDs = new List<int>();

        //        DateTime modTime = DateTime.Now;
                
        //        // populate visitors dictionary - key is SiPass employee ID
        //        foreach (ADVCOMAPILib.IAdvEmployee empl in emplList)
        //        {
        //            if (empl["EmployeeId"] == null)
        //                continue;

        //            int emplID = -1;
        //            if (!int.TryParse(empl["EmployeeId"].ToString().Trim(), out emplID))
        //                continue;

        //            if (!employeeDictNew.ContainsKey(emplID))
        //            {
        //                SiemensEmployeeTO visitorTO = new SiemensEmployeeTO();
        //                visitorTO.EmplID = emplID;
        //                visitorTO.Visitor = true;
        //                if (empl["jmbgVisitor"] != null)
        //                    visitorTO.JMBG = empl["jmbgVisitor"].ToString().Trim();
        //                if (empl["visitorVrstaKartice"] != null)
        //                    visitorTO.CardType = empl["visitorVrstaKartice"].ToString().Trim();                        
        //                employeeDictNew.Add(emplID, visitorTO);
        //            }

        //            if (empl["visitorVrstaKartice"] != null && (empl["visitorVrstaKartice"].ToString().Trim() == Constants.SiemensVisitorTypeTruck || empl["visitorVrstaKartice"].ToString().Trim() == Constants.SiemensVisitorTypeTruckDriver))
        //            {
        //                // create truck record from visitor record
        //                SiemensTruckLogTO truckTO = new SiemensTruckLogTO();

        //                truckTO.EmplID = emplID;

        //                if (empl["visitorVrstaKartice"].ToString().Trim() == Constants.SiemensVisitorTypeTruck)
        //                {
        //                    if (empl["LastName"] != null)
        //                        truckTO.TruckRegistration = empl["LastName"].ToString().Trim();
        //                    if (empl["company"] != null)
        //                    {
        //                        truckTO.BuyerName = empl["company"].ToString().Trim();
        //                        truckTO.ForwarderName = empl["company"].ToString().Trim();
        //                    }
        //                    if (empl["po"] != null)
        //                        truckTO.OrderForm = empl["po"].ToString().Trim();
        //                    truckTO.Type = Constants.SiemensVisitorTypeTruck;
        //                }
        //                else
        //                {
        //                    if (empl["FirstName"] != null)
        //                        truckTO.DriverFirstName = empl["FirstName"].ToString().Trim();
        //                    if (empl["LastName"] != null)
        //                        truckTO.DriverLastName = empl["LastName"].ToString().Trim();
        //                    truckTO.Type = Constants.SiemensVisitorTypeTruckDriver;
        //                }

        //                if (!trucksDictNew.ContainsKey(truckTO.EmplID))
        //                    trucksDictNew.Add(truckTO.EmplID, truckTO);
        //            }

        //            // create employee data record
        //            emplIDs.Add(emplID);

        //            SiemensEmployeeTO emplTO = new SiemensEmployeeTO();
        //            emplTO.EmplID = emplID;

        //            if (empl["CardNumber"] != null)
        //                emplTO.CardNumber = empl["CardNumber"].ToString().Trim();

        //            if (empl["brojKarticeVisitor"] != null)
        //                emplTO.CardNumberVisitor = empl["brojKarticeVisitor"].ToString().Trim();

        //            if (cardStatuses.ContainsKey(emplID) && cardStatuses[emplID])
        //                emplTO.CardStatus = Constants.statusActive;
        //            else
        //                emplTO.CardStatus = Constants.statusRetired;

        //            if (empl["visitorVrstaKartice"] != null)
        //                emplTO.CardType = empl["visitorVrstaKartice"].ToString().Trim();

        //            if (empl["company"] != null)
        //                emplTO.Company = empl["company"].ToString().Trim();

        //            if (emplDict.ContainsKey(emplID))
        //                emplTO.DepartmentID = emplDict[emplID].DepartmentID;

        //            if (empl["FirstName"] != null)
        //                emplTO.FirstName = empl["FirstName"].ToString().Trim();

        //            if (empl["LastName"] != null)
        //                emplTO.LastName = empl["LastName"].ToString().Trim();

        //            if (empl["EmployeeNumber"] != null)
        //                emplTO.ID = empl["EmployeeNumber"].ToString().Trim();

        //            if (empl["jmbgVisitor"] != null)
        //                emplTO.JMBG = empl["jmbgVisitor"].ToString().Trim();

        //            if (empl["po"] != null)
        //                emplTO.Po = empl["po"].ToString().Trim();

        //            if (empl["visitorVrstaVozila"] != null)
        //                emplTO.VechicleCategory = empl["visitorVrstaVozila"].ToString().Trim();

        //            if (empl["visitorMarkaVozila"] != null)
        //                emplTO.VechicleBrand = empl["visitorMarkaVozila"].ToString().Trim();

        //            if (empl["visitorBojaVozila"] != null)
        //                emplTO.VechicleColor = empl["visitorBojaVozila"].ToString().Trim();

        //            if (empl["visitorTipVozila"] != null)
        //                emplTO.VechicleType = empl["visitorTipVozila"].ToString().Trim();

        //            if (empl["visitorPropusnica"] != null)
        //                emplTO.Permition = empl["visitorPropusnica"].ToString().Trim();

        //            emplTO.Visitor = true;
        //            emplTO.CreatedBy = Constants.SiemensACTAUser;
        //            emplTO.CreatedTime = modTime;
        //            emplTO.ModifiedBy = Constants.SiemensACTAUser;
        //            emplTO.ModifiedTime = modTime;

        //            empDatalList.Add(emplTO);
        //        }

        //        foreach (int id in emplDict.Keys)
        //        {
        //            if (emplDict[id].Visitor)
        //                continue;

        //            // create employee data record
        //            emplIDs.Add(id);

        //            SiemensEmployeeTO emplTO = new SiemensEmployeeTO();
        //            emplTO.EmplID = id;

        //            if (cardStatuses.ContainsKey(id) && cardStatuses[id])
        //                emplTO.CardStatus = Constants.statusActive;
        //            else
        //                emplTO.CardStatus = Constants.statusRetired;
                                        
        //            emplTO.DepartmentID = emplDict[id].DepartmentID;
        //            emplTO.FirstName = emplDict[id].FirstName.Trim();
        //            emplTO.LastName = emplDict[id].LastName.Trim();                    
        //            emplTO.ID = emplDict[id].ID.Trim();
        //            emplTO.CardNumber = emplDict[id].CardNumber.Trim();
        //            emplTO.Visitor = false;
        //            emplTO.CreatedBy = Constants.SiemensACTAUser;
        //            emplTO.CreatedTime = modTime;
        //            emplTO.ModifiedBy = Constants.SiemensACTAUser;
        //            emplTO.ModifiedTime = modTime;

        //            empDatalList.Add(emplTO);

        //            if (!employeeDictNew.ContainsKey(emplTO.EmplID))
        //                employeeDictNew.Add(emplTO.EmplID, emplTO);
        //        }

        //        if (ConnectSiemensReportSync())
        //        {
        //            try
        //            {
        //                // get from employee_data employees that do not exist anymore
        //                List<int> emplDataIDList = siemensReportSyncDAO.getEmployeeDataIDList();
        //                string IDs = "";
        //                foreach (int id in emplDataIDList)
        //                {
        //                    if (!emplIDs.Contains(id))
        //                        IDs += id + ",";
        //                }

        //                if (IDs.Length > 0)
        //                    IDs = IDs.Substring(0, IDs.Length - 1);

        //                if (siemensReportSyncDAO.beginTransaction())
        //                {
        //                    try
        //                    {
        //                        bool saved = true;

        //                        // delete employees that do not exist anymore
        //                        // 18.06.2014. Sanja - do not delete them, maybe there are registrations for them
        //                        //if (IDs.Length > 0)
        //                        //    saved = saved && siemensReportSyncDAO.deleteEmployeeData(IDs, false);

        //                        foreach (SiemensEmployeeTO emplTO in empDatalList)
        //                        {
        //                            if (!saved)
        //                                break;

        //                            if (!emplDataIDList.Contains(emplTO.EmplID))
        //                            {
        //                                // insert new
        //                                saved = saved && (siemensReportSyncDAO.insertEmployeeData(emplTO, false) > 0);
        //                            }
        //                            else
        //                            {
        //                                // update existing
        //                                saved = saved && siemensReportSyncDAO.updateEmployeeData(emplTO, false);
        //                            }
        //                        }

        //                        if (saved)
        //                        {
        //                            siemensReportSyncDAO.commitTransaction();
        //                            trucksDict = trucksDictNew;
        //                            employeeDict = employeeDictNew;
        //                        }
        //                        else
        //                        {
        //                            siemensReportSyncDAO.rollbackTransaction();
        //                            debug.writeLog(DateTime.Now + " GetEmployeeData(): Employee data not updated.\n");
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        siemensReportSyncDAO.rollbackTransaction();
        //                        debug.writeLog(DateTime.Now + " GetEmployeeData(): Employee data not updated.\n");
        //                        throw ex;
        //                    }
        //                }
        //                else
        //                    debug.writeLog(DateTime.Now + " GetEmployeeData(): Employee data not updated.\n");

        //                DisconnectSiemensReportSync();
        //            }
        //            catch (Exception ex)
        //            {
        //                DisconnectSiemensReportSync();
        //                throw ex;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        debug.writeLog(DateTime.Now + " GetEmployeeData(): " + ex.Message + "\n");
        //        //throw ex;
        //    }
        //}
        /*comment siemens*/ 

        public bool StartReading()
        {
            try
            {
                // get states that are not registrations from .config file
                notRegStatesList = new List<string>();
                string[] states = Constants.SiemensStatesAntiPassBack.Split(',');
                foreach (string state in states)
                {
                    if (!notRegStatesList.Contains(state.Trim()))
                        notRegStatesList.Add(state.Trim());
                }

                // get states that are registrations from .config file
                regStatesList = new List<string>();
                string[] statesValid = Constants.SiemensAuditTrailValidStates.Split(',');
                foreach (string state in statesValid)
                {
                    if (!regStatesList.Contains(state.Trim()))
                        regStatesList.Add(state.Trim());
                }

                // do synchronization at the beggining to get employee data
                try
                {
                    //Synchronize();
                }
                catch (Exception ex)
                {
                    debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".StartReading() Synchronization: " + ex.Message);
                }

                this.isReading = true;

                while (nextMonthlyCheckTime < DateTime.Now)
                {
                    nextMonthlyCheckTime = nextMonthlyCheckTime.AddMinutes(monthlyCheckTimeout);
                }

                if (downloadManagerThread == null)
                {
                    downloadManagerThread = new Thread(new ThreadStart(Reading));
                    downloadManagerThread.Start();
                }
                else if (!downloadManagerThread.IsAlive)
                {
                    downloadManagerThread = new Thread(new ThreadStart(Reading));
                    downloadManagerThread.Start();
                }
                else if (downloadManagerThread.ThreadState == ThreadState.Suspended)
                {
                    debug.writeLog("Thread was suspended, it should be resumed!");
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() +
                    ".StartReading() : " + ex.Message);
                throw ex;
            }

            return this.isReading;
        }

        public bool StartSynchronization()
        {
            try
            {
                this.isSynchronizing = true;

                while (nextSyncTime < DateTime.Now)
                {
                    nextSyncTime = nextSyncTime.AddMinutes(syncTimeout);
                }

                if (synchronizationManagerThread == null)
                {
                    synchronizationManagerThread = new Thread(new ThreadStart(Synchronization));
                    synchronizationManagerThread.Start();
                }
                else if (!synchronizationManagerThread.IsAlive)
                {
                    synchronizationManagerThread = new Thread(new ThreadStart(Synchronization));
                    synchronizationManagerThread.Start();
                }
                else if (synchronizationManagerThread.ThreadState == ThreadState.Suspended)
                {
                    debug.writeLog("Synchronization thread was suspended, it should be resumed!");
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() +
                    ".StartSynchronization() : " + ex.Message);
                throw ex;
            }

            return this.isSynchronizing;
        }

        private bool isSyncTime()
        {
            if (DateTime.Now > nextSyncTime)
                return true;
            else
                return false;
        }

        private bool isMonthlyCheckTime()
        {
            if (DateTime.Now > nextMonthlyCheckTime)
                return true;
            else
                return false;
        }

        public bool chekPrerequests()
		{
            bool isOk = true;
            //bool isOk = false;
            //this.LogFileDirectory = getLogFilePath();

            //if (!this.LogFileDirectory.Equals(""))
            //{
            //    isOk = true;
            //}

			return isOk;
		}

        private string getLine(ref string name, int maxCharacterNum)
        {
            try
            {
                string line = "";

                if (name.Trim().Length <= maxCharacterNum)
                    line = name.Trim();
                else
                {
                    // get maximal words from name for line
                    int index = name.IndexOf(' ');
                    while (index >= 0)
                    {
                        int lineLength = line.Length + name.Substring(0, index).Length;
                        if (line.Trim() != "")
                            lineLength++;
                        if (lineLength <= maxCharacterNum)
                        {
                            if (line.Trim() != "")
                                line += " ";

                            line += name.Substring(0, index);
                            name = name.Substring(index + 1);
                            index = name.IndexOf(' ');

                            if (index < 0 && name.Trim() != "")
                            {
                                lineLength = line.Length + name.Length;
                                if (line.Trim() != "")
                                    lineLength++;

                                if (lineLength <= maxCharacterNum)
                                {
                                    if (line.Trim() != "")
                                        line += " ";

                                    line += name.Trim();

                                    name = "";
                                }
                            }
                        }
                        else
                            index = -1;
                    }
                }

                return line;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".getLine() : " + ex.Message);
                throw ex;
            }
        }

        private void getWorkgroupLines(string groupName, ref string line1, ref string line2, ref string line3)
        {
            try
            {
                line1 = getLine(ref groupName, Constants.SiemensWGLine1Max);
                line2 = getLine(ref groupName, Constants.SiemensWGLine2Max);
                line3 = getLine(ref groupName, Constants.SiemensWGLine3Max);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".getWorkgroupLines() : " + ex.Message);
            }
        }

        private bool isVechicleType(string type)
        {
            try
            {
                return type.Trim().ToUpper() == Constants.SiemensVisitorTypeVehiclePrivate.Trim().ToUpper() || type.Trim().ToUpper() == Constants.SiemensVisitorTypeVehicleOfficial.Trim().ToUpper();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".isVechicleType() : " + ex.Message);
                throw ex;
            }
        }

        private bool isDriver(SiemensEmployeeTO emplTO)
        {
            try
            {
                return !emplTO.Visitor 
                    || (emplTO.CardType.Trim().ToUpper() != Constants.SiemensVisitorTypeVehiclePrivate.Trim().ToUpper() && emplTO.CardType.Trim().ToUpper() != Constants.SiemensVisitorTypeVehicleOfficial.Trim().ToUpper()
                    && emplTO.CardType.Trim().ToUpper() != Constants.SiemensVisitorTypeTruck.Trim().ToUpper() && emplTO.CardType.Trim().ToUpper() != Constants.SiemensVisitorTypeTruckDriver.Trim().ToUpper());
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".isDriver() : " + ex.Message);
                throw ex;
            }
        }

        public bool AbortReading()
        {
            bool isAborted = false;
            try
            {
                try
                {
                    downloadManagerThread.Abort();
                }
                catch
                { }
                downloadManagerThread = null;
                isAborted = true;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".Reset() : " + ex.Message);
            }

            return isAborted;
        }

        public bool AbortSynchronization()
        {
            bool isAborted = false;
            try
            {
                try
                {
                    synchronizationManagerThread.Abort();
                }
                catch
                { }

                synchronizationManagerThread = null;
                isAborted = true;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".Reset() : " + ex.Message);
            }

            return isAborted;
        }

        public bool StopReadingLogs()
        {
            try
            {
                if (downloadManagerThread.IsAlive)
                {
                    this.isReading = false;
                }

                System.Console.WriteLine("Thread Stopped at:  " + DateTime.Now.ToString("hh:mm:ss"));
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".StopReadingLogs() : " + ex.Message);
            }

            return false;
        }

        public bool StopSynchronization()
        {
            try
            {
                if (synchronizationManagerThread != null && synchronizationManagerThread.IsAlive)
                {
                    this.isSynchronizing = false;
                }

                System.Console.WriteLine("Synchronization thread stopped at:  " + DateTime.Now.ToString("hh:mm:ss"));
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".StopSynchronization() : " + ex.Message);
            }

            return false;
        }
    }
}
