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
    public partial class ApplUsersCategoriesAdd : Form
    {
        private CultureInfo culture;
        // Current EmployeeCounterType
        ApplUserCategoryTO currentCategory = null;
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

        private void setLanguage()
        {
            try
            {
                // Form name
                if (currentCategory.CategoryID >= 0)
                {
                    this.Text = rm.GetString("updateApplUsersCategory", culture);
                }
                else
                {
                    this.Text = rm.GetString("addApplUsersCategory", culture);
                }

                // button's text
                btnSave.Text = rm.GetString("btnSave", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);

                // label's text

                lblID.Text = rm.GetString("lblID", culture);
                lblName.Text = rm.GetString("lblName", culture);
                lblDescription.Text = rm.GetString("lblDescription", culture);

                gbAddUsersCategories.Text = rm.GetString("gbAddUsersCategories", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersCategoriesAdd.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        
        public ApplUsersCategoriesAdd()
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            currentCategory = new ApplUserCategoryTO();
            logInUser = NotificationController.GetLogInUser();

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

            this.CenterToScreen();

            rm = new ResourceManager("UI.Resource", typeof(EmployeeCounterTypesAdd).Assembly);
            setLanguage();

            label1.Visible = true;
            btnUpdate.Visible = false;
        }

        public ApplUsersCategoriesAdd(ApplUserCategoryTO applUserCategory)
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            currentCategory = new ApplUserCategoryTO(applUserCategory.CategoryID, applUserCategory.Name, applUserCategory.Desc);

            logInUser = NotificationController.GetLogInUser();

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

            this.CenterToScreen();

            rm = new ResourceManager("UI.Resource", typeof(ApplUsersCategoriesAdd).Assembly);
            setLanguage();

            tbUsersCategoryID.Text = applUserCategory.CategoryID.ToString().Trim();
            tbName.Text = applUserCategory.Name.Trim();
            tbDescription.Text = applUserCategory.Desc.Trim();
            label1.Visible = false;
            btnSave.Visible = false;
            tbUsersCategoryID.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // validate
                if (this.tbUsersCategoryID.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("applUserCategoryIDNotSet", culture));
                    this.tbUsersCategoryID.Focus();
                    return;
                }

                if (this.tbName.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("applUserCategoryNameNotSet", culture));
                    this.tbName.Focus();
                    return;
                }

                ApplUserCategory newCat = new ApplUserCategory();
                newCat.UserCategoryTO.Name = tbName.Text.Trim();
                List<ApplUserCategoryTO> categories = newCat.Search(null);

                if (categories.Count == 0)
                {
                    currentCategory.CategoryID = Int32.Parse(this.tbUsersCategoryID.Text.Trim());
                    currentCategory.Name = this.tbName.Text.Trim();
                    currentCategory.Desc = this.tbDescription.Text.Trim();

                    int inserted = new ApplUserCategory().Save1(currentCategory);
                    if (inserted > 0)
                    {
                        reloadOnReturn = true;
                        DialogResult result = MessageBox.Show(rm.GetString("UserCategoryInserted", culture), "", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {
                            this.tbUsersCategoryID.Text = "";
                            this.tbName.Text = "";
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
                    MessageBox.Show(rm.GetString("UserCategoryNotInserted", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersCategoriesAdd.btnSave_Click(): " + ex.Message + "\n");
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

                if (this.tbName.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("ApplUsersCategoriesNameNotSett", culture));
                    this.tbName.Focus();
                    return;
                }

                ApplUserCategory category = new ApplUserCategory();
                currentCategory.CategoryID = Int32.Parse(this.tbUsersCategoryID.Text.Trim());
                currentCategory.Name = this.tbName.Text.Trim();
                currentCategory.Desc = this.tbDescription.Text.Trim();
                category.UserCategoryTO = currentCategory;

                if (category.Update(true))
                {
                    reloadOnReturn = true;
                    MessageBox.Show(rm.GetString("UserCategoryUpdated", culture));
                    this.Close();
                }
                else
                {
                    reloadOnReturn = true;
                    MessageBox.Show(rm.GetString("UserCategoryNotUpdated", culture));
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersCategories.btnSave_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " ApplUsersCategoriesAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;

            }
        }
    }
}
