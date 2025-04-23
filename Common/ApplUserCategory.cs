using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class ApplUserCategory
    {
        DAOFactory daoFactory = null;
		ApplUserCategoryDAO userCatDAO = null;

		DebugLog log;

        ApplUserCategoryTO userCatTO = new ApplUserCategoryTO();

        public ApplUserCategoryTO UserCategoryTO
		{
            get { return userCatTO; }
			set{ userCatTO = value; }
		}

        public ApplUserCategory()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            userCatDAO = daoFactory.getApplUserCategoryDAO(null);
		}
        
        public ApplUserCategory(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            userCatDAO = daoFactory.getApplUserCategoryDAO(dbConnection);
        }

        public int Save(bool doCommit)
        {            
            try
            {
                return userCatDAO.insert(this.UserCategoryTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategory.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }
        
        public int Save1(ApplUserCategoryTO userCategory)
        {
            try
            {
                return userCatDAO.insert(userCategory, true);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategory.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }


        public bool Update(bool doCommit)
        {
            try
            {
                return userCatDAO.update(this.UserCategoryTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategory.Update(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public ApplUserCategoryTO Find(int categoryID)
        {
            try
            {
                return userCatDAO.find(categoryID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategory.Find(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<ApplUserCategoryTO> Search(object conn)
        {            
            try
            {
                return userCatDAO.getUserCategories(this.UserCategoryTO, Common.Misc.getLicenceModuls(conn).Contains((int)Constants.Moduls.MedicalCheck));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategory.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<ApplUserCategoryTO> Search1 ()
        {
            try
            {
                return userCatDAO.getUserCategories(this.UserCategoryTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategory.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }
     
        public List<ApplUserCategoryTO> SearchLoginCategories(string userID, bool getDefault)
        {
            try
            {
                return userCatDAO.getUserCategoriesForLoginUser(userID, getDefault);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategory.SearchLoginCategories(): " + ex.Message + "\n");
                throw ex;
            }
        }
        
        public bool Delete(int categoryID)
        {
            try
            {
                return userCatDAO.delete(categoryID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategory.Delete(): " + ex.Message + "\n");
                throw ex;
            }
        }
        
        public bool BeginTransaction()
        {
            try
            {
                return userCatDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategory.BeginTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                userCatDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategory.CommitTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                userCatDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategory.RollbackTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return userCatDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategory.GetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                userCatDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategory.SetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

    }
}
