using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;

namespace DataAccess
{
    public interface EmployeePhysicalDataDAO
    {
        int insert(EmployeePhysicalDataTO dataTO, bool doCommit);

        bool update(EmployeePhysicalDataTO dataTO, bool doCommit);

        bool delete(string recID, bool doCommit);

        List<EmployeePhysicalDataTO> getEmployeePhysicalData(EmployeePhysicalDataTO dataTO);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        System.Data.IDbTransaction getTransaction();

        void setTransaction(System.Data.IDbTransaction trans);
    }
}
