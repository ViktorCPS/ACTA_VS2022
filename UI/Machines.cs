using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using Util;
using TransferObjects;
using Common;
using System.Collections;

namespace UI
{
    public partial class Machines : Form
    {

        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        ApplUserTO logInUser;

        MachineTO currentMachine = null;

        public Machines()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);
                setLanguage();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message); ;
            }
        }

        /// <summary>
        /// Set proper language and initialize List View
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("machineForm", culture);

                // group box text
                gbMachines.Text = rm.GetString("gbMachines", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);

                // button's text
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                btnSaveCriteria.Text = rm.GetString("SaveFilter", culture);
                btnMachineXEmployee.Text = rm.GetString("btnMachineXEmployee", culture);

                // label's text
                lblName.Text = rm.GetString("lblName", culture);
                lblDesc.Text = rm.GetString("lblDescription", culture);

                // list view
                lvMachines.BeginUpdate();
                lvMachines.Columns.Add(rm.GetString("hdrID", culture), (lvMachines.Width - 3) / 3, HorizontalAlignment.Left);
                lvMachines.Columns.Add(rm.GetString("hdrName", culture), (lvMachines.Width - 3) / 3, HorizontalAlignment.Left);
                lvMachines.Columns.Add(rm.GetString("hdrDescription", culture), (lvMachines.Width - 3) / 3, HorizontalAlignment.Left);
                lvMachines.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Machines.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void Machines_Load(object sender, EventArgs e)
        {
            try
            {
                List<MachineTO> machineList = new Machine().Search();
                populateListView(machineList);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Populate List View with Gates found
        /// </summary>
        /// <param name="gatesList"></param>
        public void populateListView(List<MachineTO> machinesList)
        {
            try
            {
                lvMachines.BeginUpdate();
                lvMachines.Items.Clear();

                if (machinesList.Count > 0)
                {
                    foreach (MachineTO machine in machinesList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = machine.MachineID.ToString().Trim();
                        item.SubItems.Add(machine.Name.ToString().Trim());
                        item.SubItems.Add(machine.Description.ToString().Trim());
                        item.Tag = machine;

                        lvMachines.Items.Add(item);
                    }
                }

                lvMachines.EndUpdate();
                lvMachines.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Machines.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                Machine machine = new Machine();
                machine.MTO.Name = tbName.Text.Trim();
                machine.MTO.Description = tbDesc.Text.Trim();
                List<MachineTO> machineList = machine.Search();

                if (machineList.Count > 0)
                {
                    populateListView(machineList);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noMachinesFound", culture));
                }

                currentMachine = new MachineTO();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Machines.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Open Add Form
                MachinesAdd addForm = new MachinesAdd();
                addForm.ShowDialog(this);

                List<MachineTO> machineList = new Machine().Search();
                populateListView(machineList);
                tbName.Text = "";
                tbDesc.Text = "";
                currentMachine = new MachineTO();
                this.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Machines.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (this.lvMachines.SelectedItems.Count != 1)
                {
                    MessageBox.Show(rm.GetString("selOneMachine", culture));
                }
                else
                {
                    currentMachine = (MachineTO)this.lvMachines.SelectedItems[0].Tag;

                    // Open Update Form
                    MachinesAdd addForm = new MachinesAdd(currentMachine);
                    addForm.ShowDialog(this);

                    List<MachineTO> machineList = new Machine().Search();
                    populateListView(machineList);
                    tbName.Text = "";
                    tbDesc.Text = "";
                    currentMachine = new MachineTO();
                    this.Invalidate();
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Machines.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvMachines.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noSelMachineDel", culture));
                }
                else
                {
                    DialogResult result = MessageBox.Show(rm.GetString("deleteMachines", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        bool isDeleted = true;
                        int selected = lvMachines.SelectedItems.Count;
                        foreach (ListViewItem item in lvMachines.SelectedItems)
                        {
                            if (((MachineTO)item.Tag).MachineID == 0)
                            {
                                MessageBox.Show(rm.GetString("defaultMachine", culture));
                                selected--;
                            }
                            else
                            {
                                // Find if exists Gate <-> Access group xref record
                                List<EmployeeXMachineTO> employeesForMachine = new Machine().FindEmployeesForMachine(((MachineTO)item.Tag).MachineID);
                                if (employeesForMachine.Count > 0)
                                {
                                    MessageBox.Show(item.Text + ": " + rm.GetString("machineHasEmployee", culture));
                                    selected--;
                                }
                                else
                                {
                                    isDeleted = new Machine().Delete(((MachineTO)item.Tag).MachineID) && isDeleted;
                                }
                            }
                        }

                        if ((selected > 0) && isDeleted)
                        {
                            MessageBox.Show(rm.GetString("machinesDel", culture));
                        }
                        else if (!isDeleted)
                        {
                            MessageBox.Show(rm.GetString("noMachineDel", culture));
                        }

                        List<MachineTO> machineList = new Machine().Search();
                        populateListView(machineList);
                        tbName.Text = "";
                        tbDesc.Text = "";
                        currentMachine = new MachineTO();
                        this.Invalidate();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Machines.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnMachineXEmployee_Click(object sender, EventArgs e)
        {
            if (this.lvMachines.SelectedItems.Count == 1)
            {
                currentMachine = (MachineTO)this.lvMachines.SelectedItems[0].Tag;
                MachinesXEmployees form = new MachinesXEmployees(currentMachine);
                form.ShowDialog();
            }
            else
            {
                MachinesXEmployees form = new MachinesXEmployees();
                form.ShowDialog();
            }

        }
    }
}
