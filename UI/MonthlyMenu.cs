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
    public partial class MonthlyMenu : Form
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        DateTime currentMonth;
        DateTime firstDayOfMonth;
        DateTime lastDayOfMonth;
        MealsTypeScheduleTO currentSchedule;
        ArrayList currentMealDayList;
        private ListViewItemComparer _comp;

        // List View indexes
        const int DateIndex = 0;
        const int MealTypeIDIndex = 1;
        const int MealTypeNameIndex = 2;



        public MonthlyMenu()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                rm = new ResourceManager("UI.Resource", typeof(MealTypesAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                setLanguage();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MonthlyMenu.MonthlyMenu(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        public MonthlyMenu(DateTime month)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                rm = new ResourceManager("UI.Resource", typeof(MealTypesAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());


                currentMonth = month;
                currentSchedule = new MealsTypeScheduleTO();
                setLanguage();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MonthlyMenu.MonthlyMenu(): " + ex.Message + "\n");
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
                this.Text = rm.GetString("frmMonthlyMenu", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
                //gbSchedule.Text = rm.GetString("gbSchedule", culture);
                btnExit.Text = rm.GetString("btnExit", culture);

                // list view
                lvMonthlyMenu.BeginUpdate();
                lvMonthlyMenu.Columns.Add(rm.GetString("lblDate", culture), (lvMonthlyMenu.Width - 4) / 3, HorizontalAlignment.Left);
                lvMonthlyMenu.Columns.Add(rm.GetString("lblMealTypeID", culture), (lvMonthlyMenu.Width - 4) / 3, HorizontalAlignment.Left);
                lvMonthlyMenu.Columns.Add(rm.GetString("lblMealType", culture), (lvMonthlyMenu.Width - 4) / 3, HorizontalAlignment.Left);
                lvMonthlyMenu.EndUpdate();

                gbSchedule.Text = rm.GetString("month", culture) + ": " + currentMonth.ToString("MMMM yyyy");
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsMenu.btnExit_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);

            }
        }

        public void populateListView(ArrayList mealTypesList)
        {
            try
            {
                lvMonthlyMenu.BeginUpdate();
                lvMonthlyMenu.Items.Clear();

                if (mealTypesList.Count > 0)
                {
                    for (int i = 0; i < mealTypesList.Count; i++)
                    {
                        MealsTypeSchedule mealTypeSchedule = (MealsTypeSchedule)mealTypesList[i];
                        ListViewItem item = new ListViewItem();

                        item.Text = mealTypeSchedule.Date.ToShortDateString();
                        item.SubItems.Add(mealTypeSchedule.MealTypeID.ToString());
                        item.SubItems.Add(mealTypeSchedule.MealType);

                        lvMonthlyMenu.Items.Add(item);
                    }
                }

                lvMonthlyMenu.EndUpdate();
                lvMonthlyMenu.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsMenu.populateListView(): " + ex.Message + "\n");
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
                 //Handle non Detail Cases
                return CompareItems(item1, item2);
            }

            public int CompareItems(ListViewItem item1, ListViewItem item2)
            {
            //     Subitem instances
                ListViewItem.ListViewSubItem sub1 = item1.SubItems[SortColumn];
                ListViewItem.ListViewSubItem sub2 = item2.SubItems[SortColumn];

              //   Return value based on sort column	
                switch (SortColumn)
                {
                    case MonthlyMenu.DateIndex:
                    {
                        DateTime firstID = new DateTime();
                        DateTime secondID = new DateTime();

                    if (!sub1.Text.Trim().Equals(""))
                    {
                        firstID = DateTime.Parse(sub1.Text.Trim());
                    }

                    if (!sub2.Text.ToString().Trim().Equals(""))
                    {
                        secondID = DateTime.Parse(sub2.Text.Trim());
                    }

                    return CaseInsensitiveComparer.Default.Compare(firstID, secondID);
                    }
                    case MonthlyMenu.MealTypeIDIndex:
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
                    case MonthlyMenu.MealTypeNameIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");
                }
            }
        }

        #endregion

        private void MonthlyMenu_Load(object sender, EventArgs e)
        {
            try
            {
                _comp = new ListViewItemComparer(lvMonthlyMenu);
                lvMonthlyMenu.ListViewItemSorter = _comp;
                lvMonthlyMenu.Sorting = SortOrder.Ascending;

                firstDayOfMonth= new DateTime(currentMonth.Year, currentMonth.Month,1 , 0, 0, 0);
                lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                ArrayList mealsMenu = new MealsTypeSchedule().Search(-1, firstDayOfMonth, lastDayOfMonth);
                if (mealsMenu.Count > 0)
                {
                    populateListView(mealsMenu);
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.MonthlyMenu_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvMonthlyMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lvMonthlyMenu.SelectedItems.Count > 0)
                {
                    currentSchedule.Date = DateTime.Parse(lvMonthlyMenu.SelectedItems[0].Text.Trim());
                    currentSchedule.MealTypeID = Int32.Parse(lvMonthlyMenu.SelectedItems[0].SubItems[1].Text.Trim());
                    currentSchedule.MealType = lvMonthlyMenu.SelectedItems[0].SubItems[2].Text.Trim();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MonthlyMenu.lvMonthlyMenu_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        //private void lvMonthlyMenu_ColumnClick(object sender, ColumnClickEventArgs e)
        //{
        //    try
        //    {
        //        SortOrder prevOrder = lvMonthlyMenu.Sorting;

        //        if (e.Column == _comp.SortColumn)
        //        {
        //            if (prevOrder == SortOrder.Ascending)
        //            {
        //                lvMonthlyMenu.Sorting = SortOrder.Descending;
        //            }
        //            else
        //            {
        //                lvMonthlyMenu.Sorting = SortOrder.Ascending;
        //            }
        //        }
        //        else
        //        {
        //            // New Sort Order
        //            _comp.SortColumn = e.Column;
        //            lvMonthlyMenu.Sorting = SortOrder.Ascending;
        //        }

        //        lvMonthlyMenu.Sort();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.writeLog(DateTime.Now + " MealsMenu.lvMonthlyMenu_ColumnClick(): " + ex.Message + "\n");
        //        MessageBox.Show(ex.Message);
        //    }

        //}

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool isDeleted = true;
                MealTypeAdditionalData additionalData = new MealTypeAdditionalData();
                if (lvMonthlyMenu.SelectedItems.Count > 0)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("messageDeleteMealTypeSchedule", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        DateTime date = new DateTime();
                        int id = -1;
                        foreach (ListViewItem item in lvMonthlyMenu.SelectedItems)
                        {
                            if (DateTime.TryParse(item.Text.Trim(), out date))
                            {
                                MealsTypeScheduleTO typeSched = new MealsTypeScheduleTO();
                                date = DateTime.Parse(item.Text.Trim());
                                id = Int32.Parse(item.SubItems[1].Text.Trim());

                                isDeleted = new MealsTypeSchedule().Delete(id, date) && isDeleted;

                                if (!isDeleted)
                                    break;
                            }
                            
                        }
                       
                        if (isDeleted)
                        {
                            MessageBox.Show(rm.GetString("mealsTypeScheduleDeleted", culture));
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("mealsTypeScheduleNotDeleted", culture));
                        }

                       ArrayList mealsMenu = new MealsTypeSchedule().Search(-1, firstDayOfMonth, lastDayOfMonth);
                        populateListView(mealsMenu);


                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("SelectMenuItem", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MonthlyMenu.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvMonthlyMenu_ColumnClick_1(object sender, ColumnClickEventArgs e)
        {
            try
            {
                SortOrder prevOrder = lvMonthlyMenu.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvMonthlyMenu.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvMonthlyMenu.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvMonthlyMenu.Sorting = SortOrder.Ascending;
                }

                lvMonthlyMenu.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsMenu.lvMonthlyMenu_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }



    }
}
