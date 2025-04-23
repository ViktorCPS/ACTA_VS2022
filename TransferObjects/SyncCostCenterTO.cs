using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SyncCostCenterTO
    {
        private int _recID = -1;
        private string _code = "";
        private string _companyCode = "";
        private string _desc = "";
        private int _result = -1;
        private string _remark = "";
        private DateTime _validFrom = new DateTime();
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private DateTime _createdTimeHist = new DateTime();

        public int RecID
        {
            get { return _recID; }
            set { _recID = value; }
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public string CompanyCode
        {
            get { return _companyCode; }
            set { _companyCode = value; }
        }

        public string Desc
        {
            get { return _desc; }
            set { _desc = value; }
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

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
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
