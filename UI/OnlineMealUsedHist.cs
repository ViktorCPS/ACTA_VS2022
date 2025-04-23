using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;
using Common;
using TransferObjects;
using System.Resources;
using System.Globalization;
using System.Collections;

namespace UI
{
    public partial class OnlineMealUsedHist : Form
    {
        DebugLog log;

        ApplUserTO logInUser;

        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private string wuString = "";
        private List<int> wuList = new List<int>();

        private Dictionary<int, OrganizationalUnitTO> oUnits = new Dictionary<int, OrganizationalUnitTO>();
        private string ouString = "";
        private List<int> ouList = new List<int>();

        private CultureInfo culture;
        private ResourceManager rm;
        Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
        Dictionary<int, EmployeeAsco4TO> ascoDict = new Dictionary<int, EmployeeAsco4TO>();
        Dictionary<int, WorkingUnitTO> wuDict = new Dictionary<int, WorkingUnitTO>();
        List<OnlineMealsUsedHistTO> listMealsUsed = new List<OnlineMealsUsedHistTO>();

        OnlineMealsUsedTO currentMealUsedTO = new OnlineMealsUsedTO();

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
        const int ModifiedBy = 24;
        const int ModifiedTime = 25;

        private ListViewItemComparer _comp;


        public OnlineMealUsedHist()
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(OnlineMealsUsedAdd).Assembly);

            logInUser = NotificationController.GetLogInUser();

            emplDict = new Employee().SearchDictionary();
            ascoDict = new EmployeeAsco4().SearchDictionary("");
            wuDict = new WorkingUnit().getWUDictionary();

            setLanguage();
        }

        public OnlineMealUsedHist(OnlineMealsUsedTO mealUsedCurrent)
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(OnlineMealsUsedAdd).Assembly);

            logInUser = NotificationController.GetLogInUser();
            setLanguage();

            emplDict = new Employee().SearchDictionary();
            ascoDict = new EmployeeAsco4().SearchDictionary("");
            wuDict = new WorkingUnit().getWUDictionary();

            currentMealUsedTO = mealUsedCurrent;
            List<OnlineMealsUsedTO> listCurrent = new List<OnlineMealsUsedTO>();
            listCurrent.Add(currentMealUsedTO);
            populateListViewMealUsedCurrent(listCurrent);

            List<OnlineMealsUsedHistTO> listHist = new OnlineMealsUsedHist().SearchMealUsed(currentMealUsedTO.TransactionID.ToString());
            populateListViewEmpMealUsed(listHist);

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
                    case OnlineMealUsedHist.Qty:
                    case OnlineMealUsedHist.EmployeeIndex:
                    case OnlineMealUsedHist.Week:
                    case OnlineMealUsedHist.Month:
                    case OnlineMealUsedHist.Day:
                    case OnlineMealUsedHist.Year:
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
                    case OnlineMealUsedHist.EmployeeName:
                    case OnlineMealUsedHist.EmployeeLast:
                    case OnlineMealUsedHist.Stringone:
                    case OnlineMealUsedHist.CostCenter:
                    case OnlineMealUsedHist.CostCenterDesc:
                    case OnlineMealUsedHist.MealType:
                    case OnlineMealUsedHist.Restaurant:
                    case OnlineMealUsedHist.Line:
                    case OnlineMealUsedHist.Insert:
                    case OnlineMealUsedHist.ValidationReader:
                    case OnlineMealUsedHist.ReasonReader:
                    case OnlineMealUsedHist.ValidationAuto:
                    case OnlineMealUsedHist.AutoReason:
                    case OnlineMealUsedHist.Approve:
                    case OnlineMealUsedHist.ApprovedBy:
                    case OnlineMealUsedHist.ModifiedBy:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    case OnlineMealUsedHist.ApprovedTime:
                    case OnlineMealUsedHist.ModifiedTime:
                        {
                            if (!item1.SubItems[OnlineMealUsedHist.ApprovedTime].Text.Trim().Equals("") && !item2.SubItems[OnlineMealUsedHist.ApprovedTime].Text.Trim().Equals(""))
                            {
                                DateTime date1 = DateTime.Parse(item1.SubItems[OnlineMealUsedHist.ApprovedTime].Text.Trim(), culture);
                                DateTime date2 = DateTime.Parse(item2.SubItems[OnlineMealUsedHist.ApprovedTime].Text.Trim(), culture);
                                return CaseInsensitiveComparer.Default.Compare(date1, date2);
                            }


                            else if (!item1.SubItems[OnlineMealUsedHist.ModifiedTime].Text.Trim().Equals("") && !item2.SubItems[OnlineMealUsedHist.ModifiedTime].Text.Trim().Equals(""))
                            {
                                DateTime date1 = DateTime.Parse(item1.SubItems[OnlineMealUsedHist.ModifiedTime].Text.Trim(), culture);
                                DateTime date2 = DateTime.Parse(item2.SubItems[OnlineMealUsedHist.ModifiedTime].Text.Trim(), culture);
                                return CaseInsensitiveComparer.Default.Compare(date1, date2);
                            }

                            else
                                return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    case OnlineMealUsedHist.Time:
                        {
                            DateTime time1 = new DateTime();
                            DateTime time2 = new DateTime();

                            string t1 = sub1.Text;
                            string t2 = sub2.Text;
                           
                            time1 = new DateTime(1, 1, 1, int.Parse(t1.Substring(0, 2)),int.Parse(t1.Substring(3, 2)),int.Parse(t1.Substring(6, 2)));
                            time2 = new DateTime(1, 1, 1, int.Parse(t2.Substring(0, 2)), int.Parse(t2.Substring(3, 2)), int.Parse(t2.Substring(6, 2)));
                            return CaseInsensitiveComparer.Default.Compare(time1, time2);

                        }
                    default:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                }
            }
        }

        #endregion


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
                log.writeLog(DateTime.Now + " OnlineMealsUsed.lvMealsUsed_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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

        private void setLanguage()
        {
            try
            {
                lblLine.Text = rm.GetString("lblLine", culture);
                lblMealType.Text = rm.GetString("lblTypeMeal", culture);
                lblRestaurant.Text = rm.GetString("lblRestaurant", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblTotal.Text = rm.GetString("lblTotalChange", culture) + " 0";
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblModifiedFrom.Text = rm.GetString("lblFrom", culture);
                lblModifiedTo.Text = rm.GetString("lblTo", culture);
                lblReason.Text = rm.GetString("lblReason", culture);
                lblStatus.Text = rm.GetString("lblStatus", culture);
                lblToApproval.Text = rm.GetString("lblTo", culture);
                lblReasonAuto.Text = rm.GetString("lblReason", culture);
                lblFromApproval.Text = rm.GetString("lblFrom", culture);
                lblQty.Text = rm.GetString("lblQty", culture);

                this.Text = rm.GetString("hdrHistoryOfChange", culture);
                gbUnitFilter.Text = rm.GetString("gbUnitFilter", culture);
                gbValidation.Text = rm.GetString("gbValidationReader", culture);
                gbValidationAuto.Text = rm.GetString("gbValidationAuto", culture);
                gbApproval.Text = rm.GetString("gbApproval", culture);
                gbEnterWay.Text = rm.GetString("gbInput", culture);
                gbHist.Text = rm.GetString("gbHistoryOfChange", culture);
                gbEventTime.Text = rm.GetString("gbEventTime", culture);
                gbModifiedTime.Text = rm.GetString("gbModifiedTime", culture);
                gbHistoryOfChange.Text = rm.GetString("hdrHistoryOfChange", culture);
                gbCurrentMeal.Text = rm.GetString("gbCurrentMealUsed", culture);

                //RB text
                rbOU.Text = rm.GetString("gbOrganizationalUnits", culture);
                rbWU.Text = rm.GetString("WUForm", culture);

                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);

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
                lvMealsUsed.Columns.Add(rm.GetString("hdrModifiedBy", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrModifiedTime", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.EndUpdate();

                lvCurrentMeal.BeginUpdate();
                lvCurrentMeal.Columns.Add(rm.GetString("hdrEmployeeID", culture), 70, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrStringone", culture), 100, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrFirstName", culture), 100, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrLastName", culture), 100, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrCostCenter", culture), 100, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrCostCenterDesc", culture), 100, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrBranch", culture), 50, HorizontalAlignment.Right);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrWeek", culture), 50, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrYear", culture), 50, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrMonth", culture), 50, HorizontalAlignment.Right);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrDay", culture), 50, HorizontalAlignment.Right);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrTime", culture), 50, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrMealType", culture), 100, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrQty", culture), 50, HorizontalAlignment.Right);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrRestaurant", culture), 100, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrLine", culture), 100, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrManual", culture), 100, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrValidationReader", culture), 100, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrReason", culture), 100, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrValidationAuto", culture), 50, HorizontalAlignment.Right);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrReason", culture), 100, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrApproved", culture), 100, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrApprovedBy", culture), 100, HorizontalAlignment.Left);
                lvCurrentMeal.Columns.Add(rm.GetString("hdrTime", culture), 100, HorizontalAlignment.Left);
                lvCurrentMeal.EndUpdate();

                lvReason.BeginUpdate();
                lvReason.Columns.Add("", lvReason.Width - 7, HorizontalAlignment.Left);
                lvReason.EndUpdate();

                lvStatus.BeginUpdate();
                lvStatus.Columns.Add("", lvStatus.Width - 7, HorizontalAlignment.Left);
                lvStatus.EndUpdate();

                lvOperater.BeginUpdate();
                lvOperater.Columns.Add("", lvOperater.Width - 25, HorizontalAlignment.Left);
                lvOperater.EndUpdate();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateWorkingUnitCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        private void rbWU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cbWU.Enabled = btnWUTree.Enabled = rbWU.Checked;

                cbOU.Enabled = btnOUTree.Enabled = !rbWU.Checked;

                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.rbWU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbOU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cbWU.Enabled = btnWUTree.Enabled = !rbOU.Checked;

                cbOU.Enabled = btnOUTree.Enabled = rbOU.Checked;

                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.rbOU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbWU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void cbOU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.cbOU_SelectedIndexChanged(): " + ex.Message + "\n");
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
                    this.cbWU.SelectedIndex = cbWU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedAdd.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " OnlineMealsUsedAdd.btnOUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void OnlineMealUsedHist_Load(object sender, EventArgs e)
        {
            tbTimeFrom.Text = "00:00";
            tbTimeTo.Text = "23:59";

            _comp = new ListViewItemComparer(lvMealsUsed);
            lvMealsUsed.ListViewItemSorter = _comp;
            lvMealsUsed.Sorting = SortOrder.Ascending;

            lvStatus.Enabled = false;
            lvOperater.Enabled = false;
            dtpFromApproval.Enabled = false;
            dtpToApproval.Enabled = false;
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

            populateWU();
            populateOU();
            populateMealTypes();
            populateRestaurant();
            populateMealPoints();
            populateListViewReasons();
            populateListViewStatus();

            List<ApplUserTO> listUsers = new ApplUser().SearchForCategory((int)Constants.Categories.HRSSC);
            populateListViewUsers(listUsers);

        }

        private void populateMealTypes()
        {
            try
            {
                List<OnlineMealsTypesTO> types = new OnlineMealsTypes().Search();

                OnlineMealsTypesTO rest = new OnlineMealsTypesTO();
                rest.Name = rm.GetString("all", culture);
                types.Insert(0, rest);

                cbMealType.DataSource = types;
                cbMealType.DisplayMember = "Name";
                cbMealType.ValueMember = "MealTypeID";

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.populateMealTypes(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.populateWU(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.populateOU(): " + ex.Message + "\n");
                throw ex;
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

                //int onlyEmplTypeID = -1;
                //int exceptEmplTypeID = -1;

                if (ID != -1)
                {
                    if (isWU)
                    {
                        string wunits = "";
                        wunits = Common.Misc.getWorkingUnitHierarhicly(ID, wuList, null);

                        // get employees from selected working unit that are not currently loaned to other working unit or are currently loand to selected working unit                        
                        employeeList = new Employee().SearchByWULoans(wunits, -1, null, DateTime.Now.Date, DateTime.Now.Date);
                    }
                    else
                    {
                        string ounits = "";

                        ounits = Common.Misc.getOrgUnitHierarhicly(ID.ToString(), ouList, null);


                        employeeList = new Employee().SearchByOU(ounits, -1, null, DateTime.Now.Date, DateTime.Now.Date);
                    }
                }


                foreach (EmployeeTO employee in employeeList)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl = new EmployeeTO();
                empl.LastName = rm.GetString("all", culture);
                employeeList.Insert(0, empl);

                cbEmployee.DataSource = employeeList;
                cbEmployee.DisplayMember = "LastName";
                cbEmployee.ValueMember = "EmployeeID";
                cbEmployee.Invalidate();
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

        private void populateRestaurant()
        {
            try
            {
                List<OnlineMealsRestaurantTO> resList = new OnlineMealsRestaurant().Search();
                OnlineMealsRestaurantTO rest = new OnlineMealsRestaurantTO();
                rest.Name = rm.GetString("all", culture);
                resList.Insert(0, rest);

                cbRestaurant.DataSource = resList;
                cbRestaurant.DisplayMember = "Name";
                cbRestaurant.ValueMember = "RestaurantID";



            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.populateRestaurant(): " + ex.Message + "\n");
                throw ex;
            }
        }
        private void populateMealPoints()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbRestaurant.SelectedIndex >= 0 && cbRestaurant.SelectedValue != null && cbRestaurant.SelectedValue is int)
                {
                    List<OnlineMealsPointsTO> points = new OnlineMealsPoints().SearchByRestaurant(((int)cbRestaurant.SelectedValue).ToString().Trim());

                    OnlineMealsPointsTO rest = new OnlineMealsPointsTO();
                    rest.Name = rm.GetString("all", culture);
                    points.Insert(0, rest);

                    cbLine.DataSource = points;
                    cbLine.DisplayMember = "Name";
                    cbLine.ValueMember = "PointID";




                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.populateRestaurant(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void cbRestaurant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                populateMealPoints();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedAdd.cbRestaurant_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }


        private void cbEventTime_Changed(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbEventTime.Checked || cbModifiedTime.Checked)
                {
                    if (cbEventTime.Checked)
                    {
                        gbEventTime.Enabled = true;
                    }
                    else
                    {
                        gbEventTime.Enabled = false;
                    }
                }
                else
                {
                    cbEventTime.Checked = true;
                    gbEventTime.Enabled = true;
                    MessageBox.Show(rm.GetString("noDateTimePickerSelected", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesHist.cbEventTime_Changed(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbModifiedTime_Changed(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbModifiedTime.Checked || cbEventTime.Checked)
                {
                    if (cbModifiedTime.Checked)
                    {
                        gbModifiedTime.Enabled = true;
                    }
                    else
                    {
                        gbModifiedTime.Enabled = false;
                    }
                }
                else
                {
                    cbModifiedTime.Checked = true;
                    gbModifiedTime.Enabled = true;
                    MessageBox.Show(rm.GetString("noDateTimePickerSelected", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassesHist.cbModifiedTime_Changed(): " + ex.Message + "\n");
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
                this.Cursor = Cursors.WaitCursor;
                if (cbEmployee.Items.Count <= 1)
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

                int qtyFrom = -1;
                int qtyTo = -1;

                if ((numQtyFrom.Value != 0 || numQtyTo.Value != 0) && numQtyFrom.Value > numQtyTo.Value)
                {
                    MessageBox.Show(rm.GetString("QtyFromGreater", culture));
                    return;
                }

                qtyFrom = (numQtyFrom.Value > 0 ? (int)numQtyFrom.Value : -1);
                qtyTo = (numQtyTo.Value > 0 ? (int)numQtyTo.Value : -1);


                DateTime modifiedFrom = new DateTime();
                DateTime modifiedTO = new DateTime();

                DateTime mealFrom = new DateTime();
                DateTime mealTO = new DateTime();

                DateTime mealFromApproved = new DateTime();
                DateTime mealTOApproved = new DateTime();

                if (cbModifiedTime.Checked)
                {
                    modifiedFrom = dtpModifiedFrom.Value;
                    modifiedTO = dtpModifiedTo.Value;
                }
                if (cbEventTime.Checked)
                {

                    string hourFrom = "";
                    string hourTo = "";
                    string minFrom = "";
                    string minTo = "";

                    string timeFrom = tbTimeFrom.Text;
                    string timeTo = tbTimeTo.Text;

                    if (timeFrom.Trim() == "" || timeTo.Trim() == "" || !timeFrom.Contains(":") || !timeTo.Contains(":"))
                    {
                        MessageBox.Show(rm.GetString("invalidTimeFormat", culture));
                        return;
                    }

                    hourFrom = timeFrom.Remove(timeFrom.IndexOf(':'));
                    hourTo = timeTo.Remove(timeTo.IndexOf(':'));

                    minFrom = timeFrom.Substring(timeFrom.IndexOf(':') + 1);
                    minTo = timeTo.Substring(timeTo.IndexOf(':') + 1);

                    int hourF = -1;
                    if (!int.TryParse(hourFrom, out hourF))
                        hourF = -1;

                    int hourT = -1;
                    if (!int.TryParse(hourTo, out hourT))
                        hourT = -1;

                    int minT = -1;
                    if (!int.TryParse(minTo, out minT))
                        minT = -1;

                    int minF = -1;
                    if (!int.TryParse(minFrom, out minF))
                        minF = -1;

                    if (hourF == -1 || hourT == -1 || minF == -1 || minT == -1 || hourF < 0 || hourF > 23 || hourT < 0 || hourT > 23 || minF < 0 || minF > 59 || minT < 0 || minT > 59)
                    {
                        MessageBox.Show(rm.GetString("invalidTimeFormat", culture));
                        return;
                    }
                    mealTO = dtpTo.Value;
                    mealFrom = dtpFrom.Value;
                    mealFrom = new DateTime(mealFrom.Year, mealFrom.Month, mealFrom.Day, hourF, minF, 0);
                    mealTO = new DateTime(mealTO.Year, mealTO.Month, mealTO.Day, hourT, minT, 0);

                    if (mealFrom > mealTO)
                    {
                        MessageBox.Show(rm.GetString("dateFromBigerThanTO", culture));
                        return;
                    }
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

                if (cbRestaurant.SelectedIndex > 0)
                {
                    restaurants = cbRestaurant.SelectedValue.ToString();
                }
                if (cbLine.SelectedIndex > 0)
                {
                    points = cbLine.SelectedValue.ToString();
                }
                if (cbMealType.SelectedIndex > 0)
                {
                    mealtypes = cbMealType.SelectedValue.ToString();
                }


                if (cbEmployee.SelectedIndex > 0)
                {

                    emplIDs = cbEmployee.SelectedValue.ToString();

                }
                else
                {
                    foreach (object item in cbEmployee.Items)
                    {
                        emplIDs += ((EmployeeTO)item).EmployeeID + ",";

                    }
                    if (emplIDs.Length > 0)
                    {
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                    }
                }
                if (cbRed.Checked && !cbGreen.Checked)
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

                listMealsUsed = new OnlineMealsUsedHist().SearchMealUsed(emplIDs, mealFrom, mealTO, restaurants, points, mealtypes, qtyFrom, qtyTo, manualCreated, onlineValidation, autoCheck, reasonsReader, reasonAutoCheck, status, operater, mealFromApproved, mealTOApproved, modifiedFrom, modifiedTO);

                populateListViewEmpMealUsed(listMealsUsed);
                if (listMealsUsed.Count <= 0)
                {

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

        public void populateListViewEmpMealUsed(List<OnlineMealsUsedHistTO> listMealsUsed)
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
                        OnlineMealsUsedHistTO mealUsedTO = (OnlineMealsUsedHistTO)listMealsUsed[i];
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
                        item.SubItems.Add(mealUsedTO.ModifiedBy);
                        item.SubItems.Add(mealUsedTO.ModifiedTime.ToString("dd.MM.yyyy"));

                        item.Tag = mealUsedTO;
                        lvMealsUsed.Items.Add(item);

                        qty += mealUsedTO.Qty;
                    }
                }

                lvMealsUsed.EndUpdate();
                lvMealsUsed.Invalidate();

                lblTotal.Text = rm.GetString("lblTotalChange", culture) + " " + listMealsUsed.Count.ToString().Trim();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateListViewEmployees(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void lvMealsUsed_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lvMealsUsed.SelectedIndices.Count > 0)
                {
                    OnlineMealsUsedHistTO mealsHist = (OnlineMealsUsedHistTO)lvMealsUsed.SelectedItems[0].Tag;

                    OnlineMealsUsedTO mealUsed = new OnlineMealsUsed().Find(mealsHist.TransactionID.ToString());

                    List<OnlineMealsUsedTO> mealUsedList = new List<OnlineMealsUsedTO>();
                    mealUsedList.Add(mealUsed);
                    populateListViewMealUsedCurrent(mealUsedList);

                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.lvMealsUsed_SelectedIndexChanged(): " + ex.Message + "\n");
                throw ex;
            }

        }
        public void populateListViewMealUsedCurrent(List<OnlineMealsUsedTO> listMealsUsed)
        {
            try
            {

                lvCurrentMeal.BeginUpdate();
                lvCurrentMeal.Items.Clear();

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
                        lvCurrentMeal.Items.Add(item);

                        qty += mealUsedTO.Qty;
                    }
                }

                lvCurrentMeal.EndUpdate();
                lvCurrentMeal.Invalidate();


            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsed.populateListViewEmployees(): " + ex.Message + "\n");
                throw ex;
            }
        }


    }
}
