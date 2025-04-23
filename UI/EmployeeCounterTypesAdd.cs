using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Resources;
using Common;
using TransferObjects;
using Util;
using System.Globalization;

namespace UI
{
    public partial class EmployeeCounterTypesAdd : Form
    {

        private CultureInfo culture;
        // Current EmployeeCounterType
        EmployeeCounterTypeTO currentType = null;
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;

        public bool reloadOnReturn;

        // Controller instance
        public NotificationController Controller;
        // Observer client instance
        public NotificationObserverClient observerClient;

        //Indicate if calling form needs to be reload
        public bool doReloadOnBack = true;

        public EmployeeCounterTypesAdd()
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            currentType = new EmployeeCounterTypeTO();
            logInUser = NotificationController.GetLogInUser();

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

            this.CenterToScreen();

            rm = new ResourceManager("UI.Resource", typeof(EmployeeCounterTypesAdd).Assembly);
            setLanguage();
            label3.Visible = true;
            btnUpdate.Visible = false;
        }

        public EmployeeCounterTypesAdd(EmployeeCounterTypeTO employeeCounterTypes)
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            currentType = new EmployeeCounterTypeTO(employeeCounterTypes.EmplCounterTypeID, employeeCounterTypes.Name, employeeCounterTypes.NameAlt, employeeCounterTypes.Desc);

            logInUser = NotificationController.GetLogInUser();

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

            this.CenterToScreen();

            rm = new ResourceManager("UI.Resource", typeof(EmployeeCounterTypesAdd).Assembly);
            setLanguage();

            tbCounterTypeID.Text = employeeCounterTypes.EmplCounterTypeID.ToString().Trim();
            tbName.Text = employeeCounterTypes.Name.Trim();
            tbNameAlt.Text = employeeCounterTypes.NameAlt.Trim();
            tbDescription.Text = employeeCounterTypes.Desc.Trim();
            btnSave.Visible = false;
            label3.Visible = false;
            tbCounterTypeID.Enabled = false;
        }

        private void setLanguage()
        {
            try
            {
                // Form name
                if (currentType.EmplCounterTypeID >= 0)
                {
                    this.Text = rm.GetString("updateEmployeeCounterTypes", culture);
                }
                else
                {
                    this.Text = rm.GetString("addEmployeeCounterTypes", culture);
                }

                // button's text
                btnSave.Text = rm.GetString("btnSave", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);

                // label's text

                lblID.Text = rm.GetString("lblID", culture);
                lblName.Text = rm.GetString("lblName1", culture);
                lblAltName.Text = rm.GetString("lblAltName", culture);
                lblDescription.Text = rm.GetString("lblDescription", culture);

                gbAddCounterTypes.Text = rm.GetString("gbAddCounterTypes", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterTypesAdd.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (this.tbName.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("empCountTypesNameNotSet", culture));
                    this.tbName.Focus();
                    return;
                }

                if (this.tbNameAlt.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("empCountTypesNameAltNotSet", culture));
                    this.tbNameAlt.Focus();
                    return;
                }

                EmployeeCounterType type = new EmployeeCounterType();
                currentType.EmplCounterTypeID = Int32.Parse(this.tbCounterTypeID.Text.Trim());
                currentType.Name = this.tbName.Text.Trim();
                currentType.NameAlt = this.tbNameAlt.Text.Trim();
                currentType.Desc = this.tbDescription.Text.Trim();
                type.TypeTO = currentType;


                if (type.Update())
                {
                    reloadOnReturn = true;
                    MessageBox.Show(rm.GetString("EmployeeCounterTypesUpdated", culture));
                    this.Close();
                }
                else
                {
                    reloadOnReturn = true;
                    MessageBox.Show(rm.GetString("EmpCountTypeNotUpdated", culture));
                    this.Close();
                }


            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterTypes.btnSave_Click(): " + ex.Message + "\n");
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
                // validate
                if (this.tbCounterTypeID.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("employeeCounterTypesIDNotSet", culture));
                    this.tbCounterTypeID.Focus();
                    return;
                }

                if (this.tbName.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("empCountTypesNameNotSet", culture));
                    this.tbName.Focus();
                    return;
                }

                if (this.tbNameAlt.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("empCountTypesNameAltNotSet", culture));
                    this.tbNameAlt.Focus();
                    return;
                }

                EmployeeCounterType newType = new EmployeeCounterType();
                newType.TypeTO.Name = tbName.Text.Trim();
                List<EmployeeCounterTypeTO> types = newType.Search();

                if (types.Count == 0)
                {
                    currentType.EmplCounterTypeID = Int32.Parse(this.tbCounterTypeID.Text.Trim());
                    currentType.Name = this.tbName.Text.Trim();
                    currentType.NameAlt = this.tbNameAlt.Text.Trim();
                    currentType.Desc = this.tbDescription.Text.Trim();

                    int inserted = new EmployeeCounterType().Save(currentType);
                    if (inserted > 0)
                    {
                        reloadOnReturn = true;
                        DialogResult result = MessageBox.Show(rm.GetString("CounterTypeInserted", culture), "", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {
                            this.tbCounterTypeID.Text = "";
                            this.tbName.Text = "";
                            this.tbNameAlt.Text = "";
                            this.tbDescription.Text = "";
                            this.tbName.Focus();
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("CounterTypeNotInserted", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeCounterTypesAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
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
                log.writeLog(DateTime.Now + " EmployeeCounterTypesAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;

            }
        }
    }
}