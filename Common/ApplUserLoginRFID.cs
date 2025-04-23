using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class ApplUserLoginRFID
    {
        DAOFactory daoFactory = null;
		ApplUserLoginRFIDDAO dao = null;

		DebugLog log;

        ApplUserLoginRFIDTO rfidTO = new ApplUserLoginRFIDTO();

		public ApplUserLoginRFIDTO RfidTO
		{
            get { return rfidTO; }
			set{ rfidTO = value; }
		}

        public ApplUserLoginRFID()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getApplUserLoginRFIDDAO(null);
		}

        public ApplUserLoginRFID(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getApplUserLoginRFIDDAO(dbConnection);
        }

        public int Save(bool doCommit)
        {            
            try
            {
                return dao.insert(this.RfidTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserLoginRFID.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return dao.update(this.RfidTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserLoginRFID.Update(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Delete(string host, DateTime created, bool doCommit)
        {
            try
            {
                return dao.delete(host, created, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserLoginRFID.Delete(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public ApplUserLoginRFIDTO SearchLoginRFID(string host)
        {
            try
            {
                return dao.getLoginRFID(host);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserLoginRFID.SearchLoginRFID(): " + ex.Message + "\n");
                throw ex;
            }
        }        
        
        public bool BeginTransaction()
        {
            try
            {
                return dao.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserLoginRFID.BeginTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                dao.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserLoginRFID.CommitTransaction(): " + ex.Message + "\n");
                throw ex;
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
                log.writeLog(DateTime.Now + " ApplUserLoginRFID.RollbackTransaction(): " + ex.Message + "\n");
                throw ex;
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
                log.writeLog(DateTime.Now + " ApplUserLoginRFID.GetTransaction(): " + ex.Message + "\n");
                throw ex;
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
                log.writeLog(DateTime.Now + " ApplUserLoginRFID.SetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
