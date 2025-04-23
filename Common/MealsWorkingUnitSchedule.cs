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
   public class MealsWorkingUnitSchedule
    {
         int _mealTypeID;
        DateTime  _date;
       int _workingUnitID;
       string _mealsType;
       string _workingUnitName;

     

        private DAOFactory daoFactory;
        private MealsWorkingUnitScheduleDAO mealEmplScheduleDAO;

        DebugLog log;       

        public int WorkingUnitID
        {
            get { return _workingUnitID; }
            set { _workingUnitID = value; }
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

        public string WorkingUnitName
        {
            get { return _workingUnitName; }
            set { _workingUnitName = value; }
        }

        public string MealsType
        {
            get { return _mealsType; }
            set { _mealsType = value; }
        }
        public MealsWorkingUnitSchedule()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

            MealTypeID = -1;
            Date = new DateTime();
            WorkingUnitID = -1;
            WorkingUnitName = "";
            MealsType = "";

			daoFactory = DAOFactory.getDAOFactory();
			mealEmplScheduleDAO = daoFactory.getMealsWorkingUnitScheduleDAO(null);
        }
        public MealsWorkingUnitSchedule(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            MealTypeID = -1;
            Date = new DateTime();
            WorkingUnitID = -1;
            WorkingUnitName = "";
            MealsType = "";

            daoFactory = DAOFactory.getDAOFactory();
            mealEmplScheduleDAO = daoFactory.getMealsWorkingUnitScheduleDAO(dbConnection);
        }

       public MealsWorkingUnitSchedule(int mealTypeID, DateTime date, int workingUnitID)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            MealTypeID = mealTypeID;
            Date = date;
            WorkingUnitID = workingUnitID;
            WorkingUnitName = "";
            MealsType = "";

            daoFactory = DAOFactory.getDAOFactory();
            mealEmplScheduleDAO = daoFactory.getMealsWorkingUnitScheduleDAO(null);
        }

        public bool Delete(int mealTypeId, DateTime date,int workingUnitID)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = mealEmplScheduleDAO.delete(mealTypeId,date,workingUnitID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedule.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }

       public bool Delete(int mealTypeId, DateTime date, int workingUnitID, bool doCommit)
       {
           bool isDeleted = false;

           try
           {
               isDeleted = mealEmplScheduleDAO.delete(mealTypeId, date, workingUnitID,doCommit);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MealsWorkingUnitSchedule.Delete(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }

           return isDeleted;

       }

    


        public int Save(int mealTypeID, DateTime date, int employeeID)
        {
            int saved = 0;
            try
            {
                saved = mealEmplScheduleDAO.insert(MealTypeID, date, employeeID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedule.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

       public int Save(int mealTypeID, DateTime date, int WUID, bool doCommit)
       {
           int saved = 0;
           try
           {
               saved = mealEmplScheduleDAO.insert(mealTypeID, date, WUID, doCommit);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MealsWorkingUnitSchedule.Save(): " + ex.Message + "\n");
               throw ex;
           }

           return saved;
       }

        public ArrayList Search(int mealTypeID, DateTime date, int workingUnitID)
        {
            ArrayList  mealTypeTOList = new ArrayList();
            ArrayList mealTypeList = new ArrayList();
            try
            {
                MealsWorkingUnitSchedule mealTypeMember = new MealsWorkingUnitSchedule();
                mealTypeTOList = mealEmplScheduleDAO.getMealsWUSchedule(mealTypeID, date, workingUnitID);
                foreach (MealsWorkingUnitScheduleTO mealTypeTO in mealTypeTOList)
                {
                    mealTypeMember = new MealsWorkingUnitSchedule();
                    mealTypeMember.receiveTransferObject(mealTypeTO);
                    mealTypeList.Add(mealTypeMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedule.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealTypeList;
        }

       public ArrayList Search(int mealTypeID, DateTime dateFrom, DateTime dateTo, int workingUnitID)
       {
           ArrayList mealTypeTOList = new ArrayList();
           ArrayList mealTypeList = new ArrayList();
           try
           {
               MealsWorkingUnitSchedule mealTypeMember = new MealsWorkingUnitSchedule();
               mealTypeTOList = mealEmplScheduleDAO.getMealsWUSchedule(mealTypeID, dateFrom,dateTo, workingUnitID);
               foreach (MealsWorkingUnitScheduleTO mealTypeTO in mealTypeTOList)
               {
                   mealTypeMember = new MealsWorkingUnitSchedule();
                   mealTypeMember.receiveTransferObject(mealTypeTO);
                   mealTypeList.Add(mealTypeMember);
               }
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MealsWorkingUnitSchedule.Search(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }

           return mealTypeList;
       }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = mealEmplScheduleDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedule.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                mealEmplScheduleDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedule.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                mealEmplScheduleDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedule.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return mealEmplScheduleDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedule.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                mealEmplScheduleDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedule.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

       public void receiveTransferObject(MealsWorkingUnitScheduleTO mealTypeSchTO)
        {
            this.MealTypeID = mealTypeSchTO.MealTypeID;
            this.Date = mealTypeSchTO.Date;
            this.WorkingUnitID = mealTypeSchTO.WorkingUnitID;
            this.MealsType = mealTypeSchTO.MealType;
            this.WorkingUnitName = mealTypeSchTO.WorkingUnit;
        }

        /// <summary>
        /// Prepare TO for DAO processing
        /// </summary>
        /// <returns></returns>
        public MealsWorkingUnitScheduleTO sendTransferObject()
        {
            MealsWorkingUnitScheduleTO mealTypeSchTO = new MealsWorkingUnitScheduleTO();

            mealTypeSchTO.MealTypeID = this.MealTypeID;
            mealTypeSchTO.Date = this.Date;
            mealTypeSchTO.MealType = this.MealsType;
            mealTypeSchTO.WorkingUnitID = this.WorkingUnitID;
            mealTypeSchTO.WorkingUnit = this.WorkingUnitName;

            return mealTypeSchTO;
        }
    }
}
