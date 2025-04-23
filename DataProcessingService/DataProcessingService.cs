using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Runtime.Serialization;
using System.IO;

using Common;
using Util;
using ReaderManagement;

namespace DataProcessingService
{
    [ServiceContract(Namespace = "http://DataProcessingService")]
    public interface IDataProcessingService
    {
        [OperationContract]
        void StartLogProcessing();
        [OperationContract]
        bool StopLogProcessing();
        [OperationContract]
        bool IsProcessing();
        [OperationContract]
        bool ChekPrerequests();
        [OperationContract]
        string GetCurrentFileInProcessing();
        [OperationContract]
        string GetDataProcessingState();
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                   IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(
     RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DataProcessingService : IDataProcessingService
    {
        private static DataProcessingService instance = null;

        // Debug
        private DebugLog log;

        // Controller instance
        private NotificationController Controller;

        // Observer client instance
        private NotificationObserverClient observerClient;

        private string lblMessageText = "";
        private string lblThreadStateText = "";
        private const string NOMESSAGE = "---";

        private object locker = new object();
        private static object instanceLocker = new object();

        private DataProcessingService()
        {
            // Init Debug
            NotificationController.SetApplicationName("ACTADataProcessingService");
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            // This service need to receive notification about currently processed files
            InitializeObserverClient();
        }

        public static DataProcessingService Instance
        {
            get
            {
                lock (instanceLocker)
                {
                    if (instance == null)
                    {
                        instance = new DataProcessingService();
                    }
                    return instance;
                }
            }
        }

        private void InitializeObserverClient()
        {
            observerClient = new NotificationObserverClient(this.ToString());
            Controller = NotificationController.GetInstance();
            Controller.AttachToNotifier(observerClient);
            this.observerClient.Notification += new NotificationEventHandler(this.FileProcessingHandler);
            this.observerClient.OnDataProcessingStateChanged += new NotificationEventHandler(this.DataProcessingStateChangedHandler);
        }

        private void FileProcessingHandler(object sender, NotificationEventArgs args)
        {
            try
            {
                lock (locker)
                {
                    if (!args.XMLFileName.Equals(""))
                    {
                        if (args.isProcessingNow)
                        {
                            lblMessageText = Constants.fileInProcessing + Path.GetFileName(args.XMLFileName);
                        }
                        else
                        {
                            lblMessageText = NOMESSAGE;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".FileProcessingHandler(): " + ex.Message + "\n");
            }
        }

        private void DataProcessingStateChangedHandler(object sender, NotificationEventArgs args)
        {
            try
            {
                lock (locker)
                {
                    if (!args.message.Equals(""))
                    {
                        if (!args.message.StartsWith("Thread"))
                        {
                            lblMessageText = args.message;
                        }
                        else
                        {
                            lblThreadStateText = args.message;
                        }
                    }
                    else
                    {
                        lblMessageText = NOMESSAGE;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".DataProcessingStateChangedHandler(): " + ex.Message + "\n");
            }
        }

        #region Service operation contracts

        public void StartLogProcessing()
        {
            DataProcessingManager procManager = DataProcessingManager.GetInstance();
            if (!procManager.IsProcessing)
            {
                if (procManager.chekPrerequests())
                {
                    procManager.StartLogProcessing();
                }
                else
                {
                    log.writeLog(DateTime.Now + " " + this.ToString() + ".StartLogProcessing() failed: " + "chekPrerequests() return false." + "\n");
                }
            }
            else
            {
                log.writeLog(DateTime.Now + " " + this.ToString() + ".StartLogProcessing() failed: " + "Processing is already started." + "\n");
            }
        }

        public bool StopLogProcessing()
        {
            DataProcessingManager procManager = DataProcessingManager.GetInstance();
            return procManager.StopLogProcessing();
        }

        public bool IsProcessing()
        {
            DataProcessingManager procManager = DataProcessingManager.GetInstance();
            return procManager.IsProcessing;
        }

        public bool ChekPrerequests()
        {
            DataProcessingManager procManager = DataProcessingManager.GetInstance();
            return procManager.chekPrerequests();
        }

        public string GetCurrentFileInProcessing()
        {
            lock (locker)
            {
                return lblMessageText;
            }
        }

        public string GetDataProcessingState()
        {
            lock (locker)
            {
                return lblThreadStateText;
            }
        }

        #endregion
    }
}
