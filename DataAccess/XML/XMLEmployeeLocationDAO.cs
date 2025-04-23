using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using TransferObjects;

namespace DataAccess
{
    /// <summary>
    /// Summary description for XMLEmployeeLocationDAO.
    /// </summary>
    public class XMLEmployeeLocationDAO : EmployeeLocationDAO
    {

        private static DataSet cachedEmployeesTO = new DataSet();
        private const string tableName = "AllEmployeeLocations";
        private const string resultTableName = "resultRows";

        public XMLEmployeeLocationDAO()
        {
        }

        #region EmployeeLocationDAO Members

        public void SetDBConnection(Object dbConnection)
        {
        }

        public int insert(int employeeID, int readerID, int antenna, int passTypeID, DateTime eventTime, int locationID, bool doCommit)
        {
            int rowsAffected = 0;
            return rowsAffected;
        }

        public bool update(int employeeID, int readerID, int antenna, int passTypeID, DateTime eventTime, int locationID)
        {
            bool isUpdated = false;
            return isUpdated;
        }

        public bool delete(int employeeID)
        {
            bool isDeleted = false;
            return isDeleted;
        }

        public EmployeeLocationTO find(int employeeID)
        {
            EmployeeLocationTO emplLocation = new EmployeeLocationTO();
            return emplLocation;
        }

        public List<EmployeeLocationTO> getEmployeeLocations(EmployeeLocationTO emplLocTO, string locationID, string wUnits)
        {
            List<EmployeeLocationTO> emplLocationList = new List<EmployeeLocationTO>();
            return emplLocationList;
        }

        public List<EmployeeLocationTO> getEmployeeLocations(EmployeeLocationTO emplLocTO, string locationID, string wUnits, DateTime fromDate, DateTime toDate)
        {
            List<EmployeeLocationTO> emplLocationList = new List<EmployeeLocationTO>();
            return emplLocationList;
        }

        public List<EmployeeLocationTO> getEmployeeLocationsAll(EmployeeLocationTO emplLocTO)
        {
            List<EmployeeLocationTO> emplLocationList = new List<EmployeeLocationTO>();
            return emplLocationList;
        }

        public DataTable getEmployeeLocationsMittalOthers(ArrayList workingUnitsOther)
        {
            return null;
        }

        public List<EmployeeLocationTO> getEmployeeLocationsMittal(string workingUnitsOther)
        {
            return null;
        }

        public List<EmployeeLocationTO> getEmployeeLocationsOther(string workingUnitsOther)
        {
            return null;
        }

        public int getTotalMittalOut(string workingUnitsOther)
        {
            return 0;
        }

        public int getTotalOtherOut(string workingUnitsOther)
        {
            return 0;
        }

        public List<EmployeeLocationTO> getEmployeeLocationsMittalDet(string locationID, string workingUnitsOther)
        {
            return null;
        }

        public List<EmployeeLocationTO> getEmployeeLocationsOtherDet(string locationID, string workingUnitsOther)
        {
            return null;
        }

        public List<EmployeeLocationTO> getEmployeeLocationsIn(string wuID)
        {
            return null;
        }

        public List<EmployeeLocationTO> getEmployeeLocationsInByWU(string locationID)
        {
            return null;
        }

        public bool beginTransaction()
        {
            return false;
        }

        public void commitTransaction()
        {
        }

        public void rollbackTransaction()
        {
        }

        public void setTransaction(IDbTransaction trans)
        {
        }

        public IDbTransaction getTransaction()
        {
            return null;
        }

        #endregion

    }
}
