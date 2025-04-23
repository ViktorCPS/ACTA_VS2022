using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects
{
    public class SecurityRouteDtlTO
    {
        private int _securityRouteID;
        private int _segmentID;
        private int _gateID;
        private string _gateName;
        private DateTime _timeFrom;
        private DateTime _timeTo;

        public int SecurityRouteID
        {
            get { return _securityRouteID; }
            set { _securityRouteID = value; }
        }

        public int SegmentID
        {
            get { return _segmentID; }
            set { _segmentID = value; }
        }

        public int GateID
        {
            get { return _gateID; }
            set { _gateID = value; }
        }

        public string GateName
        {
            get { return _gateName; }
            set { _gateName = value; }
        }

        public DateTime TimeFrom
        {
            get { return _timeFrom; }
            set { _timeFrom = value; }
        }

        public DateTime TimeTo
        {
            get { return _timeTo; }
            set { _timeTo = value; }
        }

        public SecurityRouteDtlTO()
		{
			// Init properties
            SecurityRouteID = -1;
            SegmentID = -1;
            GateID = -1;
            GateName = "";
            TimeFrom = new DateTime();
            TimeTo = new DateTime();
		}
    }
}
