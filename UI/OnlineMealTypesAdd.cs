using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;
using Common;
using System.Resources;
using System.Globalization;
using TransferObjects;

namespace UI
{
    public partial class OnlineMealTypesAdd : Form
    {
       DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        OnlineMealsTypesTO currentMealType;
        public bool reloadOnReturn;
        
        public OnlineMealTypesAdd()
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                lblMealTypeID.Visible = tbMealTypeID.Visible = label3.Visible= false;
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                currentMealType = new OnlineMealsTypesTO();
                logInUser = NotificationController.GetLogInUser();

                rm = new ResourceManager("UI.Resource", typeof(OnlineMealTypesAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();
                
                this.btnUpdate.Visible = false;
               
                reloadOnReturn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        public OnlineMealTypesAdd(OnlineMealsTypesTO mealType)
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();
                
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                currentMealType = mealType;
                logInUser = NotificationController.GetLogInUser();

                rm = new ResourceManager("UI.Resource", typeof(MealTypesAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();

                this.btnSave.Visible = false;
                this.tbMealTypeID.Enabled = false;
               
                reloadOnReturn = false;
                setFormValues();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setFormValues()
        {
            try
            {
                tbMealTypeID.Text = currentMealType.MealTypeID.ToString();
                tbName.Text = currentMealType.Name;
                tbDescription.Text = currentMealType.Description;
               
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesAdd.setFormValues((): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MealTypesAdd.btnCancel_Click((): " + ex.Message + "\n");
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
                int id = -1;
               
                currentMealType.MealTypeID = id;
                currentMealType.Name = tbName.Text.Trim();
                currentMealType.Description = tbDescription.Text.Trim();
              
                int inserted = new OnlineMealsTypes().Save(currentMealType.Name, currentMealType.Description);
                if (inserted > 0)
                {
                    reloadOnReturn = true;
                    DialogResult result = MessageBox.Show(rm.GetString("mealTypeInserted", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        this.tbMealTypeID.Text = "";
                        this.tbName.Text = "";
                        this.tbDescription.Text = "";

                        this.tbMealTypeID.Focus();
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
                log.writeLog(DateTime.Now + " MealTypesAdd.btnSave_Click(): " + ex.Message + "\n");
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
                if (this.tbName.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("mealTypeNameNotSet", culture));
                    tbName.Focus();
                    return;
                }

                currentMealType.MealTypeID = Int32.Parse(tbMealTypeID.Text.Trim());
                currentMealType.Name = tbName.Text.Trim();
                currentMealType.Description = tbDescription.Text.Trim();
               

                updated = new OnlineMealsTypes().Update(currentMealType.MealTypeID, currentMealType.Name, currentMealType.Description);

                if (updated)
                {
                    reloadOnReturn = true;
                    MessageBox.Show(rm.GetString("mealTypeUpdated", culture));
                }
                else
                {
                    MessageBox.Show(rm.GetString("mealTypeNotUpdated", culture));
                }

                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setLanguage()
        {
            try
            {
                if (currentMealType.MealTypeID != -1)
                {
                    this.Text = rm.GetString("mealTypesUpd", culture);
                    this.gbMealType.Text = rm.GetString("mealTypesUpd", culture);
                }
                else
                {
                    this.Text = rm.GetString("mealTypesAdd", culture);
                    this.gbMealType.Text = rm.GetString("mealTypesAdd", culture);
                }

                this.lblMealTypeID.Text = rm.GetString("lblMealTypeID", culture);
                this.lblDescription.Text = rm.GetString("lblDescription", culture);
                this.lblName.Text = rm.GetString("lblName", culture);
              
                this.btnCancel.Text = rm.GetString("btnCancel", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);
                this.btnUpdate.Text = rm.GetString("btnUpdate", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeAdd.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void MealTypesAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " MealTypesAdd.MealTypesAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
       
    }
}