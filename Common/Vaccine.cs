using System;
using System.Collections.Generic;
using System.Text;
using Util;
using System.Data;

using TransferObjects;
using DataAccess;

namespace Common
{
    public class Vaccine
    {
        DAOFactory daoFactory = null;
        VaccineDAO edao = null;
		
		DebugLog log;

        VaccineTO vaccineTO = new VaccineTO();

        public VaccineTO VaccineTO
        {
            get { return vaccineTO; }
            set { vaccineTO = value; }
        }
        		
		public Vaccine()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getVaccineDAO(null);
		}

        public Vaccine(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getVaccineDAO(dbConnection);
        }

        public Vaccine(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getVaccineDAO(null);
            }
        }

        public int Save(bool doCommit)
        {
            try
            {
                return edao.insert(this.VaccineTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Vaccine.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return edao.update(this.VaccineTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Vaccine.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(int vacID, bool doCommit)
        {
            try
            {
                return edao.delete(vacID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Vaccine.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<VaccineTO> SearchVaccines()
        {           
            try
            {
                return edao.getVaccines(this.VaccineTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Vaccine.SearchVaccines(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, VaccineTO> SearchVaccinesDictionary()
        {
            try
            {
                return edao.getVaccinesDictionary(this.VaccineTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Vaccine.SearchVaccinesDictionary(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Vaccine.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Vaccine.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Vaccine.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Vaccine.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Vaccine.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
