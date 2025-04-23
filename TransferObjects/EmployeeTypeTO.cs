using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeeTypeTO
    {
        private int _employeeTypeID = -1;
        private int _workingUnitID = -1;
        private string employeeTypeName = "";

        public string EmployeeTypeName
        {
            get { return employeeTypeName; }
            set { employeeTypeName = value; }
        }

        public int WorkingUnitID
        {
            get { return _workingUnitID; }
            set { _workingUnitID = value; }
        }

        public int EmployeeTypeID
        {
            get { return _employeeTypeID; }
            set { _employeeTypeID = value; }
        }
    }
}
