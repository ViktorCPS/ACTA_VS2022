using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using Common;
using System.IO;
using TransferObjects;
using System.Data;

namespace ACTAWorkAnalysisReports
{
    public class Report500
    {
        const int rowDataNum = 53;
        const int rowDataWebNum = 50;

        DebugLog debug;
        
        public Report500()
        {
            debug = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");
        }

        public string GenerateReport(Object dbConnection, List<DateTime> datesList, string fileName, List<WorkingUnitTO> listWU, int company, uint py_calc)
        {
            debug.writeLog("+ Started 500! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            
            DataSet ds = new DataSet(Path.GetFileNameWithoutExtension(fileName));
            try
            {
                TimeSpan timeSpan = datesList[datesList.Count - 1] - datesList[0].AddDays(-1);
                int numOfEmployees = 0;
                WorkingUnit wu;
                Employee Empl;

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
                System.Data.DataTable dtCompany = new System.Data.DataTable(company + "BC");
                dtCompany.Columns.Add("1", typeof(string));
                dtCompany.Columns.Add("   ", typeof(string));
                dtCompany.Columns.Add("Direct", typeof(string));
                dtCompany.Columns.Add("%  ", typeof(string));
                dtCompany.Columns.Add("Indirect", typeof(string));
                dtCompany.Columns.Add("%   ", typeof(string));
                dtCompany.Columns.Add("Total", typeof(string));
                dtCompany.Columns.Add(" %   ", typeof(string));


                int dcompany = 0;
                int numEmplCompanyDirect = 0;


                foreach (WorkingUnitTO plant in listWU)
                {
                    string plantString = "";
                    int dPlant = 0;
                    int numEmplPlantDirect = 0;
                    if (dbConnection == null)
                    {
                        wu = new WorkingUnit();
                    }
                    else
                    {
                        wu = new WorkingUnit(dbConnection);
                    }
                    System.Data.DataTable dtPlant = new System.Data.DataTable(plant.Name + "direct");
                    dtPlant.Columns.Add("1", typeof(string));
                    dtPlant.Columns.Add("   ", typeof(string));
                    dtPlant.Columns.Add("Direct", typeof(string));
                    dtPlant.Columns.Add("%  ", typeof(string));
                    dtPlant.Columns.Add("Indirect", typeof(string));
                    dtPlant.Columns.Add("%   ", typeof(string));
                    dtPlant.Columns.Add("Total", typeof(string));
                    dtPlant.Columns.Add(" %   ", typeof(string));

                    List<WorkingUnitTO> listCostCenter = wu.SearchChildWU(plant.WorkingUnitID.ToString());

                    foreach (WorkingUnitTO costCenter in listCostCenter)
                    {
                        int numEmplCostDirect = 0;
                        int dCost = 0;
                        if (dbConnection == null)
                        {
                            wu = new WorkingUnit();
                        }
                        else
                        {
                            wu = new WorkingUnit(dbConnection);
                        }

                        System.Data.DataTable dtCostCenter = new System.Data.DataTable(costCenter.Name + "direct");
                        dtCostCenter.Columns.Add("1", typeof(string));
                        dtCostCenter.Columns.Add("   ", typeof(string));
                        dtCostCenter.Columns.Add("Direct", typeof(string));
                        dtCostCenter.Columns.Add("%  ", typeof(string));
                        dtCostCenter.Columns.Add("Indirect", typeof(string));
                        dtCostCenter.Columns.Add("%   ", typeof(string));
                        dtCostCenter.Columns.Add("Total", typeof(string));
                        dtCostCenter.Columns.Add(" %   ", typeof(string));


                        List<WorkingUnitTO> listWorkshop = wu.SearchChildWU(costCenter.WorkingUnitID.ToString());

                        foreach (WorkingUnitTO workshop in listWorkshop)
                        {
                            int numEmplWorkgroupDirect = 0;
                            int d = 0; //TO CHECK IS IT FIRST TIME FILL DATATABLE FOR WORKSHOP
                            if (dbConnection == null)
                            {
                                wu = new WorkingUnit();
                            }
                            else
                            {
                                wu = new WorkingUnit(dbConnection);
                            }
                            System.Data.DataTable dtWorkGroup = new System.Data.DataTable(workshop.Name + "direct");
                            dtWorkGroup.Columns.Add("1", typeof(string));
                            dtWorkGroup.Columns.Add("   ", typeof(string));
                            dtWorkGroup.Columns.Add("Direct", typeof(string));
                            dtWorkGroup.Columns.Add("%  ", typeof(string));
                            dtWorkGroup.Columns.Add("Indirect", typeof(string));
                            dtWorkGroup.Columns.Add("%   ", typeof(string));
                            dtWorkGroup.Columns.Add("Total", typeof(string));
                            dtWorkGroup.Columns.Add(" %   ", typeof(string));

                            List<WorkingUnitTO> listUTE = wu.SearchChildWU(workshop.WorkingUnitID.ToString());
                            foreach (WorkingUnitTO workingUnit in listUTE)
                            {
                                System.Data.DataTable dtUTE = new System.Data.DataTable(workingUnit.Name + "direct");
                                dtUTE.Columns.Add("1", typeof(string));
                                dtUTE.Columns.Add("   ", typeof(string));
                                dtUTE.Columns.Add("Direct", typeof(string));
                                dtUTE.Columns.Add("%  ", typeof(string));
                                dtUTE.Columns.Add("Indirect", typeof(string));
                                dtUTE.Columns.Add("%   ", typeof(string));
                                dtUTE.Columns.Add("Total", typeof(string));
                                dtUTE.Columns.Add(" %   ", typeof(string));
                                ds.Tables.Add(dtUTE);

                                List<EmployeeTO> listEmpl = new List<EmployeeTO>();

                                listEmpl = Empl.SearchByWULoans(workingUnit.WorkingUnitID.ToString(), -1, null, datesList[0], datesList[datesList.Count - 1]);
                                numOfEmployees = listEmpl.Count;
                                numEmplWorkgroupDirect += numOfEmployees;
                                numEmplCostDirect += numOfEmployees;
                                numEmplPlantDirect += numOfEmployees;
                                numEmplCompanyDirect += numOfEmployees;
                                double totalDays = numOfEmployees * timeSpan.TotalDays;
                                //POPULATE TABLE FOR UTE AND THAN USE THIS DATA FOR WORKGROUP, COST CENTER, PLANT
                                if (!populateDataTableNew(dbConnection, dtUTE, workingUnit, datesList, company, "   Absenteeism industrial relation", 1, py_calc))
                                {

                                    //LAST EMPTY ROW
                                    DataRow rowEmptyFooter = dtUTE.NewRow();
                                    dtUTE.Rows.Add(rowEmptyFooter);

                                    //FOOTER ROW
                                    DataRow footer = dtUTE.NewRow();
                                    footer[1] = "N° Employees: " + numOfEmployees;
                                    footer[2] = "Total days: ";
                                    footer[3] = totalDays;
                                    footer[4] = "Calendar days: ";
                                    footer[5] = timeSpan.TotalDays;

                                    dtUTE.Rows.Add(footer);

                                    d++;
                                    if (d == 1)
                                    {
                                        //FIRST TIME FILL DATATABLE FOR WORKSHOP, FIRST HEADER AND THEN FOREACH ROW OF UTE TABLE SET ROW FOR WORKGROUP TABLE
                                        string ute = "";
                                        string workGroup = "";
                                        string costCenterString = "";
                                        //string plantString = "";
                                        DataRow rowHeader1 = dtWorkGroup.NewRow();

                                        if (dbConnection == null)
                                            rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
                                        else
                                            rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
                                        rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                        rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                        //rowHeader1[8] = "page: " + page;
                                        dtWorkGroup.Rows.Add(rowHeader1);

                                        DataRow rowHeader2 = dtWorkGroup.NewRow();
                                        rowHeader2[1] = "   Absenteeism industrial relation";
                                        rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                        rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                                        dtWorkGroup.Rows.Add(rowHeader2);

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


                                        DataRow rowPlant = dtWorkGroup.NewRow();
                                        rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
                                        dtWorkGroup.Rows.Add(rowPlant);

                                        DataRow rowWorkgroup = dtWorkGroup.NewRow();
                                        rowWorkgroup[1] = "Workgroup: " + workGroup + " UTE: " + "xx";
                                        dtWorkGroup.Rows.Add(rowWorkgroup);

                                        DataRow rowEmpty = dtWorkGroup.NewRow();
                                        dtWorkGroup.Rows.Add(rowEmpty);

                                        //ROW COLUMN HEADER
                                        DataRow rowColumns = dtWorkGroup.NewRow();
                                        rowColumns[2] = "Direct";
                                        rowColumns[3] = "%";
                                        rowColumns[4] = "Indirect";
                                        rowColumns[5] = "%";
                                        rowColumns[6] = "Total";
                                        rowColumns[7] = "%";
                                        dtWorkGroup.Rows.Add(rowColumns);

                                        //FIRST 6 ROWS OF UTE TABLE ARE HEADER, DONT NEED IT HERE
                                        for (int ind = 6; ind < dtUTE.Rows.Count - 2; ind++)
                                        {

                                            DataRow row = dtWorkGroup.NewRow();
                                            row[1] = dtUTE.Rows[ind][1];
                                            row[2] = dtUTE.Rows[ind][2];
                                            //row[3] = dtUTE.Rows[ind][3];
                                            row[4] = dtUTE.Rows[ind][4];
                                            //row[5] = dtUTE.Rows[ind][5];
                                            row[6] = dtUTE.Rows[ind][6];
                                            //row[7] = dtUTE.Rows[ind][7];
                                            dtWorkGroup.Rows.Add(row);
                                            dtWorkGroup.AcceptChanges();

                                        }
                                    }
                                    else
                                    {
                                        //IF IS NOT FIRST TIME, NOT FIRST UTE IN WORKGROUP THEN ADD TO EXISTING TABLE WORKGROUP
                                        for (int ind = 6; ind < dtUTE.Rows.Count - 2; ind++)
                                        {
                                            // EMPTY ROW
                                            // 03.03.2014. Sanja - two holliady rows added
                                            if (ind != rowDataNum)
                                            //if (ind != 26)
                                            {
                                                //ROW +/-
                                                if (ind != (rowDataNum + 3))
                                                {
                                                    dtWorkGroup.Rows[ind][1] = dtUTE.Rows[ind][1];
                                                    dtWorkGroup.Rows[ind][2] = double.Parse(dtWorkGroup.Rows[ind][2].ToString()) + double.Parse(dtUTE.Rows[ind][2].ToString());
                                                    dtWorkGroup.Rows[ind][4] = double.Parse(dtWorkGroup.Rows[ind][4].ToString()) + double.Parse(dtUTE.Rows[ind][4].ToString());
                                                    dtWorkGroup.Rows[ind][6] = double.Parse(dtWorkGroup.Rows[ind][6].ToString()) + double.Parse(dtUTE.Rows[ind][6].ToString());

                                                }
                                                else
                                                {
                                                    dtWorkGroup.Rows[ind][1] = dtUTE.Rows[ind][1];
                                                    dtWorkGroup.Rows[ind][2] = double.Parse(dtWorkGroup.Rows[ind][2].ToString()) + double.Parse(dtUTE.Rows[ind][2].ToString());
                                                    dtWorkGroup.Rows[ind][4] = double.Parse(dtWorkGroup.Rows[ind][4].ToString()) + double.Parse(dtUTE.Rows[ind][4].ToString());
                                                    dtWorkGroup.Rows[ind][6] = double.Parse(dtWorkGroup.Rows[ind][6].ToString()) + double.Parse(dtUTE.Rows[ind][6].ToString());
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            if (dbConnection == null)
                            {
                                wu = new WorkingUnit();
                            }
                            else
                            {
                                wu = new WorkingUnit(dbConnection);
                            }
                            List<WorkingUnitTO> oneWorkShop = new List<WorkingUnitTO>();
                            oneWorkShop.Add(workshop);
                            List<WorkingUnitTO> workShopAndUTE = wu.FindAllChildren(oneWorkShop);
                            if (dtWorkGroup.Rows.Count > 0)
                            {
                                double totalDD = 0;
                                double totalID = 0;
                                double totalDDSum = 0;
                                double totalIDSum = 0;
                                for (int i = 6; i < rowDataNum; i++)
                                {
                                    totalDD = double.Parse(dtWorkGroup.Rows[i][2].ToString());
                                    totalID = double.Parse(dtWorkGroup.Rows[i][4].ToString());
                                    totalDDSum += totalDD;
                                    totalIDSum += totalID;
                                }
                                double totaltotal = totalDDSum + totalIDSum;
                                for (int i = 6; i < rowDataNum; i++)
                                {
                                    Misc.percentage(dtWorkGroup.Rows[i], totalDDSum, totalIDSum);
                                    dtWorkGroup.Rows[i][7] = double.Parse(dtWorkGroup.Rows[i][6].ToString()) * 100 / totaltotal;
                                    dtWorkGroup.AcceptChanges();

                                }
                                double totalPerDD = 0;
                                double totalPerID = 0;
                                double totalPerSum = 0;
                                for (int i = 6; i < rowDataNum; i++)
                                {
                                    totalPerDD += double.Parse(dtWorkGroup.Rows[i][3].ToString());
                                    totalPerID += double.Parse(dtWorkGroup.Rows[i][5].ToString());
                                    totalPerSum += double.Parse(dtWorkGroup.Rows[i][7].ToString());
                                }
                                dtWorkGroup.Rows[rowDataNum + 1][3] = totalPerDD;
                                dtWorkGroup.Rows[rowDataNum + 1][5] = totalPerID;
                                dtWorkGroup.Rows[rowDataNum + 1][7] = totalPerSum;

                                dtWorkGroup.Rows[rowDataNum + 2][3] = 0;
                                dtWorkGroup.Rows[rowDataNum + 2][5] = 0;
                                dtWorkGroup.Rows[rowDataNum + 2][7] = 0;

                                dtWorkGroup.Rows[rowDataNum + 3][3] = "  ";
                                dtWorkGroup.Rows[rowDataNum + 3][5] = "  ";
                                dtWorkGroup.Rows[rowDataNum + 3][7] = "  ";
                                dtWorkGroup.AcceptChanges();
                                numOfEmployees = numEmplWorkgroupDirect;
                                double totalDays = numOfEmployees * timeSpan.TotalDays;
                                DataRow rowEmptyFooter = dtWorkGroup.NewRow();
                                dtWorkGroup.Rows.Add(rowEmptyFooter);
                                DataRow footer = dtWorkGroup.NewRow();

                                //FOOTER ROW IN WORKGROUP TABLE
                                footer[1] = "N° Employees: " + numOfEmployees;
                                footer[2] = "Total days: ";
                                footer[3] = totalDays;
                                footer[4] = "Calendar days: ";
                                footer[5] = timeSpan.TotalDays;

                                dtWorkGroup.Rows.Add(footer);
                                ds.Tables.Add(dtWorkGroup);
                                dCost++;
                                if (dCost == 1)
                                {
                                    //FIRST TIME FILL OF TABLE COST CENTER, FIRST WORKGROUP IN CC
                                    string ute = "";
                                    string workGroup = "";
                                    string costCenterString = "";
                                    //string plantString = "";
                                    DataRow rowHeader1 = dtCostCenter.NewRow();

                                    if (dbConnection == null)
                                        rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
                                    else
                                        rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
                                    rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                    rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                    //rowHeader1[8] = "page: " + page;
                                    dtCostCenter.Rows.Add(rowHeader1);

                                    DataRow rowHeader2 = dtCostCenter.NewRow();
                                    rowHeader2[1] = "   Absenteeism industrial relation";
                                    rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                    rowHeader2[7] = datesList[0].ToString("MMMM yyyy");

                                    dtCostCenter.Rows.Add(rowHeader2);
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

                                    DataRow rowPlant = dtCostCenter.NewRow();
                                    rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
                                    dtCostCenter.Rows.Add(rowPlant);
                                    DataRow rowWorkgroup = dtCostCenter.NewRow();
                                    rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
                                    dtCostCenter.Rows.Add(rowWorkgroup);
                                    DataRow rowEmpty = dtCostCenter.NewRow();
                                    dtCostCenter.Rows.Add(rowEmpty);

                                    DataRow rowColumns = dtCostCenter.NewRow();
                                    rowColumns[2] = "Direct";
                                    rowColumns[3] = "%";
                                    rowColumns[4] = "Indirect";
                                    rowColumns[5] = "%";
                                    rowColumns[6] = "Total";
                                    rowColumns[7] = "%";
                                    dtCostCenter.Rows.Add(rowColumns);

                                    //FIRST 6 ROWS ARE HEADER OF TABLE WORKGROUP
                                    for (int ind = 6; ind < dtWorkGroup.Rows.Count - 2; ind++)
                                    {
                                        DataRow row = dtCostCenter.NewRow();
                                        row[1] = dtWorkGroup.Rows[ind][1];
                                        row[2] = dtWorkGroup.Rows[ind][2];
                                        row[4] = dtWorkGroup.Rows[ind][4];
                                        row[6] = dtWorkGroup.Rows[ind][6];
                                        dtCostCenter.Rows.Add(row);
                                        dtCostCenter.AcceptChanges();

                                    }
                                }
                                else
                                {
                                    for (int ind = 6; ind < dtWorkGroup.Rows.Count - 2; ind++)
                                    {
                                        //23 IS EMPTY ROW
                                        // 03.03.2014. Sanja - two holliady rows added
                                        if (ind != rowDataNum)
                                        //if (ind != 26)
                                        {
                                            //26 IS ROW +/-, DIFFERENT
                                            if (ind != (rowDataNum + 3))
                                            {
                                                dtCostCenter.Rows[ind][1] = dtWorkGroup.Rows[ind][1];
                                                dtCostCenter.Rows[ind][2] = double.Parse(dtCostCenter.Rows[ind][2].ToString()) + double.Parse(dtWorkGroup.Rows[ind][2].ToString());
                                                dtCostCenter.Rows[ind][4] = double.Parse(dtCostCenter.Rows[ind][4].ToString()) + double.Parse(dtWorkGroup.Rows[ind][4].ToString());
                                                dtCostCenter.Rows[ind][6] = double.Parse(dtCostCenter.Rows[ind][6].ToString()) + double.Parse(dtWorkGroup.Rows[ind][6].ToString());
                                            }
                                            else
                                            {
                                                dtCostCenter.Rows[ind][1] = dtWorkGroup.Rows[ind - 1][1];
                                                dtCostCenter.Rows[ind][2] = double.Parse(dtCostCenter.Rows[ind][2].ToString()) + double.Parse(dtWorkGroup.Rows[ind][2].ToString());
                                                dtCostCenter.Rows[ind][4] = double.Parse(dtCostCenter.Rows[ind][4].ToString()) + double.Parse(dtWorkGroup.Rows[ind][4].ToString());
                                                dtCostCenter.Rows[ind][6] = double.Parse(dtCostCenter.Rows[ind][6].ToString()) + double.Parse(dtWorkGroup.Rows[ind][6].ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (dbConnection == null)
                        {
                            wu = new WorkingUnit();
                        }
                        else
                        {
                            wu = new WorkingUnit(dbConnection);
                        }
                        List<WorkingUnitTO> oneCostCenter = new List<WorkingUnitTO>();
                        oneCostCenter.Add(costCenter);
                        List<WorkingUnitTO> CostCenterAndWorkShop = wu.FindAllChildren(oneCostCenter);
                        if (dtCostCenter.Rows.Count > 0)
                        {
                            double totalDD = 0;
                            double totalID = 0;
                            double totalDDSum = 0;
                            double totalIDSum = 0;
                            for (int i = 6; i < rowDataNum; i++)
                            {
                                totalDD = double.Parse(dtCostCenter.Rows[i][2].ToString());
                                totalID = double.Parse(dtCostCenter.Rows[i][4].ToString());
                                totalDDSum += totalDD;
                                totalIDSum += totalID;
                            }
                            double totaltotal = totalDDSum + totalIDSum;
                            for (int i = 6; i < rowDataNum; i++)
                            {
                                Misc.percentage(dtCostCenter.Rows[i], totalDDSum, totalIDSum);
                                dtCostCenter.Rows[i][7] = double.Parse(dtCostCenter.Rows[i][6].ToString()) * 100 / totaltotal;
                                dtCostCenter.AcceptChanges();

                            }
                            double totalPerDD = 0;
                            double totalPerID = 0;
                            double totalPerSum = 0;
                            for (int i = 6; i < rowDataNum; i++)
                            {
                                totalPerDD += double.Parse(dtCostCenter.Rows[i][3].ToString());
                                totalPerID += double.Parse(dtCostCenter.Rows[i][5].ToString());
                                totalPerSum += double.Parse(dtCostCenter.Rows[i][7].ToString());
                            }
                            dtCostCenter.Rows[rowDataNum + 1][3] = totalPerDD;
                            dtCostCenter.Rows[rowDataNum + 1][5] = totalPerID;
                            dtCostCenter.Rows[rowDataNum + 1][7] = totalPerSum;

                            dtCostCenter.Rows[rowDataNum + 2][3] = 0;
                            dtCostCenter.Rows[rowDataNum + 2][5] = 0;
                            dtCostCenter.Rows[rowDataNum + 2][7] = 0;

                            dtCostCenter.Rows[rowDataNum + 3][3] = "  ";
                            dtCostCenter.Rows[rowDataNum + 3][5] = "  ";
                            dtCostCenter.Rows[rowDataNum + 3][7] = "  ";
                            dtCostCenter.AcceptChanges();
                            //CC FOOTER ROW
                            numOfEmployees = numEmplCostDirect;
                            double totalDays = numOfEmployees * timeSpan.TotalDays;

                            DataRow rowEmptyFooter = dtCostCenter.NewRow();
                            dtCostCenter.Rows.Add(rowEmptyFooter);

                            DataRow footer = dtCostCenter.NewRow();
                            footer[1] = "N° Employees: " + numOfEmployees;
                            footer[2] = "Total days: ";
                            footer[3] = totalDays;
                            footer[4] = "Calendar days: ";
                            footer[5] = timeSpan.TotalDays;
                            dtCostCenter.Rows.Add(footer);

                            ds.Tables.Add(dtCostCenter);

                            dPlant++;
                            if (dPlant == 1)
                            {
                                //FIRST TIME FILL PLAN TABLE
                                DataRow rowHeader1 = dtPlant.NewRow();
                                if (dbConnection == null)
                                    rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
                                else
                                    rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
                                rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                //rowHeader1[8] = "page: " + page;
                                dtPlant.Rows.Add(rowHeader1);

                                DataRow rowHeader2 = dtPlant.NewRow();
                                rowHeader2[1] = "   Absenteeism industrial relation";
                                rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                                dtPlant.Rows.Add(rowHeader2);

                                DataRow rowPlant = dtPlant.NewRow();
                                rowPlant[1] = "Plant:    " + plantString + "    Cost center: " + "xxxx";
                                dtPlant.Rows.Add(rowPlant);

                                DataRow rowWorkgroup = dtPlant.NewRow();
                                rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
                                dtPlant.Rows.Add(rowWorkgroup);

                                DataRow rowEmpty = dtPlant.NewRow();
                                dtPlant.Rows.Add(rowEmpty);

                                DataRow rowColumns = dtPlant.NewRow();
                                rowColumns[2] = "Direct";
                                rowColumns[3] = "%";
                                rowColumns[4] = "Indirect";
                                rowColumns[5] = "%";
                                rowColumns[6] = "Total";
                                rowColumns[7] = "%";
                                dtPlant.Rows.Add(rowColumns);

                                //FIRST 6 ROWS OF CC TABLE ARE HEADER ROW
                                for (int ind = 6; ind < dtCostCenter.Rows.Count - 2; ind++)
                                {
                                    DataRow row = dtPlant.NewRow();
                                    row[0] = "";
                                    row[1] = dtCostCenter.Rows[ind][1];
                                    row[2] = dtCostCenter.Rows[ind][2];
                                    row[4] = dtCostCenter.Rows[ind][4];
                                    row[6] = dtCostCenter.Rows[ind][6];
                                    dtPlant.Rows.Add(row);
                                    dtPlant.AcceptChanges();

                                }
                            }
                            else
                            {
                                for (int ind = 6; ind < dtCostCenter.Rows.Count - 2; ind++)
                                {
                                    //23 EMPTY ROW
                                    // 03.03.2014. Sanja - two holliady rows added
                                    if (ind != rowDataNum)
                                    //if (ind != 26)
                                    {
                                        //26 ROW +/-
                                        if (ind != (rowDataNum + 3))
                                        {

                                            dtPlant.Rows[ind][1] = dtCostCenter.Rows[ind][1];
                                            dtPlant.Rows[ind][2] = double.Parse(dtPlant.Rows[ind][2].ToString()) + double.Parse(dtCostCenter.Rows[ind][2].ToString());
                                            dtPlant.Rows[ind][4] = double.Parse(dtPlant.Rows[ind][4].ToString()) + double.Parse(dtCostCenter.Rows[ind][4].ToString());
                                            dtPlant.Rows[ind][6] = double.Parse(dtPlant.Rows[ind][6].ToString()) + double.Parse(dtCostCenter.Rows[ind][6].ToString());
                                        }
                                        else
                                        {
                                            dtPlant.Rows[ind][1] = dtCostCenter.Rows[ind - 1][1];
                                            dtPlant.Rows[ind][2] = double.Parse(dtPlant.Rows[ind][2].ToString()) + double.Parse(dtCostCenter.Rows[ind][2].ToString());
                                            dtPlant.Rows[ind][4] = double.Parse(dtPlant.Rows[ind][4].ToString()) + double.Parse(dtCostCenter.Rows[ind][4].ToString());
                                            dtPlant.Rows[ind][6] = double.Parse(dtPlant.Rows[ind][6].ToString()) + double.Parse(dtCostCenter.Rows[ind][6].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (dbConnection == null)
                    {
                        wu = new WorkingUnit();
                    }
                    else
                    {
                        wu = new WorkingUnit(dbConnection);
                    }

                    List<WorkingUnitTO> onePlant = new List<WorkingUnitTO>();
                    onePlant.Add(plant);
                    List<WorkingUnitTO> PlantAndCostCentre = wu.FindAllChildren(onePlant);
                    if (dtPlant.Rows.Count > 0)
                    {
                        double totalDD = 0;
                        double totalID = 0;
                        double totalDDSum = 0;
                        double totalIDSum = 0;
                        for (int i = 6; i < rowDataNum; i++)
                        {
                            totalDD = double.Parse(dtPlant.Rows[i][2].ToString());
                            totalID = double.Parse(dtPlant.Rows[i][4].ToString());
                            totalDDSum += totalDD;
                            totalIDSum += totalID;
                        }
                        double totaltotal = totalDDSum + totalIDSum;
                        for (int i = 6; i < rowDataNum; i++)
                        {
                            Misc.percentage(dtPlant.Rows[i], totalDDSum, totalIDSum);
                            dtPlant.Rows[i][7] = double.Parse(dtPlant.Rows[i][6].ToString()) * 100 / totaltotal;
                            dtPlant.AcceptChanges();

                        }
                        double totalPerDD = 0;
                        double totalPerID = 0;
                        double totalPerSum = 0;
                        for (int i = 6; i < rowDataNum; i++)
                        {
                            totalPerDD += double.Parse(dtPlant.Rows[i][3].ToString());
                            totalPerID += double.Parse(dtPlant.Rows[i][5].ToString());
                            totalPerSum += double.Parse(dtPlant.Rows[i][7].ToString());
                        }
                        dtPlant.Rows[rowDataNum + 1][3] = totalPerDD;
                        dtPlant.Rows[rowDataNum + 1][5] = totalPerID;
                        dtPlant.Rows[rowDataNum + 1][7] = totalPerSum;

                        dtPlant.Rows[rowDataNum + 2][3] = 0;
                        dtPlant.Rows[rowDataNum + 2][5] = 0;
                        dtPlant.Rows[rowDataNum + 2][7] = 0;

                        dtPlant.Rows[rowDataNum + 3][3] = "  ";
                        dtPlant.Rows[rowDataNum + 3][5] = "  ";
                        dtPlant.Rows[rowDataNum + 3][7] = "  ";
                        dtPlant.AcceptChanges();
                        //PLANT FOOTER ROW
                        numOfEmployees = numEmplPlantDirect;
                        double totalDays = numOfEmployees * timeSpan.TotalDays;
                        DataRow rowEmptyFooter = dtPlant.NewRow();
                        dtPlant.Rows.Add(rowEmptyFooter);

                        DataRow footer = dtPlant.NewRow();
                        footer[1] = "N° Employees: " + numOfEmployees;
                        footer[2] = "Total days: ";
                        footer[3] = totalDays;
                        footer[4] = "Calendar days: ";
                        footer[5] = timeSpan.TotalDays;

                        dtPlant.Rows.Add(footer);
                        ds.Tables.Add(dtPlant);

                        dcompany++;
                        if (dcompany == 1)
                        {
                            //FIRST TIME FILL PLAN TABLE
                            DataRow rowHeader1 = dtCompany.NewRow();
                            if (dbConnection == null)
                                rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
                            else
                                rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
                            rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                            rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                            //rowHeader1[8] = "page: " + page;
                            dtCompany.Rows.Add(rowHeader1);

                            DataRow rowHeader2 = dtCompany.NewRow();
                            rowHeader2[1] = "   Absenteeism industrial relation";
                            rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                            rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                            dtCompany.Rows.Add(rowHeader2);

                            DataRow rowPlant = dtCompany.NewRow();
                            rowPlant[1] = "Company report";
                            dtCompany.Rows.Add(rowPlant);

                            DataRow rowWorkgroup = dtCompany.NewRow();
                            //rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
                            dtCompany.Rows.Add(rowWorkgroup);

                            DataRow rowEmpty = dtCompany.NewRow();
                            dtCompany.Rows.Add(rowEmpty);

                            DataRow rowColumns = dtCompany.NewRow();
                            rowColumns[2] = "Direct";
                            rowColumns[3] = "%";
                            rowColumns[4] = "Indirect";
                            rowColumns[5] = "%";
                            rowColumns[6] = "Total";
                            rowColumns[7] = "%";
                            dtCompany.Rows.Add(rowColumns);

                            //FIRST 6 ROWS OF CC TABLE ARE HEADER ROW
                            for (int ind = 6; ind < dtPlant.Rows.Count - 2; ind++)
                            {
                                DataRow row = dtCompany.NewRow();
                                row[0] = "";
                                row[1] = dtPlant.Rows[ind][1];
                                row[2] = dtPlant.Rows[ind][2];
                                row[4] = dtPlant.Rows[ind][4];
                                row[6] = dtPlant.Rows[ind][6];
                                dtCompany.Rows.Add(row);
                                dtCompany.AcceptChanges();

                            }
                        }
                        else
                        {
                            for (int ind = 6; ind < dtPlant.Rows.Count - 2; ind++)
                            {
                                //23 EMPTY ROW
                                // 03.03.2014. Sanja - two holliady rows added
                                if (ind != rowDataNum)
                                //if (ind != 26)
                                {
                                    //26 ROW +/-
                                    if (ind != (rowDataNum + 3))
                                    {

                                        dtCompany.Rows[ind][1] = dtPlant.Rows[ind][1];
                                        dtCompany.Rows[ind][2] = double.Parse(dtCompany.Rows[ind][2].ToString()) + double.Parse(dtPlant.Rows[ind][2].ToString());
                                        dtCompany.Rows[ind][4] = double.Parse(dtCompany.Rows[ind][4].ToString()) + double.Parse(dtPlant.Rows[ind][4].ToString());
                                        dtCompany.Rows[ind][6] = double.Parse(dtCompany.Rows[ind][6].ToString()) + double.Parse(dtPlant.Rows[ind][6].ToString());

                                    }
                                    else
                                    {
                                        dtCompany.Rows[ind][1] = dtPlant.Rows[ind][1];
                                        dtCompany.Rows[ind][2] = double.Parse(dtCompany.Rows[ind][2].ToString()) + double.Parse(dtPlant.Rows[ind][2].ToString());
                                        dtCompany.Rows[ind][4] = double.Parse(dtCompany.Rows[ind][4].ToString()) + double.Parse(dtPlant.Rows[ind][4].ToString());
                                        dtCompany.Rows[ind][6] = double.Parse(dtCompany.Rows[ind][6].ToString()) + double.Parse(dtPlant.Rows[ind][6].ToString());
                                    }
                                }
                            }
                        }

                    }
                }

                if (dtCompany.Rows.Count > 0)
                {
                    double totalDD = 0;
                    double totalID = 0;
                    double totalDDSum = 0;
                    double totalIDSum = 0;
                    for (int i = 6; i < rowDataNum; i++)
                    {
                        totalDD = double.Parse(dtCompany.Rows[i][2].ToString());
                        totalID = double.Parse(dtCompany.Rows[i][4].ToString());
                        totalDDSum += totalDD;
                        totalIDSum += totalID;
                    }
                    double totaltotal = totalDDSum + totalIDSum;
                    for (int i = 6; i < rowDataNum; i++)
                    {
                        Misc.percentage(dtCompany.Rows[i], totalDDSum, totalIDSum);
                        dtCompany.Rows[i][7] = double.Parse(dtCompany.Rows[i][6].ToString()) * 100 / totaltotal;
                        dtCompany.AcceptChanges();

                    }
                    double totalPerDD = 0;
                    double totalPerID = 0;
                    double totalPerSum = 0;
                    for (int i = 6; i < rowDataNum; i++)
                    {
                        totalPerDD += double.Parse(dtCompany.Rows[i][3].ToString());
                        totalPerID += double.Parse(dtCompany.Rows[i][5].ToString());
                        totalPerSum += double.Parse(dtCompany.Rows[i][7].ToString());
                    }
                    dtCompany.Rows[rowDataNum + 1][3] = totalPerDD;
                    dtCompany.Rows[rowDataNum + 1][5] = totalPerID;
                    dtCompany.Rows[rowDataNum + 1][7] = totalPerSum;

                    dtCompany.Rows[rowDataNum + 2][3] = 0;
                    dtCompany.Rows[rowDataNum + 2][5] = 0;
                    dtCompany.Rows[rowDataNum + 2][7] = 0;

                    dtCompany.Rows[rowDataNum + 3][3] = "  ";
                    dtCompany.Rows[rowDataNum + 3][5] = "  ";
                    dtCompany.Rows[rowDataNum + 3][7] = "  ";
                    dtCompany.AcceptChanges();
                    //PLANT FOOTER ROW
                    numOfEmployees = numEmplCompanyDirect;
                    double totalDays = numOfEmployees * timeSpan.TotalDays;
                    DataRow rowEmptyFooter = dtCompany.NewRow();
                    dtCompany.Rows.Add(rowEmptyFooter);

                    DataRow footer = dtCompany.NewRow();
                    footer[1] = "N° Employees: " + numOfEmployees;
                    footer[2] = "Total days: ";
                    footer[3] = totalDays;
                    footer[4] = "Calendar days: ";
                    footer[5] = timeSpan.TotalDays;

                    dtCompany.Rows.Add(footer);
                    ds.Tables.Add(dtCompany);
                }
                string Pathh = Directory.GetParent(fileName).FullName;
                if (!Directory.Exists(Pathh))
                {
                    Directory.CreateDirectory(Pathh);
                }
                if (File.Exists(fileName))
                    File.Delete(fileName);

                ExportToExcel.CreateExcelDocument(ds, fileName, false, false);

                debug.writeLog("+ Finished 500! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                return fileName;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "generateExcel500() " + ex.Message);
                return "";
            }
        }


        //public string GenerateReportOld(Object dbConnection, List<DateTime> datesList, string fileName, List<WorkingUnitTO> listWU, int company)
        //{
        //    debug.writeLog("+ Started 500! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));


        //    DataSet ds = new DataSet(Path.GetFileNameWithoutExtension(fileName));
        //    try
        //    {
        //        TimeSpan timeSpan = datesList[datesList.Count - 1] - datesList[0].AddDays(-1);
        //        int numOfEmployees = 0;
        //        WorkingUnit wu;
        //        Employee Empl;

        //        if (dbConnection == null)
        //        {
        //            wu = new WorkingUnit();
        //            Empl = new Employee();
        //        }
        //        else
        //        {
        //            wu = new WorkingUnit(dbConnection);
        //            Empl = new Employee(dbConnection);
        //        }
        //        System.Data.DataTable dtCompany = new System.Data.DataTable(company + "BC");
        //        dtCompany.Columns.Add("1", typeof(string));
        //        dtCompany.Columns.Add("   ", typeof(string));
        //        dtCompany.Columns.Add("Direct", typeof(string));
        //        dtCompany.Columns.Add("%  ", typeof(string));
        //        dtCompany.Columns.Add("Indirect", typeof(string));
        //        dtCompany.Columns.Add("%   ", typeof(string));
        //        dtCompany.Columns.Add("Total", typeof(string));
        //        dtCompany.Columns.Add(" %   ", typeof(string));


        //        int dcompany = 0;
        //        int numEmplCompanyDirect = 0;


        //        foreach (WorkingUnitTO plant in listWU)
        //        {
        //            string plantString = "";
        //            int dPlant = 0;
        //            int numEmplPlantDirect = 0;
        //            if (dbConnection == null)
        //            {
        //                wu = new WorkingUnit();
        //            }
        //            else
        //            {
        //                wu = new WorkingUnit(dbConnection);
        //            }
        //            System.Data.DataTable dtPlant = new System.Data.DataTable(plant.Name + "direct");
        //            dtPlant.Columns.Add("1", typeof(string));
        //            dtPlant.Columns.Add("   ", typeof(string));
        //            dtPlant.Columns.Add("Direct", typeof(string));
        //            dtPlant.Columns.Add("%  ", typeof(string));
        //            dtPlant.Columns.Add("Indirect", typeof(string));
        //            dtPlant.Columns.Add("%   ", typeof(string));
        //            dtPlant.Columns.Add("Total", typeof(string));
        //            dtPlant.Columns.Add(" %   ", typeof(string));

        //            List<WorkingUnitTO> listCostCenter = wu.SearchChildWU(plant.WorkingUnitID.ToString());

        //            foreach (WorkingUnitTO costCenter in listCostCenter)
        //            {
        //                int numEmplCostDirect = 0;
        //                int dCost = 0;
        //                if (dbConnection == null)
        //                {
        //                    wu = new WorkingUnit();
        //                }
        //                else
        //                {
        //                    wu = new WorkingUnit(dbConnection);
        //                }

        //                System.Data.DataTable dtCostCenter = new System.Data.DataTable(costCenter.Name + "direct");
        //                dtCostCenter.Columns.Add("1", typeof(string));
        //                dtCostCenter.Columns.Add("   ", typeof(string));
        //                dtCostCenter.Columns.Add("Direct", typeof(string));
        //                dtCostCenter.Columns.Add("%  ", typeof(string));
        //                dtCostCenter.Columns.Add("Indirect", typeof(string));
        //                dtCostCenter.Columns.Add("%   ", typeof(string));
        //                dtCostCenter.Columns.Add("Total", typeof(string));
        //                dtCostCenter.Columns.Add(" %   ", typeof(string));


        //                List<WorkingUnitTO> listWorkshop = wu.SearchChildWU(costCenter.WorkingUnitID.ToString());

        //                foreach (WorkingUnitTO workshop in listWorkshop)
        //                {
        //                    int numEmplWorkgroupDirect = 0;
        //                    int d = 0; //TO CHECK IS IT FIRST TIME FILL DATATABLE FOR WORKSHOP
        //                    if (dbConnection == null)
        //                    {
        //                        wu = new WorkingUnit();
        //                    }
        //                    else
        //                    {
        //                        wu = new WorkingUnit(dbConnection);
        //                    }
        //                    System.Data.DataTable dtWorkGroup = new System.Data.DataTable(workshop.Name + "direct");
        //                    dtWorkGroup.Columns.Add("1", typeof(string));
        //                    dtWorkGroup.Columns.Add("   ", typeof(string));
        //                    dtWorkGroup.Columns.Add("Direct", typeof(string));
        //                    dtWorkGroup.Columns.Add("%  ", typeof(string));
        //                    dtWorkGroup.Columns.Add("Indirect", typeof(string));
        //                    dtWorkGroup.Columns.Add("%   ", typeof(string));
        //                    dtWorkGroup.Columns.Add("Total", typeof(string));
        //                    dtWorkGroup.Columns.Add(" %   ", typeof(string));

        //                    List<WorkingUnitTO> listUTE = wu.SearchChildWU(workshop.WorkingUnitID.ToString());
        //                    foreach (WorkingUnitTO workingUnit in listUTE)
        //                    {
        //                        System.Data.DataTable dtUTE = new System.Data.DataTable(workingUnit.Name + "direct");
        //                        dtUTE.Columns.Add("1", typeof(string));
        //                        dtUTE.Columns.Add("   ", typeof(string));
        //                        dtUTE.Columns.Add("Direct", typeof(string));
        //                        dtUTE.Columns.Add("%  ", typeof(string));
        //                        dtUTE.Columns.Add("Indirect", typeof(string));
        //                        dtUTE.Columns.Add("%   ", typeof(string));
        //                        dtUTE.Columns.Add("Total", typeof(string));
        //                        dtUTE.Columns.Add(" %   ", typeof(string));
        //                        ds.Tables.Add(dtUTE);

        //                        List<EmployeeTO> listEmpl = new List<EmployeeTO>();

        //                        listEmpl = Empl.SearchByWULoans(workingUnit.WorkingUnitID.ToString(), -1, null, datesList[0], datesList[datesList.Count - 1]);
        //                        numOfEmployees = listEmpl.Count;
        //                        numEmplWorkgroupDirect += numOfEmployees;
        //                        numEmplCostDirect += numOfEmployees;
        //                        numEmplPlantDirect += numOfEmployees;
        //                        numEmplCompanyDirect += numOfEmployees;
        //                        double totalDays = numOfEmployees * timeSpan.TotalDays;
        //                        //POPULATE TABLE FOR UTE AND THAN USE THIS DATA FOR WORKGROUP, COST CENTER, PLANT
        //                        if (!populateDataTable500(dbConnection, dtUTE, workingUnit, datesList, company, "   Absenteeism industrial relation", 1))
        //                        {

        //                            //LAST EMPTY ROW
        //                            DataRow rowEmptyFooter = dtUTE.NewRow();
        //                            dtUTE.Rows.Add(rowEmptyFooter);

        //                            //FOOTER ROW
        //                            DataRow footer = dtUTE.NewRow();
        //                            footer[1] = "N° Employees: " + numOfEmployees;
        //                            footer[2] = "Total days: ";
        //                            footer[3] = totalDays;
        //                            footer[4] = "Calendar days: ";
        //                            footer[5] = timeSpan.TotalDays;

        //                            dtUTE.Rows.Add(footer);

        //                            d++;
        //                            if (d == 1)
        //                            {
        //                                //FIRST TIME FILL DATATABLE FOR WORKSHOP, FIRST HEADER AND THEN FOREACH ROW OF UTE TABLE SET ROW FOR WORKGROUP TABLE
        //                                string ute = "";
        //                                string workGroup = "";
        //                                string costCenterString = "";
        //                                //string plantString = "";
        //                                DataRow rowHeader1 = dtWorkGroup.NewRow();

        //                                if (dbConnection == null)
        //                                    rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
        //                                else
        //                                    rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
        //                                rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
        //                                rowHeader1[7] = DateTime.Now.ToString("HH:mm");
        //                                //rowHeader1[8] = "page: " + page;
        //                                dtWorkGroup.Rows.Add(rowHeader1);

        //                                DataRow rowHeader2 = dtWorkGroup.NewRow();
        //                                rowHeader2[1] = "   Absenteeism industrial relation";
        //                                rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
        //                                rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
        //                                dtWorkGroup.Rows.Add(rowHeader2);

        //                                if (dbConnection == null)
        //                                    wu = new WorkingUnit();
        //                                else
        //                                    wu = new WorkingUnit(dbConnection);

        //                                WorkingUnitTO tempWU = wu.FindWU(workingUnit.WorkingUnitID);
        //                                ute = tempWU.Code.Trim();

        //                                // get workshop (parent of UTE)
        //                                wu.WUTO = tempWU;
        //                                tempWU = wu.getParentWorkingUnit();
        //                                workGroup = tempWU.Code.Trim();

        //                                // get cost centar
        //                                wu.WUTO = tempWU;
        //                                tempWU = wu.getParentWorkingUnit();
        //                                costCenterString = tempWU.Code.Trim();

        //                                // get plant
        //                                wu.WUTO = tempWU;
        //                                tempWU = wu.getParentWorkingUnit();
        //                                plantString = tempWU.Code.Trim();


        //                                DataRow rowPlant = dtWorkGroup.NewRow();
        //                                rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
        //                                dtWorkGroup.Rows.Add(rowPlant);

        //                                DataRow rowWorkgroup = dtWorkGroup.NewRow();
        //                                rowWorkgroup[1] = "Workgroup: " + workGroup + " UTE: " + "xx";
        //                                dtWorkGroup.Rows.Add(rowWorkgroup);

        //                                DataRow rowEmpty = dtWorkGroup.NewRow();
        //                                dtWorkGroup.Rows.Add(rowEmpty);

        //                                //ROW COLUMN HEADER
        //                                DataRow rowColumns = dtWorkGroup.NewRow();
        //                                rowColumns[2] = "Direct";
        //                                rowColumns[3] = "%";
        //                                rowColumns[4] = "Indirect";
        //                                rowColumns[5] = "%";
        //                                rowColumns[6] = "Total";
        //                                rowColumns[7] = "%";
        //                                dtWorkGroup.Rows.Add(rowColumns);

        //                                //FIRST 6 ROWS OF UTE TABLE ARE HEADER, DONT NEED IT HERE
        //                                for (int ind = 6; ind < dtUTE.Rows.Count - 2; ind++)
        //                                {

        //                                    DataRow row = dtWorkGroup.NewRow();
        //                                    row[1] = dtUTE.Rows[ind][1];
        //                                    row[2] = dtUTE.Rows[ind][2];
        //                                    //row[3] = dtUTE.Rows[ind][3];
        //                                    row[4] = dtUTE.Rows[ind][4];
        //                                    //row[5] = dtUTE.Rows[ind][5];
        //                                    row[6] = dtUTE.Rows[ind][6];
        //                                    //row[7] = dtUTE.Rows[ind][7];
        //                                    dtWorkGroup.Rows.Add(row);
        //                                    dtWorkGroup.AcceptChanges();

        //                                }
        //                            }
        //                            else
        //                            {
        //                                //IF IS NOT FIRST TIME, NOT FIRST UTE IN WORKGROUP THEN ADD TO EXISTING TABLE WORKGROUP
        //                                for (int ind = 6; ind < dtUTE.Rows.Count - 2; ind++)
        //                                {
        //                                    // EMPTY ROW
        //                                    if (ind != 24)
        //                                    {
        //                                        //ROW +/-
        //                                        if (ind != 27)
        //                                        {
        //                                            dtWorkGroup.Rows[ind][1] = dtUTE.Rows[ind][1];
        //                                            dtWorkGroup.Rows[ind][2] = double.Parse(dtWorkGroup.Rows[ind][2].ToString()) + double.Parse(dtUTE.Rows[ind][2].ToString());
        //                                            dtWorkGroup.Rows[ind][4] = double.Parse(dtWorkGroup.Rows[ind][4].ToString()) + double.Parse(dtUTE.Rows[ind][4].ToString());
        //                                            dtWorkGroup.Rows[ind][6] = double.Parse(dtWorkGroup.Rows[ind][6].ToString()) + double.Parse(dtUTE.Rows[ind][6].ToString());

        //                                        }
        //                                        else
        //                                        {
        //                                            dtWorkGroup.Rows[ind][1] = dtUTE.Rows[ind][1];
        //                                            dtWorkGroup.Rows[ind][2] = double.Parse(dtWorkGroup.Rows[ind][2].ToString()) + double.Parse(dtUTE.Rows[ind][2].ToString());
        //                                            dtWorkGroup.Rows[ind][4] = double.Parse(dtWorkGroup.Rows[ind][4].ToString()) + double.Parse(dtUTE.Rows[ind][4].ToString());
        //                                            dtWorkGroup.Rows[ind][6] = double.Parse(dtWorkGroup.Rows[ind][6].ToString()) + double.Parse(dtUTE.Rows[ind][6].ToString());
        //                                        }
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                    if (dbConnection == null)
        //                    {
        //                        wu = new WorkingUnit();
        //                    }
        //                    else
        //                    {
        //                        wu = new WorkingUnit(dbConnection);
        //                    }
        //                    List<WorkingUnitTO> oneWorkShop = new List<WorkingUnitTO>();
        //                    oneWorkShop.Add(workshop);
        //                    List<WorkingUnitTO> workShopAndUTE = wu.FindAllChildren(oneWorkShop);
        //                    if (dtWorkGroup.Rows.Count > 0)
        //                    {
        //                        double totalDD = 0;
        //                        double totalID = 0;
        //                        double totalDDSum = 0;
        //                        double totalIDSum = 0;
        //                        for (int i = 6; i < 24; i++)
        //                        {
        //                            totalDD = double.Parse(dtWorkGroup.Rows[i][2].ToString());
        //                            totalID = double.Parse(dtWorkGroup.Rows[i][4].ToString());
        //                            totalDDSum += totalDD;
        //                            totalIDSum += totalID;
        //                        }
        //                        double totaltotal = totalDDSum + totalIDSum;
        //                        for (int i = 6; i < 24; i++)
        //                        {
        //                            Misc.percentage(dtWorkGroup.Rows[i], totalDDSum, totalIDSum);
        //                            dtWorkGroup.Rows[i][7] = double.Parse(dtWorkGroup.Rows[i][6].ToString()) * 100 / totaltotal;
        //                            dtWorkGroup.AcceptChanges();

        //                        }
        //                        double totalPerDD = 0;
        //                        double totalPerID = 0;
        //                        double totalPerSum = 0;
        //                        for (int i = 6; i < 24; i++)
        //                        {
        //                            totalPerDD += double.Parse(dtWorkGroup.Rows[i][3].ToString());
        //                            totalPerID += double.Parse(dtWorkGroup.Rows[i][5].ToString());
        //                            totalPerSum += double.Parse(dtWorkGroup.Rows[i][7].ToString());
        //                        }
        //                        dtWorkGroup.Rows[25][3] = totalPerDD;
        //                        dtWorkGroup.Rows[25][5] = totalPerID;
        //                        dtWorkGroup.Rows[25][7] = totalPerSum;

        //                        dtWorkGroup.Rows[26][3] = 0;
        //                        dtWorkGroup.Rows[26][5] = 0;
        //                        dtWorkGroup.Rows[26][7] = 0;

        //                        dtWorkGroup.Rows[27][3] = "  ";
        //                        dtWorkGroup.Rows[27][5] = "  ";
        //                        dtWorkGroup.Rows[27][7] = "  ";
        //                        dtWorkGroup.AcceptChanges();
        //                        numOfEmployees = numEmplWorkgroupDirect;
        //                        double totalDays = numOfEmployees * timeSpan.TotalDays;
        //                        DataRow rowEmptyFooter = dtWorkGroup.NewRow();
        //                        dtWorkGroup.Rows.Add(rowEmptyFooter);
        //                        DataRow footer = dtWorkGroup.NewRow();

        //                        //FOOTER ROW IN WORKGROUP TABLE
        //                        footer[1] = "N° Employees: " + numOfEmployees;
        //                        footer[2] = "Total days: ";
        //                        footer[3] = totalDays;
        //                        footer[4] = "Calendar days: ";
        //                        footer[5] = timeSpan.TotalDays;

        //                        dtWorkGroup.Rows.Add(footer);
        //                        ds.Tables.Add(dtWorkGroup);
        //                        dCost++;
        //                        if (dCost == 1)
        //                        {
        //                            //FIRST TIME FILL OF TABLE COST CENTER, FIRST WORKGROUP IN CC
        //                            string ute = "";
        //                            string workGroup = "";
        //                            string costCenterString = "";
        //                            //string plantString = "";
        //                            DataRow rowHeader1 = dtCostCenter.NewRow();

        //                            if (dbConnection == null)
        //                                rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
        //                            else
        //                                rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
        //                            rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
        //                            rowHeader1[7] = DateTime.Now.ToString("HH:mm");
        //                            //rowHeader1[8] = "page: " + page;
        //                            dtCostCenter.Rows.Add(rowHeader1);

        //                            DataRow rowHeader2 = dtCostCenter.NewRow();
        //                            rowHeader2[1] = "   Absenteeism industrial relation";
        //                            rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
        //                            rowHeader2[7] = datesList[0].ToString("MMMM yyyy");

        //                            dtCostCenter.Rows.Add(rowHeader2);
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

        //                            DataRow rowPlant = dtCostCenter.NewRow();
        //                            rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
        //                            dtCostCenter.Rows.Add(rowPlant);
        //                            DataRow rowWorkgroup = dtCostCenter.NewRow();
        //                            rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
        //                            dtCostCenter.Rows.Add(rowWorkgroup);
        //                            DataRow rowEmpty = dtCostCenter.NewRow();
        //                            dtCostCenter.Rows.Add(rowEmpty);

        //                            DataRow rowColumns = dtCostCenter.NewRow();
        //                            rowColumns[2] = "Direct";
        //                            rowColumns[3] = "%";
        //                            rowColumns[4] = "Indirect";
        //                            rowColumns[5] = "%";
        //                            rowColumns[6] = "Total";
        //                            rowColumns[7] = "%";
        //                            dtCostCenter.Rows.Add(rowColumns);

        //                            //FIRST 6 ROWS ARE HEADER OF TABLE WORKGROUP
        //                            for (int ind = 6; ind < dtWorkGroup.Rows.Count - 2; ind++)
        //                            {
        //                                DataRow row = dtCostCenter.NewRow();
        //                                row[1] = dtWorkGroup.Rows[ind][1];
        //                                row[2] = dtWorkGroup.Rows[ind][2];
        //                                row[4] = dtWorkGroup.Rows[ind][4];
        //                                row[6] = dtWorkGroup.Rows[ind][6];
        //                                dtCostCenter.Rows.Add(row);
        //                                dtCostCenter.AcceptChanges();

        //                            }
        //                        }
        //                        else
        //                        {
        //                            for (int ind = 6; ind < dtWorkGroup.Rows.Count - 2; ind++)
        //                            {
        //                                //23 IS EMPTY ROW
        //                                if (ind != 24)
        //                                {
        //                                    //26 IS ROW +/-, DIFFERENT
        //                                    if (ind != 27)
        //                                    {
        //                                        dtCostCenter.Rows[ind][1] = dtWorkGroup.Rows[ind][1];
        //                                        dtCostCenter.Rows[ind][2] = double.Parse(dtCostCenter.Rows[ind][2].ToString()) + double.Parse(dtWorkGroup.Rows[ind][2].ToString());
        //                                        dtCostCenter.Rows[ind][4] = double.Parse(dtCostCenter.Rows[ind][4].ToString()) + double.Parse(dtWorkGroup.Rows[ind][4].ToString());
        //                                        dtCostCenter.Rows[ind][6] = double.Parse(dtCostCenter.Rows[ind][6].ToString()) + double.Parse(dtWorkGroup.Rows[ind][6].ToString());
        //                                    }
        //                                    else
        //                                    {
        //                                        dtCostCenter.Rows[ind][1] = dtWorkGroup.Rows[ind - 1][1];
        //                                        dtCostCenter.Rows[ind][2] = double.Parse(dtCostCenter.Rows[ind][2].ToString()) + double.Parse(dtWorkGroup.Rows[ind][2].ToString());
        //                                        dtCostCenter.Rows[ind][4] = double.Parse(dtCostCenter.Rows[ind][4].ToString()) + double.Parse(dtWorkGroup.Rows[ind][4].ToString());
        //                                        dtCostCenter.Rows[ind][6] = double.Parse(dtCostCenter.Rows[ind][6].ToString()) + double.Parse(dtWorkGroup.Rows[ind][6].ToString());
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                if (dbConnection == null)
        //                {
        //                    wu = new WorkingUnit();
        //                }
        //                else
        //                {
        //                    wu = new WorkingUnit(dbConnection);
        //                }
        //                List<WorkingUnitTO> oneCostCenter = new List<WorkingUnitTO>();
        //                oneCostCenter.Add(costCenter);
        //                List<WorkingUnitTO> CostCenterAndWorkShop = wu.FindAllChildren(oneCostCenter);
        //                if (dtCostCenter.Rows.Count > 0)
        //                {
        //                    double totalDD = 0;
        //                    double totalID = 0;
        //                    double totalDDSum = 0;
        //                    double totalIDSum = 0;
        //                    for (int i = 6; i < 23; i++)
        //                    {
        //                        totalDD = double.Parse(dtCostCenter.Rows[i][2].ToString());
        //                        totalID = double.Parse(dtCostCenter.Rows[i][4].ToString());
        //                        totalDDSum += totalDD;
        //                        totalIDSum += totalID;
        //                    }
        //                    double totaltotal = totalDDSum + totalIDSum;
        //                    for (int i = 6; i < 24; i++)
        //                    {
        //                        Misc.percentage(dtCostCenter.Rows[i], totalDDSum, totalIDSum);
        //                        dtCostCenter.Rows[i][7] = double.Parse(dtCostCenter.Rows[i][6].ToString()) * 100 / totaltotal;
        //                        dtCostCenter.AcceptChanges();

        //                    }
        //                    double totalPerDD = 0;
        //                    double totalPerID = 0;
        //                    double totalPerSum = 0;
        //                    for (int i = 6; i < 24; i++)
        //                    {
        //                        totalPerDD += double.Parse(dtCostCenter.Rows[i][3].ToString());
        //                        totalPerID += double.Parse(dtCostCenter.Rows[i][5].ToString());
        //                        totalPerSum += double.Parse(dtCostCenter.Rows[i][7].ToString());
        //                    }
        //                    dtCostCenter.Rows[25][3] = totalPerDD;
        //                    dtCostCenter.Rows[25][5] = totalPerID;
        //                    dtCostCenter.Rows[25][7] = totalPerSum;

        //                    dtCostCenter.Rows[26][3] = 0;
        //                    dtCostCenter.Rows[26][5] = 0;
        //                    dtCostCenter.Rows[26][7] = 0;

        //                    dtCostCenter.Rows[27][3] = "  ";
        //                    dtCostCenter.Rows[27][5] = "  ";
        //                    dtCostCenter.Rows[27][7] = "  ";
        //                    dtCostCenter.AcceptChanges();
        //                    //CC FOOTER ROW
        //                    numOfEmployees = numEmplCostDirect;
        //                    double totalDays = numOfEmployees * timeSpan.TotalDays;

        //                    DataRow rowEmptyFooter = dtCostCenter.NewRow();
        //                    dtCostCenter.Rows.Add(rowEmptyFooter);

        //                    DataRow footer = dtCostCenter.NewRow();
        //                    footer[1] = "N° Employees: " + numOfEmployees;
        //                    footer[2] = "Total days: ";
        //                    footer[3] = totalDays;
        //                    footer[4] = "Calendar days: ";
        //                    footer[5] = timeSpan.TotalDays;
        //                    dtCostCenter.Rows.Add(footer);

        //                    ds.Tables.Add(dtCostCenter);

        //                    dPlant++;
        //                    if (dPlant == 1)
        //                    {
        //                        //FIRST TIME FILL PLAN TABLE
        //                        DataRow rowHeader1 = dtPlant.NewRow();
        //                        if (dbConnection == null)
        //                            rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
        //                        else
        //                            rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
        //                        rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
        //                        rowHeader1[7] = DateTime.Now.ToString("HH:mm");
        //                        //rowHeader1[8] = "page: " + page;
        //                        dtPlant.Rows.Add(rowHeader1);

        //                        DataRow rowHeader2 = dtPlant.NewRow();
        //                        rowHeader2[1] = "   Absenteeism industrial relation";
        //                        rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
        //                        rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
        //                        dtPlant.Rows.Add(rowHeader2);

        //                        DataRow rowPlant = dtPlant.NewRow();
        //                        rowPlant[1] = "Plant:    " + plantString + "    Cost center: " + "xxxx";
        //                        dtPlant.Rows.Add(rowPlant);

        //                        DataRow rowWorkgroup = dtPlant.NewRow();
        //                        rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
        //                        dtPlant.Rows.Add(rowWorkgroup);

        //                        DataRow rowEmpty = dtPlant.NewRow();
        //                        dtPlant.Rows.Add(rowEmpty);

        //                        DataRow rowColumns = dtPlant.NewRow();
        //                        rowColumns[2] = "Direct";
        //                        rowColumns[3] = "%";
        //                        rowColumns[4] = "Indirect";
        //                        rowColumns[5] = "%";
        //                        rowColumns[6] = "Total";
        //                        rowColumns[7] = "%";
        //                        dtPlant.Rows.Add(rowColumns);

        //                        //FIRST 6 ROWS OF CC TABLE ARE HEADER ROW
        //                        for (int ind = 6; ind < dtCostCenter.Rows.Count - 2; ind++)
        //                        {
        //                            DataRow row = dtPlant.NewRow();
        //                            row[0] = "";
        //                            row[1] = dtCostCenter.Rows[ind][1];
        //                            row[2] = dtCostCenter.Rows[ind][2];
        //                            row[4] = dtCostCenter.Rows[ind][4];
        //                            row[6] = dtCostCenter.Rows[ind][6];
        //                            dtPlant.Rows.Add(row);
        //                            dtPlant.AcceptChanges();

        //                        }
        //                    }
        //                    else
        //                    {
        //                        for (int ind = 6; ind < dtCostCenter.Rows.Count - 2; ind++)
        //                        {
        //                            //23 EMPTY ROW
        //                            if (ind != 24)
        //                            {
        //                                //26 ROW +/-
        //                                if (ind != 27)
        //                                {

        //                                    dtPlant.Rows[ind][1] = dtCostCenter.Rows[ind][1];
        //                                    dtPlant.Rows[ind][2] = double.Parse(dtPlant.Rows[ind][2].ToString()) + double.Parse(dtCostCenter.Rows[ind][2].ToString());
        //                                    dtPlant.Rows[ind][4] = double.Parse(dtPlant.Rows[ind][4].ToString()) + double.Parse(dtCostCenter.Rows[ind][4].ToString());
        //                                    dtPlant.Rows[ind][6] = double.Parse(dtPlant.Rows[ind][6].ToString()) + double.Parse(dtCostCenter.Rows[ind][6].ToString());
        //                                }
        //                                else
        //                                {
        //                                    dtPlant.Rows[ind][1] = dtCostCenter.Rows[ind - 1][1];
        //                                    dtPlant.Rows[ind][2] = double.Parse(dtPlant.Rows[ind][2].ToString()) + double.Parse(dtCostCenter.Rows[ind][2].ToString());
        //                                    dtPlant.Rows[ind][4] = double.Parse(dtPlant.Rows[ind][4].ToString()) + double.Parse(dtCostCenter.Rows[ind][4].ToString());
        //                                    dtPlant.Rows[ind][6] = double.Parse(dtPlant.Rows[ind][6].ToString()) + double.Parse(dtCostCenter.Rows[ind][6].ToString());
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            if (dbConnection == null)
        //            {
        //                wu = new WorkingUnit();
        //            }
        //            else
        //            {
        //                wu = new WorkingUnit(dbConnection);
        //            }

        //            List<WorkingUnitTO> onePlant = new List<WorkingUnitTO>();
        //            onePlant.Add(plant);
        //            List<WorkingUnitTO> PlantAndCostCentre = wu.FindAllChildren(onePlant);
        //            if (dtPlant.Rows.Count > 0)
        //            {
        //                double totalDD = 0;
        //                double totalID = 0;
        //                double totalDDSum = 0;
        //                double totalIDSum = 0;
        //                for (int i = 6; i < 24; i++)
        //                {
        //                    totalDD = double.Parse(dtPlant.Rows[i][2].ToString());
        //                    totalID = double.Parse(dtPlant.Rows[i][4].ToString());
        //                    totalDDSum += totalDD;
        //                    totalIDSum += totalID;
        //                }
        //                double totaltotal = totalDDSum + totalIDSum;
        //                for (int i = 6; i < 24; i++)
        //                {
        //                    Misc.percentage(dtPlant.Rows[i], totalDDSum, totalIDSum);
        //                    dtPlant.Rows[i][7] = double.Parse(dtPlant.Rows[i][6].ToString()) * 100 / totaltotal;
        //                    dtPlant.AcceptChanges();

        //                }
        //                double totalPerDD = 0;
        //                double totalPerID = 0;
        //                double totalPerSum = 0;
        //                for (int i = 6; i < 24; i++)
        //                {
        //                    totalPerDD += double.Parse(dtPlant.Rows[i][3].ToString());
        //                    totalPerID += double.Parse(dtPlant.Rows[i][5].ToString());
        //                    totalPerSum += double.Parse(dtPlant.Rows[i][7].ToString());
        //                }
        //                dtPlant.Rows[25][3] = totalPerDD;
        //                dtPlant.Rows[25][5] = totalPerID;
        //                dtPlant.Rows[25][7] = totalPerSum;

        //                dtPlant.Rows[26][3] = 0;
        //                dtPlant.Rows[26][5] = 0;
        //                dtPlant.Rows[26][7] = 0;

        //                dtPlant.Rows[27][3] = "  ";
        //                dtPlant.Rows[27][5] = "  ";
        //                dtPlant.Rows[27][7] = "  ";
        //                dtPlant.AcceptChanges();
        //                //PLANT FOOTER ROW
        //                numOfEmployees = numEmplPlantDirect;
        //                double totalDays = numOfEmployees * timeSpan.TotalDays;
        //                DataRow rowEmptyFooter = dtPlant.NewRow();
        //                dtPlant.Rows.Add(rowEmptyFooter);

        //                DataRow footer = dtPlant.NewRow();
        //                footer[1] = "N° Employees: " + numOfEmployees;
        //                footer[2] = "Total days: ";
        //                footer[3] = totalDays;
        //                footer[4] = "Calendar days: ";
        //                footer[5] = timeSpan.TotalDays;

        //                dtPlant.Rows.Add(footer);
        //                ds.Tables.Add(dtPlant);

        //                dcompany++;
        //                if (dcompany == 1)
        //                {
        //                    //FIRST TIME FILL PLAN TABLE
        //                    DataRow rowHeader1 = dtCompany.NewRow();
        //                    if (dbConnection == null)
        //                        rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
        //                    else
        //                        rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
        //                    rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
        //                    rowHeader1[7] = DateTime.Now.ToString("HH:mm");
        //                    //rowHeader1[8] = "page: " + page;
        //                    dtCompany.Rows.Add(rowHeader1);

        //                    DataRow rowHeader2 = dtCompany.NewRow();
        //                    rowHeader2[1] = "   Absenteeism industrial relation";
        //                    rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
        //                    rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
        //                    dtCompany.Rows.Add(rowHeader2);

        //                    DataRow rowPlant = dtCompany.NewRow();
        //                    rowPlant[1] = "Company report";
        //                    dtCompany.Rows.Add(rowPlant);

        //                    DataRow rowWorkgroup = dtCompany.NewRow();
        //                    //rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
        //                    dtCompany.Rows.Add(rowWorkgroup);

        //                    DataRow rowEmpty = dtCompany.NewRow();
        //                    dtCompany.Rows.Add(rowEmpty);

        //                    DataRow rowColumns = dtCompany.NewRow();
        //                    rowColumns[2] = "Direct";
        //                    rowColumns[3] = "%";
        //                    rowColumns[4] = "Indirect";
        //                    rowColumns[5] = "%";
        //                    rowColumns[6] = "Total";
        //                    rowColumns[7] = "%";
        //                    dtCompany.Rows.Add(rowColumns);

        //                    //FIRST 6 ROWS OF CC TABLE ARE HEADER ROW
        //                    for (int ind = 6; ind < dtPlant.Rows.Count - 2; ind++)
        //                    {
        //                        DataRow row = dtCompany.NewRow();
        //                        row[0] = "";
        //                        row[1] = dtPlant.Rows[ind][1];
        //                        row[2] = dtPlant.Rows[ind][2];
        //                        row[4] = dtPlant.Rows[ind][4];
        //                        row[6] = dtPlant.Rows[ind][6];
        //                        dtCompany.Rows.Add(row);
        //                        dtCompany.AcceptChanges();

        //                    }
        //                }
        //                else
        //                {
        //                    for (int ind = 6; ind < dtPlant.Rows.Count - 2; ind++)
        //                    {
        //                        //23 EMPTY ROW
        //                        if (ind != 24)
        //                        {
        //                            //26 ROW +/-
        //                            if (ind != 27 && ind != 25)
        //                            {

        //                                dtCompany.Rows[ind][1] = dtPlant.Rows[ind][1];
        //                                dtCompany.Rows[ind][2] = double.Parse(dtCompany.Rows[ind][2].ToString()) + double.Parse(dtPlant.Rows[ind][2].ToString());
        //                                dtCompany.Rows[ind][4] = double.Parse(dtCompany.Rows[ind][4].ToString()) + double.Parse(dtPlant.Rows[ind][4].ToString());
        //                                dtCompany.Rows[ind][6] = double.Parse(dtCompany.Rows[ind][6].ToString()) + double.Parse(dtPlant.Rows[ind][6].ToString());

        //                            }
        //                            else
        //                            {
        //                                dtCompany.Rows[ind][1] = dtPlant.Rows[ind][1];
        //                                dtCompany.Rows[ind][2] = double.Parse(dtCompany.Rows[ind][2].ToString()) + double.Parse(dtPlant.Rows[ind][2].ToString());
        //                                dtCompany.Rows[ind][4] = double.Parse(dtCompany.Rows[ind][4].ToString()) + double.Parse(dtPlant.Rows[ind][4].ToString());
        //                                dtCompany.Rows[ind][6] = double.Parse(dtCompany.Rows[ind][6].ToString()) + double.Parse(dtPlant.Rows[ind][6].ToString());
        //                            }
        //                        }
        //                    }
        //                }

        //            }
        //        }

        //        if (dtCompany.Rows.Count > 0)
        //        {
        //            double totalDD = 0;
        //            double totalID = 0;
        //            double totalDDSum = 0;
        //            double totalIDSum = 0;
        //            for (int i = 6; i < 24; i++)
        //            {
        //                totalDD = double.Parse(dtCompany.Rows[i][2].ToString());
        //                totalID = double.Parse(dtCompany.Rows[i][4].ToString());
        //                totalDDSum += totalDD;
        //                totalIDSum += totalID;
        //            }
        //            double totaltotal = totalDDSum + totalIDSum;
        //            for (int i = 6; i < 24; i++)
        //            {
        //                Misc.percentage(dtCompany.Rows[i], totalDDSum, totalIDSum);
        //                dtCompany.Rows[i][7] = double.Parse(dtCompany.Rows[i][6].ToString()) * 100 / totaltotal;
        //                dtCompany.AcceptChanges();

        //            }
        //            double totalPerDD = 0;
        //            double totalPerID = 0;
        //            double totalPerSum = 0;
        //            for (int i = 6; i < 24; i++)
        //            {
        //                totalPerDD += double.Parse(dtCompany.Rows[i][3].ToString());
        //                totalPerID += double.Parse(dtCompany.Rows[i][5].ToString());
        //                totalPerSum += double.Parse(dtCompany.Rows[i][7].ToString());
        //            }
        //            dtCompany.Rows[25][3] = totalPerDD;
        //            dtCompany.Rows[25][5] = totalPerID;
        //            dtCompany.Rows[25][7] = totalPerSum;

        //            dtCompany.Rows[26][3] = 0;
        //            dtCompany.Rows[26][5] = 0;
        //            dtCompany.Rows[26][7] = 0;

        //            dtCompany.Rows[27][3] = "  ";
        //            dtCompany.Rows[27][5] = "  ";
        //            dtCompany.Rows[27][7] = "  ";
        //            dtCompany.AcceptChanges();
        //            //PLANT FOOTER ROW
        //            numOfEmployees = numEmplCompanyDirect;
        //            double totalDays = numOfEmployees * timeSpan.TotalDays;
        //            DataRow rowEmptyFooter = dtCompany.NewRow();
        //            dtCompany.Rows.Add(rowEmptyFooter);

        //            DataRow footer = dtCompany.NewRow();
        //            footer[1] = "N° Employees: " + numOfEmployees;
        //            footer[2] = "Total days: ";
        //            footer[3] = totalDays;
        //            footer[4] = "Calendar days: ";
        //            footer[5] = timeSpan.TotalDays;

        //            dtCompany.Rows.Add(footer);
        //            ds.Tables.Add(dtCompany);
        //        }
        //        string Pathh = Directory.GetParent(fileName).FullName;
        //        if (!Directory.Exists(Pathh))
        //        {
        //            Directory.CreateDirectory(Pathh);
        //        }
        //        if (File.Exists(fileName))
        //            File.Delete(fileName);

        //        ExportToExcel.CreateExcelDocument(ds, fileName, false, false);

        //        debug.writeLog("+ Finished 500! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
        //        return fileName;
        //    }
        //    catch (Exception ex)
        //    {
        //        debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "generateExcel500() " + ex.Message);
        //        return "";
        //    }
        //}
        
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

        private void calc400Lunch(List<IOPairProcessedTO> IOPairListDirect, List<IOPairProcessedTO> IOPairListIndirect, Dictionary<int, PassTypeTO> listPassTypes, string columnName, DataRow row,
             string emplIdsBS, string emplIds, List<DateTime> datesList)
        {
            try
            {

                row[1] = columnName;

                decimal totalDurationDirect = 0;
                decimal totalDurationIndirect = 0;

                foreach (IOPairProcessedTO iopair in IOPairListDirect)
                {

                    if (listPassTypes.ContainsKey(iopair.PassTypeID))
                    {
                        TimeSpan duration = iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay);
                        if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                            duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift
                        totalDurationDirect += (decimal)duration.TotalHours;
                    }

                }
                foreach (IOPairProcessedTO iopair in IOPairListIndirect)
                {
                    if (listPassTypes.ContainsKey(iopair.PassTypeID))
                    {

                        TimeSpan duration = iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay);
                        if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                            duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift
                        totalDurationIndirect += (decimal)duration.TotalHours;
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

        //private bool populateDataTable500(Object dbConnection, DataTable dt, WorkingUnitTO workingUnit, List<DateTime> datesList, int company, string title, int level)
        //{
        //    bool isEmpty = true;
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
        //        WorkingUnit wu;
        //        if (dbConnection == null)
        //            wu = new WorkingUnit();
        //        else
        //            wu = new WorkingUnit(dbConnection);
        //        Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDictionary = new Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>>();
        //        if (dbConnection == null)
        //            rulesDictionary = new Common.Rule().SearchWUEmplTypeDictionary();
        //        else rulesDictionary = new Common.Rule(dbConnection).SearchWUEmplTypeDictionary();

        //        Dictionary<int, PassTypeTO> passTypesDictionaryAll = new Dictionary<int, PassTypeTO>();
        //        if (dbConnection == null)
        //        {
        //            passTypesDictionaryAll = new PassType().SearchDictionary();
        //        }
        //        else
        //        {
        //            passTypesDictionaryAll = new PassType(dbConnection).SearchDictionary();
        //        }
        //        string ute = "";
        //        string workGroup = "";
        //        string costCenter = "";
        //        string plant = "";

        //        DataRow rowHeader1 = dt.NewRow();
        //        if (dbConnection == null)
        //            rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
        //        else
        //            rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
        //        rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
        //        rowHeader1[7] = DateTime.Now.ToString("HH:mm");
        //        //rowHeader1[8] = "page: " + page;
        //        dt.Rows.Add(rowHeader1);

        //        DataRow rowHeader2 = dt.NewRow();
        //        rowHeader2[1] = title;
        //        rowHeader2[6] = datesList[0].ToString("dd.MM") + "-" + datesList[datesList.Count - 1].ToString("dd.MM");
        //        //rowHeader2[7] = datesList[0].ToString("MMMM yyyy");

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
        //        Employee Empl;
        //        IOPairProcessed IOPairProc;
        //        PassType PassT;
        //        if (dbConnection == null)
        //        {
        //            Empl = new Employee();
        //            IOPairProc = new IOPairProcessed();
        //            PassT = new PassType();
        //        }
        //        else
        //        {
        //            Empl = new Employee(dbConnection);
        //            IOPairProc = new IOPairProcessed(dbConnection);
        //            PassT = new PassType(dbConnection);
        //        }
        //        List<EmployeeTO> listEmpl = new List<EmployeeTO>();

        //        listEmpl = Empl.SearchByWULoans(workingUnit.WorkingUnitID.ToString(), -1, null, datesList[0], datesList[datesList.Count - 1]);


        //        if (listEmpl.Count > 0)
        //        {
        //            string emplList = "";
        //            foreach (EmployeeTO empl in listEmpl)
        //            {

        //                emplList += empl.EmployeeID + ",";
        //            }
        //            if (emplList.Length > 0)
        //                emplList = emplList.Remove(emplList.LastIndexOf(','));


        //            Dictionary<string, string> dictEmplBranch = Misc.emplBranch(dbConnection, emplList, company);
        //            Dictionary<int, PassTypeTO> listPassTypes = new Dictionary<int, PassTypeTO>();
        //            string emplIdsDirect = "";
        //            string emplIdsIndirect = "";
        //            foreach (KeyValuePair<string, string> pair in dictEmplBranch)
        //            {
        //                if (pair.Value == "A")
        //                    emplIdsDirect += pair.Key + ",";
        //                else
        //                    emplIdsIndirect += pair.Key + ",";
        //            }

        //            if (emplIdsDirect.Length > 0)
        //                emplIdsDirect = emplIdsDirect.Remove(emplIdsDirect.LastIndexOf(','));

        //            if (emplIdsIndirect.Length > 0)
        //                emplIdsIndirect = emplIdsIndirect.Remove(emplIdsIndirect.LastIndexOf(','));
        //            List<IOPairProcessedTO> IOPairListDirect = new List<IOPairProcessedTO>();
        //            List<IOPairProcessedTO> IOPairListIndirect = new List<IOPairProcessedTO>();

        //            if (emplIdsDirect.Length > 0)
        //                IOPairListDirect = IOPairProc.SearchAllPairsForEmpl(emplIdsDirect, datesList, "");
        //            else
        //                IOPairListDirect = new List<IOPairProcessedTO>();
        //            if (dbConnection == null)
        //            {
        //                IOPairProc = new IOPairProcessed();
        //            }
        //            else
        //            {
        //                IOPairProc = new IOPairProcessed(dbConnection);
        //            }
        //            if (emplIdsIndirect.Length > 0)
        //                IOPairListIndirect = IOPairProc.SearchAllPairsForEmpl(emplIdsIndirect, datesList, "");
        //            else
        //                IOPairListIndirect = new List<IOPairProcessedTO>();
        //            if (IOPairListIndirect.Count > 0 || IOPairListDirect.Count > 0)
        //            {
        //                isEmpty = false;
        //                DataRow rowLunchBreak = dt.NewRow();
        //                rowLunchBreak[1] = "Lunch break";

        //                decimal workOnHolidaysDirect = CalcLunchBreak(emplIdsDirect, company, IOPairListDirect, datesList[0], datesList[datesList.Count - 1], dbConnection, passTypesDictionaryAll, rulesDictionary, 0);
        //                decimal workOnHolidaysIndirect = CalcLunchBreak(emplIdsIndirect, company, IOPairListIndirect, datesList[0], datesList[datesList.Count - 1], dbConnection, passTypesDictionaryAll, rulesDictionary, 1);
        //                rowLunchBreak["Direct"] = workOnHolidaysDirect;
        //                rowLunchBreak["Indirect"] = workOnHolidaysIndirect;
        //                rowLunchBreak["Total"] = workOnHolidaysIndirect + workOnHolidaysDirect;

        //                DataRow rowNotJustified = dt.NewRow();
        //                listPassTypes = passType.Find(Constants.absence.ToString(), company);


        //                if (listPassTypes.Count > 0)
        //                {

        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Unjustified absences", rowNotJustified, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else calc400Empty("Unjustified absences", rowNotJustified);

        //                DataRow rowTraining = dt.NewRow();
        //                listPassTypes = passType.FindByPaymentCode("1406", company);
        //                if (listPassTypes.Count > 0)
        //                {

        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Training", rowTraining, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else
        //                    calc400Empty("Training", rowTraining);

        //                DataRow rowRegularWork = dt.NewRow();
        //                listPassTypes = passType.FindByPaymentCode("0012", company);
        //                if (listPassTypes.Count > 0)
        //                {
        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Regular work", rowRegularWork, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else
        //                    calc400Empty("Regular work", rowRegularWork);


        //                DataRow rowBusinessTrip = dt.NewRow();
        //                listPassTypes = passType.FindByPaymentCode("0014", company);
        //                if (listPassTypes.Count > 0)
        //                {

        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Business trip", rowBusinessTrip, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else
        //                    calc400Empty("Business trip", rowBusinessTrip);


        //                DataRow rowAbsenceDuringDay = dt.NewRow();
        //                listPassTypes = passType.FindByPaymentCode("0071", company);
        //                if (listPassTypes.Count > 0)
        //                {

        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Absence during the day for business purposes", rowAbsenceDuringDay, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else
        //                    calc400Empty("Absence during the day for business purposes", rowAbsenceDuringDay);


        //                DataRow rowAbsencesOther = dt.NewRow();



        //                if (company == -2)
        //                {
        //                    listPassTypes = passType.Find("84,21,79,56,71,74,75,57,76,78,77,23,29,30,31,32,33,34,35,36,38,14,80,59,54,55,81,53,65,72,82,41,24,17,18,67,68,19,20,69,70", company);
        //                }
        //                else if (company == -3)
        //                {
        //                    listPassTypes = passType.Find("1021,1079,1078,1077,1023,1029,1031,1032,1034,1035,1038,1014,1080,1059,1055,1053,1065,1072,1082,1041,1024,1017,1018,1067,1068,1020,1070,1071,1056,1074,1075", company);
        //                }
        //                else if (company == -4)
        //                {
        //                    listPassTypes = passType.Find("2021,2079,2078,2077,2023,2029,2031,2032,2034,2035,2038,2014,2080,2059,2055,2053,2056,2065,2071,2072,2074,2075,2082,2041,2024,2017,2018,2067,2068,2020,2070", company);
        //                }
        //                else
        //                {
        //                    listPassTypes = passType.Find("3021,3079,3078,3077,3023,3029,3031,3032,3034,3035,3038,3014,3080,3059,3055,3056,3053,3065,3072,3074,3075,3082,3041,3024,3017,3018,3067,3068,3020,3070", company);
        //                }

        //                if (listPassTypes.Count > 0)
        //                {

        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Absences - Others", rowAbsencesOther, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else
        //                    calc400Empty("Absences - Others", rowAbsencesOther);



        //                DataRow rowOvertimeNotPaid = dt.NewRow();
        //                listPassTypes = passType.FindByPaymentCode("0130", company);
        //                if (listPassTypes.Count > 0)
        //                {

        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Overtime not paid", rowOvertimeNotPaid, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else
        //                    calc400Empty("Overtime not paid", rowOvertimeNotPaid);

        //                DataRow rowOvertime = dt.NewRow();
        //                if (company == -2)
        //                {
        //                    listPassTypes = passType.Find("48,-1000,4,86,88", company);
        //                }
        //                else if (company == -3)
        //                {
        //                    listPassTypes = passType.Find("1048,-1000,1004,1088", company);
        //                }
        //                else if (company == -4)
        //                {
        //                    listPassTypes = passType.Find("2048,-1000,2004,2088", company);
        //                }
        //                else
        //                {
        //                    listPassTypes = passType.Find("3048,-1000,3004,3088", company);
        //                }


        //                if (listPassTypes.Count > 0)
        //                {
        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Overtime", rowOvertime, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else
        //                    calc400Empty("Overtime", rowOvertime);

        //                DataRow rowIllness = dt.NewRow();
        //                if (company == -2)
        //                {
        //                    listPassTypes = passType.Find("26,40,25,27,62,43,63,73,39,28", company);
        //                }
        //                else if (company == -3)
        //                {
        //                    listPassTypes = passType.Find("1026,1040,1025,1027,1062,1043,1063,1073,1039,1028", company);
        //                }
        //                else if (company == -4)
        //                {
        //                    listPassTypes = passType.Find("2026,2040,2025,2027,2062,2043,2063,2073,2039,2028", company);
        //                }
        //                else
        //                {
        //                    listPassTypes = passType.Find("3026,3040,3025,3027,3062,3043,3063,3073,3039,3028", company);
        //                }

        //                if (listPassTypes.Count > 0)
        //                {

        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Illness", rowIllness, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else calc400Empty("Illness", rowIllness);


        //                DataRow rowLackOfWork = dt.NewRow();
        //                listPassTypes = passType.Find("0053", company);
        //                if (listPassTypes.Count > 0)
        //                {
        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Lack of work (65%)", rowLackOfWork, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else calc400Empty("Lack of work (65%)", rowLackOfWork);

        //                DataRow rowOvertimeNotJustified = dt.NewRow();
        //                listPassTypes = passType.FindByPaymentCode("0053", company);
        //                if (listPassTypes.Count > 0)
        //                {

        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Using overtime not justified", rowOvertimeNotJustified, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else
        //                    calc400Empty("Using overtime not justified", rowOvertimeNotJustified);


        //                DataRow rowHoliday = dt.NewRow();
        //                listPassTypes = passType.FindByPaymentCode("0040,0044", company);
        //                if (listPassTypes.Count > 0)
        //                {

        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Holiday", rowHoliday, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else
        //                    calc400Empty("Holiday", rowHoliday);


        //                DataRow rowUsedBankH = dt.NewRow();
        //                listPassTypes = passType.FindByPaymentCode("0212", company);
        //                if (listPassTypes.Count > 0)
        //                {

        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Used bank hours", rowUsedBankH, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else
        //                    calc400Empty("Used bank hours", rowUsedBankH);

        //                DataRow rowJustifiedAbsence = dt.NewRow();
        //                listPassTypes = passType.FindByPaymentCode("0060", company);
        //                if (listPassTypes.Count > 0)
        //                {

        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Justified absence", rowJustifiedAbsence, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else
        //                    calc400Empty("Justified absence", rowJustifiedAbsence);


        //                DataRow rowBloodDonations = dt.NewRow();
        //                if (company == -2)
        //                {
        //                    listPassTypes = passType.Find("11,66", company);
        //                }
        //                else if (company == -3)
        //                {
        //                    listPassTypes = passType.Find("1011,1066", company);
        //                }
        //                else if (company == -4)
        //                {
        //                    listPassTypes = passType.Find("2011,2066", company);
        //                }
        //                else
        //                {
        //                    listPassTypes = passType.Find("3011,3066", company);
        //                }
        //                if (listPassTypes.Count > 0)
        //                {

        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Blood donation", rowBloodDonations, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else
        //                    calc400Empty("Blood donation", rowBloodDonations);


        //                DataRow rowStopWorkingHours = dt.NewRow();
        //                listPassTypes = passType.FindByPaymentCode("0512", company);
        //                if (listPassTypes.Count > 0)
        //                {

        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Stop working hours current month", rowStopWorkingHours, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else
        //                    calc400Empty("Stop working hours current month", rowStopWorkingHours);


        //                DataRow rowProductiveRecovery = dt.NewRow();
        //                listPassTypes = passType.FindByPaymentCode("0612", company);
        //                if (listPassTypes.Count > 0)
        //                {

        //                    calc400Lunch(IOPairListDirect, IOPairListIndirect, listPassTypes, "Productive recovery", rowProductiveRecovery, emplIdsDirect, emplIdsIndirect, datesList);
        //                }
        //                else
        //                    calc400Empty("Productive recovery", rowProductiveRecovery);


        //                double totalDD = double.Parse(rowRegularWork[2].ToString()) + double.Parse(rowBusinessTrip[2].ToString()) + double.Parse(rowAbsenceDuringDay[2].ToString())
        //                    + double.Parse(rowLunchBreak[2].ToString()) + double.Parse(rowAbsencesOther[2].ToString()) + double.Parse(rowProductiveRecovery[2].ToString())
        //                    + double.Parse(rowTraining[2].ToString()) + double.Parse(rowStopWorkingHours[2].ToString()) + double.Parse(rowBloodDonations[2].ToString()) + double.Parse(rowJustifiedAbsence[2].ToString())
        //                    + double.Parse(rowUsedBankH[2].ToString()) + double.Parse(rowHoliday[2].ToString()) + double.Parse(rowOvertimeNotJustified[2].ToString()) + double.Parse(rowLackOfWork[2].ToString()) + double.Parse(rowIllness[2].ToString())
        //                    + double.Parse(rowOvertimeNotPaid[2].ToString()) + double.Parse(rowOvertime[2].ToString()) + double.Parse(rowNotJustified[2].ToString());

        //                double totalID = double.Parse(rowRegularWork[4].ToString()) + double.Parse(rowBusinessTrip[4].ToString()) + double.Parse(rowAbsenceDuringDay[4].ToString()) + double.Parse(rowLunchBreak[4].ToString()) + double.Parse(rowAbsencesOther[4].ToString()) + double.Parse(rowProductiveRecovery[4].ToString()) + double.Parse(rowTraining[4].ToString()) + double.Parse(rowStopWorkingHours[4].ToString()) + double.Parse(rowBloodDonations[4].ToString()) + double.Parse(rowJustifiedAbsence[4].ToString())
        //                       + double.Parse(rowUsedBankH[4].ToString()) + double.Parse(rowHoliday[4].ToString()) + double.Parse(rowOvertimeNotJustified[4].ToString()) + double.Parse(rowLackOfWork[4].ToString()) + double.Parse(rowIllness[4].ToString())
        //                       + double.Parse(rowOvertimeNotPaid[4].ToString()) + double.Parse(rowOvertime[4].ToString()) + double.Parse(rowNotJustified[4].ToString());

        //                double total = totalDD + totalID;
        //                DataRow row = dt.NewRow();
        //                row[1] = "Total";
        //                row[2] = totalDD;
        //                row[4] = totalID;
        //                row[6] = total;
        //                row[5] = 0;
        //                row[3] = 0;
        //                DataRow rowTotal = dt.NewRow();
        //                rowTotal[1] = "Planned";
        //                if (emplIdsDirect.Length > 0)
        //                    rowTotal[2] = Misc.CalcPlannedTime(dbConnection, emplIdsDirect, datesList[0], datesList[datesList.Count - 1]);
        //                else
        //                    rowTotal[2] = 0;
        //                if (emplIdsIndirect.Length > 0)
        //                    rowTotal[4] = Misc.CalcPlannedTime(dbConnection, emplIdsIndirect, datesList[0], datesList[datesList.Count - 1]);
        //                else
        //                    rowTotal[4] = 0;

        //                rowTotal[6] = double.Parse(rowTotal[2].ToString()) + double.Parse(rowTotal[4].ToString());
        //                rowTotal[5] = 0;
        //                rowTotal[3] = 0;

        //                DataRow rowTotalDiff = dt.NewRow();
        //                rowTotalDiff[1] = "+/-";
        //                rowTotalDiff[3] = "  ";
        //                rowTotalDiff[5] = "  ";
        //                rowTotalDiff[7] = "  ";
        //                rowTotalDiff[2] = double.Parse(row[2].ToString()) - double.Parse(rowTotal[2].ToString());
        //                rowTotalDiff[4] = double.Parse(row[4].ToString()) - double.Parse(rowTotal[4].ToString());
        //                rowTotalDiff[6] = double.Parse(row[6].ToString()) - double.Parse(rowTotal[6].ToString());

        //                Misc.percentage(rowRegularWork, totalDD, totalID);
        //                Misc.percentage(rowBusinessTrip, totalDD, totalID);
        //                Misc.percentage(rowAbsenceDuringDay, totalDD, totalID);
        //                Misc.percentage(rowLunchBreak, totalDD, totalID);
        //                Misc.percentage(rowAbsencesOther, totalDD, totalID);
        //                Misc.percentage(rowTraining, totalDD, totalID);
        //                Misc.percentage(rowProductiveRecovery, totalDD, totalID);
        //                Misc.percentage(rowStopWorkingHours, totalDD, totalID);
        //                Misc.percentage(rowBloodDonations, totalDD, totalID);
        //                Misc.percentage(rowJustifiedAbsence, totalDD, totalID);
        //                Misc.percentage(rowUsedBankH, totalDD, totalID);
        //                Misc.percentage(rowHoliday, totalDD, totalID);
        //                Misc.percentage(rowOvertimeNotJustified, totalDD, totalID);
        //                Misc.percentage(rowLackOfWork, totalDD, totalID);
        //                Misc.percentage(rowIllness, totalDD, totalID);
        //                Misc.percentage(rowOvertime, totalDD, totalID);
        //                Misc.percentage(rowOvertimeNotPaid, totalDD, totalID);
        //                Misc.percentage(rowNotJustified, totalDD, totalID);


        //                rowRegularWork[7] = double.Parse(rowRegularWork[6].ToString()) * 100 / total;
        //                rowBusinessTrip[7] = double.Parse(rowBusinessTrip[6].ToString()) * 100 / total;
        //                rowAbsenceDuringDay[7] = double.Parse(rowAbsenceDuringDay[6].ToString()) * 100 / total;
        //                rowLunchBreak[7] = double.Parse(rowLunchBreak[6].ToString()) * 100 / total;
        //                rowAbsencesOther[7] = double.Parse(rowAbsencesOther[6].ToString()) * 100 / total;
        //                rowTraining[7] = double.Parse(rowTraining[6].ToString()) * 100 / total;
        //                rowProductiveRecovery[7] = double.Parse(rowProductiveRecovery[6].ToString()) * 100 / total;
        //                rowStopWorkingHours[7] = double.Parse(rowStopWorkingHours[6].ToString()) * 100 / total;
        //                rowBloodDonations[7] = double.Parse(rowBloodDonations[6].ToString()) * 100 / total;
        //                rowJustifiedAbsence[7] = double.Parse(rowJustifiedAbsence[6].ToString()) * 100 / total;
        //                rowUsedBankH[7] = double.Parse(rowUsedBankH[6].ToString()) * 100 / total;
        //                rowHoliday[7] = double.Parse(rowHoliday[6].ToString()) * 100 / total;
        //                rowOvertimeNotJustified[7] = double.Parse(rowOvertimeNotJustified[6].ToString()) * 100 / total;
        //                rowLackOfWork[7] = double.Parse(rowLackOfWork[6].ToString()) * 100 / total;
        //                rowIllness[7] = double.Parse(rowIllness[6].ToString()) * 100 / total;
        //                rowOvertime[7] = double.Parse(rowOvertime[6].ToString()) * 100 / total;
        //                rowOvertimeNotPaid[7] = double.Parse(rowOvertimeNotPaid[6].ToString()) * 100 / total;
        //                rowNotJustified[7] = double.Parse(rowNotJustified[6].ToString()) * 100 / total;
        //                row[7] = double.Parse(row[6].ToString()) * 100 / total;


        //                if (rowRegularWork[7].ToString() == "NaN") rowRegularWork[7] = 0;
        //                if (rowBusinessTrip[7].ToString() == "NaN") rowBusinessTrip[7] = 0;
        //                if (rowAbsenceDuringDay[7].ToString() == "NaN") rowAbsenceDuringDay[7] = 0;
        //                if (rowLunchBreak[7].ToString() == "NaN") rowLunchBreak[7] = 0;
        //                if (rowAbsencesOther[7].ToString() == "NaN") rowAbsencesOther[7] = 0;
        //                if (rowTraining[7].ToString() == "NaN") rowTraining[7] = 0;
        //                if (rowProductiveRecovery[7].ToString() == "NaN") rowProductiveRecovery[7] = 0;
        //                if (rowStopWorkingHours[7].ToString() == "NaN") rowStopWorkingHours[7] = 0;
        //                if (rowBloodDonations[7].ToString() == "NaN") rowBloodDonations[7] = 0;
        //                if (rowJustifiedAbsence[7].ToString() == "NaN") rowJustifiedAbsence[7] = 0;
        //                if (rowUsedBankH[7].ToString() == "NaN") rowUsedBankH[7] = 0;
        //                if (rowHoliday[7].ToString() == "NaN") rowHoliday[7] = 0;
        //                if (rowOvertimeNotJustified[7].ToString() == "NaN") rowOvertimeNotJustified[7] = 0;
        //                if (rowLackOfWork[7].ToString() == "NaN") rowLackOfWork[7] = 0;
        //                if (rowIllness[7].ToString() == "NaN") rowIllness[7] = 0;
        //                if (rowOvertime[7].ToString() == "NaN") rowOvertime[7] = 0;
        //                if (rowOvertimeNotPaid[7].ToString() == "NaN") rowOvertimeNotPaid[7] = 0;
        //                if (rowNotJustified[7].ToString() == "NaN") rowNotJustified[7] = 0;
        //                if (row[7].ToString() == "NaN") row[7] = 0;

        //                double totalDPercent = double.Parse(rowRegularWork[3].ToString()) + double.Parse(rowBusinessTrip[3].ToString()) + double.Parse(rowAbsenceDuringDay[3].ToString()) + double.Parse(rowLunchBreak[3].ToString()) + double.Parse(rowAbsencesOther[3].ToString()) + double.Parse(rowProductiveRecovery[3].ToString()) + double.Parse(rowTraining[3].ToString()) + double.Parse(rowStopWorkingHours[3].ToString()) + double.Parse(rowBloodDonations[3].ToString()) + double.Parse(rowJustifiedAbsence[3].ToString())
        //                   + double.Parse(rowUsedBankH[3].ToString()) + double.Parse(rowHoliday[3].ToString()) + double.Parse(rowOvertimeNotJustified[3].ToString()) + double.Parse(rowLackOfWork[3].ToString()) + double.Parse(rowIllness[3].ToString())
        //                   + double.Parse(rowOvertimeNotPaid[3].ToString()) + double.Parse(rowOvertime[3].ToString()) + double.Parse(rowNotJustified[3].ToString());

        //                double totalIPercent = double.Parse(rowRegularWork[5].ToString()) + double.Parse(rowBusinessTrip[5].ToString()) + double.Parse(rowAbsenceDuringDay[5].ToString()) + double.Parse(rowLunchBreak[5].ToString()) + double.Parse(rowAbsencesOther[5].ToString()) + double.Parse(rowProductiveRecovery[5].ToString()) + double.Parse(rowTraining[5].ToString()) + double.Parse(rowStopWorkingHours[5].ToString()) + double.Parse(rowBloodDonations[5].ToString()) + double.Parse(rowJustifiedAbsence[5].ToString())
        //                      + double.Parse(rowUsedBankH[5].ToString()) + double.Parse(rowHoliday[5].ToString()) + double.Parse(rowOvertimeNotJustified[5].ToString()) + double.Parse(rowLackOfWork[5].ToString()) + double.Parse(rowIllness[5].ToString())
        //                      + double.Parse(rowOvertimeNotPaid[5].ToString()) + double.Parse(rowOvertime[5].ToString()) + double.Parse(rowNotJustified[5].ToString());


        //                row[3] = totalDPercent;
        //                row[5] = totalIPercent;

        //                double ts = double.Parse(rowTotal[2].ToString());
        //                double tsi = double.Parse(rowTotal[4].ToString());
        //                if (ts != 0) rowTotal[3] = 0;
        //                if (tsi != 0) rowTotal[5] = 0;

        //                rowTotal[7] = 0;
        //                Misc.roundOn2(rowRegularWork);
        //                Misc.roundOn2(rowBusinessTrip);
        //                Misc.roundOn2(rowAbsenceDuringDay);
        //                Misc.roundOn2(rowLunchBreak);
        //                Misc.roundOn2(rowAbsencesOther);
        //                Misc.roundOn2(rowTraining);
        //                Misc.roundOn2(rowProductiveRecovery);
        //                Misc.roundOn2(rowStopWorkingHours);
        //                Misc.roundOn2(rowBloodDonations);
        //                Misc.roundOn2(rowJustifiedAbsence);
        //                Misc.roundOn2(rowUsedBankH);
        //                Misc.roundOn2(rowHoliday);
        //                Misc.roundOn2(rowOvertimeNotJustified);
        //                Misc.roundOn2(rowLackOfWork);
        //                Misc.roundOn2(rowIllness);
        //                Misc.roundOn2(rowOvertime);
        //                Misc.roundOn2(rowOvertimeNotPaid);
        //                Misc.roundOn2(row);
        //                Misc.roundOn2(rowTotal);
        //                Misc.roundOn2(rowTotalDiff);
        //                Misc.roundOn2(rowNotJustified);

        //                dt.Rows.Add(rowRegularWork);
        //                dt.Rows.Add(rowBusinessTrip);
        //                dt.Rows.Add(rowAbsenceDuringDay);
        //                dt.Rows.Add(rowTraining);
        //                dt.Rows.Add(rowLunchBreak);
        //                dt.Rows.Add(rowAbsencesOther);
        //                dt.Rows.Add(rowNotJustified);
        //                dt.Rows.Add(rowProductiveRecovery);
        //                dt.Rows.Add(rowStopWorkingHours);
        //                dt.Rows.Add(rowBloodDonations);
        //                dt.Rows.Add(rowJustifiedAbsence);
        //                dt.Rows.Add(rowUsedBankH);
        //                dt.Rows.Add(rowHoliday);
        //                dt.Rows.Add(rowOvertimeNotJustified);
        //                dt.Rows.Add(rowLackOfWork);
        //                dt.Rows.Add(rowIllness);
        //                dt.Rows.Add(rowOvertime);
        //                dt.Rows.Add(rowOvertimeNotPaid);
        //                DataRow rowEmptyBottom = dt.NewRow();
        //                dt.Rows.Add(rowEmptyBottom);
        //                dt.Rows.Add(row);
        //                dt.Rows.Add(rowTotal);
        //                dt.Rows.Add(rowTotalDiff);
        //                dt.AcceptChanges();
        //            }
        //            else
        //                isEmpty = true;
        //        }
        //        return isEmpty;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //        //debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + ".populateTable500() " + ex.Message + " for wu: " + workingUnit.WorkingUnitID);
        //        //return true;
        //    }
        //}

        private bool populateDataTableNew(Object dbConnection, DataTable dt, WorkingUnitTO workingUnit, List<DateTime> datesList, int company, string title, int level, uint py_calc_id)
        {
            bool isEmpty = true;
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

                DataRow rowHeader1 = dt.NewRow();
                if (dbConnection == null)
                    rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
                else
                    rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
                rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                //rowHeader1[8] = "page: " + page;
                dt.Rows.Add(rowHeader1);

                DataRow rowHeader2 = dt.NewRow();
                rowHeader2[1] = title;
                rowHeader2[6] = datesList[0].ToString("dd.MM") + "-" + datesList[datesList.Count - 1].ToString("dd.MM");
                //rowHeader2[7] = datesList[0].ToString("MMMM yyyy");

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
                Employee Empl;

                if (dbConnection == null)
                {
                    Empl = new Employee();
                }
                else
                {
                    Empl = new Employee(dbConnection);
                }
                List<EmployeeTO> listEmpl = new List<EmployeeTO>();

                listEmpl = Empl.SearchByWULoans(workingUnit.WorkingUnitID.ToString(), -1, null, datesList[0], datesList[datesList.Count - 1]);


                if (listEmpl.Count > 0)
                {
                    string emplList = "";
                    foreach (EmployeeTO empl in listEmpl)
                    {

                        emplList += empl.EmployeeID + ",";
                    }
                    if (emplList.Length > 0)
                        emplList = emplList.Remove(emplList.LastIndexOf(','));


                    Dictionary<string, string> dictEmplBranch = Misc.emplBranch(dbConnection, emplList, company);
                    Dictionary<int, PassTypeTO> listPassTypes = new Dictionary<int, PassTypeTO>();
                    string emplIdsDirect = "";
                    string emplIdsIndirect = "";
                    foreach (KeyValuePair<string, string> pair in dictEmplBranch)
                    {
                        if (pair.Value == "A")
                            emplIdsDirect += pair.Key + ",";
                        else
                            emplIdsIndirect += pair.Key + ",";
                    }

                    if (emplIdsDirect.Length > 0)
                        emplIdsDirect = emplIdsDirect.Remove(emplIdsDirect.LastIndexOf(','));

                    if (emplIdsIndirect.Length > 0)
                        emplIdsIndirect = emplIdsIndirect.Remove(emplIdsIndirect.LastIndexOf(','));

                    List<EmployeePYDataAnaliticalTO> listIndirect = new EmployeePYDataAnalitical().Search(emplIdsIndirect, "", py_calc_id, "E");
                    List<EmployeePYDataAnaliticalTO> listDirect = new EmployeePYDataAnalitical().Search(emplIdsDirect, "", py_calc_id, "E");
                    if (listDirect.Count > 0 || listIndirect.Count > 0)
                    {
                        isEmpty = false;
                        string payment_code = "";
                        DataRow rowLunchBreak = dt.NewRow();
                        calc400New("Lunch break", rowLunchBreak, emplIdsDirect, emplIdsIndirect, "'0041'", py_calc_id);

                        DataRow rowNotJustified = dt.NewRow();
                        calc400New("Unjustified absences", rowNotJustified, emplIdsDirect, emplIdsIndirect, "'0000'", py_calc_id);

                        DataRow rowNotJustifiedClosure = dt.NewRow();
                        calc400New("Unjustified absences - closure", rowNotJustifiedClosure, emplIdsDirect, emplIdsIndirect, "'0000" + Constants.FiatClosurePaymentCode + "'", py_calc_id);

                        DataRow rowNotJustifiedLayoff = dt.NewRow();
                        calc400New("Unjustified absences - layoff", rowNotJustifiedLayoff, emplIdsDirect, emplIdsIndirect, "'0000" + Constants.FiatLayOffPaymentCode + "'", py_calc_id);

                        DataRow rowNotJustifiedStoppage = dt.NewRow();
                        calc400New("Unjustified absences - stoppage", rowNotJustifiedStoppage, emplIdsDirect, emplIdsIndirect, "'0000" + Constants.FiatStoppagePaymentCode + "'", py_calc_id);

                        DataRow rowNotJustifiedHoliday = dt.NewRow();
                        calc400New("Unjustified absences - public holiday", rowNotJustifiedHoliday, emplIdsDirect, emplIdsIndirect, "'0000" + Constants.FiatPublicHollidayPaymentCode + "'", py_calc_id);

                        DataRow rowTraining = dt.NewRow();
                        calc400New("Training", rowTraining, emplIdsDirect, emplIdsIndirect, "'1406'", py_calc_id);

                        DataRow rowRegularWork = dt.NewRow();
                        calc400New("Regular work", rowRegularWork, emplIdsDirect, emplIdsIndirect, "'0012'", py_calc_id);

                        DataRow rowBusinessTrip = dt.NewRow();
                        calc400New("Business trip", rowBusinessTrip, emplIdsDirect, emplIdsIndirect, "'0014'", py_calc_id);

                        DataRow rowAbsenceDuringDay = dt.NewRow();
                        calc400New("Absence during the day for business purposes", rowAbsenceDuringDay, emplIdsDirect, emplIdsIndirect, "'0071'", py_calc_id);

                        DataRow rowAbsencesOther = dt.NewRow();
                        payment_code = "'0045','0046','0047','0048','0056','0070','0169','0171','0269','0369','1145','1148','1155','1407','0055','0075','0112','0144','0145','0146','0147','0148','0149','0155','1150','0049','1149','1157','1144','1045'";                        
                        calc400New("Absences - Others", rowAbsencesOther, emplIdsDirect, emplIdsIndirect, payment_code, py_calc_id);

                        DataRow rowAbsencesOtherClosure = dt.NewRow();
                        payment_code = "'0045" + Constants.FiatClosurePaymentCode + "','0046" + Constants.FiatClosurePaymentCode + "','0047" + Constants.FiatClosurePaymentCode
                            + "','0048" + Constants.FiatClosurePaymentCode + "','0056" + Constants.FiatClosurePaymentCode + "','0070" + Constants.FiatClosurePaymentCode + "','0169" + Constants.FiatClosurePaymentCode
                            + "','0171" + Constants.FiatClosurePaymentCode + "','0269" + Constants.FiatClosurePaymentCode + "','0369" + Constants.FiatClosurePaymentCode + "','1145" + Constants.FiatClosurePaymentCode
                            + "','1148" + Constants.FiatClosurePaymentCode + "','1155" + Constants.FiatClosurePaymentCode + "','1407" + Constants.FiatClosurePaymentCode + "','0055" + Constants.FiatClosurePaymentCode
                            + "','0075" + Constants.FiatClosurePaymentCode + "','0112" + Constants.FiatClosurePaymentCode + "','0144" + Constants.FiatClosurePaymentCode + "','0145" + Constants.FiatClosurePaymentCode
                            + "','0146" + Constants.FiatClosurePaymentCode + "','0147" + Constants.FiatClosurePaymentCode + "','0148" + Constants.FiatClosurePaymentCode + "','0149" + Constants.FiatClosurePaymentCode
                            + "','0155" + Constants.FiatClosurePaymentCode + "','1150" + Constants.FiatClosurePaymentCode + "','0049" + Constants.FiatClosurePaymentCode + "','1149" + Constants.FiatClosurePaymentCode
                            + "','1157" + Constants.FiatClosurePaymentCode + "','1144" + Constants.FiatClosurePaymentCode + "','1045" + Constants.FiatClosurePaymentCode + "'";
                        calc400New("Absences - Others - closure", rowAbsencesOtherClosure, emplIdsDirect, emplIdsIndirect, payment_code, py_calc_id);

                        DataRow rowAbsencesOtherLayoff = dt.NewRow();
                        payment_code = "'0045" + Constants.FiatLayOffPaymentCode + "','0046" + Constants.FiatLayOffPaymentCode + "','0047" + Constants.FiatLayOffPaymentCode
                            + "','0048" + Constants.FiatLayOffPaymentCode + "','0056" + Constants.FiatLayOffPaymentCode + "','0070" + Constants.FiatLayOffPaymentCode + "','0169" + Constants.FiatLayOffPaymentCode
                            + "','0171" + Constants.FiatLayOffPaymentCode + "','0269" + Constants.FiatLayOffPaymentCode + "','0369" + Constants.FiatLayOffPaymentCode + "','1145" + Constants.FiatLayOffPaymentCode
                            + "','1148" + Constants.FiatLayOffPaymentCode + "','1155" + Constants.FiatLayOffPaymentCode + "','1407" + Constants.FiatLayOffPaymentCode + "','0055" + Constants.FiatLayOffPaymentCode
                            + "','0075" + Constants.FiatLayOffPaymentCode + "','0112" + Constants.FiatLayOffPaymentCode + "','0144" + Constants.FiatLayOffPaymentCode + "','0145" + Constants.FiatLayOffPaymentCode
                            + "','0146" + Constants.FiatLayOffPaymentCode + "','0147" + Constants.FiatLayOffPaymentCode + "','0148" + Constants.FiatLayOffPaymentCode + "','0149" + Constants.FiatLayOffPaymentCode
                            + "','0155" + Constants.FiatLayOffPaymentCode + "','1150" + Constants.FiatLayOffPaymentCode + "','0049" + Constants.FiatLayOffPaymentCode + "','1149" + Constants.FiatLayOffPaymentCode
                            + "','1157" + Constants.FiatLayOffPaymentCode + "','1144" + Constants.FiatLayOffPaymentCode + "','1045" + Constants.FiatLayOffPaymentCode + "'";
                        calc400New("Absences - Others - layoff", rowAbsencesOtherLayoff, emplIdsDirect, emplIdsIndirect, payment_code, py_calc_id);

                        DataRow rowAbsencesOtherStoppage = dt.NewRow();
                        payment_code = "'0045" + Constants.FiatStoppagePaymentCode + "','0046" + Constants.FiatStoppagePaymentCode + "','0047" + Constants.FiatStoppagePaymentCode
                            + "','0048" + Constants.FiatStoppagePaymentCode + "','0056" + Constants.FiatStoppagePaymentCode + "','0070" + Constants.FiatStoppagePaymentCode + "','0169" + Constants.FiatStoppagePaymentCode
                            + "','0171" + Constants.FiatStoppagePaymentCode + "','0269" + Constants.FiatStoppagePaymentCode + "','0369" + Constants.FiatStoppagePaymentCode + "','1145" + Constants.FiatStoppagePaymentCode
                            + "','1148" + Constants.FiatStoppagePaymentCode + "','1155" + Constants.FiatStoppagePaymentCode + "','1407" + Constants.FiatStoppagePaymentCode + "','0055" + Constants.FiatStoppagePaymentCode
                            + "','0075" + Constants.FiatStoppagePaymentCode + "','0112" + Constants.FiatStoppagePaymentCode + "','0144" + Constants.FiatStoppagePaymentCode + "','0145" + Constants.FiatStoppagePaymentCode
                            + "','0146" + Constants.FiatStoppagePaymentCode + "','0147" + Constants.FiatStoppagePaymentCode + "','0148" + Constants.FiatStoppagePaymentCode + "','0149" + Constants.FiatStoppagePaymentCode
                            + "','0155" + Constants.FiatStoppagePaymentCode + "','1150" + Constants.FiatStoppagePaymentCode + "','0049" + Constants.FiatStoppagePaymentCode + "','1149" + Constants.FiatStoppagePaymentCode
                            + "','1157" + Constants.FiatStoppagePaymentCode + "','1144" + Constants.FiatStoppagePaymentCode + "','1045" + Constants.FiatStoppagePaymentCode + "'";
                        calc400New("Absences - Others - stoppage", rowAbsencesOtherStoppage, emplIdsDirect, emplIdsIndirect, payment_code, py_calc_id);

                        DataRow rowAbsencesOtherHoliday = dt.NewRow();
                        payment_code = "'0045" + Constants.FiatPublicHollidayPaymentCode + "','0046" + Constants.FiatPublicHollidayPaymentCode + "','0047" + Constants.FiatPublicHollidayPaymentCode
                            + "','0048" + Constants.FiatPublicHollidayPaymentCode + "','0056" + Constants.FiatPublicHollidayPaymentCode + "','0070" + Constants.FiatPublicHollidayPaymentCode + "','0169" + Constants.FiatPublicHollidayPaymentCode
                            + "','0171" + Constants.FiatPublicHollidayPaymentCode + "','0269" + Constants.FiatPublicHollidayPaymentCode + "','0369" + Constants.FiatPublicHollidayPaymentCode + "','1145" + Constants.FiatPublicHollidayPaymentCode
                            + "','1148" + Constants.FiatPublicHollidayPaymentCode + "','1155" + Constants.FiatPublicHollidayPaymentCode + "','1407" + Constants.FiatPublicHollidayPaymentCode + "','0055" + Constants.FiatPublicHollidayPaymentCode
                            + "','0075" + Constants.FiatPublicHollidayPaymentCode + "','0112" + Constants.FiatPublicHollidayPaymentCode + "','0144" + Constants.FiatPublicHollidayPaymentCode + "','0145" + Constants.FiatPublicHollidayPaymentCode
                            + "','0146" + Constants.FiatPublicHollidayPaymentCode + "','0147" + Constants.FiatPublicHollidayPaymentCode + "','0148" + Constants.FiatPublicHollidayPaymentCode + "','0149" + Constants.FiatPublicHollidayPaymentCode
                            + "','0155" + Constants.FiatPublicHollidayPaymentCode + "','1150" + Constants.FiatPublicHollidayPaymentCode + "','0049" + Constants.FiatPublicHollidayPaymentCode + "','1149" + Constants.FiatPublicHollidayPaymentCode
                            + "','1157" + Constants.FiatPublicHollidayPaymentCode + "','1144" + Constants.FiatPublicHollidayPaymentCode + "','1045" + Constants.FiatPublicHollidayPaymentCode + "'";
                        calc400New("Absences - Others - public holiday", rowAbsencesOtherHoliday, emplIdsDirect, emplIdsIndirect, payment_code, py_calc_id);
                        
                        DataRow rowOvertimeNotPaid = dt.NewRow();
                        calc400New("Overtime not paid", rowOvertimeNotPaid, emplIdsDirect, emplIdsIndirect, "'0130'", py_calc_id);

                        DataRow rowOvertime = dt.NewRow();
                        calc400New("Overtime", rowOvertime, emplIdsDirect, emplIdsIndirect, "'0030'", py_calc_id);

                        DataRow rowIllness = dt.NewRow();
                        payment_code = "'0058','0156','0057','0060','0160','0257','1257','1160','0157','0061'";
                        calc400New("Illness", rowIllness, emplIdsDirect, emplIdsIndirect, payment_code, py_calc_id);

                        DataRow rowIllnessClosure = dt.NewRow();
                        payment_code = "'0058" + Constants.FiatClosurePaymentCode + "','0156" + Constants.FiatClosurePaymentCode + "','0057" + Constants.FiatClosurePaymentCode
                            + "','0060" + Constants.FiatClosurePaymentCode + "','0160" + Constants.FiatClosurePaymentCode + "','0257" + Constants.FiatClosurePaymentCode
                            + "','1257" + Constants.FiatClosurePaymentCode + "','1160" + Constants.FiatClosurePaymentCode + "','0157" + Constants.FiatClosurePaymentCode
                            + "','0061" + Constants.FiatClosurePaymentCode + "'";
                        calc400New("Illness - closure", rowIllnessClosure, emplIdsDirect, emplIdsIndirect, payment_code, py_calc_id);

                        DataRow rowIllnessLayoff = dt.NewRow();
                        payment_code = "'0058" + Constants.FiatLayOffPaymentCode + "','0156" + Constants.FiatLayOffPaymentCode + "','0057" + Constants.FiatLayOffPaymentCode
                            + "','0060" + Constants.FiatLayOffPaymentCode + "','0160" + Constants.FiatLayOffPaymentCode + "','0257" + Constants.FiatLayOffPaymentCode
                            + "','1257" + Constants.FiatLayOffPaymentCode + "','1160" + Constants.FiatLayOffPaymentCode + "','0157" + Constants.FiatLayOffPaymentCode
                            + "','0061" + Constants.FiatLayOffPaymentCode + "'";
                        calc400New("Illness - layoff", rowIllnessLayoff, emplIdsDirect, emplIdsIndirect, payment_code, py_calc_id);

                        DataRow rowIllnessStoppage = dt.NewRow();
                        payment_code = "'0058" + Constants.FiatStoppagePaymentCode + "','0156" + Constants.FiatStoppagePaymentCode + "','0057" + Constants.FiatStoppagePaymentCode
                            + "','0060" + Constants.FiatStoppagePaymentCode + "','0160" + Constants.FiatStoppagePaymentCode + "','0257" + Constants.FiatStoppagePaymentCode
                            + "','1257" + Constants.FiatStoppagePaymentCode + "','1160" + Constants.FiatStoppagePaymentCode + "','0157" + Constants.FiatStoppagePaymentCode
                            + "','0061" + Constants.FiatStoppagePaymentCode + "'";
                        calc400New("Illness - stoppage", rowIllnessStoppage, emplIdsDirect, emplIdsIndirect, payment_code, py_calc_id);

                        DataRow rowIllnessHoliday = dt.NewRow();
                        payment_code = "'0058" + Constants.FiatPublicHollidayPaymentCode + "','0156" + Constants.FiatPublicHollidayPaymentCode + "','0057" + Constants.FiatPublicHollidayPaymentCode
                            + "','0060" + Constants.FiatPublicHollidayPaymentCode + "','0160" + Constants.FiatPublicHollidayPaymentCode + "','0257" + Constants.FiatPublicHollidayPaymentCode
                            + "','1257" + Constants.FiatPublicHollidayPaymentCode + "','1160" + Constants.FiatPublicHollidayPaymentCode + "','0157" + Constants.FiatPublicHollidayPaymentCode
                            + "','0061" + Constants.FiatPublicHollidayPaymentCode + "'";
                        calc400New("Illness - public holiday", rowIllnessHoliday, emplIdsDirect, emplIdsIndirect, payment_code, py_calc_id);

                        DataRow rowLackOfWork = dt.NewRow();
                        calc400New("Lack of work (65%)", rowLackOfWork, emplIdsDirect, emplIdsIndirect, "'0053'", py_calc_id);

                        DataRow rowOvertimeOthers = dt.NewRow();
                        calc400New("Overtime - Others", rowOvertimeOthers, emplIdsDirect, emplIdsIndirect, "'0412','1231','8888','9999'", py_calc_id);

                        DataRow rowHoliday = dt.NewRow();
                        calc400New("Holiday", rowHoliday, emplIdsDirect, emplIdsIndirect, "'0040'", py_calc_id);

                        DataRow rowCollectiveHoliday = dt.NewRow();
                        calc400New("Collective holiday", rowCollectiveHoliday, emplIdsDirect, emplIdsIndirect, "'" + Constants.FiatCollectiveAnnualLeavePaymentCode + "'", py_calc_id);

                        DataRow rowReligiousHoliday = dt.NewRow();
                        calc400New("Religious holiday", rowReligiousHoliday, emplIdsDirect, emplIdsIndirect, "'0044'", py_calc_id);

                        DataRow rowNationalHoliday = dt.NewRow();
                        calc400New("National holiday", rowNationalHoliday, emplIdsDirect, emplIdsIndirect, "'0042'", py_calc_id);

                        DataRow rowUsedBankH = dt.NewRow();
                        calc400New("Used bank hours", rowUsedBankH, emplIdsDirect, emplIdsIndirect, "'0212'", py_calc_id);

                        DataRow rowUsedBankH1 = dt.NewRow();
                        calc400New("Used bank hours -1", rowUsedBankH1, emplIdsDirect, emplIdsIndirect, "'2212'", py_calc_id);

                        DataRow rowJustifiedAbsence = dt.NewRow();
                        calc400New("Justified absence", rowJustifiedAbsence, emplIdsDirect, emplIdsIndirect, "'0069'", py_calc_id);

                        DataRow rowJustifiedAbsenceClosure = dt.NewRow();
                        calc400New("Justified absence - closure", rowJustifiedAbsenceClosure, emplIdsDirect, emplIdsIndirect, "'0069" + Constants.FiatClosurePaymentCode + "'", py_calc_id);

                        DataRow rowJustifiedAbsenceLayoff = dt.NewRow();
                        calc400New("Justified absence - layoff", rowJustifiedAbsenceLayoff, emplIdsDirect, emplIdsIndirect, "'0069" + Constants.FiatLayOffPaymentCode + "'", py_calc_id);

                        DataRow rowJustifiedAbsenceStoppage = dt.NewRow();
                        calc400New("Justified absence - stoppage", rowJustifiedAbsenceStoppage, emplIdsDirect, emplIdsIndirect, "'0069" + Constants.FiatStoppagePaymentCode + "'", py_calc_id);

                        DataRow rowJustifiedAbsenceHoliday = dt.NewRow();
                        calc400New("Justified absence - public holiday", rowJustifiedAbsenceHoliday, emplIdsDirect, emplIdsIndirect, "'0069" + Constants.FiatPublicHollidayPaymentCode + "'", py_calc_id);

                        DataRow rowBloodDonations = dt.NewRow();
                        calc400New("Blood donation", rowBloodDonations, emplIdsDirect, emplIdsIndirect, "'0043'", py_calc_id);

                        DataRow rowBloodDonationsClosure = dt.NewRow();
                        calc400New("Blood donation - closure", rowBloodDonationsClosure, emplIdsDirect, emplIdsIndirect, "'0043" + Constants.FiatClosurePaymentCode + "'", py_calc_id);

                        DataRow rowBloodDonationsLayoff = dt.NewRow();
                        calc400New("Blood donation - layoff", rowBloodDonationsLayoff, emplIdsDirect, emplIdsIndirect, "'0043" + Constants.FiatLayOffPaymentCode + "'", py_calc_id);

                        DataRow rowBloodDonationsStoppage = dt.NewRow();
                        calc400New("Blood donation - stoppage", rowBloodDonationsStoppage, emplIdsDirect, emplIdsIndirect, "'0043" + Constants.FiatStoppagePaymentCode + "'", py_calc_id);

                        DataRow rowBloodDonationsHoliday = dt.NewRow();
                        calc400New("Blood donation - public holiday", rowBloodDonationsHoliday, emplIdsDirect, emplIdsIndirect, "'0043" + Constants.FiatPublicHollidayPaymentCode + "'", py_calc_id);

                        DataRow rowTissueDonations = dt.NewRow();
                        calc400New("Tissue and organs donation", rowTissueDonations, emplIdsDirect, emplIdsIndirect, "'0357'", py_calc_id);

                        DataRow rowTissueDonationsClosure = dt.NewRow();
                        calc400New("Tissue and organs donation - closure", rowTissueDonationsClosure, emplIdsDirect, emplIdsIndirect, "'0357" + Constants.FiatClosurePaymentCode + "'", py_calc_id);

                        DataRow rowTissueDonationsLayoff = dt.NewRow();
                        calc400New("Tissue and organs donation - layoff", rowTissueDonationsLayoff, emplIdsDirect, emplIdsIndirect, "'0357" + Constants.FiatLayOffPaymentCode + "'", py_calc_id);

                        DataRow rowTissueDonationsStoppage = dt.NewRow();
                        calc400New("Tissue and organs donation - stoppage", rowTissueDonationsStoppage, emplIdsDirect, emplIdsIndirect, "'0357" + Constants.FiatStoppagePaymentCode + "'", py_calc_id);

                        DataRow rowTissueDonationsHoliday = dt.NewRow();
                        calc400New("Tissue and organs donation - public holiday", rowTissueDonationsHoliday, emplIdsDirect, emplIdsIndirect, "'0357" + Constants.FiatPublicHollidayPaymentCode + "'", py_calc_id);

                        DataRow rowStopWorkingHours = dt.NewRow();
                        calc400New("Stop working hours current month", rowStopWorkingHours, emplIdsDirect, emplIdsIndirect, "'0712'", py_calc_id);

                        DataRow rowProductiveRecovery = dt.NewRow();
                        calc400New("Productive recovery", rowProductiveRecovery, emplIdsDirect, emplIdsIndirect, "'0612'", py_calc_id);


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
                            + double.Parse(rowUsedBankH[2].ToString()) + double.Parse(rowHoliday[2].ToString()) + double.Parse(rowCollectiveHoliday[2].ToString()) 
                            + double.Parse(rowReligiousHoliday[2].ToString()) + double.Parse(rowOvertimeOthers[2].ToString()) + double.Parse(rowLackOfWork[2].ToString())
                            + double.Parse(rowIllness[2].ToString()) + double.Parse(rowIllnessClosure[2].ToString())
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
                            + double.Parse(rowOvertimeOthers[4].ToString()) + double.Parse(rowLackOfWork[4].ToString()) + double.Parse(rowIllness[4].ToString())
                            + double.Parse(rowIllnessClosure[4].ToString()) + double.Parse(rowIllnessLayoff[4].ToString()) + double.Parse(rowIllnessStoppage[4].ToString()) + double.Parse(rowIllnessHoliday[4].ToString())
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
                        if (emplIdsDirect.Length > 0)
                            rowTotal[2] = Misc.CalcPlannedTime(dbConnection, emplIdsDirect, datesList[0], datesList[datesList.Count - 1]);
                        else
                            rowTotal[2] = 0;
                        if (emplIdsIndirect.Length > 0)
                            rowTotal[4] = Misc.CalcPlannedTime(dbConnection, emplIdsIndirect, datesList[0], datesList[datesList.Count - 1]);
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
                        Misc.percentage(rowAbsencesOtherLayoff, totalDD, totalID);
                        Misc.percentage(rowAbsencesOtherStoppage, totalDD, totalID);
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
                            + double.Parse(rowJustifiedAbsenceStoppage[3].ToString()) + double.Parse(rowJustifiedAbsenceHoliday[3].ToString())
                            + double.Parse(rowUsedBankH[3].ToString()) + double.Parse(rowHoliday[3].ToString()) + double.Parse(rowCollectiveHoliday[3].ToString()) + double.Parse(rowReligiousHoliday[3].ToString())
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
                    else
                        isEmpty = true;
                }
                return isEmpty;
            }
            catch (Exception ex)
            {
                throw ex;
                //debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + ".populateTable500() " + ex.Message + " for wu: " + workingUnit.WorkingUnitID);
                //return true;
            }
        }

        public string GenerateReportService(Object dbConnection, List<DateTime> datesList, string fileName, List<WorkingUnitTO> listWU, int company)
        {
            debug.writeLog("+ Started 500! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));


            DataSet ds = new DataSet(Path.GetFileNameWithoutExtension(fileName));
            try
            {
                TimeSpan timeSpan = datesList[datesList.Count - 1] - datesList[0].AddDays(-1);
                int numOfEmployees = 0;
                WorkingUnit wu;
                Employee Empl;

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
                System.Data.DataTable dtCompany = new System.Data.DataTable(company + "BC");
                dtCompany.Columns.Add("1", typeof(string));
                dtCompany.Columns.Add("   ", typeof(string));
                dtCompany.Columns.Add("Direct", typeof(string));
                dtCompany.Columns.Add("%  ", typeof(string));
                dtCompany.Columns.Add("Indirect", typeof(string));
                dtCompany.Columns.Add("%   ", typeof(string));
                dtCompany.Columns.Add("Total", typeof(string));
                dtCompany.Columns.Add(" %   ", typeof(string));
                
                int dcompany = 0;
                int numEmplCompanyDirect = 0;

                foreach (WorkingUnitTO plant in listWU)
                {
                    string plantString = "";
                    int dPlant = 0;
                    int numEmplPlantDirect = 0;
                    if (dbConnection == null)
                    {
                        wu = new WorkingUnit();
                    }
                    else
                    {
                        wu = new WorkingUnit(dbConnection);
                    }
                    System.Data.DataTable dtPlant = new System.Data.DataTable(plant.Name + "direct");
                    dtPlant.Columns.Add("1", typeof(string));
                    dtPlant.Columns.Add("   ", typeof(string));
                    dtPlant.Columns.Add("Direct", typeof(string));
                    dtPlant.Columns.Add("%  ", typeof(string));
                    dtPlant.Columns.Add("Indirect", typeof(string));
                    dtPlant.Columns.Add("%   ", typeof(string));
                    dtPlant.Columns.Add("Total", typeof(string));
                    dtPlant.Columns.Add(" %   ", typeof(string));

                    List<WorkingUnitTO> listCostCenter = wu.SearchChildWU(plant.WorkingUnitID.ToString());

                    foreach (WorkingUnitTO costCenter in listCostCenter)
                    {
                        int numEmplCostDirect = 0;
                        int dCost = 0;
                        if (dbConnection == null)
                        {
                            wu = new WorkingUnit();
                        }
                        else
                        {
                            wu = new WorkingUnit(dbConnection);
                        }

                        System.Data.DataTable dtCostCenter = new System.Data.DataTable(costCenter.Name + "direct");
                        dtCostCenter.Columns.Add("1", typeof(string));
                        dtCostCenter.Columns.Add("   ", typeof(string));
                        dtCostCenter.Columns.Add("Direct", typeof(string));
                        dtCostCenter.Columns.Add("%  ", typeof(string));
                        dtCostCenter.Columns.Add("Indirect", typeof(string));
                        dtCostCenter.Columns.Add("%   ", typeof(string));
                        dtCostCenter.Columns.Add("Total", typeof(string));
                        dtCostCenter.Columns.Add(" %   ", typeof(string));

                        List<WorkingUnitTO> listWorkshop = wu.SearchChildWU(costCenter.WorkingUnitID.ToString());

                        foreach (WorkingUnitTO workshop in listWorkshop)
                        {
                            int numEmplWorkgroupDirect = 0;
                            int d = 0; //TO CHECK IS IT FIRST TIME FILL DATATABLE FOR WORKSHOP
                            if (dbConnection == null)
                            {
                                wu = new WorkingUnit();
                            }
                            else
                            {
                                wu = new WorkingUnit(dbConnection);
                            }
                            System.Data.DataTable dtWorkGroup = new System.Data.DataTable(workshop.Name + "direct");
                            dtWorkGroup.Columns.Add("1", typeof(string));
                            dtWorkGroup.Columns.Add("   ", typeof(string));
                            dtWorkGroup.Columns.Add("Direct", typeof(string));
                            dtWorkGroup.Columns.Add("%  ", typeof(string));
                            dtWorkGroup.Columns.Add("Indirect", typeof(string));
                            dtWorkGroup.Columns.Add("%   ", typeof(string));
                            dtWorkGroup.Columns.Add("Total", typeof(string));
                            dtWorkGroup.Columns.Add(" %   ", typeof(string));

                            List<WorkingUnitTO> listUTE = wu.SearchChildWU(workshop.WorkingUnitID.ToString());
                            foreach (WorkingUnitTO workingUnit in listUTE)
                            {
                                System.Data.DataTable dtUTE = new System.Data.DataTable(workingUnit.Name + "direct");
                                dtUTE.Columns.Add("1", typeof(string));
                                dtUTE.Columns.Add("   ", typeof(string));
                                dtUTE.Columns.Add("Direct", typeof(string));
                                dtUTE.Columns.Add("%  ", typeof(string));
                                dtUTE.Columns.Add("Indirect", typeof(string));
                                dtUTE.Columns.Add("%   ", typeof(string));
                                dtUTE.Columns.Add("Total", typeof(string));
                                dtUTE.Columns.Add(" %   ", typeof(string));
                                ds.Tables.Add(dtUTE);

                                List<EmployeeTO> listEmpl = new List<EmployeeTO>();

                                listEmpl = Empl.SearchByWULoans(workingUnit.WorkingUnitID.ToString(), -1, null, datesList[0], datesList[datesList.Count - 1]);
                                numOfEmployees = listEmpl.Count;
                                numEmplWorkgroupDirect += numOfEmployees;
                                numEmplCostDirect += numOfEmployees;
                                numEmplPlantDirect += numOfEmployees;
                                numEmplCompanyDirect += numOfEmployees;
                                double totalDays = numOfEmployees * timeSpan.TotalDays;
                                //POPULATE TABLE FOR UTE AND THAN USE THIS DATA FOR WORKGROUP, COST CENTER, PLANT
                                if (!populateDataTable500New(dbConnection, dtUTE, workingUnit, datesList, company, "   Absenteeism industrial relation", 1))
                                {
                                    //LAST EMPTY ROW
                                    DataRow rowEmptyFooter = dtUTE.NewRow();
                                    dtUTE.Rows.Add(rowEmptyFooter);

                                    //FOOTER ROW
                                    DataRow footer = dtUTE.NewRow();
                                    footer[1] = "N° Employees: " + numOfEmployees;
                                    footer[2] = "Total days: ";
                                    footer[3] = totalDays;
                                    footer[4] = "Calendar days: ";
                                    footer[5] = timeSpan.TotalDays;

                                    dtUTE.Rows.Add(footer);

                                    d++;
                                    if (d == 1)
                                    {
                                        //FIRST TIME FILL DATATABLE FOR WORKSHOP, FIRST HEADER AND THEN FOREACH ROW OF UTE TABLE SET ROW FOR WORKGROUP TABLE
                                        string ute = "";
                                        string workGroup = "";
                                        string costCenterString = "";
                                        //string plantString = "";
                                        DataRow rowHeader1 = dtWorkGroup.NewRow();

                                        if (dbConnection == null)
                                            rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
                                        else
                                            rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
                                        rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                        rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                        //rowHeader1[8] = "page: " + page;
                                        dtWorkGroup.Rows.Add(rowHeader1);

                                        DataRow rowHeader2 = dtWorkGroup.NewRow();
                                        rowHeader2[1] = "   Absenteeism industrial relation";
                                        rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                        rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                                        dtWorkGroup.Rows.Add(rowHeader2);

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


                                        DataRow rowPlant = dtWorkGroup.NewRow();
                                        rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
                                        dtWorkGroup.Rows.Add(rowPlant);

                                        DataRow rowWorkgroup = dtWorkGroup.NewRow();
                                        rowWorkgroup[1] = "Workgroup: " + workGroup + " UTE: " + "xx";
                                        dtWorkGroup.Rows.Add(rowWorkgroup);

                                        DataRow rowEmpty = dtWorkGroup.NewRow();
                                        dtWorkGroup.Rows.Add(rowEmpty);

                                        //ROW COLUMN HEADER
                                        DataRow rowColumns = dtWorkGroup.NewRow();
                                        rowColumns[2] = "Direct";
                                        rowColumns[3] = "%";
                                        rowColumns[4] = "Indirect";
                                        rowColumns[5] = "%";
                                        rowColumns[6] = "Total";
                                        rowColumns[7] = "%";
                                        dtWorkGroup.Rows.Add(rowColumns);

                                        //FIRST 6 ROWS OF UTE TABLE ARE HEADER, DONT NEED IT HERE
                                        for (int ind = 6; ind < dtUTE.Rows.Count - 2; ind++)
                                        {
                                            DataRow row = dtWorkGroup.NewRow();
                                            row[1] = dtUTE.Rows[ind][1];
                                            row[2] = dtUTE.Rows[ind][2];
                                            //row[3] = dtUTE.Rows[ind][3];
                                            row[4] = dtUTE.Rows[ind][4];
                                            //row[5] = dtUTE.Rows[ind][5];
                                            row[6] = dtUTE.Rows[ind][6];
                                            //row[7] = dtUTE.Rows[ind][7];
                                            dtWorkGroup.Rows.Add(row);
                                            dtWorkGroup.AcceptChanges();
                                        }
                                    }
                                    else
                                    {
                                        //IF IS NOT FIRST TIME, NOT FIRST UTE IN WORKGROUP THEN ADD TO EXISTING TABLE WORKGROUP
                                        for (int ind = 6; ind < dtUTE.Rows.Count - 2; ind++)
                                        {
                                            // EMPTY ROW
                                            if (ind != rowDataWebNum)
                                            {
                                                //ROW +/-
                                                if (ind != (rowDataWebNum + 3))
                                                {
                                                    dtWorkGroup.Rows[ind][1] = dtUTE.Rows[ind][1];
                                                    dtWorkGroup.Rows[ind][2] = double.Parse(dtWorkGroup.Rows[ind][2].ToString()) + double.Parse(dtUTE.Rows[ind][2].ToString());
                                                    dtWorkGroup.Rows[ind][4] = double.Parse(dtWorkGroup.Rows[ind][4].ToString()) + double.Parse(dtUTE.Rows[ind][4].ToString());
                                                    dtWorkGroup.Rows[ind][6] = double.Parse(dtWorkGroup.Rows[ind][6].ToString()) + double.Parse(dtUTE.Rows[ind][6].ToString());
                                                }
                                                else
                                                {
                                                    dtWorkGroup.Rows[ind][1] = dtUTE.Rows[ind][1];
                                                    dtWorkGroup.Rows[ind][2] = double.Parse(dtWorkGroup.Rows[ind][2].ToString()) + double.Parse(dtUTE.Rows[ind][2].ToString());
                                                    dtWorkGroup.Rows[ind][4] = double.Parse(dtWorkGroup.Rows[ind][4].ToString()) + double.Parse(dtUTE.Rows[ind][4].ToString());
                                                    dtWorkGroup.Rows[ind][6] = double.Parse(dtWorkGroup.Rows[ind][6].ToString()) + double.Parse(dtUTE.Rows[ind][6].ToString());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (dbConnection == null)
                            {
                                wu = new WorkingUnit();
                            }
                            else
                            {
                                wu = new WorkingUnit(dbConnection);
                            }
                            List<WorkingUnitTO> oneWorkShop = new List<WorkingUnitTO>();
                            oneWorkShop.Add(workshop);
                            List<WorkingUnitTO> workShopAndUTE = wu.FindAllChildren(oneWorkShop);
                            if (dtWorkGroup.Rows.Count > 0)
                            {
                                double totalDD = 0;
                                double totalID = 0;
                                double totalDDSum = 0;
                                double totalIDSum = 0;
                                for (int i = 6; i < rowDataWebNum; i++)
                                {
                                    totalDD = double.Parse(dtWorkGroup.Rows[i][2].ToString());
                                    totalID = double.Parse(dtWorkGroup.Rows[i][4].ToString());
                                    totalDDSum += totalDD;
                                    totalIDSum += totalID;
                                }
                                double totaltotal = totalDDSum + totalIDSum;
                                for (int i = 6; i < rowDataWebNum; i++)
                                {
                                    Misc.percentage(dtWorkGroup.Rows[i], totalDDSum, totalIDSum);
                                    dtWorkGroup.Rows[i][7] = double.Parse(dtWorkGroup.Rows[i][6].ToString()) * 100 / totaltotal;
                                    dtWorkGroup.AcceptChanges();

                                }
                                double totalPerDD = 0;
                                double totalPerID = 0;
                                double totalPerSum = 0;
                                for (int i = 6; i < rowDataWebNum; i++)
                                {
                                    totalPerDD += double.Parse(dtWorkGroup.Rows[i][3].ToString());
                                    totalPerID += double.Parse(dtWorkGroup.Rows[i][5].ToString());
                                    totalPerSum += double.Parse(dtWorkGroup.Rows[i][7].ToString());
                                }
                                dtWorkGroup.Rows[rowDataWebNum + 1][3] = totalPerDD;
                                dtWorkGroup.Rows[rowDataWebNum + 1][5] = totalPerID;
                                dtWorkGroup.Rows[rowDataWebNum + 1][7] = totalPerSum;

                                dtWorkGroup.Rows[rowDataWebNum + 2][3] = 0;
                                dtWorkGroup.Rows[rowDataWebNum + 2][5] = 0;
                                dtWorkGroup.Rows[rowDataWebNum + 2][7] = 0;

                                dtWorkGroup.Rows[rowDataWebNum + 3][3] = "  ";
                                dtWorkGroup.Rows[rowDataWebNum + 3][5] = "  ";
                                dtWorkGroup.Rows[rowDataWebNum + 3][7] = "  ";
                                dtWorkGroup.AcceptChanges();
                                numOfEmployees = numEmplWorkgroupDirect;
                                double totalDays = numOfEmployees * timeSpan.TotalDays;
                                DataRow rowEmptyFooter = dtWorkGroup.NewRow();
                                dtWorkGroup.Rows.Add(rowEmptyFooter);
                                DataRow footer = dtWorkGroup.NewRow();

                                //FOOTER ROW IN WORKGROUP TABLE
                                footer[1] = "N° Employees: " + numOfEmployees;
                                footer[2] = "Total days: ";
                                footer[3] = totalDays;
                                footer[4] = "Calendar days: ";
                                footer[5] = timeSpan.TotalDays;

                                dtWorkGroup.Rows.Add(footer);
                                ds.Tables.Add(dtWorkGroup);
                                dCost++;
                                if (dCost == 1)
                                {
                                    //FIRST TIME FILL OF TABLE COST CENTER, FIRST WORKGROUP IN CC
                                    string ute = "";
                                    string workGroup = "";
                                    string costCenterString = "";
                                    //string plantString = "";
                                    DataRow rowHeader1 = dtCostCenter.NewRow();

                                    if (dbConnection == null)
                                        rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
                                    else
                                        rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
                                    rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                    rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                    //rowHeader1[8] = "page: " + page;
                                    dtCostCenter.Rows.Add(rowHeader1);

                                    DataRow rowHeader2 = dtCostCenter.NewRow();
                                    rowHeader2[1] = "   Absenteeism industrial relation";
                                    rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                    rowHeader2[7] = datesList[0].ToString("MMMM yyyy");

                                    dtCostCenter.Rows.Add(rowHeader2);
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

                                    DataRow rowPlant = dtCostCenter.NewRow();
                                    rowPlant[1] = "Plant:     " + plantString + "     Cost center: " + costCenterString;
                                    dtCostCenter.Rows.Add(rowPlant);
                                    DataRow rowWorkgroup = dtCostCenter.NewRow();
                                    rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
                                    dtCostCenter.Rows.Add(rowWorkgroup);
                                    DataRow rowEmpty = dtCostCenter.NewRow();
                                    dtCostCenter.Rows.Add(rowEmpty);

                                    DataRow rowColumns = dtCostCenter.NewRow();
                                    rowColumns[2] = "Direct";
                                    rowColumns[3] = "%";
                                    rowColumns[4] = "Indirect";
                                    rowColumns[5] = "%";
                                    rowColumns[6] = "Total";
                                    rowColumns[7] = "%";
                                    dtCostCenter.Rows.Add(rowColumns);

                                    //FIRST 6 ROWS ARE HEADER OF TABLE WORKGROUP
                                    for (int ind = 6; ind < dtWorkGroup.Rows.Count - 2; ind++)
                                    {
                                        DataRow row = dtCostCenter.NewRow();
                                        row[1] = dtWorkGroup.Rows[ind][1];
                                        row[2] = dtWorkGroup.Rows[ind][2];
                                        row[4] = dtWorkGroup.Rows[ind][4];
                                        row[6] = dtWorkGroup.Rows[ind][6];
                                        dtCostCenter.Rows.Add(row);
                                        dtCostCenter.AcceptChanges();

                                    }
                                }
                                else
                                {
                                    for (int ind = 6; ind < dtWorkGroup.Rows.Count - 2; ind++)
                                    {
                                        //23 IS EMPTY ROW
                                        if (ind != rowDataWebNum)
                                        {
                                            //ROW +/-, DIFFERENT
                                            if (ind != (rowDataWebNum + 3))
                                            {
                                                dtCostCenter.Rows[ind][1] = dtWorkGroup.Rows[ind][1];
                                                dtCostCenter.Rows[ind][2] = double.Parse(dtCostCenter.Rows[ind][2].ToString()) + double.Parse(dtWorkGroup.Rows[ind][2].ToString());
                                                dtCostCenter.Rows[ind][4] = double.Parse(dtCostCenter.Rows[ind][4].ToString()) + double.Parse(dtWorkGroup.Rows[ind][4].ToString());
                                                dtCostCenter.Rows[ind][6] = double.Parse(dtCostCenter.Rows[ind][6].ToString()) + double.Parse(dtWorkGroup.Rows[ind][6].ToString());
                                            }
                                            else
                                            {
                                                dtCostCenter.Rows[ind][1] = dtWorkGroup.Rows[ind - 1][1];
                                                dtCostCenter.Rows[ind][2] = double.Parse(dtCostCenter.Rows[ind][2].ToString()) + double.Parse(dtWorkGroup.Rows[ind][2].ToString());
                                                dtCostCenter.Rows[ind][4] = double.Parse(dtCostCenter.Rows[ind][4].ToString()) + double.Parse(dtWorkGroup.Rows[ind][4].ToString());
                                                dtCostCenter.Rows[ind][6] = double.Parse(dtCostCenter.Rows[ind][6].ToString()) + double.Parse(dtWorkGroup.Rows[ind][6].ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (dbConnection == null)
                        {
                            wu = new WorkingUnit();
                        }
                        else
                        {
                            wu = new WorkingUnit(dbConnection);
                        }
                        List<WorkingUnitTO> oneCostCenter = new List<WorkingUnitTO>();
                        oneCostCenter.Add(costCenter);
                        List<WorkingUnitTO> CostCenterAndWorkShop = wu.FindAllChildren(oneCostCenter);
                        if (dtCostCenter.Rows.Count > 0)
                        {
                            double totalDD = 0;
                            double totalID = 0;
                            double totalDDSum = 0;
                            double totalIDSum = 0;
                            for (int i = 6; i < rowDataWebNum; i++)
                            {
                                totalDD = double.Parse(dtCostCenter.Rows[i][2].ToString());
                                totalID = double.Parse(dtCostCenter.Rows[i][4].ToString());
                                totalDDSum += totalDD;
                                totalIDSum += totalID;
                            }
                            double totaltotal = totalDDSum + totalIDSum;
                            for (int i = 6; i < rowDataWebNum; i++)
                            {
                                Misc.percentage(dtCostCenter.Rows[i], totalDDSum, totalIDSum);
                                dtCostCenter.Rows[i][7] = double.Parse(dtCostCenter.Rows[i][6].ToString()) * 100 / totaltotal;
                                dtCostCenter.AcceptChanges();

                            }
                            double totalPerDD = 0;
                            double totalPerID = 0;
                            double totalPerSum = 0;
                            for (int i = 6; i < rowDataWebNum; i++)
                            {
                                totalPerDD += double.Parse(dtCostCenter.Rows[i][3].ToString());
                                totalPerID += double.Parse(dtCostCenter.Rows[i][5].ToString());
                                totalPerSum += double.Parse(dtCostCenter.Rows[i][7].ToString());
                            }
                            dtCostCenter.Rows[rowDataWebNum + 1][3] = totalPerDD;
                            dtCostCenter.Rows[rowDataWebNum + 1][5] = totalPerID;
                            dtCostCenter.Rows[rowDataWebNum + 1][7] = totalPerSum;

                            dtCostCenter.Rows[rowDataWebNum + 2][3] = 0;
                            dtCostCenter.Rows[rowDataWebNum + 2][5] = 0;
                            dtCostCenter.Rows[rowDataWebNum + 2][7] = 0;

                            dtCostCenter.Rows[rowDataWebNum + 3][3] = "  ";
                            dtCostCenter.Rows[rowDataWebNum + 3][5] = "  ";
                            dtCostCenter.Rows[rowDataWebNum + 3][7] = "  ";
                            dtCostCenter.AcceptChanges();
                            //CC FOOTER ROW
                            numOfEmployees = numEmplCostDirect;
                            double totalDays = numOfEmployees * timeSpan.TotalDays;

                            DataRow rowEmptyFooter = dtCostCenter.NewRow();
                            dtCostCenter.Rows.Add(rowEmptyFooter);

                            DataRow footer = dtCostCenter.NewRow();
                            footer[1] = "N° Employees: " + numOfEmployees;
                            footer[2] = "Total days: ";
                            footer[3] = totalDays;
                            footer[4] = "Calendar days: ";
                            footer[5] = timeSpan.TotalDays;
                            dtCostCenter.Rows.Add(footer);

                            ds.Tables.Add(dtCostCenter);

                            dPlant++;
                            if (dPlant == 1)
                            {
                                //FIRST TIME FILL PLAN TABLE
                                DataRow rowHeader1 = dtPlant.NewRow();
                                if (dbConnection == null)
                                    rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
                                else
                                    rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
                                rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                                rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                                //rowHeader1[8] = "page: " + page;
                                dtPlant.Rows.Add(rowHeader1);

                                DataRow rowHeader2 = dtPlant.NewRow();
                                rowHeader2[1] = "   Absenteeism industrial relation";
                                rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                                rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                                dtPlant.Rows.Add(rowHeader2);

                                DataRow rowPlant = dtPlant.NewRow();
                                rowPlant[1] = "Plant:    " + plantString + "    Cost center: " + "xxxx";
                                dtPlant.Rows.Add(rowPlant);

                                DataRow rowWorkgroup = dtPlant.NewRow();
                                rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
                                dtPlant.Rows.Add(rowWorkgroup);

                                DataRow rowEmpty = dtPlant.NewRow();
                                dtPlant.Rows.Add(rowEmpty);

                                DataRow rowColumns = dtPlant.NewRow();
                                rowColumns[2] = "Direct";
                                rowColumns[3] = "%";
                                rowColumns[4] = "Indirect";
                                rowColumns[5] = "%";
                                rowColumns[6] = "Total";
                                rowColumns[7] = "%";
                                dtPlant.Rows.Add(rowColumns);

                                //FIRST 6 ROWS OF CC TABLE ARE HEADER ROW
                                for (int ind = 6; ind < dtCostCenter.Rows.Count - 2; ind++)
                                {
                                    DataRow row = dtPlant.NewRow();
                                    row[0] = "";
                                    row[1] = dtCostCenter.Rows[ind][1];
                                    row[2] = dtCostCenter.Rows[ind][2];
                                    row[4] = dtCostCenter.Rows[ind][4];
                                    row[6] = dtCostCenter.Rows[ind][6];
                                    dtPlant.Rows.Add(row);
                                    dtPlant.AcceptChanges();
                                }
                            }
                            else
                            {
                                for (int ind = 6; ind < dtCostCenter.Rows.Count - 2; ind++)
                                {
                                    //23 EMPTY ROW
                                    if (ind != rowDataWebNum)
                                    {
                                        //ROW +/-
                                        if (ind != (rowDataWebNum + 3))
                                        {
                                            dtPlant.Rows[ind][1] = dtCostCenter.Rows[ind][1];
                                            dtPlant.Rows[ind][2] = double.Parse(dtPlant.Rows[ind][2].ToString()) + double.Parse(dtCostCenter.Rows[ind][2].ToString());
                                            dtPlant.Rows[ind][4] = double.Parse(dtPlant.Rows[ind][4].ToString()) + double.Parse(dtCostCenter.Rows[ind][4].ToString());
                                            dtPlant.Rows[ind][6] = double.Parse(dtPlant.Rows[ind][6].ToString()) + double.Parse(dtCostCenter.Rows[ind][6].ToString());
                                        }
                                        else
                                        {
                                            dtPlant.Rows[ind][1] = dtCostCenter.Rows[ind - 1][1];
                                            dtPlant.Rows[ind][2] = double.Parse(dtPlant.Rows[ind][2].ToString()) + double.Parse(dtCostCenter.Rows[ind][2].ToString());
                                            dtPlant.Rows[ind][4] = double.Parse(dtPlant.Rows[ind][4].ToString()) + double.Parse(dtCostCenter.Rows[ind][4].ToString());
                                            dtPlant.Rows[ind][6] = double.Parse(dtPlant.Rows[ind][6].ToString()) + double.Parse(dtCostCenter.Rows[ind][6].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (dbConnection == null)
                    {
                        wu = new WorkingUnit();
                    }
                    else
                    {
                        wu = new WorkingUnit(dbConnection);
                    }

                    List<WorkingUnitTO> onePlant = new List<WorkingUnitTO>();
                    onePlant.Add(plant);
                    List<WorkingUnitTO> PlantAndCostCentre = wu.FindAllChildren(onePlant);
                    if (dtPlant.Rows.Count > 0)
                    {
                        double totalDD = 0;
                        double totalID = 0;
                        double totalDDSum = 0;
                        double totalIDSum = 0;
                        for (int i = 6; i < rowDataWebNum; i++)
                        {
                            totalDD = double.Parse(dtPlant.Rows[i][2].ToString());
                            totalID = double.Parse(dtPlant.Rows[i][4].ToString());
                            totalDDSum += totalDD;
                            totalIDSum += totalID;
                        }
                        double totaltotal = totalDDSum + totalIDSum;
                        for (int i = 6; i < rowDataWebNum; i++)
                        {
                            Misc.percentage(dtPlant.Rows[i], totalDDSum, totalIDSum);
                            dtPlant.Rows[i][7] = double.Parse(dtPlant.Rows[i][6].ToString()) * 100 / totaltotal;
                            dtPlant.AcceptChanges();

                        }
                        double totalPerDD = 0;
                        double totalPerID = 0;
                        double totalPerSum = 0;
                        for (int i = 6; i < rowDataWebNum; i++)
                        {
                            totalPerDD += double.Parse(dtPlant.Rows[i][3].ToString());
                            totalPerID += double.Parse(dtPlant.Rows[i][5].ToString());
                            totalPerSum += double.Parse(dtPlant.Rows[i][7].ToString());
                        }
                        dtPlant.Rows[rowDataWebNum + 1][3] = totalPerDD;
                        dtPlant.Rows[rowDataWebNum + 1][5] = totalPerID;
                        dtPlant.Rows[rowDataWebNum + 1][7] = totalPerSum;

                        dtPlant.Rows[rowDataWebNum + 2][3] = 0;
                        dtPlant.Rows[rowDataWebNum + 2][5] = 0;
                        dtPlant.Rows[rowDataWebNum + 2][7] = 0;

                        dtPlant.Rows[rowDataWebNum + 3][3] = "  ";
                        dtPlant.Rows[rowDataWebNum + 3][5] = "  ";
                        dtPlant.Rows[rowDataWebNum + 3][7] = "  ";
                        dtPlant.AcceptChanges();
                        //PLANT FOOTER ROW
                        numOfEmployees = numEmplPlantDirect;
                        double totalDays = numOfEmployees * timeSpan.TotalDays;
                        DataRow rowEmptyFooter = dtPlant.NewRow();
                        dtPlant.Rows.Add(rowEmptyFooter);

                        DataRow footer = dtPlant.NewRow();
                        footer[1] = "N° Employees: " + numOfEmployees;
                        footer[2] = "Total days: ";
                        footer[3] = totalDays;
                        footer[4] = "Calendar days: ";
                        footer[5] = timeSpan.TotalDays;

                        dtPlant.Rows.Add(footer);
                        ds.Tables.Add(dtPlant);

                        dcompany++;
                        if (dcompany == 1)
                        {
                            //FIRST TIME FILL PLAN TABLE
                            DataRow rowHeader1 = dtCompany.NewRow();
                            if (dbConnection == null)
                                rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
                            else
                                rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
                            rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                            rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                            //rowHeader1[8] = "page: " + page;
                            dtCompany.Rows.Add(rowHeader1);

                            DataRow rowHeader2 = dtCompany.NewRow();
                            rowHeader2[1] = "   Absenteeism industrial relation";
                            rowHeader2[6] = datesList[0].ToString("dd") + "-" + datesList[datesList.Count - 1].ToString("dd.");
                            rowHeader2[7] = datesList[0].ToString("MMMM yyyy");
                            dtCompany.Rows.Add(rowHeader2);

                            DataRow rowPlant = dtCompany.NewRow();
                            rowPlant[1] = "Company report";
                            dtCompany.Rows.Add(rowPlant);

                            DataRow rowWorkgroup = dtCompany.NewRow();
                            //rowWorkgroup[1] = "Workgroup: " + "xxx" + "  UTE: " + "xx";
                            dtCompany.Rows.Add(rowWorkgroup);

                            DataRow rowEmpty = dtCompany.NewRow();
                            dtCompany.Rows.Add(rowEmpty);

                            DataRow rowColumns = dtCompany.NewRow();
                            rowColumns[2] = "Direct";
                            rowColumns[3] = "%";
                            rowColumns[4] = "Indirect";
                            rowColumns[5] = "%";
                            rowColumns[6] = "Total";
                            rowColumns[7] = "%";
                            dtCompany.Rows.Add(rowColumns);

                            //FIRST 6 ROWS OF CC TABLE ARE HEADER ROW
                            for (int ind = 6; ind < dtPlant.Rows.Count - 2; ind++)
                            {
                                DataRow row = dtCompany.NewRow();
                                row[0] = "";
                                row[1] = dtPlant.Rows[ind][1];
                                row[2] = dtPlant.Rows[ind][2];
                                row[4] = dtPlant.Rows[ind][4];
                                row[6] = dtPlant.Rows[ind][6];
                                dtCompany.Rows.Add(row);
                                dtCompany.AcceptChanges();
                            }
                        }
                        else
                        {
                            for (int ind = 6; ind < dtPlant.Rows.Count - 2; ind++)
                            {
                                //23 EMPTY ROW
                                if (ind != rowDataWebNum)
                                {
                                    //ROW +/-
                                    if (ind != (rowDataWebNum + 3) && ind != (rowDataWebNum + 1))
                                    {
                                        dtCompany.Rows[ind][1] = dtPlant.Rows[ind][1];
                                        dtCompany.Rows[ind][2] = double.Parse(dtCompany.Rows[ind][2].ToString()) + double.Parse(dtPlant.Rows[ind][2].ToString());
                                        dtCompany.Rows[ind][4] = double.Parse(dtCompany.Rows[ind][4].ToString()) + double.Parse(dtPlant.Rows[ind][4].ToString());
                                        dtCompany.Rows[ind][6] = double.Parse(dtCompany.Rows[ind][6].ToString()) + double.Parse(dtPlant.Rows[ind][6].ToString());
                                    }
                                    else
                                    {
                                        dtCompany.Rows[ind][1] = dtPlant.Rows[ind][1];
                                        dtCompany.Rows[ind][2] = double.Parse(dtCompany.Rows[ind][2].ToString()) + double.Parse(dtPlant.Rows[ind][2].ToString());
                                        dtCompany.Rows[ind][4] = double.Parse(dtCompany.Rows[ind][4].ToString()) + double.Parse(dtPlant.Rows[ind][4].ToString());
                                        dtCompany.Rows[ind][6] = double.Parse(dtCompany.Rows[ind][6].ToString()) + double.Parse(dtPlant.Rows[ind][6].ToString());
                                    }
                                }
                            }
                        }

                    }
                }

                if (dtCompany.Rows.Count > 0)
                {
                    double totalDD = 0;
                    double totalID = 0;
                    double totalDDSum = 0;
                    double totalIDSum = 0;
                    for (int i = 6; i < rowDataWebNum; i++)
                    {
                        totalDD = double.Parse(dtCompany.Rows[i][2].ToString());
                        totalID = double.Parse(dtCompany.Rows[i][4].ToString());
                        totalDDSum += totalDD;
                        totalIDSum += totalID;
                    }
                    double totaltotal = totalDDSum + totalIDSum;
                    for (int i = 6; i < rowDataWebNum; i++)
                    {
                        Misc.percentage(dtCompany.Rows[i], totalDDSum, totalIDSum);
                        dtCompany.Rows[i][7] = double.Parse(dtCompany.Rows[i][6].ToString()) * 100 / totaltotal;
                        dtCompany.AcceptChanges();

                    }
                    double totalPerDD = 0;
                    double totalPerID = 0;
                    double totalPerSum = 0;
                    for (int i = 6; i < rowDataWebNum; i++)
                    {
                        totalPerDD += double.Parse(dtCompany.Rows[i][3].ToString());
                        totalPerID += double.Parse(dtCompany.Rows[i][5].ToString());
                        totalPerSum += double.Parse(dtCompany.Rows[i][7].ToString());
                    }
                    dtCompany.Rows[rowDataWebNum + 1][3] = totalPerDD;
                    dtCompany.Rows[rowDataWebNum + 1][5] = totalPerID;
                    dtCompany.Rows[rowDataWebNum + 1][7] = totalPerSum;

                    dtCompany.Rows[rowDataWebNum + 2][3] = 0;
                    dtCompany.Rows[rowDataWebNum + 2][5] = 0;
                    dtCompany.Rows[rowDataWebNum + 2][7] = 0;

                    dtCompany.Rows[rowDataWebNum + 3][3] = "  ";
                    dtCompany.Rows[rowDataWebNum + 3][5] = "  ";
                    dtCompany.Rows[rowDataWebNum + 3][7] = "  ";
                    dtCompany.AcceptChanges();
                    //PLANT FOOTER ROW
                    numOfEmployees = numEmplCompanyDirect;
                    double totalDays = numOfEmployees * timeSpan.TotalDays;
                    DataRow rowEmptyFooter = dtCompany.NewRow();
                    dtCompany.Rows.Add(rowEmptyFooter);

                    DataRow footer = dtCompany.NewRow();
                    footer[1] = "N° Employees: " + numOfEmployees;
                    footer[2] = "Total days: ";
                    footer[3] = totalDays;
                    footer[4] = "Calendar days: ";
                    footer[5] = timeSpan.TotalDays;

                    dtCompany.Rows.Add(footer);
                    ds.Tables.Add(dtCompany);
                }
                string Pathh = Directory.GetParent(fileName).FullName;
                if (!Directory.Exists(Pathh))
                {
                    Directory.CreateDirectory(Pathh);
                }
                if (File.Exists(fileName))
                    File.Delete(fileName);

                ExportToExcel.CreateExcelDocument(ds, fileName, false, false);

                debug.writeLog("+ Finished 500! " + " for company: " + company + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                return fileName;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "generateExcel500() " + ex.Message);
                return "";
            }
        }

        private bool populateDataTable500New(Object dbConnection, DataTable dt, WorkingUnitTO workingUnit, List<DateTime> datesList, int company, string title, int level)
        {
            bool isEmpty = true;
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

                DataRow rowHeader1 = dt.NewRow();
                if (dbConnection == null)
                    rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit().FindWU(company)).Description;
                else
                    rowHeader1[1] = ((WorkingUnitTO)new WorkingUnit(dbConnection).FindWU(company)).Description;
                rowHeader1[6] = DateTime.Now.ToString("dd/MM/yy");
                rowHeader1[7] = DateTime.Now.ToString("HH:mm");
                //rowHeader1[8] = "page: " + page;
                dt.Rows.Add(rowHeader1);

                DataRow rowHeader2 = dt.NewRow();
                rowHeader2[1] = title;
                rowHeader2[6] = datesList[0].ToString("dd.MM") + "-" + datesList[datesList.Count - 1].ToString("dd.MM");
                //rowHeader2[7] = datesList[0].ToString("MMMM yyyy");

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
                Employee Empl;
                IOPairProcessed IOPairProc;
                PassType PassT;
                if (dbConnection == null)
                {
                    Empl = new Employee();
                    IOPairProc = new IOPairProcessed();
                    PassT = new PassType();
                }
                else
                {
                    Empl = new Employee(dbConnection);
                    IOPairProc = new IOPairProcessed(dbConnection);
                    PassT = new PassType(dbConnection);
                }
                List<EmployeeTO> listEmpl = new List<EmployeeTO>();

                listEmpl = Empl.SearchByWULoans(workingUnit.WorkingUnitID.ToString(), -1, null, datesList[0], datesList[datesList.Count - 1]);
                
                if (listEmpl.Count > 0)
                {
                    string emplList = "";
                    foreach (EmployeeTO empl in listEmpl)
                    {

                        emplList += empl.EmployeeID + ",";
                    }
                    if (emplList.Length > 0)
                        emplList = emplList.Remove(emplList.LastIndexOf(','));

                    Dictionary<string, string> dictEmplBranch = Misc.emplBranch(dbConnection, emplList, company);
                    Dictionary<int, PassTypeTO> listPassTypes = new Dictionary<int, PassTypeTO>();
                    string emplIdsDirect = "";
                    string emplIdsIndirect = "";
                    foreach (KeyValuePair<string, string> pair in dictEmplBranch)
                    {
                        if (pair.Value == "A")
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

                    if (emplIdsDirect.Length > 0)
                        IOPairListDirect = IOPairProc.SearchAllPairsForEmpl(emplIdsDirect, datesList, "");
                    else
                        IOPairListDirect = new List<IOPairProcessedTO>();
                    if (dbConnection == null)
                    {
                        IOPairProc = new IOPairProcessed();
                    }
                    else
                    {
                        IOPairProc = new IOPairProcessed(dbConnection);
                    }
                    if (emplIdsIndirect.Length > 0)
                        IOPairListIndirect = IOPairProc.SearchAllPairsForEmpl(emplIdsIndirect, datesList, "");
                    else
                        IOPairListIndirect = new List<IOPairProcessedTO>();
                    if (IOPairListIndirect.Count > 0 || IOPairListDirect.Count > 0)
                    {
                        isEmpty = false;
                        DataRow rowNew = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("0012,0014,0071", company);
                        if (listPassTypes.Count > 0)
                            calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "PRESENCE", rowNew, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("PRESENCE", rowNew);

                        DataRow rowNotJustified = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("0000", -1);

                        if (listPassTypes.Count > 0)
                            calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "NOT JUSTIFIED HOURS", rowNotJustified, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("NOT JUSTIFIED HOURS", rowNotJustified);

                        DataRow rowBloodDonation = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("0043", company);
                        if (listPassTypes.Count > 0) calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "BLOOD DONATION", rowBloodDonation, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("BLOOD DONATION", rowBloodDonation);

                        DataRow rowTissueDonation = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("0357", company);
                        if (listPassTypes.Count > 0) calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "TISSUE AND ORGANS DONATION", rowTissueDonation, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("TISSUE AND ORGANS DONATION", rowTissueDonation);

                        DataRow rowIllness = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("0057,0060,0157,7777", company);
                        if (listPassTypes.Count > 0) calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "ILLNESS", rowIllness, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("ILLNESS", rowIllness);

                        DataRow rowOvertime = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("0030", company);
                        if (listPassTypes.Count > 0) calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "OVERTIME", rowOvertime, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("OVERTIME", rowOvertime);

                        DataRow rowOvertimeBank = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("0312", company);
                        if (listPassTypes.Count > 0) calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "OVERPRESENCE BANK HOURS", rowOvertimeBank, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("OVERPRESENCE BANK HOURS", rowOvertimeBank);

                        DataRow rowWeddingLeaves = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("0049", company);
                        if (listPassTypes.Count > 0) calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "WEDDING LEAVES", rowWeddingLeaves, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("WEDDING LEAVES", rowWeddingLeaves);

                        DataRow rowUnpaidLeaves = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("0169,1157", company);
                        if (listPassTypes.Count > 0) calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "UNPAID LEAVES", rowUnpaidLeaves, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("UNPAID LEAVES", rowUnpaidLeaves);

                        string s = "0045,0046,0047,0048,0055,0056,0061,0069,0070,0075,0144,0145,0146,0147,0148,0149,0155,";
                        s += "0257,1257,1148,1155,0369,1145,1149,1150,0153,0171,0130,1069,0160,1160,1406";
                        listPassTypes = PassT.FindByPaymentCode(s, company);
                        DataRow rowPaidLeaves = dt.NewRow();
                        if (listPassTypes.Count > 0) calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "PAID LEAVES", rowPaidLeaves, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("PAID LEAVES", rowPaidLeaves);

                        DataRow rowStrike = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("1407", company);
                        if (listPassTypes.Count > 0) calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "STRIKE", rowStrike, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("STRIKE", rowStrike);


                        DataRow rowNoWork = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("0053", company);
                        if (listPassTypes.Count > 0) calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "NO WORK", rowNoWork, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("NO WORK", rowNoWork);

                        Dictionary<int, PassTypeTO> alPTDict = PassT.FindByPaymentCode("0040", company);
                        DataRow rowHoliday = dt.NewRow();
                        listPassTypes = new Dictionary<int, PassTypeTO>();
                        List<int> alTypes = new Common.Rule().SearchRulesExact(Constants.RuleCompanyAnnualLeave);
                        foreach (int ptID in alPTDict.Keys)
                        {
                            if (alTypes.Contains(ptID) && !listPassTypes.ContainsKey(ptID))
                                listPassTypes.Add(ptID, alPTDict[ptID]);
                        }
                        if (listPassTypes.Count > 0)
                            calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "HOLIDAYS", rowHoliday, emplIdsDirect, emplIdsIndirect, datesList);
                        else
                            calc400Empty("HOLIDAYS", rowHoliday);

                        DataRow rowCollectiveHoliday = dt.NewRow();
                        listPassTypes = new Dictionary<int, PassTypeTO>();
                        List<int> calTypes = new Common.Rule().SearchRulesExact(Constants.RuleCompanyCollectiveAnnualLeave);
                        foreach (int ptID in alPTDict.Keys)
                        {
                            if (calTypes.Contains(ptID) && !listPassTypes.ContainsKey(ptID))
                                listPassTypes.Add(ptID, alPTDict[ptID]);
                        }
                        if (listPassTypes.Count > 0)
                            calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "COLLECTIVE HOLIDAYS", rowCollectiveHoliday, emplIdsDirect, emplIdsIndirect, datesList);
                        else
                            calc400Empty("COLLECTIVE HOLIDAYS", rowCollectiveHoliday);

                        DataRow rowReligiousHoliday = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("0044", company);
                        if (listPassTypes.Count > 0)
                            calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "RELIGIOUS HOLIDAYS", rowReligiousHoliday, emplIdsDirect, emplIdsIndirect, datesList);
                        else
                            calc400Empty("RELIGIOUS HOLIDAYS", rowReligiousHoliday);

                        DataRow rowAccidents = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("0058,0156", company);
                        if (listPassTypes.Count > 0) calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "ACCIDENTS", rowAccidents, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("ACCIDENTS", rowAccidents);

                        DataRow rowUsedBankH = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("0212", company);
                        if (listPassTypes.Count > 0) calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "USED BANK HOURS", rowUsedBankH, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("USED BANK HOURS", rowUsedBankH);

                        DataRow rowUnionLeaves = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("0112", company);
                        if (listPassTypes.Count > 0) calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "UNION LEAVES", rowUnionLeaves, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("UNION LEAVES", rowUnionLeaves);

                        DataRow rowNoWorkRecoveryH = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("0512", company);
                        if (listPassTypes.Count > 0) calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "NO WORK RECOVERY HOURS", rowNoWorkRecoveryH, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("NO WORK RECOVERY HOURS", rowNoWorkRecoveryH);

                        DataRow rowProductiveLeaves = dt.NewRow();
                        listPassTypes = PassT.FindByPaymentCode("0612", company);
                        if (listPassTypes.Count > 0) calc500(IOPairListDirect, IOPairListIndirect, listPassTypes, "PRODUCTIVE RECOVERY", rowProductiveLeaves, emplIdsDirect, emplIdsIndirect, datesList);
                        else calc400Empty("PRODUCTIVE RECOVERY", rowProductiveLeaves);

                        double totalDD = double.Parse(rowProductiveLeaves[2].ToString()) + double.Parse(rowNew[2].ToString()) + double.Parse(rowNoWorkRecoveryH[2].ToString()) 
                            + double.Parse(rowPaidLeaves[2].ToString()) + double.Parse(rowUnionLeaves[2].ToString()) + double.Parse(rowUsedBankH[2].ToString())
                            + double.Parse(rowHoliday[2].ToString()) + double.Parse(rowCollectiveHoliday[2].ToString()) + double.Parse(rowReligiousHoliday[2].ToString()) 
                            + double.Parse(rowNoWork[2].ToString()) + double.Parse(rowStrike[2].ToString()) + double.Parse(rowNotJustified[2].ToString())
                            + double.Parse(rowOvertime[2].ToString()) + double.Parse(rowOvertimeBank[2].ToString()) + double.Parse(rowIllness[2].ToString()) + double.Parse(rowAccidents[2].ToString())
                            + double.Parse(rowWeddingLeaves[2].ToString()) + double.Parse(rowBloodDonation[2].ToString()) + double.Parse(rowTissueDonation[2].ToString()); ;

                        double totalID = double.Parse(rowProductiveLeaves[4].ToString()) + double.Parse(rowNew[4].ToString()) + double.Parse(rowNoWorkRecoveryH[4].ToString()) 
                            + double.Parse(rowPaidLeaves[4].ToString()) + double.Parse(rowUnionLeaves[4].ToString()) + double.Parse(rowUsedBankH[4].ToString())
                            + double.Parse(rowHoliday[4].ToString()) + double.Parse(rowCollectiveHoliday[4].ToString()) + double.Parse(rowReligiousHoliday[4].ToString()) 
                            + double.Parse(rowNoWork[4].ToString()) + double.Parse(rowStrike[4].ToString()) + double.Parse(rowNotJustified[4].ToString())
                            + double.Parse(rowOvertime[4].ToString()) + double.Parse(rowOvertimeBank[4].ToString()) + double.Parse(rowIllness[4].ToString()) + double.Parse(rowAccidents[4].ToString()) + double.Parse(rowWeddingLeaves[4].ToString())
                            + double.Parse(rowBloodDonation[4].ToString()) + double.Parse(rowTissueDonation[4].ToString());

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
                        if (emplIdsDirect.Length > 0)
                            rowTotal[2] = Misc.CalcPlannedTime(dbConnection, emplIdsDirect, datesList[0], datesList[datesList.Count - 1]);
                        else
                            rowTotal[2] = 0;
                        if (emplIdsIndirect.Length > 0)
                            rowTotal[4] = Misc.CalcPlannedTime(dbConnection, emplIdsIndirect, datesList[0], datesList[datesList.Count - 1]);
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

                        Misc.percentage(rowNew, totalDD, totalID);
                        Misc.percentage(rowNotJustified, totalDD, totalID);
                        Misc.percentage(rowOvertime, totalDD, totalID);
                        Misc.percentage(rowOvertimeBank, totalDD, totalID);
                        Misc.percentage(rowHoliday, totalDD, totalID);
                        Misc.percentage(rowCollectiveHoliday, totalDD, totalID);
                        Misc.percentage(rowReligiousHoliday, totalDD, totalID);
                        Misc.percentage(rowUsedBankH, totalDD, totalID);
                        Misc.percentage(rowAccidents, totalDD, totalID);
                        Misc.percentage(rowIllness, totalDD, totalID);
                        Misc.percentage(rowUnionLeaves, totalDD, totalID);
                        Misc.percentage(rowBloodDonation, totalDD, totalID);
                        Misc.percentage(rowTissueDonation, totalDD, totalID);
                        Misc.percentage(rowPaidLeaves, totalDD, totalID);
                        Misc.percentage(rowWeddingLeaves, totalDD, totalID);
                        Misc.percentage(rowUnpaidLeaves, totalDD, totalID);
                        Misc.percentage(rowStrike, totalDD, totalID);
                        Misc.percentage(rowNoWork, totalDD, totalID);
                        Misc.percentage(rowNoWorkRecoveryH, totalDD, totalID);
                        Misc.percentage(rowProductiveLeaves, totalDD, totalID);

                        rowNew[7] = double.Parse(rowNew[6].ToString()) * 100 / total;
                        rowNotJustified[7] = double.Parse(rowNotJustified[6].ToString()) * 100 / total;
                        rowOvertime[7] = double.Parse(rowOvertime[6].ToString()) * 100 / total;
                        rowOvertimeBank[7] = double.Parse(rowOvertimeBank[6].ToString()) * 100 / total;
                        rowHoliday[7] = double.Parse(rowHoliday[6].ToString()) * 100 / total;
                        rowCollectiveHoliday[7] = double.Parse(rowCollectiveHoliday[6].ToString()) * 100 / total;
                        rowReligiousHoliday[7] = double.Parse(rowReligiousHoliday[6].ToString()) * 100 / total;
                        rowUsedBankH[7] = double.Parse(rowUsedBankH[6].ToString()) * 100 / total;
                        rowAccidents[7] = double.Parse(rowAccidents[6].ToString()) * 100 / total;
                        rowIllness[7] = double.Parse(rowIllness[6].ToString()) * 100 / total;
                        rowUnionLeaves[7] = double.Parse(rowUnionLeaves[6].ToString()) * 100 / total;
                        rowBloodDonation[7] = double.Parse(rowBloodDonation[6].ToString()) * 100 / total;
                        rowTissueDonation[7] = double.Parse(rowTissueDonation[6].ToString()) * 100 / total;
                        rowPaidLeaves[7] = double.Parse(rowPaidLeaves[6].ToString()) * 100 / total;
                        rowWeddingLeaves[7] = double.Parse(rowWeddingLeaves[6].ToString()) * 100 / total;
                        rowUnpaidLeaves[7] = double.Parse(rowUnpaidLeaves[6].ToString()) * 100 / total;
                        rowStrike[7] = double.Parse(rowStrike[6].ToString()) * 100 / total;
                        rowNoWork[7] = double.Parse(rowNoWork[6].ToString()) * 100 / total;
                        rowNoWorkRecoveryH[7] = double.Parse(rowNoWorkRecoveryH[6].ToString()) * 100 / total;
                        rowProductiveLeaves[7] = double.Parse(rowProductiveLeaves[6].ToString()) * 100 / total;
                        row[7] = double.Parse(row[6].ToString()) * 100 / total;
                        rowTotal[7] = 0;

                        if (rowNew[7].ToString() == "NaN") rowNew[7] = 0;
                        if (rowNotJustified[7].ToString() == "NaN") rowNotJustified[7] = 0;
                        if (rowOvertimeBank[7].ToString() == "NaN") rowOvertimeBank[7] = 0;
                        if (rowOvertime[7].ToString() == "NaN") rowOvertime[7] = 0;
                        if (rowHoliday[7].ToString() == "NaN") rowHoliday[7] = 0;
                        if (rowCollectiveHoliday[7].ToString() == "NaN") rowCollectiveHoliday[7] = 0;
                        if (rowReligiousHoliday[7].ToString() == "NaN") rowReligiousHoliday[7] = 0;
                        if (rowUsedBankH[7].ToString() == "NaN") rowUsedBankH[7] = 0;
                        if (rowAccidents[7].ToString() == "NaN") rowAccidents[7] = 0;
                        if (rowIllness[7].ToString() == "NaN") rowIllness[7] = 0;
                        if (rowUnionLeaves[7].ToString() == "NaN") rowUnionLeaves[7] = 0;
                        if (rowBloodDonation[7].ToString() == "NaN") rowBloodDonation[7] = 0;
                        if (rowTissueDonation[7].ToString() == "NaN") rowTissueDonation[7] = 0;
                        if (rowPaidLeaves[7].ToString() == "NaN") rowPaidLeaves[7] = 0;
                        if (rowWeddingLeaves[7].ToString() == "NaN") rowWeddingLeaves[7] = 0;
                        if (rowUnpaidLeaves[7].ToString() == "NaN") rowUnpaidLeaves[7] = 0;
                        if (rowNoWork[7].ToString() == "NaN") rowNoWork[7] = 0;
                        if (rowStrike[7].ToString() == "NaN") rowStrike[7] = 0;
                        if (rowProductiveLeaves[7].ToString() == "NaN") rowProductiveLeaves[7] = 0;
                        if (rowNoWorkRecoveryH[7].ToString() == "NaN") rowNoWorkRecoveryH[7] = 0;
                        if (row[7].ToString() == "NaN") row[7] = 0;


                        double totalDPercent = double.Parse(rowProductiveLeaves[3].ToString()) + double.Parse(rowNew[3].ToString()) + double.Parse(rowNoWorkRecoveryH[3].ToString()) 
                            + double.Parse(rowPaidLeaves[3].ToString()) + double.Parse(rowUnionLeaves[3].ToString()) + double.Parse(rowUsedBankH[3].ToString())
                            + double.Parse(rowHoliday[3].ToString()) + double.Parse(rowCollectiveHoliday[3].ToString()) + double.Parse(rowReligiousHoliday[3].ToString()) 
                            + double.Parse(rowNoWork[3].ToString()) + double.Parse(rowStrike[3].ToString()) + double.Parse(rowNotJustified[3].ToString())
                            + double.Parse(rowOvertime[3].ToString()) + double.Parse(rowOvertimeBank[3].ToString()) + double.Parse(rowIllness[3].ToString()) + double.Parse(rowAccidents[3].ToString()) + double.Parse(rowWeddingLeaves[3].ToString())
                            + double.Parse(rowBloodDonation[3].ToString()) + double.Parse(rowTissueDonation[3].ToString());

                        double totalIPercent = double.Parse(rowProductiveLeaves[5].ToString()) + double.Parse(rowNew[5].ToString()) + double.Parse(rowNoWorkRecoveryH[5].ToString()) 
                            + double.Parse(rowPaidLeaves[5].ToString()) + double.Parse(rowUnionLeaves[5].ToString()) + double.Parse(rowUsedBankH[5].ToString())
                            + double.Parse(rowHoliday[5].ToString()) + double.Parse(rowCollectiveHoliday[5].ToString()) + double.Parse(rowReligiousHoliday[5].ToString()) 
                            + double.Parse(rowNoWork[5].ToString()) + double.Parse(rowStrike[5].ToString()) + double.Parse(rowNotJustified[5].ToString())
                            + double.Parse(rowOvertime[5].ToString()) + double.Parse(rowOvertimeBank[5].ToString()) + double.Parse(rowIllness[5].ToString()) + double.Parse(rowAccidents[5].ToString()) + double.Parse(rowWeddingLeaves[5].ToString())
                            + double.Parse(rowBloodDonation[5].ToString()) + double.Parse(rowTissueDonation[5].ToString());

                        row[3] = totalDPercent;
                        row[5] = totalIPercent;

                        double ts = double.Parse(rowTotal[2].ToString());
                        double tsi = double.Parse(rowTotal[4].ToString());
                        if (ts != 0) rowTotal[3] = 0;
                        if (tsi != 0) rowTotal[5] = 0;

                        Misc.roundOn2(rowNew);
                        Misc.roundOn2(rowNotJustified);
                        Misc.roundOn2(rowOvertime);
                        Misc.roundOn2(rowOvertimeBank);
                        Misc.roundOn2(rowHoliday);
                        Misc.roundOn2(rowCollectiveHoliday);
                        Misc.roundOn2(rowReligiousHoliday);
                        Misc.roundOn2(rowUsedBankH);
                        Misc.roundOn2(rowAccidents);
                        Misc.roundOn2(rowIllness);
                        Misc.roundOn2(rowUnionLeaves);
                        Misc.roundOn2(rowBloodDonation);
                        Misc.roundOn2(rowTissueDonation);
                        Misc.roundOn2(rowPaidLeaves);
                        Misc.roundOn2(rowWeddingLeaves);
                        Misc.roundOn2(rowUnpaidLeaves);
                        Misc.roundOn2(rowStrike);
                        Misc.roundOn2(rowNoWork);
                        Misc.roundOn2(rowNoWorkRecoveryH);
                        Misc.roundOn2(rowProductiveLeaves);
                        Misc.roundOn2(row);
                        Misc.roundOn2(rowTotal);
                        Misc.roundOn2(rowTotalDiff);

                        dt.Rows.Add(rowNew);
                        dt.Rows.Add(rowNotJustified);
                        dt.Rows.Add(rowOvertime);
                        dt.Rows.Add(rowOvertimeBank);
                        dt.Rows.Add(rowHoliday);
                        dt.Rows.Add(rowCollectiveHoliday);
                        dt.Rows.Add(rowReligiousHoliday);
                        dt.Rows.Add(rowUsedBankH);
                        dt.Rows.Add(rowAccidents);
                        dt.Rows.Add(rowIllness);
                        dt.Rows.Add(rowUnionLeaves);
                        dt.Rows.Add(rowBloodDonation);
                        dt.Rows.Add(rowTissueDonation);
                        dt.Rows.Add(rowPaidLeaves);
                        dt.Rows.Add(rowWeddingLeaves);
                        dt.Rows.Add(rowUnpaidLeaves);
                        dt.Rows.Add(rowStrike);
                        dt.Rows.Add(rowNoWork);
                        dt.Rows.Add(rowNoWorkRecoveryH);
                        dt.Rows.Add(rowProductiveLeaves);
                        DataRow rowEmptyBottom = dt.NewRow();
                        dt.Rows.Add(rowEmptyBottom);
                        dt.Rows.Add(row);
                        dt.Rows.Add(rowTotal);
                        dt.Rows.Add(rowTotalDiff);
                        dt.AcceptChanges();
                    }
                    else
                        isEmpty = true;
                }
                return isEmpty;
            }
            catch (Exception ex)
            {
                throw ex;
                //debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + ".populateTable500() " + ex.Message + " for wu: " + workingUnit.WorkingUnitID);
                //return true;
            }
        }

        private void calc500(List<IOPairProcessedTO> IOPairListDirect, List<IOPairProcessedTO> IOPairListIndirect, Dictionary<int, PassTypeTO> listPassT, string columnName, DataRow row,
                string emplIdsBS, string emplIds, List<DateTime> datesList)
        {
            try
            {
                row[1] = columnName;


                TimeSpan totalDurationDirect = new TimeSpan();
                TimeSpan totalDurationIndirect = new TimeSpan();
                foreach (IOPairProcessedTO iopair in IOPairListDirect)
                {
                    if (listPassT.ContainsKey(iopair.PassTypeID))
                    {
                        TimeSpan duration = iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay);
                        if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                            duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift
                        totalDurationDirect = totalDurationDirect.Add(duration);
                    }

                }
                foreach (IOPairProcessedTO iopair in IOPairListIndirect)
                {
                    if (listPassT.ContainsKey(iopair.PassTypeID))
                    {
                        TimeSpan duration = iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay);
                        if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                            duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift
                        totalDurationIndirect = totalDurationIndirect.Add(duration);
                    }
                }
                row[2] = totalDurationDirect.TotalHours;
                row[4] = totalDurationIndirect.TotalHours;
                row[6] = (totalDurationDirect + totalDurationIndirect).TotalHours;

            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + ".calc500() " + ex.Message);
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

        private void calc400New(string columnName, DataRow row, string emplIdsBC, string emplIds, string payment_code, uint py_calc_id)
        {
            try
            {
                List<EmployeePYDataAnaliticalTO> listPYAnaliticalDirect = new List<EmployeePYDataAnaliticalTO>();
                List<EmployeePYDataAnaliticalTO> listPYAnaliticalIndirect = new List<EmployeePYDataAnaliticalTO>();
                row[1] = columnName;
                if (emplIds.Length > 0)
                    listPYAnaliticalIndirect = new EmployeePYDataAnalitical().Search(emplIds, payment_code, py_calc_id, Constants.PYTypeEstimated);
                if (emplIdsBC.Length > 0)
                    listPYAnaliticalDirect = new EmployeePYDataAnalitical().Search(emplIdsBC, payment_code, py_calc_id, Constants.PYTypeEstimated);

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
    }
}
