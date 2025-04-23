using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
   public class EmployeeVacEvidScheduleTO
    {
        private int _employeeID = -1;
        private DateTime _vacYear = new DateTime();
        private int _segment = -1;
        private DateTime  _startDate = new DateTime();
        private DateTime _endDate = new DateTime();
        private string _status = "";

        public EmployeeVacEvidScheduleTO()
        { }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public int Segment
        {
            get { return _segment; }
            set { _segment = value; }
        }

        public DateTime VacYear
        {
            get { return _vacYear; }
            set { _vacYear = value; }
        }

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }
    }
}
