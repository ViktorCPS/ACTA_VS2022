using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeeXVaccineTO
    {
        private uint _recID = 0;
        private int _vaccineID = -1;
        private int _employeeID = -1;
        private DateTime _datePerformed = new DateTime();
        private int _rotation = -1;
        private int _rotationFlagUsed = -1;
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();

        public uint RecID
        {
            get { return _recID; }
            set { _recID = value; }
        }

        public int VaccineID
        {
            get { return _vaccineID; }
            set { _vaccineID = value; }
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

        public int Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public int RotationFlagUsed
        {
            get { return _rotationFlagUsed; }
            set { _rotationFlagUsed = value; }
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

        public EmployeeXVaccineTO()
        { }
    }
}
