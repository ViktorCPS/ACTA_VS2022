using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface EmployeeXMealTypeEmplDAO
    {
        int insert(int employeeID, int mealTypeEmpl);
        
        bool delete(string employeeID);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
