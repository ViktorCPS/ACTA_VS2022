using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using Util;
using DataAccess;

namespace Common
{
   public class OnlineMealsRestaurant
    {
        OnlineMealsRestaurantTO onlineMealsRestaurant = new OnlineMealsRestaurantTO();

        DebugLog log;
        private OnlineMealsRestaurantDAO mealsRestaurantDAO;
        private DAOFactory daoFactory;
        public OnlineMealsRestaurantTO OnlineMealsRestaurant1
        {
            get { return onlineMealsRestaurant; }
            set { onlineMealsRestaurant = value; }
        }

        public OnlineMealsRestaurant()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

          

			daoFactory = DAOFactory.getDAOFactory();
            mealsRestaurantDAO = daoFactory.getOnlineMealsRestaurantDAO(null);
        }

        public OnlineMealsRestaurant(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            

            daoFactory = DAOFactory.getDAOFactory();
            mealsRestaurantDAO = daoFactory.getOnlineMealsRestaurantDAO(dbConnection);
        }

        public List<OnlineMealsRestaurantTO> Search()
        {
            try
            {
                return mealsRestaurantDAO.getRestaurants(this.OnlineMealsRestaurant1);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsRestaurants.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }


        public int Save(string name, string Description)
        {
            int saved = 0;
            try
            {
                saved = mealsRestaurantDAO.insert(name, Description);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsRestaurants.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

        public bool Update(int restaurantID, string name, string description)
        {
            bool isUpadated = false;

            try
            {
                isUpadated = mealsRestaurantDAO.update(restaurantID, name, description);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsRestaurants.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpadated;
        }



        public bool Delete(int restaurantId)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = mealsRestaurantDAO.delete(restaurantId);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsRestaurants.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }
    }
}
