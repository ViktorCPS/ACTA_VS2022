using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using Util;
using TransferObjects;
using Common;

namespace UI
{
    public partial class MachinesAdd : Form
    {

        MachineTO currentMachine = null;
        ResourceManager rm;

        private CultureInfo culture;
        ApplUserTO logInUser;
        DebugLog log;

        public MachinesAdd()
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            currentMachine = new MachineTO();
            logInUser = NotificationController.GetLogInUser();

            this.CenterToScreen();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

            rm = new ResourceManager("UI.Resource", typeof(GatesAdd).Assembly);
            setLanguage();

            btnUpdate.Visible = false;
            lblMachineID.Visible = false;
            tbMachineID.Visible = false;
        }

        public MachinesAdd(MachineTO machine)
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            currentMachine = new MachineTO(machine.MachineID, machine.Name, machine.Description);
            logInUser = NotificationController.GetLogInUser();

            this.CenterToScreen();

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

            rm = new ResourceManager("UI.Resource", typeof(GatesAdd).Assembly);

            setLanguage();

            tbMachineID.Text = machine.MachineID.ToString().Trim();
            tbName.Text = machine.Name.Trim();
            tbDesc.Text = machine.Description.Trim();
            btnSave.Visible = false;
            tbMachineID.Enabled = false;
        }

        private void MachinesAdd_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Set proper language
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // Form name
                if (currentMachine.MachineID >= 0)
                {
                    this.Text = rm.GetString("updateMachine", culture);
                }
                else
                {
                    this.Text = rm.GetString("addMachine", culture);
                }

                // button's text
                btnSave.Text = rm.GetString("btnSave", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);

                // label's text
                lblMachineID.Text = rm.GetString("lblMachineID", culture);
                lblName.Text = rm.GetString("lblName", culture);
                lblDesc.Text = rm.GetString("lblDescription", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MachinesAdd.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!currentMachine.Name.Trim().Equals(tbName.Text.Trim()))
                {
                    Machine m = new Machine();
                    m.MTO.Name = tbName.Text.Trim();
                    List<MachineTO> machines = m.Search();

                    if (machines.Count > 0)
                    {
                        MessageBox.Show(rm.GetString("machineNameExists", culture));
                        return;
                    }
                }

                bool isUpdated = false;

                if (isUpdated = new Machine().Update(Int32.Parse(this.tbMachineID.Text.Trim()), this.tbName.Text.Trim(),
                    this.tbDesc.Text.Trim()))
                {
                    currentMachine.MachineID = Int32.Parse(this.tbMachineID.Text.Trim());
                    currentMachine.Name = this.tbName.Text.Trim();
                    currentMachine.Description = this.tbDesc.Text.Trim();

                    MessageBox.Show(rm.GetString("machineUpdated", culture));
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MachinesAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                Machine m = new Machine();
                m.MTO.Name = tbName.Text.Trim();
                List<MachineTO> machines = m.Search();

                if (machines.Count == 0)
                {
                    currentMachine.Name = this.tbName.Text.Trim();
                    currentMachine.Description = this.tbDesc.Text.Trim();

                    int inserted = new Machine().Save(currentMachine.Name, currentMachine.Description);
                    if (inserted > 0)
                    {
                        MessageBox.Show(rm.GetString("machineSaved", culture));
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("machineNameExists", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MachinesAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
