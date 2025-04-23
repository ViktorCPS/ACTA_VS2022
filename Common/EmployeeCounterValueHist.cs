using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class EmployeeCounterValueHist
    {
        DAOFactory daoFactory = null;
		EmployeeCounterValueHistDAO valueDAO = null;

		DebugLog log;

        EmployeeCounterValueHistTO valueTO = new EmployeeCounterValueHistTO();

		public EmployeeCounterValueHistTO ValueTO
		{
            get { return valueTO; }
			set{ valueTO = value; }
		}

        public EmployeeCounterValueHist()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            valueDAO = daoFactory.getEmployeeCounterValueHistDAO(null);
		}
        public EmployeeCounterValueHist(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            valueDAO = daoFactory.getEmployeeCounterValueHistDAO(dbConnection);
        }
        public int Save(bool doCommit)
        {            
            try
            {
                return valueDAO.insert(this.ValueTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValueHist.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public int Save(int emplID, string modifiedBy, bool doCommit)
        {
            try
            {
                return valueDAO.insert(emplID, modifiedBy, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValueHist.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<EmployeeCounterValueHistTO> Search()
        {
            try
            {
                return valueDAO.getEmplCounterValuesHist(this.ValueTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValueHist.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public int SearchFirstModifiedValue(int emplID, int counterType)
        {
            try
            {
                return valueDAO.getFirstModifiedValue(emplID, counterType);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValueHist.SearchFirstModifiedValue(): " + ex.Message + "\n");
                throw ex;
            }
        }
                
        public bool BeginTransaction()
        {
            try
            {
                return valueDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValueHist.BeginTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                valueDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValueHist.CommitTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                valueDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValueHist.RollbackTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return valueDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValueHist.GetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                valueDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterValueHist.SetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
