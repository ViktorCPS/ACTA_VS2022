using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Util;
using TransferObjects;
using System.Data;

namespace Common
{
    public class EmployeePYDataSum
    {
        DAOFactory daoFactory = null;
		EmployeePYDataSumDAO edao = null;
		
		DebugLog log;

        EmployeePYDataSumTO emplSum = new EmployeePYDataSumTO();

        public EmployeePYDataSumTO EmplSum
        {
            get { return emplSum; }
            set { emplSum = value; }
        }

        public EmployeePYDataSum()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getEmployeePYDataSumDAO(null);
		}
        public EmployeePYDataSum(Object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeePYDataSumDAO(dbConnection);
        }
        /// <summary>
        /// Save Employee 
        /// </summary>
        /// <returns></returns>
        public int Save( bool doCommit)
        {
            int saved = 0;
            try
            {
                saved = edao.insert(EmplSum, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataSum.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }
        
        public uint getMaxCalcID()
        {
            try
            {
                return edao.getMaxCalcID();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataSum.getMaxCalcID(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public DateTime getSumMonth(uint calcID)
        {
            try
            {
                return edao.getSumMonth(calcID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataSum.getSumMonth(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<EmployeePYDataSumTO> getSumDates(DateTime from, DateTime to)
        {
            try
            {
                return edao.getSumDates(from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataSum.getSumDates(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<EmployeePYDataSumTO> getEmployeesSum()
        {
            try
            {
                return edao.getEmployeesSum(this.EmplSum);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataSum.getEmployeesSum(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public Dictionary<int, Dictionary<string, decimal>> getEmployeesSumValues(uint calcID, string codes)
        {
            try
            {
                return edao.getEmployeesSumValues(calcID, codes);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataSum.getEmployeesSumValues(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<int> getEmployees(uint caclID)
        {
            try
            {
                return edao.getEmployees(caclID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataSum.getEmployees(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = edao.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataSum.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                edao.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataSum.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                edao.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataSum.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return edao.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataSum.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                edao.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataSum.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        
    }
}
