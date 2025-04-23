using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeeCounterValueHistTO
    {
        private uint _recID = 0;
        private int _emplCounterTypeID = -1;
        private int _emplID = -1;
        private int _value = -1;
        private string _measureUnit = "";
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();

        public uint RecID
        {
            get { return _recID; }
            set { _recID = value; }
        }

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

        public EmployeeCounterValueHistTO()
        { }

        public EmployeeCounterValueHistTO(EmployeeCounterValueTO emplCounterValueTO)
        {
            this.EmplCounterTypeID = emplCounterValueTO.EmplCounterTypeID;
            this.EmplID = emplCounterValueTO.EmplID;
            this.MeasureUnit = emplCounterValueTO.MeasureUnit;
            this.Value = emplCounterValueTO.Value;
            this.CreatedBy = emplCounterValueTO.CreatedBy;
            this.CreatedTime = emplCounterValueTO.CreatedTime;
        }
    }
}
