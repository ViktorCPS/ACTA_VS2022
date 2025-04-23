using System;
using System.Collections.Generic;
using System.Text;
using Util;
using TransferObjects;
using DataAccess;
using System.Data;

namespace Common
{
    public class OnlineMealsUsedDaily
    {
        DebugLog log;
        private OnlineMealsUsedDailyDAO mealsUsedDAO;
        private DAOFactory daoFactory;

        OnlineMealsUsedTO onlineMealsPointTO = new OnlineMealsUsedTO();

        public OnlineMealsUsedTO OnlineMealsUsedTO
        {
            get { return onlineMealsPointTO; }
            set { onlineMealsPointTO = value; }
        }

        public OnlineMealsUsedDaily()
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            mealsUsedDAO = daoFactory.getOnlineMealsUsedDailyDAO(null);
        }

        public OnlineMealsUsedDaily(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);


            daoFactory = DAOFactory.getDAOFactory();
            mealsUsedDAO = daoFactory.getOnlineMealsUsedDailyDAO(dbConnection);
     
        }

        public int Save(bool doCommit, int transfer_flag)
        {
            try
            {
                return mealsUsedDAO.insert(this.OnlineMealsUsedTO, doCommit,transfer_flag);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedDaily.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<OnlineMealsUsedTO> GetMealsToTransfer()
        {
            try
            {
                return mealsUsedDAO.getMealsToTransfer();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedDaily.GetMealsToTransfer(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool UpdateToTransfered(string transIDs, bool doCommit)
        {
            try
            {
                return mealsUsedDAO.updateToTransfered(transIDs, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedDaily.UpdateToTransfered(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool DeletePreviousDay(bool doCommit)
        {
            try
            {
                return mealsUsedDAO.deletePreviousDay(doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedDaily.DeletePreviousDay(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public int CountByStatus(int online_validation, DateTime date, int pointID)
        {
            try
            {
                return mealsUsedDAO.findByStatus(online_validation, date, pointID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedDaily.CountByStatus(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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
                log.writeLog(DateTime.Now + " OnlineMealsUsedDaily.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " OnlineMealsUsedDaily.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " OnlineMealsUsedDaily.RollbackTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " OnlineMealsUsedDaily.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " OnlineMealsUsedDaily.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int Count(int employeeID, DateTime from, DateTime to)
        {
            int mealUsedTO = 0;

            try
            {

                mealUsedTO = mealsUsedDAO.getOnlineMealUsedCount(employeeID, from, to);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedDaily.Count(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealUsedTO;
        }
    }
}
