using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Common;
using Util;
using ACTAWorkAnalysisReports;

namespace ACTAWorkAnalysisService
{
    [ServiceContract(Namespace = "http://ACTAWorkAnalysisService")]
        public interface IACTAWorkAnalysisService
        {
            [OperationContract]
            void Start();
            [OperationContract]
            void Stop();
          
        }
        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                      IncludeExceptionDetailInFaults = true)]
        [AspNetCompatibilityRequirements(
         RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ACTAWorkAnalysisService: IACTAWorkAnalysisService
        {
            private static ACTAWorkAnalysisService instance = null;
            private WorkAnalysis manager;

            // Debug
            private DebugLog log;

            private string serviceStatus = "";

            private object locker = new object();
            private static object instanceLocker = new object();

            private ACTAWorkAnalysisService()
            {
                // Init Debug
                NotificationController.SetApplicationName("ACTAWorkAnalysisService");
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                log.writeLog("start" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                // init ticket controller
                try
                {
                    if (InitializeWorkAnalysisManager() == false)
                    {
                        log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "Error initializing work analysis manager!");

                    }
                }
                catch (Exception ex)
                {
                    log.writeLog(ex);
                    log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "Error initializing work analysis manager");
                }
            }

            public static ACTAWorkAnalysisService Instance
            {
                get
                {
                    lock (instanceLocker)
                    {
                        if (instance == null)
                        {
                            instance = new ACTAWorkAnalysisService();
                        }
                        return instance;
                    }
                }
            }

            private bool InitializeWorkAnalysisManager()
            {
                bool success = false;
                try
                {
                    manager = WorkAnalysis.GetInstance();
                    success = true;
                }
                catch
                {
                    if (manager != null)
                    {
                        manager.StopReporting();
                    }
                }
                return success;
            }

            //private void SetServiceStatus(string text)
            //{
            //    lock (locker)
            //    {
            //        serviceStatus = text;
            //    }
            //}

            #region Service operation contracts
            public void Start()
            {
                try
                {
                    manager.StartReporting();
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ACTAWorkAnalysisService.Start(), Exception: " + ex.Message);
                }
            }



            public void Stop()
            {
                try
                {

                    manager.StopReporting();

                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ACTAWorkAnalysisService.Stop(), Exception: " + ex.Message);
                }
            }

       

            #endregion
        }
    } 
