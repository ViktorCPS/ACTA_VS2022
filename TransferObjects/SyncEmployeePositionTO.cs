using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SyncEmployeePositionTO
    {
        private int _recID = -1;
        private string _companyCode = "";
        private int _positionID = -1;
        private string _positionCode = "";
        private string _positionTitleSR = "";
        private string _positionTitleEN = "";
        private string _descSR = "";
        private string _descEN = "";
        private string _status = "";
        private string _createdBy = "";
        private DateTime _validFrom = new DateTime();
        private DateTime _createdTime = new DateTime();
        private DateTime _createdTimeHist = new DateTime();
        private int _result = -1;
        private string _remark = "";

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        public DateTime ValidFrom
        {
            get { return _validFrom; }
            set { _validFrom = value; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
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

        public string Status
        {
            get { return _status; }
            set { _status = value; }
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

        public string CompanyCode
        {
            get { return _companyCode; }
            set { _companyCode = value; }
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


