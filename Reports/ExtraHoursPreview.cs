using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.Collections;

using Util;
using Common;
using TransferObjects;

namespace Reports
{
    public partial class ExtraHoursPreview : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        private List<WorkingUnitTO> wUnits;
        private string wuString = "";

        Filter filter;

        List<EmployeeTO> emplArray = new List<EmployeeTO>();
        
        public ExtraHoursPreview()
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(ExtraHoursPreview).Assembly);

            setLanguage();

            logInUser = NotificationController.GetLogInUser();

            DateTime now = DateTime.Now.Date;
            dtpFromDate.Value = new DateTime(now.Year, now.Month, 1);
            dtpToDate.Value = now;

            rbEarnedHrs.Checked = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void setLanguage()
        {
            try
            {
                // form text
                this.Text = rm.GetString("extraHrsReports", culture);

                // group box's text
                this.gbCriteria.Text = rm.GetString("gbCriteria", culture);
                this.gbReport.Text = rm.GetString("gbReportType", culture);
                this.gbFilter.Text = rm.GetString("gbFilter", culture);

                // label's text
                this.lblWU.Text = rm.GetString("lblWU", culture);
                this.lblEmpl.Text = rm.GetString("lblEmployee", culture);
                this.lblFromDate.Text = rm.GetString("lblFrom", culture);
                this.lblToDate.Text = rm.GetString("lblTo", culture);
                this.lblRepType.Text = rm.GetString("lblRepType", culture);
                this.lblType.Text = rm.GetString("lblType", culture);

                // button's text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnGenerate.Text = rm.GetString("btnGenerateReport", culture);

                // radio button's text
                this.rbEarnedHrs.Text = rm.GetString("rbEarnedHrs", culture);
                this.rbUsedHrs.Text = rm.GetString("rbUsedHrs", culture);
                this.rbAvaiableHrs.Text = rm.GetString("rbAvaiableHrs", culture);

                chbHierarchy.Text = rm.GetString("hierarchically", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHoursPreview.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void rbUsedHrs_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblType.Enabled = cbType.Enabled = rbUsedHrs.Checked;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHoursPreview.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void ExtraHoursPreview_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.CenterToScreen();
                
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
                populateTypeCombo();
                
                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " ExtraHoursPreview.RoutesReports_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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

                cbWU.DataSource = wuArray;
                cbWU.DisplayMember = "Name";
                cbWU.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHoursPreview.populateWorkingUnits(): " + ex.Message + "\n");
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
                    if (chbHierarchy.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWU.SelectedValue)
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

                cbEmpl.DataSource = emplArray;
                cbEmpl.DisplayMember = "LastName";
                cbEmpl.ValueMember = "EmployeeID";
                cbEmpl.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool check = false;
                foreach (WorkingUnitTO wu in wUnits)
                {
                    if (cbWU.SelectedIndex != 0)
                    {
                        if (wu.WorkingUnitID == (int)cbWU.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                        {
                            if (!chbHierarchy.Checked)
                            {
                                chbHierarchy.Checked = true;
                                check = true;
                            }
                        }
                    }
                }

                if (cbWU.SelectedValue is int && !check)
                {
                    populateEmployeeCombo((int)cbWU.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " ExtraHoursPreview.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (rbEarnedHrs.Checked)
                {
                    generateEarnedReport();
                }
                else if (rbUsedHrs.Checked)
                {
                    generateUsedReport();
                }
                else
                {
                    generateAvaiableReport();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " ExtraHoursPreview.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void generateAvaiableReport()
        {
            try
            {
                // Table Definition for Crystal Reports
                DataSet dataSetCR = new DataSet();
                DataTable tableCR = new DataTable("extra_hours");
                DataTable tableI = new DataTable("images");

                tableCR.Columns.Add("wu", typeof(System.String));
                tableCR.Columns.Add("employee", typeof(System.String));
                tableCR.Columns.Add("date", typeof(System.String));
                tableCR.Columns.Add("time", typeof(System.String));
                tableCR.Columns.Add("date_sort", typeof(System.DateTime));
                tableCR.Columns.Add("earnedMin", typeof(System.Int32));
                tableCR.Columns.Add("imageID", typeof(byte));

                tableI.Columns.Add("imageID", typeof(byte));
                tableI.Columns.Add("image", typeof(System.Byte[]));

                //add logo image just once
                DataRow rowI = tableI.NewRow();
                rowI["image"] = Constants.LogoForReport;
                rowI["imageID"] = 1;
                tableI.Rows.Add(rowI);
                tableI.AcceptChanges();

                dataSetCR.Tables.Add(tableCR);
                dataSetCR.Tables.Add(tableI);

                List<int> employees = new List<int>();
                if (cbEmpl.SelectedIndex > 0)
                {
                    int emplID = (int)cbEmpl.SelectedValue;
                    employees.Add(emplID);
                }
                else
                {
                    foreach (EmployeeTO empl in emplArray)
                    {
                        if (empl.EmployeeID > 0)
                            employees.Add(empl.EmployeeID);
                    }
                }

                foreach (int employeeID in employees)
                {
                    List<ExtraHourTO> availableList = new ExtraHour().SearchEmployeeAvailableDates(employeeID);
                    CultureInfo ci = CultureInfo.InvariantCulture;

                    List<ExtraHourUsedTO> usedAheadList = new ExtraHourUsed().SearchByType(employeeID, Constants.extraHoursUsedRegularAdvanced);


                    if (availableList.Count > 0)
                    {
                        foreach (ExtraHourTO extraHour in availableList)
                        {
                            DataRow row = tableCR.NewRow();

                            if (cbEmpl.SelectedIndex > 0)
                            {
                                row["wu"] = "";
                                row["employee"] = "";
                            }
                            else
                            {
                                EmployeeTO em = new EmployeeTO();
                                foreach (EmployeeTO e in emplArray)
                                {
                                    if (e.EmployeeID == employeeID)
                                    {
                                        em = e;
                                        break;
                                    }
                                }
                                row["wu"] = em.WorkingUnitName;
                                row["employee"] = em.LastName ;
                            }
                            row["date"] = extraHour.DateEarned.ToString("dd.MM.yyyy", ci);
                            string time = extraHour.CalculatedTimeAmt;
                            row["time"] = time;
                            row["earnedMin"] = Util.Misc.transformStringTimeToMin(time);
                            row["date_sort"] = extraHour.DateEarned;

                            row["imageID"] = 1;

                            tableCR.Rows.Add(row);
                            tableCR.AcceptChanges();
                        }
                    }
                    if (usedAheadList.Count > 0)
                    {
                        foreach (ExtraHourUsedTO extraHourUsed in usedAheadList)
                        {
                            DataRow row = tableCR.NewRow();

                            if (cbEmpl.SelectedIndex > 0)
                            {
                                row["wu"] = "";
                                row["employee"] = "";
                            }
                            else
                            {
                                EmployeeTO em = new EmployeeTO();
                                foreach (EmployeeTO e in emplArray)
                                {
                                    if (e.EmployeeID == employeeID)
                                    {
                                        em = e;
                                    }
                                }
                                row["wu"] = em.WorkingUnitName;
                                row["employee"] = em.LastName ;
                            }
                            row["date"] = extraHourUsed.DateEarned.ToString("dd.MM.yyyy", ci);
                            string time = extraHourUsed.CalculatedTimeAmtUsed;
                            row["time"] = time;
                            row["earnedMin"] = Util.Misc.transformStringTimeToMin(time);
                            row["date_sort"] = extraHourUsed.DateEarned;

                            row["imageID"] = 1;

                            tableCR.Rows.Add(row);
                            tableCR.AcceptChanges();                            
                        }
                    }                    
                }

                if (tableCR.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }

                string selWorkingUnit = "*";
                string selEmployee = "*";


                if (cbWU.SelectedIndex >= 0 && (int)cbWU.SelectedValue >= 0)
                    selWorkingUnit = cbWU.Text;
                if (cbEmpl.SelectedIndex >= 0 && (int)cbEmpl.SelectedValue >= 0)
                    selEmployee = cbEmpl.Text;

                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                {
                    Reports.Reports_sr.ExtraHoursCRView_sr view = new Reports.Reports_sr.ExtraHoursCRView_sr(dataSetCR,
                         selWorkingUnit, selEmployee, rm.GetString("usingTime", culture), rm.GetString("usingReportName", culture), new DateTime(), new DateTime());
                    view.ShowDialog(this);
                }
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                {
                    Reports.Reports_en.ExtraHoursCRView_en view = new Reports.Reports_en.ExtraHoursCRView_en(dataSetCR,
                          selWorkingUnit, selEmployee, rm.GetString("usingTime", culture), rm.GetString("usingReportName", culture), new DateTime(), new DateTime());
                    view.ShowDialog(this);
                }
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                {
                    Reports.Reports_fi.ExtraHoursCRView_fi view = new Reports.Reports_fi.ExtraHoursCRView_fi(dataSetCR,
                         selWorkingUnit, selEmployee, rm.GetString("usingTime", culture), rm.GetString("usingReportName", culture), new DateTime(), new DateTime());
                    view.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " ExtraHoursPreview.generateAvaiableReport(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void generateUsedReport()
        {
            try
            {
                // Table Definition for Crystal Reports
                DataSet dataSetCR = new DataSet();
                DataTable tableCR = new DataTable("extra_hours");
                DataTable tableI = new DataTable("images");

                tableCR.Columns.Add("wu", typeof(System.String));
                tableCR.Columns.Add("employee", typeof(System.String));
                tableCR.Columns.Add("usedDate", typeof(System.String));
                tableCR.Columns.Add("amount", typeof(System.String));
                tableCR.Columns.Add("startTime", typeof(System.String));
                tableCR.Columns.Add("endTime", typeof(System.String));
                tableCR.Columns.Add("earnedDate", typeof(System.String));
                tableCR.Columns.Add("type", typeof(System.String));
                tableCR.Columns.Add("createdBy", typeof(System.String));
                tableCR.Columns.Add("usedDateSort", typeof(System.DateTime));
                tableCR.Columns.Add("Min", typeof(System.Int16));
                tableCR.Columns.Add("imageID", typeof(byte));

                tableI.Columns.Add("imageID", typeof(byte));
                tableI.Columns.Add("image", typeof(System.Byte[]));

                //add logo image just once
                DataRow rowI = tableI.NewRow();
                rowI["image"] = Constants.LogoForReport;
                rowI["imageID"] = 1;
                tableI.Rows.Add(rowI);
                tableI.AcceptChanges();

                dataSetCR.Tables.Add(tableCR);
                dataSetCR.Tables.Add(tableI);


                string type = "";
                if (cbType.SelectedItem.ToString().Equals(rm.GetString("regular", culture)))
                {
                    type = Constants.extraHoursUsedRegular;
                }
                else if (cbType.SelectedItem.ToString().Equals(rm.GetString("overtime", culture)))
                {
                    type = Constants.extraHoursUsedOvertime;
                }
                else if (cbType.SelectedItem.ToString().Equals(rm.GetString("rejected", culture)))
                {
                    type = Constants.extraHoursUsedRejected;
                }
                else if (cbType.SelectedItem.ToString().Equals(rm.GetString("regularAdvanced", culture)))
                {
                    type = Constants.extraHoursUsedRegularAdvanced;
                }

                List<int> employees = new List<int>();

                if (cbEmpl.SelectedIndex > 0)
                {
                    int emplID = (int)cbEmpl.SelectedValue;
                    employees.Add(emplID);
                }
                else
                {
                    foreach (EmployeeTO empl in emplArray)
                    {
                        if (empl.EmployeeID > 0)
                            employees.Add(empl.EmployeeID);
                    }
                }

                foreach (int employeeID in employees)
                {
                    List<ExtraHourUsedTO> usedList = new ExtraHourUsed().Search(employeeID, dtpFromDate.Value, dtpToDate.Value, type);
                    CultureInfo ci = CultureInfo.InvariantCulture;

                    if (usedList.Count > 0)
                    {
                        foreach (ExtraHourUsedTO extraHourUsed in usedList)
                        {
                            DataRow row = tableCR.NewRow();
                            if (cbEmpl.SelectedIndex > 0)
                            {
                                row["wu"] = "";
                                row["employee"] = "";
                            }
                            else
                            {
                                EmployeeTO em = new EmployeeTO();
                                foreach (EmployeeTO e in emplArray)
                                {
                                    if (e.EmployeeID == employeeID)
                                    {
                                        em = e;
                                        break;
                                    }
                                }
                                row["wu"] = em.WorkingUnitName;
                                row["employee"] = em.LastName ;
                            }
                            string time = extraHourUsed.DateUsed.ToString("dd.MM.yyyy", ci);
                            row["usedDate"] = time;
                            row["usedDateSort"] = extraHourUsed.DateUsed;
                            row["amount"] = extraHourUsed.CalculatedTimeAmtUsed;
                            row["Min"] = Util.Misc.transformStringTimeToMin(extraHourUsed.CalculatedTimeAmtUsed);
                            row["startTime"] = extraHourUsed.StartTime.ToString("HH:mm", ci);
                            row["endTime"] = extraHourUsed.EndTime.ToString("HH:mm", ci);
                            row["earnedDate"] = extraHourUsed.DateEarned.ToString("dd.MM.yyyy", ci);

                            if (extraHourUsed.Type == Constants.extraHoursUsedRegular)
                                row["type"] = rm.GetString("regular", culture);
                            else if (extraHourUsed.Type == Constants.extraHoursUsedOvertime)
                                row["type"] = rm.GetString("overtime", culture);
                            else if (extraHourUsed.Type == Constants.extraHoursUsedRejected)
                                row["type"] = rm.GetString("rejected", culture);
                            else if (extraHourUsed.Type == Constants.extraHoursUsedRegularAdvanced)
                                row["type"] = rm.GetString("regularAdvanced", culture);
                            else
                                row["type"] = "";
                            row["createdBy"] = extraHourUsed.CreatedByName;
                            row["imageID"] = 1;

                            tableCR.Rows.Add(row);
                            tableCR.AcceptChanges();


                        }
                    }
                }
                if (tableCR.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }

                string selWorkingUnit = "*";
                string selEmployee = "*";
                string selType = "*";


                if (cbWU.SelectedIndex >= 0 && (int)cbWU.SelectedValue >= 0)
                    selWorkingUnit = cbWU.Text;
                if (cbEmpl.SelectedIndex >= 0 && (int)cbEmpl.SelectedValue >= 0)
                    selEmployee = cbEmpl.Text;
                if (cbType.SelectedIndex >= 0 && !cbType.SelectedValue.ToString().Equals("*"))
                    selType = cbType.Text;

                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                {
                    Reports.Reports_sr.ExtraHoursUsedCRView_sr view = new Reports.Reports_sr.ExtraHoursUsedCRView_sr(dataSetCR,
                         selWorkingUnit, selEmployee, selType, dtpFromDate.Value, dtpToDate.Value);
                    view.ShowDialog(this);
                }
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                {
                    Reports.Reports_en.ExtraHoursUsedCRView_en view = new Reports.Reports_en.ExtraHoursUsedCRView_en(dataSetCR,
                         selWorkingUnit, selEmployee, selType, dtpFromDate.Value, dtpToDate.Value);
                    view.ShowDialog(this);
                }
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                {
                    Reports.Reports_fi.ExtraHoursUsedCRView_fi view = new Reports.Reports_fi.ExtraHoursUsedCRView_fi(dataSetCR,
                         selWorkingUnit, selEmployee, selType, dtpFromDate.Value, dtpToDate.Value);
                    view.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " ExtraHoursPreview.generateUsedReport(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void generateEarnedReport()
        {
            try
            {
                 // Table Definition for Crystal Reports
                DataSet dataSetCR = new DataSet();
                DataTable tableCR = new DataTable("extra_hours");
                DataTable tableI = new DataTable("images");

                tableCR.Columns.Add("wu", typeof(System.String));
                tableCR.Columns.Add("employee", typeof(System.String));
                tableCR.Columns.Add("date", typeof(System.String));
                tableCR.Columns.Add("time", typeof(System.String));
                tableCR.Columns.Add("date_sort", typeof(System.DateTime));
                tableCR.Columns.Add("earnedMin", typeof(System.Int32));
                tableCR.Columns.Add("imageID", typeof(byte));

                tableI.Columns.Add("imageID", typeof(byte));
                tableI.Columns.Add("image", typeof(System.Byte[]));

                //add logo image just once
                DataRow rowI = tableI.NewRow();
                rowI["image"] = Constants.LogoForReport;
                rowI["imageID"] = 1;
                tableI.Rows.Add(rowI);
                tableI.AcceptChanges();

                dataSetCR.Tables.Add(tableCR);
                dataSetCR.Tables.Add(tableI);

                List<int> employees = new List<int>();
                if (cbEmpl.SelectedIndex > 0)
                {
                    int emplID = (int)cbEmpl.SelectedValue;
                    employees.Add(emplID);
                }
                else
                {
                    foreach (EmployeeTO empl in emplArray)
                    {
                        if (empl.EmployeeID > 0)
                            employees.Add(empl.EmployeeID);
                    }
                }

                foreach (int employeeID in employees)
                {
                    List<ExtraHourTO> earnedList = new ExtraHour().Search(employeeID, dtpFromDate.Value, dtpToDate.Value);
                    CultureInfo ci = CultureInfo.InvariantCulture;

                    if (earnedList.Count > 0)
                    {
                        foreach (ExtraHourTO extraHour in earnedList)
                        {
                            if (extraHour.ExtraTimeAmt > 0)
                            {

                                DataRow row = tableCR.NewRow();

                                if (cbEmpl.SelectedIndex > 0)
                                {
                                    row["wu"] = "";
                                    row["employee"] = "";
                                }
                                else
                                {
                                    EmployeeTO em = new EmployeeTO();
                                    foreach (EmployeeTO e in emplArray)
                                    {
                                        if (e.EmployeeID == employeeID)
                                        {
                                            em = e;
                                            break;
                                        }
                                    }
                                    row["wu"] = em.WorkingUnitName;
                                    row["employee"] = em.LastName;
                                }
                                row["date"] = extraHour.DateEarned.ToString("dd.MM.yyyy", ci);
                                string time = extraHour.CalculatedTimeAmt;
                                row["time"] = time;
                                row["earnedMin"] = Util.Misc.transformStringTimeToMin(time);
                                row["date_sort"] = extraHour.DateEarned;

                                row["imageID"] = 1;

                                tableCR.Rows.Add(row);
                                tableCR.AcceptChanges();                              
                               
                            }
                        }
                    }
                }
                if (tableCR.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }

                string selWorkingUnit = "*";
                string selEmployee = "*";


                if (cbWU.SelectedIndex >= 0 && (int)cbWU.SelectedValue >= 0)
                    selWorkingUnit = cbWU.Text;
                if (cbEmpl.SelectedIndex >= 0 && (int)cbEmpl.SelectedValue >= 0)
                    selEmployee = cbEmpl.Text;

                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                {
                    Reports.Reports_sr.ExtraHoursCRView_sr view = new Reports.Reports_sr.ExtraHoursCRView_sr(dataSetCR,
                         selWorkingUnit, selEmployee, rm.GetString("earnedTime", culture), rm.GetString("earnedReportName", culture), dtpFromDate.Value, dtpToDate.Value);
                    view.ShowDialog(this);
                }
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                {
                    Reports.Reports_en.ExtraHoursCRView_en view = new Reports.Reports_en.ExtraHoursCRView_en(dataSetCR,
                          selWorkingUnit, selEmployee, rm.GetString("earnedTime", culture), rm.GetString("earnedReportName", culture), dtpFromDate.Value, dtpToDate.Value);
                    view.ShowDialog(this);
                }
                else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                {
                    Reports.Reports_fi.ExtraHoursCRView_fi view = new Reports.Reports_fi.ExtraHoursCRView_fi(dataSetCR,
                         selWorkingUnit, selEmployee, rm.GetString("earnedTime", culture), rm.GetString("earnedReportName", culture), dtpFromDate.Value, dtpToDate.Value);
                    view.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " ExtraHoursPreview.generateUsedReport(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbAvaiableHrs_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                dtpFromDate.Enabled = dtpToDate.Enabled = !rbAvaiableHrs.Checked;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " ExtraHoursPreview.rbAvaiableHrs_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateTypeCombo()
        {
            try
            {
                ArrayList typeList = new ArrayList();
                typeList.Add(rm.GetString("all", culture));
                typeList.Add(rm.GetString("regular", culture));
                typeList.Add(rm.GetString("overtime", culture));
                typeList.Add(rm.GetString("rejected", culture));
                typeList.Add(rm.GetString("regularAdvanced", culture));

                cbType.DataSource = typeList;

                if (typeList.Count > 0)
                    cbType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHours.populateTypeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void chbHierarchy_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (cbWU.SelectedValue is int)
                {
                    if ((int)cbWU.SelectedValue >= 0)
                        populateEmployeeCombo((int)cbWU.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

    }
}