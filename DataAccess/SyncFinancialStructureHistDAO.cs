using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface SyncFinancialStructureHistDAO
    {
        bool insert(SyncFinancialStructureTO syncFSTO, bool doCommit);

        List<SyncFinancialStructureTO> getFS(DateTime from, DateTime to, int fsID, int result);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();

    }
}
