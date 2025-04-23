using System;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for ApplUsersXWUTO.
	/// </summary>
	[XmlRootAttribute()]
	public class ApplUsersXWUTO
	{
		private string _userID = "";
		private int _working_unitID = -1;
		private string _purpose = "";

		public string UserID
		{
			get { return _userID; }
			set {_userID = value; }
		}

		public int WorkingUnitID
		{
			get { return _working_unitID; }
			set {_working_unitID = value; }
		}

		public string Purpose
		{
			get { return _purpose; }
			set {_purpose = value; }
		}

		public ApplUsersXWUTO()
		{
		}

		public ApplUsersXWUTO(string userID, int wuID, string purpose)
		{
			this.UserID = userID;
			this.WorkingUnitID = wuID;
			this.Purpose = purpose;
		}	
	}
}
