using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface EmployeeCounterValueDAO
    {
        int insert(EmployeeCounterValueTO emplCValueTO);

        bool delete(int typeID, int emplID);

        bool update(EmployeeCounterValueTO emplCValueTO, bool doCommit);

        EmployeeCounterValueTO find(int typeID, int emplID);

        List<EmployeeCounterValueTO> getEmplCounterValues(EmployeeCounterValueTO emplCValueTO);

        List<EmployeeCounterValueTO> getEmplCounterValuesNegative(string emplIDs);

        Dictionary<int, Dictionary<int, int>> getEmplCounterValues(string emplIDs);

        Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> getEmplCounterValuesTO(string emplIDs);

        Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> getEmplCounterValuesOrderedByName(string emplIDs);

        Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> getEmplCounterValuesAll();

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        int insert(EmployeeCounterValueTO employeeCounterValueTO, bool doCommit);

        List<EmployeeCounterValueTO> getModifiedValues(DateTime fromDate);

        Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> getEmplCounterValuesTO(string emplIDs, IDbTransaction trans);
    }
}
