using System;
using System.Collections.Generic;
using System.Text;
using Util;
using Common;
using System.IO;
using TransferObjects;
using System.Collections;

namespace SiemensDataAccess
{
    /// <summary>
    /// DAO layer with Siemens data base
    /// </summary>
    public abstract class SiemensDAO
    {
        public static bool threadSafe = false;
        public static string dataBase = "";
        private static string auditTrailTableName = "";
        private static string siPassUser = "";        
        
        public abstract bool TestDataSourceConnection();
        public abstract void CloseConnection();
        public abstract void CloseConnection(Object dbConnection);
        public abstract Object MakeNewDBConnection();
        public abstract void SetDBConnection(object dbConnection);
        public abstract Dictionary<int, SiemensEmployeeTO> getAllEmployees();
        public abstract Dictionary<int, SiemensEmployeeTO> getEmployeesNonVisitors();
        public abstract List<WorkingUnitTO> getAllWorkingUnits();
        public abstract List<SiemensLogTO> getNewLogs(string pointNames, int diffLog, Dictionary<int, PointTO> points, Dictionary<int, PointTO> truckPointDict);
        public abstract List<LogTO> getDiferenceLogs(string pointNamesAllLogs, ArrayList points);        
        public abstract ArrayList deserializeMapping(string filePath);
        public abstract int getDiffLogID(string filePath);
        public abstract bool setDiffLogID(int logID, string filePath);
        public abstract ArrayList getNewPoints();
        public abstract bool getCardStatus(string emplNum);
        public abstract bool getCardStatus(int emplID);
        public abstract Dictionary<int, bool> getCardStatuses();
        public abstract bool serialize(ArrayList PointTOList, string filePath);
        
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
                    return SiemensDAO.auditTrailTableName; 
            }
        }

        public static string SiPassUser
        {
            get 
            {
                if (siPassUser.Equals(""))
                    return Constants.SiemensUser;
                else
                    return SiemensDAO.siPassUser; 
            }
            
        }

        // no Data Provider parameter
        // Data Provider is found from conection string
        public static SiemensDAO getDAO()
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
                    if (File.Exists(Constants.SiPassConnPath))
                    {
                        StreamReader reader = new StreamReader(Constants.SiPassConnPath);
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

                        if (File.Exists(Constants.SiPassConnPath))
                        {
                            StreamWriter writer = new StreamWriter(Constants.SiPassConnPath);
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
                            MSSQLSiemensDAO mssqlDAOFactory = new MSSQLSiemensDAO();
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
