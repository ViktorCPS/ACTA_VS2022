using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class MealUsedTO
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

        public int PointID
        {
            get { return _pointId; }
            set { _pointId = value; }
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

        public MealUsedTO()
        {
            TransID = "";
            EmployeeID = -1;
            EventTime = new DateTime();
            PointID = -1;
            MealTypeID = -1;
            Quantity = -1;
            MoneyAmount = -1;

        }
        public MealUsedTO(string transId, int employeeId, DateTime eventTime, int pointId, int mealTypeId, int qty, int moneyAmount)
        {
            TransID = transId;
            EmployeeID = employeeId;
            EventTime = eventTime;
            PointID = pointId;
            MealTypeID = mealTypeId;
            Quantity = qty;
            MoneyAmount = moneyAmount;
        }
    }
}
