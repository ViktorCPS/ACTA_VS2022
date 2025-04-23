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
    /// Summary description for CamerasXReaders.
    /// </summary>
    public class CamerasXReaders
    {
        private int _cameraID = -1;
        private int _readerID = -1;
        private string _directionCovered = "";

        DAOFactory daoFactory = null;
        CamerasXReadersDAO camerasXReadersDAO = null;

        DebugLog log;

        public int CameraID
        {
            get { return _cameraID; }
            set { _cameraID = value; }
        }

        public int ReaderID
        {
            get { return _readerID; }
            set { _readerID = value; }
        }

        public string DirectionCovered
        {
            get { return _directionCovered; }
            set { _directionCovered = value; }
        }

        public CamerasXReaders()
		{
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            camerasXReadersDAO = daoFactory.getCamerasXReadersDAO(null);
		}
        public CamerasXReaders(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            camerasXReadersDAO = daoFactory.getCamerasXReadersDAO(dbConnection);
        }

        public CamerasXReaders(int cameraID, int readerID, string directionCovered)
		{
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            this.CameraID = cameraID;
            this.ReaderID = readerID;
            this.DirectionCovered = directionCovered;

            daoFactory = DAOFactory.getDAOFactory();
            camerasXReadersDAO = daoFactory.getCamerasXReadersDAO(null);
		}

        public int Save(int cameraID, int readerID, string directionCovered, bool doCommit)
        {
            int inserted;
            try
            {
                inserted = camerasXReadersDAO.insert(cameraID, readerID, directionCovered, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CamerasXReaders.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return inserted;
        }

        public bool Update(int cameraID, int readerID, string directionCovered, bool doCommit)
        {
            bool isUpdated;

            try
            {
                isUpdated = camerasXReadersDAO.update(cameraID, readerID, directionCovered, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CamerasXReaders.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool Delete(int cameraID, int readerID, bool doCommit)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = camerasXReadersDAO.delete(cameraID, readerID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CamerasXReaders.Delete(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " CamerasXReaders.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return extraHourTO;
        }*/

        public ArrayList Search(int cameraID, int readerID, string directionCovered)
        {
            ArrayList camerasXReadersTOList = new ArrayList();
            ArrayList camerasXReadersList = new ArrayList();

            try
            {
                CamerasXReaders camerasXReadersMember = new CamerasXReaders();
                camerasXReadersTOList = camerasXReadersDAO.getCamerasXReaders(cameraID, readerID, 
                    directionCovered);

                foreach (CamerasXReadersTO crTO in camerasXReadersTOList)
                {
                    camerasXReadersMember = new CamerasXReaders();
                    camerasXReadersMember.ReceiveTransferObject(crTO);

                    camerasXReadersList.Add(camerasXReadersMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CamerasXReaders.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return camerasXReadersList;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = camerasXReadersDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CamerasXReaders.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                camerasXReadersDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CamerasXReaders.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                camerasXReadersDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CamerasXReaders.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return camerasXReadersDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CamerasXReaders.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                camerasXReadersDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CamerasXReaders.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void ReceiveTransferObject(CamerasXReadersTO camerasXReadersTO)
        {
            this.CameraID = camerasXReadersTO.CameraID;
            this.ReaderID = camerasXReadersTO.ReaderID;
            this.DirectionCovered = camerasXReadersTO.DirectionCovered;
        }

        public CamerasXReadersTO SendTransferObject()
        {
            CamerasXReadersTO camerasXReadersTO = new CamerasXReadersTO();

            camerasXReadersTO.CameraID = this.CameraID;
            camerasXReadersTO.ReaderID = this.ReaderID;
            camerasXReadersTO.DirectionCovered = this.DirectionCovered;

            return camerasXReadersTO;
        }

        public void Clear()
        {
            this.CameraID = -1;
            this.ReaderID = -1;
            this.DirectionCovered = "";
        }
    }
}
