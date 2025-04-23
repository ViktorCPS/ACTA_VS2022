using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class OrganizationalUnitTO
    {
        private int _orgUnitID = -1;
        private int _parentOrgUnitID = -1;
        private string _name = "";
        private string _desc = "";
        private int _addressID = -1;
        private string _status = "";
        private string _code = "";

        public int OrgUnitID
        {
            get { return _orgUnitID; }
            set { _orgUnitID = value; }
        }

        public int ParentOrgUnitID
        {
            get { return _parentOrgUnitID; }
            set { _parentOrgUnitID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        public string Desc
        {
            get { return _desc; }
            set { _desc = value; }
        }

        public int AddressID
        {
            get { return _addressID; }
            set { _addressID = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public OrganizationalUnitTO()
        { }

        public OrganizationalUnitTO(int org_unit_id, int parentOUID, string description, string name, string status)
        {
            this.OrgUnitID = org_unit_id;
            this.ParentOrgUnitID = parentOUID;
            this.Desc = description;
            this.Name = name;
            this.Status = status;
        }
    }
}
