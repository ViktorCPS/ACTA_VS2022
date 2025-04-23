using System;
using System.Configuration;
using Util;

namespace DataAccess
{
	/// <summary>
	/// Factory class in a case of using XML files as data source.
	/// </summary>
    public class XMLDAOFactory : DAOFactory
    {
        public static string DirectoryPath = Constants.XMLDataSourceDir;
        //public static string DirectoryPath = Constants.XMLDataSourceDir;

        public XMLDAOFactory()
        {

        }

        public override Object MakeNewDBConnection()
        {
            return null;
        }

        public override void CloseConnection()
        {
            // TODO: Add XMLDAOFactory.CloseConnection implementation
        }

        public override void CloseConnection(Object dbConnection)
        {
            // TODO: Add XMLDAOFactory.CloseConnection implementation
        }

        public override bool TestDataSourceConnection()
        {
            // TODO: Add XMLDAOFactory.TestDataSourceConnection implementation
            return false;
        }
        public override bool TestDataSourceConnection(Object dbConnection)
        {
            // TODO: Add XMLDAOFactory.TestDataSourceConnection implementation
            return false;
        }

        public override int getDBConnectedHosts()
        {
            return 0;
        }
        //  06.02.2020. BOJAN
        // ATTENTION!!! TODO: Must be changed before used
        public override MachineDAO getMachineDAO(object dbConnection)
        {
            return null;
        }

        public override string getDBServerName()
        {
            return null;
        }

        public override string getDBName()
        {
            return null;
        }

        public override string getDBServerPort()
        {
            return null;
        }

        public override SyncWithNavDAO getSyncWithNavDAO(object dbConnection)
        {
            throw new NotImplementedException("DataAccess.XMLDAOFactory.getSyncWithNavDAO()");//return new XMLSyncWithNavDAO();
        }

        public override EmployeeDAO getEmployeeDAO(object dbConnection)
        {
            return new XMLEmployeeDAO();
        }
        public override IOPairProcessedDAO getIOPairProcessedDAO(object dbConnection)
        {
            return null;
        }

        public override LocationDAO getLocationDAO(object dbConnection)
        {
            return new XMLLocationDAO();
        }

        public override WorkingUnitsDAO getWorkingUnitsDAO(object dbConnection)
        {
            return new XMLWorkingUnitsDAO();
        }

        public override PassTypeDAO getPassTypeDAO(object dbConnection)
        {
            return new XMLPassTypeDAO();
        }

        public override AddressDAO getAddressDAO(object dbConnection)
        {
            return null;
        }
        public override TimeSchemaIntervalLibraryDAO getTimeSchemaIntervalLibraryDAO(object dbConnection)
        {
            return null;
        }
        public override TimeSchemaIntervalLibraryDtlDAO getTimeSchemaIntervalLibraryDtlDAO(object dbConnection)
        {
            return null;
        }
        public override ReaderDAO getReaderDAO(object dbConnection)
        {
            return new XMLReaderDAO();
        }

        public override TagDAO getTagDAO(object dbConnection)
        {
            return new XMLTagDAO();
        }

        public override PassDAO getPassDAO(object dbConnection)
        {
            return new XMLPassDAO();
        }

        public override PassHistDAO getPassHistDAO(object dbConnection)
        {
            return null;
        }
        public override VisitDAO getVisitDAO(object dbConnection)
        {
            return new XMLVisitDAO();
        }

        public override CitizenDAO getCitizenDAO(object dbConnection)
        {
            return new XMLCitizenDAO();
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override LogDAO getLogDAO(object dbConnection)
        {
            return new XMLLogDAO();
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override IOPairDAO getIOPairDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override WorkTimeSchemaDAO getWorkTimeSchemaDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override WorkingGroupDAO getWorkingGroupDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override EmployeeTimeScheduleDAO getEmployeeTimeScheduleDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override EmployeeAbsenceDAO getEmployeeAbsenceDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override EmployeeLocationDAO getEmployeeLocationDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override GateDAO getGateDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override ApplUsersXWUDAO getApplUsersXWUDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override ApplUserDAO getApplUserDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override ApplRoleDAO getApplRoleDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override ApplMenuItemDAO getApplMenuItemDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override ApplUsersXRoleDAO getApplUsersXRoleDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override ApplUsersLogDAO getApplUsersLogDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override ExitPermissionDAO getExitPermissionsDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override HolidayDAO getHolidayDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override EmployeeGroupAccessControlDAO getEmployeeGroupAccessControlDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override TimeAccessProfileDAO getTimeAccessProfileDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override TimeAccessProfileDtlDAO getTimeAccessProfileDtlDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override IOPairsProcessedHistDAO getIOPairsProcessedHistDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override GateTimeAccessProfileDAO getGateTimeAccessProfileDAO(object dbConnection)
        {
            return null;
        }

        public override ApplUsersLoginChangesTblDAO getApplUsersLoginChangesTblDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override AccessGroupXGateDAO getAccessGroupXGateDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override ExtraHourDAO getExtraHourDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override ExtraHourUsedDAO getExtraHourUsedDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override TimeSchemaPauseDAO getTimeSchemaPauseDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override AccessControlFileDAO getAccessControlFileDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override EmployeeImageFileDAO getEmployeeImageFileDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override VisitorDocFileDAO getVisitorDocFileDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override CameraSnapshotFileDAO getCameraSnapshotFileDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override CameraDAO getCameraDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override CamerasXReadersDAO getCamerasXReadersDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override EmployeeGroupsTimeScheduleDAO getEmployeeGroupsTimeScheduleDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override GateSyncDAO getGateSyncDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override LicenceDAO getLicenceDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override MealTypeDAO getMealTypeDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override MealAssignedDAO getMealAssignedDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override MealsPointDAO getMealsPointDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override OnlineMealsPointsDAO getOnlineMealsPointDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override OnlineMealsRestaurantDAO getOnlineMealsRestaurantDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override OnlineMealsIntervalsDAO getOnlineMealsIntervalsDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override OnlineMealsTypesDAO getOnlineMealsTypesDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override OnlineMealsUsedDAO getOnlineMealsUsedDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override OnlineMealsUsedDailyDAO getOnlineMealsUsedDailyDAO(object dbConnection)
        {
            return null;
        }

        // ATTENTION!!! TODO: Must be changed before used
        public override OnlineMealsUsedHistDAO getOnlineMealsUsedHistDAO(object dbConnection)
        {
            return null;
        }
        public override bool TestDBConnectionDummySelect(Object dbConnection)
        {
            return false;
        } 
        // ATTENTION!!! TODO: Must be changed before used
        public override MealUsedDAO getMealUsedDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override MealTypeEmplDAO getMealTypeEmplDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override EmployeeXMealTypeEmplDAO getEmployeeXMealTypeEmplDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override SecurityRouteDAO getSecurityRouteDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override SecurityRouteScheduleDAO getSecurityRouteScheduleDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override SecurityRoutesEmployeeDAO getSecurityRoutesEmployeeDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override SecurityRoutesLogDAO getSecurityRoutesLogDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override SecurityRoutesPointDAO getSecurityRoutesPointDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override SecurityRoutesReaderDAO getSecurityRoutesReaderDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override MapDAO getMapDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override MapsObjectDAO getMapsObjectDAO(object dbConnection)
        {
            return null;
        }
        // ATTENTION!!! TODO: Must be changed before used
        public override EmployeeVacEvidDAO getEmployeeVacEvidDAO(object dbConnection)
        {
            return null;
        }
        public override LockDAO getLockDAO(object dbConnection)
        {
            return null;
        }
        public override FilterDAO getFilterDAO(object dbConnection)
        {
            return null;
        }
        public override UnipromButtonDAO getUnipromButtonDAO(object dbConnection)
        {
            return null;
        }
        public override UnipromButtonLogDAO getUnipromButtonLogDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeeAsco4DAO getEmployeeAsco4DAO(object dbConnection)
        {
            return null;
        }
        public override VisitAsco4DAO getVisitAsco4DAO(object dbConnection)
        {
            return null;
        }
        public override ZINApbDAO getZINApbDAO(object dbConnection)
        {
            return null;
        }
        public override MealsTypeScheduleDAO getMealsTypeScheduleDAO(object dbConnection)
        {
            return null;
        }

        public override MealsEmployeeScheduleDAO getMealsEmployeeScheduleDAO(object dbConnection)
        {
            return null;
        }
        public override MealsWorkingUnitScheduleDAO getMealsWorkingUnitScheduleDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeeAsco4MetadataDAO getEmployeeAsco4MetadataDAO(object dbConnection)
        {
            return null;
        }
        public override ResultDAO getResultDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeeCounterTypeDAO getEmployeeCounterTypeDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeeCounterValueDAO getEmployeeCounterValueDAO(object dbConnection)
        {
            return null;
        }
        public override RuleDAO getRuleDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeeTypeDAO getEmployeeTypeDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeeLoanDAO getEmployeeLoanDAO(object dbConnection)
        {
            return null;
        }
        public override PassTypeLimitDAO getPassTypeLimitDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeeCounterValueHistDAO getEmployeeCounterValueHistDAO(object dbConnection)
        {
            return null;
        }
        public override HolidaysExtendedDAO getHolidaysExtendedDAO(object dbConnection)
        {
            return null;
        }
        public override ApplUserCategoryDAO getApplUserCategoryDAO(object dbConnection)
        {
            return null;
        }
        public override ApplUserXApplUserCategoryDAO getApplUserXCategoryDAO(object dbConnection)
        {
            return null;
        }
        public override ApplUserCategoryXPassTypeDAO getApplUserCategoryXPassTypeDAO(object dbConnection)
        {
            return null;
        }
        public override OrganizationalUnitDAO getOrganizationalUnitDAO(object dbConnection)
        {
            return null;
        }
        public override WorkingUnitXOrganizationalUnitDAO getWorkingUnitXOrganizationalUnitDAO(object dbConnection)
        {
            return null;
        }
        public override ApplUserXOrgUnitDAO getApplUserXOrgUnitDAO(object dbConnection)
        {
            return null;
        }


        public override PassTypesConfirmationDAO getPassTypesConfirmationDAO(object dbConnection)
        {
            return null;
        }
        public override SyncBufferDataHistDAO getSyncBufferDataHistDAO(object dbConnection)
        {
            return null;
        }
        public override SyncEmployeePositionHistDAO getSyncEmployeePositionHistDAO(object dbConnection)
        {
            return null;
        }
        public override SyncEmployeesHistDAO getSyncEmployeesHistDAO(object dbConnection)
        {
            return null;
        }
        public override SyncFinancialStructureHistDAO getSyncFinancialStructureHistDAO(object dbConnection)
        {
            return null;
        }
        public override SyncCostCenterHistDAO getSyncCostCenterHistDAO(object dbConnection)
        {
            return null;
        }
        public override SyncAnnualLeaveRecalcHistDAO getSyncAnnualLeaveRecalcHistDAO(object dbConnection)
        {
            return null;
        }
        public override SyncAnnualLeaveRecalcFlagDAO getSyncAnnualLeaveRecalcFlagDAO(object dbConnection)
        {
            return null;
        }
        public override SyncOrganizationalStructureHistDAO getSyncOrganizationalStructureHistDAO(object dbConnection)
        {
            return null;
        }
        public override SyncResponsibilityHistDAO getSyncResponsibilityHistDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeeResponsibilityDAO getEmployeeResponsibilityDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeeHistDAO getEmployeeHistDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeeAsco4HistDAO getEmployeeAsco4HistDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeePYDataSumDAO getEmployeePYDataSumDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeePYDataAnaliticalDAO getEmployeePYDataAnaliticalDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeePYDataBufferDAO getEmployeePYDataBufferDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeeLockedDayDAO getEmployeeLockedDayDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeeTypeVisibilityDAO getEmployeeTypeVisibilityDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeePositionDAO getEmployeePositionDAO(Object dbConnection)
        {
            return null;
        }
        public override RiskDAO getRiskDAO(Object dbConnection)
        {
            return null;
        }
        public override EmployeeXRiskDAO getEmployeeXRiskDAO(Object dbConnection)
        {
            return null;
        }
        public override EmployeePositionXRiskDAO getEmployeePositionXRiskDAO(Object dbConnection)
        {
            return null;
        }
        public override MedicalCheckPointDAO getMedicalCheckPointDAO(Object dbConnection)
        {
            return null;
        }
        public override MedicalCheckVisitHdrDAO getMedicalCheckVisitHdrDAO(Object dbConnection)
        {
            return null;
        }
        public override MedicalCheckVisitDtlDAO getMedicalCheckVisitDtlDAO(Object dbConnection)
        {
            return null;
        }
        public override MedicalCheckVisitHdrHistDAO getMedicalCheckVisitHdrHistDAO(Object dbConnection)
        {
            return null;
        }
        public override MedicalCheckVisitDtlHistDAO getMedicalCheckVisitDtlHistDAO(Object dbConnection)
        {
            return null;
        }
        public override MedicalCheckDisabilityDAO getMedicalCheckDisabilityDAO(Object dbConnection)
        {
            return null;
        }
        public override EmployeesXMedicalCheckDisabilityDAO getEmployeesXMedicalCheckDisabilityDAO(Object dbConnection)
        {
            return null;
        }
        public override VaccineDAO getVaccineDAO(Object dbConnection)
        {
            return null;
        }
        public override EmployeeXVaccineDAO getEmployeeXVaccineDAO(Object dbConnection)
        {
            return null;
        }
        public override EmployeePhysicalDataDAO getEmployeePhysicalDataDAO(Object dbConnection)
        {
            return null;
        }
        public override SystemClosingEventDAO getSystemClosingEventDAO(object dbConnection)
        {
            return null;
        }
        public override SystemMessageDAO getSystemMessageDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeeCounterMonthlyBalanceDAO getEmployeeCounterMonthlyBalanceDAO(object dbConnection)
        {
            return null;
        }
        public override EmployeeCounterMonthlyBalanceHistDAO getEmployeeCounterMonthlyBalanceHistDAO(object dbConnection)
        {
            return null;
        }
        public override BufferMonthlyBalancePaidDAO getEmployeeCounterMonthlyBalancePaidDAO(object dbConnection)
        {
            return null;
        }

        public override PassesAdditionalInfoDAO getPassesAdditionalInfoDAO(object dbConnection)
        {
            return null;
        }

        public override LogTmpAdditionalInfoDAO getLogTmpDAO(object dbConnection)
        {
            return null;
        }

        public override LogAdditionalInfoDAO getLogAdditionalInfoDAO(object dbConnection)
        {
            return null;
        }

        public override MealTypeAdditionalDataDAO getMealTypeAddDataDAO(object dbConnection)
        {
            return null;
        }

        public override EmployeePYTransportTypeDAO getEmployeePYTransportTypeDAO(object dbConnection)
        {
            return null;
        }

        public override EmployeePYTransportDataDAO getEmployeePYTransportDataDAO(object dbConnection)
        {
            return null;
        }

        public override ApplUserLoginRFIDDAO getApplUserLoginRFIDDAO(object dbConnection)
        {
            return null;
        }

        public override ACSEventsDAO getACSEventsDAO(object dbConnection)
        {
            throw new NotImplementedException();
        }

        public override DocumentsDAO getDocumentsDAO(object dbConnection)
        {
            throw new NotImplementedException();
        }
    }
}
