using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    public class EmployeeXMealTypeEmpl
    {
        int _employeeID;
        int _mealsTypeEmplID;

        private DAOFactory daoFactory;
        private EmployeeXMealTypeEmplDAO  employeeXMealTypeEmplDAO;

        DebugLog log;


        public int MealTypeEmplID
        {
            get { return _mealsTypeEmplID; }
            set { _mealsTypeEmplID = value; }
        }

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

          public EmployeeXMealTypeEmpl()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

            this.EmployeeID = -1;
            this.MealTypeEmplID = -1;

			daoFactory = DAOFactory.getDAOFactory();
            employeeXMealTypeEmplDAO = daoFactory.getEmployeeXMealTypeEmplDAO(null);
        }

          public EmployeeXMealTypeEmpl(object dbConnection)
          {
              string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
              log = new DebugLog(logFilePath);

              this.EmployeeID = -1;
              this.MealTypeEmplID = -1;

              daoFactory = DAOFactory.getDAOFactory();
              employeeXMealTypeEmplDAO = daoFactory.getEmployeeXMealTypeEmplDAO(dbConnection);
          }

        public bool Delete(string employeeID)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = this.employeeXMealTypeEmplDAO.delete(employeeID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMealTypeEmpl.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }

        public int Save(int employeeID, int mealTypeEmpl)
        {
            int saved = 0;
            try
            {
                saved = employeeXMealTypeEmplDAO.insert(employeeID,mealTypeEmpl);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMealTypeEmpl.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = employeeXMealTypeEmplDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMealTypeEmpl.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                employeeXMealTypeEmplDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMealTypeEmpl.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                employeeXMealTypeEmplDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMealTypeEmpl.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return employeeXMealTypeEmplDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMealTypeEmpl.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                employeeXMealTypeEmplDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeXMealTypeEmpl.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void receiveTransferObject(EmployeeXMealTypeEmplTO employeeXMealTypeEmplTO)
        {
            this.EmployeeID = employeeXMealTypeEmplTO.EmployeeID;
            this.MealTypeEmplID = employeeXMealTypeEmplTO.MealTypeEmplID;
           
        }

        /// <summary>
        /// Prepare TO for DAO processing
        /// </summary>
        /// <returns></returns>
        public EmployeeXMealTypeEmplTO sendTransferObject()
        {
            EmployeeXMealTypeEmplTO employeeXMealTypeEmplTO = new EmployeeXMealTypeEmplTO();

            employeeXMealTypeEmplTO.EmployeeID = this.EmployeeID;
            employeeXMealTypeEmplTO.MealTypeEmplID = this.MealTypeEmplID;
            
            return employeeXMealTypeEmplTO;
        }
    }
}
