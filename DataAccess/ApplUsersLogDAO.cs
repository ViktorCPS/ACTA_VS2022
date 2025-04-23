using System;
using System.Collections;
using System.Collections.Generic;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public interface ApplUsersLogDAO
	{
		ApplUserLogTO insert(ApplUserLogTO userTO);
        List<ApplUserLogTO> getOpenSessions(string userID, string host, string chanel);
        ApplUserLogTO findMaxSession(string userID, string host, string chanel);
		int update(int logID);
        int update(int loginID, string createdBy, string modifiedBy);
        List<ApplUserLogTO> getApplUsersLog(ApplUserLogTO logTO, string userIDs, DateTime dateFrom, DateTime dateTo, List<string> changeTables, Dictionary<string, ApplUserTO> users);
	}
}
