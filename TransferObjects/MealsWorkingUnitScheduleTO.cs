using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
   public class MealsWorkingUnitScheduleTO
    {
        private int _mealTypeID = -1;
        private DateTime _date = new DateTime();
        private int _workingUnitID = -1;
        private string _mealType = "";
        private string _workingUnit = "";

        public string WorkingUnit
        {
            get { return _workingUnit; }
            set { _workingUnit = value; }
        }

        public string MealType
        {
            get { return _mealType; }
            set { _mealType = value; }
        }

        public int WorkingUnitID
        {
            get { return _workingUnitID; }
            set { _workingUnitID = value; }
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
