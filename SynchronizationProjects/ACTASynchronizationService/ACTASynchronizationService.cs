using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Common;
using Util;
using ACTASyncManagement;

namespace ACTASynchronizationService
{
  
        [ServiceContract(Namespace = "http://ACTASinchronizationService")]
        public interface IACTASynchronizationService
        {
            [OperationContract]
            void Start();
            [OperationContract]
            void Stop();
            [OperationContract]
            string GetServiceStatus();
        }
        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                      IncludeExceptionDetailInFaults = true)]
        [AspNetCompatibilityRequirements(
         RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
        public class ACTASynchronizationService : IACTASynchronizationService
        {
            private static ACTASynchronizationService instance = null;
            private SyncManager manager;

            // Debug
            private DebugLog log;

            private string serviceStatus = "";

            private object locker = new object();
            private static object instanceLocker = new object();

            private ACTASynchronizationService()
            {
                // Init Debug
                NotificationController.SetApplicationName("ACTASynchronizationService");
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                // init ticket controller
                try
                {
                    if (InitializeSynchronizationManager() == false)
                    {
                        SetServiceStatus("Error initializing synchronization manager!");
                    }
                }
                catch (Exception ex)
                {
                    log.writeLog(ex);
                    SetServiceStatus("Error initializing synchronization manager!");
                }
            }

            public static ACTASynchronizationService Instance
            {
                get
                {
                    lock (instanceLocker)
                    {
                        if (instance == null)
                        {
                            instance = new ACTASynchronizationService();
                        }
                        return instance;
                    }
                }
            }

            private bool InitializeSynchronizationManager()
            {
                bool success = false;
                try
                {
                    manager = new SyncManager();
                    success = true;
                }
                catch
                {
                    if (manager != null)
                    {
                        manager.Stop();
                    }
                }
                return success;
            }

            private void SetServiceStatus(string text)
            {
                lock (locker)
                {
                    serviceStatus = text;
                }
            }

            #region Service operation contracts
            public void Start()
            {
                try
                {
                    manager.Start();
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ACTASynchronizationService.Start(), Exception: " + ex.Message);
                }
            }



            public void Stop()
            {
                try
                {

                    manager.Stop();

                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ACTASynchronizationService.Stop(), Exception: " + ex.Message);
                }
            }

            public string GetServiceStatus()
        {
            string managerStatus = "";
            lock (locker)
            {
                try
                {
                    managerStatus = manager.getManagerThreadStatus();
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ACTASurveillanceService.GetServiceStatus(), Exception: " + ex.Message);
                }
            }
            return managerStatus;
        }


            #endregion
        }
    } 
