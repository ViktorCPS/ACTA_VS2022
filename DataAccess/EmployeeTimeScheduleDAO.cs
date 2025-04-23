using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// EmployeeTimeScheduleDAO interface is implemented by 
	/// database specific EmployeeTimeSchedule DAO classes.
	/// </summary>
	public interface EmployeeTimeScheduleDAO
	{
		int insert(int employeeID, DateTime date, int timeSchemaID, int startCycleDay);

        int insert(int employeeID, DateTime date, int timeSchemaID, int startCycleDay, string user, bool doCommit);

		int insert(EmployeeTimeScheduleTO emplTimeScheduleTO);

        bool deleteFromToSchedule(int employeeID, DateTime fromDate, DateTime toDate, string modifiedBy, bool doCommit);

        //natalija07112017
        bool deleteFromToSchedule(int employeeID, DateTime fromDate, DateTime toDate, string modifiedBy, bool doCommit, bool statuscb); 

		bool update(int employeeID, DateTime date, int timeSchemaID, int startCycleDay);

        bool update(int employeeID, DateTime date, int timeSchemaID, int startCycleDay, bool naDan);

		EmployeeTimeScheduleTO find(int employeeID, DateTime date);

		List<EmployeeTimeScheduleTO> getEmployeeTimeSchedules(EmployeeTimeScheduleTO emplTSTO);

        List<EmployeeTimeScheduleTO> getEmployeeMonthTimeSchedules(DateTime month);

        List<EmployeeTimeScheduleTO> getEmployeeMonthSchedules(int employeeID, DateTime date);

        List<EmployeeTimeScheduleTO> getEmployeesSchedules(string employees, DateTime fromDate, DateTime toDate);

        Dictionary<int, List<EmployeeTimeScheduleTO>> getEmployeesSchedulesDS(string employees, DateTime fromDate, DateTime toDate);

        List<EmployeeTimeScheduleTO> getEmployeesSchedules(string employees, DateTime fromDate, DateTime toDate, IDbTransaction trans);

        DataSet getTimeSchedules();

        List<EmployeeTimeScheduleTO> getEmployeeMonthSchedulesFromDataSet(int employeeID, DateTime date, DataSet dsTimeSchedules);

        List<EmployeeTimeScheduleTO> getEmployeesNextSchedule(string employees, DateTime date, IDbTransaction trans);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        Dictionary<int, List<EmployeeTimeScheduleTO>> getEmployeesSchedulesDS(string employees, DateTime fromDate, DateTime toDate, IDbTransaction trans);

        Dictionary<int, List<EmployeeTimeScheduleTO>> getEmployeesSchedulesExactDate(string employees, DateTime fromDate, DateTime toDate, IDbTransaction trans);

        List<EmployeeTimeScheduleTO> findEmplSch(int emplID);
    }
}
