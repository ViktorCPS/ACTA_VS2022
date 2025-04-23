using System;
using System.Collections.Generic;
using System.Text;
using Util;
using DataAccess;
using TransferObjects;

namespace Common
{
    public class HolidaysExtended
    {
        DAOFactory daoFactory = null;
        HolidaysExtendedDAO holidayDAO = null;

		DebugLog log;

        private HolidaysExtendedTO holTO = new HolidaysExtendedTO();


		public HolidaysExtendedTO HolTO
		{
			get { return holTO; }
			set {holTO = value; }
		}
        
		public HolidaysExtended()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            holidayDAO = daoFactory.getHolidaysExtendedDAO(null);
		}
        public HolidaysExtended(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            holidayDAO = daoFactory.getHolidaysExtendedDAO(dbConnection);
        }

		public int Save()
		{
			try
			{
                return holidayDAO.insert(this.HolTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Holiday.Save(): " + ex.Message + "\n");
				throw ex;
			}
		}

        public List<string> SearchDescriptions()
        {
            try
            {
                return holidayDAO.SearchDescriptions();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Holiday.SearchDescriptions(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
		// fromDate and toDate should be in format "MM/dd/yyy"!!!!!
        public List<HolidaysExtendedTO> Search(DateTime fromDate, DateTime toDate)
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

		public bool Update()
		{
			try
			{
                return holidayDAO.update(this.HolTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Holiday.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public bool Delete(int recID)
		{
			try
			{
                return holidayDAO.delete(recID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Holiday.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        

    }
}
