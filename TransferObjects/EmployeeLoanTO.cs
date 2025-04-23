using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
   public class EmployeeLoanTO
    {
        private uint _recID = 0;
        private int _employeeID = -1;
        private DateTime _dateStart = new DateTime();
        private DateTime _dateEnd = new DateTime();
        private int _workingUnitID = -1;
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();

        public int WorkingUnitID
        {
            get { return _workingUnitID; }
            set { _workingUnitID = value; }
        }

        public DateTime DateEnd
        {
            get { return _dateEnd; }
            set { _dateEnd = value; }
        }

        public DateTime DateStart
        {
            get { return _dateStart; }
            set { _dateStart = value; }
        }

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public uint RecID
        {
            get { return _recID; }
            set { _recID = value; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public DateTime ModifiedTime
        {
            get { return _modifiedTime; }
            set { _modifiedTime = value; }
        }
    }
}
