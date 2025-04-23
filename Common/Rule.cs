using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using TransferObjects;
using Util;
using System.Data;

namespace Common
{
   public class Rule
    {
         DAOFactory daoFactory = null;
		RuleDAO rdao = null;
		
		DebugLog log;

        RuleTO ruleTO = new RuleTO();

        public RuleTO RuleTO
        {
            get { return ruleTO; }
            set { ruleTO = value; }
        }
        		
		public Rule()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			rdao = daoFactory.getRuleDAO(null);
		}

        public Rule(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            rdao = daoFactory.getRuleDAO(dbConnection);
        }

        public Rule(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                rdao = daoFactory.getRuleDAO(null);
            }
        }

        public int Save(bool doCommit)
        {
            int isUpdated = 0;

            try
            {
                isUpdated = rdao.insert(this.RuleTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool Update(bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                isUpdated = rdao.update(this.RuleTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool Update(int company, string type, int value, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                isUpdated = rdao.update(company, type, value, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> SearchWUEmplTypeDictionary(IDbTransaction trans)
        {
            try
            {
                return rdao.SearchWUEmplTypeDictionary(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.SearchWUEmplTypeDictionary(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> SearchWUEmplTypeDictionary()
        {

            try
            {
                return rdao.SearchWUEmplTypeDictionary();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.SearchWUEmplTypeDictionary(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> SearchTypeAllRules(string type)
        {
            try
            {
                return rdao.getTypeAllRules(type);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.SearchTypeAllRules(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<RuleTO> Search()
        {           
            try
            {
                return rdao.search(this.RuleTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<string> SearchRuleTypes()
        {
            try
            {
                return rdao.getRuleTypes();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.SearchRuleTypes(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<int> SearchRules(string type)
        {
            try
            {
                return rdao.getRules(type);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.SearchRules(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<int> SearchRulesExact(string type)
        {
            try
            {
                return rdao.getRulesExact(type);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.SearchRulesExact(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(int recID)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = rdao.delete(recID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }


        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = rdao.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                rdao.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                rdao.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return rdao.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                rdao.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        

        public int SearchReaderForRestaurant(string ruleType)
        {
            int reader_id = 0;
            try
            {
                reader_id = rdao.searchReaderForRestaurant(ruleType);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Rule.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return reader_id;
        }
    }
}
