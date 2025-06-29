using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Configuration;

using TransferObjects;
using Common;
using Util;
using Reports;

using System.Resources;
using System.Globalization;

namespace UI
{
    public partial class VacationAdd : Form
    {
        //properties for form type, define if it is update or add form
        private int formType;
        private const int AddForm = 0;
        private const int UpdateForm = 1;

        //language properties
        private CultureInfo culture;
        private ResourceManager rm;

        DebugLog log;
        ApplUserTO logInUser;

        //array of employees from selected wu
        List<EmployeeTO> currnetEmplArray;

        //wUnits for user
        private List<WorkingUnitTO> wUnits;
        private string wuString = "";

        //list of EmployeeVacEvidSchedule if MultiInterval
        List<EmployeeVacEvidScheduleTO> scheduleList;

        //for Update form Vacation to update
        EmployeeVacEvidTO emplVacEvid;

        public VacationAdd()
        {
            InitializeComponent();
            formType = AddForm;
            emplVacEvid = new EmployeeVacEvidTO();

        }

        public VacationAdd(EmployeeVacEvidTO emplVacation)
        {
            InitializeComponent();
            formType = UpdateForm;
            emplVacEvid = emplVacation;
            //can not update year and employee
            gbDefinitionType.Enabled = false;
            cbEmployee.Enabled = false;
            cbWU.Enabled = false;
            dtpYearFrom.Enabled = true;
            btnWUTree.Visible = false;
            chbHierarhicly.Enabled = false;
            label2.Visible = false;
            dtpYearFrom.Enabled = false;
            label3.Visible = false;

            //set values from object to form
            dtpYearFrom.Value = emplVacation.VacYear;
            nudDays.Value = emplVacation.NumOfDays;
            richTextBox1.Text = emplVacation.Note;
            emplVacEvid.VacationSchedules = new EmployeeVacEvid().SearchDetails(emplVacEvid.EmployeeID, emplVacEvid.VacYear);
           
        }

        private void VacationAdd_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.CenterToScreen();

                btnAddInterval.Visible = false;
                lvIntervals.Visible = false;
                lblDblClick.Visible = false;
                label1.Visible = false;

                scheduleList = new List<EmployeeVacEvidScheduleTO>();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(VacationEvidence).Assembly);
                setLanguage();

                logInUser = NotificationController.GetLogInUser();


                wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.VacationPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                populateWorkingUnitCombo();


                if (formType == AddForm)
                {
                    // Form name
                    this.Text = rm.GetString("vacationEvidenceAdd", culture);
                    dtpValidTo.Value = new DateTime(dtpYearFrom.Value.AddYears(1).Year, 6, 30);

                    this.dtpFrom.Value = new DateTime(dtpYearFrom.Value.Year, 1, 1);
                    this.dtpTo.Value = dtpValidTo.Value;

                    this.btnUpdate.Visible = false;
                    populateEmployeeCombo(-1);
                }
                else
                {
                    // Form name
                    this.Text = rm.GetString("vacationEvidenceUpdate", culture);

                    this.btnSave.Visible = false;

                    populateEmployeeCombo(emplVacEvid.WorkingUnitID);

                    //set values from object to form
                    cbWU.SelectedValue = emplVacEvid.WorkingUnitID;
                    cbEmployee.SelectedValue = emplVacEvid.EmployeeID;

                    dtpYearFrom.Value = emplVacEvid.VacYear;
                    dtpValidTo.Value = new DateTime(dtpYearFrom.Value.AddYears(1).Year, 6, 30);

                    nudDays.Value = emplVacEvid.NumOfDays;
                    richTextBox1.Text = emplVacEvid.Note;
                    if (emplVacEvid.VacationSchedules.Count <= 1)
                    {
                        foreach (EmployeeVacEvidScheduleTO schedule in emplVacEvid.VacationSchedules.Values)
                        {
                            dtpFrom.Value = schedule.StartDate;
                            dtpTo.Value = schedule.EndDate;
                        }
                        chbMultiInterval.Checked = false;
                    }
                    else
                    {
                        chbMultiInterval.Checked = true;
                        foreach (EmployeeVacEvidScheduleTO schedule in emplVacEvid.VacationSchedules.Values)
                        {

                            ListViewItem item = new ListViewItem();
                            item.Text = schedule.StartDate.ToString("dd.MM.yyyy");
                            item.SubItems.Add(schedule.EndDate.ToString("dd.MM.yyyy"));
                            lvIntervals.BeginUpdate();
                            lvIntervals.Items.Add(item);
                            lvIntervals.EndUpdate();
                            EmployeeVacEvidScheduleTO emplSchedule = new EmployeeVacEvidScheduleTO();

                            emplSchedule.StartDate = schedule.StartDate.Date;
                            emplSchedule.EndDate = schedule.EndDate.Date;
                            scheduleList.Add(emplSchedule);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.VacationAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }                 
        }

        /// <summary>
        /// Set proper language.
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // button's text
                btnCancel.Text = rm.GetString("btnCancel", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnSave.Text = rm.GetString("btnSave", culture);
                btnAddInterval.Text = rm.GetString("btnAddInterval", culture);


                // group box text
                this.gbData.Text = rm.GetString("gbData", culture);
                gbDefinitionType.Text = rm.GetString("gbDefinitionType", culture);
                gbTimeInterval.Text = rm.GetString("gbTimeInterval", culture);
                gbUsingPlan.Text = rm.GetString("gbUsingPlan", culture);

                // check box text
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);
                chbMultiInterval.Text = rm.GetString("chbMultiInterval", culture);

                // label's text
                lblWU.Text = rm.GetString("lblWU", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblNote.Text = rm.GetString("lblNote", culture);
                lblDblClick.Text = rm.GetString("lblDblClick", culture);
                lblYear.Text = rm.GetString("lblYear", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblDays.Text = rm.GetString("lblDaysVac", culture);
                lblValidTo.Text = rm.GetString("lblValidTo", culture);

                //radio button's text
                rbForEmpl.Text = rm.GetString("rbForEmpl", culture);
                rbForWU.Text = rm.GetString("rbForWU", culture);

                // list view initialization
                lvIntervals.BeginUpdate();
                lvIntervals.Columns.Add(rm.GetString("hdrFrom", culture), lvIntervals.Width / 2 - 10, HorizontalAlignment.Left);
                lvIntervals.Columns.Add(rm.GetString("hdrTo", culture), lvIntervals.Width / 2 - 10, HorizontalAlignment.Left);
                lvIntervals.EndUpdate();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Populate Working Unit Combo Box
        /// </summary>
        private void populateWorkingUnitCombo()
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

                cbWU.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationAdd.populateWorkingUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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
                    cbWU.SelectedIndex = cbWU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationAdd.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, EventArgs e)
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
                            if (!chbHierarhicly.Checked)
                            {
                                chbHierarhicly.Checked = true;
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
                log.writeLog(DateTime.Now + " VacationAdd.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                populateEmployeeCombo((int)cbWU.SelectedValue);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationAdd.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void populateEmployeeCombo(int wuID)
        {
            try
            {
                currnetEmplArray = new List<EmployeeTO>();
                string workUnitID = wuID.ToString();
                if (wuID == -1)
                {
                    currnetEmplArray = new Employee().SearchByWU(wuString);
                }
                else
                {
                    if (chbHierarhicly.Checked)
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

                    currnetEmplArray = new Employee().SearchByWU(workUnitID);
                }

                foreach (EmployeeTO employee in currnetEmplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                currnetEmplArray.Insert(0, empl);

                cbEmployee.DataSource = currnetEmplArray;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
                cbEmployee.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void rbForEmpl_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (rbForWU.Checked)
                {
                    cbEmployee.Enabled = false;
                    label2.Visible = false;
                    label1.Visible = true;
                }
                else
                {
                    cbEmployee.Enabled = true;
                    label1.Visible = false;
                    label2.Visible = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.rbForEmpl_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbMultiInterval_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (chbMultiInterval.Checked)
                {
                    btnAddInterval.Visible = true;
                    lvIntervals.Visible = true;
                    lblDblClick.Visible = true;
                }
                else
                {
                    btnAddInterval.Visible = false;
                    lvIntervals.Visible = false;
                    lblDblClick.Visible = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationAdd.chbMultiInterval_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAddInterval_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (dtpFrom.Value.Date >= dtpTo.Value.Date)
                {
                    MessageBox.Show(rm.GetString("msgToTimeLessThenFromTime", culture));
                    return;
                }
                foreach (EmployeeVacEvidScheduleTO vacation in scheduleList)
                {
                    if ((vacation.StartDate <= dtpTo.Value.Date && vacation.StartDate >= dtpFrom.Value.Date)
                        || (vacation.EndDate <= dtpTo.Value.Date && vacation.EndDate >= dtpFrom.Value.Date))
                    {
                        MessageBox.Show(rm.GetString("msgTimeOverloop", culture));
                        return;
                    }
                }
                ListViewItem item = new ListViewItem();
                item.Text = dtpFrom.Value.ToString("dd.MM.yyyy");
                item.SubItems.Add(dtpTo.Value.ToString("dd.MM.yyyy"));
                lvIntervals.BeginUpdate();
                lvIntervals.Items.Add(item);
                lvIntervals.EndUpdate();
                EmployeeVacEvidScheduleTO emplSchedule = new EmployeeVacEvidScheduleTO();

                emplSchedule.StartDate = dtpFrom.Value.Date;
                emplSchedule.EndDate = dtpTo.Value.Date;
                scheduleList.Add(emplSchedule);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationAdd.btnAddInterval_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvIntervals_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                scheduleList.RemoveAt(lvIntervals.SelectedItems[0].Index);
                lvIntervals.BeginUpdate();
                lvIntervals.Items.RemoveAt(lvIntervals.SelectedItems[0].Index);
                lvIntervals.EndUpdate();
                if (lvIntervals.Items.Count <= 0)
                {
                    this.dtpFrom.Value = new DateTime(DateTime.Now.Year, 1, 1);
                    this.dtpTo.Value = new DateTime(DateTime.Now.AddYears(1).Year, 6, 30);
                    chbMultiInterval.Checked = false;
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationAdd.lvIntervals_MouseDoubleClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                bool valid = Validation();
                if (valid)
                {
                    bool someExist = false;
                    bool inserted = true;
                    if (rbForEmpl.Checked)
                    {
                        EmployeeVacEvidTO employeeVacation = new EmployeeVacEvidTO();
                        employeeVacation.EmployeeID = (int)cbEmployee.SelectedValue;
                        employeeVacation.VacYear = new DateTime(dtpYearFrom.Value.Year, 1, 1);
                        employeeVacation.NumOfDays = (int)nudDays.Value;
                        employeeVacation.Note = richTextBox1.Text.ToString();
                        employeeVacation.ValidTo = dtpValidTo.Value.Date;
                        Dictionary<int, EmployeeVacEvidScheduleTO> schedules = new Dictionary<int,EmployeeVacEvidScheduleTO>();
                        if (chbMultiInterval.Checked)
                        {
                            int i = 0;
                            foreach (EmployeeVacEvidScheduleTO schedule in scheduleList)
                            {
                                i++;
                                schedule.EmployeeID = employeeVacation.EmployeeID;
                                schedule.VacYear = employeeVacation.VacYear;
                                schedule.Segment = i;
                                schedules.Add(i, schedule);
                            }
                        }
                        else
                        {
                            EmployeeVacEvidScheduleTO schedule = new EmployeeVacEvidScheduleTO();
                            schedule.EmployeeID = employeeVacation.EmployeeID;
                            schedule.VacYear = employeeVacation.VacYear;
                            schedule.Segment = 1;
                            schedule.StartDate = dtpFrom.Value.Date;
                            schedule.EndDate = dtpTo.Value.Date;
                            schedules.Add(1, schedule);
                        }
                        employeeVacation.VacationSchedules = schedules;

                        EmployeeVacEvid evid = new EmployeeVacEvid();
                        evid.EmplVacEvidTO = employeeVacation;
                        inserted = (evid.Save() > 0);
                    }
                    else
                    {
                        if (currnetEmplArray.Count <= 0)
                        {
                            MessageBox.Show(rm.GetString("noEmplInWU", culture));
                            return;
                        }
                        foreach (EmployeeTO empl in currnetEmplArray)
                        {
                            if (empl.EmployeeID != -1)
                            {
                                int vacations = new EmployeeVacEvid().getVacationsCount(empl.EmployeeID.ToString(), dtpYearFrom.Value.Date, dtpYearFrom.Value.Date, -1, -1);
                                if (vacations > 0)
                                {
                                    someExist = true;
                                }
                                else
                                {
                                    EmployeeVacEvidTO employeeVacation = new EmployeeVacEvidTO();
                                    employeeVacation.EmployeeID = empl.EmployeeID;
                                    employeeVacation.VacYear = new DateTime(dtpYearFrom.Value.Year, 1, 1);
                                    employeeVacation.NumOfDays = (int)nudDays.Value;
                                    employeeVacation.Note = richTextBox1.Text.ToString();
                                    employeeVacation.ValidTo = dtpValidTo.Value.Date;
                                    Dictionary<int, EmployeeVacEvidScheduleTO> schedules = new Dictionary<int,EmployeeVacEvidScheduleTO>();
                                    if (chbMultiInterval.Checked)
                                    {
                                        int i = 0;
                                        foreach (EmployeeVacEvidScheduleTO schedule in scheduleList)
                                        {
                                            i++;
                                            schedule.EmployeeID = employeeVacation.EmployeeID;
                                            schedule.VacYear = employeeVacation.VacYear;
                                            schedule.Segment = i;
                                            schedules.Add(i, schedule);
                                        }
                                    }
                                    else
                                    {
                                        EmployeeVacEvidScheduleTO schedule = new EmployeeVacEvidScheduleTO();
                                        schedule.EmployeeID = employeeVacation.EmployeeID;
                                        schedule.VacYear = employeeVacation.VacYear;
                                        schedule.Segment = 1;
                                        schedule.StartDate = dtpFrom.Value.Date;
                                        schedule.EndDate = dtpTo.Value.Date;
                                        schedules.Add(1, schedule);
                                    }
                                    employeeVacation.VacationSchedules = schedules;

                                    EmployeeVacEvid evid = new EmployeeVacEvid();
                                    evid.EmplVacEvidTO = employeeVacation;
                                    inserted = inserted && (evid.Save() > 0);
                                }
                            }
                        }
                    }

                    if (someExist)
                    {
                        MessageBox.Show(rm.GetString("someVacationsExist", culture));
                    }
                    if (inserted)
                    {
                        DialogResult result = MessageBox.Show(rm.GetString("vacationsInserted", culture), "VacationsAdd", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            clearControls(this.Controls);
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("vacationsNotInserted", culture));
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationAdd.btnSave_Click(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private bool Validation()
        {
            bool valid = true;
            try
            {
                if (rbForEmpl.Checked && cbEmployee.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("selEmployee", culture));
                    return false;
                }
                if (rbForEmpl.Checked&&formType==AddForm)
                {
                    int vacations = new EmployeeVacEvid().getVacationsCount(cbEmployee.SelectedValue.ToString(), dtpYearFrom.Value.Date, dtpYearFrom.Value.Date, -1, -1);
                    if (vacations > 0)
                    {
                        MessageBox.Show(rm.GetString("employeeHasVac", culture));
                        return false;
                    }
                }
                if (!chbMultiInterval.Checked)
                {
                    if (dtpFrom.Value.Date >= dtpTo.Value.Date)
                    {
                        MessageBox.Show(rm.GetString("msgToTimeLessThenFrom", culture));
                        return false;
                    }
                }
                if (rbForWU.Checked && cbWU.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("selWU", culture));
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationAdd.Validation(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return valid;
        }

        private void dtpYearFrom_ValueChanged(object sender, EventArgs e)
        {
            if (formType== AddForm && dtpYearFrom.Value.Year < DateTime.Now.AddYears(-1).Year)
                dtpYearFrom.Value = DateTime.Now;
        }
        public void clearControls(Control.ControlCollection controls)
        {
            try
            {
                foreach (Control c in controls)
                {
                    if (c is TextBox)
                    {
                        TextBox tb = (TextBox)c;
                        tb.Text = "";
                    }
                    if (c is RichTextBox)
                    {
                        RichTextBox rtb = (RichTextBox)c;
                        rtb.Text = "";
                    }
                    if (c is ComboBox)
                    {
                        ComboBox cb = (ComboBox)c;
                        cb.SelectedIndex = 0;
                    }
                    if (c is DateTimePicker)
                    {
                        DateTimePicker dtp = (DateTimePicker)c;
                        dtp.Value = DateTime.Now;
                    }
                }
                btnAddInterval.Visible = false;
                lvIntervals.Visible = false;
                lblDblClick.Visible = false;

                lvIntervals.BeginUpdate();
                lvIntervals.Items.Clear();
                lvIntervals.EndUpdate();

                scheduleList = new List<EmployeeVacEvidScheduleTO>();

                this.dtpFrom.Value = new DateTime(DateTime.Now.Year, 1, 1);
                this.dtpTo.Value = new DateTime(DateTime.Now.AddYears(1).Year, 6, 30);
                chbMultiInterval.Checked = false;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool valid = Validation();
                if (valid)
                {
                    emplVacEvid.EmployeeID = (int)cbEmployee.SelectedValue;
                    emplVacEvid.VacYear = new DateTime(dtpYearFrom.Value.Year, 1, 1);
                    emplVacEvid.NumOfDays = (int)nudDays.Value;
                    emplVacEvid.Note = richTextBox1.Text.ToString();
                    Dictionary<int, EmployeeVacEvidScheduleTO> schedules = new Dictionary<int, EmployeeVacEvidScheduleTO>();
                    if (chbMultiInterval.Checked)
                    {
                        int i = 0;
                        foreach (EmployeeVacEvidScheduleTO schedule in scheduleList)
                        {
                            i++;
                            schedule.EmployeeID = emplVacEvid.EmployeeID;
                            schedule.VacYear = emplVacEvid.VacYear;
                            schedule.Segment = i;
                            schedules.Add(i, schedule);
                        }
                    }
                    else
                    {
                        EmployeeVacEvidScheduleTO schedule = new EmployeeVacEvidScheduleTO();
                        schedule.EmployeeID = emplVacEvid.EmployeeID;
                        schedule.VacYear = emplVacEvid.VacYear;
                        schedule.Segment = 1;
                        schedule.StartDate = dtpFrom.Value.Date;
                        schedule.EndDate = dtpTo.Value.Date;
                        schedules.Add(1, schedule);
                    }
                    emplVacEvid.VacationSchedules = schedules;

                    bool updated = false;

                    EmployeeVacEvid evid = new EmployeeVacEvid();
                    evid.EmplVacEvidTO = emplVacEvid;
                    bool trans = evid.BeginTransaction();
                    int saved = 0;
                    if (trans)
                    {
                        try
                        {
                            bool deleted = evid.Remove(emplVacEvid.EmployeeID, emplVacEvid.VacYear, false);
                            updated = updated && deleted;
                            if (deleted)
                            {
                                saved = evid.Save(false);
                            }
                            if (saved > 0)
                            {
                                evid.CommitTransaction();

                                MessageBox.Show(rm.GetString("vacationUpdated", culture));
                                this.Close();
                            }
                            else
                            {
                                evid.RollbackTransaction();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (evid.GetTransaction() != null)
                                evid.RollbackTransaction();

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
                log.writeLog(DateTime.Now + " VacationAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dtpValidTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //vacation evidence is valid to the end of the year or some date in next year
                if (dtpValidTo.Value.Year != dtpYearFrom.Value.Year&&dtpValidTo.Value.Year != dtpYearFrom.Value.AddYears(1).Year)
                {
                    dtpValidTo.Value = new DateTime(dtpYearFrom.Value.AddYears(1).Year, 6, 30);
                }
                if (dtpValidTo.Value.Year == dtpYearFrom.Value.Year)
                {
                    dtpValidTo.Value = new DateTime(dtpValidTo.Value.Year, 12, 31);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationAdd.dtpValidTo_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtpFrom.Value.Date > dtpValidTo.Value)
                    dtpFrom.Value = dtpValidTo.Value;
                if (dtpFrom.Value.Year != dtpYearFrom.Value.Year)
                    dtpFrom.Value = new DateTime(dtpYearFrom.Value.Year, 1, 1);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationAdd.dtpFrom_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtpTo.Value.Date > dtpValidTo.Value)
                    dtpTo.Value = dtpValidTo.Value;
                if (dtpTo.Value.Year != dtpYearFrom.Value.Year)
                    dtpTo.Value = new DateTime(dtpYearFrom.Value.Year, 12, 31);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationAdd.dtpTo_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void VacationAdd_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + "  VacationAdd.VacationAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}