using System;

using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for EmployeeGroupAccessControlTO.
	/// </summary>
	[XmlRootAttribute()]
	public class EmployeeGroupAccessControlTO
	{
		private int     _accessGroupId = -1;
		private string  _name = "";
		private string  _description = "";

		public int AccessGroupId
		{
			get { return _accessGroupId; }
			set { _accessGroupId = value; }
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

		public EmployeeGroupAccessControlTO()
		{
		}

		public EmployeeGroupAccessControlTO(int accessGroupId, string name, string description)
		{
			this.AccessGroupId = accessGroupId;
			this.Name          = name;
			this.Description   = description;
		}
	}
}
