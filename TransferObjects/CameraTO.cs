using System;

using System.Xml;
using System.Xml.Serialization;

using Util;

namespace TransferObjects
{
    /// <summary>
    /// Summary description for CameraTO.
    /// </summary>
    [XmlRootAttribute()]
    public class CameraTO
    {
        private int _cameraID = -1;
        private string _connAddress = "";
        private string _description = "";
        private string _type = "";

        public int CameraID
        {
            get { return _cameraID; }
            set { _cameraID = value; }
        }

        public string ConnAddress
        {
            get { return _connAddress; }
            set { _connAddress = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public CameraTO()
		{
		}

        public CameraTO(int cameraID, string connAddress, 
            string description, string type)
		{
            this.CameraID= cameraID;
            this.ConnAddress = connAddress;
            this.Description = description;
            this.Type = type;
		}
    }
}
