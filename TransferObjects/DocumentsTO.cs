using System;
using System.Runtime.Serialization;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// This is Transfer Object used by the DocumentsDAO to send and receive 
	/// data from the Documents class.
	/// </summary>

    // [DataContract] & [DataMemeber] attributes from System.Runtime.Serialization put here for WCF service in ACTAWeb solution purposes only
    [DataContract]
	[XmlRootAttribute()]
	public class DocumentsTO
	{
		private int _DocumentsID = -1;
		private string _firstName = "";
		private string _lastName = "";
		private string _docName = "";
		private string _docDesc = "";
		private byte[] _picture = null;
        private string _createdBy = "";
        private DateTime _createdTime = new DateTime();
        private string _extension = "";

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

        [DataMember]
		[XmlAttributeAttribute(AttributeName="DocumentsID")]
		public int DocumentsID
		{
			get{ return _DocumentsID; }
			set{ _DocumentsID = value; }
		}

        [DataMember]
		public string FirstName
		{
			get { return _firstName; }
			set {_firstName = value; }
		}

        [DataMember]
		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; }
		}

        [DataMember]
        public string FirstAndLastName
        {
            get { return _lastName + " " +_firstName; }
        }

        public string IDLastNameFirstName
        {
            get { return ((_DocumentsID != -1 ? _DocumentsID.ToString().Trim() + " - " : "") + _lastName  + " " + _firstName).Trim(); }
        }

        public string FirstNameLastNameID
        {
            get { return (_firstName + " " + _lastName + (_DocumentsID != -1 ? " - " + _DocumentsID.ToString().Trim() : "")).Trim(); }
        }

        public string LastNameFirstNameID
        {
            get { return (_lastName + " " + _firstName + (_DocumentsID != -1 ? " - " + _DocumentsID.ToString().Trim() : "")).Trim(); }
        }

        [DataMember]
		public string DocName
		{
			get { return _docName; }
			set {_docName = value; }
		}

        [DataMember]
		public string DocDesc
		{
			get { return _docDesc; }
			set {_docDesc = value; }
		}

        
        [DataMember]
		public byte[] Picture
		{
			get { return _picture; }
			set {_picture = value; }
		}

        [DataMember]
        public string Extension
        {
            get { return _extension; }
            set { _extension = value; }
        }

        public DocumentsTO()
        { }

		public DocumentsTO(int DocumentsID, string firstName, string lastName,  
			string docName, string docDesc, byte[] picture, string extension)
		{
			this.DocumentsID = DocumentsID;
			this.FirstName = firstName;
			this.LastName = lastName;
			this.DocName = docName;
			this.DocDesc = docDesc;
			this.Picture = picture;
            this.Extension = extension;
		}


        public DocumentsTO(DocumentsTO docTO)
        {

            this.CreatedBy = docTO.CreatedBy;
            this.CreatedTime = docTO.CreatedTime;
            this.DocumentsID = docTO.DocumentsID;
            this.FirstName = docTO.FirstName;
            this.LastName = docTO.LastName;
            this.DocName = docTO.DocName;
            this.DocDesc = docTO.DocDesc;
            this.Picture = docTO.Picture;
            this.Extension = docTO.Extension;
            
        }
	}
}
