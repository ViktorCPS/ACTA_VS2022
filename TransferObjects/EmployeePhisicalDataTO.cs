using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeePhisicalDataTO
    {
        private uint _recID = 0;        
        private int _employeeID = -1;
        private DateTime _datePerformed = new DateTime();
        private int _wight = -1;
        private int _height = -1;
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
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public DateTime DatePerformed
        {
            get { return _datePerformed; }
            set { _datePerformed = value; }
        }

        public int Weight
        {
            get { return _wight; }
            set { _wight = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
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

        public EmployeePhisicalDataTO()
        { }
    }
}
