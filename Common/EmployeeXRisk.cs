using System;
using System.Collections.Generic;
using System.Text;
using Util;
using System.Data;

using TransferObjects;
using DataAccess;

namespace Common
{
    public class EmployeeXRisk
    {        
        DAOFactory daoFactory = null;
        EmployeeXRiskDAO edao = null;
		
		DebugLog log;

        EmployeeXRiskTO emplXRiskTO = new EmployeeXRiskTO();

        public EmployeeXRiskTO EmplXRiskTO
        {
            get { return emplXRiskTO; }
            set { emplXRiskTO = value; }
        }
        		
		public EmployeeXRisk()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getEmployeeXRiskDAO(null);
		}

        public EmployeeXRisk(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeeXRiskDAO(dbConnection);
        }

        public EmployeeXRisk(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getEmployeeXRiskDAO(null);
            }
        }

        public int Save(bool doCommit)
        {
            try
            {
                return edao.insert(this.EmplXRiskTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXRisk.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return edao.update(this.EmplXRiskTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXRisk.Update(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeXRisk.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeXRiskTO> SearchEmployeeXRisks()
        {           
            try
            {
                return edao.getEmployeeXRisks(this.EmplXRiskTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXRisk.SearchEmployeeXRisks(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchEmployeeXRisksCount()
        {
            try
            {
                return edao.getEmployeeXRisksCount(this.EmplXRiskTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXRisk.SearchEmployeeXRisksCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<uint, EmployeeXRiskTO> SearchEmployeeXRisks(string emplIDs, string risks, DateTime from, DateTime to)
        {
            try
            {
                return edao.getEmployeeXRisks(emplIDs, risks, from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXRisk.SearchEmployeeXRisks(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchMaxRotation()
        {
            try
            {
                return edao.getMaxRotation();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXRisk.SearchMaxRotation(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeXRiskTO> SearchEmployeeXRisksNotScheduled(string emplIDs, DateTime from, DateTime to)
        {
            try
            {
                return edao.getEmployeeXRisksNotScheduled(emplIDs, from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXVaccine.SearchEmployeeXRisksNotScheduled(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeXRisk.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeXRisk.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeXRisk.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeXRisk.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeeXRisk.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
