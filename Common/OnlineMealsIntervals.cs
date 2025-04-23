using System;
using System.Collections.Generic;
using System.Text;
using Util;
using TransferObjects;
using DataAccess;

namespace Common
{
  public class OnlineMealsIntervals
    {
       DebugLog log;
       private OnlineMealsIntervalsDAO mealsUsedIntervalsDAO;
        private DAOFactory daoFactory;

        OnlineMealsIntervalsTO onlineMealsIntervalsTO = new OnlineMealsIntervalsTO();

        public OnlineMealsIntervalsTO OnlineMealsUsedTO
        {
            get { return onlineMealsIntervalsTO; }
            set { onlineMealsIntervalsTO = value; }
        }

        public OnlineMealsIntervals()
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            mealsUsedIntervalsDAO = daoFactory.getOnlineMealsIntervalsDAO(null);
        }

        public OnlineMealsIntervals(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);


            daoFactory = DAOFactory.getDAOFactory();
            mealsUsedIntervalsDAO = daoFactory.getOnlineMealsIntervalsDAO(dbConnection);
        }
        public List<OnlineMealsIntervalsTO> SearchAll(string type)
        {
            List<OnlineMealsIntervalsTO> mealUsedIntervals = new List<OnlineMealsIntervalsTO>();
            try
            {
                mealUsedIntervals = mealsUsedIntervalsDAO.getAll(type);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealUsed.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return mealUsedIntervals;
        }

    }
}
