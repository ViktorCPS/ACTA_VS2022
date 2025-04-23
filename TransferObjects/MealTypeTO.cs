using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
   public class MealTypeTO
    {
        int _mealTypeID;
        string _name;
        string _description;
        DateTime _hoursFrom;
        DateTime _hoursTo;

        public DateTime HoursTo
        {
            get { return _hoursTo; }
            set { _hoursTo = value; }
        }

        public DateTime HoursFrom
        {
            get { return _hoursFrom; }
            set { _hoursFrom = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int MealTypeID
        {
            get { return _mealTypeID; }
            set { _mealTypeID = value; }
        }
        public MealTypeTO()
        {
            this.MealTypeID = -1;
            this.Name = "";
            this.Description = "";
            this.HoursFrom = new DateTime();
            this.HoursTo = new DateTime();
        }
        public MealTypeTO(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo)
        {
            this.MealTypeID = mealTypeID;
            this.Name = name;
            this.Description = description;
            this.HoursFrom = hoursFrom;
            this.HoursTo = hoursTo;
        }
    }
}
