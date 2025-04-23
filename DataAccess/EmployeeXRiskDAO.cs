using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;

namespace DataAccess
{
    public interface EmployeeXRiskDAO
    {
        int insert(EmployeeXRiskTO emplRiskTO, bool doCommit);

        bool update(EmployeeXRiskTO emplRiskTO, bool doCommit);

        bool delete(string recID, bool doCommit);

        List<EmployeeXRiskTO> getEmployeeXRisks(EmployeeXRiskTO emplRiskTO);

        int getEmployeeXRisksCount(EmployeeXRiskTO emplRiskTO);

        Dictionary<uint, EmployeeXRiskTO> getEmployeeXRisks(string emplIDs, string risks, DateTime from, DateTime to);

        List<EmployeeXRiskTO> getEmployeeXRisksNotScheduled(string emplIDs, DateTime from, DateTime to);

        int getMaxRotation();

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        System.Data.IDbTransaction getTransaction();

        void setTransaction(System.Data.IDbTransaction trans);
    }
}
