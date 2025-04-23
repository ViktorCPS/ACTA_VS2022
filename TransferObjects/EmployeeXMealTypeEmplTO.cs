using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
   public class EmployeeXMealTypeEmplTO
    {
        int _employeeID;
        int _mealsTypeEmplID;

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
        public EmployeeXMealTypeEmplTO()
        {
            this.EmployeeID = -1;
            this.MealTypeEmplID = -1;
        }
        public EmployeeXMealTypeEmplTO(int employeeID,int mealsTypeEmplID)
        {
            this.EmployeeID = employeeID;
            this.MealTypeEmplID = mealsTypeEmplID;
        }
    }
}
