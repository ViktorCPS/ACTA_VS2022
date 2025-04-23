using System;
using System.Collections;
using System.Text;

namespace TransferObjects
{
    public class SecurityRouteHdrTO
    {
        private int _securityRouteID;
        private string _name;
        private string _description;
        private string _routeType;

        public Hashtable Segments;

        public int SecurityRouteID
        {
            get { return _securityRouteID; }
            set { _securityRouteID = value; }
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

        public string RouteType
        {
            get { return _routeType; }
            set { _routeType = value; }
        }

        public SecurityRouteHdrTO()
		{
			// Init properties
            SecurityRouteID = -1;
			Name = "";
			Description = "";
            RouteType = "";
			Segments = new Hashtable();
		}
    }
}
