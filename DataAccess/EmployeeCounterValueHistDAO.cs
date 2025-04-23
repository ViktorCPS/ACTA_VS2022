using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface EmployeeCounterValueHistDAO
    {
        int insert(EmployeeCounterValueHistTO emplCValueTO, bool doCommit);

        int insert(int emplID, string modifiedBy, bool doCommit);
        
        List<EmployeeCounterValueHistTO> getEmplCounterValuesHist(EmployeeCounterValueHistTO emplCValueTO);

        int getFirstModifiedValue(int emplID, int counterType);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
