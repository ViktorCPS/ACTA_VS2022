using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TransferObjects;
using Util;
using System.Globalization;
using System.Resources;
using Common;

namespace UI
{
    public partial class OnlineMealReports : UserControl
    {

        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        const int colNum = 5;
        ApplUserTO logInUser;

        private List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
        private Dictionary<int, OrganizationalUnitTO> oUnits = new Dictionary<int, OrganizationalUnitTO>();
        private string wuString = "";
        private string ouString = "";
        private List<int> wuList = new List<int>();
        private List<int> ouList = new List<int>();

        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<int, int>>>> dataDictionary = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<int, int>>>>();
        private Dictionary<string, Dictionary<string, Dictionary<int, int>>> dataDictionaryDaily = new Dictionary<string, Dictionary<string, Dictionary<int, int>>>();

        //List<ApplRoleTO> currentRoles;
        //Hashtable menuItemsPermissions;

        //string menuItemID;
        //string menuItemUsedID;

        //bool addUsedPermission = false;
        //bool updateUsedPermission = false;
        //bool deleteUsedPermission = false;

        public OnlineMealReports()
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(MealsUsed).Assembly);

            setLanguage();

            tbFromTime.Text = "00:00";
            tbToTime.Text = "00:00";
            logInUser = NotificationController.GetLogInUser();

            dtpFrom.Value = DateTime.Now.Date.AddDays(-DateTime.Now.Day + 1);
            dtpTo.Value = DateTime.Now.Date;
        }
        private void setLanguage()
        {
            try
            {
                //gb text
                gbFilter.Text = rm.GetString("gbUnitFilter", culture);
                gbPeriod.Text = rm.GetString("rbPeriod", culture);

                //lblText
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblDate.Text = rm.GetString("lblDate", culture) + ":";


                //RB text
                rbOU.Text = rm.GetString("gbOrganizationalUnits", culture);
                rbWU.Text = rm.GetString("WUForm", culture);
                rbDaily.Text = rm.GetString("rbDaily", culture);
                rbPeriodical.Text = rm.GetString("rbCumulative", culture);

                //button text
                btnGenerate.Text = rm.GetString("btnGenerateReport", culture);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OnlineMealsReports.setLanguage(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsReports.populateOU(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsReports.populateWU(): " + ex.Message + "\n");
                throw ex;
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
                log.writeLog(DateTime.Now + " OnlineMealsReports.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " OnlineMealsReports.btnOUTreeView_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void OnlineMealReports_Load(object sender, EventArgs e)
        {
            try
            {

                //emplDict = new Employee().SearchDictionary();
                //ascoDict = new EmployeeAsco4().SearchDictionary("");
                //wuDict = new WorkingUnit().getWUDictionary();


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

                //menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                //currentRoles = NotificationController.GetCurrentRoles();

                //menuItemID = NotificationController.GetCurrentMenuItemID();
                //int index = menuItemID.LastIndexOf('_');

                //menuItemUsedID = menuItemID + "_" + rm.GetString("tpMealsUsed", culture);
                //setVisibility();


                populateWU();
                populateOU();


            }
            catch (Exception ex)
            {

                log.writeLog(DateTime.Now + " OnlineMealsReports.OnlineMealReports_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
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
        }

        private void rbDaily_CheckedChanged(object sender, EventArgs e)
        {

            if (rbDaily.Checked)
            {
                rbPeriodical.Checked = false;
                gbPeriod.Enabled = false;
                dtpDate.Enabled = true;

            }
            else
            {
                rbDaily.Checked = false;
                gbPeriod.Enabled = true;
                dtpDate.Enabled = false;

            }


        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string wu = "";
                string employees = "";
                List<EmployeeTO> emplArray = new List<EmployeeTO>();
                int wuID = -1;
                if (rbWU.Checked)
                {
                    wuID = (int)cbWU.SelectedValue;
                    string workUnitID = wuID.ToString();
                    if (wuID == -1)
                    {
                        MessageBox.Show(rm.GetString("lblWUNotSelected", culture));
                        return;
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
                                wu = workingUnit.Description;
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
                    wuID = (int)cbOU.SelectedValue;
                    string orgUnitID = wuID.ToString();
                    if (wuID == -1)
                    {
                        MessageBox.Show(rm.GetString("lblOUNotSelected", culture));
                        return;
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
                                wu = organizationalUnit.Desc;
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
                string emplIDs = "";
                if (emplArray.Count > 0)
                {
                    foreach (EmployeeTO empl in emplArray)
                    {
                        emplIDs += empl.EmployeeID + ",";

                    }
                    if (emplIDs.Length > 0)
                    {
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                    }
                }
                else
                {
                    if (rbWU.Checked)
                        MessageBox.Show(rm.GetString("noEmployeesForWU", culture));
                    else
                        MessageBox.Show(rm.GetString("noEmployeesForOU", culture));
                    return;
                }

                if (rbPeriodical.Checked)
                {
                    DateTime mealFrom = new DateTime();
                    DateTime mealTO = new DateTime();

                    string hourFrom = "";
                    string hourTo = "";
                    string minFrom = "";
                    string minTo = "";

                    string timeFrom = tbFromTime.Text;
                    string timeTo = tbToTime.Text;

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

                    List<OnlineMealsIntervalsTO> listIntervals = new OnlineMealsIntervals().SearchAll(Constants.MealReportTypeCumulative);

                    List<OnlineMealsUsedTO> listMeals = new OnlineMealsUsed().SearchMealUsed(emplIDs,mealFrom, mealTO, "", "", "", -1, -1, -1, -1, -1, "", "", "", "", new DateTime(), new DateTime());
                    if (listMeals.Count <= 0)
                    {
                        MessageBox.Show(rm.GetString("noReportData", culture));
                        return;
                    }
                    dataDictionary = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<int, int>>>>();
                    foreach (OnlineMealsUsedTO meal in listMeals)
                    {
                        if (!meal.Approved.Equals(Constants.MealDeleted))
                        {
                            string intervalString = "";
                            if (!dataDictionary.ContainsKey(meal.RestaurantName))
                                dataDictionary.Add(meal.RestaurantName, new Dictionary<string, Dictionary<string, Dictionary<int, int>>>());

                            if (!dataDictionary[meal.RestaurantName].ContainsKey(meal.PointName))
                                dataDictionary[meal.RestaurantName].Add(meal.PointName, new Dictionary<string, Dictionary<int, int>>());

                            foreach (OnlineMealsIntervalsTO interval in listIntervals)
                            {
                                string intervalTO = interval.Interval_start_time.ToString("HH.mm") + ":" + interval.Interval_end_time.ToString("HH.mm");
                                if (!dataDictionary[meal.RestaurantName][meal.PointName].ContainsKey(intervalTO))
                                    dataDictionary[meal.RestaurantName][meal.PointName].Add(intervalTO, new Dictionary<int, int>());

                                if (meal.EventTime.TimeOfDay >= interval.Interval_start_time.TimeOfDay && meal.EventTime.TimeOfDay <= interval.Interval_end_time.TimeOfDay)
                                {
                                    intervalString = intervalTO;
                                }
                                if (!dataDictionary[meal.RestaurantName][meal.PointName][intervalTO].ContainsKey(1))
                                    dataDictionary[meal.RestaurantName][meal.PointName][intervalTO].Add(1, 0);
                                if (!dataDictionary[meal.RestaurantName][meal.PointName][intervalTO].ContainsKey(0))
                                    dataDictionary[meal.RestaurantName][meal.PointName][intervalTO].Add(0, 0);
                            }

                            dataDictionary[meal.RestaurantName][meal.PointName][intervalString][meal.OnlineValidation]+=meal.Qty;
                        }
                    }

                    generateXLSReportCumulative(wuID, wu,mealFrom,mealTO);

                }
                else
                {
                    DateTime date = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, dtpDate.Value.Day, 0, 0, 0);

                    List<OnlineMealsIntervalsTO> listIntervals = new OnlineMealsIntervals().SearchAll(Constants.MealReportTypeDaily);
                    List<OnlineMealsUsedTO> listMeals = new OnlineMealsUsed().SearchMealUsed(emplIDs, date, date.AddDays(1), "", "", "", -1, -1, -1, -1, -1, "", "", "", "", new DateTime(), new DateTime());
                    if (listMeals.Count <= 0)
                    {
                        MessageBox.Show(rm.GetString("noReportDate", culture));
                        return;
                    }
                    dataDictionaryDaily = new Dictionary<string, Dictionary<string, Dictionary<int, int>>>();
                    foreach (OnlineMealsUsedTO meal in listMeals)
                    {
                        if (!meal.Approved.Equals(Constants.MealDeleted))
                        {
                            string intervalString = "";
                            if (!dataDictionaryDaily.ContainsKey(meal.RestaurantName))
                                dataDictionaryDaily.Add(meal.RestaurantName, new Dictionary<string, Dictionary<int, int>>());

                            foreach (OnlineMealsIntervalsTO interval in listIntervals)
                            {
                                string intervalTO = interval.Interval_start_time.ToString("HH.mm") + ":" + interval.Interval_end_time.ToString("HH.mm");


                                if (!dataDictionaryDaily[meal.RestaurantName].ContainsKey(intervalTO))
                                    dataDictionaryDaily[meal.RestaurantName].Add(intervalTO, new Dictionary<int, int>());
                                if (!dataDictionaryDaily[meal.RestaurantName][intervalTO].ContainsKey(1))
                                    dataDictionaryDaily[meal.RestaurantName][intervalTO].Add(1, 0);
                                if (!dataDictionaryDaily[meal.RestaurantName][intervalTO].ContainsKey(0))
                                    dataDictionaryDaily[meal.RestaurantName][intervalTO].Add(0, 0);

                                if (meal.CreatedTime.TimeOfDay >= interval.Interval_start_time.TimeOfDay && meal.CreatedTime.TimeOfDay <= interval.Interval_end_time.TimeOfDay)
                                {
                                    intervalString = intervalTO;

                                }
                            }
                            dataDictionaryDaily[meal.RestaurantName][intervalString][meal.OnlineValidation]+=meal.Qty;
                        }
                    }
                    generateXLSReportDaily(wuID, wu);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " OnlineMealsUsed.releaseObject(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {

                this.Cursor = Cursors.Arrow;
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

        private void generateXLSReportCumulative(int WUid, string workingUnit, DateTime from, DateTime to)
        {
            try
            {

                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);


                object misValue = System.Reflection.Missing.Value;

                Microsoft.Office.Interop.Excel.Worksheet wsAnomalies = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;

                wsAnomalies.Name = rm.GetString("repMeals", culture);
                wsAnomalies.Cells[1, 1] = rm.GetString("reportMealTitleCumulative", culture) + workingUnit;

                wsAnomalies.Cells[3, 2] = rm.GetString("period", culture);

                wsAnomalies.Cells[3, 3] = rm.GetString("lblUsersLogFrom", culture) + from.ToString("dd.MM.yyyy HH:mm");
                wsAnomalies.Cells[3, 4] = rm.GetString("lblUsersLogTo", culture) + to.ToString("dd.MM.yyyy HH:mm");

                wsAnomalies.Cells[5, 1] = rm.GetString("repRestReader", culture);
                wsAnomalies.Cells[5, 2] = rm.GetString("gbTimeInterval", culture);
                wsAnomalies.Cells[5, 3] = rm.GetString("hdrGreen", culture);
                wsAnomalies.Cells[5, 4] = rm.GetString("hdrRed", culture);
                wsAnomalies.Cells[5, 5] = rm.GetString("hdrTotal", culture);

                setRowFontWeight(wsAnomalies, 5, true);

                int i = 7;

                int totalRed = 0;
                int totalGreen = 0;
                foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, Dictionary<int, int>>>> pairRestaurant in dataDictionary)
                {
                    int restRed = 0;
                    int restGreen = 0;
                    foreach (KeyValuePair<string, Dictionary<string, Dictionary<int, int>>> pairPoint in dataDictionary[pairRestaurant.Key])
                    {
                        int red = 0;
                        int green = 0;
                        foreach (KeyValuePair<string, Dictionary<int, int>> pairInterval in dataDictionary[pairRestaurant.Key][pairPoint.Key])
                        {
                            wsAnomalies.Cells[i, 1] = pairPoint.Key;
                            wsAnomalies.Cells[i, 2] = pairInterval.Key;
                            foreach (KeyValuePair<int, int> pairValidation in dataDictionary[pairRestaurant.Key][pairPoint.Key][pairInterval.Key])
                            {
                                //red
                                if (pairValidation.Key == 0)
                                {
                                    wsAnomalies.Cells[i, 4] = pairValidation.Value;
                                    red += pairValidation.Value;
                                }
                                else
                                {
                                    wsAnomalies.Cells[i, 3] = pairValidation.Value;
                                    green += pairValidation.Value;
                                }
                            }
                            if (!dataDictionary[pairRestaurant.Key][pairPoint.Key][pairInterval.Key].ContainsKey(0))
                                wsAnomalies.Cells[i, 4] = 0;
                            if (!dataDictionary[pairRestaurant.Key][pairPoint.Key][pairInterval.Key].ContainsKey(1))
                                wsAnomalies.Cells[i, 3] = 0;

                            wsAnomalies.Cells[i, 5] = dataDictionary[pairRestaurant.Key][pairPoint.Key][pairInterval.Key][0] + dataDictionary[pairRestaurant.Key][pairPoint.Key][pairInterval.Key][1];
                            i++;

                        }
                        i++;

                        wsAnomalies.Cells[i, 2] = rm.GetString("hdrTotal", culture);

                        wsAnomalies.Cells[i, 3] = green;
                        wsAnomalies.Cells[i, 4] = red;
                        wsAnomalies.Cells[i, 5] = red + green;
                        restGreen += green;
                        restRed += red;
                        i++;
                    }
                    i++;

                    wsAnomalies.Cells[i, 2] = rm.GetString("hdrTotal", culture) + pairRestaurant.Key;

                    wsAnomalies.Cells[i, 3] = restGreen;
                    wsAnomalies.Cells[i, 4] = restRed;
                    wsAnomalies.Cells[i, 5] = restRed + restGreen;
                    totalGreen += restGreen;
                    totalRed += restRed;
                    i++;
                }
                wsAnomalies.Cells[i, 2] = rm.GetString("hdrTotal", culture);
                wsAnomalies.Cells[i, 3] = totalGreen;
                wsAnomalies.Cells[i, 4] = totalRed;
                wsAnomalies.Cells[i, 5] = totalGreen + totalRed;

                if (i == 3)
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }

                //wsAnomalies.Columns.AutoFit();
                wsAnomalies.Rows.AutoFit();


                string reportName = "MealUsedReport" + DateTime.Now.Date.ToString("yyyyMMddHHmmss") + WUid;

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


        private void generateXLSReportDaily(int WUID, string workingUnit)
        {
            try
            {

                CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

                Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);


                object misValue = System.Reflection.Missing.Value;

                Microsoft.Office.Interop.Excel.Worksheet wsAnomalies = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;

                wsAnomalies.Name = rm.GetString("repMeals", culture);
                wsAnomalies.Cells[1, 1] = rm.GetString("reportMealTitle", culture) + workingUnit;


                wsAnomalies.Cells[3, 2] = rm.GetString("repDay", culture);
                wsAnomalies.Cells[3, 3] = "" + dtpDate.Value.ToString("dd.MM.yyyy");


                wsAnomalies.Cells[5, 1] = rm.GetString("repRestReader", culture);
                wsAnomalies.Cells[5, 2] = rm.GetString("gbTimeInterval", culture);
                wsAnomalies.Cells[5, 3] = rm.GetString("hdrGreen", culture);
                wsAnomalies.Cells[5, 4] = rm.GetString("hdrRed", culture);
                wsAnomalies.Cells[5, 5] = rm.GetString("hdrTotal", culture);


                setRowFontWeight(wsAnomalies, 5, true);

                int i = 7;

                int totalRed = 0;
                int totalGreen = 0;
                foreach (KeyValuePair<string, Dictionary<string, Dictionary<int, int>>> pairRestaurant in dataDictionaryDaily)
                {
                    int restRed = 0;
                    int restGreen = 0;

                    foreach (KeyValuePair<string, Dictionary<int, int>> pairInterval in dataDictionaryDaily[pairRestaurant.Key])
                    {
                        wsAnomalies.Cells[i, 1] = pairRestaurant.Key;
                        wsAnomalies.Cells[i, 2] = pairInterval.Key;
                        foreach (KeyValuePair<int, int> pairValidation in dataDictionaryDaily[pairRestaurant.Key][pairInterval.Key])
                        {
                            //red
                            if (pairValidation.Key == 0)
                            {
                                wsAnomalies.Cells[i, 4] = pairValidation.Value;
                                restRed += pairValidation.Value;
                            }
                            else
                            {
                                wsAnomalies.Cells[i, 3] = pairValidation.Value;
                                restGreen += pairValidation.Value;
                            }
                        }
                        if (!dataDictionaryDaily[pairRestaurant.Key][pairInterval.Key].ContainsKey(0))
                            wsAnomalies.Cells[i, 4] = 0;
                        if (!dataDictionaryDaily[pairRestaurant.Key][pairInterval.Key].ContainsKey(1))
                            wsAnomalies.Cells[i, 3] = 0;

                        wsAnomalies.Cells[i, 5] = dataDictionaryDaily[pairRestaurant.Key][pairInterval.Key][0] + dataDictionaryDaily[pairRestaurant.Key][pairInterval.Key][1];
                        i++;

                    }

                    i++;
                    wsAnomalies.Cells[i, 2] = rm.GetString("hdrTotal", culture) + pairRestaurant.Key;
                    wsAnomalies.Cells[i, 3] = restGreen;
                    wsAnomalies.Cells[i, 4] = restRed;
                    wsAnomalies.Cells[i, 5] = restRed + restGreen;
                    totalGreen += restGreen;
                    totalRed += restRed;
                    i++;
                }
                wsAnomalies.Cells[i, 2] = rm.GetString("hdrTotal", culture);
                wsAnomalies.Cells[i, 3] = totalGreen;
                wsAnomalies.Cells[i, 4] = totalRed;
                wsAnomalies.Cells[i, 5] = totalGreen + totalRed;


                if (i == 3)
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }

                //wsAnomalies.Columns.AutoFit();
                wsAnomalies.Rows.AutoFit();


                string reportName = "MealUsedDailyReport" + DateTime.Now.Date.ToString("yyyyMMddHHmmss") + WUID;

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

    }
}
