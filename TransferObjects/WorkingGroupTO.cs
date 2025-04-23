using System;

namespace TransferObjects
{
	/// <summary>
	/// This is Transfer Object used by the WTGroupDAO to send and receive 
	/// data from the Employee class.
	/// </summary>
	public class WorkingGroupTO
	{
		private int _employeeGroupID = -1;
		private string _groupName = "";
		private string _description = "";

		public int EmployeeGroupID
		{
			get{ return _employeeGroupID; }
			set{ _employeeGroupID = value; }
		}

		public string GroupName
		{
			get { return _groupName; }
			set {_groupName = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}
		
		public WorkingGroupTO()
		{ }

		public WorkingGroupTO(int employeeGroupID, string groupName, string description)
		{
			this.EmployeeGroupID = employeeGroupID;
			this.GroupName = groupName;
			this.Description = description;
		}
	}
}
