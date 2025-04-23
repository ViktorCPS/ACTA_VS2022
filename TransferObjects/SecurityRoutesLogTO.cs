using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SecurityRoutesLogTO
    {
        private int _logID;
        private int _readerID;
        private string _tagID;
        private int _employeeID;
        private int _pointID;
        private DateTime _eventTime;
        private string _employeeName;
        private string _pointName;

        public string PointName
        {
            get { return _pointName; }
            set { _pointName = value; }
        }

        public string EmployeeName
        {
            get { return _employeeName; }
            set { _employeeName = value; }
        }

        public int LogID
        {
            get { return _logID; }
            set { _logID = value; }
        }

        public int ReaderID
        {
            get { return _readerID; }
            set { _readerID = value; }
        }

        public string TagID
        {
            get { return _tagID; }
            set { _tagID = value; }
        }

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        public int PointID
        {
            get { return _pointID; }
            set { _pointID = value; }
        }

        public DateTime EventTime
        {
            get { return _eventTime; }
            set { _eventTime = value; }
        }

        public SecurityRoutesLogTO()
        {
            // Init properties
            LogID = -1;
            ReaderID = -1;
            TagID = "";
            EmployeeID = -1;
            PointID = -1;
            EventTime = new DateTime();
        }
    }
}
