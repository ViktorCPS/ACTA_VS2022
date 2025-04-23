using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface ApplUserXOrgUnitDAO
    {
        int insert(string userID, int ouID, string purpose);

        int insert(string userID, int ouID, string purpose, bool doCommit);

        bool delete(string userID, int ouID, string purpose);

        bool delete(string userID, int ouID, string purpose, bool doCommit);

        List<ApplUserXOrgUnitTO> getApplUsersXOU(ApplUserXOrgUnitTO auXwuTO);

        List<ApplUserXOrgUnitTO> getApplUsersXOU(string ids);

        Dictionary<int, OrganizationalUnitTO> findOUForUserIDDictionary(string userID, string purpose, DAOFactory daoFactory);

        List<ApplUserTO> findUsersForOUID(int ouID, string purpose, List<string> statuses);

        List<ApplUserXOrgUnitTO> findGrantedParentOUForOU(ApplUserXOrgUnitTO auXouTO);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        bool delete(int ouID, bool doCommit);
    }
}
