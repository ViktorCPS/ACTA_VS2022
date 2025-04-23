using System;
using System.Runtime.Serialization;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// LocationTO (Transfer Object) used to send 
	/// and receive data from and to  DAO
	/// </summary>
	[XmlRootAttribute()]
    [DataContract]
	public class LocationTO
	{
		private int _locationId = -1;
		private string _name = "";
		private string _description = "";
		private int _parentLocationId = -1;
		private int _addressID = -1;
		private string _status = "";
        private string _segmentColor = "";
		
		[XmlAttributeAttribute(AttributeName="LocationID")]
        [DataMember]
		public int LocationID
		{
			get{ return _locationId; }
			set{ _locationId = value; }
		}

        [DataMember]
		public string Name
		{
			get { return _name; }
			set {_name = value; }
		}

        [DataMember]
		public string Description
		{
			get { return _description; }
			set {_description = value; }
		}

        [DataMember]
		public int ParentLocationID
		{
			get { return _parentLocationId; }
			set { _parentLocationId = value; }
		}

        [DataMember]
		public int AddressID
		{
			get { return _addressID; }
			set { _addressID = value; }
		}

        [DataMember]
		public string Status
		{
			get { return _status; }
			set { _status = value; }
		}

        public string SegmentColor
        {
            get { return _segmentColor; }
            set { _segmentColor = value; }
        }

		public LocationTO()
		{
		}

		public LocationTO(int locationId, string name, string description, int parentLocationId, int addressID, string status)
		{
			this.LocationID = locationId;
			this.Name = name;
			this.Description = description;
			this.ParentLocationID = parentLocationId;
			this.AddressID = addressID;
			this.Status = status;
		}

        public LocationTO(LocationTO locTO)
        {
            this.LocationID = locTO.LocationID;
            this.Name = locTO.Name;
            this.Description = locTO.Description;
            this.ParentLocationID = locTO.ParentLocationID;
            this.AddressID = locTO.AddressID;
            this.Status = locTO.Status;
            this.SegmentColor = locTO.SegmentColor;
        }
	}
}
