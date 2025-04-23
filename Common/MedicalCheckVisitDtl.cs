using System;
using System.Collections.Generic;
using System.Text;
using Util;
using System.Data;

using TransferObjects;
using DataAccess;

namespace Common
{
    public class MedicalCheckVisitDtl
    {
        DAOFactory daoFactory = null;
        MedicalCheckVisitDtlDAO edao = null;
		
		DebugLog log;

        MedicalCheckVisitDtlTO visitDtlTO = new MedicalCheckVisitDtlTO();

        public MedicalCheckVisitDtlTO VisitDtlTO
        {
            get { return visitDtlTO; }
            set { visitDtlTO = value; }
        }
        		
		public MedicalCheckVisitDtl()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getMedicalCheckVisitDtlDAO(null);
		}

        public MedicalCheckVisitDtl(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getMedicalCheckVisitDtlDAO(dbConnection);
        }

        public MedicalCheckVisitDtl(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getMedicalCheckVisitDtlDAO(null);
            }
        }

        public int Save(bool doCommit)
        {
            try
            {
                return edao.insert(this.VisitDtlTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtl.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return edao.update(this.VisitDtlTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtl.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(string recIDs, bool doCommit)
        {
            try
            {
                return edao.delete(recIDs, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtl.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<MedicalCheckVisitDtlTO> SearchMedicalCheckVisitDetails()
        {           
            try
            {
                return edao.getMedicalCheckVisitDetails(this.VisitDtlTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtl.SearchMedicalCheckVisitDetails(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<MedicalCheckVisitDtlTO> SearchPerformedVisits(string emplIDs, string type, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return edao.getPerformedVisits(emplIDs, type, fromDate, toDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtl.SearchPerformedVisits(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, Dictionary<string, List<int>>> SearchScheduledVisits(string emplIDs, string type)
        {
            try
            {
                return edao.getScheduledVisits(emplIDs, type);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtl.SearchScheduledVisits(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<MedicalCheckVisitDtlTO> SearchMedicalCheckVisitDetails(string recIDs)
        {
            try
            {
                return edao.getMedicalCheckVisitDetails(recIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtl.SearchMedicalCheckVisitDetails(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtl.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtl.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtl.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtl.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitDtl.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
