using System;
using System.Collections;
using System.Data;

using TransferObjects;
using System.Collections.Generic;

namespace DataAccess
{
	/// <summary>
	/// Summary description for PassDAO.
	/// </summary>
	public interface PassDAO
	{
		int insert(int employeeID, string direction, DateTime eventTime, int passTypeID, int pairGenUsed,
			int locationID, int manualCreated, int isWrkHrsCount);

		//int insert(PassTO passTo);
		int insert(PassTO passTO, bool doCommit);

		int insert(PassTO passTO, string createdBy, bool doCommit);

        int insertGetID(PassTO passTO, bool doCommit);

		int insert(List<PassTO> passTOList, ExitPermissionTO perm, string createdBy, DAOFactory factory);

		int insertPassesPermission(List<PassTO> passTOList, ExitPermissionTO perm, string createdBy, DAOFactory factory);

		bool delete(string passID);

		bool delete(string passID, bool doCommit);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

		bool log2pass(List<PassTO> passTOList,  List<LogTO> logTO, ExitPermissionTO exitPerm, DAOFactory factory);

        void logToPass(List<PassTO> passTOList, List<LogTO> logTOList, DAOFactory factory);

		bool update(int passID, int employeeID, string direction, DateTime eventTime, int passTypeID, int pairGenUsed,
			int locationID, int manualCreated, int isWrkHrsCount);

		bool update(int employeeID, string date, bool doCommit);

		bool update(PassTO passTo, bool doCommit);
		
		PassTO find(string passID);

		List<PassTO> getPasses(PassTO passTO);

        List<PassTO> getPassForPerm(int employeeID, string direction, DateTime eventTime, int isWrkHrsCount);

		List<PassTO> getPassesInterval(PassTO passTO, DateTime fromTime, DateTime toTime, string wUnits, DateTime advFromTime, DateTime advToTime);

        Dictionary<int, Dictionary<DateTime, List<PassTO>>> getPassesInterval(DateTime month, string emplIDs);

		int getPassesIntervalCount(PassTO passTO, DateTime fromTime, DateTime toTime, string wUnits, DateTime advFromTime, DateTime advToTime);

        List<PassTO> getPassesForSnapshots(string passID);

		//ArrayList getPasses(PassTO passTo);

		List<PassTO> getPassesList(PassTO passTO);
		
		ArrayList getEmpoloyeesByDate();
		
		List<PassTO> getPassesForEmployee(int empoyeeID, string date);

        List<PassTO> getPassesForEmployeeSched(int employeeID);

        List<PassTO> getPassesForEmployeeAll(int empoyeeID, DateTime date);

        List<PassTO> getPassesForExitPerm(int employeeID, DateTime eventTime);

        List<PassTO> getPassesForExitPerm(int employeeID, DateTime startTime, int offset);

        List<PassTO> getPermPassesForPair(int employeeID, DateTime startTime, DateTime endTime);

        List<PassTO> getCurrentPasses(PassTO passTO, DateTime fromTime, DateTime toTime, string wUnits, string modifiedBy);

		PassTO getPassBeforePermissionPass(int empoyeeID, DateTime eventTime);

        bool serialize(List<PassTO> passesTOList);

        List<PassTO> deserialize(string filePath);

        void UnlockPasses(DateTime startDate, DateTime endDate, bool doCommit);

        List<PassTO> getPassesIntervalForZINReport(DateTime fromTime, DateTime toTime, string employees);

        int getPassesIntervalCountForZINReport(DateTime fromTime, DateTime toTime, string employees);

        List<PassTO> getDiferencePasses(DateTime lastReadingTime);

        Dictionary<int, Dictionary<DateTime, List<PassTO>>> getPassesForEmployeesPeriod(string employeesID, DateTime from, DateTime to);

        int getGate(int readerID);

        int getOrganUnitID(ulong tagID);

        bool compareGates(int gateID, int organUnitID);
        
        List<PassTO> getListOfIN_OUTforDay(DateTime date, int emplID, string direction);

        void updateListPassesToWrkHrs0(string passIDs);
        List<PassTO> getPassesForDayForEmployee(int emplID, DateTime day);
        void updatePassesOnUnprocessed(int emplID, DateTime date, bool doCommit);
    }
}
