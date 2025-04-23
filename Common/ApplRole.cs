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
	/// Summary description for ApplRole.
	/// </summary>
	public class ApplRole
	{		
		DAOFactory daoFactory = null;
		ApplRoleDAO applRoleDAO = null;

		DebugLog log;

        ApplRoleTO roleTO = new ApplRoleTO();

		public ApplRoleTO RoleTO
		{
			get{ return roleTO; }
			set{ roleTO = value; }
		}
        
		public ApplRole()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			applRoleDAO = daoFactory.getApplRoleDAO(null);
		}
        public ApplRole(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            applRoleDAO = daoFactory.getApplRoleDAO(dbConnection);
        }

		public ApplRole(int applRoleID, string name, string description)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			applRoleDAO = daoFactory.getApplRoleDAO(null);

			this.RoleTO.ApplRoleID = applRoleID;
			this.RoleTO.Name = name;
			this.RoleTO.Description = description;
		}

		public int Save(int applRoleID, string name, string description)
		{
			int inserted;
			try
			{
				inserted = applRoleDAO.insert(applRoleID, name, description);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRole.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}
        
		public List<ApplRoleTO> Search()
		{
			try
			{
				return applRoleDAO.getApplRoles(this.RoleTO);				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRole.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public List<ApplRoleTO> SearchUserCreatedRoles()
		{
			try
			{
				return applRoleDAO.getUserCreatedRoles();				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRole.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool Update(int applRoleID, string name, string description)
		{
			bool isUpdated;

			try
			{
				isUpdated = applRoleDAO.update(applRoleID, name, description);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRole.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool UpdateOnEmptyRole(int applRoleID)
		{
			bool isUpdated;

			try
			{
				isUpdated = applRoleDAO.updateOnEmptyRole(applRoleID, daoFactory);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRole.UpdateOnEmptyRole(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool Delete(int applRoleID)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = applRoleDAO.delete(applRoleID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRole.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public ApplRoleTO Find(int applRoleID)
		{
			ApplRoleTO applRoleTO = new ApplRoleTO();

			try
			{
				applRoleTO = applRoleDAO.find(applRoleID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRole.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return applRoleTO;
		}

		public int FindEmptyRole()
		{
			int applRoleID = 0;

			try
			{
				applRoleID = applRoleDAO.findEmptyRole();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRole.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return applRoleID;
		}
        
		public void Clear()
		{
			this.RoleTO = new ApplRoleTO();
		}

		/// <summary>
		/// Send list of ApplRoleTO objects to serialization.
		/// </summary>
		/// <param name="locatioTOList">List of LocationTO</param>
		private void CacheData(List<ApplRoleTO> applRoleTOList)
		{
			try
			{
				applRoleDAO.serialize(applRoleTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRole.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}		
		
		public void CacheData()
		{
			List<ApplRoleTO> applRoleTOList = new List<ApplRoleTO>();

			try
			{
				applRoleTOList = applRoleDAO.getApplRoles(this.RoleTO);
				this.CacheData(applRoleTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRole.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}		
		
		/// <summary>
		/// Cache all of the ApplRoles from database
		/// </summary>
		public void CacheAllData()
		{
			try
			{
				this.CacheData(applRoleDAO.getApplRoles(new ApplRoleTO()));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRole.CacheAllData(): " + ex.Message + "\n");
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
				applRoleDAO = daoFactory.getApplRoleDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplRole.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}
	}
}
