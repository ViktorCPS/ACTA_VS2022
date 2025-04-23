using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Util;
using System.Data;
using DataAccess;
using TransferObjects;

namespace Common
{
    public class OnlineMealsPoints
    {


        DebugLog log;
        private OnlineMealsPointsDAO mealsPointDAO;
        private DAOFactory daoFactory;

        OnlineMealsPointsTO onlineMealsPointTO = new OnlineMealsPointsTO();

        public OnlineMealsPointsTO OnlineMealsPointTO
        {
            get { return onlineMealsPointTO; }
            set { onlineMealsPointTO = value; }
        }



        public OnlineMealsPoints()
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);



            daoFactory = DAOFactory.getDAOFactory();
            mealsPointDAO = daoFactory.getOnlineMealsPointDAO(null);
        }

        public OnlineMealsPoints(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);


            daoFactory = DAOFactory.getDAOFactory();
            mealsPointDAO = daoFactory.getOnlineMealsPointDAO(dbConnection);
        }
        public int Save()
        {
            int saved = 0;
            try
            {
                saved = mealsPointDAO.insert(this.OnlineMealsPointTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsPoints.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }
        public OnlineMealsPointsTO find(int pointID)
        {
            OnlineMealsPointsTO rTo = new OnlineMealsPointsTO();
            try
            {
                rTo = mealsPointDAO.find(pointID.ToString());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsPoints.find(): " + ex.Message + "\n");
                throw ex;
            }
            return rTo;
        }
        public List<OnlineMealsPointsTO> Search(string ipAddress, int ant_num)
        {
            List<OnlineMealsPointsTO> mealUsedTOList = new List<OnlineMealsPointsTO>();

            try
            {

                mealUsedTOList = mealsPointDAO.getMealsPoints(ipAddress, ant_num);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealPoint.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealUsedTOList;
        }

        public List<OnlineMealsPointsTO> SearchByRestaurant(string restaurantID)
        {
            List<OnlineMealsPointsTO> mealUsedTOList = new List<OnlineMealsPointsTO>();

            try
            {

                mealUsedTOList = mealsPointDAO.findByRestaurant(restaurantID);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealPoint.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealUsedTOList;
        }


        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = mealsPointDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsPoint.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                mealsPointDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsPoint.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                mealsPointDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsPoint.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return mealsPointDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsPoint.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                mealsPointDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsPoint.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<OnlineMealsPointsTO> Search()
        {
            try
            {
                return mealsPointDAO.getMealsPoints(this.OnlineMealsPointTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsPoint.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<OnlineMealsPointsTO> searchForIDs(string pointIds)
        {
            try
            {
                return mealsPointDAO.searchForIDs(pointIds);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsPoint.Search(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public bool Update()
        {
            bool isUpadated = false;

            try
            {
                isUpadated = mealsPointDAO.update(this.OnlineMealsPointTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsPoint.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpadated;
        }

        public bool Delete(int pointID)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = mealsPointDAO.delete(pointID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsPoint.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }
    }
}
