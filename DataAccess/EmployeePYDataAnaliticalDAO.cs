using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface EmployeePYDataAnaliticalDAO
    {
        int insert(EmployeePYDataAnaliticalTO emplPYDataAnalitical, bool doCommit);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();

        List<EmployeePYDataAnaliticalTO> getEmployeesAnalitical(string employees, string payment_code, uint py_calc_id, string type);
    }
}
