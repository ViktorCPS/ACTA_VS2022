using System;
using System.Collections;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for EmployeeGroupAccessControlDAO.
	/// </summary>
	public interface EmployeeGroupAccessControlDAO
	{
		int insert(string name, string description);

		bool update(string accessGroupId, string name, string description);

		bool delete(string accessGroupId);

		EmployeeGroupAccessControlTO find(string accessGroupId);

		ArrayList getEmployeeGroupAccessControl(string name);

		void serialize(ArrayList EmployeeGroupAccessControlTOList);
	}
}
