using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.Collections;

using Common;
using Util;
using TransferObjects;

namespace UI
{
    public partial class MealsWorkingUnitSchedules : UserControl
    {
        private CultureInfo culture;
        private ResourceManager rm;
        ApplUserTO logInUser;
        // Debug log
        DebugLog log;
        private string wuString = "";
        List<EmployeeTO> currentEmplArray;
        int firstControlSelected = -1;

        private List<WorkingUnitTO> wUnits;

        int x = 3;
        int y = 3;
        List<HolidayTO> holidays = new List<HolidayTO>();
        List<MealDayControl> listControls = new List<MealDayControl>();

        List<WorkingUnitTO> currentWorkingUnits = new List<WorkingUnitTO>();
        bool byDays = false;
        MealDayControl SelControl = new MealDayControl();

        List<int> mealsList = new List<int>();

        List<MealControl> mealControls = new List<MealControl>();
        int selectedControl = -1;
        protected List<WorkTimeSchemaTO> timeSchema;
        List<int> selEmployees = new List<int>();
        Dictionary<int, List<DateTime>> employeeWorkingDays = new Dictionary<int, List<DateTime>>();
        Dictionary<int, WorkTimeSchemaTO> emplTimeSchema = new Dictionary<int, WorkTimeSchemaTO>();
        Dictionary<int, List<DateTime>> employeeVacations = new Dictionary<int, List<DateTime>>();

        bool loading = false;

        private EmployeeTO currentEmployee;

        public EmployeeTO CurrentEmployee
        {
            get 
            { 
                return currentEmployee; 
            }
            set 
            { 
                currentEmployee = value;
                cbOverrideSch.Visible = false;
                tbNote.Visible = true;
                cbEmployee.SelectedValue = currentEmployee.EmployeeID;
                cbEmployee.Enabled = false;
            }
        }

        public MealsWorkingUnitSchedules()
        {
            loading = true;
            InitializeComponent();
            if (!this.DesignMode)
            {
                // Debug
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();

                // Set Language
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(Locations).Assembly);
            }
        }       

        private void MealsWorkingUnitSchedules_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (!this.DesignMode)
                {
                    holidays = new Holiday().Search(new DateTime(), new DateTime());
                    setLanguage();
                    this.btnByMeals.Enabled = false;

                    wUnits = new List<WorkingUnitTO>();
                    if (logInUser.UserID.Equals(Constants.selfServUser))
                    {
                        wUnits = new WorkingUnit().Search();
                    }
                    else
                    {
                        if (logInUser != null)
                        {
                            wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.RestaurantPurpose);
                        }
                    }
                    foreach (WorkingUnitTO wUnit in wUnits)
                    {
                        wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                    }

                    if (wuString.Length > 0)
                    {
                        wuString = wuString.Substring(0, wuString.Length - 1);
                    }

                    ArrayList list = new MealType().Search(-1, "", "", new DateTime(), new DateTime());
                    populateLvMeals(list);

                    populateWorkingUnitCombo();
                    cbWorkingUnit_SelectedIndexChanged(this, new EventArgs());
                    setCalendar(dtpMonth.Value);
                    loading = false;

                    tbNote.Text = rm.GetString("selTypeInList", culture);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedules.MealsWorkingUnitSchedules_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        public void controlSet(int controlNum)
        {
            try
            {
                SelControl = listControls[controlNum];
                if (cbWorkingUnit.SelectedIndex > 0 && cbEmployee.SelectedIndex <=0)
                {
                    DateTime date = SelControl.date;

                    if (!byDays)
                    {
                        if (selectedControl >= 0)
                        {
                            MealType type = mealControls[selectedControl].MealType;
                            int WUID = (int)cbWorkingUnit.SelectedValue;

                            List<int> wuList = new List<int>();
                            foreach (WorkingUnitTO wu in currentWorkingUnits)
                            {
                                if (wu.WorkingUnitID != WUID)
                                    wuList.Add(wu.WorkingUnitID);
                            }
                            wuList.Add(WUID);

                            MealsWorkingUnitSchedule schedule = new MealsWorkingUnitSchedule();

                            DateTime from = date.Date;
                            DateTime to = date.Date.AddDays(1);

                            ArrayList employeeScheduleList = new ArrayList();
                            ArrayList wuScheduleList = new ArrayList();
                            List<int> preDefinedEmpl = new List<int>();
                            foreach (int workingUnitID in wuList)
                            {
                                employeeScheduleList.AddRange(new MealsEmployeeSchedule().Search(from, to, workingUnitID));
                                wuScheduleList.AddRange(new MealsWorkingUnitSchedule().Search(-1, date.Date, workingUnitID));
                            }

                            if (!cbOverrideSch.Checked && cbOverrideSch.Visible)
                            {
                                foreach (MealsEmployeeSchedule meSch in employeeScheduleList)
                                {
                                    if (!preDefinedEmpl.Contains(meSch.EmployeeID))
                                        preDefinedEmpl.Add(meSch.EmployeeID);
                                }
                            }

                            bool trans = schedule.BeginTransaction();
                            bool saved = true;
                            if (trans)
                            {
                                foreach (MealsWorkingUnitSchedule sch in wuScheduleList)
                                {
                                    saved = saved && schedule.Delete(-1, date, sch.WorkingUnitID, false);
                                }
                                if (cbOverrideSch.Checked || !cbOverrideSch.Visible)
                                {
                                    foreach (MealsEmployeeSchedule sch in employeeScheduleList)
                                    {
                                        sch.SetTransaction(schedule.GetTransaction());
                                        saved = saved && sch.Delete(-1, date, sch.EmployeeID, false);
                                    }
                                }
                                foreach (int workingUnitID in wuList)
                                {
                                    saved = saved && (schedule.Save(type.MealTypeID, date, workingUnitID, false) == 1);
                                }
                                if (saved)
                                {
                                    MealsEmployeeSchedule emplMealSchedules = new MealsEmployeeSchedule();
                                    emplMealSchedules.SetTransaction(schedule.GetTransaction());

                                    foreach (EmployeeTO empl in currentEmplArray)
                                    {
                                        if (employeeWorkingDays.ContainsKey(empl.EmployeeID) && employeeWorkingDays[empl.EmployeeID].Contains(date.Date))
                                        {
                                            if (empl.EmployeeID != -1 && !preDefinedEmpl.Contains(empl.EmployeeID))
                                            {
                                                saved = saved && (emplMealSchedules.Save(type.MealTypeID, date, empl.EmployeeID, "", false) == 1);
                                            }
                                        }
                                    }
                                }

                                if (saved)
                                {
                                    schedule.CommitTransaction();
                                    SelControl.ChosenType = type;
                                }
                                else
                                {
                                    schedule.RollbackTransaction();
                                    MessageBox.Show(rm.GetString("wuScheduleNOTSaved", culture));
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (MealDayControl control in listControls)
                        {
                            if (SelControl.date != control.date)
                                control.Selected = false;
                        }
                        SelControl.Selected = true;
                        ArrayList typeSchedules = new MealsTypeSchedule().Search(-1, date.Date, date.Date);
                        mealsList = new List<int>();
                        foreach (MealsTypeSchedule sch in typeSchedules)
                        {
                            if (!mealsList.Contains(sch.MealTypeID))
                                mealsList.Add(sch.MealTypeID);
                        }

                        foreach (MealControl control in mealControls)
                        {
                            MealType itemSch = control.MealType;
                            selectedControl = -1;
                            if (mealsList.Contains(itemSch.MealTypeID))
                            {
                                control.ControlColor = Color.CornflowerBlue;
                                control.Enable = true;
                            }
                            else
                            {
                                control.ControlColor = Color.White;
                                control.Enable = false;
                            }
                        }
                    }                   
                }
                else if (cbEmployee.SelectedIndex > 0)
                {
                    tbNote.Text = rm.GetString("selMealTypeToSaveSch", culture);
                    DateTime date = SelControl.date;
                    if (!byDays)
                    {
                        ArrayList employeeScheduleList = new ArrayList();
                        MealType mealType = mealControls[selectedControl].MealType;
                        List<int> preDefinedEmpl = new List<int>();
                        int emplID = (int)cbEmployee.SelectedValue;
                        MealsEmployeeSchedule schedule = new MealsEmployeeSchedule();
                        employeeScheduleList = new MealsEmployeeSchedule().SearchForEmpl(date, date, emplID);
                        bool trans = schedule.BeginTransaction();
                        bool saved = true;
                        if (trans)
                        {
                            foreach (MealsEmployeeSchedule oldSch in employeeScheduleList)
                            {
                                saved = saved && schedule.Delete(oldSch.MealTypeID, oldSch.Date, oldSch.EmployeeID, false);
                            }
                            if (saved)
                            {
                                saved = saved && (schedule.Save(mealType.MealTypeID, date, emplID, "", false) == 1);
                            }
                            if (saved)
                            {
                                schedule.CommitTransaction();
                                SelControl.ChosenType = mealType;
                            }
                            else
                            {
                                schedule.RollbackTransaction();
                                MessageBox.Show(rm.GetString("wuScheduleNOTSaved", culture));
                            }
                        }
                    }
                    else
                    {
                        foreach (MealDayControl control in listControls)
                        {
                            if (SelControl.date != control.date)
                                control.Selected = false;
                        }
                        SelControl.Selected = true;
                        ArrayList typeSchedules = new MealsTypeSchedule().Search(-1, date.Date, date.Date);
                        mealsList = new List<int>();
                        foreach (MealsTypeSchedule sch in typeSchedules)
                        {
                            if (!mealsList.Contains(sch.MealTypeID))
                                mealsList.Add(sch.MealTypeID);
                        }

                        foreach (MealControl control in mealControls)
                        {
                            MealType itemSch = control.MealType;
                            selectedControl = -1;
                            if (mealsList.Contains(itemSch.MealTypeID))
                            {
                                control.ControlColor = Color.CornflowerBlue;
                                control.Enable = true;
                            }
                            else
                            {
                                control.ControlColor = Color.White;
                                control.Enable = false;
                            }
                        }
                    }                    
                }
                else
                {
                    MessageBox.Show(rm.GetString("selWorkingUnit", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedules.controlSet(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public void controlSelected(int controlNum)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                if (firstControlSelected == -1)
                {
                    firstControlSelected = controlNum;
                    MealDayControl SelControl = listControls[controlNum];
                    foreach (MealDayControl control in listControls)
                    {
                        if (SelControl.date != control.date)
                            control.Selected = false;
                    }
                }
                else if (controlNum > firstControlSelected)
                {
                    for (int i = firstControlSelected; i <= controlNum; i++)
                    {
                        MealDayControl control = (MealDayControl)listControls[i];
                        control.Selected = true;
                    }
                    firstControlSelected = -1;
                }
                else
                {
                    for (int i = firstControlSelected; i <= controlNum; i--)
                    {
                        MealDayControl control = (MealDayControl)listControls[i];
                        control.Selected = true;
                    }
                    firstControlSelected = -1;
                }
            }
            else if (Control.ModifierKeys != Keys.Control)
            {
                MealDayControl SelControl = listControls[controlNum];
                foreach (MealDayControl control in listControls)
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
                foreach (MealDayControl control in listControls)
                {
                    control.Selected = false;
                }
            }
        }
        /// <summary>
        /// Populate Working Unit Combo Box
        /// </summary>
        private void populateWorkingUnitCombo()
        {
            try
            {
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                foreach (WorkingUnitTO wuTO in wUnits)
                {
                    wuArray.Add(wuTO);
                }

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWorkingUnit.DataSource = wuArray;
                cbWorkingUnit.DisplayMember = "Name";
                cbWorkingUnit.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedules.populateWorkingUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Populate Employee Combo Box
        /// </summary>
        private void populateEmployeeCombo(int wuID)
        {
            try
            {
                currentEmplArray = new List<EmployeeTO>();
                currentWorkingUnits = new List<WorkingUnitTO>();
                string workUnitID = wuID.ToString();
                if (wuID == -1)
                {
                    currentEmplArray = new Employee().SearchByWU(wuString);
                }
                else
                {
                    if (chbHierarhicly.Checked)
                    {
                        List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWorkingUnit.SelectedValue)
                            {
                                wuList.Add(workingUnit);
                                workUnit.WUTO = workingUnit;
                            }
                        }
                        if (workUnit.WUTO.ChildWUNumber > 0)
                            wuList = workUnit.FindAllChildren(wuList);
                        workUnitID = "";
                        foreach (WorkingUnitTO wunit in wuList)
                        {
                            workUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (workUnitID.Length > 0)
                        {
                            workUnitID = workUnitID.Substring(0, workUnitID.Length - 1);
                        }

                        currentWorkingUnits = wuList;
                    }
                    currentEmplArray = new Employee().SearchByWU(workUnitID);
                }

                foreach (EmployeeTO employee in currentEmplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                currentEmplArray.Insert(0, empl);

                cbEmployee.DataSource = currentEmplArray;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
                cbEmployee.Invalidate();

                if (currentEmployee != null && currentEmployee.EmployeeID != -1)
                    cbEmployee.SelectedValue = currentEmployee.EmployeeID;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Passes.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void setLanguage()
        {
            try
            {
                lblWorkingUnit.Text = rm.GetString("lblWorkingUnit", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                btnByDays.Text = rm.GetString("btnByDays", culture);
                btnByMeals.Text = rm.GetString("btnByMeals", culture);
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);
                cbOverrideSch.Text = rm.GetString("cbOverrideSch", culture);

                lblMeals.Text = rm.GetString("mealAccessable", culture);
                gbLegend.Text = rm.GetString("gbLegend", culture);
                lblDiseabled.Text = rm.GetString("lblDiseabled", culture);
                lblNotWorkingDays.Text = rm.GetString("lblNotWorkingDays", culture);
                lblDaysWithMeals.Text = rm.GetString("lblDaysWithMeals", culture);
                lblWorkingDays.Text = rm.GetString("lblWorkingDays", culture);
                lblMealAvaiable.Text = rm.GetString("lblMealAvaiable", culture);                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedules.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                    cbWorkingUnit.SelectedIndex = cbWorkingUnit.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedules.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (cbWorkingUnit.SelectedValue is int)
                {
                    if ((int)cbWorkingUnit.SelectedValue >= 0)
                        populateEmployeeCombo((int)cbWorkingUnit.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedules.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateLvMeals(ArrayList meals)
        {
            try
            {
                //lvMeals.BeginUpdate();
                //for (int i = 0; i < meals.Count; i++)
                //{
                //    MealType type = (MealType)meals[i];
                //    ListViewItem item = new ListViewItem();
                //    item.Text = type.Name.ToString().Trim();
                //    item.Tag = type;

                //    lvMeals.Items.Add(item);
                //}

                //lvMeals.EndUpdate();
                //lvMeals.Invalidate();

                x = 3;
                y = 25;
                for (int i = 0; i < meals.Count; i++)
                {
                    MealType type = (MealType)meals[i];
                    MealControl control = new MealControl(type, this, i);
                    control.ControlColor = Color.White;
                    control.Location = new Point(x, y);
                    panelMeals.Controls.Add(control);
                    mealControls.Add(control);
                    y += 40;
                }               
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedules.populateLvMeals(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbWorkingUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool check = false;
                foreach (WorkingUnitTO wu in wUnits)
                {
                    if (cbWorkingUnit.SelectedIndex != 0)
                    {
                        if (wu.WorkingUnitID == (int)cbWorkingUnit.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                        {
                            if (!chbHierarhicly.Checked)
                            {
                                chbHierarhicly.Checked = true;
                                check = true;
                            }
                        }
                    }
                }

                if (cbWorkingUnit.SelectedValue is int && !check)
                {
                    populateEmployeeCombo((int)cbWorkingUnit.SelectedValue);
                   
                }
                if (cbWorkingUnit.SelectedIndex > 0)
                {
                    loadEmployeeData();
                }

                setCalendar(dtpMonth.Value);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedules.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void setCalendar(DateTime month)
        {
            panel1.Controls.Clear();
            x = 3;
            y = 3;
            listControls = new List<MealDayControl>();

            DateTime date = new DateTime(month.Year, month.Month, 1);
            DateTime dateTemp = new DateTime(month.Year, month.Month, 1);          

            ArrayList schedules = new ArrayList();
            Dictionary<DateTime, MealsWorkingUnitSchedule> scheduleDictionary = new Dictionary<DateTime, MealsWorkingUnitSchedule>();
            ArrayList emplSchedules = new ArrayList();
            Dictionary<DateTime, MealsEmployeeSchedule> emplScheduleDictionary = new Dictionary<DateTime, MealsEmployeeSchedule>();
            ArrayList typeSchedules = new ArrayList();
            Dictionary<DateTime, MealsTypeSchedule> typeScheduleDictionary = new Dictionary<DateTime, MealsTypeSchedule>();
            WorkTimeSchemaTO timeSchema = new WorkTimeSchemaTO();
            int selEmpl = -1;

            try
            {
                if (!btnByMeals.Enabled)
                {
                    if (selectedControl >= 0)
                    {
                        MealType type = mealControls[selectedControl].MealType;
                        typeSchedules = new MealsTypeSchedule().Search(type.MealTypeID, new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1), new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1).AddMonths(1).AddDays(-1));
                        foreach (MealsTypeSchedule sch in typeSchedules)
                        {
                            if (!typeScheduleDictionary.ContainsKey(sch.Date.Date))
                            {
                                typeScheduleDictionary.Add(sch.Date.Date, sch);
                            }
                        }
                    }
                }
                if (cbWorkingUnit.Items.Count > 0 && (int)cbWorkingUnit.SelectedValue > 0 && (int)cbEmployee.SelectedValue < 0)
                {
                    schedules = new MealsWorkingUnitSchedule().Search(-1, new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1), new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1).AddMonths(1).AddDays(-1), (int)cbWorkingUnit.SelectedValue);
                    foreach (MealsWorkingUnitSchedule sch in schedules)
                    {
                        if (!scheduleDictionary.ContainsKey(sch.Date.Date))
                        {
                            scheduleDictionary.Add(sch.Date.Date, sch);
                        }
                    }
                }
                if ((int)cbEmployee.SelectedValue > 0)
                {
                    selEmpl = (int)cbEmployee.SelectedValue;
                    if (emplTimeSchema.ContainsKey(selEmpl))
                        timeSchema = emplTimeSchema[selEmpl];
                    DateTime to = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1).AddMonths(1).AddDays(-1);
                    emplSchedules = new MealsEmployeeSchedule().SearchForEmpl(new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1), to, selEmpl);
                    foreach (MealsEmployeeSchedule sch in emplSchedules)
                    {
                        if (!emplScheduleDictionary.ContainsKey(sch.Date.Date))
                        {
                            emplScheduleDictionary.Add(sch.Date.Date, sch);
                        }
                    }
                }
            }
            catch { }

            int startDay = 0;
            startDay = getDayOfWeek(date);
            int j = startDay;
            x += 107 * startDay;
            for (int i = 1; i <= 6; i++)
            {
                while (j <= 6)
                {
                    if (date.Month != dateTemp.Month)
                        break;
                    MealsWorkingUnitSchedule sch = new MealsWorkingUnitSchedule();
                    MealsEmployeeSchedule emplSch = new MealsEmployeeSchedule();
                    if (scheduleDictionary.ContainsKey(dateTemp.Date))
                    {
                        sch = scheduleDictionary[dateTemp.Date];                       
                    }
                    if (emplScheduleDictionary.ContainsKey(dateTemp.Date))
                    {
                        emplSch = emplScheduleDictionary[dateTemp.Date];
                    }
                    bool workingDay = true;

                    if (selEmpl > 0)
                    {
                        if (employeeWorkingDays.ContainsKey(selEmpl))
                        {
                            if (!employeeWorkingDays[selEmpl].Contains(dateTemp.Date))
                            {
                                workingDay = false;
                            }
                        }
                        else
                        {
                            workingDay = false;
                        }
                    }

                    bool vacation = false;
                    if (selEmpl > 0)
                    {
                        if (employeeVacations.ContainsKey(selEmpl))
                        {
                            if (!employeeVacations[selEmpl].Contains(dateTemp.Date))
                            {
                                vacation = true;
                            }
                        }                     
                    }
                    
                    MealDayControl meal = new MealDayControl(dateTemp, timeSchema, holidays,workingDay, vacation, listControls.Count, this);                   
                    meal.Location = new Point(x, y);                    
                    panel1.Controls.Add(meal);
                    listControls.Add(meal);
                    if (typeScheduleDictionary.ContainsKey(dateTemp.Date))
                    {
                        meal.Selected = true;
                        meal.Avaiable = true;
                    }
                    if (byDays)
                        meal.byDays= true;
                    if (sch.MealTypeID != -1)
                        meal.Schedule = sch;
                    if (emplSch.MealTypeID != -1)
                        meal.EmplSchedule = emplSch;
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

        private void dtpMonth_ValueChanged(object sender, EventArgs e)
        {
            if(!loading && (cbWorkingUnit.SelectedIndex >0 ||cbEmployee.SelectedIndex>0 ) )
            loadEmployeeData();
            setCalendar(dtpMonth.Value);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            dtpMonth.Value = dtpMonth.Value.AddMonths(1);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            dtpMonth.Value = dtpMonth.Value.AddMonths(-1);
        }

        private void btnByDays_Click(object sender, EventArgs e)
        {
            btnByDays.Enabled = false;
            btnByMeals.Enabled = true;

            panel1.BackColor = Color.WhiteSmoke;
            panelMeals.BackColor = Color.LightGray;
            foreach (MealDayControl control in listControls)
            {
                control.byDays = true;
            }
            foreach (MealControl control in mealControls)
            {
                control.ControlColor = Color.White;
                control.Enable = true;
            }
            byDays = true;
            setCalendar(dtpMonth.Value);

            tbNote.Text = rm.GetString("selDayInCalendar", culture);
        }

        private void btnByMeals_Click(object sender, EventArgs e)
        {
            btnByDays.Enabled = true;
            btnByMeals.Enabled =false;
            byDays = false;

            panel1.BackColor = Color.LightGray;
            panelMeals.BackColor = Color.WhiteSmoke;
            foreach (MealControl control in mealControls)
            {
                control.ControlColor = Color.White;
                control.Enable = true;
            }
            selectedControl = -1;
            setCalendar(dtpMonth.Value);

            tbNote.Text = rm.GetString("selTypeInList", culture);
            
        }
       
        //private void lvMeals_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
                
        //        if (lvMeals.SelectedItems.Count == 1)
        //        {
        //            if (!byDays)
        //            {
        //                setCalendar(dtpMonth.Value);
        //            }
        //            else
        //            {
        //                if (cbWorkingUnit.SelectedIndex > 0 && SelControl.date != new DateTime())
        //                {
        //                    if (!mealsList.Contains(((MealType)lvMeals.SelectedItems[0].Tag).MealTypeID))
        //                    {
        //                        MessageBox.Show(rm.GetString("oneOfSelected",culture));

        //                        selItemChange = true;
        //                        foreach (ListViewItem item in lvMeals.Items)
        //                            item.Selected = false;

        //                        selItemChange = false;
        //                        return;
        //                    }

        //                    DateTime date = SelControl.date;

        //                    MealType type = (MealType)lvMeals.SelectedItems[0].Tag;
        //                    int WUID = (int)cbWorkingUnit.SelectedValue;

        //                    List<int> wuList = new List<int>();
        //                    foreach (WorkingUnit wu in currentWorkingUnits)
        //                    {
        //                        wuList.Add(wu.WorkingUnitID);
        //                    }
        //                    wuList.Add(WUID);

        //                    MealsWorkingUnitSchedule schedule = new MealsWorkingUnitSchedule();

        //                    DateTime from = date.Date;
        //                    DateTime to = date.Date.AddDays(1);


        //                    ArrayList employeeScheduleList = new ArrayList();
        //                    ArrayList wuScheduleList = new ArrayList();
        //                    List<int> preDefinedEmpl = new List<int>();
        //                    foreach (int workingUnitID in wuList)
        //                    {
        //                        employeeScheduleList.AddRange(new MealsEmployeeSchedule().Search(from, to, workingUnitID));
        //                        wuScheduleList.AddRange(new MealsWorkingUnitSchedule().Search(-1, date.Date, workingUnitID));
        //                    }

        //                    if (!cbOverrideSch.Checked && cbOverrideSch.Visible)
        //                    {
        //                        foreach (MealsEmployeeSchedule meSch in employeeScheduleList)
        //                        {
        //                            if (!preDefinedEmpl.Contains(meSch.EmployeeID))
        //                                preDefinedEmpl.Add(meSch.EmployeeID);
        //                        }
        //                    }

        //                    bool trans = schedule.BeginTransaction();
        //                    bool saved = true;
        //                    if (trans)
        //                    {

        //                        foreach (MealsWorkingUnitSchedule sch in wuScheduleList)
        //                        {
        //                            saved = saved && schedule.Delete(-1, date, sch.WorkingUnitID, false);
        //                        }
        //                        if (cbOverrideSch.Checked || !cbOverrideSch.Visible)
        //                        {
        //                            foreach (MealsEmployeeSchedule sch in employeeScheduleList)
        //                            {
        //                                sch.SetTransaction(schedule.GetTransaction());
        //                                saved = saved && sch.Delete(-1, date, sch.EmployeeID, false);
        //                            }
        //                        }
        //                        foreach (int workingUnitID in wuList)
        //                        {
        //                            saved = saved && (schedule.Save(type.MealTypeID, date, workingUnitID, false) == 1);
        //                            if (saved)
        //                            {
        //                                MealsEmployeeSchedule emplMealSchedules = new MealsEmployeeSchedule();
        //                                emplMealSchedules.SetTransaction(schedule.GetTransaction());

        //                                foreach (Employee empl in currentEmplArray)
        //                                {
        //                                    if (empl.EmployeeID != -1 && !preDefinedEmpl.Contains(empl.EmployeeID))
        //                                    {
        //                                        saved = saved && (emplMealSchedules.Save(type.MealTypeID, date, empl.EmployeeID, false) == 1);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                        if (saved)
        //                        {
        //                            schedule.CommitTransaction();
        //                            SelControl.ChosenType = type;
        //                        }
        //                        else
        //                        {
        //                            schedule.RollbackTransaction();
        //                            MessageBox.Show(rm.GetString("wuScheduleNOTSaved", culture));
        //                        }
        //                    }
        //                }

        //                foreach (ListViewItem item in lvMeals.Items)
        //                {
        //                    item.BackColor = Color.White;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.writeLog(DateTime.Now + " MealsWorkingUnitSchedules.lvMeals_SelectedIndexChanged(): " + ex.Message + "\n");
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void cbEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (cbEmployee.SelectedIndex > 0)
                {
                    if ((int)cbEmployee.SelectedValue == -1)
                    {
                        cbOverrideSch.Visible = true;
                    }
                    else
                    {
                        cbOverrideSch.Visible = false;
                        if (cbWorkingUnit.SelectedIndex == 0)
                        {
                            loadEmployeeData();
                        }
                    }
                }
                else
                {
                    cbOverrideSch.Visible = true;
                }

                setCalendar(dtpMonth.Value);
              
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedules.cbEmployee_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void loadEmployeeData()
        {
            try
            {
                employeeWorkingDays = new Dictionary<int,List<DateTime>>();
                emplTimeSchema = new Dictionary<int, WorkTimeSchemaTO>();
                employeeVacations = new Dictionary<int, List<DateTime>>();

                List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();
                DateTime selDate = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
                timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(this.getSelectedEmployeesString(), selDate, selDate.AddMonths(1).AddDays(-1));
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

                EmployeeAbsence absence = new EmployeeAbsence();
                absence.EmplAbsTO.PassTypeID = Constants.vacation;
                absence.EmplAbsTO.DateStart = selDate;
                absence.EmplAbsTO.DateEnd = selDate.AddMonths(1).AddDays(-1);
                List<EmployeeAbsenceTO> absenceList = absence.Search("");

                foreach (EmployeeAbsenceTO abs in absenceList)
                {
                    if (!employeeVacations.ContainsKey(abs.EmployeeID))
                    {
                        employeeVacations.Add(abs.EmployeeID, new List<DateTime>());
                    }
                    for (DateTime dateIterator = abs.DateStart.Date; dateIterator <= abs.DateEnd.Date; dateIterator = dateIterator.AddDays(1))
                    {
                        employeeVacations[abs.EmployeeID].Add(dateIterator);
                    }
                }

                int i = 0;
                foreach (int emplID in selEmployees)
                {
                    i++;
                    if (!employeeWorkingDays.ContainsKey(emplID))
                        employeeWorkingDays.Add(emplID, new List<DateTime>());
                    DateTime currentDate = selDate;
                    while (currentDate.Month == dtpMonth.Value.Month)
                    {
                        WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                        List<WorkTimeIntervalTO> timeSchemaIntervalList = this.getTimeSchemaInterval(emplID, currentDate, timeScheduleList,out schema);
                        
                        if (timeSchemaIntervalList.Count > 0)
                        {
                            WorkTimeIntervalTO interval = timeSchemaIntervalList[0];
                            TimeSpan duration = interval.EndTime - interval.StartTime;
                            if (duration.TotalMinutes > 0)
                                employeeWorkingDays[emplID].Add(currentDate.Date);
                        }
                        if (schema.TimeSchemaID != -1)
                        {
                            if (!emplTimeSchema.ContainsKey(emplID))
                                emplTimeSchema.Add(emplID, schema);
                        }
                        currentDate = currentDate.AddDays(1);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedules.loadEmployeeData(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public virtual List<WorkTimeIntervalTO> getTimeSchemaInterval(int employeeID, DateTime date, List<EmployeeTimeScheduleTO> timeScheduleList, out WorkTimeSchemaTO emplTimeSch)
        {
            emplTimeSch = new WorkTimeSchemaTO();
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
                    emplTimeSch = schema;
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

        protected string getSelectedEmployeesString()
        {
            string selectedEmployees = "";
            selEmployees = new List<int>();
            if (cbWorkingUnit.SelectedIndex == 0 && cbEmployee.SelectedIndex > 0)
            {
                selEmployees.Add((int)cbEmployee.SelectedValue);
                selectedEmployees += ((int)cbEmployee.SelectedValue).ToString();
            }
            else
            {
                if (currentEmplArray != null)
                {
                    foreach (EmployeeTO empl in currentEmplArray)
                    {
                        if (empl.EmployeeID != -1)
                        {
                            selectedEmployees += empl.EmployeeID.ToString() + ", ";
                            selEmployees.Add(empl.EmployeeID);
                        }
                    }
                    selectedEmployees = selectedEmployees.Substring(0, selectedEmployees.Length - 2);
                }
            }
            return selectedEmployees;
        }

        internal void mealsSelectionChange(MealType mealType, int orderNumber)
        {
            try
            {

                if (orderNumber >= 0)
                {
                    if (!byDays)
                    {
                        foreach (MealControl control in mealControls)
                        {
                            if (control.orderNumber == orderNumber)
                            {
                                control.ControlColor = Color.CornflowerBlue;
                                tbNote.Text = rm.GetString("selDayToAddSchedule", culture);
                            }
                            else
                            {
                                control.ControlColor = Color.White;
                            }
                        }
                        selectedControl = orderNumber;
                        setCalendar(dtpMonth.Value);
                    }
                    else
                    {
                        if (cbWorkingUnit.SelectedIndex > 0 && SelControl.date != new DateTime() && cbEmployee.SelectedIndex <= 0)
                        {
                            if (!mealsList.Contains(mealType.MealTypeID))
                            {
                                MessageBox.Show(rm.GetString("oneOfSelected", culture));
                            }

                            DateTime date = SelControl.date;

                            MealType type = mealType;
                            int WUID = (int)cbWorkingUnit.SelectedValue;

                            List<int> wuList = new List<int>();
                            foreach (WorkingUnitTO wu in currentWorkingUnits)
                            {
                                wuList.Add(wu.WorkingUnitID);
                            }
                            wuList.Add(WUID);

                            MealsWorkingUnitSchedule schedule = new MealsWorkingUnitSchedule();

                            DateTime from = date.Date;
                            DateTime to = date.Date.AddDays(1);


                            ArrayList employeeScheduleList = new ArrayList();
                            ArrayList wuScheduleList = new ArrayList();
                            List<int> preDefinedEmpl = new List<int>();
                            foreach (int workingUnitID in wuList)
                            {
                                employeeScheduleList.AddRange(new MealsEmployeeSchedule().Search(from, to, workingUnitID));
                                wuScheduleList.AddRange(new MealsWorkingUnitSchedule().Search(-1, date.Date, workingUnitID));
                            }

                            if (!cbOverrideSch.Checked && cbOverrideSch.Visible)
                            {
                                foreach (MealsEmployeeSchedule meSch in employeeScheduleList)
                                {
                                    if (!preDefinedEmpl.Contains(meSch.EmployeeID))
                                        preDefinedEmpl.Add(meSch.EmployeeID);
                                }
                            }

                            bool trans = schedule.BeginTransaction();
                            bool saved = true;
                            if (trans)
                            {

                                foreach (MealsWorkingUnitSchedule sch in wuScheduleList)
                                {
                                    saved = saved && schedule.Delete(-1, date, sch.WorkingUnitID, false);
                                }
                                if (cbOverrideSch.Checked || !cbOverrideSch.Visible)
                                {
                                    foreach (MealsEmployeeSchedule sch in employeeScheduleList)
                                    {
                                        sch.SetTransaction(schedule.GetTransaction());
                                        saved = saved && sch.Delete(-1, date, sch.EmployeeID, false);
                                    }
                                }
                                foreach (int workingUnitID in wuList)
                                {
                                    saved = saved && (schedule.Save(type.MealTypeID, date, workingUnitID, false) == 1);
                                    if (saved)
                                    {
                                        MealsEmployeeSchedule emplMealSchedules = new MealsEmployeeSchedule();
                                        emplMealSchedules.SetTransaction(schedule.GetTransaction());

                                        foreach (EmployeeTO empl in currentEmplArray)
                                        {
                                            if (employeeWorkingDays.ContainsKey(empl.EmployeeID) && employeeWorkingDays[empl.EmployeeID].Contains(date.Date))
                                            {
                                                if (empl.EmployeeID != -1 && !preDefinedEmpl.Contains(empl.EmployeeID))
                                                {
                                                    saved = saved && (emplMealSchedules.Save(type.MealTypeID, date, empl.EmployeeID, "", false) == 1);
                                                }
                                            }
                                        }

                                    }
                                }
                                if (saved)
                                {
                                    schedule.CommitTransaction();
                                    SelControl.ChosenType = type;
                                }
                                else
                                {
                                    schedule.RollbackTransaction();
                                    MessageBox.Show(rm.GetString("wuScheduleNOTSaved", culture));
                                }
                            }
                        }
                        else if(cbEmployee.SelectedIndex >0)
                        {
                            if (!byDays)
                            {
                                foreach (MealControl control in mealControls)
                                {
                                    if (control.orderNumber == orderNumber)
                                    {
                                        control.ControlColor = Color.CornflowerBlue;
                                    }
                                    else
                                    {
                                        control.ControlColor = Color.White;
                                    }
                                }
                                selectedControl = orderNumber;
                                setCalendar(dtpMonth.Value);
                            }
                            else
                            {
                                ArrayList employeeScheduleList = new ArrayList();
                                List<int> preDefinedEmpl = new List<int>();
                                int emplID = (int)cbEmployee.SelectedValue;
                                DateTime date = SelControl.date;
                                MealsEmployeeSchedule schedule = new MealsEmployeeSchedule();
                                employeeScheduleList = new MealsEmployeeSchedule().SearchForEmpl(date, date, emplID);
                                bool trans = schedule.BeginTransaction();
                                bool saved = true;
                                if (trans)
                                {
                                    foreach (MealsEmployeeSchedule oldSch in employeeScheduleList)
                                    {
                                        saved = saved && schedule.Delete(oldSch.MealTypeID, oldSch.Date, oldSch.EmployeeID, false);
                                    }
                                    if (saved)
                                    {
                                        saved = saved && (schedule.Save(mealType.MealTypeID, date, emplID, "", false) == 1);
                                    }
                                    if (saved)
                                    {
                                        schedule.CommitTransaction();
                                        SelControl.ChosenType = mealType;
                                    }
                                    else
                                    {
                                        schedule.RollbackTransaction();
                                        MessageBox.Show(rm.GetString("wuScheduleNOTSaved", culture));
                                    }
                                }
                            }
                        }

                        foreach (MealControl control in mealControls)
                        {
                            control.ControlColor = Color.White;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsWorkingUnitSchedules.mealsSelectionChange(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
