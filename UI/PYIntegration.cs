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
using ACTAWorkAnalysisReports;

namespace UI
{
    public partial class PYIntegration : Form
    {
        // Language
        private CultureInfo culture;
        private ResourceManager rm;

        // Debug log
        DebugLog log;

        //data
        private List<int> wuIDList = new List<int>();
        private List<WorkingUnitTO> wUnits;
        private string wuString = "";

        private List<EmployeeTO> currentEmplArray = new List<EmployeeTO>();
        private Dictionary<int, EmployeeAsco4TO> ascoDict = new Dictionary<int, EmployeeAsco4TO>();
        
        Dictionary<int, PassTypeTO> typesDict = new Dictionary<int, PassTypeTO>();

        ApplUserTO logInUser;

        Dictionary<int, WorkingUnitTO> wuDict = new Dictionary<int, WorkingUnitTO>();

        int absentCompanySelected = -1;

        public PYIntegration()
        {
            InitializeComponent();

            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();

            // Set Language
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(PYIntegration).Assembly);
            setLanguage();

            // set date interval
            DateTime lastMonth = DateTime.Now.AddMonths(-1);

            dtFrom.Value = new DateTime(lastMonth.Year, lastMonth.Month, 1);
            dtTo.Value = dtFrom.Value.AddMonths(1).AddDays(-1);

            numBHBalanceMonth.Value = Constants.defaultBHBalanceMonth;
                        
            chbShowLeavingEmployees.Checked = false;
            dtpBorderDay.Value = DateTime.Now.Date;
            dtpBorderDay.Enabled = false;
            chbWorkAnalysisReport.Checked = true;
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("PYIntegration", culture);

                //label's text                
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblPath.Text = rm.GetString("lblPath", culture);
                lblBHPath.Text = "BH " + rm.GetString("lblPath", culture).ToLower();
                lblSWPath.Text = "SW " + rm.GetString("lblPath", culture).ToLower();
                lblCalcID.Text = rm.GetString("lblCalcID", culture);
                lblCalcIDBH.Text = rm.GetString("lblCalcID", culture);
                lblVacCalcID.Text = rm.GetString("lblCalcID", culture);
                lblSWCalcID.Text = rm.GetString("lblCalcID", culture);
                lblSWPayCalcID.Text = rm.GetString("lblCalcID", culture);
                lblLeavingEmployeesCalcID.Text = rm.GetString("lblCalcID", culture);
                lblCalcIDNJOvertime.Text = rm.GetString("lblCalcID", culture);
                lblBorderDate.Text = rm.GetString("lblBorderDate", culture);
                lblWUAbsent.Text = rm.GetString("lblWU", culture);
                lblPTAbsent.Text = rm.GetString("lblPassType", culture);
                lblCalcIDAbsent.Text = rm.GetString("lblCalcID", culture);
                lblBHBalanceMonth.Text = rm.GetString("lblBHBalanceMonth", culture);
                lblSWBalanceMonth.Text = rm.GetString("lblBHBalanceMonth", culture);

                //check box's text
                chbBHPayRegular.Text = rm.GetString("chbBankHours", culture);
                cbBankHoursPay.Text = rm.GetString("cbBankHoursPayFile", culture);
                cbSWPay.Text = rm.GetString("cbSWPayFile", culture);
                chbVacationPay.Text = rm.GetString("chbVacationPay", culture);
                chbStopWorkingPay.Text = rm.GetString("chbStopWorkingPay", culture);
                chbSWPayRegular.Text = rm.GetString("chbStopWorking", culture);
                chbBHPayment.Text = rm.GetString("chbPayment", culture);
                chbSWPayment.Text = rm.GetString("chbPayment", culture);
                chbShowLeavingEmployees.Text = rm.GetString("chbShowLeavingEmployees", culture);
                chbWorkAnalysisReport.Text = rm.GetString("chbWorkAnalysisReport", culture);

                //radio button's text
                rbAllEmployees.Text = rm.GetString("lblAll", culture);
                rbSelected.Text = rm.GetString("rbSelected", culture);
                rbEstimated.Text = rm.GetString("rbEstimated", culture);
                rbReal.Text = rm.GetString("rbReal", culture);

                //group box's text
                gbEmployees.Text = rm.GetString("gbEmployees", culture);
                gbType.Text = rm.GetString("gbType", culture);
                gbRestartOvertimeCounter.Text = rm.GetString("gbRestartOvertimeCounter", culture);
                gbBHCuttOffMonths.Text = rm.GetString("gbBHCuttOffMonths", culture);
                gbVacationCutOffMonth.Text = rm.GetString("gbVacationCuttOffMonths", culture);
                gbCompany.Text = rm.GetString("gbCompany", culture);
                gbDate.Text = rm.GetString("gbDate", culture);
                gbPaidLeaves.Text = rm.GetString("gbPaidLeaves", culture);
                gbSWPay.Text = rm.GetString("gbStopWorkingCutOffMonth", culture);
                gbStopWorkingCutOffMonth.Text = rm.GetString("gbStopWorkingPay", culture);
                gbNotJustifiedOverTimeCounter.Text = rm.GetString("gbNotJustifiedOverTimeCounter", culture);
                gbLeavingEmployees.Text = rm.GetString("gbLeavingEmployees", culture);
                gbAbsence.Text = rm.GetString("gbAbsent", culture);

                //tab page's text
                tpCreateReports.Text = rm.GetString("tpCreateReports", culture);
                tpRestarCounters.Text = rm.GetString("tpRestarCounters", culture);
                tabAdditionalReports.Text = rm.GetString("tabAdditionalReports", culture);

                //button's text
                btnBrowse.Text = rm.GetString("btnBrowse", culture);
                btnBHBrowse.Text = rm.GetString("btnBrowse", culture) + " BH";
                btnSWBrowse.Text = rm.GetString("btnBrowse", culture) + " SW";
                btnClose.Text = rm.GetString("btnClose", culture);
                btnGenerateData.Text = rm.GetString("btnGenerateData", culture);
                btnRecalculate.Text = rm.GetString("btnRecalculate", culture);
                btnRecalculateBH.Text = rm.GetString("btnRecalculate", culture);
                btnRecalculateVac.Text = rm.GetString("btnRecalculate", culture);
                btnRecalculatePaidLeaves.Text = rm.GetString("btnRecalculate", culture);
                btnRecalculateSW.Text = rm.GetString("btnRecalculate", culture);
                btnRecalculateSWPay.Text = rm.GetString("btnRecalculate", culture);
                btnRecalculateNJOvertime.Text = rm.GetString("btnRecalculate", culture);
                btnLeavingEmployeesRecalculate.Text = rm.GetString("btnRecalculate", culture);
                btnGenerateAbsent.Text = rm.GetString("btnGenerate", culture);

                // list view
                lvCompany.BeginUpdate();
                lvCompany.Columns.Add("", lvCompany.Width - 22, HorizontalAlignment.Left);
                lvCompany.EndUpdate();

                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrID", culture), lvEmployees.Width / 4 - 11, HorizontalAlignment.Left);                
                lvEmployees.Columns.Add(rm.GetString("hdrName", culture), (lvEmployees.Width / 4) * 3 - 11, HorizontalAlignment.Left);                
                lvEmployees.EndUpdate();

                lvPTAbsent.BeginUpdate();                
                lvPTAbsent.Columns.Add(rm.GetString("hdrName", culture), lvPTAbsent.Width - 21, HorizontalAlignment.Left);
                lvPTAbsent.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateCompanyList()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new WorkingUnit().getRootWorkingUnitsList(wuString);

                lvCompany.BeginUpdate();
                lvCompany.Items.Clear();

                foreach (WorkingUnitTO wu in wuArray)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = wu.Name;
                    item.Tag = wu;
                    lvCompany.Items.Add(item);
                }

                lvCompany.EndUpdate();                
                lvCompany.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.populateCompanyList(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PYIntegration_Load(object sender, EventArgs e)
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
                    wuIDList.Add(wUnit.WorkingUnitID);
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }
                typesDict = new PassType().SearchDictionary();

                //foreach (PassTypeTO passTypeTO in typesDict.Values)
                //{
                //    if (passTypeTO.PassTypeID == Constants.absence)
                //    {
                //        passTypeTO.PaymentCode = "0070";
                //    }
                //}

                populateCompanyList();

                tbPath.Text = Constants.RootDirectory;

                cbBankHoursPay_CheckedChanged(this, new EventArgs());

                cbSWPay_CheckedChanged(this, new EventArgs());

                chbBHPayRegular_CheckedChanged(this, new EventArgs());

                chbSWPayRegular_CheckedChanged(this, new EventArgs());

                ascoDict = new EmployeeAsco4().SearchDictionary("");

                populateWUList();
                cbWU.SelectedIndex = cbWU.FindStringExact("148");
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.PYIntegration_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Populate Employee Combo Box
        /// </summary>
        private void populateEmployees()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<int> wuIDList = new List<int>();
                foreach (ListViewItem item in lvCompany.SelectedItems)
                {
                    wuIDList.Add(((WorkingUnitTO)item.Tag).WorkingUnitID);
                }

                bool validData = true;
                if (dtFrom.Value.Date > dtTo.Value.Date)
                {
                    validData = false;
                    MessageBox.Show(rm.GetString("endDateBeforeStart", culture));
                }
                if (validData && !validateBorderDay())
                {
                    validData = false;
                    MessageBox.Show(rm.GetString("notValidBorderDay", culture));                    
                }

                currentEmplArray = new List<EmployeeTO>();
                List<EmployeeTO> emplList = new List<EmployeeTO>();

                if (validData)
                {
                    foreach (int wuID in wuIDList)
                    {
                        string workUnitID = wuID.ToString();
                        if (wuID == -1)
                        {
                            emplList.AddRange(new Employee().SearchByWU(wuString));
                        }
                        else
                        {
                            List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                            WorkingUnit workUnit = new WorkingUnit();
                            foreach (WorkingUnitTO workingUnit in wUnits)
                            {
                                if (workingUnit.WorkingUnitID == wuID)
                                {
                                    wuList.Add(workingUnit);
                                    workUnit.WUTO = workingUnit;
                                }
                            }
                            if (workUnit.WUTO.ChildWUNumber > 0)
                                wuList = workUnit.FindAllChildren(wuList);
                            workUnitID = "";
                            foreach (WorkingUnitTO wunit in wuList)
                            {
                                workUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                            }

                            if (workUnitID.Length > 0)
                            {
                                workUnitID = workUnitID.Substring(0, workUnitID.Length - 1);
                            }

                            emplList.AddRange(new Employee().SearchByWU(workUnitID));
                        }
                    }
                }                                
                
                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();

                foreach (EmployeeTO empl in emplList)
                {
                    // employee should be added in employee list and in list preview if it is ACTIVE or retired date is less or equal to period end
                    // if show living employees is checked, then show only RETIRED employees whose retired date is between interval start and border day
                    if (chbShowLeavingEmployees.Checked)
                    {
                        if (empl.Status.Trim().ToUpper().Equals(Constants.statusRetired.Trim().ToUpper()) && ascoDict.ContainsKey(empl.EmployeeID)
                        && ascoDict[empl.EmployeeID].DatetimeValue3.Date > dtFrom.Value.Date 
                        && ascoDict[empl.EmployeeID].DatetimeValue3.Date <= dtpBorderDay.Value.AddDays(1).Date)
                        {
                            currentEmplArray.Add(empl);                            
                        }
                        else
                            continue;
                    }
                    else if (ascoDict.ContainsKey(empl.EmployeeID) 
                        && (empl.Status.Trim().ToUpper().Equals(Constants.statusActive.Trim().ToUpper())
                        && ascoDict[empl.EmployeeID].DatetimeValue2.Date <= dtTo.Value.Date)
                        || (empl.Status.Trim().ToUpper().Equals(Constants.statusRetired.Trim().ToUpper())
                        && ascoDict[empl.EmployeeID].DatetimeValue3.Date > dtFrom.Value.Date))
                    {
                        currentEmplArray.Add(empl);
                    }
                    else
                        continue;

                    ListViewItem item = new ListViewItem();
                    item.Text = empl.EmployeeID.ToString().Trim();
                    item.SubItems.Add(empl.FirstAndLastName);

                    item.Tag = empl;
                    lvEmployees.Items.Add(item);
                }

                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
            finally 
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbDialog = new FolderBrowserDialog();
                fbDialog.SelectedPath = tbPath.Text.ToString();

                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    this.tbPath.Text = fbDialog.SelectedPath.ToString();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.btnBrowse_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
                
        private void btnGenerateData_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<EmployeeTO> selectedEmployees = new List<EmployeeTO>();

                if (rbAllEmployees.Checked)
                    selectedEmployees = currentEmplArray;
                else
                {
                    if (lvEmployees.SelectedItems.Count <= 0)
                    {
                        MessageBox.Show(rm.GetString("selectEmployees", culture));
                        return;
                    }

                    foreach (ListViewItem item in lvEmployees.SelectedItems)
                    {
                        selectedEmployees.Add((EmployeeTO)item.Tag);
                    }
                }

                if (selectedEmployees.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }
                
                string type = "";
                if (rbEstimated.Checked)
                    type = Constants.PYTypeEstimated;
                else
                    type = Constants.PYTypeReal;

                // check file for bh payment
                Dictionary<int, Dictionary<DateTime, decimal>> emplBHPayHours = new Dictionary<int, Dictionary<DateTime, decimal>>();
                if (cbBankHoursPay.Checked)
                {
                    char delimiter = '\t';

                    if (!File.Exists(tbBHPath.Text.Trim()))
                    {
                        MessageBox.Show(rm.GetString("enterBHFilePath", culture));
                        return;
                    }

                    Dictionary<int, List<string>> invalidRows = new Dictionary<int, List<string>>();
                    Dictionary<int, EmployeeTO> invalidEmployees = new Dictionary<int, EmployeeTO>();
                    // make list of items from file rows                       
                    FileStream str = File.Open(tbBHPath.Text.Trim(), System.IO.FileMode.Open);
                    StreamReader reader = new StreamReader(str);

                    // read file lines, skip header
                    string line = reader.ReadLine(); // header
                    line = reader.ReadLine(); // first row
                    while (line != null)
                    {
                        if (!line.Trim().Equals(""))
                        {
                            string[] rowItems = line.Split(delimiter);
                            if (rowItems.Length >= 8)
                            {
                                int emplID = -1;
                                decimal bhPay = 0;
                                decimal bhBalance = 0;
                                DateTime month = new DateTime();

                                if (!int.TryParse(rowItems[0].ToString().Trim(), out emplID))
                                    emplID = -1;
                                if (!decimal.TryParse(rowItems[7].ToString().Trim(), out bhPay))
                                    bhPay = 0;
                                if (!decimal.TryParse(rowItems[6].ToString().Trim(), out bhBalance))
                                    bhBalance = 0;
                                if (!DateTime.TryParseExact("01." + rowItems[2].ToString().Trim(), Constants.dateFormat, null, DateTimeStyles.None, out month))
                                    month = new DateTime();

                                if (emplID == -1 || month == new DateTime() || bhPay < 0 || (bhPay > 0 && bhPay > bhBalance))
                                {
                                    if (!invalidRows.ContainsKey(emplID))
                                        invalidRows.Add(emplID, new List<string>());

                                    if (!invalidEmployees.ContainsKey(emplID))
                                    {
                                        EmployeeTO invalidEmpl = new EmployeeTO();
                                        invalidEmpl.EmployeeID = emplID;
                                        invalidEmpl.FirstName = emplID.ToString().Trim();

                                        invalidEmployees.Add(emplID, invalidEmpl);
                                    }

                                    if (emplID == -1)
                                        invalidRows[emplID].Add("INVALID EMPLOYEE " + line);
                                    
                                    if (month == new DateTime())
                                        invalidRows[emplID].Add("INVALID MONTH " + line);
                                    
                                    if (bhPay < 0 || bhPay > bhBalance)
                                        invalidRows[emplID].Add("INVALID BH PAYMENT HOURS " + line);                                    
                                }
                                else if (bhPay > 0)
                                {
                                    if (!emplBHPayHours.ContainsKey(emplID))
                                        emplBHPayHours.Add(emplID, new Dictionary<DateTime, decimal>());

                                    if (!emplBHPayHours[emplID].ContainsKey(month.Date))
                                        emplBHPayHours[emplID].Add(month.Date, bhPay);
                                }
                            }
                        }

                        line = reader.ReadLine();
                    }

                    reader.Close();
                    str.Dispose();
                    str.Close();

                    if (type == Constants.PYTypeReal && invalidRows.Count > 0)
                    {
                        UnregularDataPreview preview = new UnregularDataPreview(invalidRows, invalidEmployees);
                        preview.ShowDialog();
                        log.writeLog(this.ToString() + ".btnGenerateData_Click(): unregular data found");
                        return;
                    }
                }

                // check file for sw payment
                Dictionary<int, Dictionary<DateTime, decimal>> emplSWPayHours = new Dictionary<int, Dictionary<DateTime, decimal>>();
                if (cbSWPay.Checked)
                {
                    char delimiter = '\t';

                    if (!File.Exists(tbSWPath.Text.Trim()))
                    {
                        MessageBox.Show(rm.GetString("enterSWFilePath", culture));
                        return;
                    }

                    Dictionary<int, List<string>> invalidRows = new Dictionary<int, List<string>>();
                    Dictionary<int, EmployeeTO> invalidEmployees = new Dictionary<int, EmployeeTO>();
                    // make list of items from file rows                       
                    FileStream str = File.Open(tbSWPath.Text.Trim(), System.IO.FileMode.Open);
                    StreamReader reader = new StreamReader(str);

                    // read file lines, skip header
                    string line = reader.ReadLine(); // header
                    line = reader.ReadLine(); // first row
                    while (line != null)
                    {
                        if (!line.Trim().Equals(""))
                        {
                            string[] rowItems = line.Split(delimiter);
                            if (rowItems.Length >= 8)
                            {
                                int emplID = -1;
                                decimal swPay = 0;
                                decimal swBalance = 0;
                                DateTime month = new DateTime();

                                if (!int.TryParse(rowItems[0].ToString().Trim(), out emplID))
                                    emplID = -1;
                                if (!decimal.TryParse(rowItems[7].ToString().Trim(), out swPay))
                                    swPay = 0;
                                if (!decimal.TryParse(rowItems[6].ToString().Trim(), out swBalance))
                                    swBalance = 0;
                                if (!DateTime.TryParseExact("01." + rowItems[2].ToString().Trim(), Constants.dateFormat, null, DateTimeStyles.None, out month))
                                    month = new DateTime();

                                if (emplID == -1 || month == new DateTime() || swPay < 0 || (swPay > 0 && swPay >swBalance))
                                {
                                    if (!invalidRows.ContainsKey(emplID))
                                        invalidRows.Add(emplID, new List<string>());

                                    if (!invalidEmployees.ContainsKey(emplID))
                                    {
                                        EmployeeTO invalidEmpl = new EmployeeTO();
                                        invalidEmpl.EmployeeID = emplID;
                                        invalidEmpl.FirstName = emplID.ToString().Trim();

                                        invalidEmployees.Add(emplID, invalidEmpl);
                                    }

                                    if (emplID == -1)
                                        invalidRows[emplID].Add("INVALID EMPLOYEE " + line);

                                    if (month == new DateTime())
                                        invalidRows[emplID].Add("INVALID MONTH " + line);

                                    if (swPay < 0 || swPay > swBalance)
                                        invalidRows[emplID].Add("INVALID SW PAYMENT HOURS " + line);
                                }
                                else if (swPay > 0)
                                {
                                    if (!emplSWPayHours.ContainsKey(emplID))
                                        emplSWPayHours.Add(emplID, new Dictionary<DateTime, decimal>());

                                    if (!emplSWPayHours[emplID].ContainsKey(month.Date))
                                        emplSWPayHours[emplID].Add(month.Date, swPay);
                                }
                            }
                        }

                        line = reader.ReadLine();
                    }

                    reader.Close();
                    str.Dispose();
                    str.Close();

                    if (type == Constants.PYTypeReal && invalidRows.Count > 0)
                    {
                        UnregularDataPreview preview = new UnregularDataPreview(invalidRows, invalidEmployees);
                        preview.ShowDialog();
                        log.writeLog(this.ToString() + ".btnGenerateData_Click(): unregular data found");
                        return;
                    }
                }

                //WorkingUnitTO company = cbCompany.SelectedItem as WorkingUnitTO;

                // check if data for generating work analysis report are correct
                if (chbWorkAnalysisReport.Checked)
                {
                    // check if whole last month payroll is generated, else, do not allow generating work analysis reports
                    DateTime firstDayLastMonth = new DateTime(DateTime.Now.Date.AddMonths(-1).Year, DateTime.Now.Date.AddMonths(-1).Month, 1);
                    DateTime lastDayLastMonth = firstDayLastMonth.AddMonths(1).AddDays(-1);

                    if (chbShowLeavingEmployees.Checked || dtFrom.Value.Date != firstDayLastMonth.Date || dtTo.Value.Date != lastDayLastMonth.Date)
                    {
                        DialogResult result = MessageBox.Show(rm.GetString("workAnalysisForLeavingEmployeesNotLastMonth", culture), "", MessageBoxButtons.YesNo);

                        if (result == DialogResult.No)
                            chbWorkAnalysisReport.Checked = false;
                    }
                }
                                
                string employeeString = "";
                foreach (EmployeeTO empl in selectedEmployees)
                {
                    employeeString += empl.EmployeeID.ToString() + ", ";
                }
                if (employeeString.Length > 0)
                    employeeString = employeeString.Substring(0, employeeString.Length - 2);
                                
                List<string> analiticStrings = new List<string>();
                List<string> sumStrings = new List<string>();
                List<string> bufferStrings = new List<string>();
                List<string> bufferExpatStrings = new List<string>();
                Dictionary<int, OnlineMealsTypesTO> mealTypesDict = new Dictionary<int,OnlineMealsTypesTO>();
                bool unRegularDataFound = false;
                Dictionary<int, List<string>> unregularEmployees = new Dictionary<int,List<string>>();
                Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
                uint calcID = Common.Misc.GenerateFiatPYData(employeeString, emplDict, dtFrom.Value.Date, dtTo.Value.Date, type, analiticStrings, bufferStrings, bufferExpatStrings, sumStrings, mealTypesDict,
                     ref unRegularDataFound, unregularEmployees, emplBHPayHours, emplSWPayHours, chbVacationPay.Checked, cbBankHoursPay.Checked, chbBHPayRegular.Checked, (int)numBHBalanceMonth.Value, chbBHPayment.Checked, 
                     cbSWPay.Checked, chbStopWorkingPay.Checked, chbSWPayRegular.Checked, (int)numSWBalanceMonth.Value, chbSWPayment.Checked, chbShowLeavingEmployees.Checked);

                if (type == Constants.PYTypeReal && unRegularDataFound)
                {
                    foreach (int id in unregularEmployees.Keys)
                    {
                        for (int i = 0; i < unregularEmployees[id].Count; i++)
                        {
                            unregularEmployees[id][i] = rm.GetString(unregularEmployees[id][i], culture);
                        }
                    }
                    UnregularDataPreview preview = new UnregularDataPreview(unregularEmployees, emplDict);
                    preview.ShowDialog();
                    log.writeLog(this.ToString() + ".btnGenerateData_Click(): unregular data found");
                }
                else if (calcID == 0)
                {
                    MessageBox.Show(rm.GetString("reportGenerateFaild", culture));
                }
                else
                {
                    //string filePath = tbPath.Text.ToString() + "\\employee_py_data_sum_" + calcID + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                    string filePath = tbPath.Text.ToString() + "\\PY_SYNC_SDD General Data_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    StreamWriter writer = new StreamWriter(filePath, true, Encoding.Unicode);
                    writer.WriteLine("py_calc_id\tcompany_code\ttype\temployee_id\tdate_start\tdate_end\tpayment_code\thrs_amount\temployee_type\tcost_center_name\tcost_center_desc\tdate_start_sickness");
                    foreach (string row in sumStrings)
                    {
                        writer.WriteLine(row);
                    }
                    writer.Close();

                    // 01.02.203. Sanja - if pay of stop working is checked save just sum records with stop working pay code
                    if (!chbStopWorkingPay.Checked)
                    {
                        filePath = tbPath.Text.ToString() + "\\PY_SYNC_SDD Detailed Data_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        writer = new StreamWriter(filePath, true, Encoding.Unicode);
                        writer.WriteLine("py_calc_id\tcompany_code\ttype\temployee_id\tdate_start\tpayment_code\thrs_amount\temployee_type\tcost_center_name\tcost_center_desc\tdate_start_sickness");
                        foreach (string row in analiticStrings)
                        {
                            writer.WriteLine(row);
                        }
                        writer.Close();

                        filePath = tbPath.Text.ToString() + "\\PY_SYNC_SDD Buffer Data_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        writer = new StreamWriter(filePath, true, Encoding.Unicode);
                        string header = "py_calc_id\tcompany_code\ttype\temployee_id\tfirst_name\tlast_name\tdate_start\tdate_end\tfund_hrs\tfund_day\tfund_hrs_est\tfund_day_est\tmeal_counter\ttransport_counter\t"
                            + "vacation_left_curr_year\tvacation_left_prev_year\tvacation_used_curr_year\tvacation_used_prev_year\tbank_hrs_balans\tstop_working_hrs_balans\t"
                            + "paid_leave_balans\tpaid_leave_used\temployee_type\tcost_center_name\tcost_center_desc\t";

                        foreach (int mType in mealTypesDict.Keys)
                        {
                            header += mealTypesDict[mType].Name.Trim().Replace(" ", "_") + "_approved\t";
                            header += mealTypesDict[mType].Name.Trim().Replace(" ", "_") + "_not_approved\t";
                        }

                        header += "not_justified_overtime_balans\tnot_justified_overtime\t";

                        writer.WriteLine(header);
                        foreach (string row in bufferStrings)
                        {
                            writer.WriteLine(row);
                        }
                        writer.Close();

                        filePath = tbPath.Text.ToString() + "\\PY_SYNC_SDD Buffer Data_Expat_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        writer = new StreamWriter(filePath, true, Encoding.Unicode);
                        header = "py_calc_id\tcompany_code\ttype\temployee_id\tfirst_name\tlast_name\tdate_start\tdate_end\tfund_hrs\tfund_day\tfund_hrs_est\tfund_day_est\tmeal_counter\ttransport_counter\t"
                            + "vacation_left_curr_year\tvacation_left_prev_year\tvacation_used_curr_year\tvacation_used_prev_year\tbank_hrs_balans\tstop_working_hrs_balans\t"
                            + "paid_leave_balans\tpaid_leave_used\temployee_type\tcost_center_name\tcost_center_desc\t";

                        foreach (int mType in mealTypesDict.Keys)
                        {
                            header += mealTypesDict[mType].Name.Trim().Replace(" ", "_") + "_approved\t";
                            header += mealTypesDict[mType].Name.Trim().Replace(" ", "_") + "_not_approved\t";
                        }

                        header += "not_justified_overtime_balans\tnot_justified_overtime\t";

                        writer.WriteLine(header);
                        foreach (string row in bufferExpatStrings)
                        {
                            writer.WriteLine(row);
                        }
                        writer.Close();
                    }

                    // generate work analysis reports
                    if (chbWorkAnalysisReport.Checked)
                    {
                        // create dates list
                        List<DateTime> dateList = new List<DateTime>();
                        DateTime currDate = dtFrom.Value.Date;
                        while (currDate.Date <= dtTo.Value.Date)
                        {
                            dateList.Add(currDate.Date);
                            currDate = currDate.AddDays(1);
                        }

                        Dictionary<int, string> dictionaryCompaniesNames = new Dictionary<int, string>();
                        foreach (int comp in Enum.GetValues(typeof(Constants.FiatCompanies)))
                        {
                            string name = Enum.GetName(typeof(Constants.FiatCompanies), comp);
                            dictionaryCompaniesNames.Add(comp, name);
                        }

                        Dictionary<int, string> reportFilePaths = Constants.FiatSharedAreaWAReports;
                        Dictionary<int, string> anomalyReportFilePaths = Constants.FiatSharedAreaWAAnomaliesReports;
                        // get path and generate report for each company
                        foreach (ListViewItem compItem in lvCompany.SelectedItems)
                        {
                            filePath = "";
                            WorkingUnitTO company = (WorkingUnitTO)compItem.Tag;
                            if (reportFilePaths.ContainsKey(company.WorkingUnitID))
                            {
                                filePath = reportFilePaths[company.WorkingUnitID];

                                // check if path exists
                                if (!filePath.Trim().Equals("") && !Directory.Exists(filePath))
                                    filePath = "";
                            }

                            string compName = "";
                            if (dictionaryCompaniesNames.ContainsKey(company.WorkingUnitID))
                                compName = dictionaryCompaniesNames[company.WorkingUnitID];

                            if (filePath.Trim().Equals(""))
                            {
                                FolderBrowserDialog fbDialog = new FolderBrowserDialog();
                                fbDialog.Description = compName.Trim() + " Work Analysis reports path";

                                if (fbDialog.ShowDialog() == DialogResult.OK)
                                    filePath = fbDialog.SelectedPath;
                                else
                                    continue;
                            }

                            filePath += "\\" + dtFrom.Value.Year.ToString().Trim();

                            if (!Directory.Exists(filePath))
                                Directory.CreateDirectory(filePath);

                            filePath += "\\" + dtFrom.Value.Month.ToString().Trim();

                            if (!Directory.Exists(filePath))
                                Directory.CreateDirectory(filePath);

                            filePath += "\\" + DateTime.Now.Date.ToString("yyyyMMdd");

                            int counter = 0;
                            string pathAdd = "";
                            while (Directory.Exists(filePath + pathAdd))
                            {
                                counter++;
                                pathAdd = "_" + counter.ToString().Trim();
                            }

                            filePath += pathAdd;

                            Directory.CreateDirectory(filePath);

                            string anomalyFilePath = "";
                            if (anomalyReportFilePaths.ContainsKey(company.WorkingUnitID))
                            {
                                anomalyFilePath = anomalyReportFilePaths[company.WorkingUnitID];

                                // check if path exists
                                if (!anomalyFilePath.Trim().Equals("") && !Directory.Exists(anomalyFilePath))
                                    anomalyFilePath = "";
                            }

                            if (anomalyFilePath.Trim().Equals(""))
                            {
                                FolderBrowserDialog fbDialog = new FolderBrowserDialog();
                                fbDialog.Description = compName.Trim() + " Work Analysis anomalies reports path";

                                if (fbDialog.ShowDialog() == DialogResult.OK)
                                    anomalyFilePath = fbDialog.SelectedPath;
                                else
                                    continue;
                            }

                            anomalyFilePath += "\\" + dtFrom.Value.Year.ToString().Trim() + "_" + dtFrom.Value.Month.ToString().Trim().PadLeft(2, '0');

                            if (!Directory.Exists(anomalyFilePath))
                                Directory.CreateDirectory(anomalyFilePath);

                            anomalyFilePath += "\\PY EXPORT_" + DateTime.Now.Date.ToString("yyyyMMdd");

                            counter = 0;
                            pathAdd = "";
                            while (Directory.Exists(anomalyFilePath + pathAdd))
                            {
                                counter++;
                                pathAdd = "_" + counter.ToString().Trim();
                            }

                            anomalyFilePath += pathAdd;

                            Directory.CreateDirectory(anomalyFilePath);

                            // get list of plants                                        
                            List<WorkingUnitTO> plantList = new WorkingUnit().SearchChildWU(company.WorkingUnitID.ToString());

                            // generate all reports
                            new Report400().GenerateReport(null, filePath + "\\400_" + compName.Trim() + "_Month_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx",
                                plantList, dateList, company.WorkingUnitID, calcID);
                            new Report500().GenerateReport(null, dateList, filePath + "\\500_" + compName.Trim() + "_Month_" + DateTime.Now.ToString("yyyy_MM_dd")
                                + ".xlsx", plantList, company.WorkingUnitID, calcID);
                            new ReportAnomalies().GenerateReport(dateList, anomalyFilePath + "\\TM_Anomalies_" + compName.Trim() + "_"
                                + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx", plantList, company.WorkingUnitID);
                            new ReportWageTypes().GenerateReport(null, filePath + "\\Wage_Type_" + compName.Trim()
                                + "_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx", plantList, company.WorkingUnitID,
                                !NotificationController.GetLanguage().Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()), dateList);
                        }
                    }

                    if (type == Constants.PYTypeReal)
                    {
                        numAbsentCalcID.Value = calcID;
                        //btnGenerateAbsent_Click(this, new EventArgs());
                    }

                    MessageBox.Show(rm.GetString("reportGenerateSucc", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.btnGenerateData_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void tbEmployees_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string emplName = tbEmployee.Text.Trim().ToUpper();
                int emplID = -1;
                if (!int.TryParse(emplName, out emplID))
                    emplID = -1;

                lvEmployees.SelectedItems.Clear();

                if (emplName.Trim().Equals(""))
                {
                    tbEmployee.Focus();
                    return;
                }
                
                foreach (ListViewItem item in lvEmployees.Items)
                {
                    if ((emplID != -1 && ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim().ToUpper().StartsWith(emplID.ToString().Trim().ToUpper()))
                        || (emplID == -1 && ((EmployeeTO)item.Tag).FirstAndLastName.Trim().ToUpper().StartsWith(emplName.Trim().ToUpper())))
                    {
                        item.Selected = true;
                        lvEmployees.Select();                        
                        lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));
                        lvEmployees.Invalidate();
                        tbEmployee.Focus();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.tbEmployees_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbSelected_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!rbSelected.Checked)
                {
                    tbEmployee.Text = "";
                    lvEmployees.SelectedItems.Clear();
                    lvEmployees.Invalidate();
                }

                lblEmployee.Enabled = lvEmployees.Enabled = tbEmployee.Enabled = rbSelected.Checked;
                chbWorkAnalysisReport.Checked = !rbSelected.Checked;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.rbSelected_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRecalculate_Click(object sender, EventArgs e)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;

                if (nudCalcID.Value <= 0)
                {
                    MessageBox.Show(rm.GetString("enterCalcID", culture));
                }
                else
                {
                    Common.Rule rule = new Common.Rule();
                    rule.RuleTO.RuleType = Constants.RuleCompanyOvertimePaid;
                    List<RuleTO> rulesList = rule.Search();
                    EmployeePYDataSum emplSum = new EmployeePYDataSum();
                    emplSum.EmplSum.PYCalcID = Convert.ToUInt32(nudCalcID.Value);
                    List<EmployeePYDataSumTO> list = new List<EmployeePYDataSumTO>();
                    List<string> paymentCodes = new List<string>();
                    foreach (RuleTO ruleTO in rulesList)
                    {
                        if (typesDict.ContainsKey(ruleTO.RuleValue))
                        {
                            if (!paymentCodes.Contains(typesDict[ruleTO.RuleValue].PaymentCode))
                            {
                                paymentCodes.Add(typesDict[ruleTO.RuleValue].PaymentCode);
                            }
                        }
                    }

                    foreach (string pCode in paymentCodes)
                    {
                        emplSum.EmplSum.PaymentCode = pCode;
                        list.AddRange(emplSum.getEmployeesSum());
                    }

                    string employeeIDString = "";

                    foreach (EmployeePYDataSumTO sum in list)
                    {
                        employeeIDString += sum.EmployeeID.ToString() + ", ";
                    }
                    if (employeeIDString.Length > 0)
                        employeeIDString = employeeIDString.Substring(0, employeeIDString.Length - 2);

                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounterValues = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
                    emplCounterValues = new EmployeeCounterValue().SearchValues(employeeIDString);
                    EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();
                    EmployeeCounterValue counter = new EmployeeCounterValue();
                    bool trans = counterHist.BeginTransaction();
                    if (trans)
                    {
                        try
                        {
                            counter.SetTransaction(counterHist.GetTransaction());
                            bool saved = true;
                            // move to hist table
                            foreach (EmployeePYDataSumTO sum in list)
                            {
                                if (sum.HrsAmount > 0)
                                {
                                    if (emplCounterValues.ContainsKey(sum.EmployeeID) && emplCounterValues[sum.EmployeeID].ContainsKey((int)Constants.EmplCounterTypes.OvertimeCounter))
                                    {
                                        int newValue = emplCounterValues[sum.EmployeeID][(int)Constants.EmplCounterTypes.OvertimeCounter].Value;
                                        newValue -= (int)(sum.HrsAmount * 60);

                                        counterHist.ValueTO = new EmployeeCounterValueHistTO(emplCounterValues[sum.EmployeeID][(int)Constants.EmplCounterTypes.OvertimeCounter]);
                                        counterHist.ValueTO.ModifiedBy = Constants.payRollUser;
                                        saved = saved && (counterHist.Save(false) >= 0);

                                        if (!saved)
                                            break;

                                        counter.ValueTO = new EmployeeCounterValueTO();
                                        counter.ValueTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.OvertimeCounter;
                                        counter.ValueTO.EmplID = sum.EmployeeID;
                                        counter.ValueTO.Value = newValue;
                                        counter.ValueTO.ModifiedBy = Constants.payRollUser;

                                        saved = saved && counter.Update(false);
                                        if (!saved)
                                            break;

                                    }
                                }
                            }
                            if (!saved)
                            {
                                if (counterHist.GetTransaction() != null)
                                    counterHist.RollbackTransaction();
                                MessageBox.Show(rm.GetString("CountersUpdateFaild", culture));
                            }
                            else
                            {
                                counterHist.CommitTransaction();
                                MessageBox.Show(rm.GetString("CountersUpdateSucc", culture));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (counterHist.GetTransaction() != null)
                                counterHist.RollbackTransaction();
                            throw new Exception(".manualPairCounters() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
                        }

                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.btnRecalculate_Click(): " + ex.Message + "\n");
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

                if (nudBHCalcID.Value <= 0)
                {
                    MessageBox.Show(rm.GetString("enterCalcID", culture));
                }
                else
                {
                    // first, prepare data for recalculation counters for paid bank hours
                    Common.Rule rule = new Common.Rule();                    
                    
                    EmployeePYDataSum emplSum = new EmployeePYDataSum();
                    uint calcID = Convert.ToUInt32(nudBHCalcID.Value);

                    DateTime sumMonth = emplSum.getSumMonth(calcID).Date;                                                         
                    
                    Dictionary<int, Dictionary<DateTime, Dictionary<int, BufferMonthlyBalancePaidTO>>> dict = new BufferMonthlyBalancePaid().SearchEmployeeBalancesPaid(calcID, (int)Constants.EmplCounterTypes.BankHoursCounter);
                    
                    string employeeIDString = "";
                                        
                    foreach (int id in dict.Keys)
                    {
                        employeeIDString += id.ToString() + ",";
                    }

                    if (employeeIDString.Length > 0)
                        employeeIDString = employeeIDString.Substring(0, employeeIDString.Length - 1);

                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounterValues = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();                    
                    if (employeeIDString.Length > 0)
                        emplCounterValues = new EmployeeCounterValue().SearchValues(employeeIDString);                    

                    // 30.07.2013. Sanja
                    // second, recalculate bank hours balances
                    // balance record for particular month contains earned and spent bank hours of that month and current month balance
                    // negative balances are increased and moved to next month and they are not payed
                    // positive balances are decreased                    
                    List<int> earnedList = rule.SearchRulesExact(Constants.RuleCompanyBankHourMonthly);
                    List<string> pcEarnedHours = new List<string>();
                    foreach (int type in earnedList)
                    {
                        if (typesDict.ContainsKey(type) && !pcEarnedHours.Contains(typesDict[type].PaymentCode))
                            pcEarnedHours.Add(typesDict[type].PaymentCode);
                    }

                    List<int> usedList = rule.SearchRulesExact(Constants.RuleCompanyBankHourUsed);
                    List<string> pcUsedHours = new List<string>();
                    foreach (int type in usedList)
                    {
                        if (typesDict.ContainsKey(type) && !pcUsedHours.Contains(typesDict[type].PaymentCode))
                            pcUsedHours.Add(typesDict[type].PaymentCode);
                    }

                    string codes = "";
                    foreach (string pCode in pcEarnedHours)
                    {
                        codes += "'" + pCode + "',";
                    }

                    foreach (string pCode in pcUsedHours)
                    {
                        codes += "'" + pCode + "',";
                    }

                    if (codes.Length > 0)
                        codes = codes.Substring(0, codes.Length - 1);

                    Dictionary<int, Dictionary<string, decimal>> emplSumDict = emplSum.getEmployeesSumValues(calcID, codes);
                    List<int> emplList = emplSum.getEmployees(calcID);

                    string emplIDs = "";
                    foreach (int id in emplList)
                    {
                        emplIDs += id.ToString().Trim() + ",";
                    }

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                    DateTime payedMonth = new EmployeeCounterMonthlyBalance().GetMinPositiveMonth(emplIDs).AddMonths(-1).Date;

                    Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>> emplBalances = new EmployeeCounterMonthlyBalance().SearchEmployeeBalances(emplIDs.Trim(), payedMonth, false);
                    
                    EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();
                    EmployeeCounterValue counter = new EmployeeCounterValue();
                    EmployeeCounterMonthlyBalance balance = new EmployeeCounterMonthlyBalance();
                    EmployeeCounterMonthlyBalanceHist balanceHist = new EmployeeCounterMonthlyBalanceHist();
                    BufferMonthlyBalancePaid balancePaid = new BufferMonthlyBalancePaid();
                    bool saved = true;
                    if (counterHist.BeginTransaction())
                    {
                        try
                        {
                            counter.SetTransaction(counterHist.GetTransaction());
                            balanceHist.SetTransaction(counterHist.GetTransaction());
                            balance.SetTransaction(counterHist.GetTransaction());
                            balancePaid.SetTransaction(counterHist.GetTransaction());

                            // reacalculate counters and balances for paid bank hours
                            foreach (int id in dict.Keys)
                            {
                                int bhPaidHours = 0;

                                foreach (DateTime month in dict[id].Keys)
                                {
                                    if (dict[id][month].ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter)
                                        && emplBalances.ContainsKey(id) && emplBalances[id].ContainsKey(month) && emplBalances[id][month].ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                                    {
                                        bhPaidHours += dict[id][month][(int)Constants.EmplCounterTypes.BankHoursCounter].ValuePaid;

                                        // empty payed month balances                                                                            
                                        balanceHist.BalanceTO = new EmployeeCounterMonthlyBalanceTO(emplBalances[id][month][(int)Constants.EmplCounterTypes.BankHoursCounter]);
                                        balanceHist.BalanceTO.ModifiedBy = Constants.payRollUser;
                                        balanceHist.BalanceTO.ModifiedTime = DateTime.Now;
                                        saved = saved && (balanceHist.Save(false) >= 0);

                                        if (!saved)
                                            break;

                                        balance.BalanceTO = new EmployeeCounterMonthlyBalanceTO(emplBalances[id][month][(int)Constants.EmplCounterTypes.BankHoursCounter]);
                                        balance.BalanceTO.Balance -= dict[id][month][(int)Constants.EmplCounterTypes.BankHoursCounter].ValuePaid;
                                        emplBalances[id][month][(int)Constants.EmplCounterTypes.BankHoursCounter].Balance -= dict[id][month][(int)Constants.EmplCounterTypes.BankHoursCounter].ValuePaid;
                                        balance.BalanceTO.ModifiedBy = Constants.payRollUser;
                                        balance.BalanceTO.ModifiedTime = DateTime.Now;
                                        saved = saved && balance.Update(false);

                                        // set balance payed record as used
                                        balancePaid.BalanceTO = dict[id][month][(int)Constants.EmplCounterTypes.BankHoursCounter];
                                        balancePaid.BalanceTO.RecalcFlag = Constants.yesInt;
                                        balancePaid.BalanceTO.ModifiedBy = Constants.payRollUser;
                                        balancePaid.BalanceTO.ModifiedTime = DateTime.Now;
                                        saved = saved && balancePaid.Update(false);

                                        if (!saved)
                                            break;
                                    }
                                }

                                if (!saved)
                                    break;

                                // recalculate counters
                                if (emplCounterValues.ContainsKey(id) && emplCounterValues[id].ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                                {
                                    int newValue = emplCounterValues[id][(int)Constants.EmplCounterTypes.BankHoursCounter].Value;
                                    newValue -= bhPaidHours;

                                    counterHist.ValueTO = new EmployeeCounterValueHistTO(emplCounterValues[id][(int)Constants.EmplCounterTypes.BankHoursCounter]);
                                    counterHist.ValueTO.ModifiedBy = Constants.payRollUser;
                                    saved = saved && (counterHist.Save(false) >= 0);

                                    if (!saved)
                                        break;

                                    counter.ValueTO = new EmployeeCounterValueTO();
                                    counter.ValueTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.BankHoursCounter;
                                    counter.ValueTO.EmplID = id;
                                    counter.ValueTO.Value = newValue;
                                    counter.ValueTO.ModifiedBy = Constants.payRollUser;

                                    saved = saved && counter.Update(false);

                                    if (!saved)
                                        break;
                                }
                            }

                            if (saved)
                            {                    
                                List<EmployeeCounterMonthlyBalanceTO> histList = new List<EmployeeCounterMonthlyBalanceTO>();
                                List<EmployeeCounterMonthlyBalanceTO> updateList = new List<EmployeeCounterMonthlyBalanceTO>();
                                List<EmployeeCounterMonthlyBalanceTO> insertList = new List<EmployeeCounterMonthlyBalanceTO>();
                                foreach (int id in emplList)
                                {
                                    // create employee balance record for sum month
                                    bool insertCurrent = true;
                                    EmployeeCounterMonthlyBalanceTO emplBalanceTO = new EmployeeCounterMonthlyBalanceTO();
                                    if (emplBalances.ContainsKey(id) && emplBalances[id].ContainsKey(sumMonth) && emplBalances[id][sumMonth].ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                                    {
                                        emplBalanceTO = emplBalances[id][sumMonth][(int)Constants.EmplCounterTypes.BankHoursCounter];
                                        histList.Add(new EmployeeCounterMonthlyBalanceTO(emplBalanceTO));
                                        insertCurrent = false;
                                    }
                                    else
                                    {
                                        emplBalanceTO.EmployeeID = id;
                                        emplBalanceTO.Month = sumMonth;
                                        emplBalanceTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.BankHoursCounter;
                                    }

                                    // set balance values
                                    emplBalanceTO.ValueEarned = 0;
                                    emplBalanceTO.ValueUsed = 0;

                                    if (emplSumDict.ContainsKey(id))
                                    {
                                        foreach (string code in emplSumDict[id].Keys)
                                        {
                                            if (pcEarnedHours.Contains(code))
                                                emplBalanceTO.ValueEarned += (int)(emplSumDict[id][code] * 60);

                                            if (pcUsedHours.Contains(code))
                                                emplBalanceTO.ValueUsed += (int)(emplSumDict[id][code] * 60);
                                        }
                                    }

                                    int earned = emplBalanceTO.ValueEarned;
                                    int used = emplBalanceTO.ValueUsed;

                                    if (emplBalances.ContainsKey(id))
                                    {
                                        // check if previous month balance is negative
                                        if (emplBalances[id].ContainsKey(sumMonth.AddMonths(-1))
                                            && emplBalances[id][sumMonth.AddMonths(-1)].ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter)
                                            && emplBalances[id][sumMonth.AddMonths(-1)][(int)Constants.EmplCounterTypes.BankHoursCounter].Balance < 0)
                                        {
                                            histList.Add(new EmployeeCounterMonthlyBalanceTO(emplBalances[id][sumMonth.AddMonths(-1)][(int)Constants.EmplCounterTypes.BankHoursCounter]));
                                            emplBalanceTO.Balance = emplBalances[id][sumMonth.AddMonths(-1)][(int)Constants.EmplCounterTypes.BankHoursCounter].Balance + earned - used;
                                            emplBalances[id][sumMonth.AddMonths(-1)][(int)Constants.EmplCounterTypes.BankHoursCounter].Balance = 0;
                                            updateList.Add(emplBalances[id][sumMonth.AddMonths(-1)][(int)Constants.EmplCounterTypes.BankHoursCounter]);
                                        }
                                        else if (used == 0)
                                            emplBalanceTO.Balance = earned;
                                        else
                                        {
                                            // decrease used hours from balances starting from the oldest nonzero balance
                                            foreach (DateTime month in emplBalances[id].Keys)
                                            {
                                                if (!emplBalances[id][month].ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter)
                                                    || emplBalances[id][month][(int)Constants.EmplCounterTypes.BankHoursCounter].Balance == 0)
                                                    continue;

                                                if (used == 0)
                                                    break;

                                                histList.Add(new EmployeeCounterMonthlyBalanceTO(emplBalances[id][month][(int)Constants.EmplCounterTypes.BankHoursCounter]));
                                                if (emplBalances[id][month][(int)Constants.EmplCounterTypes.BankHoursCounter].Balance >= used)
                                                {
                                                    emplBalances[id][month][(int)Constants.EmplCounterTypes.BankHoursCounter].Balance -= used;                                                    
                                                    used = 0;
                                                }
                                                else
                                                {
                                                    used -= emplBalances[id][month][(int)Constants.EmplCounterTypes.BankHoursCounter].Balance;
                                                    emplBalances[id][month][(int)Constants.EmplCounterTypes.BankHoursCounter].Balance = 0;                                                   
                                                }
                                                updateList.Add(emplBalances[id][month][(int)Constants.EmplCounterTypes.BankHoursCounter]);
                                            }

                                            emplBalanceTO.Balance = earned - used;
                                        }
                                    }
                                    else
                                        emplBalanceTO.Balance = earned - used;                                    

                                    if (insertCurrent)
                                        insertList.Add(emplBalanceTO);
                                    else
                                        updateList.Add(emplBalanceTO);
                                }

                                DateTime modTime = DateTime.Now;
                                foreach (EmployeeCounterMonthlyBalanceTO balTO in insertList)
                                {
                                    balance.BalanceTO = balTO;
                                    balance.BalanceTO.CreatedBy = Constants.payRollUser;
                                    balance.BalanceTO.CreatedTime = modTime;
                                    saved = saved && balance.Save(false) > 0;

                                    if (!saved)
                                        break;
                                }

                                foreach (EmployeeCounterMonthlyBalanceTO balTO in histList)
                                {
                                    balanceHist.BalanceTO = balTO;
                                    balanceHist.BalanceTO.ModifiedBy = Constants.payRollUser;
                                    balanceHist.BalanceTO.ModifiedTime = modTime;
                                    saved = saved && (balanceHist.Save(false) >= 0);

                                    if (!saved)
                                        break;
                                }

                                foreach (EmployeeCounterMonthlyBalanceTO balTO in updateList)
                                {
                                    balance.BalanceTO = balTO;
                                    balance.BalanceTO.ModifiedBy = Constants.payRollUser;
                                    balance.BalanceTO.ModifiedTime = modTime;
                                    saved = saved && balance.Update(false);

                                    if (!saved)
                                        break;
                                }
                            }

                            if (!saved)
                            {
                                if (counterHist.GetTransaction() != null)
                                    counterHist.RollbackTransaction();
                                MessageBox.Show(rm.GetString("CountersUpdateFaild", culture));
                                return;
                            }
                            else
                            {
                                counterHist.CommitTransaction();
                                MessageBox.Show(rm.GetString("CountersUpdateSucc", culture));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (counterHist.GetTransaction() != null)
                                counterHist.RollbackTransaction();

                            throw ex;
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                        return;
                    }                    
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.btnRecalculateBH_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {                
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.lvCompany_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRecalculateVac_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (numVacCutOffDate.Value <= 0)
                {
                    MessageBox.Show(rm.GetString("enterCalcID", culture));
                }
                else
                {
                    Common.Rule rule = new Common.Rule();
                    rule.RuleTO.RuleType = Constants.RuleCompensationForUnusedVacation;
                    List<RuleTO> rulesList = rule.Search();
                    List<string> pcVacation = new List<string>();
                    foreach (RuleTO ruleTO in rulesList)
                    {
                        if (typesDict.ContainsKey(ruleTO.RuleValue))
                        {
                            if (!pcVacation.Contains(typesDict[ruleTO.RuleValue].PaymentCode))
                            {
                                pcVacation.Add(typesDict[ruleTO.RuleValue].PaymentCode);
                            }
                        }
                    }
                    EmployeePYDataSum emplSum = new EmployeePYDataSum();
                    emplSum.EmplSum.PYCalcID = Convert.ToUInt32(numVacCutOffDate.Value);
                    List<EmployeePYDataSumTO> list = new List<EmployeePYDataSumTO>();
                    List<string> paymentCodes = new List<string>();
                    foreach (RuleTO ruleTO in rulesList)
                    {
                        if (typesDict.ContainsKey(ruleTO.RuleValue))
                        {
                            if (!paymentCodes.Contains(typesDict[ruleTO.RuleValue].PaymentCode))
                            {
                                paymentCodes.Add(typesDict[ruleTO.RuleValue].PaymentCode);
                            }
                        }
                    }

                    foreach (string pCode in paymentCodes)
                    {
                        emplSum.EmplSum.PaymentCode = pCode;
                        list.AddRange(emplSum.getEmployeesSum());
                    }

                    string employeeIDString = "";

                    foreach (EmployeePYDataSumTO sum in list)
                    {
                        employeeIDString += sum.EmployeeID.ToString() + ", ";
                    }

                    if (employeeIDString.Length > 0)
                        employeeIDString = employeeIDString.Substring(0, employeeIDString.Length - 2);

                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounterValues = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
                    emplCounterValues = new EmployeeCounterValue().SearchValues(employeeIDString);
                    EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();
                    EmployeeCounterValue counter = new EmployeeCounterValue();
                    BufferMonthlyBalancePaid balancePaid = new BufferMonthlyBalancePaid();
                    bool trans = counterHist.BeginTransaction();
                    if (trans)
                    {
                        try
                        {
                            counter.SetTransaction(counterHist.GetTransaction());
                            balancePaid.SetTransaction(counterHist.GetTransaction());
                            bool saved = true;
                            // move to hist table
                            foreach (EmployeePYDataSumTO sum in list)
                            {
                                if (sum.HrsAmount > 0)
                                {
                                    if (emplCounterValues.ContainsKey(sum.EmployeeID) && emplCounterValues[sum.EmployeeID].ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter)
                                        && pcVacation.Contains(sum.PaymentCode))
                                    {
                                        int newValue = emplCounterValues[sum.EmployeeID][(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value;
                                        newValue += (int)(sum.HrsAmount / 8);

                                        counterHist.ValueTO = new EmployeeCounterValueHistTO(emplCounterValues[sum.EmployeeID][(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter]);
                                        counterHist.ValueTO.ModifiedBy = Constants.payRollUser;
                                        saved = saved && (counterHist.Save(false) >= 0);

                                        if (!saved)
                                            break;

                                        counter.ValueTO = new EmployeeCounterValueTO();
                                        counter.ValueTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter;
                                        counter.ValueTO.EmplID = sum.EmployeeID;
                                        counter.ValueTO.Value = newValue;
                                        counter.ValueTO.ModifiedBy = Constants.payRollUser;
                                        saved = saved && counter.Update(false);

                                        if (!saved)
                                            break;

                                        balancePaid.BalanceTO.EmployeeID = sum.EmployeeID;
                                        balancePaid.BalanceTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter;
                                        balancePaid.BalanceTO.Month = new DateTime(sum.DateStart.Year, sum.DateStart.Month, 1);
                                        balancePaid.BalanceTO.PYCalcID = sum.PYCalcID;
                                        balancePaid.BalanceTO.ValuePaid = (int)(sum.HrsAmount / 8);
                                        balancePaid.BalanceTO.RecalcFlag = Constants.yesInt;
                                        balancePaid.BalanceTO.CreatedBy = Constants.payRollUser;
                                        saved = saved && balancePaid.Save(false) > 0;

                                        if (!saved)
                                            break;
                                    }
                                }
                            }
                            if (!saved)
                            {
                                if (counterHist.GetTransaction() != null)
                                    counterHist.RollbackTransaction();
                                MessageBox.Show(rm.GetString("CountersUpdateFaild", culture));
                            }
                            else
                            {
                                counterHist.CommitTransaction();
                                MessageBox.Show(rm.GetString("CountersUpdateSucc", culture));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (counterHist.GetTransaction() != null)
                                counterHist.RollbackTransaction();
                            throw new Exception(".manualPairCounters() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.btnRecalculateVac_Click(): " + ex.Message + "\n");
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

                // get all pass types dictionary
                Dictionary<int, PassTypeTO> ptDict = new PassType().SearchDictionary();
                
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
                log.writeLog(DateTime.Now + " PYIntegration.btnRecalculatePaidLeaves_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRecalculateSW_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (numSWCalcID.Value <= 0)
                {
                    MessageBox.Show(rm.GetString("enterCalcID", culture));
                }
                else
                {
                    List<string> paymentCodes = new List<string>();
                    PassType pt = new PassType();
                    pt.PTypeTO.PassTypeID = Constants.ptFiatSWPay;
                    List<PassTypeTO> ptList = pt.Search();

                    foreach (PassTypeTO ptTO in ptList)
                    {
                        paymentCodes.Add(ptTO.PaymentCode);
                    }

                    EmployeePYDataSum emplSum = new EmployeePYDataSum();
                    emplSum.EmplSum.PYCalcID = Convert.ToUInt32(numSWCalcID.Value);
                    List<EmployeePYDataSumTO> list = new List<EmployeePYDataSumTO>();

                    foreach (string pCode in paymentCodes)
                    {
                        emplSum.EmplSum.PaymentCode = pCode;
                        list.AddRange(emplSum.getEmployeesSum());
                    }

                    string employeeIDString = "";

                    foreach (EmployeePYDataSumTO sum in list)
                    {
                        employeeIDString += sum.EmployeeID.ToString() + ",";
                    }
                    if (employeeIDString.Length > 0)
                        employeeIDString = employeeIDString.Substring(0, employeeIDString.Length - 1);

                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounterValues = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
                    emplCounterValues = new EmployeeCounterValue().SearchValues(employeeIDString);
                    EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();
                    EmployeeCounterValue counter = new EmployeeCounterValue();
                    bool trans = counterHist.BeginTransaction();
                    if (trans)
                    {
                        try
                        {
                            counter.SetTransaction(counterHist.GetTransaction());
                            bool saved = true;
                            // move to hist table
                            foreach (EmployeePYDataSumTO sum in list)
                            {
                                if (sum.HrsAmount > 0)
                                {
                                    if (emplCounterValues.ContainsKey(sum.EmployeeID) && emplCounterValues[sum.EmployeeID].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter)
                                        && paymentCodes.Contains(sum.PaymentCode))
                                    {
                                        int newValue = emplCounterValues[sum.EmployeeID][(int)Constants.EmplCounterTypes.StopWorkingCounter].Value;
                                        newValue -= (int)Math.Round(sum.HrsAmount * 60, MidpointRounding.AwayFromZero);

                                        counterHist.ValueTO = new EmployeeCounterValueHistTO(emplCounterValues[sum.EmployeeID][(int)Constants.EmplCounterTypes.StopWorkingCounter]);
                                        counterHist.ValueTO.ModifiedBy = Constants.payRollUser;
                                        saved = saved && (counterHist.Save(false) >= 0);

                                        if (!saved)
                                            break;

                                        counter.ValueTO = new EmployeeCounterValueTO();
                                        counter.ValueTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.StopWorkingCounter;
                                        counter.ValueTO.EmplID = sum.EmployeeID;
                                        counter.ValueTO.Value = newValue;
                                        counter.ValueTO.ModifiedBy = Constants.payRollUser;

                                        saved = saved && counter.Update(false);
                                        
                                        if (!saved)
                                            break;
                                    }
                                }
                            }
                            if (!saved)
                            {
                                if (counterHist.GetTransaction() != null)
                                    counterHist.RollbackTransaction();
                                MessageBox.Show(rm.GetString("CountersUpdateFaild", culture));
                            }
                            else
                            {
                                counterHist.CommitTransaction();
                                MessageBox.Show(rm.GetString("CountersUpdateSucc", culture));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (counterHist.GetTransaction() != null)
                                counterHist.RollbackTransaction();
                            throw new Exception(".manualPairCounters() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
                        }

                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.btnRecalculateSW_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRecalculateNJOvertime_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (numNJOvertime.Value <= 0)
                {
                    MessageBox.Show(rm.GetString("enterCalcID", culture));
                }
                else
                {
                    List<EmployeePYDataBufferTO> list = new EmployeePYDataBuffer().getEmployeeBuffers(Convert.ToUInt32(numNJOvertime.Value));
                    
                    string employeeIDString = "";

                    foreach (EmployeePYDataBufferTO buff in list)
                    {
                        employeeIDString += buff.EmployeeID.ToString() + ",";
                    }
                    if (employeeIDString.Length > 0)
                        employeeIDString = employeeIDString.Substring(0, employeeIDString.Length - 1);

                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounterValues = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
                    emplCounterValues = new EmployeeCounterValue().SearchValues(employeeIDString);
                    EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();
                    EmployeeCounterValue counter = new EmployeeCounterValue();
                    bool trans = counterHist.BeginTransaction();
                    if (trans)
                    {
                        try
                        {
                            counter.SetTransaction(counterHist.GetTransaction());
                            bool saved = true;
                            // move to hist table
                            foreach (EmployeePYDataBufferTO buff in list)
                            {
                                if (buff.NotJustifiedOvertime > 0)
                                {
                                    if (emplCounterValues.ContainsKey(buff.EmployeeID) && emplCounterValues[buff.EmployeeID].ContainsKey((int)Constants.EmplCounterTypes.NotJustifiedOvertime))                                    
                                    {
                                        int newValue = emplCounterValues[buff.EmployeeID][(int)Constants.EmplCounterTypes.NotJustifiedOvertime].Value;
                                        newValue += buff.NotJustifiedOvertime;

                                        counterHist.ValueTO = new EmployeeCounterValueHistTO(emplCounterValues[buff.EmployeeID][(int)Constants.EmplCounterTypes.NotJustifiedOvertime]);
                                        counterHist.ValueTO.ModifiedBy = Constants.payRollUser;
                                        saved = saved && (counterHist.Save(false) >= 0);

                                        if (!saved)
                                            break;

                                        counter.ValueTO = new EmployeeCounterValueTO();
                                        counter.ValueTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.NotJustifiedOvertime;
                                        counter.ValueTO.EmplID = buff.EmployeeID;
                                        counter.ValueTO.Value = newValue;
                                        counter.ValueTO.ModifiedBy = Constants.payRollUser;

                                        saved = saved && counter.Update(false);
                                        if (!saved)
                                            break;

                                    }
                                }
                            }
                            if (!saved)
                            {
                                if (counterHist.GetTransaction() != null)
                                    counterHist.RollbackTransaction();
                                MessageBox.Show(rm.GetString("CountersUpdateFaild", culture));
                            }
                            else
                            {
                                counterHist.CommitTransaction();
                                MessageBox.Show(rm.GetString("CountersUpdateSucc", culture));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (counterHist.GetTransaction() != null)
                                counterHist.RollbackTransaction();
                            throw new Exception(".manualPairCounters() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.btnRecalculateNJOvertime_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dtpBorderDay_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.dtpBorderDay_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbShowLeavingEmployees_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                dtpBorderDay.Enabled = chbShowLeavingEmployees.Checked;
                chbWorkAnalysisReport.Checked = !chbShowLeavingEmployees.Checked;
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.chbShowLeavingEmployees_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private bool validateBorderDay()
        {
            try
            {
                if (chbShowLeavingEmployees.Checked && (dtpBorderDay.Value < dtFrom.Value || dtpBorderDay.Value > dtTo.Value))
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.validateBorderDay(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.dtFrom_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.dtTo_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
                
        private void btnLeavingEmployeesRecalculate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (numLeavingEmployeesCalcID.Value <= 0)
                {
                    MessageBox.Show(rm.GetString("enterCalcID", culture));
                }
                else
                {
                    List<EmployeePYDataBufferTO> list = new EmployeePYDataBuffer().getEmployeeBuffers(Convert.ToUInt32(numLeavingEmployeesCalcID.Value));

                    string employeeIDString = "";

                    if (list.Count <= 0)
                        return;
                    
                    foreach (EmployeePYDataBufferTO buff in list)
                    {
                        employeeIDString += buff.EmployeeID.ToString() + ",";
                    }

                    if (employeeIDString.Length > 0)
                        employeeIDString = employeeIDString.Substring(0, employeeIDString.Length - 1);

                    // get employees and asco values
                    Dictionary<int, EmployeeTO> emplDict = new Employee().SearchDictionary(employeeIDString);
                    Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary(employeeIDString);

                    string leavingIDs = "";

                    foreach (int id in emplDict.Keys)
                    {
                        if (emplDict[id].Status.Trim().ToUpper().Equals(Constants.statusRetired.Trim().ToUpper()) && ascoDict.ContainsKey(id)
                            && !ascoDict[id].DatetimeValue3.Date.Equals(new DateTime()) && ascoDict[id].DatetimeValue3.Date <= list[0].DateEnd.Date.AddDays(1)
                            && ascoDict[id].DatetimeValue3.Date > list[0].DateStart.Date)
                            leavingIDs += id.ToString().Trim() + ",";
                    }

                    if (leavingIDs.Length > 0)
                        leavingIDs = leavingIDs.Substring(0, leavingIDs.Length - 1);

                    Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>> emplBalances = new EmployeeCounterMonthlyBalance().SearchEmployeeBalances(leavingIDs, new DateTime(), false);
                    Dictionary<int, Dictionary<DateTime, Dictionary<int, BufferMonthlyBalancePaidTO>>> paidBalances = new BufferMonthlyBalancePaid().SearchEmployeeBalancesPaid(leavingIDs, new DateTime(), new DateTime());

                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounterValues = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
                    emplCounterValues = new EmployeeCounterValue().SearchValues(leavingIDs);
                    EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();
                    EmployeeCounterValue counter = new EmployeeCounterValue();
                    EmployeeCounterMonthlyBalanceHist balanceHist = new EmployeeCounterMonthlyBalanceHist();
                    EmployeeCounterMonthlyBalance balance = new EmployeeCounterMonthlyBalance();
                    BufferMonthlyBalancePaid balancePaid = new BufferMonthlyBalancePaid();
                    bool trans = counterHist.BeginTransaction();
                    if (trans)
                    {
                        try
                        {
                            counter.SetTransaction(counterHist.GetTransaction());
                            bool saved = true;
                            // move to hist table
                            foreach (int emplID in emplCounterValues.Keys)
                            {
                                if (emplCounterValues[emplID].ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                                {
                                    int newValue = 0;

                                    counterHist.ValueTO = new EmployeeCounterValueHistTO(emplCounterValues[emplID][(int)Constants.EmplCounterTypes.BankHoursCounter]);
                                    counterHist.ValueTO.ModifiedBy = Constants.payRollUser;
                                    saved = saved && (counterHist.Save(false) >= 0);

                                    if (!saved)
                                        break;

                                    counter.ValueTO = new EmployeeCounterValueTO();
                                    counter.ValueTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.BankHoursCounter;
                                    counter.ValueTO.EmplID = emplID;
                                    counter.ValueTO.Value = newValue;
                                    counter.ValueTO.ModifiedBy = Constants.payRollUser;

                                    saved = saved && counter.Update(false);
                                    if (!saved)
                                        break;
                                }

                                if (emplCounterValues[emplID].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                                {
                                    int newValue = 0;

                                    counterHist.ValueTO = new EmployeeCounterValueHistTO(emplCounterValues[emplID][(int)Constants.EmplCounterTypes.StopWorkingCounter]);
                                    counterHist.ValueTO.ModifiedBy = Constants.payRollUser;
                                    saved = saved && (counterHist.Save(false) >= 0);

                                    if (!saved)
                                        break;

                                    counter.ValueTO = new EmployeeCounterValueTO();
                                    counter.ValueTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.StopWorkingCounter;
                                    counter.ValueTO.EmplID = emplID;
                                    counter.ValueTO.Value = newValue;
                                    counter.ValueTO.ModifiedBy = Constants.payRollUser;

                                    saved = saved && counter.Update(false);
                                    if (!saved)
                                        break;
                                }
                            }

                            foreach (int emplID in emplBalances.Keys)
                            {
                                foreach (DateTime month in emplBalances[emplID].Keys)
                                {
                                    foreach (int counterType in emplBalances[emplID][month].Keys)
                                    {
                                        if (emplBalances[emplID][month][counterType].Balance == 0)
                                            continue;

                                        balanceHist.SetTransaction(counterHist.GetTransaction());
                                        balanceHist.BalanceTO = new EmployeeCounterMonthlyBalanceTO(emplBalances[emplID][month][counterType]);
                                        balanceHist.BalanceTO.ModifiedBy = Constants.payRollUser;
                                        balanceHist.BalanceTO.ModifiedTime = DateTime.Now;
                                        saved = saved && (balanceHist.Save(false) >= 0);

                                        if (!saved)
                                            break;

                                        balance.BalanceTO = new EmployeeCounterMonthlyBalanceTO(emplBalances[emplID][month][counterType]);

                                        balancePaid.SetTransaction(counterHist.GetTransaction());
                                        if (paidBalances.ContainsKey(emplID) && paidBalances[emplID].ContainsKey(month) && paidBalances[emplID][month].ContainsKey(counterType))
                                        {
                                            balancePaid.BalanceTO = new BufferMonthlyBalancePaidTO(paidBalances[emplID][month][counterType]);
                                            balancePaid.BalanceTO.ValuePaid = balance.BalanceTO.Balance;
                                            balancePaid.BalanceTO.ModifiedBy = Constants.payRollUser;
                                            balancePaid.BalanceTO.ModifiedTime = DateTime.Now;
                                            saved = saved && balancePaid.Update(false);
                                        }
                                        else
                                        {
                                            balancePaid.BalanceTO.EmployeeID = emplID;
                                            balancePaid.BalanceTO.EmplCounterTypeID = counterType;
                                            balancePaid.BalanceTO.Month = month;
                                            balancePaid.BalanceTO.PYCalcID = Convert.ToUInt32(numLeavingEmployeesCalcID.Value);
                                            balancePaid.BalanceTO.ValuePaid = balance.BalanceTO.Balance;
                                            balancePaid.BalanceTO.RecalcFlag = Constants.yesInt;
                                            balancePaid.BalanceTO.CreatedBy = Constants.payRollUser;
                                            saved = saved && balancePaid.Save(false) > 0;
                                        }

                                        if (!saved)
                                            break;

                                        balance.BalanceTO.Balance = 0;
                                        balance.BalanceTO.ModifiedBy = Constants.payRollUser;
                                        balance.BalanceTO.ModifiedTime = DateTime.Now;
                                        balance.SetTransaction(counterHist.GetTransaction());
                                        saved = saved && balance.Update(false);

                                        if (!saved)
                                            break;
                                    }
                                }

                                if (!saved)
                                    break;
                            }

                            if (!saved)
                            {
                                if (counterHist.GetTransaction() != null)
                                    counterHist.RollbackTransaction();
                                MessageBox.Show(rm.GetString("CountersUpdateFaild", culture));
                            }
                            else
                            {
                                counterHist.CommitTransaction();
                                MessageBox.Show(rm.GetString("CountersUpdateSucc", culture));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (counterHist.GetTransaction() != null)
                                counterHist.RollbackTransaction();
                            throw new Exception(".manualPairCounters() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.btnRecalculateNJOvertime_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbBankHoursPay_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbBankHoursPay.Checked)
                    chbBHPayRegular.Checked = false;

                lblBHPath.Enabled = tbBHPath.Enabled = btnBHBrowse.Enabled = cbBankHoursPay.Checked;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.cbBankHoursPay_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnBHBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fbDialog = new OpenFileDialog();
                fbDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //fbDialog.Filter = "XLS (*.xls)|*.xls|" +
                //"XLSX (*.xlsx)|*.xlsx";
                fbDialog.Filter = "Text files (*.txt)|*.txt";
                fbDialog.Title = "Open file";

                if (fbDialog.ShowDialog() == DialogResult.OK)                
                    tbBHPath.Text = fbDialog.FileName;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.btnBHBrowse_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSWBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fbDialog = new OpenFileDialog();
                fbDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //fbDialog.Filter = "XLS (*.xls)|*.xls|" +
                //"XLSX (*.xlsx)|*.xlsx";
                fbDialog.Filter = "Text files (*.txt)|*.txt";
                fbDialog.Title = "Open file";

                if (fbDialog.ShowDialog() == DialogResult.OK)
                    tbSWPath.Text = fbDialog.FileName;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.btnSWBrowse_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbBHPayRegular_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chbBHPayRegular.Checked)
                    cbBankHoursPay.Checked = false;
                else
                {
                    numBHBalanceMonth.Value = Constants.defaultBHBalanceMonth;
                    chbBHPayment.Checked = false;
                }

                numBHBalanceMonth.Enabled = lblBHBalanceMonth.Enabled = chbBHPayment.Enabled = chbBHPayRegular.Checked;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.chbBHPayRegular_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateWUList()
        {
            try
            {
                cbWU.DataSource = wUnits;
                cbWU.DisplayMember = "Name";
                cbWU.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.populateWUList(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbWU.SelectedValue != null && cbWU.SelectedValue is int)
                {
                    int selCompany = Common.Misc.getRootWorkingUnit((int)cbWU.SelectedValue, wuDict);

                    if (selCompany != absentCompanySelected)
                    {
                        absentCompanySelected = selCompany;
                        populatePTList();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populatePTList()
        {
            try
            {
                lvPTAbsent.BeginUpdate();
                lvPTAbsent.Items.Clear();

                List<PassTypeTO> ptList = new List<PassTypeTO>();
                bool isAltLang = logInUser.LangCode != Constants.Lang_sr;

                if (absentCompanySelected != -1)
                    ptList = new PassType().SearchForCompany(absentCompanySelected, isAltLang);

                List<int> closureTypes = Constants.FiatClosureTypes();
                List<int> layoffTypes = Constants.FiatLayOffTypes();
                List<int> stoppageTypes = Constants.FiatStoppageTypes();
                List<int> holidayTypes = Constants.FiatHolidayTypes();

                foreach (PassTypeTO pt in ptList)
                {
                    ListViewItem item = new ListViewItem();
                    
                    if (isAltLang)
                        item.Text = pt.DescriptionAltAndID.Trim();
                    else
                        item.Text = pt.DescriptionAndID.Trim();

                    item.Tag = pt;

                    if (cbWU.Text == "148" && !closureTypes.Contains(pt.PassTypeID) && !layoffTypes.Contains(pt.PassTypeID) && !stoppageTypes.Contains(pt.PassTypeID) 
                        && !holidayTypes.Contains(pt.PassTypeID) && (pt.PaymentCode == "0058" || pt.PaymentCode == "0057" || pt.PaymentCode == "0157"))
                        item.Selected = true;

                    lvPTAbsent.Items.Add(item);
                }

                lvPTAbsent.EndUpdate();
                lvPTAbsent.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.populatePTList(): " + ex.Message + "\n");
                throw ex;
            }
        }
        
        private void btnGenerateAbsent_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                uint calcID = 0;
                if (!uint.TryParse(numAbsentCalcID.Value.ToString().Trim(), out calcID))
                    calcID = 0;

                if (calcID <= 0)
                {
                    MessageBox.Show(rm.GetString("enterCalcID", culture));
                    return;
                }

                if (lvPTAbsent.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("selectPT", culture));
                    return;
                }

                string wuIDs = cbWU.SelectedValue.ToString().Trim();

                wuIDs = Common.Misc.getWorkingUnitHierarhicly(wuIDs, wuIDList, null);

                List<EmployeeTO> emplList = new Employee().SearchByWU(wuIDs);

                List<int> emplIDs = new List<int>();
                string ids = "";
                foreach (EmployeeTO empl in emplList)
                {
                    emplIDs.Add(empl.EmployeeID);
                    ids += empl.EmployeeID + ",";
                }

                if (ids.Length > 0)
                    ids = ids.Substring(0, ids.Length - 1);

                string codes = "";

                foreach (ListViewItem item in lvPTAbsent.SelectedItems)
                {
                    codes += "'" + ((PassTypeTO)item.Tag).PaymentCode.Trim() + "',";
                }

                if (codes.Length > 0)
                    codes = codes.Substring(0, codes.Length - 1);

                //Dictionary<int, Dictionary<string, decimal>> sumDict = new Dictionary<int, Dictionary<string, decimal>>();
                //EmployeePYDataSum sum = new EmployeePYDataSum();
                //if (emplIDs.Count > 0 && codes.Length > 0)
                //    sumDict = sum.getEmployeesSumValues(calcID, codes);

                List<EmployeePYDataAnaliticalTO> analiticalList = new List<EmployeePYDataAnaliticalTO>();
                if (ids.Length > 0 && codes.Length > 0)
                    analiticalList = new EmployeePYDataAnalitical().Search(ids, codes, calcID, "");

                Dictionary<int, Dictionary<string, decimal>> sumDict = new Dictionary<int, Dictionary<string, decimal>>();
                foreach (EmployeePYDataAnaliticalTO analitical in analiticalList)
                {
                    if (!sumDict.ContainsKey(analitical.EmployeeID))
                        sumDict.Add(analitical.EmployeeID, new Dictionary<string, decimal>());

                    if (!sumDict[analitical.EmployeeID].ContainsKey(analitical.PaymentCode))
                        sumDict[analitical.EmployeeID].Add(analitical.PaymentCode, 0);

                    sumDict[analitical.EmployeeID][analitical.PaymentCode] += analitical.HrsAmount;
                }

                DateTime month = new EmployeePYDataSum().getSumMonth(calcID);
                string delimiter = ";";

                // create header
                string header = "Plant" + delimiter + "ID" + delimiter + "Year" + delimiter + "Month" + delimiter + "Hours" + delimiter;

                List<string> lines = new List<string>();
                foreach (int id in sumDict.Keys)
                {
                    if (!emplIDs.Contains(id))
                        continue;

                    string row = cbWU.Text.Trim() + delimiter + id.ToString().Trim() + delimiter + month.Year.ToString().Trim() + delimiter + month.Month.ToString().Trim() + delimiter;

                    decimal hours = 0;

                    foreach (string code in sumDict[id].Keys)
                    {
                        hours += sumDict[id][code];
                    }

                    row += hours.ToString(Constants.doubleFormat).Replace('.', ',') + delimiter;

                    lines.Add(row);
                }

                if (lines.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }

                string reportName = "Absence_" + month.Date.ToString("yyyy_MM") + ".csv";
                string filePath = Constants.FiatSharedAbsencePath.Trim();
                
                // check if path exists
                if (!filePath.Trim().Equals("") && !Directory.Exists(filePath))
                    filePath = "";                

                if (filePath.Trim().Equals(""))
                {
                    FolderBrowserDialog fbDialog = new FolderBrowserDialog();
                    fbDialog.Description = "Absence reports path";

                    if (fbDialog.ShowDialog() == DialogResult.OK)
                        filePath = fbDialog.SelectedPath;
                    else
                        return;
                }
                
                filePath += "\\" + reportName;

                FileStream stream = new FileStream(filePath, FileMode.Append);
                stream.Close();

                StreamWriter writer = new StreamWriter(filePath, true, Encoding.Unicode);
                // insert header
                writer.WriteLine(header);

                foreach (string line in lines)
                {
                    writer.WriteLine(line);
                }

                writer.Close();

                MessageBox.Show(rm.GetString("FileGenerated", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.btnGenerateAbsent_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbSWPayRegular_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chbSWPayRegular.Checked)
                    cbSWPay.Checked = false;
                else                
                {
                    numSWBalanceMonth.Value = Constants.defaultSWBalanceMonth;
                    chbSWPayment.Checked = false;
                }

                numSWBalanceMonth.Enabled = lblSWBalanceMonth.Enabled = chbSWPayment.Enabled = chbSWPayRegular.Checked;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.chbSWPayRegular_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRecalculateSWPay_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (numSWPayCalcID.Value <= 0)
                {
                    MessageBox.Show(rm.GetString("enterCalcID", culture));
                }
                else
                {
                    // first, prepare data for recalculation counters for paid stop working hours
                    Common.Rule rule = new Common.Rule();

                    EmployeePYDataSum emplSum = new EmployeePYDataSum();
                    uint calcID = Convert.ToUInt32(numSWPayCalcID.Value);

                    DateTime sumMonth = emplSum.getSumMonth(calcID).Date;

                    Dictionary<int, Dictionary<DateTime, Dictionary<int, BufferMonthlyBalancePaidTO>>> dict = new BufferMonthlyBalancePaid().SearchEmployeeBalancesPaid(calcID, (int)Constants.EmplCounterTypes.StopWorkingCounter);

                    string employeeIDString = "";

                    foreach (int id in dict.Keys)
                    {
                        employeeIDString += id.ToString() + ",";
                    }

                    if (employeeIDString.Length > 0)
                        employeeIDString = employeeIDString.Substring(0, employeeIDString.Length - 1);

                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounterValues = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();
                    if (employeeIDString.Length > 0)
                        emplCounterValues = new EmployeeCounterValue().SearchValues(employeeIDString);

                    // 30.07.2013. Sanja
                    // second, recalculate stop working hours balances
                    // balance record for particular month contains earned and spent stop working hours of that month and current month balance
                    // negative balances are increased and moved to next month and they are not payed
                    // positive balances are decreased                    
                    List<int> earnedList = rule.SearchRulesExact(Constants.RuleCompanyStopWorkingMonthly);
                    List<string> pcEarnedHours = new List<string>();
                    foreach (int type in earnedList)
                    {
                        if (typesDict.ContainsKey(type) && !pcEarnedHours.Contains(typesDict[type].PaymentCode))
                            pcEarnedHours.Add(typesDict[type].PaymentCode);
                    }

                    List<int> usedList = rule.SearchRulesExact(Constants.RuleCompanyStopWorkingDone);
                    List<string> pcUsedHours = new List<string>();
                    foreach (int type in usedList)
                    {
                        if (typesDict.ContainsKey(type) && !pcUsedHours.Contains(typesDict[type].PaymentCode))
                            pcUsedHours.Add(typesDict[type].PaymentCode);
                    }

                    string codes = "";
                    foreach (string pCode in pcEarnedHours)
                    {
                        codes += "'" + pCode + "',";
                    }

                    foreach (string pCode in pcUsedHours)
                    {
                        codes += "'" + pCode + "',";
                    }

                    if (codes.Length > 0)
                        codes = codes.Substring(0, codes.Length - 1);

                    Dictionary<int, Dictionary<string, decimal>> emplSumDict = emplSum.getEmployeesSumValues(calcID, codes);
                    List<int> emplList = emplSum.getEmployees(calcID);

                    string emplIDs = "";
                    foreach (int id in emplList)
                    {
                        emplIDs += id.ToString().Trim() + ",";
                    }

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                    DateTime payedMonth = new EmployeeCounterMonthlyBalance().GetMinPositiveMonth(emplIDs).AddMonths(-1).Date;

                    Dictionary<int, Dictionary<DateTime, Dictionary<int, EmployeeCounterMonthlyBalanceTO>>> emplBalances = new EmployeeCounterMonthlyBalance().SearchEmployeeBalances(emplIDs.Trim(), payedMonth, false);

                    EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();
                    EmployeeCounterValue counter = new EmployeeCounterValue();
                    EmployeeCounterMonthlyBalance balance = new EmployeeCounterMonthlyBalance();
                    EmployeeCounterMonthlyBalanceHist balanceHist = new EmployeeCounterMonthlyBalanceHist();
                    BufferMonthlyBalancePaid balancePaid = new BufferMonthlyBalancePaid();
                    bool saved = true;
                    if (counterHist.BeginTransaction())
                    {
                        try
                        {
                            counter.SetTransaction(counterHist.GetTransaction());
                            balanceHist.SetTransaction(counterHist.GetTransaction());
                            balance.SetTransaction(counterHist.GetTransaction());
                            balancePaid.SetTransaction(counterHist.GetTransaction());

                            // reacalculate counters and balances for paid stop working hours
                            foreach (int id in dict.Keys)
                            {
                                int swPaidHours = 0;

                                foreach (DateTime month in dict[id].Keys)
                                {
                                    if (dict[id][month].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter)
                                        && emplBalances.ContainsKey(id) && emplBalances[id].ContainsKey(month) && emplBalances[id][month].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                                    {
                                        swPaidHours += dict[id][month][(int)Constants.EmplCounterTypes.StopWorkingCounter].ValuePaid;

                                        // empty payed month balances                                                                            
                                        balanceHist.BalanceTO = new EmployeeCounterMonthlyBalanceTO(emplBalances[id][month][(int)Constants.EmplCounterTypes.StopWorkingCounter]);
                                        balanceHist.BalanceTO.ModifiedBy = Constants.payRollUser;
                                        balanceHist.BalanceTO.ModifiedTime = DateTime.Now;
                                        saved = saved && (balanceHist.Save(false) >= 0);

                                        if (!saved)
                                            break;

                                        balance.BalanceTO = new EmployeeCounterMonthlyBalanceTO(emplBalances[id][month][(int)Constants.EmplCounterTypes.StopWorkingCounter]);
                                        balance.BalanceTO.Balance -= dict[id][month][(int)Constants.EmplCounterTypes.StopWorkingCounter].ValuePaid;
                                        emplBalances[id][month][(int)Constants.EmplCounterTypes.StopWorkingCounter].Balance -= dict[id][month][(int)Constants.EmplCounterTypes.StopWorkingCounter].ValuePaid;
                                        balance.BalanceTO.ModifiedBy = Constants.payRollUser;
                                        balance.BalanceTO.ModifiedTime = DateTime.Now;
                                        saved = saved && balance.Update(false);

                                        // set balance payed record as used
                                        balancePaid.BalanceTO = dict[id][month][(int)Constants.EmplCounterTypes.StopWorkingCounter];
                                        balancePaid.BalanceTO.RecalcFlag = Constants.yesInt;
                                        balancePaid.BalanceTO.ModifiedBy = Constants.payRollUser;
                                        balancePaid.BalanceTO.ModifiedTime = DateTime.Now;
                                        saved = saved && balancePaid.Update(false);

                                        if (!saved)
                                            break;
                                    }
                                }

                                if (!saved)
                                    break;

                                // recalculate counters
                                if (emplCounterValues.ContainsKey(id) && emplCounterValues[id].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                                {
                                    int newValue = emplCounterValues[id][(int)Constants.EmplCounterTypes.StopWorkingCounter].Value;
                                    newValue -= swPaidHours;

                                    counterHist.ValueTO = new EmployeeCounterValueHistTO(emplCounterValues[id][(int)Constants.EmplCounterTypes.StopWorkingCounter]);
                                    counterHist.ValueTO.ModifiedBy = Constants.payRollUser;
                                    saved = saved && (counterHist.Save(false) >= 0);

                                    if (!saved)
                                        break;

                                    counter.ValueTO = new EmployeeCounterValueTO();
                                    counter.ValueTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.StopWorkingCounter;
                                    counter.ValueTO.EmplID = id;
                                    counter.ValueTO.Value = newValue;
                                    counter.ValueTO.ModifiedBy = Constants.payRollUser;

                                    saved = saved && counter.Update(false);

                                    if (!saved)
                                        break;
                                }
                            }

                            if (saved)
                            {
                                List<EmployeeCounterMonthlyBalanceTO> histList = new List<EmployeeCounterMonthlyBalanceTO>();
                                List<EmployeeCounterMonthlyBalanceTO> updateList = new List<EmployeeCounterMonthlyBalanceTO>();
                                List<EmployeeCounterMonthlyBalanceTO> insertList = new List<EmployeeCounterMonthlyBalanceTO>();
                                foreach (int id in emplList)
                                {
                                    // create employee balance record for sum month
                                    bool insertCurrent = true;
                                    EmployeeCounterMonthlyBalanceTO emplBalanceTO = new EmployeeCounterMonthlyBalanceTO();
                                    if (emplBalances.ContainsKey(id) && emplBalances[id].ContainsKey(sumMonth) && emplBalances[id][sumMonth].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter))
                                    {
                                        emplBalanceTO = emplBalances[id][sumMonth][(int)Constants.EmplCounterTypes.StopWorkingCounter];
                                        histList.Add(new EmployeeCounterMonthlyBalanceTO(emplBalanceTO));
                                        insertCurrent = false;
                                    }
                                    else
                                    {
                                        emplBalanceTO.EmployeeID = id;
                                        emplBalanceTO.Month = sumMonth;
                                        emplBalanceTO.EmplCounterTypeID = (int)Constants.EmplCounterTypes.StopWorkingCounter;
                                    }

                                    // set balance values
                                    emplBalanceTO.ValueEarned = 0;
                                    emplBalanceTO.ValueUsed = 0;

                                    if (emplSumDict.ContainsKey(id))
                                    {
                                        foreach (string code in emplSumDict[id].Keys)
                                        {
                                            if (pcEarnedHours.Contains(code))
                                                emplBalanceTO.ValueEarned += (int)Math.Round(emplSumDict[id][code] * 60, 0);

                                            if (pcUsedHours.Contains(code))
                                                emplBalanceTO.ValueUsed += (int)Math.Round(emplSumDict[id][code] * 60, 0);
                                        }
                                    }

                                    int earned = emplBalanceTO.ValueEarned;
                                    int used = emplBalanceTO.ValueUsed;

                                    if (emplBalances.ContainsKey(id))
                                    {
                                        // check if previous month balance is negative
                                        if (emplBalances[id].ContainsKey(sumMonth.AddMonths(-1))
                                            && emplBalances[id][sumMonth.AddMonths(-1)].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter)
                                            && emplBalances[id][sumMonth.AddMonths(-1)][(int)Constants.EmplCounterTypes.StopWorkingCounter].Balance < 0)
                                        {
                                            histList.Add(new EmployeeCounterMonthlyBalanceTO(emplBalances[id][sumMonth.AddMonths(-1)][(int)Constants.EmplCounterTypes.StopWorkingCounter]));
                                            emplBalanceTO.Balance = emplBalances[id][sumMonth.AddMonths(-1)][(int)Constants.EmplCounterTypes.StopWorkingCounter].Balance + earned - used;
                                            emplBalances[id][sumMonth.AddMonths(-1)][(int)Constants.EmplCounterTypes.StopWorkingCounter].Balance = 0;
                                            updateList.Add(emplBalances[id][sumMonth.AddMonths(-1)][(int)Constants.EmplCounterTypes.StopWorkingCounter]);
                                        }
                                        else if (used == 0)
                                            emplBalanceTO.Balance = earned;
                                        else
                                        {
                                            // decrease used hours from balances starting from the oldest nonzero balance
                                            foreach (DateTime month in emplBalances[id].Keys)
                                            {
                                                if (!emplBalances[id][month].ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter)
                                                    || emplBalances[id][month][(int)Constants.EmplCounterTypes.StopWorkingCounter].Balance == 0)
                                                    continue;

                                                if (used == 0)
                                                    break;

                                                histList.Add(new EmployeeCounterMonthlyBalanceTO(emplBalances[id][month][(int)Constants.EmplCounterTypes.StopWorkingCounter]));
                                                if (emplBalances[id][month][(int)Constants.EmplCounterTypes.StopWorkingCounter].Balance >= used)
                                                {
                                                    emplBalances[id][month][(int)Constants.EmplCounterTypes.StopWorkingCounter].Balance -= used;
                                                    used = 0;
                                                }
                                                else
                                                {
                                                    used -= emplBalances[id][month][(int)Constants.EmplCounterTypes.StopWorkingCounter].Balance;
                                                    emplBalances[id][month][(int)Constants.EmplCounterTypes.StopWorkingCounter].Balance = 0;
                                                }
                                                updateList.Add(emplBalances[id][month][(int)Constants.EmplCounterTypes.StopWorkingCounter]);
                                            }

                                            emplBalanceTO.Balance = earned - used;
                                        }
                                    }
                                    else
                                        emplBalanceTO.Balance = earned - used;

                                    if (insertCurrent)
                                        insertList.Add(emplBalanceTO);
                                    else
                                        updateList.Add(emplBalanceTO);
                                }

                                DateTime modTime = DateTime.Now;
                                foreach (EmployeeCounterMonthlyBalanceTO balTO in insertList)
                                {
                                    balance.BalanceTO = balTO;
                                    balance.BalanceTO.CreatedBy = Constants.payRollUser;
                                    balance.BalanceTO.CreatedTime = modTime;
                                    saved = saved && balance.Save(false) > 0;

                                    if (!saved)
                                        break;
                                }

                                foreach (EmployeeCounterMonthlyBalanceTO balTO in histList)
                                {
                                    balanceHist.BalanceTO = balTO;
                                    balanceHist.BalanceTO.ModifiedBy = Constants.payRollUser;
                                    balanceHist.BalanceTO.ModifiedTime = modTime;
                                    saved = saved && (balanceHist.Save(false) >= 0);

                                    if (!saved)
                                        break;
                                }

                                foreach (EmployeeCounterMonthlyBalanceTO balTO in updateList)
                                {
                                    balance.BalanceTO = balTO;
                                    balance.BalanceTO.ModifiedBy = Constants.payRollUser;
                                    balance.BalanceTO.ModifiedTime = modTime;
                                    saved = saved && balance.Update(false);

                                    if (!saved)
                                        break;
                                }
                            }

                            if (!saved)
                            {
                                if (counterHist.GetTransaction() != null)
                                    counterHist.RollbackTransaction();
                                MessageBox.Show(rm.GetString("CountersUpdateFaild", culture));
                                return;
                            }
                            else
                            {
                                counterHist.CommitTransaction();
                                MessageBox.Show(rm.GetString("CountersUpdateSucc", culture));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (counterHist.GetTransaction() != null)
                                counterHist.RollbackTransaction();

                            throw ex;
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.btnRecalculateSWPay_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbSWPay_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbSWPay.Checked)
                    chbSWPayRegular.Checked = false;

                lblSWPath.Enabled = tbSWPath.Enabled = btnSWBrowse.Enabled = cbSWPay.Checked;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PYIntegration.cbSWPay_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}

