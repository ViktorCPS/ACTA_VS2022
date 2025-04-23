using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeeCounterValueTO
    {
        private int _emplCounterTypeID = -1;
        private int _emplID = -1;
        private int _value = -1;
        private string _measureUnit = "";
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();

        public int EmplCounterTypeID
        {
            get { return _emplCounterTypeID; }
            set { _emplCounterTypeID = value; }
        }

        public int EmplID
        {
            get { return _emplID; }
            set { _emplID = value; }
        }

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string MeasureUnit
        {
            get { return _measureUnit; }
            set { _measureUnit = value; }
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

        public EmployeeCounterValueTO()
        { }

        public EmployeeCounterValueTO(EmployeeCounterValueTO valueTO)
        {
            this.CreatedBy = valueTO.CreatedBy;
            this.CreatedTime = valueTO.CreatedTime;
            this.EmplCounterTypeID = valueTO.EmplCounterTypeID;
            this.EmplID = valueTO.EmplID;
            this.MeasureUnit = valueTO.MeasureUnit;
            this.ModifiedBy = valueTO.ModifiedBy;
            this.ModifiedTime = valueTO.ModifiedTime;
            this.Value = valueTO.Value;
        }
    }
}
