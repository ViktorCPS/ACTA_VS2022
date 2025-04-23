using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface BufferMonthlyBalancePaidDAO
    {
        int insert(BufferMonthlyBalancePaidTO balanceTO, bool doCommit);

        bool update(BufferMonthlyBalancePaidTO balanceTO, bool doCommit);        

        Dictionary<int, Dictionary<DateTime, Dictionary<int, BufferMonthlyBalancePaidTO>>> getEmployeeBalancesPaid(uint pyCalcID, int counterType);

        Dictionary<int, Dictionary<DateTime, Dictionary<int, BufferMonthlyBalancePaidTO>>> getEmployeeBalancesPaid(string emplIDs, DateTime from, DateTime to);
        
        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
