using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface SyncBufferDataHistDAO
    {
        bool insert(SyncBufferDataTO syncBDTO, bool doCommit);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();

        DateTime getMaxDate();
    }
}
