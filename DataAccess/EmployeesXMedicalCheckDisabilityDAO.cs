using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;

namespace DataAccess
{
    public interface EmployeesXMedicalCheckDisabilityDAO
    {
        int insert(EmployeeXMedicalCheckDisabilityTO disabilityTO, bool doCommit);

        bool update(EmployeeXMedicalCheckDisabilityTO disabilityTO, bool doCommit);

        bool delete(string recID, bool doCommit);

        List<EmployeeXMedicalCheckDisabilityTO> getEmployeeXMedicalCheckDisabilities(EmployeeXMedicalCheckDisabilityTO disabilityTO);

        Dictionary<uint, EmployeeXMedicalCheckDisabilityTO> getEmployeeXMedicalCheckDisabilities(string emplIDs, string data, DateTime from, DateTime to);
        
        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        System.Data.IDbTransaction getTransaction();

        void setTransaction(System.Data.IDbTransaction trans);
    }
}
