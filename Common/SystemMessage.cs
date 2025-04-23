using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using Util;
using TransferObjects;

namespace Common
{
    public class SystemMessage
    {
        DAOFactory daoFactory = null;
        SystemMessageDAO dao = null;
		
		DebugLog log;

        SystemMessageTO msgTO = new SystemMessageTO();

        public SystemMessageTO MessageTO
        {
            get { return msgTO; }
            set { msgTO = value; }
        }
        		
		public SystemMessage()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			dao = daoFactory.getSystemMessageDAO(null);
		}

        public SystemMessage(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getSystemMessageDAO(dbConnection);
        }

        public SystemMessage(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                dao = daoFactory.getSystemMessageDAO(null);
            }
        }

        public int Save(bool doCommit)
        {
            try
            {
                return dao.insert(this.MessageTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessage.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return dao.update(this.MessageTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessage.Update(): " + ex.Message + "\n");
                throw ex;
            }
        }
                
        public bool Delete(int msgID, bool doCommit)
        {
            try
            {
                return dao.delete(msgID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessage.Delete(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<SystemMessageTO> Search(DateTime from, DateTime to, string company, int role)
        {
            try
            {
                return dao.getSystemMessages(from, to, company, role);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessage.Delete(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = dao.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessage.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                dao.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessage.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                dao.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessage.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return dao.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessage.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                dao.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessage.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
