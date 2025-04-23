using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using TransferObjects;


namespace DataAccess
{
	/// <summary>
	/// WorkingGroupDAO interface is implemented by 
	/// database specific WorkingGroup DAO classes
	/// </summary>
	public interface WorkingGroupDAO
	{
        int insert(string groupName, string description, DateTime date, int timeSchemaID, int startCycleDay);

		int insert(WorkingGroupTO wrkGroupTO);

		bool delete(int empolyeeGroupID);

		bool update(int employeeGroupID, string groupName, string description);

        bool update(int employeeGroupID, string groupName, string description, bool doCommit);

		WorkingGroupTO find(int employeeGroupID);

        WorkingGroupTO find(int employeeGroupID, IDbTransaction trans);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

		List<WorkingGroupTO> getWorkingGroups(WorkingGroupTO wgTO);

        List<WorkingGroupTO> getWorkingGroupsIDSort(WorkingGroupTO wgTO);

        bool delete(int employeeGroupID, bool doCommit);
    }
}
