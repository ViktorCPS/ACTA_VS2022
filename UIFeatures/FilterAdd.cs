using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;
using System.Collections;

namespace UIFeatures
{
    public partial class FilterAdd : Form
    {
        private Form form;
        private TabPage page;

        ResourceManager rm;
        private CultureInfo culture;
        DebugLog log;

        Filter currentFilter;

        string tabID = "";

        int shortView = 0;
        int longView = 0;
        ArrayList listFilters;

        Hashtable filtersTable = new Hashtable();

        private int sortOrder;
        private int sortField;

        // List View indexes
        const int FilterNameIndex = 0;
        const int DescriptionIndex = 1;
        const int DefaultIndex = 2;

        public FilterAdd(Form f, Filter filter)
        {
            InitializeComponent();

            form = f;

            currentFilter = filter;
        }

        public FilterAdd(Form f,Filter filter, TabPage p)
        {
            InitializeComponent();

            form = f;
            page = p;

            if (page != null)
                tabID = page.Text;

            currentFilter = filter;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbName.Text.ToString().Trim().Length == 0)
                {
                    MessageBox.Show(rm.GetString("tbNameNotSet", culture));
                    return;
                }
                int def = Constants.filterNotDefault;
                
                Filter filter = new Filter();                
              
                //get default filters from data base
                ArrayList defFilters = filter.getDefaults(tabID);

                //if filter is makred as default or there is no default filter in data base set this filter as default
                if (defFilters.Count == 0||cbDefault.Checked)
                    def = Constants.filterDefault;

                bool filtersaved = false;
                if (def == Constants.filterNotDefault || defFilters.Count == 0)
                    filtersaved = filter.saveFilter(form, tabID, tbName.Text.ToString(), tbDescription.Text.ToString(), def,true);
                else
                {
                   bool trans = filter.BeginTransaction();
                   if (trans)
                   {
                       try
                       {
                           bool updated = filter.setNoDefaults(tabID,false);
                           filtersaved = filter.saveFilter(form, tabID, tbName.Text.ToString(), tbDescription.Text.ToString(), def, false);
                           if (filtersaved)
                               filter.CommitTransaction();
                           else
                               filter.RollbackTransaction();
                       }
                       catch(Exception ex)
                       {
                           filter.RollbackTransaction();
                           log.writeLog(DateTime.Now + " FilterAdd.FilterAdd_Load(): " + ex.Message + "\n");
                           MessageBox.Show(ex.Message);
                       }
                   }
                   else
                   {
                       MessageBox.Show(rm.GetString("FilterNOTSaved", culture));
                   }
                }
                if (filtersaved)
                {
                    MessageBox.Show(rm.GetString("FilterSaved", culture));
                    this.Close();
                }
                else
                    MessageBox.Show(rm.GetString("FilterNOTSaved", culture));

                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " FilterAdd.FilterAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void PopulateListView()
        {
            try
            {
                listFilters = currentFilter.getFilters(tabID);
                listFilters.Sort(new ArrayListSort(sortOrder, FilterAdd.FilterNameIndex));    
                if (listFilters.Count <= 0)
                    btnAllFilters.Visible = false;
                else
                    btnAllFilters.Visible = true;

                lvFilters.BeginUpdate();
                lvFilters.Items.Clear();

                foreach (FilterTO filter in listFilters)
                {
                    if (!filtersTable.ContainsKey(filter.FilterID))
                    {
                        filtersTable.Add(filter.FilterID, filter);
                    }

                    ListViewItem item = new ListViewItem();

                    item.Text = filter.FilterName;
                    item.SubItems.Add(filter.Description);
                    if (filter.Default == Constants.filterDefault)
                        item.SubItems.Add(rm.GetString("yes", culture));
                    else
                        item.SubItems.Add(rm.GetString("no", culture));

                    item.Tag = filter;

                    lvFilters.Items.Add(item);
                }

                lvFilters.EndUpdate();
            }
                
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " FilterAdd.PopulateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void PopulateListViewOrder()
        {
            try
            {                

                lvFilters.BeginUpdate();
                lvFilters.Items.Clear();

                foreach (FilterTO filter in listFilters)
                {                  
                    ListViewItem item = new ListViewItem();

                    item.Text = filter.FilterName;
                    item.SubItems.Add(filter.Description);
                    if (filter.Default == Constants.filterDefault)
                        item.SubItems.Add(rm.GetString("yes", culture));
                    else
                        item.SubItems.Add(rm.GetString("no", culture));

                    item.Tag = filter;

                    lvFilters.Items.Add(item);
                }

                lvFilters.EndUpdate();
            }

            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PopulateListViewOrder(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }


        private void FilterAdd_Load(object sender, EventArgs e)
        {
            try
            {
                shortView = btnAllFilters.Location.Y + btnAllFilters.Height + 50;
                longView = gbFilters.Location.Y + gbFilters.Height + 50+lblDoubleClickToDelete.Height;

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                this.CenterToScreen();
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                rm = new ResourceManager("UIFeatures.Resource", typeof(FilterAdd).Assembly);

                setLanguage();

                if (currentFilter.FilterID == -1)
                {
                    //form text
                    this.Text = rm.GetString("FilterAdd", culture);
                    btnSave.Visible = true;
                    btnUpdate.Visible = false;
                }
                else
                {
                    //form text
                    this.Text = rm.GetString("FilterUpdate", culture);
                    btnUpdate.Visible = true;
                    btnSave.Visible = false;
                    tbDescription.Text = currentFilter.Description;
                    tbName.Text = currentFilter.FilterName;
                    cbDefault.Checked = (currentFilter.Default == Constants.filterDefault);
                }
                this.Height = shortView;

                PopulateListView();
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " FilterAdd.FilterAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setLanguage()
        {
            try
            {                

                //label's text
                lblDescription.Text = rm.GetString("lblDescription", culture);
                lblName.Text = rm.GetString("lblName", culture);
                lblDoubleClickToDelete.Text = rm.GetString("lblDoubleClickToDelete", culture);

                //button's text
                btnCancel.Text = rm.GetString("btnCancel", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnSave.Text = rm.GetString("btnSave", culture);
                btnAllFilters.Text = rm.GetString("showAllFilters", culture);

                //check box's text
                cbDefault.Text = rm.GetString("cbDefault", culture);

                //group box's text
                gbFilters.Text = rm.GetString("gbFilters", culture);

                //list view text
                lvFilters.BeginUpdate();
                lvFilters.Columns.Add(rm.GetString("hdrName", culture),lvFilters.Width/3+10);
                lvFilters.Columns.Add(rm.GetString("hdrDescription", culture), lvFilters.Width / 3+10);
                lvFilters.Columns.Add(rm.GetString("hdrDefault", culture), lvFilters.Width / 3 - 43);
                lvFilters.EndUpdate();
            }
            catch(Exception ex)
            {
                log.writeLog(DateTime.Now + " FilterAdd.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvFilters_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (lvFilters.SelectedItems.Count == 1)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("messageDeleteFilter", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        FilterTO filter = (FilterTO)lvFilters.SelectedItems[0].Tag;
                        if (filter.Default == Constants.filterDefault)
                        {
                            //MessageBox.Show(rm.GetString("DefFilter", culture));
                            //return;
                            if (filtersTable.Count > 1)
                            {
                                FilterTO filterTo = new FilterTO();
                                filterTo.CreatedTime = new DateTime();
                                foreach (FilterTO fTo in filtersTable.Values)
                                {
                                    if (filterTo.CreatedTime < fTo.CreatedTime)
                                        filterTo = fTo;
                                }
                                if (filterTo.CreatedTime > new DateTime() && filterTo.Default != Constants.filterDefault)
                                {
                                    bool updated = currentFilter.updateFilter(form, tabID, filterTo.FilterName, filterTo.Description, Constants.filterDefault, filterTo.FilterID, true);
                                }
                            }
                            
                        }
                        bool deleted = currentFilter.Delete(filter.FilterID);

                        if (deleted)
                            MessageBox.Show(rm.GetString("filterDeleted", culture));
                        else
                            MessageBox.Show(rm.GetString("filterNotDeleted", culture));

                        PopulateListView();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " FilterAdd.lvFilters_DoubleClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbName.Text.ToString().Trim().Length == 0)
                {
                    MessageBox.Show(rm.GetString("tbNameNotSet", culture));
                    return;
                }
                int def = Constants.filterNotDefault;

                Filter filter = new Filter();

                //get default filters from data base
                ArrayList defFilters = filter.getDefaults(tabID);

                foreach (FilterTO f in defFilters)
                {
                    if (f.FilterID == currentFilter.FilterID)
                    {
                        defFilters.Remove(f);
                        break;
                    }
                }

                //if filter is makred as default or there is no default filter in data base set this filter as default
                if (defFilters.Count == 0 || cbDefault.Checked)
                    def = Constants.filterDefault;

                bool filtersaved = false;
                if (def == Constants.filterNotDefault || defFilters.Count == 0)
                    filtersaved = filter.updateFilter(form, tabID, tbName.Text.ToString(), tbDescription.Text.ToString(), def,currentFilter.FilterID, true);
                else
                {
                    bool trans = filter.BeginTransaction();
                    if (trans)
                    {
                        try
                        {
                            bool updated = filter.setNoDefaults(tabID, false);
                            filtersaved = filter.updateFilter(form, tabID, tbName.Text.ToString(), tbDescription.Text.ToString(), def, currentFilter.FilterID, false);
                            if (filtersaved)
                                filter.CommitTransaction();
                            else
                                filter.RollbackTransaction();
                        }
                        catch (Exception ex)
                        {
                            filter.RollbackTransaction();
                            log.writeLog(DateTime.Now + " FilterAdd.btnUpdate_Click(): " + ex.Message + "\n");
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("FilterNOTUpdated", culture));
                    }
                }
                if (filtersaved)
                {
                    MessageBox.Show(rm.GetString("FilterUpdated", culture));
                    this.Close();
                }
                else
                    MessageBox.Show(rm.GetString("FilterNOTUpdated", culture));


            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " FilterAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAllFilters_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Height == shortView)
                {
                    this.Height = longView;
                    btnAllFilters.Text = rm.GetString("hideFilters", culture);
                }
                else
                {
                    this.Height = shortView;
                    btnAllFilters.Text = rm.GetString("showAllFilters", culture);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " FilterAdd.btnAllFilters_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvFilters_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int prevOrder = sortOrder;

                if (e.Column == sortField)
                {
                    if (prevOrder == Constants.sortAsc)
                    {
                        sortOrder = Constants.sortDesc;
                    }
                    else
                    {
                        sortOrder = Constants.sortAsc;
                    }
                }
                else
                {
                    // New Sort Order
                    sortOrder = Constants.sortAsc;
                }

                sortField = e.Column;

                listFilters.Sort(new ArrayListSort(sortOrder, sortField));
                
                PopulateListViewOrder();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " FilterAdd.lvEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show("Exception in lvEmployees_ColumnClick():" + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        #region Inner Class for sorting Array List of FilterAdd

        /*
		 *  Class used for sorting Array List of FilterAdd
		*/

        private class ArrayListSort : IComparer
        {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(object x, object y)
            {
                FilterTO f1 = null;
                FilterTO f2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    f1 = (FilterTO)x;
                    f2 = (FilterTO)y;
                }
                else
                {
                    f1 = (FilterTO)y;
                    f2 = (FilterTO)x;
                }

                switch (compField)
                {
                    case FilterAdd.FilterNameIndex:
                        return f1.FilterName.CompareTo(f2.FilterName);
                    case FilterAdd.DescriptionIndex:
                        return f1.Description.CompareTo(f2.Description);
                    case FilterAdd.DefaultIndex:
                        return f1.Default.CompareTo(f2.Default);
                    default:
                        return f1.FilterName.CompareTo(f2.FilterName);
                }
            }
        }

        #endregion
    }
}