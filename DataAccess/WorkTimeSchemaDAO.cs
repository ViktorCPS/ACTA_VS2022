using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// WorkTimeSchemaDAO is implemented by database specific class;
	/// </summary>
	public interface WorkTimeSchemaDAO
	{
		int insert(WorkTimeSchemaTO wtSchemaTO);

		bool delete(int workTimeSchemaID);

		bool delete(WorkTimeIntervalTO workTimeDayTO);

		bool update(WorkTimeSchemaTO weScemaTO);

		WorkTimeSchemaTO find(int schemaID);

	    List<WorkTimeSchemaTO> getWorkTimeSchemas(WorkTimeSchemaTO tsTO);

        List<WorkTimeSchemaTO> getWorkTimeSchemas(WorkTimeSchemaTO tsTO, IDbTransaction trans);
        
        List<WorkTimeSchemaTO> getWorkTimeSchemas(string timeSchemaID);

		bool timeSchemaIsUsed(int timeSchemaID);

        List<WorkTimeIntervalTO> GetCriticalMoments();

		int getPauses();

        Dictionary<int, WorkTimeSchemaTO> getDictionary(WorkTimeSchemaTO workTimeSchemaTO);

        Dictionary<int, WorkTimeSchemaTO> getDictionary(WorkTimeSchemaTO workTimeSchemaTO, IDbTransaction trans);

        Dictionary<int, WorkTimeSchemaTO> getWorkTimeSchemasDictionary(WorkTimeSchemaTO tsTO, IDbTransaction trans);
    }
}
