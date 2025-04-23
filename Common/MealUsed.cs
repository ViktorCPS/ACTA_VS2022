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
    public class MealUsed
    {
        string _transId;
        int _employeeId;
        DateTime _eventTime;
        int _pointId;
        string _pointName;
        int _mealTypeId;
        string _mealTypeName;
        int _quantity;
        int _moneyAmount;
        string _employeeName;
        int _workingUnitID;
        string _workingUnit;

        private DAOFactory daoFactory;
        private MealUsedDAO mealUsedDAO;

        DebugLog log;

        public int MoneyAmount
        {
            get { return _moneyAmount; }
            set { _moneyAmount = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public int MealTypeID
        {
            get { return _mealTypeId; }
            set { _mealTypeId = value; }
        }

        public string MealTypeName
        {
            get { return _mealTypeName; }
            set { _mealTypeName = value; }
        }

        public string PointName
        {
            get { return _pointName; }
            set { _pointName = value; }
        }

        public int PointID
        {
            get { return _pointId; }
            set { _pointId = value; }
        }

        public DateTime EventTime
        {
            get { return _eventTime; }
            set { _eventTime = value; }
        }

        public int EmployeeID
        {
            get { return _employeeId; }
            set { _employeeId = value; }
        }

        public string TransID
        {
            get { return _transId; }
            set { _transId = value; }
        }
        
        public string EmployeeName
        {
            get { return _employeeName; }
            set { _employeeName = value; }
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

        public MealUsed()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

            TransID = "";
            EmployeeID = -1;
            EventTime = new DateTime();
            PointID = -1;
            MealTypeID = -1;
            Quantity = -1;
            MoneyAmount = -1;

			daoFactory = DAOFactory.getDAOFactory();
            mealUsedDAO = daoFactory.getMealUsedDAO(null);
        }

        public MealUsed(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            TransID = "";
            EmployeeID = -1;
            EventTime = new DateTime();
            PointID = -1;
            MealTypeID = -1;
            Quantity = -1;
            MoneyAmount = -1;

            daoFactory = DAOFactory.getDAOFactory();
            mealUsedDAO = daoFactory.getMealUsedDAO(dbConnection);
        }

        public bool Delete(string TransId)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = this.mealUsedDAO.delete(TransId);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealUsed.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }

        public int getTransID()
        {
            int count = 0;
            try
            {
                count = mealUsedDAO.getTransID();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealUsed.getTransID(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return count;
        }
        public int Save(string transId, int employeeId, DateTime eventTime, int pointId, int mealTypeId, int quantity, int moneyAmount)
        {
            int saved = 0;
            try
            {
                saved = mealUsedDAO.insert(transId, employeeId, eventTime, pointId, mealTypeId, quantity, moneyAmount);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealUsed.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

        public ArrayList Search(int transId, int employeeId, DateTime eventTime, int pointId, int mealTypeId, int quantity, int moneyAmount)
        {
            ArrayList mealUsedTOList = new ArrayList();
            ArrayList mealUsedList = new ArrayList();
            try
            {
                MealUsed mealUsedMember = new MealUsed();
                mealUsedTOList = mealUsedDAO.getMealsUsed(transId, employeeId, eventTime, pointId, mealTypeId, quantity, moneyAmount);
                foreach (MealUsedTO mealUsedTO in mealUsedTOList)
                {
                    mealUsedMember = new MealUsed();
                    mealUsedMember.receiveTransferObject(mealUsedTO);
                    mealUsedList.Add(mealUsedMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealUsed.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealUsedList;
        }

        public ArrayList Search(int employeeId, DateTime from, DateTime to, int pointId, int mealTypeId, int qtyFrom, int qtyTo, int moneyAmtFrom, int moneyAmtTo, string wUnits)
        {
            ArrayList mealUsedTOList = new ArrayList();
            ArrayList mealUsedList = new ArrayList();
            try
            {
                MealUsed mealUsedMember = new MealUsed();
                mealUsedTOList = mealUsedDAO.getMealsUsed(employeeId, from, to, pointId, mealTypeId, qtyFrom, qtyTo, moneyAmtFrom, moneyAmtTo, wUnits);
                foreach (MealUsedTO mealUsedTO in mealUsedTOList)
                {
                    mealUsedMember = new MealUsed();
                    mealUsedMember.receiveTransferObject(mealUsedTO);
                    mealUsedList.Add(mealUsedMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealUsed.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealUsedList;
        }

        public ArrayList Search(int employeeID, DateTime from, DateTime to)
        {
            ArrayList mealUsedTOList = new ArrayList();
            ArrayList mealUsedList = new ArrayList();
            try
            {
                MealUsed mealUsedMember = new MealUsed();
                mealUsedTOList = mealUsedDAO.getMealsUsed(employeeID, from, to);
                foreach (MealUsedTO mealUsedTO in mealUsedTOList)
                {
                    mealUsedMember = new MealUsed();
                    mealUsedMember.receiveTransferObject(mealUsedTO);
                    mealUsedList.Add(mealUsedMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealUsed.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealUsedList;
        }

        /// <summary>
        /// Comment: to.AddDays(1) will be done in DataAccess layer
        /// </summary>
        /// <returns>
        /// </returns>
        public ArrayList SearchNumberOfUsedMeals(DateTime from, DateTime to, string wuString)
        {
            ArrayList mealUsedTOList = new ArrayList();
            ArrayList mealUsedList = new ArrayList();
            try
            {
                MealUsed mealUsedMember = new MealUsed();
                mealUsedTOList = mealUsedDAO.getNumerOfUsedMeals(from, to, wuString);
                foreach (MealUsedTO mealUsedTO in mealUsedTOList)
                {
                    mealUsedMember = new MealUsed();
                    mealUsedMember.receiveTransferObject(mealUsedTO);
                    mealUsedList.Add(mealUsedMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealUsed.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealUsedList;
        }


        public int SearchCount(int transId, int employeeId, DateTime eventTime, int pointId, int mealTypeId, int quantity, int moneyAmount)
        {
            int count = 0;
            try
            {
                count = mealUsedDAO.getMealsUsedCount(transId, employeeId, eventTime, pointId, mealTypeId, quantity, moneyAmount);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealUsed.SearchCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return count;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = mealUsedDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealUsed.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                mealUsedDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealUsed.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                mealUsedDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealUsed.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return mealUsedDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealUsed.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                mealUsedDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealUsed.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }


        public void receiveTransferObject(MealUsedTO mealUsedTO)
        {
            this.TransID = mealUsedTO.TransID;
            this.EmployeeID = mealUsedTO.EmployeeID;
            this.EventTime = mealUsedTO.EventTime;
            this.MealTypeID = mealUsedTO.MealTypeID;
            this.PointID = mealUsedTO.PointID;
            this.Quantity = mealUsedTO.Quantity;
            this.MoneyAmount = mealUsedTO.MoneyAmount;
            this.PointName = mealUsedTO.PointName;
            this.MealTypeName = mealUsedTO.MealTypeName;
            this.EmployeeName = mealUsedTO.EmployeeName;
            this.WorkingUnitID = mealUsedTO.WorkingUnitID;
            this.WorkingUnit = mealUsedTO.WorkingUnit;

        }

        /// <summary>
        /// Prepare TO for DAO processing
        /// </summary>
        /// <returns></returns>
        public MealUsedTO sendTransferObject()
        {
            MealUsedTO mealUsedTO = new MealUsedTO();

            mealUsedTO.TransID = this.TransID;
            mealUsedTO.EmployeeID = this.EmployeeID;
            mealUsedTO.MealTypeID = this.MealTypeID;
            mealUsedTO.EventTime = this.EventTime;
            mealUsedTO.PointID = this.PointID;
            mealUsedTO.Quantity = this.Quantity;
            mealUsedTO.MoneyAmount = this.MoneyAmount;
            mealUsedTO.PointName = this.PointName;
            mealUsedTO.MealTypeName = this.MealTypeName;
            mealUsedTO.EmployeeName = this.EmployeeName;
            mealUsedTO.WorkingUnitID = this.WorkingUnitID;
            mealUsedTO.WorkingUnit = this.WorkingUnit;

            return mealUsedTO;
        }
    }
}
