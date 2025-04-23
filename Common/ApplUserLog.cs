using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

using Util;
using DataAccess;
using TransferObjects;

namespace Common
{
	/// <summary>
	/// Summary description for ApplUserLog.
	/// </summary>
	public class ApplUserLog
	{		
		DAOFactory daoFactory = null;
		ApplUsersLogDAO applUserLogDAO = null;
		DebugLog log;

        ApplUserLogTO userLogTO = new ApplUserLogTO();

        public ApplUserLogTO UserLogTO
		{
            get { return userLogTO; }
            set { userLogTO = value; }
		}
				
		public ApplUserLog()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			applUserLogDAO = daoFactory.getApplUsersLogDAO(null);
		}

        public ApplUserLog(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            applUserLogDAO = daoFactory.getApplUsersLogDAO(dbConnection);
        }

		public ApplUserLog(DateTime loginTime, DateTime logOutTime, string host, int sessionID )
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			applUserLogDAO = daoFactory.getApplUsersLogDAO(null);

			this.UserLogTO.LogInTime = loginTime;
			this.UserLogTO.LogOutTime = logOutTime;
			this.UserLogTO.Host = host;
			this.UserLogTO.SessionID = sessionID;
			
		}
		public ApplUserLogTO Insert()
		{
            ApplUserLogTO logTO = new ApplUserLogTO();

			try
			{
                logTO = applUserLogDAO.insert(this.UserLogTO);				 	
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + this.ToString() + ".btnLogIn_Click(): " + ex.Message + "\n");
			}

            return logTO;
		}

        public ApplUserLogTO FindMaxSession(string userID, string host, string chanel)
        {
            ApplUserLogTO logTO = new ApplUserLogTO();

            try
            {
                logTO = applUserLogDAO.findMaxSession(userID, host, chanel);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + this.ToString() + ".Update: " + ex.Message + "\n");
            }

            return logTO;
        }

		public int Update()
		{
            int upd = -1;

			try
			{
                upd = applUserLogDAO.update(this.UserLogTO.SessionID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + this.ToString() + ".Update: " + ex.Message + "\n");
			}

            return upd;
	    }

        public int Update(string createdBy, string modifiedBy)
        {
            int upd = -1;

            try
            {                
                upd = applUserLogDAO.update(this.userLogTO.SessionID, createdBy, modifiedBy);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + this.ToString() + ".Update: " + ex.Message + "\n");
            }

            return upd;
        }

        public List<ApplUserLogTO> Search(string userIDs, DateTime dateFrom, DateTime dateTo, List<string> changeTables, Dictionary<string, ApplUserTO> users)
		{
			try
			{			
				return applUserLogDAO.getApplUsersLog(this.UserLogTO, userIDs, dateFrom, dateTo, changeTables, users);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersLog.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<ApplUserLogTO> SearchOpenSessions(string userID, string host, string chanel)
        {            
            try
            {
                return applUserLogDAO.getOpenSessions(userID, host, chanel);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserLog.SearchOpenSessions(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }	
    }
}

