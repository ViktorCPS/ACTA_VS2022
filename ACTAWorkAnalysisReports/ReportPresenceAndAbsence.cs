using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using System.IO;
using System.Data;
using TransferObjects;
using Common;

namespace ACTAWorkAnalysisReports
{
    public class ReportPresenceAndAbsence
    {
        DebugLog debug;
        public ReportPresenceAndAbsence()
        {

            debug = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");

        }
        public string GenerateReport(Object dbConnection, string filePath, List<WorkingUnitTO> listPlants, DateTime day, int company, bool isAltLang)
        {
            debug.writeLog("+ Started presence and absence report! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            try
            {

                DataSet ds = new DataSet(Path.GetFileNameWithoutExtension(filePath));
                PassType passType;
                WorkingUnit workingUnit;
                IOPairProcessed ioPairProc;
                Employee Employee;
                if (dbConnection == null)
                {
                    passType = new PassType();
                    workingUnit = new WorkingUnit();
                    ioPairProc = new IOPairProcessed();
                    Employee = new Employee();
                }
                else
                {
                    passType = new PassType(dbConnection);
                    workingUnit = new WorkingUnit(dbConnection);
                    ioPairProc = new IOPairProcessed(dbConnection);
                    Employee = new Employee(dbConnection);
                }

                List<PassTypeTO> listPassTypes = new List<PassTypeTO>();
                listPassTypes = passType.SearchForCompany(company, false);

                int page = 0;
                int rowStart = 1;
                List<DateTime> datesList = new List<DateTime>();
                datesList.Add(day);

                string mypath = filePath;


                int numChart = 0;
                int numOfEmployees = 0;
                foreach (WorkingUnitTO plant in listPlants)
                {
                    if (dbConnection == null)
                        workingUnit = new WorkingUnit();
                    else
                        workingUnit = new WorkingUnit(dbConnection);
                    List<WorkingUnitTO> listCostCenter = workingUnit.SearchChildWU(plant.WorkingUnitID.ToString());

                    foreach (WorkingUnitTO costCenter in listCostCenter)
                    {
                        List<WorkingUnitTO> listWorkshop = new List<WorkingUnitTO>();
                        listWorkshop.Add(costCenter);
                        if (dbConnection == null)
                        {
                            Employee = new Employee();
                            workingUnit = new WorkingUnit();
                        }
                        else
                        {
                            Employee = new Employee(dbConnection);
                            workingUnit = new WorkingUnit(dbConnection);
                        }

                        listWorkshop = workingUnit.FindAllChildren(listWorkshop);

                        string wuIDs = "";
                        foreach (WorkingUnitTO wu in listWorkshop)
                        {
                            if (costCenter.WorkingUnitID != wu.WorkingUnitID)
                                wuIDs += wu.WorkingUnitID + ",";
                        }
                        if (wuIDs.Length > 0)
                            wuIDs = wuIDs.Remove(wuIDs.LastIndexOf(','));

                        List<EmployeeTO> listEmpl = new List<EmployeeTO>();

                        listEmpl = Employee.SearchByWULoans(wuIDs, -1, null, datesList[0], datesList[datesList.Count - 1]);

                        if (listEmpl.Count > 0)
                        {
                            Dictionary<string, string> dictEmplTypes = Misc.emplTypes(dbConnection, listEmpl, company);

                            string emplIdsBC = "";
                            string emplIds = "";
                            foreach (KeyValuePair<string, string> pair in dictEmplTypes)
                            {
                                if (pair.Value.Equals("BC"))
                                    emplIdsBC += pair.Key + ",";
                                else
                                    emplIds += pair.Key + ",";
                            }

                            if (emplIdsBC.Length > 0)
                                emplIdsBC = emplIdsBC.Remove(emplIdsBC.LastIndexOf(','));

                            if (emplIds.Length > 0)
                                emplIds = emplIds.Remove(emplIds.LastIndexOf(','));

                            if (emplIdsBC.Length > 0)
                            {

                                //dtable.Clear();
                                dictEmplTypes = Misc.emplBranch(dbConnection, emplIdsBC, company);
                                string emplIdsBCDirect = "";
                                string emplIdsBCIndirect = "";
                                foreach (KeyValuePair<string, string> pair in dictEmplTypes)
                                {
                                    if (pair.Value.Equals("A"))
                                        emplIdsBCDirect += pair.Key + ",";
                                    else
                                        emplIdsBCIndirect += pair.Key + ",";
                                }

                                if (emplIdsBCDirect.Length > 0)
                                    emplIdsBCDirect = emplIdsBCDirect.Remove(emplIdsBCDirect.LastIndexOf(','));

                                if (emplIdsBCIndirect.Length > 0)
                                    emplIdsBCIndirect = emplIdsBCIndirect.Remove(emplIdsBCIndirect.LastIndexOf(','));
                                List<IOPairProcessedTO> IOPairListDirectBC = new List<IOPairProcessedTO>();
                                List<IOPairProcessedTO> IOPairListIndirectBC = new List<IOPairProcessedTO>();
                                if (dbConnection == null)
                                    ioPairProc = new IOPairProcessed();
                                else
                                    ioPairProc = new IOPairProcessed(dbConnection);

                                if (emplIdsBCDirect.Length > 0)
                                    IOPairListDirectBC = ioPairProc.SearchAllPairsForEmpl(emplIdsBCDirect, datesList, "");
                                else
                                    IOPairListDirectBC = new List<IOPairProcessedTO>();

                                if (dbConnection == null)
                                    ioPairProc = new IOPairProcessed();
                                else
                                    ioPairProc = new IOPairProcessed(dbConnection);

                                if (emplIdsBCIndirect.Length > 0)
                                    IOPairListIndirectBC = ioPairProc.SearchAllPairsForEmpl(emplIdsBCIndirect, datesList, "");
                                else
                                    IOPairListIndirectBC = new List<IOPairProcessedTO>();


                                int presenceDirect = 0;
                                int absenceDirect = 0;
                                int totalDirect = 0;
                                if (emplIdsBCDirect.Length > 0)
                                {
                                    absenceDirect = emplIdsBCDirect.Trim().Split(',').Length;
                                    presenceDirect = TotalCalcPresence(dbConnection, presenceDirect, IOPairListDirectBC, emplIdsBCDirect, company, listPassTypes);
                                    absenceDirect -= presenceDirect;
                                }

                                int presenceIndirect = 0;
                                int totalIndirect = 0;
                                int absenceIndirect = 0;
                                if (emplIdsBCIndirect.Length > 0)
                                {
                                    absenceIndirect = emplIdsBCIndirect.Trim().Split(',').Length;
                                    presenceIndirect = TotalCalcPresence(dbConnection, presenceIndirect, IOPairListIndirectBC, emplIdsBCIndirect, company, listPassTypes);
                                    absenceIndirect -= presenceIndirect;
                                }
                                DataTable dtable = new DataTable(costCenter.Name + "BC");
                                if (isAltLang)
                                {
                                    dtable.Columns.Add("1", typeof(string));
                                    dtable.Columns.Add("   ", typeof(string));
                                    dtable.Columns.Add("      ", typeof(string));
                                    dtable.Columns.Add("Direct", typeof(string));
                                    dtable.Columns.Add("Indirect", typeof(string));
                                    dtable.Columns.Add("Total", typeof(string));
                                }
                                else
                                {
                                    dtable.Columns.Add("1", typeof(string));
                                    dtable.Columns.Add("   ", typeof(string));
                                    dtable.Columns.Add("      ", typeof(string));
                                    dtable.Columns.Add("Direktan", typeof(string));
                                    dtable.Columns.Add("Indirektan", typeof(string));
                                    dtable.Columns.Add("Ukupno", typeof(string));
                                }
                                totalDirect = presenceDirect + absenceDirect;
                                totalIndirect = presenceIndirect + absenceIndirect;
                                numOfEmployees = emplIdsBC.Split(',').Length;
                                numChart++;
                                populateDataTablePA(rowStart, dtable, true, isAltLang, presenceDirect, absenceDirect, presenceIndirect, absenceIndirect, dbConnection, listWorkshop[listWorkshop.Count - 1], "BC", datesList, numOfEmployees, company, 1);


                                page++;
                                ds.Tables.Add(dtable);
                                ds.AcceptChanges();

                            }
                            if (emplIds.Length > 0)
                            {
                                dictEmplTypes = Misc.emplBranch(dbConnection, emplIds, company);
                                string emplIdsDirect = "";
                                string emplIdsIndirect = "";
                                foreach (KeyValuePair<string, string> pair in dictEmplTypes)
                                {
                                    if (pair.Value.Equals("A"))
                                        emplIdsDirect += pair.Key + ",";
                                    else
                                        emplIdsIndirect += pair.Key + ",";
                                }

                                if (emplIdsDirect.Length > 0)
                                    emplIdsDirect = emplIdsDirect.Remove(emplIdsDirect.LastIndexOf(','));

                                if (emplIdsIndirect.Length > 0)
                                    emplIdsIndirect = emplIdsIndirect.Remove(emplIdsIndirect.LastIndexOf(','));
                                List<IOPairProcessedTO> IOPairListDirect = new List<IOPairProcessedTO>();
                                List<IOPairProcessedTO> IOPairListIndirect = new List<IOPairProcessedTO>();
                                if (dbConnection == null)
                                    ioPairProc = new IOPairProcessed();
                                else
                                    ioPairProc = new IOPairProcessed(dbConnection);
                                if (emplIdsDirect.Length > 0)
                                    IOPairListDirect = new IOPairProcessed().SearchAllPairsForEmpl(emplIdsDirect, datesList, "");
                                else
                                    IOPairListDirect = new List<IOPairProcessedTO>();
                                if (dbConnection == null)
                                    ioPairProc = new IOPairProcessed();
                                else
                                    ioPairProc = new IOPairProcessed(dbConnection);

                                if (emplIdsIndirect.Length > 0)
                                    IOPairListIndirect = new IOPairProcessed().SearchAllPairsForEmpl(emplIdsIndirect, datesList, "");
                                else
                                    IOPairListIndirect = new List<IOPairProcessedTO>();

                                int presenceDirect = 0;
                                int absenceDirect = 0;
                                int totalDirect = 0;
                                if (emplIdsDirect.Length > 0)
                                {
                                    absenceDirect = emplIdsDirect.Split(',').Length;
                                    presenceDirect = TotalCalcPresence(dbConnection, presenceDirect, IOPairListDirect, emplIdsDirect, company, listPassTypes);
                                    absenceDirect -= presenceDirect;

                                }

                                int presenceIndirect = 0;
                                int totalIndirect = 0;
                                int absenceIndirect = 0;
                                if (emplIdsIndirect.Length > 0)
                                {
                                    absenceIndirect = emplIdsIndirect.Split(',').Length;
                                    presenceIndirect = TotalCalcPresence(dbConnection, presenceIndirect, IOPairListIndirect, emplIdsIndirect, company, listPassTypes);
                                    absenceIndirect -= presenceIndirect;
                                }
                                DataTable dtable = new DataTable(costCenter.Name + "WC");
                                if (isAltLang)
                                {
                                    dtable.Columns.Add("1", typeof(string));
                                    dtable.Columns.Add("   ", typeof(string));
                                    dtable.Columns.Add("      ", typeof(string));
                                    dtable.Columns.Add("Direct", typeof(string));
                                    dtable.Columns.Add("Indirect", typeof(string));
                                    dtable.Columns.Add("Total", typeof(string));
                                }
                                else
                                {
                                    dtable.Columns.Add("1", typeof(string));
                                    dtable.Columns.Add("   ", typeof(string));
                                    dtable.Columns.Add("      ", typeof(string));
                                    dtable.Columns.Add("Direktan", typeof(string));
                                    dtable.Columns.Add("Indirektan", typeof(string));
                                    dtable.Columns.Add("Ukupno", typeof(string));
                                }
                                totalDirect = presenceDirect + absenceDirect;
                                totalIndirect = presenceIndirect + absenceIndirect;
                                numOfEmployees = emplIds.Split(',').Length;
                                numChart++;
                                rowStart = populateDataTablePA(rowStart, dtable, false, isAltLang, presenceDirect, absenceDirect, presenceIndirect, absenceIndirect, dbConnection, listWorkshop[listWorkshop.Count - 1], "WC", datesList, numOfEmployees, company, 1);

                                ds.Tables.Add(dtable);
                                ds.AcceptChanges();


                                page++;

                            }

                        }
                    }


                    if (dbConnection == null)
                    {
                        Employee = new Employee();
                        workingUnit = new WorkingUnit();
                    }
                    else
                    {
                        Employee = new Employee(dbConnection);
                        workingUnit = new WorkingUnit(dbConnection);
                    }
                    listCostCenter = new List<WorkingUnitTO>();
                    listCostCenter.Add(plant);
                    listCostCenter = workingUnit.FindAllChildren(listCostCenter);

                    string wuID = "";
                    foreach (WorkingUnitTO wu in listCostCenter)
                    {
                        if (plant.WorkingUnitID != wu.WorkingUnitID)
                            wuID += wu.WorkingUnitID + ",";
                    }
                    if (wuID.Length > 0)
                        wuID = wuID.Remove(wuID.LastIndexOf(','));

                    List<EmployeeTO> listEmplPlant = new List<EmployeeTO>();

                    listEmplPlant = Employee.SearchByWULoans(wuID, -1, null, datesList[0], datesList[datesList.Count - 1]);

                    if (listEmplPlant.Count > 0)
                    {
                        Dictionary<string, string> dictEmplTypes = Misc.emplTypes(dbConnection, listEmplPlant, company);

                        string emplIdsBC = "";
                        string emplIds = "";
                        foreach (KeyValuePair<string, string> pair in dictEmplTypes)
                        {
                            if (pair.Value.Equals("BC"))
                                emplIdsBC += pair.Key + ",";
                            else
                                emplIds += pair.Key + ",";
                        }

                        if (emplIdsBC.Length > 0)
                            emplIdsBC = emplIdsBC.Remove(emplIdsBC.LastIndexOf(','));

                        if (emplIds.Length > 0)
                            emplIds = emplIds.Remove(emplIds.LastIndexOf(','));

                        if (emplIdsBC.Length > 0)
                        {

                            //dtable.Clear();
                            dictEmplTypes = Misc.emplBranch(dbConnection, emplIdsBC, company);
                            string emplIdsBCDirect = "";
                            string emplIdsBCIndirect = "";
                            foreach (KeyValuePair<string, string> pair in dictEmplTypes)
                            {
                                if (pair.Value.Equals("A"))
                                    emplIdsBCDirect += pair.Key + ",";
                                else
                                    emplIdsBCIndirect += pair.Key + ",";
                            }

                            if (emplIdsBCDirect.Length > 0)
                                emplIdsBCDirect = emplIdsBCDirect.Remove(emplIdsBCDirect.LastIndexOf(','));

                            if (emplIdsBCIndirect.Length > 0)
                                emplIdsBCIndirect = emplIdsBCIndirect.Remove(emplIdsBCIndirect.LastIndexOf(','));
                            List<IOPairProcessedTO> IOPairListDirectBC = new List<IOPairProcessedTO>();
                            List<IOPairProcessedTO> IOPairListIndirectBC = new List<IOPairProcessedTO>();
                            if (dbConnection == null)
                                ioPairProc = new IOPairProcessed();
                            else
                                ioPairProc = new IOPairProcessed(dbConnection);

                            if (emplIdsBCDirect.Length > 0)
                                IOPairListDirectBC = ioPairProc.SearchAllPairsForEmpl(emplIdsBCDirect, datesList, "");
                            else
                                IOPairListDirectBC = new List<IOPairProcessedTO>();

                            if (dbConnection == null)
                                ioPairProc = new IOPairProcessed();
                            else
                                ioPairProc = new IOPairProcessed(dbConnection);

                            if (emplIdsBCIndirect.Length > 0)
                                IOPairListIndirectBC = ioPairProc.SearchAllPairsForEmpl(emplIdsBCIndirect, datesList, "");
                            else
                                IOPairListIndirectBC = new List<IOPairProcessedTO>();


                            int presenceDirect = 0;
                            int absenceDirect = 0;
                            int totalDirect = 0;
                            if (emplIdsBCDirect.Length > 0)
                            {
                                absenceDirect = emplIdsBCDirect.Split(',').Length;
                                presenceDirect = TotalCalcPresence(dbConnection, presenceDirect, IOPairListDirectBC, emplIdsBCDirect, company, listPassTypes);
                                absenceDirect -= presenceDirect;
                            }

                            int presenceIndirect = 0;
                            int totalIndirect = 0;
                            int absenceIndirect = 0;
                            if (emplIdsBCIndirect.Length > 0)
                            {
                                absenceIndirect = emplIdsBCIndirect.Split(',').Length;
                                presenceIndirect = TotalCalcPresence(dbConnection, presenceIndirect, IOPairListIndirectBC, emplIdsBCIndirect, company, listPassTypes);
                                absenceIndirect -= presenceIndirect;
                            }
                            DataTable dtable = new DataTable(plant.Name + "BC");
                            if (isAltLang)
                            {
                                dtable.Columns.Add("1", typeof(string));
                                dtable.Columns.Add("   ", typeof(string));
                                dtable.Columns.Add("      ", typeof(string));
                                dtable.Columns.Add("Direct", typeof(string));
                                dtable.Columns.Add("Indirect", typeof(string));
                                dtable.Columns.Add("Total", typeof(string));
                            }
                            else
                            {
                                dtable.Columns.Add("1", typeof(string));
                                dtable.Columns.Add("   ", typeof(string));
                                dtable.Columns.Add("      ", typeof(string));
                                dtable.Columns.Add("Direktan", typeof(string));
                                dtable.Columns.Add("Indirektan", typeof(string));
                                dtable.Columns.Add("Ukupno", typeof(string));
                            }
                            totalDirect = presenceDirect + absenceDirect;
                            totalIndirect = presenceIndirect + absenceIndirect;
                            numOfEmployees = emplIdsBC.Split(',').Length;
                            numChart++;
                            populateDataTablePA(rowStart, dtable, true, isAltLang, presenceDirect, absenceDirect, presenceIndirect, absenceIndirect, dbConnection, listCostCenter[listCostCenter.Count - 1], "BC", datesList, numOfEmployees, company, 2);


                            page++;
                            ds.Tables.Add(dtable);
                            ds.AcceptChanges();

                        }
                        if (emplIds.Length > 0)
                        {
                            dictEmplTypes = Misc.emplBranch(dbConnection, emplIds, company);
                            string emplIdsDirect = "";
                            string emplIdsIndirect = "";
                            foreach (KeyValuePair<string, string> pair in dictEmplTypes)
                            {
                                if (pair.Value.Equals("A"))
                                    emplIdsDirect += pair.Key + ",";
                                else
                                    emplIdsIndirect += pair.Key + ",";
                            }

                            if (emplIdsDirect.Length > 0)
                                emplIdsDirect = emplIdsDirect.Remove(emplIdsDirect.LastIndexOf(','));

                            if (emplIdsIndirect.Length > 0)
                                emplIdsIndirect = emplIdsIndirect.Remove(emplIdsIndirect.LastIndexOf(','));
                            List<IOPairProcessedTO> IOPairListDirect = new List<IOPairProcessedTO>();
                            List<IOPairProcessedTO> IOPairListIndirect = new List<IOPairProcessedTO>();
                            if (dbConnection == null)
                                ioPairProc = new IOPairProcessed();
                            else
                                ioPairProc = new IOPairProcessed(dbConnection);
                            if (emplIdsDirect.Length > 0)
                                IOPairListDirect = new IOPairProcessed().SearchAllPairsForEmpl(emplIdsDirect, datesList, "");
                            else
                                IOPairListDirect = new List<IOPairProcessedTO>();
                            if (dbConnection == null)
                                ioPairProc = new IOPairProcessed();
                            else
                                ioPairProc = new IOPairProcessed(dbConnection);

                            if (emplIdsIndirect.Length > 0)
                                IOPairListIndirect = new IOPairProcessed().SearchAllPairsForEmpl(emplIdsIndirect, datesList, "");
                            else
                                IOPairListIndirect = new List<IOPairProcessedTO>();

                            int presenceDirect = 0;
                            int absenceDirect = 0;
                            int totalDirect = 0;
                            if (emplIdsDirect.Length > 0)
                            {
                                absenceDirect = emplIdsDirect.Split(',').Length;
                                presenceDirect = TotalCalcPresence(dbConnection, presenceDirect, IOPairListDirect, emplIdsDirect, company, listPassTypes);
                                absenceDirect -= presenceDirect;

                            }

                            int presenceIndirect = 0;
                            int totalIndirect = 0;
                            int absenceIndirect = 0;
                            if (emplIdsIndirect.Length > 0)
                            {
                                absenceIndirect = emplIdsIndirect.Split(',').Length;
                                presenceIndirect = TotalCalcPresence(dbConnection, presenceIndirect, IOPairListIndirect, emplIdsIndirect, company, listPassTypes);
                                absenceIndirect -= presenceIndirect;
                            }
                            DataTable dtable = new DataTable(plant.Name + "WC");
                            if (isAltLang)
                            {
                                dtable.Columns.Add("1", typeof(string));
                                dtable.Columns.Add("   ", typeof(string));
                                dtable.Columns.Add("      ", typeof(string));
                                dtable.Columns.Add("Direct", typeof(string));
                                dtable.Columns.Add("Indirect", typeof(string));
                                dtable.Columns.Add("Total", typeof(string));
                            }
                            else
                            {
                                dtable.Columns.Add("1", typeof(string));
                                dtable.Columns.Add("   ", typeof(string));
                                dtable.Columns.Add("      ", typeof(string));
                                dtable.Columns.Add("Direktan", typeof(string));
                                dtable.Columns.Add("Indirektan", typeof(string));
                                dtable.Columns.Add("Ukupno", typeof(string));
                            }
                            totalDirect = presenceDirect + absenceDirect;
                            totalIndirect = presenceIndirect + absenceIndirect;
                            numOfEmployees = emplIds.Split(',').Length;
                            numChart++;
                            rowStart = populateDataTablePA(rowStart, dtable, false, isAltLang, presenceDirect, absenceDirect, presenceIndirect, absenceIndirect, dbConnection, listCostCenter[listCostCenter.Count - 1], "WC", datesList, numOfEmployees, company, 2);

                            ds.Tables.Add(dtable);
                            ds.AcceptChanges();


                            page++;


                        }
                    }
                }
                if (dbConnection == null)
                {
                    Employee = new Employee();
                    workingUnit = new WorkingUnit();
                }
                else
                {
                    Employee = new Employee(dbConnection);
                    workingUnit = new WorkingUnit(dbConnection);
                }

                //listCostCenter = workingUnit.FindAllChildren(listCostCenter);
                listPlants = new List<WorkingUnitTO>();
                listPlants.Add(workingUnit.FindWU(company));
                listPlants = workingUnit.FindAllChildren(listPlants);

                string wunits = "";
                foreach (WorkingUnitTO wu in listPlants)
                {
                    if (company != wu.WorkingUnitID)
                        wunits += wu.WorkingUnitID + ",";
                }
                if (wunits.Length > 0)
                    wunits = wunits.Remove(wunits.LastIndexOf(','));

                List<EmployeeTO> listEmplCompany = new List<EmployeeTO>();

                listEmplCompany = Employee.SearchByWULoans(wunits, -1, null, datesList[0], datesList[datesList.Count - 1]);

                if (listEmplCompany.Count > 0)
                {
                    Dictionary<string, string> dictEmplTypes = Misc.emplTypes(dbConnection, listEmplCompany, company);

                    string emplIdsBC = "";
                    string emplIds = "";
                    foreach (KeyValuePair<string, string> pair in dictEmplTypes)
                    {
                        if (pair.Value.Equals("BC"))
                            emplIdsBC += pair.Key + ",";
                        else
                            emplIds += pair.Key + ",";
                    }

                    if (emplIdsBC.Length > 0)
                        emplIdsBC = emplIdsBC.Remove(emplIdsBC.LastIndexOf(','));

                    if (emplIds.Length > 0)
                        emplIds = emplIds.Remove(emplIds.LastIndexOf(','));

                    if (emplIdsBC.Length > 0)
                    {

                        //dtable.Clear();
                        dictEmplTypes = Misc.emplBranch(dbConnection, emplIdsBC, company);
                        string emplIdsBCDirect = "";
                        string emplIdsBCIndirect = "";
                        foreach (KeyValuePair<string, string> pair in dictEmplTypes)
                        {
                            if (pair.Value.Equals("A"))
                                emplIdsBCDirect += pair.Key + ",";
                            else
                                emplIdsBCIndirect += pair.Key + ",";
                        }

                        if (emplIdsBCDirect.Length > 0)
                            emplIdsBCDirect = emplIdsBCDirect.Remove(emplIdsBCDirect.LastIndexOf(','));

                        if (emplIdsBCIndirect.Length > 0)
                            emplIdsBCIndirect = emplIdsBCIndirect.Remove(emplIdsBCIndirect.LastIndexOf(','));
                        List<IOPairProcessedTO> IOPairListDirectBC = new List<IOPairProcessedTO>();
                        List<IOPairProcessedTO> IOPairListIndirectBC = new List<IOPairProcessedTO>();
                        if (dbConnection == null)
                            ioPairProc = new IOPairProcessed();
                        else
                            ioPairProc = new IOPairProcessed(dbConnection);

                        if (emplIdsBCDirect.Length > 0)
                            IOPairListDirectBC = ioPairProc.SearchAllPairsForEmpl(emplIdsBCDirect, datesList, "");
                        else
                            IOPairListDirectBC = new List<IOPairProcessedTO>();

                        if (dbConnection == null)
                            ioPairProc = new IOPairProcessed();
                        else
                            ioPairProc = new IOPairProcessed(dbConnection);

                        if (emplIdsBCIndirect.Length > 0)
                            IOPairListIndirectBC = ioPairProc.SearchAllPairsForEmpl(emplIdsBCIndirect, datesList, "");
                        else
                            IOPairListIndirectBC = new List<IOPairProcessedTO>();


                        int presenceDirect = 0;
                        int absenceDirect = 0;
                        int totalDirect = 0;
                        if (emplIdsBCDirect.Length > 0)
                        {
                            absenceDirect = emplIdsBCDirect.Split(',').Length;
                            presenceDirect = TotalCalcPresence(dbConnection, presenceDirect, IOPairListDirectBC, emplIdsBCDirect, company, listPassTypes);
                            absenceDirect -= presenceDirect;
                        }

                        int presenceIndirect = 0;
                        int totalIndirect = 0;
                        int absenceIndirect = 0;
                        if (emplIdsBCIndirect.Length > 0)
                        {
                            absenceIndirect = emplIdsBCIndirect.Split(',').Length;
                            presenceIndirect = TotalCalcPresence(dbConnection, presenceIndirect, IOPairListIndirectBC, emplIdsBCIndirect, company, listPassTypes);
                            absenceIndirect -= presenceIndirect;
                        }
                        DataTable dtable = new DataTable(company + "BC");
                        if (isAltLang)
                        {
                            dtable.Columns.Add("1", typeof(string));
                            dtable.Columns.Add("   ", typeof(string));
                            dtable.Columns.Add("      ", typeof(string));
                            dtable.Columns.Add("Direct", typeof(string));
                            dtable.Columns.Add("Indirect", typeof(string));
                            dtable.Columns.Add("Total", typeof(string));
                        }
                        else
                        {
                            dtable.Columns.Add("1", typeof(string));
                            dtable.Columns.Add("   ", typeof(string));
                            dtable.Columns.Add("      ", typeof(string));
                            dtable.Columns.Add("Direktan", typeof(string));
                            dtable.Columns.Add("Indirektan", typeof(string));
                            dtable.Columns.Add("Ukupno", typeof(string));
                        }
                        totalDirect = presenceDirect + absenceDirect;
                        totalIndirect = presenceIndirect + absenceIndirect;
                        numOfEmployees = emplIdsBC.Split(',').Length;
                        numChart++;
                        populateDataTablePA(rowStart, dtable, true, isAltLang, presenceDirect, absenceDirect, presenceIndirect, absenceIndirect, dbConnection, listPlants[listPlants.Count - 1], "BC", datesList, numOfEmployees, company, 3);


                        page++;
                        ds.Tables.Add(dtable);
                        ds.AcceptChanges();

                    }
                    if (emplIds.Length > 0)
                    {
                        dictEmplTypes = Misc.emplBranch(dbConnection, emplIds, company);
                        string emplIdsDirect = "";
                        string emplIdsIndirect = "";
                        foreach (KeyValuePair<string, string> pair in dictEmplTypes)
                        {
                            if (pair.Value.Equals("A"))
                                emplIdsDirect += pair.Key + ",";
                            else
                                emplIdsIndirect += pair.Key + ",";
                        }

                        if (emplIdsDirect.Length > 0)
                            emplIdsDirect = emplIdsDirect.Remove(emplIdsDirect.LastIndexOf(','));

                        if (emplIdsIndirect.Length > 0)
                            emplIdsIndirect = emplIdsIndirect.Remove(emplIdsIndirect.LastIndexOf(','));
                        List<IOPairProcessedTO> IOPairListDirect = new List<IOPairProcessedTO>();
                        List<IOPairProcessedTO> IOPairListIndirect = new List<IOPairProcessedTO>();
                        if (dbConnection == null)
                            ioPairProc = new IOPairProcessed();
                        else
                            ioPairProc = new IOPairProcessed(dbConnection);
                        if (emplIdsDirect.Length > 0)
                            IOPairListDirect = new IOPairProcessed().SearchAllPairsForEmpl(emplIdsDirect, datesList, "");
                        else
                            IOPairListDirect = new List<IOPairProcessedTO>();
                        if (dbConnection == null)
                            ioPairProc = new IOPairProcessed();
                        else
                            ioPairProc = new IOPairProcessed(dbConnection);

                        if (emplIdsIndirect.Length > 0)
                            IOPairListIndirect = new IOPairProcessed().SearchAllPairsForEmpl(emplIdsIndirect, datesList, "");
                        else
                            IOPairListIndirect = new List<IOPairProcessedTO>();

                        int presenceDirect = 0;
                        int absenceDirect = 0;
                        int totalDirect = 0;
                        if (emplIdsDirect.Length > 0)
                        {
                            absenceDirect = emplIdsDirect.Split(',').Length;
                            presenceDirect = TotalCalcPresence(dbConnection, presenceDirect, IOPairListDirect, emplIdsDirect, company, listPassTypes);
                            absenceDirect -= presenceDirect;

                        }

                        int presenceIndirect = 0;
                        int totalIndirect = 0;
                        int absenceIndirect = 0;
                        if (emplIdsIndirect.Length > 0)
                        {
                            absenceIndirect = emplIdsIndirect.Split(',').Length;
                            presenceIndirect = TotalCalcPresence(dbConnection, presenceIndirect, IOPairListIndirect, emplIdsIndirect, company, listPassTypes);
                            absenceIndirect -= presenceIndirect;
                        }
                        DataTable dtable = new DataTable(company + "WC");
                        if (isAltLang)
                        {
                            dtable.Columns.Add("1", typeof(string));
                            dtable.Columns.Add("   ", typeof(string));
                            dtable.Columns.Add("      ", typeof(string));
                            dtable.Columns.Add("Direct", typeof(string));
                            dtable.Columns.Add("Indirect", typeof(string));
                            dtable.Columns.Add("Total", typeof(string));
                        }
                        else
                        {
                            dtable.Columns.Add("1", typeof(string));
                            dtable.Columns.Add("   ", typeof(string));
                            dtable.Columns.Add("      ", typeof(string));
                            dtable.Columns.Add("Direktan", typeof(string));
                            dtable.Columns.Add("Indirektan", typeof(string));
                            dtable.Columns.Add("Ukupno", typeof(string));
                        }
                        totalDirect = presenceDirect + absenceDirect;
                        totalIndirect = presenceIndirect + absenceIndirect;
                        numOfEmployees = emplIds.Split(',').Length;
                        numChart++;
                        rowStart = populateDataTablePA(rowStart, dtable, false, isAltLang, presenceDirect, absenceDirect, presenceIndirect, absenceIndirect, dbConnection, listPlants[listPlants.Count - 1], "WC", datesList, numOfEmployees, company, 3);

                        ds.Tables.Add(dtable);
                        ds.AcceptChanges();


                        page++;
                    }
                }
                string Pathh = Directory.GetParent(filePath).FullName;
                if (!Directory.Exists(Pathh))
                {
                    Directory.CreateDirectory(Pathh);
                }
                if (File.Exists(filePath))
                    File.Delete(filePath);

                ExportToExcel.CreateExcelDocument(ds, filePath, true, false);


                debug.writeLog("+ Finished presence and absence report! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                return mypath;
            }
            catch (Exception ex)
            {

                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "generateExcelPA() " + ex.Message);
                return "";
            }
        }


        private int TotalCalcPresence(Object dbConnection, int presence, List<IOPairProcessedTO> IOPairList, string emplIds, int company, List<PassTypeTO> passTypesAll)
        {
            List<string> listEmpl = new List<string>();
            Employee Employee;
            Common.Rule rule;
            //if (dbConnection == null)
            //{
            //    Employee = new Employee();
            //    rule = new Common.Rule();
            //}
            //else
            //{
            //    Employee = new Employee(dbConnection);
            //    rule = new Common.Rule(dbConnection);
            //}
            foreach (string empls in emplIds.Split(','))
            {
                if (dbConnection == null)
                {
                    Employee = new Employee();
                    rule = new Common.Rule();
                }
                else
                {
                    Employee = new Employee(dbConnection);
                    rule = new Common.Rule(dbConnection);
                }
                List<string> listPassTypesPresence = new List<string>();
                int pass_type_id = -1;

                EmployeeTO empl = Employee.Find(empls);
                if (company != -1)
                {

                    rule.RuleTO.WorkingUnitID = company;
                    rule.RuleTO.EmployeeTypeID = empl.EmployeeTypeID;
                    rule.RuleTO.RuleType = Constants.RuleCompanyRegularWork;

                    List<RuleTO> rules = rule.Search();

                    if (rules.Count == 1)
                    {
                        //  pass_type_presence += rules[0].RuleValue + ",";
                        pass_type_id = rules[0].RuleValue;
                        listPassTypesPresence.Add(pass_type_id.ToString());

                    }


                    foreach (PassTypeTO pt in passTypesAll)
                    {
                        if (pt.IsPass == Constants.overtimePassType)
                        {
                            listPassTypesPresence.Add(pt.PassTypeID.ToString());
                        }
                    }


                    foreach (IOPairProcessedTO iopair in IOPairList)
                    {
                        if (listPassTypesPresence.Contains(iopair.PassTypeID.ToString()))
                        {
                            if (!listEmpl.Contains(iopair.EmployeeID.ToString()))
                            {
                                listEmpl.Add(iopair.EmployeeID.ToString());
                                presence++;
                            }

                        }

                    }
                }

            }
            return presence;
        }


        private int populateDataTablePA(int rowStart, DataTable dt, bool isDirect, bool isAltLang, int presenceDirect, int absenceDirect, int presenceIndirect, int absenceIndirect, object dbConnection, WorkingUnitTO workingUnit, string qualify, List<DateTime> datesList, int numOfEmployees, int company, int level)
        {
            try
            {
                WorkingUnit wu;
                if (dbConnection == null)
                    wu = new WorkingUnit();
                else
                    wu = new WorkingUnit(dbConnection);

                string ute = "";
                string workGroup = "";
                string costCenter = "";
                string plant = "";
                string title = "Presence and absence";
                if (!isAltLang)
                    title = "Prisustva i odsustva";


                WorkingUnitTO tempWU = wu.FindWU(workingUnit.WorkingUnitID);
                ute = tempWU.Code.Trim();
                wu.WUTO = tempWU;
                tempWU = wu.getParentWorkingUnit();
                workGroup = tempWU.Code.Trim();
                // get cost centar
                wu.WUTO = tempWU;
                tempWU = wu.getParentWorkingUnit();
                costCenter = tempWU.Code.Trim();

                // get plant
                wu.WUTO = tempWU;
                tempWU = wu.getParentWorkingUnit();
                plant = tempWU.Code.Trim();
                DataRow row1 = dt.NewRow();
                if (dbConnection == null)
                    row1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
                else
                    row1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
                row1[4] = DateTime.Now.ToString("dd/MM/yyyy");
                row1[5] = DateTime.Now.ToString("HH:mm");
                dt.Rows.Add(row1);
                rowStart++;

                DataRow row2 = dt.NewRow();
                row2[1] = title;
                row2[4] = datesList[0].ToString("dd.MM") + "-" + datesList[datesList.Count - 1].ToString("dd.MM");
                row2[5] = datesList[0].ToString("yyyy");
                dt.Rows.Add(row2);
                rowStart++;
                DataRow rowQualify = dt.NewRow();
                if (isAltLang)

                    rowQualify[1] = "Qualify:   " + qualify;
                else
                    rowQualify[1] = "Qualify:   " + qualify;

                dt.Rows.Add(rowQualify);
                rowStart++;
                DataRow rowPlant = dt.NewRow();
                if (level == 1)
                {
                    if (isAltLang)
                        rowPlant[1] = "Plant:     " + plant + "     Cost center: " + costCenter;
                    else
                        rowPlant[1] = "Fabrika:     " + plant + "     Mesto troška: " + costCenter;
                }
                else if (level == 2)
                {
                    if (isAltLang)
                        rowPlant[1] = "Plant:     " + plant + "     Cost center: " + "xxxx";
                    else
                        rowPlant[1] = "Fabrika:     " + plant + "     Mesto troška: " + "xxxx";

                }
                else
                {
                    if (isAltLang)
                        rowPlant[1] = "Company report";
                    else
                        rowPlant[1] = "Izveštaj za celu kompaniju";
                }
                dt.Rows.Add(rowPlant);
                rowStart++;

                DataRow rowColumns = dt.NewRow();
                if (isAltLang)
                {
                    rowColumns[3] = "Direct";
                    rowColumns[4] = "Indirect";
                    rowColumns[5] = "Total";
                }
                else
                {
                    rowColumns[3] = "Direktan";
                    rowColumns[4] = "Indirektan";
                    rowColumns[5] = "Ukupno";
                }
                dt.Rows.Add(rowColumns);
                rowStart++;
                DataRow rowEmptyFirst = dt.NewRow();
                dt.Rows.Add(rowEmptyFirst);
                rowStart++;
                DataRow rowPresence = dt.NewRow();
                rowPresence[0] = " ";

                if (isAltLang)
                    rowPresence[1] = "Presence";
                else
                    rowPresence[1] = "Prisutno";
                rowPresence[2] = " ";
                rowPresence[3] = presenceDirect;
                rowPresence[4] = presenceIndirect;
                rowPresence[5] = presenceDirect + presenceIndirect;

                DataRow rowAbsence = dt.NewRow();
                rowAbsence[0] = " ";

                if (isAltLang)
                    rowAbsence[1] = "Absence";
                else
                    rowAbsence[1] = "Odsutno";
                rowAbsence[2] = " ";
                rowAbsence[3] = absenceDirect;
                rowAbsence[4] = absenceIndirect;
                rowAbsence[5] = absenceDirect + absenceIndirect;

                DataRow rowTotal = dt.NewRow();
                rowTotal[0] = " ";
                if (isAltLang)
                    rowTotal[1] = "Total";
                else
                    rowTotal[1] = "Ukupno";
                rowTotal[2] = " ";
                rowTotal[3] = int.Parse(rowPresence[3].ToString()) + int.Parse(rowAbsence[3].ToString());
                rowTotal[4] = int.Parse(rowPresence[4].ToString()) + int.Parse(rowAbsence[4].ToString());
                rowTotal[5] = int.Parse(rowPresence[5].ToString()) + int.Parse(rowAbsence[5].ToString());

                dt.Rows.Add(rowPresence);
                dt.Rows.Add(rowAbsence);
                dt.Rows.Add(rowTotal);
                rowStart += 3;
                int rowI = rowStart;
                TimeSpan timeSpan = datesList[datesList.Count - 1] - datesList[0].AddDays(-1);

                double totalDays = numOfEmployees * timeSpan.TotalDays;
                if (isAltLang)
                {
                    DataRow row = dt.NewRow();
                    row[1] = "N° Employees: " + numOfEmployees;

                    row[2] = "Total days: ";
                    row[3] = totalDays;


                    row[4] = "Calendar days";
                    row[5] = timeSpan.TotalDays;
                    dt.Rows.Add(row);
                }
                else
                {
                    DataRow row = dt.NewRow();
                    row[1] = "Br. zaposlenih: " + numOfEmployees;

                    row[2] = "Ukupno: ";
                    row[3] = totalDays;


                    row[4] = "Kalendar. dana";
                    row[5] = timeSpan.TotalDays;
                    dt.Rows.Add(row);

                }

                dt.AcceptChanges();
                return rowStart;
            }
            catch (Exception ex)
            {
                return rowStart;
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + ".populateTable400() " + ex.Message);

            }

        }

    }
}
