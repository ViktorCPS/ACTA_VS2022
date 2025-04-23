using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Runtime.Serialization;
using System.IO;
using System.Configuration;

using Common;
using Util;
using TransferObjects;
using ReaderInterfaceSManagement;
using System.Collections;

namespace ReaderInterfaceSService
{
    [ServiceContract(Namespace = "http://ReaderInterfaceSService")]
    public interface IReaderInterfaceSService
    {
        [OperationContract]
        void StartLogDownload();
        [OperationContract]
        void StopLogDownload();
        [OperationContract]
        void AbortLogDownload();
        //[OperationContract]
        //bool ChekPrerequests();
        //[OperationContract]
        //ReadersInfos GetReadersInfos();
        //[OperationContract]
        //ReadersStatuses GetReadersStatuses();
        //[OperationContract]
        //void SetSelectedReaders(ReadersInfos selectedReaders);
        [OperationContract]
        bool IsLogDownloadStarted();
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                   IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(
     RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReaderInterfaceSService : IReaderInterfaceSService
    {
        private static ReaderInterfaceSService instance = null;

        // Debug
        private DebugLog log;

        //// Controller instance
        //private NotificationController Controller;

        //// Observer client instance
        //private NotificationObserverClient observerClient;

        //// List of all readers as defined in the config file
        //private List<ReaderTO> readers = new List<ReaderTO>();

        //// List of selected readers as received from a service client
        //private List<ReaderTO> selectedReaders = new List<ReaderTO>();

        //private ReadersInfos readersInfos;
        //private ReadersStatuses readersStatuses;
        private bool isLogDownloadStarted = false;
        private bool isSynchronizationStarted = false;
        private bool isListeningStarted = false;

        private object locker = new object();
        private static object instanceLocker = new object();

        // DB time constants in ms
        private const long connectToDBOverallTimeOut = 300000;
        private const int connectToDBRetryInterval = 5000;

        private ReaderInterfaceSService()
        {
            // uncomment to enable pause for attaching process to debugger
            //System.Threading.Thread.Sleep(30000);

            // Init DebugLog
            NotificationController.SetApplicationName("ReaderInterfaceSService");
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

           // CreateReadersList();

            //// This service need to receive notification about current downloads
            //InitializeObserverClient();

            //// Initialize readers infos and download statuses data
            //readersInfos = new ReadersInfos(readers);
            //readersStatuses = new ReadersStatuses(readers);
        }

        public static ReaderInterfaceSService Instance
        {
            get
            {
                lock (instanceLocker)
                {
                    if (instance == null)
                    {
                        instance = new ReaderInterfaceSService();
                    }
                    return instance;
                }
            }
        }

        //private void CreateReadersList()
        //{
        //    bool succeeded = false;
        //    DateTime t0 = DateTime.Now;
        //    string gates = "";
        //    while (!succeeded)
        //    {
        //        try
        //        {
        //            lock (locker)
        //            {
        //                gates = ConfigurationManager.AppSettings["Gates"];
        //                if (gates != null)
        //                {
        //                    readers = new Reader().Search(gates);

        //                    foreach (ReaderTO rTO in readers)
        //                    {
        //                        selectedReaders.Add(rTO);
        //                    }
        //                }
        //                succeeded = true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.writeLog(DateTime.Now + " Exception in: " +
        //                this.ToString() + ".CreateReadersList() : " + ex.Message + "\n");
        //        }
        //        finally
        //        {
        //            if (!succeeded)
        //            {
        //                System.Threading.Thread.Sleep(connectToDBRetryInterval);
        //                if ((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > connectToDBOverallTimeOut)
        //                {
        //                    succeeded = true; // abort creating readers list
        //                    log.writeLog(DateTime.Now + " " + this.ToString() +
        //                        ".CreateReadersList() : Unable to create readers list for gates " + gates + "!\n");
        //                }
        //            }
        //        }
        //    }
        //}

        //private void InitializeObserverClient()
        //{
        //    observerClient = new NotificationObserverClient(this.ToString());
        //    Controller = NotificationController.GetInstance();
        //    Controller.AttachToNotifier(observerClient);
        //    this.observerClient.Notification += new NotificationEventHandler(this.ReaderActionHandler);
        //    this.observerClient.ReaderNotification += new NotificationEventHandler(this.ReaderAlertHandler);
        //}

        //private void ReaderActionHandler(object sender, NotificationEventArgs args)
        //{
        //    try
        //    {
        //        lock (locker)
        //        {
        //            foreach (ReaderTO reader in readers)
        //            {
        //                if (reader.ReaderID == args.reader.ReaderID)
        //                {
        //                    int readerID = reader.ReaderID;

        //                    readersStatuses.ActionReaderID = readerID;
        //                    readersStatuses.StatusesData[readerID].IsDownloading = args.isDownloading;
        //                    readersStatuses.StatusesData[readerID].Action = args.readerAction;
        //                    readersStatuses.StatusesData[readerID].NextTimeDownload = args.nextTimeDownload;
        //                    readersStatuses.StatusesData[readerID].MemoryOccupation = args.reader.MemoryOccupation;
        //                    readersStatuses.StatusesData[readerID].IsReaderActionDone = true;

        //                    readersStatuses.StatusesData[readerID].IsNetExist = true;
        //                    readersStatuses.StatusesData[readerID].IsDataExist = true;
        //                    readersStatuses.StatusesData[readerID].IsReaderAlertDone = false;

        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".ReaderActionHandler(): " + ex.Message + "\n");
        //    }
        //}

        //private void ReaderAlertHandler(object sender, NotificationEventArgs args)
        //{
        //    try
        //    {
        //        lock (locker)
        //        {
        //            foreach (ReaderTO reader in readers)
        //            {
        //                if (reader.ReaderID == args.reader.ReaderID)
        //                {
        //                    int readerID = reader.ReaderID;

        //                    readersStatuses.AlertReaderID = readerID;
        //                    readersStatuses.StatusesData[readerID].IsNetExist = args.isNetExist;
        //                    readersStatuses.StatusesData[readerID].IsDataExist = args.isDataExist;
        //                    readersStatuses.StatusesData[readerID].IsReaderAlertDone = true;

        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".ReaderAlertHandler(): " + ex.Message + "\n");
        //    }
        //}

        #region Service operation contracts

        public void StartLogDownload()
        {
            if (!isListeningStarted)
            {
                ReaderEventController eventController = ReaderEventController.GetInstance();
                eventController.StartListening();
                isListeningStarted = true;
            }

            DownloadManager downloadManager = DownloadManager.GetInstance();
            if (!isLogDownloadStarted)
            {
                //if (selectedReaders.Count > 0)
                //{
                //    if (downloadManager.chekPrerequests())
                //    {
                        //downloadManager.CreateReaderList(GetSelectedReaders());
                        //downloadManager.PushOldReaderLogs();
                        downloadManager.StartReading();
                        isLogDownloadStarted = true;
                        log.writeLog("\n" + DateTime.Now + "StartLogDownload() : Log Reading Started!  \n");
                //    }
                //    else
                //    {
                //        log.writeLog(DateTime.Now + " " + this.ToString() + ".StartLogDownload() failed: " + "chekPrerequests() return false." + "\n");
                //    }
                //}
                //else
                //{
                //    log.writeLog(DateTime.Now + " " + this.ToString() + ".StartLogDownload() failed: " + "Selected readers count = 0." + "\n");
                //}
            }
            else
            {
                log.writeLog(DateTime.Now + " " + this.ToString() + ".StartLogDownload() failed: " + "Downloading is already started." + "\n");
            }

            if (!isSynchronizationStarted)
            {
                downloadManager.StartSynchronization();
                isSynchronizationStarted = true;
                log.writeLog("\n" + DateTime.Now + "StartLogDownload() : Synchronization Started!  \n");                
            }
            else
            {
                log.writeLog(DateTime.Now + " " + this.ToString() + ".StartLogDownload() failed: " + "Synchronization is already started." + "\n");
            }
        }

        public void StopLogDownload()
        {
            DownloadManager downloadManager = DownloadManager.GetInstance();
            downloadManager.StopReadingLogs();
            downloadManager.StopSynchronization();
            ReaderEventController controller = ReaderEventController.GetInstance();
            controller.StopListening();
            isLogDownloadStarted = false;
            isSynchronizationStarted = false;
            log.writeLog(DateTime.Now + " " + this.ToString() + " has been closed! \n");
        }

        public void AbortLogDownload()
        {
            DownloadManager downloadManager = DownloadManager.GetInstance();
            downloadManager.AbortReading();
            isLogDownloadStarted = false;
            downloadManager.AbortSynchronization();
            isSynchronizationStarted = false;
            log.writeLog("\n " + DateTime.Now + " AbortLogDownload() : Log Reading Stopped!  \n");
        }

        //public bool ChekPrerequests()
        //{
        //    DownloadManager downloadManager = DownloadManager.GetInstance();
        //    return downloadManager.chekPrerequests();
        //}

        //public ReadersInfos GetReadersInfos()
        //{
        //    return readersInfos;
        //}

        //public ReadersStatuses GetReadersStatuses()
        //{
        //    lock (locker)
        //    {
        //        return readersStatuses;
        //    }
        //}

        //public void SetSelectedReaders(ReadersInfos selectedReaders)
        //{
        //    lock (locker)
        //    {
        //        this.selectedReaders.Clear();
        //        foreach (ReaderInfo readerInfo in selectedReaders.InfosData)
        //        {
        //            ReaderTO reader = GetReaderByID(readerInfo.ReaderID);
        //            if (reader != null)
        //            {
        //                this.selectedReaders.Add(reader);
        //            }
        //        }
        //    }
        //}

        public bool IsLogDownloadStarted()
        {
            return isLogDownloadStarted && isSynchronizationStarted;
        }

        #endregion

        // Helper methods

        //private List<ReaderTO> GetSelectedReaders()
        //{
        //    lock (locker)
        //    {
        //        log.writeLog(DateTime.Now + " Start : GetSelectedReaders() \n");
        //        foreach (ReaderTO reader in selectedReaders)
        //        {
        //            log.writeLog("\n***** Reader *****\n");
        //            log.writeLog("ReaderID: " + reader.ReaderID.ToString() + " \n");
        //            log.writeLog("A0GateID: " + reader.A0GateID.ToString() + " \n");
        //            log.writeLog("A1GateID: " + reader.A1GateID.ToString() + " \n");
        //            log.writeLog("DownloadInterval: " + reader.DownloadInterval.ToString() + " \n");
        //            log.writeLog("DownloadStartTime: " + reader.DownloadStartTime.ToString() + " \n");
        //            log.writeLog("\n***** Reader *****\n");
        //        }
        //        log.writeLog(DateTime.Now + " End : GetSelectedReaders() \n");

        //        return selectedReaders;
        //    }
        //}

        //private ReaderTO GetReaderByID(int readerID)
        //{
        //    foreach (ReaderTO reader in readers)
        //    {
        //        if (reader.ReaderID == readerID)
        //        {
        //            return reader;
        //        }
        //    }
        //    return null;
        //}
    }
}
