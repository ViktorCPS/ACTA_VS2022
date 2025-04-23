using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class EmployeeCounterTypeTO
    {
        private int _emplCounterTypeID = -1;        
        private string _name = "";
        private string _nameAlt = "";
        private string _desc = "";

        public int EmplCounterTypeID
        {
            get { return _emplCounterTypeID; }
            set { _emplCounterTypeID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string NameAlt
        {
            get { return _nameAlt; }
            set { _nameAlt = value; }
        }

        public string Desc
        {
            get { return _desc; }
            set { _desc = value; }
        }

        public EmployeeCounterTypeTO()
        {
            this.EmplCounterTypeID = -1;
            this.Name = "";
            this.NameAlt = "";
            this.Desc = "";
        }

        public EmployeeCounterTypeTO(int emplCountTypeID, string name, string nameAlt, string desc)
        {
            this.EmplCounterTypeID = emplCountTypeID;
            this.Name = name;
            this.NameAlt = nameAlt;
            this.Desc = desc;
        }
    }
}
