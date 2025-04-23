using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using DataAccess;
using Util;
using System.Data;

namespace Common
{
    public class SyncEmployeePositionHist
    {
        private SyncEmployeePositionHistDAO posDAO;
        private DAOFactory daoFactory;

        DebugLog log;

        SyncEmployeePositionTO posTO = new SyncEmployeePositionTO();

        public SyncEmployeePositionTO PosTO
        {
            get { return posTO; }
            set { posTO = value; }
        }

        public SyncEmployeePositionHist()
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            posDAO = daoFactory.getSyncEmployeePositionHistDAO(null);
        }

        public SyncEmployeePositionHist(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            posDAO = daoFactory.getSyncEmployeePositionHistDAO(dbConnection);
        }

        public bool insert(bool doCommit)
        {
            try
            {
                return posDAO.insert(PosTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncEmployeePositionHist.insert(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<SyncEmployeePositionTO> Search(DateTime from, DateTime to, int posID, int result)
        {
            try
            {
                return posDAO.getPositions(from, to, posID, result);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncEmployeePositionHist.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = posDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncEmployeePositionHist.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                posDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncEmployeePositionHist.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                posDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncEmployeePositionHist.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return posDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncEmployeePositionHist.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                posDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SyncEmployeePositionHist.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
