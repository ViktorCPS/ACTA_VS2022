using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface LogTmpAdditionalInfoDAO
    {
        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();

        int insert(LogTmpAdditionalInfoTO logTmpTO);

        List<LogTmpAdditionalInfoTO> getLogs(LogTmpAdditionalInfoTO logTmpAdditionalInfoTO);

        bool update(LogTmpAdditionalInfoTO logTmpTo, bool doCommit);

        bool delete(int readerID, uint tagID, int antenna, DateTime eventTime);

        LogTmpAdditionalInfoTO find(int readerID, uint tagID, int antenna, DateTime eventTime);
    }
}
