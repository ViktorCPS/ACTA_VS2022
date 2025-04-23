using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface EmployeePYTransportDataDAO
    {
        int insert(EmployeePYTransportDataTO dataTO, bool doCommit);

        bool delete(string emplIDs, DateTime from, DateTime to, bool doCommit);

        Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>> getEmplTransportData(string emplIDs, DateTime month);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
