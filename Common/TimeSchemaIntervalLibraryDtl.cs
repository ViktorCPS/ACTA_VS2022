using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Util;
using TransferObjects;

namespace Common
{
   public class TimeSchemaIntervalLibraryDtl
    {
        DAOFactory daoFactory = null;
       TimeSchemaIntervalLibraryDtlDAO edao = null;

        DebugLog log;

        TimeSchemaIntervalLibraryDtlTO timeSchemaIntervalTO = new TimeSchemaIntervalLibraryDtlTO();


        public TimeSchemaIntervalLibraryDtlTO TimeSchemaIntervalLibraryDtlTO
        {
            get { return timeSchemaIntervalTO; }
            set { timeSchemaIntervalTO = value; }
        }
        		
		public TimeSchemaIntervalLibraryDtl()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getTimeSchemaIntervalLibraryDtlDAO(null);
		}

        public TimeSchemaIntervalLibraryDtl(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getTimeSchemaIntervalLibraryDtlDAO(null);
            }
        }

        public TimeSchemaIntervalLibraryDtl(Object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
             edao = daoFactory.getTimeSchemaIntervalLibraryDtlDAO(dbConnection);
        }
        public Dictionary<int, List<TimeSchemaIntervalLibraryDtlTO>> GetTimeSchemaIntervalLibraryDtlDictionary()
        {
            try
            {
                return edao.getTimeSchemaIntervalLibraryDtlDictionary(this.timeSchemaIntervalTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeSchemaIntervalLibraryDtl.getTimeSchemaIntervalLibraryDtlDictionary(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
