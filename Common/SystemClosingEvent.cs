using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using Util;
using TransferObjects;

namespace Common
{
    public class SystemClosingEvent
    {
        DAOFactory daoFactory = null;
        SystemClosingEventDAO dao = null;
		
		DebugLog log;

        SystemClosingEventTO eventTO = new SystemClosingEventTO();

        public SystemClosingEventTO EventTO
        {
            get { return eventTO; }
            set { eventTO = value; }
        }
        		
		public SystemClosingEvent()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			dao = daoFactory.getSystemClosingEventDAO(null);
		}

        public SystemClosingEvent(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getSystemClosingEventDAO(dbConnection);
        }

        public SystemClosingEvent(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                dao = daoFactory.getSystemClosingEventDAO(null);
            }
        }

        public int Save(bool doCommit)
        {
            try
            {
                return dao.insert(this.EventTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemClosingEvent.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return dao.update(this.EventTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemClosingEvent.Update(): " + ex.Message + "\n");
                throw ex;
            }
        }
                
        public bool Delete(int eventID, bool doCommit)
        {
            try
            {
                return dao.delete(eventID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemClosingEvent.Delete(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<SystemClosingEventTO> Search(DateTime from, DateTime to)
        {
            try
            {
                return dao.getClosingEvents(from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemClosingEvent.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<string> SearchMessages(string type)
        {
            try
            {
                return dao.getClosingEventsMessages(type);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemClosingEvent.SearchMessages(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SystemClosingEvent.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SystemClosingEvent.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SystemClosingEvent.RollbackTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SystemClosingEvent.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SystemClosingEvent.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
