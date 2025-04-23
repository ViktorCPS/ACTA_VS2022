using System;
using System.Collections.Generic;
using System.Text;
using Util;
using System.Data;

using TransferObjects;
using DataAccess;

namespace Common
{
    public class EmployeeLockedDay
    {
        DAOFactory daoFactory = null;
        EmployeeLockedDayDAO dao = null;
		
		DebugLog log;

        EmployeeLockedDayTO emplLockedDayTO = new EmployeeLockedDayTO();

        public EmployeeLockedDayTO EmplLockedDayTO
        {
            get { return emplLockedDayTO; }
            set { emplLockedDayTO = value; }
        }
        		
		public EmployeeLockedDay()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getEmployeeLockedDayDAO(null);
		}

        public EmployeeLockedDay(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getEmployeeLockedDayDAO(dbConnection);
        }

        public EmployeeLockedDay(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                dao = daoFactory.getEmployeeLockedDayDAO(null);
            }
        }

        public int Save(bool doCommit)
        {
            try
            {
                return dao.insert(this.EmplLockedDayTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLockedDay.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(bool doCommit)
        {
            try
            {
                return dao.delete(this.EmplLockedDayTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLockedDay.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, List<DateTime>> SearchLockedDays(string emplIDs, string type, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return dao.getLockedDays(emplIDs, type, fromDate, toDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeLockedDay.SearchLockedDays(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeLockedDay.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeLockedDay.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeLockedDay.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeLockedDay.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeLockedDay.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
