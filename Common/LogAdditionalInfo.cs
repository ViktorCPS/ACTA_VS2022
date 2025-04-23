using System;
using System.Collections.Generic;
using System.Text;

using DataAccess;
using TransferObjects;
using Util;
using System.Data;

namespace Common
{
    public class LogAdditionalInfo
    {
        DAOFactory daoFactory = null;
        LogAdditionalInfoDAO logAddInfoDAO = null;

        DebugLog log;

        LogAdditionalInfoTO logAddInfoTO = new LogAdditionalInfoTO();

        public LogAdditionalInfoTO LogAddInfoTO
        {
            get { return logAddInfoTO; }
            set { logAddInfoTO = value; }
        }

        public LogAdditionalInfo()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            logAddInfoDAO = daoFactory.getLogAdditionalInfoDAO(null);
		}

        public LogAdditionalInfo(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            logAddInfoDAO = daoFactory.getLogAdditionalInfoDAO(dbConnection);
        }

        public LogAdditionalInfo(uint log_id, string gps_data, string cardholder_name, int cardholder_id, string created_by, DateTime created_time) //, string modified_by, DateTime modified_time)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            logAddInfoDAO = daoFactory.getLogAdditionalInfoDAO(null);

            this.LogAddInfoTO.LogID = log_id;
            this.LogAddInfoTO.GpsData = gps_data;
            this.LogAddInfoTO.CardholderID = cardholder_id;
            this.LogAddInfoTO.CardholderName = cardholder_name;
            this.LogAddInfoTO.CreatedBy = created_by;
            this.LogAddInfoTO.CreatedTime = created_time;
            //this.LogAddInfoTO.ModifiedBy = modified_by;
            //this.LogAddInfoTO.ModifiedTime = modified_time;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = logAddInfoDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogAdditionalInfo.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                logAddInfoDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogAdditionalInfo.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                logAddInfoDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogAdditionalInfo.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return logAddInfoDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogAdditionalInfo.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                logAddInfoDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogAdditionalInfo.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int Save(LogAdditionalInfoTO logAdditionalInfoTO)
        {
            int inserted;
            try
            {
                inserted = logAddInfoDAO.insert(logAdditionalInfoTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogAdditionalInfo.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return inserted;
        }

        public List<LogAdditionalInfoTO> Search()
        {
            try
            {
                return logAddInfoDAO.getLogs(this.logAddInfoTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogAdditionalInfo.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Update(LogAdditionalInfoTO logAddInfoTO, bool doCommit)
        {
            bool isUpdated;

            try
            {
                isUpdated = logAddInfoDAO.update(logAddInfoTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogAdditionalInfo.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public void Clear()
        {
            this.logAddInfoTO = new LogAdditionalInfoTO();
        }

        public LogAdditionalInfoTO Find(int logID)
        {
            LogAdditionalInfoTO logTo = new LogAdditionalInfoTO();

            try
            {
                logTo = logAddInfoDAO.find(logID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogAdditionalInfo.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return logTo;
        }

        public bool Delete(int logID)
        {
            bool isDeleted;

            try
            {
                isDeleted = logAddInfoDAO.delete(logID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogAdditionalInfo.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }
    
    
    
    
    }
}
