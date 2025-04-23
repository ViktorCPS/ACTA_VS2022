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
    public partial class EmployeesXWorkingUnits : Form
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
        const int WorkingUnitNameIndex = 3;

        // NOTE: removedEmployees is not in use. If it is needed in the future,
        //relations between addedEmployees and removedEmployees need to be added. 
        //See ApplUsersXRoles.cs for example
        private ArrayList removedEmployees;
        private ArrayList addedEmployees;

        private string prevSelValuecbWU = "";

        private bool saveForPrevIndex = false;

        List<WorkingUnitTO> workingUnitArray;
        List<WorkingUnitTO> wuArray;

        public EmployeesXWorkingUnits()
        {
            InitializeComponent();
            setProperties();
        }

		public void setProperties()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			removedEmployees = new ArrayList();
			addedEmployees   = new ArrayList();

			rm = new ResourceManager("UI.Resource",typeof(EmployeesXWorkingUnits).Assembly);
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
                    case EmployeesXWorkingUnits.EmployeeIDIndex:
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
                    case EmployeesXWorkingUnits.FirstNameIndex:
                    case EmployeesXWorkingUnits.LastNameIndex:
                    case EmployeesXWorkingUnits.WorkingUnitNameIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");
                }
            }
        }

        #endregion

        /// <summary>
        /// Set proper language and initialize List View
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("employeesXWUForm", culture);

                // TabPage text
                tabPageEmployees.Text = rm.GetString("tabPageEmployees", culture);
                tabPageWorkingUnits.Text = rm.GetString("tabPageWorkingUnits", culture);
                tabPageEmployeesXWorkingUnitss.Text = rm.GetString("tabPageEmployeesXWorkingUnits", culture);

                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);
                btnSave.Text = rm.GetString("btnSave", culture);

                // group box text
                gbWU.Text = rm.GetString("gbWU", culture);

                // label's text
                lblWorkingUnit.Text = rm.GetString("lblWorkingUnit", culture);
                lblWorkingUnitDesc.Text = rm.GetString("lblDescription", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblSelWUName.Text = rm.GetString("lblName", culture);
                lblSelWUDesc.Text = rm.GetString("lblDescription", culture);
                lblWUName.Text = rm.GetString("lblName", culture);
                lblDesc.Text = rm.GetString("lblDescription", culture);
                lblEmployeeForWU.Text = rm.GetString("lblEmployeeForWU", culture);
                lblWU.Text = rm.GetString("lblWU", culture);
                lblWUDesc.Text = rm.GetString("lblDescription", culture);
                lblEmployeesForWU.Text = rm.GetString("lblEmployeesForWU", culture);
                lblSelEmployees.Text = rm.GetString("lblEmployeeForWU", culture);

                // list view				
                lvEmployee.BeginUpdate();
                lvEmployee.Columns.Add(rm.GetString("hdrEmployeeID", culture), (lvEmployee.Width - 4) / 4, HorizontalAlignment.Left);
                lvEmployee.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployee.Width - 4) / 4, HorizontalAlignment.Left);
                lvEmployee.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployee.Width - 4) / 4, HorizontalAlignment.Left);
                lvEmployee.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvEmployee.Width - 4) / 4, HorizontalAlignment.Left);
                lvEmployee.EndUpdate();

                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrEmployeeID", culture), (lvEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();

                lvSelectedEmployees.BeginUpdate();
                lvSelectedEmployees.Columns.Add(rm.GetString("hdrEmployeeID", culture), (lvSelectedEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvSelectedEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), (lvSelectedEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvSelectedEmployees.Columns.Add(rm.GetString("hdrLastName", culture), (lvSelectedEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvSelectedEmployees.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvSelectedEmployees.Width - 4) / 4, HorizontalAlignment.Left);
                lvSelectedEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateEmployeeCombo()
        {
            try
            {
                int workingUnitID = -1;

                List<string> statuses = new List<string>();
                statuses.Add(Constants.statusActive);
                statuses.Add(Constants.statusBlocked);
                
                if (cbWorkingUnit.SelectedIndex > 0)
                {
                    workingUnitID = (int)cbWorkingUnit.SelectedValue;
                }

                List<EmployeeTO> emplArray = new List<EmployeeTO>();

                if (workingUnitID == -1)
                {
                    emplArray = new Employee().SearchWithStatuses(statuses, "");
                }
                else
                {
                    emplArray = new Employee().SearchWithStatuses(statuses, workingUnitID.ToString());
                }

                foreach (EmployeeTO employee in emplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                emplArray.Insert(0, empl);

                cbEmployee.DataSource = emplArray;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "WorkingUnitID";
                cbEmployee.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.populateEmployeeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateWorkingUnitCombo()
        {
            try
            {
                WorkingUnit workingUnit = new WorkingUnit();
                workingUnit.WUTO.Status = Constants.DefaultStateActive;
                workingUnitArray = workingUnit.Search();
                workingUnitArray.Insert(0, new WorkingUnitTO(-1, -1, "", rm.GetString("all", culture), "", -1));

                cbWorkingUnit.DataSource = workingUnitArray;
                cbWorkingUnit.DisplayMember = "Name";
                cbWorkingUnit.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.populateWorkingUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateWUCombo(ComboBox cbName)
        {
            try
            {
                WorkingUnit wu = new WorkingUnit();
                wu.WUTO.Status = Constants.DefaultStateActive;
                wuArray = wu.Search();
                wuArray.Insert(0, new WorkingUnitTO(-1, -1, "", rm.GetString("all", culture), "", -1));

                cbName.DataSource = wuArray;
                cbName.DisplayMember = "Name";
                cbName.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.populateWUCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void clearListView(ListView lvName)
        {
            lvName.BeginUpdate();
            lvName.Items.Clear();
            lvName.EndUpdate();
            lvName.Invalidate();
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
                        item.SubItems.Add(employee.WorkingUnitName.Trim());

                        lvName.Items.Add(item);
                    }
                }

                lvName.EndUpdate();
                lvName.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.populateEmployeeListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void EmployeesXWU_Load(object sender, System.EventArgs e)
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

                populateWorkingUnitCombo();
                populateEmployeeCombo();
                populateWUCombo(cbWU);
                populateWUCombo(cbWUName);

                //BLOKIRANJE -> status najnoviji ODBLOKIRANO 18.01.2018
                /*
                btnAdd.Enabled = false;
                btnAddAll.Enabled = false;
                btnRemove.Enabled = false;
                btnRemoveAll.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                 * */

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.EmployeesXWU_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
            /* Maybe populate functions should be here instead in Constructor */
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.btnClose_Click(): " + ex.Message + "\n");
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
                if (cbWU.SelectedIndex > 0)
                {
                    removedEmployees.Clear();
                    addedEmployees.Clear();
                    cbWU.SelectedIndex = 0;

                    MessageBox.Show(rm.GetString("EmployeeXWUCancelChanges", culture));
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
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbWorkingUnit_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbWorkingUnit.SelectedIndex > 0)
                {
                    WorkingUnit workingUnit = new WorkingUnit();
                    workingUnit.Find((int)cbWorkingUnit.SelectedValue);

                    if (workingUnit.WUTO.WorkingUnitID != -1)
                    {
                        tbWorkingUnitDesc.Text = workingUnit.WUTO.Description.Trim();
                        populateEmployeeCombo();
                    }
                }
                else
                {
                    tbWorkingUnitDesc.Text = "";
                    populateEmployeeCombo();
                }
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbEmployee_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbEmployee.SelectedIndex > 0)
                {
                    WorkingUnit wu = new WorkingUnit();
                    wu.Find(Int32.Parse(cbEmployee.SelectedValue.ToString().Trim()));
                    if (wu.WUTO.WorkingUnitID != -1)
                    {
                        tbSelWUName.Text = wu.WUTO.Name.Trim();
                        tbSelWUDesc.Text = wu.WUTO.Description.Trim();
                    }
                }
                else
                {
                    tbSelWUName.Text = "";
                    tbSelWUDesc.Text = "";
                }
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.cbEmployee_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbWUName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbWUName.SelectedIndex > 0)
                {
                    WorkingUnit wu = new WorkingUnit();
                    wu.Find(Int32.Parse(cbWUName.SelectedValue.ToString().Trim()));
                    if (wu.WUTO.WorkingUnitID != -1)
                    {
                        List<string> statuses = new List<string>();
                        statuses.Add(Constants.statusActive);
                        statuses.Add(Constants.statusBlocked);
                        
                        tbDesc.Text = wu.WUTO.Description.Trim();
                        populateEmployeeListView(new Employee().SearchWithStatuses(statuses, wu.WUTO.WorkingUnitID.ToString()), lvEmployee);
                    }
                }
                else
                {
                    tbDesc.Text = "";
                    clearListView(lvEmployee);
                }
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.cbWUName_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (removedEmployees.Count > 0 || addedEmployees.Count > 0)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("EmployeeXWUSaveChanges", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        saveForPrevIndex = true;
                        btnSave.PerformClick();
                    }
                }

                if (cbWU.SelectedIndex > 0)
                {
                    WorkingUnit wu = new WorkingUnit();
                    wu.Find(Int32.Parse(cbWU.SelectedValue.ToString().Trim()));
                    if (wu.WUTO.WorkingUnitID != -1)
                    {
                        List<string> statuses = new List<string>();
                        statuses.Add(Constants.statusActive);
                        statuses.Add(Constants.statusBlocked);
                        
                        tbWUDesc.Text = wu.WUTO.Description.Trim();
                        List<EmployeeTO> emplinWU = new Employee().SearchWithStatuses(statuses, wu.WUTO.WorkingUnitID.ToString());
                        populateEmployeeListView(emplinWU, lvSelectedEmployees);
                        
                        List<EmployeeTO> emplNotinWU = new Employee().SearchWithStatuses(statuses, "");
                        List<EmployeeTO> employeesNotinWU = new List<EmployeeTO>();
                        foreach (EmployeeTO empl in emplNotinWU)
                        {
                            bool inWU = false;
                            foreach (EmployeeTO emp in emplinWU)
                            {
                                if (emp.EmployeeID == empl.EmployeeID)
                                {
                                    inWU = true;
                                    break;
                                }
                            }

                            if (!inWU)
                            {
                                employeesNotinWU.Add(empl);
                            }
                        }
                        populateEmployeeListView(employeesNotinWU, lvEmployees);
                    }
                }
                else
                {
                    tbWUDesc.Text = "";
                    clearListView(lvSelectedEmployees);
                    clearListView(lvEmployees);
                }

                removedEmployees.Clear();
                addedEmployees.Clear();
                if (cbWU.SelectedIndex != 0)
                    prevSelValuecbWU = cbWU.SelectedValue.ToString();
                else
                    prevSelValuecbWU = "ALL";
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.btnAddAll_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.btnRemoveAll_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.btnRemove_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.lvEmployee_ColumnClick(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.lvEmployees_ColumnClick(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.lvSelectedEmployees_ColumnClick(): " + ex.Message + "\n");
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
                if ((saveForPrevIndex && !prevSelValuecbWU.ToUpper().Equals("ALL")) || (cbWU.SelectedIndex > 0))
                {
                    bool isUpdated = true;
                    Employee employee = new Employee();

                    isUpdated = employee.BeginTransaction();

                    if (isUpdated)
                    {
                        /*foreach(ListViewItem item in removedEmployees)
                        {
                            //change it to 0
                        }*/
                        string wuID;
                        if (saveForPrevIndex)
                            wuID = prevSelValuecbWU;
                        else
                            wuID = cbWU.SelectedValue.ToString();

                        foreach (ListViewItem item in addedEmployees)
                        {
                            isUpdated = employee.UpdateWU(item.Text.Trim(), wuID, false) && isUpdated;
                        }
                    }
                    else
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                        return;
                    }

                    if (isUpdated)
                    {
                        employee.CommitTransaction();

                        //to refresh cbEmployee combo (to assign new Access group ID to each employee)
                        //and to refresh tbSelAccessGroupName and tbSelAccessGroupDesc
                        int employeeSelIndex = cbEmployee.SelectedIndex;
                        cbWorkingUnit_SelectedIndexChanged(sender, e);
                        cbEmployee.SelectedIndex = employeeSelIndex;
                        //to refresh lvEmployee list
                        cbWUName_SelectedIndexChanged(sender, e);

                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("EmployeeXWorkingUnitSaved", culture));
                        removedEmployees.Clear();
                        addedEmployees.Clear();
                        //this.Close();
                    }
                    else
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("EmployeeXWorkingUnitNotSaved", culture));
                        employee.RollbackTransaction();
                    }
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("SelectWU", culture));
                }

                saveForPrevIndex = false;

                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.EmployeesXWorkingUnits_KeyUp(): " + ex.Message + "\n");
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
                string wuString = "";

                foreach (WorkingUnitTO wUnit in workingUnitArray)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuString);
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    this.cbWorkingUnit.SelectedIndex = cbWorkingUnit.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnWUTree1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string wuString = "";

                foreach (WorkingUnitTO wUnit in wuArray)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuString);
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    this.cbWU.SelectedIndex = cbWorkingUnit.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesXWorkingUnits.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}