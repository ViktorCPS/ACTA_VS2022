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
    public class MealPoint
    {
        int _mealPointID;
        string _terminalSerial;
        string _name;
        string _description;

        private DAOFactory daoFactory;
        private MealsPointDAO mealsPointDAO;

        DebugLog log;

        public int MealPointID
        {
            get { return _mealPointID; }
            set { _mealPointID = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string TerminalSerial
        {
            get { return _terminalSerial; }
            set { _terminalSerial = value; }
        }

        public MealPoint()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

            MealPointID = -1;
            TerminalSerial = "";
            Name = "";
            Description = "";

			daoFactory = DAOFactory.getDAOFactory();
            mealsPointDAO = daoFactory.getMealsPointDAO(null);
        }

        public MealPoint(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            MealPointID = -1;
            TerminalSerial = "";
            Name = "";
            Description = "";

            daoFactory = DAOFactory.getDAOFactory();
            mealsPointDAO = daoFactory.getMealsPointDAO(dbConnection);
        }

        public MealPoint(int mealPointID, string terminalSerial, string name, string desc)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            MealPointID = mealPointID;
            TerminalSerial = terminalSerial;
            Name = name;
            Description = desc;

            daoFactory = DAOFactory.getDAOFactory();
            mealsPointDAO = daoFactory.getMealsPointDAO(null);
        }

        public bool Update(int mealPointID, string terminalSerial, string name, string description)
        {
            bool isUpdated = false;

            try
            {
                isUpdated = this.mealsPointDAO.update(mealPointID, terminalSerial, name, description);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsPoint.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool Delete(string pointsId)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = this.mealsPointDAO.delete(pointsId);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsPoint.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }

        public int Save(string terminalSerial, string name, string description)
        {
            int saved = 0;
            try
            {
                saved = mealsPointDAO.insert(terminalSerial, name, description);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsPoint.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

        public ArrayList Search(int pointId, string terminalSerial, string name, string description)
        {
            ArrayList mealsPointTOList = new ArrayList();
            ArrayList mealsPointList = new ArrayList();
            try
            {
                MealPoint mealsPointMember = new MealPoint();
                mealsPointTOList = mealsPointDAO.getMealsPoints(pointId, terminalSerial, name, description);
                foreach (MealsPointTO mealsPointTO in mealsPointTOList)
                {
                    mealsPointMember = new MealPoint();
                    mealsPointMember.receiveTransferObject(mealsPointTO);
                    mealsPointList.Add(mealsPointMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsPoint.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealsPointList;
        }


        public int SearchCount(int pointId, string terminalSerial, string name, string description)
        {
            int count = 0;
            try
            {
                count = mealsPointDAO.getMealsPointsCount(pointId, terminalSerial, name, description);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsPoint.SearchCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return count;
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
                log.writeLog(DateTime.Now + " MealsPoint.BeginTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MealsPoint.CommitTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MealsPoint.RollbackTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MealsPoint.GetTransaction(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MealsPoint.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void receiveTransferObject(MealsPointTO mealsPointTO)
        {
            this.MealPointID = mealsPointTO.PointID;
            this.TerminalSerial = mealsPointTO.TerminalSerial;
            this.Name = mealsPointTO.Name;
            this.Description = mealsPointTO.Description;            
        }

        /// <summary>
        /// Prepare TO for DAO processing
        /// </summary>
        /// <returns></returns>
        public MealsPointTO sendTransferObject()
        {
            MealsPointTO mealsPointTO = new MealsPointTO();

            mealsPointTO.PointID  = this.MealPointID;
            mealsPointTO.TerminalSerial = this.TerminalSerial;
            mealsPointTO.Name= this.Name;
            mealsPointTO.Description = this.Description;
           
            return mealsPointTO;
        }
    }
}
