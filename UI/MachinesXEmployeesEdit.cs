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

namespace UI
{
    public partial class MachinesXEmployeesEdit : Form
    {

        ApplUserTO logInUser;
        private CultureInfo culture;
        ResourceManager rm;
        DebugLog log;

        EmployeeTO currentEmployee = null;
        EmployeeXMachineTO currentEXM = null;
        List<EmployeeTO> allEmployees = new List<EmployeeTO>();

        public MachinesXEmployeesEdit(EmployeeTO currentEmployee, EmployeeXMachineTO currentEXM)
        {
            InitializeComponent();
            this.currentEmployee = currentEmployee;
            this.currentEXM = currentEXM;

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
            logInUser = NotificationController.GetLogInUser();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(ApplUsersXRoles).Assembly);
            setLanguage();
            populateEmployeesList();
            populateMachineCombo();

            cbEmplName.Text = currentEmployee.LastName + " " + currentEmployee.FirstName;
            cbEmplName.Enabled = false;
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
                btnUpdate.Text = rm.GetString("btnUpdate", culture);

                // label's text
                lblMachine.Text = rm.GetString("lblMachine", culture);
                lblName.Text = rm.GetString("lblEmployee", culture);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MachinesXEmployeesEdit.setLanguage(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateMachineCombo()
        {
            try
            {
                List<MachineTO> machines = new Machine().Search();
                machines.RemoveAll(m => m.MachineID == currentEXM.MachineTO.MachineID);
                machines.Insert(0, new MachineTO(rm.GetString("all", culture)));

                this.cbMachines.DataSource = machines;
                this.cbMachines.DisplayMember = "Name";
                this.cbMachines.ValueMember = "MachineID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MachinesXEmployeesEdit.populateMachineCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateEmployeesList()
        {
            try
            {
                allEmployees = new Employee().Search();

                cbEmplName.DataSource = allEmployees;
                cbEmplName.DisplayMember = "FirstAndLastName";
                cbEmplName.ValueMember = "EmployeeID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MachinesXEmployeesEdit.populateEmployeesList(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!cbMachines.Text.Equals("*"))
            {
                EmployeeXMachineTO exmTO = new EmployeeXMachineTO();
                exmTO.EmployeeTO = currentEmployee;
                exmTO.MachineTO = cbMachines.SelectedItem as MachineTO;
                exmTO.Date = DateTime.Now;
                exmTO.Status = "ACTIVE";

                List<EmployeeXMachineTO> listForSave = new List<EmployeeXMachineTO>();
                List<EmployeeXMachineTO> listForUpdate = new List<EmployeeXMachineTO>();
                listForSave.Add(exmTO);
                currentEXM.Status = "FINISHED";
                listForUpdate.Add(currentEXM);

                int result = new Machine().SaveEmployeesForMachine(listForSave);
                if (result == 1)
                {
                    new Machine().UpdateEmployeesForMachine(listForUpdate);
                    MessageBox.Show("Uspesno ste dodelili radniku drugu masinu.");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show(rm.GetString("selOneMachineMust", culture));
                return;
            }
        }
    }
}
