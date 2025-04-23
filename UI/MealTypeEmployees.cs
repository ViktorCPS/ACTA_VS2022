using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;

using System.Data;
using TransferObjects;
using Common;
using Util;
using Reports;

using System.Resources;
using System.Globalization;

namespace UI
{
    public partial class MealTypeEmployees : UserControl
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        MealTypeEmpl currentMealTypeEmpl;
        ArrayList currentMealTypeEmplList;
        private ListViewItemComparer _comp;

        // List View indexes
        const int IDIndex = 0;
        const int NameIndex = 1;
        const int DescIndex = 2;

        public MealTypeEmployees()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                rm = new ResourceManager("UI.Resource", typeof(MealTypeEmployees).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();
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
                    case MealTypeEmployees.IDIndex:
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
                    case MealTypeEmployees.NameIndex:
                    case MealTypeEmployees.DescIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");
                }
            }
        }

        #endregion

        public void populateListView(ArrayList mealTypeEmplList)
        {
            try
            {
                lvMealTypeEmpl.BeginUpdate();
                lvMealTypeEmpl.Items.Clear();

                if (mealTypeEmplList.Count > 0)
                {
                    for (int i = 0; i < mealTypeEmplList.Count; i++)
                    {
                        MealTypeEmpl mealTypeEmpl = (MealTypeEmpl)mealTypeEmplList[i];
                        ListViewItem item = new ListViewItem();

                        item.Text = mealTypeEmpl.MealTypeEmplID.ToString().Trim();
                        item.SubItems.Add(mealTypeEmpl.Name.Trim());
                        item.SubItems.Add(mealTypeEmpl.Description.Trim());

                        lvMealTypeEmpl.Items.Add(item);
                    }
                }

                lvMealTypeEmpl.EndUpdate();
                lvMealTypeEmpl.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmployees.populateListView(): " + ex.Message + "\n");
                throw ex;
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
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnClear.Text = rm.GetString("btnClear", culture);

                // groupbox text
                this.gbSearch.Text = rm.GetString("gbSearch", culture);

                //label's text
                this.lblMealTypeEmplID.Text = rm.GetString("lblMealTypeEmplID", culture);
                this.lblDescription.Text = rm.GetString("lblDescription", culture);
                this.lblName.Text = rm.GetString("lblName", culture);

                // list view initialization
                lvMealTypeEmpl.BeginUpdate();
                lvMealTypeEmpl.Columns.Add(rm.GetString("hdrMealTypeEmplID", culture), (lvMealTypeEmpl.Width - 3) / 3 - 7, HorizontalAlignment.Right);
                lvMealTypeEmpl.Columns.Add(rm.GetString("hdrName", culture), (lvMealTypeEmpl.Width - 3) / 3 - 7, HorizontalAlignment.Left);
                lvMealTypeEmpl.Columns.Add(rm.GetString("hdrDescription", culture), (lvMealTypeEmpl.Width - 3) / 3 - 7, HorizontalAlignment.Left);

                lvMealTypeEmpl.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmployees.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void lvMealTypeEmpl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lvMealTypeEmpl.SelectedItems.Count == 1)
            {
                try
                {
                    // populate MealType's search form
                    tbMealTypeEmplID.Text = lvMealTypeEmpl.SelectedItems[0].Text.Trim();
                    currentMealTypeEmpl.MealTypeEmplID = Int32.Parse(tbMealTypeEmplID.Text);
                    currentMealTypeEmpl.Name = tbName.Text = lvMealTypeEmpl.SelectedItems[0].SubItems[1].Text.Trim();
                    currentMealTypeEmpl.Description = tbDescription.Text = lvMealTypeEmpl.SelectedItems[0].SubItems[2].Text.Trim();
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now + " MealTypeEmployees.lvMealTypeEmployees_SelectedIndexChanged(): " + ex.Message + "\n");
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbMealTypeEmplID.Text = "";
            tbDescription.Text = "";
            tbName.Text = "";

            currentMealTypeEmpl = new MealTypeEmpl();

            lvMealTypeEmpl.Items.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                currentMealTypeEmpl = new MealTypeEmpl();
                
                if (!tbMealTypeEmplID.Text.Equals(""))
                {
                    int id;
                    bool idIsInt = Int32.TryParse(tbMealTypeEmplID.Text.ToString().Trim(), out id);

                    if (!idIsInt)
                    {
                        MessageBox.Show(rm.GetString("MealTypeEmployeeIDNotInt", culture));
                        return;
                    }
                    else
                    {
                        currentMealTypeEmpl.MealTypeEmplID = id;
                    }
                }
                if (!tbName.Text.Equals(""))
                {
                    currentMealTypeEmpl.Name = tbName.Text.ToString().Trim();
                }
                if (!tbDescription.Text.Equals(""))
                {
                    currentMealTypeEmpl.Description = tbDescription.Text.ToString().Trim();
                }
                
                currentMealTypeEmplList = currentMealTypeEmpl.Search(currentMealTypeEmpl.MealTypeEmplID, currentMealTypeEmpl.Name, currentMealTypeEmpl.Description);

                if (currentMealTypeEmplList.Count > 0)
                {
                    populateListView(currentMealTypeEmplList);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noMealTypesFound", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmployees.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                MealTypeEmplAdd mtea = new MealTypeEmplAdd();
                mtea.ShowDialog();
                if (mtea.reloadOnReturn)
                {
                    btnClear.PerformClick();
                    currentMealTypeEmplList = currentMealTypeEmpl.Search(-1, "", "");
                    populateListView(currentMealTypeEmplList);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmployees.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvMealTypeEmpl.SelectedItems.Count > 0)
                {
                    MealTypeEmplAdd mtea = new MealTypeEmplAdd(currentMealTypeEmpl);
                    mtea.ShowDialog();
                    if (mtea.reloadOnReturn)
                    {
                        btnClear.PerformClick();
                        currentMealTypeEmplList = currentMealTypeEmpl.Search(-1, "", "");
                        populateListView(currentMealTypeEmplList);
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noMealTypeUpd", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmployees.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool isDeleted = true;

                if (lvMealTypeEmpl.SelectedItems.Count > 0)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("messageDeleteMealType", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        foreach (ListViewItem item in lvMealTypeEmpl.SelectedItems)
                        {
                            isDeleted = currentMealTypeEmpl.Delete(item.Text.Trim()) && isDeleted;
                            if (!isDeleted)
                                break;
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
                        currentMealTypeEmplList = currentMealTypeEmpl.Search(-1, "", "");
                        populateListView(currentMealTypeEmplList);
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("SelectMealType", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmployees.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void MealTypeEmployees_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                _comp = new ListViewItemComparer(lvMealTypeEmpl);
                lvMealTypeEmpl.ListViewItemSorter = _comp;
                lvMealTypeEmpl.Sorting = SortOrder.Ascending;

                currentMealTypeEmpl = new MealTypeEmpl();
                currentMealTypeEmplList = currentMealTypeEmpl.Search(-1, "", "");
                populateListView(currentMealTypeEmplList);
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " MealTypeEmployees.MealTypeEmployees_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvMealTypeEmpl_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                SortOrder prevOrder = lvMealTypeEmpl.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvMealTypeEmpl.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvMealTypeEmpl.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvMealTypeEmpl.Sorting = SortOrder.Ascending;
                }

                lvMealTypeEmpl.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypeEmployees.lvMealTypeEmpl_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void tbMealTypeEmplID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int id;
                if (!tbMealTypeEmplID.Text.Equals(""))
                {
                    bool idIsInt = Int32.TryParse(tbMealTypeEmplID.Text.ToString().Trim(), out id);

                    if (!idIsInt)
                    {
                        MessageBox.Show(rm.GetString("MealTypeEmployeeIDNotInt", culture));
                        if (!tbMealTypeEmplID.Text.Equals(""))
                        {
                            tbMealTypeEmplID.Text = tbMealTypeEmplID.Text.Substring(0, tbMealTypeEmplID.Text.Length - 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealtypeEmployees.tbMealTypeEmplID_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
