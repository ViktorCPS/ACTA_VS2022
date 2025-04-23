using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeePYTransportDataTO
    {
        private int _employeeID = -1;
		private DateTime _date = new DateTime();
		private int _transportTypeID = -1;

        public int EmployeeID
		{
            get { return _employeeID; }
            set { _employeeID = value; }
		}

		public DateTime Date
		{
			get { return _date; }
			set {_date = value; }
		}

		public int TransportTypeID
		{
            get { return _transportTypeID; }
            set { _transportTypeID = value; }
		}

		public EmployeePYTransportDataTO()
		{
		}

        public EmployeePYTransportDataTO(EmployeePYTransportDataTO dataTO)
		{
            this.EmployeeID = dataTO.EmployeeID;
            this.Date = dataTO.Date;
            this.TransportTypeID = dataTO.TransportTypeID;
		}
    }
}
