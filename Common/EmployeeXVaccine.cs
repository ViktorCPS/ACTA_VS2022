using System;
using System.Collections.Generic;
using System.Text;
using Util;
using System.Data;

using TransferObjects;
using DataAccess;

namespace Common
{
    public class EmployeeXVaccine
    {
        DAOFactory daoFactory = null;
        EmployeeXVaccineDAO edao = null;
		
		DebugLog log;

        EmployeeXVaccineTO emplXVacTO = new EmployeeXVaccineTO();

        public EmployeeXVaccineTO EmplXVaccineTO
        {
            get { return emplXVacTO; }
            set { emplXVacTO = value; }
        }
        		
		public EmployeeXVaccine()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getEmployeeXVaccineDAO(null);
		}

        public EmployeeXVaccine(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeeXVaccineDAO(dbConnection);
        }

        public EmployeeXVaccine(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getEmployeeXVaccineDAO(null);
            }
        }

        public int Save(bool doCommit)
        {
            try
            {
                return edao.insert(this.EmplXVaccineTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXVaccine.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return edao.update(this.EmplXVaccineTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXVaccine.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(string recID, bool doCommit)
        {
            try
            {
                return edao.delete(recID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXVaccine.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeXVaccineTO> SearchEmployeeXVaccines()
        {           
            try
            {
                return edao.getEmployeeXVaccines(this.EmplXVaccineTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXVaccine.SearchEmployeeXVaccines(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<uint, EmployeeXVaccineTO> SearchEmployeeXVaccines(string emplIDs, string vaccines, DateTime from, DateTime to)
        {
            try
            {
                return edao.getEmployeeXVaccines(emplIDs, vaccines, from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXVaccine.SearchEmployeeXVaccines(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeXVaccineTO> SearchEmployeeXVaccinesNotProcessed(string emplIDs)
        {
            try
            {
                return edao.getEmployeeXVaccinesNotProcessed(emplIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXVaccine.SearchEmployeeXVaccinesNotProcessed(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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
                log.writeLog(DateTime.Now + " EmployeeXVaccine.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeXVaccine.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeXVaccine.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeXVaccine.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeXVaccine.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
