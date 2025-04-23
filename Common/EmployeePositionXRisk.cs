using System;
using System.Collections.Generic;
using System.Text;
using Util;
using System.Data;

using TransferObjects;
using DataAccess;

namespace Common
{
    public class EmployeePositionXRisk
    {
        DAOFactory daoFactory = null;
        EmployeePositionXRiskDAO edao = null;
		
		DebugLog log;

        EmployeePositionXRiskTO emplXRiskTO = new EmployeePositionXRiskTO();

        public EmployeePositionXRiskTO EmplPositionXRiskTO
        {
            get { return emplXRiskTO; }
            set { emplXRiskTO = value; }
        }
        		
		public EmployeePositionXRisk()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getEmployeePositionXRiskDAO(null);
		}

        public EmployeePositionXRisk(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeePositionXRiskDAO(dbConnection);
        }

        public EmployeePositionXRisk(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getEmployeePositionXRiskDAO(null);
            }
        }

        public int Save(bool doCommit)
        {
            try
            {
                return edao.insert(this.EmplPositionXRiskTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePositionXRisk.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return edao.update(this.EmplPositionXRiskTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePositionXRisk.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(int posID, int riskID, bool doCommit)
        {
            try
            {
                return edao.delete(posID, riskID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePositionXRisk.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeePositionXRiskTO> SearchEmployeePositionXRisks()
        {           
            try
            {
                return edao.getEmployeePositionXRisks(this.EmplPositionXRiskTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePositionXRisk.SearchEmployeePositionXRisks(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<EmployeePositionXRiskTO> SearchEmployeePositionXRisksByWU(int wu)
        {
            try
            {
                return edao.getEmployeePositionXRisksByWU(this.EmplPositionXRiskTO,wu);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePositionXRisk.SearchEmployeePositionXRisks(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<RiskTO> SearchRisks(string posIDs)
        {
            try
            {
                return edao.getRisks(posIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePositionXRisk.SearchRisks(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePositionXRisk.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePositionXRisk.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePositionXRisk.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePositionXRisk.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeePositionXRisk.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
