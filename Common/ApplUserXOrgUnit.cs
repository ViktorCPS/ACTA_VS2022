using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using Util;
using DataAccess;
using System.Data;

namespace Common
{
   public class ApplUserXOrgUnit
    {
       DAOFactory daoFactory = null;
       ApplUserXOrgUnitDAO applUsersXOUDAO = null;

		DebugLog log;

        ApplUserXOrgUnitTO auXouTO = new ApplUserXOrgUnitTO();

        public ApplUserXOrgUnitTO AuXOUnitTO
		{
			get { return auXouTO; }
			set {auXouTO = value; }
		}

		public ApplUserXOrgUnit()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            applUsersXOUDAO = daoFactory.getApplUserXOrgUnitDAO(null);
		}

        public ApplUserXOrgUnit(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            applUsersXOUDAO = daoFactory.getApplUserXOrgUnitDAO(dbConnection);
        }

        public ApplUserXOrgUnit(string userID, int ouID, string purpose)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            applUsersXOUDAO = daoFactory.getApplUserXOrgUnitDAO(null);

            this.AuXOUnitTO.UserID = userID;
            this.AuXOUnitTO.OrgUnitID = ouID;
            this.AuXOUnitTO.Purpose = purpose;
		}

		public int Save(string userID, int ouID, string purpose)
		{
			int inserted;
			try
			{
                inserted = applUsersXOUDAO.insert(userID, ouID, purpose);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXOU.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

		public int Save(string userID, int ouID, string purpose, bool doCommit)
		{
			int inserted;
			try
			{
                inserted = applUsersXOUDAO.insert(userID, ouID, purpose, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXOU.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}

        public List<ApplUserXOrgUnitTO> Search()
		{
			try
			{
                return applUsersXOUDAO.getApplUsersXOU(this.AuXOUnitTO);				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXOU.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<ApplUserXOrgUnitTO> Search(string ids)
        {
            try
            {
                return applUsersXOUDAO.getApplUsersXOU(ids);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersXOU.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, OrganizationalUnitTO> FindOUForUserDictionary(string userID, string purpose)
        {
            try
            {
                return applUsersXOUDAO.findOUForUserIDDictionary(userID, purpose, daoFactory);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersXOU.FindOUForUserDictionary(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
	
		public bool Delete(string userID, int ouID, string purpose)
		{
			bool isDeleted = false;

			try
			{
                isDeleted = applUsersXOUDAO.delete(userID, ouID, purpose);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXOU.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

        public bool Delete(int ouID, bool doCommit)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = applUsersXOUDAO.delete(ouID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersXOU.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

		public bool Delete(string userID, int ouID, string purpose, bool doCommit)
		{
			bool isDeleted = false;

			try
			{
                isDeleted = applUsersXOUDAO.delete(userID, ouID, purpose, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXOU.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

        public List<ApplUserTO> FindUsersForOU(int ouID, string purpose, List<string> statuses)
        {
            try
            {
                return applUsersXOUDAO.findUsersForOUID(ouID, purpose, statuses);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersXOU.FindUsersForWU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<ApplUserXOrgUnitTO> FindGrantedParentOUForOU()
        {
            try
            {
                return applUsersXOUDAO.findGrantedParentOUForOU(this.AuXOUnitTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersXWU.FindGrantedParentOUForOU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool BeginTransaction()
		{
			bool isStarted = false;

			try
			{
                isStarted = applUsersXOUDAO.beginTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXOU.BeginTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isStarted;
		}

		public void CommitTransaction()
		{
			try
			{
                applUsersXOUDAO.commitTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXOU.CommitTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void RollbackTransaction()
		{
			try
			{
                applUsersXOUDAO.rollbackTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXOU.RollbackTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public IDbTransaction GetTransaction()
		{
			try
			{
                return applUsersXOUDAO.getTransaction();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXOU.GetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void SetTransaction(IDbTransaction trans)
		{
			try
			{
                applUsersXOUDAO.setTransaction(trans);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUsersXOU.SetTransaction(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        
		public void Clear()
		{
            this.AuXOUnitTO = new ApplUserXOrgUnitTO();
		}
    }
}
