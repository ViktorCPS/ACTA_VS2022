using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;

namespace DataAccess
{
    public interface EmployeePositionXRiskDAO
    {
        int insert(EmployeePositionXRiskTO emplPosRiskTO, bool doCommit);

        bool update(EmployeePositionXRiskTO emplPosRiskTO, bool doCommit);

        bool delete(int posID, int riskID, bool doCommit);

        List<EmployeePositionXRiskTO> getEmployeePositionXRisks(EmployeePositionXRiskTO emplPosRiskTO);

        List<EmployeePositionXRiskTO> getEmployeePositionXRisksByWU(EmployeePositionXRiskTO emplPosRiskTO, int working_unit_id);

        List<RiskTO> getRisks(string posIDs);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        System.Data.IDbTransaction getTransaction();

        void setTransaction(System.Data.IDbTransaction trans);
    }
}
