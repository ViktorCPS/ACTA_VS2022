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
    public partial class MealPoints : UserControl
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        MealPoint currentMealPoint;
        ArrayList currentMealPointsList;
        private ListViewItemComparer _comp;

        // List View indexes
        const int IDIndex = 0;
        const int TerminalIndex = 1;
        const int NameIndex = 2;
        const int DescIndex = 3;

        public MealPoints()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                rm = new ResourceManager("UI.Resource", typeof(MealPoints).Assembly);
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
                    case MealPoints.IDIndex:
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
                    case MealPoints.TerminalIndex:
                    case MealPoints.NameIndex:
                    case MealPoints.DescIndex:
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
                this.gbSearch.Text = rm.GetString("gbSearch", culture);

                //label's text
                this.lblPointID.Text = rm.GetString("lblMealPointID", culture);
                this.lblDescription.Text = rm.GetString("lblDescription", culture);
                this.lblName.Text = rm.GetString("lblName", culture);
                this.lblTerminalSerial.Text = rm.GetString("lblTerminalSerial", culture);

                // list view initialization
                lvMealPoints.BeginUpdate();
                lvMealPoints.Columns.Add(rm.GetString("hdrMealPointID", culture), (lvMealPoints.Width - 4) / 4 - 5, HorizontalAlignment.Right);
                lvMealPoints.Columns.Add(rm.GetString("hdrTerminalSerial", culture), (lvMealPoints.Width - 4) / 4 - 5, HorizontalAlignment.Left);
                lvMealPoints.Columns.Add(rm.GetString("hdrName", culture), (lvMealPoints.Width - 4) / 4 - 5, HorizontalAlignment.Left);
                lvMealPoints.Columns.Add(rm.GetString("hdrDescription", culture), (lvMealPoints.Width - 4) / 4 - 5, HorizontalAlignment.Left);

                lvMealPoints.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPoints.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public void populateListView(ArrayList mealTypeEmplList)
        {
            try
            {
                lvMealPoints.BeginUpdate();
                lvMealPoints.Items.Clear();

                if (mealTypeEmplList.Count > 0)
                {
                    for (int i = 0; i < mealTypeEmplList.Count; i++)
                    {
                        MealPoint mealTypeEmpl = (MealPoint)mealTypeEmplList[i];
                        ListViewItem item = new ListViewItem();

                        item.Text = mealTypeEmpl.MealPointID.ToString().Trim();
                        item.SubItems.Add(mealTypeEmpl.TerminalSerial.Trim());
                        item.SubItems.Add(mealTypeEmpl.Name.Trim());
                        item.SubItems.Add(mealTypeEmpl.Description.Trim());

                        lvMealPoints.Items.Add(item);
                    }

                }

                lvMealPoints.EndUpdate();
                lvMealPoints.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPoints.populateListView(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void lvMealPoints_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lvMealPoints.SelectedItems.Count == 1)
            {
                try
                {
                    // populate MealType's search form
                    tbMealPointID.Text = lvMealPoints.SelectedItems[0].Text.Trim();
                    currentMealPoint.MealPointID = Int32.Parse(tbMealPointID.Text);
                    currentMealPoint.TerminalSerial = tbTerminalSerial.Text = lvMealPoints.SelectedItems[0].SubItems[1].Text.Trim();
                    currentMealPoint.Name = tbName.Text = lvMealPoints.SelectedItems[0].SubItems[2].Text.Trim();
                    currentMealPoint.Description = tbDescription.Text = lvMealPoints.SelectedItems[0].SubItems[3].Text.Trim();
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now + " MealPoints.lvMealPoints_SelectedIndexChanged(): " + ex.Message + "\n");
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
                tbTerminalSerial.Text = "";

                currentMealPoint = new MealPoint();
                lvMealPoints.Items.Clear();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPoints.btnClear_Click(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                currentMealPoint = new MealPoint();

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
                        currentMealPoint.MealPointID = id;
                    }
                }

                if (!this.tbTerminalSerial.Text.Equals(""))
                {
                    currentMealPoint.TerminalSerial = tbTerminalSerial.Text.ToString().Trim();
                }
                if (!tbName.Text.Equals(""))
                {
                    currentMealPoint.Name = tbName.Text.ToString().Trim();
                }
                if (!tbDescription.Text.Equals(""))
                {
                    currentMealPoint.Description = tbDescription.Text.ToString().Trim();
                }
                
                currentMealPointsList = currentMealPoint.Search(currentMealPoint.MealPointID, currentMealPoint.TerminalSerial, currentMealPoint.Name, currentMealPoint.Description);

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
                log.writeLog(DateTime.Now + " MealPoints.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                MealPointAdd mpa = new MealPointAdd();
                mpa.ShowDialog();
                if (mpa.reloadOnReturn)
                {
                    btnClear.PerformClick();
                    currentMealPointsList = currentMealPoint.Search(-1, "", "", "");
                    populateListView(currentMealPointsList);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPoints.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvMealPoints.SelectedItems.Count > 0)
                {
                    MealPointAdd mpa = new MealPointAdd(currentMealPoint);
                    mpa.ShowDialog();
                    if (mpa.reloadOnReturn)
                    {
                        btnClear.PerformClick();
                        currentMealPointsList = currentMealPoint.Search(-1, "", "", "");
                        populateListView(currentMealPointsList);
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noMealPointUpd", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPoints.btnUpdate_Click(): " + ex.Message + "\n");
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
                            isDeleted = currentMealPoint.Delete(item.Text.Trim()) && isDeleted;
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
                        currentMealPointsList = currentMealPoint.Search(-1, "", "", "");
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
                log.writeLog(DateTime.Now + " MealPoints.btnDelete_Click(): " + ex.Message + "\n");
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

                currentMealPoint = new MealPoint();
                currentMealPointsList = currentMealPoint.Search(-1, "", "", "");
                populateListView(currentMealPointsList);

                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " MealPoints.MealPoints_Load(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MealPoints.tbMealPointID_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
