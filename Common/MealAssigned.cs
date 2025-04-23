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
    public class MealAssigned
    {
        int _employeeID;
        int _mealTypeID;
        DateTime _validFrom;
        DateTime _validTo;
        int _quantity;
        int _quantityDaily;
        int moneyAmount;
        string _employeeLastName;
        string _employeeFirstName;
        string _mealTypeName;
        int _recID;

        private DAOFactory daoFactory;
        private MealAssignedDAO mealAssignedDAO;

        DebugLog log;

        public int RecID
        {
            get { return _recID; }
            set { _recID = value; }
        }

        public string MealTypeName
        {
            get { return _mealTypeName; }
            set { _mealTypeName = value; }
        }

        public string EmployeeFirstName
        {
            get { return _employeeFirstName; }
            set { _employeeFirstName = value; }
        }

        public string EmployeeLastName
        {
            get { return _employeeLastName; }
            set { _employeeLastName = value; }
        }      

        public int MoneyAmount
        {
            get { return moneyAmount; }
            set { moneyAmount = value; }
        }

        public int QuantityDaily
        {
            get { return _quantityDaily; }
            set { _quantityDaily = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public DateTime ValidTo
        {
            get { return _validTo; }
            set { _validTo = value; }
        }

        public DateTime ValidFrom
        {
            get { return _validFrom; }
            set { _validFrom = value; }
        }

        public int MealTypeID
        {
            get { return _mealTypeID; }
            set { _mealTypeID = value; }
        }

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public MealAssigned()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

            EmployeeID = -1;
            MealTypeID = -1;
            ValidFrom = new DateTime();
            ValidTo = new DateTime();
            Quantity = -1;
            QuantityDaily = -1;
            MoneyAmount = -1;
            EmployeeFirstName = "";
            EmployeeLastName = "";
            MealTypeName = "";
            RecID = -1;

			daoFactory = DAOFactory.getDAOFactory();
            mealAssignedDAO = daoFactory.getMealAssignedDAO(null);
        }
        public MealAssigned(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            EmployeeID = -1;
            MealTypeID = -1;
            ValidFrom = new DateTime();
            ValidTo = new DateTime();
            Quantity = -1;
            QuantityDaily = -1;
            MoneyAmount = -1;
            EmployeeFirstName = "";
            EmployeeLastName = "";
            MealTypeName = "";
            RecID = -1;

            daoFactory = DAOFactory.getDAOFactory();
            mealAssignedDAO = daoFactory.getMealAssignedDAO(dbConnection);
        }

        public bool Delete(string recId)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = this.mealAssignedDAO.delete(recId);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssigned.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }

        public bool Save(int employeeID, int mealTypeID, DateTime validFrom, DateTime validTo, int quantity, int quantityDaily, int moneyAmount)
        {
            bool saved = false;
            try
            {
                saved = mealAssignedDAO.insert(employeeID, mealTypeID, validFrom, validTo, quantity, quantityDaily, moneyAmount);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssigned.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

        public ArrayList Search(string employeeID, string mealTypeID, DateTime validFrom, DateTime validTo)
        {
            ArrayList mealAssignedTOList = new ArrayList();
            ArrayList mealassignedList = new ArrayList();
            try
            {
                MealAssigned mealAssignedMember = new MealAssigned();
                mealAssignedTOList = mealAssignedDAO.getMealsAssigned(employeeID, mealTypeID, validFrom, validTo);
                foreach (MealAssignedTO mealAssignedTO in mealAssignedTOList)
                {
                    mealAssignedMember = new MealAssigned();
                    mealAssignedMember.receiveTransferObject(mealAssignedTO);
                    mealassignedList.Add(mealAssignedMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssigned.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealassignedList;
        }

        public int SearchCount(string employeeID, string mealTypeID, DateTime validFrom, DateTime validTo)
        {
            int count = 0;
            try
            {
                count = mealAssignedDAO.getMealsAssignedCount(employeeID, mealTypeID, validFrom, validTo);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssigned.SearchCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return count;
        }

        public ArrayList Search(int employeeID, int mealTypeID, DateTime validFrom, DateTime validTo,
            int qtyFrom, int qtyTo, int qtyDailyFrom, int qtyDailyTo, int mAmtFrom, int mAmtTo, bool unlimited)
        {
            ArrayList mealAssignedTOList = new ArrayList();
            ArrayList mealassignedList = new ArrayList();
            try
            {
                MealAssigned mealAssignedMember = new MealAssigned();
                mealAssignedTOList = mealAssignedDAO.getMealsAssigned(employeeID, mealTypeID, validFrom, validTo,
                    qtyFrom, qtyTo, qtyDailyFrom, qtyDailyTo, mAmtFrom, mAmtTo, unlimited);
                foreach (MealAssignedTO mealAssignedTO in mealAssignedTOList)
                {
                    mealAssignedMember = new MealAssigned();
                    mealAssignedMember.receiveTransferObject(mealAssignedTO);
                    mealassignedList.Add(mealAssignedMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssigned.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealassignedList;
        }

        public ArrayList Search(string employeeID)
        {
            ArrayList mealAssignedTOList = new ArrayList();
            ArrayList mealassignedList = new ArrayList();
            try
            {
                MealAssigned mealAssignedMember = new MealAssigned();
                mealAssignedTOList = mealAssignedDAO.getMealsAssigned(employeeID);
                foreach (MealAssignedTO mealAssignedTO in mealAssignedTOList)
                {
                    mealAssignedMember = new MealAssigned();
                    mealAssignedMember.receiveTransferObject(mealAssignedTO);
                    mealassignedList.Add(mealAssignedMember);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssigned.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mealassignedList;
        }

        public int SearchCount(int employeeID, int mealTypeID, DateTime validFrom, DateTime validTo,
            int qtyFrom, int qtyTo, int qtyDailyFrom, int qtyDailyTo, int mAmtFrom, int mAmtTo, bool unlimited)
        {
            int count = 0;
            try
            {
                count = mealAssignedDAO.getMealsAssignedCount(employeeID, mealTypeID, validFrom, validTo,
                    qtyFrom, qtyTo, qtyDailyFrom, qtyDailyTo, mAmtFrom, mAmtTo, unlimited);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssigned.SearchCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return count;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = mealAssignedDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssigned.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                mealAssignedDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssigned.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                mealAssignedDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssigned.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return mealAssignedDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssigned.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                mealAssignedDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssigned.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void receiveTransferObject(MealAssignedTO mealAssignedTO)
        {
            this.EmployeeID = mealAssignedTO.EmployeeID;
            this.MealTypeID = mealAssignedTO.MealTypeID;
            this.ValidFrom = mealAssignedTO.ValidFrom;
            this.ValidTo = mealAssignedTO.ValidTo;
            this.Quantity= mealAssignedTO.Quantity;
            this.QuantityDaily = mealAssignedTO.QuantityDaily;
            this.moneyAmount = mealAssignedTO.MoneyAmount;
            this.EmployeeLastName = mealAssignedTO.EmployeeLastName;
            this.EmployeeFirstName = mealAssignedTO.EmployeeFirstName;
            this.MealTypeName = mealAssignedTO.MealTypeName;
            this.RecID = mealAssignedTO.RecID;
        }

        /// <summary>
        /// Prepare TO for DAO processing
        /// </summary>
        /// <returns></returns>
        public MealAssignedTO sendTransferObject()
        {
            MealAssignedTO mealAssignedTO = new MealAssignedTO();

            mealAssignedTO.EmployeeID = this.EmployeeID;
            mealAssignedTO.MealTypeID = this.MealTypeID;
            mealAssignedTO.ValidFrom = this.ValidFrom;
            mealAssignedTO.ValidTo = this.ValidTo;
            mealAssignedTO.Quantity = this.QuantityDaily;
            mealAssignedTO.MoneyAmount = this.moneyAmount;
            mealAssignedTO.MealTypeName = this.MealTypeName;
            mealAssignedTO.EmployeeFirstName = this.EmployeeFirstName;
            mealAssignedTO.EmployeeLastName = this.EmployeeLastName;
            mealAssignedTO.RecID = this.RecID;

            return mealAssignedTO;
        }
    }
}
