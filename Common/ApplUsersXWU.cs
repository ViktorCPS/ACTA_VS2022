using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// Summary description for ApplUsersXWU.
	/// </summary>
	public class ApplUsersXWU
	{
		DAOFactory daoFactory = null;
		ApplUsersXWUDAO applUsersXWUDAO = null;

		DebugLog log;

        ApplUsersXWUTO auXwuTO = new ApplUsersXWUTO();

        public ApplUsersXWUTO AuXWUnitTO
		{
			get { return auXwuTO; }
			set {auXwuTO = value; }
		}

		public ApplUsersXWU()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			applUsersXWUDAO = daoFactory.getApplUsersXWUDAO(null);
		}

        public ApplUsersXWU(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            applUsersXWUDAO = daoFactory.getApplUsersXWUDAO(dbConnection);
        }

		public ApplUsersXWU(string userID, int wuID, string purpose)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			applUsersXWUDAO = daoFactory.getApplUsersXWUDAO(null);

			this.AuXWUnitTO.UserID = userID;
			this.AuXWUnitTO.WorkingUnitID = wuID;
			this.AuXWUnitTO.Purpose = purpose;
		}

		public int Save(string userID, int wuID, string purpose)
		{
			int inserted;
			try
			{
				inserted = applUsersXWUDAO.insert(userID, wuID, purpose);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

		public int Save(string userID, int wuID, string purpose, bool doCommit)
		{
			int inserted;
			try
			{
				inserted = applUsersXWUDAO.insert(userID, wuID, purpose, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

        public List<ApplUsersXWUTO> Search()
		{
			try
			{				
				return applUsersXWUDAO.getApplUsersXWU(this.AuXWUnitTO);				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool Update(string userID, int wuID, string purpose)
		{
			bool isUpdated;

			try
			{
				isUpdated = applUsersXWUDAO.update(userID, wuID, purpose);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool Delete(string userID, int wuID, string purpose)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = applUsersXWUDAO.delete(userID, wuID, purpose);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public bool Delete(string userID, int wuID, string purpose, bool doCommit)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = applUsersXWUDAO.delete(userID, wuID, purpose, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public bool Delete(int wuID, bool doCommit)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = applUsersXWUDAO.delete(wuID, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public ApplUsersXWUTO Find(string userID, int wuID, string purpose)
		{
			ApplUsersXWUTO applUsersXWUTO = new ApplUsersXWUTO();

			try
			{
				applUsersXWUTO = applUsersXWUDAO.find(userID, wuID, purpose);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return applUsersXWUTO;
		}

		public List<WorkingUnitTO> FindWUForUser(string userID, string purpose)
		{			
			try
			{
				return applUsersXWUDAO.findWUForUserID(userID, purpose, daoFactory);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.FindWUForUser(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<WorkingUnitTO> FindWUForUser(string userID, string purpose, IDbTransaction trans)
        {
            try
            {
                return applUsersXWUDAO.findWUForUser(userID, purpose, daoFactory, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersXWU.FindWUForUser(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int,WorkingUnitTO> FindWUForUserDictionary(string userID, string purpose)
        {
            try
            {
                return applUsersXWUDAO.findWUForUserIDDictionary(userID, purpose, daoFactory);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersXWU.FindWUForUserDictionary(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<WorkingUnitTO> FindWUForUser(string userID, string purpose, bool createDAO)
        {            
            try
            {
                return applUsersXWUDAO.findWUForUserID(userID, purpose, daoFactory);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersXWU.FindWUForUser(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public List<ApplUserTO> FindUsersForWU(int wuID, string purpose, List<string> statuses)
		{
			try
			{
				return applUsersXWUDAO.findUsersForWUID(wuID, purpose, statuses);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.FindUsersForWU(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<ApplUsersXWUTO> FindGrantedParentWUForWU()
		{
			try
			{				
				return applUsersXWUDAO.findGrantedParentWUForWU(this.AuXWUnitTO);				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.FindGrantedParentWUForWU(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool BeginTransaction()
		{
			bool isStarted = false;

			try
			{
				isStarted = applUsersXWUDAO.beginTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.BeginTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isStarted;
		}

		public void CommitTransaction()
		{
			try
			{
				applUsersXWUDAO.commitTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.CommitTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void RollbackTransaction()
		{
			try
			{
				applUsersXWUDAO.rollbackTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.RollbackTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public IDbTransaction GetTransaction()
		{
			try
			{
				return applUsersXWUDAO.getTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.GetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void SetTransaction(IDbTransaction trans)
		{
			try
			{
				applUsersXWUDAO.setTransaction(trans);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.SetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        
		public void Clear()
		{
			this.AuXWUnitTO = new ApplUsersXWUTO();
		}

		
		/// <summary>
		/// Send list of ApplUsersXWUTO objects to serialization.
		/// </summary>
		/// <param name="locatioTOList">List of LocationTO</param>
        private void CacheData(List<ApplUsersXWUTO> applUsersXWUTOList)
		{
			try
			{
				applUsersXWUDAO.serialize(applUsersXWUTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		public void CacheData()
		{
            List<ApplUsersXWUTO> applUsersXWUTOList = new List<ApplUsersXWUTO>();

			try
			{
				applUsersXWUTOList = applUsersXWUDAO.getApplUsersXWU(this.AuXWUnitTO);
				this.CacheData(applUsersXWUTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		
		/// <summary>
		/// Cache all of the Gates from database
		/// </summary>
		public void CacheAllData()
		{
			try
			{
				this.CacheData(applUsersXWUDAO.getApplUsersXWU(new ApplUsersXWUTO()));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.CacheAllData(): " + ex.Message + "\n");
				throw ex;
			}
		}

		/// <summary>
		/// Change DAO, start to use XML data source
		/// </summary>
		public void SwitchToXMLDAO()
		{
			try
			{
                daoFactory = DAOFactory.getDAOFactory(Constants.XMLDataProvider);
				applUsersXWUDAO = daoFactory.getApplUsersXWUDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXWU.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}
	}
}
