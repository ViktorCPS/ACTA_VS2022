using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using TransferObjects;
using Util;
using Common;

namespace UI
{
    public partial class StatisticReportViewMultiWUControl : StatisticReportViewControl
    {
        //calculate value for each selected working unit depending of graph type
        //in minutes and percent of planed working time and realize working time
        //draw pie based on calculate values 
        //write all values and percentes into the table
        private int _graphType;
        private Dictionary<int, List<EmployeeTimeScheduleTO>> _emplTimeSchedules;
        private Dictionary<int, List<EmployeeTO>> _employees;
        private Dictionary<int, Dictionary<int, List<IOPairTO>>> _ioPairsList;
        private List<WorkingUnitTO> _wuList;
        private Hashtable _colors;
        private int totalValue;
        private Dictionary<WorkingUnitTO, int> valuesForWorkingUnits;
        //In order to have same color of workin units on all pie charts pass colors from window
        public Hashtable Colors
        {
            get { return _colors; }
            set { _colors = value; }
        }
        //Selected working units
        public List<WorkingUnitTO> WorkingUnitsList
        {
            get { return _wuList; }
            set { _wuList = value; }
        }

        //key is WUID memeber is ArrayList of employees
        public Dictionary<int, List<EmployeeTO>> Employees
        {
            get { return _employees; }
            set { _employees = value; }
        }

        //key is employeeID memeber is ArrayList of timeShedules
        public Dictionary<int, List<EmployeeTimeScheduleTO>> EmplTimeSchedules
        {
            get { return _emplTimeSchedules; }
            set { _emplTimeSchedules = value; }
        }

        //key is WUID member is hashtable with IOPairs for employees
        public Dictionary<int, Dictionary<int, List<IOPairTO>>> IOPairsList
        {
            get { return _ioPairsList; }
            set { _ioPairsList = value; }
        }
        //Value showing wich value needs to calculate
        public int GraphType
        {
            get { return _graphType; }
            set { _graphType = value; }
        }
        public StatisticReportViewMultiWUControl()
        {
            InitializeComponent();

            this.GraphType = 0;
            this.IOPairsList = new Dictionary<int,Dictionary<int,List<IOPairTO>>>();
            this.TimeSchemaList = new List<WorkTimeSchemaTO>();
            this.EmplTimeSchedules = new Dictionary<int,List<EmployeeTimeScheduleTO>>();
            this.Employees = new Dictionary<int,List<EmployeeTO>>();
            this.WorkingUnitsList = new List<WorkingUnitTO>();
            this.Holidays = new List<HolidayTO>();
            this.PassTypes = new Dictionary<int,PassTypeTO>();
            this.CalculatePlanedAndRealizedWorkingTime = false;
            this.Colors = new Hashtable();
            this.totalValue = 0;
            this.DateFrom = new DateTime();
            this.DateTo = new DateTime();
            this.valuesForWorkingUnits = new Dictionary<WorkingUnitTO,int>();
        }

        public StatisticReportViewMultiWUControl(int graphType, Dictionary<int, Dictionary<int, List<IOPairTO>>> ioPairs, Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedules, List<WorkTimeSchemaTO> timeSchemaList, Dictionary<int, List<EmployeeTO>> employees, List<WorkingUnitTO> workingUnitsList, List<HolidayTO> holydays, Dictionary<int, PassTypeTO> passTypes, bool calculatePlanedAndRealizedWorkingTime, Hashtable colors, DateTime dateFrom, DateTime dateTo)
        {
            InitializeComponent();

            this.GraphType = graphType;
            this.IOPairsList = ioPairs;
            this.TimeSchemaList = timeSchemaList;
            this.EmplTimeSchedules = emplTimeSchedules;
            this.Employees = employees;
            this.WorkingUnitsList = workingUnitsList;
            this.Holidays = holydays;
            this.PassTypes = passTypes;
            this.Colors = colors;
            this.CalculatePlanedAndRealizedWorkingTime = calculatePlanedAndRealizedWorkingTime;
            this.totalValue = 0;
            this.DateFrom = dateFrom;
            this.DateTo = dateTo;
            //calculate planed and working time if (CalculatePlanedAndRealizedWorkingTime) and value
            valuesForWorkingUnits = this.getValuesForWU();
        }

        private void StatisticReportViewMultiWUControl_Load(object sender, EventArgs e)
        {
           try
           {
               this.SetPieChartValues(valuesForWorkingUnits);
               ShowPlannedWorkingTime();
               ShowRealizedWorkingTime();
               this.setListViewValues(valuesForWorkingUnits);
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);
           }
        }

        //Calculate from list of IOPairs phisical attendance, approved absence, realize working time and
        // whole day absence in minutes 
        private int CalucuateIOPairsTimeByPassTypes(List<IOPairTO> ioPairs)
        {
            int value = 0;
            try
            {
                foreach (IOPairTO ioPair in ioPairs)
                {
                    PassTypeTO passType = PassTypes[ioPair.PassTypeID];

                    TimeSpan duration = (new DateTime(ioPair.EndTime.TimeOfDay.Ticks - ioPair.StartTime.TimeOfDay.Ticks)).TimeOfDay;
                    if((this.CalculatePlanedAndRealizedWorkingTime)&&(ioPair.PassTypeID == Constants.officialTravel || ioPair.PassTypeID == Constants.regularWork || ioPair.PassTypeID == Constants.officialOut))
                    {
                        this.RealizeWorkingTimeMin += (int)duration.TotalMinutes; 
                    }
                    if (this.GraphType != Constants.extraHoursGraphType)
                    {
                        switch (this.GraphType)
                        {
                            case Constants.physicalAttendanceGraphType:
                                if (passType.IsPass == Constants.passOnReader && ioPair.PassTypeID == Constants.regularWork)
                                {
                                    value += (int)duration.TotalMinutes;
                                }
                                break;
                            case Constants.wholeDayAbsenceGraphType:
                                if (passType.IsPass == Constants.wholeDayAbsence)
                                {
                                    value += (int)duration.TotalMinutes;
                                }
                                break;
                            case Constants.absenceDuringWorkingTimeGraphType:
                                if ((passType.IsPass == Constants.passOnReader && ioPair.PassTypeID != Constants.regularWork) ||
                                    (passType.IsPass == Constants.otherPaymentCode && (ioPair.PassTypeID == Constants.automaticShortBreakPassType || ioPair.PassTypeID == Constants.automaticPausePassType)))
                                {
                                    value += (int)duration.TotalMinutes;
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return value;
        }

        private void SetPieChartValues(Dictionary<WorkingUnitTO, int> valuesForWu)//seting values for pie chart graph
        {
            try
            {
                decimal[] ValuseForPieChart = new decimal[valuesForWu.Count];
                string[] PieSliceTexts = new string[valuesForWu.Count];
                string[] toolTips = new string[valuesForWu.Count];
                Color[] colors = new Color[valuesForWu.Count];
                float[] displ = new float[valuesForWu.Count];
                int i = 0;
                //foreach working unit in hashtable set slice value and text
                foreach (WorkingUnitTO wu in valuesForWu.Keys)
                {
                    ValuseForPieChart[i] =(decimal)((int)valuesForWu[wu]);
                    PieSliceTexts[i] = "";
                    if (this.totalValue > 0)
                    {
                        //if slice is larger than 5% show text on slice
                        if ((ValuseForPieChart[i] * 100 / this.totalValue) > 5)
                        {
                            PieSliceTexts[i] = wu.Name;
                        }
                    }
                    colors[i] =(Color)Colors[wu.WorkingUnitID];
                    toolTips[i] = wu.Name;
                    displ[i] = (float)0.05;
                    i++;
                }
                this.pccStatisticsView.Values = ValuseForPieChart;
                this.pccStatisticsView.Colors = colors;
                this.pccStatisticsView.Texts = PieSliceTexts;        
                this.pccStatisticsView.ToolTips = toolTips;               
                this.pccStatisticsView.SliceRelativeDisplacements = displ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private Dictionary<WorkingUnitTO, int> getValuesForWU()
        {
            //key is WU member is value for wu in minutes
            Dictionary<WorkingUnitTO, int> valuesForWU = new Dictionary<WorkingUnitTO, int>();
            try
            {
                 //foreach selected working unit get IOPairs and list of Employees
                foreach (WorkingUnitTO workingUnit in this.WorkingUnitsList)
                {
                    int value = 0;
                    Dictionary<int, List<IOPairTO>> ioPairsWU = new Dictionary<int,List<IOPairTO>>();
                    List<EmployeeTO> employeesWU = new List<EmployeeTO>();
                    if (this.IOPairsList.ContainsKey(workingUnit.WorkingUnitID))
                    {
                        ioPairsWU = this.IOPairsList[workingUnit.WorkingUnitID];
                    }
                    if (this.Employees.ContainsKey(workingUnit.WorkingUnitID))
                    {
                        employeesWU = this.Employees[workingUnit.WorkingUnitID];
                    }
                    //foreach employee from list of employees for wu get list of IOPairs and TimeShedules
                    foreach (EmployeeTO employee in employeesWU)
                    {
                        List<IOPairTO> ioPairsEmpl = new List<IOPairTO>();
                        List<EmployeeTimeScheduleTO> timeSchedulesForEmployee = new List<EmployeeTimeScheduleTO>();

                        if (ioPairsWU.ContainsKey(employee.EmployeeID))
                        {
                            ioPairsEmpl = ioPairsWU[employee.EmployeeID];
                        }
                        if (EmplTimeSchedules.ContainsKey(employee.EmployeeID))
                        {
                            timeSchedulesForEmployee = this.EmplTimeSchedules[employee.EmployeeID];
                        }
                        if (this.CalculatePlanedAndRealizedWorkingTime || this.GraphType != Constants.extraHoursGraphType)
                        {
                            value += this.CalucuateIOPairsTimeByPassTypes(ioPairsEmpl);
                        }
                        //we need to pass trough all days in interval if graphtype is extraHours or absenceDuringWorkingTime
                        //or if this is first control and need's to calculate planed working time
                        if (this.GraphType == Constants.extraHoursGraphType || this.GraphType == Constants.absenceDuringWorkingTimeGraphType || this.CalculatePlanedAndRealizedWorkingTime)
                        {
                            for (DateTime date = DateFrom; date <= DateTo; date = date.AddDays(1))
                             {
                            List<IOPairTO> ioPairsEmplDate = this.getIOPairsForEmplDate(ioPairsEmpl, date);//geting ioPairs for specified employee and date
                            
                                //if there is IOPairs and graphType is extraHours or this is first control call method for calculation
                                if ((ioPairsEmplDate.Count > 0) && ((this.GraphType == Constants.extraHoursGraphType) || this.CalculatePlanedAndRealizedWorkingTime))
                                {
                                   // if graphType is extraHoursGraphType calculate extraHours 
                                    bool calculateExtrHrs = (this.GraphType == Constants.extraHoursGraphType);
                                    value += calculatePlanedWorkingTimeAndExtraHours(date, timeSchedulesForEmployee, ioPairsEmplDate, calculateExtrHrs);//calculate extra hours for specified employee and date
                                }
                                else//if there is no IOPairs calculate Planed working time and absence during working time
                                {
                                    if (timeSchedulesForEmployee.Count > 0)
                                    {
                                        List<WorkTimeIntervalTO> intervalsEmpl = new List<WorkTimeIntervalTO>();
                                        intervalsEmpl = this.getTimeSchemaInterval(employee.EmployeeID, date, timeSchedulesForEmployee);//geting time intervals list for specified employee and date

                                        foreach (WorkTimeIntervalTO tsInterval in intervalsEmpl)//calculate total planed working time
                                        {
                                            TimeSpan duration = (new DateTime(tsInterval.EndTime.TimeOfDay.Ticks - tsInterval.StartTime.TimeOfDay.Ticks)).TimeOfDay;
                                            if (this.CalculatePlanedAndRealizedWorkingTime)
                                            {
                                                this.PlannedWorkingTimeMin += (int)duration.TotalMinutes;
                                            }
                                            if ((ioPairsEmpl.Count == 0)&&(this.GraphType == Constants.absenceDuringWorkingTimeGraphType))
                                            {
                                                value += (int)duration.TotalMinutes;
                                            }
                                        }
                                    }
                                    if (this.GraphType == Constants.extraHoursGraphType)
                                    {
                                        value += this.calculatePlanedWorkingTimeAndExtraHours(date, timeSchedulesForEmployee, ioPairsEmpl, true);
                                    }
                                }
                            }                            
                        }
                    }
                    valuesForWU.Add(workingUnit, value);
                    this.totalValue += value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return valuesForWU;
        }

        private void setListViewValues(Dictionary<WorkingUnitTO, int> valuesForWu)
        {
            try
            {
                this.lvStatistics.BeginUpdate();
                int i = 0;
                
                //foreach working unit calculate percent's and write it to table 
              foreach (WorkingUnitTO wu in this.WorkingUnitsList)
              {
                  int value = valuesForWu[wu];
                  this.lvStatistics.Items.Add(wu.Name);
                  this.lvStatistics.Items[i].UseItemStyleForSubItems = false;
                  this.lvStatistics.Items[i].SubItems.Add("");
                  this.lvStatistics.Items[i].SubItems[1].BackColor = (Color)Colors[wu.WorkingUnitID];
                  decimal hours = (decimal)value / 60;
                  this.lvStatistics.Items[i].SubItems.Add(hours.ToString("F1"));
                                   if (this.RealizeWorkingTimeMin > 0)//check if realized time is zero in order to avoid dividing with zero
                      this.lvStatistics.Items[i].SubItems.Add(value * 100 / this.RealizeWorkingTimeMin + "%");
                  else
                      this.lvStatistics.Items[i].SubItems.Add("");
                  if (this.PlannedWorkingTimeMin > 0)//check if planed time is zero in order to avoid dividing with zero
                      this.lvStatistics.Items[i].SubItems.Add(value * 100 / this.PlannedWorkingTimeMin + "%");
                  else
                      this.lvStatistics.Items[i].SubItems.Add("");
                  
                      i++;
              }
                
                this.lvStatistics.EndUpdate();
                this.lvStatistics.Invalidate();
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }      
    }
}
