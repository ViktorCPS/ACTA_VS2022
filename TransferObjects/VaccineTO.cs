using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class VaccineTO
    {
        private int _vaccineID = -1;        
        private string _vaccineType = "";
        private string _descSR = "";
        private string _descEN = "";        
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();

        public int VaccineID
        {
            get { return _vaccineID; }
            set { _vaccineID = value; }
        }

        public string VaccineType
        {
            get { return _vaccineType; }
            set { _vaccineType = value; }
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

        public string VaccineTypeDescSR
        {
            get { return "(" + _vaccineType + ") " + _descSR; }
        }

        public string VaccineTypeDescEN
        {
            get { return "(" + _vaccineType + ") " + _descEN; }
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

        public VaccineTO()
        { }
    }
}
