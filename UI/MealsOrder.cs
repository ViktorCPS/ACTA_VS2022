using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;
using System.Collections;
using Winforms.Components.ApplicationIdleData;

namespace UI
{
    public partial class MealsOrder : Form
    {
        bool _IsRunning = false;
        string idleTime = "";
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        DateTime mondayDate = new DateTime();
        ArrayList scheduleList = new ArrayList();
        Dictionary<DayOfWeek, ArrayList> mealsByDay;
        Dictionary<string, RestaurantShiftsTO> shiftsByDate; 
        List<Control> listControlsTypes = new List<Control>();
        static int count = 0;
        EmployeeTO currentEmpl;
        Dictionary<string, DateTime> dateOfTheDay;
        static bool canSeeAndEditCurrent = false;
        static bool canOnlyViewCurrent = false;
        bool isAdministrator = false;
        string shift = "";

        public MealsOrder()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;

                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                rm = new ResourceManager("UI.Resource", typeof(MealTypesAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                canSeeAndEditCurrent = false;
                canOnlyViewCurrent = false;
                isAdministrator = true;
                mealsByDay = new Dictionary<DayOfWeek, ArrayList>();
                dateOfTheDay = new Dictionary<string, DateTime>();
                shiftsByDate = new Dictionary<string, RestaurantShiftsTO>();
                mondayDate = new DateTime();
                setDates();
                scheduleList = new MealsTypeSchedule().Search(-1, mondayDate, mondayDate.AddDays(6));
                currentEmpl = new EmployeeTO();
                setLanguage();
                setBackgroundPicture();
                //TIMER code
                //SetIdleTimer();
                //if (!idleTime.Equals("") && !idleTime.Equals("0"))
                //{
                //    applicationIdle.IdleAsync += new EventHandler(applicationIdle_IdleAsync);
                //}

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.MealsOrder(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }            

        public MealsOrder(EmployeeTO employee)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;

                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                rm = new ResourceManager("UI.Resource", typeof(MealTypesAdd).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                
                canSeeAndEditCurrent = false;
                canOnlyViewCurrent = false;
                isAdministrator = false;
                mealsByDay = new Dictionary<DayOfWeek, ArrayList>();
                dateOfTheDay = new Dictionary<string, DateTime>();
                shiftsByDate = new Dictionary<string, RestaurantShiftsTO>();
                setDates();
                scheduleList = new MealsTypeSchedule().Search(-1, mondayDate, mondayDate.AddDays(6));
                currentEmpl = employee;
                setLanguage();
                setBackgroundPicture();

                //TIMER code
                //SetIdleTimer();
                //if (!idleTime.Equals("") && !idleTime.Equals("0"))
                //{
                //    applicationIdle.IdleAsync += new EventHandler(applicationIdle_IdleAsync);
                //}

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.MealsOrder(): " + ex.Message + "\n");
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
                lblError.Text = "";
                //lblMonday.Text = rm.GetString("MondayDay", culture);
                //lblTuesday.Text = rm.GetString("TuesdayDay", culture);
                //lblWednesday.Text = rm.GetString("WednesdayDay", culture);
                //lblThursday.Text = rm.GetString("ThursdayDay", culture);
                //lblFriday.Text = rm.GetString("FridayDay", culture);
                //lblSaturday.Text = rm.GetString("SaturdayDay", culture);
       
                if (currentEmpl != null && currentEmpl.EmployeeID != -1)
                { 
                     lblEmployee.Text = rm.GetString("Employee", culture);
                     lblEmplData.Text =  currentEmpl.FirstAndLastName + " (" + currentEmpl.EmployeeID + ")";
                }
                else
                {
                    lblEmployee.Text = "";
                    lblEmplData.Text = "";
                }
                lblToOrder1.Text = lblToOrder2.Text = lblToOrder3.Text = lblToOrder4.Text = lblToOrder5.Text = rm.GetString("toOrder", culture);
                lblYouOrdered1.Text = lblYouOrdered2.Text = lblYouOrdered3.Text = lblYouOrdered4.Text = lblYouOrdered5.Text = rm.GetString("youOrdered", culture);
                pbOrderMonday.Image = pbOrderTuesday.Image = pbOrderWednesday.Image = pbOrderThursday.Image = pbOrderFriday.Image  =  null;
                tbOrderMonday.Text = tbOrderTuesday.Text = tbOrderWednesday.Text = tbOrderThursday.Text = tbOrderFriday.Text  = "";               
                lblMoMealName.Text = lblTuMealName.Text = lblWeMealName.Text = lblThMealName.Text = lblFrMealName.Text = "";
                btnExit.Text = rm.GetString("btnExit", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setDates()
        {
            try
            {
               
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;

                DateTime now = DateTime.Now;
                if (!canSeeAndEditCurrent && !canOnlyViewCurrent)
                {
                    switch (now.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            mondayDate = now.AddDays(7);
                            break;
                        case DayOfWeek.Tuesday:
                            mondayDate = now.AddDays(6);
                            break;
                        case DayOfWeek.Wednesday:
                            mondayDate = now.AddDays(5);
                            break;
                        case DayOfWeek.Thursday:
                            mondayDate = now.AddDays(4);
                            break;
                        case DayOfWeek.Friday:
                            mondayDate = now.AddDays(3);
                            break;
                        case DayOfWeek.Saturday:
                            mondayDate = now.AddDays(2);
                            break;
                        case DayOfWeek.Sunday:
                            mondayDate = now.AddDays(1);
                            break;
                    }
                }
                else
                {
                    switch (now.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            mondayDate = now;
                            if(isAdministrator == false)
                            {
                                gbMonday.Visible = false;
                            }
                            break;
                        case DayOfWeek.Tuesday:
                            mondayDate = now.AddDays(-1);
                            if (isAdministrator == false)
                            {
                                gbMonday.Visible = false;
                                gbTuesday.Visible = false;
                            }
                            break;
                        case DayOfWeek.Wednesday:
                            mondayDate = now.AddDays(-2);
                            if (isAdministrator == false)
                            {
                                gbMonday.Visible = false;
                                gbTuesday.Visible = false;
                                gbWednesday.Visible = false;
                            }
                            break;
                        case DayOfWeek.Thursday:
                            mondayDate = now.AddDays(-3);
                            if (isAdministrator == false)
                            {
                                gbMonday.Visible = false;
                                gbTuesday.Visible = false;
                                gbWednesday.Visible = false;
                                gbThursday.Visible = false;
                            }
                            break;
                        case DayOfWeek.Friday:
                            mondayDate = now.AddDays(-4);
                            if (isAdministrator == false)
                            {
                                gbMonday.Visible = false;
                                gbTuesday.Visible = false;
                                gbWednesday.Visible = false;
                                gbThursday.Visible = false;
                                gbFriday.Visible = false;
                            }
                            break;
                        case DayOfWeek.Saturday:
                            mondayDate = now.AddDays(-5);
                            if (isAdministrator == false)
                            {
                                gbMonday.Visible = false;
                                gbTuesday.Visible = false;
                                gbWednesday.Visible = false;
                                gbThursday.Visible = false;
                                gbFriday.Visible = false;
                                gbSaturday.Visible = false;
                            }
                            break;
                        case DayOfWeek.Sunday:
                            mondayDate = now.AddDays(-6);
                            if (isAdministrator == false)
                            {
                                gbMonday.Visible = false;
                                gbTuesday.Visible = false;
                                gbWednesday.Visible = false;
                                gbThursday.Visible = false;
                                gbFriday.Visible = false;
                                gbSaturday.Visible = false;
                                gbSunday.Visible = false;
                            }
                            break;

                    }
                }
                lblMonday.Text = rm.GetString("MondayDay", culture) + " " + mondayDate.ToShortDateString();;
                lblTuesday.Text = rm.GetString("TuesdayDay", culture) + " " + mondayDate.AddDays(1).ToShortDateString(); ;
                lblWednesday.Text = rm.GetString("WednesdayDay", culture) + " " + mondayDate.AddDays(2).ToShortDateString();
                lblThursday.Text = rm.GetString("ThursdayDay", culture) +" " + mondayDate.AddDays(3).ToShortDateString();
                lblFriday.Text = rm.GetString("FridayDay", culture) + " " + mondayDate.AddDays(4).ToShortDateString();
                lblSaturday.Text = rm.GetString("SaturdayDay", culture) + " " + mondayDate.AddDays(5).ToShortDateString();
                lblSunday.Text = rm.GetString("SundayDay", culture) + " " + mondayDate.AddDays(6).ToShortDateString();

                DateTime date = new DateTime(mondayDate.Year, mondayDate.Month, mondayDate.Day, 0, 0, 0);
                dateOfTheDay.Add(DayOfWeek.Monday.ToString(), date);
                dateOfTheDay.Add(DayOfWeek.Tuesday.ToString(), date.AddDays(1));
                dateOfTheDay.Add(DayOfWeek.Wednesday.ToString(), date.AddDays(2));
                dateOfTheDay.Add(DayOfWeek.Thursday.ToString(), date.AddDays(3));
                dateOfTheDay.Add(DayOfWeek.Friday.ToString(), date.AddDays(4));
                dateOfTheDay.Add(DayOfWeek.Saturday.ToString(), date.AddDays(5));
                dateOfTheDay.Add(DayOfWeek.Sunday.ToString(), date.AddDays(6));

                shiftsByDate = new Dictionary<string, RestaurantShiftsTO>();
                shiftsByDate.Add(DayOfWeek.Monday.ToString(), new RestaurantShiftsTO());
                shiftsByDate.Add(DayOfWeek.Tuesday.ToString(), new RestaurantShiftsTO());
                shiftsByDate.Add(DayOfWeek.Wednesday.ToString(), new RestaurantShiftsTO());
                shiftsByDate.Add(DayOfWeek.Thursday.ToString(), new RestaurantShiftsTO());
                shiftsByDate.Add(DayOfWeek.Friday.ToString(), new RestaurantShiftsTO());
                shiftsByDate.Add(DayOfWeek.Saturday.ToString(), new RestaurantShiftsTO());
                shiftsByDate.Add(DayOfWeek.Sunday.ToString(), new RestaurantShiftsTO());
                setButtonsColor();

                bool enableEverything = true;
                if (canOnlyViewCurrent)// if canOnlyViewCurrent is true, disable everything on screen
                {
                    enableEverything = false;
                }
                enableOrDisableView(enableEverything);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.setDates(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setBackgroundPicture()
        {
            try
            {
                Image image = Image.FromFile(Constants.KeteringPricaBackgroundImagePath);
                if (image != null)
                {
                    this.BackgroundImage = image;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.setBackgroundPicture(): " + ex.Message + "\n");
            }
        }

        private void enableOrDisableView(bool p)
        {
            try
            {
                panelMoMeals.Enabled = p;
                panelTuMeals.Enabled = p;
                panelWeMeals.Enabled = p;
                panelThMeals.Enabled = p;
                panelFrMeals.Enabled = p;

                panelMoOrder.Enabled = p;
                panelTuOrder.Enabled = p;
                panelWeOrder.Enabled = p;
                panelThOrder.Enabled = p;
                panelFrOrder.Enabled = p;

                btnMondayI.Enabled = btnMondayII.Enabled = btnMondayIII.Enabled = p;
                btnTuesdayI.Enabled = btnTuesdayII.Enabled = btnTuesdayIII.Enabled = p;
                btnWednesdayI.Enabled = btnWednesdayII.Enabled = btnWednesdayIII.Enabled = p;
                btnThursdayI.Enabled = btnThursdayII.Enabled = btnThursdayIII.Enabled = p;
                btnFridayI.Enabled = btnFridayII.Enabled = btnFridayIII.Enabled = p;
               // btnSaturdayI.Enabled = btnSaturdayII.Enabled = btnSaturdayIII.Enabled = btnSaturdayMeal.Enabled = p;
               // btnSundayI.Enabled = btnSundayII.Enabled = btnSundayIII.Enabled = btnSundayMeal.Enabled = p;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.enableOrDisableView(): " + ex.Message + "\n");
            }
        }
        
        private void MealsOrder_Load(object sender, EventArgs e)
        {
            try
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;

                CreateControls();

                checkOrders(); //check if employee already ordered somethin for next week
                count = scheduleList.Count; //count meals
                foreach (MealsTypeSchedule schedule in scheduleList)
                {
                    int mealTypeID = schedule.MealTypeID;
                    DayOfWeek day = schedule.Date.DayOfWeek;

                    if (!mealsByDay.ContainsKey(day))
                    {
                        mealsByDay.Add(day, new ArrayList());
                    }

                    mealsByDay[day].Add(schedule); // for every day in week, add array of meals
                }

                PictureBox[] pics = new PictureBox[count];
                Label[] labels = new Label[count];
                int i = 0;

                foreach (DayOfWeek day in mealsByDay.Keys) //foreach day in a week
                {
                    ArrayList meals = mealsByDay[day]; //get meals for that day
                    int y = 0;

                    foreach (MealsTypeSchedule typeSchedule in meals)  //every meal add to form using control
                    {
                        MealPictureControl control = new MealPictureControl(typeSchedule.MealTypeID, day, this);
                        control.Location = new Point(0, y);
                        i++;                       
                        switch (day)
                        {
                            case DayOfWeek.Monday:
                                if(gbMonday.Visible)
                                    panelMoMeals.Controls.Add(control);
                                break;
                            case DayOfWeek.Tuesday:
                                if(gbTuesday.Visible)
                                    panelTuMeals.Controls.Add(control);
                                break;
                            case DayOfWeek.Wednesday:
                                if(gbWednesday.Visible)                                
                                    panelWeMeals.Controls.Add(control);
                                break;
                            case DayOfWeek.Thursday:
                                if(gbThursday.Visible)
                                    panelThMeals.Controls.Add(control);
                                break;
                            case DayOfWeek.Friday:
                                if(gbFriday.Visible)
                                    panelFrMeals.Controls.Add(control);
                                break;
                            //case DayOfWeek.Saturday:
                            //    if(gbSaturday.Visible)
                            //      //  panelSaMeals.Controls.Add(control);
                            //    break;
                            //case DayOfWeek.Sunday:
                            //    if (gbSunday.Visible)
                            //        //  panelSaMeals.Controls.Add(control);
                            //        break;
                        }
                        listControlsTypes.Add(control);
                        y += 104;
                    }
                }

                //TIMER code
                SetIdleTimer();
                //if (!idleTime.Equals("") && !idleTime.Equals("0"))
                //{
                //    applicationIdle.IdleAsync += new EventHandler(applicationIdle_IdleAsync);
                //}
                //SetIdleTimer();
                if (!idleTime.Equals("") && !idleTime.Equals("0"))
                {
                    StartIdleTimer();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.MealsOrder_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void checkOrders()
        {
            try
            {
                if (isAdministrator == false)
                {
                    DateTime monday = dateOfTheDay[DayOfWeek.Monday.ToString()];
                    DateTime sunday = dateOfTheDay[DayOfWeek.Sunday.ToString()];
                    DateTime sundayMidnight = new DateTime(sunday.Year, sunday.Month, sunday.Day, 23, 59, 59);
                    ArrayList listOfOrderedMeals = new MealsEmployeeSchedule().SearchForEmpl(monday, sundayMidnight, currentEmpl.EmployeeID); //check ordered meals from monday 00:00 until friday 23:59
                    if (listOfOrderedMeals.Count > 0)
                    {
                        foreach (string day in dateOfTheDay.Keys)
                        {
                            foreach (MealsEmployeeSchedule schedule in listOfOrderedMeals)
                            {
                                if (schedule.Date.Equals(dateOfTheDay[day]))
                                {
                                    int typeID = schedule.MealTypeID;
                                    MealTypeAdditionalDataTO additionalData = new MealTypeAdditionalData().GetAdditionalData(typeID);
                                    MealTypeTO type = new MealType().GetMealType(typeID);
                                    byte[] byteArrayIn = null;
                                    Image image = null;
                                    if (additionalData.Picture == null)
                                    {
                                        image = Image.FromFile(Constants.DefaultMealImagePath);
                                    }
                                    else
                                    {
                                        byteArrayIn = additionalData.Picture;
                                        MemoryStream ms = new MemoryStream(byteArrayIn);
                                        image = Image.FromStream(ms);
                                    }
                                    shiftsByDate[day].CurrentTypeID = typeID;
                                    switch (day)
                                    {
                                        case "Monday":
                                            pbOrderMonday.Image = image;
                                            pbOrderMonday.Tag = typeID;
                                            if (additionalData.DescriptionAdditional != null && !additionalData.DescriptionAdditional.Equals(""))
                                            {
                                                lblMoMealName.Text = type.Name;
                                                tbOrderMonday.Text = additionalData.DescriptionAdditional;
                                            }
                                            switch (schedule.Shift)
                                            {
                                                case "I":
                                                    btnMondayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnMondayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnMondayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    break;
                                                case "II":
                                                    btnMondayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnMondayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnMondayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    break;
                                                case "III":
                                                    btnMondayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnMondayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnMondayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    break;
                                                default:
                                                    btnMondayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnMondayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnMondayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    shiftsByDate[day].Current = "";
                                                    break;
                                            }
                                            break;
                                        case "Tuesday":
                                            pbOrderTuesday.Image = image;
                                            pbOrderTuesday.Tag = typeID;
                                            if (additionalData.DescriptionAdditional != null && !additionalData.DescriptionAdditional.Equals(""))
                                            {
                                                lblTuMealName.Text = type.Name;
                                                tbOrderTuesday.Text = additionalData.DescriptionAdditional;
                                            }
                                            switch (schedule.Shift)
                                            {
                                                case "I":
                                                    btnTuesdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnTuesdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnTuesdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    break;
                                                case "II":
                                                    btnTuesdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnTuesdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnTuesdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    break;
                                                case "III":
                                                    btnTuesdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnTuesdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnTuesdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    break;
                                                default:
                                                    btnTuesdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnTuesdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnTuesdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    shiftsByDate[day].Current = "";
                                                    break;
                                            }
                                            break;
                                        case "Wednesday":
                                            pbOrderWednesday.Image = image;
                                            pbOrderWednesday.Tag = typeID;
                                            if (additionalData.DescriptionAdditional != null && !additionalData.DescriptionAdditional.Equals(""))
                                            {
                                                lblWeMealName.Text = type.Name;
                                                tbOrderWednesday.Text = additionalData.DescriptionAdditional;
                                            }
                                            switch (schedule.Shift)
                                            {
                                                case "I":
                                                    btnWednesdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnWednesdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnWednesdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    break;
                                                case "II":
                                                    btnWednesdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnWednesdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnWednesdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    break;
                                                case "III":
                                                    btnWednesdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnWednesdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnWednesdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    break;
                                                default:
                                                    btnWednesdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnWednesdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnWednesdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    shiftsByDate[day].Current = "";
                                                    break;
                                            }
                                            break;
                                        case "Thursday":
                                            pbOrderThursday.Image = image;
                                            pbOrderThursday.Tag = typeID;
                                            if (additionalData.DescriptionAdditional != null && !additionalData.DescriptionAdditional.Equals(""))
                                            {
                                                lblThMealName.Text = type.Name;
                                                tbOrderThursday.Text = additionalData.DescriptionAdditional;
                                            }
                                            switch (schedule.Shift)
                                            {
                                                case "I":
                                                    btnThursdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnThursdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnThursdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    break;
                                                case "II":
                                                    btnThursdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnThursdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnThursdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    break;
                                                case "III":
                                                    btnThursdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnThursdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnThursdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    break;
                                                default:
                                                    btnThursdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnThursdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnThursdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    shiftsByDate[day].Current = "";
                                                    break;
                                            }
                                            break;
                                        case "Friday":
                                            pbOrderFriday.Image = image;
                                            pbOrderFriday.Tag = typeID;
                                            if (additionalData.DescriptionAdditional != null && !additionalData.DescriptionAdditional.Equals(""))
                                            {
                                                lblFrMealName.Text = type.Name;
                                                tbOrderFriday.Text = additionalData.DescriptionAdditional;
                                            }
                                            switch (schedule.Shift)
                                            {
                                                case "I":
                                                    btnFridayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnFridayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnFridayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    break;
                                                case "II":
                                                    btnFridayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnFridayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnFridayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    break;
                                                case "III":
                                                    btnFridayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnFridayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnFridayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    break;
                                                default:
                                                    btnFridayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnFridayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnFridayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    shiftsByDate[day].Current = "";
                                                    break;
                                            }
                                            break;
                                        case "Saturday":
                                            //pbOrderSaturday.Image = image;
                                            //pbOrderSaturday.Tag = typeID;
                                            //if (additionalData.DescriptionAdditional != null && !additionalData.DescriptionAdditional.Equals(""))
                                            //{
                                                //lblSaMealName.Text = type.Name;
                                                //tbOrderSaturday.Text = additionalData.DescriptionAdditional;
                                            //}
                                            switch (schedule.Shift)
                                            {
                                                case "I":
                                                    btnSaturdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnSaturdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSaturdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSaturdayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    btnSaturdayMeal.Text = rm.GetString("youOrdered", culture) + " " + type.Name;
                                                    shiftsByDate[day].SaturdayOrder = true;
                                                    break;
                                                case "II":
                                                    btnSaturdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnSaturdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSaturdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSaturdayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    btnSaturdayMeal.Text = rm.GetString("youOrdered", culture) + " " + type.Name;
                                                    shiftsByDate[day].SaturdayOrder = true;
                                                    break;
                                                case "III":
                                                    btnSaturdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnSaturdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSaturdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSaturdayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    btnSaturdayMeal.Text = rm.GetString("youOrdered", culture) + " " + type.Name;
                                                    shiftsByDate[day].SaturdayOrder = true;
                                                    break;
                                                default:
                                                    btnSaturdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSaturdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSaturdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    shiftsByDate[day].Current = "";
                                                    btnSaturdayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSaturdayMeal.Text = rm.GetString("makeReservation", culture);
                                                    shiftsByDate[day].SaturdayOrder = false;
                                                    break;
                                            }
                                            break;
                                        case "Sunday":
                                            //pbOrderSaturday.Image = image;
                                            //pbOrderSaturday.Tag = typeID;
                                            //if (additionalData.DescriptionAdditional != null && !additionalData.DescriptionAdditional.Equals(""))
                                            //{
                                                //lblSaMealName.Text = type.Name;
                                                //tbOrderSaturday.Text = additionalData.DescriptionAdditional;
                                            //}
                                            switch (schedule.Shift)
                                            {
                                                case "I":
                                                    btnSundayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnSundayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSundayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSundayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    btnSundayMeal.Text = rm.GetString("youOrdered", culture) + " " + type.Name;
                                                    shiftsByDate[day].SundayOrder = true;
                                                    break;
                                                case "II":
                                                    btnSundayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnSundayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSundayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSundayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    btnSundayMeal.Text = rm.GetString("youOrdered", culture) + " " + type.Name;
                                                    shiftsByDate[day].SundayOrder = true;
                                                    break;
                                                case "III":
                                                    btnSundayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    shiftsByDate[day].Current = schedule.Shift;
                                                    btnSundayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSaturdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSundayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                                                    btnSundayMeal.Text = rm.GetString("youOrdered", culture) + " " + type.Name;
                                                    shiftsByDate[day].SundayOrder = true;
                                                    break;
                                                default:
                                                    btnSundayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSaturdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSundayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    shiftsByDate[day].Current = "";
                                                    btnSundayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    btnSundayMeal.Text = rm.GetString("makeReservation", culture);
                                                    shiftsByDate[day].SundayOrder = false;
                                                    break;
                                            }
                                            break;

                                    }
                                }
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.checkOrders(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setButtonsColor()
        {
            btnMondayI.BackColor = btnMondayII.BackColor = btnMondayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
            btnTuesdayI.BackColor = btnTuesdayII.BackColor = btnTuesdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
            btnWednesdayI.BackColor = btnWednesdayII.BackColor = btnWednesdayIII.BackColor  = System.Drawing.ColorTranslator.FromHtml("#94c45c");
            btnThursdayI.BackColor = btnThursdayII.BackColor = btnThursdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
            btnFridayI.BackColor = btnFridayII.BackColor = btnFridayIII.BackColor =System.Drawing.ColorTranslator.FromHtml("#94c45c");
            btnSaturdayI.BackColor = btnSaturdayII.BackColor = btnSaturdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
            btnSundayI.BackColor = btnSundayII.BackColor = btnSundayIII.BackColor =  System.Drawing.ColorTranslator.FromHtml("#94c45c");
            btnSaturdayMeal.BackColor = btnSundayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
            btnSaturdayMeal.Text = btnSundayMeal.Text = rm.GetString("makeReservation", culture);
           
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {   
            MemoryStream ms = new MemoryStream();
            try
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.imageToByteArray(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return ms.ToArray();
        }

        public void setOrders(string day, int typeID,Image image) //must be public because it is called from MealPictureControl form
        {
            try
            {

                if (shiftsByDate[day].Current.Equals(""))
                {
                    lblError.Text = rm.GetString("shiftIsNotChosen", culture);
                }
                else
                {
                    lblError.Text = "";
                    MealTypeAdditionalDataTO additionalData = new MealTypeAdditionalData().GetAdditionalData(typeID);
                    MealTypeTO type = new MealType().GetMealType(typeID);
                    MealsEmployeeSchaduleTO order = new MealsEmployeeSchaduleTO();
                    DateTime date = dateOfTheDay[day];
                    DateTime from = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    DateTime to = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                    ArrayList listOfOrderedMeals = new MealsEmployeeSchedule().SearchForEmpl(from, to, currentEmpl.EmployeeID); //check ordered meals from monday 00:00 until friday 23:59

                    int result = 0;
                    bool updated = false;
                    bool exist = false;
                    if (isAdministrator == false)
                    {
                        if (listOfOrderedMeals.Count == 0) //there are no orders for that day, do the INSERT
                        {
                            result = new MealsEmployeeSchedule().Save(typeID, from, currentEmpl.EmployeeID, shiftsByDate[day].Current);
                        }
                        else //employee ordered before something, but he wants to change it, do the UPDATE
                        {
                            exist = true;
                            updated = new MealsEmployeeSchedule().Update(typeID, from, currentEmpl.EmployeeID, shiftsByDate[day].Current);
                        }
                    }
                    else
                    {
                        //so data could show
                        exist = true;
                        updated = true;
                    }

                    if ((exist == false && result > 0) || (exist == true && updated == true))
                    {
                        shiftsByDate[day].CurrentTypeID = typeID;
                        switch (day)
                        {
                            case "Monday":
                                pbOrderMonday.Image = image;
                                pbOrderMonday.Tag = typeID;
                                if (additionalData.DescriptionAdditional != null && !additionalData.DescriptionAdditional.Equals(""))
                                {
                                    lblMoMealName.Text = type.Name;
                                    tbOrderMonday.Text = additionalData.DescriptionAdditional;
                                }
                                break;
                            case "Tuesday":
                                pbOrderTuesday.Image = image;
                                pbOrderTuesday.Tag = typeID;
                                if (additionalData.DescriptionAdditional != null && !additionalData.DescriptionAdditional.Equals(""))
                                {
                                    lblTuMealName.Text = type.Name;
                                    tbOrderTuesday.Text = additionalData.DescriptionAdditional;
                                }
                                break;
                            case "Wednesday":
                                pbOrderWednesday.Image = image;
                                pbOrderWednesday.Tag = typeID;
                                if (additionalData.DescriptionAdditional != null && !additionalData.DescriptionAdditional.Equals(""))
                                {
                                    lblWeMealName.Text = type.Name;
                                    tbOrderWednesday.Text = additionalData.DescriptionAdditional;
                                }
                                break;
                            case "Thursday":
                                pbOrderThursday.Image = image;
                                pbOrderThursday.Tag = typeID;
                                if (additionalData.DescriptionAdditional != null && !additionalData.DescriptionAdditional.Equals(""))
                                {
                                    lblThMealName.Text = type.Name;
                                    tbOrderThursday.Text = additionalData.DescriptionAdditional;
                                }
                                break;
                            case "Friday":
                                pbOrderFriday.Image = image;
                                pbOrderFriday.Tag = typeID;
                                if (additionalData.DescriptionAdditional != null && !additionalData.DescriptionAdditional.Equals(""))
                                {
                                    lblFrMealName.Text = type.Name;
                                    tbOrderFriday.Text = additionalData.DescriptionAdditional;
                                }
                                break;
                            case "Saturday":
                                //pbOrderSaturday.Image = image;
                                //pbOrderSaturday.Tag = typeID;
                                if (additionalData.DescriptionAdditional != null && !additionalData.DescriptionAdditional.Equals(""))
                                {
                                    //lblSaMealName.Text = type.Name;
                                    //tbOrderSaturday.Text = additionalData.DescriptionAdditional;
                                }
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("ErrorSavingMeal", culture));
                    }
                }
            }
            
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.setOrders(): " + ex.Message + "\n");
                if (isAdministrator == false)
                {
                    MessageBox.Show(rm.GetString("ErrorSavingMeal", culture));
                }
            }        
        }

        private void CreateControls()
        {
            try
            {
                Control firstCtrl = null;

                foreach (Control ctrl in panelMoMeals.Controls)
                {
                    if (ctrl is MealPictureControl)
                    {
                        firstCtrl = ctrl;
                        break;
                    }
                }

                if (firstCtrl != null)
                    panelMoMeals.ScrollControlIntoView(firstCtrl);

                foreach (Control ctrl in panelTuMeals.Controls)
                {
                    if (ctrl is MealPictureControl)
                    {
                        firstCtrl = ctrl;
                        break;
                    }
                }

                if (firstCtrl != null)
                    panelTuMeals.ScrollControlIntoView(firstCtrl);

                foreach (Control ctrl in panelWeMeals.Controls)
                {
                    if (ctrl is MealPictureControl)
                    {
                        firstCtrl = ctrl;
                        break;
                    }
                }

                if (firstCtrl != null)
                    panelWeMeals.ScrollControlIntoView(firstCtrl);

                foreach (Control ctrl in panelThMeals.Controls)
                {
                    if (ctrl is MealPictureControl)
                    {
                        firstCtrl = ctrl;
                        break;
                    }
                }

                if (firstCtrl != null)
                    panelThMeals.ScrollControlIntoView(firstCtrl);

                foreach (Control ctrl in panelFrMeals.Controls)
                {
                    if (ctrl is MealPictureControl)
                    {
                        firstCtrl = ctrl;
                        break;
                    }
                }

                if (firstCtrl != null)
                    panelFrMeals.ScrollControlIntoView(firstCtrl);

                foreach (Control c in listControlsTypes)
                {
                    c.Dispose();
                }
                listControlsTypes.Clear();
                listControlsTypes = new List<Control>();
                GC.Collect();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.CreateControls(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void MealsOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                CreateControls();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.MealsOrder_FormClosing(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //TIMER code
            if (!idleTime.Equals("") && !idleTime.Equals("0"))
            {
                if (_IsRunning)
                {
                    applicationIdle.Stop();
                    _IsRunning = false;
                }
            }
            canSeeAndEditCurrent = false;
            canOnlyViewCurrent = false;
            this.Close();
        }

        private void btnThisWeek_Click(object sender, EventArgs e)
        {
            try
            {
                //TIMER code
                if (!idleTime.Equals("") && !idleTime.Equals("0"))
                {
                    if (_IsRunning)
                    {
                        applicationIdle.Stop();
                        _IsRunning = false;
                    }
                }
                canOnlyViewCurrent = false;
                canSeeAndEditCurrent = false;
                MealsOrderPIN pin = new MealsOrderPIN();
                pin.ShowDialog();
                if (canOnlyViewCurrent)
                {
                    canSeeAndEditCurrent = false;
                    setLanguage();
                    mealsByDay = new Dictionary<DayOfWeek, ArrayList>();
                    dateOfTheDay = new Dictionary<string, DateTime>();
                    setDates();
                    scheduleList = new MealsTypeSchedule().Search(-1, mondayDate, mondayDate.AddDays(6)); //od ponedeljka do subote, zato AddDays(5)
                    MealsOrder_Load(this, null); //applicationIdle.Start() se  nalazi u Load
                }
                else if (canSeeAndEditCurrent)
                {
                    canOnlyViewCurrent = false;
                    setLanguage();
                    mealsByDay = new Dictionary<DayOfWeek, ArrayList>();
                    dateOfTheDay = new Dictionary<string, DateTime>();
                    setDates();
                    scheduleList = new MealsTypeSchedule().Search(-1, mondayDate, mondayDate.AddDays(6)); //od ponedeljka do subote, zato AddDays(5)
                    MealsOrder_Load(this, null); //applicationIdle.Start() se  nalazi u Load
                }
                else
                {
                    //TIMER code
                    if (!idleTime.Equals("") && !idleTime.Equals("0"))
                    {
                        if (!_IsRunning)
                        {
                            applicationIdle.Start();
                            _IsRunning = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnThisWeek_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public static void canSeeAndEditCurrentWeek(bool canSeeAndEdit)
        {
            try
            {
                canSeeAndEditCurrent = canSeeAndEdit;
            }
            catch (Exception ex)
            {
            }
        }

        public static void canOnlySeeCurrentWeek(bool onlyView)
        {
            try
            {
                canOnlyViewCurrent = onlyView;
            }
            catch
            {
            }
        }

        private void SetIdleTimer()
        {
            try
            {
                idleTime = Constants.IdleTimeKeteringPrica;
                if (!idleTime.Equals("") && !idleTime.Equals("0"))
                {
                    int time = Convert.ToInt32(idleTime);
                    TimeSpan timeSpan = new TimeSpan(0, 0, time);
                    applicationIdle.IdleTime = timeSpan;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.setIdleTimer(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public void StartIdleTimer()
        {
            try
            {
                if (!_IsRunning)
                {
                    applicationIdle.Start();
                    _IsRunning = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.StartIdleTimer(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void applicationIdle_IdleAsync(object sender, EventArgs e)
        {
            try
            {
                BeginInvoke(new MethodInvoker(
                    delegate() { applicationIdle_Idle(sender, e); })
                    );
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.applicationIdle_IdleAsync(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void applicationIdle_Idle(object sender, EventArgs e)
        {
            try
            {
                if (applicationIdle != null)
                {
                    if (_IsRunning)
                    {
                        applicationIdle.Stop();
                        _IsRunning = false;
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.applicationIdle_Idle(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        #region Delete meal order on double click
        private void pbOrderMonday_DoubleClick(object sender, EventArgs e)
        {
            bool load = false;
            try
            {
                if (pbOrderMonday.Image != null)
                {
                    load = true;
                    int typeID = Convert.ToInt32(pbOrderMonday.Tag);
                    string day = DayOfWeek.Monday.ToString();
                    DateTime date = dateOfTheDay[day];
                    int emplID = currentEmpl.EmployeeID;
                    bool isDeleted = false;

                    if (!isAdministrator)
                    {
                        isDeleted = new MealsEmployeeSchedule().Delete(typeID, date, emplID);
                    }
                    else
                    {
                        isDeleted = true;
                    }
                    if (isDeleted)
                    {
                        pbOrderMonday.Image = null;
                        lblMoMealName.Text = "";
                        tbOrderMonday.Text = "";
                        shiftsByDate[day].CurrentTypeID = 0;
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("mealCouldNotBeDeleted", culture));
                    }
                }
                else
                    load = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.pbOrderMonday_DoubleClick(): " + ex.Message + "\n");
               // MessageBox.Show(ex.Message);
                MessageBox.Show(rm.GetString("mealCouldNotBeDeleted", culture));
            }
            finally
            {
                if (load)
                {
                    setLanguage();
                    mealsByDay = new Dictionary<DayOfWeek, ArrayList>();
                    dateOfTheDay = new Dictionary<string, DateTime>();
                    setDates();
                    scheduleList = new MealsTypeSchedule().Search(-1, mondayDate, mondayDate.AddDays(5)); //od ponedeljka do subote, zato AddDays(5)
                    MealsOrder_Load(this, null); //applicationIdle.Start() se  nalazi u Load
                }
            }
        }

        private void pbOrderTuesday_DoubleClick(object sender, EventArgs e)
        {
            bool load = false;
            try
            {
                if (pbOrderTuesday.Image != null)
                {
                    load = true;
                    int typeID = Convert.ToInt32(pbOrderTuesday.Tag);
                    string day = DayOfWeek.Tuesday.ToString();
                    DateTime date = dateOfTheDay[day];
                    int emplID = currentEmpl.EmployeeID;
                    bool isDeleted = false;

                    if (!isAdministrator)
                    {
                        isDeleted = new MealsEmployeeSchedule().Delete(typeID, date, emplID);
                    }
                    else
                    {
                        isDeleted = true;
                    }
                    if (isDeleted)
                    {
                        pbOrderTuesday.Image = null;
                        lblTuMealName.Text = "";
                        tbOrderTuesday.Text = "";
                        shiftsByDate[day].CurrentTypeID = 0;
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("mealCouldNotBeDeleted", culture));
                    }
                }
                else
                    load = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.pbOrderTuesday_DoubleClick(): " + ex.Message + "\n");
                // MessageBox.Show(ex.Message);
                MessageBox.Show(rm.GetString("mealCouldNotBeDeleted", culture));
            }
            finally
            {
                if (load)
                {
                    setLanguage();
                    mealsByDay = new Dictionary<DayOfWeek, ArrayList>();
                    dateOfTheDay = new Dictionary<string, DateTime>();
                    setDates();
                    scheduleList = new MealsTypeSchedule().Search(-1, mondayDate, mondayDate.AddDays(5)); //od ponedeljka do subote, zato AddDays(5)
                    MealsOrder_Load(this, null); //applicationIdle.Start() se  nalazi u Load
                }
            }
        }

        private void pbOrderWednesday_DoubleClick(object sender, EventArgs e)
        {
            bool load = false;

            try
            {
                if (pbOrderWednesday.Image != null)
                {
                    load = true;
                    int typeID = Convert.ToInt32(pbOrderWednesday.Tag);
                    string day = DayOfWeek.Wednesday.ToString();
                    DateTime date = dateOfTheDay[day];
                    int emplID = currentEmpl.EmployeeID;
                    bool isDeleted = false;

                    if (!isAdministrator)
                    {
                        isDeleted = new MealsEmployeeSchedule().Delete(typeID, date, emplID);
                    }
                    else
                    {
                        isDeleted = true;
                    }
                    if (isDeleted)
                    {
                        pbOrderWednesday.Image = null;
                        lblWeMealName.Text = "";
                        tbOrderWednesday.Text = "";
                        shiftsByDate[day].CurrentTypeID = 0;
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("mealCouldNotBeDeleted", culture));
                    }

                }
                else
                    load = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.pbOrderWednesday_DoubleClick(): " + ex.Message + "\n");
                // MessageBox.Show(ex.Message);
                MessageBox.Show(rm.GetString("mealCouldNotBeDeleted", culture));
            }
            finally
            {
                if (load)
                {
                    setLanguage();
                    mealsByDay = new Dictionary<DayOfWeek, ArrayList>();
                    dateOfTheDay = new Dictionary<string, DateTime>();
                    setDates();
                    scheduleList = new MealsTypeSchedule().Search(-1, mondayDate, mondayDate.AddDays(5)); //od ponedeljka do subote, zato AddDays(5)
                    MealsOrder_Load(this, null); //applicationIdle.Start() se  nalazi u Load
                }
            }
        }

        private void pbOrderThursday_DoubleClick(object sender, EventArgs e)
        {
            bool load = false;

            try
            {
                if (pbOrderThursday.Image != null)
                {
                    load = true;
                    int typeID = Convert.ToInt32(pbOrderThursday.Tag);
                    string day = DayOfWeek.Thursday.ToString();
                    DateTime date = dateOfTheDay[day];
                    int emplID = currentEmpl.EmployeeID;
                    bool isDeleted = false;

                    if (!isAdministrator)
                    {
                        isDeleted = new MealsEmployeeSchedule().Delete(typeID, date, emplID);
                    }
                    else
                    {
                        isDeleted = true;
                    }

                    if (isDeleted)
                    {
                        pbOrderThursday.Image = null;
                        lblThMealName.Text = "";
                        tbOrderThursday.Text = "";
                        shiftsByDate[day].CurrentTypeID = 0;
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("mealCouldNotBeDeleted", culture));
                    }
                }
                else
                    load = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.pbOrderTuesday_DoubleClick(): " + ex.Message + "\n");
                // MessageBox.Show(ex.Message);
                MessageBox.Show(rm.GetString("mealCouldNotBeDeleted", culture));
            }
            finally
            {
                if (load)
                {
                    setLanguage();
                    mealsByDay = new Dictionary<DayOfWeek, ArrayList>();
                    dateOfTheDay = new Dictionary<string, DateTime>();
                    setDates();
                    scheduleList = new MealsTypeSchedule().Search(-1, mondayDate, mondayDate.AddDays(5)); //od ponedeljka do subote, zato AddDays(5)
                    MealsOrder_Load(this, null); //applicationIdle.Start() se  nalazi u Load
                }
            }

        }

        private void pbOrderFriday_DoubleClick(object sender, EventArgs e)
        {
            bool load = false;
            try
            {
                if (pbOrderFriday.Image != null)
                {
                    load = true;
                    int typeID = Convert.ToInt32(pbOrderFriday.Tag);
                    string day = DayOfWeek.Friday.ToString();
                    DateTime date = dateOfTheDay[day];
                    int emplID = currentEmpl.EmployeeID;
                    bool isDeleted = false;

                    if (!isAdministrator)
                    {
                        isDeleted = new MealsEmployeeSchedule().Delete(typeID, date, emplID);
                    }
                    else
                    {
                        isDeleted = true;
                    }
                    if (isDeleted)
                    {
                        pbOrderFriday.Image = null;
                        lblFrMealName.Text = "";
                        tbOrderFriday.Text = "";
                        shiftsByDate[day].CurrentTypeID = 0;
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("mealCouldNotBeDeleted", culture));
                    }
                }
                else
                    load = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.pbOrderFriday_DoubleClick(): " + ex.Message + "\n");
                // MessageBox.Show(ex.Message);
                MessageBox.Show(rm.GetString("mealCouldNotBeDeleted", culture));
            }
            finally
            {
                if (load)
                {
                    setLanguage();
                    mealsByDay = new Dictionary<DayOfWeek, ArrayList>();
                    dateOfTheDay = new Dictionary<string, DateTime>();
                    setDates();
                    scheduleList = new MealsTypeSchedule().Search(-1, mondayDate, mondayDate.AddDays(5)); //od ponedeljka do subote, zato AddDays(5)
                    MealsOrder_Load(this, null); //applicationIdle.Start() se  nalazi u Load
                }
            }

        }

        private void pbOrderSaturday_DoubleClick(object sender, EventArgs e)
        {
            bool load = false;
            try
            {
                //if (pbOrderSaturday.Image != null && isAdministrator == false)
                //{
                //    load = true;
                //    int typeID = Convert.ToInt32(pbOrderSaturday.Tag);
                //    string day = DayOfWeek.Saturday.ToString();
                //    DateTime date = dateOfTheDay[day];
                //    int emplID = currentEmpl.EmployeeID;
                //    bool isDeleted = false;

                //    if (!isAdministrator)
                //    {
                //        isDeleted = new MealsEmployeeSchedule().Delete(typeID, date, emplID);
                //    }
                //    else
                //    {
                //        isDeleted = true;
                //    }
                //    if (isDeleted)
                //    {
                //        pbOrderSaturday.Image = null;
                //        lblSaMealName.Text = "";
                //        tbOrderSaturday.Text = "";
                //    }
                //    else
                //    {
                //        MessageBox.Show(rm.GetString("mealCouldNotBeDeleted", culture));
                //    }
                //}
                //else
                //    load = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.pbOrderSaturday_DoubleClick(): " + ex.Message + "\n");
                // MessageBox.Show(ex.Message);
                MessageBox.Show(rm.GetString("mealCouldNotBeDeleted", culture));
            }
            finally
            {
                if (load)
                {
                    setLanguage();
                    mealsByDay = new Dictionary<DayOfWeek, ArrayList>();
                    dateOfTheDay = new Dictionary<string, DateTime>();
                    setDates();
                    scheduleList = new MealsTypeSchedule().Search(-1, mondayDate, mondayDate.AddDays(5)); //od ponedeljka do subote, zato AddDays(5)
                    MealsOrder_Load(this, null); //applicationIdle.Start() se  nalazi u Load
                }
            }
        }
        #endregion

        #region Monday shifts
        private void btnMondayI_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnMondayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnMondayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnMondayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");

                shiftsByDate[DayOfWeek.Monday.ToString()].Current = new RestaurantShiftsTO().First;

                int currentType = shiftsByDate[DayOfWeek.Monday.ToString()].CurrentTypeID;
                if (currentType != 0)
                {
                    updateOrder(DayOfWeek.Monday.ToString());
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnMondayI_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }
        }

        private void btnMondayII_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnMondayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnMondayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnMondayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                
                shiftsByDate[DayOfWeek.Monday.ToString()].Current = new RestaurantShiftsTO().Second;

                int currentType = shiftsByDate[DayOfWeek.Monday.ToString()].CurrentTypeID;
                if (currentType != 0)
                {
                    updateOrder(DayOfWeek.Monday.ToString());
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnMondayII_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }
        }

        private void btnMondayIII_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnMondayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnMondayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnMondayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");

                shiftsByDate[DayOfWeek.Monday.ToString()].Current = new RestaurantShiftsTO().Third;
                
                int currentType = shiftsByDate[DayOfWeek.Monday.ToString()].CurrentTypeID;
                if (currentType != 0)
                {
                    updateOrder(DayOfWeek.Monday.ToString());
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnMondayIII_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }
        }
        #endregion

        #region Tuesday shifts
        private void btnTuesdayI_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnTuesdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnTuesdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnTuesdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");

                shiftsByDate[DayOfWeek.Tuesday.ToString()].Current = new RestaurantShiftsTO().First;

                int currentType = shiftsByDate[DayOfWeek.Tuesday.ToString()].CurrentTypeID;
                if (currentType != 0)
                {
                    updateOrder(DayOfWeek.Tuesday.ToString());
                }          
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnTuesdayI_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }
        }

        private void btnTuesdayII_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnTuesdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnTuesdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnTuesdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                
                shiftsByDate[DayOfWeek.Tuesday.ToString()].Current = new RestaurantShiftsTO().Second;

                int currentType = shiftsByDate[DayOfWeek.Tuesday.ToString()].CurrentTypeID;
                if (currentType != 0)
                {
                    updateOrder(DayOfWeek.Tuesday.ToString());
                }     
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnTuesdayII_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }

        }
        
        private void btnTuesdayIII_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnTuesdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnTuesdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnTuesdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                               
                shiftsByDate[DayOfWeek.Tuesday.ToString()].Current = new RestaurantShiftsTO().Third;

                int currentType = shiftsByDate[DayOfWeek.Tuesday.ToString()].CurrentTypeID;
                if (currentType != 0)
                {
                    updateOrder(DayOfWeek.Tuesday.ToString());
                }     
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnTuesdayIII_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }

        }
        #endregion

        #region Wednesday shifts
        private void btnWednesdayI_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnWednesdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnWednesdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnWednesdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");       
      
                shiftsByDate[DayOfWeek.Wednesday.ToString()].Current = new RestaurantShiftsTO().First;

                int currentType = shiftsByDate[DayOfWeek.Wednesday.ToString()].CurrentTypeID;
                if (currentType != 0)
                {
                    updateOrder(DayOfWeek.Wednesday.ToString());
                }     
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnWednesdayI_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }

        }

        private void btnWednesdayII_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnWednesdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnWednesdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnWednesdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");                                                   
                
                shiftsByDate[DayOfWeek.Wednesday.ToString()].Current = new RestaurantShiftsTO().Second;

                int currentType = shiftsByDate[DayOfWeek.Wednesday.ToString()].CurrentTypeID;
                if (currentType != 0)
                {
                    //update shift!
                    string currentShift = shiftsByDate[DayOfWeek.Wednesday.ToString()].Current;
                    if (!currentShift.Equals(""))
                    {
                        updateOrder(DayOfWeek.Wednesday.ToString());
                    }
                } 
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnWednesdayII_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }

        }

        private void btnWednesdayIII_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnWednesdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnWednesdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnWednesdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                
                shiftsByDate[DayOfWeek.Wednesday.ToString()].Current = new RestaurantShiftsTO().Third;

                int currentType = shiftsByDate[DayOfWeek.Wednesday.ToString()].CurrentTypeID;
                if (currentType != 0)
                {
                    updateOrder(DayOfWeek.Wednesday.ToString());
                } 
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnWednesdayIII_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }

        }
        #endregion

        #region Thursday shifts
        private void btnThursdayI_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnThursdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnThursdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnThursdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                
                shiftsByDate[DayOfWeek.Thursday.ToString()].Current = new RestaurantShiftsTO().First;

                int currentType = shiftsByDate[DayOfWeek.Thursday.ToString()].CurrentTypeID;
                if (currentType != 0)
                {
                    //update shift!
                    string currentShift = shiftsByDate[DayOfWeek.Thursday.ToString()].Current;
                    if (!currentShift.Equals(""))
                    {
                        updateOrder(DayOfWeek.Thursday.ToString());
                    }
                } 
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnThursdayI_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }

        }

        private void btnThursdayII_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnThursdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnThursdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnThursdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                
                shiftsByDate[DayOfWeek.Thursday.ToString()].Current = new RestaurantShiftsTO().Second;

                int currentType = shiftsByDate[DayOfWeek.Thursday.ToString()].CurrentTypeID;
                if (currentType != 0)
                {
                    updateOrder(DayOfWeek.Thursday.ToString());
                } 
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnThursdayII_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }

        }

        private void btnThursdayIII_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnThursdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnThursdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnThursdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                
                shiftsByDate[DayOfWeek.Thursday.ToString()].Current = new RestaurantShiftsTO().Third;

                int currentType = shiftsByDate[DayOfWeek.Thursday.ToString()].CurrentTypeID;
                if (currentType != 0)
                {
                    updateOrder(DayOfWeek.Thursday.ToString());
                } 
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnThursdayIII_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }

        }
        #endregion

        #region Friday shifts
        private void btnFridayI_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnFridayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnFridayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnFridayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");                                                   
                
                shiftsByDate[DayOfWeek.Friday.ToString()].Current = new RestaurantShiftsTO().First;

                int currentType = shiftsByDate[DayOfWeek.Friday.ToString()].CurrentTypeID;
                if (currentType != 0)
                {
                    updateOrder(DayOfWeek.Friday.ToString());
                } 
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnFridayI_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }

        }

        private void btnFridayII_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnFridayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnFridayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnFridayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                
                shiftsByDate[DayOfWeek.Friday.ToString()].Current = new RestaurantShiftsTO().Second;
                
                int currentType = shiftsByDate[DayOfWeek.Friday.ToString()].CurrentTypeID;
                if (currentType != 0)
                {
                    updateOrder(DayOfWeek.Friday.ToString());
                } 
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnFridayII_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }

        }

        private void btnFridayIII_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnFridayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnFridayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnFridayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                 
                shiftsByDate[DayOfWeek.Friday.ToString()].Current = new RestaurantShiftsTO().Third;
                
                int currentType = shiftsByDate[DayOfWeek.Friday.ToString()].CurrentTypeID;
                if (currentType != 0)
                {
                    updateOrder(DayOfWeek.Friday.ToString());
                } 
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnFridayIII_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }

        }
        #endregion

        #region Saturday shifts and meal
        private void btnSaturdayI_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnSaturdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnSaturdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnSaturdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                    
                shiftsByDate[DayOfWeek.Saturday.ToString()].Current = new RestaurantShiftsTO().First;
                if (shiftsByDate[DayOfWeek.Saturday.ToString()].SaturdayOrder == true)
                {
                    string currentShift = shiftsByDate[DayOfWeek.Saturday.ToString()].Current;
                    if (!currentShift.Equals(""))
                    {
                        //saveMeal
                        lblError.Text = "";

                        MealTypeTO type = new MealType().GetMealType(Constants.saturdayMealType);
                        int result = 0;
                        bool updated = false;
                        bool exist = false;
                        if (isAdministrator == false)
                        {
                            DateTime date = dateOfTheDay[DayOfWeek.Saturday.ToString()];
                            DateTime from = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                            DateTime to = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                            ArrayList listOfOrderedMeals = new MealsEmployeeSchedule().SearchForEmpl(from, to, currentEmpl.EmployeeID); //check ordered meals from monday 00:00 until friday 23:59

                            if (listOfOrderedMeals.Count > 0) //order exists, do the update
                            {
                                exist = true;
                                updated = new MealsEmployeeSchedule().Update(Constants.saturdayMealType, from, currentEmpl.EmployeeID, shiftsByDate[DayOfWeek.Saturday.ToString()].Current);
                            }
                        }
                        else
                        {
                            //so data could show
                            exist = true;
                            updated = true;
                        }
                        if ((exist == false && result > 0) || (exist == true && updated == true))
                        {
                            shiftsByDate[DayOfWeek.Saturday.ToString()].SaturdayOrder = true;
                            btnSaturdayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                            btnSaturdayMeal.Text = rm.GetString("youOrdered", culture) + " " + type.Name;
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("ErrorUpdatingMeal", culture));
                        }

                    }
                    else
                    {
                        lblError.Text = rm.GetString("shiftIsNotChosen", culture);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnSaturdayI_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }

        }

        private void btnSaturdayII_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnSaturdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnSaturdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnSaturdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                                  
                shiftsByDate[DayOfWeek.Saturday.ToString()].Current = new RestaurantShiftsTO().Second;
                if (shiftsByDate[DayOfWeek.Saturday.ToString()].SaturdayOrder == true)
                {
                    string currentShift = shiftsByDate[DayOfWeek.Saturday.ToString()].Current;
                    if (!currentShift.Equals(""))
                    {
                        //saveMeal
                        lblError.Text = "";

                        MealTypeTO type = new MealType().GetMealType(Constants.saturdayMealType);
                        int result = 0;
                        bool updated = false;
                        bool exist = false;
                        if (isAdministrator == false)
                        {
                            DateTime date = dateOfTheDay[DayOfWeek.Saturday.ToString()];
                            DateTime from = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                            DateTime to = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                            ArrayList listOfOrderedMeals = new MealsEmployeeSchedule().SearchForEmpl(from, to, currentEmpl.EmployeeID); //check ordered meals from monday 00:00 until friday 23:59

                            if (listOfOrderedMeals.Count > 0) //order exists, do the update
                            {
                                exist = true;
                                updated = new MealsEmployeeSchedule().Update(Constants.saturdayMealType, from, currentEmpl.EmployeeID, shiftsByDate[DayOfWeek.Saturday.ToString()].Current);
                            }
                        }
                        else
                        {
                            //so data could show
                            exist = true;
                            updated = true;
                        }
                        if ((exist == false && result > 0) || (exist == true && updated == true))
                        {
                            shiftsByDate[DayOfWeek.Saturday.ToString()].SaturdayOrder = true;
                            btnSaturdayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                            btnSaturdayMeal.Text = rm.GetString("youOrdered", culture) + " " + type.Name;
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("ErrorUpdatingMeal", culture));
                        }

                    }
                    else
                    {
                        lblError.Text = rm.GetString("shiftIsNotChosen", culture);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnSaturdayII_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }

        }

        private void btnSaturdayIII_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnSaturdayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnSaturdayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnSaturdayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");

                shiftsByDate[DayOfWeek.Saturday.ToString()].Current = new RestaurantShiftsTO().Third;
                if (shiftsByDate[DayOfWeek.Saturday.ToString()].SaturdayOrder == true)
                {
                    string currentShift = shiftsByDate[DayOfWeek.Saturday.ToString()].Current;
                    if (!currentShift.Equals(""))
                    {
                        //saveMeal
                        lblError.Text = "";
                        MealTypeTO type = new MealType().GetMealType(Constants.saturdayMealType);
                        int result = 0;
                        bool updated = false;
                        bool exist = false;
                        if (isAdministrator == false)
                        {
                            DateTime date = dateOfTheDay[DayOfWeek.Saturday.ToString()];
                            DateTime from = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                            DateTime to = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                            ArrayList listOfOrderedMeals = new MealsEmployeeSchedule().SearchForEmpl(from, to, currentEmpl.EmployeeID); //check ordered meals from monday 00:00 until friday 23:59

                            if (listOfOrderedMeals.Count > 0) //order exists, do the update
                            {
                                exist = true;
                                updated = new MealsEmployeeSchedule().Update(Constants.saturdayMealType, from, currentEmpl.EmployeeID, shiftsByDate[DayOfWeek.Saturday.ToString()].Current);
                            }
                        }
                        else
                        {
                            //so data could show
                            exist = true;
                            updated = true;
                        }
                        if ((exist == false && result > 0) || (exist == true && updated == true))
                        {
                            shiftsByDate[DayOfWeek.Saturday.ToString()].SaturdayOrder = true;
                            btnSaturdayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                            btnSaturdayMeal.Text = rm.GetString("youOrdered", culture) + " " + type.Name;
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("ErrorUpdatingMeal", culture));
                        }

                    }
                    else
                    {
                        lblError.Text = rm.GetString("shiftIsNotChosen", culture);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnSaturdayIII_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }

        }

        private void btnSaturdayMeal_Click(object sender, EventArgs e)
        {
            try
            {
                bool mealIsOrded = shiftsByDate[DayOfWeek.Saturday.ToString()].SaturdayOrder;
                if (mealIsOrded)
                {
                    //delete meal :D
                    bool load = false;
                    try
                    {
                        if (isAdministrator == false)
                        {
                            load = true;
                            int typeID = Constants.saturdayMealType;
                            string day = DayOfWeek.Saturday.ToString();
                            DateTime date = dateOfTheDay[day];
                            int emplID = currentEmpl.EmployeeID;
                            bool isDeleted = false;

                            if (!isAdministrator)
                            {
                                isDeleted = new MealsEmployeeSchedule().Delete(typeID, date, emplID);
                            }
                            else
                            {
                                isDeleted = true;
                            }
                            if (isDeleted)
                            {
                                btnSaturdayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                btnSaturdayMeal.Text = rm.GetString("makeReservation", culture);
                                shiftsByDate[DayOfWeek.Saturday.ToString()].SaturdayOrder = false;
                            }
                            else
                            {
                                MessageBox.Show(rm.GetString("mealCouldNotBeDeleted", culture));
                            }
                        }
                        else
                        {
                            btnSaturdayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                            btnSaturdayMeal.Text = rm.GetString("makeReservation", culture);
                            shiftsByDate[DayOfWeek.Saturday.ToString()].SaturdayOrder = false;
                            load = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.writeLog(DateTime.Now + " MealsOrder.btnSaturdayMeal_Click(): " + ex.Message + "\n");
                        // MessageBox.Show(ex.Message);
                        MessageBox.Show(rm.GetString("mealCouldNotBeDeleted", culture));
                    }
                    finally
                    {
                        if (load)
                        {
                            setLanguage();
                            mealsByDay = new Dictionary<DayOfWeek, ArrayList>();
                            dateOfTheDay = new Dictionary<string, DateTime>();
                            setDates();
                            scheduleList = new MealsTypeSchedule().Search(-1, mondayDate, mondayDate.AddDays(6)); //od ponedeljka do subote, zato AddDays(5)
                            MealsOrder_Load(this, null); //applicationIdle.Start() se  nalazi u Load
                        }
                    }
                }
                else
                {
                    string currentShift = shiftsByDate[DayOfWeek.Saturday.ToString()].Current;
                    if (!currentShift.Equals(""))
                    {
                        //saveMeal
                        lblError.Text = "";

                        MealTypeTO type = new MealType().GetMealType(Constants.saturdayMealType);
                        int result = 0;
                        bool updated = false;
                        bool exist = false;
                        if (isAdministrator == false)
                        {
                            DateTime date = dateOfTheDay[DayOfWeek.Saturday.ToString()];
                            DateTime from = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                            DateTime to = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                            ArrayList listOfOrderedMeals = new MealsEmployeeSchedule().SearchForEmpl(from, to, currentEmpl.EmployeeID); //check ordered meals from monday 00:00 until friday 23:59

                            if (listOfOrderedMeals.Count == 0) //there are no orders for that day, do the INSERT
                            {
                                result = new MealsEmployeeSchedule().Save(Constants.saturdayMealType, from, currentEmpl.EmployeeID, shiftsByDate[DayOfWeek.Saturday.ToString()].Current);
                            }
                            else //employee ordered before something, but he wants to change it, do the UPDATE
                            {
                                exist = true;
                                updated = new MealsEmployeeSchedule().Update(Constants.saturdayMealType, from, currentEmpl.EmployeeID, shiftsByDate[DayOfWeek.Saturday.ToString()].Current);
                            }
                        }
                        else
                        {
                            //so data could show
                            exist = true;
                            updated = true;
                        }
                        if ((exist == false && result > 0) || (exist == true && updated == true))
                        {
                            shiftsByDate[DayOfWeek.Saturday.ToString()].SaturdayOrder = true;
                            btnSaturdayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                            btnSaturdayMeal.Text = rm.GetString("youOrdered", culture) + " " + type.Name;
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("ErrorSavingMeal", culture));
                        }

                    }
                    else
                    {
                        lblError.Text = rm.GetString("shiftIsNotChosen", culture);
                    }
                }
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnSaturdayMeal_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }
        }
        #endregion

        #region Sunday and meal
        private void btnSundayI_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnSundayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnSundayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnSundayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");

                shiftsByDate[DayOfWeek.Sunday.ToString()].Current = new RestaurantShiftsTO().First;
                if (shiftsByDate[DayOfWeek.Sunday.ToString()].SundayOrder == true)
                {
                    string currentShift = shiftsByDate[DayOfWeek.Sunday.ToString()].Current;
                    if (!currentShift.Equals(""))
                    {
                        //saveMeal
                        lblError.Text = "";
                        MealTypeTO type = new MealType().GetMealType(Constants.sundayMealType);
                        int result = 0;
                        bool updated = false;
                        bool exist = false;
                        if (isAdministrator == false)
                        {
                            DateTime date = dateOfTheDay[DayOfWeek.Sunday.ToString()];
                            DateTime from = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                            DateTime to = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                            ArrayList listOfOrderedMeals = new MealsEmployeeSchedule().SearchForEmpl(from, to, currentEmpl.EmployeeID); //check ordered meals from monday 00:00 until friday 23:59

                            if (listOfOrderedMeals.Count > 0) //order exists, do the update
                            {
                                exist = true;
                                updated = new MealsEmployeeSchedule().Update(Constants.sundayMealType, from, currentEmpl.EmployeeID, shiftsByDate[DayOfWeek.Sunday.ToString()].Current);
                            }
                        }
                        else
                        {
                            //so data could show
                            exist = true;
                            updated = true;
                        }
                        if ((exist == false && result > 0) || (exist == true && updated == true))
                        {
                            shiftsByDate[DayOfWeek.Sunday.ToString()].SundayOrder = true;
                            btnSundayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                            btnSundayMeal.Text = rm.GetString("youOrdered", culture) + " " + type.Name;
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("ErrorUpdatingMeal", culture));
                        }

                    }
                    else
                    {
                        lblError.Text = rm.GetString("shiftIsNotChosen", culture);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnSundayI_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }
        }

        private void btnSundayII_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnSundayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnSundayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnSundayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");

                shiftsByDate[DayOfWeek.Sunday.ToString()].Current = new RestaurantShiftsTO().Second;
                if (shiftsByDate[DayOfWeek.Sunday.ToString()].SundayOrder == true)
                {
                    string currentShift = shiftsByDate[DayOfWeek.Sunday.ToString()].Current;
                    if (!currentShift.Equals(""))
                    {
                        //saveMeal
                        lblError.Text = "";
                        MealTypeTO type = new MealType().GetMealType(Constants.sundayMealType);
                        int result = 0;
                        bool updated = false;
                        bool exist = false;
                        if (isAdministrator == false)
                        {
                            DateTime date = dateOfTheDay[DayOfWeek.Sunday.ToString()];
                            DateTime from = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                            DateTime to = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                            ArrayList listOfOrderedMeals = new MealsEmployeeSchedule().SearchForEmpl(from, to, currentEmpl.EmployeeID); //check ordered meals from monday 00:00 until friday 23:59

                            if (listOfOrderedMeals.Count > 0) //order exists, do the update
                            {
                                exist = true;
                                updated = new MealsEmployeeSchedule().Update(Constants.sundayMealType, from, currentEmpl.EmployeeID, shiftsByDate[DayOfWeek.Sunday.ToString()].Current);
                            }
                        }
                        else
                        {
                            //so data could show
                            exist = true;
                            updated = true;
                        }
                        if ((exist == false && result > 0) || (exist == true && updated == true))
                        {
                            shiftsByDate[DayOfWeek.Sunday.ToString()].SundayOrder = true;
                            btnSundayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                            btnSundayMeal.Text = rm.GetString("youOrdered", culture) + " " + type.Name;
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("ErrorUpdatingMeal", culture));
                        }

                    }
                    else
                    {
                        lblError.Text = rm.GetString("shiftIsNotChosen", culture);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnSundayII_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }
        }

        private void btnSundayIII_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                btnSundayIII.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                btnSundayI.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                btnSundayII.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");

                shiftsByDate[DayOfWeek.Sunday.ToString()].Current = new RestaurantShiftsTO().Third;

                if (shiftsByDate[DayOfWeek.Sunday.ToString()].SundayOrder == true)
                {
                    string currentShift = shiftsByDate[DayOfWeek.Sunday.ToString()].Current;
                    if (!currentShift.Equals(""))
                    {
                        //saveMeal
                        lblError.Text = "";
                        MealTypeTO type = new MealType().GetMealType(Constants.sundayMealType);
                        int result = 0;
                        bool updated = false;
                        bool exist = false;
                        if (isAdministrator == false)
                        {
                            DateTime date = dateOfTheDay[DayOfWeek.Sunday.ToString()];
                            DateTime from = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                            DateTime to = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                            ArrayList listOfOrderedMeals = new MealsEmployeeSchedule().SearchForEmpl(from, to, currentEmpl.EmployeeID); //check ordered meals from monday 00:00 until friday 23:59

                            if (listOfOrderedMeals.Count > 0) //order exists, do the update
                            {
                                exist = true;
                                updated = new MealsEmployeeSchedule().Update(Constants.sundayMealType, from, currentEmpl.EmployeeID, shiftsByDate[DayOfWeek.Sunday.ToString()].Current);
                            }
                        }
                        else
                        {
                            //so data could show
                            exist = true;
                            updated = true;
                        }
                        if ((exist == false && result > 0) || (exist == true && updated == true))
                        {
                            shiftsByDate[DayOfWeek.Sunday.ToString()].SundayOrder = true;
                            btnSundayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                            btnSundayMeal.Text = rm.GetString("youOrdered", culture) + " " + type.Name;
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("ErrorUpdatingMeal", culture));
                        }

                    }
                    else
                    {
                        lblError.Text = rm.GetString("shiftIsNotChosen", culture);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnSundayIII_Click(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }
        }

        private void btnSundayMeal_Click(object sender, EventArgs e)
        {
            try
            {
                bool mealIsOrded = shiftsByDate[DayOfWeek.Sunday.ToString()].SundayOrder;
                if (mealIsOrded)
                {
                    //delete meal :D
                    bool load = false;
                    try
                    {
                        if (isAdministrator == false)
                        {
                            load = true;
                            int typeID = Constants.sundayMealType;
                            string day = DayOfWeek.Sunday.ToString();
                            DateTime date = dateOfTheDay[day];
                            int emplID = currentEmpl.EmployeeID;
                            bool isDeleted = false;

                            if (!isAdministrator)
                            {
                                isDeleted = new MealsEmployeeSchedule().Delete(typeID, date, emplID);
                            }
                            else
                            {
                                isDeleted = true;
                            }
                            if (isDeleted)
                            {
                                btnSundayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                                btnSundayMeal.Text = rm.GetString("makeReservation", culture);
                                shiftsByDate[DayOfWeek.Sunday.ToString()].SundayOrder = false;
                            }
                            else
                            {
                                MessageBox.Show(rm.GetString("mealCouldNotBeDeleted", culture));
                            }
                        }
                        else
                        {
                            btnSundayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#94c45c");
                            btnSundayMeal.Text = rm.GetString("makeReservation", culture);
                            shiftsByDate[DayOfWeek.Sunday.ToString()].SundayOrder = false;
                            load = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.writeLog(DateTime.Now + " MealsOrder.btnSundayMeal_Click(): " + ex.Message + "\n");
                        MessageBox.Show(rm.GetString("mealCouldNotBeDeleted", culture));
                    }
                    finally
                    {
                        if (load)
                        {
                            setLanguage();
                            mealsByDay = new Dictionary<DayOfWeek, ArrayList>();
                            dateOfTheDay = new Dictionary<string, DateTime>();
                            setDates();
                            scheduleList = new MealsTypeSchedule().Search(-1, mondayDate, mondayDate.AddDays(6)); //od ponedeljka do subote, zato AddDays(5)
                            MealsOrder_Load(this, null); //applicationIdle.Start() se  nalazi u Load
                        }
                    }
                }
                else
                {
                    string currentShift = shiftsByDate[DayOfWeek.Sunday.ToString()].Current;
                    if (!currentShift.Equals(""))
                    {
                        //saveMeal
                        lblError.Text = "";

                        MealTypeTO type = new MealType().GetMealType(Constants.sundayMealType);
                        int result = 0;
                        bool updated = false;
                        bool exist = false;
                        if (isAdministrator == false)
                        {

                            DateTime date = dateOfTheDay[DayOfWeek.Sunday.ToString()];
                            DateTime from = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                            DateTime to = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                            ArrayList listOfOrderedMeals = new MealsEmployeeSchedule().SearchForEmpl(from, to, currentEmpl.EmployeeID); //check ordered meals from monday 00:00 until friday 23:59

                            if (listOfOrderedMeals.Count == 0) //there are no orders for that day, do the INSERT
                            {
                                result = new MealsEmployeeSchedule().Save(Constants.sundayMealType, from, currentEmpl.EmployeeID, shiftsByDate[DayOfWeek.Sunday.ToString()].Current);
                            }
                            else //employee ordered before something, but he wants to change it, do the UPDATE
                            {
                                exist = true;
                                updated = new MealsEmployeeSchedule().Update(Constants.sundayMealType, from, currentEmpl.EmployeeID, shiftsByDate[DayOfWeek.Sunday.ToString()].Current);
                            }
                        }
                        else
                        {
                            //so data could show
                            exist = true;
                            updated = true;
                        }
                        if ((exist == false && result > 0) || (exist == true && updated == true))
                        {
                            shiftsByDate[DayOfWeek.Sunday.ToString()].SundayOrder = true;
                            btnSundayMeal.BackColor = System.Drawing.ColorTranslator.FromHtml("#D4EBA2");
                            btnSundayMeal.Text = rm.GetString("youOrdered", culture) + " " + type.Name;
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("ErrorSavingMeal", culture));
                        }

                    }
                    else
                    {
                        lblError.Text = rm.GetString("shiftIsNotChosen", culture);
                    }
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.btnSunday(): " + ex.Message + "\n");
                lblError.Text = rm.GetString("errorChoosingShift", culture);
            }
        }
        #endregion

        //if only shifts are changed, reservation is updated
        private void updateOrder(string orderDay)
        {
            try
            {
                string day = orderDay;
                //update shift!
                string currentShift = shiftsByDate[day].Current;
                int currentType = shiftsByDate[day].CurrentTypeID;
                if (!currentShift.Equals(""))
                {
                    //saveMeal
                    lblError.Text = "";

                    bool updated = false;
                    bool exist = false;
                    if (isAdministrator == false)
                    {

                        DateTime date = dateOfTheDay[day];
                        DateTime from = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                        DateTime to = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                        ArrayList listOfOrderedMeals = new MealsEmployeeSchedule().SearchForEmpl(from, to, currentEmpl.EmployeeID); //check ordered meals from monday 00:00 until friday 23:59

                        if (listOfOrderedMeals.Count >= 0)
                        {
                            exist = true;
                            updated = new MealsEmployeeSchedule().Update(currentType, from, currentEmpl.EmployeeID, currentShift);
                        }
                    }
                    else
                    {
                        //so data could show
                        exist = true;
                        updated = true;
                    }
                    if ((exist != true || updated != true))
                    {
                        MessageBox.Show(rm.GetString("ErrorUpdatingMeal", culture));
                    }

                }
                else
                {
                    lblError.Text = rm.GetString("shiftIsNotChosen", culture);
                }
            }
            catch(Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsOrder.updateOrder(): " + ex.Message + "\n");
            }
        }
    }
}

public class RestaurantShiftsTO
{
    string _first = "I";
    string _second = "II";
    string _third = "III";
    string _current = "";
    int _currentTypeID = 0;
    bool _saturday_order = false;
    bool _sunday_order = false;

    public string First
    {
        get { return _first; }
        set { _first = value; }
    }

    public string Second
    {
        get { return _second; }
        set { _second = value; }
    }

    public string Third
    {
        get { return _third; }
        set { _third = value; }
    }

    public string Current
    {
        get { return _current; }
        set { _current = value; }
    }

    public bool SaturdayOrder
    {
        get { return _saturday_order; }
        set { _saturday_order = value; }
    }

    public bool SundayOrder
    {
        get { return _sunday_order; }
        set { _sunday_order = value; }
    }

    public int CurrentTypeID
    {
        get { return _currentTypeID; }
        set { _currentTypeID = value; }
    }

    public RestaurantShiftsTO()
    {

    }





}

