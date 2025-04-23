using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Resources;
using System.Globalization;

using TransferObjects;
using UI;
using Common;
using Util;

namespace UI
{
    public partial class MealTypes : UserControl
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
        const int FromIndex = 3;
        const int ToIndex = 4;

        public MealTypes()
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

                DateTime date = DateTime.Now;
                dtpHoursFrom.Value = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                dtpHoursTo.Value = new DateTime(date.Year, date.Month, date.Day, 23, 59, 0);
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
                    case MealTypes.MealTypeIDIndex:
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
                    case MealTypes.NameIndex:
                    case MealTypes.DescriptionIndex:
                    case MealTypes.FromIndex:
                    case MealTypes.ToIndex:
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
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClear.Text = rm.GetString("btnClear", culture);

                // groupbox text
                gbSearch.Text = rm.GetString("gbSearch", culture);

                //label's text
                this.lblMealTypeID.Text = rm.GetString("lblMealTypeID", culture);
                this.lblDescription.Text = rm.GetString("lblDescription", culture);
                this.lblName.Text = rm.GetString("lblName", culture);
                this.lblHoursFrom.Text = rm.GetString("lblFrom", culture);
                this.lblHoursTo.Text = rm.GetString("lblTo", culture);

                // list view initialization
                lvMealTypes.BeginUpdate();
                lvMealTypes.Columns.Add(rm.GetString("hdrMealTypeID", culture), (lvMealTypes.Width - 5) / 5 - 4, HorizontalAlignment.Right);
                lvMealTypes.Columns.Add(rm.GetString("hdrName", culture), (lvMealTypes.Width - 5) / 5 - 4, HorizontalAlignment.Left);
                lvMealTypes.Columns.Add(rm.GetString("hdrDescription", culture), (lvMealTypes.Width - 5) / 5 - 4, HorizontalAlignment.Left);
                lvMealTypes.Columns.Add(rm.GetString("hdrFrom", culture), (lvMealTypes.Width - 5) / 5 - 4, HorizontalAlignment.Left);
                lvMealTypes.Columns.Add(rm.GetString("hdrTo", culture), (lvMealTypes.Width - 5) / 5 - 4, HorizontalAlignment.Left);
                lvMealTypes.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypes.setLanguage(): " + ex.Message + "\n");
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
                        item.SubItems.Add(mealType.HoursFrom.ToString("HH:mm").Trim());
                        item.SubItems.Add(mealType.HoursTo.ToString("HH:mm").Trim());

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
                    currentMealType.MealTypeID = Int32.Parse(tbMealTypeID.Text);
                    currentMealType.Name = tbName.Text = lvMealTypes.SelectedItems[0].SubItems[1].Text.Trim();
                    currentMealType.Description = tbDescription.Text = lvMealTypes.SelectedItems[0].SubItems[2].Text.Trim();
                    DateTime date = DateTime.Parse(lvMealTypes.SelectedItems[0].SubItems[3].Text.Trim(), culture);
                    DateTime newDate = DateTime.Now;
                    currentMealType.HoursFrom = new DateTime(newDate.Year, newDate.Month, newDate.Day, date.Hour, date.Minute, 0);
                    DateTime oldDate = dtpHoursFrom.Value;
                    dtpHoursFrom.Value = new DateTime(oldDate.Year, oldDate.Month, oldDate.Day, date.Hour, date.Minute, 0);
                    date = DateTime.Parse(lvMealTypes.SelectedItems[0].SubItems[4].Text.Trim(), culture);
                    currentMealType.HoursTo = new DateTime(newDate.Year, newDate.Month, newDate.Day, date.Hour, date.Minute, 0);
                    oldDate = dtpHoursTo.Value;
                    dtpHoursTo.Value = new DateTime(oldDate.Year, oldDate.Month, oldDate.Day, date.Hour, date.Minute, 0);
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

                DateTime date = DateTime.Now;
                dtpHoursFrom.Value = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                dtpHoursTo.Value = new DateTime(date.Year, date.Month, date.Day, 23, 59, 0);

                clearCurrentMealType();

                lvMealTypes.Items.Clear();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypes.btnClear_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
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
                        currentMealType.MealTypeID = id;
                    }
                }
                
                if (!tbName.Text.Equals(""))
                {
                    currentMealType.Name = tbName.Text.ToString().Trim();
                }
                
                if (!tbDescription.Text.Equals(""))
                {
                    currentMealType.Description = tbDescription.Text.ToString().Trim();
                }

                DateTime date = DateTime.Now;
                currentMealType.HoursFrom = new DateTime(date.Year, date.Month, date.Day, dtpHoursFrom.Value.Hour, dtpHoursFrom.Value.Minute, 0);
                currentMealType.HoursTo = new DateTime(date.Year, date.Month, date.Day, dtpHoursTo.Value.Hour, dtpHoursTo.Value.Minute, 0);

                currentMealTypeList = currentMealType.Search(currentMealType.MealTypeID, currentMealType.Name, currentMealType.Description, currentMealType.HoursFrom, currentMealType.HoursTo);

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
                log.writeLog(DateTime.Now + " MealTypes.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                MealTypesAdd mta = new MealTypesAdd();
                mta.ShowDialog();
                if (mta.reloadOnReturn)
                {
                    btnClear.PerformClick();
                    currentMealTypeList = currentMealType.Search(-1, "", "", new DateTime(), new DateTime());
                    populateListView(currentMealTypeList);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypes.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvMealTypes.SelectedItems.Count > 0)
                {
                    MealTypesAdd mta = new MealTypesAdd(currentMealType);
                    mta.ShowDialog();
                    if (mta.reloadOnReturn)
                    {
                        btnClear.PerformClick();
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
                log.writeLog(DateTime.Now + " MealTypes.btnUpdate_Click(): " + ex.Message + "\n");
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

                                isDeleted = currentMealType.Delete(id) && isDeleted;
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
                log.writeLog(DateTime.Now + " MealTypes.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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

                currentMealTypeList = currentMealType.Search(-1, "", "", new DateTime(), new DateTime());
                populateListView(currentMealTypeList);
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " MealTypes.MealTypes_Load(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MealTypes.lvMealTypes_ColumnClick(): " + ex.Message + "\n");
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
                        MessageBox.Show(rm.GetString("MealTypeIDNotInt", culture));
                        if (!tbMealTypeID.Text.Equals(""))
                        {
                            tbMealTypeID.Text = tbMealTypeID.Text.Substring(0, tbMealTypeID.Text.Length - 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypes.tbMealTypeID_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void clearCurrentMealType()
        {
            try
            {
                currentMealType = new MealType();
                DateTime date = DateTime.Now;
                currentMealType.HoursFrom = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                currentMealType.HoursTo = new DateTime(date.Year, date.Month, date.Day, 23, 59, 0);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypes.tbMealTypeID_TextChanged(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
