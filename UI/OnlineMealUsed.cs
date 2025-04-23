using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;


using TransferObjects;
using Common;
using Util;
using System.Globalization;
using System.Resources;
using System.Collections;
using System.IO;

namespace UI
{
    public partial class OnlineMealUsed : UserControl
    {
        private OnlineMealsUsedTO currentMeal = null;
        const int colNum = 25;
        // List View indexes
        const int EmployeeIndex = 0;
        const int Stringone = 1;
        const int EmployeeName = 2;
        const int EmployeeLast = 3;
        const int CostCenter = 4;
        const int CostCenterDesc = 5;
        const int Branch = 6;
        const int Week = 7;
        const int Year = 8;
        const int Month = 9;
        const int Day = 10;
        const int Time = 11;
        const int MealType = 12;
        const int Qty = 13;
        const int Restaurant = 14;
        const int Line = 15;

        const int Insert = 16;
        const int ValidationReader = 17;
        const int ReasonReader = 18;
        const int ValidationAuto = 19;
        const int AutoReason = 20;
        const int Approve = 21;
        const int ApprovedBy = 22;
        const int ApprovedTime = 23;

        private ListViewItemComparer _comp;
        private List<int> wuList = new List<int>();
        private List<int> ouList = new List<int>();

        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private Dictionary<int, OrganizationalUnitTO> oUnits = new Dictionary<int, OrganizationalUnitTO>();
        private string wuString = "";
        private string ouString = "";

        List<OnlineMealsUsedTO> listMealsUsed = new List<OnlineMealsUsedTO>();

        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
        Dictionary<int, EmployeeAsco4TO> ascoDict = new Dictionary<int, EmployeeAsco4TO>();
        Dictionary<int, WorkingUnitTO> wuDict = new Dictionary<int, WorkingUnitTO>();
        Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rules = new Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>>();

        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;

        string menuItemID;
        string menuItemUsedID;

        bool addUsedPermission = false;
        bool updateUsedPermission = false;
        bool deleteUsedPermission = false;

        private int startIndex;

        bool emplDateChanged = true;

        public OnlineMealUsed()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(MealsUsed).Assembly);

                setLanguage();

                currentMeal = new OnlineMealsUsedTO();
                logInUser = NotificationController.GetLogInUser();

                DateTime now = DateTime.Now;

                dtFrom.Value = now.Date.AddDays(-now.Day + 1);
                dtTo.Value = now.AddSeconds(-now.Second);

                btnPrev.Visible = false;
                btnNext.Visible = false;
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
                    case OnlineMealUsed.Qty:
                    case OnlineMealUsed.EmployeeIndex:
                    case OnlineMealUsed.Week:
                    case OnlineMealUsed.Month:
                    case OnlineMealUsed.Day:
                    case OnlineMealUsed.Year:
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
                    case OnlineMealUsed.EmployeeName:
                    case OnlineMealUsed.EmployeeLast:
                    case OnlineMealUsed.Stringone:
                    case OnlineMealUsed.CostCenter:
                    case OnlineMealUsed.CostCenterDesc:
                    case OnlineMealUsed.MealType:
                    case OnlineMealUsed.Restaurant:
                    case OnlineMealUsed.Line:
                    case OnlineMealUsed.Insert:
                    case OnlineMealUsed.ValidationReader:
                    case OnlineMealUsed.ReasonReader:
                    case OnlineMealUsed.ValidationAuto:
                    case OnlineMealUsed.AutoReason:
                    case OnlineMealUsed.Approve:
                    case OnlineMealUsed.ApprovedBy:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    case OnlineMealUsed.ApprovedTime:
                        {
                            if (!item1.SubItems[OnlineMealUsed.ApprovedTime].Text.Trim().Equals("") && !item2.SubItems[OnlineMealUsed.ApprovedTime].Text.Trim().Equals(""))
                            {
                                DateTime date1 = DateTime.Parse(item1.SubItems[OnlineMealUsed.ApprovedTime].Text.Trim(), culture);
                                DateTime date2 = DateTime.Parse(item2.SubItems[OnlineMealUsed.ApprovedTime].Text.Trim(), culture);
                                return CaseInsensitiveComparer.Default.Compare(date1, date2);
                            }
                            else
                                return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    case OnlineMealUsed.Time:
                        {
                            DateTime time1 = new DateTime();
                            DateTime time2 = new DateTime();

                            string t1 = sub1.Text;
                            string t2 = sub2.Text;

                            time1 = new DateTime(1, 1, 1, int.Parse(t1.Substring(0, 2)), int.Parse(t1.Substring(3, 2)), int.Parse(t1.Substring(6, 2)));
                            time2 = new DateTime(1, 1, 1, int.Parse(t2.Substring(0, 2)), int.Parse(t2.Substring(3, 2)), int.Parse(t2.Substring(6, 2)));
                            return CaseInsensitiveComparer.Default.Compare(time1, time2);


                        }
                    default:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                }
            }
        }

        #endregion


        private void setLanguage()
        {
            try
            {
                //group box text
                //gbFilter.Text = rm.GetString("gbFilter", culture);
                gbEmployee.Text = rm.GetString("gbEmployees", culture);
                gbValidation.Text = rm.GetString("gbValidationReader", culture);
                gbValidationAuto.Text = rm.GetString("gbValidationAuto", culture);
                gbApproval.Text = rm.GetString("gbApproval", culture);
                gbEnterWay.Text = rm.GetString("gbInput", culture);
                gbExport.Text = rm.GetString("", culture);
                gbApprove.Text = rm.GetString("gbApproveStatus", culture);
                gbFilter.Text = rm.GetString("gbUnitFilter", culture);
                gbAutoCheck.Text = rm.GetString("gbAutoCheck", culture);
                gbMassiveInput.Text = rm.GetString("gbMassiveInput", culture);

                //lblText
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblLine.Text = rm.GetString("lblLine", culture);
                lblMealType.Text = rm.GetString("lblTypeMeal", culture);
                lblOperater.Text = rm.GetString("lblOperator", culture);
                lblQty.Text = rm.GetString("lblQty", culture);
                lblReason.Text = rm.GetString("lblReason", culture);
                lblRestaurant.Text = rm.GetString("lblRestaurant", culture);
                lblStatus.Text = rm.GetString("lblStatus", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblToApproval.Text = rm.GetString("lblTo", culture);
                lblReasonAuto.Text = rm.GetString("lblReason", culture);
                lblFromApproval.Text = rm.GetString("lblFrom", culture);
                lblTotal.Text = rm.GetString("lblTotalMeals", culture) + " 0";
                lblFromCheck.Text = rm.GetString("lblFrom", culture);
                lblToCheck.Text = rm.GetString("lblTo", culture);
                lblCompany.Text = rm.GetString("lblCompany", culture);
                lblPath.Text = rm.GetString("lblFilePath", culture);

                //cb text
                cbRed.Text = rm.GetString("chbRed", culture);
                cbGreen.Text = rm.GetString("chbGreen", culture);
                cbManual.Text = rm.GetString("lblManual", culture);
                cbNOK.Text = rm.GetString("chbNOK", culture);
                cbOK.Text = rm.GetString("chbOK", culture);
                cbReader.Text = rm.GetString("chbCardReader", culture);
                cbSelectAllEmpl.Text = rm.GetString("chbSelectAll", culture);
                chbApproval.Text = rm.GetString("", culture);

                //RB text
                rbOU.Text = rm.GetString("gbOrganizationalUnits", culture);
                rbWU.Text = rm.GetString("WUForm", culture);
                rbSummary.Text = rm.GetString("rbSummary", culture);
                rbAnalitical.Text = rm.GetString("rbAnalitic", culture);

                //button text
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnApprove.Text = rm.GetString("btnApprove", culture);
                btnNotApprove.Text = rm.GetString("btnNotApprove", culture);
                btnDelete.Text = rm.GetString("btnDeleteStatus", culture);
                btnExport.Text = rm.GetString("btnExport", culture);
                btnHistory.Text = rm.GetString("btnHistoryOfChange", culture);
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnValidate.Text = rm.GetString("btnValidate", culture);
                btnBusinessTrip.Text = rm.GetString("btnBusinessTrip", culture);
                btnBrowse.Text = rm.GetString("btnBrowse", culture);

                lvRestaurant.BeginUpdate();
                lvRestaurant.Columns.Add("", lvRestaurant.Width - 21, HorizontalAlignment.Left);
                lvRestaurant.EndUpdate();

                lvMealType.BeginUpdate();
                lvMealType.Columns.Add("", lvMealType.Width - 21, HorizontalAlignment.Left);
                lvMealType.EndUpdate();

                lvLine.BeginUpdate();
                lvLine.Columns.Add("", lvLine.Width - 21, HorizontalAlignment.Left);
                lvLine.EndUpdate();

                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrFirstName", culture), (lvEmployees.Width - 7) / 2 - 15, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrLastName", culture), (lvEmployees.Width - 7) / 2 - 6, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();

                lvReason.BeginUpdate();
                lvReason.Columns.Add("", lvReason.Width - 21, HorizontalAlignment.Left);
                lvReason.EndUpdate();

                lvStatus.BeginUpdate();
                lvStatus.Columns.Add("", lvStatus.Width - 21, HorizontalAlignment.Left);
                lvStatus.EndUpdate();

                lvOperater.BeginUpdate();
                lvOperater.Columns.Add("", lvOperater.Width - 21, HorizontalAlignment.Left);
                lvOperater.EndUpdate();

                lvValidationReasonAuto.BeginUpdate();
                lvValidationReasonAuto.Columns.Add("", lvValidationReasonAuto.Width - 21, HorizontalAlignment.Left);
                lvValidationReasonAuto.EndUpdate();

                lvMealsUsed.BeginUpdate();
                lvMealsUsed.Columns.Add(rm.GetString("hdrEmployeeID", culture), 70, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrStringone", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrFirstName", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrLastName", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrCostCenter", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrCostCenterDesc", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrBranch", culture), 50, HorizontalAlignment.Right);
                lvMealsUsed.Columns.Add(rm.GetString("hdrWeek", culture), 50, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrYear", culture), 50, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrMonth", culture), 50, HorizontalAlignment.Right);
                lvMealsUsed.Columns.Add(rm.GetString("hdrDay", culture), 50, HorizontalAlignment.Right);
                lvMealsUsed.Columns.Add(rm.GetString("hdrTime", culture), 50, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrMealType", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrQty", culture), 50, HorizontalAlignment.Right);
                lvMealsUsed.Columns.Add(rm.GetString("hdrRestaurant", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrLine", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrManual", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrValidationReader", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrReason", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrValidationAuto", culture), 50, HorizontalAlignment.Right);
                lvMealsUsed.Columns.Add(rm.GetString("hdrReason", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrApproved", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrApprovedBy", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrTime", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateWorkingUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

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
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateWorkingUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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
                    this.cbWU.SelectedIndex = cbWU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.btnWUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnOUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsOrgUnits = new OrganizationalUnit().getOrganizationUnits(ouString);
                WorkingUnitsTreeView orgUnitsTreeView = new WorkingUnitsTreeView(dsOrgUnits);
                orgUnitsTreeView.ShowDialog();
                if (!orgUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbOU.SelectedIndex = cbOU.FindStringExact(orgUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.btnOUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbWU_CheckedChanged(object sender, EventArgs e)
        {
            int wuID = -1;

            if (rbWU.Checked)
            {
                cbWU.Enabled = btnWUTree.Enabled = true;
                cbOU.Enabled = btnOUTree.Enabled = false;

                if (cbWU.SelectedIndex > 0)
                    wuID = (int)cbWU.SelectedValue;
            }
            else
            {
                cbOU.Enabled = btnOUTree.Enabled = true;
                cbWU.Enabled = btnWUTree.Enabled = false;

                if (cbOU.SelectedIndex > 0)
                    wuID = (int)cbOU.SelectedValue;
            }

            populateEmployees();
        }

        private void rbOU_CheckedChanged(object sender, EventArgs e)
        {
            int wuID = -1;

            if (!rbOU.Checked)
            {
                cbWU.Enabled = btnWUTree.Enabled = true;
                cbOU.Enabled = btnOUTree.Enabled = false;

                if (cbWU.SelectedIndex > 0)
                    wuID = (int)cbWU.SelectedValue;
            }
            else
            {
                cbOU.Enabled = btnOUTree.Enabled = true;
                cbWU.Enabled = btnWUTree.Enabled = false;

                if (cbOU.SelectedIndex > 0)
                    wuID = (int)cbOU.SelectedValue;
            }

            populateEmployees();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                OnlineMealsUsedAdd onlineMealsUsedAdd = new OnlineMealsUsedAdd();
                onlineMealsUsedAdd.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealUsed.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public void populateListViewRestaurants(List<OnlineMealsRestaurantTO> mealTypesList)
        {
            try
            {
                lvRestaurant.BeginUpdate();
                lvRestaurant.Items.Clear();

                if (mealTypesList.Count > 0)
                {
                    for (int i = 0; i < mealTypesList.Count; i++)
                    {
                        OnlineMealsRestaurantTO mealType = (OnlineMealsRestaurantTO)mealTypesList[i];
                        ListViewItem item = new ListViewItem();

                        item.Text = mealType.Name.ToString().Trim();
                        item.ToolTipText = mealType.RestaurantID.ToString();
                        item.Tag = mealType;
                        lvRestaurant.Items.Add(item);
                    }
                }

                lvRestaurant.EndUpdate();
                lvRestaurant.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateListViewRestaurants(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateListViewReasons()
        {
            try
            {
                lvReason.BeginUpdate();
                lvReason.Items.Clear();
                List<string> reasons = new List<string>();
                reasons.Add(Constants.RestaurantCompany);
                reasons.Add(Constants.RestaurantLocation);
                reasons.Add(Constants.RestaurantTagNotActive);
                reasons.Add(Constants.RestaurantUsed);
                reasons.Add(Constants.RestaurantEmployeeNotActive);

                for (int i = 0; i < reasons.Count; i++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = reasons[i].Trim();
                    lvReason.Items.Add(item);
                }

                lvReason.EndUpdate();
                lvReason.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateListViewRestaurants(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateListViewAutoCheckReasons()
        {
            try
            {
                lvValidationReasonAuto.BeginUpdate();
                lvValidationReasonAuto.Items.Clear();
                List<string> reasons = new List<string>();
                reasons.Add(Constants.AutoCheckFailureReason.DOUBLE_REGISTRATION.ToString().Trim());
                reasons.Add(Constants.AutoCheckFailureReason.NO_EFFECTIVE_WORK.ToString().Trim());
                reasons.Add(Constants.AutoCheckFailureReason.NO_MINIMAL_PRESENCE.ToString().Trim());
                reasons.Add(Constants.AutoCheckFailureReason.NOT_WORKING_DAY.ToString().Trim());
                reasons.Add(Constants.AutoCheckFailureReason.UNJUSTIFED_DAY.ToString().Trim());
                reasons.Add(Constants.AutoCheckFailureReason.WHOLE_DAY_ABSENCE.ToString().Trim());

                for (int i = 0; i < reasons.Count; i++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = reasons[i].Trim();
                    lvValidationReasonAuto.Items.Add(item);
                }

                lvValidationReasonAuto.EndUpdate();
                lvValidationReasonAuto.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateListViewRestaurants(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateListViewStatus()
        {
            try
            {
                lvStatus.BeginUpdate();
                lvStatus.Items.Clear();
                List<string> reasons = new List<string>();
                reasons.Add(Constants.MealApproved);
                reasons.Add(Constants.MealNotApproved);
                reasons.Add(Constants.MealDeleted);
                reasons.Add(Constants.MealBusinessTrip);

                for (int i = 0; i < reasons.Count; i++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = reasons[i].Trim();
                    lvStatus.Items.Add(item);
                }

                lvStatus.EndUpdate();
                lvStatus.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateListViewRestaurants(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateListViewTypesMealType(List<OnlineMealsTypesTO> mealTypesList)
        {
            try
            {
                lvMealType.BeginUpdate();
                lvMealType.Items.Clear();

                if (mealTypesList.Count > 0)
                {
                    for (int i = 0; i < mealTypesList.Count; i++)
                    {
                        OnlineMealsTypesTO mealType = (OnlineMealsTypesTO)mealTypesList[i];
                        ListViewItem item = new ListViewItem();

                        item.Text = mealType.Name.ToString().Trim();
                        item.ToolTipText = mealType.MealTypeID.ToString();
                        item.Tag = mealType;
                        lvMealType.Items.Add(item);
                    }
                }

                lvMealType.EndUpdate();
                lvMealType.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateListViewTypesMealType(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateListViewUsers(List<ApplUserTO> usersList)
        {
            try
            {
                lvOperater.BeginUpdate();
                lvOperater.Items.Clear();

                if (usersList.Count > 0)
                {
                    for (int i = 0; i < usersList.Count; i++)
                    {
                        ApplUserTO user = (ApplUserTO)usersList[i];
                        ListViewItem item = new ListViewItem();

                        item.Text = user.Name.ToString().Trim();

                        item.Tag = user;
                        lvOperater.Items.Add(item);
                    }
                }

                lvOperater.EndUpdate();
                lvOperater.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateListViewTypesMealType(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateListViewTypesLines(List<OnlineMealsPointsTO> mealTypesList)
        {
            try
            {
                lvLine.BeginUpdate();
                lvLine.Items.Clear();

                if (mealTypesList.Count > 0)
                {
                    for (int i = 0; i < mealTypesList.Count; i++)
                    {
                        OnlineMealsPointsTO mealType = (OnlineMealsPointsTO)mealTypesList[i];
                        ListViewItem item = new ListViewItem();

                        item.Text = mealType.Name.ToString().Trim();
                        item.ToolTipText = mealType.PointID.ToString();
                        item.Tag = mealType;
                        lvLine.Items.Add(item);
                    }
                }

                lvLine.EndUpdate();
                lvLine.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateListViewTypesLines(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateListViewEmployees1(int wuID)
        {
            try
            {
                List<EmployeeTO> emplArray = new List<EmployeeTO>();
                if (rbWU.Checked)
                {
                    string workUnitID = wuID.ToString();
                    if (wuID == -1)
                    {
                        //emplArray = new Employee().SearchByWU(wuString);
                    }
                    else
                    {
                        List<WorkingUnitTO> wList = new List<WorkingUnitTO>();
                        WorkingUnit workUnit = new WorkingUnit();
                        foreach (WorkingUnitTO workingUnit in wUnits)
                        {
                            if (workingUnit.WorkingUnitID == (int)this.cbWU.SelectedValue)
                            {
                                wList.Add(workingUnit);
                                workUnit.WUTO = workingUnit;
                            }
                        }
                        if (workUnit.WUTO.ChildWUNumber > 0)
                            wList = workUnit.FindAllChildren(wList);
                        workUnitID = "";
                        foreach (WorkingUnitTO wunit in wList)
                        {
                            if (wuList.Contains(wunit.WorkingUnitID))
                                workUnitID += wunit.WorkingUnitID.ToString().Trim() + ",";
                        }

                        if (workUnitID.Length > 0)
                        {
                            workUnitID = workUnitID.Substring(0, workUnitID.Length - 1);
                        }

                        emplArray = new Employee().SearchByWU(workUnitID);
                    }

                }
                else
                {

                    string orgUnitID = wuID.ToString();
                    if (wuID == -1)
                    {
                        //emplArray = new Employee().SearchByOU(ouString, -1, -1, -1, dtpFrom.Value, dtpTo.Value);
                    }
                    else
                    {
                        List<OrganizationalUnitTO> oList = new List<OrganizationalUnitTO>();
                        OrganizationalUnit orgUnit = new OrganizationalUnit();
                        foreach (KeyValuePair<int, OrganizationalUnitTO> organizationalUnitPair in oUnits)
                        {
                            OrganizationalUnitTO organizationalUnit = organizationalUnitPair.Value;
                            if (organizationalUnit.OrgUnitID == (int)this.cbOU.SelectedValue)
                            {
                                oList.Add(organizationalUnit);
                                orgUnit.OrgUnitTO = organizationalUnit;
                            }
                        }

                        oList = orgUnit.FindAllChildren(oList);
                        orgUnitID = "";
                        foreach (OrganizationalUnitTO ounit in oList)
                        {
                            if (ouList.Contains(ounit.OrgUnitID))
                                orgUnitID += ounit.OrgUnitID.ToString().Trim() + ",";
                        }

                        if (orgUnitID.Length > 0)
                        {
                            orgUnitID = orgUnitID.Substring(0, orgUnitID.Length - 1);
                        }

                        emplArray = new Employee().SearchByOU(orgUnitID, -1, null, dtpFrom.Value, dtpTo.Value);
                    }
                }

                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();

                if (emplArray.Count > 0)
                {
                    for (int i = 0; i < emplArray.Count; i++)
                    {
                        EmployeeTO emplTO = (EmployeeTO)emplArray[i];
                        ListViewItem item = new ListViewItem();

                        item.Text = emplTO.FirstName.Trim();
                        item.ToolTipText = emplTO.EmployeeID.ToString();
                        item.SubItems.Add(emplTO.LastName.Trim());
                        item.Tag = emplTO;
                        lvEmployees.Items.Add(item);
                    }
                }

                lvEmployees.EndUpdate();
                lvEmployees.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateListViewEmployees(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateListViewEmpMealUsed(List<OnlineMealsUsedTO> listMealsUsed)
        {
            try
            {
                lvMealsUsed.BeginUpdate();
                lvMealsUsed.Items.Clear();

                int qty = 0;
                if (listMealsUsed.Count > 0)
                {
                    for (int i = 0; i < listMealsUsed.Count; i++)
                    {
                        OnlineMealsUsedTO mealUsedTO = (OnlineMealsUsedTO)listMealsUsed[i];
                        ListViewItem item = new ListViewItem();

                        EmployeeTO empl = new EmployeeTO();
                        if (emplDict.ContainsKey(mealUsedTO.EmployeeID))
                            empl = emplDict[mealUsedTO.EmployeeID];
                        string stringone = "";
                        string branch = "";

                        if (ascoDict.ContainsKey(mealUsedTO.EmployeeID))
                        {
                            stringone = ascoDict[mealUsedTO.EmployeeID].NVarcharValue2.Trim();
                            branch = ascoDict[mealUsedTO.EmployeeID].NVarcharValue6.Trim();
                        }

                        WorkingUnitTO costCentre = new WorkingUnitTO();
                        if (wuDict.ContainsKey(empl.WorkingUnitID)
                            && wuDict.ContainsKey(wuDict[empl.WorkingUnitID].ParentWorkingUID)
                            && wuDict.ContainsKey(wuDict[wuDict[empl.WorkingUnitID].ParentWorkingUID].ParentWorkingUID))
                            costCentre = wuDict[wuDict[wuDict[empl.WorkingUnitID].ParentWorkingUID].ParentWorkingUID];

                        item.Text = mealUsedTO.EmployeeID.ToString().Trim();
                        item.SubItems.Add(stringone);
                        item.SubItems.Add(empl.FirstName.Trim());
                        item.SubItems.Add(empl.LastName.Trim());
                        item.SubItems.Add(costCentre.Name);
                        item.SubItems.Add(costCentre.Description);
                        item.SubItems.Add(branch);
                        item.SubItems.Add(culture.Calendar.GetWeekOfYear(mealUsedTO.EventTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString());
                        item.SubItems.Add(mealUsedTO.EventTime.ToString("yyyy"));
                        item.SubItems.Add(mealUsedTO.EventTime.ToString("MM"));
                        item.SubItems.Add(mealUsedTO.EventTime.ToString("dd"));
                        item.SubItems.Add(mealUsedTO.EventTime.ToString("HH:mm:ss"));
                        item.SubItems.Add(mealUsedTO.MealType.Trim());
                        item.SubItems.Add(mealUsedTO.Qty.ToString().Trim());
                        item.SubItems.Add(mealUsedTO.RestaurantName.Trim());
                        item.SubItems.Add(mealUsedTO.PointName.Trim());
                        if (mealUsedTO.ManualCreated == 1)
                            item.SubItems.Add(rm.GetString("lblManual", culture));
                        else
                            item.SubItems.Add(rm.GetString("chbCardReader", culture));

                        if (mealUsedTO.OnlineValidation != -1)
                        {
                            if (mealUsedTO.OnlineValidation == 1)
                                item.SubItems.Add(rm.GetString("chbGreen", culture));
                            else
                                item.SubItems.Add(rm.GetString("chbRed", culture));
                        }
                        else
                            item.SubItems.Add("");
                        item.SubItems.Add(mealUsedTO.ReasonRefused);
                        if (mealUsedTO.AutoCheck != -1)
                        {
                            if (mealUsedTO.AutoCheck == 1)
                                item.SubItems.Add(rm.GetString("chbOK", culture));
                            else
                                item.SubItems.Add(rm.GetString("chbNOK", culture));
                        }
                        else
                            item.SubItems.Add("");
                        item.SubItems.Add(mealUsedTO.AutoCheckFailureReason);
                        item.SubItems.Add(mealUsedTO.Approved);
                        item.SubItems.Add(mealUsedTO.ApprovedBy);
                        if (mealUsedTO.ApprovedTime != new DateTime())
                            item.SubItems.Add(mealUsedTO.ApprovedTime.ToString("dd.MM.yyyy"));
                        else
                            item.SubItems.Add("");

                        item.Tag = mealUsedTO;
                        lvMealsUsed.Items.Add(item);

                        qty += mealUsedTO.Qty;
                    }
                }

                lvMealsUsed.EndUpdate();
                lvMealsUsed.Invalidate();

                lblTotal.Text = rm.GetString("lblTotalMeals", culture) + " " + qty.ToString().Trim();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateListViewEmployees(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int wuID = -1;
                if (cbWU.SelectedIndex > 0)
                    wuID = (int)cbWU.SelectedValue;

                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbOU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int ouID = -1;
                if (cbOU.SelectedIndex > 0)
                    ouID = (int)cbOU.SelectedValue;

                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvRestaurant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string restaurantID = "";
                foreach (int index in lvRestaurant.SelectedIndices)
                {
                    restaurantID += ((OnlineMealsRestaurantTO)lvRestaurant.Items[index].Tag).RestaurantID + ",";

                }
                if (restaurantID.Length > 0)
                    restaurantID = restaurantID.Remove(restaurantID.LastIndexOf(','));
                List<OnlineMealsPointsTO> currentLineList = new OnlineMealsPoints().SearchByRestaurant(restaurantID);
                populateListViewTypesLines(currentLineList);
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " OnlineMealsUsed.lvRestaurant_SelectedIndexChanged(): " + ex.Message + "\n");
            }
        }

        private void cbRed_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRed.Checked)
                lvReason.Enabled = true;
            else
                lvReason.Enabled = false;
        }

        private void cbNOK_CheckedChanged(object sender, EventArgs e)
        {
            if (cbNOK.Checked)
                lvValidationReasonAuto.Enabled = true;
            else
                lvValidationReasonAuto.Enabled = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvEmployees.Items.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noEmployeesSelected", culture));
                    return;
                }

                string restaurants = "";
                string points = "";
                string mealtypes = "";
                string emplIDs = "";
                string reasonsReader = "";
                string status = "";
                string reasonAutoCheck = "";
                string operater = "";

                int manualCreated = -1;
                int onlineValidation = -1;
                int autoCheck = -1;

                if ((numQtyFrom.Value != 0 || numQtyTo.Value != 0) && numQtyFrom.Value > numQtyTo.Value)
                {
                    MessageBox.Show(rm.GetString("QtyFromGreater", culture));
                    return;
                }

                int qtyFrom = (numQtyFrom.Value > 0 ? (int)numQtyFrom.Value : -1);
                int qtyTo = (numQtyTo.Value > 0 ? (int)numQtyTo.Value : -1);

                DateTime mealFrom = new DateTime();
                DateTime mealTO = new DateTime();

                DateTime mealFromApproved = new DateTime();
                DateTime mealTOApproved = new DateTime();

                //string hourFrom = "";
                //string hourTo = "";
                //string minFrom = "";
                //string minTo = "";

                //string timeFrom = tbTimeFrom.Text;
                //string timeTo = tbTimeTo.Text;

                //if (timeFrom.Trim() == "" || timeTo.Trim() == "" || !timeFrom.Contains(":") || !timeTo.Contains(":"))
                //{
                //    MessageBox.Show(rm.GetString("invalidTimeFormat", culture));
                //    return;
                //}

                //hourFrom = timeFrom.Remove(timeFrom.IndexOf(':'));
                //hourTo = timeTo.Remove(timeTo.IndexOf(':'));

                //minFrom = timeFrom.Substring(timeFrom.IndexOf(':') + 1);
                //minTo = timeTo.Substring(timeTo.IndexOf(':') + 1);

                //int hourF = -1;
                //if (!int.TryParse(hourFrom, out hourF))
                //    hourF = -1;

                //int hourT = -1;
                //if (!int.TryParse(hourTo, out hourT))
                //    hourT = -1;

                //int minT = -1;
                //if (!int.TryParse(minTo, out minT))
                //    minT = -1;

                //int minF = -1;
                //if (!int.TryParse(minFrom, out minF))
                //    minF = -1;

                //if (hourF == -1 || hourT == -1 || minF == -1 || minT == -1 || hourF < 0 || hourF > 23 || hourT < 0 || hourT > 23 || minF < 0 || minF > 59 || minT < 0 || minT > 59)
                //{
                //    MessageBox.Show(rm.GetString("invalidTimeFormat", culture));
                //    return;
                //}

                mealTO = dtpTo.Value;
                mealFrom = dtpFrom.Value;
                //mealFrom = new DateTime(mealFrom.Year, mealFrom.Month, mealFrom.Day, hourF, minF, 0);
                //mealTO = new DateTime(mealTO.Year, mealTO.Month, mealTO.Day, hourT, minT, 0);

                if (mealFrom > mealTO)
                {
                    MessageBox.Show(rm.GetString("dateFromBigerThanTO", culture));
                    return;
                }

                if (cbManual.Checked && !cbReader.Checked)
                    manualCreated = 1;
                else if (!cbManual.Checked && cbReader.Checked)
                    manualCreated = 0;
                else
                    manualCreated = -1;

                if (cbGreen.Checked && !cbRed.Checked)
                    onlineValidation = 1;
                else if (cbRed.Checked && !cbGreen.Checked)
                    onlineValidation = 0;
                else
                    onlineValidation = -1;

                if (cbOK.Checked && !cbNOK.Checked)
                    autoCheck = 1;
                else if (cbNOK.Checked && !cbOK.Checked)
                    autoCheck = 0;
                else
                    autoCheck = -1;

                foreach (int index in lvRestaurant.SelectedIndices)
                {
                    restaurants += ((OnlineMealsRestaurantTO)lvRestaurant.Items[index].Tag).RestaurantID + ",";
                }
                if (restaurants.Length > 0)
                {
                    restaurants = restaurants.Substring(0, restaurants.Length - 1);
                }

                foreach (int index in lvLine.SelectedIndices)
                {
                    points += ((OnlineMealsPointsTO)lvLine.Items[index].Tag).PointID + ",";
                }
                if (points.Length > 0)
                {
                    points = points.Substring(0, points.Length - 1);
                }

                foreach (int index in lvMealType.SelectedIndices)
                {
                    mealtypes += ((OnlineMealsTypesTO)lvMealType.Items[index].Tag).MealTypeID + ",";
                }
                if (mealtypes.Length > 0)
                {
                    mealtypes = mealtypes.Substring(0, mealtypes.Length - 1);
                }
                if (lvEmployees.SelectedItems.Count > 0)
                {
                    foreach (int index in lvEmployees.SelectedIndices)
                    {
                        emplIDs += ((EmployeeTO)lvEmployees.Items[index].Tag).EmployeeID + ",";

                    }
                    if (emplIDs.Length > 0)
                    {
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        emplIDs += ((EmployeeTO)item.Tag).EmployeeID + ",";

                    }
                    if (emplIDs.Length > 0)
                    {
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                    }
                }
                if (cbRed.Checked)
                {
                    foreach (int index in lvReason.SelectedIndices)
                    {
                        reasonsReader += lvReason.Items[index].Text + ",";

                    }
                    if (reasonsReader.Length > 0)
                    {
                        reasonsReader = reasonsReader.Substring(0, reasonsReader.Length - 1);
                    }
                }
                if (cbNOK.Checked)
                {

                    foreach (int index in lvValidationReasonAuto.SelectedIndices)
                    {
                        reasonAutoCheck += lvValidationReasonAuto.Items[index].Text + ",";

                    }
                    if (reasonAutoCheck.Length > 0)
                    {
                        reasonAutoCheck = reasonAutoCheck.Substring(0, reasonAutoCheck.Length - 1);
                    }
                }
                if (chbApproval.Checked)
                {
                    foreach (int index in lvStatus.SelectedIndices)
                    {
                        status += lvStatus.Items[index].Text + ",";

                    }
                    if (status.Length > 0)
                    {
                        status = status.Substring(0, status.Length - 1);
                    }

                    foreach (int index in lvOperater.SelectedIndices)
                    {
                        operater += ((ApplUserTO)lvOperater.Items[index].Tag).UserID + ",";

                    }
                    if (operater.Length > 0)
                    {
                        operater = operater.Substring(0, operater.Length - 1);
                    }
                    if (mealFromApproved.Date > mealTOApproved.Date)
                    {
                        MessageBox.Show(rm.GetString("dateFromBigerThanTO", culture));
                        return;
                    }

                    mealTOApproved = dtpToApproval.Value;
                    mealFromApproved = dtpFromApproval.Value;
                }

                listMealsUsed = new OnlineMealsUsed().SearchMealUsed(emplIDs, mealFrom, mealTO, restaurants, points, mealtypes, qtyFrom, qtyTo, manualCreated, onlineValidation, autoCheck, reasonsReader, reasonAutoCheck, status, operater, mealFromApproved, mealTOApproved);
                int qty = 0;
                foreach (OnlineMealsUsedTO meal in listMealsUsed)
                {
                    qty += meal.Qty;
                }
                lblTotal.Text = rm.GetString("lblTotalMeals", culture) + " " + qty.ToString().Trim();
                populateListView(listMealsUsed, startIndex);
                if (listMealsUsed.Count <= 0)
                {
                    //lvMealsUsed.BeginUpdate();
                    //lvMealsUsed.Items.Clear();
                    //lvMealsUsed.EndUpdate();
                    MessageBox.Show(rm.GetString("noMealsUsedFound", culture));
                }
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " OnlineMealsUsed.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void OnlineMealUsed_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                dtpFrom.Value = DateTime.Now.Date;
                dtpTo.Value = DateTime.Now.Date.AddDays(1).AddMinutes(-1);
                //tbTimeFrom.Text = "00:00";
                //tbTimeTo.Text = "23:59";
                startIndex = 0;
                _comp = new ListViewItemComparer(lvMealsUsed);
                lvMealsUsed.ListViewItemSorter = _comp;
                lvMealsUsed.Sorting = SortOrder.Ascending;

                lvStatus.Enabled = false;
                lvOperater.Enabled = false;
                dtpFromApproval.Enabled = false;
                dtpToApproval.Enabled = false;

                emplDict = new Employee().SearchDictionary();
                ascoDict = new EmployeeAsco4().SearchDictionary("");
                wuDict = new WorkingUnit().getWUDictionary();
                rules = new Common.Rule().SearchWUEmplTypeDictionary();

                List<ApplUserTO> listUsers = new ApplUser().SearchForCategory((int)Constants.Categories.HRSSC);
                populateListViewUsers(listUsers);
                List<OnlineMealsRestaurantTO> currentRestaurantList = new OnlineMealsRestaurant().Search();
                populateListViewRestaurants(currentRestaurantList);

                List<OnlineMealsTypesTO> currentMealTypeList = new OnlineMealsTypes().Search();
                populateListViewTypesMealType(currentMealTypeList);

                List<OnlineMealsPointsTO> currentLineList = new OnlineMealsPoints().SearchByRestaurant("");
                populateListViewTypesLines(currentLineList);

                if (logInUser != null)
                {
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                    oUnits = new ApplUserXOrgUnit().FindOUForUserDictionary(logInUser.UserID.Trim(), Constants.EmployeesPurpose);
                }

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";
                    wuList.Add(wUnit.WorkingUnitID);
                }

                if (wuString.Length > 0)
                    wuString = wuString.Substring(0, wuString.Length - 1);

                foreach (int id in oUnits.Keys)
                {
                    ouString += id.ToString().Trim() + ",";
                    ouList.Add(id);
                }

                if (ouString.Length > 0)
                    ouString = ouString.Substring(0, ouString.Length - 1);

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();

                menuItemID = NotificationController.GetCurrentMenuItemID();
                int index = menuItemID.LastIndexOf('_');

                menuItemUsedID = menuItemID + "_" + rm.GetString("tpMealsUsed", culture);
                setVisibility();

                populateWU();
                populateOU();
                populateListViewReasons();
                populateListViewStatus();
                populateListViewAutoCheckReasons();

                populateCompany();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.OnlineMealUsed_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void setVisibility()
        {
            try
            {
                int permissionUsed;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permissionUsed = (((int[])menuItemsPermissions[menuItemUsedID])[role.ApplRoleID]);

                    addUsedPermission = addUsedPermission || (((permissionUsed / 4) % 2) == 0 ? false : true);
                    updateUsedPermission = updateUsedPermission || (((permissionUsed / 2) % 2) == 0 ? false : true);
                    deleteUsedPermission = deleteUsedPermission || ((permissionUsed % 2) == 0 ? false : true);

                }

                btnAdd.Enabled = addUsedPermission;
                btnApprove.Enabled = updateUsedPermission;
                btnNotApprove.Enabled = updateUsedPermission;
                btnDelete.Enabled = deleteUsedPermission;
                btnBusinessTrip.Enabled = updateUsedPermission;
                dtFrom.Enabled = dtTo.Enabled = cbCompany.Enabled = btnValidate.Enabled = updateUsedPermission;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.setVisibility(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void populateWU()
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsed.populateWU(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateCompany()
        {
            try
            {
                List<WorkingUnitTO> companyAllList = new WorkingUnit().getRootWorkingUnitsList(wuString);
                List<WorkingUnitTO> companyList = new List<WorkingUnitTO>();

                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rules = new Common.Rule().SearchTypeAllRules(Constants.RuleRestaurant);

                foreach (WorkingUnitTO wu in companyAllList)
                {
                    if (!rules.ContainsKey(wu.WorkingUnitID))
                        continue;

                    bool useRestaurant = false;
                    foreach (int type in rules[wu.WorkingUnitID].Keys)
                    {
                        foreach (string ruleType in rules[wu.WorkingUnitID][type].Keys)
                        {
                            if (rules[wu.WorkingUnitID][type][ruleType].RuleValue == Constants.yesInt)
                            {
                                useRestaurant = true;
                                break;
                            }
                        }

                        if (useRestaurant)
                            break;
                    }

                    if (useRestaurant)
                        companyList.Add(wu);
                }

                if (companyList.Count > 0)
                {
                    cbCompany.DataSource = companyList;
                    cbCompany.DisplayMember = "Name";
                    cbCompany.ValueMember = "WorkingUnitID";

                    //btnValidate.Enabled = true;
                }
                else
                    btnValidate.Enabled = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsed.populateCompany(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateOU()
        {
            try
            {
                List<OrganizationalUnitTO> ouArray = new List<OrganizationalUnitTO>();

                foreach (int id in oUnits.Keys)
                {
                    ouArray.Add(oUnits[id]);
                }

                ouArray.Insert(0, new OrganizationalUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), ""));

                cbOU.DataSource = ouArray;
                cbOU.DisplayMember = "Name";
                cbOU.ValueMember = "OrgUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsed.populateOU(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void toHist(OnlineMealsUsedTO mealUsed, OnlineMealsUsedHistTO mealUsedHist)
        {
            try
            {

                mealUsedHist.Approved = mealUsed.Approved;
                mealUsedHist.ApprovedBy = mealUsed.ApprovedBy;
                mealUsedHist.ApprovedDesc = mealUsed.ApprovedDesc;
                mealUsedHist.ApprovedTime = mealUsed.ApprovedTime;
                mealUsedHist.AutoCheck = mealUsed.AutoCheck;
                mealUsedHist.AutoCheckFailureReason = mealUsed.AutoCheckFailureReason;
                mealUsedHist.AutoCheckTime = mealUsed.AutoCheckTime;
                mealUsedHist.CreatedBy = mealUsed.CreatedBy;
                mealUsedHist.CreatedTime = mealUsed.CreatedTime;
                mealUsedHist.EmployeeID = mealUsed.EmployeeID;
                mealUsedHist.EmployeeLocation = mealUsed.EmployeeLocation;
                mealUsedHist.EmployeeName = mealUsed.EmployeeName;
                mealUsedHist.EmployeeStringone = mealUsed.EmployeeStringone;
                mealUsedHist.EventTime = mealUsed.EventTime;
                mealUsedHist.ManualCreated = mealUsed.ManualCreated;
                mealUsedHist.MealType = mealUsed.MealType;
                mealUsedHist.MealTypeID = mealUsed.MealTypeID;
                mealUsedHist.ModifiedBy = mealUsed.ModifiedBy;
                mealUsedHist.ModifiedTime = mealUsed.ModifiedTime;
                mealUsedHist.OnlineValidation = mealUsed.OnlineValidation;
                mealUsedHist.PointID = mealUsed.PointID;
                mealUsedHist.PointName = mealUsed.PointName;
                mealUsedHist.Qty = mealUsed.Qty;
                mealUsedHist.ReasonRefused = mealUsed.ReasonRefused;
                mealUsedHist.RestaurantName = mealUsed.RestaurantName;
                mealUsedHist.TransactionID = mealUsed.TransactionID;


            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsed.toHist(): " + ex.Message + "\n");

                throw ex;
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                OnlineMealsUsed onlineMeal = new OnlineMealsUsed();
                OnlineMealsUsedHist onlineMealHist = new OnlineMealsUsedHist();

                string error = "";
                bool allApproved = true;
                foreach (ListViewItem item in lvMealsUsed.SelectedItems)
                {
                    OnlineMealsUsedTO mealUsed = (OnlineMealsUsedTO)item.Tag;

                    string validMealChange = validateAutoCheckDateInterval(mealUsed.EventTime).Trim();

                    if (!validMealChange.Trim().Equals(""))
                    {
                        error = validMealChange;
                        allApproved = false;
                        continue;
                    }

                    if (onlineMeal.BeginTransaction())
                    {
                        try
                        {
                            bool approved = true;

                            OnlineMealsUsedHistTO mealUsedHist = new OnlineMealsUsedHistTO();
                            toHist(mealUsed, mealUsedHist);

                            onlineMealHist.OnlineMealsUsedHistTO = mealUsedHist;
                            onlineMealHist.SetTransaction(onlineMeal.GetTransaction());

                            approved = approved && onlineMealHist.Save(false) > 0;

                            if (approved)
                            {
                                mealUsed.Approved = Constants.MealDeleted;
                                mealUsed.ApprovedDesc = Constants.MealDeleted;
                                mealUsed.ApprovedTime = DateTime.Now;
                                mealUsed.ApprovedBy = logInUser.UserID;

                                onlineMeal.OnlineMealsUsedTO = mealUsed;

                                approved = approved && onlineMeal.Update(false);
                            }

                            if (approved)
                                onlineMeal.CommitTransaction();
                            else
                            {
                                if (onlineMeal.GetTransaction() != null)
                                    onlineMeal.RollbackTransaction();

                                allApproved = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (onlineMeal.GetTransaction() != null)
                                onlineMeal.RollbackTransaction();

                            allApproved = false;

                            log.writeLog(DateTime.Now + " OnlineMealsUsed.btnDelete_Click(): " + ex.Message + "\n");
                        }
                    }
                    else
                        allApproved = false;
                }

                if (allApproved)
                {
                    MessageBox.Show(rm.GetString("onlineMealUsedUpdated", culture) + " " + error.Trim());
                }
                else
                {
                    MessageBox.Show(rm.GetString("onlineMealUsedNotUpdated", culture) + " " + error.Trim());
                }

                btnSearch.PerformClick();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(rm.GetString("onlineMealUsedNotUpdated", culture) + " " + ex.Message);
            }
            finally
            {

                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                OnlineMealsUsed onlineMeal = new OnlineMealsUsed();
                OnlineMealsUsedHist onlineMealHist = new OnlineMealsUsedHist();

                string error = "";
                bool allApproved = true;
                foreach (ListViewItem item in lvMealsUsed.SelectedItems)
                {
                    OnlineMealsUsedTO mealUsed = (OnlineMealsUsedTO)item.Tag;

                    string validMealChange = validateAutoCheckDateInterval(mealUsed.EventTime).Trim();

                    if (!validMealChange.Trim().Equals(""))
                    {
                        error = validMealChange;
                        allApproved = false;
                        continue;
                    }

                    if (onlineMeal.BeginTransaction())
                    {
                        try
                        {
                            bool approved = true;

                            OnlineMealsUsedHistTO mealUsedHist = new OnlineMealsUsedHistTO();
                            toHist(mealUsed, mealUsedHist);

                            onlineMealHist.OnlineMealsUsedHistTO = mealUsedHist;
                            onlineMealHist.SetTransaction(onlineMeal.GetTransaction());

                            approved = approved && onlineMealHist.Save(false) > 0;

                            if (approved)
                            {
                                mealUsed.Approved = Constants.MealApproved;
                                mealUsed.ApprovedDesc = Constants.MealApproved;
                                mealUsed.ApprovedTime = DateTime.Now;
                                mealUsed.ApprovedBy = logInUser.UserID;

                                onlineMeal.OnlineMealsUsedTO = mealUsed;

                                approved = approved && onlineMeal.Update(false);
                            }

                            if (approved)
                                onlineMeal.CommitTransaction();                                
                            else
                            {
                                if (onlineMeal.GetTransaction() != null)
                                    onlineMeal.RollbackTransaction();

                                allApproved = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (onlineMeal.GetTransaction() != null)
                                onlineMeal.RollbackTransaction();

                            allApproved = false;

                            log.writeLog(DateTime.Now + " OnlineMealsUsed.btnApprove_Click(): " + ex.Message + "\n");
                        }
                    }
                    else
                        allApproved = false;
                }

                if (allApproved)
                {
                    MessageBox.Show(rm.GetString("onlineMealUsedUpdated", culture) + " " + error.Trim());
                }
                else
                {
                    MessageBox.Show(rm.GetString("onlineMealUsedNotUpdated", culture) + " " + error.Trim());
                }

                btnSearch.PerformClick();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.btnApprove_Click(): " + ex.Message + "\n");
                MessageBox.Show(rm.GetString("onlineMealUsedNotUpdated", culture) + " " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnNotApprove_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                OnlineMealsUsed onlineMeal = new OnlineMealsUsed();
                OnlineMealsUsedHist onlineMealHist = new OnlineMealsUsedHist();

                string error = "";
                bool allApproved = true;
                foreach (ListViewItem item in lvMealsUsed.SelectedItems)
                {
                    OnlineMealsUsedTO mealUsed = (OnlineMealsUsedTO)item.Tag;

                    string validMealChange = validateAutoCheckDateInterval(mealUsed.EventTime).Trim();

                    if (!validMealChange.Trim().Equals(""))
                    {
                        error = validMealChange;
                        allApproved = false;
                        continue;
                    }

                    if (onlineMeal.BeginTransaction())
                    {
                        try
                        {
                            bool approved = true;

                            OnlineMealsUsedHistTO mealUsedHist = new OnlineMealsUsedHistTO();
                            toHist(mealUsed, mealUsedHist);

                            onlineMealHist.OnlineMealsUsedHistTO = mealUsedHist;
                            onlineMealHist.SetTransaction(onlineMeal.GetTransaction());

                            approved = approved && onlineMealHist.Save(false) > 0;

                            if (approved)
                            {
                                mealUsed.Approved = Constants.MealNotApproved;
                                mealUsed.ApprovedDesc = Constants.MealNotApproved;
                                mealUsed.ApprovedTime = DateTime.Now;
                                mealUsed.ApprovedBy = logInUser.UserID;

                                onlineMeal.OnlineMealsUsedTO = mealUsed;

                                approved = approved && onlineMeal.Update(false);
                            }

                            if (approved)
                                onlineMeal.CommitTransaction();
                            else
                            {
                                if (onlineMeal.GetTransaction() != null)
                                    onlineMeal.RollbackTransaction();

                                allApproved = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (onlineMeal.GetTransaction() != null)
                                onlineMeal.RollbackTransaction();

                            allApproved = false;

                            log.writeLog(DateTime.Now + " OnlineMealsUsed.btnNotApprove_Click(): " + ex.Message + "\n");
                        }
                    }
                    else
                        allApproved = false;
                }

                if (allApproved)
                {
                    MessageBox.Show(rm.GetString("onlineMealUsedUpdated", culture) + " " + error.Trim());
                }
                else
                {
                    MessageBox.Show(rm.GetString("onlineMealUsedNotUpdated", culture) + " " + error.Trim());
                }

                btnSearch.PerformClick();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.btnNotApprove_Click(): " + ex.Message + "\n");
                MessageBox.Show(rm.GetString("onlineMealUsedNotUpdated", culture) + " " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chbApproval_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chbApproval.Checked)
                {
                    lvStatus.Enabled = true;
                    lvOperater.Enabled = true;
                    dtpFromApproval.Enabled = true;
                    dtpToApproval.Enabled = true;
                }
                else
                {
                    lvStatus.Enabled = false;
                    lvOperater.Enabled = false;
                    dtpFromApproval.Enabled = false;
                    dtpToApproval.Enabled = false;
                }
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " OnlineMealsUsed.chbApproval_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbSelectAllEmpl_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbSelectAllEmpl.Checked)
                {

                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        item.Selected = true;
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        item.Selected = false;
                    }

                }
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " OnlineMealsUsed.cbSelectAllEmpl_CheckedChanged(): " + ex.Message + "\n");
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
                startIndex = 0;
                populateListView(listMealsUsed, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.lvMealsUsed_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        private void tbEmployee_TextChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string text = tbEmployee.Text.Trim();

                int id = -1;

                lvEmployees.SelectedItems.Clear();

                if (int.TryParse(text, out id))
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        if (((EmployeeTO)item.Tag).EmployeeID.ToString().Trim().StartsWith(id.ToString().Trim()))
                        {
                            item.Selected = true;
                            lvEmployees.Select();
                            lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));

                            break;
                        }
                    }
                }
                else
                {
                    foreach (ListViewItem item in lvEmployees.Items)
                    {
                        if (((EmployeeTO)item.Tag).FirstAndLastName.ToString().Trim().ToUpper().StartsWith(text.ToString().Trim().ToUpper()))
                        {
                            item.Selected = true;
                            lvEmployees.Select();
                            lvEmployees.EnsureVisible(lvEmployees.Items.IndexOf(lvEmployees.SelectedItems[0]));

                            break;
                        }
                    }
                }

                tbEmployee.Focus();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.tbEmployee_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void generateXLSReportAnalitical()
        {
            try
            {

                string reportName = "MealUsed" + DateTime.Now.Date.ToString("yyyyMMdd");

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = reportName;
                sfd.InitialDirectory = Constants.csvDocPath;
                sfd.Filter = "CSV (*.csv)|*.csv";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                string filePath = sfd.FileName;
                StreamWriter writer = new StreamWriter(filePath, true, System.Text.Encoding.Unicode);
               
                string txt = "";
                txt += rm.GetString("hdrMealID", culture) + "\t";
                txt += rm.GetString("hdrEmployeeID", culture) + "\t";
                txt += rm.GetString("hdrStringone", culture) + "\t";
                txt += rm.GetString("hdrFirstName", culture) + "\t";
                txt += rm.GetString("hdrLastName", culture) + "\t";
                txt += rm.GetString("hdrCostCenter", culture) + "\t";
                txt += rm.GetString("hdrCostCenterDesc", culture) + "\t";
                txt += rm.GetString("hdrBranch", culture) + "\t";
                txt += rm.GetString("hdrWeek", culture) + "\t";
                txt += rm.GetString("hdrYear", culture) + "\t";
                txt += rm.GetString("hdrMonth", culture) + "\t";
                txt += rm.GetString("hdrDay", culture) + "\t";
                txt += rm.GetString("hdrTime", culture) + "\t";
                txt += rm.GetString("hdrMealType", culture) + "\t";
                txt += rm.GetString("hdrQty", culture) + "\t";
                txt += rm.GetString("hdrRestaurant", culture) + "\t";
                txt += rm.GetString("hdrLine", culture) + "\t";
                txt += rm.GetString("hdrManual", culture) + "\t";
                txt += rm.GetString("hdrValidationReader", culture) + "\t";
                txt += rm.GetString("hdrReason", culture) + "\t";
                txt += rm.GetString("hdrValidationAuto", culture) + "\t";
                txt += rm.GetString("hdrReason", culture) + "\t";
                txt += rm.GetString("hdrApproved", culture) + "\t";
                txt += rm.GetString("hdrApprovedBy", culture) + "\t";
                txt += rm.GetString("hdrTime", culture) + "\t";
                writer.WriteLine(txt);

                for (int j = 0; j < listMealsUsed.Count; j++)
                {
                    txt = "";
                    OnlineMealsUsedTO mealUsedTO = listMealsUsed[j];

                    EmployeeTO empl = new EmployeeTO();
                    if (emplDict.ContainsKey(mealUsedTO.EmployeeID))
                        empl = emplDict[mealUsedTO.EmployeeID];
                    string stringone = "";
                    string branch = "";

                    if (ascoDict.ContainsKey(mealUsedTO.EmployeeID))
                    {
                        stringone = ascoDict[mealUsedTO.EmployeeID].NVarcharValue2.Trim();
                        branch = ascoDict[mealUsedTO.EmployeeID].NVarcharValue6.Trim();
                    }

                    WorkingUnitTO costCentre = new WorkingUnitTO();
                    if (wuDict.ContainsKey(empl.WorkingUnitID)
                        && wuDict.ContainsKey(wuDict[empl.WorkingUnitID].ParentWorkingUID)
                        && wuDict.ContainsKey(wuDict[wuDict[empl.WorkingUnitID].ParentWorkingUID].ParentWorkingUID))
                        costCentre = wuDict[wuDict[wuDict[empl.WorkingUnitID].ParentWorkingUID].ParentWorkingUID];

                    txt += mealUsedTO.TransactionID.ToString().Trim() + "\t";
                    txt += mealUsedTO.EmployeeID.ToString().Trim() + "\t";
                    txt += stringone + "\t";
                    txt += empl.FirstName.Trim() + "\t";
                    txt += empl.LastName.Trim() + "\t";
                    txt += costCentre.Name + "\t";
                    txt += costCentre.Description + "\t";
                    txt += branch + "\t";
                    txt += culture.Calendar.GetWeekOfYear(mealUsedTO.EventTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString() + "\t";
                    txt += mealUsedTO.EventTime.ToString("yyyy") + "\t";
                    txt += mealUsedTO.EventTime.ToString("MM") + "\t";
                    txt += mealUsedTO.EventTime.ToString("dd") + "\t";
                    txt += mealUsedTO.EventTime.ToString("HH:mm:ss") + "\t";
                    txt += mealUsedTO.MealType.Trim() + "\t";
                    txt += mealUsedTO.Qty.ToString().Trim() + "\t";
                    txt += mealUsedTO.RestaurantName.Trim() + "\t";
                    txt += mealUsedTO.PointName.Trim() + "\t";
                    if (mealUsedTO.ManualCreated == 1)
                        txt += rm.GetString("lblManual", culture) + "\t";
                    else
                        txt += rm.GetString("chbCardReader", culture) + "\t";

                    if (mealUsedTO.OnlineValidation != -1)
                    {
                        if (mealUsedTO.OnlineValidation == 1)
                            txt += rm.GetString("chbGreen", culture) + "\t";
                        else
                            txt += rm.GetString("chbRed", culture) + "\t";
                    }
                    else
                        txt += "" + "\t";
                    txt += mealUsedTO.ReasonRefused + "\t";
                    if (mealUsedTO.AutoCheck != -1)
                    {
                        if (mealUsedTO.AutoCheck == 1)
                            txt += rm.GetString("chbOK", culture) + "\t";
                        else
                            txt += rm.GetString("chbNOK", culture) + "\t";
                    }
                    else
                        txt += "" + "\t";
                    txt += mealUsedTO.AutoCheckFailureReason + "\t";
                    txt += mealUsedTO.Approved + "\t";
                    txt += mealUsedTO.ApprovedBy + "\t";
                    if (mealUsedTO.ApprovedTime != new DateTime())
                        txt += mealUsedTO.ApprovedTime.ToString("dd.MM.yyyy") + "\t";
                    else
                        txt += "";
                    writer.WriteLine(txt);

                }
                writer.Close();
                MessageBox.Show(rm.GetString("msgGeneratingFinished", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.generateXLSReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void generateXLSReportSummary()
        {
            try
            {

                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);


                object misValue = System.Reflection.Missing.Value;

                Microsoft.Office.Interop.Excel.Worksheet wsAnomalies = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;

                wsAnomalies.Name = "Meals";

                wsAnomalies.Cells[1, 1] = rm.GetString("hdrCompany", culture);
                wsAnomalies.Cells[1, 2] = rm.GetString("hdrWorkingUnit", culture);
                wsAnomalies.Cells[1, 3] = rm.GetString("hdrRestaurant", culture);
                wsAnomalies.Cells[1, 4] = rm.GetString("hdrLine", culture);
                wsAnomalies.Cells[1, 5] = rm.GetString("hdrMealType", culture);
                wsAnomalies.Cells[1, 6] = rm.GetString("hdrGreen", culture);
                wsAnomalies.Cells[1, 7] = rm.GetString("hdrRed", culture);
                wsAnomalies.Cells[1, 8] = rm.GetString("hdrTotal", culture);
                wsAnomalies.Cells[1, 9] = rm.GetString("hdrApprovedXLS", culture);
                wsAnomalies.Cells[1, 10] = rm.GetString("hdrNotApproved", culture);
                wsAnomalies.Cells[1, 11] = rm.GetString("hdrDeleted", culture);
                wsAnomalies.Cells[1, 12] = rm.GetString("hdrBusinessTrip", culture);
                wsAnomalies.Cells[1, 13] = rm.GetString("hdrTotal", culture);

                setRowFontWeight(wsAnomalies, 1, true);

                int i = 2;
                Dictionary<int, Dictionary<int, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>>> dictionaryMeals = new Dictionary<int, Dictionary<int, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>>>();

                int qtyGreen = 0;
                int qtyRed = 0;
                int qtyApp = 0;
                int qtyNotApp = 0;
                int qtyDeleted = 0;
                int qtyBusiness = 0;

                foreach (OnlineMealsUsedTO mealsUsed in listMealsUsed)
                {

                    EmployeeTO empl = emplDict[mealsUsed.EmployeeID];
                  
                    EmployeeAsco4TO asco = ascoDict[mealsUsed.EmployeeID];
                    if (!dictionaryMeals.ContainsKey(asco.IntegerValue4))
                        dictionaryMeals.Add(asco.IntegerValue4, new Dictionary<int, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>>());
                    if (!dictionaryMeals[asco.IntegerValue4].ContainsKey(empl.WorkingUnitID))
                        dictionaryMeals[asco.IntegerValue4].Add(empl.WorkingUnitID, new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>());

                    if (!dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID].ContainsKey(mealsUsed.RestaurantName))
                        dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID].Add(mealsUsed.RestaurantName, new Dictionary<string, Dictionary<string, Dictionary<string, int>>>());

                    if (!dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName].ContainsKey(mealsUsed.PointName))
                        dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName].Add(mealsUsed.PointName, new Dictionary<string, Dictionary<string, int>>());

                    if (!dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName].ContainsKey(mealsUsed.MealType))
                        dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName].Add(mealsUsed.MealType, new Dictionary<string, int>());

                    if (mealsUsed.OnlineValidation == 1)
                    {
                        if (!dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType].ContainsKey("Green"))
                            dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType].Add("Green", mealsUsed.Qty);
                        else
                            dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType]["Green"] += mealsUsed.Qty;

                        qtyGreen += mealsUsed.Qty;
                    }
                    else if (mealsUsed.OnlineValidation == 0)
                    {
                        if (!dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType].ContainsKey("Red"))
                            dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType].Add("Red", mealsUsed.Qty);
                        else
                            dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType]["Red"] += mealsUsed.Qty;

                        qtyRed++;
                        
                    }
                    if (mealsUsed.Approved == Constants.MealApproved)
                    {

                        if (!dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType].ContainsKey(Constants.MealApproved))
                            dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType].Add(Constants.MealApproved, mealsUsed.Qty);
                        else
                            dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType][Constants.MealApproved] += mealsUsed.Qty;
                        qtyApp += mealsUsed.Qty;
                        
                    }
                    else if (mealsUsed.Approved == Constants.MealNotApproved)
                    {

                        if (!dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType].ContainsKey(Constants.MealNotApproved))
                            dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType].Add(Constants.MealNotApproved, mealsUsed.Qty);
                        else
                            dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType][Constants.MealNotApproved] += mealsUsed.Qty;
                        qtyNotApp += mealsUsed.Qty;
                       
                    }
                    else if (mealsUsed.Approved == Constants.MealDeleted)
                    {

                        if (!dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType].ContainsKey(Constants.MealDeleted))
                            dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType].Add(Constants.MealDeleted, mealsUsed.Qty);
                        else
                            dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType][Constants.MealDeleted] += mealsUsed.Qty;
                        qtyDeleted += mealsUsed.Qty;
                       
                    }
                    else if (mealsUsed.Approved == Constants.MealBusinessTrip)
                    {

                        if (!dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType].ContainsKey(Constants.MealBusinessTrip))
                            dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType].Add(Constants.MealBusinessTrip, mealsUsed.Qty);
                        else
                            dictionaryMeals[asco.IntegerValue4][empl.WorkingUnitID][mealsUsed.RestaurantName][mealsUsed.PointName][mealsUsed.MealType][Constants.MealBusinessTrip] += mealsUsed.Qty;
                        qtyBusiness += mealsUsed.Qty;
                       
                    }
                }
                i++;
                foreach (KeyValuePair<int, Dictionary<int, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>>> item in dictionaryMeals)
                {
                    foreach (KeyValuePair<int, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>> subitem1 in dictionaryMeals[item.Key])
                    {
                        foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> subitem2 in dictionaryMeals[item.Key][subitem1.Key])
                        {
                            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, int>>> subitem3 in dictionaryMeals[item.Key][subitem1.Key][subitem2.Key])
                            {
                                foreach (KeyValuePair<string, Dictionary<string, int>> subitem4 in dictionaryMeals[item.Key][subitem1.Key][subitem2.Key][subitem3.Key])
                                {
                                    if (wuDict.ContainsKey(item.Key) && wuDict.ContainsKey(subitem1.Key))
                                    {
                                        wsAnomalies.Cells[i, 1] = wuDict[item.Key].Description;
                                        wsAnomalies.Cells[i, 2] = wuDict[subitem1.Key].Name;
                                    } 
                                        wsAnomalies.Cells[i, 3] = subitem2.Key;
                                    
                                    wsAnomalies.Cells[i, 4] = subitem3.Key;
                                    wsAnomalies.Cells[i, 5] = subitem4.Key;
                                    if (subitem4.Value.ContainsKey("Green"))
                                        wsAnomalies.Cells[i, 6] = subitem4.Value["Green"];
                                    else wsAnomalies.Cells[i, 6] = 0;

                                    if (subitem4.Value.ContainsKey("Red"))
                                        wsAnomalies.Cells[i, 7] = subitem4.Value["Red"];
                                    else wsAnomalies.Cells[i, 7] = 0;

                                    if (subitem4.Value.ContainsKey("Green") && subitem4.Value.ContainsKey("Red"))
                                        wsAnomalies.Cells[i, 8] = subitem4.Value["Red"] + subitem4.Value["Green"];
                                    else
                                    {
                                        if (subitem4.Value.ContainsKey("Red"))
                                            wsAnomalies.Cells[i, 8] = subitem4.Value["Red"];
                                        if (subitem4.Value.ContainsKey("Green"))
                                            wsAnomalies.Cells[i, 8] = subitem4.Value["Green"];
                                    }

                                    if (subitem4.Value.ContainsKey(Constants.MealApproved))
                                        wsAnomalies.Cells[i, 9] = subitem4.Value[Constants.MealApproved];
                                    else wsAnomalies.Cells[i, 9] = 0;

                                    if (subitem4.Value.ContainsKey(Constants.MealNotApproved))
                                        wsAnomalies.Cells[i, 10] = subitem4.Value[Constants.MealNotApproved];
                                    else wsAnomalies.Cells[i, 10] = 0;

                                    if (subitem4.Value.ContainsKey(Constants.MealDeleted))
                                        wsAnomalies.Cells[i, 11] = subitem4.Value[Constants.MealDeleted];
                                    else wsAnomalies.Cells[i, 11] = 0;

                                    if (subitem4.Value.ContainsKey(Constants.MealBusinessTrip))
                                        wsAnomalies.Cells[i, 12] = subitem4.Value[Constants.MealBusinessTrip];
                                    else wsAnomalies.Cells[i, 12] = 0;

                                    if (subitem4.Value.ContainsKey(Constants.MealApproved) && subitem4.Value.ContainsKey(Constants.MealNotApproved) && subitem4.Value.ContainsKey(Constants.MealDeleted) && subitem4.Value.ContainsKey(Constants.MealBusinessTrip))
                                        wsAnomalies.Cells[i, 13] = subitem4.Value[Constants.MealApproved] + subitem4.Value[Constants.MealNotApproved] + subitem4.Value[Constants.MealDeleted] + subitem4.Value[Constants.MealBusinessTrip];
                                    else
                                    {
                                        int sumApproved = 0;
                                        if (subitem4.Value.ContainsKey(Constants.MealApproved))
                                            sumApproved += subitem4.Value[Constants.MealApproved];
                                        if (subitem4.Value.ContainsKey(Constants.MealNotApproved))
                                            sumApproved += subitem4.Value[Constants.MealNotApproved];
                                        if (subitem4.Value.ContainsKey(Constants.MealDeleted))
                                            sumApproved += subitem4.Value[Constants.MealDeleted];
                                        if (subitem4.Value.ContainsKey(Constants.MealBusinessTrip))
                                            sumApproved += subitem4.Value[Constants.MealBusinessTrip];

                                        wsAnomalies.Cells[i, 13] = sumApproved;
                                    }
                                    i++;
                                }
                            }
                        }
                    }

                }


                if (i == 3)
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }
                setCellBorder(wsAnomalies.Cells[i, 1]);
                setCellBorder(wsAnomalies.Cells[i, 2]);
                setCellBorder(wsAnomalies.Cells[i, 3]);
                setCellBorder(wsAnomalies.Cells[i, 4]);
                setCellBorder(wsAnomalies.Cells[i, 5]);
                setCellBorder(wsAnomalies.Cells[i, 6]);
                setCellBorder(wsAnomalies.Cells[i, 7]);
                setCellBorder(wsAnomalies.Cells[i, 8]);
                setCellBorder(wsAnomalies.Cells[i, 9]);
                setCellBorder(wsAnomalies.Cells[i, 10]);
                setCellBorder(wsAnomalies.Cells[i, 11]);
                setCellBorder(wsAnomalies.Cells[i, 12]);
                setCellBorder(wsAnomalies.Cells[i, 13]);
                i++;
                wsAnomalies.Cells[i, 5] = rm.GetString("hdrTotal", culture);
                wsAnomalies.Cells[i, 6] = qtyGreen;
                wsAnomalies.Cells[i, 7] = qtyRed;
                wsAnomalies.Cells[i, 8] = qtyRed + qtyGreen;
                wsAnomalies.Cells[i, 9] = qtyApp;
                wsAnomalies.Cells[i, 10] = qtyNotApp;
                wsAnomalies.Cells[i, 11] = qtyDeleted;
                wsAnomalies.Cells[i, 12] = qtyBusiness;
                wsAnomalies.Cells[i, 13] = qtyDeleted + qtyApp + qtyNotApp + qtyBusiness;

                wsAnomalies.Columns.AutoFit();
                wsAnomalies.Rows.AutoFit();


                string reportName = "MealUsedSummary" + DateTime.Now.Date.ToString("yyyyMMdd");

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = reportName;
                sfd.InitialDirectory = Constants.csvDocPath;
                sfd.Filter = "XLS (*.xls)|*.xls";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                string filePath = sfd.FileName;

                wb.SaveAs(filePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
                                    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                                    Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, misValue, misValue, misValue, misValue);

                wb.Close(true, null, null);
                xla.Workbooks.Close();
                xla.Quit();

                releaseObject(wsAnomalies);


                releaseObject(wb);
                releaseObject(xla);

                System.Threading.Thread.CurrentThread.CurrentCulture = Oldci;

                //System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.generateXLSReport(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsed.releaseObject(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        private void setRowFontWeight(Microsoft.Office.Interop.Excel.Worksheet ws, int row, bool isBold)
        {
            try
            {
                for (int i = 1; i <= colNum; i++)
                {
                    ((Microsoft.Office.Interop.Excel.Range)ws.Cells[row, i]).Font.Bold = isBold;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsed.setCellFontWeight(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void setCellBorder(object cell)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Range range = ((Microsoft.Office.Interop.Excel.Range)cell);
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsed.setCellFontWeight(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (rbAnalitical.Checked)
                    generateXLSReportAnalitical();
                else
                {
                    generateXLSReportSummary();

                }
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.btnOUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }

        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvMealsUsed.SelectedIndices.Count == 1)
                {
                    OnlineMealUsedHist onlineMeal = new OnlineMealUsedHist((OnlineMealsUsedTO)lvMealsUsed.SelectedItems[0].Tag);
                    onlineMeal.ShowDialog();
                }
                else
                {
                    OnlineMealUsedHist onlineMeal = new OnlineMealUsedHist();
                    onlineMeal.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.btnHistory_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (cbCompany.SelectedIndex < 0)
                    {
                        MessageBox.Show(rm.GetString("selectCompany", culture));
                        return;
                    }

                    if (dtFrom.Value > dtTo.Value)
                    {
                        MessageBox.Show(rm.GetString("fromTimeGreater", culture));
                        return;
                    }

                    this.Cursor = Cursors.WaitCursor;

                    string dateError = validateAutoCheckDateInterval(dtFrom.Value);

                    if (!dateError.Trim().Equals(""))
                    {
                        MessageBox.Show(dateError);
                        return;
                    }

                    // get employees for selected company
                    string emplIDs = "";
                    List<EmployeeTO> emplList = new Employee().SearchByWU(Common.Misc.getWorkingUnitHierarhicly((int)cbCompany.SelectedValue, wuList, null));
                    Dictionary<int, EmployeeTO> employees = new Dictionary<int, EmployeeTO>();

                    foreach (EmployeeTO empl in emplList)
                    {
                        emplIDs += empl.EmployeeID + ",";

                        if (!employees.ContainsKey(empl.EmployeeID))
                            employees.Add(empl.EmployeeID, empl);
                    }

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                    // get all time schemas
                    Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema().getDictionary();

                    // get all pass types
                    Dictionary<int, PassTypeTO> passTypesAll = new PassType().SearchDictionary();

                    // get meals to validate
                    Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>> usedMealsDict = new OnlineMealsUsed().SearchMealsForValidation(dtFrom.Value, dtTo.Value, emplIDs);

                    // get all meals
                    Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>> usedMealsValidateDict = new OnlineMealsUsed().SearchMealsValidated(dtFrom.Value.Date.AddDays(-1), dtTo.Value.Date.AddDays(2), dtFrom.Value, dtTo.Value, emplIDs);

                    Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                    Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>> emplDaySchemas = new Dictionary<int, Dictionary<DateTime, WorkTimeSchemaTO>>();
                    Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>> emplDayIntervals = new Dictionary<int, Dictionary<DateTime, List<WorkTimeIntervalTO>>>();

                    // get schedules for selected employees and date interval
                    Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, dtFrom.Value.Date.AddDays(-1), dtTo.Value.Date.AddDays(1), null);

                    // calculated day presence of effective work pass types by employees by days
                    Dictionary<int, Dictionary<DateTime, int>> emplDayPresence = new Dictionary<int, Dictionary<DateTime, int>>();

                    // minimal presence for meal allowance by employee
                    Dictionary<int, int> emplMinPresence = new Dictionary<int, int>();

                    // regular work pass types by employee
                    Dictionary<int, List<int>> emplRegularWork = new Dictionary<int, List<int>>();

                    // official out pass types by employee
                    Dictionary<int, int> emplOfficialOut = new Dictionary<int, int>();

                    // stop working pass types by employee
                    Dictionary<int, int> emplStopWorking = new Dictionary<int, int>();

                    // night work rules by employee
                    Dictionary<int, DateTime> emplNightWorkEnd = new Dictionary<int, DateTime>();
                    
                    // effective types for employee
                    Dictionary<int, List<int>> emplEffTypes = new Dictionary<int, List<int>>();

                    //dictionary for effective work key is working unit id value is list of pass types for effective work
                    Dictionary<int, Dictionary<int, List<int>>> effectiveWorkDict = new Dictionary<int, Dictionary<int, List<int>>>();

                    int expatOutType = -1;
                    if (rules.ContainsKey(Constants.defaultWorkingUnitID) && rules[Constants.defaultWorkingUnitID].ContainsKey((int)Constants.EmployeeTypesFIAT.BC)
                        && rules[Constants.defaultWorkingUnitID][(int)Constants.EmployeeTypesFIAT.BC].ContainsKey(Constants.RuleExpatOutType))
                        expatOutType = rules[Constants.defaultWorkingUnitID][(int)Constants.EmployeeTypesFIAT.BC][Constants.RuleExpatOutType].RuleValue;

                    // get effective work types
                    foreach (int wuID in rules.Keys)
                    {
                        if (!effectiveWorkDict.ContainsKey(wuID))
                            effectiveWorkDict.Add(wuID, new Dictionary<int, List<int>>());

                        foreach (int type in rules[wuID].Keys)
                        {
                            if (!effectiveWorkDict[wuID].ContainsKey(type))
                                effectiveWorkDict[wuID].Add(type, new List<int>());

                            foreach (string ruleName in Constants.effectiveWorkWageTypes())
                            {
                                if (rules[wuID][type].ContainsKey(ruleName))
                                {
                                    int value = rules[wuID][type][ruleName].RuleValue;

                                    if (!effectiveWorkDict[wuID][type].Contains(value))
                                        effectiveWorkDict[wuID][type].Add(value);
                                }
                            }

                            // add work on holiday for expats out as effective type
                            if (type == expatOutType)
                            {
                                if (rules[wuID][type].ContainsKey(Constants.RuleWorkOnHolidayPassType))
                                {
                                    int value = rules[wuID][type][Constants.RuleWorkOnHolidayPassType].RuleValue;
                                    if (!effectiveWorkDict[wuID][type].Contains(value))
                                    {
                                        effectiveWorkDict[wuID][type].Add(value);
                                    }
                                }
                            }

                            // add overtime, bank hours and stop working done
                            if (rules[wuID][type].ContainsKey(Constants.RuleCompanyOvertimePaid))
                            {
                                int value = rules[wuID][type][Constants.RuleCompanyOvertimePaid].RuleValue;

                                if (!effectiveWorkDict[wuID][type].Contains(value))
                                    effectiveWorkDict[wuID][type].Add(value);
                            }

                            if (rules[wuID][type].ContainsKey(Constants.RuleCompanyBankHour))
                            {
                                int value = rules[wuID][type][Constants.RuleCompanyBankHour].RuleValue;

                                if (!effectiveWorkDict[wuID][type].Contains(value))
                                    effectiveWorkDict[wuID][type].Add(value);
                            }

                            if (rules[wuID][type].ContainsKey(Constants.RuleCompanyStopWorkingDone))
                            {
                                int value = rules[wuID][type][Constants.RuleCompanyStopWorkingDone].RuleValue;

                                if (!effectiveWorkDict[wuID][type].Contains(value))
                                    effectiveWorkDict[wuID][type].Add(value);
                            }
                        }
                    }

                    // get intervals and schemas by employees and dates and prepare data for each employee
                    foreach (int emplID in employees.Keys)
                    {
                        EmployeeTO empl = employees[emplID];

                        DateTime currDate = dtFrom.Value.Date.AddDays(-1);

                        List<EmployeeTimeScheduleTO> emplScheduleList = new List<EmployeeTimeScheduleTO>();

                        if (emplSchedules.ContainsKey(emplID))
                            emplScheduleList = emplSchedules[emplID];

                        while (currDate <= dtTo.Value.Date.AddDays(1))
                        {
                            if (!emplDayIntervals.ContainsKey(emplID))
                                emplDayIntervals.Add(emplID, new Dictionary<DateTime, List<WorkTimeIntervalTO>>());

                            if (!emplDayIntervals[emplID].ContainsKey(currDate.Date))
                                emplDayIntervals[emplID].Add(currDate.Date, Common.Misc.getTimeSchemaInterval(currDate.Date, emplScheduleList, schemas));

                            if (!emplDaySchemas.ContainsKey(emplID))
                                emplDaySchemas.Add(emplID, new Dictionary<DateTime, WorkTimeSchemaTO>());

                            WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                            if (emplDayIntervals[emplID][currDate.Date].Count > 0 && schemas.ContainsKey(emplDayIntervals[emplID][currDate.Date][0].TimeSchemaID))
                                sch = schemas[emplDayIntervals[emplID][currDate.Date][0].TimeSchemaID];

                            if (!emplDaySchemas[emplID].ContainsKey(currDate.Date))
                                emplDaySchemas[emplID].Add(currDate.Date, sch);

                            currDate = currDate.AddDays(1).Date;
                        }

                        int emplCompany = Common.Misc.getRootWorkingUnit(empl.WorkingUnitID, wuDict);

                        List<int> effTypes = new List<int>();

                        if (effectiveWorkDict.ContainsKey(emplCompany) && effectiveWorkDict[emplCompany].ContainsKey(empl.EmployeeTypeID))
                            effTypes = effectiveWorkDict[emplCompany][empl.EmployeeTypeID];

                        if (!emplEffTypes.ContainsKey(emplID))
                            emplEffTypes.Add(emplID, new List<int>());

                        emplEffTypes[emplID] = effTypes;

                        if (rules.ContainsKey(emplCompany) && rules[emplCompany].ContainsKey(empl.EmployeeTypeID) && rules[emplCompany][empl.EmployeeTypeID].ContainsKey(Constants.RuleMealMinPresence))
                        {
                            if (!emplMinPresence.ContainsKey(emplID))
                                emplMinPresence.Add(emplID, rules[emplCompany][empl.EmployeeTypeID][Constants.RuleMealMinPresence].RuleValue);
                            else
                                emplMinPresence[emplID] = rules[emplCompany][empl.EmployeeTypeID][Constants.RuleMealMinPresence].RuleValue;
                        }

                        if (rules.ContainsKey(emplCompany) && rules[emplCompany].ContainsKey(empl.EmployeeTypeID))
                        {
                            if (rules[emplCompany][empl.EmployeeTypeID].ContainsKey(Constants.RuleCompanyRegularWork))
                            {
                                if (!emplRegularWork.ContainsKey(emplID))
                                    emplRegularWork.Add(emplID, new List<int>());

                                emplRegularWork[emplID].Add(rules[emplCompany][empl.EmployeeTypeID][Constants.RuleCompanyRegularWork].RuleValue);
                            }

                            if (empl.EmployeeTypeID == expatOutType && rules[emplCompany][empl.EmployeeTypeID].ContainsKey(Constants.RuleWorkOnHolidayPassType))
                            {
                                if (!emplRegularWork.ContainsKey(emplID))
                                    emplRegularWork.Add(emplID, new List<int>());

                                emplRegularWork[emplID].Add(rules[emplCompany][empl.EmployeeTypeID][Constants.RuleWorkOnHolidayPassType].RuleValue);
                            }
                        }                        

                        if (rules.ContainsKey(emplCompany) && rules[emplCompany].ContainsKey(empl.EmployeeTypeID) && rules[emplCompany][empl.EmployeeTypeID].ContainsKey(Constants.RuleCompanyOfficialOut))
                        {
                            if (!emplOfficialOut.ContainsKey(emplID))
                                emplOfficialOut.Add(emplID, rules[emplCompany][empl.EmployeeTypeID][Constants.RuleCompanyOfficialOut].RuleValue);
                            else
                                emplOfficialOut[emplID] = rules[emplCompany][empl.EmployeeTypeID][Constants.RuleCompanyOfficialOut].RuleValue;
                        }

                        if (rules.ContainsKey(emplCompany) && rules[emplCompany].ContainsKey(empl.EmployeeTypeID) && rules[emplCompany][empl.EmployeeTypeID].ContainsKey(Constants.RuleCompanyStopWorking))
                        {
                            if (!emplStopWorking.ContainsKey(emplID))
                                emplStopWorking.Add(emplID, rules[emplCompany][empl.EmployeeTypeID][Constants.RuleCompanyStopWorking].RuleValue);
                            else
                                emplStopWorking[emplID] = rules[emplCompany][empl.EmployeeTypeID][Constants.RuleCompanyStopWorking].RuleValue;
                        }

                        if (rules.ContainsKey(emplCompany) && rules[emplCompany].ContainsKey(empl.EmployeeTypeID) && rules[emplCompany][empl.EmployeeTypeID].ContainsKey(Constants.RuleNightWork))
                        {
                            if (!emplNightWorkEnd.ContainsKey(emplID))
                                emplNightWorkEnd.Add(emplID, rules[emplCompany][empl.EmployeeTypeID][Constants.RuleNightWork].RuleDateTime2);
                            else
                                emplNightWorkEnd[emplID] = rules[emplCompany][empl.EmployeeTypeID][Constants.RuleNightWork].RuleDateTime2;
                        }

                        if (rules.ContainsKey(emplCompany) && rules[emplCompany].ContainsKey(empl.EmployeeTypeID) && rules[emplCompany][empl.EmployeeTypeID].ContainsKey(Constants.RuleCompanyInitialOvertime))
                        {
                            emplEffTypes[emplID].Add(rules[emplCompany][empl.EmployeeTypeID][Constants.RuleCompanyInitialOvertime].RuleValue);                            
                        }
                    }

                    // get pairs for selected employees and date interval
                    List<DateTime> dateList = new List<DateTime>();
                    DateTime currentDate = dtFrom.Value.Date.AddDays(-1);
                    while (currentDate.Date <= dtTo.Value.Date.AddDays(1))
                    {
                        dateList.Add(currentDate.Date);

                        currentDate = currentDate.AddDays(1).Date;
                    }
                    List<IOPairProcessedTO> pairs = new IOPairProcessed().SearchAllPairsForEmpl(emplIDs, dateList, "");

                    // get day pairs for employees
                    foreach (IOPairProcessedTO pair in pairs)
                    {
                        if (!emplDayPairs.ContainsKey(pair.EmployeeID))
                            emplDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                        if (!emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                            emplDayPairs[pair.EmployeeID].Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                        emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date].Add(pair);
                    }

                    Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplBelongingDayPairs = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();

                    // go through all pairs, set them to belonging day and calculate day presence by rules for meals
                    foreach (IOPairProcessedTO pair in pairs)
                    {
                        DateTime pairDate = pair.IOPairDate.Date;

                        List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                        if (emplDayPairs.ContainsKey(pair.EmployeeID) && emplDayPairs[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                            dayPairs = emplDayPairs[pair.EmployeeID][pair.IOPairDate.Date];

                        WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                        if (emplDaySchemas.ContainsKey(pair.EmployeeID) && emplDaySchemas[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                            sch = emplDaySchemas[pair.EmployeeID][pair.IOPairDate.Date];

                        List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();
                        if (emplDayIntervals.ContainsKey(pair.EmployeeID) && emplDayIntervals[pair.EmployeeID].ContainsKey(pair.IOPairDate.Date))
                            dayIntervals = emplDayIntervals[pair.EmployeeID][pair.IOPairDate.Date];

                        bool previousDayPair = Common.Misc.isPreviousDayPair(pair, passTypesAll, dayPairs, sch, dayIntervals);

                        if (previousDayPair)
                            pairDate = pairDate.AddDays(-1);

                        if (pairDate.Date >= dtFrom.Value.Date.AddDays(-1))
                        {
                            if (!emplBelongingDayPairs.ContainsKey(pair.EmployeeID))
                                emplBelongingDayPairs.Add(pair.EmployeeID, new Dictionary<DateTime, List<IOPairProcessedTO>>());
                            if (!emplBelongingDayPairs[pair.EmployeeID].ContainsKey(pairDate.Date))
                                emplBelongingDayPairs[pair.EmployeeID].Add(pairDate.Date, new List<IOPairProcessedTO>());

                            emplBelongingDayPairs[pair.EmployeeID][pairDate.Date].Add(pair);
                        }
                    }

                    foreach (int emplID in emplBelongingDayPairs.Keys)
                    {
                        List<int> effTypes = new List<int>();

                        if (emplEffTypes.ContainsKey(emplID))
                            effTypes = emplEffTypes[emplID];
                        
                        if (!emplDayPresence.ContainsKey(emplID))
                            emplDayPresence.Add(emplID, new Dictionary<DateTime, int>());

                        foreach (DateTime date in emplBelongingDayPairs[emplID].Keys)
                        {
                            if (!emplDayPresence[emplID].ContainsKey(date))
                                emplDayPresence[emplID].Add(date, 0);

                            List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();

                            if (emplDayIntervals.ContainsKey(emplID) && emplDayIntervals[emplID].ContainsKey(date))
                                dayIntervals = emplDayIntervals[emplID][date];

                            int schPresence = 0;
                            int outSchPresence = 0;
                            bool overtimeConnected = false;
                            for (int i = 0; i < emplBelongingDayPairs[emplID][date].Count; i++)
                            {
                                IOPairProcessedTO pair = emplBelongingDayPairs[emplID][date][i];

                                if (effTypes.Contains(pair.PassTypeID) || (emplStopWorking.ContainsKey(pair.EmployeeID) && pair.PassTypeID == emplStopWorking[pair.EmployeeID]))                                    
                                {
                                    int pairDuration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                                    if (pair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                        pairDuration++;

                                    if (passTypesAll.ContainsKey(pair.PassTypeID) && passTypesAll[pair.PassTypeID].IsPass == Constants.overtimePassType)
                                    {
                                        outSchPresence += pairDuration;

                                        // check conectivity with regular work pairs
                                        if ((i > 0 && passTypesAll.ContainsKey(emplBelongingDayPairs[emplID][date][i - 1].PassTypeID)
                                            && passTypesAll[emplBelongingDayPairs[emplID][date][i - 1].PassTypeID].IsPass != Constants.overtimePassType
                                            && pair.StartTime.Equals(emplBelongingDayPairs[emplID][date][i - 1].EndTime))
                                            || (i < emplBelongingDayPairs[emplID][date].Count - 1 && passTypesAll.ContainsKey(emplBelongingDayPairs[emplID][date][i + 1].PassTypeID)
                                            && passTypesAll[emplBelongingDayPairs[emplID][date][i + 1].PassTypeID].IsPass != Constants.overtimePassType
                                            && pair.EndTime.Equals(emplBelongingDayPairs[emplID][date][i + 1].StartTime)))
                                            overtimeConnected = true;
                                    }
                                    else
                                        schPresence += pairDuration;
                                }
                            }

                            if (overtimeConnected)
                                emplDayPresence[emplID][date] += schPresence + outSchPresence;
                            else if (isWorkingDay(dayIntervals))
                                emplDayPresence[emplID][date] += schPresence;
                            else
                                emplDayPresence[emplID][date] += outSchPresence;
                        }
                    }

                    bool allValidate = true;
                    foreach (int emplID in usedMealsDict.Keys)
                    {
                        Dictionary<DateTime, List<OnlineMealsUsedTO>> emplMealsToValidate = new Dictionary<DateTime, List<OnlineMealsUsedTO>>();
                        Dictionary<DateTime, List<OnlineMealsUsedTO>> emplMeals = new Dictionary<DateTime, List<OnlineMealsUsedTO>>();
                        if (usedMealsValidateDict.ContainsKey(emplID))
                            emplMeals = usedMealsValidateDict[emplID];

                        foreach (DateTime date in usedMealsDict[emplID].Keys)
                        {
                            foreach (OnlineMealsUsedTO meal in usedMealsDict[emplID][date])
                            {
                                emplMealsToValidate = usedMealsDict[emplID];

                                List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();
                                List<WorkTimeIntervalTO> prevDayIntervals = new List<WorkTimeIntervalTO>();
                                List<WorkTimeIntervalTO> nextDayIntervals = new List<WorkTimeIntervalTO>();
                                if (emplDayIntervals.ContainsKey(meal.EmployeeID))
                                {
                                    if (emplDayIntervals[meal.EmployeeID].ContainsKey(meal.EventTime.Date))
                                        dayIntervals = emplDayIntervals[meal.EmployeeID][meal.EventTime.Date];
                                    if (emplDayIntervals[meal.EmployeeID].ContainsKey(meal.EventTime.Date.AddDays(-1)))
                                        prevDayIntervals = emplDayIntervals[meal.EmployeeID][meal.EventTime.Date.AddDays(-1)];
                                    if (emplDayIntervals[meal.EmployeeID].ContainsKey(meal.EventTime.Date.AddDays(1)))
                                        nextDayIntervals = emplDayIntervals[meal.EmployeeID][meal.EventTime.Date.AddDays(1)];
                                }

                                string error = validateMeal(meal, emplBelongingDayPairs, emplDayPresence, emplMinPresence, emplMeals, emplMealsToValidate, passTypesAll, emplEffTypes,
                                    emplRegularWork, emplOfficialOut, emplNightWorkEnd, dayIntervals, nextDayIntervals, prevDayIntervals);
                                OnlineMealsUsedHistTO mealHist = new OnlineMealsUsedHistTO();
                                if (error.Trim().Equals(""))
                                {
                                    // if meal is valid set auto check values
                                    if (meal.AutoCheck != Constants.yesInt)
                                        mealHist = new OnlineMealsUsedHistTO(meal);

                                    meal.AutoCheck = Constants.yesInt;
                                    meal.AutoCheckTime = DateTime.Now;
                                    meal.AutoCheckFailureReason = "";

                                    meal.Approved = "";
                                    meal.ApprovedBy = "";
                                    meal.ApprovedDesc = "";
                                    meal.ApprovedTime = new DateTime();
                                }
                                else
                                {
                                    // if meal is not valid set auto check values, approved to not approved status
                                    if (meal.AutoCheck != Constants.noInt || (meal.AutoCheck == Constants.noInt && meal.AutoCheckFailureReason != error))
                                        mealHist = new OnlineMealsUsedHistTO(meal);

                                    meal.AutoCheck = Constants.noInt;
                                    meal.AutoCheckFailureReason = error;
                                    meal.AutoCheckTime = DateTime.Now;
                                    meal.Approved = Constants.MealNotApproved;
                                    meal.ApprovedBy = Constants.AutoCheckUser;
                                    meal.ApprovedDesc = Constants.MealNotApproved;
                                    meal.ApprovedTime = DateTime.Now;
                                }

                                // move meal to hist if it is changed, else just change modification values
                                allValidate = changeMeal(meal, mealHist) && allValidate;
                            }
                        }
                    }

                    if (allValidate)
                        MessageBox.Show(rm.GetString("allMealsValidate", culture));
                    else
                        MessageBox.Show(rm.GetString("allMealsNotValidate", culture));

                    // show not valid meals in search list
                    previewValidateMeals();
                }
                catch (Exception ex)
                {
                    log.writeLog(DateTime.Now + " OnlineMealsUsed.btnValidate_Click(): " + ex.Message + "\n");
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.btnValidate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private bool previousDayMeal(DateTime mealTime, DateTime nightEnd, List<WorkTimeIntervalTO> emplDayIntervals, List<IOPairProcessedTO> prevDayPairs)
        {
            try
            {
                bool prevDayMeal = false;
                if (mealTime.TimeOfDay <= nightEnd.TimeOfDay)
                {
                    // check if employees has night shift                    
                    foreach (WorkTimeIntervalTO interval in emplDayIntervals)
                    {
                        if (interval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0) && interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes > 0)
                        {
                            prevDayMeal = true;
                            break;
                        }
                    }

                    // if there is no night shift check if there is previous day pair in which meal has happend
                    if (!prevDayMeal)
                    {
                        foreach (IOPairProcessedTO pair in prevDayPairs)
                        {
                            if (mealTime >= pair.StartTime && mealTime <= pair.EndTime)
                            {
                                prevDayMeal = true;
                                break;
                            }
                        }
                    }
                }

                return prevDayMeal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string validateMeal(OnlineMealsUsedTO meal, Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> emplBelongingDayPairs,
            Dictionary<int, Dictionary<DateTime, int>> emplDayPresence, Dictionary<int, int> emplMinPresence, Dictionary<DateTime, List<OnlineMealsUsedTO>> emplMeals,
            Dictionary<DateTime, List<OnlineMealsUsedTO>> emplMealsToValidate, Dictionary<int, PassTypeTO> passTypes, Dictionary<int, List<int>> effTypes,
            Dictionary<int, List<int>> emplRegularWork, Dictionary<int, int> emplOfficialOut, Dictionary<int, DateTime> emplNightWorkEnd, 
            List<WorkTimeIntervalTO> emplDayIntervals, List<WorkTimeIntervalTO> emplNextDayIntervals, List<WorkTimeIntervalTO> emplPrevDayIntervals)
        {
            try
            {
                string error = "";

                // if meal is before night work end time, it belongs to previous day
                DateTime date = meal.EventTime.Date;

                DateTime nightEnd = new DateTime();

                if (emplNightWorkEnd.ContainsKey(meal.EmployeeID))
                    nightEnd = emplNightWorkEnd[meal.EmployeeID];

                List<WorkTimeIntervalTO> dayIntervals = emplDayIntervals;
                List<WorkTimeIntervalTO> nextDayIntervals = emplNextDayIntervals;

                List<IOPairProcessedTO> prevDayPairs = new List<IOPairProcessedTO>();
                if (emplBelongingDayPairs.ContainsKey(meal.EmployeeID) && emplBelongingDayPairs[meal.EmployeeID].ContainsKey(meal.EventTime.Date.AddDays(-1)))
                    prevDayPairs = emplBelongingDayPairs[meal.EmployeeID][meal.EventTime.Date.AddDays(-1)];

                if (previousDayMeal(meal.EventTime, nightEnd, emplDayIntervals, prevDayPairs))
                {
                    date = date.AddDays(-1);
                    dayIntervals = emplPrevDayIntervals;
                    nextDayIntervals = emplDayIntervals;
                }

                // check if there already exists valid meal for this day
                // compare meal with meals form date day after night end and meals from next date day before night end
                List<OnlineMealsUsedTO> dayMeals = new List<OnlineMealsUsedTO>();
                if (emplMeals.ContainsKey(date))
                {
                    foreach (OnlineMealsUsedTO dateMeal in emplMeals[date])
                    {
                        List<IOPairProcessedTO> previousDayPairs = new List<IOPairProcessedTO>();
                        if (emplBelongingDayPairs.ContainsKey(dateMeal.EmployeeID) && emplBelongingDayPairs[dateMeal.EmployeeID].ContainsKey(dateMeal.EventTime.Date.AddDays(-1)))
                            previousDayPairs = emplBelongingDayPairs[dateMeal.EmployeeID][dateMeal.EventTime.Date.AddDays(-1)];

                        if (dateMeal.EventTime.TimeOfDay > nightEnd.TimeOfDay || !previousDayMeal(dateMeal.EventTime, nightEnd, dayIntervals, previousDayPairs))
                            dayMeals.Add(dateMeal);
                    }
                }

                if (emplMeals.ContainsKey(date.AddDays(1)))
                {
                    foreach (OnlineMealsUsedTO dateMeal in emplMeals[date.AddDays(1)])
                    {
                        List<IOPairProcessedTO> previousDayPairs = new List<IOPairProcessedTO>();
                        if (emplBelongingDayPairs.ContainsKey(dateMeal.EmployeeID) && emplBelongingDayPairs[dateMeal.EmployeeID].ContainsKey(dateMeal.EventTime.Date.AddDays(-1)))
                            previousDayPairs = emplBelongingDayPairs[dateMeal.EmployeeID][dateMeal.EventTime.Date.AddDays(-1)];
                        
                        if (dateMeal.EventTime.TimeOfDay <= nightEnd.TimeOfDay && previousDayMeal(dateMeal.EventTime, nightEnd, nextDayIntervals, previousDayPairs))
                            dayMeals.Add(dateMeal);
                    }
                }

                foreach (OnlineMealsUsedTO dayMeal in dayMeals)
                {
                    if (dayMeal.TransactionID == meal.TransactionID)
                        continue;

                    if (dayMeal.Approved == Constants.MealApproved || dayMeal.AutoCheck == Constants.yesInt)
                    {
                        error = Constants.AutoCheckFailureReason.DOUBLE_REGISTRATION.ToString().Trim();
                        break;
                    }
                }

                if (!error.Trim().Equals(""))
                    return error;

                List<OnlineMealsUsedTO> dayMealsToValidate = new List<OnlineMealsUsedTO>();
                if (emplMealsToValidate.ContainsKey(date))
                {
                    foreach (OnlineMealsUsedTO dateMeal in emplMealsToValidate[date])
                    {
                        List<IOPairProcessedTO> previousDayPairs = new List<IOPairProcessedTO>();
                        if (emplBelongingDayPairs.ContainsKey(dateMeal.EmployeeID) && emplBelongingDayPairs[dateMeal.EmployeeID].ContainsKey(dateMeal.EventTime.Date.AddDays(-1)))
                            previousDayPairs = emplBelongingDayPairs[dateMeal.EmployeeID][dateMeal.EventTime.Date.AddDays(-1)];

                        if (dateMeal.EventTime.TimeOfDay > nightEnd.TimeOfDay || !previousDayMeal(dateMeal.EventTime, nightEnd, dayIntervals, previousDayPairs))
                            dayMealsToValidate.Add(dateMeal);
                    }
                }

                if (emplMealsToValidate.ContainsKey(date.AddDays(1)))
                {
                    foreach (OnlineMealsUsedTO dateMeal in emplMealsToValidate[date.AddDays(1)])
                    {
                        List<IOPairProcessedTO> previousDayPairs = new List<IOPairProcessedTO>();
                        if (emplBelongingDayPairs.ContainsKey(dateMeal.EmployeeID) && emplBelongingDayPairs[dateMeal.EmployeeID].ContainsKey(dateMeal.EventTime.Date.AddDays(-1)))
                            previousDayPairs = emplBelongingDayPairs[dateMeal.EmployeeID][dateMeal.EventTime.Date.AddDays(-1)];
                        
                        if (dateMeal.EventTime.TimeOfDay <= nightEnd.TimeOfDay && previousDayMeal(dateMeal.EventTime, nightEnd, nextDayIntervals, previousDayPairs))
                            dayMealsToValidate.Add(dateMeal);
                    }
                }

                // check if there already exists valid meal for this day in currently validate
                foreach (OnlineMealsUsedTO dayMeal in dayMealsToValidate)
                {
                    if (dayMeal.TransactionID == meal.TransactionID)
                        continue;

                    if (dayMeal.Approved == Constants.MealApproved || dayMeal.AutoCheck == Constants.yesInt)
                    {
                        error = Constants.AutoCheckFailureReason.DOUBLE_REGISTRATION.ToString().Trim();
                        break;
                    }
                }

                if (!error.Trim().Equals(""))
                    return error;

                // check minimal presence for meal
                int presence = 0;
                if (emplDayPresence.ContainsKey(meal.EmployeeID) && emplDayPresence[meal.EmployeeID].ContainsKey(date.Date))
                    presence = emplDayPresence[meal.EmployeeID][date.Date];

                int minPresence = 0;
                if (emplMinPresence.ContainsKey(meal.EmployeeID))
                    minPresence = emplMinPresence[meal.EmployeeID];

                if (presence == 0 || presence < minPresence)
                    error = Constants.AutoCheckFailureReason.NO_MINIMAL_PRESENCE.ToString().Trim();

                if (!error.Trim().Equals(""))
                    return error;

                // check if meal is inside efective work
                bool effWorkFound = false;
                bool notWorkingDay = true;
                bool wholeDayAbsence = false;
                bool unjustifiedDay = true;
                if (emplBelongingDayPairs.ContainsKey(meal.EmployeeID) && emplBelongingDayPairs[meal.EmployeeID].ContainsKey(date))
                {
                    foreach (IOPairProcessedTO pair in emplBelongingDayPairs[meal.EmployeeID][date])
                    {
                        DateTime pairEnd = pair.EndTime;

                        // prolong end to midnight
                        if (pairEnd.TimeOfDay == new TimeSpan(23, 59, 0))
                            pairEnd = pairEnd.AddSeconds(59);

                        if (pair.PassTypeID != Constants.absence)
                            unjustifiedDay = false;

                        if (passTypes.ContainsKey(pair.PassTypeID) && passTypes[pair.PassTypeID].IsPass == Constants.wholeDayAbsence)
                            wholeDayAbsence = true;

                        if (passTypes.ContainsKey(pair.PassTypeID) && passTypes[pair.PassTypeID].IsPass != Constants.overtimePassType)
                            notWorkingDay = false;

                        //if (meal.EventTime.TimeOfDay >= pair.StartTime.TimeOfDay && meal.EventTime.TimeOfDay <= pairEnd.TimeOfDay)
                        //{
                            // check if pair is regular work, official out or some of overtime effective types
                            if ((emplRegularWork.ContainsKey(meal.EmployeeID) && emplRegularWork[meal.EmployeeID].Contains(pair.PassTypeID))
                                || (emplOfficialOut.ContainsKey(meal.EmployeeID) && pair.PassTypeID == emplOfficialOut[meal.EmployeeID])
                                || (effTypes.ContainsKey(meal.EmployeeID) && effTypes[meal.EmployeeID].Contains(pair.PassTypeID)
                                && passTypes.ContainsKey(pair.PassTypeID) && passTypes[pair.PassTypeID].IsPass == Constants.overtimePassType))
                            {
                                effWorkFound = true;
                                break;
                            }
                        //}
                    }
                }
                else
                    unjustifiedDay = false;

                if (!effWorkFound)
                {
                    if (notWorkingDay)
                        error = Constants.AutoCheckFailureReason.NOT_WORKING_DAY.ToString().Trim();
                    else if (unjustifiedDay)
                        error = Constants.AutoCheckFailureReason.UNJUSTIFED_DAY.ToString().Trim();
                    else if (wholeDayAbsence)
                        error = Constants.AutoCheckFailureReason.WHOLE_DAY_ABSENCE.ToString().Trim();
                    //else
                    //    error = Constants.AutoCheckFailureReason.NO_EFFECTIVE_WORK.ToString().Trim();
                }

                return error;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.validateMeal(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool changeMeal(OnlineMealsUsedTO mealTO, OnlineMealsUsedHistTO histTO)
        {
            try
            {
                mealTO.ModifiedBy = Constants.AutoCheckUser;
                OnlineMealsUsed meal = new OnlineMealsUsed();
                meal.OnlineMealsUsedTO = mealTO;
                // if there is no meal for history, set modified values and update meal, else, move old meal to history before updating
                if (histTO.TransactionID == -1)
                    return meal.Update(true);
                else
                {
                    if (meal.BeginTransaction())
                    {
                        try
                        {
                            bool updated = true;

                            updated = meal.Update(false);

                            if (updated)
                            {
                                OnlineMealsUsedHist hist = new OnlineMealsUsedHist();
                                hist.OnlineMealsUsedHistTO = histTO;
                                hist.SetTransaction(meal.GetTransaction());

                                updated = hist.Save(false) > 0;
                            }

                            if (updated)
                            {
                                meal.CommitTransaction();
                                return true;
                            }
                            else
                            {
                                if (meal.GetTransaction() != null)
                                    meal.RollbackTransaction();
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (meal.GetTransaction() != null)
                                meal.RollbackTransaction();
                            throw ex;
                        }
                    }
                    else
                        return false;
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.changeMeal(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool isWorkingDay(List<WorkTimeIntervalTO> intervals)
        {
            try
            {
                TimeSpan ts = new TimeSpan(0, 0, 0);
                for (int i = 0; i < intervals.Count; i++)
                {
                    if (intervals[i].StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                        continue;

                    ts = ts.Add(intervals[i].EndTime.TimeOfDay.Subtract(intervals[i].StartTime.TimeOfDay));
                }

                return (ts > new TimeSpan(0, 0, 0));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.isWorkingDay(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void previewValidateMeals()
        {
            try
            {
                rbWU.Checked = true;
                rbWU_CheckedChanged(this, new EventArgs());

                cbWU.SelectedValue = (int)cbCompany.SelectedValue;
                cbWU_SelectedIndexChanged(this, new EventArgs());

                lvRestaurant.SelectedItems.Clear();
                lvLine.SelectedItems.Clear();
                lvMealType.SelectedItems.Clear();

                numQtyFrom.Value = 0;
                numQtyTo.Value = 0;

                emplDateChanged = false;
                dtpFrom.Value = dtFrom.Value;
                emplDateChanged = false;
                dtpTo.Value = dtTo.Value;
                //tbTimeFrom.Text = dtFrom.Value.ToString("HH:mm");
                //tbTimeTo.Text = dtTo.Value.ToString("HH:mm");

                cbManual.Checked = cbReader.Checked = false;

                cbSelectAllEmpl.Checked = false;
                cbSelectAllEmpl_CheckedChanged(this, new EventArgs());

                cbGreen.Checked = cbRed.Checked = false;
                cbRed_CheckedChanged(this, new EventArgs());
                lvReason.SelectedItems.Clear();

                cbOK.Checked = cbNOK.Checked = false;
                cbNOK_CheckedChanged(this, new EventArgs());
                lvValidationReasonAuto.SelectedItems.Clear();

                chbApproval.Checked = false;
                chbApproval_CheckedChanged(this, new EventArgs());
                lvStatus.SelectedItems.Clear();
                lvOperater.SelectedItems.Clear();

                btnSearch.PerformClick();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.previewValidateMeals(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private string validateAutoCheckDateInterval(DateTime date)
        {
            try
            {
                int hrsscCutOffDate = -1;
                DateTime nightWorkEnd = new DateTime();

                EmployeeTO empl = new EmployeeTO();

                // get login employee
                foreach (int emplID in ascoDict.Keys)
                {
                    if (ascoDict[emplID].NVarcharValue5.Equals(NotificationController.GetLogInUser().UserID))
                    {
                        if (emplDict.ContainsKey(emplID))
                            empl = emplDict[emplID];

                        break;
                    }
                }

                // get hrssc cut off date
                int company = Common.Misc.getRootWorkingUnit(empl.WorkingUnitID, wuDict);

                if (rules.ContainsKey(company) && rules[company].ContainsKey(empl.EmployeeTypeID) && rules[company][empl.EmployeeTypeID].ContainsKey(Constants.RuleHRSSCCutOffDate))
                    hrsscCutOffDate = rules[company][empl.EmployeeTypeID][Constants.RuleHRSSCCutOffDate].RuleValue;

                if (rules.ContainsKey(company) && rules[company].ContainsKey(empl.EmployeeTypeID) && rules[company][empl.EmployeeTypeID].ContainsKey(Constants.RuleNightWork))
                    nightWorkEnd = rules[company][empl.EmployeeTypeID][Constants.RuleNightWork].RuleDateTime2;

                DateTime currentMonth = DateTime.Now.AddDays(-DateTime.Now.Day + 1).Date;
                DateTime minDateTimeCurrMonth = new DateTime(currentMonth.Year, currentMonth.Month, currentMonth.Day, nightWorkEnd.Hour, nightWorkEnd.Minute, 0);
                DateTime minDateTimePrevMonth = minDateTimeCurrMonth.AddMonths(-1);

                if (Common.Misc.countWorkingDays(DateTime.Now.Date, null) > hrsscCutOffDate)
                {
                    if (date < minDateTimeCurrMonth)
                        return rm.GetString("minMonthMealValidate", culture) + " " + minDateTimeCurrMonth.ToString("dd.MM.yyyy. HH:mm");
                    else
                        return "";
                }
                else if (date < minDateTimePrevMonth)
                    return rm.GetString("minMonthMealValidate", culture) + " " + minDateTimePrevMonth.ToString("dd.MM.yyyy. HH:mm");
                else
                    return "";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.validateAutoCheckDateInterval(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnBusinessTrip_Click(object sender, EventArgs e)
        {
            try
            {
                OnlineMealsUsed onlineMeal = new OnlineMealsUsed();
                OnlineMealsUsedHist onlineMealHist = new OnlineMealsUsedHist();

                string error = "";
                bool allApproved = true;
                foreach (ListViewItem item in lvMealsUsed.SelectedItems)
                {
                    OnlineMealsUsedTO mealUsed = (OnlineMealsUsedTO)item.Tag;

                    string validMealChange = validateAutoCheckDateInterval(mealUsed.EventTime).Trim();

                    if (!validMealChange.Trim().Equals(""))
                    {
                        error = validMealChange;
                        allApproved = false;
                        continue;
                    }

                    if (onlineMeal.BeginTransaction())
                    {
                        try
                        {
                            bool approved = true;

                            OnlineMealsUsedHistTO mealUsedHist = new OnlineMealsUsedHistTO();
                            toHist(mealUsed, mealUsedHist);

                            onlineMealHist.OnlineMealsUsedHistTO = mealUsedHist;
                            onlineMealHist.SetTransaction(onlineMeal.GetTransaction());

                            approved = approved && onlineMealHist.Save(false) > 0;

                            if (approved)
                            {
                                mealUsed.Approved = Constants.MealBusinessTrip;
                                mealUsed.ApprovedDesc = Constants.MealBusinessTrip;
                                mealUsed.ApprovedTime = DateTime.Now;
                                mealUsed.ApprovedBy = logInUser.UserID;

                                onlineMeal.OnlineMealsUsedTO = mealUsed;

                                approved = approved && onlineMeal.Update(false);
                            }

                            if (approved)
                                onlineMeal.CommitTransaction();
                            else
                            {
                                if (onlineMeal.GetTransaction() != null)
                                    onlineMeal.RollbackTransaction();

                                allApproved = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (onlineMeal.GetTransaction() != null)
                                onlineMeal.RollbackTransaction();

                            allApproved = false;

                            log.writeLog(DateTime.Now + " OnlineMealsUsed.btnBusinessTrip_Click(): " + ex.Message + "\n");
                        }
                    }
                    else
                        allApproved = false;
                }

                if (allApproved)
                {
                    MessageBox.Show(rm.GetString("onlineMealUsedUpdated", culture) + " " + error.Trim());
                }
                else
                {
                    MessageBox.Show(rm.GetString("onlineMealUsedNotUpdated", culture) + " " + error.Trim());
                }

                btnSearch.PerformClick();
            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " OnlineMealsUsed.btnBusinessTrip_Click(): " + ex.Message + "\n");
                MessageBox.Show(rm.GetString("onlineMealUsedNotUpdated", culture) + " " + ex.Message);
            }
            finally
            {

                this.Cursor = Cursors.Arrow;
            }
        }

        public void populateListView(List<OnlineMealsUsedTO> listOnlineMealsList, int startIndex)
        {
            try
            {
                if (listOnlineMealsList.Count > Constants.recordsPerPage)
                {
                    btnPrev.Visible = true;
                    btnNext.Visible = true;
                }
                else
                {
                    btnPrev.Visible = false;
                    btnNext.Visible = false;
                }

                lvMealsUsed.BeginUpdate();
                lvMealsUsed.Items.Clear();
                int qty = 0;
                if (listOnlineMealsList.Count > 0)
                {
                    if ((startIndex >= 0) && (startIndex < listOnlineMealsList.Count))
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
                        if (lastIndex >= listOnlineMealsList.Count)
                        {
                            btnNext.Enabled = false;
                            lastIndex = listOnlineMealsList.Count;
                        }
                        else
                        {
                            btnNext.Enabled = true;
                        }

                        //ListViewItem[] lvItems = new ListViewItem[lastIndex - startIndex];

                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            OnlineMealsUsedTO mealUsedTO = listOnlineMealsList[i];
                            ListViewItem item = new ListViewItem();

                            EmployeeTO empl = new EmployeeTO();
                            if (emplDict.ContainsKey(mealUsedTO.EmployeeID))
                                empl = emplDict[mealUsedTO.EmployeeID];
                            string stringone = "";
                            string branch = "";

                            if (ascoDict.ContainsKey(mealUsedTO.EmployeeID))
                            {
                                stringone = ascoDict[mealUsedTO.EmployeeID].NVarcharValue2.Trim();
                                branch = ascoDict[mealUsedTO.EmployeeID].NVarcharValue6.Trim();
                            }

                            WorkingUnitTO costCentre = new WorkingUnitTO();
                            if (wuDict.ContainsKey(empl.WorkingUnitID)
                                && wuDict.ContainsKey(wuDict[empl.WorkingUnitID].ParentWorkingUID)
                                && wuDict.ContainsKey(wuDict[wuDict[empl.WorkingUnitID].ParentWorkingUID].ParentWorkingUID))
                                costCentre = wuDict[wuDict[wuDict[empl.WorkingUnitID].ParentWorkingUID].ParentWorkingUID];

                            item.Text = mealUsedTO.EmployeeID.ToString().Trim();
                            item.SubItems.Add(stringone);
                            item.SubItems.Add(empl.FirstName.Trim());
                            item.SubItems.Add(empl.LastName.Trim());
                            item.SubItems.Add(costCentre.Name);
                            item.SubItems.Add(costCentre.Description);
                            item.SubItems.Add(branch);
                            item.SubItems.Add(culture.Calendar.GetWeekOfYear(mealUsedTO.EventTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString());
                            item.SubItems.Add(mealUsedTO.EventTime.ToString("yyyy"));
                            item.SubItems.Add(mealUsedTO.EventTime.ToString("MM"));
                            item.SubItems.Add(mealUsedTO.EventTime.ToString("dd"));
                            item.SubItems.Add(mealUsedTO.EventTime.ToString("HH:mm:ss"));
                            item.SubItems.Add(mealUsedTO.MealType.Trim());
                            item.SubItems.Add(mealUsedTO.Qty.ToString().Trim());
                            item.SubItems.Add(mealUsedTO.RestaurantName.Trim());
                            item.SubItems.Add(mealUsedTO.PointName.Trim());
                            if (mealUsedTO.ManualCreated == 1)
                                item.SubItems.Add(rm.GetString("lblManual", culture));
                            else
                                item.SubItems.Add(rm.GetString("chbCardReader", culture));

                            if (mealUsedTO.OnlineValidation != -1)
                            {
                                if (mealUsedTO.OnlineValidation == 1)
                                    item.SubItems.Add(rm.GetString("chbGreen", culture));
                                else
                                    item.SubItems.Add(rm.GetString("chbRed", culture));
                            }
                            else
                                item.SubItems.Add("");
                            item.SubItems.Add(mealUsedTO.ReasonRefused);
                            if (mealUsedTO.AutoCheck != -1)
                            {
                                if (mealUsedTO.AutoCheck == 1)
                                    item.SubItems.Add(rm.GetString("chbOK", culture));
                                else
                                    item.SubItems.Add(rm.GetString("chbNOK", culture));
                            }
                            else
                                item.SubItems.Add("");
                            item.SubItems.Add(mealUsedTO.AutoCheckFailureReason);
                            item.SubItems.Add(mealUsedTO.Approved);
                            item.SubItems.Add(mealUsedTO.ApprovedBy);
                            if (mealUsedTO.ApprovedTime != new DateTime())
                                item.SubItems.Add(mealUsedTO.ApprovedTime.ToString("dd.MM.yyyy"));
                            else
                                item.SubItems.Add("");

                            item.Tag = mealUsedTO;
                            lvMealsUsed.Items.Add(item);

                            qty += mealUsedTO.Qty;
                        }

                        //lvEmployees.Items.AddRange(lvItems);
                    }
                }

                lvMealsUsed.EndUpdate();
                lvMealsUsed.Invalidate();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateListView(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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
                populateListView(listMealsUsed, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.btnPrev_Click(): " + ex.Message + "\n");
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
                populateListView(listMealsUsed, startIndex);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.btnNext_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                OpenFileDialog fbDialog = new OpenFileDialog();
                fbDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                fbDialog.Filter = "Text files (*.txt)|*.txt";
                fbDialog.Title = "Open file";

                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;

                    string path = fbDialog.FileName;
                    tbPath.Text = fbDialog.FileName;

                    List<OnlineMealsUsedTO> mealList = new List<OnlineMealsUsedTO>();
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.FileStream str = System.IO.File.Open(path, System.IO.FileMode.Open);
                        System.IO.StreamReader reader = new System.IO.StreamReader(str);

                        // read file lines, skip header
                        string line = reader.ReadLine(); // header
                        line = reader.ReadLine(); // first row
                        string mealIDs = "";
                        while (line != null)
                        {
                            if (!line.Trim().Equals(""))
                            {
                                mealIDs += line.Trim() + ",";
                            }

                            line = reader.ReadLine();
                        }

                        reader.Close();
                        str.Dispose();
                        str.Close();

                        if (mealIDs.Length > 0)
                        {
                            mealIDs = mealIDs.Substring(0, mealIDs.Length - 1);
                            mealList = new OnlineMealsUsed().Search(mealIDs);
                        }

                        populateListView(mealList, 0);
                    }
                    else
                        return;
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.btnBrowse_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (emplDateChanged)
                    populateEmployees();
                else
                    emplDateChanged = true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.dtpFrom_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (emplDateChanged)
                    populateEmployees();
                else
                    emplDateChanged = true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.dtpTo_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateEmployees()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<EmployeeTO> employeeList = new List<EmployeeTO>();

                bool isWU = rbWU.Checked;

                int ID = -1;
                if (isWU && cbWU.SelectedIndex > 0)
                    ID = (int)cbWU.SelectedValue;
                else if (!isWU && cbOU.SelectedIndex > 0)
                    ID = (int)cbOU.SelectedValue;
                
                DateTime from = dtpFrom.Value.Date;
                DateTime to = dtpTo.Value.Date;

                if (from > to)
                {
                    MessageBox.Show(rm.GetString("dateFromBigerThanTO", culture));
                    return;
                }

                if (ID != -1)
                {
                    if (isWU)
                    {
                        string wunits = Common.Misc.getWorkingUnitHierarhicly(ID, wuList, null);                        

                        // get employees from selected working unit that are not currently loaned to other working unit or are currently loand to selected working unit                        
                        employeeList = new Employee().SearchByWULoans(wunits, -1, null, from.Date, to.Date);
                    }
                    else
                    {
                        string ounits = Common.Misc.getOrgUnitHierarhicly(ID.ToString(), ouList, null);
                        
                        employeeList = new Employee().SearchByOU(ounits, -1, null, from.Date, to.Date);
                    }
                }

                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();

                foreach (EmployeeTO empl in employeeList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = empl.FirstName.Trim();                    
                    item.SubItems.Add(empl.LastName);                    
                    item.ToolTipText = empl.EmployeeID.ToString().Trim();

                    item.Tag = empl;
                    lvEmployees.Items.Add(item);
                }

                lvEmployees.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
