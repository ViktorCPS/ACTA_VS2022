using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface OrganizationalUnitDAO
    {
        int insert(OrganizationalUnitTO ouTO, bool doCommit);

        bool update(OrganizationalUnitTO ouTO, bool doCommit);

        bool delete(int ouID);

        OrganizationalUnitTO find(int ouID);

        List<OrganizationalUnitTO> getOrgUnits(OrganizationalUnitTO ouTO);

        Dictionary<int, OrganizationalUnitTO> getOrgUnitsDictionary();
        
        List<OrganizationalUnitTO> getOrgUnits(string oUnits);
        List<OrganizationalUnitTO> getOUByName(string name);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        DataSet getOrgUnitsDS(string oUnits);

        List<OrganizationalUnitTO> getChildOU(string parentID);

        bool delete(int ouID, bool doCommit);

        int findMAXOUID();

    }
}
