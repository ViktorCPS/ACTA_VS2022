using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

using Util;
using Common;
using TransferObjects;

namespace ACTAWorkAnalysisReports
{
    public class BadgeSystemInterface
    {
        DebugLog debug;
        private const string delimiter = ";";

        public BadgeSystemInterface()
        {
            debug = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");
        }

        public void GenerateReportService(Object conn, string path, List<int> companyList, DateTime day)
        {
            debug.writeLog("+ Started BadgeSystemInterface! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            
            try
            {
                // declare objects
                Employee empl = null;
                EmployeeAsco4 asco = null;
                IOPairProcessed pair = null;
                EmployeesTimeSchedule emplSchedule = null;
                WorkingUnit wu = null;
                OrganizationalUnit ou = null;
                PassType pt = null;
                TimeSchema schema = null;
                EmployeeResponsibility res = null;
                Tag tag = null;
                Common.Rule rule = null;

                if (conn != null)
                {
                    empl = new Employee(conn);
                    asco = new EmployeeAsco4(conn);
                    pair = new IOPairProcessed(conn);
                    emplSchedule = new EmployeesTimeSchedule(conn);
                    wu = new WorkingUnit(conn);
                    ou = new OrganizationalUnit(conn);
                    pt = new PassType(conn);
                    schema = new TimeSchema(conn);
                    res = new EmployeeResponsibility(conn);
                    tag = new Tag(conn);
                    rule = new Common.Rule(conn);
                }
                else
                {
                    empl = new Employee();
                    asco = new EmployeeAsco4();
                    pair = new IOPairProcessed();
                    emplSchedule = new EmployeesTimeSchedule();
                    wu = new WorkingUnit();
                    ou = new OrganizationalUnit();
                    pt = new PassType();
                    schema = new TimeSchema();
                    res = new EmployeeResponsibility();
                    tag = new Tag();
                    rule = new Common.Rule();
                }
                
                List<int> holiday = rule.SearchRulesExact(Constants.RuleHolidayPassType);
                List<int> personalHoliday = rule.SearchRulesExact(Constants.RulePersonalHolidayPassType);
                List<int> unjustifiedAbsence = rule.SearchRulesExact(Constants.RuleCompanyNotJustifiedAbsence);
                List<int> bhUsed = rule.SearchRulesExact(Constants.RuleCompanyBankHourUsed);
                List<int> overtimeUsed = rule.SearchRulesExact(Constants.RuleCompanyInitialOvertimeUsed);
                List<int> stopWorking = rule.SearchRulesExact(Constants.RuleCompanyStopWorking);
                List<int> strike = rule.SearchRulesExact(Constants.RuleCompanyStrike);                

                // get work locations
                Dictionary<string, string> workLocations = Constants.FiatWorkLocations();

                // get data dictionaries
                Dictionary<int, WorkingUnitTO> wuDict = wu.getWUDictionary();
                Dictionary<int, OrganizationalUnitTO> ouDict = ou.SearchDictionary();
                Dictionary<int, PassTypeTO> ptDict = pt.SearchDictionary();
                Dictionary<int, WorkTimeSchemaTO> schDict = schema.getDictionary();

                // get responsibilities, supervisors and ou responsible employees
                List<EmployeeResponsibilityTO> resList = res.Search();

                List<int> supervisorsList = new List<int>();
                Dictionary<int, List<int>> ouResponsible = new Dictionary<int, List<int>>();

                // Sanja 27.04.2014 - supervisors has stringone for which they are responsible in files, not stringone they belong to - TEST START
                // supervisors stringones
                Dictionary<int, List<string>> emplStringoneList = new Dictionary<int, List<string>>();
                // Sanja 27.04.2014 - supervisors has stringone for which they are responsible in files, not stringone they belong to - TEST END

                string resIDs = "";
                foreach (EmployeeResponsibilityTO resTO in resList)
                {
                    if (resTO.Type == Constants.emplResTypeWU)
                    {
                        if (!supervisorsList.Contains(resTO.EmployeeID))
                            supervisorsList.Add(resTO.EmployeeID);

                        // Sanja 27.04.2014 - supervisors has stringone for which they are responsible in files, not stringone they belong to - TEST START
                        if (!emplStringoneList.ContainsKey(resTO.EmployeeID))
                            emplStringoneList.Add(resTO.EmployeeID, new List<string>());
                        
                        emplStringoneList[resTO.EmployeeID].Add(getWUStringone(resTO.UnitID, wuDict));
                        // Sanja 27.04.2014 - supervisors has stringone for which they are responsible in files, not stringone they belong to - TEST END
                    }

                    if (resTO.Type == Constants.emplResTypeOU)
                    {
                        if (!ouResponsible.ContainsKey(resTO.UnitID))
                            ouResponsible.Add(resTO.UnitID, new List<int>());

                        if (!ouResponsible[resTO.UnitID].Contains(resTO.EmployeeID))
                            ouResponsible[resTO.UnitID].Add(resTO.EmployeeID);

                        resIDs += resTO.EmployeeID.ToString().Trim() + ",";
                    }
                }

                // get all wu
                List<int> allowedWU = new List<int>();
                foreach (int id in wuDict.Keys)
                {
                    allowedWU.Add(id);
                }

                // get employees
                List<EmployeeTO> emplList = new List<EmployeeTO>();
                foreach (int company in companyList)
                {
                    emplList.AddRange(empl.SearchByWULoans(Common.Misc.getWorkingUnitHierarhicly(company, allowedWU, conn), -1, null, day.Date, day.Date));
                }

                string emplIDs = "";
                foreach (EmployeeTO emplTO in emplList)
                {
                    emplIDs += emplTO.EmployeeID.ToString().Trim() + ",";
                    resIDs += emplTO.EmployeeID.ToString().Trim() + ",";
                }
                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
                //Dictionary<int, List<TagTO>> tagDict = new Dictionary<int, List<TagTO>>();
                if (resIDs.Length > 0)
                {
                    resIDs = resIDs.Substring(0, resIDs.Length - 1);
                    emplDict = empl.SearchDictionary(resIDs);
                    //tagDict = tag.SearchTagForEmployees();
                }

                // get asco data
                Dictionary<int, EmployeeAsco4TO> ascoDict = asco.SearchDictionary("");

                // get all schedules
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = emplSchedule.SearchEmployeesSchedulesExactDate(emplIDs, day.Date, day.AddDays(1).Date, null);

                // get all pairs
                List<DateTime> dateList = new List<DateTime>();
                dateList.Add(day.Date);
                dateList.Add(day.AddDays(1).Date);
                List<IOPairProcessedTO> allPairs = pair.SearchAllPairsForEmpl(emplIDs, dateList, "");

                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>> emplDaySchemas = new Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>>();
                Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>> emplDayIntervals = new Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>>();

                // UTE report lines
                Dictionary<int, List<string>> emplUTELines = new Dictionary<int, List<string>>();
                // RESPONSIBLE report lines
                Dictionary<int, List<string>> emplRESLines = new Dictionary<int, List<string>>();

                // get intervals and schemas by employees and dates
                foreach (EmployeeTO emplTO in emplList)
                {
                    if (!emplUTELines.ContainsKey(emplTO.EmployeeID))
                        emplUTELines.Add(emplTO.EmployeeID, new List<string>());

                    if (!emplRESLines.ContainsKey(emplTO.EmployeeID))
                        emplRESLines.Add(emplTO.EmployeeID, new List<string>());
                    
                    // create responsibility file line
                    //string tagID = "";
                    //if (tagDict.ContainsKey(emplTO.EmployeeID))
                    //{
                    //    foreach (TagTO emplTag in tagDict[emplTO.EmployeeID])
                    //    {
                    //        if (emplTag.Status == Constants.statusActive)
                    //        {
                    //            tagID = emplTag.TagID.ToString().Trim();
                    //            break;
                    //        }
                    //    }
                    //}
                    string JMBG = "";
                    if (ascoDict.ContainsKey(emplTO.EmployeeID))
                        JMBG = ascoDict[emplTO.EmployeeID].NVarcharValue4.Trim();
                    emplRESLines[emplTO.EmployeeID].Add(JMBG.Trim());
                    emplRESLines[emplTO.EmployeeID].Add(emplTO.FirstAndLastName.Trim());

                    int resPersonID = -1;
                    bool selfResponsible = false;
                    if (ouResponsible.ContainsKey(emplTO.OrgUnitID))
                    {
                        foreach (int id in ouResponsible[emplTO.OrgUnitID])
                        {
                            if (id != emplTO.EmployeeID)
                                resPersonID = id;
                            else
                                selfResponsible = true;                            
                        }
                    }

                    if (selfResponsible)
                    {
                        if (ouDict.ContainsKey(emplTO.OrgUnitID) && emplTO.OrgUnitID != ouDict[emplTO.OrgUnitID].ParentOrgUnitID)
                            resPersonID = -1;
                        else if (resPersonID == -1)
                            resPersonID = emplTO.EmployeeID;
                    }

                    if (resPersonID == -1 && ouDict.ContainsKey(emplTO.OrgUnitID) && ouResponsible.ContainsKey(ouDict[emplTO.OrgUnitID].ParentOrgUnitID))
                    {
                        foreach (int id in ouResponsible[ouDict[emplTO.OrgUnitID].ParentOrgUnitID])
                        {
                            if (id != emplTO.EmployeeID)
                            {
                                resPersonID = id;
                                break;
                            }
                        }
                    }

                    //string resTagID = "";
                    string resName = "";
                    string resJMBG = "";
                    if (resPersonID != -1)
                    {
                        //if (tagDict.ContainsKey(resPersonID))
                        //{
                        //    foreach (TagTO emplTag in tagDict[resPersonID])
                        //    {
                        //        if (emplTag.Status == Constants.statusActive)
                        //        {
                        //            resTagID = emplTag.TagID.ToString().Trim();
                        //            break;
                        //        }
                        //    }
                        //}

                        if (ascoDict.ContainsKey(resPersonID))
                            resJMBG = ascoDict[resPersonID].NVarcharValue4.Trim();

                        if (emplDict.ContainsKey(resPersonID))
                            resName = emplDict[resPersonID].FirstAndLastName.Trim();
                    }

                    emplRESLines[emplTO.EmployeeID].Add(resJMBG.Trim());
                    emplRESLines[emplTO.EmployeeID].Add(resName.Trim());

                    // create UTE file line
                    // date
                    emplUTELines[emplTO.EmployeeID].Add(day.Date.ToString("yyyyMMdd"));
                    
                    // work location
                    if (ascoDict.ContainsKey(emplTO.EmployeeID) && ascoDict[emplTO.EmployeeID].NVarcharValue7.Trim() != "")
                    {
                        emplUTELines[emplTO.EmployeeID].Add(ascoDict[emplTO.EmployeeID].NVarcharValue7.Trim());

                        if (workLocations.ContainsKey(ascoDict[emplTO.EmployeeID].NVarcharValue7.Trim()))
                            emplUTELines[emplTO.EmployeeID].Add(workLocations[ascoDict[emplTO.EmployeeID].NVarcharValue7.Trim()].Trim());
                        else
                            emplUTELines[emplTO.EmployeeID].Add("N/A");
                    }
                    else
                    {
                        emplUTELines[emplTO.EmployeeID].Add("N/A");
                        emplUTELines[emplTO.EmployeeID].Add("N/A");
                    }

                    // company
                    if (ascoDict.ContainsKey(emplTO.EmployeeID) && wuDict.ContainsKey(ascoDict[emplTO.EmployeeID].IntegerValue4))
                    {
                        emplUTELines[emplTO.EmployeeID].Add(wuDict[ascoDict[emplTO.EmployeeID].IntegerValue4].Code.Trim());
                        emplUTELines[emplTO.EmployeeID].Add(wuDict[ascoDict[emplTO.EmployeeID].IntegerValue4].Description.Trim());
                    }
                    else
                    {
                        emplUTELines[emplTO.EmployeeID].Add("N/A");
                        emplUTELines[emplTO.EmployeeID].Add("N/A");
                    }

                    // employee name
                    emplUTELines[emplTO.EmployeeID].Add(emplTO.LastName.Trim());
                    emplUTELines[emplTO.EmployeeID].Add(emplTO.FirstName.Trim());
                    
                    // date of birth
                    if (ascoDict.ContainsKey(emplTO.EmployeeID) && ascoDict[emplTO.EmployeeID].DatetimeValue5 != new DateTime())
                        emplUTELines[emplTO.EmployeeID].Add(ascoDict[emplTO.EmployeeID].DatetimeValue5.ToString("yyyyMMdd"));
                    else
                        emplUTELines[emplTO.EmployeeID].Add("");

                    // JMBG
                    if (ascoDict.ContainsKey(emplTO.EmployeeID) && ascoDict[emplTO.EmployeeID].NVarcharValue4.Trim() != "")
                        emplUTELines[emplTO.EmployeeID].Add(ascoDict[emplTO.EmployeeID].NVarcharValue4.Trim());
                    else
                        emplUTELines[emplTO.EmployeeID].Add("N/A");
                    
                    DateTime currDate = day.Date;
                    List<EmployeeTimeScheduleTO> emplScheduleList = new List<EmployeeTimeScheduleTO>();

                    if (emplSchedules.ContainsKey(emplTO.EmployeeID))
                        emplScheduleList = emplSchedules[emplTO.EmployeeID];

                    int passAllowed = Constants.yesInt;
                    bool isFlexyShift = false;
                    List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();
                    while (currDate <= day.AddDays(1).Date)
                    {
                        if (!emplDayIntervals.ContainsKey(emplTO.EmployeeID))
                            emplDayIntervals.Add(emplTO.EmployeeID, new Dictionary<DateTime, List<WorkTimeIntervalTO>>());

                        if (!emplDayIntervals[emplTO.EmployeeID].ContainsKey(currDate.Date))
                            emplDayIntervals[emplTO.EmployeeID].Add(currDate.Date, Common.Misc.getTimeSchemaInterval(currDate.Date, emplScheduleList, schDict));

                        if (!emplDaySchemas.ContainsKey(emplTO.EmployeeID))
                            emplDaySchemas.Add(emplTO.EmployeeID, new Dictionary<DateTime, WorkTimeSchemaTO>());

                        WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                        if (emplDayIntervals[emplTO.EmployeeID][currDate.Date].Count > 0 && schDict.ContainsKey(emplDayIntervals[emplTO.EmployeeID][currDate.Date][0].TimeSchemaID))
                            sch = schDict[emplDayIntervals[emplTO.EmployeeID][currDate.Date][0].TimeSchemaID];

                        if (!emplDaySchemas[emplTO.EmployeeID].ContainsKey(currDate.Date))
                            emplDaySchemas[emplTO.EmployeeID].Add(currDate.Date, sch);

                        if (currDate.Date == day.Date)
                        {
                            foreach (WorkTimeIntervalTO interval in emplDayIntervals[emplTO.EmployeeID][currDate.Date])
                            {
                                if (interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0) && interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes > 0)
                                {
                                    passAllowed = Constants.noInt;
                                    break;
                                }
                            }

                            dayIntervals = emplDayIntervals[emplTO.EmployeeID][currDate.Date];

                            if (sch.Type == Constants.schemaTypeFlexi)
                                isFlexyShift = true;
                        }

                        currDate = currDate.AddDays(1).Date;
                    }

                    // pass allowed
                    emplUTELines[emplTO.EmployeeID].Add(passAllowed.ToString().Trim());

                    // supervisor flag
                    if (supervisorsList.Contains(emplTO.EmployeeID))
                        emplUTELines[emplTO.EmployeeID].Add(Constants.yesInt.ToString().Trim());
                    else
                        emplUTELines[emplTO.EmployeeID].Add(Constants.noInt.ToString().Trim());
                    
                    // Sanja 27.04.2014 - supervisors has stringone for which they are responsible in files, not stringone they belong to - TEST START
                    if (!emplStringoneList.ContainsKey(emplTO.EmployeeID))
                        emplStringoneList.Add(emplTO.EmployeeID, new List<string>());
                    
                    if (!supervisorsList.Contains(emplTO.EmployeeID))
                        emplStringoneList[emplTO.EmployeeID].Add(getWUStringone(emplTO.WorkingUnitID, wuDict));

                    emplUTELines[emplTO.EmployeeID].Add("STRINGONE");
                    // file stringone                    
                    //WorkingUnitTO ute = new WorkingUnitTO();
                    //WorkingUnitTO workshop = new WorkingUnitTO();
                    //WorkingUnitTO costCenter = new WorkingUnitTO();
                    //WorkingUnitTO plant = new WorkingUnitTO();

                    //if (wuDict.ContainsKey(emplTO.WorkingUnitID))
                    //    ute = wuDict[emplTO.WorkingUnitID];

                    //if (wuDict.ContainsKey(ute.ParentWorkingUID))
                    //    workshop = wuDict[ute.ParentWorkingUID];

                    //if (wuDict.ContainsKey(workshop.ParentWorkingUID))
                    //    costCenter = wuDict[workshop.ParentWorkingUID];

                    //if (wuDict.ContainsKey(costCenter.ParentWorkingUID))
                    //    plant = wuDict[costCenter.ParentWorkingUID];

                    //emplUTELines[emplTO.EmployeeID].Add(getUTECode(plant, 3) + getUTECode(costCenter, 4) + "000000" + getUTECode(workshop, 2) + getUTECode(ute, 2));

                    // Sanja 27.04.2014 - supervisors has stringone for which they are responsible in files, not stringone they belong to - TEST END

                    // shift code                    
                    emplUTELines[emplTO.EmployeeID].Add(getShiftCode(dayIntervals, isFlexyShift));
                    
                    // absence code
                    emplUTELines[emplTO.EmployeeID].Add("");
                }

                // get day pairs for employees
                foreach (IOPairProcessedTO dayPair in allPairs)
                {
                    if (!emplDayPairs.ContainsKey(dayPair.EmployeeID))
                        emplDayPairs.Add(dayPair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                    if (!emplDayPairs[dayPair.EmployeeID].ContainsKey(dayPair.IOPairDate.Date))
                        emplDayPairs[dayPair.EmployeeID].Add(dayPair.IOPairDate.Date, new List<IOPairProcessedTO>());

                    emplDayPairs[dayPair.EmployeeID][dayPair.IOPairDate.Date].Add(dayPair);
                }

                Dictionary<int, List<IOPairProcessedTO>> emplBelongingDayPairs = new Dictionary<int, List<IOPairProcessedTO>>();
                foreach (IOPairProcessedTO pairTO in allPairs)
                {
                    DateTime pairDate = pairTO.IOPairDate.Date;

                    List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                    if (emplDayPairs.ContainsKey(pairTO.EmployeeID) && emplDayPairs[pairTO.EmployeeID].ContainsKey(pairTO.IOPairDate.Date))
                        dayPairs = emplDayPairs[pairTO.EmployeeID][pairTO.IOPairDate.Date];

                    WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                    if (emplDaySchemas.ContainsKey(pairTO.EmployeeID) && emplDaySchemas[pairTO.EmployeeID].ContainsKey(pairTO.IOPairDate.Date))
                        sch = emplDaySchemas[pairTO.EmployeeID][pairTO.IOPairDate.Date];

                    List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();
                    if (emplDayIntervals.ContainsKey(pairTO.EmployeeID) && emplDayIntervals[pairTO.EmployeeID].ContainsKey(pairTO.IOPairDate.Date))
                        dayIntervals = emplDayIntervals[pairTO.EmployeeID][pairTO.IOPairDate.Date];

                    bool previousDayPair = Common.Misc.isPreviousDayPair(pairTO, ptDict, dayPairs, sch, dayIntervals);

                    if (previousDayPair)
                        pairDate = pairDate.AddDays(-1).Date;

                    if (pairDate.Date == day.Date)
                    {
                        if (!emplBelongingDayPairs.ContainsKey(pairTO.EmployeeID))
                            emplBelongingDayPairs.Add(pairTO.EmployeeID, new List<IOPairProcessedTO>());

                        emplBelongingDayPairs[pairTO.EmployeeID].Add(pairTO);
                    }
                }

                foreach (EmployeeTO emplTO in emplList)
                {
                    if (!emplUTELines.ContainsKey(emplTO.EmployeeID))
                        continue;
                    
                    List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                    if (emplBelongingDayPairs.ContainsKey(emplTO.EmployeeID))
                        dayPairs = emplBelongingDayPairs[emplTO.EmployeeID];

                    emplUTELines[emplTO.EmployeeID].Add(getAbsenceCode(dayPairs, ptDict, holiday, personalHoliday, unjustifiedAbsence, bhUsed, overtimeUsed, stopWorking, strike));
                }
                
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileUTEPath = path + "\\UTE_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                string fileRESPath = path + "\\RESPONSIBLE_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                if (File.Exists(fileUTEPath))
                    File.Delete(fileUTEPath);

                if (File.Exists(fileRESPath))
                    File.Delete(fileRESPath);

                // create UTE file
                StreamWriter writerUTE = File.AppendText(fileUTEPath);
                foreach (int id in emplUTELines.Keys)
                {
                    if (!emplStringoneList.ContainsKey(id))
                        continue;

                    foreach (string stringone in emplStringoneList[id])
                    {
                        string line = "";
                        foreach (string data in emplUTELines[id])
                        {
                            if (data != "STRINGONE")
                                line += data + delimiter;
                            else
                                line += stringone + delimiter;
                        }

                        if (line.Length > 0)
                            line = line.Substring(0, line.Length - delimiter.Length);

                        writerUTE.WriteLine(line);
                    }
                }
                writerUTE.Close();

                // create RESPONSIBLE file
                StreamWriter writerRES = File.AppendText(fileRESPath);
                foreach (int id in emplRESLines.Keys)
                {
                    string line = "";
                    foreach (string data in emplRESLines[id])
                    {
                        line += data + delimiter;
                    }

                    if (line.Length > 0)
                        line = line.Substring(0, line.Length - delimiter.Length);

                    writerRES.WriteLine(line);
                }
                writerRES.Close();

                debug.writeLog("+ Finished BadgeSystemInterface! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "generateExcelBadgeSystemInterface() " + ex.Message);                
            }
        }

        private string getUTECode(WorkingUnitTO ute, int characterNum)
        {
            try
            {
                if (ute.Code.Trim().Length == characterNum)
                    return ute.Code.Trim();
                else if (ute.Code.Trim().Length > characterNum)
                    return ute.Code.Substring(0, characterNum);
                else
                    return ute.Code.PadLeft(characterNum, '0');
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "getUTECode() " + ex.Message);
                throw ex;
            }
        }

        private string getShiftCode(List<WorkTimeIntervalTO> dayIntervals, bool isFlexyShift)
        {
            try
            {
                string shiftCode = "";

                foreach (WorkTimeIntervalTO interval in dayIntervals)
                {
                    if (interval.StartTime.TimeOfDay == new TimeSpan(6, 0, 0) && interval.EndTime.TimeOfDay == new TimeSpan(14, 0, 0))
                    {
                        shiftCode = "1";
                        break;
                    }
                    else if (interval.StartTime.TimeOfDay == new TimeSpan(14, 0, 0) && interval.EndTime.TimeOfDay == new TimeSpan(22, 0, 0))
                    {
                        shiftCode = "2";
                        break;
                    }
                    else if (interval.StartTime.TimeOfDay == new TimeSpan(22, 0, 0) && interval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                    {
                        shiftCode = "3";
                        break;
                    }
                    else if (interval.StartTime.TimeOfDay == new TimeSpan(9, 0, 0) && interval.EndTime.TimeOfDay == new TimeSpan(17, 0, 0))
                    {
                        shiftCode = "4";
                        break;
                    }
                    else if (interval.StartTime.TimeOfDay == new TimeSpan(12, 0, 0) && interval.EndTime.TimeOfDay == new TimeSpan(20, 0, 0))
                    {
                        shiftCode = "5";
                        break;
                    }
                    else if (interval.StartTime.TimeOfDay == new TimeSpan(8, 0, 0) && interval.EndTime.TimeOfDay == new TimeSpan(16, 0, 0))
                    {
                        shiftCode = "C";
                        break;
                    }
                    else if (isFlexyShift && interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                    {
                        shiftCode = "C";
                        break;
                    }
                }

                return shiftCode;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "getShiftCode() " + ex.Message);
                throw ex;
            }
        }

        private string getWUStringone(int wuID, Dictionary<int, WorkingUnitTO> wuDict)
        {
            try
            {
                WorkingUnitTO ute = new WorkingUnitTO();
                WorkingUnitTO workshop = new WorkingUnitTO();
                WorkingUnitTO costCenter = new WorkingUnitTO();
                WorkingUnitTO plant = new WorkingUnitTO();

                if (wuDict.ContainsKey(wuID))
                    ute = wuDict[wuID];

                if (wuDict.ContainsKey(ute.ParentWorkingUID))
                    workshop = wuDict[ute.ParentWorkingUID];

                if (wuDict.ContainsKey(workshop.ParentWorkingUID))
                    costCenter = wuDict[workshop.ParentWorkingUID];

                if (wuDict.ContainsKey(costCenter.ParentWorkingUID))
                    plant = wuDict[costCenter.ParentWorkingUID];

                return getUTECode(plant, 3) + getUTECode(costCenter, 4) + "000000" + getUTECode(workshop, 2) + getUTECode(ute, 2);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "getWUStringone() " + ex.Message);
                throw ex;
            }
        }

        private string getAbsenceCode(List<IOPairProcessedTO> dayPairs, Dictionary<int, PassTypeTO> ptDict, List<int> holiday, List<int> personalHoliday, List<int> unjustifiedAbsence, 
            List<int> bhUsed, List<int> overtimeUsed, List<int> stopWorking, List<int> strike)
        {
            try
            {
                string absenceCode = Constants.noInt.ToString().Trim();

                foreach (IOPairProcessedTO pair in dayPairs)
                {
                    if ((ptDict.ContainsKey(pair.PassTypeID) && ptDict[pair.PassTypeID].IsPass == Constants.wholeDayAbsence)
                        || holiday.Contains(pair.PassTypeID) || personalHoliday.Contains(pair.PassTypeID) || unjustifiedAbsence.Contains(pair.PassTypeID)
                        || bhUsed.Contains(pair.PassTypeID) || overtimeUsed.Contains(pair.PassTypeID) || stopWorking.Contains(pair.PassTypeID) 
                        || strike.Contains(pair.PassTypeID))
                    {
                        absenceCode = Constants.yesInt.ToString().Trim();
                        break;
                    }
                }

                return absenceCode;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Exception in: " + this.ToString() + "getAbsenceCode() " + ex.Message);
                throw ex;
            }
        }
    }
}
