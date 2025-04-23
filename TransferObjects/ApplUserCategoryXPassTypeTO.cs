using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class ApplUserCategoryXPassTypeTO
    {
        private int _categoryID = -1;
        private int _passTypeID = -1;
        private string _purpose = "";

        public int CategoryID
        {
            get { return _categoryID; }
            set { _categoryID = value; }
        }

        public int PassTypeID
        {
            get { return _passTypeID; }
            set { _passTypeID = value; }
        }

        public string Purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }
    }
}
