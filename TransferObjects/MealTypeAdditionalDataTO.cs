using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class MealTypeAdditionalDataTO
    {

        int _mealTypeID;
        string _description_additional;
        byte[] _picture;

        public int MealTypeID
        {
            get { return _mealTypeID; }
            set { _mealTypeID = value; }
        }

        public string DescriptionAdditional
        {
            get { return _description_additional; }
            set { _description_additional = value; }
        }

        public byte[] Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        public MealTypeAdditionalDataTO()
        {
            this.MealTypeID = -1;
            this.Picture = null;
            this.DescriptionAdditional = "";
        }
        public MealTypeAdditionalDataTO(int mealTypeID, string descriptionAdditional, byte[] picture)
        {
            this.MealTypeID = mealTypeID;
            this.Picture = picture;
            this.DescriptionAdditional = descriptionAdditional;
        }
    }
}
