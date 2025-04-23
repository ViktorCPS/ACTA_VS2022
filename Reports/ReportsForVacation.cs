using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using TransferObjects;
using Util;
using Common;
using System.Threading;
using Microsoft.Office.Interop.Excel;

namespace Reports {
    public partial class ReportsForVacation : Form {

        CultureInfo culture;
        ResourceManager rm;
        ApplUserTO logInUser;
        DebugLog log;

        public ReportsForVacation() {
            InitializeComponent();

            string logFilePath = Util.Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(ReportsForVacation).Assembly);

            setLanguage();
        }

        private void setLanguage() {

            lblDate.Text = rm.GetString("lblDate", culture);
            btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
            btnCancel.Text = rm.GetString("btnCancel", culture);

            this.Text = rm.GetString("ReportsForVacation", culture);
        }

        private void btnGenerate_Click(object sender, EventArgs e) {
            try {
                this.Cursor = Cursors.WaitCursor;

                List<EmployeeTO> employees = new Employee().GetNumberOfDaysVacationPerEmployees(dtpFromDate.Value.Date);

                if (employees.Count == 0) {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("dataNotFound", culture));
                    return;
                }

                if (NotificationController.GetLanguage().Equals(Util.Constants.Lang_sr)) {
                    Reports_sr.EmployeesVacationCRView view = new Reports_sr.EmployeesVacationCRView(
                        this.PopulateCRData(employees,false),
                        dtpFromDate.Value.Date);
                    view.ShowDialog(this);
                }
                else if (NotificationController.GetLanguage().Equals(Util.Constants.Lang_en)) {
                    Reports_en.EmployeesVacationCRView_en view = new Reports_en.EmployeesVacationCRView_en(
                        this.PopulateCRData(employees,false),
                        dtpFromDate.Value);
                    view.ShowDialog(this);
                }
            }
            catch (Exception ex) {

                log.writeLog(DateTime.Now + " ReportsForVacation.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private DataSet PopulateCRData(List<EmployeeTO> dataList, bool ExportExcel) {
            DataSet dataSet = new DataSet();
            System.Data.DataTable table = new System.Data.DataTable("employees_vacation");
            List<VacationReportTO> vacationReportToList = new List<VacationReportTO>();
            table.Columns.Add("empl_id", typeof(int));
            table.Columns.Add("full_name", typeof(System.String));
            table.Columns.Add("orgUnit_name", typeof(System.String));
            table.Columns.Add("workingUnit_name", typeof(System.String));
            table.Columns.Add("division_name", typeof(System.String));
            table.Columns.Add("vacation_thisYear", typeof(int));
            table.Columns.Add("vacation_lastYear", typeof(int));
            table.Columns.Add("vacation_used", typeof(int));
            table.Columns.Add("days_left_previous_year",typeof(int));
            table.Columns.Add("days_left_current_year",typeof(int));
            table.Columns.Add("spent_ytd", typeof(int));
            table.Columns.Add("t_max", typeof(int));
            table.Columns.Add("accrual_in_days_ytd", typeof(int));
            table.Columns.Add("imageID", typeof(byte));

            System.Data.DataTable tableI = new System.Data.DataTable("images");
            tableI.Columns.Add("image", typeof(System.Byte[]));
            tableI.Columns.Add("imageID", typeof(byte));

            try {
                int i = 0;
                foreach (EmployeeTO employeeTO in dataList) {
                    DataRow row = table.NewRow();
                    VacationReportTO vacationReportTo = new VacationReportTO();
                    row["empl_id"] = employeeTO.EmployeeID.ToString();
                    vacationReportTo.emp_id = employeeTO.EmployeeID;
                    row["full_name"] = employeeTO.FirstName;
                    vacationReportTo.fullName = employeeTO.FirstName;
                    row["orgUnit_name"] = employeeTO.OrgUnitName;
                    vacationReportTo.orgUnit_name = employeeTO.OrgUnitName;
                    if (!employeeTO.OrgUnitName.Contains("Visitor"))
                    {
                        row["workingUnit_name"] = employeeTO.WorkingUnitName;
                        vacationReportTo.workingUnit_name = employeeTO.WorkingUnitName;
                        row["division_name"] = employeeTO.Division;
                        vacationReportTo.division_name = employeeTO.Division;
                        DataRow row2 = row;
                        int num2 = employeeTO.VacationThisYear;
                        string str2 = num2.ToString();
                        row2["vacation_thisYear"] = str2;
                        vacationReportTo.vacation_thisYear = employeeTO.VacationThisYear;
                        DataRow row3 = row;
                        num2 = employeeTO.VacationLastYear;
                        string str3 = num2.ToString();
                        row3["vacation_lastYear"] = str3;
                        vacationReportTo.vacation_lastYear = employeeTO.VacationLastYear;
                        DataRow row4 = row;
                        num2 = employeeTO.VacationUsed;
                        string str4 = num2.ToString();
                        row4["vacation_used"] = str4;
                        vacationReportTo.vacation_used = employeeTO.VacationUsed;
                        int daysLeftLast = this.CalculateDaysLeftLast(employeeTO);
                        int daysLeftCurrent = this.CalculateDaysLeftCurrent(employeeTO);
                        int spentYTD = employeeTO.VacationThisYear - daysLeftCurrent;
                        int tmax = this.CalculateTMax(employeeTO);
                        int accrual = this.CalculateAccrual(employeeTO, tmax, spentYTD);
                        row["days_left_previous_year"] = daysLeftLast.ToString();
                        vacationReportTo.days_left_previous_year = daysLeftLast;
                        row["days_left_current_year"] = daysLeftCurrent.ToString();
                        vacationReportTo.days_left_current_year = daysLeftCurrent;
                        row["spent_ytd"] = spentYTD.ToString();
                        vacationReportTo.spent_ytd = spentYTD;
                        row["t_max"] = tmax.ToString();
                        vacationReportTo.t_max = tmax;
                        row["accrual_in_days_ytd"] = accrual.ToString();
                        vacationReportTo.accrual_in_days_ytd = accrual;
                        row["imageID"] = 1;
                        if (i == 0)
                        {
                            DataRow dataRow = tableI.NewRow();
                            dataRow["image"] = Util.Constants.LogoForReport;
                            dataRow["imageID"] = 1;
                            tableI.Rows.Add(dataRow);
                            tableI.AcceptChanges();
                        }
                        table.Rows.Add(row);
                        i++;
                        vacationReportToList.Add(vacationReportTo);
                    }
                }

                table.AcceptChanges();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " ReportsForVacation.PopulateCRData(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            dataSet.Tables.Add(table);
            if (ExportExcel)
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                        Microsoft.Office.Interop.Excel.Application app= new Microsoft.Office.Interop.Excel.Application();
                        Workbook wb = app.Workbooks.Add(XlSheetType.xlWorksheet);
                        Worksheet ws = (Worksheet)app.ActiveSheet;
                        app.Visible = false;
                        ws.Cells[1, 1] = "Employee ID";
                        ws.Cells[1, 2] = "Name";
                        ws.Cells[1, 3] = "Organization Unit";
                        ws.Cells[1, 4] = "Working Unit";
                        ws.Cells[1, 5] = "Divison";
                        ws.Cells[1, 6] = "Vacation Days(This Year)";
                        ws.Cells[1, 7] = "Vacation Days(Last Year)";
                        ws.Cells[1, 8] = "Vacation Days Used";
                        ws.Cells[1, 9] = "Days Left (Previous Year)";
                        ws.Cells[1, 10] = "Days Left (Current Year)";
                        ws.Cells[1, 11] = "Spent YTD";
                        ws.Cells[1, 12] = "Theoretical Maximum";
                        ws.Cells[1, 13] = "Accrual In Days YTD";
                        int red = 2;
                        foreach (VacationReportTO item in vacationReportToList)
                        {
                            ws.Cells[red, 1] = item.emp_id.ToString();
                            ws.Cells[red, 2] = item.fullName.ToString();
                            ws.Cells[red, 3] = item.orgUnit_name.ToString();
                            ws.Cells[red, 4] = item.workingUnit_name.ToString();
                            ws.Cells[red, 5] = item.division_name.ToString();
                            ws.Cells[red, 6] = item.vacation_thisYear.ToString();
                            ws.Cells[red, 7] = item.vacation_lastYear.ToString();
                            ws.Cells[red, 8] = item.vacation_used.ToString();
                            ws.Cells[red, 9] = item.days_left_previous_year.ToString();
                            ws.Cells[red, 10] = item.days_left_current_year.ToString();
                            ws.Cells[red, 11] = item.spent_ytd.ToString();
                            ws.Cells[red, 12] = item.t_max.ToString();
                            ws.Cells[red, 13] = item.accrual_in_days_ytd.ToString();
                            red++;
                        }
                        ws.Columns.AutoFit();
                        wb.SaveAs(sfd.FileName, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, true, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing,Type.Missing,Type.Missing);
                        app.Visible = true;
                        app.Quit();
                    }
                }
            }
            dataSet.Tables.Add(tableI);
            return dataSet;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<EmployeeTO> vacationPerEmployees = new Employee().GetNumberOfDaysVacationPerEmployees(this.dtpFromDate.Value.Date);
            if (vacationPerEmployees.Count == 0)
            {
                this.Cursor = Cursors.Arrow;
                int num = (int)MessageBox.Show(this.rm.GetString("dataNotFound", this.culture));
            }
            else
                this.PopulateCRData(vacationPerEmployees, true);
        }
        private int CalculateDaysLeftLast(EmployeeTO employeeTO)
        {
            int daysLeft2019 = 0;
            if (employeeTO.VacationLastYear > employeeTO.VacationUsed)
                daysLeft2019 = employeeTO.VacationLastYear - employeeTO.VacationUsed;
            return daysLeft2019;
        }
        private int CalculateDaysLeftCurrent(EmployeeTO employeeTO)
        {
            int daysLeft2020 = employeeTO.VacationThisYear + employeeTO.VacationLastYear - employeeTO.VacationUsed;
            if (employeeTO.VacationLastYear > employeeTO.VacationUsed)
                daysLeft2020 = employeeTO.VacationThisYear;
            return daysLeft2020;
        }
        private int CalculateTMax(EmployeeTO employeeTO)
        {
            return Convert.ToInt32(Math.Round((double)DateTime.Now.Month * 1.75, MidpointRounding.AwayFromZero));
        }
        private int CalculateAccrual(EmployeeTO employeeTO, int tMax, int spentYTD)
        {
            int accrual = 0;
            if (tMax > spentYTD)
                accrual = tMax - spentYTD;
            return accrual;
        }
    }
}
