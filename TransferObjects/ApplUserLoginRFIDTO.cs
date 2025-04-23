using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class ApplUserLoginRFIDTO
    {
        private string _host = "";
        private string _tagID = "";
        private DateTime _createdTime = new DateTime();

        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }

        public string TagID
        {
            get { return _tagID; }
            set { _tagID = value; }
        }

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }
    }
}
