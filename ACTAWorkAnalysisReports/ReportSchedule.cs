using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransferObjects;
using Common;
using Util;
using System.IO;
using System.Data;

namespace ACTAWorkAnalysisReports
{
    public class ReportSchedule
    {
        private const string delimiter = ";";
        public void GenerateReport(int company, string filePath)
        {
            try
            {
                DataSet ds = new DataSet("Test");

                System.Data.DataTable dtCompany = new System.Data.DataTable(company.ToString());
                dtCompany.Columns.Add("Country ID", typeof(string));
                dtCompany.Columns.Add("Payroll ID", typeof(string));
                dtCompany.Columns.Add("Plant", typeof(string));
                dtCompany.Columns.Add("Unit", typeof(string));
                dtCompany.Columns.Add("Workshop", typeof(string));
                dtCompany.Columns.Add("Ute", typeof(string));
                dtCompany.Columns.Add("Shift code", typeof(string));
                dtCompany.Columns.Add("Position ID", typeof(string));
                dtCompany.Columns.Add("Position name", typeof(string));
                dtCompany.Columns.Add("Working hours of month", typeof(string));
                dtCompany.Columns.Add("Year-Month", typeof(string));

                ds.Tables.Add(dtCompany);
                ds.AcceptChanges();
                string workUnitID = company.ToString();
                List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();

                if (company != -1)
                {
                    List<WorkingUnitTO> plantWU = new WorkingUnit().SearchChildWU(company.ToString());
                    wUnits.AddRange(plantWU);
                    foreach (WorkingUnitTO plant in plantWU)
                    {
                        List<WorkingUnitTO> ccWU = new WorkingUnit().SearchChildWU(plant.WorkingUnitID.ToString());
                        wUnits.AddRange(ccWU);
                        foreach (WorkingUnitTO cc in ccWU)
                        {
                            List<WorkingUnitTO> workGroupWU = new WorkingUnit().SearchChildWU(cc.WorkingUnitID.ToString());
                            wUnits.AddRange(workGroupWU);
                            foreach (WorkingUnitTO workGroup in workGroupWU)
                            {
                                List<WorkingUnitTO> uteWU = new WorkingUnit().SearchChildWU(workGroup.WorkingUnitID.ToString());
                                wUnits.AddRange(uteWU);
                            }
                        }
                    }
                }
                string wUnitIds = "";
                foreach (WorkingUnitTO wu in wUnits)
                {
                    wUnitIds += wu.WorkingUnitID + ",";

                }
                if (wUnitIds.Length > 0)
                    wUnitIds = wUnitIds.Substring(0, wUnitIds.Length - 1);

                List<EmployeeTO> currentEmployeesList = new Employee().SearchByWULoans(wUnitIds, -1, null, DateTime.Now.Date, DateTime.Now.Date);
                string emplIDs = "";
                foreach (EmployeeTO empl in currentEmployeesList)
                {
                    emplIDs += empl.EmployeeID.ToString().Trim() + ",";
                }

                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, DateTime.Now.Date, DateTime.Now.Date, null);
                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary(emplIDs);
                Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema().getDictionary();

                Dictionary<int, EmployeePositionTO> employeePostions = new EmployeePosition().SearchEmployeePositionsDictionary();

                Dictionary<int, List<TimeSchemaIntervalLibraryTO>> timeSchemaLibrary = new TimeSchemaIntervalLibrary().GetTimeSchemaIntervalLibraryDictionary();
                Dictionary<int, List<TimeSchemaIntervalLibraryDtlTO>> timeSchemaLibraryDtl = new TimeSchemaIntervalLibraryDtl().GetTimeSchemaIntervalLibraryDtlDictionary();
                DateTime fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                fromDate = fromDate.AddMonths(-1);
                DateTime toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                toDate = toDate.AddDays(-1);
                Dictionary<int, int> workingHourOfMonth = Common.Misc.GetEmployeeFundHrs(emplIDs, fromDate, toDate);

                DataRow rowHeader = dtCompany.NewRow();
                rowHeader[0] = "Country ID";
                rowHeader[1] = "Payroll ID";
                rowHeader[2] = "Plant";
                rowHeader[3] = "Unit";
                rowHeader[4] = "Workshop";
                rowHeader[5] = "Ute";
                rowHeader[6] = "Shift code";
                rowHeader[7] = "Position ID";
                rowHeader[8] = "Position name";
                rowHeader[9] = "Working hour of month N-1";
                rowHeader[10] = "Year-Month N-1";
                dtCompany.Rows.Add(rowHeader);
                dtCompany.AcceptChanges();

                string header = "";
                for (int i = 0; i < rowHeader.ItemArray.Length; i++)
                {
                    header += rowHeader[i].ToString().Trim().Replace(delimiter, " ") + delimiter;
                }

                List<string> reportData = new List<string>();

                //  string report = "\tID\t\tUnit\tWorkshop\tShift code\tPosition ID\tPosition Name\tWorking hour of month N-1\tYear-Month N-1" + Environment.NewLine;
                foreach (EmployeeTO empl in currentEmployeesList)
                {
                    DataRow row = dtCompany.NewRow();
                    string rowData = "";
                    if (ascoDict.ContainsKey(empl.EmployeeID))
                    {
                        row[0] = ascoDict[empl.EmployeeID].NVarcharValue4.Trim();
                    }
                    else
                    {
                        row[0] = "N/A";
                    }

                    rowData += row[0].ToString().Trim().Replace(delimiter, " ") + delimiter;

                    row[1] = empl.EmployeeID.ToString().Trim();
                    rowData += row[1].ToString().Trim().Replace(delimiter, " ") + delimiter;

                    WorkingUnitTO ute = new WorkingUnit().FindWU(empl.WorkingUnitID);
                    string stringone = ute.Name;
                    row[2] = stringone.Substring(0, 3);
                    row[3] = stringone.Substring(4, 4);
                    row[4] = stringone.Substring(9, 2);
                    row[5] = stringone.Substring(12, 2);

                    rowData += row[2].ToString().Trim().Replace(delimiter, " ") + delimiter;
                    rowData += row[3].ToString().Trim().Replace(delimiter, " ") + delimiter;
                    rowData += row[4].ToString().Trim().Replace(delimiter, " ") + delimiter;
                    rowData += row[5].ToString().Trim().Replace(delimiter, " ") + delimiter;

                    // get shift
                    List<EmployeeTimeScheduleTO> schedules = new List<EmployeeTimeScheduleTO>();
                    if (emplSchedules.ContainsKey(empl.EmployeeID))
                        schedules = emplSchedules[empl.EmployeeID];

                    WorkTimeSchemaTO sch = Common.Misc.getTimeSchema(DateTime.Now.Date, schedules, schemas);
                    List<TimeSchemaIntervalLibraryTO> listTimeSchemaLibrary = new List<TimeSchemaIntervalLibraryTO>();
                    if (sch.TimeSchemaID != -1)
                    {
                        if (timeSchemaLibrary.ContainsKey(sch.TimeSchemaID))
                            listTimeSchemaLibrary = timeSchemaLibrary[sch.TimeSchemaID];

                        List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(DateTime.Now.Date, schedules, schemas);

                        string time_schema_code = "";
                        string intervalstring = "";

                        foreach (WorkTimeIntervalTO interval in intervals)
                        {
                            intervalstring += interval.StartTime.ToString("HH:mm:ss") + "-" + interval.EndTime.ToString("HH:mm:ss") + ";";
                        }

                        if (intervalstring.Length > 0)
                            intervalstring = intervalstring.Substring(0, intervalstring.Length - 1);

                        foreach (TimeSchemaIntervalLibraryTO tsch in listTimeSchemaLibrary)
                        {
                            List<TimeSchemaIntervalLibraryDtlTO> timeSchemaLibraryIntervalDtl = new List<TimeSchemaIntervalLibraryDtlTO>();

                            if (timeSchemaLibraryDtl.ContainsKey(tsch.TimeSchemaIntervalId))
                                timeSchemaLibraryIntervalDtl = timeSchemaLibraryDtl[tsch.TimeSchemaIntervalId];

                            string interv = "";
                            foreach (TimeSchemaIntervalLibraryDtlTO dtl in timeSchemaLibraryIntervalDtl)
                            {
                                interv += dtl.StartTime.ToString("HH:mm:ss") + "-" + dtl.EndTime.ToString("HH:mm:ss") + ";";
                            }

                            if (interv.Length > 0)
                                interv = interv.Substring(0, interv.Length - 1);

                            string[] niz = interv.Split(';');
                            string[] niz1 = intervalstring.Split(';');
                            int ind = 0;
                            for (int i = 0; i < niz1.Length; i++)
                            {
                                for (int j = 0; j < niz.Length; j++)
                                {
                                    if (niz1[i].Equals(niz[j]))
                                        ind++;
                                }
                            }
                            if (ind == niz1.Length)
                            {
                                // if (intervalstring.Equals(interv))
                                time_schema_code = tsch.TimeSchemaIntervalId.ToString();
                                break;
                            }
                        }

                        if (time_schema_code.Equals(""))
                        {
                            row[6] = "N/A";
                        }
                        else
                        {
                            row[6] = time_schema_code;
                        }

                        rowData += row[6].ToString().Trim().Replace(delimiter, " ") + delimiter;
                    }

                    string postionName = "";
                    string positionId = "";
                    if (ascoDict.ContainsKey(empl.EmployeeID))
                    {
                        if (employeePostions.ContainsKey(ascoDict[empl.EmployeeID].IntegerValue6))
                        {
                            positionId = employeePostions[ascoDict[empl.EmployeeID].IntegerValue6].PositionCode.ToString();
                            postionName = employeePostions[ascoDict[empl.EmployeeID].IntegerValue6].PositionTitleEN;
                        }
                    }
                    if (positionId.Equals(""))
                    {
                        row[7] = "N/A";
                    }
                    else
                    {
                        row[7] = positionId;
                    }

                    rowData += row[7].ToString().Trim().Replace(delimiter, " ") + delimiter;

                    if (postionName.Equals(""))
                    {
                        row[8] = "N/A";
                    }
                    else
                    {
                        row[8] = postionName;
                    }

                    rowData += row[8].ToString().Trim().Replace(delimiter, " ") + delimiter;

                    if (workingHourOfMonth.ContainsKey(empl.EmployeeID))
                        row[9] = workingHourOfMonth[empl.EmployeeID].ToString();
                    else row[9] = "N/A";

                    rowData += row[9].ToString().Trim().Replace(delimiter, " ") + delimiter;

                    row[10] = DateTime.Now.AddMonths(-1).ToString("yyyy") + "-" + DateTime.Now.AddMonths(-1).ToString("MM");

                    rowData += row[10].ToString().Trim().Replace(delimiter, " ") + delimiter;

                    dtCompany.Rows.Add(row);
                    dtCompany.AcceptChanges();

                    reportData.Add(rowData);
                }

                string Path = Directory.GetParent(filePath).FullName;
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }

                if (File.Exists(filePath))
                    File.Delete(filePath);

                //ExportToExcel.CreateExcelDocument(ds, filePath, false, true);
                CreateReportTXT(header, reportData, filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateReportTXT(string header, List<string> reportData, string filePath)
        {
            try
            {
                //filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\test.txt";
                FileStream stream = new FileStream(filePath, FileMode.Create);
                stream.Dispose();
                stream.Close();

                StreamWriter writer = new StreamWriter(filePath, true, Encoding.Unicode);
                // insert header
                writer.WriteLine(header);

                foreach (string line in reportData)
                {
                    writer.WriteLine(line);
                }

                writer.Dispose();
                writer.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
