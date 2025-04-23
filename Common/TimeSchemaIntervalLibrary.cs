using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using Util;
using DataAccess;

namespace Common
{
    public class TimeSchemaIntervalLibrary
    {
        DAOFactory daoFactory = null;
        TimeSchemaIntervalLibraryDAO edao = null;

        DebugLog log;

        TimeSchemaIntervalLibraryTO timeSchemaIntervalTO = new TimeSchemaIntervalLibraryTO();


        public TimeSchemaIntervalLibraryTO TimeSchemaIntervalLibraryTO
        {
            get { return timeSchemaIntervalTO; }
            set { timeSchemaIntervalTO = value; }
        }

        public TimeSchemaIntervalLibrary()
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getTimeSchemaIntervalLibraryDAO(null);
        }

        public TimeSchemaIntervalLibrary(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getTimeSchemaIntervalLibraryDAO(null);
            }
        }

        public TimeSchemaIntervalLibrary(Object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getTimeSchemaIntervalLibraryDAO(dbConnection);
        }

        public Dictionary<int, List<TimeSchemaIntervalLibraryTO>> GetTimeSchemaIntervalLibraryDictionary()
        {
            try
            {
                return edao.getTimeSchemaIntervalLibraryDictionary(this.TimeSchemaIntervalLibraryTO);
            }
            catch (Exception ex)
            {
				log.writeLog(DateTime.Now + " TimeSchemaIntervalLibrary.getTimeSchemaIntervalLibraryDictionary(): " + ex.Message + "\n");
				throw ex;
            }
        }
    }
}
