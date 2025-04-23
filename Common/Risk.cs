using System;
using System.Collections.Generic;
using System.Text;
using Util;
using System.Data;

using TransferObjects;
using DataAccess;

namespace Common
{
    public class Risk
    {
        DAOFactory daoFactory = null;
        RiskDAO edao = null;
		
		DebugLog log;

        RiskTO riskTO = new RiskTO();

        public RiskTO RiskTO
        {
            get { return riskTO; }
            set { riskTO = value; }
        }
        		
		public Risk()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getRiskDAO(null);
		}

        public Risk(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getRiskDAO(dbConnection);
        }

        public Risk(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getRiskDAO(null);
            }
        }

        public int Save(bool doCommit)
        {
            try
            {
                return edao.insert(this.RiskTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Risk.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return edao.update(this.RiskTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Risk.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(int riskID, bool doCommit)
        {
            try
            {
                return edao.delete(riskID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Risk.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<RiskTO> SearchRisks()
        {           
            try
            {
                return edao.getRisks(this.RiskTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Risk.SearchRisks(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, RiskTO> SearchRisksDictionary()
        {
            try
            {
                return edao.getRisksDictionary(this.RiskTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Risk.SearchRisksDictionary(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Risk.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Risk.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Risk.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Risk.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Risk.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
