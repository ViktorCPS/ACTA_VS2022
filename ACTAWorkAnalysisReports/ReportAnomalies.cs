using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using Util;
using Common;
using TransferObjects;

namespace ACTAWorkAnalysisReports
{
   public class ReportAnomalies
   {
       private class AnomalyCategory
       {
           private string name = "";
           private int id = -1;

           public string Name
           {
               get { return name; }
               set { name = value; }
           }

           public int ID
           {
               get { return id; }
               set { id = value; }
           }

           public AnomalyCategory(string name, int id)
           {
               this.Name = name;
               this.ID = id;
           }
       }
        DebugLog debug;
        public ReportAnomalies()
        {

            debug = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");

        }

        public void GenerateReport(List<DateTime> datesList, string filePath, List<WorkingUnitTO> listWU, int company)
        {
            debug.writeLog("+ Started anomalies report! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            try
            {

                string myPath = filePath;
                object misValue = System.Reflection.Missing.Value;
                DataSet ds = new DataSet();
                DataTable dtable = new DataTable();

                dtable.Columns.Add(" ", typeof(string));
                dtable.Columns.Add("Cost center", typeof(string));
                dtable.Columns.Add("CC Desc", typeof(string));
                dtable.Columns.Add("Workgroup", typeof(string));
                dtable.Columns.Add("UTE", typeof(string));
                dtable.Columns.Add("Branch", typeof(string));
                dtable.Columns.Add("Employee", typeof(string));
                dtable.Columns.Add("ID", typeof(string));
                dtable.Columns.Add("Type", typeof(string));
                dtable.Columns.Add("Date", typeof(string));
                dtable.Columns.Add("Anomaly category", typeof(string));

                ds.Tables.Add(dtable);
                List<AnomalyCategory> categoryList = new List<AnomalyCategory>();

                int numOfEmployees = 0;

                AnomalyCategory cat = new AnomalyCategory("Notverified data", (int)Constants.AnomalyCategories.NotVerified);
                categoryList.Add(cat);
                cat = new AnomalyCategory("Notconfirmed data", (int)Constants.AnomalyCategories.NotConfirmed);
                categoryList.Add(cat);
                cat = new AnomalyCategory("Unjustified absences", (int)Constants.AnomalyCategories.Unjustified);
                categoryList.Add(cat);
                cat = new AnomalyCategory("Overtime to justify hours", (int)Constants.AnomalyCategories.OvertimeToJusitify);
                categoryList.Add(cat);
                cat = new AnomalyCategory("Negative counters", (int)Constants.AnomalyCategories.NegativeBuffers);
                categoryList.Add(cat);

                Dictionary<int, string> categories = new Dictionary<int, string>();
                foreach (AnomalyCategory anomaly in categoryList)
                {

                    categories.Add(anomaly.ID, anomaly.Name);
                }

                List<WorkingUnitTO> listCostCenter = new WorkingUnit().FindAllChildren(listWU);

                string wuIDs = "";
                foreach (WorkingUnitTO wu in listCostCenter)
                {
                    wuIDs += wu.WorkingUnitID + ",";
                }
                if (wuIDs.Length > 0)
                    wuIDs = wuIDs.Remove(wuIDs.LastIndexOf(','));

                List<EmployeeTO> listEmpl = new List<EmployeeTO>();

                listEmpl = new Employee().SearchByWULoans(wuIDs, -1, null, datesList[0], datesList[datesList.Count - 1]);

                if (listEmpl.Count > 0)
                {
                    Dictionary<string, string> dictEmplTypes = Misc.emplTypes(null, listEmpl, company);

                    Dictionary<int, WorkingUnitTO> WUnits = Misc.getWUnits();
                    Dictionary<int, EmployeeTO> employees = new Dictionary<int, EmployeeTO>();
                    string emplIds = "";
                    foreach (EmployeeTO empl in listEmpl)
                    {
                        if (!employees.ContainsKey(empl.EmployeeID))
                            employees.Add(empl.EmployeeID, empl);
                        else
                            employees[empl.EmployeeID] = empl;

                        emplIds += empl.EmployeeID.ToString().Trim() + ",";
                    }
                    if (emplIds.Length > 0)
                        emplIds = emplIds.Remove(emplIds.LastIndexOf(','));

                    List<EmployeeAsco4TO> emplAscoList = new EmployeeAsco4().Search(emplIds);
                    Dictionary<int, string> branchList = new Dictionary<int, string>();
                    Dictionary<int, string> uteList = new Dictionary<int, string>();
                    Dictionary<int, string> workgroupList = new Dictionary<int, string>();
                    Dictionary<int, string> costcenterList = new Dictionary<int, string>();
                    Dictionary<int, string> costcenterListDesc = new Dictionary<int, string>();

                    foreach (EmployeeAsco4TO asco in emplAscoList)
                    {
                        EmployeeTO empl = new EmployeeTO();

                        if (employees.ContainsKey(asco.EmployeeID))
                            empl = employees[asco.EmployeeID];

                        WorkingUnitTO tempWU = new WorkingUnitTO();
                        if (WUnits.ContainsKey(empl.WorkingUnitID))
                            tempWU = WUnits[empl.WorkingUnitID];

                        if (!branchList.ContainsKey(asco.EmployeeID))
                            branchList.Add(asco.EmployeeID, asco.NVarcharValue6.Trim());
                        else
                            branchList[asco.EmployeeID] = asco.NVarcharValue6.Trim();

                        if (!uteList.ContainsKey(asco.EmployeeID))
                            uteList.Add(asco.EmployeeID, tempWU.Code.Trim());
                        else
                            uteList[asco.EmployeeID] = tempWU.Code.Trim();

                        if (WUnits.ContainsKey(tempWU.ParentWorkingUID))
                            tempWU = WUnits[tempWU.ParentWorkingUID];

                        if (!workgroupList.ContainsKey(asco.EmployeeID))
                            workgroupList.Add(asco.EmployeeID, tempWU.Code.Trim());
                        else
                            workgroupList[asco.EmployeeID] = tempWU.Code.Trim();

                        if (WUnits.ContainsKey(tempWU.ParentWorkingUID))
                            tempWU = WUnits[tempWU.ParentWorkingUID];

                        string cost = tempWU.Name.Trim();
                        string costDesc = tempWU.Description.Trim();
                        if (WUnits.ContainsKey(tempWU.ParentWorkingUID))
                            tempWU = WUnits[tempWU.ParentWorkingUID];

                        string plant = tempWU.Code.Trim();
                        if (!costcenterList.ContainsKey(asco.EmployeeID))
                            costcenterList.Add(asco.EmployeeID, cost);
                        else
                            costcenterList[asco.EmployeeID] = cost;

                        if (!costcenterListDesc.ContainsKey(asco.EmployeeID))
                            costcenterListDesc.Add(asco.EmployeeID, costDesc);
                        else
                            costcenterListDesc[asco.EmployeeID] = costDesc;
                    }
                    Dictionary<int, Dictionary<DateTime, List<int>>> emplDateAnomalyCategories = new Dictionary<int, Dictionary<DateTime, List<int>>>();
                    //dictEmplTypes = emplBranch(emplIds, company);

                    List<IOPairProcessedTO> IOPairList = new IOPairProcessed().SearchAllPairsForEmpl(emplIds, datesList, "");
                    foreach (IOPairProcessedTO pair in IOPairList)
                    {
                        int catID = -1;

                        // set anomaly category if exists
                        if (pair.PassTypeID == Constants.absence)
                        {
                            catID = (int)Constants.AnomalyCategories.Unjustified;
                            if (categories.ContainsKey(catID))
                            {
                                if (!emplDateAnomalyCategories.ContainsKey(pair.EmployeeID))
                                    emplDateAnomalyCategories.Add(pair.EmployeeID, new Dictionary<DateTime, List<int>>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                    emplDateAnomalyCategories[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<int>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Contains(catID))
                                    emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Add(catID);
                            }
                        }

                        if (pair.PassTypeID == Constants.overtimeUnjustified)
                        {
                            catID = (int)Constants.AnomalyCategories.OvertimeToJusitify;
                            if (categories.ContainsKey(catID))
                            {
                                if (!emplDateAnomalyCategories.ContainsKey(pair.EmployeeID))
                                    emplDateAnomalyCategories.Add(pair.EmployeeID, new Dictionary<DateTime, List<int>>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                    emplDateAnomalyCategories[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<int>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Contains(catID))
                                    emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Add(catID);
                            }
                        }

                        if (pair.VerificationFlag == (int)Constants.Verification.NotVerified)
                        {
                            catID = (int)Constants.AnomalyCategories.NotVerified;
                            if (categories.ContainsKey(catID))
                            {
                                if (!emplDateAnomalyCategories.ContainsKey(pair.EmployeeID))
                                    emplDateAnomalyCategories.Add(pair.EmployeeID, new Dictionary<DateTime, List<int>>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                    emplDateAnomalyCategories[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<int>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Contains(catID))
                                    emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Add(catID);
                            }
                        }

                        if (pair.ConfirmationFlag == (int)Constants.Confirmation.NotConfirmed)
                        {
                            catID = (int)Constants.AnomalyCategories.NotConfirmed;
                            if (categories.ContainsKey(catID))
                            {
                                if (!emplDateAnomalyCategories.ContainsKey(pair.EmployeeID))
                                    emplDateAnomalyCategories.Add(pair.EmployeeID, new Dictionary<DateTime, List<int>>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                    emplDateAnomalyCategories[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<int>());

                                if (!emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Contains(catID))
                                    emplDateAnomalyCategories[pair.EmployeeID][pair.IOPairDate.Date].Add(catID);
                            }
                        }


                    }
                    if (categories.ContainsKey((int)Constants.AnomalyCategories.NegativeBuffers))
                    {
                        // get negative counters
                        List<EmployeeCounterValueTO> negativeCounters = new EmployeeCounterValue().SearchNegative(emplIds);

                        foreach (EmployeeCounterValueTO val in negativeCounters)
                        {
                            if (!emplDateAnomalyCategories.ContainsKey(val.EmplID))
                                emplDateAnomalyCategories.Add(val.EmplID, new Dictionary<DateTime, List<int>>());

                            if (!emplDateAnomalyCategories[val.EmplID].ContainsKey(new DateTime()))
                                emplDateAnomalyCategories[val.EmplID].Add(new DateTime(), new List<int>());

                            if (!emplDateAnomalyCategories[val.EmplID][new DateTime()].Contains((int)Constants.AnomalyCategories.NegativeBuffers))
                                emplDateAnomalyCategories[val.EmplID][new DateTime()].Add((int)Constants.AnomalyCategories.NegativeBuffers);
                        }

                        // get negative annual leave counters
                        Dictionary<int, Dictionary<int, int>> emplCounters = new EmployeeCounterValue().Search(emplIds);

                        foreach (int emplID in emplCounters.Keys)
                        {
                            if (emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter) && emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter)
                                && emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter) && (emplCounters[emplID][(int)Constants.EmplCounterTypes.AnnualLeaveCounter]
                                + emplCounters[emplID][(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter] - emplCounters[emplID][(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter]) < 0)
                            {
                                if (!emplDateAnomalyCategories.ContainsKey(emplID))
                                    emplDateAnomalyCategories.Add(emplID, new Dictionary<DateTime, List<int>>());

                                if (!emplDateAnomalyCategories[emplID].ContainsKey(new DateTime()))
                                    emplDateAnomalyCategories[emplID].Add(new DateTime(), new List<int>());

                                if (!emplDateAnomalyCategories[emplID][new DateTime()].Contains((int)Constants.AnomalyCategories.NegativeBuffers))
                                    emplDateAnomalyCategories[emplID][new DateTime()].Add((int)Constants.AnomalyCategories.NegativeBuffers);
                            }
                        }
                    }
                    if (emplDateAnomalyCategories.Count > 0)
                    {

                        DataRow rowDate = dtable.NewRow();

                        rowDate[9] = DateTime.Now.ToString("dd.MM.yy HH:mm");
                        dtable.Rows.Add(rowDate);

                        DataRow rowTitle = dtable.NewRow();

                        rowTitle[5] = "Outstanding data";
                        dtable.Rows.Add(rowTitle);

                        DataRow rowStringone = dtable.NewRow();

                        rowStringone[1] = "Company: " + Enum.GetName(typeof(Constants.FiatCompanies), company);
                        rowStringone[5] = "From: " + datesList[0].ToString("dd.MM.yyyy");
                        dtable.Rows.Add(rowStringone);

                        DataRow rowTO = dtable.NewRow();

                        rowTO[5] = "To:     " + datesList[datesList.Count - 1].ToString("dd.MM.yyyy");

                        dtable.Rows.Add(rowTO);
                        DataRow rowColumn = dtable.NewRow();
                        rowColumn[0] = " ";
                        rowColumn[1] = "Cost center";
                        rowColumn[2] = "CC Desc";
                        rowColumn[3] = "Workgroup";
                        rowColumn[4] = "UTE";
                        rowColumn[5] = "Branch";
                        rowColumn[6] = "Employee";
                        rowColumn[7] = "ID";
                        rowColumn[8] = "Type";
                        rowColumn[9] = "Date";
                        rowColumn[10] = "Anomaly category";

                        dtable.Rows.Add(rowColumn);
                    }
                    foreach (int emplID in emplDateAnomalyCategories.Keys)
                    {
                        foreach (DateTime anomalyDate in emplDateAnomalyCategories[emplID].Keys)
                        {
                            foreach (int catID in emplDateAnomalyCategories[emplID][anomalyDate])
                            {
                                // create result row
                                // stringone

                                DataRow row = dtable.NewRow();
                                row[0] = "";

                                if (costcenterList.ContainsKey(emplID))
                                    row[1] = costcenterList[emplID].Trim();
                                else
                                    row[1] = " ";
                                if (costcenterListDesc.ContainsKey(emplID))
                                    row[2] = costcenterListDesc[emplID].Trim();
                                else
                                    row[2] = " ";
                                if (workgroupList.ContainsKey(emplID))
                                    row[3] = workgroupList[emplID].Trim();
                                else
                                    row[3] = " ";
                                if (uteList.ContainsKey(emplID))
                                    row[4] = uteList[emplID].Trim();
                                else
                                    row[4] = " ";
                                if (branchList.ContainsKey(emplID))
                                    row[5] = branchList[emplID].Trim();
                                else
                                    row[5] = " ";
                                if (dictEmplTypes.ContainsKey(emplID.ToString()))
                                    row[8] = dictEmplTypes[emplID.ToString()];
                                else
                                    row[8] = " ";

                                // employee name
                                if (employees.ContainsKey(emplID))
                                    row[6] = employees[emplID].FirstAndLastName.Trim();
                                else
                                    row[6] = " ";

                                row[7] = emplID;
                                row[9] = anomalyDate.ToString("dd.MM.yyy");

                                // anomaly category
                                if (categories.ContainsKey(catID))
                                    row[10] = categories[catID].Trim();
                                else
                                    row[10] = " ";

                                dtable.Rows.Add(row);
                                dtable.AcceptChanges();

                            }
                        }
                    }

                    numOfEmployees = listEmpl.Count;

                    string Path = Directory.GetParent(filePath).FullName;
                    if (!Directory.Exists(Path))
                    {
                        Directory.CreateDirectory(Path);
                    }
                    if (File.Exists(filePath))
                        File.Delete(filePath);

                    ExportToExcel.CreateExcelDocument(ds, filePath, false, true);

                }
                debug.writeLog("+ Fisnished anomalies report! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "generateExcelAnomalies() " + ex.Message);


            }
        }

    }
}
