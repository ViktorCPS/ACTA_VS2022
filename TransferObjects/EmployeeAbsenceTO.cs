using System;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for EmployeeAbsenceTO.
	/// </summary>
	public class EmployeeAbsenceTO
	{
		private int _recID = -1;
		private int _employeeID = -1;
		private int _passTypeID = -1;
		private DateTime _dateStart = new DateTime();
		private DateTime _dateEnd = new DateTime();
		private int _used = -1;
		private string _employeeName = "";
		private string _passType = "";
        private string _createdBy = "";
        private DateTime _vacationYear = new DateTime();
        private string _description = "";
        private DateTime _createdTime = new DateTime();

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public DateTime VacationYear
        {
            get { return _vacationYear; }
            set { _vacationYear = value; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

		public int RecID
		{
			get{ return _recID; }
			set{ _recID = value; }
		}

		public int EmployeeID
		{
			get{ return _employeeID; }
			set{ _employeeID = value; }
		}

		public int PassTypeID
		{
			get{ return _passTypeID; }
			set{ _passTypeID = value; }
		}

		public DateTime DateStart
		{
			get{ return _dateStart; }
			set{ _dateStart = value; }
		}

		public DateTime DateEnd
		{
			get{ return _dateEnd; }
			set{ _dateEnd = value; }
		}

		public int Used
		{
			get{ return _used; }
			set{ _used = value; }
		}

		public string EmployeeName
		{
			get{ return _employeeName; }
			set{ _employeeName = value; }
		}

		public string PassType
		{
			get{ return _passType; }
			set{ _passType = value; }
		}

		public EmployeeAbsenceTO(int recID, int employeeID, int passTypeID, DateTime dateStart, DateTime dateEnd, int used)
		{
			this.RecID = recID;
			this.EmployeeID = employeeID;
			this.PassTypeID = passTypeID;
			this.DateStart = dateStart;
			this.DateEnd = dateEnd;
			this.Used = used;
            this.CreatedBy = "";
            this.VacationYear = new DateTime();
            this.Description = "";
		}

		public EmployeeAbsenceTO()
		{ }
	}
}
