using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class ApplUserCategoryTO
    {
        private int _categoryID = -1;
        private string _desc = "";
        private string _name = "";
        private string _createdBy = "";
        
        public int CategoryID
        {
            get { return _categoryID; }
            set { _categoryID = value; }
        }

        public string Desc
        {
            get { return _desc; }
            set { _desc = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public ApplUserCategoryTO()
        { }

        public ApplUserCategoryTO(int categoryID, string desc, string name)
        {
            this.CategoryID = categoryID;
            this.Desc = desc;
            this.Name = name;
        }
    }
}
