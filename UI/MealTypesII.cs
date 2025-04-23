using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Resources;
using System.Globalization;

using TransferObjects;
using UI;
using Common;
using Util;
using System.Collections;

namespace UI
{
    public partial class MealTypesII : UserControl
    {

        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        MealType currentMealType;
        ArrayList currentMealTypeList;
        private ListViewItemComparer _comp;

        // List View indexes
        const int MealTypeIDIndex = 0;
        const int NameIndex = 1;
        const int DescriptionIndex = 2;

        public MealTypesII()
        {
            try
            {
                InitializeComponent();
                if (!this.DesignMode)
                {
                    string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                    log = new DebugLog(logFilePath);
                    logInUser = NotificationController.GetLogInUser();
                    rm = new ResourceManager("UI.Resource", typeof(MealTypesII).Assembly);
                    culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                    setLanguage();
                    currentMealType = new MealType();
                }
            }
            catch
            {}
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("mealTypes", culture);
                // button's text
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
               
                // list view initialization
                lvMealTypes.BeginUpdate();
                lvMealTypes.Columns.Add(rm.GetString("hdrMealTypeID", culture), (lvMealTypes.Width - 5) / 3 - 6, HorizontalAlignment.Right);
                lvMealTypes.Columns.Add(rm.GetString("hdrName", culture), (lvMealTypes.Width - 5) / 3 - 6, HorizontalAlignment.Left);
                lvMealTypes.Columns.Add(rm.GetString("hdrDescription", culture), (lvMealTypes.Width - 5) / 3 - 6, HorizontalAlignment.Left);
                lvMealTypes.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesII.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateListView(ArrayList mealTypesList)
        {
            try
            {
                lvMealTypes.BeginUpdate();
                lvMealTypes.Items.Clear();

                if (mealTypesList.Count > 0)
                {
                    for (int i = 0; i < mealTypesList.Count; i++)
                    {
                        MealType mealType = (MealType)mealTypesList[i];
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
                log.writeLog(DateTime.Now + " MealTypesII.populateListView(): " + ex.Message + "\n");
                throw ex;
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
                    case MealTypesII.MealTypeIDIndex:
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
                    case MealTypesII.NameIndex:
                    case MealTypesII.DescriptionIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");
                }
            }
        }

        #endregion

        private void MealTypesII_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.DesignMode)
                {
                    this.Cursor = Cursors.WaitCursor;
                    // Initialize comparer object
                    _comp = new ListViewItemComparer(lvMealTypes);
                    lvMealTypes.ListViewItemSorter = _comp;
                    lvMealTypes.Sorting = SortOrder.Ascending;

                    currentMealTypeList = currentMealType.Search(-1, "", "", new DateTime(), new DateTime());
                    populateListView(currentMealTypeList);
                    this.Cursor = Cursors.Arrow;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesII.MealTypesII_Load(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool isDeleted = true;
                bool hasSchedule = false;
                MealTypeAdditionalData additionalData = new MealTypeAdditionalData();
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
                                ArrayList mealTypeScheduleList = new MealsTypeSchedule().Search(id, new DateTime(), new DateTime());
                                if(mealTypeScheduleList.Count == 0)
                                {
                                    hasSchedule = false;
                                    if (additionalData.DoesTableExists() == 1 && additionalData.GetAdditionalData(id).MealTypeID != -1 && additionalData.GetAdditionalData(id).MealTypeID != -0)
                                    {

                                        MealTypeAdditionalData mealTypeAddData = new MealTypeAdditionalData();
                                        bool trans = mealTypeAddData.BeginTransaction();
                                        bool succ = trans;

                                        if (trans)
                                        {
                                            try
                                            {
                                                isDeleted = mealTypeAddData.Delete(id, false) && isDeleted;
                                                succ = succ && isDeleted;
                                                if (succ)
                                                {
                                                    MealType mealType = new MealType();
                                                    mealType.SetTransaction(mealTypeAddData.GetTransaction());
                                                    succ = succ && mealType.Delete(id, false);
                                                    if (succ)
                                                    {
                                                        mealTypeAddData.CommitTransaction();
                                                        isDeleted = true;
                                                    }
                                                    else
                                                    {
                                                        mealTypeAddData.RollbackTransaction();
                                                        isDeleted = false;
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                if (mealTypeAddData.GetTransaction() != null)
                                                {
                                                    mealTypeAddData.RollbackTransaction();
                                                    isDeleted = false;
                                                }
                                            }
                                        }
                                    }
                                    else
                                        isDeleted = currentMealType.Delete(id) && isDeleted;

                                    if (!isDeleted)
                                        break;
                                }
                                else
                                {
                                    hasSchedule = true;
                                    break;
                                }                                    
                            }
                            else
                            {
                                isDeleted = false;
                                hasSchedule = false;
                                break;
                            }
                        }

                        if(hasSchedule)
                        {
                            MessageBox.Show(rm.GetString("mealTypeInSchedule", culture));
                        }
                        else if (isDeleted)
                        {
                            MessageBox.Show(rm.GetString("mealTypesDeleted", culture));
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("mealTypesNotDeleted", culture));
                        }

                        currentMealTypeList = currentMealType.Search(-1, "", "", new DateTime(), new DateTime());
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
                log.writeLog(DateTime.Now + " MealTypesII.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvMealTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lvMealTypes.SelectedItems.Count > 0)
                {
                    currentMealType.MealTypeID = Int32.Parse(lvMealTypes.SelectedItems[0].Text.Trim());
                    currentMealType.Name = lvMealTypes.SelectedItems[0].SubItems[1].Text.Trim();
                    currentMealType.Description = lvMealTypes.SelectedItems[0].SubItems[2].Text.Trim();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesII.lvMealTypes_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MealTypesII.lvMealTypes_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                MealTypesAddII add = new MealTypesAddII();
                add.ShowDialog();
                if (add.reloadOnReturn)
                {
                    currentMealTypeList = currentMealType.Search(-1, "", "", new DateTime(), new DateTime());
                    populateListView(currentMealTypeList);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesII.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvMealTypes.SelectedItems.Count > 0)
                {
                    MealTypesAddII mta = new MealTypesAddII(currentMealType);
                    mta.ShowDialog();
                    if (mta.reloadOnReturn)
                    {
                        currentMealTypeList = currentMealType.Search(-1, "", "", new DateTime(), new DateTime());
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
                log.writeLog(DateTime.Now + " MealTypesII.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}