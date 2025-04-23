using System;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for HolidayTO.
	/// </summary>
	[XmlRootAttribute()]
	public class HolidayTO
	{
		private string _description = "";
		private DateTime _holidayDate = new DateTime();

		public string Description
		{
			get { return _description; }
			set {_description = value; }
		}

		public DateTime HolidayDate
		{
			get { return _holidayDate; }
			set {_holidayDate = value; }
		}

		public HolidayTO()
		{
		}

		public HolidayTO(string description, DateTime holidayDate)
		{
			this.Description = description;
			this.HolidayDate = holidayDate;
		}
	}
}
