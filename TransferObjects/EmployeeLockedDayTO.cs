using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeeLockedDayTO
    {
        private uint _recID = 0;
        private int _emplID = -1;
        private DateTime _date = new DateTime();
        private string _type = "";
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();

        public uint RecID
        {
            get { return _recID; }
            set { _recID = value; }
        }

        public int EmployeeID
        {
            get { return _emplID; }
            set { _emplID = value; }
        }

        public DateTime LockedDate
        {
            get { return _date; }
            set { _date = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
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

        public EmployeeLockedDayTO()
        { }
    }
}
