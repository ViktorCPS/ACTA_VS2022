using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using DataAccess;
using Util;
using System.Data;

namespace Common
{
    public class SyncOrganizationalStructureHist
    {
        private SyncOrganizationalStructureHistDAO osDAO;
		private DAOFactory daoFactory;

		DebugLog log;

        SyncOrganizationalStructureTO osTO = new SyncOrganizationalStructureTO();

        public SyncOrganizationalStructureTO OSTO
        {
            get { return osTO; }
            set { osTO = value; }
        }

		public SyncOrganizationalStructureHist()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			osDAO = daoFactory.getSyncOrganizationalStructureHistDAO(null);
		}


        public SyncOrganizationalStructureHist(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            osDAO = daoFactory.getSyncOrganizationalStructureHistDAO(dbConnection);
        }

        public bool insert(bool doCommit)
        {
            try
            {
                return osDAO.insert(OSTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncOrganizationalStructureHist.insert(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<SyncOrganizationalStructureTO> Search(DateTime from, DateTime to, int ouID, int result)
        {
            try
            {
                return osDAO.getOU(from, to, ouID, result);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncOrganizationalStructureHist.Search(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SyncOrganizationalStructureHist.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SyncOrganizationalStructureHist.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SyncOrganizationalStructureHist.RollbackTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SyncOrganizationalStructureHist.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SyncOrganizationalStructureHist.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
