using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SyncBufferDataTO
    {
        private int _recID = -1;
        private int _employeeID = -1;
        private int _daysOfHolidayForCurrentYearLeft = -1;
        private int _daysOfHolidayForPreviousYearLeft = -1;
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        
        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }
        public int DaysOfHolidayForPreviousYearLeft
        {
            get { return _daysOfHolidayForPreviousYearLeft; }
            set { _daysOfHolidayForPreviousYearLeft = value; }
        }

        public int DaysOfHolidayForCurrentYearLeft
        {
            get { return _daysOfHolidayForCurrentYearLeft; }
            set { _daysOfHolidayForCurrentYearLeft = value; }
        }
        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public int RecID
        {
            get { return _recID; }
            set { _recID = value; }
        }
    }
}
