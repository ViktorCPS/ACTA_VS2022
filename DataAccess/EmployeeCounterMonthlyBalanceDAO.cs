using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface EmployeeCounterMonthlyBalanceDAO
    {
        int insert(EmployeeCounterMonthlyBalanceTO balanceTO, bool doCommit);
        
        bool update(EmployeeCounterMonthlyBalanceTO balanceTO, bool doCommit);

        Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>> getEmployeeBalances(string emplIDs, DateTime month, bool exactMonth);

        Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>> getEmployeeBalances(string emplIDs, DateTime month, int counterTypeID);

        List<EmployeeCounterMonthlyBalanceTO> getEmployeeBalances(string emplIDs, string types, DateTime from, DateTime to);

        DateTime getMinPositiveMonth(string emplIDs);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
