using System;

using System.Xml;
using System.Xml.Serialization;

using Util;

namespace TransferObjects
{
    /// <summary>
    /// Summary description for CameraSnapshotFileTO.
    /// </summary>
    [XmlRootAttribute()]
    public class CameraSnapshotFileTO
    {
        private int _recordID = -1;
        private int _cameraID = -1;
        private DateTime _cameraCreatedTime = new DateTime(0);
        private DateTime _fileCreatedTime = new DateTime(0);
        private byte[] _content = null;
        private string _fileName = "";
        private uint _tagID = 0;
        private DateTime _eventTime = new DateTime();

        public DateTime EventTime
        {
            get { return _eventTime; }
            set { _eventTime = value; }
        }

        public uint TagID
        {
            get { return _tagID; }
            set { _tagID = value; }
        }

        public int RecordID
        {
            get { return _recordID; }
            set { _recordID = value; }
        }

        public int CameraID
        {
            get { return _cameraID; }
            set { _cameraID = value; }
        }

        public DateTime CameraCreatedTime
        {
            get { return _cameraCreatedTime; }
            set { _cameraCreatedTime = value; }
        }

        public DateTime FileCreatedTime
        {
            get { return _fileCreatedTime; }
            set { _fileCreatedTime = value; }
        }

        public byte[] Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public CameraSnapshotFileTO()
		{
		}

        public CameraSnapshotFileTO(int recordID, int cameraID,
            DateTime cameraCreatedTime, DateTime fileCreatedTime, byte[] content, string fileName)
		{
            this.RecordID = recordID;
            this.CameraID = cameraID;
            this.CameraCreatedTime = cameraCreatedTime;
            this.FileCreatedTime = fileCreatedTime;
            this.Content = content;
            this.FileName = fileName;
		}
    }
}
