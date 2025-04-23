using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class ApplUserXOrgUnitTO
    {
        private string _userID = "";
        private int _orgUnitID = -1;
        private string _purpose = "";

        public string Purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }

        public int OrgUnitID
        {
            get { return _orgUnitID; }
            set { _orgUnitID = value; }
        }

        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
    }
}
