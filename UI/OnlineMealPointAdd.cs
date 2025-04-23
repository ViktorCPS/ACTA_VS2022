using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;
using Common;
using TransferObjects;
using System.Resources;
using System.Globalization;

namespace UI
{
    public partial class OnlineMealPointAdd : Form
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        OnlineMealsPointsTO currentMealPoint;
        public bool reloadOnReturn;

        public OnlineMealPointAdd()
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                currentMealPoint = new OnlineMealsPointsTO();

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

        public OnlineMealPointAdd(OnlineMealsPointsTO mealPoint)
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


        private void populateMealTypeCombo()
        {
            try
            {
                List<OnlineMealsTypesTO> emplArray = new List<OnlineMealsTypesTO>();

                emplArray = new OnlineMealsTypes().Search();
                OnlineMealsTypesTO empl = new OnlineMealsTypesTO();
                empl.Name = rm.GetString("all", culture);
                emplArray.Insert(0, empl);

                cbMealType.DataSource = emplArray;
                cbMealType.DisplayMember = "Name";
                cbMealType.ValueMember = "MealTypeID";
                cbMealType.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsUsed.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        private void populateRestaurantCombo()
        {
            try
            {
                List<OnlineMealsRestaurantTO> emplArray = new List<OnlineMealsRestaurantTO>();

                emplArray = new OnlineMealsRestaurant().Search();
                OnlineMealsRestaurantTO empl = new OnlineMealsRestaurantTO();
                empl.Name = rm.GetString("all", culture);
                emplArray.Insert(0, empl);

                cbRestaurant.DataSource = emplArray;
                cbRestaurant.DisplayMember = "Name";
                cbRestaurant.ValueMember = "RestaurantID";
                cbRestaurant.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsUsed.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        private void setLanguage()
        {
            try
            {
                if (currentMealPoint.PointID != -1)
                {
                    this.Text = rm.GetString("mealPointUpd", culture);
                    this.gbMealPoint.Text = rm.GetString("mealPointUpd", culture);
                }
                else
                {
                    this.Text = rm.GetString("mealPointAdd", culture);
                    this.gbMealPoint.Text = rm.GetString("mealPointAdd", culture);
                }

                this.lblMealPointID.Text = rm.GetString("lblLineID", culture);
                this.lblDescription.Text = rm.GetString("lblDescription", culture);
                this.lblName.Text = rm.GetString("lblName", culture);
                this.lblTerminalSerial.Text = rm.GetString("hdrRestaurant", culture);
                this.lblAntenna.Text = rm.GetString("lblReaderAnt", culture);
                this.lblIPAddress.Text = rm.GetString("lblIpAddress", culture);
                this.lblPeripherialDesc.Text = rm.GetString("lblReaderPeripherialDesc", culture);
                this.lblPreipherial.Text = rm.GetString("lblReaderPeripherial", culture);
                this.lblMealType.Text = rm.GetString("lblMealsType", culture);

                this.btnCancel.Text = rm.GetString("btnCancel", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);
                this.btnUpdate.Text = rm.GetString("btnUpdate", culture);

                populateMealTypeCombo();
                populateRestaurantCombo();
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
                tbMealPointID.Text = currentMealPoint.PointID.ToString();
                tbName.Text = currentMealPoint.Name;
                tbDescription.Text = currentMealPoint.Description;

                cbRestaurant.SelectedIndex = cbRestaurant.FindStringExact(currentMealPoint.RestaurantName);
                cbMealType.SelectedIndex = cbMealType.FindStringExact(currentMealPoint.MealType);


                tbAntenna.Text = currentMealPoint.Reader_ant.ToString();
                tbIPAddress.Text = currentMealPoint.ReaderIPAddress;
                tbPeripherial.Text = currentMealPoint.Reader_peripherial.ToString();
                tbPeripherialDesc.Text = currentMealPoint.ReaderPeripherialDesc;

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPointAdd.setFormValues((): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void clearFormValues()
        {
            try
            {
                tbMealPointID.Text = "";
                tbName.Text = "";
                tbDescription.Text = "";

                cbRestaurant.SelectedIndex = 0;
                cbMealType.SelectedIndex = 0;


                tbAntenna.Text = "";
                tbIPAddress.Text = "";
                tbPeripherial.Text = "";
                tbPeripherialDesc.Text = "";

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
                if (cbMealType.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("mealTypeNotSet", culture));
                    cbMealType.Focus();
                    return;
                }


                if (this.tbName.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("mealPointNameNotSet", culture));
                    tbName.Focus();
                    return;
                }
                if (cbRestaurant.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("mealRestaurantNotSet", culture));
                    cbRestaurant.Focus();
                    return;
                }

                if (this.tbIPAddress.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("mealIPaddressNotSet", culture));
                    tbName.Focus();
                    return;
                }

                if (this.tbPeripherial.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("mealPeripherialNotSet", culture));
                    tbName.Focus();
                    return;
                }
                if (this.tbAntenna.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("mealAntenaNotSet", culture));
                    tbName.Focus();
                    return;
                }
                currentMealPoint.Name = tbName.Text.Trim();
                currentMealPoint.Description = tbDescription.Text.Trim();
                currentMealPoint.RestaurantID = (int)cbRestaurant.SelectedValue;
                currentMealPoint.ReaderIPAddress = tbIPAddress.Text;
                currentMealPoint.Reader_peripherial = Int32.Parse(tbPeripherial.Text);
                currentMealPoint.ReaderPeripherialDesc = tbPeripherialDesc.Text;
                currentMealPoint.Reader_ant = Int32.Parse(tbAntenna.Text);
                currentMealPoint.MealTypeID = (int)cbMealType.SelectedValue;
                OnlineMealsPoints onlineMealPoint = new OnlineMealsPoints();
                onlineMealPoint.OnlineMealsPointTO = currentMealPoint;
                int inserted = onlineMealPoint.Save();

                if (inserted > 0)
                {
                    reloadOnReturn = true;
                    DialogResult result = MessageBox.Show(rm.GetString("mealPointInserted", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        this.tbMealPointID.Text = "";
                        this.tbName.Text = "";
                        this.tbDescription.Text = "";
                        this.cbRestaurant.SelectedIndex = 0;
                        clearFormValues();
                        this.cbRestaurant.Focus();
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
                if (cbMealType.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("mealTypeNotSet", culture));
                    cbMealType.Focus();
                    return;
                }


                if (this.tbName.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("mealPointNameNotSet", culture));
                    tbName.Focus();
                    return;
                }
                if (cbRestaurant.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("mealRestaurantNotSet", culture));
                    cbRestaurant.Focus();
                    return;
                }

                if (this.tbIPAddress.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("mealIPaddressNotSet", culture));
                    tbName.Focus();
                    return;
                }

                if (this.tbPeripherial.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("mealPeripherialNotSet", culture));
                    tbName.Focus();
                    return;
                }
                if (this.tbAntenna.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("mealAntenaNotSet", culture));
                    tbName.Focus();
                    return;
                }


                currentMealPoint.PointID = Int32.Parse(tbMealPointID.Text.Trim());
                currentMealPoint.Name = tbName.Text.Trim();
                currentMealPoint.Description = tbDescription.Text.Trim();
                currentMealPoint.RestaurantID = (int)cbRestaurant.SelectedValue;
                currentMealPoint.Reader_ant = Int32.Parse(tbAntenna.Text);
                currentMealPoint.Reader_peripherial = Int32.Parse(tbPeripherial.Text);
                currentMealPoint.ReaderIPAddress = tbIPAddress.Text;
                currentMealPoint.ReaderPeripherialDesc = tbPeripherialDesc.Text;
                currentMealPoint.MealTypeID = (int) cbMealType.SelectedValue;
                OnlineMealsPoints onlineMeal = new OnlineMealsPoints();
                onlineMeal.OnlineMealsPointTO = currentMealPoint;
                updated = onlineMeal.Update();
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
            finally
            {
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