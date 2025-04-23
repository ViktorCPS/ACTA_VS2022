using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeeXMedicalCheckDisabilityTO
    {
        private uint _recID = 0;
        private int _employeeID = -1;
        private int _disabilityID = -1;
        private string _type = "";
        private DateTime _dateStart = new DateTime();
        private DateTime _dateEnd = new DateTime();
        private string _note = "";
        private int _flagEmail = -1;
        private DateTime _flagEmailCratedTime = new DateTime();
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

        public int DisabilityID
        {
            get { return _disabilityID; }
            set { _disabilityID = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public DateTime DateStart
        {
            get { return _dateStart; }
            set { _dateStart = value; }
        }

        public DateTime DateEnd
        {
            get { return _dateEnd; }
            set { _dateEnd = value; }
        }

        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }

        public int FlagEmail
        {
            get { return _flagEmail; }
            set { _flagEmail = value; }
        }

        public DateTime FlagEmailCratedTime
        {
            get { return _flagEmailCratedTime; }
            set { _flagEmailCratedTime = value; }
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

        public EmployeeXMedicalCheckDisabilityTO()
        { }
    }
}
