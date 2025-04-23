using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class EmployeeCounterMonthlyBalance
    {
        DAOFactory daoFactory = null;
		EmployeeCounterMonthlyBalanceDAO dao = null;

		DebugLog log;

        EmployeeCounterMonthlyBalanceTO balanceTO = new EmployeeCounterMonthlyBalanceTO();

		public EmployeeCounterMonthlyBalanceTO BalanceTO
		{
            get { return balanceTO; }
			set{ balanceTO = value; }
		}

        public EmployeeCounterMonthlyBalance()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getEmployeeCounterMonthlyBalanceDAO(null);
		}

        public EmployeeCounterMonthlyBalance(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getEmployeeCounterMonthlyBalanceDAO(dbConnection);
        }
        
        public int Save(bool doCommit)
        {
            try
            {
                return dao.insert(this.BalanceTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalance.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }
       
        public bool Update(bool doCommit)
        {
            try
            {
                return dao.update(this.BalanceTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalance.Update(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>> SearchEmployeeBalances(string emplIDs, DateTime month, bool exactMonth)
        {
            try
            {
                return dao.getEmployeeBalances(emplIDs, month, exactMonth);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalance.SearchEmployeeBalances(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>> SearchEmployeeBalances(string emplIDs, DateTime month, int counterTypeID)
        {
            try
            {
                return dao.getEmployeeBalances(emplIDs, month, counterTypeID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalance.SearchEmployeeBalances(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<EmployeeCounterMonthlyBalanceTO> SearchEmployeeBalances(string emplIDs, string types, DateTime from, DateTime to)
        {
            try
            {
                return dao.getEmployeeBalances(emplIDs, types, from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalance.SearchEmployeeBalances(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public DateTime GetMinPositiveMonth(string emplIDs)
        {
            try
            {
                return dao.getMinPositiveMonth(emplIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalance.GetMinPositiveMonth(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalance.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalance.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalance.RollbackTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalance.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeCounterMonthlyBalance.SetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
