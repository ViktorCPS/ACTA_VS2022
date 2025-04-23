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
	/// Summary description for TimeSchemaPause.
	/// </summary>
	public class TimeSchemaPause
	{		
		DAOFactory daoFactory = null;
		TimeSchemaPauseDAO timeSchemaPauseDAO = null;

		DebugLog log;

        TimeSchemaPauseTO pauseTO = new TimeSchemaPauseTO();

        public TimeSchemaPauseTO PauseTO
		{
			get { return pauseTO; }
			set { pauseTO = value; }
		}
        
		public TimeSchemaPause()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			timeSchemaPauseDAO = daoFactory.getTimeSchemaPauseDAO(null);
		}

        public TimeSchemaPause(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            timeSchemaPauseDAO = daoFactory.getTimeSchemaPauseDAO(dbConnection);
        }

		public TimeSchemaPause(int pauseID, string description, int pauseDuration,
			int earliestUseTime, int latestUseTime, int pauseOffset, int shortBreakDuration)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			this.PauseTO.PauseID            = pauseID;
			this.PauseTO.Description        = description;
			this.PauseTO.PauseDuration      = pauseDuration;
			this.PauseTO.EarliestUseTime    = earliestUseTime;
			this.PauseTO.LatestUseTime      = latestUseTime;
			this.PauseTO.PauseOffset        = pauseOffset;
			this.PauseTO.ShortBreakDuration = shortBreakDuration;

			daoFactory = DAOFactory.getDAOFactory();
			timeSchemaPauseDAO = daoFactory.getTimeSchemaPauseDAO(null);			
		}

		public int Save(int pauseID, string description, int pauseDuration,
			int earliestUseTime, int latestUseTime, int pauseOffset, int shortBreakDuration)
		{
			int inserted;
			try
			{
				inserted = timeSchemaPauseDAO.insert(pauseID, description, pauseDuration,
					earliestUseTime, latestUseTime, pauseOffset, shortBreakDuration);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPause.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

		public bool Update(int pauseID, string description, int pauseDuration,
			int earliestUseTime, int latestUseTime, int pauseOffset, int shortBreakDuration)
		{
			bool isUpdated;

			try
			{
				isUpdated = timeSchemaPauseDAO.update(pauseID, description, pauseDuration,
					earliestUseTime, latestUseTime, pauseOffset, shortBreakDuration);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPause.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public bool Delete(int pauseID)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = timeSchemaPauseDAO.delete(pauseID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPause.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public TimeSchemaPauseTO Find(int pauseID)
		{
			TimeSchemaPauseTO timeSchemaPauseTO = new TimeSchemaPauseTO();

			try
			{
				timeSchemaPauseTO = timeSchemaPauseDAO.find(pauseID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPause.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return timeSchemaPauseTO;
		}

		public int FindMAXPauseID()
		{
			int pID = 0;

			try
			{
				pID = timeSchemaPauseDAO.findMAXPauseID();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPause.FindMAXPauseID(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return pID;
		}
        
        public void Clear()
		{
            this.PauseTO = new TimeSchemaPauseTO();			
		}

        // Same as method Search(string description, IDBTransaction trans)
        public List<TimeSchemaPauseTO> Search(string description)
		{
			try
			{		
				return timeSchemaPauseDAO.getTimeSchemaPause(description);				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPause.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        // Same as method Search(string description)
        public List<TimeSchemaPauseTO> Search(string description, IDbTransaction trans)
        {
            try
            {
                return timeSchemaPauseDAO.getTimeSchemaPause(description, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeSchemaPause.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		private void CacheData(List<TimeSchemaPauseTO> timeSchemaPauseTOList)
		{
			try
			{
				timeSchemaPauseDAO.serialize(timeSchemaPauseTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPause.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}

		/*
		 * public void CacheData(string description, string fromDate, string toDate)
		{
			ArrayList timeSchemaPauseTOList = new ArrayList();

			try
			{
				timeSchemaPauseTOList = timeSchemaPauseDAO.getTimeSchemaPause(description, fromDate, toDate);
				this.CacheData(timeSchemaPauseTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPause.CacheData(): " + ex.Message + "\n");
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
				this.CacheData(timeSchemaPauseDAO.getTimeSchemaPause("", "", ""));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPause.CacheAllData(): " + ex.Message + "\n");
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
				timeSchemaPauseDAO = daoFactory.getTimeSchemaPauseDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPause.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}
	}
}
