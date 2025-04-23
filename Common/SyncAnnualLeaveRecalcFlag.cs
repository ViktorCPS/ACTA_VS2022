using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using Util;

namespace Common
{
    public class SyncAnnualLeaveRecalcFlag
    {
        private SyncAnnualLeaveRecalcFlagDAO dao;
		private DAOFactory daoFactory;

		DebugLog log;
        
		public SyncAnnualLeaveRecalcFlag()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			dao = daoFactory.getSyncAnnualLeaveRecalcFlagDAO(null);
		}

        public SyncAnnualLeaveRecalcFlag(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getSyncAnnualLeaveRecalcFlagDAO(dbConnection);
        }
        
        public int GetFlag()
        {
            try
            {
                return dao.getFlag();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncAnnualLeaveRecalcFlag.GetFlag(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Update(int flag, bool isServiceUpdate, bool doCommit)
        {
            try
            {
                return dao.update(flag, isServiceUpdate, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncAnnualLeaveRecalcFlag.Update(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = dao.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncAnnualLeaveRecalcFlag.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                dao.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncAnnualLeaveRecalcFlag.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                dao.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncAnnualLeaveRecalcFlag.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return dao.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncAnnualLeaveRecalcFlag.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                dao.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncAnnualLeaveRecalcFlag.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
