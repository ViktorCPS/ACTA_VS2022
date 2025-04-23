using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using SyncDataAccess;
using TransferObjects;
using Util;
using System.IO;

namespace ACTASyncManagement
{
    public class SyncManager
    {
        //thread for reading cards on validation terminal
        private Thread worker;
        //stop thread
        private bool stopMonitoring = false;

        //debuging object
        private DebugLog log = null;

        //for thread controlling
        private object locker = new object();

        public string managerThreadStatus;

        private string statusNOTOK = "Thread ERROR";

        Object ACTAConnection;
        Object SyncConnection;

        SynchronizationDAO syncDAO;

        //transaction start time in ticks
        private DateTime nextSyncTime = Constants.SyncStartTime;

        //how long to wait response from validation terminal
        private long syncTimeout = Constants.SyncTimeout;  // in min

        public void Start()
        {
            // set next sync time
            while (nextSyncTime < DateTime.Now)
                nextSyncTime = nextSyncTime.AddMinutes(syncTimeout);

            ApplUserTO dpUser = new ApplUserTO();
            dpUser.UserID = Constants.syncUser;
            NotificationController.SetLogInUser(dpUser);
            WriteLog(" SyncManager started!");
            worker = new Thread(new ThreadStart(Work));
            worker.Name = "Sync Manager";
            worker.Start();
        }

        public SyncManager()
		{
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);			
        }

        public void Stop()
        {
            try
            {
                WriteLog(" SyncManager is ending!");
                stopMonitoring = true;
                if (worker != null)
                {
                    worker.Join(); // Wait for the worker's thread to finish.
                }

                WriteLog(" SyncManager stopped! ");
            }
            catch (Exception ex)
            {
                log.writeLog(ex);
            }
        }
        
        private void Work()
        {
            try
            {
                while (!stopMonitoring)
                {
                    if (isSyncTime())
                    {
                        if (connectToDataBases())
                        {
                            SetManagerThreadStatus("Synchronization Financial Structure ");
                            WriteLog(" SyncFinancialStructure STARTED+++");
                            SyncFinancialStructure();
                            WriteLog(" SyncFinancialStructure FINISHED+++");
                            SetManagerThreadStatus(" Synchronization Cost Centers ");
                            WriteLog(" SyncCostCenters STARTED+++");
                            SyncCostCenters();
                            WriteLog(" SyncCostCenters FINISHED+++");
                            WriteLog(" SyncOrganizacionalStructure STARTED+++");
                            SetManagerThreadStatus(" Synchronization Organizational Structure ");
                            SyncOrganizacionalStructure();
                            WriteLog(" SyncOrganizacionalStructure FINISHED+++");
                            WriteLog(" SyncEmployeePositions STARTED+++");
                            SetManagerThreadStatus(" Synchronization Employee Positions ");
                            SyncEmployeePositions();
                            WriteLog(" SyncEmployeePositions FINISHED+++");
                            WriteLog(" SyncEmployees STARTED+++");
                            SetManagerThreadStatus(" Synchronization Employees ");
                            SyncEmployees();
                            WriteLog(" SyncEmployees FINISHED+++");
                            WriteLog(" SyncResponsibilityData STARTED+++");
                            SetManagerThreadStatus(" Synchronization Responsibility Data ");
                            SyncResponsibilityData();
                            WriteLog(" SyncResponsibilityData FINISHED+++");
                            SetManagerThreadStatus("Synchronization Annnual Leaves Recalculation ");
                            WriteLog(" SyncAnnualLeaveRecalculation STARTED+++");
                            SyncAnnualLeaveRecalculation();
                            WriteLog(" SyncAnnualLeaveRecalculation FINISHED+++");
                            SetManagerThreadStatus("Synchronization Inactive Users ");
                            WriteLog(" SyncInactiveUsers STARTED+++");
                            SyncInactiveUsers();
                            WriteLog(" SyncInactiveUsers FINISHED+++");
                            WriteLog(" SyncBufferData STARTED+++");
                            SetManagerThreadStatus(" Synchronization Buffer Data ");
                            SyncBufferData();
                            WriteLog(" SyncBufferData FINISHED+++");
                            SetManagerThreadStatus(" Connection closing ");
                            closeConnections();                            
                            nextSyncTime = nextSyncTime.AddMinutes(syncTimeout);
                            SetManagerThreadStatus(" Next migration at " + nextSyncTime.ToString("dd.MM.yyyy HH:mm") + "\n Ready...");
                            WriteLog(" Next migration at " + nextSyncTime.ToString("dd.MM.yyyy HH:mm") + "\n");
                        }
                    }

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                WriteLog(" WORK() exception: " + ex.Message);
                SetManagerThreadStatus(statusNOTOK);
            }
        }

        private void SyncInactiveUsers()
        {
            try
            {
                // all users created six months ago and before that never succesufully logged in on system, should be retired
                ApplUser user = new ApplUser(ACTAConnection);
                List<ApplUserTO> inactiveUsersList = user.SearchInactiveUsers(DateTime.Now.Date.AddMonths(-Constants.FiatInactiveUsersMonths));

                foreach (ApplUserTO userTO in inactiveUsersList)
                {
                    // set user to RETIRED
                    userTO.Status = Constants.statusRetired;
                    userTO.ModifiedBy = Constants.syncUser;
                    user.UserTO = userTO;
                    user.Update();
                }
            }
            catch (Exception ex)
            {
                WriteLog(" SyncInactiveUsers() exception: " + ex.Message);
            }
        }

        private void SyncResponsibilityData()
        {
            try
            {   
                //list to insert to sync table
                List<SyncResponsibilityTO> responsibilityRecords = syncDAO.getDifResponsibility();

                  //if there is no records continue
                if (responsibilityRecords.Count <= 0)
                {
                    WriteLog(" SyncEmployees - Records not found. ");
                }
                //if records found begin processing
                else
                {
                    WriteLog(" SyncResponsibilityData - Num of records found: " + responsibilityRecords.Count.ToString());

                    Employee employee = new Employee(ACTAConnection);
                    EmployeeAsco4 employeeAsco4 = new EmployeeAsco4(ACTAConnection);
                    EmployeeResponsibility emplRespons = new EmployeeResponsibility(ACTAConnection);
                    WorkingUnit workingUnit = new WorkingUnit(ACTAConnection);
                    OrganizationalUnit orgUnit = new OrganizationalUnit(ACTAConnection);                    
                    ApplUsersXWU applUserXwu = new ApplUsersXWU(ACTAConnection);
                    ApplUserXOrgUnit applUserXou = new ApplUserXOrgUnit(ACTAConnection);
                    SyncResponsibilityHist syncResponse = new SyncResponsibilityHist(ACTAConnection);
                    ApplUserXApplUserCategory applUserXCategory = new ApplUserXApplUserCategory(ACTAConnection);
                    ApplUser user = new ApplUser(ACTAConnection);

                    Dictionary<int, WorkingUnitTO> wuDict = workingUnit.getWUDictionary();
                    Dictionary<int, OrganizationalUnitTO> ouDict = orgUnit.SearchDictionary();

                    int count = 0;
                    foreach (SyncResponsibilityTO syncResp in responsibilityRecords)
                    {
                        count++;
                        SetManagerThreadStatus("Synchronization Responsibility Data " + count.ToString() + "/" + responsibilityRecords.Count.ToString());

                        EmployeeResponsibilityTO respTODel = new EmployeeResponsibilityTO();
                        EmployeeResponsibilityTO respTOInsert = new EmployeeResponsibilityTO();
                        List<ApplUsersXWUTO> applUsersXWUInsert = new List<ApplUsersXWUTO>();
                        List<ApplUsersXWUTO> applUsersXWUDelete = new List<ApplUsersXWUTO>();
                        List<ApplUserXOrgUnitTO> applUsersXOUInsert = new List<ApplUserXOrgUnitTO>();
                        List<ApplUserXOrgUnitTO> applUsersXOUDelete = new List<ApplUserXOrgUnitTO>();
                        ApplUserXApplUserCategoryTO applUsersXCategoryInsert = new ApplUserXApplUserCategoryTO();
                        ApplUserXApplUserCategoryTO applUsersXCategoryDel = new ApplUserXApplUserCategoryTO();
                        EmployeeAsco4TO employeeAsco4TO = new EmployeeAsco4TO();

                        try
                        {
                            string error = "";
                            if (syncResp.ValidFrom.Equals(new DateTime()) && syncResp.ValidTo.Equals(new DateTime()))
                                error = "No validation dates for rec_id: " + syncResp.RecID.ToString().Trim();

                            if ((!syncResp.ValidFrom.Equals(new DateTime()) && syncResp.ValidFrom.Date > DateTime.Now.Date)
                                || (syncResp.ValidFrom.Equals(new DateTime()) && !syncResp.ValidTo.Equals(new DateTime()) && syncResp.ValidTo.Date > DateTime.Now.Date))
                            {
                                continue;
                            }

                            bool delRecord = false;

                            if (syncResp.ValidTo.Date <= DateTime.Now.Date)
                            {
                                delRecord = true;
                            }
                            
                            if (syncResp.ValidTo.Date < syncResp.ValidFrom.Date && syncResp.ValidTo != new DateTime())
                            {
                                error = "Valid from can not be after valid to for rec_id = " + syncResp.RecID;
                            }
                            if (syncResp.UnitID == -1 || syncResp.ResponsiblePersonID == -1 || syncResp.StructureType == "")
                            {
                                error = "Argument missing for rec_id = " + syncResp.RecID;
                            }

                            if (error.Trim().Equals(""))
                            {
                                employeeAsco4.EmplAsco4TO = new EmployeeAsco4TO();
                                employeeAsco4.EmplAsco4TO.EmployeeID = syncResp.ResponsiblePersonID;
                                List<EmployeeAsco4TO> ascoList = employeeAsco4.Search();                                
                                if (ascoList.Count > 0)
                                    employeeAsco4TO = ascoList[0];

                                if (employeeAsco4TO.EmployeeID == -1)
                                    error = "Employee not found for rec_id = " + syncResp.RecID;

                                if (employeeAsco4TO.NVarcharValue5 == "")
                                    error = "Username not found for rec_id = " + syncResp.RecID;
                                else
                                {
                                    user.UserTO = new ApplUserTO();
                                    user.UserTO.UserID = employeeAsco4TO.NVarcharValue5;
                                    List<ApplUserTO> users = user.Search();
                                    if (users.Count <= 0)
                                        error = "No user found for user ID = " + employeeAsco4TO.NVarcharValue5 + "; rec_id = " + syncResp.RecID;                                    
                                }

                                if (!Constants.SyncRespTypes.ContainsKey(syncResp.StructureType))
                                    error = "Invalid structure type for rec_id = " + syncResp.RecID;

                                if (syncResp.StructureType == Constants.syncResFSType)
                                {
                                    if (!wuDict.ContainsKey(syncResp.UnitID))
                                        error = "Working unit with unit_id = " + syncResp.UnitID + "; NOT found";
                                }
                                else if (syncResp.StructureType == Constants.syncResOSType)
                                {
                                    if (!ouDict.ContainsKey(syncResp.UnitID))
                                        error = "Organizational unit with unit_id = " + syncResp.UnitID + "; NOT found";
                                }

                                if (error.Trim().Equals(""))
                                {
                                    emplRespons.ResponsibilityTO = new EmployeeResponsibilityTO();
                                    emplRespons.ResponsibilityTO.EmployeeID = syncResp.ResponsiblePersonID;
                                    emplRespons.ResponsibilityTO.UnitID = syncResp.UnitID;
                                    emplRespons.ResponsibilityTO.Type = Constants.SyncRespTypes[syncResp.StructureType];

                                    List<EmployeeResponsibilityTO> list = emplRespons.Search();
                                    if (list.Count > 0)
                                    {
                                        if (syncResp.ValidTo.Date > DateTime.Now.Date)
                                        {
                                            continue;
                                        }
                                        else if (syncResp.ValidTo.Equals(new DateTime()))
                                            error = "Responsibility for employee: " + syncResp.ResponsiblePersonID.ToString().Trim() + " and unit: " + syncResp.UnitID.ToString().Trim() 
                                                + " and type: " + syncResp.StructureType.Trim() + " already exists";
                                        else
                                        {
                                            respTODel = list[0];

                                            if (respTODel.Type == Constants.emplResTypeWU)
                                            {
                                                bool deleteSupervisorCategory = true;
                                                bool responsibleUnitCC = false;
                                                int defaultUnitID = -1;

                                                // get responsible unit
                                                WorkingUnitTO resUnit = new WorkingUnitTO();
                                                if (wuDict.ContainsKey(respTODel.UnitID))
                                                    resUnit = wuDict[respTODel.UnitID];

                                                string unitCCCode = "";

                                                if (resUnit.Name.Length >= Constants.fsPlantCodeLenght + 1 + Constants.fsCostCenterCodeLenght)
                                                    unitCCCode = resUnit.Name.Substring(0, Constants.fsCostCenterCodeLenght + Constants.fsPlantCodeLenght + 1);

                                                int unitCompany = Common.Misc.getRootWorkingUnit(resUnit.WorkingUnitID, wuDict);

                                                // if there exist other FS responsibility, leave category
                                                // if there exist other responsibility for same cost center, leave visibility, else delete visibility under unit cost center
                                                emplRespons.ResponsibilityTO = new EmployeeResponsibilityTO();
                                                emplRespons.ResponsibilityTO.EmployeeID = syncResp.ResponsiblePersonID;
                                                emplRespons.ResponsibilityTO.Type = respTODel.Type;
                                                list = emplRespons.Search();

                                                foreach (EmployeeResponsibilityTO oldResp in list)
                                                {
                                                    if (oldResp.UnitID == respTODel.UnitID)
                                                        continue;

                                                    if (defaultUnitID == -1)
                                                        defaultUnitID = oldResp.UnitID;

                                                    deleteSupervisorCategory = false;

                                                    WorkingUnitTO wuTO = new WorkingUnitTO();
                                                    if (wuDict.ContainsKey(oldResp.UnitID))
                                                        wuTO = wuDict[oldResp.UnitID];

                                                    int rootID = Common.Misc.getRootWorkingUnit(wuTO.WorkingUnitID, wuDict);
                                                    string costCenterCode = "";
                                                    if (wuTO.Name.Length >= Constants.fsPlantCodeLenght + 1 + Constants.fsCostCenterCodeLenght)
                                                        costCenterCode = wuTO.Name.Substring(0, Constants.fsCostCenterCodeLenght + Constants.fsPlantCodeLenght + 1);

                                                    if (unitCCCode == costCenterCode && unitCompany == rootID)
                                                    {
                                                        responsibleUnitCC = true;
                                                        break;
                                                    }
                                                }

                                                if (deleteSupervisorCategory)
                                                {
                                                    applUserXCategory.UserXCategoryTO.CategoryID = (int)Constants.UserCategoriesFIAT.Supervisor;
                                                    applUserXCategory.UserXCategoryTO.UserID = employeeAsco4TO.NVarcharValue5;
                                                    List<ApplUserXApplUserCategoryTO> wuList = applUserXCategory.Search();
                                                    if (wuList.Count > 0)
                                                    {
                                                        applUsersXCategoryDel = wuList[0];
                                                    }
                                                }

                                                if (!responsibleUnitCC)
                                                {
                                                    // get unit cost center
                                                    workingUnit.WUTO = new WorkingUnitTO();
                                                    workingUnit.WUTO.Name = unitCCCode;
                                                    List<WorkingUnitTO> CostCenterList = workingUnit.SearchExact();
                                                    WorkingUnitTO costCenterWU = new WorkingUnitTO();
                                                    if (CostCenterList.Count > 0)
                                                    {
                                                        foreach (WorkingUnitTO wu in CostCenterList)
                                                        {
                                                            int ID = Common.Misc.getRootWorkingUnit(wu.WorkingUnitID, wuDict);
                                                            if (unitCompany == ID)
                                                            {
                                                                costCenterWU = wu;
                                                            }
                                                        }
                                                    }

                                                    if (costCenterWU.WorkingUnitID != -1)
                                                    {
                                                        List<WorkingUnitTO> ccList = new List<WorkingUnitTO>();
                                                        ccList.Add(costCenterWU);
                                                        ccList = new WorkingUnit(ACTAConnection).FindAllChildren(ccList);

                                                        foreach (WorkingUnitTO ccUnit in ccList)
                                                        {
                                                            ApplUsersXWUTO userXwuTO = new ApplUsersXWUTO();
                                                            userXwuTO.UserID = employeeAsco4TO.NVarcharValue5;
                                                            userXwuTO.WorkingUnitID = ccUnit.WorkingUnitID;
                                                            userXwuTO.Purpose = Constants.EmployeesPurpose;
                                                            applUsersXWUDelete.Add(userXwuTO);
                                                        }
                                                    }
                                                }

                                                //set first working unit to default one
                                                if (employeeAsco4TO.IntegerValue2 == syncResp.UnitID)
                                                    employeeAsco4TO.IntegerValue2 = defaultUnitID;
                                            }
                                            else if (respTODel.Type == Constants.emplResTypeOU)
                                            {
                                                bool deleteWCDRCategory = true;
                                                int defaultUnitID = -1;

                                                // get responsible unit
                                                OrganizationalUnitTO resUnit = new OrganizationalUnitTO();
                                                if (ouDict.ContainsKey(respTODel.UnitID))
                                                    resUnit = ouDict[respTODel.UnitID];

                                                // if there is responsibility for parent unit, do not delete visibility
                                                List<EmployeeResponsibilityTO> parentResList = new List<EmployeeResponsibilityTO>();
                                                if (resUnit.ParentOrgUnitID != resUnit.OrgUnitID)
                                                {
                                                    emplRespons.ResponsibilityTO = new EmployeeResponsibilityTO();
                                                    emplRespons.ResponsibilityTO.EmployeeID = syncResp.ResponsiblePersonID;
                                                    emplRespons.ResponsibilityTO.Type = respTODel.Type;
                                                    emplRespons.ResponsibilityTO.UnitID = resUnit.ParentOrgUnitID;
                                                    parentResList = emplRespons.Search();
                                                }

                                                List<OrganizationalUnitTO> unitChildren = new List<OrganizationalUnitTO>();
                                                if (resUnit.OrgUnitID != -1)
                                                {
                                                    unitChildren.Add(resUnit);
                                                    unitChildren = new OrganizationalUnit(ACTAConnection).FindAllChildren(unitChildren);
                                                }

                                                // delete unit and all it's children visibilities
                                                List<int> unitChildrenIDs = new List<int>();
                                                foreach (OrganizationalUnitTO child in unitChildren)
                                                {
                                                    if (parentResList.Count <= 0)
                                                    {
                                                        unitChildrenIDs.Add(child.OrgUnitID);

                                                        ApplUserXOrgUnitTO userUnit = new ApplUserXOrgUnitTO();
                                                        userUnit.UserID = employeeAsco4TO.NVarcharValue5;
                                                        userUnit.OrgUnitID = child.OrgUnitID;
                                                        userUnit.Purpose = Constants.EmployeesPurpose;
                                                        applUsersXOUDelete.Add(userUnit);
                                                    }
                                                }

                                                // if there exist other OS responsibility, leave category                                                
                                                // if there is responsibility for any unit child, insert child and all it's children visibilities
                                                emplRespons.ResponsibilityTO = new EmployeeResponsibilityTO();
                                                emplRespons.ResponsibilityTO.EmployeeID = syncResp.ResponsiblePersonID;
                                                emplRespons.ResponsibilityTO.Type = respTODel.Type;
                                                list = emplRespons.Search();

                                                // get organization units already visible
                                                applUserXou.AuXOUnitTO = new ApplUserXOrgUnitTO();
                                                applUserXou.AuXOUnitTO.UserID = employeeAsco4TO.NVarcharValue5;
                                                applUserXou.AuXOUnitTO.Purpose = Constants.EmployeesPurpose;
                                                List<ApplUserXOrgUnitTO> unitsVisible = applUserXou.Search();

                                                List<int> unitID = new List<int>();

                                                foreach (ApplUserXOrgUnitTO visibleUnit in unitsVisible)
                                                {
                                                    if (!unitChildrenIDs.Contains(visibleUnit.OrgUnitID))
                                                        unitID.Add(visibleUnit.OrgUnitID);
                                                }

                                                foreach (EmployeeResponsibilityTO oldResp in list)
                                                {
                                                    if (oldResp.UnitID == respTODel.UnitID)
                                                        continue;

                                                    if (defaultUnitID == -1)
                                                        defaultUnitID = oldResp.UnitID;

                                                    deleteWCDRCategory = false;

                                                    if (unitChildrenIDs.Contains(oldResp.UnitID))
                                                    {
                                                        OrganizationalUnitTO orgUnitTO = new OrganizationalUnitTO();
                                                        if (ouDict.ContainsKey(oldResp.UnitID))
                                                            orgUnitTO = ouDict[oldResp.UnitID];

                                                        if (orgUnitTO.OrgUnitID != -1)
                                                        {
                                                            List<OrganizationalUnitTO> orgUnitsTOSee = new List<OrganizationalUnitTO>();
                                                            orgUnitsTOSee.Add(orgUnitTO);
                                                            orgUnitsTOSee = new OrganizationalUnit(ACTAConnection).FindAllChildren(orgUnitsTOSee);

                                                            foreach (OrganizationalUnitTO unit in orgUnitsTOSee)
                                                            {
                                                                if (!unitID.Contains(unit.OrgUnitID))
                                                                {
                                                                    ApplUserXOrgUnitTO userUnit = new ApplUserXOrgUnitTO();
                                                                    userUnit.UserID = employeeAsco4TO.NVarcharValue5;
                                                                    userUnit.OrgUnitID = unit.OrgUnitID;
                                                                    userUnit.Purpose = Constants.EmployeesPurpose;
                                                                    applUsersXOUInsert.Add(userUnit);
                                                                    unitID.Add(userUnit.OrgUnitID);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (deleteWCDRCategory)
                                                {
                                                    applUserXCategory.UserXCategoryTO.CategoryID = (int)Constants.UserCategoriesFIAT.WCDirectRisponsible;
                                                    applUserXCategory.UserXCategoryTO.UserID = employeeAsco4TO.NVarcharValue5;
                                                    List<ApplUserXApplUserCategoryTO> wuList = applUserXCategory.Search();
                                                    if (wuList.Count > 0)
                                                    {
                                                        applUsersXCategoryDel = wuList[0];
                                                    }
                                                }

                                                //set first organizational unit to default one
                                                if (employeeAsco4TO.IntegerValue3 == syncResp.UnitID)
                                                    employeeAsco4TO.IntegerValue3 = defaultUnitID;
                                            }
                                        }
                                    } //NEW RECORD
                                    else
                                    {
                                        if (syncResp.ValidTo.Date <= DateTime.Now.Date && syncResp.ValidTo.Date != new DateTime())
                                        {
                                            error = "Record not found and valid to date passed for rrec_id = " + syncResp.RecID;
                                        }
                                        else
                                        {
                                            respTOInsert = new EmployeeResponsibilityTO();
                                            respTOInsert = emplRespons.ResponsibilityTO;

                                            // new responsibility for working unit
                                            if (respTOInsert.Type == Constants.emplResTypeWU)
                                            {
                                                // check if there already exists supervisor category and insert if it does not exist
                                                applUserXCategory.UserXCategoryTO = new ApplUserXApplUserCategoryTO();
                                                applUserXCategory.UserXCategoryTO.CategoryID = -1;
                                                applUserXCategory.UserXCategoryTO.UserID = employeeAsco4TO.NVarcharValue5;
                                                List<ApplUserXApplUserCategoryTO> wuList = applUserXCategory.Search();
                                                bool defaultExist = false;
                                                bool insertCategory = true;
                                                foreach (ApplUserXApplUserCategoryTO category in wuList)
                                                {
                                                    if (category.DefaultCategory == Constants.yesInt)
                                                        defaultExist = true;
                                                    if (category.CategoryID == (int)Constants.UserCategoriesFIAT.Supervisor)
                                                        insertCategory = false;
                                                }

                                                if (insertCategory)
                                                {
                                                    applUsersXCategoryInsert.CategoryID = (int)Constants.UserCategoriesFIAT.Supervisor;
                                                    if (defaultExist)
                                                        applUsersXCategoryInsert.DefaultCategory = Constants.noInt;
                                                    else
                                                        applUsersXCategoryInsert.DefaultCategory = Constants.yesInt;
                                                    applUsersXCategoryInsert.UserID = employeeAsco4TO.NVarcharValue5;
                                                }

                                                // check if employee has visibility under cost center of responsibility wu and all children
                                                // if any visibility does not exists, add it                                                
                                                // get all visibilities for user
                                                applUserXwu.AuXWUnitTO = new ApplUsersXWUTO();
                                                Dictionary<int, WorkingUnitTO> userUnits = applUserXwu.FindWUForUserDictionary(employeeAsco4TO.NVarcharValue5, Constants.EmployeesPurpose);
                                                                                                                                              
                                                List<int> visibleUnitID = new List<int>();
                                                
                                                // get responsible working unit
                                                WorkingUnitTO wuTO = new WorkingUnitTO();
                                                if (wuDict.ContainsKey(syncResp.UnitID))
                                                    wuTO = wuDict[syncResp.UnitID];

                                                // get company
                                                int rootID = Common.Misc.getRootWorkingUnit(wuTO.WorkingUnitID, wuDict);

                                                // get wu cost center and all children  
                                                string costCenterCode = "";
                                                if (wuTO.Name.Length >= Constants.fsPlantCodeLenght + 1 + Constants.fsCostCenterCodeLenght)
                                                    costCenterCode = wuTO.Name.Substring(0, Constants.fsCostCenterCodeLenght + Constants.fsPlantCodeLenght + 1);

                                                workingUnit.WUTO = new WorkingUnitTO();
                                                workingUnit.WUTO.Name = costCenterCode;
                                                List<WorkingUnitTO> CostCenterList = workingUnit.SearchExact();
                                                WorkingUnitTO costCenterWU = new WorkingUnitTO();
                                                if (CostCenterList.Count > 0)
                                                {
                                                    foreach (WorkingUnitTO wu in CostCenterList)
                                                    {
                                                        int ID = Common.Misc.getRootWorkingUnit(wu.WorkingUnitID, wuDict);
                                                        if (rootID == ID)
                                                        {
                                                            costCenterWU = wu;
                                                        }
                                                    }
                                                }

                                                List<WorkingUnitTO> workingUnits = new List<WorkingUnitTO>();

                                                if (costCenterWU.WorkingUnitID != -1)
                                                {
                                                    workingUnits.Add(costCenterWU);
                                                    workingUnits = new WorkingUnit(ACTAConnection).FindAllChildren(workingUnits);

                                                    foreach (WorkingUnitTO unitTO in workingUnits)
                                                    {
                                                        if (!visibleUnitID.Contains(unitTO.WorkingUnitID) && !userUnits.ContainsKey(unitTO.WorkingUnitID))
                                                        {
                                                            ApplUsersXWUTO userWU = new ApplUsersXWUTO();
                                                            userWU.WorkingUnitID = unitTO.WorkingUnitID;
                                                            userWU.UserID = employeeAsco4TO.NVarcharValue5;
                                                            userWU.Purpose = Constants.EmployeesPurpose;
                                                            applUsersXWUInsert.Add(userWU);
                                                            visibleUnitID.Add(unitTO.WorkingUnitID);
                                                        }
                                                    }
                                                }
                                                
                                                // if there is no default working unit, set it
                                                if (employeeAsco4TO.IntegerValue2 == -1)
                                                    employeeAsco4TO.IntegerValue2 = syncResp.UnitID;
                                            }
                                            // new responsibility for organizational unit
                                            else if (respTOInsert.Type == Constants.emplResTypeOU)
                                            {
                                                // check if there already exists wc direct responsible category and insert if it does not exist
                                                applUserXCategory.UserXCategoryTO = new ApplUserXApplUserCategoryTO();
                                                applUserXCategory.UserXCategoryTO.CategoryID = -1;
                                                applUserXCategory.UserXCategoryTO.UserID = employeeAsco4TO.NVarcharValue5;
                                                List<ApplUserXApplUserCategoryTO> wuList = applUserXCategory.Search();
                                                bool defaultExst = false;
                                                bool insertDirectResp = true;
                                                foreach (ApplUserXApplUserCategoryTO category in wuList)
                                                {
                                                    if (category.DefaultCategory == Constants.yesInt)
                                                        defaultExst = true;
                                                    if (category.CategoryID == (int)Constants.UserCategoriesFIAT.WCDirectRisponsible)
                                                        insertDirectResp = false;
                                                }

                                                if (insertDirectResp)
                                                {
                                                    applUsersXCategoryInsert.CategoryID = (int)Constants.UserCategoriesFIAT.WCDirectRisponsible;
                                                    if (defaultExst)
                                                        applUsersXCategoryInsert.DefaultCategory = Constants.noInt;
                                                    else
                                                        applUsersXCategoryInsert.DefaultCategory = Constants.yesInt;
                                                    applUsersXCategoryInsert.UserID = employeeAsco4TO.NVarcharValue5;
                                                }

                                                // check if employee has visibility under responsibility ou and all children
                                                // if any visibility does not exists, add it                                                
                                                // get all visibilities for user
                                                applUserXou.AuXOUnitTO = new ApplUserXOrgUnitTO();
                                                Dictionary<int, OrganizationalUnitTO> userUnits = applUserXou.FindOUForUserDictionary(employeeAsco4TO.NVarcharValue5, Constants.EmployeesPurpose);

                                                List<int> visibleUnitID = new List<int>();

                                                // get responsible working unit
                                                OrganizationalUnitTO ouTO = new OrganizationalUnitTO();
                                                if (ouDict.ContainsKey(syncResp.UnitID))
                                                    ouTO = ouDict[syncResp.UnitID];

                                                if (ouTO.OrgUnitID != -1)
                                                {
                                                    List<OrganizationalUnitTO> orgUnitsTOSee = new List<OrganizationalUnitTO>();
                                                    orgUnitsTOSee.Add(ouTO);
                                                    orgUnitsTOSee = new OrganizationalUnit(ACTAConnection).FindAllChildren(orgUnitsTOSee);

                                                    foreach (OrganizationalUnitTO unit in orgUnitsTOSee)
                                                    {
                                                        if (!visibleUnitID.Contains(unit.OrgUnitID) && !userUnits.ContainsKey(unit.OrgUnitID))
                                                        {
                                                            ApplUserXOrgUnitTO userUnit = new ApplUserXOrgUnitTO();
                                                            userUnit.UserID = employeeAsco4TO.NVarcharValue5;
                                                            userUnit.OrgUnitID = unit.OrgUnitID;
                                                            userUnit.Purpose = Constants.EmployeesPurpose;
                                                            applUsersXOUInsert.Add(userUnit);
                                                            visibleUnitID.Add(userUnit.OrgUnitID);
                                                        }
                                                    }
                                                }

                                                // if there is no default organizational unit, set it
                                                if (employeeAsco4TO.IntegerValue3 == -1)
                                                    employeeAsco4TO.IntegerValue3 = syncResp.UnitID;
                                            }
                                        }
                                    }
                                }
                            }

                            //do inserts, updates,deletes in ACTA DB
                            //begin ACTA transaction
                            bool trans = emplRespons.BeginTransaction();
                            if (trans)
                            {
                                try
                                {
                                    employeeAsco4.EmplAsco4TO = employeeAsco4TO;
                                    bool succ = true;
                                    if (error == "")
                                    {
                                        employeeAsco4.SetTransaction(emplRespons.GetTransaction());
                                        if (respTOInsert.UnitID != -1)
                                        {
                                            emplRespons.ResponsibilityTO = respTOInsert;
                                            succ =  succ && emplRespons.insert(false);
                                        }
                                        else if (respTODel.UnitID != -1)
                                        {
                                            emplRespons.ResponsibilityTO = respTODel;
                                            succ = succ && emplRespons.delete(false);
                                        }

                                        succ = succ && employeeAsco4.update(false);

                                        applUserXou.AuXOUnitTO = new ApplUserXOrgUnitTO();
                                        applUserXou.SetTransaction(emplRespons.GetTransaction());
                                        if (applUsersXOUDelete.Count > 0)
                                        {                                            
                                            foreach (ApplUserXOrgUnitTO applUserXOUTO in applUsersXOUDelete)
                                            {
                                                succ = succ && applUserXou.Delete(applUserXOUTO.UserID, applUserXOUTO.OrgUnitID, Constants.EmployeesPurpose, false);
                                            }
                                        }

                                        applUserXwu.SetTransaction(emplRespons.GetTransaction());
                                        if (applUsersXWUDelete.Count > 0)
                                        {                                            
                                            foreach (ApplUsersXWUTO applUserXWUTO in applUsersXWUDelete)
                                            {
                                                succ = succ && applUserXwu.Delete(applUserXWUTO.UserID, applUserXWUTO.WorkingUnitID, Constants.EmployeesPurpose, false);
                                            }
                                        }

                                        if (applUsersXOUInsert.Count > 0)
                                        {                                                                                   
                                            foreach (ApplUserXOrgUnitTO applUserXOUTO in applUsersXOUInsert)
                                            {
                                                succ = succ && applUserXou.Save(applUserXOUTO.UserID, applUserXOUTO.OrgUnitID, Constants.EmployeesPurpose, false) > 0;
                                            }
                                        }
                                        if (applUsersXWUInsert.Count > 0)
                                        {                                            
                                            foreach (ApplUsersXWUTO applUserXWUTO in applUsersXWUInsert)
                                            {
                                                succ = succ && applUserXwu.Save(applUserXWUTO.UserID, applUserXWUTO.WorkingUnitID, applUserXWUTO.Purpose, false) > 0;
                                            }
                                        }

                                        applUserXCategory.SetTransaction(emplRespons.GetTransaction());
                                        if (applUsersXCategoryInsert.CategoryID != -1)
                                        {
                                            applUserXCategory.UserXCategoryTO = applUsersXCategoryInsert;
                                            succ = succ && applUserXCategory.Save(false) > 0;
                                        }

                                        if (applUsersXCategoryDel.CategoryID != -1)
                                        {
                                            succ = succ && applUserXCategory.Delete(applUsersXCategoryDel.CategoryID, applUsersXCategoryDel.UserID, false);
                                        }
                                    }
                                    if (succ)
                                    {
                                        syncResponse.SetTransaction(emplRespons.GetTransaction());
                                        syncResponse.ResponsibilityTO = syncResp;
                                        if (error == "")
                                            syncResponse.ResponsibilityTO.Result = Constants.resultSucc;
                                        else
                                        {
                                            WriteLog(" SyncResponsibilityData - " + error);
                                            syncResponse.ResponsibilityTO.Result = Constants.resultFaild;
                                            syncResponse.ResponsibilityTO.Remark = error;
                                        }
                                        succ = succ && syncResponse.insert(false);
                                        if (succ)
                                        {
                                            //do not del if valid to date not passed
                                            if (delRecord)
                                            {
                                                //before commit ACTA transaction try to delete record from sync DB
                                                succ = syncDAO.delSyncResponsibility(syncResp);
                                            }
                                            if (succ)
                                            {
                                                emplRespons.CommitTransaction();
                                            }
                                            else
                                            {
                                                emplRespons.RollbackTransaction();
                                                WriteLog(" SyncResponsibilityData() Delete record from sync_organizational_structure fail for rec_id = " + syncResp.RecID.ToString());
                                            }
                                        }
                                        else
                                        {
                                            emplRespons.RollbackTransaction();
                                        }
                                    }
                                    if (!succ)
                                    {
                                        if (emplRespons.GetTransaction() != null)
                                            emplRespons.RollbackTransaction();

                                        WriteLog(" SyncResponsibilityData() ACTA DB record insert faild without exception fpr rec_id = " + syncResp.RecID.ToString());
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (emplRespons.GetTransaction() != null)
                                        emplRespons.RollbackTransaction();
                                    WriteLog(" SyncResponsibilityData() exception: " + ex.Message + "; for rec_id = " + syncResp.RecID.ToString());
                                }
                            }
                            else
                            {
                                WriteLog(" SyncResponsibilityData() ACTA DB begin transaction faild! ");
                            }

                        }
                        catch (Exception ex)
                        {
                            WriteLog(" SyncResponsibilityData() exception: " + ex.Message + "; for rec_id = " + syncResp.RecID);
                        }
                    }
                }            
            }
            catch (Exception ex)
            {
                WriteLog(" SyncResponsibilityData() exception: " + ex.Message);
            }
        }

        private void SyncBufferData()
        {
            try
            {
                //list to insert to sync table
                List<SyncBufferDataTO> bufferRecords = new List<SyncBufferDataTO>();

                SyncBufferDataHist bufferHist = new SyncBufferDataHist(ACTAConnection);
              
                //get from history table last date sync
                DateTime lastSyncData =  bufferHist.GetMaxDate();
                
                //find new counter values
                EmployeeCounterValue counterValue = new EmployeeCounterValue(ACTAConnection);
                List<EmployeeCounterValueTO> newCounterValues = counterValue.SearchModifiedValues(lastSyncData);

                foreach (EmployeeCounterValueTO value in newCounterValues)
                {
                    if (value.EmplCounterTypeID == (int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter)
                    {
                        try
                        {
                            //find values for this and previous year
                            EmployeeCounterValueTO thisYearValue = counterValue.Find((int)Constants.EmplCounterTypes.AnnualLeaveCounter, value.EmplID);
                            EmployeeCounterValueTO prevYearValue = counterValue.Find((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter, value.EmplID);
                            
                            // change values to show days of previous/current year annual leave left
                            if (prevYearValue.Value >= value.Value)
                                prevYearValue.Value = prevYearValue.Value - value.Value;
                            else
                            {
                                thisYearValue.Value = thisYearValue.Value + prevYearValue.Value - value.Value;
                                prevYearValue.Value = 0;
                            }

                            SyncBufferDataTO data = new SyncBufferDataTO();
                            data.EmployeeID = value.EmplID;
                            data.DaysOfHolidayForPreviousYearLeft = prevYearValue.Value;
                            data.DaysOfHolidayForCurrentYearLeft = thisYearValue.Value;
                            data.CreatedBy = Constants.syncUser;
                            data.CreatedTime = DateTime.Now;
                            data.RecID = syncDAO.insertSyncBufferData(data);
                            if (data.RecID >= 0)
                            {
                                bufferHist.BDTO = data;
                                bool succ = bufferHist.insert(true);
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog(" SyncBufferData() exception: " + ex.Message+"; for employee_id = "+value.EmplID+" and counter_type_id = "+value.EmplCounterTypeID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(" SyncBufferData() exception: " + ex.Message);
            }
        }

        private void SyncEmployees()
        {
            try
            {
                //get all records from sync_employees
                List<SyncEmployeeTO> emplRecords = syncDAO.getDifEmployees();

                //if there is no records continue
                if (emplRecords.Count <= 0)
                {
                    WriteLog(" SyncEmployees - Records not found. ");
                }
                //if records found begin processing
                else
                {
                    WriteLog(" SyncEmployees - Num of records found: " + emplRecords.Count.ToString());

                    // create all necessary Common objects
                    Employee employee = new Employee(ACTAConnection);
                    EmployeeHist emplHist = new EmployeeHist(ACTAConnection);
                    EmployeeAsco4Hist emplAscoHist = new EmployeeAsco4Hist(ACTAConnection);
                    EmployeeAsco4 employeeAsco4 = new EmployeeAsco4(ACTAConnection);
                    EmployeeImageFile eif = new EmployeeImageFile(ACTAConnection);
                    Tag tag = new Tag(ACTAConnection);
                    WorkingUnit workingUnit = new WorkingUnit(ACTAConnection);
                    ApplUser applUser = new ApplUser(ACTAConnection);
                    OrganizationalUnit orgUnit = new OrganizationalUnit(ACTAConnection);
                    EmployeeCounterValue emplCounter = new EmployeeCounterValue(ACTAConnection);
                    EmployeeCounterType emplCounterType = new EmployeeCounterType(ACTAConnection);
                    EmployeeCounterValueHist emplCounterHist = new EmployeeCounterValueHist(ACTAConnection);
                    ApplUserXOrgUnit applUserXOrgUnit = new ApplUserXOrgUnit(ACTAConnection);
                    ApplUsersXWU applUserXWU = new ApplUsersXWU(ACTAConnection);
                    SyncEmployeeHist syncEmpl = new SyncEmployeeHist(ACTAConnection);
                    ApplUserXApplUserCategory applUserXCategory = new ApplUserXApplUserCategory(ACTAConnection);
                    IOPair ioPair = new IOPair(ACTAConnection);
                    Log logObj = new Log(ACTAConnection);
                    EmployeesTimeSchedule emplTimeSchedule = new EmployeesTimeSchedule(ACTAConnection);
                    TimeSchema ts = new TimeSchema(ACTAConnection);
                    EmployeePosition emplPosition = new EmployeePosition(ACTAConnection);
                    EmployeeXRisk emplXRisk = new EmployeeXRisk(ACTAConnection);
                    EmployeePositionXRisk posXRisk = new EmployeePositionXRisk(ACTAConnection);

                    // get minimal day allowed to reprocess
                    DateTime minReprocessedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date.AddMonths(-1);
                    // get max HRSSC date
                    int maxHrssc = 0;
                    List<int> hrsscDates = new Common.Rule(ACTAConnection).SearchRulesExact(Constants.RuleHRSSCCutOffDate);
                    foreach (int value in hrsscDates)
                    {
                        if (value > maxHrssc)
                            maxHrssc = value;
                    }

                    if (Common.Misc.countWorkingDays(DateTime.Now, ACTAConnection) > maxHrssc)
                        minReprocessedDate = minReprocessedDate.AddMonths(1).Date;

                    //rules dictionary first key is Working Unit ID, secund key is Employee Type ID, third key is ryle_Type; value is RuleTO
                    Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDictionary = new Rule(ACTAConnection).SearchWUEmplTypeDictionary();

                    //get all time Schemas
                    Dictionary<int, WorkTimeSchemaTO> dictTimeSchema = ts.getDictionary();

                    // get all working units
                    Dictionary<int, WorkingUnitTO> wuDict = workingUnit.getWUDictionary();

                    // get old employees and employee's asco values for synchronization arrived employees
                    string emplIDs = "";
                    foreach (SyncEmployeeTO syncEmplTO in emplRecords)
                    {
                        emplIDs += syncEmplTO.EmployeeID.ToString().Trim() + ",";
                    }

                    if (emplIDs.Trim().Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                    int countInt = 0;
                    bool tagChanged = false;
                    
                    //process records one by one
                    foreach (SyncEmployeeTO syncEmplTO in emplRecords)
                    {
                        countInt++;

                        SetManagerThreadStatus("Synchronization Employees " + countInt.ToString() + "/" + emplRecords.Count.ToString());

                        try
                        {
                            // do not process if valid from is future date
                            if (syncEmplTO.ValidFrom.Date > DateTime.Now.Date || syncEmplTO.ValidFrom.Equals(new DateTime()))
                                continue;
                            
                            // get old employee from db if exists
                            employee.EmplTO = new EmployeeTO();                            
                            EmployeeTO employeeTO = employee.Find(syncEmplTO.EmployeeID.ToString().Trim());
                             
                            // get old employee asco data if exists
                            employeeAsco4.EmplAsco4TO = new EmployeeAsco4TO();
                            List<EmployeeAsco4TO> ascoList = employeeAsco4.Search(syncEmplTO.EmployeeID.ToString().Trim());

                            EmployeeAsco4TO employeeAsco4TO = new EmployeeAsco4TO();
                            if (ascoList.Count > 0)
                                employeeAsco4TO = ascoList[0];

                            if (employeeAsco4TO.EmployeeID != -1)                                                            
                                emplAscoHist.EmplAsco4TO = new EmployeeAsco4TO(employeeAsco4TO);                            
                            else
                                employeeAsco4TO.EmployeeID = syncEmplTO.EmployeeID;

                            //objects for update insert and delete from ACTA DB                                                       
                            EmployeeTO employeeHistTO = new EmployeeTO();                            
                            TagTO tagUpdate = new TagTO();
                            TagTO tagInsert = new TagTO();
                            List<ApplUserTO> applUsersTOUpdate = new List<ApplUserTO>();
                            ApplUserTO userToInsert = new ApplUserTO();
                            List<ApplUserXApplUserCategoryTO> userXCategoriesTOInsert = new List<ApplUserXApplUserCategoryTO>();
                            List<ApplUserXOrgUnitTO> applUsersXOUInsert = new List<ApplUserXOrgUnitTO>();                            
                            List<ApplUsersXWUTO> applUsersXWUInsert = new List<ApplUsersXWUTO>();                            
                            List<EmployeeCounterValueTO> newCounter = new List<EmployeeCounterValueTO>();
                            List<EmployeeCounterValueTO> updateCounter = new List<EmployeeCounterValueTO>();
                            List<EmployeeCounterValueTO> histCounter = new List<EmployeeCounterValueTO>();
                            List<LogTO> logsTOReprocess = new List<LogTO>();
                            List<EmployeeTimeScheduleTO> emplTimeScheduleInsert = new List<EmployeeTimeScheduleTO>();
                            List<EmployeeTimeScheduleTO> emplTimeSchedulesDel = new List<EmployeeTimeScheduleTO>();
                            List<EmployeeXRiskTO> emplRisksUpdate = new List<EmployeeXRiskTO>();
                            List<EmployeeXRiskTO> emplRisksInsert = new List<EmployeeXRiskTO>();
                            
                            string oldUserName = "";
                            bool emplExist = false;

                            if (employeeTO.EmployeeID >= 0)                            
                            {
                                emplExist = true;
                                employeeHistTO = new EmployeeTO(employeeTO);
                            }
                            else
                            {
                                employeeTO.EmployeeID = syncEmplTO.EmployeeID;
                                employeeTO.WorkingGroupID = 0;
                            }

                            //if we need to reprocess all passes if employee change type or company
                            bool reprocessData = false;
                            bool reprocessLog = false;
                            
                            //if we need to delete all pairs processed in day and to reprocess day
                            bool reprocessDays = false;
                            List<DateTime> dayToReprocess = new List<DateTime>();
                            DateTime reprocressDateStart = new DateTime();
                            bool dataChange = false;
                            string error = "";
                            bool setDefaultShift = false;

                            //update just parameter for employee_id_old
                            if (syncEmplTO.EmployeeIDOld != -1)
                            {
                                dataChange = true;
                                employeeAsco4TO.IntegerValue1 = syncEmplTO.EmployeeIDOld;
                            }

                            //update just parameter for first_name
                            if (syncEmplTO.FirstName != "" && syncEmplTO.FirstName != Constants.syncStringNullValue)
                            {
                                dataChange = true;
                                employeeTO.FirstName = syncEmplTO.FirstName;
                            }
                            else if (!emplExist)
                            {
                                error = "Employee column first_name can not be null";
                            }

                            //update just parameter for last_name
                            if (syncEmplTO.LastName != "" && syncEmplTO.LastName != Constants.syncStringNullValue)
                            {
                                dataChange = true;
                                employeeTO.LastName = syncEmplTO.LastName;
                            }
                            else if (!emplExist)
                            {
                                error = "Employee column last_name can not be null";
                            }

                            //update just parameter for picture, later insert it to image table
                            if (syncEmplTO.Picture != null)
                            {
                                dataChange = true;
                                string newFileName = employeeTO.EmployeeID.ToString().Trim() + ".jpg";
                                employeeTO.Picture = newFileName;
                            }

                            //update just parameter for JMBG
                            if (syncEmplTO.JMBG != "")
                            {
                                dataChange = true;
                                if (syncEmplTO.JMBG == Constants.syncStringNullValue)
                                {
                                    employeeAsco4TO.NVarcharValue4 = "";
                                }
                                else
                                {
                                    employeeAsco4TO.NVarcharValue4 = syncEmplTO.JMBG;
                                }
                            }

                            //update just parameter for email adress
                            if (syncEmplTO.EmailAddress != "")
                            {
                                dataChange = true;
                                if (syncEmplTO.EmailAddress == Constants.syncStringNullValue)
                                {
                                    employeeAsco4TO.NVarcharValue3 = "";
                                }
                                else
                                {
                                    employeeAsco4TO.NVarcharValue3 = syncEmplTO.EmailAddress;
                                }
                            }

                            //update just parameter for work location id
                            if (syncEmplTO.WorkLocationID != -1)
                            {
                                dataChange = true;
                                if (syncEmplTO.WorkLocationID == Constants.syncIntNullValue)
                                {
                                    employeeAsco4TO.IntegerValue5 = -1;
                                }
                                else
                                {
                                    employeeAsco4TO.IntegerValue5 = syncEmplTO.WorkLocationID;
                                }
                            }

                            //update just parameter for work location code
                            if (syncEmplTO.WorkLocationCode != "")
                            {
                                dataChange = true;
                                if (syncEmplTO.WorkLocationCode == Constants.syncStringNullValue)
                                {
                                    employeeAsco4TO.NVarcharValue7 = "";
                                }
                                else
                                {
                                    employeeAsco4TO.NVarcharValue7 = syncEmplTO.WorkLocationCode;
                                }
                            }

                            // update just parameter for starting using date annual leave
                            if (!syncEmplTO.AnnualLeaveStartDate.Equals(new DateTime()))
                            {
                                dataChange = true;
                                employeeAsco4TO.DatetimeValue4 = syncEmplTO.AnnualLeaveStartDate;
                            }

                            // update just parameter for address
                            if (!syncEmplTO.Address.Trim().Equals(""))
                            {
                                dataChange = true;

                                if (syncEmplTO.Address.Trim().ToUpper().Equals(Constants.syncStringNullValue.Trim().ToUpper()))
                                    employeeAsco4TO.NVarcharValue8 = "";
                                else
                                    employeeAsco4TO.NVarcharValue8 = syncEmplTO.Address.Trim();
                            }

                            // update just parameter for phone 1
                            if (!syncEmplTO.PhoneNumber1.Trim().Equals(""))
                            {
                                dataChange = true;

                                if (syncEmplTO.PhoneNumber1.Trim().ToUpper().Equals(Constants.syncStringNullValue.Trim().ToUpper()))
                                    employeeAsco4TO.NVarcharValue9 = "";
                                else
                                    employeeAsco4TO.NVarcharValue9 = syncEmplTO.PhoneNumber1.Trim();
                            }

                            // update just parameter for phone 2
                            if (!syncEmplTO.PhoneNumber2.Trim().Equals(""))
                            {
                                dataChange = true;

                                if (syncEmplTO.PhoneNumber2.Trim().ToUpper().Equals(Constants.syncStringNullValue.Trim().ToUpper()))
                                    employeeAsco4TO.NVarcharValue10 = "";
                                else
                                    employeeAsco4TO.NVarcharValue10 = syncEmplTO.PhoneNumber2;
                            }

                            // update just parameter for date of birth
                            if (!syncEmplTO.DateOfBirth.Equals(new DateTime()))
                            {
                                dataChange = true;
                                                                
                                employeeAsco4TO.DatetimeValue5 = syncEmplTO.DateOfBirth;
                            }
                            
                            //update  branch and employee stringone if employee exist; if it is new employee it will be created on after update working unit id 
                            if (syncEmplTO.EmployeeBranch != "")
                            {
                                dataChange = true;
                                if (syncEmplTO.EmployeeBranch == Constants.syncStringNullValue)
                                {
                                    employeeAsco4TO.NVarcharValue6 = "";
                                }
                                else
                                {
                                    employeeAsco4TO.NVarcharValue6 = syncEmplTO.EmployeeBranch;
                                }

                                if (employeeAsco4TO.NVarcharValue2.Length > 0)
                                {
                                    employeeAsco4TO.NVarcharValue2 = employeeAsco4TO.NVarcharValue2.Substring(0, employeeAsco4TO.NVarcharValue2.Length - 1) + employeeAsco4TO.NVarcharValue6;
                                }
                            }

                            //update status for employee, tag and applUser if exists
                            if (syncEmplTO.Status != "" && syncEmplTO.Status != Constants.syncStringNullValue)
                            {
                                dataChange = true;                               

                                // currently active employee is retired
                                //if (employeeTO.Status == Constants.statusActive && syncEmplTO.Status == Constants.statusRetired)
                                if (syncEmplTO.Status == Constants.statusRetired)
                                {
                                    if (employeeAsco4TO.NVarcharValue5 != "")
                                    {
                                        applUser.UserTO = new ApplUserTO();
                                        applUser.UserTO.UserID = employeeAsco4TO.NVarcharValue5;
                                        applUser.UserTO.Status = Constants.statusActive;
                                        List<ApplUserTO> usersTO = applUser.Search();
                                        if (usersTO.Count > 0)
                                        {
                                            ApplUserTO applUserTOUpdate = usersTO[0];
                                            applUserTOUpdate.Status = Constants.statusRetired;
                                            applUsersTOUpdate.Add(applUserTOUpdate);
                                        }
                                    }

                                    tag.TgTO = new TagTO();
                                    tag.TgTO.OwnerID = syncEmplTO.EmployeeID;
                                    tag.TgTO.Status = Constants.statusActive;
                                    List<TagTO> tagList = tag.Search();
                                    if (tagList.Count > 0)
                                    {
                                        tagUpdate = tagList[0];
                                        tagUpdate.Status = Constants.statusRetired;
                                        tagChanged = true;
                                    }

                                    //set time schedule to default and delete all io_pairs_processed   
                                    // if last working day is entering third shift, set leaving third shift for first retired day and leave pairs for that day, and set default schema to second retired day
                                    emplTimeSchedulesDel = emplTimeSchedule.SearchEmployeesSchedules(syncEmplTO.EmployeeID.ToString(), syncEmplTO.ValidFrom.Date, new DateTime());
                                    bool firstRetiredDayThirdShiftLeaving = false;
                                    WorkTimeSchemaTO retiredDaySchema = new WorkTimeSchemaTO();
                                    // get intervals for last working day
                                    Dictionary<int, List<EmployeeTimeScheduleTO>> lastWorkingSchList = emplTimeSchedule.SearchEmployeesSchedulesExactDate(syncEmplTO.EmployeeID.ToString().Trim(), 
                                        syncEmplTO.ValidFrom.Date.AddDays(-1), syncEmplTO.ValidFrom.Date.AddDays(-1), null);

                                    if (lastWorkingSchList.ContainsKey(syncEmplTO.EmployeeID) && lastWorkingSchList[syncEmplTO.EmployeeID].Count > 0)
                                    {
                                        List<WorkTimeIntervalTO> lastDayIntervals = Common.Misc.getTimeSchemaInterval(syncEmplTO.ValidFrom.AddDays(-1), lastWorkingSchList[syncEmplTO.EmployeeID], dictTimeSchema);

                                        // check if last schedule has midnight ending interval                                        
                                        foreach (WorkTimeIntervalTO intTO in lastDayIntervals)
                                        {
                                            if (intTO.EndTime.Hour == 23 && intTO.EndTime.Minute == 59 && intTO.EndTime.TimeOfDay.Subtract(intTO.StartTime.TimeOfDay).TotalMinutes > 0)
                                            {
                                                firstRetiredDayThirdShiftLeaving = true;

                                                if (dictTimeSchema.ContainsKey(intTO.TimeSchemaID))
                                                    retiredDaySchema = dictTimeSchema[intTO.TimeSchemaID];
                                                break;
                                            }
                                        }
                                    }

                                    if (firstRetiredDayThirdShiftLeaving)
                                    {
                                        // get ending third shift day and insert that schedule into first retired day
                                        // after that insert default schema
                                        int startSchemaDay = -1;
                                        foreach (int day in retiredDaySchema.Days.Keys)
                                        {
                                            if (retiredDaySchema.Days[day].Count == 1)
                                            {
                                                foreach (WorkTimeIntervalTO intTO in retiredDaySchema.Days[day].Values)
                                                {
                                                    if (intTO.StartTime.Hour == 0 && intTO.StartTime.Minute == 0 && intTO.EndTime.TimeOfDay.Subtract(intTO.StartTime.TimeOfDay).TotalMinutes > 0)
                                                    {
                                                        startSchemaDay = day;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (startSchemaDay != -1)
                                                break;
                                        }

                                        if (retiredDaySchema.TimeSchemaID != -1 && startSchemaDay != -1)
                                        {
                                            EmployeeTimeScheduleTO etsRetiredInsert = new EmployeeTimeScheduleTO();
                                            etsRetiredInsert.EmployeeID = syncEmplTO.EmployeeID;
                                            etsRetiredInsert.StartCycleDay = startSchemaDay;
                                            etsRetiredInsert.Date = syncEmplTO.ValidFrom.Date;
                                            etsRetiredInsert.TimeSchemaID = retiredDaySchema.TimeSchemaID;
                                            emplTimeScheduleInsert.Add(etsRetiredInsert);
                                        }
                                    }

                                    EmployeeTimeScheduleTO etsInsert = new EmployeeTimeScheduleTO();
                                    etsInsert.EmployeeID = syncEmplTO.EmployeeID;
                                    etsInsert.StartCycleDay = 0;
                                    if (firstRetiredDayThirdShiftLeaving)
                                        etsInsert.Date = syncEmplTO.ValidFrom.Date.AddDays(1);
                                    else
                                        etsInsert.Date = syncEmplTO.ValidFrom.Date;
                                    etsInsert.TimeSchemaID = Constants.defaultSchemaID;
                                    emplTimeScheduleInsert.Add(etsInsert);
                                   
                                    DateTime endDate = new IOPairProcessed(ACTAConnection).getMaxDateOfPair(employeeTO.EmployeeID.ToString(), employee.GetTransaction());
                                    if (endDate == new DateTime())
                                        endDate = DateTime.Now.Date;
                                    
                                    List<DateTime> datesList = new List<DateTime>();
                                    DateTime startReprocessDay = firstRetiredDayThirdShiftLeaving ? syncEmplTO.ValidFrom.Date.AddDays(1) : syncEmplTO.ValidFrom.Date;
                                    for (DateTime dt = startReprocessDay; dt <= endDate; dt = dt.AddDays(1))
                                    {
                                        datesList.Add(dt);
                                    }

                                    if (endDate >= syncEmplTO.ValidFrom.Date)
                                    {
                                        reprocessDays = true;
                                        dayToReprocess.AddRange(datesList);
                                    }
                                }
                                // cuurrently retired employee is activated again
                                else if (employeeTO.Status == Constants.statusRetired && syncEmplTO.Status == Constants.statusActive)
                                {
                                    if (employeeAsco4TO.NVarcharValue5 != "")
                                    {
                                        applUser.UserTO = new ApplUserTO();
                                        applUser.UserTO.UserID = employeeAsco4TO.NVarcharValue5;
                                        applUser.UserTO.Status = Constants.statusRetired;
                                        List<ApplUserTO> usersTO = applUser.Search();
                                        if (usersTO.Count > 0)
                                        {
                                            ApplUserTO applUserTOUpdate = usersTO[0];
                                            applUserTOUpdate.Status = Constants.statusActive;
                                            applUsersTOUpdate.Add(applUserTOUpdate);
                                        }
                                    }

                                    tag.TgTO = new TagTO();
                                    tag.TgTO.OwnerID = syncEmplTO.EmployeeID;
                                    tag.TgTO.Status = Constants.statusRetired;
                                    List<TagTO> tagList = tag.Search();
                                    if (tagList.Count > 0)
                                    {
                                        tagUpdate = tagList[0];
                                        tagUpdate.Status = Constants.statusActive;
                                        tagChanged = true;

                                        reprocessLog = true;
                                        logObj.LgTO = new LogTO();
                                        logObj.LgTO.TagID = tagUpdate.TagID;
                                        logsTOReprocess = logObj.SearchForLogPeriod(syncEmplTO.ValidFrom.Date, DateTime.Now.Date);
                                    }

                                    // reprocess days if neaded
                                    DateTime endDate = new IOPairProcessed(ACTAConnection).getMaxDateOfPair(employeeTO.EmployeeID.ToString(), employee.GetTransaction());

                                    if (endDate.Date >= syncEmplTO.ValidFrom.Date)
                                    {
                                        List<DateTime> datesList = new List<DateTime>();
                                        for (DateTime dt = syncEmplTO.ValidFrom.Date; dt <= endDate; dt = dt.AddDays(1))
                                        {
                                            datesList.Add(dt);
                                        }                                        
                                        
                                        reprocessDays = true;
                                        dayToReprocess.AddRange(datesList);
                                    }

                                    // insert default group schedule from valid from day
                                    setDefaultShift = true;

                                    emplTimeSchedulesDel = emplTimeSchedule.SearchEmployeesSchedules(syncEmplTO.EmployeeID.ToString(), syncEmplTO.ValidFrom.Date, new DateTime());
                                }

                                employeeTO.Status = syncEmplTO.Status;

                                // if status is ACTIVE and employee is new, set hiring date in datetime_value_2
                                if (syncEmplTO.Status == Constants.statusActive)
                                {
                                    if (syncEmplTO.ValidFrom.Equals(new DateTime()))
                                        error = "Hiring date missing";
                                    else
                                    {
                                        employeeAsco4TO.DatetimeValue2 = syncEmplTO.ValidFrom.Date;
                                        employeeAsco4TO.DatetimeValue3 = new DateTime();
                                    }
                                }
                                else if (syncEmplTO.Status == Constants.statusRetired)
                                {
                                    // set retired date into asco table (datetime_value_3)
                                    if (syncEmplTO.ValidFrom.Equals(new DateTime()))
                                        error = "Termination date missing";
                                    else
                                        employeeAsco4TO.DatetimeValue3 = syncEmplTO.ValidFrom.Date;
                                }
                            }

                            //update just parameter for personal holiday category
                            if (syncEmplTO.PersonalHolidayCategory != -1)
                            {
                                dataChange = true;
                                if (syncEmplTO.PersonalHolidayCategory > Constants.holidayMaxCategory 
                                    || (syncEmplTO.PersonalHolidayCategory <= 0 && syncEmplTO.PersonalHolidayCategory != Constants.syncIntNullValue))
                                {
                                    error = "Unknown personal holiday category for employee id = " + employeeTO.EmployeeID + "; category = " + syncEmplTO.PersonalHolidayCategory;
                                }
                                else
                                {
                                    if (syncEmplTO.PersonalHolidayCategory == Constants.syncIntNullValue)
                                    {
                                        employeeAsco4TO.NVarcharValue1 = "";
                                    }
                                    else
                                    {
                                        employeeAsco4TO.NVarcharValue1 = syncEmplTO.PersonalHolidayCategory.ToString();
                                    }
                                }
                            }

                            //update just parameter for personal holiday date and reprocess data if needed
                            if (syncEmplTO.PersonalHolidayDate != new DateTime())
                            {
                                dataChange = true;
                                if (syncEmplTO.ValidFrom.Date <= new DateTime(DateTime.Now.Year, employeeAsco4TO.DatetimeValue1.Month, employeeAsco4TO.DatetimeValue1.Day))
                                {
                                    reprocessDays = true;
                                    dayToReprocess.Add(new DateTime(DateTime.Now.Year, employeeAsco4TO.DatetimeValue1.Month, employeeAsco4TO.DatetimeValue1.Day));
                                }
                                if (syncEmplTO.ValidFrom.Date <= new DateTime(DateTime.Now.Year, syncEmplTO.PersonalHolidayDate.Month, syncEmplTO.PersonalHolidayDate.Day))
                                {
                                    reprocessDays = true;
                                    dayToReprocess.Add(new DateTime(DateTime.Now.Year, syncEmplTO.PersonalHolidayDate.Month, syncEmplTO.PersonalHolidayDate.Day));
                                }
                                if (syncEmplTO.PersonalHolidayDate == Constants.syncDateTimeNullValue())
                                {
                                    employeeAsco4TO.DatetimeValue1 = new DateTime();
                                }
                                else
                                {
                                    employeeAsco4TO.DatetimeValue1 = syncEmplTO.PersonalHolidayDate;
                                }
                            }

                            //if delete tag or change set status on retired
                            if (syncEmplTO.DeleteTag == Constants.yesInt)
                            {                                
                                tag.TgTO.OwnerID = syncEmplTO.EmployeeID;
                                tag.TgTO.Status = Constants.statusActive;
                                List<TagTO> tagList = tag.Search();
                                if (tagList.Count > 0)
                                {
                                    dataChange = true;
                                    tagUpdate = tagList[0];
                                    tagUpdate.Status = Constants.statusRetired;
                                    tagChanged = true;
                                }
                            }

                            //update tag_id and reprocess data if needed
                            if (syncEmplTO.TagID != 0)
                            {
                                dataChange = true;
                                tag.TgTO = new TagTO();
                                tag.TgTO.OwnerID = syncEmplTO.EmployeeID;
                                tag.TgTO.Status = Constants.statusActive;
                                List<TagTO> tagList = tag.Search();
                                if (tagList.Count > 0)
                                {
                                    tagUpdate = tagList[0];
                                    tagUpdate.Status = Constants.statusRetired;
                                    tagChanged = true;
                                }

                                tag.TgTO = new TagTO();
                                tag.TgTO.TagID = syncEmplTO.TagID;
                                tag.TgTO.Status = Constants.statusActive;
                                tagList = tag.Search();
                                if (tagList.Count > 0)
                                {
                                    error = "Tag has to be unique. Tag id = " + syncEmplTO.TagID + " already is in ACTA DB for employee id = " + tagList[0].OwnerID;
                                }
                                else
                                {
                                    tagInsert.OwnerID = syncEmplTO.EmployeeID;
                                    tagInsert.TagID = syncEmplTO.TagID;
                                    tagInsert.Status = Constants.statusActive;
                                    tagChanged = true;
                                    if (tagUpdate.RecordID > 0)
                                    {
                                        reprocessLog = true;
                                        //logsTOReprocess = logObj.SearchForLogPeriod(syncEmplTO.TagIDValidFrom.Date,DateTime.Now.Date);  
                                        logObj.LgTO = new LogTO();
                                        logObj.LgTO.TagID = tagUpdate.TagID;
                                        logsTOReprocess = logObj.SearchForLogPeriod(syncEmplTO.ValidFrom.Date, DateTime.Now.Date);
                                    }
                                }
                            }

                            //update just parameter for organizational unit id
                            if (syncEmplTO.OrganizationalUnitID != -1)
                            {
                                dataChange = true;

                                bool orgChanged = false;
                                // if employee type is changed, organizational unit will be changed in type changing
                                if (syncEmplTO.EmployeeTypeID == -1)
                                {
                                    // move expat and task force to default org unit
                                    if ((employeeTO.EmployeeTypeID == (int)Constants.EmployeeTypesFIAT.Expat || employeeTO.EmployeeTypeID == (int)Constants.EmployeeTypesFIAT.TaskForce)
                                        && orgUnit.Find(Constants.defaultSYSOU).OrgUnitID != -1)
                                    {
                                        employeeTO.OrgUnitID = Constants.defaultSYSOU;
                                        orgChanged = true;
                                    }

                                    // move expat out to expat org unit
                                    if (employeeTO.EmployeeTypeID == (int)Constants.EmployeeTypesFIAT.ExpatOut && orgUnit.Find(Constants.expatOutOU).OrgUnitID != -1)
                                    {
                                        employeeTO.OrgUnitID = Constants.expatOutOU;
                                        orgChanged = true;
                                    }
                                }

                                if (!orgChanged)
                                {
                                    OrganizationalUnitTO unitTO = orgUnit.Find(syncEmplTO.OrganizationalUnitID);
                                    if (unitTO.OrgUnitID != -1)
                                    {
                                        employeeTO.OrgUnitID = syncEmplTO.OrganizationalUnitID;
                                    }
                                    else
                                    {
                                        error = "Organizational unit NOT found";
                                    }
                                }
                            }
                            else if (!emplExist)
                            {
                                error = "Employee column organizational_unit_id can not be null";
                            }

                            //update working unit id and if company changed reprocess data
                            if (syncEmplTO.FsUnitID > -1)
                            {
                                dataChange = true;
                                //WorkingUnitTO workingUnitTO = workingUnit.FindWU(syncEmplTO.FsUnitID);
                                if (!wuDict.ContainsKey(syncEmplTO.FsUnitID))
                                {
                                    error = "Working unit for employee not found! Working unit id = " + syncEmplTO.FsUnitID + "; for employee id = " + syncEmplTO.EmployeeID;
                                }
                                else
                                {
                                    WorkingUnitTO workingUnitOldTO = new WorkingUnitTO();
                                    if (wuDict.ContainsKey(employeeTO.WorkingUnitID))
                                        workingUnitOldTO = wuDict[employeeTO.WorkingUnitID];

                                    employeeTO.WorkingUnitID = syncEmplTO.FsUnitID;
                                    string branch = "";
                                    if (employeeAsco4TO.NVarcharValue6 != "")
                                        branch = employeeAsco4TO.NVarcharValue6;
                                    else if (syncEmplTO.EmployeeBranch != "")
                                        branch = syncEmplTO.EmployeeBranch;

                                    employeeAsco4TO.NVarcharValue2 = wuDict[syncEmplTO.FsUnitID].Name + "." + branch;
                                    
                                    int newCompany = Common.Misc.getRootWorkingUnit(syncEmplTO.FsUnitID, wuDict);
                                    int oldCompany = Common.Misc.getRootWorkingUnit(workingUnitOldTO.WorkingUnitID, wuDict);
                                    
                                    // move expat/task force/agency to company expat/task force/agency working units if type is not changed
                                    if (employeeTO.WorkingUnitID > 0 && syncEmplTO.EmployeeTypeID == -1)
                                    {
                                        int root = Common.Misc.getRootWorkingUnit(employeeTO.WorkingUnitID, wuDict);

                                        if (employeeTO.EmployeeTypeID == (int)Constants.EmployeeTypesFIAT.Expat && Constants.ExpatWorkingUnits.ContainsKey(root))
                                            employeeTO.WorkingUnitID = Constants.ExpatWorkingUnits[root];

                                        if (employeeTO.EmployeeTypeID == (int)Constants.EmployeeTypesFIAT.TaskForce && Constants.TaskWorkingUnits.ContainsKey(root))
                                            employeeTO.WorkingUnitID = Constants.TaskWorkingUnits[root];

                                        if (employeeTO.EmployeeTypeID == (int)Constants.EmployeeTypesFIAT.Agency && Constants.AgencyWorkingUnits.ContainsKey(root))
                                            employeeTO.WorkingUnitID = Constants.AgencyWorkingUnits[root];

                                        newCompany = Common.Misc.getRootWorkingUnit(employeeTO.WorkingUnitID, wuDict);
                                    }

                                    // set new company to asco field
                                    employeeAsco4TO.IntegerValue4 = newCompany;

                                    if (newCompany != oldCompany && oldCompany != -1)
                                    {
                                        reprocessData = true;
                                        if (reprocressDateStart == new DateTime() || reprocressDateStart.Date > syncEmplTO.ValidFrom.Date)
                                        {
                                            reprocressDateStart = syncEmplTO.ValidFrom.Date;
                                        }
                                    }
                                }
                            }
                            else if (!emplExist)
                            {
                                error = "Employee column working_unit_id can not be null";
                            }

                            //employee type change update appl users
                            if (syncEmplTO.EmployeeTypeID > -1)
                            {
                                dataChange = true;
                                int oldType = employeeTO.EmployeeTypeID;
                                employeeTO.EmployeeTypeID = syncEmplTO.EmployeeTypeID;

                                // if old type is expat or task force or expat out, employee has special organizational unit
                                // if old type is expat or task force or agency, employee has special working unit
                                // if some of type with special units are changed to types with regular units, last send unit should be found and special unit should be changed
                                bool changeOU = (oldType == (int)Constants.EmployeeTypesFIAT.Expat || oldType == (int)Constants.EmployeeTypesFIAT.TaskForce
                                    || oldType == (int)Constants.EmployeeTypesFIAT.ExpatOut) && syncEmplTO.EmployeeTypeID != (int)Constants.EmployeeTypesFIAT.Expat 
                                    && syncEmplTO.EmployeeTypeID != (int)Constants.EmployeeTypesFIAT.TaskForce && syncEmplTO.EmployeeTypeID != (int)Constants.EmployeeTypesFIAT.ExpatOut
                                    && syncEmplTO.OrganizationalUnitID == -1;
                                bool changeWU = (oldType == (int)Constants.EmployeeTypesFIAT.Expat || oldType == (int)Constants.EmployeeTypesFIAT.TaskForce
                                    || oldType == (int)Constants.EmployeeTypesFIAT.Agency) && syncEmplTO.EmployeeTypeID != (int)Constants.EmployeeTypesFIAT.Expat
                                    && syncEmplTO.EmployeeTypeID != (int)Constants.EmployeeTypesFIAT.TaskForce && syncEmplTO.EmployeeTypeID != (int)Constants.EmployeeTypesFIAT.Agency
                                    && syncEmplTO.FsUnitID == -1;

                                if (emplExist)
                                {
                                    reprocessData = true;
                                    if (reprocressDateStart == new DateTime() || reprocressDateStart.Date > syncEmplTO.ValidFrom.Date)
                                    {
                                        reprocressDateStart = syncEmplTO.ValidFrom.Date;
                                    }
                                }

                                switch (employeeTO.EmployeeTypeID)
                                {
                                    case (int)Constants.EmployeeTypesFIAT.BC:
                                        if (employeeAsco4TO.NVarcharValue5 != "")
                                        {
                                            applUser.UserTO.UserID = employeeAsco4TO.NVarcharValue5;
                                            applUser.UserTO.Status = Constants.statusActive;
                                            List<ApplUserTO> usersTO = applUser.Search();
                                            if (usersTO.Count > 0)
                                            {
                                                ApplUserTO applUserTOUpdate = usersTO[0];
                                                applUserTOUpdate.Status = Constants.statusRetired;
                                                applUsersTOUpdate.Add(applUserTOUpdate);
                                            }
                                        }
                                        break;

                                    //foreach type from 2 to 5 insert appl user and categories and visibility
                                    case (int)Constants.EmployeeTypesFIAT.WC:
                                    case (int)Constants.EmployeeTypesFIAT.ProfessionalExpert:
                                    case (int)Constants.EmployeeTypesFIAT.Professional:
                                    case (int)Constants.EmployeeTypesFIAT.Manager:
                                        //if employee is new or doesn't have username no need to insert appl user
                                        if (employeeAsco4TO.NVarcharValue5 != "")
                                        {
                                            applUser.UserTO = new ApplUserTO();
                                            applUser.UserTO.UserID = employeeAsco4TO.NVarcharValue5;
                                            List<ApplUserTO> usersTO = applUser.Search();
                                            //if there is user but not ACTIVE update it
                                            if (usersTO.Count > 0)
                                            {
                                                if (!emplExist)
                                                {
                                                    error = "User with same user name already exist. user_name = " + employeeAsco4TO.NVarcharValue5;
                                                }
                                                if (usersTO[0].Status != Constants.statusActive)
                                                {
                                                    ApplUserTO applUserTOUpdate = usersTO[0];
                                                    applUserTOUpdate.Status = Constants.statusActive;
                                                    applUsersTOUpdate.Add(applUserTOUpdate);
                                                }
                                            }
                                            //if there is no user with this username
                                            else
                                            {
                                                //set appl user to insert
                                                userToInsert.Status = Constants.statusActive;
                                                userToInsert.UserID = employeeAsco4TO.NVarcharValue5;
                                                if (employeeAsco4TO.NVarcharValue4.Length > 7)
                                                    userToInsert.Password = employeeAsco4TO.NVarcharValue4.Substring(0, 7);
                                                else
                                                    userToInsert.Password = employeeAsco4TO.NVarcharValue4;
                                                employeeTO.Password = userToInsert.Password;
                                                userToInsert.NumOfTries = Constants.numOfTries;
                                                userToInsert.Name = employeeTO.FirstName + " " + employeeTO.LastName;

                                                userToInsert.LangCode = Constants.Lang_sr;
                                                if (syncEmplTO.Language == Constants.langENG_FIAT)
                                                    userToInsert.LangCode = Constants.Lang_en;

                                                ApplUserXApplUserCategoryTO category = new ApplUserXApplUserCategoryTO();
                                                category.CategoryID = (int)Constants.UserCategoriesFIAT.WCSelfService;
                                                category.UserID = userToInsert.UserID;
                                                category.DefaultCategory = Constants.yesInt;
                                                userXCategoriesTOInsert.Add(category);
                                            }
                                        }
                                        break;
                                    case (int)Constants.EmployeeTypesFIAT.Expat:
                                    case (int)Constants.EmployeeTypesFIAT.TaskForce:
                                    case (int)Constants.EmployeeTypesFIAT.Agency:
                                        //if employee is new or doesn't have username no need to insert appl user
                                        if (employeeAsco4TO.NVarcharValue5 != "")
                                        {
                                            applUser.UserTO = new ApplUserTO();
                                            applUser.UserTO.UserID = employeeAsco4TO.NVarcharValue5;
                                            List<ApplUserTO> usersTO = applUser.Search();
                                            //if there is user but not ACTIVE update it
                                            if (usersTO.Count > 0)
                                            {
                                                if (usersTO[0].Status != Constants.statusActive)
                                                {
                                                    ApplUserTO applUserTOUpdate = usersTO[0];
                                                    applUserTOUpdate.Status = Constants.statusActive;
                                                    applUsersTOUpdate.Add(applUserTOUpdate);
                                                }
                                            }
                                            //if there is no user with this username
                                            else
                                            {
                                                //set appl user to insert
                                                userToInsert.Status = Constants.statusActive;
                                                userToInsert.UserID = employeeAsco4TO.NVarcharValue5;
                                                if (employeeAsco4TO.NVarcharValue4.Length > 7)
                                                    userToInsert.Password = employeeAsco4TO.NVarcharValue4.Substring(0, 7);
                                                else
                                                    userToInsert.Password = employeeAsco4TO.NVarcharValue4;

                                                userToInsert.NumOfTries = Constants.numOfTries;
                                                userToInsert.Name = employeeTO.FirstName + " " + employeeTO.LastName;

                                                userToInsert.LangCode = Constants.Lang_sr;
                                                if (syncEmplTO.Language == Constants.langENG_FIAT)
                                                    userToInsert.LangCode = Constants.Lang_en;
                                            }
                                        }

                                        // move expat and task force to default org unit
                                        // agency should stay in their organizational unit
                                        if (employeeTO.EmployeeTypeID != (int)Constants.EmployeeTypesFIAT.Agency && orgUnit.Find(Constants.defaultSYSOU).OrgUnitID != -1)
                                            employeeTO.OrgUnitID = Constants.defaultSYSOU;

                                        // move expat/task force/agency to company expat/task force/agency working units
                                        if (employeeTO.WorkingUnitID > 0)
                                        {
                                            int root = Common.Misc.getRootWorkingUnit(employeeTO.WorkingUnitID, wuDict);

                                            Dictionary<int, int> specCompanies = new Dictionary<int, int>();
                                            if (oldType == (int)Constants.EmployeeTypesFIAT.Expat)
                                                specCompanies = Constants.ExpatWorkingUnits;
                                            else if (oldType == (int)Constants.EmployeeTypesFIAT.TaskForce)
                                                specCompanies = Constants.TaskWorkingUnits;
                                            else if (oldType == (int)Constants.EmployeeTypesFIAT.Agency)
                                                specCompanies = Constants.AgencyWorkingUnits;

                                            if (specCompanies.Count > 0)
                                            {
                                                foreach (int key in specCompanies.Keys)
                                                {
                                                    if (employeeTO.WorkingUnitID == specCompanies[key])
                                                    {
                                                        root = key;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (employeeTO.EmployeeTypeID == (int)Constants.EmployeeTypesFIAT.Expat && Constants.ExpatWorkingUnits.ContainsKey(root))
                                                employeeTO.WorkingUnitID = Constants.ExpatWorkingUnits[root];

                                            if (employeeTO.EmployeeTypeID == (int)Constants.EmployeeTypesFIAT.TaskForce && Constants.TaskWorkingUnits.ContainsKey(root))
                                                employeeTO.WorkingUnitID = Constants.TaskWorkingUnits[root];

                                            if (employeeTO.EmployeeTypeID == (int)Constants.EmployeeTypesFIAT.Agency && Constants.AgencyWorkingUnits.ContainsKey(root))
                                                employeeTO.WorkingUnitID = Constants.AgencyWorkingUnits[root];

                                            int company = Common.Misc.getRootWorkingUnit(employeeTO.WorkingUnitID, wuDict);
                                            employeeAsco4TO.IntegerValue4 = company;
                                        }
                                        
                                        break;
                                    case (int)Constants.EmployeeTypesFIAT.ExpatOut:
                                        // move expat out to expat out organizational unit if exists
                                        OrganizationalUnitTO expatOrg = new OrganizationalUnit().Find(Constants.expatOutOU);

                                        if (expatOrg.OrgUnitID != -1)                                        
                                            employeeTO.OrgUnitID = expatOrg.OrgUnitID;

                                        break;
                                }

                                if (changeOU)
                                {
                                    // set organizatioanl unit to last sent OU
                                    int ouID = syncEmpl.SearchLastOU(syncEmplTO.EmployeeID);
                                    if (ouID != -1 && orgUnit.Find(ouID).OrgUnitID != -1)
                                        employeeTO.OrgUnitID = ouID;
                                    else
                                        error = "Last organizational unit NOT found";
                                }

                                if (changeWU)
                                {
                                    // set organizatioanl unit to last sent OU
                                    int wuID = syncEmpl.SearchLastWU(syncEmplTO.EmployeeID);

                                    if (wuID == -1 || !wuDict.ContainsKey(wuID))
                                    {
                                        error = "Working unit for employee not found! Working unit id = " + wuID.ToString().Trim() + "; for employee id = " + syncEmplTO.EmployeeID;
                                    }
                                    else
                                    {
                                        WorkingUnitTO workingUnitOldTO = new WorkingUnitTO();
                                        if (wuDict.ContainsKey(employeeTO.WorkingUnitID))
                                            workingUnitOldTO = wuDict[employeeTO.WorkingUnitID];

                                        employeeTO.WorkingUnitID = wuID;
                                        string branch = "";
                                        if (employeeAsco4TO.NVarcharValue6 != "")
                                            branch = employeeAsco4TO.NVarcharValue6;
                                        else if (syncEmplTO.EmployeeBranch != "")
                                            branch = syncEmplTO.EmployeeBranch;

                                        employeeAsco4TO.NVarcharValue2 = wuDict[wuID].Name + "." + branch;

                                        int newCompany = Common.Misc.getRootWorkingUnit(wuID, wuDict);
                                        int oldCompany = Common.Misc.getRootWorkingUnit(workingUnitOldTO.WorkingUnitID, wuDict);
                                        
                                        // set new company to asco field
                                        employeeAsco4TO.IntegerValue4 = newCompany;
                                    }
                                }
                            }
                            else if (!emplExist)
                            {
                                error = "Employee column employee_type_id can not be null";
                            }

                            // update employee position, delete risks related to old position and add risk related to new position
                            if (syncEmplTO.PositionID != -1)
                            {
                                // check if position belongs to employee company and is active
                                emplPosition.EmplPositionTO = new EmployeePositionTO();
                                
                                // for expats, task force and agency, get original company
                                switch (employeeTO.EmployeeTypeID)
                                {
                                    case (int)Constants.EmployeeTypesFIAT.Expat:
                                        foreach (int key in Constants.ExpatWorkingUnits.Keys)
                                        {
                                            if (Constants.ExpatWorkingUnits[key] == employeeTO.WorkingUnitID)
                                            {
                                                emplPosition.EmplPositionTO.WorkingUnitID = key;
                                                break;
                                            }
                                        }
                                        break;
                                    case (int)Constants.EmployeeTypesFIAT.TaskForce:
                                        foreach (int key in Constants.TaskWorkingUnits.Keys)
                                        {
                                            if (Constants.TaskWorkingUnits[key] == employeeTO.WorkingUnitID)
                                            {
                                                emplPosition.EmplPositionTO.WorkingUnitID = key;
                                                break;
                                            }
                                        }
                                        break;
                                    case (int)Constants.EmployeeTypesFIAT.Agency:
                                        foreach (int key in Constants.AgencyWorkingUnits.Keys)
                                        {
                                            if (Constants.AgencyWorkingUnits[key] == employeeTO.WorkingUnitID)
                                            {
                                                emplPosition.EmplPositionTO.WorkingUnitID = key;
                                                break;
                                            }
                                        }
                                        break;
                                }

                                if (emplPosition.EmplPositionTO.WorkingUnitID == -1)
                                    emplPosition.EmplPositionTO.WorkingUnitID = employeeAsco4TO.IntegerValue4;

                                emplPosition.EmplPositionTO.PositionID = syncEmplTO.PositionID;
                                emplPosition.EmplPositionTO.Status = Constants.statusActive;
                                List<EmployeePositionTO> posList = emplPosition.SearchEmployeePositions();

                                if (posList.Count <= 0)
                                {
                                    error = "Active employee position does not exist for employee company";
                                }
                                else
                                {
                                    dataChange = true;

                                    int oldPosition = employeeAsco4TO.IntegerValue6;

                                    employeeAsco4TO.IntegerValue6 = syncEmplTO.PositionID;

                                    // get all risk related to new position
                                    posXRisk.EmplPositionXRiskTO = new EmployeePositionXRiskTO();
                                    posXRisk.EmplPositionXRiskTO.PositionID = syncEmplTO.PositionID;
                                    List<EmployeePositionXRiskTO> newRisks = posXRisk.SearchEmployeePositionXRisks();

                                    // get all risk related to old position
                                    posXRisk.EmplPositionXRiskTO = new EmployeePositionXRiskTO();
                                    posXRisk.EmplPositionXRiskTO.PositionID = oldPosition;
                                    List<EmployeePositionXRiskTO> oldRisks = posXRisk.SearchEmployeePositionXRisks();

                                    // get all employee risks
                                    emplXRisk.EmplXRiskTO = new EmployeeXRiskTO();
                                    emplXRisk.EmplXRiskTO.EmployeeID = syncEmplTO.EmployeeID;
                                    List<EmployeeXRiskTO> emplRisks = emplXRisk.SearchEmployeeXRisks();

                                    // delete all risks related to old position
                                    List<int> riskDeleted = new List<int>();
                                    foreach (EmployeePositionXRiskTO oldRisk in oldRisks)
                                    {
                                        foreach (EmployeeXRiskTO risk in emplRisks)
                                        {
                                            if (risk.RiskID == oldRisk.RiskID && risk.DateEnd.Equals(new DateTime()))
                                            {
                                                risk.DateEnd = syncEmplTO.ValidFrom.Date;
                                                emplRisksUpdate.Add(risk);
                                                riskDeleted.Add(risk.RiskID);
                                            }
                                        }
                                    }

                                    // add new risks if they are not already defined and active for employee
                                    foreach (EmployeePositionXRiskTO newRisk in newRisks)
                                    {
                                        bool riskFound = false;
                                        foreach (EmployeeXRiskTO risk in emplRisks)
                                        {
                                            if (risk.RiskID == newRisk.RiskID && risk.DateEnd.Equals(new DateTime()) && !riskDeleted.Contains(risk.RiskID))
                                            {
                                                riskFound = true;
                                                break;
                                            }
                                        }

                                        if (!riskFound)
                                        {
                                            EmployeeXRiskTO insertRisk = new EmployeeXRiskTO();
                                            insertRisk.EmployeeID = syncEmplTO.EmployeeID;
                                            insertRisk.DateStart = syncEmplTO.ValidFrom.Date;
                                            insertRisk.RiskID = newRisk.RiskID;
                                            insertRisk.Rotation = newRisk.Rotation;
                                            insertRisk.LastDatePerformed = syncEmplTO.ValidFrom.Date;

                                            emplRisksInsert.Add(insertRisk);
                                        }
                                    }
                                }
                            }

                            //for new employees insert counters, for old, update counters if there were no changes through TM
                            if (!emplExist)
                            {
                                // create new counters for all counter type
                                List<EmployeeCounterTypeTO> types = emplCounterType.Search();
                                foreach (EmployeeCounterTypeTO countType in types)
                                {
                                    EmployeeCounterValueTO counter = new EmployeeCounterValueTO();
                                    counter.EmplCounterTypeID = countType.EmplCounterTypeID;
                                    counter.EmplID = employeeTO.EmployeeID;
                                    if (Constants.MeasureForCounterType.ContainsKey(countType.EmplCounterTypeID))
                                        counter.MeasureUnit = Constants.MeasureForCounterType[countType.EmplCounterTypeID];
                                    else
                                        counter.MeasureUnit = Constants.emplCounterMesureDay;
                                    counter.Value = 0;
                                    newCounter.Add(counter);
                                }
                            }

                            //if annual leave changed update annual leave used based on current values
                            if (syncEmplTO.AnnualLeaveCurrentYear != -1 || syncEmplTO.AnnualLeaveCurrentYearLeft != -1 || syncEmplTO.AnnualLeavePreviousYear != -1
                                || syncEmplTO.AnnualLeavePreviousYearLeft != -1)
                            {
                                if (syncEmplTO.AnnualLeaveCurrentYear >= 0 && syncEmplTO.AnnualLeavePreviousYear >= 0 && syncEmplTO.AnnualLeaveCurrentYearLeft >= 0
                                    && syncEmplTO.AnnualLeavePreviousYearLeft >= 0 && syncEmplTO.AnnualLeaveCurrentYear >= syncEmplTO.AnnualLeaveCurrentYearLeft
                                    && syncEmplTO.AnnualLeavePreviousYear >= syncEmplTO.AnnualLeavePreviousYearLeft)
                                {
                                    // get annual leave (bank hours) first cutt off date
                                    DateTime cutOff1 = new DateTime();

                                    int company = Common.Misc.getRootWorkingUnit(employeeTO.WorkingUnitID, wuDict);
                                    if (rulesDictionary.ContainsKey(company) && rulesDictionary[company].ContainsKey(employeeTO.EmployeeTypeID)
                                        && rulesDictionary[company][employeeTO.EmployeeTypeID].ContainsKey(Constants.RuleAnnualLeaveCutOffDate))
                                        cutOff1 = rulesDictionary[company][employeeTO.EmployeeTypeID][Constants.RuleAnnualLeaveCutOffDate].RuleDateTime1;

                                    if (!cutOff1.Equals(new DateTime()))
                                    {
                                        // if first cut off date passed, do not calculate previous year annual leave
                                        if (DateTime.Now.Date > new DateTime(DateTime.Now.Year, cutOff1.Month, cutOff1.Day))
                                        {
                                            syncEmplTO.AnnualLeavePreviousYearLeft = 0;
                                        }
                                    }

                                    List<EmployeeCounterValueTO> currentCounters = new List<EmployeeCounterValueTO>();
                                    int currentYearValueHist = -1;
                                    int prevYearValueHist = -1;
                                    int usedValueHist = -1;
                                    dataChange = true;
                                    if (emplExist)
                                    {
                                        emplCounter.ValueTO = new EmployeeCounterValueTO();
                                        emplCounter.ValueTO.EmplID = employeeTO.EmployeeID;
                                        currentCounters = emplCounter.Search();
                                        EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();
                                        currentYearValueHist = counterHist.SearchFirstModifiedValue(employeeTO.EmployeeID, (int)Constants.EmplCounterTypes.AnnualLeaveCounter);
                                        prevYearValueHist = counterHist.SearchFirstModifiedValue(employeeTO.EmployeeID, (int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter);
                                        usedValueHist = counterHist.SearchFirstModifiedValue(employeeTO.EmployeeID, (int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter);
                                    }
                                    else
                                    {
                                        currentCounters = newCounter;
                                    }

                                    int index = -1;
                                    int indexUsed = -1;
                                    int indexPrev = -1;
                                    bool annualLeaveUsed = false;
                                    for (int i = 0; i < currentCounters.Count; i++)
                                    {
                                        EmployeeCounterValueTO count = currentCounters[i];
                                        if (count.EmplCounterTypeID == (int)Constants.EmplCounterTypes.AnnualLeaveCounter)
                                        {
                                            index = i;
                                        }
                                        if (count.EmplCounterTypeID == (int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter)
                                        {
                                            indexUsed = i;
                                        }
                                        if (count.EmplCounterTypeID == (int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter)
                                        {
                                            indexPrev = i;
                                        }
                                    }

                                    if (currentYearValueHist != -1)
                                        annualLeaveUsed = true;

                                    if (prevYearValueHist != -1)
                                        annualLeaveUsed = true;

                                    if (usedValueHist != -1)
                                        annualLeaveUsed = true;

                                    if (annualLeaveUsed)
                                        error = "Employee has used or changed some vacation days. Vacation days can not be changed.";
                                    else
                                    {
                                        if (emplExist)
                                        {
                                            EmployeeCounterValueTO countCurr = new EmployeeCounterValueTO();
                                            countCurr = new EmployeeCounterValueTO(currentCounters[index]);
                                            if (syncEmplTO.AnnualLeaveCurrentYear != -1 && currentCounters[index].Value != syncEmplTO.AnnualLeaveCurrentYear)
                                            {
                                                histCounter.Add(currentCounters[index]);
                                                if (syncEmplTO.AnnualLeaveCurrentYear != Constants.syncIntNullValue)
                                                    countCurr.Value = syncEmplTO.AnnualLeaveCurrentYear;
                                                else
                                                    countCurr.Value = 0;
                                                updateCounter.Add(countCurr);
                                            }

                                            EmployeeCounterValueTO countPrev = new EmployeeCounterValueTO();
                                            countPrev = new EmployeeCounterValueTO(currentCounters[indexPrev]);
                                            if (syncEmplTO.AnnualLeavePreviousYear != -1 && currentCounters[indexPrev].Value != syncEmplTO.AnnualLeavePreviousYear)
                                            {
                                                histCounter.Add(currentCounters[indexPrev]);
                                                if (syncEmplTO.AnnualLeavePreviousYear != Constants.syncIntNullValue)
                                                    countPrev.Value = syncEmplTO.AnnualLeavePreviousYear;
                                                else
                                                    countPrev.Value = 0;
                                                updateCounter.Add(countPrev);
                                            }

                                            int usedLastYear = 0;
                                            int usedCurrentYear = 0;
                                            int usedVacation = 0;
                                            if (syncEmplTO.AnnualLeaveCurrentYearLeft != -1)
                                            {
                                                usedCurrentYear = countCurr.Value - syncEmplTO.AnnualLeaveCurrentYearLeft;
                                            }
                                            if (syncEmplTO.AnnualLeavePreviousYearLeft != -1)
                                            {
                                                usedLastYear = countPrev.Value - syncEmplTO.AnnualLeavePreviousYearLeft;
                                            }
                                            if (usedLastYear < countPrev.Value && usedCurrentYear > 0)
                                            {
                                                error = "Using vacation for current year before used whole vacation from last year.";
                                            }
                                            if (syncEmplTO.AnnualLeaveCurrentYearLeft != -1 || syncEmplTO.AnnualLeavePreviousYearLeft != -1)
                                            {
                                                usedVacation = usedLastYear + usedCurrentYear;

                                                if (usedVacation != currentCounters[indexUsed].Value)
                                                {
                                                    histCounter.Add(currentCounters[indexUsed]);
                                                    EmployeeCounterValueTO count = new EmployeeCounterValueTO(currentCounters[indexUsed]);
                                                    count.Value = usedVacation;
                                                    updateCounter.Add(count);
                                                }
                                            }
                                            if (updateCounter.Count > 0 && (syncEmplTO.AnnualLeaveCurrentYearLeft == -1 || syncEmplTO.AnnualLeavePreviousYearLeft == -1 || syncEmplTO.AnnualLeavePreviousYear == -1
                                                || syncEmplTO.AnnualLeavePreviousYear == -1))
                                                error = "To update/insert annual leave we need all four valid values.";
                                        }
                                        else
                                        {
                                            if (syncEmplTO.AnnualLeaveCurrentYear > -1)
                                            {
                                                newCounter[index].Value = syncEmplTO.AnnualLeaveCurrentYear;
                                            }
                                            if (syncEmplTO.AnnualLeavePreviousYear > -1)
                                            {
                                                newCounter[indexPrev].Value = syncEmplTO.AnnualLeavePreviousYear;
                                            }

                                            int usedLastYear = 0;
                                            int usedCurrentYear = 0;
                                            int usedVacation = 0;
                                            if (syncEmplTO.AnnualLeaveCurrentYearLeft != -1)
                                            {
                                                usedCurrentYear = newCounter[index].Value - syncEmplTO.AnnualLeaveCurrentYearLeft;
                                            }
                                            if (syncEmplTO.AnnualLeavePreviousYearLeft != -1)
                                            {
                                                usedLastYear = newCounter[indexPrev].Value - syncEmplTO.AnnualLeavePreviousYearLeft;
                                            }
                                            if (usedLastYear < newCounter[indexPrev].Value && usedCurrentYear > 0)
                                            {
                                                error = "Using vacation for current year before used whole vacation from last year.";
                                            }
                                            if (syncEmplTO.AnnualLeaveCurrentYearLeft != -1 || syncEmplTO.AnnualLeavePreviousYearLeft != -1)
                                            {
                                                usedVacation = usedLastYear + usedCurrentYear;

                                                newCounter[indexUsed].Value = usedVacation;
                                            }
                                        }
                                    }
                                }
                                else
                                    error = "To update/insert annual leave we need all four annual leave valid values.";
                            }

                            if ((!emplExist && error == "") || setDefaultShift)
                            {
                                // insert default shift (default working group and its schedule) from hiring date
                                Dictionary<string, RuleTO> emplRules = new Dictionary<string, RuleTO>();                                
                                int WUID = Common.Misc.getRootWorkingUnit(employeeTO.WorkingUnitID, wuDict);
                                if (rulesDictionary.ContainsKey(WUID) && rulesDictionary[WUID].ContainsKey(employeeTO.EmployeeTypeID))
                                {
                                    emplRules = rulesDictionary[WUID][employeeTO.EmployeeTypeID];
                                    if (emplRules.ContainsKey(Constants.RuleDefaultShift))
                                    {
                                        employeeTO.WorkingGroupID = emplRules[Constants.RuleDefaultShift].RuleValue;
                                        List<EmployeeGroupsTimeScheduleTO> timeScheduleList = new EmployeeGroupsTimeSchedule(ACTAConnection).SearchMonthSchedule(emplRules[Constants.RuleDefaultShift].RuleValue, syncEmplTO.ValidFrom.Date);

                                        int timeScheduleIndex = -1;
                                        for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
                                        {
                                            if (syncEmplTO.ValidFrom.Date >= timeScheduleList[scheduleIndex].Date)
                                            {
                                                timeScheduleIndex = scheduleIndex;
                                            }
                                        }

                                        Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> days = new Dictionary<int, Dictionary<int, WorkTimeIntervalTO>>();
                                        int dayNum = -1;
                                        if (timeScheduleIndex >= 0)
                                        {
                                            EmployeeGroupsTimeScheduleTO egts = timeScheduleList[timeScheduleIndex];
                                            int startDay = egts.StartCycleDay;
                                            int schemaID = egts.TimeSchemaID;

                                            WorkTimeSchemaTO actualTimeSchema = null;
                                            if (dictTimeSchema.ContainsKey(schemaID))
                                                actualTimeSchema = dictTimeSchema[schemaID];

                                            if (actualTimeSchema != null)
                                            {
                                                EmployeeTimeScheduleTO schedule = new EmployeeTimeScheduleTO();

                                                days = actualTimeSchema.Days;
                                                int cycleDuration = actualTimeSchema.CycleDuration;

                                                TimeSpan ts1 = new TimeSpan(syncEmplTO.ValidFrom.Date.Ticks - egts.Date.Date.Ticks);
                                                dayNum = (startDay + (int)ts1.TotalDays) % cycleDuration;
                                                schedule.Date = syncEmplTO.ValidFrom.Date;
                                                schedule.EmployeeID = syncEmplTO.EmployeeID;
                                                schedule.StartCycleDay = dayNum;
                                                schedule.TimeSchemaID = actualTimeSchema.TimeSchemaID;
                                                emplTimeScheduleInsert.Add(schedule); 
                                            }
                                        }
                                    }
                                }
                            }

                            //if username changed old user update to retired and make new user with all rules and categories as old one
                            if (syncEmplTO.Username != "")
                            {
                                dataChange = true;
                                oldUserName = employeeAsco4TO.NVarcharValue5;

                                applUser.UserTO = new ApplUserTO();
                                applUser.UserTO.UserID = oldUserName;
                                List<ApplUserTO> usersTOList = applUser.Search();
                                if (usersTOList.Count > 0 && oldUserName != "" && oldUserName != syncEmplTO.Username)
                                {
                                    ApplUserTO applUserTOUpdate = usersTOList[0];
                                    applUserTOUpdate.Status = Constants.statusRetired;
                                    applUsersTOUpdate.Add(applUserTOUpdate);
                                }

                                if (syncEmplTO.Username == Constants.syncStringNullValue)
                                    employeeAsco4TO.NVarcharValue5 = "";
                                else
                                {
                                    // set new username in asco
                                    employeeAsco4TO.NVarcharValue5 = syncEmplTO.Username;

                                    switch (employeeTO.EmployeeTypeID)
                                    {
                                        case (int)Constants.EmployeeTypesFIAT.BC:
                                            if (employeeAsco4TO.NVarcharValue5 != "")
                                            {
                                                applUser.UserTO.UserID = employeeAsco4TO.NVarcharValue5;
                                                applUser.UserTO.Status = Constants.statusActive;
                                                List<ApplUserTO> usersTO = applUser.Search();
                                                if (usersTO.Count > 0)
                                                {
                                                    ApplUserTO applUserTOUpdate = usersTO[0];
                                                    applUserTOUpdate.Status = Constants.statusRetired;
                                                    applUsersTOUpdate.Add(applUserTOUpdate);
                                                }
                                            }
                                            break;

                                        //foreach type from 2 to 5 insert appl user and categories and visibility
                                        case (int)Constants.EmployeeTypesFIAT.WC:
                                        case (int)Constants.EmployeeTypesFIAT.ProfessionalExpert:
                                        case (int)Constants.EmployeeTypesFIAT.Professional:
                                        case (int)Constants.EmployeeTypesFIAT.Manager:
                                            //if employee is new or doesn't have username no need to insert appl user
                                            if (employeeAsco4TO.NVarcharValue5 != "" || (syncEmplTO.Username != "" && !emplExist))
                                            {
                                                applUser.UserTO = new ApplUserTO();
                                                applUser.UserTO.UserID = employeeAsco4TO.NVarcharValue5;
                                                List<ApplUserTO> usersTO = applUser.Search();
                                                //if there is user but not ACTIVE update it
                                                if (usersTO.Count > 0)
                                                {
                                                    if (!emplExist)
                                                    {
                                                        error = "User with same user name already exist. user_name = " + employeeAsco4TO.NVarcharValue5;
                                                    }
                                                    if (usersTO[0].Status != Constants.statusActive)
                                                    {
                                                        ApplUserTO applUserTOUpdate = usersTO[0];
                                                        applUserTOUpdate.Status = Constants.statusActive;
                                                        applUsersTOUpdate.Add(applUserTOUpdate);
                                                    }
                                                }
                                                //if there is no user with this username
                                                else
                                                {
                                                    //set appl user to insert
                                                    userToInsert.Status = Constants.statusActive;
                                                    userToInsert.UserID = employeeAsco4TO.NVarcharValue5;
                                                    if (employeeAsco4TO.NVarcharValue4.Length > 7)
                                                        userToInsert.Password = employeeAsco4TO.NVarcharValue4.Substring(0, 7);
                                                    else
                                                        userToInsert.Password = employeeAsco4TO.NVarcharValue4;
                                                    employeeTO.Password = userToInsert.Password;
                                                    userToInsert.NumOfTries = Constants.numOfTries;
                                                    userToInsert.Name = employeeTO.FirstName + " " + employeeTO.LastName;

                                                    userToInsert.LangCode = Constants.Lang_sr;
                                                    if (syncEmplTO.Language == Constants.langENG_FIAT)
                                                        userToInsert.LangCode = Constants.Lang_en;

                                                    ApplUserXApplUserCategoryTO category = new ApplUserXApplUserCategoryTO();
                                                    category.CategoryID = (int)Constants.UserCategoriesFIAT.WCSelfService;
                                                    category.UserID = userToInsert.UserID;
                                                    category.DefaultCategory = Constants.yesInt;
                                                    userXCategoriesTOInsert.Add(category);
                                                }
                                            }

                                            break;
                                        case (int)Constants.EmployeeTypesFIAT.Expat:
                                        case (int)Constants.EmployeeTypesFIAT.TaskForce:
                                        case (int)Constants.EmployeeTypesFIAT.Agency:
                                            //if employee is new or doesn't have username no need to insert appl user
                                            if (employeeAsco4TO.NVarcharValue5 != "")
                                            {
                                                applUser.UserTO = new ApplUserTO();
                                                applUser.UserTO.UserID = employeeAsco4TO.NVarcharValue5;
                                                List<ApplUserTO> usersTO = applUser.Search();
                                                //if there is user but not ACTIVE update it
                                                if (usersTO.Count > 0)
                                                {
                                                    if (usersTO[0].Status != Constants.statusActive)
                                                    {
                                                        ApplUserTO applUserTOUpdate = usersTO[0];
                                                        applUserTOUpdate.Status = Constants.statusActive;
                                                        applUsersTOUpdate.Add(applUserTOUpdate);
                                                    }
                                                }
                                                //if there is no user with this username
                                                else
                                                {
                                                    //set appl user to insert
                                                    userToInsert.Status = Constants.statusActive;
                                                    userToInsert.UserID = employeeAsco4TO.NVarcharValue5;
                                                    if (employeeAsco4TO.NVarcharValue4.Length > 7)
                                                        userToInsert.Password = employeeAsco4TO.NVarcharValue4.Substring(0, 7);
                                                    else
                                                        userToInsert.Password = employeeAsco4TO.NVarcharValue4;

                                                    userToInsert.NumOfTries = Constants.numOfTries;
                                                    userToInsert.Name = employeeTO.FirstName + " " + employeeTO.LastName;

                                                    userToInsert.LangCode = Constants.Lang_sr;
                                                    if (syncEmplTO.Language == Constants.langENG_FIAT)
                                                        userToInsert.LangCode = Constants.Lang_en;

                                                    if (employeeTO.WorkingUnitID > 0)
                                                    {
                                                        int root = Common.Misc.getRootWorkingUnit(employeeTO.WorkingUnitID, wuDict);
                                                        if (Constants.ExpatWorkingUnits.ContainsKey(root))
                                                            employeeTO.WorkingUnitID = Constants.ExpatWorkingUnits[root];
                                                    }
                                                }
                                            }
                                            else if (!emplExist)
                                            {
                                                //set appl user to insert
                                                userToInsert.Status = Constants.statusActive;
                                                userToInsert.UserID = syncEmplTO.Username;
                                                if (userToInsert.UserID == "")
                                                {
                                                    userToInsert.UserID = employeeTO.EmployeeID.ToString();
                                                    employeeAsco4TO.NVarcharValue5 = userToInsert.UserID;
                                                }
                                                if (employeeAsco4TO.NVarcharValue4.Length > 7)
                                                    userToInsert.Password = employeeAsco4TO.NVarcharValue4.Substring(0, 7);
                                                else
                                                    userToInsert.Password = employeeAsco4TO.NVarcharValue4;
                                                employeeTO.Password = userToInsert.Password;

                                                userToInsert.NumOfTries = Constants.numOfTries;
                                                userToInsert.Name = employeeTO.FirstName + " " + employeeTO.LastName;

                                                userToInsert.LangCode = Constants.Lang_sr;
                                                if (syncEmplTO.Language == Constants.langENG_FIAT)
                                                    userToInsert.LangCode = Constants.Lang_en;
                                            }
                                            break;
                                    }

                                    //if employee had some user_name before
                                    if (oldUserName != "" && oldUserName != employeeAsco4TO.NVarcharValue5)
                                    {
                                        if (emplExist)
                                        {
                                            // set old visibility and categories to new user to
                                            applUserXOrgUnit.AuXOUnitTO.UserID = oldUserName;
                                            List<ApplUserXOrgUnitTO> oldUserOUList = applUserXOrgUnit.Search();
                                            foreach (ApplUserXOrgUnitTO oldRule in oldUserOUList)
                                            {
                                                bool ruleExist = false;
                                                foreach (ApplUserXOrgUnitTO newRule in applUsersXOUInsert)
                                                {
                                                    if (oldRule.OrgUnitID == newRule.OrgUnitID && oldRule.Purpose == newRule.Purpose)
                                                    {
                                                        ruleExist = true;
                                                    }
                                                }

                                                if (!ruleExist)
                                                {
                                                    ApplUserXOrgUnitTO ruleAdd = oldRule;
                                                    ruleAdd.UserID = employeeAsco4TO.NVarcharValue5;
                                                    applUsersXOUInsert.Add(ruleAdd);
                                                }
                                            }

                                            applUserXWU.AuXWUnitTO.UserID = oldUserName;
                                            List<ApplUsersXWUTO> oldUserWUList = applUserXWU.Search();
                                            foreach (ApplUsersXWUTO oldRule in oldUserWUList)
                                            {
                                                bool ruleExist = false;
                                                foreach (ApplUsersXWUTO newRule in applUsersXWUInsert)
                                                {
                                                    if (oldRule.WorkingUnitID == newRule.WorkingUnitID && oldRule.Purpose == newRule.Purpose)
                                                    {
                                                        ruleExist = true;
                                                    }
                                                }

                                                if (!ruleExist)
                                                {
                                                    ApplUsersXWUTO ruleAdd = oldRule;
                                                    ruleAdd.UserID = employeeAsco4TO.NVarcharValue5;
                                                    applUsersXWUInsert.Add(ruleAdd);
                                                }
                                            }
                                                                                        
                                            applUserXCategory.UserXCategoryTO = new ApplUserXApplUserCategoryTO();
                                            applUserXCategory.UserXCategoryTO.UserID = oldUserName;
                                            List<ApplUserXApplUserCategoryTO> oldUserCategoriesList = applUserXCategory.Search();
                                            foreach (ApplUserXApplUserCategoryTO oldCategory in oldUserCategoriesList)
                                            {
                                                bool categoryExist = false;
                                                foreach (ApplUserXApplUserCategoryTO newCategory in userXCategoriesTOInsert)
                                                {
                                                    if (oldCategory.CategoryID == newCategory.CategoryID)
                                                    {
                                                        categoryExist = true;
                                                    }
                                                }

                                                if (!categoryExist)
                                                {
                                                    ApplUserXApplUserCategoryTO categoryAdd = oldCategory;
                                                    categoryAdd.UserID = employeeAsco4TO.NVarcharValue5;
                                                    userXCategoriesTOInsert.Add(categoryAdd);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            
                            // check if there is reporcessing for locked period
                            if (error == "" && reprocessDays)
                            {
                                foreach (DateTime day in dayToReprocess)
                                {
                                    if (day.Date < minReprocessedDate.Date)
                                    {
                                        error = "Changing locked period";
                                        break;
                                    }
                                }
                            }

                            if (error == "" && reprocessData && reprocressDateStart.Date < minReprocessedDate.Date)
                                error = "Changing locked period";

                            if (error == "" && reprocessLog)
                            {
                                foreach (LogTO log in logsTOReprocess)
                                {
                                    if (log.EventTime.Date < minReprocessedDate.Date)
                                    {
                                        error = "Changing locked period";
                                        break;
                                    }
                                }
                            }

                            //do inserts, updates, deletes in ACTA DB
                            //begin ACTA transaction
                            bool trans = employee.BeginTransaction();
                            if (trans)
                            {
                                try
                                {
                                    employee.EmplTO = employeeTO;
                                    employeeAsco4.EmplAsco4TO = employeeAsco4TO;
                                    bool succ = true;
                                    if (error == "" && dataChange)
                                    {
                                        employeeAsco4.SetTransaction(employee.GetTransaction());
                                        if (emplExist)
                                        {
                                            if (employeeHistTO.OrgUnitID != employeeTO.OrgUnitID || employeeHistTO.FirstName != employeeTO.FirstName || employeeHistTO.LastName != employeeTO.LastName || employeeHistTO.WorkingUnitID != employeeTO.WorkingUnitID
                                                || employeeHistTO.Status != employeeTO.Status || employeeHistTO.Password != employeeTO.Password || employeeHistTO.AddressID != employeeTO.AddressID || employeeHistTO.Picture != employeeTO.Picture
                                                || employeeHistTO.WorkingGroupID != employeeTO.WorkingGroupID || employeeHistTO.Type != employeeTO.Type || employeeHistTO.AccessGroupID != employeeTO.AccessGroupID || employeeHistTO.EmployeeTypeID != employeeTO.EmployeeTypeID)
                                            {
                                                succ = succ && employee.Update(false);
                                                emplHist.SetTransaction(employee.GetTransaction());

                                                succ = succ && emplHist.Save(employeeHistTO.EmployeeID.ToString(), employeeHistTO.FirstName, employeeHistTO.LastName, employeeHistTO.WorkingUnitID.ToString(), employeeHistTO.Status, employeeHistTO.Password,
                                                employeeHistTO.AddressID.ToString(), employeeHistTO.Picture, employeeHistTO.WorkingGroupID.ToString(), employeeHistTO.Type, employeeHistTO.AccessGroupID.ToString(), employeeHistTO.EmployeeTypeID.ToString(),
                                                employeeHistTO.OrgUnitID.ToString(), employeeHistTO.CreatedBy, employeeHistTO.CreatedTime, syncEmplTO.ValidFrom, false) > 0;
                                            }

                                            EmployeeAsco4TO asco4Hist = emplAscoHist.EmplAsco4TO;
                                            if (employeeAsco4TO.DatetimeValue1 != asco4Hist.DatetimeValue1 || employeeAsco4TO.DatetimeValue2 != asco4Hist.DatetimeValue2 || employeeAsco4TO.DatetimeValue3 != asco4Hist.DatetimeValue3 || employeeAsco4TO.DatetimeValue4 != asco4Hist.DatetimeValue4
                                                || employeeAsco4TO.DatetimeValue5 != asco4Hist.DatetimeValue5 || employeeAsco4TO.DatetimeValue6 != asco4Hist.DatetimeValue6 || employeeAsco4TO.DatetimeValue7 != asco4Hist.DatetimeValue7 || employeeAsco4TO.DatetimeValue8 != asco4Hist.DatetimeValue8
                                                || employeeAsco4TO.DatetimeValue9 != asco4Hist.DatetimeValue9 || employeeAsco4TO.DatetimeValue10 != asco4Hist.DatetimeValue10 || employeeAsco4TO.IntegerValue1 != asco4Hist.IntegerValue1 || employeeAsco4TO.IntegerValue2 != asco4Hist.IntegerValue2
                                                || employeeAsco4TO.IntegerValue3 != asco4Hist.IntegerValue3 || employeeAsco4TO.IntegerValue4 != asco4Hist.IntegerValue4 || employeeAsco4TO.IntegerValue5 != asco4Hist.IntegerValue5 || employeeAsco4TO.IntegerValue6 != asco4Hist.IntegerValue6
                                                || employeeAsco4TO.IntegerValue7 != asco4Hist.IntegerValue7 || employeeAsco4TO.IntegerValue8 != asco4Hist.IntegerValue8 || employeeAsco4TO.IntegerValue9 != asco4Hist.IntegerValue9 || employeeAsco4TO.IntegerValue10 != asco4Hist.IntegerValue10
                                                || employeeAsco4TO.NVarcharValue1 != asco4Hist.NVarcharValue1 || employeeAsco4TO.NVarcharValue2 != asco4Hist.NVarcharValue2 || employeeAsco4TO.NVarcharValue3 != asco4Hist.NVarcharValue3 || employeeAsco4TO.NVarcharValue4 != asco4Hist.NVarcharValue4
                                                || employeeAsco4TO.NVarcharValue5 != asco4Hist.NVarcharValue5 || employeeAsco4TO.NVarcharValue6 != asco4Hist.NVarcharValue6 || employeeAsco4TO.NVarcharValue7 != asco4Hist.NVarcharValue7 || employeeAsco4TO.NVarcharValue8 != asco4Hist.NVarcharValue8
                                                || employeeAsco4TO.NVarcharValue9 != asco4Hist.NVarcharValue9 || employeeAsco4TO.NVarcharValue10 != asco4Hist.NVarcharValue10)
                                            {
                                                succ = succ && employeeAsco4.update(false);
                                                emplAscoHist.SetTransaction(employeeAsco4.GetTransaction());
                                                emplAscoHist.save(false);
                                            }

                                            emplTimeSchedule.SetTransaction(employee.GetTransaction());
                                            if (emplTimeSchedulesDel.Count > 0)
                                            {
                                                succ = succ && emplTimeSchedule.DeleteFromToSchedule(employee.EmplTO.EmployeeID, syncEmplTO.ValidFrom, new DateTime(), Constants.syncUser, false);
                                            }

                                            foreach (EmployeeTimeScheduleTO etsInsert in emplTimeScheduleInsert)
                                            {
                                                succ = succ && emplTimeSchedule.Save(etsInsert.EmployeeID, etsInsert.Date, etsInsert.TimeSchemaID, etsInsert.StartCycleDay, "", false) > 0;
                                            }

                                            if (reprocessDays)
                                            {
                                                DateTime startDate = new DateTime();
                                                DateTime endDate = new DateTime();

                                                foreach (DateTime date in dayToReprocess)
                                                {
                                                    if (startDate == new DateTime() || startDate > date)
                                                    {
                                                        startDate = date;
                                                    }
                                                    if (endDate == new DateTime() || endDate < date)
                                                        endDate = date;
                                                }

                                                //list of datetime for each employee
                                                Dictionary<int, List<DateTime>> emplDateWholeDayList = new Dictionary<int, List<DateTime>>();
                                                emplDateWholeDayList.Add(employee.EmplTO.EmployeeID, dayToReprocess);

                                                if (dayToReprocess.Count > 0)
                                                    succ = succ && Common.Misc.ReprocessPairsAndRecalculateCounters(employee.EmplTO.EmployeeID.ToString(), syncEmplTO.ValidFrom, endDate, employee.GetTransaction(), emplDateWholeDayList, ACTAConnection, "");
                                            }

                                            if (syncEmplTO.Picture != null)
                                            {
                                                eif.SetTransaction(employee.GetTransaction());
                                                eif.Delete(employeeTO.EmployeeID, false);
                                                succ = succ && eif.Save(employeeTO.EmployeeID, syncEmplTO.Picture, false) > 0;
                                            }
                                        }
                                        else
                                        {
                                            if (employeeTO.Status == "")
                                                employeeTO.Status = Constants.statusActive;
                                            
                                            succ = succ && employee.Save(employeeTO.EmployeeID.ToString(), employeeTO.FirstName, employeeTO.LastName, employeeTO.WorkingUnitID.ToString(), employeeTO.Status, employeeTO.Password,
                                                employeeTO.AddressID.ToString(), employeeTO.Picture, employeeTO.WorkingGroupID.ToString(), employeeTO.EmployeeTypeID.ToString(), employeeTO.AccessGroupID.ToString(), employeeTO.OrgUnitID, false) > 0;

                                            succ = succ && employeeAsco4.save(false);

                                            //insert time schema for new employee 
                                            emplTimeSchedule.SetTransaction(employee.GetTransaction());
                                            if (syncEmplTO.ValidFrom.Day != 1)
                                            {
                                                succ = succ && (emplTimeSchedule.Save(employeeTO.EmployeeID, new DateTime(syncEmplTO.ValidFrom.Year, 
                                                    syncEmplTO.ValidFrom.Month, 1), Constants.defaultSchemaID, Constants.defaultStartDay, "", false) > 0 ? true : false);
                                            }

                                            if (employeeTO.Picture != "")
                                            {
                                                eif.SetTransaction(employee.GetTransaction());
                                                succ = succ && eif.Save(employeeTO.EmployeeID, syncEmplTO.Picture, false) > 0;
                                            }

                                            //insert employee location for new employee
                                            EmployeeLocation emplLoc = new EmployeeLocation(ACTAConnection);
                                            emplLoc.SetTransaction(employee.GetTransaction());
                                            succ = succ && (emplLoc.Save(employeeTO.EmployeeID, -1, -1, -1, new DateTime(0), -1, false) > 0 ? true : false);

                                            emplTimeSchedule.SetTransaction(emplLoc.GetTransaction());
                                            foreach (EmployeeTimeScheduleTO etsInsert in emplTimeScheduleInsert)
                                            {
                                                succ = succ && emplTimeSchedule.Save(etsInsert.EmployeeID, etsInsert.Date, etsInsert.TimeSchemaID, etsInsert.StartCycleDay, "", false) > 0;
                                            }
                                        }

                                        if (tagUpdate.OwnerID != -1)
                                        {
                                            tag.SetTransaction(employee.GetTransaction());
                                            succ = succ && tag.Update(tagUpdate.RecordID, tagUpdate.TagID, tagUpdate.OwnerID, tagUpdate.Status, tagUpdate.Description, new DateTime(), new DateTime(), Constants.syncUser, false);                                            
                                        }

                                        if (tagInsert.OwnerID != -1)
                                        {
                                            tag.SetTransaction(employee.GetTransaction());
                                            succ = succ && tag.SaveFromS(tagInsert.TagID, tagInsert.OwnerID, tagInsert.Status, tagInsert.Description, new DateTime(), new DateTime(), Constants.syncUser, false) > 0;
                                        }

                                        applUser.SetTransaction(employee.GetTransaction());
                                        if (applUsersTOUpdate.Count > 0)
                                        {
                                            foreach (ApplUserTO user in applUsersTOUpdate)
                                            {
                                                applUser.UserTO = user;
                                                succ = succ && applUser.Update(false);
                                            }
                                        }

                                        if (userToInsert.UserID != "")
                                        {
                                            applUser.UserTO = userToInsert;
                                            succ = succ && applUser.Save(false) > 0;
                                        }
                                                                                
                                        if (applUsersXOUInsert.Count > 0)
                                        {
                                            applUserXOrgUnit.SetTransaction(employee.GetTransaction());
                                                                                        
                                            if (employeeAsco4TO.NVarcharValue5 != "")
                                                applUserXOrgUnit.Delete(employeeAsco4TO.NVarcharValue5, -1, "", false);

                                            foreach (ApplUserXOrgUnitTO applUserXOUTO in applUsersXOUInsert)
                                            {
                                                succ = succ && applUserXOrgUnit.Save(applUserXOUTO.UserID, applUserXOUTO.OrgUnitID, applUserXOUTO.Purpose, false) > 0;
                                            }
                                        }
                                                                                
                                        if (applUsersXWUInsert.Count > 0)
                                        {
                                            applUserXWU.SetTransaction(employee.GetTransaction());
                                                                                        
                                            if (employeeAsco4TO.NVarcharValue5 != "")
                                                applUserXWU.Delete(employeeAsco4TO.NVarcharValue5, -1, "", false);

                                            foreach (ApplUsersXWUTO applUserXWUTO in applUsersXWUInsert)
                                            {
                                                succ = succ && applUserXWU.Save(applUserXWUTO.UserID, applUserXWUTO.WorkingUnitID, applUserXWUTO.Purpose, false) > 0;
                                            }
                                        }

                                        if (userXCategoriesTOInsert.Count > 0)
                                        {
                                            applUserXCategory.SetTransaction(employee.GetTransaction());
                                            
                                            List<ApplUserXApplUserCategoryTO> usersCategories = new List<ApplUserXApplUserCategoryTO>();
                                            if (employeeAsco4TO.NVarcharValue5 != "")
                                            {
                                                applUserXCategory.UserXCategoryTO.UserID = employeeAsco4TO.NVarcharValue5;
                                                usersCategories = applUserXCategory.Search(employee.GetTransaction());
                                            }
                                            
                                            //if (employeeAsco4TO.NVarcharValue5 != "")
                                            //    applUserXCategory.Delete(employeeAsco4TO.NVarcharValue5, false);
                                            
                                            foreach (ApplUserXApplUserCategoryTO applUserXCategoryTO in userXCategoriesTOInsert)
                                            {
                                                bool categoryExists = false;

                                                foreach (ApplUserXApplUserCategoryTO userCategory in usersCategories)
                                                {
                                                    if (applUserXCategoryTO.CategoryID == userCategory.CategoryID && applUserXCategoryTO.UserID == userCategory.UserID)
                                                        categoryExists = true;
                                                }

                                                if (!categoryExists)
                                                {
                                                    applUserXCategory.UserXCategoryTO = applUserXCategoryTO;
                                                    succ = succ && applUserXCategory.Save(false) > 0;
                                                }
                                            }
                                        }

                                        if (histCounter.Count > 0)
                                        {
                                            emplCounterHist.SetTransaction(employee.GetTransaction());
                                            foreach (EmployeeCounterValueTO oldValue in histCounter)
                                            {
                                                emplCounterHist.ValueTO = new EmployeeCounterValueHistTO(oldValue);
                                                succ = succ && emplCounterHist.Save(false) > 0;
                                            }
                                        }

                                        if (emplRisksUpdate.Count > 0 || emplRisksInsert.Count > 0)
                                        {
                                            emplXRisk.SetTransaction(employee.GetTransaction());
                                            foreach (EmployeeXRiskTO updRisk in emplRisksUpdate)
                                            {
                                                emplXRisk.EmplXRiskTO = new EmployeeXRiskTO();
                                                emplXRisk.EmplXRiskTO = updRisk;
                                                succ = succ && emplXRisk.Update(false);
                                            }

                                            foreach (EmployeeXRiskTO insRisk in emplRisksInsert)
                                            {
                                                emplXRisk.EmplXRiskTO = new EmployeeXRiskTO();
                                                emplXRisk.EmplXRiskTO = insRisk;
                                                succ = succ && emplXRisk.Save(false) > 0;
                                            }
                                        }

                                        emplCounter.SetTransaction(employee.GetTransaction());
                                        if (!emplExist)
                                        {
                                            foreach (EmployeeCounterValueTO oldValue in newCounter)
                                            {
                                                emplCounter.ValueTO = oldValue;
                                                succ = succ && emplCounter.Save(false) > 0;
                                            }
                                        }
                                        else
                                        {
                                            foreach (EmployeeCounterValueTO oldValue in updateCounter)
                                            {
                                                emplCounter.ValueTO = oldValue;
                                                succ = succ && emplCounter.Update(false);                                                
                                            }
                                        }

                                        if (reprocessData)
                                        {
                                            ioPair.SetTransaction(employee.GetTransaction());
                                            Dictionary<int, List<DateTime>> dictionary = new Dictionary<int, List<DateTime>>();
                                            dictionary.Add(employee.EmplTO.EmployeeID, new List<DateTime>());
                                            DateTime date = reprocressDateStart.Date;
                                            while (date <= DateTime.Now.Date)
                                            {
                                                dictionary[employee.EmplTO.EmployeeID].Add(date);
                                                date = date.AddDays(1);
                                            }
                                            succ = succ && ioPair.updateToUnprocessed(dictionary, false);
                                        }

                                        if (reprocessLog && logsTOReprocess.Count > 0)
                                        {
                                            logObj.SetTransaction(employee.GetTransaction());
                                            foreach(LogTO logTO in logsTOReprocess)
                                            {
                                                logTO.PassGenUsed = (int)Constants.PassGenUsed.Unused;
                                                logTO.ActionCommited = (int)Constants.actionCommitedAllowed;
                                                logTO.EventHappened = (int)Constants.EventTag.eventTagAllowed;
                                                succ = succ && logObj.Update(logTO, false);
                                            }
                                        }
                                    }

                                    if (succ)
                                    {
                                        syncEmpl.SetTransaction(employee.GetTransaction());
                                        syncEmpl.SyncEmplTO = syncEmplTO;
                                        if (error == "")
                                            syncEmpl.SyncEmplTO.Result = Constants.resultSucc;
                                        else
                                        {
                                            WriteLog(" SyncEmployees - " + error);
                                            syncEmpl.SyncEmplTO.Result = Constants.resultFaild;
                                            syncEmpl.SyncEmplTO.Remark = error;
                                        }

                                        succ = succ && syncEmpl.insert(false);
                                        if (succ)
                                        {
                                            //before commit ACTA transaction try to delete record from sync DB
                                            succ = syncDAO.delSyncEmployee(syncEmplTO);
                                            if (succ)
                                            {
                                                employee.CommitTransaction();
                                            }
                                            else
                                            {
                                                employee.RollbackTransaction();
                                                WriteLog(" SyncEmployees() Delete record from sync_organizational_structure fail for rec_id = " + syncEmpl.SyncEmplTO.RecID.ToString());
                                            }
                                        }
                                        else
                                        {
                                            employee.RollbackTransaction();
                                        }
                                    }

                                    if (!succ)
                                    {
                                        if (employee.GetTransaction() != null)
                                            employee.RollbackTransaction();
                                        WriteLog(" SyncEmployees() ACTA DB record insert faild without exception for rec_id = " + syncEmplTO.RecID.ToString());
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (employee.GetTransaction() != null)
                                        employee.RollbackTransaction();
                                    WriteLog(" SyncEmployees() exception: " + ex.Message + "; for rec_id = " + syncEmplTO.RecID.ToString());
                                }
                            }
                            else
                            {
                                WriteLog(" SyncEmployees() ACTA DB begin transaction faild! ");
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog(" SyncEmployees() exception: " + ex.Message);
                        }                        
                    }

                    if (tagChanged)
                    {
                        DataManager dataMgr = new DataManager();
                        if (dataMgr.CheckNewTags(ACTAConnection))
                        {
                            dataMgr.FinalizeTagsTracking(false, true, null, ACTAConnection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(" SyncEmployees() exception: " + ex.Message);
            }
        }

        private void SyncOrganizacionalStructure()
        {
            try
            {
                //get all records from sync_organizational_structures
                List<SyncOrganizationalStructureTO> osRecords = syncDAO.getDifOrganizationalStructures();
                               
                //if there is no records continue
                if (osRecords.Count <= 0)
                {
                    WriteLog(" SyncOrganizationalStructure - Records not found. ");
                }
                //if records found begin processing
                else
                {
                    WriteLog(" SyncOrganizationalStructure - Num of records found: " + osRecords.Count.ToString());

                    OrganizationalUnit orgUnit = new OrganizationalUnit(ACTAConnection);
                    SyncOrganizationalStructureHist syncOS = new SyncOrganizationalStructureHist(ACTAConnection);
                    WorkingUnit workingUnit = new WorkingUnit(ACTAConnection);
                    WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(ACTAConnection);                    
                    ApplUserXOrgUnit auXou = new ApplUserXOrgUnit(ACTAConnection);
                    EmployeeResponsibility emplResponsibility = new EmployeeResponsibility(ACTAConnection);
                    EmployeeAsco4 asco = new EmployeeAsco4(ACTAConnection);

                    List<ApplUserXOrgUnitTO> applUsersXOUDel = new List<ApplUserXOrgUnitTO>();
                    List<ApplUserXOrgUnitTO> applUsersXOUInsert = new List<ApplUserXOrgUnitTO>();

                    Dictionary<int, WorkingUnitTO> wuDIct = workingUnit.getWUDictionary();

                    int count = 0;
                    //process records one by one
                    foreach (SyncOrganizationalStructureTO osTO in osRecords)
                    {
                        count++;
                        SetManagerThreadStatus("Synchronization Organizational Structure: " + count.ToString() + "/" + osRecords.Count.ToString());

                        if (osTO.ValidFrom.Date > DateTime.Now.Date || osTO.ValidFrom.Equals(new DateTime()))
                            continue;

                        string error = "";
                        bool dataChange = false;
                        
                        try
                        {
                            WorkingUnitXOrganizationalUnitTO delWUTO = new WorkingUnitXOrganizationalUnitTO();

                            OrganizationalUnitTO orgUnitTO = orgUnit.Find(osTO.UnitID);
                            bool unitExist = false;
                            if (orgUnitTO.OrgUnitID >= 0)
                                unitExist = true;

                            //if unit exist
                            if (unitExist)
                            {
                                WorkingUnitTO newWUTO = new WorkingUnitTO();
                                //if description or code changed and date time is now or passed update working unit description or code
                                if (osTO.Description != "" || osTO.Code != "" || osTO.Status != "")
                                {
                                    dataChange = true;
                                    if (osTO.Description != "")
                                    {
                                        orgUnitTO.Desc = osTO.Description;
                                        orgUnitTO.Name = osTO.Description;
                                    }

                                    if (osTO.Code != "")
                                        orgUnitTO.Code = osTO.Code;
                                    if (osTO.Status != "")
                                        orgUnitTO.Status = osTO.Status;
                                }

                                //if cost center stringone changed update WorkingUnitXOrganizationalUnit
                                if (osTO.CostCenterStringone != "" || osTO.CompanyCode != "")
                                {
                                    if (osTO.CompanyCode == "1367")
                                        osTO.CostCenterStringone = osTO.CostCenterStringone.Substring(3);

                                    if (osTO.CostCenterStringone.Trim().Length < 3)
                                    {
                                        error = "Cost center code invalid lenght";
                                        WriteLog(" SyncOrganizationalStructure(): Cost center code invalid lenght for rec id = " + osTO.RecID);
                                    }

                                    if (error.Trim().Equals(""))
                                    {
                                        dataChange = true;                                        
                                        
                                        string stringone = osTO.CostCenterStringone.Substring(0, 3) + "." + osTO.CostCenterStringone.Substring(3);
                                        workingUnit.WUTO.Name = stringone;
                                        List<WorkingUnitTO> workingUnits = workingUnit.SearchExact();

                                        if (workingUnits.Count <= 0)
                                        {
                                            error = "CostCenter for stringone: " + stringone + " not found";
                                        }

                                        WorkingUnitTO companyWU = new WorkingUnitTO();
                                        if (osTO.CompanyCode != "")
                                        {
                                            workingUnit.WUTO.Name = osTO.CompanyCode;
                                            List<WorkingUnitTO> workingUnitsCompany = workingUnit.SearchExact();
                                            if (workingUnitsCompany.Count <= 0)
                                                error = "Company for company_code: " + osTO.CompanyCode + " not found";
                                            else
                                                companyWU = workingUnitsCompany[0];
                                        }

                                        if (error.Trim().Equals(""))
                                        {
                                            wuXou.WUXouTO = new WorkingUnitXOrganizationalUnitTO();
                                            wuXou.WUXouTO.OrgUnitID = osTO.UnitID;
                                            List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                                            if (list.Count > 0)
                                            {
                                                delWUTO = list[0];
                                                if (osTO.CompanyCode == "")
                                                    companyWU = workingUnit.FindWU(Common.Misc.getRootWorkingUnit(list[0].WorkingUnitID, wuDIct));
                                            }
                                            foreach (WorkingUnitTO wuTO in workingUnits)
                                            {
                                                int compID = Common.Misc.getRootWorkingUnit(wuTO.WorkingUnitID, wuDIct);
                                                if (compID == companyWU.WorkingUnitID)
                                                {
                                                    newWUTO = wuTO;
                                                }
                                            }
                                            if (newWUTO.WorkingUnitID == -1)
                                            {
                                                error = "Working unit not found for stringone: " + stringone + "; and company_code: " + osTO.CompanyCode;
                                            }
                                            else
                                            {
                                                //wuXou.WUXouTO.OrgUnitID = orgUnitTO.OrgUnitID;
                                                wuXou.WUXouTO.WorkingUnitID = newWUTO.WorkingUnitID;
                                            }
                                        }
                                    }
                                }

                                if (error.Trim().Equals(""))
                                {
                                    // if parent org unit is changed
                                    if (osTO.ParentUnitID != -1 && orgUnitTO.ParentOrgUnitID != osTO.ParentUnitID)
                                    {                                        
                                        dataChange = true;
                                        orgUnit.OrgUnitTO = new OrganizationalUnitTO();
                                        orgUnit.OrgUnitTO.OrgUnitID = osTO.ParentUnitID;
                                        List<OrganizationalUnitTO> orgUnits = orgUnit.Search();

                                        if (orgUnits.Count <= 0) //&& !newUnits.Contains(osTO.ParentUnitID))
                                            error = "Parent Organizational unit for id= " + osTO.ParentUnitID.ToString() + " not found";
                                        else
                                            orgUnitTO.ParentOrgUnitID = osTO.ParentUnitID;

                                        if (error.Trim().Equals(""))
                                        {
                                            //if parent changed, delete all visibility from old parent under unit and all its children (do not delete if user is responsible for some of units)

                                            // find all children of changing unit
                                            orgUnit.OrgUnitTO = new OrganizationalUnitTO();
                                            List<OrganizationalUnitTO> childUnits = new List<OrganizationalUnitTO>();
                                            childUnits.Add(orgUnitTO);
                                            childUnits = orgUnit.FindAllChildren(childUnits);

                                            string childIDs = "";
                                            foreach (OrganizationalUnitTO child in childUnits)
                                            {
                                                childIDs += child.OrgUnitID.ToString().Trim() + ",";
                                            }

                                            if (childIDs.Length > 0)
                                                childIDs = childIDs.Substring(0, childIDs.Length - 1);

                                            // get all users that have unit and its children units visibility
                                            List<ApplUserXOrgUnitTO> ouVisibility = new List<ApplUserXOrgUnitTO>();
                                            auXou.AuXOUnitTO = new ApplUserXOrgUnitTO();                                            
                                            ouVisibility = auXou.Search(childIDs);

                                            Dictionary<string, List<int>> responsibleUsersUnits = new Dictionary<string, List<int>>();

                                            foreach (OrganizationalUnitTO child in childUnits)
                                            {
                                                // get all responsible users for this unit
                                                Dictionary<int, List<int>> emplResponsible = emplResponsibility.SearchUnitsResponsibilitiesByEmployee(child.OrgUnitID, Constants.emplResTypeOU);

                                                if (emplResponsible.Count > 0)
                                                {
                                                    string emplIDs = "";
                                                    foreach (int emplID in emplResponsible.Keys)
                                                    {
                                                        emplIDs = emplID.ToString().Trim() + ",";
                                                    }

                                                    if (emplIDs.Length > 0)
                                                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                                                    asco.EmplAsco4TO = new EmployeeAsco4TO();
                                                    Dictionary<int, EmployeeAsco4TO> ascoDict = asco.SearchDictionary(emplIDs);

                                                    foreach (int emplID in emplResponsible.Keys)
                                                    {
                                                        if (ascoDict.ContainsKey(emplID) && !ascoDict[emplID].NVarcharValue5.Trim().Equals(""))
                                                        {
                                                            List<OrganizationalUnitTO> childList = new List<OrganizationalUnitTO>();
                                                            childList.Add(child);
                                                            orgUnit = new OrganizationalUnit(ACTAConnection);
                                                            childList = orgUnit.FindAllChildren(childList);
                                                            List<int> unitIDs = new List<int>();
                                                            foreach (OrganizationalUnitTO chUnit in childList)
                                                            {
                                                                unitIDs.Add(chUnit.OrgUnitID);
                                                            }

                                                            if (!responsibleUsersUnits.ContainsKey(ascoDict[emplID].NVarcharValue5.Trim()))
                                                                responsibleUsersUnits.Add(ascoDict[emplID].NVarcharValue5.Trim(), unitIDs);
                                                            else
                                                                responsibleUsersUnits[ascoDict[emplID].NVarcharValue5].AddRange(unitIDs);
                                                        }
                                                    }
                                                }
                                            }

                                            foreach (ApplUserXOrgUnitTO uXo in ouVisibility)
                                            {
                                                if (responsibleUsersUnits.ContainsKey(uXo.UserID) && responsibleUsersUnits[uXo.UserID].Contains(uXo.OrgUnitID))
                                                    continue;

                                                applUsersXOUDel.Add(uXo);
                                            }

                                            // add visibility, if does not already exist, under unit and its children for all users that have new parent visibility
                                            // get all users that can see new parent                                             
                                            List<ApplUserXOrgUnitTO> ouNewVisibility = new List<ApplUserXOrgUnitTO>();
                                            auXou.AuXOUnitTO = new ApplUserXOrgUnitTO();
                                            auXou.AuXOUnitTO.OrgUnitID = orgUnitTO.ParentOrgUnitID;
                                            ouNewVisibility = auXou.Search();

                                            foreach (ApplUserXOrgUnitTO ouVis in ouNewVisibility)
                                            {
                                                foreach (OrganizationalUnitTO chUnit in childUnits)
                                                {
                                                    if (responsibleUsersUnits.ContainsKey(ouVis.UserID) && responsibleUsersUnits[ouVis.UserID].Contains(chUnit.OrgUnitID))
                                                        continue;

                                                    ApplUserXOrgUnitTO newOUVisibility = new ApplUserXOrgUnitTO();
                                                    newOUVisibility.OrgUnitID = chUnit.OrgUnitID;
                                                    newOUVisibility.Purpose = ouVis.Purpose;
                                                    newOUVisibility.UserID = ouVis.UserID;

                                                    applUsersXOUInsert.Add(newOUVisibility);
                                                }
                                            }                                            
                                        }
                                    }
                                }                                
                            }
                            else //NEW ORGNIZATIONAL UNIT
                            {
                                dataChange = true;
                                WorkingUnitTO newWUTO = new WorkingUnitTO();
                                applUsersXOUInsert = new List<ApplUserXOrgUnitTO>();
                                orgUnitTO.OrgUnitID = osTO.UnitID;
                                //if description or code changed and date time is now or passed update working unit description or code
                                if (osTO.Description != "")
                                {
                                    orgUnitTO.Desc = osTO.Description;
                                    orgUnitTO.Name = osTO.Description;
                                    //orgUnit.OrgUnitTO = new OrganizationalUnitTO();
                                    //orgUnit.OrgUnitTO.Name = orgUnitTO.Name;
                                    //List<OrganizationalUnitTO> units = orgUnit.Search();
                                    //if (units.Count > 0)
                                    //{
                                    //    error = "Organizational unit name must be uniqe. Unit with same name already exist.";
                                    //}
                                }
                                else
                                {
                                    error = "Can not insert organizational unit without description. Org unit id = " + orgUnitTO.OrgUnitID.ToString();
                                }

                                if (error.Trim().Equals(""))
                                {
                                    if (osTO.Code != "")
                                        orgUnitTO.Code = osTO.Code;

                                    //if cost center stringone changed update WorkingUnitXOrganizationalUnit
                                    if (osTO.CostCenterStringone != "" && osTO.CompanyCode != "")
                                    {
                                        if (osTO.CompanyCode == "1367")
                                            osTO.CostCenterStringone = osTO.CostCenterStringone.Substring(3);

                                        if (osTO.CostCenterStringone.Trim().Length < 3)
                                        {
                                            error = "Cost center stringone invalid lenght";
                                            WriteLog(" SyncOrganizationalStructure(): Cost center stringone invalid lenght for rec id = " + osTO.RecID);
                                        }

                                        if (error.Trim().Equals(""))
                                        {
                                            string stringone = osTO.CostCenterStringone.Substring(0, 3) + "." + osTO.CostCenterStringone.Substring(3);

                                            workingUnit.WUTO.Name = stringone;
                                            List<WorkingUnitTO> workingUnits = workingUnit.SearchExact();
                                            if (workingUnits.Count <= 0)
                                            {
                                                error = "CostCenter for stringone: " + stringone + " not found";
                                            }

                                            workingUnit.WUTO.Name = osTO.CompanyCode;
                                            WorkingUnitTO companyWU = new WorkingUnitTO();
                                            List<WorkingUnitTO> workingUnitsCompany = workingUnit.SearchExact();
                                            if (workingUnitsCompany.Count <= 0)
                                                error = "Company for company_code: " + osTO.CompanyCode + " not found";
                                            else
                                                companyWU = workingUnitsCompany[0];
                                            foreach (WorkingUnitTO wuTO in workingUnits)
                                            {
                                                int compID = Common.Misc.getRootWorkingUnit(wuTO.WorkingUnitID, wuDIct);
                                                if (compID == companyWU.WorkingUnitID)
                                                {
                                                    newWUTO = wuTO;
                                                }
                                            }
                                            if (newWUTO.WorkingUnitID == -1)
                                            {
                                                error = "Working unit not found for stringone: " + stringone + "; and company_code: " + osTO.CompanyCode;
                                            }
                                            else
                                            {
                                                wuXou.WUXouTO.OrgUnitID = orgUnitTO.OrgUnitID;
                                                wuXou.WUXouTO.WorkingUnitID = newWUTO.WorkingUnitID;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        error = "Working unit can not be null ";
                                    }

                                    if (error.Trim().Equals(""))
                                    {
                                        if (osTO.ParentUnitID != -1)
                                        {
                                            if (orgUnitTO.OrgUnitID != osTO.ParentUnitID)
                                            {
                                                orgUnit.OrgUnitTO = new OrganizationalUnitTO();
                                                orgUnit.OrgUnitTO.OrgUnitID = osTO.ParentUnitID;
                                                List<OrganizationalUnitTO> orgUnits = orgUnit.Search();

                                                if (orgUnits.Count <= 0) // && !newUnits.Contains(osTO.ParentUnitID))
                                                    error = "Parent Organizational unit for id= " + osTO.ParentUnitID.ToString() + " not found";
                                                else
                                                    orgUnitTO.ParentOrgUnitID = osTO.ParentUnitID;

                                                if (error.Trim().Equals(""))
                                                {
                                                    // get users that should be given visibility under new made unit
                                                    // add all users from new parent visibility
                                                    List<ApplUserXOrgUnitTO> ouVisibility = new List<ApplUserXOrgUnitTO>();

                                                    auXou.AuXOUnitTO = new ApplUserXOrgUnitTO();
                                                    auXou.AuXOUnitTO.OrgUnitID = orgUnitTO.ParentOrgUnitID;
                                                    ouVisibility = auXou.Search();

                                                    foreach (ApplUserXOrgUnitTO ouVis in ouVisibility)
                                                    {
                                                        ApplUserXOrgUnitTO newOUVisibility = new ApplUserXOrgUnitTO();
                                                        newOUVisibility.OrgUnitID = orgUnitTO.OrgUnitID;
                                                        newOUVisibility.Purpose = ouVis.Purpose;
                                                        newOUVisibility.UserID = ouVis.UserID;

                                                        applUsersXOUInsert.Add(newOUVisibility);
                                                    }
                                                }
                                            }
                                            else
                                                orgUnitTO.ParentOrgUnitID = osTO.ParentUnitID;
                                        }
                                        else
                                        {
                                            error = "No parent_unit_id value found for unit_id = " + osTO.UnitID;
                                        }
                                    }
                                }
                            }

                            #region DB update
                            //begin ACTA transaction
                            bool trans = orgUnit.BeginTransaction();
                            if (trans)
                            {
                                try
                                {
                                    orgUnit.OrgUnitTO = orgUnitTO;                                    
                                    bool succ = true;
                                    if (error == "" && dataChange)
                                    {
                                        if (unitExist)
                                            succ = orgUnit.Update(false);
                                        else
                                        {
                                            orgUnit.OrgUnitTO.Status = Constants.statusActive;
                                            succ = orgUnit.Save(false) > 0;
                                        }                                        
                                        if (wuXou.WUXouTO.WorkingUnitID != -1)
                                        {
                                            wuXou.SetTransaction(orgUnit.GetTransaction());
                                            wuXou.WUXouTO.OrgUnitID = orgUnit.OrgUnitTO.OrgUnitID;
                                            wuXou.WUXouTO.WorkingUnitID = wuXou.WUXouTO.WorkingUnitID;
                                            //delete old working unit connection if exists
                                            if (delWUTO.OrgUnitID != -1)
                                                succ = succ && wuXou.Delete(delWUTO.OrgUnitID, delWUTO.WorkingUnitID, false);
                                            //insert new working unit connection
                                            succ = succ && wuXou.Save(wuXou.WUXouTO, false) > 0;
                                        }
                                        auXou.SetTransaction(orgUnit.GetTransaction());
                                        if (applUsersXOUDel.Count > 0)
                                        {
                                            foreach (ApplUserXOrgUnitTO applUserXOUTO in applUsersXOUDel)
                                            {
                                                succ = succ && auXou.Delete(applUserXOUTO.UserID, applUserXOUTO.OrgUnitID, applUserXOUTO.Purpose, false);
                                            }
                                        }
                                        if (applUsersXOUInsert.Count > 0)
                                        {
                                            foreach (ApplUserXOrgUnitTO applUserXOUTO in applUsersXOUInsert)
                                            {
                                                if (applUserXOUTO.Purpose.Trim() == "")
                                                    applUserXOUTO.Purpose = Constants.EmployeesPurpose;
                                                succ = succ && auXou.Save(applUserXOUTO.UserID, applUserXOUTO.OrgUnitID, applUserXOUTO.Purpose, false) > 0;
                                            }
                                        }
                                    }
                                    if (succ)
                                    {
                                        syncOS.SetTransaction(orgUnit.GetTransaction());
                                        syncOS.OSTO = osTO;
                                        if (error == "")
                                            syncOS.OSTO.Result = Constants.resultSucc;
                                        else
                                        {
                                            WriteLog(" SyncOrganizationalStructure - " + error);
                                            syncOS.OSTO.Result = Constants.resultFaild;
                                            syncOS.OSTO.Remark = error;
                                        }
                                        succ = syncOS.insert(false);
                                        if (succ)
                                        {
                                            //before commit ACTA transaction try to delete record from sync DB
                                            succ = syncDAO.delSyncOrganizationalStructure(osTO);
                                            if (succ)
                                            {
                                                orgUnit.CommitTransaction();
                                            }
                                            else
                                            {
                                                orgUnit.RollbackTransaction();
                                                WriteLog(" SyncOrganizationalStructure() Delete record from sync_organizational_structure fail for rec_id = " + osTO.RecID.ToString());
                                            }
                                        }
                                        else
                                        {
                                            syncOS.RollbackTransaction();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (orgUnit.GetTransaction() != null)
                                        orgUnit.RollbackTransaction();
                                    WriteLog(" SyncOrganizationalStructure() exception: " + ex.Message);
                                }
                            }
                            else
                            {
                                WriteLog(" SyncOrganizationalStructure() ACTA DB begin transaction faild! ");
                            }
                            #endregion DB                            
                        }
                        catch (Exception ex)
                        {
                            WriteLog(" SyncOrganizationalStructure() exception: " + ex.Message + "; for rec_id = " + osTO.RecID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(" SyncOrganizacionalStructure() exception: " + ex.Message);
            }
        }

        private void SyncOrganizacionalStructureOld()
        {
            try
            {
                //get all records from sync_organizational_structures
                List<SyncOrganizationalStructureTO> osRecords = syncDAO.getDifOrganizationalStructures();
                List<int> newUnits = new List<int>();
                List<int> newUnitsErrors = new List<int>();
                foreach (SyncOrganizationalStructureTO structTO in osRecords)
                    newUnits.Add(structTO.UnitID);
                //if there is no records continue
                if (osRecords.Count <= 0)
                {
                    WriteLog(" SyncOrganizationalStructure - Records not found. ");
                }
                //if records found begin processing
                else
                {
                    WriteLog(" SyncOrganizationalStructure - Num of records found: " + osRecords.Count.ToString());

                    OrganizationalUnit orgUnit = new OrganizationalUnit(ACTAConnection);
                    SyncOrganizationalStructureHist syncOS = new SyncOrganizationalStructureHist(ACTAConnection);
                    WorkingUnit workingUnit = new WorkingUnit(ACTAConnection);
                    WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(ACTAConnection);
                    ApplUserXOrgUnit auXou = new ApplUserXOrgUnit(ACTAConnection);

                    List<ApplUserXOrgUnitTO> applUsersXOUDel = new List<ApplUserXOrgUnitTO>();
                    List<ApplUserXOrgUnitTO> applUsersXOUInsert = new List<ApplUserXOrgUnitTO>();

                    int count = 0;
                    //process records one by one
                    foreach (SyncOrganizationalStructureTO osTO in osRecords)
                    {
                        count++;
                        try
                        {
                            OrganizationalUnitTO orgUnitTO = orgUnit.Find(osTO.UnitID);
                            bool unitExist = false;
                            if (orgUnitTO.OrgUnitID >= 0)
                            {
                                unitExist = true;
                            }
                            //if unit exist
                            if (unitExist)
                            {
                                bool dataChange = false;
                                string error = "";
                                WorkingUnitTO newWUTO = new WorkingUnitTO();
                                WorkingUnitXOrganizationalUnitTO delWUTO = new WorkingUnitXOrganizationalUnitTO();


                                //if description or code changed and date time is now or passed update working unit description or code
                                if ((osTO.Description != "" && osTO.ValidFrom.Date <= DateTime.Now.Date)
                                    || (osTO.Code != "" && osTO.ValidFrom.Date <= DateTime.Now.Date)
                                    || (osTO.Status != "" && osTO.ValidFrom.Date <= DateTime.Now.Date))
                                {
                                    dataChange = true;
                                    if (osTO.Description != "" )
                                    {
                                        orgUnitTO.Desc = osTO.Description;
                                        orgUnitTO.Name = osTO.Description;
                                    }
                                    if (osTO.Code != "")
                                        orgUnitTO.Code = osTO.Code;
                                    if (osTO.Status != "")
                                        orgUnitTO.Status = osTO.Status;
                                }
                                //if cost center stringone changed update WorkingUnitXOrganizationalUnit
                                if (osTO.CostCenterStringone != "" || osTO.CompanyCode != "")
                                {
                                    dataChange = true;
                                    Dictionary<int, WorkingUnitTO> wuDIct = workingUnit.getWUDictionary();
                                    string stringone = osTO.CostCenterStringone.Substring(0, 3) + "." + osTO.CostCenterStringone.Substring(3);
                                    workingUnit.WUTO.Name = stringone;
                                    List<WorkingUnitTO> workingUnits = workingUnit.SearchExact();

                                    if (workingUnits.Count <= 0)
                                    {
                                        if (osTO.CompanyCode == "1367")
                                        {
                                            stringone = "000.0000";
                                            workingUnit.WUTO.Name = stringone;
                                            workingUnits = workingUnit.SearchExact();
                                        }
                                        if (workingUnits.Count <= 0)
                                        {
                                            error = "CostCenter for stringone: " + stringone + " not found";
                                        }
                                    }
                                    WorkingUnitTO companyWU = new WorkingUnitTO();
                                    if (osTO.CompanyCode != "")
                                    {
                                        workingUnit.WUTO.Name = osTO.CompanyCode;
                                        List<WorkingUnitTO> workingUnitsCompany = workingUnit.SearchExact();
                                        if (workingUnitsCompany.Count <= 0)
                                            error = "Company for company_code: " + osTO.CompanyCode + " not found";
                                        else
                                            companyWU = workingUnitsCompany[0];
                                    }
                                    wuXou.WUXouTO = new WorkingUnitXOrganizationalUnitTO();
                                    wuXou.WUXouTO.OrgUnitID = osTO.UnitID;
                                    List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                                    if (list.Count > 0)
                                    {
                                        delWUTO = list[0];
                                        if (osTO.CompanyCode == "")
                                            companyWU = workingUnit.FindWU(Common.Misc.getRootWorkingUnit(list[0].WorkingUnitID, wuDIct));
                                    }
                                    foreach (WorkingUnitTO wuTO in workingUnits)
                                    {
                                        int compID = Common.Misc.getRootWorkingUnit(wuTO.WorkingUnitID, wuDIct);
                                        if (compID == companyWU.WorkingUnitID)
                                        {
                                            newWUTO = wuTO;
                                        }
                                    }
                                    if (newWUTO.WorkingUnitID == -1)
                                    {
                                        error = "Working unit not found for stringone: " + stringone + "; and company_code: " + osTO.CompanyCode;
                                    }
                                    wuXou.WUXouTO.OrgUnitID = orgUnitTO.OrgUnitID;
                                }
                                //if parent org unit changed delete all appl_user_x_oragnizational_units from old parent and add all from new parent
                                if (osTO.ParentUnitID != -1 && osTO.ValidFrom.Date <= DateTime.Now.Date)
                                {
                                    dataChange = true;

                                    orgUnit.OrgUnitTO = new OrganizationalUnitTO();
                                    orgUnit.OrgUnitTO.OrgUnitID = osTO.ParentUnitID;
                                    List<OrganizationalUnitTO> orgUnits = orgUnit.Search();

                                    if (orgUnits.Count <= 0 && !newUnits.Contains(osTO.ParentUnitID))
                                        error = "Parent Organizational unit for id= " + osTO.ParentUnitID.ToString() + " not found";

                                    orgUnitTO.ParentOrgUnitID = osTO.UnitID;
                                  

                                }
                                //begin ACTA transaction
                                bool trans = orgUnit.BeginTransaction();
                                if (trans)
                                {
                                    try
                                    {
                                        orgUnit.OrgUnitTO = orgUnitTO;
                                        bool succ = true;
                                        if (error == "" && dataChange)
                                        {
                                            succ = orgUnit.Update(false);
                                            if (newWUTO.WorkingUnitID != -1)
                                            {
                                                wuXou.SetTransaction(orgUnit.GetTransaction());
                                                wuXou.WUXouTO.OrgUnitID = orgUnit.OrgUnitTO.OrgUnitID;
                                                wuXou.WUXouTO.WorkingUnitID = newWUTO.WorkingUnitID;
                                                //delete old working unit connection
                                                succ = succ && wuXou.Delete(delWUTO.OrgUnitID, delWUTO.WorkingUnitID,false);
                                                //insert new working unit connection
                                                succ = succ && wuXou.Save(wuXou.WUXouTO, false) > 0;
                                            }
                                            auXou.SetTransaction(orgUnit.GetTransaction());
                                            if (applUsersXOUDel.Count > 0)
                                            {
                                                foreach (ApplUserXOrgUnitTO applUserXOUTO in applUsersXOUDel)
                                                {
                                                    succ = succ && auXou.Delete(applUserXOUTO.UserID, applUserXOUTO.OrgUnitID, applUserXOUTO.Purpose, false);
                                                }
                                            }
                                            if (applUsersXOUInsert.Count > 0)
                                            {
                                                foreach (ApplUserXOrgUnitTO applUserXOUTO in applUsersXOUInsert)
                                                {
                                                    if (applUserXOUTO.Purpose.Trim() == "")
                                                        applUserXOUTO.Purpose = Constants.EmployeesPurpose;
                                                    succ = succ && auXou.Save(applUserXOUTO.UserID, applUserXOUTO.OrgUnitID, applUserXOUTO.Purpose, false) > 0;
                                                }
                                            }
                                        }
                                        if (succ)
                                        {
                                            syncOS.SetTransaction(orgUnit.GetTransaction());
                                            syncOS.OSTO = osTO;
                                            if (error == "")
                                                syncOS.OSTO.Result = Constants.resultSucc;
                                            else
                                            {
                                                WriteLog(" SyncOrganizationalStructure - " + error);
                                                syncOS.OSTO.Result = Constants.resultFaild;
                                                syncOS.OSTO.Remark = error;
                                            }
                                            succ = syncOS.insert(false);
                                            if (succ)
                                            {
                                                //before commit ACTA transaction try to delete record from sync DB
                                                succ = syncDAO.delSyncOrganizationalStructure(osTO);
                                                if (succ)
                                                {
                                                    orgUnit.CommitTransaction();
                                                }
                                                else
                                                {
                                                    orgUnit.RollbackTransaction();
                                                    WriteLog(" SyncOrganizationalStructure() Delete record from sync_organizational_structure fail for rec_id = " + osTO.RecID.ToString());
                                                }
                                            }
                                            else
                                            {
                                                syncOS.RollbackTransaction();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (orgUnit.GetTransaction() != null)
                                            orgUnit.RollbackTransaction();
                                        WriteLog(" SyncOrganizationalStructure() exception: " + ex.Message);
                                    }
                                }
                                else
                                {
                                    WriteLog(" SyncOrganizationalStructure() ACTA DB begin transaction faild! ");
                                }
                            }
                            else//NEW ORGNAIZATIONAL UNIT
                            {
                                if (osTO.ValidFrom.Date > DateTime.Now.Date )
                                    continue;
                                string error = "";
                                WorkingUnitTO newWUTO = new WorkingUnitTO();
                                applUsersXOUInsert = new List<ApplUserXOrgUnitTO>();
                                orgUnitTO.OrgUnitID = osTO.UnitID;
                                //if description or code changed and date time is now or passed update working unit description or code
                                if (osTO.Description != "")
                                {
                                    orgUnitTO.Desc = osTO.Description;
                                    orgUnitTO.Name = osTO.Description;
                                    //orgUnit.OrgUnitTO = new OrganizationalUnitTO();
                                    //orgUnit.OrgUnitTO.Name = orgUnitTO.Name;
                                    //List<OrganizationalUnitTO> units = orgUnit.Search();
                                    //if (units.Count > 0)
                                    //{
                                    //    error = "Organizational unit name must be uniqe. Unit with same name already exist.";
                                    //}
                                }
                                else
                                {
                                    error = "Can not insert organizational unit without description. Org unit id = " + orgUnitTO.OrgUnitID.ToString();
                                }
                                if (osTO.Code != "")
                                    orgUnitTO.Code = osTO.Code;

                                //if cost center stringone changed update WorkingUnitXOrganizationalUnit
                                if (osTO.CostCenterStringone != "" && osTO.CompanyCode != "")
                                {
                                    Dictionary<int, WorkingUnitTO> wuDIct = workingUnit.getWUDictionary();
                                    string stringone = osTO.CostCenterStringone.Substring(0, 3) + "." + osTO.CostCenterStringone.Substring(3);
                                    
                                    workingUnit.WUTO.Name = stringone;
                                    List<WorkingUnitTO> workingUnits = workingUnit.SearchExact();
                                    if (workingUnits.Count <= 0)
                                    {
                                        if (osTO.CompanyCode == "1367")
                                        {
                                            stringone = "000.0000";
                                            workingUnit.WUTO.Name = stringone;
                                            workingUnits = workingUnit.SearchExact();                                            
                                        }
                                        if (workingUnits.Count <= 0)
                                        {
                                            error = "CostCenter for stringone: " + stringone + " not found";
                                        }
                                    }
                                    workingUnit.WUTO.Name = osTO.CompanyCode;
                                    WorkingUnitTO companyWU = new WorkingUnitTO();
                                    List<WorkingUnitTO> workingUnitsCompany = workingUnit.SearchExact();
                                    if (workingUnitsCompany.Count <= 0)
                                        error = "Company for company_code: " + osTO.CompanyCode + " not found";
                                    else
                                        companyWU = workingUnitsCompany[0];
                                    foreach (WorkingUnitTO wuTO in workingUnits)
                                    {
                                        int compID = Common.Misc.getRootWorkingUnit(wuTO.WorkingUnitID, wuDIct);
                                        if (compID == companyWU.WorkingUnitID)
                                        {
                                            newWUTO = wuTO;
                                        }
                                    }
                                    if (newWUTO.WorkingUnitID == -1)
                                    {
                                        error = "Working unit not found for stringone: " + stringone + "; and company_code: " + osTO.CompanyCode;
                                    }
                                    wuXou.WUXouTO.OrgUnitID = orgUnitTO.OrgUnitID;
                                }
                                else
                                {
                                    error = "Working unit can not be null ";
                                }
                                //if parent org unit changed delete all appl_user_x_oragnizational_units from old parent and add all from new parent
                                if (osTO.ParentUnitID != -1)
                                {
                                    orgUnit.OrgUnitTO = new OrganizationalUnitTO();
                                    orgUnit.OrgUnitTO.OrgUnitID = osTO.ParentUnitID;
                                    List<OrganizationalUnitTO> orgUnits = orgUnit.Search();

                                    if (orgUnits.Count <= 0 && !newUnits.Contains(osTO.ParentUnitID))
                                        error = "Parent Organizational unit for id= " + osTO.ParentUnitID.ToString() + " not found";

                                   
                                    orgUnitTO.ParentOrgUnitID = osTO.UnitID;                                   

                                }
                                else
                                {
                                    error = "No parent_unit_id value found for unit_id = " + osTO.UnitID;
                                }
                                //begin ACTA transaction
                                bool trans = orgUnit.BeginTransaction();
                                if (trans)
                                {
                                    try
                                    {
                                        orgUnitTO.Status = Constants.statusActive;
                                        orgUnit.OrgUnitTO = orgUnitTO;
                                        bool succ = true;
                                        if (error == "")
                                        {
                                            succ = orgUnit.Save(false) > 0;
                                            if (newWUTO.WorkingUnitID != -1)
                                            {
                                                wuXou.SetTransaction(orgUnit.GetTransaction());
                                                wuXou.WUXouTO.OrgUnitID = orgUnit.OrgUnitTO.OrgUnitID;
                                                wuXou.WUXouTO.WorkingUnitID = newWUTO.WorkingUnitID;
                                                //insert new working unit connection
                                                succ = succ && wuXou.Save(wuXou.WUXouTO, false) > 0;
                                            }
                                            auXou.SetTransaction(orgUnit.GetTransaction());
                                            if (applUsersXOUInsert.Count > 0)
                                            {
                                                foreach (ApplUserXOrgUnitTO applUserXOUTO in applUsersXOUInsert)
                                                {
                                                    if(applUserXOUTO.Purpose.Trim() == "")
                                                        applUserXOUTO.Purpose = Constants.EmployeesPurpose;
                                                    succ = succ && auXou.Save(applUserXOUTO.UserID, applUserXOUTO.OrgUnitID, applUserXOUTO.Purpose, false) > 0;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!newUnitsErrors.Contains(osTO.UnitID))
                                                newUnitsErrors.Add(osTO.UnitID);
                                        }
                                        if (succ)
                                        {
                                            syncOS.SetTransaction(orgUnit.GetTransaction());
                                            syncOS.OSTO = osTO;
                                            if (error == "")
                                                syncOS.OSTO.Result = Constants.resultSucc;
                                            else
                                            {
                                                WriteLog(" SyncOrganizationalStructure - " + error);
                                                syncOS.OSTO.Result = Constants.resultFaild;
                                                syncOS.OSTO.Remark = error;
                                            }
                                            succ = syncOS.insert(false);
                                            if (succ)
                                            {
                                                //before commit ACTA transaction try to delete record from sync DB
                                                succ = syncDAO.delSyncOrganizationalStructure(osTO);
                                                if (succ)
                                                {
                                                    orgUnit.CommitTransaction();
                                                }
                                                else
                                                {
                                                    orgUnit.RollbackTransaction();
                                                    WriteLog(" SyncOrganizationalStructure() Delete record from sync_organizational_structure fail for rec_id = " + osTO.RecID.ToString());
                                                }
                                            }
                                            else
                                            {
                                                orgUnit.RollbackTransaction();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (orgUnit.GetTransaction() != null)
                                            orgUnit.RollbackTransaction();
                                        WriteLog(" SyncOrganizationalStructure() exception: " + ex.Message);
                                    }
                                }
                                else
                                {
                                    WriteLog(" SyncOrganizationalStructure() ACTA DB begin transaction faild! ");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog(" SyncOrganizationalStructure() exception: " + ex.Message + "; for rec_id = " + osTO.RecID);
                        }
                        SetManagerThreadStatus("Synchronization Organizational Structure: " + count.ToString() + "/" + osRecords.Count.ToString());
                    }

                      //process records one by one
                    foreach (SyncOrganizationalStructureTO osTO in osRecords)
                    {
                        try
                        {
                            //if parent org unit changed delete all appl_user_x_oragnizational_units from old parent and add all from new parent
                            if (osTO.ParentUnitID != -1 && !newUnitsErrors.Contains(osTO.UnitID))
                            {

                                OrganizationalUnitTO orgUnitTO = orgUnit.Find(osTO.UnitID);
                                if (orgUnitTO.OrgUnitID == -1)
                                {
                                    continue;
                                }
                                int oldParent = orgUnitTO.ParentOrgUnitID;
                                orgUnitTO.ParentOrgUnitID = osTO.ParentUnitID;

                                //get all appl_user_x_org_units to del from old parent
                                auXou.AuXOUnitTO.OrgUnitID = oldParent;
                                List<ApplUserXOrgUnitTO> applUsersXOUParent = auXou.Search();

                                auXou.AuXOUnitTO.OrgUnitID = orgUnitTO.OrgUnitID;
                                List<ApplUserXOrgUnitTO> applUsersXOUChild = auXou.Search();


                                //get all user rights from new parent to insert
                                auXou.AuXOUnitTO.OrgUnitID = orgUnitTO.ParentOrgUnitID;
                                List<ApplUserXOrgUnitTO> applUsersXWUParent = auXou.Search();

                                auXou.AuXOUnitTO.OrgUnitID = orgUnitTO.OrgUnitID;
                                List<ApplUserXOrgUnitTO> applUsersXWUChild = auXou.Search();

                                applUsersXOUInsert = new List<ApplUserXOrgUnitTO>();

                                foreach (ApplUserXOrgUnitTO userWUP in applUsersXWUParent)
                                {
                                    bool notExists = true;
                                    foreach (ApplUserXOrgUnitTO userWUC in applUsersXWUChild)
                                    {
                                        if ((userWUP.UserID == userWUC.UserID) && (userWUP.Purpose == userWUC.Purpose))
                                        {
                                            notExists = false;
                                            break;
                                        }
                                    }

                                    if (notExists)
                                    {
                                        ApplUserXOrgUnitTO newAUXOU = new ApplUserXOrgUnitTO();
                                        newAUXOU.UserID = userWUP.UserID;
                                        newAUXOU.Purpose = userWUP.Purpose;
                                        newAUXOU.OrgUnitID = orgUnitTO.OrgUnitID;
                                        applUsersXOUInsert.Add(newAUXOU);
                                    }
                                }

                                //begin ACTA transaction
                                bool trans = orgUnit.BeginTransaction();
                                if (trans)
                                {
                                    try
                                    {
                                        orgUnitTO.Status = Constants.statusActive;
                                        orgUnit.OrgUnitTO = orgUnitTO;
                                        bool succ = true;
                                       
                                        succ = orgUnit.Update(false);
                                        auXou.SetTransaction(orgUnit.GetTransaction());
                                        if (applUsersXOUDel.Count > 0)
                                        {
                                            foreach (ApplUserXOrgUnitTO applUserXOUTO in applUsersXOUDel)
                                            {
                                                succ = succ && auXou.Delete(applUserXOUTO.UserID, applUserXOUTO.OrgUnitID, applUserXOUTO.Purpose, false);
                                            }
                                        }
                                        if (applUsersXOUInsert.Count > 0)
                                        {
                                            foreach (ApplUserXOrgUnitTO applUserXOUTO in applUsersXOUInsert)
                                            {
                                                if (applUserXOUTO.Purpose.Trim() == "")
                                                    applUserXOUTO.Purpose = Constants.EmployeesPurpose;
                                                succ = succ && auXou.Save(applUserXOUTO.UserID, applUserXOUTO.OrgUnitID, applUserXOUTO.Purpose, false) > 0;
                                            }
                                        }
                                        if (succ)
                                        {                                           
                                           orgUnit.CommitTransaction();                                               
                                        }
                                        else
                                        {
                                            orgUnit.RollbackTransaction();
                                            WriteLog(" SyncOrganizationalStructure()Update parent for sync_organizational_structure fail for rec_id = " + osTO.RecID.ToString());
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (orgUnit.GetTransaction() != null)
                                            orgUnit.RollbackTransaction();
                                        WriteLog(" SyncOrganizationalStructure() exception: " + ex.Message);
                                    }
                                }
                                else
                                {
                                    WriteLog(" SyncOrganizationalStructure() ACTA DB begin transaction faild! ");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog(" SyncOrganizationalStructure() exception: " + ex.Message + "; for rec_id = " + osTO.RecID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(" SyncOrganizacionalStructure() exception: " + ex.Message);
            }
        }
        
        private void SyncFinancialStructure()
        {
            try
            {
                //get all records from sync_finansial_structures
                List<SyncFinancialStructureTO> fsRecords = syncDAO.getDifFinancialStructures();
                
                //if there is no records continue
                if (fsRecords.Count <= 0)
                {
                    WriteLog(" SyncFinancialStructure - Records not found. ");
                }
                //if records found begin processing
                else
                {
                    WriteLog(" SyncFinancialStructure - Num of records found: " + fsRecords.Count.ToString());

                    // common objects
                    WorkingUnit workingUnit = new WorkingUnit(ACTAConnection);
                    SyncFinancialStructureHist syncFS = new SyncFinancialStructureHist(ACTAConnection);
                    ApplUsersXWU userXWU = new ApplUsersXWU(ACTAConnection);
                    EmployeeAsco4 asco = new EmployeeAsco4(ACTAConnection);
                    EmployeeResponsibility emplResponsibility = new EmployeeResponsibility(ACTAConnection);

                    int count = 0;
                    //process records one by one
                    foreach (SyncFinancialStructureTO fsTO in fsRecords)
                    {
                        count++;

                        SetManagerThreadStatus("Synchronization Financial Structure: " + count.ToString() + "/" + fsRecords.Count.ToString());

                        if (fsTO.ValidFrom.Date > DateTime.Now.Date || fsTO.ValidFrom.Equals(new DateTime()))
                            continue;

                        string error = "";
                        try
                        {
                            WorkingUnitTO companyWU = new WorkingUnitTO();
                            if (fsTO.CompanyCode.Length > 0)
                            {
                                workingUnit.WUTO = new WorkingUnitTO();
                                workingUnit.WUTO.Name = fsTO.CompanyCode;
                                List<WorkingUnitTO> companyList = workingUnit.SearchExact();
                                if (companyList.Count > 0)
                                {
                                    companyWU = companyList[0];
                                }

                                if (companyWU.WorkingUnitID == -1)
                                {
                                    error = "Invalid company code for stringone";
                                    WriteLog(" SyncFinancialStructure() Invalid company code for stringone = " + fsTO.UnitStringone);
                                }
                            }

                            WorkingUnitTO workShopWU = new WorkingUnitTO();
                            WorkingUnitTO costCenterWU = new WorkingUnitTO();
                            WorkingUnitTO plantWU = new WorkingUnitTO();
                            List<ApplUsersXWUTO> usersForWU = new List<ApplUsersXWUTO>();
                            List<ApplUsersXWUTO> usersForWUDelete = new List<ApplUsersXWUTO>();
                            List<WorkingUnitTO> allWU = new List<WorkingUnitTO>();
                            WorkingUnitTO wuTO = new WorkingUnitTO();
                            Dictionary<int, List<int>> emplResponsible = new Dictionary<int,List<int>>();
                            List<EmployeeAsco4TO> emplRespAsco = new List<EmployeeAsco4TO>();
                            List<EmployeeAsco4TO> ascoToUpdate = new List<EmployeeAsco4TO>();
                            List<WorkingUnitTO> oldCCUnits = new List<WorkingUnitTO>();
                            List<WorkingUnitTO> newCCUnits = new List<WorkingUnitTO>();
                            List<string> responsibleUsers = new List<string>();
                            List<string> oldCCDeleteUsers = new List<string>();
                            Dictionary<string, List<WorkingUnitTO>> responsibleUsersUnits = new Dictionary<string, List<WorkingUnitTO>>();

                            string companyCode = "";
                            string plantCode = "";
                            string costCenterCode = "";
                            string workShopCode = "";
                            string teamCode = "";

                            bool unitExist = false;
                            bool updateVisibility = true;

                            if (error.Trim().Equals(""))
                            {
                                // get current working units dictionary
                                workingUnit.WUTO = new WorkingUnitTO();
                                Dictionary<int, WorkingUnitTO> wuDict = workingUnit.getWUDictionary();

                                // check if working unit exists
                                if (wuDict.ContainsKey(fsTO.UnitID))
                                    wuTO = wuDict[fsTO.UnitID];
                                                             
                                if (wuTO.WorkingUnitID != -1)
                                    unitExist = true;

                                if (unitExist)//UPDATE WORKING UNIT
                                {
                                    WorkingUnitTO oldCostCenterWU = new WorkingUnitTO();
                                    
                                    string oldPlantCode = "";
                                    string oldCostCenterCode = "";
                                    string oldStringone = "";

                                    //if description changed and date time is now or passed update working unit description                                     
                                    if (fsTO.Description != "")
                                        wuTO.Description = fsTO.Description;

                                    if (fsTO.Status != "")
                                        wuTO.Status = fsTO.Status;

                                    // current unit stringone
                                    string stringone = wuTO.Name;
                                    oldStringone = wuTO.Name;

                                    // get unit company
                                    int wuRoot = Common.Misc.getRootWorkingUnit(fsTO.UnitID, wuDict);
                                    WorkingUnitTO company = new WorkingUnitTO();
                                    if (wuDict.ContainsKey(wuRoot))
                                        company = wuDict[wuRoot];

                                    //if stringone changed update parent wu_id and visibility and update employee_stringone
                                    if (fsTO.UnitStringone != "")
                                    {
                                        //if company changed do not update just insert error description into sync history table
                                        if (fsTO.UnitStringone.Length != Constants.fsUnitStringoneLenght)
                                        {
                                            error = "Invalid stringone lenght";
                                            WriteLog(" SyncFinancialStructure() Invalid stringone lenght for working unit_id = " + wuTO.WorkingUnitID + "; stringone = " + fsTO.UnitStringone);
                                        }

                                        if (error.Trim().Equals(""))
                                        {
                                            stringone = fsTO.UnitStringone;

                                            // check if unit with new stringone already exists
                                            if (oldStringone != stringone)
                                            {
                                                workingUnit.WUTO = new WorkingUnitTO();
                                                workingUnit.WUTO.Name = stringone;
                                                if (workingUnit.SearchExact().Count > 0)
                                                {
                                                    error = "Working unit exists for new stringone";
                                                    WriteLog(" SyncFinancialStructure() Working unit exists for new stringone: working unit_id = " + wuTO.WorkingUnitID + "; stringone = " + fsTO.UnitStringone);
                                                }
                                            }
                                        }
                                    }

                                    if (company.Code != fsTO.CompanyCode && fsTO.CompanyCode != "")
                                    {
                                        error = "Company changed";
                                        WriteLog(" SyncFinancialStructure() Company changed for working unit_id = " + wuTO.WorkingUnitID);
                                    }

                                    if (error.Trim().Equals(""))
                                    {
                                        // if company code is not sent, get company working unit                                                
                                        if (fsTO.CompanyCode == "")
                                            companyWU = company;

                                        plantCode = stringone.Substring(0, Constants.fsPlantCodeLenght);
                                        costCenterCode = stringone.Substring(Constants.fsPlantCodeLenght + 1, Constants.fsCostCenterCodeLenght);
                                        workShopCode = stringone.Substring(Constants.fsPlantCodeLenght + Constants.fsCostCenterCodeLenght + 2, Constants.fsWorkShopCodeLenght);
                                        teamCode = stringone.Substring(fsTO.UnitStringone.LastIndexOf('.') + 1, Constants.fsTeamCodeLenght);
                                        oldPlantCode = wuTO.Name.Substring(0, Constants.fsPlantCodeLenght);
                                        oldCostCenterCode = wuTO.Name.Substring(Constants.fsPlantCodeLenght + 1, Constants.fsCostCenterCodeLenght);

                                        // if stringone is not changed, do not change visibility
                                        if (oldStringone == stringone)
                                            updateVisibility = false;

                                        //if stringone changed just 2 last digits no need to update visibility 
                                        if (oldStringone.Substring(0, oldStringone.Length - 3) == plantCode + "." + costCenterCode + "." + workShopCode
                                            && wuTO.Code != teamCode)
                                            updateVisibility = false;

                                        if (oldStringone != stringone)
                                        {
                                            wuTO.Name = fsTO.UnitStringone;
                                            wuTO.Code = fsTO.UnitStringone.Substring(fsTO.UnitStringone.Length - 2);
                                        }

                                        // get new workshop
                                        workingUnit.WUTO = new WorkingUnitTO();
                                        workingUnit.WUTO.Name = plantCode + "." + costCenterCode + "." + workShopCode;
                                        List<WorkingUnitTO> workShopList = workingUnit.SearchExact();
                                        if (workShopList.Count > 0)
                                        {
                                            foreach (WorkingUnitTO wu in workShopList)
                                            {
                                                int ID = Common.Misc.getRootWorkingUnit(wu.WorkingUnitID, wuDict);
                                                if (companyWU.WorkingUnitID == ID)
                                                {
                                                    workShopWU = wu;
                                                }
                                            }
                                        }

                                        // get new cost center
                                        workingUnit.WUTO = new WorkingUnitTO();
                                        workingUnit.WUTO.Name = plantCode + "." + costCenterCode;
                                        List<WorkingUnitTO> CostCenterList = workingUnit.SearchExact();
                                        if (CostCenterList.Count > 0)
                                        {
                                            foreach (WorkingUnitTO wu in CostCenterList)
                                            {
                                                int ID = Common.Misc.getRootWorkingUnit(wu.WorkingUnitID, wuDict);
                                                if (companyWU.WorkingUnitID == ID)
                                                {
                                                    costCenterWU = CostCenterList[0];
                                                }
                                            }
                                        }

                                        // get new plant
                                        workingUnit.WUTO = new WorkingUnitTO();
                                        workingUnit.WUTO.Name = plantCode;
                                        List<WorkingUnitTO> plantList = workingUnit.SearchExact();
                                        if (plantList.Count > 0)
                                        {
                                            foreach (WorkingUnitTO wu in plantList)
                                            {
                                                int ID = Common.Misc.getRootWorkingUnit(wu.WorkingUnitID, wuDict);
                                                if (companyWU.WorkingUnitID == ID)
                                                {
                                                    plantWU = plantList[0];
                                                }
                                            }
                                        }

                                        // get old cost center
                                        workingUnit.WUTO = new WorkingUnitTO();
                                        workingUnit.WUTO.Name = oldPlantCode + "." + oldCostCenterCode;
                                        List<WorkingUnitTO> oldCostCenterList = workingUnit.SearchExact();
                                        if (oldCostCenterList.Count > 0)
                                        {
                                            foreach (WorkingUnitTO wu in oldCostCenterList)
                                            {
                                                int ID = Common.Misc.getRootWorkingUnit(wu.WorkingUnitID, wuDict);
                                                if (companyWU.WorkingUnitID == ID)
                                                {
                                                    oldCostCenterWU = oldCostCenterList[0];
                                                }
                                            }
                                        }

                                        // if old cost center is same as new cost center do not change visibility
                                        if (costCenterWU.WorkingUnitID != -1 && oldCostCenterWU.WorkingUnitID == costCenterWU.WorkingUnitID)
                                            updateVisibility = false;

                                        if (updateVisibility)
                                        {
                                            // add new working unit to list of units under which new visibility would be added
                                            allWU.Add(wuTO);

                                            // find all responsibilities for employees responsible for working unit and add them visibility to all hierarchy later                                                    
                                            // if there is no responsibility for any old cost center child unit, delete old cost center visibility for user later                                                    
                                            emplResponsible = emplResponsibility.SearchUnitsResponsibilitiesByEmployee(fsTO.UnitID, Constants.emplResTypeWU);

                                            // get all old cost center child units                                                    
                                            oldCCUnits = new List<WorkingUnitTO>();
                                            oldCCUnits.Add(oldCostCenterWU);
                                            workingUnit = new WorkingUnit(ACTAConnection);
                                            oldCCUnits = workingUnit.FindAllChildren(oldCCUnits);

                                            // get new cost center units if new cost center exists
                                            if (costCenterWU.WorkingUnitID != -1)
                                            {
                                                newCCUnits = new List<WorkingUnitTO>();
                                                newCCUnits.Add(costCenterWU);
                                                workingUnit = new WorkingUnit(ACTAConnection);
                                                newCCUnits = workingUnit.FindAllChildren(newCCUnits);
                                            }

                                            // get all users for employee responsible for unit and those whose visibility under old cost center should be deleted
                                            string emplIDs = "";
                                            List<int> delEmpls = new List<int>();
                                            foreach (int emplID in emplResponsible.Keys)
                                            {
                                                bool existOldCCResponsibility = false;

                                                foreach (WorkingUnitTO wuOld in oldCCUnits)
                                                {
                                                    if (wuOld.WorkingUnitID != fsTO.UnitID && emplResponsible[emplID].Contains(wuOld.WorkingUnitID))
                                                    {
                                                        existOldCCResponsibility = true;
                                                        break;
                                                    }
                                                }

                                                if (!existOldCCResponsibility)
                                                    delEmpls.Add(emplID);

                                                emplIDs += emplID.ToString().Trim() + ",";
                                            }

                                            if (emplIDs.Length > 0)
                                            {
                                                emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                                                asco.EmplAsco4TO = new EmployeeAsco4TO();
                                                List<EmployeeAsco4TO> ascoList = asco.Search(emplIDs);

                                                foreach (EmployeeAsco4TO emplAsco in ascoList)
                                                {
                                                    if (!emplAsco.NVarcharValue5.Trim().Equals(""))
                                                    {
                                                        responsibleUsers.Add(emplAsco.NVarcharValue5);

                                                        if (delEmpls.Contains(emplAsco.EmployeeID))
                                                            oldCCDeleteUsers.Add(emplAsco.NVarcharValue5);
                                                    }
                                                }
                                            }
                                        }

                                        if (oldStringone != stringone)
                                        {
                                            //foreach employee belong working unit update stringone
                                            Employee empl = new Employee(ACTAConnection);
                                            empl.EmplTO.WorkingUnitID = fsTO.UnitID;
                                            List<EmployeeTO> employeesBelong = empl.Search();
                                            foreach (EmployeeTO emplBelong in employeesBelong)
                                            {
                                                asco.EmplAsco4TO = new EmployeeAsco4TO();
                                                asco.EmplAsco4TO.EmployeeID = emplBelong.EmployeeID;
                                                List<EmployeeAsco4TO> employeeAsco = asco.Search();
                                                if (employeeAsco.Count > 0)
                                                {
                                                    EmployeeAsco4TO ascoTemp = employeeAsco[0];
                                                    ascoTemp.NVarcharValue2 = wuTO.Name + "." + ascoTemp.NVarcharValue6;
                                                    ascoTemp.EmployeeID = emplBelong.EmployeeID;
                                                    ascoToUpdate.Add(ascoTemp);
                                                }
                                            }
                                        }
                                    }
                                }
                                else //NEW WORKING UNIT                            
                                {
                                    if (fsTO.UnitStringone.Trim().Equals("") || fsTO.CompanyCode.Trim().Equals(""))
                                    {
                                        if (fsTO.UnitStringone.Trim().Equals(""))
                                        {
                                            error = "Stringone missing";
                                            WriteLog(" SyncFinancialStructure() Stringone missing for working unit_id = " + wuTO.WorkingUnitID);
                                        }

                                        if (fsTO.CompanyCode.Trim().Equals(""))
                                        {
                                            error = "Company code missing";
                                            WriteLog(" SyncFinancialStructure() Company code missing for working unit_id = " + wuTO.WorkingUnitID);
                                        }
                                    }
                                    else if (fsTO.UnitStringone.Length != Constants.fsUnitStringoneLenght)
                                    {
                                        error = "Invalid stringone lenght";
                                        WriteLog(" SyncFinancialStructure() Invalid stringone lenght for working unit_id = " + wuTO.WorkingUnitID + "; stringone = " + fsTO.UnitStringone);
                                    }

                                    if (error.Trim().Equals(""))
                                    {
                                        // create new working unit                                    
                                        wuTO.WorkingUnitID = fsTO.UnitID;
                                        wuTO.Status = Constants.statusActive;
                                        wuTO.Description = fsTO.Description;

                                        companyCode = fsTO.CompanyCode;
                                        plantCode = fsTO.UnitStringone.Substring(0, Constants.fsPlantCodeLenght);
                                        costCenterCode = fsTO.UnitStringone.Substring(Constants.fsPlantCodeLenght + 1, Constants.fsCostCenterCodeLenght);
                                        workShopCode = fsTO.UnitStringone.Substring(Constants.fsPlantCodeLenght + Constants.fsCostCenterCodeLenght + 2, Constants.fsWorkShopCodeLenght);
                                        teamCode = fsTO.UnitStringone.Substring(fsTO.UnitStringone.LastIndexOf('.') + 1, Constants.fsTeamCodeLenght);

                                        wuTO.Name = plantCode + "." + costCenterCode + "." + workShopCode + "." + teamCode;
                                        wuTO.Code = teamCode;

                                        // check if there already exist working unit with same name
                                        workingUnit.WUTO = new WorkingUnitTO();
                                        workingUnit.WUTO.Name = wuTO.Name;
                                        if (workingUnit.SearchExact().Count > 0)
                                        {
                                            error = "Working unit exists for new stringone";
                                            WriteLog(" SyncFinancialStructure() Working unit exists for new stringone: working unit_id = " + wuTO.WorkingUnitID + "; stringone = " + fsTO.UnitStringone);
                                        }

                                        if (error.Trim().Equals(""))
                                        {
                                            // find all working units in hiearchy
                                            // find workshop by name from unit stringone
                                            workingUnit.WUTO = new WorkingUnitTO();
                                            workingUnit.WUTO.Name = plantCode + "." + costCenterCode + "." + workShopCode;
                                            List<WorkingUnitTO> workShopList = workingUnit.SearchExact();
                                            if (workShopList.Count > 0)
                                            {
                                                foreach (WorkingUnitTO wu in workShopList)
                                                {
                                                    int ID = Common.Misc.getRootWorkingUnit(wu.WorkingUnitID, wuDict);
                                                    if (companyWU.WorkingUnitID == ID)
                                                        workShopWU = wu;
                                                }
                                            }

                                            // find cost center by name from unit stringone
                                            workingUnit.WUTO = new WorkingUnitTO();
                                            workingUnit.WUTO.Name = plantCode + "." + costCenterCode;
                                            List<WorkingUnitTO> CostCenterList = workingUnit.SearchExact();
                                            if (CostCenterList.Count > 0)
                                            {
                                                foreach (WorkingUnitTO wu in CostCenterList)
                                                {
                                                    int ID = Common.Misc.getRootWorkingUnit(wu.WorkingUnitID, wuDict);
                                                    if (companyWU.WorkingUnitID == ID)
                                                        costCenterWU = wu;
                                                }
                                            }

                                            // find plant by name from unit stringone
                                            workingUnit.WUTO = new WorkingUnitTO();
                                            workingUnit.WUTO.Name = plantCode;
                                            List<WorkingUnitTO> plantList = workingUnit.SearchExact();
                                            if (plantList.Count > 0)
                                            {
                                                foreach (WorkingUnitTO wu in plantList)
                                                {
                                                    int ID = Common.Misc.getRootWorkingUnit(wu.WorkingUnitID, wuDict);
                                                    if (companyWU.WorkingUnitID == ID)
                                                        plantWU = wu;
                                                }
                                            }

                                            allWU.Add(wuTO);
                                        }
                                    }
                                }
                            }

                            workingUnit.WUTO = new WorkingUnitTO();
                            int nextID = workingUnit.FindMINWUID() - 1;

                            // get users that should be given visibility under new made units
                            List<ApplUsersXWUTO> wuVisibility = new List<ApplUsersXWUTO>();                            
                            if (plantWU.WorkingUnitID == -1 || costCenterWU.WorkingUnitID == -1)
                            {
                                // if new cost center and/or plant are inserted, get all users that has company visibility and give them visibility to new structure
                                userXWU.AuXWUnitTO = new ApplUsersXWUTO();
                                userXWU.AuXWUnitTO.WorkingUnitID = companyWU.WorkingUnitID;
                                wuVisibility = userXWU.Search();
                            }
                            else if (costCenterWU.WorkingUnitID != -1)
                            {
                                //if cost center exist for each employee responsible for wu under cost center add all new wu's under cost center
                                userXWU.AuXWUnitTO = new ApplUsersXWUTO();
                                userXWU.AuXWUnitTO.WorkingUnitID = costCenterWU.WorkingUnitID;
                                wuVisibility = userXWU.Search();
                            }

                            //begin ACTA transaction
                            bool trans = workingUnit.BeginTransaction();
                            if (trans)
                            {
                                try
                                {
                                    bool succ = true;

                                    if (error.Trim().Equals(""))
                                    {
                                        //if all wu in hierarchy exist just update wu values
                                        if (workShopWU.WorkingUnitID != -1)
                                        {
                                            wuTO.ParentWorkingUID = workShopWU.WorkingUnitID;
                                        }
                                        //enter missing levels of working units
                                        else
                                        {
                                            if (plantWU.WorkingUnitID == -1)
                                            {
                                                plantWU.WorkingUnitID = nextID;
                                                nextID--;
                                                plantWU.Status = Constants.statusActive;
                                                plantWU.ParentWorkingUID = companyWU.WorkingUnitID;
                                                plantWU.Name = plantCode;
                                                plantWU.Description = plantWU.Name;
                                                plantWU.Code = plantCode;
                                                workingUnit.WUTO = plantWU;
                                                allWU.Add(plantWU);
                                                succ = succ && (workingUnit.Save(false) > 0);
                                            }
                                            if (costCenterWU.WorkingUnitID == -1)
                                            {
                                                costCenterWU.WorkingUnitID = nextID;
                                                nextID--;
                                                costCenterWU.Status = Constants.statusActive;
                                                costCenterWU.ParentWorkingUID = plantWU.WorkingUnitID;
                                                costCenterWU.Name = plantCode + "." + costCenterCode;
                                                costCenterWU.Description = costCenterWU.Name;
                                                costCenterWU.Code = costCenterCode;
                                                workingUnit.WUTO = costCenterWU;
                                                allWU.Add(costCenterWU);
                                                newCCUnits.Add(costCenterWU);
                                                succ = succ && workingUnit.Save(false) > 0;
                                            }
                                            if (workShopWU.WorkingUnitID == -1)
                                            {
                                                workShopWU.WorkingUnitID = nextID;
                                                nextID--;
                                                workShopWU.Status = Constants.statusActive;
                                                workShopWU.ParentWorkingUID = costCenterWU.WorkingUnitID;
                                                workShopWU.Name = plantCode + "." + costCenterCode + "." + workShopCode;
                                                workShopWU.Description = workShopWU.Name;
                                                workShopWU.Code = workShopCode;
                                                workingUnit.WUTO = workShopWU;
                                                allWU.Add(workShopWU);                                                
                                                newCCUnits.Add(workShopWU);
                                                succ = succ && workingUnit.Save(false) > 0;
                                            }

                                            wuTO.ParentWorkingUID = workShopWU.WorkingUnitID;
                                        }
                                                                                
                                        newCCUnits.Add(wuTO);

                                        // save/update working unit
                                        workingUnit.WUTO = wuTO;
                                        if (!unitExist)
                                            succ = succ && workingUnit.Save(false) > 0;
                                        else
                                            succ = succ && workingUnit.Update(false);

                                        // give visibility to all new made working units to all users that should see them                                        
                                        foreach (ApplUsersXWUTO wuTemp in wuVisibility)
                                        {
                                            foreach (WorkingUnitTO wu in allWU)
                                            {
                                                ApplUsersXWUTO userXWUTO = new ApplUsersXWUTO();
                                                userXWUTO.UserID = wuTemp.UserID;
                                                userXWUTO.WorkingUnitID = wu.WorkingUnitID;
                                                userXWUTO.Purpose = wuTemp.Purpose;
                                                usersForWU.Add(userXWUTO);
                                            }
                                        }
                                        
                                        userXWU.SetTransaction(workingUnit.GetTransaction());

                                        //delete appl_user_x_working_units belong to cost center
                                        foreach (WorkingUnitTO unit in allWU)
                                        {
                                            succ = succ && userXWU.Delete(unit.WorkingUnitID, false);
                                        }

                                        // delete users visibility if exist users for deletion (employee that lost old cost center visibility)
                                        foreach (string userID in oldCCDeleteUsers)
                                        {
                                            foreach (WorkingUnitTO wu in oldCCUnits)
                                            {
                                                succ = succ && userXWU.Delete(userID, wu.WorkingUnitID, "", false);
                                            }
                                        }
                                                                                
                                        //insert new records
                                        foreach (ApplUsersXWUTO applWU in usersForWU)
                                        {
                                            userXWU.AuXWUnitTO = applWU;
                                            succ = succ && (userXWU.Save(applWU.UserID, applWU.WorkingUnitID, applWU.Purpose, false) > 0);                                            
                                        }

                                        // after changes done, get all units under which responsible users have visibility
                                        responsibleUsersUnits = new Dictionary<string, List<WorkingUnitTO>>();
                                        foreach (string userID in responsibleUsers)
                                        {
                                            if (!responsibleUsersUnits.ContainsKey(userID))
                                            {
                                                userXWU.AuXWUnitTO = new ApplUsersXWUTO();
                                                responsibleUsersUnits.Add(userID, userXWU.FindWUForUser(userID, Constants.EmployeesPurpose, workingUnit.GetTransaction()));
                                            }
                                        }

                                        // add responsible users visibility to new cost center if it does not exist
                                        foreach (string userID in responsibleUsers)
                                        {
                                            List<WorkingUnitTO> visibileUnits = new List<WorkingUnitTO>();
                                            if (responsibleUsersUnits.ContainsKey(userID))
                                                visibileUnits = responsibleUsersUnits[userID];

                                            foreach (WorkingUnitTO newCCwu in newCCUnits)
                                            {
                                                bool visibilityExists = false;
                                                foreach (WorkingUnitTO visWU in visibileUnits)
                                                {                                                    
                                                    if (newCCwu.WorkingUnitID == visWU.WorkingUnitID)
                                                    {
                                                        visibilityExists = true;
                                                        break;
                                                    }
                                                }

                                                if (!visibilityExists)
                                                {
                                                    succ = succ && userXWU.Save(userID, newCCwu.WorkingUnitID, Constants.EmployeesPurpose, false) > 0;
                                                }
                                            }
                                        }

                                        asco.SetTransaction(workingUnit.GetTransaction());
                                        foreach (EmployeeAsco4TO ascoTO in ascoToUpdate)
                                        {
                                            asco.EmplAsco4TO = ascoTO;
                                            succ = succ && asco.update(false);
                                        }
                                    }

                                    if (succ)
                                    {
                                        syncFS.SetTransaction(workingUnit.GetTransaction());
                                        syncFS.FSTO = fsTO;
                                        if (error.Trim().Equals(""))
                                            syncFS.FSTO.Result = Constants.resultSucc;
                                        else
                                        {
                                            syncFS.FSTO.Result = Constants.resultFaild;
                                            syncFS.FSTO.Remark = error;
                                        }

                                        succ = succ && syncFS.insert(false);
                                        if (succ)
                                        {
                                            //before commit ACTA transaction try to delete record from sync DB
                                            succ = succ && syncDAO.delSyncFinancialStructure(fsTO);
                                            if (succ)
                                            {
                                                workingUnit.CommitTransaction();
                                            }
                                            else
                                            {
                                                workingUnit.RollbackTransaction();
                                                WriteLog(" SyncFinancialStructure() Delete record from sync_financial_structure fail for rec_id = " + fsTO.RecID.ToString());
                                            }
                                        }
                                        else
                                        {
                                            workingUnit.RollbackTransaction();
                                        }
                                    }
                                    else
                                        workingUnit.RollbackTransaction();
                                }
                                catch (Exception ex)
                                {
                                    if (workingUnit.GetTransaction() != null)
                                        workingUnit.RollbackTransaction();
                                    WriteLog(" SyncFinancialStructure() exception: " + ex.Message);
                                }
                            }
                            else
                            {
                                WriteLog(" SyncFinancialStructure() ACTA DB begin transaction faild! ");
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog(" SyncFinancialStructure() exception: " + ex.Message + "; for rec_id = " + fsTO.RecID.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(" SyncFinancialStructure() exception: " + ex.Message);
            }
        }

        private void SyncCostCenters()
        {
            try
            {
                //get all records from sync_cost_centers
                List<SyncCostCenterTO> ccRecords = syncDAO.getDifCostCenters();
                
                //if there is no records continue
                if (ccRecords.Count <= 0)
                {
                    WriteLog(" SyncCostCenters - Records not found. ");
                }
                //if records found begin processing
                else
                {
                    WriteLog(" SyncCostCenters - Num of records found: " + ccRecords.Count.ToString());

                    SyncCostCenterHist syncCCHist = new SyncCostCenterHist(ACTAConnection);
                    WorkingUnit workingUnit = new WorkingUnit(ACTAConnection);
                    
                    Dictionary<int, WorkingUnitTO> wuDIct = workingUnit.getWUDictionary();

                    int count = 0;
                    //process records one by one
                    foreach (SyncCostCenterTO ccTO in ccRecords)
                    {
                        count++;
                        SetManagerThreadStatus("Synchronization Cost Centers: " + count.ToString() + "/" + ccRecords.Count.ToString());

                        if (ccTO.ValidFrom.Date > DateTime.Now.Date || ccTO.ValidFrom.Equals(new DateTime()))
                            continue;

                        string error = "";
                        try
                        {
                            bool dataChange = false;

                            WorkingUnitTO ccWUTO = new WorkingUnitTO();

                            // check if there is cc code and company code - invalid record if any of this data missing
                            if (ccTO.Code.Trim().Equals(""))
                            {
                                error = "Cost center code missing";
                                WriteLog(" SyncCostCenters() Cost center code missing for rec id = " + ccTO.RecID);
                            }

                            if (ccTO.CompanyCode.Trim().Equals(""))
                            {
                                error = "Cost center company code missing";
                                WriteLog(" SyncCostCenters() Cost center company code missing for rec id = " + ccTO.RecID);
                            }

                            if (ccTO.Code.Trim().Length < 3)
                            {
                                error = "Cost center code invalid lenght";
                                WriteLog(" SyncCostCenters() Cost center code invalid lenght for rec id = " + ccTO.RecID);
                            }
                            
                            if (error.Trim().Equals(""))
                            {
                                if (ccTO.CompanyCode == "1367")
                                    ccTO.Code = ccTO.Code.Substring(3);

                                string stringone = ccTO.Code.Substring(0, 3) + "." + ccTO.Code.Substring(3);
                                workingUnit.WUTO = new WorkingUnitTO();
                                workingUnit.WUTO.Name = stringone;
                                List<WorkingUnitTO> workingUnits = workingUnit.SearchExact();
                                
                                if (workingUnits.Count <= 0)
                                {
                                    error = "CostCenter for stringone: " + stringone + " not found";
                                }                                    

                                if (error.Trim().Equals(""))
                                {
                                    WorkingUnitTO companyWU = new WorkingUnitTO();
                                    workingUnit.WUTO = new WorkingUnitTO();
                                    workingUnit.WUTO.Name = ccTO.CompanyCode;
                                    List<WorkingUnitTO> workingUnitsCompany = workingUnit.SearchExact();
                                    if (workingUnitsCompany.Count <= 0)
                                        error = "Company for company_code: " + ccTO.CompanyCode + " not found";
                                    else
                                        companyWU = workingUnitsCompany[0];                                    
                                
                                    if (error.Trim().Equals(""))
                                    {                                        
                                        foreach (WorkingUnitTO wuTO in workingUnits)
                                        {
                                            int compID = Common.Misc.getRootWorkingUnit(wuTO.WorkingUnitID, wuDIct);
                                            if (compID == companyWU.WorkingUnitID)
                                            {
                                                ccWUTO = wuTO;
                                            }
                                        }
                                        if (ccWUTO.WorkingUnitID == -1)                                        
                                            error = "Working unit not found for stringone: " + stringone + "; and company_code: " + ccTO.CompanyCode;                                        
                                        else
                                        {
                                            dataChange = true;
                                            ccWUTO.Description = ccTO.Desc;
                                        }
                                    }
                                }
                            }

                            // begin ACTA transaction
                            bool trans = workingUnit.BeginTransaction();
                            if (trans)
                            {
                                try
                                {
                                    bool succ = true;

                                    if (error.Trim().Equals("") && dataChange)
                                    {
                                        // save/update working unit
                                        workingUnit.WUTO = ccWUTO;                                        
                                        succ = succ && workingUnit.Update(false);
                                    }

                                    if (succ)
                                    {
                                        syncCCHist.SetTransaction(workingUnit.GetTransaction());
                                        syncCCHist.CCTO = ccTO;
                                        if (error.Trim().Equals(""))
                                            syncCCHist.CCTO.Result = Constants.resultSucc;
                                        else
                                        {
                                            syncCCHist.CCTO.Result = Constants.resultFaild;
                                            syncCCHist.CCTO.Remark = error;
                                        }

                                        succ = succ && syncCCHist.insert(false);

                                        if (succ)
                                        {
                                            //before commit ACTA transaction try to delete record from sync DB
                                            succ = succ && syncDAO.delSyncCostCenters(ccTO);
                                            if (succ)
                                            {
                                                workingUnit.CommitTransaction();
                                            }
                                            else
                                            {
                                                workingUnit.RollbackTransaction();
                                                WriteLog(" SyncCostCenters() Delete record from sync_cost_centers fail for rec_id = " + ccTO.RecID.ToString());
                                            }
                                        }
                                        else
                                        {
                                            workingUnit.RollbackTransaction();
                                        }
                                    }
                                    else
                                        workingUnit.RollbackTransaction();
                                }
                                catch (Exception ex)
                                {
                                    if (workingUnit.GetTransaction() != null)
                                        workingUnit.RollbackTransaction();
                                    WriteLog(" SyncCostCenters() exception: " + ex.Message);
                                }
                            }
                            else
                            {
                                WriteLog(" SyncCostCenters() ACTA DB begin transaction faild! ");
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog(" SyncCostCenters() exception: " + ex.Message + "; for rec_id = " + ccTO.RecID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(" SyncCostCenters() exception: " + ex.Message);
            }
        }

        private void SyncEmployeePositions()
        {
            try
            {
                //get all records from sync_employee_positions
                List<SyncEmployeePositionTO> posRecords = syncDAO.getDifEmployeePositions();

                //if there is no records continue
                if (posRecords.Count <= 0)
                {
                    WriteLog(" SyncEmployeePositions - Records not found. ");
                }
                //if records found begin processing
                else
                {
                    WriteLog(" SyncEmployeePositions - Num of records found: " + posRecords.Count.ToString());

                    EmployeePosition position = new EmployeePosition(ACTAConnection);
                    SyncEmployeePositionHist syncPos = new SyncEmployeePositionHist(ACTAConnection);
                    WorkingUnit workingUnit = new WorkingUnit(ACTAConnection);                    

                    int count = 0;
                    //process records one by one
                    foreach (SyncEmployeePositionTO syncPosTO in posRecords)
                    {
                        count++;
                        SetManagerThreadStatus("Synchronization Employee Positions: " + count.ToString() + "/" + posRecords.Count.ToString());

                        if (syncPosTO.ValidFrom.Date > DateTime.Now.Date || syncPosTO.ValidFrom.Equals(new DateTime()))
                            continue;

                        string error = "";
                        bool dataChange = false;

                        try
                        {
                            position.EmplPositionTO = new EmployeePositionTO();
                            position.EmplPositionTO.PositionID = syncPosTO.PositionID;
                            List<EmployeePositionTO> posList = position.SearchEmployeePositions();
                            EmployeePositionTO posTO = new EmployeePositionTO();
                            bool posExist = false;
                            if (posList.Count > 0)
                            {
                                posExist = true;
                                posTO = posList[0];
                            }

                            //if position already exists
                            if (posExist)
                            {                                
                                //if description, title or code changed and date time is now or passed update position description, title or code
                                if (syncPosTO.PositionCode.Trim() != "")
                                {
                                    dataChange = true;
                                    posTO.PositionCode = syncPosTO.PositionCode;                                    
                                }

                                if (syncPosTO.PositionTitleSR.Trim() != "")
                                {
                                    dataChange = true;
                                    posTO.PositionTitleSR = syncPosTO.PositionTitleSR;
                                }

                                if (syncPosTO.PositionTitleEN.Trim() != "")
                                {
                                    dataChange = true;
                                    posTO.PositionTitleEN = syncPosTO.PositionTitleEN;
                                }

                                if (syncPosTO.DescSR.Trim() != "")
                                {
                                    dataChange = true;
                                    posTO.DescSR = syncPosTO.DescSR;
                                }

                                if (syncPosTO.DescEN.Trim() != "")
                                {
                                    dataChange = true;
                                    posTO.DescEN = syncPosTO.DescEN;
                                }

                                if (syncPosTO.Status.Trim() != "")
                                {                                    
                                    dataChange = true;
                                    posTO.Status = syncPosTO.Status;
                                }
                                                                
                                if (syncPosTO.CompanyCode != "")
                                {
                                    if (syncPosTO.CompanyCode.Trim().Length < 3)
                                    {
                                        error = "Company code invalid lenght";
                                        WriteLog(" SyncEmployeePositions(): Company code invalid lenght for rec id = " + syncPosTO.RecID);
                                    }

                                    if (error.Trim().Equals(""))
                                    {
                                        WorkingUnitTO companyWU = new WorkingUnitTO();
                                        if (syncPosTO.CompanyCode != "")
                                        {
                                            workingUnit.WUTO.Name = syncPosTO.CompanyCode;
                                            List<WorkingUnitTO> workingUnitsCompany = workingUnit.SearchExact();
                                            if (workingUnitsCompany.Count <= 0)
                                                error = "Company for company_code: " + syncPosTO.CompanyCode + " not found";
                                            else
                                                companyWU = workingUnitsCompany[0];
                                        }

                                        if (error.Trim().Equals(""))
                                        {
                                            dataChange = true;
                                            posTO.WorkingUnitID = companyWU.WorkingUnitID;
                                        }
                                    }
                                }                               
                            }
                            else //NEW EMPLOYEE POSITION
                            {
                                dataChange = true;

                                posTO.PositionID = syncPosTO.PositionID;

                                if (syncPosTO.CompanyCode.Trim().Equals(""))
                                {
                                    error = "Can not insert new position without company code. PositionID = " + syncPosTO.PositionID.ToString().Trim();
                                }

                                if (syncPosTO.PositionCode.Trim().Equals(""))
                                {
                                    error = "Can not insert new position without position code. PositionID = " + syncPosTO.PositionID.ToString().Trim();
                                }

                                if (syncPosTO.PositionTitleSR.Trim().Equals(""))
                                {
                                    error = "Can not insert new position without position title SR. PositionID = " + syncPosTO.PositionID.ToString().Trim();
                                }

                                if (syncPosTO.PositionTitleEN.Trim().Equals(""))
                                {
                                    error = "Can not insert new position without position title EN. PositionID = " + syncPosTO.PositionID.ToString().Trim();
                                }

                                if (error.Trim().Equals(""))
                                {
                                    if (syncPosTO.CompanyCode.Trim().Length < 3)
                                    {
                                        error = "Cost center code invalid lenght";
                                        WriteLog(" SyncEmployeePositions(): Company code invalid lenght for rec id = " + syncPosTO.RecID);
                                    }

                                    if (error.Trim().Equals(""))
                                    {
                                        workingUnit.WUTO.Name = syncPosTO.CompanyCode;
                                        List<WorkingUnitTO> workingUnitsCompany = workingUnit.SearchExact();
                                        if (workingUnitsCompany.Count <= 0)
                                            error = "Company for company_code: " + syncPosTO.CompanyCode + " not found";
                                        else
                                        {
                                            posTO.WorkingUnitID = workingUnitsCompany[0].WorkingUnitID;
                                            posTO.PositionCode = syncPosTO.PositionCode.Trim();
                                            posTO.PositionTitleSR = syncPosTO.PositionTitleSR.Trim();
                                            posTO.PositionTitleEN = syncPosTO.PositionTitleEN.Trim();

                                            if (syncPosTO.DescSR.Trim() != "")
                                                posTO.DescSR = syncPosTO.DescSR.Trim();

                                            if (syncPosTO.DescEN.Trim() != "")
                                                posTO.DescEN = syncPosTO.DescEN.Trim();
                                        }
                                    }
                                }
                            }

                            #region DB update
                            //begin ACTA transaction
                            bool trans = position.BeginTransaction();
                            if (trans)
                            {
                                try
                                {
                                    position.EmplPositionTO = posTO;
                                    bool succ = true;
                                    if (error == "" && dataChange)
                                    {
                                        if (posExist)
                                            succ = position.Update(false);
                                        else
                                        {                                            
                                            position.EmplPositionTO.Status = Constants.statusActive;
                                            succ = position.Save(false) > 0;
                                        }
                                    }
                                    if (succ)
                                    {
                                        syncPos.SetTransaction(position.GetTransaction());
                                        syncPos.PosTO = syncPosTO;
                                        if (error == "")
                                            syncPos.PosTO.Result = Constants.resultSucc;
                                        else
                                        {
                                            WriteLog(" SyncEmployeePositions - " + error);
                                            syncPos.PosTO.Result = Constants.resultFaild;
                                            syncPos.PosTO.Remark = error;
                                        }
                                        succ = syncPos.insert(false);
                                        if (succ)
                                        {
                                            //before commit ACTA transaction try to delete record from sync DB
                                            succ = syncDAO.delSyncEmployeePosition(syncPosTO);
                                            if (succ)
                                            {
                                                position.CommitTransaction();
                                            }
                                            else
                                            {
                                                position.RollbackTransaction();
                                                WriteLog(" SyncEmployeePositions() Delete record from sync_employee_positions fail for rec_id = " + syncPosTO.RecID.ToString());
                                            }
                                        }
                                        else
                                        {
                                            position.RollbackTransaction();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (position.GetTransaction() != null)
                                        position.RollbackTransaction();
                                    WriteLog(" SyncEmployeePositions() exception: " + ex.Message);
                                }
                            }
                            else
                            {
                                WriteLog(" SyncEmployeePositions() ACTA DB begin transaction faild! ");
                            }
                            #endregion DB
                        }
                        catch (Exception ex)
                        {
                            WriteLog(" SyncEmployeePositions() exception: " + ex.Message + "; for rec_id = " + syncPosTO.RecID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(" SyncEmployeePositions() exception: " + ex.Message);
            }
        }

        private void SyncAnnualLeaveRecalculation()
        {
            try
            {
                // check if recalculation should be synchronized
                if (new SyncAnnualLeaveRecalcFlag(ACTAConnection).GetFlag() == 0)
                    return;

                //get all records from sync_annual_leave_recalc
                List<SyncAnnualLeaveRecalcTO> recalcRecords = syncDAO.getDifAnnualLeaveRecalc(-1);

                //if there is no records continue
                if (recalcRecords.Count <= 0)
                {
                    WriteLog(" SyncAnnualLeaveRecalculation - Records not found. ");
                }
                //if records found begin processing
                else
                {
                    WriteLog(" SyncAnnualLeaveRecalculation - Num of records found: " + recalcRecords.Count.ToString());

                    SyncAnnualLeaveRecalcHist syncRecalcHist = new SyncAnnualLeaveRecalcHist(ACTAConnection);
                    EmployeeCounterValue counter = new EmployeeCounterValue(ACTAConnection);
                    EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist(ACTAConnection);
                    
                    // do not allow to recalculate twice for same employee
                    List<int> recalculatedEmployees = new List<int>();

                    int current = (int)Constants.EmplCounterTypes.AnnualLeaveCounter;
                    int previous = (int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter;
                    int used = (int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter;

                    int count = 0;
                    //process records one by one
                    foreach (SyncAnnualLeaveRecalcTO recalcTO in recalcRecords)
                    {
                        count++;
                        SetManagerThreadStatus("Synchronization Recalculation Annual Leaves: " + count.ToString() + "/" + recalcRecords.Count.ToString());

                        string error = "";
                        try
                        {
                            // check if employee is already recalculated
                            if (recalculatedEmployees.Contains(recalcTO.EmployeeID))
                            {
                                error = "Employee is already recalculated";
                                WriteLog(" SyncAnnualLeaveRecalculation() Employee is already recalculated for employee id = " + recalcTO.EmployeeID);
                            }

                            // check if some data is missing
                            if (recalcTO.EmployeeID == -1)
                            {
                                error = "Employee missing";
                                WriteLog(" SyncAnnualLeaveRecalculation() Employee missing for rec id = " + recalcTO.RecID);
                            }

                            if (recalcTO.Year.Year != DateTime.Now.Year)
                            {
                                error = "Invalid year";
                                WriteLog(" SyncAnnualLeaveRecalculation() Invalid year for rec id = " + recalcTO.RecID);
                            }

                            if (recalcTO.NumOfDays < 0)
                            {
                                error = "Invalid number of days";
                                WriteLog(" SyncAnnualLeaveRecalculation() Invalid number of days for rec id = " + recalcTO.RecID);
                            }

                            Dictionary<int, EmployeeCounterValueTO> oldCounters = new Dictionary<int, EmployeeCounterValueTO>();
                            Dictionary<int, EmployeeCounterValueTO> newCounters = new Dictionary<int, EmployeeCounterValueTO>();

                            if (error.Trim().Equals(""))
                            {
                                // get employee counters
                                Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounters = counter.SearchValues(recalcTO.EmployeeID.ToString().Trim());

                                if (!emplCounters.ContainsKey(recalcTO.EmployeeID) || !emplCounters[recalcTO.EmployeeID].ContainsKey(current)
                                    || !emplCounters[recalcTO.EmployeeID].ContainsKey(previous) || !emplCounters[recalcTO.EmployeeID].ContainsKey(used))
                                {
                                    error = "Employee has no counters";
                                    WriteLog(" SyncAnnualLeaveRecalculation() Employee has no counters for employee_id = " + recalcTO.EmployeeID.ToString().Trim());
                                }

                                if (error.Trim().Equals(""))
                                {
                                    // add annual leave counters to list
                                    foreach (int type in emplCounters[recalcTO.EmployeeID].Keys)
                                    {
                                        if (type == current || type == previous || type == used)
                                        {
                                            if (!oldCounters.ContainsKey(type))
                                                oldCounters.Add(type, emplCounters[recalcTO.EmployeeID][type]);
                                            if (!newCounters.ContainsKey(type))
                                                newCounters.Add(type, new EmployeeCounterValueTO(emplCounters[recalcTO.EmployeeID][type]));
                                        }
                                    }
                                    
                                    if (newCounters.ContainsKey(current) && newCounters.ContainsKey(previous) && newCounters.ContainsKey(used))
                                    {
                                        // recalculate annual leaves
                                        // used = used - previous
                                        // previous = current
                                        // current = sync num of days
                                        newCounters[used].Value -= newCounters[previous].Value;
                                        newCounters[previous].Value = newCounters[current].Value;
                                        newCounters[current].Value = recalcTO.NumOfDays;
                                    }
                                    else
                                    {
                                        error = "Employee has no all annual leave counters";
                                        WriteLog(" SyncAnnualLeaveRecalculation() Employee has no all annual leave counters for employee_id = " + recalcTO.EmployeeID.ToString().Trim());
                                    }
                                }
                            }

                            // begin ACTA transaction
                            bool trans = counter.BeginTransaction();
                            if (trans)
                            {
                                try
                                {
                                    bool succ = true;

                                    if (error.Trim().Equals(""))
                                    {
                                        // update counters and move old counter values to hist table if updated
                                        counterHist.SetTransaction(counter.GetTransaction());
                                        foreach (int type in oldCounters.Keys)
                                        {
                                            // move to hist table
                                            counterHist.ValueTO = new EmployeeCounterValueHistTO(oldCounters[type]);
                                            counterHist.ValueTO.ModifiedBy = Constants.syncUser;
                                            succ = succ && (counterHist.Save(false) >= 0);

                                            if (!succ)
                                                break;
                                        }

                                        if (succ)
                                        {
                                            foreach (int type in newCounters.Keys)
                                            {
                                                // update counter
                                                counter.ValueTO = new EmployeeCounterValueTO(newCounters[type]);
                                                counter.ValueTO.ModifiedBy = Constants.syncUser;

                                                succ = succ && counter.Update(false);

                                                if (!succ)
                                                    break;
                                            }
                                        }
                                    }

                                    if (succ)
                                    {
                                        syncRecalcHist.SetTransaction(counter.GetTransaction());
                                        syncRecalcHist.RecalcTO = recalcTO;
                                        if (error.Trim().Equals(""))
                                        {
                                            recalculatedEmployees.Add(recalcTO.EmployeeID);
                                            syncRecalcHist.RecalcTO.Result = Constants.resultSucc;
                                        }
                                        else
                                        {
                                            syncRecalcHist.RecalcTO.Result = Constants.resultFaild;
                                            syncRecalcHist.RecalcTO.Remark = error;
                                        }

                                        succ = succ && syncRecalcHist.insert(false);

                                        if (succ)
                                        {
                                            //before commit ACTA transaction try to delete record from sync DB
                                            succ = succ && syncDAO.delSyncAnnualLeaveRecalc(recalcTO);
                                            if (succ)
                                            {
                                                counter.CommitTransaction();
                                            }
                                            else
                                            {
                                                counter.RollbackTransaction();
                                                WriteLog(" SyncAnnualLeaveRecalculation() Delete record from sync_cost_centers fail for rec_id = " + recalcTO.RecID.ToString());
                                            }
                                        }
                                        else
                                        {
                                            counter.RollbackTransaction();
                                        }
                                    }
                                    else
                                        counter.RollbackTransaction();
                                }
                                catch (Exception ex)
                                {
                                    if (counter.GetTransaction() != null)
                                        counter.RollbackTransaction();
                                    WriteLog(" SyncAnnualLeaveRecalculation() exception: " + ex.Message);
                                }
                            }
                            else
                            {
                                WriteLog(" SyncAnnualLeaveRecalculation() ACTA DB begin transaction faild! ");
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog(" SyncAnnualLeaveRecalculation() exception: " + ex.Message + "; for rec_id = " + recalcTO.RecID);
                        }
                    }
                }

                if (!new SyncAnnualLeaveRecalcFlag(ACTAConnection).Update(Constants.noInt, true, true))
                    WriteLog(" SyncAnnualLeaveRecalculation(): Setting recalculation flag to 0 faild! ");

            }
            catch (Exception ex)
            {
                WriteLog(" SyncAnnualLeaveRecalculation() exception: " + ex.Message);
            }
        }

        private void closeConnections()
        {
            try
            {
                DBConnectionManager.Instance.CloseDBConnection(ACTAConnection);
                syncDAO.CloseConnection(SyncConnection);
                WriteLog(" SyncManager.closeConnections() Connection closed successfully!");
               
            }
            catch (Exception ex)
            {
                WriteLog(" SyncManager.closeConnections() exception: " + ex.Message);
            }
        }

        private bool connectToDataBases()
        {
            bool succ = false;
            try
            {
                ACTAConnection = DBConnectionManager.Instance.MakeNewDBConnection();
                if (ACTAConnection == null)
                {
                    WriteLog(" SyncManager.connectToDataBases() Make new ACTA DB connection faild!");
                    return false;
                }
                if (syncDAO == null)
                    syncDAO = SynchronizationDAO.getDAO();
                SyncConnection = syncDAO.MakeNewDBConnection();
                if (SyncConnection == null)
                {
                    WriteLog(" SyncManager.connectToDataBases() Make new Sync DB connection faild!");
                    return false; 
                }
                syncDAO.setDBConnection(SyncConnection);

                succ = true;
            }
            catch (Exception ex)
            {
                WriteLog(" SyncManager.connectToDataBases() exception: " + ex.Message);
            }
            return succ;
        }

        private bool isSyncTime()
        {
            if (DateTime.Now > nextSyncTime)
                return true;
            else
                return false;
        }

        public void WriteLog(string message)
        {
            log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") +  message);
        }

        private void SetManagerThreadStatus(string text)
        {
            lock (locker)
            {
                managerThreadStatus = text;
            }
        }

        public string getManagerThreadStatus()
        {
            lock (locker)
            {
               return managerThreadStatus;
            }
        }
    }
}
