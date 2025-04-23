using System;
using System.Collections.Generic;
using System.Text;
using Util;
using DataAccess;
using System.Data;
using TransferObjects;
using System.Collections;

namespace Common
{
    public class OnlineMealsUsed
    {

        DebugLog log;
        private OnlineMealsUsedDAO mealsUsedDAO;
        private DAOFactory daoFactory;

        OnlineMealsUsedTO onlineMealsPointTO = new OnlineMealsUsedTO();

        public OnlineMealsUsedTO OnlineMealsUsedTO
        {
            get { return onlineMealsPointTO; }
            set { onlineMealsPointTO = value; }
        }

        public OnlineMealsUsed()
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            mealsUsedDAO = daoFactory.getOnlineMealsUsedDAO(null);
        }

        public OnlineMealsUsed(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);


            daoFactory = DAOFactory.getDAOFactory();
            mealsUsedDAO = daoFactory.getOnlineMealsUsedDAO(dbConnection);
        }

        public OnlineMealsUsedTO Find(string transactionID)
        {
            OnlineMealsUsedTO mealUsed = new OnlineMealsUsedTO();
            try
            {
                mealUsed = mealsUsedDAO.find(transactionID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealUsed.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return mealUsed;
        }

        public int CountByStatus(int online_validation, DateTime date, int pointID)
        {
            int mealUsed = 0;
            try
            {
                mealUsed = mealsUsedDAO.findByStatus(online_validation,date,pointID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealUsed.CountByStatus(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return mealUsed;
        }

        public List<OnlineMealsUsedTO> SearchMealUsed(string employeeIDs, DateTime from, DateTime to, string restaurantIDs, string pointsIDs, string mealTypeIDs, int qtyFrom, int qtyTO, int manual_created, int online_validation, int auto_check, string reasonsReader, string reasonsAutoCheck, string status, string operater, DateTime approvedFrom, DateTime approvedTo) {

            List<OnlineMealsUsedTO> mealUsedTOList = new List<OnlineMealsUsedTO>();

            try
            {

                mealUsedTOList = mealsUsedDAO.getMealsUsed(employeeIDs,from,to,restaurantIDs,pointsIDs,mealTypeIDs,qtyFrom, qtyTO,manual_created,online_validation,auto_check,reasonsReader,reasonsAutoCheck,status, operater, approvedFrom,approvedTo);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealUsed.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealUsedTOList;
        }

        public List<OnlineMealsUsedTO> Search(int employeeID, DateTime from, DateTime to)
        {
            List<OnlineMealsUsedTO> mealUsedTOList = new List<OnlineMealsUsedTO>();

            try
            {

                mealUsedTOList = mealsUsedDAO.getMealsUsed(employeeID, from, to);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealUsed.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealUsedTOList;
        }

        public List<OnlineMealsUsedTO> Search(string employeeIDs, DateTime from, DateTime to)
        {
            try
            {
                return mealsUsedDAO.getMealsUsed(employeeIDs, from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealUsed.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<OnlineMealsUsedTO> Search(string IDs)
        {
            try
            {
                return mealsUsedDAO.getMealsUsed(IDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealUsed.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public int Count(int employeeID, DateTime from, DateTime to)
        {
            int mealUsedTO = 0;

            try
            {

                mealUsedTO= mealsUsedDAO.getOnlineMealUsedCount(employeeID, from, to);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealUsed.Count(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealUsedTO;
        }
        public Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>> SearchMealsUsedDict(string employees, DateTime from, DateTime to)
        {
            try
            {
                return mealsUsedDAO.getMealsUsedDict(employees, from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealUsed.SearchMealsUsedDict(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>> SearchMealsForValidation(DateTime from, DateTime to, string emplIDs)
        {
            try
            {
                return mealsUsedDAO.getMealsForValidation(from, to, emplIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealUsed.SearchMealsForValidation(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>> SearchMealsValidated(DateTime from, DateTime to, DateTime fromValidation, DateTime toValidation, string emplIDs)
        {
            try
            {
                return mealsUsedDAO.getMealsValidated(from, to, fromValidation, toValidation, emplIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealUsed.SearchMealsValidated(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Update(bool doCommit)
        {
            try
            {
                return mealsUsedDAO.update(this.OnlineMealsUsedTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealUsed.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int Save(bool doCommit)
        {            
            try
            {
                return mealsUsedDAO.insert(this.OnlineMealsUsedTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealUsed.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = mealsUsedDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                mealsUsedDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                mealsUsedDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return mealsUsedDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                mealsUsedDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<OnlineMealsUsedTO> Search(int employeeId, DateTime from, DateTime to, int pointId, int mealTypeId, int qtyFrom, int qtyTo, string wUnits)
        {
            List<OnlineMealsUsedTO> mealUsedTOList = new List<OnlineMealsUsedTO>();

            try
            {

                mealUsedTOList = mealsUsedDAO.getMealsUsed(employeeId, from, to, pointId, mealTypeId, qtyFrom, qtyTo, wUnits);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealUsedTOList;
        }

    }
}
