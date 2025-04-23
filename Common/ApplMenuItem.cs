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
	/// Summary description for ApplMenuItem.
	/// </summary>
	public class ApplMenuItem
	{
		DAOFactory daoFactory = null;
		ApplMenuItemDAO applMenuItemDAO = null;

		DebugLog log;

        ApplMenuItemTO menuItemTO = new ApplMenuItemTO();

		public ApplMenuItemTO MenuItemTO
		{
			get{ return menuItemTO; }
			set{ menuItemTO = value; }
		}
        
		public ApplMenuItem()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			applMenuItemDAO = daoFactory.getApplMenuItemDAO(null);            
		}
        public ApplMenuItem(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            applMenuItemDAO = daoFactory.getApplMenuItemDAO(dbConnection);
        }

		public ApplMenuItem(string applMenuItemID, string name, string description, string langCode, string position, int[] permitionRole)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			applMenuItemDAO = daoFactory.getApplMenuItemDAO(null);

			this.MenuItemTO.ApplMenuItemID = applMenuItemID;
			this.MenuItemTO.Name = name;
			this.MenuItemTO.Description = description;
            this.MenuItemTO.LangCode = langCode;
            this.MenuItemTO.Position = position;
			this.MenuItemTO.ArrayToPermissions(permitionRole);
		}

		public int Save(ApplMenuItemTO applMenuItemTO)
		{
			int inserted;
			try
			{
				inserted = applMenuItemDAO.insert(applMenuItemTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplMenuItem.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}


		public List<ApplMenuItemTO> Search()
		{
			try
			{
				return applMenuItemDAO.getApplMenuItems(this.MenuItemTO);				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplMenuItem.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool Update(ApplMenuItemTO applMenuItemTO)
		{
			bool isUpdated;

			try
			{
				isUpdated = applMenuItemDAO.update(applMenuItemTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplMenuItem.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

        public bool UpdateSamePosition(ApplMenuItemTO applMenuItemTO)
        {
            bool isUpdated;

            try
            {
                isUpdated = applMenuItemDAO.updateSamePosition(applMenuItemTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplMenuItem.UpdateSamePosition(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

		public bool UpdateEmptyRole(int applRoleID, bool doCommit)
		{
			bool isUpdated;

			try
			{
				isUpdated = applMenuItemDAO.updateEmptyRole(applRoleID, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplMenuItem.UpdateEmptyRole(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool Delete(string applMenuItemID)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = applMenuItemDAO.delete(applMenuItemID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplMenuItem.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public ApplMenuItemTO Find(string applMenuItemID)
		{
			ApplMenuItemTO applMenuItemTO = new ApplMenuItemTO();

			try
			{
				applMenuItemTO = applMenuItemDAO.find(applMenuItemID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplMenuItem.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return applMenuItemTO;
		}
        	
		public void Clear()
		{
			this.MenuItemTO = new ApplMenuItemTO();
		}
        
		/// <summary>
		/// Send list of ApplMenuItemTO objects to serialization.
		/// </summary>
		/// <param name="applMenuItemTOList">List of ApplMenuItemTO</param>
		private void CacheData(List<ApplMenuItemTO> applMenuItemTOList)
		{
			try
			{
				applMenuItemDAO.serialize(applMenuItemTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplMenuItem.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		
		public void CacheData()
		{
			List<ApplMenuItemTO> applMenuItemTOList = new List<ApplMenuItemTO>();

			try
			{
				applMenuItemTOList = applMenuItemDAO.getApplMenuItems(this.MenuItemTO);
				this.CacheData(applMenuItemTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplMenuItem.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		
		/// <summary>
		/// Cache all of the ApplMenuItems from database
		/// </summary>
		public void CacheAllData()
		{
			try
			{				
				this.CacheData(applMenuItemDAO.getApplMenuItems(new ApplMenuItemTO()));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplMenuItem.CacheAllData(): " + ex.Message + "\n");
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
				applMenuItemDAO = daoFactory.getApplMenuItemDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplMenuItem.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}
	}
}
