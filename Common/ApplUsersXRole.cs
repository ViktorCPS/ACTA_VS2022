using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// Summary description for ApplUsersXRole.
	/// </summary>
	public class ApplUsersXRole
	{		
		DAOFactory daoFactory = null;
		ApplUsersXRoleDAO applUsersXRoleDAO = null;

		DebugLog log;

        ApplUsersXRoleTO auXRoleTO = new ApplUsersXRoleTO();

        public ApplUsersXRoleTO AuXRoleTO
		{
			get { return auXRoleTO; }
			set {auXRoleTO = value; }
		}
        
		public ApplUsersXRole()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			applUsersXRoleDAO = daoFactory.getApplUsersXRoleDAO(null);
		}
        public ApplUsersXRole(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            applUsersXRoleDAO = daoFactory.getApplUsersXRoleDAO(dbConnection);
        }		
		public ApplUsersXRole(string userID, int roleID)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			applUsersXRoleDAO = daoFactory.getApplUsersXRoleDAO(null);

			this.AuXRoleTO.UserID = userID;
			this.AuXRoleTO.RoleID = roleID;
		}

		public int Save(string userID, int roleID)
		{
			int inserted;
			try
			{
				inserted = applUsersXRoleDAO.insert(userID, roleID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRole.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

		public int Save(string userID, int roleID, bool doCommit)
		{
			int inserted;
			try
			{
				inserted = applUsersXRoleDAO.insert(userID, roleID, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRole.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

		public List<ApplUsersXRoleTO> Search()
		{
			try
			{
				return applUsersXRoleDAO.getApplUsersXRoles(this.AuXRoleTO);				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRole.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool Update(string userID, int roleID)
		{
			bool isUpdated;

			try
			{
				isUpdated = applUsersXRoleDAO.update(userID, roleID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRole.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool Delete(string userID, int roleID)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = applUsersXRoleDAO.delete(userID, roleID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRole.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public bool Delete(string userID, int roleID, bool doCommit)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = applUsersXRoleDAO.delete(userID, roleID, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRole.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public ApplUsersXRoleTO Find(string userID, int roleID)
		{
			ApplUsersXRoleTO applUsersXRoleTO = new ApplUsersXRoleTO();

			try
			{
				applUsersXRoleTO = applUsersXRoleDAO.find(userID, roleID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRole.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return applUsersXRoleTO;
		}

		public List<ApplRoleTO> FindRolesForUser(string userID)
		{
			try
			{				
				return applUsersXRoleDAO.findRolesForUserID(userID, daoFactory);				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRole.FindRolesForUser(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<ApplUserTO> FindUsersForRoleID(int roleID)
		{			
			try
			{				
				return applUsersXRoleDAO.findUsersForRoleID(roleID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRole.FindUsersForRoleID(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool BeginTransaction()
		{
			bool isStarted = false;

			try
			{
				isStarted = applUsersXRoleDAO.beginTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRole.BeginTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isStarted;
		}

		public void CommitTransaction()
		{
			try
			{
				applUsersXRoleDAO.commitTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRole.CommitTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        
		public void Clear()
		{
			this.AuXRoleTO = new ApplUsersXRoleTO();
		}
		
		/// <summary>
		/// Send list of ApplUsersXRoleTO objects to serialization.
		/// </summary>
		/// <param name="applUsersXRoleTOList">List of ApplUsersXRoleTO</param>
		private void CacheData(List<ApplUsersXRoleTO> applUsersXRoleTOList)
		{
			try
			{
				applUsersXRoleDAO.serialize(applUsersXRoleTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRole.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		public void CacheData()
		{
            List<ApplUsersXRoleTO> applUsersXRoleTOList = new List<ApplUsersXRoleTO>();

			try
			{
				applUsersXRoleTOList = applUsersXRoleDAO.getApplUsersXRoles(this.AuXRoleTO);
				this.CacheData(applUsersXRoleTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRole.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		
		/// <summary>
		/// Cache all of the ApplUsersXRoles from database
		/// </summary>
		public void CacheAllData()
		{
			try
			{
				this.CacheData(applUsersXRoleDAO.getApplUsersXRoles(new ApplUsersXRoleTO()));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRole.CacheAllData(): " + ex.Message + "\n");
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
				applUsersXRoleDAO = daoFactory.getApplUsersXRoleDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXRole.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}
	}
}
