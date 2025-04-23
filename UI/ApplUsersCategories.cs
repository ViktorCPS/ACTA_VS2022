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

namespace UI
{
    public partial class ApplUsersCategories : System.Windows.Forms.Form
    {

        List<ApplUserCategoryTO> currentUserCategoryList;
        ApplUserCategoryTO currentUserCategory;

        // List View indexes		
        const int CategoryIDIndex = 0;
        const int DescriptionIndex = 1;
        const int NameIndex = 2;

        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        public ApplUsersCategories()
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(SystemMessages).Assembly);

            logInUser = NotificationController.GetLogInUser();

            this.CenterToScreen();
            setLanguage();

            currentUserCategoryList = new List<ApplUserCategoryTO>();
            populateNameCombo();
        }

        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("ApplUsersCategories", culture);

                // group box text
                gbApplUsersCategories.Text = rm.GetString("gbApplUsersCategories", culture);

                // button's text
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClose.Text = rm.GetString("btnClose", culture);

                // label's text
                lblName.Text = rm.GetString("lblName", culture);

                lvCategory.BeginUpdate();
                lvCategory.Columns.Add(rm.GetString("UserCatID", culture), 100, HorizontalAlignment.Left);
                lvCategory.Columns.Add(rm.GetString("Name", culture), 160, HorizontalAlignment.Left);
                lvCategory.Columns.Add(rm.GetString("Description", culture), 260, HorizontalAlignment.Left);
                lvCategory.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersCategories.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateNameCombo()
        {
            try
            {
                List<ApplUserCategoryTO> userCategoryArray = new ApplUserCategory().Search(null);
                userCategoryArray.Insert(0, new ApplUserCategoryTO(-1, rm.GetString("all", culture), rm.GetString("all", culture)));

                cbName.DataSource = userCategoryArray;
                cbName.DisplayMember = "Name";
                cbName.ValueMember = "CategoryID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersCategoruies.populateNameCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int id = 0;
                if (cbName.SelectedIndex > 0)
                {
                    id = Int32.Parse(cbName.SelectedValue.ToString().Trim());
                }

                List<ApplUserCategoryTO> userCatList = new List<ApplUserCategoryTO>();
                ApplUserCategory userCategory = new ApplUserCategory();
                userCategory.UserCategoryTO.CategoryID = id;
                currentUserCategoryList = userCategory.Search(null);

                if (currentUserCategoryList.Count > 0)
                {
                    populateListView(currentUserCategoryList);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noUserCategoriesFound", culture));
                }

                currentUserCategory = new ApplUserCategoryTO();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersCategories.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;
                ApplUsersCategoriesAdd addForm = new ApplUsersCategoriesAdd();
                addForm.ShowDialog(this);

                currentUserCategory = new ApplUserCategoryTO();
                List<ApplUserCategoryTO> typeList = new ApplUserCategory().Search(null);
                populateListView(typeList);
                populateNameCombo();
                this.Invalidate();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersCategories.btnAdd_Click(): " + ex.Message + "\n");
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
                if (this.lvCategory.SelectedItems.Count != 1)
                {
                    MessageBox.Show(rm.GetString("selOneCat", culture));
                }
                else
                {
                    currentUserCategory = (ApplUserCategoryTO)lvCategory.SelectedItems[0].Tag;

                    // Open Update Form
                    ApplUsersCategoriesAdd addForm = new ApplUsersCategoriesAdd(currentUserCategory);
                    addForm.ShowDialog(this);

                    List<ApplUserCategoryTO> typeList = new ApplUserCategory().Search(null);
                    populateListView(typeList);
                    populateNameCombo();
                    cbName.SelectedIndex = 0;
                    currentUserCategory = new ApplUserCategoryTO();
                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersCategory.btnUpdate_Click(): " + ex.Message + "\n");
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


                if (lvCategory.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noSelUserCatDel", culture));
                }
                else
                {
                    DialogResult result = MessageBox.Show(rm.GetString("deleteUserCategories", culture), "", MessageBoxButtons.YesNo);
                    bool isDeleted = true;

                    if (result == DialogResult.Yes)
                    {
                        int selected = lvCategory.SelectedItems.Count;

                        foreach (ListViewItem item in lvCategory.SelectedItems)
                        {
                            currentUserCategory = (ApplUserCategoryTO)lvCategory.SelectedItems[0].Tag;
                            ApplUserCategory cat = new ApplUserCategory();
                            cat.UserCategoryTO = currentUserCategory;
                            isDeleted = new ApplUserCategory().Delete(currentUserCategory.CategoryID) && isDeleted;
                        }

                        if ((selected > 0) && isDeleted)
                        {
                            MessageBox.Show(rm.GetString("usersCategoriesDel", culture));
                        }
                        else
                            if (!isDeleted)
                            {
                                MessageBox.Show(rm.GetString("noUsersCategoriesDel", culture));
                            }

                        List<ApplUserCategoryTO> typeList = new ApplUserCategory().Search(null);
                        populateListView(typeList);
                        populateNameCombo();
                        cbName.SelectedIndex = 0;

                        currentUserCategory = new ApplUserCategoryTO();
                        this.Invalidate();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersCategory.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsersCategory.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        
        public void populateListView(List<ApplUserCategoryTO> usersCategoryList)
        {
            try
            {
                lvCategory.BeginUpdate();
                lvCategory.Items.Clear();

                if (usersCategoryList.Count > 0)
                {
                    foreach (ApplUserCategoryTO category in usersCategoryList)
                    {

                        ListViewItem item = new ListViewItem();
                        item = lvCategory.Items.Add(category.CategoryID.ToString());
                        item.SubItems.Add(category.Name.Trim());
                        item.SubItems.Add(category.Desc.Trim());
                        item.Tag = category;
                    }
                }
                lvCategory.EndUpdate();
                lvCategory.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ApplUsers.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
