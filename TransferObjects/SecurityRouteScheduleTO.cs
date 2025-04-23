using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SecurityRouteScheduleTO
    {
        private int _employeeID;
        private string _employeeName;
        private DateTime _date;
        private int _securityRouteID;
        private string _routeName;
        private string _type;
        private string _status;

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

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public int SecurityRouteID
        {
            get { return _securityRouteID; }
            set { _securityRouteID = value; }
        }

        public string RouteName
        {
            get { return _routeName; }
            set { _routeName = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public SecurityRouteScheduleTO()
		{
			// Init properties
            EmployeeID = -1;
            EmployeeName = "";
            Date = new DateTime();
            SecurityRouteID = -1;
			RouteName = "";
            Type = "";
            Status = "";
		}
    }
}
