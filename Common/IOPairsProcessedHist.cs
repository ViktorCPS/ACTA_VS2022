using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using DataAccess;
using Util;
using System.Data;

namespace Common
{
    public class IOPairsProcessedHist
    {
        DAOFactory daoFactory = null;
		IOPairsProcessedHistDAO edao = null;
		
		DebugLog log;

        IOPairsProcessedHistTO pairTO = new IOPairsProcessedHistTO();

        public IOPairsProcessedHistTO IOPairProcessedHistTO
        {
            get { return pairTO; }
            set { pairTO = value; }
        }
        		
		public IOPairsProcessedHist()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getIOPairsProcessedHistDAO(null);
		}

        public IOPairsProcessedHist(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getIOPairsProcessedHistDAO(dbConnection);
        }

        public IOPairsProcessedHist(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getIOPairsProcessedHistDAO(null);
            }
        }

        public uint Save(bool doCommit)
        {
            uint isUpdated = 0;

            try
            {
                isUpdated = edao.insert(this.IOPairProcessedHistTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairProcessedHist.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public int Save(int emplID, string modifiedBy, DateTime modifiedTime, DateTime date, int alertStatus, bool doCommit)
        {
            try
            {
                return edao.insert(emplID, modifiedBy, modifiedTime, date, alertStatus, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairProcessedHist.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int Save(string recIDs, string modifiedBy, DateTime modifiedTime, int alertStatus, bool doCommit)
        {
            try
            {
                return edao.insert(recIDs, modifiedBy, modifiedTime, alertStatus, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairProcessedHist.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IOPairsProcessedHistTO Find(uint recID)
        {
            try
            {
                return edao.find(recID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairProcessedHist.Find(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public Dictionary<int, Dictionary<DateTime, Dictionary<string, List<IOPairsProcessedHistTO>>>> SearchAllPairsForEmpl(string employeeIDString, List<DateTime> datesList)
        {
            try
            {
                return edao.getIOPairsAllForEmpl(employeeIDString, datesList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairProcessedHist.SearchAllPairsForEmpl(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<IOPairsProcessedHistTO> Search()
        {
            try
            {
                return edao.search(this.IOPairProcessedHistTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairProcessedHist.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<DateTime, List<IOPairsProcessedHistTO>> SearchIOPairsSet(int emplID, DateTime date)
        {
            try
            {
                return edao.getIOPairsSet(emplID, date);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairProcessedHist.SearchIOPairsSet(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool IsAlert(int emplID, DateTime date)
        {
            try
            {
                return edao.isAlert(emplID, date);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairProcessedHist.IsAlert(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public Dictionary<int, Dictionary<DateTime, int>> SearchAlerts(string emplIDs, DateTime from, DateTime to)
        {
            try
            {
                return edao.getAlerts(emplIDs, from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairProcessedHist.SearchAlerts(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                edao.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPairProcessedHist.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " IOPairProcessedHist.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " IOPairProcessedHist.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " IOPairProcessedHist.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
