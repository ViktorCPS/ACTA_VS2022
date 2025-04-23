using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class PassTypeLimit
    {
        DAOFactory daoFactory = null;
		PassTypeLimitDAO limitDAO = null;

		DebugLog log;

        PassTypeLimitTO ptLimitTO = new PassTypeLimitTO();

		public PassTypeLimitTO PTLimitTO
		{
            get { return ptLimitTO; }
			set{ ptLimitTO = value; }
		}

        public PassTypeLimit()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            limitDAO = daoFactory.getPassTypeLimitDAO(null);
		}


        public PassTypeLimit(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            limitDAO = daoFactory.getPassTypeLimitDAO(dbConnection);
        }

        public int Save()
        {            
            try
            {
                return limitDAO.insert(this.PTLimitTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypeLimit.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public Dictionary<int, PassTypeLimitTO> SearchDictionary()
        {
            try
            {
                return limitDAO.getPassTypeLimitsDictionary(this.PTLimitTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypeLimit.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public List<PassTypeLimitTO> Search()
        {
            try
            {
                return limitDAO.getPassTypeLimits(this.PTLimitTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypeLimit.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Update()
        {
            try
            {
                return limitDAO.update(this.PTLimitTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypeLimit.Update(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Delete(int limitID)
        {
            try
            {
                return limitDAO.delete(limitID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypeLimit.Delete(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public PassTypeLimitTO Find(int limitID)
        {
            try
            {
                return limitDAO.find(limitID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypeLimit.Find(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool BeginTransaction()
        {
            try
            {
                return limitDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypeLimit.BeginTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                limitDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypeLimit.CommitTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                limitDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypeLimit.RollbackTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return limitDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypeLimit.GetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                limitDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypeLimit.SetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

       
    }
}
