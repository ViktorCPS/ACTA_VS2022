using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using TransferObjects;
using Util;

namespace DataAccess
{
	/// <summary>
	/// WorkingUnitsDAO interface is implemented by 
	/// database specific WorkingUnits DAO classes
	/// </summary>
	public interface WorkingUnitsDAO
	{
		int insert(WorkingUnitTO wuTO);

		bool delete(int workingUnitsID);

		bool update(WorkingUnitTO wuTO);

		bool update(WorkingUnitTO wuTO, bool doCommit);

		WorkingUnitTO find(int workingUnitID);

		WorkingUnitTO find(int workingUnitID, bool useTrans);

		int findMAXWUID();

		List<WorkingUnitTO> getWorkingUnits(WorkingUnitTO wuTO);

        List<WorkingUnitTO> getWUnits(string wUnits);
        List<WorkingUnitTO> getWUByName(string name);

		bool beginTransaction();

		void commitTransaction();

		void rollbackTransaction();

		IDbTransaction getTransaction();

		void setTransaction(IDbTransaction trans);

        void serialize(List<WorkingUnitTO> WorkingUnitsTO);

        List<WorkingUnitTO> getWorkingUnitsForOU(string orgUnitID); // Nenad 03. XI 2017. Load working units by selected organizational unit

        DataSet getWorkingUnits(string workingUnitID);

        DataSet getRootWorkingUnits(string workingUnitID);

        List<WorkingUnitTO> getRootWorkingUnitsList(string workigUnitID);

        List<WorkingUnitTO> getRootWU(string wuIDs);

        List<WorkingUnitTO> getChildWU(string parentID);

        Dictionary<int, WorkingUnitTO> getWUDictionary();

        int findMINWUID();

        int insert(WorkingUnitTO workingUnitTO, bool doCommit);

        List<WorkingUnitTO> getWorkingUnitsExact(WorkingUnitTO workingUnitTO);

        Dictionary<int, WorkingUnitTO> getWUDictionary(IDbTransaction trans);
        List<WorkingUnitTO> getAllWU();
    }
}
