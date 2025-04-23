using System;
using System.Collections.Generic;
using System.Text;
using Util;
using System.Data;

using TransferObjects;
using DataAccess;

namespace Common
{
    public class MedicalCheckPoint
    {
        DAOFactory daoFactory = null;
        MedicalCheckPointDAO edao = null;
		
		DebugLog log;

        MedicalCheckPointTO pointTO = new MedicalCheckPointTO();

        public MedicalCheckPointTO PointTO
        {
            get { return pointTO; }
            set { pointTO = value; }
        }
        		
		public MedicalCheckPoint()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getMedicalCheckPointDAO(null);
		}

        public MedicalCheckPoint(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getMedicalCheckPointDAO(dbConnection);
        }

        public MedicalCheckPoint(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getMedicalCheckPointDAO(null);
            }
        }

        public int Save(bool doCommit)
        {
            try
            {
                return edao.insert(this.PointTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckPoint.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return edao.update(this.PointTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckPoint.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(int pointID, bool doCommit)
        {
            try
            {
                return edao.delete(pointID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckPoint.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<MedicalCheckPointTO> SearchMedicalCheckPoints()
        {           
            try
            {
                return edao.getMedicalCheckPoints(this.PointTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckPoint.SearchMedicalCheckPoints(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, MedicalCheckPointTO> SearchMedicalCheckPointsDictionary()
        {
            try
            {
                return edao.getMedicalCheckPointsDictionary(this.PointTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MedicalCheckPoint.SearchMedicalCheckPointsDictionary(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckPoint.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckPoint.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckPoint.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckPoint.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MedicalCheckPoint.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
