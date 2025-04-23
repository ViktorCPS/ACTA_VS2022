using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;

namespace DataAccess
{
    public interface EmployeeLockedDayDAO
    {
        int insert(EmployeeLockedDayTO lockedDayTO, bool doCommit);

        bool delete(EmployeeLockedDayTO lockedDayTO, bool doCommit);

        Dictionary<int, List<DateTime>> getLockedDays(string emplIDs, string type, DateTime from, DateTime to);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        System.Data.IDbTransaction getTransaction();

        void setTransaction(System.Data.IDbTransaction trans);
    }
}
