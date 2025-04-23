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

using Common;
using Util;
using TransferObjects;

namespace UI
{
    public partial class SecurityRoutesScheduleAdd : Form
    {
        private CultureInfo culture;
        ResourceManager rm;

        // Controller instance
        public NotificationController Controller;

        // Observer client instance
        public NotificationObserverClient observerClient;

        DebugLog log;
        ApplUserTO logInUser;

        const int NameIndex = 0;
        const int FromIndex = 1;
        const int ToIndex = 2;

        private ListViewItemComparer _comp;

        private DateTime selectedDay = new DateTime();
        private DateTime minSelected;
        private DateTime maxSelected;
        private Dictionary<DateTime, DayOfCalendar> calendarDays;
        private List<DateTime> selectedDays;

        private bool shiftStatus = false;

        private int emplID;
        private DateTime date;

        private string routeType;

        public SecurityRoutesScheduleAdd(int emplID, DateTime date, string routeType)
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();
                InitObserverClient();

                calendarDays = new Dictionary<DateTime,DayOfCalendar>();
                selectedDays = new List<DateTime>();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(SecurityRoutesScheduleAdd).Assembly);

                setLanguage();

                logInUser = NotificationController.GetLogInUser();

                this.dtpMonth.Value = new DateTime(date.Year, date.Month, 1);
                this.emplID = emplID;
                this.date = date;

                this.routeType = routeType;
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
                    case SecurityRoutesScheduleAdd.NameIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    case SecurityRoutesScheduleAdd.FromIndex:
                    case SecurityRoutesScheduleAdd.ToIndex:
                        {
                            DateTime dt1 = new DateTime(1, 1, 1, 0, 0, 0);
                            DateTime dt2 = new DateTime(1, 1, 1, 0, 0, 0);

                            if (!sub1.Text.Trim().Equals(""))
                            {
                                dt1 = DateTime.ParseExact(sub1.Text.Trim(), "HH:mm", null);
                            }

                            if (!sub2.Text.Trim().Equals(""))
                            {
                                dt2 = DateTime.ParseExact(sub2.Text.Trim(), "HH:mm", null);
                            }

                            return CaseInsensitiveComparer.Default.Compare(dt1, dt2);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");

                }
            }
        }

        #endregion

        private void InitObserverClient()
        {
            Controller = NotificationController.GetInstance();
            observerClient = new NotificationObserverClient(this.ToString());
            Controller.AttachToNotifier(observerClient);
            this.observerClient.Notification += new NotificationEventHandler(this.DaySelected);
        }

        private void setLanguage()
        {
            try
            {
                // form text
                this.Text = rm.GetString("SecurityRoutesScheduleAddForm", culture);

                // group box's text
                this.gbAssignRoutes.Text = rm.GetString("gbAssignRoutes", culture);
                this.gbEmployee.Text = rm.GetString("gbEmployee", culture);
                this.gbRoutes.Text = rm.GetString("gbRoutes", culture);
                
                // label's text
                this.lblRouteName.Text = rm.GetString("lblRoute", culture);
                this.lblWU.Text = rm.GetString("lblWU", culture);
                this.lblEmpl.Text = rm.GetString("lblEmployee", culture);
                this.lblMonth.Text = rm.GetString("lblMonth", culture);
                this.tbMon.Text = rm.GetString("Mon", culture);
                this.tbTue.Text = rm.GetString("Tue", culture);
                this.tbWed.Text = rm.GetString("Wed", culture);
                this.tbThr.Text = rm.GetString("Thr", culture);
                this.tbFri.Text = rm.GetString("Fri", culture);
                this.tbSat.Text = rm.GetString("Sat", culture);
                this.tbSun.Text = rm.GetString("Sun", culture);

                // button's text
                this.btnAssign.Text = rm.GetString("btnAssign", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);
                this.btnCancel.Text = rm.GetString("btnCancel", culture);
                this.btnClearSelDays.Text = rm.GetString("btnClearSelectedDays", culture);
                this.btnClear.Text = rm.GetString("btnClear", culture);

                // list views
                lvDetails.BeginUpdate();
                lvDetails.Columns.Add(rm.GetString("hdrGate", culture), (lvDetails.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvDetails.Columns.Add(rm.GetString("hdrFrom", culture), (lvDetails.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvDetails.Columns.Add(rm.GetString("hdrTo", culture), (lvDetails.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvDetails.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void SecurityRoutesScheduleAdd_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Initialize comparer object
                _comp = new ListViewItemComparer(lvDetails);
                lvDetails.ListViewItemSorter = _comp;
                _comp.SortColumn = FromIndex;
                lvDetails.Sorting = SortOrder.Ascending;

                populateRoutes();

                if (routeType.Equals(Constants.routeTag))
                {
                    populateWorkingUnits();
                    populateEmployeeCombo(-1);
                }
                else
                {
                    lblWU.Visible = cbWU.Visible = btnWUTree.Visible = false;
                    populateRouteEmployeeCombo();
                }

                setCalendar(this.dtpMonth.Value);

                if (emplID >= 0)
                {
                    cbEmpl.SelectedValue = emplID;

                    if (cbEmpl.SelectedIndex < 0)
                    {
                        cbEmpl.SelectedIndex = 0;
                    }

                    cbWU.Enabled = cbEmpl.Enabled = btnWUTree.Enabled = false;

                    populateRouteSchedule(emplID, dtpMonth.Value);

                    foreach (DateTime day in calendarDays.Keys)
                    {
                        if (calendarDays[day].Date.Date.Equals(date.Date))
                        {
                            calendarDays[day].SelectDay();
                            selectedDays.Add(calendarDays[day].Date.Date);
                            break;
                        }
                    }
                }

                populateRouteSchedule(emplID, dtpMonth.Value);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.SecurityRoutesScheduleAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvDetails_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvDetails.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvDetails.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvDetails.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvDetails.Sorting = SortOrder.Ascending;
                }

                lvDetails.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.lvDetails_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateRoutes()
        {
            try
            {
                ArrayList routes = new SecurityRouteHdr().Search("", "");
                SecurityRouteHdr route = new SecurityRouteHdr();
                route.Name = rm.GetString("all", culture);
                routes.Insert(0, route);

                cbRouteName.DataSource = routes;
                cbRouteName.DisplayMember = "Name";
                cbRouteName.ValueMember = "SecurityRouteID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.SecurityRoutes_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbRouteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbRouteName.SelectedIndex <= 0)
                {
                    lvDetails.Items.Clear();
                }
                else
                {
                    ArrayList routesDtl = new ArrayList();
                    if (routeType.Equals(Constants.routeTag))
                    {
                        routesDtl = new SecurityRouteHdr().SearchDetailsTag((int)cbRouteName.SelectedValue);
                    }
                    else
                    {
                        routesDtl = new SecurityRouteHdr().SearchDetailsTerminal((int)cbRouteName.SelectedValue);
                    }

                    populateListView(routesDtl);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.cbRouteName_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateWorkingUnits()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new WorkingUnit().Search();

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWU.DataSource = wuArray;
                cbWU.DisplayMember = "Name";
                cbWU.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.populateWorkingUnits(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateEmployeeCombo(int wuID)
        {
            try
            {
                List<EmployeeTO> emplArray = new List<EmployeeTO>();

                Employee empl = new Employee();
                if (wuID == -1)
                {
                    emplArray = empl.Search();
                }
                else
                {
                    empl.EmplTO.WorkingUnitID = wuID;
                    emplArray = empl.Search();
                }

                foreach (EmployeeTO employee in emplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO emplTO = new EmployeeTO();
                emplTO.LastName = rm.GetString("all", culture);
                emplArray.Insert(0, emplTO);

                cbEmpl.DataSource = emplArray;
                cbEmpl.DisplayMember = "LastName";
                cbEmpl.ValueMember = "EmployeeID";
                cbEmpl.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateRouteEmployeeCombo()
        {
            try
            {
                ArrayList routeEmplArray = new ArrayList();

                routeEmplArray = new SecurityRoutesEmployee().SearchByWU("");

                SecurityRoutesEmployee routeEmpl = new SecurityRoutesEmployee();
                routeEmpl.EmployeeName = rm.GetString("all", culture);
                routeEmplArray.Insert(0, routeEmpl);

                cbEmpl.DataSource = routeEmplArray;
                cbEmpl.DisplayMember = "EmployeeName";
                cbEmpl.ValueMember = "EmployeeID";
                cbEmpl.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.populateRouteEmployeeCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateListView(ArrayList routesDtl)
        {
            try
            {
                lvDetails.BeginUpdate();
                lvDetails.Items.Clear();

                if (routesDtl.Count > 0)
                {
                    foreach (SecurityRouteDtl dtl in routesDtl)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = dtl.GateName.Trim();
                        item.SubItems.Add(dtl.TimeFrom.ToString("HH:mm"));
                        item.SubItems.Add(dtl.TimeTo.ToString("HH:mm"));
                        item.ToolTipText = "Route ID: " + dtl.SecurityRouteID.ToString().Trim() + "\nSegment ID: " + dtl.SegmentID.ToString().Trim();

                        lvDetails.Items.Add(item);
                    }
                }

                lvDetails.EndUpdate();
                lvDetails.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.populateListView(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnWUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits("");
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbWU.SelectedIndex = cbWU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbWU.SelectedValue is int)
                {
                    this.Cursor = Cursors.WaitCursor;
                    populateEmployeeCombo((int)cbWU.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setCalendar(DateTime month)
        {
            try
            {
                int currentMonth = month.Month;
                int width = this.tbMon.Width - Constants.calendarHSpace;
                int height = this.tbMon.Height * 2;
                int startXPos = Constants.calendarLeft;
                int startYPos = this.tbMon.Height + Constants.calendarTop;
                int xPos;
                int yPos;
                int row = 0;

                DateTime date = new DateTime(month.Year, month.Month, 1);

                // set to default values
                this.panel1.Controls.Clear();
                this.calendarDays.Clear();
                this.selectedDays.Clear();
                this.selectedDay = new DateTime();
                this.minSelected = date.AddMonths(2);
                this.maxSelected = date.AddMonths(-1);

                addCalendarHeader();

                while (date.Month == currentMonth)
                {
                    xPos = startXPos + getDayOfWeek(date) * (width + Constants.calendarHSpace);
                    yPos = startYPos + row * (height + Constants.calendarVSpace);

                    DayOfCalendar dayOfCalendar = new DayOfCalendar(xPos, yPos, "", "", date.Day.ToString());
                    dayOfCalendar.Date = new DateTime(date.Year, date.Month, date.Day);
                    dayOfCalendar.Schema = new WorkTimeSchemaTO();
                    dayOfCalendar.StartDay = -1;

                    this.panel1.Controls.Add(dayOfCalendar);
                    this.calendarDays.Add(date.Date, dayOfCalendar);

                    // if next day is Monday, drow in next row
                    if (date.DayOfWeek.Equals(DayOfWeek.Sunday))
                    {
                        row++;
                    }

                    date = date.AddDays(1.0);
                }

                setNotWorkingDays(dtpMonth.Value);

                this.panel1.Invalidate();
                this.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.setCalendar(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Returns day of week.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private int getDayOfWeek(DateTime date)
        {
            try
            {
                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return 0;
                    //break;
                    case DayOfWeek.Tuesday:
                        return 1;
                    //break;
                    case DayOfWeek.Wednesday:
                        return 2;
                    //break;
                    case DayOfWeek.Thursday:
                        return 3;
                    //break;
                    case DayOfWeek.Friday:
                        return 4;
                    //break;
                    case DayOfWeek.Saturday:
                        return 5;
                    //break;
                    case DayOfWeek.Sunday:
                        return 6;
                    //break;
                    default:
                        return 0;
                    //break;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.getDayOfWeek(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Add header to the calendar.
        /// </summary>
        private void addCalendarHeader()
        {
            this.panel1.Controls.Add(tbMon);
            this.panel1.Controls.Add(tbTue);
            this.panel1.Controls.Add(tbWed);
            this.panel1.Controls.Add(tbThr);
            this.panel1.Controls.Add(tbFri);
            this.panel1.Controls.Add(tbSat);
            this.panel1.Controls.Add(tbSun);
        }

        /// <summary>
        /// When calendar value is changed, calendar for selected month is drawn.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpMonth_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                setCalendar(this.dtpMonth.Value);
                if (cbEmpl.SelectedValue is int)
                {
                    populateRouteSchedule((int)cbEmpl.SelectedValue, dtpMonth.Value);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.dtpMonth_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        public void DaySelected(object sender, NotificationEventArgs e)
        {
            try
            {
                if (!e.daySelected.Equals(new DateTime()))
                {
                    selectedDays.Clear();
                    if (!shiftStatus)
                    {
                        selectedDay = new DateTime();

                        // selected month is shown and days of next month that belongs to last week of selected month
                        // max and min selected date should have initial values that are less of first day of selected month, and greater then end of last week of selected month
                        minSelected = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1, 0, 0, 0).AddMonths(2);
                        maxSelected = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1, 0, 0, 0).AddMonths(-1);
                        UnselectDays(e.daySelected);
                    }

                    selectedDay = e.daySelected;
                    if (selectedDay.Date < minSelected.Date)
                    {
                        minSelected = selectedDay.Date;
                    }
                    if (selectedDay.Date > maxSelected.Date)
                    {
                        maxSelected = selectedDay.Date;
                    }

                    if (this.shiftStatus)
                    {
                        DateTime currDate = minSelected.Date;
                        while (currDate.Date <= maxSelected.Date)
                        {
                            selectedDays.Add(currDate.Date);
                            if (calendarDays.ContainsKey(currDate.Date))
                                calendarDays[currDate.Date].SelectDay();
                            currDate = currDate.AddDays(1);
                        }
                    }
                    else
                    {
                        selectedDays.Add(selectedDay);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.DaySelected(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public void UnselectDays(DateTime selectedDay)
        {
            try
            {
                foreach (DateTime day in this.calendarDays.Keys)
                {
                    if (!selectedDays.Contains(day.Date) && !day.Date.Equals(selectedDay))
                    {
                        calendarDays[day].UnselectDay();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.UnselectDays(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void SecurityRoutesScheduleAdd_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // if Shift is pressed
            if (e.KeyValue == 16)
            {
                this.shiftStatus = true;
            }
        }

        private void SecurityRoutesScheduleAdd_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // if Shift is released
                if (e.KeyValue == 16)
                {
                    this.shiftStatus = false;
                }

                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.SecurityRoutesScheduleAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setNotWorkingDays(DateTime month)
        {
            try
            {
                //foreach (DayOfCalendar day in calendarDays)
                //{
                //    HolidayTO hTO = new Holiday().Find(day.Date);

                //    if ((day.Date.DayOfWeek.Equals(DayOfWeek.Saturday)) ||
                //        (day.Date.DayOfWeek.Equals(DayOfWeek.Sunday)) ||
                //        (!hTO.HolidayDate.Equals(new DateTime())))
                //    {
                //        day.SetNotWorkingDay();
                //    }
                //}

                DateTime startDate = new DateTime(month.Year, month.Month, 1);
                DateTime endDate = new DateTime(month.Year, month.Month, 1).AddMonths(1).AddDays(-1);

                List<HolidayTO> holidays = new Holiday().Search(startDate.Date, endDate.Date);
                List<HolidaysExtendedTO> holidaysExt = new HolidaysExtended().Search(startDate.Date, endDate.Date);
                List<DateTime> holDates = new List<DateTime>();

                foreach (HolidayTO hol in holidays)
                {
                    if (!holDates.Contains(hol.HolidayDate.Date))
                        holDates.Add(hol.HolidayDate.Date);
                }

                foreach (HolidaysExtendedTO hol in holidaysExt)
                {
                    DateTime holDate = hol.DateStart.Date;
                    while (holDate.Date <= hol.DateEnd.Date)
                    {
                        if (!holDates.Contains(holDate.Date))
                            holDates.Add(holDate.Date);

                        holDate = holDate.AddDays(1);
                    }
                }

                foreach (DateTime day in calendarDays.Keys)
                {
                    if (calendarDays[day].Date.Month != dtpMonth.Value.Month)
                        calendarDays[day].SetOtherMonthDay();

                    if ((calendarDays[day].Date.DayOfWeek.Equals(DayOfWeek.Saturday)) ||
                        (calendarDays[day].Date.DayOfWeek.Equals(DayOfWeek.Sunday)) ||
                        (holDates.Contains(calendarDays[day].Date.Date)))
                    {
                        if (calendarDays[day].Date.Month != dtpMonth.Value.Month)
                            calendarDays[day].SetNotWorkingDayOtherMonth();
                        else
                            calendarDays[day].SetNotWorkingDay();
                    }
                    //if (Common.Misc.isLockedDate(calendarDays[day].Date))
                    //{
                    //    if (calendarDays[day].Date.Month != dtpMonth.Value.Month)
                    //        calendarDays[day].SetLockedDayOtherMonth();
                    //    else
                    //        calendarDays[day].SetLockedDay();
                    //}
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.setNotWorkingDays(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnClear_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                foreach (DateTime day in calendarDays.Keys)
                {
                    calendarDays[day].Schema = new WorkTimeSchemaTO();
                    calendarDays[day].Routes = "";
                    calendarDays[day].StartDay = -1;
                    calendarDays[day].setSchemaText("");
                    calendarDays[day].setCycleDayText("");
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.btnClear_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void SecurityRoutesScheduleAdd_Closed(object sender, System.EventArgs e)
        {
            Controller.DettachFromNotifier(this.observerClient);
        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbEmpl.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("selEmpl", culture));
                }
                else if (cbRouteName.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("selectRoute", culture));
                }
                else
                {
                    int routeID = (int)cbRouteName.SelectedValue;
                    if (selectedDays.Count == 0)
                    {
                        MessageBox.Show(rm.GetString("selectOneDayCalendarRouteSch", culture));
                    }
                    else
                    {
                        foreach(DateTime selDate in selectedDays)
                        {
                            if (calendarDays.ContainsKey(selDate.Date))
                            {
                                string[] routes = calendarDays[selDate.Date].Routes.Split(',');
                                bool containsRoute = false;
                                foreach (string r in routes)
                                {
                                    if (r.Trim().Equals(cbRouteName.SelectedValue.ToString().Trim()))
                                    {
                                        containsRoute = true;
                                        break;
                                    }
                                }

                                if (!containsRoute)
                                {
                                    calendarDays[selDate.Date].Routes +=
                                        (calendarDays[selDate.Date].Routes.Length == 0) ? cbRouteName.SelectedValue.ToString().Trim() : "," + cbRouteName.SelectedValue.ToString().Trim();
                                    calendarDays[selDate.Date].setSchemaText(rm.GetString("Route", culture) + ": " + calendarDays[selDate.Date].Routes);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.btnAssign_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClearSelDays_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (selectedDays.Count > 0)
                {
                    foreach (DateTime day in selectedDays)
                    {
                        if (calendarDays.ContainsKey(day.Date))
                        {
                            calendarDays[day.Date].Schema = new WorkTimeSchemaTO();
                            calendarDays[day.Date].Routes = "";
                            calendarDays[day.Date].StartDay = -1;
                            calendarDays[day.Date].setSchemaText("");
                            calendarDays[day.Date].setCycleDayText("");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.btnClearSelDays_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SecurityRouteSchedule routeSchedule = new SecurityRouteSchedule();
            
            try
            {
                if (cbEmpl.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("selectEmplRouteSch", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                bool trans = routeSchedule.BeginTransaction();
                bool isSaved = true;

                if (trans)
                {
                    foreach (DateTime calDate in calendarDays.Keys)
                    {
                        isSaved = routeSchedule.Delete((int)cbEmpl.SelectedValue, calDate.Date, false) && isSaved;

                        if (calendarDays[calDate.Date].Routes.Length > 0)
                        {
                            string[] routes = calendarDays[calDate.Date].Routes.Split(',');
                            foreach (string route in routes)
                            {
                                isSaved = (routeSchedule.Save((int)cbEmpl.SelectedValue, Int32.Parse(route), calDate.Date, false) > 0 ? true : false) && isSaved;

                                if (!isSaved)
                                {
                                    break;
                                }
                            }
                        }
                        if (!isSaved)
                        {
                            break;
                        }
                    }

                    if (isSaved)
                    {
                        routeSchedule.CommitTransaction();
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("routeScheduleSaved", culture));
                        foreach (DateTime day in this.calendarDays.Keys)
                        {
                            calendarDays[day].UnselectDay();
                        }
                        selectedDays = new List<DateTime>();
                    }
                    else
                    {
                        routeSchedule.RollbackTransaction();
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("routeScheduleNotSaved", culture));
                    }
                } // if(trans)
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                if (routeSchedule.GetTransaction() != null)
                {
                    routeSchedule.RollbackTransaction();
                }
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateRouteSchedule(int emplID, DateTime month)
        {
            try
            {
                btnClear.PerformClick();

                if (emplID >= 0)
                {
                    DateTime firstDay = new DateTime(month.Year, month.Month, 1);
                    DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
                    ArrayList routeSchedule = new SecurityRouteSchedule().Search(emplID, -1, firstDay, lastDay);

                    Hashtable routes = new Hashtable();
                    string routesID = "";
                    foreach (SecurityRouteSchedule sch in routeSchedule)
                    {
                        if (!routes.ContainsKey(sch.Date.Date))
                        {
                            routes.Add(sch.Date.Date, "");
                        }

                        if (routes[sch.Date.Date].ToString().Length == 0)
                        {
                            routesID = routes[sch.Date.Date].ToString();
                            routesID += sch.SecurityRouteID.ToString().Trim();
                            routes[sch.Date.Date] = routesID;
                        }
                        else
                        {
                            routesID = routes[sch.Date.Date].ToString();
                            routesID += "," + sch.SecurityRouteID.ToString().Trim();
                            routes[sch.Date.Date] = routesID;
                        }
                    }

                    foreach (DateTime calDate in calendarDays.Keys)
                    {
                        if (routes.ContainsKey(calendarDays[calDate].Date.Date))
                        {
                            calendarDays[calDate].Routes = routes[calendarDays[calDate].Date.Date].ToString();
                            calendarDays[calDate].setSchemaText(rm.GetString("Route", culture) + ": " + routes[calendarDays[calDate].Date.Date].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.populateRouteSchedule(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbEmpl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbEmpl.SelectedValue is int)
                {
                    populateRouteSchedule((int)cbEmpl.SelectedValue, dtpMonth.Value);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesScheduleAdd.cbEmpl_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}