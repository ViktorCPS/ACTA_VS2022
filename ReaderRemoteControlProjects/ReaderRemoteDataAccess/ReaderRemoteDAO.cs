using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Util;
using TransferObjects;

namespace ReaderRemoteDataAccess
{
   public abstract class ReaderRemoteDAO
    {
        public abstract bool TestDataSourceConnection();
        public abstract void CloseConnection();
        public abstract void CloseConnection(Object dbConnection);
        public abstract Object MakeNewDBConnection();
        public abstract int Count(int employeeID, DateTime from, DateTime to);
        public abstract int Save(OnlineMealsUsedTO onlineMealUsedTO, bool doCommit, int transferFlag);
        public abstract List<OnlineMealsUsedTO> GetMealsToTransfer();
        public abstract bool UpdateToTransfered(string transIDs, bool doCommit);
        public abstract bool DeletePreviousDay(bool doCommit);
        public abstract void setDBConnection(object dbConnection);

        // no Data Provider parameter
        // Data Provider is found from conection string
        public static ReaderRemoteDAO getDAO()
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
                    connectionString = ConfigurationManager.AppSettings["restaurantConnectionString"];
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

                        Util.Misc.configAdd("restaurantConnectionString", connStringCrypted);
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
                            MSSQLReaderRemoteDAO mssqlDAOFactory = new MSSQLReaderRemoteDAO();
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
