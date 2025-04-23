using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;

namespace DataAccess
{
    public interface RiskDAO
    {
        int insert(RiskTO riskTO, bool doCommit);

        bool update(RiskTO riskTO, bool doCommit);

        bool delete(int riskID, bool doCommit);

        List<RiskTO> getRisks(RiskTO riskTO);

        Dictionary<int, RiskTO> getRisksDictionary(RiskTO riskTO);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        System.Data.IDbTransaction getTransaction();

        void setTransaction(System.Data.IDbTransaction trans);
    }
}
