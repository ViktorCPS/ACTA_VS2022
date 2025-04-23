using System;

using System.Xml;
using System.Xml.Serialization;

using Util;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for ExtraHourTO.
	/// </summary>
	[XmlRootAttribute()]
	public class ExtraHourTO
	{
		private int      _employeeID   = -1;
		private DateTime _dateEarned   = new DateTime();
		private int      _extraTimeAmt = -1;
		private string   _calculatedTimeAmt = "";
		private int      _employeeSum = -1;

		public int EmployeeID
		{
			get { return _employeeID; }
			set { _employeeID = value; }
		}

		public DateTime DateEarned
		{
			get { return _dateEarned; }
			set { _dateEarned = value; }
		}

		public int ExtraTimeAmt
		{
			get { return _extraTimeAmt; }
			set { _extraTimeAmt = value; }
		}

		public string CalculatedTimeAmt
		{
			get { return _calculatedTimeAmt; }
			set { _calculatedTimeAmt = value; }
		}

		public int EmployeeSum
		{
			get { return _employeeSum; }
			set { _employeeSum = value; }
		}

		public ExtraHourTO()
		{
		}

		public ExtraHourTO(int employeeID, DateTime dateEarned, int extraTimeAmt)
		{
			this.EmployeeID   = employeeID;
			this.DateEarned   = dateEarned;
			this.ExtraTimeAmt = extraTimeAmt;
			this.CalculatedTimeAmt = Util.Misc.transformMinToStringTime(extraTimeAmt);
		}
	}
}
