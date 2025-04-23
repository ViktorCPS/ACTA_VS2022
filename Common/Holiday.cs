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
	/// Summary description for Holiday.
	/// </summary>
	public class Holiday
	{
		DAOFactory daoFactory = null;
		HolidayDAO holidayDAO = null;

		DebugLog log;

        private HolidayTO holTO = new HolidayTO();

		public HolidayTO HolTO
		{
			get { return holTO; }
			set {holTO = value; }
		}
        
		public Holiday()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			holidayDAO = daoFactory.getHolidayDAO(null);
		}
        public Holiday(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            holidayDAO = daoFactory.getHolidayDAO(dbConnection);
        }

		public Holiday(string description, DateTime holidayDate)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			this.HolTO.Description = description;
			this.HolTO.HolidayDate = holidayDate;

			daoFactory = DAOFactory.getDAOFactory();
			holidayDAO = daoFactory.getHolidayDAO(null);
		}

		public int Save(string description, DateTime holidayDate)
		{
			try
			{
				return holidayDAO.insert(description, holidayDate);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Holiday.Save(): " + ex.Message + "\n");
				throw ex;
			}
		}

		// fromDate and toDate should be in format "MM/dd/yyy"!!!!!
		public List<HolidayTO> Search(DateTime fromDate, DateTime toDate)
		{
			try
			{				
				return holidayDAO.getHolidays(this.HolTO, fromDate, toDate);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Holiday.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool Update(string description, DateTime oldHolidayDate, DateTime newHolidayDate)
		{
			try
			{
				return holidayDAO.update(description, oldHolidayDate, newHolidayDate);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Holiday.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool Delete(DateTime holidayDate)
		{
			try
			{
				return holidayDAO.delete(holidayDate);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Holiday.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public HolidayTO Find(DateTime holidayDate)
		{
			try
			{
				return holidayDAO.find(holidayDate);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Holiday.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        		
		public void Clear()
		{
			this.HolTO = new HolidayTO();
		}

		/// <summary>
		/// Send list of HolidayTO objects to serialization.
		/// </summary>
		/// <param name="locatioTOList">List of LocationTO</param>
		private void CacheData(List<HolidayTO> holidayTOList)
		{
			try
			{
				holidayDAO.serialize(holidayTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Holiday.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		// fromDate and toDate should be in format "MM/dd/yyy"!!!!!
		public void CacheData(DateTime fromDate, DateTime toDate)
		{
			List<HolidayTO> holidayTOList = new List<HolidayTO>();

			try
			{
				holidayTOList = holidayDAO.getHolidays(this.HolTO, fromDate, toDate);
				this.CacheData(holidayTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Holiday.CacheData(): " + ex.Message + "\n");
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
				this.CacheData(holidayDAO.getHolidays(new HolidayTO(), new DateTime(), new DateTime()));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Holiday.CacheAllData(): " + ex.Message + "\n");
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
				holidayDAO = daoFactory.getHolidayDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Holiday.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}
	}
}
