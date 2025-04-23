using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SecurityRoutesPointTO
    {
        private int _controlPointID;
        private string _tagID;
        private string _tagName;
        private string _name;
        private string _description;

        public int ControlPointID
        {
            get { return _controlPointID; }
            set { _controlPointID = value; }
        }

        public string TagID
        {
            get { return _tagID; }
            set { _tagID = value; }
        }

        public string TagName
        {
            get { return _tagName; }
            set { _tagName = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public SecurityRoutesPointTO()
        {
            // Init properties
            ControlPointID = -1;
            TagID = "";
            TagName = "";
            Name = "";
            Description = "";
        }
    }
}
