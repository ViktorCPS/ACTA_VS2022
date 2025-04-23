using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

using Common;
using TransferObjects;
using Util;

namespace ACTAWorkAnalysisReports
{
    public class ReportWageTypes
    {
        DebugLog debug;
        public ReportWageTypes()
        {

            debug = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");

        }
        public string GenerateReport(Object dbConnection, string filePath, List<WorkingUnitTO> listPlants, int company, bool isAltLang, List<DateTime> datesList)
        {
            debug.writeLog("+ Started wage types report! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            try
            {

                DataSet ds = new DataSet(Path.GetFileNameWithoutExtension(filePath));

                List<PassTypeTO> listPassTypes = new List<PassTypeTO>();

                if (dbConnection != null)
                    listPassTypes = new PassType(dbConnection).SearchForCompany(company, !isAltLang);
                else
                    listPassTypes = new PassType().SearchForCompany(company, !isAltLang);

                int numOfPassTypes = 0;
                int page = 0;
                int rowStart = 1;
                if (datesList == null)
                {
                    datesList = new List<DateTime>();
                    DateTime prevDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    prevDay = prevDay.AddDays(-1);
                    datesList.Add(prevDay);
                }
                string myPath = filePath;
                object misValue = System.Reflection.Missing.Value;

                int numChart = 0;
                int numOfEmployees = 0;
                foreach (WorkingUnitTO plant in listPlants)
                {

                    List<WorkingUnitTO> listCostCenter = new List<WorkingUnitTO>();
                    if (dbConnection == null)
                        listCostCenter = new WorkingUnit().SearchChildWU(plant.WorkingUnitID.ToString());
                    else
                        listCostCenter = new WorkingUnit(dbConnection).SearchChildWU(plant.WorkingUnitID.ToString());
                    string plantString = "";
                    foreach (WorkingUnitTO costCenter in listCostCenter)
                    {

                        List<WorkingUnitTO> listWorkshop = new List<WorkingUnitTO>();
                        listWorkshop.Add(costCenter);
                        if (dbConnection == null)
                            listWorkshop = new WorkingUnit().FindAllChildren(listWorkshop);
                        else
                            listWorkshop = new WorkingUnit(dbConnection).FindAllChildren(listWorkshop);

                        string wuIDs = "";
                        foreach (WorkingUnitTO wu in listWorkshop)
                        {
                            if (costCenter.WorkingUnitID != wu.WorkingUnitID)
                                wuIDs += wu.WorkingUnitID + ",";
                        }
                        if (wuIDs.Length > 0)
                            wuIDs = wuIDs.Remove(wuIDs.LastIndexOf(','));

                        List<EmployeeTO> listEmpl = new List<EmployeeTO>();
                        if (dbConnection == null)
                            listEmpl = new Employee().SearchByWULoans(wuIDs, -1, null, datesList[0], datesList[datesList.Count - 1]);
                        else
                            listEmpl = new Employee(dbConnection).SearchByWULoans(wuIDs, -1, null, datesList[0], datesList[datesList.Count - 1]);

                        if (listEmpl.Count > 0)
                        {
                            Dictionary<string, string> dictEmplTypes = Misc.emplTypes(dbConnection, listEmpl, company);

                            string emplIdsBS = "";
                            string emplIds = "";
                            foreach (KeyValuePair<string, string> pair in dictEmplTypes)
                            {
                                if (pair.Value.Equals("BC"))
                                    emplIdsBS += pair.Key + ",";
                                else
                                    emplIds += pair.Key + ",";
                            }

                            if (emplIdsBS.Length > 0)
                                emplIdsBS = emplIdsBS.Remove(emplIdsBS.LastIndexOf(','));

                            if (emplIds.Length > 0)
                                emplIds = emplIds.Remove(emplIds.LastIndexOf(','));


                            if (emplIdsBS.Length > 0)
                            {
                                System.Data.DataTable dtDaily = new System.Data.DataTable(costCenter.Name + "BC");

                                if (!isAltLang)
                                {
                                    dtDaily.Columns.Add("1", typeof(string));
                                    dtDaily.Columns.Add("   ", typeof(string));
                                    dtDaily.Columns.Add("Direktan", typeof(string));
                                    dtDaily.Columns.Add("%  ", typeof(string));
                                    dtDaily.Columns.Add("Indirektan", typeof(string));
                                    dtDaily.Columns.Add("%   ", typeof(string));
                                    dtDaily.Columns.Add("Ukupno", typeof(string));
                                    dtDaily.Columns.Add(" %   ", typeof(string));
                                }
                                else
                                {
                                    dtDaily.Columns.Add("1", typeof(string));
                                    dtDaily.Columns.Add("   ", typeof(string));
                                    dtDaily.Columns.Add("Direct", typeof(string));
                                    dtDaily.Columns.Add("%  ", typeof(string));
                                    dtDaily.Columns.Add("Indirect", typeof(string));
                                    dtDaily.Columns.Add("%   ", typeof(string));
                                    dtDaily.Columns.Add("Total", typeof(string));
                                    dtDaily.Columns.Add(" %   ", typeof(string));
                                }
                                ds.Tables.Add(dtDaily);
                                ds.AcceptChanges();
                                dictEmplTypes = Misc.emplBranch(dbConnection, emplIdsBS, company);
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
                                List<IOPairProcessedTO> IOPairListDirect = new List<IOPairProcessedTO>();
                                List<IOPairProcessedTO> IOPairListIndirect = new List<IOPairProcessedTO>();
                                IOPairProcessed ioPairProc;
                                if (dbConnection == null)
                                    ioPairProc = new IOPairProcessed();
                                else
                                    ioPairProc = new IOPairProcessed(dbConnection);

                                if (emplIdsBCDirect.Length > 0)
                                    IOPairListDirect = ioPairProc.SearchAllPairsForEmpl(emplIdsBCDirect, datesList, "");
                                else
                                    IOPairListDirect = new List<IOPairProcessedTO>();

                                if (dbConnection == null)
                                    ioPairProc = new IOPairProcessed();
                                else
                                    ioPairProc = new IOPairProcessed(dbConnection);

                                if (emplIdsBCIndirect.Length > 0)
                                    IOPairListIndirect = ioPairProc.SearchAllPairsForEmpl(emplIdsBCIndirect, datesList, "");
                                else
                                    IOPairListIndirect = new List<IOPairProcessedTO>();
                                if (IOPairListDirect.Count > 0 || IOPairListIndirect.Count > 0)
                                {
                                    Dictionary<int, double> dictPassTypesDirect = new Dictionary<int, double>();
                                    Dictionary<int, double> dictPassTypesIndirect = new Dictionary<int, double>();
                                    if (emplIdsBCDirect.Length > 0)
                                        TotalCalc(dictPassTypesDirect, dictPassTypesIndirect, true, IOPairListDirect, IOPairListIndirect);
                                    if (emplIdsBCIndirect.Length > 0)
                                        TotalCalc(dictPassTypesDirect, dictPassTypesIndirect, false, IOPairListDirect, IOPairListIndirect);
                                    //if (emplIdsBCDirect.Length > 0)
                                    //    CalcNightWork(dbConnection, emplIdsBCDirect, company, IOPairListDirect, dictPassTypesDirect);
                                    //if (emplIdsBCIndirect.Length > 0)
                                    //    CalcNightWork(dbConnection, emplIdsBCIndirect, company, IOPairListIndirect, dictPassTypesIndirect);


                                    //if (emplIdsBCDirect.Length > 0)
                                    //    CalcRotaryWork(dbConnection, emplIdsBCDirect, company, IOPairListDirect, datesList, dictPassTypesDirect);
                                    //if (emplIdsBCIndirect.Length > 0)
                                    //    CalcRotaryWork(dbConnection, emplIdsBCIndirect, company, IOPairListIndirect, datesList, dictPassTypesIndirect);


                                    //if (emplIdsBCDirect.Length > 0)
                                    //    CalcWorkOnHoliday(dbConnection, emplIdsBCDirect, company, IOPairListDirect, datesList, dictPassTypesDirect);
                                    //if (emplIdsBCIndirect.Length > 0)
                                    //    CalcWorkOnHoliday(dbConnection, emplIdsBCIndirect, company, IOPairListIndirect, datesList, dictPassTypesIndirect);

                                    if (dictPassTypesDirect.Count > dictPassTypesIndirect.Count)
                                        numOfPassTypes = dictPassTypesDirect.Count;
                                    else
                                    {
                                        numOfPassTypes = dictPassTypesIndirect.Count;
                                    }
                                    if (emplIdsBCDirect.Length > 0 && emplIdsBCIndirect.Length > 0)
                                        numOfEmployees = emplIdsBCIndirect.Split(',').Length + emplIdsBCDirect.Split(',').Length;
                                    else if (emplIdsBCIndirect.Length > 0)
                                        numOfEmployees = emplIdsBCIndirect.Split(',').Length;
                                    else if (emplIdsBCDirect.Length > 0)
                                        numOfEmployees = emplIdsBCDirect.Split(',').Length;
                                    page++;
                                    numChart++;
                                    populateDataTableWageTypesNew(dbConnection, dtDaily, numOfEmployees, rowStart, "BC", costCenter, datesList, company, true,
                                        dictPassTypesDirect, dictPassTypesIndirect, emplIdsBCDirect, emplIdsBCIndirect, listPassTypes, isAltLang, 1);
                                }
                            }
                            if (emplIds.Length > 0)
                            {
                                System.Data.DataTable dtDaily = new System.Data.DataTable(costCenter.Name + "WC");

                                if (!isAltLang)
                                {
                                    dtDaily.Columns.Add("1", typeof(string));
                                    dtDaily.Columns.Add("   ", typeof(string));
                                    dtDaily.Columns.Add("Direktan", typeof(string));
                                    dtDaily.Columns.Add("%  ", typeof(string));
                                    dtDaily.Columns.Add("Indirektan", typeof(string));
                                    dtDaily.Columns.Add("%   ", typeof(string));
                                    dtDaily.Columns.Add("Ukupno", typeof(string));
                                    dtDaily.Columns.Add(" %   ", typeof(string));
                                }
                                else
                                {
                                    dtDaily.Columns.Add("1", typeof(string));
                                    dtDaily.Columns.Add("   ", typeof(string));
                                    dtDaily.Columns.Add("Direct", typeof(string));
                                    dtDaily.Columns.Add("%  ", typeof(string));
                                    dtDaily.Columns.Add("Indirect", typeof(string));
                                    dtDaily.Columns.Add("%   ", typeof(string));
                                    dtDaily.Columns.Add("Total", typeof(string));
                                    dtDaily.Columns.Add(" %   ", typeof(string));
                                }
                                ds.Tables.Add(dtDaily);
                                ds.AcceptChanges();
                                dictEmplTypes = Misc.emplBranch(dbConnection, emplIds, company);
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
                                List<IOPairProcessedTO> IOPairListDirect = new List<IOPairProcessedTO>();
                                List<IOPairProcessedTO> IOPairListIndirect = new List<IOPairProcessedTO>();
                                IOPairProcessed ioPairProc;
                                if (dbConnection == null)
                                    ioPairProc = new IOPairProcessed();
                                else
                                    ioPairProc = new IOPairProcessed(dbConnection);

                                if (emplIdsBCDirect.Length > 0)
                                    IOPairListDirect = ioPairProc.SearchAllPairsForEmpl(emplIdsBCDirect, datesList, "");
                                else
                                    IOPairListDirect = new List<IOPairProcessedTO>();
                                if (dbConnection == null)
                                    ioPairProc = new IOPairProcessed();
                                else
                                    ioPairProc = new IOPairProcessed(dbConnection);
                                if (emplIdsBCIndirect.Length > 0)
                                    IOPairListIndirect = ioPairProc.SearchAllPairsForEmpl(emplIdsBCIndirect, datesList, "");
                                else
                                    IOPairListIndirect = new List<IOPairProcessedTO>();
                                if (IOPairListDirect.Count > 0 || IOPairListIndirect.Count > 0)
                                {
                                    Dictionary<int, double> dictPassTypesDirect = new Dictionary<int, double>();
                                    Dictionary<int, double> dictPassTypesIndirect = new Dictionary<int, double>();
                                    if (emplIdsBCDirect.Length > 0)
                                        TotalCalc(dictPassTypesDirect, dictPassTypesIndirect, true, IOPairListDirect, IOPairListIndirect);
                                    if (emplIdsBCIndirect.Length > 0)
                                        TotalCalc(dictPassTypesDirect, dictPassTypesIndirect, false, IOPairListDirect, IOPairListIndirect);
                                    //if (emplIdsBCDirect.Length > 0)
                                    //    CalcNightWork(dbConnection, emplIdsBCDirect, company, IOPairListDirect, dictPassTypesDirect);
                                    //if (emplIdsBCIndirect.Length > 0)
                                    //    CalcNightWork(dbConnection, emplIdsBCIndirect, company, IOPairListIndirect, dictPassTypesIndirect);


                                    //if (emplIdsBCDirect.Length > 0)
                                    //    CalcRotaryWork(dbConnection, emplIdsBCDirect, company, IOPairListDirect, datesList, dictPassTypesDirect);
                                    //if (emplIdsBCIndirect.Length > 0)
                                    //    CalcRotaryWork(dbConnection, emplIdsBCIndirect, company, IOPairListIndirect, datesList, dictPassTypesIndirect);


                                    //if (emplIdsBCDirect.Length > 0)
                                    //    CalcWorkOnHoliday(dbConnection, emplIdsBCDirect, company, IOPairListDirect, datesList, dictPassTypesDirect);
                                    //if (emplIdsBCIndirect.Length > 0)
                                    //    CalcWorkOnHoliday(dbConnection, emplIdsBCIndirect, company, IOPairListIndirect, datesList, dictPassTypesIndirect);
                                    if (dictPassTypesDirect.Count > dictPassTypesIndirect.Count)
                                        numOfPassTypes = dictPassTypesDirect.Count;
                                    else
                                    {
                                        numOfPassTypes = dictPassTypesIndirect.Count;
                                    }


                                    numChart++;
                                    if (emplIdsBCDirect.Length > 0 && emplIdsBCIndirect.Length > 0)
                                        numOfEmployees = emplIdsBCIndirect.Split(',').Length + emplIdsBCDirect.Split(',').Length;
                                    else if (emplIdsBCIndirect.Length > 0)
                                        numOfEmployees = emplIdsBCIndirect.Split(',').Length;
                                    else if (emplIdsBCDirect.Length > 0)
                                        numOfEmployees = emplIdsBCDirect.Split(',').Length;
                                    page++;

                                    populateDataTableWageTypesNew(dbConnection, dtDaily, numOfEmployees, rowStart, "WC", costCenter, datesList, company, false,
                                        dictPassTypesDirect, dictPassTypesIndirect, emplIdsBCDirect, emplIdsBCIndirect, listPassTypes, isAltLang, 1);


                                }
                            }
                        }

                    }

                    listCostCenter = new List<WorkingUnitTO>();
                    listCostCenter.Add(plant);
                    if (dbConnection == null)
                        listCostCenter = new WorkingUnit().FindAllChildren(listCostCenter);
                    else
                        listCostCenter = new WorkingUnit(dbConnection).FindAllChildren(listCostCenter);

                    string wuID = "";
                    foreach (WorkingUnitTO wu in listCostCenter)
                    {
                        if (plant.WorkingUnitID != wu.WorkingUnitID)
                            wuID += wu.WorkingUnitID + ",";
                    }
                    if (wuID.Length > 0)
                        wuID = wuID.Remove(wuID.LastIndexOf(','));

                    List<EmployeeTO> listEmplPlant = new List<EmployeeTO>();
                    if (dbConnection == null)
                        listEmplPlant = new Employee().SearchByWULoans(wuID, -1, null, datesList[0], datesList[datesList.Count - 1]);
                    else
                        listEmplPlant = new Employee(dbConnection).SearchByWULoans(wuID, -1, null, datesList[0], datesList[datesList.Count - 1]);

                    if (listEmplPlant.Count > 0)
                    {
                        Dictionary<string, string> dictEmplTypes = Misc.emplTypes(dbConnection, listEmplPlant, company);

                        string emplIdsBS = "";
                        string emplIds = "";
                        foreach (KeyValuePair<string, string> pair in dictEmplTypes)
                        {
                            if (pair.Value.Equals("BC"))
                                emplIdsBS += pair.Key + ",";
                            else
                                emplIds += pair.Key + ",";
                        }

                        if (emplIdsBS.Length > 0)
                            emplIdsBS = emplIdsBS.Remove(emplIdsBS.LastIndexOf(','));

                        if (emplIds.Length > 0)
                            emplIds = emplIds.Remove(emplIds.LastIndexOf(','));


                        if (emplIdsBS.Length > 0)
                        {
                            System.Data.DataTable dtDaily = new System.Data.DataTable(plant.Name + "BC");

                            if (!isAltLang)
                            {
                                dtDaily.Columns.Add("1", typeof(string));
                                dtDaily.Columns.Add("   ", typeof(string));
                                dtDaily.Columns.Add("Direktan", typeof(string));
                                dtDaily.Columns.Add("%  ", typeof(string));
                                dtDaily.Columns.Add("Indirektan", typeof(string));
                                dtDaily.Columns.Add("%   ", typeof(string));
                                dtDaily.Columns.Add("Ukupno", typeof(string));
                                dtDaily.Columns.Add(" %   ", typeof(string));
                            }
                            else
                            {
                                dtDaily.Columns.Add("1", typeof(string));
                                dtDaily.Columns.Add("   ", typeof(string));
                                dtDaily.Columns.Add("Direct", typeof(string));
                                dtDaily.Columns.Add("%  ", typeof(string));
                                dtDaily.Columns.Add("Indirect", typeof(string));
                                dtDaily.Columns.Add("%   ", typeof(string));
                                dtDaily.Columns.Add("Total", typeof(string));
                                dtDaily.Columns.Add(" %   ", typeof(string));
                            }
                            ds.Tables.Add(dtDaily);
                            ds.AcceptChanges();
                            dictEmplTypes = Misc.emplBranch(dbConnection, emplIdsBS, company);
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
                            List<IOPairProcessedTO> IOPairListDirect = new List<IOPairProcessedTO>();
                            List<IOPairProcessedTO> IOPairListIndirect = new List<IOPairProcessedTO>();
                            IOPairProcessed ioPairProc;
                            if (dbConnection == null)
                                ioPairProc = new IOPairProcessed();
                            else
                                ioPairProc = new IOPairProcessed(dbConnection);

                            if (emplIdsBCDirect.Length > 0)
                                IOPairListDirect = ioPairProc.SearchAllPairsForEmpl(emplIdsBCDirect, datesList, "");
                            else
                                IOPairListDirect = new List<IOPairProcessedTO>();

                            if (dbConnection == null)
                                ioPairProc = new IOPairProcessed();
                            else
                                ioPairProc = new IOPairProcessed(dbConnection);

                            if (emplIdsBCIndirect.Length > 0)
                                IOPairListIndirect = ioPairProc.SearchAllPairsForEmpl(emplIdsBCIndirect, datesList, "");
                            else
                                IOPairListIndirect = new List<IOPairProcessedTO>();
                            if (IOPairListDirect.Count > 0 || IOPairListIndirect.Count > 0)
                            {
                                Dictionary<int, double> dictPassTypesDirect = new Dictionary<int, double>();
                                Dictionary<int, double> dictPassTypesIndirect = new Dictionary<int, double>();
                                if (emplIdsBCDirect.Length > 0)
                                    TotalCalc(dictPassTypesDirect, dictPassTypesIndirect, true, IOPairListDirect, IOPairListIndirect);
                                if (emplIdsBCIndirect.Length > 0)
                                    TotalCalc(dictPassTypesDirect, dictPassTypesIndirect, false, IOPairListDirect, IOPairListIndirect);


                                if (dictPassTypesDirect.Count > dictPassTypesIndirect.Count)
                                    numOfPassTypes = dictPassTypesDirect.Count;
                                else
                                {
                                    numOfPassTypes = dictPassTypesIndirect.Count;
                                }
                                if (emplIdsBCDirect.Length > 0 && emplIdsBCIndirect.Length > 0)
                                    numOfEmployees = emplIdsBCIndirect.Split(',').Length + emplIdsBCDirect.Split(',').Length;
                                else if (emplIdsBCIndirect.Length > 0)
                                    numOfEmployees = emplIdsBCIndirect.Split(',').Length;
                                else if (emplIdsBCDirect.Length > 0)
                                    numOfEmployees = emplIdsBCDirect.Split(',').Length;
                                page++;
                                numChart++;
                                populateDataTableWageTypesNew(dbConnection, dtDaily, numOfEmployees, rowStart, "BC", listCostCenter[0], datesList, company, true,
                                    dictPassTypesDirect, dictPassTypesIndirect, emplIdsBCDirect, emplIdsBCIndirect, listPassTypes, isAltLang, 2);
                            }
                        }
                        if (emplIds.Length > 0)
                        {
                            System.Data.DataTable dtDaily = new System.Data.DataTable(plant.Name + "WC");

                            if (!isAltLang)
                            {
                                dtDaily.Columns.Add("1", typeof(string));
                                dtDaily.Columns.Add("   ", typeof(string));
                                dtDaily.Columns.Add("Direktan", typeof(string));
                                dtDaily.Columns.Add("%  ", typeof(string));
                                dtDaily.Columns.Add("Indirektan", typeof(string));
                                dtDaily.Columns.Add("%   ", typeof(string));
                                dtDaily.Columns.Add("Ukupno", typeof(string));
                                dtDaily.Columns.Add(" %   ", typeof(string));
                            }
                            else
                            {
                                dtDaily.Columns.Add("1", typeof(string));
                                dtDaily.Columns.Add("   ", typeof(string));
                                dtDaily.Columns.Add("Direct", typeof(string));
                                dtDaily.Columns.Add("%  ", typeof(string));
                                dtDaily.Columns.Add("Indirect", typeof(string));
                                dtDaily.Columns.Add("%   ", typeof(string));
                                dtDaily.Columns.Add("Total", typeof(string));
                                dtDaily.Columns.Add(" %   ", typeof(string));
                            }
                            ds.Tables.Add(dtDaily);
                            ds.AcceptChanges();
                            dictEmplTypes = Misc.emplBranch(dbConnection, emplIds, company);
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
                            List<IOPairProcessedTO> IOPairListDirect = new List<IOPairProcessedTO>();
                            List<IOPairProcessedTO> IOPairListIndirect = new List<IOPairProcessedTO>();
                            IOPairProcessed ioPairProc;
                            if (dbConnection == null)
                                ioPairProc = new IOPairProcessed();
                            else
                                ioPairProc = new IOPairProcessed(dbConnection);

                            if (emplIdsBCDirect.Length > 0)
                                IOPairListDirect = ioPairProc.SearchAllPairsForEmpl(emplIdsBCDirect, datesList, "");
                            else
                                IOPairListDirect = new List<IOPairProcessedTO>();
                            if (dbConnection == null)
                                ioPairProc = new IOPairProcessed();
                            else
                                ioPairProc = new IOPairProcessed(dbConnection);
                            if (emplIdsBCIndirect.Length > 0)
                                IOPairListIndirect = ioPairProc.SearchAllPairsForEmpl(emplIdsBCIndirect, datesList, "");
                            else
                                IOPairListIndirect = new List<IOPairProcessedTO>();
                            if (IOPairListDirect.Count > 0 || IOPairListIndirect.Count > 0)
                            {
                                Dictionary<int, double> dictPassTypesDirect = new Dictionary<int, double>();
                                Dictionary<int, double> dictPassTypesIndirect = new Dictionary<int, double>();
                                if (emplIdsBCDirect.Length > 0)
                                    TotalCalc(dictPassTypesDirect, dictPassTypesIndirect, true, IOPairListDirect, IOPairListIndirect);
                                if (emplIdsBCIndirect.Length > 0)
                                    TotalCalc(dictPassTypesDirect, dictPassTypesIndirect, false, IOPairListDirect, IOPairListIndirect);


                                if (dictPassTypesDirect.Count > dictPassTypesIndirect.Count)
                                    numOfPassTypes = dictPassTypesDirect.Count;
                                else
                                {
                                    numOfPassTypes = dictPassTypesIndirect.Count;
                                }


                                numChart++;
                                if (emplIdsBCDirect.Length > 0 && emplIdsBCIndirect.Length > 0)
                                    numOfEmployees = emplIdsBCIndirect.Split(',').Length + emplIdsBCDirect.Split(',').Length;
                                else if (emplIdsBCIndirect.Length > 0)
                                    numOfEmployees = emplIdsBCIndirect.Split(',').Length;
                                else if (emplIdsBCDirect.Length > 0)
                                    numOfEmployees = emplIdsBCDirect.Split(',').Length;
                                page++;

                                populateDataTableWageTypesNew(dbConnection, dtDaily, numOfEmployees, rowStart, "WC", listCostCenter[0], datesList, company, false,
                                    dictPassTypesDirect, dictPassTypesIndirect, emplIdsBCDirect, emplIdsBCIndirect, listPassTypes, isAltLang, 2);


                            }
                        }
                    }

                }

                listPlants = new List<WorkingUnitTO>();
                if (dbConnection == null)
                    listPlants.Add(new WorkingUnit().FindWU(company));
                else
                    listPlants.Add(new WorkingUnit(dbConnection).FindWU(company));
                if (dbConnection == null)
                    listPlants = new WorkingUnit().FindAllChildren(listPlants);
                else
                    listPlants = new WorkingUnit(dbConnection).FindAllChildren(listPlants);

                string wunits = "";
                foreach (WorkingUnitTO wu in listPlants)
                {
                    if (company != wu.WorkingUnitID)
                        wunits += wu.WorkingUnitID + ",";
                }
                if (wunits.Length > 0)
                    wunits = wunits.Remove(wunits.LastIndexOf(','));

                List<EmployeeTO> listEmplCompany = new List<EmployeeTO>();
                if (dbConnection == null)
                    listEmplCompany = new Employee().SearchByWULoans(wunits, -1, null, datesList[0], datesList[datesList.Count - 1]);
                else
                    listEmplCompany = new Employee(dbConnection).SearchByWULoans(wunits, -1, null, datesList[0], datesList[datesList.Count - 1]);

                if (listEmplCompany.Count > 0)
                {
                    Dictionary<string, string> dictEmplTypes = Misc.emplTypes(dbConnection, listEmplCompany, company);

                    string emplIdsBS = "";
                    string emplIds = "";
                    foreach (KeyValuePair<string, string> pair in dictEmplTypes)
                    {
                        if (pair.Value.Equals("BC"))
                            emplIdsBS += pair.Key + ",";
                        else
                            emplIds += pair.Key + ",";
                    }

                    if (emplIdsBS.Length > 0)
                        emplIdsBS = emplIdsBS.Remove(emplIdsBS.LastIndexOf(','));

                    if (emplIds.Length > 0)
                        emplIds = emplIds.Remove(emplIds.LastIndexOf(','));


                    if (emplIdsBS.Length > 0)
                    {
                        System.Data.DataTable dtDaily = new System.Data.DataTable(company + "BC");

                        if (!isAltLang)
                        {
                            dtDaily.Columns.Add("1", typeof(string));
                            dtDaily.Columns.Add("   ", typeof(string));
                            dtDaily.Columns.Add("Direktan", typeof(string));
                            dtDaily.Columns.Add("%  ", typeof(string));
                            dtDaily.Columns.Add("Indirektan", typeof(string));
                            dtDaily.Columns.Add("%   ", typeof(string));
                            dtDaily.Columns.Add("Ukupno", typeof(string));
                            dtDaily.Columns.Add(" %   ", typeof(string));
                        }
                        else
                        {
                            dtDaily.Columns.Add("1", typeof(string));
                            dtDaily.Columns.Add("   ", typeof(string));
                            dtDaily.Columns.Add("Direct", typeof(string));
                            dtDaily.Columns.Add("%  ", typeof(string));
                            dtDaily.Columns.Add("Indirect", typeof(string));
                            dtDaily.Columns.Add("%   ", typeof(string));
                            dtDaily.Columns.Add("Total", typeof(string));
                            dtDaily.Columns.Add(" %   ", typeof(string));
                        }
                        ds.Tables.Add(dtDaily);
                        ds.AcceptChanges();
                        dictEmplTypes = Misc.emplBranch(dbConnection, emplIdsBS, company);
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
                        List<IOPairProcessedTO> IOPairListDirect = new List<IOPairProcessedTO>();
                        List<IOPairProcessedTO> IOPairListIndirect = new List<IOPairProcessedTO>();
                        IOPairProcessed ioPairProc;
                        if (dbConnection == null)
                            ioPairProc = new IOPairProcessed();
                        else
                            ioPairProc = new IOPairProcessed(dbConnection);

                        if (emplIdsBCDirect.Length > 0)
                            IOPairListDirect = ioPairProc.SearchAllPairsForEmpl(emplIdsBCDirect, datesList, "");
                        else
                            IOPairListDirect = new List<IOPairProcessedTO>();

                        if (dbConnection == null)
                            ioPairProc = new IOPairProcessed();
                        else
                            ioPairProc = new IOPairProcessed(dbConnection);

                        if (emplIdsBCIndirect.Length > 0)
                            IOPairListIndirect = ioPairProc.SearchAllPairsForEmpl(emplIdsBCIndirect, datesList, "");
                        else
                            IOPairListIndirect = new List<IOPairProcessedTO>();
                        if (IOPairListDirect.Count > 0 || IOPairListIndirect.Count > 0)
                        {
                            Dictionary<int, double> dictPassTypesDirect = new Dictionary<int, double>();
                            Dictionary<int, double> dictPassTypesIndirect = new Dictionary<int, double>();
                            if (emplIdsBCDirect.Length > 0)
                                TotalCalc(dictPassTypesDirect, dictPassTypesIndirect, true, IOPairListDirect, IOPairListIndirect);
                            if (emplIdsBCIndirect.Length > 0)
                                TotalCalc(dictPassTypesDirect, dictPassTypesIndirect, false, IOPairListDirect, IOPairListIndirect);


                            if (dictPassTypesDirect.Count > dictPassTypesIndirect.Count)
                                numOfPassTypes = dictPassTypesDirect.Count;
                            else
                            {
                                numOfPassTypes = dictPassTypesIndirect.Count;
                            }
                            if (emplIdsBCDirect.Length > 0 && emplIdsBCIndirect.Length > 0)
                                numOfEmployees = emplIdsBCIndirect.Split(',').Length + emplIdsBCDirect.Split(',').Length;
                            else if (emplIdsBCIndirect.Length > 0)
                                numOfEmployees = emplIdsBCIndirect.Split(',').Length;
                            else if (emplIdsBCDirect.Length > 0)
                                numOfEmployees = emplIdsBCDirect.Split(',').Length;
                            page++;
                            numChart++;
                            WorkingUnitTO companyWU = new WorkingUnitTO();
                            if (dbConnection == null)
                                companyWU = new WorkingUnit().FindWU(company);
                            else
                                companyWU = new WorkingUnit(dbConnection).FindWU(company);
                            populateDataTableWageTypesNew(dbConnection, dtDaily, numOfEmployees, rowStart, "BC", companyWU, datesList, company, true,
                                dictPassTypesDirect, dictPassTypesIndirect, emplIdsBCDirect, emplIdsBCIndirect, listPassTypes, isAltLang, 3);
                        }
                    }
                    if (emplIds.Length > 0)
                    {
                        System.Data.DataTable dtDaily = new System.Data.DataTable(company + "WC");

                        if (!isAltLang)
                        {
                            dtDaily.Columns.Add("1", typeof(string));
                            dtDaily.Columns.Add("   ", typeof(string));
                            dtDaily.Columns.Add("Direktan", typeof(string));
                            dtDaily.Columns.Add("%  ", typeof(string));
                            dtDaily.Columns.Add("Indirektan", typeof(string));
                            dtDaily.Columns.Add("%   ", typeof(string));
                            dtDaily.Columns.Add("Ukupno", typeof(string));
                            dtDaily.Columns.Add(" %   ", typeof(string));
                        }
                        else
                        {
                            dtDaily.Columns.Add("1", typeof(string));
                            dtDaily.Columns.Add("   ", typeof(string));
                            dtDaily.Columns.Add("Direct", typeof(string));
                            dtDaily.Columns.Add("%  ", typeof(string));
                            dtDaily.Columns.Add("Indirect", typeof(string));
                            dtDaily.Columns.Add("%   ", typeof(string));
                            dtDaily.Columns.Add("Total", typeof(string));
                            dtDaily.Columns.Add(" %   ", typeof(string));
                        }
                        ds.Tables.Add(dtDaily);
                        ds.AcceptChanges();
                        dictEmplTypes = Misc.emplBranch(dbConnection, emplIds, company);
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
                        List<IOPairProcessedTO> IOPairListDirect = new List<IOPairProcessedTO>();
                        List<IOPairProcessedTO> IOPairListIndirect = new List<IOPairProcessedTO>();
                        IOPairProcessed ioPairProc;
                        if (dbConnection == null)
                            ioPairProc = new IOPairProcessed();
                        else
                            ioPairProc = new IOPairProcessed(dbConnection);

                        if (emplIdsBCDirect.Length > 0)
                            IOPairListDirect = ioPairProc.SearchAllPairsForEmpl(emplIdsBCDirect, datesList, "");
                        else
                            IOPairListDirect = new List<IOPairProcessedTO>();
                        if (dbConnection == null)
                            ioPairProc = new IOPairProcessed();
                        else
                            ioPairProc = new IOPairProcessed(dbConnection);
                        if (emplIdsBCIndirect.Length > 0)
                            IOPairListIndirect = ioPairProc.SearchAllPairsForEmpl(emplIdsBCIndirect, datesList, "");
                        else
                            IOPairListIndirect = new List<IOPairProcessedTO>();
                        if (IOPairListDirect.Count > 0 || IOPairListIndirect.Count > 0)
                        {
                            Dictionary<int, double> dictPassTypesDirect = new Dictionary<int, double>();
                            Dictionary<int, double> dictPassTypesIndirect = new Dictionary<int, double>();
                            if (emplIdsBCDirect.Length > 0)
                                TotalCalc(dictPassTypesDirect, dictPassTypesIndirect, true, IOPairListDirect, IOPairListIndirect);
                            if (emplIdsBCIndirect.Length > 0)
                                TotalCalc(dictPassTypesDirect, dictPassTypesIndirect, false, IOPairListDirect, IOPairListIndirect);


                            if (dictPassTypesDirect.Count > dictPassTypesIndirect.Count)
                                numOfPassTypes = dictPassTypesDirect.Count;
                            else
                            {
                                numOfPassTypes = dictPassTypesIndirect.Count;
                            }


                            numChart++;
                            if (emplIdsBCDirect.Length > 0 && emplIdsBCIndirect.Length > 0)
                                numOfEmployees = emplIdsBCIndirect.Split(',').Length + emplIdsBCDirect.Split(',').Length;
                            else if (emplIdsBCIndirect.Length > 0)
                                numOfEmployees = emplIdsBCIndirect.Split(',').Length;
                            else if (emplIdsBCDirect.Length > 0)
                                numOfEmployees = emplIdsBCDirect.Split(',').Length;
                            page++;
                            WorkingUnitTO companyWU = new WorkingUnitTO();
                            if (dbConnection == null)
                                companyWU = new WorkingUnit().FindWU(company);
                            else
                                companyWU = new WorkingUnit(dbConnection).FindWU(company);
                            populateDataTableWageTypesNew(dbConnection, dtDaily, numOfEmployees, rowStart, "WC", companyWU, datesList, company, false,
                                dictPassTypesDirect, dictPassTypesIndirect, emplIdsBCDirect, emplIdsBCIndirect, listPassTypes, isAltLang, 3);


                        }
                    }
                }
                string Pathh = Directory.GetParent(filePath).FullName;
                if (!Directory.Exists(Pathh))
                {
                    Directory.CreateDirectory(Pathh);
                }
                if (File.Exists(filePath))
                    File.Delete(filePath);

                ExportToExcel.CreateExcelDocument(ds, filePath, false, false);

                debug.writeLog("+ Finished wage types report! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                return myPath;
            }
            catch (Exception ex)
            {

                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "generateExcelWageTypes() " + ex.Message);
                return "";
            }
        }
        private void TotalCalc(Dictionary<int, double> dictDirect, Dictionary<int, double> dictIndirect, bool isDirect, List<IOPairProcessedTO> IOPairListDirect, List<IOPairProcessedTO> IOPairListIndirect)
        {
            if (isDirect)
            {
                foreach (IOPairProcessedTO iopair in IOPairListDirect)
                {

                    TimeSpan duration = iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay);
                    if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                        duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift
                    if (dictDirect.ContainsKey(iopair.PassTypeID))
                        dictDirect[iopair.PassTypeID] += duration.TotalHours;
                    else
                    {
                        dictDirect.Add(iopair.PassTypeID, duration.TotalHours);
                    }


                }
            }
            else
            {
                foreach (IOPairProcessedTO iopair in IOPairListIndirect)
                {

                    TimeSpan duration = iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay);
                    if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                        duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift
                    if (dictIndirect.ContainsKey(iopair.PassTypeID))
                        dictIndirect[iopair.PassTypeID] += duration.TotalHours;
                    else
                    {
                        dictIndirect.Add(iopair.PassTypeID, duration.TotalHours);
                    }
                }
            }

        }
        private void populateDataTableWageTypesNew(Object dbConnection, DataTable dt, int numOfEmployees, int rowStart, string qualify, WorkingUnitTO workingUnit, List<DateTime> datesList, int company, bool isDirect,
         Dictionary<int, double> dictPassTypesDirect, Dictionary<int, double> dictPassTypesIndirect, string emplIdsBS, string emplIds, List<PassTypeTO> listPassTypes, bool isAltLang, int level)
        {
            try
            {
                WorkingUnit wu;
                if (dbConnection == null)
                    wu = new WorkingUnit();
                else
                    wu = new WorkingUnit(dbConnection);

                //string ute = "";
                //string workGroup = "";
                string costCenter = "";
                string plant = "";
                string title = "Wage types";
                if (!isAltLang)
                    title = "Tipovi plaćanja";

                if (level == 1)
                {
                    WorkingUnitTO tempWU = wu.FindWU(workingUnit.WorkingUnitID);
                    costCenter = tempWU.Code.Trim();


                    wu.WUTO = tempWU;
                    tempWU = wu.getParentWorkingUnit();
                    plant = tempWU.Code.Trim();

                }
                else
                {
                    WorkingUnitTO tempWU = wu.FindWU(workingUnit.WorkingUnitID);
                    plant = tempWU.Code.Trim();

                }
                //// get cost centar
                //wu.WUTO = tempWU;
                //tempWU = wu.getParentWorkingUnit();
                //costCenter = tempWU.Code.Trim();

                //// get plant
                //wu.WUTO = tempWU;
                //tempWU = wu.getParentWorkingUnit();
                //plant = tempWU.Code.Trim();
                DataRow row1 = dt.NewRow();
                if (dbConnection == null)
                    row1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
                else
                    row1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
                row1[6] = DateTime.Now.ToString("dd/MM/yyyy");
                row1[7] = DateTime.Now.ToString("HH:mm");
                dt.Rows.Add(row1);
                rowStart++;

                DataRow row2 = dt.NewRow();
                row2[1] = title;
                row2[6] = datesList[0].ToString("dd.MM") + "-" + datesList[datesList.Count - 1].ToString("dd.MM.yyyy");
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
                        rowPlant[1] = "Izveštaj za kompaniju";
                }
                dt.Rows.Add(rowPlant);
                rowStart++;

                DataRow rowColumns = dt.NewRow();
                if (isAltLang)
                {
                    rowColumns[2] = "Direct";
                    rowColumns[3] = "%";
                    rowColumns[4] = "Indirect";
                    rowColumns[5] = "%";
                    rowColumns[6] = "Total";
                    rowColumns[7] = "%";
                }
                else
                {
                    rowColumns[2] = "Direktan";
                    rowColumns[3] = "%";
                    rowColumns[4] = "Indirektan";
                    rowColumns[5] = "%";
                    rowColumns[6] = "Ukupno";
                    rowColumns[7] = "%";
                }
                dt.Rows.Add(rowColumns);
                rowStart++;
                DataRow rowEmptyFirst = dt.NewRow();
                dt.Rows.Add(rowEmptyFirst);
                rowStart++;

                double totalDD = 0;
                double totalID = 0;
                PassTypeTO passType;
                foreach (KeyValuePair<int, double> pair in dictPassTypesDirect)
                {
                    if (dictPassTypesIndirect.ContainsKey(pair.Key))
                    {
                        if (dbConnection == null)
                            passType = new PassType().Find(pair.Key);
                        else
                            passType = new PassType(dbConnection).Find(pair.Key);

                        DataRow rowDaily = dt.NewRow();
                        if (isAltLang)
                            rowDaily[1] = passType.DescriptionAltAndID;
                        else
                            rowDaily[1] = passType.DescriptionAndID;
                        rowDaily[2] = pair.Value;
                        rowDaily[4] = dictPassTypesIndirect[pair.Key];
                        totalDD += double.Parse(rowDaily[2].ToString());
                        totalID += double.Parse(rowDaily[4].ToString());
                        rowDaily[6] = double.Parse(rowDaily[2].ToString()) + double.Parse(rowDaily[4].ToString());

                        dt.Rows.Add(rowDaily);
                    }
                    else
                    {
                        if (dbConnection == null)
                            passType = new PassType().Find(pair.Key);
                        else
                            passType = new PassType(dbConnection).Find(pair.Key);
                        DataRow rowDaily = dt.NewRow();
                        if (isAltLang)
                            rowDaily[1] = passType.DescriptionAltAndID;
                        else
                            rowDaily[1] = passType.DescriptionAndID;
                        rowDaily[2] = pair.Value;
                        rowDaily[4] = 0;
                        totalDD += double.Parse(rowDaily[2].ToString());
                        totalID += double.Parse(rowDaily[4].ToString());
                        rowDaily[6] = double.Parse(rowDaily[2].ToString()) + double.Parse(rowDaily[4].ToString());

                        dt.Rows.Add(rowDaily);
                    }
                }
                foreach (KeyValuePair<int, double> pair in dictPassTypesIndirect)
                {
                    if (dictPassTypesDirect.Count == 0)
                    {
                        if (dbConnection == null)
                            passType = new PassType().Find(pair.Key);
                        else
                            passType = new PassType(dbConnection).Find(pair.Key);
                        DataRow rowDaily = dt.NewRow();
                        if (isAltLang)
                            rowDaily[1] = passType.DescriptionAltAndID;
                        else
                            rowDaily[1] = passType.DescriptionAndID;
                        rowDaily[2] = 0;
                        rowDaily[4] = pair.Value;
                        totalDD += double.Parse(rowDaily[2].ToString());
                        totalID += double.Parse(rowDaily[4].ToString());
                        rowDaily[6] = double.Parse(rowDaily[2].ToString()) + double.Parse(rowDaily[4].ToString());
                        dt.Rows.Add(rowDaily);
                    }
                    else if (dictPassTypesDirect.Count > 0)
                    {
                        if (!dictPassTypesDirect.ContainsKey(pair.Key))
                        {

                            if (dbConnection == null)
                                passType = new PassType().Find(pair.Key);
                            else
                                passType = new PassType(dbConnection).Find(pair.Key);
                            DataRow rowDaily = dt.NewRow();
                            if (isAltLang)
                                rowDaily[1] = passType.DescriptionAltAndID;
                            else
                                rowDaily[1] = passType.DescriptionAndID;
                            rowDaily[2] = 0;
                            rowDaily[4] = pair.Value;
                            totalDD += double.Parse(rowDaily[2].ToString());
                            totalID += double.Parse(rowDaily[4].ToString());
                            rowDaily[6] = double.Parse(rowDaily[2].ToString()) + double.Parse(rowDaily[4].ToString());
                            dt.Rows.Add(rowDaily);
                        }
                    }
                }

                //foreach (PassTypeTO passType in listPassTypes)
                //{
                //    if ((isDirect && !dictPassTypesDirect.ContainsKey(passType.PassTypeID) || (!isDirect && !dictPassTypesIndirect.ContainsKey(passType.PassTypeID))))
                //    {
                //        DataRow rowDaily = dt.NewRow();
                //        rowDaily[1] = passType.DescriptionAltAndID;
                //        rowDaily[2] = 0;
                //        rowDaily[4] = 0;
                //        totalDD += double.Parse(rowDaily[2].ToString());
                //        totalID += double.Parse(rowDaily[4].ToString());
                //        rowDaily[6] = double.Parse(rowDaily[2].ToString()) + double.Parse(rowDaily[4].ToString());
                //        dt.Rows.Add(rowDaily);
                //    }
                //}

                double total = totalDD + totalID;
                double totalDPercent = 0;
                double totalIPercent = 0;
                for (int i = 6; i < dt.Rows.Count; i++)
                {
                    DataRow rowPer = dt.Rows[i];
                    Misc.percentage(rowPer, totalDD, totalID);
                    rowPer[7] = double.Parse(rowPer[6].ToString()) * 100 / total;
                    if (rowPer[7].ToString() == "NaN") rowPer[7] = 0;

                    totalDPercent += double.Parse(rowPer[3].ToString());
                    totalIPercent += double.Parse(rowPer[5].ToString());
                    Misc.roundOn2(rowPer);

                }
                DataRow rowEmpty = dt.NewRow();
                dt.Rows.Add(rowEmpty);
                DataRow row = dt.NewRow();
                if (isAltLang)
                    row[1] = "Total";
                else
                    row[1] = "Ukupno";

                row[2] = totalDD;
                row[4] = totalID;
                row[6] = total;
                row[5] = 0;
                row[3] = 0;
                DataRow rowTotal = dt.NewRow();
                if (isAltLang)
                    rowTotal[1] = "Planned";
                else
                    rowTotal[1] = "Planirano";

                if (emplIdsBS.Length > 0)
                {
                    if (datesList.Count == 1)
                        rowTotal[2] = CalcPlannedTimeDaily(dbConnection, emplIdsBS, datesList[0].AddDays(-1).Date, datesList[0].Date);
                    else
                        rowTotal[2] = Misc.CalcPlannedTime(dbConnection, emplIdsBS, datesList[0].Date, datesList[datesList.Count - 1].Date);
                }
                else
                    rowTotal[2] = 0;

                if (emplIds.Length > 0)
                {
                    if (datesList.Count == 1)
                        rowTotal[4] = CalcPlannedTimeDaily(dbConnection, emplIds, datesList[0].AddDays(-1).Date, datesList[0].Date);
                    else
                        rowTotal[4] = Misc.CalcPlannedTime(dbConnection, emplIds, datesList[0].Date, datesList[datesList.Count - 1].Date);
                }
                else
                    rowTotal[4] = 0;

                rowTotal[6] = double.Parse(rowTotal[2].ToString()) + double.Parse(rowTotal[4].ToString());
                rowTotal[5] = 0;
                rowTotal[3] = 0;

                DataRow rowTotalDiff = dt.NewRow();
                rowTotalDiff[1] = "+/-";
                rowTotalDiff[3] = "  ";
                rowTotalDiff[5] = "  ";
                rowTotalDiff[7] = "  ";
                rowTotalDiff[2] = double.Parse(row[2].ToString()) - double.Parse(rowTotal[2].ToString());
                rowTotalDiff[4] = double.Parse(row[4].ToString()) - double.Parse(rowTotal[4].ToString());
                rowTotalDiff[6] = double.Parse(row[6].ToString()) - double.Parse(rowTotal[6].ToString());


                row[7] = double.Parse(row[6].ToString()) * 100 / total;
                if (row[7].ToString() == "NaN") row[7] = 0;

                row[3] = totalDPercent;
                row[5] = totalIPercent;

                double ts = double.Parse(rowTotal[2].ToString());
                double tsi = double.Parse(rowTotal[4].ToString());
                if (ts != 0) rowTotal[3] = 0;
                if (tsi != 0) rowTotal[5] = 0;

                rowTotal[7] = 0;
                Misc.roundOn2(row);
                Misc.roundOn2(rowTotal);
                Misc.roundOn2(rowTotalDiff);

                dt.Rows.Add(row);
                dt.Rows.Add(rowTotal);
                dt.Rows.Add(rowTotalDiff);
                DataRow rowEmptyEnd = dt.NewRow();
                dt.Rows.Add(rowEmptyEnd);
                TimeSpan timeSpan = datesList[datesList.Count - 1] - datesList[0].AddDays(-1);

                double totalDays = numOfEmployees * timeSpan.TotalDays;
                if (isAltLang)
                {
                    DataRow rowbottom = dt.NewRow();
                    rowbottom[1] = "N° Employees: " + numOfEmployees;

                    rowbottom[2] = "Total days: ";
                    rowbottom[3] = totalDays;


                    rowbottom[4] = "Calendar days";
                    rowbottom[5] = timeSpan.TotalDays;
                    dt.Rows.Add(rowbottom);
                }
                else
                {
                    DataRow rowbottom = dt.NewRow();
                    rowbottom[1] = "Br. zaposlenih: " + numOfEmployees;

                    rowbottom[2] = "Ukupno: ";
                    rowbottom[3] = totalDays;


                    rowbottom[4] = "Kalendar. dana";
                    rowbottom[5] = timeSpan.TotalDays;
                    dt.Rows.Add(rowbottom);

                }


                dt.AcceptChanges();

            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + ".populateTable400() " + ex.Message);

            }

        }
        private double CalcPlannedTimeDaily(Object dbConnection, string emplids, DateTime fromDate, DateTime date)
        {
            try
            {
                string[] employees = emplids.Split(',');
                double plannedTime = 0;

                EmployeesTimeSchedule EmplTimeS;
                TimeSchema timeSch;

                if (dbConnection == null)
                {
                    EmplTimeS = new EmployeesTimeSchedule();
                    timeSch = new TimeSchema();
                }
                else
                {

                    EmplTimeS = new EmployeesTimeSchedule(dbConnection);
                    timeSch = new TimeSchema(dbConnection);
                }
                List<WorkTimeIntervalTO> intervalsEmpl = new List<WorkTimeIntervalTO>();
                foreach (string empl in employees)
                {
                    List<EmployeeTimeScheduleTO> timeScheduleList = EmplTimeS.SearchEmployeesSchedules(empl.ToString(), fromDate, date);
                    string schemaID = "";
                    foreach (EmployeeTimeScheduleTO employeeTimeSchedule in timeScheduleList)
                    {
                        schemaID += employeeTimeSchedule.TimeSchemaID.ToString() + ", ";
                    }
                    if (!schemaID.Equals(""))
                    {
                        schemaID = schemaID.Substring(0, schemaID.Length - 2);
                    }
                    List<WorkTimeSchemaTO> timeSchema = timeSch.Search(schemaID);


                    if (timeScheduleList.Count > 0)
                    {
                        intervalsEmpl = Misc.getTimeSchemaInterval(int.Parse(empl), date, timeScheduleList, timeSchema);//geting time intervals list for specified employee and date

                        foreach (WorkTimeIntervalTO tsInterval in intervalsEmpl)
                        {
                            TimeSpan duration = (new DateTime(tsInterval.EndTime.TimeOfDay.Ticks - tsInterval.StartTime.TimeOfDay.Ticks)).TimeOfDay;
                            plannedTime += (double)duration.Hours;
                        }
                    }

                }
                return plannedTime;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
