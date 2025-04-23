using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface PassTypeLimitDAO
    {
        int insert(PassTypeLimitTO ptLimitTO);

        bool delete(int limitID);

        bool update(PassTypeLimitTO ptLimitTO);

        PassTypeLimitTO find(int limitID);

        List<PassTypeLimitTO> getPassTypeLimits(PassTypeLimitTO ptLimitTO);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        Dictionary<int, PassTypeLimitTO> getPassTypeLimitsDictionary(PassTypeLimitTO passTypeLimitTO);
    }
}
