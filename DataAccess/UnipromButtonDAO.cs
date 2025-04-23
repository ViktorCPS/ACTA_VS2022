using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

namespace DataAccess
{
    public interface UnipromButtonDAO
    {
        int update(int readerID, int antennaOutput, int status, bool doCommit);

        ArrayList search(int readerId, int antenna);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();
    }
}
