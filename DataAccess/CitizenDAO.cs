using System;
using System.Collections;

namespace DataAccess
{
	/// <summary>
	/// Summary description for CitizenDAO.
	/// </summary>
	public interface CitizenDAO
	{
		ArrayList getCitizens(string jmbg, string firstName, string lastName);
	}
}
