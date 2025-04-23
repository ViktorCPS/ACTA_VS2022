using System;
using System.Collections.Generic;
using System.Text;
using Util;
using DataAccess;
using TransferObjects;
using System.Data;

namespace Common
{
    public class EmployeePYDataAnalitical
    {
        DAOFactory daoFactory = null;
        EmployeePYDataAnaliticalDAO edao = null;

        DebugLog log;

        EmployeePYDataAnaliticalTO emplAnaliticalTO = new EmployeePYDataAnaliticalTO();

        public EmployeePYDataAnaliticalTO EmplAnaliticalTO
        {
            get { return emplAnaliticalTO; }
            set { emplAnaliticalTO = value; }
        }

        public EmployeePYDataAnalitical()
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeePYDataAnaliticalDAO(null);
        }
        public EmployeePYDataAnalitical(Object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeePYDataAnaliticalDAO(dbConnection);
        }
        /// <summary>
        /// Save Employee 
        /// </summary>
        /// <returns></returns>
        public int Save(bool doCommit)
        {
            int saved = 0;
            try
            {
                saved = edao.insert(EmplAnaliticalTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataAnalitical.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }
        public List<EmployeePYDataAnaliticalTO> Search(string employees, string payment_code, uint py_calc_id, string type)
        {

            List<EmployeePYDataAnaliticalTO> list = new List<EmployeePYDataAnaliticalTO>();
            try
            {
                list = edao.getEmployeesAnalitical(employees, payment_code, py_calc_id, type);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYDataAnalitical.Search(): " + ex.Message + "\n");
                throw ex;
            }
            return list;
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
                log.writeLog(DateTime.Now + " EmployeePYDataAnalitical.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYDataAnalitical.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYDataAnalitical.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYDataAnalitical.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePYDataAnalitical.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
