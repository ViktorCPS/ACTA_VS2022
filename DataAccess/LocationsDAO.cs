using System;
using System.Collections;
using System.Collections.Generic;

using TransferObjects;
using Util;

namespace DataAccess
{
	/// <summary>
	/// LocationDAO interface is implemented by 
	/// database specific Locations DAO classes
	/// </summary>
	public interface LocationDAO
	{
		int insert(LocationTO locTO);

		bool delete(int locationID);

		bool update(LocationTO locTO);

		LocationTO find(int locationID);

		int findMAXLocID();

		List<LocationTO> getLocations(LocationTO locTO);

        Dictionary<int, LocationTO> getLocationsDict(LocationTO locTO);

        void serialize(List<LocationTO> LoactionTOList);

        System.Data.DataSet getLocations(string locationID);

        List<LocationTO> getLocationsForMap(int mapID);

        List<LocationTO> getRootLocations();

        List<LocationTO> getChildLocations(string parentID);
    }
}
