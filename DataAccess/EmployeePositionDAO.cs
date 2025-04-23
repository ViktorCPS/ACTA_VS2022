using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;

namespace DataAccess
{
    public interface EmployeePositionDAO
    {
        int insert(EmployeePositionTO posTO, bool doCommit);
        
        bool update(EmployeePositionTO posTO, bool doCommit);
        
        bool delete(int posID, bool doCommit);

        List<EmployeePositionTO> getEmployeePositions(EmployeePositionTO posTO);
     
        Dictionary<int,EmployeePositionTO> getEmployeePositionsDictionary(EmployeePositionTO posTO);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        System.Data.IDbTransaction getTransaction();

        void setTransaction(System.Data.IDbTransaction trans);
    }
}
