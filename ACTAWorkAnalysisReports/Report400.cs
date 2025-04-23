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
    public class Report400
    {
        const int rowDataNum = 54;

        DebugLog debug;
        
        public Report400()
        {
            debug = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");
        }

        public string GenerateReport(Object dbConnection, string filePath, List<WorkingUnitTO> listWU, List<DateTime> datesList, int company, uint py_calc_id)
        {
            debug.writeLog("+ Started 400! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
        
            try
            {
                DataSet ds = new DataSet(Path.GetFileNameWithoutExtension(filePath));
                int numOfEmployees = 0;
                TimeSpan timeSpan = datesList[datesList.Count - 1] - datesList[0].AddDays(-1);
                WorkingUnit wu;
                Employee Empl;

                WorkingUnitTO companyWU = new WorkingUnitTO();
                if (dbConnection != null)
                {
                    companyWU = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company));
                }
                else
                {
                    companyWU = ((WorkingUnitTO)new WorkingUnit().FindWU(company));
                }
                if (dbConnection == null)
                {

                    wu = new WorkingUnit();
                    Empl = new Employee();

                }
                else
                {
                    wu = new WorkingUnit(dbConnection);
                    Empl = new Employee(dbConnection);
                }

                System.Data.DataTable dtCompanyBC = new System.Data.DataTable(company + "BC");
                dtCompanyBC.Columns.Add("1", typeof(string));
                dtCompanyBC.Columns.Add("   ", typeof(string));
                dtCompanyBC.Columns.Add("Direct", typeof(string));
                dtCompanyBC.Columns.Add("%  ", typeof(string));
                dtCompanyBC.Columns.Add("Indirect", typeof(string));
                dtCompanyBC.Columns.Add("%   ", typeof(string));
                dtCompanyBC.Columns.Add("Total", typeof(string));
                dtCompanyBC.Columns.Add(" %   ", typeof(string));

                System.Data.DataTable dtCompanyWC = new System.Data.DataTable(company + "WC");
                dtCompanyWC.Columns.Add("1", typeof(string));
                dtCompanyWC.Columns.Add("   ", typeof(string));
                dtCompanyWC.Columns.Add("Direct", typeof(string));
                dtCompanyWC.Columns.Add("%  ", typeof(string));
                dtCompanyWC.Columns.Add("Indirect", typeof(string));
                dtCompanyWC.Columns.Add("%   ", typeof(string));
                dtCompanyWC.Columns.Add("Total", typeof(string));
                dtCompanyWC.Columns.Add(" %   ", typeof(string));
                int icompany = 0;
                int dcompany = 0;
                int numEmplCompanyDirect = 0;
                int numEmplCompanyIndirect = 0;
                //foreach plant in company find CCs and than data for each cost center write in datatable od plant
                foreach (WorkingUnitTO plant in listWU)
                {
                    int iPlant = 0;
                    int dPlant = 0;
                    int numEmplPlantDirect = 0;
                    int numEmplPlantIndirect = 0;
                    string plantString = "";
                    List<WorkingUnitTO> listCostCenter = new List<WorkingUnitTO>();
                    if (dbConnection == null)
                        listCostCenter = new WorkingUnit().SearchChildWU(plant.WorkingUnitID.ToString());
                    else
                        listCostCenter = new WorkingUnit(dbConnection).SearchChildWU(plant.WorkingUnitID.ToString());

                    System.Data.DataTable dtPlantBC = new System.Data.DataTable(plant.WorkingUnitID + "BC");
                    dtPlantBC.Columns.Add("1", typeof(string));
                    dtPlantBC.Columns.Add("   ", typeof(string));
                    dtPlantBC.Columns.Add("Direct", typeof(string));
                    dtPlantBC.Columns.Add("%  ", typeof(string));
                    dtPlantBC.Columns.Add("Indirect", typeof(string));
                    dtPlantBC.Columns.Add("%   ", typeof(string));
                    dtPlantBC.Columns.Add("Total", typeof(string));
                    dtPlantBC.Columns.Add(" %   ", typeof(string));

                    System.Data.DataTable dtPlantWC = new System.Data.DataTable(plant.WorkingUnitID + "WC");
                    dtPlantWC.Columns.Add("1", typeof(string));
                    dtPlantWC.Columns.Add("   ", typeof(string));
                    dtPlantWC.Columns.Add("Direct", typeof(string));
                    dtPlantWC.Columns.Add("%  ", typeof(string));
                    dtPlantWC.Columns.Add("Indirect", typeof(string));
                    dtPlantWC.Columns.Add("%   ", typeof(string));
                    dtPlantWC.Columns.Add("Total", typeof(string));
                    dtPlantWC.Columns.Add(" %   ", typeof(string));

                    foreach (WorkingUnitTO costCenter in listCostCenter)
                    {
                        int dCost = 0;
                        int iCost = 0;
                        int numEmplCostDirect = 0;
                        int numEmplCostIndirect = 0;
                        List<WorkingUnitTO> listWorkshop = new List<WorkingUnitTO>();

                        if (dbConnection == null)
                            wu = new WorkingUnit();
                        else
                            wu = new WorkingUnit(dbConnection);

                        listWorkshop = wu.SearchChildWU(costCenter.WorkingUnitID.ToString());


                        System.Data.DataTable dtCostCenterBC = new System.Data.DataTable(costCenter.WorkingUnitID + "BC");
                        dtCostCenterBC.Columns.Add("1", typeof(string));
                        dtCostCenterBC.Columns.Add("   ", typeof(string));
                        dtCostCenterBC.Columns.Add("Direct", typeof(string));
                        dtCostCenterBC.Columns.Add("%  ", typeof(string));
                        dtCostCenterBC.Columns.Add("Indirect", typeof(string));
                        dtCostCenterBC.Columns.Add("%   ", typeof(string));
                        dtCostCenterBC.Columns.Add("Total", typeof(string));
                        dtCostCenterBC.Columns.Add(" %   ", typeof(string));

                        System.Data.DataTable dtCostCenterWC = new System.Data.DataTable(costCenter.WorkingUnitID + "WC");
                        dtCostCenterWC.Columns.Add("1", typeof(string));
                        dtCostCenterWC.Columns.Add("   ", typeof(string));
                        dtCostCenterWC.Columns.Add("Direct", typeof(string));
                        dtCostCenterWC.Columns.Add("%  ", typeof(string));
                        dtCostCenterWC.Columns.Add("Indirect", typeof(string));
                        dtCostCenterWC.Columns.Add("%   ", typeof(string));
                        dtCostCenterWC.Columns.Add("Total", typeof(string));
                        dtCostCenterWC.Columns.Add(" %   ", typeof(string));

                        foreach (WorkingUnitTO workshop in listWorkshop)
                        {
                            int d = 0;
                            int i = 0;
                            int numEmplWorkgroupDirect = 0;
                            int numEmplWorkgroupIndirect = 0;

                            if (dbConnection == null)
                                wu = new WorkingUnit();
                            else
                                wu = new WorkingUnit(dbConnection);
                            List<WorkingUnitTO> listUTE = wu.SearchChildWU(workshop.WorkingUnitID.ToString());

                            System.Data.DataTable dtWorkGroupBC = new System.Data.DataTable(workshop.WorkingUnitID + "BC");
                            dtWorkGroupBC.Columns.Add("1", typeof(string));
                            dtWorkGroupBC.Columns.Add("   ", typeof(string));
                            dtWorkGroupBC.Columns.Add("Direct", typeof(string));
                            dtWorkGroupBC.Columns.Add("%  ", typeof(string));
                            dtWorkGroupBC.Columns.Add("Indirect", typeof(string));
                            dtWorkGroupBC.Columns.Add("%   ", typeof(string));
                            dtWorkGroupBC.Columns.Add("Total", typeof(string));
                            dtWorkGroupBC.Columns.Add(" %   ", typeof(string));

                            System.Data.DataTable dtWorkGroupWC = new System.Data.DataTable(workshop.WorkingUnitID + "WC");
                            dtWorkGroupWC.Columns.Add("1", typeof(string));
                            dtWorkGroupWC.Columns.Add("   ", typeof(string));
                            dtWorkGroupWC.Columns.Add("Direct", typeof(string));
                            dtWorkGroupWC.Columns.Add("%  ", typeof(string));
                            dtWorkGroupWC.Columns.Add("Indirect", typeof(string));
                            dtWorkGroupWC.Columns.Add("%   ", typeof(string));
                            dtWorkGroupWC.Columns.Add("Total", typeof(string));
                            dtWorkGroupWC.Columns.Add(" %   ", typeof(string));


                            foreach (WorkingUnitTO workingUnit in listUTE)
                            {
                                List<EmployeeTO> listEmpl = new List<EmployeeTO>();
                                if (dbConnection == null)
                                    Empl = new Employee();
                                else
                                    Empl = new Employee(dbConnection);

                                listEmpl = Empl.SearchByWULoans(workingUnit.WorkingUnitID.ToString(), -1, null, datesList[0], datesList[datesList.Count - 1]);

                                if (listEmpl.Count > 0)
                                {
                                    Dictionary<string, string> dictEmplTypes = Misc.emplTypes(dbConnection, listEmpl, company);

                                    string emplIdsBC = "";
                                    string emplIdsWC = "";
                                    //employees types, for two datatable
                                    foreach (KeyValuePair<string, string> pair in dictEmplTypes)
                                    {
                                        if (pair.Value.Equals("BC"))
                                            emplIdsBC += pair.Key + ",";
                                        else
                                            emplIdsWC += pair.Key + ",";
                                    }

                                    if (emplIdsBC.Length > 0)
                                        emplIdsBC = emplIdsBC.Remove(emplIdsBC.LastIndexOf(','));

                                    if (emplIdsWC.Length > 0)
                                        emplIdsWC = emplIdsWC.Remove(emplIdsWC.LastIndexOf(','));


                                    //datatable BC
                                    if (emplIdsBC.Length > 0)
                                    {
                                        System.Data.DataTable dtBC = new System.Data.DataTable(workingUnit.WorkingUnitID + " BC");
                                        dtBC.Columns.Add("1", typeof(string));
                                        dtBC.Columns.Add("   ", typeof(string));
                                        dtBC.Columns.Add("Direct", typeof(string));
                                        dtBC.Columns.Add("%  ", typeof(string));
                                        dtBC.Columns.Add("Indirect", typeof(string));
                                        dtBC.Columns.Add("%   ", typeof(string));
                                        dtBC.Columns.Add("Total", typeof(string));
                                        dtBC.Columns.Add(" %   ", typeof(string));

                                        dictEmplTypes = Misc.emplBranch(dbConnection, emplIdsBC, company);
                                        string emplIdsBCDirect = "";
                                        string emplIdsBCIndirect = "";

                                        //split employees - direct and indirect, two columns in report
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


                                        numOfEmployees = emplIdsBC.Split(',').Length;
                                        numEmplWorkgroupDirect += numOfEmployees;
                                        numEmplCostDirect += numOfEmployees;
                                        numEmplPlantDirect += numOfEmployees;
                                        numEmplCompanyDirect += numOfEmployees;
                                        double totalDays = numOfEmployees * timeSpan.TotalDays;
                                        //if (IOPairListDirectBC.Count > 0 || IOPairListIndirectBC.Count > 0)
                                        //{

                                        populateDataTableNew(dbConnection, dtBC, datesList, companyWU, emplIdsBCDirect, emplIdsBCIndirect, "BC", workingUnit, "", py_calc_id);

                                        DataRow rowEmptyFooter = dtBC.NewRow();
                                        dtBC.Rows.Add(rowEmptyFooter);

                                        DataRow footer = dtBC.NewRow();
                                        footer[1] = "N° Employees: " + numOfEmployees;
                                        footer[2] = "Total days: ";
                                        footer[3] = totalDays;
                                        footer[4] = "Calendar days: ";
                                        footer[5] = timeSpan.TotalDays;
                                        dtBC.Rows.Add(footer);

                                        ds.Tables.Add(dtBC);
                                        ds.AcceptChanges();

                                        d++;
                                        if (d == 1)
                                        {
                                            string ute = "";
                                            string workGroup = "";
                                            string costCenterString = "";
                                            //string plantString = "";
                                            if (dbConnection == null)
                                                wu = new WorkingUnit();
                                            else
                                                wu = new WorkingUnit(dbConnection);

                                            DataRow rowHeader1 = dtWorkGroupBC.NewRow();

                                            rowHeader1[1] = companyWU.Description;

                                            rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                            rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                            //rowHeader1[8] = "page: " + page;
                                            dtWorkGroupBC.Rows.Add(rowHeader1);

                                            DataRow rowHeader2 = dtWorkGroupBC.NewRow();
                                            rowHeader2[1] = "   Absenteeism industrial relation";
                                            rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                            rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                                            dtWorkGroupBC.Rows.Add(rowHeader2);

                                            WorkingUnitTO tempWU = wu.FindWU(workingUnit.WorkingUnitID);
                                            ute = tempWU.Code.Trim();

                                            // get workshop (parent of UTE)
                                            wu.WUTO = tempWU;
                                            tempWU = wu.getParentWorkingUnit();
                                            workGroup = tempWU.Code.Trim();

                                            // get cost centar
                                            wu.WUTO = tempWU;
                                            tempWU = wu.getParentWorkingUnit();
                                            costCenterString = tempWU.Code.Trim();

                                            // get plant
                                            wu.WUTO = tempWU;
                                            tempWU = wu.getParentWorkingUnit();
                                            plantString = tempWU.Code.Trim();

                                            DataRow rowHeader3 = dtWorkGroupBC.NewRow();
                                            rowHeader3[1] = "Qualify:   " + "BC";
                                            dtWorkGroupBC.Rows.Add(rowHeader3);

                                            DataRow rowPlant = dtWorkGroupBC.NewRow();
                                            rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
                                            dtWorkGroupBC.Rows.Add(rowPlant);

                                            DataRow rowWorkgroup = dtWorkGroupBC.NewRow();
                                            rowWorkgroup[1] = "Workgroup: " + workGroup + " UTE: " + "xx";
                                            dtWorkGroupBC.Rows.Add(rowWorkgroup);


                                            DataRow rowEmpty = dtWorkGroupBC.NewRow();
                                            dtWorkGroupBC.Rows.Add(rowEmpty);

                                            DataRow rowColumns = dtWorkGroupBC.NewRow();
                                            rowColumns[2] = "Direct";
                                            rowColumns[3] = "%";
                                            rowColumns[4] = "Indirect";
                                            rowColumns[5] = "%";
                                            rowColumns[6] = "Total";
                                            rowColumns[7] = "%";
                                            dtWorkGroupBC.Rows.Add(rowColumns);
                                            for (int ind = 7; ind < dtBC.Rows.Count - 2; ind++)
                                            {
                                                DataRow row = dtWorkGroupBC.NewRow();
                                                row[1] = dtBC.Rows[ind][1];
                                                row[2] = dtBC.Rows[ind][2];
                                                row[4] = dtBC.Rows[ind][4];
                                                row[6] = dtBC.Rows[ind][6];
                                                dtWorkGroupBC.Rows.Add(row);
                                                dtWorkGroupBC.AcceptChanges();
                                            }
                                        }
                                        else
                                        {
                                            for (int ind = 7; ind < dtBC.Rows.Count - 2; ind++)
                                            {
                                                // 03.03.2014. two new holiday rows added
                                                if (ind != rowDataNum)
                                                //if (ind != 27)
                                                {
                                                    dtWorkGroupBC.Rows[ind][1] = dtBC.Rows[ind][1];
                                                    dtWorkGroupBC.Rows[ind][2] = double.Parse(dtWorkGroupBC.Rows[ind][2].ToString()) + double.Parse(dtBC.Rows[ind][2].ToString());
                                                    dtWorkGroupBC.Rows[ind][4] = double.Parse(dtWorkGroupBC.Rows[ind][4].ToString()) + double.Parse(dtBC.Rows[ind][4].ToString());
                                                    dtWorkGroupBC.Rows[ind][6] = double.Parse(dtWorkGroupBC.Rows[ind][6].ToString()) + double.Parse(dtBC.Rows[ind][6].ToString());
                                                }
                                            }
                                        }
                                        //}
                                    }
                                    if (emplIdsWC.Length > 0)
                                    {
                                        System.Data.DataTable dtWC = new System.Data.DataTable(workingUnit.WorkingUnitID + " WC");
                                        dtWC.Columns.Add("1", typeof(string));
                                        dtWC.Columns.Add("   ", typeof(string));
                                        dtWC.Columns.Add("Direct", typeof(string));
                                        dtWC.Columns.Add("%  ", typeof(string));
                                        dtWC.Columns.Add("Indirect", typeof(string));
                                        dtWC.Columns.Add("%   ", typeof(string));
                                        dtWC.Columns.Add("Total", typeof(string));
                                        dtWC.Columns.Add(" %   ", typeof(string));

                                        dictEmplTypes = Misc.emplBranch(dbConnection, emplIdsWC, company);
                                        string emplIdsWCDirect = "";
                                        string emplIdsWCIndirect = "";
                                        foreach (KeyValuePair<string, string> pair in dictEmplTypes)
                                        {
                                            if (pair.Value.Equals("A"))
                                                emplIdsWCDirect += pair.Key + ",";
                                            else
                                                emplIdsWCIndirect += pair.Key + ",";
                                        }

                                        if (emplIdsWCDirect.Length > 0)
                                            emplIdsWCDirect = emplIdsWCDirect.Remove(emplIdsWCDirect.LastIndexOf(','));

                                        if (emplIdsWCIndirect.Length > 0)
                                            emplIdsWCIndirect = emplIdsWCIndirect.Remove(emplIdsWCIndirect.LastIndexOf(','));

                                        numOfEmployees = emplIdsWC.Split(',').Length;
                                        numEmplWorkgroupIndirect += numOfEmployees;
                                        numEmplCostIndirect += numOfEmployees;
                                        numEmplPlantIndirect += numOfEmployees;
                                        numEmplCompanyIndirect += numOfEmployees;
                                        double totalDays = numOfEmployees * timeSpan.TotalDays;

                                        populateDataTableNew(dbConnection, dtWC, datesList, companyWU, emplIdsWCDirect, emplIdsWCIndirect, "WC", workingUnit, "", py_calc_id);

                                        DataRow rowEmptyFooter = dtWC.NewRow();
                                        dtWC.Rows.Add(rowEmptyFooter);
                                        DataRow footer = dtWC.NewRow();
                                        footer[1] = "N° Employees: " + numOfEmployees;
                                        footer[2] = "Total days: ";
                                        footer[3] = totalDays;
                                        footer[4] = "Calendar days: ";
                                        footer[5] = timeSpan.TotalDays;
                                        dtWC.Rows.Add(footer);
                                        ds.Tables.Add(dtWC);
                                        ds.AcceptChanges();

                                        i++;
                                        if (i == 1)
                                        {
                                            string ute = "";
                                            string workGroup = "";
                                            string costCenterString = "";
                                            //string plantString = "";

                                            DataRow rowHeader1 = dtWorkGroupWC.NewRow();

                                            rowHeader1[1] = companyWU.Description;
                                            rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                            rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                            //rowHeader1[8] = "page: " + page;
                                            dtWorkGroupWC.Rows.Add(rowHeader1);
                                            DataRow rowHeader2 = dtWorkGroupWC.NewRow();
                                            rowHeader2[1] = "   Absenteeism industrial relation";
                                            rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                            rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                                            dtWorkGroupWC.Rows.Add(rowHeader2);

                                            if (dbConnection == null)
                                                wu = new WorkingUnit();
                                            else
                                                wu = new WorkingUnit(dbConnection);

                                            WorkingUnitTO tempWU = wu.FindWU(workingUnit.WorkingUnitID);
                                            ute = tempWU.Code.Trim();

                                            // get workshop (parent of UTE)
                                            wu.WUTO = tempWU;
                                            tempWU = wu.getParentWorkingUnit();
                                            workGroup = tempWU.Code.Trim();

                                            // get cost centar
                                            wu.WUTO = tempWU;
                                            tempWU = wu.getParentWorkingUnit();
                                            costCenterString = tempWU.Code.Trim();

                                            // get plant
                                            wu.WUTO = tempWU;
                                            tempWU = wu.getParentWorkingUnit();
                                            plantString = tempWU.Code.Trim();
                                            DataRow rowHeader3 = dtWorkGroupWC.NewRow();

                                            rowHeader3[1] = "Qualify:   " + "WC";
                                            dtWorkGroupWC.Rows.Add(rowHeader3);

                                            DataRow rowPlant = dtWorkGroupWC.NewRow();
                                            rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
                                            dtWorkGroupWC.Rows.Add(rowPlant);
                                            DataRow rowWorkgroup = dtWorkGroupWC.NewRow();
                                            rowWorkgroup[1] = "Workgroup: " + workGroup + " UTE: " + "xx";
                                            dtWorkGroupWC.Rows.Add(rowWorkgroup);
                                            DataRow rowEmpty = dtWorkGroupWC.NewRow();
                                            dtWorkGroupWC.Rows.Add(rowEmpty);

                                            DataRow rowColumns = dtWorkGroupWC.NewRow();
                                            rowColumns[2] = "Direct";
                                            rowColumns[3] = "%";
                                            rowColumns[4] = "Indirect";
                                            rowColumns[5] = "%";
                                            rowColumns[6] = "Total";
                                            rowColumns[7] = "%";
                                            dtWorkGroupWC.Rows.Add(rowColumns);
                                            for (int ind = 7; ind < dtWC.Rows.Count - 2; ind++)
                                            {
                                                DataRow row = dtWorkGroupWC.NewRow();
                                                row[1] = dtWC.Rows[ind][1];
                                                row[2] = dtWC.Rows[ind][2];
                                                row[4] = dtWC.Rows[ind][4];
                                                row[6] = dtWC.Rows[ind][6];
                                                dtWorkGroupWC.Rows.Add(row);
                                                dtWorkGroupWC.AcceptChanges();
                                            }
                                        }
                                        else
                                        {
                                            for (int ind = 7; ind < dtWC.Rows.Count - 2; ind++)
                                            {
                                                // 03.03.2014. two new holiday rows added
                                                if (ind != rowDataNum)
                                                //if (ind != 27)
                                                {
                                                    dtWorkGroupWC.Rows[ind][1] = dtWC.Rows[ind][1];
                                                    dtWorkGroupWC.Rows[ind][2] = double.Parse(dtWorkGroupWC.Rows[ind][2].ToString()) + double.Parse(dtWC.Rows[ind][2].ToString());
                                                    dtWorkGroupWC.Rows[ind][4] = double.Parse(dtWorkGroupWC.Rows[ind][4].ToString()) + double.Parse(dtWC.Rows[ind][4].ToString());
                                                    dtWorkGroupWC.Rows[ind][6] = double.Parse(dtWorkGroupWC.Rows[ind][6].ToString()) + double.Parse(dtWC.Rows[ind][6].ToString());
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            if (dbConnection == null)
                                wu = new WorkingUnit();
                            else
                                wu = new WorkingUnit(dbConnection);
                            List<WorkingUnitTO> oneWorkShop = new List<WorkingUnitTO>();
                            oneWorkShop.Add(workshop);
                            List<WorkingUnitTO> workShopAndUTE = wu.FindAllChildren(oneWorkShop);

                            if (dtWorkGroupBC.Rows.Count > 0)
                            {
                                double totalDD = 0;
                                double totalID = 0;
                                double totalDDSum = 0;
                                double totalIDSum = 0;
                                for (int ind = 7; ind < rowDataNum; ind++)
                                {
                                    totalDD = double.Parse(dtWorkGroupBC.Rows[ind][2].ToString());
                                    totalID = double.Parse(dtWorkGroupBC.Rows[ind][4].ToString());
                                    totalDDSum += totalDD;
                                    totalIDSum += totalID;
                                }
                                double totaltotal = totalDDSum + totalIDSum;
                                for (int ind = 7; ind < rowDataNum; ind++)
                                {
                                    Misc.percentage(dtWorkGroupBC.Rows[ind], totalDDSum, totalIDSum);

                                    dtWorkGroupBC.Rows[ind][7] = double.Parse(dtWorkGroupBC.Rows[ind][6].ToString()) * 100 / totaltotal;
                                    dtWorkGroupBC.AcceptChanges();
                                }
                                double totalPerDD = 0;
                                double totalPerID = 0;
                                double totalPerSum = 0;
                                for (int ind = 7; ind < rowDataNum; ind++)
                                {
                                    totalPerDD += double.Parse(dtWorkGroupBC.Rows[ind][3].ToString());
                                    totalPerID += double.Parse(dtWorkGroupBC.Rows[ind][5].ToString());
                                    totalPerSum += double.Parse(dtWorkGroupBC.Rows[ind][7].ToString());
                                }
                                dtWorkGroupBC.Rows[rowDataNum + 1][3] = totalPerDD;
                                dtWorkGroupBC.Rows[rowDataNum + 1][5] = totalPerID;
                                dtWorkGroupBC.Rows[rowDataNum + 1][7] = totalPerSum;

                                dtWorkGroupBC.Rows[rowDataNum + 2][3] = 0;
                                dtWorkGroupBC.Rows[rowDataNum + 2][5] = 0;
                                dtWorkGroupBC.Rows[rowDataNum + 2][7] = 0;

                                dtWorkGroupBC.Rows[rowDataNum + 3][3] = "  ";
                                dtWorkGroupBC.Rows[rowDataNum + 3][5] = "  ";
                                dtWorkGroupBC.Rows[rowDataNum + 3][7] = "  ";
                                numOfEmployees = numEmplWorkgroupDirect;
                                double totalDays = numOfEmployees * timeSpan.TotalDays;
                                DataRow rowEmptyFooter = dtWorkGroupBC.NewRow();
                                dtWorkGroupBC.Rows.Add(rowEmptyFooter);

                                DataRow footer = dtWorkGroupBC.NewRow();
                                footer[1] = "N° Employees: " + numOfEmployees;
                                footer[2] = "Total days: ";
                                footer[3] = totalDays;
                                footer[4] = "Calendar days: ";
                                footer[5] = timeSpan.TotalDays;
                                dtWorkGroupBC.Rows.Add(footer);

                                ds.Tables.Add(dtWorkGroupBC);
                                ds.AcceptChanges();

                                dCost++;
                                if (dCost == 1)
                                {
                                    string ute = "";
                                    string workGroup = "";
                                    string costCenterString = "";
                                    DataRow rowHeader1 = dtCostCenterBC.NewRow();

                                    rowHeader1[1] = companyWU.Description;

                                    rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                    rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                    dtCostCenterBC.Rows.Add(rowHeader1);

                                    DataRow rowHeader2 = dtCostCenterBC.NewRow();
                                    rowHeader2[1] = "   Absenteeism industrial relation";
                                    rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                    rowHeader2[7] = datesList[0].ToString("MMMM yyyy");

                                    dtCostCenterBC.Rows.Add(rowHeader2);
                                    if (dbConnection == null)
                                        wu = new WorkingUnit();
                                    else
                                        wu = new WorkingUnit(dbConnection);

                                    WorkingUnitTO tempWU = wu.FindWU(listUTE[0].WorkingUnitID);
                                    ute = tempWU.Code.Trim();

                                    // get workshop (parent of UTE)
                                    wu.WUTO = tempWU;
                                    tempWU = wu.getParentWorkingUnit();
                                    workGroup = tempWU.Code.Trim();

                                    // get cost centar
                                    wu.WUTO = tempWU;
                                    tempWU = wu.getParentWorkingUnit();
                                    costCenterString = tempWU.Code.Trim();

                                    // get plant
                                    wu.WUTO = tempWU;
                                    tempWU = wu.getParentWorkingUnit();
                                    plantString = tempWU.Code.Trim();

                                    DataRow rowHeader3 = dtCostCenterBC.NewRow();
                                    rowHeader3[1] = "Qualify:   " + "BC";
                                    dtCostCenterBC.Rows.Add(rowHeader3);

                                    DataRow rowPlant = dtCostCenterBC.NewRow();
                                    rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
                                    dtCostCenterBC.Rows.Add(rowPlant);

                                    DataRow rowWorkgroup = dtCostCenterBC.NewRow();
                                    rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
                                    dtCostCenterBC.Rows.Add(rowWorkgroup);

                                    DataRow rowEmpty = dtCostCenterBC.NewRow();
                                    dtCostCenterBC.Rows.Add(rowEmpty);

                                    DataRow rowColumns = dtCostCenterBC.NewRow();
                                    rowColumns[2] = "Direct";
                                    rowColumns[3] = "%";
                                    rowColumns[4] = "Indirect";
                                    rowColumns[5] = "%";
                                    rowColumns[6] = "Total";
                                    rowColumns[7] = "%";
                                    dtCostCenterBC.Rows.Add(rowColumns);

                                    for (int ind = 7; ind < dtWorkGroupBC.Rows.Count - 2; ind++)
                                    {
                                        DataRow row = dtCostCenterBC.NewRow();

                                        row[1] = dtWorkGroupBC.Rows[ind][1];
                                        row[2] = dtWorkGroupBC.Rows[ind][2];
                                        row[4] = dtWorkGroupBC.Rows[ind][4];
                                        row[6] = dtWorkGroupBC.Rows[ind][6];
                                        dtCostCenterBC.Rows.Add(row);
                                        dtCostCenterBC.AcceptChanges();
                                    }
                                }
                                else
                                {
                                    for (int ind = 7; ind < dtWorkGroupBC.Rows.Count - 2; ind++)
                                    {
                                        // 03.03.2014. two new holiday rows added
                                        if (ind != rowDataNum)
                                        //if (ind != 27)
                                        {
                                            dtCostCenterBC.Rows[ind][1] = dtWorkGroupBC.Rows[ind][1];
                                            dtCostCenterBC.Rows[ind][2] = double.Parse(dtCostCenterBC.Rows[ind][2].ToString()) + double.Parse(dtWorkGroupBC.Rows[ind][2].ToString());
                                            dtCostCenterBC.Rows[ind][4] = double.Parse(dtCostCenterBC.Rows[ind][4].ToString()) + double.Parse(dtWorkGroupBC.Rows[ind][4].ToString());
                                            dtCostCenterBC.Rows[ind][6] = double.Parse(dtCostCenterBC.Rows[ind][6].ToString()) + double.Parse(dtWorkGroupBC.Rows[ind][6].ToString());
                                        }
                                    }
                                }
                            }
                            if (dtWorkGroupWC.Rows.Count > 0)
                            {
                                double totalDD = 0;
                                double totalID = 0;
                                double totalDDSum = 0;
                                double totalIDSum = 0;
                                for (int ind = 7; ind < rowDataNum; ind++)
                                {
                                    totalDD = double.Parse(dtWorkGroupWC.Rows[ind][2].ToString());
                                    totalID = double.Parse(dtWorkGroupWC.Rows[ind][4].ToString());
                                    totalDDSum += totalDD;
                                    totalIDSum += totalID;
                                }
                                double totaltotal = totalDDSum + totalIDSum;
                                for (int ind = 7; ind < rowDataNum; ind++)
                                {
                                    Misc.percentage(dtWorkGroupWC.Rows[ind], totalDDSum, totalIDSum);

                                    dtWorkGroupWC.Rows[ind][7] = double.Parse(dtWorkGroupWC.Rows[ind][6].ToString()) * 100 / totaltotal;
                                    dtWorkGroupWC.AcceptChanges();
                                }
                                double totalPerDD = 0;
                                double totalPerID = 0;
                                double totalPerSum = 0;
                                for (int ind = 7; ind < rowDataNum; ind++)
                                {
                                    totalPerDD += double.Parse(dtWorkGroupWC.Rows[ind][3].ToString());
                                    totalPerID += double.Parse(dtWorkGroupWC.Rows[ind][5].ToString());
                                    totalPerSum += double.Parse(dtWorkGroupWC.Rows[ind][7].ToString());
                                }
                                dtWorkGroupWC.Rows[rowDataNum + 1][3] = totalPerDD;
                                dtWorkGroupWC.Rows[rowDataNum + 1][5] = totalPerID;
                                dtWorkGroupWC.Rows[rowDataNum + 1][7] = totalPerSum;

                                dtWorkGroupWC.Rows[rowDataNum + 2][3] = 0;
                                dtWorkGroupWC.Rows[rowDataNum + 2][5] = 0;
                                dtWorkGroupWC.Rows[rowDataNum + 2][7] = 0;

                                dtWorkGroupWC.Rows[rowDataNum + 3][3] = "  ";
                                dtWorkGroupWC.Rows[rowDataNum + 3][5] = "  ";
                                dtWorkGroupWC.Rows[rowDataNum + 3][7] = "  ";
                                numOfEmployees = numEmplWorkgroupIndirect;
                                double totalDays = numOfEmployees * timeSpan.TotalDays;

                                DataRow rowEmptyFooter = dtWorkGroupWC.NewRow();
                                dtWorkGroupWC.Rows.Add(rowEmptyFooter);

                                DataRow footer = dtWorkGroupWC.NewRow();
                                footer[1] = "N° Employees: " + numOfEmployees;
                                footer[2] = "Total days: ";
                                footer[3] = totalDays;
                                footer[4] = "Calendar days: ";
                                footer[5] = timeSpan.TotalDays;

                                dtWorkGroupWC.Rows.Add(footer);
                                ds.Tables.Add(dtWorkGroupWC);
                                ds.AcceptChanges();

                                iCost++;
                                if (iCost == 1)
                                {
                                    string ute = "";
                                    string workGroup = "";
                                    string costCenterString = "";
                                    //string plantString = "";
                                    DataRow rowHeader1 = dtCostCenterWC.NewRow();

                                    rowHeader1[1] = companyWU.Description;

                                    rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                    rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                    //rowHeader1[8] = "page: " + page;
                                    dtCostCenterWC.Rows.Add(rowHeader1);

                                    DataRow rowHeader2 = dtCostCenterWC.NewRow();
                                    rowHeader2[1] = "   Absenteeism industrial relation";
                                    rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                    rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                                    dtCostCenterWC.Rows.Add(rowHeader2);
                                    if (dbConnection == null)
                                        wu = new WorkingUnit();
                                    else
                                        wu = new WorkingUnit(dbConnection);

                                    WorkingUnitTO tempWU = wu.FindWU(listUTE[0].WorkingUnitID);
                                    ute = tempWU.Code.Trim();

                                    // get workshop (parent of UTE)
                                    wu.WUTO = tempWU;
                                    tempWU = wu.getParentWorkingUnit();
                                    workGroup = tempWU.Code.Trim();

                                    // get cost centar
                                    wu.WUTO = tempWU;
                                    tempWU = wu.getParentWorkingUnit();
                                    costCenterString = tempWU.Code.Trim();

                                    // get plant
                                    wu.WUTO = tempWU;
                                    tempWU = wu.getParentWorkingUnit();
                                    plantString = tempWU.Code.Trim();

                                    DataRow rowHeader3 = dtCostCenterWC.NewRow();
                                    rowHeader3[1] = "Qualify:   " + "WC";
                                    dtCostCenterWC.Rows.Add(rowHeader3);

                                    DataRow rowPlant = dtCostCenterWC.NewRow();
                                    rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
                                    dtCostCenterWC.Rows.Add(rowPlant);

                                    DataRow rowWorkgroup = dtCostCenterWC.NewRow();
                                    rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
                                    dtCostCenterWC.Rows.Add(rowWorkgroup);

                                    DataRow rowEmpty = dtCostCenterWC.NewRow();
                                    dtCostCenterWC.Rows.Add(rowEmpty);

                                    DataRow rowColumns = dtCostCenterWC.NewRow();
                                    rowColumns[2] = "Direct";
                                    rowColumns[3] = "%";
                                    rowColumns[4] = "Indirect";
                                    rowColumns[5] = "%";
                                    rowColumns[6] = "Total";
                                    rowColumns[7] = "%";
                                    dtCostCenterWC.Rows.Add(rowColumns);

                                    for (int ind = 7; ind < dtWorkGroupWC.Rows.Count - 2; ind++)
                                    {
                                        DataRow row = dtCostCenterWC.NewRow();

                                        row[1] = dtWorkGroupWC.Rows[ind][1];
                                        row[2] = dtWorkGroupWC.Rows[ind][2];
                                        row[4] = dtWorkGroupWC.Rows[ind][4];
                                        row[6] = dtWorkGroupWC.Rows[ind][6];
                                        dtCostCenterWC.Rows.Add(row);
                                        dtCostCenterWC.AcceptChanges();
                                    }
                                }
                                else
                                {
                                    for (int ind = 7; ind < dtWorkGroupWC.Rows.Count - 2; ind++)
                                    {
                                        // 03.03.2014. two new holiday rows added
                                        if (ind != rowDataNum)
                                        //if (ind != 27)
                                        {
                                            dtCostCenterWC.Rows[ind][1] = dtWorkGroupWC.Rows[ind][1];
                                            dtCostCenterWC.Rows[ind][2] = double.Parse(dtCostCenterWC.Rows[ind][2].ToString()) + double.Parse(dtWorkGroupWC.Rows[ind][2].ToString());
                                            dtCostCenterWC.Rows[ind][4] = double.Parse(dtCostCenterWC.Rows[ind][4].ToString()) + double.Parse(dtWorkGroupWC.Rows[ind][4].ToString());
                                            dtCostCenterWC.Rows[ind][6] = double.Parse(dtCostCenterWC.Rows[ind][6].ToString()) + double.Parse(dtWorkGroupWC.Rows[ind][6].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        if (dbConnection == null)
                            wu = new WorkingUnit();
                        else
                            wu = new WorkingUnit(dbConnection);
                        List<WorkingUnitTO> oneCostCenter = new List<WorkingUnitTO>();
                        oneCostCenter.Add(costCenter);
                        List<WorkingUnitTO> CostCenterAndWorkShop = wu.FindAllChildren(oneCostCenter);

                        if (dtCostCenterBC.Rows.Count > 0)
                        {
                            double totalDD = 0;
                            double totalID = 0;
                            double totalDDSum = 0;
                            double totalIDSum = 0;
                            for (int i = 7; i < rowDataNum; i++)
                            {
                                totalDD = double.Parse(dtCostCenterBC.Rows[i][2].ToString());
                                totalID = double.Parse(dtCostCenterBC.Rows[i][4].ToString());
                                totalDDSum += totalDD;
                                totalIDSum += totalID;
                            }
                            double totaltotal = totalDDSum + totalIDSum;
                            for (int i = 7; i < rowDataNum; i++)
                            {
                                Misc.percentage(dtCostCenterBC.Rows[i], totalDDSum, totalIDSum);

                                dtCostCenterBC.Rows[i][7] = double.Parse(dtCostCenterBC.Rows[i][6].ToString()) * 100 / totaltotal;
                                dtCostCenterBC.AcceptChanges();
                            }
                            double totalPerDD = 0;
                            double totalPerID = 0;
                            double totalPerSum = 0;
                            for (int i = 7; i < rowDataNum; i++)
                            {
                                totalPerDD += double.Parse(dtCostCenterBC.Rows[i][3].ToString());
                                totalPerID += double.Parse(dtCostCenterBC.Rows[i][5].ToString());
                                totalPerSum += double.Parse(dtCostCenterBC.Rows[i][7].ToString());
                            }
                            dtCostCenterBC.Rows[rowDataNum + 1][3] = totalPerDD;
                            dtCostCenterBC.Rows[rowDataNum + 1][5] = totalPerID;
                            dtCostCenterBC.Rows[rowDataNum + 1][7] = totalPerSum;

                            dtCostCenterBC.Rows[rowDataNum + 2][3] = 0;
                            dtCostCenterBC.Rows[rowDataNum + 2][5] = 0;
                            dtCostCenterBC.Rows[rowDataNum + 2][7] = 0;

                            dtCostCenterBC.Rows[rowDataNum + 3][3] = "  ";
                            dtCostCenterBC.Rows[rowDataNum + 3][5] = "  ";
                            dtCostCenterBC.Rows[rowDataNum + 3][7] = "  ";
                            numOfEmployees = numEmplCostDirect;
                            double totalDays = numOfEmployees * timeSpan.TotalDays;
                            DataRow rowEmptyFooter = dtCostCenterBC.NewRow();
                            dtCostCenterBC.Rows.Add(rowEmptyFooter);

                            DataRow footer = dtCostCenterBC.NewRow();
                            footer[1] = "N° Employees: " + numOfEmployees;
                            footer[2] = "Total days: ";
                            footer[3] = totalDays;
                            footer[4] = "Calendar days: ";
                            footer[5] = timeSpan.TotalDays;
                            dtCostCenterBC.Rows.Add(footer);

                            ds.Tables.Add(dtCostCenterBC);
                            ds.AcceptChanges();

                            dPlant++;
                            if (dPlant == 1)
                            {
                                DataRow rowHeader1 = dtPlantBC.NewRow();

                                rowHeader1[1] = companyWU.Description;

                                rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                //rowHeader1[8] = "page: " + page;
                                dtPlantBC.Rows.Add(rowHeader1);

                                DataRow rowHeader2 = dtPlantBC.NewRow();
                                rowHeader2[1] = "   Absenteeism industrial relation";
                                rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                                dtPlantBC.Rows.Add(rowHeader2);

                                DataRow rowHeader3 = dtPlantBC.NewRow();
                                rowHeader3[1] = "Qualify:   " + "BC";
                                dtPlantBC.Rows.Add(rowHeader3);

                                DataRow rowPlant = dtPlantBC.NewRow();
                                rowPlant[1] = "Plant:    " + plantString + "    Cost center: " + "xxxx";
                                dtPlantBC.Rows.Add(rowPlant);

                                DataRow rowWorkgroup = dtPlantBC.NewRow();
                                rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
                                dtPlantBC.Rows.Add(rowWorkgroup);

                                DataRow rowEmpty = dtPlantBC.NewRow();
                                dtPlantBC.Rows.Add(rowEmpty);

                                DataRow rowColumns = dtPlantBC.NewRow();
                                rowColumns[2] = "Direct";
                                rowColumns[3] = "%";
                                rowColumns[4] = "Indirect";
                                rowColumns[5] = "%";
                                rowColumns[6] = "Total";
                                rowColumns[7] = "%";
                                dtPlantBC.Rows.Add(rowColumns);

                                for (int ind = 7; ind < dtCostCenterBC.Rows.Count - 2; ind++)
                                {
                                    DataRow row = dtPlantBC.NewRow();

                                    row[1] = dtCostCenterBC.Rows[ind][1];
                                    row[2] = dtCostCenterBC.Rows[ind][2];
                                    row[4] = dtCostCenterBC.Rows[ind][4];
                                    row[6] = dtCostCenterBC.Rows[ind][6];
                                    dtPlantBC.Rows.Add(row);
                                    dtPlantBC.AcceptChanges();
                                }
                            }
                            else
                            {
                                for (int ind = 7; ind < dtCostCenterBC.Rows.Count - 2; ind++)
                                {
                                    // 03.03.2014. two new holiday rows added
                                    if (ind != rowDataNum)
                                    //if (ind != 27)
                                    {
                                        dtPlantBC.Rows[ind][1] = dtCostCenterBC.Rows[ind][1];
                                        dtPlantBC.Rows[ind][2] = double.Parse(dtPlantBC.Rows[ind][2].ToString()) + double.Parse(dtCostCenterBC.Rows[ind][2].ToString());
                                        dtPlantBC.Rows[ind][4] = double.Parse(dtPlantBC.Rows[ind][4].ToString()) + double.Parse(dtCostCenterBC.Rows[ind][4].ToString());
                                        dtPlantBC.Rows[ind][6] = double.Parse(dtPlantBC.Rows[ind][6].ToString()) + double.Parse(dtCostCenterBC.Rows[ind][6].ToString());
                                    }
                                }
                            }
                        }
                        if (dtCostCenterWC.Rows.Count > 0)
                        {
                            double totalDD = 0;
                            double totalID = 0;
                            double totalDDSum = 0;
                            double totalIDSum = 0;
                            for (int i = 7; i < rowDataNum; i++)
                            {
                                totalDD = double.Parse(dtCostCenterWC.Rows[i][2].ToString());
                                totalID = double.Parse(dtCostCenterWC.Rows[i][4].ToString());
                                totalDDSum += totalDD;
                                totalIDSum += totalID;
                            }
                            double totaltotal = totalDDSum + totalIDSum;
                            for (int i = 7; i < rowDataNum; i++)
                            {
                                Misc.percentage(dtCostCenterWC.Rows[i], totalDDSum, totalIDSum);

                                dtCostCenterWC.Rows[i][7] = double.Parse(dtCostCenterWC.Rows[i][6].ToString()) * 100 / totaltotal;
                                dtCostCenterWC.AcceptChanges();
                            }
                            double totalPerDD = 0;
                            double totalPerID = 0;
                            double totalPerSum = 0;
                            for (int i = 7; i < rowDataNum; i++)
                            {
                                totalPerDD += double.Parse(dtCostCenterWC.Rows[i][3].ToString());
                                totalPerID += double.Parse(dtCostCenterWC.Rows[i][5].ToString());
                                totalPerSum += double.Parse(dtCostCenterWC.Rows[i][7].ToString());
                            }
                            dtCostCenterWC.Rows[rowDataNum + 1][3] = totalPerDD;
                            dtCostCenterWC.Rows[rowDataNum + 1][5] = totalPerID;
                            dtCostCenterWC.Rows[rowDataNum + 1][7] = totalPerSum;

                            dtCostCenterWC.Rows[rowDataNum + 2][3] = 0;
                            dtCostCenterWC.Rows[rowDataNum + 2][5] = 0;
                            dtCostCenterWC.Rows[rowDataNum + 2][7] = 0;

                            dtCostCenterWC.Rows[rowDataNum + 3][3] = "  ";
                            dtCostCenterWC.Rows[rowDataNum + 3][5] = "  ";
                            dtCostCenterWC.Rows[rowDataNum + 3][7] = "  ";

                            numOfEmployees = numEmplCostIndirect;
                            double totalDays = numOfEmployees * timeSpan.TotalDays;

                            DataRow rowEmptyFooter = dtCostCenterWC.NewRow();
                            dtCostCenterWC.Rows.Add(rowEmptyFooter);

                            DataRow footer = dtCostCenterWC.NewRow();
                            footer[1] = "N° Employees: " + numOfEmployees;
                            footer[2] = "Total days: ";
                            footer[3] = totalDays;
                            footer[4] = "Calendar days: ";
                            footer[5] = timeSpan.TotalDays;
                            dtCostCenterWC.Rows.Add(footer);

                            ds.Tables.Add(dtCostCenterWC);
                            ds.AcceptChanges();

                            iPlant++;
                            if (iPlant == 1)
                            {
                                DataRow rowHeader1 = dtPlantWC.NewRow();

                                rowHeader1[1] = companyWU.Description;

                                rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                //rowHeader1[8] = "page: " + page;
                                dtPlantWC.Rows.Add(rowHeader1);

                                DataRow rowHeader2 = dtPlantWC.NewRow();
                                rowHeader2[1] = "   Absenteeism industrial relation";
                                rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                                dtPlantWC.Rows.Add(rowHeader2);

                                DataRow rowHeader3 = dtPlantWC.NewRow();
                                rowHeader3[1] = "Qualify:   " + "WC";
                                dtPlantWC.Rows.Add(rowHeader3);

                                DataRow rowPlant = dtPlantWC.NewRow();
                                rowPlant[1] = "Plant:    " + plantString + "    Cost center: " + "xxxx";
                                dtPlantWC.Rows.Add(rowPlant);

                                DataRow rowWorkgroup = dtPlantWC.NewRow();
                                rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
                                dtPlantWC.Rows.Add(rowWorkgroup);

                                DataRow rowEmpty = dtPlantWC.NewRow();
                                dtPlantWC.Rows.Add(rowEmpty);

                                DataRow rowColumns = dtPlantWC.NewRow();
                                rowColumns[2] = "Direct";
                                rowColumns[3] = "%";
                                rowColumns[4] = "Indirect";
                                rowColumns[5] = "%";
                                rowColumns[6] = "Total";
                                rowColumns[7] = "%";
                                dtPlantWC.Rows.Add(rowColumns);

                                for (int ind = 7; ind < dtCostCenterWC.Rows.Count - 2; ind++)
                                {
                                    DataRow row = dtPlantWC.NewRow();

                                    row[1] = dtCostCenterWC.Rows[ind][1];
                                    row[2] = dtCostCenterWC.Rows[ind][2];
                                    row[4] = dtCostCenterWC.Rows[ind][4];
                                    row[6] = dtCostCenterWC.Rows[ind][6];
                                    dtPlantWC.Rows.Add(row);
                                    dtPlantWC.AcceptChanges();
                                }
                            }
                            else
                            {
                                for (int ind = 7; ind < dtCostCenterWC.Rows.Count - 2; ind++)
                                {
                                    // 03.03.2014. two new holiday rows added
                                    if (ind != rowDataNum)
                                    //if (ind != 27)
                                    {
                                        dtPlantWC.Rows[ind][1] = dtCostCenterWC.Rows[ind][1];
                                        dtPlantWC.Rows[ind][2] = double.Parse(dtPlantWC.Rows[ind][2].ToString()) + double.Parse(dtCostCenterWC.Rows[ind][2].ToString());
                                        dtPlantWC.Rows[ind][4] = double.Parse(dtPlantWC.Rows[ind][4].ToString()) + double.Parse(dtCostCenterWC.Rows[ind][4].ToString());
                                        dtPlantWC.Rows[ind][6] = double.Parse(dtPlantWC.Rows[ind][6].ToString()) + double.Parse(dtCostCenterWC.Rows[ind][6].ToString());
                                    }
                                }
                            }
                        }
                    }
                    if (dbConnection == null)
                        wu = new WorkingUnit();
                    else
                        wu = new WorkingUnit(dbConnection);
                    List<WorkingUnitTO> onePlant = new List<WorkingUnitTO>();
                    onePlant.Add(plant);
                    List<WorkingUnitTO> PlantAndCostCentre = wu.FindAllChildren(onePlant);

                    if (dtPlantBC.Rows.Count > 0)
                    {
                        double totalDD = 0;
                        double totalID = 0;
                        double totalDDSum = 0;
                        double totalIDSum = 0;
                        for (int i = 7; i < rowDataNum; i++)
                        {
                            totalDD = double.Parse(dtPlantBC.Rows[i][2].ToString());
                            totalID = double.Parse(dtPlantBC.Rows[i][4].ToString());
                            totalDDSum += totalDD;
                            totalIDSum += totalID;
                        }
                        double totaltotal = totalDDSum + totalIDSum;
                        for (int i = 7; i < rowDataNum; i++)
                        {
                            Misc.percentage(dtPlantBC.Rows[i], totalDDSum, totalIDSum);

                            dtPlantBC.Rows[i][7] = double.Parse(dtPlantBC.Rows[i][6].ToString()) * 100 / totaltotal;
                            dtPlantBC.AcceptChanges();
                        }
                        double totalPerDD = 0;
                        double totalPerID = 0;
                        double totalPerSum = 0;
                        for (int i = 7; i < rowDataNum; i++)
                        {
                            totalPerDD += double.Parse(dtPlantBC.Rows[i][3].ToString());
                            totalPerID += double.Parse(dtPlantBC.Rows[i][5].ToString());
                            totalPerSum += double.Parse(dtPlantBC.Rows[i][7].ToString());
                        }
                        dtPlantBC.Rows[rowDataNum + 1][3] = totalPerDD;
                        dtPlantBC.Rows[rowDataNum + 1][5] = totalPerID;
                        dtPlantBC.Rows[rowDataNum + 1][7] = totalPerSum;

                        dtPlantBC.Rows[rowDataNum + 2][3] = 0;
                        dtPlantBC.Rows[rowDataNum + 2][5] = 0;
                        dtPlantBC.Rows[rowDataNum + 2][7] = 0;

                        dtPlantBC.Rows[rowDataNum + 3][3] = "  ";
                        dtPlantBC.Rows[rowDataNum + 3][5] = "  ";
                        dtPlantBC.Rows[rowDataNum + 3][7] = "  ";

                        numOfEmployees = numEmplPlantDirect;
                        double totalDays = numOfEmployees * timeSpan.TotalDays;
                        DataRow rowEmptyFooter = dtPlantBC.NewRow();
                        dtPlantBC.Rows.Add(rowEmptyFooter);

                        DataRow footer = dtPlantBC.NewRow();
                        footer[1] = "N° Employees: " + numOfEmployees;
                        footer[2] = "Total days: ";
                        footer[3] = totalDays;
                        footer[4] = "Calendar days: ";
                        footer[5] = timeSpan.TotalDays;
                        dtPlantBC.Rows.Add(footer);

                        ds.Tables.Add(dtPlantBC);
                        ds.AcceptChanges();

                        dcompany++;
                        if (dcompany == 1)
                        {
                            DataRow rowHeader1 = dtCompanyBC.NewRow();
                            rowHeader1[1] = companyWU.Description;

                            rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                            rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                            //rowHeader1[8] = "page: " + page;
                            dtCompanyBC.Rows.Add(rowHeader1);

                            DataRow rowHeader2 = dtCompanyBC.NewRow();
                            rowHeader2[1] = "   Absenteeism industrial relation";
                            rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                            rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                            dtCompanyBC.Rows.Add(rowHeader2);


                            DataRow rowHeader3 = dtCompanyBC.NewRow();
                            rowHeader3[1] = "Qualify:   " + "BC";
                            dtCompanyBC.Rows.Add(rowHeader3);

                            DataRow rowPlant = dtCompanyBC.NewRow();
                            rowPlant[1] = "Company report";
                            dtCompanyBC.Rows.Add(rowPlant);

                            DataRow rowWorkgroup = dtCompanyBC.NewRow();
                            dtCompanyBC.Rows.Add(rowWorkgroup);

                            DataRow rowEmpty = dtCompanyBC.NewRow();
                            dtCompanyBC.Rows.Add(rowEmpty);

                            DataRow rowColumns = dtCompanyBC.NewRow();
                            rowColumns[2] = "Direct";
                            rowColumns[3] = "%";
                            rowColumns[4] = "Indirect";
                            rowColumns[5] = "%";
                            rowColumns[6] = "Total";
                            rowColumns[7] = "%";
                            dtCompanyBC.Rows.Add(rowColumns);

                            for (int ind = 7; ind < dtPlantBC.Rows.Count - 2; ind++)
                            {
                                DataRow row = dtCompanyBC.NewRow();

                                row[1] = dtPlantBC.Rows[ind][1];
                                row[2] = dtPlantBC.Rows[ind][2];
                                row[4] = dtPlantBC.Rows[ind][4];
                                row[6] = dtPlantBC.Rows[ind][6];
                                dtCompanyBC.Rows.Add(row);
                                dtCompanyBC.AcceptChanges();
                            }
                        }
                        else
                        {
                            for (int ind = 7; ind < dtPlantBC.Rows.Count - 2; ind++)
                            {
                                // 03.03.2014. two new holiday rows added
                                if (ind != rowDataNum)
                                //if (ind != 27)
                                {
                                    dtCompanyBC.Rows[ind][1] = dtPlantBC.Rows[ind][1];
                                    dtCompanyBC.Rows[ind][2] = double.Parse(dtCompanyBC.Rows[ind][2].ToString()) + double.Parse(dtPlantBC.Rows[ind][2].ToString());
                                    dtCompanyBC.Rows[ind][4] = double.Parse(dtCompanyBC.Rows[ind][4].ToString()) + double.Parse(dtPlantBC.Rows[ind][4].ToString());
                                    dtCompanyBC.Rows[ind][6] = double.Parse(dtCompanyBC.Rows[ind][6].ToString()) + double.Parse(dtPlantBC.Rows[ind][6].ToString());
                                }
                            }
                        }
                    }
                    if (dtPlantWC.Rows.Count > 0)
                    {
                        double totalDD = 0;
                        double totalID = 0;
                        double totalDDSum = 0;
                        double totalIDSum = 0;
                        for (int i = 7; i < rowDataNum; i++)
                        {
                            totalDD = double.Parse(dtPlantWC.Rows[i][2].ToString());
                            totalID = double.Parse(dtPlantWC.Rows[i][4].ToString());
                            totalDDSum += totalDD;
                            totalIDSum += totalID;
                        }
                        double totaltotal = totalDDSum + totalIDSum;
                        for (int i = 7; i < rowDataNum; i++)
                        {
                            Misc.percentage(dtPlantWC.Rows[i], totalDDSum, totalIDSum);

                            dtPlantWC.Rows[i][7] = double.Parse(dtPlantWC.Rows[i][6].ToString()) * 100 / totaltotal;
                            dtPlantWC.AcceptChanges();
                        }
                        double totalPerDD = 0;
                        double totalPerID = 0;
                        double totalPerSum = 0;
                        for (int i = 7; i < rowDataNum; i++)
                        {
                            totalPerDD += double.Parse(dtPlantWC.Rows[i][3].ToString());
                            totalPerID += double.Parse(dtPlantWC.Rows[i][5].ToString());
                            totalPerSum += double.Parse(dtPlantWC.Rows[i][7].ToString());
                        }
                        dtPlantWC.Rows[rowDataNum + 1][3] = totalPerDD;
                        dtPlantWC.Rows[rowDataNum + 1][5] = totalPerID;
                        dtPlantWC.Rows[rowDataNum + 1][7] = totalPerSum;

                        dtPlantWC.Rows[rowDataNum + 2][3] = 0;
                        dtPlantWC.Rows[rowDataNum + 2][5] = 0;
                        dtPlantWC.Rows[rowDataNum + 2][7] = 0;

                        dtPlantWC.Rows[rowDataNum + 3][3] = "  ";
                        dtPlantWC.Rows[rowDataNum + 3][5] = "  ";
                        dtPlantWC.Rows[rowDataNum + 3][7] = "  ";
                        numOfEmployees = numEmplPlantIndirect;
                        double totalDays = numOfEmployees * timeSpan.TotalDays;
                        DataRow rowEmptyFooter = dtPlantWC.NewRow();
                        dtPlantWC.Rows.Add(rowEmptyFooter);

                        DataRow footer = dtPlantWC.NewRow();
                        footer[1] = "N° Employees: " + numOfEmployees;
                        footer[2] = "Total days: ";
                        footer[3] = totalDays;
                        footer[4] = "Calendar days: ";
                        footer[5] = timeSpan.TotalDays;

                        dtPlantWC.Rows.Add(footer);
                        ds.Tables.Add(dtPlantWC);
                        ds.AcceptChanges();

                        icompany++;
                        if (icompany == 1)
                        {
                            DataRow rowHeader1 = dtCompanyWC.NewRow();

                            rowHeader1[1] = companyWU.Description;

                            rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                            rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                            //rowHeader1[8] = "page: " + page;
                            dtCompanyWC.Rows.Add(rowHeader1);

                            DataRow rowHeader2 = dtCompanyWC.NewRow();
                            rowHeader2[1] = "   Absenteeism industrial relation";
                            rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                            rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                            dtCompanyWC.Rows.Add(rowHeader2);

                            DataRow rowHeader3 = dtCompanyWC.NewRow();
                            rowHeader3[1] = "Qualify:   " + "WC";
                            dtCompanyWC.Rows.Add(rowHeader3);

                            DataRow rowPlant = dtCompanyWC.NewRow();
                            rowPlant[1] = "Company report";
                            dtCompanyWC.Rows.Add(rowPlant);

                            DataRow rowWorkgroup = dtCompanyWC.NewRow();
                            dtCompanyWC.Rows.Add(rowWorkgroup);

                            DataRow rowEmpty = dtCompanyWC.NewRow();
                            dtCompanyWC.Rows.Add(rowEmpty);

                            DataRow rowColumns = dtCompanyWC.NewRow();
                            rowColumns[2] = "Direct";
                            rowColumns[3] = "%";
                            rowColumns[4] = "Indirect";
                            rowColumns[5] = "%";
                            rowColumns[6] = "Total";
                            rowColumns[7] = "%";
                            dtCompanyWC.Rows.Add(rowColumns);

                            for (int ind = 7; ind < dtPlantWC.Rows.Count - 2; ind++)
                            {
                                DataRow row = dtCompanyWC.NewRow();

                                row[1] = dtPlantWC.Rows[ind][1];
                                row[2] = dtPlantWC.Rows[ind][2];
                                row[4] = dtPlantWC.Rows[ind][4];
                                row[6] = dtPlantWC.Rows[ind][6];
                                dtCompanyWC.Rows.Add(row);
                                dtCompanyWC.AcceptChanges();
                            }
                        }
                        else
                        {
                            for (int ind = 7; ind < dtPlantWC.Rows.Count - 2; ind++)
                            {
                                // 03.03.2014. two new holiday rows added
                                if (ind != rowDataNum)
                                //if (ind != 27)
                                {
                                    dtCompanyWC.Rows[ind][1] = dtCompanyWC.Rows[ind][1];
                                    dtCompanyWC.Rows[ind][2] = double.Parse(dtCompanyWC.Rows[ind][2].ToString()) + double.Parse(dtPlantWC.Rows[ind][2].ToString());
                                    dtCompanyWC.Rows[ind][4] = double.Parse(dtCompanyWC.Rows[ind][4].ToString()) + double.Parse(dtPlantWC.Rows[ind][4].ToString());
                                    dtCompanyWC.Rows[ind][6] = double.Parse(dtCompanyWC.Rows[ind][6].ToString()) + double.Parse(dtPlantWC.Rows[ind][6].ToString());
                                }
                            }
                        }
                    }
                }
                if (dtCompanyBC.Rows.Count > 6)
                {
                    double totalDD = 0;
                    double totalID = 0;
                    double totalDDSum = 0;
                    double totalIDSum = 0;
                    for (int i = 7; i < rowDataNum; i++)
                    {
                        totalDD = double.Parse(dtCompanyBC.Rows[i][2].ToString());
                        totalID = double.Parse(dtCompanyBC.Rows[i][4].ToString());
                        totalDDSum += totalDD;
                        totalIDSum += totalID;
                    }
                    double totaltotal = totalDDSum + totalIDSum;
                    for (int i = 7; i < rowDataNum; i++)
                    {
                        Misc.percentage(dtCompanyBC.Rows[i], totalDDSum, totalIDSum);

                        dtCompanyBC.Rows[i][7] = double.Parse(dtCompanyBC.Rows[i][6].ToString()) * 100 / totaltotal;
                        dtCompanyBC.AcceptChanges();
                    }
                    double totalPerDD = 0;
                    double totalPerID = 0;
                    double totalPerSum = 0;
                    for (int i = 7; i < rowDataNum; i++)
                    {
                        totalPerDD += double.Parse(dtCompanyBC.Rows[i][3].ToString());
                        totalPerID += double.Parse(dtCompanyBC.Rows[i][5].ToString());
                        totalPerSum += double.Parse(dtCompanyBC.Rows[i][7].ToString());
                    }
                    dtCompanyBC.Rows[rowDataNum + 1][3] = totalPerDD;
                    dtCompanyBC.Rows[rowDataNum + 1][5] = totalPerID;
                    dtCompanyBC.Rows[rowDataNum + 1][7] = totalPerSum;

                    dtCompanyBC.Rows[rowDataNum + 2][3] = 0;
                    dtCompanyBC.Rows[rowDataNum + 2][5] = 0;
                    dtCompanyBC.Rows[rowDataNum + 2][7] = 0;

                    dtCompanyBC.Rows[rowDataNum + 3][3] = "  ";
                    dtCompanyBC.Rows[rowDataNum + 3][5] = "  ";
                    dtCompanyBC.Rows[rowDataNum + 3][7] = "  ";
                    dtCompanyBC.AcceptChanges();
                    numOfEmployees = numEmplCompanyDirect;
                    double totalDays = numOfEmployees * timeSpan.TotalDays;
                    DataRow rowEmptyFooter = dtCompanyBC.NewRow();
                    dtCompanyBC.Rows.Add(rowEmptyFooter);

                    DataRow footer = dtCompanyBC.NewRow();
                    footer[1] = "N° Employees: " + numOfEmployees;
                    footer[2] = "Total days: ";
                    footer[3] = totalDays;
                    footer[4] = "Calendar days: ";
                    footer[5] = timeSpan.TotalDays;
                    dtCompanyBC.Rows.Add(footer);

                    ds.Tables.Add(dtCompanyBC);
                    ds.AcceptChanges();
                }
                if (dtCompanyWC.Rows.Count > 6)
                {
                    double totalDD = 0;
                    double totalID = 0;
                    double totalDDSum = 0;
                    double totalIDSum = 0;
                    for (int i = 7; i < rowDataNum; i++)
                    {
                        totalDD = double.Parse(dtCompanyWC.Rows[i][2].ToString());
                        totalID = double.Parse(dtCompanyWC.Rows[i][4].ToString());
                        totalDDSum += totalDD;
                        totalIDSum += totalID;
                    }
                    double totaltotal = totalDDSum + totalIDSum;
                    for (int i = 7; i < rowDataNum; i++)
                    {
                        Misc.percentage(dtCompanyWC.Rows[i], totalIDSum, totalIDSum);
                        dtCompanyWC.Rows[i][7] = double.Parse(dtCompanyWC.Rows[i][6].ToString()) * 100 / totaltotal;
                        dtCompanyWC.AcceptChanges();

                    }
                    double totalPerDD = 0;
                    double totalPerID = 0;
                    double totalPerSum = 0;
                    for (int i = 7; i < rowDataNum; i++)
                    {
                        totalPerDD += double.Parse(dtCompanyWC.Rows[i][3].ToString());
                        totalPerID += double.Parse(dtCompanyWC.Rows[i][5].ToString());
                        totalPerSum += double.Parse(dtCompanyWC.Rows[i][7].ToString());
                    }
                    dtCompanyWC.Rows[rowDataNum + 1][3] = totalPerDD;
                    dtCompanyWC.Rows[rowDataNum + 1][5] = totalPerID;
                    dtCompanyWC.Rows[rowDataNum + 1][7] = totalPerSum;

                    dtCompanyWC.Rows[rowDataNum + 2][3] = 0;
                    dtCompanyWC.Rows[rowDataNum + 2][5] = 0;
                    dtCompanyWC.Rows[rowDataNum + 2][7] = 0;

                    dtCompanyWC.Rows[rowDataNum + 3][3] = "  ";
                    dtCompanyWC.Rows[rowDataNum + 3][5] = "  ";
                    dtCompanyWC.Rows[rowDataNum + 3][7] = "  ";

                    dtCompanyWC.AcceptChanges();
                    numOfEmployees = numEmplCompanyIndirect;
                    double totalDays = numOfEmployees * timeSpan.TotalDays;
                    DataRow rowEmptyFooter = dtCompanyWC.NewRow();
                    dtCompanyWC.Rows.Add(rowEmptyFooter);

                    DataRow footer = dtCompanyWC.NewRow();
                    footer[1] = "N° Employees: " + numOfEmployees;
                    footer[2] = "Total days: ";
                    footer[3] = totalDays;
                    footer[4] = "Calendar days: ";
                    footer[5] = timeSpan.TotalDays;

                    dtCompanyWC.Rows.Add(footer);
                    dtCompanyWC.AcceptChanges();
                    ds.Tables.Add(dtCompanyWC);
                    ds.AcceptChanges();
                }

                string Pathh = Directory.GetParent(filePath).FullName;
                if (!Directory.Exists(Pathh))
                {
                    Directory.CreateDirectory(Pathh);
                }
                if (File.Exists(filePath))
                    File.Delete(filePath);

                ExportToExcel.CreateExcelDocument(ds, filePath, false, false);

                debug.writeLog("+ Finished 400! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                return filePath;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "generateExcel400() " + ex.Message);
                return "";
            }
        }

        //public string GenerateReportOld(Object dbConnection, string filePath, List<WorkingUnitTO> listWU, List<DateTime> datesList, int company)
        //{
        //    debug.writeLog("+ Started 400! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
        //    try
        //    {
        //        lunchTypes = new Dictionary<int, Dictionary<int, decimal>>();


        //        Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDictionary = new Common.Rule().SearchWUEmplTypeDictionary();
        //        Dictionary<int, PassTypeTO> passTypesDictionaryAll = new Dictionary<int, PassTypeTO>();
        //        if (dbConnection == null)
        //        {
        //            passTypesDictionaryAll = new PassType().SearchDictionary();
        //        }
        //        else
        //        {
        //            passTypesDictionaryAll = new PassType(dbConnection).SearchDictionary();
        //        }
        //        DataSet ds = new DataSet(Path.GetFileNameWithoutExtension(filePath));
        //        int numOfEmployees = 0;
        //        TimeSpan timeSpan = datesList[datesList.Count - 1] - datesList[0].AddDays(-1);
        //        WorkingUnit wu;
        //        Employee Empl;
        //        IOPairProcessed ioPairProc;

        //        WorkingUnitTO companyWU = new WorkingUnitTO();
        //        if (dbConnection != null)
        //        {
        //            companyWU = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company));
        //        }
        //        else
        //        {
        //            companyWU = ((WorkingUnitTO)new WorkingUnit().FindWU(company));
        //        }
        //        if (dbConnection == null)
        //        {

        //            wu = new WorkingUnit();
        //            Empl = new Employee();
        //            ioPairProc = new IOPairProcessed();
        //        }
        //        else
        //        {
        //            wu = new WorkingUnit(dbConnection);
        //            Empl = new Employee(dbConnection);
        //            ioPairProc = new IOPairProcessed(dbConnection);
        //        }
        //        System.Data.DataTable dtCompanyBC = new System.Data.DataTable(company + "BC");
        //        dtCompanyBC.Columns.Add("1", typeof(string));
        //        dtCompanyBC.Columns.Add("   ", typeof(string));
        //        dtCompanyBC.Columns.Add("Direct", typeof(string));
        //        dtCompanyBC.Columns.Add("%  ", typeof(string));
        //        dtCompanyBC.Columns.Add("Indirect", typeof(string));
        //        dtCompanyBC.Columns.Add("%   ", typeof(string));
        //        dtCompanyBC.Columns.Add("Total", typeof(string));
        //        dtCompanyBC.Columns.Add(" %   ", typeof(string));

        //        System.Data.DataTable dtCompanyWC = new System.Data.DataTable(company + "WC");
        //        dtCompanyWC.Columns.Add("1", typeof(string));
        //        dtCompanyWC.Columns.Add("   ", typeof(string));
        //        dtCompanyWC.Columns.Add("Direct", typeof(string));
        //        dtCompanyWC.Columns.Add("%  ", typeof(string));
        //        dtCompanyWC.Columns.Add("Indirect", typeof(string));
        //        dtCompanyWC.Columns.Add("%   ", typeof(string));
        //        dtCompanyWC.Columns.Add("Total", typeof(string));
        //        dtCompanyWC.Columns.Add(" %   ", typeof(string));
        //        int icompany = 0;
        //        int dcompany = 0;
        //        int numEmplCompanyDirect = 0;
        //        int numEmplCompanyIndirect = 0;
        //        //foreach plant in company find CCs and than data for each cost center write in datatable od plant
        //        foreach (WorkingUnitTO plant in listWU)
        //        {
        //            int iPlant = 0;
        //            int dPlant = 0;
        //            int numEmplPlantDirect = 0;
        //            int numEmplPlantIndirect = 0;
        //            string plantString = "";
        //            List<WorkingUnitTO> listCostCenter = new List<WorkingUnitTO>();
        //            if (dbConnection == null)
        //                listCostCenter = new WorkingUnit().SearchChildWU(plant.WorkingUnitID.ToString());
        //            else
        //                listCostCenter = new WorkingUnit(dbConnection).SearchChildWU(plant.WorkingUnitID.ToString());

        //            System.Data.DataTable dtPlantBC = new System.Data.DataTable(plant.WorkingUnitID + "BC");
        //            dtPlantBC.Columns.Add("1", typeof(string));
        //            dtPlantBC.Columns.Add("   ", typeof(string));
        //            dtPlantBC.Columns.Add("Direct", typeof(string));
        //            dtPlantBC.Columns.Add("%  ", typeof(string));
        //            dtPlantBC.Columns.Add("Indirect", typeof(string));
        //            dtPlantBC.Columns.Add("%   ", typeof(string));
        //            dtPlantBC.Columns.Add("Total", typeof(string));
        //            dtPlantBC.Columns.Add(" %   ", typeof(string));

        //            System.Data.DataTable dtPlantWC = new System.Data.DataTable(plant.WorkingUnitID + "WC");
        //            dtPlantWC.Columns.Add("1", typeof(string));
        //            dtPlantWC.Columns.Add("   ", typeof(string));
        //            dtPlantWC.Columns.Add("Direct", typeof(string));
        //            dtPlantWC.Columns.Add("%  ", typeof(string));
        //            dtPlantWC.Columns.Add("Indirect", typeof(string));
        //            dtPlantWC.Columns.Add("%   ", typeof(string));
        //            dtPlantWC.Columns.Add("Total", typeof(string));
        //            dtPlantWC.Columns.Add(" %   ", typeof(string));

        //            foreach (WorkingUnitTO costCenter in listCostCenter)
        //            {
        //                int dCost = 0;
        //                int iCost = 0;
        //                int numEmplCostDirect = 0;
        //                int numEmplCostIndirect = 0;
        //                List<WorkingUnitTO> listWorkshop = new List<WorkingUnitTO>();

        //                if (dbConnection == null)
        //                    wu = new WorkingUnit();
        //                else
        //                    wu = new WorkingUnit(dbConnection);

        //                listWorkshop = wu.SearchChildWU(costCenter.WorkingUnitID.ToString());


        //                System.Data.DataTable dtCostCenterBC = new System.Data.DataTable(costCenter.WorkingUnitID + "BC");
        //                dtCostCenterBC.Columns.Add("1", typeof(string));
        //                dtCostCenterBC.Columns.Add("   ", typeof(string));
        //                dtCostCenterBC.Columns.Add("Direct", typeof(string));
        //                dtCostCenterBC.Columns.Add("%  ", typeof(string));
        //                dtCostCenterBC.Columns.Add("Indirect", typeof(string));
        //                dtCostCenterBC.Columns.Add("%   ", typeof(string));
        //                dtCostCenterBC.Columns.Add("Total", typeof(string));
        //                dtCostCenterBC.Columns.Add(" %   ", typeof(string));

        //                System.Data.DataTable dtCostCenterWC = new System.Data.DataTable(costCenter.WorkingUnitID + "WC");
        //                dtCostCenterWC.Columns.Add("1", typeof(string));
        //                dtCostCenterWC.Columns.Add("   ", typeof(string));
        //                dtCostCenterWC.Columns.Add("Direct", typeof(string));
        //                dtCostCenterWC.Columns.Add("%  ", typeof(string));
        //                dtCostCenterWC.Columns.Add("Indirect", typeof(string));
        //                dtCostCenterWC.Columns.Add("%   ", typeof(string));
        //                dtCostCenterWC.Columns.Add("Total", typeof(string));
        //                dtCostCenterWC.Columns.Add(" %   ", typeof(string));

        //                foreach (WorkingUnitTO workshop in listWorkshop)
        //                {
        //                    int d = 0;
        //                    int i = 0;
        //                    int numEmplWorkgroupDirect = 0;
        //                    int numEmplWorkgroupIndirect = 0;

        //                    if (dbConnection == null)
        //                        wu = new WorkingUnit();
        //                    else
        //                        wu = new WorkingUnit(dbConnection);
        //                    List<WorkingUnitTO> listUTE = wu.SearchChildWU(workshop.WorkingUnitID.ToString());

        //                    System.Data.DataTable dtWorkGroupBC = new System.Data.DataTable(workshop.WorkingUnitID + "BC");
        //                    dtWorkGroupBC.Columns.Add("1", typeof(string));
        //                    dtWorkGroupBC.Columns.Add("   ", typeof(string));
        //                    dtWorkGroupBC.Columns.Add("Direct", typeof(string));
        //                    dtWorkGroupBC.Columns.Add("%  ", typeof(string));
        //                    dtWorkGroupBC.Columns.Add("Indirect", typeof(string));
        //                    dtWorkGroupBC.Columns.Add("%   ", typeof(string));
        //                    dtWorkGroupBC.Columns.Add("Total", typeof(string));
        //                    dtWorkGroupBC.Columns.Add(" %   ", typeof(string));

        //                    System.Data.DataTable dtWorkGroupWC = new System.Data.DataTable(workshop.WorkingUnitID + "WC");
        //                    dtWorkGroupWC.Columns.Add("1", typeof(string));
        //                    dtWorkGroupWC.Columns.Add("   ", typeof(string));
        //                    dtWorkGroupWC.Columns.Add("Direct", typeof(string));
        //                    dtWorkGroupWC.Columns.Add("%  ", typeof(string));
        //                    dtWorkGroupWC.Columns.Add("Indirect", typeof(string));
        //                    dtWorkGroupWC.Columns.Add("%   ", typeof(string));
        //                    dtWorkGroupWC.Columns.Add("Total", typeof(string));
        //                    dtWorkGroupWC.Columns.Add(" %   ", typeof(string));


        //                    foreach (WorkingUnitTO workingUnit in listUTE)
        //                    {
        //                        List<EmployeeTO> listEmpl = new List<EmployeeTO>();
        //                        if (dbConnection == null)
        //                            Empl = new Employee();
        //                        else
        //                            Empl = new Employee(dbConnection);

        //                        listEmpl = Empl.SearchByWULoans(workingUnit.WorkingUnitID.ToString(), -1, null, datesList[0], datesList[datesList.Count - 1]);

        //                        if (listEmpl.Count > 0)
        //                        {
        //                            Dictionary<string, string> dictEmplTypes = Misc.emplTypes(dbConnection, listEmpl, company);

        //                            string emplIdsBC = "";
        //                            string emplIdsWC = "";
        //                            //employees types, for two datatable
        //                            foreach (KeyValuePair<string, string> pair in dictEmplTypes)
        //                            {
        //                                if (pair.Value.Equals("BC"))
        //                                    emplIdsBC += pair.Key + ",";
        //                                else
        //                                    emplIdsWC += pair.Key + ",";
        //                            }

        //                            if (emplIdsBC.Length > 0)
        //                                emplIdsBC = emplIdsBC.Remove(emplIdsBC.LastIndexOf(','));

        //                            if (emplIdsWC.Length > 0)
        //                                emplIdsWC = emplIdsWC.Remove(emplIdsWC.LastIndexOf(','));


        //                            //datatable BC
        //                            if (emplIdsBC.Length > 0)
        //                            {
        //                                System.Data.DataTable dtBC = new System.Data.DataTable(workingUnit.WorkingUnitID + " BC");
        //                                dtBC.Columns.Add("1", typeof(string));
        //                                dtBC.Columns.Add("   ", typeof(string));
        //                                dtBC.Columns.Add("Direct", typeof(string));
        //                                dtBC.Columns.Add("%  ", typeof(string));
        //                                dtBC.Columns.Add("Indirect", typeof(string));
        //                                dtBC.Columns.Add("%   ", typeof(string));
        //                                dtBC.Columns.Add("Total", typeof(string));
        //                                dtBC.Columns.Add(" %   ", typeof(string));

        //                                dictEmplTypes = Misc.emplBranch(dbConnection, emplIdsBC, company);
        //                                string emplIdsBCDirect = "";
        //                                string emplIdsBCIndirect = "";

        //                                //split employees - direct and indirect, two columns in report
        //                                foreach (KeyValuePair<string, string> pair in dictEmplTypes)
        //                                {
        //                                    if (pair.Value.Equals("A"))
        //                                        emplIdsBCDirect += pair.Key + ",";
        //                                    else
        //                                        emplIdsBCIndirect += pair.Key + ",";
        //                                }

        //                                if (emplIdsBCDirect.Length > 0)
        //                                    emplIdsBCDirect = emplIdsBCDirect.Remove(emplIdsBCDirect.LastIndexOf(','));

        //                                if (emplIdsBCIndirect.Length > 0)
        //                                    emplIdsBCIndirect = emplIdsBCIndirect.Remove(emplIdsBCIndirect.LastIndexOf(','));

        //                                List<IOPairProcessedTO> IOPairListDirectBC = new List<IOPairProcessedTO>();
        //                                List<IOPairProcessedTO> IOPairListIndirectBC = new List<IOPairProcessedTO>();

        //                                //find iopairs for both direct and indirect
        //                                if (dbConnection == null)
        //                                    ioPairProc = new IOPairProcessed();
        //                                else
        //                                    ioPairProc = new IOPairProcessed(dbConnection);

        //                                if (emplIdsBCDirect.Length > 0)
        //                                    IOPairListDirectBC = ioPairProc.SearchAllPairsForEmpl(emplIdsBCDirect, datesList, "");
        //                                else
        //                                    IOPairListDirectBC = new List<IOPairProcessedTO>();

        //                                if (dbConnection == null)
        //                                    ioPairProc = new IOPairProcessed();
        //                                else
        //                                    ioPairProc = new IOPairProcessed(dbConnection);

        //                                if (emplIdsBCIndirect.Length > 0)
        //                                    IOPairListIndirectBC = ioPairProc.SearchAllPairsForEmpl(emplIdsBCIndirect, datesList, "");
        //                                else
        //                                    IOPairListIndirectBC = new List<IOPairProcessedTO>();

        //                                numOfEmployees = emplIdsBC.Split(',').Length;
        //                                numEmplWorkgroupDirect += numOfEmployees;
        //                                numEmplCostDirect += numOfEmployees;
        //                                numEmplPlantDirect += numOfEmployees;
        //                                numEmplCompanyDirect += numOfEmployees;
        //                                double totalDays = numOfEmployees * timeSpan.TotalDays;
        //                                if (IOPairListDirectBC.Count > 0 || IOPairListIndirectBC.Count > 0)
        //                                {

        //                                    populateDataTable(dbConnection, dtBC, datesList, companyWU, IOPairListDirectBC, IOPairListIndirectBC, emplIdsBCDirect, emplIdsBCIndirect, "BC", workingUnit, "", passTypesDictionaryAll, rulesDictionary);

        //                                    DataRow rowEmptyFooter = dtBC.NewRow();
        //                                    dtBC.Rows.Add(rowEmptyFooter);

        //                                    DataRow footer = dtBC.NewRow();
        //                                    footer[1] = "N° Employees: " + numOfEmployees;
        //                                    footer[2] = "Total days: ";
        //                                    footer[3] = totalDays;
        //                                    footer[4] = "Calendar days: ";
        //                                    footer[5] = timeSpan.TotalDays;
        //                                    dtBC.Rows.Add(footer);

        //                                    ds.Tables.Add(dtBC);
        //                                    ds.AcceptChanges();

        //                                    d++;
        //                                    if (d == 1)
        //                                    {
        //                                        string ute = "";
        //                                        string workGroup = "";
        //                                        string costCenterString = "";
        //                                        //string plantString = "";
        //                                        if (dbConnection == null)
        //                                            wu = new WorkingUnit();
        //                                        else
        //                                            wu = new WorkingUnit(dbConnection);

        //                                        DataRow rowHeader1 = dtWorkGroupBC.NewRow();

        //                                        rowHeader1[1] = companyWU.Description;

        //                                        rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
        //                                        rowHeader1[7] = DateTime.Now.ToString("HH:mm");
        //                                        //rowHeader1[8] = "page: " + page;
        //                                        dtWorkGroupBC.Rows.Add(rowHeader1);

        //                                        DataRow rowHeader2 = dtWorkGroupBC.NewRow();
        //                                        rowHeader2[1] = "   Absenteeism industrial relation";
        //                                        rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
        //                                        rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
        //                                        dtWorkGroupBC.Rows.Add(rowHeader2);

        //                                        WorkingUnitTO tempWU = wu.FindWU(workingUnit.WorkingUnitID);
        //                                        ute = tempWU.Code.Trim();

        //                                        // get workshop (parent of UTE)
        //                                        wu.WUTO = tempWU;
        //                                        tempWU = wu.getParentWorkingUnit();
        //                                        workGroup = tempWU.Code.Trim();

        //                                        // get cost centar
        //                                        wu.WUTO = tempWU;
        //                                        tempWU = wu.getParentWorkingUnit();
        //                                        costCenterString = tempWU.Code.Trim();

        //                                        // get plant
        //                                        wu.WUTO = tempWU;
        //                                        tempWU = wu.getParentWorkingUnit();
        //                                        plantString = tempWU.Code.Trim();

        //                                        DataRow rowHeader3 = dtWorkGroupBC.NewRow();
        //                                        rowHeader3[1] = "Qualify:   " + "BC";
        //                                        dtWorkGroupBC.Rows.Add(rowHeader3);

        //                                        DataRow rowPlant = dtWorkGroupBC.NewRow();
        //                                        rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
        //                                        dtWorkGroupBC.Rows.Add(rowPlant);

        //                                        DataRow rowWorkgroup = dtWorkGroupBC.NewRow();
        //                                        rowWorkgroup[1] = "Workgroup: " + workGroup + " UTE: " + "xx";
        //                                        dtWorkGroupBC.Rows.Add(rowWorkgroup);


        //                                        DataRow rowEmpty = dtWorkGroupBC.NewRow();
        //                                        dtWorkGroupBC.Rows.Add(rowEmpty);

        //                                        DataRow rowColumns = dtWorkGroupBC.NewRow();
        //                                        rowColumns[2] = "Direct";
        //                                        rowColumns[3] = "%";
        //                                        rowColumns[4] = "Indirect";
        //                                        rowColumns[5] = "%";
        //                                        rowColumns[6] = "Total";
        //                                        rowColumns[7] = "%";
        //                                        dtWorkGroupBC.Rows.Add(rowColumns);
        //                                        for (int ind = 7; ind < dtBC.Rows.Count - 2; ind++)
        //                                        {
        //                                            DataRow row = dtWorkGroupBC.NewRow();
        //                                            row[1] = dtBC.Rows[ind][1];
        //                                            row[2] = dtBC.Rows[ind][2];
        //                                            row[4] = dtBC.Rows[ind][4];
        //                                            row[6] = dtBC.Rows[ind][6];
        //                                            dtWorkGroupBC.Rows.Add(row);
        //                                            dtWorkGroupBC.AcceptChanges();
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        for (int ind = 7; ind < dtBC.Rows.Count - 2; ind++)
        //                                        {
        //                                            if (ind != 25)
        //                                            {

        //                                                dtWorkGroupBC.Rows[ind][1] = dtBC.Rows[ind][1];
        //                                                dtWorkGroupBC.Rows[ind][2] = double.Parse(dtWorkGroupBC.Rows[ind][2].ToString()) + double.Parse(dtBC.Rows[ind][2].ToString());
        //                                                dtWorkGroupBC.Rows[ind][4] = double.Parse(dtWorkGroupBC.Rows[ind][4].ToString()) + double.Parse(dtBC.Rows[ind][4].ToString());
        //                                                dtWorkGroupBC.Rows[ind][6] = double.Parse(dtWorkGroupBC.Rows[ind][6].ToString()) + double.Parse(dtBC.Rows[ind][6].ToString());

        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            if (emplIdsWC.Length > 0)
        //                            {
        //                                System.Data.DataTable dtWC = new System.Data.DataTable(workingUnit.WorkingUnitID + " WC");
        //                                dtWC.Columns.Add("1", typeof(string));
        //                                dtWC.Columns.Add("   ", typeof(string));
        //                                dtWC.Columns.Add("Direct", typeof(string));
        //                                dtWC.Columns.Add("%  ", typeof(string));
        //                                dtWC.Columns.Add("Indirect", typeof(string));
        //                                dtWC.Columns.Add("%   ", typeof(string));
        //                                dtWC.Columns.Add("Total", typeof(string));
        //                                dtWC.Columns.Add(" %   ", typeof(string));

        //                                dictEmplTypes = Misc.emplBranch(dbConnection, emplIdsWC, company);
        //                                string emplIdsWCDirect = "";
        //                                string emplIdsWCIndirect = "";
        //                                foreach (KeyValuePair<string, string> pair in dictEmplTypes)
        //                                {
        //                                    if (pair.Value.Equals("A"))
        //                                        emplIdsWCDirect += pair.Key + ",";
        //                                    else
        //                                        emplIdsWCIndirect += pair.Key + ",";
        //                                }

        //                                if (emplIdsWCDirect.Length > 0)
        //                                    emplIdsWCDirect = emplIdsWCDirect.Remove(emplIdsWCDirect.LastIndexOf(','));

        //                                if (emplIdsWCIndirect.Length > 0)
        //                                    emplIdsWCIndirect = emplIdsWCIndirect.Remove(emplIdsWCIndirect.LastIndexOf(','));

        //                                List<IOPairProcessedTO> IOPairListDirect = new List<IOPairProcessedTO>();
        //                                List<IOPairProcessedTO> IOPairListIndirect = new List<IOPairProcessedTO>();

        //                                if (dbConnection == null)
        //                                    ioPairProc = new IOPairProcessed();
        //                                else
        //                                    ioPairProc = new IOPairProcessed(dbConnection);
        //                                if (emplIdsWCDirect.Length > 0)
        //                                    IOPairListDirect = ioPairProc.SearchAllPairsForEmpl(emplIdsWCDirect, datesList, "");
        //                                else
        //                                    IOPairListDirect = new List<IOPairProcessedTO>();

        //                                if (dbConnection == null)
        //                                    ioPairProc = new IOPairProcessed();
        //                                else
        //                                    ioPairProc = new IOPairProcessed(dbConnection);

        //                                if (emplIdsWCIndirect.Length > 0)
        //                                    IOPairListIndirect = ioPairProc.SearchAllPairsForEmpl(emplIdsWCIndirect, datesList, "");
        //                                else
        //                                    IOPairListIndirect = new List<IOPairProcessedTO>();

        //                                numOfEmployees = emplIdsWC.Split(',').Length;
        //                                numEmplWorkgroupIndirect += numOfEmployees;
        //                                numEmplCostIndirect += numOfEmployees;
        //                                numEmplPlantIndirect += numOfEmployees;
        //                                numEmplCompanyIndirect += numOfEmployees;
        //                                double totalDays = numOfEmployees * timeSpan.TotalDays;

        //                                if (IOPairListDirect.Count > 0 || IOPairListIndirect.Count > 0)
        //                                {

        //                                    populateDataTable(dbConnection, dtWC, datesList, companyWU, IOPairListDirect, IOPairListIndirect, emplIdsWCDirect, emplIdsWCIndirect, "WC", workingUnit, "", passTypesDictionaryAll, rulesDictionary);

        //                                    DataRow rowEmptyFooter = dtWC.NewRow();
        //                                    dtWC.Rows.Add(rowEmptyFooter);
        //                                    DataRow footer = dtWC.NewRow();
        //                                    footer[1] = "N° Employees: " + numOfEmployees;
        //                                    footer[2] = "Total days: ";
        //                                    footer[3] = totalDays;
        //                                    footer[4] = "Calendar days: ";
        //                                    footer[5] = timeSpan.TotalDays;
        //                                    dtWC.Rows.Add(footer);
        //                                    ds.Tables.Add(dtWC);
        //                                    ds.AcceptChanges();


        //                                    i++;
        //                                    if (i == 1)
        //                                    {
        //                                        string ute = "";
        //                                        string workGroup = "";
        //                                        string costCenterString = "";
        //                                        //string plantString = "";

        //                                        DataRow rowHeader1 = dtWorkGroupWC.NewRow();

        //                                        rowHeader1[1] = companyWU.Description;
        //                                        rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
        //                                        rowHeader1[7] = DateTime.Now.ToString("HH:mm");
        //                                        //rowHeader1[8] = "page: " + page;
        //                                        dtWorkGroupWC.Rows.Add(rowHeader1);
        //                                        DataRow rowHeader2 = dtWorkGroupWC.NewRow();
        //                                        rowHeader2[1] = "   Absenteeism industrial relation";
        //                                        rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
        //                                        rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
        //                                        dtWorkGroupWC.Rows.Add(rowHeader2);

        //                                        if (dbConnection == null)
        //                                            wu = new WorkingUnit();
        //                                        else
        //                                            wu = new WorkingUnit(dbConnection);

        //                                        WorkingUnitTO tempWU = wu.FindWU(workingUnit.WorkingUnitID);
        //                                        ute = tempWU.Code.Trim();

        //                                        // get workshop (parent of UTE)
        //                                        wu.WUTO = tempWU;
        //                                        tempWU = wu.getParentWorkingUnit();
        //                                        workGroup = tempWU.Code.Trim();

        //                                        // get cost centar
        //                                        wu.WUTO = tempWU;
        //                                        tempWU = wu.getParentWorkingUnit();
        //                                        costCenterString = tempWU.Code.Trim();

        //                                        // get plant
        //                                        wu.WUTO = tempWU;
        //                                        tempWU = wu.getParentWorkingUnit();
        //                                        plantString = tempWU.Code.Trim();
        //                                        DataRow rowHeader3 = dtWorkGroupWC.NewRow();

        //                                        rowHeader3[1] = "Qualify:   " + "WC";
        //                                        dtWorkGroupWC.Rows.Add(rowHeader3);

        //                                        DataRow rowPlant = dtWorkGroupWC.NewRow();
        //                                        rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
        //                                        dtWorkGroupWC.Rows.Add(rowPlant);
        //                                        DataRow rowWorkgroup = dtWorkGroupWC.NewRow();
        //                                        rowWorkgroup[1] = "Workgroup: " + workGroup + " UTE: " + "xx";
        //                                        dtWorkGroupWC.Rows.Add(rowWorkgroup);
        //                                        DataRow rowEmpty = dtWorkGroupWC.NewRow();
        //                                        dtWorkGroupWC.Rows.Add(rowEmpty);

        //                                        DataRow rowColumns = dtWorkGroupWC.NewRow();
        //                                        rowColumns[2] = "Direct";
        //                                        rowColumns[3] = "%";
        //                                        rowColumns[4] = "Indirect";
        //                                        rowColumns[5] = "%";
        //                                        rowColumns[6] = "Total";
        //                                        rowColumns[7] = "%";
        //                                        dtWorkGroupWC.Rows.Add(rowColumns);
        //                                        for (int ind = 7; ind < dtWC.Rows.Count - 2; ind++)
        //                                        {
        //                                            DataRow row = dtWorkGroupWC.NewRow();
        //                                            row[1] = dtWC.Rows[ind][1];
        //                                            row[2] = dtWC.Rows[ind][2];
        //                                            row[4] = dtWC.Rows[ind][4];
        //                                            row[6] = dtWC.Rows[ind][6];
        //                                            dtWorkGroupWC.Rows.Add(row);
        //                                            dtWorkGroupWC.AcceptChanges();
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        for (int ind = 7; ind < dtWC.Rows.Count - 2; ind++)
        //                                        {
        //                                            if (ind != 25)
        //                                            {
        //                                                dtWorkGroupWC.Rows[ind][1] = dtWC.Rows[ind][1];
        //                                                dtWorkGroupWC.Rows[ind][2] = double.Parse(dtWorkGroupWC.Rows[ind][2].ToString()) + double.Parse(dtWC.Rows[ind][2].ToString());
        //                                                dtWorkGroupWC.Rows[ind][4] = double.Parse(dtWorkGroupWC.Rows[ind][4].ToString()) + double.Parse(dtWC.Rows[ind][4].ToString());
        //                                                dtWorkGroupWC.Rows[ind][6] = double.Parse(dtWorkGroupWC.Rows[ind][6].ToString()) + double.Parse(dtWC.Rows[ind][6].ToString());
        //                                            }
        //                                        }
        //                                    }


        //                                }
        //                            }
        //                        }

        //                    }
        //                    if (dbConnection == null)
        //                        wu = new WorkingUnit();
        //                    else
        //                        wu = new WorkingUnit(dbConnection);
        //                    List<WorkingUnitTO> oneWorkShop = new List<WorkingUnitTO>();
        //                    oneWorkShop.Add(workshop);
        //                    List<WorkingUnitTO> workShopAndUTE = wu.FindAllChildren(oneWorkShop);

        //                    if (dtWorkGroupBC.Rows.Count > 0)
        //                    {
        //                        double totalDD = 0;
        //                        double totalID = 0;
        //                        double totalDDSum = 0;
        //                        double totalIDSum = 0;
        //                        for (int ind = 7; ind < 25; ind++)
        //                        {
        //                            totalDD = double.Parse(dtWorkGroupBC.Rows[ind][2].ToString());
        //                            totalID = double.Parse(dtWorkGroupBC.Rows[ind][4].ToString());
        //                            totalDDSum += totalDD;
        //                            totalIDSum += totalID;
        //                        }
        //                        double totaltotal = totalDDSum + totalIDSum;
        //                        for (int ind = 7; ind < 25; ind++)
        //                        {
        //                            Misc.percentage(dtWorkGroupBC.Rows[ind], totalDDSum, totalIDSum);

        //                            dtWorkGroupBC.Rows[ind][7] = double.Parse(dtWorkGroupBC.Rows[ind][6].ToString()) * 100 / totaltotal;
        //                            dtWorkGroupBC.AcceptChanges();
        //                        }
        //                        double totalPerDD = 0;
        //                        double totalPerID = 0;
        //                        double totalPerSum = 0;
        //                        for (int ind = 7; ind < 25; ind++)
        //                        {
        //                            totalPerDD += double.Parse(dtWorkGroupBC.Rows[ind][3].ToString());
        //                            totalPerID += double.Parse(dtWorkGroupBC.Rows[ind][5].ToString());
        //                            totalPerSum += double.Parse(dtWorkGroupBC.Rows[ind][7].ToString());
        //                        }
        //                        dtWorkGroupBC.Rows[26][3] = totalPerDD;
        //                        dtWorkGroupBC.Rows[26][5] = totalPerID;
        //                        dtWorkGroupBC.Rows[26][7] = totalPerSum;

        //                        dtWorkGroupBC.Rows[27][3] = 0;
        //                        dtWorkGroupBC.Rows[27][5] = 0;
        //                        dtWorkGroupBC.Rows[27][7] = 0;

        //                        dtWorkGroupBC.Rows[28][3] = "  ";
        //                        dtWorkGroupBC.Rows[28][5] = "  ";
        //                        dtWorkGroupBC.Rows[28][7] = "  ";
        //                        numOfEmployees = numEmplWorkgroupDirect;
        //                        double totalDays = numOfEmployees * timeSpan.TotalDays;
        //                        DataRow rowEmptyFooter = dtWorkGroupBC.NewRow();
        //                        dtWorkGroupBC.Rows.Add(rowEmptyFooter);

        //                        DataRow footer = dtWorkGroupBC.NewRow();
        //                        footer[1] = "N° Employees: " + numOfEmployees;
        //                        footer[2] = "Total days: ";
        //                        footer[3] = totalDays;
        //                        footer[4] = "Calendar days: ";
        //                        footer[5] = timeSpan.TotalDays;
        //                        dtWorkGroupBC.Rows.Add(footer);

        //                        ds.Tables.Add(dtWorkGroupBC);
        //                        ds.AcceptChanges();

        //                        dCost++;
        //                        if (dCost == 1)
        //                        {
        //                            string ute = "";
        //                            string workGroup = "";
        //                            string costCenterString = "";
        //                            DataRow rowHeader1 = dtCostCenterBC.NewRow();

        //                            rowHeader1[1] = companyWU.Description;

        //                            rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
        //                            rowHeader1[7] = DateTime.Now.ToString("HH:mm");
        //                            dtCostCenterBC.Rows.Add(rowHeader1);

        //                            DataRow rowHeader2 = dtCostCenterBC.NewRow();
        //                            rowHeader2[1] = "   Absenteeism industrial relation";
        //                            rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
        //                            rowHeader2[7] = datesList[0].ToString("MMMM yyyy");

        //                            dtCostCenterBC.Rows.Add(rowHeader2);
        //                            if (dbConnection == null)
        //                                wu = new WorkingUnit();
        //                            else
        //                                wu = new WorkingUnit(dbConnection);

        //                            WorkingUnitTO tempWU = wu.FindWU(listUTE[0].WorkingUnitID);
        //                            ute = tempWU.Code.Trim();

        //                            // get workshop (parent of UTE)
        //                            wu.WUTO = tempWU;
        //                            tempWU = wu.getParentWorkingUnit();
        //                            workGroup = tempWU.Code.Trim();

        //                            // get cost centar
        //                            wu.WUTO = tempWU;
        //                            tempWU = wu.getParentWorkingUnit();
        //                            costCenterString = tempWU.Code.Trim();

        //                            // get plant
        //                            wu.WUTO = tempWU;
        //                            tempWU = wu.getParentWorkingUnit();
        //                            plantString = tempWU.Code.Trim();

        //                            DataRow rowHeader3 = dtCostCenterBC.NewRow();
        //                            rowHeader3[1] = "Qualify:   " + "BC";
        //                            dtCostCenterBC.Rows.Add(rowHeader3);

        //                            DataRow rowPlant = dtCostCenterBC.NewRow();
        //                            rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
        //                            dtCostCenterBC.Rows.Add(rowPlant);

        //                            DataRow rowWorkgroup = dtCostCenterBC.NewRow();
        //                            rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
        //                            dtCostCenterBC.Rows.Add(rowWorkgroup);

        //                            DataRow rowEmpty = dtCostCenterBC.NewRow();
        //                            dtCostCenterBC.Rows.Add(rowEmpty);

        //                            DataRow rowColumns = dtCostCenterBC.NewRow();
        //                            rowColumns[2] = "Direct";
        //                            rowColumns[3] = "%";
        //                            rowColumns[4] = "Indirect";
        //                            rowColumns[5] = "%";
        //                            rowColumns[6] = "Total";
        //                            rowColumns[7] = "%";
        //                            dtCostCenterBC.Rows.Add(rowColumns);

        //                            for (int ind = 7; ind < dtWorkGroupBC.Rows.Count - 2; ind++)
        //                            {
        //                                DataRow row = dtCostCenterBC.NewRow();

        //                                row[1] = dtWorkGroupBC.Rows[ind][1];
        //                                row[2] = dtWorkGroupBC.Rows[ind][2];
        //                                row[4] = dtWorkGroupBC.Rows[ind][4];
        //                                row[6] = dtWorkGroupBC.Rows[ind][6];
        //                                dtCostCenterBC.Rows.Add(row);
        //                                dtCostCenterBC.AcceptChanges();

        //                            }
        //                        }
        //                        else
        //                        {
        //                            for (int ind = 7; ind < dtWorkGroupBC.Rows.Count - 2; ind++)
        //                            {
        //                                if (ind != 25)
        //                                {

        //                                    dtCostCenterBC.Rows[ind][1] = dtWorkGroupBC.Rows[ind][1];
        //                                    dtCostCenterBC.Rows[ind][2] = double.Parse(dtCostCenterBC.Rows[ind][2].ToString()) + double.Parse(dtWorkGroupBC.Rows[ind][2].ToString());
        //                                    dtCostCenterBC.Rows[ind][4] = double.Parse(dtCostCenterBC.Rows[ind][4].ToString()) + double.Parse(dtWorkGroupBC.Rows[ind][4].ToString());
        //                                    dtCostCenterBC.Rows[ind][6] = double.Parse(dtCostCenterBC.Rows[ind][6].ToString()) + double.Parse(dtWorkGroupBC.Rows[ind][6].ToString());
        //                                }
        //                            }
        //                        }
        //                    }
        //                    if (dtWorkGroupWC.Rows.Count > 0)
        //                    {
        //                        double totalDD = 0;
        //                        double totalID = 0;
        //                        double totalDDSum = 0;
        //                        double totalIDSum = 0;
        //                        for (int ind = 7; ind < 25; ind++)
        //                        {
        //                            totalDD = double.Parse(dtWorkGroupWC.Rows[ind][2].ToString());
        //                            totalID = double.Parse(dtWorkGroupWC.Rows[ind][4].ToString());
        //                            totalDDSum += totalDD;
        //                            totalIDSum += totalID;
        //                        }
        //                        double totaltotal = totalDDSum + totalIDSum;
        //                        for (int ind = 7; ind < 25; ind++)
        //                        {
        //                            Misc.percentage(dtWorkGroupWC.Rows[ind], totalDDSum, totalIDSum);

        //                            dtWorkGroupWC.Rows[ind][7] = double.Parse(dtWorkGroupWC.Rows[ind][6].ToString()) * 100 / totaltotal;
        //                            dtWorkGroupWC.AcceptChanges();
        //                        }
        //                        double totalPerDD = 0;
        //                        double totalPerID = 0;
        //                        double totalPerSum = 0;
        //                        for (int ind = 7; ind < 25; ind++)
        //                        {
        //                            totalPerDD += double.Parse(dtWorkGroupWC.Rows[ind][3].ToString());
        //                            totalPerID += double.Parse(dtWorkGroupWC.Rows[ind][5].ToString());
        //                            totalPerSum += double.Parse(dtWorkGroupWC.Rows[ind][7].ToString());
        //                        }
        //                        dtWorkGroupWC.Rows[26][3] = totalPerDD;
        //                        dtWorkGroupWC.Rows[26][5] = totalPerID;
        //                        dtWorkGroupWC.Rows[26][7] = totalPerSum;

        //                        dtWorkGroupWC.Rows[27][3] = 0;
        //                        dtWorkGroupWC.Rows[27][5] = 0;
        //                        dtWorkGroupWC.Rows[27][7] = 0;

        //                        dtWorkGroupWC.Rows[28][3] = "  ";
        //                        dtWorkGroupWC.Rows[28][5] = "  ";
        //                        dtWorkGroupWC.Rows[28][7] = "  ";
        //                        numOfEmployees = numEmplWorkgroupIndirect;
        //                        double totalDays = numOfEmployees * timeSpan.TotalDays;

        //                        DataRow rowEmptyFooter = dtWorkGroupWC.NewRow();
        //                        dtWorkGroupWC.Rows.Add(rowEmptyFooter);

        //                        DataRow footer = dtWorkGroupWC.NewRow();
        //                        footer[1] = "N° Employees: " + numOfEmployees;
        //                        footer[2] = "Total days: ";
        //                        footer[3] = totalDays;
        //                        footer[4] = "Calendar days: ";
        //                        footer[5] = timeSpan.TotalDays;

        //                        dtWorkGroupWC.Rows.Add(footer);
        //                        ds.Tables.Add(dtWorkGroupWC);
        //                        ds.AcceptChanges();



        //                        iCost++;
        //                        if (iCost == 1)
        //                        {
        //                            string ute = "";
        //                            string workGroup = "";
        //                            string costCenterString = "";
        //                            //string plantString = "";
        //                            DataRow rowHeader1 = dtCostCenterWC.NewRow();


        //                            rowHeader1[1] = companyWU.Description;

        //                            rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
        //                            rowHeader1[7] = DateTime.Now.ToString("HH:mm");
        //                            //rowHeader1[8] = "page: " + page;
        //                            dtCostCenterWC.Rows.Add(rowHeader1);

        //                            DataRow rowHeader2 = dtCostCenterWC.NewRow();
        //                            rowHeader2[1] = "   Absenteeism industrial relation";
        //                            rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
        //                            rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
        //                            dtCostCenterWC.Rows.Add(rowHeader2);
        //                            if (dbConnection == null)
        //                                wu = new WorkingUnit();
        //                            else
        //                                wu = new WorkingUnit(dbConnection);

        //                            WorkingUnitTO tempWU = wu.FindWU(listUTE[0].WorkingUnitID);
        //                            ute = tempWU.Code.Trim();

        //                            // get workshop (parent of UTE)
        //                            wu.WUTO = tempWU;
        //                            tempWU = wu.getParentWorkingUnit();
        //                            workGroup = tempWU.Code.Trim();

        //                            // get cost centar
        //                            wu.WUTO = tempWU;
        //                            tempWU = wu.getParentWorkingUnit();
        //                            costCenterString = tempWU.Code.Trim();

        //                            // get plant
        //                            wu.WUTO = tempWU;
        //                            tempWU = wu.getParentWorkingUnit();
        //                            plantString = tempWU.Code.Trim();

        //                            DataRow rowHeader3 = dtCostCenterWC.NewRow();
        //                            rowHeader3[1] = "Qualify:   " + "WC";
        //                            dtCostCenterWC.Rows.Add(rowHeader3);

        //                            DataRow rowPlant = dtCostCenterWC.NewRow();
        //                            rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
        //                            dtCostCenterWC.Rows.Add(rowPlant);

        //                            DataRow rowWorkgroup = dtCostCenterWC.NewRow();
        //                            rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
        //                            dtCostCenterWC.Rows.Add(rowWorkgroup);

        //                            DataRow rowEmpty = dtCostCenterWC.NewRow();
        //                            dtCostCenterWC.Rows.Add(rowEmpty);

        //                            DataRow rowColumns = dtCostCenterWC.NewRow();
        //                            rowColumns[2] = "Direct";
        //                            rowColumns[3] = "%";
        //                            rowColumns[4] = "Indirect";
        //                            rowColumns[5] = "%";
        //                            rowColumns[6] = "Total";
        //                            rowColumns[7] = "%";
        //                            dtCostCenterWC.Rows.Add(rowColumns);

        //                            for (int ind = 7; ind < dtWorkGroupWC.Rows.Count - 2; ind++)
        //                            {
        //                                DataRow row = dtCostCenterWC.NewRow();

        //                                row[1] = dtWorkGroupWC.Rows[ind][1];
        //                                row[2] = dtWorkGroupWC.Rows[ind][2];
        //                                row[4] = dtWorkGroupWC.Rows[ind][4];
        //                                row[6] = dtWorkGroupWC.Rows[ind][6];
        //                                dtCostCenterWC.Rows.Add(row);
        //                                dtCostCenterWC.AcceptChanges();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            for (int ind = 7; ind < dtWorkGroupWC.Rows.Count - 2; ind++)
        //                            {
        //                                if (ind != 25)
        //                                {
        //                                    dtCostCenterWC.Rows[ind][1] = dtWorkGroupWC.Rows[ind][1];
        //                                    dtCostCenterWC.Rows[ind][2] = double.Parse(dtCostCenterWC.Rows[ind][2].ToString()) + double.Parse(dtWorkGroupWC.Rows[ind][2].ToString());
        //                                    dtCostCenterWC.Rows[ind][4] = double.Parse(dtCostCenterWC.Rows[ind][4].ToString()) + double.Parse(dtWorkGroupWC.Rows[ind][4].ToString());
        //                                    dtCostCenterWC.Rows[ind][6] = double.Parse(dtCostCenterWC.Rows[ind][6].ToString()) + double.Parse(dtWorkGroupWC.Rows[ind][6].ToString());
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                if (dbConnection == null)
        //                    wu = new WorkingUnit();
        //                else
        //                    wu = new WorkingUnit(dbConnection);
        //                List<WorkingUnitTO> oneCostCenter = new List<WorkingUnitTO>();
        //                oneCostCenter.Add(costCenter);
        //                List<WorkingUnitTO> CostCenterAndWorkShop = wu.FindAllChildren(oneCostCenter);

        //                if (dtCostCenterBC.Rows.Count > 0)
        //                {
        //                    double totalDD = 0;
        //                    double totalID = 0;
        //                    double totalDDSum = 0;
        //                    double totalIDSum = 0;
        //                    for (int i = 7; i < 25; i++)
        //                    {
        //                        totalDD = double.Parse(dtCostCenterBC.Rows[i][2].ToString());
        //                        totalID = double.Parse(dtCostCenterBC.Rows[i][4].ToString());
        //                        totalDDSum += totalDD;
        //                        totalIDSum += totalID;
        //                    }
        //                    double totaltotal = totalDDSum + totalIDSum;
        //                    for (int i = 7; i < 25; i++)
        //                    {
        //                        Misc.percentage(dtCostCenterBC.Rows[i], totalDDSum, totalIDSum);

        //                        dtCostCenterBC.Rows[i][7] = double.Parse(dtCostCenterBC.Rows[i][6].ToString()) * 100 / totaltotal;
        //                        dtCostCenterBC.AcceptChanges();
        //                    }
        //                    double totalPerDD = 0;
        //                    double totalPerID = 0;
        //                    double totalPerSum = 0;
        //                    for (int i = 7; i < 25; i++)
        //                    {
        //                        totalPerDD += double.Parse(dtCostCenterBC.Rows[i][3].ToString());
        //                        totalPerID += double.Parse(dtCostCenterBC.Rows[i][5].ToString());
        //                        totalPerSum += double.Parse(dtCostCenterBC.Rows[i][7].ToString());
        //                    }
        //                    dtCostCenterBC.Rows[26][3] = totalPerDD;
        //                    dtCostCenterBC.Rows[26][5] = totalPerID;
        //                    dtCostCenterBC.Rows[26][7] = totalPerSum;

        //                    dtCostCenterBC.Rows[27][3] = 0;
        //                    dtCostCenterBC.Rows[27][5] = 0;
        //                    dtCostCenterBC.Rows[27][7] = 0;

        //                    dtCostCenterBC.Rows[28][3] = "  ";
        //                    dtCostCenterBC.Rows[28][5] = "  ";
        //                    dtCostCenterBC.Rows[28][7] = "  ";
        //                    numOfEmployees = numEmplCostDirect;
        //                    double totalDays = numOfEmployees * timeSpan.TotalDays;
        //                    DataRow rowEmptyFooter = dtCostCenterBC.NewRow();
        //                    dtCostCenterBC.Rows.Add(rowEmptyFooter);

        //                    DataRow footer = dtCostCenterBC.NewRow();
        //                    footer[1] = "N° Employees: " + numOfEmployees;
        //                    footer[2] = "Total days: ";
        //                    footer[3] = totalDays;
        //                    footer[4] = "Calendar days: ";
        //                    footer[5] = timeSpan.TotalDays;
        //                    dtCostCenterBC.Rows.Add(footer);

        //                    ds.Tables.Add(dtCostCenterBC);
        //                    ds.AcceptChanges();

        //                    dPlant++;
        //                    if (dPlant == 1)
        //                    {
        //                        DataRow rowHeader1 = dtPlantBC.NewRow();


        //                        rowHeader1[1] = companyWU.Description;

        //                        rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
        //                        rowHeader1[7] = DateTime.Now.ToString("HH:mm");
        //                        //rowHeader1[8] = "page: " + page;
        //                        dtPlantBC.Rows.Add(rowHeader1);

        //                        DataRow rowHeader2 = dtPlantBC.NewRow();
        //                        rowHeader2[1] = "   Absenteeism industrial relation";
        //                        rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
        //                        rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
        //                        dtPlantBC.Rows.Add(rowHeader2);


        //                        DataRow rowHeader3 = dtPlantBC.NewRow();
        //                        rowHeader3[1] = "Qualify:   " + "BC";
        //                        dtPlantBC.Rows.Add(rowHeader3);

        //                        DataRow rowPlant = dtPlantBC.NewRow();
        //                        rowPlant[1] = "Plant:    " + plantString + "    Cost center: " + "xxxx";
        //                        dtPlantBC.Rows.Add(rowPlant);

        //                        DataRow rowWorkgroup = dtPlantBC.NewRow();
        //                        rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
        //                        dtPlantBC.Rows.Add(rowWorkgroup);

        //                        DataRow rowEmpty = dtPlantBC.NewRow();
        //                        dtPlantBC.Rows.Add(rowEmpty);

        //                        DataRow rowColumns = dtPlantBC.NewRow();
        //                        rowColumns[2] = "Direct";
        //                        rowColumns[3] = "%";
        //                        rowColumns[4] = "Indirect";
        //                        rowColumns[5] = "%";
        //                        rowColumns[6] = "Total";
        //                        rowColumns[7] = "%";
        //                        dtPlantBC.Rows.Add(rowColumns);

        //                        for (int ind = 7; ind < dtCostCenterBC.Rows.Count - 2; ind++)
        //                        {
        //                            DataRow row = dtPlantBC.NewRow();

        //                            row[1] = dtCostCenterBC.Rows[ind][1];
        //                            row[2] = dtCostCenterBC.Rows[ind][2];
        //                            row[4] = dtCostCenterBC.Rows[ind][4];
        //                            row[6] = dtCostCenterBC.Rows[ind][6];
        //                            dtPlantBC.Rows.Add(row);
        //                            dtPlantBC.AcceptChanges();

        //                        }
        //                    }
        //                    else
        //                    {
        //                        for (int ind = 7; ind < dtCostCenterBC.Rows.Count - 2; ind++)
        //                        {
        //                            if (ind != 25)
        //                            {

        //                                dtPlantBC.Rows[ind][1] = dtCostCenterBC.Rows[ind][1];
        //                                dtPlantBC.Rows[ind][2] = double.Parse(dtPlantBC.Rows[ind][2].ToString()) + double.Parse(dtCostCenterBC.Rows[ind][2].ToString());
        //                                dtPlantBC.Rows[ind][4] = double.Parse(dtPlantBC.Rows[ind][4].ToString()) + double.Parse(dtCostCenterBC.Rows[ind][4].ToString());
        //                                dtPlantBC.Rows[ind][6] = double.Parse(dtPlantBC.Rows[ind][6].ToString()) + double.Parse(dtCostCenterBC.Rows[ind][6].ToString());
        //                            }
        //                        }
        //                    }
        //                }
        //                if (dtCostCenterWC.Rows.Count > 0)
        //                {
        //                    double totalDD = 0;
        //                    double totalID = 0;
        //                    double totalDDSum = 0;
        //                    double totalIDSum = 0;
        //                    for (int i = 7; i < 25; i++)
        //                    {
        //                        totalDD = double.Parse(dtCostCenterWC.Rows[i][2].ToString());
        //                        totalID = double.Parse(dtCostCenterWC.Rows[i][4].ToString());
        //                        totalDDSum += totalDD;
        //                        totalIDSum += totalID;
        //                    }
        //                    double totaltotal = totalDDSum + totalIDSum;
        //                    for (int i = 7; i < 25; i++)
        //                    {
        //                        Misc.percentage(dtCostCenterWC.Rows[i], totalDDSum, totalIDSum);

        //                        dtCostCenterWC.Rows[i][7] = double.Parse(dtCostCenterWC.Rows[i][6].ToString()) * 100 / totaltotal;
        //                        dtCostCenterWC.AcceptChanges();
        //                    }
        //                    double totalPerDD = 0;
        //                    double totalPerID = 0;
        //                    double totalPerSum = 0;
        //                    for (int i = 7; i < 25; i++)
        //                    {
        //                        totalPerDD += double.Parse(dtCostCenterWC.Rows[i][3].ToString());
        //                        totalPerID += double.Parse(dtCostCenterWC.Rows[i][5].ToString());
        //                        totalPerSum += double.Parse(dtCostCenterWC.Rows[i][7].ToString());
        //                    }
        //                    dtCostCenterWC.Rows[26][3] = totalPerDD;
        //                    dtCostCenterWC.Rows[26][5] = totalPerID;
        //                    dtCostCenterWC.Rows[26][7] = totalPerSum;

        //                    dtCostCenterWC.Rows[27][3] = 0;
        //                    dtCostCenterWC.Rows[27][5] = 0;
        //                    dtCostCenterWC.Rows[27][7] = 0;

        //                    dtCostCenterWC.Rows[28][3] = "  ";
        //                    dtCostCenterWC.Rows[28][5] = "  ";
        //                    dtCostCenterWC.Rows[28][7] = "  ";

        //                    numOfEmployees = numEmplCostIndirect;
        //                    double totalDays = numOfEmployees * timeSpan.TotalDays;

        //                    DataRow rowEmptyFooter = dtCostCenterWC.NewRow();
        //                    dtCostCenterWC.Rows.Add(rowEmptyFooter);

        //                    DataRow footer = dtCostCenterWC.NewRow();
        //                    footer[1] = "N° Employees: " + numOfEmployees;
        //                    footer[2] = "Total days: ";
        //                    footer[3] = totalDays;
        //                    footer[4] = "Calendar days: ";
        //                    footer[5] = timeSpan.TotalDays;
        //                    dtCostCenterWC.Rows.Add(footer);

        //                    ds.Tables.Add(dtCostCenterWC);
        //                    ds.AcceptChanges();


        //                    iPlant++;
        //                    if (iPlant == 1)
        //                    {
        //                        DataRow rowHeader1 = dtPlantWC.NewRow();

        //                        rowHeader1[1] = companyWU.Description;

        //                        rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
        //                        rowHeader1[7] = DateTime.Now.ToString("HH:mm");
        //                        //rowHeader1[8] = "page: " + page;
        //                        dtPlantWC.Rows.Add(rowHeader1);

        //                        DataRow rowHeader2 = dtPlantWC.NewRow();
        //                        rowHeader2[1] = "   Absenteeism industrial relation";
        //                        rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
        //                        rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
        //                        dtPlantWC.Rows.Add(rowHeader2);

        //                        DataRow rowHeader3 = dtPlantWC.NewRow();
        //                        rowHeader3[1] = "Qualify:   " + "WC";
        //                        dtPlantWC.Rows.Add(rowHeader3);


        //                        DataRow rowPlant = dtPlantWC.NewRow();
        //                        rowPlant[1] = "Plant:    " + plantString + "    Cost center: " + "xxxx";
        //                        dtPlantWC.Rows.Add(rowPlant);

        //                        DataRow rowWorkgroup = dtPlantWC.NewRow();
        //                        rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
        //                        dtPlantWC.Rows.Add(rowWorkgroup);

        //                        DataRow rowEmpty = dtPlantWC.NewRow();
        //                        dtPlantWC.Rows.Add(rowEmpty);

        //                        DataRow rowColumns = dtPlantWC.NewRow();
        //                        rowColumns[2] = "Direct";
        //                        rowColumns[3] = "%";
        //                        rowColumns[4] = "Indirect";
        //                        rowColumns[5] = "%";
        //                        rowColumns[6] = "Total";
        //                        rowColumns[7] = "%";
        //                        dtPlantWC.Rows.Add(rowColumns);

        //                        for (int ind = 7; ind < dtCostCenterWC.Rows.Count - 2; ind++)
        //                        {
        //                            DataRow row = dtPlantWC.NewRow();

        //                            row[1] = dtCostCenterWC.Rows[ind][1];
        //                            row[2] = dtCostCenterWC.Rows[ind][2];
        //                            row[4] = dtCostCenterWC.Rows[ind][4];
        //                            row[6] = dtCostCenterWC.Rows[ind][6];
        //                            dtPlantWC.Rows.Add(row);
        //                            dtPlantWC.AcceptChanges();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        for (int ind = 7; ind < dtCostCenterWC.Rows.Count - 2; ind++)
        //                        {
        //                            if (ind != 25)
        //                            {

        //                                dtPlantWC.Rows[ind][1] = dtCostCenterWC.Rows[ind][1];
        //                                dtPlantWC.Rows[ind][2] = double.Parse(dtPlantWC.Rows[ind][2].ToString()) + double.Parse(dtCostCenterWC.Rows[ind][2].ToString());
        //                                dtPlantWC.Rows[ind][4] = double.Parse(dtPlantWC.Rows[ind][4].ToString()) + double.Parse(dtCostCenterWC.Rows[ind][4].ToString());
        //                                dtPlantWC.Rows[ind][6] = double.Parse(dtPlantWC.Rows[ind][6].ToString()) + double.Parse(dtCostCenterWC.Rows[ind][6].ToString());
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            if (dbConnection == null)
        //                wu = new WorkingUnit();
        //            else
        //                wu = new WorkingUnit(dbConnection);
        //            List<WorkingUnitTO> onePlant = new List<WorkingUnitTO>();
        //            onePlant.Add(plant);
        //            List<WorkingUnitTO> PlantAndCostCentre = wu.FindAllChildren(onePlant);

        //            if (dtPlantBC.Rows.Count > 0)
        //            {
        //                double totalDD = 0;
        //                double totalID = 0;
        //                double totalDDSum = 0;
        //                double totalIDSum = 0;
        //                for (int i = 7; i < 25; i++)
        //                {
        //                    totalDD = double.Parse(dtPlantBC.Rows[i][2].ToString());
        //                    totalID = double.Parse(dtPlantBC.Rows[i][4].ToString());
        //                    totalDDSum += totalDD;
        //                    totalIDSum += totalID;
        //                }
        //                double totaltotal = totalDDSum + totalIDSum;
        //                for (int i = 7; i < 25; i++)
        //                {
        //                    Misc.percentage(dtPlantBC.Rows[i], totalDDSum, totalIDSum);

        //                    dtPlantBC.Rows[i][7] = double.Parse(dtPlantBC.Rows[i][6].ToString()) * 100 / totaltotal;
        //                    dtPlantBC.AcceptChanges();
        //                }
        //                double totalPerDD = 0;
        //                double totalPerID = 0;
        //                double totalPerSum = 0;
        //                for (int i = 7; i < 25; i++)
        //                {
        //                    totalPerDD += double.Parse(dtPlantBC.Rows[i][3].ToString());
        //                    totalPerID += double.Parse(dtPlantBC.Rows[i][5].ToString());
        //                    totalPerSum += double.Parse(dtPlantBC.Rows[i][7].ToString());
        //                }
        //                dtPlantBC.Rows[26][3] = totalPerDD;
        //                dtPlantBC.Rows[26][5] = totalPerID;
        //                dtPlantBC.Rows[26][7] = totalPerSum;

        //                dtPlantBC.Rows[27][3] = 0;
        //                dtPlantBC.Rows[27][5] = 0;
        //                dtPlantBC.Rows[27][7] = 0;

        //                dtPlantBC.Rows[28][3] = "  ";
        //                dtPlantBC.Rows[28][5] = "  ";
        //                dtPlantBC.Rows[28][7] = "  ";

        //                numOfEmployees = numEmplPlantDirect;
        //                double totalDays = numOfEmployees * timeSpan.TotalDays;
        //                DataRow rowEmptyFooter = dtPlantBC.NewRow();
        //                dtPlantBC.Rows.Add(rowEmptyFooter);

        //                DataRow footer = dtPlantBC.NewRow();
        //                footer[1] = "N° Employees: " + numOfEmployees;
        //                footer[2] = "Total days: ";
        //                footer[3] = totalDays;
        //                footer[4] = "Calendar days: ";
        //                footer[5] = timeSpan.TotalDays;
        //                dtPlantBC.Rows.Add(footer);

        //                ds.Tables.Add(dtPlantBC);
        //                ds.AcceptChanges();

        //                dcompany++;
        //                if (dcompany == 1)
        //                {
        //                    DataRow rowHeader1 = dtCompanyBC.NewRow();
        //                    rowHeader1[1] = companyWU.Description;

        //                    rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
        //                    rowHeader1[7] = DateTime.Now.ToString("HH:mm");
        //                    //rowHeader1[8] = "page: " + page;
        //                    dtCompanyBC.Rows.Add(rowHeader1);

        //                    DataRow rowHeader2 = dtCompanyBC.NewRow();
        //                    rowHeader2[1] = "   Absenteeism industrial relation";
        //                    rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
        //                    rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
        //                    dtCompanyBC.Rows.Add(rowHeader2);


        //                    DataRow rowHeader3 = dtCompanyBC.NewRow();
        //                    rowHeader3[1] = "Qualify:   " + "BC";
        //                    dtCompanyBC.Rows.Add(rowHeader3);

        //                    DataRow rowPlant = dtCompanyBC.NewRow();
        //                    rowPlant[1] = "Company report";
        //                    dtCompanyBC.Rows.Add(rowPlant);

        //                    DataRow rowWorkgroup = dtCompanyBC.NewRow();
        //                    dtCompanyBC.Rows.Add(rowWorkgroup);

        //                    DataRow rowEmpty = dtCompanyBC.NewRow();
        //                    dtCompanyBC.Rows.Add(rowEmpty);

        //                    DataRow rowColumns = dtCompanyBC.NewRow();
        //                    rowColumns[2] = "Direct";
        //                    rowColumns[3] = "%";
        //                    rowColumns[4] = "Indirect";
        //                    rowColumns[5] = "%";
        //                    rowColumns[6] = "Total";
        //                    rowColumns[7] = "%";
        //                    dtCompanyBC.Rows.Add(rowColumns);

        //                    for (int ind = 7; ind < dtPlantBC.Rows.Count - 2; ind++)
        //                    {

        //                        DataRow row = dtCompanyBC.NewRow();

        //                        row[1] = dtPlantBC.Rows[ind][1];
        //                        row[2] = dtPlantBC.Rows[ind][2];
        //                        row[4] = dtPlantBC.Rows[ind][4];
        //                        row[6] = dtPlantBC.Rows[ind][6];
        //                        dtCompanyBC.Rows.Add(row);
        //                        dtCompanyBC.AcceptChanges();

        //                    }
        //                }
        //                else
        //                {
        //                    for (int ind = 7; ind < dtPlantBC.Rows.Count - 2; ind++)
        //                    {
        //                        if (ind != 25)
        //                        {

        //                            dtCompanyBC.Rows[ind][1] = dtPlantBC.Rows[ind][1];
        //                            dtCompanyBC.Rows[ind][2] = double.Parse(dtCompanyBC.Rows[ind][2].ToString()) + double.Parse(dtPlantBC.Rows[ind][2].ToString());
        //                            dtCompanyBC.Rows[ind][4] = double.Parse(dtCompanyBC.Rows[ind][4].ToString()) + double.Parse(dtPlantBC.Rows[ind][4].ToString());
        //                            dtCompanyBC.Rows[ind][6] = double.Parse(dtCompanyBC.Rows[ind][6].ToString()) + double.Parse(dtPlantBC.Rows[ind][6].ToString());
        //                        }
        //                    }
        //                }

        //            }
        //            if (dtPlantWC.Rows.Count > 0)
        //            {
        //                double totalDD = 0;
        //                double totalID = 0;
        //                double totalDDSum = 0;
        //                double totalIDSum = 0;
        //                for (int i = 7; i < 25; i++)
        //                {
        //                    totalDD = double.Parse(dtPlantWC.Rows[i][2].ToString());
        //                    totalID = double.Parse(dtPlantWC.Rows[i][4].ToString());
        //                    totalDDSum += totalDD;
        //                    totalIDSum += totalID;
        //                }
        //                double totaltotal = totalDDSum + totalIDSum;
        //                for (int i = 7; i < 25; i++)
        //                {
        //                    Misc.percentage(dtPlantWC.Rows[i], totalDDSum, totalIDSum);

        //                    dtPlantWC.Rows[i][7] = double.Parse(dtPlantWC.Rows[i][6].ToString()) * 100 / totaltotal;
        //                    dtPlantWC.AcceptChanges();
        //                }
        //                double totalPerDD = 0;
        //                double totalPerID = 0;
        //                double totalPerSum = 0;
        //                for (int i = 7; i < 25; i++)
        //                {
        //                    totalPerDD += double.Parse(dtPlantWC.Rows[i][3].ToString());
        //                    totalPerID += double.Parse(dtPlantWC.Rows[i][5].ToString());
        //                    totalPerSum += double.Parse(dtPlantWC.Rows[i][7].ToString());
        //                }
        //                dtPlantWC.Rows[26][3] = totalPerDD;
        //                dtPlantWC.Rows[26][5] = totalPerID;
        //                dtPlantWC.Rows[26][7] = totalPerSum;

        //                dtPlantWC.Rows[27][3] = 0;
        //                dtPlantWC.Rows[27][5] = 0;
        //                dtPlantWC.Rows[27][7] = 0;

        //                dtPlantWC.Rows[28][3] = "  ";
        //                dtPlantWC.Rows[28][5] = "  ";
        //                dtPlantWC.Rows[28][7] = "  ";
        //                numOfEmployees = numEmplPlantIndirect;
        //                double totalDays = numOfEmployees * timeSpan.TotalDays;
        //                DataRow rowEmptyFooter = dtPlantWC.NewRow();
        //                dtPlantWC.Rows.Add(rowEmptyFooter);

        //                DataRow footer = dtPlantWC.NewRow();
        //                footer[1] = "N° Employees: " + numOfEmployees;
        //                footer[2] = "Total days: ";
        //                footer[3] = totalDays;
        //                footer[4] = "Calendar days: ";
        //                footer[5] = timeSpan.TotalDays;

        //                dtPlantWC.Rows.Add(footer);
        //                ds.Tables.Add(dtPlantWC);
        //                ds.AcceptChanges();

        //                icompany++;
        //                if (icompany == 1)
        //                {
        //                    DataRow rowHeader1 = dtCompanyWC.NewRow();

        //                    rowHeader1[1] = companyWU.Description;

        //                    rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
        //                    rowHeader1[7] = DateTime.Now.ToString("HH:mm");
        //                    //rowHeader1[8] = "page: " + page;
        //                    dtCompanyWC.Rows.Add(rowHeader1);

        //                    DataRow rowHeader2 = dtCompanyWC.NewRow();
        //                    rowHeader2[1] = "   Absenteeism industrial relation";
        //                    rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
        //                    rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
        //                    dtCompanyWC.Rows.Add(rowHeader2);

        //                    DataRow rowHeader3 = dtCompanyWC.NewRow();
        //                    rowHeader3[1] = "Qualify:   " + "WC";
        //                    dtCompanyWC.Rows.Add(rowHeader3);


        //                    DataRow rowPlant = dtCompanyWC.NewRow();
        //                    rowPlant[1] = "Company report";
        //                    dtCompanyWC.Rows.Add(rowPlant);

        //                    DataRow rowWorkgroup = dtCompanyWC.NewRow();
        //                    dtCompanyWC.Rows.Add(rowWorkgroup);

        //                    DataRow rowEmpty = dtCompanyWC.NewRow();
        //                    dtCompanyWC.Rows.Add(rowEmpty);

        //                    DataRow rowColumns = dtCompanyWC.NewRow();
        //                    rowColumns[2] = "Direct";
        //                    rowColumns[3] = "%";
        //                    rowColumns[4] = "Indirect";
        //                    rowColumns[5] = "%";
        //                    rowColumns[6] = "Total";
        //                    rowColumns[7] = "%";
        //                    dtCompanyWC.Rows.Add(rowColumns);

        //                    for (int ind = 7; ind < dtPlantWC.Rows.Count - 2; ind++)
        //                    {
        //                        DataRow row = dtCompanyWC.NewRow();

        //                        row[1] = dtPlantWC.Rows[ind][1];
        //                        row[2] = dtPlantWC.Rows[ind][2];
        //                        row[4] = dtPlantWC.Rows[ind][4];
        //                        row[6] = dtPlantWC.Rows[ind][6];
        //                        dtCompanyWC.Rows.Add(row);
        //                        dtCompanyWC.AcceptChanges();
        //                    }
        //                }
        //                else
        //                {
        //                    for (int ind = 7; ind < dtPlantWC.Rows.Count - 2; ind++)
        //                    {
        //                        if (ind != 25)
        //                        {
        //                            dtCompanyWC.Rows[ind][1] = dtCompanyWC.Rows[ind][1];
        //                            dtCompanyWC.Rows[ind][2] = double.Parse(dtCompanyWC.Rows[ind][2].ToString()) + double.Parse(dtPlantWC.Rows[ind][2].ToString());
        //                            dtCompanyWC.Rows[ind][4] = double.Parse(dtCompanyWC.Rows[ind][4].ToString()) + double.Parse(dtPlantWC.Rows[ind][4].ToString());
        //                            dtCompanyWC.Rows[ind][6] = double.Parse(dtCompanyWC.Rows[ind][6].ToString()) + double.Parse(dtPlantWC.Rows[ind][6].ToString());
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        if (dtCompanyBC.Rows.Count > 6)
        //        {
        //            double totalDD = 0;
        //            double totalID = 0;
        //            double totalDDSum = 0;
        //            double totalIDSum = 0;
        //            for (int i = 7; i < 25; i++)
        //            {
        //                totalDD = double.Parse(dtCompanyBC.Rows[i][2].ToString());
        //                totalID = double.Parse(dtCompanyBC.Rows[i][4].ToString());
        //                totalDDSum += totalDD;
        //                totalIDSum += totalID;
        //            }
        //            double totaltotal = totalDDSum + totalIDSum;
        //            for (int i = 7; i < 25; i++)
        //            {
        //                Misc.percentage(dtCompanyBC.Rows[i], totalDDSum, totalIDSum);

        //                dtCompanyBC.Rows[i][7] = double.Parse(dtCompanyBC.Rows[i][6].ToString()) * 100 / totaltotal;
        //                dtCompanyBC.AcceptChanges();
        //            }
        //            double totalPerDD = 0;
        //            double totalPerID = 0;
        //            double totalPerSum = 0;
        //            for (int i = 7; i < 25; i++)
        //            {
        //                totalPerDD += double.Parse(dtCompanyBC.Rows[i][3].ToString());
        //                totalPerID += double.Parse(dtCompanyBC.Rows[i][5].ToString());
        //                totalPerSum += double.Parse(dtCompanyBC.Rows[i][7].ToString());
        //            }
        //            dtCompanyBC.Rows[26][3] = totalPerDD;
        //            dtCompanyBC.Rows[26][5] = totalPerID;
        //            dtCompanyBC.Rows[26][7] = totalPerSum;

        //            dtCompanyBC.Rows[27][3] = 0;
        //            dtCompanyBC.Rows[27][5] = 0;
        //            dtCompanyBC.Rows[27][7] = 0;

        //            dtCompanyBC.Rows[28][3] = "  ";
        //            dtCompanyBC.Rows[28][5] = "  ";
        //            dtCompanyBC.Rows[28][7] = "  ";
        //            dtCompanyBC.AcceptChanges();
        //            numOfEmployees = numEmplCompanyDirect;
        //            double totalDays = numOfEmployees * timeSpan.TotalDays;
        //            DataRow rowEmptyFooter = dtCompanyBC.NewRow();
        //            dtCompanyBC.Rows.Add(rowEmptyFooter);

        //            DataRow footer = dtCompanyBC.NewRow();
        //            footer[1] = "N° Employees: " + numOfEmployees;
        //            footer[2] = "Total days: ";
        //            footer[3] = totalDays;
        //            footer[4] = "Calendar days: ";
        //            footer[5] = timeSpan.TotalDays;
        //            dtCompanyBC.Rows.Add(footer);

        //            ds.Tables.Add(dtCompanyBC);
        //            ds.AcceptChanges();
        //        }
        //        if (dtCompanyWC.Rows.Count > 6)
        //        {
        //            double totalDD = 0;
        //            double totalID = 0;
        //            double totalDDSum = 0;
        //            double totalIDSum = 0;
        //            for (int i = 7; i < 25; i++)
        //            {
        //                totalDD = double.Parse(dtCompanyWC.Rows[i][2].ToString());
        //                totalID = double.Parse(dtCompanyWC.Rows[i][4].ToString());
        //                totalDDSum += totalDD;
        //                totalIDSum += totalID;
        //            }
        //            double totaltotal = totalDDSum + totalIDSum;
        //            for (int i = 7; i < 25; i++)
        //            {
        //                Misc.percentage(dtCompanyWC.Rows[i], totalIDSum, totalIDSum);
        //                dtCompanyWC.Rows[i][7] = double.Parse(dtCompanyWC.Rows[i][6].ToString()) * 100 / totaltotal;
        //                dtCompanyWC.AcceptChanges();

        //            }
        //            double totalPerDD = 0;
        //            double totalPerID = 0;
        //            double totalPerSum = 0;
        //            for (int i = 7; i < 25; i++)
        //            {
        //                totalPerDD += double.Parse(dtCompanyWC.Rows[i][3].ToString());
        //                totalPerID += double.Parse(dtCompanyWC.Rows[i][5].ToString());
        //                totalPerSum += double.Parse(dtCompanyWC.Rows[i][7].ToString());
        //            }
        //            dtCompanyWC.Rows[26][3] = totalPerDD;
        //            dtCompanyWC.Rows[26][5] = totalPerID;
        //            dtCompanyWC.Rows[26][7] = totalPerSum;

        //            dtCompanyWC.Rows[27][3] = 0;
        //            dtCompanyWC.Rows[27][5] = 0;
        //            dtCompanyWC.Rows[27][7] = 0;

        //            dtCompanyWC.Rows[28][3] = "  ";
        //            dtCompanyWC.Rows[28][5] = "  ";
        //            dtCompanyWC.Rows[28][7] = "  ";

        //            dtCompanyWC.AcceptChanges();
        //            numOfEmployees = numEmplCompanyIndirect;
        //            double totalDays = numOfEmployees * timeSpan.TotalDays;
        //            DataRow rowEmptyFooter = dtCompanyWC.NewRow();
        //            dtCompanyWC.Rows.Add(rowEmptyFooter);

        //            DataRow footer = dtCompanyWC.NewRow();
        //            footer[1] = "N° Employees: " + numOfEmployees;
        //            footer[2] = "Total days: ";
        //            footer[3] = totalDays;
        //            footer[4] = "Calendar days: ";
        //            footer[5] = timeSpan.TotalDays;

        //            dtCompanyWC.Rows.Add(footer);
        //            dtCompanyWC.AcceptChanges();
        //            ds.Tables.Add(dtCompanyWC);
        //            ds.AcceptChanges();
        //        }

        //        string Pathh = Directory.GetParent(filePath).FullName;
        //        if (!Directory.Exists(Pathh))
        //        {
        //            Directory.CreateDirectory(Pathh);
        //        }
        //        if (File.Exists(filePath))
        //            File.Delete(filePath);

        //        ExportToExcel.CreateExcelDocument(ds, filePath, false, false);


        //        debug.writeLog("+ Finished 400! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
        //        return filePath;
        //    }
        //    catch (Exception ex)
        //    {

        //        debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "generateExcel400() " + ex.Message);
        //        return "";
        //    }
        //}


        public string GenerateReportService(Object dbConnection, string filePath, List<WorkingUnitTO> listWU, List<DateTime> datesList, int company)
        {
            debug.writeLog("+ Started 400! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            try
            {
                Dictionary<int, PassTypeTO> passTypesDictionaryAll = new Dictionary<int, PassTypeTO>();
                if (dbConnection == null)
                {
                    passTypesDictionaryAll = new PassType().SearchDictionary();
                }
                else
                {
                    passTypesDictionaryAll = new PassType(dbConnection).SearchDictionary();
                }

                DataSet ds = new DataSet(Path.GetFileNameWithoutExtension(filePath));
                int numOfEmployees = 0;
                TimeSpan timeSpan = datesList[datesList.Count - 1] - datesList[0].AddDays(-1);
                WorkingUnit wu;
                Employee Empl;
                IOPairProcessed ioPairProc;

                WorkingUnitTO companyWU = new WorkingUnitTO();
                if (dbConnection != null)
                {
                    companyWU = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company));
                }
                else
                {
                    companyWU = ((WorkingUnitTO)new WorkingUnit().FindWU(company));
                }
                if (dbConnection == null)
                {
                    wu = new WorkingUnit();
                    Empl = new Employee();
                    ioPairProc = new IOPairProcessed();
                }
                else
                {
                    wu = new WorkingUnit(dbConnection);
                    Empl = new Employee(dbConnection);
                    ioPairProc = new IOPairProcessed(dbConnection);
                }

                System.Data.DataTable dtCompanyBC = new System.Data.DataTable(company + "BC");
                dtCompanyBC.Columns.Add("1", typeof(string));
                dtCompanyBC.Columns.Add("   ", typeof(string));
                dtCompanyBC.Columns.Add("Direct", typeof(string));
                dtCompanyBC.Columns.Add("%  ", typeof(string));
                dtCompanyBC.Columns.Add("Indirect", typeof(string));
                dtCompanyBC.Columns.Add("%   ", typeof(string));
                dtCompanyBC.Columns.Add("Total", typeof(string));
                dtCompanyBC.Columns.Add(" %   ", typeof(string));

                System.Data.DataTable dtCompanyWC = new System.Data.DataTable(company + "WC");
                dtCompanyWC.Columns.Add("1", typeof(string));
                dtCompanyWC.Columns.Add("   ", typeof(string));
                dtCompanyWC.Columns.Add("Direct", typeof(string));
                dtCompanyWC.Columns.Add("%  ", typeof(string));
                dtCompanyWC.Columns.Add("Indirect", typeof(string));
                dtCompanyWC.Columns.Add("%   ", typeof(string));
                dtCompanyWC.Columns.Add("Total", typeof(string));
                dtCompanyWC.Columns.Add(" %   ", typeof(string));
                int icompany = 0;
                int dcompany = 0;
                int numEmplCompanyDirect = 0;
                int numEmplCompanyIndirect = 0;
                //foreach plant in company find CCs and than data for each cost center write in datatable od plant
                foreach (WorkingUnitTO plant in listWU)
                {
                    int iPlant = 0;
                    int dPlant = 0;
                    int numEmplPlantDirect = 0;
                    int numEmplPlantIndirect = 0;
                    string plantString = "";
                    List<WorkingUnitTO> listCostCenter = new List<WorkingUnitTO>();
                    if (dbConnection == null)
                        listCostCenter = new WorkingUnit().SearchChildWU(plant.WorkingUnitID.ToString());
                    else
                        listCostCenter = new WorkingUnit(dbConnection).SearchChildWU(plant.WorkingUnitID.ToString());

                    System.Data.DataTable dtPlantBC = new System.Data.DataTable(plant.WorkingUnitID + "BC");
                    dtPlantBC.Columns.Add("1", typeof(string));
                    dtPlantBC.Columns.Add("   ", typeof(string));
                    dtPlantBC.Columns.Add("Direct", typeof(string));
                    dtPlantBC.Columns.Add("%  ", typeof(string));
                    dtPlantBC.Columns.Add("Indirect", typeof(string));
                    dtPlantBC.Columns.Add("%   ", typeof(string));
                    dtPlantBC.Columns.Add("Total", typeof(string));
                    dtPlantBC.Columns.Add(" %   ", typeof(string));

                    System.Data.DataTable dtPlantWC = new System.Data.DataTable(plant.WorkingUnitID + "WC");
                    dtPlantWC.Columns.Add("1", typeof(string));
                    dtPlantWC.Columns.Add("   ", typeof(string));
                    dtPlantWC.Columns.Add("Direct", typeof(string));
                    dtPlantWC.Columns.Add("%  ", typeof(string));
                    dtPlantWC.Columns.Add("Indirect", typeof(string));
                    dtPlantWC.Columns.Add("%   ", typeof(string));
                    dtPlantWC.Columns.Add("Total", typeof(string));
                    dtPlantWC.Columns.Add(" %   ", typeof(string));

                    foreach (WorkingUnitTO costCenter in listCostCenter)
                    {
                        int dCost = 0;
                        int iCost = 0;
                        int numEmplCostDirect = 0;
                        int numEmplCostIndirect = 0;
                        List<WorkingUnitTO> listWorkshop = new List<WorkingUnitTO>();

                        if (dbConnection == null)
                            wu = new WorkingUnit();
                        else
                            wu = new WorkingUnit(dbConnection);

                        listWorkshop = wu.SearchChildWU(costCenter.WorkingUnitID.ToString());
                        
                        System.Data.DataTable dtCostCenterBC = new System.Data.DataTable(costCenter.WorkingUnitID + "BC");
                        dtCostCenterBC.Columns.Add("1", typeof(string));
                        dtCostCenterBC.Columns.Add("   ", typeof(string));
                        dtCostCenterBC.Columns.Add("Direct", typeof(string));
                        dtCostCenterBC.Columns.Add("%  ", typeof(string));
                        dtCostCenterBC.Columns.Add("Indirect", typeof(string));
                        dtCostCenterBC.Columns.Add("%   ", typeof(string));
                        dtCostCenterBC.Columns.Add("Total", typeof(string));
                        dtCostCenterBC.Columns.Add(" %   ", typeof(string));

                        System.Data.DataTable dtCostCenterWC = new System.Data.DataTable(costCenter.WorkingUnitID + "WC");
                        dtCostCenterWC.Columns.Add("1", typeof(string));
                        dtCostCenterWC.Columns.Add("   ", typeof(string));
                        dtCostCenterWC.Columns.Add("Direct", typeof(string));
                        dtCostCenterWC.Columns.Add("%  ", typeof(string));
                        dtCostCenterWC.Columns.Add("Indirect", typeof(string));
                        dtCostCenterWC.Columns.Add("%   ", typeof(string));
                        dtCostCenterWC.Columns.Add("Total", typeof(string));
                        dtCostCenterWC.Columns.Add(" %   ", typeof(string));

                        foreach (WorkingUnitTO workshop in listWorkshop)
                        {
                            int d = 0;
                            int i = 0;
                            int numEmplWorkgroupDirect = 0;
                            int numEmplWorkgroupIndirect = 0;

                            if (dbConnection == null)
                                wu = new WorkingUnit();
                            else
                                wu = new WorkingUnit(dbConnection);
                            List<WorkingUnitTO> listUTE = wu.SearchChildWU(workshop.WorkingUnitID.ToString());

                            System.Data.DataTable dtWorkGroupBC = new System.Data.DataTable(workshop.WorkingUnitID + "BC");
                            dtWorkGroupBC.Columns.Add("1", typeof(string));
                            dtWorkGroupBC.Columns.Add("   ", typeof(string));
                            dtWorkGroupBC.Columns.Add("Direct", typeof(string));
                            dtWorkGroupBC.Columns.Add("%  ", typeof(string));
                            dtWorkGroupBC.Columns.Add("Indirect", typeof(string));
                            dtWorkGroupBC.Columns.Add("%   ", typeof(string));
                            dtWorkGroupBC.Columns.Add("Total", typeof(string));
                            dtWorkGroupBC.Columns.Add(" %   ", typeof(string));

                            System.Data.DataTable dtWorkGroupWC = new System.Data.DataTable(workshop.WorkingUnitID + "WC");
                            dtWorkGroupWC.Columns.Add("1", typeof(string));
                            dtWorkGroupWC.Columns.Add("   ", typeof(string));
                            dtWorkGroupWC.Columns.Add("Direct", typeof(string));
                            dtWorkGroupWC.Columns.Add("%  ", typeof(string));
                            dtWorkGroupWC.Columns.Add("Indirect", typeof(string));
                            dtWorkGroupWC.Columns.Add("%   ", typeof(string));
                            dtWorkGroupWC.Columns.Add("Total", typeof(string));
                            dtWorkGroupWC.Columns.Add(" %   ", typeof(string));
                            
                            foreach (WorkingUnitTO workingUnit in listUTE)
                            {
                                List<EmployeeTO> listEmpl = new List<EmployeeTO>();
                                if (dbConnection == null)
                                    Empl = new Employee();
                                else
                                    Empl = new Employee(dbConnection);

                                listEmpl = Empl.SearchByWULoans(workingUnit.WorkingUnitID.ToString(), -1, null, datesList[0], datesList[datesList.Count - 1]);

                                if (listEmpl.Count > 0)
                                {
                                    Dictionary<string, string> dictEmplTypes = Misc.emplTypes(dbConnection, listEmpl, company);

                                    string emplIdsBC = "";
                                    string emplIdsWC = "";
                                    //employees types, for two datatable
                                    foreach (KeyValuePair<string, string> pair in dictEmplTypes)
                                    {
                                        if (pair.Value.Equals("BC"))
                                            emplIdsBC += pair.Key + ",";
                                        else
                                            emplIdsWC += pair.Key + ",";
                                    }

                                    if (emplIdsBC.Length > 0)
                                        emplIdsBC = emplIdsBC.Remove(emplIdsBC.LastIndexOf(','));

                                    if (emplIdsWC.Length > 0)
                                        emplIdsWC = emplIdsWC.Remove(emplIdsWC.LastIndexOf(','));
                                    
                                    //datatable BC
                                    if (emplIdsBC.Length > 0)
                                    {
                                        System.Data.DataTable dtBC = new System.Data.DataTable(workingUnit.WorkingUnitID + " BC");
                                        dtBC.Columns.Add("1", typeof(string));
                                        dtBC.Columns.Add("   ", typeof(string));
                                        dtBC.Columns.Add("Direct", typeof(string));
                                        dtBC.Columns.Add("%  ", typeof(string));
                                        dtBC.Columns.Add("Indirect", typeof(string));
                                        dtBC.Columns.Add("%   ", typeof(string));
                                        dtBC.Columns.Add("Total", typeof(string));
                                        dtBC.Columns.Add(" %   ", typeof(string));

                                        dictEmplTypes = Misc.emplBranch(dbConnection, emplIdsBC, company);
                                        string emplIdsBCDirect = "";
                                        string emplIdsBCIndirect = "";

                                        //split employees - direct and indirect, two columns in report
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

                                        //find iopairs for both direct and indirect
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

                                        numOfEmployees = emplIdsBC.Split(',').Length;
                                        numEmplWorkgroupDirect += numOfEmployees;
                                        numEmplCostDirect += numOfEmployees;
                                        numEmplPlantDirect += numOfEmployees;
                                        numEmplCompanyDirect += numOfEmployees;
                                        double totalDays = numOfEmployees * timeSpan.TotalDays;
                                        if (IOPairListDirectBC.Count > 0 || IOPairListIndirectBC.Count > 0)
                                        {
                                            populateDataTable400New(dbConnection, dtBC, datesList, companyWU, IOPairListDirectBC, IOPairListIndirectBC, emplIdsBCDirect, emplIdsBCIndirect, "BC", workingUnit, "", passTypesDictionaryAll);

                                            DataRow rowEmptyFooter = dtBC.NewRow();
                                            dtBC.Rows.Add(rowEmptyFooter);

                                            DataRow footer = dtBC.NewRow();
                                            footer[1] = "N° Employees: " + numOfEmployees;
                                            footer[2] = "Total days: ";
                                            footer[3] = totalDays;
                                            footer[4] = "Calendar days: ";
                                            footer[5] = timeSpan.TotalDays;
                                            dtBC.Rows.Add(footer);

                                            ds.Tables.Add(dtBC);
                                            ds.AcceptChanges();

                                            d++;
                                            if (d == 1)
                                            {
                                                string ute = "";
                                                string workGroup = "";
                                                string costCenterString = "";
                                                //string plantString = "";
                                                if (dbConnection == null)
                                                    wu = new WorkingUnit();
                                                else
                                                    wu = new WorkingUnit(dbConnection);

                                                DataRow rowHeader1 = dtWorkGroupBC.NewRow();

                                                rowHeader1[1] = companyWU.Description;

                                                rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                                rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                                //rowHeader1[8] = "page: " + page;
                                                dtWorkGroupBC.Rows.Add(rowHeader1);

                                                DataRow rowHeader2 = dtWorkGroupBC.NewRow();
                                                rowHeader2[1] = "   Absenteeism industrial relation";
                                                rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                                rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                                                dtWorkGroupBC.Rows.Add(rowHeader2);

                                                WorkingUnitTO tempWU = wu.FindWU(workingUnit.WorkingUnitID);
                                                ute = tempWU.Code.Trim();

                                                // get workshop (parent of UTE)
                                                wu.WUTO = tempWU;
                                                tempWU = wu.getParentWorkingUnit();
                                                workGroup = tempWU.Code.Trim();

                                                // get cost centar
                                                wu.WUTO = tempWU;
                                                tempWU = wu.getParentWorkingUnit();
                                                costCenterString = tempWU.Code.Trim();

                                                // get plant
                                                wu.WUTO = tempWU;
                                                tempWU = wu.getParentWorkingUnit();
                                                plantString = tempWU.Code.Trim();

                                                DataRow rowHeader3 = dtWorkGroupBC.NewRow();
                                                rowHeader3[1] = "Qualify:   " + "BC";
                                                dtWorkGroupBC.Rows.Add(rowHeader3);

                                                DataRow rowPlant = dtWorkGroupBC.NewRow();
                                                rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
                                                dtWorkGroupBC.Rows.Add(rowPlant);

                                                DataRow rowWorkgroup = dtWorkGroupBC.NewRow();
                                                rowWorkgroup[1] = "Workgroup: " + workGroup + " UTE: " + "xx";
                                                dtWorkGroupBC.Rows.Add(rowWorkgroup);


                                                DataRow rowEmpty = dtWorkGroupBC.NewRow();
                                                dtWorkGroupBC.Rows.Add(rowEmpty);

                                                DataRow rowColumns = dtWorkGroupBC.NewRow();
                                                rowColumns[2] = "Direct";
                                                rowColumns[3] = "%";
                                                rowColumns[4] = "Indirect";
                                                rowColumns[5] = "%";
                                                rowColumns[6] = "Total";
                                                rowColumns[7] = "%";
                                                dtWorkGroupBC.Rows.Add(rowColumns);
                                                for (int ind = 7; ind < dtBC.Rows.Count - 2; ind++)
                                                {
                                                    DataRow row = dtWorkGroupBC.NewRow();
                                                    row[1] = dtBC.Rows[ind][1];
                                                    row[2] = dtBC.Rows[ind][2];

                                                    row[4] = dtBC.Rows[ind][4];

                                                    row[6] = dtBC.Rows[ind][6];


                                                    dtWorkGroupBC.Rows.Add(row);
                                                    dtWorkGroupBC.AcceptChanges();
                                                }
                                            }
                                            else
                                            {
                                                for (int ind = 7; ind < dtBC.Rows.Count - 2; ind++)
                                                {
                                                    if (ind != 22)
                                                    {

                                                        dtWorkGroupBC.Rows[ind][1] = dtBC.Rows[ind][1];
                                                        dtWorkGroupBC.Rows[ind][2] = double.Parse(dtWorkGroupBC.Rows[ind][2].ToString()) + double.Parse(dtBC.Rows[ind][2].ToString());
                                                        dtWorkGroupBC.Rows[ind][4] = double.Parse(dtWorkGroupBC.Rows[ind][4].ToString()) + double.Parse(dtBC.Rows[ind][4].ToString());
                                                        dtWorkGroupBC.Rows[ind][6] = double.Parse(dtWorkGroupBC.Rows[ind][6].ToString()) + double.Parse(dtBC.Rows[ind][6].ToString());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (emplIdsWC.Length > 0)
                                    {
                                        System.Data.DataTable dtWC = new System.Data.DataTable(workingUnit.WorkingUnitID + " WC");
                                        dtWC.Columns.Add("1", typeof(string));
                                        dtWC.Columns.Add("   ", typeof(string));
                                        dtWC.Columns.Add("Direct", typeof(string));
                                        dtWC.Columns.Add("%  ", typeof(string));
                                        dtWC.Columns.Add("Indirect", typeof(string));
                                        dtWC.Columns.Add("%   ", typeof(string));
                                        dtWC.Columns.Add("Total", typeof(string));
                                        dtWC.Columns.Add(" %   ", typeof(string));

                                        dictEmplTypes = Misc.emplBranch(dbConnection, emplIdsWC, company);
                                        string emplIdsWCDirect = "";
                                        string emplIdsWCIndirect = "";
                                        foreach (KeyValuePair<string, string> pair in dictEmplTypes)
                                        {
                                            if (pair.Value.Equals("A"))
                                                emplIdsWCDirect += pair.Key + ",";
                                            else
                                                emplIdsWCIndirect += pair.Key + ",";
                                        }

                                        if (emplIdsWCDirect.Length > 0)
                                            emplIdsWCDirect = emplIdsWCDirect.Remove(emplIdsWCDirect.LastIndexOf(','));

                                        if (emplIdsWCIndirect.Length > 0)
                                            emplIdsWCIndirect = emplIdsWCIndirect.Remove(emplIdsWCIndirect.LastIndexOf(','));

                                        List<IOPairProcessedTO> IOPairListDirect = new List<IOPairProcessedTO>();
                                        List<IOPairProcessedTO> IOPairListIndirect = new List<IOPairProcessedTO>();

                                        if (dbConnection == null)
                                            ioPairProc = new IOPairProcessed();
                                        else
                                            ioPairProc = new IOPairProcessed(dbConnection);
                                        if (emplIdsWCDirect.Length > 0)
                                            IOPairListDirect = ioPairProc.SearchAllPairsForEmpl(emplIdsWCDirect, datesList, "");
                                        else
                                            IOPairListDirect = new List<IOPairProcessedTO>();

                                        if (dbConnection == null)
                                            ioPairProc = new IOPairProcessed();
                                        else
                                            ioPairProc = new IOPairProcessed(dbConnection);

                                        if (emplIdsWCIndirect.Length > 0)
                                            IOPairListIndirect = ioPairProc.SearchAllPairsForEmpl(emplIdsWCIndirect, datesList, "");
                                        else
                                            IOPairListIndirect = new List<IOPairProcessedTO>();

                                        numOfEmployees = emplIdsWC.Split(',').Length;
                                        numEmplWorkgroupIndirect += numOfEmployees;
                                        numEmplCostIndirect += numOfEmployees;
                                        numEmplPlantIndirect += numOfEmployees;
                                        numEmplCompanyIndirect += numOfEmployees;
                                        double totalDays = numOfEmployees * timeSpan.TotalDays;

                                        if (IOPairListDirect.Count > 0 || IOPairListIndirect.Count > 0)
                                        {

                                            populateDataTable400New(dbConnection, dtWC, datesList, companyWU, IOPairListDirect, IOPairListIndirect, emplIdsWCDirect, emplIdsWCIndirect, "WC", workingUnit, "", passTypesDictionaryAll);

                                            DataRow rowEmptyFooter = dtWC.NewRow();
                                            dtWC.Rows.Add(rowEmptyFooter);
                                            DataRow footer = dtWC.NewRow();
                                            footer[1] = "N° Employees: " + numOfEmployees;
                                            footer[2] = "Total days: ";
                                            footer[3] = totalDays;
                                            footer[4] = "Calendar days: ";
                                            footer[5] = timeSpan.TotalDays;
                                            dtWC.Rows.Add(footer);

                                            ds.Tables.Add(dtWC);
                                            ds.AcceptChanges();


                                            i++;
                                            if (i == 1)
                                            {
                                                string ute = "";
                                                string workGroup = "";
                                                string costCenterString = "";
                                                //string plantString = "";

                                                DataRow rowHeader1 = dtWorkGroupWC.NewRow();

                                                rowHeader1[1] = companyWU.Description;

                                                rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                                rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                                //rowHeader1[8] = "page: " + page;
                                                dtWorkGroupWC.Rows.Add(rowHeader1);

                                                DataRow rowHeader2 = dtWorkGroupWC.NewRow();
                                                rowHeader2[1] = "   Absenteeism industrial relation";
                                                rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                                rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                                                dtWorkGroupWC.Rows.Add(rowHeader2);

                                                if (dbConnection == null)
                                                    wu = new WorkingUnit();
                                                else
                                                    wu = new WorkingUnit(dbConnection);

                                                WorkingUnitTO tempWU = wu.FindWU(workingUnit.WorkingUnitID);
                                                ute = tempWU.Code.Trim();

                                                // get workshop (parent of UTE)
                                                wu.WUTO = tempWU;
                                                tempWU = wu.getParentWorkingUnit();
                                                workGroup = tempWU.Code.Trim();

                                                // get cost centar
                                                wu.WUTO = tempWU;
                                                tempWU = wu.getParentWorkingUnit();
                                                costCenterString = tempWU.Code.Trim();

                                                // get plant
                                                wu.WUTO = tempWU;
                                                tempWU = wu.getParentWorkingUnit();
                                                plantString = tempWU.Code.Trim();
                                                DataRow rowHeader3 = dtWorkGroupWC.NewRow();

                                                rowHeader3[1] = "Qualify:   " + "WC";
                                                dtWorkGroupWC.Rows.Add(rowHeader3);

                                                DataRow rowPlant = dtWorkGroupWC.NewRow();
                                                rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
                                                dtWorkGroupWC.Rows.Add(rowPlant);
                                                DataRow rowWorkgroup = dtWorkGroupWC.NewRow();
                                                rowWorkgroup[1] = "Workgroup: " + workGroup + " UTE: " + "xx";
                                                dtWorkGroupWC.Rows.Add(rowWorkgroup);
                                                DataRow rowEmpty = dtWorkGroupWC.NewRow();
                                                dtWorkGroupWC.Rows.Add(rowEmpty);

                                                DataRow rowColumns = dtWorkGroupWC.NewRow();
                                                rowColumns[2] = "Direct";
                                                rowColumns[3] = "%";
                                                rowColumns[4] = "Indirect";
                                                rowColumns[5] = "%";
                                                rowColumns[6] = "Total";
                                                rowColumns[7] = "%";
                                                dtWorkGroupWC.Rows.Add(rowColumns);
                                                for (int ind = 7; ind < dtWC.Rows.Count - 2; ind++)
                                                {
                                                    DataRow row = dtWorkGroupWC.NewRow();

                                                    row[1] = dtWC.Rows[ind][1];
                                                    row[2] = dtWC.Rows[ind][2];
                                                    row[4] = dtWC.Rows[ind][4];
                                                    row[6] = dtWC.Rows[ind][6];
                                                    dtWorkGroupWC.Rows.Add(row);
                                                    dtWorkGroupWC.AcceptChanges();
                                                }
                                            }
                                            else
                                            {
                                                for (int ind = 7; ind < dtWC.Rows.Count - 2; ind++)
                                                {
                                                    if (ind != 22)
                                                    {

                                                        dtWorkGroupWC.Rows[ind][1] = dtWC.Rows[ind][1];
                                                        dtWorkGroupWC.Rows[ind][2] = double.Parse(dtWorkGroupWC.Rows[ind][2].ToString()) + double.Parse(dtWC.Rows[ind][2].ToString());
                                                        dtWorkGroupWC.Rows[ind][4] = double.Parse(dtWorkGroupWC.Rows[ind][4].ToString()) + double.Parse(dtWC.Rows[ind][4].ToString());
                                                        dtWorkGroupWC.Rows[ind][6] = double.Parse(dtWorkGroupWC.Rows[ind][6].ToString()) + double.Parse(dtWC.Rows[ind][6].ToString());

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            if (dbConnection == null)
                                wu = new WorkingUnit();
                            else
                                wu = new WorkingUnit(dbConnection);
                            List<WorkingUnitTO> oneWorkShop = new List<WorkingUnitTO>();
                            oneWorkShop.Add(workshop);
                            List<WorkingUnitTO> workShopAndUTE = wu.FindAllChildren(oneWorkShop);

                            if (dtWorkGroupBC.Rows.Count > 0)
                            {
                                double totalDD = 0;
                                double totalID = 0;
                                double totalDDSum = 0;
                                double totalIDSum = 0;
                                for (int ind = 7; ind < 22; ind++)
                                {
                                    totalDD = double.Parse(dtWorkGroupBC.Rows[ind][2].ToString());
                                    totalID = double.Parse(dtWorkGroupBC.Rows[ind][4].ToString());
                                    totalDDSum += totalDD;
                                    totalIDSum += totalID;
                                }
                                double totaltotal = totalDDSum + totalIDSum;
                                for (int ind = 7; ind < 22; ind++)
                                {
                                    Misc.percentage(dtWorkGroupBC.Rows[ind], totalDDSum, totalIDSum);

                                    dtWorkGroupBC.Rows[ind][7] = double.Parse(dtWorkGroupBC.Rows[ind][6].ToString()) * 100 / totaltotal;
                                    dtWorkGroupBC.AcceptChanges();
                                }
                                double totalPerDD = 0;
                                double totalPerID = 0;
                                double totalPerSum = 0;
                                for (int ind = 7; ind < 22; ind++)
                                {
                                    totalPerDD += double.Parse(dtWorkGroupBC.Rows[ind][3].ToString());
                                    totalPerID += double.Parse(dtWorkGroupBC.Rows[ind][5].ToString());
                                    totalPerSum += double.Parse(dtWorkGroupBC.Rows[ind][7].ToString());
                                }
                                dtWorkGroupBC.Rows[23][3] = totalPerDD;
                                dtWorkGroupBC.Rows[23][5] = totalPerID;
                                dtWorkGroupBC.Rows[23][7] = totalPerSum;

                                dtWorkGroupBC.Rows[24][3] = 0;
                                dtWorkGroupBC.Rows[24][5] = 0;
                                dtWorkGroupBC.Rows[24][7] = 0;

                                dtWorkGroupBC.Rows[25][3] = "  ";
                                dtWorkGroupBC.Rows[25][5] = "  ";
                                dtWorkGroupBC.Rows[25][7] = "  ";
                                numOfEmployees = numEmplWorkgroupDirect;
                                double totalDays = numOfEmployees * timeSpan.TotalDays;
                                DataRow rowEmptyFooter = dtWorkGroupBC.NewRow();
                                dtWorkGroupBC.Rows.Add(rowEmptyFooter);

                                DataRow footer = dtWorkGroupBC.NewRow();
                                footer[1] = "N° Employees: " + numOfEmployees;
                                footer[2] = "Total days: ";
                                footer[3] = totalDays;
                                footer[4] = "Calendar days: ";
                                footer[5] = timeSpan.TotalDays;
                                dtWorkGroupBC.Rows.Add(footer);

                                ds.Tables.Add(dtWorkGroupBC);
                                ds.AcceptChanges();

                                dCost++;
                                if (dCost == 1)
                                {
                                    string ute = "";
                                    string workGroup = "";
                                    string costCenterString = "";
                                    DataRow rowHeader1 = dtCostCenterBC.NewRow();

                                    rowHeader1[1] = companyWU.Description;

                                    rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                    rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                    dtCostCenterBC.Rows.Add(rowHeader1);

                                    DataRow rowHeader2 = dtCostCenterBC.NewRow();
                                    rowHeader2[1] = "   Absenteeism industrial relation";
                                    rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                    rowHeader2[7] = datesList[0].ToString("MMMM yyyy");

                                    dtCostCenterBC.Rows.Add(rowHeader2);
                                    if (dbConnection == null)
                                        wu = new WorkingUnit();
                                    else
                                        wu = new WorkingUnit(dbConnection);

                                    WorkingUnitTO tempWU = wu.FindWU(listUTE[0].WorkingUnitID);
                                    ute = tempWU.Code.Trim();

                                    // get workshop (parent of UTE)
                                    wu.WUTO = tempWU;
                                    tempWU = wu.getParentWorkingUnit();
                                    workGroup = tempWU.Code.Trim();

                                    // get cost centar
                                    wu.WUTO = tempWU;
                                    tempWU = wu.getParentWorkingUnit();
                                    costCenterString = tempWU.Code.Trim();

                                    // get plant
                                    wu.WUTO = tempWU;
                                    tempWU = wu.getParentWorkingUnit();
                                    plantString = tempWU.Code.Trim();

                                    DataRow rowHeader3 = dtCostCenterBC.NewRow();
                                    rowHeader3[1] = "Qualify:   " + "BC";
                                    dtCostCenterBC.Rows.Add(rowHeader3);

                                    DataRow rowPlant = dtCostCenterBC.NewRow();
                                    rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
                                    dtCostCenterBC.Rows.Add(rowPlant);

                                    DataRow rowWorkgroup = dtCostCenterBC.NewRow();
                                    rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
                                    dtCostCenterBC.Rows.Add(rowWorkgroup);

                                    DataRow rowEmpty = dtCostCenterBC.NewRow();
                                    dtCostCenterBC.Rows.Add(rowEmpty);

                                    DataRow rowColumns = dtCostCenterBC.NewRow();
                                    rowColumns[2] = "Direct";
                                    rowColumns[3] = "%";
                                    rowColumns[4] = "Indirect";
                                    rowColumns[5] = "%";
                                    rowColumns[6] = "Total";
                                    rowColumns[7] = "%";
                                    dtCostCenterBC.Rows.Add(rowColumns);

                                    for (int ind = 7; ind < dtWorkGroupBC.Rows.Count - 2; ind++)
                                    {
                                        DataRow row = dtCostCenterBC.NewRow();

                                        row[1] = dtWorkGroupBC.Rows[ind][1];
                                        row[2] = dtWorkGroupBC.Rows[ind][2];
                                        row[4] = dtWorkGroupBC.Rows[ind][4];
                                        row[6] = dtWorkGroupBC.Rows[ind][6];
                                        dtCostCenterBC.Rows.Add(row);
                                        dtCostCenterBC.AcceptChanges();

                                    }
                                }
                                else
                                {
                                    for (int ind = 7; ind < dtWorkGroupBC.Rows.Count - 2; ind++)
                                    {
                                        if (ind != 22)
                                        {

                                            dtCostCenterBC.Rows[ind][1] = dtWorkGroupBC.Rows[ind][1];
                                            dtCostCenterBC.Rows[ind][2] = double.Parse(dtCostCenterBC.Rows[ind][2].ToString()) + double.Parse(dtWorkGroupBC.Rows[ind][2].ToString());
                                            dtCostCenterBC.Rows[ind][4] = double.Parse(dtCostCenterBC.Rows[ind][4].ToString()) + double.Parse(dtWorkGroupBC.Rows[ind][4].ToString());
                                            dtCostCenterBC.Rows[ind][6] = double.Parse(dtCostCenterBC.Rows[ind][6].ToString()) + double.Parse(dtWorkGroupBC.Rows[ind][6].ToString());
                                        }
                                    }
                                }
                            }
                            if (dtWorkGroupWC.Rows.Count > 0)
                            {
                                double totalDD = 0;
                                double totalID = 0;
                                double totalDDSum = 0;
                                double totalIDSum = 0;
                                for (int ind = 7; ind < 22; ind++)
                                {
                                    totalDD = double.Parse(dtWorkGroupWC.Rows[ind][2].ToString());
                                    totalID = double.Parse(dtWorkGroupWC.Rows[ind][4].ToString());
                                    totalDDSum += totalDD;
                                    totalIDSum += totalID;
                                }
                                double totaltotal = totalDDSum + totalIDSum;
                                for (int ind = 7; ind < 22; ind++)
                                {
                                    Misc.percentage(dtWorkGroupWC.Rows[ind], totalDDSum, totalIDSum);

                                    dtWorkGroupWC.Rows[ind][7] = double.Parse(dtWorkGroupWC.Rows[ind][6].ToString()) * 100 / totaltotal;
                                    dtWorkGroupWC.AcceptChanges();
                                }
                                double totalPerDD = 0;
                                double totalPerID = 0;
                                double totalPerSum = 0;
                                for (int ind = 7; ind < 22; ind++)
                                {
                                    totalPerDD += double.Parse(dtWorkGroupWC.Rows[ind][3].ToString());
                                    totalPerID += double.Parse(dtWorkGroupWC.Rows[ind][5].ToString());
                                    totalPerSum += double.Parse(dtWorkGroupWC.Rows[ind][7].ToString());
                                }
                                dtWorkGroupWC.Rows[23][3] = totalPerDD;
                                dtWorkGroupWC.Rows[23][5] = totalPerID;
                                dtWorkGroupWC.Rows[23][7] = totalPerSum;

                                dtWorkGroupWC.Rows[24][3] = 0;
                                dtWorkGroupWC.Rows[24][5] = 0;
                                dtWorkGroupWC.Rows[24][7] = 0;

                                dtWorkGroupWC.Rows[25][3] = "  ";
                                dtWorkGroupWC.Rows[25][5] = "  ";
                                dtWorkGroupWC.Rows[25][7] = "  ";
                                numOfEmployees = numEmplWorkgroupIndirect;
                                double totalDays = numOfEmployees * timeSpan.TotalDays;

                                DataRow rowEmptyFooter = dtWorkGroupWC.NewRow();
                                dtWorkGroupWC.Rows.Add(rowEmptyFooter);

                                DataRow footer = dtWorkGroupWC.NewRow();
                                footer[1] = "N° Employees: " + numOfEmployees;
                                footer[2] = "Total days: ";
                                footer[3] = totalDays;
                                footer[4] = "Calendar days: ";
                                footer[5] = timeSpan.TotalDays;

                                dtWorkGroupWC.Rows.Add(footer);
                                ds.Tables.Add(dtWorkGroupWC);
                                ds.AcceptChanges();

                                iCost++;
                                if (iCost == 1)
                                {
                                    string ute = "";
                                    string workGroup = "";
                                    string costCenterString = "";
                                    //string plantString = "";
                                    DataRow rowHeader1 = dtCostCenterWC.NewRow();

                                    rowHeader1[1] = companyWU.Description;

                                    rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                    rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                    //rowHeader1[8] = "page: " + page;
                                    dtCostCenterWC.Rows.Add(rowHeader1);

                                    DataRow rowHeader2 = dtCostCenterWC.NewRow();
                                    rowHeader2[1] = "   Absenteeism industrial relation";
                                    rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                    rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                                    dtCostCenterWC.Rows.Add(rowHeader2);
                                    if (dbConnection == null)
                                        wu = new WorkingUnit();
                                    else
                                        wu = new WorkingUnit(dbConnection);

                                    WorkingUnitTO tempWU = wu.FindWU(listUTE[0].WorkingUnitID);
                                    ute = tempWU.Code.Trim();

                                    // get workshop (parent of UTE)
                                    wu.WUTO = tempWU;
                                    tempWU = wu.getParentWorkingUnit();
                                    workGroup = tempWU.Code.Trim();

                                    // get cost centar
                                    wu.WUTO = tempWU;
                                    tempWU = wu.getParentWorkingUnit();
                                    costCenterString = tempWU.Code.Trim();

                                    // get plant
                                    wu.WUTO = tempWU;
                                    tempWU = wu.getParentWorkingUnit();
                                    plantString = tempWU.Code.Trim();

                                    DataRow rowHeader3 = dtCostCenterWC.NewRow();
                                    rowHeader3[1] = "Qualify:   " + "WC";
                                    dtCostCenterWC.Rows.Add(rowHeader3);

                                    DataRow rowPlant = dtCostCenterWC.NewRow();
                                    rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
                                    dtCostCenterWC.Rows.Add(rowPlant);

                                    DataRow rowWorkgroup = dtCostCenterWC.NewRow();
                                    rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
                                    dtCostCenterWC.Rows.Add(rowWorkgroup);

                                    DataRow rowEmpty = dtCostCenterWC.NewRow();
                                    dtCostCenterWC.Rows.Add(rowEmpty);

                                    DataRow rowColumns = dtCostCenterWC.NewRow();
                                    rowColumns[2] = "Direct";
                                    rowColumns[3] = "%";
                                    rowColumns[4] = "Indirect";
                                    rowColumns[5] = "%";
                                    rowColumns[6] = "Total";
                                    rowColumns[7] = "%";
                                    dtCostCenterWC.Rows.Add(rowColumns);

                                    for (int ind = 7; ind < dtWorkGroupWC.Rows.Count - 2; ind++)
                                    {
                                        DataRow row = dtCostCenterWC.NewRow();

                                        row[1] = dtWorkGroupWC.Rows[ind][1];
                                        row[2] = dtWorkGroupWC.Rows[ind][2];
                                        row[4] = dtWorkGroupWC.Rows[ind][4];
                                        row[6] = dtWorkGroupWC.Rows[ind][6];

                                        dtCostCenterWC.Rows.Add(row);
                                        dtCostCenterWC.AcceptChanges();
                                    }
                                }
                                else
                                {
                                    for (int ind = 7; ind < dtWorkGroupWC.Rows.Count - 2; ind++)
                                    {
                                        if (ind != 22)
                                        {

                                            dtCostCenterWC.Rows[ind][1] = dtWorkGroupWC.Rows[ind][1];
                                            dtCostCenterWC.Rows[ind][2] = double.Parse(dtCostCenterWC.Rows[ind][2].ToString()) + double.Parse(dtWorkGroupWC.Rows[ind][2].ToString());
                                            dtCostCenterWC.Rows[ind][4] = double.Parse(dtCostCenterWC.Rows[ind][4].ToString()) + double.Parse(dtWorkGroupWC.Rows[ind][4].ToString());
                                            dtCostCenterWC.Rows[ind][6] = double.Parse(dtCostCenterWC.Rows[ind][6].ToString()) + double.Parse(dtWorkGroupWC.Rows[ind][6].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        if (dbConnection == null)
                            wu = new WorkingUnit();
                        else
                            wu = new WorkingUnit(dbConnection);
                        List<WorkingUnitTO> oneCostCenter = new List<WorkingUnitTO>();
                        oneCostCenter.Add(costCenter);
                        List<WorkingUnitTO> CostCenterAndWorkShop = wu.FindAllChildren(oneCostCenter);

                        if (dtCostCenterBC.Rows.Count > 0)
                        {
                            double totalDD = 0;
                            double totalID = 0;
                            double totalDDSum = 0;
                            double totalIDSum = 0;
                            for (int i = 7; i < 22; i++)
                            {
                                totalDD = double.Parse(dtCostCenterBC.Rows[i][2].ToString());
                                totalID = double.Parse(dtCostCenterBC.Rows[i][4].ToString());
                                totalDDSum += totalDD;
                                totalIDSum += totalID;

                            }
                            double totaltotal = totalDDSum + totalIDSum;
                            for (int i = 7; i < 22; i++)
                            {
                                Misc.percentage(dtCostCenterBC.Rows[i], totalDDSum, totalIDSum);

                                dtCostCenterBC.Rows[i][7] = double.Parse(dtCostCenterBC.Rows[i][6].ToString()) * 100 / totaltotal;
                                dtCostCenterBC.AcceptChanges();
                            }
                            double totalPerDD = 0;
                            double totalPerID = 0;
                            double totalPerSum = 0;
                            for (int i = 7; i < 22; i++)
                            {
                                totalPerDD += double.Parse(dtCostCenterBC.Rows[i][3].ToString());
                                totalPerID += double.Parse(dtCostCenterBC.Rows[i][5].ToString());
                                totalPerSum += double.Parse(dtCostCenterBC.Rows[i][7].ToString());
                            }
                            dtCostCenterBC.Rows[23][3] = totalPerDD;
                            dtCostCenterBC.Rows[23][5] = totalPerID;
                            dtCostCenterBC.Rows[23][7] = totalPerSum;

                            dtCostCenterBC.Rows[24][3] = 0;
                            dtCostCenterBC.Rows[24][5] = 0;
                            dtCostCenterBC.Rows[24][7] = 0;

                            dtCostCenterBC.Rows[25][3] = "  ";
                            dtCostCenterBC.Rows[25][5] = "  ";
                            dtCostCenterBC.Rows[25][7] = "  ";
                            numOfEmployees = numEmplCostDirect;
                            double totalDays = numOfEmployees * timeSpan.TotalDays;
                            DataRow rowEmptyFooter = dtCostCenterBC.NewRow();
                            dtCostCenterBC.Rows.Add(rowEmptyFooter);

                            DataRow footer = dtCostCenterBC.NewRow();
                            footer[1] = "N° Employees: " + numOfEmployees;
                            footer[2] = "Total days: ";
                            footer[3] = totalDays;
                            footer[4] = "Calendar days: ";
                            footer[5] = timeSpan.TotalDays;
                            dtCostCenterBC.Rows.Add(footer);

                            ds.Tables.Add(dtCostCenterBC);
                            ds.AcceptChanges();

                            dPlant++;
                            if (dPlant == 1)
                            {
                                DataRow rowHeader1 = dtPlantBC.NewRow();
                                
                                rowHeader1[1] = companyWU.Description;

                                rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                //rowHeader1[8] = "page: " + page;
                                dtPlantBC.Rows.Add(rowHeader1);

                                DataRow rowHeader2 = dtPlantBC.NewRow();
                                rowHeader2[1] = "   Absenteeism industrial relation";
                                rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                                dtPlantBC.Rows.Add(rowHeader2);
                                
                                DataRow rowHeader3 = dtPlantBC.NewRow();
                                rowHeader3[1] = "Qualify:   " + "BC";
                                dtPlantBC.Rows.Add(rowHeader3);

                                DataRow rowPlant = dtPlantBC.NewRow();
                                rowPlant[1] = "Plant:    " + plantString + "    Cost center: " + "xxxx";
                                dtPlantBC.Rows.Add(rowPlant);

                                DataRow rowWorkgroup = dtPlantBC.NewRow();
                                rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
                                dtPlantBC.Rows.Add(rowWorkgroup);

                                DataRow rowEmpty = dtPlantBC.NewRow();
                                dtPlantBC.Rows.Add(rowEmpty);

                                DataRow rowColumns = dtPlantBC.NewRow();
                                rowColumns[2] = "Direct";
                                rowColumns[3] = "%";
                                rowColumns[4] = "Indirect";
                                rowColumns[5] = "%";
                                rowColumns[6] = "Total";
                                rowColumns[7] = "%";
                                dtPlantBC.Rows.Add(rowColumns);

                                for (int ind = 7; ind < dtCostCenterBC.Rows.Count - 2; ind++)
                                {
                                    DataRow row = dtPlantBC.NewRow();

                                    row[1] = dtCostCenterBC.Rows[ind][1];
                                    row[2] = dtCostCenterBC.Rows[ind][2];
                                    row[4] = dtCostCenterBC.Rows[ind][4];
                                    row[6] = dtCostCenterBC.Rows[ind][6];
                                    dtPlantBC.Rows.Add(row);
                                    dtPlantBC.AcceptChanges();

                                }
                            }
                            else
                            {
                                for (int ind = 7; ind < dtCostCenterBC.Rows.Count - 2; ind++)
                                {
                                    if (ind != 22)
                                    {

                                        dtPlantBC.Rows[ind][1] = dtCostCenterBC.Rows[ind][1];
                                        dtPlantBC.Rows[ind][2] = double.Parse(dtPlantBC.Rows[ind][2].ToString()) + double.Parse(dtCostCenterBC.Rows[ind][2].ToString());
                                        dtPlantBC.Rows[ind][4] = double.Parse(dtPlantBC.Rows[ind][4].ToString()) + double.Parse(dtCostCenterBC.Rows[ind][4].ToString());
                                        dtPlantBC.Rows[ind][6] = double.Parse(dtPlantBC.Rows[ind][6].ToString()) + double.Parse(dtCostCenterBC.Rows[ind][6].ToString());
                                    }
                                }
                            }
                        }
                        if (dtCostCenterWC.Rows.Count > 0)
                        {
                            double totalDD = 0;
                            double totalID = 0;
                            double totalDDSum = 0;
                            double totalIDSum = 0;
                            for (int i = 7; i < 22; i++)
                            {
                                totalDD = double.Parse(dtCostCenterWC.Rows[i][2].ToString());
                                totalID = double.Parse(dtCostCenterWC.Rows[i][4].ToString());
                                totalDDSum += totalDD;
                                totalIDSum += totalID;
                            }
                            double totaltotal = totalDDSum + totalIDSum;
                            for (int i = 7; i < 22; i++)
                            {
                                Misc.percentage(dtCostCenterWC.Rows[i], totalDDSum, totalIDSum);

                                dtCostCenterWC.Rows[i][7] = double.Parse(dtCostCenterWC.Rows[i][6].ToString()) * 100 / totaltotal;
                                dtCostCenterWC.AcceptChanges();
                            }
                            double totalPerDD = 0;
                            double totalPerID = 0;
                            double totalPerSum = 0;
                            for (int i = 7; i < 22; i++)
                            {
                                totalPerDD += double.Parse(dtCostCenterWC.Rows[i][3].ToString());
                                totalPerID += double.Parse(dtCostCenterWC.Rows[i][5].ToString());
                                totalPerSum += double.Parse(dtCostCenterWC.Rows[i][7].ToString());
                            }
                            dtCostCenterWC.Rows[23][3] = totalPerDD;
                            dtCostCenterWC.Rows[23][5] = totalPerID;
                            dtCostCenterWC.Rows[23][7] = totalPerSum;

                            dtCostCenterWC.Rows[24][3] = 0;
                            dtCostCenterWC.Rows[24][5] = 0;
                            dtCostCenterWC.Rows[24][7] = 0;

                            dtCostCenterWC.Rows[25][3] = "  ";
                            dtCostCenterWC.Rows[25][5] = "  ";
                            dtCostCenterWC.Rows[25][7] = "  ";

                            numOfEmployees = numEmplCostIndirect;
                            double totalDays = numOfEmployees * timeSpan.TotalDays;

                            DataRow rowEmptyFooter = dtCostCenterWC.NewRow();
                            dtCostCenterWC.Rows.Add(rowEmptyFooter);

                            DataRow footer = dtCostCenterWC.NewRow();
                            footer[1] = "N° Employees: " + numOfEmployees;
                            footer[2] = "Total days: ";
                            footer[3] = totalDays;
                            footer[4] = "Calendar days: ";
                            footer[5] = timeSpan.TotalDays;
                            dtCostCenterWC.Rows.Add(footer);

                            ds.Tables.Add(dtCostCenterWC);
                            ds.AcceptChanges();

                            iPlant++;
                            if (iPlant == 1)
                            {
                                DataRow rowHeader1 = dtPlantWC.NewRow();

                                rowHeader1[1] = companyWU.Description;

                                rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                //rowHeader1[8] = "page: " + page;
                                dtPlantWC.Rows.Add(rowHeader1);

                                DataRow rowHeader2 = dtPlantWC.NewRow();
                                rowHeader2[1] = "   Absenteeism industrial relation";
                                rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                                dtPlantWC.Rows.Add(rowHeader2);

                                DataRow rowHeader3 = dtPlantWC.NewRow();
                                rowHeader3[1] = "Qualify:   " + "WC";
                                dtPlantWC.Rows.Add(rowHeader3);

                                DataRow rowPlant = dtPlantWC.NewRow();
                                rowPlant[1] = "Plant:    " + plantString + "    Cost center: " + "xxxx";
                                dtPlantWC.Rows.Add(rowPlant);

                                DataRow rowWorkgroup = dtPlantWC.NewRow();
                                rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
                                dtPlantWC.Rows.Add(rowWorkgroup);

                                DataRow rowEmpty = dtPlantWC.NewRow();
                                dtPlantWC.Rows.Add(rowEmpty);

                                DataRow rowColumns = dtPlantWC.NewRow();
                                rowColumns[2] = "Direct";
                                rowColumns[3] = "%";
                                rowColumns[4] = "Indirect";
                                rowColumns[5] = "%";
                                rowColumns[6] = "Total";
                                rowColumns[7] = "%";
                                dtPlantWC.Rows.Add(rowColumns);

                                for (int ind = 7; ind < dtCostCenterWC.Rows.Count - 2; ind++)
                                {
                                    DataRow row = dtPlantWC.NewRow();

                                    row[1] = dtCostCenterWC.Rows[ind][1];
                                    row[2] = dtCostCenterWC.Rows[ind][2];
                                    row[4] = dtCostCenterWC.Rows[ind][4];
                                    row[6] = dtCostCenterWC.Rows[ind][6];
                                    dtPlantWC.Rows.Add(row);
                                    dtPlantWC.AcceptChanges();
                                }
                            }
                            else
                            {
                                for (int ind = 7; ind < dtCostCenterWC.Rows.Count - 2; ind++)
                                {
                                    if (ind != 22)
                                    {
                                        dtPlantWC.Rows[ind][1] = dtCostCenterWC.Rows[ind][1];
                                        dtPlantWC.Rows[ind][2] = double.Parse(dtPlantWC.Rows[ind][2].ToString()) + double.Parse(dtCostCenterWC.Rows[ind][2].ToString());
                                        dtPlantWC.Rows[ind][4] = double.Parse(dtPlantWC.Rows[ind][4].ToString()) + double.Parse(dtCostCenterWC.Rows[ind][4].ToString());
                                        dtPlantWC.Rows[ind][6] = double.Parse(dtPlantWC.Rows[ind][6].ToString()) + double.Parse(dtCostCenterWC.Rows[ind][6].ToString());
                                    }
                                }
                            }
                        }
                    }
                    if (dbConnection == null)
                        wu = new WorkingUnit();
                    else
                        wu = new WorkingUnit(dbConnection);
                    List<WorkingUnitTO> onePlant = new List<WorkingUnitTO>();
                    onePlant.Add(plant);
                    List<WorkingUnitTO> PlantAndCostCentre = wu.FindAllChildren(onePlant);

                    if (dtPlantBC.Rows.Count > 0)
                    {
                        double totalDD = 0;
                        double totalID = 0;
                        double totalDDSum = 0;
                        double totalIDSum = 0;
                        for (int i = 7; i < 22; i++)
                        {
                            totalDD = double.Parse(dtPlantBC.Rows[i][2].ToString());
                            totalID = double.Parse(dtPlantBC.Rows[i][4].ToString());
                            totalDDSum += totalDD;
                            totalIDSum += totalID;
                        }
                        double totaltotal = totalDDSum + totalIDSum;
                        for (int i = 7; i < 22; i++)
                        {
                            Misc.percentage(dtPlantBC.Rows[i], totalDDSum, totalIDSum);

                            dtPlantBC.Rows[i][7] = double.Parse(dtPlantBC.Rows[i][6].ToString()) * 100 / totaltotal;
                            dtPlantBC.AcceptChanges();
                        }
                        double totalPerDD = 0;
                        double totalPerID = 0;
                        double totalPerSum = 0;
                        for (int i = 7; i < 22; i++)
                        {
                            totalPerDD += double.Parse(dtPlantBC.Rows[i][3].ToString());
                            totalPerID += double.Parse(dtPlantBC.Rows[i][5].ToString());
                            totalPerSum += double.Parse(dtPlantBC.Rows[i][7].ToString());
                        }
                        dtPlantBC.Rows[23][3] = totalPerDD;
                        dtPlantBC.Rows[23][5] = totalPerID;
                        dtPlantBC.Rows[23][7] = totalPerSum;

                        dtPlantBC.Rows[24][3] = 0;
                        dtPlantBC.Rows[24][5] = 0;
                        dtPlantBC.Rows[24][7] = 0;

                        dtPlantBC.Rows[25][3] = "  ";
                        dtPlantBC.Rows[25][5] = "  ";
                        dtPlantBC.Rows[25][7] = "  ";

                        numOfEmployees = numEmplPlantDirect;
                        double totalDays = numOfEmployees * timeSpan.TotalDays;
                        DataRow rowEmptyFooter = dtPlantBC.NewRow();
                        dtPlantBC.Rows.Add(rowEmptyFooter);

                        DataRow footer = dtPlantBC.NewRow();
                        footer[1] = "N° Employees: " + numOfEmployees;
                        footer[2] = "Total days: ";
                        footer[3] = totalDays;
                        footer[4] = "Calendar days: ";
                        footer[5] = timeSpan.TotalDays;
                        dtPlantBC.Rows.Add(footer);

                        ds.Tables.Add(dtPlantBC);
                        ds.AcceptChanges();

                        dcompany++;
                        if (dcompany == 1)
                        {
                            DataRow rowHeader1 = dtCompanyBC.NewRow();
                            
                            rowHeader1[1] = companyWU.Description;

                            rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                            rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                            //rowHeader1[8] = "page: " + page;
                            dtCompanyBC.Rows.Add(rowHeader1);

                            DataRow rowHeader2 = dtCompanyBC.NewRow();
                            rowHeader2[1] = "   Absenteeism industrial relation";
                            rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                            rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                            dtCompanyBC.Rows.Add(rowHeader2);

                            DataRow rowHeader3 = dtCompanyBC.NewRow();
                            rowHeader3[1] = "Qualify:   " + "BC";
                            dtCompanyBC.Rows.Add(rowHeader3);

                            DataRow rowPlant = dtCompanyBC.NewRow();
                            rowPlant[1] = "Company report";
                            dtCompanyBC.Rows.Add(rowPlant);

                            DataRow rowWorkgroup = dtCompanyBC.NewRow();
                            dtCompanyBC.Rows.Add(rowWorkgroup);

                            DataRow rowEmpty = dtCompanyBC.NewRow();
                            dtCompanyBC.Rows.Add(rowEmpty);

                            DataRow rowColumns = dtCompanyBC.NewRow();
                            rowColumns[2] = "Direct";
                            rowColumns[3] = "%";
                            rowColumns[4] = "Indirect";
                            rowColumns[5] = "%";
                            rowColumns[6] = "Total";
                            rowColumns[7] = "%";
                            dtCompanyBC.Rows.Add(rowColumns);

                            for (int ind = 7; ind < dtPlantBC.Rows.Count - 2; ind++)
                            {
                                DataRow row = dtCompanyBC.NewRow();

                                row[1] = dtPlantBC.Rows[ind][1];
                                row[2] = dtPlantBC.Rows[ind][2];
                                row[4] = dtPlantBC.Rows[ind][4];
                                row[6] = dtPlantBC.Rows[ind][6];
                                dtCompanyBC.Rows.Add(row);
                                dtCompanyBC.AcceptChanges();

                            }
                        }
                        else
                        {
                            for (int ind = 7; ind < dtPlantBC.Rows.Count - 2; ind++)
                            {
                                if (ind != 22)
                                {
                                    dtCompanyBC.Rows[ind][1] = dtPlantBC.Rows[ind][1];
                                    dtCompanyBC.Rows[ind][2] = double.Parse(dtCompanyBC.Rows[ind][2].ToString()) + double.Parse(dtPlantBC.Rows[ind][2].ToString());
                                    dtCompanyBC.Rows[ind][4] = double.Parse(dtCompanyBC.Rows[ind][4].ToString()) + double.Parse(dtPlantBC.Rows[ind][4].ToString());
                                    dtCompanyBC.Rows[ind][6] = double.Parse(dtCompanyBC.Rows[ind][6].ToString()) + double.Parse(dtPlantBC.Rows[ind][6].ToString());

                                }
                            }
                        }
                    }
                    if (dtPlantWC.Rows.Count > 0)
                    {
                        double totalDD = 0;
                        double totalID = 0;
                        double totalDDSum = 0;
                        double totalIDSum = 0;
                        for (int i = 7; i < 22; i++)
                        {
                            totalDD = double.Parse(dtPlantWC.Rows[i][2].ToString());
                            totalID = double.Parse(dtPlantWC.Rows[i][4].ToString());
                            totalDDSum += totalDD;
                            totalIDSum += totalID;
                        }
                        double totaltotal = totalDDSum + totalIDSum;
                        for (int i = 7; i < 22; i++)
                        {
                            Misc.percentage(dtPlantWC.Rows[i], totalDDSum, totalIDSum);

                            dtPlantWC.Rows[i][7] = double.Parse(dtPlantWC.Rows[i][6].ToString()) * 100 / totaltotal;
                            dtPlantWC.AcceptChanges();
                        }
                        double totalPerDD = 0;
                        double totalPerID = 0;
                        double totalPerSum = 0;
                        for (int i = 7; i < 22; i++)
                        {
                            totalPerDD += double.Parse(dtPlantWC.Rows[i][3].ToString());
                            totalPerID += double.Parse(dtPlantWC.Rows[i][5].ToString());
                            totalPerSum += double.Parse(dtPlantWC.Rows[i][7].ToString());
                        }
                        dtPlantWC.Rows[23][3] = totalPerDD;
                        dtPlantWC.Rows[23][5] = totalPerID;
                        dtPlantWC.Rows[23][7] = totalPerSum;

                        dtPlantWC.Rows[24][3] = 0;
                        dtPlantWC.Rows[24][5] = 0;
                        dtPlantWC.Rows[24][7] = 0;

                        dtPlantWC.Rows[25][3] = "  ";
                        dtPlantWC.Rows[25][5] = "  ";
                        dtPlantWC.Rows[25][7] = "  ";
                        numOfEmployees = numEmplPlantIndirect;
                        double totalDays = numOfEmployees * timeSpan.TotalDays;
                        DataRow rowEmptyFooter = dtPlantWC.NewRow();
                        dtPlantWC.Rows.Add(rowEmptyFooter);

                        DataRow footer = dtPlantWC.NewRow();
                        footer[1] = "N° Employees: " + numOfEmployees;
                        footer[2] = "Total days: ";
                        footer[3] = totalDays;
                        footer[4] = "Calendar days: ";
                        footer[5] = timeSpan.TotalDays;

                        dtPlantWC.Rows.Add(footer);
                        ds.Tables.Add(dtPlantWC);
                        ds.AcceptChanges();

                        icompany++;
                        if (icompany == 1)
                        {
                            DataRow rowHeader1 = dtCompanyWC.NewRow();

                            rowHeader1[1] = companyWU.Description;

                            rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                            rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                            //rowHeader1[8] = "page: " + page;
                            dtCompanyWC.Rows.Add(rowHeader1);

                            DataRow rowHeader2 = dtCompanyWC.NewRow();
                            rowHeader2[1] = "   Absenteeism industrial relation";
                            rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                            rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                            dtCompanyWC.Rows.Add(rowHeader2);

                            DataRow rowHeader3 = dtCompanyWC.NewRow();
                            rowHeader3[1] = "Qualify:   " + "WC";
                            dtCompanyWC.Rows.Add(rowHeader3);
                            
                            DataRow rowPlant = dtCompanyWC.NewRow();
                            rowPlant[1] = "Company report";
                            dtCompanyWC.Rows.Add(rowPlant);

                            DataRow rowWorkgroup = dtCompanyWC.NewRow();
                            dtCompanyWC.Rows.Add(rowWorkgroup);

                            DataRow rowEmpty = dtCompanyWC.NewRow();
                            dtCompanyWC.Rows.Add(rowEmpty);

                            DataRow rowColumns = dtCompanyWC.NewRow();
                            rowColumns[2] = "Direct";
                            rowColumns[3] = "%";
                            rowColumns[4] = "Indirect";
                            rowColumns[5] = "%";
                            rowColumns[6] = "Total";
                            rowColumns[7] = "%";
                            dtCompanyWC.Rows.Add(rowColumns);

                            for (int ind = 7; ind < dtPlantWC.Rows.Count - 2; ind++)
                            {
                                DataRow row = dtCompanyWC.NewRow();

                                row[1] = dtPlantWC.Rows[ind][1];
                                row[2] = dtPlantWC.Rows[ind][2];
                                row[4] = dtPlantWC.Rows[ind][4];
                                row[6] = dtPlantWC.Rows[ind][6];
                                dtCompanyWC.Rows.Add(row);
                                dtCompanyWC.AcceptChanges();
                            }
                        }
                        else
                        {
                            for (int ind = 7; ind < dtPlantWC.Rows.Count - 2; ind++)
                            {
                                if (ind != 22)
                                {
                                    dtCompanyWC.Rows[ind][1] = dtCompanyWC.Rows[ind][1];
                                    dtCompanyWC.Rows[ind][2] = double.Parse(dtCompanyWC.Rows[ind][2].ToString()) + double.Parse(dtPlantWC.Rows[ind][2].ToString());
                                    dtCompanyWC.Rows[ind][4] = double.Parse(dtCompanyWC.Rows[ind][4].ToString()) + double.Parse(dtPlantWC.Rows[ind][4].ToString());
                                    dtCompanyWC.Rows[ind][6] = double.Parse(dtCompanyWC.Rows[ind][6].ToString()) + double.Parse(dtPlantWC.Rows[ind][6].ToString());

                                }
                            }
                        }
                    }
                }
                if (dtCompanyBC.Rows.Count > 6)
                {
                    double totalDD = 0;
                    double totalID = 0;
                    double totalDDSum = 0;
                    double totalIDSum = 0;
                    for (int i = 7; i < 22; i++)
                    {
                        totalDD = double.Parse(dtCompanyBC.Rows[i][2].ToString());
                        totalID = double.Parse(dtCompanyBC.Rows[i][4].ToString());
                        totalDDSum += totalDD;
                        totalIDSum += totalID;
                    }
                    double totaltotal = totalDDSum + totalIDSum;
                    for (int i = 7; i < 22; i++)
                    {
                        Misc.percentage(dtCompanyBC.Rows[i], totalDDSum, totalIDSum);

                        dtCompanyBC.Rows[i][7] = double.Parse(dtCompanyBC.Rows[i][6].ToString()) * 100 / totaltotal;
                        dtCompanyBC.AcceptChanges();
                    }
                    double totalPerDD = 0;
                    double totalPerID = 0;
                    double totalPerSum = 0;
                    for (int i = 7; i < 22; i++)
                    {
                        totalPerDD += double.Parse(dtCompanyBC.Rows[i][3].ToString());
                        totalPerID += double.Parse(dtCompanyBC.Rows[i][5].ToString());
                        totalPerSum += double.Parse(dtCompanyBC.Rows[i][7].ToString());
                    }
                    dtCompanyBC.Rows[23][3] = totalPerDD;
                    dtCompanyBC.Rows[23][5] = totalPerID;
                    dtCompanyBC.Rows[23][7] = totalPerSum;

                    dtCompanyBC.Rows[24][3] = 0;
                    dtCompanyBC.Rows[24][5] = 0;
                    dtCompanyBC.Rows[24][7] = 0;

                    dtCompanyBC.Rows[25][3] = "  ";
                    dtCompanyBC.Rows[25][5] = "  ";
                    dtCompanyBC.Rows[25][7] = "  ";
                    dtCompanyBC.AcceptChanges();
                    numOfEmployees = numEmplCompanyDirect;
                    double totalDays = numOfEmployees * timeSpan.TotalDays;
                    DataRow rowEmptyFooter = dtCompanyBC.NewRow();
                    dtCompanyBC.Rows.Add(rowEmptyFooter);

                    DataRow footer = dtCompanyBC.NewRow();
                    footer[1] = "N° Employees: " + numOfEmployees;
                    footer[2] = "Total days: ";
                    footer[3] = totalDays;
                    footer[4] = "Calendar days: ";
                    footer[5] = timeSpan.TotalDays;
                    dtCompanyBC.Rows.Add(footer);

                    ds.Tables.Add(dtCompanyBC);
                    ds.AcceptChanges();
                }
                if (dtCompanyWC.Rows.Count > 6)
                {
                    double totalDD = 0;
                    double totalID = 0;
                    double totalDDSum = 0;
                    double totalIDSum = 0;
                    for (int i = 7; i < 22; i++)
                    {
                        totalDD = double.Parse(dtCompanyWC.Rows[i][2].ToString());
                        totalID = double.Parse(dtCompanyWC.Rows[i][4].ToString());
                        totalDDSum += totalDD;
                        totalIDSum += totalID;
                    }
                    double totaltotal = totalDDSum + totalIDSum;
                    for (int i = 7; i < 22; i++)
                    {
                        Misc.percentage(dtCompanyWC.Rows[i], totalIDSum, totalIDSum);
                        dtCompanyWC.Rows[i][7] = double.Parse(dtCompanyWC.Rows[i][6].ToString()) * 100 / totaltotal;
                        dtCompanyWC.AcceptChanges();

                    }
                    double totalPerDD = 0;
                    double totalPerID = 0;
                    double totalPerSum = 0;
                    for (int i = 7; i < 22; i++)
                    {
                        totalPerDD += double.Parse(dtCompanyWC.Rows[i][3].ToString());
                        totalPerID += double.Parse(dtCompanyWC.Rows[i][5].ToString());
                        totalPerSum += double.Parse(dtCompanyWC.Rows[i][7].ToString());
                    }
                    dtCompanyWC.Rows[23][3] = totalPerDD;
                    dtCompanyWC.Rows[23][5] = totalPerID;
                    dtCompanyWC.Rows[23][7] = totalPerSum;

                    dtCompanyWC.Rows[24][3] = 0;
                    dtCompanyWC.Rows[24][5] = 0;
                    dtCompanyWC.Rows[24][7] = 0;

                    dtCompanyWC.Rows[25][3] = "  ";
                    dtCompanyWC.Rows[25][5] = "  ";
                    dtCompanyWC.Rows[25][7] = "  ";

                    dtCompanyWC.AcceptChanges();
                    numOfEmployees = numEmplCompanyIndirect;
                    double totalDays = numOfEmployees * timeSpan.TotalDays;
                    DataRow rowEmptyFooter = dtCompanyWC.NewRow();
                    dtCompanyWC.Rows.Add(rowEmptyFooter);

                    DataRow footer = dtCompanyWC.NewRow();
                    footer[1] = "N° Employees: " + numOfEmployees;
                    footer[2] = "Total days: ";
                    footer[3] = totalDays;
                    footer[4] = "Calendar days: ";
                    footer[5] = timeSpan.TotalDays;

                    dtCompanyWC.Rows.Add(footer);
                    dtCompanyWC.AcceptChanges();
                    ds.Tables.Add(dtCompanyWC);
                    ds.AcceptChanges();
                }

                string Pathh = Directory.GetParent(filePath).FullName;
                if (!Directory.Exists(Pathh))
                {
                    Directory.CreateDirectory(Pathh);
                }
                if (File.Exists(filePath))
                    File.Delete(filePath);

                ExportToExcel.CreateExcelDocument(ds, filePath, false, false);

                debug.writeLog("+ Finished 400! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                return filePath;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "generateExcel400() " + ex.Message);
                return "";
            }
        }

        public List<WorkTimeIntervalTO> getTimeSchemaInterval(int employeeID, DateTime date, List<EmployeeTimeScheduleTO> timeScheduleListForEmployee, List<WorkTimeSchemaTO> timeSchema)
        {
            List<WorkTimeIntervalTO> intervalList = new List<WorkTimeIntervalTO>();
            try
            {
                int timeScheduleIndex = -1;

                for (int scheduleIndex = 0; scheduleIndex < timeScheduleListForEmployee.Count; scheduleIndex++)
                {

                    if (date >= timeScheduleListForEmployee[scheduleIndex].Date)
                    {
                        timeScheduleIndex = scheduleIndex;
                    }
                }

                if (timeScheduleIndex >= 0)
                {
                    int cycleDuration = 0;
                    int startDay = timeScheduleListForEmployee[timeScheduleIndex].StartCycleDay;
                    int schemaID = timeScheduleListForEmployee[timeScheduleIndex].TimeSchemaID;
                    List<WorkTimeSchemaTO> timeSchemaEmployee = new List<WorkTimeSchemaTO>();
                    foreach (WorkTimeSchemaTO timeSch in timeSchema)
                    {
                        if (timeSch.TimeSchemaID == schemaID)
                        {
                            timeSchemaEmployee.Add(timeSch);
                        }
                    }
                    WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                    if (timeSchemaEmployee.Count > 0)
                    {
                        schema = timeSchemaEmployee[0];
                        cycleDuration = schema.CycleDuration;
                    }

                    TimeSpan days = new TimeSpan(date.Date.Ticks - timeScheduleListForEmployee[timeScheduleIndex].Date.Date.Ticks);

                    Dictionary<int, WorkTimeIntervalTO> table = schema.Days[(startDay + (int)days.TotalDays) % cycleDuration];
                    for (int i = 0; i < table.Count; i++)
                    {
                        intervalList.Add(table[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return intervalList;
        }
        Dictionary<int, Dictionary<int, decimal>> lunchTypes = new Dictionary<int, Dictionary<int, decimal>>();
        private decimal CalcLunchBreak(string emplID, int company, List<IOPairProcessedTO> IOPairsList, DateTime fromDate, DateTime toDate, object dbConnection, Dictionary<int, PassTypeTO> passTypesDictionaryAll, Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDictionary, int direct)
        {
            try
            {
                decimal TotalDuration = 0;
                if (emplID.Length > 0)
                {
                    //debug.writeLog("holuday, start " + DateTime.Now.ToString("HH:mm:ss"));
                    Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();


                    foreach (IOPairProcessedTO pair in IOPairsList)
                    {
                        if (!emplDayPairs.ContainsKey(pair.EmployeeID))
                            emplDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                        if (!emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                            emplDayPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                        emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                    }

                    Dictionary<int, EmployeeTO> employeesData = new Employee().SearchDictionary(emplID);


                    //dictionary for efective work key is working unit id value is list of pass types for effective work for lunch break
                    Dictionary<int, Dictionary<int, List<int>>> effectiveLunchBreakWorkDict = new Dictionary<int, Dictionary<int, List<int>>>();
                    //get types for effective works (use it for lunch break )
                    foreach (int wuID in rulesDictionary.Keys)
                    {
                        if (!effectiveLunchBreakWorkDict.ContainsKey(wuID))
                            effectiveLunchBreakWorkDict.Add(wuID, new Dictionary<int, List<int>>());
                        foreach (int emplTpe in rulesDictionary[wuID].Keys)
                        {
                            if (!effectiveLunchBreakWorkDict[wuID].ContainsKey(emplTpe))
                                effectiveLunchBreakWorkDict[wuID].Add(emplTpe, new List<int>());
                            foreach (string ruleName in Constants.effectiveWorkLunchBreakWageTypes())
                            {
                                if (rulesDictionary[wuID][emplTpe].ContainsKey(ruleName))
                                {
                                    int value = rulesDictionary[wuID][emplTpe][ruleName].RuleValue;
                                    if (!effectiveLunchBreakWorkDict[wuID][emplTpe].Contains(value))
                                    {
                                        effectiveLunchBreakWorkDict[wuID][emplTpe].Add(value);
                                    }
                                }
                            }
                        }
                    }

                    foreach (int emplId in emplDayPairs.Keys)
                    {
                        Dictionary<string, RuleTO> emplRules = new Dictionary<string, RuleTO>();
                        EmployeeTO empl = new EmployeeTO();

                        List<string> rulesList = new List<string>();
                        if (employeesData.ContainsKey(emplId))
                            empl = employeesData[emplId];

                        //get employee rules
                        if (rulesDictionary.ContainsKey(company) && rulesDictionary[company].ContainsKey(empl.EmployeeTypeID))
                        {
                            emplRules = rulesDictionary[company][empl.EmployeeTypeID];
                        }
                        //get all types considare effective work for lunch break
                        List<int> emplEffTypesLunchBreak = new List<int>();
                        if (effectiveLunchBreakWorkDict.ContainsKey(company) && effectiveLunchBreakWorkDict[company].ContainsKey(empl.EmployeeTypeID))
                        {
                            emplEffTypesLunchBreak = effectiveLunchBreakWorkDict[company][empl.EmployeeTypeID];
                        }
                        foreach (DateTime date in emplDayPairs[emplId].Keys)
                        {
                            decimal maxDuration = 0;
                            int maxType = -1;
                            decimal emplDuration = 0;

                            foreach (IOPairProcessedTO pairTo in emplDayPairs[emplId][date])
                            {
                                if (emplEffTypesLunchBreak.Contains(pairTo.PassTypeID))
                                {
                                    decimal duration = (decimal)(pairTo.EndTime - pairTo.StartTime).TotalHours;
                                    emplDuration += duration;
                                    if (maxDuration < duration)
                                    {
                                        maxDuration = duration;
                                        maxType = pairTo.PassTypeID;
                                    }
                                }
                            }

                            if (emplRules.ContainsKey(Constants.RuleMealMinPresence) && emplDuration > (emplRules[Constants.RuleMinPresence].RuleValue / 60) && maxDuration > (decimal)Constants.LunchBreakDuration / 60)
                            {
                                if (!lunchTypes.ContainsKey(direct))
                                {
                                    lunchTypes.Add(direct, new Dictionary<int, decimal>());
                                }
                                if (!lunchTypes[direct].ContainsKey(maxType))
                                    lunchTypes[direct].Add(maxType, 0);

                                lunchTypes[direct][maxType] += (decimal)Constants.LunchBreakDuration / 60;

                                TotalDuration += (decimal)Constants.LunchBreakDuration / 60;
                            }
                        }
                    }
                }
                return TotalDuration;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private bool isNationalHoliday(IOPairProcessedTO pair, string EmplTimeSchema, WorkTimeIntervalTO pairInterval)
        //{
        //    try
        //    {
        //        DateTime pairDate = pair.IOPairDate.Date;

        //        // if pair is from second interval of night shift, it belongs to previous day
        //        //WorkTimeIntervalTO pairInterval = getPairInterval(pair, dayPairs);
        //        if (pairInterval.TimeSchemaID != -1 && Common.Misc.isThirdShiftEndInterval(pairInterval))
        //            pairDate = pairDate.AddDays(-1);

        //        // check if date is national holiday, national holidays are transferd from Sunday to first working day
        //        List<DateTime> nationalHolidaysDays = new List<DateTime>();
        //        List<DateTime> nationalHolidaysSundays = new List<DateTime>();
        //        List<DateTime> nationalHolidaysSundaysDays = new List<DateTime>();
        //        Dictionary<string, List<DateTime>> personalHolidayDays = new Dictionary<string, List<DateTime>>();
        //        List<HolidaysExtendedTO> nationalTransferableHolidays = new List<HolidaysExtendedTO>();

        //        Common.Misc.getHolidays(pairDate, pairDate, personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, null, nationalTransferableHolidays);

        //        if (EmplTimeSchema.ToUpper().Equals(Constants.schemaTypeIndustrial.Trim().ToUpper()))
        //        {
        //            if (nationalHolidaysDays.Contains(pairDate.Date))
        //                return true;
        //        }
        //        else if (nationalHolidaysDays.Contains(pairDate.Date) || nationalHolidaysSundays.Contains(pairDate.Date))
        //            return true;

        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //private bool isPersonalHoliday(IOPairProcessedTO pair, WorkTimeIntervalTO pairInterval)
        //{
        //    try
        //    {
        //        string holidayType = "";
        //        DateTime pairDate = pair.IOPairDate.Date;

        //        // if pair is from second interval of night shift, it belongs to previous day
        //        //WorkTimeIntervalTO pairInterval = getPairInterval(pair, dayPairs);
        //        if (pairInterval.TimeSchemaID != -1 && Common.Misc.isThirdShiftEndInterval(pairInterval))
        //            pairDate = pairDate.AddDays(-1);

        //        // check if date is personal holiday, no transfering holidays for personal holidays
        //        // get employee personal holiday category
        //        EmployeeAsco4 emplAsco = new EmployeeAsco4();
        //        emplAsco.EmplAsco4TO.EmployeeID = pair.EmployeeID;
        //        List<EmployeeAsco4TO> ascoList = emplAsco.Search();

        //        if (ascoList.Count == 1)
        //        {
        //            holidayType = ascoList[0].NVarcharValue1.Trim();

        //            if (!holidayType.Trim().Equals(""))
        //            {
        //                // if category is IV, find holiday date and check if pair date is holiday
        //                if (holidayType.Trim().Equals(Constants.holidayTypeIV.Trim()))
        //                {
        //                    DateTime holDate = ascoList[0].DatetimeValue1.Date;

        //                    if (holDate.Month == pairDate.Month && holDate.Day == pairDate.Day)
        //                        return true;
        //                }
        //                else
        //                {
        //                    // if category is I, II or III, check if pair date is personal holiday
        //                    HolidaysExtended holExtended = new HolidaysExtended();
        //                    holExtended.HolTO.Type = Constants.personalHoliday.Trim();
        //                    holExtended.HolTO.Category = holidayType.Trim();

        //                    if (holExtended.Search(pairDate, pairDate).Count > 0)
        //                        return true;
        //                }
        //            }
        //        }

        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private void populateDataTable400New(Object dbConnection, DataTable dt, List<DateTime> datesList, WorkingUnitTO company, List<IOPairProcessedTO> IOPairListDirect, List<IOPairProcessedTO> IOPairListIndirect, string emplIdsBS, string emplIds, string qualify, WorkingUnitTO workingUnit, string title, List<DateTime> nationalHolidaysDays,
        //        Dictionary<string, List<DateTime>> personalHolidayDays,
        //        List<DateTime> nationalHolidaysSundays,
        //        List<HolidaysExtendedTO> nationalTransferableHolidays, Dictionary<int, PassTypeTO> passTypesDictionaryAll, Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDictionary)
        //{
        //    try
        //    {
        //        PassType passType;
        //        if (dbConnection == null)
        //        {

        //            passType = new PassType();
        //        }
        //        else
        //        {
        //            passType = new PassType(dbConnection);
        //        }
        //        Dictionary<int, PassTypeTO> listPassTypes = new Dictionary<int, PassTypeTO>();
        //        WorkingUnit wu;
        //        if (dbConnection == null)
        //            wu = new WorkingUnit();
        //        else
        //            wu = new WorkingUnit(dbConnection);
        //        string ute = "";
        //        string workGroup = "";
        //        string costCenter = "";
        //        string plant = "";

        //        DataRow rowHeader1 = dt.NewRow();

        //        rowHeader1[1] = company.Description;

        //        //rowHeader1[1] = "FIAT GROUP AUTOMOBILES S.P.A.";
        //        rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
        //        rowHeader1[7] = DateTime.Now.ToString("HH:mm");
        //        //rowHeader1[8] = "page: " + page;
        //        dt.Rows.Add(rowHeader1);

        //        DataRow rowHeader2 = dt.NewRow();
        //        rowHeader2[1] = title;
        //        rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
        //        rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
        //        dt.Rows.Add(rowHeader2);

        //        WorkingUnitTO tempWU = wu.FindWU(workingUnit.WorkingUnitID);
        //        ute = tempWU.Code.Trim();

        //        // get workshop (parent of UTE)
        //        wu.WUTO = tempWU;
        //        tempWU = wu.getParentWorkingUnit();
        //        workGroup = tempWU.Code.Trim();

        //        // get cost centar
        //        wu.WUTO = tempWU;
        //        tempWU = wu.getParentWorkingUnit();
        //        costCenter = tempWU.Code.Trim();

        //        // get plant
        //        wu.WUTO = tempWU;
        //        tempWU = wu.getParentWorkingUnit();
        //        plant = tempWU.Code.Trim();
        //        DataRow rowHeader3 = dt.NewRow();

        //        rowHeader3[1] = "Qualify:   " + qualify;
        //        dt.Rows.Add(rowHeader3);

        //        DataRow rowPlant = dt.NewRow();

        //        rowPlant[1] = "Plant:     " + plant + "     Cost center: " + costCenter;

        //        dt.Rows.Add(rowPlant);

        //        DataRow rowWorkgroup = dt.NewRow();

        //        rowWorkgroup[1] = "Workgroup: " + workGroup + "  UTE: " + ute;

        //        dt.Rows.Add(rowWorkgroup);


        //        DataRow rowEmpty = dt.NewRow();
        //        dt.Rows.Add(rowEmpty);

        //        DataRow rowColumns = dt.NewRow();
        //        rowColumns[2] = "Direct";
        //        rowColumns[3] = "%";
        //        rowColumns[4] = "Indirect";
        //        rowColumns[5] = "%";
        //        rowColumns[6] = "Total";
        //        rowColumns[7] = "%";
        //        dt.Rows.Add(rowColumns);
        //        DataRow rowNotJustified = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0000", -1);


        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "NOT JUSTIFIED HOURS", rowNotJustified, emplIdsBS, emplIds, datesList);
        //        }
        //        else calc400Empty("NOT JUSTIFIED HOURS", rowNotJustified);
        //        DataRow rowNew = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0012,0014,0041,0071", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "PRESENCE", rowNew, emplIdsBS, emplIds, datesList);
        //        }
        //        else
        //            calc400Empty("PRESENCE", rowNew);

        //        DataRow rowRegularWork = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0012", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {
        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "REGULAR WORK", rowRegularWork, emplIdsBS, emplIds, datesList);
        //        }
        //        else
        //            calc400Empty("REGULAR WORK", rowRegularWork);




        //        DataRow rowBusinessTrip = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0014", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "BUSINESS TRIP", rowBusinessTrip, emplIdsBS, emplIds, datesList);
        //        }
        //        else
        //            calc400Empty("BUSINESS TRIP", rowBusinessTrip);


        //        DataRow rowAbsenceDuringDay = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0071", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "ABSENCE DURING THE DAY FOR BUSINESS PURPOSES", rowAbsenceDuringDay, emplIdsBS, emplIds, datesList);
        //        }
        //        else
        //            calc400Empty("ABSENCE DURING THE DAY FOR BUSINESS PURPOSES", rowAbsenceDuringDay);

        //        //WORK ON NATIONAL HOLIDAY
        //        List<string> rulesList = new List<string>();
        //        foreach (string ruleName in Constants.effectiveWorkWageTypes())
        //        {
        //            Common.Rule rule = new Common.Rule();
        //            rule.RuleTO.WorkingUnitID = company.WorkingUnitID;
        //            //rule.RuleTO.EmployeeTypeID = empl.EmployeeTypeID;
        //            rule.RuleTO.RuleType = ruleName;
        //            List<RuleTO> rules = rule.Search();
        //            foreach (RuleTO ruleto in rules)
        //            {
        //                if (!rulesList.Contains(ruleto.RuleValue.ToString()))
        //                    rulesList.Add(ruleto.RuleValue.ToString());
        //            }
        //        }


        //        DataRow rowWorkOnNationalHoliday = dt.NewRow();
        //        rowWorkOnNationalHoliday[1] = "WORK DURING NATIONAL HOLIDAY";

        //        decimal workOnHolidaysDirect = CalcWorkOnHoliday(emplIdsBS, company.WorkingUnitID, IOPairListDirect, datesList[0], datesList[datesList.Count - 1], dbConnection, rulesList, nationalHolidaysDays, personalHolidayDays, nationalHolidaysSundays, nationalTransferableHolidays, passTypesDictionaryAll, rulesDictionary);
        //        decimal workOnHolidaysIndirect = CalcWorkOnHoliday(emplIds, company.WorkingUnitID, IOPairListIndirect, datesList[0], datesList[datesList.Count - 1], dbConnection, rulesList, nationalHolidaysDays, personalHolidayDays, nationalHolidaysSundays, nationalTransferableHolidays, passTypesDictionaryAll, rulesDictionary);
        //        rowWorkOnNationalHoliday["Direct"] = workOnHolidaysDirect;
        //        rowWorkOnNationalHoliday["Indirect"] = workOnHolidaysIndirect;
        //        rowWorkOnNationalHoliday["Total"] = workOnHolidaysIndirect + workOnHolidaysDirect;


        //        Common.Rule rule1 = new Common.Rule();
        //        rule1.RuleTO.WorkingUnitID = company.WorkingUnitID;
        //        //rule.RuleTO.EmployeeTypeID = empl.EmployeeTypeID;
        //        rule1.RuleTO.RuleType = Constants.RuleCompanyInitialOvertime;
        //        List<RuleTO> rules1 = rule1.Search();
        //        int pass_type_id = -1;
        //        if (rules1.Count > 0)
        //            pass_type_id = int.Parse(rules1[0].RuleValue.ToString());

        //        DataRow rowOvertimeToJustify = dt.NewRow();

        //        if (passTypesDictionaryAll.ContainsKey(pass_type_id))
        //        {
        //            listPassTypes = new Dictionary<int, PassTypeTO>();
        //            listPassTypes.Add(pass_type_id, passTypesDictionaryAll[pass_type_id]);
        //        }
        //        else
        //        {
        //            listPassTypes = new Dictionary<int, PassTypeTO>();
        //        }

        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "OVERTIME TO JUSTIFY", rowOvertimeToJustify, emplIdsBS, emplIds, datesList);
        //        }
        //        else
        //            calc400Empty("OVERTIME TO JUSTIFY", rowOvertimeToJustify);



        //        DataRow rowOvertime = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0030", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "OVERTIME", rowOvertime, emplIdsBS, emplIds, datesList);
        //        }
        //        else
        //            calc400Empty("OVERTIME", rowOvertime);

        //        DataRow rowOvertimeBank = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0312", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {
        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "OVERPRESENCE BANK HOURS", rowOvertimeBank, emplIdsBS, emplIds, datesList);
        //        }
        //        else
        //            calc400Empty("OVERPRESENCE BANK HOURS", rowOvertimeBank);

        //        DataRow rowAbsentism = dt.NewRow();
        //        string s = "0043,0045,0046,0047,0048,0049,0055,0056,0057,0058,0060,0061,0069,0070,0075,0144,0145,0146,0148,0149,0155,0156,";
        //        s += "0157,0169,0257,1257,1148,1155,0369,1145,1149,1150,0153,0171,0130,1069,0160,1160,1406,7777";
        //        listPassTypes = passType.FindByPaymentCode(s, company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "ABSENTEISM", rowAbsentism, emplIdsBS, emplIds, datesList);
        //        }
        //        else calc400Empty("ABSENTEEISM", rowAbsentism);


        //        DataRow rowStrike = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("1407", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {
        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "STRIKE", rowStrike, emplIdsBS, emplIds, datesList);
        //        }
        //        else calc400Empty("STRIKE", rowStrike);

        //        DataRow rowNoWork = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0053", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "NO WORK", rowNoWork, emplIdsBS, emplIds, datesList);
        //        }
        //        else
        //            calc400Empty("NO WORK", rowNoWork);


        //        DataRow rowHoliday = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0040,0044", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "HOLIDAY", rowHoliday, emplIdsBS, emplIds, datesList);
        //        }
        //        else
        //            calc400Empty("HOLIDAY", rowHoliday);


        //        DataRow rowUsedBankH = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0212", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "USED BANK HOURS", rowUsedBankH, emplIdsBS, emplIds, datesList);
        //        }
        //        else
        //            calc400Empty("USED BANK HOURS", rowUsedBankH);

        //        DataRow rowUnionLeaves = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0112", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "UNION LEAVES", rowUnionLeaves, emplIdsBS, emplIds, datesList);
        //        }
        //        else
        //            calc400Empty("UNION LEAVES", rowUnionLeaves);


        //        DataRow rowSchoolLeaves = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0147", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "SCHOOL LEAVES", rowSchoolLeaves, emplIdsBS, emplIds, datesList);
        //        }
        //        else
        //            calc400Empty("SCHOOL LEAVES", rowSchoolLeaves);


        //        DataRow rowNoWorkRecoveryH = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0512", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "NO WORK RECOVERY HOURS", rowNoWorkRecoveryH, emplIdsBS, emplIds, datesList);
        //        }
        //        else
        //            calc400Empty("NO WORK RECOVERY HOURS", rowNoWorkRecoveryH);


        //        DataRow rowProductiveLeaves = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0612", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "PRODUCTIVE RECOVERY", rowProductiveLeaves, emplIdsBS, emplIds, datesList);
        //        }
        //        else
        //            calc400Empty("PRODUCTIVE RECOVERY", rowProductiveLeaves);



        //        double totalDD = double.Parse(rowRegularWork[2].ToString()) + double.Parse(rowBusinessTrip[2].ToString()) + double.Parse(rowAbsenceDuringDay[2].ToString()) + double.Parse(rowWorkOnNationalHoliday[2].ToString()) + double.Parse(rowOvertimeToJustify[2].ToString()) + double.Parse(rowProductiveLeaves[2].ToString()) + double.Parse(rowNew[2].ToString()) + double.Parse(rowNoWorkRecoveryH[2].ToString()) + double.Parse(rowSchoolLeaves[2].ToString()) + double.Parse(rowUnionLeaves[2].ToString())
        //            + double.Parse(rowUsedBankH[2].ToString()) + double.Parse(rowHoliday[2].ToString()) + double.Parse(rowNoWork[2].ToString()) + double.Parse(rowStrike[2].ToString()) + double.Parse(rowAbsentism[2].ToString())
        //            + double.Parse(rowOvertime[2].ToString()) + double.Parse(rowOvertimeBank[2].ToString()) + double.Parse(rowNotJustified[2].ToString());

        //        double totalID = double.Parse(rowRegularWork[4].ToString()) + double.Parse(rowBusinessTrip[4].ToString()) + double.Parse(rowAbsenceDuringDay[4].ToString()) + double.Parse(rowWorkOnNationalHoliday[4].ToString()) + double.Parse(rowOvertimeToJustify[4].ToString()) + double.Parse(rowProductiveLeaves[4].ToString()) + double.Parse(rowNew[4].ToString()) + double.Parse(rowNoWorkRecoveryH[4].ToString()) + double.Parse(rowSchoolLeaves[4].ToString()) + double.Parse(rowUnionLeaves[4].ToString())
        //               + double.Parse(rowUsedBankH[4].ToString()) + double.Parse(rowHoliday[4].ToString()) + double.Parse(rowNoWork[4].ToString()) + double.Parse(rowStrike[4].ToString()) + double.Parse(rowAbsentism[4].ToString())
        //               + double.Parse(rowOvertime[4].ToString()) + double.Parse(rowOvertimeBank[4].ToString()) + double.Parse(rowNotJustified[4].ToString());

        //        double total = totalDD + totalID;
        //        DataRow row = dt.NewRow();
        //        row[1] = "Total";
        //        row[2] = totalDD;
        //        row[4] = totalID;
        //        row[6] = total;
        //        row[5] = 0;
        //        row[3] = 0;
        //        DataRow rowTotal = dt.NewRow();
        //        rowTotal[1] = "Planned";
        //        if (emplIdsBS.Length > 0)
        //            rowTotal[2] = Misc.CalcPlannedTime(dbConnection, emplIdsBS, datesList[0], datesList[datesList.Count - 1]);
        //        else
        //            rowTotal[2] = 0;
        //        if (emplIds.Length > 0)
        //            rowTotal[4] = Misc.CalcPlannedTime(dbConnection, emplIds, datesList[0], datesList[datesList.Count - 1]);
        //        else
        //            rowTotal[4] = 0;

        //        rowTotal[6] = double.Parse(rowTotal[2].ToString()) + double.Parse(rowTotal[4].ToString());
        //        rowTotal[5] = 0;
        //        rowTotal[3] = 0;

        //        DataRow rowTotalDiff = dt.NewRow();
        //        rowTotalDiff[1] = "+/-";
        //        rowTotalDiff[3] = "  ";
        //        rowTotalDiff[5] = "  ";
        //        rowTotalDiff[7] = "  ";
        //        rowTotalDiff[2] = double.Parse(row[2].ToString()) - double.Parse(rowTotal[2].ToString());
        //        rowTotalDiff[4] = double.Parse(row[4].ToString()) - double.Parse(rowTotal[4].ToString());
        //        rowTotalDiff[6] = double.Parse(row[6].ToString()) - double.Parse(rowTotal[6].ToString());

        //        Misc.percentage(rowRegularWork, totalDD, totalID);
        //        Misc.percentage(rowBusinessTrip, totalDD, totalID);
        //        Misc.percentage(rowAbsenceDuringDay, totalDD, totalID);
        //        Misc.percentage(rowWorkOnNationalHoliday, totalDD, totalID);
        //        Misc.percentage(rowOvertimeToJustify, totalDD, totalID);
        //        Misc.percentage(rowNew, totalDD, totalID);
        //        Misc.percentage(rowProductiveLeaves, totalDD, totalID);
        //        Misc.percentage(rowNoWorkRecoveryH, totalDD, totalID);
        //        Misc.percentage(rowSchoolLeaves, totalDD, totalID);
        //        Misc.percentage(rowUnionLeaves, totalDD, totalID);
        //        Misc.percentage(rowUsedBankH, totalDD, totalID);
        //        Misc.percentage(rowHoliday, totalDD, totalID);
        //        Misc.percentage(rowNoWork, totalDD, totalID);
        //        Misc.percentage(rowStrike, totalDD, totalID);
        //        Misc.percentage(rowAbsentism, totalDD, totalID);
        //        Misc.percentage(rowOvertimeBank, totalDD, totalID);
        //        Misc.percentage(rowOvertime, totalDD, totalID);
        //        Misc.percentage(rowNotJustified, totalDD, totalID);



        //        rowRegularWork[7] = double.Parse(rowRegularWork[6].ToString()) * 100 / total;
        //        rowBusinessTrip[7] = double.Parse(rowBusinessTrip[6].ToString()) * 100 / total;
        //        rowAbsenceDuringDay[7] = double.Parse(rowAbsenceDuringDay[6].ToString()) * 100 / total;
        //        rowWorkOnNationalHoliday[7] = double.Parse(rowWorkOnNationalHoliday[6].ToString()) * 100 / total;
        //        rowOvertimeToJustify[7] = double.Parse(rowOvertimeToJustify[6].ToString()) * 100 / total;
        //        rowNew[7] = double.Parse(rowNew[6].ToString()) * 100 / total;
        //        rowProductiveLeaves[7] = double.Parse(rowProductiveLeaves[6].ToString()) * 100 / total;
        //        rowNoWorkRecoveryH[7] = double.Parse(rowNoWorkRecoveryH[6].ToString()) * 100 / total;
        //        rowSchoolLeaves[7] = double.Parse(rowSchoolLeaves[6].ToString()) * 100 / total;
        //        rowUnionLeaves[7] = double.Parse(rowUnionLeaves[6].ToString()) * 100 / total;
        //        rowUsedBankH[7] = double.Parse(rowUsedBankH[6].ToString()) * 100 / total;
        //        rowHoliday[7] = double.Parse(rowHoliday[6].ToString()) * 100 / total;
        //        rowNoWork[7] = double.Parse(rowNoWork[6].ToString()) * 100 / total;
        //        rowStrike[7] = double.Parse(rowStrike[6].ToString()) * 100 / total;
        //        rowAbsentism[7] = double.Parse(rowAbsentism[6].ToString()) * 100 / total;
        //        rowOvertimeBank[7] = double.Parse(rowOvertimeBank[6].ToString()) * 100 / total;
        //        rowOvertime[7] = double.Parse(rowOvertime[6].ToString()) * 100 / total;
        //        rowNotJustified[7] = double.Parse(rowNotJustified[6].ToString()) * 100 / total;
        //        row[7] = double.Parse(row[6].ToString()) * 100 / total;



        //        if (rowRegularWork[7].ToString() == "NaN") rowRegularWork[7] = 0;
        //        if (rowBusinessTrip[7].ToString() == "NaN") rowBusinessTrip[7] = 0;
        //        if (rowAbsenceDuringDay[7].ToString() == "NaN") rowAbsenceDuringDay[7] = 0;
        //        if (rowWorkOnNationalHoliday[7].ToString() == "NaN") rowWorkOnNationalHoliday[7] = 0;
        //        if (rowOvertimeToJustify[7].ToString() == "NaN") rowOvertimeToJustify[7] = 0;
        //        if (rowNew[7].ToString() == "NaN") rowNew[7] = 0;
        //        if (rowProductiveLeaves[7].ToString() == "NaN") rowProductiveLeaves[7] = 0;
        //        if (rowNoWorkRecoveryH[7].ToString() == "NaN") rowNoWorkRecoveryH[7] = 0;
        //        if (rowSchoolLeaves[7].ToString() == "NaN") rowSchoolLeaves[7] = 0;
        //        if (rowUnionLeaves[7].ToString() == "NaN") rowUnionLeaves[7] = 0;
        //        if (rowUsedBankH[7].ToString() == "NaN") rowUsedBankH[7] = 0;
        //        if (rowHoliday[7].ToString() == "NaN") rowHoliday[7] = 0;
        //        if (rowNoWork[7].ToString() == "NaN") rowNoWork[7] = 0;
        //        if (rowStrike[7].ToString() == "NaN") rowStrike[7] = 0;
        //        if (rowAbsentism[7].ToString() == "NaN") rowAbsentism[7] = 0;
        //        if (rowOvertimeBank[7].ToString() == "NaN") rowOvertimeBank[7] = 0;
        //        if (rowOvertime[7].ToString() == "NaN") rowOvertime[7] = 0;
        //        if (rowNotJustified[7].ToString() == "NaN") rowNotJustified[7] = 0;
        //        if (row[7].ToString() == "NaN") row[7] = 0;

        //        double totalDPercent = double.Parse(rowRegularWork[3].ToString()) + double.Parse(rowBusinessTrip[3].ToString()) + double.Parse(rowAbsenceDuringDay[3].ToString()) + double.Parse(rowWorkOnNationalHoliday[3].ToString()) + double.Parse(rowOvertimeToJustify[3].ToString()) + double.Parse(rowProductiveLeaves[3].ToString()) + double.Parse(rowNew[3].ToString()) + double.Parse(rowNoWorkRecoveryH[3].ToString()) + double.Parse(rowSchoolLeaves[3].ToString()) + double.Parse(rowUnionLeaves[3].ToString())
        //           + double.Parse(rowUsedBankH[3].ToString()) + double.Parse(rowHoliday[3].ToString()) + double.Parse(rowNoWork[3].ToString()) + double.Parse(rowStrike[3].ToString()) + double.Parse(rowAbsentism[3].ToString())
        //           + double.Parse(rowOvertime[3].ToString()) + double.Parse(rowOvertimeBank[3].ToString()) + double.Parse(rowNotJustified[3].ToString());

        //        double totalIPercent = double.Parse(rowRegularWork[5].ToString()) + double.Parse(rowBusinessTrip[5].ToString()) + double.Parse(rowAbsenceDuringDay[5].ToString()) + double.Parse(rowWorkOnNationalHoliday[5].ToString()) + double.Parse(rowOvertimeToJustify[5].ToString()) + double.Parse(rowProductiveLeaves[5].ToString()) + double.Parse(rowNew[5].ToString()) + double.Parse(rowNoWorkRecoveryH[5].ToString()) + double.Parse(rowSchoolLeaves[5].ToString()) + double.Parse(rowUnionLeaves[5].ToString())
        //              + double.Parse(rowUsedBankH[5].ToString()) + double.Parse(rowHoliday[5].ToString()) + double.Parse(rowNoWork[5].ToString()) + double.Parse(rowStrike[5].ToString()) + double.Parse(rowAbsentism[5].ToString())
        //              + double.Parse(rowOvertime[5].ToString()) + double.Parse(rowOvertimeBank[5].ToString()) + double.Parse(rowNotJustified[5].ToString());
        //        ;



        //        row[3] = totalDPercent;
        //        row[5] = totalIPercent;

        //        double ts = double.Parse(rowTotal[2].ToString());
        //        double tsi = double.Parse(rowTotal[4].ToString());
        //        if (ts != 0) rowTotal[3] = 0;
        //        if (tsi != 0) rowTotal[5] = 0;

        //        rowTotal[7] = 0;
        //        Misc.roundOn2(rowRegularWork);
        //        Misc.roundOn2(rowBusinessTrip);
        //        Misc.roundOn2(rowAbsenceDuringDay);
        //        Misc.roundOn2(rowWorkOnNationalHoliday);
        //        Misc.roundOn2(rowOvertimeToJustify);
        //        Misc.roundOn2(rowNew);
        //        Misc.roundOn2(rowProductiveLeaves);
        //        Misc.roundOn2(rowNoWorkRecoveryH);
        //        Misc.roundOn2(rowSchoolLeaves);
        //        Misc.roundOn2(rowUnionLeaves);
        //        Misc.roundOn2(rowUsedBankH);
        //        Misc.roundOn2(rowHoliday);
        //        Misc.roundOn2(rowNoWork);
        //        Misc.roundOn2(rowStrike);
        //        Misc.roundOn2(rowAbsentism);
        //        Misc.roundOn2(rowOvertimeBank);
        //        Misc.roundOn2(rowOvertime);
        //        Misc.roundOn2(row);
        //        Misc.roundOn2(rowTotal);
        //        Misc.roundOn2(rowTotalDiff);
        //        Misc.roundOn2(rowNotJustified);

        //        dt.Rows.Add(rowNew);
        //        dt.Rows.Add(rowRegularWork);
        //        dt.Rows.Add(rowBusinessTrip);
        //        dt.Rows.Add(rowAbsenceDuringDay);
        //        dt.Rows.Add(rowWorkOnNationalHoliday);
        //        dt.Rows.Add(rowOvertimeToJustify);
        //        dt.Rows.Add(rowNotJustified);
        //        dt.Rows.Add(rowProductiveLeaves);
        //        dt.Rows.Add(rowNoWorkRecoveryH);
        //        dt.Rows.Add(rowSchoolLeaves);
        //        dt.Rows.Add(rowUnionLeaves);
        //        dt.Rows.Add(rowUsedBankH);
        //        dt.Rows.Add(rowHoliday);
        //        dt.Rows.Add(rowNoWork);
        //        dt.Rows.Add(rowStrike);
        //        dt.Rows.Add(rowAbsentism);
        //        dt.Rows.Add(rowOvertimeBank);
        //        dt.Rows.Add(rowOvertime);
        //        DataRow rowEmptyBottom = dt.NewRow();
        //        dt.Rows.Add(rowEmptyBottom);
        //        dt.Rows.Add(row);
        //        dt.Rows.Add(rowTotal);
        //        dt.Rows.Add(rowTotalDiff);

        //        dt.AcceptChanges();

        //    }
        //    catch (Exception ex)
        //    {
        //        debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + ".populateTable400() " + ex.Message);

        //    }

        //}
        
        //private void populateDataTable(Object dbConnection, DataTable dt, List<DateTime> datesList, WorkingUnitTO company, List<IOPairProcessedTO> IOPairListDirect, List<IOPairProcessedTO> IOPairListIndirect, string emplIdsBS, string emplIds, string qualify, WorkingUnitTO workingUnit, string title
        //      , Dictionary<int, PassTypeTO> passTypesDictionaryAll, Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDictionary)
        //{
        //    try
        //    {

        //        lunchTypes = new Dictionary<int, Dictionary<int, decimal>>();

        //        //get all time Schemas
        //        Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema().getDictionary();
        //        Dictionary<int, List<EmployeeTimeScheduleTO>> directEmplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIdsBS, datesList[0].AddDays(-1), datesList[datesList.Count - 1], null);
        //        Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>> directEmplDayIntervals = new Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>>();
        //        Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>> directEmplDaySchemas = new Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>>();


        //        DateTime currDate = datesList[0];

        //        if (emplIdsBS.Trim().Length > 0)
        //        {
        //            foreach (string empl in emplIdsBS.Split(','))
        //            {
        //                while (currDate <= datesList[datesList.Count - 1].AddDays(1))
        //                {
        //                    int emplID = int.Parse(empl);
        //                    List<EmployeeTimeScheduleTO> emplScheduleList = new List<EmployeeTimeScheduleTO>();

        //                    if (directEmplSchedules.ContainsKey(emplID))
        //                        emplScheduleList = directEmplSchedules[emplID];

        //                    if (!directEmplDayIntervals.ContainsKey(emplID))
        //                        directEmplDayIntervals.Add(emplID, new Dictionary<DateTime, List<WorkTimeIntervalTO>>());

        //                    if (!directEmplDayIntervals[emplID].ContainsKey(currDate.Date))
        //                        directEmplDayIntervals[emplID].Add(currDate.Date, Common.Misc.getTimeSchemaInterval(currDate.Date, emplScheduleList, schemas));

        //                    if (!directEmplDaySchemas.ContainsKey(emplID))
        //                        directEmplDaySchemas.Add(emplID, new Dictionary<DateTime, WorkTimeSchemaTO>());

        //                    WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
        //                    if (directEmplDayIntervals[emplID][currDate.Date].Count > 0 && schemas.ContainsKey(directEmplDayIntervals[emplID][currDate.Date][0].TimeSchemaID))
        //                        sch = schemas[directEmplDayIntervals[emplID][currDate.Date][0].TimeSchemaID];

        //                    if (!directEmplDaySchemas[emplID].ContainsKey(currDate.Date))
        //                        directEmplDaySchemas[emplID].Add(currDate.Date, sch);

        //                    currDate = currDate.AddDays(1).Date;
        //                }
        //            }
        //        }
        //        Dictionary<int, List<EmployeeTimeScheduleTO>> indirectEmplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIds, datesList[0].AddDays(-1), datesList[datesList.Count - 1], null);
        //        Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>> indirectEmplDayIntervals = new Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>>();
        //        Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>> indirectEmplDaySchemas = new Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>>();


        //        currDate = datesList[0];
        //        if (emplIds.Trim().Length > 0)
        //        {
        //            foreach (string empl in emplIds.Split(','))
        //            {
        //                while (currDate <= datesList[datesList.Count - 1].AddDays(1))
        //                {
        //                    int emplID = int.Parse(empl);
        //                    List<EmployeeTimeScheduleTO> emplScheduleList = new List<EmployeeTimeScheduleTO>();

        //                    if (indirectEmplSchedules.ContainsKey(emplID))
        //                        emplScheduleList = indirectEmplSchedules[emplID];

        //                    if (!indirectEmplDayIntervals.ContainsKey(emplID))
        //                        indirectEmplDayIntervals.Add(emplID, new Dictionary<DateTime, List<WorkTimeIntervalTO>>());

        //                    if (!indirectEmplDayIntervals[emplID].ContainsKey(currDate.Date))
        //                        indirectEmplDayIntervals[emplID].Add(currDate.Date, Common.Misc.getTimeSchemaInterval(currDate.Date, emplScheduleList, schemas));

        //                    if (!indirectEmplDaySchemas.ContainsKey(emplID))
        //                        indirectEmplDaySchemas.Add(emplID, new Dictionary<DateTime, WorkTimeSchemaTO>());

        //                    WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
        //                    if (indirectEmplDayIntervals[emplID][currDate.Date].Count > 0 && schemas.ContainsKey(indirectEmplDayIntervals[emplID][currDate.Date][0].TimeSchemaID))
        //                        sch = schemas[indirectEmplDayIntervals[emplID][currDate.Date][0].TimeSchemaID];

        //                    if (!indirectEmplDaySchemas[emplID].ContainsKey(currDate.Date))
        //                        indirectEmplDaySchemas[emplID].Add(currDate.Date, sch);

        //                    currDate = currDate.AddDays(1).Date;
        //                }
        //            }
        //        }

        //        PassType passType;
        //        if (dbConnection == null)
        //        {

        //            passType = new PassType();
        //        }
        //        else
        //        {
        //            passType = new PassType(dbConnection);
        //        }
        //        Dictionary<int, PassTypeTO> listPassTypes = new Dictionary<int, PassTypeTO>();
        //        WorkingUnit wu;
        //        if (dbConnection == null)
        //            wu = new WorkingUnit();
        //        else
        //            wu = new WorkingUnit(dbConnection);
        //        string ute = "";
        //        string workGroup = "";
        //        string costCenter = "";
        //        string plant = "";

        //        DataRow rowHeader1 = dt.NewRow();

        //        rowHeader1[1] = company.Description;

        //        //rowHeader1[1] = "FIAT GROUP AUTOMOBILES S.P.A.";
        //        rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
        //        rowHeader1[7] = DateTime.Now.ToString("HH:mm");
        //        //rowHeader1[8] = "page: " + page;
        //        dt.Rows.Add(rowHeader1);

        //        DataRow rowHeader2 = dt.NewRow();
        //        rowHeader2[1] = title;
        //        rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
        //        rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
        //        dt.Rows.Add(rowHeader2);

        //        WorkingUnitTO tempWU = wu.FindWU(workingUnit.WorkingUnitID);
        //        ute = tempWU.Code.Trim();

        //        // get workshop (parent of UTE)
        //        wu.WUTO = tempWU;
        //        tempWU = wu.getParentWorkingUnit();
        //        workGroup = tempWU.Code.Trim();

        //        // get cost centar
        //        wu.WUTO = tempWU;
        //        tempWU = wu.getParentWorkingUnit();
        //        costCenter = tempWU.Code.Trim();

        //        // get plant
        //        wu.WUTO = tempWU;
        //        tempWU = wu.getParentWorkingUnit();
        //        plant = tempWU.Code.Trim();
        //        DataRow rowHeader3 = dt.NewRow();

        //        rowHeader3[1] = "Qualify:   " + qualify;
        //        dt.Rows.Add(rowHeader3);

        //        DataRow rowPlant = dt.NewRow();

        //        rowPlant[1] = "Plant:     " + plant + "     Cost center: " + costCenter;

        //        dt.Rows.Add(rowPlant);

        //        DataRow rowWorkgroup = dt.NewRow();

        //        rowWorkgroup[1] = "Workgroup: " + workGroup + "  UTE: " + ute;

        //        dt.Rows.Add(rowWorkgroup);


        //        DataRow rowEmpty = dt.NewRow();
        //        dt.Rows.Add(rowEmpty);

        //        DataRow rowColumns = dt.NewRow();
        //        rowColumns[2] = "Direct";
        //        rowColumns[3] = "%";
        //        rowColumns[4] = "Indirect";
        //        rowColumns[5] = "%";
        //        rowColumns[6] = "Total";
        //        rowColumns[7] = "%";
        //        dt.Rows.Add(rowColumns);

        //        DataRow rowLunchBreak = dt.NewRow();
        //        rowLunchBreak[1] = "Lunch break";

        //        decimal workOnHolidaysDirect = CalcLunchBreak(emplIdsBS, company.WorkingUnitID, IOPairListDirect, datesList[0], datesList[datesList.Count - 1], dbConnection, passTypesDictionaryAll, rulesDictionary, 0);
        //        decimal workOnHolidaysIndirect = CalcLunchBreak(emplIds, company.WorkingUnitID, IOPairListIndirect, datesList[0], datesList[datesList.Count - 1], dbConnection, passTypesDictionaryAll, rulesDictionary, 1);
        //        rowLunchBreak["Direct"] = workOnHolidaysDirect;
        //        rowLunchBreak["Indirect"] = workOnHolidaysIndirect;
        //        rowLunchBreak["Total"] = workOnHolidaysIndirect + workOnHolidaysDirect;

        //        DataRow rowNotJustified = dt.NewRow();
        //        listPassTypes = passType.Find(Constants.absence.ToString(), company.WorkingUnitID);


        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Unjustified absences", rowNotJustified, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else calc400Empty("Unjustified absences", rowNotJustified);

        //        DataRow rowTraining = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("1406", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Training", rowTraining, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else
        //            calc400Empty("Training", rowTraining);

        //        DataRow rowRegularWork = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0012", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {
        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Regular work", rowRegularWork, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else
        //            calc400Empty("Regular work", rowRegularWork);




        //        DataRow rowBusinessTrip = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0014", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Business trip", rowBusinessTrip, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else
        //            calc400Empty("Business trip", rowBusinessTrip);


        //        DataRow rowAbsenceDuringDay = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0071", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Absence during the day for business purposes", rowAbsenceDuringDay, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else
        //            calc400Empty("Absence during the day for business purposes", rowAbsenceDuringDay);


        //        DataRow rowAbsencesOther = dt.NewRow();



        //        if (company.WorkingUnitID == -2)
        //        {
        //            listPassTypes = passType.Find("84,21,79,56,71,74,75,57,76,78,77,23,29,30,31,32,33,34,35,36,38,14,80,59,54,55,81,53,65,72,82,41,24,17,18,67,68,19,20,69,70", company.WorkingUnitID);
        //        }
        //        else if (company.WorkingUnitID == -3)
        //        {
        //            listPassTypes = passType.Find("1021,1079,1078,1077,1023,1029,1031,1032,1034,1035,1038,1014,1080,1059,1055,1053,1065,1072,1082,1041,1024,1017,1018,1067,1068,1020,1070,1071,1056,1074,1075", company.WorkingUnitID);
        //        }
        //        else if (company.WorkingUnitID == -4)
        //        {
        //            listPassTypes = passType.Find("2021,2079,2078,2077,2023,2029,2031,2032,2034,2035,2038,2014,2080,2059,2055,2053,2056,2065,2071,2072,2074,2075,2082,2041,2024,2017,2018,2067,2068,2020,2070", company.WorkingUnitID);
        //        }
        //        else
        //        {
        //            listPassTypes = passType.Find("3021,3079,3078,3077,3023,3029,3031,3032,3034,3035,3038,3014,3080,3059,3055,3056,3053,3065,3072,3074,3075,3082,3041,3024,3017,3018,3067,3068,3020,3070", company.WorkingUnitID);
        //        }

        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Absences - Others", rowAbsencesOther, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else
        //            calc400Empty("Absences - Others", rowAbsencesOther);



        //        DataRow rowOvertimeNotPaid = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0130", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Overtime not paid", rowOvertimeNotPaid, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else
        //            calc400Empty("Overtime not paid", rowOvertimeNotPaid);

        //        DataRow rowOvertime = dt.NewRow();
        //        if (company.WorkingUnitID == -2)
        //        {
        //            listPassTypes = passType.Find("48,-1000,4,86,88", company.WorkingUnitID);
        //        }
        //        else if (company.WorkingUnitID == -3)
        //        {
        //            listPassTypes = passType.Find("1048,-1000,1004,1088", company.WorkingUnitID);
        //        }
        //        else if (company.WorkingUnitID == -4)
        //        {
        //            listPassTypes = passType.Find("2048,-1000,2004,2088", company.WorkingUnitID);
        //        }
        //        else
        //        {
        //            listPassTypes = passType.Find("3048,-1000,3004,3088", company.WorkingUnitID);
        //        }


        //        if (listPassTypes.Count > 0)
        //        {
        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Overtime", rowOvertime, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else
        //            calc400Empty("Overtime", rowOvertime);

        //        DataRow rowIllness = dt.NewRow();
        //        if (company.WorkingUnitID == -2)
        //        {
        //            listPassTypes = passType.Find("26,40,25,27,62,43,63,73,39,28", company.WorkingUnitID);
        //        }
        //        else if (company.WorkingUnitID == -3)
        //        {
        //            listPassTypes = passType.Find("1026,1040,1025,1027,1062,1043,1063,1073,1039,1028", company.WorkingUnitID);
        //        }
        //        else if (company.WorkingUnitID == -4)
        //        {
        //            listPassTypes = passType.Find("2026,2040,2025,2027,2062,2043,2063,2073,2039,2028", company.WorkingUnitID);
        //        }
        //        else
        //        {
        //            listPassTypes = passType.Find("3026,3040,3025,3027,3062,3043,3063,3073,3039,3028", company.WorkingUnitID);
        //        }

        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Illness", rowIllness, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else calc400Empty("Illness", rowIllness);


        //        DataRow rowLackOfWork = dt.NewRow();
        //        listPassTypes = passType.Find("0053", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {
        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Lack of work (65%)", rowLackOfWork, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else calc400Empty("Lack of work (65%)", rowLackOfWork);

        //        DataRow rowOvertimeNotJustified = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0053", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Using overtime not justified", rowOvertimeNotJustified, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else
        //            calc400Empty("Using overtime not justified", rowOvertimeNotJustified);


        //        DataRow rowHoliday = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0040,0044", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Holiday", rowHoliday, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else
        //            calc400Empty("Holiday", rowHoliday);


        //        DataRow rowUsedBankH = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0212", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Used bank hours", rowUsedBankH, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else
        //            calc400Empty("Used bank hours", rowUsedBankH);

        //        DataRow rowJustifiedAbsence = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0060", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Justified absence", rowJustifiedAbsence, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else
        //            calc400Empty("Justified absence", rowJustifiedAbsence);


        //        DataRow rowBloodDonations = dt.NewRow();
        //        if (company.WorkingUnitID == -2)
        //        {
        //            listPassTypes = passType.Find("11,66", company.WorkingUnitID);
        //        }
        //        else if (company.WorkingUnitID == -3)
        //        {
        //            listPassTypes = passType.Find("1011,1066", company.WorkingUnitID);
        //        }
        //        else if (company.WorkingUnitID == -4)
        //        {
        //            listPassTypes = passType.Find("2011,2066", company.WorkingUnitID);
        //        }
        //        else
        //        {
        //            listPassTypes = passType.Find("3011,3066", company.WorkingUnitID);
        //        }
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Blood donation", rowBloodDonations, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else
        //            calc400Empty("Blood donation", rowBloodDonations);


        //        DataRow rowStopWorkingHours = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0512", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Stop working hours current month", rowStopWorkingHours, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else
        //            calc400Empty("Stop working hours current month", rowStopWorkingHours);


        //        DataRow rowProductiveRecovery = dt.NewRow();
        //        listPassTypes = passType.FindByPaymentCode("0612", company.WorkingUnitID);
        //        if (listPassTypes.Count > 0)
        //        {

        //            calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Productive recovery", rowProductiveRecovery, emplIdsBS, emplIds, datesList, directEmplDayIntervals, directEmplDaySchemas, indirectEmplDayIntervals, indirectEmplDaySchemas, passTypesDictionaryAll);
        //        }
        //        else
        //            calc400Empty("Productive recovery", rowProductiveRecovery);


        //        double totalDD = double.Parse(rowRegularWork[2].ToString()) + double.Parse(rowBusinessTrip[2].ToString()) + double.Parse(rowAbsenceDuringDay[2].ToString())
        //            + double.Parse(rowLunchBreak[2].ToString()) + double.Parse(rowAbsencesOther[2].ToString()) + double.Parse(rowProductiveRecovery[2].ToString())
        //            + double.Parse(rowTraining[2].ToString()) + double.Parse(rowStopWorkingHours[2].ToString()) + double.Parse(rowBloodDonations[2].ToString()) + double.Parse(rowJustifiedAbsence[2].ToString())
        //            + double.Parse(rowUsedBankH[2].ToString()) + double.Parse(rowHoliday[2].ToString()) + double.Parse(rowOvertimeNotJustified[2].ToString()) + double.Parse(rowLackOfWork[2].ToString()) + double.Parse(rowIllness[2].ToString())
        //            + double.Parse(rowOvertimeNotPaid[2].ToString()) + double.Parse(rowOvertime[2].ToString()) + double.Parse(rowNotJustified[2].ToString());

        //        double totalID = double.Parse(rowRegularWork[4].ToString()) + double.Parse(rowBusinessTrip[4].ToString()) + double.Parse(rowAbsenceDuringDay[4].ToString()) + double.Parse(rowLunchBreak[4].ToString()) + double.Parse(rowAbsencesOther[4].ToString()) + double.Parse(rowProductiveRecovery[4].ToString()) + double.Parse(rowTraining[4].ToString()) + double.Parse(rowStopWorkingHours[4].ToString()) + double.Parse(rowBloodDonations[4].ToString()) + double.Parse(rowJustifiedAbsence[4].ToString())
        //               + double.Parse(rowUsedBankH[4].ToString()) + double.Parse(rowHoliday[4].ToString()) + double.Parse(rowOvertimeNotJustified[4].ToString()) + double.Parse(rowLackOfWork[4].ToString()) + double.Parse(rowIllness[4].ToString())
        //               + double.Parse(rowOvertimeNotPaid[4].ToString()) + double.Parse(rowOvertime[4].ToString()) + double.Parse(rowNotJustified[4].ToString());

        //        double total = totalDD + totalID;
        //        DataRow row = dt.NewRow();
        //        row[1] = "Total";
        //        row[2] = totalDD;
        //        row[4] = totalID;
        //        row[6] = total;
        //        row[5] = 0;
        //        row[3] = 0;
        //        DataRow rowTotal = dt.NewRow();
        //        rowTotal[1] = "Planned";
        //        if (emplIdsBS.Length > 0)
        //            rowTotal[2] = Misc.CalcPlannedTime(dbConnection, emplIdsBS, datesList[0], datesList[datesList.Count - 1]);
        //        else
        //            rowTotal[2] = 0;
        //        if (emplIds.Length > 0)
        //            rowTotal[4] = Misc.CalcPlannedTime(dbConnection, emplIds, datesList[0], datesList[datesList.Count - 1]);
        //        else
        //            rowTotal[4] = 0;

        //        rowTotal[6] = double.Parse(rowTotal[2].ToString()) + double.Parse(rowTotal[4].ToString());
        //        rowTotal[5] = 0;
        //        rowTotal[3] = 0;

        //        DataRow rowTotalDiff = dt.NewRow();
        //        rowTotalDiff[1] = "+/-";
        //        rowTotalDiff[3] = "  ";
        //        rowTotalDiff[5] = "  ";
        //        rowTotalDiff[7] = "  ";
        //        rowTotalDiff[2] = double.Parse(row[2].ToString()) - double.Parse(rowTotal[2].ToString());
        //        rowTotalDiff[4] = double.Parse(row[4].ToString()) - double.Parse(rowTotal[4].ToString());
        //        rowTotalDiff[6] = double.Parse(row[6].ToString()) - double.Parse(rowTotal[6].ToString());

        //        Misc.percentage(rowRegularWork, totalDD, totalID);
        //        Misc.percentage(rowBusinessTrip, totalDD, totalID);
        //        Misc.percentage(rowAbsenceDuringDay, totalDD, totalID);
        //        Misc.percentage(rowLunchBreak, totalDD, totalID);
        //        Misc.percentage(rowAbsencesOther, totalDD, totalID);
        //        Misc.percentage(rowTraining, totalDD, totalID);
        //        Misc.percentage(rowProductiveRecovery, totalDD, totalID);
        //        Misc.percentage(rowStopWorkingHours, totalDD, totalID);
        //        Misc.percentage(rowBloodDonations, totalDD, totalID);
        //        Misc.percentage(rowJustifiedAbsence, totalDD, totalID);
        //        Misc.percentage(rowUsedBankH, totalDD, totalID);
        //        Misc.percentage(rowHoliday, totalDD, totalID);
        //        Misc.percentage(rowOvertimeNotJustified, totalDD, totalID);
        //        Misc.percentage(rowLackOfWork, totalDD, totalID);
        //        Misc.percentage(rowIllness, totalDD, totalID);
        //        Misc.percentage(rowOvertime, totalDD, totalID);
        //        Misc.percentage(rowOvertimeNotPaid, totalDD, totalID);
        //        Misc.percentage(rowNotJustified, totalDD, totalID);


        //        rowRegularWork[7] = double.Parse(rowRegularWork[6].ToString()) * 100 / total;
        //        rowBusinessTrip[7] = double.Parse(rowBusinessTrip[6].ToString()) * 100 / total;
        //        rowAbsenceDuringDay[7] = double.Parse(rowAbsenceDuringDay[6].ToString()) * 100 / total;
        //        rowLunchBreak[7] = double.Parse(rowLunchBreak[6].ToString()) * 100 / total;
        //        rowAbsencesOther[7] = double.Parse(rowAbsencesOther[6].ToString()) * 100 / total;
        //        rowTraining[7] = double.Parse(rowTraining[6].ToString()) * 100 / total;
        //        rowProductiveRecovery[7] = double.Parse(rowProductiveRecovery[6].ToString()) * 100 / total;
        //        rowStopWorkingHours[7] = double.Parse(rowStopWorkingHours[6].ToString()) * 100 / total;
        //        rowBloodDonations[7] = double.Parse(rowBloodDonations[6].ToString()) * 100 / total;
        //        rowJustifiedAbsence[7] = double.Parse(rowJustifiedAbsence[6].ToString()) * 100 / total;
        //        rowUsedBankH[7] = double.Parse(rowUsedBankH[6].ToString()) * 100 / total;
        //        rowHoliday[7] = double.Parse(rowHoliday[6].ToString()) * 100 / total;
        //        rowOvertimeNotJustified[7] = double.Parse(rowOvertimeNotJustified[6].ToString()) * 100 / total;
        //        rowLackOfWork[7] = double.Parse(rowLackOfWork[6].ToString()) * 100 / total;
        //        rowIllness[7] = double.Parse(rowIllness[6].ToString()) * 100 / total;
        //        rowOvertime[7] = double.Parse(rowOvertime[6].ToString()) * 100 / total;
        //        rowOvertimeNotPaid[7] = double.Parse(rowOvertimeNotPaid[6].ToString()) * 100 / total;
        //        rowNotJustified[7] = double.Parse(rowNotJustified[6].ToString()) * 100 / total;
        //        row[7] = double.Parse(row[6].ToString()) * 100 / total;


        //        if (rowRegularWork[7].ToString() == "NaN") rowRegularWork[7] = 0;
        //        if (rowBusinessTrip[7].ToString() == "NaN") rowBusinessTrip[7] = 0;
        //        if (rowAbsenceDuringDay[7].ToString() == "NaN") rowAbsenceDuringDay[7] = 0;
        //        if (rowLunchBreak[7].ToString() == "NaN") rowLunchBreak[7] = 0;
        //        if (rowAbsencesOther[7].ToString() == "NaN") rowAbsencesOther[7] = 0;
        //        if (rowTraining[7].ToString() == "NaN") rowTraining[7] = 0;
        //        if (rowProductiveRecovery[7].ToString() == "NaN") rowProductiveRecovery[7] = 0;
        //        if (rowStopWorkingHours[7].ToString() == "NaN") rowStopWorkingHours[7] = 0;
        //        if (rowBloodDonations[7].ToString() == "NaN") rowBloodDonations[7] = 0;
        //        if (rowJustifiedAbsence[7].ToString() == "NaN") rowJustifiedAbsence[7] = 0;
        //        if (rowUsedBankH[7].ToString() == "NaN") rowUsedBankH[7] = 0;
        //        if (rowHoliday[7].ToString() == "NaN") rowHoliday[7] = 0;
        //        if (rowOvertimeNotJustified[7].ToString() == "NaN") rowOvertimeNotJustified[7] = 0;
        //        if (rowLackOfWork[7].ToString() == "NaN") rowLackOfWork[7] = 0;
        //        if (rowIllness[7].ToString() == "NaN") rowIllness[7] = 0;
        //        if (rowOvertime[7].ToString() == "NaN") rowOvertime[7] = 0;
        //        if (rowOvertimeNotPaid[7].ToString() == "NaN") rowOvertimeNotPaid[7] = 0;
        //        if (rowNotJustified[7].ToString() == "NaN") rowNotJustified[7] = 0;
        //        if (row[7].ToString() == "NaN") row[7] = 0;

        //        double totalDPercent = double.Parse(rowRegularWork[3].ToString()) + double.Parse(rowBusinessTrip[3].ToString()) + double.Parse(rowAbsenceDuringDay[3].ToString()) + double.Parse(rowLunchBreak[3].ToString()) + double.Parse(rowAbsencesOther[3].ToString()) + double.Parse(rowProductiveRecovery[3].ToString()) + double.Parse(rowTraining[3].ToString()) + double.Parse(rowStopWorkingHours[3].ToString()) + double.Parse(rowBloodDonations[3].ToString()) + double.Parse(rowJustifiedAbsence[3].ToString())
        //           + double.Parse(rowUsedBankH[3].ToString()) + double.Parse(rowHoliday[3].ToString()) + double.Parse(rowOvertimeNotJustified[3].ToString()) + double.Parse(rowLackOfWork[3].ToString()) + double.Parse(rowIllness[3].ToString())
        //           + double.Parse(rowOvertimeNotPaid[3].ToString()) + double.Parse(rowOvertime[3].ToString()) + double.Parse(rowNotJustified[3].ToString());

        //        double totalIPercent = double.Parse(rowRegularWork[5].ToString()) + double.Parse(rowBusinessTrip[5].ToString()) + double.Parse(rowAbsenceDuringDay[5].ToString()) + double.Parse(rowLunchBreak[5].ToString()) + double.Parse(rowAbsencesOther[5].ToString()) + double.Parse(rowProductiveRecovery[5].ToString()) + double.Parse(rowTraining[5].ToString()) + double.Parse(rowStopWorkingHours[5].ToString()) + double.Parse(rowBloodDonations[5].ToString()) + double.Parse(rowJustifiedAbsence[5].ToString())
        //              + double.Parse(rowUsedBankH[5].ToString()) + double.Parse(rowHoliday[5].ToString()) + double.Parse(rowOvertimeNotJustified[5].ToString()) + double.Parse(rowLackOfWork[5].ToString()) + double.Parse(rowIllness[5].ToString())
        //              + double.Parse(rowOvertimeNotPaid[5].ToString()) + double.Parse(rowOvertime[5].ToString()) + double.Parse(rowNotJustified[5].ToString());


        //        row[3] = totalDPercent;
        //        row[5] = totalIPercent;

        //        double ts = double.Parse(rowTotal[2].ToString());
        //        double tsi = double.Parse(rowTotal[4].ToString());
        //        if (ts != 0) rowTotal[3] = 0;
        //        if (tsi != 0) rowTotal[5] = 0;

        //        rowTotal[7] = 0;
        //        Misc.roundOn2(rowRegularWork);
        //        Misc.roundOn2(rowBusinessTrip);
        //        Misc.roundOn2(rowAbsenceDuringDay);
        //        Misc.roundOn2(rowLunchBreak);
        //        Misc.roundOn2(rowAbsencesOther);
        //        Misc.roundOn2(rowTraining);
        //        Misc.roundOn2(rowProductiveRecovery);
        //        Misc.roundOn2(rowStopWorkingHours);
        //        Misc.roundOn2(rowBloodDonations);
        //        Misc.roundOn2(rowJustifiedAbsence);
        //        Misc.roundOn2(rowUsedBankH);
        //        Misc.roundOn2(rowHoliday);
        //        Misc.roundOn2(rowOvertimeNotJustified);
        //        Misc.roundOn2(rowLackOfWork);
        //        Misc.roundOn2(rowIllness);
        //        Misc.roundOn2(rowOvertime);
        //        Misc.roundOn2(rowOvertimeNotPaid);
        //        Misc.roundOn2(row);
        //        Misc.roundOn2(rowTotal);
        //        Misc.roundOn2(rowTotalDiff);
        //        Misc.roundOn2(rowNotJustified);


        //        dt.Rows.Add(rowRegularWork);
        //        dt.Rows.Add(rowBusinessTrip);
        //        dt.Rows.Add(rowAbsenceDuringDay);
        //        dt.Rows.Add(rowTraining);
        //        dt.Rows.Add(rowLunchBreak);
        //        dt.Rows.Add(rowAbsencesOther);
        //        dt.Rows.Add(rowNotJustified);
        //        dt.Rows.Add(rowProductiveRecovery);
        //        dt.Rows.Add(rowStopWorkingHours);
        //        dt.Rows.Add(rowBloodDonations);
        //        dt.Rows.Add(rowJustifiedAbsence);
        //        dt.Rows.Add(rowUsedBankH);
        //        dt.Rows.Add(rowHoliday);
        //        dt.Rows.Add(rowOvertimeNotJustified);
        //        dt.Rows.Add(rowLackOfWork);
        //        dt.Rows.Add(rowIllness);
        //        dt.Rows.Add(rowOvertime);
        //        dt.Rows.Add(rowOvertimeNotPaid);
        //        DataRow rowEmptyBottom = dt.NewRow();
        //        dt.Rows.Add(rowEmptyBottom);
        //        dt.Rows.Add(row);
        //        dt.Rows.Add(rowTotal);
        //        dt.Rows.Add(rowTotalDiff);

        //        dt.AcceptChanges();

        //    }
        //    catch (Exception ex)
        //    {
        //        debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + ".populateTable400() " + ex.Message);

        //    }

        //}
        
        private void populateDataTableNew(Object dbConnection, DataTable dt, List<DateTime> datesList, WorkingUnitTO company, string emplIdsBC, string emplIds, string qualify, WorkingUnitTO workingUnit, string title, uint py_calc_id)
        {
            try
            {

                List<EmployeePYDataAnaliticalTO> listIndirect = new EmployeePYDataAnalitical().Search(emplIds, "", py_calc_id, Constants.PYTypeEstimated);
                List<EmployeePYDataAnaliticalTO> listDirect = new EmployeePYDataAnalitical().Search(emplIdsBC, "", py_calc_id, Constants.PYTypeEstimated);
                if (listDirect.Count > 0 || listIndirect.Count > 0)
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

                    DataRow rowHeader1 = dt.NewRow();

                    rowHeader1[1] = company.Description;

                    //rowHeader1[1] = "FIAT GROUP AUTOMOBILES S.P.A.";
                    rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                    rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                    //rowHeader1[8] = "page: " + page;
                    dt.Rows.Add(rowHeader1);

                    DataRow rowHeader2 = dt.NewRow();
                    rowHeader2[1] = title;
                    rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                    rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                    dt.Rows.Add(rowHeader2);

                    WorkingUnitTO tempWU = wu.FindWU(workingUnit.WorkingUnitID);
                    ute = tempWU.Code.Trim();

                    // get workshop (parent of UTE)
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
                    DataRow rowHeader3 = dt.NewRow();

                    rowHeader3[1] = "Qualify:   " + qualify;
                    dt.Rows.Add(rowHeader3);

                    DataRow rowPlant = dt.NewRow();

                    rowPlant[1] = "Plant:     " + plant + "     Cost center: " + costCenter;

                    dt.Rows.Add(rowPlant);

                    DataRow rowWorkgroup = dt.NewRow();

                    rowWorkgroup[1] = "Workgroup: " + workGroup + "  UTE: " + ute;

                    dt.Rows.Add(rowWorkgroup);


                    DataRow rowEmpty = dt.NewRow();
                    dt.Rows.Add(rowEmpty);

                    DataRow rowColumns = dt.NewRow();
                    rowColumns[2] = "Direct";
                    rowColumns[3] = "%";
                    rowColumns[4] = "Indirect";
                    rowColumns[5] = "%";
                    rowColumns[6] = "Total";
                    rowColumns[7] = "%";
                    dt.Rows.Add(rowColumns);
                    string payment_code = "";

                    DataRow rowLunchBreak = dt.NewRow();
                    calc400New("Lunch break", rowLunchBreak, emplIdsBC, emplIds, "'0041'", py_calc_id);

                    DataRow rowNotJustified = dt.NewRow();
                    calc400New("Unjustified absences", rowNotJustified, emplIdsBC, emplIds, "'0000'", py_calc_id);

                    DataRow rowNotJustifiedClosure = dt.NewRow();
                    calc400New("Unjustified absences - closure", rowNotJustifiedClosure, emplIdsBC, emplIds, "'0000" + Constants.FiatClosurePaymentCode + "'", py_calc_id);

                    DataRow rowNotJustifiedLayoff = dt.NewRow();
                    calc400New("Unjustified absences - layoff", rowNotJustifiedLayoff, emplIdsBC, emplIds, "'0000" + Constants.FiatLayOffPaymentCode + "'", py_calc_id);

                    DataRow rowNotJustifiedStoppage = dt.NewRow();
                    calc400New("Unjustified absences - stoppage", rowNotJustifiedStoppage, emplIdsBC, emplIds, "'0000" + Constants.FiatStoppagePaymentCode + "'", py_calc_id);

                    DataRow rowNotJustifiedHoliday = dt.NewRow();
                    calc400New("Unjustified absences - public holiday", rowNotJustifiedHoliday, emplIdsBC, emplIds, "'0000" + Constants.FiatPublicHollidayPaymentCode + "'", py_calc_id);

                    DataRow rowTraining = dt.NewRow();
                    calc400New("Training", rowTraining, emplIdsBC, emplIds, "'1406'", py_calc_id);

                    DataRow rowRegularWork = dt.NewRow();
                    calc400New("Regular work", rowRegularWork, emplIdsBC, emplIds, "'0012'", py_calc_id);

                    DataRow rowBusinessTrip = dt.NewRow();
                    calc400New("Business trip", rowBusinessTrip, emplIdsBC, emplIds, "'0014'", py_calc_id);

                    DataRow rowAbsenceDuringDay = dt.NewRow();
                    calc400New("Absence during the day for business purposes", rowAbsenceDuringDay, emplIdsBC, emplIds, "'0071'", py_calc_id);

                    DataRow rowAbsencesOther = dt.NewRow();
                    payment_code = "'0045','0046','0047','0048','0056','0070','0169','0171','0269','0369','1145','1148','1155','1407','0055','0075','0112','0144','0145','0146','0147','0148','0149','0155','1150','0049','1149','1157','1144','1045'";
                    calc400New("Absences - Others", rowAbsencesOther, emplIdsBC, emplIds, payment_code, py_calc_id);

                    DataRow rowAbsencesOtherClosure = dt.NewRow();
                    payment_code = "'0045" + Constants.FiatClosurePaymentCode+ "','0046" + Constants.FiatClosurePaymentCode + "','0047" + Constants.FiatClosurePaymentCode 
                       + "','0048" + Constants.FiatClosurePaymentCode + "','0056" + Constants.FiatClosurePaymentCode + "','0070" + Constants.FiatClosurePaymentCode + "','0169" + Constants.FiatClosurePaymentCode 
                       + "','0171" + Constants.FiatClosurePaymentCode + "','0269" + Constants.FiatClosurePaymentCode + "','0369" + Constants.FiatClosurePaymentCode + "','1145" + Constants.FiatClosurePaymentCode 
                       + "','1148" + Constants.FiatClosurePaymentCode + "','1155" + Constants.FiatClosurePaymentCode + "','1407" + Constants.FiatClosurePaymentCode + "','0055" + Constants.FiatClosurePaymentCode 
                       + "','0075" + Constants.FiatClosurePaymentCode + "','0112" + Constants.FiatClosurePaymentCode + "','0144" + Constants.FiatClosurePaymentCode + "','0145" + Constants.FiatClosurePaymentCode 
                       + "','0146" + Constants.FiatClosurePaymentCode + "','0147" + Constants.FiatClosurePaymentCode + "','0148" + Constants.FiatClosurePaymentCode + "','0149" + Constants.FiatClosurePaymentCode 
                       + "','0155" + Constants.FiatClosurePaymentCode + "','1150" + Constants.FiatClosurePaymentCode + "','0049" + Constants.FiatClosurePaymentCode + "','1149" + Constants.FiatClosurePaymentCode
                       + "','1157" + Constants.FiatClosurePaymentCode + "','1144" + Constants.FiatClosurePaymentCode + "','1045" + Constants.FiatClosurePaymentCode + "'";
                    calc400New("Absences - Others - closure", rowAbsencesOtherClosure, emplIdsBC, emplIds, payment_code, py_calc_id);

                    DataRow rowAbsencesOtherLayoff = dt.NewRow();
                    payment_code = "'0045" + Constants.FiatLayOffPaymentCode + "','0046" + Constants.FiatLayOffPaymentCode + "','0047" + Constants.FiatLayOffPaymentCode
                       + "','0048" + Constants.FiatLayOffPaymentCode + "','0056" + Constants.FiatLayOffPaymentCode + "','0070" + Constants.FiatLayOffPaymentCode + "','0169" + Constants.FiatLayOffPaymentCode
                       + "','0171" + Constants.FiatLayOffPaymentCode + "','0269" + Constants.FiatLayOffPaymentCode + "','0369" + Constants.FiatLayOffPaymentCode + "','1145" + Constants.FiatLayOffPaymentCode
                       + "','1148" + Constants.FiatLayOffPaymentCode + "','1155" + Constants.FiatLayOffPaymentCode + "','1407" + Constants.FiatLayOffPaymentCode + "','0055" + Constants.FiatLayOffPaymentCode
                       + "','0075" + Constants.FiatLayOffPaymentCode + "','0112" + Constants.FiatLayOffPaymentCode + "','0144" + Constants.FiatLayOffPaymentCode + "','0145" + Constants.FiatLayOffPaymentCode
                       + "','0146" + Constants.FiatLayOffPaymentCode + "','0147" + Constants.FiatLayOffPaymentCode + "','0148" + Constants.FiatLayOffPaymentCode + "','0149" + Constants.FiatLayOffPaymentCode
                       + "','0155" + Constants.FiatLayOffPaymentCode + "','1150" + Constants.FiatLayOffPaymentCode + "','0049" + Constants.FiatLayOffPaymentCode + "','1149" + Constants.FiatLayOffPaymentCode
                       + "','1157" + Constants.FiatLayOffPaymentCode + "','1144" + Constants.FiatLayOffPaymentCode + "','1045" + Constants.FiatLayOffPaymentCode + "'";
                    calc400New("Absences - Others - layoff", rowAbsencesOtherLayoff, emplIdsBC, emplIds, payment_code, py_calc_id);

                    DataRow rowAbsencesOtherStoppage = dt.NewRow();
                    payment_code = "'0045" + Constants.FiatStoppagePaymentCode + "','0046" + Constants.FiatStoppagePaymentCode + "','0047" + Constants.FiatStoppagePaymentCode
                       + "','0048" + Constants.FiatStoppagePaymentCode + "','0056" + Constants.FiatStoppagePaymentCode + "','0070" + Constants.FiatStoppagePaymentCode + "','0169" + Constants.FiatStoppagePaymentCode
                       + "','0171" + Constants.FiatStoppagePaymentCode + "','0269" + Constants.FiatStoppagePaymentCode + "','0369" + Constants.FiatStoppagePaymentCode + "','1145" + Constants.FiatStoppagePaymentCode
                       + "','1148" + Constants.FiatStoppagePaymentCode + "','1155" + Constants.FiatStoppagePaymentCode + "','1407" + Constants.FiatStoppagePaymentCode + "','0055" + Constants.FiatStoppagePaymentCode
                       + "','0075" + Constants.FiatStoppagePaymentCode + "','0112" + Constants.FiatStoppagePaymentCode + "','0144" + Constants.FiatStoppagePaymentCode + "','0145" + Constants.FiatStoppagePaymentCode
                       + "','0146" + Constants.FiatStoppagePaymentCode + "','0147" + Constants.FiatStoppagePaymentCode + "','0148" + Constants.FiatStoppagePaymentCode + "','0149" + Constants.FiatStoppagePaymentCode
                       + "','0155" + Constants.FiatStoppagePaymentCode + "','1150" + Constants.FiatStoppagePaymentCode + "','0049" + Constants.FiatStoppagePaymentCode + "','1149" + Constants.FiatStoppagePaymentCode
                       + "','1157" + Constants.FiatStoppagePaymentCode + "','1144" + Constants.FiatStoppagePaymentCode + "','1045" + Constants.FiatStoppagePaymentCode + "'";
                    calc400New("Absences - Others - stoppage", rowAbsencesOtherStoppage, emplIdsBC, emplIds, payment_code, py_calc_id);

                    DataRow rowAbsencesOtherHoliday = dt.NewRow();
                    payment_code = "'0045" + Constants.FiatPublicHollidayPaymentCode + "','0046" + Constants.FiatPublicHollidayPaymentCode + "','0047" + Constants.FiatPublicHollidayPaymentCode
                       + "','0048" + Constants.FiatPublicHollidayPaymentCode + "','0056" + Constants.FiatPublicHollidayPaymentCode + "','0070" + Constants.FiatPublicHollidayPaymentCode + "','0169" + Constants.FiatPublicHollidayPaymentCode
                       + "','0171" + Constants.FiatPublicHollidayPaymentCode + "','0269" + Constants.FiatPublicHollidayPaymentCode + "','0369" + Constants.FiatPublicHollidayPaymentCode + "','1145" + Constants.FiatPublicHollidayPaymentCode
                       + "','1148" + Constants.FiatPublicHollidayPaymentCode + "','1155" + Constants.FiatPublicHollidayPaymentCode + "','1407" + Constants.FiatPublicHollidayPaymentCode + "','0055" + Constants.FiatPublicHollidayPaymentCode
                       + "','0075" + Constants.FiatPublicHollidayPaymentCode + "','0112" + Constants.FiatPublicHollidayPaymentCode + "','0144" + Constants.FiatPublicHollidayPaymentCode + "','0145" + Constants.FiatPublicHollidayPaymentCode
                       + "','0146" + Constants.FiatPublicHollidayPaymentCode + "','0147" + Constants.FiatPublicHollidayPaymentCode + "','0148" + Constants.FiatPublicHollidayPaymentCode + "','0149" + Constants.FiatPublicHollidayPaymentCode
                       + "','0155" + Constants.FiatPublicHollidayPaymentCode + "','1150" + Constants.FiatPublicHollidayPaymentCode + "','0049" + Constants.FiatPublicHollidayPaymentCode + "','1149" + Constants.FiatPublicHollidayPaymentCode
                       + "','1157" + Constants.FiatPublicHollidayPaymentCode + "','1144" + Constants.FiatPublicHollidayPaymentCode + "','1045" + Constants.FiatPublicHollidayPaymentCode + "'";
                    calc400New("Absences - Others - public holiday", rowAbsencesOtherHoliday, emplIdsBC, emplIds, payment_code, py_calc_id);

                    DataRow rowOvertimeNotPaid = dt.NewRow();
                    calc400New("Overtime not paid", rowOvertimeNotPaid, emplIdsBC, emplIds, "'0130'", py_calc_id);

                    DataRow rowOvertime = dt.NewRow();
                    calc400New("Overtime", rowOvertime, emplIdsBC, emplIds, "'0030'", py_calc_id);

                    DataRow rowIllness = dt.NewRow();
                    payment_code = "'0058','0156','0057','0060','0160','0257','1257','1160','0157','0061'";
                    calc400New("Illness", rowIllness, emplIdsBC, emplIds, payment_code, py_calc_id);

                    DataRow rowIllnessClosure = dt.NewRow();
                    payment_code = "'0058" + Constants.FiatClosurePaymentCode + "','0156" + Constants.FiatClosurePaymentCode + "','0057" + Constants.FiatClosurePaymentCode 
                        + "','0060" + Constants.FiatClosurePaymentCode + "','0160" + Constants.FiatClosurePaymentCode + "','0257" + Constants.FiatClosurePaymentCode 
                        + "','1257" + Constants.FiatClosurePaymentCode + "','1160" + Constants.FiatClosurePaymentCode + "','0157" + Constants.FiatClosurePaymentCode 
                        + "','0061" + Constants.FiatClosurePaymentCode + "'";
                    calc400New("Illness - closure", rowIllnessClosure, emplIdsBC, emplIds, payment_code, py_calc_id);

                    DataRow rowIllnessLayoff = dt.NewRow();
                    payment_code = "'0058" + Constants.FiatLayOffPaymentCode + "','0156" + Constants.FiatLayOffPaymentCode + "','0057" + Constants.FiatLayOffPaymentCode
                        + "','0060" + Constants.FiatLayOffPaymentCode + "','0160" + Constants.FiatLayOffPaymentCode + "','0257" + Constants.FiatLayOffPaymentCode
                        + "','1257" + Constants.FiatLayOffPaymentCode + "','1160" + Constants.FiatLayOffPaymentCode + "','0157" + Constants.FiatLayOffPaymentCode
                        + "','0061" + Constants.FiatLayOffPaymentCode + "'";
                    calc400New("Illness - layoff", rowIllnessLayoff, emplIdsBC, emplIds, payment_code, py_calc_id);

                    DataRow rowIllnessStoppage = dt.NewRow();
                    payment_code = "'0058" + Constants.FiatStoppagePaymentCode + "','0156" + Constants.FiatStoppagePaymentCode + "','0057" + Constants.FiatStoppagePaymentCode
                        + "','0060" + Constants.FiatStoppagePaymentCode + "','0160" + Constants.FiatStoppagePaymentCode + "','0257" + Constants.FiatStoppagePaymentCode
                        + "','1257" + Constants.FiatStoppagePaymentCode + "','1160" + Constants.FiatStoppagePaymentCode + "','0157" + Constants.FiatStoppagePaymentCode
                        + "','0061" + Constants.FiatStoppagePaymentCode + "'";
                    calc400New("Illness - stoppage", rowIllnessStoppage, emplIdsBC, emplIds, payment_code, py_calc_id);

                    DataRow rowIllnessHoliday = dt.NewRow();
                    payment_code = "'0058" + Constants.FiatPublicHollidayPaymentCode + "','0156" + Constants.FiatPublicHollidayPaymentCode + "','0057" + Constants.FiatPublicHollidayPaymentCode
                        + "','0060" + Constants.FiatPublicHollidayPaymentCode + "','0160" + Constants.FiatPublicHollidayPaymentCode + "','0257" + Constants.FiatPublicHollidayPaymentCode
                        + "','1257" + Constants.FiatPublicHollidayPaymentCode + "','1160" + Constants.FiatPublicHollidayPaymentCode + "','0157" + Constants.FiatPublicHollidayPaymentCode
                        + "','0061" + Constants.FiatPublicHollidayPaymentCode + "'";
                    calc400New("Illness - public holiday", rowIllnessHoliday, emplIdsBC, emplIds, payment_code, py_calc_id);

                    DataRow rowLackOfWork = dt.NewRow();
                    calc400New("Lack of work (65%)", rowLackOfWork, emplIdsBC, emplIds, "'0053'", py_calc_id);

                    DataRow rowOvertimeOthers = dt.NewRow();
                    calc400New("Overtime - Others", rowOvertimeOthers, emplIdsBC, emplIds, "'0412','1231','8888','9999'", py_calc_id);

                    DataRow rowHoliday = dt.NewRow();
                    calc400New("Holiday", rowHoliday, emplIdsBC, emplIds, "'0040'", py_calc_id);

                    DataRow rowCollectiveHoliday = dt.NewRow();
                    calc400New("Collective holiday", rowCollectiveHoliday, emplIdsBC, emplIds, "'" + Constants.FiatCollectiveAnnualLeavePaymentCode + "'", py_calc_id);

                    DataRow rowReligiousHoliday = dt.NewRow();
                    calc400New("Religious holiday", rowReligiousHoliday, emplIdsBC, emplIds, "'0044'", py_calc_id);

                    DataRow rowNationalHoliday = dt.NewRow();
                    calc400New("National holiday", rowNationalHoliday, emplIdsBC, emplIds, "'0042'", py_calc_id);

                    DataRow rowUsedBankH = dt.NewRow();
                    calc400New("Used bank hours", rowUsedBankH, emplIdsBC, emplIds, "'0212'", py_calc_id);

                    DataRow rowUsedBankH1 = dt.NewRow();
                    calc400New("Used bank hours -1", rowUsedBankH1, emplIdsBC, emplIds, "'2212'", py_calc_id);

                    DataRow rowJustifiedAbsence = dt.NewRow();
                    calc400New("Justified absence", rowJustifiedAbsence, emplIdsBC, emplIds, "'0069'", py_calc_id);

                    DataRow rowJustifiedAbsenceClosure = dt.NewRow();
                    calc400New("Justified absence - closure", rowJustifiedAbsenceClosure, emplIdsBC, emplIds, "'0069" + Constants.FiatClosurePaymentCode + "'", py_calc_id);

                    DataRow rowJustifiedAbsenceLayoff = dt.NewRow();
                    calc400New("Justified absence - layoff", rowJustifiedAbsenceLayoff, emplIdsBC, emplIds, "'0069" + Constants.FiatLayOffPaymentCode + "'", py_calc_id);

                    DataRow rowJustifiedAbsenceStoppage = dt.NewRow();
                    calc400New("Justified absence - stoppage", rowJustifiedAbsenceStoppage, emplIdsBC, emplIds, "'0069" + Constants.FiatStoppagePaymentCode + "'", py_calc_id);

                    DataRow rowJustifiedAbsenceHoliday = dt.NewRow();
                    calc400New("Justified absence - public holiday", rowJustifiedAbsenceHoliday, emplIdsBC, emplIds, "'0069" + Constants.FiatPublicHollidayPaymentCode + "'", py_calc_id);

                    DataRow rowBloodDonations = dt.NewRow();
                    calc400New("Blood donation", rowBloodDonations, emplIdsBC, emplIds, "'0043'", py_calc_id);

                    DataRow rowBloodDonationsClosure = dt.NewRow();
                    calc400New("Blood donation - closure", rowBloodDonationsClosure, emplIdsBC, emplIds, "'0043" + Constants.FiatClosurePaymentCode + "'", py_calc_id);

                    DataRow rowBloodDonationsLayoff = dt.NewRow();
                    calc400New("Blood donation - layoff", rowBloodDonationsLayoff, emplIdsBC, emplIds, "'0043" + Constants.FiatLayOffPaymentCode + "'", py_calc_id);

                    DataRow rowBloodDonationsStoppage = dt.NewRow();
                    calc400New("Blood donation - stoppage", rowBloodDonationsStoppage, emplIdsBC, emplIds, "'0043" + Constants.FiatStoppagePaymentCode + "'", py_calc_id);

                    DataRow rowBloodDonationsHoliday = dt.NewRow();
                    calc400New("Blood donation - public holiday", rowBloodDonationsHoliday, emplIdsBC, emplIds, "'0043" + Constants.FiatPublicHollidayPaymentCode + "'", py_calc_id);

                    DataRow rowTissueDonations = dt.NewRow();
                    calc400New("Tissue and organs donation", rowTissueDonations, emplIdsBC, emplIds, "'0357'", py_calc_id);

                    DataRow rowTissueDonationsClosure = dt.NewRow();
                    calc400New("Tissue and organs donation - closure", rowTissueDonationsClosure, emplIdsBC, emplIds, "'0357" + Constants.FiatClosurePaymentCode + "'", py_calc_id);

                    DataRow rowTissueDonationsLayoff = dt.NewRow();
                    calc400New("Tissue and organs donation - layoff", rowTissueDonationsLayoff, emplIdsBC, emplIds, "'0357" + Constants.FiatLayOffPaymentCode + "'", py_calc_id);

                    DataRow rowTissueDonationsStoppage = dt.NewRow();
                    calc400New("Tissue and organs donation - stoppage", rowTissueDonationsStoppage, emplIdsBC, emplIds, "'0357" + Constants.FiatStoppagePaymentCode + "'", py_calc_id);

                    DataRow rowTissueDonationsHoliday = dt.NewRow();
                    calc400New("Tissue and organs donation - public holiday", rowTissueDonationsHoliday, emplIdsBC, emplIds, "'0357" + Constants.FiatPublicHollidayPaymentCode + "'", py_calc_id);

                    DataRow rowStopWorkingHours = dt.NewRow();
                    calc400New("Stop working hours current month", rowStopWorkingHours, emplIdsBC, emplIds, "'0712'", py_calc_id);

                    DataRow rowProductiveRecovery = dt.NewRow();
                    calc400New("Productive recovery", rowProductiveRecovery, emplIdsBC, emplIds, "'0612'", py_calc_id);

                    double totalDD = double.Parse(rowRegularWork[2].ToString()) + double.Parse(rowBusinessTrip[2].ToString()) + double.Parse(rowAbsenceDuringDay[2].ToString())
                        + double.Parse(rowLunchBreak[2].ToString()) + double.Parse(rowAbsencesOther[2].ToString()) + double.Parse(rowAbsencesOtherClosure[2].ToString())
                        + double.Parse(rowAbsencesOtherLayoff[2].ToString()) + double.Parse(rowAbsencesOtherStoppage[2].ToString()) + double.Parse(rowAbsencesOtherHoliday[2].ToString())
                        + double.Parse(rowProductiveRecovery[2].ToString()) + double.Parse(rowTraining[2].ToString()) + double.Parse(rowStopWorkingHours[2].ToString())
                        + double.Parse(rowBloodDonations[2].ToString()) + double.Parse(rowBloodDonationsClosure[2].ToString()) + double.Parse(rowBloodDonationsLayoff[2].ToString())
                        + double.Parse(rowBloodDonationsStoppage[2].ToString()) + double.Parse(rowBloodDonationsHoliday[2].ToString())
                        + double.Parse(rowTissueDonations[2].ToString()) + double.Parse(rowTissueDonationsClosure[2].ToString()) + double.Parse(rowTissueDonationsLayoff[2].ToString())
                        + double.Parse(rowTissueDonationsStoppage[2].ToString()) + double.Parse(rowTissueDonationsHoliday[2].ToString())
                        + double.Parse(rowJustifiedAbsence[2].ToString()) + double.Parse(rowJustifiedAbsenceClosure[2].ToString()) + double.Parse(rowJustifiedAbsenceLayoff[2].ToString()) 
                        + double.Parse(rowJustifiedAbsenceStoppage[2].ToString()) + double.Parse(rowJustifiedAbsenceHoliday[2].ToString()) 
                        + double.Parse(rowUsedBankH[2].ToString()) + double.Parse(rowHoliday[2].ToString()) + double.Parse(rowCollectiveHoliday[2].ToString()) + double.Parse(rowReligiousHoliday[2].ToString())
                        + double.Parse(rowOvertimeOthers[2].ToString()) + double.Parse(rowLackOfWork[2].ToString()) + double.Parse(rowIllness[2].ToString()) + double.Parse(rowIllnessClosure[2].ToString())
                        + double.Parse(rowIllnessLayoff[2].ToString()) + double.Parse(rowIllnessStoppage[2].ToString()) + double.Parse(rowIllnessHoliday[2].ToString())
                        + double.Parse(rowOvertimeNotPaid[2].ToString()) + double.Parse(rowOvertime[2].ToString()) + double.Parse(rowNotJustified[2].ToString())
                        + double.Parse(rowNotJustifiedClosure[2].ToString()) + double.Parse(rowNotJustifiedLayoff[2].ToString()) + double.Parse(rowNotJustifiedStoppage[2].ToString()) 
                        + double.Parse(rowNotJustifiedHoliday[2].ToString()) + double.Parse(rowUsedBankH1[2].ToString()) + double.Parse(rowNationalHoliday[2].ToString());

                    double totalID = double.Parse(rowRegularWork[4].ToString()) + double.Parse(rowBusinessTrip[4].ToString()) + double.Parse(rowAbsenceDuringDay[4].ToString())
                        + double.Parse(rowLunchBreak[4].ToString()) + double.Parse(rowAbsencesOther[4].ToString()) + double.Parse(rowAbsencesOtherClosure[4].ToString())
                        + double.Parse(rowAbsencesOtherLayoff[4].ToString()) + double.Parse(rowAbsencesOtherStoppage[4].ToString()) + double.Parse(rowAbsencesOtherHoliday[4].ToString())
                        + double.Parse(rowProductiveRecovery[4].ToString()) + double.Parse(rowTraining[4].ToString()) + double.Parse(rowStopWorkingHours[4].ToString()) 
                        + double.Parse(rowBloodDonations[4].ToString()) + double.Parse(rowBloodDonationsClosure[4].ToString()) + double.Parse(rowBloodDonationsLayoff[4].ToString())
                        + double.Parse(rowBloodDonationsStoppage[4].ToString()) + double.Parse(rowBloodDonationsHoliday[4].ToString())
                        + double.Parse(rowTissueDonations[4].ToString()) + double.Parse(rowTissueDonationsClosure[4].ToString()) + double.Parse(rowTissueDonationsLayoff[4].ToString())
                        + double.Parse(rowTissueDonationsStoppage[4].ToString()) + double.Parse(rowTissueDonationsHoliday[4].ToString())
                        + double.Parse(rowJustifiedAbsence[4].ToString()) + double.Parse(rowJustifiedAbsenceClosure[4].ToString()) + double.Parse(rowJustifiedAbsenceLayoff[4].ToString()) 
                        + double.Parse(rowJustifiedAbsenceStoppage[4].ToString()) + double.Parse(rowJustifiedAbsenceHoliday[4].ToString()) + double.Parse(rowUsedBankH[4].ToString())
                        + double.Parse(rowHoliday[4].ToString()) + double.Parse(rowCollectiveHoliday[4].ToString()) + double.Parse(rowReligiousHoliday[4].ToString())
                        + double.Parse(rowOvertimeOthers[4].ToString()) + double.Parse(rowLackOfWork[4].ToString()) + double.Parse(rowIllness[4].ToString()) + double.Parse(rowIllnessClosure[4].ToString())
                        + double.Parse(rowIllnessLayoff[4].ToString()) + double.Parse(rowIllnessStoppage[4].ToString()) + double.Parse(rowIllnessHoliday[4].ToString())
                        + double.Parse(rowOvertimeNotPaid[4].ToString()) + double.Parse(rowOvertime[4].ToString()) + double.Parse(rowNotJustified[4].ToString())
                        + double.Parse(rowNotJustifiedClosure[4].ToString()) + double.Parse(rowNotJustifiedLayoff[4].ToString()) + double.Parse(rowNotJustifiedStoppage[4].ToString())
                        + double.Parse(rowNotJustifiedHoliday[4].ToString()) + double.Parse(rowUsedBankH1[4].ToString()) + double.Parse(rowNationalHoliday[4].ToString());

                    double total = totalDD + totalID;
                    DataRow row = dt.NewRow();
                    row[1] = "Total";
                    row[2] = totalDD;
                    row[4] = totalID;
                    row[6] = total;
                    row[5] = 0;
                    row[3] = 0;
                    DataRow rowTotal = dt.NewRow();
                    rowTotal[1] = "Planned";
                    if (emplIdsBC.Length > 0)
                        rowTotal[2] = Misc.CalcPlannedTime(dbConnection, emplIdsBC, datesList[0], datesList[datesList.Count - 1]);
                    else
                        rowTotal[2] = 0;
                    if (emplIds.Length > 0)
                        rowTotal[4] = Misc.CalcPlannedTime(dbConnection, emplIds, datesList[0], datesList[datesList.Count - 1]);
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

                    Misc.percentage(rowRegularWork, totalDD, totalID);
                    Misc.percentage(rowBusinessTrip, totalDD, totalID);
                    Misc.percentage(rowAbsenceDuringDay, totalDD, totalID);
                    Misc.percentage(rowLunchBreak, totalDD, totalID);
                    Misc.percentage(rowAbsencesOther, totalDD, totalID);
                    Misc.percentage(rowAbsencesOtherClosure, totalDD, totalID);
                    Misc.percentage(rowAbsencesOtherStoppage, totalDD, totalID);
                    Misc.percentage(rowAbsencesOtherLayoff, totalDD, totalID);
                    Misc.percentage(rowAbsencesOtherHoliday, totalDD, totalID);
                    Misc.percentage(rowTraining, totalDD, totalID);
                    Misc.percentage(rowProductiveRecovery, totalDD, totalID);
                    Misc.percentage(rowStopWorkingHours, totalDD, totalID);
                    Misc.percentage(rowBloodDonations, totalDD, totalID);
                    Misc.percentage(rowBloodDonationsClosure, totalDD, totalID);
                    Misc.percentage(rowBloodDonationsLayoff, totalDD, totalID);
                    Misc.percentage(rowBloodDonationsStoppage, totalDD, totalID);
                    Misc.percentage(rowBloodDonationsHoliday, totalDD, totalID);
                    Misc.percentage(rowTissueDonations, totalDD, totalID);
                    Misc.percentage(rowTissueDonationsClosure, totalDD, totalID);
                    Misc.percentage(rowTissueDonationsLayoff, totalDD, totalID);
                    Misc.percentage(rowTissueDonationsStoppage, totalDD, totalID);
                    Misc.percentage(rowTissueDonationsHoliday, totalDD, totalID);
                    Misc.percentage(rowJustifiedAbsence, totalDD, totalID);
                    Misc.percentage(rowJustifiedAbsenceClosure, totalDD, totalID);
                    Misc.percentage(rowJustifiedAbsenceLayoff, totalDD, totalID);
                    Misc.percentage(rowJustifiedAbsenceStoppage, totalDD, totalID);
                    Misc.percentage(rowJustifiedAbsenceHoliday, totalDD, totalID);
                    Misc.percentage(rowUsedBankH, totalDD, totalID);
                    Misc.percentage(rowHoliday, totalDD, totalID);
                    Misc.percentage(rowCollectiveHoliday, totalDD, totalID);
                    Misc.percentage(rowReligiousHoliday, totalDD, totalID);
                    Misc.percentage(rowOvertimeOthers, totalDD, totalID);
                    Misc.percentage(rowLackOfWork, totalDD, totalID);
                    Misc.percentage(rowIllness, totalDD, totalID);
                    Misc.percentage(rowIllnessClosure, totalDD, totalID);
                    Misc.percentage(rowIllnessLayoff, totalDD, totalID);
                    Misc.percentage(rowIllnessStoppage, totalDD, totalID);
                    Misc.percentage(rowIllnessHoliday, totalDD, totalID);
                    Misc.percentage(rowOvertime, totalDD, totalID);
                    Misc.percentage(rowOvertimeNotPaid, totalDD, totalID);
                    Misc.percentage(rowNotJustified, totalDD, totalID);
                    Misc.percentage(rowNotJustifiedClosure, totalDD, totalID);
                    Misc.percentage(rowNotJustifiedLayoff, totalDD, totalID);
                    Misc.percentage(rowNotJustifiedStoppage, totalDD, totalID);
                    Misc.percentage(rowNotJustifiedHoliday, totalDD, totalID);
                    Misc.percentage(rowUsedBankH1, totalDD, totalID);
                    Misc.percentage(rowNationalHoliday, totalDD, totalID);

                    rowRegularWork[7] = double.Parse(rowRegularWork[6].ToString()) * 100 / total;
                    rowBusinessTrip[7] = double.Parse(rowBusinessTrip[6].ToString()) * 100 / total;
                    rowAbsenceDuringDay[7] = double.Parse(rowAbsenceDuringDay[6].ToString()) * 100 / total;
                    rowLunchBreak[7] = double.Parse(rowLunchBreak[6].ToString()) * 100 / total;
                    rowAbsencesOther[7] = double.Parse(rowAbsencesOther[6].ToString()) * 100 / total;
                    rowAbsencesOtherClosure[7] = double.Parse(rowAbsencesOtherClosure[6].ToString()) * 100 / total;
                    rowAbsencesOtherLayoff[7] = double.Parse(rowAbsencesOtherLayoff[6].ToString()) * 100 / total;
                    rowAbsencesOtherStoppage[7] = double.Parse(rowAbsencesOtherStoppage[6].ToString()) * 100 / total;
                    rowAbsencesOtherHoliday[7] = double.Parse(rowAbsencesOtherHoliday[6].ToString()) * 100 / total;
                    rowTraining[7] = double.Parse(rowTraining[6].ToString()) * 100 / total;
                    rowProductiveRecovery[7] = double.Parse(rowProductiveRecovery[6].ToString()) * 100 / total;
                    rowStopWorkingHours[7] = double.Parse(rowStopWorkingHours[6].ToString()) * 100 / total;
                    rowBloodDonations[7] = double.Parse(rowBloodDonations[6].ToString()) * 100 / total;
                    rowBloodDonationsClosure[7] = double.Parse(rowBloodDonationsClosure[6].ToString()) * 100 / total;
                    rowBloodDonationsLayoff[7] = double.Parse(rowBloodDonationsLayoff[6].ToString()) * 100 / total;
                    rowBloodDonationsStoppage[7] = double.Parse(rowBloodDonationsStoppage[6].ToString()) * 100 / total;
                    rowBloodDonationsHoliday[7] = double.Parse(rowBloodDonationsHoliday[6].ToString()) * 100 / total;
                    rowTissueDonations[7] = double.Parse(rowTissueDonations[6].ToString()) * 100 / total;
                    rowTissueDonationsClosure[7] = double.Parse(rowTissueDonationsClosure[6].ToString()) * 100 / total;
                    rowTissueDonationsLayoff[7] = double.Parse(rowTissueDonationsLayoff[6].ToString()) * 100 / total;
                    rowTissueDonationsStoppage[7] = double.Parse(rowTissueDonationsStoppage[6].ToString()) * 100 / total;
                    rowTissueDonationsHoliday[7] = double.Parse(rowTissueDonationsHoliday[6].ToString()) * 100 / total;
                    rowJustifiedAbsence[7] = double.Parse(rowJustifiedAbsence[6].ToString()) * 100 / total;
                    rowJustifiedAbsenceClosure[7] = double.Parse(rowJustifiedAbsenceClosure[6].ToString()) * 100 / total;
                    rowJustifiedAbsenceLayoff[7] = double.Parse(rowJustifiedAbsenceLayoff[6].ToString()) * 100 / total;
                    rowJustifiedAbsenceStoppage[7] = double.Parse(rowJustifiedAbsenceStoppage[6].ToString()) * 100 / total;
                    rowJustifiedAbsenceHoliday[7] = double.Parse(rowJustifiedAbsenceHoliday[6].ToString()) * 100 / total;
                    rowUsedBankH[7] = double.Parse(rowUsedBankH[6].ToString()) * 100 / total;
                    rowHoliday[7] = double.Parse(rowHoliday[6].ToString()) * 100 / total;
                    rowCollectiveHoliday[7] = double.Parse(rowCollectiveHoliday[6].ToString()) * 100 / total;
                    rowReligiousHoliday[7] = double.Parse(rowReligiousHoliday[6].ToString()) * 100 / total;
                    rowOvertimeOthers[7] = double.Parse(rowOvertimeOthers[6].ToString()) * 100 / total;
                    rowLackOfWork[7] = double.Parse(rowLackOfWork[6].ToString()) * 100 / total;
                    rowIllness[7] = double.Parse(rowIllness[6].ToString()) * 100 / total;
                    rowIllnessClosure[7] = double.Parse(rowIllnessClosure[6].ToString()) * 100 / total;
                    rowIllnessLayoff[7] = double.Parse(rowIllnessLayoff[6].ToString()) * 100 / total;
                    rowIllnessStoppage[7] = double.Parse(rowIllnessStoppage[6].ToString()) * 100 / total;
                    rowIllnessHoliday[7] = double.Parse(rowIllnessHoliday[6].ToString()) * 100 / total;
                    rowOvertime[7] = double.Parse(rowOvertime[6].ToString()) * 100 / total;
                    rowOvertimeNotPaid[7] = double.Parse(rowOvertimeNotPaid[6].ToString()) * 100 / total;
                    rowNotJustified[7] = double.Parse(rowNotJustified[6].ToString()) * 100 / total;
                    rowNotJustifiedClosure[7] = double.Parse(rowNotJustifiedClosure[6].ToString()) * 100 / total;
                    rowNotJustifiedLayoff[7] = double.Parse(rowNotJustifiedLayoff[6].ToString()) * 100 / total;
                    rowNotJustifiedStoppage[7] = double.Parse(rowNotJustifiedStoppage[6].ToString()) * 100 / total;
                    rowNotJustifiedHoliday[7] = double.Parse(rowNotJustifiedHoliday[6].ToString()) * 100 / total;
                    rowUsedBankH1[7] = double.Parse(rowUsedBankH1[6].ToString()) * 100 / total;
                    row[7] = double.Parse(row[6].ToString()) * 100 / total;
                    rowNationalHoliday[7] = double.Parse(rowNationalHoliday[6].ToString()) * 100 / total;

                    if (rowRegularWork[7].ToString() == "NaN") rowRegularWork[7] = 0;
                    if (rowBusinessTrip[7].ToString() == "NaN") rowBusinessTrip[7] = 0;
                    if (rowAbsenceDuringDay[7].ToString() == "NaN") rowAbsenceDuringDay[7] = 0;
                    if (rowLunchBreak[7].ToString() == "NaN") rowLunchBreak[7] = 0;
                    if (rowAbsencesOther[7].ToString() == "NaN") rowAbsencesOther[7] = 0;
                    if (rowAbsencesOtherClosure[7].ToString() == "NaN") rowAbsencesOtherClosure[7] = 0;
                    if (rowAbsencesOtherLayoff[7].ToString() == "NaN") rowAbsencesOtherLayoff[7] = 0;
                    if (rowAbsencesOtherStoppage[7].ToString() == "NaN") rowAbsencesOtherStoppage[7] = 0;
                    if (rowAbsencesOtherHoliday[7].ToString() == "NaN") rowAbsencesOtherHoliday[7] = 0;
                    if (rowTraining[7].ToString() == "NaN") rowTraining[7] = 0;
                    if (rowProductiveRecovery[7].ToString() == "NaN") rowProductiveRecovery[7] = 0;
                    if (rowStopWorkingHours[7].ToString() == "NaN") rowStopWorkingHours[7] = 0;
                    if (rowBloodDonations[7].ToString() == "NaN") rowBloodDonations[7] = 0;
                    if (rowBloodDonationsClosure[7].ToString() == "NaN") rowBloodDonationsClosure[7] = 0;
                    if (rowBloodDonationsLayoff[7].ToString() == "NaN") rowBloodDonationsLayoff[7] = 0;
                    if (rowBloodDonationsStoppage[7].ToString() == "NaN") rowBloodDonationsStoppage[7] = 0;
                    if (rowBloodDonationsHoliday[7].ToString() == "NaN") rowBloodDonationsHoliday[7] = 0;
                    if (rowTissueDonations[7].ToString() == "NaN") rowTissueDonations[7] = 0;
                    if (rowTissueDonationsClosure[7].ToString() == "NaN") rowTissueDonationsClosure[7] = 0;
                    if (rowTissueDonationsLayoff[7].ToString() == "NaN") rowTissueDonationsLayoff[7] = 0;
                    if (rowTissueDonationsStoppage[7].ToString() == "NaN") rowTissueDonationsStoppage[7] = 0;
                    if (rowTissueDonationsHoliday[7].ToString() == "NaN") rowTissueDonationsHoliday[7] = 0;
                    if (rowJustifiedAbsence[7].ToString() == "NaN") rowJustifiedAbsence[7] = 0;
                    if (rowJustifiedAbsenceClosure[7].ToString() == "NaN") rowJustifiedAbsenceClosure[7] = 0;
                    if (rowJustifiedAbsenceLayoff[7].ToString() == "NaN") rowJustifiedAbsenceLayoff[7] = 0;
                    if (rowJustifiedAbsenceStoppage[7].ToString() == "NaN") rowJustifiedAbsenceStoppage[7] = 0;
                    if (rowJustifiedAbsenceHoliday[7].ToString() == "NaN") rowJustifiedAbsenceHoliday[7] = 0;
                    if (rowUsedBankH[7].ToString() == "NaN") rowUsedBankH[7] = 0;
                    if (rowHoliday[7].ToString() == "NaN") rowHoliday[7] = 0;
                    if (rowCollectiveHoliday[7].ToString() == "NaN") rowCollectiveHoliday[7] = 0;
                    if (rowReligiousHoliday[7].ToString() == "NaN") rowReligiousHoliday[7] = 0;
                    if (rowOvertimeOthers[7].ToString() == "NaN") rowOvertimeOthers[7] = 0;
                    if (rowLackOfWork[7].ToString() == "NaN") rowLackOfWork[7] = 0;
                    if (rowIllness[7].ToString() == "NaN") rowIllness[7] = 0;
                    if (rowIllnessClosure[7].ToString() == "NaN") rowIllnessClosure[7] = 0;
                    if (rowIllnessLayoff[7].ToString() == "NaN") rowIllnessLayoff[7] = 0;
                    if (rowIllnessStoppage[7].ToString() == "NaN") rowIllnessStoppage[7] = 0;
                    if (rowIllnessHoliday[7].ToString() == "NaN") rowIllnessHoliday[7] = 0;
                    if (rowOvertime[7].ToString() == "NaN") rowOvertime[7] = 0;
                    if (rowOvertimeNotPaid[7].ToString() == "NaN") rowOvertimeNotPaid[7] = 0;
                    if (rowNotJustified[7].ToString() == "NaN") rowNotJustified[7] = 0;
                    if (rowNotJustifiedClosure[7].ToString() == "NaN") rowNotJustifiedClosure[7] = 0;
                    if (rowNotJustifiedLayoff[7].ToString() == "NaN") rowNotJustifiedLayoff[7] = 0;
                    if (rowNotJustifiedStoppage[7].ToString() == "NaN") rowNotJustifiedStoppage[7] = 0;
                    if (rowNotJustifiedHoliday[7].ToString() == "NaN") rowNotJustifiedHoliday[7] = 0;
                    if (row[7].ToString() == "NaN") row[7] = 0;
                    if (rowUsedBankH1[7].ToString() == "NaN") rowUsedBankH1[7] = 0;
                    if (rowNationalHoliday[7].ToString() == "NaN") rowNationalHoliday[7] = 0;

                    double totalDPercent = double.Parse(rowRegularWork[3].ToString()) + double.Parse(rowBusinessTrip[3].ToString()) + double.Parse(rowAbsenceDuringDay[3].ToString())
                        + double.Parse(rowLunchBreak[3].ToString()) + double.Parse(rowAbsencesOther[3].ToString()) + double.Parse(rowAbsencesOtherClosure[3].ToString())
                        + double.Parse(rowAbsencesOtherLayoff[3].ToString()) + double.Parse(rowAbsencesOtherStoppage[3].ToString()) + double.Parse(rowAbsencesOtherHoliday[3].ToString())
                        + double.Parse(rowProductiveRecovery[3].ToString()) + double.Parse(rowTraining[3].ToString()) + double.Parse(rowStopWorkingHours[3].ToString())
                        + double.Parse(rowBloodDonations[3].ToString()) + double.Parse(rowBloodDonationsClosure[3].ToString()) + double.Parse(rowBloodDonationsLayoff[3].ToString())
                        + double.Parse(rowBloodDonationsStoppage[3].ToString()) + double.Parse(rowBloodDonationsHoliday[3].ToString())
                        + double.Parse(rowTissueDonations[3].ToString()) + double.Parse(rowTissueDonationsClosure[3].ToString()) + double.Parse(rowTissueDonationsLayoff[3].ToString())
                        + double.Parse(rowTissueDonationsStoppage[3].ToString()) + double.Parse(rowTissueDonationsHoliday[3].ToString())
                        + double.Parse(rowJustifiedAbsence[3].ToString()) + double.Parse(rowJustifiedAbsenceClosure[3].ToString()) + double.Parse(rowJustifiedAbsenceLayoff[3].ToString())
                        + double.Parse(rowJustifiedAbsenceStoppage[3].ToString()) + double.Parse(rowJustifiedAbsenceHoliday[3].ToString()) + double.Parse(rowUsedBankH[3].ToString())
                        + double.Parse(rowHoliday[3].ToString()) + double.Parse(rowCollectiveHoliday[3].ToString()) + double.Parse(rowReligiousHoliday[3].ToString())
                        + double.Parse(rowOvertimeOthers[3].ToString()) + double.Parse(rowLackOfWork[3].ToString()) + double.Parse(rowIllness[3].ToString())
                        + double.Parse(rowIllnessClosure[3].ToString()) + double.Parse(rowIllnessLayoff[3].ToString()) + double.Parse(rowIllnessStoppage[3].ToString()) 
                        + double.Parse(rowIllnessHoliday[3].ToString()) + double.Parse(rowOvertimeNotPaid[3].ToString()) + double.Parse(rowOvertime[3].ToString())
                        + double.Parse(rowNotJustified[3].ToString()) + double.Parse(rowNotJustifiedClosure[3].ToString()) + double.Parse(rowNotJustifiedLayoff[3].ToString())
                        + double.Parse(rowNotJustifiedStoppage[3].ToString()) + double.Parse(rowNotJustifiedHoliday[3].ToString())
                        + double.Parse(rowUsedBankH1[3].ToString()) + double.Parse(rowNationalHoliday[3].ToString());

                    double totalIPercent = double.Parse(rowRegularWork[5].ToString()) + double.Parse(rowBusinessTrip[5].ToString()) + double.Parse(rowAbsenceDuringDay[5].ToString())
                        + double.Parse(rowLunchBreak[5].ToString()) + double.Parse(rowAbsencesOther[5].ToString()) + double.Parse(rowAbsencesOtherClosure[5].ToString())
                        + double.Parse(rowAbsencesOtherLayoff[5].ToString()) + double.Parse(rowAbsencesOtherStoppage[5].ToString()) + double.Parse(rowAbsencesOtherHoliday[5].ToString()) 
                        + double.Parse(rowProductiveRecovery[5].ToString()) + double.Parse(rowTraining[5].ToString()) + double.Parse(rowStopWorkingHours[5].ToString())
                        + double.Parse(rowBloodDonations[5].ToString()) + double.Parse(rowBloodDonationsClosure[5].ToString()) + double.Parse(rowBloodDonationsLayoff[5].ToString())
                        + double.Parse(rowBloodDonationsStoppage[5].ToString()) + double.Parse(rowBloodDonationsHoliday[5].ToString())
                        + double.Parse(rowTissueDonations[5].ToString()) + double.Parse(rowTissueDonationsClosure[5].ToString()) + double.Parse(rowTissueDonationsLayoff[5].ToString())
                        + double.Parse(rowTissueDonationsStoppage[5].ToString()) + double.Parse(rowTissueDonationsHoliday[5].ToString())
                        + double.Parse(rowJustifiedAbsence[5].ToString()) + double.Parse(rowJustifiedAbsenceClosure[5].ToString()) + double.Parse(rowJustifiedAbsenceLayoff[5].ToString())
                        + double.Parse(rowJustifiedAbsenceStoppage[5].ToString()) + double.Parse(rowJustifiedAbsenceHoliday[5].ToString()) + double.Parse(rowUsedBankH[5].ToString())
                        + double.Parse(rowHoliday[5].ToString()) + double.Parse(rowCollectiveHoliday[5].ToString()) + double.Parse(rowReligiousHoliday[5].ToString())
                        + double.Parse(rowOvertimeOthers[5].ToString()) + double.Parse(rowLackOfWork[5].ToString()) + double.Parse(rowIllness[5].ToString())
                        + double.Parse(rowIllnessClosure[5].ToString()) + double.Parse(rowIllnessLayoff[5].ToString()) + double.Parse(rowIllnessStoppage[5].ToString())
                        + double.Parse(rowIllnessHoliday[5].ToString()) + double.Parse(rowOvertimeNotPaid[5].ToString()) + double.Parse(rowOvertime[5].ToString())
                        + double.Parse(rowNotJustified[5].ToString()) + double.Parse(rowNotJustifiedClosure[5].ToString()) + double.Parse(rowNotJustifiedLayoff[5].ToString())
                        + double.Parse(rowNotJustifiedStoppage[5].ToString()) + double.Parse(rowNotJustifiedHoliday[5].ToString())
                        + double.Parse(rowUsedBankH1[5].ToString()) + double.Parse(rowNationalHoliday[5].ToString());

                    row[3] = totalDPercent;
                    row[5] = totalIPercent;

                    double ts = double.Parse(rowTotal[2].ToString());
                    double tsi = double.Parse(rowTotal[4].ToString());
                    if (ts != 0) rowTotal[3] = 0;
                    if (tsi != 0) rowTotal[5] = 0;

                    rowTotal[7] = 0;
                    Misc.roundOn2(rowRegularWork);
                    Misc.roundOn2(rowBusinessTrip);
                    Misc.roundOn2(rowAbsenceDuringDay);
                    Misc.roundOn2(rowLunchBreak);
                    Misc.roundOn2(rowAbsencesOther);
                    Misc.roundOn2(rowAbsencesOtherClosure);
                    Misc.roundOn2(rowAbsencesOtherLayoff);
                    Misc.roundOn2(rowAbsencesOtherStoppage);
                    Misc.roundOn2(rowAbsencesOtherHoliday);
                    Misc.roundOn2(rowTraining);
                    Misc.roundOn2(rowProductiveRecovery);
                    Misc.roundOn2(rowStopWorkingHours);
                    Misc.roundOn2(rowBloodDonations);
                    Misc.roundOn2(rowBloodDonationsClosure);
                    Misc.roundOn2(rowBloodDonationsLayoff);
                    Misc.roundOn2(rowBloodDonationsStoppage);
                    Misc.roundOn2(rowBloodDonationsHoliday);
                    Misc.roundOn2(rowTissueDonations);
                    Misc.roundOn2(rowTissueDonationsClosure);
                    Misc.roundOn2(rowTissueDonationsLayoff);
                    Misc.roundOn2(rowTissueDonationsStoppage);
                    Misc.roundOn2(rowTissueDonationsHoliday);
                    Misc.roundOn2(rowJustifiedAbsence);
                    Misc.roundOn2(rowJustifiedAbsenceClosure);
                    Misc.roundOn2(rowJustifiedAbsenceLayoff);
                    Misc.roundOn2(rowJustifiedAbsenceStoppage);
                    Misc.roundOn2(rowJustifiedAbsenceHoliday);
                    Misc.roundOn2(rowUsedBankH);
                    Misc.roundOn2(rowHoliday);
                    Misc.roundOn2(rowCollectiveHoliday);
                    Misc.roundOn2(rowReligiousHoliday);
                    Misc.roundOn2(rowOvertimeOthers);
                    Misc.roundOn2(rowLackOfWork);
                    Misc.roundOn2(rowIllness);
                    Misc.roundOn2(rowIllnessClosure);
                    Misc.roundOn2(rowIllnessLayoff);
                    Misc.roundOn2(rowIllnessStoppage);
                    Misc.roundOn2(rowIllnessHoliday);
                    Misc.roundOn2(rowOvertime);
                    Misc.roundOn2(rowOvertimeNotPaid);
                    Misc.roundOn2(row);
                    Misc.roundOn2(rowTotal);
                    Misc.roundOn2(rowTotalDiff);
                    Misc.roundOn2(rowNotJustified);
                    Misc.roundOn2(rowNotJustifiedClosure);
                    Misc.roundOn2(rowNotJustifiedLayoff);
                    Misc.roundOn2(rowNotJustifiedStoppage);
                    Misc.roundOn2(rowNotJustifiedHoliday);
                    Misc.roundOn2(rowUsedBankH1);
                    Misc.roundOn2(rowNationalHoliday);

                    dt.Rows.Add(rowRegularWork);
                    dt.Rows.Add(rowBusinessTrip);
                    dt.Rows.Add(rowAbsenceDuringDay);
                    dt.Rows.Add(rowAbsencesOther);
                    dt.Rows.Add(rowAbsencesOtherClosure);
                    dt.Rows.Add(rowAbsencesOtherLayoff);
                    dt.Rows.Add(rowAbsencesOtherStoppage);
                    dt.Rows.Add(rowAbsencesOtherHoliday);
                    dt.Rows.Add(rowTraining);
                    dt.Rows.Add(rowLunchBreak);
                    dt.Rows.Add(rowNotJustified);
                    dt.Rows.Add(rowNotJustifiedClosure);
                    dt.Rows.Add(rowNotJustifiedLayoff);
                    dt.Rows.Add(rowNotJustifiedStoppage);
                    dt.Rows.Add(rowNotJustifiedHoliday);
                    dt.Rows.Add(rowProductiveRecovery);
                    dt.Rows.Add(rowStopWorkingHours);
                    dt.Rows.Add(rowBloodDonations);
                    dt.Rows.Add(rowBloodDonationsClosure);
                    dt.Rows.Add(rowBloodDonationsLayoff);
                    dt.Rows.Add(rowBloodDonationsStoppage);
                    dt.Rows.Add(rowBloodDonationsHoliday);
                    dt.Rows.Add(rowTissueDonations);
                    dt.Rows.Add(rowTissueDonationsClosure);
                    dt.Rows.Add(rowTissueDonationsLayoff);
                    dt.Rows.Add(rowTissueDonationsStoppage);
                    dt.Rows.Add(rowTissueDonationsHoliday);
                    dt.Rows.Add(rowJustifiedAbsence);
                    dt.Rows.Add(rowJustifiedAbsenceClosure);
                    dt.Rows.Add(rowJustifiedAbsenceLayoff);
                    dt.Rows.Add(rowJustifiedAbsenceStoppage);
                    dt.Rows.Add(rowJustifiedAbsenceHoliday);
                    dt.Rows.Add(rowUsedBankH);
                    dt.Rows.Add(rowUsedBankH1);
                    dt.Rows.Add(rowHoliday);
                    dt.Rows.Add(rowCollectiveHoliday);
                    dt.Rows.Add(rowReligiousHoliday);
                    dt.Rows.Add(rowNationalHoliday);
                    dt.Rows.Add(rowOvertimeOthers);
                    dt.Rows.Add(rowLackOfWork);
                    dt.Rows.Add(rowIllness);
                    dt.Rows.Add(rowIllnessClosure);
                    dt.Rows.Add(rowIllnessLayoff);
                    dt.Rows.Add(rowIllnessStoppage);
                    dt.Rows.Add(rowIllnessHoliday);
                    dt.Rows.Add(rowOvertime);
                    dt.Rows.Add(rowOvertimeNotPaid);
                    DataRow rowEmptyBottom = dt.NewRow();
                    dt.Rows.Add(rowEmptyBottom);
                    dt.Rows.Add(row);
                    dt.Rows.Add(rowTotal);
                    dt.Rows.Add(rowTotalDiff);

                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + ".populateTable400() " + ex.Message);
            }
        }

        private void populateDataTable400New(Object dbConnection, DataTable dt, List<DateTime> datesList, WorkingUnitTO company, List<IOPairProcessedTO> IOPairListDirect, 
            List<IOPairProcessedTO> IOPairListIndirect, string emplIdsBS, string emplIds, string qualify, WorkingUnitTO workingUnit, string title, Dictionary<int, PassTypeTO> passTypesDictionaryAll)
        {
            try
            {
                PassType passType;
                if (dbConnection == null)                
                    passType = new PassType();                
                else                
                    passType = new PassType(dbConnection);
                
                Dictionary<int, PassTypeTO> listPassTypes = new Dictionary<int, PassTypeTO>();
                WorkingUnit wu;
                if (dbConnection == null)
                    wu = new WorkingUnit();
                else
                    wu = new WorkingUnit(dbConnection);

                string ute = "";
                string workGroup = "";
                string costCenter = "";
                string plant = "";

                DataRow rowHeader1 = dt.NewRow();

                rowHeader1[1] = company.Description;

                //rowHeader1[1] = "FIAT GROUP AUTOMOBILES S.P.A.";
                rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                //rowHeader1[8] = "page: " + page;
                dt.Rows.Add(rowHeader1);

                DataRow rowHeader2 = dt.NewRow();
                rowHeader2[1] = title;
                rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                dt.Rows.Add(rowHeader2);

                WorkingUnitTO tempWU = wu.FindWU(workingUnit.WorkingUnitID);
                ute = tempWU.Code.Trim();

                // get workshop (parent of UTE)
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
                DataRow rowHeader3 = dt.NewRow();

                rowHeader3[1] = "Qualify:   " + qualify;
                dt.Rows.Add(rowHeader3);

                DataRow rowPlant = dt.NewRow();

                rowPlant[1] = "Plant:     " + plant + "     Cost center: " + costCenter;

                dt.Rows.Add(rowPlant);

                DataRow rowWorkgroup = dt.NewRow();

                rowWorkgroup[1] = "Workgroup: " + workGroup + "  UTE: " + ute;

                dt.Rows.Add(rowWorkgroup);

                DataRow rowEmpty = dt.NewRow();
                dt.Rows.Add(rowEmpty);

                DataRow rowColumns = dt.NewRow();
                rowColumns[2] = "Direct";
                rowColumns[3] = "%";
                rowColumns[4] = "Indirect";
                rowColumns[5] = "%";
                rowColumns[6] = "Total";
                rowColumns[7] = "%";
                dt.Rows.Add(rowColumns);
                DataRow rowNotJustified = dt.NewRow();
                listPassTypes = passType.FindByPaymentCode("0000", -1);
                
                if (listPassTypes.Count > 0)
                    calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "NOT JUSTIFIED HOURS", rowNotJustified, emplIdsBS, emplIds, datesList);                
                else 
                    calc400Empty("NOT JUSTIFIED HOURS", rowNotJustified);
                
                DataRow rowPresence = dt.NewRow();
                listPassTypes = passType.FindByPaymentCode("0012,0014,0041,0071", company.WorkingUnitID);
                if (listPassTypes.Count > 0)
                    calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "PRESENCE", rowPresence, emplIdsBS, emplIds, datesList);
                else
                    calc400Empty("PRESENCE", rowPresence);

                DataRow rowOvertime = dt.NewRow();
                listPassTypes = passType.FindByPaymentCode("0030", company.WorkingUnitID);
                if (listPassTypes.Count > 0)                
                    calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "OVERTIME", rowOvertime, emplIdsBS, emplIds, datesList);                
                else
                    calc400Empty("OVERTIME", rowOvertime);

                DataRow rowOvertimeBank = dt.NewRow();
                listPassTypes = passType.FindByPaymentCode("0312", company.WorkingUnitID);
                if (listPassTypes.Count > 0)                
                    calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "OVERPRESENCE BANK HOURS", rowOvertimeBank, emplIdsBS, emplIds, datesList);                
                else
                    calc400Empty("OVERPRESENCE BANK HOURS", rowOvertimeBank);

                DataRow rowAbsentism = dt.NewRow();
                string s = "0043,0045,0046,0047,0048,0049,0055,0056,0057,0058,0060,0061,0069,0070,0075,0144,0145,0146,0148,0149,0155,0156,";
                s += "0157,0169,0257,1257,1148,1155,0369,1145,1149,1150,0153,0171,0130,1069,0160,1160,1406,7777,1157,0357";
                listPassTypes = passType.FindByPaymentCode(s, company.WorkingUnitID);
                if (listPassTypes.Count > 0)
                    calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "ABSENTEISM", rowAbsentism, emplIdsBS, emplIds, datesList);                
                else
                    calc400Empty("ABSENTEEISM", rowAbsentism);

                DataRow rowStrike = dt.NewRow();
                listPassTypes = passType.FindByPaymentCode("1407", company.WorkingUnitID);
                if (listPassTypes.Count > 0)                
                    calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "STRIKE", rowStrike, emplIdsBS, emplIds, datesList);
                else
                    calc400Empty("STRIKE", rowStrike);

                DataRow rowNoWork = dt.NewRow();
                listPassTypes = passType.FindByPaymentCode("0053", company.WorkingUnitID);
                if (listPassTypes.Count > 0)
                    calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "NO WORK", rowNoWork, emplIdsBS, emplIds, datesList);                
                else
                    calc400Empty("NO WORK", rowNoWork);

                Dictionary<int, PassTypeTO> alPTDict = passType.FindByPaymentCode("0040", company.WorkingUnitID);
                DataRow rowHoliday = dt.NewRow();
                listPassTypes = new Dictionary<int, PassTypeTO>();
                List<int> alTypes = new Common.Rule().SearchRulesExact(Constants.RuleCompanyAnnualLeave);
                foreach (int ptID in alPTDict.Keys)
                {
                    if (alTypes.Contains(ptID) && !listPassTypes.ContainsKey(ptID))
                        listPassTypes.Add(ptID, alPTDict[ptID]);
                }
                if (listPassTypes.Count > 0)
                    calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "HOLIDAY", rowHoliday, emplIdsBS, emplIds, datesList);                
                else
                    calc400Empty("HOLIDAY", rowHoliday);

                DataRow rowCollectiveHoliday = dt.NewRow();
                listPassTypes = new Dictionary<int, PassTypeTO>();
                List<int> calTypes = new Common.Rule().SearchRulesExact(Constants.RuleCompanyCollectiveAnnualLeave);
                foreach (int ptID in alPTDict.Keys)
                {
                    if (calTypes.Contains(ptID) && !listPassTypes.ContainsKey(ptID))
                        listPassTypes.Add(ptID, alPTDict[ptID]);
                }
                if (listPassTypes.Count > 0)
                    calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "COLLECTIVE HOLIDAY", rowCollectiveHoliday, emplIdsBS, emplIds, datesList);                
                else
                    calc400Empty("COLLECTIVE HOLIDAY", rowCollectiveHoliday);

                DataRow rowReligiousHoliday = dt.NewRow();
                listPassTypes = passType.FindByPaymentCode("0044", company.WorkingUnitID);
                if (listPassTypes.Count > 0)
                    calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "RELIGIOUS HOLIDAY", rowReligiousHoliday, emplIdsBS, emplIds, datesList);                
                else
                    calc400Empty("RELIGIOUS HOLIDAY", rowReligiousHoliday);

                DataRow rowUsedBankH = dt.NewRow();
                listPassTypes = passType.FindByPaymentCode("0212", company.WorkingUnitID);
                if (listPassTypes.Count > 0)                
                    calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "USED BANK HOURS", rowUsedBankH, emplIdsBS, emplIds, datesList);                
                else
                    calc400Empty("USED BANK HOURS", rowUsedBankH);

                DataRow rowUnionLeaves = dt.NewRow();
                listPassTypes = passType.FindByPaymentCode("0112", company.WorkingUnitID);
                if (listPassTypes.Count > 0)
                    calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "UNION LEAVES", rowUnionLeaves, emplIdsBS, emplIds, datesList);                
                else
                    calc400Empty("UNION LEAVES", rowUnionLeaves);

                DataRow rowSchoolLeaves = dt.NewRow();
                listPassTypes = passType.FindByPaymentCode("0147", company.WorkingUnitID);
                if (listPassTypes.Count > 0)
                    calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "SCHOOL LEAVES", rowSchoolLeaves, emplIdsBS, emplIds, datesList);                
                else
                    calc400Empty("SCHOOL LEAVES", rowSchoolLeaves);

                DataRow rowNoWorkRecoveryH = dt.NewRow();
                listPassTypes = passType.FindByPaymentCode("0512", company.WorkingUnitID);
                if (listPassTypes.Count > 0)
                    calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "NO WORK RECOVERY HOURS", rowNoWorkRecoveryH, emplIdsBS, emplIds, datesList);                
                else
                    calc400Empty("NO WORK RECOVERY HOURS", rowNoWorkRecoveryH);
                
                DataRow rowProductiveLeaves = dt.NewRow();
                listPassTypes = passType.FindByPaymentCode("0612", company.WorkingUnitID);
                if (listPassTypes.Count > 0)
                    calc400(IOPairListDirect, IOPairListIndirect, listPassTypes, "PRODUCTIVE RECOVERY", rowProductiveLeaves, emplIdsBS, emplIds, datesList);                
                else
                    calc400Empty("PRODUCTIVE RECOVERY", rowProductiveLeaves);
                
                double totalDD = +double.Parse(rowProductiveLeaves[2].ToString()) + double.Parse(rowPresence[2].ToString()) + double.Parse(rowNoWorkRecoveryH[2].ToString()) 
                    + double.Parse(rowSchoolLeaves[2].ToString()) + double.Parse(rowUnionLeaves[2].ToString()) + double.Parse(rowUsedBankH[2].ToString()) 
                    + double.Parse(rowHoliday[2].ToString()) + double.Parse(rowCollectiveHoliday[2].ToString()) + double.Parse(rowReligiousHoliday[2].ToString()) 
                    + double.Parse(rowNoWork[2].ToString()) + double.Parse(rowStrike[2].ToString()) + double.Parse(rowAbsentism[2].ToString())
                    + double.Parse(rowOvertime[2].ToString()) + double.Parse(rowOvertimeBank[2].ToString()) + double.Parse(rowNotJustified[2].ToString());

                double totalID = +double.Parse(rowProductiveLeaves[4].ToString()) + double.Parse(rowPresence[4].ToString()) + double.Parse(rowNoWorkRecoveryH[4].ToString()) 
                    + double.Parse(rowSchoolLeaves[4].ToString()) + double.Parse(rowUnionLeaves[4].ToString()) + double.Parse(rowUsedBankH[4].ToString())
                    + double.Parse(rowHoliday[4].ToString()) + double.Parse(rowCollectiveHoliday[4].ToString()) + double.Parse(rowReligiousHoliday[4].ToString()) 
                    + double.Parse(rowNoWork[4].ToString()) + double.Parse(rowStrike[4].ToString()) + double.Parse(rowAbsentism[4].ToString())
                    + double.Parse(rowOvertime[4].ToString()) + double.Parse(rowOvertimeBank[4].ToString()) + double.Parse(rowNotJustified[4].ToString());

                double total = totalDD + totalID;
                DataRow row = dt.NewRow();
                row[1] = "Total";
                row[2] = totalDD;
                row[4] = totalID;
                row[6] = total;
                row[5] = 0;
                row[3] = 0;
                DataRow rowTotal = dt.NewRow();
                rowTotal[1] = "Planned";
                if (emplIdsBS.Length > 0)
                    rowTotal[2] = Misc.CalcPlannedTime(dbConnection, emplIdsBS, datesList[0], datesList[datesList.Count - 1]);
                else
                    rowTotal[2] = 0;
                if (emplIds.Length > 0)
                    rowTotal[4] = Misc.CalcPlannedTime(dbConnection, emplIds, datesList[0], datesList[datesList.Count - 1]);
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

                Misc.percentage(rowPresence, totalDD, totalID);
                Misc.percentage(rowProductiveLeaves, totalDD, totalID);
                Misc.percentage(rowNoWorkRecoveryH, totalDD, totalID);
                Misc.percentage(rowSchoolLeaves, totalDD, totalID);
                Misc.percentage(rowUnionLeaves, totalDD, totalID);
                Misc.percentage(rowUsedBankH, totalDD, totalID);
                Misc.percentage(rowHoliday, totalDD, totalID);
                Misc.percentage(rowCollectiveHoliday, totalDD, totalID);
                Misc.percentage(rowReligiousHoliday, totalDD, totalID);
                Misc.percentage(rowNoWork, totalDD, totalID);
                Misc.percentage(rowStrike, totalDD, totalID);
                Misc.percentage(rowAbsentism, totalDD, totalID);
                Misc.percentage(rowOvertimeBank, totalDD, totalID);
                Misc.percentage(rowOvertime, totalDD, totalID);
                Misc.percentage(rowNotJustified, totalDD, totalID);

                rowPresence[7] = double.Parse(rowPresence[6].ToString()) * 100 / total;
                rowProductiveLeaves[7] = double.Parse(rowProductiveLeaves[6].ToString()) * 100 / total;
                rowNoWorkRecoveryH[7] = double.Parse(rowNoWorkRecoveryH[6].ToString()) * 100 / total;
                rowSchoolLeaves[7] = double.Parse(rowSchoolLeaves[6].ToString()) * 100 / total;
                rowUnionLeaves[7] = double.Parse(rowUnionLeaves[6].ToString()) * 100 / total;
                rowUsedBankH[7] = double.Parse(rowUsedBankH[6].ToString()) * 100 / total;
                rowHoliday[7] = double.Parse(rowHoliday[6].ToString()) * 100 / total;
                rowCollectiveHoliday[7] = double.Parse(rowCollectiveHoliday[6].ToString()) * 100 / total;
                rowReligiousHoliday[7] = double.Parse(rowReligiousHoliday[6].ToString()) * 100 / total;
                rowNoWork[7] = double.Parse(rowNoWork[6].ToString()) * 100 / total;
                rowStrike[7] = double.Parse(rowStrike[6].ToString()) * 100 / total;
                rowAbsentism[7] = double.Parse(rowAbsentism[6].ToString()) * 100 / total;
                rowOvertimeBank[7] = double.Parse(rowOvertimeBank[6].ToString()) * 100 / total;
                rowOvertime[7] = double.Parse(rowOvertime[6].ToString()) * 100 / total;
                rowNotJustified[7] = double.Parse(rowNotJustified[6].ToString()) * 100 / total;
                row[7] = double.Parse(row[6].ToString()) * 100 / total;

                if (rowPresence[7].ToString() == "NaN") rowPresence[7] = 0;
                if (rowProductiveLeaves[7].ToString() == "NaN") rowProductiveLeaves[7] = 0;
                if (rowNoWorkRecoveryH[7].ToString() == "NaN") rowNoWorkRecoveryH[7] = 0;
                if (rowSchoolLeaves[7].ToString() == "NaN") rowSchoolLeaves[7] = 0;
                if (rowUnionLeaves[7].ToString() == "NaN") rowUnionLeaves[7] = 0;
                if (rowUsedBankH[7].ToString() == "NaN") rowUsedBankH[7] = 0;
                if (rowHoliday[7].ToString() == "NaN") rowHoliday[7] = 0;
                if (rowCollectiveHoliday[7].ToString() == "NaN") rowCollectiveHoliday[7] = 0;
                if (rowReligiousHoliday[7].ToString() == "NaN") rowReligiousHoliday[7] = 0;
                if (rowNoWork[7].ToString() == "NaN") rowNoWork[7] = 0;
                if (rowStrike[7].ToString() == "NaN") rowStrike[7] = 0;
                if (rowAbsentism[7].ToString() == "NaN") rowAbsentism[7] = 0;
                if (rowOvertimeBank[7].ToString() == "NaN") rowOvertimeBank[7] = 0;
                if (rowOvertime[7].ToString() == "NaN") rowOvertime[7] = 0;
                if (rowNotJustified[7].ToString() == "NaN") rowNotJustified[7] = 0;
                if (row[7].ToString() == "NaN") row[7] = 0;

                double totalDPercent = double.Parse(rowProductiveLeaves[3].ToString()) + double.Parse(rowPresence[3].ToString()) + double.Parse(rowNoWorkRecoveryH[3].ToString()) 
                    + double.Parse(rowSchoolLeaves[3].ToString()) + double.Parse(rowUnionLeaves[3].ToString()) + double.Parse(rowUsedBankH[3].ToString())
                    + double.Parse(rowHoliday[3].ToString()) + double.Parse(rowCollectiveHoliday[3].ToString()) + double.Parse(rowReligiousHoliday[3].ToString()) 
                    + double.Parse(rowNoWork[3].ToString()) + double.Parse(rowStrike[3].ToString()) + double.Parse(rowAbsentism[3].ToString())
                    + double.Parse(rowOvertime[3].ToString()) + double.Parse(rowOvertimeBank[3].ToString()) + double.Parse(rowNotJustified[3].ToString());

                double totalIPercent = double.Parse(rowProductiveLeaves[5].ToString()) + double.Parse(rowPresence[5].ToString()) + double.Parse(rowNoWorkRecoveryH[5].ToString()) 
                    + double.Parse(rowSchoolLeaves[5].ToString()) + double.Parse(rowUnionLeaves[5].ToString()) + double.Parse(rowUsedBankH[5].ToString())
                    + double.Parse(rowHoliday[5].ToString()) + double.Parse(rowCollectiveHoliday[5].ToString()) + double.Parse(rowReligiousHoliday[5].ToString()) 
                    + double.Parse(rowNoWork[5].ToString()) + double.Parse(rowStrike[5].ToString()) + double.Parse(rowAbsentism[5].ToString())
                    + double.Parse(rowOvertime[5].ToString()) + double.Parse(rowOvertimeBank[5].ToString()) + double.Parse(rowNotJustified[5].ToString());
                
                row[3] = totalDPercent;
                row[5] = totalIPercent;

                double ts = double.Parse(rowTotal[2].ToString());
                double tsi = double.Parse(rowTotal[4].ToString());
                if (ts != 0) rowTotal[3] = 0;
                if (tsi != 0) rowTotal[5] = 0;

                rowTotal[7] = 0;

                Misc.roundOn2(rowPresence);
                Misc.roundOn2(rowProductiveLeaves);
                Misc.roundOn2(rowNoWorkRecoveryH);
                Misc.roundOn2(rowSchoolLeaves);
                Misc.roundOn2(rowUnionLeaves);
                Misc.roundOn2(rowUsedBankH);
                Misc.roundOn2(rowHoliday);
                Misc.roundOn2(rowCollectiveHoliday);
                Misc.roundOn2(rowReligiousHoliday);
                Misc.roundOn2(rowNoWork);
                Misc.roundOn2(rowStrike);
                Misc.roundOn2(rowAbsentism);
                Misc.roundOn2(rowOvertimeBank);
                Misc.roundOn2(rowOvertime);
                Misc.roundOn2(row);
                Misc.roundOn2(rowTotal);
                Misc.roundOn2(rowTotalDiff);
                Misc.roundOn2(rowNotJustified);

                dt.Rows.Add(rowPresence);
                dt.Rows.Add(rowNotJustified);
                dt.Rows.Add(rowProductiveLeaves);
                dt.Rows.Add(rowNoWorkRecoveryH);
                dt.Rows.Add(rowSchoolLeaves);
                dt.Rows.Add(rowUnionLeaves);
                dt.Rows.Add(rowUsedBankH);
                dt.Rows.Add(rowHoliday);
                dt.Rows.Add(rowCollectiveHoliday);
                dt.Rows.Add(rowReligiousHoliday);
                dt.Rows.Add(rowNoWork);
                dt.Rows.Add(rowStrike);
                dt.Rows.Add(rowAbsentism);
                dt.Rows.Add(rowOvertimeBank);
                dt.Rows.Add(rowOvertime);
                DataRow rowEmptyBottom = dt.NewRow();
                dt.Rows.Add(rowEmptyBottom);
                dt.Rows.Add(row);
                dt.Rows.Add(rowTotal);
                dt.Rows.Add(rowTotalDiff);

                dt.AcceptChanges();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + ".populateTable400() " + ex.Message);
            }
        }

        private void calc400(List<IOPairProcessedTO> IOPairListDirect, List<IOPairProcessedTO> IOPairListIndirect, Dictionary<int, PassTypeTO> listPassTypes, string columnName, DataRow row,
               string emplIdsBS, string emplIds, List<DateTime> datesList)
        {
            try
            {

                row[1] = columnName;


                TimeSpan totalDurationDirect = new TimeSpan();
                TimeSpan totalDurationIndirect = new TimeSpan();

                foreach (IOPairProcessedTO iopair in IOPairListDirect)
                {

                    if (listPassTypes.ContainsKey(iopair.PassTypeID))
                    {
                        TimeSpan duration = iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay);
                        if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                            duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift
                        totalDurationDirect = totalDurationDirect.Add(duration);
                    }

                }
                foreach (IOPairProcessedTO iopair in IOPairListIndirect)
                {
                    if (listPassTypes.ContainsKey(iopair.PassTypeID))
                    {

                        TimeSpan duration = iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay);
                        if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                            duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift
                        totalDurationIndirect = totalDurationIndirect.Add(duration);
                    }
                }
                row["Direct"] = totalDurationDirect.TotalHours;
                row["Indirect"] = totalDurationIndirect.TotalHours;
                row["Total"] = (totalDurationDirect + totalDurationIndirect).TotalHours;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void calc400New(string columnName, DataRow row, string emplIdsBC, string emplIds, string payment_code, uint py_calc_id)
        {
            try
            {
                List<EmployeePYDataAnaliticalTO> listPYAnaliticalDirect = new List<EmployeePYDataAnaliticalTO>();
                List<EmployeePYDataAnaliticalTO> listPYAnaliticalIndirect = new List<EmployeePYDataAnaliticalTO>();
                row[1] = columnName;
                if (emplIds.Length > 0)
                    listPYAnaliticalIndirect = new EmployeePYDataAnalitical().Search(emplIds, payment_code, py_calc_id, "E");
                if (emplIdsBC.Length > 0)
                    listPYAnaliticalDirect = new EmployeePYDataAnalitical().Search(emplIdsBC, payment_code, py_calc_id, "E");

                decimal totalDurationDirect = 0;
                decimal totalDurationIndirect = 0;

                foreach (EmployeePYDataAnaliticalTO iopair in listPYAnaliticalDirect)
                {
                    totalDurationDirect += iopair.HrsAmount;
                }
                foreach (EmployeePYDataAnaliticalTO iopair in listPYAnaliticalIndirect)
                {
                    totalDurationIndirect += iopair.HrsAmount;
                }

                row["Direct"] = totalDurationDirect;
                row["Indirect"] = totalDurationIndirect;
                row["Total"] = totalDurationDirect + totalDurationIndirect;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void calc400Lunch(List<IOPairProcessedTO> IOPairListDirect, List<IOPairProcessedTO> IOPairListIndirect, Dictionary<int, PassTypeTO> listPassTypes, string columnName, DataRow row,
              string emplIdsBS, string emplIds, List<DateTime> datesList, Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>> directEmplDayIntervals, Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>> directEmplDaySchemas, Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>> indirectEmplDayIntervals, Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>> indirectEmplDaySchemas, Dictionary<int, PassTypeTO> passTypesDictionaryAll)
        {
            try
            {


                row[1] = columnName;

                decimal totalDurationDirect = 0;
                decimal totalDurationIndirect = 0;

                Dictionary<DateTime, List<IOPairProcessedTO>> directDaily = new Dictionary<DateTime, List<IOPairProcessedTO>>();
                foreach (IOPairProcessedTO pair in IOPairListDirect)
                {
                    if (!directDaily.ContainsKey(pair.IOPairDate))
                        directDaily.Add(pair.IOPairDate, new List<IOPairProcessedTO>());

                    directDaily[pair.IOPairDate].Add(pair);
                }
                Dictionary<DateTime, List<IOPairProcessedTO>> indirectDaily = new Dictionary<DateTime, List<IOPairProcessedTO>>();
                foreach (IOPairProcessedTO pair in IOPairListIndirect)
                {
                    if (!indirectDaily.ContainsKey(pair.IOPairDate))
                        indirectDaily.Add(pair.IOPairDate, new List<IOPairProcessedTO>());

                    indirectDaily[pair.IOPairDate].Add(pair);
                }

                foreach (IOPairProcessedTO iopair in IOPairListDirect)
                {
                    List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                    if (directDaily.ContainsKey(iopair.IOPairDate))
                        dayPairs = directDaily[iopair.IOPairDate];

                    List<WorkTimeIntervalTO> interval = new List<WorkTimeIntervalTO>();
                    if (directEmplDayIntervals.ContainsKey(iopair.EmployeeID) && directEmplDayIntervals[iopair.EmployeeID].ContainsKey(iopair.IOPairDate))
                        interval = directEmplDayIntervals[iopair.EmployeeID][iopair.IOPairDate];

                    WorkTimeSchemaTO schema = null;
                    if (directEmplDaySchemas.ContainsKey(iopair.EmployeeID) && directEmplDaySchemas[iopair.EmployeeID].ContainsKey(iopair.IOPairDate))
                        schema = directEmplDaySchemas[iopair.EmployeeID][iopair.IOPairDate];


                    if ((iopair.IOPairDate == datesList[0] && !Common.Misc.isPreviousDayPair(iopair, passTypesDictionaryAll, dayPairs, schema, interval)) || (iopair.IOPairDate != datesList[0] && iopair.IOPairDate != datesList[datesList.Count - 1]) || (iopair.IOPairDate == datesList[datesList.Count - 1] && Common.Misc.isPreviousDayPair(iopair, passTypesDictionaryAll, dayPairs, schema, interval)))
                    {
                        if (listPassTypes.ContainsKey(iopair.PassTypeID))
                        {
                            TimeSpan duration = iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay);
                            if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                                duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift
                            totalDurationDirect += (decimal)duration.TotalHours;
                        }

                    }
                }
                foreach (IOPairProcessedTO iopair in IOPairListIndirect)
                {
                    List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                    if (indirectDaily.ContainsKey(iopair.IOPairDate))
                        dayPairs = indirectDaily[iopair.IOPairDate];

                    List<WorkTimeIntervalTO> interval = new List<WorkTimeIntervalTO>();
                    if (indirectEmplDayIntervals.ContainsKey(iopair.EmployeeID) && indirectEmplDayIntervals[iopair.EmployeeID].ContainsKey(iopair.IOPairDate))
                        interval = indirectEmplDayIntervals[iopair.EmployeeID][iopair.IOPairDate];

                    WorkTimeSchemaTO schema = null;
                    if (indirectEmplDaySchemas.ContainsKey(iopair.EmployeeID) && indirectEmplDaySchemas[iopair.EmployeeID].ContainsKey(iopair.IOPairDate))
                        schema = indirectEmplDaySchemas[iopair.EmployeeID][iopair.IOPairDate];

                    if ((iopair.IOPairDate == datesList[0] && Common.Misc.isPreviousDayPair(iopair, passTypesDictionaryAll, dayPairs, schema, interval)) || (iopair.IOPairDate != datesList[0] && iopair.IOPairDate != datesList[datesList.Count - 1]) || (iopair.IOPairDate == datesList[datesList.Count - 1] && !Common.Misc.isPreviousDayPair(iopair, passTypesDictionaryAll, dayPairs, schema, interval)))
                    {
                        if (listPassTypes.ContainsKey(iopair.PassTypeID))
                        {
                            TimeSpan duration = iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay);
                            if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                                duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift
                            totalDurationIndirect += (decimal)duration.TotalHours;
                        }
                    }

                }

                foreach (int direct in lunchTypes.Keys)
                {
                    foreach (int passT in lunchTypes[direct].Keys)
                    {
                        if (listPassTypes.ContainsKey(passT))
                        {
                            if (direct == 0) totalDurationDirect -= lunchTypes[direct][passT];
                            else if (direct == 1) totalDurationIndirect -= lunchTypes[direct][passT];
                        }
                    }
                }

                row["Direct"] = totalDurationDirect;
                row["Indirect"] = totalDurationIndirect;
                row["Total"] = (totalDurationDirect + totalDurationIndirect);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void calc400Empty(string columnName, DataRow row)
        {
            try
            {
                row[1] = columnName;


                row[2] = 0;
                row[4] = 0;
                row[6] = 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private bool isWorkOnHoliday(Dictionary<string, List<DateTime>> personalHolidayDays, List<DateTime> nationalHolidaysDays, List<DateTime> nationalHolidaysDaysSundays,
            List<HolidaysExtendedTO> nationalTransferableHolidays, DateTime date, WorkTimeSchemaTO schema, EmployeeAsco4TO asco, EmployeeTO empl,
            Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict)
        {
            try
            {
                bool isHoliday = false;

                if (Common.Misc.isExpatOut(rulesDict, empl))
                    return isHoliday;

                // personal holidays are work on holiday for everyone
                if ((personalHolidayDays.ContainsKey(asco.NVarcharValue1) && personalHolidayDays[asco.NVarcharValue1].Contains(date.Date))
                    || (asco.DatetimeValue1.Day == date.Day && asco.DatetimeValue1.Month == date.Month))
                    isHoliday = true;
                else if (schema.Type.Trim().ToUpper() == Constants.schemaTypeIndustrial.Trim().ToUpper())
                {
                    // industrial schema has work on holiday just for national holidays, no transfer of holidays
                    if (nationalHolidaysDays.Contains(date.Date))
                        isHoliday = true;
                }
                else
                {
                    // count work on holiday just once
                    if (nationalHolidaysDays.Contains(date.Date))
                        isHoliday = true;
                    else if (nationalHolidaysDaysSundays.Contains(date.Date))
                    {
                        // check if work is already counted on belonging national holiday
                        DateTime maxNationalHolidayEnd = new DateTime();
                        DateTime maxNationalHolidayStart = new DateTime();

                        foreach (HolidaysExtendedTO hol in nationalTransferableHolidays)
                        {
                            if (hol.DateEnd.Date > maxNationalHolidayEnd.Date && hol.DateEnd.Date < date.Date)
                            {
                                maxNationalHolidayStart = hol.DateStart.Date;
                                maxNationalHolidayEnd = hol.DateEnd.Date;
                            }
                        }

                        bool holidayCalculated = false;
                        if (!maxNationalHolidayStart.Equals(new DateTime()) && !maxNationalHolidayEnd.Equals(new DateTime()))
                        {
                            // check if work on holiday is already calculated for this holiday
                            DateTime currDate = maxNationalHolidayStart.Date;
                            while (currDate.Date <= maxNationalHolidayEnd.Date)
                            {


                                currDate = currDate.AddDays(1);
                            }
                        }

                        isHoliday = !holidayCalculated;
                    }
                }

                return isHoliday;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PYIntegration.isWorkOnHoliday(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
