using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface EmployeeCounterMonthlyBalanceHistDAO
    {
        int insert(EmployeeCounterMonthlyBalanceTO balanceTO, bool doCommit);
        
        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
