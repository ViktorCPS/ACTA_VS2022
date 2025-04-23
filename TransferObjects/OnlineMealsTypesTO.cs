using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class OnlineMealsTypesTO
    {
        int mealTypeID = -1;
        string name = "";
        string description = "";
        string _created_by = "";

        DateTime _created_time = new DateTime();
        string _modifiedBy = "";
        DateTime _modifiedTime = new DateTime();

        public DateTime ModifiedTime
        {
            get { return _modifiedTime; }
            set { _modifiedTime = value; }
        }

        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public DateTime Created_time
        {
            get { return _created_time; }
            set { _created_time = value; }
        }

        public string Created_by
        {
            get { return _created_by; }
            set { _created_by = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        public int MealTypeID
        {
            get { return mealTypeID; }
            set { mealTypeID = value; }
        }

        public OnlineMealsTypesTO()
        {

            MealTypeID = -1;
            Description = "";
            Name = "";
            ModifiedBy = "";
            Created_by = "";
            Created_time = new DateTime();
            ModifiedTime = new DateTime();

        }
    }
}
