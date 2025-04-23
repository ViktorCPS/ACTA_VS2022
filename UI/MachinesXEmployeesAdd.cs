using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TransferObjects;
using System.Resources;
using System.Globalization;
using Util;
using Common;

namespace UI
{
    public partial class MachinesXEmployeesAdd : Form
    {

        ApplUserTO logInUser;
        private CultureInfo culture;
        ResourceManager rm;
        DebugLog log;

        MachineTO currentMachine = null;
        List<EmployeeXMachineTO> employeesForMachine = null;
        List<EmployeeTO> allEmployees = new List<EmployeeTO>();
        List<EmployeeXMachineTO> listForInsert = new List<EmployeeXMachineTO>();

        public MachinesXEmployeesAdd(MachineTO currentMachine)
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            logInUser = NotificationController.GetLogInUser();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(ApplUsersXRoles).Assembly);
            setLanguage();

            this.currentMachine = currentMachine;
            populateMachineCombo();
            cbMachines.Text = currentMachine.Name;
            tbRoleDesc.Text = currentMachine.Description;
            cbMachines.Enabled = false;
            employeesForMachine = new Machine().FindEmployeesForMachine(currentMachine.MachineID);
            populateEmployeesList();
            populateEmployeeListView(employeesForMachine, lvEmployees);
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
                btnSave.Text = rm.GetString("btnSave", culture);

                // label's text
                lblMachine.Text = rm.GetString("lblMachine", culture);
                lblRoleDesc.Text = rm.GetString("lblDescription", culture);
                lblEmployeesForMachine.Text = rm.GetString("lblEmployeesForMachine", culture);

                // list view
                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrEmplID", culture), (lvEmployees.Width - 6) / 3, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployees.Width - 6) / 3, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployees.Width - 6) / 3, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MachinesXEmployeesAdd.setLanguage(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MachinesXEmployeesAdd.populateMachineCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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
                log.writeLog(DateTime.Now + " MachinesXEmployeesAdd.populateEmployeeListView(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MachinesXEmployeesAdd.populateEmployeesList(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvEmployees.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvEmployees.SelectedItems)
                    {
                        EmployeeXMachineTO eXM = new EmployeeXMachineTO();
                        eXM.MachineTO = new MachineTO();
                        eXM.MachineTO.MachineID = currentMachine.MachineID;
                        eXM.EmployeeTO = new EmployeeTO();
                        eXM.EmployeeTO.EmployeeID = ((EmployeeTO)item.Tag).EmployeeID;
                        eXM.Status = "ACTIVE";
                        eXM.Date = DateTime.Now;

                        listForInsert.Add(eXM);
                    }

                    int result = new Machine().SaveEmployeesForMachine(listForInsert);
                    if (result > 0)
                    {
                        MessageBox.Show("Uspesno ste dodali nove radnike za izabranu masinu.");
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Radnici nisu dodati na masini.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Niste izabrali ni jednog zaposlenog.");
                    return;
                }
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " MachinesXEmployeesAdd.btnSave_Click(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
