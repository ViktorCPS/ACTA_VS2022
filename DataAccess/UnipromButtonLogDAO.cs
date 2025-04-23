using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

namespace DataAccess
{
    public interface UnipromButtonLogDAO
    {
        int insert(int readerID, int antennaOutput, int status, bool doCommit);

        ArrayList search(int readerId);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();
    }
}
