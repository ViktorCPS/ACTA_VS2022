using System;
using System.Collections;
using System.Collections.Generic;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for ExitPermissionDAO.
	/// </summary>
	public interface ExitPermissionDAO
	{
        int insert(ExitPermissionTO permTO);

		int insert(int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, string verifiedBy);

		int insert(int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, string verifiedBy, bool doCommit);

		bool insertRetroactive(int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, PassTO passTO, DAOFactory factory, string verifiedBy);

		bool delete(int permissionID);

        bool update(ExitPermissionTO permTO);

		bool update(int permissionID, int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, string verifiedBy);

		bool update(int permissionID, int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, bool doCommit);

		bool updateRetroactive(int permissionID, int employeeID, int passTypeID, DateTime startTime, int offset, int used, string description, PassTO pass, DAOFactory factory, string verifiedBy);

		ExitPermissionTO find(int permissionID);
                
		List<ExitPermissionTO> getExitPermissions(ExitPermissionTO permTO, DateTime fromTime, DateTime toTime, string wUnits);

        List<ExitPermissionTO> getExitPermissionsVerifiedBy(ExitPermissionTO permTO, DateTime fromTime, DateTime toTime, string wUnits);

        List<ExitPermissionTO> getValidExitPermissions(string employeeID);

        void serialize(List<ExitPermissionTO> ExitPermissionsTOList);
	}
}
