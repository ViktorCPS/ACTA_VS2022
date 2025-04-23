using System;
using System.Collections;
using System.Collections.Generic;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for ApplUsersXRoleDAO.
	/// </summary>
	public interface ApplUsersXRoleDAO
	{
		int insert(string userID, int roleID);

		int insert(string userID, int roleID, bool doCommit);

		bool delete(string userID, int roleID);

		bool delete(string userID, int roleID, bool doCommit);

		bool update(string userID, int roleID);

		ApplUsersXRoleTO find(string userID, int roleID);

		List<ApplUsersXRoleTO> getApplUsersXRoles(ApplUsersXRoleTO auXrTO);

		List<ApplRoleTO> findRolesForUserID(string userID, DAOFactory factory);

		List<ApplUserTO> findUsersForRoleID(int roleID);

		bool beginTransaction();

		void commitTransaction();
	
		void serialize(List<ApplUsersXRoleTO> applUsersXWUTOList);
	}
}
