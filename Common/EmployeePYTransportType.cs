using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class EmployeePYTransportType
    {
        DAOFactory daoFactory = null;
        EmployeePYTransportTypeDAO dao = null;

		DebugLog log;

        EmployeePYTransportTypeTO typeTO = new EmployeePYTransportTypeTO();

        public EmployeePYTransportTypeTO TypeTO
		{
            get { return typeTO; }
			set{ typeTO = value; }
		}

        public EmployeePYTransportType()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getEmployeePYTransportTypeDAO(null);
		}

        public EmployeePYTransportType(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getEmployeePYTransportTypeDAO(dbConnection);
        }

        public Dictionary<int, EmployeePYTransportTypeTO> SearchEmployeeTransportTypes()
        {
            try
            {
                return dao.getEmplTransportTypes();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYTransportType.SearchEmployeeTransportTypes(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYTransportType.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYTransportType.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYTransportType.RollbackTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYTransportType.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYTransportType.SetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
