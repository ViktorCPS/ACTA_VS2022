using System;
using System.Xml;
using System.Xml.Serialization;

using Util;

namespace TransferObjects
{
    [XmlRootAttribute()]
    public class VisitorDocFileTO
    {
        private int _visitID = -1;
        private int _docType = -1;
        private byte[] _content = null;
        private DateTime _modifiedTime = new DateTime(0);
        private string _contentName = "";
        private DateTime _createdTime = new DateTime(0);

        public int VisitID
        {
            get { return _visitID; }
            set { _visitID = value; }
        }

        public int DocType
        {
            get { return _docType; }
            set { _docType = value; }
        }

        public byte[] Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public DateTime ModifiedTime
        {
            get { return _modifiedTime; }
            set { _modifiedTime = value; }
        }

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        public string ContentName
        {
            get { return _contentName; }
            set { _contentName = value; }
        }

        public VisitorDocFileTO()
        {
        }

        public VisitorDocFileTO(int visitID, int docType, byte[] content, DateTime modifiedTime, string contentName, DateTime createdTime)
        {
            this.VisitID = visitID;
            this.DocType = docType;
            this.Content = content;
            this.ModifiedTime = modifiedTime;
            this.ContentName = contentName;
            this.CreatedTime = CreatedTime;
        }
    }
}
