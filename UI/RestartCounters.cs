using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.IO;

using Util;
using Common;
using TransferObjects;

namespace UI
{
    public partial class RestartCounters : Form
    {
        private const string delimiter = "\t";

        // Language
        private CultureInfo culture;
        private ResourceManager rm;

        // Debug log
        DebugLog log;

        // login user
        ApplUserTO logInUser;

        //data
        private List<WorkingUnitTO> wUnits;
        private string wuString = "";

        Dictionary<int, WorkingUnitTO> wuDict = new Dictionary<int, WorkingUnitTO>();
        Dictionary<int, PassTypeTO> ptDict = new Dictionary<int, PassTypeTO>();
        Dictionary<int, WorkTimeSchemaTO> schDict = new Dictionary<int, WorkTimeSchemaTO>();

        public RestartCounters()
        {
            InitializeComponent();

            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();

            // Set Language
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(RestartCounters).Assembly);
            
            setLanguage();
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("RestartCounters", culture);
                
                //group box's text
                gbRestartOvertimeCounter.Text = rm.GetString("gbRestartOvertimeCounter", culture);
                gbBHCuttOffMonths.Text = rm.GetString("gbBHCuttOffMonths", culture);
                gbVacationCutOffMonth.Text = rm.GetString("gbVacationCuttOffMonths", culture);
                gbPaidLeaves.Text = rm.GetString("gbPaidLeaves", culture);                
                                
                //button's text                
                btnClose.Text = rm.GetString("btnClose", culture);                
                btnRecalculateOvertime.Text = rm.GetString("btnRecalculate", culture);
                btnRecalculateBH.Text = rm.GetString("btnRecalculate", culture);
                btnRecalculateVac.Text = rm.GetString("btnRecalculate", culture);
                btnRecalculatePaidLeaves.Text = rm.GetString("btnRecalculate", culture);                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RestartCounters.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RestartCounters_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                wuDict = new WorkingUnit().getWUDictionary();

                wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                ptDict = new PassType().SearchDictionary();
                schDict = new TimeSchema().getDictionary();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RestartCounters.RestartCounters_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRecalculatePaidLeaves_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                                
                string paidLeavesPT = "";

                foreach (int ptID in ptDict.Keys)
                {
                    if (ptDict[ptID].LimitCompositeID != -1)
                        paidLeavesPT += ptID.ToString().Trim() + ",";
                }

                if (paidLeavesPT.Length > 0)
                    paidLeavesPT = paidLeavesPT.Substring(0, paidLeavesPT.Length - 1);

                // get all pairs from begining of the year
                List<IOPairProcessedTO> allPairs = new IOPairProcessed().SearchAllPairsForEmpl("", new DateTime(DateTime.Now.Year, 1, 1), new DateTime(), paidLeavesPT);

                Dictionary<int, int> emplPaidLeavesCount = new Dictionary<int, int>();

                foreach (IOPairProcessedTO pair in allPairs)
                {
                    // count only third shift begining
                    if (pair.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                        continue;

                    if (!emplPaidLeavesCount.ContainsKey(pair.EmployeeID))
                        emplPaidLeavesCount.Add(pair.EmployeeID, 0);

                    emplPaidLeavesCount[pair.EmployeeID]++;
                }

                // get all paid leave counters for all employees
                Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounters = new EmployeeCounterValue().SearchValuesAll();

                EmployeeCounterValue counter = new EmployeeCounterValue();
                EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();
                if (counter.BeginTransaction())
                {
                    try
                    {
                        bool saved = true;
                        foreach (int emplID in emplCounters.Keys)
                        {
                            if (!emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.PaidLeaveCounter))
                                continue;

                            // update counters with new value, updated counters insert to hist table
                            counterHist.SetTransaction(counter.GetTransaction());

                            // move to hist table
                            counterHist.ValueTO = new EmployeeCounterValueHistTO(emplCounters[emplID][(int)Constants.EmplCounterTypes.PaidLeaveCounter]);
                            counterHist.ValueTO.ModifiedBy = NotificationController.GetLogInUser().UserID;
                            saved = saved && (counterHist.Save(false) >= 0);

                            if (!saved)
                                break;

                            if (emplPaidLeavesCount.ContainsKey(emplID))
                                emplCounters[emplID][(int)Constants.EmplCounterTypes.PaidLeaveCounter].Value = emplPaidLeavesCount[emplID];
                            else
                                emplCounters[emplID][(int)Constants.EmplCounterTypes.PaidLeaveCounter].Value = 0;

                            counter.ValueTO = new EmployeeCounterValueTO(emplCounters[emplID][(int)Constants.EmplCounterTypes.PaidLeaveCounter]);
                            counter.ValueTO.ModifiedBy = NotificationController.GetLogInUser().UserID;

                            saved = saved && counter.Update(false);

                            if (!saved)
                                break;
                        }

                        if (saved)
                        {
                            counter.CommitTransaction();
                            MessageBox.Show(rm.GetString("paidLeavesRecalculated", culture));
                        }
                        else
                        {
                            if (counter.GetTransaction() != null)
                                counter.RollbackTransaction();
                            MessageBox.Show(rm.GetString("paidLeavesNotRecalculated", culture));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (counter.GetTransaction() != null)
                            counter.RollbackTransaction();
                        throw ex;
                    }
                }
                else
                    MessageBox.Show(rm.GetString("paidLeavesNotRecalculated", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RestartCounters.btnRecalculatePaidLeaves_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRecalculateOvertime_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                // get all overtime paid pass types                
                List<int> overtimePTList = new Common.Rule().SearchRulesExact(Constants.RuleCompanyOvertimePaid);

                string overtimePT = "";

                foreach (int ptID in overtimePTList)
                {
                    overtimePT += ptID.ToString().Trim() + ",";
                }

                if (overtimePT.Length > 0)
                    overtimePT = overtimePT.Substring(0, overtimePT.Length - 1);

                // get all overtime paid pairs from begining of the month
                DateTime firstMonthDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                List<IOPairProcessedTO> allPairs = new IOPairProcessed().SearchAllPairsForEmpl("", firstMonthDay.Date, new DateTime(), overtimePT);

                // get all pairs from first day of the month
                List<IOPairProcessedTO> firstDayPairs = new IOPairProcessed().SearchAllPairsForEmpl("", firstMonthDay.Date, firstMonthDay.Date, "");

                Dictionary<int, List<IOPairProcessedTO>> emplFirstDayPairs = new Dictionary<int,List<IOPairProcessedTO>>();
                string emplIDs = "";
                foreach (IOPairProcessedTO pair in firstDayPairs)
                {
                    if (!emplFirstDayPairs.ContainsKey(pair.EmployeeID))
                    {
                        emplFirstDayPairs.Add(pair.EmployeeID, new List<IOPairProcessedTO>());
                        emplIDs += pair.EmployeeID.ToString().Trim() + ",";
                    }

                    emplFirstDayPairs[pair.EmployeeID].Add(pair);
                }

                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                // get first month day schedules
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, firstMonthDay.Date, firstMonthDay.Date, null);

                Dictionary<int, int> emplOvertimeCount = new Dictionary<int, int>();

                foreach (IOPairProcessedTO pair in allPairs)
                {
                    // if pair is on month first day, check if it belongs to previous month
                    if (pair.IOPairDate.Equals(firstMonthDay.Date))
                    {
                        List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                        if (emplFirstDayPairs.ContainsKey(pair.EmployeeID))
                            dayPairs = emplFirstDayPairs[pair.EmployeeID];

                        List<EmployeeTimeScheduleTO> scheduleList = new List<EmployeeTimeScheduleTO>();
                        if (emplSchedules.ContainsKey(pair.EmployeeID))
                            scheduleList = emplSchedules[pair.EmployeeID];

                        if (Common.Misc.isPreviousDayPair(pair, ptDict, dayPairs, Common.Misc.getTimeSchema(pair.IOPairDate.Date, scheduleList, schDict), Common.Misc.getTimeSchemaInterval(pair.IOPairDate.Date, scheduleList, schDict)))
                            continue;
                    }

                    if (!emplOvertimeCount.ContainsKey(pair.EmployeeID))
                        emplOvertimeCount.Add(pair.EmployeeID, 0);
                                        
                    emplOvertimeCount[pair.EmployeeID] += (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                    if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                        emplOvertimeCount[pair.EmployeeID]++;
                }

                // get all overtime counters for all employees
                Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounters = new EmployeeCounterValue().SearchValuesAll();

                EmployeeCounterValue counter = new EmployeeCounterValue();
                EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();
                if (counter.BeginTransaction())
                {
                    try
                    {
                        bool saved = true;
                        foreach (int emplID in emplCounters.Keys)
                        {
                            if (!emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.OvertimeCounter))
                                continue;

                            // update counters with new value, updated counters insert to hist table
                            counterHist.SetTransaction(counter.GetTransaction());

                            // move to hist table
                            counterHist.ValueTO = new EmployeeCounterValueHistTO(emplCounters[emplID][(int)Constants.EmplCounterTypes.OvertimeCounter]);
                            counterHist.ValueTO.ModifiedBy = NotificationController.GetLogInUser().UserID;
                            saved = saved && (counterHist.Save(false) >= 0);

                            if (!saved)
                                break;

                            if (emplOvertimeCount.ContainsKey(emplID))
                                emplCounters[emplID][(int)Constants.EmplCounterTypes.OvertimeCounter].Value = emplOvertimeCount[emplID];
                            else
                                emplCounters[emplID][(int)Constants.EmplCounterTypes.OvertimeCounter].Value = 0;

                            counter.ValueTO = new EmployeeCounterValueTO(emplCounters[emplID][(int)Constants.EmplCounterTypes.OvertimeCounter]);
                            counter.ValueTO.ModifiedBy = NotificationController.GetLogInUser().UserID;

                            saved = saved && counter.Update(false);

                            if (!saved)
                                break;
                        }

                        if (saved)
                        {
                            counter.CommitTransaction();
                            MessageBox.Show(rm.GetString("CountersUpdateSucc", culture));
                        }
                        else
                        {
                            if (counter.GetTransaction() != null)
                                counter.RollbackTransaction();
                            MessageBox.Show(rm.GetString("CountersUpdateFaild", culture));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (counter.GetTransaction() != null)
                            counter.RollbackTransaction();
                        throw ex;
                    }
                }
                else
                    MessageBox.Show(rm.GetString("CountersUpdateFaild", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RestartCounters.btnRecalculateOvertime_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRecalculateVac_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // get annual leave cut off dates
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict = new Common.Rule().SearchTypeAllRules(Constants.RuleAnnualLeaveCutOffDate);

                if (rulesDict.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noAnnualLeaveCutOffDate", culture));
                    return;
                }

                // get all employees with asco data
                Dictionary<int, EmployeeTO> emplDict = new Employee().SearchDictionaryWithASCO();

                // get all annual leave pass types                
                List<int> alPTList = new Common.Rule().SearchRulesExact(Constants.RuleCompanyAnnualLeave);
                alPTList.AddRange(new Common.Rule().SearchRulesExact(Constants.RuleCompanyCollectiveAnnualLeave));

                string alPT = "";
                foreach (int ptID in alPTList)
                {
                    alPT += ptID.ToString().Trim() + ",";
                }

                if (alPT.Length > 0)
                    alPT = alPT.Substring(0, alPT.Length - 1);

                DateTime minDate = new DateTime();
                // get minimal cut off date
                foreach (int company in rulesDict.Keys)
                {
                    foreach (int type in rulesDict[company].Keys)
                    {
                        if (rulesDict[company][type].ContainsKey(Constants.RuleAnnualLeaveCutOffDate))
                        {
                            DateTime cutOff = new DateTime(DateTime.Now.Year, rulesDict[company][type][Constants.RuleAnnualLeaveCutOffDate].RuleDateTime1.Month, rulesDict[company][type][Constants.RuleAnnualLeaveCutOffDate].RuleDateTime1.Day);

                            if (cutOff >= DateTime.Now)
                                cutOff = cutOff.AddYears(-1).Date;

                            if (minDate.Equals(new DateTime()) || minDate.Date > cutOff.Date)
                                minDate = cutOff.Date;
                        }
                    }
                }

                // get all annual leave pairs from min date
                List<IOPairProcessedTO> allPairs = new IOPairProcessed().SearchAllPairsForEmpl("", minDate.AddDays(1), new DateTime(), alPT);

                Dictionary<int, int> emplAnnualLeavesUsed = new Dictionary<int, int>();

                foreach (IOPairProcessedTO pair in allPairs)
                {
                    // count only third shift begining
                    if (pair.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                        continue;

                    // skip if it is not pair after cut off date
                    if (emplDict.ContainsKey(pair.EmployeeID))
                    {
                        EmployeeTO empl = emplDict[pair.EmployeeID];

                        if (empl.Tag != null && empl.Tag is EmployeeAsco4TO)
                        {
                            EmployeeAsco4TO asco = (EmployeeAsco4TO)empl.Tag;

                            if (rulesDict.ContainsKey(asco.IntegerValue4) && rulesDict[asco.IntegerValue4].ContainsKey(empl.EmployeeTypeID)
                                && rulesDict[asco.IntegerValue4][empl.EmployeeTypeID].ContainsKey(Constants.RuleAnnualLeaveCutOffDate))
                            {
                                DateTime cutOff = new DateTime(DateTime.Now.Year, rulesDict[asco.IntegerValue4][empl.EmployeeTypeID][Constants.RuleAnnualLeaveCutOffDate].RuleDateTime1.Month, rulesDict[asco.IntegerValue4][empl.EmployeeTypeID][Constants.RuleAnnualLeaveCutOffDate].RuleDateTime1.Day);

                                if (cutOff >= DateTime.Now)
                                    cutOff = cutOff.AddYears(-1).Date;

                                if (pair.IOPairDate <= cutOff.Date)
                                    continue;

                                if (!emplAnnualLeavesUsed.ContainsKey(pair.EmployeeID))
                                    emplAnnualLeavesUsed.Add(pair.EmployeeID, 0);

                                emplAnnualLeavesUsed[pair.EmployeeID]++;
                            }
                        }
                    }
                }

                // get all annual leave counters for all employees
                Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounters = new EmployeeCounterValue().SearchValuesAll();

                EmployeeCounterValue counter = new EmployeeCounterValue();
                EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();
                Dictionary<int, int> emplAnnualLeavePaidDays = new Dictionary<int, int>();
                if (counter.BeginTransaction())
                {                    
                    try
                    {
                        bool saved = true;
                        foreach (int emplID in emplCounters.Keys)
                        {
                            if (!emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter)
                                || !emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter)
                                || !emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))
                                continue;

                            if (!emplAnnualLeavePaidDays.ContainsKey(emplID))
                                emplAnnualLeavePaidDays.Add(emplID, 0);

                            int used = emplCounters[emplID][(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value;

                            if (emplAnnualLeavesUsed.ContainsKey(emplID))
                                used -= emplAnnualLeavesUsed[emplID];

                            if (emplCounters[emplID][(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value > used)
                                emplAnnualLeavePaidDays[emplID] = emplCounters[emplID][(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value - used;

                            // update counters with new value, updated counters insert to hist table
                            counterHist.SetTransaction(counter.GetTransaction());

                            foreach (int cType in emplCounters[emplID].Keys)
                            {
                                if (cType != (int)Constants.EmplCounterTypes.AnnualLeaveCounter
                                    && cType != (int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter
                                    && cType != (int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter)
                                    continue;

                                // move to hist table
                                counterHist.ValueTO = new EmployeeCounterValueHistTO(emplCounters[emplID][cType]);
                                counterHist.ValueTO.ModifiedBy = NotificationController.GetLogInUser().UserID;
                                saved = saved && (counterHist.Save(false) >= 0);

                                if (!saved)
                                    break;
                            }

                            if (!saved)
                                break;

                            if (emplAnnualLeavesUsed.ContainsKey(emplID))
                                emplCounters[emplID][(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value = emplAnnualLeavesUsed[emplID];
                            else
                                emplCounters[emplID][(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value = 0;

                            counter.ValueTO = new EmployeeCounterValueTO(emplCounters[emplID][(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter]);
                            counter.ValueTO.ModifiedBy = NotificationController.GetLogInUser().UserID;

                            saved = saved && counter.Update(false);

                            if (!saved)
                                break;

                            counter.ValueTO = new EmployeeCounterValueTO(emplCounters[emplID][(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter]);
                            counter.ValueTO.Value = emplCounters[emplID][(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value;

                            saved = saved && counter.Update(false);

                            if (!saved)
                                break;
                        }

                        if (saved)
                        {
                            counter.CommitTransaction();
                            createFile(emplAnnualLeavePaidDays, "AnnualLeaveDaysLeft");
                            MessageBox.Show(rm.GetString("CountersUpdateSucc", culture));
                        }
                        else
                        {
                            if (counter.GetTransaction() != null)
                                counter.RollbackTransaction();
                            MessageBox.Show(rm.GetString("CountersUpdateFaild", culture));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (counter.GetTransaction() != null)
                            counter.RollbackTransaction();
                        throw ex;
                    }
                }
                else
                    MessageBox.Show(rm.GetString("CountersUpdateFaild", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RestartCounters.btnRecalculateVac_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRecalculateBH_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // get bank hours cut off dates
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> bhCutDate1Dict = new Common.Rule().SearchTypeAllRules(Constants.RuleBankHrsCutOffDate1);
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> bhCutDate2Dict = new Common.Rule().SearchTypeAllRules(Constants.RuleBankHrsCutOffDate2);
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> bhRoundingDict = new Common.Rule().SearchTypeAllRules(Constants.RuleBankHoursUsedRounding);
                
                if (bhCutDate1Dict.Count <= 0 && bhCutDate2Dict.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noBHCutOffDate", culture));
                    return;
                }

                // get all employees with asco data
                Dictionary<int, EmployeeTO> emplDict = new Employee().SearchDictionaryWithASCO();

                // get all bank hours pass types
                List<int> bhPTList = new Common.Rule().SearchRulesExact(Constants.RuleCompanyBankHour);

                // get all bank hours used pass types
                List<int> bhUsedPTList = new Common.Rule().SearchRulesExact(Constants.RuleCompanyBankHourUsed);

                string bhPT = "";                
                foreach (int ptID in bhPTList)
                {
                    bhPT += ptID.ToString().Trim() + ",";
                }

                foreach (int ptID in bhUsedPTList)
                {
                    bhPT += ptID.ToString().Trim() + ",";
                }

                if (bhPT.Length > 0)
                    bhPT = bhPT.Substring(0, bhPT.Length - 1);

                DateTime minDate = new DateTime();
                DateTime maxDate = new DateTime();
                Dictionary<int, Dictionary<int, DateTime>> cutOffDateDict = new Dictionary<int, Dictionary<int, DateTime>>();

                List<WorkingUnitTO> companies = new WorkingUnit().getRootWorkingUnitsList("");
                Dictionary<int, Dictionary<int, string>> typesDict = new EmployeeType().SearchDictionary();
                
                // get minimal cut off date
                foreach (WorkingUnitTO companyWU in companies)
                {
                    int company = companyWU.WorkingUnitID;

                    if (!cutOffDateDict.ContainsKey(company))
                        cutOffDateDict.Add(company, new Dictionary<int, DateTime>());

                    if (!typesDict.ContainsKey(company))
                        continue;

                    foreach (int type in typesDict[company].Keys)
                    {
                        DateTime cutOff = new DateTime();
                        DateTime cutOff1 = new DateTime();
                        DateTime cutOff2 = new DateTime();

                        if (!cutOffDateDict[company].ContainsKey(type))
                            cutOffDateDict[company].Add(type, cutOff);

                        if (bhCutDate1Dict.ContainsKey(company) && bhCutDate1Dict[company].ContainsKey(type) && bhCutDate1Dict[company][type].ContainsKey(Constants.RuleBankHrsCutOffDate1))
                            cutOff1 = new DateTime(DateTime.Now.Year, bhCutDate1Dict[company][type][Constants.RuleBankHrsCutOffDate1].RuleDateTime1.Month, bhCutDate1Dict[company][type][Constants.RuleBankHrsCutOffDate1].RuleDateTime1.Day);

                        if (bhCutDate2Dict.ContainsKey(company) && bhCutDate2Dict[company].ContainsKey(type) && bhCutDate2Dict[company][type].ContainsKey(Constants.RuleBankHrsCutOffDate2))
                            cutOff2 = new DateTime(DateTime.Now.Year, bhCutDate2Dict[company][type][Constants.RuleBankHrsCutOffDate2].RuleDateTime1.Month, bhCutDate2Dict[company][type][Constants.RuleBankHrsCutOffDate2].RuleDateTime1.Day);

                        if (cutOff1.Equals(new DateTime()) || cutOff1.Equals(Constants.dateTimeNullValue()))
                        {
                            if (cutOff2.Equals(new DateTime()) || cutOff2.Equals(Constants.dateTimeNullValue()))
                                continue;
                            else
                                cutOff = cutOff2;
                        }
                        else if (cutOff2.Equals(new DateTime()) || cutOff2.Equals(Constants.dateTimeNullValue()))
                            cutOff = cutOff1;

                        if (!cutOff.Equals(new DateTime()))
                        {
                            if (cutOff >= DateTime.Now)
                                cutOff = cutOff.AddYears(-1).Date;
                        }
                        else
                        {
                            if (cutOff1.Date < DateTime.Now.Date && cutOff2.Date < DateTime.Now)
                                cutOff = getMax(cutOff1, cutOff2).Date;
                            else if (cutOff1.Date >= DateTime.Now.Date && cutOff2.Date >= DateTime.Now)
                                cutOff = getMax(cutOff1, cutOff2).Date.AddYears(-1);
                            else
                                cutOff = getMin(cutOff1, cutOff2).Date;
                        }

                        if (minDate.Equals(new DateTime()) || minDate.Date > cutOff.Date)
                            minDate = cutOff.Date;

                        if (maxDate.Equals(new DateTime()) || maxDate.Date < cutOff.Date)
                            maxDate = cutOff.Date;

                        cutOffDateDict[company][type] = cutOff.Date;
                    }
                }

                // get all bank hours pairs from min date
                List<IOPairProcessedTO> allPairs = new IOPairProcessed().SearchAllPairsForEmpl("", minDate.AddDays(1), new DateTime(), bhPT);

                // get all pairs from min date to max date
                List<IOPairProcessedTO> allEmplPairs = new IOPairProcessed().SearchAllPairsForEmpl("", minDate.AddDays(1), maxDate.AddDays(1), "");
                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();

                string emplIDs = "";
                foreach (IOPairProcessedTO pair in allEmplPairs)
                {
                    if (!emplDayPairs.ContainsKey(pair.EmployeeID))
                    {
                        emplDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                        emplIDs += pair.EmployeeID.ToString().Trim() + ",";
                    }
                    if (!emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                        emplDayPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());
                    emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                }

                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                Dictionary<int, int> emplBHCount = new Dictionary<int, int>();

                // get employee schedules for critical days
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, minDate.AddDays(1), maxDate.AddDays(1), null);

                foreach (IOPairProcessedTO pair in allPairs)
                {
                    // skip if it is not pair after cut off date
                    if (emplDict.ContainsKey(pair.EmployeeID))
                    {
                        EmployeeTO empl = emplDict[pair.EmployeeID];

                        if (empl.Tag != null && empl.Tag is EmployeeAsco4TO)
                        {
                            EmployeeAsco4TO asco = (EmployeeAsco4TO)empl.Tag;

                            if (cutOffDateDict.ContainsKey(asco.IntegerValue4) && cutOffDateDict[asco.IntegerValue4].ContainsKey(empl.EmployeeTypeID)
                                && !cutOffDateDict[asco.IntegerValue4][empl.EmployeeTypeID].Equals(new DateTime()))
                            {
                                DateTime cutOff = cutOffDateDict[asco.IntegerValue4][empl.EmployeeTypeID];

                                // do not calculate hours before cut off date
                                if (pair.IOPairDate <= cutOff.Date)
                                    continue;

                                // if pair is from day after cut off date, check if it belongs to previous day
                                if (pair.IOPairDate.Date == cutOff.Date.AddDays(1))
                                {
                                    List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                                    if (emplDayPairs.ContainsKey(pair.EmployeeID) && emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                                        dayPairs = emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date];

                                    List<EmployeeTimeScheduleTO> scheduleList = new List<EmployeeTimeScheduleTO>();
                                    if (emplSchedules.ContainsKey(pair.EmployeeID))
                                        scheduleList = emplSchedules[pair.EmployeeID];

                                    if (Common.Misc.isPreviousDayPair(pair, ptDict, dayPairs, Common.Misc.getTimeSchema(pair.IOPairDate.Date, scheduleList, schDict), Common.Misc.getTimeSchemaInterval(pair.IOPairDate.Date, scheduleList, schDict)))
                                        continue;
                                }

                                if (!emplBHCount.ContainsKey(pair.EmployeeID))
                                    emplBHCount.Add(pair.EmployeeID, 0);

                                int bhRounding = 1;

                                if (bhRoundingDict.ContainsKey(asco.IntegerValue4) && bhRoundingDict[asco.IntegerValue4].ContainsKey(empl.EmployeeTypeID)
                                    && bhRoundingDict[asco.IntegerValue4][empl.EmployeeTypeID].ContainsKey(Constants.RuleBankHoursUsedRounding))
                                    bhRounding = bhRoundingDict[asco.IntegerValue4][empl.EmployeeTypeID][Constants.RuleBankHoursUsedRounding].RuleValue;

                                int pairDuration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;
                                if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                                    pairDuration++;

                                if (bhPTList.Contains(pair.PassTypeID))
                                    emplBHCount[pair.EmployeeID] += pairDuration;
                                else if (bhUsedPTList.Contains(pair.PassTypeID))
                                {
                                    if (pairDuration % bhRounding != 0)
                                        pairDuration += bhRounding - (pairDuration % bhRounding);
                                    emplBHCount[pair.EmployeeID] -= pairDuration;
                                }
                            }
                        }
                    }
                }

                // get all bank hours counters for all employees
                Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounters = new EmployeeCounterValue().SearchValuesAll();

                EmployeeCounterValue counter = new EmployeeCounterValue();
                EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();
                Dictionary<int, int> emplBHPaidHours = new Dictionary<int, int>();
                if (counter.BeginTransaction())
                {
                    try
                    {
                        bool saved = true;
                        foreach (int emplID in emplCounters.Keys)
                        {
                            if (!emplCounters[emplID].ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                                continue;

                            if (!emplBHPaidHours.ContainsKey(emplID))
                                emplBHPaidHours.Add(emplID, 0);

                            int bhHours = emplCounters[emplID][(int)Constants.EmplCounterTypes.BankHoursCounter].Value;

                            if (emplBHCount.ContainsKey(emplID))
                                bhHours -= emplBHCount[emplID];

                            emplBHPaidHours[emplID] = bhHours;

                            // update counters with new value, updated counters insert to hist table
                            counterHist.SetTransaction(counter.GetTransaction());

                            // move to hist table
                            counterHist.ValueTO = new EmployeeCounterValueHistTO(emplCounters[emplID][(int)Constants.EmplCounterTypes.BankHoursCounter]);
                            counterHist.ValueTO.ModifiedBy = NotificationController.GetLogInUser().UserID;
                            saved = saved && (counterHist.Save(false) >= 0);

                            if (!saved)
                                break;

                            if (emplBHCount.ContainsKey(emplID))
                                emplCounters[emplID][(int)Constants.EmplCounterTypes.BankHoursCounter].Value = emplBHCount[emplID];
                            else
                                emplCounters[emplID][(int)Constants.EmplCounterTypes.BankHoursCounter].Value = 0;

                            counter.ValueTO = new EmployeeCounterValueTO(emplCounters[emplID][(int)Constants.EmplCounterTypes.BankHoursCounter]);
                            counter.ValueTO.ModifiedBy = NotificationController.GetLogInUser().UserID;

                            saved = saved && counter.Update(false);

                            if (!saved)
                                break;
                        }

                        if (saved)
                        {
                            counter.CommitTransaction();
                            createFile(emplBHPaidHours, "BankHoursLeft");
                            MessageBox.Show(rm.GetString("CountersUpdateSucc", culture));
                        }
                        else
                        {
                            if (counter.GetTransaction() != null)
                                counter.RollbackTransaction();
                            MessageBox.Show(rm.GetString("CountersUpdateFaild", culture));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (counter.GetTransaction() != null)
                            counter.RollbackTransaction();
                        throw ex;
                    }
                }
                else
                    MessageBox.Show(rm.GetString("CountersUpdateFaild", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RestartCounters.btnRecalculateBH_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void createFile(Dictionary<int, int> emplLeftValues, string fileName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SaveFileDialog fbDialog = new SaveFileDialog();

                fbDialog.DefaultExt = ".txt";
                fbDialog.Filter = "Text files (*.txt)|*.txt";
                fbDialog.Title = "Save file";
                fbDialog.FileName = fileName + DateTime.Now.ToString("yyyy_MM_dd_HH_mm");
                fbDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;

                    string filePath = fbDialog.FileName;
                    fbDialog.Dispose();

                    FileStream stream = new FileStream(filePath, FileMode.Append);
                    stream.Close();

                    // insert header                    
                    string header = rm.GetString("hdrEmplID", culture) + delimiter + rm.GetString("hdrValue", culture);

                    StreamWriter writer = File.AppendText(filePath);
                    writer.WriteLine(header);

                    foreach (int id in emplLeftValues.Keys)
                    {
                        writer.WriteLine(id.ToString().Trim() + delimiter + emplLeftValues[id].ToString().Trim());
                    }

                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RestartCounters.createFile(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private DateTime getMin(DateTime date1, DateTime date2)
        {
            try
            {
                if (date1.Date < date2.Date)
                    return date1;
                else
                    return date2;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RestartCounters.getMin(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private DateTime getMax(DateTime date1, DateTime date2)
        {
            try
            {
                if (date1.Date > date2.Date)
                    return date1;
                else
                    return date2;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " RestartCounters.getMax(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
