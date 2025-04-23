using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;
using Reports;

namespace UI
{
    public partial class MealAssignedAdd : Form
    {
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;

        //current working units
        List<WorkingUnitTO> wUnits;
        private string wuString = "";

        //toolTips
        ToolTip ttSelectEmpl;
        ToolTip ttSelectPeriod;
        ToolTip ttSelectAssignType;

        DateTime firstDayNextMonth;
        DateTime lastDayNextMonth;

        //Current Employee
        protected Employee currentEmployee;
        protected List<EmployeeTO> currentEmployeesList;

        DateTime validFrom;
        DateTime validTo;
        MealType selMealType;

        public MealAssignedAdd()
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                logInUser = NotificationController.GetLogInUser();
                ttSelectAssignType = new ToolTip();
                ttSelectEmpl = new ToolTip();
                ttSelectPeriod = new ToolTip();

                rm = new ResourceManager("UI.Resource", typeof(MealsAssigned).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                setLanguage();

                firstDayNextMonth = DateTime.Today.AddMonths(1);
                firstDayNextMonth = new DateTime(firstDayNextMonth.Year, firstDayNextMonth.Month, 1);
                lastDayNextMonth = firstDayNextMonth.AddMonths(1);
                lastDayNextMonth = lastDayNextMonth.AddDays(-1);
                dtpFrom.Value = firstDayNextMonth;
                dtpTo.Value = lastDayNextMonth;
                
                selMealType = new MealType();
                currentEmployee = new Employee();
                currentEmployeesList = new List<EmployeeTO>();
                validFrom = new DateTime(1900, 1, 1);
                validTo = new DateTime(2099, 1, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void setLanguage()
        {
            try
            {
                //form text
                this.Text = rm.GetString("MealAssignedAddTitle", culture);

                //goup box's text
                this.gbAssignmentType.Text = rm.GetString("gbAssignmentType", culture);
                this.gbEmployees.Text = rm.GetString("gbEmployees", culture);
                this.gbValidity.Text = rm.GetString("gbValidity", culture);
                this.gbMealType.Text = rm.GetString("gbMealType", culture);

                //label's text
                this.lblWorkingUnit.Text = rm.GetString("lblWorkingUnit", culture);
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);
                this.lblNumber.Text = rm.GetString("lblNumber", culture);
                this.lblDailyLimit.Text = rm.GetString("lblDailyLimit", culture);
                this.lblAmount.Text = rm.GetString("lblAmount", culture);
                this.lblMealType.Text = rm.GetString("lblMealType", culture);

                //radio button's
                this.rbMeals.Text = rm.GetString("rbMeals", culture);
                this.rbMoney.Text = rm.GetString("rbMoney", culture);

                //button's text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnClear.Text = rm.GetString("btnClear", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);

                //check box's text
                this.chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);
                this.chbUnlimited.Text = rm.GetString("chbUnlimited", culture);
                this.chbUnlimitedMeals.Text = rm.GetString("chbUnlimited", culture);
                this.chbUnlimitedMoney.Text = rm.GetString("chbUnlimited", culture);
                this.chbUnlimitedDailyQTY.Text = rm.GetString("chbUnlimited", culture);

                //Tool tip's text
                this.ttSelectEmpl.SetToolTip(this.lvEmployees, rm.GetString("ttSelectEmpl", culture));
                this.ttSelectPeriod.SetToolTip(this.gbValidity, rm.GetString("ttSelectPeriod", culture));
                this.ttSelectAssignType.SetToolTip(this.gbAssignmentType, rm.GetString("ttSelectAssignType", culture));

                // List View Header
                this.lvAssignMeals.BeginUpdate();

                lvAssignMeals.Columns.Add(rm.GetString("lblEmployeeID", culture), (lvAssignMeals.Width - 6) / 8, HorizontalAlignment.Left);
                lvAssignMeals.Columns.Add(rm.GetString("hdrEmployee", culture), (lvAssignMeals.Width - 6) / 8, HorizontalAlignment.Left);
                lvAssignMeals.Columns.Add(rm.GetString("hdrFrom", culture), (lvAssignMeals.Width - 6) / 8, HorizontalAlignment.Left);
                lvAssignMeals.Columns.Add(rm.GetString("hdrTo", culture), (lvAssignMeals.Width - 6) / 8, HorizontalAlignment.Left);
                lvAssignMeals.Columns.Add(rm.GetString("hdrMealType", culture), (lvAssignMeals.Width - 6) / 8, HorizontalAlignment.Left);
                lvAssignMeals.Columns.Add(rm.GetString("hdrAssignmentType", culture), (lvAssignMeals.Width - 6) / 8, HorizontalAlignment.Left);
                lvAssignMeals.Columns.Add(rm.GetString("hdrQuantity", culture), (lvAssignMeals.Width - 6) / 8, HorizontalAlignment.Left);
                lvAssignMeals.Columns.Add(rm.GetString("hdrDailyLimit", culture), (lvAssignMeals.Width - 6) / 8, HorizontalAlignment.Left);

                lvAssignMeals.EndUpdate();

                this.lvEmployees.BeginUpdate();

                lvEmployees.Columns.Add(rm.GetString("lblEmployeeID", culture), ((lvEmployees.Width) / 2 - 40), HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrEmployee", culture), ((lvEmployees.Width) / 2 + 20), HorizontalAlignment.Left);

                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssignedAdd.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                cbWorkingUnit.SelectedIndex = 0;
                chbHierarhicly.Checked = true;
                dtpFrom.Value = firstDayNextMonth;
                dtpTo.Value = lastDayNextMonth;
                chbUnlimited.Checked = false;
                rbMeals.Checked = true;
                chbUnlimitedMeals.Checked = false;
                chbUnlimitedDailyQTY.Checked = false;
                chbUnlimitedMoney.Checked = false;
                nudQuantity.Value = 22;
                nudQuantityDaily.Value = 1;
                tbAmount.Text = "";
                tbAmount.Enabled = false;
                cbMealType.SelectedIndex = 0;
                lvAssignMeals.BeginUpdate();
                lvAssignMeals.Items.Clear();
                lvAssignMeals.EndUpdate();
                lvAssignMeals.Invalidate();

                foreach (ListViewItem item in lvEmployees.Items)
                {
                    item.BackColor = Color.White;
                }

                validFrom = new DateTime(1900, 1, 1);
                validTo = new DateTime(2099, 1, 1);

                this.btnAssign.Enabled = true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssignedAdd.btnClear_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbWorkingUnit_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.updatelvEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssignedAdd.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void updatelvEmployees()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int wunits = -1;
                List<WorkingUnitTO> wuList = new List<WorkingUnitTO>();
                // Value -1 is assigned to "ALL" in cbParentUnitID
                if (cbWorkingUnit.SelectedIndex > 0 && Int32.Parse(cbWorkingUnit.SelectedValue.ToString().Trim()) != -1)
                {
                    wunits = (int)cbWorkingUnit.SelectedValue;
                }
                else
                    wunits = -1;

                if (this.chbHierarhicly.Checked && !this.cbWorkingUnit.Text.ToString().Equals("*"))
                {
                    WorkingUnit wu = new WorkingUnit();
                    if (wunits != -1)
                    {
                        wu.WUTO.WorkingUnitID = wunits;
                        wuList = wu.Search();
                    }
                    else
                    {
                        for (int i = wuList.Count - 1; i >= 0; i--)
                        {
                            if (wuList[i].WorkingUnitID == wuList[i].ParentWorkingUID)
                            {
                                wuList.RemoveAt(i);
                            }
                        }
                    }

                    wuList = wu.FindAllChildren(wuList);

                }
                else if (!this.chbHierarhicly.Checked && !this.cbWorkingUnit.Text.ToString().Equals("*"))
                {
                    WorkingUnit wu = new WorkingUnit();
                    wu.WUTO.ParentWorkingUID = wunits;
                    wuList = wu.Search();
                }
                else
                {
                    wuList = wUnits;
                }
                string wunitsString = "";
                foreach (WorkingUnitTO workingUnit in wuList)
                {
                    wunitsString += workingUnit.WorkingUnitID + ",";
                }

                if (wunitsString.Length > 0)
                {
                    wunitsString = wunitsString.Substring(0, wunitsString.Length - 1);
                }
                currentEmployeesList = currentEmployee.SearchByWU(wunitsString);

                lvEmployees.Items.Clear();
                populateEmployeesListView(currentEmployeesList);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " MealAssignedAdd.updatelvEmployees(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private string parse(string forParsing)
        {
            string parsedString = forParsing.Trim();
            if (parsedString.StartsWith("*"))
            {
                parsedString = parsedString.Substring(1);
                parsedString = "%" + parsedString;
            }

            if (parsedString.EndsWith("*"))
            {
                parsedString = parsedString.Substring(0, parsedString.Length - 1);
                parsedString = parsedString + "%";
            }

            return parsedString;
        }

        private void populateEmployeesListView(List<EmployeeTO> employeesList)
        {
            try
            {
                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();

                for (int i = 0; i < employeesList.Count; i++)
                {
                    EmployeeTO employee = employeesList[i];
                    ListViewItem item = new ListViewItem();

                    item.Text = employee.EmployeeID.ToString().Trim();
                    item.SubItems.Add(employee.LastName.Trim() + " " + employee.FirstName.Trim());
                    item.Tag = employee.EmployeeID;
                    lvEmployees.Items.Add(item);
                }

                lvEmployees.EndUpdate();
                lvEmployees.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PresenceGraphForDayControl.populateEmployeesListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void MealAssignedAdd_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.rbMeals.Checked = true;
                this.rbMoney_CheckedChanged(sender, e);

                this.Cursor = Cursors.WaitCursor;
                wUnits = new List<WorkingUnitTO>();

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
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

                currentEmployeesList = currentEmployee.SearchByWU(wuString);
                populateEmployeesListView(currentEmployeesList);

                MealType mealType = new MealType();
                ArrayList mealTypeList = new ArrayList();
                mealTypeList = mealType.Search(-1, "", "", new DateTime(), new DateTime());
                populateMealTypeCombo(mealTypeList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssignedAdd.MealAssignedAdd_Load((): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateMealTypeCombo(ArrayList array)
        {
            try
            {
                this.cbMealType.DataSource = array;
                this.cbMealType.DisplayMember = "Name";
                this.cbMealType.ValueMember = "MealTypeID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssignedAdd.populateMealTypeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                log.writeLog(DateTime.Now + " MealAssignedAdd.populateWorkigUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssignedAdd.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbUnlimited_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!chbUnlimited.Checked)
                {
                    this.dtpFrom.Enabled = true;
                    this.dtpTo.Enabled = true;
                }
                else
                {
                    this.dtpFrom.Enabled = false;
                    this.dtpTo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssignedAdd.chbUnlimited_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbUnlimitedMeals_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.chbUnlimitedDailyQTY.Checked = this.chbUnlimitedMeals.Checked;
                if (!chbUnlimitedMeals.Checked)
                {
                    this.nudQuantity.Enabled = true;
                   
                }
                else
                {
                    this.nudQuantity.Enabled = false;
                    
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssignedAdd.chbUnlimitedMeals_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbUnlimitedDailyQTY_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (!chbUnlimitedDailyQTY.Checked)
                {
                    this.nudQuantityDaily.Enabled = true;
                }
                else
                {
                    this.nudQuantityDaily.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssignedAdd.chbUnlimitedDailyQTY_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbUnlimitedMoney_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!chbUnlimitedMoney.Checked)
                {
                    this.tbAmount.Enabled = true;
                }
                else
                {
                    this.tbAmount.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssignedAdd.chbUnlimitedMoney_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbMeals_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (rbMeals.Checked)
                {
                    this.nudQuantity.Enabled = true;
                    this.nudQuantityDaily.Enabled = true;
                    this.chbUnlimitedMeals.Enabled = true;
                    this.chbUnlimitedDailyQTY.Enabled = true;
                    this.lblDailyLimit.Enabled = true;
                    this.lblNumber.Enabled = true;
                }
                else
                {
                    this.nudQuantity.Enabled = false;
                    this.nudQuantityDaily.Enabled = false;
                    this.chbUnlimitedMeals.Enabled = false;
                    this.chbUnlimitedDailyQTY.Enabled = false;
                    this.lblDailyLimit.Enabled = false;
                    this.lblNumber.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssignedAdd.rbMeals_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbMoney_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (rbMoney.Checked)
                {
                    this.tbAmount.Enabled = true;
                    this.chbUnlimitedMoney.Enabled = true;
                    this.lblAmount.Enabled = true;
                }
                else
                {
                    this.tbAmount.Enabled = false;
                    this.chbUnlimitedMoney.Enabled = false;
                    this.lblAmount.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssignedAdd.rbMoney_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbHierarhicly_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.updatelvEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssignedAdd.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvEmployees.SelectedItems.Count == 0)
                {
                    MessageBox.Show(rm.GetString("selEmpl", culture));
                    return;
                }
                if (rbMoney.Checked)
                {
                    try
                    {
                        if (!tbAmount.Text.Trim().Equals(""))
                        {
                            Int32.Parse(tbAmount.Text.Trim());
                        }
                    }
                    catch
                    {
                        MessageBox.Show(rm.GetString("messageAmountMustBeNum", culture));
                        return;
                    }
                }
                MealAssigned mealAssigned = new MealAssigned();
                selMealType = (MealType)cbMealType.SelectedItem;
                ArrayList mealAssignList = new ArrayList();
                string selEmpl = "";
                foreach (ListViewItem item in lvEmployees.SelectedItems)
                {
                    selEmpl += item.Tag.ToString() + ", ";
                }
                if (selEmpl.Length > 0)
                {
                    selEmpl = selEmpl.Substring(0, selEmpl.Length - 2);
                }
                int num = 0;
                if (!chbUnlimited.Checked)
                {
                    num = mealAssigned.SearchCount(selEmpl, selMealType.MealTypeID.ToString(), dtpFrom.Value.Date, dtpTo.Value.Date);
                    if (num > 0)
                    {
                        mealAssignList = mealAssigned.Search(selEmpl, selMealType.MealTypeID.ToString(), dtpFrom.Value.Date, dtpTo.Value.Date);
                    }
                }

                lvAssignMeals.BeginUpdate();
                lvAssignMeals.Items.Clear();

                foreach (ListViewItem item in lvEmployees.SelectedItems)
                {
                    foreach (MealAssigned ma in mealAssignList)
                    {
                        if (ma.EmployeeID == int.Parse(item.Tag.ToString()))
                        {
                            item.BackColor = Color.Red;
                        }
                    }
                    if (item.BackColor.Equals(Color.Red))
                    {
                        continue;
                    }

                    ListViewItem lastItem = null;
                    lastItem = (ListViewItem)item.Clone();
                    if (!chbUnlimited.Checked)
                    {
                        lastItem.SubItems.Add(dtpFrom.Value.Date.ToString("dd.MM.yyyy"));
                        lastItem.SubItems.Add(dtpTo.Value.Date.ToString("dd.MM.yyyy"));
                        validFrom = dtpFrom.Value.Date;
                        validTo = dtpTo.Value.Date;
                    }
                    else
                    {
                        lastItem.SubItems.Add("N/A");
                        lastItem.SubItems.Add("N/A");
                    }
                    lastItem.SubItems.Add(selMealType.Name.ToString());
                    if (this.rbMeals.Checked)
                    {
                        lastItem.SubItems.Add(rm.GetString("rbMeals", culture));
                        if (!chbUnlimitedMeals.Checked)
                        {
                            lastItem.SubItems.Add(nudQuantity.Value.ToString());
                        }
                        else
                        {
                            lastItem.SubItems.Add("N/A");
                        }
                        if (!chbUnlimitedDailyQTY.Checked)
                        {
                            lastItem.SubItems.Add(nudQuantityDaily.Value.ToString());
                        }
                        else
                        {
                            lastItem.SubItems.Add("N/A");
                        }
                    }
                    else// if(this.rbMoney.Checked)
                    {
                        lastItem.SubItems.Add(rm.GetString("rbMoney", culture));
                        if (!chbUnlimitedMoney.Checked)
                        {
                            lastItem.SubItems.Add(tbAmount.Text);
                        }
                        else
                        {
                            lastItem.SubItems.Add("N/A");
                        }
                        lastItem.SubItems.Add("N/A");
                    }

                    if (lastItem != null)
                    {
                        lvAssignMeals.Items.Add(lastItem);
                    }
                }

                lvAssignMeals.EndUpdate();
                lvAssignMeals.Invalidate();
                if (num > 0)
                {
                    MessageBox.Show(rm.GetString("someMealsAlreadyDefined", culture));
                }
                this.btnAssign.Enabled = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssignedAdd.btnAssign_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                MealAssigned mealAssigned = new MealAssigned();
                ArrayList mealAssignList = new ArrayList();
                string selEmpl = "";
                bool saved = true;
                foreach (ListViewItem item in lvEmployees.SelectedItems)
                {
                    selEmpl += item.Tag.ToString() + ", ";
                }
                if (selEmpl.Length > 0)
                {
                    selEmpl = selEmpl.Substring(0, selEmpl.Length - 2);
                }
                int num = -1;
                if (!chbUnlimited.Checked)
                {
                    num = mealAssigned.SearchCount(selEmpl, selMealType.MealTypeID.ToString(), dtpFrom.Value.Date, dtpTo.Value.Date);
                    if (num > 0)
                    {
                        mealAssignList = mealAssigned.Search(selEmpl, selMealType.MealTypeID.ToString(), dtpFrom.Value.Date, dtpTo.Value.Date);

                    }
                }

                int qty;
                int qtyDaily;
                int moneyAmount;

                if (rbMeals.Checked)
                {
                    // unlimited
                    qty = qtyDaily = Constants.unlimited;
                    // not defined
                    moneyAmount = Constants.undefined;
                }
                else
                {
                    // not defined
                    qty = qtyDaily = Constants.undefined;
                    // unlimited
                    moneyAmount = Constants.unlimited;
                }

                ListViewItem lvItem = lvAssignMeals.Items[0];

                if (lvItem.SubItems[5].Text.Equals(rm.GetString("rbMeals", culture)))
                {
                    if (!lvItem.SubItems[6].Text.Equals(@"N/A"))
                    {
                        qty = int.Parse(lvItem.SubItems[6].Text.ToString());
                    }
                }
                else
                {
                    if (!lvItem.SubItems[6].Text.Equals(@"N/A"))
                    {
                        moneyAmount = int.Parse(lvItem.SubItems[6].Text.ToString());
                    }
                }
                if (!lvItem.SubItems[7].Text.Equals(@"N/A"))
                {
                    qtyDaily = int.Parse(lvItem.SubItems[7].Text.ToString());
                }


                foreach (ListViewItem item in lvAssignMeals.Items)
                {
                    foreach (MealAssigned ma in mealAssignList)
                    {
                        if (ma.EmployeeID == int.Parse(item.Tag.ToString()))
                        {
                            item.BackColor = Color.Red;
                        }
                    }
                    if (item.BackColor.Equals(Color.Red))
                    {
                        continue;
                    }
                    bool itemSaved = mealAssigned.Save(int.Parse(item.Tag.ToString()), selMealType.MealTypeID, validFrom, validTo, qty, qtyDaily, moneyAmount);
                    saved = saved && itemSaved;
                }
                if (saved)
                {
                    MessageBox.Show(rm.GetString("mealsAssigned", culture));
                }
                else
                {
                    MessageBox.Show(rm.GetString("someMealsAlreadyDefined", culture));
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssignedAdd.btnAssign_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void nudQuantity_ValueChanged(object sender, EventArgs e)
        {
            if (nudQuantity.Value < nudQuantityDaily.Value)
            {
                throw new Exception(rm.GetString("qtyValueChanged", culture));
            }
        }

        private void nudQuantityDaily_ValueChanged(object sender, EventArgs e)
        {
            if (nudQuantity.Value < nudQuantityDaily.Value)
            {
                throw new Exception(rm.GetString("qtyValueChanged", culture));
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
                log.writeLog(DateTime.Now + " MealAssignedAdd.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void MealAssignedAdd_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealAssignedAdd.MealAssignedAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}