using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    /// <summary>
    /// EmployeeGroupsTimeScheduleDAO interface is implemented by 
    /// database specific EmployeeGroupsTimeSchedule DAO classes.
    /// </summary>
    public interface EmployeeGroupsTimeScheduleDAO
    {
        int insert(int employeeGroupID, DateTime date, int timeSchemaID, int startCycleDay, bool doCommit);

        bool deleteMonthSchedule(int employeeGroupID, DateTime date, bool doCommit);

        bool deleteSchedule(int employeeGroupID, DateTime from, DateTime to, bool doCommit);

        List<EmployeeGroupsTimeScheduleTO> getEmployeeMonthSchedules(int employeeGroupID, DateTime date);

        List<EmployeeGroupsTimeScheduleTO> getEmployeeMonthSchedules(int employeeGroupID, DateTime date, IDbTransaction trans);

        List<EmployeeGroupsTimeScheduleTO> getGroupsSchedules(string groups, DateTime fromDate, DateTime toDate, IDbTransaction trans);

        List<EmployeeGroupsTimeScheduleTO> getGroupFromSchedules(int employeeGroupID, DateTime date, IDbTransaction trans);

        List<EmployeeGroupsTimeScheduleTO> getGroupsNextSchedule(string employeeGroups, DateTime date);

        bool deleteSchedule(int employeeGroupID, bool doCommit);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        //natalija07112017
        EmployeeGroupsTimeScheduleTO find(int timeSchemaID, int employeeGroupID, IDbTransaction trans); 
    }
}
