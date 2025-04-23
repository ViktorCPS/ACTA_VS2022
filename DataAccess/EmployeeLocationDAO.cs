using System;
using System.Collections;
using System.Collections.Generic;
using TransferObjects;
using System.Data;

namespace DataAccess
{
	/// <summary>
	/// Summary description for EmployeeLocationDAO.
	/// </summary>
	public interface EmployeeLocationDAO
	{		
		int insert(int employeeID, int readerID, int antenna, int passTypeID, DateTime eventTime, int locationID, bool doCommit);

		bool delete(int employeeID);

		bool update(int employeeID, int readerID, int antenna, int passTypeID, DateTime eventTime, int locationID);

		EmployeeLocationTO find(int employeeID);

		List<EmployeeLocationTO> getEmployeeLocations(EmployeeLocationTO emplLocTO, string locationID, string wUnits);

        List<EmployeeLocationTO> getEmployeeLocations(EmployeeLocationTO emplLocTO, string locationID, string wUnits,DateTime fromDate,DateTime toDate);

		List<EmployeeLocationTO> getEmployeeLocationsAll(EmployeeLocationTO emplLocTO);

        List<EmployeeLocationTO> getEmployeeLocationsMittal(string workingUnitsOther);

        List<EmployeeLocationTO> getEmployeeLocationsOther(string workingUnitsOther);

		int getTotalMittalOut(string workingUnitsOther);

		int getTotalOtherOut(string workingUnitsOther);

        List<EmployeeLocationTO> getEmployeeLocationsMittalDet(string locationID, string workingUnitsOther);

        List<EmployeeLocationTO> getEmployeeLocationsOtherDet(string locationID, string workingUnitsOther);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        void SetDBConnection(Object dbConnection);

        List<EmployeeLocationTO> getEmployeeLocationsIn(string wuID);

        List<EmployeeLocationTO> getEmployeeLocationsInByWU(string locationID);
	}
}
