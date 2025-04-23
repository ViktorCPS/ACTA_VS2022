using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeePositionTO
    {
        private int _workingUnitID = -1;
        private int _positionID = -1;
        private string _positionCode = "";
        private string _positionTitleSR = "";
        private string _positionTitleEN = "";
        private string _descSR = "";
        private string _descEN = "";
        private string _status = "";
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();

        public int WorkingUnitID
        {
            get { return _workingUnitID; }
            set { _workingUnitID = value; }
        }

        public int PositionID
        {
            get { return _positionID; }
            set { _positionID = value; }
        }

        public string PositionCode
        {
            get { return _positionCode; }
            set { _positionCode = value; }
        }

        public string PositionTitleSR
        {
            get { return _positionTitleSR; }
            set { _positionTitleSR = value; }
        }

        public string PositionTitleEN
        {
            get { return _positionTitleEN; }
            set { _positionTitleEN = value; }
        }

        public string PositionCodeTitleSR
        {
            get { return "(" + _positionCode + ") " + _positionTitleSR; }
            set { _positionCode = value; }
        }

        public string PositionCodeTitleEN
        {
            get { return "(" + _positionCode + ") " + _positionTitleEN; }
            set { _positionCode = value; }
        }

        public string DescSR
        {
            get { return _descSR; }
            set { _descSR = value; }
        }

        public string DescEN
        {
            get { return _descEN; }
            set { _descEN = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
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

        public EmployeePositionTO()
        { }
    }
}
