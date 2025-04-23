using System;

using System.Xml;
using System.Xml.Serialization;

using Util;

namespace TransferObjects
{
    /// <summary>
    /// Summary description for CamerasXReadersTO.
    /// </summary>
    [XmlRootAttribute()]
    public class CamerasXReadersTO
    {
        private int _cameraID = -1;
        private int _readerID = -1;
        private string _directionCovered = "";

        public int CameraID
        {
            get { return _cameraID; }
            set { _cameraID = value; }
        }

        public int ReaderID
        {
            get { return _readerID; }
            set { _readerID = value; }
        }

        public string DirectionCovered
        {
            get { return _directionCovered; }
            set { _directionCovered = value; }
        }

        public CamerasXReadersTO()
		{
		}

        public CamerasXReadersTO(int cameraID, int readerID, string directionCovered)
		{
            this.CameraID= cameraID;
            this.ReaderID = readerID;
            this.DirectionCovered = directionCovered;
		}
    }
}
