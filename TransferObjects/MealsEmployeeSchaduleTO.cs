using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
     public class MealsEmployeeSchaduleTO
    {
        private int _mealTypeID = -1;
        private DateTime _date = new DateTime();
        private int _employeeID = -1;
        private string _shift = "";
        private string _employeeFirstName = "";
        private string _employeeLastName = "";
        private string _mealType = "";
        private int _workingUnitID = -1;
        private string _workingUnit = "";

        public string MealType
        {
            get { return _mealType; }
            set { _mealType = value; }
        }

        public string EmployeeLastName
        {
            get { return _employeeLastName; }
            set { _employeeLastName = value; }
        } 

        public string EmployeeFirstName
        {
            get { return _employeeFirstName; }
            set { _employeeFirstName = value; }
        }

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }


        public string Shift
        {
            get { return _shift; }
            set { _shift = value; }
        } 

        public int MealTypeID
        {
            get { return _mealTypeID; }
            set { _mealTypeID = value; }
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
    }
}
