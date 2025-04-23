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
    public class MealTypeEmpl
    {
        int _mealTypeEmplID;
        string _name;
        string _description;

        public int MealTypeEmplID
        {
            get { return _mealTypeEmplID; }
            set { _mealTypeEmplID = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

         private DAOFactory daoFactory;
        private MealTypeEmplDAO mealTypeEmplDAO;

        DebugLog log;

        

         public MealTypeEmpl()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

            this.MealTypeEmplID = -1;
            this.Name = "";
            this.Description = "";

			daoFactory = DAOFactory.getDAOFactory();
            mealTypeEmplDAO = daoFactory.getMealTypeEmplDAO(null);
        }

         public MealTypeEmpl(object dbConnection)
         {
             string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
             log = new DebugLog(logFilePath);

             this.MealTypeEmplID = -1;
             this.Name = "";
             this.Description = "";

             daoFactory = DAOFactory.getDAOFactory();
             mealTypeEmplDAO = daoFactory.getMealTypeEmplDAO(dbConnection);
         }

        public bool Delete(string mealsTypeEmplID)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = this.mealTypeEmplDAO.delete(mealsTypeEmplID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmpl.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }

        public int Save(string name, string description)
        {
            int saved = 0;
            try
            {
                saved = mealTypeEmplDAO.insert(name,description);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmpl.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

        public ArrayList Search(int mealTypeEmplID, string name, string description)
        {
            ArrayList mealTypeEmplTOList = new ArrayList();
            ArrayList mealTypeEmplList = new ArrayList();
            try
            {
                MealTypeEmpl mealTypeEmplMember = new MealTypeEmpl();
                mealTypeEmplTOList = mealTypeEmplDAO.getMealsTypeEmpl(mealTypeEmplID,name,description);
                foreach (MealTypeEmplTO mealTypeEmplTO in mealTypeEmplTOList)
                {
                    mealTypeEmplMember = new MealTypeEmpl();
                    mealTypeEmplMember.receiveTransferObject(mealTypeEmplTO);
                    mealTypeEmplList.Add(mealTypeEmplMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmpl.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealTypeEmplList;
        }

        public int SearchCount(int mealTypeEmplID, string name, string description)
        {
            int count = 0;
            try
            {
                count = mealTypeEmplDAO.getMealsTypeEmplCount(mealTypeEmplID, name, description);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmpl.SearchCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return count;
        }

       public bool Update(int mealTypeEmplID, string name, string description)
        {
            bool isUpdated = false;

            try
            {
                isUpdated = this.mealTypeEmplDAO.update(mealTypeEmplID, name, description);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmpl.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = mealTypeEmplDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmpl.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                mealTypeEmplDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmpl.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                mealTypeEmplDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmpl.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return mealTypeEmplDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmpl.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                mealTypeEmplDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmpl.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void receiveTransferObject(MealTypeEmplTO mealTypeEmplTO)
        {
            this.MealTypeEmplID = mealTypeEmplTO.MealTypeEmplID;
            this.Name = mealTypeEmplTO.Name;
            this.Description = mealTypeEmplTO.Description;            
        }

        /// <summary>
        /// Prepare TO for DAO processing
        /// </summary>
        /// <returns></returns>
        public MealTypeEmplTO sendTransferObject()
        {
            MealTypeEmplTO mealTypeEmplTO = new MealTypeEmplTO();

            mealTypeEmplTO.MealTypeEmplID = this.MealTypeEmplID;
            mealTypeEmplTO.Name = this.Name;
            mealTypeEmplTO.Description = this.Description;
           
            return mealTypeEmplTO;
        }
    }
}
