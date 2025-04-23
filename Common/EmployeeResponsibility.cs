using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DataAccess;
using Util;
using TransferObjects;

namespace Common
{
    public class EmployeeResponsibility
    {
        private EmployeeResponsibilityDAO osDAO;
		private DAOFactory daoFactory;

		DebugLog log;

        EmployeeResponsibilityTO responsibilityTO = new EmployeeResponsibilityTO();

        public EmployeeResponsibilityTO ResponsibilityTO
        {
            get { return responsibilityTO; }
            set { responsibilityTO = value; }
        }

		public EmployeeResponsibility()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			osDAO = daoFactory.getEmployeeResponsibilityDAO(null);
		}

        public EmployeeResponsibility(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            osDAO = daoFactory.getEmployeeResponsibilityDAO(dbConnection);
        }

        public bool delete(bool doCommit)
        {
            try
            {
                return osDAO.delete(ResponsibilityTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeResponsibility.delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool insert(bool doCommit)
        {
            try
            {
                return osDAO.insert(ResponsibilityTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeResponsibility.insert(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, List<int>> SearchUnitsResponsibilitiesByEmployee(int id, string type)
        {
            try
            {
                return osDAO.getUnitsResponsibilitiesByEmployee(id, type);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeResponsibility.SearchUnitsResponsibilitiesByEmployee(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeResponsibilityTO> Search()
        {
            try
            {
                return osDAO.getEmplResponsibility(ResponsibilityTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeResponsibility.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeResponsibilityTO> Search(string unitIDs, string type)
        {
            try
            {
                return osDAO.getEmplResponsibility(unitIDs, type);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeResponsibility.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
       
        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = osDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeResponsibility.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                osDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeResponsibility.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                osDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeResponsibility.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return osDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeResponsibility.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                osDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeResponsibility.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }       
    }
}
