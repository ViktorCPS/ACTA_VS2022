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

namespace Reports.PMC
{
    public partial class PMCStatisticalReports : Form
    {        
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private string wuString = "";
        private List<int> wuList = new List<int>();

        //private Dictionary<int, OrganizationalUnitTO> oUnits = new Dictionary<int, OrganizationalUnitTO>();
        //private string ouString = "";
        //private List<int> ouList = new List<int>();

        Dictionary<int, WorkingUnitTO> wuDict = new Dictionary<int, WorkingUnitTO>();
        //Dictionary<int, OrganizationalUnitTO> ouDict = new Dictionary<int, OrganizationalUnitTO>();
        Dictionary<int, EmployeeAsco4TO> ascoDict = new Dictionary<int, EmployeeAsco4TO>();
        Dictionary<int, string> emplTypes = new Dictionary<int, string>();

        public PMCStatisticalReports()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(PMCStatisticalReports).Assembly);

                logInUser = NotificationController.GetLogInUser();

                setLanguage();

                rbHiredEmployees.Checked = true;
                
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("PMCStatisticalReports", culture);

                //label's text                
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);

                //button's text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnGenerate.Text = rm.GetString("btnGenerate", culture);

                //group box text
                this.gbDateInterval.Text = rm.GetString("gbDateInterval", culture);                
                this.gbReportType.Text = rm.GetString("gbReportType", culture);
                this.gbCategory.Text = rm.GetString("gbCategory", culture);

                // radio button's text
                this.rbAgeAverage.Text = rm.GetString("rbAgeAverage", culture);
                this.rbGender.Text = rm.GetString("rbGender", culture);
                this.rbHiredEmployees.Text = rm.GetString("rbHiredEmployees", culture);
                this.rbLeavingEmployees.Text = rm.GetString("rbLeavingEmployees", culture);
                this.rbPositionChange.Text = rm.GetString("rbPositionChange", culture);

                // list view
                lvCategory.BeginUpdate();
                lvCategory.Columns.Add(rm.GetString("hdrCategory", culture), lvCategory.Width - 22, HorizontalAlignment.Left);
                lvCategory.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCStatisticalReports.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void PMCStatisticalReports_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                //ouDict = new OrganizationalUnit().SearchDictionary();
                wuDict = new WorkingUnit().getWUDictionary();
                
                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                    //oUnits = new ApplUserXOrgUnit().FindOUForUserDictionary(logInUser.UserID.Trim(), Constants.ReportPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                    wuList.Add(wUnit.WorkingUnitID);
                }

                if (wuString.Length > 0)
                    wuString = wuString.Substring(0, wuString.Length - 1);

                //foreach (int id in oUnits.Keys)
                //{
                //    ouString += id.ToString().Trim() + ",";
                //    ouList.Add(id);
                //}

                //if (ouString.Length > 0)
                //    ouString = ouString.Substring(0, ouString.Length - 1);

                ascoDict = new EmployeeAsco4().SearchDictionary("");
                Dictionary<int, Dictionary<int, string>> emplTypesAll = new EmployeeType().SearchDictionary();

                foreach (int company in emplTypesAll.Keys)
                {
                    foreach (int type in emplTypesAll[company].Keys)
                    {
                        if (!emplTypes.ContainsKey(type))
                            emplTypes.Add(type, emplTypesAll[company][type].Trim().ToUpper());
                    }
                }

                populateCategories();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCStatisticalReports.PMCStatisticalReports_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateCategories()
        {
            try
            {
                lvCategory.BeginUpdate();
                lvCategory.Items.Clear();

                foreach (int type in emplTypes.Keys)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = emplTypes[type];
                    item.Tag = type;
                    item.ToolTipText = type.ToString().Trim();
                    lvCategory.Items.Add(item);
                }

                lvCategory.EndUpdate();
                lvCategory.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PMCStatisticalReports.populateCategories(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {                
                if (dtpFrom.Value.Date > dtpTo.Value.Date)
                {
                    MessageBox.Show(rm.GetString("invalidDateInterval", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                List<EmployeeTO> emplList = new Employee().SearchByWULoans(wuString, -1, null, dtpFrom.Value.Date, dtpTo.Value.Date);
                List<string> headerList = new List<string>();                
                string reportName = "";
                int colNum = 0;
                if (rbHiredEmployees.Checked)
                {
                    headerList.Add(rm.GetString("hdrID", culture));
                    headerList.Add(rm.GetString("hdrFirstName", culture));
                    headerList.Add(rm.GetString("hdrLastName", culture));
                    headerList.Add(rm.GetString("hdrPosition", culture));
                    headerList.Add(rm.GetString("hdrCategory", culture));
                    colNum = 5;
                    reportName = "HiredEmployees_";
                }
                else if (rbLeavingEmployees.Checked)
                {
                    headerList.Add(rm.GetString("hdrID", culture));
                    headerList.Add(rm.GetString("hdrFirstName", culture));
                    headerList.Add(rm.GetString("hdrLastName", culture));
                    headerList.Add(rm.GetString("hdrPosition", culture));
                    headerList.Add(rm.GetString("hdrCategory", culture));
                    colNum = 5;
                    reportName = "LeavingEmployees_";
                }
                else if (rbGender.Checked)
                {
                    headerList.Add(rm.GetString("hdrCategory", culture));
                    headerList.Add(rm.GetString("hdrMale", culture));
                    headerList.Add(rm.GetString("hdrFemale", culture));
                    headerList.Add(rm.GetString("hdrMale", culture) + "(%)");
                    headerList.Add(rm.GetString("hdrFemale", culture) + "(%)");
                    headerList.Add(rm.GetString("hdrTotal", culture));
                    colNum = 6;
                    reportName = "GenderReport_";
                }
                else if (rbAgeAverage.Checked)
                {
                    headerList.Add(rm.GetString("hdrCategory", culture));
                    headerList.Add(rm.GetString("hdrAgeAverage", culture));
                    colNum = 2;
                    reportName = "AgeAverageReport_";
                }
                if (rbPositionChange.Checked)
                {
                    headerList.Add(rm.GetString("hdrID", culture));
                    headerList.Add(rm.GetString("hdrFirstName", culture));
                    headerList.Add(rm.GetString("hdrLastName", culture));
                    headerList.Add(rm.GetString("hdrPosition", culture));
                    headerList.Add(rm.GetString("hdrOldPosition", culture));
                    headerList.Add(rm.GetString("hdrCategory", culture));
                    colNum = 6;
                    reportName = "PositionChangeEmployees_";
                }

                List<int> selectedTypes = new List<int>();
                if (lvCategory.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvCategory.SelectedItems)
                    {
                        selectedTypes.Add((int)item.Tag);
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvCategory.Items)
                    {
                        selectedTypes.Add((int)item.Tag);
                    }
                }

                if (rbAgeAverage.Checked || rbGender.Checked)
                    generateAgeGenderReport(emplList, colNum, headerList, reportName, selectedTypes);
                else if (rbHiredEmployees.Checked || rbLeavingEmployees.Checked)
                    generateChangeStatusEmployeesReport(emplList, colNum, headerList, reportName, selectedTypes);
                else if (rbPositionChange.Checked)
                    generatePositionChangeReport(emplList, colNum, headerList, reportName, selectedTypes);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCStatisticalReports.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        
        private void generateAgeGenderReport(List<EmployeeTO> emplList, int colNum, List<string> headerList, string reportName, List<int> selectedTypes)
        {
            try
            {
                List<List<string>> rows = new List<List<string>>();
                Dictionary<int, int> numOfPeopleByCategories = new Dictionary<int, int>();
                Dictionary<int, int> ageByCategories = new Dictionary<int, int>();
                Dictionary<int, int> maleByCategories = new Dictionary<int, int>();
                Dictionary<int, int> femaleByCategories = new Dictionary<int, int>();

                foreach (EmployeeTO empl in emplList)
                {
                    if (!selectedTypes.Contains(empl.EmployeeTypeID))
                        continue;

                    if (!numOfPeopleByCategories.ContainsKey(empl.EmployeeTypeID))
                        numOfPeopleByCategories.Add(empl.EmployeeTypeID, 0);

                    numOfPeopleByCategories[empl.EmployeeTypeID]++;

                    if (ascoDict.ContainsKey(empl.EmployeeID))
                    {                        
                        if (rbGender.Checked && ascoDict[empl.EmployeeID].NVarcharValue9.Trim() != "")
                        {
                            if (ascoDict[empl.EmployeeID].NVarcharValue9.Trim().ToUpper() == Constants.male.ToString().ToUpper())
                            {
                                if (!maleByCategories.ContainsKey(empl.EmployeeTypeID))
                                    maleByCategories.Add(empl.EmployeeTypeID, 0);

                                maleByCategories[empl.EmployeeTypeID]++;
                            }
                            else if (ascoDict[empl.EmployeeID].NVarcharValue9.Trim().ToUpper() == Constants.female.ToString().ToUpper())
                            {
                                if (!femaleByCategories.ContainsKey(empl.EmployeeTypeID))
                                    femaleByCategories.Add(empl.EmployeeTypeID, 0);

                                femaleByCategories[empl.EmployeeTypeID]++;
                            }
                        }
                                                
                        if (rbAgeAverage.Checked && ascoDict[empl.EmployeeID].NVarcharValue4.Length >= 7)
                        {
                            DateTime dateOfBirth = new DateTime();
                            int day = -1;
                            int month = -1;
                            int year = -1;

                            if (int.TryParse(ascoDict[empl.EmployeeID].NVarcharValue4.Substring(0, 2), out day)
                                && int.TryParse(ascoDict[empl.EmployeeID].NVarcharValue4.Substring(2, 2), out month)
                                && int.TryParse(ascoDict[empl.EmployeeID].NVarcharValue4.Substring(4, 3), out year))
                            {
                                if (year > 900)
                                    year += 1000;
                                else
                                    year += 2000;

                                try
                                {
                                    dateOfBirth = new DateTime(year, month, day);
                                }
                                catch
                                {
                                    dateOfBirth = new DateTime();
                                }

                                if (dateOfBirth != new DateTime())
                                {
                                    if (!ageByCategories.ContainsKey(empl.EmployeeTypeID))
                                        ageByCategories.Add(empl.EmployeeTypeID, 0);

                                    int age = DateTime.Now.Year - dateOfBirth.Year;

                                    // get this year birthday
                                    DateTime birthday = new DateTime(DateTime.Now.Year, dateOfBirth.Month, dateOfBirth.Day);

                                    if (birthday > DateTime.Now.Date)
                                        age--;

                                    ageByCategories[empl.EmployeeTypeID] += age;
                                }
                            }
                        }
                    }
                }

                // create list of rows
                if (rbAgeAverage.Checked)
                {
                    foreach (int category in emplTypes.Keys)
                    {
                        if (!selectedTypes.Contains(category))
                            continue;

                        string average = "N/A";
                        if (numOfPeopleByCategories.ContainsKey(category) && numOfPeopleByCategories[category] > 0 && ageByCategories.ContainsKey(category))
                            average = ((decimal)ageByCategories[category] / (decimal)numOfPeopleByCategories[category]).ToString(Constants.doubleFormat);

                        // add row to age average report
                        List<string> row = new List<string>();
                        if (emplTypes.ContainsKey(category))
                            row.Add(emplTypes[category]);
                        else
                            row.Add("N/A");
                        row.Add(average.Trim());

                        rows.Add(row);
                    }
                }
                else if (rbGender.Checked)
                {
                    foreach (int category in emplTypes.Keys)
                    {
                        if (!selectedTypes.Contains(category))
                            continue;

                        int maleNum = 0;
                        int femaleNum = 0;
                        int total = 0;

                        if (maleByCategories.ContainsKey(category))
                            maleNum = maleByCategories[category];

                        if (femaleByCategories.ContainsKey(category))
                            femaleNum = femaleByCategories[category];

                        if (numOfPeopleByCategories.ContainsKey(category) && numOfPeopleByCategories[category] > 0)
                            total = numOfPeopleByCategories[category];

                        string malePercent = "0.00";
                        string femalePercent = "0.00";

                        if (total > 0)
                        {
                            malePercent = (((decimal)maleNum * 100) / (decimal)total).ToString(Constants.doubleFormat);
                            femalePercent = (((decimal)femaleNum * 100) / (decimal)total).ToString(Constants.doubleFormat);
                        }

                        // add row to age average report
                        List<string> row = new List<string>();
                        if (emplTypes.ContainsKey(category))
                            row.Add(emplTypes[category]);
                        else
                            row.Add("N/A");
                        row.Add(maleNum.ToString().Trim());
                        row.Add(femaleNum.ToString().Trim());
                        row.Add(malePercent.Trim());
                        row.Add(femalePercent.Trim());
                        row.Add(total.ToString().Trim());

                        rows.Add(row);
                    }
                }

                List<int> boldRows = new List<int>();
                
                // bold headerRow
                boldRows.Add(1);
                
                generateReport(colNum, headerList, rows, reportName, boldRows);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCStatisticalReports.generateAgeGenderReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void generateChangeStatusEmployeesReport(List<EmployeeTO> emplList, int colNum, List<string> headerList, string reportName, List<int> selectedTypes)
        {
            try
            {
                List<List<string>> rows = new List<List<string>>();
                Dictionary<int, int> emplByCategories = new Dictionary<int, int>();
                Dictionary<int, int> emplByPositions = new Dictionary<int, int>();
                int total = 0;
                List<int> boldRows = new List<int>();                

                foreach (EmployeeTO empl in emplList)
                {
                    if (!selectedTypes.Contains(empl.EmployeeTypeID))
                        continue;

                    if (ascoDict.ContainsKey(empl.EmployeeID))
                    {
                        if (rbHiredEmployees.Checked && ascoDict[empl.EmployeeID].DatetimeValue2 != new DateTime()
                            && ascoDict[empl.EmployeeID].DatetimeValue2.Date >= dtpFrom.Value.Date && ascoDict[empl.EmployeeID].DatetimeValue2.Date <= dtpTo.Value.Date)
                        {
                            // add row to hiring report
                            List<string> row = new List<string>();
                            row.Add(empl.EmployeeID.ToString().Trim());
                            row.Add(empl.FirstName.ToString().Trim());
                            row.Add(empl.LastName.ToString().Trim());
                            if (wuDict.ContainsKey(empl.WorkingUnitID))
                                row.Add(wuDict[empl.WorkingUnitID].Name.Trim());
                            else
                                row.Add("N/A");
                            if (emplTypes.ContainsKey(empl.EmployeeTypeID))
                                row.Add(emplTypes[empl.EmployeeTypeID]);
                            else
                                row.Add("N/A");

                            rows.Add(row);

                            if (!emplByCategories.ContainsKey(empl.EmployeeTypeID))
                                emplByCategories.Add(empl.EmployeeTypeID, 0);

                            emplByCategories[empl.EmployeeTypeID]++;

                            if (!emplByPositions.ContainsKey(empl.WorkingUnitID))
                                emplByPositions.Add(empl.WorkingUnitID, 0);

                            emplByPositions[empl.WorkingUnitID]++;

                            total++;
                        }

                        if (rbLeavingEmployees.Checked && ascoDict[empl.EmployeeID].DatetimeValue3 != new DateTime()
                            && ascoDict[empl.EmployeeID].DatetimeValue3.Date > dtpFrom.Value.Date && ascoDict[empl.EmployeeID].DatetimeValue2.Date <= dtpTo.Value.Date.AddDays(1))
                        {
                            // add row to leaving report
                            List<string> row = new List<string>();
                            row.Add(empl.EmployeeID.ToString().Trim());
                            row.Add(empl.FirstName.ToString().Trim());
                            row.Add(empl.LastName.ToString().Trim());
                            if (wuDict.ContainsKey(empl.WorkingUnitID))
                                row.Add(wuDict[empl.WorkingUnitID].Name.Trim());
                            else
                                row.Add("N/A");
                            if (emplTypes.ContainsKey(empl.EmployeeTypeID))
                                row.Add(emplTypes[empl.EmployeeTypeID]);
                            else
                                row.Add("N/A");

                            rows.Add(row);

                            if (!emplByCategories.ContainsKey(empl.EmployeeTypeID))
                                emplByCategories.Add(empl.EmployeeTypeID, 0);

                            emplByCategories[empl.EmployeeTypeID]++;

                            if (!emplByPositions.ContainsKey(empl.WorkingUnitID))
                                emplByPositions.Add(empl.WorkingUnitID, 0);

                            emplByPositions[empl.WorkingUnitID]++;

                            total++;
                        }
                    }
                }

                if (total > 0)
                {
                    // add header row to bold rows
                    boldRows.Add(1);
                    int rowNum = total + 3;

                    rows.Add(hdrTotalRow("", colNum));
                    rows.Add(hdrTotalRow(rm.GetString("hdrSumCategories", culture), colNum));
                    boldRows.Add(rowNum++);

                    // generate sum rows                
                    foreach (int category in emplByCategories.Keys)
                    {
                        List<string> row = new List<string>();
                        row.Add("");
                        row.Add("");
                        if (emplTypes.ContainsKey(category))
                            row.Add(emplTypes[category]);
                        else
                            row.Add("N/A");
                        row.Add(emplByCategories[category].ToString());
                        row.Add("");
                        rows.Add(row);
                        boldRows.Add(rowNum++);
                    }

                    rows.Add(hdrTotalRow("", colNum));
                    boldRows.Add(rowNum++);
                    rows.Add(hdrTotalRow(rm.GetString("hdrSumPositions", culture), colNum));
                    boldRows.Add(rowNum++);

                    foreach (int wu in emplByPositions.Keys)
                    {
                        List<string> row = new List<string>();
                        row.Add("");
                        row.Add("");
                        if (wuDict.ContainsKey(wu))
                            row.Add(wuDict[wu].Name.Trim());
                        else
                            row.Add("N/A");
                        row.Add(emplByPositions[wu].ToString());
                        row.Add("");
                        rows.Add(row);
                        boldRows.Add(rowNum++);
                    }

                    rows.Add(hdrTotalRow("", colNum));
                    boldRows.Add(rowNum++);

                    // add total line
                    List<string> totalRow = new List<string>();
                    totalRow.Add("");
                    totalRow.Add("");
                    totalRow.Add(rm.GetString("hdrTotal", culture));
                    totalRow.Add(total.ToString().Trim());
                    totalRow.Add("");
                    rows.Add(totalRow);
                    boldRows.Add(rowNum++);
                }

                generateReport(colNum, headerList, rows, reportName, boldRows);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCStatisticalReports.generateChangeStatusEmployeesReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private List<string> hdrTotalRow(string hdr, int colNum)
        {
            try
            {
                List<string> row = new List<string>();
                row.Add(hdr.Trim());
                for (int i = 1; i < colNum; i++)
                {
                    row.Add("");
                }

                return row;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCStatisticalReports.emptyRow(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void generatePositionChangeReport(List<EmployeeTO> emplList, int colNum, List<string> headerList, string reportName, List<int> selectedTypes)
        {
            try
            {
                string emplIDs = "";
                foreach (EmployeeTO empl in emplList)
                {
                    emplIDs += empl.EmployeeID.ToString().Trim() + ",";
                }

                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                Dictionary<int, List<EmployeeHistTO>> emplHistDict = new EmployeeHist().SearchEmployeeChanges(dtpFrom.Value.Date, dtpTo.Value.Date, emplIDs);

                List<List<string>> rows = new List<List<string>>();
                foreach (EmployeeTO empl in emplList)
                {
                    if (!selectedTypes.Contains(empl.EmployeeTypeID))
                        continue;

                    if (!emplHistDict.ContainsKey(empl.EmployeeID))
                        continue;

                    foreach (EmployeeHistTO histTO in emplHistDict[empl.EmployeeID])
                    {
                        if (empl.WorkingUnitID != histTO.WorkingUnitID)
                        {
                            // add row to position change report
                            List<string> row = new List<string>();
                            row.Add(empl.EmployeeID.ToString().Trim());
                            row.Add(empl.FirstName.ToString().Trim());
                            row.Add(empl.LastName.ToString().Trim());
                            if (wuDict.ContainsKey(empl.WorkingUnitID))
                                row.Add(wuDict[empl.WorkingUnitID].Name.Trim());
                            else
                                row.Add("N/A");
                            if (wuDict.ContainsKey(histTO.WorkingUnitID))
                                row.Add(wuDict[histTO.WorkingUnitID].Name.Trim());
                            else
                                row.Add("N/A");
                            if (emplTypes.ContainsKey(empl.EmployeeTypeID))
                                row.Add(emplTypes[empl.EmployeeTypeID]);
                            else
                                row.Add("N/A");

                            rows.Add(row);
                            continue;
                        }
                    }
                }

                List<int> boldRows = new List<int>();

                // bold headerRow
                boldRows.Add(1);

                generateReport(colNum, headerList, rows, reportName, boldRows);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCStatisticalReports.generatePositionChangeReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void generateReport(int colNum, List<string> headerList, List<List<string>> rows, string reportName, List<int> boldRows)
        {
            try
            {
                if (rows.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }

                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                object misValue = System.Reflection.Missing.Value;

                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;

                // insert header
                for (int i = 1; i <= colNum; i++)
                {
                    if (i - 1 < headerList.Count)
                        ws.Cells[1, i] = headerList[i - 1].Trim();
                    else
                        ws.Cells[1, i] = "";
                }
                
                for (int rowNum = 0; rowNum < rows.Count; rowNum++)
                {
                    for (int j = 0; j < rows[rowNum].Count; j++)
                    {
                        ws.Cells[rowNum + 2, j + 1] = rows[rowNum][j];
                    }
                }

                foreach (int rowNum in boldRows)
                {
                    setRowFontWeight(ws, rowNum, colNum, true);
                }

                ws.Columns.AutoFit();
                ws.Rows.AutoFit();

                reportName += DateTime.Now.ToString("ddMMyyyy_HH_mm_ss");

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = reportName;
                sfd.InitialDirectory = Constants.csvDocPath;
                sfd.Filter = "XLS (*.xls)|*.xls";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                string filePath = sfd.FileName;

                wb.SaveAs(filePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
                                    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                                    Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, misValue, misValue, misValue, misValue);

                wb.Close(true, null, null);
                xla.Workbooks.Close();
                xla.Quit();

                releaseObject(ws);
                releaseObject(wb);
                releaseObject(xla);

                System.Threading.Thread.CurrentThread.CurrentCulture = Oldci;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PMCStatisticalReports.generateReport(): " + ex.Message + "\n");
                throw ex;
            }            
        }

        private void setRowFontWeight(Microsoft.Office.Interop.Excel.Worksheet ws, int row, int colNum, bool isBold)
        {
            try
            {
                for (int i = 1; i <= colNum; i++)
                {
                    ((Microsoft.Office.Interop.Excel.Range)ws.Cells[row, i]).Font.Bold = isBold;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCStatisticalReports.setRowFontWeight(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " PMCStatisticalReports.releaseObject(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}

