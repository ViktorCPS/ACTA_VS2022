using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class MealAssignedTO
    {
        int _employeeID;
        int _mealTypeID;
        DateTime _validFrom;
        DateTime _validTo;
        int _quantity;
        int _quantityDaily;
        int _moneyAmount;
        string _employeeLastName;
        string _employeeFirstName;
        string _mealTypeName;
        int _recID;

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
            get { return _moneyAmount; }
            set { _moneyAmount = value; }
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
        public MealAssignedTO()
        {
            this.EmployeeID = -1;
            this.MealTypeID = -1;
            this.ValidFrom = new DateTime();
            this.ValidTo = new DateTime();
            this.Quantity = -1;
            this.QuantityDaily = -1;
            this.MoneyAmount = -1;
            this.RecID = -1;
            EmployeeFirstName = "";
            EmployeeLastName = "";
            MealTypeName = "";
        }
        public MealAssignedTO(int employeeID, int mealTypeID, DateTime validFrom, DateTime validTo,int qty, int qtyDaily, int moneyAmount, string emplFirstName, string emplLastName, string mealTypeName)
        {
            this.EmployeeID = employeeID;
            this.MealTypeID = mealTypeID;
            this.ValidFrom = validFrom;
            this.ValidTo = validTo;
            this.Quantity = qty;
            this.QuantityDaily = qtyDaily;
            this.MoneyAmount = moneyAmount;
            EmployeeFirstName = emplFirstName;
            EmployeeLastName = emplLastName;
            MealTypeName = mealTypeName;
        }
    }
}
