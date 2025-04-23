using System;
using System.Collections;
using System.Configuration;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// Summary description for EmployeeGroupAccessControl.
	/// </summary>
	public class EmployeeGroupAccessControl //: ISerializable 
	{
		private int     _accessGroupId = -1;
		private string  _name = "";
		private string  _description = "";

		DAOFactory daoFactory = null;
		EmployeeGroupAccessControlDAO employeeGroupAccessControlDAO = null;

		DebugLog log;

		public int AccessGroupId
		{
			get { return _accessGroupId; }
			set { _accessGroupId = value; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		public EmployeeGroupAccessControl()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			employeeGroupAccessControlDAO = daoFactory.getEmployeeGroupAccessControlDAO(null);
		}

        public EmployeeGroupAccessControl(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            employeeGroupAccessControlDAO = daoFactory.getEmployeeGroupAccessControlDAO(dbConnection);
        }

		public EmployeeGroupAccessControl(int accessGroupId, string name, string description)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			this.AccessGroupId = accessGroupId;
			this.Name          = name;
			this.Description   = description;

			daoFactory = DAOFactory.getDAOFactory();
			employeeGroupAccessControlDAO = daoFactory.getEmployeeGroupAccessControlDAO(null);			
		}

		public int Save(string name, string description)
		{
			int inserted;
			try
			{
				inserted = employeeGroupAccessControlDAO.insert(name, description);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeGroupAccessControl.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

		public bool Update(string accessGroupId, string name, string description)
		{
			bool isUpdated;

			try
			{
				isUpdated = employeeGroupAccessControlDAO.update(accessGroupId, name, description);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeGroupAccessControl.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool Delete(string accessGroupId)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = employeeGroupAccessControlDAO.delete(accessGroupId);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeGroupAccessControl.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}		

		public EmployeeGroupAccessControlTO Find(string accessGroupId)
		{
			EmployeeGroupAccessControlTO employeeGroupAccessControlTO = new EmployeeGroupAccessControlTO();

			try
			{
				employeeGroupAccessControlTO = employeeGroupAccessControlDAO.find(accessGroupId);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeGroupAccessControl.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return employeeGroupAccessControlTO;
		}

		public void ReceiveTransferObject(EmployeeGroupAccessControlTO employeeGroupAccessControlTO)
		{
			this.AccessGroupId = employeeGroupAccessControlTO.AccessGroupId;
			this.Name          = employeeGroupAccessControlTO.Name;
			this.Description   = employeeGroupAccessControlTO.Description;
		}

		public EmployeeGroupAccessControlTO SendTransferObject()
		{
			EmployeeGroupAccessControlTO employeeGroupAccessControlTO = new EmployeeGroupAccessControlTO();

			employeeGroupAccessControlTO.AccessGroupId = this.AccessGroupId;
			employeeGroupAccessControlTO.Name          = this.Name;
			employeeGroupAccessControlTO.Description   = this.Description;

			return employeeGroupAccessControlTO;
		}

		public void Clear()
		{
			this.AccessGroupId = -1;
			this.Name          = "";
			this.Description   = "";
		}

		public ArrayList Search(string name)
		{
			ArrayList employeeGroupAccessControlTOList = new ArrayList();
			ArrayList employeeGroupAccessControlList = new ArrayList();

			try
			{
				EmployeeGroupAccessControl employeeGroupAccessControlMember = new EmployeeGroupAccessControl();
				employeeGroupAccessControlTOList = employeeGroupAccessControlDAO.getEmployeeGroupAccessControl(name);

				foreach(EmployeeGroupAccessControlTO egacTO in employeeGroupAccessControlTOList)
				{
					employeeGroupAccessControlMember = new EmployeeGroupAccessControl();
					employeeGroupAccessControlMember.ReceiveTransferObject(egacTO);
					
					employeeGroupAccessControlList.Add(employeeGroupAccessControlMember);
				}
				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeGroupAccessControl.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return employeeGroupAccessControlList;
		}

		private void CacheData(ArrayList employeeGroupAccessControlTOList)
		{
			try
			{
				employeeGroupAccessControlDAO.serialize(employeeGroupAccessControlTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeGroupAccessControl.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}

		/*
		 * public void CacheData(string description, string fromDate, string toDate)
		{
			ArrayList employeeGroupAccessControlTOList = new ArrayList();

			try
			{
				employeeGroupAccessControlTOList = employeeGroupAccessControlDAO.getEmployeeGroupAccessControl(description, fromDate, toDate);
				this.CacheData(employeeGroupAccessControlTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeGroupAccessControl.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		
		/// <summary>
		/// Cache all of the Holidays from database
		/// </summary>
		public void CacheAllData()
		{
			try
			{
				this.CacheData(employeeGroupAccessControlDAO.getEmployeeGroupAccessControl("", "", ""));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeGroupAccessControl.CacheAllData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		 */

		/// <summary>
		/// Change DAO, start to use XML data source
		/// </summary>
		public void SwitchToXMLDAO()
		{
			try
			{
				daoFactory = DAOFactory.getDAOFactory();
				employeeGroupAccessControlDAO = daoFactory.getEmployeeGroupAccessControlDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " EmployeeGroupAccessControl.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}
	}
}
