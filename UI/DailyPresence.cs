using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TransferObjects;
using Common;
using System.Resources;
using System.Globalization;
using Util;
using System.IO;
using ACTAWorkAnalysisReports;

namespace UI
{
    public partial class DailyPresence : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        ApplUserTO logInUser;

        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private Dictionary<int, OrganizationalUnitTO> oUnits = new Dictionary<int, OrganizationalUnitTO>();
        private string wuString = "";
        private string ouString = "";
        private List<int> wuList = new List<int>();
        private List<int> ouList = new List<int>();


        public DailyPresence()
        {
            try
            {
                InitializeComponent();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(MealsUsed).Assembly);
                logInUser = NotificationController.GetLogInUser();
                setLanguage();
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " DailyPresence.DailyPresence(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void setLanguage()
        {
            try
            {
                //gb text
                gbFilter.Text = rm.GetString("gbUnitFilter", culture);

                //lblText
                lblDate.Text = rm.GetString("lblDate", culture) + ":";


                //RB text
                rbOU.Text = rm.GetString("gbOrganizationalUnits", culture);
                rbWU.Text = rm.GetString("WUForm", culture);

                //button text
                btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
                btnClose.Text = rm.GetString("btnClose", culture);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsReports.setLanguage(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string filePath = "";

                DataSet ds = new DataSet("DailyPresence" + DateTime.Now.ToString("ddMMyyyy"));
                string sickleave = "Sickleave";
                string vacation = "Vacation";
                string paidLeave = "Paid absence";
                string absent = "Absent";
                string active = "Active";

                string wu = "";

                List<EmployeeTO> emplArray = new List<EmployeeTO>();
                int wuID = -1;
                if (rbWU.Checked)
                {
                    wuID = (int)cbWU.SelectedValue;
                    string workUnitID = wuID.ToString();
                    if (wuID == -1)
                    {
                        MessageBox.Show(rm.GetString("lblWUNotSelected", culture));
                        return;
                        //emplArray = new Employee().SearchByWU(wuString);
                    }
                    else
                    {
                        List<WorkingUnitTO> wList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWU.SelectedValue)
                            {
                                wList.Add(workingUnit);
                                workUnit.WUTO = workingUnit;
                                wu = workingUnit.Description;
                            }
                        }
                        if (workUnit.WUTO.ChildWUNumber > 0)
                            wList = workUnit.FindAllChildren(wList);
                        workUnitID = "";
                        foreach (WorkingUnitTO wunit in wList)
                        {
                            if (wuList.Contains(wunit.WorkingUnitID))
                                workUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (workUnitID.Length > 0)
                        {
                            workUnitID = workUnitID.Substring(0, workUnitID.Length - 1);
                        }

                        //emplArray = new Employee().SearchByWU(workUnitID);
                        emplArray = new Employee().SearchByWULoans(workUnitID, -1, null, dtpDate.Value.Date, dtpDate.Value.Date);
                    }
                }
                else
                {
                    wuID = (int)cbOU.SelectedValue;
                    string orgUnitID = wuID.ToString();
                    if (wuID == -1)
                    {
                        MessageBox.Show(rm.GetString("lblOUNotSelected", culture));
                        return;
                        //emplArray = new Employee().SearchByOU(ouString, -1, -1, -1, dtpFrom.Value, dtpTo.Value);
                    }
                    else
                    {
                        List<OrganizationalUnitTO> oList = new List<OrganizationalUnitTO>();
                        OrganizationalUnit orgUnit = new OrganizationalUnit();
                        foreach (KeyValuePair<int, OrganizationalUnitTO> organizationalUnitPair in oUnits)
                        {
                            OrganizationalUnitTO organizationalUnit = organizationalUnitPair.Value;
                            if (organizationalUnit.OrgUnitID == (int)this.cbOU.SelectedValue)
                            {
                                oList.Add(organizationalUnit);
                                orgUnit.OrgUnitTO = organizationalUnit;
                                wu = organizationalUnit.Desc;
                            }
                        }

                        oList = orgUnit.FindAllChildren(oList);
                        orgUnitID = "";
                        foreach (OrganizationalUnitTO ounit in oList)
                        {
                            if (ouList.Contains(ounit.OrgUnitID))
                                orgUnitID += ounit.OrgUnitID.ToString().Trim() + ",";
                        }

                        if (orgUnitID.Length > 0)
                        {
                            orgUnitID = orgUnitID.Substring(0, orgUnitID.Length - 1);
                        }

                        emplArray = new Employee().SearchByOU(orgUnitID, -1, null, dtpDate.Value.Date, dtpDate.Value.Date);
                    }
                }
                string emplIDs = "";
                if (emplArray.Count > 0)
                {
                    foreach (EmployeeTO empl in emplArray)
                    {
                        emplIDs += empl.EmployeeID + ",";

                    }
                    if (emplIDs.Length > 0)
                    {
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                    }
                }
                else
                {
                    if (rbWU.Checked)
                        MessageBox.Show(rm.GetString("noEmployeesForWU", culture));
                    else
                        MessageBox.Show(rm.GetString("noEmployeesForOU", culture));
                    return;
                }
                List<DateTime> datesList = new List<DateTime>();
                datesList.Add(dtpDate.Value.Date);
                int company = -1;


                int pass_type = -1;
                int annual_leave = -1;
                Common.Rule ruleOffset = new Common.Rule();

                ruleOffset.RuleTO.RuleType = Constants.RuleCompanySickLeaveNCF;

                List<RuleTO> rulesPayslipOffset = ruleOffset.Search();

                if (rulesPayslipOffset.Count > 0)
                {
                    pass_type = rulesPayslipOffset[0].RuleValue;
                }

                Common.Rule ruleAnnualLeave = new Common.Rule();

                ruleAnnualLeave.RuleTO.RuleType = Constants.RuleCompanyAnnualLeave;

                List<RuleTO> rulesAnnualLeave = ruleAnnualLeave.Search();

                if (rulesAnnualLeave.Count > 0)
                {
                    annual_leave = rulesAnnualLeave[0].RuleValue;
                }
                PassTypesConfirmation passConf = new PassTypesConfirmation();
                passConf.PTConfirmTO.PassTypeID = pass_type;
                List<PassTypesConfirmationTO> listPassTypesSickLeave = passConf.Search();

                Dictionary<int, PassTypesConfirmationTO> dictSickLeave = new Dictionary<int, PassTypesConfirmationTO>();
                dictSickLeave.Add(pass_type, new PassTypesConfirmationTO());
                foreach (PassTypesConfirmationTO pc in listPassTypesSickLeave)
                {
                    dictSickLeave.Add(pc.ConfirmationPassTypeID, pc);
                }
                List<EmployeeTypeTO> emplTypes = new EmployeeType().Search();
                Dictionary<int, EmployeeTypeTO> dictEmplType = new Dictionary<int, EmployeeTypeTO>();
                foreach (EmployeeTypeTO type in emplTypes)
                {
                    dictEmplType.Add(type.EmployeeTypeID, type);
                }
                Dictionary<int, int> listEmplTypes = new Dictionary<int, int>();
                Dictionary<int, int> emplTypeCount = new Dictionary<int, int>();
                foreach (EmployeeTO empl in emplArray)
                {
                    listEmplTypes.Add(empl.EmployeeID, empl.EmployeeTypeID);
                    if (emplTypeCount.ContainsKey(empl.EmployeeTypeID))
                        emplTypeCount[empl.EmployeeTypeID]++;
                    else
                        emplTypeCount.Add(empl.EmployeeTypeID, 1);
                }
                Dictionary<int, PassTypeTO> listPassTypesPaidLeave = new Dictionary<int, PassTypeTO>();
                Dictionary<int, PassTypeTO> dictPresence = new Dictionary<int, PassTypeTO>();

                List<PassTypeTO> listPT = new PassType().Search();

                int passTypePresence = -1;
                Common.Rule rule = new Common.Rule();
                rule.RuleTO.WorkingUnitID = company;
                rule.RuleTO.RuleType = Constants.RuleCompanyRegularWork;

                List<RuleTO> rules = rule.Search();

                if (rules.Count > 0)
                {
                    passTypePresence = rules[0].RuleValue;
                }


                foreach (PassTypeTO pt in listPT)
                {
                    if (pt.PassTypeID == passTypePresence || pt.IsPass == Constants.overtimePassType)
                    {
                        dictPresence.Add(pt.PassTypeID, pt);
                    }
                    else if (pt.PassTypeID != annual_leave && !dictSickLeave.ContainsKey(pt.PassTypeID))
                    {
                        listPassTypesPaidLeave.Add(pt.PassTypeID, pt);
                    }
                }

                List<IOPairProcessedTO> IOPairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, datesList, "");
                Dictionary<int, Dictionary<string, int>> dictionaryCount = new Dictionary<int, Dictionary<string, int>>();
                Dictionary<int, IOPairProcessedTO> dictionaryEmpl = new Dictionary<int, IOPairProcessedTO>();
                Dictionary<int, Dictionary<int, IOPairProcessedTO>> IOPairsEmpl = new Dictionary<int, Dictionary<int, IOPairProcessedTO>>();
                foreach (IOPairProcessedTO pair in IOPairs)
                {
                    if (IOPairsEmpl.ContainsKey(pair.EmployeeID))
                    {
                        if (!IOPairsEmpl[pair.EmployeeID].ContainsKey(pair.PassTypeID))
                        {
                            IOPairsEmpl[pair.EmployeeID].Add(pair.PassTypeID, pair);
                        }
                    }
                    else
                    {
                        Dictionary<int, IOPairProcessedTO> listIOPairs = new Dictionary<int, IOPairProcessedTO>();
                        listIOPairs.Add(pair.PassTypeID, pair);
                        IOPairsEmpl.Add(pair.EmployeeID, listIOPairs);
                    }

                }
                foreach (KeyValuePair<int, Dictionary<int, IOPairProcessedTO>> mainPair in IOPairsEmpl)
                {
                    foreach (KeyValuePair<int, IOPairProcessedTO> pair in mainPair.Value)
                    {
                        if (dictPresence.ContainsKey(pair.Key))
                        {
                            if (!dictionaryEmpl.ContainsKey(pair.Value.EmployeeID))
                            {
                                dictionaryEmpl.Add(pair.Value.EmployeeID, pair.Value);
                                if (dictionaryCount.ContainsKey(listEmplTypes[pair.Value.EmployeeID]))
                                {
                                    if (dictionaryCount[listEmplTypes[pair.Value.EmployeeID]].ContainsKey(active))
                                    {
                                        dictionaryCount[listEmplTypes[pair.Value.EmployeeID]][active]++;
                                    }
                                    else
                                    {
                                        Dictionary<string, int> dict = new Dictionary<string, int>();
                                        dict.Add(active, 1);
                                        dictionaryCount[listEmplTypes[pair.Value.EmployeeID]].Add(active, 1);
                                    }
                                }
                                else
                                {
                                    Dictionary<string, int> dict = new Dictionary<string, int>();
                                    dict.Add(active, 1);
                                    dictionaryCount.Add(listEmplTypes[pair.Value.EmployeeID], dict);
                                }
                            }
                        }
                    }
                }
                foreach (KeyValuePair<int, Dictionary<int, IOPairProcessedTO>> main in IOPairsEmpl)
                {

                    foreach (KeyValuePair<int, IOPairProcessedTO> pair in main.Value)
                    {
                        if (!dictionaryEmpl.ContainsKey(pair.Value.EmployeeID))
                        {
                            if (dictSickLeave.ContainsKey(pair.Key))
                            {
                                dictionaryEmpl.Add(pair.Value.EmployeeID, pair.Value);
                                if (dictionaryCount.ContainsKey(listEmplTypes[pair.Value.EmployeeID]))
                                {
                                    if (dictionaryCount[listEmplTypes[pair.Value.EmployeeID]].ContainsKey(sickleave))
                                    {
                                        dictionaryCount[listEmplTypes[pair.Value.EmployeeID]][sickleave]++;
                                    }
                                    else
                                    {
                                        dictionaryCount[listEmplTypes[pair.Value.EmployeeID]].Add(sickleave, 1);
                                    }
                                }
                                else
                                {
                                    Dictionary<string, int> dict = new Dictionary<string, int>();
                                    dict.Add(sickleave, 1);
                                    dictionaryCount.Add(listEmplTypes[pair.Value.EmployeeID], dict);
                                }
                            }
                            else if (pair.Key == annual_leave)
                            {
                                dictionaryEmpl.Add(pair.Value.EmployeeID, pair.Value);

                                if (dictionaryCount.ContainsKey(listEmplTypes[pair.Value.EmployeeID]))
                                {
                                    if (dictionaryCount[listEmplTypes[pair.Value.EmployeeID]].ContainsKey(vacation))
                                    {
                                        dictionaryCount[listEmplTypes[pair.Value.EmployeeID]][vacation]++;
                                    }
                                    else
                                    {
                                        dictionaryCount[listEmplTypes[pair.Value.EmployeeID]].Add(vacation, 1);
                                    }
                                }
                                else
                                {
                                    Dictionary<string, int> dict = new Dictionary<string, int>();
                                    dict.Add(vacation, 1);
                                    dictionaryCount.Add(listEmplTypes[pair.Value.EmployeeID], dict);
                                }
                            }

                        }
                    }
                }



                DataTable table = new DataTable();
                table.Columns.Add("Category", typeof(string));
                table.Columns.Add("No.", typeof(string));
                table.Columns.Add(sickleave, typeof(string));
                table.Columns.Add("Accidents", typeof(string));
                table.Columns.Add("TotalSickleave", typeof(string));
                table.Columns.Add("RateSickleave", typeof(string));
                table.Columns.Add(vacation, typeof(string));
                table.Columns.Add("RateVacation", typeof(string));
                table.Columns.Add(paidLeave, typeof(string));
                table.Columns.Add("RatePaidleave", typeof(string));
                table.Columns.Add(absent, typeof(string));
                table.Columns.Add("RateAbsent", typeof(string));
                table.Columns.Add(active, typeof(string));
                table.Columns.Add("RateActive", typeof(string));
                table.Columns.Add("Remarks", typeof(string));

                ds.Tables.Add(table);
                ds.AcceptChanges();
                Dictionary<string, int> dictAll = new Dictionary<string, int>();
                foreach (KeyValuePair<int, EmployeeTypeTO> emplType in dictEmplType)
                {
                    DataRow row = table.NewRow();
               
                    row["Category"] = dictEmplType[emplType.Key].EmployeeTypeName;
                    if (!emplTypeCount.ContainsKey(emplType.Key))
                    {
                        row["No."] = "0";
                    }
                    else
                    {
                        row["No."] = emplTypeCount[emplType.Key];
                    }
                    if (dictAll.ContainsKey("No."))
                    {
                        dictAll["No."] += int.Parse(row["No."].ToString());
                    }
                    else
                    {
                        dictAll.Add("No.", int.Parse(row["No."].ToString()));
                    }
                    row[sickleave] = "0";
                    row["Accidents"] = "0";
                    row["TotalSickleave"] = "0";
                    row["RateSickleave"] = "0";
                    row[vacation] = "0";
                    row["RateVacation"] = "0";
                    row[paidLeave] = "0";
                    row["RatePaidleave"] = "0";
                    row[absent] = "0";
                    row["RateAbsent"] = "0";
                    row[active] = "0";
                    row["RateActive"] = "0";
                    row["Remarks"] = " ";
                    if (emplTypeCount.ContainsKey(emplType.Key))
                    {
                        if (dictionaryCount.ContainsKey(emplType.Key))
                        {

                            int num = 0;
                            int activeNum = 0;
                            foreach (KeyValuePair<string, int> pairValue in dictionaryCount[emplType.Key])
                            {
                                if (dictAll.ContainsKey(pairValue.Key))
                                {
                                    dictAll[pairValue.Key] += pairValue.Value;
                                }
                                else
                                {
                                    dictAll.Add(pairValue.Key, pairValue.Value);
                                }
                                if (pairValue.Key.Equals(sickleave))
                                {
                                    row[sickleave] = pairValue.Value;

                                    row["TotalSickleave"] = pairValue.Value;
                                    if (pairValue.Value != 0)
                                    {
                                        row["RateSickleave"] = Math.Round(((decimal)pairValue.Value * 100) / emplTypeCount[emplType.Key], 2);
                                    }
                                    num += pairValue.Value;
                                }
                                else if (pairValue.Key.Equals(vacation))
                                {
                                    row[vacation] = pairValue.Value;

                                    if (pairValue.Value != 0)
                                    {
                                        row["RateVacation"] = Math.Round(((decimal)pairValue.Value * 100) / emplTypeCount[emplType.Key], 2);
                                    }
                                    num += pairValue.Value;
                                }
                                else if (pairValue.Key.Equals(active))
                                {
                                    activeNum = pairValue.Value;
                                    row[active] = pairValue.Value;

                                    if (pairValue.Value != 0)
                                    {
                                        row["RateActive"] = Math.Round(((decimal)pairValue.Value * 100) / emplTypeCount[emplType.Key], 2);
                                    }

                                }
                            }
                            row[paidLeave] = (emplTypeCount[emplType.Key] - num - activeNum);
                            if (dictAll.ContainsKey(paidLeave))
                                dictAll[paidLeave] += int.Parse(row[paidLeave].ToString());
                            else
                                dictAll.Add(paidLeave, int.Parse(row[paidLeave].ToString()));

                            if ((emplTypeCount[emplType.Key] - num - activeNum) != 0)
                            {
                                row["RatePaidleave"] = Math.Round(((decimal)(emplTypeCount[emplType.Key] - num - activeNum) * 100) / emplTypeCount[emplType.Key], 2);
                            }

                            row[absent] = emplTypeCount[emplType.Key] - activeNum;
                            row["RateAbsent"] = Math.Round(((decimal)(emplTypeCount[emplType.Key] - activeNum) * 100) / (decimal)emplTypeCount[emplType.Key], 2);
                            if (dictAll.ContainsKey(absent))
                                dictAll[absent] += int.Parse(row[absent].ToString());
                            else
                                dictAll.Add(absent, int.Parse(row[absent].ToString()));

                        }
                        else
                        {
                            row[paidLeave] = (emplTypeCount[emplType.Key]);


                            if ((emplTypeCount[emplType.Key]) != 0)
                            {
                                row["RatePaidleave"] = Math.Round(((decimal)(emplTypeCount[emplType.Key]) * 100) / emplTypeCount[emplType.Key], 2);
                            }
                            row[absent] = emplTypeCount[emplType.Key];
                            row["RateAbsent"] = Math.Round(((decimal)(emplTypeCount[emplType.Key]) * 100) / (decimal)emplTypeCount[emplType.Key], 2);

                        }
                    }
                    table.Rows.Add(row);
                    table.AcceptChanges();
                }

                DataRow rowAll = table.NewRow();
                rowAll["Category"] = rm.GetString("CatAll", culture);
                rowAll[sickleave] = "0";
                rowAll["Accidents"] = "0";
                rowAll["TotalSickleave"] = "0";
                rowAll["RateSickleave"] = "0";
                rowAll[vacation] = "0";
                rowAll["RateVacation"] = "0";
                rowAll[paidLeave] = "0";
                rowAll["RatePaidleave"] = "0";
                rowAll[absent] = "0";
                rowAll["RateAbsent"] = "0";
                rowAll[active] = "0";
                rowAll["RateActive"] = "0";
                rowAll["Remarks"] = " ";

                if (dictAll.ContainsKey("No."))
                    rowAll["No."] = dictAll["No."];

                if (dictAll.ContainsKey(sickleave))
                    rowAll[sickleave] = dictAll[sickleave];

                rowAll["TotalSickleave"] = rowAll[sickleave];

                if (!rowAll[sickleave].Equals("0"))
                {
                    rowAll["RateSickleave"] = Math.Round((double.Parse(rowAll[sickleave].ToString()) * 100) / double.Parse(rowAll["No."].ToString()), 2);
                }
                if (dictAll.ContainsKey(vacation))
                    rowAll[vacation] = dictAll[vacation];
                if (!rowAll[vacation].Equals("0"))
                {
                    rowAll["RateVacation"] = Math.Round((double.Parse(rowAll[vacation].ToString()) * 100) / double.Parse(rowAll["No."].ToString()), 2);
                }
                if (dictAll.ContainsKey(paidLeave))
                    rowAll[paidLeave] = dictAll[paidLeave];
                if (!rowAll[paidLeave].Equals("0"))
                {
                    rowAll["RatePaidleave"] = Math.Round((double.Parse(rowAll[paidLeave].ToString()) * 100) / double.Parse(rowAll["No."].ToString()), 2);
                }
                if (dictAll.ContainsKey(absent))
                    rowAll[absent] = dictAll[absent];
                if (!rowAll[absent].Equals("0"))
                {
                    rowAll["RateAbsent"] = Math.Round((double.Parse(rowAll[absent].ToString()) * 100) / double.Parse(rowAll["No."].ToString()), 2);
                }
                if (dictAll.ContainsKey(active))
                    rowAll[active] = dictAll[active];
                if (!rowAll[active].Equals("0"))
                {
                    rowAll["RateActive"] = Math.Round((double.Parse(rowAll[active].ToString()) * 100) / double.Parse(rowAll["No."].ToString()), 2);
                }
                table.Rows.Add(rowAll);
                table.AcceptChanges();

                string[] columns = new string[table.Columns.Count];
                columns[0] = rm.GetString("category", culture);
                columns[1] = rm.GetString("numberEmpl", culture);
                columns[2] = rm.GetString("sickleave", culture);
                columns[3] = rm.GetString("accidents", culture);
                columns[4] = rm.GetString("total", culture);
                columns[5] = rm.GetString("rate", culture);
                columns[6] = rm.GetString("vacation", culture);
                columns[7] = rm.GetString("rate", culture);
                columns[8] = rm.GetString("paidAbsence", culture);
                columns[9] = rm.GetString("rate", culture);
                columns[10] = rm.GetString("absent", culture);
                columns[11] = rm.GetString("rate", culture);
                columns[12] = rm.GetString("active", culture);
                columns[13] = rm.GetString("rate", culture);
                columns[14] = rm.GetString("remarks", culture);

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = "DailyPresenceReport" + DateTime.Now.ToString("ddMMyyyy");
                sfd.InitialDirectory = Constants.csvDocPath;
                sfd.Filter = "XLSX (*.xlsx)|*.xlsx";

                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    this.Cursor = Cursors.Arrow;
                    return;
                }

                filePath = sfd.FileName;
                string selectedWU = "";
                if (rbOU.Checked)
                {
                    selectedWU = rm.GetString("selectedOU", culture) + " " + ((OrganizationalUnitTO)cbOU.SelectedItem).Name;
                }
                else
                {
                    selectedWU = rm.GetString("selectedWU", culture) + " " + ((WorkingUnitTO)cbWU.SelectedItem).Name;
                }
                ExportToExcel.CreateExcelDocument(ds, filePath, columns, selectedWU, rm.GetString("Date", culture) + " " + dtpDate.Value.Date.ToString("dd.MM.yyyy"));
                string Pathh = Directory.GetParent(filePath).FullName;


                this.Cursor = Cursors.Arrow;
                MessageBox.Show(rm.GetString("generateFinish", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " DailyPresence.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);

            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateOU()
        {
            try
            {
                List<OrganizationalUnitTO> ouArray = new List<OrganizationalUnitTO>();

                foreach (int id in oUnits.Keys)
                {
                    ouArray.Add(oUnits[id]);
                }

                ouArray.Insert(0, new OrganizationalUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), ""));

                cbOU.DataSource = ouArray;
                cbOU.DisplayMember = "Name";
                cbOU.ValueMember = "OrgUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " DailyPresence.populateOU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateWU()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    wuArray.Add(wuTO);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWU.DataSource = wuArray;
                cbWU.DisplayMember = "Name";
                cbWU.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " DailyPresence.populateWU(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void rbOU_CheckedChanged(object sender, EventArgs e)
        {
            int wuID = -1;

            if (!rbOU.Checked)
            {
                cbWU.Enabled = btnWUTree.Enabled = true;
                cbOU.Enabled = btnOUTree.Enabled = false;

                if (cbWU.SelectedIndex > 0)
                    wuID = (int)cbWU.SelectedValue;
            }
            else
            {
                cbOU.Enabled = btnOUTree.Enabled = true;
                cbWU.Enabled = btnWUTree.Enabled = false;

                if (cbOU.SelectedIndex > 0)
                    wuID = (int)cbOU.SelectedValue;
            }
        }

        private void rbWU_CheckedChanged(object sender, EventArgs e)
        {
            int wuID = -1;

            if (rbWU.Checked)
            {
                cbWU.Enabled = btnWUTree.Enabled = true;
                cbOU.Enabled = btnOUTree.Enabled = false;

                if (cbWU.SelectedIndex > 0)
                    wuID = (int)cbWU.SelectedValue;
            }
            else
            {
                cbOU.Enabled = btnOUTree.Enabled = true;
                cbWU.Enabled = btnWUTree.Enabled = false;

                if (cbOU.SelectedIndex > 0)
                    wuID = (int)cbOU.SelectedValue;
            }
        }

        private void btnWUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuString);
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    this.cbWU.SelectedIndex = cbWU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " DailyPresence.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnOUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsOrgUnits = new OrganizationalUnit().getOrganizationUnits(ouString);
                WorkingUnitsTreeView orgUnitsTreeView = new WorkingUnitsTreeView(dsOrgUnits);
                orgUnitsTreeView.ShowDialog();
                if (!orgUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbOU.SelectedIndex = cbOU.FindStringExact(orgUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " DailyPresence.btnOUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DailyPresence_Load(object sender, EventArgs e)
        {

            try
            {
                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                    oUnits = new ApplUserXOrgUnit().FindOUForUserDictionary(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                    wuList.Add(wUnit.WorkingUnitID);
                }

                if (wuString.Length > 0)
                    wuString = wuString.Substring(0, wuString.Length - 1);

                foreach (int id in oUnits.Keys)
                {
                    ouString += id.ToString().Trim() + ",";
                    ouList.Add(id);
                }

                if (ouString.Length > 0)
                    ouString = ouString.Substring(0, ouString.Length - 1);

                //menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                //currentRoles = NotificationController.GetCurrentRoles();

                //menuItemID = NotificationController.GetCurrentMenuItemID();
                //int index = menuItemID.LastIndexOf('_');

                //menuItemUsedID = menuItemID + "_" + rm.GetString("tpMealsUsed", culture);
                //setVisibility();


                populateWU();
                populateOU();

            }
            catch (Exception ex)
            {


                log.writeLog(DateTime.Now + " DailyPresence.DailyPresence_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
