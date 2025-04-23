using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using TransferObjects;
using Util;
using System.Collections;
using System.Data;

namespace Common
{
    public class MealsTypeSchedule
    {
          int _mealTypeID;
        DateTime  _date;
       string _mealType;
        DateTime _hoursFrom;
        DateTime _hoursTo;

        private DAOFactory daoFactory;
        private MealsTypeScheduleDAO mealTypeDAO;

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

        public string MealType
        {
            get { return _mealType; }
            set { _mealType = value; }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public int MealTypeID
        {
            get { return _mealTypeID; }
            set { _mealTypeID = value; }
        }

        public MealsTypeSchedule()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

            MealTypeID = -1;
            Date = new DateTime();
            MealType = "";
            HoursFrom = new DateTime();
            HoursTo = new DateTime();

			daoFactory = DAOFactory.getDAOFactory();
			mealTypeDAO = daoFactory.getMealsTypeScheduleDAO(null);
        }
        public MealsTypeSchedule(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            MealTypeID = -1;
            Date = new DateTime();
            MealType = "";
            HoursFrom = new DateTime();
            HoursTo = new DateTime();

            daoFactory = DAOFactory.getDAOFactory();
            mealTypeDAO = daoFactory.getMealsTypeScheduleDAO(dbConnection);
        }

        public MealsTypeSchedule(int mealTypeID, DateTime  date, string mealType, DateTime hoursFrom, DateTime hoursTo)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            MealTypeID = mealTypeID;
            Date = date;
            MealType = mealType;
            HoursFrom = hoursFrom;
            HoursTo = hoursTo;

            daoFactory = DAOFactory.getDAOFactory();
            mealTypeDAO = daoFactory.getMealsTypeScheduleDAO(null);
        }

        public bool Delete(int mealTypeId, DateTime date)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = mealTypeDAO.delete(mealTypeId,date);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeSchedule.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }

        public bool Delete(int mealTypeId, DateTime date, bool doCommit)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = mealTypeDAO.delete(mealTypeId, date,doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeSchedule.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }


        public bool Update(int mealTypeID, DateTime date, DateTime hoursFrom, DateTime hoursTo)
        {
            bool isUpadated = false;

            try
            {
                isUpadated = mealTypeDAO.update(mealTypeID, date, hoursFrom, hoursTo);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpadated;
        }


        public int Save(int mealTypeID, DateTime date, DateTime hoursFrom, DateTime hoursTo)
        {
            int saved = 0;
            try
            {
                saved = mealTypeDAO.insert(MealTypeID, date, hoursFrom, hoursTo);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

        public int Save(int mealTypeID, DateTime date, DateTime hoursFrom, DateTime hoursTo, bool doCommit)
        {
            int saved = 0;
            try
            {
                saved = mealTypeDAO.insert(mealTypeID, date, hoursFrom, hoursTo, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealType.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

        public ArrayList Search(int mealTypeID, DateTime dateFrom, DateTime dateTo)
        {
            ArrayList  mealTypeTOList = new ArrayList();
            ArrayList mealTypeList = new ArrayList();
            try
            {
                MealsTypeSchedule mealTypeMember = new MealsTypeSchedule();
                mealTypeTOList = mealTypeDAO.getMealsTypeSchedule(mealTypeID, dateFrom, dateTo);
                foreach (MealsTypeScheduleTO mealTypeTO in mealTypeTOList)
                {
                    mealTypeMember = new MealsTypeSchedule();
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

        public void receiveTransferObject(MealsTypeScheduleTO mealTypeSchTO)
        {
            this.MealTypeID = mealTypeSchTO.MealTypeID;
            this.Date = mealTypeSchTO.Date;
            this.MealType = mealTypeSchTO.MealType;
            this.HoursFrom = mealTypeSchTO.HrsFrom;
            this.HoursTo = mealTypeSchTO.HrsTo;
        }

        /// <summary>
        /// Prepare TO for DAO processing
        /// </summary>
        /// <returns></returns>
        public MealsTypeScheduleTO sendTransferObject()
        {
            MealsTypeScheduleTO mealTypeSchTO = new MealsTypeScheduleTO();

            mealTypeSchTO.MealTypeID = this.MealTypeID;
            mealTypeSchTO.Date = this.Date;
            mealTypeSchTO.MealType = this.MealType;
            mealTypeSchTO.HrsFrom = this.HoursFrom;
            mealTypeSchTO.HrsTo = this.HoursTo;

            return mealTypeSchTO;
        }

       
    }
}
