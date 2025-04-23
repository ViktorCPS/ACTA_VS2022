using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class MealTypeAdditionalData
    {
        MealTypeAdditionalDataTO mealTypeAddDataTO = new MealTypeAdditionalDataTO();

        private DAOFactory daoFactory;
        private MealTypeAdditionalDataDAO mealTypeAddDataDAO;

        public MealTypeAdditionalDataTO MealTypeAddDataTO
        {
            get { return mealTypeAddDataTO; }
            set { mealTypeAddDataTO = value; }
        }

        DebugLog log;

        public MealTypeAdditionalData()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
            mealTypeAddDataDAO = daoFactory.getMealTypeAddDataDAO(null);
        }

        public MealTypeAdditionalData(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            mealTypeAddDataDAO = daoFactory.getMealTypeAddDataDAO(dbConnection);
        }

        public MealTypeAdditionalData(int mealTypeID, string descAdditional, byte[] picture)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            mealTypeAddDataDAO = daoFactory.getMealTypeAddDataDAO(null);

            this.MealTypeAddDataTO.MealTypeID = mealTypeID;
            this.MealTypeAddDataTO.DescriptionAdditional = descAdditional;
            this.MealTypeAddDataTO.Picture = picture;
        }

        public bool Delete(int mealTypeId)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = mealTypeAddDataDAO.delete(mealTypeId);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdditionalData.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }

        public bool Update(int mealTypeID,string descriptionAdd, byte[] picture)
        {
            bool isUpadated = false;

            try
            {
                isUpadated = mealTypeAddDataDAO.update(mealTypeID, descriptionAdd, picture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdditionalData.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpadated;
        }


        public int Save(int mealTypeID, string descriptionAdd, byte[] picture)
        {
            int saved = 0;
            try
            {
                saved = mealTypeAddDataDAO.insert(mealTypeID, descriptionAdd, picture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdditionalData.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }


        public bool Delete(int mealTypeId, bool doCommit)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = mealTypeAddDataDAO.delete(mealTypeId, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdditionalData.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }

        public bool Update(int mealTypeID, string descriptionAdd, byte[] picture, bool doCommit)
        {
            bool isUpadated = false;

            try
            {
                isUpadated = mealTypeAddDataDAO.update(mealTypeID, descriptionAdd, picture, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdditionalData.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpadated;
        }


        public int Save(int mealTypeID, string descriptionAdd, byte[] picture, bool doCommit)
        {
            int saved = 0;
            try
            {
                saved = mealTypeAddDataDAO.insert(mealTypeID, descriptionAdd, picture, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdditionalData.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

        public MealTypeAdditionalDataTO GetAdditionalData (int mealTypeID)
        {
            try
            {
                return mealTypeAddDataDAO.getAdditionalData(mealTypeID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdditionalData.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

        }

        public int DoesTableExists()
        {

            int exist = 0;

            try
            {
                exist = mealTypeAddDataDAO.tableExists();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdditionalData.DoesTableExists(): " + ex.Message + "\n");
                throw ex;
            }

            return exist;

        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = mealTypeAddDataDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdditionalData.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                mealTypeAddDataDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdditionalData.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                mealTypeAddDataDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdditionalData.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return mealTypeAddDataDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdditionalData.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                mealTypeAddDataDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdditionalData.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

    }
}
