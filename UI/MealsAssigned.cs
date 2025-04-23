using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Data;

using TransferObjects;
using Common;
using Util;
using Reports;

namespace UI
{
    public partial class MealsAssigned : UserControl
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;
        MealAssigned currentMealAssigned;
        ArrayList currentMealAssignedList;
        List<WorkingUnitTO> wUnits;
        private string wuString = "";
        private int sortOrder;
        private int sortField;
        private int startIndex;

        // List View indexes
        const int EmplLastNameIndex = 0;
        const int EmplFirstNameIndex = 1;
        const int MealTypeIndex = 2;
        const int ValidFromIndex = 3;
        const int ValidToIndex = 4;
        const int QuantityIndex = 5;
        const int QuantityDailyIndex = 6;
        const int MoneyAmountIndex = 7;

        public MealsAssigned()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();
                currentMealAssigned = new MealAssigned();
                currentMealAssignedList = new ArrayList();

                rm = new ResourceManager("UI.Resource", typeof(MealsAssigned).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();
                
                this.btnPrev.Visible = false;
                this.btnNext.Visible = false;
                this.lblTotal.Text = rm.GetString("lblTotal", culture) + "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void populateListView(ArrayList mealsAssignedList, int startIndex)
        {
            try
            {
                if (mealsAssignedList.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvMealsAssigned.BeginUpdate();
                lvMealsAssigned.Items.Clear();

                if (mealsAssignedList.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < mealsAssignedList.Count))
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
                        if (lastIndex >= mealsAssignedList.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = mealsAssignedList.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            MealAssigned mealAssigned = (MealAssigned)mealsAssignedList[i];
                            ListViewItem item = new ListViewItem();

                            item.Text = mealAssigned.EmployeeLastName;
                            item.SubItems.Add(mealAssigned.EmployeeFirstName);
                            item.SubItems.Add(mealAssigned.MealTypeName);
                            item.SubItems.Add(mealAssigned.ValidFrom.ToString("dd.MM.yyyy"));
                            item.SubItems.Add(mealAssigned.ValidTo.ToString("dd.MM.yyyy"));
                            item.SubItems.Add(mealAssigned.Quantity != Constants.undefined ? (mealAssigned.Quantity != Constants.unlimited ? mealAssigned.Quantity.ToString().Trim() : rm.GetString("lvUnlimited", culture)) : "N/A");
                            item.SubItems.Add(mealAssigned.QuantityDaily != Constants.undefined ? (mealAssigned.QuantityDaily != Constants.unlimited ? mealAssigned.QuantityDaily.ToString().Trim() : rm.GetString("lvUnlimited", culture)) : "N/A");
                            item.SubItems.Add(mealAssigned.MoneyAmount != Constants.undefined ? (mealAssigned.MoneyAmount != Constants.unlimited ? mealAssigned.MoneyAmount.ToString().Trim() : rm.GetString("lvUnlimited", culture)) : "N/A");

                            item.Tag = mealAssigned;
                            lvMealsAssigned.Items.Add(item);
                        }
                    }
                }

                lvMealsAssigned.EndUpdate();
                lvMealsAssigned.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsAssigned.populateListView(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void lvMealsAssigned_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
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
                currentMealAssignedList.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                populateListView(currentMealAssignedList, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsAssigned.lvMealsAssigned_ColumnClick(): " + ex.Message + "\n");
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
                this.Cursor = Cursors.WaitCursor;
                this.cbWorkingUnit.SelectedIndex = 0;
                this.cbEmplName.SelectedIndex = 0;
                this.cbMealType.SelectedIndex = 0;
                this.dtpFrom.Value = DateTime.Now;
                this.dtpTo.Value = DateTime.Now;
                this.numQtyFrom.Value = 0;
                this.numQtyTo.Value = 0;
                this.numQtyDailyFrom.Value = 0;
                this.numQtyDailyTo.Value = 0;
                this.numMoneyAmtFrom.Value = 0;
                this.numMoneyAmtTo.Value = 0;

                currentMealAssigned = new MealAssigned();

                clearListView();
                this.lblTotal.Text = rm.GetString("lblTotal", culture) + "0";
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " MealsAssigned.btnClear_Click((): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int count = 0;
                MealAssigned mealAssigned = new MealAssigned();

                int employeeID = (cbEmplName.SelectedIndex >= 0 ? (int)cbEmplName.SelectedValue : -1);
                int mealTypeID = (cbMealType.SelectedIndex >= 0 ? (int)cbMealType.SelectedValue : -1);
                int qtyFrom = (numQtyFrom.Value > 0 ? (int)numQtyFrom.Value : -1);
                int qtyTo = (numQtyTo.Value > 0 ? (int)numQtyTo.Value : -1);
                int qtyDailyFrom = (numQtyDailyFrom.Value > 0 ? (int)numQtyDailyFrom.Value : -1);
                int qtyDailyTo = (numQtyDailyTo.Value > 0 ? (int)numQtyDailyTo.Value : -1);
                int mAmtFrom = (numMoneyAmtFrom.Value > 0 ? (int)numMoneyAmtFrom.Value : -1);
                int mAmtTo = (numMoneyAmtTo.Value > 0 ? (int)numMoneyAmtTo.Value : -1);

                if (!wuString.Equals(""))
                {
                    count = mealAssigned.SearchCount(employeeID, mealTypeID, dtpFrom.Value.Date, dtpTo.Value.Date,
                        qtyFrom, qtyTo, qtyDailyFrom, qtyDailyTo, mAmtFrom, mAmtTo, cbUnlimited.Checked);
                    
                    if (count > Constants.maxRecords)
                    {
                        DialogResult result = MessageBox.Show(rm.GetString("MealsAssignedGreaterThenAllowed", culture), "", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            currentMealAssignedList = mealAssigned.Search(employeeID, mealTypeID, dtpFrom.Value.Date, dtpTo.Value.Date,
                        qtyFrom, qtyTo, qtyDailyFrom, qtyDailyTo, mAmtFrom, mAmtTo, cbUnlimited.Checked);
                        }
                        else
                        {
                            currentMealAssignedList.Clear();
                            clearListView();
                        }
                    }
                    else
                    {
                        if (count > 0)
                        {
                            currentMealAssignedList = mealAssigned.Search(employeeID, mealTypeID, dtpFrom.Value.Date, dtpTo.Value.Date,
                                qtyFrom, qtyTo, qtyDailyFrom, qtyDailyTo, mAmtFrom, mAmtTo, cbUnlimited.Checked);
                        }
                        else
                        {
                            currentMealAssignedList.Clear();
                            clearListView();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noMealsUsedFound", culture));
                    return;
                }

                if (currentMealAssignedList.Count > 0)
                {
                    startIndex = 0;
                    currentMealAssignedList.Sort(new ArrayListSort(sortOrder, sortField));
                    populateListView(currentMealAssignedList, startIndex);
                    this.lblTotal.Visible = true;
                    this.lblTotal.Text = rm.GetString("lblTotal", culture) + currentMealAssignedList.Count.ToString().Trim();
                }
                else
                {
                    MessageBox.Show(rm.GetString("noMealsAssignedFound", culture));
                    clearListView();
                    this.lblTotal.Text = rm.GetString("lblTotal", culture) + "0";
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " MealsAssigned.btnSearch_Click((): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void clearListView()
        {
            try
            {
                lvMealsAssigned.BeginUpdate();
                lvMealsAssigned.Items.Clear();
                lvMealsAssigned.EndUpdate();

                lvMealsAssigned.Invalidate();

                btnPrev.Visible = false;
                btnNext.Visible = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsAssigned.clearListView(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                MealAssignedAdd maa = new MealAssignedAdd();
                maa.ShowDialog();

                btnClear.PerformClick();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsAssigned.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void MealsAssigned_Load(object sender, EventArgs e)
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
                populateWorkigUnitCombo();
                
                populateEmployeeCombo(-1);

                MealType mealType = new MealType();
                ArrayList mealTypeList = new ArrayList();
                mealTypeList = mealType.Search(-1, "", "", new DateTime(), new DateTime());
                populateMealTypeCombo(mealTypeList);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " MealAssigned.btnClear_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvMealsAssigned_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lvMealsAssigned.SelectedItems.Count > 0)
                {
                    cbEmplName.SelectedIndex = cbEmplName.FindStringExact(lvMealsAssigned.SelectedItems[0].Text + " " + lvMealsAssigned.SelectedItems[0].SubItems[1].Text);
                    cbMealType.SelectedIndex = cbMealType.FindStringExact(lvMealsAssigned.SelectedItems[0].SubItems[2].Text);
                    dtpFrom.Value = DateTime.Parse(lvMealsAssigned.SelectedItems[0].SubItems[3].Text, culture);
                    dtpTo.Value = DateTime.Parse(lvMealsAssigned.SelectedItems[0].SubItems[4].Text, culture);
                    decimal qty = 0;
                    decimal qtyDaily = 0;
                    decimal mAmt = 0;
                    Decimal.TryParse(lvMealsAssigned.SelectedItems[0].SubItems[5].Text, out qty);
                    Decimal.TryParse(lvMealsAssigned.SelectedItems[0].SubItems[6].Text.Trim(), out qtyDaily);
                    Decimal.TryParse(lvMealsAssigned.SelectedItems[0].SubItems[7].Text, out mAmt);
                    numQtyFrom.Value = numQtyTo.Value = qty;
                    numQtyDailyFrom.Value = numQtyDailyTo.Value = qtyDaily;
                    numMoneyAmtFrom.Value = numMoneyAmtTo.Value = mAmt;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsAssigned.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
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

                cbEmplName.DataSource = emplArray;
                cbEmplName.DisplayMember = "LastName";
                cbEmplName.ValueMember = "EmployeeID";
                cbEmplName.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsAssigned.populateEmployeeCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void populateWorkigUnitCombo()
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
                log.writeLog(DateTime.Now + " MealsAssigned.populateWorkigUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void populateMealTypeCombo(ArrayList array)
        {
            try
            {
                MealType mealType = new MealType();
                mealType.Name = rm.GetString("all", culture);
                array.Insert(0, mealType);

                this.cbMealType.DataSource = array;
                this.cbMealType.DisplayMember = "Name";
                this.cbMealType.ValueMember = "MealTypeID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsAssigned.populateMealTypeCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void setLanguage()
        {
            try
            {
                this.gbSearch.Text = rm.GetString("gbSearch", culture);

                this.lblWorkingUnit.Text = rm.GetString("lblWorkingUnit", culture);
                this.lblEmployee.Text = rm.GetString("lblEmployee", culture);
                this.lblMealType.Text = rm.GetString("lblMealType", culture);
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);
                this.lblQty.Text = rm.GetString("lblQty", culture);
                this.lblQtyDaily.Text = rm.GetString("lblQtyDaily", culture);
                this.lblMoneyAmt.Text = rm.GetString("lblMoneyAmt", culture);

                this.btnSearch.Text = rm.GetString("btnSearch", culture);
                this.btnAdd.Text = rm.GetString("btnAdd", culture);
                this.btnDelete.Text = rm.GetString("btnDelete", culture);
                this.btnClear.Text = rm.GetString("btnClear", culture);

                this.cbUnlimited.Text = rm.GetString("unlimited", culture);

                // List View Header
                this.lvMealsAssigned.BeginUpdate();

                lvMealsAssigned.Columns.Add(rm.GetString("hdrLastName", culture), (lvMealsAssigned.Width - 8) / 8 - 3, HorizontalAlignment.Left);
                lvMealsAssigned.Columns.Add(rm.GetString("hdrFirstName", culture), (lvMealsAssigned.Width - 8) / 8 - 3, HorizontalAlignment.Left);
                lvMealsAssigned.Columns.Add(rm.GetString("hdrMealType", culture), (lvMealsAssigned.Width - 8) / 8 - 3, HorizontalAlignment.Left);
                lvMealsAssigned.Columns.Add(rm.GetString("hdrValidFrom", culture), (lvMealsAssigned.Width - 8) / 8 - 3, HorizontalAlignment.Left);
                lvMealsAssigned.Columns.Add(rm.GetString("hdrValidTo", culture), (lvMealsAssigned.Width - 8) / 8 - 3, HorizontalAlignment.Left);
                lvMealsAssigned.Columns.Add(rm.GetString("hdrQuantity", culture), (lvMealsAssigned.Width - 8) / 8 - 3, HorizontalAlignment.Right);
                lvMealsAssigned.Columns.Add(rm.GetString("hdrQuantityDaily", culture), (lvMealsAssigned.Width - 8) / 8 - 3, HorizontalAlignment.Right);
                lvMealsAssigned.Columns.Add(rm.GetString("hdrMoneyAmount", culture), (lvMealsAssigned.Width - 8) / 8 - 3, HorizontalAlignment.Right);

                lvMealsAssigned.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealsAssigned.populateMealTypeCombo(): " + ex.Message + "\n");
                throw ex;
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
                populateListView(currentMealAssignedList, startIndex);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " MealsAssigned.btnPrev_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                startIndex += Constants.recordsPerPage;
                populateListView(currentMealAssignedList, startIndex);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " MealsAssigned.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        #region Inner Class for sorting Array List of Employees

        /*
		 *  Class used for sorting Array List of Employees
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
                MealAssigned ma1 = null;
                MealAssigned ma2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    ma1 = (MealAssigned)x;
                    ma2 = (MealAssigned)y;
                }
                else
                {
                    ma1 = (MealAssigned)y;
                    ma2 = (MealAssigned)x;
                }

                switch (compField)
                {
                    case MealsAssigned.EmplLastNameIndex:
                        return ma1.EmployeeLastName.CompareTo(ma2.EmployeeLastName);
                    case MealsAssigned.EmplFirstNameIndex:
                        return ma1.EmployeeFirstName.CompareTo(ma2.EmployeeFirstName);
                    case MealsAssigned.MealTypeIndex:
                        return ma1.MealTypeName.CompareTo(ma2.MealTypeName);
                    case MealsAssigned.ValidFromIndex:
                        return ma1.ValidFrom.CompareTo(ma2.ValidFrom);
                    case MealsAssigned.ValidToIndex:
                        return ma1.ValidTo.CompareTo(ma2.ValidTo);
                    case MealsAssigned.QuantityIndex:
                        return ma1.Quantity.CompareTo(ma2.Quantity);
                    case MealsAssigned.QuantityDailyIndex:
                        return ma1.QuantityDaily.CompareTo(ma2.QuantityDaily);
                    case MealsAssigned.MoneyAmountIndex:
                        return ma1.MoneyAmount.CompareTo(ma2.MoneyAmount);
                    default:
                        return ma1.EmployeeLastName.CompareTo(ma2.EmployeeLastName);
                }
            }
        }

        #endregion

        private void cbWorkingUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool check = false;
                if (!chbHierarhicly.Checked)
                {
                    foreach (WorkingUnitTO wu in wUnits)
                    {
                        if (cbWorkingUnit.SelectedIndex != 0)
                        {
                            if (wu.WorkingUnitID == (int)cbWorkingUnit.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
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
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " MealsAssigned.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool isDeleted = true;

                if (lvMealsAssigned.SelectedItems.Count > 0)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("messageDeleteMealAssigned", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        foreach (ListViewItem item in lvMealsAssigned.SelectedItems)
                        {
                            isDeleted = currentMealAssigned.Delete(((MealAssigned)(item.Tag)).RecID.ToString()) && isDeleted;
                            if (!isDeleted)
                                break;
                        }

                        if (isDeleted)
                        {
                            MessageBox.Show(rm.GetString("mealAssignedDeleted", culture));
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("mealAssignedNotDeleted", culture));
                        }

                        btnClear.PerformClick();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("SelectMealAssigned", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealPoints.btnDelete_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MealPoints.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " MealsAssigned.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
