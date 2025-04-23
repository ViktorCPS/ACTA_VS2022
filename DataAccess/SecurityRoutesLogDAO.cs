using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Collections.Generic;
using TransferObjects;

namespace DataAccess
{
    public interface SecurityRoutesLogDAO
    {
        int insert(int readerID, string tagID, int employeeID, DateTime eventTime);

        ArrayList getLogsInterval(int employeeID, int readerID, string tagID, DateTime fromTime, DateTime toTime, string wUnits);

        int getEmplCount(string employeeID);

        int insert(int readerID, string tagID, int employeeID, DateTime eventTime, bool doCommit);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();

        ArrayList search(int EmployeeID, string tagID, string workingUnitID, DateTime dateFrom, DateTime dateTo, DateTime fromTime, DateTime toTime);
    }
}
