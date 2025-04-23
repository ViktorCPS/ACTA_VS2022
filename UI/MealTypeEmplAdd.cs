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
    public partial class MealTypeEmplAdd : Form
    {

        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        MealTypeEmpl currentMealTypeEmpl;
        public bool reloadOnReturn;

        public MealTypeEmplAdd()
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                currentMealTypeEmpl = new MealTypeEmpl();

                rm = new ResourceManager("UI.Resource", typeof(MealTypeEmplAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();

                this.btnUpdate.Visible = false;
                this.tbMealTypeEmplID.Enabled = false;
                reloadOnReturn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public MealTypeEmplAdd(MealTypeEmpl mealTypeEmpl)
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                currentMealTypeEmpl = mealTypeEmpl;

                rm = new ResourceManager("UI.Resource", typeof(MealTypeEmplAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();

                this.btnSave.Visible = false;
                this.tbMealTypeEmplID.Enabled = false;
                reloadOnReturn = false;
                setFormValues();
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
                if (currentMealTypeEmpl.MealTypeEmplID != -1)
                {
                    this.Text = rm.GetString("mealTypeEmplUpd", culture);
                    this.gbMealTypeEmpl.Text = rm.GetString("mealTypeEmplUpd", culture);
                }
                else
                {
                    this.Text = rm.GetString("mealTypeEmplAdd", culture);
                    this.gbMealTypeEmpl.Text = rm.GetString("mealTypeEmplAdd", culture);
                }

                this.lblMealTypeEmplID.Text = rm.GetString("lblMealTypeEmplID", culture);
                this.lblDescription.Text = rm.GetString("lblDescription", culture);
                this.lblName.Text = rm.GetString("lblName", culture);

                this.btnCancel.Text = rm.GetString("btnCancel", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);
                this.btnUpdate.Text = rm.GetString("btnUpdate", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmplAdd.setLanguage((): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setFormValues()
        {
            try
            {
                tbMealTypeEmplID.Text = currentMealTypeEmpl.MealTypeEmplID.ToString();
                tbName.Text = currentMealTypeEmpl.Name;
                tbDescription.Text = currentMealTypeEmpl.Description;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmplAdd.setFormValues((): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MealTypeEmplAdd.btnCancel_Click(): " + ex.Message + "\n");
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
                bool updated = false;

                // validate
                if (this.tbName.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("mealTypeEmplNameNotSet", culture));
                    tbName.Focus();
                    return;
                }

                currentMealTypeEmpl.MealTypeEmplID = Int32.Parse(tbMealTypeEmplID.Text.Trim());
                currentMealTypeEmpl.Name = tbName.Text.Trim();
                currentMealTypeEmpl.Description = tbDescription.Text.Trim();

                updated = currentMealTypeEmpl.Update(currentMealTypeEmpl.MealTypeEmplID, currentMealTypeEmpl.Name, currentMealTypeEmpl.Description);

                if (updated)
                {
                    reloadOnReturn = true;
                    MessageBox.Show(rm.GetString("mealTypeUpdated", culture));
                    this.Close();
                }
                else
                {
                    MessageBox.Show(rm.GetString("mealTypeNotUpdated", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // validate
                if (this.tbName.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("mealTypeEmplNameNotSet", culture));
                    tbName.Focus();
                    return;
                }

                currentMealTypeEmpl.Name = tbName.Text.Trim();
                currentMealTypeEmpl.Description = tbDescription.Text.Trim();

                int inserted = currentMealTypeEmpl.Save(currentMealTypeEmpl.Name, currentMealTypeEmpl.Description);
                if (inserted > 0)
                {
                    reloadOnReturn = true;
                    DialogResult result = MessageBox.Show(rm.GetString("mealTypeInserted", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        this.tbMealTypeEmplID.Text = "";
                        this.tbName.Text = "";
                        this.tbDescription.Text = "";
                        this.tbName.Focus();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("mealTypeNotInserted", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void MealTypeEmplAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " MealTypeEmplAdd.MealTypeEmplAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}