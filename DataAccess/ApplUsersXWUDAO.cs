using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for ApplUsersXWUDAO.
	/// </summary>
	public interface ApplUsersXWUDAO
	{
		int insert(string userID, int wuID, string purpose);

		int insert(string userID, int wuID, string purpose, bool doCommit);

		bool delete(string userID, int wuID, string purpose);

		bool delete(string userID, int wuID, string purpose, bool doCommit);

		bool delete(int wuID, bool doCommit);

		bool update(string userID, int wuID, string purpose);

		ApplUsersXWUTO find(string userID, int wuID, string purpose);

		List<ApplUsersXWUTO> getApplUsersXWU(ApplUsersXWUTO auXwuTO);

        List<WorkingUnitTO> findWUForUserID(string userID, string purpose, DAOFactory daoFactory);

        List<WorkingUnitTO> findWUForUser(string userID, string purpose, DAOFactory daoFactory, IDbTransaction trans);

       Dictionary<int,WorkingUnitTO> findWUForUserIDDictionary(string userID, string purpose, DAOFactory daoFactory);

		List<ApplUserTO> findUsersForWUID(int wuID, string purpose, List<string> statuses);

        List<ApplUsersXWUTO> findGrantedParentWUForWU(ApplUsersXWUTO auXwuTO);

		bool beginTransaction();

		void commitTransaction();

		void rollbackTransaction();

		IDbTransaction getTransaction();

		void setTransaction(IDbTransaction trans);
	
		void serialize(List<ApplUsersXWUTO> applUsersXWUTOList);
	
	}
}
