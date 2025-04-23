using System;
using System.Runtime.Serialization;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for WorkingUnitTO.
	/// </summary>
    [DataContract]
	[XmlRootAttribute()]
	public class WorkingUnitTO
	{
		private int _workingUnitId = -1;
		private int _parentWorkingUnitID = -1;
		private string _description = "";
		private string _name = "";
		private string _status = "";
		private int _addressID = -1;
        private int _emplNumber = -1; //number of employees 
        private int _childWUNumber = -1; //number of child working units
        private string _code = "";

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public int ChildWUNumber
        {
            get { return _childWUNumber; }
            set { _childWUNumber = value; }
        }

        public int EmplNumber
        {
            get { return _emplNumber; }
            set { _emplNumber = value; }
        }

        [DataMember]
		[XmlAttributeAttribute(AttributeName="WorkingUnitID")]
		public int WorkingUnitID
		{
			get { return _workingUnitId; }
			set { _workingUnitId = value; }
		}

        [DataMember]
		public int ParentWorkingUID
		{
			get { return _parentWorkingUnitID; }
			set { _parentWorkingUnitID = value; }
		}

        [DataMember]
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

        [DataMember]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

        [DataMember]
		public string Status
		{
			get { return _status; }
			set { _status = value; }
		}

        [DataMember]
		public int AddressID
		{
			get { return _addressID; }
			set { _addressID = value; }
		}

        public WorkingUnitTO()
        { }

		public WorkingUnitTO(int working_unit_id, int parentWUID, string description, string name, string status, int addressID)
		{
			this.WorkingUnitID = working_unit_id;
			this.ParentWorkingUID = parentWUID;
			this.Description = description;
			this.Name = name;
			this.Status = status;
			this.AddressID = addressID;
            this.EmplNumber = -1;
            this.ChildWUNumber = -1;
		}
	}
}
