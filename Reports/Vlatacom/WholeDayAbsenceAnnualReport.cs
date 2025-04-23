using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;

using Common;
using TransferObjects;
using Util;

namespace Reports.Vlatacom
{
    public partial class WholeDayAbsenceAnnualReport : Form
    {
        CultureInfo culture;
        ResourceManager rm;
        ApplUserTO logInUser;
        DebugLog debug;

        // Working Units that logInUser is granted to
        List<WorkingUnitTO> wUnits;
        string wuString;

        List<EmployeeTO> currentEmployees = new List<EmployeeTO>();
        Dictionary<int, EmployeeTO> employeesDic = new Dictionary<int, EmployeeTO>();
       
        // all pass types
        Dictionary<int, PassTypeTO> passTypes = new Dictionary<int, PassTypeTO>();

        private const int ptDayOff = 4;
        private const int ptVacation = 5;
        private const int ptSickLeave = 6;
        private const int ptBusinessTrip = 11;        

        //private Dictionary<int, Color> ptColors = new Dictionary<int, Color>();

        Filter filter;

        public WholeDayAbsenceAnnualReport()
        {
            try
            {
                InitializeComponent();

                // Init debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                debug = new DebugLog(logFilePath);

                // Language tool
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(WholeDayAbsenceAnnualReport).Assembly);
                setLanguage();
                logInUser = NotificationController.GetLogInUser();
                                
                dtpYear.Value = new DateTime(DateTime.Now.Year, 1, 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void populateWorkigUnitCombo()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wuArray = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWorkingUnit.DataSource = wuArray;
                cbWorkingUnit.DisplayMember = "Name";
                cbWorkingUnit.ValueMember = "WorkingUnitID";

                cbWorkingUnit.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " WholeDayAbsenceAnnualReport.populateWorkigUnitCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void WholeDayAbsenceAnnualReport_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                populateWorkigUnitCombo();

                wuString = "";
                foreach (WorkingUnitTO wu in wUnits)
                {
                    wuString += wu.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                populateEmplCombo(-1);

                // load filter
                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
                                
                //get all passTypes
                List<int> isPass = new List<int>();                
                isPass.Add(Constants.wholeDayAbsence);
                List<PassTypeTO> ptList = new PassType().Search(isPass);
                foreach (PassTypeTO pt in ptList)
                {
                    if (!passTypes.ContainsKey(pt.PassTypeID))
                        passTypes.Add(pt.PassTypeID, pt);
                }
                                
                //ptColors.Add(ptVacation, Color.Orange);
                //ptColors.Add(ptSickLeave, Color.Lavender);
                //ptColors.Add(ptBusinessTrip, Color.LightYellow);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " WholeDayAbsenceAnnualReport.WholeDayAbsenceAnnualReport_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateEmplCombo(int wuID)
        {
            try
            {
                string workUnitID = wuID.ToString();
                if (wuID < 0)
                {
                    currentEmployees = new Employee().SearchByWU(wuString);
                }
                else
                {
                    if (chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnit.SelectedValue)
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
                    }
                    currentEmployees = new Employee().SearchByWU(workUnitID);
                }

                // remove retired
                if (!chbShowRetired.Checked)
                {
                    IEnumerator emplEnum = currentEmployees.GetEnumerator();

                    while (emplEnum.MoveNext())
                    {
                        EmployeeTO emplTO = (EmployeeTO)emplEnum.Current;
                        if (emplTO.Status == Constants.statusRetired)
                        {
                            currentEmployees.Remove((EmployeeTO)emplEnum.Current);
                            emplEnum = currentEmployees.GetEnumerator();
                        }
                    }
                }

                foreach (EmployeeTO employee in currentEmployees)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                currentEmployees.Insert(0, empl);

                cbEmployee.DataSource = currentEmployees;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
                cbEmployee.Invalidate();
                cbEmployee.Invalidate();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " WholeDayAbsenceAnnualReport.populateEmplCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("WholeDayAbsenceAnnualReport", culture);

                gbWorkingUnit.Text = rm.GetString("workingUnits", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
                
                lblWorkingUnitName.Text = rm.GetString("lblName", culture);
                lblYear.Text = rm.GetString("lblYear", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);

                btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                btnSaveCriteria.Text = rm.GetString("btnSave", culture);

                chbShowRetired.Text = rm.GetString("chbShowRetired", culture);
                chbHierarhicly.Text = rm.GetString("hierarchically", culture);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " WholeDayAbsenceAnnualReport.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbWorkingUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbWorkingUnit.SelectedIndex >= 0 && cbWorkingUnit.SelectedValue is int)
                    populateEmplCombo((int)cbWorkingUnit.SelectedValue);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " WholeDayAbsenceAnnualReport.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbWorkingUnit.SelectedIndex >= 0 && cbWorkingUnit.SelectedValue is int)
                {
                    if ((int)cbWorkingUnit.SelectedValue >= 0)
                        populateEmplCombo((int)cbWorkingUnit.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " WholeDayAbsenceAnnualReport.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSaveCriteria_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                UIFeatures.FilterAdd filterAdd = new UIFeatures.FilterAdd(this, filter);
                filterAdd.ShowDialog();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " WholeDayAbsenceAnnualReport.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbFilter.SelectedIndex == 0)
                {
                    this.btnSaveCriteria.Text = rm.GetString("SaveFilter", culture);
                }
                else
                {
                    this.btnSaveCriteria.Text = rm.GetString("UpdateFilter", culture);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " WholeDayAbsenceAnnualReport.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbShowRetired_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmplCombo((int)cbWorkingUnit.SelectedValue);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " WholeDayAbsenceAnnualReport.chbShowRetired_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<EmployeeTO> employees = new List<EmployeeTO>();
                List<int> employeesID = new List<int>();
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                if (cbEmployee.SelectedIndex > 0)
                {
                    // calculate for selected employee
                    employees.Add((EmployeeTO)cbEmployee.SelectedItem);
                }
                else
                {
                    // calculate for all employees from combo box
                    employees = currentEmployees;
                }

                foreach (EmployeeTO empl in employees)
                {
                    // Employee IDs
                    if (empl.EmployeeID > -1)
                    {
                        employeesID.Add(empl.EmployeeID);

                        if (!employeesDic.ContainsKey(empl.EmployeeID))
                            employeesDic.Add(empl.EmployeeID, empl);
                    }
                }

                if (employeesID.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noCSVExport", culture));
                    return;
                }

                // get all valid IO Pairs for selected employees and time interval
                // io pairs are sorted by employee, io_pair_date, iopair start_time ascending
                List<int> passTypes = new List<int>();
                passTypes.Add(ptDayOff);
                passTypes.Add(ptVacation);
                passTypes.Add(ptSickLeave);
                passTypes.Add(ptBusinessTrip);
                List<IOPairTO> ioPairList = new IOPair().SearchWholeDayAbsenceIOPairs(employeesID, passTypes, dtpYear.Value.Year);

                // Key is Employee ID, value is List of valid IO Pairs for that Employee
                Dictionary<int, List<IOPairTO>> emplPairs = new Dictionary<int, List<IOPairTO>>();

                foreach (int emplID in employeesID)
                {
                    emplPairs.Add(emplID, new List<IOPairTO>());
                }

                // io pairs for particular employee will be sorted by io_pair_date, start_time ascending
                for (int i = 0; i < ioPairList.Count; i++)
                {
                    // add only pairs longer then 0min
                    if (ioPairList[i].EndTime.Subtract(ioPairList[i].StartTime).TotalMinutes > 0)
                        emplPairs[ioPairList[i].EmployeeID].Add(ioPairList[i]);
                }
                
                // key is employee id, value is dictionary (key is month, value is dictionary of day num by absence type)
                Dictionary<int, Dictionary<int, Dictionary<int, int>>> emplAbsenceDayNum = new Dictionary<int,Dictionary<int,Dictionary<int,int>>>();

                // sum by employee and pass types - key is employee id, value is dictionary (key is pass type, value is num of absences of that type)
                Dictionary<int, Dictionary<int, int>> emplDayNumByPassType = new Dictionary<int, Dictionary<int, int>>();

                // sum by months pass types - key is month, value is num of absences in that month
                Dictionary<int, Dictionary<int, int>> monthDayNumByPassTypes = new Dictionary<int, Dictionary<int, int>>();

                for (int i = 1; i <= 12; i++)
                {
                    monthDayNumByPassTypes.Add(i, new Dictionary<int, int>());
                }

                // calculate day num by pass types for each employee
                foreach (int emplID in employeesID)
                {
                    // key is absence pass type id, value is total hours of that pass type
                    Dictionary<int, Dictionary<int, int>> absenceDayNumByMonth = new Dictionary<int, Dictionary<int, int>>();

                    // key is absence pass type id, value is total hours of that pass type
                    Dictionary<int, int> absenceDayNum = new Dictionary<int, int>();

                    if (!emplAbsenceDayNum.ContainsKey(emplID))
                        emplAbsenceDayNum.Add(emplID, new Dictionary<int, Dictionary<int, int>>());

                    if (!emplDayNumByPassType.ContainsKey(emplID))
                        emplDayNumByPassType.Add(emplID, new Dictionary<int, int>());

                    foreach (IOPairTO pair in emplPairs[emplID])
                    {
                        if (!emplAbsenceDayNum[emplID].ContainsKey(pair.IOPairDate.Month))
                            emplAbsenceDayNum[emplID].Add(pair.IOPairDate.Month, new Dictionary<int, int>());

                        if (!emplAbsenceDayNum[emplID][pair.IOPairDate.Month].ContainsKey(pair.PassTypeID))
                            emplAbsenceDayNum[emplID][pair.IOPairDate.Month].Add(pair.PassTypeID, 1);
                        else
                            emplAbsenceDayNum[emplID][pair.IOPairDate.Month][pair.PassTypeID]++;

                        if (!emplDayNumByPassType[emplID].ContainsKey(pair.PassTypeID))
                            emplDayNumByPassType[emplID].Add(pair.PassTypeID, 1);
                        else
                            emplDayNumByPassType[emplID][pair.PassTypeID]++;

                        if (!monthDayNumByPassTypes[pair.IOPairDate.Month].ContainsKey(pair.PassTypeID))
                            monthDayNumByPassTypes[pair.IOPairDate.Month].Add(pair.PassTypeID, 1);
                        else
                            monthDayNumByPassTypes[pair.IOPairDate.Month][pair.PassTypeID]++;
                    }
                }

                // if there are no data for report
                if (emplAbsenceDayNum.Count == 0)
                {
                    MessageBox.Show(rm.GetString("noCSVExport", culture));
                    return;
                }

                // generate report
                generateReport(emplAbsenceDayNum, emplDayNumByPassType, monthDayNumByPassTypes);

                this.Close();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " WholeDayAbsenceAnnualReport.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void generateReport(Dictionary<int, Dictionary<int, Dictionary<int, int>>> emplAbsenceDayNum, Dictionary<int, Dictionary<int, int>> emplDayNumByPassType, Dictionary<int, Dictionary<int, int>> monthDayNum)
        {
            try
            {
                if (emplAbsenceDayNum.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noCSVExport", culture));
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = "CelodnevnaOdsustva_GodisnjiIzvestaj_" + DateTime.Now.ToString("dd_MM_yyy_HH_mm") + ".xls";
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                sfd.Filter = "XLS (*.xls)|*.xls";                
                                
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;
                
                string filePath = sfd.FileName;
                
                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;

                object misValue = System.Reflection.Missing.Value;

                int i = 1;
                int j = 1;
                int num = 1;

                // insert year in first row
                //ws.Cells.NumberFormat = "@";
                ws.Cells[i, j] = dtpYear.Value.Year.ToString().Trim();
                setCellFontWeight(ws.Cells[i, j], true);

                // next row is header (rbr, employee, months, sum)
                i++;
                ws.Cells[i, 1] = "rbr";
                ws.Cells[i, 2] = "Zaposleni";
                ws.Cells[i, 3] = "Tip";
                for (j = 4; j < 16; j++)
                {
                    ws.Cells[i, j] = (j - 3).ToString().Trim();                    
                }

                ws.Cells[i, j] = "SUMA";
                
                // set header color
                for (j = 1; j < 17; j++)
                {
                    //setCellColor(ws.Cells[i, j], Color.Silver);
                    setCellFontWeight(ws.Cells[i, j], true);
                }

                i++;
                foreach (int emplID in emplAbsenceDayNum.Keys)
                {
                    // rbr
                    ws.Cells[i, 1] = num.ToString();
                    setCellFontWeight(ws.Cells[i, 1], true);

                    // employee name
                    if (employeesDic.ContainsKey(emplID))
                        ws.Cells[i, 2] = employeesDic[emplID].LastName.Trim();
                    else
                        ws.Cells[i, 2] = "";
                    setCellFontWeight(ws.Cells[i, 2], true);

                    // absence type
                    if (passTypes.ContainsKey(ptDayOff))
                        ws.Cells[i, 3] = passTypes[ptDayOff].Description.Trim();
                    else
                        ws.Cells[i, 3] = "";
                    if (passTypes.ContainsKey(ptVacation))
                        ws.Cells[i + 1, 3] = passTypes[ptVacation].Description.Trim();
                    else
                        ws.Cells[i + 1, 3] = "";                    

                    if (passTypes.ContainsKey(ptSickLeave))
                        ws.Cells[i + 2, 3] = passTypes[ptSickLeave].Description.Trim();
                    else
                        ws.Cells[i + 2, 3] = "";

                    if (passTypes.ContainsKey(ptBusinessTrip))
                        ws.Cells[i + 3, 3] = passTypes[ptBusinessTrip].Description.Trim();
                    else
                        ws.Cells[i + 3, 3] = "";

                    for (j = 4; j < 16; j++)
                    {
                        // pass type day off
                        if (emplAbsenceDayNum[emplID].ContainsKey(j - 3) && emplAbsenceDayNum[emplID][j - 3].ContainsKey(ptDayOff))
                            ws.Cells[i, j] = emplAbsenceDayNum[emplID][j - 3][ptDayOff].ToString().Trim();
                        else
                            ws.Cells[i, j] = "";
                        // pass type vacation
                        if (emplAbsenceDayNum[emplID].ContainsKey(j - 3) && emplAbsenceDayNum[emplID][j - 3].ContainsKey(ptVacation))
                            ws.Cells[i + 1, j] = emplAbsenceDayNum[emplID][j - 3][ptVacation].ToString().Trim();
                        else
                            ws.Cells[i + 1, j] = "";                        

                        // pass type sick leave
                        if (emplAbsenceDayNum[emplID].ContainsKey(j - 3) && emplAbsenceDayNum[emplID][j - 3].ContainsKey(ptSickLeave))
                            ws.Cells[i + 2, j] = emplAbsenceDayNum[emplID][j - 3][ptSickLeave].ToString().Trim();
                        else
                            ws.Cells[i + 2, j] = "";
                        
                        // pass type business trip
                        if (emplAbsenceDayNum[emplID].ContainsKey(j - 3) && emplAbsenceDayNum[emplID][j - 3].ContainsKey(ptBusinessTrip))
                            ws.Cells[i + 3, j] = emplAbsenceDayNum[emplID][j - 3][ptBusinessTrip].ToString().Trim();
                        else
                            ws.Cells[i + 3, j] = "";                        
                    }
                    // day off sum
                    if (emplDayNumByPassType.ContainsKey(emplID) && emplDayNumByPassType[emplID].ContainsKey(ptDayOff))
                        ws.Cells[i, j] = emplDayNumByPassType[emplID][ptDayOff].ToString().Trim();
                    else
                        ws.Cells[i, j] = "0";

                    // vacation sum
                    if (emplDayNumByPassType.ContainsKey(emplID) && emplDayNumByPassType[emplID].ContainsKey(ptVacation))
                        ws.Cells[i + 1, j] = emplDayNumByPassType[emplID][ptVacation].ToString().Trim();
                    else
                        ws.Cells[i + 1, j] = "0";
                    //setCellColor(ws.Cells[i, j], ptColors[ptVacation]);

                    // sick leave sum
                    if (emplDayNumByPassType.ContainsKey(emplID) && emplDayNumByPassType[emplID].ContainsKey(ptSickLeave))
                        ws.Cells[i + 2, j] = emplDayNumByPassType[emplID][ptSickLeave].ToString().Trim();
                    else
                        ws.Cells[i + 2, j] = "0";
                    //setCellColor(ws.Cells[i + 1, j], ptColors[ptSickLeave]);

                    // buisness sum
                    if (emplDayNumByPassType.ContainsKey(emplID) && emplDayNumByPassType[emplID].ContainsKey(ptBusinessTrip))
                        ws.Cells[i + 3, j] = emplDayNumByPassType[emplID][ptBusinessTrip].ToString().Trim();
                    else                    
                        ws.Cells[i + 3, j] = "0";
                    //setCellColor(ws.Cells[i + 2, j], ptColors[ptBusinessTrip]);

                    i += 4;
                    num++;
                }

                ws.Cells[i, 2] = "SUMA (po mesecima)";
                setCellFontWeight(ws.Cells[i, 2], true);

                // absence type
                if (passTypes.ContainsKey(ptDayOff))
                    ws.Cells[i, 3] = passTypes[ptDayOff].Description.Trim();
                else
                    ws.Cells[i, 3] = "";

                if (passTypes.ContainsKey(ptVacation))
                    ws.Cells[i + 1, 3] = passTypes[ptVacation].Description.Trim();
                else
                    ws.Cells[i + 1, 3] = "";

                if (passTypes.ContainsKey(ptSickLeave))
                    ws.Cells[i + 2, 3] = passTypes[ptSickLeave].Description.Trim();
                else
                    ws.Cells[i + 2, 3] = "";
                
                if (passTypes.ContainsKey(ptBusinessTrip))
                    ws.Cells[i + 3, 3] = passTypes[ptBusinessTrip].Description.Trim();
                else
                    ws.Cells[i + 3, 3] = "";

                ws.Cells[i + 4, 3] = "TOTAL";
                setCellFontWeight(ws.Cells[i + 4, 3], true);

                int totalDayOff = 0;
                int totalVacation = 0;
                int totalSickLeave = 0;
                int totalBusinessTrip = 0;
                int totalsByMonths = 0;

                // monthly sum by pass types
                for (j = 4; j < 16; j++)
                {
                    totalsByMonths = 0;

                    if (monthDayNum.ContainsKey(j - 3) && monthDayNum[j - 3].ContainsKey(ptDayOff))
                    {
                        ws.Cells[i, j] = monthDayNum[j - 3][ptDayOff].ToString().Trim();
                        totalDayOff += monthDayNum[j - 3][ptDayOff];
                        totalsByMonths += monthDayNum[j - 3][ptDayOff];
                    }
                    else
                        ws.Cells[i, j] = "0";

                    if (monthDayNum.ContainsKey(j - 3) && monthDayNum[j - 3].ContainsKey(ptVacation))
                    {
                        ws.Cells[i + 1, j] = monthDayNum[j - 3][ptVacation].ToString().Trim();
                        totalVacation += monthDayNum[j - 3][ptVacation];
                        totalsByMonths += monthDayNum[j - 3][ptVacation];
                    }
                    else
                        ws.Cells[i + 1, j] = "0";

                    if (monthDayNum.ContainsKey(j - 3) && monthDayNum[j - 3].ContainsKey(ptSickLeave))
                    {
                        ws.Cells[i + 2, j] = monthDayNum[j - 3][ptSickLeave].ToString().Trim();
                        totalSickLeave += monthDayNum[j - 3][ptSickLeave];
                        totalsByMonths += monthDayNum[j - 3][ptSickLeave];
                    }
                    else
                        ws.Cells[i + 2, j] = "0";

                    if (monthDayNum.ContainsKey(j - 3) && monthDayNum[j - 3].ContainsKey(ptBusinessTrip))
                    {
                        ws.Cells[i + 3, j] = monthDayNum[j - 3][ptBusinessTrip].ToString().Trim();
                        totalBusinessTrip += monthDayNum[j - 3][ptBusinessTrip];
                        totalsByMonths += monthDayNum[j - 3][ptBusinessTrip];
                    }
                    else
                        ws.Cells[i + 3, j] = "0";

                    ws.Cells[i + 4, j] = totalsByMonths.ToString();
                    setCellFontWeight(ws.Cells[i + 4, j], true);
                }

                ws.Cells[i, 16] = totalDayOff.ToString();
                ws.Cells[i +1, 16] = totalVacation.ToString();
                ws.Cells[i + 2, 16] = totalSickLeave.ToString();
                ws.Cells[i + 3, 16] = totalBusinessTrip.ToString();
                ws.Cells[i + 4, 16] = (totalDayOff + totalVacation + totalSickLeave + totalBusinessTrip).ToString();
                setCellFontWeight(ws.Cells[i + 4, 16], true);

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

                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " WholeDayAbsenceAnnualReport.generateCSVReport(): " + ex.Message + "\n");
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
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WholeDayAbsenceAnnualReport.releaseObject(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        //private void setCellColor(object cell, Color color)
        //{
        //    try
        //    {
        //        ((Microsoft.Office.Interop.Excel.Range)cell).Interior.Color = System.Drawing.ColorTranslator.ToOle(color);
        //    }
        //    catch (Exception ex)            
        //    {
        //        debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WholeDayAbsenceAnnualReport.setCellColor(): " + ex.Message + "\n");
        //        throw ex;
        //    }
        //}

        private void setCellFontWeight(object cell, bool isBold)
        {
            try
            {
                ((Microsoft.Office.Interop.Excel.Range)cell).Font.Bold = isBold;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " WholeDayAbsenceAnnualReport.setCellFontWeight(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
