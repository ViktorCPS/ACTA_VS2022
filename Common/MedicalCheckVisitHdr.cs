using System;
using System.Collections.Generic;
using System.Text;
using Util;
using System.Data;

using TransferObjects;
using DataAccess;

namespace Common
{
    public class MedicalCheckVisitHdr
    {
        DAOFactory daoFactory = null;
        MedicalCheckVisitHdrDAO edao = null;
		
		DebugLog log;

        MedicalCheckVisitHdrTO visitHdrTO = new MedicalCheckVisitHdrTO();

        public MedicalCheckVisitHdrTO VisitHdrTO
        {
            get { return visitHdrTO; }
            set { visitHdrTO = value; }
        }
        		
		public MedicalCheckVisitHdr()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getMedicalCheckVisitHdrDAO(null);
		}

        public MedicalCheckVisitHdr(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getMedicalCheckVisitHdrDAO(dbConnection);
        }

        public MedicalCheckVisitHdr(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getMedicalCheckVisitHdrDAO(null);
            }
        }

        public bool Save(bool doCommit)
        {
            try
            {
                return edao.insert(this.VisitHdrTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdr.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return edao.update(this.VisitHdrTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdr.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(string visitID, bool doCommit)
        {
            try
            {
                return edao.delete(visitID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdr.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteEmptyVisits(string visitIDs, bool doCommit)
        {
            try
            {
                return edao.deleteEmptyVisits(visitIDs, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdr.DeleteEmptyVisits(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<MedicalCheckVisitHdrTO> SearchEmptyVisits(string visitIDs)
        {
            try
            {
                return edao.getEmptyVisits(visitIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdr.SearchEmptyVisits(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<MedicalCheckVisitHdrTO> SearchMedicalCheckVisitHeaders()
        {           
            try
            {
                return edao.getMedicalCheckVisitHeaders(this.VisitHdrTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdr.SearchMedicalCheckVisitHeaders(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<MedicalCheckVisitHdrTO> SearchMedicalCheckVisits(string visitIDs)
        {
            try
            {
                return edao.getMedicalCheckVisits(visitIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdr.SearchMedicalCheckVisits(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<uint, MedicalCheckVisitHdrTO> SearchMedicalCheckVisits(string emplIDs, string status, string point, string check, string type, DateTime from, DateTime to)
        {
            try
            {
                return edao.getMedicalCheckVisits(emplIDs, status, point, check, type, from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdr.SearchMedicalCheckVisits(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdr.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdr.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdr.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdr.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckVisitHdr.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
