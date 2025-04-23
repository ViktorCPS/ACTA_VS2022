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
	/// Summary description for ExitPermission.
	/// </summary>
	public class ExitPermission
	{
        DAOFactory daoFactory = null;
		ExitPermissionDAO exitPermissionDAO = null;

		DebugLog log;

        private ExitPermissionTO permTO = new ExitPermissionTO();

        public ExitPermissionTO PermTO
        {
            get{ return permTO; }
            set{ permTO = value; }
        }
        
		public ExitPermission()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			exitPermissionDAO = daoFactory.getExitPermissionsDAO(null);
		}

        public ExitPermission(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            exitPermissionDAO = daoFactory.getExitPermissionsDAO(dbConnection);
        }

		public ExitPermission(int permissionID, int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, string issuedBy, DateTime issuedTime)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			exitPermissionDAO = daoFactory.getExitPermissionsDAO(null);

            this.PermTO = new ExitPermissionTO(permissionID, employeeID, passTypeID, startTime, offset, used, description, issuedBy, issuedTime);
		}

        public int Save(ExitPermissionTO permTO)
        {
            try
            {
                if (Misc.isLockedDate(permTO.StartTime.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(permTO.StartTime.Date);
                    throw new Exception(exceptionString);
                }
                
                return exitPermissionDAO.insert(permTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermission.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }

		public int Save(int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, string verifiedBy)
		{
			try
			{
                if (Misc.isLockedDate(startTime.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(startTime.Date);
                    throw new Exception(exceptionString);
                }
				
                return exitPermissionDAO.insert(employeeID, passTypeID, startTime, offset, used, description, verifiedBy);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermission.Save(): " + ex.Message + "\n");
				throw ex;
			}
		}

		public bool SaveRetroactive(int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, PassTO passTO, string verifiedBy)
		{
			try
			{
                if (Misc.isLockedDate(startTime.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(startTime.Date);
                    throw new Exception(exceptionString);
                }
				
                return exitPermissionDAO.insertRetroactive(employeeID, passTypeID, startTime, offset, used, description, passTO, daoFactory, verifiedBy);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermission.SaveRetroactive(): " + ex.Message + "\n");
				throw ex;
			}
		}


		public List<ExitPermissionTO> Search(DateTime fromTime, DateTime toTime, string wUnits)
		{
			try
			{
				return exitPermissionDAO.getExitPermissions(this.PermTO, fromTime, toTime, wUnits);
            }
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermission.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<ExitPermissionTO> SearchVerifiedBy(DateTime fromTime, DateTime toTime, string wUnits)
		{
            try
            {
                return exitPermissionDAO.getExitPermissionsVerifiedBy(this.PermTO, fromTime, toTime, wUnits);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermission.SearchVerifiedBy(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
		}

        public List<ExitPermissionTO> SearchValid(string employeeID)
		{			
			try
			{
				return exitPermissionDAO.getValidExitPermissions(employeeID);				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermission.SearchValid(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public bool Update(ExitPermissionTO permTO)
        {
            bool isUpdated;

            try
            {
                if (Misc.isLockedDate(permTO.StartTime.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(permTO.StartTime.Date);
                    throw new Exception(exceptionString);
                }
                isUpdated = exitPermissionDAO.update(permTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermission.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

		public bool Update(int permisionID, int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, string verifiedBy)
		{
			bool isUpdated;

			try
			{
                if (Misc.isLockedDate(startTime.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(startTime.Date);
                    throw new Exception(exceptionString);
                }
				isUpdated = exitPermissionDAO.update(permisionID, employeeID, passTypeID, startTime, offset, used, description, verifiedBy);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermission.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool UpdateRetroactive(int permisionID, int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, PassTO passTO, string verifiedBy)
		{
			bool isUpdated;

			try
			{
                if (Misc.isLockedDate(startTime.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(startTime.Date);
                    throw new Exception(exceptionString);
                }
				isUpdated = exitPermissionDAO.updateRetroactive(permisionID, employeeID, passTypeID, startTime, offset, used, description, passTO, daoFactory, verifiedBy);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermission.UpdateRetroactive(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool Delete(int permissionID,DateTime startTime)
		{
			bool isDeleted = false;

			try
			{
                if (Misc.isLockedDate(startTime.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(startTime.Date);
                    throw new Exception(exceptionString);
                }
				isDeleted = exitPermissionDAO.delete(permissionID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermission.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public ExitPermissionTO Find(int permissionID)
		{
			try
			{
				return exitPermissionDAO.find(permissionID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermission.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void Clear()
		{
            this.PermTO = new ExitPermissionTO();
		}

		/// <summary>
		/// Send list of ExitPermissionTO objects to serialization.
		/// </summary>
		/// <param name="exitPermissionTOList">List of ExitPermissionTO</param>
        private void CacheData(List<ExitPermissionTO> exitPermissionTOList)
		{
			try
			{
				exitPermissionDAO.serialize(exitPermissionTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermission.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		
		public void CacheData(string permissionID, string employeeID, string passTypeID, DateTime startTime, string offset, string used, string description, string issuedBy, string issuedTime, string wUnits)
		{
            List<ExitPermissionTO> exitPermissionTOList = new List<ExitPermissionTO>();

			try
			{
				exitPermissionTOList = exitPermissionDAO.getExitPermissions(this.PermTO, startTime, startTime, wUnits);
				this.CacheData(exitPermissionTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermission.CacheData(): " + ex.Message + "\n");
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
				this.CacheData(exitPermissionDAO.getExitPermissions(new ExitPermissionTO(), new DateTime(0), new DateTime(0), ""));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermission.CacheAllData(): " + ex.Message + "\n");
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
				exitPermissionDAO = daoFactory.getExitPermissionsDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermission.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}
	}
}
