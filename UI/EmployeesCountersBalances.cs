using System;
using System.Collections;
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

namespace UI
{
    public partial class EmployeesCountersBalances : Form
    {
        private const int emplIDIndex = 0;
        private const int emplNameIndex = 1;
        private const int monthIndex = 2;
        private const int typeIndex = 3;
        private const int earnedIndex = 4;
        private const int usedIndex = 5;
        private const int balanceIndex = 6;

        private int sortOrder = Constants.sortAsc;
        private int sortField = emplNameIndex;
        private int startIndex = 0;

        private ListViewItemComparer _comp;

        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private string wuString = "";
        private List<int> wuList = new List<int>();

        private Dictionary<int, OrganizationalUnitTO> oUnits = new Dictionary<int, OrganizationalUnitTO>();
        private string ouString = "";
        private List<int> ouList = new List<int>();

        Dictionary<int, WorkingUnitTO> wuDict = new Dictionary<int, WorkingUnitTO>();
        Dictionary<int, OrganizationalUnitTO> ouDict = new Dictionary<int, OrganizationalUnitTO>();

        List<EmployeeCounterMonthlyBalanceTO> balanceList = new List<EmployeeCounterMonthlyBalanceTO>();
        Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
        Dictionary<int, EmployeeCounterTypeTO> typeDict = new Dictionary<int, EmployeeCounterTypeTO>();
        
        public EmployeesCountersBalances()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(EmployeesCountersBalances).Assembly);

                logInUser = NotificationController.GetLogInUser();

                setLanguage();

                rbWU.Checked = true;
                chbHierarhiclyWU.Checked = true;
                chbHierachyOU.Checked = true;
                                
                dtpFrom.Value = getMonthBegining(DateTime.Now.Date);
                dtpTo.Value = getMonthBegining(DateTime.Now.Date);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }        

        #region Inner Class for sorting List of Employees
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
                    case EmployeesCountersBalances.emplIDIndex:
                        {
                            int id1 = -1;
                            int id2 = -1;

                            int.TryParse(sub1.Text, out id1);
                            int.TryParse(sub2.Text, out id2);

                            return CaseInsensitiveComparer.Default.Compare(id1, id2);
                        }
                    case EmployeesCountersBalances.emplNameIndex:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                    default:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                }
            }
        }

        private class ArrayListSort : IComparer<EmployeeCounterMonthlyBalanceTO>
        {
            private int compOrder;
            private int compField;
            private Dictionary<int, EmployeeTO> employees = new Dictionary<int, EmployeeTO>();
            private Dictionary<int, EmployeeCounterTypeTO> types = new Dictionary<int, EmployeeCounterTypeTO>();
            
            public ArrayListSort(int sortOrder, int sortField, Dictionary<int, EmployeeTO> employees, Dictionary<int, EmployeeCounterTypeTO> types)
            {
                compOrder = sortOrder;
                compField = sortField;

                this.employees = employees;
                this.types = types;                
            }

            public int Compare(EmployeeCounterMonthlyBalanceTO x, EmployeeCounterMonthlyBalanceTO y)
            {
                EmployeeCounterMonthlyBalanceTO p1 = null;
                EmployeeCounterMonthlyBalanceTO p2 = null;
                
                EmployeeTO empl1 = new EmployeeTO();
                EmployeeTO empl2 = new EmployeeTO();

                EmployeeCounterTypeTO type1 = new EmployeeCounterTypeTO();
                EmployeeCounterTypeTO type2 = new EmployeeCounterTypeTO();

                if (compOrder == Constants.sortAsc)
                {
                    p1 = x;
                    p2 = y;
                }
                else
                {
                    p1 = y;
                    p2 = x;
                }
                
                if (employees.ContainsKey(p1.EmployeeID))
                    empl1 = employees[p1.EmployeeID];
                if (employees.ContainsKey(p2.EmployeeID))
                    empl2 = employees[p2.EmployeeID];

                if (types.ContainsKey(p1.EmplCounterTypeID))
                    type1 = types[p1.EmplCounterTypeID];
                if (types.ContainsKey(p2.EmplCounterTypeID))
                    type2 = types[p2.EmplCounterTypeID];
                
                switch (compField)
                {
                    case EmployeesCountersBalances.emplIDIndex:
                        return p1.EmployeeID.CompareTo(p2.EmployeeID);
                    case EmployeesCountersBalances.emplNameIndex:
                        return empl1.FirstAndLastName.CompareTo(empl2.FirstAndLastName);
                    case EmployeesCountersBalances.monthIndex:
                        return p1.Month.CompareTo(p2.Month);                   
                    case EmployeesCountersBalances.typeIndex:
                        {
                            if (NotificationController.GetLogInUser().LangCode.Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                                return type1.Name.CompareTo(type2.Name);
                            else
                                return type1.NameAlt.CompareTo(type2.NameAlt);
                        }                    
                    case EmployeesCountersBalances.earnedIndex:
                        return p1.ValueEarned.CompareTo(p2.ValueEarned);
                    case EmployeesCountersBalances.usedIndex:
                        return p1.ValueUsed.CompareTo(p2.ValueUsed);
                    case EmployeesCountersBalances.balanceIndex:
                        return p1.Balance.CompareTo(p2.Balance);
                    default:
                        return empl1.FirstAndLastName.CompareTo(empl2.FirstAndLastName);
                }
            }
        }

        #endregion

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("EmployeesCountersBalances", culture);

                //label's text
                this.lblEmployee.Text = rm.GetString("lblEmployee", culture);
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);

                //button's text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnPreview.Text = rm.GetString("btnPreview", culture);
                this.btnExport.Text = rm.GetString("btnExport", culture);

                //group box text
                this.gbDateInterval.Text = rm.GetString("gbTimeInterval", culture);
                this.gbUnitFilter.Text = rm.GetString("gbUnitFilter", culture);
                                
                // check box text
                this.chbHierarhiclyWU.Text = rm.GetString("chbHierarhicly", culture);
                this.chbHierachyOU.Text = rm.GetString("chbHierarhicly", culture);

                // list view                
                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrID", culture), 75, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrName", culture), lvEmployees.Width - 100, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();

                lvCounters.BeginUpdate();
                lvCounters.Columns.Add(rm.GetString("hdrCounterType", culture), lvCounters.Width - 22, HorizontalAlignment.Left);
                lvCounters.EndUpdate();

                lvBalances.BeginUpdate();
                lvBalances.Columns.Add(rm.GetString("hdrID", culture), 75, HorizontalAlignment.Left);
                lvBalances.Columns.Add(rm.GetString("hdrName", culture), 150, HorizontalAlignment.Left);
                lvBalances.Columns.Add(rm.GetString("hdrMonth", culture), 80, HorizontalAlignment.Left);
                lvBalances.Columns.Add(rm.GetString("hdrCounterType", culture), 140, HorizontalAlignment.Left);
                lvBalances.Columns.Add(rm.GetString("hdrValueEarned", culture) + " (h)", 85, HorizontalAlignment.Left);
                lvBalances.Columns.Add(rm.GetString("hdrValueUsed", culture) + " (h)", 85, HorizontalAlignment.Left);
                lvBalances.Columns.Add(rm.GetString("hdrBalance", culture) + " (h)", 85, HorizontalAlignment.Left);
                lvBalances.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeesCountersBalances.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void EmployeesCountersBalances_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Initialize comparer objects
                _comp = new ListViewItemComparer(lvEmployees);
                _comp.SortColumn = emplNameIndex;
                lvEmployees.ListViewItemSorter = _comp;
                lvEmployees.Sorting = SortOrder.Ascending;
                                
                ouDict = new OrganizationalUnit().SearchDictionary();
                wuDict = new WorkingUnit().getWUDictionary();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                    oUnits = new ApplUserXOrgUnit().FindOUForUserDictionary(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                    wuList.Add(wUnit.WorkingUnitID);
                }

                if (wuString.Length > 0)
                    wuString = wuString.Substring(0, wuString.Length - 1);

                foreach (int id in oUnits.Keys)
                {
                    ouString += id.ToString().Trim() + ",";
                    ouList.Add(id);
                }

                if (ouString.Length > 0)
                    ouString = ouString.Substring(0, ouString.Length - 1);
                
                populateWU();
                populateOU();
                populateCounters();

                btnNext.Visible = btnPrev.Visible = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeesCountersBalances.EmployeesCountersBalances_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateCounters()
        {
            try
            {
                lvCounters.BeginUpdate();
                lvCounters.Items.Clear();

                List<EmployeeCounterTypeTO> counterTypes = new EmployeeCounterType().Search();

                foreach (EmployeeCounterTypeTO type in counterTypes)
                {
                    if (!typeDict.ContainsKey(type.EmplCounterTypeID))
                        typeDict.Add(type.EmplCounterTypeID, type);

                    ListViewItem item = new ListViewItem();

                    if (logInUser.LangCode.Trim().ToUpper() == Constants.Lang_sr.Trim().ToUpper())
                        item.Text = type.Name;
                    else
                        item.Text = type.NameAlt;
                    item.Tag = type;                    
                    lvCounters.Items.Add(item);
                }

                lvCounters.EndUpdate();
                lvCounters.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesCountersBalances.populateCounters(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void rbWU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cbWU.Enabled = rbWU.Checked;
                cbOU.Enabled = !rbWU.Checked;
                chbHierarhiclyWU.Enabled = rbWU.Checked;
                chbHierachyOU.Enabled = !rbWU.Checked;

                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeesCountersBalances.rbWU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbOU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cbWU.Enabled = !rbOU.Checked;
                cbOU.Enabled = rbOU.Checked;
                chbHierarhiclyWU.Enabled = !rbOU.Checked;
                chbHierachyOU.Enabled = rbOU.Checked;

                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeesCountersBalances.rbOU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateWU()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    wuArray.Add(wuTO);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWU.DataSource = wuArray;
                cbWU.DisplayMember = "Name";
                cbWU.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeesCountersBalances.populateWU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateOU()
        {
            try
            {
                List<OrganizationalUnitTO> ouArray = new List<OrganizationalUnitTO>();

                foreach (int id in oUnits.Keys)
                {
                    ouArray.Add(oUnits[id]);
                }

                ouArray.Insert(0, new OrganizationalUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), ""));

                cbOU.DataSource = ouArray;
                cbOU.DisplayMember = "Name";
                cbOU.ValueMember = "OrgUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeesCountersBalances.populateOU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private DateTime getMonthBegining(DateTime month)
        {
            try
            {
                return new DateTime(month.Year, month.Month, 1).Date;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeesCountersBalances.populateOU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateEmployees()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<EmployeeTO> employeeList = new List<EmployeeTO>();

                DateTime from = getMonthBegining(dtpFrom.Value.Date);
                DateTime to = getMonthBegining(dtpTo.Value.Date).AddMonths(1).AddDays(-1);

                if (from.Date > to.Date)
                {
                    MessageBox.Show(rm.GetString("invalidDateInterval", culture));
                    return;
                }

                bool isWU = rbWU.Checked;

                int ID = -1;
                if (isWU && cbWU.SelectedIndex > 0)
                    ID = (int)cbWU.SelectedValue;
                else if (!isWU && cbOU.SelectedIndex > 0)
                    ID = (int)cbOU.SelectedValue;

                if (ID != -1)
                {
                    if (isWU)
                    {
                        string wunits = "";
                        if (chbHierarhiclyWU.Checked)
                            wunits = Common.Misc.getWorkingUnitHierarhicly(ID, wuList, null);
                        else
                            wunits = ID.ToString().Trim();

                        // get employees from selected working unit that are not currently loaned to other working unit or are currently loand to selected working unit                        
                        employeeList = new Employee().SearchByWULoans(wunits, -1, null, from.Date, to.Date);
                    }
                    else
                    {
                        string ounits = "";
                        if (chbHierachyOU.Checked)
                            ounits = Common.Misc.getOrgUnitHierarhicly(ID.ToString(), ouList, null);
                        else
                            ounits = ID.ToString().Trim();

                        employeeList = new Employee().SearchByOU(ounits, -1, null, from.Date, to.Date);
                    }
                }

                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();

                foreach (EmployeeTO empl in employeeList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = empl.EmployeeID.ToString().Trim();
                    item.SubItems.Add(empl.FirstAndLastName);
                    item.ToolTipText = empl.FirstAndLastName;

                    item.Tag = empl;
                    lvEmployees.Items.Add(item);
                }

                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeesCountersBalances.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarhiclyWU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeesCountersBalances.chbHierarhiclyWU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbHierachyOU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeesCountersBalances.chbHierachyOU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeesCountersBalances.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbOU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeesCountersBalances.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        
        private void tbEmployee_TextChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string text = tbEmployee.Text.Trim();

                int id = -1;

                lvEmployees.SelectedItems.Clear();

                if (int.TryParse(text, out id))
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        if (((EmployeeTO)item.Tag).EmployeeID.ToString().Trim().StartsWith(id.ToString().Trim()))
                        {
                            item.Selected = true;
                            lvEmployees.Select();
                            lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));

                            break;
                        }
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        if (((EmployeeTO)item.Tag).FirstAndLastName.ToString().Trim().ToUpper().StartsWith(text.ToString().Trim().ToUpper()))
                        {
                            item.Selected = true;
                            lvEmployees.Select();
                            lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));

                            break;
                        }
                    }
                }

                tbEmployee.Focus();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesCountersBalances.tbEmployee_TextChanged(): " + ex.Message + "\n");
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

                SortOrder prevOrder = lvEmployees.Sorting;

                if (e.Column == _comp.SortColumn)
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
                    _comp.SortColumn = e.Column;
                    lvEmployees.Sorting = SortOrder.Ascending;
                }

                lvEmployees.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesCountersBalances.lvEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeesCountersBalances.dtpFrom_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeesCountersBalances.dtpTo_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnWUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits(wuString);
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    this.cbWU.SelectedIndex = cbWU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesCountersBalances.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                System.Data.DataSet dsOrgUnits = new OrganizationalUnit().getOrganizationUnits(ouString);
                WorkingUnitsTreeView orgUnitsTreeView = new WorkingUnitsTreeView(dsOrgUnits);
                orgUnitsTreeView.ShowDialog();
                if (!orgUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbOU.SelectedIndex = cbOU.FindStringExact(orgUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesCountersBalances.btnOUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
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
                populatePreview();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesCountersBalances.btnPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnNext_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex += Constants.recordsPerPage;
                populatePreview();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesCountersBalances.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        public void populatePreview()
        {
            try
            {
                if (balanceList.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvBalances.BeginUpdate();
                lvBalances.Items.Clear();

                if ((startIndex >= 0) && (startIndex < balanceList.Count))
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
                    if (lastIndex >= balanceList.Count)
                    {
                        btnNext.Enabled = false;
                        lastIndex = balanceList.Count;
                    }
                    else
                    {
                        btnNext.Enabled = true;
                    }

                    for (int i = startIndex; i < lastIndex; i++)
                    {
                        ListViewItem item = new ListViewItem();                        

                        item.Text = balanceList[i].EmployeeID.ToString().Trim();
                        if (emplDict.ContainsKey(balanceList[i].EmployeeID))
                            item.SubItems.Add(emplDict[balanceList[i].EmployeeID].FirstAndLastName.Trim());
                        else
                            item.SubItems.Add("N/A");
                        item.SubItems.Add(balanceList[i].Month.Date.ToString("MM.yyyy."));
                        if (typeDict.ContainsKey(balanceList[i].EmplCounterTypeID))
                        {
                            if (logInUser.LangCode.Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                                item.SubItems.Add(typeDict[balanceList[i].EmplCounterTypeID].Name.Trim());
                            else
                                item.SubItems.Add(typeDict[balanceList[i].EmplCounterTypeID].NameAlt.Trim());
                        }
                        else
                            item.SubItems.Add("N/A");
                        item.SubItems.Add(((decimal)balanceList[i].ValueEarned / 60).ToString(Constants.doubleFormat));
                        item.SubItems.Add(((decimal)balanceList[i].ValueUsed / 60).ToString(Constants.doubleFormat));
                        item.SubItems.Add(((decimal)balanceList[i].Balance / 60).ToString(Constants.doubleFormat));
                                                
                        item.Tag = balanceList[i];
                        
                        lvBalances.Items.Add(item);
                    }
                }

                lvBalances.EndUpdate();
                lvBalances.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesCountersBalances.populatePreview(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                DateTime from = getMonthBegining(dtpFrom.Value.Date);
                DateTime to = getMonthBegining(dtpTo.Value.Date).AddMonths(1).AddDays(-1);

                if (from.Date > to.Date)
                {
                    MessageBox.Show(rm.GetString("invalidDateInterval", culture));
                    return;
                }

                string IDs = "";
                emplDict = new Dictionary<int, EmployeeTO>();
                balanceList = new List<EmployeeCounterMonthlyBalanceTO>();
                if (lvEmployees.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvEmployees.SelectedItems)
                    {
                        IDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";
                        if (!emplDict.ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                            emplDict.Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        IDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";
                        if (!emplDict.ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                            emplDict.Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);
                    }
                }

                if (IDs.Length > 0)
                    IDs = IDs.Substring(0, IDs.Length - 1);                

                string types = "";
                if (lvCounters.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvCounters.SelectedItems)
                    {
                        types += ((EmployeeCounterTypeTO)item.Tag).EmplCounterTypeID.ToString().Trim() + ",";                        
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvCounters.Items)
                    {
                        types += ((EmployeeCounterTypeTO)item.Tag).EmplCounterTypeID.ToString().Trim() + ","; 
                    }
                }

                if (types.Length > 0)
                    types = types.Substring(0, types.Length - 1); 

                if (IDs.Length <= 0 || types.Length <= 0)
                {
                    MessageBox.Show(rm.GetString("noPreviewData", culture));
                    return;
                }

                balanceList = new EmployeeCounterMonthlyBalance().SearchEmployeeBalances(IDs, types, from, to);
                balanceList.Sort(new ArrayListSort(sortOrder, sortField, emplDict, typeDict));
                populatePreview();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesCountersBalances.btnPreview_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvBalances.Items.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noReportData", culture));
                    return;
                }

                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                object misValue = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;

                // insert header                  
                ws.Cells[1, 1] = rm.GetString("hdrID", culture);
                ws.Cells[1, 2] = rm.GetString("hdrName", culture);
                ws.Cells[1, 3] = rm.GetString("hdrMonth", culture);
                ws.Cells[1, 4] = rm.GetString("hdrCounterType", culture);                
                ws.Cells[1, 5] = rm.GetString("hdrValueEarned", culture);
                ws.Cells[1, 6] = rm.GetString("hdrValueUsed", culture);
                ws.Cells[1, 7] = rm.GetString("hdrBalance", culture);
                ws.Cells[1, 8] = rm.GetString("hdrForPaying", culture);
                
                int colNum = 8;

                setRowFontWeight(ws, 1, colNum, true);

                int row = 2;

                for (int i = 0; i < balanceList.Count; i++)
                {
                    ws.Cells[row, 1] = balanceList[i].EmployeeID.ToString().Trim();
                    if (emplDict.ContainsKey(balanceList[i].EmployeeID))
                        ws.Cells[row, 2] = emplDict[balanceList[i].EmployeeID].FirstAndLastName.Trim();
                    else
                        ws.Cells[row, 2] = "N/A";
                    ws.Cells[row, 3] = balanceList[i].Month.Date.ToString("MM.yyyy.");
                    if (typeDict.ContainsKey(balanceList[i].EmplCounterTypeID))
                    {
                        if (logInUser.LangCode.Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                            ws.Cells[row, 4] = typeDict[balanceList[i].EmplCounterTypeID].Name.Trim();
                        else
                            ws.Cells[row, 4] = typeDict[balanceList[i].EmplCounterTypeID].NameAlt.Trim();
                    }
                    else
                        ws.Cells[row, 4] = "N/A";
                    ws.Cells[row, 5] = ((decimal)balanceList[i].ValueEarned / 60).ToString(Constants.doubleFormat);
                    ws.Cells[row, 6] = ((decimal)balanceList[i].ValueUsed / 60).ToString(Constants.doubleFormat);
                    ws.Cells[row, 7] = ((decimal)balanceList[i].Balance / 60).ToString(Constants.doubleFormat);
                    
                    row++;
                }

                ws.Columns.AutoFit();
                ws.Rows.AutoFit();

                string reportName = "CounterBalancePreview_" + DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss");

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = reportName;
                sfd.InitialDirectory = Constants.csvDocPath;
                sfd.Filter = "XLS (*.xls)|*.xls";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                string filePath = sfd.FileName;

                wb.SaveAs(filePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
                                    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                                    Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, misValue, misValue, misValue, misValue);

                wb.Close(true, null, null);
                xla.Workbooks.Close();
                xla.Quit();

                releaseObject(ws);
                releaseObject(wb);
                releaseObject(xla);

                System.Threading.Thread.CurrentThread.CurrentCulture = Oldci;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesCountersBalances.btnExport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvBalances_ColumnClick(object sender, ColumnClickEventArgs e)
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
                    sortField = e.Column;
                    sortOrder = Constants.sortAsc;
                }

                balanceList.Sort(new ArrayListSort(sortOrder, sortField, emplDict, typeDict));
                populatePreview();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeesCountersBalances.lvBalances_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setRowFontWeight(Microsoft.Office.Interop.Excel.Worksheet ws, int row, int colNum, bool isBold)
        {
            try
            {
                for (int i = 1; i <= colNum; i++)
                {
                    ((Microsoft.Office.Interop.Excel.Range)ws.Cells[row, i]).Font.Bold = isBold;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeesCountersBalances.setRowFontWeight(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " EmployeesCountersBalances.releaseObject(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
