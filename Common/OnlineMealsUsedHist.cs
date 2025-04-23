using System;
using System.Collections.Generic;
using System.Text;
using Util;
using DataAccess;
using TransferObjects;
using System.Data;

namespace Common
{
    public class OnlineMealsUsedHist
    {
        DebugLog log;
        private OnlineMealsUsedHistDAO mealsUsedDAO;
        private DAOFactory daoFactory;

        OnlineMealsUsedHistTO onlineMealsPointTO = new OnlineMealsUsedHistTO();

        public OnlineMealsUsedHistTO OnlineMealsUsedHistTO
        {
            get { return onlineMealsPointTO; }
            set { onlineMealsPointTO = value; }
        }



        public OnlineMealsUsedHist()
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            mealsUsedDAO = daoFactory.getOnlineMealsUsedHistDAO(null);
        }

        public OnlineMealsUsedHist(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);


            daoFactory = DAOFactory.getDAOFactory();
            mealsUsedDAO = daoFactory.getOnlineMealsUsedHistDAO(dbConnection);
        }

        public List<OnlineMealsUsedHistTO> SearchMealUsed(string employeeIDs, DateTime from, DateTime to, string restaurantIDs, string pointsIDs, string mealTypeIDs, int qtyFrom, int qtyTO, int manual_created, int online_validation, int auto_check, string reasonsReader, string reasonsAutoCheck, string status, string operater, DateTime approvedFrom, DateTime approvedTo, DateTime modifiedFrom, DateTime modifiedTo)
        {

            List<OnlineMealsUsedHistTO> mealUsedTOList = new List<OnlineMealsUsedHistTO>();

            try
            {

                mealUsedTOList = mealsUsedDAO.getMealsUsed(employeeIDs, from, to, restaurantIDs, pointsIDs, mealTypeIDs, qtyFrom, qtyTO, manual_created, online_validation, auto_check, reasonsReader, reasonsAutoCheck, status, operater, approvedFrom, approvedTo,modifiedFrom,modifiedTo);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedHist.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealUsedTOList;
        }
        public List<OnlineMealsUsedHistTO> SearchMealUsed(string transactionID)
        {

            List<OnlineMealsUsedHistTO> mealUsedTOList = new List<OnlineMealsUsedHistTO>();

            try
            {

                mealUsedTOList = mealsUsedDAO.getMealsUsed(transactionID);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedHist.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealUsedTOList;
        }
     

        public int Save(bool doCommit)
        {
            try
            {
                return mealsUsedDAO.insert(this.OnlineMealsUsedHistTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedHist.Save(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " OnlineMealsUsedHist.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " OnlineMealsUsedHist.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " OnlineMealsUsedHist.RollbackTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " OnlineMealsUsedHist.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " OnlineMealsUsedHist.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

      

    }
}

