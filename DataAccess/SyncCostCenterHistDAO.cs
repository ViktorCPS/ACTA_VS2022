using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    public interface SyncCostCenterHistDAO
    {
        bool insert(SyncCostCenterTO syncCCTO, bool doCommit);

        List<SyncCostCenterTO> getCC(DateTime from, DateTime to, string code, string company, int result);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();
    }
}
