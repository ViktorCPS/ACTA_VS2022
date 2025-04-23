using System;
using System.Collections.Generic;
using System.Text;

using DataAccess;
using TransferObjects;
using Util;
using System.Data;
using System.Globalization;

namespace Common
{
    public class LogTmpAdditionalInfo
    {
        DAOFactory daoFactory = null;
        LogTmpAdditionalInfoDAO logTmpDAO = null;

        DebugLog log;

        LogTmpAdditionalInfoTO logTmpTO = new LogTmpAdditionalInfoTO();

        public LogTmpAdditionalInfoTO LogTmpTO
        {
            get { return logTmpTO; }
            set { logTmpTO = value; }
        }
             
        public LogTmpAdditionalInfo()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            logTmpDAO = daoFactory.getLogTmpDAO(null);
		}

        public LogTmpAdditionalInfo(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            logTmpDAO = daoFactory.getLogTmpDAO(dbConnection);
        }

        public LogTmpAdditionalInfo(int reader_id, uint tag_id,
            int antenna, int event_happened, int action_commited, DateTime event_time, string gps_data, string cardholder_name, int cardholder_id)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            logTmpDAO = daoFactory.getLogTmpDAO(null);

            this.LogTmpTO.ReaderID = reader_id;
            this.LogTmpTO.TagID = tag_id;
            this.LogTmpTO.Antenna = antenna;
            this.LogTmpTO.EventHappened = event_happened;
            this.LogTmpTO.ActionCommited = action_commited;
            this.LogTmpTO.EventTime = event_time;
            this.LogTmpTO.GpsData = gps_data;
            this.LogTmpTO.CardholderName = cardholder_name;
            this.LogTmpTO.CardholderID = cardholder_id;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = logTmpDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogTmpAdditionalInfo.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                logTmpDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogTmpAdditionalInfo.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                logTmpDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogTmpAdditionalInfo.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return logTmpDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogTmpAdditionalInfo.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                logTmpDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogTmpAdditionalInfo.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int Save(LogTmpAdditionalInfoTO logTmpTo)
        {
            int inserted;
            try
            {
                inserted = logTmpDAO.insert(logTmpTo);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogTmpAdditionalInfo.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return inserted;
        }

        public List<LogTmpAdditionalInfoTO> Search()
        {
            try
            {
                return logTmpDAO.getLogs(this.logTmpTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogTmpAdditionalInfo.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Update(LogTmpAdditionalInfoTO logTmpTo, bool doCommit)
        {
            bool isUpdated;

            try
            {
                isUpdated = logTmpDAO.update(logTmpTo, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogTmpAdditionalInfo.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public void Clear()
        {
            this.LogTmpTO = new LogTmpAdditionalInfoTO();
        }

        public LogTmpAdditionalInfoTO Find(int readerID, uint tagID, int antenna, DateTime eventTime)
        {
            LogTmpAdditionalInfoTO logTo = new LogTmpAdditionalInfoTO();

            try
            {
                logTo = logTmpDAO.find(readerID, tagID, antenna, eventTime);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogTmpAdditionalInfo.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return logTo;
        }

        public bool Delete(int readerID, uint tagID, int antenna, DateTime eventTime)
        {
            bool isDeleted;

            try
            {
                isDeleted = logTmpDAO.delete(readerID, tagID, antenna, eventTime);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LogTmpAdditionalInfo.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }





    }
}
