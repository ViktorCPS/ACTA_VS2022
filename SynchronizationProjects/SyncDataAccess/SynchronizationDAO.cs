using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransferObjects;
using System.IO;
using Util;
using System.Configuration;

namespace SyncDataAccess
{
    public abstract class SynchronizationDAO
    {
        public abstract bool TestDataSourceConnection();
        public abstract void CloseConnection();
        public abstract void CloseConnection(Object dbConnection);
        public abstract Object MakeNewDBConnection();
        public abstract List<SyncEmployeeTO> getDifEmployees();
        public abstract List<SyncFinancialStructureTO> getDifFinancialStructures();
        public abstract List<SyncCostCenterTO> getDifCostCenters();
        public abstract List<SyncOrganizationalStructureTO> getDifOrganizationalStructures();
        public abstract List<SyncOrganizationalStructureTO> getDifOrganizationalStructuresParental();
        public abstract List<SyncOrganizationalStructureTO> getDifOrganizationalStructuresNOTParental();
        public abstract List<SyncResponsibilityTO> getDifResponsibility();
        public abstract List<SyncAnnualLeaveRecalcTO> getDifAnnualLeaveRecalc(int emplID);
        public abstract List<SyncEmployeePositionTO> getDifEmployeePositions();
        public abstract bool delSyncEmployee(SyncEmployeeTO syncEmployee);
        public abstract bool delSyncEmployeePosition(SyncEmployeePositionTO syncEmployeePosition);
        public abstract bool delSyncResponsibility(SyncResponsibilityTO syncEmployee);
        public abstract bool delSyncFinancialStructure(SyncFinancialStructureTO syncFinancialStructure);
        public abstract bool delSyncCostCenters(SyncCostCenterTO syncCC);
        public abstract bool delSyncOrganizationalStructure(SyncOrganizationalStructureTO syncOrganizationalStructure);
        public abstract bool delSyncAnnualLeaveRecalc(SyncAnnualLeaveRecalcTO syncRecalc);
        public abstract int insertSyncBufferData(SyncBufferDataTO syncBuffer);
        public abstract void setDBConnection(object dbConnection);
        public static string dataBase = "";
        private static string tableName = "";
        private static string UserName = "";

        public string DataBase
        {
            get
            {
                if (dataBase.Equals(""))
                    return Constants.SiemensDataBase;
                else
                    return dataBase;
            }
        }

        public static string AuditTrailTableName
        {
            get
            {
                if (tableName.Equals(""))
                    return Constants.SiemensTableName;
                else
                    return SynchronizationDAO.tableName;
            }
        }

        public static string User
        {
            get
            {
                if (UserName.Equals(""))
                    return Constants.SiemensUser;
                else
                    return SynchronizationDAO.UserName;
            }

        }

        // no Data Provider parameter
        // Data Provider is found from conection string
        public static SynchronizationDAO getDAO()
        {
            try
            {
                string connectionString = "";
                int startIndex = -1;
                int endIndex = -1;
                string dataProvider = "";

                string XMLAlternative = "NO";


                try
                {
                    connectionString = ConfigurationManager.AppSettings["syncConnectionString"];
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

                        Util.Misc.configAdd("syncConnectionString", connStringCrypted);
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
                }
                switch (dataProvider.ToLower())
                {
                    case "sqlserver":
                        {
                            MSSQLSynchronizationDAO mssqlDAOFactory = new MSSQLSynchronizationDAO();                           
                            if (mssqlDAOFactory.TestDataSourceConnection() || (XMLAlternative == "NO"))
                            {
                                return mssqlDAOFactory;
                            }
                            else
                            {
                                return null; //new XMLDAOFactory();
                            }
                        }
                    case "oracle":
                    //return new OracleDAOFactory();
                    case "mysql":
                        //{
                        //    //MySQLSiemensDAO mysqlDAOFactory = new MySQLSiemensDAO();

                        //    //if (mysqlDAOFactory.TestDataSourceConnection() || (XMLAlternative == "NO"))
                        //    //{
                        //    //    return mysqlDAOFactory;
                        //    //}
                        //    //else
                        //    //{
                        //    //    return null;//new XMLDAOFactory();
                        //    //}
                        //}
                    case "xml":
                    //return new XMLDAOFactory();
                    case "":
                    //return getDAOFactory(Int32.Parse(ConfigurationManager.AppSettings["DataProvider"]));
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
