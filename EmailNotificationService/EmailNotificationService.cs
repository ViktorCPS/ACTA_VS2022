using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Common;
using Util;
using EmailNotificationManagement;

namespace EmailNotificationService
{
    [ServiceContract(Namespace = "http://EmailNotificationService")]
    public interface IEmailNotificationService
    {
        [OperationContract]
        void StartNotification();
        [OperationContract]
        void StopNotification();
        [OperationContract]
        string GetServiceStatus();
        [OperationContract]
        string GetNotificationStatus();
    }
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                  IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(
     RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class EmailNotificationService : IEmailNotificationService
    {
        private static EmailNotificationService instance = null;
        private NotificationManager manager;

        // Debug
        private DebugLog log;

        private string serviceStatus = "";

        private object locker = new object();
        private static object instanceLocker = new object();

        private EmailNotificationService()
        {
            // Init Debug
            NotificationController.SetApplicationName("ACTAEmailNotificationService");
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            // init ticket controller
            try
            {
                if (InitializeNotificationManager() == false)
                {
                    SetServiceStatus("Error initializing notification manager!");
                }
            }
            catch (Exception ex)
            {
                log.writeLog(ex);
                SetServiceStatus("Error initializing notification manager!");
            }
        }

        public static EmailNotificationService Instance
        {
            get
            {
                lock (instanceLocker)
                {
                    if (instance == null)
                    {
                        instance = new EmailNotificationService();
                    }
                    return instance;
                }
            }
        }

        private bool InitializeNotificationManager()
        {
            bool success = false;
            try
            {
                manager =  NotificationManager.GetInstance();
                success = true;
            }
            catch
            {
                if (manager != null)
                {
                    manager.StopNotification();
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

        public void StartNotification()
        {

            manager.StartNotification();

        }

        public void StopNotification()
        {
            manager.StopNotification();
        }

        public string GetServiceStatus()
        {
            lock (locker)
            {
                return serviceStatus;
            }
        }
        public string GetNotificationStatus()
        {
            lock (locker)
            {
                return manager.getStatusMessage();
            }
        }

        #endregion
    }
}
