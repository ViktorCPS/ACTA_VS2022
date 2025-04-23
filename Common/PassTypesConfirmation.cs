using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DataAccess;
using Util;
using TransferObjects;

namespace Common
{
   public class PassTypesConfirmation
    {
        DAOFactory daoFactory = null;
        PassTypesConfirmationDAO dao = null;

		DebugLog log;

        PassTypesConfirmationTO ptConfirmTO = new PassTypesConfirmationTO();

        public PassTypesConfirmationTO PTConfirmTO
		{
			get { return ptConfirmTO; }
			set {ptConfirmTO = value; }
		}

		public PassTypesConfirmation()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			dao = daoFactory.getPassTypesConfirmationDAO(null);
		}
        public PassTypesConfirmation(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getPassTypesConfirmationDAO(dbConnection);
        }


        public int Save(PassTypesConfirmationTO ptConfirmTO)
		{
			int inserted;
			try
			{
                inserted = dao.insert(ptConfirmTO);
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " PassTypesConfirmation.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

        public int Save(PassTypesConfirmationTO ptcTO, bool doCommit)
		{
			int inserted;
			try
			{
				inserted = dao.insert(ptcTO,doCommit);
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " PassTypesConfirmation.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

        public List<PassTypesConfirmationTO> Search()
		{
			try
			{				
				return dao.getPTConfirmation(this.ptConfirmTO);				
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " PassTypesConfirmation.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public Dictionary<int, List<int>> SearchDictionary()
        {
            try
            {
                return dao.getPTConfirmationDictionary(this.ptConfirmTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypesConfirmation.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

	
		public bool Delete(int passTypeID, int passTypeConfirmationID)
		{
			bool isDeleted = false;

			try
			{
                isDeleted = dao.delete(passTypeID, passTypeConfirmationID);
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " PassTypesConfirmation.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

        public bool Delete(int passTypeID, int passTypeConfirmationID, bool doCommit)
		{
			bool isDeleted = false;

			try
			{
                isDeleted = dao.delete(passTypeID, passTypeConfirmationID, doCommit);
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " PassTypesConfirmation.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}
        public bool Delete(int passTypeID, bool doCommit)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = dao.delete(passTypeID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassTypesConfirmation.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

		public bool BeginTransaction()
		{
			bool isStarted = false;

			try
			{
				isStarted = dao.beginTransaction();
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " PassTypesConfirmation.BeginTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isStarted;
		}

		public void CommitTransaction()
		{
			try
			{
				dao.commitTransaction();
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " PassTypesConfirmation.CommitTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void RollbackTransaction()
		{
			try
			{
				dao.rollbackTransaction();
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " PassTypesConfirmation.RollbackTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public IDbTransaction GetTransaction()
		{
			try
			{
				return dao.getTransaction();
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " PassTypesConfirmation.GetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void SetTransaction(IDbTransaction trans)
		{
			try
			{
				dao.setTransaction(trans);
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " PassTypesConfirmation.SetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        
		public void Clear()
		{
			this.ptConfirmTO = new PassTypesConfirmationTO();
		}
	
    }
}
