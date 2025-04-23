using System;
using System.Collections;
using System.Text;
using System.Data;

namespace DataAccess
{
    public interface SecurityRoutesEmployeeDAO
    {
        int insert(string employeeID);

        bool delete(string employeID);

        ArrayList getEmployeesByWU(string wUnits);

        ArrayList getEmployees();

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();
    }
}
