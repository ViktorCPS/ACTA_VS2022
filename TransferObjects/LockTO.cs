using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
   public class LockTO
    {
        private int _lockID;
        private DateTime _lockDate;
        private string _type;
        private string _comment;
        private string _createdBy;
        private DateTime _createdTime;

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }
       
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public DateTime LockDate
        {
            get { return _lockDate; }
            set { _lockDate = value; }
        }

        public int LockID
        {
            get { return _lockID; }
            set { _lockID = value; }
        }
       public LockTO()
       {
           LockID = -1;
           LockDate = new DateTime();
           Comment = "";
           Type = "";
           CreatedBy = "";
           CreatedTime = new DateTime();
       }
    }
}
