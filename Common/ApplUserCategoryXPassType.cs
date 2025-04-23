using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class ApplUserCategoryXPassType
    {
        DAOFactory daoFactory = null;
		ApplUserCategoryXPassTypeDAO ucXptDAO = null;

		DebugLog log;

        ApplUserCategoryXPassTypeTO userCatXPtTO = new ApplUserCategoryXPassTypeTO();

		public ApplUserCategoryXPassTypeTO UserCategoryXPassTypeTO
		{
            get { return userCatXPtTO; }
			set{ userCatXPtTO = value; }
		}

        public ApplUserCategoryXPassType()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            ucXptDAO = daoFactory.getApplUserCategoryXPassTypeDAO(null);
		}
        public ApplUserCategoryXPassType(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            ucXptDAO = daoFactory.getApplUserCategoryXPassTypeDAO(dbConnection);
        }

        public int Save(bool doCommit)
        {            
            try
            {
                return ucXptDAO.insert(this.UserCategoryXPassTypeTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategoryXPassType.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public Dictionary<int, List<int>> SearchTypeDictionary()
        {
            try
            {
                return ucXptDAO.getUserCategoriesXPassTypesDictionary(this.UserCategoryXPassTypeTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategoryXPassType.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public List<ApplUserCategoryXPassTypeTO> Search()
        {
            try
            {
                return ucXptDAO.getUserCategoriesXPassTypes(this.UserCategoryXPassTypeTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategoryXPassType.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public bool Delete(int passTypeID, bool doCommit)
        {
            try
            {
                return ucXptDAO.delete(passTypeID,doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategoryXPassType.Delete(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public bool Delete(int categoryID, int ptID, string purpose)
        {
            try
            {
                return ucXptDAO.delete(categoryID, ptID, purpose);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategoryXPassType.Delete(): " + ex.Message + "\n");
                throw ex;
            }
        }
        
        public bool BeginTransaction()
        {
            try
            {
                return ucXptDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategoryXPassType.BeginTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                ucXptDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategoryXPassType.CommitTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                ucXptDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategoryXPassType.RollbackTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return ucXptDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategoryXPassType.GetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                ucXptDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUserCategoryXPassType.SetTransaction(): " + ex.Message + "\n");
                throw ex;
            }
        }



      
    }
}
