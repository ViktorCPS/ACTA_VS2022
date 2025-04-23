using System;
using System.Xml;
using System.Xml.Serialization;

namespace TransferObjects
{
	/// <summary>
	/// ExitPermissionTO (Transfer Object) used to send 
	/// and receive data from and to DAO
	/// </summary>
	[XmlRootAttribute()]
	public class ExitPermissionTO
	{
		private int _permissionID = -1;
		private int _employeeID = -1;
		private int _passTypeID = -1;
		private DateTime _startTime = new DateTime();
		private int _offset = -1;
		private int _used = -1;
		private string _description = "";
		private string _issuedBy = "";
		private DateTime _issuedTime = new DateTime();
		private string _employeeName = "";
		private string _passTypeDesc = "";
		private string _userName = "";
		private string _verifiedBy = "";
		private string _workingUnitName = "";

		public int PermissionID
		{
			get{ return _permissionID; }
			set{ _permissionID = value; }
		}

		public int EmployeeID
		{
			get { return _employeeID; }
			set {_employeeID = value; }
		}

		public int PassTypeID
		{
			get { return _passTypeID; }
			set {_passTypeID = value; }
		}

		public DateTime StartTime
		{
			get { return _startTime; }
			set {_startTime = value; }
		}

		public int Offset
		{
			get { return _offset; }
			set {_offset = value; }
		}

		public int Used
		{
			get { return _used; }
			set {_used = value; }
		}

		public string Description
		{
			get { return _description; }
			set {_description = value; }
		}

		public string IssuedBy
		{
			get { return _issuedBy; }
			set {_issuedBy = value; }
		}

		public DateTime IssuedTime
		{
			get { return _issuedTime; }
			set {_issuedTime = value; }
		}

		public string EmployeeName
		{
			get { return _employeeName; }
			set {_employeeName = value; }
		}

		public string PassTypeDesc
		{
			get { return _passTypeDesc; }
			set {_passTypeDesc = value; }
		}

		public string UserName
		{
			get { return _userName; }
			set {_userName = value; }
		}

		public string VerifiedBy
		{
			get { return _verifiedBy; }
			set {_verifiedBy = value; }
		}

		public string WorkingUnitName
		{
			get{ return _workingUnitName; }
			set{ _workingUnitName = value; }
		}

        public ExitPermissionTO()
        { }

		public ExitPermissionTO(int permissionID, int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, string issuedBy, DateTime issuedTime)
		{
			this.PermissionID = permissionID;
			this.EmployeeID = employeeID;
			this.PassTypeID = passTypeID;
			this.StartTime = startTime;
			this.Offset = offset;
			this.Used = used;
			this.Description = description;
			this.IssuedBy = issuedBy;
			this.IssuedTime = issuedTime;
		}
	}
}
