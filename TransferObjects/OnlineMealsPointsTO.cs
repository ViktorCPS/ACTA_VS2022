using System;
using System.Collections.Generic;
using System.Text;
using Util;

namespace TransferObjects
{
  public  class OnlineMealsPointsTO
    {
        int _pointID;
        int _restaurantID;
        string _name;
        string _description;
        string _readerIPAddress;
        int _reader_ant;
        int _reader_peripherial;
        string _readerPeripherialDesc;
        string _createdBy;
        DateTime _createdTime;
        string _modifiedBy;
        DateTime _modifiedTime;
        int meal_type_id;

        string locations = "";

        public string Locations
        {
            get { return locations; }
            set { locations = value; }
        }

        string restaurantName;

        public string RestaurantName
        {
            get { return restaurantName; }
            set { restaurantName = value; }
        }
        string mealType;

        public string MealType
        {
            get { return mealType; }
            set { mealType = value; }
        }

        public int MealTypeID
        {
            get { return meal_type_id; }
            set { meal_type_id = value; }
        }

        DebugLog log;
     

        public int PointID
        {
            get { return _pointID; }
            set { _pointID = value; }
        }
        public int RestaurantID
        {
            get { return _restaurantID; }
            set { _restaurantID = value; }
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }


        public string ReaderIPAddress
        {
            get { return _readerIPAddress; }
            set { _readerIPAddress = value; }
        }


        public int Reader_ant
        {
            get { return _reader_ant; }
            set { _reader_ant = value; }
        }


        public int Reader_peripherial
        {
            get { return _reader_peripherial; }
            set { _reader_peripherial = value; }
        }


        public string ReaderPeripherialDesc
        {
            get { return _readerPeripherialDesc; }
            set { _readerPeripherialDesc = value; }
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
        public OnlineMealsPointsTO() {

            ModifiedBy = "";
            ModifiedTime = new DateTime();
            CreatedBy = "";
            CreatedTime = new DateTime();
            ReaderPeripherialDesc = "";
            Name = "";
            Description = "";
            Reader_peripherial = -1;
            Reader_ant = -1;
            ReaderIPAddress = "";
            PointID = -1;
            RestaurantID = -1;
            MealTypeID = -1;
            RestaurantName = "";
            MealType = "";
        }

    }
}
