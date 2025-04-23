using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class ApplUserXApplUserCategoryTO
    {
        private int _categoryID = -1;
        private string _userID = "";
        private int _defaultCategory = -1;
        private string _created_by = "";
        private string _modifiedBy = "";
        
        public int CategoryID
        {
            get { return _categoryID; }
            set { _categoryID = value; }
        }

        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        public int DefaultCategory
        {
            get { return _defaultCategory; }
            set { _defaultCategory = value; }
        }

        public string CreatedBy
        {
            get { return _created_by; }
            set { _created_by = value; }
        }

        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public ApplUserXApplUserCategoryTO()
        { }
    }
}
