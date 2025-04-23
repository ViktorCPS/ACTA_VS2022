using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class EmployeePYTransportData
    {
        DAOFactory daoFactory = null;
        EmployeePYTransportDataDAO dao = null;

		DebugLog log;

        EmployeePYTransportDataTO dataTO = new EmployeePYTransportDataTO();

        public EmployeePYTransportDataTO DataTO
		{
            get { return dataTO; }
			set{ dataTO = value; }
		}

        public EmployeePYTransportData()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getEmployeePYTransportDataDAO(null);
		}

        public EmployeePYTransportData(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getEmployeePYTransportDataDAO(dbConnection);
        }
        
        public int Save(bool doCommit)
        {
            try
            {
                return dao.insert(this.DataTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYTransportData.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Delete(string emplIDs, DateTime from, DateTime to, bool doCommit)
        {
            try
            {
                return dao.delete(emplIDs, from, to, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYTransportData.Delete(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>> Search(string emplIDs, DateTime month)
        {
            try
            {
                return dao.getEmplTransportData(emplIDs, month);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYTransportData.Search(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYTransportData.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYTransportData.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYTransportData.RollbackTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYTransportData.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYTransportData.SetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
