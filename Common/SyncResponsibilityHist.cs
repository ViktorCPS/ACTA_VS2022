using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Util;
using TransferObjects;
using System.Data;

namespace Common
{
    public  class SyncResponsibilityHist
    {
        private SyncResponsibilityHistDAO osDAO;
		private DAOFactory daoFactory;

		DebugLog log;

        SyncResponsibilityTO responsibilityTO = new SyncResponsibilityTO();

        public SyncResponsibilityTO ResponsibilityTO
        {
            get { return responsibilityTO; }
            set { responsibilityTO = value; }
        }

		public SyncResponsibilityHist()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			osDAO = daoFactory.getSyncResponsibilityHistDAO(null);
		}


        public SyncResponsibilityHist(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            osDAO = daoFactory.getSyncResponsibilityHistDAO(dbConnection);
        }

        public bool insert(bool doCommit)
        {
            try
            {
                return osDAO.insert(ResponsibilityTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncResponsibilityHist.insert(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<SyncResponsibilityTO> Search(DateTime from, DateTime to, int resID, int result)
        {
            try
            {
                return osDAO.getResponsibilities(from, to, resID, result);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncResponsibilityHist.Search(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SyncResponsibilityHist.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SyncResponsibilityHist.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SyncResponsibilityHist.RollbackTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SyncResponsibilityHist.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SyncResponsibilityHist.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
