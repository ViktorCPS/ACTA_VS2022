using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;

namespace DataAccess
{
    public interface EmployeeXVaccineDAO
    {
        int insert(EmployeeXVaccineTO vacTO, bool doCommit);

        bool update(EmployeeXVaccineTO vacTO, bool doCommit);

        bool delete(string recID, bool doCommit);

        List<EmployeeXVaccineTO> getEmployeeXVaccines(EmployeeXVaccineTO vacTO);

        Dictionary<uint, EmployeeXVaccineTO> getEmployeeXVaccines(string emplIDs, string vaccines, DateTime from, DateTime to);

        List<EmployeeXVaccineTO> getEmployeeXVaccinesNotProcessed(string emplIDs);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        System.Data.IDbTransaction getTransaction();

        void setTransaction(System.Data.IDbTransaction trans);
    }
}
