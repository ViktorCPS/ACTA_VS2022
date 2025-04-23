using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Util;
using System.Resources;
using TransferObjects;
using Common;
using System.Globalization;
using System.Collections;

namespace UI
{
    public partial class OnlineMealRestaurant : UserControl
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        OnlineMealsRestaurantTO currentRestaurantTO;
        List<OnlineMealsRestaurantTO> currentRestaurantList;
        private ListViewItemComparer _comp;

        // List View indexes
        const int RestaurantIDIndex = 0;
        const int NameIndex = 1;
        const int DescriptionIndex = 2;

        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;

        string menuItemID;
        string menuItemRestaurantID;

        bool addRestaurantPermission = false;
        bool updateRestaurantPermission = false;
        bool deleteRestaurantPermission = false;


        public OnlineMealRestaurant()
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
                    case OnlineMealRestaurant.RestaurantIDIndex:
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
                    case OnlineMealRestaurant.NameIndex:
                    case OnlineMealRestaurant.DescriptionIndex:
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
                btnRestaurantAdd.Text = rm.GetString("btnAdd", culture);
                btnRestaurantUpdate.Text = rm.GetString("btnUpdate", culture);
                btnRestaurantDelete.Text = rm.GetString("btnDelete", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClear.Text = rm.GetString("btnClear", culture);

                // groupbox text
                gbSearch.Text = rm.GetString("gbSearch", culture);

                //label's text
                this.lblRestaurantID.Text = rm.GetString("lblRestaurantID", culture);
                this.lblDescription.Text = rm.GetString("lblDescription", culture);
                this.lblName.Text = rm.GetString("lblName", culture);

                // list view initialization
                lvMealTypes.BeginUpdate();
                lvMealTypes.Columns.Add(rm.GetString("hdrRestaurantID", culture), (lvMealTypes.Width - 5) / 3 - 4, HorizontalAlignment.Right);
                lvMealTypes.Columns.Add(rm.GetString("hdrName", culture), (lvMealTypes.Width - 5) / 3 - 4, HorizontalAlignment.Left);
                lvMealTypes.Columns.Add(rm.GetString("hdrDescription", culture), (lvMealTypes.Width - 5) / 3 - 4, HorizontalAlignment.Left);

                lvMealTypes.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsRestaurant.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public void populateListView(List<OnlineMealsRestaurantTO> mealTypesList)
        {
            try
            {
                lvMealTypes.BeginUpdate();
                lvMealTypes.Items.Clear();

                if (mealTypesList.Count > 0)
                {
                    for (int i = 0; i < mealTypesList.Count; i++)
                    {
                        OnlineMealsRestaurantTO mealType = (OnlineMealsRestaurantTO)mealTypesList[i];
                        ListViewItem item = new ListViewItem();

                        item.Text = mealType.RestaurantID.ToString().Trim();
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
                log.writeLog(DateTime.Now + " OnlineMealsRestaurant.populateListView(): " + ex.Message + "\n");
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
                    tbRestaurantID.Text = lvMealTypes.SelectedItems[0].Text.Trim();
                    currentRestaurantTO.RestaurantID = Int32.Parse(tbRestaurantID.Text);
                    currentRestaurantTO.Name = tbName.Text = lvMealTypes.SelectedItems[0].SubItems[1].Text.Trim();
                    currentRestaurantTO.Description = tbDescription.Text = lvMealTypes.SelectedItems[0].SubItems[2].Text.Trim();
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now + " OnlinrMealRestaurant.lvMealTypes_SelectedIndexChanged(): " + ex.Message + "\n");
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                tbRestaurantID.Text = "";
                tbDescription.Text = "";
                tbName.Text = "";

                clearCurrentMealType();

                lvMealTypes.Items.Clear();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsRestaurant.btnClear_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                currentRestaurantTO = new OnlineMealsRestaurantTO();
                // validate
                if (!tbRestaurantID.Text.Equals(""))
                {
                    int id;
                    bool idIsInt = Int32.TryParse(tbRestaurantID.Text.ToString().Trim(), out id);

                    if (!idIsInt)
                    {
                        MessageBox.Show(rm.GetString("restaurantIDNotInt", culture));
                        return;
                    }
                    else
                    {
                        currentRestaurantTO.RestaurantID = id;
                    }
                }

                if (!tbName.Text.Equals(""))
                {
                    currentRestaurantTO.Name = tbName.Text.ToString().Trim();
                }

                if (!tbDescription.Text.Equals(""))
                {
                    currentRestaurantTO.Description = tbDescription.Text.ToString().Trim();
                }

                OnlineMealsRestaurant onlineMealTypes = new OnlineMealsRestaurant();
                onlineMealTypes.OnlineMealsRestaurant1 = currentRestaurantTO;
                currentRestaurantList = onlineMealTypes.Search();

                if (currentRestaurantList.Count > 0)
                {
                    populateListView(currentRestaurantList);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noRestaurantsFound", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsRestaurant.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
               
                OnlineMealRestaurantAdd mta = new OnlineMealRestaurantAdd();
                mta.ShowDialog();
                if (mta.reloadOnReturn)
                {
                    btnClear.PerformClick();
                    currentRestaurantList = new OnlineMealsRestaurant().Search();
                    populateListView(currentRestaurantList);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsRestaurant.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvMealTypes.SelectedItems.Count > 0)
                {
                    OnlineMealRestaurantAdd mta = new OnlineMealRestaurantAdd(currentRestaurantTO);
                    mta.ShowDialog();
                    if (mta.reloadOnReturn)
                    {
                        btnClear.PerformClick();
                        currentRestaurantList = new OnlineMealsRestaurant().Search();
                        populateListView(currentRestaurantList);
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noRestaurantUpd", culture));
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsRestaurant.btnUpdate_Click(): " + ex.Message + "\n");
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
                    DialogResult result = MessageBox.Show(rm.GetString("messageDeleteRestaurant", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        int id = -1;
                        foreach (ListViewItem item in lvMealTypes.SelectedItems)
                        {
                            if (Int32.TryParse(item.Text.Trim(), out id))
                            {
                                isDeleted = new OnlineMealsRestaurant().Delete(id) && isDeleted;
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
                            MessageBox.Show(rm.GetString("restaurantsDeleted", culture));
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("restaurantsNotDeleted", culture));
                        }

                        btnClear.PerformClick();
                        currentRestaurantList = new OnlineMealsRestaurant().Search();
                        populateListView(currentRestaurantList);
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("selectRestaurant", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsRestaurant.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(rm.GetString("restaurantsNotDeleted", culture));
            }
        }
        private void setVisibility()
        {
            try
            {

                int permissionRestaurant;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permissionRestaurant = (((int[])menuItemsPermissions[menuItemRestaurantID])[role.ApplRoleID]);

                    addRestaurantPermission = addRestaurantPermission || (((permissionRestaurant / 4) % 2) == 0 ? false : true);
                    updateRestaurantPermission = updateRestaurantPermission || (((permissionRestaurant / 2) % 2) == 0 ? false : true);
                    deleteRestaurantPermission = deleteRestaurantPermission || ((permissionRestaurant % 2) == 0 ? false : true);
                }

                btnRestaurantAdd.Enabled = addRestaurantPermission;
                btnRestaurantUpdate.Enabled = updateRestaurantPermission;
                btnRestaurantDelete.Enabled = deleteRestaurantPermission;


            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsRestaurant.setVisibility(): " + ex.Message + "\n");
                throw ex;
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
                currentRestaurantTO.RestaurantID = -1;
                currentRestaurantTO.Description = "";
                currentRestaurantTO.Name = "";

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();

                menuItemID = NotificationController.GetCurrentMenuItemID();
                int index = menuItemID.LastIndexOf('_');

                menuItemRestaurantID = menuItemID + "_" + rm.GetString("tpRestaurants", culture);
                setVisibility();
             
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " OnlineMealsRestaurant.MealTypes_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvMealTypes_ColumnClick(object sender, ColumnClickEventArgs e)
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
                log.writeLog(DateTime.Now + " OnlineMealRestaurant.lvMealTypes_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void tbMealTypeID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int id;
                if (!tbRestaurantID.Text.Equals(""))
                {
                    bool idIsInt = Int32.TryParse(tbRestaurantID.Text.ToString().Trim(), out id);

                    if (!idIsInt)
                    {
                        MessageBox.Show(rm.GetString("restaurantIDNotInt", culture));
                        if (!tbRestaurantID.Text.Equals(""))
                        {
                            tbRestaurantID.Text = tbRestaurantID.Text.Substring(0, tbRestaurantID.Text.Length - 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealRestaurant.tbMealTypeID_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void clearCurrentMealType()
        {
            try
            {
                currentRestaurantTO = new OnlineMealsRestaurantTO();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealTypes.tbMealTypeID_TextChanged(): " + ex.Message + "\n");
                throw ex;
            }
        }


    }
}
