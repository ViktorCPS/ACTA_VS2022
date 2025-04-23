using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using TransferObjects;
using System.Data;

namespace ACTAWorkAnalysisReports
{
  public static  class Misc
    {
        public static void roundOn2(DataRow row)
        {
            try
            {
                row[0] = " ";
                for (int i = 1; i < row.ItemArray.Length; i++)
                {
                    double num = 0;
                    if (double.TryParse(row[i].ToString(), out num))
                    {
                        row[i] = Math.Round(num, 2);
                        if (row[i].ToString().Contains('.'))
                        {
                            if (row[i].ToString().Substring(row[i].ToString().IndexOf('.') + 1).Length == 1)
                                row[i] = row[i].ToString() + "0";
                        }
                        else
                        {
                            row[i] = row[i].ToString() + ".00";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void percentage(DataRow row, double totalDD, double totalID)
        {
            try
            {
                double ts = double.Parse(row[2].ToString());
                double tsi = double.Parse(row[4].ToString());
                if (ts != 0) row[3] = ts * 100 / totalDD;
                else
                    row[3] = 0;
                if (tsi != 0) row[5] = tsi * 100 / totalID;
                else
                    row[5] = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Dictionary<string, string> emplBranch(Object dbConnection, string listEmpl, int company)
        {
            string empl = "";
            //// get selected company
            Dictionary<string, string> dictEmplTypes = new Dictionary<string, string>();
            foreach (string employee in listEmpl.Split(','))
            {
                EmployeeAsco4 emplAsco;
                if (dbConnection == null)
                    emplAsco = new EmployeeAsco4();
                else
                    emplAsco = new EmployeeAsco4(dbConnection);

                emplAsco.EmplAsco4TO.EmployeeID = int.Parse(employee);
                List<EmployeeAsco4TO> emplAscoList = emplAsco.Search();


                if (emplAscoList.Count == 1)
                {
                    empl = emplAscoList[0].NVarcharValue6;
                }
                if (!dictEmplTypes.ContainsKey(employee))
                    dictEmplTypes.Add(employee, empl);
            }

            return dictEmplTypes;
        }
        public static Dictionary<string, string> emplTypes(Object dbConnection, List<EmployeeTO> listEmpl, int company)
        {
            string empl = "";
            //// get selected company
            Dictionary<string, string> dictEmplTypes = new Dictionary<string, string>();
            foreach (EmployeeTO employee in listEmpl)
            {
                EmployeeType emplType;
                if (dbConnection == null)
                    emplType = new EmployeeType();
                else
                    emplType = new EmployeeType(dbConnection);

                List<EmployeeTypeTO> listEmplTypes = new List<EmployeeTypeTO>();
                listEmplTypes = emplType.Search();

                foreach (EmployeeTypeTO emplTypeTO in listEmplTypes)
                {
                    if (emplTypeTO.EmployeeTypeID == employee.EmployeeTypeID && emplTypeTO.WorkingUnitID == company)
                    {
                        empl = emplTypeTO.EmployeeTypeName;
                        break;
                    }
                }
                if (!dictEmplTypes.ContainsKey(employee.EmployeeID.ToString()))
                    dictEmplTypes.Add(employee.EmployeeID.ToString(), empl);
            }

            return dictEmplTypes;
        }
        public static double CalcPlannedTime(Object dbConnection, string emplids, DateTime fromDate, DateTime toDate)
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
                    List<EmployeeTimeScheduleTO> timeScheduleList = EmplTimeS.SearchEmployeesSchedules(empl.ToString(), fromDate, toDate);
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

                    for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
                    {
                        if (timeScheduleList.Count > 0)
                        {
                            intervalsEmpl = getTimeSchemaInterval(int.Parse(empl), date, timeScheduleList, timeSchema);//geting time intervals list for specified employee and date

                            foreach (WorkTimeIntervalTO tsInterval in intervalsEmpl)
                            {
                                TimeSpan duration = (new DateTime(tsInterval.EndTime.TimeOfDay.Ticks - tsInterval.StartTime.TimeOfDay.Ticks)).TimeOfDay;
                                plannedTime += (double)duration.Hours;
                            }
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
        public static List<WorkTimeIntervalTO> getTimeSchemaInterval(int employeeID, DateTime date, List<EmployeeTimeScheduleTO> timeScheduleListForEmployee, List<WorkTimeSchemaTO> timeSchema)
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
        public static Dictionary<int, WorkingUnitTO> getWUnits()
        {
            try
            {
                Dictionary<int, WorkingUnitTO> wUnits = new Dictionary<int, WorkingUnitTO>();
                List<WorkingUnitTO> wuList = new WorkingUnit().Search();

                foreach (WorkingUnitTO wu in wuList)
                {
                    if (!wUnits.ContainsKey(wu.WorkingUnitID))
                        wUnits.Add(wu.WorkingUnitID, wu);
                    else
                        wUnits[wu.WorkingUnitID] = wu;
                }

                return wUnits;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
