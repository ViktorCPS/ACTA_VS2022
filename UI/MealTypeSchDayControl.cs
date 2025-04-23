using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TransferObjects;
using Common;
using Util;
using System.Resources;
using System.Globalization;
using System.Collections;

namespace UI
{
    public partial class MealTypeSchDayControl : UserControl
    {
        public DateTime date = new DateTime();
        private DateTime from = new DateTime();

        public DateTime From
        {
            get { return from; }
            set 
            {
                from = value;
                dtpFrom.Value = value;
            }
        }
        private DateTime to = new DateTime();

        public DateTime To
        {
            get { return to; }
            set 
            {
                to = value;
                dtpTo.Value = value;
            }
        }
        public List<HolidayTO> holidays = new List<HolidayTO>();

        private MealTypeSchedules parentControl;
        private int controlNumber = -1;

        public MealsTypeSchedule schedule;

        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        Color controlColor;

        private bool selected = false;

        public bool Selected
        {
            get { return selected; }
            set 
            {
                selected = value; if (selected)
                {
                    this.BackColor = Color.Blue;
                }
                else
                {
                    this.BackColor = controlColor;
                }
            }
        }

        public MealTypeSchDayControl()
        {
            InitializeComponent();
            try
            {
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                rm = new ResourceManager("UI.Resource", typeof(MealsAssigned).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();


                from = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,0,0,0);
                to = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            }
            catch { }
        }

        public MealTypeSchDayControl(DateTime fromTime, DateTime toTime, List<HolidayTO> holidayList, DateTime dateSch, int controlNum, MealTypeSchedules parent)
        {
            InitializeComponent();
            try
            {
                date = dateSch;
                from = fromTime;
                to = toTime;
                holidays = holidayList;
                controlNumber = controlNum;
                parentControl = parent;

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                rm = new ResourceManager("UI.Resource", typeof(MealsAssigned).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();
            }
            catch { }
        }

        private void setLanguage()
        {
            try
            {

                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);
                lblDay.Text = rm.GetString(this.date.ToString("ddd"), culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesII.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void MealTypeSchDayControl_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.DesignMode)
                {
                    lblDayNum.Text = date.Day.ToString();
                    dtpFrom.Value = from;
                    dtpTo.Value = to;

                    bool isHoliday = IsDateHolidayDate();

                    if (date.Date >= DateTime.Now.Date)
                    {
                        if (isHoliday)
                        {
                            this.BackColor = Color.Pink;
                        }
                        else
                        {
                            this.BackColor = Color.LightBlue;
                        }
                    }
                    else
                    {
                        this.Enabled = false;
                    }
                    controlColor = this.BackColor;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesII.sMealTypeSchDayControl_Load(): " + ex.Message + "\n");
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

        private void MealTypeSchDayControl_Click(object sender, EventArgs e)
        {
            changeSelection();
        }

        private void lblDay_Click(object sender, EventArgs e)
        {
            changeSelection();
        }

        private void lblDayNum_Click(object sender, EventArgs e)
        {
            changeSelection();
        }

        private void lblTo_Click(object sender, EventArgs e)
        {
            changeSelection();
        }

        private void lblFrom_Click(object sender, EventArgs e)
        {
            changeSelection();
        }

        private void changeSelection()
        {
            Selected = !selected;
            if (selected)
            {
                parentControl.controlSelected(controlNumber);
            }
            else
            {
                parentControl.controlDeselected(controlNumber);
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            from = dtpFrom.Value;
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            to = dtpTo.Value;
        }
    }
}
