using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using Util;
using TransferObjects;

namespace Common
{
    public class SyncCostCenterHist
    {
        private SyncCostCenterHistDAO ccDAO;
		private DAOFactory daoFactory;

		DebugLog log;

        SyncCostCenterTO ccTO = new SyncCostCenterTO();

        public SyncCostCenterTO CCTO
        {
            get { return ccTO; }
            set { ccTO = value; }
        }

		public SyncCostCenterHist()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			ccDAO = daoFactory.getSyncCostCenterHistDAO(null);
		}


        public SyncCostCenterHist(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            ccDAO = daoFactory.getSyncCostCenterHistDAO(dbConnection);
        }

        public bool insert(bool doCommit)
        {
            try
            {
                return ccDAO.insert(CCTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncCostCenterHist.insert(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<SyncCostCenterTO> Search(DateTime from, DateTime to, string code, string company, int result)
        {
            try
            {
                return ccDAO.getCC(from, to, code, company, result);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncCostCenterHist.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        } 

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = ccDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncCostCenterHist.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                ccDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncCostCenterHist.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                ccDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncCostCenterHist.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return ccDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncCostCenterHist.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                ccDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncCostCenterHist.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
