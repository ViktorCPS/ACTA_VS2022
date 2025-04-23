using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface SyncEmployeesHistDAO
    {
        bool insert(SyncEmployeeTO syncEmplTO, bool doCommit);

        List<SyncEmployeeTO> getEmployees(DateTime from, DateTime to, int emplID, int result);

        int getLastOU(int employeeID);

        int getLastWU(int employeeID);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();
    }
}
