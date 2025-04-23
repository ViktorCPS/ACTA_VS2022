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
   public class MealsEmployeeSchedule
    {
         int _mealTypeID;
        DateTime  _date;
       int _employeeID;
       string _mealsType;
       string _employeeFirstName;
       string _employeeLastName;
       string _workingUnit;
       int _workingUnitID;
       string _shift;

     

        private DAOFactory daoFactory;
        private MealsEmployeeScheduleDAO mealEmplScheduleDAO;

        DebugLog log;       

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
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

        public string EmployeeLastName
        {
            get { return _employeeLastName; }
            set { _employeeLastName = value; }
        }

        public string EmployeeFirstName
        {
            get { return _employeeFirstName; }
            set { _employeeFirstName = value; }
        }

        public string MealsType
        {
            get { return _mealsType; }
            set { _mealsType = value; }
        }

        public int WorkingUnitID
        {
            get { return _workingUnitID; }
            set { _workingUnitID = value; }
        }

        public string WorkingUnit
        {
            get { return _workingUnit; }
            set { _workingUnit = value; }
        }

        public string Shift
        {
            get { return _shift; }
            set { _shift = value; }
        }

        public MealsEmployeeSchedule()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

            MealTypeID = -1;
            Date = new DateTime();
            EmployeeID = -1;
            EmployeeLastName = "";
            EmployeeFirstName = "";
            MealsType = "";
            WorkingUnit = "";
            Shift = "";

			daoFactory = DAOFactory.getDAOFactory();
			mealEmplScheduleDAO = daoFactory.getMealsEmployeeScheduleDAO(null);
        }

        public MealsEmployeeSchedule(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            MealTypeID = -1;
            Date = new DateTime();
            EmployeeID = -1;
            EmployeeLastName = "";
            EmployeeFirstName = "";
            MealsType = "";
            WorkingUnit = "";
            Shift = "";

            daoFactory = DAOFactory.getDAOFactory();
            mealEmplScheduleDAO = daoFactory.getMealsEmployeeScheduleDAO(dbConnection);
        }

       public MealsEmployeeSchedule(int mealTypeID, DateTime date, int employeeID)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            MealTypeID = mealTypeID;
            Date = date;
            EmployeeID = employeeID;
            EmployeeLastName = "";
            EmployeeFirstName = "";
            MealsType = "";
            WorkingUnit = "";
            Shift = "";

            daoFactory = DAOFactory.getDAOFactory();
            mealEmplScheduleDAO = daoFactory.getMealsEmployeeScheduleDAO(null);
        }

        public bool Delete(int mealTypeId, DateTime date,int employeeID)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = mealEmplScheduleDAO.delete(mealTypeId,date,employeeID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsEmployeeSchedule.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }

       public bool Delete(int mealTypeId, DateTime date, int employeeID, bool doCommit)
       {
           bool isDeleted = false;

           try
           {
               isDeleted = mealEmplScheduleDAO.delete(mealTypeId, date, employeeID, doCommit);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MealsEmployeeSchedule.Delete(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }

           return isDeleted;

       }

    

        public int Save(int mealTypeID, DateTime date, int employeeID, string shift)
        {
            int saved = 0;
            try
            {
                saved = mealEmplScheduleDAO.insert(mealTypeID, date, employeeID, shift);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsEmployeeSchedule.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }


        public bool Update(int mealTypeID, DateTime date, int employeeID, string shift)
        {
            bool isUpdated = false;

            try
            {
                isUpdated = mealEmplScheduleDAO.update(mealTypeID, date, employeeID, shift);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsEmployeeSchedule.Update(): " + ex.Message + "\n");
                throw ex;
            }
            return isUpdated;

        }



       public int Save(int mealTypeID, DateTime date, int employeeID,string shift, bool doCommit)
       {
           int saved = 0;
           try
           {
               saved = mealEmplScheduleDAO.insert(mealTypeID, date, employeeID, shift, doCommit);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MealsEmployeeSchedule.Save(): " + ex.Message + "\n");
               throw ex;
           }

           return saved;
       }

        public ArrayList Search(int mealTypeID, DateTime date, int employeeID)
        {
            ArrayList  mealTypeTOList = new ArrayList();
            ArrayList mealTypeList = new ArrayList();
            try
            {
                MealsEmployeeSchedule mealTypeMember = new MealsEmployeeSchedule();
                mealTypeTOList = mealEmplScheduleDAO.getMealsEmployeeSchedule(mealTypeID, date, employeeID);
                foreach (MealsEmployeeSchaduleTO mealTypeTO in mealTypeTOList)
                {
                    mealTypeMember = new MealsEmployeeSchedule();
                    mealTypeMember.receiveTransferObject(mealTypeTO);
                    mealTypeList.Add(mealTypeMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsEmployeeSchedule.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealTypeList;
        }

       public ArrayList Search(DateTime from, DateTime to, int WUID)
       {
           ArrayList mealTypeTOList = new ArrayList();
           ArrayList mealTypeList = new ArrayList();
           try
           {
               MealsEmployeeSchedule mealTypeMember = new MealsEmployeeSchedule();
               mealTypeTOList = mealEmplScheduleDAO.getMealsEmployeeSchedule(from, to, WUID);
               foreach (MealsEmployeeSchaduleTO mealTypeTO in mealTypeTOList)
               {
                   mealTypeMember = new MealsEmployeeSchedule();
                   mealTypeMember.receiveTransferObject(mealTypeTO);
                   mealTypeList.Add(mealTypeMember);
               }
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MealsEmployeeSchedule.Search(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }

           return mealTypeList;
       }

       public ArrayList SearchForEmpl(DateTime from, DateTime to, int employeeID)
       {
           ArrayList mealTypeTOList = new ArrayList();
           ArrayList mealTypeList = new ArrayList();
           try
           {
               MealsEmployeeSchedule mealTypeMember = new MealsEmployeeSchedule();
               mealTypeTOList = mealEmplScheduleDAO.getMealsEmployeeScheduleForEmpl(from, to, employeeID);
               foreach (MealsEmployeeSchaduleTO mealTypeTO in mealTypeTOList)
               {
                   mealTypeMember = new MealsEmployeeSchedule();
                   mealTypeMember.receiveTransferObject(mealTypeTO);
                   mealTypeList.Add(mealTypeMember);
               }
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MealsEmployeeSchedule.Search(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }

           return mealTypeList;
       }

       public ArrayList Search(DateTime from, DateTime to, string WUID, int emplID, int mealTypeID)
       {
           ArrayList mealTypeTOList = new ArrayList();
           ArrayList mealTypeList = new ArrayList();
           try
           {
               MealsEmployeeSchedule mealTypeMember = new MealsEmployeeSchedule();
               mealTypeTOList = mealEmplScheduleDAO.getScheduleForDateEmplMeal(from, to, WUID, emplID, mealTypeID);
               foreach (MealsEmployeeSchaduleTO mealTypeTO in mealTypeTOList)
               {
                   mealTypeMember = new MealsEmployeeSchedule();
                   mealTypeMember.receiveTransferObject(mealTypeTO);
                   mealTypeList.Add(mealTypeMember);
               }
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MealsEmployeeSchedule.Search(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }

           return mealTypeList;
       }

       public ArrayList SearchNumberOfScheduledMeals(DateTime from, DateTime to, string wuString)
       {
           ArrayList mealTypeTOList = new ArrayList();
           ArrayList mealTypeList = new ArrayList();
           try
           {
               MealsEmployeeSchedule mealTypeMember = new MealsEmployeeSchedule();
               mealTypeTOList = mealEmplScheduleDAO.getNumberOfScheduledMeals(from, to, wuString);
               foreach (MealsEmployeeSchaduleTO mealTypeTO in mealTypeTOList)
               {
                   mealTypeMember = new MealsEmployeeSchedule();
                   mealTypeMember.receiveTransferObject(mealTypeTO);
                   mealTypeList.Add(mealTypeMember);
               }
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MealsEmployeeSchedule.Search(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }

           return mealTypeList;
       }

       public Dictionary<DateTime, List<EmployeeTO>> SearchDateEndEmployeesWhoOrdered (DateTime from, DateTime to, string WUID)
       {
           Dictionary<DateTime, List<EmployeeTO>> mealOrdersDictionary = new Dictionary<DateTime, List<EmployeeTO>>();

           try
           {
               mealOrdersDictionary = mealEmplScheduleDAO.getDateEndEmployeesWhoOrdered(from, to, WUID);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MealsEmployeeSchedule.SearchDateEndEmployeesWhoOrdered(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }

           return mealOrdersDictionary;
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
                log.writeLog(DateTime.Now + " MealsEmployeeSchedule.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MealsEmployeeSchedule.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MealsEmployeeSchedule.RollbackTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MealsEmployeeSchedule.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MealsEmployeeSchedule.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void receiveTransferObject(MealsEmployeeSchaduleTO mealTypeSchTO)
        {
            this.MealTypeID = mealTypeSchTO.MealTypeID;
            this.Date = mealTypeSchTO.Date;
            this.EmployeeID = mealTypeSchTO.EmployeeID;
            this.MealsType = mealTypeSchTO.MealType;
            this.EmployeeFirstName = mealTypeSchTO.EmployeeFirstName;
            this.EmployeeLastName = mealTypeSchTO.EmployeeLastName;
            this.WorkingUnitID = mealTypeSchTO.WorkingUnitID;
            this.WorkingUnit = mealTypeSchTO.WorkingUnit;
            this.Shift = mealTypeSchTO.Shift;
        }

        /// <summary>
        /// Prepare TO for DAO processing
        /// </summary>
        /// <returns></returns>
        public MealsEmployeeSchaduleTO sendTransferObject()
        {
            MealsEmployeeSchaduleTO mealTypeSchTO = new MealsEmployeeSchaduleTO();

            mealTypeSchTO.MealTypeID = this.MealTypeID;
            mealTypeSchTO.Date = this.Date;
            mealTypeSchTO.MealType = this.MealsType;
            mealTypeSchTO.EmployeeID = this.EmployeeID;
            mealTypeSchTO.EmployeeFirstName = this.EmployeeFirstName;
            mealTypeSchTO.EmployeeLastName = this.EmployeeLastName;
            mealTypeSchTO.WorkingUnitID = this.WorkingUnitID;
            mealTypeSchTO.WorkingUnit = this.WorkingUnit;
            mealTypeSchTO.Shift = this.Shift;

            return mealTypeSchTO;
        }
    }
}
