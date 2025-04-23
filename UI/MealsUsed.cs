using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

using TransferObjects;
using Common;
using Util;

namespace UI
{
    public partial class MealsUsed : UserControl
    {
        private MealUsed currentMeal = null;

        // List View indexes
        const int EmployeeIndex = 0;
        const int EventTimeIndex = 1;
        const int PointIndex = 2;
        const int MealTypeIndex = 3;
        const int QtyIndex = 4;
        const int MoneyAmtIndex = 5;

        private List<WorkingUnitTO> wUnits;
        private string wuString = "";
        ArrayList mealsList;

        private ListViewItemComparer _comp;

        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        // report data
        string selWorkingUnit = "*";
        string selEmployee = "*";
        string selPoint = "*";
        string selMealType = "*";
        string fromQty = "";
        string toQty = "";
        string fromMoneyAmt = "";
        string toMoneyAmt = "";
        string qty = "";
        string moneyAmt = "";

        public MealsUsed()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(MealsUsed).Assembly);
                
                setLanguage();
                
                currentMeal = new MealUsed();
                logInUser = NotificationController.GetLogInUser();
                mealsList = new ArrayList();

                this.rbNo.Checked = true;
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
            private CultureInfo culture;

            public ListViewItemComparer(ListView lv)
            {
                _listView = lv;
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
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
                    case MealsUsed.QtyIndex:
                    case MealsUsed.MoneyAmtIndex:
                        {
                            int firstID = -1;
                            int secondID = -1;

                            if (!sub1.Text.Trim().Equals(""))
                            {
                                firstID = Int32.Parse(sub1.Text.Trim());
                            }

                            if (!sub2.Text.ToString().Trim().Equals(""))
                            {
                                secondID = Int32.Parse(sub2.Text.Trim());
                            }

                            return CaseInsensitiveComparer.Default.Compare(firstID, secondID);
                        }
                    case MealsUsed.EmployeeIndex:
                    case MealsUsed.PointIndex:
                    case MealsUsed.MealTypeIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    case MealsUsed.EventTimeIndex:
                        {
                            DateTime date1 = DateTime.Parse(item1.SubItems[1].Text.Trim(), culture);
                            DateTime date2 = DateTime.Parse(item2.SubItems[1].Text.Trim(), culture);
                            return CaseInsensitiveComparer.Default.Compare(date1, date2);
                        }                        
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");
                }
            }
        }

        #endregion

        #region Inner Class for sorting Array List of MealsUsed

        /*
		 *  Class used for sorting Array List of MealsUsed
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
                MealUsed ma1 = null;
                MealUsed ma2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    ma1 = (MealUsed)x;
                    ma2 = (MealUsed)y;
                }
                else
                {
                    ma1 = (MealUsed)y;
                    ma2 = (MealUsed)x;
                }

                switch (compField)
                {
                    case MealsUsed.EmployeeIndex:
                        return ma1.EmployeeName.CompareTo(ma2.EmployeeName);
                    case MealsUsed.EventTimeIndex:
                        return ma1.EventTime.CompareTo(ma2.EventTime);
                    case MealsUsed.PointIndex:
                        return ma1.PointName.CompareTo(ma2.PointName);
                    case MealsUsed.MealTypeIndex:
                        return ma1.MealTypeName.CompareTo(ma2.MealTypeName);
                    case MealsUsed.QtyIndex:
                        return ma1.Quantity.CompareTo(ma2.Quantity);
                    case MealsUsed.MoneyAmtIndex:
                        return ma1.MoneyAmount.CompareTo(ma2.MoneyAmount);
                    default:
                        return ma1.EmployeeName.CompareTo(ma2.EmployeeName);
                }
            }
        }

        #endregion

        private void setLanguage()
        {
            try
            {
                // form text
                this.Text = rm.GetString("MealsUsedForm", culture);

                // group box's text
                this.gbMealsUsed.Text = rm.GetString("gbMealsUsed", culture);
                this.gbReport.Text = rm.GetString("gbReport", culture);

                // label's text
                this.lblWU.Text = rm.GetString("lblWU", culture);
                this.lblEmpl.Text = rm.GetString("lblEmployee", culture);
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);
                this.lblPoint.Text = rm.GetString("lblPoint", culture);
                this.lblMealType.Text = rm.GetString("lblMealType", culture);
                this.lblQty.Text = rm.GetString("lblQty", culture);
                this.lblMoneyAmt.Text = rm.GetString("lblMoneyAmt", culture);
                this.lblSummary.Text = rm.GetString("lblSummary", culture);

                // radio button's text
                this.rbYes.Text = rm.GetString("yes", culture);
                this.rbNo.Text = rm.GetString("no", culture);

                // button's text
                this.btnClear.Text = rm.GetString("btnClear", culture);
                this.btnSearch.Text = rm.GetString("btnSearch", culture);
                this.btnGenerateReport.Text = rm.GetString("btnGenerateReport", culture);
                
                // list view
                lvMealsUsed.BeginUpdate();
                lvMealsUsed.Columns.Add(rm.GetString("hdrEmployee", culture), (lvMealsUsed.Width - 6) / 6 - 4, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrEventTime", culture), (lvMealsUsed.Width - 6) / 6 - 4, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrPoint", culture), (lvMealsUsed.Width - 6) / 6 - 4, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrMealType", culture), (lvMealsUsed.Width - 6) / 6 - 4, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrQuantity", culture), (lvMealsUsed.Width - 6) / 6 - 4, HorizontalAlignment.Right);
                lvMealsUsed.Columns.Add(rm.GetString("hdrMoneyAmount", culture), (lvMealsUsed.Width - 6) / 6 - 4, HorizontalAlignment.Right);
                lvMealsUsed.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsUsed.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void MealsUsed_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

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
                }

                // Initialize comparer object
                _comp = new ListViewItemComparer(lvMealsUsed);
                lvMealsUsed.ListViewItemSorter = _comp;
                lvMealsUsed.Sorting = SortOrder.Ascending;

                populateWorkingUnitCombo();
                populateEmployeeCombo(-1);
                populatePointCombo();
                populateMealTypeCombo();

                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " MealsUsed.MealsUsed_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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

                cbWU.DataSource = wuArray;
                cbWU.DisplayMember = "Name";
                cbWU.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsUsed.populateWorkingUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Populate Point Combo Box
        /// </summary>
        private void populatePointCombo()
        {
            try
            {
                ArrayList pointArray = new MealPoint().Search(-1, "", "", "");
                pointArray.Insert(0, new MealPoint(-1, "", rm.GetString("all", culture), rm.GetString("all", culture)));

                cbPoint.DataSource = pointArray;
                cbPoint.DisplayMember = "Name";
                cbPoint.ValueMember = "MealPointID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsUsed.populatePointCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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

                cbMealType.DataSource = mealTypeArray;
                cbMealType.DisplayMember = "Name";
                cbMealType.ValueMember = "MealTypeID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsUsed.populateMealTypeCombo(): " + ex.Message + "\n");
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
                List<EmployeeTO> emplArray = new List<EmployeeTO>();

                string workUnitID = wuID.ToString();
                if (wuID == -1)
                {
                    emplArray = new Employee().SearchByWU(wuString);
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
                    emplArray = new Employee().SearchByWU(workUnitID);
                }

                foreach (EmployeeTO employee in emplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                emplArray.Insert(0, empl);

                cbEmpl.DataSource = emplArray;
                cbEmpl.DisplayMember = "LastName";
                cbEmpl.ValueMember = "EmployeeID";
                cbEmpl.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsUsed.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool check = false;
                if (!chbHierarhicly.Checked)
                {
                    foreach (WorkingUnitTO wu in wUnits)
                    {
                        if (cbWU.SelectedIndex != 0)
                        {
                            if (wu.WorkingUnitID == (int)cbWU.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
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
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsUsed.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                cbWU.SelectedIndex = 0;
                cbEmpl.SelectedIndex = 0;
                cbPoint.SelectedIndex = 0;
                cbMealType.SelectedIndex = 0;
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
                numMoneyAmtFrom.Value = 0;
                numMoneyAmtTo.Value = 0;
                numQtyFrom.Value = 0;
                numQtyTo.Value = 0;

                mealsList = new ArrayList();
                lvMealsUsed.Items.Clear();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsUsed.btnClear_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //validate
                /*if (dtpFrom.Value.Date > dtpTo.Value.Date)
                {
                    MessageBox.Show(rm.GetString("msgToTimeLessThenFromTime", culture));
                    return;
                }

                if (numQtyFrom.Value != 0 && numQtyTo.Value != 0 && numQtyFrom.Value > numQtyTo.Value)
                {
                    MessageBox.Show(rm.GetString("QtyFromGreater", culture));
                    return;
                }

                if (numMoneyAmtFrom.Value != 0 && numMoneyAmtTo.Value != 0 && numMoneyAmtFrom.Value > numMoneyAmtTo.Value)
                {
                    MessageBox.Show(rm.GetString("moneyAmtFromGreater", culture));
                    return;
                }*/

                int employeeID = (cbEmpl.SelectedIndex >= 0 ? (int)cbEmpl.SelectedValue : -1);
                int pointID = (cbPoint.SelectedIndex >= 0 ? (int)cbPoint.SelectedValue : -1);
                int mealTypeID = (cbMealType.SelectedIndex >= 0 ? (int)cbMealType.SelectedValue : -1);
                int qtyFrom = (numQtyFrom.Value > 0 ? (int)numQtyFrom.Value : -1);
                int qtyTo = (numQtyTo.Value > 0 ? (int)numQtyTo.Value : -1);
                int mAmtFrom = (numMoneyAmtFrom.Value > 0 ? (int)numMoneyAmtFrom.Value : -1);
                int mAmtTo = (numMoneyAmtTo.Value > 0 ? (int)numMoneyAmtTo.Value : -1);

                if (wuString.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("noMealsUsedFound", culture));
                }
                else
                {
                    mealsList = new MealUsed().Search(employeeID, dtpFrom.Value.Date, dtpTo.Value.Date,
                        pointID, mealTypeID, qtyFrom, qtyTo, mAmtFrom, mAmtTo, wuString);

                    if (mealsList.Count == 0)
                    {
                        MessageBox.Show(rm.GetString("noMealsUsedFound", culture));
                    }
                    else
                    {
                        // set report parameters
                        if (cbWU.SelectedIndex >= 0)
                            selWorkingUnit = cbWU.Text;
                        if (cbEmpl.SelectedIndex >= 0)
                            selEmployee = cbEmpl.Text;
                        if (cbPoint.SelectedIndex >= 0)
                            selPoint = cbPoint.Text;
                        if (cbMealType.SelectedIndex >= 0)
                            selMealType = cbMealType.Text;
                        fromQty = numQtyFrom.Value.ToString();
                        toQty = numQtyTo.Value > 0 ? numQtyTo.Value.ToString() : rm.GetString("lvUnlimited", culture);
                        fromMoneyAmt = numMoneyAmtFrom.Value.ToString();
                        toMoneyAmt = numMoneyAmtTo.Value > 0 ? numMoneyAmtTo.Value.ToString() : rm.GetString("lvUnlimited", culture);
                        qty = fromQty + " - " + toQty;
                        moneyAmt = fromMoneyAmt + " - " + toMoneyAmt;

                        populateListView();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsUsed.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate List View with meals used found
        /// </summary>
        /// <param name="locationsList"></param>
        public void populateListView()
        {
            try
            {
                lvMealsUsed.BeginUpdate();
                lvMealsUsed.Items.Clear();

                if (mealsList.Count > 0)
                {
                    foreach (MealUsed meal in mealsList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = meal.EmployeeName.Trim();
                        item.SubItems.Add(meal.EventTime.ToString("dd.MM.yyyy HH:mm").Trim());
                        item.SubItems.Add(meal.PointName.Trim());
                        item.SubItems.Add(meal.MealTypeName.Trim());
                        item.SubItems.Add(meal.Quantity != Constants.undefined ? meal.Quantity.ToString().Trim() : "N/A");
                        item.SubItems.Add(meal.MoneyAmount != Constants.undefined ? meal.MoneyAmount.ToString().Trim() : "N/A");
                        item.Tag = meal.TransID;
                        item.ToolTipText = "Trans ID: " + meal.TransID.Trim();

                        lvMealsUsed.Items.Add(item);
                    }
                }

                lvMealsUsed.EndUpdate();
                lvMealsUsed.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsUsed.populateListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void lvMealsUsed_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lvMealsUsed.SelectedItems.Count > 0)
                {
                    cbEmpl.SelectedIndex = cbEmpl.FindStringExact(lvMealsUsed.SelectedItems[0].Text);
                    dtpFrom.Value = DateTime.Parse(lvMealsUsed.SelectedItems[0].SubItems[1].Text, culture);
                    dtpTo.Value = DateTime.Parse(lvMealsUsed.SelectedItems[0].SubItems[1].Text, culture);
                    cbPoint.SelectedIndex = cbPoint.FindStringExact(lvMealsUsed.SelectedItems[0].SubItems[2].Text);
                    cbMealType.SelectedIndex = cbMealType.FindStringExact(lvMealsUsed.SelectedItems[0].SubItems[3].Text);
                    decimal qty = 0;
                    decimal mAmt = 0;
                    Decimal.TryParse(lvMealsUsed.SelectedItems[0].SubItems[4].Text, out qty);
                    Decimal.TryParse(lvMealsUsed.SelectedItems[0].SubItems[5].Text.Trim(), out mAmt);
                    numQtyFrom.Value = numQtyTo.Value = qty;
                    numMoneyAmtFrom.Value = numMoneyAmtTo.Value = mAmt;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsUsed.lvMealsUsed_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvMealsUsed_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                SortOrder prevOrder = lvMealsUsed.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvMealsUsed.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvMealsUsed.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvMealsUsed.Sorting = SortOrder.Ascending;
                }

                lvMealsUsed.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsUsed.lvMealsUsed_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (mealsList.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("meals_used");
                    DataTable tableI = new DataTable("images");

                    tableCR.Columns.Add("event_time", typeof(System.String));
                    tableCR.Columns.Add("empl_name", typeof(System.String));
                    tableCR.Columns.Add("point_name", typeof(System.String));
                    tableCR.Columns.Add("imageID", typeof(byte));
                    tableCR.Columns.Add("meal_type_name", typeof(System.String));
                    tableCR.Columns.Add("qty", typeof(int));
                    tableCR.Columns.Add("money_amt", typeof(int));

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

                    foreach (MealUsed meal in mealsList)
                    {
                        DataRow row = tableCR.NewRow();

                        row["event_time"] = meal.EventTime.ToString("dd.MM.yyyy   HH:mm");
                        row["empl_name"] = meal.EmployeeName;
                        row["point_name"] = meal.PointName;
                        row["meal_type_name"] = meal.MealTypeName;
                        row["qty"] = meal.Quantity;
                        row["money_amt"] = meal.MoneyAmount;

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

                    if (rbNo.Checked)
                    {
                        if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                        {
                            Reports.Reports_sr.MealsUsedCRView_sr view = new Reports.Reports_sr.MealsUsedCRView_sr(dataSetCR,
                                dtpFrom.Value.Date, dtpTo.Value.Date, selWorkingUnit, selEmployee, selPoint,
                                selMealType, qty, moneyAmt);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                        {
                            Reports.Reports_en.MealsUsedCRView_en view = new Reports.Reports_en.MealsUsedCRView_en(dataSetCR,
                                dtpFrom.Value.Date, dtpTo.Value.Date, selWorkingUnit, selEmployee, selPoint,
                                selMealType, qty, moneyAmt);
                            view.ShowDialog(this);
                        }
                    }
                    else if (rbYes.Checked)
                    {
                        if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                        {
                            Reports.Reports_sr.MealsUsedSummaryCRView_sr view = new Reports.Reports_sr.MealsUsedSummaryCRView_sr(dataSetCR,
                                dtpFrom.Value.Date, dtpTo.Value.Date, selWorkingUnit, selEmployee, selPoint,
                                selMealType, qty, moneyAmt);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                        {
                            Reports.Reports_en.MealsUsedSummaryCRView_en view = new Reports.Reports_en.MealsUsedSummaryCRView_en(dataSetCR,
                                dtpFrom.Value.Date, dtpTo.Value.Date, selWorkingUnit, selEmployee, selPoint,
                                selMealType, qty, moneyAmt);
                            view.ShowDialog(this);
                        }
                    }
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " MealsUsed.btnGenerateReport_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MealsUsed.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                if (cbWU.SelectedValue is int)
                {
                    if ((int)cbWU.SelectedValue >= 0)
                        populateEmployeeCombo((int)cbWU.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsUsed.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}