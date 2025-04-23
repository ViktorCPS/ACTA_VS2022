using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;
using DataAccess;
using Util;

namespace Common
{
    public class ApplUsersLoginChangesTbl
    {
        DAOFactory daoFactory = null;
        ApplUsersLoginChangesTblDAO dao = null;
		
		DebugLog log;

        ApplUsersLoginChangesTblTO tblTO = new ApplUsersLoginChangesTblTO();

        public ApplUsersLoginChangesTblTO TblTO
        {
            get { return tblTO; }
            set { tblTO = value; }
        }
        		
		public ApplUsersLoginChangesTbl()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getApplUsersLoginChangesTblDAO(null);
		}

        public ApplUsersLoginChangesTbl(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getApplUsersLoginChangesTblDAO(dbConnection);
        }

        public ApplUsersLoginChangesTbl(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                dao = daoFactory.getApplUsersLoginChangesTblDAO(null);
            }
        }

        public List<string> SearchTableNames()
        {           
            try
            {
                return dao.getTablesNames();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLoginChangesTbl.SearchTableNames(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<string> SearchAllTableNames()
        {
            try
            {
                return dao.getAllTablesNames();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLoginChangesTbl.SelectAllTableNames(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

        }

        public int Save1(List<string> listToSaveToDB)
        {
            try
            {
                    return dao.insert(listToSaveToDB);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLoginChangesTbl.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Delete()
        {
            bool isDeleted = false;

            try
            {
                isDeleted = dao.delete();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLoginChangesTbl.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public int Save(string table)
        {
            try
            {
                return dao.insert(table);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersLoginChangesTbl.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
