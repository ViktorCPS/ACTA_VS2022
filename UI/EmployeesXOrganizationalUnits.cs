using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;

using Common;
using TransferObjects;
using Util;

namespace UI
{
    public partial class EmployeesXOrganizationalUnits : Form
    {
        ApplUserTO logInUser;
        ResourceManager rm;
        private CultureInfo culture;
        DebugLog log;

        private ListViewItemComparer _comp1;
        private ListViewItemComparer _comp2;
        private ListViewItemComparer _comp3;

        // List View indexes
        const int EmployeeIDIndex = 0;
        const int FirstNameIndex = 1;
        const int LastNameIndex = 2;
        const int OrgUnitNameIndex = 3;

        // NOTE: removedEmployees is not in use. If it is needed in the future,
        //relations between addedEmployees and removedEmployees need to be added. 
        //See ApplUsersXRoles.cs for example
        private List<ListViewItem> removedEmployees = new List<ListViewItem>();
        private List<ListViewItem> addedEmployees = new List<ListViewItem>();

        private string prevSelValuecbOU = "";

        private bool saveForPrevIndex = false;

        List<OrganizationalUnitTO> orgUnitArray = new List<OrganizationalUnitTO>();
        List<OrganizationalUnitTO> ouArray = new List<OrganizationalUnitTO>();

        Dictionary<int, OrganizationalUnitTO> ouDict = new Dictionary<int, OrganizationalUnitTO>();

        public EmployeesXOrganizationalUnits()
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();

            this.CenterToScreen();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            
            rm = new ResourceManager("UI.Resource", typeof(EmployeesXOrganizationalUnits).Assembly);
            setLanguage();
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
                    case EmployeesXOrganizationalUnits.EmployeeIDIndex:
                        int firstID = -1;
                        int secondID = -1;

                        if (!sub1.Text.Trim().Equals(""))
                        {
                            firstID = Int32.Parse(sub1.Text.Trim());
                        }

                        if (!sub2.Text.Trim().Equals(""))
                        {
                            secondID = Int32.Parse(sub2.Text.Trim());
                        }

                        return CaseInsensitiveComparer.Default.Compare(firstID, secondID);
                    case EmployeesXOrganizationalUnits.FirstNameIndex:
                    case EmployeesXOrganizationalUnits.LastNameIndex:
                    case EmployeesXOrganizationalUnits.OrgUnitNameIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");
                }
            }
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("employeesXOUForm", culture);

                // TabPage text
                tabPageEmployees.Text = rm.GetString("tabPageEmployees", culture);
                tabPageOrgUnits.Text = rm.GetString("tabPageOrgUnits", culture);
                tabPageEmployeesXOrgUnits.Text = rm.GetString("tabPageEmployeesXOrgUnits", culture);

                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);
                btnSave.Text = rm.GetString("btnSave", culture);

                // group box text
                gbOU.Text = rm.GetString("gbOU", culture);

                // label's text
                lblOrgUnit.Text = rm.GetString("lblOrgUnit", culture);
                lblOrgUnitDesc.Text = rm.GetString("lblDescription", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblSelOUName.Text = rm.GetString("lblName", culture);
                lblSelOUDesc.Text = rm.GetString("lblDescription", culture);
                lblOUName.Text = rm.GetString("lblName", culture);
                lblDesc.Text = rm.GetString("lblDescription", culture);
                lblEmployeeForOU.Text = rm.GetString("lblEmployeeForOU", culture);
                lblOU.Text = rm.GetString("lblOrgUnit", culture);
                lblOUDesc.Text = rm.GetString("lblDescription", culture);
                lblEmployeesForOU.Text = rm.GetString("lblEmployeesForOU", culture);
                lblSelEmployees.Text = rm.GetString("lblEmployeeForOU", culture);

                // list view				
                lvEmployee.BeginUpdate();
                lvEmployee.Columns.Add(rm.GetString("hdrEmployeeID", culture), (lvEmployee.Width - 4) / 4, HorizontalAlignment.Left);
                lvEmployee.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployee.Width - 4) / 4, HorizontalAlignment.Left);
                lvEmployee.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployee.Width - 4) / 4, HorizontalAlignment.Left);
                lvEmployee.Columns.Add(rm.GetString("hdrOrgUnit", culture), (lvEmployee.Width - 4) / 4, HorizontalAlignment.Left);
                lvEmployee.EndUpdate();

                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrEmployeeID", culture), (lvEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrOrgUnit", culture), (lvEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();

                lvSelectedEmployees.BeginUpdate();
                lvSelectedEmployees.Columns.Add(rm.GetString("hdrEmployeeID", culture), (lvSelectedEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvSelectedEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), (lvSelectedEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvSelectedEmployees.Columns.Add(rm.GetString("hdrLastName", culture), (lvSelectedEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvSelectedEmployees.Columns.Add(rm.GetString("hdrOrgUnit", culture), (lvSelectedEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvSelectedEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateEmployeeCombo()
        {
            try
            {
                int ouID = -1;

                if (cbOrgUnit.SelectedIndex > 0)
                {
                    ouID = (int)cbOrgUnit.SelectedValue;
                }
                                
                Employee empl = new Employee();
                empl.EmplTO.OrgUnitID = ouID;
                List<EmployeeTO> emplArray = empl.Search();
                List<EmployeeTO> emplList = new List<EmployeeTO>();
                foreach (EmployeeTO employee in emplArray)
                {
                    if (!employee.Status.Trim().ToUpper().Equals(Constants.statusActive.Trim().ToUpper()) && !employee.Status.Trim().ToUpper().Equals(Constants.statusBlocked.Trim().ToUpper()))
                        continue;

                    employee.LastName += " " + employee.FirstName;
                    emplList.Add(employee);
                }

                EmployeeTO emplAll = new EmployeeTO();
                emplAll.LastName = rm.GetString("all", culture);
                emplList.Insert(0, emplAll);

                cbEmployee.DataSource = emplList;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "OrgUnitID";
                cbEmployee.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.populateEmployeeCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateOrgUnitCombo()
        {
            try
            {
                OrganizationalUnit orgUnit = new OrganizationalUnit();
                orgUnit.OrgUnitTO.Status = Constants.DefaultStateActive;
                orgUnitArray = orgUnit.Search();
                orgUnitArray.Insert(0, new OrganizationalUnitTO(-1, -1, "", rm.GetString("all", culture), ""));

                cbOrgUnit.DataSource = orgUnitArray;
                cbOrgUnit.DisplayMember = "Name";
                cbOrgUnit.ValueMember = "OrgUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.populateOrgUnitCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateOUCombo(ComboBox cb)
        {
            try
            {
                OrganizationalUnit ou = new OrganizationalUnit();
                ou.OrgUnitTO.Status = Constants.DefaultStateActive;
                ouArray = ou.Search();
                ouArray.Insert(0, new OrganizationalUnitTO(-1, -1, "", rm.GetString("all", culture), ""));

                cb.DataSource = ouArray;
                cb.DisplayMember = "Name";
                cb.ValueMember = "OrgUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.populateOUCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void clearListView(ListView lvName)
        {
            try
            {
                lvName.BeginUpdate();
                lvName.Items.Clear();
                lvName.EndUpdate();
                lvName.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.clearListView(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateEmployeeListView(List<EmployeeTO> employeeList, ListView lvName)
        {
            try
            {
                lvName.BeginUpdate();
                lvName.Items.Clear();

                if (employeeList.Count > 0)
                {
                    foreach (EmployeeTO employee in employeeList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = employee.EmployeeID.ToString().Trim();
                        item.SubItems.Add(employee.FirstName.Trim());
                        item.SubItems.Add(employee.LastName.Trim());
                        if (ouDict.ContainsKey(employee.OrgUnitID))
                            item.SubItems.Add(ouDict[employee.OrgUnitID].Name.Trim());
                        else
                            item.SubItems.Add("");

                        lvName.Items.Add(item);
                    }
                }

                lvName.EndUpdate();
                lvName.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.populateEmployeeListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void EmployeesXOrganizationalUnits_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Initialize comparer objects
                _comp1 = new ListViewItemComparer(lvEmployee);
                lvEmployee.ListViewItemSorter = _comp1;
                lvEmployee.Sorting = SortOrder.Ascending;

                _comp2 = new ListViewItemComparer(lvEmployees);
                lvEmployees.ListViewItemSorter = _comp2;
                lvEmployees.Sorting = SortOrder.Ascending;

                _comp3 = new ListViewItemComparer(lvSelectedEmployees);
                lvSelectedEmployees.ListViewItemSorter = _comp3;
                lvSelectedEmployees.Sorting = SortOrder.Ascending;

                ouDict = new OrganizationalUnit().SearchDictionary();

                populateOrgUnitCombo();
                populateEmployeeCombo();
                populateOUCombo(cbOU);
                populateOUCombo(cbOUName);

                //BLOKIRANJE -> status najnoviji ODBLOKIRANO 18.01.2018
                /*
                btnAdd.Enabled = false;
                btnAddAll.Enabled = false;
                btnRemove.Enabled = false;
                btnRemoveAll.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;*/
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.EmployeesXOrganizationalUnits_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }            
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbOU.SelectedIndex > 0)
                {
                    removedEmployees.Clear();
                    addedEmployees.Clear();
                    cbOU.SelectedIndex = 0;

                    MessageBox.Show(rm.GetString("EmployeeXOUCancelChanges", culture));
                }
                else
                {
                    removedEmployees.Clear();
                    addedEmployees.Clear();

                    clearListView(lvSelectedEmployees);
                    clearListView(lvEmployees);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbOrgUnit_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbOrgUnit.SelectedIndex > 0)
                {
                    OrganizationalUnitTO orgUnit = new OrganizationalUnitTO();
                    if (ouDict.ContainsKey((int)cbOrgUnit.SelectedValue))
                        orgUnit = ouDict[(int)cbOrgUnit.SelectedValue];

                    if (orgUnit.OrgUnitID != -1)
                    {
                        tbOrgUnitDesc.Text = orgUnit.Desc.Trim();
                        populateEmployeeCombo();
                    }
                }
                else
                {
                    tbOrgUnitDesc.Text = "";
                    populateEmployeeCombo();
                }
            }
            catch (Exception ex)
            {                
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.cbOrgUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbEmployee_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbEmployee.SelectedIndex > 0)
                {
                    OrganizationalUnitTO orgUnit = new OrganizationalUnitTO();
                    if (ouDict.ContainsKey((int)cbEmployee.SelectedValue))
                        orgUnit = ouDict[(int)cbEmployee.SelectedValue];

                    if (orgUnit.OrgUnitID != -1)
                    {
                        tbSelOUName.Text = orgUnit.Name.Trim();
                        tbSelOUDesc.Text = orgUnit.Desc.Trim();
                    }
                }
                else
                {
                    tbSelOUName.Text = "";
                    tbSelOUDesc.Text = "";
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.cbEmployee_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbOUName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbOUName.SelectedIndex > 0)
                {
                    OrganizationalUnitTO orgUnit = new OrganizationalUnitTO();
                    if (ouDict.ContainsKey((int)cbOUName.SelectedValue))
                        orgUnit = ouDict[(int)cbOUName.SelectedValue];
                    if (orgUnit.OrgUnitID != -1)
                    {
                        Employee empl = new Employee();
                        empl.EmplTO.OrgUnitID = orgUnit.OrgUnitID;
                        List<EmployeeTO> emplArray = empl.Search();
                        List<EmployeeTO> emplList = new List<EmployeeTO>();
                        foreach (EmployeeTO employee in emplArray)
                        {
                            if (!employee.Status.Trim().ToUpper().Equals(Constants.statusActive.Trim().ToUpper()) && !employee.Status.Trim().ToUpper().Equals(Constants.statusBlocked.Trim().ToUpper()))
                                continue;
                            
                            emplList.Add(employee);
                        }

                        tbDesc.Text = orgUnit.Desc.Trim();
                        populateEmployeeListView(emplList, lvEmployee);
                    }
                }
                else
                {
                    tbDesc.Text = "";
                    clearListView(lvEmployee);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.cbOUName_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbOU_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (removedEmployees.Count > 0 || addedEmployees.Count > 0)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("EmployeeXOUSaveChanges", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        saveForPrevIndex = true;
                        btnSave.PerformClick();
                    }
                }

                if (cbOU.SelectedIndex > 0)
                {
                    OrganizationalUnitTO orgUnit = new OrganizationalUnitTO();
                    if (ouDict.ContainsKey((int)cbOU.SelectedValue))
                        orgUnit = ouDict[(int)cbOU.SelectedValue];
                    if (orgUnit.OrgUnitID != -1)
                    {
                        tbOUDesc.Text = orgUnit.Desc.Trim();

                        Employee empl = new Employee();
                        empl.EmplTO.OrgUnitID = orgUnit.OrgUnitID;
                        List<EmployeeTO> emplArray = empl.Search();
                        List<EmployeeTO> emplInOUList = new List<EmployeeTO>();
                        List<int> emplOUIDs = new List<int>();
                        foreach (EmployeeTO employee in emplArray)
                        {
                            if (!employee.Status.Trim().ToUpper().Equals(Constants.statusActive.Trim().ToUpper()) && !employee.Status.Trim().ToUpper().Equals(Constants.statusBlocked.Trim().ToUpper()))
                                continue;

                            emplInOUList.Add(employee);
                            emplOUIDs.Add(employee.EmployeeID);                                
                        }

                        populateEmployeeListView(emplInOUList, lvSelectedEmployees);

                        List<EmployeeTO> emplList = new Employee().Search();
                        List<EmployeeTO> emplNotInOU = new List<EmployeeTO>();
                        foreach (EmployeeTO employee in emplList)
                        {
                            if (!employee.Status.Trim().ToUpper().Equals(Constants.statusActive.Trim().ToUpper()) && !employee.Status.Trim().ToUpper().Equals(Constants.statusBlocked.Trim().ToUpper()))
                                continue;

                            if (!emplOUIDs.Contains(employee.EmployeeID))
                                emplNotInOU.Add(employee);                            
                        }

                        populateEmployeeListView(emplNotInOU, lvEmployees);
                    }
                }
                else
                {
                    tbOUDesc.Text = "";
                    clearListView(lvSelectedEmployees);
                    clearListView(lvEmployees);
                }

                removedEmployees.Clear();
                addedEmployees.Clear();
                if (cbOU.SelectedIndex != 0)
                    prevSelValuecbOU = cbOU.SelectedValue.ToString();
                else
                    prevSelValuecbOU = "ALL";
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                foreach (ListViewItem item in lvEmployees.SelectedItems)
                {
                    lvEmployees.Items.Remove(item);
                    lvSelectedEmployees.Items.Add(item);
                    addedEmployees.Add(item);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAddAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                foreach (ListViewItem item in lvEmployees.Items)
                {
                    lvEmployees.Items.Remove(item);
                    lvSelectedEmployees.Items.Add(item);
                    addedEmployees.Add(item);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.btnAddAll_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRemoveAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                foreach (ListViewItem item in lvSelectedEmployees.Items)
                {
                    lvSelectedEmployees.Items.Remove(item);
                    lvEmployees.Items.Add(item);
                    removedEmployees.Add(item);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.btnRemoveAll_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRemove_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                foreach (ListViewItem item in lvSelectedEmployees.SelectedItems)
                {
                    lvSelectedEmployees.Items.Remove(item);
                    lvEmployees.Items.Add(item);
                    removedEmployees.Add(item);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.btnRemove_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvEmployee_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvEmployee.Sorting;
                //lvEmployee.Sorting = SortOrder.None;

                if (e.Column == _comp1.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvEmployee.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvEmployee.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp1.SortColumn = e.Column;
                    lvEmployee.Sorting = SortOrder.Ascending;
                }

                lvEmployee.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.lvEmployee_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvEmployees_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvEmployees.Sorting;
                //lvEmployees.Sorting = SortOrder.None;

                if (e.Column == _comp2.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvEmployees.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvEmployees.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp2.SortColumn = e.Column;
                    lvEmployees.Sorting = SortOrder.Ascending;
                }

                lvEmployees.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.lvEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvSelectedEmployees_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvSelectedEmployees.Sorting;
                //lvSelectedEmployees.Sorting = SortOrder.None;

                if (e.Column == _comp3.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvSelectedEmployees.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvSelectedEmployees.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp3.SortColumn = e.Column;
                    lvSelectedEmployees.Sorting = SortOrder.Ascending;
                }

                lvSelectedEmployees.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.lvSelectedEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void EmployeesXWorkingUnits_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.EmployeesXWorkingUnits_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnOUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsOrgUnits = new OrganizationalUnit().getOrganizationUnits("");
                WorkingUnitsTreeView orgUnitsTreeView = new WorkingUnitsTreeView(dsOrgUnits);
                orgUnitsTreeView.ShowDialog();
                if (!orgUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbOrgUnit.SelectedIndex = cbOrgUnit.FindStringExact(orgUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.btnOUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnOUTree1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsOrgUnits = new OrganizationalUnit().getOrganizationUnits("");
                WorkingUnitsTreeView orgUnitsTreeView = new WorkingUnitsTreeView(dsOrgUnits);
                orgUnitsTreeView.ShowDialog();
                if (!orgUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbOU.SelectedIndex = cbOU.FindStringExact(orgUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.btnOUTree1_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if ((saveForPrevIndex && !prevSelValuecbOU.ToUpper().Equals("ALL")) || (cbOU.SelectedIndex > 0))
                {
                    bool isUpdated = true;
                    Employee employee = new Employee();

                    isUpdated = employee.BeginTransaction();

                    if (isUpdated)
                    {
                        try
                        {
                            /*foreach(ListViewItem item in removedEmployees)
                            {
                                //change it to 0
                            }*/
                            string ouID;
                            if (saveForPrevIndex)
                                ouID = prevSelValuecbOU;
                            else
                                ouID = cbOU.SelectedValue.ToString();

                            foreach (ListViewItem item in addedEmployees)
                            {
                                isUpdated = employee.UpdateOU(item.Text.Trim(), ouID, false) && isUpdated;
                            }

                            if (isUpdated)
                            {
                                employee.CommitTransaction();

                                //to refresh cbEmployee combo (to assign new Access group ID to each employee)
                                //and to refresh tbSelAccessGroupName and tbSelAccessGroupDesc
                                int employeeSelIndex = cbEmployee.SelectedIndex;
                                cbOrgUnit_SelectedIndexChanged(sender, e);
                                cbEmployee.SelectedIndex = employeeSelIndex;
                                //to refresh lvEmployee list
                                cbOUName_SelectedIndexChanged(sender, e);
                                
                                MessageBox.Show(rm.GetString("EmployeeXWorkingUnitSaved", culture));
                                removedEmployees.Clear();
                                addedEmployees.Clear();
                                //this.Close();
                            }
                            else
                            {
                                MessageBox.Show(rm.GetString("EmployeeWorkingUnitNotSaved", culture));
                                employee.RollbackTransaction();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (employee.GetTransaction() != null)
                                employee.RollbackTransaction();

                            throw ex;
                        }
                    }
                    else
                        MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                }
                else                
                    MessageBox.Show(rm.GetString("SelectOU", culture));                
                
                saveForPrevIndex = false;
            }
            catch (Exception ex)
            {                
                log.writeLog(DateTime.Now + " EmployeesXOrganizationalUnits.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
