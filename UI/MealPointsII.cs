using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
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
    public partial class MealPointsII : UserControl
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        MealPoint currentMealPoint;
        ArrayList currentMealPointList;
        private ListViewItemComparer _comp;

        // List View indexes
        const int TerminalIndex = 2;
        const int NameIndex = 0;
        const int DescIndex = 1;


        public MealPointsII()
        {
            InitializeComponent();
            if (!this.DesignMode)
            {
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                rm = new ResourceManager("UI.Resource", typeof(MealTypesII).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            }
        }

        private void setLanguage()
        {
            try
            {
                // button's text
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);

                // list view initialization
                lvMealPoints.BeginUpdate();
                lvMealPoints.Columns.Add(rm.GetString("hdrName", culture), (lvMealPoints.Width - 5) / 3 - 6, HorizontalAlignment.Right);
                lvMealPoints.Columns.Add(rm.GetString("hdrDescription", culture), (lvMealPoints.Width - 5) / 3 - 6, HorizontalAlignment.Left);
                lvMealPoints.Columns.Add(rm.GetString("hdrSerialNumber", culture), (lvMealPoints.Width - 5) / 3 - 6, HorizontalAlignment.Left);
                lvMealPoints.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPointsII.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void MealPointsII_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                currentMealPoint = new MealPoint();
                setLanguage();

                _comp =new ListViewItemComparer(lvMealPoints);
                lvMealPoints.ListViewItemSorter = _comp;
                lvMealPoints.Sorting = SortOrder.Ascending;

                currentMealPointList = currentMealPoint.Search(-1, "", "", "");
                populateListView(currentMealPointList);
            }
        }

        public void populateListView(ArrayList mealPointsList)
        {
            try
            {
               
                lvMealPoints.BeginUpdate();
                lvMealPoints.Items.Clear();

                if (mealPointsList.Count > 0)
                {
                    for (int i = 0; i < mealPointsList.Count; i++)
                    {
                        MealPoint mealPoint = (MealPoint)mealPointsList[i];
                        ListViewItem item = new ListViewItem();

                        item.Text = mealPoint.Name.Trim();
                        item.SubItems.Add(mealPoint.Description.Trim());
                        item.SubItems.Add(mealPoint.TerminalSerial.ToString().Trim());
                        item.Tag = mealPoint.MealPointID;

                        lvMealPoints.Items.Add(item);
                    }
                }

                lvMealPoints.EndUpdate();
                lvMealPoints.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPointsII.populateListView(): " + ex.Message + "\n");
                throw ex;
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
                        int id = -1;
                        foreach (ListViewItem item in lvMealPoints.SelectedItems)
                        {
                            if (Int32.TryParse(item.Tag.ToString().Trim(), out id))
                            {
                                isDeleted = currentMealPoint.Delete(id.ToString()) && isDeleted;
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
                            MessageBox.Show(rm.GetString("mealPointsDeleted", culture));
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("mealPointsNotDeleted", culture));
                        }

                        currentMealPointList = currentMealPoint.Search(-1, "", "","");
                        populateListView(currentMealPointList);
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("SelectMealPoint", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPointsII.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                MealPointAddII mpa = new MealPointAddII();
                mpa.ShowDialog();
                if (mpa.reloadOnReturn)
                {                    
                    currentMealPointList = currentMealPoint.Search(-1, "", "", "");
                    populateListView(currentMealPointList);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPointsII.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvMealPoints.SelectedItems.Count > 0)
                {
                    MealPointAddII mpa = new MealPointAddII(currentMealPoint);
                    mpa.ShowDialog();
                    if (mpa.reloadOnReturn)
                    {
                        currentMealPointList = currentMealPoint.Search(-1, "", "", "");
                        populateListView(currentMealPointList);
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noMealPointUpd", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPointsII.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvMealPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lvMealPoints.SelectedItems.Count == 1)
                {
                    try
                    {
                        // populate MealType's search form
                        currentMealPoint.MealPointID = Int32.Parse(lvMealPoints.SelectedItems[0].Tag.ToString().Trim());
                        currentMealPoint.TerminalSerial = lvMealPoints.SelectedItems[0].SubItems[2].Text.Trim();
                        currentMealPoint.Name = lvMealPoints.SelectedItems[0].SubItems[0].Text.Trim();
                        currentMealPoint.Description = lvMealPoints.SelectedItems[0].SubItems[1].Text.Trim();
                    }
                    catch (Exception ex)
                    {
                        log.writeLog(DateTime.Now + " MealPoints.lvMealPoints_SelectedIndexChanged(): " + ex.Message + "\n");
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPointsII.lvMealPoints_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvMealPoints_ColumnClick(object sender, ColumnClickEventArgs e)
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
                log.writeLog(DateTime.Now + " MealPoints.lvMealPoints_ColumnClick(): " + ex.Message + "\n");
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
                    case MealPointsII.TerminalIndex:
                    case MealPointsII.NameIndex:
                    case MealPointsII.DescIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");
                }
            }
        }

        #endregion

    }
}
