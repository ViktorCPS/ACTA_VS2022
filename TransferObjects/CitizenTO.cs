using System;

namespace TransferObjects
{
	/// <summary>
	/// Summary description for CitizenTO.
	/// </summary>
	public class CitizenTO
	{
		private string _jmbg;
		private string _firstName;
		private string _lastName;

		public string JMBG
		{
			get{ return _jmbg; }
			set{ _jmbg = value; }
		}

		public string FirstName
		{
			get{ return _firstName; }
			set{ _firstName = value; }
		}

		public string LastName
		{
			get{ return _lastName; }
			set{ _lastName = value; }
		}

		public CitizenTO()
		{
			this.JMBG = "";
			this.FirstName = "";
			this.LastName = "";
			
		}

		public CitizenTO(string jmbg, string firstName, string lastName)
		{
			this.JMBG = jmbg;
			this.FirstName = firstName;
			this.LastName = lastName;		
		}
	}
}
