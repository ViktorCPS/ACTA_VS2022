using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for ExtraHourDAO.
	/// </summary>
	public interface ExtraHourDAO
	{
		int insert(int employeeID, DateTime dateEarned, int extraTimeAmt);

        int insert(int employeeID, DateTime dateEarned, int extraTimeAmt, bool doCommit);

		bool update(int employeeID, DateTime dateEarned, int extraTimeAmt);

		bool delete(int employeeID, DateTime dateEarned);

		ExtraHourTO find(int employeeID, DateTime dateEarned);

		List<ExtraHourTO> getExtraHour(int employeeID, DateTime earnedFrom, DateTime earnedTo);

		List<ExtraHourTO> getEmployeeSum(int employeeID);

		List<ExtraHourTO> getEmployeeAvailableDates(int employeeID);

		int extraHoursCount();

		void serialize(List<ExtraHourTO> extraHourTOList);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
	}
}
