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
    public partial class OnlineMealPoint : UserControl
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        OnlineMealsPointsTO currentMealPoint;
        List<OnlineMealsPointsTO> currentMealPointsList;
        private ListViewItemComparer _comp;

        // List View indexes
        const int IDIndex = 0;
        const int PointName = 1;
        const int PointDec = 2;
        const int Restaurant = 3;
        const int MealType = 4;
        const int IPAddress = 5;
        const int Antenna = 6;
        const int Peripherial=7;
        const int PeripherialDesc = 8;

        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;

        string menuItemID;
        string menuItemLinesID;

        bool readLinesPermission = false;
        bool addLinesPermission = false;
        bool updateLinesPermission = false;
        bool deleteLinesPermission = false;

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
                    case OnlineMealPoint.IDIndex:
                    case OnlineMealPoint.Antenna:
                    case OnlineMealPoint.Peripherial:
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
                    case OnlineMealPoint.PointName:
                    case OnlineMealPoint.PointDec:
                    case OnlineMealPoint.Restaurant:
                    case OnlineMealPoint.MealType:
                    case OnlineMealPoint.IPAddress:
                    case OnlineMealPoint.PeripherialDesc:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");
                }
            }
        }

        #endregion

        public OnlineMealPoint()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                rm = new ResourceManager("UI.Resource", typeof(OnlineMealPoint).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();
                populateMealTypeCombo();
                populateRestaurantCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setVisibility()
        {
            try
            {

                int permissionLines;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permissionLines = (((int[])menuItemsPermissions[menuItemLinesID])[role.ApplRoleID]);

                    addLinesPermission = addLinesPermission || (((permissionLines / 4) % 2) == 0 ? false : true);
                    updateLinesPermission = updateLinesPermission || (((permissionLines / 2) % 2) == 0 ? false : true);
                    deleteLinesPermission = deleteLinesPermission || ((permissionLines % 2) == 0 ? false : true);

                }

                btnLineAdd.Enabled = addLinesPermission;
                btnLineUpdate.Enabled = updateLinesPermission;
                btnLineDelete.Enabled = deleteLinesPermission;


            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealPoint.setVisibility(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void setLanguage()
        {
            try
            {
                // button's text
                btnLineAdd.Text = rm.GetString("btnAdd", culture);
                btnLineUpdate.Text = rm.GetString("btnUpdate", culture);
                btnLineDelete.Text = rm.GetString("btnDelete", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClear.Text = rm.GetString("btnClear", culture);

                // groupbox text
                this.gbSearch.Text = rm.GetString("gbSearch", culture);

                //label's text
                this.lblPointID.Text = rm.GetString("lblLineID", culture);
                this.lblDescription.Text = rm.GetString("lblDescription", culture);
                this.lblName.Text = rm.GetString("lblName", culture);
                this.lblTerminalSerial.Text = rm.GetString("hdrRestaurant", culture);
                this.lblAntenna.Text = rm.GetString("lblReaderAnt", culture);
                this.lblIPAddress.Text = rm.GetString("lblIpAddress", culture);
                this.lblPeripherialDesc.Text = rm.GetString("lblReaderPeripherialDesc", culture);
                this.lblPreipherial.Text = rm.GetString("lblReaderPeripherial", culture);
                this.lblMealType.Text = rm.GetString("lblMealsType", culture);



                // list view initialization
                lvMealPoints.BeginUpdate();
                lvMealPoints.Columns.Add(rm.GetString("hdrMealPointID", culture), (lvMealPoints.Width - 4) / 9 - 2, HorizontalAlignment.Right);
                lvMealPoints.Columns.Add(rm.GetString("hdrName", culture), (lvMealPoints.Width - 4) / 9 - 2, HorizontalAlignment.Left);
                lvMealPoints.Columns.Add(rm.GetString("hdrDescription", culture), (lvMealPoints.Width - 4) / 9 - 2, HorizontalAlignment.Left);
                lvMealPoints.Columns.Add(rm.GetString("hdrRestaurant", culture), (lvMealPoints.Width - 4) / 9 - 2, HorizontalAlignment.Left);
                lvMealPoints.Columns.Add(rm.GetString("hdrMealType", culture), (lvMealPoints.Width - 4) / 9 - 2, HorizontalAlignment.Left);
                lvMealPoints.Columns.Add(rm.GetString("hdrIpAddress", culture), (lvMealPoints.Width - 4) / 9 + 2, HorizontalAlignment.Left);
                lvMealPoints.Columns.Add(rm.GetString("hdrReaderAnt", culture), (lvMealPoints.Width - 4) / 9 + 2, HorizontalAlignment.Left);
                lvMealPoints.Columns.Add(rm.GetString("hdrReaderPeripherial", culture), (lvMealPoints.Width - 4) / 9 - 2, HorizontalAlignment.Left);
                lvMealPoints.Columns.Add(rm.GetString("hdrReaderPeripherialDesc", culture), (lvMealPoints.Width - 4) / 9, HorizontalAlignment.Left);
                lvMealPoints.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealPoint.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public void populateListView(List<OnlineMealsPointsTO> mealTypeEmplList)
        {
            try
            {
                lvMealPoints.BeginUpdate();
                lvMealPoints.Items.Clear();

                if (mealTypeEmplList.Count > 0)
                {
                    for (int i = 0; i < mealTypeEmplList.Count; i++)
                    {
                        OnlineMealsPointsTO mealTypeEmpl = (OnlineMealsPointsTO)mealTypeEmplList[i];
                        ListViewItem item = new ListViewItem();

                        item.Text = mealTypeEmpl.PointID.ToString().Trim();
                        item.SubItems.Add(mealTypeEmpl.Name.Trim());
                        item.SubItems.Add(mealTypeEmpl.Description.Trim());
                        item.SubItems.Add(mealTypeEmpl.RestaurantName.ToString().Trim());
                        item.SubItems.Add(mealTypeEmpl.MealType.ToString().Trim());
                        item.SubItems.Add(mealTypeEmpl.ReaderIPAddress.Trim());
                        item.SubItems.Add(mealTypeEmpl.Reader_ant.ToString().Trim());
                        item.SubItems.Add(mealTypeEmpl.Reader_peripherial.ToString().Trim());
                        item.SubItems.Add(mealTypeEmpl.ReaderPeripherialDesc.Trim());

                        lvMealPoints.Items.Add(item);
                    }

                }

                lvMealPoints.EndUpdate();
                lvMealPoints.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealPoint.populateListView(): " + ex.Message + "\n");
                throw ex;
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
                log.writeLog(DateTime.Now + " OnlineMealPoint.populateEmployeeCombo(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " OnlineMealPoint.populateRestaurantCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void lvMealPoints_SelectedIndexChanged(object sender, System.EventArgs e)
        {
           
            if (lvMealPoints.SelectedItems.Count == 1)
            {
                try
                {
                    currentMealPoint = new OnlineMealsPointsTO();
                    // populate MealType's search form
                    tbMealPointID.Text = lvMealPoints.SelectedItems[0].Text.Trim();
                    currentMealPoint.PointID = Int32.Parse(tbMealPointID.Text);
                   
                    cbRestaurant.SelectedIndex = cbRestaurant.FindStringExact(lvMealPoints.SelectedItems[0].SubItems[3].Text);
                    currentMealPoint.RestaurantID = (int)cbRestaurant.SelectedValue;
                    currentMealPoint.Name = tbName.Text = lvMealPoints.SelectedItems[0].SubItems[1].Text.Trim();
                    currentMealPoint.Description = tbDescription.Text = lvMealPoints.SelectedItems[0].SubItems[2].Text.Trim();
                 
                    cbMealType.SelectedIndex = cbMealType.FindStringExact(lvMealPoints.SelectedItems[0].SubItems[4].Text);
                    currentMealPoint.MealTypeID = (int)cbMealType.SelectedValue;
                   
                    currentMealPoint.ReaderIPAddress = tbIPAddress.Text = lvMealPoints.SelectedItems[0].SubItems[5].Text.Trim();
                    tbAntenna.Text = lvMealPoints.SelectedItems[0].SubItems[6].Text.Trim();
                    currentMealPoint.Reader_ant = Int32.Parse(tbAntenna.Text);
                    tbPeripherial.Text = lvMealPoints.SelectedItems[0].SubItems[7].Text.Trim();
                    currentMealPoint.Reader_peripherial = Int32.Parse(tbPeripherial.Text);
                    currentMealPoint.ReaderPeripherialDesc = tbPeripherialDesc.Text = lvMealPoints.SelectedItems[0].SubItems[8].Text.Trim();
                    currentMealPoint.RestaurantName = ((OnlineMealsRestaurantTO)cbRestaurant.SelectedItem).Name;
                    currentMealPoint.MealType = ((OnlineMealsTypesTO)cbMealType.SelectedItem).Name;
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now + " OnlineMealPoint.lvMealPoints_SelectedIndexChanged(): " + ex.Message + "\n");
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                tbMealPointID.Text = "";
                tbDescription.Text = "";
                tbName.Text = "";
                tbPeripherialDesc.Text = "";
                tbPeripherial.Text = "";
                tbIPAddress.Text = "";
                tbAntenna.Text = "";
                cbMealType.SelectedIndex = 0;
                cbRestaurant.SelectedIndex = 0;


                currentMealPoint = new OnlineMealsPointsTO();
                lvMealPoints.Items.Clear();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealPoint.btnClear_Click(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                currentMealPoint = new OnlineMealsPointsTO();

                if (!tbMealPointID.Text.Equals(""))
                {
                    int id;
                    bool idIsInt = Int32.TryParse(tbMealPointID.Text.ToString().Trim(), out id);

                    if (!idIsInt)
                    {
                        MessageBox.Show(rm.GetString("MealPointIDNotInt", culture));
                        return;
                    }
                    else
                    {
                        currentMealPoint.PointID = id;
                    }
                }

                if (cbRestaurant.SelectedIndex > 0)
                {

                    currentMealPoint.RestaurantID = (int)cbRestaurant.SelectedValue;


                }
                if (!tbName.Text.Equals(""))
                {
                    currentMealPoint.Name = tbName.Text.ToString().Trim();
                }
                if (!tbDescription.Text.Equals(""))
                {
                    currentMealPoint.Description = tbDescription.Text.ToString().Trim();
                }
                if (!tbIPAddress.Text.Equals(""))
                {

                    currentMealPoint.ReaderIPAddress = tbIPAddress.Text;
                }
                if (!tbAntenna.Text.Equals(""))
                {
                    int id;
                    bool idIsInt = Int32.TryParse(tbAntenna.Text.ToString().Trim(), out id);

                    if (!idIsInt)
                    {
                        MessageBox.Show(rm.GetString("antennaNotInt", culture));
                        return;
                    }
                    else
                    {
                        currentMealPoint.Reader_ant = id;
                    }


                }
                if (!tbPeripherial.Text.Equals(""))
                {

                    int id;
                    bool idIsInt = Int32.TryParse(tbPeripherial.Text.ToString().Trim(), out id);

                    if (!idIsInt)
                    {
                        MessageBox.Show(rm.GetString("MealPointIDNotInt", culture));
                        return;
                    }
                    else
                    {
                        currentMealPoint.Reader_peripherial = id;
                    }

                }
                if (!tbPeripherialDesc.Text.Equals(""))
                {

                    currentMealPoint.ReaderPeripherialDesc = tbPeripherialDesc.Text;
                }
                if (cbMealType.SelectedIndex > 0)
                {



                    currentMealPoint.MealTypeID =(int) cbMealType.SelectedValue;
                    

                }

                OnlineMealsPoints onlineMealPoint = new OnlineMealsPoints();
                onlineMealPoint.OnlineMealsPointTO = currentMealPoint;
                currentMealPointsList = onlineMealPoint.Search();

                if (currentMealPointsList.Count > 0)
                {
                    populateListView(currentMealPointsList);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noMealPointsFound", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealPoint.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                OnlineMealPointAdd mpa = new OnlineMealPointAdd();
                mpa.ShowDialog();
                if (mpa.reloadOnReturn)
                {
                    btnClear.PerformClick();
                    currentMealPointsList = new OnlineMealsPoints().Search();
                    populateListView(currentMealPointsList);
                    populateMealTypeCombo();
                    populateRestaurantCombo();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealPoint.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvMealPoints.SelectedItems.Count > 0)
                {
                    OnlineMealPointAdd mpa = new OnlineMealPointAdd(currentMealPoint);
                    mpa.ShowDialog();
                    if (mpa.reloadOnReturn)
                    {
                        btnClear.PerformClick();
                        currentMealPointsList = new OnlineMealsPoints().Search();
                        populateListView(currentMealPointsList);
                        populateMealTypeCombo();
                        populateRestaurantCombo();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noMealPointUpd", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealPoint.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool isDeleted = true;

                if (lvMealPoints.SelectedItems.Count > 0)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("messageDeleteMealPoint", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        foreach (ListViewItem item in lvMealPoints.SelectedItems)
                        {
                            isDeleted = new OnlineMealsPoints().Delete(Int32.Parse(item.Text.Trim())) && isDeleted;
                            if (!isDeleted)
                                break;
                        }

                        if (isDeleted)
                        {
                            MessageBox.Show(rm.GetString("mealPointsDeleted", culture));
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("mealPointsNotDeleted", culture));
                        }

                        btnClear.PerformClick();
                        currentMealPointsList = new OnlineMealsPoints().Search();
                        populateListView(currentMealPointsList);
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("SelectMealPoint", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealPoint.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void MealPoints_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                _comp = new ListViewItemComparer(lvMealPoints);
                lvMealPoints.ListViewItemSorter = _comp;
                lvMealPoints.Sorting = SortOrder.Ascending;

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();

                menuItemID = NotificationController.GetCurrentMenuItemID();
                int index = menuItemID.LastIndexOf('_');

                menuItemLinesID = menuItemID + "_" + rm.GetString("tpLines", culture);
                setVisibility();
               
               

                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " OnlineMealPoint.MealPoints_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

     

        private void tbMealPointID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int id;
                if (!tbMealPointID.Text.Equals(""))
                {
                    bool idIsInt = Int32.TryParse(tbMealPointID.Text.ToString().Trim(), out id);

                    if (!idIsInt)
                    {
                        MessageBox.Show(rm.GetString("MealPointIDNotInt", culture));
                        if (!tbMealPointID.Text.Equals(""))
                        {
                            tbMealPointID.Text = tbMealPointID.Text.Substring(0, tbMealPointID.Text.Length - 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealPoint.tbMealPointID_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvMealPoints_ColumnClick_1(object sender, ColumnClickEventArgs e)
        {
            try
            {
                SortOrder prevOrder = lvMealPoints.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvMealPoints.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvMealPoints.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvMealPoints.Sorting = SortOrder.Ascending;
                }

                lvMealPoints.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealPoint.lvMealPoints_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }


    }
}
