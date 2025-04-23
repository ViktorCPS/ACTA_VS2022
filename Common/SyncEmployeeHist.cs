using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using TransferObjects;
using Util;
using System.Data;

namespace Common
{
    public class SyncEmployeeHist
    {
        private SyncEmployeesHistDAO emplHistDAO;
		private DAOFactory daoFactory;

		DebugLog log;

        SyncEmployeeTO syncEmplTO = new SyncEmployeeTO();

        public SyncEmployeeTO SyncEmplTO
        {
            get { return syncEmplTO; }
            set { syncEmplTO = value; }
        }

		public SyncEmployeeHist()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			emplHistDAO = daoFactory.getSyncEmployeesHistDAO(null);
		}

        public SyncEmployeeHist(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            emplHistDAO = daoFactory.getSyncEmployeesHistDAO(dbConnection);
        }

        public bool insert(bool doCommit)
        {
            try
            {
                return emplHistDAO.insert(SyncEmplTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncEmployeeHist.insert(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<SyncEmployeeTO> Search(DateTime from, DateTime to, int emplID, int result)
        {
            try
            {
                return emplHistDAO.getEmployees(from, to, emplID, result);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncEmployeeHist.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchLastOU(int employeeID)
        {
            try
            {
                return emplHistDAO.getLastOU(employeeID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncEmployeeHist.SearchLastOU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchLastWU(int employeeID)
        {
            try
            {
                return emplHistDAO.getLastWU(employeeID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncEmployeeHist.SearchLastWU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = emplHistDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncEmployeeHist.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                emplHistDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncEmployeeHist.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                emplHistDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncEmployeeHist.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return emplHistDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncEmployeeHist.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                emplHistDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncEmployeeHist.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
