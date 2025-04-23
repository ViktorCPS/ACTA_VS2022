using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using Util;
using TransferObjects;

namespace Common
{
    public class EmployeeCounterMonthlyBalanceHist
    {
        DAOFactory daoFactory = null;
        EmployeeCounterMonthlyBalanceHistDAO dao = null;

		DebugLog log;

        EmployeeCounterMonthlyBalanceTO balanceTO = new EmployeeCounterMonthlyBalanceTO();

		public EmployeeCounterMonthlyBalanceTO BalanceTO
		{
            get { return balanceTO; }
			set{ balanceTO = value; }
		}

        public EmployeeCounterMonthlyBalanceHist()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getEmployeeCounterMonthlyBalanceHistDAO(null);
		}

        public EmployeeCounterMonthlyBalanceHist(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getEmployeeCounterMonthlyBalanceHistDAO(dbConnection);
        }
        
        public int Save(bool doCommit)
        {
            try
            {
                return dao.insert(this.BalanceTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalanceHist.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool BeginTransaction()
        {
            try
            {
                return dao.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalanceHist.BeginTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                dao.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalanceHist.CommitTransaction(): " + ex.Message + "\n");
                throw ex;
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
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalanceHist.RollbackTransaction(): " + ex.Message + "\n");
                throw ex;
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
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalanceHist.GetTransaction(): " + ex.Message + "\n");
                throw ex;
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
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalanceHist.SetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
