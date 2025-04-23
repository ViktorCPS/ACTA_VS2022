using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TransferObjects;
using System.Resources;
using Util;
using System.Globalization;
using Common;
using System.Collections;

namespace UI
{
    public partial class MCVaccines : UserControl
    {

        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;

        const int Type = 0;
        const int Desc = 1;

        private int startIndex;
        List<VaccineTO> vaccines = new List<VaccineTO>();
        private ListViewItemComparer _comp;
        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;

        string menuItemID;
        string menuItemUsedID;

        bool addUsedPermission = false;
        bool updateUsedPermission = false;
        bool deleteUsedPermission = false;

        public MCVaccines()
        {
            try
            {
                InitializeComponent();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                rm = new ResourceManager("UI.Resource", typeof(MCRisks).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                btnPrev.Visible = false;
                btnNext.Visible = false;
            }

            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCVaccines.MCVaccines(): " + ex.Message + "\n");
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
            private CultureInfo culture;

            public ListViewItemComparer(ListView lv)
            {
                _listView = lv;
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
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
                    case MCVaccines.Type:
                    case MCVaccines.Desc:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }

                    default:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                }
            }
        }

        #endregion


        private void setLanguage()
        {
            try
            {
                lvVaccines.BeginUpdate();
                lvVaccines.Columns.Add(rm.GetString("hdrCompany", culture), (lvVaccines.Width - 5) / 2 - 4, HorizontalAlignment.Right);
                lvVaccines.Columns.Add(rm.GetString("hdrVaccineType", culture), (lvVaccines.Width - 5) / 2 - 4, HorizontalAlignment.Left);
                lvVaccines.EndUpdate();
                // groupbox text
                this.gbSearch.Text = rm.GetString("gbSearch", culture);

                lblDesc.Text = rm.GetString("lblDesc", culture);
                lblVacType.Text = rm.GetString("lblVaccineType", culture);

                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex -= Constants.recordsPerPage;
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
                populateList(vaccines, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCVaccines.btnPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex += Constants.recordsPerPage;
                populateList(vaccines, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCVaccines.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void populateList(List<VaccineTO> risks, int startIndex)
        {
            try
            {
                if (risks.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvVaccines.BeginUpdate();
                lvVaccines.Items.Clear();

                if (risks.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < risks.Count))
                    {
                        if (startIndex == 0)
                        {
                            btnPrev.Enabled = false;
                        }
                        else
                        {
                            btnPrev.Enabled = true;
                        }

                        int lastIndex = startIndex + Constants.recordsPerPage;
                        if (lastIndex >= risks.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = risks.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            VaccineTO vaccineTO = risks[i];
                            ListViewItem item = new ListViewItem();

                            EmployeeTO empl = new EmployeeTO();


                            item.Text = vaccineTO.VaccineType;

                            if (logInUser.LangCode.Equals(Constants.Lang_en))
                                item.SubItems.Add(vaccineTO.DescEN);
                            else item.SubItems.Add(vaccineTO.DescSR);

                            item.Tag = vaccineTO;
                            lvVaccines.Items.Add(item);
                        }
                    }
                }

                lvVaccines.EndUpdate();
                lvVaccines.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCVaccines.populateList(): " + ex.Message + "\n");
                throw ex;

            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                VaccineTO vaccineTo = new VaccineTO();
                vaccineTo.VaccineType = txtVaccineType.Text;
                if (logInUser.LangCode.Equals(Constants.Lang_en))
                    vaccineTo.DescEN = txtDescription.Text;
                else vaccineTo.DescSR = txtDescription.Text;

                Vaccine vaccine = new Vaccine();
                vaccine.VaccineTO = vaccineTo;
                vaccines = vaccine.SearchVaccines();
                if (vaccines.Count > 0)
                    populateList(vaccines, startIndex);
                else MessageBox.Show(rm.GetString("", culture));

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCVaccines.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void lvVaccines_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                SortOrder prevOrder = lvVaccines.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvVaccines.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvVaccines.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvVaccines.Sorting = SortOrder.Ascending;
                }

                lvVaccines.Sort();
                startIndex = 0;
                populateList(vaccines, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCVaccines.lvVaccines_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void MCVaccines_Load(object sender, EventArgs e)
        {
            try
            {
                _comp = new ListViewItemComparer(lvVaccines);
                lvVaccines.ListViewItemSorter = _comp;
                lvVaccines.Sorting = SortOrder.Ascending;
                setLanguage();

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();

                menuItemID = NotificationController.GetCurrentMenuItemID();
                int index = menuItemID.LastIndexOf('_');

                menuItemUsedID = menuItemID + "_" + rm.GetString("tpVaccines", culture);
                setVisibility();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCVaccines.MCVaccines_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                new MCVaccinesAdd().ShowDialog();
                btnSearch_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                
              log.writeLog(DateTime.Now + " MCVaccines.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvVaccines.SelectedItems.Count == 1)
                {
                    new MCVaccinesAdd((VaccineTO)lvVaccines.SelectedItems[0].Tag).ShowDialog();
                    btnSearch_Click(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " MCVaccines.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvVaccines.SelectedItems.Count == 1)
                {
                    VaccineTO vaccineTO = (VaccineTO)lvVaccines.SelectedItems[0].Tag;
                    Vaccine vaccine = new Vaccine();
                    vaccine.VaccineTO = vaccineTO;
                    vaccine.Delete(vaccineTO.VaccineID, false);
                    btnSearch_Click(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " MCVaccines.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void setVisibility()
        {
            try
            {
                int permissionUsed;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permissionUsed = (((int[])menuItemsPermissions[menuItemUsedID])[role.ApplRoleID]);

                    addUsedPermission = addUsedPermission || (((permissionUsed / 4) % 2) == 0 ? false : true);
                    updateUsedPermission = updateUsedPermission || (((permissionUsed / 2) % 2) == 0 ? false : true);
                    deleteUsedPermission = deleteUsedPermission || ((permissionUsed % 2) == 0 ? false : true);

                }

                btnAdd.Enabled = addUsedPermission;
                btnUpdate.Enabled = updateUsedPermission;
                btnDelete.Enabled = deleteUsedPermission;

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MCDisabilties.setVisibility(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
