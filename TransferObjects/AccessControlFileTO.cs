using System;

using System.Xml;
using System.Xml.Serialization;

using Util;

namespace TransferObjects
{
    /// <summary>
    /// Summary description for AccessControlFileTO.
    /// </summary>
    [XmlRootAttribute()]
    public class AccessControlFileTO
    {
        private int _recordID = -1;
        private string _type = "";		
        private int _readerID = -1;
        private int _delayed = -1;
        private string _status = "";
        private DateTime _uploadStartTime = new DateTime(0);
        private DateTime _uploadEndTime = new DateTime(0);
        private string _errorContent = "";
        private byte[] _content = null;
        private DateTime _createdTime = new DateTime(0);
        private string _createdBy = "";

        public int RecordID
		{
            get { return _recordID; }
            set { _recordID = value; }
		}

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public int ReaderID
        {
            get { return _readerID; }
            set { _readerID = value; }
        }

        public int Delayed
        {
            get { return _delayed; }
            set { _delayed = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public DateTime UploadStartTime
		{
            get { return _uploadStartTime; }
            set { _uploadStartTime = value; }
		}

        public DateTime UploadEndTime
        {
            get { return _uploadEndTime; }
            set { _uploadEndTime = value; }
        }

        public string ErrorContent
        {
            get { return _errorContent; }
            set { _errorContent = value; }
        }

        public byte[] Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

		public AccessControlFileTO()
		{
		}

        public AccessControlFileTO(int recordID, string type, int readerID, int delayed,
            string status, DateTime uploadStartTime, DateTime uploadEndTime,
            string errorContent, byte[] content, DateTime createdTime, string createdBy)
		{
            this.RecordID = recordID;
            this.Type = type;
            this.ReaderID = readerID;
            this.Delayed = delayed;
            this.Status = status;
            this.UploadStartTime = uploadStartTime;
            this.UploadEndTime = uploadEndTime;
            this.ErrorContent = errorContent;
            this.Content = content;
            this.CreatedTime = createdTime;
            this.CreatedBy = createdBy;
		}
    }
}
