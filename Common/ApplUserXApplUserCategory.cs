using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class ApplUserXApplUserCategory
    {
        DAOFactory daoFactory = null;
		ApplUserXApplUserCategoryDAO userXCatDAO = null;

		DebugLog log;

        ApplUserXApplUserCategoryTO userXCatTO = new ApplUserXApplUserCategoryTO();

        public ApplUserXApplUserCategoryTO UserXCategoryTO
		{
            get { return userXCatTO; }
			set{ userXCatTO = value; }
		}

        public ApplUserXApplUserCategory()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            userXCatDAO = daoFactory.getApplUserXCategoryDAO(null);
		}

        public ApplUserXApplUserCategory(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            userXCatDAO = daoFactory.getApplUserXCategoryDAO(dbConnection);
        }

        public int Save(bool doCommit)
        {            
            try
            {
                return userXCatDAO.insert(this.UserXCategoryTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXApplUserCategory.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return userXCatDAO.update(this.UserXCategoryTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXApplUserCategory.Update(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool SetDefaultCategory()
        {
            try
            {
                return userXCatDAO.setDefaultCategory(this.UserXCategoryTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXApplUserCategory.SetDefaultCategory(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public Dictionary<string, List<int>> SearchUserCategoryDictionary()
        {
            try
            {
               return userXCatDAO.getUserCategoriesDict();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXApplUserCategory.SearchUserCategoryDictionary(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<ApplUserXApplUserCategoryTO> Search()
        {
            try
            {
                return userXCatDAO.getUserCategories(this.UserXCategoryTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXApplUserCategory.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<ApplUserXApplUserCategoryTO> Search(IDbTransaction trans)
        {
            try
            {
                return userXCatDAO.getUserCategories(this.UserXCategoryTO, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXApplUserCategory.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<int> SearchSupervisors(string emplIDs)
        {
            try
            {
                return userXCatDAO.getSupervisors(emplIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXApplUserCategory.SearchSupervisors(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Delete(int categoryID, string userID, bool doCommit)
        {
            try
            {
                return userXCatDAO.delete(categoryID,userID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXApplUserCategory.Delete(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public bool Delete(int categoryID, string userID)
        {
            try
            {
                return userXCatDAO.delete(categoryID, userID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXApplUserCategory.Delete(): " + ex.Message + "\n");
                throw ex;
            }
        }


        public bool Delete( string userID, bool doCommit)
        {
            try
            {
                return userXCatDAO.delete( userID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXApplUserCategory.Delete(): " + ex.Message + "\n");
                throw ex;
            }
        }
        
        public bool BeginTransaction()
        {
            try
            {
                return userXCatDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXApplUserCategory.BeginTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                userXCatDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXApplUserCategory.CommitTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                userXCatDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXApplUserCategory.RollbackTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return userXCatDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXApplUserCategory.GetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                userXCatDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserXApplUserCategory.SetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }    
    }
}
