using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;

namespace UI
{
    public partial class MealPointAdd : Form
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        MealPoint currentMealPoint;
        public bool reloadOnReturn;

        public MealPointAdd()
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                currentMealPoint = new MealPoint();

                rm = new ResourceManager("UI.Resource", typeof(MealPointAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();

                this.btnUpdate.Visible = false;
                this.tbMealPointID.Enabled = false;
                reloadOnReturn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public MealPointAdd(MealPoint mealPoint)
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                currentMealPoint = mealPoint;

                rm = new ResourceManager("UI.Resource", typeof(MealPointAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();

                this.btnSave.Visible = false;
                this.tbMealPointID.Enabled = false;
                setFormValues();
                reloadOnReturn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setLanguage()
        {
            try
            {
                if (currentMealPoint.MealPointID != -1)
                {
                    this.Text = rm.GetString("mealPointUpd", culture);
                    this.gbMealPoint.Text = rm.GetString("mealPointUpd", culture);
                }
                else
                {
                    this.Text = rm.GetString("mealPointAdd", culture);
                    this.gbMealPoint.Text = rm.GetString("mealPointAdd", culture);
                }

                this.lblMealPointID.Text = rm.GetString("lblMealPointID", culture);
                this.lblDescription.Text = rm.GetString("lblDescription", culture);
                this.lblName.Text = rm.GetString("lblName", culture);
                this.lblTerminalSerial.Text = rm.GetString("lblTerminalSerial", culture);

                this.btnCancel.Text = rm.GetString("btnCancel", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);
                this.btnUpdate.Text = rm.GetString("btnUpdate", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPointAdd.setLanguage((): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setFormValues()
        {
            try
            {
                tbMealPointID.Text = currentMealPoint.MealPointID.ToString();
                tbName.Text = currentMealPoint.Name;
                tbDescription.Text = currentMealPoint.Description;
                tbTerminalSerial.Text = currentMealPoint.TerminalSerial;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPointAdd.setFormValues((): " + ex.Message + "\n");
                throw ex;
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
                log.writeLog(DateTime.Now + " MealPointAdd.btnCancel_Click(): " + ex.Message + "\n");
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
                if (this.tbTerminalSerial.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("mealPointTerminalNotSet", culture));
                    tbTerminalSerial.Focus();
                    return;
                }

                if (this.tbName.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("mealPointNameNotSet", culture));
                    tbName.Focus();
                    return;
                }

                currentMealPoint.Name = tbName.Text.Trim();
                currentMealPoint.Description = tbDescription.Text.Trim();
                currentMealPoint.TerminalSerial = tbTerminalSerial.Text.Trim();

                int inserted = currentMealPoint.Save(currentMealPoint.TerminalSerial, currentMealPoint.Name, currentMealPoint.Description);

                if (inserted > 0)
                {
                    reloadOnReturn = true;
                    DialogResult result = MessageBox.Show(rm.GetString("mealPointInserted", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        this.tbMealPointID.Text = "";
                        this.tbName.Text = "";
                        this.tbDescription.Text = "";
                        this.tbTerminalSerial.Text = "";
                        this.tbTerminalSerial.Focus();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("mealPointNotInserted", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPointAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                bool updated = false;

                // validate
                if (this.tbTerminalSerial.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("mealPointTerminalNotSet", culture));
                    tbTerminalSerial.Focus();
                    return;
                }

                if (this.tbName.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("mealPointNameNotSet", culture));
                    tbName.Focus();
                    return;
                }

                currentMealPoint.MealPointID = Int32.Parse(tbMealPointID.Text.Trim());
                currentMealPoint.Name = tbName.Text.Trim();
                currentMealPoint.Description = tbDescription.Text.Trim();
                currentMealPoint.TerminalSerial = tbTerminalSerial.Text.Trim();

                updated = currentMealPoint.Update(currentMealPoint.MealPointID, currentMealPoint.TerminalSerial, currentMealPoint.Name, currentMealPoint.Description);
                if (updated)
                {
                    reloadOnReturn = true;
                    MessageBox.Show(rm.GetString("mealPointUpdated", culture));
                    this.Close();
                }
                else
                {
                    MessageBox.Show(rm.GetString("mealPointNotUpdated", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPointAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void MealPointAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " MealPointAdd.MealPointAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}