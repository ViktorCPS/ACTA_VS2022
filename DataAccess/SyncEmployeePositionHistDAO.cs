using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface SyncEmployeePositionHistDAO
    {
        bool insert(SyncEmployeePositionTO syncPosTO, bool doCommit);

        List<SyncEmployeePositionTO> getPositions(DateTime from, DateTime to, int posID, int result);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();
    }
}
