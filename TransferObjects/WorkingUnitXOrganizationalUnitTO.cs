using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
   public class WorkingUnitXOrganizationalUnitTO
    {
        private int orgUnitID = -1;
        private int workingUnitID = -1;
        private string purpose = "";

        public string Purpose
        {
            get { return purpose; }
            set { purpose = value; }
        }

        public int WorkingUnitID
        {
            get { return workingUnitID; }
            set { workingUnitID = value; }
        }

        public int OrgUnitID
        {
            get { return orgUnitID; }
            set { orgUnitID = value; }
        } 
    }
}
