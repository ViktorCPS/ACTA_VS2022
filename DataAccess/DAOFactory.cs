using System;
using System.Configuration;
using System.Security.Cryptography;
using Util;

namespace DataAccess
{
	/// <summary>
	/// DAOFactory abstract class
	/// </summary>
	public abstract class DAOFactory
	{
        public static bool threadSafe = false;

        public abstract SyncWithNavDAO getSyncWithNavDAO(Object dbConnection);
        public abstract EmployeeDAO getEmployeeDAO(Object dbConnection);
        public abstract DocumentsDAO getDocumentsDAO(Object dbConnection);
        public abstract LocationDAO getLocationDAO(Object dbConnection);
        public abstract WorkingUnitsDAO getWorkingUnitsDAO(Object dbConnection);
        public abstract PassTypeDAO getPassTypeDAO(Object dbConnection);
        public abstract AddressDAO getAddressDAO(Object dbConnection);
        public abstract MealsTypeScheduleDAO getMealsTypeScheduleDAO(Object dbConnection);
        public abstract ReaderDAO getReaderDAO(Object dbConnection);
        public abstract TagDAO getTagDAO(Object dbConnection);
        public abstract PassDAO getPassDAO(Object dbConnection);
        public abstract PassHistDAO getPassHistDAO(Object dbConnection);
        public abstract PassesAdditionalInfoDAO getPassesAdditionalInfoDAO(Object dbConnection);
        public abstract VisitDAO getVisitDAO(Object dbConnection);
        public abstract CitizenDAO getCitizenDAO(Object dbConnection);
        public abstract LogDAO getLogDAO(Object dbConnection);
        public abstract LogTmpAdditionalInfoDAO getLogTmpDAO(Object dbConnection);
        public abstract LogAdditionalInfoDAO getLogAdditionalInfoDAO(Object dbConnection);
        public abstract IOPairDAO getIOPairDAO(Object dbConnection);
        public abstract WorkTimeSchemaDAO getWorkTimeSchemaDAO(Object dbConnection);
        public abstract WorkingGroupDAO getWorkingGroupDAO(Object dbConnection);
        public abstract EmployeeTimeScheduleDAO getEmployeeTimeScheduleDAO(Object dbConnection);
        public abstract EmployeeAbsenceDAO getEmployeeAbsenceDAO(Object dbConnection);
        public abstract EmployeeLocationDAO getEmployeeLocationDAO(Object dbConnection);
        public abstract GateDAO getGateDAO(Object dbConnection);
        public abstract ApplUsersXWUDAO getApplUsersXWUDAO(Object dbConnection);
        public abstract ApplUserDAO getApplUserDAO(Object dbConnection);
        public abstract ApplUsersLogDAO getApplUsersLogDAO(Object dbConnection);
        public abstract ApplRoleDAO getApplRoleDAO(Object dbConnection);
        public abstract ApplMenuItemDAO getApplMenuItemDAO(Object dbConnection);
        public abstract ApplUsersXRoleDAO getApplUsersXRoleDAO(Object dbConnection);
        public abstract ExitPermissionDAO getExitPermissionsDAO(Object dbConnection);
        public abstract HolidayDAO getHolidayDAO(Object dbConnection);
        public abstract EmployeeGroupAccessControlDAO getEmployeeGroupAccessControlDAO(Object dbConnection);
        public abstract TimeAccessProfileDAO getTimeAccessProfileDAO(Object dbConnection);
        public abstract TimeAccessProfileDtlDAO getTimeAccessProfileDtlDAO(Object dbConnection);
        public abstract GateTimeAccessProfileDAO getGateTimeAccessProfileDAO(Object dbConnection);
        public abstract AccessGroupXGateDAO getAccessGroupXGateDAO(Object dbConnection);
        public abstract ExtraHourDAO getExtraHourDAO(Object dbConnection);
        public abstract ExtraHourUsedDAO getExtraHourUsedDAO(Object dbConnection);
        public abstract TimeSchemaPauseDAO getTimeSchemaPauseDAO(Object dbConnection);
        public abstract AccessControlFileDAO getAccessControlFileDAO(Object dbConnection);
        public abstract EmployeeImageFileDAO getEmployeeImageFileDAO(Object dbConnection);
        public abstract VisitorDocFileDAO getVisitorDocFileDAO(Object dbConnection);
        public abstract CameraSnapshotFileDAO getCameraSnapshotFileDAO(Object dbConnection);
        public abstract CameraDAO getCameraDAO(Object dbConnection);
        public abstract CamerasXReadersDAO getCamerasXReadersDAO(Object dbConnection);
        public abstract EmployeeGroupsTimeScheduleDAO getEmployeeGroupsTimeScheduleDAO(Object dbConnection);
        public abstract GateSyncDAO getGateSyncDAO(Object dbConnection);
        public abstract LicenceDAO getLicenceDAO(Object dbConnection);
        public abstract MealTypeDAO getMealTypeDAO(Object dbConnection);
        public abstract MealTypeAdditionalDataDAO getMealTypeAddDataDAO(Object dbConnection);
        public abstract MealAssignedDAO getMealAssignedDAO(Object dbConnection);
        public abstract MealsPointDAO getMealsPointDAO(Object dbConnection);
        public abstract OnlineMealsPointsDAO getOnlineMealsPointDAO(Object dbConnection);
        public abstract OnlineMealsRestaurantDAO getOnlineMealsRestaurantDAO(Object dbConnection);
        public abstract OnlineMealsTypesDAO getOnlineMealsTypesDAO(Object dbConnection);
        public abstract OnlineMealsUsedDAO getOnlineMealsUsedDAO(Object dbConnection);
        public abstract OnlineMealsUsedHistDAO getOnlineMealsUsedHistDAO(Object dbConnection);
        public abstract OnlineMealsIntervalsDAO getOnlineMealsIntervalsDAO(Object dbConnection);
        public abstract MealUsedDAO getMealUsedDAO(Object dbConnection);
        public abstract MealTypeEmplDAO getMealTypeEmplDAO(Object dbConnection);
        public abstract EmployeeXMealTypeEmplDAO getEmployeeXMealTypeEmplDAO(Object dbConnection);
        public abstract SecurityRouteDAO getSecurityRouteDAO(Object dbConnection);
        public abstract SecurityRouteScheduleDAO getSecurityRouteScheduleDAO(Object dbConnection);
        public abstract SecurityRoutesPointDAO getSecurityRoutesPointDAO(Object dbConnection);
        public abstract SecurityRoutesReaderDAO getSecurityRoutesReaderDAO(Object dbConnection);
        public abstract SecurityRoutesEmployeeDAO getSecurityRoutesEmployeeDAO(Object dbConnection);
        public abstract SecurityRoutesLogDAO getSecurityRoutesLogDAO(Object dbConnection);
        public abstract MapDAO getMapDAO(Object dbConnection);
        public abstract MapsObjectDAO getMapsObjectDAO(Object dbConnection);
        public abstract EmployeeVacEvidDAO getEmployeeVacEvidDAO(Object dbConnection);
        public abstract LockDAO getLockDAO(Object dbConnection);
        public abstract FilterDAO getFilterDAO(Object dbConnection);
        public abstract UnipromButtonDAO getUnipromButtonDAO(Object dbConnection);
        public abstract UnipromButtonLogDAO getUnipromButtonLogDAO(Object dbConnection);
        public abstract EmployeeAsco4DAO getEmployeeAsco4DAO(Object dbConnection);
        public abstract ZINApbDAO getZINApbDAO(Object dbConnection);
        public abstract MachineDAO getMachineDAO(Object dbConnection); //   06.02.2020. BOJAN
        public abstract VisitAsco4DAO getVisitAsco4DAO(Object dbConnection);
        public abstract MealsEmployeeScheduleDAO getMealsEmployeeScheduleDAO(Object dbConnection);
        public abstract MealsWorkingUnitScheduleDAO getMealsWorkingUnitScheduleDAO(Object dbConnection);
        public abstract EmployeeAsco4MetadataDAO getEmployeeAsco4MetadataDAO(Object dbConnection);
        public abstract ResultDAO getResultDAO(Object dbConnection);
        public abstract IOPairProcessedDAO getIOPairProcessedDAO(Object dbConnection);
        public abstract EmployeeCounterTypeDAO getEmployeeCounterTypeDAO(Object dbConnection);
        public abstract EmployeeCounterValueDAO getEmployeeCounterValueDAO(Object dbConnection);
        public abstract RuleDAO getRuleDAO(Object dbConnection);
        public abstract EmployeeTypeDAO getEmployeeTypeDAO(Object dbConnection);
        public abstract EmployeeLoanDAO getEmployeeLoanDAO(Object dbConnection);
        public abstract IOPairsProcessedHistDAO getIOPairsProcessedHistDAO(Object dbConnection);
        public abstract PassTypeLimitDAO getPassTypeLimitDAO(Object dbConnection);
        public abstract EmployeeCounterValueHistDAO getEmployeeCounterValueHistDAO(Object dbConnection);
        public abstract HolidaysExtendedDAO getHolidaysExtendedDAO(Object dbConnection);
        public abstract ApplUserCategoryDAO getApplUserCategoryDAO(Object dbConnection);
        public abstract ApplUserXApplUserCategoryDAO getApplUserXCategoryDAO(Object dbConnection);
        public abstract ApplUserCategoryXPassTypeDAO getApplUserCategoryXPassTypeDAO(Object dbConnection);
        public abstract OrganizationalUnitDAO getOrganizationalUnitDAO(Object dbConnection);
        public abstract WorkingUnitXOrganizationalUnitDAO getWorkingUnitXOrganizationalUnitDAO(Object dbConnection);
        public abstract ApplUserXOrgUnitDAO getApplUserXOrgUnitDAO(Object dbConnection);
        public abstract PassTypesConfirmationDAO getPassTypesConfirmationDAO(Object dbConnection);
        public abstract SyncBufferDataHistDAO getSyncBufferDataHistDAO(Object dbConnection);
        public abstract SyncEmployeesHistDAO getSyncEmployeesHistDAO(Object dbConnection);
        public abstract SyncFinancialStructureHistDAO getSyncFinancialStructureHistDAO(Object dbConnection);
        public abstract SyncCostCenterHistDAO getSyncCostCenterHistDAO(Object dbConnection);
        public abstract SyncOrganizationalStructureHistDAO getSyncOrganizationalStructureHistDAO(Object dbConnection);
        public abstract SyncResponsibilityHistDAO getSyncResponsibilityHistDAO(Object dbConnection);
        public abstract SyncAnnualLeaveRecalcHistDAO getSyncAnnualLeaveRecalcHistDAO(Object dbConnection);
        public abstract SyncEmployeePositionHistDAO getSyncEmployeePositionHistDAO(Object dbConnection);
        public abstract EmployeeResponsibilityDAO getEmployeeResponsibilityDAO(Object dbConnection);
        public abstract EmployeeHistDAO getEmployeeHistDAO(Object dbConnection);
        public abstract EmployeeAsco4HistDAO getEmployeeAsco4HistDAO(Object dbConnection);
        public abstract EmployeePYDataSumDAO getEmployeePYDataSumDAO(Object dbConnection);
        public abstract EmployeePYDataAnaliticalDAO getEmployeePYDataAnaliticalDAO(Object dbConnection);
        public abstract EmployeePYDataBufferDAO getEmployeePYDataBufferDAO(Object dbConnection);
        public abstract ApplUsersLoginChangesTblDAO getApplUsersLoginChangesTblDAO(Object dbConnection);
        public abstract EmployeeLockedDayDAO getEmployeeLockedDayDAO(Object dbConnection);
        public abstract EmployeeTypeVisibilityDAO getEmployeeTypeVisibilityDAO(Object dbConnection);
        public abstract OnlineMealsUsedDailyDAO getOnlineMealsUsedDailyDAO(Object dbConnection);
        public abstract EmployeePositionDAO getEmployeePositionDAO(Object dbConnection);
        public abstract RiskDAO getRiskDAO(Object dbConnection);
        public abstract EmployeeXRiskDAO getEmployeeXRiskDAO(Object dbConnection);
        public abstract EmployeePositionXRiskDAO getEmployeePositionXRiskDAO(Object dbConnection);
        public abstract MedicalCheckPointDAO getMedicalCheckPointDAO(Object dbConnection);
        public abstract MedicalCheckVisitHdrDAO getMedicalCheckVisitHdrDAO(Object dbConnection);
        public abstract MedicalCheckVisitDtlDAO getMedicalCheckVisitDtlDAO(Object dbConnection);
        public abstract MedicalCheckVisitHdrHistDAO getMedicalCheckVisitHdrHistDAO(Object dbConnection);
        public abstract MedicalCheckVisitDtlHistDAO getMedicalCheckVisitDtlHistDAO(Object dbConnection);
        public abstract MedicalCheckDisabilityDAO getMedicalCheckDisabilityDAO(Object dbConnection);
        public abstract EmployeesXMedicalCheckDisabilityDAO getEmployeesXMedicalCheckDisabilityDAO(Object dbConnection);
        public abstract VaccineDAO getVaccineDAO(Object dbConnection);
        public abstract EmployeeXVaccineDAO getEmployeeXVaccineDAO(Object dbConnection);
        public abstract EmployeePhysicalDataDAO getEmployeePhysicalDataDAO(Object dbConnection);
        public abstract SystemClosingEventDAO getSystemClosingEventDAO(Object dbConnection);
        public abstract SystemMessageDAO getSystemMessageDAO(Object dbConnection);
        public abstract EmployeeCounterMonthlyBalanceDAO getEmployeeCounterMonthlyBalanceDAO(Object dbConnection);
        public abstract EmployeeCounterMonthlyBalanceHistDAO getEmployeeCounterMonthlyBalanceHistDAO(Object dbConnection);
        public abstract BufferMonthlyBalancePaidDAO getEmployeeCounterMonthlyBalancePaidDAO(Object dbConnection);
        public abstract TimeSchemaIntervalLibraryDAO getTimeSchemaIntervalLibraryDAO(Object dbConnection);
        public abstract TimeSchemaIntervalLibraryDtlDAO getTimeSchemaIntervalLibraryDtlDAO(Object dbConnection);
        public abstract SyncAnnualLeaveRecalcFlagDAO getSyncAnnualLeaveRecalcFlagDAO(Object dbConnection);
        public abstract EmployeePYTransportTypeDAO getEmployeePYTransportTypeDAO(Object dbConnection);
        public abstract EmployeePYTransportDataDAO getEmployeePYTransportDataDAO(Object dbConnection);
        public abstract ApplUserLoginRFIDDAO getApplUserLoginRFIDDAO(Object dbConnection);
        public abstract ACSEventsDAO getACSEventsDAO(Object dbConnection);
        public abstract bool TestDataSourceConnection();
        public abstract bool TestDataSourceConnection(Object dbConnection);
		public abstract void CloseConnection();
        public abstract void CloseConnection(Object dbConnection);
        public abstract Object MakeNewDBConnection();
        public abstract bool TestDBConnectionDummySelect(Object dbConnection);
        public abstract int getDBConnectedHosts();
        public abstract string getDBServerName();
        public abstract string getDBServerPort();
        public abstract string getDBName();

		public static DAOFactory getDAOFactory(int whichFactory)
		{
            try
            {
                string XMLAlternative = "NO";
                try
                {
                    XMLAlternative = (ConfigurationManager.AppSettings["XMLAlternative"]).ToUpper();
                }
                catch { }

                switch (whichFactory)
                {
                    case 1:
                        {
                            MSSQLDAOFactory mssqlDAOFactory = new MSSQLDAOFactory();
                            if (mssqlDAOFactory.TestDataSourceConnection() || (XMLAlternative == "NO"))
                            {
                                return mssqlDAOFactory;
                            }
                            else
                            {
                                return new XMLDAOFactory();
                            }
                        }
                    case 2:
                    //return new OracleDAOFactory();
                    case 3:
                        {
                            MySQLDAOFactory mysqlDAOFactory = new MySQLDAOFactory();

                            if (mysqlDAOFactory.TestDataSourceConnection() || (XMLAlternative == "NO"))
                            {
                                return mysqlDAOFactory;
                            }
                            else
                            {
                                return new XMLDAOFactory();
                            }
                        }
                    case 4:
                        return new XMLDAOFactory();

                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating database layer -> getDAOFactory: " + ex.Message);
            }
		}

        // Same as getDAOFactory(int whichFactory), no Data Provider parameter
        // Data Provider is found from conection string
        // If Data Provider can not be found, try to get it from App.config and call previous method
        public static DAOFactory getDAOFactory()
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
                    XMLAlternative = (ConfigurationManager.AppSettings["XMLAlternative"]).ToUpper();
                }
                catch { }

                try
                {
                    connectionString = ConfigurationManager.AppSettings["connectionString"];
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

                        Util.Misc.configAdd("connectionString", connStringCrypted);
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
                            MSSQLDAOFactory mssqlDAOFactory = new MSSQLDAOFactory();
                            if (mssqlDAOFactory.TestDataSourceConnection() || (XMLAlternative == "NO"))
                            {
                                return mssqlDAOFactory;
                            }
                            else
                            {
                                return new XMLDAOFactory();
                            }
                        }
                    case "oracle":
                    //return new OracleDAOFactory();
                    case "mysql":
                        {
                            MySQLDAOFactory mysqlDAOFactory = new MySQLDAOFactory();

                            if (mysqlDAOFactory.TestDataSourceConnection() || (XMLAlternative == "NO"))
                            {
                                return mysqlDAOFactory;
                            }
                            else
                            {
                                return new XMLDAOFactory();
                            }
                        }
                    case "xml":
                        return new XMLDAOFactory();
                    case "":
                        return getDAOFactory(Int32.Parse(ConfigurationManager.AppSettings["DataProvider"]));
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating database layer -> getDAOFactory: " + ex.Message);
            }
        }

        // No testing database connection, used for objects which will call methods who create their own connections
        public static DAOFactory getDAOFactoryWithoutTestConnection()
        {
            try
            {
                string connectionString = "";
                int startIndex = -1;
                int endIndex = -1;
                string dataProvider = "";

                try
                {
                    connectionString = ConfigurationManager.AppSettings["connectionString"];
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

                        Util.Misc.configAdd("connectionString", connStringCrypted);
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
                            MSSQLDAOFactory mssqlDAOFactory = new MSSQLDAOFactory();
                            return mssqlDAOFactory;
                        }
                    case "oracle":
                    //return new OracleDAOFactory();
                    case "mysql":
                        {
                            MySQLDAOFactory mysqlDAOFactory = new MySQLDAOFactory();
                            return mysqlDAOFactory;
                        }
                    case "xml":
                        return new XMLDAOFactory();
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating database layer -> getDAOFactoryWithoutTestConnection: " + ex.Message);
            }
        }
    }
}
