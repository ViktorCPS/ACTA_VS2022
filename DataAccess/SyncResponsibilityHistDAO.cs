using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface SyncResponsibilityHistDAO
    {
        bool insert(SyncResponsibilityTO syncResponsibilityTO, bool doCommit);

        List<SyncResponsibilityTO> getResponsibilities(DateTime from, DateTime to, int resID, int result);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();
    }
}
