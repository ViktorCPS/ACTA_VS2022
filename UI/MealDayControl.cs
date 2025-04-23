using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.Collections;

using Common;
using Util;
using TransferObjects;

namespace UI
{
    public partial class MealDayControl : UserControl
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        public DateTime date = new DateTime();
        public WorkTimeSchemaTO timeSchema;
        public bool isWorkingDay;
        public List<HolidayTO> holidays = new List<HolidayTO>();
        public bool isVacation = false;
        Color controlColor;
        public bool NotWorkingDay = false;
        private MealsWorkingUnitSchedule schedule;

        private MealsEmployeeSchedule emplSchedule;

        public MealsWorkingUnitSchedule Schedule
        {
            get { return schedule; }
            set 
            { 
                schedule = value;
                lblMeal.Text = schedule.MealsType;
                if (!Avaiable || byDays)
                {
                    this.BackColor = Color.Gray;
                    controlColor = Color.Gray;
                }
            }
        }

        public MealsEmployeeSchedule EmplSchedule
        {
            get { return emplSchedule; }
            set
            {
                emplSchedule = value;
                lblMeal.Text = emplSchedule.MealsType;
                if (!Avaiable || byDays)
                {
                    this.BackColor = Color.Gray;
                    controlColor = Color.Gray;
                }
            }
        }

        private MealsWorkingUnitSchedules parentControl;
        private int controlNumber = -1;

        private bool selected = false;
        public bool Avaiable = false;
        public bool byDays = false;
        private MealType chosenType;

        public MealType ChosenType
        {
            get { return chosenType; }
            set
            {
                chosenType = value;
                this.BackColor = Color.Gray;
                controlColor = Color.Gray;
                lblMeal.Text = chosenType.Name;
            }
        }

        public bool Selected
        {
            get { return selected; }
            set
            {
                if (!NotWorkingDay)
                {
                    selected = value;
                    if (selected)
                    {
                        this.BackColor = Color.CornflowerBlue;
                    }
                    else
                    {
                        this.BackColor = controlColor;
                    }
                }
            }
        }

        public MealDayControl()
        {
            InitializeComponent();
            if (!this.DesignMode)
            {
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                rm = new ResourceManager("UI.Resource", typeof(MealsAssigned).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();
            }

        }

        public MealDayControl(DateTime Date, WorkTimeSchemaTO timeSch, List<HolidayTO> holidayList, bool workingDay, bool Vac, int controlNum, MealsWorkingUnitSchedules parent)
        {
            InitializeComponent();
            if (!this.DesignMode)
            {
                date = Date;
                parentControl = parent;
                controlNumber = controlNum;

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                rm = new ResourceManager("UI.Resource", typeof(MealsAssigned).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();
                holidays = holidayList;
                timeSchema = timeSch;
                isWorkingDay = workingDay;
                isVacation = Vac;
            }
        }

        private void setLanguage()
        {
            try
            {
                lblDay.Text = rm.GetString(this.date.ToString("ddd"), culture);
                lblDayNum.Text = date.Day.ToString();
                lblMeal.Text = "";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesII.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void MealDayControl_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.DesignMode)
                {
                    chosenType = new MealType();
                    if (date.Date < DateTime.Now.Date)
                    {
                        this.Enabled = false;
                        NotWorkingDay = true;
                    }
                    else
                    {
                        if (timeSchema == null || timeSchema.TimeSchemaID == -1)
                        {
                            if (chosenType.MealTypeID != -1)
                            {
                                this.BackColor = Color.Gray;
                                lblMeal.Text = chosenType.Name;
                            }
                            else if (IsDateHolidayDate())
                            {
                                this.BackColor = Color.Pink;
                            }
                            else
                            {
                                this.BackColor = Color.Yellow;
                            }
                        }
                        else
                        {

                            if (!timeSchema.Type.Equals(Constants.schemaTypeIndustrial))
                            {
                                if (IsDateHolidayDate() || isVacation || (!isWorkingDay))
                                {
                                    this.BackColor = Color.Pink;
                                    NotWorkingDay = true;
                                    lblNotWorking.Visible = true;

                                    if (IsDateHolidayDate())
                                    {
                                        lblNotWorking.Text = rm.GetString("holiday", culture);
                                    }
                                    else if (isVacation)
                                    {
                                        lblNotWorking.Text = rm.GetString("vacation", culture);
                                    }
                                    else if (!isWorkingDay)
                                    {
                                        lblNotWorking.Text = rm.GetString("notWorking", culture);
                                    }
                                }
                                else
                                {
                                    this.BackColor = Color.Yellow;
                                }
                            }
                            else
                            {
                                if (isVacation)
                                {
                                    NotWorkingDay = true;
                                    lblNotWorking.Text = rm.GetString("vacation", culture);
                                }
                                else if (!isWorkingDay)
                                {
                                    NotWorkingDay = true;
                                    lblNotWorking.Text = rm.GetString("notWorking", culture);
                                }
                            }
                        }
                    }
                    controlColor = this.BackColor;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesII.MealDayControl_Load(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool IsDateHolidayDate()
        {
            bool dateIsHoliday = false;
            try
            {
                foreach (HolidayTO holiday in holidays)
                {
                    if (holiday.HolidayDate.Date == date.Date)
                    {
                        dateIsHoliday = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesII.IsDateHolidayDate(): " + ex.Message + "\n");
                throw ex;
            }
            return dateIsHoliday;
        }

        private void lblDay_Click(object sender, EventArgs e)
        {
            changeSelection();
        }
        private void changeSelection()
        {
            if (!NotWorkingDay && (Avaiable || byDays))
            {
                parentControl.controlSet(controlNumber);
            }
            
        }

        private void lblNotWorking_Click(object sender, EventArgs e)
        {
            changeSelection();
        }

        private void lblMeal_Click(object sender, EventArgs e)
        {
            changeSelection();
        }

        private void lblDayNum_Click(object sender, EventArgs e)
        {
            changeSelection();
        }

        private void MealDayControl_Click(object sender, EventArgs e)
        {
            changeSelection();
        }
    }
}
