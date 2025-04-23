using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface SyncOrganizationalStructureHistDAO
    {
        bool insert(SyncOrganizationalStructureTO syncFSTO, bool doCommit);

        List<SyncOrganizationalStructureTO> getOU(DateTime from, DateTime to, int ouID, int result);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();
    }
}
