using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Util;
using DataAccess;
using TransferObjects;

namespace Common
{
    /// <summary>
    /// Summary description for Licence.
    /// </summary>
    public class Licence
    {       
		DAOFactory daoFactory = null;
		LicenceDAO licenceDAO = null;

		DebugLog log;

        LicenceTO licTO = new LicenceTO();

		public LicenceTO LicTO
		{
			get{ return licTO; }
			set{ licTO = value; }
		}
        
		public Licence()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			licenceDAO = daoFactory.getLicenceDAO(null);
		}
        public Licence(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            licenceDAO = daoFactory.getLicenceDAO(dbConnection);
        }

		public Licence(int recID, string licenceKey)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			licenceDAO = daoFactory.getLicenceDAO(null);

			this.LicTO.RecID = recID;
			this.LicTO.LicenceKey = licenceKey;
		}

		public int Save(string licenceKey, bool doCommit)
		{
			int inserted;
			try
			{
				inserted = licenceDAO.insert(licenceKey, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Licence.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}


		public List<LicenceTO> Search()
		{
			try
			{
                return licenceDAO.getLicences(this.LicTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Licence.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool Update(int recID, string licenceKey, bool doCommit)
		{
			bool isUpdated;

			try
			{
				isUpdated = licenceDAO.update(recID, licenceKey, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Licence.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool Delete(string licenceKey)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = licenceDAO.delete(licenceKey);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Licence.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public LicenceTO FindMAX()
		{
            LicenceTO licenceTO = new LicenceTO();

			try
			{
                licenceTO = licenceDAO.findMAX();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Licence.FindMAX(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

            return licenceTO;
		}
        
        public void Clear()
        {
            this.LicTO = new LicenceTO();
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = licenceDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Licence.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                licenceDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Licence.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                licenceDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Licence.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return licenceDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Licence.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                licenceDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Licence.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
