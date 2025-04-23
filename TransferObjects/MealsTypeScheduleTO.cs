using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
   public class MealsTypeScheduleTO
    {
        private int _mealTypeID = -1;
        private DateTime _date = new DateTime();
        private DateTime _hrsFrom = new DateTime();
        private DateTime _hrsTo = new DateTime();
        private string _mealType = "";

        public string MealType
        {
            get { return _mealType; }
            set { _mealType = value; }
        }

        public DateTime HrsTo
        {
            get { return _hrsTo; }
            set { _hrsTo = value; }
        }

        public DateTime HrsFrom
        {
            get { return _hrsFrom; }
            set { _hrsFrom = value; }
        } 

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public int MealTypeID
        {
            get { return _mealTypeID; }
            set { _mealTypeID = value; }
        }

    }
}
