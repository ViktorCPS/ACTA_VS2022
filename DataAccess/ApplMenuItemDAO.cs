using System;
using System.Collections;
using System.Collections.Generic;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for ApplMenuItemDAO.
	/// </summary>
	public interface ApplMenuItemDAO
	{
		int insert(ApplMenuItemTO applMenuItemTO);

		bool delete(string applMenuItemID);

		bool update(ApplMenuItemTO applMenuItemTO);

        bool updateSamePosition(ApplMenuItemTO applMenuItemTO);

		bool updateEmptyRole(int applRoleID, bool doCommit);

		ApplMenuItemTO find(string applMenuItemID);

		List<ApplMenuItemTO> getApplMenuItems(ApplMenuItemTO menuItemTO);

		void serialize(List<ApplMenuItemTO> applMenuItemsTOList);
	}
}
