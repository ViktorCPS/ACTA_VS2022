using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.IO;

using Util;
using Common;
using TransferObjects;

namespace UI
{
    public partial class TransportData : Form
    {
        private const int emplIDIndex = 0;
        private const int emplNameIndex = 1;
        
        private int emplSortField;

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

        int startIndex = 0;
        Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>> dataDict = new Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>>();
        Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
        Dictionary<int, EmployeeAsco4TO> ascoDict = new Dictionary<int, EmployeeAsco4TO>();
        DateTime month = new DateTime();
        Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>> emplDayIntervals = new Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>>();

        Dictionary<int, EmployeePYTransportTypeTO> typeDict = new Dictionary<int, EmployeePYTransportTypeTO>();
        List<EmployeePYTransportTypeTO> typeList = new List<EmployeePYTransportTypeTO>();
        Dictionary<int, WorkTimeSchemaTO> schemas = new Dictionary<int, WorkTimeSchemaTO>();

        public TransportData()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(TransportData).Assembly);

                logInUser = NotificationController.GetLogInUser();

                setLanguage();

                rbWU.Checked = true;

                dtpMonth.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    case TransportData.emplIDIndex:
                        {
                            int id1 = -1;
                            int id2 = -1;

                            int.TryParse(sub1.Text, out id1);
                            int.TryParse(sub2.Text, out id2);

                            return CaseInsensitiveComparer.Default.Compare(id1, id2);
                        }
                    case TransportData.emplNameIndex:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
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
                this.Text = rm.GetString("TransportData", culture);

                //label's text
                this.lblEmployee.Text = rm.GetString("lblEmployee", culture);

                //button's text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnSearch.Text = rm.GetString("btnSearch", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);
                this.btnSetDefault.Text = rm.GetString("btnSetDefault", culture);
                this.btnValidate.Text = rm.GetString("btnValidate", culture);

                //group box text
                this.gbMonth.Text = rm.GetString("gbMonth", culture);
                this.gbUnitFilter.Text = rm.GetString("gbUnitFilter", culture);
                this.gbSearch.Text = rm.GetString("gbSearch", culture);

                // list view                
                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrID", culture), 70, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrName", culture), 165, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " GeoxPYReport.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void TransportData_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                btnPrev.Visible = false;
                btnNext.Visible = false;

                emplSortField = emplNameIndex;

                // Initialize comparer objects
                _comp = new ListViewItemComparer(lvEmployees);
                _comp.SortColumn = emplSortField;
                lvEmployees.ListViewItemSorter = _comp;
                lvEmployees.Sorting = SortOrder.Ascending;

                wuDict = new WorkingUnit().getWUDictionary();
                ouDict = new OrganizationalUnit().SearchDictionary();
                
                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                    oUnits = new ApplUserXOrgUnit().FindOUForUserDictionary(logInUser.UserID.Trim(), Constants.ReportPurpose);
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

                rbWU.Checked = true;
                rbWU_CheckedChanged(this, new EventArgs());

                typeDict = new EmployeePYTransportType().SearchEmployeeTransportTypes();

                EmployeePYTransportTypeTO typeAll = new EmployeePYTransportTypeTO();
                typeAll.Name = rm.GetString("all", culture);

                typeList.Add(typeAll);

                foreach (int key in typeDict.Keys)
                {
                    typeList.Add(typeDict[key]);
                }

                schemas = new TimeSchema().getDictionary();

                month = dtpMonth.Value.Date;
                
                populateTransportData(false);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TransportData.TransportData_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TransportData.populateWU(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TransportData.populateOU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateEmployees()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<EmployeeTO> employeeList = new List<EmployeeTO>();

                bool isWU = rbWU.Checked;

                int ID = -1;
                if (isWU && cbWU.SelectedIndex > 0)
                    ID = (int)cbWU.SelectedValue;
                else if (!isWU && cbOU.SelectedIndex > 0)
                    ID = (int)cbOU.SelectedValue;

                DateTime from = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
                DateTime to = from.AddMonths(1).AddDays(-1); ;

                if (from > to)
                    MessageBox.Show(rm.GetString("invalidDateInterval", culture));
                else if (ID != -1)
                {
                    if (isWU)
                    {
                        string wunits = Common.Misc.getWorkingUnitHierarhicly(ID, wuList, null);

                        // get employees from selected working unit that are not currently loaned to other working unit or are currently loand to selected working unit                        
                        employeeList = new Employee().SearchByWULoans(wunits, -1, null, from.Date, to.Date);
                    }
                    else
                    {
                        string ounits = Common.Misc.getOrgUnitHierarhicly(ID.ToString(), ouList, null);

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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TransportData.populateEmployees(): " + ex.Message + "\n");
                throw ex;
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
                cbWU.Enabled = rbWU.Checked;

                cbOU.Enabled = !rbWU.Checked;

                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TransportData.rbWU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbOU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cbWU.Enabled = !rbOU.Checked;

                cbOU.Enabled = rbOU.Checked;

                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TransportData.rbOU_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TransportData.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TransportData.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
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

                if (!text.Trim().Equals(""))
                {
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
                }

                tbEmployee.Focus();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TransportData.tbEmployee_TextChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " TransportData.lvEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dtpMonth_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TransportData.dtpMonth_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public void populateTransportData(bool saveValues)
        {
            try
            {
                if (dataDict.Keys.Count > Constants.recPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }
                
                foreach (Control ctrl in dataPanel.Controls)
                {
                    if (ctrl is EmployeeTransportData)
                    {
                        if (saveValues)
                        {
                            Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>> dict = ((EmployeeTransportData)ctrl).GetDataValues();

                            foreach (int id in dict.Keys)
                            {
                                if (id != -1)
                                {
                                    if (!dataDict.ContainsKey(id))
                                        dataDict.Add(id, dict[id]);
                                    else
                                        dataDict[id] = dict[id];
                                }
                            }
                        }
                    }
                }

                dataPanel.Controls.Clear();

                GC.Collect();

                int xPos = 0;
                int yPos = 0;

                // insert header line (name label + days of month)
                EmployeeTransportData hdr = new EmployeeTransportData(new EmployeeTO(), new Dictionary<DateTime, EmployeePYTransportDataTO>(), month, typeDict, typeList,
                    new Dictionary<DateTime, List<WorkTimeIntervalTO>>(), xPos, yPos, true);
                dataPanel.Controls.Add(hdr);

                yPos += hdr.Height;

                if (dataDict.Keys.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < dataDict.Keys.Count))
                    {
                        if (startIndex == 0)
                        {
                            btnPrev.Enabled = false;
                        }
                        else
                        {
                            btnPrev.Enabled = true;
                        }

                        int lastIndex = startIndex + Constants.recPerPage;
                        if (lastIndex >= dataDict.Keys.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = dataDict.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }
                        
                        IEnumerator dataEnumerator = dataDict.Keys.GetEnumerator();
                        int i = 0;
                        while (dataEnumerator.MoveNext() && i < lastIndex)
                        {
                            if (i < startIndex)
                            {
                                i++;
                                continue;
                            }

                            int id = (int)dataEnumerator.Current;

                            EmployeeTO empl = new EmployeeTO();
                            Dictionary<DateTime, EmployeePYTransportDataTO> data = new Dictionary<DateTime, EmployeePYTransportDataTO>();
                            Dictionary<DateTime, List<WorkTimeIntervalTO>> dayIntervals = new Dictionary<DateTime, List<WorkTimeIntervalTO>>();

                            if (emplDict.ContainsKey(id))
                                empl = emplDict[id];

                            if (dataDict.ContainsKey(id))                            
                                data = dataDict[id];

                            if (emplDayIntervals.ContainsKey(empl.EmployeeID))
                                dayIntervals = emplDayIntervals[empl.EmployeeID];

                            EmployeeTransportData ctrl = new EmployeeTransportData(empl, data, month, typeDict, typeList, dayIntervals, xPos, yPos, false);                            
                            dataPanel.Controls.Add(ctrl);
                            ctrl.SetDataValues();
                            yPos += ctrl.Height;
                            
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TransportData.populateTransportData(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string emplIDs = "";
                emplDict = new Dictionary<int, EmployeeTO>();
                ascoDict = new Dictionary<int, EmployeeAsco4TO>();
                dataDict = new Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>>();
                month = new DateTime(dtpMonth.Value.Date.Year, dtpMonth.Value.Date.Month, 1);
                DateTime firstDay = new DateTime(month.Year, month.Month, 1);
                DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
                emplDayIntervals = new Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>>();
                startIndex = 0;

                if (lvEmployees.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvEmployees.SelectedItems)
                    {
                        if (item.Tag != null && item.Tag is EmployeeTO)
                        {
                            emplIDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";

                            if (!emplDict.ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                                emplDict.Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);
                        }
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        if (item.Tag != null && item.Tag is EmployeeTO)
                        {
                            emplIDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";

                            if (!emplDict.ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                                emplDict.Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);
                        }
                    }
                }

                Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>> transportDict = new Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>>();
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new Dictionary<int, List<EmployeeTimeScheduleTO>>();

                if (emplIDs.Length > 0)
                {
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                    transportDict = new EmployeePYTransportData().Search(emplIDs, month);
                    ascoDict = new EmployeeAsco4().SearchDictionary(emplIDs);
                    emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, firstDay.Date, lastDay.Date, null);
                }

                foreach (int id in emplDict.Keys)
                {
                    if (!dataDict.ContainsKey(id))
                        dataDict.Add(id, new Dictionary<DateTime, EmployeePYTransportDataTO>());

                    if (transportDict.ContainsKey(id))
                        dataDict[id] = transportDict[id];

                    if (!emplDayIntervals.ContainsKey(id))
                        emplDayIntervals.Add(id, new Dictionary<DateTime, List<WorkTimeIntervalTO>>());

                    List<EmployeeTimeScheduleTO> schList = new List<EmployeeTimeScheduleTO>();
                    if (emplSchedules.ContainsKey(id))
                        schList = emplSchedules[id];
                    for (DateTime day = firstDay.Date; day.Date <= lastDay.Date; day = day.Date.AddDays(1))
                    {
                        if (!emplDayIntervals[id].ContainsKey(day.Date))
                            emplDayIntervals[id].Add(day.Date, Common.Misc.getTimeSchemaInterval(day.Date, schList, schemas));
                    }
                }

                populateTransportData(false);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TransportData.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void TransportData_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                dataPanel.Controls.Clear();

                GC.Collect();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TransportData.TransportData_FormClosing(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                startIndex -= Constants.recPerPage;

                if (startIndex < 0)
                    startIndex = 0;

                populateTransportData(true);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TransportData.btnPrev_Click(): " + ex.Message + "\n");
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

                startIndex += Constants.recPerPage;

                populateTransportData(true);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TransportData.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSetDefault_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataDict.Keys.Count > 0)
                {
                    this.Cursor = Cursors.WaitCursor;

                    DateTime firstDay = new DateTime(month.Year, month.Month, 1);
                    DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
                    
                    string emplIDs = "";
                    foreach (int id in dataDict.Keys)
                    {
                        emplIDs += id.ToString().Trim() + ",";
                    }

                    if (emplIDs.Length > 0)
                    {
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                        
                        foreach (int id in dataDict.Keys)
                        {
                            if (ascoDict.ContainsKey(id) && typeDict.ContainsKey(ascoDict[id].IntegerValue1))
                            {
                                for (DateTime day = firstDay.Date; day <= lastDay.Date; day = day.AddDays(1))
                                {
                                    // get intervals for current day
                                    List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                                    
                                    if (emplDayIntervals.ContainsKey(id) && emplDayIntervals[id].ContainsKey(day.Date))
                                        intervals = emplDayIntervals[id][day.Date];

                                    bool workingDay = false;
                                    foreach (WorkTimeIntervalTO interval in intervals)
                                    {
                                        if (interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                        {
                                            workingDay = true;
                                            break;
                                        }
                                    }

                                    if (workingDay)
                                    {
                                        if (!dataDict[id].ContainsKey(day.Date))
                                        {
                                            EmployeePYTransportDataTO dataTO = new EmployeePYTransportDataTO();
                                            dataTO.EmployeeID = id;
                                            dataTO.Date = day.Date;
                                            dataTO.TransportTypeID = ascoDict[id].IntegerValue1;

                                            dataDict[id].Add(day.Date, dataTO);
                                        }
                                        else if (dataDict[id][day.Date].TransportTypeID == -1)
                                            dataDict[id][day.Date].TransportTypeID = ascoDict[id].IntegerValue1;
                                    }
                                }
                            }
                        }

                        startIndex = 0;
                        populateTransportData(false);

                        MessageBox.Show(rm.GetString("deafultDataSet", culture));
                    }
                }
                else
                    MessageBox.Show(rm.GetString("noData", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TransportData.btnSetDefault_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Control ctrl in dataPanel.Controls)
                {
                    if (ctrl is EmployeeTransportData)
                    {
                        Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>> dict = ((EmployeeTransportData)ctrl).GetDataValues();

                        foreach (int id in dict.Keys)
                        {
                            if (id != -1)
                            {
                                if (!dataDict.ContainsKey(id))
                                    dataDict.Add(id, dict[id]);
                                else
                                    dataDict[id] = dict[id];
                            }
                        }
                    }
                }

                if (dataDict.Keys.Count > 0)
                {
                    // check if transport type is selected for every working day and not selected for not working days
                    DateTime firstDay = new DateTime(month.Year, month.Month, 1);
                    DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);

                    bool valid = true;
                    foreach (int id in dataDict.Keys)
                    {
                        for (DateTime day = firstDay.Date; day <= lastDay.Date; day = day.Date.AddDays(1))
                        {
                            List<WorkTimeIntervalTO> intervals = new List<WorkTimeIntervalTO>();
                            if (emplDayIntervals.ContainsKey(id) && emplDayIntervals[id].ContainsKey(day.Date))
                                intervals = emplDayIntervals[id][day.Date];

                            bool workingDay = false;
                            foreach (WorkTimeIntervalTO interval in intervals)
                            {
                                if (interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                                {
                                    workingDay = true;
                                    break;
                                }
                            }

                            int typeID = -1;
                            if (dataDict[id].ContainsKey(day.Date))
                                typeID = dataDict[id][day.Date].TransportTypeID;

                            if ((workingDay && typeID == -1) || (!workingDay && typeID != -1))
                            {
                                valid = false;
                                break;
                            }
                        }

                        if (!valid)
                            break;
                    }

                    if (!valid)
                    {
                        DialogResult result = MessageBox.Show(rm.GetString("notValidTransportDaysExists", culture), "", MessageBoxButtons.YesNo);

                        if (result == DialogResult.No)
                            return;
                    }
                    
                    string emplIDs = "";
                    foreach (int id in dataDict.Keys)
                    {
                        emplIDs += id.ToString().Trim() + ",";
                    }

                    if (emplIDs.Length > 0)
                    {
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                        EmployeePYTransportData data = new EmployeePYTransportData();
                        if (data.BeginTransaction())
                        {
                            try
                            {
                                bool saved = true;
                                // delete existing employee month data
                                saved = saved && data.Delete(emplIDs, firstDay, lastDay, false);

                                if (saved)
                                {
                                    // insert new month data for employees
                                    foreach (int id in dataDict.Keys)
                                    {
                                        foreach (DateTime date in dataDict[id].Keys)
                                        {
                                            if (dataDict[id][date].TransportTypeID != -1)
                                            {
                                                data.DataTO = dataDict[id][date];
                                                saved = saved && (data.Save(false) > 0);
                                            }

                                            if (!saved)
                                                break;
                                        }

                                        if (!saved)
                                            break;
                                    }
                                }

                                if (saved)
                                {
                                    data.CommitTransaction();

                                    MessageBox.Show(rm.GetString("dataSaved", culture));
                                }
                                else
                                {
                                    if (data.GetTransaction() != null)
                                        data.RollbackTransaction();

                                    MessageBox.Show(rm.GetString("dataNotSaved", culture));
                                }
                            }
                            catch (Exception ex)
                            {
                                if (data.GetTransaction() != null)
                                    data.RollbackTransaction();

                                throw ex;
                            }
                        }
                        else
                            MessageBox.Show(rm.GetString("dataNotSaved", culture));
                    }
                }
                else
                    MessageBox.Show(rm.GetString("noDataToSave", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TransportData.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string emplIDs = "";
                Dictionary<int, EmployeeTO> employeeDict = new Dictionary<int, EmployeeTO>();                
                DateTime selMonth = new DateTime(dtpMonth.Value.Date.Year, dtpMonth.Value.Date.Month, 1);
                DateTime firstDay = new DateTime(selMonth.Year, selMonth.Month, 1);
                DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
                
                if (lvEmployees.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvEmployees.SelectedItems)
                    {
                        if (item.Tag != null && item.Tag is EmployeeTO)
                        {
                            emplIDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";

                            if (!employeeDict.ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                                employeeDict.Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);
                        }
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        if (item.Tag != null && item.Tag is EmployeeTO)
                        {
                            emplIDs += ((EmployeeTO)item.Tag).EmployeeID.ToString().Trim() + ",";

                            if (!employeeDict.ContainsKey(((EmployeeTO)item.Tag).EmployeeID))
                                employeeDict.Add(((EmployeeTO)item.Tag).EmployeeID, (EmployeeTO)item.Tag);
                        }
                    }
                }

                Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>> transportDict = new Dictionary<int, Dictionary<DateTime, EmployeePYTransportDataTO>>();
                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new Dictionary<int, List<EmployeeTimeScheduleTO>>();

                if (emplIDs.Length > 0)
                {
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                    transportDict = new EmployeePYTransportData().Search(emplIDs, selMonth);
                    emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, firstDay.Date, lastDay.Date, null);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noData", culture));
                    return;
                }

                Dictionary<int, List<DateTime>> notValidDict = new Dictionary<int, List<DateTime>>();
                foreach (int id in employeeDict.Keys)
                {
                    List<EmployeeTimeScheduleTO> schList = new List<EmployeeTimeScheduleTO>();
                    if (emplSchedules.ContainsKey(id))
                        schList = emplSchedules[id];
                    for (DateTime day = firstDay.Date; day.Date <= lastDay.Date; day = day.Date.AddDays(1))
                    {
                        List<WorkTimeIntervalTO> intervals = Common.Misc.getTimeSchemaInterval(day.Date, schList, schemas);

                        bool workingDay = false;
                        foreach (WorkTimeIntervalTO interval in intervals)
                        {
                            if (interval.StartTime.TimeOfDay != new TimeSpan(0, 0, 0))
                            {
                                workingDay = true;
                                break;
                            }
                        }

                        int typeID = -1;

                        if (transportDict.ContainsKey(id) && transportDict[id].ContainsKey(day.Date))
                            typeID = transportDict[id][day.Date].TransportTypeID;

                        if ((workingDay && typeID == -1) || (!workingDay && typeID != -1))
                        {
                            if (!notValidDict.ContainsKey(id))
                                notValidDict.Add(id, new List<DateTime>());

                            notValidDict[id].Add(day.Date);
                        }
                    }
                }

                if (notValidDict.Keys.Count <= 0)
                    MessageBox.Show(rm.GetString("validTransportDays", culture));
                else
                    MessageBox.Show(rm.GetString("notValidTransportDaysExist", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TransportData.btnValidate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
