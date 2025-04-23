using System;
using System.Collections;
using System.Collections.Generic;

using TransferObjects;
using System.Data;

namespace DataAccess
{
	/// <summary>
	/// Summary description for ApplUserDAO.
	/// </summary>
	public interface ApplUserDAO
	{
		int insert(ApplUserTO userTO);

		bool delete(string userID);

		bool update(ApplUserTO userTO);

        bool updatePassword(string employeeID, string password);

		bool updateExitPermVerification(ApplUserTO userTO);

		ApplUserTO find(string userID);

		ApplUserTO findUserPassword(string userID, string password);

		ApplUserTO findExitPermVerification(string userID);

		List<ApplUserTO> getApplUsers(ApplUserTO userTO);

        List<ApplUserTO> getInactiveUsers(DateTime monthCreated);

        Dictionary<string, ApplUserTO> getApplUsersDictionary(ApplUserTO userTO);

        List<ApplUserTO> getApplUsersForCategory(int userCategory);

        List<ApplUserTO> getApplUsersVerifiedByWUnits(string wUnits);

        List<ApplUserTO> getApplUsersWithStatus(ApplUserTO userTO, List<string> statuses);

        void serialize(List<ApplUserTO> applUsersXWUTOList);

        int insert(ApplUserTO applUserTO, bool doCommit);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();

        bool updateExitPermVerification(ApplUserTO applUserTO, bool doCommit);

        bool update(ApplUserTO applUserTO, bool doCommit);
    }
}
