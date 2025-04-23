using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;

namespace DataAccess
{
    public interface VaccineDAO
    {
        int insert(VaccineTO vacTO, bool doCommit);

        bool update(VaccineTO vacTO, bool doCommit);

        bool delete(int vacID, bool doCommit);

        List<VaccineTO> getVaccines(VaccineTO vacTO);

        Dictionary<int, VaccineTO> getVaccinesDictionary(VaccineTO vacTO);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        System.Data.IDbTransaction getTransaction();

        void setTransaction(System.Data.IDbTransaction trans);
    }
}
