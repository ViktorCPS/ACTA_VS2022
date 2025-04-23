using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
   public class OnlineMealsRestaurantTO
    {
        int _restaurantID;
        string _name;
        string _description;
        string _created_by;

        DateTime _created_time;
        string _modifiedBy;
        DateTime _modifiedTime;

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
            get { return _description; }
            set { _description = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int RestaurantID
        {
            get { return _restaurantID; }
            set { _restaurantID = value; }
        }

       public OnlineMealsRestaurantTO(){

           RestaurantID = -1;
           Description = "";
           Name = "";
           ModifiedBy = "";
           Created_by = "";
           Created_time = new DateTime();
           ModifiedTime=new DateTime();

   }

    }
}
