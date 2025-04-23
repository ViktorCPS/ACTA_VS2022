using System;
using System.Collections;
using System.Collections.Generic;

using TransferObjects;
using System.Data;

namespace DataAccess
{
	/// <summary>
	/// Database specific Log classes implemets 
	/// LogDAO interface  
	/// </summary>
	public interface LogDAO
	{
		// Insert data from logTo object into Log table
		int insert(LogTO logTo);

        // Insert data from logTo object into Log table and LogAdditionalInfo table
        int insertMobile(LogTmpAdditionalInfoTO logTo, bool doCommit);

		// Delete log with specified LogID
		bool delete(int LogID);

		// Update Log with data from Log Transfer Object
		//bool update(LogTO logTo);

		// Update Log with data from Log Transfer Object 
		bool update(LogTO logTo, bool doCommit);

		// Find log record with a given ID
		LogTO find(int LogID);

		// Get all records from Log table, 
		// return ArrayList of Log transmission objects
		List<LogTO> getLogs(LogTO logTo);

		// Get all records from Log table
		List<LogTO> getLogs(string logID, string readerID, string tagID, string antenna, string eventHappened,
						string actionCommited, string  eventTime, string passGenUsed);

		Dictionary<long, LogTO> getLogs(int event1, int event2, int passGenUsed);

		bool serialize(List<LogTO> visitTO, string filePath);

		List<LogTO> deserialize(string filePath);

        bool serializeMobile(List<LogTmpAdditionalInfoTO> logList, string filePath);

        List<LogTmpAdditionalInfoTO> deserializeMobile(string filePath);        

		int insertToTmp(LogTO logTo);

        int insertToMobileTmp(LogTmpAdditionalInfoTO logTo);

		bool clearLogTmp();

        bool clearLogMobileTmp();

		int importLog();

        List<LogTmpAdditionalInfoTO> getLogMobileTmp();

        List<LogTO> getTrespassLogs(int locationID, int gateID, int readerID, string direction, int employeeID, int eventHappened, DateTime dateFrom, DateTime dateTo);
        
        bool deleteLogs(DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo, string readerId);
        
        List<LogTO> getLogsForGraph(DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo, string readerId);

        List<LogTO> getLogsForPeriod(LogTO logTO, DateTime from, DateTime to);

        List<LogTO> getLogIn(int employeeID, DateTime dateTime);

        List<LogTO> getLogsForReader(int readerID);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();
    }
}
