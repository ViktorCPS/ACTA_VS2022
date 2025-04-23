using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Globalization;
using System.Resources;

using Common;
using Util;
using TransferObjects;

namespace UI
{
    public partial class MealEmployeeSchedulePreview : UserControl
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        ApplUserTO logInUser;
        List<EmployeeTO> currentEmplArray;
        private List<WorkingUnitTO> wUnits;
        private string wuString = "";
        private string currentWuString = "";

        private int startIndex;

        Dictionary<int, string> emplWUDic;

        ArrayList currentMealsUsed;
        ArrayList currentOrder;
        ArrayList orderedAndUsed;

        Dictionary<int, OrderedUsedMealsCountTO> noOfUsedOrderedByEmplDic;
        Dictionary<int, OrderedUsedMealsCountTO> noOfUsedOrderedByWUDic;

        private int sortOrder;
        private int sortField;

        // List View indexes List of used meals
        const int EmployeeIndex = 0;
        const int TimeIndex = 1;
        const int MealTypeIndex = 2;
        const int MealPointIndex = 3;
        Hashtable employeeQty = new Hashtable();

        //List View Indexes List of orders
        const int WorkingUnitIndex = 0;
        const int EmplForOrderIndex = 1;
        const int DateIndex = 2;
        const int MealTypeForOrderIndex = 3;
        const int ShiftIndex = 4;

        //List View Indexes List of ordered and used
        const int WorkingUnitNameIndex = 0;
        const int EmplIDIndex = 1;
        const int EmplUsedOrderedIndex = 2;
        const int NoOfOrderedIndex = 3;
        const int NoOfUsedIndex = 4;

        static int whatList = 0;

        bool fillTheDictionary = false;

        public MealEmployeeSchedulePreview()
        {
            InitializeComponent(); 
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);
            setLanguage();

            logInUser = NotificationController.GetLogInUser();
            fillTheDictionary = true;


            currentMealsUsed = new ArrayList();
            currentOrder = new ArrayList();
            orderedAndUsed = new ArrayList();
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

                cbWU.DataSource = wuArray;
                cbWU.DisplayMember = "Name";
                cbWU.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.populateWorkingUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Populate Meal point Combo Box
        /// </summary>
        private void populateMealPointCombo()
        {
            try
            {
                MealPoint mp = new MealPoint();
                ArrayList mpArray = new ArrayList();
                mpArray = mp.Search(-1, "", "", "");
                mpArray.Insert(0, new MealPoint(-1,"", rm.GetString("all", culture), ""));

                cbPlace.DataSource = mpArray;
                cbPlace.DisplayMember = "Name";
                cbPlace.ValueMember = "MealPointID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.populateWorkingUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

          /// <summary>
        /// Populate Meal type Combo Box
        /// </summary>
        private void populateMealTypeCombo()
        {
            try
            {
                MealType mt = new MealType();
                ArrayList mtArray = new ArrayList();
                mtArray = mt.Search(-1, "", "",new DateTime(),new DateTime());
                mtArray.Insert(0, new MealType(-1, rm.GetString("all", culture), "", new DateTime(),new DateTime()));

                cbMealType.DataSource = mtArray;
                cbMealType.DisplayMember = "Name";
                cbMealType.ValueMember = "MealTypeID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.populateWorkingUnitCombo(): " + ex.Message + "\n");
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
                            if (workingUnit.WorkingUnitID == (int)this.cbWU.SelectedValue)
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
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Set proper language.
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // button's text
                btnGenerateReport.Text = rm.GetString("btnGenerateReport", culture);
                btnClear.Text = rm.GetString("btnClear", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);
                               
                // group box text
                this.gbReport.Text = rm.GetString("gbReport", culture);
                gbSearchCriteria.Text = rm.GetString("gbSearchCriteria", culture);
                gbSearchResults.Text = rm.GetString("gbSearchResults", culture);
             
                // check box text               
                chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);

                // label's text
                lblWorkingUnit.Text = rm.GetString("lblWU", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblSumary.Text = rm.GetString("lblSumary", culture);
                lblPlace.Text = rm.GetString("lblPlace", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblMealType.Text = rm.GetString("lblMealType", culture);
                lblSumary.Text = rm.GetString("lblSumary", culture);
                lblQty.Text = rm.GetString("lblQty", culture);

                // RadioButton ButtonState text
                rbNo.Text = rm.GetString("no", culture);
                rbYes.Text = rm.GetString("yes", culture);
                rbOrderDoesntExist.Text = rm.GetString("rbOrderDoesntExist", culture);
                rbOrderExists.Text = rm.GetString("rbOrderExist", culture);
                rbOrders.Text = rm.GetString("rbOrders", culture);
                rbUsedMeals.Text = rm.GetString("rbUsedMeals", culture);
                rbUsedOrdered.Text = rm.GetString("rbUsedOrdered", culture);

                // list view initialization
                lvEmployeeMeals.BeginUpdate();
                lvEmployeeMeals.Columns.Add(rm.GetString("hdrEmployee", culture), (lvEmployeeMeals.Width - 10) / 4, HorizontalAlignment.Left);
                lvEmployeeMeals.Columns.Add(rm.GetString("hdrTime", culture), (lvEmployeeMeals.Width - 10) / 4, HorizontalAlignment.Left);
                lvEmployeeMeals.Columns.Add(rm.GetString("hdrMealType", culture), (lvEmployeeMeals.Width - 10) / 4, HorizontalAlignment.Left);
                lvEmployeeMeals.Columns.Add(rm.GetString("hdrPlace", culture), (lvEmployeeMeals.Width - 10) / 4, HorizontalAlignment.Left);               
                lvEmployeeMeals.EndUpdate();

                // list view initialization
                lvOrders.BeginUpdate();
                lvOrders.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvOrders.Width - 10) / 5, HorizontalAlignment.Left);
                lvOrders.Columns.Add(rm.GetString("hdrEmployee", culture), (lvOrders.Width - 10) / 5, HorizontalAlignment.Left);
                lvOrders.Columns.Add(rm.GetString("hdrDate", culture), (lvOrders.Width - 10) / 5, HorizontalAlignment.Left);
                lvOrders.Columns.Add(rm.GetString("hdrMealType", culture), (lvOrders.Width - 10) / 5, HorizontalAlignment.Left);
                lvOrders.Columns.Add(rm.GetString("hdrShift", culture), (lvOrders.Width - 10) / 5, HorizontalAlignment.Left);
                lvOrders.EndUpdate();

                //list view initialization
                lvUsedOrdered.BeginUpdate();
                lvUsedOrdered.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvUsedOrdered.Width - 10) / 5, HorizontalAlignment.Left);
                lvUsedOrdered.Columns.Add(rm.GetString("hdrID", culture), (lvUsedOrdered.Width - 10) / 5, HorizontalAlignment.Left);
                lvUsedOrdered.Columns.Add(rm.GetString("hdrEmployee", culture), (lvUsedOrdered.Width - 10) / 5, HorizontalAlignment.Left);
                lvUsedOrdered.Columns.Add(rm.GetString("hdrNoOfOrdered", culture), (lvUsedOrdered.Width - 10) / 5, HorizontalAlignment.Left);
                lvUsedOrdered.Columns.Add(rm.GetString("hdrNoOfUsed", culture), (lvUsedOrdered.Width - 10) / 5, HorizontalAlignment.Left);
                lvUsedOrdered.EndUpdate();
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void MealEmployeeSchedulePreview_Load(object sender, EventArgs e)
        {
            try
            {
                sortOrder = Constants.sortAsc;
                if (rbUsedMeals.Checked)
                    sortField = MealEmployeeSchedulePreview.TimeIndex;
                else if (rbOrderExists.Checked || rbOrderDoesntExist.Checked)
                    sortField = MealEmployeeSchedulePreview.DateIndex;
                
                startIndex = 0;

                this.lblTotal.Visible = false;

                wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.RestaurantPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                }

                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                    currentWuString = wuString;
                }

                populateWorkingUnitCombo();
                populateEmployeeCombo(-1);
                populateMealPointCombo();
                populateMealTypeCombo();

                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;

                
                if (fillTheDictionary)
                {
                    emplWUDic = new Dictionary<int,string>();
                    List<EmployeeTO> employeeList = new Employee().Search();
                    foreach (EmployeeTO empl in employeeList)
                    {
                        if (emplWUDic.ContainsKey(empl.EmployeeID))
                        {
                            emplWUDic.Add(empl.EmployeeID, empl.WorkingUnitName);
                        }
                    }
                    fillTheDictionary = false;
                }
                rbUsedMeals_CheckedChanged(this, null);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.MealEmployeeSchedulePreview_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (cbWU.SelectedValue is int)
                {
                    if ((int)cbWU.SelectedValue >= 0)
                        populateEmployeeCombo((int)cbWU.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
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
                    cbWU.SelectedIndex = cbWU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                if (rbUsedMeals.Checked)
                {
                    #region Used meals
                    MealUsed mealUsed = new MealUsed();

                    int employeeID = -1;
                    int mealPointID = -1;
                    int mealType = -1;
                    string wuStr = wuString;

                    if (cbEmployee.SelectedIndex > 0)
                    {
                        employeeID = (int)cbEmployee.SelectedValue;
                    }
                    if (cbPlace.SelectedIndex > 0)
                    {
                        mealPointID = (int)cbPlace.SelectedValue;
                    }
                    if (cbMealType.SelectedIndex > 0)
                    {
                        mealType = (int)cbMealType.SelectedValue;
                    }
                    if (cbWU.SelectedIndex > 0)
                    {
                        if (!chbHierarhicly.Checked)
                        {
                            wuStr = cbWU.SelectedValue.ToString();
                        }
                    }

                    // ne treba dodavati na dtpTO .AddDays(1) jer je u metodi to odradjeno
                    currentMealsUsed = mealUsed.Search(employeeID, dtpFrom.Value.Date, dtpTo.Value.Date, mealPointID, mealType, -1, -1, -1, -1, wuStr);
                    if (numQtyFrom.Value > 0 || numQtyTo.Value > 0)
                    {
                        employeeQty = new Hashtable();
                        foreach (MealUsed meal in currentMealsUsed)
                        {
                            if (employeeQty.Contains(meal.EmployeeID))
                            {
                                employeeQty[meal.EmployeeID] = ((int)employeeQty[meal.EmployeeID]) + 1;
                            }
                            else
                            {
                                employeeQty.Add(meal.EmployeeID, 1);
                            }
                        }

                        ArrayList temp = new ArrayList();
                        foreach (MealUsed meal in currentMealsUsed)
                        {
                            if (employeeQty.Contains(meal.EmployeeID))
                            {
                                int i = (int)employeeQty[meal.EmployeeID];
                                if (numQtyFrom.Value > 0)
                                {
                                    if (((numQtyTo.Value > 0 && i <= numQtyTo.Value) || numQtyTo.Value <= 0) && i >= numQtyFrom.Value)
                                    {
                                        temp.Add(meal);
                                    }
                                }
                                else if (numQtyTo.Value > 0 && i <= numQtyTo.Value)
                                {
                                    temp.Add(meal);
                                }
                            }
                        }
                        currentMealsUsed = temp;
                    }
                    if (currentMealsUsed.Count > 0)
                    {
                        startIndex = 0;
                        currentMealsUsed.Sort(new ArrayListSort(sortOrder, sortField));
                        populateListViewUsed(currentMealsUsed, startIndex);
                        this.lblTotal.Visible = true;
                        this.lblTotal.Text = rm.GetString("lblTotal", culture) + currentMealsUsed.Count.ToString().Trim();
                    }
                    else
                    {
                        lvEmployeeMeals.Items.Clear();
                    }

                    #endregion
                }
                else if (rbOrders.Checked)
                {
                    #region Ordered meals
                    int employeeID = -1;
                    int mealType = -1;
                    string wuStr = wuString;

                    if (cbEmployee.SelectedIndex > 0)
                    {
                        employeeID = (int)cbEmployee.SelectedValue;
                    }

                    if (cbMealType.SelectedIndex > 0)
                    {
                        mealType = (int)cbMealType.SelectedValue;
                    }
                    if (cbWU.SelectedIndex > 0)
                    {
                        if (!chbHierarhicly.Checked)
                        {
                            wuStr = cbWU.SelectedValue.ToString();
                        }
                    }

                    if (rbOrderExists.Checked) //with orders
                    {
                        MealsEmployeeSchedule mealsEmplSchedule = new MealsEmployeeSchedule();
                        currentOrder = mealsEmplSchedule.Search(dtpFrom.Value.Date, dtpTo.Value.AddDays(1).Date, wuStr, employeeID, mealType);

                        if (currentOrder.Count > 0)
                        {
                            startIndex = 0;
                            sortField = 2;
                            currentOrder.Sort(new ArrayListSort(sortOrder, sortField));
                            populateListViewOrders(currentOrder, startIndex, false);
                            this.lblTotal.Visible = true;
                            this.lblTotal.Text = rm.GetString("lblTotal", culture) + currentOrder.Count.ToString().Trim();
                        }
                        else
                        {
                            this.lblTotal.Visible = false;
                            this.lblTotal.Text = "";
                            lvOrders.Items.Clear();
                        }

                    }
                    else //without orders
                    {
                        TimeSpan range = (dtpTo.Value.AddDays(1).Date - dtpFrom.Value.Date); //da bi sracunao i poslednji dan
                        Dictionary<DateTime, List<EmployeeTO>> whoOrderedWhen = new MealsEmployeeSchedule().SearchDateEndEmployeesWhoOrdered(dtpFrom.Value.Date, dtpTo.Value.AddDays(1).Date, wuStr);

                        currentOrder = new ArrayList();
                        populateEmployeeCombo(Convert.ToInt32(cbWU.SelectedValue));
                        for (int i = 0; i < range.Days; i++)
                        {
                            DateTime date = dtpFrom.Value.AddDays(i);

                            if (whoOrderedWhen.ContainsKey(date))
                            {
                                List<EmployeeTO> emplWhoOrdered = whoOrderedWhen[date];
                                foreach (EmployeeTO employee in currentEmplArray)
                                {
                                    if (!emplWhoOrdered.Contains(employee))
                                    {
                                        if (employee.EmployeeID != -1)
                                        {
                                            MealsEmployeeSchedule emplSchedule = new MealsEmployeeSchedule();
                                            emplSchedule.EmployeeID = employee.EmployeeID;
                                            emplSchedule.WorkingUnit = employee.WorkingUnitName;
                                            //  emplSchedule.EmployeeFirstName = employee.FirstName;
                                            emplSchedule.EmployeeLastName = employee.LastName;
                                            emplSchedule.Shift = "";
                                            emplSchedule.MealTypeID = 0;
                                            emplSchedule.Date = date;

                                            currentOrder.Add(emplSchedule);
                                        }
                                    }
                                }
                            }
                            else
                            {

                                foreach (EmployeeTO employee in currentEmplArray)
                                {
                                    if (employee.EmployeeID != -1)
                                    {
                                        MealsEmployeeSchedule emplSchedule = new MealsEmployeeSchedule();
                                        emplSchedule.EmployeeID = employee.EmployeeID;
                                        emplSchedule.WorkingUnit = employee.WorkingUnitName;
                                        //  emplSchedule.EmployeeFirstName = employee.FirstName;
                                        emplSchedule.EmployeeLastName = employee.LastName;
                                        emplSchedule.Shift = "";
                                        emplSchedule.MealTypeID = 0;
                                        emplSchedule.Date = date;

                                        currentOrder.Add(emplSchedule);
                                    }
                                }
                            }

                        }

                        if (currentOrder.Count > 0)
                        {
                            startIndex = 0;
                            sortField = 2;
                            currentOrder.Sort(new ArrayListSort(sortOrder, sortField));
                            populateListViewOrders(currentOrder, startIndex, true);
                            this.lblTotal.Visible = true;
                            this.lblTotal.Text = rm.GetString("lblTotal", culture) + currentOrder.Count.ToString().Trim();
                        }
                        else
                        {
                            this.lblTotal.Visible = false;
                            this.lblTotal.Text = "";
                            lvOrders.Items.Clear();
                        }
                    }
                    #endregion
                }
                else
                {
                    string fromTime = "";
                    string toTime = "";
                    fromTime = dtpFrom.Value.ToString("dd.MM.yyyy");
                    toTime = dtpTo.Value.ToString("dd.MM.yyyy");
                    string wuStr = wuString;
                    if (cbWU.SelectedIndex > 0)
                    {
                        if (!chbHierarhicly.Checked)
                        {
                            wuStr = cbWU.SelectedValue.ToString();
                        }
                    }

                    MealUsed mealUsed = new MealUsed();
                    MealsEmployeeSchedule mealsEmplSchedule = new MealsEmployeeSchedule();

                    ArrayList usedMeals = mealUsed.SearchNumberOfUsedMeals(dtpFrom.Value.Date, dtpTo.Value.Date, wuStr);
                    ArrayList orderedMeals = mealsEmplSchedule.SearchNumberOfScheduledMeals(dtpFrom.Value.Date, dtpTo.Value.AddDays(1).Date, wuStr);
                    orderedAndUsed = new ArrayList();
                    noOfUsedOrderedByEmplDic = new Dictionary<int, OrderedUsedMealsCountTO>();
                    noOfUsedOrderedByWUDic = new Dictionary<int, OrderedUsedMealsCountTO>();

                    foreach (MealsEmployeeSchedule mes in orderedMeals)
                    {
                        if (noOfUsedOrderedByEmplDic.ContainsKey(mes.EmployeeID))
                        {
                            noOfUsedOrderedByEmplDic[mes.EmployeeID].NoOfOrdered += 1;
                        }
                        else
                        {
                            OrderedUsedMealsCountTO oum = new OrderedUsedMealsCountTO();
                            oum.EmployeeID = mes.EmployeeID;
                            oum.Employee = mes.EmployeeLastName + " " + mes.EmployeeFirstName;
                            oum.WorkingUnitID = mes.WorkingUnitID;
                            oum.WorkingUnit = mes.WorkingUnit;
                            oum.NoOfOrdered += +1;
                            oum.NoOfUsed += 0;
                            noOfUsedOrderedByEmplDic.Add(oum.EmployeeID, oum);

                        }

                        if (noOfUsedOrderedByWUDic.ContainsKey(mes.WorkingUnitID))
                        {
                            noOfUsedOrderedByWUDic[mes.WorkingUnitID].NoOfOrdered += 1;
                        }
                        else
                        {
                            OrderedUsedMealsCountTO oum = new OrderedUsedMealsCountTO();
                            oum.EmployeeID = -1;
                            oum.Employee = "";
                            oum.WorkingUnitID = mes.WorkingUnitID;
                            oum.WorkingUnit = mes.WorkingUnit;
                            oum.NoOfOrdered += +1;
                            oum.NoOfUsed += 0;
                            noOfUsedOrderedByWUDic.Add(oum.WorkingUnitID, oum);
                        }

                    }
                    
                    foreach (MealUsed mu in usedMeals)
                    {
                        if(noOfUsedOrderedByEmplDic.ContainsKey(mu.EmployeeID))
                        {
                            noOfUsedOrderedByEmplDic[mu.EmployeeID].NoOfUsed += 1;
                        }
                        else
                        {
                            OrderedUsedMealsCountTO oum = new OrderedUsedMealsCountTO();
                            oum.EmployeeID = mu.EmployeeID;
                            oum.Employee = mu.EmployeeName;
                            oum.WorkingUnitID = mu.WorkingUnitID;
                            oum.WorkingUnit = mu.WorkingUnit;
                            oum.NoOfOrdered += 0;
                            oum.NoOfUsed += 1;
                            noOfUsedOrderedByEmplDic.Add(oum.EmployeeID, oum);
                        }

                        if (noOfUsedOrderedByWUDic.ContainsKey(mu.WorkingUnitID))
                        {
                            noOfUsedOrderedByWUDic[mu.WorkingUnitID].NoOfUsed += 1;
                        }
                        else
                        {
                            OrderedUsedMealsCountTO oum = new OrderedUsedMealsCountTO();
                            oum.EmployeeID = -1;
                            oum.Employee = "";
                            oum.WorkingUnitID = mu.WorkingUnitID;
                            oum.WorkingUnit = mu.WorkingUnit;
                            oum.NoOfOrdered += 0;
                            oum.NoOfUsed += 1;
                            noOfUsedOrderedByWUDic.Add(oum.WorkingUnitID, oum);
                        }
                    }

                    foreach (int id in noOfUsedOrderedByEmplDic.Keys)
                    {
                        OrderedUsedMealsCountTO oum = noOfUsedOrderedByEmplDic[id];
                        orderedAndUsed.Add(oum);
                    }

                    if (noOfUsedOrderedByEmplDic.Count > 0)
                    {
                        startIndex = 0;
                        sortField = 0;
                        orderedAndUsed.Sort(new ArrayListSort(sortOrder, sortField));
                        //populateListViewWithUsedAndOrdered(noOfUsedOrderedByEmplDic, startIndex);
                        populateListViewWithUsedAndOrdered(orderedAndUsed, startIndex);
                        this.lblTotal.Visible = true;
                        this.lblTotal.Text = rm.GetString("lblTotal", culture) + orderedAndUsed.Count.ToString().Trim();
                    }
                    else
                    {
                        this.lblTotal.Visible = false;
                        this.lblTotal.Text = "";
                        lvUsedOrdered.Items.Clear();
                    }
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate List View with Meals used found
        /// </summary>
        /// <param name="mealsUsedList"></param>
        private void populateListViewUsed(ArrayList mealsUsedList, int startIndex)
        {
            try
            {
                if (mealsUsedList.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvEmployeeMeals.BeginUpdate();
                lvEmployeeMeals.Items.Clear();

                if (mealsUsedList.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < mealsUsedList.Count))
                    {
                        if (startIndex == 0)
                        {
                            btnPrev.Enabled = false;
                        }
                        else
                        {
                            btnPrev.Enabled = true;
                        }

                        int lastIndex = startIndex + Constants.recordsPerPage;
                        if (lastIndex >= mealsUsedList.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = mealsUsedList.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        for (int i = startIndex; i < lastIndex; i++)
                        {
                           
                            MealUsed meal = (MealUsed)mealsUsedList[i];
                            ListViewItem item = new ListViewItem();
                            item.Text = meal.EmployeeName.ToString().Trim();
                            if (!meal.EventTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                            {
                                item.SubItems.Add(meal.EventTime.ToString("dd/MM/yyyy   HH:mm"));
                            }
                            else
                            {
                                item.SubItems.Add("");
                            }
                            item.SubItems.Add(meal.MealTypeName.Trim());
                           
                            item.SubItems.Add(meal.PointName.Trim());
                          
                            item.Tag = meal;

                            lvEmployeeMeals.Items.Add(item);
                        }
                    }
                }

                lvEmployeeMeals.EndUpdate();
                lvEmployeeMeals.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.populateListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Populate List View with Meals order found
        /// </summary>
        /// <param name="mealsOrderList"></param>
        private void populateListViewOrders(ArrayList mealsOrderList, int startIndex, bool noOrder)
        {
            try
            {
                if (mealsOrderList.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvOrders.BeginUpdate();
                lvOrders.Items.Clear();

                if (mealsOrderList.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < mealsOrderList.Count))
                    {
                        if (startIndex == 0)
                        {
                            btnPrev.Enabled = false;
                        }
                        else
                        {
                            btnPrev.Enabled = true;
                        }

                        int lastIndex = startIndex + Constants.recordsPerPage;
                        if (lastIndex >= mealsOrderList.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = mealsOrderList.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        for (int i = startIndex; i < lastIndex; i++)
                        {

                            MealsEmployeeSchedule schedule = (MealsEmployeeSchedule)mealsOrderList[i];
                            ListViewItem item = new ListViewItem();
                            item.Text = schedule.WorkingUnit.ToString().Trim();
                            if (noOrder)
                            {
                                item.SubItems.Add(schedule.EmployeeLastName.ToString().Trim());
                                item.SubItems.Add(schedule.Date.ToShortDateString());
                                item.SubItems.Add(rm.GetString("noReservation", culture));
                             //   item.SubItems.Add(rm.GetString("noReservation", culture));
                                item.SubItems.Add("");
                            }
                            else
                            {
                                item.SubItems.Add(schedule.EmployeeFirstName.ToString().Trim() + " " + schedule.EmployeeLastName.ToString().Trim());
                                item.SubItems.Add(schedule.Date.ToShortDateString());
                                item.SubItems.Add(schedule.MealsType.Trim());
                                if(schedule.Shift.Equals(""))
                                    item.SubItems.Add("");
                                else
                                    item.SubItems.Add(schedule.Shift.ToString());
                            }
                            item.Tag = schedule;
                            lvOrders.Items.Add(item);
                        }
                    }
                }

                lvOrders.EndUpdate();
                lvOrders.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.populateListViewOrders(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Populate List View with Meals orders and used found
        /// </summary>
        /// <param></param>
        private void populateListViewWithUsedAndOrdered(ArrayList usedOrderedList, int startIndex) //private void populateListViewWithUsedAndOrdered(Dictionary<int, OrderedUsedMealsCountTO> noOfUsedOrderedByEmplDic, int startIndex)
        {
            try
            {
                if (noOfUsedOrderedByEmplDic.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvUsedOrdered.BeginUpdate();
                lvUsedOrdered.Items.Clear();

                if (noOfUsedOrderedByEmplDic.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < noOfUsedOrderedByEmplDic.Count))
                    {
                        if (startIndex == 0)
                        {
                            btnPrev.Enabled = false;
                        }
                        else
                        {
                            btnPrev.Enabled = true;
                        }

                        int lastIndex = startIndex + Constants.recordsPerPage;
                        if (lastIndex >= noOfUsedOrderedByEmplDic.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = noOfUsedOrderedByEmplDic.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        //foreach (int i in noOfUsedOrderedByEmplDic.Keys)
                        for (int i = startIndex; i < lastIndex; i++)
                        {

                            //OrderedUsedMealsCountTO oum = noOfUsedOrderedByEmplDic[i];
                            OrderedUsedMealsCountTO oum = (OrderedUsedMealsCountTO)usedOrderedList[i];
                            ListViewItem item = new ListViewItem();
                            item.Text = oum.WorkingUnit.ToString().Trim();
                            item.SubItems.Add(oum.EmployeeID.ToString());
                            item.SubItems.Add(oum.Employee.ToString().Trim());
                            item.SubItems.Add(oum.NoOfOrdered.ToString().Trim());
                            item.SubItems.Add(oum.NoOfUsed.ToString().Trim());
                            item.Tag = oum;
                            lvUsedOrdered.Items.Add(item);
                        }
                    }
                }

                lvUsedOrdered.EndUpdate();
                lvUsedOrdered.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.populateListViewWithUsedAndOrdered(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        //private void populateListViewNoOrders(List<MealsEmployeeSchedule> noOrdersList, int startIndex, bool noOrder)
        //{
        //    try
        //    {
        //        if (noOrdersList.Count > Constants.recordsPerPage)
        //        {
        //            btnPrev.Visible = true;
        //            btnNext.Visible = true;
        //        }
        //        else
        //        {
        //            btnPrev.Visible = false;
        //            btnNext.Visible = false;
        //        }

        //        lvOrders.BeginUpdate();
        //        lvOrders.Items.Clear();

        //        if (noOrdersList.Count > 0)
        //        {
        //            if ((startIndex >= 0) && (startIndex < noOrdersList.Count))
        //            {
        //                if (startIndex == 0)
        //                {
        //                    btnPrev.Enabled = false;
        //                }
        //                else
        //                {
        //                    btnPrev.Enabled = true;
        //                }

        //                int lastIndex = startIndex + Constants.recordsPerPage;
        //                if (lastIndex >= noOrdersList.Count)
        //                {
        //                    btnNext.Enabled = false;
        //                    lastIndex = noOrdersList.Count;
        //                }
        //                else
        //                {
        //                    btnNext.Enabled = true;
        //                }

        //                for (int i = startIndex; i < lastIndex; i++)
        //                {

        //                    MealsEmployeeSchedule schedule = noOrdersList[i];
        //                    ListViewItem item = new ListViewItem();
        //                    item.Text = schedule.WorkingUnit.ToString().Trim();
        //                    item.SubItems.Add(schedule.EmployeeFirstName.ToString().Trim() + " " + schedule.EmployeeLastName.ToString().Trim());
        //                    item.SubItems.Add(schedule.Date.ToShortDateString());
        //                    item.SubItems.Add(rm.GetString("noReservation", culture));
        //                    item.SubItems.Add(""); 
        //                    item.Tag = schedule;
        //                    lvOrders.Items.Add(item);
        //                }
        //            }
        //        }

        //        lvOrders.EndUpdate();
        //        lvOrders.Invalidate();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.populateListViewOrders(): " + ex.Message + "\n");
        //        throw new Exception(ex.Message);
        //    }
        //}

        private void btnWithoutNext_Click(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex += Constants.recordsPerPage;
                if (rbUsedMeals.Checked)
                    populateListViewUsed(currentMealsUsed, startIndex);
                else if (rbOrders.Checked)
                {
                    if (rbOrderDoesntExist.Checked)
                        populateListViewOrders(currentOrder, startIndex, true);
                    else
                        populateListViewOrders(currentOrder, startIndex, false);
                }
                else if (rbUsedOrdered.Checked)
                    populateListViewWithUsedAndOrdered(orderedAndUsed, startIndex); //populateListViewWithUsedAndOrdered(noOfUsedOrderedByEmplDic, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex -= Constants.recordsPerPage;
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
                if(rbUsedMeals.Checked)
                    populateListViewUsed(currentMealsUsed, startIndex);
                else if (rbOrders.Checked)
                {
                    if (rbOrderDoesntExist.Checked)
                        populateListViewOrders(currentOrder, startIndex, true);
                    else
                        populateListViewOrders(currentOrder, startIndex, false);
                }
                else if (rbUsedOrdered.Checked)
                    populateListViewWithUsedAndOrdered(orderedAndUsed, startIndex); //populateListViewWithUsedAndOrdered(noOfUsedOrderedByEmplDic, startIndex);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.btnPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        
        #region Inner Class for sorting Array List of Passes

        /*
		 *  Class used for sorting Array List of meal used
		*/

        private class ArrayListSort : IComparer
        {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(object x, object y)
            {
                if (whatList == 0)
                {
                    MealUsed mu1 = null;
                    MealUsed mu2 = null;

                    if (compOrder == Constants.sortAsc)
                    {
                        mu1 = (MealUsed)x;
                        mu2 = (MealUsed)y;
                    }
                    else
                    {
                        mu1 = (MealUsed)y;
                        mu2 = (MealUsed)x;
                    }

                    switch (compField)
                    {
                        case MealEmployeeSchedulePreview.EmployeeIndex:
                            return mu1.EmployeeName.CompareTo(mu2.EmployeeName);
                        case MealEmployeeSchedulePreview.TimeIndex:
                            return mu1.EventTime.CompareTo(mu2.EventTime);
                        case MealEmployeeSchedulePreview.MealTypeIndex:
                            return mu1.MealTypeName.CompareTo(mu2.MealTypeName);
                        case MealEmployeeSchedulePreview.MealPointIndex:
                            return mu1.PointName.CompareTo(mu2.PointName);
                        default:
                            return mu1.EventTime.CompareTo(mu2.EventTime);
                    }
                }
                else if (whatList == 1)
                {
                    MealsEmployeeSchedule mu1 = null;
                    MealsEmployeeSchedule mu2 = null;

                    if (compOrder == Constants.sortAsc)
                    {
                        mu1 = (MealsEmployeeSchedule)x;
                        mu2 = (MealsEmployeeSchedule)y;
                    }
                    else
                    {
                        mu1 = (MealsEmployeeSchedule)y;
                        mu2 = (MealsEmployeeSchedule)x;
                    }

                    switch (compField)
                    {
                        case MealEmployeeSchedulePreview.WorkingUnitIndex:
                            return mu1.WorkingUnit.CompareTo(mu2.WorkingUnit);
                        case MealEmployeeSchedulePreview.EmplForOrderIndex:
                            return mu1.EmployeeID.CompareTo(mu2.EmployeeID);
                        case MealEmployeeSchedulePreview.DateIndex:
                            return mu1.Date.CompareTo(mu2.Date);
                        case MealEmployeeSchedulePreview.MealTypeForOrderIndex:
                            return mu1.MealsType.CompareTo(mu2.MealsType);
                        case MealEmployeeSchedulePreview.ShiftIndex:
                            return mu1.Shift.CompareTo(mu2.Shift);
                        default:
                            return mu1.Date.CompareTo(mu2.Date);
                    }
                    
                }
                else
                {
                    OrderedUsedMealsCountTO oum1 = null;
                    OrderedUsedMealsCountTO oum2 = null;

                    if (compOrder == Constants.sortAsc)
                    {
                        oum1 = (OrderedUsedMealsCountTO)x;
                        oum2 = (OrderedUsedMealsCountTO)y;
                    }
                    else
                    {
                        oum1 = (OrderedUsedMealsCountTO)y;
                        oum2 = (OrderedUsedMealsCountTO)x;
                    }

                    switch (compField)
                    {
                        case MealEmployeeSchedulePreview.WorkingUnitNameIndex:
                            return oum1.WorkingUnit.CompareTo(oum2.WorkingUnit);
                        case MealEmployeeSchedulePreview.EmplIDIndex:
                            return oum1.EmployeeID.CompareTo(oum2.EmployeeID);
                        case MealEmployeeSchedulePreview.EmplUsedOrderedIndex:
                            return oum1.Employee.CompareTo(oum2.Employee);
                        case MealEmployeeSchedulePreview.NoOfOrderedIndex:
                            return oum1.NoOfOrdered.CompareTo(oum2.NoOfOrdered);
                        case MealEmployeeSchedulePreview.NoOfUsedIndex:
                            return oum1.NoOfUsed.CompareTo(oum2.NoOfUsed);
                        default:
                            return oum1.WorkingUnit.CompareTo(oum2.WorkingUnit);
                    }

                }
                
            }
        }

        #endregion

        private void lvEmployeeMeals_ColumnClick(object sender, ColumnClickEventArgs e)
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

                currentMealsUsed.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                populateListViewUsed(currentMealsUsed, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.lvEmployeeMeals_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvOrders_ColumnClick(object sender, ColumnClickEventArgs e)
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

                currentOrder.Sort(new ArrayListSort(sortOrder, sortField));
             //   currentMealsUsed.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                if (rbOrderExists.Checked)
                    populateListViewOrders(currentOrder, startIndex, false);
                else
                    populateListViewOrders(currentOrder, startIndex, true);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.lvOrders_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvUsedOrdered_ColumnClick(object sender, ColumnClickEventArgs e)
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

                orderedAndUsed.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                //populateListViewWithUsedAndOrdered(noOfUsedOrderedByEmplDic, startIndex);
                populateListViewWithUsedAndOrdered(orderedAndUsed, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.lvOrders_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.cbWU.SelectedIndex = 0;           
                this.cbEmployee.SelectedIndex = 0;
                this.cbMealType.SelectedIndex = 0;
                this.cbPlace.SelectedIndex = 0;
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbUsedMeals.Checked)
                {
                    #region Used meals
                    if (currentMealsUsed.Count > 0)
                    {
                        // Table Definition for Crystal Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("meals_used");
                        DataTable tableI = new DataTable("images");

                        tableCR.Columns.Add("empl_name", typeof(System.String));
                        tableCR.Columns.Add("event_time", typeof(System.String));
                        tableCR.Columns.Add("meal_type_name", typeof(System.String));
                        tableCR.Columns.Add("point_name", typeof(System.String));
                        tableCR.Columns.Add("imageID", typeof(byte));

                        tableI.Columns.Add("imageID", typeof(byte));
                        tableI.Columns.Add("image", typeof(System.Byte[]));

                        //add logo image just once
                        DataRow rowI = tableI.NewRow();
                        rowI["image"] = Constants.LogoForReport;
                        rowI["imageID"] = 1;
                        tableI.Rows.Add(rowI);
                        tableI.AcceptChanges();

                        dataSetCR.Tables.Add(tableCR);
                        dataSetCR.Tables.Add(tableI);

                        foreach (MealUsed meal in currentMealsUsed)
                        {
                            DataRow row = tableCR.NewRow();

                            row["empl_name"] = meal.EmployeeName;
                            row["event_time"] = meal.EventTime.ToString("dd.MM.yyyy HH:mm");
                            row["point_name"] = meal.PointName;
                            row["meal_type_name"] = meal.MealTypeName;
                            row["imageID"] = 1;

                            tableCR.Rows.Add(row);
                            tableCR.AcceptChanges();
                        }

                        if (tableCR.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Arrow;
                            MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                            return;
                        }

                        string selWorkingUnit = "*";
                        string selEmployee = "*";
                        string selMealPoint = "*";
                        string selMealType = "*";
                        string fromTime = "";
                        string toTime = "";

                        string qty = "";
                        int qtyFrom = (numQtyFrom.Value > 0 ? (int)numQtyFrom.Value : -1);
                        int qtyTo = (numQtyTo.Value > 0 ? (int)numQtyTo.Value : -1);
                        string fromQty = numQtyFrom.Value.ToString();
                        string toQty = numQtyTo.Value > 0 ? numQtyTo.Value.ToString() : rm.GetString("lvUnlimited", culture);

                        qty = fromQty + " - " + toQty;

                        if (cbWU.SelectedIndex >= 0 && (int)cbWU.SelectedValue >= 0)
                            selWorkingUnit = cbWU.Text;
                        if (cbEmployee.SelectedIndex >= 0 && (int)cbEmployee.SelectedValue >= 0)
                            selEmployee = cbEmployee.Text;
                        if (cbMealType.SelectedIndex >= 0 && (int)cbMealType.SelectedValue >= 0)
                            selMealType = cbMealType.Text;
                        if (cbPlace.SelectedIndex > 0)
                            selMealPoint = cbPlace.Text;

                        fromTime = dtpFrom.Value.ToString("dd.MM.yyyy");
                        toTime = dtpTo.Value.ToString("dd.MM.yyyy");
                        if (rbNo.Checked)
                        {
                            if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                            {
                                Reports.Reports_sr.MealsUsedIICRView_sr view = new Reports.Reports_sr.MealsUsedIICRView_sr(dataSetCR,
                                    dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selMealPoint, selMealType, qty);
                                view.ShowDialog(this);
                            }
                            else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                            {
                                Reports.Reports_en.MealsUsedIICRView_en view = new Reports.Reports_en.MealsUsedIICRView_en(dataSetCR,
                                     dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selMealPoint, selMealType, qty);
                                view.ShowDialog(this);
                            }

                        }
                        else
                        {
                            if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                            {
                                Reports.Reports_sr.MealsUsedIISummaryCRView_sr view = new Reports.Reports_sr.MealsUsedIISummaryCRView_sr(dataSetCR,
                                    dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selMealPoint, selMealType, qty);
                                view.ShowDialog(this);
                            }
                            if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                            {
                                Reports.Reports_en.MealsUsedIISummaryCRView_en view = new Reports.Reports_en.MealsUsedIISummaryCRView_en(dataSetCR,
                                    dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selMealPoint, selMealType, qty);
                                view.ShowDialog(this);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }
                    #endregion
                }
                else if (rbOrders.Checked)
                {
                    #region Oredered meals
                    if (currentOrder.Count > 0)
                    {
                        // Table Definition for Crystal Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("meals_empl_schedule");

                        tableCR.Columns.Add("work_unit", typeof(System.String));
                        tableCR.Columns.Add("employee", typeof(System.String));
                        tableCR.Columns.Add("date", typeof(System.String));
                        tableCR.Columns.Add("meal_type", typeof(System.String));

                        dataSetCR.Tables.Add(tableCR);

                        foreach (MealsEmployeeSchedule schedule in currentOrder)
                        {
                            DataRow row = tableCR.NewRow();

                            row["work_unit"] = schedule.WorkingUnit;
                            row["employee"] = schedule.EmployeeFirstName + " " + schedule.EmployeeLastName;
                            row["date"] = schedule.Date.ToShortDateString();
                            if (rbOrderDoesntExist.Checked)
                                row["meal_type"] = rm.GetString("noReservation", culture);
                            else
                                row["meal_type"] = schedule.MealsType;

                            tableCR.Rows.Add(row);
                            tableCR.AcceptChanges();
                        }

                        if (tableCR.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Arrow;
                            MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                            return;
                        }

                        string selWorkingUnit = "*";
                        string selEmployee = "*";
                        string selMealType = "*";
                        string fromTime = "";
                        string toTime = "";
                        bool ordersExists = false;


                        if (cbWU.SelectedIndex >= 0)
                            selWorkingUnit = cbWU.Text;
                        if (cbEmployee.SelectedIndex >= 0 && (int)cbEmployee.SelectedValue >= 0)
                            selEmployee = cbEmployee.Text;
                        if (cbMealType.SelectedIndex >= 0 && (int)cbMealType.SelectedValue >= 0)
                            selMealType = cbMealType.Text;
                        if (rbOrderExists.Checked)
                            ordersExists = true;

                        fromTime = dtpFrom.Value.ToString("dd.MM.yyyy");
                        toTime = dtpTo.Value.ToString("dd.MM.yyyy");

                        if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                        {
                            Reports.Reports_sr.MealEmployeeScheduleCRView_sr view = new Reports.Reports_sr.MealEmployeeScheduleCRView_sr(dataSetCR,
                                dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selMealType, ordersExists);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                        {
                            Reports.Reports_sr.MealEmployeeScheduleCRView_sr view = new Reports.Reports_sr.MealEmployeeScheduleCRView_sr(dataSetCR,
                               dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selMealType, ordersExists);
                            view.ShowDialog(this);
                            //Reports.Reports_en.MealEmployeeScheduleCRView_en view = new Reports.Reports_en.MealEmployeeScheduleCRView_en(dataSetCR,
                            //     dtpFrom.Value, dtpTo.Value, selWorkingUnit, selEmployee, selMealType);
                            //view.ShowDialog(this);
                        }

                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }
                    #endregion
                }
                else // rbUsedOrdered.Checked
                {
                    if (orderedAndUsed.Count == 0)
                    {
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }
                    else
                    {
                        string selWorkingUnit = "*";
                        if (cbWU.SelectedIndex >= 0 && (int)cbWU.SelectedValue >= 0)
                            selWorkingUnit = cbWU.Text;

                        // Table Definition for Crystal Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("details");
                        DataTable tableSUM = new DataTable("summary");

                        tableCR.Columns.Add("workingUnit", typeof(System.String));
                        tableCR.Columns.Add("emplID", typeof(System.String));
                        tableCR.Columns.Add("employee", typeof(System.String));
                        tableCR.Columns.Add("ordered", typeof(System.String));
                        tableCR.Columns.Add("used", typeof(System.String));

                        tableSUM.Columns.Add("workingUnitSum", typeof(System.String));
                        tableSUM.Columns.Add("orderedSum", typeof(System.Int32));
                        tableSUM.Columns.Add("usedSum", typeof(System.Int32));

                        dataSetCR.Tables.Add(tableCR);
                        dataSetCR.Tables.Add(tableSUM);

                        foreach(OrderedUsedMealsCountTO oum in orderedAndUsed)
                        {
                            DataRow row = tableCR.NewRow();
                            row["workingUnit"] = oum.WorkingUnit;
                            row["emplID"] = oum.EmployeeID;
                            row["employee"] = oum.Employee;
                            row["ordered"] = oum.NoOfOrdered;
                            row["used"] = oum.NoOfUsed;
                            tableCR.Rows.Add(row);
                            tableCR.AcceptChanges();
                        }

                        //Dictionary<int, WorkingUnitTO> wuDic = new WorkingUnit().SearchAllWU();
                        //foreach (int wuID in wuDic.Keys)
                        //{
                            //if(noOfUsedOrderedByWUDic.ContainsKey(wuID))
                        foreach(int wuID in noOfUsedOrderedByWUDic.Keys)
                            {
                                OrderedUsedMealsCountTO oum = noOfUsedOrderedByWUDic[wuID];
                                DataRow row = tableSUM.NewRow();
                                row["workingUnitSUM"] = oum.WorkingUnit;
                                row["orderedSUM"] = oum.NoOfOrdered;
                                row["usedSUM"] = oum.NoOfUsed;
                                tableSUM.Rows.Add(row);
                                tableSUM.AcceptChanges();
                            }
                        //}

                        if (tableCR.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Arrow;
                            MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                            return;
                        }

                        if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                        {
                            Reports.Reports_sr.MealOrderedAndUsedCRView view = new Reports.Reports_sr.MealOrderedAndUsedCRView(dataSetCR,
                                dtpFrom.Value, dtpTo.Value, selWorkingUnit);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                        {
                            Reports.Reports_en.MealOrderedAndUsedCRView view = new Reports.Reports_en.MealOrderedAndUsedCRView(dataSetCR,
                               dtpFrom.Value, dtpTo.Value, selWorkingUnit);
                            view.ShowDialog(this);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.btnGenerateReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool check = false;
                foreach (WorkingUnitTO wu in wUnits)
                {
                    if (cbWU.SelectedIndex != 0)
                    {
                        if (wu.WorkingUnitID == (int)cbWU.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                        {
                            if (!chbHierarhicly.Checked)
                            {
                                chbHierarhicly.Checked = true;
                                check = true;
                            }
                        }
                    }
                }

                if (cbWU.SelectedValue is int && !check)
                {
                    populateEmployeeCombo((int)cbWU.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void gbSearchCriteria_Enter(object sender, EventArgs e)
        {

        }
        
        //what list = 0;
        private void rbUsedMeals_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbUsedMeals.Checked)
                {
                    whatList = 0;

                    lvOrders.Items.Clear();
                    lvOrders.Visible = false;
                    lvUsedOrdered.Items.Clear();
                    lvUsedOrdered.Visible = false;
                    lvEmployeeMeals.Visible = true;

                    rbOrders.Checked = false;
                    rbUsedOrdered.Checked = false;
                    gbWithOrWithout.Visible = false;
                    lblEmployee.Visible = lblPlace.Visible = lblMealType.Visible = lblQty.Visible = label1.Visible = lblSumary.Visible = true;
                    cbEmployee.Visible = cbPlace.Visible = cbMealType.Visible = true;
                    rbYes.Visible = rbNo.Visible = true;
                    numQtyFrom.Visible = numQtyTo.Visible = true;
                    btnClear_Click(this, null);
                    this.lblTotal.Visible = false;
                }
                //else
                //{
                //    whatList = 1;
                //    rbOrders.Checked = true;
                //    gbWithOrWithout.Visible = true;
                //    lvEmployeeMeals.Items.Clear();
                //    lvOrders.Visible = true;
                //    lblSumary.Visible = false;
                //    rbYes.Visible = false;
                //    rbNo.Visible = false;
                //    rbOrderDoesntExist_CheckedChanged(this, null);
                //}
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.rbUsedMeals_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        
       //what list = 1;
        private void rbOrders_CheckedChanged(object sender, EventArgs e)
        {
            if(rbOrders.Checked)
            {
                whatList = 1;

                lvEmployeeMeals.Items.Clear();
                lvEmployeeMeals.Visible = false;
                lvUsedOrdered.Items.Clear();
                lvUsedOrdered.Visible = false;
                lvOrders.Visible = true;

                rbUsedMeals.Checked = false;
                rbUsedOrdered.Checked = false;
                gbWithOrWithout.Visible = true;
                lblSumary.Visible = false;
                rbYes.Visible = false;
                rbNo.Visible = false;
                rbOrderDoesntExist_CheckedChanged(this, null);
            }
        }

       //what list = 2;
        private void rbUsedOrdered_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUsedOrdered.Checked)
            {
                whatList = 2;

                lvEmployeeMeals.Items.Clear();
                lvEmployeeMeals.Visible = false;
                lvOrders.Items.Clear();              
                lvOrders.Visible = false;
                lvUsedOrdered.Items.Clear();
                lvUsedOrdered.Visible = true;
               
                rbUsedMeals.Checked = false;
                rbOrders.Checked = false;
                gbWithOrWithout.Visible = false;
                lblSumary.Visible = false;
                rbYes.Visible = false;
                rbNo.Visible = false;
                lblEmployee.Visible = lblPlace.Visible = lblMealType.Visible = lblQty.Visible = label1.Visible = lblSumary.Visible = false;
                cbEmployee.Visible = cbPlace.Visible = cbMealType.Visible = false;
                numQtyFrom.Visible = numQtyTo.Visible = false;
                btnClear_Click(this, null);
                this.lblTotal.Visible = false;
            }
        }
 
        private void rbOrderDoesntExist_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbOrders.Checked && rbOrderDoesntExist.Checked) //bez rezervacije
                {
                    lblEmployee.Visible = lblPlace.Visible = lblMealType.Visible = lblQty.Visible = label1.Visible = false ;
                    cbEmployee.Visible = cbPlace.Visible = cbMealType.Visible = false;
                    numQtyFrom.Visible = numQtyTo.Visible = false;
                    lvOrders.Items.Clear();
                    this.lblTotal.Visible = false;
                    btnClear_Click(this, null);

                }
                else //if( rbOrders.Checked && rbOrderExists.Checked) // sa rezervacijom
                {
                    lblEmployee.Visible = lblMealType.Visible = true;
                    cbEmployee.Visible = cbMealType.Visible = true;
                    lblPlace.Visible = lblQty.Visible = label1.Visible = false;
                    cbPlace.Visible = false;
                    numQtyFrom.Visible = numQtyTo.Visible = false;
                    lvOrders.Items.Clear();
                    this.lblTotal.Visible = false;
                    btnClear_Click(this, null);
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealEmployeeSchedulePreview.rbUsedMeals_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }





    }

    public class OrderedUsedMealsCountTO
    {
        int employeeID = -1;

        public int EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

        int workingUnitID = -1;

        public int WorkingUnitID
        {
            get { return workingUnitID; }
            set { workingUnitID = value; }
        }

        string employee = "";

        public string Employee
        {
            get { return employee; }
            set { employee = value; }
        }

        string workingUnit = "";

        public string WorkingUnit
        {
            get { return workingUnit; }
            set { workingUnit = value; }
        }
        int noOfOrdered = -0;

        public int NoOfOrdered
        {
            get { return noOfOrdered; }
            set { noOfOrdered = value; }
        }
        int noOfUsed = 0;


        public int NoOfUsed
        {
            get { return noOfUsed; }
            set { noOfUsed = value; }
        }

        public OrderedUsedMealsCountTO()
        {
        }



    }
    

}
