using System;
using System.Collections;
using System.Configuration;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// Summary description for TimeAccessProfile.
	/// </summary>
	public class TimeAccessProfile
	{
		private int     _timeAccessProfileId = -1;
		private string  _name = "";
		private string  _description = "";

		DAOFactory daoFactory = null;
		TimeAccessProfileDAO timeAccessProfileDAO = null;

		DebugLog log;

		public int TimeAccessProfileId
		{
			get { return _timeAccessProfileId; }
			set { _timeAccessProfileId = value; }
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

		public TimeAccessProfile()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			timeAccessProfileDAO = daoFactory.getTimeAccessProfileDAO(null);
		}
        public TimeAccessProfile(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            timeAccessProfileDAO = daoFactory.getTimeAccessProfileDAO(dbConnection);
        }

		public TimeAccessProfile(int timeAccessProfileId, string name, string description)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			this.TimeAccessProfileId = timeAccessProfileId;
			this.Name                = name;
			this.Description         = description;

			daoFactory = DAOFactory.getDAOFactory();
			timeAccessProfileDAO = daoFactory.getTimeAccessProfileDAO(null);			
		}

		public int Save(string name, string description, bool doCommit)
		{
			int inserted;
			try
			{
				inserted = timeAccessProfileDAO.insert(name, description, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfile.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

		public bool Update(string timeAccessProfileId, string name, string description, bool doCommit)
		{
			bool isUpdated;

			try
			{
				isUpdated = timeAccessProfileDAO.update(timeAccessProfileId, name, description, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfile.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool Delete(string timeAccessProfileId, bool doCommit)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = timeAccessProfileDAO.delete(timeAccessProfileId, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfile.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}	
	
		public bool BeginTransaction()
		{
			bool isStarted = false;

			try
			{
				isStarted = timeAccessProfileDAO.beginTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfile.BeginTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isStarted;
		}

		public void CommitTransaction()
		{
			try
			{
				timeAccessProfileDAO.commitTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfile.CommitTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void RollbackTransaction()
		{
			try
			{
				timeAccessProfileDAO.rollbackTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfile.CommitTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public IDbTransaction GetTransaction()
		{
			try
			{
				return timeAccessProfileDAO.getTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfile.GetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void SetTransaction(IDbTransaction trans)
		{
			try
			{
				timeAccessProfileDAO.setTransaction(trans);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfile.SetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public TimeAccessProfileTO Find(string timeAccessProfileId)
		{
			TimeAccessProfileTO timeAccessProfileTO = new TimeAccessProfileTO();

			try
			{
				timeAccessProfileTO = timeAccessProfileDAO.find(timeAccessProfileId);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfile.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return timeAccessProfileTO;
		}

		public void ReceiveTransferObject(TimeAccessProfileTO timeAccessProfileTO)
		{
			this.TimeAccessProfileId = timeAccessProfileTO.TimeAccessProfileId;
			this.Name                = timeAccessProfileTO.Name;
			this.Description         = timeAccessProfileTO.Description;
		}

		public TimeAccessProfileTO SendTransferObject()
		{
			TimeAccessProfileTO timeAccessProfileTO = new TimeAccessProfileTO();

			timeAccessProfileTO.TimeAccessProfileId = this.TimeAccessProfileId;
			timeAccessProfileTO.Name                = this.Name;
			timeAccessProfileTO.Description         = this.Description;

			return timeAccessProfileTO;
		}

		public void Clear()
		{
			this.TimeAccessProfileId = -1;
			this.Name                = "";
			this.Description         = "";
		}

		public ArrayList Search(string name)
		{
			ArrayList timeAccessProfileTOList = new ArrayList();
			ArrayList timeAccessProfileList = new ArrayList();

			try
			{
				TimeAccessProfile timeAccessProfileMember = new TimeAccessProfile();
				timeAccessProfileTOList = timeAccessProfileDAO.getTimeAccessProfile(name);

				foreach(TimeAccessProfileTO tapTO in timeAccessProfileTOList)
				{
					timeAccessProfileMember = new TimeAccessProfile();
					timeAccessProfileMember.ReceiveTransferObject(tapTO);
					
					timeAccessProfileList.Add(timeAccessProfileMember);
				}
				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfile.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return timeAccessProfileList;
		}

		private void CacheData(ArrayList timeAccessProfileTOList)
		{
			try
			{
				timeAccessProfileDAO.serialize(timeAccessProfileTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfile.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}

		/*
		 * public void CacheData(string description, string fromDate, string toDate)
		{
			ArrayList timeAccessProfileTOList = new ArrayList();

			try
			{
				timeAccessProfileTOList = timeAccessProfileDAO.getTimeAccessProfile(description, fromDate, toDate);
				this.CacheData(timeAccessProfileTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfile.CacheData(): " + ex.Message + "\n");
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
				this.CacheData(timeAccessProfileDAO.getTimeAccessProfile("", "", ""));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfile.CacheAllData(): " + ex.Message + "\n");
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
                daoFactory = DAOFactory.getDAOFactory(Constants.XMLDataProvider);
				timeAccessProfileDAO = daoFactory.getTimeAccessProfileDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeAccessProfile.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}
	}
}
