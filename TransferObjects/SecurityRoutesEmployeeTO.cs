using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SecurityRoutesEmployeeTO
    {
        private int _employeeID;
        private string _employeeName;
        private string _wuName;
                
        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public string EmployeeName
        {
            get { return _employeeName; }
            set { _employeeName = value; }
        }

        public string WUName
        {
            get { return _wuName; }
            set { _wuName = value; }
        }

        public SecurityRoutesEmployeeTO()
        {
            // Init properties
            EmployeeID = -1;
            EmployeeName = "";
            WUName = "";
        }
    }
}
