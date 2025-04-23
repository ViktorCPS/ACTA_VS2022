using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface EmployeeLoanDAO
    {
        int insert(EmployeeLoanTO loanTO, bool doCommit);

        bool update(EmployeeLoanTO loanTO);

        bool delete(int recID);

        List<EmployeeLoanTO> search(EmployeeLoanTO loanTO);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        List<EmployeeLoanTO> search(EmployeeLoanTO employeeLoanTO, DateTime fromDate, DateTime toDate);
    }
}
