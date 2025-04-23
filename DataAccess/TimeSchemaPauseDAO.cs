using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for TimeSchemaPauseDAO.
	/// </summary>
	public interface TimeSchemaPauseDAO
	{
		int insert(int pauseID, string description, int pauseDuration,
			int earliestUseTime, int latestUseTime, int pauseOffset, int shortBreakDuration);

		bool update(int pauseID, string description, int pauseDuration,
			int earliestUseTime, int latestUseTime, int pauseOffset, int shortBreakDuration);

		bool delete(int pauseID);

		TimeSchemaPauseTO find(int pauseID);

		int findMAXPauseID();

		List<TimeSchemaPauseTO> getTimeSchemaPause(string description);
        
        List<TimeSchemaPauseTO> getTimeSchemaPause(string description, IDbTransaction trans);

		void serialize(List<TimeSchemaPauseTO> timeSchemaPauseTOList);
	}
}
