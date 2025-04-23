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

namespace Reports.PIO
{
    public partial class WorkLists : Form
    {
        ApplUserTO logInUser;
        ResourceManager rm;
        private CultureInfo culture;
        DebugLog log;

        // Working Units that logInUser is granted to
        //ArrayList wUnits;
        private string wuString = "";

        Filter filter;

        public WorkLists()
        {
            try
            {
                InitializeComponent();

                // Init debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();
                this.CenterToScreen();

                // Language tool
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(WorkingUnitsReports).Assembly);
                setLanguage();

                populateWorkigUnitCombo();

                DateTime date = DateTime.Now.Date;
                dtpFromDate.Value = new DateTime(date.Year, date.Month, 1);
                dtpToDate.Value = date;

                rbSummary.Checked = true;
                cbEmployee.Enabled = false;

                if (wuString.Equals(""))
                {
                    MessageBox.Show(rm.GetString("noEmplReportPrivilege", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkLists.WorkLists(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("WorkListsReport", culture);

                // button's text
                btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);

                // group box text
                gbWorkingUnit.Text = rm.GetString("workingUnits", culture);
                gbEmployee.Text = rm.GetString("gbEmployees", culture);
                gbTimeInterval.Text = rm.GetString("timeInterval", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);

                // label's text
                lblWorkingUnitName.Text = rm.GetString("lblName", culture);
                lbEmployeeName.Text = rm.GetString("lblName", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblReportType.Text = rm.GetString("reportType", culture);

                // check box text
                chbHierarhicly.Text = rm.GetString("hierarchically", culture);

                // radio button's text
                rbAnalytical.Text = rm.GetString("analitycal", culture);
                rbSummary.Text = rm.GetString("summary", culture);
                rbAbsences.Text = rm.GetString("absences", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkLists.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateWorkigUnitCombo()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wuArray = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                    //wUnits = (ArrayList)wuArray.Clone();
                }

                foreach (WorkingUnitTO wUnit in wuArray)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWorkingUnit.DataSource = wuArray;
                cbWorkingUnit.DisplayMember = "Name";
                cbWorkingUnit.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkLists.populateWorkigUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateEmployeeCombo(List<EmployeeTO> array)
        {
            try
            {
                foreach (EmployeeTO employee in array)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                cbEmployee.DataSource = array;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkLists.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbWorkingUnit.Items.Count > 1)
                {
                    if (this.dtpFromDate.Value <= this.dtpToDate.Value)
                    {
                        if (rbSummary.Checked)
                        {
                            int wuIndex = 0;
                            if (cbWorkingUnit.SelectedIndex >= 0)
                                wuIndex = cbWorkingUnit.SelectedIndex;
                            PIOPaymentReports paymentReport = new PIOPaymentReports(wuIndex, chbHierarhicly.Checked, dtpFromDate.Value, 
                                dtpToDate.Value, -1, false);
                            paymentReport.btnGenerateReport_Click(sender, e);
                        }
                        else if (rbAnalytical.Checked)
                        {
                            if ((cbEmployee.Items.Count > 0)
                                && (cbEmployee.SelectedIndex >= 0))
                            {
                                int wuIndex = 0;
                                if (cbWorkingUnit.SelectedIndex >= 0)
                                    wuIndex = cbWorkingUnit.SelectedIndex;
                                
                                int emplIndex = (int)cbEmployee.SelectedValue;
                                PIOPaymentReports paymentReport = new PIOPaymentReports(wuIndex, false, dtpFromDate.Value, 
                                    dtpToDate.Value, emplIndex, false);
                                paymentReport.btnGenerateReport_Click(sender, e);
                            }
                            else
                            {
                                MessageBox.Show(rm.GetString("employeeNotNull", culture));
                                return;
                            }
                        }
                        else if (rbAbsences.Checked)
                        {
                            int wuIndex = 0;
                            if (cbWorkingUnit.SelectedIndex >= 0)
                                wuIndex = cbWorkingUnit.SelectedIndex;
                            PIOPaymentReports paymentReport = new PIOPaymentReports(wuIndex, chbHierarhicly.Checked, dtpFromDate.Value, 
                                dtpToDate.Value, -1, true);
                            paymentReport.btnGenerateReport_Click(sender, e);
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("wrongDatePickUp", culture));
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noEmplReportPrivilege", culture));
                    return;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkLists.btnGenerate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbSummary_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbSummary.Checked)
                {
                    cbEmployee.Enabled = false;
                    chbHierarhicly.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkLists.rbSummary_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbAnalytical_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
            
                if (rbAnalytical.Checked)
                {
                    cbEmployee.Enabled = true;
                    chbHierarhicly.Enabled = false;

                    if (!wuString.Equals(""))
                    {
                        Employee empl = new Employee();
                        List<EmployeeTO> emplList = new List<EmployeeTO>();

                        if (cbWorkingUnit.SelectedIndex > 0)
                        {
                            empl.EmplTO.WorkingUnitID = (int)cbWorkingUnit.SelectedValue;
                            emplList = empl.Search();
                        }
                        else
                        {
                            emplList = empl.SearchByWU(wuString);
                        }

                        populateEmployeeCombo(emplList);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkLists.rbAnalytical_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbAbsences_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbAbsences.Checked)
                {
                    cbEmployee.Enabled = false;
                    chbHierarhicly.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkLists.rbAbsences_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbWorkingUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (rbAnalytical.Checked)
                {
                    if (!wuString.Equals(""))
                    {
                        Employee empl = new Employee();
                        List<EmployeeTO> emplList = new List<EmployeeTO>();

                        if (cbWorkingUnit.SelectedIndex > 0)
                        {
                            empl.EmplTO.WorkingUnitID = (int)cbWorkingUnit.SelectedValue;
                            emplList = empl.Search();
                        }
                        else
                        {
                            emplList = empl.SearchByWU(wuString);
                        }

                        populateEmployeeCombo(emplList);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkLists.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {

                this.Cursor = Cursors.Arrow;
            }
        }

        private void WorkLists_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MittalWorkingUnitsReports.MittalWorkingUnitsReports_Load(): " + ex.Message + "\n");
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
            }
            finally
            {

                this.Cursor = Cursors.Arrow;
            }
        }
    }
}