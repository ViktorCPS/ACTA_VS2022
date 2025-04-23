using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

using Util;
using DataAccess;

namespace Common
{
    public class DBConnectionManager
    {
        protected DebugLog log = null;
        private DAOFactory daoFactory = null;

        private static readonly DBConnectionManager instance = new DBConnectionManager();

        private DBConnectionManager()
        {
            log = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");
            daoFactory = DAOFactory.getDAOFactory();
        }

        public static DBConnectionManager Instance
        {
            get { return instance; }
        }

        public Object MakeNewDBConnection()
        {
            Object dbConnection = null;

            try
            {
                dbConnection = daoFactory.MakeNewDBConnection();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " DBConnectionManager.MakeNewDBConnection(): " + ex.Message + "\n");
            }

            return dbConnection;
        }

        public void CloseDBConnection(Object dbConnection)
        {
            try
            {
                daoFactory.CloseConnection(dbConnection);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " DBConnectionManager.CloseDBConnection(): " + ex.Message + "\n");
            }
        }
        public bool TestDBConnection(Object dbConnection)
        {
            bool connected = false;
            try
            {
                connected = daoFactory.TestDataSourceConnection(dbConnection);
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " DBConnectionManager.CloseDBConnection(): " + ex.Message + "\n");
                
            }
            return connected;
        }
        public void SetThreadSafe(bool safe)
        {
            DAOFactory.threadSafe = safe;
        }
        public bool TestDBConnectionDummySelect(Object dbConnection)
        {
            try
            {
                return daoFactory.TestDBConnectionDummySelect(dbConnection);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " DBConnectionManager.TestDBConnectionDummySelect(): " + ex.Message + "\n");
                return false;
            }
        }
    }
}
