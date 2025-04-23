using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using TransferObjects;
using Common;
using Util;

namespace UI
{
    public partial class StatisticReportViewSingleWUControl : StatisticReportViewControl
    {
        //calculate working time and absence
        //in minutes and percent of planed working time and realize working time
        //draw pie based on calculate values 
        //write all values and percentes into the table
        private Dictionary<int, List<EmployeeTimeScheduleTO>> _emplTimeSchedules;
        private List<EmployeeTO> _employees;
        private Dictionary<int, List<IOPairTO>> _ioPairsList;
        Color[] colors;
        protected int PhysicalAttendanceMin;
        protected int WholeDayAbsenceMin;
        protected int ApprovedAbsenceDuringWorkingTimeMin;
        protected int UnapprovedAbsenceDuringWorkingTimeMin;
        protected int ExtraHoursMin;

        public List<EmployeeTO> Employees
        {
            get { return _employees; }
            set { _employees = value; }
        }

        //key is employeeID memebers is ArrayList of timeShedules
        public Dictionary<int, List<EmployeeTimeScheduleTO>> EmplTimeSchedules
        {
            get { return _emplTimeSchedules; }
            set { _emplTimeSchedules = value; }
        }

        //key is employeeID memebers is ArrayList of iopairs
        public Dictionary<int, List<IOPairTO>> IOPairsList
        {
            get { return _ioPairsList; }
            set { _ioPairsList = value; }
        }

        protected override void setBounds()
        {
            try
            {
                base.setBounds();
                lvStatistics.Clear();
                PopulateListView();
                if (this.PlannedWorkingTimeMin != 0)
                {
                    setListViewValues();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public StatisticReportViewSingleWUControl()
        {
            InitializeComponent();
            PhysicalAttendanceMin = 0;
            WholeDayAbsenceMin = 0;
            ApprovedAbsenceDuringWorkingTimeMin = 0;
            UnapprovedAbsenceDuringWorkingTimeMin = 0;
            this.CalculatePlanedAndRealizedWorkingTime = true;

        }
        public StatisticReportViewSingleWUControl(Dictionary<int, List<IOPairTO>> ioPairs, Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedules, List<WorkTimeSchemaTO> timeSchemaList, List<EmployeeTO> employees, List<HolidayTO> holydays, Dictionary<int, PassTypeTO> passTypes)
        {
            InitializeComponent();
            PhysicalAttendanceMin = 0;
            WholeDayAbsenceMin = 0;
            ApprovedAbsenceDuringWorkingTimeMin = 0;
            UnapprovedAbsenceDuringWorkingTimeMin = 0;
            ExtraHoursMin = 0;
            this.IOPairsList = ioPairs;
            this.TimeSchemaList = timeSchemaList;
            this.Employees = employees;
            this.EmplTimeSchedules = emplTimeSchedules;
            this.Holidays = holydays;
            this.PassTypes = passTypes;
            this.CalculatePlanedAndRealizedWorkingTime = true;
            colors = new Color[6];
            colors[0] = Color.FromArgb(80, Color.Green);
            colors[1] = Color.FromArgb(80, Color.Purple);
            colors[2] = Color.FromArgb(80, Color.Yellow);
            colors[3] = Color.FromArgb(80, Color.Pink);
            colors[4] = Color.FromArgb(80, Color.Blue);
            colors[5] = Color.FromArgb(80, Color.Red);
        }

        private void StatisticReportViewSingleWUControl_Load(object sender, EventArgs e)
        {
            try
            {
                // first calculate values for pie 
                CalculateValues();

                //pass values to the control for pie drawing
                SetPieChartValues();
                //fill list with calculated values
                setListViewValues();
                //fill text boxes with values 
                ShowPlannedWorkingTime();
                ShowRealizedWorkingTime();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void CalculateValues()
        {
            try
            {
                List<WorkTimeIntervalTO> intervalsEmpl = new List<WorkTimeIntervalTO>();
                //calculate holes in working time for days with IOPairs for all employees and days
                this.UnapprovedAbsenceDuringWorkingTimeMin += calculateHolesDuringWorkingTime(this.IOPairsList, this.EmplTimeSchedules, this.Employees);
                foreach (EmployeeTO employee in Employees)
                {
                    List<IOPairTO> ioPairsEmpl = new List<IOPairTO>();
                    List<EmployeeTimeScheduleTO> timeSchedulesForEmployee = new List<EmployeeTimeScheduleTO>();
                    //if hashtable conteins time shedules for specific employee fill array list with it
                    if (this.EmplTimeSchedules.ContainsKey(employee.EmployeeID))
                    {
                        timeSchedulesForEmployee = EmplTimeSchedules[employee.EmployeeID];//time chedules for current employee
                    }
                    //if hashtable conteins ioPairs for specific employee fill array list with it
                    if (this.IOPairsList.ContainsKey(employee.EmployeeID))
                    {
                        ioPairsEmpl = IOPairsList[employee.EmployeeID];
                        //calculate values for which doesn't need time schemas
                        CalucuateIOPairsTimeByPassTypes(ioPairsEmpl);
                    }

                    for (DateTime date = DateFrom; date <= DateTo; date = date.AddDays(1))
                    {
                        if (ioPairsEmpl.Count > 0)
                        {
                            //For each employee and date if there is IOPairs calculate extra hours and planed working time
                            List<IOPairTO> ioPairsEmplDate = this.getIOPairsForEmplDate(ioPairsEmpl, date);//geting ioPairs for specified employee and date
                            this.ExtraHoursMin += calculatePlanedWorkingTimeAndExtraHours(date, timeSchedulesForEmployee, ioPairsEmplDate, true);//calculate extra hours for specified employee and date
                        }
                        else//if there is no IOPairs all intervals are holes in working time
                        {
                            if (timeSchedulesForEmployee.Count > 0)
                            {
                                intervalsEmpl = this.getTimeSchemaInterval(employee.EmployeeID, date, timeSchedulesForEmployee);//geting time intervals list for specified employee and date

                                foreach (WorkTimeIntervalTO tsInterval in intervalsEmpl)
                                {
                                    TimeSpan duration = (new DateTime(tsInterval.EndTime.TimeOfDay.Ticks - tsInterval.StartTime.TimeOfDay.Ticks)).TimeOfDay;
                                    this.PlannedWorkingTimeMin += (int)duration.TotalMinutes;
                                    if (ioPairsEmpl.Count == 0)
                                    {
                                        //calculate holes in working time for days without IOPairs 
                                        this.UnapprovedAbsenceDuringWorkingTimeMin += (int)duration.TotalMinutes;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SetPieChartValues()//seting values for pie chart graph
        {
            try
            {
                //count total pie chart value
                int total = this.PhysicalAttendanceMin + this.UnapprovedAbsenceDuringWorkingTimeMin + this.ApprovedAbsenceDuringWorkingTimeMin + this.WholeDayAbsenceMin + this.ExtraHoursMin;
                //set list of values for pie chart, order is very important
                decimal[] ValuseForPieChart = {this.PhysicalAttendanceMin, this.WholeDayAbsenceMin,this.ApprovedAbsenceDuringWorkingTimeMin,
                this.UnapprovedAbsenceDuringWorkingTimeMin, this.ExtraHoursMin};
                this.pccStatisticsView.Values = ValuseForPieChart;

                this.pccStatisticsView.Colors = colors;
                string[] toolTips = { rm.GetString("lvPhysicalAttendance", culture), rm.GetString("chbWholeDayAbsence", culture)
                    , rm.GetString("ttApprovedAbsence", culture), rm.GetString("ttUnapprovedAbsence", culture), rm.GetString("chbExtraHours", culture)};
                //set slice string's to default value
                string[] PieSliceTexts = { "", "", "", "", "" };
                if (total > 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        //if slice is larger than 5% take value from toolTip and set it to slice string
                        if ((ValuseForPieChart[i] * 100 / total) > 5)
                        {
                            if (toolTips[i].Length > (pccStatisticsView.Width - 20) / 14)
                            {
                                PieSliceTexts[i] = toolTips[i].Substring(0, (pccStatisticsView.Width - 20) / 14);
                            }
                            else
                                PieSliceTexts[i] = toolTips[i];
                        }
                    }
                }
                this.pccStatisticsView.Texts = PieSliceTexts;
                this.pccStatisticsView.ToolTips = toolTips;
                float[] displ = { (float)0.05, (float)0.05, (float)0.05, (float)0.05, (float)0.05 };
                this.pccStatisticsView.SliceRelativeDisplacements = displ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void PopulateListView()
        {
            try
            {
                lvStatistics.BeginUpdate();
                lvStatistics.Items.Add(rm.GetString("lvPhysicalAttendance", culture));
                lvStatistics.Items.Add(rm.GetString("chbWholeDayAbsence", culture));
                lvStatistics.Items.Add(rm.GetString("chbAbsenceDuringWorkingTime", culture));
                lvStatistics.Items.Add(rm.GetString("lvApprovedAbsence", culture));
                lvStatistics.Items.Add(rm.GetString("lvUnapprovedAbsence", culture));
                lvStatistics.Items.Add(rm.GetString("chbExtraHours", culture));
                foreach (ListViewItem item in lvStatistics.Items)
                {
                    item.UseItemStyleForSubItems = false;
                    item.SubItems.Add("");
                    if (item.Index < 2)
                        item.SubItems[1].BackColor = colors[item.Index];
                    if (item.Index > 2)
                        item.SubItems[1].BackColor = colors[item.Index - 1];
                }
                lvStatistics.EndUpdate();
                lvStatistics.Invalidate();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void setListViewValues()
        {
            try
            {
                this.lvStatistics.BeginUpdate();

                string stringPysicalAttendanceRealized = "";
                string stringPysicalAttendancePlaned = "";
                string stringWholeDayAbsenceRealized = "";
                string stringWholeDayAbsencePlaned = "";
                string stringApprovedAbsenceRealized = "";
                string stringApprovedAbsencePlaned = "";
                string stringAbsenceRealized = "";
                string stringAbsencePlaned = "";
                string stringUnapprovedAbsenceRealized = "";
                string stringUnapprovedAbsencePlaned = "";
                string stringExtraHoursRealized = "";
                string stringExtraHoursPlaned = "";
                decimal hours = 0;
                //in order to avoid dividing by zero check if planed or realized time are zero before dividing
                if (this.PlannedWorkingTimeMin != 0)
                {
                    stringPysicalAttendancePlaned = this.PhysicalAttendanceMin * 100 / this.PlannedWorkingTimeMin + "%";
                    stringWholeDayAbsencePlaned = this.WholeDayAbsenceMin * 100 / this.PlannedWorkingTimeMin + "%";
                    stringAbsencePlaned = (this.ApprovedAbsenceDuringWorkingTimeMin + this.UnapprovedAbsenceDuringWorkingTimeMin) * 100 / this.PlannedWorkingTimeMin + "%";
                    stringApprovedAbsencePlaned = this.ApprovedAbsenceDuringWorkingTimeMin * 100 / this.PlannedWorkingTimeMin + "%";
                    stringUnapprovedAbsencePlaned = this.UnapprovedAbsenceDuringWorkingTimeMin * 100 / this.PlannedWorkingTimeMin + "%";
                    stringExtraHoursPlaned = this.ExtraHoursMin * 100 / this.PlannedWorkingTimeMin + "%";
                }
                if (this.RealizeWorkingTimeMin != 0)
                {
                    stringPysicalAttendanceRealized = this.PhysicalAttendanceMin * 100 / this.RealizeWorkingTimeMin + "%";
                    stringWholeDayAbsenceRealized = this.WholeDayAbsenceMin * 100 / this.RealizeWorkingTimeMin + "%";
                    stringAbsenceRealized = (this.ApprovedAbsenceDuringWorkingTimeMin + this.UnapprovedAbsenceDuringWorkingTimeMin) * 100 / this.RealizeWorkingTimeMin + "%";
                    stringApprovedAbsenceRealized = this.ApprovedAbsenceDuringWorkingTimeMin * 100 / this.RealizeWorkingTimeMin + "%";
                    stringUnapprovedAbsenceRealized = this.UnapprovedAbsenceDuringWorkingTimeMin * 100 / this.RealizeWorkingTimeMin + "%";
                    stringExtraHoursRealized = this.ExtraHoursMin * 100 / this.RealizeWorkingTimeMin + "%";
                }
                hours = (decimal)this.PhysicalAttendanceMin / 60;

                this.lvStatistics.Items[0].SubItems.Add(hours.ToString("F1"));
                this.lvStatistics.Items[0].SubItems.Add(stringPysicalAttendanceRealized);
                this.lvStatistics.Items[0].SubItems.Add(stringPysicalAttendancePlaned);
                hours = (decimal)this.WholeDayAbsenceMin / 60;
                this.lvStatistics.Items[1].SubItems.Add(hours.ToString("F1"));
                this.lvStatistics.Items[1].SubItems.Add(stringWholeDayAbsenceRealized);
                this.lvStatistics.Items[1].SubItems.Add(stringWholeDayAbsencePlaned);
                hours = (decimal)(this.ApprovedAbsenceDuringWorkingTimeMin + this.UnapprovedAbsenceDuringWorkingTimeMin) / 60;
                this.lvStatistics.Items[2].SubItems.Add(hours.ToString("F1"));
                this.lvStatistics.Items[2].SubItems.Add(stringAbsenceRealized);
                this.lvStatistics.Items[2].SubItems.Add(stringAbsencePlaned);
                hours = (decimal)this.ApprovedAbsenceDuringWorkingTimeMin / 60;
                this.lvStatistics.Items[3].SubItems.Add(hours.ToString("F1"));
                this.lvStatistics.Items[3].SubItems.Add(stringApprovedAbsenceRealized);
                this.lvStatistics.Items[3].SubItems.Add(stringApprovedAbsencePlaned);
                hours = (decimal)this.UnapprovedAbsenceDuringWorkingTimeMin / 60;
                this.lvStatistics.Items[4].SubItems.Add(hours.ToString("F1"));
                this.lvStatistics.Items[4].SubItems.Add(stringUnapprovedAbsenceRealized);
                this.lvStatistics.Items[4].SubItems.Add(stringUnapprovedAbsencePlaned);
                hours = (decimal)this.ExtraHoursMin / 60;
                this.lvStatistics.Items[5].SubItems.Add(hours.ToString("F1"));
                this.lvStatistics.Items[5].SubItems.Add(stringExtraHoursRealized);
                this.lvStatistics.Items[5].SubItems.Add(stringExtraHoursPlaned);

                this.lvStatistics.EndUpdate();
                this.lvStatistics.EndUpdate();
                this.lvStatistics.Invalidate();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //Calculate from list of IOPairs phisical attendance, approved absence, realize working time and
        // whole day absence in minutes 
        private void CalucuateIOPairsTimeByPassTypes(List<IOPairTO> ioPairs)
        {
            try
            {
                foreach (IOPairTO ioPair in ioPairs)
                {
                    PassTypeTO passType = PassTypes[ioPair.PassTypeID];

                    TimeSpan duration = (new DateTime(ioPair.EndTime.TimeOfDay.Ticks - ioPair.StartTime.TimeOfDay.Ticks)).TimeOfDay;

                    switch (passType.IsPass)
                    {
                        case Constants.otherPaymentCode:
                            if (ioPair.PassTypeID == Constants.automaticShortBreakPassType || ioPair.PassTypeID == Constants.automaticPausePassType)
                            {
                                this.ApprovedAbsenceDuringWorkingTimeMin += (int)duration.TotalMinutes;
                            }
                            break;
                        case Constants.passOnReader:
                            if (ioPair.PassTypeID == Constants.regularWork)
                            {
                                this.PhysicalAttendanceMin += (int)duration.TotalMinutes;
                                this.RealizeWorkingTimeMin += (int)duration.TotalMinutes;
                            }
                            else
                            {
                                this.ApprovedAbsenceDuringWorkingTimeMin += (int)duration.TotalMinutes;
                                if (ioPair.PassTypeID == Constants.officialOut)
                                {
                                    this.RealizeWorkingTimeMin += (int)duration.TotalMinutes;
                                }
                            }
                            break;
                        case Constants.wholeDayAbsence:
                            this.WholeDayAbsenceMin += (int)duration.TotalMinutes;
                            if (ioPair.PassTypeID == Constants.officialTravel)
                            {
                                this.RealizeWorkingTimeMin += (int)duration.TotalMinutes;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
