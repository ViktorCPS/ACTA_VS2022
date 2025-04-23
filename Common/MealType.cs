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
    public class MealType
    {
        int _mealTypeID;
        string _name;
        string _description;
        DateTime _hoursFrom;
        DateTime _hoursTo;

        private DAOFactory daoFactory;
        private MealTypeDAO mealTypeDAO;

        DebugLog log;

        public DateTime HoursTo
        {
            get { return _hoursTo; }
            set { _hoursTo = value; }
        }

        public DateTime HoursFrom
        {
            get { return _hoursFrom; }
            set { _hoursFrom = value; }
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

        public int MealTypeID
        {
            get { return _mealTypeID; }
            set { _mealTypeID = value; }
        }

        public MealType()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

            MealTypeID = -1;
            Name = "";
            Description = "";
            HoursFrom = new DateTime();
            HoursTo = new DateTime();

			daoFactory = DAOFactory.getDAOFactory();
			mealTypeDAO = daoFactory.getMealTypeDAO(null);
        }

        public MealType(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            MealTypeID = -1;
            Name = "";
            Description = "";
            HoursFrom = new DateTime();
            HoursTo = new DateTime();

            daoFactory = DAOFactory.getDAOFactory();
            mealTypeDAO = daoFactory.getMealTypeDAO(dbConnection);
        }

        public MealType(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            MealTypeID = mealTypeID;
            Name = name;
            Description = description;
            HoursFrom = hoursFrom;
            HoursTo = hoursTo;

            daoFactory = DAOFactory.getDAOFactory();
            mealTypeDAO = daoFactory.getMealTypeDAO(null);
        }

        public bool Delete(int mealTypeId)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = mealTypeDAO.delete(mealTypeId);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }

        public bool Delete(int mealTypeId, bool doCommit)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = mealTypeDAO.delete(mealTypeId, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }

        public bool Update(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo)
        {
            bool isUpadated = false;

            try
            {
                isUpadated = mealTypeDAO.update(mealTypeID, name, description, hoursFrom, hoursTo);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpadated;
        }

        public bool Update(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo, bool doCommit)
        {
            bool isUpadated = false;

            try
            {
                isUpadated = mealTypeDAO.update(mealTypeID, name, description, hoursFrom, hoursTo, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpadated;
        }
        public int Save(int MealTypeID,string name,string Description,DateTime hoursFrom,DateTime hoursTo)
        {
            int saved = 0;
            try
            {
                saved = mealTypeDAO.insert(MealTypeID, name, Description, hoursFrom, hoursTo);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

        public int Save(int MealTypeID, string name, string Description, DateTime hoursFrom, DateTime hoursTo, bool doCommit)
        {
            int saved = 0;
            try
            {
                saved = mealTypeDAO.insert(MealTypeID, name, Description, hoursFrom, hoursTo, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }



        public ArrayList Search(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo)
        {
            ArrayList  mealTypeTOList = new ArrayList();
            ArrayList mealTypeList = new ArrayList();
            try
            {
                MealType mealTypeMember = new MealType();
                mealTypeTOList = mealTypeDAO.getMealsType(mealTypeID, name, description, hoursFrom, hoursTo);
                foreach (MealTypeTO mealTypeTO in mealTypeTOList)
                {
                    mealTypeMember = new MealType();
                    mealTypeMember.receiveTransferObject(mealTypeTO);
                    mealTypeList.Add(mealTypeMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealTypeList;
        }

        public MealTypeTO GetMealType(int mealTypeID)
        {
            MealTypeTO mealType = new MealTypeTO();
            try
            {

                mealType = mealTypeDAO.getMealType(mealTypeID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.GetMealType(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return mealType;
        }

        public int SearchCount(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo)
        {
            int count = 0;
            try
            {
                count = mealTypeDAO.getMealsTypeCount(mealTypeID,name,description,hoursFrom,hoursTo);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.SearchCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return count;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = mealTypeDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                mealTypeDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                mealTypeDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return mealTypeDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                mealTypeDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void receiveTransferObject(MealTypeTO mealTypeTO)
        {
            this.MealTypeID = mealTypeTO.MealTypeID;
            this.Name = mealTypeTO.Name;
            this.Description = mealTypeTO.Description;
            this.HoursFrom = mealTypeTO.HoursFrom;
            this.HoursTo = mealTypeTO.HoursTo;
        }

        /// <summary>
        /// Prepare TO for DAO processing
        /// </summary>
        /// <returns></returns>
        public MealTypeTO sendTransferObject()
        {
            MealTypeTO mealTypeTO = new MealTypeTO();

            mealTypeTO.MealTypeID = this.MealTypeID;
            mealTypeTO.Name = this.Name;
            mealTypeTO.Description = this.Description;
            mealTypeTO.HoursFrom = this.HoursFrom;
            mealTypeTO.HoursTo = this.HoursTo;
           
            return mealTypeTO;
        }

        public int getMaxID()
        {
            int maxID = -1;
            try
            {
                maxID = mealTypeDAO.getMaxID();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.getMaxID(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return maxID;
        }
    }
}
