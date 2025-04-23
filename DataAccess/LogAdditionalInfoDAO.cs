using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface LogAdditionalInfoDAO
    {
        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();

        int insert(LogAdditionalInfoTO logAdditionalInfoTO);

        List<LogAdditionalInfoTO> getLogs(LogAdditionalInfoTO logAdditionalInfoTO);

        bool update(LogAdditionalInfoTO logAdditionalInfoTO, bool doCommit);

        bool delete(int logID);

        LogAdditionalInfoTO find(int logID);
    }
}
