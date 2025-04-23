using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccess
{
    public interface SyncAnnualLeaveRecalcFlagDAO
    {
        bool update(int flag, bool isServiceUpdate, bool doCommit);

        int getFlag();

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();
    }
}
