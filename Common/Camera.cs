using System;
using System.Collections;
using System.Configuration;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    /// <summary>
    /// Summary description for Camera.
    /// </summary>
    public class Camera
    {
        private int _cameraID = -1;
        private string _connAddress = "";
        private string _description = "";
        private string _type = "";

        DAOFactory daoFactory = null;
        CameraDAO cameraDAO = null;

        DebugLog log;

        public int CameraID
        {
            get { return _cameraID; }
            set { _cameraID = value; }
        }

        public string ConnAddress
        {
            get { return _connAddress; }
            set { _connAddress = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public Camera()
		{
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            cameraDAO = daoFactory.getCameraDAO(null);
		}

        public Camera(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            cameraDAO = daoFactory.getCameraDAO(dbConnection);
        }

        public Camera(int cameraID, string connAddress,
            string description, string type)
		{
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            this.CameraID = cameraID;
            this.ConnAddress = connAddress;
            this.Description = description;
            this.Type = type;

            daoFactory = DAOFactory.getDAOFactory();
            cameraDAO = daoFactory.getCameraDAO(null);
		}

        public int Save(int cameraID, string connAddress, string description,
            string type, bool doCommit)
        {
            int inserted;
            try
            {
                inserted = cameraDAO.insert(cameraID, connAddress, description,
                    type, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Camera.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return inserted;
        }

        public bool Update(int cameraID, string connAddress, string description,
            string type, bool doCommit)
        {
            bool isUpdated;

            try
            {
                isUpdated = cameraDAO.update(cameraID, connAddress, description,
                    type, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Camera.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool Delete(int cameraID, bool doCommit)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = cameraDAO.delete(cameraID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Camera.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        /*public ExtraHourTO Find(int employeeID, DateTime dateEarned)
        {
            ExtraHourTO extraHourTO = new ExtraHourTO();

            try
            {
                extraHourTO = extraHourDAO.find(employeeID, dateEarned);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Camera.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return extraHourTO;
        }*/

        public ArrayList Search(int cameraID, string connAddress, string description,
            string type)
        {
            ArrayList cameraTOList = new ArrayList();
            ArrayList cameraList = new ArrayList();

            try
            {
                Camera cameraMember = new Camera();
                cameraTOList = cameraDAO.getCameras(cameraID, connAddress, 
                    description, type);

                foreach (CameraTO csfTO in cameraTOList)
                {
                    cameraMember = new Camera();
                    cameraMember.ReceiveTransferObject(csfTO);

                    cameraList.Add(cameraMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Camera.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return cameraList;
        }

        public int GetCameraNextID()
        {
            int ID = 0;

            try
            {
                ID = cameraDAO.getCameraNextID();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Camera.SearchCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return ID;
        }

        public ArrayList SearchOnGate(int gateID)
        {
            ArrayList cameraTOList = new ArrayList();
            ArrayList cameraList = new ArrayList();

            try
            {
                Camera cameraMember = new Camera();
                cameraTOList = cameraDAO.getCamerasOnGate(gateID);

                foreach (CameraTO csfTO in cameraTOList)
                {
                    cameraMember = new Camera();
                    cameraMember.ReceiveTransferObject(csfTO);

                    cameraList.Add(cameraMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Camera.SearchOnGate(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return cameraList;
        }

        public ArrayList SearchForReaders(string readerID, string direction)
        {
            ArrayList cameraTOList = new ArrayList();
            ArrayList cameraList = new ArrayList();

            try
            {
                Camera cameraMember = new Camera();
                cameraTOList = cameraDAO.getCamerasForReaders(readerID, direction);

                foreach (CameraTO csfTO in cameraTOList)
                {
                    cameraMember = new Camera();
                    cameraMember.ReceiveTransferObject(csfTO);

                    cameraList.Add(cameraMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Camera.SearchOnGate(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return cameraList;
        }


        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = cameraDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Camera.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                cameraDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Camera.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                cameraDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Camera.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return cameraDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Camera.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                cameraDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Camera.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void ReceiveTransferObject(CameraTO cameraTO)
        {
            this.CameraID = cameraTO.CameraID;
            this.ConnAddress = cameraTO.ConnAddress;
            this.Description = cameraTO.Description;
            this.Type = cameraTO.Type;
        }

        public CameraTO SendTransferObject()
        {
            CameraTO cameraTO = new CameraTO();

            cameraTO.CameraID = this.CameraID;
            cameraTO.ConnAddress = this.ConnAddress;
            cameraTO.Description = this.Description;
            cameraTO.Type = this.Type;

            return cameraTO;
        }

        public void Clear()
        {
            this.CameraID = -1;
            this.ConnAddress = "";
            this.Description = "";
            this.Type = "";
        }

        public ArrayList getCamerasForMap(int mapID)
        {
            ArrayList cameraTOList = new ArrayList();
            ArrayList cameraList = new ArrayList();

            try
            {
                Camera cameraMember = new Camera();
                cameraTOList = cameraDAO.getCamerasForMap(mapID);

                foreach (CameraTO csfTO in cameraTOList)
                {
                    cameraMember = new Camera();
                    cameraMember.ReceiveTransferObject(csfTO);

                    cameraList.Add(cameraMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Camera.getCamerasForMap(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return cameraList;
        }
    }
}
