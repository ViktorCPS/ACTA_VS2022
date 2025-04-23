using System;
using System.Collections;
using System.Collections.Generic;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for ApplRoleDAO.
	/// </summary>
	public interface ApplRoleDAO
	{
		int insert(int applRoleID, string name, string description);

		bool delete(int applRoleID);

		bool update(int appRoleID, string name, string description);

		bool updateOnEmptyRole(int applRoleID, DAOFactory factory);

		ApplRoleTO find(int applRoleID);

		int findEmptyRole();

		List<ApplRoleTO> getApplRoles(ApplRoleTO roleTO);

	    List<ApplRoleTO> getUserCreatedRoles();

		void serialize(List<ApplRoleTO> applRolesTOList);
	}
}
