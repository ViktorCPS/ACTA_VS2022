using System;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for TimeAccessProfileTO.
	/// </summary>
	[XmlRootAttribute()]
	public class TimeAccessProfileTO
	{
		private int     _timeAccessProfileId = -1;
		private string  _name = "";
		private string  _description = "";

		public int TimeAccessProfileId
		{
			get { return _timeAccessProfileId; }
			set { _timeAccessProfileId = value; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		public TimeAccessProfileTO()
		{
		}

		public TimeAccessProfileTO(int timeAccessProfileId, string name, string description)
		{
			this.TimeAccessProfileId = timeAccessProfileId;
			this.Name                = name;
			this.Description         = description;
		}
	}
}
