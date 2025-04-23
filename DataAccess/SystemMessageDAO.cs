using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface SystemMessageDAO
    {
        int insert(SystemMessageTO msgTO, bool doCommit);

        bool update(SystemMessageTO msgTO, bool doCommit);

        bool delete(int msgID, bool doCommit);

        List<SystemMessageTO> getSystemMessages(DateTime from, DateTime to, string company, int role);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();
    }
}
