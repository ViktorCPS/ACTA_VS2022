using System;
using System.Collections;
using System.Collections.Generic;
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
using DataAccess;

namespace Reports.Niksic
{
    public partial class NiksicMonthlyReport : Form
    {
        // Language settings
        private CultureInfo culture;
        private ResourceManager rm;

        // Debug
        DebugLog debug;
        ApplUserTO logInUser;

        private List<WorkingUnitTO> wUnits;
        private string wuString = "";

        List<EmployeeTO> emplArray = new List<EmployeeTO>();

        public NiksicMonthlyReport()
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                // Init debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                debug = new DebugLog(logFilePath);

                // Language tool
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(NiksicMonthlyReport).Assembly);
                setLanguage();

                // Get User that is login and Working Units that User is granted to
                logInUser = NotificationController.GetLogInUser();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("monthlyReport", culture);

                gbEmployees.Text = rm.GetString("gbEmployees", culture);
                                
                chbHierarhicly.Text = rm.GetString("hierarchically", culture);

                this.btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
                this.btnCancel.Text = rm.GetString("btnCancel", culture);

                lblWorkingUnit.Text = rm.GetString("lblWorkingUnits", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblMonth.Text = rm.GetString("lblMonth", culture);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " NiksicMonthlyReport.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NiksicMonthlyReport_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                dtpMonth.Value = DateTime.Now.Date;

                wUnits = new List<WorkingUnitTO>();
                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                populateWorkingUnits();
                populateEmployeeCombo(-1);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " NiksicMonthlyReport.NiksicMonthlyReport_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateWorkingUnits()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    wuArray.Add(wuTO);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWorkingUnit.DataSource = wuArray;
                cbWorkingUnit.DisplayMember = "Name";
                cbWorkingUnit.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " NiksicMonthlyReport.populateWorkingUnits(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateEmployeeCombo(int wuID)
        {
            try
            {
                emplArray = new List<EmployeeTO>();
                string workUnitID = wuID.ToString();
                if (wuID < 0)
                {
                    emplArray = new Employee().SearchByWU(wuString);
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
                                        
                    emplArray = new Employee().SearchByWU(workUnitID);
                }

                foreach (EmployeeTO employee in emplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                emplArray.Insert(0, empl);

                cbEmployee.DataSource = emplArray;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
                cbEmployee.Invalidate();
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " NiksicMonthlyReport.populateEmployeeCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void cbWorkingUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbWorkingUnit.SelectedValue is int)
                {
                    this.Cursor = Cursors.WaitCursor;
                    populateEmployeeCombo((int)cbWorkingUnit.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " NiksicMonthlyReport.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                throw ex;
            }
            finally 
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbWorkingUnit.SelectedValue is int)
                {
                    this.Cursor = Cursors.WaitCursor;
                    populateEmployeeCombo((int)cbWorkingUnit.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " NiksicMonthlyReport.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DataSet dataSetCR = new DataSet();
                DataTable tableCR = new DataTable("monthlyReport");

                tableCR.Columns.Add("employee_id", typeof(System.String));

                dataSetCR.Tables.Add(tableCR);

                Employee empl = new Employee();
                List<EmployeeTO> employees = new List<EmployeeTO>();

                if (cbEmployee.SelectedIndex > 0)
                {
                    empl.EmplTO.EmployeeID = (int)cbEmployee.SelectedValue;
                    employees = empl.Search();
                    if (employees.Count > 0)
                    {
                        empl.EmplTO = employees[0];

                        for (int i = 1; i < 26; i++)
                        {
                            DataRow row = tableCR.NewRow();
                            row["employee_id"] = empl.EmplTO.EmployeeID.ToString().Trim();
                            tableCR.Rows.Add(row);
                            tableCR.AcceptChanges();
                        }
                    }
                }

                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                {
                    Reports.Niksic.Niksic_sr.MonthlyReportCRView view =
                        new Reports.Niksic.Niksic_sr.MonthlyReportCRView(dataSetCR, empl.EmplTO.WorkingUnitName.Trim(), empl.EmplTO.LastName.Trim() + " " 
                        + empl.EmplTO.FirstName.Trim(), empl.EmplTO.EmployeeID, dtpMonth.Value.Date);
                    view.ShowDialog(this);
                }
                //else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                //{
                //    Reports.Niksic.Niksic_en.MonthlyReportCRView view =
                //        new Reports.Niksic.Niksic_en.MonthlyReportCRView();
                //    view.ShowDialog(this);
                //}
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " NiksicMonthlyReport.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}