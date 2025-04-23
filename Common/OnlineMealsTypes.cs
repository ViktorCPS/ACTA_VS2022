using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using Util;
using System.Data;
using DataAccess;

namespace Common
{
   public class OnlineMealsTypes
    {

        OnlineMealsTypesTO onlineMealsTypesTO = new OnlineMealsTypesTO();

        public OnlineMealsTypesTO OnlineMealsTypesTO
        {
            get { return onlineMealsTypesTO; }
            set { onlineMealsTypesTO = value; }
        }

       
        DebugLog log;
        private OnlineMealsTypesDAO mealsTypesDAO;
        private DAOFactory daoFactory;

       

        public OnlineMealsTypes()
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);



            daoFactory = DAOFactory.getDAOFactory();
            mealsTypesDAO = daoFactory.getOnlineMealsTypesDAO(null);
        }

        public OnlineMealsTypes(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);


            daoFactory = DAOFactory.getDAOFactory();
            mealsTypesDAO = daoFactory.getOnlineMealsTypesDAO(dbConnection);
        }


        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = mealsTypesDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsTypes.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                mealsTypesDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsTypes.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                mealsTypesDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsTypes.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return mealsTypesDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsTypes.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                mealsTypesDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsTypes.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<OnlineMealsTypesTO> Search()
        {
            try
            {
                return mealsTypesDAO.getTypes(this.OnlineMealsTypesTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsTypes.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(int mealTypeId)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = mealsTypesDAO.delete(mealTypeId);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsTypes.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }

        public int Save(string name, string Description)
        {
            int saved = 0;
            try
            {
                saved = mealsTypesDAO.insert(name, Description);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsTypes.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

        public bool Update(int mealTypeID, string name, string description)
        {
            bool isUpadated = false;

            try
            {
                isUpadated = mealsTypesDAO.update(mealTypeID, name, description);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsTypes.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpadated;
        }
    }
}
