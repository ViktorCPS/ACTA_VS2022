using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class HolidaysExtendedTO
    {
        private string _description = "";
        private string _type = "";
        private string _category = "";
        private DateTime _year = new DateTime();
        private DateTime _dateStart = new DateTime();
        private DateTime _dateEnd = new DateTime();
        private int _recID = -1;
        private int _sundayTransferable = -1;

        public int SundayTransferable
        {
            get { return _sundayTransferable; }
            set { _sundayTransferable = value; }
        }

        public int RecID
        {
            get { return _recID; }
            set { _recID = value; }
        }

        public DateTime DateEnd
        {
            get { return _dateEnd; }
            set { _dateEnd = value; }
        }

        public DateTime DateStart
        {
            get { return _dateStart; }
            set { _dateStart = value; }
        }

        public DateTime Year
        {
            get { return _year; }
            set { _year = value; }
        }

        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
}
