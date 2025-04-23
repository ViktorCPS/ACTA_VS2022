using System;

namespace DataAccess
{
	/// <summary>
	/// DAO implementation for managing Citizen data form XML files, 
	/// using serialization/deserialization.
	/// </summary>
	public class XMLCitizenDAO : CitizenDAO
	{
		public XMLCitizenDAO()
		{
			
		}
		#region CitizenDAO Members

		public System.Collections.ArrayList getCitizens(string jmbg, string firstName, string lastName)
		{
			// TODO:  Add XMLCitizenDAO.getCitizens implementation
			return null;
		}

		#endregion

	}
}
