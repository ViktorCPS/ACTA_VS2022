using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class BufferMonthlyBalancePaid
    {
        DAOFactory daoFactory = null;
		BufferMonthlyBalancePaidDAO dao = null;

		DebugLog log;

        BufferMonthlyBalancePaidTO balanceTO = new BufferMonthlyBalancePaidTO();

		public BufferMonthlyBalancePaidTO BalanceTO
		{
            get { return balanceTO; }
			set{ balanceTO = value; }
		}

        public BufferMonthlyBalancePaid()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getEmployeeCounterMonthlyBalancePaidDAO(null);
		}

        public BufferMonthlyBalancePaid(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getEmployeeCounterMonthlyBalancePaidDAO(dbConnection);
        }
        
        public int Save(bool doCommit)
        {
            try
            {
                return dao.insert(this.BalanceTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " BufferMonthlyBalancePaid.Save(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " BufferMonthlyBalancePaid.Update(): " + ex.Message + "\n");
                throw ex;
            }
        }
       
        public Dictionary<int, Dictionary<DateTime, Dictionary<int, BufferMonthlyBalancePaidTO>>> SearchEmployeeBalancesPaid(uint pyCalcID, int counterType)
        {
            try
            {
                return dao.getEmployeeBalancesPaid(pyCalcID, counterType);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " BufferMonthlyBalancePaid.SearchEmployeeBalancesPaid(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public Dictionary<int, Dictionary<DateTime, Dictionary<int, BufferMonthlyBalancePaidTO>>> SearchEmployeeBalancesPaid(string emplIDs, DateTime from, DateTime to)
        {
            try
            {
                return dao.getEmployeeBalancesPaid(emplIDs, from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " BufferMonthlyBalancePaid.SearchEmployeeBalancesPaid(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " BufferMonthlyBalancePaid.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " BufferMonthlyBalancePaid.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " BufferMonthlyBalancePaid.RollbackTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " BufferMonthlyBalancePaid.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " BufferMonthlyBalancePaid.SetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
