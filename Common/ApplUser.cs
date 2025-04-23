using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

using DataAccess;
using TransferObjects;
using Util;
using System.Data;

namespace Common
{
	/// <summary>
	/// Summary description for ApplUser.
	/// </summary>
	public class ApplUser
	{
        DAOFactory daoFactory = null;
		ApplUserDAO applUserDAO = null;

		DebugLog log;

        ApplUserTO userTO = new ApplUserTO();

        public ApplUserTO UserTO
		{
			get{ return userTO; }
			set{ userTO = value; }
		}

		public ApplUser()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			applUserDAO = daoFactory.getApplUserDAO(null);
		}
        public ApplUser(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            applUserDAO = daoFactory.getApplUserDAO(dbConnection);
        }

		public ApplUser(string userID, string password, string name, string description, int privilegeLvl,
			string status, int numOfTries, string langCode)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            applUserDAO = daoFactory.getApplUserDAO(null);

			this.UserTO.UserID = userID;
            this.UserTO.Password = password;
            this.UserTO.Name = name;
            this.UserTO.Description = description;
            this.UserTO.PrivilegeLvl = privilegeLvl;
            this.UserTO.Status = status;
            this.UserTO.NumOfTries = numOfTries;
            this.UserTO.LangCode = langCode;
		}

		public int Save()
		{
			try
			{
				return applUserDAO.insert(this.UserTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUser.Save(): " + ex.Message + "\n");
				throw ex;
			}
		}

        public int Save(bool doCommit)
        {
            try
            {
                return applUserDAO.insert(this.UserTO,doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUser.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }

		public List<ApplUserTO> Search()
		{
			try
			{
				return applUserDAO.getApplUsers(this.UserTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUser.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<ApplUserTO> SearchInactiveUsers(DateTime monthCreated)
        {
            try
            {
                return applUserDAO.getInactiveUsers(monthCreated);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUser.SearchInactiveUsers(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<string, ApplUserTO> SearchDictionary()
        {
            try
            {
                return applUserDAO.getApplUsersDictionary(this.UserTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUser.SearchDictionary(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<ApplUserTO> SearchForCategory(int usersCategory)
        {
            try
            {
                return applUserDAO.getApplUsersForCategory(usersCategory);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUser.SearchForCategory(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<ApplUserTO> SearchWithStatus(List<string> statuses)
        {
            try
            {
                return applUserDAO.getApplUsersWithStatus(this.UserTO, statuses);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUser.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public List<ApplUserTO> SearchVerifiedByWUnits(string wUnits)
		{
			try
			{				
				return applUserDAO.getApplUsersVerifiedByWUnits(wUnits);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUser.SearchVerifiedByWUnits(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool Update()
		{
			try
			{
				return applUserDAO.update(this.UserTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUser.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public bool Update(bool doCommit)
        {
            try
            {
                return applUserDAO.update(this.UserTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUser.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool UpdatePassword(string userID, string password)
        {
            try
            {
                return applUserDAO.updatePassword(userID, password);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUser.UpdatePassword(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool UpdateExitPermVerification(bool doCommit)
        {
            try
            {
                return applUserDAO.updateExitPermVerification(this.UserTO,doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUser.UpdateExitPermVerification(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public bool UpdateExitPermVerification()
		{
			try
			{
				return applUserDAO.updateExitPermVerification(this.UserTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUser.UpdateExitPermVerification(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool Delete(string userID)
		{
            try
			{
				return applUserDAO.delete(userID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUser.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public ApplUserTO Find(string userID)
		{
            try
			{
				return applUserDAO.find(userID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUser.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public ApplUserTO FindUserPassword(string userID, string password)
		{
			try
			{
				if (this.applUserDAO == null || this.applUserDAO is XMLDAOFactory)
				{
					throw new Exception("No Database connection.");
				}

				return applUserDAO.findUserPassword(userID, password);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUser.FindPassword(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public ApplUserTO FindExitPermVerification(string userID)
		{
			try
			{
				return applUserDAO.findExitPermVerification(userID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUser.FindExitPermVerification(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        
		public void Clear()
		{
			this.UserTO = new ApplUserTO();
		}

		
		/// <summary>
		/// Send list of ApplUserTO objects to serialization.
		/// </summary>
		/// <param name="locatioTOList">List of LocationTO</param>
		private void CacheData(List<ApplUserTO> applUserTOList)
		{
			try
			{
				applUserDAO.serialize(applUserTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUser.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		
		public void CacheData()
		{
			List<ApplUserTO> applUserTOList = new List<ApplUserTO>();

			try
			{
				applUserTOList = applUserDAO.getApplUsers(this.UserTO);
				this.CacheData(applUserTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUser.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		
		/// <summary>
		/// Cache all of the ApplUsers from database
		/// </summary>
		public void CacheAllData()
		{
			try
			{
				this.CacheData(applUserDAO.getApplUsers(new ApplUserTO()));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUser.CacheAllData(): " + ex.Message + "\n");
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
				applUserDAO = daoFactory.getApplUserDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ApplUser.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = applUserDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                applUserDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                applUserDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return applUserDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                applUserDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassType.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        
    }
}
