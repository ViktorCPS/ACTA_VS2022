using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class MedicalCheckPointTO
    {
        private int _pointID = -1;
        private string _desc = "";        
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _modifiedBy = "";
        private DateTime _modifiedTime = new DateTime();
        
        public int PointID
        {
            get { return _pointID; }
            set { _pointID = value; }
        }

        public string Desc
        {
            get { return _desc; }
            set { _desc = value; }
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

        public MedicalCheckPointTO()
        { }
    }
}
