using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;
using System.Resources;
using TransferObjects;
using System.Globalization;
using Common;

namespace UI
{
    public partial class OnlineMealRestaurantAdd : Form
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        OnlineMealsRestaurantTO currentRestaurant;
        public bool reloadOnReturn;
        
        public OnlineMealRestaurantAdd()
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                lblRestaurantID.Visible = tbRestaurantID.Visible = label3.Visible= false;
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                currentRestaurant = new OnlineMealsRestaurantTO();
                logInUser = NotificationController.GetLogInUser();

                rm = new ResourceManager("UI.Resource", typeof(OnlineMealRestaurantAdd).Assembly);
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

        public OnlineMealRestaurantAdd(OnlineMealsRestaurantTO restaurantTO)
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();
                
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                currentRestaurant = restaurantTO;
                logInUser = NotificationController.GetLogInUser();

                rm = new ResourceManager("UI.Resource", typeof(MealTypesAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();

                this.btnSave.Visible = false;
                this.tbRestaurantID.Enabled = false;
               
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
                tbRestaurantID.Text = currentRestaurant.RestaurantID.ToString();
                tbName.Text = currentRestaurant.Name;
                tbDescription.Text = currentRestaurant.Description;
               
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsRestaurantADD.setFormValues((): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " OnlineMealsRestaurantADD.btnCancel_Click((): " + ex.Message + "\n");
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
               
                currentRestaurant.RestaurantID = id;
                currentRestaurant.Name = tbName.Text.Trim();
                currentRestaurant.Description = tbDescription.Text.Trim();
              
                int inserted = new OnlineMealsRestaurant().Save(currentRestaurant.Name, currentRestaurant.Description);
                if (inserted > 0)
                {
                    reloadOnReturn = true;
                    DialogResult result = MessageBox.Show(rm.GetString("restaurantInserted", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        this.tbRestaurantID.Text = "";
                        this.tbName.Text = "";
                        this.tbDescription.Text = "";

                        this.tbRestaurantID.Focus();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("restorantNotInserted", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsRestaurantADD.btnSave_Click(): " + ex.Message + "\n");
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
                    MessageBox.Show(rm.GetString("restaurantNameNotSet", culture));
                    tbName.Focus();
                    return;
                }

                currentRestaurant.RestaurantID = Int32.Parse(tbRestaurantID.Text.Trim());
                currentRestaurant.Name = tbName.Text.Trim();
                currentRestaurant.Description = tbDescription.Text.Trim();
               

                updated = new OnlineMealsRestaurant().Update(currentRestaurant.RestaurantID, currentRestaurant.Name, currentRestaurant.Description);

                if (updated)
                {
                    reloadOnReturn = true;
                    MessageBox.Show(rm.GetString("restaurantUpdated", culture));
                }
                else
                {
                    MessageBox.Show(rm.GetString("restaurantNotUpdated", culture));
                }

                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsRestaurantADD.btnUpdate_Click(): " + ex.Message + "\n");
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
                if (currentRestaurant.RestaurantID != -1)
                {
                    this.Text = rm.GetString("restaurantUpd", culture);
                    this.gbRestaurant.Text = rm.GetString("restaurantUpd", culture);
                }
                else
                {
                    this.Text = rm.GetString("restaurantAdd", culture);
                    this.gbRestaurant.Text = rm.GetString("restaurantAdd", culture);
                }

                this.lblRestaurantID.Text = rm.GetString("lblRestaurantID", culture);
                this.lblDescription.Text = rm.GetString("lblDescription", culture);
                this.lblName.Text = rm.GetString("lblName", culture);
              
                this.btnCancel.Text = rm.GetString("btnCancel", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);
                this.btnUpdate.Text = rm.GetString("btnUpdate", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsRestaurantADD.setLanguage(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " OnlineMealsRestaurantADD.MealTypesAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
       
    }
}