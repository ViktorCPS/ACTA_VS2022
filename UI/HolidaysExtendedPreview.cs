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
using Common;
using TransferObjects;
using System.Collections;

namespace UI
{
    public partial class HolidaysExtendedPreview : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        List<HolidaysExtendedTO> list = new List<HolidaysExtendedTO>();
        // List View indexes
        const int DescriptionIndex = 0;
        const int CategoryIndex = 2;
        const int TypeIndex = 1;
        const int yearIndex = 3;
        const int fromIndex = 4;
        const int toIndex = 5;

        private ListViewItemComparer _comp;

        public HolidaysExtendedPreview()
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);
            setLanguage();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Set proper language.
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // button's text
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClose.Text = rm.GetString("btnClose", culture);

                // Form name
                this.Text = rm.GetString("menuHolidays", culture);

                // group box text
                this.gbSearch.Text = rm.GetString("gbSearch", culture);
             
                // label's text
                lblDescription.Text = rm.GetString("lblDescription", culture);
                lblCategory.Text = rm.GetString("lblCategory", culture);
                lblType.Text = rm.GetString("lblType", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblYear.Text = rm.GetString("lblYear", culture);

                // list view initialization
                lvHolidays.BeginUpdate();
                lvHolidays.Columns.Add(rm.GetString("hdrDescription", culture), lvHolidays.Width / 7 + 60, HorizontalAlignment.Left);
                lvHolidays.Columns.Add(rm.GetString("hdrType", culture), lvHolidays.Width / 7, HorizontalAlignment.Left);
                lvHolidays.Columns.Add(rm.GetString("hdrCategory", culture), lvHolidays.Width  / 7 -30, HorizontalAlignment.Left);
                lvHolidays.Columns.Add(rm.GetString("hdrYear", culture), lvHolidays.Width  / 7 -20, HorizontalAlignment.Left);
                lvHolidays.Columns.Add(rm.GetString("hdrFrom", culture), lvHolidays.Width  / 7, HorizontalAlignment.Left);
                lvHolidays.Columns.Add(rm.GetString("hdrTo", culture), lvHolidays.Width / 7, HorizontalAlignment.Left);
                lvHolidays.Columns.Add(rm.GetString("hdrSundayTrans", culture), lvHolidays.Width / 7-30, HorizontalAlignment.Left);
                lvHolidays.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysExtendedPreview.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Populate Category Combo Box
        /// </summary>
        private void populateCategoryCombo()
        {
            try
            {
                cbType.Items.Add(rm.GetString("all", culture));
                cbType.Items.Add(rm.GetString(Constants.nationalHoliday,culture));
                cbType.Items.Add(rm.GetString(Constants.personalHoliday,culture));

                cbType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysExtendedPreview.populateCategoryCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Populate Description Combo Box
        /// </summary>
        private void populateDescriptionCombo(List<string> descriptions)
        {
            try
            {
                cbDescription.Items.Add(rm.GetString("all", culture));
                foreach (string str in descriptions)
                {
                    cbDescription.Items.Add(str);
                }

                cbDescription.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysExtendedPreview.populateDescriptionCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Populate Type Combo Box
        /// </summary>
        private void populateTypeCombo()
        {
            try
            {
                cbCategory.Items.Add(rm.GetString("all", culture));
                cbCategory.Items.Add(Constants.holidayTypeI);
                cbCategory.Items.Add(Constants.holidayTypeII);
                cbCategory.Items.Add(Constants.holidayTypeIII);

                cbCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysExtendedPreview.populateCategoryCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void HolidaysExtendedPreview_Load(object sender, EventArgs e)
        {
            try
            {

                // Initialize comparer object
                _comp = new ListViewItemComparer(lvHolidays);
                lvHolidays.ListViewItemSorter = _comp;
                lvHolidays.Sorting = SortOrder.Ascending;

                List<string> descriptions = new HolidaysExtended().SearchDescriptions();
                populateDescriptionCombo(descriptions);
                populateCategoryCombo();
                populateTypeCombo();
                dtpYear.Value = DateTime.Now;
                dtpFrom.Value = new DateTime(DateTime.Now.Year, 1, 1);
                dtpTo.Value = new DateTime(DateTime.Now.Year, 12, 31);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysExtendedPreview.HolidaysExtendedPreview_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void dtpYear_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                dtpFrom.Value = new DateTime(dtpYear.Value.Year, 1, 1);
                dtpTo.Value = new DateTime(dtpYear.Value.Year, 12, 31);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysExtendedPreview.dtpYear_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                HolidaysExtended holidays = new HolidaysExtended();
                if (cbDescription.SelectedIndex > 0)
                    holidays.HolTO.Description = cbDescription.SelectedItem.ToString();
                if (cbType.SelectedIndex > 0)
                {
                    if (cbType.SelectedIndex == 1)
                        holidays.HolTO.Type = Constants.nationalHoliday;
                    else
                        holidays.HolTO.Type = Constants.personalHoliday;
                }
                if (cbCategory.SelectedIndex > 0)
                    holidays.HolTO.Category = cbCategory.SelectedItem.ToString();
                holidays.HolTO.Year = dtpYear.Value;
                list = holidays.Search(dtpFrom.Value.Date, dtpTo.Value.Date);
                populateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysExtendedPreview.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Populate List View with Holidays found
        /// </summary>
        /// <param name="holidaysList"></param>
        public void populateListView()
        {
            try
            {
                lvHolidays.BeginUpdate();
                lvHolidays.Items.Clear();

                if (list.Count > 0)
                {
                    foreach (HolidaysExtendedTO holiday in list)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = holiday.Description.Trim();
                        item.SubItems.Add(rm.GetString(holiday.Type.Trim(), culture));
                        if (holiday.Category.Trim() != "")
                            item.SubItems.Add(holiday.Category.Trim());
                        else
                            item.SubItems.Add("N/A");
                        item.SubItems.Add(holiday.Year.ToString("yyyy").Trim());
                        item.SubItems.Add(holiday.DateStart.ToString("dd.MM.yyy").Trim());
                        item.SubItems.Add(holiday.DateEnd.ToString("dd.MM.yyy").Trim());
                        if (holiday.SundayTransferable == Constants.yesInt)
                        {
                            item.SubItems.Add(rm.GetString("yes",culture));
                        }
                        else
                        {
                            item.SubItems.Add(rm.GetString("no", culture));
                        }
                        item.Tag = holiday;

                        lvHolidays.Items.Add(item);
                    }
                }

                lvHolidays.EndUpdate();
                lvHolidays.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysExtendedPreview.populateListView(): " + ex.Message + "\n");
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
                    case HolidaysExtendedPreview.DescriptionIndex:
                    case HolidaysExtendedPreview.CategoryIndex:
                    case HolidaysExtendedPreview.TypeIndex:
                    case HolidaysExtendedPreview.yearIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    case HolidaysExtendedPreview.fromIndex:
                    case HolidaysExtendedPreview.toIndex:
                        {
                            DateTime dt1 = new DateTime(1, 1, 1, 0, 0, 0);
                            DateTime dt2 = new DateTime(1, 1, 1, 0, 0, 0);

                            if (!sub1.Text.Trim().Equals(""))
                            {
                                dt1 = DateTime.ParseExact(sub1.Text.Trim(), "dd.MM.yyyy", null);
                            }

                            if (!sub2.Text.Trim().Equals(""))
                            {
                                dt2 = DateTime.ParseExact(sub2.Text.Trim(), "dd.MM.yyyy", null);
                            }

                            return CaseInsensitiveComparer.Default.Compare(dt1, dt2);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");

                }
            }
        }

        #endregion

        private void lvHolidays_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvHolidays.Sorting;
                lvHolidays.Sorting = SortOrder.None;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvHolidays.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvHolidays.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvHolidays.Sorting = SortOrder.Ascending;


                } lvHolidays.ListViewItemSorter = _comp;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysExtendedPreview.lvHolidays_ColumnClick(): " + ex.Message + "\n");
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

                if (lvHolidays.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noSelHolidayDel", culture));
                }
                else
                {
                    DialogResult result = MessageBox.Show(rm.GetString("deleteHolidays", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        bool isDeleted = true;
                        foreach (ListViewItem item in lvHolidays.SelectedItems)
                        {
                            isDeleted = new HolidaysExtended().Delete(((HolidaysExtendedTO)item.Tag).RecID) && isDeleted;
                        }
                        if (isDeleted)
                        {
                            MessageBox.Show(rm.GetString("HolidaysDel", culture));
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("noHolidayDel", culture));
                        }

                        btnSearch_Click(this, new EventArgs());
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysExtendedPreview.btnDelete_Click(): " + ex.Message + "\n");
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
                HolidaysExtendedAdd add = new HolidaysExtendedAdd();
                add.ShowDialog();
                btnSearch_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysExtendedPreview.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (this.lvHolidays.SelectedItems.Count != 1)
                {
                    MessageBox.Show(rm.GetString("selOneHoliday", culture));
                }
                else
                {
                    HolidaysExtendedTO currentHoliday = (HolidaysExtendedTO)lvHolidays.SelectedItems[0].Tag;

                    // Open Update Form
                    HolidaysExtendedAdd addForm = new HolidaysExtendedAdd(currentHoliday);
                    addForm.ShowDialog(this);

                    btnSearch_Click(this, new EventArgs());
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Holidays.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
