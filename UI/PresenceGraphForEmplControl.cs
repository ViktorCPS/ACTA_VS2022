using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Drawing.Printing;
using System.Data;

using TransferObjects;
using Common;
using Reports;
using Util;

namespace UI
{
    public partial class PresenceGraphForEmplControl : PresenceGraphForDayControl
    {
        private int noOfDaysInMonth;
        DailyTimeGrid dailyTimeGrid = new DailyTimeGrid();
        EmployeeWorkingDayView employeeWorkingDayView = new EmployeeWorkingDayView();

        public PresenceGraphForEmplControl()
        {
            InitializeComponent();
        }

        protected override void InitialiseListView()
        {
            lvWorkingUnits.BeginUpdate();
            lvWorkingUnits.Columns.Add(rm.GetString("lblWorkingUnit", culture), (lvWorkingUnits.Width - 4) / 2, HorizontalAlignment.Left);
            lvWorkingUnits.Columns.Add(rm.GetString("lblParentWUID", culture), (lvWorkingUnits.Width - 4) / 2 - 20, HorizontalAlignment.Left);
            lvWorkingUnits.EndUpdate();
            lvEmployees.BeginUpdate();
            lvEmployees.Columns.Add(rm.GetString("lblEmployee", culture), (lvEmployees.Width - 4) / 2, HorizontalAlignment.Left);
            lvEmployees.Columns.Add(rm.GetString("lblEmployeeID", culture), (lvEmployees.Width - 4) / 2 - 20, HorizontalAlignment.Left);
            lvEmployees.EndUpdate();
        }

        public override List<WorkTimeIntervalTO> getTimeSchemaInterval(int employeeID, DateTime date, List<EmployeeTimeScheduleTO> timeScheduleList)
        {
            List<WorkTimeIntervalTO> intervalList = new List<WorkTimeIntervalTO>();

            int timeScheduleIndex = -1;

            for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
            {
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                if (date >= timeScheduleList[scheduleIndex].Date)
                //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
                {
                    timeScheduleIndex = scheduleIndex;
                }
            }

            if (timeScheduleIndex >= 0)
            {
                int cycleDuration = 0;
                int startDay = timeScheduleList[timeScheduleIndex].StartCycleDay;
                int schemaID = timeScheduleList[timeScheduleIndex].TimeSchemaID;
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
                //TimeSpan days = date - ((EmployeesTimeSchedule) (timeScheduleList[timeScheduleIndex])).Date;
                //interval = (TimeSchemaInterval) ((Hashtable)(schema.Days[(startDay + days.Days) % cycleDuration]))[0];
                TimeSpan days = new TimeSpan(date.Date.Ticks - timeScheduleList[timeScheduleIndex].Date.Date.Ticks);

                Dictionary<int, WorkTimeIntervalTO> table = schema.Days[(startDay + (int)days.TotalDays) % cycleDuration];
                for (int i = 0; i < table.Count; i++)
                {
                    intervalList.Add(table[i]);
                }
            }

            return intervalList;
        }

        protected override void populateEmployeesListView(List<EmployeeTO> employeesList)
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

                        item.Text = employee.EmployeeID.ToString();
                        item.SubItems.Add(employee.LastName.Trim() + " " + employee.FirstName.Trim());
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

        protected override void FindIOPairsForSelectedEmployees()
        {
            startIndex = 0;
            try
            {
                List<DateTime> datesList = new List<DateTime>();
                DateTime dateTime = new DateTime(int.Parse(dtpDay.Value.Year.ToString()), int.Parse(dtpDay.Value.Month.ToString()), 1);
                DateTime nextMonth = dateTime.AddMonths(1);
                this.noOfDaysInMonth = 0;
                int isWrkCounter = -1;
                if (rbYes.Checked)
                {
                    isWrkCounter = (int)Constants.IsWrkCount.IsCounter;
                }
                if (rbNo.Checked)
                {
                    isWrkCounter = (int)Constants.IsWrkCount.IsNotCounter;
                }
                while (dateTime < nextMonth)
                {
                    datesList.Add(dateTime);
                    dateTime = dateTime.AddDays(1);
                    noOfDaysInMonth++;
                }
                string selctedEmployee = "";
                foreach (ListViewItem item in lvEmployees.SelectedItems)
                {
                    selctedEmployee = item.Tag.ToString();
                }

                currentIOPairList = new IOPair().SearchAllPairsForEmpl(selctedEmployee, datesList, (int)cbLocation.SelectedValue, isWrkCounter);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.btnShow_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void SetLanguage()
        {
            this.gbMonthNavigation.Text = rm.GetString("gbMonthNavigation", culture);
            this.gbDay.Text = rm.GetString("gbMonth", culture);
            this.lblDate.Text = rm.GetString("lblDate", culture);
            this.lblIOPairDateChanged.Text = rm.GetString("IOPairDateUpdated", culture);
        }

        protected void PresenceGraphForEmplControl_Load(object sender, System.EventArgs e)
        {
            SetVisibility();
            dtpDay.Value = DateTime.Now;
            this.lvEmployees.MultiSelect = false;
            SetLanguage();
            this.sortField = 1;
            sortList = 1;
        }

        protected override void setSortOrder()
        {
            sortOrder = 0;
            sortField = 1;
            currentEmployeesList.Sort(new ArrayListSort(sortOrder, sortField, sortList));
        }

        private void SetVisibility()
        {
            this.gbDaysNavigation.Visible = false;
            this.gbPageNavigation.Visible = false;
            this.lblEmployee.Visible = false;
            this.lblTotal.Visible = false;
            this.chbShowNextDay.Visible = false;
            this.chbSelectAll.Visible = false;
            this.lblDate.Visible = false;
            this.lblIOPairDateChanged.Visible = false;
        }

        protected override void btnShow_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (lvEmployees.SelectedItems.Count > 0)
                {
                    FindIOPairsForSelectedEmployees();
                    DrawGraph(startIndex);
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
            }
        }

        protected override void DrawGraph(int startIndex)
        {
            try
            {
                employeeWorkingDayView.Dispose();
                dailyTimeGrid.Dispose();
                this.lblIOPairDateChanged.Visible = false;
                this.lblTotal.Visible = true;
                this.lblDate.Visible = true;
                this.gbGraphicReport.Controls.Clear();
                this.lblDate.SetBounds(5, 17, 10, 10);
                this.lblTotal.SetBounds((this.gbGraphicReport.Width - 10) / 6 + 2, 17, 10, 10);
                this.gbGraphicReport.Controls.Add(lblTotal);
                this.gbGraphicReport.Controls.Add(lblDate);
                string date = "";

                if (startIndex >= 0)
                {
                    int lastIndex = noOfDaysInMonth;

                    dailyTimeGrid = new DailyTimeGrid();
                    dailyTimeGrid.SetBounds(5 + (this.gbGraphicReport.Width - 10) / 4, 15, (this.gbGraphicReport.Width - 10) / 4 * 3, 20);
                    this.gbGraphicReport.Controls.Add(dailyTimeGrid);
                    DateTime dateTime = new DateTime(int.Parse(dtpDay.Value.Year.ToString()), int.Parse(dtpDay.Value.Month.ToString()), 1);
                    List<EmployeeTimeScheduleTO> timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(lvEmployees.SelectedItems[0].Tag.ToString(), dateTime, dateTime.AddMonths(1));
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
                    for (int i = startIndex; i < lastIndex; i++)
                    {
                        List<IOPairTO> ioPairsForDay = new List<IOPairTO>();

                        foreach (IOPairTO iopair in currentIOPairList)
                        {
                            if (iopair.StartTime.Date.Equals(dateTime.Date) || iopair.EndTime.Date.Equals(dateTime.Date))
                            {
                                ioPairsForDay.Add(iopair);
                            }
                        }
                        List<WorkTimeIntervalTO> timeSchemaIntervalList = this.getTimeSchemaInterval(int.Parse(lvEmployees.SelectedItems[0].Tag.ToString()), dateTime, timeScheduleList);
                        date = rm.GetString(dateTime.DayOfWeek.ToString().Substring(0, 3), culture) + " " + dateTime.ToString("dd.MM.yyyy");
                        employeeWorkingDayView = new EmployeeWorkingDayView(0, 24, 60, ioPairsForDay, date, "", passTypes, timeSchemaIntervalList);
                        employeeWorkingDayView.SetBounds(5, 35 + 15 * (i - startIndex), this.gbGraphicReport.Width - 10, 15);
                        this.gbGraphicReport.Controls.Add(employeeWorkingDayView);
                        if (date.Substring(0, 3).Equals(rm.GetString(Constants.Saturday, culture)) || date.ToString().Substring(0, 3).Equals(rm.GetString(Constants.Sunday, culture)))
                        {
                            employeeWorkingDayView.displayStringColor = Brushes.Red;
                        }
                        if ((i - startIndex) % 2 != 0)
                        {
                            employeeWorkingDayView.BackgroundColor = Color.LightGray;
                        }
                        if (i == lastIndex - 1)
                        {
                            employeeWorkingDayView.IsLast = true;
                        }
                        dateTime = dateTime.AddDays(1);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.DrawGraph(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public override void InitObserverClient()
        {
            Controller = NotificationController.GetInstance();
            observerClient = new NotificationObserverClient(this.ToString());
            Controller.AttachToNotifier(observerClient);
            this.observerClient.Notification += new NotificationEventHandler(this.IOPairsDateChanged);
        }

        private void IOPairsDateChanged(object sender, NotificationEventArgs e)
        {
            try
            {
                if (e.showIOPairsDateChanged)
                {
                    this.lblIOPairDateChanged.Visible = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForEmplControl.IOPairsDateChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            this.btnNextMonth.Enabled = false;
            this.btnPrevMonth.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.dtpDay.Value = this.dtpDay.Value.AddMonths(1);
                this.btnShow_Click(sender, e);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.btnNextMonth_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.btnNextMonth.Enabled = true;
                this.btnPrevMonth.Enabled = true;
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnPrevMonth_Click(object sender, EventArgs e)
        {
            this.btnNextMonth.Enabled = false;
            this.btnPrevMonth.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.dtpDay.Value = this.dtpDay.Value.AddMonths(-1);
                this.btnShow_Click(sender, e);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.btnPrevMonth_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.btnNextMonth.Enabled = true;
                this.btnPrevMonth.Enabled = true;
                this.Cursor = Cursors.Arrow;
            }
        }

       
    }
}
