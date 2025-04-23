using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Common;
using Util;
using ACTASurveillanceManagement;
using TransferObjects;

namespace ACTASurveillanceService
{

    [ServiceContract(Namespace = "http://ACTASurveillanceService")]
    public interface IACTASurveillanceService
    {
        [OperationContract]
        void Start();
        [OperationContract]
        void Stop();
        [OperationContract]
        CardOwnerTO GetCurrentOwner();       
        [OperationContract]
        string GetServiceStatus();
    }
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                  IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(
     RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ACTASurveillanceService : IACTASurveillanceService
    {
        private static ACTASurveillanceService instance = null;
        private SurveillanceManager manager;

        // Debug
        private DebugLog log;

        private string serviceStatus = "";

        private object locker = new object();
        private static object instanceLocker = new object();

        private ACTASurveillanceService()
        {
            // Init Debug
            NotificationController.SetApplicationName("ACTASurveillanceService");
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            // init ticket controller
            try
            {
                if (InitializeSurveillanceManager() == false)
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

        public static ACTASurveillanceService Instance
        {
            get
            {
                lock (instanceLocker)
                {
                    if (instance == null)
                    {
                        instance = new ACTASurveillanceService();
                    }
                    return instance;
                }
            }
        }

        private bool InitializeSurveillanceManager()
        {
            bool success = false;
            try
            {
                manager = SurveillanceManager.GetInstance();
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ACTASurveillanceService.Start(), Exception: " + ex.Message);
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ACTASurveillanceService.Stop(), Exception: " + ex.Message);
            }
        }

        public string GetServiceStatus()
        {
            string managerStatus = "";
            lock (locker)
            {
                try
                {
                    managerStatus = manager.getStatus();
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ACTASurveillanceService.GetServiceStatus(), Exception: " + ex.Message);
                }
            }
            return managerStatus;
        }


        public CardOwnerTO GetCurrentOwner()
        {
            CardOwnerTO owner = null;
            lock (locker)
            {
                try
                {
                    owner = manager.getOwner();
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ACTASurveillanceService.GetCurrentOwner(), Exception: " + ex.Message);
                }
            }
            return owner;
        }

    

        #endregion
    }
}