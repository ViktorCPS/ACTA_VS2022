using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using Util;
using TransferObjects;

namespace Common
{
    public class SyncAnnualLeaveRecalcHist
    {
        private SyncAnnualLeaveRecalcHistDAO dao;
		private DAOFactory daoFactory;

		DebugLog log;

        SyncAnnualLeaveRecalcTO recalcTO = new SyncAnnualLeaveRecalcTO();

        public SyncAnnualLeaveRecalcTO RecalcTO
        {
            get { return recalcTO; }
            set { recalcTO = value; }
        }

		public SyncAnnualLeaveRecalcHist()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			dao = daoFactory.getSyncAnnualLeaveRecalcHistDAO(null);
		}


        public SyncAnnualLeaveRecalcHist(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getSyncAnnualLeaveRecalcHistDAO(dbConnection);
        }

        public bool insert(bool doCommit)
        {
            try
            {
                return dao.insert(RecalcTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncAnnualLeaveRecalcHist.insert(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<SyncAnnualLeaveRecalcTO> Search(DateTime from, DateTime to, int emplID, int result)
        {
            try
            {
                return dao.getALRecalculations(from, to, emplID, result);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncAnnualLeaveRecalcHist.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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
                log.writeLog(DateTime.Now + " SyncAnnualLeaveRecalcHist.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SyncAnnualLeaveRecalcHist.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SyncAnnualLeaveRecalcHist.RollbackTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SyncAnnualLeaveRecalcHist.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SyncAnnualLeaveRecalcHist.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
