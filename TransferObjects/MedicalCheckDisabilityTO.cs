using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class MedicalCheckDisabilityTO
    {
        private int _workingUnitID = -1;
        private int _disabilityID = -1;
        private string _disabilityCode = "";
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

        public int DisabilityID
        {
            get { return _disabilityID; }
            set { _disabilityID = value; }
        }

        public string DisabilityCode
        {
            get { return _disabilityCode; }
            set { _disabilityCode = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
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

        public string DisabilityCodeDescSR
        {
            get { return "(" + _disabilityCode + ") " + _descSR; }
        }

        public string DisabilityCodeDescEN
        {
            get { return "(" + _disabilityCode + ") " + _descEN; }
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

        public MedicalCheckDisabilityTO()
        { }
    }
}
