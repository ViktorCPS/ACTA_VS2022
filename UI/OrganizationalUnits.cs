using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;
using Common;
using System.Globalization;
using System.Resources;
using TransferObjects;
using System.Collections;

namespace UI
{
    public partial class OrganizationalUnits : Form
    {
        bool readPermission = false;
        bool addPermission = false;
        bool updatePermission = false;
        bool deletePermission = false;

        private CultureInfo culture;
        
        // Current WOking Unit
        protected OrganizationalUnitTO currentWorkingUnit;

        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;
        string menuItemID;

        OrganizationalUnitTO currentWUTree;
        ToolTip toolTip1;
        private ListViewItemComparer _comp;
        private System.Data.DataSet dsWorkingUnits;
        public System.Data.DataView wuDataView;

        List<OrganizationalUnitTO> wuArray;

        // List View indexes
        //const int WorkingUnitID = 0;
        const int WUName = 0;
        const int Description = 1;
        const int ParentWUID = 2;

        public OrganizationalUnits()
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

            rm = new ResourceManager("UI.Resource", typeof(WorkingUnits).Assembly);
            currentWorkingUnit = new OrganizationalUnitTO();
            currentWUTree = new OrganizationalUnitTO();
            logInUser = NotificationController.GetLogInUser();
            toolTip1 = new ToolTip();
            setLanguage();
            this.CenterToScreen();
            this.cbParentUnitID.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Set proper language.
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("OUForm", culture);

                // group box text
                gbOrganizationalUnits.Text = rm.GetString("gbOrganizationalUnits", culture);

                // tab page's text
                tpListView.Text = rm.GetString("tpListView", culture);
                tpTreeView.Text = rm.GetString("tpTreeView", culture);

                // button's text
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnEmplXOU.Text = rm.GetString("btnEmplXOU", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                btnAddTree.Text = rm.GetString("btnAdd", culture);
                btnUpdateTree.Text = rm.GetString("btnUpdate", culture);
                btnDeleteTree.Text = rm.GetString("btnDelete", culture);
                btnEmplXOUTree.Text = rm.GetString("btnEmplXOU", culture);
                btnCloseTree.Text = rm.GetString("btnClose", culture);

                // label's text
                lblParentOUID.Text = rm.GetString("lblParentOUID", culture);
                lblDescription.Text = rm.GetString("lblDescription", culture);
                lblName.Text = rm.GetString("lblName", culture);

                
                lvWorkingUnits.BeginUpdate();
                lvWorkingUnits.Columns.Add(rm.GetString("lblName", culture), (lvWorkingUnits.Width - 3) / 3, HorizontalAlignment.Left);
                lvWorkingUnits.Columns.Add(rm.GetString("lblDescription", culture), (lvWorkingUnits.Width - 3) / 3, HorizontalAlignment.Left);
                lvWorkingUnits.Columns.Add(rm.GetString("lblParentOUID", culture), (lvWorkingUnits.Width - 3) / 3, HorizontalAlignment.Left);
                lvWorkingUnits.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnits.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCloseTree_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OrganizationalUnits_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                _comp = new ListViewItemComparer(lvWorkingUnits);
                lvWorkingUnits.ListViewItemSorter = _comp;
                lvWorkingUnits.Sorting = SortOrder.Ascending;

                populateWorkigUnitCombo();

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                OrganizationalUnit wu = new OrganizationalUnit();
                wu.OrgUnitTO.Status = Constants.DefaultStateActive;
                List<OrganizationalUnitTO> workingUnitsList = wu.Search();
                populateListView(workingUnitsList);
                dsWorkingUnits = new OrganizationalUnit().getOrganizationUnits("");
                populateDataTreeView();

                //BLOKIRANJE -> status najnoviji ODBLOKIRANO 18.01.2018
                /*
                btnAddTree.Enabled = false;
                btnDeleteTree.Enabled = false;
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;*/
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnits.OrganizationalUnits_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }	
        }

        private void populateDataTreeView()
        {
            try
            {
                if (dsWorkingUnits != null)
                {
                    this.wuDataView = dsWorkingUnits.Tables["Organization units"].DefaultView;
                    BindingSource bs = new BindingSource();
                    bs.DataSource = this.wuDataView;

                    this.dataTreeView1.IDColumn = "";
                    this.dataTreeView1.ParentIDColumn = "";
                    this.dataTreeView1.ToolTipTextColumn = "";
                    this.dataTreeView1.NameColumn = "";
                    this.dataTreeView1.DataSource = bs;
                    this.dataTreeView1.IDColumn = "organizational_unit_id";
                    this.dataTreeView1.ParentIDColumn = "parent_organizational_unit_id";
                    this.dataTreeView1.ToolTipTextColumn = "description";
                    this.dataTreeView1.NameColumn = "name";
                    this.dataTreeView1.Refresh();
                    dataTreeView1.ExpandAll();
                    dataTreeView1.SelectedNode = dataTreeView1.Nodes[0];
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnits.populateDataTreeView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Populate List View with Working Units found.
        /// </summary>
        /// <param name="workingUnitsList"></param>
        public void populateListView(List<OrganizationalUnitTO> orgUnitsList)
        {
            try
            {
                lvWorkingUnits.BeginUpdate();
                lvWorkingUnits.Items.Clear();

                OrganizationalUnit tempOu = new OrganizationalUnit();
                if (orgUnitsList.Count > 0)
                {
                    foreach (OrganizationalUnitTO wunit in orgUnitsList)
                    {
                        ListViewItem item = new ListViewItem();

                        //if ((currentWorkingUnit.Find(Int32.Parse(item.Text.Trim()))) && (currentWorkingUnit.WorkingUnitID != 0))
                        //if ((tempWu.Find(wunit.WorkingUnitID)))// && (tempWu.WorkingUnitID != 0))
                        //{
                        item.Text = wunit.Name.Trim();
                        item.SubItems.Add(wunit.Desc.ToString().Trim());
                        tempOu.OrgUnitTO.OrgUnitID = wunit.ParentOrgUnitID;
                        item.SubItems.Add(tempOu.Find(tempOu.OrgUnitTO.OrgUnitID).Name.Trim());
                        item.Tag = wunit;

                        lvWorkingUnits.Items.Add(item);
                        //}
                    }
                }

                lvWorkingUnits.EndUpdate();
                lvWorkingUnits.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnits.setLanguage(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        private void setVisibility()
        {
            try
            {
                int permission;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permission = (((int[])menuItemsPermissions[menuItemID])[role.ApplRoleID]);
                    readPermission = readPermission || (((permission / 8) % 2) == 0 ? false : true);
                    addPermission = addPermission || (((permission / 4) % 2) == 0 ? false : true);
                    updatePermission = updatePermission || (((permission / 2) % 2) == 0 ? false : true);
                    deletePermission = deletePermission || ((permission % 2) == 0 ? false : true);
                }

                btnSearch.Enabled = readPermission;
                btnAdd.Enabled = btnAddTree.Enabled = addPermission;
                btnUpdate.Enabled = btnUpdateTree.Enabled = updatePermission;
                btnDelete.Enabled = btnDeleteTree.Enabled = deletePermission;
                btnEmplXOU.Enabled = btnEmplXOUTree.Enabled = readPermission && addPermission && updatePermission && deletePermission;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnits.setVisibility(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate combo box with values form data table. 
        /// Exclude those which are child of this working unit.
        /// </summary>
        private void populateWorkigUnitCombo()
        {
            OrganizationalUnit wu = new OrganizationalUnit();
            wu.OrgUnitTO.Status = Constants.DefaultStateActive;
            wuArray = wu.Search();
            List<OrganizationalUnitTO> actualWu = new List<OrganizationalUnitTO>();
            // Add All as a first member of combo
            OrganizationalUnitTO allOU = new OrganizationalUnitTO();
            allOU.OrgUnitID = -1;
            allOU.ParentOrgUnitID = 0;
            allOU.Name = rm.GetString("all", culture);
            actualWu.Insert(0,allOU);

            foreach (OrganizationalUnitTO ouMember in wuArray)
            {
                // TODO: Is this part necessary? 0 or DEFAULT Working Unit shod be shown by its name!!!!!
                if (ouMember.OrgUnitID == 0)
                {
                    //wuMember.Name = "DEFAULT";
                    actualWu.Insert(1, ouMember);
                }
                else
                {
                    if (!currentWorkingUnit.OrgUnitID.Equals(0))
                    {
                        if ((ouMember.ParentOrgUnitID != currentWorkingUnit.OrgUnitID) || (ouMember.OrgUnitID == 0))
                        {
                            actualWu.Add(ouMember);
                        }
                        else
                        {
                            if (ouMember.OrgUnitID == currentWorkingUnit.OrgUnitID)
                            {
                                actualWu.Add(ouMember);
                            }
                        }
                    }
                    else
                    {
                        actualWu.Add(ouMember);
                    }
                }
            }

            cbParentUnitID.DataSource = actualWu;
            cbParentUnitID.DisplayMember = "Name";
            cbParentUnitID.ValueMember = "OrgUnitID";
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
                    case OrganizationalUnits.Description:
                    case OrganizationalUnits.ParentWUID:
                    case OrganizationalUnits.WUName:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");

                }
            }

        }

        #endregion

        private void btnDeleteTree_Click(object sender, EventArgs e)
        {
            // All employees that belongs to this working unit 
            // must be deleted first
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int selected = dataTreeView1.SelectedNodes.Count;

                if (dataTreeView1.SelectedNodes.Count > 0)
                {
                    DialogResult result1 = MessageBox.Show(rm.GetString("messageOUDel1", culture), "", MessageBoxButtons.YesNo);
                    if (result1 == DialogResult.Yes)
                    {
                        foreach (TreeNode node in dataTreeView1.SelectedNodes)
                        {
                            if (((int)node.Tag) == 0)
                            {
                                MessageBox.Show(rm.GetString("defaultOUDel", culture));
                                selected--;
                            }
                            else
                            {
                                // Check if some Employees belong to this working unit
                                Employee empl = new Employee();
                                empl.EmplTO.OrgUnitID = (int)node.Tag;
                                List<EmployeeTO> employeeList = empl.Search();
                                // If some Employees belong to this working unit, delete them first
                                if (employeeList.Count > 0)
                                {
                                    MessageBox.Show(node.Text + ": " + rm.GetString("messageOUDel2", culture));
                                    selected--;
                                }
                                else
                                {
                                    OrganizationalUnit oUnit = new OrganizationalUnit();
                                    oUnit.OrgUnitTO.ParentOrgUnitID = (int)node.Tag;
                                    oUnit.OrgUnitTO.Status = Constants.DefaultStateActive;

                                    List<OrganizationalUnitTO> childWU = oUnit.Search();

                                    if (childWU.Count > 1)
                                    {
                                        MessageBox.Show(node.Text + ": " + rm.GetString("messageOUDel3", culture));
                                        selected--;
                                    }
                                    else
                                    {
                                        WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit();
                                        wuXou.WUXouTO.OrgUnitID = (int)node.Tag;
                                        List<WorkingUnitXOrganizationalUnitTO> wuList = wuXou.Search();

                                        bool isDeleted = true;
                                        // This working unit is a parent to itself
                                        if ((childWU.Count == 1) && (employeeList.Count == 0))
                                        {
                                            OrganizationalUnit ou = new OrganizationalUnit();
                                            ou.OrgUnitTO = childWU[0];
                                            if (ou.OrgUnitTO.OrgUnitID == ou.OrgUnitTO.ParentOrgUnitID)
                                            {
                                                bool trans = ou.BeginTransaction();
                                                if (trans)
                                                {
                                                    ApplUserXOrgUnit applUsersXOU = new ApplUserXOrgUnit();
                                                    applUsersXOU.SetTransaction(ou.GetTransaction());
                                                    isDeleted = applUsersXOU.Delete(ou.OrgUnitTO.OrgUnitID, false) && isDeleted;
                                                    if (isDeleted)
                                                    {
                                                        wuXou.SetTransaction(ou.GetTransaction());
                                                        foreach (WorkingUnitXOrganizationalUnitTO wuXouTO in wuList)
                                                        {
                                                            isDeleted = isDeleted && wuXou.Delete(wuXouTO.OrgUnitID, wuXouTO.WorkingUnitID, false);
                                                        }

                                                        //isDeleted = wu.Delete(wu.WorkingUnitID) && isDeleted;
                                                        isDeleted = isDeleted && ou.Delete(ou.OrgUnitTO.OrgUnitID, false);
                                                    }

                                                    if (isDeleted)
                                                    {
                                                        ou.CommitTransaction();

                                                        //if ((selected > 0) && isDeleted)
                                                        MessageBox.Show(rm.GetString("ouDeleted", culture));
                                                    }
                                                    else
                                                    {
                                                        ou.RollbackTransaction();

                                                        //else if (!isDeleted)
                                                        MessageBox.Show(rm.GetString("ouNotDeleted", culture));
                                                    }
                                                    currentWUTree = new OrganizationalUnitTO();
                                                }
                                                else
                                                {
                                                    MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show(node.Text + ": " + rm.GetString("messageWUDel3", culture));
                                                selected--;
                                            }
                                        }

                                        // There is no child working units to this one
                                        if ((childWU.Count == 0) && (employeeList.Count == 0))
                                        {
                                            OrganizationalUnit ou = new OrganizationalUnit();
                                            bool trans = ou.BeginTransaction();
                                            if (trans)
                                            {
                                                try
                                                {
                                                    ApplUserXOrgUnit applUsersXOU = new ApplUserXOrgUnit();
                                                    applUsersXOU.SetTransaction(ou.GetTransaction());
                                                    isDeleted = applUsersXOU.Delete((int)node.Tag, false) && isDeleted;
                                                    if (isDeleted)
                                                    {
                                                        wuXou.SetTransaction(ou.GetTransaction());
                                                        foreach (WorkingUnitXOrganizationalUnitTO wuXouTO in wuList)
                                                        {
                                                            isDeleted = isDeleted && wuXou.Delete(wuXouTO.OrgUnitID, wuXouTO.WorkingUnitID, false);
                                                        }

                                                        //isDeleted = currentWorkingUnit.Delete((int) item.Tag) && isDeleted;
                                                        isDeleted = isDeleted && ou.Delete((int)node.Tag, false);
                                                    }

                                                    if (isDeleted)
                                                    {
                                                        ou.CommitTransaction();

                                                        //if ((selected > 0) && isDeleted)
                                                        MessageBox.Show(rm.GetString("ouDeleted", culture));
                                                    }
                                                    else
                                                    {
                                                        ou.RollbackTransaction();

                                                        //else if (!isDeleted)
                                                        MessageBox.Show(rm.GetString("ouNotDeleted", culture));
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    if (ou.GetTransaction() != null)
                                                        ou.RollbackTransaction();
                                                    throw ex;
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                                                return;
                                            }
                                        }
                                    }
                                    //}                                        
                                }
                            }
                        }
                        dsWorkingUnits = new OrganizationalUnit().getOrganizationUnits("");
                        populateDataTreeView();

                        OrganizationalUnit orgUnit = new OrganizationalUnit();
                        orgUnit.OrgUnitTO.Status = Constants.DefaultStateActive;

                        List<OrganizationalUnitTO> oudaoList = orgUnit.Search();
                        populateListView(oudaoList);
                        tbName.Text = "";
                        tbDescription.Text = "";
                        currentWorkingUnit = new OrganizationalUnitTO();
                        populateWorkigUnitCombo();
                        cbParentUnitID.SelectedValue = -1;
                        this.Invalidate();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("messageOUDel4", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnits.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            // All employees that belongs to this working unit 
            // must be deleted first
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int selected = lvWorkingUnits.SelectedItems.Count;

                if (lvWorkingUnits.SelectedItems.Count > 0)
                {
                    DialogResult result1 = MessageBox.Show(rm.GetString("messageOUDel1",culture), "", MessageBoxButtons.YesNo);
                    if (result1 == DialogResult.Yes)
                    {
                        foreach (ListViewItem item in lvWorkingUnits.SelectedItems)
                        {
                            if (((OrganizationalUnitTO)item.Tag).OrgUnitID == 0)
                            {
                                MessageBox.Show(rm.GetString("defaultOUDel", culture));
                                selected--;
                            }
                            else
                            {
                                // Check if some Employees belong to this working unit
                                Employee empl = new Employee();
                                empl.EmplTO.OrgUnitID = ((OrganizationalUnitTO)item.Tag).OrgUnitID;
                                List<EmployeeTO> employeeList = empl.Search();
                                // If some Employees belong to this working unit, delete them first
                                if (employeeList.Count > 0)
                                {
                                    MessageBox.Show(item.Text + ": " + rm.GetString("messageOUDel2", culture));
                                    selected--;
                                }
                                else
                                {
                                    /*
                                    // Check if some Users are granted to this working unit
                                    ArrayList userList = new ApplUsersXWU().Search("", item.Tag.ToString().Trim(), "");
                                    // If some Users are granted to this working unit, delete them first
                                    if (userList.Count > 0)
                                    {
                                        MessageBox.Show(item.Text + ": " + rm.GetString("wuHasUsers", culture));
                                        selected--;
                                    }
							
                                    else
                                    {*/
                                    // Check if this Working Unit is a parent working unit to some other
                                    //ArrayList childWU = currentWorkingUnit.Search("", item.Text.Trim(), "", "", "");
                                    OrganizationalUnit workUnit = new OrganizationalUnit();
                                    workUnit.OrgUnitTO.ParentOrgUnitID = ((OrganizationalUnitTO)item.Tag).OrgUnitID;
                                    workUnit.OrgUnitTO.Status = Constants.DefaultStateActive;
                                    List<OrganizationalUnitTO> childWU = workUnit.Search();

                                    if (childWU.Count > 1)
                                    {
                                        MessageBox.Show(item.Text + ": " + rm.GetString("messageOUDel3", culture));
                                        selected--;
                                    }
                                    else
                                    {
                                        bool isDeleted = true;
                                        WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit();
                                        wuXou.WUXouTO.OrgUnitID = ((OrganizationalUnitTO)item.Tag).OrgUnitID;
                                        List<WorkingUnitXOrganizationalUnitTO> wuList = wuXou.Search();

                                        // This working unit is a parent to itself
                                        if ((childWU.Count == 1) && (employeeList.Count == 0))
                                        {
                                            OrganizationalUnit wu = new OrganizationalUnit();
                                            wu.OrgUnitTO = childWU[0];
                                            if (wu.OrgUnitTO.OrgUnitID.Equals(wu.OrgUnitTO.ParentOrgUnitID))
                                            {
                                                bool trans = wu.BeginTransaction();
                                                if (trans)
                                                {
                                                    ApplUserXOrgUnit applUsersXWU = new ApplUserXOrgUnit();
                                                    applUsersXWU.SetTransaction(wu.GetTransaction());
                                                    isDeleted = applUsersXWU.Delete(wu.OrgUnitTO.OrgUnitID, false) && isDeleted;
                                                    if (isDeleted)
                                                    {
                                                        wuXou.SetTransaction(wu.GetTransaction());
                                                        foreach (WorkingUnitXOrganizationalUnitTO wuXouTO in wuList)
                                                        {
                                                            isDeleted = isDeleted && wuXou.Delete(wuXouTO.OrgUnitID, wuXouTO.WorkingUnitID, false);
                                                        }

                                                        //isDeleted = wu.Delete(wu.WorkingUnitID) && isDeleted;
                                                        isDeleted = isDeleted && wu.Delete(wu.OrgUnitTO.OrgUnitID, false);
                                                    }

                                                    if (isDeleted)
                                                    {
                                                        wu.CommitTransaction();

                                                        //if ((selected > 0) && isDeleted)
                                                        MessageBox.Show(rm.GetString("ouDeleted", culture));
                                                    }
                                                    else
                                                    {
                                                        wu.RollbackTransaction();

                                                        //else if (!isDeleted)
                                                        MessageBox.Show(rm.GetString("ouNotDeleted", culture));
                                                    }
                                                    currentWorkingUnit = new OrganizationalUnitTO();
                                                }
                                                else
                                                {
                                                    MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show(item.Text + ": " + rm.GetString("messageOUDel3", culture));
                                                selected--;
                                            }
                                        }

                                        // There is no child working units to this one
                                        if ((childWU.Count == 0) && (employeeList.Count == 0))
                                        {
                                            OrganizationalUnit wu = new OrganizationalUnit();
                                            bool trans = wu.BeginTransaction();
                                            if (trans)
                                            {
                                                try
                                                {
                                                    ApplUserXOrgUnit applUsersXOU = new ApplUserXOrgUnit();
                                                    applUsersXOU.SetTransaction(wu.GetTransaction());
                                                    isDeleted = applUsersXOU.Delete(((OrganizationalUnitTO)item.Tag).OrgUnitID, false) && isDeleted;
                                                    if (isDeleted)
                                                    {
                                                        wuXou.SetTransaction(wu.GetTransaction());
                                                        foreach (WorkingUnitXOrganizationalUnitTO wuXouTO in wuList)
                                                        {
                                                            isDeleted = isDeleted && wuXou.Delete(wuXouTO.OrgUnitID, wuXouTO.WorkingUnitID, false);
                                                        }

                                                        isDeleted = isDeleted && wu.Delete(((OrganizationalUnitTO)item.Tag).OrgUnitID, false);                                                        
                                                    }

                                                    if (isDeleted)
                                                    {
                                                        wu.CommitTransaction();

                                                        //if ((selected > 0) && isDeleted)
                                                        MessageBox.Show(rm.GetString("wuDeleted", culture));
                                                    }
                                                    else
                                                    {
                                                        wu.RollbackTransaction();

                                                        //else if (!isDeleted)
                                                        MessageBox.Show(rm.GetString("wuNotDeleted", culture));
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    if (wu.GetTransaction() != null)
                                                        wu.RollbackTransaction();
                                                    throw ex;
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                                                return;
                                            }
                                        }
                                    }
                                    //}
                                }
                            }
                        }

                        OrganizationalUnit orgUnit = new OrganizationalUnit();
                        orgUnit.OrgUnitTO.Status = Constants.DefaultStateActive;
                        List<OrganizationalUnitTO> orgUnitsList = orgUnit.Search();

                        populateListView(orgUnitsList);
                        //cbParentUnitID.SelectedValue = currentWorkingUnit.ParentWorkingUID;
                        tbName.Text = "";
                        tbDescription.Text = "";
                        currentWorkingUnit = new OrganizationalUnitTO();
                        populateWorkigUnitCombo();
                        cbParentUnitID.SelectedValue = -1;

                        dsWorkingUnits = new OrganizationalUnit().getOrganizationUnits("");
                        populateDataTreeView();

                        this.Invalidate();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("messageOUDel4", culture));
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnits.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Value -1 is assigned to "ALL" in cbParentUnitID
                //if (Int32.Parse(cbParentUnitID.SelectedValue.ToString().Trim()) != -1)
                //{
                //    partentWUID = cbParentUnitID.SelectedValue.ToString().Trim();
                //}

                OrganizationalUnit oUnit = new OrganizationalUnit();
                oUnit.OrgUnitTO.ParentOrgUnitID = (int)cbParentUnitID.SelectedValue;
                oUnit.OrgUnitTO.Desc = tbDescription.Text.Trim();
                oUnit.OrgUnitTO.Name = tbName.Text.Trim();
                oUnit.OrgUnitTO.Status = Constants.DefaultStateActive;
                List<OrganizationalUnitTO> workingUnitsList = oUnit.Search();

                if (workingUnitsList.Count > 0)
                {
                    populateListView(workingUnitsList);
                }
                else
                {
                    MessageBox.Show(rm.GetString("messageOUSearch1",culture));
                }

                currentWorkingUnit = new OrganizationalUnitTO();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnits.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }		
        }

        private void btnWUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsOrgUnits = new OrganizationalUnit().getOrganizationUnits("");
                WorkingUnitsTreeView orgUnitsTreeView = new WorkingUnitsTreeView(dsOrgUnits);
                orgUnitsTreeView.ShowDialog();
                if (!orgUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbParentUnitID.SelectedIndex = cbParentUnitID.FindStringExact(orgUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnits.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAddTree_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                OrganizationalUnitsAdd workingUnitsAdd = new OrganizationalUnitsAdd(new OrganizationalUnitTO());
                workingUnitsAdd.ShowDialog();

                OrganizationalUnit wu = new OrganizationalUnit();
                dsWorkingUnits = wu.getOrganizationUnits("");
                populateDataTreeView();

                wu.OrgUnitTO.Status = Constants.DefaultStateActive;
                List<OrganizationalUnitTO> wudaoList = wu.Search();
                populateListView(wudaoList);
                tbName.Text = "";
                tbDescription.Text = "";
                currentWorkingUnit = new OrganizationalUnitTO();
                populateWorkigUnitCombo();
                cbParentUnitID.SelectedValue = -1;
                this.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnits.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUpdateTree_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.dataTreeView1.SelectedNodes.Count == 0)
                {
                    MessageBox.Show(rm.GetString("messageOUUpd1",culture));
                }
                else if (this.dataTreeView1.SelectedNodes.Count > 1)
                {
                    MessageBox.Show(rm.GetString("messageOUUpd2", culture));
                }
                else
                {
                    OrganizationalUnitsAdd workingUnitsAdd = new OrganizationalUnitsAdd(currentWUTree);
                    workingUnitsAdd.ShowDialog();

                    OrganizationalUnit wu = new OrganizationalUnit();
                    dsWorkingUnits = wu.getOrganizationUnits("");
                    populateDataTreeView();

                    wu.OrgUnitTO.Status = Constants.DefaultStateActive;
                    List<OrganizationalUnitTO> wudaoList = wu.Search();
                    populateListView(wudaoList);
                    tbName.Text = "";
                    tbDescription.Text = "";
                    currentWorkingUnit = new OrganizationalUnitTO();
                    populateWorkigUnitCombo();
                    cbParentUnitID.SelectedValue = -1;
                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnits.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dataTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if ((dataTreeView1.SelectedNodes.Count >= 0) && (ModifierKeys != Keys.Shift) && (ModifierKeys != Keys.Control))
                {
                    TreeNode node = e.Node;

                    currentWUTree = new OrganizationalUnitTO();
                    // WorkingUnit parentWU = new WorkingUnit();
                    if (node != null)
                    {
                        currentWUTree = new OrganizationalUnit().Find((int)node.Tag);
                        //parentWU = currentWorkingUnit.GetParentLocation();
                    }
                }
                else
                {
                    currentWUTree = new OrganizationalUnitTO();
                }
            }

            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnits.locationsTreeView_AfterSelect(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void dataTreeView1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                // Get the node at the current mouse pointer location.
                TreeNode theNode = this.dataTreeView1.GetNodeAt(e.X, e.Y);
                if ((theNode != null))
                {
                    // Verify that the tag property is not "null".
                    if (theNode.ToolTipText != "")
                    {
                        // Change the ToolTip only if the pointer moved to a new node.
                        if (theNode.ToolTipText != this.toolTip1.GetToolTip(this.dataTreeView1))
                        {
                            this.toolTip1.Show(theNode.ToolTipText, this, e.X + 50, e.Y + 100);
                        }
                    }
                    else
                    {
                        this.toolTip1.SetToolTip(this, "");
                    }
                }
                else     // Pointer is not over a node so clear the ToolTip.
                {
                    this.toolTip1.SetToolTip(this, "");
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnits.dataTreeView1_MouseMove(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Open Add Form
                OrganizationalUnitsAdd addForm = new OrganizationalUnitsAdd(new OrganizationalUnitTO());
                addForm.ShowDialog(this);

                OrganizationalUnit wu = new OrganizationalUnit();
                wu.OrgUnitTO.Status = Constants.DefaultStateActive;
                List<OrganizationalUnitTO> wudaoList = wu.Search();
                populateListView(wudaoList);

                tbName.Text = "";
                tbDescription.Text = "";
                currentWorkingUnit = new OrganizationalUnitTO();
                populateWorkigUnitCombo();
                cbParentUnitID.SelectedValue = -1;

                wu = new OrganizationalUnit();
                dsWorkingUnits = wu.getOrganizationUnits("");
                populateDataTreeView();

                this.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnits.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (this.lvWorkingUnits.SelectedItems.Count == 0)
                {
                    MessageBox.Show(rm.GetString("messageWUUpd1",culture));
                }
                else if (this.lvWorkingUnits.SelectedItems.Count > 1)
                {
                    MessageBox.Show(rm.GetString("messageWUUpd2", culture));
                }
                else
                {
                    currentWorkingUnit = (OrganizationalUnitTO)lvWorkingUnits.SelectedItems[0].Tag;

                    // Open Update Form
                    OrganizationalUnitsAdd addForm = new OrganizationalUnitsAdd(currentWorkingUnit);
                    addForm.ShowDialog(this);

                    OrganizationalUnit wUnit = new OrganizationalUnit();
                    wUnit.OrgUnitTO.Status = Constants.DefaultStateActive;
                    List<OrganizationalUnitTO> wudaoList = wUnit.Search();
                    populateListView(wudaoList);
                    tbName.Text = "";
                    tbDescription.Text = "";
                    currentWorkingUnit = new OrganizationalUnitTO();
                    populateWorkigUnitCombo();
                    cbParentUnitID.SelectedValue = -1;

                    Common.OrganizationalUnit wu = new OrganizationalUnit();
                    dsWorkingUnits = wu.getOrganizationUnits("");
                    populateDataTreeView();

                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnits.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnEmplXOU_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeesXOrganizationalUnits emplXOrg = new EmployeesXOrganizationalUnits();
                emplXOrg.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnits.btnEmplXOU_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
