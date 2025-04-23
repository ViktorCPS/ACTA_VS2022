using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using TransferObjects;
using Util;


namespace Common
{
    public class ACSEvents
    {

        DAOFactory daoFactory = null;
        ACSEventsDAO acsEvDAO = null;

        DebugLog log;

        ACSEventTO acsEvTO = new ACSEventTO();

        public ACSEventTO ACSEvTO
        {
            get { return acsEvTO; }
            set { acsEvTO = value; }
        }
        public ACSEvents()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            acsEvDAO = daoFactory.getACSEventsDAO(null);
		}

        public ACSEvents(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            acsEvDAO = daoFactory.getACSEventsDAO(dbConnection);
        }

        public List<ACSEventTO> Search()
        {
            try
            {
                return acsEvDAO.findAll();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACSEvents.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }



    }
}
