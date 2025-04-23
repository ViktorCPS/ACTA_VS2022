using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Data;

using MySql.Data.MySqlClient;
using Util;
using DataAccess.MySQL;


namespace DataAccess
{
    /// <summary>
    /// Summary description for MySQLDAOFactory.
    /// </summary>
    public class MySQLDAOFactory : DAOFactory
    {
        public static readonly string Driver = "";
        private static string ConnectionString = "";
        // thread-safe locker
        private static object locker = new object();
        public static MySqlConnection connection;
        private MySqlConnection tsConnection = null;

        /// <summary>
        /// Finalizer
        /// </summary>
        ~MySQLDAOFactory()
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

        public MySQLDAOFactory()
        {
            lock (locker)
            {
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
            }
        }
        public override bool TestDBConnectionDummySelect(Object dbConnection)
        {
            try
            {
                DataSet dataSet = new DataSet();
                MySqlConnection conn = dbConnection as MySqlConnection;

                string cmdText = "SELECT Now()";
                MySqlCommand cmd = new MySqlCommand(cmdText, conn);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataSet, "Test");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public static MySqlConnection getConnection()
        {
            lock (locker)
            {
                try
                {
                    if (connection == null)
                    {
                        // Set connection for the first time
                        connection = new MySqlConnection(ConnectionString);
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
                    MySqlConnection conn = dbConnection as MySqlConnection;
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
                MySqlConnection mySqlconnection = null;
                try
                {
                    mySqlconnection = new MySqlConnection(ConnectionString);
                    mySqlconnection.Open();
                    return mySqlconnection;
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
                MySqlConnection testConn = getConnection();
                if (threadSafe)
                {
                    tsConnection = MakeNewDBConnection() as MySqlConnection;
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
            //catch(Exception ex)
            catch
            {
                return isConnected;
                //throw ex;
            }

            return isConnected;
        }

        //  06.02.2020. BOJAN
        public override MachineDAO getMachineDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLMachineDAO();
            }
            else
            {
                return new MySQLMachineDAO((MySqlConnection)dbConnection);
            }
        }

        public override bool TestDataSourceConnection(Object dbConnection)
        {
            bool isConnected = false;

            try
            {
                MySqlConnection testConn = (MySqlConnection)dbConnection;
                if (threadSafe)
                {
                    tsConnection = MakeNewDBConnection() as MySqlConnection;
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
            //catch(Exception ex)
            catch
            {
                return isConnected;
                //throw ex;
            }

            return isConnected;
        }

        public override int getDBConnectedHosts()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("show full processlist", connection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Processes");
                DataTable table = dataSet.Tables["Processes"];

                int hosts = 0;

                if (table.Rows.Count > 0)
                {
                    hosts = table.Rows.Count;
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
                MySqlCommand cmd = new MySqlCommand("show session variables like 'pid_file'", connection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Pid_file");
                DataTable table = dataSet.Tables["Pid_file"];

                if (table.Rows.Count == 1)
                {
                    string serverName = null;
                    foreach (DataRow row in table.Rows)
                    {
                        serverName = (row["value"].ToString());
                        serverName = serverName.Substring(serverName.LastIndexOf(@"\") + 1, serverName.LastIndexOf(@".") - 1 - serverName.LastIndexOf(@"\"));
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
                MySqlCommand cmd = new MySqlCommand("SELECT DATABASE() AS name", connection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "DBName");
                DataTable table = dataSet.Tables["DBName"];

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
            try
            {
                MySqlCommand cmd = new MySqlCommand("show session variables like 'port'", connection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Port");
                DataTable table = dataSet.Tables["Port"];

                if (table.Rows.Count == 1)
                {
                    string serverPort = null;
                    foreach (DataRow row in table.Rows)
                    {
                        serverPort = row["value"].ToString();
                    }
                    return serverPort;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: getDBPortName " + ex.Message);
            }
        }

        public override SyncWithNavDAO getSyncWithNavDAO(object dbConnection)
        {
            throw new NotImplementedException("DataAccess.MySQLDAOFactory.getSyncWithNavDAO()");
            /*if (dbConnection != null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MySQLSyncWithNavDAO(tsConnection);
                }
                else
                {
                    return new MySQLSyncWithNavDAO();
                }
            }
            else
            {
                return new MySQLSyncWithNavDAO((MySqlConnection)dbConnection);
            }
             * */
        }

        public override EmployeeDAO getEmployeeDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MySQLEmployeDAO(tsConnection);
                }
                else
                {
                    return new MySQLEmployeDAO();
                }
            }
            else
            {
                return new MySQLEmployeDAO((MySqlConnection)dbConnection);
            }
        }

        public override DocumentsDAO getDocumentsDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MySQLDocumentsDAO(tsConnection);
                }
                else
                {
                    return new MySQLDocumentsDAO();
                }
            }
            else
            {
                return new MySQLDocumentsDAO((MySqlConnection)dbConnection);
            }
        }

        public override LocationDAO getLocationDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MySQLLocationDAO(tsConnection);
                }
                else
                {
                    return new MySQLLocationDAO();
                }
            }
            else
            {
                return new MySQLLocationDAO((MySqlConnection)dbConnection);
            }
        }

        public override WorkingUnitsDAO getWorkingUnitsDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MySQLWorkingUnitsDAO(tsConnection);
                }
                else
                {
                    return new MySQLWorkingUnitsDAO();
                }
            }
            else
            {
                return new MySQLWorkingUnitsDAO((MySqlConnection)dbConnection);
            }
        }
        public override TimeSchemaIntervalLibraryDAO getTimeSchemaIntervalLibraryDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MySQLTimeSchemaIntervalLibraryDAO(tsConnection);
                }
                else
                {
                    return new MySQLTimeSchemaIntervalLibraryDAO();
                }
            }
            else
            {
                return new MySQLTimeSchemaIntervalLibraryDAO((MySqlConnection)dbConnection);
            }
        }
        public override TimeSchemaIntervalLibraryDtlDAO getTimeSchemaIntervalLibraryDtlDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MySQLTimeSchemaIntervalLibraryDtlDAO(tsConnection);
                }
                else
                {
                    return new MySQLTimeSchemaIntervalLibraryDtlDAO();
                }
            }
            else
            {
                return new MySQLTimeSchemaIntervalLibraryDtlDAO((MySqlConnection)dbConnection);
            }
        }
        public override PassTypeDAO getPassTypeDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MySQLPassTypeDAO(tsConnection);
                }
                else
                {
                    return new MySQLPassTypeDAO();
                }
            }
            else
            {
                return new MySQLPassTypeDAO((MySqlConnection)dbConnection);
            }
        }

        public override AddressDAO getAddressDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLAddressDAO();
            }
            else
            {
                return new MySQLAddressDAO((MySqlConnection)dbConnection);
            }
        }

        public override ReaderDAO getReaderDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLReaderDAO();
            }
            else
            {
                return new MySQLReaderDAO((MySqlConnection)dbConnection);
            }
        }

        public override TagDAO getTagDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLTagDAO();
            }
            else
            {
                return new MySQLTagDAO((MySqlConnection)dbConnection);
            }
        }

        public override PassDAO getPassDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MySQLPassDAO(tsConnection);
                }
                else
                {
                    return new MySQLPassDAO();
                }
            }
            else
            {
                return new MySQLPassDAO((MySqlConnection)dbConnection);
            }
        }

        public override PassHistDAO getPassHistDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLPassHistDAO();
            }
            else
            {
                return new MySQLPassHistDAO((MySqlConnection)dbConnection);
            }
        }

        public override VisitDAO getVisitDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLVisitDAO();
            }
            else
            {
                return new MySQLVisitDAO((MySqlConnection)dbConnection);
            }
        }

        public override CitizenDAO getCitizenDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLCitizenDAO();
            }
            else
            {
                return new MySQLCitizenDAO((MySqlConnection)dbConnection);
            }
        }

        public override LogDAO getLogDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLLogDAO();
            }
            else
            {
                return new MySQLLogDAO((MySqlConnection)dbConnection);
            }
        }

        public override IOPairDAO getIOPairDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MySQLIOPairDAO(tsConnection);
                }
                else
                {
                    return new MySQLIOPairDAO();
                }
            }
            else
            {
                return new MySQLIOPairDAO((MySqlConnection)dbConnection);
            }
        }

        public override WorkTimeSchemaDAO getWorkTimeSchemaDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MySQLWorkTimeShemaDAO(tsConnection);
                }
                else
                {
                    return new MySQLWorkTimeShemaDAO();
                }
            }
            else
            {
                return new MySQLWorkTimeShemaDAO((MySqlConnection)dbConnection);
            }
        }

        public override WorkingGroupDAO getWorkingGroupDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLWorkingGroupDAO();
            }
            else
            {
                return new MySQLWorkingGroupDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeeTimeScheduleDAO getEmployeeTimeScheduleDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MySQLEmployeeTimeSheduleDAO(tsConnection);
                }
                else
                {
                    return new MySQLEmployeeTimeSheduleDAO();
                }
            }
            else
            {
                return new MySQLEmployeeTimeSheduleDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeeAbsenceDAO getEmployeeAbsenceDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeAbsenceDAO();
            }
            else
            {
                return new MySQLEmployeeAbsenceDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeeLocationDAO getEmployeeLocationDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeLocationDAO();
            }
            else
            {
                return new MySQLEmployeeLocationDAO((MySqlConnection)dbConnection);
            }
        }

        public override GateDAO getGateDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLGateDAO();
            }
            else
            {
                return new MySQLGateDAO((MySqlConnection)dbConnection);
            }
        }

        public override MealsTypeScheduleDAO getMealsTypeScheduleDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLMealsTypeScheduleDAO();
            }
            else
            {
                return new MySQLMealsTypeScheduleDAO((MySqlConnection)dbConnection);
            }
        }

        public override ApplUsersXWUDAO getApplUsersXWUDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MySQLApplXWUDAO(tsConnection);
                }
                else
                {
                    return new MySQLApplXWUDAO();
                }
            }
            else
            {
                return new MySQLApplXWUDAO((MySqlConnection)dbConnection);
            }
        }

        public override ApplUserDAO getApplUserDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MySQLApplUserDAO(tsConnection);
                }
                else
                {
                    return new MySQLApplUserDAO();
                }
            }
            else
            {
                return new MySQLApplUserDAO((MySqlConnection)dbConnection);
            }
        }

        public override ApplUsersLogDAO getApplUsersLogDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLApplUsersLogDAO();
            }
            else
            {
                return new MySQLApplUsersLogDAO((MySqlConnection)dbConnection);
            }
        }

        public override ApplRoleDAO getApplRoleDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLApplRoleDAO();
            }
            else
            {
                return new MySQLApplRoleDAO((MySqlConnection)dbConnection);
            }
        }

        public override ApplMenuItemDAO getApplMenuItemDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLApplMenuItemDAO();
            }
            else
            {
                return new MySQLApplMenuItemDAO((MySqlConnection)dbConnection);
            }
        }

        public override ApplUsersXRoleDAO getApplUsersXRoleDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLApplXRoleDAO();
            }
            else
            {
                return new MySQLApplXRoleDAO((MySqlConnection)dbConnection);
            }
        }

        public override ExitPermissionDAO getExitPermissionsDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLExitPermissionDAO();
            }
            else
            {
                return new MySQLExitPermissionDAO((MySqlConnection)dbConnection);
            }
        }

        public override HolidayDAO getHolidayDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLHolidayDAO();
            }
            else
            {
                return new MySQLHolidayDAO((MySqlConnection)dbConnection);
            }
        }
        public override RuleDAO getRuleDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLRuleDAO();
            }
            else
            {
                return new MySQLRuleDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeeGroupAccessControlDAO getEmployeeGroupAccessControlDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeGroupAccessControlDAO();
            }
            else
            {
                return new MySQLEmployeeGroupAccessControlDAO((MySqlConnection)dbConnection);
            }
        }

        public override TimeAccessProfileDAO getTimeAccessProfileDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLTimeAccessProfileDAO();
            }
            else
            {
                return new MySQLTimeAccessProfileDAO((MySqlConnection)dbConnection);
            }
        }

        public override TimeAccessProfileDtlDAO getTimeAccessProfileDtlDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLTimeAccessProfileDtlDAO();
            }
            else
            {
                return new MySQLTimeAccessProfileDtlDAO((MySqlConnection)dbConnection);
            }
        }
        public override IOPairProcessedDAO getIOPairProcessedDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLIOPairProcessedDAO();
            }
            else
            {
                return new MySQLIOPairProcessedDAO((MySqlConnection)dbConnection);
            }
        }

        public override GateTimeAccessProfileDAO getGateTimeAccessProfileDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLGateTimeAccessProfileDAO();
            }
            else
            {
                return new MySQLGateTimeAccessProfileDAO((MySqlConnection)dbConnection);
            }
        }

        public override AccessGroupXGateDAO getAccessGroupXGateDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLAccessGroupXGateDAO();
            }
            else
            {
                return new MySQLAccessGroupXGateDAO((MySqlConnection)dbConnection);
            }
        }

        public override ExtraHourDAO getExtraHourDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLExtraHourDAO();
            }
            else
            {
                return new MySQLExtraHourDAO((MySqlConnection)dbConnection);
            }
        }

        public override ExtraHourUsedDAO getExtraHourUsedDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLExtraHourUsedDAO();
            }
            else
            {
                return new MySQLExtraHourUsedDAO((MySqlConnection)dbConnection);
            }
        }

        public override TimeSchemaPauseDAO getTimeSchemaPauseDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLTimeSchemaPauseDAO();
            }
            else
            {
                return new MySQLTimeSchemaPauseDAO((MySqlConnection)dbConnection);
            }
        }

        public override AccessControlFileDAO getAccessControlFileDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLAccessControlFileDAO();
            }
            else
            {
                return new MySQLAccessControlFileDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeeImageFileDAO getEmployeeImageFileDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeImageFileDAO();
            }
            else
            {
                return new MySQLEmployeeImageFileDAO((MySqlConnection)dbConnection);
            }
        }

        public override VisitorDocFileDAO getVisitorDocFileDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLVisitorDocFileDAO();
            }
            else
            {
                return new MySQLVisitorDocFileDAO((MySqlConnection)dbConnection);
            }
        }

        public override CameraSnapshotFileDAO getCameraSnapshotFileDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLCameraSnapshotFileDAO();
            }
            else
            {
                return new MySQLCameraSnapshotFileDAO((MySqlConnection)dbConnection);
            }
        }

        public override CameraDAO getCameraDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLCameraDAO();
            }
            else
            {
                return new MySQLCameraDAO((MySqlConnection)dbConnection);
            }
        }

        public override CamerasXReadersDAO getCamerasXReadersDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLCamerasXReadersDAO();
            }
            else
            {
                return new MySQLCamerasXReadersDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeeGroupsTimeScheduleDAO getEmployeeGroupsTimeScheduleDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeGroupsTimeScheduleDAO();
            }
            else
            {
                return new MySQLEmployeeGroupsTimeScheduleDAO((MySqlConnection)dbConnection);
            }
        }

        public override GateSyncDAO getGateSyncDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLGateSyncDAO();
            }
            else
            {
                return new MySQLGateSyncDAO((MySqlConnection)dbConnection);
            }
        }

        public override LicenceDAO getLicenceDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLLicenceDAO();
            }
            else
            {
                return new MySQLLicenceDAO((MySqlConnection)dbConnection);
            }
        }

        public override MealTypeDAO getMealTypeDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLMealTypeDAO();
            }
            else
            {
                return new MySQLMealTypeDAO((MySqlConnection)dbConnection);
            }
        }

        public override MealAssignedDAO getMealAssignedDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLMealAssignedDAO();
            }
            else
            {
                return new MySQLMealAssignedDAO((MySqlConnection)dbConnection);
            }
        }
        public override MealsPointDAO getMealsPointDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLMealsPointDAO();
            }
            else
            {
                return new MySQLMealsPointDAO((MySqlConnection)dbConnection);
            }
        }
        public override OnlineMealsPointsDAO getOnlineMealsPointDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLOnlineMealsPointsDAO();
            }
            else
            {
                return new MySQLOnlineMealsPointsDAO((MySqlConnection)dbConnection);
            }
        }
        public override OnlineMealsRestaurantDAO getOnlineMealsRestaurantDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLOnlineMealsRestaurantDAO();
            }
            else
            {
                return new MySQLOnlineMealsRestaurantDAO((MySqlConnection)dbConnection);
            }
        }
        public override OnlineMealsTypesDAO getOnlineMealsTypesDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLOnlineMealsTypesDAO();
            }
            else
            {
                return new MySQLOnlineMealsTypesDAO((MySqlConnection)dbConnection);
            }
        }
        public override OnlineMealsUsedDAO getOnlineMealsUsedDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLOnlineMealsUsedDAO();
            }
            else
            {
                return new MySQLOnlineMealsUsedDAO((MySqlConnection)dbConnection);
            }
        }
        public override OnlineMealsUsedDailyDAO getOnlineMealsUsedDailyDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLOnlineMealsUsedDailyDAO();
            }
            else
            {
                return new MySQLOnlineMealsUsedDailyDAO((MySqlConnection)dbConnection);
            }
        }
        public override OnlineMealsUsedHistDAO getOnlineMealsUsedHistDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLOnlineMealsUsedHistDAO();
            }
            else
            {
                return new MySQLOnlineMealsUsedHistDAO((MySqlConnection)dbConnection);
            }
        }

        public override OnlineMealsIntervalsDAO getOnlineMealsIntervalsDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLOnlineMealsIntervalsDAO();
            }
            else
            {
                return new MySQLOnlineMealsIntervalsDAO((MySqlConnection)dbConnection);
            }
        }
        public override MealUsedDAO getMealUsedDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLMealUsedDAO();
            }
            else
            {
                return new MySQLMealUsedDAO((MySqlConnection)dbConnection);
            }
        }
        public override MealTypeEmplDAO getMealTypeEmplDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLMealTypeEmplDAO();
            }
            else
            {
                return new MySQLMealTypeEmplDAO((MySqlConnection)dbConnection);
            }
        }
        public override EmployeeXMealTypeEmplDAO getEmployeeXMealTypeEmplDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeXMealTypeEmplDAO();
            }
            else
            {
                return new MySQLEmployeeXMealTypeEmplDAO((MySqlConnection)dbConnection);
            }
        }
        public override SecurityRouteDAO getSecurityRouteDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLSecurityRouteDAO();
            }
            else
            {
                return new MySQLSecurityRouteDAO((MySqlConnection)dbConnection);
            }
        }
        public override SecurityRouteScheduleDAO getSecurityRouteScheduleDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLSecurityRouteScheduleDAO();
            }
            else
            {
                return new MySQLSecurityRouteScheduleDAO((MySqlConnection)dbConnection);
            }
        }
        public override SecurityRoutesEmployeeDAO getSecurityRoutesEmployeeDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLSecurityRoutesEmployeeDAO();
            }
            else
            {
                return new MySQLSecurityRoutesEmployeeDAO((MySqlConnection)dbConnection);
            }
        }
        public override SecurityRoutesLogDAO getSecurityRoutesLogDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLSecurityRoutesLogDAO();
            }
            else
            {
                return new MySQLSecurityRoutesLogDAO((MySqlConnection)dbConnection);
            }
        }
        public override SecurityRoutesPointDAO getSecurityRoutesPointDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLSecurityRoutesPointDAO();
            }
            else
            {
                return new MySQLSecurityRoutesPointDAO((MySqlConnection)dbConnection);
            }
        }
        public override SecurityRoutesReaderDAO getSecurityRoutesReaderDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLSecurityRoutesReaderDAO();
            }
            else
            {
                return new MySQLSecurityRoutesReaderDAO((MySqlConnection)dbConnection);
            }
        }
        public override MapDAO getMapDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLMapDAO();
            }
            else
            {
                return new MySQLMapDAO((MySqlConnection)dbConnection);
            }
        }
        public override MapsObjectDAO getMapsObjectDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLMapsObjectDAO();
            }
            else
            {
                return new MySQLMapsObjectDAO((MySqlConnection)dbConnection);
            }
        }
        public override EmployeeVacEvidDAO getEmployeeVacEvidDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeVacEvidDAO();
            }
            else
            {
                return new MySQLEmployeeVacEvidDAO((MySqlConnection)dbConnection);
            }
        }
        public override LockDAO getLockDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLLockDAO();
            }
            else
            {
                return new MySQLLockDAO((MySqlConnection)dbConnection);
            }
        }
        public override FilterDAO getFilterDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLFilterDAO();
            }
            else
            {
                return new MySQLFilterDAO((MySqlConnection)dbConnection);
            }
        }
        public override UnipromButtonDAO getUnipromButtonDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLUnipromButtonDAO();
            }
            else
            {
                return new MySQLUnipromButtonDAO((MySqlConnection)dbConnection);
            }
        }
        public override UnipromButtonLogDAO getUnipromButtonLogDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLUnipromButtonLogDAO();
            }
            else
            {
                return new MySQLUnipromButtonLogDAO((MySqlConnection)dbConnection);
            }
        }
        public override EmployeeAsco4DAO getEmployeeAsco4DAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeAsco4DAO();
            }
            else
            {
                return new MySQLEmployeeAsco4DAO((MySqlConnection)dbConnection);
            }
        }
        public override VisitAsco4DAO getVisitAsco4DAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLVisitAsco4DAO();
            }
            else
            {
                return new MySQLVisitAsco4DAO((MySqlConnection)dbConnection);
            }
        }
        public override ZINApbDAO getZINApbDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLZINApbDAO();
            }
            else
            {
                return new MySQLZINApbDAO((MySqlConnection)dbConnection);
            }
        }
        public override MealsEmployeeScheduleDAO getMealsEmployeeScheduleDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLMealsEmployeeScheduleDAO();
            }
            else
            {
                return new MySQLMealsEmployeeScheduleDAO((MySqlConnection)dbConnection);
            }
        }
        public override MealsWorkingUnitScheduleDAO getMealsWorkingUnitScheduleDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLMealsWorkingUnitScheduleDAO();
            }
            else
            {
                return new MySQLMealsWorkingUnitScheduleDAO((MySqlConnection)dbConnection);
            }
        }
        public override EmployeeAsco4MetadataDAO getEmployeeAsco4MetadataDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeAsco4MetadataDAO();
            }
            else
            {
                return new MySQLEmployeeAsco4MetadataDAO((MySqlConnection)dbConnection);
            }
        }
        public override ResultDAO getResultDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                if (threadSafe && (tsConnection != null))
                {
                    return new MySQLResultDAO(tsConnection);
                }
                else
                {
                    return new MySQLResultDAO();
                }
            }
            else
            {
                return new MySQLResultDAO((MySqlConnection)dbConnection);
            }
        }
        public override EmployeeCounterTypeDAO getEmployeeCounterTypeDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeCounterTypeDAO();
            }
            else
            {
                return new MySQLEmployeeCounterTypeDAO((MySqlConnection)dbConnection);
            }
        }
        public override EmployeeTypeDAO getEmployeeTypeDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeTypeDAO();
            }
            else
            {
                return new MySQLEmployeeTypeDAO((MySqlConnection)dbConnection);
            }
        }
        public override EmployeeCounterValueDAO getEmployeeCounterValueDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeCounterValueDAO();
            }
            else
            {
                return new MySQLEmployeeCounterValueDAO((MySqlConnection)dbConnection);
            }
        }
        public override EmployeeLoanDAO getEmployeeLoanDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeLoanDAO();
            }
            else
            {
                return new MySQLEmployeeLoanDAO((MySqlConnection)dbConnection);
            }
        }
        public override IOPairsProcessedHistDAO getIOPairsProcessedHistDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLIOPairsProcessedHistDAO();
            }
            else
            {
                return new MySQLIOPairsProcessedHistDAO((MySqlConnection)dbConnection);
            }
        }
        public override PassTypeLimitDAO getPassTypeLimitDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLPassTypeLimitDAO();
            }
            else
            {
                return new MySQLPassTypeLimitDAO((MySqlConnection)dbConnection);
            }
        }
        public override EmployeeCounterValueHistDAO getEmployeeCounterValueHistDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeCounterValueHistDAO();
            }
            else
            {
                return new MySQLEmployeeCounterValueHistDAO((MySqlConnection)dbConnection);
            }
        }
        public override HolidaysExtendedDAO getHolidaysExtendedDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLHolidaysExtendedDAO();
            }
            else
            {
                return new MySQLHolidaysExtendedDAO((MySqlConnection)dbConnection);
            }
        }
        public override ApplUserCategoryDAO getApplUserCategoryDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLApplUserCategoryDAO();
            }
            else
            {
                return new MySQLApplUserCategoryDAO((MySqlConnection)dbConnection);
            }
        }
        public override ApplUserXApplUserCategoryDAO getApplUserXCategoryDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLApplUserXApplUserCategoryDAO();
            }
            else
            {
                return new MySQLApplUserXApplUserCategoryDAO((MySqlConnection)dbConnection);
            }
        }
        public override ApplUserCategoryXPassTypeDAO getApplUserCategoryXPassTypeDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLApplUserCategoryXPassTypeDAO();
            }
            else
            {
                return new MySQLApplUserCategoryXPassTypeDAO((MySqlConnection)dbConnection);
            }
        }
        public override OrganizationalUnitDAO getOrganizationalUnitDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLOrganizationalUnitDAO();
            }
            else
            {
                return new MySQLOrganizationalUnitDAO((MySqlConnection)dbConnection);
            }
        }
        public override WorkingUnitXOrganizationalUnitDAO getWorkingUnitXOrganizationalUnitDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLWorkingUnitXOrganizationalUnitDAO();
            }
            else
            {
                return new MySQLWorkingUnitXOrganizationalUnitDAO((MySqlConnection)dbConnection);
            }
        }
        public override ApplUserXOrgUnitDAO getApplUserXOrgUnitDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLApplUserXOrgUnitDAO();
            }
            else
            {
                return new MySQLApplUserXOrgUnitDAO((MySqlConnection)dbConnection);
            }
        }
        public override PassTypesConfirmationDAO getPassTypesConfirmationDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLPassTypesConfirmationDAO();
            }
            else
            {
                return new MySQLPassTypesConfirmationDAO((MySqlConnection)dbConnection);
            }
        }
        public override SyncBufferDataHistDAO getSyncBufferDataHistDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLSyncBufferDataHistDAO();
            }
            else
            {
                return new MySQLSyncBufferDataHistDAO((MySqlConnection)dbConnection);
            }
        }
        public override SyncEmployeesHistDAO getSyncEmployeesHistDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLSyncEmployeesHistDAO();
            }
            else
            {
                return new MySQLSyncEmployeesHistDAO((MySqlConnection)dbConnection);
            }
        }
        public override SyncFinancialStructureHistDAO getSyncFinancialStructureHistDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLSyncFinancialStrictureHistDAO();
            }
            else
            {
                return new MySQLSyncFinancialStrictureHistDAO((MySqlConnection)dbConnection);
            }
        }
        public override SyncOrganizationalStructureHistDAO getSyncOrganizationalStructureHistDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLSyncOrganizationalStructureHistDAO();
            }
            else
            {
                return new MySQLSyncOrganizationalStructureHistDAO((MySqlConnection)dbConnection);
            }
        }

        public override SyncResponsibilityHistDAO getSyncResponsibilityHistDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLSyncResponsibilityHistDAO();
            }
            else
            {
                return new MySQLSyncResponsibilityHistDAO((MySqlConnection)dbConnection);
            }
        }

        public override SyncEmployeePositionHistDAO getSyncEmployeePositionHistDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLSyncEmployeePositionHistDAO();
            }
            else
            {
                return new MySQLSyncEmployeePositionHistDAO((MySqlConnection)dbConnection);
            }
        }

        public override SyncCostCenterHistDAO getSyncCostCenterHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLSyncCostCenterHistDAO();
            }
            else
            {
                return new MySQLSyncCostCenterHistDAO((MySqlConnection)dbConnection);
            }
        }

        public override SyncAnnualLeaveRecalcHistDAO getSyncAnnualLeaveRecalcHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLSyncAnnualLeaveRecalcHistDAO();
            }
            else
            {
                return new MySQLSyncAnnualLeaveRecalcHistDAO((MySqlConnection)dbConnection);
            }
        }

        public override SyncAnnualLeaveRecalcFlagDAO getSyncAnnualLeaveRecalcFlagDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLSyncAnnualLeaveRecalcFlagDAO();
            }
            else
            {
                return new MySQLSyncAnnualLeaveRecalcFlagDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeeResponsibilityDAO getEmployeeResponsibilityDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeResponsibilityDAO();
            }
            else
            {
                return new MySQLEmployeeResponsibilityDAO((MySqlConnection)dbConnection);
            }
        }
        public override EmployeeHistDAO getEmployeeHistDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeHistDAO();
            }
            else
            {
                return new MySQLEmployeeHistDAO((MySqlConnection)dbConnection);
            }
        }
        public override EmployeeAsco4HistDAO getEmployeeAsco4HistDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeeAsco4HistDAO();
            }
            else
            {
                return new MySQLEmployeeAsco4HistDAO((MySqlConnection)dbConnection);
            }
        }
        public override EmployeePYDataSumDAO getEmployeePYDataSumDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeePYDataSumDAO();
            }
            else
            {
                return new MySQLEmployeePYDataSumDAO((MySqlConnection)dbConnection);
            }
        }
        public override EmployeePYDataAnaliticalDAO getEmployeePYDataAnaliticalDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeePYDataAnaliticalDAO();
            }
            else
            {
                return new MySQLEmployeePYDataAnaliticalDAO((MySqlConnection)dbConnection);
            }
        }
        public override EmployeePYDataBufferDAO getEmployeePYDataBufferDAO(object dbConnection)
        {
            if (dbConnection != null)
            {
                return new MySQLEmployeePYDataBufferDAO();
            }
            else
            {
                return new MySQLEmployeePYDataBufferDAO((MySqlConnection)dbConnection);
            }
        }

        public override ApplUsersLoginChangesTblDAO getApplUsersLoginChangesTblDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLApplUsersLoginChangesTblDAO();
            }
            else
            {
                return new MySQLApplUsersLoginChangesTblDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeeLockedDayDAO getEmployeeLockedDayDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLEmployeeLockedDayDAO();
            }
            else
            {
                return new MySQLEmployeeLockedDayDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeeTypeVisibilityDAO getEmployeeTypeVisibilityDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLEmployeeTypeVisibilityDAO();
            }
            else
            {
                return new MySQLEmployeeTypeVisibilityDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeePositionDAO getEmployeePositionDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLEmployeePositionDAO();
            }
            else
            {
                return new MySQLEmployeePositionDAO((MySqlConnection)dbConnection);
            }
        }

        public override RiskDAO getRiskDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLRiskDAO();
            }
            else
            {
                return new MySQLRiskDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeeXRiskDAO getEmployeeXRiskDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLEmployeeXRiskDAO();
            }
            else
            {
                return new MySQLEmployeeXRiskDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeePositionXRiskDAO getEmployeePositionXRiskDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLEmployeePositionXRiskDAO();
            }
            else
            {
                return new MySQLEmployeePositionXRiskDAO((MySqlConnection)dbConnection);
            }
        }

        public override MedicalCheckPointDAO getMedicalCheckPointDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLMedicalCheckPointDAO();
            }
            else
            {
                return new MySQLMedicalCheckPointDAO((MySqlConnection)dbConnection);
            }
        }

        public override MedicalCheckVisitHdrDAO getMedicalCheckVisitHdrDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLMedicalCheckVisitHdrDAO();
            }
            else
            {
                return new MySQLMedicalCheckVisitHdrDAO((MySqlConnection)dbConnection);
            }
        }

        public override MedicalCheckVisitDtlDAO getMedicalCheckVisitDtlDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLMedicalCheckVisitDtlDAO();
            }
            else
            {
                return new MySQLMedicalCheckVisitDtlDAO((MySqlConnection)dbConnection);
            }
        }

        public override MedicalCheckVisitHdrHistDAO getMedicalCheckVisitHdrHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLMedicalCheckVisitHdrHistDAO();
            }
            else
            {
                return new MySQLMedicalCheckVisitHdrHistDAO((MySqlConnection)dbConnection);
            }
        }

        public override MedicalCheckVisitDtlHistDAO getMedicalCheckVisitDtlHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLMedicalCheckVisitDtlHistDAO();
            }
            else
            {
                return new MySQLMedicalCheckVisitDtlHistDAO((MySqlConnection)dbConnection);
            }
        }

        public override MedicalCheckDisabilityDAO getMedicalCheckDisabilityDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLMedicalCheckDisabilityDAO();
            }
            else
            {
                return new MySQLMedicalCheckDisabilityDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeesXMedicalCheckDisabilityDAO getEmployeesXMedicalCheckDisabilityDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLEmployeeXMedicalCheckDisabilityDAO();
            }
            else
            {
                return new MySQLEmployeeXMedicalCheckDisabilityDAO((MySqlConnection)dbConnection);
            }
        }

        public override VaccineDAO getVaccineDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLVaccineDAO();
            }
            else
            {
                return new MySQLVaccineDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeeXVaccineDAO getEmployeeXVaccineDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLEmployeeXVaccineDAO();
            }
            else
            {
                return new MySQLEmployeeXVaccineDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeePhysicalDataDAO getEmployeePhysicalDataDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLEmployeePhysicalDataDAO();
            }
            else
            {
                return new MySQLEmployeePhysicalDataDAO((MySqlConnection)dbConnection);
            }
        }

        public override SystemClosingEventDAO getSystemClosingEventDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLSystemClosingEventDAO();
            }
            else
            {
                return new MySQLSystemClosingEventDAO((MySqlConnection)dbConnection);
            }
        }        

        public override SystemMessageDAO getSystemMessageDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLSystemMessageDAO();
            }
            else
            {
                return new MySQLSystemMessageDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeeCounterMonthlyBalanceDAO getEmployeeCounterMonthlyBalanceDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLEmployeeCounterMonthlyBalanceDAO();
            }
            else
            {
                return new MySQLEmployeeCounterMonthlyBalanceDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeeCounterMonthlyBalanceHistDAO getEmployeeCounterMonthlyBalanceHistDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLEmployeeCounterMonthlyBalanceHistDAO();
            }
            else
            {
                return new MySQLEmployeeCounterMonthlyBalanceHistDAO((MySqlConnection)dbConnection);
            }
        }

        public override BufferMonthlyBalancePaidDAO getEmployeeCounterMonthlyBalancePaidDAO(Object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLBufferMonthlyBalancePaidDAO();
            }
            else
            {
                return new MySQLBufferMonthlyBalancePaidDAO((MySqlConnection)dbConnection);
            }
        }

        public override PassesAdditionalInfoDAO getPassesAdditionalInfoDAO(object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLPassesAdditionalInfoDAO();
            }
            else
            {
                return new MySQLPassesAdditionalInfoDAO((MySqlConnection)dbConnection);
            }
        }

        public override LogTmpAdditionalInfoDAO getLogTmpDAO(object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLLogTmpAdditionalInfoDAO();
            }
            else
            {
                return new MySQLLogTmpAdditionalInfoDAO((MySqlConnection)dbConnection);
            }
        }

        public override LogAdditionalInfoDAO getLogAdditionalInfoDAO(object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLLogAdditionalInfoDAO();
            }
            else
            {
                return new MySQLLogAdditionalInfoDAO((MySqlConnection)dbConnection);
            }
        }

        public override MealTypeAdditionalDataDAO getMealTypeAddDataDAO(object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLMealTypeAdditionalDataDAO();
            }
            else
            {
                return new MySQLMealTypeAdditionalDataDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeePYTransportTypeDAO getEmployeePYTransportTypeDAO(object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLEmployeePYTransportTypeDAO();
            }
            else
            {
                return new MySQLEmployeePYTransportTypeDAO((MySqlConnection)dbConnection);
            }
        }

        public override EmployeePYTransportDataDAO getEmployeePYTransportDataDAO(object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLEmployeePYTransportDataDAO();
            }
            else
            {
                return new MySQLEmployeePYTransportDataDAO((MySqlConnection)dbConnection);
            }
        }

        public override ApplUserLoginRFIDDAO getApplUserLoginRFIDDAO(object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLApplUserLoginRFIDDAO();
            }
            else
            {
                return new MySQLApplUserLoginRFIDDAO((MySqlConnection)dbConnection);
            }
        }

        public override ACSEventsDAO getACSEventsDAO(object dbConnection)
        {
            if (dbConnection == null)
            {
                return new MySQLACSEventsDAO();
            }
            else
            {
                return new MySQLACSEventsDAO((MySqlConnection)dbConnection);
            }
        }
    }
}
