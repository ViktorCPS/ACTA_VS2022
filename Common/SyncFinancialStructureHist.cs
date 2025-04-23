using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Util;
using TransferObjects;
using System.Data;

namespace Common
{
    public class SyncFinancialStructureHist
    {
        private SyncFinancialStructureHistDAO fsDAO;
		private DAOFactory daoFactory;

		DebugLog log;

        SyncFinancialStructureTO fsTO = new SyncFinancialStructureTO();

        public SyncFinancialStructureTO FSTO
        {
            get { return fsTO; }
            set { fsTO = value; }
        }

		public SyncFinancialStructureHist()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			fsDAO = daoFactory.getSyncFinancialStructureHistDAO(null);
		}


        public SyncFinancialStructureHist(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            fsDAO = daoFactory.getSyncFinancialStructureHistDAO(dbConnection);
        }

        public bool insert(bool doCommit)
        {
            try
            {
                return fsDAO.insert(FSTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncFinancialStructureHist.insert(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<SyncFinancialStructureTO> Search(DateTime from, DateTime to, int fsID, int result)
        {
            try
            {
                return fsDAO.getFS(from, to, fsID, result);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncFinancialStructureHist.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = fsDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncFinancialStructureHist.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                fsDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncFinancialStructureHist.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                fsDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncFinancialStructureHist.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return fsDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncFinancialStructureHist.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                fsDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncFinancialStructureHist.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
