using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;

using Util;
using TransferObjects;

namespace SiemensDataAccess
{
    public abstract class SiemensReportDAO
    {
        public static bool threadSafe = false;

        public abstract bool TestDataSourceConnection();
        public abstract void CloseConnection();
        public abstract void CloseConnection(Object dbConnection);
        public abstract Object MakeNewDBConnection();
        public abstract void SetDBConnection(object dbConnection);

        public abstract int insertApplUser(ApplUserTO userTO, bool doCommit);
        public abstract int insertApplUserCategory(ApplUserCategoryTO categoryTO, bool doCommit);
        public abstract int insertApplUserXApplUserCategory(ApplUserXApplUserCategoryTO userXCatTO, bool doCommit);
        public abstract int insertApplUserXOrganizationalUnit(ApplUserXOrgUnitTO userXOrgUnitTO, bool doCommit);
        public abstract ApplUserLogTO insertApplUserLog(ApplUserLogTO userTO);
        public abstract int insertEmployeeData(SiemensEmployeeTO empTO, bool doCommit);
        public abstract int insertRegAddInfo(uint PK, int siPassID, bool doCommit);
        public abstract int insertKamioniAddInfo(int id, int siPassVechicleID, int siPassDriverID, bool doCommit);
        public abstract int insertVechicleLog(SiemensVechicleLogTO logTO, bool doCommit);
        public abstract bool updateApplUser(ApplUserTO userTO, bool doCommit);
        public abstract int updateApplUserLog(int loginID, string modifiedBy);
        public abstract bool updateEmployeeData(SiemensEmployeeTO emplTO, bool doCommit);
        public abstract bool setApplUserDefaultCategory(ApplUserXApplUserCategoryTO userXCatTO);
        public abstract bool deleteApplUserXApplUserCategory(string user_id, int categoryID, bool doCommit);
        public abstract bool deleteApplUserXOrganizationalUnit(string userID, int ouID, string purpose, bool doCommit);
        public abstract bool deleteEmployeeData(string emplID, bool doCommit);
        public abstract Dictionary<int, SiemensEmployeeTO> getEmployeeData(string emplNums, string groupIDs, bool filterVisitors, bool visitor, string cardTypes);
        public abstract List<int> getEmployeeDataIDList();
        public abstract Dictionary<int, List<uint>> getRegAddInfo(string PKs, int id);
        public abstract Dictionary<int, Dictionary<int, List<int>>> getKamioniAddInfo(string ids, int driverID, int truckID);
        public abstract List<SiemensVechicleLogTO> getVechiclePasses(DateTime from, DateTime to);
                
        public abstract bool beginTransaction();
        public abstract void commitTransaction();
        public abstract void rollbackTransaction();
        public abstract IDbTransaction getTransaction();
        public abstract void setTransaction(IDbTransaction trans);
        
        // Data Provider is found from conection string
        public static SiemensReportDAO getDAO()
        {
            try
            {
                string connectionString = "";
                int startIndex = -1;
                int endIndex = -1;
                string dataProvider = "";

                try
                {
                    // get connection string from .config file
                    connectionString = ConfigurationManager.AppSettings["connectionStringSiPassReport"];
                    if (connectionString == null || connectionString.Equals(""))
                    {
                        throw new Exception(Constants.connStringNotFound);
                    }

                    // encrypt connection string
                    if (connectionString.ToLower().StartsWith("data provider"))
                    {
                        // encrypt a string to a byte array.
                        byte[] buffer = Util.Misc.encrypt(connectionString);

                        string connStringCrypted = Convert.ToBase64String(buffer);

                        Util.Misc.configAdd("connectionStringSiPassReport", connStringCrypted);
                    }
                    else
                    {
                        try
                        {
                            byte[] buffer = Convert.FromBase64String(connectionString);
                            connectionString = Util.Misc.decrypt(buffer);
                        }
                        catch
                        {
                            connectionString = "";
                        }

                        if (connectionString.Trim().Equals(""))
                            return null;
                    }
                }
                catch
                {
                    throw new Exception(Constants.connStringNotFound);
                }

                startIndex = connectionString.ToLower().IndexOf("data provider");

                if (startIndex >= 0)
                {
                    endIndex = connectionString.IndexOf(";", startIndex);

                    if (endIndex >= startIndex)
                    {
                        // take data provider value
                        // data provider part of the connection string is like "data provider=mysql;" and we need "mysql"
                        // or string is like "data provider=sqlserver;" and we need "sqlserver"
                        startIndex = connectionString.IndexOf("=", startIndex);
                        if (startIndex >= 0)
                            dataProvider = connectionString.Substring(startIndex + 1, endIndex - startIndex - 1);
                    }
                    int start = connectionString.ToLower().IndexOf("database");
                    if (start > 0)
                    {
                        int end = endIndex = connectionString.IndexOf(";", start);
                        if (end >= start)
                        {
                            start = connectionString.IndexOf("=", start);
                        }
                    }
                }
                switch (dataProvider.ToLower())
                {
                    case "sqlserver":
                        {
                            MSSQLSiemensReportDAO mssqlDAOFactory = new MSSQLSiemensReportDAO();
                            if (mssqlDAOFactory.TestDataSourceConnection())
                            {
                                return mssqlDAOFactory;
                            }
                            else
                            {
                                return null; //new XMLDAOFactory();
                            }
                        }                    
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating database layer -> getDAOFactory: " + ex.Message);
            }
        }
    }
}
