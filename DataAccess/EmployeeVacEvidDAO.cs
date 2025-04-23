using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface EmployeeVacEvidDAO
    {
        int insert(EmployeeVacEvidTO employeeVacEvidTO);

        List<EmployeeVacEvidTO> search(string employeeID, DateTime yearFrom, DateTime yearTo ,int daysApproveMin, int daysApproveMax);

        int searchCount(string employeeID, DateTime yearFrom,DateTime yearTo, int daysApproveMin, int daysApproveMax);

        List<EmployeeVacEvidScheduleTO> getVacationSchedules(int employeeID, DateTime vacYear);

        bool delete(int employeeID, DateTime vacYear);
        
        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();

        bool delete(int employeeID, DateTime vacYear, bool doCommit);

        int insert(EmployeeVacEvidTO employeeVacEvidTO, bool doCommit);

        List<EmployeeVacEvidScheduleTO> getVacationSchedules(string employeesString, DateTime yearFrom, DateTime yearTo);
    }
}
