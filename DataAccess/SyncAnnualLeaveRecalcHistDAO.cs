using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface SyncAnnualLeaveRecalcHistDAO
    {
        bool insert(SyncAnnualLeaveRecalcTO syncRecalcTO, bool doCommit);

        List<SyncAnnualLeaveRecalcTO> getALRecalculations(DateTime from, DateTime to, int emplID, int result);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();
    }
}
