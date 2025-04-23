using System;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for ApplUsersXRoleTO.
	/// </summary>
	[XmlRootAttribute()]
	public class ApplUsersXRoleTO
	{
		private string _userID = "";
		private int _roleID = -1;

		public string UserID
		{
			get { return _userID; }
			set {_userID = value; }
		}

		public int RoleID
		{
			get { return _roleID; }
			set {_roleID = value; }
		}

		public ApplUsersXRoleTO()
		{
		}

		public ApplUsersXRoleTO(string userID, int roleID)
		{
			this.UserID = userID;
			this.RoleID = roleID;
		}	
	}
}
