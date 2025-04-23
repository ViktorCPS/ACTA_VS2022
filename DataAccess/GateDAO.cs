using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// LocationDAO interface is implemented by 
	/// database specific Locations DAO classes.
	/// </summary>
	public interface GateDAO
	{
		int insert(string name, string description, DateTime downloadStartTime, int downloadInterval, int downloadEraseCounter);

		bool delete(int gateID);

		bool update(int gateID, string name, string description, DateTime downloadStartTime, int downloadInterval, int downloadEraseCounter);

		bool beginTransaction();

		void commitTransaction();

		void rollbackTransaction();

		IDbTransaction getTransaction();

		void setTransaction(IDbTransaction trans);

		GateTO find(int gateID);

		List<GateTO> getGates(GateTO gateTO);

        List<GateTO> getGatesForLocation(int locationID);

        List<GateTO> getGatesGetGateTAProfile();

		GateTO findGetAccessProfile(string gateID);

        List<GateTO> getGatesWithGateTAProfile(string gateTimeaccessProfileId);

		bool updateGateTAProfile(string  gateID, string gateTAProfileID, bool doCommit);

        void serialize(List<GateTO> GateTOList);

        List<GateTO> getGatesForMap(int mapID);

        int insert(string name, string description, DateTime downloadStartTime, int downloadInterval, int downloadEraseCounter, bool doCommit);

        List<GateTO> getGatesForLocationEnabled(int locationID);
    }
}
