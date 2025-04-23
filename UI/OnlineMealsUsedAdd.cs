using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.Data.OleDb;

using Common;
using TransferObjects;
using Util;

namespace UI
{
    public partial class OnlineMealsUsedAdd : Form
    {
        private const int emplIDIndex = 0;
        private const int emplStringoneIndex = 1;
        private const int emplNameIndex = 2;
        private const int dateIndex = 3;
        private const int typeIndex = 4;
        private const int pointIndex = 5;
        private const int qtyIndex = 6;

        private const int xlsIndex = 1;
        private const int xlsxIndex = 2;

        private const char delimiter = '\t';

        private const int massiveUploadColumns = 7;
                
        private int emplSortField;
        private int mealSortOrder;
        private int mealSortField;

        private ListViewItemComparer _comp;

        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private string wuString = "";
        private List<int> wuList = new List<int>();

        private Dictionary<int, OrganizationalUnitTO> oUnits = new Dictionary<int, OrganizationalUnitTO>();
        private string ouString = "";
        private List<int> ouList = new List<int>();

        Dictionary<int, EmployeeAsco4TO> ascoDict = new Dictionary<int, EmployeeAsco4TO>();        
        Dictionary<int, OnlineMealsTypesTO> mealsTypesDict = new Dictionary<int, OnlineMealsTypesTO>();
        Dictionary<int, OnlineMealsPointsTO> mealsPointsDict = new Dictionary<int, OnlineMealsPointsTO>();
        Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
        Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rules = new Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>>();
        Dictionary<int, WorkingUnitTO> wuDict = new Dictionary<int, WorkingUnitTO>();

        List<OnlineMealsUsedTO> mealsList = new List<OnlineMealsUsedTO>();
        Dictionary<int, List<DateTime>> emplMealTimeDict = new Dictionary<int, List<DateTime>>();
                        
        public OnlineMealsUsedAdd()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(OnlineMealsUsedAdd).Assembly);

                logInUser = NotificationController.GetLogInUser();

                setLanguage();

                rbWU.Checked = true;
                chbHierarhiclyWU.Checked = true;
                chbHierachyOU.Checked = true;
                chbHierachyOU.Visible = chbHierarhiclyWU.Visible = false;

                dtpTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Inner Class for sorting Array List of Employees Absences
        /*
		 *  Class used for sorting items in the List View 
		*/
        private class ListViewItemComparer : IComparer
        {
            private ListView _listView;

            public ListViewItemComparer(ListView lv)
            {
                _listView = lv;
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
                    case OnlineMealsUsedAdd.emplIDIndex:
                        {
                            int id1 = -1;
                            int id2 = -1;

                            int.TryParse(sub1.Text, out id1);
                            int.TryParse(sub2.Text, out id2);

                            return CaseInsensitiveComparer.Default.Compare(id1, id2);
                        }
                    case OnlineMealsUsedAdd.emplStringoneIndex:
                    case OnlineMealsUsedAdd.emplNameIndex:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                    default:
                        return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        
                }
            }
        }

        /*
		 *  Class used for sorting List of Online Meals Used
		*/

        private class ListSort : IComparer<OnlineMealsUsedTO>
        {
            private int compOrder;
            private int compField;
            public ListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(OnlineMealsUsedTO x, OnlineMealsUsedTO y)
            {
                OnlineMealsUsedTO meal1 = null;
                OnlineMealsUsedTO meal2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    meal1 = x;
                    meal2 = y;
                }
                else
                {
                    meal1 = y;
                    meal2 = x;
                }

                switch (compField)
                {
                    case OnlineMealsUsedAdd.emplIDIndex:
                        return meal1.EmployeeID.CompareTo(meal2.EmployeeID);
                    case OnlineMealsUsedAdd.emplStringoneIndex:
                        return meal1.EmployeeStringone.CompareTo(meal2.EmployeeStringone);
                    case OnlineMealsUsedAdd.emplNameIndex:
                        return meal1.EmployeeName.CompareTo(meal2.EmployeeName);
                    case OnlineMealsUsedAdd.dateIndex:
                        return meal1.EventTime.CompareTo(meal2.EventTime);
                    case OnlineMealsUsedAdd.typeIndex:
                        return meal1.MealType.CompareTo(meal2.MealType);
                    case OnlineMealsUsedAdd.pointIndex:
                        return meal1.PointName.CompareTo(meal2.PointName);
                    case OnlineMealsUsedAdd.qtyIndex:
                        return meal1.Qty.CompareTo(meal2.Qty);
                    default:
                        return meal1.EmployeeName.CompareTo(meal2.EmployeeName);
                }
            }
        }

        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("OnlineMealsUsedAdd", culture);
                
                //label's text
                this.lblPath.Text = rm.GetString("lblPath", culture);
                this.lblRestaurant.Text = rm.GetString("lblRestaurant", culture);
                this.lblPoint.Text = rm.GetString("lblPoint", culture);

                //button's text
                this.btnCancel.Text = rm.GetString("btnCancel", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);
                this.btnBrowse.Text = rm.GetString("btnBrowse", culture);                
                this.btnRemove.Text = rm.GetString("btnRemoveSel", culture);

                //group box text
                this.gbDates.Text = rm.GetString("gbDates", culture);
                this.gbMassiveInput.Text = rm.GetString("gbMassiveInput", culture);
                this.gbMealType.Text = rm.GetString("gbMealType", culture);
                this.gbQty.Text = rm.GetString("gbQty", culture);
                this.gbUnitFilter.Text = rm.GetString("gbUnitFilter", culture);
                this.gbMealPoint.Text = rm.GetString("gbMealPoint", culture);
                this.gbTime.Text = rm.GetString("gbTime", culture);

                // radio button text
                //this.rbWU.Text = rm.GetString("rbWU", culture);
                //this.rbOU.Text = rm.GetString("rbOU", culture);

                // check box text
                this.chbHierarhiclyWU.Text = rm.GetString("chbHierarhicly", culture);
                this.chbHierachyOU.Text = rm.GetString("chbHierarhicly", culture);
               
                // list view
                lvDates.BeginUpdate();
                lvDates.Columns.Add("", lvDates.Width - 22, HorizontalAlignment.Left);
                lvDates.EndUpdate();

                lvEmployees.BeginUpdate();
                lvEmployees.Columns.Add(rm.GetString("hdrID", culture), 50, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrStringone", culture), 100, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrName", culture), 120, HorizontalAlignment.Left);
                lvEmployees.Columns.Add(rm.GetString("hdrLocation", culture), 50, HorizontalAlignment.Left);
                lvEmployees.EndUpdate();

                lvMealsUsed.BeginUpdate();
                lvMealsUsed.Columns.Add(rm.GetString("hdrID", culture), 50, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrStringone", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrName", culture), 110, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrLocation", culture), 50, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrDate", culture), 100, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrMealType", culture), 80, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrMealPoint", culture), 70, HorizontalAlignment.Left);
                lvMealsUsed.Columns.Add(rm.GetString("hdrQty", culture), 40, HorizontalAlignment.Left);
                lvMealsUsed.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void OnlineMealsUsedAdd_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                mealSortOrder = Constants.sortAsc;
                emplSortField = mealSortField = emplNameIndex;

                // Initialize comparer objects
                _comp = new ListViewItemComparer(lvEmployees);
                _comp.SortColumn = emplSortField;
                lvEmployees.ListViewItemSorter = _comp;
                lvEmployees.Sorting = SortOrder.Ascending;

                emplDict = new Employee().SearchDictionary();
                ascoDict = new EmployeeAsco4().SearchDictionary("");
                rules = new Common.Rule().SearchWUEmplTypeDictionary();
                wuDict = new WorkingUnit().getWUDictionary();
                List<OnlineMealsPointsTO> pointList = new OnlineMealsPoints().Search();
                List<OnlineMealsTypesTO> typeList = new OnlineMealsTypes().Search();

                foreach (OnlineMealsTypesTO type in typeList)
                {
                    if (!mealsTypesDict.ContainsKey(type.MealTypeID))
                        mealsTypesDict.Add(type.MealTypeID, type);
                }

                foreach (OnlineMealsPointsTO point in pointList)
                {
                    if (!mealsPointsDict.ContainsKey(point.PointID))
                        mealsPointsDict.Add(point.PointID, point);
                }

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
                populateDates();
                populateMealTypes();
                populateRestaurant();
                populateMealPoints();

                rbWU.Checked = true;
                rbWU_CheckedChanged(this, new EventArgs());
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.OnlineMealsUsedAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
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

                DateTime currMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime from = currMonth.AddMonths(-1);
                DateTime to = currMonth.AddMonths(2).AddDays(-1);
                
                if (ID != -1)
                {
                    if (isWU)
                    {
                        string wunits = "";
                        if (chbHierarhiclyWU.Checked)
                            wunits = Common.Misc.getWorkingUnitHierarhicly(ID, wuList, null);
                        else
                            wunits = ID.ToString().Trim();

                        // get employees from selected working unit that are not currently loaned to other working unit or are currently loand to selected working unit                        
                        employeeList = new Employee().SearchByWULoans(wunits, -1, null, from.Date, to.Date);
                    }
                    else
                    {
                        string ounits = "";
                        if (chbHierachyOU.Checked)
                            ounits = Common.Misc.getOrgUnitHierarhicly(ID.ToString(), ouList, null);
                        else
                            ounits = ID.ToString().Trim();

                        employeeList = new Employee().SearchByOU(ounits, -1, null, from.Date, to.Date);                        
                    }
                }

                lvEmployees.BeginUpdate();
                lvEmployees.Items.Clear();

                foreach (EmployeeTO empl in employeeList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = empl.EmployeeID.ToString().Trim();
                    if (ascoDict.ContainsKey(empl.EmployeeID))
                        item.SubItems.Add(ascoDict[empl.EmployeeID].NVarcharValue2.Trim());
                    else
                        item.SubItems.Add("");
                    item.SubItems.Add(empl.FirstAndLastName);
                    if (ascoDict.ContainsKey(empl.EmployeeID))
                        item.SubItems.Add(ascoDict[empl.EmployeeID].NVarcharValue7.Trim());
                    else
                        item.SubItems.Add("");
                    item.ToolTipText = empl.FirstAndLastName;

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

        private void populateMealUsed()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                mealsList.Sort(new ListSort(mealSortOrder, mealSortField));

                lvMealsUsed.BeginUpdate();
                lvMealsUsed.Items.Clear();

                foreach (OnlineMealsUsedTO meal in mealsList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = meal.EmployeeID.ToString().Trim();
                    item.SubItems.Add(meal.EmployeeStringone.Trim());
                    item.SubItems.Add(meal.EmployeeName.Trim());
                    item.SubItems.Add(meal.EmployeeLocation.Trim());
                    item.SubItems.Add(meal.EventTime.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    item.SubItems.Add(meal.MealType.Trim());
                    item.SubItems.Add(meal.PointName.Trim());
                    item.SubItems.Add(meal.Qty.ToString().Trim());
                    item.ToolTipText = meal.EmployeeName;

                    item.Tag = meal;
                    lvMealsUsed.Items.Add(item);
                }

                lvMealsUsed.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.populateMealUsed(): " + ex.Message + "\n");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateMealTypes()
        {
            try
            {
                List<OnlineMealsTypesTO> types = new OnlineMealsTypes().Search();

                cbMealTypes.DataSource = types;
                cbMealTypes.DisplayMember = "Name";
                cbMealTypes.ValueMember = "MealTypeID";

                cbMealTypes.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.populateMealTypes(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateRestaurant()
        {
            try
            {
                List<OnlineMealsRestaurantTO> resList = new OnlineMealsRestaurant().Search();

                cbRestaurant.DataSource = resList;
                cbRestaurant.DisplayMember = "Name";
                cbRestaurant.ValueMember = "RestaurantID";

                cbRestaurant.SelectedIndex = 0;
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

                    cbPoint.DataSource = points;
                    cbPoint.DisplayMember = "Name";
                    cbPoint.ValueMember = "PointID";

                    cbPoint.SelectedIndex = 0;
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

        private void populateDates()
        {
            try
            {
                lvDates.BeginUpdate();
                lvDates.Items.Clear();

                DateTime currMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime start = currMonth.AddMonths(-1);
                DateTime end = currMonth.AddMonths(2).AddDays(-1);

                DateTime currDate = start.Date;
                int currMonthIndex = 0;
                while (currDate.Date <= end.Date)
                {
                    if (currDate.Date < currMonth.Date)
                        currMonthIndex++;

                    ListViewItem item = new ListViewItem();
                    item.Text = currDate.Date.ToString(Constants.dateFormat);
                    item.Tag = currDate.Date;
                    lvDates.Items.Add(item);

                    currDate = currDate.AddDays(1);
                }

                lvDates.EndUpdate();                
                lvDates.EnsureVisible(currMonthIndex);
                lvDates.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.populateEmployees(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void rbWU_CheckedChanged(object sender, EventArgs e)
        {
            try            
            {
                cbWU.Enabled = chbHierarhiclyWU.Enabled = btnWUTree.Enabled = rbWU.Checked;

                cbOU.Enabled = chbHierachyOU.Enabled = btnOUTree.Enabled = !rbWU.Checked;

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
                cbWU.Enabled = chbHierarhiclyWU.Enabled = btnWUTree.Enabled = !rbOU.Checked;

                cbOU.Enabled = chbHierachyOU.Enabled = btnOUTree.Enabled = rbOU.Checked;

                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.rbOU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbHierarhiclyWU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.chbHierarhiclyWU_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbHierachyOU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                populateEmployees();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsedAdd.chbHierachyOU_CheckedChanged(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " OnlineMealsUsedAdd.tbEmployee_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvEmployees.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("selectEmployees", culture));
                    return;
                }

                if (lvDates.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("selectDates", culture));
                    return;
                }

                if (cbMealTypes.SelectedIndex < 0)
                {
                    MessageBox.Show(rm.GetString("noSelectedMealType", culture));
                    return;
                }

                if (cbPoint.SelectedIndex < 0)
                {
                    MessageBox.Show(rm.GetString("noSelectedMealPoint", culture));
                    return;
                }

                if (numQty.Value <= 0)
                {
                    MessageBox.Show(rm.GetString("enterQty", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                string invalidDate = "";
                foreach (ListViewItem emplItem in lvEmployees.SelectedItems)
                {
                    EmployeeTO empl = (EmployeeTO)emplItem.Tag;

                    foreach (ListViewItem dateItem in lvDates.SelectedItems)
                    {
                        DateTime date = (DateTime)dateItem.Tag;
                        DateTime mealTime = new DateTime(date.Year, date.Month, date.Day, dtpTime.Value.Hour, dtpTime.Value.Minute, 0);

                        if (emplMealTimeDict.ContainsKey(empl.EmployeeID) && emplMealTimeDict[empl.EmployeeID].Contains(mealTime))
                            continue;

                        string error = validateMealTime(mealTime);

                        if (!error.Trim().Equals(""))
                        {
                            invalidDate = error;
                            continue;
                        }

                        OnlineMealsUsedTO meal = new OnlineMealsUsedTO();
                        meal.EmployeeID = empl.EmployeeID;
                        meal.EmployeeName = empl.FirstAndLastName;
                        if (ascoDict.ContainsKey(empl.EmployeeID))
                            meal.EmployeeStringone = ascoDict[empl.EmployeeID].NVarcharValue2.Trim();
                        if (ascoDict.ContainsKey(empl.EmployeeID))
                            meal.EmployeeLocation = ascoDict[empl.EmployeeID].NVarcharValue7.Trim();
                        meal.EventTime = mealTime;
                        meal.ManualCreated = (int)Constants.recordCreated.Manualy;
                        meal.MealTypeID = (int)cbMealTypes.SelectedValue;
                        meal.MealType = cbMealTypes.Text;
                        meal.OnlineValidation = (int)Constants.MealOnlineValidation.Valid;
                        meal.PointID = (int)cbPoint.SelectedValue;
                        meal.PointName = cbPoint.Text;
                        meal.Qty = (int)numQty.Value;

                        mealsList.Add(meal);

                        if (!emplMealTimeDict.ContainsKey(empl.EmployeeID))
                            emplMealTimeDict.Add(empl.EmployeeID, new List<DateTime>());
                        emplMealTimeDict[empl.EmployeeID].Add(mealTime);
                    }
                }

                populateMealUsed();

                if (!invalidDate.Trim().Equals(""))
                    MessageBox.Show(invalidDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedAdd.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvMealsUsed.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lvMealsUsed.SelectedItems)
                    {
                        OnlineMealsUsedTO meal = (OnlineMealsUsedTO)item.Tag;
                        mealsList.Remove(meal);

                        if (emplMealTimeDict.ContainsKey(meal.EmployeeID) && emplMealTimeDict[meal.EmployeeID].Contains(meal.EventTime))
                        {
                            emplMealTimeDict[meal.EmployeeID].Remove(meal.EventTime);

                            if (emplMealTimeDict[meal.EmployeeID].Count == 0)
                                emplMealTimeDict.Remove(meal.EmployeeID);
                        }
                    }

                    populateMealUsed();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedAdd.btnRemove_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (mealsList.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noMealsUsedToSave", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                OnlineMealsUsed mealUsed = new OnlineMealsUsed();

                if (mealUsed.BeginTransaction())
                {
                    bool saved = true;
                    try
                    {
                        foreach (OnlineMealsUsedTO meal in mealsList)
                        {
                            if (meal.Qty > 1)
                            {
                                meal.Approved = Constants.MealApproved;
                                meal.ApprovedBy = NotificationController.GetLogInUser().UserID;
                                meal.ApprovedTime = DateTime.Now;
                                meal.ApprovedDesc = Constants.MealApproved;
                            }

                            mealUsed.OnlineMealsUsedTO = meal;

                            saved = saved && (mealUsed.Save(false) > 0);

                            if (!saved)
                                break;
                        }

                        if (saved)
                        {
                            mealUsed.CommitTransaction();
                            MessageBox.Show(rm.GetString("mealsUsedSaved", culture));
                            mealsList.Clear();
                            emplMealTimeDict.Clear();
                            populateMealUsed();
                        }
                        else
                        {
                            if (mealUsed.GetTransaction() != null)
                                mealUsed.RollbackTransaction();
                            MessageBox.Show(rm.GetString("mealsUsedNotSaved", culture));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (mealUsed.GetTransaction() != null)
                            mealUsed.RollbackTransaction();
                        throw ex;
                    }
                }
                else
                    MessageBox.Show(rm.GetString("mealsUsedNotSaved", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvMealsUsed_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int prevOrder = mealSortOrder;

                if (e.Column == mealSortField)
                {
                    if (prevOrder == Constants.sortAsc)
                    {
                        mealSortOrder = Constants.sortDesc;
                    }
                    else
                    {
                        mealSortOrder = Constants.sortAsc;
                    }
                }
                else
                {
                    // New Sort Order
                    mealSortOrder = Constants.sortAsc;
                }

                mealSortField = e.Column;
                
                populateMealUsed();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedAdd.lvMealsUsed_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvEmployees_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvEmployees.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvEmployees.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvEmployees.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvEmployees.Sorting = SortOrder.Ascending;
                }

                lvEmployees.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedAdd.lvEmployees_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnBrowse_ClickExcel(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                OpenFileDialog fbDialog = new OpenFileDialog();
                fbDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                fbDialog.Filter = "XLS (*.xls)|*.xls|" +
                "XLSX (*.xlsx)|*.xlsx";
                fbDialog.Title = "Open file";

                if (fbDialog.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;

                    // make connection to selected XLS file
                    string path = fbDialog.FileName;
                    tbPath.Text = fbDialog.FileName;
                    
                    string excelConnectionString = "";
                    if (fbDialog.FilterIndex == xlsIndex)
                        excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0; HDR=YES;'";
                    else if (fbDialog.FilterIndex == xlsxIndex)
                        excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0 xml; HDR=YES;'";

                    OleDbConnection conn = new OleDbConnection(excelConnectionString);
                    conn.Open();

                    // Get all sheetnames from an excel file into data table
                    DataTable dbSchema = new System.Data.DataTable();
                    dbSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    List<string> sheetNames = new List<string>();
                    if (dbSchema != null && dbSchema.Rows.Count > 0)
                    {
                        // Loop through all worksheets
                        for (int i = 0; i < dbSchema.Rows.Count; i++)
                        {
                            sheetNames.Add(dbSchema.Rows[i]["TABLE_NAME"].ToString());
                        }
                    }

                    if (sheetNames.Count > 0)
                    {
                        // get data from first sheet
                        OleDbCommand sqlcom = new OleDbCommand("SELECT * FROM [" + sheetNames[0].Trim() + "]", conn);
                        OleDbDataAdapter da = new OleDbDataAdapter(sqlcom);
                        DataSet ds = new DataSet();
                        da.Fill(ds, "EmployeesMeals");
                        DataTable table = ds.Tables["EmployeesMeals"];
                        if (table.Rows.Count > 0)
                        {
                            List<DataRow> invalidRows = new List<DataRow>();
                            // make list of items from XLS rows                            
                            foreach (DataRow row in table.Rows)
                            {
                                if (row.ItemArray.Length >= massiveUploadColumns)
                                {
                                    int emplID = -1;
                                    int typeID = -1;
                                    int pointID = -1;
                                    int qty = -1;
                                    DateTime date = new DateTime();
                                    if (!int.TryParse(row.ItemArray[0].ToString().Trim(), out emplID))
                                        emplID = -1;                                    
                                    if (!DateTime.TryParseExact(row.ItemArray[3].ToString().Trim(), Constants.dateFormat + " " + Constants.timeFormat, null, DateTimeStyles.None, out date))
                                    {
                                        if (!DateTime.TryParseExact(row.ItemArray[3].ToString().Trim(), Constants.dateFormat, null, DateTimeStyles.None, out date))
                                            date = new DateTime();
                                    }
                                    if (!int.TryParse(row.ItemArray[4].ToString().Trim(), out typeID))
                                        typeID = -1;
                                    if (!int.TryParse(row.ItemArray[5].ToString().Trim(), out pointID))
                                        pointID = -1;
                                    if (!int.TryParse(row.ItemArray[6].ToString().Trim(), out qty))
                                        qty = -1;

                                    if (!emplDict.ContainsKey(emplID) || !mealsTypesDict.ContainsKey(typeID) || !mealsPointsDict.ContainsKey(pointID) || qty <= 0 || date.Equals(new DateTime())
                                        || (emplMealTimeDict.ContainsKey(emplID) && emplMealTimeDict[emplID].Contains(date)))
                                    {
                                        invalidRows.Add(row);
                                        continue;
                                    }

                                    OnlineMealsUsedTO meal = new OnlineMealsUsedTO();
                                    meal.EmployeeID = emplID;
                                    meal.EmployeeName = emplDict[emplID].FirstAndLastName;
                                    if (ascoDict.ContainsKey(emplID))
                                        meal.EmployeeStringone = ascoDict[emplID].NVarcharValue2.Trim();
                                    if (ascoDict.ContainsKey(emplID))
                                        meal.EmployeeLocation = ascoDict[emplID].NVarcharValue7.Trim();
                                    meal.EventTime = date;
                                    meal.ManualCreated = (int)Constants.recordCreated.Manualy;
                                    meal.MealTypeID = typeID;
                                    meal.MealType = mealsTypesDict[typeID].Name;
                                    meal.OnlineValidation = (int)Constants.MealOnlineValidation.Valid;
                                    meal.PointID = pointID;
                                    meal.PointName = mealsPointsDict[pointID].Name;
                                    meal.Qty = qty;

                                    mealsList.Add(meal);

                                    if (!emplMealTimeDict.ContainsKey(emplID))
                                        emplMealTimeDict.Add(emplID, new List<DateTime>());
                                    emplMealTimeDict[emplID].Add(date);
                                }
                            }

                            populateMealUsed();

                            if (invalidRows.Count > 0)                            
                                MessageBox.Show(rm.GetString("invalidDataMealsUsedMassiveInput", culture));
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("noDataInXLS", culture));
                        }
                    }

                    conn.Close();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedAdd.btnBrowse_Click(): " + ex.Message + "\n");
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

                    List<string> invalidRows = new List<string>();
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.FileStream str = System.IO.File.Open(path, System.IO.FileMode.Open);
                        System.IO.StreamReader reader = new System.IO.StreamReader(str);

                        // read file lines, skip header
                        string line = reader.ReadLine(); // header
                        line = reader.ReadLine(); // first row
                        while (line != null)
                        {
                            if (!line.Trim().Equals(""))
                            {                                
                                // make list of items from file rows
                                string[] rowItems = line.Split(delimiter);
                                if (rowItems.Length >= massiveUploadColumns)
                                {
                                    int emplID = -1;
                                    int typeID = -1;
                                    int pointID = -1;
                                    int qty = -1;
                                    DateTime date = new DateTime();
                                    if (!int.TryParse(rowItems[0].ToString().Trim(), out emplID))
                                        emplID = -1;
                                    if (!DateTime.TryParseExact(rowItems[3].ToString().Trim(), Constants.dateFormat + " " + Constants.timeFormat, null, DateTimeStyles.None, out date))
                                    {
                                        if (!DateTime.TryParseExact(rowItems[3].ToString().Trim(), Constants.dateFormat, null, DateTimeStyles.None, out date))
                                            date = new DateTime();
                                    }
                                    if (!int.TryParse(rowItems[4].ToString().Trim(), out typeID))
                                        typeID = -1;
                                    if (!int.TryParse(rowItems[5].ToString().Trim(), out pointID))
                                        pointID = -1;
                                    if (!int.TryParse(rowItems[6].ToString().Trim(), out qty))
                                        qty = -1;

                                    if (!emplDict.ContainsKey(emplID) || !mealsTypesDict.ContainsKey(typeID) || !mealsPointsDict.ContainsKey(pointID) || qty <= 0 || date.Equals(new DateTime())
                                        || (emplMealTimeDict.ContainsKey(emplID) && emplMealTimeDict[emplID].Contains(date)))
                                    {
                                        invalidRows.Add(line);
                                        line = reader.ReadLine();
                                        continue;
                                    }

                                    OnlineMealsUsedTO meal = new OnlineMealsUsedTO();
                                    meal.EmployeeID = emplID;
                                    meal.EmployeeName = emplDict[emplID].FirstAndLastName;
                                    if (ascoDict.ContainsKey(emplID))
                                        meal.EmployeeStringone = ascoDict[emplID].NVarcharValue2.Trim();
                                    if (ascoDict.ContainsKey(emplID))
                                        meal.EmployeeLocation = ascoDict[emplID].NVarcharValue7.Trim();
                                    meal.EventTime = date;
                                    meal.ManualCreated = (int)Constants.recordCreated.Manualy;
                                    meal.MealTypeID = typeID;
                                    meal.MealType = mealsTypesDict[typeID].Name;
                                    meal.OnlineValidation = (int)Constants.MealOnlineValidation.Valid;
                                    meal.PointID = pointID;
                                    meal.PointName = mealsPointsDict[pointID].Name;
                                    meal.Qty = qty;

                                    mealsList.Add(meal);

                                    if (!emplMealTimeDict.ContainsKey(emplID))
                                        emplMealTimeDict.Add(emplID, new List<DateTime>());
                                    emplMealTimeDict[emplID].Add(date);
                                }
                                else
                                    invalidRows.Add(line);
                            }

                            line = reader.ReadLine();
                        }

                        reader.Close();
                        str.Dispose();
                        str.Close();

                        populateMealUsed();

                        if (invalidRows.Count > 0)
                            MessageBox.Show(rm.GetString("invalidDataMealsUsedMassiveInput", culture));
                    }
                    else
                        return;                    
                }
                else                
                    return;                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsUsedAdd.btnBrowse_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private string validateMealTime(DateTime date)
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
                log.writeLog(DateTime.Now + " OnlineMealsUsedAdd.validateMealTime(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
