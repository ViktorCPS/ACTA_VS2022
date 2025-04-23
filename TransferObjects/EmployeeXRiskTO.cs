using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeeXRiskTO
    {
        private uint _recID = 0;
        private int _employeeID = -1;
        private int _riskID = -1;
        private DateTime _dateStart = new DateTime();        
        private DateTime _dateEnd = new DateTime();        
        private int _rotation = -1;
        private DateTime _lastDatePerformed = new DateTime();
        private uint _lastVisitRecID = 0;
        private DateTime _lastScheduleDate = new DateTime();
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

        public int RiskID
        {
            get { return _riskID; }
            set { _riskID = value; }
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

        public int Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public DateTime LastDatePerformed
        {
            get { return _lastDatePerformed; }
            set { _lastDatePerformed = value; }
        }

        public uint LastVisitRecID
        {
            get { return _lastVisitRecID; }
            set { _lastVisitRecID = value; }
        }

        public DateTime LastScheduleDate
        {
            get { return _lastScheduleDate; }
            set { _lastScheduleDate = value; }
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

        public EmployeeXRiskTO()
        { }
    }
}
