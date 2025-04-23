using System;

using System.Xml;
using System.Xml.Serialization;

using Util;

namespace TransferObjects
{
    /// <summary>
    /// Summary description for GateSyncTO.
    /// </summary>
    public class GateSyncTO
    {
        private int _gateID = -1;
        private string _readerControlRequest = "";
        private string _monitoring = "";

        public int GateID
		{
            get { return _gateID; }
            set { _gateID = value; }
		}

        public string ReaderControlRequest
        {
            get { return _readerControlRequest; }
            set { _readerControlRequest = value; }
        }

        public string Monitoring
        {
            get { return _monitoring; }
            set { _monitoring = value; }
        }

        public GateSyncTO()
		{
		}
    }
}
