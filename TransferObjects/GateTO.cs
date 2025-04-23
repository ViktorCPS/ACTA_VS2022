using System;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// GateTO (Transfer Object) used to send 
	/// and receive data from and to DAO
	/// </summary>
	[XmlRootAttribute()]
	public class GateTO
	{
		private int _gateID = -1;
		private string _name = "";
		private string _description = "";
		private DateTime _downloadStartTime = new DateTime();
		private int _downloadInterval = -1;
		private int _downloadEraseCounter = -1;
		private int _gateTimeaccessProfileID = -1; // 17/05/2007, Bilja, this property is not in the constructors, use GET/SET methods
        private string _gateType = ""; //  30.01.2020. BOJAN

		public int GateID
		{
			get{ return _gateID; }
			set{ _gateID = value; }
		}

		public string Name
		{
			get { return _name; }
			set {_name = value; }
		}

        //  30.01.2020. BOJAN
        public string GateType
        {
            get { return _gateType; }
            set { _gateType = value; }
        }

		public string Description
		{
			get { return _description; }
			set {_description = value; }
		}

		public DateTime DownloadStartTime
		{
			get { return _downloadStartTime; }
			set {_downloadStartTime = value; }
		}

		public int DownloadInterval
		{
			get { return _downloadInterval; }
			set {_downloadInterval = value; }
		}

		public int DownloadEraseCounter
		{
			get { return _downloadEraseCounter; }
			set {_downloadEraseCounter = value; }
		}

		public int GateTimeaccessProfileID
		{
			get{ return _gateTimeaccessProfileID; }
			set{ _gateTimeaccessProfileID = value; }
		}

		public GateTO()
		{
		}

		public GateTO(int gateID, string name, string description, DateTime downloadStartTime,
			int downloadInterval, int downloadEraseCounter)
		{
			this.GateID = gateID;
			this.Name = name;
			this.Description = description;
			this.DownloadStartTime = downloadStartTime;
			this.DownloadInterval = downloadInterval;
			this.DownloadEraseCounter = downloadEraseCounter;
		}
	}
}
