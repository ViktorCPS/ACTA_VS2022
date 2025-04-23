
using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Util;
using TransferObjects;
using System.Data;

namespace Common
{
   public class SyncBufferDataHist
    {
         private SyncBufferDataHistDAO bdDAO;
		private DAOFactory daoFactory;

		DebugLog log;

        SyncBufferDataTO bdTO = new SyncBufferDataTO();

        public SyncBufferDataTO BDTO
        {
            get { return bdTO; }
            set { bdTO = value; }
        }

		public SyncBufferDataHist()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            bdDAO = daoFactory.getSyncBufferDataHistDAO(null);
		}


        public SyncBufferDataHist(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            bdDAO = daoFactory.getSyncBufferDataHistDAO(dbConnection);
        }
        public DateTime GetMaxDate()
        {
            try
            {
                return bdDAO.getMaxDate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncBufferDataHist.GetMaxDate(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public bool insert(bool doCommit)
        {

            try
            {
                return bdDAO.insert(BDTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncBufferDataHist.insert(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = bdDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncBufferDataHist.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                bdDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncBufferDataHist.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                bdDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncBufferDataHist.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return bdDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncBufferDataHist.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                bdDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncBufferDataHist.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

       
    }
}
