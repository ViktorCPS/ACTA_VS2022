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
    public partial class EmployeesResponsibilities : Form
    {
        ApplUserTO logInUser;

        private CultureInfo culture;
        ResourceManager rm;
        DebugLog log;
                
        private ListViewItemComparer _comp1;
        private ListViewItemComparer _comp2;
        private ListViewItemComparer _comp3;
        private ListViewItemComparer _comp4;
        private ListViewItemComparer _comp5;
        
        private const int LastNameIndex = 0;
        private const int FirstNameIndex = 1;
        private const int UserIDIndex = 2;

        private const int wuType = 0;
        private const int ouType = 1;

        int startIndex = 0;
        int emplSortFiled;
        int emplSortOrder;

        private List<EmployeeResponsibilityTO> originalSelectedResponsibility = new List<EmployeeResponsibilityTO>();
        private List<EmployeeResponsibilityTO> removedEmplResponsibility = new List<EmployeeResponsibilityTO>();
        private List<EmployeeResponsibilityTO> addedEmplResponsibility = new List<EmployeeResponsibilityTO>();
                        
        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private List<WorkingUnitTO> wUnitsCombo = new List<WorkingUnitTO>();
        private string wuString = "";
        private List<int> wuList = new List<int>();

        private Dictionary<int, OrganizationalUnitTO> oUnits = new Dictionary<int, OrganizationalUnitTO>();
        private List<OrganizationalUnitTO> oUnitsList = new List<OrganizationalUnitTO>();
        private List<OrganizationalUnitTO> oUnitsComboList = new List<OrganizationalUnitTO>();
        private string ouString = "";
        private List<int> ouList = new List<int>();

        Dictionary<int, WorkingUnitTO> wuDict = new Dictionary<int, WorkingUnitTO>();
        Dictionary<int, OrganizationalUnitTO> ouDict = new Dictionary<int, OrganizationalUnitTO>();

        List<EmployeeTO> emplList = new List<EmployeeTO>();
        List<EmployeeTO> emplComboList = new List<EmployeeTO>();
        Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
        Dictionary<int, EmployeeAsco4TO> ascoDict = new Dictionary<int, EmployeeAsco4TO>();
        Dictionary<string, ApplUserTO> userDict = new Dictionary<string, ApplUserTO>();

        private int previewUnitType;
        private int settingsUnitType;

        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;
        string menuItemID;
        bool addPermission = false;
        bool updatePermission = false;
        bool deletePermission = false;
        
        public EmployeesResponsibilities()
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(EmployeesResponsibilities).Assembly);
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
                return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);                
            }
        }

        private class ArrayListSort : IComparer<EmployeeTO>
        {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(EmployeeTO x, EmployeeTO y)
            {
                EmployeeTO empl1 = null;
                EmployeeTO empl2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    empl1 = x;
                    empl2 = y;
                }
                else
                {
                    empl1 = y;
                    empl2 = x;
                }

                switch (compField)
                {
                    case EmployeesResponsibilities.LastNameIndex:
                        return empl1.LastName.ToUpper().CompareTo(empl2.LastName.ToUpper());
                    case EmployeesResponsibilities.FirstNameIndex:
                        return empl1.FirstName.ToUpper().CompareTo(empl2.FirstName.ToUpper());
                    case EmployeesResponsibilities.UserIDIndex:
                        {
                            string user1 = "";
                            string user2 = "";
                            if (empl1.Tag != null && empl1.Tag is string)
                                user1 = empl1.Tag.ToString().Trim();
                            if (empl2.Tag != null && empl2.Tag is string)
                                user2 = empl2.Tag.ToString().Trim();
                            return user1.ToUpper().CompareTo(user2.ToUpper());
                        }
                    default:
                        return empl1.LastName.CompareTo(empl2.LastName);
                }
            }
        }

        #endregion

        private void EmployeesResponsibilities_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer objects
                _comp1 = new ListViewItemComparer(lvWU);
                lvWU.ListViewItemSorter = _comp1;
                lvWU.Sorting = SortOrder.Ascending;

                _comp2 = new ListViewItemComparer(lvOU);
                lvOU.ListViewItemSorter = _comp2;
                lvOU.Sorting = SortOrder.Ascending;

                _comp3 = new ListViewItemComparer(lvResEmployees);
                lvResEmployees.ListViewItemSorter = _comp3;
                lvResEmployees.Sorting = SortOrder.Ascending;

                _comp4 = new ListViewItemComparer(lvUnits);
                lvUnits.ListViewItemSorter = _comp4;
                lvUnits.Sorting = SortOrder.Ascending;
                
                _comp5 = new ListViewItemComparer(lvResponsibilities);
                lvResponsibilities.ListViewItemSorter = _comp5;
                lvResponsibilities.Sorting = SortOrder.Ascending;

                emplSortFiled = LastNameIndex;
                emplSortOrder = Constants.sortAsc;

                wuDict = new WorkingUnit().getWUDictionary();
                ouDict = new OrganizationalUnit().SearchDictionary();
                                
                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                    oUnits = new ApplUserXOrgUnit().FindOUForUserDictionary(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                }

                WorkingUnitTO wuAll = new WorkingUnitTO();
                wuAll.Name = rm.GetString("all", culture);
                wUnitsCombo.Add(wuAll);
                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                    wuList.Add(wUnit.WorkingUnitID);
                    wUnitsCombo.Add(wUnit);
                }

                if (wuString.Length > 0)
                    wuString = wuString.Substring(0, wuString.Length - 1);

                OrganizationalUnitTO ouAll = new OrganizationalUnitTO();
                ouAll.Name = rm.GetString("all", culture);
                oUnitsComboList.Add(ouAll);
                foreach (int id in oUnits.Keys)
                {
                    ouString += id.ToString().Trim() + ",";
                    ouList.Add(id);
                    oUnitsList.Add(oUnits[id]);
                    oUnitsComboList.Add(oUnits[id]);
                }

                if (ouString.Length > 0)
                    ouString = ouString.Substring(0, ouString.Length - 1);

                // get employees and asco data
                emplDict = new Employee().SearchDictionary();

                List<EmployeeTO> employeeList = new Employee().SearchByWU(wuString);

                EmployeeTO emplAll = new EmployeeTO();
                emplAll.LastName = rm.GetString("all", culture);
                emplComboList.Add(emplAll);
                string emplIDs = "";
                foreach (EmployeeTO empl in employeeList)
                {
                    if (empl.Status.Trim().ToUpper().Equals(Constants.statusRetired.Trim().ToUpper()))
                        continue;

                    emplList.Add(empl);
                    emplComboList.Add(empl);

                    emplIDs += empl.EmployeeID.ToString().Trim() + ",";
                }

                if (emplIDs.Length > 0)
                {
                    ascoDict = new EmployeeAsco4().SearchDictionary(emplIDs.Substring(0, emplIDs.Length - 1));
                }

                // get users dictionary
                userDict = new ApplUser().SearchDictionary();

                // set userID to each employee
                foreach (EmployeeTO empl in emplList)
                {
                    if (ascoDict.ContainsKey(empl.EmployeeID) && userDict.ContainsKey(ascoDict[empl.EmployeeID].NVarcharValue5.Trim()))
                        empl.Tag = ascoDict[empl.EmployeeID].NVarcharValue5.Trim();
                    else
                        empl.Tag = "";
                }

                emplList.Sort(new ArrayListSort(emplSortOrder, emplSortFiled));

                populateEmplCombo();                
                populateEmployees();
                
                rbWU.Checked = true;
                rbWU_CheckedChanged(this, new EventArgs());
                rbWUType.Checked = true;
                rbWUType_CheckedChanged(this, new EventArgs());

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();
                
                setVisibility();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.EmployeesResponsibilities_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        public void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("EmployeeResponisibilites", culture);

                // TabPage text
                tabPreview.Text = rm.GetString("tabPreview", culture);
                tabSettings.Text = rm.GetString("tabSettings", culture);
                
                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);                
                btnSave.Text = rm.GetString("btnSave", culture);
                btnRemove.Text = rm.GetString("btnRemoveSel", culture);                

                // label's text                
                lblEmpl.Text = rm.GetString("lblEmployee", culture);
                lblEmplID.Text = rm.GetString("lblEmplID", culture);
                lblUnit.Text = rm.GetString("lblUnit", culture);
                lblUnits.Text = rm.GetString("lblUnits", culture);
                lblEmployees.Text = rm.GetString("lblEmployees", culture);
                lblWUForEmpl.Text = rm.GetString("lblWUForEmpl", culture);
                lblOUForEmpl.Text = rm.GetString("lblOUForEmpl", culture);
                lblEmplForUnit.Text = rm.GetString("lblEmplForUnit", culture);

                // radio button text
                rbWU.Text = rm.GetString("rbWU", culture);
                rbOU.Text = rm.GetString("rbOU", culture);
                rbWUType.Text = rm.GetString("rbWU", culture);
                rbOUType.Text = rm.GetString("rbOU", culture);
                
                // group boxes
                gbType.Text = rm.GetString("gbType", culture);
                gbUnitPreview.Text = rm.GetString("gbUnitPreview", culture);
                gbUnitType.Text = rm.GetString("gbType", culture);
                gbEmplPreview.Text = rm.GetString("gbEmplPreview", culture);

                // list view
                lvWU.BeginUpdate();
                lvWU.Columns.Add(rm.GetString("hdrName", culture), lvWU.Width / 2 - 10, HorizontalAlignment.Left);
                lvWU.Columns.Add(rm.GetString("hdrDescription", culture), lvWU.Width / 2 - 10, HorizontalAlignment.Left);
                lvWU.EndUpdate();

                lvOU.BeginUpdate();
                lvOU.Columns.Add(rm.GetString("hdrName", culture), lvOU.Width / 2 - 10, HorizontalAlignment.Left);
                lvOU.Columns.Add(rm.GetString("hdrDescription", culture), lvOU.Width / 2 - 10, HorizontalAlignment.Left);
                lvOU.EndUpdate();

                lvResEmployees.BeginUpdate();
                lvResEmployees.Columns.Add(rm.GetString("hdrLastName", culture), lvResEmployees.Width / 3 - 7, HorizontalAlignment.Left);
                lvResEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), lvResEmployees.Width / 3 - 7, HorizontalAlignment.Left);
                lvResEmployees.Columns.Add(rm.GetString("hdrUserID", culture), lvResEmployees.Width / 3 - 7, HorizontalAlignment.Left);
                lvResEmployees.EndUpdate();

                lvUnits.BeginUpdate();
                lvUnits.Columns.Add(rm.GetString("hdrName", culture), lvUnits.Width / 2 - 10, HorizontalAlignment.Left);
                lvUnits.Columns.Add(rm.GetString("hdrDescription", culture), lvUnits.Width / 2 - 10, HorizontalAlignment.Left);
                lvUnits.EndUpdate();
                
                lvEmployees.BeginUpdate();                
                lvEmployees.Columns.Add(rm.GetString("hdrLastName", culture), lvEmployees.Width / 3 - 7, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), lvEmployees.Width / 3 - 7, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrUserID", culture), lvEmployees.Width / 3 - 7, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();

                lvResponsibilities.BeginUpdate();
                lvResponsibilities.Columns.Add(rm.GetString("hdrUserID", culture), lvResponsibilities.Width / 5 - 4, HorizontalAlignment.Left);
                lvResponsibilities.Columns.Add(rm.GetString("hdrLastName", culture), lvResponsibilities.Width / 5 - 4, HorizontalAlignment.Left);
                lvResponsibilities.Columns.Add(rm.GetString("hdrFirstName", culture), lvResponsibilities.Width / 5 - 4, HorizontalAlignment.Left);
                lvResponsibilities.Columns.Add(rm.GetString("hdrType", culture), lvResponsibilities.Width / 5 - 4, HorizontalAlignment.Left);
                lvResponsibilities.Columns.Add(rm.GetString("hdrUnit", culture), lvResponsibilities.Width / 5 - 4, HorizontalAlignment.Left);
                lvResponsibilities.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.setLanguage(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void populateEmployees()
        {
            try
            {
                if (emplList.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();

                if (emplList.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < emplList.Count))
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
                        if (lastIndex >= emplList.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = emplList.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            EmployeeTO employee = emplList[i];
                            ListViewItem item = new ListViewItem();
                                                        
                            item.Text = employee.LastName.Trim();
                            item.SubItems.Add(employee.FirstName.Trim());
                            if (employee.Tag != null && employee.Tag is string)
                                item.SubItems.Add(employee.Tag.ToString());
                            else
                                item.SubItems.Add("");
                            item.ToolTipText = employee.EmployeeID.ToString();
                            item.Tag = employee;
                            lvEmployees.Items.Add(item);                            
                        }
                    }
                }

                lvEmployees.EndUpdate();
                lvEmployees.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateWU(List<EmployeeResponsibilityTO> resList)
        {
            try
            {
                lvWU.BeginUpdate();
                lvWU.Items.Clear();

                foreach (EmployeeResponsibilityTO resTO in resList)
                {
                    if (resTO.Type.Trim().ToUpper().Equals(Constants.emplResTypeWU.Trim().ToUpper()))
                    {
                        WorkingUnitTO wuTO = new WorkingUnitTO();

                        if (wuDict.ContainsKey(resTO.UnitID))
                            wuTO = wuDict[resTO.UnitID];

                        ListViewItem item = new ListViewItem();
                        item.Text = wuTO.Name.Trim();
                        item.SubItems.Add(wuTO.Description.Trim());
                        item.ToolTipText = wuTO.WorkingUnitID.ToString().Trim();
                        lvWU.Items.Add(item);
                    }
                }

                lvWU.EndUpdate();
                lvWU.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.populateWU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateOU(List<EmployeeResponsibilityTO> resList)
        {
            try
            {
                lvOU.BeginUpdate();
                lvOU.Items.Clear();

                foreach (EmployeeResponsibilityTO resTO in resList)
                {
                    if (resTO.Type.Trim().ToUpper().Equals(Constants.emplResTypeOU.Trim().ToUpper()))
                    {
                        OrganizationalUnitTO ouTO = new OrganizationalUnitTO();

                        if (ouDict.ContainsKey(resTO.UnitID))
                            ouTO = ouDict[resTO.UnitID];

                        ListViewItem item = new ListViewItem();
                        item.Text = ouTO.Name.Trim();
                        item.SubItems.Add(ouTO.Desc.Trim());
                        item.ToolTipText = ouTO.OrgUnitID.ToString().Trim();
                        lvOU.Items.Add(item);
                    }
                }

                lvOU.EndUpdate();
                lvOU.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.populateOU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateEmplCombo()
        {
            try
            {                
                cbEmpl.DataSource = emplComboList;
                cbEmpl.DisplayMember = "FirstAndLastName";
                cbEmpl.ValueMember = "EmployeeID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.populateEmplCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateUnitCombo()
        {
            try
            {
                if (previewUnitType == wuType)
                {
                    cbUnit.DataSource = wUnitsCombo;
                    cbUnit.DisplayMember = "Name";
                    cbUnit.ValueMember = "WorkingUnitID";
                }
                else
                {
                    cbUnit.DataSource = oUnitsComboList;
                    cbUnit.DisplayMember = "Name";
                    cbUnit.ValueMember = "OrgUnitID";
                }

                cbUnit.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.populateUnitCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateResEmployees(List<EmployeeResponsibilityTO> resList)
        {
            try
            {
                lvResEmployees.BeginUpdate();
                lvResEmployees.Items.Clear();

                foreach (EmployeeResponsibilityTO resTO in resList)
                {
                    EmployeeTO employee = new EmployeeTO();

                    if (emplDict.ContainsKey(resTO.EmployeeID))
                    {
                        employee = emplDict[resTO.EmployeeID];

                        if (ascoDict.ContainsKey(resTO.EmployeeID) && userDict.ContainsKey(ascoDict[resTO.EmployeeID].NVarcharValue5.Trim()))
                            employee.Tag = ascoDict[resTO.EmployeeID].NVarcharValue5.Trim();
                    }

                    ListViewItem item = new ListViewItem();

                    item.Text = employee.LastName.Trim();
                    item.SubItems.Add(employee.FirstName.Trim());
                    if (employee.Tag != null && employee.Tag is string)
                        item.SubItems.Add(employee.Tag.ToString());
                    else
                        item.SubItems.Add("");
                    item.ToolTipText = employee.EmployeeID.ToString();
                    item.Tag = employee;
                    lvResEmployees.Items.Add(item);
                }

                lvResEmployees.EndUpdate();
                lvResEmployees.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.populateResEmployees(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateUnits()
        {
            try
            {
                lvUnits.BeginUpdate();
                lvUnits.Items.Clear();

                if (settingsUnitType == wuType)
                {
                    foreach (WorkingUnitTO wuTO in wUnits)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = wuTO.Name.Trim();
                        item.SubItems.Add(wuTO.Description.Trim());
                        item.ToolTipText = wuTO.WorkingUnitID.ToString().Trim();
                        item.Tag = wuTO;
                        lvUnits.Items.Add(item);
                    }
                }
                else
                {
                    foreach (OrganizationalUnitTO ouTO in oUnitsList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = ouTO.Name.Trim();
                        item.SubItems.Add(ouTO.Desc.Trim());
                        item.ToolTipText = ouTO.OrgUnitID.ToString().Trim();
                        item.Tag = ouTO;
                        lvUnits.Items.Add(item);
                    }
                }

                lvUnits.EndUpdate();
                lvUnits.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.populateUnits(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateResponsibilities()
        {
            try
            {
                lvResponsibilities.BeginUpdate();
                lvResponsibilities.Items.Clear();

                string unitIDs = "";                

                foreach (ListViewItem item in lvUnits.SelectedItems)
                {
                    if (settingsUnitType == wuType)
                        unitIDs += ((WorkingUnitTO)item.Tag).WorkingUnitID.ToString().Trim() + ",";
                    else
                        unitIDs += ((OrganizationalUnitTO)item.Tag).OrgUnitID.ToString().Trim() + ",";
                }

                if (unitIDs.Length > 0)
                    unitIDs = unitIDs.Substring(0, unitIDs.Length - 1);

                if (unitIDs.Length > 0)
                {
                    string type = Constants.emplResTypeWU;
                    if (settingsUnitType == ouType)
                        type = Constants.emplResTypeOU;
                    originalSelectedResponsibility = new EmployeeResponsibility().Search(unitIDs, type);
                }
                else
                    originalSelectedResponsibility = new List<EmployeeResponsibilityTO>();

                foreach (EmployeeResponsibilityTO resTO in originalSelectedResponsibility)
                {
                    ListViewItem item = new ListViewItem();

                    if (ascoDict.ContainsKey(resTO.EmployeeID) && userDict.ContainsKey(ascoDict[resTO.EmployeeID].NVarcharValue5.Trim()))
                        item.Text = ascoDict[resTO.EmployeeID].NVarcharValue5.Trim();
                    else
                        item.Text = "";
                    if (emplDict.ContainsKey(resTO.EmployeeID))
                    {
                        item.SubItems.Add(emplDict[resTO.EmployeeID].LastName.Trim());
                        item.SubItems.Add(emplDict[resTO.EmployeeID].FirstName.Trim());
                        item.ToolTipText = emplDict[resTO.EmployeeID].EmployeeID.ToString().Trim();
                    }
                    else
                    {
                        item.SubItems.Add("");
                        item.SubItems.Add("");
                    }

                    item.SubItems.Add(resTO.Type.Trim());

                    if (resTO.Type.Trim().ToUpper().Equals(Constants.emplResTypeWU.Trim().ToUpper()) && wuDict.ContainsKey(resTO.UnitID))
                        item.SubItems.Add(wuDict[resTO.UnitID].Name.Trim());
                    else if (resTO.Type.Trim().ToUpper().Equals(Constants.emplResTypeOU.Trim().ToUpper()) && ouDict.ContainsKey(resTO.UnitID))
                        item.SubItems.Add(ouDict[resTO.UnitID].Name.Trim());
                    else
                        item.SubItems.Add("");

                    item.Tag = resTO;
                    lvResponsibilities.Items.Add(item);
                }

                lvResponsibilities.EndUpdate();
                lvResponsibilities.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.populateResponsibilities(): " + ex.Message + "\n");
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
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.btnPrev_Click(): " + ex.Message + "\n");
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
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbEmpl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                tbEmplID.Text = "";
                if (cbEmpl.SelectedValue != null && cbEmpl.SelectedValue is int)
                {
                    List<EmployeeResponsibilityTO> resList = new List<EmployeeResponsibilityTO>();

                    if ((int)cbEmpl.SelectedValue != -1)
                    {
                        tbEmplID.Text = cbEmpl.SelectedValue.ToString();

                        EmployeeResponsibility emplRes = new EmployeeResponsibility();
                        emplRes.ResponsibilityTO.EmployeeID = (int)cbEmpl.SelectedValue;

                        resList = emplRes.Search();
                    }

                    populateWU(resList);
                    populateOU(resList);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.cbEmpl_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvWU_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvWU.Sorting;

                if (e.Column == _comp1.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvWU.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvWU.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp1.SortColumn = e.Column;
                    lvWU.Sorting = SortOrder.Ascending;
                }

                lvWU.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.lvWU_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvOU_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvOU.Sorting;

                if (e.Column == _comp2.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvOU.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvOU.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp2.SortColumn = e.Column;
                    lvOU.Sorting = SortOrder.Ascending;
                }

                lvOU.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.lvOU_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvEmployees_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int prevOrder = emplSortOrder;

                if (e.Column == emplSortFiled)
                {
                    if (prevOrder == Constants.sortAsc)
                    {
                        emplSortOrder = Constants.sortDesc;
                    }
                    else
                    {
                        emplSortOrder = Constants.sortAsc;
                    }
                }
                else
                {
                    // New Sort Order
                    emplSortOrder = Constants.sortAsc;
                }

                emplSortFiled = e.Column;

                emplList.Sort(new ArrayListSort(emplSortOrder, emplSortFiled));
                startIndex = 0;
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.lvEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbWU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                rbOU.Checked = !rbWU.Checked;

                if (rbWU.Checked)
                {
                    previewUnitType = wuType;
                    populateUnitCombo();

                    List<EmployeeResponsibilityTO> resList = new List<EmployeeResponsibilityTO>();
                    populateResEmployees(resList);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.rbWU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbOU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                rbWU.Checked = !rbOU.Checked;

                if (rbOU.Checked)
                {
                    previewUnitType = ouType;
                    populateUnitCombo();
                    
                    List<EmployeeResponsibilityTO> resList = new List<EmployeeResponsibilityTO>();
                    populateResEmployees(resList);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.rbOU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                                
                if (cbUnit.SelectedValue != null && cbUnit.SelectedValue is int)
                {
                    List<EmployeeResponsibilityTO> resList = new List<EmployeeResponsibilityTO>();

                    if ((int)cbUnit.SelectedValue != -1)
                    {
                        EmployeeResponsibility emplRes = new EmployeeResponsibility();

                        if (previewUnitType == wuType)
                            emplRes.ResponsibilityTO.Type = Constants.emplResTypeWU;
                        else
                            emplRes.ResponsibilityTO.Type = Constants.emplResTypeOU;

                        emplRes.ResponsibilityTO.UnitID = (int)cbUnit.SelectedValue;

                        resList = emplRes.Search();
                    }

                    populateResEmployees(resList);                    
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.cbUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbWUType_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //rbOUType.Checked = !rbWUType.Checked;

                if (rbWUType.Checked)
                {
                    if (addedEmplResponsibility.Count > 0 || removedEmplResponsibility.Count > 0)
                    {
                        DialogResult result = MessageBox.Show(rm.GetString("responsibilityChanged", culture), "", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {
                            btnSave.PerformClick();
                        }
                        else
                        {
                            addedEmplResponsibility = new List<EmployeeResponsibilityTO>();
                            removedEmplResponsibility = new List<EmployeeResponsibilityTO>();
                        }
                    }

                    settingsUnitType = wuType;
                    populateUnits();
                    populateResponsibilities();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.rbWUType_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbOUType_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //rbWUType.Checked = !rbOUType.Checked;

                if (rbOUType.Checked)
                {
                    if (addedEmplResponsibility.Count > 0 || removedEmplResponsibility.Count > 0)
                    {
                        DialogResult result = MessageBox.Show(rm.GetString("responsibilityChanged", culture), "", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {
                            btnSave.PerformClick();
                        }
                        else
                        {
                            addedEmplResponsibility = new List<EmployeeResponsibilityTO>();
                            removedEmplResponsibility = new List<EmployeeResponsibilityTO>();
                        }
                    }

                    settingsUnitType = ouType;
                    populateUnits();
                    populateResponsibilities();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.rbOUType_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvUnits_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvUnits.Sorting;

                if (e.Column == _comp4.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvUnits.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvUnits.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp4.SortColumn = e.Column;
                    lvUnits.Sorting = SortOrder.Ascending;
                }

                lvUnits.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.lvUnits_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvResEmployees_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvResEmployees.Sorting;

                if (e.Column == _comp3.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvResEmployees.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvResEmployees.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp3.SortColumn = e.Column;
                    lvResEmployees.Sorting = SortOrder.Ascending;
                }

                lvResEmployees.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.lvResEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (addedEmplResponsibility.Count > 0 || removedEmplResponsibility.Count > 0)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("responsibilityChanged", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        btnSave.PerformClick();
                    }
                    else
                    {
                        addedEmplResponsibility = new List<EmployeeResponsibilityTO>();
                        removedEmplResponsibility = new List<EmployeeResponsibilityTO>();
                    }
                }

                populateResponsibilities();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.lvUnits_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvResponsibilities_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvResponsibilities.Sorting;

                if (e.Column == _comp5.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvResponsibilities.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvResponsibilities.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp5.SortColumn = e.Column;
                    lvResponsibilities.Sorting = SortOrder.Ascending;
                }

                lvResponsibilities.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.lvResponsibilities_ColumnClick(): " + ex.Message + "\n");
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
                this.Cursor = Cursors.WaitCursor;

                string type = Constants.emplResTypeWU;

                if (settingsUnitType == ouType)
                    type = Constants.emplResTypeOU;

                bool notUserEmplFound = false;
                foreach (ListViewItem unitItem in lvUnits.SelectedItems)
                {
                    int unitID = -1;
                    if (settingsUnitType == wuType)
                        unitID = ((WorkingUnitTO)unitItem.Tag).WorkingUnitID;
                    else
                        unitID = ((OrganizationalUnitTO)unitItem.Tag).OrgUnitID;

                    foreach (ListViewItem emplItem in lvEmployees.SelectedItems)
                    {
                        if (((EmployeeTO)emplItem.Tag).Tag != null && ((EmployeeTO)emplItem.Tag).Tag is string && userDict.ContainsKey(((EmployeeTO)emplItem.Tag).Tag.ToString()))
                        {
                            EmployeeResponsibilityTO addRes = new EmployeeResponsibilityTO();
                            addRes.EmployeeID = ((EmployeeTO)emplItem.Tag).EmployeeID;
                            addRes.Type = type;
                            addRes.UnitID = unitID;

                            bool addItem = false;

                            int origIndex = isListResponsibility(addRes, originalSelectedResponsibility);
                            if (origIndex < 0 && isListResponsibility(addRes, addedEmplResponsibility) < 0)
                            {
                                addedEmplResponsibility.Add(addRes);
                                addItem = true;
                            }
                            else if (origIndex >= 0)
                            {
                                int index = isListResponsibility(addRes, removedEmplResponsibility);

                                if (index >= 0)
                                {
                                    // if removed does not contain this responsibility, do nothing, else, add it to list view and remove it from removed
                                    removedEmplResponsibility.RemoveAt(index);
                                    addItem = true;
                                }
                            }

                            if (addItem)
                            {
                                ListViewItem item = new ListViewItem();

                                if (ascoDict.ContainsKey(addRes.EmployeeID) && userDict.ContainsKey(ascoDict[addRes.EmployeeID].NVarcharValue5.Trim()))
                                    item.Text = ascoDict[addRes.EmployeeID].NVarcharValue5.Trim();
                                else
                                    item.Text = "";
                                if (emplDict.ContainsKey(addRes.EmployeeID))
                                {
                                    item.SubItems.Add(emplDict[addRes.EmployeeID].LastName.Trim());
                                    item.SubItems.Add(emplDict[addRes.EmployeeID].FirstName.Trim());
                                    item.ToolTipText = emplDict[addRes.EmployeeID].EmployeeID.ToString().Trim();
                                }
                                else
                                {
                                    item.SubItems.Add("");
                                    item.SubItems.Add("");
                                }

                                item.SubItems.Add(addRes.Type.Trim());

                                if (addRes.Type.Trim().ToUpper().Equals(Constants.emplResTypeWU.Trim().ToUpper()) && wuDict.ContainsKey(addRes.UnitID))
                                    item.SubItems.Add(wuDict[addRes.UnitID].Name.Trim());
                                else if (addRes.Type.Trim().ToUpper().Equals(Constants.emplResTypeOU.Trim().ToUpper()) && ouDict.ContainsKey(addRes.UnitID))
                                    item.SubItems.Add(ouDict[addRes.UnitID].Name.Trim());
                                else
                                    item.SubItems.Add("");

                                item.Tag = addRes;
                                lvResponsibilities.Items.Add(item);
                                lvResEmployees.Sort();
                            }
                        }
                        else
                            notUserEmplFound = true;
                    }
                }

                if (notUserEmplFound)
                    MessageBox.Show(rm.GetString("notUserEmplResponsibility", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                bool removeUserNotAllowed = false;
                foreach (ListViewItem resItem in lvResponsibilities.SelectedItems)
                {
                    EmployeeResponsibilityTO res = (EmployeeResponsibilityTO)resItem.Tag;

                    if (ascoDict.ContainsKey(res.EmployeeID))
                    {
                        lvResponsibilities.Items.Remove(resItem);

                        if (isListResponsibility((EmployeeResponsibilityTO)resItem.Tag, originalSelectedResponsibility) >= 0)
                            removedEmplResponsibility.Add((EmployeeResponsibilityTO)resItem.Tag);
                        else
                        {
                            int index = isListResponsibility((EmployeeResponsibilityTO)resItem.Tag, addedEmplResponsibility);

                            if (index >= 0)
                                addedEmplResponsibility.RemoveAt(index);
                        }
                    }
                    else
                        removeUserNotAllowed = true;
                }

                if (removeUserNotAllowed)
                    MessageBox.Show(rm.GetString("notAllowedEmplResponsibility", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.btnRemove_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private int isListResponsibility(EmployeeResponsibilityTO resTO, List<EmployeeResponsibilityTO> resList)
        {
            try
            {
                int index = -1;

                for (int i = 0; i < resList.Count; i++)
                {
                    EmployeeResponsibilityTO res = resList[i];
                    if (res.EmployeeID == resTO.EmployeeID && res.Type == resTO.Type && res.UnitID == resTO.UnitID)
                    {
                        index = i;
                        break;
                    }
                }

                return index;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.isListResponsibility(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (addedEmplResponsibility.Count == 0 && removedEmplResponsibility.Count == 0)
                {
                    MessageBox.Show(rm.GetString("noEmplResponsibilityChanges", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                bool savedAll = true;

                EmployeeResponsibility emplRes = new EmployeeResponsibility();
                foreach (EmployeeResponsibilityTO addRes in addedEmplResponsibility)
                {
                    string userID = "";

                    if (ascoDict.ContainsKey(addRes.EmployeeID) && userDict.ContainsKey(ascoDict[addRes.EmployeeID].NVarcharValue5))
                        userID = ascoDict[addRes.EmployeeID].NVarcharValue5;

                    if (userID.Trim().Equals(""))
                    {
                        savedAll = false;
                        log.writeLog(DateTime.Now + " EmployeesResponsibilities.btnSave_Click(): User does not exist for employee or not allowed employee " + addRes.EmployeeID.ToString().Trim() + "\n");                        
                        continue;
                    }

                    // prepare data
                    ApplUsersXWU applUserXwu = new ApplUsersXWU();
                    Dictionary<int, WorkingUnitTO> userWUUnits = applUserXwu.FindWUForUserDictionary(userID, Constants.EmployeesPurpose);

                    ApplUserXOrgUnit applUserXou = new ApplUserXOrgUnit();
                    Dictionary<int, OrganizationalUnitTO> userOUUnits = applUserXou.FindOUForUserDictionary(userID, Constants.EmployeesPurpose);

                    // get responsible unit and all its children
                    WorkingUnitTO wuTO = new WorkingUnitTO();
                    List<WorkingUnitTO> wuUnitsTOSee = new List<WorkingUnitTO>();                    
                    OrganizationalUnitTO ouTO = new OrganizationalUnitTO();
                    List<OrganizationalUnitTO> orgUnitsTOSee = new List<OrganizationalUnitTO>();

                    if (addRes.Type.Trim().ToUpper().Equals(Constants.emplResTypeWU.Trim().ToUpper()))
                    {
                        if (wuDict.ContainsKey(addRes.UnitID))
                            wuTO = wuDict[addRes.UnitID];

                        if (wuTO.WorkingUnitID != -1)
                        {
                            wuUnitsTOSee.Add(wuTO);
                            wuUnitsTOSee = new WorkingUnit().FindAllChildren(wuUnitsTOSee);
                        }
                    }
                    else
                    {                                    
                        if (ouDict.ContainsKey(addRes.UnitID))
                            ouTO = ouDict[addRes.UnitID];

                        if (ouTO.OrgUnitID != -1)
                        {                            
                            orgUnitsTOSee.Add(ouTO);
                            orgUnitsTOSee = new OrganizationalUnit().FindAllChildren(orgUnitsTOSee);
                        }
                    }

                    emplRes.ResponsibilityTO = addRes;
                    if (emplRes.BeginTransaction())
                    {
                        bool saved = true;

                        try
                        {
                            // save new responsibility
                            saved = emplRes.insert(false);

                            if (saved)
                            {
                                if (addRes.Type.Trim().ToUpper().Equals(Constants.emplResTypeWU.Trim().ToUpper()))
                                {
                                    // set WU visibility
                                    List<int> visibleUnitID = new List<int>();
                                    applUserXwu.SetTransaction(emplRes.GetTransaction());
                                    foreach (WorkingUnitTO unit in wuUnitsTOSee)
                                    {
                                        if (!visibleUnitID.Contains(unit.WorkingUnitID) && !userWUUnits.ContainsKey(unit.WorkingUnitID))
                                        {
                                            saved = saved && applUserXwu.Save(userID, unit.WorkingUnitID, Constants.EmployeesPurpose, false) > 0;

                                            if (!saved)
                                                break;

                                            visibleUnitID.Add(unit.WorkingUnitID);
                                        }
                                    }

                                    if (saved)
                                    {
                                        // if there is no default working unit, set it
                                        EmployeeAsco4 asco = new EmployeeAsco4();
                                        if (ascoDict.ContainsKey(addRes.EmployeeID) && ascoDict[addRes.EmployeeID].IntegerValue2 == -1)
                                        {
                                            asco.EmplAsco4TO = ascoDict[addRes.EmployeeID];
                                            asco.EmplAsco4TO.IntegerValue2 = addRes.UnitID;
                                            asco.SetTransaction(emplRes.GetTransaction());
                                            saved = saved && asco.update(false);
                                        }
                                    }
                                }
                                else
                                {
                                    // set OU visibility
                                    List<int> visibleUnitID = new List<int>();
                                    applUserXou.SetTransaction(emplRes.GetTransaction());
                                    foreach (OrganizationalUnitTO unit in orgUnitsTOSee)
                                    {
                                        if (!visibleUnitID.Contains(unit.OrgUnitID) && !userOUUnits.ContainsKey(unit.OrgUnitID))
                                        {
                                            saved = saved && applUserXou.Save(userID, unit.OrgUnitID, Constants.EmployeesPurpose, false) > 0;

                                            if (!saved)
                                                break;

                                            visibleUnitID.Add(unit.OrgUnitID);
                                        }
                                    }

                                    if (saved)
                                    {
                                        // if there is no default organizational unit, set it
                                        EmployeeAsco4 asco = new EmployeeAsco4();

                                        if (ascoDict.ContainsKey(addRes.EmployeeID) && ascoDict[addRes.EmployeeID].IntegerValue3 == -1)
                                        {
                                            asco.EmplAsco4TO = ascoDict[addRes.EmployeeID];
                                            asco.EmplAsco4TO.IntegerValue3 = addRes.UnitID;
                                            asco.SetTransaction(emplRes.GetTransaction());
                                            saved = saved && asco.update(false);
                                        }
                                    }
                                }
                            }

                            if (saved)
                                emplRes.CommitTransaction();
                            else if (emplRes.GetTransaction() != null)
                                emplRes.RollbackTransaction();
                        }
                        catch (Exception ex)
                        {
                            if (emplRes.GetTransaction() != null)
                                emplRes.RollbackTransaction();

                            log.writeLog(DateTime.Now + " EmployeesResponsibilities.btnSave_Click(): " + ex.Message + "\n");

                            saved = false;
                        }

                        savedAll = savedAll && saved;
                    }
                    else
                        savedAll = false;
                }

                foreach (EmployeeResponsibilityTO remRes in removedEmplResponsibility)
                {
                    string userID = "";

                    if (ascoDict.ContainsKey(remRes.EmployeeID) && userDict.ContainsKey(ascoDict[remRes.EmployeeID].NVarcharValue5))
                        userID = ascoDict[remRes.EmployeeID].NVarcharValue5;

                    if (userID.Trim().Equals(""))
                    {
                        savedAll = false;
                        log.writeLog(DateTime.Now + " EmployeesResponsibilities.btnSave_Click(): User does not exist for employee or not allowed employee " + remRes.EmployeeID.ToString().Trim() + "\n");
                        continue;
                    }

                    // prepare data
                    ApplUsersXWU applUserXwu = new ApplUsersXWU();
                    Dictionary<int, WorkingUnitTO> userWUUnits = applUserXwu.FindWUForUserDictionary(userID, Constants.EmployeesPurpose);

                    ApplUserXOrgUnit applUserXou = new ApplUserXOrgUnit();
                    Dictionary<int, OrganizationalUnitTO> userOUUnits = applUserXou.FindOUForUserDictionary(userID, Constants.EmployeesPurpose);

                    // get responsibilities
                    emplRes.ResponsibilityTO = new EmployeeResponsibilityTO();
                    emplRes.ResponsibilityTO.EmployeeID = remRes.EmployeeID;
                    emplRes.ResponsibilityTO.Type = remRes.Type;
                    List<EmployeeResponsibilityTO> list = emplRes.Search();

                    // get responsible unit and all its children
                    WorkingUnitTO wuTO = new WorkingUnitTO();
                    List<WorkingUnitTO> wuUnitsChildren = new List<WorkingUnitTO>();
                    List<int> wuUnitsChildrenIDs = new List<int>();
                    List<int> wuList = new List<int>();
                    OrganizationalUnitTO ouTO = new OrganizationalUnitTO();
                    List<OrganizationalUnitTO> ouUnitsChildren = new List<OrganizationalUnitTO>();
                    List<int> ouUnitsChildrenIDs = new List<int>();
                    List<int> ouList = new List<int>();

                    int defaultUnitID = -1;

                    if (remRes.Type.Trim().ToUpper().Equals(Constants.emplResTypeWU.Trim().ToUpper()))
                    {
                        if (wuDict.ContainsKey(remRes.UnitID))
                            wuTO = wuDict[remRes.UnitID];

                        if (wuTO.WorkingUnitID != -1)
                        {
                            wuUnitsChildren.Add(wuTO);
                            wuUnitsChildren = new WorkingUnit().FindAllChildren(wuUnitsChildren);

                            foreach (WorkingUnitTO wu in wuUnitsChildren)
                            {
                                wuUnitsChildrenIDs.Add(wu.WorkingUnitID);
                            }
                        }

                        if (!userWUUnits.ContainsKey(wuTO.ParentWorkingUID) || wuTO.WorkingUnitID == wuTO.ParentWorkingUID)
                        {
                            foreach (EmployeeResponsibilityTO oldResp in list)
                            {
                                if (oldResp.UnitID == remRes.UnitID)
                                    continue;

                                if (defaultUnitID == -1)
                                    defaultUnitID = oldResp.UnitID;

                                if (wuUnitsChildrenIDs.Contains(oldResp.UnitID))
                                {
                                    // find all responsible child children
                                    WorkingUnitTO wUnitTO = new WorkingUnitTO();
                                    if (wuDict.ContainsKey(oldResp.UnitID))
                                        wUnitTO = wuDict[oldResp.UnitID];

                                    if (wUnitTO.WorkingUnitID != -1)
                                    {
                                        List<WorkingUnitTO> wuListToSee = new List<WorkingUnitTO>();
                                        wuListToSee.Add(wUnitTO);
                                        wuListToSee = new WorkingUnit().FindAllChildren(wuListToSee);

                                        foreach (WorkingUnitTO unit in wuListToSee)
                                        {
                                            if (!wuList.Contains(unit.WorkingUnitID))
                                                wuList.Add(unit.WorkingUnitID);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (ouDict.ContainsKey(remRes.UnitID))
                            ouTO = ouDict[remRes.UnitID];

                        if (ouTO.OrgUnitID != -1)
                        {
                            ouUnitsChildren.Add(ouTO);
                            ouUnitsChildren = new OrganizationalUnit().FindAllChildren(ouUnitsChildren);

                            foreach (OrganizationalUnitTO ou in ouUnitsChildren)
                            {
                                ouUnitsChildrenIDs.Add(ou.OrgUnitID);
                            }
                        }

                        if (!userOUUnits.ContainsKey(ouTO.ParentOrgUnitID) || ouTO.OrgUnitID == ouTO.ParentOrgUnitID)
                        {
                            foreach (EmployeeResponsibilityTO oldResp in list)
                            {
                                if (oldResp.UnitID == remRes.UnitID)
                                    continue;

                                if (defaultUnitID == -1)
                                    defaultUnitID = oldResp.UnitID;

                                if (ouUnitsChildrenIDs.Contains(oldResp.UnitID))
                                {
                                    // find all responsible child children
                                    OrganizationalUnitTO oUnitTO = new OrganizationalUnitTO();
                                    if (ouDict.ContainsKey(oldResp.UnitID))
                                        oUnitTO = ouDict[oldResp.UnitID];

                                    if (oUnitTO.OrgUnitID != -1)
                                    {
                                        List<OrganizationalUnitTO> ouListToSee = new List<OrganizationalUnitTO>();
                                        ouListToSee.Add(oUnitTO);
                                        ouListToSee = new OrganizationalUnit().FindAllChildren(ouListToSee);

                                        foreach (OrganizationalUnitTO unit in ouListToSee)
                                        {
                                            if (!ouList.Contains(unit.OrgUnitID))
                                                ouList.Add(unit.OrgUnitID);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    emplRes.ResponsibilityTO = remRes;                    
                    if (emplRes.BeginTransaction())
                    {
                        bool saved = true;

                        try
                        {
                            // delete responsibility
                            saved = emplRes.delete(false);

                            if (saved)
                            {
                                if (remRes.Type.Trim().ToUpper().Equals(Constants.emplResTypeWU.Trim().ToUpper()))
                                {                                    
                                    // if there is parent visibility do nothing else delete wu and child visibilities, leave children visibilities if exists children responsibility
                                    if (!userWUUnits.ContainsKey(wuTO.ParentWorkingUID) || wuTO.WorkingUnitID == wuTO.ParentWorkingUID)
                                    {
                                        // delete unit and all it's children visibilities                                        
                                        applUserXwu.SetTransaction(emplRes.GetTransaction());
                                        foreach (WorkingUnitTO child in wuUnitsChildren)
                                        {
                                            if (wuList.Contains(child.WorkingUnitID))
                                                continue;                                            
                                                                                        
                                            applUserXwu.AuXWUnitTO = new ApplUsersXWUTO();
                                            saved = saved && applUserXwu.Delete(userID, child.WorkingUnitID, Constants.EmployeesPurpose, false);

                                            if (!saved)
                                                break;
                                        }

                                        if (saved)
                                        {
                                            //set first working unit to default one                                            
                                            EmployeeAsco4 asco = new EmployeeAsco4();
                                            if (ascoDict.ContainsKey(remRes.EmployeeID) && ascoDict[remRes.EmployeeID].IntegerValue2 == remRes.UnitID)
                                            {
                                                asco.EmplAsco4TO = ascoDict[remRes.EmployeeID];
                                                asco.EmplAsco4TO.IntegerValue2 = defaultUnitID;
                                                asco.SetTransaction(emplRes.GetTransaction());
                                                saved = saved && asco.update(false);
                                            }                                            
                                        }
                                    }
                                }
                                else
                                {
                                    // if there is parent visibility do nothing else delete ou and child visibilities, leave children visibilities if exists children responsibility
                                    if (!userOUUnits.ContainsKey(ouTO.ParentOrgUnitID) || ouTO.OrgUnitID == ouTO.ParentOrgUnitID)
                                    {
                                        // delete unit and all it's children visibilities                                        
                                        applUserXou.SetTransaction(emplRes.GetTransaction());
                                        foreach (OrganizationalUnitTO child in ouUnitsChildren)
                                        {
                                            if (ouList.Contains(child.OrgUnitID))
                                                continue;

                                            applUserXou.AuXOUnitTO = new ApplUserXOrgUnitTO();
                                            saved = saved && applUserXou.Delete(userID, child.OrgUnitID, Constants.EmployeesPurpose, false);

                                            if (!saved)
                                                break;
                                        }

                                        if (saved)
                                        {
                                            //set first organizational unit to default one                                            
                                            EmployeeAsco4 asco = new EmployeeAsco4();
                                            if (ascoDict.ContainsKey(remRes.EmployeeID) && ascoDict[remRes.EmployeeID].IntegerValue3 == remRes.UnitID)
                                            {
                                                asco.EmplAsco4TO = ascoDict[remRes.EmployeeID];
                                                asco.EmplAsco4TO.IntegerValue3 = defaultUnitID;
                                                asco.SetTransaction(emplRes.GetTransaction());
                                                saved = saved && asco.update(false);
                                            }
                                        }
                                    }
                                }
                            }

                            if (saved)
                                emplRes.CommitTransaction();
                            else if (emplRes.GetTransaction() != null)
                                emplRes.RollbackTransaction();
                        }
                        catch (Exception ex)
                        {
                            if (emplRes.GetTransaction() != null)
                                emplRes.RollbackTransaction();

                            log.writeLog(DateTime.Now + " EmployeesResponsibilities.btnSave_Click(): " + ex.Message + "\n");

                            saved = false;
                        }

                        savedAll = savedAll && saved;
                    }
                    else
                        savedAll = false;
                }

                if (savedAll)
                    MessageBox.Show(rm.GetString("responsibilitiesSaved", culture));
                else
                    MessageBox.Show(rm.GetString("responsibilitiesNotSaved", culture));

                addedEmplResponsibility = new List<EmployeeResponsibilityTO>();
                removedEmplResponsibility = new List<EmployeeResponsibilityTO>();
                populateResponsibilities();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setVisibility()
        {
            try
            {
                int permissionUsed;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permissionUsed = (((int[])menuItemsPermissions[menuItemID])[role.ApplRoleID]);

                    addPermission = addPermission || (((permissionUsed / 4) % 2) == 0 ? false : true);
                    updatePermission = updatePermission || (((permissionUsed / 2) % 2) == 0 ? false : true);
                    deletePermission = deletePermission || ((permissionUsed % 2) == 0 ? false : true);

                }

                if (!addPermission && !updatePermission && !deletePermission)
                    tabControl1.TabPages.Remove(tabSettings);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesResponsibilities.setVisibility(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}