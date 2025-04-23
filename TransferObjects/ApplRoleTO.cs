using System;
using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for ApplRoleTO.
	/// </summary>
	[XmlRootAttribute()]
	public class ApplRoleTO
	{
		private int _applRoleID = -1;
		private string _name = "";
		private string _description = "";

		public int ApplRoleID
		{
			get{ return _applRoleID; }
			set{ _applRoleID = value; }
		}

		public string Name
		{
			get { return _name; }
			set {_name = value; }
		}

		public string Description
		{
			get { return _description; }
			set {_description = value; }
		}

		public ApplRoleTO()
		{
		}

		public ApplRoleTO(int applRoleID, string name, string description)
		{
			this.ApplRoleID = applRoleID;
			this.Name = name;
			this.Description = description;
		}
	}
}
