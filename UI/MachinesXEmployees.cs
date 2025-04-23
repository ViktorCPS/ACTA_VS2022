using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TransferObjects;
using System.Globalization;
using System.Resources;
using Util;
using Common;
using System.Collections;

namespace UI
{
    public partial class MachinesXEmployees : Form
    {

        ApplUserTO logInUser;
        private CultureInfo culture;
        ResourceManager rm;
        DebugLog log;

        MachineTO currentMachine = null;
        List<EmployeeTO> allEmployees = new List<EmployeeTO>();
        List<EmployeeXMachineTO> employeesForMachine = null;
        private ArrayList addedEmployees;
        private ArrayList removedEmployees;
        ArrayList originalSelectedEmployeeList;
        int machineID = -1;

        public MachinesXEmployees()
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            logInUser = NotificationController.GetLogInUser();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(ApplUsersXRoles).Assembly);
            setLanguage();
        }

        public MachinesXEmployees(MachineTO currentMachine)
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            logInUser = NotificationController.GetLogInUser();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(ApplUsersXRoles).Assembly);
            setLanguage();

            this.currentMachine = currentMachine;
            this.machineID = currentMachine.MachineID;
            cbMachines.Text = currentMachine.Name;
            cbMachines.Enabled = false;
        }

        /// <summary>
        /// Set proper language.
        /// </summary>
        public void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("formMachineXEmployee", culture);

                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
                btnReport.Text = rm.GetString("btnReport", culture);

                // label's text
                lblMachine.Text = rm.GetString("lblMachine", culture);
                lblRoleDesc.Text = rm.GetString("lblDescription", culture);
                lblSelEmployees.Text = rm.GetString("lblSelEmployees", culture);

                // list view

                lvSelectedEmployees.BeginUpdate();
                lvSelectedEmployees.Columns.Add(rm.GetString("hdrEmplID", culture), (lvSelectedEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvSelectedEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), (lvSelectedEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvSelectedEmployees.Columns.Add(rm.GetString("hdrLastName", culture), (lvSelectedEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvSelectedEmployees.Columns.Add(rm.GetString("hdrDate", culture), (lvSelectedEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvSelectedEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MachinesXEmployees.setLanguage(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void MachinesXEmployees_Load(object sender, EventArgs e)
        {
            removedEmployees = new ArrayList();
            addedEmployees = new ArrayList();
            originalSelectedEmployeeList = new ArrayList();

            populateMachineCombo();
            populateEmployeesList();

            if (currentMachine != null)
            {
                employeesForMachine = new Machine().FindEmployeesForMachine(machineID);
                cbMachines.Text = currentMachine.Name;
                tbRoleDesc.Text = currentMachine.Description;

                populateData();

                foreach (EmployeeXMachineTO exm in employeesForMachine)
                {
                    originalSelectedEmployeeList.Add(exm);
                }
            }
        }

        private void populateEmployeeListView(List<EmployeeXMachineTO> employeesList, ListView lvName)
        {
            try
            {
                lvName.BeginUpdate();
                lvName.Items.Clear();

                foreach (EmployeeXMachineTO empl in employeesList)
                {
                    for (int i = 0; i < allEmployees.Count; i++)
                    {
                        if (empl.EmployeeTO.EmployeeID == allEmployees[i].EmployeeID)
                        {
                            allEmployees.Remove(allEmployees[i]);
                        }
                    }
                }

                if (allEmployees.Count > 0)
                {
                    foreach (EmployeeTO em in allEmployees)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = em.EmployeeID.ToString().Trim();
                        item.SubItems.Add(em.FirstName.ToString().Trim());
                        item.SubItems.Add(em.LastName.ToString().Trim());
                        item.Tag = em;
                        lvName.Items.Add(item);
                    }
                }

                lvName.EndUpdate();
                lvName.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MachinesXEmployees.populateEmployeeListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateSelectedEmployeesListView(List<EmployeeXMachineTO> employeesList, ListView lvName)
        {
            try
            {
                lvName.BeginUpdate();
                lvName.Items.Clear();

                if (employeesList.Count > 0)
                {
                    foreach (EmployeeXMachineTO employee in employeesList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = employee.EmployeeTO.EmployeeID.ToString().Trim();
                        item.SubItems.Add(employee.EmployeeTO.FirstName.Trim());
                        item.SubItems.Add(employee.EmployeeTO.LastName.Trim());
                        item.SubItems.Add(employee.Date.ToString("dd.MM.yyyy"));
                        item.Tag = employee;
                        lvName.Items.Add(item);
                    }
                }

                lvName.EndUpdate();
                lvName.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MachinesXEmployees.populateSelectedEmployeesListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateMachineCombo()
        {
            try
            {

                List<MachineTO> machines = new Machine().Search();
                machines.Insert(0, new MachineTO(rm.GetString("all", culture)));

                this.cbMachines.DataSource = machines;
                this.cbMachines.DisplayMember = "Name";
                this.cbMachines.ValueMember = "MachineID";

                if (currentMachine != null)
                {
                    this.cbMachines.Text = currentMachine.Name;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MachinesXEmployees.populateMachineCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbMachines_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (currentMachine == null)
                {
                    currentMachine = cbMachines.SelectedItem as MachineTO;
                }

                tbRoleDesc.Text = currentMachine.Description;
                employeesForMachine = new Machine().FindEmployeesForMachine(machineID);

                populateData();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MachinesXEmployees.cbMachines_SelectedIndexChanged(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateEmployeesList()
        {
            try
            {
                allEmployees = new Employee().Search();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MachinesXEmployees.populateEmployeesList(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateData()
        {
            try
            {
                populateEmployeesList();
                populateSelectedEmployeesListView(employeesForMachine, lvSelectedEmployees);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MachinesXEmployees.populateData(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!cbMachines.Text.Equals("*"))
            {
                currentMachine = cbMachines.SelectedItem as MachineTO;
                MachinesXEmployeesAdd form = new MachinesXEmployeesAdd(currentMachine);
                form.ShowDialog();

                employeesForMachine = new Machine().FindEmployeesForMachine(currentMachine.MachineID);
                populateData();
            }
            else
            {
                MessageBox.Show("Morate izabrati mašinu.");
                return;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<EmployeeXMachineTO> listForUpdate = new List<EmployeeXMachineTO>();

                if (lvSelectedEmployees.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noSelEmployeeDel", culture));
                }
                else
                {
                    DialogResult result = MessageBox.Show(rm.GetString("deleteEmployeesFromMachine", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        bool isDeleted = true;

                        foreach (ListViewItem item in lvSelectedEmployees.SelectedItems)
                        {
                            EmployeeXMachineTO exmTO = item.Tag as EmployeeXMachineTO;
                            listForUpdate.Add(exmTO);
                        }

                        isDeleted = new Machine().UpdateEmployeesForMachine(listForUpdate) && isDeleted;

                        if (isDeleted)
                        {
                            MessageBox.Show(rm.GetString("employeesDelFromMachine", culture));
                        }
                        else if (!isDeleted)
                        {
                            MessageBox.Show(rm.GetString("noEmployeeDelFromMachine", culture));
                        }

                        employeesForMachine = new Machine().FindEmployeesForMachine(currentMachine.MachineID);
                        populateData();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MachinesXEmployees.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lvSelectedEmployees.SelectedItems.Count == 1)
            {
                EmployeeTO empl = allEmployees.Find(emm => ((EmployeeXMachineTO)lvSelectedEmployees.SelectedItems[0].Tag).EmployeeTO.EmployeeID == emm.EmployeeID);
                MachinesXEmployeesEdit form = new MachinesXEmployeesEdit(empl, (EmployeeXMachineTO)lvSelectedEmployees.SelectedItems[0].Tag);
                form.ShowDialog();

                employeesForMachine = new Machine().FindEmployeesForMachine(currentMachine.MachineID);
                populateData();
            }
            else
            {
                MessageBox.Show(rm.GetString("noSelEmployeeDel", culture));
                return;
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvSelectedEmployees.Items.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("machines_x_employees");
                    DataTable tableI = new DataTable("images");

                    tableCR.Columns.Add("employee_id", typeof(System.String));
                    tableCR.Columns.Add("first_name", typeof(System.String));
                    tableCR.Columns.Add("last_name", typeof(System.String));
                    tableCR.Columns.Add("date", typeof(System.String));
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

                    foreach (EmployeeXMachineTO exmTO in employeesForMachine)
                    {
                        DataRow row = tableCR.NewRow();

                        row["employee_id"] = exmTO.EmployeeTO.EmployeeID.ToString();
                        row["first_name"] =  exmTO.EmployeeTO.FirstName;
                        row["last_name"] = exmTO.EmployeeTO.LastName;
                        row["date"] = exmTO.Date.ToString("dd.MM.yyyy");
                        row["imageID"] = 1;

                        tableCR.Rows.Add(row);
                        tableCR.AcceptChanges();
                    }

                    if (tableCR.Rows.Count == 0)
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }

                    string selMachine = "*";

                    if (cbMachines.SelectedIndex >= 0 && (int)cbMachines.SelectedValue >= 0)
                        selMachine = cbMachines.Text;

                    if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                    {
                        Reports.Reports_sr.MachinesXEmployeesCRView view = new Reports.Reports_sr.MachinesXEmployeesCRView(dataSetCR, selMachine);
                        view.ShowDialog(this);
                    }
                    //else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                    //{
                    //    Reports.Reports_en.EmployeePassesCRView_en view = new Reports.Reports_en.EmployeePassesCRView_en(dataSetCR,
                    //        dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selGate, selDirection, selLocation,
                    //        fromTime, toTime, firstIn, lastOut, advanced);
                    //    view.ShowDialog(this);
                    //}
                    //else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                    //{
                    //    Reports.Reports_fi.EmployeePassesCRView_fi view = new Reports.Reports_fi.EmployeePassesCRView_fi(dataSetCR,
                    //        dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selGate, selDirection, selLocation,
                    //        fromTime, toTime, firstIn, lastOut, advanced);
                    //    view.ShowDialog(this);
                    //}
                }
                else
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MachinesXEmployees.btnReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
