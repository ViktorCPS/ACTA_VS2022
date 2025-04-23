using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SyncAnnualLeaveRecalcTO
    {
        private int _recID = -1;
        private int _employeeID = -1;
        private int _numOfDays = -1;
        private DateTime _year = new DateTime();
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private DateTime _createdTimeHist = new DateTime();
        private int _result = -1;
        private string _remark = "";

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

        public int NumOfDays
        {
            get { return _numOfDays; }
            set { _numOfDays = value; }
        }

        public DateTime Year
        {
            get { return _year; }
            set { _year = value; }
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

        public int Result
        {
            get { return _result; }
            set { _result = value; }
        }

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        public DateTime CreatedTimeHist
        {
            get { return _createdTimeHist; }
            set { _createdTimeHist = value; }
        }
    }
}
