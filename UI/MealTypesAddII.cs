using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;
using System.Collections;

namespace UI
{
    public partial class MealTypesAddII : Form
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        MealType currentMealType;
        public bool reloadOnReturn;
        bool canSeeBtnAdditional = false;
        MealTypeAdditionalData currentMealTypeAddData = new MealTypeAdditionalData();

        public MealTypesAddII()
        {
            InitializeComponent();
            this.CenterToScreen();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            currentMealType = new MealType();
            logInUser = NotificationController.GetLogInUser();

            rm = new ResourceManager("UI.Resource", typeof(MealTypesAdd).Assembly);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            setLanguage();

            this.btnUpdate.Visible = false;
            int maxID = currentMealType.getMaxID();
            if (maxID >= 0)
            {
                maxID++;
                tbMealTypeID.Text = maxID .ToString();
                tbMealTypeID.Enabled = false;
            }

            if (currentMealTypeAddData.DoesTableExists() == 1)
            {
                canSeeBtnAdditional = true;
                this.btnAdditionalData.Visible = true;
                this.btnAdditionalData.Enabled = true;
            }
            else
            {
                canSeeBtnAdditional = false;
                this.btnAdditionalData.Visible = false;
                this.btnAdditionalData.Enabled = false;
            }

            
        }
        public MealTypesAddII(MealType mealType)
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

            if (currentMealTypeAddData.DoesTableExists() == 1)
            {
                canSeeBtnAdditional = true;
                this.btnAdditionalData.Visible = true;
                this.btnAdditionalData.Enabled = true;
            }
            else
            {
                canSeeBtnAdditional = false;
                this.btnAdditionalData.Visible = false;
                this.btnAdditionalData.Enabled = false;
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
                log.writeLog(DateTime.Now + " MealTypesAddII.btnCancel_Click(): " + ex.Message + "\n");
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
                int id = -1;
                if (!tbMealTypeID.Text.Trim().Equals(""))
                {
                    if (!Int32.TryParse(tbMealTypeID.Text.Trim(), out id))
                    {
                        MessageBox.Show(rm.GetString("mealTypeIDNotNum", culture));
                        tbMealTypeID.Focus();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("mealTypeIDNotSet", culture));
                    tbMealTypeID.Focus();
                    return;
                }

                if (this.tbName.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("mealTypeNameNotSet", culture));
                    tbName.Focus();
                    return;
                }

                ArrayList list = new MealType().Search(-1, tbName.Text.ToString().Trim(), "", new DateTime(), new DateTime());
                if (list.Count > 0)
                {
                    MessageBox.Show(rm.GetString("mealTypeNameExist", culture));
                    tbName.SelectAll();
                    tbName.Focus();
                    return;
                }

                currentMealType.MealTypeID = id;
                currentMealType.Name = tbName.Text.Trim();
                currentMealType.Description = tbDescription.Text.Trim();
                int inserted = 0;

                if (canSeeBtnAdditional && currentMealTypeAddData.MealTypeAddDataTO != null)
                {
                    bool trans = currentMealType.BeginTransaction();
                    bool succ = trans;

                    if (trans)
                    {
                        try
                        {
                            inserted = currentMealType.Save(currentMealType.MealTypeID, currentMealType.Name, currentMealType.Description, new DateTime(2010, 08, 27), new DateTime(2010, 08, 27), false);
                            succ = succ && (inserted > 0);
                            if (succ)
                            {
                                MealTypeAdditionalData additionalData = new MealTypeAdditionalData();
                                additionalData.SetTransaction(currentMealType.GetTransaction());
                                succ = succ && additionalData.Save(currentMealType.MealTypeID, currentMealTypeAddData.MealTypeAddDataTO.DescriptionAdditional, currentMealTypeAddData.MealTypeAddDataTO.Picture, false) == 1;
                                if (succ)
                                {
                                    currentMealType.CommitTransaction();
                                    inserted = 1;
                                }
                                else
                                {
                                    currentMealType.RollbackTransaction();
                                    inserted = 0;
                                }
                            }
                        }
                        catch
                        {
                            if (currentMealType.GetTransaction() != null)
                            {
                                currentMealType.RollbackTransaction();
                                inserted = 0;
                            }
                        }
                    }
                }
                else
                {
                    inserted = currentMealType.Save(currentMealType.MealTypeID, currentMealType.Name, currentMealType.Description, new DateTime(2010, 08, 27), new DateTime(2010, 08, 27));
                }

                if (inserted > 0)
                {
                    reloadOnReturn = true;
                    DialogResult result = MessageBox.Show(rm.GetString("mealTypeInserted", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        int maxID = currentMealType.getMaxID();
                        if (maxID >= 0)
                        {
                            maxID++;
                            tbMealTypeID.Text = maxID.ToString();
                            tbMealTypeID.Enabled = false;
                        }
                        else
                        {
                            tbMealTypeID.Text = "";
                            tbMealTypeID.Enabled = true;
                        }
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
                log.writeLog(DateTime.Now + " MealTypesAddII.btnSave_Click(): " + ex.Message + "\n");
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

                MealTypeAdditionalDataTO addData = new MealTypeAdditionalData().GetAdditionalData(currentMealType.MealTypeID);
                bool dataExist = false;
                if (addData.MealTypeID > 0)
                    dataExist = true;
                else
                    dataExist = false;

                if (canSeeBtnAdditional && currentMealTypeAddData.MealTypeAddDataTO != null && !dataExist)
                {
                    bool trans = currentMealType.BeginTransaction();
                    bool succ = trans;

                    if (trans)
                    {
                        try
                        {
                            updated  = currentMealType.Update(currentMealType.MealTypeID, currentMealType.Name, currentMealType.Description, new DateTime(2010, 08, 27), new DateTime(2010, 08, 27), false);
                            succ = succ && updated;
                            if (succ)
                            {
                                MealTypeAdditionalData additionalData = new MealTypeAdditionalData();
                                additionalData.SetTransaction(currentMealType.GetTransaction());
                                succ = succ && additionalData.Save(currentMealType.MealTypeID, currentMealTypeAddData.MealTypeAddDataTO.DescriptionAdditional, currentMealTypeAddData.MealTypeAddDataTO.Picture, false) == 1;
                                if (succ)
                                {
                                    currentMealType.CommitTransaction();
                                    updated = true;
                                }
                                else
                                {
                                    currentMealType.RollbackTransaction();
                                    updated = false;
                                }
                            }                           
                        }
                        catch
                        {
                            if (currentMealType.GetTransaction() != null)
                            {
                                currentMealType.RollbackTransaction();
                                updated = false;
                            }
                        }
                    }

                }
                else
                    updated = currentMealType.Update(currentMealType.MealTypeID, currentMealType.Name, currentMealType.Description, new DateTime(2010, 08, 27), new DateTime(2010, 08, 27));
                

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
                log.writeLog(DateTime.Now + " MealTypeAddII.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
                log.writeLog(DateTime.Now + " MealTypeAddII.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdditionalData_Click(object sender, EventArgs e)
        {
            try
            {
                MealTypesAdditionalData additionalData = new MealTypesAdditionalData();
                additionalData.setMealID(tbMealTypeID.Text.ToString());
                additionalData.setMealName(tbName.Text.ToString());
                additionalData.ShowDialog();
                currentMealTypeAddData = additionalData.currentMealTypeAddData; // vraca se objeka
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}