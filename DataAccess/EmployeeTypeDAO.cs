using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface EmployeeTypeDAO
    {
        int insert(EmployeeTypeTO rule);

        bool update(EmployeeTypeTO rule);

        bool delete(int recID);

        List<EmployeeTypeTO> search(EmployeeTypeTO rule);

        Dictionary<int, Dictionary<int, string>> searchDictionary(EmployeeTypeTO typeTO);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
