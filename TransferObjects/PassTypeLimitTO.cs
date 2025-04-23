using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class PassTypeLimitTO
    {
        private int _ptLimitID = -1;        
        private string _type = "";
        private int _value = -1;
        private string _measureUnit = "";
        private int _period = -1;
        private DateTime _startDate = new DateTime();
        private string _name = "";

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

	
        public int PtLimitID
        {
            get { return _ptLimitID; }
            set { _ptLimitID = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
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

        public int Period
        {
            get { return _period; }
            set { _period = value; }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public PassTypeLimitTO()
        { }
    }
}
