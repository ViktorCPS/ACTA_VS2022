using System;
using System.Collections.Generic;
using System.Text;
using Util;
using System.Data;

using TransferObjects;
using DataAccess;

namespace Common
{
    public class MedicalCheckVisitDtlHist
    {
        DAOFactory daoFactory = null;
        MedicalCheckVisitDtlHistDAO edao = null;
		
		DebugLog log;

        MedicalCheckVisitDtlHistTO visitDtlHistTO = new MedicalCheckVisitDtlHistTO();

        public MedicalCheckVisitDtlHistTO VisitDtlHistTO
        {
            get { return visitDtlHistTO; }
            set { visitDtlHistTO = value; }
        }
        		
		public MedicalCheckVisitDtlHist()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getMedicalCheckVisitDtlHistDAO(null);
		}

        public MedicalCheckVisitDtlHist(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getMedicalCheckVisitDtlHistDAO(dbConnection);
        }

        public MedicalCheckVisitDtlHist(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getMedicalCheckVisitDtlHistDAO(null);
            }
        }

        public int Save(bool doCommit)
        {
            try
            {
                return edao.insert(this.VisitDtlHistTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtlHist.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return edao.update(this.VisitDtlHistTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtlHist.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(uint recIDHist, bool doCommit)
        {
            try
            {
                return edao.delete(recIDHist, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtlHist.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<MedicalCheckVisitDtlHistTO> SearchMedicalCheckVisitDetailsHistory()
        {           
            try
            {
                return edao.getMedicalCheckVisitDetailsHistory(this.VisitDtlHistTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtlHist.SearchMedicalCheckVisitDetailsHistory(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public DataTable SearchMedicalCheckVisitDetailsHistory(string recIDs)
        {
            try
            {
                return edao.getMedicalCheckVisitDetailsHistory(recIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtlHist.SearchMedicalCheckVisitHistDetails(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtlHist.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtlHist.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtlHist.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtlHist.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtlHist.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
