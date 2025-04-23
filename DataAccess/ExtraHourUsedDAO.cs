using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for ExtraHourUsedDAO.
	/// </summary>
	public interface ExtraHourUsedDAO
	{
		int insert(int employeeID, DateTime dateEarned, DateTime dateUsed, int extraTimeAmtUsed, DateTime startTime, DateTime endTime, Int32 ioPairID, string type, bool doCommit);

		bool update(int recordID, int employeeID, DateTime dateEarned, DateTime dateUsed, 
			int extraTimeAmtUsed, bool doCommit);

        bool delete(int recordID, bool isRegularWork);

		bool beginTransaction();

		void commitTransaction();

		void rollbackTransaction();

		IDbTransaction getTransaction();

		void setTransaction(IDbTransaction trans);

		/*ExtraHourTO find(int employeeID, DateTime dateEarned);*/

        List<ExtraHourUsedTO> getExtraHourUsed(int employeeID, DateTime usedFrom, DateTime usedTo, string type);

        List<ExtraHourUsedTO> getExtraHourUsedByType(int emplID, string type);

        List<ExtraHourUsedTO> getEmployeeUsedSum(int employeeID);

        int getEmployeeUsedSumByType(int employeeID, DateTime fromDate, DateTime toDate, string type);

        List<ExtraHourUsedTO> getEmployeeUsedSumDateEarned(int employeeID);

		bool existOverlap(int employeeID, DateTime dateUsed, DateTime startTime, DateTime endTime);

        void serialize(List<ExtraHourUsedTO> extraHourUsedTOList);
	}
}
