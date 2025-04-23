using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeeTypeVisibilityTO
    {
        private int _employeeTypeID = -1;
        private int _workingUnitID = -1;
        private int _applUserCategoryID = -1;
        private int _value = -1;
        
        public int EmployeeTypeID
        {
            get { return _employeeTypeID; }
            set { _employeeTypeID = value; }
        }

        public int WorkingUnitID
        {
            get { return _workingUnitID; }
            set { _workingUnitID = value; }
        }

        public int ApplUserCategoryID
        {
            get { return _applUserCategoryID; }
            set { _applUserCategoryID = value; }
        }

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public EmployeeTypeVisibilityTO()
        { }
    }
}
