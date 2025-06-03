using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.IO;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Text;
using System.Security.Cryptography;

using Common;
using ReaderManagement;
using Reports;
using UI;
using Util;
using TransferObjects;
using ACTAConfigManipulation;
//using SiemensUI;
using System.Collections.Generic;

namespace ACTAAdmin
{
    /// <summary>
    /// Summary description for ACTA.
    /// </summary>
    public class ACTA : System.Windows.Forms.Form
    {
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuEmployees;
        private System.Windows.Forms.MenuItem menuWorkingUnits;
        private System.Windows.Forms.MenuItem menuLocations;
        private System.Windows.Forms.MenuItem menuLibraries;
        private System.Windows.Forms.MenuItem menuPassTypes;
        private IContainer components;
        private System.Windows.Forms.MenuItem menuReaders;
        private System.Windows.Forms.MenuItem menuReports;
        private CultureInfo culture;

        DebugLog log;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuMaps;
        private System.Windows.Forms.MenuItem menuMaintaining;
        private System.Windows.Forms.MenuItem menuInterventions;
        private System.Windows.Forms.MenuItem menuHelp;
        private System.Windows.Forms.MenuItem menuImportData;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuLogToPasses;
        private System.Windows.Forms.MenuItem menuPassesToIOPairs;
        private System.Windows.Forms.MenuItem menuImportLog;
        private System.Windows.Forms.MenuItem menuProcessingTables;
        private System.Windows.Forms.MenuItem menuLibExport;
        private System.Windows.Forms.MenuItem menuIOPairs;
        private System.Windows.Forms.MenuItem menuPasses;
        private System.Windows.Forms.MenuItem menuEmployeeAbsences;
        private System.Windows.Forms.MenuItem menuAbout;
        private System.Windows.Forms.MenuItem menuGates;
        private System.Windows.Forms.MenuItem menuConfiguration;
        private System.Windows.Forms.MenuItem menuDataAccess;
        private System.Windows.Forms.MenuItem menuUserRoles;
        private System.Windows.Forms.MenuItem menuUsers;
        private System.Windows.Forms.MenuItem menuRoles;
        private System.Windows.Forms.MenuItem menuUsersRoles;
        private System.Windows.Forms.MenuItem menuRoleMaintainence;
        private System.Windows.Forms.MenuItem menuRolePrivileges;


        // Log In User
        private ApplUserTO _currentUser = null;

        // Roles for Log In User
        List<ApplRoleTO> currentRoles;

        // Array of Menu Item Permissions
        int[] itemPermissions;

        // Hashtable of all Menu Item Permissions contining inividual Menu Item Permissions
        Hashtable menuItemsPermissions;

        // Controller instance
        public NotificationController Controller;

        // Observer client instance
        public NotificationObserverClient observerClient;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.StatusBar ACTAStatusBar;
        private System.Windows.Forms.StatusBarPanel sbPanelDate;
        private System.Windows.Forms.StatusBarPanel sbPanelUser;
        private System.Windows.Forms.StatusBarPanel sbPanelLogInTime;
        private System.Windows.Forms.MenuItem menuExitPermissions;
        private System.Windows.Forms.PictureBox pbClientLogo;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.MenuItem menuHolidays;
        private System.Windows.Forms.MenuItem menuWT;
        private System.Windows.Forms.MenuItem menuWTSchema;
        private System.Windows.Forms.MenuItem menuStandardRep;
        private System.Windows.Forms.MenuItem menuMittalRep;
        private System.Windows.Forms.MenuItem menuEmplAccess;
        private System.Windows.Forms.MenuItem menuPresence;
        private System.Windows.Forms.MenuItem menuRepLoc;
        private System.Windows.Forms.MenuItem menuRepWU;
        private System.Windows.Forms.MenuItem menuRepEmpl;
        private System.Windows.Forms.MenuItem menuPresenceEmpl;
        private System.Windows.Forms.MenuItem menuTimeSchedule;
        private System.Windows.Forms.MenuItem menuEmplReport;
        private System.Windows.Forms.MenuItem menuReadersSettings;
        private System.Windows.Forms.MenuItem menuEmployeeLocations;
        private System.Windows.Forms.MenuItem menuManual;
        private System.Windows.Forms.MenuItem menuPaymentRep;
        private System.Windows.Forms.MenuItem menuWTGroups;
        private System.Windows.Forms.MenuItem menuRetiredTags;
        private System.Windows.Forms.MenuItem menuProlongTS;
        private System.Windows.Forms.MenuItem menuMitallWU;
        private System.Windows.Forms.MenuItem menuMittalEmpl;
        private System.Windows.Forms.MenuItem menuMittalSch;
        private System.Windows.Forms.MenuItem menuMittalEmplAn;
        private System.Windows.Forms.MenuItem menuAccessControl;
        private System.Windows.Forms.MenuItem menuAccessControlGroups;
        private System.Windows.Forms.MenuItem menuEmployeeAccessControlGroups;
        private System.Windows.Forms.MenuItem menuAccessControlGroupsGates;
        private System.Windows.Forms.MenuItem menuAccessProfiles;
        private System.Windows.Forms.MenuItem menuTimeAccessProfiles;
        private System.Windows.Forms.MenuItem menuGateAccessProfiles;
        private System.Windows.Forms.MenuItem menuAssignGateProfile;
        private System.Windows.Forms.MenuItem menuApplyAccessControlParameters;
        private System.Windows.Forms.MenuItem menuMittalEmployeeLocations;
        private System.Windows.Forms.MenuItem menuWUReport;
        private System.Windows.Forms.MenuItem menuExtraHours;
        private System.Windows.Forms.MenuItem menuPauses;
        private System.Windows.Forms.MenuItem menuPIOPaymentRep;
        private System.Windows.Forms.MenuItem menuPIORep;
        private System.Windows.Forms.MenuItem menuInformations;
        private MenuItem menuPIOWorkListsRep;
        private MenuItem menuPIOWorkingSaturdays;
        private MenuItem menuVisits;
        private MenuItem menuAccessControlStatus;
        private MenuItem menuCameras;
        private MenuItem menuLiveView;

        ResourceManager rm;

        //indicate if message box will be showen to user or not
        private bool showMessageBox = true;

        public ACTASplashScreen splash = null;
        private MenuItem menuItemMonitor;
        private MenuItem menuVisitors;
        private MenuItem menuVisitorsEnter;
        private MenuItem menuVisitorsExit;
        private MenuItem menuVisitorsReport;
        private MenuItem menuCustomizedReports;
        private MenuItem menuRestaurant;
        private MenuItem menuItemRestaurant;
        private MenuItem menuGraficalRep;
        private MenuItem menuEmplPresenceRep;
        private MenuItem menuWUStatisticalRep;
        private MenuItem menuPicManagament;
        private MenuItem menuEmplPhotoManagament;
        private MenuItem menuCameraSnapshostManagament;
        private MenuItem menuItemTrespass;
        private MenuItem menuItemVisitors;
        private MenuItem menuItemMillennium;
        private MenuItem menuItemMillenniumEmpl;
        private MenuItem menuItemMillenniumWU;
        private MenuItem menuItemMillenniumTypes;

        private Thread liveViewThread;

        public string userID;
        private MenuItem menuRoutes;
        private MenuItem menuItemRoutes;
        private MenuItem menuRoutesTerminal;
        private MenuItem menuItemRoutesTerminal;
        private MenuItem menuItemATBFOD;
        private MenuItem menuItemATBFODPayment;
        private MenuItem menuExit;
        private MenuItem menuMapsReports;
        private MenuItem menuMapMaintenance;
        private MenuItem menuMapObjectMaintenance;
        private MenuItem menuPeopleCounter;
        private MenuItem menuStandardReport;
        private MenuItem menuJUBMES;
        private MenuItem menuJUBMESPayrolls;
        private MenuItem menuAdvancedReport;
        private MenuItem menuBasicReport;
        private MenuItem menuJEEP;
        private MenuItem menuJEEPWUReports;
        private MenuItem menuItemReportsByType;
        private MenuItem menuItemVacationEvidence;
        private MenuItem menuMappingSiPass;
        private MenuItem menuConnectionSetup;
        private MenuItem menuLog2XML;
        private MenuItem menuItem5;
        //private MenuItem menuItemLocking;
        private MenuItem menuItemUNIPROM;
        private MenuItem menuItemUNIPROMIOPairsReport;
        public string password;
        private MenuItem menuItemUNIPROMDailyPreviewReport;
        private MenuItem menuItemTagsPrview;
        private MenuItem menuItemUNIPROMRAMP;
        private MenuItem menuItemZIN;
        private MenuItem menuItemZINPassesPreview;
        private MenuItem menuItemExtraHours;
        private MenuItem menuItemEmplCategories;
        private MenuItem menuItemEUNET;
        private MenuItem menuItemEUNETPresenceReport;
        private MenuItem menuItemMinistry;
        private MenuItem menuMinistryEmplPresence;
        private MenuItem menuItemEnterDataByEmployee;
        private MenuItem menuItemGSK;
        private MenuItem menuItemGSKPresenceTracking;
        private MenuItem menuItemStatisticReport;
        List<int> modulesList;
        private MenuItem menuItemNiksic;
        private MenuItem menuItemMonthlyReport;
        private MenuItem menuItemSumPassesOnReader;
        private MenuItem menuItemSinvozCustomizedReports;
        private MenuItem menuItemSinvozReportsByPassTypes;
        private MenuItem menuItemVlatacom;
        private MenuItem menuItemVlatacomWholeDayAbsenceAnnualReport;
        private MenuItem menuItemOrganizationalUnits;
        private MenuItem menuItemFiat;
        private MenuItem menuItemPYIntegration;
        private MenuItem menuDSFReports;
        private MenuItem menuDSFPresenceReport;
        private MenuItem menuItemLames;
        private MenuItem menuItemLamesCumulativeReport;
        private MenuItem menuItemMassiveInput;
        private MenuItem menuEmplResponisibility;
        private MenuItem meniItemWN;
        private MenuItem meniItemWNDailyPresence;
        private MenuItem menuItemWNMonthly;
        private MenuItem menuConfezioniAndrea;
        private MenuItem menuConfezioniAndreaMonthlyReport;
        private MenuItem menuRules;
        private MenuItem menuCutOffDate;
        private MenuItem menuItemMC;
        private MenuItem menuItem7;
        private MenuItem menuItemRestartCounters;
        private MenuItem menuItem6;
        private MenuItem menuItemSynchronization;
        private MenuItem menuItemPMC;
        private MenuItem menuItemPMCMonthlyReport;
        private MenuItem menuItemPMCCumulativeReports;
        private MenuItem menuItemPMCStatisticalReports;
        private MenuItem menuItemSystem;
        private MenuItem menuItemSystemClosingEvents;
        private MenuItem menuItemSystemMessages;
        private MenuItem menuItemPMCPaymentReport;
        private MenuItem menuItemEmplCounterBalances;
        private MenuItem menuItemGeox;
        private MenuItem menuItemGeoxPYReport;
        private MenuItem menuItemFiatDecisions;
        private MenuItem menuItemCounterType;
        private MenuItem menuItemUsersCategory;
        private MenuItem menuItemULChangesTbl;
        private StatusBarPanel sbPanelDB;
        private MenuItem menuItemGrundfos;
        private MenuItem menuItemGrundfosTransportData;
        private MenuItem menuItemGrundfosPYReport;
        private MenuItem menuItemHyatt;
        private MenuItem menuItemHyattPYReport;
        private PictureBox pictureBox1;
        private MenuItem menuItemHyattTimeAndAttendance;
        private MenuItem menuItemHyattOperatersCategoriesReport;
        private MenuItem menuItemHyattOperatersRolesReport;
        private MenuItem menuItemHyattOperatersOUWUReport;
        private MenuItem menuItemHyattCategoriesPrivilegesReport;
        private MenuItem menuItemHyattRolePrivilegesReport;
        private MenuItem menuItemEmployeesPersonalHoliday;
        private MenuItem menuItemHyattRolesPrivilegiesReportHRSSC;
        private MenuItem menuItemHyattRolesPrivilegiesReportPY;
        private MenuItem menuItemHyattRolesPrivilegiesReportCANTEEN;
        private MenuItem menuItemHyattRolesPrivilegiesReportHRSSCEXTENDED;
        private MenuItem menuItemHyattRolesPrivilegiesReportMedicalCheck;
        private MenuItem menuItemHyattRolesPrivilegiesReportSystemClose;
        private MenuItem menuItemHyattPYGIDReport;
        private MenuItem menuItemAnualLeaveReport;
        private MenuItem menuItemStartWorkingDate;
        private MenuItem menuItemMagna;
        private MenuItem menuItemEmployeePT;
        private MenuItem menuDocManipulation;
        private MenuItem menuMonthlyTypeReport;
        private MenuItem menuReportsForSalaryForecast;
        private MenuItem menuReportsForOpenPairsByEmployee;
        private MenuItem menuVacationReport;
        private MenuItem menuRecordsOfBreaks;
        private MenuItem menuMachines;
        private MenuItem menuItem8;
        private MenuItem menuItemLastTerminalReading;
       

        

        public ApplUserTO CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        //UnipromUI.RampControlForm form;

        public ACTA()
        {
            try
            {
                InitializeComponent();
                // Debug
                NotificationController.SetApplicationName(this.Text.Replace(" ", ""));
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                InitializeObserverClient();
                this.CenterToScreen();
                rm = new ResourceManager("UI.Resource", typeof(Employees).Assembly);
                currentRoles = new List<ApplRoleTO>();
                itemPermissions = new int[40];
                menuItemsPermissions = new Hashtable();

                InitializeController();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        private void InitializeController()
        {
            Controller = NotificationController.GetInstance();
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ACTA));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuLibraries = new System.Windows.Forms.MenuItem();
            this.menuEmployees = new System.Windows.Forms.MenuItem();
            this.menuWT = new System.Windows.Forms.MenuItem();
            this.menuWTSchema = new System.Windows.Forms.MenuItem();
            this.menuHolidays = new System.Windows.Forms.MenuItem();
            this.menuWTGroups = new System.Windows.Forms.MenuItem();
            this.menuProlongTS = new System.Windows.Forms.MenuItem();
            this.menuPauses = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuWorkingUnits = new System.Windows.Forms.MenuItem();
            this.menuItemOrganizationalUnits = new System.Windows.Forms.MenuItem();
            this.menuPassTypes = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuLocations = new System.Windows.Forms.MenuItem();
            this.menuGates = new System.Windows.Forms.MenuItem();
            this.menuReaders = new System.Windows.Forms.MenuItem();
            this.menuCameras = new System.Windows.Forms.MenuItem();
            this.menuItemRestaurant = new System.Windows.Forms.MenuItem();
            this.menuRoutes = new System.Windows.Forms.MenuItem();
            this.menuRoutesTerminal = new System.Windows.Forms.MenuItem();
            this.menuItemMC = new System.Windows.Forms.MenuItem();
            this.menuMaps = new System.Windows.Forms.MenuItem();
            this.menuMapMaintenance = new System.Windows.Forms.MenuItem();
            this.menuMapObjectMaintenance = new System.Windows.Forms.MenuItem();
            this.menuMachines = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.menuReports = new System.Windows.Forms.MenuItem();
            this.menuStandardRep = new System.Windows.Forms.MenuItem();
            this.menuPresence = new System.Windows.Forms.MenuItem();
            this.menuRepLoc = new System.Windows.Forms.MenuItem();
            this.menuRepWU = new System.Windows.Forms.MenuItem();
            this.menuRepEmpl = new System.Windows.Forms.MenuItem();
            this.menuPresenceEmpl = new System.Windows.Forms.MenuItem();
            this.menuMonthlyTypeReport = new System.Windows.Forms.MenuItem();
            this.menuReportsForSalaryForecast = new System.Windows.Forms.MenuItem();
            this.menuReportsForOpenPairsByEmployee = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuTimeSchedule = new System.Windows.Forms.MenuItem();
            this.menuEmplReport = new System.Windows.Forms.MenuItem();
            this.menuWUReport = new System.Windows.Forms.MenuItem();
            this.menuItemReportsByType = new System.Windows.Forms.MenuItem();
            this.menuItemExtraHours = new System.Windows.Forms.MenuItem();
            this.menuVacationReport = new System.Windows.Forms.MenuItem();
            this.menuEmployeeLocations = new System.Windows.Forms.MenuItem();
            this.menuLiveView = new System.Windows.Forms.MenuItem();
            this.menuVisits = new System.Windows.Forms.MenuItem();
            this.menuItemTrespass = new System.Windows.Forms.MenuItem();
            this.menuItemRoutes = new System.Windows.Forms.MenuItem();
            this.menuItemRoutesTerminal = new System.Windows.Forms.MenuItem();
            this.menuGraficalRep = new System.Windows.Forms.MenuItem();
            this.menuEmplPresenceRep = new System.Windows.Forms.MenuItem();
            this.menuWUStatisticalRep = new System.Windows.Forms.MenuItem();
            this.menuCustomizedReports = new System.Windows.Forms.MenuItem();
            this.menuMittalRep = new System.Windows.Forms.MenuItem();
            this.menuPaymentRep = new System.Windows.Forms.MenuItem();
            this.menuRetiredTags = new System.Windows.Forms.MenuItem();
            this.menuMitallWU = new System.Windows.Forms.MenuItem();
            this.menuMittalEmpl = new System.Windows.Forms.MenuItem();
            this.menuMittalSch = new System.Windows.Forms.MenuItem();
            this.menuMittalEmplAn = new System.Windows.Forms.MenuItem();
            this.menuMittalEmployeeLocations = new System.Windows.Forms.MenuItem();
            this.menuItemSumPassesOnReader = new System.Windows.Forms.MenuItem();
            this.menuPIORep = new System.Windows.Forms.MenuItem();
            this.menuPIOPaymentRep = new System.Windows.Forms.MenuItem();
            this.menuPIOWorkListsRep = new System.Windows.Forms.MenuItem();
            this.menuPIOWorkingSaturdays = new System.Windows.Forms.MenuItem();
            this.menuItemMillennium = new System.Windows.Forms.MenuItem();
            this.menuItemMillenniumEmpl = new System.Windows.Forms.MenuItem();
            this.menuItemMillenniumWU = new System.Windows.Forms.MenuItem();
            this.menuItemMillenniumTypes = new System.Windows.Forms.MenuItem();
            this.menuItemSinvozCustomizedReports = new System.Windows.Forms.MenuItem();
            this.menuItemSinvozReportsByPassTypes = new System.Windows.Forms.MenuItem();
            this.menuItemATBFOD = new System.Windows.Forms.MenuItem();
            this.menuItemATBFODPayment = new System.Windows.Forms.MenuItem();
            this.menuItemVlatacom = new System.Windows.Forms.MenuItem();
            this.menuItemVlatacomWholeDayAbsenceAnnualReport = new System.Windows.Forms.MenuItem();
            this.menuJUBMES = new System.Windows.Forms.MenuItem();
            this.menuJUBMESPayrolls = new System.Windows.Forms.MenuItem();
            this.menuJEEP = new System.Windows.Forms.MenuItem();
            this.menuJEEPWUReports = new System.Windows.Forms.MenuItem();
            this.menuItemUNIPROM = new System.Windows.Forms.MenuItem();
            this.menuItemUNIPROMIOPairsReport = new System.Windows.Forms.MenuItem();
            this.menuItemUNIPROMDailyPreviewReport = new System.Windows.Forms.MenuItem();
            this.menuItemZIN = new System.Windows.Forms.MenuItem();
            this.menuItemZINPassesPreview = new System.Windows.Forms.MenuItem();
            this.menuItemEmplCategories = new System.Windows.Forms.MenuItem();
            this.menuItemStatisticReport = new System.Windows.Forms.MenuItem();
            this.menuItemEUNET = new System.Windows.Forms.MenuItem();
            this.menuItemEUNETPresenceReport = new System.Windows.Forms.MenuItem();
            this.menuItemMinistry = new System.Windows.Forms.MenuItem();
            this.menuMinistryEmplPresence = new System.Windows.Forms.MenuItem();
            this.menuItemGSK = new System.Windows.Forms.MenuItem();
            this.menuItemGSKPresenceTracking = new System.Windows.Forms.MenuItem();
            this.menuItemNiksic = new System.Windows.Forms.MenuItem();
            this.menuItemMonthlyReport = new System.Windows.Forms.MenuItem();
            this.menuItemFiat = new System.Windows.Forms.MenuItem();
            this.menuItemPYIntegration = new System.Windows.Forms.MenuItem();
            this.menuItemEmplCounterBalances = new System.Windows.Forms.MenuItem();
            this.menuItemFiatDecisions = new System.Windows.Forms.MenuItem();
            this.menuDSFReports = new System.Windows.Forms.MenuItem();
            this.menuDSFPresenceReport = new System.Windows.Forms.MenuItem();
            this.menuItemLames = new System.Windows.Forms.MenuItem();
            this.menuItemLamesCumulativeReport = new System.Windows.Forms.MenuItem();
            this.meniItemWN = new System.Windows.Forms.MenuItem();
            this.meniItemWNDailyPresence = new System.Windows.Forms.MenuItem();
            this.menuItemWNMonthly = new System.Windows.Forms.MenuItem();
            this.menuConfezioniAndrea = new System.Windows.Forms.MenuItem();
            this.menuConfezioniAndreaMonthlyReport = new System.Windows.Forms.MenuItem();
            this.menuItemPMC = new System.Windows.Forms.MenuItem();
            this.menuItemPMCMonthlyReport = new System.Windows.Forms.MenuItem();
            this.menuItemPMCCumulativeReports = new System.Windows.Forms.MenuItem();
            this.menuItemPMCStatisticalReports = new System.Windows.Forms.MenuItem();
            this.menuItemPMCPaymentReport = new System.Windows.Forms.MenuItem();
            this.menuItemGeox = new System.Windows.Forms.MenuItem();
            this.menuItemGeoxPYReport = new System.Windows.Forms.MenuItem();
            this.menuItemGrundfos = new System.Windows.Forms.MenuItem();
            this.menuItemGrundfosTransportData = new System.Windows.Forms.MenuItem();
            this.menuItemGrundfosPYReport = new System.Windows.Forms.MenuItem();
            this.menuItemHyatt = new System.Windows.Forms.MenuItem();
            this.menuItemHyattPYReport = new System.Windows.Forms.MenuItem();
            this.menuItemHyattTimeAndAttendance = new System.Windows.Forms.MenuItem();
            this.menuItemHyattOperatersCategoriesReport = new System.Windows.Forms.MenuItem();
            this.menuItemHyattOperatersRolesReport = new System.Windows.Forms.MenuItem();
            this.menuItemHyattOperatersOUWUReport = new System.Windows.Forms.MenuItem();
            this.menuItemHyattCategoriesPrivilegesReport = new System.Windows.Forms.MenuItem();
            this.menuItemHyattRolePrivilegesReport = new System.Windows.Forms.MenuItem();
            this.menuItemHyattRolesPrivilegiesReportHRSSC = new System.Windows.Forms.MenuItem();
            this.menuItemHyattRolesPrivilegiesReportPY = new System.Windows.Forms.MenuItem();
            this.menuItemHyattRolesPrivilegiesReportCANTEEN = new System.Windows.Forms.MenuItem();
            this.menuItemHyattRolesPrivilegiesReportHRSSCEXTENDED = new System.Windows.Forms.MenuItem();
            this.menuItemHyattRolesPrivilegiesReportMedicalCheck = new System.Windows.Forms.MenuItem();
            this.menuItemHyattRolesPrivilegiesReportSystemClose = new System.Windows.Forms.MenuItem();
            this.menuItemEmployeesPersonalHoliday = new System.Windows.Forms.MenuItem();
            this.menuItemHyattPYGIDReport = new System.Windows.Forms.MenuItem();
            this.menuItemAnualLeaveReport = new System.Windows.Forms.MenuItem();
            this.menuItemStartWorkingDate = new System.Windows.Forms.MenuItem();
            this.menuItemMagna = new System.Windows.Forms.MenuItem();
            this.menuItemEmployeePT = new System.Windows.Forms.MenuItem();
            this.menuMapsReports = new System.Windows.Forms.MenuItem();
            this.menuPeopleCounter = new System.Windows.Forms.MenuItem();
            this.menuStandardReport = new System.Windows.Forms.MenuItem();
            this.menuAdvancedReport = new System.Windows.Forms.MenuItem();
            this.menuBasicReport = new System.Windows.Forms.MenuItem();
            this.menuDocManipulation = new System.Windows.Forms.MenuItem();
            this.menuInterventions = new System.Windows.Forms.MenuItem();
            this.menuPasses = new System.Windows.Forms.MenuItem();
            this.menuIOPairs = new System.Windows.Forms.MenuItem();
            this.menuEmployeeAbsences = new System.Windows.Forms.MenuItem();
            this.menuExitPermissions = new System.Windows.Forms.MenuItem();
            this.menuExtraHours = new System.Windows.Forms.MenuItem();
            this.menuItemVacationEvidence = new System.Windows.Forms.MenuItem();
            this.menuItemEnterDataByEmployee = new System.Windows.Forms.MenuItem();
            this.menuItemMassiveInput = new System.Windows.Forms.MenuItem();
            this.menuRecordsOfBreaks = new System.Windows.Forms.MenuItem();
            this.menuMaintaining = new System.Windows.Forms.MenuItem();
            this.menuLibExport = new System.Windows.Forms.MenuItem();
            this.menuImportData = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuProcessingTables = new System.Windows.Forms.MenuItem();
            this.menuImportLog = new System.Windows.Forms.MenuItem();
            this.menuLogToPasses = new System.Windows.Forms.MenuItem();
            this.menuPassesToIOPairs = new System.Windows.Forms.MenuItem();
            this.menuReadersSettings = new System.Windows.Forms.MenuItem();
            this.menuPicManagament = new System.Windows.Forms.MenuItem();
            this.menuEmplPhotoManagament = new System.Windows.Forms.MenuItem();
            this.menuCameraSnapshostManagament = new System.Windows.Forms.MenuItem();
            this.menuItemVisitors = new System.Windows.Forms.MenuItem();
            this.menuLog2XML = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuMappingSiPass = new System.Windows.Forms.MenuItem();
            this.menuConnectionSetup = new System.Windows.Forms.MenuItem();
            this.menuItemUNIPROMRAMP = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItemTagsPrview = new System.Windows.Forms.MenuItem();
            this.menuItemLastTerminalReading = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItemRestartCounters = new System.Windows.Forms.MenuItem();
            this.menuItemSynchronization = new System.Windows.Forms.MenuItem();
            this.menuConfiguration = new System.Windows.Forms.MenuItem();
            this.menuDataAccess = new System.Windows.Forms.MenuItem();
            this.menuEmplAccess = new System.Windows.Forms.MenuItem();
            this.menuEmplResponisibility = new System.Windows.Forms.MenuItem();
            this.menuUserRoles = new System.Windows.Forms.MenuItem();
            this.menuUsers = new System.Windows.Forms.MenuItem();
            this.menuRoles = new System.Windows.Forms.MenuItem();
            this.menuRoleMaintainence = new System.Windows.Forms.MenuItem();
            this.menuRolePrivileges = new System.Windows.Forms.MenuItem();
            this.menuUsersRoles = new System.Windows.Forms.MenuItem();
            this.menuAccessControl = new System.Windows.Forms.MenuItem();
            this.menuAccessControlGroups = new System.Windows.Forms.MenuItem();
            this.menuEmployeeAccessControlGroups = new System.Windows.Forms.MenuItem();
            this.menuAccessControlGroupsGates = new System.Windows.Forms.MenuItem();
            this.menuAccessProfiles = new System.Windows.Forms.MenuItem();
            this.menuTimeAccessProfiles = new System.Windows.Forms.MenuItem();
            this.menuGateAccessProfiles = new System.Windows.Forms.MenuItem();
            this.menuAssignGateProfile = new System.Windows.Forms.MenuItem();
            this.menuApplyAccessControlParameters = new System.Windows.Forms.MenuItem();
            this.menuAccessControlStatus = new System.Windows.Forms.MenuItem();
            this.menuRules = new System.Windows.Forms.MenuItem();
            this.menuCutOffDate = new System.Windows.Forms.MenuItem();
            this.menuItemSystem = new System.Windows.Forms.MenuItem();
            this.menuItemSystemClosingEvents = new System.Windows.Forms.MenuItem();
            this.menuItemSystemMessages = new System.Windows.Forms.MenuItem();
            this.menuItemCounterType = new System.Windows.Forms.MenuItem();
            this.menuItemUsersCategory = new System.Windows.Forms.MenuItem();
            this.menuItemULChangesTbl = new System.Windows.Forms.MenuItem();
            this.menuItemMonitor = new System.Windows.Forms.MenuItem();
            this.menuVisitors = new System.Windows.Forms.MenuItem();
            this.menuVisitorsEnter = new System.Windows.Forms.MenuItem();
            this.menuVisitorsExit = new System.Windows.Forms.MenuItem();
            this.menuVisitorsReport = new System.Windows.Forms.MenuItem();
            this.menuRestaurant = new System.Windows.Forms.MenuItem();
            this.menuHelp = new System.Windows.Forms.MenuItem();
            this.menuManual = new System.Windows.Forms.MenuItem();
            this.menuAbout = new System.Windows.Forms.MenuItem();
            this.menuInformations = new System.Windows.Forms.MenuItem();
            this.ACTAStatusBar = new System.Windows.Forms.StatusBar();
            this.sbPanelDate = new System.Windows.Forms.StatusBarPanel();
            this.sbPanelUser = new System.Windows.Forms.StatusBarPanel();
            this.sbPanelLogInTime = new System.Windows.Forms.StatusBarPanel();
            this.sbPanelDB = new System.Windows.Forms.StatusBarPanel();
            this.pbClientLogo = new System.Windows.Forms.PictureBox();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelLogInTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClientLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuLibraries,
            this.menuReports,
            this.menuInterventions,
            this.menuMaintaining,
            this.menuConfiguration,
            this.menuItemMonitor,
            this.menuVisitors,
            this.menuRestaurant,
            this.menuHelp});
            this.mainMenu1.Collapse += new System.EventHandler(this.mainMenu1_Collapse);
            // 
            // menuLibraries
            // 
            this.menuLibraries.Index = 0;
            this.menuLibraries.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuEmployees,
            this.menuWT,
            this.menuItem1,
            this.menuWorkingUnits,
            this.menuItemOrganizationalUnits,
            this.menuPassTypes,
            this.menuItem4,
            this.menuLocations,
            this.menuGates,
            this.menuReaders,
            this.menuCameras,
            this.menuItemRestaurant,
            this.menuRoutes,
            this.menuRoutesTerminal,
            this.menuItemMC,
            this.menuMaps,
            this.menuMachines,
            this.menuItem2,
            this.menuExit});
            this.menuLibraries.ShowShortcut = false;
            this.menuLibraries.Text = "Libraries";
            // 
            // menuEmployees
            // 
            this.menuEmployees.Index = 0;
            this.menuEmployees.Shortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.menuEmployees.Text = "&Employees";
            this.menuEmployees.Click += new System.EventHandler(this.menuEmployees_Click);
            // 
            // menuWT
            // 
            this.menuWT.Index = 1;
            this.menuWT.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuWTSchema,
            this.menuHolidays,
            this.menuWTGroups,
            this.menuProlongTS,
            this.menuPauses});
            this.menuWT.Text = "Work Time";
            // 
            // menuWTSchema
            // 
            this.menuWTSchema.Index = 0;
            this.menuWTSchema.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.menuWTSchema.Text = "Work Time Schema";
            this.menuWTSchema.Click += new System.EventHandler(this.menuWTSchema_Click);
            // 
            // menuHolidays
            // 
            this.menuHolidays.Index = 1;
            this.menuHolidays.Shortcut = System.Windows.Forms.Shortcut.CtrlH;
            this.menuHolidays.Text = "Holidays";
            this.menuHolidays.Click += new System.EventHandler(this.menuHolidays_Click);
            // 
            // menuWTGroups
            // 
            this.menuWTGroups.Index = 2;
            this.menuWTGroups.Shortcut = System.Windows.Forms.Shortcut.CtrlG;
            this.menuWTGroups.Text = "Work Time Groups";
            this.menuWTGroups.Click += new System.EventHandler(this.menuWTGroups_Click);
            // 
            // menuProlongTS
            // 
            this.menuProlongTS.Index = 3;
            this.menuProlongTS.Text = "Prolonging time schedules";
            this.menuProlongTS.Click += new System.EventHandler(this.menuProlongTS_Click);
            // 
            // menuPauses
            // 
            this.menuPauses.Index = 4;
            this.menuPauses.Text = "Pauses";
            this.menuPauses.Click += new System.EventHandler(this.menuPauses_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 2;
            this.menuItem1.Text = "-";
            // 
            // menuWorkingUnits
            // 
            this.menuWorkingUnits.Index = 3;
            this.menuWorkingUnits.Shortcut = System.Windows.Forms.Shortcut.CtrlW;
            this.menuWorkingUnits.Text = "&Working Units";
            this.menuWorkingUnits.Click += new System.EventHandler(this.menuWorkingUnits_Click);
            // 
            // menuItemOrganizationalUnits
            // 
            this.menuItemOrganizationalUnits.Index = 4;
            this.menuItemOrganizationalUnits.Text = "Organizational Units";
            this.menuItemOrganizationalUnits.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuPassTypes
            // 
            this.menuPassTypes.Index = 5;
            this.menuPassTypes.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
            this.menuPassTypes.Text = "Pass Types";
            this.menuPassTypes.Click += new System.EventHandler(this.menuPassType_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 6;
            this.menuItem4.Text = "-";
            // 
            // menuLocations
            // 
            this.menuLocations.Index = 7;
            this.menuLocations.Shortcut = System.Windows.Forms.Shortcut.CtrlL;
            this.menuLocations.Text = "&Locations";
            this.menuLocations.Click += new System.EventHandler(this.menuLocations_Click);
            // 
            // menuGates
            // 
            this.menuGates.Index = 8;
            this.menuGates.Shortcut = System.Windows.Forms.Shortcut.CtrlK;
            this.menuGates.Text = "Gates";
            this.menuGates.Click += new System.EventHandler(this.menuGates_Click);
            // 
            // menuReaders
            // 
            this.menuReaders.Index = 9;
            this.menuReaders.Shortcut = System.Windows.Forms.Shortcut.CtrlT;
            this.menuReaders.Text = "Readers";
            this.menuReaders.Click += new System.EventHandler(this.menuReaders_Click);
            // 
            // menuCameras
            // 
            this.menuCameras.Index = 10;
            this.menuCameras.Text = "Cameras";
            this.menuCameras.Click += new System.EventHandler(this.menuCameras_Click);
            // 
            // menuItemRestaurant
            // 
            this.menuItemRestaurant.Index = 11;
            this.menuItemRestaurant.Text = "Restaurant";
            this.menuItemRestaurant.Click += new System.EventHandler(this.menuItemRestaurant_Click);
            // 
            // menuRoutes
            // 
            this.menuRoutes.Index = 12;
            this.menuRoutes.Text = "Routes by tag";
            this.menuRoutes.Click += new System.EventHandler(this.menuRoutes_Click);
            // 
            // menuRoutesTerminal
            // 
            this.menuRoutesTerminal.Index = 13;
            this.menuRoutesTerminal.Text = "Routes by terminal";
            this.menuRoutesTerminal.Click += new System.EventHandler(this.menuRoutesTerminal_Click);
            // 
            // menuItemMC
            // 
            this.menuItemMC.Index = 14;
            this.menuItemMC.Text = "Medical check";
            this.menuItemMC.Click += new System.EventHandler(this.menuMedicalCheck);
            // 
            // menuMaps
            // 
            this.menuMaps.Index = 15;
            this.menuMaps.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuMapMaintenance,
            this.menuMapObjectMaintenance});
            this.menuMaps.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.menuMaps.Text = "Maps";
            this.menuMaps.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuMapMaintenance
            // 
            this.menuMapMaintenance.Index = 0;
            this.menuMapMaintenance.Text = "Map maintenance";
            this.menuMapMaintenance.Click += new System.EventHandler(this.menuMapMaintenance_Click);
            // 
            // menuMapObjectMaintenance
            // 
            this.menuMapObjectMaintenance.Index = 1;
            this.menuMapObjectMaintenance.Text = "Map object maintenance";
            this.menuMapObjectMaintenance.Click += new System.EventHandler(this.menuMapObjectMaintenance_Click);
            // 
            // menuMachines
            // 
            this.menuMachines.Index = 16;
            this.menuMachines.Text = "Machines";
            this.menuMachines.Click += new System.EventHandler(this.menuMachines_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 17;
            this.menuItem2.Text = "-";
            // 
            // menuExit
            // 
            this.menuExit.Index = 18;
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click_1);
            // 
            // menuReports
            // 
            this.menuReports.Index = 1;
            this.menuReports.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuStandardRep,
            this.menuGraficalRep,
            this.menuCustomizedReports,
            this.menuMapsReports,
            this.menuPeopleCounter,
            this.menuDocManipulation});
            this.menuReports.Text = "Reports";
            this.menuReports.Click += new System.EventHandler(this.menuReports_Click);
            // 
            // menuStandardRep
            // 
            this.menuStandardRep.Index = 0;
            this.menuStandardRep.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPresence,
            this.menuTimeSchedule,
            this.menuEmployeeLocations,
            this.menuLiveView,
            this.menuVisits,
            this.menuItemTrespass,
            this.menuItemRoutes,
            this.menuItemRoutesTerminal});
            this.menuStandardRep.Text = "Standard Reports";
            // 
            // menuPresence
            // 
            this.menuPresence.Index = 0;
            this.menuPresence.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuRepLoc,
            this.menuRepWU,
            this.menuRepEmpl,
            this.menuPresenceEmpl,
            this.menuMonthlyTypeReport,
            this.menuReportsForSalaryForecast,
            this.menuReportsForOpenPairsByEmployee,
            this.menuItem8});
            this.menuPresence.Text = "Presence Reports";
            // 
            // menuRepLoc
            // 
            this.menuRepLoc.Index = 0;
            this.menuRepLoc.Text = "Reports for Location";
            this.menuRepLoc.Click += new System.EventHandler(this.menuRepLoc_Click);
            // 
            // menuRepWU
            // 
            this.menuRepWU.Index = 1;
            this.menuRepWU.Text = "Reports for Working Unit";
            this.menuRepWU.Click += new System.EventHandler(this.menuRepWU_Click);
            // 
            // menuRepEmpl
            // 
            this.menuRepEmpl.Index = 2;
            this.menuRepEmpl.Text = "Reports for Employees";
            this.menuRepEmpl.Click += new System.EventHandler(this.menuRepEmpl_Click);
            // 
            // menuPresenceEmpl
            // 
            this.menuPresenceEmpl.Index = 3;
            this.menuPresenceEmpl.Text = "Presence Tracking Reports";
            this.menuPresenceEmpl.Click += new System.EventHandler(this.menuPresence_Click);
            // 
            // menuMonthlyTypeReport
            // 
            this.menuMonthlyTypeReport.Index = 4;
            this.menuMonthlyTypeReport.Text = "Monthly Presence Tracking Report";
            this.menuMonthlyTypeReport.Click += new System.EventHandler(this.menuMonthlyTypeReport_Click);
            // 
            // menuReportsForSalaryForecast
            // 
            this.menuReportsForSalaryForecast.Index = 5;
            this.menuReportsForSalaryForecast.Text = "Reports for Salary (Forecast)";
            this.menuReportsForSalaryForecast.Click += new System.EventHandler(this.menuReportsForSalaryForecast_Click);
            // 
            // menuReportsForOpenPairsByEmployee
            // 
            this.menuReportsForOpenPairsByEmployee.Index = 6;
            this.menuReportsForOpenPairsByEmployee.Text = "Report for Open Pairs By Employee";
            this.menuReportsForOpenPairsByEmployee.Click += new System.EventHandler(this.menuReportsForOpenPairsByEmployee_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 7;
            this.menuItem8.Text = "Report for SMP --- Broj prisutnih i odsutnih";
            this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click_1);
            // 
            // menuTimeSchedule
            // 
            this.menuTimeSchedule.Index = 1;
            this.menuTimeSchedule.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuEmplReport,
            this.menuWUReport,
            this.menuItemReportsByType,
            this.menuItemExtraHours,
            this.menuVacationReport});
            this.menuTimeSchedule.Text = "Time Schedule Reports";
            // 
            // menuEmplReport
            // 
            this.menuEmplReport.Index = 0;
            this.menuEmplReport.Text = "Reports for Employee";
            this.menuEmplReport.Click += new System.EventHandler(this.menuEmplReport_Click);
            // 
            // menuWUReport
            // 
            this.menuWUReport.Index = 1;
            this.menuWUReport.Text = "Reports for Working Unit";
            this.menuWUReport.Click += new System.EventHandler(this.menuWUReport_Click);
            // 
            // menuItemReportsByType
            // 
            this.menuItemReportsByType.Index = 2;
            this.menuItemReportsByType.Text = "Reports by types";
            this.menuItemReportsByType.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // menuItemExtraHours
            // 
            this.menuItemExtraHours.Index = 3;
            this.menuItemExtraHours.Text = "Extra hours";
            this.menuItemExtraHours.Click += new System.EventHandler(this.menuItemExtraHours_Click);
            // 
            // menuVacationReport
            // 
            this.menuVacationReport.Index = 4;
            this.menuVacationReport.Text = "Reports for Vacation";
            this.menuVacationReport.Click += new System.EventHandler(this.menuVacationReport_Click);
            // 
            // menuEmployeeLocations
            // 
            this.menuEmployeeLocations.Index = 2;
            this.menuEmployeeLocations.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.menuEmployeeLocations.Text = "Employee Locations";
            this.menuEmployeeLocations.Click += new System.EventHandler(this.menuEmployeeLocations_Click);
            // 
            // menuLiveView
            // 
            this.menuLiveView.Index = 3;
            this.menuLiveView.Text = "Live view";
            this.menuLiveView.Click += new System.EventHandler(this.menuLiveView_Click);
            // 
            // menuVisits
            // 
            this.menuVisits.Index = 4;
            this.menuVisits.Text = "Visits";
            this.menuVisits.Click += new System.EventHandler(this.menuVisits_Click);
            // 
            // menuItemTrespass
            // 
            this.menuItemTrespass.Index = 5;
            this.menuItemTrespass.Text = "Trespass";
            this.menuItemTrespass.Click += new System.EventHandler(this.menuItemTrespass_Click);
            // 
            // menuItemRoutes
            // 
            this.menuItemRoutes.Index = 6;
            this.menuItemRoutes.Text = "Routes";
            this.menuItemRoutes.Click += new System.EventHandler(this.menuItemRoutes_Click);
            // 
            // menuItemRoutesTerminal
            // 
            this.menuItemRoutesTerminal.Index = 7;
            this.menuItemRoutesTerminal.Text = "Routes by terminal";
            this.menuItemRoutesTerminal.Click += new System.EventHandler(this.menuItemRoutesTerminal_Click);
            // 
            // menuGraficalRep
            // 
            this.menuGraficalRep.Index = 1;
            this.menuGraficalRep.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuEmplPresenceRep,
            this.menuWUStatisticalRep});
            this.menuGraficalRep.Text = "Grafical reports";
            // 
            // menuEmplPresenceRep
            // 
            this.menuEmplPresenceRep.Index = 0;
            this.menuEmplPresenceRep.Text = "Employees presence report";
            this.menuEmplPresenceRep.Click += new System.EventHandler(this.menuEmplPresenceRep_Click);
            // 
            // menuWUStatisticalRep
            // 
            this.menuWUStatisticalRep.Index = 1;
            this.menuWUStatisticalRep.Text = "Working unit statistical report";
            this.menuWUStatisticalRep.Click += new System.EventHandler(this.menuWUStatisticalRep_Click);
            // 
            // menuCustomizedReports
            // 
            this.menuCustomizedReports.Index = 2;
            this.menuCustomizedReports.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuMittalRep,
            this.menuPIORep,
            this.menuItemMillennium,
            this.menuItemSinvozCustomizedReports,
            this.menuItemATBFOD,
            this.menuItemVlatacom,
            this.menuJUBMES,
            this.menuJEEP,
            this.menuItemUNIPROM,
            this.menuItemZIN,
            this.menuItemEUNET,
            this.menuItemMinistry,
            this.menuItemGSK,
            this.menuItemNiksic,
            this.menuItemFiat,
            this.menuDSFReports,
            this.menuItemLames,
            this.meniItemWN,
            this.menuConfezioniAndrea,
            this.menuItemPMC,
            this.menuItemGeox,
            this.menuItemGrundfos,
            this.menuItemHyatt,
            this.menuItemMagna});
            this.menuCustomizedReports.Text = "Customized Reports";
            // 
            // menuMittalRep
            // 
            this.menuMittalRep.Index = 0;
            this.menuMittalRep.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPaymentRep,
            this.menuRetiredTags,
            this.menuMitallWU,
            this.menuMittalEmpl,
            this.menuMittalSch,
            this.menuMittalEmployeeLocations,
            this.menuItemSumPassesOnReader});
            this.menuMittalRep.Text = "Mittal Reports";
            // 
            // menuPaymentRep
            // 
            this.menuPaymentRep.Index = 0;
            this.menuPaymentRep.Text = "Payment";
            this.menuPaymentRep.Click += new System.EventHandler(this.menuPaymentRep_Click);
            // 
            // menuRetiredTags
            // 
            this.menuRetiredTags.Index = 1;
            this.menuRetiredTags.Text = "Retired tags";
            this.menuRetiredTags.Click += new System.EventHandler(this.menuRetiredTags_Click);
            // 
            // menuMitallWU
            // 
            this.menuMitallWU.Index = 2;
            this.menuMitallWU.Text = "Reports for working unit";
            this.menuMitallWU.Click += new System.EventHandler(this.menuMitallWU_Click);
            // 
            // menuMittalEmpl
            // 
            this.menuMittalEmpl.Index = 3;
            this.menuMittalEmpl.Text = "Report for employees";
            this.menuMittalEmpl.Click += new System.EventHandler(this.menuMittalEmpl_Click);
            // 
            // menuMittalSch
            // 
            this.menuMittalSch.Index = 4;
            this.menuMittalSch.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuMittalEmplAn});
            this.menuMittalSch.Text = "Time schedule reports";
            // 
            // menuMittalEmplAn
            // 
            this.menuMittalEmplAn.Index = 0;
            this.menuMittalEmplAn.Text = "Reports for employee";
            this.menuMittalEmplAn.Click += new System.EventHandler(this.menuMittalEmplAn_Click);
            // 
            // menuMittalEmployeeLocations
            // 
            this.menuMittalEmployeeLocations.Index = 5;
            this.menuMittalEmployeeLocations.Text = "Employee locations";
            this.menuMittalEmployeeLocations.Click += new System.EventHandler(this.menuMittalEmployeeLocations_Click);
            // 
            // menuItemSumPassesOnReader
            // 
            this.menuItemSumPassesOnReader.Index = 6;
            this.menuItemSumPassesOnReader.Text = "Sum passes on reader";
            this.menuItemSumPassesOnReader.Click += new System.EventHandler(this.menuItemSumPassesOnReader_Click);
            // 
            // menuPIORep
            // 
            this.menuPIORep.Index = 1;
            this.menuPIORep.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPIOPaymentRep,
            this.menuPIOWorkListsRep,
            this.menuPIOWorkingSaturdays});
            this.menuPIORep.Text = "PIO Reports";
            // 
            // menuPIOPaymentRep
            // 
            this.menuPIOPaymentRep.Index = 0;
            this.menuPIOPaymentRep.Text = "PIO Payment";
            this.menuPIOPaymentRep.Click += new System.EventHandler(this.menuPIOPaymentRep_Click);
            // 
            // menuPIOWorkListsRep
            // 
            this.menuPIOWorkListsRep.Index = 1;
            this.menuPIOWorkListsRep.Text = "PIO Work lists";
            this.menuPIOWorkListsRep.Click += new System.EventHandler(this.menuPIOWorkListsRep_Click);
            // 
            // menuPIOWorkingSaturdays
            // 
            this.menuPIOWorkingSaturdays.Index = 2;
            this.menuPIOWorkingSaturdays.Text = "PIO Working saturdays";
            this.menuPIOWorkingSaturdays.Click += new System.EventHandler(this.menuPIOWorkingSaturdays_Click);
            // 
            // menuItemMillennium
            // 
            this.menuItemMillennium.Index = 2;
            this.menuItemMillennium.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemMillenniumEmpl,
            this.menuItemMillenniumWU,
            this.menuItemMillenniumTypes});
            this.menuItemMillennium.Text = "Millennium Reports";
            // 
            // menuItemMillenniumEmpl
            // 
            this.menuItemMillenniumEmpl.Index = 0;
            this.menuItemMillenniumEmpl.Text = "Reports for employee";
            this.menuItemMillenniumEmpl.Click += new System.EventHandler(this.menuItemMillenniumEmpl_Click);
            // 
            // menuItemMillenniumWU
            // 
            this.menuItemMillenniumWU.Index = 1;
            this.menuItemMillenniumWU.Text = "Reports for working unit";
            this.menuItemMillenniumWU.Click += new System.EventHandler(this.menuItemMillenniumWU_Click);
            // 
            // menuItemMillenniumTypes
            // 
            this.menuItemMillenniumTypes.Index = 2;
            this.menuItemMillenniumTypes.Text = "Presence reports by types";
            this.menuItemMillenniumTypes.Click += new System.EventHandler(this.menuItemMillenniumTypes_Click);
            // 
            // menuItemSinvozCustomizedReports
            // 
            this.menuItemSinvozCustomizedReports.Index = 3;
            this.menuItemSinvozCustomizedReports.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSinvozReportsByPassTypes});
            this.menuItemSinvozCustomizedReports.Text = "Sinvoz Reports";
            // 
            // menuItemSinvozReportsByPassTypes
            // 
            this.menuItemSinvozReportsByPassTypes.Index = 0;
            this.menuItemSinvozReportsByPassTypes.Text = "Reports by Types";
            this.menuItemSinvozReportsByPassTypes.Click += new System.EventHandler(this.menuItemSinvozReportsByPassTypes_Click);
            // 
            // menuItemATBFOD
            // 
            this.menuItemATBFOD.Index = 4;
            this.menuItemATBFOD.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemATBFODPayment});
            this.menuItemATBFOD.Text = "ATBFODReports";
            // 
            // menuItemATBFODPayment
            // 
            this.menuItemATBFODPayment.Index = 0;
            this.menuItemATBFODPayment.Text = "Payment Reports";
            this.menuItemATBFODPayment.Click += new System.EventHandler(this.menuItemATBFODPayment_Click);
            // 
            // menuItemVlatacom
            // 
            this.menuItemVlatacom.Index = 5;
            this.menuItemVlatacom.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemVlatacomWholeDayAbsenceAnnualReport});
            this.menuItemVlatacom.Text = "Vlatacom Reports";
            // 
            // menuItemVlatacomWholeDayAbsenceAnnualReport
            // 
            this.menuItemVlatacomWholeDayAbsenceAnnualReport.Index = 0;
            this.menuItemVlatacomWholeDayAbsenceAnnualReport.Text = "WholeDayAbsenceAnnualReport";
            this.menuItemVlatacomWholeDayAbsenceAnnualReport.Click += new System.EventHandler(this.menuItemVlatacomWholeDayAbsenceAnnualReport_Click);
            // 
            // menuJUBMES
            // 
            this.menuJUBMES.Index = 6;
            this.menuJUBMES.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuJUBMESPayrolls});
            this.menuJUBMES.Text = "JUBMES";
            // 
            // menuJUBMESPayrolls
            // 
            this.menuJUBMESPayrolls.Index = 0;
            this.menuJUBMESPayrolls.Text = "Payment report";
            this.menuJUBMESPayrolls.Click += new System.EventHandler(this.menuJUBMESPayrolls_Click);
            // 
            // menuJEEP
            // 
            this.menuJEEP.Index = 7;
            this.menuJEEP.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuJEEPWUReports});
            this.menuJEEP.Text = "JEEP";
            // 
            // menuJEEPWUReports
            // 
            this.menuJEEPWUReports.Index = 0;
            this.menuJEEPWUReports.Text = "Working unit reports";
            this.menuJEEPWUReports.Click += new System.EventHandler(this.menuJEEPWUReports_Click);
            // 
            // menuItemUNIPROM
            // 
            this.menuItemUNIPROM.Index = 8;
            this.menuItemUNIPROM.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemUNIPROMIOPairsReport,
            this.menuItemUNIPROMDailyPreviewReport});
            this.menuItemUNIPROM.Text = "UNIPROM";
            // 
            // menuItemUNIPROMIOPairsReport
            // 
            this.menuItemUNIPROMIOPairsReport.Index = 0;
            this.menuItemUNIPROMIOPairsReport.Text = "IOPairsReport";
            this.menuItemUNIPROMIOPairsReport.Click += new System.EventHandler(this.menuItemUNIPROMIOPairsReport_Click);
            // 
            // menuItemUNIPROMDailyPreviewReport
            // 
            this.menuItemUNIPROMDailyPreviewReport.Index = 1;
            this.menuItemUNIPROMDailyPreviewReport.Text = "DailyPreviewReport";
            this.menuItemUNIPROMDailyPreviewReport.Click += new System.EventHandler(this.menuItemUNIPROMDailyPreviewReport_Click);
            // 
            // menuItemZIN
            // 
            this.menuItemZIN.Index = 9;
            this.menuItemZIN.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemZINPassesPreview,
            this.menuItemEmplCategories,
            this.menuItemStatisticReport});
            this.menuItemZIN.Text = "ZIN";
            // 
            // menuItemZINPassesPreview
            // 
            this.menuItemZINPassesPreview.Index = 0;
            this.menuItemZINPassesPreview.Text = "Passes preview";
            this.menuItemZINPassesPreview.Click += new System.EventHandler(this.menuItemPassesPreview_Click);
            // 
            // menuItemEmplCategories
            // 
            this.menuItemEmplCategories.Index = 1;
            this.menuItemEmplCategories.Text = "Employees<->Categories";
            this.menuItemEmplCategories.Click += new System.EventHandler(this.menuItemEmplCategories_Click);
            // 
            // menuItemStatisticReport
            // 
            this.menuItemStatisticReport.Index = 2;
            this.menuItemStatisticReport.Text = "Statistic report";
            this.menuItemStatisticReport.Click += new System.EventHandler(this.menuItemStatisticReport_Click);
            // 
            // menuItemEUNET
            // 
            this.menuItemEUNET.Index = 10;
            this.menuItemEUNET.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemEUNETPresenceReport});
            this.menuItemEUNET.Text = "EUNET";
            // 
            // menuItemEUNETPresenceReport
            // 
            this.menuItemEUNETPresenceReport.Index = 0;
            this.menuItemEUNETPresenceReport.Text = "Presence Reports by Types";
            this.menuItemEUNETPresenceReport.Click += new System.EventHandler(this.menuItemEUNETPresenceReport_Click);
            // 
            // menuItemMinistry
            // 
            this.menuItemMinistry.Index = 11;
            this.menuItemMinistry.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuMinistryEmplPresence});
            this.menuItemMinistry.Text = "Ministry";
            // 
            // menuMinistryEmplPresence
            // 
            this.menuMinistryEmplPresence.Index = 0;
            this.menuMinistryEmplPresence.Text = "EmplloyeePresence";
            this.menuMinistryEmplPresence.Click += new System.EventHandler(this.menuMinistryEmplPresence_Click);
            // 
            // menuItemGSK
            // 
            this.menuItemGSK.Index = 12;
            this.menuItemGSK.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemGSKPresenceTracking});
            this.menuItemGSK.Text = "GSK";
            // 
            // menuItemGSKPresenceTracking
            // 
            this.menuItemGSKPresenceTracking.Index = 0;
            this.menuItemGSKPresenceTracking.Text = "GSKPresenceTracking";
            this.menuItemGSKPresenceTracking.Click += new System.EventHandler(this.menuItemGSKPresenceTracking_Click);
            // 
            // menuItemNiksic
            // 
            this.menuItemNiksic.Index = 13;
            this.menuItemNiksic.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemMonthlyReport});
            this.menuItemNiksic.Text = "Niksic";
            // 
            // menuItemMonthlyReport
            // 
            this.menuItemMonthlyReport.Index = 0;
            this.menuItemMonthlyReport.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.menuItemMonthlyReport.Text = "Monthly Report";
            this.menuItemMonthlyReport.Click += new System.EventHandler(this.menuItemMonthlyReport_Click);
            // 
            // menuItemFiat
            // 
            this.menuItemFiat.Index = 14;
            this.menuItemFiat.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPYIntegration,
            this.menuItemEmplCounterBalances,
            this.menuItemFiatDecisions});
            this.menuItemFiat.Text = "FIAT";
            // 
            // menuItemPYIntegration
            // 
            this.menuItemPYIntegration.Index = 0;
            this.menuItemPYIntegration.Text = "PY Integration";
            this.menuItemPYIntegration.Click += new System.EventHandler(this.menuItem8_Click);
            // 
            // menuItemEmplCounterBalances
            // 
            this.menuItemEmplCounterBalances.Index = 1;
            this.menuItemEmplCounterBalances.Text = "Employee counter balances";
            this.menuItemEmplCounterBalances.Click += new System.EventHandler(this.menuItemEmplCounterBalances_Click);
            // 
            // menuItemFiatDecisions
            // 
            this.menuItemFiatDecisions.Index = 2;
            this.menuItemFiatDecisions.Text = "Decisions";
            this.menuItemFiatDecisions.Click += new System.EventHandler(this.menuItemDecisions_Click);
            // 
            // menuDSFReports
            // 
            this.menuDSFReports.Index = 15;
            this.menuDSFReports.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuDSFPresenceReport});
            this.menuDSFReports.Text = "DSF reports";
            // 
            // menuDSFPresenceReport
            // 
            this.menuDSFPresenceReport.Index = 0;
            this.menuDSFPresenceReport.Text = "Presence report";
            this.menuDSFPresenceReport.Click += new System.EventHandler(this.menuDSFPresenceReport_Click);
            // 
            // menuItemLames
            // 
            this.menuItemLames.Index = 16;
            this.menuItemLames.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemLamesCumulativeReport});
            this.menuItemLames.Text = "Lames";
            // 
            // menuItemLamesCumulativeReport
            // 
            this.menuItemLamesCumulativeReport.Index = 0;
            this.menuItemLamesCumulativeReport.Text = "Cumulative report";
            this.menuItemLamesCumulativeReport.Click += new System.EventHandler(this.menuItemLamesCumulativeReport_Click);
            // 
            // meniItemWN
            // 
            this.meniItemWN.Index = 17;
            this.meniItemWN.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.meniItemWNDailyPresence,
            this.menuItemWNMonthly});
            this.meniItemWN.Text = "Waker and Neuson";
            // 
            // meniItemWNDailyPresence
            // 
            this.meniItemWNDailyPresence.Index = 0;
            this.meniItemWNDailyPresence.Text = "DailyPresence";
            this.meniItemWNDailyPresence.Click += new System.EventHandler(this.meniItemWNDailyPresence_Click);
            // 
            // menuItemWNMonthly
            // 
            this.menuItemWNMonthly.Index = 1;
            this.menuItemWNMonthly.Text = "Monthly report";
            this.menuItemWNMonthly.Click += new System.EventHandler(this.menuItemWNMonthly_Click);
            // 
            // menuConfezioniAndrea
            // 
            this.menuConfezioniAndrea.Index = 18;
            this.menuConfezioniAndrea.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuConfezioniAndreaMonthlyReport});
            this.menuConfezioniAndrea.Text = "Confezioni Andrea";
            // 
            // menuConfezioniAndreaMonthlyReport
            // 
            this.menuConfezioniAndreaMonthlyReport.Index = 0;
            this.menuConfezioniAndreaMonthlyReport.Text = "Monthly report";
            this.menuConfezioniAndreaMonthlyReport.Click += new System.EventHandler(this.menuConfezioniAndreaMonthlyReport_Click);
            // 
            // menuItemPMC
            // 
            this.menuItemPMC.Index = 19;
            this.menuItemPMC.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPMCMonthlyReport,
            this.menuItemPMCCumulativeReports,
            this.menuItemPMCStatisticalReports,
            this.menuItemPMCPaymentReport});
            this.menuItemPMC.Text = "PMC izveštaji";
            // 
            // menuItemPMCMonthlyReport
            // 
            this.menuItemPMCMonthlyReport.Index = 0;
            this.menuItemPMCMonthlyReport.Text = "Monthly report";
            this.menuItemPMCMonthlyReport.Click += new System.EventHandler(this.menuItemPMCMonthlyReport_Click);
            // 
            // menuItemPMCCumulativeReports
            // 
            this.menuItemPMCCumulativeReports.Index = 1;
            this.menuItemPMCCumulativeReports.Text = "Cumulative reports";
            this.menuItemPMCCumulativeReports.Click += new System.EventHandler(this.menuItemPMCCumulativeReports_Click);
            // 
            // menuItemPMCStatisticalReports
            // 
            this.menuItemPMCStatisticalReports.Index = 2;
            this.menuItemPMCStatisticalReports.Text = "Statistical reports";
            this.menuItemPMCStatisticalReports.Click += new System.EventHandler(this.menuItemPMCStatisticalReports_Click);
            // 
            // menuItemPMCPaymentReport
            // 
            this.menuItemPMCPaymentReport.Index = 3;
            this.menuItemPMCPaymentReport.Text = "Payment report";
            this.menuItemPMCPaymentReport.Click += new System.EventHandler(this.menuItemPMCPaymentReport_Click);
            // 
            // menuItemGeox
            // 
            this.menuItemGeox.Index = 20;
            this.menuItemGeox.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemGeoxPYReport});
            this.menuItemGeox.Text = "Geox";
            // 
            // menuItemGeoxPYReport
            // 
            this.menuItemGeoxPYReport.Index = 0;
            this.menuItemGeoxPYReport.Text = "Payment report";
            this.menuItemGeoxPYReport.Click += new System.EventHandler(this.menuItemGeoxPYReport_Click);
            // 
            // menuItemGrundfos
            // 
            this.menuItemGrundfos.Index = 21;
            this.menuItemGrundfos.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemGrundfosTransportData,
            this.menuItemGrundfosPYReport});
            this.menuItemGrundfos.Text = "Grundfos";
            // 
            // menuItemGrundfosTransportData
            // 
            this.menuItemGrundfosTransportData.Index = 0;
            this.menuItemGrundfosTransportData.Text = "Transport data";
            this.menuItemGrundfosTransportData.Click += new System.EventHandler(this.menuItemGrundfosTransportData_Click);
            // 
            // menuItemGrundfosPYReport
            // 
            this.menuItemGrundfosPYReport.Index = 1;
            this.menuItemGrundfosPYReport.Text = "PY report";
            this.menuItemGrundfosPYReport.Click += new System.EventHandler(this.menuItemGrundfosPYReport_Click);
            // 
            // menuItemHyatt
            // 
            this.menuItemHyatt.Index = 22;
            this.menuItemHyatt.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemHyattPYReport,
            this.menuItemHyattTimeAndAttendance,
            this.menuItemHyattOperatersCategoriesReport,
            this.menuItemHyattOperatersRolesReport,
            this.menuItemHyattOperatersOUWUReport,
            this.menuItemHyattCategoriesPrivilegesReport,
            this.menuItemHyattRolePrivilegesReport,
            this.menuItemEmployeesPersonalHoliday,
            this.menuItemHyattPYGIDReport,
            this.menuItemAnualLeaveReport,
            this.menuItemStartWorkingDate});
            this.menuItemHyatt.Text = "Hyatt";
            // 
            // menuItemHyattPYReport
            // 
            this.menuItemHyattPYReport.Index = 0;
            this.menuItemHyattPYReport.Text = "Payment report";
            this.menuItemHyattPYReport.Click += new System.EventHandler(this.menuItemHyattPYReport_Click);
            // 
            // menuItemHyattTimeAndAttendance
            // 
            this.menuItemHyattTimeAndAttendance.Index = 1;
            this.menuItemHyattTimeAndAttendance.Text = "Time and attendance report";
            this.menuItemHyattTimeAndAttendance.Click += new System.EventHandler(this.menuItemHyattTimeAndAtendanceReport_Click);
            // 
            // menuItemHyattOperatersCategoriesReport
            // 
            this.menuItemHyattOperatersCategoriesReport.Index = 2;
            this.menuItemHyattOperatersCategoriesReport.Text = "Operaters report";
            this.menuItemHyattOperatersCategoriesReport.Click += new System.EventHandler(this.menuItemHyattOperatersCategoriesReport_Click);
            // 
            // menuItemHyattOperatersRolesReport
            // 
            this.menuItemHyattOperatersRolesReport.Index = 3;
            this.menuItemHyattOperatersRolesReport.Text = "Operaters roles report";
            this.menuItemHyattOperatersRolesReport.Click += new System.EventHandler(this.menuItemHyattOperatersRolesReport_Click);
            // 
            // menuItemHyattOperatersOUWUReport
            // 
            this.menuItemHyattOperatersOUWUReport.Index = 4;
            this.menuItemHyattOperatersOUWUReport.Text = "Operaters OUWU report";
            this.menuItemHyattOperatersOUWUReport.Click += new System.EventHandler(this.menuItemHyattOperatersOUWUReport_Click);
            // 
            // menuItemHyattCategoriesPrivilegesReport
            // 
            this.menuItemHyattCategoriesPrivilegesReport.Index = 5;
            this.menuItemHyattCategoriesPrivilegesReport.Text = "Categories privileges report";
            this.menuItemHyattCategoriesPrivilegesReport.Click += new System.EventHandler(this.menuItemHyattCategoriesPrivilegesReport_Click);
            // 
            // menuItemHyattRolePrivilegesReport
            // 
            this.menuItemHyattRolePrivilegesReport.Index = 6;
            this.menuItemHyattRolePrivilegesReport.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemHyattRolesPrivilegiesReportHRSSC,
            this.menuItemHyattRolesPrivilegiesReportPY,
            this.menuItemHyattRolesPrivilegiesReportCANTEEN,
            this.menuItemHyattRolesPrivilegiesReportHRSSCEXTENDED,
            this.menuItemHyattRolesPrivilegiesReportMedicalCheck,
            this.menuItemHyattRolesPrivilegiesReportSystemClose});
            this.menuItemHyattRolePrivilegesReport.Text = "Roles privileges report";
            // 
            // menuItemHyattRolesPrivilegiesReportHRSSC
            // 
            this.menuItemHyattRolesPrivilegiesReportHRSSC.Index = 0;
            this.menuItemHyattRolesPrivilegiesReportHRSSC.Text = "HRSSC";
            this.menuItemHyattRolesPrivilegiesReportHRSSC.Click += new System.EventHandler(this.menuItemHyattRolesPrivilegiesReportHRSSC_Click);
            // 
            // menuItemHyattRolesPrivilegiesReportPY
            // 
            this.menuItemHyattRolesPrivilegiesReportPY.Index = 1;
            this.menuItemHyattRolesPrivilegiesReportPY.Text = "PY";
            this.menuItemHyattRolesPrivilegiesReportPY.Click += new System.EventHandler(this.menuItemHyattRolesPrivilegiesReportPY_Click_1);
            // 
            // menuItemHyattRolesPrivilegiesReportCANTEEN
            // 
            this.menuItemHyattRolesPrivilegiesReportCANTEEN.Index = 2;
            this.menuItemHyattRolesPrivilegiesReportCANTEEN.Text = "CANTEEN";
            this.menuItemHyattRolesPrivilegiesReportCANTEEN.Click += new System.EventHandler(this.menuItemHyattRolesPrivilegiesReportCANTEEN_Click);
            // 
            // menuItemHyattRolesPrivilegiesReportHRSSCEXTENDED
            // 
            this.menuItemHyattRolesPrivilegiesReportHRSSCEXTENDED.Index = 3;
            this.menuItemHyattRolesPrivilegiesReportHRSSCEXTENDED.Text = "HRSSC EXTENDED";
            this.menuItemHyattRolesPrivilegiesReportHRSSCEXTENDED.Click += new System.EventHandler(this.menuItemHyattRolesPrivilegiesReportHRSSCEXTENDED_Click);
            // 
            // menuItemHyattRolesPrivilegiesReportMedicalCheck
            // 
            this.menuItemHyattRolesPrivilegiesReportMedicalCheck.Index = 4;
            this.menuItemHyattRolesPrivilegiesReportMedicalCheck.Text = "Medical check";
            this.menuItemHyattRolesPrivilegiesReportMedicalCheck.Click += new System.EventHandler(this.menuItemHyattRolesPrivilegiesReportMedicalCheck_Click_1);
            // 
            // menuItemHyattRolesPrivilegiesReportSystemClose
            // 
            this.menuItemHyattRolesPrivilegiesReportSystemClose.Index = 5;
            this.menuItemHyattRolesPrivilegiesReportSystemClose.Text = "System close";
            this.menuItemHyattRolesPrivilegiesReportSystemClose.Click += new System.EventHandler(this.menuItemHyattRolesPrivilegiesReportSystemClose_Click);
            // 
            // menuItemEmployeesPersonalHoliday
            // 
            this.menuItemEmployeesPersonalHoliday.Index = 7;
            this.menuItemEmployeesPersonalHoliday.Text = "Employees personal holidays";
            this.menuItemEmployeesPersonalHoliday.Click += new System.EventHandler(this.menuItemEmployeesPersonalHoliday_Click);
            // 
            // menuItemHyattPYGIDReport
            // 
            this.menuItemHyattPYGIDReport.Index = 8;
            this.menuItemHyattPYGIDReport.Text = "Payment report with GID";
            this.menuItemHyattPYGIDReport.Click += new System.EventHandler(this.menuItemHyattPYGIDReport_Click);
            // 
            // menuItemAnualLeaveReport
            // 
            this.menuItemAnualLeaveReport.Index = 9;
            this.menuItemAnualLeaveReport.Text = "Anual leave report";
            this.menuItemAnualLeaveReport.Click += new System.EventHandler(this.menuItemAnualLeaveReport_Click);
            // 
            // menuItemStartWorkingDate
            // 
            this.menuItemStartWorkingDate.Index = 10;
            this.menuItemStartWorkingDate.Text = "Start working date";
            this.menuItemStartWorkingDate.Click += new System.EventHandler(this.menuItemStartWorkingDate_Click);
            // 
            // menuItemMagna
            // 
            this.menuItemMagna.Index = 23;
            this.menuItemMagna.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemEmployeePT});
            this.menuItemMagna.Text = "Magna";
            // 
            // menuItemEmployeePT
            // 
            this.menuItemEmployeePT.Index = 0;
            this.menuItemEmployeePT.Text = "Employee pass types";
            this.menuItemEmployeePT.Click += new System.EventHandler(this.menuItemEmployeePT_Click);
            // 
            // menuMapsReports
            // 
            this.menuMapsReports.Index = 3;
            this.menuMapsReports.Text = "Maps";
            this.menuMapsReports.Click += new System.EventHandler(this.menuMapsReports_Click);
            // 
            // menuPeopleCounter
            // 
            this.menuPeopleCounter.Index = 4;
            this.menuPeopleCounter.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuStandardReport,
            this.menuAdvancedReport,
            this.menuBasicReport});
            this.menuPeopleCounter.Text = "People counter";
            // 
            // menuStandardReport
            // 
            this.menuStandardReport.Index = 0;
            this.menuStandardReport.Text = "Standard report";
            this.menuStandardReport.Click += new System.EventHandler(this.menuStandardReport_Click);
            // 
            // menuAdvancedReport
            // 
            this.menuAdvancedReport.Index = 1;
            this.menuAdvancedReport.Text = "Advanced report";
            this.menuAdvancedReport.Click += new System.EventHandler(this.menuAdvancedReport_Click);
            // 
            // menuBasicReport
            // 
            this.menuBasicReport.Index = 2;
            this.menuBasicReport.Text = "Basic report";
            this.menuBasicReport.Click += new System.EventHandler(this.menuBasicReport_Click);
            // 
            // menuDocManipulation
            // 
            this.menuDocManipulation.Index = 5;
            this.menuDocManipulation.Text = "Document manipulation";
            this.menuDocManipulation.Click += new System.EventHandler(this.menuDocManipulation_Click);
            // 
            // menuInterventions
            // 
            this.menuInterventions.Index = 2;
            this.menuInterventions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPasses,
            this.menuIOPairs,
            this.menuEmployeeAbsences,
            this.menuExitPermissions,
            this.menuExtraHours,
            this.menuItemVacationEvidence,
            this.menuItemEnterDataByEmployee,
            this.menuItemMassiveInput,
            this.menuRecordsOfBreaks});
            this.menuInterventions.Text = "Interventions";
            // 
            // menuPasses
            // 
            this.menuPasses.Index = 0;
            this.menuPasses.Text = "Passes";
            this.menuPasses.Click += new System.EventHandler(this.menuPasses_Click);
            // 
            // menuIOPairs
            // 
            this.menuIOPairs.Index = 1;
            this.menuIOPairs.Text = "IO Pairs";
            this.menuIOPairs.Click += new System.EventHandler(this.menuIOPairs_Click);
            // 
            // menuEmployeeAbsences
            // 
            this.menuEmployeeAbsences.Index = 2;
            this.menuEmployeeAbsences.Text = "Employee Absences";
            this.menuEmployeeAbsences.Click += new System.EventHandler(this.menuEmployeeAbsences_Click);
            // 
            // menuExitPermissions
            // 
            this.menuExitPermissions.Index = 3;
            this.menuExitPermissions.Text = "Exit Permissions";
            this.menuExitPermissions.Click += new System.EventHandler(this.menuExitPermissions_Click);
            // 
            // menuExtraHours
            // 
            this.menuExtraHours.Index = 4;
            this.menuExtraHours.Text = "Extra hours";
            this.menuExtraHours.Click += new System.EventHandler(this.menuExtraHours_Click);
            // 
            // menuItemVacationEvidence
            // 
            this.menuItemVacationEvidence.Index = 5;
            this.menuItemVacationEvidence.Text = "Vacation evidence";
            this.menuItemVacationEvidence.Click += new System.EventHandler(this.menuItemVacationEvidence_Click);
            // 
            // menuItemEnterDataByEmployee
            // 
            this.menuItemEnterDataByEmployee.Index = 6;
            this.menuItemEnterDataByEmployee.Text = "Enter data by employee";
            this.menuItemEnterDataByEmployee.Click += new System.EventHandler(this.menuItemEnterDataByEmployee_Click);
            // 
            // menuItemMassiveInput
            // 
            this.menuItemMassiveInput.Index = 7;
            this.menuItemMassiveInput.Text = "Massive input";
            this.menuItemMassiveInput.Click += new System.EventHandler(this.menuItemMassiveInput_Click);
            // 
            // menuRecordsOfBreaks
            // 
            this.menuRecordsOfBreaks.Index = 8;
            this.menuRecordsOfBreaks.Text = "Records of Breaks";
            this.menuRecordsOfBreaks.Click += new System.EventHandler(this.menuRecordsOfBreaks_Click);
            // 
            // menuMaintaining
            // 
            this.menuMaintaining.Index = 3;
            this.menuMaintaining.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuLibExport,
            this.menuImportData,
            this.menuItem3,
            this.menuProcessingTables,
            this.menuReadersSettings,
            this.menuPicManagament,
            this.menuLog2XML,
            this.menuItem5,
            this.menuMappingSiPass,
            this.menuConnectionSetup,
            this.menuItemUNIPROMRAMP,
            this.menuItem6,
            this.menuItemTagsPrview,
            this.menuItemLastTerminalReading,
            this.menuItem7,
            this.menuItemRestartCounters,
            this.menuItemSynchronization});
            this.menuMaintaining.Text = "Maintaining";
            // 
            // menuLibExport
            // 
            this.menuLibExport.Index = 0;
            this.menuLibExport.Text = "Export Libraries";
            this.menuLibExport.Click += new System.EventHandler(this.menuLibExport_Click);
            // 
            // menuImportData
            // 
            this.menuImportData.Index = 1;
            this.menuImportData.Text = "Import Data";
            this.menuImportData.Click += new System.EventHandler(this.menuImportData_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.Text = "-";
            // 
            // menuProcessingTables
            // 
            this.menuProcessingTables.Index = 3;
            this.menuProcessingTables.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuImportLog,
            this.menuLogToPasses,
            this.menuPassesToIOPairs});
            this.menuProcessingTables.Text = "Processing Tables";
            // 
            // menuImportLog
            // 
            this.menuImportLog.Index = 0;
            this.menuImportLog.Text = "Import Log";
            this.menuImportLog.Click += new System.EventHandler(this.menuImportLog_Click);
            // 
            // menuLogToPasses
            // 
            this.menuLogToPasses.Enabled = false;
            this.menuLogToPasses.Index = 1;
            this.menuLogToPasses.Text = "Log to Passes";
            this.menuLogToPasses.Click += new System.EventHandler(this.menuLogToPasses_Click);
            // 
            // menuPassesToIOPairs
            // 
            this.menuPassesToIOPairs.Enabled = false;
            this.menuPassesToIOPairs.Index = 2;
            this.menuPassesToIOPairs.Text = "Passes to IOPairs";
            this.menuPassesToIOPairs.Click += new System.EventHandler(this.menuPassesToIOPairs_Click);
            // 
            // menuReadersSettings
            // 
            this.menuReadersSettings.Index = 4;
            this.menuReadersSettings.Text = "Readers Settings";
            this.menuReadersSettings.Click += new System.EventHandler(this.menuReadersSettings_Click);
            // 
            // menuPicManagament
            // 
            this.menuPicManagament.Index = 5;
            this.menuPicManagament.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuEmplPhotoManagament,
            this.menuCameraSnapshostManagament,
            this.menuItemVisitors});
            this.menuPicManagament.Text = "Picture managament";
            // 
            // menuEmplPhotoManagament
            // 
            this.menuEmplPhotoManagament.Index = 0;
            this.menuEmplPhotoManagament.Text = "Employee photo managament";
            this.menuEmplPhotoManagament.Click += new System.EventHandler(this.menuEmplPhotoManagament_Click);
            // 
            // menuCameraSnapshostManagament
            // 
            this.menuCameraSnapshostManagament.Index = 1;
            this.menuCameraSnapshostManagament.Text = "Camera snapshots managament";
            this.menuCameraSnapshostManagament.Click += new System.EventHandler(this.menuCameraSnapshostManagament_Click);
            // 
            // menuItemVisitors
            // 
            this.menuItemVisitors.Index = 2;
            this.menuItemVisitors.Text = "Visitors";
            this.menuItemVisitors.Click += new System.EventHandler(this.menuItemVisitors_Click);
            // 
            // menuLog2XML
            // 
            this.menuLog2XML.Index = 6;
            this.menuLog2XML.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
            this.menuLog2XML.Text = "Log -> XML";
            this.menuLog2XML.Click += new System.EventHandler(this.menuLog2XML_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 7;
            this.menuItem5.Text = "-";
            // 
            // menuMappingSiPass
            // 
            this.menuMappingSiPass.Index = 8;
            this.menuMappingSiPass.Text = "Mapping ACTA <-> SiPass";
            this.menuMappingSiPass.Click += new System.EventHandler(this.menuMappingSiPass_Click);
            // 
            // menuConnectionSetup
            // 
            this.menuConnectionSetup.Index = 9;
            this.menuConnectionSetup.Text = "Asco DB connection setup";
            this.menuConnectionSetup.Click += new System.EventHandler(this.menuConnectionSetup_Click);
            // 
            // menuItemUNIPROMRAMP
            // 
            this.menuItemUNIPROMRAMP.Index = 10;
            this.menuItemUNIPROMRAMP.Text = "Ramp-UNIPROM";
            this.menuItemUNIPROMRAMP.Click += new System.EventHandler(this.menuItemUNIPROMRAMP_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 11;
            this.menuItem6.Text = "-";
            // 
            // menuItemTagsPrview
            // 
            this.menuItemTagsPrview.Index = 12;
            this.menuItemTagsPrview.Text = "Tag preview";
            this.menuItemTagsPrview.Click += new System.EventHandler(this.menuItemTagsPrview_Click);
            // 
            // menuItemLastTerminalReading
            // 
            this.menuItemLastTerminalReading.Index = 13;
            this.menuItemLastTerminalReading.Text = "Last terminal reading";
            this.menuItemLastTerminalReading.Click += new System.EventHandler(this.menuItemLastTerminalReading_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 14;
            this.menuItem7.Text = "-";
            // 
            // menuItemRestartCounters
            // 
            this.menuItemRestartCounters.Index = 15;
            this.menuItemRestartCounters.Text = "Restart counters";
            this.menuItemRestartCounters.Click += new System.EventHandler(this.menuItemRestartCounters_Click);
            // 
            // menuItemSynchronization
            // 
            this.menuItemSynchronization.Index = 16;
            this.menuItemSynchronization.Text = "Synchronization";
            this.menuItemSynchronization.Click += new System.EventHandler(this.menuItemSynchronization_Click);
            // 
            // menuConfiguration
            // 
            this.menuConfiguration.Index = 4;
            this.menuConfiguration.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuDataAccess,
            this.menuUserRoles,
            this.menuAccessControl,
            this.menuRules,
            this.menuCutOffDate,
            this.menuItemSystem});
            this.menuConfiguration.Text = "Configuration";
            // 
            // menuDataAccess
            // 
            this.menuDataAccess.Index = 0;
            this.menuDataAccess.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuEmplAccess,
            this.menuEmplResponisibility});
            this.menuDataAccess.Text = "Data Access";
            // 
            // menuEmplAccess
            // 
            this.menuEmplAccess.Index = 0;
            this.menuEmplAccess.Text = "Employee Access";
            this.menuEmplAccess.Click += new System.EventHandler(this.menuReportEmplAccsess_Click);
            // 
            // menuEmplResponisibility
            // 
            this.menuEmplResponisibility.Index = 1;
            this.menuEmplResponisibility.Text = "Employee responsibility";
            this.menuEmplResponisibility.Click += new System.EventHandler(this.menuEmplResponisibility_Click);
            // 
            // menuUserRoles
            // 
            this.menuUserRoles.Index = 1;
            this.menuUserRoles.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuUsers,
            this.menuRoles,
            this.menuUsersRoles});
            this.menuUserRoles.Text = "Users and Roles";
            // 
            // menuUsers
            // 
            this.menuUsers.Index = 0;
            this.menuUsers.Text = "Users";
            this.menuUsers.Click += new System.EventHandler(this.menuUsers_Click);
            // 
            // menuRoles
            // 
            this.menuRoles.Index = 1;
            this.menuRoles.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuRoleMaintainence,
            this.menuRolePrivileges});
            this.menuRoles.Text = "Roles";
            // 
            // menuRoleMaintainence
            // 
            this.menuRoleMaintainence.Index = 0;
            this.menuRoleMaintainence.Text = "Maintainenece";
            this.menuRoleMaintainence.Click += new System.EventHandler(this.menuRoleMaintainence_Click);
            // 
            // menuRolePrivileges
            // 
            this.menuRolePrivileges.Index = 1;
            this.menuRolePrivileges.Text = "Privileges";
            this.menuRolePrivileges.Click += new System.EventHandler(this.menuRolePrivileges_Click);
            // 
            // menuUsersRoles
            // 
            this.menuUsersRoles.Index = 2;
            this.menuUsersRoles.Text = "Users <-> Roles";
            this.menuUsersRoles.Click += new System.EventHandler(this.menuUsersRoles_Click);
            // 
            // menuAccessControl
            // 
            this.menuAccessControl.Index = 2;
            this.menuAccessControl.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuAccessControlGroups,
            this.menuEmployeeAccessControlGroups,
            this.menuAccessControlGroupsGates,
            this.menuAccessProfiles,
            this.menuApplyAccessControlParameters,
            this.menuAccessControlStatus});
            this.menuAccessControl.Text = "Access control";
            this.menuAccessControl.Click += new System.EventHandler(this.menuAccessControl_Click);
            // 
            // menuAccessControlGroups
            // 
            this.menuAccessControlGroups.Index = 0;
            this.menuAccessControlGroups.Text = "Access control groups";
            this.menuAccessControlGroups.Click += new System.EventHandler(this.menuAccessControlGroups_Click);
            // 
            // menuEmployeeAccessControlGroups
            // 
            this.menuEmployeeAccessControlGroups.Index = 1;
            this.menuEmployeeAccessControlGroups.Text = "Employee <-> Access control groups";
            this.menuEmployeeAccessControlGroups.Click += new System.EventHandler(this.menuEmployeeAccessControlGroups_Click);
            // 
            // menuAccessControlGroupsGates
            // 
            this.menuAccessControlGroupsGates.Index = 2;
            this.menuAccessControlGroupsGates.Text = "Access control groups <-> Gates";
            this.menuAccessControlGroupsGates.Click += new System.EventHandler(this.menuAccessControlGroupsGates_Click);
            // 
            // menuAccessProfiles
            // 
            this.menuAccessProfiles.Index = 3;
            this.menuAccessProfiles.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuTimeAccessProfiles,
            this.menuGateAccessProfiles,
            this.menuAssignGateProfile});
            this.menuAccessProfiles.Text = "Profiles";
            // 
            // menuTimeAccessProfiles
            // 
            this.menuTimeAccessProfiles.Index = 0;
            this.menuTimeAccessProfiles.Text = "Weekly access schedules";
            this.menuTimeAccessProfiles.Click += new System.EventHandler(this.menuTimeAccessProfiles_Click);
            // 
            // menuGateAccessProfiles
            // 
            this.menuGateAccessProfiles.Index = 1;
            this.menuGateAccessProfiles.Text = "Gate access profiles";
            this.menuGateAccessProfiles.Click += new System.EventHandler(this.menuGateAccessProfiles_Click);
            // 
            // menuAssignGateProfile
            // 
            this.menuAssignGateProfile.Index = 2;
            this.menuAssignGateProfile.Text = "Gates <-> Weekly access schedules";
            this.menuAssignGateProfile.Click += new System.EventHandler(this.menuAssignGateProfile_Click);
            // 
            // menuApplyAccessControlParameters
            // 
            this.menuApplyAccessControlParameters.Index = 4;
            this.menuApplyAccessControlParameters.Text = "Apply access control parameters";
            this.menuApplyAccessControlParameters.Click += new System.EventHandler(this.menuApplyAccessControlParameters_Click);
            // 
            // menuAccessControlStatus
            // 
            this.menuAccessControlStatus.Index = 5;
            this.menuAccessControlStatus.Text = "Access control status";
            this.menuAccessControlStatus.Click += new System.EventHandler(this.menuAccessControlStatus_Click);
            // 
            // menuRules
            // 
            this.menuRules.Index = 3;
            this.menuRules.Text = "Rules setting";
            this.menuRules.Click += new System.EventHandler(this.menuRules_Click);
            // 
            // menuCutOffDate
            // 
            this.menuCutOffDate.Index = 4;
            this.menuCutOffDate.Text = "Cut off date setting";
            this.menuCutOffDate.Click += new System.EventHandler(this.menuCutOffDate_Click);
            // 
            // menuItemSystem
            // 
            this.menuItemSystem.Index = 5;
            this.menuItemSystem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSystemClosingEvents,
            this.menuItemSystemMessages,
            this.menuItemCounterType,
            this.menuItemUsersCategory,
            this.menuItemULChangesTbl});
            this.menuItemSystem.Text = "System";
            // 
            // menuItemSystemClosingEvents
            // 
            this.menuItemSystemClosingEvents.Index = 0;
            this.menuItemSystemClosingEvents.Text = "Closing events";
            this.menuItemSystemClosingEvents.Click += new System.EventHandler(this.menuItemSystemClosingEvents_Click);
            // 
            // menuItemSystemMessages
            // 
            this.menuItemSystemMessages.Index = 1;
            this.menuItemSystemMessages.Text = "Messages";
            this.menuItemSystemMessages.Click += new System.EventHandler(this.menuItemSystemMessages_Click);
            // 
            // menuItemCounterType
            // 
            this.menuItemCounterType.Index = 2;
            this.menuItemCounterType.Text = "Tip brojaca";
            this.menuItemCounterType.Click += new System.EventHandler(this.menuItemCounterType_Click);
            // 
            // menuItemUsersCategory
            // 
            this.menuItemUsersCategory.Index = 3;
            this.menuItemUsersCategory.Text = "Kategorije";
            this.menuItemUsersCategory.Click += new System.EventHandler(this.menuItemUsersCategory_Click);
            // 
            // menuItemULChangesTbl
            // 
            this.menuItemULChangesTbl.Index = 4;
            this.menuItemULChangesTbl.Text = "Tabele";
            this.menuItemULChangesTbl.Click += new System.EventHandler(this.menuItemULChangesTbl_Click);
            // 
            // menuItemMonitor
            // 
            this.menuItemMonitor.Index = 5;
            this.menuItemMonitor.Text = "Monitor";
            this.menuItemMonitor.Click += new System.EventHandler(this.menuItemMonitor_Click);
            // 
            // menuVisitors
            // 
            this.menuVisitors.Index = 6;
            this.menuVisitors.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuVisitorsEnter,
            this.menuVisitorsExit,
            this.menuVisitorsReport});
            this.menuVisitors.Text = "Visitors";
            this.menuVisitors.Select += new System.EventHandler(this.menuVisitors_Select);
            this.menuVisitors.Click += new System.EventHandler(this.menuVisitors_Click);
            // 
            // menuVisitorsEnter
            // 
            this.menuVisitorsEnter.Index = 0;
            this.menuVisitorsEnter.Text = "Enter";
            this.menuVisitorsEnter.Click += new System.EventHandler(this.menuVisitorsEnter_Click);
            // 
            // menuVisitorsExit
            // 
            this.menuVisitorsExit.Index = 1;
            this.menuVisitorsExit.Text = "Exit";
            this.menuVisitorsExit.Click += new System.EventHandler(this.menuVisitorsExit_Click);
            // 
            // menuVisitorsReport
            // 
            this.menuVisitorsReport.Index = 2;
            this.menuVisitorsReport.Text = "Visitors Report";
            this.menuVisitorsReport.Click += new System.EventHandler(this.menuVisitorsReport_Click);
            // 
            // menuRestaurant
            // 
            this.menuRestaurant.Index = 7;
            this.menuRestaurant.Text = "Restaurant ";
            this.menuRestaurant.Click += new System.EventHandler(this.menuRestaurant_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.Index = 8;
            this.menuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuManual,
            this.menuAbout,
            this.menuInformations});
            this.menuHelp.Text = "Help";
            // 
            // menuManual
            // 
            this.menuManual.Index = 0;
            this.menuManual.Text = "Manual";
            this.menuManual.Click += new System.EventHandler(this.menuManual_Click);
            // 
            // menuAbout
            // 
            this.menuAbout.Index = 1;
            this.menuAbout.Text = "About Project";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // menuInformations
            // 
            this.menuInformations.Index = 2;
            this.menuInformations.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
            this.menuInformations.Text = "Information";
            this.menuInformations.Click += new System.EventHandler(this.menuInformations_Click);
            // 
            // ACTAStatusBar
            // 
            this.ACTAStatusBar.Location = new System.Drawing.Point(0, 508);
            this.ACTAStatusBar.Name = "ACTAStatusBar";
            this.ACTAStatusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.sbPanelDate,
            this.sbPanelUser,
            this.sbPanelLogInTime,
            this.sbPanelDB});
            this.ACTAStatusBar.Size = new System.Drawing.Size(794, 22);
            this.ACTAStatusBar.TabIndex = 3;
            // 
            // sbPanelDate
            // 
            this.sbPanelDate.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.sbPanelDate.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
            this.sbPanelDate.MinWidth = 50;
            this.sbPanelDate.Name = "sbPanelDate";
            this.sbPanelDate.Width = 50;
            // 
            // sbPanelUser
            // 
            this.sbPanelUser.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.sbPanelUser.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
            this.sbPanelUser.Name = "sbPanelUser";
            this.sbPanelUser.Width = 10;
            // 
            // sbPanelLogInTime
            // 
            this.sbPanelLogInTime.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.sbPanelLogInTime.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
            this.sbPanelLogInTime.MinWidth = 70;
            this.sbPanelLogInTime.Name = "sbPanelLogInTime";
            // 
            // sbPanelDB
            // 
            this.sbPanelDB.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.sbPanelDB.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.sbPanelDB.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
            this.sbPanelDB.Name = "sbPanelDB";
            this.sbPanelDB.Width = 10;
            // 
            // pbClientLogo
            // 
            this.pbClientLogo.Image = global::ACTAAdmin.ResImages.ACTASplash;
            this.pbClientLogo.Location = new System.Drawing.Point(653, 447);
            this.pbClientLogo.Name = "pbClientLogo";
            this.pbClientLogo.Size = new System.Drawing.Size(106, 30);
            this.pbClientLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbClientLogo.TabIndex = 4;
            this.pbClientLogo.TabStop = false;
            // 
            // pbLogo
            // 
            this.pbLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbLogo.Image")));
            this.pbLogo.Location = new System.Drawing.Point(26, 424);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(178, 69);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 2;
            this.pbLogo.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, -4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(794, 534);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // ACTA
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(794, 530);
            this.Controls.Add(this.pbClientLogo);
            this.Controls.Add(this.ACTAStatusBar);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.Name = "ACTA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ACTA Admin";
            this.Load += new System.EventHandler(this.ACTA_Load);
            this.Closed += new System.EventHandler(this.ACTA_Closed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ACTA_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelLogInTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbPanelDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClientLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void menuEmployees_Click(object sender, System.EventArgs e)
        {
            try
            {
                Employees employees = new Employees();

                NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuEmployees.Text);

                employees.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuEmployees_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        [STAThread]
        public static void Main(string[] args)
        {
            bool newMutexCreated = false;

            try
            {
                string mutexName = "Local\\" +
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                Mutex mutex = null;
                try
                {
                    // Create a new mutex object with a unique name
                    mutex = new Mutex(false, mutexName, out newMutexCreated);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace +
                        "\n\n" + "Application Exiting...", "Exception thrown");
                    Application.Exit();
                }

                if (newMutexCreated ||
                   ((ConfigurationManager.AppSettings["MultipleInstances"] != null) &&
                    (ConfigurationManager.AppSettings["MultipleInstances"].ToUpper() == "YES")))
                {
                    //ACTA acta = new ACTA();
                    //Application.Run(acta);

                    string connString = ConfigurationManager.AppSettings["connectionString"];
                    if (connString == null || connString.Equals(""))
                    {
                        // if there is no connection string in App.config or it is empty string, ask user to enter connection parameters
                        ResourceManager rm;
                        CultureInfo culture;
                        culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                        rm = new ResourceManager("UI.Resource", typeof(Employees).Assembly);
                        DialogResult result = MessageBox.Show(rm.GetString("msgConnStringNotFound", culture), "", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {
                            DBSetup dbSetup = new DBSetup("", "", "", "", Constants.dbSetupACTA);
                            dbSetup.ShowDialog();
                        }
                        else
                        {
                            return;
                        }
                    }
                    string userName = "";
                    string userPassword = "";
                    if (args.Length == 2)
                    {
                        userName = args[0];
                        userPassword = args[1];
                    }


                    ACTAAdminApplicationContext context = new ACTAAdminApplicationContext(userName, userPassword);

                    if (!context.isMainFormClosed)
                    {
                        Application.Run(context);
                    }
                }
                else
                {
                    ResourceManager rm;
                    CultureInfo culture;
                    culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                    rm = new ResourceManager("UI.Resource", typeof(Employees).Assembly);
                    MessageBox.Show(rm.GetString("applRunning", culture));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitializeObserverClient()
        {
            observerClient = new NotificationObserverClient(this.ToString());
            Controller = NotificationController.GetInstance();
            Controller.AttachToNotifier(observerClient);
            this.observerClient.Notification += new NotificationEventHandler(this.setCurrentUser);
        }

        private void menuWorkingUnits_Click(object sender, System.EventArgs e)
        {
            try
            {
                WorkingUnits wu = new WorkingUnits();

                NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuWorkingUnits.Text);

                wu.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuWorkingUnits_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuLocations_Click(object sender, System.EventArgs e)
        {
            try
            {
                Locations locations = new Locations();

                NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuLocations.Text);

                locations.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuLocations_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuPassType_Click(object sender, System.EventArgs e)
        {
            try
            {
                //PassTypes passTypes = new PassTypes();
                TMTypes passTypes = new TMTypes();
                NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuPassTypes.Text);

                passTypes.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuPassType_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("ACTAAdminForm", culture);
                // menu Libreries text
                menuLibraries.Text = rm.GetString("menuLibraries", culture);
                menuEmployees.Text = rm.GetString("menuEmployees", culture);
                    menuWorkingUnits.Text = rm.GetString("menuWorkingUnits", culture);
                menuItemOrganizationalUnits.Text = rm.GetString("menuItemOrganizationalUnits", culture);
                menuPassTypes.Text = rm.GetString("menuPassTypes", culture);
                menuLocations.Text = rm.GetString("menuLocations", culture);
                menuGates.Text = rm.GetString("menuGates", culture);
                menuReaders.Text = rm.GetString("menuReaders", culture);
                menuCameras.Text = rm.GetString("menuCameras", culture);
                menuExit.Text = rm.GetString("menuExit", culture);
                menuWT.Text = rm.GetString("menuWT", culture);
                menuWTSchema.Text = rm.GetString("WTSchemaTitle", culture);
                menuHolidays.Text = rm.GetString("menuHolidays", culture);
                menuWTGroups.Text = rm.GetString("menuWTGroups", culture);
                menuProlongTS.Text = rm.GetString("menuProlongTS", culture);
                menuPauses.Text = rm.GetString("menuPauses", culture);
                menuItemRestaurant.Text = rm.GetString("menuRestaurant", culture);
                menuRoutes.Text = rm.GetString("menuRoutesTag", culture);
                menuRoutesTerminal.Text = rm.GetString("menuRoutesTerminal", culture);
                menuMaps.Text = rm.GetString("menuMaps", culture);
                menuPeopleCounter.Text = rm.GetString("menuPeopleCounter", culture);
                menuRestaurant.Text = rm.GetString("menuRestaurant", culture);
                menuItemMC.Text = rm.GetString("medCheck", culture);
                menuRecordsOfBreaks.Text = rm.GetString("menuRecordsOfBreaks", culture);//  17.01.2020. BOJAN
                menuMachines.Text = rm.GetString("menuMachines", culture);//  05.02.2020. BOJAN

                // menu Reports text
                menuReports.Text = rm.GetString("menuReports", culture);
                menuStandardRep.Text = rm.GetString("menuStandardRep", culture);
                menuDocManipulation.Text = rm.GetString("menuDocManipulation", culture);
                menuCustomizedReports.Text = rm.GetString("menuCustomizedReports", culture);
                menuGraficalRep.Text = rm.GetString("menuGraficalRep", culture);
                menuPresence.Text = rm.GetString("menuPresence", culture);
                menuRepLoc.Text = rm.GetString("menuRepLoc", culture);
                menuRepWU.Text = rm.GetString("menuRepWU", culture);
                menuRepEmpl.Text = rm.GetString("menuRepEmpl", culture);
                menuPresenceEmpl.Text = rm.GetString("menuPresenceEmpl", culture);
                //menuTypeReport.Text = rm.GetString("menuTypeReport", culture);
                menuMonthlyTypeReport.Text = rm.GetString("menuMonthlyTypeReport", culture);
                menuTimeSchedule.Text = rm.GetString("menuTimeSchedule", culture);
                menuEmplReport.Text = rm.GetString("menuEmplReport", culture);
                menuWUReport.Text = rm.GetString("menuWUReport", culture);
                menuItemReportsByType.Text = rm.GetString("menuItemReportsByType", culture);
                menuEmployeeLocations.Text = rm.GetString("menuEmployeeLocations", culture);
                menuLiveView.Text = rm.GetString("menuLiveView", culture);
                menuVisits.Text = rm.GetString("menuVisits", culture);
                menuEmplPresenceRep.Text = rm.GetString("menuEmplPresenceRep", culture);
                menuWUStatisticalRep.Text = rm.GetString("menuWUStatisticalRep", culture);
                menuMittalRep.Text = rm.GetString("menuMittalRep", culture);
                menuPaymentRep.Text = rm.GetString("menuPaymentRep", culture);
                menuRetiredTags.Text = rm.GetString("menuRetiredTags", culture);
                menuMitallWU.Text = rm.GetString("menuRepWU", culture);
                menuMittalEmpl.Text = rm.GetString("menuRepEmpl", culture);
                menuMittalSch.Text = rm.GetString("menuTimeSchedule", culture);
                menuMittalEmplAn.Text = rm.GetString("menuEmplReport", culture);
                menuMittalEmployeeLocations.Text = rm.GetString("menuMittalEmployeeLocations", culture);
                menuItemSumPassesOnReader.Text = rm.GetString("menuItemSumPassesOnReader", culture);
                menuPIORep.Text = rm.GetString("menuPIORep", culture);
                menuPIOPaymentRep.Text = rm.GetString("menuPIOPaymentRep", culture);
                menuPIOWorkListsRep.Text = rm.GetString("menuPIOWorkListsRep", culture);
                menuPIOWorkingSaturdays.Text = rm.GetString("menuPIOWorkingSaturdays", culture);
                menuItemTrespass.Text = rm.GetString("menuItemTrespass", culture);
                menuItemMillennium.Text = rm.GetString("menuItemMillennium", culture);
                menuItemMillenniumEmpl.Text = rm.GetString("menuEmplReport", culture);
                menuItemMillenniumWU.Text = rm.GetString("menuWUReport", culture);
                menuItemMillenniumTypes.Text = rm.GetString("menuTypeReport", culture);
                menuItemRoutes.Text = rm.GetString("menuRoutesTag", culture);
                menuItemRoutesTerminal.Text = rm.GetString("menuRoutesTerminal", culture);
                menuItemATBFOD.Text = rm.GetString("menuATBFODRep", culture);
                menuItemATBFODPayment.Text = rm.GetString("menuATBFODPaymentRep", culture);
                //menuItemATBFODEmplPresence.Text = rm.GetString("menuATBFODPresenceRep", culture);
                menuMapsReports.Text = rm.GetString("menuMaps", culture);
                menuJUBMES.Text = rm.GetString("menuJUBMES", culture);
                menuJUBMESPayrolls.Text = rm.GetString("menuJUBMESPayrolls", culture);
                menuJEEP.Text = rm.GetString("menuJEEP", culture);
                menuJEEPWUReports.Text = rm.GetString("menuJEEPWUReports", culture);
                menuItemUNIPROM.Text = rm.GetString("menuItemUNIPROM", culture);
                menuItemUNIPROMIOPairsReport.Text = rm.GetString("menuItemUNIPROMIOPairsReport", culture);
                menuItemUNIPROMDailyPreviewReport.Text = rm.GetString("menuItemUNIPROMDailyPreviewReport", culture);
                menuItemZIN.Text = rm.GetString("menuItemZIN", culture);
                menuItemZINPassesPreview.Text = rm.GetString("menuItemZINPassesPreview", culture);
                menuItemExtraHours.Text = rm.GetString("menuItemExtraHours", culture);
                menuItemEmplCategories.Text = rm.GetString("menuItemEmplCategories", culture);
                menuItemEUNET.Text = rm.GetString("menuItemEUNET", culture);
                menuItemEUNETPresenceReport.Text = rm.GetString("menuItemEUNETPresenceReport", culture);
                menuItemMinistry.Text = rm.GetString("menuItemMinistry", culture);
                menuMinistryEmplPresence.Text = rm.GetString("menuMinistryEmplPresence", culture);
                menuItemGSK.Text = rm.GetString("menuItemGSK", culture);
                menuItemGSKPresenceTracking.Text = rm.GetString("menuItemGSKPresenceTracking", culture);
                menuItemStatisticReport.Text = rm.GetString("menuItemStatisticReport", culture);
                menuItemNiksic.Text = rm.GetString("menuItemNiksic", culture);
                menuItemMonthlyReport.Text = rm.GetString("menuItemMonthlyReport", culture);
                menuItemSinvozCustomizedReports.Text = rm.GetString("menuItemSinvozCustomizedReports", culture);
                menuItemSinvozReportsByPassTypes.Text = rm.GetString("menuItemSinvozReportsByPassTypes", culture);
                menuItemVlatacom.Text = rm.GetString("menuItemVlatacom", culture);
                menuItemVlatacomWholeDayAbsenceAnnualReport.Text = rm.GetString("menuItemVlatacomWholeDayAbsenceAnnualReport", culture);
                menuItemFiat.Text = rm.GetString("menuItemFiat", culture);
                menuItemFiatDecisions.Text = rm.GetString("menuItemFiatDecisions", culture);
                menuItemPYIntegration.Text = rm.GetString("menuItemPYIntegration", culture);
                //menuItemFiat.Text = rm.GetString("menuItemFiat", culture);
                menuDSFReports.Text = rm.GetString("menuDSFReports", culture);
                menuDSFPresenceReport.Text = rm.GetString("menuDSFPresenceReport", culture);
                menuItemLames.Text = rm.GetString("menuItemLames", culture);
                menuItemLamesCumulativeReport.Text = rm.GetString("menuItemLamesCumulativeReport", culture);
                meniItemWN.Text = rm.GetString("menuItemWakerNeuson", culture);
                meniItemWNDailyPresence.Text = rm.GetString("menuItemDailyPresenceWN", culture);
                menuItemWNMonthly.Text = rm.GetString("menuItemWNMonthly", culture);
                menuConfezioniAndrea.Text = rm.GetString("menuConfezioniAndrea", culture);
                menuConfezioniAndreaMonthlyReport.Text = rm.GetString("menuConfezioniAndreaMonthlyReport", culture);
                menuItemPMC.Text = rm.GetString("menuItemPMC", culture);
                menuItemPMCMonthlyReport.Text = rm.GetString("menuItemPMCMonthlyReport", culture);
                menuItemPMCCumulativeReports.Text = rm.GetString("menuItemPMCCumulativeReports", culture);
                menuItemPMCStatisticalReports.Text = rm.GetString("menuItemPMCStatisticalReports", culture);
                menuItemPMCPaymentReport.Text = rm.GetString("menuItemPMCPaymentReport", culture);
                menuItemEmplCounterBalances.Text = rm.GetString("menuItemEmplCounterBalances", culture);
                menuItemGeox.Text = rm.GetString("menuItemGeox", culture);
                menuItemGeoxPYReport.Text = rm.GetString("menuItemGeoxPYReport", culture);
                menuItemGrundfos.Text = rm.GetString("menuItemGrundfos", culture);
                menuItemGrundfosTransportData.Text = rm.GetString("menuItemGrundfosTransportData", culture);
                menuItemGrundfosPYReport.Text = rm.GetString("menuItemGrundfosPYReport", culture);
                menuItemHyatt.Text = rm.GetString("menuItemHyatt", culture);
                menuItemHyattPYReport.Text = rm.GetString("menuItemHyattPYReports", culture);
                menuItemHyattTimeAndAttendance.Text = rm.GetString("menuItemTimeAndAttendence", culture);
                menuItemHyattOperatersCategoriesReport.Text = rm.GetString("menuItemHyattOperatersCategoriesReport", culture);
                menuItemHyattOperatersRolesReport.Text = rm.GetString("menuItemHyattOperatersRolesReport", culture);
                menuItemHyattOperatersOUWUReport.Text = rm.GetString("menuItemHyattOperatersOUWUReport", culture);
                menuItemHyattCategoriesPrivilegesReport.Text = rm.GetString("menuItemHyattCategoriesPrivilegesReport", culture);
                menuItemHyattRolePrivilegesReport.Text = rm.GetString("menuItemHyattRolePrivilegesReport", culture);
                menuItemHyattRolesPrivilegiesReportHRSSC.Text = rm.GetString("menuItemHyattRolesPrivilegiesReportHRSSC", culture);
                menuItemHyattRolesPrivilegiesReportPY.Text = rm.GetString("menuItemHyattRolesPrivilegiesReportPY", culture);
                menuItemHyattRolesPrivilegiesReportCANTEEN.Text = rm.GetString("menuItemHyattRolesPrivilegiesReportCANTEEN", culture);
                menuItemHyattRolesPrivilegiesReportHRSSCEXTENDED.Text = rm.GetString("menuItemHyattRolesPrivilegiesReportHRSSCEXTENDED", culture);
                menuItemHyattRolesPrivilegiesReportMedicalCheck.Text = rm.GetString("menuItemHyattRolesPrivilegiesReportMedicalCheck", culture);
                menuItemHyattRolesPrivilegiesReportSystemClose.Text = rm.GetString("menuItemHyattRolesPrivilegiesReportSystemClose", culture);
                menuItemEmployeesPersonalHoliday.Text = rm.GetString("menuItemEmployeesPersonalHoliday", culture);
                menuItemHyattPYGIDReport.Text = rm.GetString("menuItemHyattPYGIDReport", culture);
                menuItemAnualLeaveReport.Text = rm.GetString("menuItemAnnualLeaveReport", culture);
                menuItemStartWorkingDate.Text = rm.GetString("menuItemStartWorkingDate", culture);
                menuItemMagna.Text = rm.GetString("menuItemMagna", culture);
                menuItemEmployeePT.Text = rm.GetString("menuItemEmployeePT", culture);
                menuReportsForSalaryForecast.Text = rm.GetString("menuReportsForSalaryForecast", culture);
                menuReportsForOpenPairsByEmployee.Text = rm.GetString("menuReportsForOpenPairsByEmployee", culture);   //  24.05.2019. BOJAN
                menuVacationReport.Text = rm.GetString("menuVacationReport", culture);  //  10.06.2019. BOJAN
                
                // menu Interventions text
                menuInterventions.Text = rm.GetString("menuInterventions", culture);
                menuIOPairs.Text = rm.GetString("menuIOPairs", culture);
                menuPasses.Text = rm.GetString("menuPasses", culture);
                menuEmployeeAbsences.Text = rm.GetString("menuEmployeeAbsences", culture);
                menuExitPermissions.Text = rm.GetString("menuExitPermissions", culture);
                menuExtraHours.Text = rm.GetString("menuExtraHours", culture);
                menuItemVacationEvidence.Text = rm.GetString("menuItemVacationEvidence", culture);
                menuItemEnterDataByEmployee.Text = rm.GetString("menuItemEnterDataByEmployee", culture);
                menuItemMassiveInput.Text = rm.GetString("menuItemMassiveInput", culture);

                // menu Maintaining text
                menuMaintaining.Text = rm.GetString("menuMaintaining", culture);
                menuLibExport.Text = rm.GetString("menuLibExport", culture);
                menuImportData.Text = rm.GetString("menuImportData", culture);
                menuProcessingTables.Text = rm.GetString("menuProcessingTables", culture);
                menuLogToPasses.Text = rm.GetString("menuLogToPasses", culture);
                menuPassesToIOPairs.Text = rm.GetString("menuPassesToIOPairs", culture);
                menuImportLog.Text = rm.GetString("menuImportLog", culture);
                menuReadersSettings.Text = rm.GetString("menuReadersSettings", culture);
                menuPicManagament.Text = rm.GetString("menuPicManagament", culture);
                menuEmplPhotoManagament.Text = rm.GetString("menuEmplPhotoManagament", culture);
                menuCameraSnapshostManagament.Text = rm.GetString("menuCameraSnapshostManagament", culture);
                menuItemVisitors.Text = rm.GetString("menuVisitors", culture);
                menuLog2XML.Text = "Log -> XML";
                menuItemUNIPROMRAMP.Text = rm.GetString("menuItemUNIPROMRAMP", culture);
                //menuItemLocking.Text = rm.GetString("menuLocking", culture);
                menuItemTagsPrview.Text = rm.GetString("menuItemTagsPrview", culture);
                menuItemLastTerminalReading.Text = rm.GetString("menuItemLastTerminalReading", culture);
                menuItemRestartCounters.Text = rm.GetString("menuItemRestartCounters", culture);
                menuItemSynchronization.Text = rm.GetString("menuItemSynchronization", culture);

                // Menu Configuration
                menuConfiguration.Text = rm.GetString("menuConfiguration", culture);
                menuDataAccess.Text = rm.GetString("menuDataAccess", culture);
                menuEmplAccess.Text = rm.GetString("menuEmplAccess", culture);
                menuUserRoles.Text = rm.GetString("menuUserRoles", culture);
                menuUsers.Text = rm.GetString("menuUsers", culture);
                menuRoles.Text = rm.GetString("menuRoles", culture);
                menuRoleMaintainence.Text = rm.GetString("menuRoleMaintainence", culture);
                menuRolePrivileges.Text = rm.GetString("menuRolePrivileges", culture);
                menuUsersRoles.Text = rm.GetString("menuUsersRoles", culture);
                menuAccessControl.Text = rm.GetString("menuAccessControl", culture);
                menuAccessControlGroups.Text = rm.GetString("menuAccessControlGroups", culture);
                menuEmployeeAccessControlGroups.Text = rm.GetString("menuEmployeeAccessControlGroups", culture);
                menuAccessControlGroupsGates.Text = rm.GetString("menuAccessControlGroupsGates", culture);
                menuAccessProfiles.Text = rm.GetString("menuAccessProfiles", culture);
                menuTimeAccessProfiles.Text = rm.GetString("menuTimeAccessProfiles", culture);
                menuGateAccessProfiles.Text = rm.GetString("menuGateAccessProfiles", culture);
                menuAssignGateProfile.Text = rm.GetString("menuAssignGateProfile", culture);
                menuApplyAccessControlParameters.Text = rm.GetString("menuApplyAccessControlParameters", culture);
                menuAccessControlStatus.Text = rm.GetString("menuAccessControlStatus", culture);
                menuEmplResponisibility.Text = rm.GetString("menuEmplResponisibility", culture);
                menuRules.Text = rm.GetString("menuRules", culture);
                menuCutOffDate.Text = rm.GetString("menuCutOffDate", culture);
                menuItemSystem.Text = rm.GetString("menuItemSystem", culture);
                menuItemSystemClosingEvents.Text = rm.GetString("menuItemSystemClosingEvents", culture);
                menuItemSystemMessages.Text = rm.GetString("menuItemSystemMessages", culture);
                menuItemCounterType.Text = rm.GetString("menuItemCouterTypes", culture);
                menuItemUsersCategory.Text = rm.GetString("menuItemUsersCategory", culture);
                menuItemULChangesTbl.Text = rm.GetString("menuItemUserLoginChangesTbl", culture);
                

                // menu Monitor text
                menuItemMonitor.Text = rm.GetString("menuMonitor", culture);

                // menu Visitors text
                menuVisitors.Text = rm.GetString("menuVisitors", culture);
                menuVisitorsEnter.Text = rm.GetString("menuVisitorsEnter", culture);
                menuVisitorsExit.Text = rm.GetString("menuVisitorsExit", culture);
                menuVisitorsReport.Text = rm.GetString("menuVisitorsReport", culture);

                // menu Restaurant text
                menuRestaurant.Text = rm.GetString("menuRestaurant", culture);

                //menu Map text
                menuMapObjectMaintenance.Text = rm.GetString("menuMapObjectMaintenance", culture);
                menuMapMaintenance.Text = rm.GetString("menuMapMaintenance", culture);

                // menu People counter text
                menuStandardReport.Text = rm.GetString("menuStandardReport", culture);
                menuAdvancedReport.Text = rm.GetString("menuAdvancedReport1", culture);
                menuBasicReport.Text = rm.GetString("menuBasicReport", culture);

                //menu siemens compatibility
                menuMappingSiPass.Text = rm.GetString("menuItemMapping", culture);
                menuConnectionSetup.Text = rm.GetString("menuConnectionSetup", culture);

                // menu Help text
                menuHelp.Text = rm.GetString("menuHelp", culture);
                menuAbout.Text = rm.GetString("menuAbout", culture);
                menuManual.Text = rm.GetString("menuManual", culture);
                menuInformations.Text = rm.GetString("menuInformations", culture);                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void menuReaders_Click(object sender, System.EventArgs e)
        {
            try
            {
                Readers readers = new Readers();

                NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuReaders.Text);

                readers.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuReaders_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void ACTA_Load(object sender, System.EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.ShowActivityInSplashScreen("Establishing database connection...");

            // Test database connection. If connection can't be established, 
            // show message and abort applcation running.
            try
            {
                if (Controller.TestDataBaseConnection())
                {
                    DialogResult result = MessageBox.Show(rm.GetString("DBConnNotEstablished", culture), "", MessageBoxButtons.OK);
                    if (result == DialogResult.OK)
                    {
                        //do not show another message boxes, one is enough
                        showMessageBox = false;

                        this.Close();
                        return;
                    }
                }

                splash.Close();

                LogInForm logIn = new LogInForm();
                if (userID.Equals("") || password.Equals(""))
                {
                    logIn.ShowDialog();
                }
                else
                {
                    logIn.btnOKClick(userID, password);
                }

                if (this.CurrentUser == null)
                {
                    this.Close();
                }
                else if (this.CurrentUser.UserID.Trim().Equals(""))
                {
                    this.Close();
                }
                else
                {
                    /*
                    NotificationController.SetApplicationName(this.Text);
					
                    // Debug
                    string logFilePath = Constants.logFilePath + this.Text + "Log.txt";
                    log = new DebugLog(logFilePath);
                    */

                    NotificationController.SetLogInUser(this.CurrentUser);
                    NotificationController.SetLastLocking();
                    culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                    // test licence
                    int licenceValidity = Controller.testLicence();

                    // if licence does not exist or it is not valid
                    if (licenceValidity >= 0)
                    {
                        if (licenceValidity == Constants.functionalityNotValid)
                        {
                            log.writeLog(DateTime.Now + "ACTA.ACTA_Load() : " + rm.GetString("functionalityNotValid", culture) + "\n");
                            MessageBox.Show(rm.GetString("functionalityNotValid", culture));
                        }
                        else
                        {
                            // if log in user has ADMIN role, he has permisson to generate licence request and send it
                            if (Common.Misc.isADMINRole(this.CurrentUser.UserID))
                            {
                                string serverName = Controller.getDBServerName();
                                string port = Controller.getDBServerPort();

                                switch (licenceValidity)
                                {
                                    case Constants.noLicence:
                                        {
                                            Common.Misc.generateLicenceRequest(serverName, port);
                                            MessageBox.Show(rm.GetString("noLicenceADMIN1", culture) + "\n"
                                                + rm.GetString("noLicenceADMIN2", culture) + "\n"
                                                + Application.StartupPath + "\\licreq.txt" + "\n" + rm.GetString("licenceADMIN", culture));

                                            break;
                                        }
                                    case Constants.invalidLicence:
                                        {
                                            Common.Misc.generateLicenceRequest(serverName, port);
                                            MessageBox.Show(rm.GetString("invalidLicenceADMIN1", culture) + "\n"
                                                + rm.GetString("invalidLicenceADMIN2", culture) + "\n"
                                                + Application.StartupPath + "\\licreq.txt" + "\n" + rm.GetString("licenceADMIN", culture));

                                            this.Close();
                                            return;
                                        }
                                    case Constants.maxConnNumExceeeded:
                                        {
                                            DialogResult result = MessageBox.Show(rm.GetString("maxConnNumExceeededADMIN1", culture)
                                                + "\n" + rm.GetString("closeSessions", culture), "", MessageBoxButtons.YesNo);
                                            if (result == DialogResult.No)
                                            {
                                                Common.Misc.generateLicenceRequest(serverName, port);
                                                MessageBox.Show(rm.GetString("maxConnNumExceeededADMIN1", culture) + "\n"
                                                    + rm.GetString("maxConnNumExceeededADMIN2", culture) + "\n"
                                                    + Application.StartupPath + "\\licreq.txt" + "\n" + rm.GetString("licenceADMIN", culture));
                                            }
                                            else
                                            {
                                                List<ApplUserLogTO> usersLog = new ApplUserLog().SearchOpenSessions("", "", Constants.UserLoginChanel.DESKTOP.ToString().Trim());

                                                bool isUpdated = true;
                                                ApplUserLog logUser = new ApplUserLog();
                                                foreach (ApplUserLogTO userLog in usersLog)
                                                {
                                                    logUser.UserLogTO = userLog;
                                                    isUpdated = (logUser.Update(Constants.autoUser, "") >= 0 ? true : false) && isUpdated;

                                                    if (!isUpdated)
                                                        break;
                                                }

                                                if (isUpdated)
                                                {
                                                    MessageBox.Show(rm.GetString("sessionsClosed", culture));
                                                }
                                                else
                                                {
                                                    MessageBox.Show(rm.GetString("sessionsNotClosed", culture));
                                                }
                                            }

                                            this.Close();
                                            return;
                                        }
                                    case Constants.menuItemsNotUpdated:
                                        {
                                            MessageBox.Show(rm.GetString("menuItemsNotUpdated", culture));
                                            this.Close();
                                            return;
                                        }
                                }
                            }
                            else
                            {
                                switch (licenceValidity)
                                {
                                    case Constants.noLicence:
                                        {
                                            MessageBox.Show(rm.GetString("noLicence", culture));
                                            break;
                                        }
                                    case Constants.invalidLicence:
                                        {
                                            MessageBox.Show(rm.GetString("invalidLicence", culture));
                                            this.Close();
                                            return;
                                        }
                                    case Constants.maxConnNumExceeeded:
                                        {
                                            MessageBox.Show(rm.GetString("maxConnNumExceeeded", culture));
                                            this.Close();
                                            return;
                                        }
                                    case Constants.menuItemsNotUpdated:
                                        {
                                            MessageBox.Show(rm.GetString("menuItemsNotUpdated", culture));
                                            this.Close();
                                            return;
                                        }
                                }
                            }
                        }
                    }

                    ApplUserLog userlog = new ApplUserLog();
                    userlog.UserLogTO.UserID = this.CurrentUser.UserID;
                    userlog.UserLogTO.LoginChanel = Constants.UserLoginChanel.DESKTOP.ToString();
                    userlog.UserLogTO.LoginStatus = Constants.UserLoginStatus.SUCCESSFUL.ToString();
                    userlog.UserLogTO.LoginType = Constants.UserLoginType.TM.ToString();
                    userlog.UserLogTO = userlog.Insert();

                    NotificationController.SetLog(userlog.UserLogTO);

                    log.createLog();

                    if (NotificationController.GetChangePassword())
                    {
                        ChangePassword changePasForm = new ChangePassword();
                        changePasForm.ShowDialog();
                    }

                    // Find all Roles or CurrentUser
                    currentRoles = new ApplUsersXRole().FindRolesForUser(this.CurrentUser.UserID);
                    NotificationController.SetCurrentRoles(currentRoles);

                    // Get all Menu Items
                    List<ApplMenuItemTO> menuItems = new ApplMenuItem().Search();

                    // Go through all Menu Items, if User has role with read permission, Menu Item is visible
                    foreach (ApplMenuItemTO item in menuItems)
                    {
                        itemPermissions = item.PermissionsToArray();

                        // set current user permission not to be greater than SYS permission for particular item
                        for (int i = 1; i < itemPermissions.Length; i++)
                        {
                            if (itemPermissions[i] > itemPermissions[0]) itemPermissions[i] = itemPermissions[0];
                        }

                        menuItemsPermissions.Add(item.ApplMenuItemID, itemPermissions);
                    }

                    NotificationController.SetMenuItemsPermissions(menuItemsPermissions);
                    setLanguage();

                    setMenuItemsVisibility();

                    modulesList = Common.Misc.getLicenceModuls(null);
                    if (menuItemUNIPROMRAMP.Visible && menuMaintaining.Visible)
                    {
                        menuItemUNIPROMRAMP.PerformClick();
                    }

                    if (menuItemMassiveInput.Visible)
                    {
                        string costumer = Common.Misc.getCustomer(null);
                        int cost = 0;
                        if (int.TryParse(costumer, out cost) && (cost != (int)Constants.Customers.FIAT))
                            menuItemMassiveInput.Visible = false;
                    }

                    if (menuItemSynchronization.Visible)
                    {
                        string costumer = Common.Misc.getCustomer(null);
                        int cost = 0;
                        if (int.TryParse(costumer, out cost) && (cost != (int)Constants.Customers.FIAT))
                            menuItemSynchronization.Visible = false;
                    }

                    if (menuCustomizedReports.Visible)
                    {
                        string costumer = Common.Misc.getCustomer(null);
                        int cost = 0;
                        if (int.TryParse(costumer, out cost) && (cost == (int)Constants.Customers.AAC))
                            menuCustomizedReports.Visible = false;
                    }

                    // set client logo
                    if (File.Exists(Constants.LogoPath))
                    {
                        pbClientLogo.Visible = true;
                        pbClientLogo.Image = new Bitmap(Constants.LogoPath);
                        pbClientLogo.Height = pbClientLogo.Image.Height;
                        pbClientLogo.Width = pbClientLogo.Image.Width;
                        int x = this.ClientSize.Width - pbClientLogo.Width - pbLogo.Location.X;
                        int y = ACTAStatusBar.Location.Y - pbClientLogo.Height -
                            (ACTAStatusBar.Location.Y - pbLogo.Location.Y - pbLogo.Height);
                        pbClientLogo.Location = new Point(x, y);
                    }
                    else
                    {
                        pbClientLogo.Visible = false;
                    }

                    // Status bar
                    ACTAStatusBar.ShowPanels = false;
                    sbPanelDate.Alignment = HorizontalAlignment.Left;
                    sbPanelDate.Text = DateTime.Now.ToString("dd.MM.yyy.");
                    sbPanelUser.Alignment = HorizontalAlignment.Left;
                    sbPanelUser.Text = CurrentUser.UserID.Trim() + " [" + CurrentUser.Name.Trim() + "]";
                    sbPanelLogInTime.Alignment = HorizontalAlignment.Left;
                    sbPanelLogInTime.Text = rm.GetString("LogInTime", culture) + " " + DateTime.Now.ToString("HH:mm");
                    sbPanelDB.Alignment = HorizontalAlignment.Right;
                    sbPanelDB.Text = rm.GetString("lblServer", culture) + " " + Controller.getDBServerName().Trim() + " " + rm.GetString("lblDatabase", culture) + " " + Controller.getDBName().Trim();                    
                    ACTAStatusBar.ShowPanels = true;
                    this.Cursor = Cursors.Arrow;
                    menuItemVacationEvidence.Visible = false;
                    menuDocManipulation.Visible = false;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + "ACTA.ACTA_Load() : " + ex.StackTrace + "\n");
                this.Close();
                MessageBox.Show(ex.Message);
            }

            // Start automtic reader logs processing
            /*
            DialogResult proc = MessageBox.Show(rm.GetString("StartProcLog", culture), "", MessageBoxButtons.YesNo);
            if(proc == DialogResult.Yes)
            {
                DownloadManager logManager = DownloadManager.GetInstance();
                logManager.StartLogProcessing();

                if (logManager.StartLogProcessing())
                {
                    MessageBox.Show(rm.GetString("LogProcStarted", culture));
                }

            }
            */

        }

        private void ShowActivityInSplashScreen(string activity)
        {
            if (this.splash != null)
            {
                this.splash.ActivityText = activity;
            }
        }

        private void menuRepWU_Click(object sender, System.EventArgs e)
        {
            try
            {
                WorkingUnitsReports wuReports = new WorkingUnitsReports();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuStandardRep.Text
                + "_" + menuPresence.Text + "_" + menuRepWU.Text);
                wuReports.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuRepWU_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuRepLoc_Click(object sender, System.EventArgs e)
        {
            try
            {
                LocationsReports locReports = new LocationsReports();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuStandardRep.Text
                + "_" + menuPresence.Text + "_" + menuRepLoc.Text);
                locReports.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuRepLoc_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuRepEmpl_Click(object sender, System.EventArgs e)
        {
            try
            {
                EmployeesReports emplReports = new EmployeesReports();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuStandardRep.Text
                + "_" + menuPresence.Text + "_" + menuRepEmpl.Text);
                emplReports.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuRepEmpl_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuWTSchema_Click(object sender, System.EventArgs e)
        {
            try
            {
                WTSchema wtsch = new WTSchema();

                NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuWT.Text + "_" + menuWTSchema.Text);

                wtsch.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuWTSchema_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuWTGroups_Click(object sender, System.EventArgs e)
        {
            try
            {
                WTGroups wtGroup = new WTGroups();

                NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuWT.Text + "_" + menuWTGroups.Text);

                wtGroup.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuWTGroups_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuPresence_Click(object sender, System.EventArgs e)
        {
            try
            {
                PresenceTracking presenceReports = new PresenceTracking();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuStandardRep.Text
                + "_" + menuPresence.Text);
                presenceReports.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuPresence_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuExit_Click(object sender, System.EventArgs e)
        {

        }

        private void menuLibExport_Click(object sender, System.EventArgs e)
        {
            LibrariesExportImport exportLib = new LibrariesExportImport(0);
            //NotificationController.SetCurrentMenuItemID(menuMaintaining.Text + "_" + menuLibExport.Text);
            exportLib.ShowDialog(this);
        }

        private void menuImportData_Click(object sender, System.EventArgs e)
        {
            try
            {
                LibrariesExportImport importLib = new LibrariesExportImport(1);
                //NotificationController.SetCurrentMenuItemID(menuMaintaining.Text + "_" + menuImportData.Text);
                importLib.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuLibImport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuLogToPasses_Click(object sender, System.EventArgs e)
        {
            try
            {
                Pass pass = new Pass();
                Employee employee = new Employee();
                employee.EmplTO.Type = Constants.emplSpecial;
                Dictionary<int, EmployeeTO> specialEmpl = employee.SearchDictionary();

                employee.EmplTO.Type = Constants.emplSpecial;
                Dictionary<int, EmployeeTO> extraOrdEmpl = employee.SearchDictionary();
                //NotificationController.SetCurrentMenuItemID(menuMaintaining.Text + "_" + menuProcessingTables.Text
                //+ "_" + menuLogToPasses.Text);
                pass.PopulatePasses(specialEmpl, extraOrdEmpl);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuLogToPasses_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuPassesToIOPairs_Click(object sender, System.EventArgs e)
        {
            try
            {
                IOPair ioPair = new IOPair();

                Employee employee = new Employee();
                employee.EmplTO.Type = Constants.emplSpecial;
                Dictionary<int, EmployeeTO> specialEmpl = employee.SearchDictionary();

                employee.EmplTO.Type = Constants.emplSpecial;
                Dictionary<int, EmployeeTO> extraOrdEmpl = employee.SearchDictionary();

                //get all time Schemas
                Dictionary<int, WorkTimeSchemaTO> dictTimeSchema = new TimeSchema().getDictionary();
                //NotificationController.SetCurrentMenuItemID(menuMaintaining.Text + "_" + menuProcessingTables.Text
                //+ "_" + menuPassesToIOPairs.Text);
                ioPair.ClassifyPasses(specialEmpl, extraOrdEmpl, null, dictTimeSchema);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuPassesToIOPairs_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuImportLog_Click(object sender, System.EventArgs e)
        {
            // Start automtic reader logs processing
            //DownloadManager logManager = DownloadManager.GetInstance();
            DataProcessingManager procManager = DataProcessingManager.GetInstance();

            if (!procManager.IsProcessing)
            {
                DialogResult proc = MessageBox.Show(rm.GetString("StartProcLog", culture), "", MessageBoxButtons.YesNo);
                if (proc == DialogResult.Yes)
                {
                    if (procManager.chekPrerequests())
                    {
                        procManager.StartLogProcessing();
                    }
                    else
                    {
                        DialogResult procreq = MessageBox.Show(rm.GetString("SettingsFailed", culture), "", MessageBoxButtons.OK);
                        if (procreq == DialogResult.OK)
                        {
                            return;
                        }
                    }
                    /*
                    if (logManager.StartLogProcessing())
                    {
                        MessageBox.Show(rm.GetString("LogProcStarted", culture));
                    }
                    */
                }
            }
            else
            {
                DialogResult proc = MessageBox.Show(rm.GetString("StopProcLog", culture), "", MessageBoxButtons.YesNo);
                if (proc == DialogResult.Yes)
                {
                    if (procManager.StopLogProcessing())
                    {
                        MessageBox.Show(rm.GetString("LogProcStopped", culture));
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("LogProcCantStop", culture));
                    }
                }
            }

        }

        private void menuIOPairs_Click(object sender, System.EventArgs e)
        {
            try
            {
                IOPairs IOPairsForm = new IOPairs();

                NotificationController.SetCurrentMenuItemID(menuInterventions.Text + "_" + menuIOPairs.Text);

                IOPairsForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItem1_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuPasses_Click(object sender, System.EventArgs e)
        {
            try
            {
                Passes passes = new Passes();

                NotificationController.SetCurrentMenuItemID(menuInterventions.Text + "_" + menuPasses.Text);

                passes.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuPasses_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuEmployeeAbsences_Click(object sender, System.EventArgs e)
        {
            try
            {
                UI.EmployeeAbsences emplAbsences = new UI.EmployeeAbsences();

                NotificationController.SetCurrentMenuItemID(menuInterventions.Text + "_" + menuEmployeeAbsences.Text);

                emplAbsences.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuEmployeeAbsences_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuAbout_Click(object sender, System.EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

        private void menuGates_Click(object sender, System.EventArgs e)
        {
            try
            {
                Gates gates = new Gates();

                NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuGates.Text);

                gates.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuGates_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuReportEmplAccsess_Click(object sender, System.EventArgs e)
        {
            try
            {
                ApplUserXWU userXWU = new ApplUserXWU();
                //NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuDataAccess.Text
                //+ "_" + menuEmplAccess.Text);
                userXWU.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuGates_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuUsers_Click(object sender, System.EventArgs e)
        {
            try
            {
                ApplUsers users = new ApplUsers();

                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuUserRoles.Text + "_" + menuUsers.Text);

                users.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuUsers_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuUsersRoles_Click(object sender, System.EventArgs e)
        {
            try
            {
                ApplUsersXRoles userXRoleForm = new ApplUsersXRoles();
                //NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuUserRoles.Text + "_" + menuUsersRoles.Text);
                userXRoleForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuUsersRoles_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuRoleMaintainence_Click(object sender, System.EventArgs e)
        {
            try
            {
                ApplRoles roles = new ApplRoles();

                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuUserRoles.Text + "_"
                    + menuRoles.Text + "_" + menuRoleMaintainence.Text);

                roles.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuRoles_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuRolePrivileges_Click(object sender, System.EventArgs e)
        {
            try
            {
                List<ApplRoleTO> roleList = new ApplRole().SearchUserCreatedRoles();
                ApplMenuItem mItem = new ApplMenuItem();
                mItem.MenuItemTO.LangCode = NotificationController.GetLanguage();
                List<ApplMenuItemTO> menuItemList = mItem.Search();

                RolesPermissions rolePermForm = new RolesPermissions(roleList, menuItemList);
                //NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuUserRoles.Text + "_"
                //	+ menuRoles.Text + "_" + menuRolePrivileges.Text);
                rolePermForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuRolePrivileges_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setCurrentUser(object sender, NotificationEventArgs args)
        {
            try
            {
                if (args.logInUser != null)
                {
                    this.CurrentUser = new ApplUserTO(args.logInUser.UserID, args.logInUser.Password, args.logInUser.Name,
                        args.logInUser.Description, args.logInUser.PrivilegeLvl, args.logInUser.Status,
                        args.logInUser.NumOfTries, args.logInUser.LangCode);
                    this.CurrentUser.ExitPermVerification = args.logInUser.ExitPermVerification;
                    this.CurrentUser.ExtraHoursAdvancedAmt = args.logInUser.ExtraHoursAdvancedAmt;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.setCurrentUser(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void ACTA_Closed(object sender, System.EventArgs e)
        {
            int logOut = 0;

            try
            {
                ApplUserLog ulog = new ApplUserLog();

                ulog.UserLogTO = NotificationController.GetLog();
                logOut = ulog.Update();

                ACTAMonitorLib.Monitor.Instance.StopMonitoring();

                Controller.DettachFromNotifier(this.observerClient);
                if (CurrentUser != null)
                {
                    CurrentUser = new ApplUserTO();
                }

                DataProcessingManager dataProcManager = DataProcessingManager.GetInstance();

                if (dataProcManager.IsProcessing)
                {
                    if (dataProcManager.StopLogProcessing())
                    {
                        MessageBox.Show(rm.GetString("LogProcStopped", culture));
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("LogProcCantStop", culture));
                    }
                }

                if (liveViewThread != null)
                {
                    liveViewThread.Abort();
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.ACTA_Closed(): " + ex.Message + "\n");

                if (showMessageBox)
                    MessageBox.Show(ex.Message);
            }
        }

        private bool checkVisibility(string MenuItemID)
        {
            bool visible = false;

            try
            {
                foreach (ApplRoleTO role in currentRoles)
                {
                    int readPermission = (((int[])menuItemsPermissions[MenuItemID])[role.ApplRoleID] / 8) % 2;
                    if (Common.Misc.getLicenceModuls(null).Contains((int)Constants.Moduls.SiemensCompatibility))
                    {
                        //Those menu items are visible only for ADMIN role
                        if (Common.Misc.isADMINRole(CurrentUser.UserID) &&
                            (MenuItemID == (menuHelp.Text + "_" + menuInformations.Text) || MenuItemID == (menuLibraries.Text + "_" + menuWT.Text + "_" + menuProlongTS.Text)))
                            return true;
                    }
                    else
                    {
                        //Those menu items are visible only for ADMIN role
                        if (Common.Misc.isADMINRole(CurrentUser.UserID) &&
                            (MenuItemID == (menuHelp.Text + "_" + menuInformations.Text) || MenuItemID == (menuLibraries.Text + "_" + menuWT.Text + "_" + menuProlongTS.Text)
                            || MenuItemID == (menuMaintaining.Text + "_" + menuLog2XML.Text)))
                            return true;
                    }

                    visible = visible || (readPermission == 0 ? false : true);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.checkVisibility(): " + ex.Message + MenuItemID + "\n");
                MessageBox.Show(ex.Message);
            }

            return visible;
        }

        private void setMenuItemsVisibility()
        {
            try
            {
                // Go through all Menu Items on the form
                foreach (MenuItem item in mainMenu1.MenuItems)
                {
                    item.Visible = checkVisibility(item.Text);
                    item.Enabled = item.Visible;

                    if (item.Visible)
                    {
                        // second level
                        foreach (MenuItem subItem in item.MenuItems)
                        {
                            if (!subItem.Text.Equals("-"))
                            {
                                subItem.Visible = checkVisibility(item.Text + "_" + subItem.Text);
                                subItem.Enabled = subItem.Visible;
                            }

                            if (subItem.Visible)
                            {
                                // third level
                                foreach (MenuItem subsubItem in subItem.MenuItems)
                                {
                                    if (!subsubItem.Text.Equals("-"))
                                    {
                                        subsubItem.Visible = checkVisibility(item.Text + "_" + subItem.Text
                                            + "_" + subsubItem.Text);
                                        subsubItem.Enabled = subsubItem.Visible;
                                    }

                                    if (subsubItem.Visible)
                                    {
                                        // fourth level
                                        foreach (MenuItem subsubsubItem in subsubItem.MenuItems)
                                        {
                                            if (!subsubsubItem.Text.Equals("-"))
                                            {
                                                subsubsubItem.Visible = checkVisibility(item.Text + "_" + subItem.Text
                                                    + "_" + subsubItem.Text + "_" + subsubsubItem.Text);
                                                subsubsubItem.Enabled = subsubsubItem.Visible;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.setMenuItemsVisibility(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuEmployeeLocations_Click(object sender, System.EventArgs e)
        {
            try
            {
                EmployeeLocations emplLocations = new EmployeeLocations();

                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuStandardRep.Text + "_" + menuEmployeeLocations.Text);

                emplLocations.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuEmployeeLocations_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuEmplReport_Click(object sender, System.EventArgs e)
        {
            try
            {
                EmpoyeeAnaliticReport emplReport = new EmpoyeeAnaliticReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuStandardRep.Text
                + "_" + menuTimeSchedule.Text + "_" + menuEmplReport.Text);
                emplReport.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuRoles_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuExitPermissions_Click(object sender, System.EventArgs e)
        {
            try
            {
                ExitPermissions exitPerm = new ExitPermissions();
                NotificationController.SetCurrentMenuItemID(menuInterventions.Text + "_"
                    + menuExitPermissions.Text);
                exitPerm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuExitPermissions_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuHolidays_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Holidays holiday = new Holidays();
                HolidaysExtendedPreview holiday = new HolidaysExtendedPreview();
                NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_"
                    + menuWT.Text + "_" + menuHolidays.Text);
                holiday.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuHolidays_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuReadersSettings_Click(object sender, System.EventArgs e)
        {
            try
            {
                // NotificationController.SetCurrentMenuItemID(menuMaintaining.Text + "_" + menuReadersSettings.Text);
                ReaderSetup rs = new ReaderSetup();
                rs.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuReadersSettings_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuManual_Click(object sender, System.EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Constants.HelpManualPath);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuManual_Click(): " + ex.Message + "\n");
                MessageBox.Show(rm.GetString("manualNotOpened", culture));
            }
        }

        private void menuPaymentRep_Click(object sender, System.EventArgs e)
        {
            try
            {
                PaymentReports paymentReport = new PaymentReports();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuMittalRep.Text
                + "_" + menuPaymentRep.Text);
                paymentReport.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuPaymentRep_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuRetiredTags_Click(object sender, System.EventArgs e)
        {
            try
            {
                TagsReports tagsReport = new TagsReports();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuMittalRep.Text
                + "_" + menuRetiredTags.Text);
                tagsReport.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuRetiredTags_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuProlongTS_Click(object sender, System.EventArgs e)
        {
            try
            {
                ProlongTS prolongForm = new ProlongTS();
                NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuWT.Text
                + "_" + menuProlongTS.Text);
                prolongForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuProlongTS_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuMitallWU_Click(object sender, System.EventArgs e)
        {
            try
            {
                Reports.Mittal.MittalWorkingUnitsReports wu = new Reports.Mittal.MittalWorkingUnitsReports();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuMittalRep.Text
                + "_" + menuMitallWU.Text);
                wu.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuMitallWU_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuMittalEmplAn_Click(object sender, System.EventArgs e)
        {
            try
            {
                Reports.Mittal.MittalEmpoyeeAnaliticReport analitic = new Reports.Mittal.MittalEmpoyeeAnaliticReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuMittalRep.Text
                + "_" + menuMittalSch.Text + "_" + menuMittalEmplAn.Text);
                analitic.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuMittalEmplAn_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuMittalEmpl_Click(object sender, System.EventArgs e)
        {
            try
            {
                Reports.Mittal.MittalEmployeesReports empl = new Reports.Mittal.MittalEmployeesReports();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuMittalRep.Text
                + "_" + menuMittalEmpl.Text);
                empl.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuMittalEmpl_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuAccessControlGroups_Click(object sender, System.EventArgs e)
        {
            try
            {
                EmployeeAccessGroups accessGroups = new EmployeeAccessGroups();
                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuAccessControl.Text + "_"
                    + menuAccessControlGroups.Text);
                accessGroups.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuAccessControlGroups_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuEmployeeAccessControlGroups_Click(object sender, System.EventArgs e)
        {
            try
            {
                EmployeesXAccessGroups employeesXAccessGroupsForm = new EmployeesXAccessGroups();
                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuAccessControl.Text + "_"
                    + menuEmployeeAccessControlGroups.Text);
                employeesXAccessGroupsForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuEmployeeAccessControlGroups_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuAccessControlGroupsGates_Click(object sender, System.EventArgs e)
        {
            try
            {
                AccessGroupsXGates accessGroupsXGatesForm = new AccessGroupsXGates();
                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuAccessControl.Text + "_"
                    + menuAccessControlGroupsGates.Text);
                accessGroupsXGatesForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuAccessControlGroupsGates_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuTimeAccessProfiles_Click(object sender, System.EventArgs e)
        {
            try
            {
                TimeAccessProfiles timeAccessProfiles = new TimeAccessProfiles();
                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuAccessControl.Text + "_"
                    + menuAccessProfiles.Text + "_" + menuTimeAccessProfiles.Text);
                timeAccessProfiles.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuTimeAccessProfiles_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuGateAccessProfiles_Click(object sender, System.EventArgs e)
        {
            try
            {
                GateTimeAccessProfiles gateTimeAccessProfiles = new GateTimeAccessProfiles();
                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuAccessControl.Text + "_"
                    + menuAccessProfiles.Text + "_" + menuGateAccessProfiles.Text);
                gateTimeAccessProfiles.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuGateAccessProfiles_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuAssignGateProfile_Click(object sender, System.EventArgs e)
        {
            try
            {
                GatesXGateTimeProfile gatesXGateTimeProfile = new GatesXGateTimeProfile();
                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuAccessControl.Text + "_"
                    + menuAccessProfiles.Text + "_" + menuAssignGateProfile.Text);
                gatesXGateTimeProfile.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuAssignGateProfile_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuApplyAccessControlParameters_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuAccessControl.Text + "_"
                    + menuApplyAccessControlParameters.Text);

                DataManager dataMgr = new DataManager();
                DialogResult proc = MessageBox.Show(rm.GetString("updateAccessGroupsNow", culture), "", MessageBoxButtons.YesNo);
                if (proc == DialogResult.Yes)
                {
                    dataMgr.FinalizeTimeaccessprofilesTracking(true);
                }
                if (proc == DialogResult.No)
                {
                    dataMgr.FinalizeTimeaccessprofilesTracking(false);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuApplyAccessControlParameters_Click(): " + ex.Message + "\n");
                MessageBox.Show(rm.GetString("notUpdateAccessGroupsNow", culture));
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void menuMittalEmployeeLocations_Click(object sender, System.EventArgs e)
        {
            try
            {
                Reports.Mittal.MittalEmployeeLocation mittalEmplLoc = new Reports.Mittal.MittalEmployeeLocation();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuMittalRep.Text + "_"
                    + menuMittalEmployeeLocations.Text);
                mittalEmplLoc.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuMittalEmployeeLocations_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuWUReport_Click(object sender, System.EventArgs e)
        {
            try
            {
                Reports.EmployeeAnalyticalWU WUEmployee = new Reports.EmployeeAnalyticalWU();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuStandardRep.Text + "_"
                    + menuTimeSchedule.Text + "_" + menuWUReport.Text);
                WUEmployee.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuWUReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }

      /*  private void menuTypeReport_Click(object sender, System.EventArgs e)
        {
            try
            {
                Reports.EmployeePresenceType PresenceType = new Reports.EmployeePresenceType();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuStandardRep.Text + "_"
                    + menuPresence.Text + "_" + menuTypeReport.Text);
                PresenceType.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuTypeReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }*/

        private void menuExtraHours_Click(object sender, System.EventArgs e)
        {
            try
            {
                ExtraHours extraHours = new ExtraHours();
                NotificationController.SetCurrentMenuItemID(menuInterventions.Text + "_" + menuExtraHours.Text);
                extraHours.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuExtraHours_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuPauses_Click(object sender, System.EventArgs e)
        {
            try
            {
                TimeSchemaPauses timeSchemaPauses = new TimeSchemaPauses();
                NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuWT.Text + "_" + menuPauses.Text);
                timeSchemaPauses.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuPauses_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuPIOPaymentRep_Click(object sender, System.EventArgs e)
        {
            try
            {
                Reports.PIO.PIOPaymentReports PIOPaymentReport = new Reports.PIO.PIOPaymentReports();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuPIORep.Text
                + "_" + menuPIOPaymentRep.Text);
                PIOPaymentReport.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuPIOPaymentRep_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuInformations_Click(object sender, System.EventArgs e)
        {
            try
            {
                Info info = new Info();
                NotificationController.SetCurrentMenuItemID(menuHelp.Text + "_" + menuInformations.Text);
                info.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuInformations_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuPIOWorkListsRep_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.PIO.WorkLists workListsReport = new Reports.PIO.WorkLists();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuPIORep.Text
                + "_" + menuPIOWorkListsRep.Text);
                workListsReport.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuPIOWorkListsRep_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuPIOWorkingSaturdays_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.PIO.PIOWorkingSaturdays pioWorkingSaturdays = new Reports.PIO.PIOWorkingSaturdays();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuPIORep.Text
                + "_" + menuPIOWorkingSaturdays.Text);
                pioWorkingSaturdays.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuPIOWorkingSaturdays_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuVisits_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.VisitorReports visitorReports = new Reports.VisitorReports();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuStandardRep.Text
                + "_" + menuVisits.Text);
                visitorReports.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuVisits_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuAccessControlStatus_Click(object sender, EventArgs e)
        {
            try
            {
                AccessControlStatus acs = new AccessControlStatus();
                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuAccessControl.Text + "_"
                    + menuAccessControlStatus.Text);
                acs.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuAccessControlStatus_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuCameras_Click(object sender, EventArgs e)
        {
            try
            {
                Cameras cameras = new Cameras();
                NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuCameras.Text);
                cameras.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuCameras_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuLiveView_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (liveViewThread != null)
                    {
                        liveViewThread.Abort();
                    }
                }
                catch (ThreadAbortException)
                {
                }

                //liveViewThread = new Thread(new ParameterizedThreadStart(ShowLiveView));
                liveViewThread = new Thread(new ThreadStart(ShowLiveView));
                liveViewThread.SetApartmentState(ApartmentState.STA);
                liveViewThread.Start();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuLiveView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        //private void ShowLiveView(object file)
        private void ShowLiveView()
        {
            try
            {
                Reports.Video_surveillance vs = new Video_surveillance();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuStandardRep + "_" + menuLiveView.Text);
                vs.ShowDialog();

                if (liveViewThread != null)
                {
                    liveViewThread.Abort();
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.ShowLiveView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemMonitor_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor cursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                NotificationController.SetCurrentMenuItemID(menuItemMonitor.Text);
                ACTAMonitorLib.Monitor.Instance.Show();
                this.Cursor = cursor;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemMonitor_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuVisitorsEnter_Click(object sender, EventArgs e)
        {
            try
            {
                Visitors visitorsEnter = new Visitors("Enter");
                NotificationController.SetCurrentMenuItemID(menuVisitors.Text + "_" + menuVisitorsEnter.Text);
                visitorsEnter.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuVisitorsEnter_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuVisitorsExit_Click(object sender, EventArgs e)
        {
            try
            {
                Visitors visitorsExit = new Visitors("Exit");
                NotificationController.SetCurrentMenuItemID(menuVisitors.Text + "_" + menuVisitorsExit.Text);
                visitorsExit.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuVisitorsExit_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuVisitorsReport_Click(object sender, EventArgs e)
        {
            try
            {
                VisitorsView visitorsView = new VisitorsView();
                NotificationController.SetCurrentMenuItemID(menuVisitors.Text + "_" + menuVisitorsReport.Text);
                visitorsView.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuVisitorsReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void mainMenu1_Collapse(object sender, EventArgs e)
        {
            try
            {
                ApplUserLogTO applUserLog = new ApplUserLog().FindMaxSession(NotificationController.GetLogInUser().UserID, System.Net.Dns.GetHostName(), Constants.UserLoginChanel.DESKTOP.ToString());

                if (!applUserLog.LogOutTime.Equals(new DateTime()))
                {
                    MessageBox.Show(rm.GetString("SessionClosed", culture));
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.mainMenu1_Collapse(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void ACTA_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.ACTA_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void menuEmplPresenceRep_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeePresenceGraphicReports employeePresenceGraphicReports = new EmployeePresenceGraphicReports();
                NotificationController.SetCurrentMenuItemID(menuInterventions.Text + "_" + menuIOPairs.Text);
                employeePresenceGraphicReports.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuEmplPresenceRep_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuWUStatisticalRep_Click(object sender, EventArgs e)
        {

            try
            {
                StatisticGraphicReports statisticGraphicReports = new StatisticGraphicReports();
                NotificationController.SetCurrentMenuItemID(menuWUStatisticalRep.Text);
                statisticGraphicReports.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuWUStatisticalRep_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemRestaurant_Click(object sender, EventArgs e)
        {
            try
            {
                if (modulesList.Contains((int)Constants.Moduls.RestaurantI))
                {
                    string costumer = Common.Misc.getCustomer(null);
                    int cost = 0;
                    bool costum = int.TryParse(costumer, out cost);
                    if (cost == (int)Constants.Customers.FIAT)
                    {
                        RestaurantIII restaurant = new RestaurantIII();
                        NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuItemRestaurant.Text);
                        restaurant.ShowDialog();
                    }
                    else
                    {
                        Restaurant restaurant = new Restaurant();
                        NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuItemRestaurant.Text);
                        restaurant.ShowDialog();
                    }
                }
                else if (modulesList.Contains((int)Constants.Moduls.RestaurantII))
                {
                    UI.RestaurantII res = new UI.RestaurantII();
                    res.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemRestaurant_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuRestaurant_Click(object sender, EventArgs e)
        {
            try
            {
                UsingMeals meals = new UsingMeals();
                NotificationController.SetCurrentMenuItemID(menuItemRestaurant.Text);
                meals.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemRestaurant_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuEmplPhotoManagament_Click(object sender, EventArgs e)
        {
            try
            {
                EmplPhotosMaintenance emplPhotosMaintenance = new EmplPhotosMaintenance();
                NotificationController.SetCurrentMenuItemID(menuEmplPhotoManagament.Text);
                emplPhotosMaintenance.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuEmplPhotoManagament_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuCameraSnapshostManagament_Click(object sender, EventArgs e)
        {
            try
            {
                CamSnapshotMaintenance camSnapshotMaintenance = new CamSnapshotMaintenance();
                NotificationController.SetCurrentMenuItemID(menuMaintaining.Text + "_" + menuPicManagament.Text + "_" + menuCameraSnapshostManagament.Text);
                camSnapshotMaintenance.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuCameraSnapshostManagament_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemTrespass_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                TrespassReports camSnapshotMaintenance = new TrespassReports();
                NotificationController.SetCurrentMenuItemID(menuItemTrespass.Text);
                camSnapshotMaintenance.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemTrespass_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void menuItemVisitors_Click(object sender, EventArgs e)
        {
            try
            {
                VisitorsMaintenance visitorMaintenance = new VisitorsMaintenance();
                NotificationController.SetCurrentMenuItemID(menuItemVisitors.Text);
                visitorMaintenance.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemVisitors_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuLog2XML_Click(object sender, EventArgs e)
        {
            try
            {
                ImportLog impLog = new ImportLog();
                NotificationController.SetCurrentMenuItemID(menuMaintaining.Text + "_" + menuLog2XML.Text);
                impLog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuLog2XML_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemMillenniumEmpl_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.Millennium.MillenniumEmpoyeeAnaliticReport emplReport = new Reports.Millennium.MillenniumEmpoyeeAnaliticReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemMillennium.Text + "_" + menuItemMillenniumEmpl.Text);
                emplReport.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemMillenniumEmpl_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemMillenniumWU_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.Millennium.MillenniumEmployeeAnalyticalWU WUReport = new Reports.Millennium.MillenniumEmployeeAnalyticalWU();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemMillennium.Text + "_" + menuItemMillenniumWU.Text); NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemMillennium.Text + "_" + menuItemMillenniumEmpl.Text);

                WUReport.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemMillenniumWU_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemMillenniumTypes_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.Millennium.MillenniumEmployeePresenceType presenceTypeReport = new Reports.Millennium.MillenniumEmployeePresenceType();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemMillennium.Text + "_" + menuItemMillenniumTypes.Text);
                presenceTypeReport.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemMillenniumTypes_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuRoutes_Click(object sender, EventArgs e)
        {
            try
            {
                SecurityRoutes secRoutes = new SecurityRoutes(Constants.routeTag);
                NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuRoutes.Text);
                secRoutes.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuRoutes_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemRoutes_Click(object sender, EventArgs e)
        {
            try
            {
                RoutesReports secRoutes = new RoutesReports(Constants.routeTag);
                secRoutes.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemRoutes_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuRoutesTerminal_Click(object sender, EventArgs e)
        {
            try
            {
                SecurityRoutes secRoutes = new SecurityRoutes(Constants.routeTerminal);
                NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuRoutesTerminal.Text);

                secRoutes.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuRoutesTerminal_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemRoutesTerminal_Click(object sender, EventArgs e)
        {
            try
            {
                RoutesReports secRoutes = new RoutesReports(Constants.routeTerminal);
                secRoutes.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemRoutesTerminal_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemATBFODPayment_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.ATBFOD.ATBFODPaymentReport paymentReport = new Reports.ATBFOD.ATBFODPaymentReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemATBFOD.Text + "_" + menuItemATBFODPayment.Text);
                paymentReport.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemATBFODPayment_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemATBFODEmplPresence_Click(object sender, EventArgs e)
        {
            try
            {
                //Reports.ATBFOD.ATBFODEmployeePresenceReportForWU presenceReport = new Reports.ATBFOD.ATBFODEmployeePresenceReportForWU();
                //NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemATBFOD.Text + "_" + menuItemATBFODEmplPresence.Text);
                //presenceReport.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemATBFODEmplAnalitic_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuMapMaintenance_Click(object sender, EventArgs e)
        {
            try
            {
                UI.Maps maps = new Maps();
                maps.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuMapMaintenance_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuMapObjectMaintenance_Click(object sender, EventArgs e)
        {
            try
            {
                UI.MapObjects mapObj = new UI.MapObjects();
                mapObj.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuMapObjectMaintenance_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuMapsReports_Click(object sender, EventArgs e)
        {
            try
            {
                UI.MapView mapObj = new UI.MapView();
                mapObj.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuMapsReports_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuStandardReport_Click(object sender, EventArgs e)
        {
            try
            {
                UI.CustomersVisitGraph visitGraph = new UI.CustomersVisitGraph();
                visitGraph.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuBasicReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuJUBMESPayrolls_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.JUBMES.JUBMESPaymentReports JUBMESPaymentReport = new Reports.JUBMES.JUBMESPaymentReports();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuJUBMES.Text + "_" + menuJUBMESPayrolls.Text);
                JUBMESPaymentReport.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuJUBMESPaymentRep_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuAdvancedReport_Click(object sender, EventArgs e)
        {
            try
            {
                UI.CustomVisitGraphAdv visitGraph = new UI.CustomVisitGraphAdv();
                visitGraph.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuAdvancedReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuBasicReport_Click(object sender, EventArgs e)
        {
            try
            {
                UI.CostumerVisitReport visitReport = new UI.CostumerVisitReport();
                visitReport.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuAdvancedReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuJEEPWUReports_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.JEEP.WorkingUnitsReports wuReports = new Reports.JEEP.WorkingUnitsReports();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuJEEP.Text + "_" + menuJEEPWUReports.Text);
                wuReports.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuJEEPWUReports_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.EmployeePresenceType wuReports = new Reports.EmployeePresenceType(true);
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuStandardRep.Text + "_"
                    + menuTimeSchedule.Text + "_" + menuItemReportsByType.Text);
                wuReports.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuJEEPWUReports_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemVacationEvidence_Click(object sender, EventArgs e)
        {
            try
            {
                UI.VacationEvidence vacEvid = new UI.VacationEvidence();
                vacEvid.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemVacationEvidence_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //EmplPersonalRecordsMDI ioPair = new EmplPersonalRecordsMDI();
                ////NotificationController.SetCurrentMenuItemID(menuMaintaining.Text + "_" + menuProcessingTables.Text
                ////+ "_" + menuPassesToIOPairs.Text);
                //ioPair.ShowDialog();               

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuPassesToIOPairs_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuMappingSiPass_Click(object sender, EventArgs e)
        {
            try
            {
                //SiemensMapping mapping = new SiemensMapping();
                //mapping.ShowDialog();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuMappingSiPass_Click): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuConnectionSetup_Click(object sender, EventArgs e)
        {
            try
            {
                //SiemensDBConnSetup setup = new SiemensDBConnSetup();
                //setup.ShowDialog();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuConnectionSetup_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        //private void menuItemLocking_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        UI.Lockings locking = new UI.Lockings();
        //        locking.ShowDialog(this);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.writeLog(DateTime.Now + " ACTA.menuItemLocking_Click(): " + ex.Message + "\n");
        //        MessageBox.Show(ex.Message);
        //    }
        //}


        private void menuItemUNIPROMIOPairsReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.UNIPROM.IOPairsReport rep = new Reports.UNIPROM.IOPairsReport();
                rep.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemUNIPROMIOPairsReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemUNIPROMRAMP_Click(object sender, EventArgs e)
        {
            try
            {
                //try
                //{
                //    form.Show();
                //    form.Focus();
                //}
                //catch
                //{
                //    form = new UnipromUI.RampControlForm();
                //    form.Show();
                //}

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemUNIPROMRAMP_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemUNIPROMDailyPreviewReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.UNIPROM.IOPairsUNIPROM rep = new Reports.UNIPROM.IOPairsUNIPROM();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemUNIPROM.Text + "_" + menuItemUNIPROMDailyPreviewReport.Text);
                rep.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemUNIPROMIOPairsReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }


        private void menuItemTagsPrview_Click(object sender, EventArgs e)
        {
            try
            {
                TagsPreview tp = new TagsPreview();
                NotificationController.SetCurrentMenuItemID(menuMaintaining.Text + "_" + menuItemTagsPrview.Text);
                tp.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemTagsPrview_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }

        private void menuItemPassesPreview_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.ZIN.ZINPassesReport tp = new Reports.ZIN.ZINPassesReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemZIN.Text + "_" + menuItemZINPassesPreview.Text);
                tp.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemTagsPrview_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }



        private void menuItemExtraHours_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.ExtraHoursPreview extra = new ExtraHoursPreview();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuStandardRep.Text
              + "_" + menuTimeSchedule.Text + "_" + menuItemExtraHours.Text);
                extra.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemTagsPrview_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }


        private void menuItemEmplCategories_Click(object sender, EventArgs e)
        {
            try
            {
                //SiemensUI.EmplCategories empl = new EmplCategories();
                //empl.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemEmplCategories_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //SiemensUI.VisitsAsco4 visits = new VisitsAsco4();
                //visits.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.button2_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

       

        private void menuReports_Click(object sender, EventArgs e)
        {

        }

        private void menuItemEUNETPresenceReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.EUNET.EmployeePresenceType type = new Reports.EUNET.EmployeePresenceType();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemEUNET.Text + "_" + menuItemEUNETPresenceReport.Text);
                type.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.button2_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuVisitors_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string costumer = Common.Misc.getCustomer(null);
                int cost = 0;
                bool costum = int.TryParse(costumer, out cost);
                if (cost == (int)Constants.Customers.ZIN)
                {
                    //SiemensUI.VisitsAsco4 visits = new VisitsAsco4();
                    //visits.ShowDialog();                    
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuVisitors_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void menuVisitors_Select(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string costumer = Common.Misc.getCustomer(null);
                int cost = 0;
                bool costum = int.TryParse(costumer, out cost);
                if (cost == (int)Constants.Customers.ZIN)
                {
                    //SiemensUI.VisitsAsco4 visits = new VisitsAsco4();
                    //visits.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuVisitors_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }



        private void menuMinistryEmplPresence_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                Reports.Ministarstvo.MinistarstvoEmployeePresence report = new Reports.Ministarstvo.MinistarstvoEmployeePresence();
                report.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemMinistry_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void menuItemEnterDataByEmployee_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                ACTASelftService.SelfServMain main = new ACTASelftService.SelfServMain();
                main.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemEnterDataByEmployee_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void menuItemGSKPresenceTracking_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                Reports.GSK.GSKPresenceTracking gsk = new Reports.GSK.GSKPresenceTracking();
                gsk.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemGSKPresenceTracking_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void menuItemStatisticReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.ZIN.ZINPresencePreview zin = new Reports.ZIN.ZINPresencePreview();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemZIN.Text + "_" + menuItemStatisticReport.Text);
                zin.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemStatisticReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }



        private void menuItemRestaurantII_Click(object sender, EventArgs e)
        {
            try
            {
                UI.RestaurantII res = new UI.RestaurantII();
                res.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemRestaurantII_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuAccessControl_Click(object sender, EventArgs e)
        {

        }

        private void menuItemMonthlyReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.Niksic.NiksicMonthlyReport monthlyReport = new Reports.Niksic.NiksicMonthlyReport();
                monthlyReport.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemMonthlyReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemSumPassesOnReader_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.Mittal.MittalCountPassesOnReader passesReport = new Reports.Mittal.MittalCountPassesOnReader();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuMittalRep.Text
                + "_" + menuItemSumPassesOnReader.Text);
                passesReport.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemSumPassesOnReader_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemLastTerminalReading_Click(object sender, EventArgs e)
        {
            try
            {
                LastTerminalReading terminalReading = new LastTerminalReading();
                NotificationController.SetCurrentMenuItemID(menuMaintaining.Text + "_" + menuItemLastTerminalReading.Text);
                terminalReading.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemLastTerminalReading_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemSinvozReportsByPassTypes_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.Sinvoz.PassTypesReport ptReport = new Reports.Sinvoz.PassTypesReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemSinvozCustomizedReports.Text + "_" + menuItemSinvozReportsByPassTypes.Text);
                ptReport.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemSinvozReportsByPassTypes_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemVlatacomWholeDayAbsenceAnnualReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.Vlatacom.WholeDayAbsenceAnnualReport absenceReport = new Reports.Vlatacom.WholeDayAbsenceAnnualReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemVlatacom.Text + "_" + menuItemVlatacomWholeDayAbsenceAnnualReport.Text);
                absenceReport.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemSinvozReportsByPassTypes_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            try
            {
                OrganizationalUnits wu = new OrganizationalUnits();

                NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuWorkingUnits.Text);

                wu.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuWorkingUnits_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }


        private void menuItem8_Click(object sender, EventArgs e)
        {
            try
            {
                PYIntegration integration = new PYIntegration();
                integration.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItem8_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuDSFPresenceReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.DSF.DSFPresenceReport presenceReport = new Reports.DSF.DSFPresenceReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuDSFReports.Text + "_" + menuDSFPresenceReport.Text);
                presenceReport.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuDSFPresenceReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemLamesCumulativeReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.Lames.LamesCumulativeReport cumulativeReport = new Reports.Lames.LamesCumulativeReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemLames.Text + "_" + menuItemLamesCumulativeReport.Text);
                cumulativeReport.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemLamesCumulativeReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemMassiveInput_Click(object sender, EventArgs e)
        {
            try
            {                
                // massive input is allowed only if system is closed and DP finshed his work
                SystemClosingEventTO eventTO = new SystemClosingEventTO();
                List<SystemClosingEventTO> closingList = new SystemClosingEvent().Search(DateTime.Now, DateTime.Now);

                foreach (SystemClosingEventTO evtTO in closingList)
                {
                    if (evtTO.Type.Trim().ToUpper() == Constants.closingEventTypeDemanded.Trim().ToUpper())
                    {
                        eventTO = evtTO;                        
                    }

                    if (evtTO.Type.Trim().ToUpper().Equals(Constants.closingEventTypeRegularPeriodical))
                    {
                        if ((evtTO.StartTime.TimeOfDay < evtTO.EndTime.TimeOfDay && DateTime.Now.TimeOfDay >= evtTO.StartTime.TimeOfDay && DateTime.Now.TimeOfDay < evtTO.EndTime.TimeOfDay)
                            || (evtTO.StartTime.TimeOfDay >= evtTO.EndTime.TimeOfDay && (DateTime.Now.TimeOfDay < evtTO.EndTime.TimeOfDay || DateTime.Now.TimeOfDay >= evtTO.StartTime.TimeOfDay)))
                        {
                            bool altLang = !NotificationController.GetLogInUser().LangCode.Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());

                            if (altLang)
                                MessageBox.Show(evtTO.MessageEN.Trim());
                            else
                                MessageBox.Show(evtTO.MessageSR.Trim());

                            return;
                        }
                    }
                }

                if (eventTO.EventID == -1 || eventTO.DPEngineState.Trim().ToUpper() != Constants.DPEngineState.FINISHED.ToString().Trim().ToUpper())
                {
                    MessageBox.Show(rm.GetString("systemNotClosed", culture));
                }
                else
                {
                    AbsenceMassiveInput absenceForm = new AbsenceMassiveInput();
                    NotificationController.SetCurrentMenuItemID(menuInterventions.Text + "_" + menuItemMassiveInput.Text);
                    absenceForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemMassiveInput_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuEmplResponisibility_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeesResponsibilities resForm = new EmployeesResponsibilities();
                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuDataAccess.Text + "_" + menuEmplResponisibility.Text);
                resForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuEmplResponisibility_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void meniItemWNDailyPresence_Click(object sender, EventArgs e)
        {
            try
            {
                DailyPresence presence = new DailyPresence();
                presence.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.meniItemWNDailyPresence_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemWNMonthly_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.WakerNeuson.WNMonthlyReport report = new Reports.WakerNeuson.WNMonthlyReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + meniItemWN.Text + " " + menuItemWNMonthly.Text);
                report.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemWNMonthly_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuConfezioniAndreaMonthlyReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.ConfezioniAndrea.CAMonthlyReport report = new Reports.ConfezioniAndrea.CAMonthlyReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuConfezioniAndrea.Text + " " + menuConfezioniAndreaMonthlyReport.Text);
                report.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuConfezioniAndreaMonthlyReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuRules_Click(object sender, EventArgs e)
        {
            try
            {
                Rules form = new Rules();
                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuRules.Text);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuRules_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuCutOffDate_Click(object sender, EventArgs e)
        {
            try
            {
                RulesCutOffDate form = new RulesCutOffDate();
                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuCutOffDate.Text);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuCutOffDate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            new MedicalCheck().ShowDialog();
        }

        private void menuMedicalCheck(object sender, EventArgs e)
        {
            try
            {
                NotificationController.SetCurrentMenuItemID(menuLibraries.Text + "_" + menuItemMC.Text);
                new MedicalCheck().ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuMedicalCheck(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemRestartCounters_Click(object sender, EventArgs e)
        {
            try
            {
                RestartCounters form = new RestartCounters();
                NotificationController.SetCurrentMenuItemID(menuMaintaining.Text + "_" + menuItemRestartCounters.Text);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemRestartCounters_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemSynchronization_Click(object sender, EventArgs e)
        {
            try
            {
                SynchronizationPreview form = new SynchronizationPreview();
                NotificationController.SetCurrentMenuItemID(menuMaintaining.Text + "_" + menuItemSynchronization.Text);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemSynchronization_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemPMCMonthlyReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.PMC.PMCMonthlyReport report = new Reports.PMC.PMCMonthlyReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemPMC.Text + " " + menuItemPMCMonthlyReport.Text);
                report.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemPMCMonthlyReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemPMCCumulativeReports_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.PMC.PMCCumulativeReports report = new Reports.PMC.PMCCumulativeReports();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemPMC.Text + " " + menuItemPMCCumulativeReports.Text);
                report.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemPMCCumulativeReports_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemPMCStatisticalReports_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.PMC.PMCStatisticalReports report = new Reports.PMC.PMCStatisticalReports();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemPMC.Text + " " + menuItemPMCStatisticalReports.Text);
                report.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemPMCStatisticalReports_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemSystemClosingEvents_Click(object sender, EventArgs e)
        {
            try
            {
                SystemClosingEvents form = new SystemClosingEvents();
                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuItemSystem.Text + "_" + menuItemSystemClosingEvents.Text);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemSystemClosingEvents_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemSystemMessages_Click(object sender, EventArgs e)
        {
            try
            {
                SystemMessages form = new SystemMessages();
                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuItemSystem.Text + "_" + menuItemSystemMessages.Text);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemSystemMessages_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemPMCPaymentReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.PMC.PMCPYReport report = new Reports.PMC.PMCPYReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemPMC.Text + " " + menuItemPMCPaymentReport.Text);
                report.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemPMCPaymentReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemEmplCounterBalances_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeesCountersBalances form = new EmployeesCountersBalances();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemFiat.Text + " " + menuItemEmplCounterBalances.Text);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemEmplCounterBalances_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemGeoxPYReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.Geox.GeoxPYReport report = new Reports.Geox.GeoxPYReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemGeox.Text + " " + menuItemGeoxPYReport.Text);
                report.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemGeoxPYReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemDecisions_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.FIAT.Decisions report = new Reports.FIAT.Decisions();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemFiat.Text + " " + menuItemFiatDecisions.Text);
                report.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemGeoxPYReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            Reports.FIAT.Decisions report = new Reports.FIAT.Decisions();
            //NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemFiat.Text + " " + menuItemGeoxPYReport.Text);
            report.ShowDialog();
        }

        private void menuItemCounterType_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeeCounterTypes employeeCounterTypes = new EmployeeCounterTypes();
                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuItemSystem.Text + "_" + menuItemCounterType.Text);
                employeeCounterTypes.ShowDialog();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemCounterType_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemUsersCategory_Click(object sender, EventArgs e)
        {

            try
            {
                ApplUsersCategories applUsersCategories = new ApplUsersCategories();
                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuItemSystem.Text + "_" + menuItemUsersCategory.Text);
                applUsersCategories.ShowDialog();
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemUsersCategory_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }


        private void menuItemULChangesTbl_Click(object sender, EventArgs e)
        {
            try
            {
                
                ApplUsersLoginChangesTbls applUsersLoginChangesTbls = new ApplUsersLoginChangesTbls();
                NotificationController.SetCurrentMenuItemID(menuConfiguration.Text + "_" + menuItemSystem.Text + "_" + menuItemULChangesTbl.Text);
                applUsersLoginChangesTbls.ShowDialog();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemULChangesTbl_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCreateFiles_Click(object sender, EventArgs e)
        {
            try
            {
                CreateAndroidFiles createFiles = new CreateAndroidFiles();
                createFiles.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMealsOrder_Click(object sender, EventArgs e)
        {
            try
            {
                MealsOrder mealsOrder = new MealsOrder();
                mealsOrder.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemGrundfosTransportData_Click(object sender, EventArgs e)
        {
            try
            {
                TransportData dataForm = new TransportData();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemGrundfos.Text + " " + menuItemGrundfosTransportData.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemGrundfosTransportData_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemGrundfosPYReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.Grundfos.GrundfosPYReport dataForm = new Reports.Grundfos.GrundfosPYReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemGrundfos.Text + " " + menuItemGrundfosPYReport.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemGrundfosPYReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemHyattPYReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.Hyatt.HyattPYReport dataForm = new Reports.Hyatt.HyattPYReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemHyatt.Text + " " + menuItemHyattPYReport.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemHyattPYReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }
        private void menuItemHyattTimeAndAtendanceReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.Hyatt.HyattTimeAndAtendance dataForm = new Reports.Hyatt.HyattTimeAndAtendance();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemHyatt.Text + " " + menuItemHyattTimeAndAttendance.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemHyattTimeAndAttendance_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            try
            {
                Reports.OperatersRoles dataForm = new Reports.OperatersRoles();
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemHyattTimeAndAttendance_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemHyattOperatersCategoriesReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.OperatersCategories dataForm= new Reports.OperatersCategories();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemHyatt.Text + " " + menuItemHyattOperatersCategoriesReport.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemHyattOperatersCategoriesReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click_4(object sender, EventArgs e)
        {
            try
            {
                Reports.RolesPrivilegesReportHRSSC dataForm = new Reports.RolesPrivilegesReportHRSSC();
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemHyattTimeAndAttendance_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemHyattOperatersRolesReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.OperatersRoles dataForm = new Reports.OperatersRoles();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemHyatt.Text + " " + menuItemHyattOperatersRolesReport.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemHyattOperatersRolesReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemHyattOperatersOUWUReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.OperatersOUWUReports dataForm = new Reports.OperatersOUWUReports();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemHyatt.Text + " " + menuItemHyattOperatersOUWUReport.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemHyattOperatersOUWUReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemHyattCategoriesPrivilegesReport_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.CategoriesPrivilegesReport dataForm = new Reports.CategoriesPrivilegesReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemHyatt.Text + " " + menuItemHyattCategoriesPrivilegesReport.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemHyattCategoriesPrivilegesReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

       

        private void menuItemEmployeesPersonalHoliday_Click(object sender, EventArgs e)
        {
            try
            {
                Reports.EmployeesPersonalHoliday dataForm = new Reports.EmployeesPersonalHoliday();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemHyatt.Text + " " + menuItemEmployeesPersonalHoliday.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemEmployeesPersonalHoliday_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemHyattRolesPrivilegiesReportHRSSC_Click(object sender, EventArgs e)
        {
            try
            {

                Reports.RolesPrivilegesReportHRSSC dataForm = new Reports.RolesPrivilegesReportHRSSC();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemHyatt.Text + " " + menuItemHyattRolesPrivilegiesReportHRSSC.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemHyattRolePrivilegesReportHRSSC_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemHyattRolesPrivilegiesReportPY_Click_1(object sender, EventArgs e)
        {

            try
            {

                Reports.RolesPrivilegesReportPY dataForm = new Reports.RolesPrivilegesReportPY();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemHyatt.Text + " " + menuItemHyattRolesPrivilegiesReportPY.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemHyattRolePrivilegesReportPY_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }

        private void menuItemHyattRolesPrivilegiesReportCANTEEN_Click(object sender, EventArgs e)
        {
            try
            {

                Reports.RolesPrivilegesReportCANTEEN dataForm = new Reports.RolesPrivilegesReportCANTEEN();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemHyatt.Text + " " + menuItemHyattRolesPrivilegiesReportCANTEEN.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemHyattRolePrivilegesReportCANTEEN_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemHyattRolesPrivilegiesReportHRSSCEXTENDED_Click(object sender, EventArgs e)
        {
            try
            {

                Reports.RolesPrivilegesReportHRSSCEXTENDED dataForm = new Reports.RolesPrivilegesReportHRSSCEXTENDED();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemHyatt.Text + " " + menuItemHyattRolesPrivilegiesReportHRSSCEXTENDED.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemHyattRolesPrivilegiesReportHRSSCEXTENDED_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemHyattRolesPrivilegiesReportMedicalCheck_Click_1(object sender, EventArgs e)
        {
            try
            {

                Reports.RolesPrivilegesReportMedicalCheck dataForm = new Reports.RolesPrivilegesReportMedicalCheck();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemHyatt.Text + " " + menuItemHyattRolesPrivilegiesReportMedicalCheck.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemHyattRolesPrivilegiesReportMedicalCheck_Click_1(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemHyattRolesPrivilegiesReportSystemClose_Click(object sender, EventArgs e)
        {
            try
            {

                Reports.RolesPrivilegesReportSystemClose dataForm = new Reports.RolesPrivilegesReportSystemClose();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemHyatt.Text + " " + menuItemHyattRolesPrivilegiesReportSystemClose.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemHyattRolesPrivilegiesReportSystemClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemHyattPYGIDReport_Click(object sender, EventArgs e)
        {
            try
            {

                Reports.Hyatt.HyattPYGIDReport dataForm = new Reports.Hyatt.HyattPYGIDReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemHyatt.Text + " " + menuItemHyattPYGIDReport.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemHyattPYGIDReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemAnualLeaveReport_Click(object sender, EventArgs e)
        {
            try
            {

                Reports.AnnualLeaveReport dataForm = new Reports.AnnualLeaveReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemHyatt.Text + " " + menuItemAnualLeaveReport.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemAnnualLeaveReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemStartWorkingDate_Click(object sender, EventArgs e)
        {
            try
            {

                Reports.StartWorkingDateReport dataForm = new Reports.StartWorkingDateReport();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemHyatt.Text + " " + menuItemStartWorkingDate.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemStartWorkingDate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItemEmployeePT_Click(object sender, EventArgs e)
        {
            try
            {

                Reports.Magna.MagnaMilos dataForm = new Reports.Magna.MagnaMilos();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuCustomizedReports.Text + "_" + menuItemMagna.Text + " " + menuItemEmployeePT.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuItemEmployeePT_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuDocManipulation_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeesDocuments emplDocMaintenance = new EmployeesDocuments();
                NotificationController.SetCurrentMenuItemID(menuDocManipulation.Text);
                emplDocMaintenance.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuDocManipulation_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuMonthlyTypeReport_Click(object sender, EventArgs e)
        {            
            try
            {
                MonthlyPresenceTracking dataForm = new MonthlyPresenceTracking();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuStandardRep.Text + "_"
                    + menuPresence.Text + "_" + menuMonthlyTypeReport.Text);
                dataForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuMonthlyTypeReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
           
        }

        private void menuReportsForSalaryForecast_Click(object sender, EventArgs e)
        {
            try
            {
                ReportForSalaryForecast salaryREportForm = new ReportForSalaryForecast();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuStandardRep.Text + "_"
                    + menuPresence.Text + "_" + menuReportsForSalaryForecast.Text);
                salaryREportForm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuReportsForSalaryForecast_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }

        private void menuReportsForOpenPairsByEmployee_Click(object sender, EventArgs e) {
            try {
                OpenPairsReportByEmployees Form = new OpenPairsReportByEmployees();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuStandardRep.Text + "_"
                    + menuPresence.Text + "_" + menuReportsForOpenPairsByEmployee.Text);
                Form.ShowDialog();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " ACTA.menuReportsForOpenPairsByEmployee_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuVacationReport_Click(object sender, EventArgs e) {
            try {
                ReportsForVacation Form = new ReportsForVacation();
                NotificationController.SetCurrentMenuItemID(menuReports.Text + "_" + menuStandardRep.Text + "_"
                    + menuTimeSchedule.Text + "_" + menuVacationReport.Text);
                Form.ShowDialog();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " ACTA.menuVacationReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuRecordsOfBreaks_Click(object sender, EventArgs e)
        {
            try
            {
                UI.RecordsOfBreaks form = new UI.RecordsOfBreaks();

                NotificationController.SetCurrentMenuItemID(menuInterventions.Text + "_" + menuRecordsOfBreaks.Text);

                form.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuRecordsOfBreaks_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuMachines_Click(object sender, EventArgs e)
        {
            try
            {
                Machines form = new Machines();

                NotificationController.SetCurrentMenuItemID(menuInterventions.Text + "_" + menuMachines.Text);

                form.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ACTA.menuMachines_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItem8_Click_1(object sender, EventArgs e)
        {
            BrojPrisutnihOdsutnihSMP frm = new BrojPrisutnihOdsutnihSMP();
            frm.ShowDialog();
        }

       
       
    
        
        

      
    }
}


