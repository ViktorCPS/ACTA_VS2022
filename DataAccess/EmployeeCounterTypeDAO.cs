using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface EmployeeCounterTypeDAO
    {
        int insert(EmployeeCounterTypeTO emplCTypeTO);

        bool delete(int typeID);

        bool update(EmployeeCounterTypeTO emplCTypeTO);

        EmployeeCounterTypeTO find(int typeID);

        List<EmployeeCounterTypeTO> getEmplCounterTypes(EmployeeCounterTypeTO emplCTypeTO);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
