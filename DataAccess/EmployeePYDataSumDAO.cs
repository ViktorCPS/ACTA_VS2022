using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface EmployeePYDataSumDAO
    {
        int insert(EmployeePYDataSumTO emplPYDataSum, bool doCommit);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();

        uint getMaxCalcID();

        DateTime getSumMonth(uint calcID);

        List<EmployeePYDataSumTO> getSumDates(DateTime from, DateTime to);

        List<int> getEmployees(uint calcID);

        List<EmployeePYDataSumTO> getEmployeesSum(EmployeePYDataSumTO emplTO);

        Dictionary<int, Dictionary<string, decimal>> getEmployeesSumValues(uint calcID, string codes);
    }
}
