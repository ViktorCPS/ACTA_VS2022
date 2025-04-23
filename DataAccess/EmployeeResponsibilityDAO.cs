using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    public interface EmployeeResponsibilityDAO
    {
        bool insert(EmployeeResponsibilityTO emplResponsibilityTO, bool doCommit);

        List<EmployeeResponsibilityTO> getEmplResponsibility(EmployeeResponsibilityTO ouTO);

        List<EmployeeResponsibilityTO> getEmplResponsibility(string ids, string type);

        Dictionary<int, List<int>> getUnitsResponsibilitiesByEmployee(int id, string type);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        bool delete(EmployeeResponsibilityTO ResponsibilityTO, bool doCommit);
    }
}

