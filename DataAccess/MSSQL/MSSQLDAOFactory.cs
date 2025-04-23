using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;

using TransferObjects;
using Util;
using DataAccess.MSSQL;

namespace DataAccess
{
    /// <summary>
    /// MSSQLDAOFactory.
    /// </summary>
    public class MSSQLDAOFactory : DAOFactory
    {
        public static readonly string Driver = "";
        private static string ConnectionString = "";
        // thread-safe locker
        private static object locker = new object();
        public static SqlConnection connection;
        private SqlConnection tsConnection = null;

        /// <summary>
        /// Finalizer
        /// </summary>
        ~MSSQLDAOFactory()
        {
            Dispose();
        }

        private void Dispose()
        {
            if (tsConnection != null)
            {
                CloseConnection(tsConnection);
            }
        }

        public MSSQLDAOFactory()
        {
            lock (locker)
            {
                // Decrypt user name and password
                //Crypt cr = new Crypt();

                // Decrypt connection string
                ConnectionString = ConfigurationManager.AppSettings["connectionString"];

                byte[] buffer = Convert.FromBase64String(ConnectionString);
                ConnectionString = Util.Misc.decrypt(buffer);

                // ConnectionString contains data provader and it should be ejected from connection string
                int startIndex = -1;

                startIndex = ConnectionString.ToLower().IndexOf("server=");

                if (startIndex >= 0)
                {
                    ConnectionString = ConnectionString.Substring(startIndex);
                }

                //ConnectionString = cr.DecryptConnectionString(ConnectionString);
            }
        }
        public override bool TestDBConnectionDummySelect(Object dbConnection)
        {
            try
            {
                DataSet dataSet = new DataSet();
                SqlConnection conn = dbConnection as SqlConnection;
                string cmdText = "SELECT GetDate()";

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataSet, "Test");
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public static SqlConnection getConnection()
        {
            lock (locker)
            {
                try
                {
                    if (connection == null)
                    {
                        // Set connection for the first time
                        connection = new SqlConnection(ConnectionString);
                        connection.Open();
                        return connection;
                    }
                    else
                    {
                        // TODO: Check if connection is closed or opened
                        // Connection already established
                        return connection;
                    }
                }
                catch (InvalidOperationException ioex)
                {
                    connection = null;
                    throw ioex;
                }
                catch (SqlException sqlex)
                {
                    connection = null;
                    throw sqlex;
                }
                catch (Exception ex)
                {
                    connection = null;
                    throw ex;
                }
            }
        }

        public override void CloseConnection()
        {
            lock (locker)
            {
                try
                {
                    if (connection != null) connection.Close();
                }
                catch { }
                finally
                {
                    connection = null;
                }
            }
        }

        public override void CloseConnection(Object dbConnection)
        {
            try
            {
                if (dbConnection != null)
                {
                    SqlConnection conn = dbConnection as SqlConnection;
                    if (conn != null)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            catch { }
        }

        public override Object MakeNewDBConnection()
        {
            lock (locker)
            {
                SqlConnection sqlConnection = null;
                try
                {
                    sqlConnection = new SqlConnection(ConnectionString);
                    sqlConnection.Open();
                    return sqlConnection;
                }
                catch (InvalidOperationException ioex)
                {
                    throw ioex;
                }
                catch (SqlException sqlex)
                {
                    throw sqlex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public override bool TestDataSourceConnection()
        {
            bool isConnected = false;

            try
            {
                SqlConnection testConn = getConnection();
                if (threadSafe)
                {
                    tsConnection = MakeNewDBConnection() as SqlConnection;
                }
                if (!threadSafe)
                {
                    isConnected = (testConn.State == ConnectionState.Open);
                }
                else
                {
                    isConnected = (((tsConnection != null) && (tsConnection.State == ConnectionState.Open)) &&
                                   (testConn.State == ConnectionState.Open));
                }
            }
            catch
            {
                return isConnected;
            }

            return isConnected;
        }

        public override bool TestDataSourceConnection(Object dbConnection)
        {
            bool isConnected = false;

            try
            {
                SqlConnection testConn = (SqlConnection)dbConnection;
                if (threadSafe)
                {
                    tsConnection = MakeNewDBConnection() as SqlConnection;
                }
                if (!threadSafe)
                {
                    isConnected = (testConn.State == ConnectionState.Open);
                }
                else
                {
                    isConnected = (((tsConnection != null) && (tsConnection.State == ConnectionState.Open)) &&
                                   (testConn.State == ConnectionState.Open));
                }
            }
            catch
            {
                return isConnected;
            }

            return isConnected;
        }

        public override int getDBConnectedHosts()
        {
            try
            {
                int hosts = -1;

                string cmdText = "USE master IF IS_SRVROLEMEMBER ('serveradmin','actamgr') = 1 "
                + "SELECT  COUNT(*) as conn_hosts FROM sysprocesses WHERE sid = SUSER_SID('actamgr') "
                + "ELSE SELECT -1 as conn_hosts USE ACTA";

                SqlCommand cmd = new SqlCommand(cmdText, connection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Logins");
                DataTable table = dataSet.Tables["Logins"];

                if (table.Rows.Count == 1)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        hosts = Int32.Parse(row["conn_hosts"].ToString().Trim());
                    }
                }

                return hosts;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }
        }

        public override string getDBServerName()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select @@SERVERNAME as name", connection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "server_name");
                DataTable table = dataSet.Tables["server_name"];

                if (table.Rows.Count == 1)
                {
                    string serverName = null;
                    foreach (DataRow row in table.Rows)
                    {
                        serverName = (row["name"].ToString());
                    }
                    return serverName;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: getDBServerName " + ex.Message);
            }
        }

        public override string getDBName()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT DB_NAME() AS name", connection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "db_name");
                DataTable table = dataSet.Tables["db_name"];

                if (table.Rows.Count == 1)
                {
                    string dbName = null;
                    foreach (DataRow row in table.Rows)
                    {
                        dbName = (row["name"].ToString());
                    }
                    return dbName;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: getDBName " + ex.Message);
            }
        }

        public override string getDBServerPort()
        {
            return null;
        }

        public override SyncWithNavDAO getSyncWithNavDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MSSQLSyncWithNavDAO(tsConnection);
                }
                else
                {
                    return new MSSQLSyncWithNavDAO();
                }
            }
            else
            {
                return new MSSQLSyncWithNavDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeeDAO getEmployeeDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MSSQLEmployeeDAO(tsConnection);
                }
                else
                {
                    return new MSSQLEmployeeDAO();
                }
            }
            else
            {
                return new MSSQLEmployeeDAO((SqlConnection)dbConnection);
            }
        }

        public override DocumentsDAO getDocumentsDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MSSQLDocumentsDAO(tsConnection);
                }
                else
                {
                    return new MSSQLDocumentsDAO();
                }
            }
            else
            {
                return new MSSQLDocumentsDAO((SqlConnection)dbConnection);
            }
        }



        public override LocationDAO getLocationDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MSSQLLocationDAO(tsConnection);
                }
                else
                {
                    return new MSSQLLocationDAO();
                }
            }
            else
            {
                return new MSSQLLocationDAO((SqlConnection)dbConnection);
            }
        }

        public override WorkingUnitsDAO getWorkingUnitsDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MSSQLWorkingUnitsDAO(tsConnection);
                }
                else
                {
                    return new MSSQLWorkingUnitsDAO();
                }
            }
            else
            {
                return new MSSQLWorkingUnitsDAO((SqlConnection)dbConnection);
            }
        }

        public override TimeSchemaIntervalLibraryDAO getTimeSchemaIntervalLibraryDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MSSQLTimeSchemaIntervalLibraryDAO(tsConnection);
                }
                else
                {
                    return new MSSQLTimeSchemaIntervalLibraryDAO();
                }
            }
            else
            {
                return new MSSQLTimeSchemaIntervalLibraryDAO((SqlConnection)dbConnection);
            }
        }
        public override TimeSchemaIntervalLibraryDtlDAO getTimeSchemaIntervalLibraryDtlDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MSSQLTimeSchemaIntervalLibraryDtlDAO(tsConnection);
                }
                else
                {
                    return new MSSQLTimeSchemaIntervalLibraryDtlDAO();
                }
            }
            else
            {
                return new MSSQLTimeSchemaIntervalLibraryDtlDAO((SqlConnection)dbConnection);
            }
        }
        public override PassTypeDAO getPassTypeDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MSSQLPassTypeDAO(tsConnection);
                }
                else
                {
                    return new MSSQLPassTypeDAO();
                }
            }
            else
            {
                return new MSSQLPassTypeDAO((SqlConnection)dbConnection);
            }
        }

        public override AddressDAO getAddressDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLAddressDAO();
            }
            else
            {
                return new MSSQLAddressDAO((SqlConnection)dbConnection);
            }
        }

        public override ReaderDAO getReaderDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLReaderDAO();
            }
            else
            {
                return new MSSQLReaderDAO((SqlConnection)dbConnection);
            }
        }

        public override TagDAO getTagDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLTagDAO();
            }
            else
            {
                return new MSSQLTagDAO((SqlConnection)dbConnection);
            }
        }

        public override PassDAO getPassDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MSSQLPassDAO(tsConnection);
                }
                else
                {
                    return new MSSQLPassDAO();
                }
            }
            else
            {
                return new MSSQLPassDAO((SqlConnection)dbConnection);
            }
        }

        public override PassHistDAO getPassHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLPassHistDAO();
            }
            else
            {
                return new MSSQLPassHistDAO((SqlConnection)dbConnection);
            }
        }
        public override VisitDAO getVisitDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLVisitDAO();
            }
            else
            {
                return new MSSQLVisitDAO((SqlConnection)dbConnection);
            }
        }

        public override CitizenDAO getCitizenDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLCitizenDAO();
            }
            else
            {
                return new MSSQLCitizenDAO((SqlConnection)dbConnection);
            }
        }

        public override LogDAO getLogDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLLogDAO();
            }
            else
            {
                return new MSSQLLogDAO((SqlConnection)dbConnection);
            }
        }

        public override IOPairDAO getIOPairDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MSSQLIOPairDAO(tsConnection);
                }
                else
                {
                    return new MSSQLIOPairDAO();
                }
            }
            else
            {
                return new MSSQLIOPairDAO((SqlConnection)dbConnection);
            }
        }

        public override WorkTimeSchemaDAO getWorkTimeSchemaDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MSSQLWorkTimeSchemaDAO(tsConnection);
                }
                else
                {
                    return new MSSQLWorkTimeSchemaDAO();
                }
            }
            else
            {
                return new MSSQLWorkTimeSchemaDAO((SqlConnection)dbConnection);
            }
        }

        public override WorkingGroupDAO getWorkingGroupDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLWorkingGroupDAO();
            }
            else
            {
                return new MSSQLWorkingGroupDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeeTimeScheduleDAO getEmployeeTimeScheduleDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MSSQLEmployeeTimeSchedule(tsConnection);
                }
                else
                {
                    return new MSSQLEmployeeTimeSchedule();
                }
            }
            else
            {
                return new MSSQLEmployeeTimeSchedule((SqlConnection)dbConnection);
            }
        }

        public override EmployeeAbsenceDAO getEmployeeAbsenceDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeAbsenceDAO();
            }
            else
            {
                return new MSSQLEmployeeAbsenceDAO((SqlConnection)dbConnection);
            }
        }

        public override RuleDAO getRuleDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLRuleDAO();
            }
            else
            {
                return new MSSQLRuleDAO((SqlConnection)dbConnection);
            }
        }


        public override EmployeeLocationDAO getEmployeeLocationDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeLocationDAO();
            }
            else
            {
                return new MSSQLEmployeeLocationDAO((SqlConnection)dbConnection);
            }
        }

        public override GateDAO getGateDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLGateDAO();
            }
            else
            {
                return new MSSQLGateDAO((SqlConnection)dbConnection);
            }
        }

        public override ApplUsersXWUDAO getApplUsersXWUDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MSSQLApplUsersXWUDAO(tsConnection);
                }
                else
                {
                    return new MSSQLApplUsersXWUDAO();
                }
            }
            else
            {
                return new MSSQLApplUsersXWUDAO((SqlConnection)dbConnection);
            }
        }

        public override ApplUserDAO getApplUserDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MSSQLApplUserDAO(tsConnection);
                }
                else
                {
                    return new MSSQLApplUserDAO();
                }
            }
            else
            {
                return new MSSQLApplUserDAO((SqlConnection)dbConnection);
            }
        }

        public override ApplUsersLogDAO getApplUsersLogDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLApplUsersLogDAO();
            }
            else
            {
                return new MSSQLApplUsersLogDAO((SqlConnection)dbConnection);
            }
        }


        public override ApplRoleDAO getApplRoleDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLApplRoleDAO();
            }
            else
            {
                return new MSSQLApplRoleDAO((SqlConnection)dbConnection);
            }
        }

        public override ApplMenuItemDAO getApplMenuItemDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLApplMenuItemDAO();
            }
            else
            {
                return new MSSQLApplMenuItemDAO((SqlConnection)dbConnection);
            }
        }

        public override IOPairProcessedDAO getIOPairProcessedDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLIOPairProcessedDAO();
            }
            else
            {
                return new MSSQLIOPairProcessedDAO((SqlConnection)dbConnection);
            }
        }

        public override ApplUsersXRoleDAO getApplUsersXRoleDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLApplUsersXRoleDAO();
            }
            else
            {
                return new MSSQLApplUsersXRoleDAO((SqlConnection)dbConnection);
            }
        }

        public override ExitPermissionDAO getExitPermissionsDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLExitPermissionDAO();
            }
            else
            {
                return new MSSQLExitPermissionDAO((SqlConnection)dbConnection);
            }
        }

        public override HolidayDAO getHolidayDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLHolidayDAO();
            }
            else
            {
                return new MSSQLHolidayDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeeGroupAccessControlDAO getEmployeeGroupAccessControlDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeGroupAccessControlDAO();
            }
            else
            {
                return new MSSQLEmployeeGroupAccessControlDAO((SqlConnection)dbConnection);
            }
        }

        public override TimeAccessProfileDAO getTimeAccessProfileDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLTimeAccessProfileDAO();
            }
            else
            {
                return new MSSQLTimeAccessProfileDAO((SqlConnection)dbConnection);
            }
        }

        public override TimeAccessProfileDtlDAO getTimeAccessProfileDtlDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLTimeAccessProfileDtlDAO();
            }
            else
            {
                return new MSSQLTimeAccessProfileDtlDAO((SqlConnection)dbConnection);
            }
        }

        public override GateTimeAccessProfileDAO getGateTimeAccessProfileDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLGateTimeAccessProfileDAO();
            }
            else
            {
                return new MSSQLGateTimeAccessProfileDAO((SqlConnection)dbConnection);
            }
        }

        public override AccessGroupXGateDAO getAccessGroupXGateDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLAccessGroupXGateDAO();
            }
            else
            {
                return new MSSQLAccessGroupXGateDAO((SqlConnection)dbConnection);
            }
        }

        public override MealsTypeScheduleDAO getMealsTypeScheduleDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMealsTypeScheduleDAO();
            }
            else
            {
                return new MSSQLMealsTypeScheduleDAO((SqlConnection)dbConnection);
            }
        }

        public override ExtraHourDAO getExtraHourDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLExtraHourDAO();
            }
            else
            {
                return new MSSQLExtraHourDAO((SqlConnection)dbConnection);
            }
        }

        public override ExtraHourUsedDAO getExtraHourUsedDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLExtraHourUsedDAO();
            }
            else
            {
                return new MSSQLExtraHourUsedDAO((SqlConnection)dbConnection);
            }
        }

        public override TimeSchemaPauseDAO getTimeSchemaPauseDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLTimeSchemaPauseDAO();
            }
            else
            {
                return new MSSQLTimeSchemaPauseDAO((SqlConnection)dbConnection);
            }
        }

        public override AccessControlFileDAO getAccessControlFileDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLAccessControlFileDAO();
            }
            else
            {
                return new MSSQLAccessControlFileDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeeImageFileDAO getEmployeeImageFileDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeImageFileDAO();
            }
            else
            {
                return new MSSQLEmployeeImageFileDAO((SqlConnection)dbConnection);
            }
        }

        public override VisitorDocFileDAO getVisitorDocFileDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLVisitorDocFileDAO();
            }
            else
            {
                return new MSSQLVisitorDocFileDAO((SqlConnection)dbConnection);
            }
        }

        public override CameraSnapshotFileDAO getCameraSnapshotFileDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLCameraSnapshotFileDAO();
            }
            else
            {
                return new MSSQLCameraSnapshotFileDAO((SqlConnection)dbConnection);
            }
        }

        public override CameraDAO getCameraDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLCameraDAO();
            }
            else
            {
                return new MSSQLCameraDAO((SqlConnection)dbConnection);
            }
        }

        public override CamerasXReadersDAO getCamerasXReadersDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLCamerasXReadersDAO();
            }
            else
            {
                return new MSSQLCamerasXReadersDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeeGroupsTimeScheduleDAO getEmployeeGroupsTimeScheduleDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeGroupsTimeScheduleDAO();
            }
            else
            {
                return new MSSQLEmployeeGroupsTimeScheduleDAO((SqlConnection)dbConnection);
            }
        }

        public override GateSyncDAO getGateSyncDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLGateSyncDAO();
            }
            else
            {
                return new MSSQLGateSyncDAO((SqlConnection)dbConnection);
            }
        }

        public override LicenceDAO getLicenceDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLLicenceDAO();
            }
            else
            {
                return new MSSQLLicenceDAO((SqlConnection)dbConnection);
            }
        }

        public override MealTypeDAO getMealTypeDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMealTypeDAO();
            }
            else
            {
                return new MSSQLMealTypeDAO((SqlConnection)dbConnection);
            }
        }
        public override MealAssignedDAO getMealAssignedDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMealAssignedDAO();
            }
            else
            {
                return new MSSQLMealAssignedDAO((SqlConnection)dbConnection);
            }
        }
        public override MealsPointDAO getMealsPointDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMealsPointDAO();
            }
            else
            {
                return new MSSQLMealsPointDAO((SqlConnection)dbConnection);
            }
        }

        //  06.02.2020. BOJAN
        public override MachineDAO getMachineDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMachineDAO();
            }
            else
            {
                return new MSSQLMachineDAO((SqlConnection)dbConnection);
            }
        }

        public override OnlineMealsPointsDAO getOnlineMealsPointDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLOnlineMealsPointsDAO();
            }
            else
            {
                return new MSSQLOnlineMealsPointsDAO((SqlConnection)dbConnection);
            }
        }
        public override OnlineMealsRestaurantDAO getOnlineMealsRestaurantDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLOnlineMealsRestaurantDAO();
            }
            else
            {
                return new MSSQLOnlineMealsRestaurantDAO((SqlConnection)dbConnection);
            }
        }
        public override OnlineMealsTypesDAO getOnlineMealsTypesDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLOnlineMealsTypesDAO();
            }
            else
            {
                return new MSSQLOnlineMealsTypesDAO((SqlConnection)dbConnection);
            }
        }
        public override OnlineMealsUsedDAO getOnlineMealsUsedDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLOnlineMealsUsedDAO();
            }
            else
            {
                return new MSSQLOnlineMealsUsedDAO((SqlConnection)dbConnection);
            }
        }
        public override OnlineMealsUsedDailyDAO getOnlineMealsUsedDailyDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLOnlineMealsUsedDailyDAO();
            }
            else
            {
                return new MSSQLOnlineMealsUsedDailyDAO((SqlConnection)dbConnection);
            }
        }
        public override OnlineMealsUsedHistDAO getOnlineMealsUsedHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLOnlineMealsUsedHistDAO();
            }
            else
            {
                return new MSSQLOnlineMealsUsedHistDAO((SqlConnection)dbConnection);
            }
        }
        public override OnlineMealsIntervalsDAO getOnlineMealsIntervalsDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLOnlineMealsIntervalsDAO();
            }
            else
            {
                return new MSSQLOnlineMealsIntervalsDAO((SqlConnection)dbConnection);
            }
        }
        public override MealUsedDAO getMealUsedDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMealUsedDAO();
            }
            else
            {
                return new MSSQLMealUsedDAO((SqlConnection)dbConnection);
            }
        }
        public override MealTypeEmplDAO getMealTypeEmplDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMealTypeEmplDAO();
            }
            else
            {
                return new MSSQLMealTypeEmplDAO((SqlConnection)dbConnection);
            }
        }
        public override EmployeeXMealTypeEmplDAO getEmployeeXMealTypeEmplDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeXMealTypeEmplDAO();
            }
            else
            {
                return new MSSQLEmployeeXMealTypeEmplDAO((SqlConnection)dbConnection);
            }
        }
        public override SecurityRouteDAO getSecurityRouteDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSecurityRouteDAO();
            }
            else
            {
                return new MSSQLSecurityRouteDAO((SqlConnection)dbConnection);
            }
        }
        public override SecurityRouteScheduleDAO getSecurityRouteScheduleDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSecurityRouteScheduleDAO();
            }
            else
            {
                return new MSSQLSecurityRouteScheduleDAO((SqlConnection)dbConnection);
            }
        }
        public override SecurityRoutesEmployeeDAO getSecurityRoutesEmployeeDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSecurityRoutesEmployeeDAO();
            }
            else
            {
                return new MSSQLSecurityRoutesEmployeeDAO((SqlConnection)dbConnection);
            }
        }
        public override SecurityRoutesLogDAO getSecurityRoutesLogDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSecurityRoutesLogDAO();
            }
            else
            {
                return new MSSQLSecurityRoutesLogDAO((SqlConnection)dbConnection);
            }
        }
        public override SecurityRoutesPointDAO getSecurityRoutesPointDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSecurityRoutesPointDAO();
            }
            else
            {
                return new MSSQLSecurityRoutesPointDAO((SqlConnection)dbConnection);
            }
        }
        public override SecurityRoutesReaderDAO getSecurityRoutesReaderDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSecurityRoutesReaderDAO();
            }
            else
            {
                return new MSSQLSecurityRoutesReaderDAO((SqlConnection)dbConnection);
            }
        }
        public override MapDAO getMapDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMapDAO();
            }
            else
            {
                return new MSSQLMapDAO((SqlConnection)dbConnection);
            }
        }
        public override MapsObjectDAO getMapsObjectDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMapsObjectDAO();
            }
            else
            {
                return new MSSQLMapsObjectDAO((SqlConnection)dbConnection);
            }
        }
        public override EmployeeVacEvidDAO getEmployeeVacEvidDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeVacEvidDAO();
            }
            else
            {
                return new MSSQLEmployeeVacEvidDAO((SqlConnection)dbConnection);
            }
        }
        public override LockDAO getLockDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLLockDAO();
            }
            else
            {
                return new MSSQLLockDAO((SqlConnection)dbConnection);
            }
        }
        public override FilterDAO getFilterDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLFilterDAO();
            }
            else
            {
                return new MSSQLFilterDAO((SqlConnection)dbConnection);
            }
        }
        public override UnipromButtonDAO getUnipromButtonDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLUnipromButtonDAO();
            }
            else
            {
                return new MSSQLUnipromButtonDAO((SqlConnection)dbConnection);
            }
        }
        public override UnipromButtonLogDAO getUnipromButtonLogDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLUnipromButtonLogDAO();
            }
            else
            {
                return new MSSQLUnipromButtonLogDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeeAsco4DAO getEmployeeAsco4DAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeAsco4DAO();
            }
            else
            {
                return new MSSQLEmployeeAsco4DAO((SqlConnection)dbConnection);
            }
        }

        public override VisitAsco4DAO getVisitAsco4DAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLVisitAsco4DAO();
            }
            else
            {
                return new MSSQLVisitAsco4DAO((SqlConnection)dbConnection);
            }
        }
        public override ZINApbDAO getZINApbDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLZINApbDAO();
            }
            else
            {
                return new MSSQLZINApbDAO((SqlConnection)dbConnection);
            }
        }
        public override MealsEmployeeScheduleDAO getMealsEmployeeScheduleDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMealsEmployeeScheduleDAO();
            }
            else
            {
                return new MSSQLMealsEmployeeScheduleDAO((SqlConnection)dbConnection);
            }
        }
        public override MealsWorkingUnitScheduleDAO getMealsWorkingUnitScheduleDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMealsWorkingUnitScheduleDAO();
            }
            else
            {
                return new MSSQLMealsWorkingUnitScheduleDAO((SqlConnection)dbConnection);
            }
        }
        public override EmployeeAsco4MetadataDAO getEmployeeAsco4MetadataDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeAsco4MetadataDAO();
            }
            else
            {
                return new MSSQLEmployeeAsco4MetadataDAO((SqlConnection)dbConnection);
            }
        }
        public override ResultDAO getResultDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MSSQLResultDAO(tsConnection);
                }
                else
                {
                    return new MSSQLResultDAO();
                }
            }
            else
            {
                return new MSSQLResultDAO((SqlConnection)dbConnection);
            }
        }
        public override EmployeeCounterTypeDAO getEmployeeCounterTypeDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeCounterTypeDAO();
            }
            else
            {
                return new MSSQLEmployeeCounterTypeDAO((SqlConnection)dbConnection);
            }
        }
        public override EmployeeTypeDAO getEmployeeTypeDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeTypeDAO();
            }
            else
            {
                return new MSSQLEmployeeTypeDAO((SqlConnection)dbConnection);
            }
        }
        public override EmployeeCounterValueDAO getEmployeeCounterValueDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeCounterValueDAO();
            }
            else
            {
                return new MSSQLEmployeeCounterValueDAO((SqlConnection)dbConnection);
            }
        }
        public override EmployeeLoanDAO getEmployeeLoanDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeLoanDAO();
            }
            else
            {
                return new MSSQLEmployeeLoanDAO((SqlConnection)dbConnection);
            }
        }
        public override IOPairsProcessedHistDAO getIOPairsProcessedHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLIOPairsProcessedHistDAO();
            }
            else
            {
                return new MSSQLIOPairsProcessedHistDAO((SqlConnection)dbConnection);
            }
        }
        public override PassTypeLimitDAO getPassTypeLimitDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLPassTypeLimitDAO();
            }
            else
            {
                return new MSSQLPassTypeLimitDAO((SqlConnection)dbConnection);
            }
        }
        public override EmployeeCounterValueHistDAO getEmployeeCounterValueHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeCounterValueHistDAO();
            }
            else
            {
                return new MSSQLEmployeeCounterValueHistDAO((SqlConnection)dbConnection);
            }
        }
        public override HolidaysExtendedDAO getHolidaysExtendedDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLHolidaysExtendedDAO();
            }
            else
            {
                return new MSSQLHolidaysExtendedDAO((SqlConnection)dbConnection);
            }
        }
        public override ApplUserCategoryDAO getApplUserCategoryDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLApplUserCategoryDAO();
            }
            else
            {
                return new MSSQLApplUserCategoryDAO((SqlConnection)dbConnection);
            }
        }
        public override ApplUserXApplUserCategoryDAO getApplUserXCategoryDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLApplUserXApplUserCategoryDAO();
            }
            else
            {
                return new MSSQLApplUserXApplUserCategoryDAO((SqlConnection)dbConnection);
            }
        }
        public override ApplUserCategoryXPassTypeDAO getApplUserCategoryXPassTypeDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLApplUserCategoryXPassTypeDAO();
            }
            else
            {
                return new MSSQLApplUserCategoryXPassTypeDAO((SqlConnection)dbConnection);
            }
        }
        public override OrganizationalUnitDAO getOrganizationalUnitDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLOrganizationalUnitDAO();
            }
            else
            {
                return new MSSQLOrganizationalUnitDAO((SqlConnection)dbConnection);
            }
        }

        public override WorkingUnitXOrganizationalUnitDAO getWorkingUnitXOrganizationalUnitDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLWorkingUnitXOrganizationalUnitDAO();
            }
            else
            {
                return new MSSQLWorkingUnitXOrganizationalUnitDAO((SqlConnection)dbConnection);
            }
        }
        public override ApplUserXOrgUnitDAO getApplUserXOrgUnitDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLApplUserXOrgUnitDAO();
            }
            else
            {
                return new MSSQLApplUserXOrgUnitDAO((SqlConnection)dbConnection);
            }
        }
        public override PassTypesConfirmationDAO getPassTypesConfirmationDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLPassTypesConfirmationDAO();
            }
            else
            {
                return new MSSQLPassTypesConfirmationDAO((SqlConnection)dbConnection);
            }
        }
        public override SyncBufferDataHistDAO getSyncBufferDataHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSyncBufferDataHistDAO();
            }
            else
            {
                return new MSSQLSyncBufferDataHistDAO((SqlConnection)dbConnection);
            }
        }
        public override SyncEmployeesHistDAO getSyncEmployeesHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSyncEmployeesHistDAO();
            }
            else
            {
                return new MSSQLSyncEmployeesHistDAO((SqlConnection)dbConnection);
            }
        }
        public override SyncFinancialStructureHistDAO getSyncFinancialStructureHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSyncFinancialStructureHistDAO();
            }
            else
            {
                return new MSSQLSyncFinancialStructureHistDAO((SqlConnection)dbConnection);
            }
        }
        public override SyncOrganizationalStructureHistDAO getSyncOrganizationalStructureHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSyncOrganizationalStructureHistDAO();
            }
            else
            {
                return new MSSQLSyncOrganizationalStructureHistDAO((SqlConnection)dbConnection);
            }
        }
        public override SyncEmployeePositionHistDAO getSyncEmployeePositionHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSyncEmployeePositionHistDAO();
            }
            else
            {
                return new MSSQLSyncEmployeePositionHistDAO((SqlConnection)dbConnection);
            }
        }
        public override SyncResponsibilityHistDAO getSyncResponsibilityHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSyncResponsibilityHistDAO();
            }
            else
            {
                return new MSSQLSyncResponsibilityHistDAO((SqlConnection)dbConnection);
            }
        }
        public override SyncCostCenterHistDAO getSyncCostCenterHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSyncCostCenterHistDAO();
            }
            else
            {
                return new MSSQLSyncCostCenterHistDAO((SqlConnection)dbConnection);
            }
        }
        public override SyncAnnualLeaveRecalcHistDAO getSyncAnnualLeaveRecalcHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSyncAnnualLeaveRecalcHistDAO();
            }
            else
            {
                return new MSSQLSyncAnnualLeaveRecalcHistDAO((SqlConnection)dbConnection);
            }
        }
        public override SyncAnnualLeaveRecalcFlagDAO getSyncAnnualLeaveRecalcFlagDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSyncAnnualLeaveRecalcFlagDAO();
            }
            else
            {
                return new MSSQLSyncAnnualLeaveRecalcFlagDAO((SqlConnection)dbConnection);
            }
        }
        public override EmployeeResponsibilityDAO getEmployeeResponsibilityDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeResponsibilityDAO();
            }
            else
            {
                return new MSSQLEmployeeResponsibilityDAO((SqlConnection)dbConnection);
            }
        }
        public override EmployeeHistDAO getEmployeeHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeHistDAO();
            }
            else
            {
                return new MSSQLEmployeeHistDAO((SqlConnection)dbConnection);
            }
        }
        public override EmployeeAsco4HistDAO getEmployeeAsco4HistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeAsco4HistDAO();
            }
            else
            {
                return new MSSQLEmployeeAsco4HistDAO((SqlConnection)dbConnection);
            }
        }
        public override EmployeePYDataSumDAO getEmployeePYDataSumDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeePYDataSumDAO();
            }
            else
            {
                return new MSSQLEmployeePYDataSumDAO((SqlConnection)dbConnection);
            }
        }
        public override EmployeePYDataAnaliticalDAO getEmployeePYDataAnaliticalDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeePYDataAnaliticalDAO();
            }
            else
            {
                return new MSSQLEmployeePYDataAnaliticalDAO((SqlConnection)dbConnection);
            }
        }
        public override EmployeePYDataBufferDAO getEmployeePYDataBufferDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeePYDataBufferDAO();
            }
            else
            {
                return new MSSQLEmployeePYDataBufferDAO((SqlConnection)dbConnection);
            }
        }

        public override ApplUsersLoginChangesTblDAO getApplUsersLoginChangesTblDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLApplUsersLoginChangesTblDAO();
            }
            else
            {
                return new MSSQLApplUsersLoginChangesTblDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeeLockedDayDAO getEmployeeLockedDayDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeLockedDayDAO();
            }
            else
            {
                return new MSSQLEmployeeLockedDayDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeeTypeVisibilityDAO getEmployeeTypeVisibilityDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeTypeVisibilityDAO();
            }
            else
            {
                return new MSSQLEmployeeTypeVisibilityDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeePositionDAO getEmployeePositionDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeePositionDAO();
            }
            else
            {
                return new MSSQLEmployeePositionDAO((SqlConnection)dbConnection);
            }
        }

        public override RiskDAO getRiskDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLRiskDAO();
            }
            else
            {
                return new MSSQLRiskDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeeXRiskDAO getEmployeeXRiskDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeXRiskDAO();
            }
            else
            {
                return new MSSQLEmployeeXRiskDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeePositionXRiskDAO getEmployeePositionXRiskDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeePositionXRiskDAO();
            }
            else
            {
                return new MSSQLEmployeePositionXRiskDAO((SqlConnection)dbConnection);
            }
        }

        public override MedicalCheckPointDAO getMedicalCheckPointDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMedicalCheckPointDAO();
            }
            else
            {
                return new MSSQLMedicalCheckPointDAO((SqlConnection)dbConnection);
            }
        }

        public override MedicalCheckVisitHdrDAO getMedicalCheckVisitHdrDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMedicalCheckVisitHdrDAO();
            }
            else
            {
                return new MSSQLMedicalCheckVisitHdrDAO((SqlConnection)dbConnection);
            }
        }

        public override MedicalCheckVisitDtlDAO getMedicalCheckVisitDtlDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMedicalCheckVisitDtlDAO();
            }
            else
            {
                return new MSSQLMedicalCheckVisitDtlDAO((SqlConnection)dbConnection);
            }
        }

        public override MedicalCheckVisitHdrHistDAO getMedicalCheckVisitHdrHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMedicalCheckVisitHdrHistDAO();
            }
            else
            {
                return new MSSQLMedicalCheckVisitHdrHistDAO((SqlConnection)dbConnection);
            }
        }

        public override MedicalCheckVisitDtlHistDAO getMedicalCheckVisitDtlHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMedicalCheckVisitDtlHistDAO();
            }
            else
            {
                return new MSSQLMedicalCheckVisitDtlHistDAO((SqlConnection)dbConnection);
            }
        }

        public override MedicalCheckDisabilityDAO getMedicalCheckDisabilityDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMedicalCheckDisabilityDAO();
            }
            else
            {
                return new MSSQLMedicalCheckDisabilityDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeesXMedicalCheckDisabilityDAO getEmployeesXMedicalCheckDisabilityDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeXMedicalCheckDisabilityDAO();
            }
            else
            {
                return new MSSQLEmployeeXMedicalCheckDisabilityDAO((SqlConnection)dbConnection);
            }
        }

        public override VaccineDAO getVaccineDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLVaccineDAO();
            }
            else
            {
                return new MSSQLVaccineDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeeXVaccineDAO getEmployeeXVaccineDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeXVaccineDAO();
            }
            else
            {
                return new MSSQLEmployeeXVaccineDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeePhysicalDataDAO getEmployeePhysicalDataDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeePhysicalDataDAO();
            }
            else
            {
                return new MSSQLEmployeePhysicalDataDAO((SqlConnection)dbConnection);
            }
        }

        public override SystemClosingEventDAO getSystemClosingEventDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSystemClosingEventDAO();
            }
            else
            {
                return new MSSQLSystemClosingEventDAO((SqlConnection)dbConnection);
            }
        }

        public override SystemMessageDAO getSystemMessageDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLSystemMessageDAO();
            }
            else
            {
                return new MSSQLSystemMessageDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeeCounterMonthlyBalanceDAO getEmployeeCounterMonthlyBalanceDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeCounterMonthlyBalanceDAO();
            }
            else
            {
                return new MSSQLEmployeeCounterMonthlyBalanceDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeeCounterMonthlyBalanceHistDAO getEmployeeCounterMonthlyBalanceHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeeCounterMonthlyBalanceHistDAO();
            }
            else
            {
                return new MSSQLEmployeeCounterMonthlyBalanceHistDAO((SqlConnection)dbConnection);
            }
        }

        public override BufferMonthlyBalancePaidDAO getEmployeeCounterMonthlyBalancePaidDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLBufferMonthlyBalancePaidDAO();
            }
            else
            {
                return new MSSQLBufferMonthlyBalancePaidDAO((SqlConnection)dbConnection);
            }
        }

        public override LogTmpAdditionalInfoDAO getLogTmpDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLLogTmpAdditionalInfoDAO();
            }
            else
            {
                return new MSSQLLogTmpAdditionalInfoDAO((SqlConnection)dbConnection);
            }
        }

        public override PassesAdditionalInfoDAO getPassesAdditionalInfoDAO(object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLPassesAdditionalInfoDAO();
            }
            else
            {
                return new MSSQLPassesAdditionalInfoDAO((SqlConnection)dbConnection);
            }
        
        }

        public override LogAdditionalInfoDAO getLogAdditionalInfoDAO(object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLLogAdditionalInfoDAO();
            }
            else
            {
                return new MSSQLLogAdditionalInfoDAO((SqlConnection)dbConnection);
            }
        }

        public override MealTypeAdditionalDataDAO getMealTypeAddDataDAO(object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLMealTypeAdditionalDataDAO();
            }
            else
            {
                return new MSSQLMealTypeAdditionalDataDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeePYTransportTypeDAO getEmployeePYTransportTypeDAO(object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeePYTransportTypeDAO();
            }
            else
            {
                return new MSSQLEmployeePYTransportTypeDAO((SqlConnection)dbConnection);
            }
        }

        public override EmployeePYTransportDataDAO getEmployeePYTransportDataDAO(object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLEmployeePYTransportDataDAO();
            }
            else
            {
                return new MSSQLEmployeePYTransportDataDAO((SqlConnection)dbConnection);
            }
        }

        public override ApplUserLoginRFIDDAO getApplUserLoginRFIDDAO(object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLApplUserLoginRFIDDAO();
            }
            else
            {
                return new MSSQLApplUserLoginRFIDDAO((SqlConnection)dbConnection);
            }
        }

        public override ACSEventsDAO getACSEventsDAO(object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MSSQLACSEventsDAO();
            }
            else
            {
                return new MSSQLACSEventsDAO((SqlConnection)dbConnection);
            }
        }
    }
}
