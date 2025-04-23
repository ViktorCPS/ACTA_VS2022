using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Resources;
using System.Globalization;

using Util;
using Common;
using TransferObjects;

namespace UI
{
    public partial class MealTypeSchedules : UserControl
    {
        
        int x = 3;
        int y = 3;
        List<HolidayTO> holidays = new List<HolidayTO>();
        List<MealTypeSchDayControl> listControls = new List<MealTypeSchDayControl>();

        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;

        int firstControlSelected = -1;
        private bool loading = false;

        public MealTypeSchedules()
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            logInUser = NotificationController.GetLogInUser();

            rm = new ResourceManager("UI.Resource", typeof(MealsAssigned).Assembly);
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
        }

        private void MealTypeSchedules_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (!this.DesignMode)
                {
                    loading = true;
                    DateTime from = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    DateTime to = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 0);
                    dtpFrom.Value = from;
                    dtpTo.Value = to;

                    holidays = new Holiday().Search(new DateTime(), new DateTime());
                    setLanguage();
                    populateMealTypeCombo();
                    dtpMonth.Value = DateTime.Now;
                    loading = false;
                    setCalendar(dtpMonth.Value);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesSchedules.MealTypeSchedules_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }


        private void setLanguage()
        {
            try
            {
                this.gbGroupChange.Text = rm.GetString("gbGroupChange", culture);
                this.gbMealData.Text = rm.GetString("gbMealData", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblMeal.Text = rm.GetString("lblMeal1", culture);
                btnChange.Text = rm.GetString("btnSet", culture);
                btnSave.Text = rm.GetString("btnSave", culture);
                gbLegend.Text = rm.GetString("gbLegend", culture);
                lblChangableDays.Text = rm.GetString("lblChangableDays", culture);
                lblDiseabled.Text = rm.GetString("lblDiseabled", culture);
                lblHolidays.Text = rm.GetString("lHolidays", culture);
                lblSelectedDays.Text = rm.GetString("lblSelectedDays", culture);
                btnMonthlyMenu.Text = rm.GetString("btnMonthlyMenu", culture);
                
               
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesSchedules.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        /// <summary>
        /// Populate Meal Type Combo Box
        /// </summary>
        private void populateMealTypeCombo()
        {
            try
            {
                ArrayList mealTypeArray = new MealType().Search(-1, "", "", new DateTime(), new DateTime());
                mealTypeArray.Insert(0, new MealType(-1, rm.GetString("all", culture), rm.GetString("all", culture), new DateTime(), new DateTime(0)));

                cbMeal.DataSource = mealTypeArray;
                cbMeal.DisplayMember = "Name";
                cbMeal.ValueMember = "MealTypeID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesSchedules.populateMealTypeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }


        private void setCalendar(DateTime month)
        {
            foreach (Control c in listControls)
            {
                c.Dispose();
            }
            listControls.Clear();
            listControls = new List<MealTypeSchDayControl>();
            panel1.Controls.Clear();
            GC.Collect();

            x = 3;
            y = 3;
         //   listControls = new List<MealTypeSchDayControl>();

            DateTime date = new DateTime(month.Year, month.Month, 1);
            DateTime dateTemp = new DateTime(month.Year, month.Month, 1);

            DateTime from = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime to = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            ArrayList schedules = new ArrayList();
            Dictionary<DateTime, MealsTypeSchedule> scheduleDictionary = new Dictionary<DateTime, MealsTypeSchedule>();

            try
            {
                if (cbMeal.Items.Count > 0 && (int)cbMeal.SelectedValue > 0)
                {
                    schedules = new MealsTypeSchedule().Search((int)cbMeal.SelectedValue, new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1), new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1).AddMonths(1).AddDays(-1));
                    foreach (MealsTypeSchedule sch in schedules)
                    {
                        if (!scheduleDictionary.ContainsKey(sch.Date.Date))
                        {
                            scheduleDictionary.Add(sch.Date.Date, sch);
                        }
                    }
                }
            }
            catch { }

            int startDay = 0;                       
            startDay = getDayOfWeek(date);
            int j = startDay;
            x += 116 * startDay;
            for (int i = 1; i <= 6; i++)
            {
                while ( j <= 6 )                
                {
                    if(date.Month != dateTemp.Month)
                        break;
                    DateTime currFrom = from;
                    DateTime currTo = to;
                    MealsTypeSchedule sch = new MealsTypeSchedule();
                    if (scheduleDictionary.ContainsKey(dateTemp.Date))
                    {
                        sch = scheduleDictionary[dateTemp.Date];
                        currFrom = scheduleDictionary[dateTemp.Date].HoursFrom;
                        currTo = scheduleDictionary[dateTemp.Date].HoursTo;
                    }
                    MealTypeSchDayControl meal = new MealTypeSchDayControl(currFrom, currTo, holidays, dateTemp, listControls.Count, this);
                    meal.Location = new Point(x, y);
                    if(sch.MealTypeID != -1)
                    meal.schedule = sch;
                    panel1.Controls.Add(meal);
                    listControls.Add(meal);
                    x += meal.Width + 6;
                    j++;
                    dateTemp = dateTemp.AddDays(1);
                }
                j = 0;
                startDay = 0;
                y += 75;
                x = 3;
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
                log.writeLog(DateTime.Now + " MealTypesSchedules.getDayOfWeek(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        public void controlSelected(int controlNum)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                if (firstControlSelected == -1)
                {
                    firstControlSelected = controlNum;
                    MealTypeSchDayControl SelControl = listControls[controlNum];
                    foreach (MealTypeSchDayControl control in listControls)
                    {
                        if (SelControl.date != control.date)
                            control.Selected = false;
                    }
                }
                else if (controlNum > firstControlSelected)
                {
                    for (int i = firstControlSelected; i <= controlNum; i++)
                    {
                        MealTypeSchDayControl control = (MealTypeSchDayControl)listControls[i];
                        control.Selected = true;
                    }
                    firstControlSelected = -1;
                }
                else
                {
                    for (int i = firstControlSelected; i <= controlNum; i--)
                    {
                        MealTypeSchDayControl control = (MealTypeSchDayControl)listControls[i];
                        control.Selected = true;
                    }
                    firstControlSelected = -1;
                }
            }
            else if (Control.ModifierKeys != Keys.Control)
            {
                MealTypeSchDayControl SelControl = listControls[controlNum];
                foreach (MealTypeSchDayControl control in listControls)
                {
                    if (SelControl.date != control.date)
                        control.Selected = false;
                }
                firstControlSelected = controlNum;
            }
            else
            {

                firstControlSelected = controlNum;
            }
        }

        public void controlDeselected(int controlNum)
        {
            if (Control.ModifierKeys != Keys.Control)
            {
                foreach (MealTypeSchDayControl control in listControls)
                {
                    control.Selected = false;
                }
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (MealTypeSchDayControl control in listControls)
                {
                    if (control.Selected)
                    {
                        control.From = dtpFrom.Value;
                        control.To = dtpTo.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesSchedules.btnChange_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            dtpMonth.Value = dtpMonth.Value.AddMonths(1);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            dtpMonth.Value = dtpMonth.Value.AddMonths(-1);
        }

        private void dtpMonth_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if(!loading)
                setCalendar(dtpMonth.Value);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesSchedules.dtpMonth_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbMeal_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(!loading)
                setCalendar(dtpMonth.Value);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesSchedules.dtpMonth_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool valid = validate();
                if (valid)
                {
                    bool saved = true;
                    MealsTypeSchedule sch = new MealsTypeSchedule();
                    sch.BeginTransaction();
                    try
                    {
                        foreach (MealTypeSchDayControl control in listControls)
                        {
                            if (control.Enabled)
                            {
                                
                                if (control.schedule == null)
                                {
                                    if (control.From.TimeOfDay != control.To.TimeOfDay)
                                    {
                                        int i = sch.Save((int)cbMeal.SelectedValue, control.date, control.From, control.To, false);
                                        saved = saved && (i == 1);
                                    }
                                }
                                else
                                {
                                    saved = saved && sch.Delete((int)cbMeal.SelectedValue, control.date, false);
                                    if (saved)
                                    {
                                        if (control.From.TimeOfDay != control.To.TimeOfDay)
                                        {
                                            saved = saved && (sch.Save((int)cbMeal.SelectedValue, control.date, control.From, control.To, false) == 1);
                                        }
                                    }
                                }
                            }
                           
                        }

                        if (saved)
                        {
                            sch.CommitTransaction();
                            MessageBox.Show(rm.GetString("MealSchSaved", culture));
                        }
                        else
                        {
                            sch.RollbackTransaction();
                            MessageBox.Show(rm.GetString("MealSchNOTSaved", culture));
                        }
                    }
                    catch (Exception ex)
                    {
                        sch.RollbackTransaction();
                        log.writeLog(DateTime.Now + " MealTypesSchedules.btnSave_Click(): " + ex.Message + "\n");
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesSchedules.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private bool validate()
        {
            if ((int)cbMeal.SelectedValue == -1)
            {
                MessageBox.Show(rm.GetString("selMealType", culture));
                return false;
            }
            return true;
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnViewMonthSchedule_Click(object sender, EventArgs e)
        {
            try
            {
                MonthlyMenu menu = new MonthlyMenu(dtpMonth.Value.Date);
                menu.ShowDialog();
                MealTypeSchedules_Load(this, null);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealTypesSchedules.btnViewMonthSchedule_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

    }
}
