using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;

using Util;
using Common;
using TransferObjects;

namespace SiemensDataAccess
{
    public abstract class BrezaDAO
    {
        public static bool threadSafe = false;

        public abstract bool TestDataSourceConnection();
        public abstract void CloseConnection();
        public abstract void CloseConnection(Object dbConnection);
        public abstract Object MakeNewDBConnection();
        public abstract void SetDBConnection(object dbConnection);
        public abstract uint insert(SiemensLogTO log, bool doCommit);
        public abstract int insertTruckLog(SiemensTruckLogTO log, bool doCommit);
        public abstract bool insertEmployeeImage(SiemensEmployeeImageTO imgTO, bool doCommit);
        public abstract bool insertEmp(SiemensEmpTO empTO, bool doCommit);
        public abstract bool updateEmployeeToProcessed(uint pk, bool doCommit);
        public abstract bool updateDepartmentToProcessed(uint pk, bool doCommit);        
        public abstract Dictionary<string, SiemensEmployeeTO> getEmployees();
        public abstract List<SiemensEmployeeTO> getEmployeesUnprocessed();        
        public abstract List<SiemensDepartmentTO> getDepartmentsUnprocessed();
        public abstract int getParentDepartment(int id);
        public abstract int getLevelDepartment(int id);
        public abstract string getNameDepartment(int id);
        public abstract List<SiemensLogTO> getPasses(string id, string direction, string created, string remark, DateTime from, DateTime to);
        public abstract List<SiemensTruckLogTO> getTruckPasses(string truckReg, string buyer, string forwarder, string order, string driverLastName, string driverFirstName, string direction, string created, DateTime from, DateTime to);
        public abstract List<string> getEmplImages();
        public abstract Dictionary<string, string> getEmpls();
        public abstract DateTime getMaxDatePassTransfered();
        public abstract DateTime getMaxDateTruckPassTransfered();
        public abstract ArrayList deserializeDepartments(string filePath);
        public abstract bool serializeDepartments(ArrayList DeptTOList, string filePath);
        public abstract bool beginTransaction();
        public abstract void commitTransaction();
        public abstract void rollbackTransaction();
        public abstract IDbTransaction getTransaction();
        public abstract void setTransaction(IDbTransaction trans);
        
        public static string dataBase = "";
        private static string auditTrailTableName = "";
        private static string siPassUser = "";

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
                if (auditTrailTableName.Equals(""))
                    return Constants.SiemensTableName;
                else
                    return BrezaDAO.auditTrailTableName;
            }
        }

        public static string SiPassUser
        {
            get
            {
                if (siPassUser.Equals(""))
                    return Constants.SiemensUser;
                else
                    return BrezaDAO.siPassUser;
            }

        }

        // no Data Provider parameter
        // Data Provider is found from conection string
        public static BrezaDAO getDAO()
        {
            try
            {
                string connectionString = "";
                int startIndex = -1;
                int endIndex = -1;
                string dataProvider = "";

                string XMLAlternative = "NO";
                //try
                //{
                //    XMLAlternative = (ConfigurationManager.AppSettings["XMLAlternative"]).ToUpper();
                //}
                //catch { }

                try
                {
                    if (File.Exists(Constants.SiPassConnPathBreza))
                    {
                        StreamReader reader = new StreamReader(Constants.SiPassConnPathBreza);
                        connectionString = reader.ReadLine();
                        reader.Close();
                    }
                    if (connectionString == null || connectionString.Equals(""))
                    {
                        throw new Exception(Constants.connStringNotFound);
                    }

                    // encrypt connection string
                    if (connectionString.ToLower().StartsWith("data provider"))
                    {
                        //// encrypt a string to a byte array.
                        //byte[] buffer = Util.Misc.encrypt(connectionString);

                        //string connStringCrypted = Convert.ToBase64String(buffer);

                        if (File.Exists(Constants.SiPassConnPathBreza))
                        {
                            StreamWriter writer = new StreamWriter(Constants.SiPassConnPathBreza);
                            writer.WriteLine(connectionString);
                            writer.Close();
                        }
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
                            dataBase = connectionString.Substring(start + 1, end - start - 1);
                        }
                    }

                    start = connectionString.ToLower().IndexOf("table");
                    if (start > 0)
                    {
                        int end = endIndex = connectionString.IndexOf(";", start);
                        if (end >= start)
                        {
                            start = connectionString.IndexOf("=", start);
                            auditTrailTableName = connectionString.Substring(start + 1, end - start - 1);
                        }
                    }

                    start = connectionString.ToLower().IndexOf("sipassuser");
                    if (start > 0)
                    {
                        int end = endIndex = connectionString.IndexOf(";", start);
                        if (end >= start)
                        {
                            start = connectionString.IndexOf("=", start);
                            siPassUser = connectionString.Substring(start + 1, end - start - 1);
                        }
                    }


                }
                switch (dataProvider.ToLower())
                {
                    case "sqlserver":
                        {
                            MSSQLBrezaDAO mssqlDAOFactory = new MSSQLBrezaDAO();
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
                    //MySQLSiemensDAO mysqlDAOFactory = new MySQLSiemensDAO();

                    //if (mysqlDAOFactory.TestDataSourceConnection() || (XMLAlternative == "NO"))
                    //{
                    //    return mysqlDAOFactory;
                    //}
                    //else
                    //{
                    //    return null;//new XMLDAOFactory();
                    //}
                    // }
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

