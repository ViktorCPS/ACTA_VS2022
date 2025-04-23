using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DataAccess;
using Util;
using TransferObjects;

namespace Common
{
    public class EmployeeType
    {
          DAOFactory daoFactory = null;
          EmployeeTypeDAO edao = null;
		
		DebugLog log;

        EmployeeTypeTO emplTypeTO = new EmployeeTypeTO();

        public EmployeeTypeTO EmployeeTypeTO
        {
            get { return emplTypeTO; }
            set { emplTypeTO = value; }
        }
        		
		public EmployeeType()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getEmployeeTypeDAO(null);
		}
        public EmployeeType(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeeTypeDAO(dbConnection);
        }

        public EmployeeType(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getEmployeeTypeDAO(null);
            }
        }

        public int Save()
        {
            int isUpdated = 0;

            try
            {
                isUpdated = edao.insert(this.EmployeeTypeTO);
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
                isUpdated = edao.update(this.EmployeeTypeTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeType.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public List<EmployeeTypeTO> Search()
        {           
            try
            {
                return edao.search(this.EmployeeTypeTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeType.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, Dictionary<int, string>> SearchDictionary()
        {
            try
            {
                return edao.searchDictionary(this.EmployeeTypeTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeType.SearchDictionary(): " + ex.Message + "\n");
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
