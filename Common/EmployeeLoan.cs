using System;
using System.Collections.Generic;
using System.Text;
using Util;
using TransferObjects;
using DataAccess;
using System.Data;

namespace Common
{
    public class EmployeeLoan
    {
          DAOFactory daoFactory = null;
          EmployeeLoanDAO edao = null;
		
		DebugLog log;

        EmployeeLoanTO emplTypeTO = new EmployeeLoanTO();

        public EmployeeLoanTO EmployeeLoanTO
        {
            get { return emplTypeTO; }
            set { emplTypeTO = value; }
        }
        		
		public EmployeeLoan()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeeLoanDAO(null);
		}
        public EmployeeLoan(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeeLoanDAO(dbConnection);
        }

        public EmployeeLoan(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getEmployeeLoanDAO(null);
            }
        }

        public int Save(bool doCommit)
        {
            int isUpdated = 0;

            try
            {
                isUpdated = edao.insert(this.EmployeeLoanTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeType.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }
        public bool Update()
        {
            bool isUpdated = false;

            try
            {
                isUpdated = edao.update(this.EmployeeLoanTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeType.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public List<EmployeeLoanTO> Search()
        {
           
            try
            {
                return edao.search(this.EmployeeLoanTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeType.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

        }
        public List<EmployeeLoanTO> Search(DateTime fromDate, DateTime toDate)
        {

            try
            {
                return edao.search(this.EmployeeLoanTO, fromDate, toDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeType.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

        }
        public bool Delete(int recID)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = edao.delete(recID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeType.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
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
                log.writeLog(DateTime.Now + " EmployeeType.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeType.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeType.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeType.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeType.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
