using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Resources;
using System.Globalization;
using System.Drawing.Printing;

using TransferObjects;
using Common;
using Reports;
using Util;

namespace UI
{
    public partial class PresenceGraphForDayControl : UserControl
    {
       protected DebugLog log;
       protected ResourceManager rm;
       protected CultureInfo culture;
       protected ApplUserTO logInUser;
       protected List<WorkTimeSchemaTO> timeSchema;
      
        // Key is Pass Type Id, Value is Pass Type
       protected Dictionary<int, PassTypeTO> passTypes = new Dictionary<int,PassTypeTO>();
       // Controller instance
       public NotificationController Controller;
       // Observer client instance
       public NotificationObserverClient observerClient;

       protected List<IOPairTO> currentIOPairList;
       protected int startIndex;
        // Current WOking Unit
       protected WorkingUnitTO currentWorkingUnit ;
       protected List<WorkingUnitTO> currentWorkingUnitsList ;
        //Current Employee
       protected EmployeeTO currentEmployee;
       protected List<EmployeeTO> currentEmployeesList;
       protected List<EmployeeTO> selectedEmployeesList;
        //lvEmployee sort
       protected int sortOrder;
       protected int sortField;
       protected int sortList;

       protected List<WorkingUnitTO> wUnits ;
       protected string wuString = "";
        
        //Working units List View indexes
        const int WUName = 0;
        const int ParentWUID = 1;

        protected ListViewItemComparer _comp;
        //properties for printing
        private PageSettings pgSettings = new PageSettings();
        PrintDocument printDocument1 = new PrintDocument();
        private Bitmap memoryImage;
        DailyTimeGrid dailyTimeGrid = new DailyTimeGrid();
        EmployeeWorkingDayView employeeWorkingDayView = new EmployeeWorkingDayView();

        //for locations tree view
        List<LocationTO> locArray;

        public PresenceGraphForDayControl()
        {
            InitializeComponent();
            InitObserverClient();
        }

        protected void PresenceGraphForDayControl_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (!this.DesignMode)
                {
                    currentEmployee = new EmployeeTO();
                    currentWorkingUnit = new WorkingUnitTO();


                    setVisibility();

                    currentEmployeesList = new List<EmployeeTO>();
                    currentWorkingUnitsList = new List<WorkingUnitTO>();

                    startIndex = 0;
                    // Debug
                    string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                    log = new DebugLog(logFilePath);

                    logInUser = NotificationController.GetLogInUser();
                    culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                    rm = new ResourceManager("UI.Resource", typeof(EmployeePresenceGraphicReports).Assembly);

                    setLanguage();
                    InitialiseListView();
                    populateParentWorkigUnitCombo();
                    populateLocationCombo();
                    if (wUnits.Count > 0)
                    {
                        populateWorkingUnitsListView(wUnits);
                    }

                    // get all Pass Types
                    List<PassTypeTO> passTypesAll = new PassType().Search();
                    foreach (PassTypeTO pt in passTypesAll)
                    {
                        passTypes.Add(pt.PassTypeID, pt);
                    }

                    _comp = new ListViewItemComparer(lvWorkingUnits);
                    sortList = 0;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePresenceGrphicReports.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setVisibility()
        {
            this.btnNext.Visible = false;
            this.btnPrev.Visible = false;
            this.gbPageNavigation.Visible = false;
            this.panelLate.Visible = false;
            this.lblLate.Visible = false;
            this.lblEmployee.Visible = false;
            this.lblTotal.Visible = false;                                  
        }

        public virtual void InitObserverClient()
        {
            Controller = NotificationController.GetInstance();
            observerClient = new NotificationObserverClient(this.ToString());            
        }

        protected void lvEmployees_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
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

                currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField,sortList));
                populateEmployeesListView(currentEmployeesList);
               
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.lvEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show("Exception in lvEmployees_ColumnClick():" + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        protected void btnPrev_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex -= Constants.recordsPerGraph;
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
                DrawGraph(startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.btnPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        protected void btnNext_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex += Constants.recordsPerGraph;
                DrawGraph(startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        protected void lvWorkingUnits_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                SortOrder prevOrder = lvWorkingUnits.Sorting;
                lvWorkingUnits.Sorting = SortOrder.None;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvWorkingUnits.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvWorkingUnits.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvWorkingUnits.Sorting = SortOrder.Ascending;

                } 
                lvWorkingUnits.ListViewItemSorter = _comp;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.lvWorkingUnits_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        protected void setLanguage()
        {
            try
            {
                // group box text
                gbWorkingUnits.Text = rm.GetString("gbWorkingUnits", culture);
                gbEmployees.Text = rm.GetString("gbEmployees", culture);
                gbDay.Text = rm.GetString("gbDay", culture);
                gbGraphicReport.Text = rm.GetString("gbGraphicReport", culture);
                gbDaysNavigation.Text = rm.GetString("gbDaysNavigation", culture);
                gbPageNavigation.Text = rm.GetString("gbPageNavigation", culture);
                gbLegend.Text = rm.GetString("gbLegend", culture);
                gbLocation.Text = rm.GetString("gbLocation", culture);
                gbIsWrkHrs.Text = rm.GetString("gbIsWrkHrs", culture);
                

                // button's text
                btnShow.Text = rm.GetString("btnShow", culture);
                btnPrint.Text = rm.GetString("btnPrint", culture);
                btnClose.Text = rm.GetString("btnClose", culture);

                //radio button's text
                rbAll.Text = rm.GetString("rbAll", culture);
                rbYes.Text = rm.GetString("yes", culture);
                rbNo.Text = rm.GetString("no", culture);

                // label's text
                lblParentWUID.Text = rm.GetString("lblParentWUID", culture);
                lblTotal.Text = rm.GetString("lblTotal", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblLate.Text = rm.GetString("lblLate", culture);
                lblOfficiallyOutgoing.Text = rm.GetString("lblOfficiallyOutgoing", culture);
                lblOutgoingInPrivate.Text = rm.GetString("lblOutgoingInPrivate", culture);
                lblPause.Text = rm.GetString("lbPause", culture);
                lblRegularWork.Text = rm.GetString("lblRegularWork", culture);
                lblSickLeave.Text = rm.GetString("lblSickLeave", culture);
                lblTheRestOfAllDayAbsence.Text = rm.GetString("lblTheRestOfAllDayAbsence", culture);
                lblTheRestOfReaderPasses.Text = rm.GetString("lblTheRestOfReaderPasses", culture);
                lblVacation.Text = rm.GetString("lblVacation", culture);

                //CheckBox's text
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);
                chbSelectAll.Text = rm.GetString("chbSelectAll", culture);
                chbShowNextDay.Text = rm.GetString("chbShowNextDay", culture);

                                               
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePresenceGrphicReports.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        protected virtual void InitialiseListView()
        {
            lvWorkingUnits.BeginUpdate();
            lvWorkingUnits.Columns.Add(rm.GetString("lblWorkingUnit", culture), (lvWorkingUnits.Width - 4) / 2, HorizontalAlignment.Left);
            lvWorkingUnits.Columns.Add(rm.GetString("lblParentWUID", culture), (lvWorkingUnits.Width - 4) / 2 - 20, HorizontalAlignment.Left);
            lvWorkingUnits.EndUpdate();
            lvEmployees.BeginUpdate();
            lvEmployees.Columns.Add(rm.GetString("hdrEmployee", culture), (lvEmployees.Width - 4) / 2, HorizontalAlignment.Left);
            lvEmployees.Columns.Add(rm.GetString("lblWorkingUnit", culture), (lvEmployees.Width - 4) / 2 - 20, HorizontalAlignment.Left);
            lvEmployees.EndUpdate();
        }
        /// <summary>
        /// Populate Location Combo Box
        /// </summary>
        private void populateLocationCombo()
        {
            try
            {
                Location loc = new Location();
                loc.LocTO.Status = Constants.DefaultStateActive.Trim();
                locArray = loc.Search();
                locArray.Insert(0, new LocationTO(-1, rm.GetString("all", culture), rm.GetString("all", culture), 0, 0, ""));

                cbLocation.DataSource = locArray;
                cbLocation.DisplayMember = "Name";
                cbLocation.ValueMember = "LocationID";
                cbLocation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateWorkingUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        protected void populateParentWorkigUnitCombo()
        {
            try
            {
                wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    wuArray.Add(wuTO);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                this.cbParentWorkingUnit.DataSource = wuArray;
                this.cbParentWorkingUnit.DisplayMember = "Name";
                this.cbParentWorkingUnit.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.populateWorkigUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        public void lvWorkingUnits_SelectedIndexChanged(object o, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string wunits = "";
                WorkingUnit wu = new WorkingUnit();
                List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                
                foreach (ListViewItem item in lvWorkingUnits.SelectedItems)
                {
                    foreach (WorkingUnitTO workingUnit in currentWorkingUnitsList)
                    {
                        if (workingUnit.WorkingUnitID == int.Parse(item.Tag.ToString()))
                        {
                            wuList.Add(workingUnit);
                        }
                    }
                }

                 if (chbHierarhicly.Checked)
                {
                    wuList = wu.FindAllChildren(wuList);
                }

                foreach (WorkingUnitTO workingUnit in wuList)
                {
                    wunits += workingUnit.WorkingUnitID + ",";
                }

                if (wunits.Length > 0)
                {
                    wunits = wunits.Substring(0, wunits.Length - 1);
                }

                currentEmployeesList = new Employee().SearchByWU(wunits);
                setSortOrder();
                lvEmployees.Items.Clear();
                populateEmployeesListView(currentEmployeesList);
            }

            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.cbWorkingUnits_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        public void lvEmployees_Click(object o, EventArgs e)
        {
            this.chbSelectAll.Checked = false;
        }

        protected virtual void setSortOrder()
        {
            sortOrder = 0;
            sortField = 0;
            currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField, sortList));
        }

        protected void populateWorkingUnitsListView(List<WorkingUnitTO> WorkingUnitsList)
        {
            try
            {
                lvWorkingUnits.BeginUpdate();
                lvWorkingUnits.Items.Clear();

                WorkingUnit tempWu = new WorkingUnit();
                if (WorkingUnitsList.Count > 0)
                {
                    foreach (WorkingUnitTO wunit in WorkingUnitsList)
                    {
                        ListViewItem item = new ListViewItem();

                        //if ((currentWorkingUnit.Find(Int32.Parse(item.Text.Trim()))) && (currentWorkingUnit.WorkingUnitID != 0))
                        //if ((tempWu.Find(wunit.WorkingUnitID)))// && (tempWu.WorkingUnitID != 0))
                        //{
                            item.Text = wunit.Name.Trim();
                            tempWu.WUTO.ParentWorkingUID = wunit.ParentWorkingUID;
                            item.SubItems.Add(tempWu.getParentWorkingUnit().Name.Trim());
                            item.Tag = wunit.WorkingUnitID;

                            lvWorkingUnits.Items.Add(item);

                        //}
                    }
                }

                lvWorkingUnits.EndUpdate();
                lvWorkingUnits.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.populateWorkingUnitsListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public void chbHierarhicly_CheckedChanged(object o, EventArgs e)
        {
            try
            {
                List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();

                foreach (ListViewItem item in lvWorkingUnits.SelectedItems)
                {
                    foreach (WorkingUnitTO workingUnit in currentWorkingUnitsList)
                    {
                        if (workingUnit.WorkingUnitID == Int32.Parse(item.Tag.ToString()))
                        {
                            wuList.Add(workingUnit);

                        }
                    }
                }

                LoadWorkingUnitsList();
                lvWorkingUnits.Items.Clear();

                if (currentWorkingUnitsList.Count > 0)
                {
                    populateWorkingUnitsListView(currentWorkingUnitsList);
                }

                foreach (WorkingUnitTO selectedWU in wuList)
                {
                    foreach (ListViewItem item in lvWorkingUnits.Items)
                    {
                        if (selectedWU.WorkingUnitID == Int32.Parse(item.Tag.ToString()))
                        {
                            item.Selected = true;
                        }
                    }
                }
                currentWorkingUnit = new WorkingUnitTO();


            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.cbParentWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

        }
        public void cbParentWorkingUnit_SelectedIndexChanged(object o, EventArgs e)
        {
            try
            {
                LoadWorkingUnitsList();
                lvWorkingUnits.Items.Clear();
                if (currentWorkingUnitsList.Count > 0)
                {
                    populateWorkingUnitsListView(currentWorkingUnitsList);
                }

                currentWorkingUnit = new WorkingUnitTO();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.cbParentWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        public void LoadWorkingUnitsList()// Load list of working units
        {
            int parentWUID = -1;

            // Value -1 is assigned to "ALL" in cbParentUnitID
            if (cbParentWorkingUnit.SelectedIndex > 0 && Int32.Parse(cbParentWorkingUnit.SelectedValue.ToString().Trim()) != -1)
            {
                parentWUID = (int)cbParentWorkingUnit.SelectedValue;
            }

            if (this.chbHierarhicly.Checked && !this.cbParentWorkingUnit.Text.ToString().Equals("*"))
            {
                WorkingUnit wu = new WorkingUnit();
                if (parentWUID != -1)
                {
                    wu.WUTO.WorkingUnitID = parentWUID;
                    currentWorkingUnitsList = wu.Search();
                }
                else
                {
                    for (int i = currentWorkingUnitsList.Count - 1; i >= 0; i--)
                    {
                        if (currentWorkingUnitsList[i].WorkingUnitID == currentWorkingUnitsList[i].ParentWorkingUID)
                        {
                            currentWorkingUnitsList.RemoveAt(i);
                        }
                    }
                }

                currentWorkingUnitsList = wu.FindAllChildren(currentWorkingUnitsList);
            }
            else if (!this.chbHierarhicly.Checked && !this.cbParentWorkingUnit.Text.ToString().Equals("*"))
            {
                WorkingUnit workUnit = new WorkingUnit();
                workUnit.WUTO.ParentWorkingUID = parentWUID;
                currentWorkingUnitsList = workUnit.Search();
            }
            else
            {
                currentWorkingUnitsList = wUnits;
            }
        }
        protected virtual void populateEmployeesListView(List<EmployeeTO> employeesList)
        {
            try
            {
                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();
                if (employeesList.Count > 0)
                {
                    for (int i = 0; i < employeesList.Count; i++)
                    {
                        EmployeeTO employee = employeesList[i];
                        ListViewItem item = new ListViewItem();

                        item.Text = employee.LastName.Trim() + " " + employee.FirstName.Trim();
                        item.SubItems.Add(employee.WorkingUnitName.Trim());
                        item.Tag = employee.EmployeeID;
                        lvEmployees.Items.Add(item);
                    }

                    if (chbSelectAll.Checked)
                    {
                        foreach (ListViewItem item in lvEmployees.Items)
                        {
                            item.Selected = true;
                        }
                    }
                }
                lvEmployees.EndUpdate();
                lvEmployees.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.populateEmployeesListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        protected string getSelectedEmployeesString()
        {
            string selectedEmployees = "";
            foreach (ListViewItem item in lvEmployees.SelectedItems)
            {
                selectedEmployees += item.Tag.ToString()+", ";
            }
            selectedEmployees = selectedEmployees.Substring(0, selectedEmployees.Length - 2);
            return selectedEmployees;
        }

        public virtual List<WorkTimeIntervalTO> getTimeSchemaInterval(int employeeID, DateTime date, List<EmployeeTimeScheduleTO> timeScheduleList)
        {
            List<WorkTimeIntervalTO> intervalList = new List<WorkTimeIntervalTO>();
            List<EmployeeTimeScheduleTO> timeScheduleListForEmployee = new List<EmployeeTimeScheduleTO>();
            foreach (EmployeeTimeScheduleTO employeeSchedule in timeScheduleList)
            {
                if (employeeSchedule.EmployeeID == employeeID)
                {
                    timeScheduleListForEmployee.Add(employeeSchedule);
                }
            }
            int timeScheduleIndex = -1;

            for (int scheduleIndex = 0; scheduleIndex < timeScheduleListForEmployee.Count; scheduleIndex++)
            {
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                if (date >= timeScheduleListForEmployee[scheduleIndex].Date)
                //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleListForEmployee[scheduleIndex])).Date.Month))
                {
                    timeScheduleIndex = scheduleIndex;
                }
            }

            if (timeScheduleIndex >= 0)
            {
                int cycleDuration = 0;
                int startDay = timeScheduleListForEmployee[timeScheduleIndex].StartCycleDay;
                int schemaID = timeScheduleListForEmployee[timeScheduleIndex].TimeSchemaID;
                List<WorkTimeSchemaTO> timeSchemaEmployee = new List<WorkTimeSchemaTO>();
                foreach (WorkTimeSchemaTO timeSch in timeSchema)
                {
                    if (timeSch.TimeSchemaID == schemaID)
                    {
                        timeSchemaEmployee.Add(timeSch);
                    }
                }
                WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                if (timeSchemaEmployee.Count > 0)
                {
                    schema = timeSchemaEmployee[0];
                    cycleDuration = schema.CycleDuration;
                }

                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                //TimeSpan days = date - ((EmployeesTimeSchedule) (timeScheduleListForEmployee[timeScheduleIndex])).Date;
                //interval = (TimeSchemaInterval) ((Hashtable)(schema.Days[(startDay + days.Days) % cycleDuration]))[0];
                TimeSpan days = new TimeSpan(date.Date.Ticks - timeScheduleListForEmployee[timeScheduleIndex].Date.Date.Ticks);

                Dictionary<int, WorkTimeIntervalTO> table = schema.Days[(startDay + (int)days.TotalDays) % cycleDuration];
                for (int i = 0; i < table.Count; i++)
                {
                    intervalList.Add(table[i]);
                }
            }

            return intervalList;
        }

        protected virtual void FindIOPairsForSelectedEmployees()
        {
            startIndex = 0;
            try
            {
                DateTime nextDay = new DateTime();
                if (chbShowNextDay.Checked)
                {
                    nextDay = dtpDay.Value.AddDays(1);
                }
                else
                {
                    nextDay = dtpDay.Value;
                }
                List<int> selectedEmployeesListInt = new List<int>();
                selectedEmployeesList = new List<EmployeeTO>();
                this.gbPageNavigation.Visible = false;
                this.btnNext.Visible = false;
                this.btnPrev.Visible = false;

                int isWrkCounter = -1;
                if (rbYes.Checked)
                {
                    isWrkCounter = (int)Constants.IsWrkCount.IsCounter;
                }
                if (rbNo.Checked)
                {
                    isWrkCounter = (int)Constants.IsWrkCount.IsNotCounter;
                }

                foreach (ListViewItem item in lvEmployees.SelectedItems)
                {
                    foreach (EmployeeTO employee in currentEmployeesList)
                    {
                        if (int.Parse(item.Tag.ToString()) == employee.EmployeeID)
                        {
                            selectedEmployeesListInt.Add(employee.EmployeeID);
                            selectedEmployeesList.Add(employee);
                        }
                    }
                }
                IOPair ioPair = new IOPair();

                currentIOPairList = ioPair.SearchEmplDateWithOpenPairs(dtpDay.Value.Date, nextDay, selectedEmployeesListInt, (int)cbLocation.SelectedValue,isWrkCounter);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.btnShow_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        protected virtual void btnShow_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.btnNextDay.Enabled = false;
            this.btnPrevDay.Enabled = false;
            try
            {
                if (lvEmployees.SelectedItems.Count > 0)
                {
                    FindIOPairsForSelectedEmployees();
                    this.DrawGraph(startIndex);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noIOPairsFound", culture));
                    return;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.btnShow_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
                this.btnPrevDay.Enabled = true;
                this.btnNextDay.Enabled = true;
            }
        }

        protected virtual void btnNextDay_Click(object sender, EventArgs e)
        {
            this.dtpDay.Value = dtpDay.Value.AddDays(1);
            this.Cursor = Cursors.WaitCursor;
            this.btnNextDay.Enabled = false;
            this.btnPrevDay.Enabled = false;
            try
            {
                 if (lvEmployees.SelectedItems.Count > 0)
                {
                     FindIOPairsForSelectedEmployees();
                     this.DrawGraph(startIndex);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noIOPairsFound", culture));
                    return;
                }                                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.btnNextDay_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
                this.btnPrevDay.Enabled = true;
                this.btnNextDay.Enabled = true;
            }
        }

        protected virtual void btnPrevDay_Click(object sender, EventArgs e)
        {
            this.dtpDay.Value = dtpDay.Value.AddDays(-1);
            this.Cursor = Cursors.WaitCursor;
            this.btnNextDay.Enabled = false;
            this.btnPrevDay.Enabled = false;
            try
            {
                if (lvEmployees.SelectedItems.Count > 0)
                {
                  FindIOPairsForSelectedEmployees();
                  this.DrawGraph(startIndex);
                }
                else
                {
                    MessageBox.Show(rm.GetString("noIOPairsFound", culture));
                    return;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.btnNextDay_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
                this.btnPrevDay.Enabled = true;
                this.btnNextDay.Enabled = true;
            }
        }

        protected virtual void DrawGraph(int startIndex)
        {
            try
            {
                dailyTimeGrid.Dispose();
                employeeWorkingDayView.Dispose();
                if (selectedEmployeesList.Count > Constants.recordsPerGraph)
                {
                    gbPageNavigation.Visible = true;
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    gbPageNavigation.Visible = false;
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }
                this.lblTotal.Visible = true;
                this.lblEmployee.Visible = true;
                this.gbGraphicReport.Controls.Clear();
                this.lblEmployee.SetBounds(5, 17, 10, 10);
                this.gbGraphicReport.Controls.Add(lblEmployee);
                this.lblTotal.SetBounds((this.gbGraphicReport.Width - 10) / 6 + 2, 17, 10, 10);
                this.gbGraphicReport.Controls.Add(lblTotal);

                string Name = "";

                if (selectedEmployeesList.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < selectedEmployeesList.Count))
                    {
                        if (startIndex == 0)
                        {
                            btnPrev.Enabled = false;
                        }
                        else
                        {
                            btnPrev.Enabled = true;
                        }

                        int lastIndex = startIndex + Constants.recordsPerGraph;
                        if (lastIndex >= selectedEmployeesList.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = selectedEmployeesList.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }


                        dailyTimeGrid = new DailyTimeGrid();
                        dailyTimeGrid.SetBounds(5 + (this.gbGraphicReport.Width - 10) / 4, 15, (this.gbGraphicReport.Width - 10) / 4 * 3, 20);
                        List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();
                        if (chbShowNextDay.Checked)
                        {
                            dailyTimeGrid.MaxValue = 48;
                            timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(this.getSelectedEmployeesString(), dtpDay.Value.Date, dtpDay.Value.Date.AddDays(1));
                        }
                        else
                        {
                            timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(this.getSelectedEmployeesString(), dtpDay.Value.Date, dtpDay.Value.Date);
                        }
                        this.gbGraphicReport.Controls.Add(dailyTimeGrid);
                        
                       
                        string schemaID = "";
                        foreach (EmployeeTimeScheduleTO employeeTimeSchedule in timeScheduleList)
                        {
                            schemaID += employeeTimeSchedule.TimeSchemaID.ToString() + ", ";
                        }
                        if (!schemaID.Equals(""))
                        {
                            schemaID = schemaID.Substring(0, schemaID.Length - 2);
                        }
                                                
                        timeSchema = new TimeSchema().Search(schemaID);

                        int count = startIndex;
                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            List<IOPairTO> ioPairsForSelectedEmployee = new List<IOPairTO>();
                            List<IOPairTO> ioPairsForSelectedEmployeeForNextDay = new List<IOPairTO>();
                            EmployeeTO employee = selectedEmployeesList[i];

                            foreach (IOPairTO iopair in currentIOPairList)
                            {
                                if (iopair.EmployeeID == employee.EmployeeID)
                                {
                                    if (iopair.StartTime.Date.Equals(dtpDay.Value.Date) || iopair.EndTime.Date.Equals(dtpDay.Value.Date))
                                    {
                                        ioPairsForSelectedEmployee.Add(iopair);
                                    }
                                    else
                                    {
                                        ioPairsForSelectedEmployeeForNextDay.Add(iopair);
                                    }
                                }
                            }

                            List<WorkTimeIntervalTO> timeSchemaIntervalList = this.getTimeSchemaInterval(employee.EmployeeID, dtpDay.Value.Date, timeScheduleList);
                            List<WorkTimeIntervalTO> timeSchemaIntervalListForNextDay = this.getTimeSchemaInterval(employee.EmployeeID, dtpDay.Value.Date.AddDays(1), timeScheduleList);
                            Name = employee.LastName + " " + employee.FirstName;
                            employeeWorkingDayView = new EmployeeWorkingDayView(0, 24, 60, ioPairsForSelectedEmployee, Name, employee.WorkingUnitName, passTypes, timeSchemaIntervalList);
                            employeeWorkingDayView.SetBounds(5, 35 + 15 * (i - startIndex), this.gbGraphicReport.Width - 10, 15);
                            this.gbGraphicReport.Controls.Add(employeeWorkingDayView);

                            if (chbShowNextDay.Checked)
                            {
                                employeeWorkingDayView.IOPairListForNextDay = ioPairsForSelectedEmployeeForNextDay;
                                employeeWorkingDayView.ItervalListForNextDay = timeSchemaIntervalListForNextDay;
                            }

                            if ((i - startIndex) % 2 != 0)
                            {
                                employeeWorkingDayView.BackgroundColor = Color.LightGray;
                            }

                            if (i == lastIndex - 1)
                            {
                                employeeWorkingDayView.IsLast = true;
                            }                           
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.DrawGraph(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public void chbSelectAll_CheckedChanged(object o, EventArgs e)
        {
            try
            {
                lvEmployees.BeginUpdate();
                if (chbSelectAll.Checked)
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        item.Selected = true;
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        item.Selected = false;
                    }
                }
                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.chbSelectAll_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        
        private string parse(string forParsing)
        {
            string parsedString = forParsing.Trim();
            if (parsedString.StartsWith("*"))
            {
                parsedString = parsedString.Substring(1);
                parsedString = "%" + parsedString;
            }

            if (parsedString.EndsWith("*"))
            {
                parsedString = parsedString.Substring(0, parsedString.Length - 1);
                parsedString = parsedString + "%";
            }

            return parsedString;
        }

        protected class ListViewItemComparer : IComparer
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
                    case PresenceGraphForDayControl.ParentWUID:
                    case PresenceGraphForDayControl.WUName:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");

                }
            }
        }
        /*
		 *  Class used for sorting Array List of Employees
		*/

        protected class ArrayListSort : IComparer<EmployeeTO>
        {
            private int compOrder;
            private int compField;
            private int compList;
            public ArrayListSort(int sortOrder, int sortField, int sortList)
            {
                compOrder = sortOrder;
                compField = sortField;
                compList = sortList;
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
                if (compList == 0)
                {
                    switch (compField)
                    {
                        case 0:
                            return empl1.LastName.CompareTo(empl2.LastName);
                        case 1:
                            return empl1.WorkingUnitName.CompareTo(empl2.WorkingUnitName);
                        default:
                            return empl1.LastName.CompareTo(empl2.LastName);
                    }
                }
                else
                {
                    switch (compField)
                    {
                        case 0:
                            return empl1.LastName.CompareTo(empl2.LastName);
                        case 1:
                            return empl1.EmployeeID.CompareTo(empl2.EmployeeID);
                        default:
                            return empl1.LastName.CompareTo(empl2.LastName);
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Controller.CloseGraphicReportsClick(true);
        }

        private void btnStatisticsReports_Click(object sender, EventArgs e)
        {
            try
            {
                StatisticGraphicReports statisticGraphicReports = new StatisticGraphicReports();
                statisticGraphicReports.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.btnStatisticsReports_Click): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }       

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        
        private void CaptureScreen()
        {
            Graphics mygraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, mygraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            IntPtr dc1 = mygraphics.GetHdc();
            IntPtr dc2 = memoryGraphics.GetHdc();
            BitBlt(dc2,0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, dc1, 0, 0, 13369376);
            mygraphics.ReleaseHdc(dc1);
            memoryGraphics.ReleaseHdc(dc2);
        }

        private void printDocument1_PrintPage(System.Object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }
        
        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);

            CaptureScreen();
            printDocument1.DefaultPageSettings = pgSettings;
            printDocument1.DefaultPageSettings.Landscape = true;
            PrintDialog dlg = new PrintDialog();
            dlg.Document = printDocument1;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
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
                    this.cbParentWorkingUnit.SelectedIndex = cbParentWorkingUnit.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnLocationTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                LocationsTreeView locationsTreeView = new LocationsTreeView(locArray);
                locationsTreeView.ShowDialog();
                if (!locationsTreeView.selectedLocation.Equals(""))
                {
                    this.cbLocation.SelectedIndex = cbLocation.FindStringExact(locationsTreeView.selectedLocation);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

            
    }
}
