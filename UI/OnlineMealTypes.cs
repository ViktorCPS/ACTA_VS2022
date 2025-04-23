using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Util;
using System.Resources;
using Common;
using System.Globalization;
using System.Collections;
using TransferObjects;

namespace UI
{
    public partial class OnlineMealTypes : UserControl
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        OnlineMealsTypesTO currentMealTypeTO;
        List<OnlineMealsTypesTO> currentMealTypeList;
        private ListViewItemComparer _comp;

        // List View indexes
        const int MealTypeIDIndex = 0;
        const int NameIndex = 1;
        const int DescriptionIndex = 2;
        const int FromIndex = 3;
        const int ToIndex = 4;


     

        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;

        string menuItemID;
        string menuItemMealTypeID;
       
        bool addMealTypePermission = false;
        bool updateMealTypePermission = false;
        bool deleteMealTypePermission = false;

     

        public OnlineMealTypes()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                rm = new ResourceManager("UI.Resource", typeof(MealTypes).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();

                clearCurrentMealType();

               
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Inner Class for sorting items in View List

        /*
		 *  Class used for sorting items in the List View 
		 */
        private class ListViewItemComparer : IComparer
        {
            private ListView _listView;

            public ListViewItemComparer(ListView lv)
            {
                _listView = lv;
            }
            public ListView ListView
            {
                get { return _listView; }
            }

            private int _sortColumn = 0;

            public int SortColumn
            {
                get { return _sortColumn; }
                set { _sortColumn = value; }
            }

            public int Compare(object a, object b)
            {
                ListViewItem item1 = (ListViewItem)a;
                ListViewItem item2 = (ListViewItem)b;

                if (ListView.Sorting == SortOrder.Descending)
                {
                    ListViewItem temp = item1;
                    item1 = item2;
                    item2 = temp;
                }
                // Handle non Detail Cases
                return CompareItems(item1, item2);
            }

            public int CompareItems(ListViewItem item1, ListViewItem item2)
            {
                // Subitem instances
                ListViewItem.ListViewSubItem sub1 = item1.SubItems[SortColumn];
                ListViewItem.ListViewSubItem sub2 = item2.SubItems[SortColumn];

                // Return value based on sort column	
                switch (SortColumn)
                {
                    case UI.OnlineMealTypes.MealTypeIDIndex:
                        {
                            int firstID = -1;
                            int secondID = -1;

                            if (!sub1.Text.Trim().Equals(""))
                            {
                                firstID = Int32.Parse(sub1.Text.Trim());
                            }

                            if (!sub2.Text.ToString().Trim().Equals(""))
                            {
                                secondID = Int32.Parse(sub2.Text.Trim());
                            }

                            return CaseInsensitiveComparer.Default.Compare(firstID, secondID);
                        }
                    case UI.OnlineMealTypes.NameIndex:
                    case UI.OnlineMealTypes.DescriptionIndex:
                    case UI.OnlineMealTypes.FromIndex:
                    case UI.OnlineMealTypes.ToIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");
                }
            }
        }

        #endregion

        private void setLanguage()
        {
            try
            {
                // button's text
                btnMealTypeAdd.Text = rm.GetString("btnAdd", culture);
                btnMealTypeUpdate.Text = rm.GetString("btnUpdate", culture);
                btnMealTypeDelete.Text = rm.GetString("btnDelete", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClear.Text = rm.GetString("btnClear", culture);

                // groupbox text
                gbSearch.Text = rm.GetString("gbSearch", culture);

                //label's text
                this.lblMealTypeID.Text = rm.GetString("lblMealTypeID", culture);
                this.lblDescription.Text = rm.GetString("lblDescription", culture);
                this.lblName.Text = rm.GetString("lblName", culture);
              
                // list view initialization
                lvMealTypes.BeginUpdate();
                lvMealTypes.Columns.Add(rm.GetString("hdrMealTypeID", culture), (lvMealTypes.Width - 5) / 3 - 4, HorizontalAlignment.Right);
                lvMealTypes.Columns.Add(rm.GetString("hdrName", culture), (lvMealTypes.Width - 5) / 3 - 4, HorizontalAlignment.Left);
                lvMealTypes.Columns.Add(rm.GetString("hdrDescription", culture), (lvMealTypes.Width - 5) /3 - 4, HorizontalAlignment.Left);
               
                lvMealTypes.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypes.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void setVisibility()
        {
            try
            {

                int permissionMealType;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permissionMealType = (((int[])menuItemsPermissions[menuItemMealTypeID])[role.ApplRoleID]);

                    addMealTypePermission = addMealTypePermission || (((permissionMealType / 4) % 2) == 0 ? false : true);
                    updateMealTypePermission = updateMealTypePermission || (((permissionMealType / 2) % 2) == 0 ? false : true);
                    deleteMealTypePermission = deleteMealTypePermission || ((permissionMealType % 2) == 0 ? false : true);

                }

                btnMealTypeAdd.Enabled = addMealTypePermission;
                btnMealTypeDelete.Enabled = deleteMealTypePermission;
                btnMealTypeUpdate.Enabled = updateMealTypePermission;
            

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Securityroutes.setVisibility(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public void populateListView(List<OnlineMealsTypesTO> mealTypesList)
        {
            try
            {
                lvMealTypes.BeginUpdate();
                lvMealTypes.Items.Clear();

                if (mealTypesList.Count > 0)
                {
                    for (int i = 0; i < mealTypesList.Count; i++)
                    {
                        OnlineMealsTypesTO mealType = (OnlineMealsTypesTO)mealTypesList[i];
                        ListViewItem item = new ListViewItem();

                        item.Text = mealType.MealTypeID.ToString().Trim();
                        item.SubItems.Add(mealType.Name.Trim());
                        item.SubItems.Add(mealType.Description.Trim());
                      
                        lvMealTypes.Items.Add(item);
                    }
                }

                lvMealTypes.EndUpdate();
                lvMealTypes.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypes.populateListView(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void lvMealTypes_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lvMealTypes.SelectedItems.Count == 1)
            {
                try
                {
                    // populate MealType's search form
                    tbMealTypeID.Text = lvMealTypes.SelectedItems[0].Text.Trim();
                    currentMealTypeTO.MealTypeID = Int32.Parse(tbMealTypeID.Text);
                    currentMealTypeTO.Name = tbName.Text = lvMealTypes.SelectedItems[0].SubItems[1].Text.Trim();
                    currentMealTypeTO.Description = tbDescription.Text = lvMealTypes.SelectedItems[0].SubItems[2].Text.Trim();
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now + " MealTypes.lvMealTypes_SelectedIndexChanged(): " + ex.Message + "\n");
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                tbMealTypeID.Text = "";
                tbDescription.Text = "";
                tbName.Text = "";

                clearCurrentMealType();

                lvMealTypes.Items.Clear();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealType.btnClear_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                currentMealTypeTO = new OnlineMealsTypesTO();
                // validate
                if (!tbMealTypeID.Text.Equals(""))
                {
                    int id;
                    bool idIsInt = Int32.TryParse(tbMealTypeID.Text.ToString().Trim(), out id);

                    if (!idIsInt)
                    {
                        MessageBox.Show(rm.GetString("MealTypeIDNotInt", culture));
                        return;
                    }
                    else
                    {
                        currentMealTypeTO.MealTypeID = id;
                    }
                }
                
                if (!tbName.Text.Equals(""))
                {
                    currentMealTypeTO.Name = tbName.Text.ToString().Trim();
                }
                
                if (!tbDescription.Text.Equals(""))
                {
                    currentMealTypeTO.Description = tbDescription.Text.ToString().Trim();
                }

                Common.OnlineMealsTypes onlineMealTypes = new OnlineMealsTypes();
                onlineMealTypes.OnlineMealsTypesTO = currentMealTypeTO;
                currentMealTypeList = onlineMealTypes.Search();

                if (currentMealTypeList.Count > 0)
                {
                    populateListView(currentMealTypeList);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noMealTypesFound", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealType.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                OnlineMealTypesAdd mta = new OnlineMealTypesAdd();
                mta.ShowDialog();
                if (mta.reloadOnReturn)
                {
                    btnClear.PerformClick();
                    currentMealTypeList = new OnlineMealsTypes().Search();
                    populateListView(currentMealTypeList);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealType.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvMealTypes.SelectedItems.Count > 0)
                {
                    OnlineMealTypesAdd mta = new OnlineMealTypesAdd(currentMealTypeTO);
                    mta.ShowDialog();
                    if (mta.reloadOnReturn)
                    {
                        btnClear.PerformClick();
                        currentMealTypeList = new OnlineMealsTypes().Search();
                        populateListView(currentMealTypeList);
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noMealTypeUpd", culture));
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealType.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool isDeleted = true;

                if (lvMealTypes.SelectedItems.Count > 0)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("messageDeleteMealType", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        int id = -1;
                        foreach (ListViewItem item in lvMealTypes.SelectedItems)
                        {
                            if (Int32.TryParse(item.Text.Trim(), out id))
                            {
                                isDeleted = new OnlineMealsTypes().Delete(id) && isDeleted;
                                if (!isDeleted)
                                    break;
                            }
                            else
                            {
                                isDeleted = false;
                                break;
                            }
                        }

                        if (isDeleted)
                        {
                            MessageBox.Show(rm.GetString("mealTypesDeleted", culture));
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("mealTypesNotDeleted", culture));
                        }

                        btnClear.PerformClick();
                        currentMealTypeList = new OnlineMealsTypes().Search();
                        populateListView(currentMealTypeList);
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("SelectMealType", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypes.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(rm.GetString("mealTypesNotDeleted", culture));
            }
        }

        private void MealTypes_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                _comp = new ListViewItemComparer(lvMealTypes);
                lvMealTypes.ListViewItemSorter = _comp;
                lvMealTypes.Sorting = SortOrder.Ascending;
                currentMealTypeTO.MealTypeID = -1;
                currentMealTypeTO.Description = "";
                currentMealTypeTO.Name = "";
                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();

                menuItemID = NotificationController.GetCurrentMenuItemID();
                int index = menuItemID.LastIndexOf('_');

                menuItemMealTypeID = menuItemID + "_" + rm.GetString("tpMealTypes", culture);
                setVisibility();
                
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " OnlineMealTypes.MealTypes_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

     
        private void tbMealTypeID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int id;
                if (!tbMealTypeID.Text.Equals(""))
                {
                    bool idIsInt = Int32.TryParse(tbMealTypeID.Text.ToString().Trim(), out id);

                    if (!idIsInt)
                    {
                        MessageBox.Show(rm.GetString("OnlineMealTypeIDNotInt", culture));
                        if (!tbMealTypeID.Text.Equals(""))
                        {
                            tbMealTypeID.Text = tbMealTypeID.Text.Substring(0, tbMealTypeID.Text.Length - 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealTypes.tbMealTypeID_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void clearCurrentMealType()
        {
            try
            {
                currentMealTypeTO = new OnlineMealsTypesTO();
                DateTime date = DateTime.Now;
               
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealTypes.tbMealTypeID_TextChanged(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void lvMealTypes_ColumnClick_1(object sender, ColumnClickEventArgs e)
        {
            try
            {
                SortOrder prevOrder = lvMealTypes.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvMealTypes.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvMealTypes.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvMealTypes.Sorting = SortOrder.Ascending;
                }

                lvMealTypes.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypes.lvMealTypes_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        
    }
}
