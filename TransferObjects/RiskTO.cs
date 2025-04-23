using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class RiskTO
    {
        private int _workingUnitID = -1;
        private int _riskID = -1;
        private string _riskCode = "";        
        private string _descSR = "";
        private string _descEN = "";
        private int _defaultRotation = -1;
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();

        public int WorkingUnitID
        {
            get { return _workingUnitID; }
            set { _workingUnitID = value; }
        }

        public int RiskID
        {
            get { return _riskID; }
            set { _riskID = value; }
        }

        public string RiskCode
        {
            get { return _riskCode; }
            set { _riskCode = value; }
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

        public int DefaultRotation
        {
            get { return _defaultRotation; }
            set { _defaultRotation = value; }
        }

        public string RiskCodeDescSR
        {
            get { return "(" + _riskCode + ") " + _descSR; }            
        }

        public string RiskCodeDescEN
        {
            get { return "(" + _riskCode + ") " + _descEN; }
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

        public RiskTO()
        { }
    }
}
