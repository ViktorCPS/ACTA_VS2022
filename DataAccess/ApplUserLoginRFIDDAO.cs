using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface ApplUserLoginRFIDDAO
    {
        int insert(ApplUserLoginRFIDTO rfidTO, bool doCommit);

        bool update(ApplUserLoginRFIDTO rfidTO, bool doCommit);

        bool delete(string host, DateTime created, bool docommit);

        ApplUserLoginRFIDTO getLoginRFID(string host);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
