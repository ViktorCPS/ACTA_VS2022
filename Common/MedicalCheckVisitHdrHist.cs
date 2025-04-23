using System;
using System.Collections.Generic;
using System.Text;
using Util;
using System.Data;

using TransferObjects;
using DataAccess;

namespace Common
{
    public class MedicalCheckVisitHdrHist
    {
        DAOFactory daoFactory = null;
        MedicalCheckVisitHdrHistDAO edao = null;
		
		DebugLog log;

        MedicalCheckVisitHdrHistTO visitHdrHistTO = new MedicalCheckVisitHdrHistTO();

        public MedicalCheckVisitHdrHistTO VisitHdrHistTO
        {
            get { return visitHdrHistTO; }
            set { visitHdrHistTO = value; }
        }
        		
		public MedicalCheckVisitHdrHist()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getMedicalCheckVisitHdrHistDAO(null);
		}

        public MedicalCheckVisitHdrHist(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getMedicalCheckVisitHdrHistDAO(dbConnection);
        }

        public MedicalCheckVisitHdrHist(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getMedicalCheckVisitHdrHistDAO(null);
            }
        }

        public bool Save(bool doCommit)
        {
            try
            {
                return edao.insert(this.VisitHdrHistTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdrHist.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return edao.update(this.VisitHdrHistTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdrHist.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(uint recID, bool doCommit)
        {
            try
            {
                return edao.delete(recID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdrHist.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<MedicalCheckVisitHdrHistTO> SearchMedicalCheckVisitHeadersHistory()
        {           
            try
            {
                return edao.getMedicalCheckVisitHeadersHistory(this.VisitHdrHistTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdrHist.SearchMedicalCheckVisitHeadersHistory(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public DataTable SearchMedicalCheckVisitHeadersHistory(uint visitID)
        {
            try
            {
                return edao.getMedicalCheckVisitHeadersHistory(visitID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdrHist.SearchMedicalCheckVisitHeadersHistory(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<uint> SearchMedicalCheckVisitHeadersHistory(string visitIDs)
        {
            try
            {
                return edao.getMedicalCheckVisitHeadersHistory(visitIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdrHist.SearchMedicalCheckVisitHeadersHistory(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = edao.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdrHist.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                edao.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdrHist.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdrHist.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdrHist.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdrHist.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
