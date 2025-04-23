using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
  public  class MealTypeEmplTO
    {
        int _mealTypeEmplID;        
        string _name;
        string _description;

      public int MealTypeEmplID
      {
          get { return _mealTypeEmplID; }
          set { _mealTypeEmplID = value; }
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

        public MealTypeEmplTO()
        {
            this.MealTypeEmplID = -1;
            this.Name = "";
            this.Description = "";
        }
        public MealTypeEmplTO(int mealTypeID, string name, string description)
        {
            this.MealTypeEmplID = mealTypeID;
            this.Name = name;
            this.Description = description;
        }

    }
}
